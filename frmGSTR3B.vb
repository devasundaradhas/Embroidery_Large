Imports System.Data
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO

Public Class frmGSTR3B

    Dim Cn As New SqlConnection(Common_Procedures.Connection_String)
    Dim Servername As String
    Dim Password As String
    Dim TransactionDataBase As String

    Private Sub frmGSTR3B_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

        'Cn.ConnectionString = "INITIAL CATALOG = GSTR;Data Source=" & Me.Servername & _
        '                    ";User Id=SA;Password=" & Password
        Cn.Open()

        Me.MdiParent = MDIParent1

        Dim I As Integer

        For I = 2017 To 2100
            cboYear.Items.Add(I.ToString)
        Next

        Me.Top = 40

        If Not Cn Is Nothing Then

            Try

                Dim DAdapt As New SqlDataAdapter
                Dim CMD As New SqlCommand
                Dim DSet As New DataSet


                DAdapt.SelectCommand = CMD
                CMD.Connection = Cn

                If Len(cboGSTIN.Text) = 0 Then
                    CMD.CommandText = "SELECT COMPANY_NAME FROM COMPANY_HEAD"
                Else
                    CMD.CommandText = "SELECT COMPANY_NAME FROM COMPANY_HEAD WHERE COMPANY_GSTIN = '" & cboGSTIN.Text & "'"
                End If

                DAdapt.Fill(DSet, "COMPANY_HEAD")


                cboACName.DataSource = DSet.Tables("COMPANY_HEAD")
                cboACName.DisplayMember = "COMPANY_NAME"

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If



    End Sub

    Private Sub cboACName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboACName.GotFocus

        If Not Cn Is Nothing Then

            Dim DAdapt As New SqlDataAdapter
            Dim CMD As New SqlCommand
            Dim DSet As New DataSet


            DAdapt.SelectCommand = CMD
            CMD.Connection = Cn

            If Len(cboGSTIN.Text) = 0 Then
                CMD.CommandText = "SELECT COMPANY_NAME FROM  COMPANY_HEAD"
            Else
                CMD.CommandText = "SELECT COMPANY_NAME FROM COMPANY_HEAD WHERE COMPANY_GSTIN = '" & cboGSTIN.Text & "'"
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

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click

        GenerateGSTR3BInfo()

    End Sub

    Public Sub GenerateGSTR3BInfo()

        If Len(Trim(cboGSTIN.Text)) = 0 Then
            MsgBox("SELECT A VALID GSTIN")
            Exit Sub
        End If

        If Len(cboYear.Text) = 0 Or Len(cboMonth.Text) = 0 Then
            MsgBox("Invalid Year/Month Selection")
            Exit Sub
        End If

        'Dim Cn As New SqlConnection

        'Cn.ConnectionString = "INITIAL CATALOG = " & TransactionDataBase & ";Data Source=" & Me.Servername & _
        '               ";User Id=SA;Password=" & Password
        'Cn.Open()

        Dim DAdapt As New SqlDataAdapter
        Dim CMD As New SqlCommand
        Dim DSet As New DataSet
        Dim Cmp_IdNo As String

        DAdapt.SelectCommand = CMD

        Try

            CMD.Connection = Cn

            'CMD.CommandText = "SELECT SERVERNAME,SERVERPASSWORD,TRANSDATABASENAME,GSTINFIELD,GSTINTABLE FROM ACCOUNT_MASTER WHERE GSTIN = '" & cboGSTIN.Text & "'"
            'DAdapt.Fill(DSet, "ACCOUNT_MASTER")

            'Cn.ConnectionString = "INITIAL CATALOG = " & DSet.Tables("ACCOUNT_MASTER").Rows(0).Item(2) & ";Data Source=" & DSet.Tables("ACCOUNT_MASTER").Rows(0).Item(0) & _
            '                ";User Id=SA;Password=" & Authenticate.RevertAuthenticationCode(DSet.Tables("ACCOUNT_MASTER").Rows(0).Item(1))
            'MsgBox(Cn.ConnectionString)
            'Cn.Open()

            CMD.Connection = Cn

            CMD.CommandText = "SELECT COUNT(COMPANY_GSTINNo),MIN(COMPANY_IDNo) FROM COMPANY_HEAD " & _
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


            CMD.CommandText = "DELETE FROM GSTR3B "
            CMD.ExecuteNonQuery()

            '------------------------------------------------------- HARISH

            If cboACName.Text = "HARISH TRADERS" Then
                CMD.CommandText = " INSERT INTO GSTR3B SELECT ISNULL(SUM(ISNULL(TOTAL_AMT,0)),0),ISNULL(SUM(ISNULL(IGST_AMOUNT,0)),0),ISNULL(SUM(ISNULL(CGST_AMOUNT,0)),0)," & _
                                  " ISNULL(SUM(ISNULL(SGST_AMOUNT,0)),0),0,0,0,0,0,'" & cboGSTIN.Text & "' FROM TH_BILL_GST " & _
                                  " WHERE B_DATE >= '" & StartDate() & "' AND B_DATE <= '" & EndDate() & "'"
            ElseIf cboACName.Text = "HARISH EMBROIDERY" Then
                CMD.CommandText = " INSERT INTO GSTR3B SELECT ISNULL(SUM(ISNULL(TOTAL_AMT,0)),0),ISNULL(SUM(ISNULL(IGST,0)),0),ISNULL(SUM(ISNULL(CGST,0)),0)," & _
                                  " ISNULL(SUM(ISNULL(SGST,0)),0),0,0,0,0,0,'" & cboGSTIN.Text & "' FROM EMB_BILL_GST " & _
                                  " WHERE B_DATE >= '" & StartDate() & "' AND B_DATE <= '" & EndDate() & "'"
            Else

                CMD.CommandText = " INSERT INTO GSTR3B SELECT ISNULL(SUM(ISNULL(GSTD.TAXABLE_AMOUNT,0)),0),ISNULL(SUM(ISNULL(GSTD.IGST_AMOUNT,0)),0),ISNULL(SUM(ISNULL(GSTD.CGST_AMOUNT,0)),0)," & _
                                  " ISNULL(SUM(ISNULL(GSTD.SGST_AMOUNT,0)),0),0,0,0,0,0,'" & cboGSTIN.Text & "' FROM Sales_GST_Tax_Details GSTD " & _
                                  " INNER JOIN SALES_HEAD SH ON SH.SALES_CODE = GSTD.SALES_CODE AND SH.SALES_DATE >= CONVERT(SMALLDATETIME,'" & StartDate() & "') AND SH.SALES_DATE <= CONVERT(SMALLDATETIME,'" & EndDate() & "')   AND SH.COMPANY_IDNo = " & Cmp_IdNo.ToString
            End If

            'MsgBox(CMD.CommandText)
            CMD.ExecuteNonQuery()


            

           

            '------------------------------


            If cboACName.Text = "HARISH TRADERS" Then

            ElseIf cboACName.Text = "HARISH EMBROIDERY" Then

            Else

                CMD.CommandText = " UPDATE GSTR3B SET ITC_IGST = ITC.IGST,ITC_CGST = ITC.CGST,ITC_SGST = ITC.SGST,ITC_CESS = 0 FROM " & _
                                  " (SELECT ISNULL(SUM(ISNULL(GSTD.IGST_AMOUNT,0)),0) AS IGST ,ISNULL(SUM(ISNULL(GSTD.CGST_AMOUNT,0)),0) AS CGST," & _
                                  " ISNULL(SUM(ISNULL(GSTD.SGST_AMOUNT,0)),0) AS SGST FROM PURCHASE_GST_Tax_Details GSTD " & _
                                  " INNER JOIN PURCHASE_HEAD PH ON GSTD.PURCHASE_CODE = PH.PURCHASE_CODE " & _
                                  " INNER JOIN LEDGER_HEAD LH ON LH.LEDGER_IDNO = PH.LEDGER_IDNO " & _
                                  " WHERE PH.PURCHASE_DATE >= '" & StartDate() & "' AND PH.PURCHASE_DATE <= '" & EndDate() & "' AND LEN(LH.LEDGER_GSTINNo) = 15   AND PH.COMPANY_IDNo = " & Cmp_IdNo.ToString & " ) ITC"

            End If

            CMD.ExecuteNonQuery()

           


           




            CMD.Connection = Cn
            CMD.CommandText = "SELECT * FROM GSTR3B "
            DAdapt.SelectCommand = CMD
            DAdapt.Fill(DSet, "GSTR3B")

            If DSet.Tables("GSTR3B").Rows.Count > 0 Then

                txtTaxableValue.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(0), 2, TriState.False, TriState.False, TriState.False)
                txtIGST.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(1), 2, TriState.False, TriState.False, TriState.False)
                txtCGST.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(2), 2, TriState.False, TriState.False, TriState.False)
                txtSGST.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(3), 2, TriState.False, TriState.False, TriState.False)
                txtCESS.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(4), 2, TriState.False, TriState.False, TriState.False)
                txtITC_IGST.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(5), 2, TriState.False, TriState.False, TriState.False)
                txtITC_CGST.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(6), 2, TriState.False, TriState.False, TriState.False)
                txtITC_SGST.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(7), 2, TriState.False, TriState.False, TriState.False)
                txtITC_CESS.Text = FormatNumber(DSet.Tables("GSTR3B").Rows(0).Item(8), 2, TriState.False, TriState.False, TriState.False)

            End If

        Catch EX As Exception

            MsgBox(EX.Message & " GSTR3B DATA COULD NOT BE GENERATED")
        End Try

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        GenerateGSTR3BInfo()
        ExportGSTR3BtoExcel()
        Exit Sub

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub


    Private Sub ExportGSTR3BtoExcel()

        Try


            If File.Exists(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr3B.xlsm") Then
                File.Delete(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr3B.xlsm")
            End If

            Dim excel_app As New Excel.Application
            excel_app.Visible = True

            File.Copy(Application.StartupPath & "\GSTR3B_Excel_Utility.xlsm", Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr3B.xlsm")

            Dim workbook As Excel.Workbook = excel_app.Workbooks.Open(Application.StartupPath & "\" & cboACName.Text & "-" & cboYear.Text & "-" & cboMonth.Text & "-gstr3B.xlsm")

            'Dim sheet As Excel.Worksheet = FindSheet(workbook, "b2b")
            Dim sheet As Excel.Worksheet


            sheet = workbook.Sheets("gstr-3b")

            sheet.Cells(5, 3) = cboGSTIN.Text
            sheet.Cells(6, 3) = cboACName.Text

            sheet.Cells(6, 7) = cboMonth.Text

            'If cboMonth.Text = "January" Or cboMonth.Text = "February" Or cboMonth.Text = "March" Then
            'sheet.Cells(5, 7) = (Val(Microsoft.VisualBasic.Right(cboYear.Text, 2)) - 1).ToString() & "-" & cboYear.Text
            'Else
            sheet.Cells(5, 7) = cboYear.Text + "-" + (Val(Microsoft.VisualBasic.Right(cboYear.Text, 2)) + 1).ToString
            'End If

            'sheet.Cells(11, 3) = "500.00"
            sheet.Cells(11, 3) = txtTaxableValue.Text
            sheet.Cells(11, 4) = txtIGST.Text
            sheet.Cells(11, 5) = txtCGST.Text
            'sheet.Cells(11, 6) = "0.00"
            sheet.Cells(11, 7) = txtCESS.Text

            sheet.Cells(26, 3) = txtITC_IGST.Text
            sheet.Cells(26, 4) = txtITC_CGST.Text
            'sheet.Cells(11, 8) = txtITC_SGST.Text
            sheet.Cells(26, 6) = txtITC_CESS.Text

            workbook.Save()
            'workbook.Close()
            'excel_app.Quit()


        Catch ex As Exception

        End Try

    End Sub

End Class