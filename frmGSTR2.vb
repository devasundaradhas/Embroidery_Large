Imports System.Data
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO

Public Class frmGSTR2

    Dim Cn As New SqlConnection
    Dim Servername As String
    Dim Password As String
    Dim TransactionDataBase As String


    Private Sub frmGSTR2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Using SR As StreamReader = New StreamReader(System.Windows.Forms.Application.StartupPath & "\LOGINPARAMETERS.TXT")

                Dim LINE As String
                Dim LNNO As Integer = 1

                LINE = SR.ReadLine

                If LINE <> Nothing Then
                    Servername = Authenticate.RevertAuthenticationCode(LINE)
                End If


                While LINE <> Nothing

                    If LNNO = 2 Then
                        Password = Authenticate.RevertAuthenticationCode(LINE)
                    End If

                    If LNNO = 3 Then
                        TransactionDataBase = Authenticate.RevertAuthenticationCode(LINE)
                    End If

                    LINE = SR.ReadLine
                    LNNO = LNNO + 1

                End While

            End Using

        Catch ex As Exception

            MsgBox(ex.Message)

        End Try

        Cn.ConnectionString = "INITIAL CATALOG = GSTR;Data Source=" & Me.Servername & _
                            ";User Id=SA;Password=" & Password
        Cn.Open()

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
                    CMD.CommandText = "SELECT COMPANY_NAME FROM " & TransactionDataBase & ".. COMPANY_HEAD"
                Else
                    CMD.CommandText = "SELECT COMPANY_NAME FROM " & TransactionDataBase & "..COMPANY_HEAD WHERE COMPANY_GSTIN = '" & cboGSTIN.Text & "'"
                End If

                DAdapt.Fill(DSet, "COMPANY_HEAD")


                cboACName.DataSource = DSet.Tables("COMPANY_HEAD")
                cboACName.DisplayMember = "COMPANY_NAME"

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If


        DGV_B2B.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        DGV_B2BUR.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        DGV_HSN.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)

    End Sub

    Private Sub txtRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRefresh.Click

        GenerateGSTR2Info()
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
                CMD.CommandText = "SELECT COMPANY_NAME FROM " & TransactionDataBase & ".. COMPANY_HEAD"
            Else
                CMD.CommandText = "SELECT COMPANY_NAME FROM " & TransactionDataBase & "..COMPANY_HEAD WHERE COMPANY_GSTIN = '" & cboGSTIN.Text & "'"
            End If

            DAdapt.Fill(DSet, "COMPANY_HEAD")


            cboACName.DataSource = DSet.Tables("COMPANY_HEAD")
            cboACName.DisplayMember = "COMPANY_NAME"

        End If


    End Sub

    Private Sub cboACName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboACName.SelectedIndexChanged

    End Sub

    Private Sub cboGSTIN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGSTIN.GotFocus

        Dim DAdapt As New SqlDataAdapter
        Dim CMD As New SqlCommand
        Dim DSet As New DataSet

        DAdapt.SelectCommand = CMD
        CMD.Connection = Cn

        If Len(cboACName.Text) = 0 Then
            CMD.CommandText = "SELECT COMPANY_GSTINNo FROM " & TransactionDataBase & "..COMPANY_HEAD "
        Else
            CMD.CommandText = "SELECT COMPANY_GSTINNo FROM " & TransactionDataBase & "..COMPANY_HEAD WHERE COMPANY_NAME = '" & cboACName.Text & "'"
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


        StartDate = "1-" & Microsoft.VisualBasic.Left((Trim(Split(cboMonth.Text)(0))), 3) & "-" & cboYear.Text


    End Function

    Private Function EndDate() As String

        EndDate = "31-Dec-9999"

        If StartDate() = "1-Jan-2000" Then
            Exit Function
        End If

        If UBound(Split(cboMonth.Text, "-")) > 0 Then

            EndDate = DateAdd(DateInterval.Month, 3, CDate(StartDate())).ToString
        Else

            EndDate = DateAdd(DateInterval.Month, 1, CDate(StartDate())).ToString
        End If

        EndDate = Format(DateAdd(DateInterval.Day, -1, CDate(EndDate)), "dd-MMM-yyyy")

    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Public Sub GenerateGSTR2Info()

        If Len(Trim(cboGSTIN.Text)) = 0 Then
            MsgBox("SELECT A VALID GSTIN")
            Exit Sub
        End If

        If Len(cboYear.Text) = 0 Or Len(cboMonth.Text) = 0 Then
            MsgBox("Invalid Year/Month Selection")
            Exit Sub
        End If

        Dim TransactCn As New SqlConnection

        TransactCn.ConnectionString = "INITIAL CATALOG = " & TransactionDataBase & ";Data Source=" & Me.Servername & _
                       ";User Id=SA;Password=" & Password
        TransactCn.Open()

        Dim DAdapt As New SqlDataAdapter
        Dim CMD As New SqlCommand
        Dim DSet As New DataSet

        DAdapt.SelectCommand = CMD

       

        Try

            
           

            CMD.Connection = TransactCn


            CMD.CommandText = "SELECT COUNT(COMPANY_GSTINNo) FROM COMPANY_HEAD " & _
                             " WHERE COMPANY_GSTINNo IN ('" & cboGSTIN.Text & "')"
            ''MsgBox(CMD.CommandText)
            DAdapt.Fill(DSet, "ACCOUNT_MASTER_GSTIN")

            If DSet.Tables("ACCOUNT_MASTER_GSTIN").Rows(0).Item(0) <= 0 Then
                MsgBox("INVALID GSTIN ENTERED. ENTER A CORRECT GSTIN")
                Exit Sub
            End If


            CMD.CommandText = "DELETE FROM GSTR..GSTR2_B2B "
            CMD.ExecuteNonQuery()

            CMD.CommandText = "DELETE FROM GSTR..GSTR2_B2BUR "
            CMD.ExecuteNonQuery()

            CMD.CommandText = "DELETE FROM GSTR..GSTR2_HSN "
            CMD.ExecuteNonQuery()



            CMD.CommandText = " INSERT INTO GSTR..GSTR2_B2B SELECT LH.LEDGER_GSTINNo AS GSTIN, PH.Purchase_NO AS INVN,PH.Purchase_DATE AS INVDATE,PH.NET_AMOUNT AS INVVALUE," & _
                              " CONVERT(VARCHAR,STH.STATE_CODE)+'-'+STH.STATE_NAME,'N','Regular'," & _
                              " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0) AS GSTRATE,SUM(GSTD.TAXABLE_AMOUNT) AS TAXABLE_RATE," & _
                              " SUM(GSTD.IGST_AMOUNT),SUM(GSTD.CGST_AMOUNT),SUM(GSTD.SGST_AMOUNT),0,'Input Goods',SUM(GSTD.IGST_AMOUNT),SUM(GSTD.CGST_AMOUNT),SUM(GSTD.SGST_AMOUNT),0,'" & _
                              cboGSTIN.Text & "' FROM " & _
                              " Purchase_HEAD PH INNER JOIN Purchase_GST_Tax_Details GSTD ON PH.Purchase_CODE = GSTD.Purchase_CODE INNER JOIN LEDGER_HEAD LH ON PH.LEDGER_IDNO = LH.LEDGER_IDNO " & _
                              " INNER JOIN STATE_HEAD STH ON STH.STATE_IDNO = LH.STATE_IDNO " & _
                              " WHERE  LEN(LTRIM(RTRIM(LH.LEDGER_GSTINNo))) = 15   AND PH.PURCHASE_DATE >= '" & StartDate() & "' AND PH.PURCHASE_DATE <= '" & EndDate() & "' GROUP BY  " & _
                              " LH.LEDGER_GSTINNo , PH.Purchase_NO,PH.Purchase_DATE,PH.NET_AMOUNT," & _
                              " ISNULL(GSTD.IGST_PERCENTAGE,0)+ISNULL(GSTD.CGST_PERCENTAGE,0)+ISNULL(GSTD.SGST_PERCENTAGE,0),CONVERT(VARCHAR,STH.STATE_CODE)+'-'+STH.STATE_NAME"

            CMD.ExecuteNonQuery()

            '---------------------------------------------------------------------

            CMD.CommandText = "TRUNCATE TABLE GSTR..PURCHASE_REG_HSN"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "INSERT INTO GSTR..PURCHASE_REG_HSN SELECT PH.PURCHASE_CODE,PD.HSN_CODE,UH.UNIT_NAME,SUM(PD.NOOF_ITEMS),SUM(PD.ASSESSABLE_VALUE),0,0,0,0,0,0 " & _
                              " FROM  Purchase_HEAD PH INNER JOIN Purchase_Details PD ON PH.Purchase_CODE = PD.Purchase_CODE " & _
                              " INNER JOIN UNIT_HEAD UH ON PD.UNIT_IDNO = UH.UNIT_IDNO " & _
                              " AND PH.PURCHASE_DATE >= '" & StartDate() & "' AND PH.PURCHASE_DATE <= '" & EndDate() & "' AND PH.LEDGER_IDNO IN (SELECT LEDGER_IDNO FROM LEDGER_HEAD " & _
                              " WHERE LEN(LEDGER_GSTINNo) =15)  GROUP BY PD.HSN_CODE,UH.UNIT_NAME,PH.PURCHASE_CODE"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE GSTR..PURCHASE_REG_HSN SET IGST_RATE = (SELECT IGST_PERCENTAGE FROM Purchase_GST_Tax_Details WHERE PURCHASE_CODE = GSTR..PURCHASE_REG_HSN.PURCHASE_CODE " & _
                              " AND HSN_CODE = GSTR..PURCHASE_REG_HSN.HSN_CODE)"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE GSTR..PURCHASE_REG_HSN SET CGST_RATE = (SELECT CGST_PERCENTAGE FROM Purchase_GST_Tax_Details WHERE PURCHASE_CODE = GSTR..PURCHASE_REG_HSN.PURCHASE_CODE " & _
                              " AND HSN_CODE = GSTR..PURCHASE_REG_HSN.HSN_CODE)"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE GSTR..PURCHASE_REG_HSN SET SGST_RATE = (SELECT SGST_PERCENTAGE FROM Purchase_GST_Tax_Details WHERE PURCHASE_CODE = GSTR..PURCHASE_REG_HSN.PURCHASE_CODE " & _
                              " AND HSN_CODE = GSTR..PURCHASE_REG_HSN.HSN_CODE)"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE GSTR..PURCHASE_REG_HSN SET IGST_AMOUNT  = (TAXABLE_AMOUNT * IGST_RATE /100) WHERE IGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE GSTR..PURCHASE_REG_HSN SET CGST_AMOUNT  = (TAXABLE_AMOUNT * CGST_RATE /100) WHERE CGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE GSTR..PURCHASE_REG_HSN SET SGST_AMOUNT  = (TAXABLE_AMOUNT * SGST_RATE /100) WHERE SGST_RATE > 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "INSERT INTO GSTR..GSTR2_HSN SELECT PD.HSN_CODE,'',UH.UNIT_NAME,SUM(PD.NOOF_ITEMS),SUM(PD.ASSESSABLE_VALUE)+(SUM(PD.ASSESSABLE_VALUE) * PD.GST_PERCENTAGE /100), " & _
                              "SUM(PD.ASSESSABLE_VALUE)," & _
                              "0,0,0,0,'" & cboGSTIN.Text & "',PD.GST_PERCENTAGE " & _
                              " FROM  Purchase_HEAD PH INNER JOIN Purchase_Details PD ON PH.Purchase_CODE = PD.Purchase_CODE " & _
                              " INNER JOIN UNIT_HEAD UH ON PD.UNIT_IDNO = UH.UNIT_IDNO " & _
                              " AND PH.PURCHASE_DATE >= '" & StartDate() & "' AND PH.PURCHASE_DATE <= '" & EndDate() & "' AND PH.LEDGER_IDNO IN (SELECT LEDGER_IDNO FROM LEDGER_HEAD " & _
                              " WHERE LEN(LEDGER_GSTINNo) =15)  GROUP BY PD.HSN_CODE,UH.UNIT_NAME,PD.GST_PERCENTAGE "


            CMD.ExecuteNonQuery()


            CMD.CommandText = "UPDATE GSTR..GSTR2_HSN SET IGST = (SELECT SUM(IGST_AMOUNT) FROM GSTR..PURCHASE_REG_HSN WHERE HSN_CODE =  GSTR..GSTR2_HSN.HSN_CODE AND " & _
                              " UQC = GSTR..GSTR2_HSN.UQC AND ISNULL(IGST_RATE,0) + ISNULL(CGST_RATE,0) +ISNULL(SGST_RATE,0) = GSTR..GSTR2_HSN.GST_RATE)"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE GSTR..GSTR2_HSN SET CGST = (SELECT SUM(CGST_AMOUNT) FROM GSTR..PURCHASE_REG_HSN WHERE HSN_CODE =  GSTR..GSTR2_HSN.HSN_CODE AND " & _
                              " UQC = GSTR..GSTR2_HSN.UQC AND ISNULL(IGST_RATE,0) + ISNULL(CGST_RATE,0) +ISNULL(SGST_RATE,0) = GSTR..GSTR2_HSN.GST_RATE)"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE GSTR..GSTR2_HSN SET SGST = (SELECT SUM(SGST_AMOUNT) FROM GSTR..PURCHASE_REG_HSN WHERE HSN_CODE =  GSTR..GSTR2_HSN.HSN_CODE AND " & _
                              " UQC = GSTR..GSTR2_HSN.UQC  AND ISNULL(IGST_RATE,0) + ISNULL(CGST_RATE,0) +ISNULL(SGST_RATE,0) = GSTR..GSTR2_HSN.GST_RATE)"
            CMD.ExecuteNonQuery()

            CMD.Connection = Cn
            CMD.CommandText = "SELECT * FROM GSTR2_B2B ORDER BY GSTIN,INVNO"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR2_B2B")

            Dim I As Integer

            DGV_B2B.Rows.Clear()

            For I = 1 To DSet.Tables("GSTR2_B2B").Rows.Count

                DGV_B2B.Rows.Add()
                DGV_B2B.Item(0, I - 1).Value = DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(0).ToString
                DGV_B2B.Item(1, I - 1).Value = DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(1).ToString
                DGV_B2B.Item(2, I - 1).Value = Format(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(2), "dd-MMM-yyyy")
                DGV_B2B.Item(3, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(3), 2, TriState.True, TriState.True, TriState.True)
                DGV_B2B.Item(4, I - 1).Value = DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(4).ToString
                DGV_B2B.Item(5, I - 1).Value = DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(5).ToString
                DGV_B2B.Item(6, I - 1).Value = DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(6).ToString
                DGV_B2B.Item(7, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(7), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(8, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(8), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(9, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(9), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(10, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(10), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(11, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(11), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(12, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(12), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(13, I - 1).Value = DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(13).ToString
                DGV_B2B.Item(14, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(14), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(15, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(15), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(16, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(16), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2B.Item(17, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2B").Rows(I - 1).Item(17), 2, TriState.False, TriState.False, TriState.False)


            Next



            '-----------------------------------------------------------------

            DGV_B2BUR.Rows.Clear()

            CMD.CommandText = "SELECT * FROM GSTR2_B2BUR ORDER BY SUPPLIER_NAME,INVDATE,INVNO"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR2_B2BUR")

            For I = 1 To DSet.Tables("GSTR2_B2BUR").Rows.Count

                DGV_B2BUR.Rows.Add()
                DGV_B2BUR.Item(0, I - 1).Value = DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(0)
                DGV_B2BUR.Item(1, I - 1).Value = DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(1)
                DGV_B2BUR.Item(2, I - 1).Value = Format(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(2), "dd-MMM-yyyy")
                DGV_B2BUR.Item(3, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(3), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(4, I - 1).Value = DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(4)
                DGV_B2BUR.Item(5, I - 1).Value = DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(5)
                DGV_B2BUR.Item(6, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(6), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(7, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(7), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(8, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(8), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(9, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(9), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(10, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(10), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(11, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(11), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(12, I - 1).Value = DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(12)
                DGV_B2BUR.Item(13, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(13), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(14, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(14), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(15, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(15), 2, TriState.False, TriState.False, TriState.False)
                DGV_B2BUR.Item(16, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_B2BUR").Rows(I - 1).Item(16), 2, TriState.False, TriState.False, TriState.False)

            Next



            '------------------------------------------------------------------

            DGV_HSN.Rows.Clear()

            CMD.CommandText = "SELECT * FROM GSTR2_HSN"
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR2_HSN")

            For I = 1 To DSet.Tables("GSTR2_HSN").Rows.Count

                DGV_HSN.Rows.Add()
                DGV_HSN.Item(0, I - 1).Value = DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(0)
                DGV_HSN.Item(1, I - 1).Value = DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(1)
                DGV_HSN.Item(2, I - 1).Value = DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(2)
                DGV_HSN.Item(3, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(3), 2, TriState.False, TriState.False, TriState.False)
                DGV_HSN.Item(4, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(4), 2, TriState.False, TriState.False, TriState.False)
                DGV_HSN.Item(5, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(5), 2, TriState.False, TriState.False, TriState.False)
                DGV_HSN.Item(6, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(6), 2, TriState.False, TriState.False, TriState.False)
                DGV_HSN.Item(7, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(7), 2, TriState.False, TriState.False, TriState.False)
                DGV_HSN.Item(8, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(8), 2, TriState.False, TriState.False, TriState.False)
                DGV_HSN.Item(9, I - 1).Value = FormatNumber(DSet.Tables("GSTR2_HSN").Rows(I - 1).Item(9), 2, TriState.False, TriState.False, TriState.False)

            Next


        Catch ex As Exception

            MsgBox(Err.Description & " ERROR OCCURED")
            Exit Sub

        End Try

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        GenerateGSTR2Info()
        ExportGSTR2toExcel()

        Exit Sub

    End Sub


    Private Sub ExportGSTR2toExcel()


        Dim excel_app As New Excel.Application
        Dim workbook As Excel.Workbook

        lblAlert.Visible = True
        Timer1.Enabled = True

        Try

            Dim totrows As Integer = 0
            Dim writtenrows As Integer = 0

            totrows = (DGV_B2B.Rows.Count + DGV_B2BUR.Rows.Count + DGV_HSN.Rows.Count) - 3



            If totrows > 0 Then
                ProgressBar1.Value = 0
                ProgressBar1.Visible = True
            End If



            For Each workbook In excel_app.Workbooks
                If workbook.Name = cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr2.xlsx" Then
                    MsgBox("Close the Workbook Named " & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr2.xlsx. It needs to be closed to proceed further ")
                    Exit Sub
                End If
            Next

            If File.Exists(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr2.xlsx") Then
                File.Delete(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr2.xlsx")
            End If


            File.Copy(Application.StartupPath & "\GSTR2_Excel_Workbook_TemplateNew.xlsx", Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr2.xlsx")

            workbook = excel_app.Workbooks.Open(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr2.xlsx")

            'Dim sheet As Excel.Worksheet = FindSheet(workbook, "b2b")
            Dim sheet As Excel.Worksheet



            Dim I As Integer
            Dim J As Integer

            sheet = workbook.Sheets("b2b")

            For I = 0 To DGV_B2B.RowCount - 2
                For J = 0 To DGV_B2B.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_B2B.Item(J, I).Value.ToString
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                    ProgressBar1.Refresh()
                End If
            Next

            '----------------------

            sheet = workbook.Sheets("b2bur")


            For I = 0 To DGV_B2BUR.RowCount - 2
                For J = 0 To DGV_B2BUR.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_B2BUR.Item(J, I).Value.ToString
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                    ProgressBar1.Refresh()
                End If

            Next

            '------------------------

            sheet = workbook.Sheets("hsnsum")

            For I = 0 To DGV_HSN.RowCount - 2
                For J = 0 To DGV_HSN.ColumnCount - 2
                    sheet.Cells(I + 5, J + 1) = DGV_HSN.Item(J, I).Value.ToString
                Next

                If totrows > 0 Then
                    writtenrows = writtenrows + 1
                    ProgressBar1.Value = writtenrows / totrows * 100
                    ProgressBar1.Refresh()
                End If

            Next



            'workbook.Save()
            'workbook.Close()
            'excel_app.Quit()

        Catch ex As Exception

            MsgBox(ex.Message)

        Finally

            Timer1.Enabled = False
            excel_app.Visible = True
            lblAlert.Visible = False
            ProgressBar1.Visible = False

        End Try

    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        lblAlert.Visible = Not lblAlert.Visible
    End Sub
End Class