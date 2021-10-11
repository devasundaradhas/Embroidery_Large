Imports System.Data
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO
Imports System.String
Imports Newtonsoft.Json
Imports System.Web
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports Microsoft.Reporting.WebForms
Imports System.Data.Sql

Imports System.Configuration


Public Class frmGSTR1

    Dim cn As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim Servername As String
    Dim Password As String
    Dim TransactionDataBase As String

    Private Sub frmGSTR1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        'Try

        '    Using SR As StreamReader = New StreamReader(System.Windows.Forms.Application.StartupPath & "\LOGINPARAMETERS.TXT")

        '        Dim LINE As String
        '        Dim LNNO As Integer = 1

        '        LINE = SR.ReadLine

        '        If LINE <> Nothing Then
        '            Servername = Authenticate.RevertAuthenticationCode(LINE)
        '        End If


        '        While LINE <> Nothing

        '            If LNNO = 2 Then
        '                Password = Authenticate.RevertAuthenticationCode(LINE)
        '            End If

        '            If LNNO = 3 Then
        '                TransactionDataBase = Authenticate.RevertAuthenticationCode(LINE)
        '            End If

        '            LINE = SR.ReadLine
        '            LNNO = LNNO + 1

        '        End While

        '    End Using

        'Catch ex As Exception

        '    MsgBox(ex.Message)

        'End Try


        'Cn.ConnectionString = "INITIAL CATALOG = GSTR;Data Source=" & Servername & _
        '                    ";User Id=SA;Password=" & Password

        cn.Open()

        lblAlert.Visible = False

        Me.MdiParent = MDIParent1

        Me.Top = 40
        Me.Left = 0

        Dim I As Integer

        For I = 2017 To 2100
            cboYear.Items.Add(I.ToString)
        Next


        If Not Cn Is Nothing Then

            Try

                Dim DAdapt As New SqlDataAdapter
                Dim CMD As New SqlCommand
                Dim DSet As New DataSet


                DAdapt.SelectCommand = CMD
                CMD.Connection = Cn

                If Len(cboGSTIN.Text) = 0 Then
                    CMD.CommandText = "SELECT COMPANY_NAME FROM  COMPANY_HEAD"
                Else
                    CMD.CommandText = "SELECT COMPANY_NAME FROM  COMPANY_HEAD WHERE COMPANY_GSTIN = '" & cboGSTIN.Text & "'"
                End If

                DAdapt.Fill(DSet, "COMPANY_HEAD")


                cboACName.DataSource = DSet.Tables("COMPANY_HEAD")
                cboACName.DisplayMember = "COMPANY_NAME"

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If

        DGV_B2B.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        DGV_B2CL.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        DGV_B2CS.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        DGV_HSN.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)

        Me.BringToFront()

    End Sub

    Private Sub txtRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click

        GenerateGSTR1Info()

        Exit Sub


    End Sub

    Private Sub cboACName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboACName.GotFocus

        If Not Cn Is Nothing Then

            Dim DAdapt As New SqlDataAdapter
            Dim CMD As New SqlCommand
            Dim DSet As New DataSet


            DAdapt.SelectCommand = CMD
            CMD.Connection = Cn

            If Len(cboGSTIN.Text) = 0 Then
                CMD.CommandText = "SELECT COMPANY_NAME FROM COMPANY_HEAD"
            Else
                CMD.CommandText = "SELECT COMPANY_NAME FROM COMPANY_HEAD WHERE COMPANY_GSTINNo = '" & cboGSTIN.Text & "'"
            End If

            DAdapt.Fill(DSet, "COMPANY_HEAD")

            cboACName.DataSource = DSet.Tables("COMPANY_HEAD")
            cboACName.DisplayMember = "COMPANY_NAME"

        End If

    End Sub



    Private Sub cboGSTIN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGSTIN.GotFocus

        Dim DAdapt As New SqlDataAdapter
        Dim CMD As New SqlCommand
        Dim DSet As New DataSet

        DAdapt.SelectCommand = CMD
        CMD.Connection = cn

        If Len(cboACName.Text) = 0 Then
            CMD.CommandText = "SELECT COMPANY_GSTINNo FROM COMPANY_HEAD "
        Else
            CMD.CommandText = "SELECT COMPANY_GSTINNo FROM COMPANY_HEAD WHERE COMPANY_NAME = '" & cboACName.Text & "'"
        End If

        DAdapt.Fill(DSet, "COMPANY_HEAD")

        cboGSTIN.DataSource = DSet.Tables("COMPANY_HEAD")
        cboGSTIN.DisplayMember = "COMPANY_GSTINNo"

    End Sub

    Private Sub cboGSTIN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGSTIN.SelectedIndexChanged

    End Sub

    Private Function StartDate() As String

        StartDate = "1/Jan/2000"
        If Len(cboYear.Text) = 0 Then
            MsgBox("Invalid Year")
            Exit Function
        End If

        If Len(cboMonth.Text) = 0 Then
            MsgBox("Invalid Year")
            Exit Function
        End If


        StartDate = "1-" & Microsoft.VisualBasic.Left((Microsoft.VisualBasic.Trim(Microsoft.VisualBasic.Split(cboMonth.Text)(0))), 3) & "-" & cboYear.Text


    End Function

    Private Function EndDate() As String

        EndDate = "31-Dec-9999"

        If StartDate() = "1-Jan-2000" Then
            Exit Function
        End If

        ' Dim MON() As String = Microsoft.VisualBasic.Split(cboMonth.Text, "-")

        If UBound(Microsoft.VisualBasic.Split(cboMonth.Text, "-")) > 0 Then

            EndDate = DateAdd(DateInterval.Month, 3, CDate(StartDate())).ToString
            EndDate = DateAdd(DateInterval.Day, -1, CDate(EndDate))
            Dim MON() As String = Microsoft.VisualBasic.Split(cboMonth.Text, "-")
            EndDate = Microsoft.VisualBasic.Split(EndDate, "/")(0) + "-" + Microsoft.VisualBasic.Trim(MON(1)) + "-" + cboYear.Text

        Else

            EndDate = DateAdd(DateInterval.Month, 1, CDate(StartDate())).ToString
            EndDate = DateAdd(DateInterval.Day, -1, CDate(EndDate))
            EndDate = Microsoft.VisualBasic.Split(EndDate, "/")(0) + "-" + Microsoft.VisualBasic.Left(cboMonth.Text, 3) + "-" + cboYear.Text

        End If

    End Function

    Private Sub lblGSTIN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblGSTIN.Click

    End Sub

    Private Sub lblACName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblACName.Click

    End Sub

    Private Sub cboMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMonth.SelectedIndexChanged

    End Sub

    Private Sub cboYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboYear.SelectedIndexChanged

    End Sub

    Private Sub lblMonth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMonth.Click

    End Sub

    Private Sub lblYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblYear.Click

    End Sub

    Private Sub TAB_GSTR1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TAB_GSTR1.SelectedIndexChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub DGV_HSN_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_HSN.CellContentClick

    End Sub

    Private Sub HSN_SUMMARY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HSN_SUMMARY.Click

    End Sub

    Private Sub DGV_B2CS_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_B2CS.CellContentClick

    End Sub

    Private Sub B2CS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B2CS.Click

    End Sub

    Private Sub DGV_B2CL_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_B2CL.CellContentClick

    End Sub

    Private Sub B2CL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B2CL.Click

    End Sub

    Private Sub DGV_B2B_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub B2B_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B2B.Click

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        GenerateGSTR1Info()
        ExportGSTR1toExcel()

        Exit Sub

    End Sub

    Public Sub GenerateGSTR1Info()

        If Len(Microsoft.VisualBasic.Trim(cboGSTIN.Text)) = 0 Then
            MsgBox("SELECT A VALID GSTIN")
            Exit Sub
        End If

        If Len(cboYear.Text) = 0 Or Len(cboMonth.Text) = 0 Then
            MsgBox("Invalid Year/Month Selection")
            Exit Sub
        End If

        'Dim Cn As New SqlConnection

        Dim DAdapt As New SqlDataAdapter
        Dim CMD As New SqlCommand
        Dim DSet As New DataSet
        Dim Cmp_IdNo As String

        DAdapt.SelectCommand = CMD

        Dim TX As SqlTransaction

        CMD.Connection = cn

        TX = Cn.BeginTransaction

        'Try

        CMD.Transaction = TX

        CMD.Connection = Cn

        CMD.CommandText = "SELECT COUNT(COMPANY_GSTINNo),MIN(COMPANY_IDNo) FROM COMPANY_HEAD " &
                                     " WHERE COMPANY_GSTINNo IN ('" & cboGSTIN.Text & "')"
        DAdapt.Fill(DSet, "ACCOUNT_MASTER_GSTIN")

        If DSet.Tables("ACCOUNT_MASTER_GSTIN").Rows(0).Item(0) <= 0 Then
            MsgBox("INVALID GSTIN ENTERED. ENTER A CORRECT GSTIN")
            Exit Sub
        Else
            If Not IsDBNull(DSet.Tables(0).Rows(0).Item(1)) Then
                Cmp_IdNo = DSet.Tables(0).Rows(0).Item(1)
            End If
        End If

        CMD.CommandText = "DELETE FROM GSTR1_B2B "
        CMD.ExecuteNonQuery()

        CMD.CommandText = "DELETE FROM GSTR1_B2CL "
        CMD.ExecuteNonQuery()

        CMD.CommandText = "DELETE FROM GSTR1_B2CS "
        CMD.ExecuteNonQuery()

        CMD.CommandText = "DELETE FROM GSTR1_CDNR "
        CMD.ExecuteNonQuery()

        CMD.CommandText = "DELETE FROM GSTR1_CDNUR "
        CMD.ExecuteNonQuery()

        CMD.CommandText = "DELETE FROM GSTR1_ADV"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "DELETE FROM GSTR1_HSN "
        CMD.ExecuteNonQuery()

        CMD.CommandText = "DELETE FROM GSTR1_DOC"
        CMD.ExecuteNonQuery()


        If cboACName.Text = "HARISH TRADERS" Then

            CMD.CommandText = " INSERT INTO GSTR1_B2B SELECT PM.GSTIN AS GSTIN, SH.B_NO AS INVNO,SH.B_DATE AS INVDATE,SH.NETT_AMT AS INVVALUE," &
                              " CONVERT(VARCHAR,PM.STATECODE)+'-'+PM.STATE,'N','Regular',''," &
                              " ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0) AS GSTRATE,SUM(SD.VALUE) AS TAXABLE_AMOUNT," &
                              " 0,'" &
                              cboGSTIN.Text & "' FROM " &
                              " TH_BILL_GST SH INNER JOIN TH_BILL_DETAILS_GST SD ON SH.B_NO = SD.B_NO INNER JOIN PARTY PM ON SH.PARTY_ID = PM.CMP_ID" &
                              " WHERE ( LEN(LTRIM(RTRIM(PM.GSTIN))) = 15 AND  SH.B_DATE >= (SELECT ISNULL(GSTIN_DATE,'1-JUL-2017') FROM PARTY WHERE CMP_ID = SH.PARTY_ID)) " &
                              " AND SH.B_DATE >= '" & StartDate() & "' AND SH.B_DATE <= '" & EndDate() & "' GROUP BY  " &
                              " PM.GSTIN , SH.B_NO,SH.B_DATE,SH.NETT_AMT ," &
                              " CONVERT(VARCHAR,PM.STATECODE)+'-'+PM.STATE," &
                              " ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0)"

        Else

            CMD.CommandText = " INSERT INTO GSTR1_B2B SELECT LH.LEDGER_GSTINNo AS GSTIN,LH.LEDGER_NAME AS PARTYNAME, SH.SALES_NO AS INVNO,SH.SALES_DATE AS INVDATE,SH.ASSESSABLE_VALUE+SH.IGST_AMOUNT+SH.CGST_AMOUNT+SH.SGST_AMOUNT AS INVVALUE," &
                              " CONVERT(VARCHAR,STH.STATE_CODE)+'-'+STH.STATE_NAME,CONVERT(VARCHAR,ISNULL(SH.REVERSE_CHARGE,0)),0,'Regular',''," &
                              " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) AS GSTRATE,SUM(GSTD.TAXABLE_AMOUNT) AS TAXABLE_AMOUNT," &
                              " 0,'" &
                              cboGSTIN.Text & "' FROM " &
                              " SALES_HEAD SH INNER JOIN Sales_GST_Tax_Details GSTD ON SH.SALES_CODE = GSTD.SALES_CODE INNER JOIN LEDGER_HEAD LH ON SH.LEDGER_IDNO = LH.LEDGER_IDNO" &
                              " INNER JOIN STATE_HEAD STH ON STH.STATE_IDNO = LH.STATE_IDNO " &
                              " WHERE LEN(LTRIM(RTRIM(LH.LEDGER_GSTINNo))) = 15 AND SH.SALES_DATE >= '" & StartDate() & "' AND SH.SALES_DATE <= '" & EndDate() & "'  AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString & " GROUP BY  " &
                              " LH.LEDGER_GSTINNo , SH.SALES_NO,SH.SALES_DATE,SH.ASSESSABLE_VALUE+SH.IGST_AMOUNT+SH.CGST_AMOUNT+SH.SGST_AMOUNT,CONVERT(VARCHAR,STH.STATE_CODE) +'-'+STH.STATE_NAME," &
                              " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0),LH.LEDGER_NAME,SH.REVERSE_CHARGE"

        End If

        CMD.ExecuteNonQuery()

        CMD.CommandText = " UPDATE GSTR1_B2B SET REVERSE_CHARGE = 'N' WHERE REVERSE_CHARGE = '0'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = " UPDATE GSTR1_B2B SET REVERSE_CHARGE = 'Y' WHERE REVERSE_CHARGE = '1'"
        CMD.ExecuteNonQuery()

        If cboACName.Text = "HARISH TRADERS" Then

            CMD.CommandText = " INSERT INTO GSTR1_B2CL SELECT SH.B_NO AS INVNO,SH.B_DATE AS INVDATE,SH.NETT_AMT AS INVVALUE,CONVERT(VARCHAR,PM.STATECODE)+'-'+PM.STATE," &
                             " ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0) AS GSTRATE,SUM(SD.VALUE) AS TAXABLE_AMOUNT," &
                             " 0,'','" & cboGSTIN.Text & "' FROM " &
                             " TH_BILL_GST SH INNER JOIN TH_BILL_DETAILS_GST SD ON SH.B_NO = SD.B_NO INNER JOIN PARTY PM ON SH.PARTY_ID = PM.CMP_ID" &
                             " WHERE ( NOT LEN(LTRIM(RTRIM(PM.GSTIN))) = 15 OR  SH.B_DATE < (SELECT ISNULL(GSTIN_DATE,'31-DEC-2049') FROM PARTY WHERE CMP_ID = SH.PARTY_ID)) " &
                             " AND SH.TOTAL_AMT > 250000.00  AND SH.B_DATE >= '" & StartDate() & "' AND SH.B_DATE <= '" & EndDate() & "' GROUP BY  " &
                             " CONVERT(VARCHAR,PM.STATECODE)+'-'+PM.STATE, SH.B_NO,SH.B_DATE,SH.NETT_AMT," &
                             " ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0)"

        Else

            CMD.CommandText = " INSERT INTO GSTR1_B2CL SELECT SH.SALES_NO AS INVNO,SH.SALES_DATE AS INVDATE,SH.ASSESSABLE_VALUE+SH.IGST_AMOUNT+SH.CGST_AMOUNT+SH.SGST_AMOUNT AS INVVALUE,CONVERT(VARCHAR,STH.STATE_IDNO)+'-'+STH.STATE_NAME,0," &
                              " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) AS GSTRATE," &
                              " SUM(TAXABLE_AMOUNT) AS TAXABLE_AMT ,0,'','" & cboGSTIN.Text & "' FROM " &
                              " SALES_HEAD SH INNER JOIN Sales_GST_Tax_Details GSTD ON SH.SALES_CODE = GSTD.SALES_CODE INNER JOIN LEDGER_HEAD LH ON SH.LEDGER_IDNO = LH.LEDGER_IDNO " &
                              " INNER JOIN STATE_HEAD STH ON STH.STATE_IDNO = LH.STATE_IDNO " &
                              " WHERE   LEN(LTRIM(RTRIM(LH.LEDGER_GSTINNo))) = 15  AND GSTD.TAXABLE_AMOUNT > 250000  AND SH.SALES_DATE >= '" & StartDate() & "' AND SH.SALES_DATE <= '" & EndDate() & "'  AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString & "GROUP BY " &
                              " SH.SALES_NO ,SH.SALES_DATE,SH.ASSESSABLE_VALUE+SH.IGST_AMOUNT+SH.CGST_AMOUNT+SH.SGST_AMOUNT, CONVERT(VARCHAR,STH.STATE_IDNO)+'-'+STH.STATE_NAME, " &
                              " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) "


        End If

        CMD.ExecuteNonQuery()

        If cboACName.Text = "HARISH TRADERS" Then

            CMD.CommandText = " INSERT INTO GSTR1_B2CS SELECT 'OE',CONVERT(VARCHAR,PM.STATECODE)+'-'+PM.STATE," &
                              " ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0) AS GSTRATE," &
                              " SUM(SD.VALUE) AS TAXABLE_AMOUNT,0,'','" & cboGSTIN.Text & "' FROM " &
                              " TH_BILL_GST SH INNER JOIN TH_BILL_DETAILS_GST SD ON SH.B_NO = SD.B_NO INNER JOIN PARTY PM ON SH.PARTY_ID = PM.CMP_ID" &
                              " WHERE ( NOT LEN(LTRIM(RTRIM(PM.GSTIN))) = 15 OR  SH.B_DATE < (SELECT ISNULL(GSTIN_DATE,'31-DEC-2049') FROM PARTY WHERE CMP_ID = SH.PARTY_ID)) " &
                              " AND SH.TOTAL_AMT < 250000.01  AND SH.B_DATE >= '" & StartDate() & "' AND SH.B_DATE <= '" & EndDate() & "' GROUP BY  " &
                              " CONVERT(VARCHAR,PM.STATECODE)+'-'+PM.STATE, " &
                              " ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0)"

        Else

            CMD.CommandText = " INSERT INTO GSTR1_B2CS SELECT 'OE',CONVERT(VARCHAR,STH.STATE_CODE)+'-'+STH.STATE_NAME,0," &
                             " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) AS GSTRATE," &
                             " SUM(GSTD.TAXABLE_AMOUNT) AS TAXABLE_AMT ,0,'','" & cboGSTIN.Text & "' FROM " &
                             " SALES_HEAD SH INNER JOIN Sales_GST_Tax_Details GSTD ON SH.SALES_CODE = GSTD.SALES_CODE INNER JOIN LEDGER_HEAD LH ON SH.LEDGER_IDNO = LH.LEDGER_IDNO " &
                             " INNER JOIN STATE_HEAD STH ON STH.STATE_IDNO = LH.STATE_IDNO " &
                             " WHERE  (LH.LEDGER_GSTINNo IS NULL OR LEN(LTRIM(RTRIM(LH.LEDGER_GSTINNo))) = 0) AND GSTD.TAXABLE_AMOUNT < 250001.01  AND SH.SALES_DATE >= '" & StartDate() & "' AND SH.SALES_DATE <= '" & EndDate() & "'  AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString & " GROUP BY " &
                             " SH.SALES_NO ,SH.SALES_DATE,SH.NET_AMOUNT,CONVERT(VARCHAR,STH.STATE_CODE)+'-'+STH.STATE_NAME, " &
                             " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) "

        End If


        CMD.ExecuteNonQuery()


        If cboACName.Text = "HARISH TRADERS" Then
        ElseIf cboACName.Text = "HARISH EMBROIDERY" Then
        Else
            CMD.CommandText = " INSERT INTO GSTR1_CDNR SELECT LH.LEDGER_GSTINNo,LH.LEDGER_NAME,SH.BILL_NO  ,SH.BILL_DATE ,SH.OTHER_GST_ENTRY_REFERENCE_CODE,SH.OTHER_GST_ENTRY_DATE,LEFT(SH.OTHER_GST_ENTRY_TYPE,1)," &
                              " CONVERT(VARCHAR,STH.STATE_IDNO)+'-'+STH.STATE_NAME,SH.NET_AMOUNT,0," &
                              " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0)  ," &
                              " SUM(TAXABLE_AMOUNT)   ,0,'N','" & cboGSTIN.Text & "' FROM " &
                              " Other_GST_Entry_Head SH INNER JOIN Other_GST_Entry_Tax_Details GSTD ON SH.OTHER_GST_ENTRY_REFERENCE_CODE = GSTD.OTHER_GST_ENTRY_REFERENCE_CODE AND SH.OTHER_GST_ENTRY_TYPE IN ('CRNT','DRNT') INNER JOIN LEDGER_HEAD LH ON SH.LEDGER_IDNO = LH.LEDGER_IDNO " &
                              " INNER JOIN STATE_HEAD STH ON STH.STATE_IDNO = LH.STATE_IDNO " &
                              " WHERE   LEN(LTRIM(RTRIM(LH.LEDGER_GSTINNo))) = 15   AND SH.OTHER_GST_ENTRY_DATE >= '" & StartDate() & "' AND SH.OTHER_GST_ENTRY_DATE <= '" & EndDate() & "'  AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString & " GROUP BY " &
                              " LEDGER_GSTINNo,LH.LEDGER_NAME,SH.BILL_NO ,SH.BILL_DATE,SH.OTHER_GST_ENTRY_REFERENCE_CODE,SH.OTHER_GST_ENTRY_DATE,LEFT(SH.OTHER_GST_ENTRY_TYPE,1)," &
                              "  CONVERT(VARCHAR,STH.STATE_IDNO)+'-'+STH.STATE_NAME,SH.NET_AMOUNT," &
                              " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) "
        End If

        CMD.ExecuteNonQuery()

        '--------------------

        'If cboACName.Text = "HARISH TRADERS" Then
        'ElseIf cboACName.Text = "HARISH EMBROIDERY" Then
        'Else
        '    '
        '    CMD.CommandText = " INSERT INTO GSTR1_CDNUR SELECT UNREGISTER_TYPE,LH.LEDGER_NAME,SH.OTHER_GST_ENTRY_REFERENCE_CODE,SH.OTHER_GST_ENTRY_DATE,LEFT(SH.OTHER_GST_ENTRY_TYPE,1)," &
        '                      " SH.BILL_NO  ,SH.BILL_DATE , CONVERT(VARCHAR,STH.STATE_IDNO)+'-'+STH.STATE_NAME,SH.NET_AMOUNT,0," &
        '                      " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0)  ," &
        '                      " SUM(TAXABLE_AMOUNT)   ,0,'N','" & cboGSTIN.Text & "' FROM " &
        '                      " Other_GST_Entry_Head SH INNER JOIN Other_GST_Entry_Tax_Details GSTD ON SH.OTHER_GST_ENTRY_REFERENCE_CODE = GSTD.OTHER_GST_ENTRY_REFERENCE_CODE  AND SH.OTHER_GST_ENTRY_TYPE IN ('CRNT','DRNT') INNER JOIN LEDGER_HEAD LH ON SH.LEDGER_IDNO = LH.LEDGER_IDNO " &
        '                      " INNER JOIN STATE_HEAD STH ON STH.STATE_IDNO = LH.STATE_IDNO " &
        '                      " WHERE   (LEN(LTRIM(RTRIM(LH.LEDGER_GSTINNo))) <> 15 OR LH.LEDGER_GSTINNo IS NULL)   AND SH.OTHER_GST_ENTRY_DATE >= '" & StartDate() & "' AND SH.OTHER_GST_ENTRY_DATE <= '" & EndDate() & "'  AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString & " GROUP BY " &
        '                      " UNREGISTER_TYPE,LH.LEDGER_NAME,SH.BILL_NO ,SH.BILL_DATE,SH.OTHER_GST_ENTRY_REFERENCE_CODE,SH.OTHER_GST_ENTRY_DATE,LEFT(SH.OTHER_GST_ENTRY_TYPE,1)," &
        '                      "  CONVERT(VARCHAR,STH.STATE_IDNO)+'-'+STH.STATE_NAME,SH.NET_AMOUNT," &
        '                      " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) "
        'End If

        'CMD.ExecuteNonQuery()

        '--------------------

        'If cboACName.Text = "HARISH TRADERS" Then
        'ElseIf cboACName.Text = "HARISH EMBROIDERY" Then
        'Else
        '    '
        '    CMD.CommandText = " INSERT INTO GSTR1_ADV SELECT  CONVERT(VARCHAR,STH.STATE_IDNO)+'-'+STH.STATE_NAME,0," &
        '                      " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0)  ," &
        '                      " SUM(TAXABLE_AMOUNT)   ,0,'" & cboGSTIN.Text & "' FROM " &
        '                      " Other_GST_Entry_Head SH INNER JOIN Other_GST_Entry_Tax_Details GSTD ON SH.OTHER_GST_ENTRY_REFERENCE_CODE = GSTD.OTHER_GST_ENTRY_REFERENCE_CODE  AND SH.OTHER_GST_ENTRY_TYPE IN ('ADV.RECT') INNER JOIN LEDGER_HEAD LH ON SH.LEDGER_IDNO = LH.LEDGER_IDNO " &
        '                      " INNER JOIN STATE_HEAD STH ON STH.STATE_IDNO = LH.STATE_IDNO " &
        '                      " WHERE   SH.OTHER_GST_ENTRY_DATE >= '" & StartDate() & "' AND SH.OTHER_GST_ENTRY_DATE <= '" & EndDate() & "'  AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString & " GROUP BY " &
        '                      " UNREGISTER_TYPE,LH.LEDGER_NAME,SH.BILL_NO ,SH.BILL_DATE,SH.OTHER_GST_ENTRY_REFERENCE_CODE,SH.OTHER_GST_ENTRY_DATE,LEFT(SH.OTHER_GST_ENTRY_TYPE,1)," &
        '                      " CONVERT(VARCHAR,STH.STATE_IDNO)+'-'+STH.STATE_NAME , ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) "
        'End If

        'CMD.ExecuteNonQuery()

        CMD.CommandText = "TRUNCATE TABLE SALES_REG_HSN"
        CMD.ExecuteNonQuery()

        If cboACName.Text = "HARISH TRADERS" Then

            CMD.CommandText = "INSERT INTO SALES_REG_HSN SELECT SH.B_NO,SH.HSN_CODE,'TUB-TUBES',SUM(SD.QUANTITY),SUM(VALUE),SH.IGST_RATE,SH.CGST_RATE,SH.SGST_RATE,0,0,0 " &
                              " FROM  TH_BILL_GST SH INNER JOIN TH_BILL_Details_GST SD ON SH.B_NO = SD.B_NO " &
                               " WHERE SH.B_DATE >= '" & StartDate() & "' AND SH.B_DATE <= '" & EndDate() & "'  GROUP BY SH.B_NO,SH.HSN_CODE,SH.IGST_RATE,SH.CGST_RATE,SH.SGST_RATE"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET IGST_AMOUNT  = (TAXABLE_AMOUNT * IGST_RATE /100) WHERE IGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET CGST_AMOUNT  = (TAXABLE_AMOUNT * CGST_RATE /100) WHERE CGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET SGST_AMOUNT  = (TAXABLE_AMOUNT * SGST_RATE /100) WHERE SGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "INSERT INTO GSTR1_HSN SELECT SH.HSN_CODE,'','TUB-TUBES',SUM(SD.QUANTITY),SUM(SD.VALUE)+(SUM(SD.VALUE) * (ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0)) /100), " &
                              "SUM(SD.VALUE)," &
                              "0,0,0,0,'" & cboGSTIN.Text & "',ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0) " &
                              " FROM  TH_BILL_GST SH INNER JOIN TH_BILL_Details_GST SD ON SH.B_NO = SD.B_NO " &
                              " AND SH.B_DATE >= '" & StartDate() & "' AND SH.B_DATE <= '" & EndDate() & "'  GROUP BY SH.HSN_CODE,ISNULL(SH.IGST_RATE,0)+ISNULL(SH.CGST_RATE,0)+ISNULL(SH.SGST_RATE,0) "


            CMD.ExecuteNonQuery()

        Else

            CMD.CommandText = "INSERT INTO SALES_REG_HSN SELECT SH.SALES_CODE,SD.HSN_CODE,UH.UNIT_NAME,SUM(SD.NOOF_ITEMS),SUM(SD.ASSESSABLE_VALUE),0,0,0,0,0,0 " &
                              " FROM  SALES_HEAD SH INNER JOIN SALES_Details SD ON SH.SALES_CODE = SD.SALES_CODE " &
                              " INNER JOIN UNIT_HEAD UH ON SD.UNIT_IDNO = UH.UNIT_IDNO " &
                              " AND SH.SALES_DATE >= '" & StartDate() & "' AND SH.SALES_DATE <= '" & EndDate() & "' AND SH.LEDGER_IDNO IN (SELECT LEDGER_IDNO FROM LEDGER_HEAD " &
                              " WHERE LEN(LEDGER_GSTINNo) =15)   AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString & " GROUP BY SD.HSN_CODE,UH.UNIT_NAME,SH.SALES_CODE "
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET IGST_RATE = (SELECT IGST_PERCENTAGE FROM SALES_GST_Tax_Details WHERE SALES_CODE = SALES_REG_HSN.SALES_CODE " &
                          " AND HSN_CODE = SALES_REG_HSN.HSN_CODE)"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET CGST_RATE = (SELECT CGST_PERCENTAGE FROM SALES_GST_Tax_Details WHERE SALES_CODE = SALES_REG_HSN.SALES_CODE " &
                              " AND HSN_CODE = SALES_REG_HSN.HSN_CODE)"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET SGST_RATE = (SELECT SGST_PERCENTAGE FROM SALES_GST_Tax_Details WHERE SALES_CODE = SALES_REG_HSN.SALES_CODE " &
                              " AND HSN_CODE = SALES_REG_HSN.HSN_CODE)"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET IGST_AMOUNT  = (TAXABLE_AMOUNT * IGST_RATE /100) WHERE IGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET CGST_AMOUNT  = (TAXABLE_AMOUNT * CGST_RATE /100) WHERE CGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_REG_HSN SET SGST_AMOUNT  = (TAXABLE_AMOUNT * SGST_RATE /100) WHERE SGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "INSERT INTO GSTR1_HSN SELECT SD.HSN_CODE,'',UH.UNIT_NAME,SUM(SD.NOOF_ITEMS),SUM(SD.ASSESSABLE_VALUE)+(SUM(SD.ASSESSABLE_VALUE) * SD.GST_PERCENTAGE /100), " &
                              "SUM(SD.ASSESSABLE_VALUE)," &
                              "0,0,0,0,'" & cboGSTIN.Text & "',SD.GST_PERCENTAGE,'" & Common_Procedures.FnRange & "' " &
                              " FROM  SALES_HEAD SH INNER JOIN SALES_Details SD ON SH.SALES_CODE = SD.SALES_CODE " &
                              " INNER JOIN UNIT_HEAD UH ON SD.UNIT_IDNO = UH.UNIT_IDNO " &
                              " AND SH.SALES_DATE >= '" & StartDate() & "' AND SH.SALES_DATE <= '" & EndDate() & "' AND SH.LEDGER_IDNO IN (SELECT LEDGER_IDNO FROM LEDGER_HEAD " &
                              " WHERE LEN(LEDGER_GSTINNo) =15)   AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString & " GROUP BY SD.HSN_CODE,UH.UNIT_NAME,SD.GST_PERCENTAGE"

            '

            CMD.ExecuteNonQuery()

        End If

        CMD.CommandText = "UPDATE GSTR1_HSN SET IGST = (SELECT SUM(IGST_AMOUNT) FROM SALES_REG_HSN WHERE HSN_CODE =  GSTR1_HSN.HSN_CODE AND " &
                          " UQC = GSTR1_HSN.UQC AND ISNULL(IGST_RATE,0) + ISNULL(CGST_RATE,0) +ISNULL(SGST_RATE,0) = GSTR1_HSN.GST_RATE)"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "UPDATE GSTR1_HSN SET CGST = (SELECT SUM(CGST_AMOUNT) FROM SALES_REG_HSN WHERE HSN_CODE =  GSTR1_HSN.HSN_CODE AND " &
                          " UQC = GSTR1_HSN.UQC AND ISNULL(IGST_RATE,0) + ISNULL(CGST_RATE,0) +ISNULL(SGST_RATE,0) = GSTR1_HSN.GST_RATE)"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "UPDATE GSTR1_HSN SET SGST = (SELECT SUM(SGST_AMOUNT) FROM SALES_REG_HSN WHERE HSN_CODE =  GSTR1_HSN.HSN_CODE AND " &
                          " UQC = GSTR1_HSN.UQC  AND ISNULL(IGST_RATE,0) + ISNULL(CGST_RATE,0) +ISNULL(SGST_RATE,0) = GSTR1_HSN.GST_RATE)"
        CMD.ExecuteNonQuery()


        If cboACName.Text = "HARISH TRADERS" Then

            CMD.CommandText = " INSERT INTO GSTR1_DOC (SL_No,TYPE_OF_DOCUMENT,CMP_GSTIN) VALUES (1,'Invoices for outward supply','" & cboGSTIN.Text & "')"
            CMD.ExecuteNonQuery()


            CMD.CommandText = "SELECT MIN(B_NO), MAX(B_NO),COUNT(B_NO) FROM TH_BILL_GST WHERE B_DATE >= '" & StartDate() & "' AND B_DATE <= '" & EndDate() & "'"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "INV_DET")

            If DSet.Tables("INV_DET").Rows.Count > 0 Then
                If DSet.Tables("INV_DET").Rows(0).Item(2) > 0 Then
                    CMD.CommandText = " UPDATE GSTR1_DOC SET SL_NO_FROM = '" & DSet.Tables("INV_DET").Rows(0).Item(0).ToString & "' , SL_NO_TO = '" & DSet.Tables("INV_DET").Rows(0).Item(1).ToString & "', " &
                    " DOC_COUNT = " & DSet.Tables("INV_DET").Rows(0).Item(2) & ",CAN_DOC_COUNT = " & (DSet.Tables("INV_DET").Rows(0).Item(1) - DSet.Tables("INV_DET").Rows(0).Item(0) - DSet.Tables("INV_DET").Rows(0).Item(2) + 1).ToString
                    CMD.ExecuteNonQuery()
                End If
            End If


            DSet.Tables("INV_DET").Clear()

        ElseIf cboACName.Text = "HARISH EMBROIDERY" Then
            '
            CMD.CommandText = " INSERT INTO GSTR1_DOC (SL_No,TYPE_OF_DOCUMENT,CMP_GSTIN) VALUES (1,'Invoices for outward supply','" & cboGSTIN.Text & "')"
            CMD.ExecuteNonQuery()


            CMD.CommandText = "SELECT MIN(B_NO), MAX(B_NO),COUNT(B_NO) FROM EMB_BILL_GST WHERE B_DATE >= '" & StartDate() & "' AND B_DATE <= '" & EndDate() & "'"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "INV_DET")

            If DSet.Tables("INV_DET").Rows.Count > 0 Then
                If DSet.Tables("INV_DET").Rows(0).Item(2) > 0 Then
                    CMD.CommandText = " UPDATE GSTR1_DOC SET SL_NO_FROM = '" & DSet.Tables("INV_DET").Rows(0).Item(0).ToString & "' , SL_NO_TO = '" & DSet.Tables("INV_DET").Rows(0).Item(1).ToString & "', " &
                    " DOC_COUNT = " & DSet.Tables("INV_DET").Rows(0).Item(2) & ",CAN_DOC_COUNT = " & (DSet.Tables("INV_DET").Rows(0).Item(1) - DSet.Tables("INV_DET").Rows(0).Item(0) - DSet.Tables("INV_DET").Rows(0).Item(2) + 1).ToString
                    CMD.ExecuteNonQuery()
                End If
            End If

            DSet.Tables("INV_DET").Clear()

        Else

            CMD.CommandText = " INSERT INTO GSTR1_DOC (SL_No,TYPE_OF_DOCUMENT,CMP_GSTIN) VALUES (1,'Invoices for outward supply','" & cboGSTIN.Text & "')"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "SELECT MIN(CONVERT(INT,SALES_NO)), MAX(CONVERT(INT,SALES_NO)),COUNT(SALES_NO) FROM SALES_HEAD SH WHERE SALES_DATE >= '" & StartDate() & "' AND SALES_DATE <= '" & EndDate() & "'  AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "INV_DET")

            If DSet.Tables("INV_DET").Rows.Count > 0 Then
                If DSet.Tables("INV_DET").Rows(0).Item(2) > 0 Then
                    CMD.CommandText = " UPDATE GSTR1_DOC SET SL_NO_FROM = '" & DSet.Tables("INV_DET").Rows(0).Item(0).ToString & "' , SL_NO_TO = '" & DSet.Tables("INV_DET").Rows(0).Item(1).ToString & "', " &
                    " DOC_COUNT = " & DSet.Tables("INV_DET").Rows(0).Item(2) & ",CAN_DOC_COUNT = " & (DSet.Tables("INV_DET").Rows(0).Item(1) - DSet.Tables("INV_DET").Rows(0).Item(0) - DSet.Tables("INV_DET").Rows(0).Item(2) + 1).ToString
                    CMD.ExecuteNonQuery()
                End If
            End If

            DSet.Tables("INV_DET").Clear()

        End If

        CMD.ExecuteNonQuery()

        TX.Commit()

        Try

            CMD.Connection = Cn
            CMD.CommandText = "SELECT * FROM GSTR1_B2B ORDER BY GSTIN,INVNO"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR1_B2B")

            Dim I As Integer

            DGV_B2B.Rows.Clear()

            For I = 1 To DSet.Tables("GSTR1_B2B").Rows.Count

                DGV_B2B.Rows.Add()
                DGV_B2B.Item(0, I - 1).Value = DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(0)
                DGV_B2B.Item(1, I - 1).Value = DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(1)
                DGV_B2B.Item(2, I - 1).Value = DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(2)
                DGV_B2B.Item(3, I - 1).Value = Format(DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(3), "dd-MMM-yyyy")
                DGV_B2B.Item(4, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(4), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(5, I - 1).Value = DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(5)
                DGV_B2B.Item(6, I - 1).Value = DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(6)

                DGV_B2B.Item(8, I - 1).Value = DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(8)
                DGV_B2B.Item(9, I - 1).Value = DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(9)
                DGV_B2B.Item(10, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(10), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(11, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(11), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(12, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2B").Rows(I - 1).Item(12), 2, TriState.False, TriState.False, TriState.False)


            Next



            '-----------------------------------------------------------------

            DGV_B2CL.Rows.Clear()

            CMD.CommandText = "SELECT * FROM GSTR1_B2CL ORDER BY INVNO"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR1_B2CL")

            For I = 1 To DSet.Tables("GSTR1_B2CL").Rows.Count

                DGV_B2CL.Rows.Add()
                DGV_B2CL.Item(0, I - 1).Value = DSet.Tables("GSTR1_B2CL").Rows(I - 1).Item(0)
                DGV_B2CL.Item(1, I - 1).Value = DSet.Tables("GSTR1_B2CL").Rows(I - 1).Item(1)
                DGV_B2CL.Item(2, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2CL").Rows(I - 1).Item(2), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2CL.Item(3, I - 1).Value = DSet.Tables("GSTR1_B2CL").Rows(I - 1).Item(3)

                DGV_B2CL.Item(5, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2CL").Rows(I - 1).Item(5), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2CL.Item(6, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2CL").Rows(I - 1).Item(6), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2CL.Item(7, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2CL").Rows(I - 1).Item(7), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2CL.Item(8, I - 1).Value = DSet.Tables("GSTR1_B2CL").Rows(I - 1).Item(8)


            Next

            '-----------------------------------------------------------------

            DGV_B2CS.Rows.Clear()

            CMD.CommandText = "SELECT * FROM GSTR1_B2CS"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR1_B2CS")

            For I = 1 To DSet.Tables("GSTR1_B2CS").Rows.Count

                DGV_B2CS.Rows.Add()
                DGV_B2CS.Item(0, I - 1).Value = DSet.Tables("GSTR1_B2CS").Rows(I - 1).Item(0)
                DGV_B2CS.Item(1, I - 1).Value = DSet.Tables("GSTR1_B2CS").Rows(I - 1).Item(1)

                DGV_B2CS.Item(3, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2CS").Rows(I - 1).Item(3), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2CS.Item(4, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2CS").Rows(I - 1).Item(4), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2CS.Item(5, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_B2CS").Rows(I - 1).Item(5), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2CS.Item(6, I - 1).Value = DSet.Tables("GSTR1_B2CS").Rows(I - 1).Item(6)

            Next

            '-----------------------------------------------------------------

            DGV_CDNR.Rows.Clear()

            CMD.CommandText = "SELECT * FROM GSTR1_CDNR"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR1_CDNR")

            For I = 1 To DSet.Tables("GSTR1_CDNR").Rows.Count

                DGV_CDNR.Rows.Add()
                DGV_CDNR.Item(0, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(0)
                DGV_CDNR.Item(1, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(1)
                DGV_CDNR.Item(2, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(2)
                DGV_CDNR.Item(3, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(3)
                DGV_CDNR.Item(4, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(4)
                DGV_CDNR.Item(5, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(5)
                DGV_CDNR.Item(6, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(6)
                DGV_CDNR.Item(7, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(7)


                DGV_CDNR.Item(8, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(8), 2, TriState.False, TriState.False, TriState.False)
                'DGV_CDNR.Item(9, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(9), 2, TriState.False, TriState.False, TriState.False)
                DGV_CDNR.Item(10, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(10), 2, TriState.False, TriState.False, TriState.False)
                DGV_CDNR.Item(11, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(11), 2, TriState.False, TriState.False, TriState.False)
                DGV_CDNR.Item(12, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(12), 2, TriState.False, TriState.False, TriState.False)


                DGV_CDNR.Item(13, I - 1).Value = DSet.Tables("GSTR1_CDNR").Rows(I - 1).Item(13)

            Next

            '-----------------------------------------------------------------

            DGV_CDNUR.Rows.Clear()

            CMD.CommandText = "SELECT * FROM GSTR1_CDNUR"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR1_CDNUR")

            For I = 1 To DSet.Tables("GSTR1_CDNUR").Rows.Count

                DGV_CDNUR.Rows.Add()
                DGV_CDNUR.Item(0, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(0)
                DGV_CDNUR.Item(1, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(1)
                DGV_CDNUR.Item(2, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(2)
                DGV_CDNUR.Item(3, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(3)
                DGV_CDNUR.Item(4, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(4)
                DGV_CDNUR.Item(5, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(5)
                DGV_CDNUR.Item(6, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(6)
                DGV_CDNUR.Item(7, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(7)


                DGV_CDNUR.Item(8, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(8), 2, TriState.False, TriState.False, TriState.False)
                'DGV_CDNUR.Item(9, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(9), 2, TriState.False, TriState.False, TriState.False)
                DGV_CDNUR.Item(10, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(10), 2, TriState.False, TriState.False, TriState.False)
                DGV_CDNUR.Item(11, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(11), 2, TriState.False, TriState.False, TriState.False)
                DGV_CDNUR.Item(12, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(12), 2, TriState.False, TriState.False, TriState.False)


                DGV_CDNUR.Item(13, I - 1).Value = DSet.Tables("GSTR1_CDNUR").Rows(I - 1).Item(13)

            Next

            '------------------------------------------------------------------

            DGV_ADV.Rows.Clear()

            CMD.CommandText = "SELECT * FROM GSTR1_ADV"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR1_ADV")

            For I = 1 To DSet.Tables("GSTR1_ADV").Rows.Count

                DGV_ADV.Rows.Add()
                DGV_ADV.Item(0, I - 1).Value = DSet.Tables("GSTR1_ADV").Rows(I - 1).Item(0)

                DGV_ADV.Item(2, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_ADV").Rows(I - 1).Item(2), 2, TriState.False, TriState.False, TriState.False)
                DGV_ADV.Item(3, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_ADV").Rows(I - 1).Item(3), 2, TriState.False, TriState.False, TriState.False)
                DGV_ADV.Item(4, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_ADV").Rows(I - 1).Item(4), 2, TriState.False, TriState.False, TriState.False)

            Next

            '------------------------------------------------------------------

            DGV_HSN.Rows.Clear()

            CMD.CommandText = "SELECT * FROM GSTR1_HSN"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR1_HSN")

            For I = 1 To DSet.Tables("GSTR1_HSN").Rows.Count

                DGV_HSN.Rows.Add()

                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(0)) Then
                    DGV_HSN.Item(0, I - 1).Value = DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(0)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(1)) Then
                    DGV_HSN.Item(1, I - 1).Value = DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(1)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(2)) Then
                    DGV_HSN.Item(2, I - 1).Value = DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(2)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(3)) Then
                    DGV_HSN.Item(3, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(3), 2, TriState.False, TriState.False, TriState.False)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(4)) Then
                    DGV_HSN.Item(4, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(4), 2, TriState.False, TriState.False, TriState.False)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(5)) Then
                    DGV_HSN.Item(5, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(5), 2, TriState.False, TriState.False, TriState.False)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(6)) Then
                    DGV_HSN.Item(6, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(6), 2, TriState.False, TriState.False, TriState.False)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(7)) Then
                    DGV_HSN.Item(7, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(7), 2, TriState.False, TriState.False, TriState.False)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(8)) Then
                    DGV_HSN.Item(8, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(8), 2, TriState.False, TriState.False, TriState.False)
                End If
                If Not IsDBNull(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(9)) Then
                    DGV_HSN.Item(9, I - 1).Value = FormatNumber(DSet.Tables("GSTR1_HSN").Rows(I - 1).Item(9), 2, TriState.False, TriState.False, TriState.False)
                End If

            Next


            CMD.CommandText = "SELECT * FROM GSTR1_DOC ORDER BY SL_NO "

            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR1_DOCS")

            For I = 1 To DSet.Tables("GSTR1_DOCS").Rows.Count

                DGV_DOCS.Rows.Add()
                DGV_DOCS.Item(0, I - 1).Value = DSet.Tables("GSTR1_DOCS").Rows(I - 1).Item(1)
                DGV_DOCS.Item(1, I - 1).Value = DSet.Tables("GSTR1_DOCS").Rows(I - 1).Item(2)
                DGV_DOCS.Item(2, I - 1).Value = DSet.Tables("GSTR1_DOCS").Rows(I - 1).Item(3)
                DGV_DOCS.Item(3, I - 1).Value = DSet.Tables("GSTR1_DOCS").Rows(I - 1).Item(4)
                DGV_DOCS.Item(4, I - 1).Value = DSet.Tables("GSTR1_DOCS").Rows(I - 1).Item(5)
                DGV_DOCS.Item(5, I - 1).Value = DSet.Tables("GSTR1_DOCS").Rows(I - 1).Item(6)

            Next

        Catch ex As Exception

            MsgBox(ex.Message)
            Exit Sub

        End Try

    End Sub

    Private Sub ExportGSTR1toExcel()

        'Timer1.Start()
        'Timer1.Enabled = True

        Dim excel_app As New Excel.Application
        Dim workbook As Excel.Workbook

        Try

            lblAlert.Visible = True

            Dim totrows As Integer = 0
            Dim writtenrows As Integer



            totrows = (DGV_B2B.Rows.Count + DGV_B2CL.Rows.Count + DGV_B2CS.Rows.Count + DGV_CDNR.Rows.Count + DGV_CDNUR.Rows.Count + DGV_ADV.Rows.Count + DGV_HSN.Rows.Count + DGV_DOCS.Rows.Count) - 8

            If totrows > 0 Then
                ProgressBar1.Value = 0
                ProgressBar1.Visible = True
            End If



            'MsgBox(excel_app.Workbooks.Count)
            For Each workbook In excel_app.Workbooks
                If workbook.Name = cboACName.Text & "-" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr1.xlsx" Then
                    MsgBox("Close the Workbook Named " & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr1.xlsx. It needs to be closed to proceed further ")
                    lblAlert.Visible = False
                    ProgressBar1.Visible = False
                    Exit Sub
                End If
            Next

            If File.Exists(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr1.xlsx") Then
                File.Delete(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr1.xlsx")
            End If

            File.Copy(Application.StartupPath & "\GSTR1_Excel_Workbook_Template.xlsx", Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr1.xlsx")

            workbook = excel_app.Workbooks.Open(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr1.xlsx")

            'Dim sheet As Excel.Worksheet = FindSheet(workbook, "b2b")
            Dim sheet As Excel.Worksheet

            Dim I As Integer
            Dim J As Integer

            sheet = workbook.Sheets("b2b")

            For I = 0 To DGV_B2B.RowCount - 2
                For J = 0 To DGV_B2B.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_B2B.Item(J, I).Value
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                End If

            Next

            sheet = workbook.Sheets("b2cl")


            For I = 0 To DGV_B2CL.RowCount - 2
                For J = 0 To DGV_B2CL.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_B2CL.Item(J, I).Value
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                End If

            Next

            sheet = workbook.Sheets("b2cs")

            For I = 0 To DGV_B2CS.RowCount - 2
                For J = 0 To DGV_B2CS.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_B2CS.Item(J, I).Value
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                End If

            Next

            sheet = workbook.Sheets("cdnr")

            For I = 0 To DGV_CDNR.RowCount - 2
                For J = 0 To DGV_CDNR.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_CDNR.Item(J, I).Value
                Next


                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                End If

            Next

            sheet = workbook.Sheets("cdnur")

            For I = 0 To DGV_CDNUR.RowCount - 2
                For J = 0 To DGV_CDNUR.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_CDNUR.Item(J, I).Value
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                End If

            Next

            sheet = workbook.Sheets("at")

            For I = 0 To DGV_ADV.RowCount - 2
                For J = 0 To DGV_ADV.ColumnCount - 1
                    sheet.Cells(I + 5, J + 1) = DGV_ADV.Item(J, I).Value
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                End If

            Next

            sheet = workbook.Sheets("hsn")

            For I = 0 To DGV_HSN.RowCount - 2
                For J = 0 To DGV_HSN.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_HSN.Item(J, I).Value
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                End If

            Next

            sheet = workbook.Sheets("docs")

            For I = 0 To DGV_DOCS.RowCount - 2
                For J = 0 To DGV_DOCS.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_DOCS.Item(J, I).Value
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                End If

            Next



        Catch ex As Exception

            MsgBox(Err.Description)

        Finally

            excel_app.Visible = True
            lblAlert.Visible = False
            ProgressBar1.Visible = False

        End Try

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        lblAlert.Visible = Not lblAlert.Visible
    End Sub

    Private Sub Generate_GSTR1_Json()

        If Len(Microsoft.VisualBasic.Trim(cboYear.Text)) = 0 Or Len(Microsoft.VisualBasic.Trim(cboMonth.Text)) = 0 Then
            MessageBox.Show("Invalid Filing Period", "Filing Period Error...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'Dim GSTR1 As List(Of GSTR1) = New List(Of GSTR1)()

        Dim GSTR1 As New GSTR1


        Dim PREV_FN_YR As String
        Dim FN_YR As String

        If cboMonth.Text = "January" Or cboMonth.Text = "February" Or cboMonth.Text = "March" Then
            PREV_FN_YR = Microsoft.VisualBasic.Right((Val(cboYear.Text) - 2).ToString, 2) + "-" + Microsoft.VisualBasic.Right((Val(cboYear.Text) - 1).ToString, 2)
            FN_YR = Microsoft.VisualBasic.Right((Val(cboYear.Text) - 1).ToString, 2) + "-" + Microsoft.VisualBasic.Right((Val(cboYear.Text)).ToString, 2)
        Else
            PREV_FN_YR = Microsoft.VisualBasic.Right((Val(cboYear.Text) - 1).ToString, 2) + "-" + Microsoft.VisualBasic.Right((Val(cboYear.Text)).ToString, 2)
            FN_YR = Microsoft.VisualBasic.Right((Val(cboYear.Text)).ToString, 2) + "-" + Microsoft.VisualBasic.Right((Val(cboYear.Text) + 1).ToString, 2)
        End If

        Dim DSet As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim DA As New SqlClient.SqlDataAdapter

        cmd.Connection = cn
        cmd.CommandText = " SELECT DISTINCT C.COMPANY_GSTINNO,ISNULL(SUM(B.NET_AMOUNT),0) FROM COMPANY_HEAD C LEFT OUTER JOIN SALES_HEAD B ON C.COMPANY_IDNo = B.COMPANY_IDNo " &
                          " WHERE C.COMPANY_GSTINNO = '" & cboGSTIN.Text & "' AND SALES_CODE LIKE '%" & PREV_FN_YR & "' GROUP BY C.COMPANY_GSTINNO"
        DA.SelectCommand = cmd
        DA.Fill(DSet)

        'Dim GSTR1_A As New GSTR1

        With GSTR1

            .gstin = DSet.Rows(0).Item(0)
            .fp = GetFilingPeriod()
            If Len(Microsoft.VisualBasic.Trim(.fp)) = 0 Then
                MessageBox.Show("Invalid Filing Period", "Filing Period Error...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            .b2b = Get_b2b_List()

            .hsn = get_HSN_Info()
            .doc_issue = get_DOC_Info()

        End With


        Dim json As String = JsonConvert.SerializeObject(GSTR1, Formatting.Indented)
        json = json.Replace("to_doc_no", "to")
        json = json.Replace("from_doc_no", "from")

        If Not IO.Directory.Exists(Application.StartupPath & "\GSTR1") Then
            IO.Directory.CreateDirectory(Application.StartupPath & "\GSTR1")
        End If

        Dim file As StreamWriter
        file = New StreamWriter(Application.StartupPath & "\GSTR1\GSTR1_" & Microsoft.VisualBasic.Left(cboACName.Text, 10) & "_" & cboGSTIN.Text & "_" & cboMonth.Text & "_" & cboYear.Text & ".json", False)
        file.Write(json)
        file.Close()
        file.Dispose()

    End Sub

    Private Function GetFilingPeriod() As String

        GetFilingPeriod = ""

        Try
            If Len(Microsoft.VisualBasic.Trim(cboMonth.Text)) = 0 Or Len(Microsoft.VisualBasic.Trim(cboYear.Text)) = 0 Then
                Return ("")
            End If

            Dim mon() = Microsoft.VisualBasic.Split(cboMonth.Text, "-")

            GetFilingPeriod = DatePart("m", CDate("1-" & mon(UBound(mon)) & "-" & "2020")).ToString.PadLeft(2, "0") & cboYear.Text

            Exit Function

        Catch ex As Exception

            Return ("")

        End Try

    End Function

    Private Function Get_b2b_List() As List(Of b2b)

        Dim b2b As List(Of b2b) = New List(Of b2b)()

        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = cn
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        cmd.CommandText = " Select distinct Ledger_GSTINNo, Ledger_IdNo, GSTIN_VERIFIED from Ledger_Head where Ledger_IdNo in (Select Ledger_IdNo from Sales_GST_Tax_Details where Sales_Date >= '" & StartDate() & "' and " &
                          " Sales_Date <= '" & EndDate() & "' and Company_IdNo = (Select Company_IdNo from Company_Head where Company_GSTINNo = '" & cboGSTIN.Text & "'))"
        da.SelectCommand = cmd
        da.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Dim b2b_a As New b2b
            With b2b_a
                .ctin = dt.Rows(i).Item(0)
                .inv = get_invoices(dt.Rows(i).Item(1))
            End With
            b2b.Add(b2b_a)
        Next

        Get_b2b_List = b2b

    End Function


    Private Function get_HSN_Info() As hsn

        Dim hsn As New hsn
        Dim hsndata As List(Of hsndata) = New List(Of hsndata)

        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = cn
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable

        cmd.CommandText = " Select a.hsn_code,sum(a.Taxable_Amount),sum(isnull(a.igst_amount,0)),sum(isnull(a.cgst_amount,0))," &
                          " sum(isnull(a.sgst_amount,0)), " &
                          " isnull(a.cgst_percentage,0)+isnull(a.sgst_percentage,0)+isnull(a.igst_percentage,0),I.IsService from  sales_gst_tax_details  a " &
                          " inner join ItemGroup_Head I On a.hsn_code = i.Item_HSN_Code Where a.Sales_Date >= '" & StartDate() & "' and a.Sales_Date <= '" & EndDate() & "' " &
                          "  and Company_IdNo = (Select Company_IdNo from Company_Head where Company_GSTINNo = '" & cboGSTIN.Text & "') group by a.hsn_code,isnull(a.cgst_percentage,0)+isnull(a.sgst_percentage,0)+isnull(a.igst_percentage,0),I.IsService"
        da.SelectCommand = cmd
        da.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Dim hsndata_a As New hsndata
            With hsndata_a
                .num = i + 1
                .hsn_sc = dt.Rows(i).Item(0)

                If Not IsDBNull(dt.Rows(i).Item("IsService")) Then
                    If dt.Rows(i).Item("IsService") = True Then
                        .uqc = "NA"
                        .qty = 0
                        GoTo A
                    End If
                End If

                .uqc = "NOS"

                cmd.CommandText = " Select sum(isnull(noof_items,0)) from  sales_details  a " &
                                  " Where a.Sales_Date >= '" & StartDate() & "' and a.Sales_Date <= '" & EndDate() & "' and hsn_code = '" & dt.Rows(i).Item(0) & "' " &
                                  " and Company_IdNo = (Select Company_IdNo from Company_Head where Company_GSTINNo = '" & cboGSTIN.Text & "') "
                da.SelectCommand = cmd
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    If Not IsDBNull(dt1.Rows(0).Item(0)) Then .qty = dt1.Rows(0).Item(0)
                End If
                dt1.Rows.Clear()
                dt1.Dispose()

A:
                .txval = dt.Rows(i).Item(1)
                .iamt = dt.Rows(i).Item(2)
                .camt = dt.Rows(i).Item(3)
                .samt = dt.Rows(i).Item(4)
                .rt = dt.Rows(i).Item(5)

            End With

            hsndata.Add(hsndata_a)
        Next

        hsn.data = hsndata

        get_HSN_Info = hsn

    End Function

    Private Function get_invoices(Led_Id As Integer) As List(Of b2binv)

        Dim b2binv As List(Of b2binv) = New List(Of b2binv)()

        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = cn
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        cmd.CommandText = " Select DISTINCT a.Sales_No,a.Sales_date,a.Net_Amount,s.State_Code from sales_head a inner join Ledger_Head L on a.Ledger_IdNo = l.Ledger_IdNo inner join State_Head s " &
                          " On L.State_IdNo = S.State_IdNo where  A.Ledger_IdNo =  " & Led_Id.ToString & " And Sales_Date >= '" & StartDate() & "' and " &
                          " Sales_Date <= '" & EndDate() & "'  and a.Company_IdNo = (Select Company_IdNo from Company_Head where Company_GSTINNo = '" & cboGSTIN.Text & "')"
        da.SelectCommand = cmd
        da.Fill(dt)

        For i = 0 To dt.Rows.Count - 1

            Dim b2binv_a As New b2binv

            With b2binv_a
                .inum = dt.Rows(i).Item("Sales_No") '
                '.idt = Format(dt.Rows(i).Item("Sales_Date"), "dd") + "-" + Format(dt.Rows(i).Item("Sales_Date"), "MM") + "-" + Format(dt.Rows(i).Item("Sales_Date"), "yyyy")
                '.idt = DatePart("dd", dt.Rows(i).Item("Sales_Date")) + "-" + DatePart("MM", dt.Rows(i).Item("Sales_Date")) + "-" + DatePart("yyyy", dt.Rows(i).Item("Sales_Date"))
                .idt = DatePart("d", dt.Rows(i).Item("Sales_Date")).ToString.PadLeft(2, "0") + "-" + DatePart("m", dt.Rows(i).Item("Sales_Date")).ToString.PadLeft(2, "0") + "-" + DatePart("yyyy", dt.Rows(i).Item("Sales_Date")).ToString.PadLeft(4, "0")
                .val = Val(FormatNumber(dt.Rows(i).Item("Net_Amount"), 2, TriState.False, TriState.False, TriState.False))
                .pos = dt.Rows(i).Item("State_Code")
                If Len(.pos) = 1 Then
                    .pos = "0" + .pos
                End If
                .rchrg = "N"
                .inv_typ = "R"
                .itms = get_invoice_items(Led_Id, dt.Rows(i).Item("Sales_No"))
            End With

            b2binv.Add(b2binv_a)

        Next


        get_invoices = b2binv

    End Function

    Private Function get_invoice_items(Led_Id As Integer, inv_num As String) As List(Of b2binvitem)

        Dim b2binvitem As List(Of b2binvitem) = New List(Of b2binvitem)()

        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = cn
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        cmd.CommandText = " Select DISTINCT * from  sales_gst_tax_details  a " &
                          " Where Sales_Date >= '" & StartDate() & "' and " &
                          " Sales_Date <= '" & EndDate() & "' and Sales_No = '" & inv_num & "' " &
                          " And Company_IdNo = (Select Company_IdNo from Company_Head where Company_GSTINNo = '" & cboGSTIN.Text & "')"
        da.SelectCommand = cmd
        da.Fill(dt)

        For i = 0 To dt.Rows.Count - 1

            Dim b2binvitem_a As New b2binvitem

            With b2binvitem_a

                .num = dt.Rows(i).Item("Sl_No")

                'Dim invitmdetls As New b2binvitemdetails
                Dim invitmdetls As New b2binvitemdetails         'List(Of b2binvitemdetails) = New List(Of b2binvitemdetails)()
                'Dim invitmdetls_a As New b2binvitemdetails

                invitmdetls.rt = dt.Rows(i).Item("CGST_Percentage") + dt.Rows(i).Item("SGST_Percentage") + dt.Rows(i).Item("IGST_Percentage")
                invitmdetls.txval = dt.Rows(i).Item("Taxable_Amount")
                invitmdetls.camt = dt.Rows(i).Item("CGST_Amount")
                invitmdetls.samt = dt.Rows(i).Item("SGST_Amount")
                invitmdetls.iamt = dt.Rows(i).Item("IGST_Amount")
                invitmdetls.csamt = 0.00
                'invitmdetls.Add(invitmdetls_a)

                .itm_det = invitmdetls

            End With

            b2binvitem.Add(b2binvitem_a)

        Next

        get_invoice_items = b2binvitem

    End Function

    Private Function get_DOC_Info() As doc_issue

        Dim doc_issue As New doc_issue

        Dim doc_det As New List(Of doc_det)
        Dim doc_det_A As New doc_det

        Dim docs As New List(Of docs)
        Dim docs_a As New docs

        'Dim hsndata As List(Of hsndata) = New List(Of hsndata)

        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = cn
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable

        cmd.CommandText = " Select min(CONVERT(INT,sales_no)),max(CONVERT(INT,sales_no)),count(sales_no) from sales_head A " &
                          " Where a.Sales_Date >= '" & StartDate() & "' and a.Sales_Date <= '" & EndDate() & "' " &
                          "  and Company_IdNo = (Select Company_IdNo from Company_Head where Company_GSTINNo = '" & cboGSTIN.Text & "')"
        da.SelectCommand = cmd
        da.Fill(dt)

        doc_det_A.doc_num = 1

        docs_a.num = 1
        docs_a.from_doc_no = dt.Rows(0).Item(0)
        docs_a.to_doc_no = dt.Rows(0).Item(1)
        docs_a.totnum = dt.Rows(0).Item(1) - dt.Rows(0).Item(0) + 1
        docs_a.net_issue = dt.Rows(0).Item(2)
        docs_a.cancel = docs_a.totnum - docs_a.net_issue

        docs.Add(docs_a)

        doc_det_A.docs = docs

        doc_det.Add(doc_det_A)

        doc_issue.doc_det = doc_det

        Return doc_issue

    End Function

    Private Sub btnJSON_Click(sender As Object, e As EventArgs) Handles btnJSON.Click

        If MessageBox.Show("PLEASE ENSURE IF THIS COMPUTER HAS AN WORKING INTERNET CONNECTION. PLEASE EXIT IF THERE IS NO INTERNET CONNECTION. MAY WE CONTINUE ? ", "CONTINUE WITH INTERNET CONNECTION ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        lbl_Working.Visible = True

        Try

            Dim cmd As New SqlClient.SqlCommand
            cmd.Connection = cn
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt As New DataTable
            Dim g As GST_UTILITY

            cmd.CommandText = " Select distinct a.Ledger_Name,a.Ledger_GSTINNo,a.Ledger_IdNo,a.GSTIN_VERIFIED,s.State_Code from Ledger_Head a left outer Join State_Head s " &
                              " on a.State_idNo = s.State_IdNo " &
                              " where Ledger_IdNo In (Select Ledger_IdNo from Sales_GST_Tax_Details SGTD where SGTD.Sales_Date >= '" & StartDate() & "' and " &
                              " SGTD.Sales_Date <= '" & EndDate() & "' and SGTD.Company_IdNo = (Select Company_IdNo from Company_Head where Company_GSTINNo = '" & cboGSTIN.Text & "'))"
            da.SelectCommand = cmd
            da.Fill(dt)

            For i = 0 To dt.Rows.Count - 1


                If IsDBNull(dt.Rows(i).Item("State_Code")) Then
                    MessageBox.Show("Please Provide State information for the Ledger : " & dt.Rows(i).Item("Ledger_Name"), "Invalid State", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If dt.Rows(i).Item("State_Code") = 0 Then
                    MessageBox.Show("Please Provide State information for the Ledger : " & dt.Rows(i).Item("Ledger_Name"), "Invalid State", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If IsDBNull(dt.Rows(i).Item("Ledger_GSTINNo")) Then
                    MessageBox.Show("Please Provide GSTIN information for the Ledger : " & dt.Rows(i).Item("Ledger_Name"), "Invalid State", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If Len(Microsoft.VisualBasic.Trim(dt.Rows(i).Item("Ledger_GSTINNo"))) <> 15 Then
                    MessageBox.Show("Please Provide PROPER GSTIN information for the Ledger : " & dt.Rows(i).Item("Ledger_Name"), "Invalid State", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If IsDBNull(dt.Rows(i).Item("GSTIN_VERIFIED")) Then
                    Dim VERIFYSTSTUS = g.Get_GSTIN_Reg_Info(dt.Rows(i).Item("Ledger_GSTINNo"), dt.Rows(i).Item("State_Code"))
                    If Len(VERIFYSTSTUS) = 0 Then
                        MessageBox.Show("ERROR OCCURED WHEN VERIFYING GSTIN FOR THE LEDGER : " & dt.Rows(i).Item("Ledger_Name"), "IGST VALIDATION FAILS", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    Else
                        If Microsoft.VisualBasic.Split(VERIFYSTSTUS, "@#$")(0).ToUpper = "INVALID GSTIN" Then
                            MessageBox.Show("ERROR OCCURED WHEN VERIFYING GSTIN FOR THE LEDGER : " & dt.Rows(i).Item("Ledger_Name"), "IGST VALIDATION FAILS", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If

                If dt.Rows(i).Item("State_Code") = False Then
                    Dim VERIFYSTSTUS = g.Get_GSTIN_Reg_Info(dt.Rows(i).Item("Ledger_GSTINNo"), dt.Rows(i).Item("State_Code"))
                    If Len(VERIFYSTSTUS) = 0 Then
                        MessageBox.Show("ERROR OCCURED WHEN VERIFYING GSTIN FOR THE LEDGER : " & dt.Rows(i).Item("Ledger_Name"), "IGST VALIDATION FAILS", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    Else
                        If Microsoft.VisualBasic.Split(VERIFYSTSTUS, "@#$")(0).ToUpper = "INVALID GSTIN" Then
                            MessageBox.Show("ERROR OCCURED WHEN VERIFYING GSTIN FOR THE LEDGER : " & dt.Rows(i).Item("Ledger_Name"), "IGST VALIDATION FAILS", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If


            Next

A:

            Generate_GSTR1_Json()

            MessageBox.Show("JSON FILE TO BE UPLOADED FOR FILING GSTR 1 FOR THE PERIOD OF " & cboMonth.Text & " " & cboYear.Text & " HAS BEEN GENERATED SUCCESSFULLY ", "FILE GENERATED", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch EX As Exception

            MessageBox.Show(EX.Message, "ERROR OCCURED", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lbl_Working.Visible = False

        Finally

            lbl_Working.Visible = False

        End Try

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)

        'Dim g As New GST_UTILITY
        'MsgBox(g.Get_GSTIN_Reg_Info(cboGSTIN.Text))

    End Sub

    Private Sub btn_MailJSON_Click(sender As Object, e As EventArgs) Handles btn_MailJSON.Click

        If Not IO.File.Exists(Application.StartupPath & "\GSTR1\GSTR1_" & Microsoft.VisualBasic.Left(cboACName.Text, 10) & "_" & cboGSTIN.Text & "_" & cboMonth.Text & "_" & cboYear.Text & ".json") Then
            MessageBox.Show("GSTR1 JSON FILE IS NOT FOUND. PLEASE GENERATE IT .. ", "JSON FILE NOT FOUND", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            GeneratePDF()
            SendMail()
        End If

    End Sub

    Public Sub SendMail(Optional SendJSON As Boolean = True, Optional SendPDF As Boolean = True)

        Try

            Dim CMD As New SqlClient.SqlCommand
            CMD.Connection = cn
            Dim DSet As New DataSet
            Dim DAdapt As New SqlClient.SqlDataAdapter
            DAdapt.SelectCommand = CMD

            Dim Recepient As String
            Dim Smtp_Server As New SmtpClient
            Dim e_mail As New MailMessage()
            Dim attach As Attachment
            Dim Msg As String

            Msg = "Dear Madam/Sir ," & vbCrLf & vbCrLf
            Msg = Msg + " Please find the JSON file to be uploaded for filing GSTR1 " & vbCrLf
            Msg = Msg + " for the Period of  : " & cboMonth.Text & " - " & cboYear.Text & vbCrLf
            Msg = Msg + " GSTIN : " & cboGSTIN.Text & ", BUSINESS NAME : " & cboACName.Text

            Msg = Msg + vbCrLf + vbCrLf & vbCrLf & vbCrLf & vbCrLf
            Msg = Msg + "Sent by TSOFT SOLUTIONS on behslf of " & vbCrLf & cboACName.Text

            Msg = Msg + vbCrLf & vbCrLf & vbCrLf

            Msg = Msg + "This JSON File was generated and mailed from accountbyTS / embroITS software powered by " & vbCrLf
            Msg = Msg + "TSOFT Solutions. For clartifications call / whatsapp 8220078876 "
            Msg = Msg + vbCrLf & vbCrLf

            Msg = Msg + " * Our software solutions have EWB and eInvoice features inbuilt; are User friendly"
            Msg = Msg + " * Our software simplifies data transfer between Tax Practioner / CA and client"

            Smtp_Server.UseDefaultCredentials = False
            Smtp_Server.Credentials = New Net.NetworkCredential("gstreturns.nbs@gmail.com", "Arul_1301")
            Smtp_Server.Port = 587
            Smtp_Server.EnableSsl = True
            Smtp_Server.Host = "smtp.gmail.com"

            CMD.CommandText = "SELECT GSTP_CA_Mail_Id FROM COMPANY_HEAD WHERE COMPANY_GSTINNO = '" & cboGSTIN.Text & "'"
            DAdapt.Fill(DSet, "MAIL_ADDRESS")

            If DSet.Tables("MAIL_ADDRESS").Rows.Count <= 0 Then
                MsgBox("MAIL ID OF GST PRACTIONER / CA NOT FOUND")
                Exit Sub
            Else
                Recepient = DSet.Tables("MAIL_ADDRESS").Rows(0).Item(0)
            End If

            e_mail = New MailMessage()
            e_mail.From = New MailAddress("gstreturns.nbs@gmail.com")
            e_mail.To.Add(Recepient)
            e_mail.Subject = cboACName.Text & " GSTR1 json file for " & cboMonth.Text & " - " & cboYear.Text
            e_mail.IsBodyHtml = False
            e_mail.Body = Msg

            If SendJSON Then
                attach = New Attachment(Application.StartupPath & "\GSTR1\GSTR1_" & Microsoft.VisualBasic.Left(cboACName.Text, 10) & "_" & cboGSTIN.Text & "_" & cboMonth.Text & "_" & cboYear.Text & ".json")
                e_mail.Attachments.Add(attach)
            End If

            If SendPDF Then
                attach = New Attachment(Application.StartupPath & "\GSTR1\GSTR1_" & Microsoft.VisualBasic.Left(cboACName.Text, 10) & "_" & cboGSTIN.Text & "_" & cboMonth.Text & "_" & cboYear.Text & ".pdf")
                e_mail.Attachments.Add(attach)
            End If

            Smtp_Server.Send(e_mail)

            MessageBox.Show("GSTR1 json file and PDF for " & cboMonth.Text & " - " & cboYear.Text & " E-Mailed", "JSON FILE EMAILED ! ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception

            MessageBox.Show(ex.Message & ". GSTR1 json file for " & cboMonth.Text & " - " & cboYear.Text & " could not be E-Mailed", "JSON FILE FAILED ! ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try

    End Sub

    Private Sub cboACName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboACName.SelectedIndexChanged

    End Sub

    Private Sub cboACName_KeyDown(sender As Object, e As KeyEventArgs) Handles cboACName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Nothing, cboACName, "", cboGSTIN, "", "", "", "")
    End Sub

    Private Sub cboGSTIN_KeyDown(sender As Object, e As KeyEventArgs) Handles cboGSTIN.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Nothing, cboGSTIN, cboACName, cboYear, "", "", "", "")
    End Sub

    Private Sub cboYear_KeyDown(sender As Object, e As KeyEventArgs) Handles cboYear.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Nothing, cboYear, cboGSTIN, cboMonth, "", "", "", "")
    End Sub

    Private Sub cboMonth_KeyDown(sender As Object, e As KeyEventArgs) Handles cboMonth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Nothing, cboMonth, cboYear, btnJSON, "", "", "", "")
    End Sub

    Private Sub cboACName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboACName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Nothing, cboACName, cboGSTIN, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
        If Asc(e.KeyChar) = 32 Then
            cboACName.DroppedDown = True
        End If

    End Sub

    Private Sub cboGSTIN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboGSTIN.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Nothing, cboGSTIN, cboYear, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
        If Asc(e.KeyChar) = 32 Then
            cboGSTIN.DroppedDown = True
        End If
    End Sub

    Private Sub cboYear_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboYear.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Nothing, cboYear, cboMonth, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
        If Asc(e.KeyChar) = 32 Then
            cboYear.DroppedDown = True
        End If

    End Sub

    Private Sub cboMonth_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboMonth.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Nothing, cboMonth, btnJSON, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
        If Asc(e.KeyChar) = 32 Then
            cboMonth.DroppedDown = True
        End If

    End Sub

    Public Sub GeneratePDF()

        Dim da As New SqlClient.SqlDataAdapter
        Dim cmd As New SqlClient.SqlCommand
        Dim dtbl1 As DataTable
        Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource

        Dim rptViewer As New Microsoft.Reporting.WinForms.ReportViewer


        rptViewer.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
        rptViewer.LocalReport.ReportPath = Microsoft.VisualBasic.Trim(Common_Procedures.AppPath) & "\Reports\Report_GSTR1_Outward_Unregistered_with_PartyName.rdlc"

        cmd.Connection = cn

        cmd.CommandText = "Truncate table ReportTemp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Truncate table ReportTempSub"
        cmd.ExecuteNonQuery()

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@fromdate", StartDate)
        cmd.Parameters.AddWithValue("@todate", EndDate)

        Dim rptcondt As String = ""


        rptcondt = Microsoft.VisualBasic.Trim(rptcondt) & IIf(Microsoft.VisualBasic.Trim(rptcondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_NameToIdNo(cn, cboACName.Text)))

        cmd.CommandText = "Insert into ReportTemp(Name7                         ,Name5               , Name1              , Name2     ,Meters1        ,  Date1       , Name3             ,  Name4                                                                   ,Currency1                                                           ,  Currency2                                                                                                                                                             , Currency3          , Currency4         , Currency5        ,Currency6      ,weight1                                                                                                                                                   ,Name6    ) " &
                                                      " Select  a.Sales_No+cast(a.Sl_No as varchar) ,LH.ledger_Name ,LH.ledger_GSTinNo  ,a.Sales_No ,a.for_OrderBy  ,a.Sales_Date  , a.HSN_Code        ,(case when sh.State_Name <>'' then sh.State_Code + '-'+ sh.State_Name end )     ,  a.Taxable_Amount + a.CGST_Amount+a.SGST_Amount + a.IGST_Amount    ,     (case when a.CGST_Percentage <> 0 and a.SGST_Percentage <> 0 then a.CGST_Percentage + a.SGST_Percentage  when a.IGST_Percentage <> 0 then a.IGST_Percentage end )   , a.Taxable_Amount  , a.CGST_Amount     ,a.SGST_Amount     ,a.IGST_Amount  ,(select sum(sd.Noof_Items) from Sales_Details sd where a.Sales_CODE = sd.Sales_CODE and a.HSN_Code = sd.HSN_Code group by  sd.Sales_CODE ,sd.HSN_Code )   ,(select top 1 uh.unit_Name from Sales_Details sd1 LEFT OUTER JOIN UNIT_HEAD UH ON sd1.unit_idno = uh.unit_idno   where a.Sales_CODE = sd1.Sales_CODE and a.HSN_Code = sd1.HSN_Code  )    from Sales_GST_Tax_Details a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  LEFT OUTER JOIN Ledger_Head LH on A.Ledger_IdNo = LH.Ledger_IdNo LEFT OUTER JOIN State_Head SH ON LH.State_IdNo = SH.State_IdNo   Where " & rptcondt & IIf(rptcondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate  Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
        cmd.ExecuteNonQuery()

        da = New SqlClient.SqlDataAdapter("select Company_Name as Company_Name, Company_Address1  , Company_Address2 , 'INFORMATION TO BE FURNISHED IN GSTR 1' as Report_Heading1, 'PERIOD :" & cboMonth.Text & " - " & cboYear.Text & "'  as Report_Heading2, '' as Report_Heading3, Name7 ,Name1 , Name2 ,Meters1 ,  Date1 , Name3    ,  Name4 , Name5 ,Name6, Currency1 ,  Currency2 , Currency3 , Currency4 , Currency5 ,Currency6 ,Weight1 from reporttemp Order by Date1, Meters1 ", cn)
        dtbl1 = New DataTable
        da.Fill(dtbl1)

        RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
        RpDs1.Name = "DataSet1"
        RpDs1.Value = dtbl1

        'If Trim(LCase(RptIpDet_ReportName)) = "outward supply - registered with partyname" Then
        rptViewer.LocalReport.ReportPath = Microsoft.VisualBasic.Trim(Common_Procedures.AppPath) & "\Reports\Report_GSTR1_Outward_Unregistered_with_PartyName.rdlc"
        'Else
        '    rptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_GSTR1_Outward_Unregistered_Register.rdlc"
        'End If

        rptViewer.LocalReport.DataSources.Clear()

        rptViewer.LocalReport.DataSources.Add(RpDs1)

        rptViewer.LocalReport.Refresh()
        rptViewer.RefreshReport()


        Dim warnings As Microsoft.Reporting.WinForms.Warning() = {}
        Dim streamids As String() = {}
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim filenameExtension As String = Nothing
        Dim bytes As Byte() = rptViewer.LocalReport.Render("PDF", Nothing, mimeType, encoding, filenameExtension, streamids, warnings)
        Dim FileNm As String = Application.StartupPath & "\GSTR1\GSTR1_" & Microsoft.VisualBasic.Left(cboACName.Text, 10) & "_" & cboGSTIN.Text & "_" & cboMonth.Text & "_" & cboYear.Text & ".pdf"

        If Not IO.Directory.Exists(Application.StartupPath & "\GSTR1") Then
            IO.Directory.CreateDirectory(Application.StartupPath & "\GSTR1")
        End If

        Using fs As FileStream = New FileStream(FileNm, FileMode.Create)
            fs.Write(bytes, 0, bytes.Length)
        End Using

    End Sub

    Private Sub btn_Mail_GSTR1_PDF_Click_2(sender As Object, e As EventArgs) Handles btn_Mail_GSTR1_PDF.Click


        GeneratePDF()
        SendMail(False)



    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles btn_SendJSON.Click

        If Not IO.File.Exists(Application.StartupPath & "\GSTR1\GSTR1_" & Microsoft.VisualBasic.Left(cboACName.Text, 10) & "_" & cboGSTIN.Text & "_" & cboMonth.Text & "_" & cboYear.Text & ".json") Then
            MessageBox.Show("GSTR1 JSON FILE IS NOT FOUND. PLEASE GENERATE IT .. ", "JSON FILE NOT FOUND", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            SendMail(True, False)
        End If

    End Sub
End Class

