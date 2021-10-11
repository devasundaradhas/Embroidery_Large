Public Class Profit_And_Loss
    Implements Interface_MDIActions
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private Opn_Stock As Double = 0
    Private Cls_Stock As Double = 0
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetMxIndx As Integer
    Private prn_pageHeight As Double = 0
    Private prn_DetAr(200, 10) As String
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer


    Public Structure Report_ComboDetails
        Dim PKey As String
        Dim TableName As String
        Dim Selection_FieldName As String
        Dim Return_FieldName As String
        Dim Condition As String
        Dim Display_Name As String
        Dim BlankFieldCondition As String
        Dim CtrlType_Cbo_OR_Txt As String
    End Structure

'    Private prn_HdDt As New DataTable
'    Private prn_DetDt As New DataTable
'    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
'    Private prn_DetAr(50, 10) As String
'    Private prn_DetMxIndx As Integer
'    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer = 0
    Private RptCboDet(10) As Report_ComboDetails

    Private Sub clear()

        dtp_FromDate.Text = Common_Procedures.Company_FromDate

        If Val(Common_Procedures.settings.Report_Show_CurrentDate_IN_ToDate) = 1 Then
            dtp_ToDate.Text = Date.Today
            If dtp_ToDate.Visible = False Then
                dtp_FromDate.Text = Date.Today
            End If

        Else

            dtp_ToDate.Text = Common_Procedures.Company_ToDate
            If dtp_ToDate.Visible = False Then
                dtp_FromDate.Text = Common_Procedures.Company_ToDate
            End If

        End If

        cbo_Inputs1.Text = ""

        lbl_OpeningStock_Value.Text = "0.00"
        lbl_PurchaseAccValue.Text = "0.00"
        lbl_DirectExpensesValue.Text = "0.00"
        lbl_GrossProfitValueDb.Text = "0.00"
        lbl_SalesAccValue.Text = "0.00"
        lbl_ClosingStockValue.Text = "0.00"
        lbl_GrossLossValueCr.Text = "0.00"

        lbl_GrossTotalDb.Text = "0.00"
        lbl_GrossTotalCr.Text = "0.00"

        lbl_GrossLossDbValue.Text = "0.00"
        lbl_indirectExpenseValue.Text = "0.00"
        lbl_NetProfitValueDb.Text = "0.00"
        lbl_GrossProfitValueCr.Text = "0.00"
        lbl_IncomeValue.Text = "0.00"
        lbl_NetLossValueCr.Text = "0.00"
        lbl_TotalDb.Text = "0.00"
        lbl_TotalCr.Text = "0.00"

        lbl_GrossProfitDb.Visible = False
        lbl_GrossprofitCr.Visible = False
        lbl_GrossLossDb.Visible = False
        lbl_GrossLossCr.Visible = False
        lbl_NetProfitDb.Visible = False
        lbl_NetLossCr.Visible = False

        lbl_GrossProfitValueDb.Visible = False
        lbl_GrossLossValueCr.Visible = False
        lbl_GrossLossDbValue.Visible = False
        lbl_GrossProfitValueCr.Visible = False
        lbl_NetProfitValueDb.Visible = False
        lbl_NetLossValueCr.Visible = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If
        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub Profit_And_Loss_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub

    Private Sub Profit_Loss_Simple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strRpInpts As String = ""
        Dim ShowCompCol_STS As Boolean = True

        con.Open()

        Me.Left = 0
        Me.Top = 0
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width - 10
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height - 90

        pnl_ReportInputs.Location = New Point(0, 0)
        pnl_ReportDetails.Location = New Point(0, 0)
        pnl_GridView.Location = New Point(0, 95)
        pnl_GridView.Visible = False

        'pnl_ReportDetails.Dock = DockStyle.Bottom
        pnl_ReportInputs.Top = Screen.PrimaryScreen.WorkingArea.Top + lbl_FormTitle.Height

        pnl_ReportInputs.Height = 80
        pnl_ReportInputs.Width = Screen.PrimaryScreen.WorkingArea.Width - 20

        pnl_ReportInputs.Width = Screen.PrimaryScreen.WorkingArea.Width - 20

        opt_Simple.Checked = True


        AddHandler dtp_FromDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Inputs1.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_FromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Inputs1.LostFocus, AddressOf ControlLostFocus


        For i = 1 To 10
            RptCboDet(i).PKey = ""
            RptCboDet(i).TableName = ""
            RptCboDet(i).Selection_FieldName = ""
            RptCboDet(i).Return_FieldName = ""
            RptCboDet(i).Condition = ""
            RptCboDet(i).BlankFieldCondition = ""
            RptCboDet(i).Display_Name = ""
            RptCboDet(i).CtrlType_Cbo_OR_Txt = ""
        Next

        RptCboDet(1).PKey = "Z"
        RptCboDet(1).TableName = "Company_Head"
        RptCboDet(1).Selection_FieldName = "Company_ShortName"
        RptCboDet(1).Return_FieldName = "Company_IdNo"
        RptCboDet(1).Condition = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            RptCboDet(1).Condition = "(Company_Type <> 'UNACCOUNT')"
        End If
        RptCboDet(1).Display_Name = "Company"
        RptCboDet(1).BlankFieldCondition = "(Company_IdNo = 0)"
        RptCboDet(1).CtrlType_Cbo_OR_Txt = "C"

        ShowCompCol_STS = Common_Procedures.Show_CompanyCondition_for_Report(con)
        If ShowCompCol_STS = False Then
            lbl_Inputs1.Visible = False
            cbo_Inputs1.Visible = False
        Else
            lbl_Inputs1.Visible = True
            cbo_Inputs1.Visible = True
        End If
        cbo_Inputs1.Tag = "Z"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_Inputs1, con, RptCboDet(1).TableName, RptCboDet(1).Selection_FieldName, RptCboDet(1).Condition, RptCboDet(1).BlankFieldCondition)
        lbl_Inputs1.Text = RptCboDet(1).Display_Name & " *"

        If Trim(UCase(cbo_Inputs1.Tag)) = "Z" Then
            If ShowCompCol_STS = False Then
                lbl_Inputs1.Visible = False
                cbo_Inputs1.Visible = False
            End If
        End If

        clear()

        If ShowCompCol_STS = False Then
            Show_Report()
        End If

    End Sub

    Private Function Company_Condition() As String
        Dim Condt As String = ""

        Condt = ""
        If cbo_Inputs1.Visible = True Then
            If Trim(cbo_Inputs1.Text) <> "" Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " tZ.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Inputs1.Text)))
            End If
        End If
        Company_Condition = Condt
    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '-------------------
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----------------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---------------
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '-----------------
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '-----------------
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '---------------
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '----------------
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '-----------------
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-------------
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If opt_Simple.Checked = True Then
        Print_Selection()

        ElseIf opt_Details.Checked = True Then
            WeightBridge_Entry_Pending_Report_Printing()

        End If

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '-----------------
    End Sub

    Private Sub dtp_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_ToDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Inputs1.Visible And cbo_Inputs1.Enabled Then
                cbo_Inputs1.Focus()
            Else
                txt_Selection.Focus()
                Show_Report()
            End If
        End If
    End Sub

    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_Inputs1.Visible And cbo_Inputs1.Enabled Then
                cbo_Inputs1.Focus()
            Else
                txt_Selection.Focus()
                Show_Report()
            End If
        End If
    End Sub

    Private Sub dtp_FromDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_ToDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                dtp_ToDate.Focus()
            ElseIf cbo_Inputs1.Visible And cbo_Inputs1.Enabled Then
                cbo_Inputs1.Focus()
            Else
                btn_Show.Focus()
                Show_Report()
            End If

        End If

    End Sub

    Private Sub dtp_FromDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FromDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                dtp_ToDate.Focus()
            ElseIf cbo_Inputs1.Visible And cbo_Inputs1.Enabled Then
                cbo_Inputs1.Focus()
            Else
                btn_Show.Focus()
                Show_Report()
            End If
        End If

    End Sub

    Private Sub cbo_Inputs1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Inputs1.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, RptCboDet(1).TableName, RptCboDet(1).Selection_FieldName, RptCboDet(1).Condition, RptCboDet(1).BlankFieldCondition)
    End Sub

    Private Sub cbo_Inputs1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Inputs1.KeyDown

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Inputs1, Nothing, Nothing, RptCboDet(1).TableName, RptCboDet(1).Selection_FieldName, RptCboDet(1).Condition, RptCboDet(1).BlankFieldCondition)

            With cbo_Inputs1
                If (e.KeyValue = 38 And .DropDownStyle = ComboBoxStyle.Simple) Or (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True
                    If dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_FromDate.Visible And dtp_FromDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If

                ElseIf (e.KeyValue = 40 And .DropDownStyle = ComboBoxStyle.Simple) Or (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    btn_Show.Focus()
                    Show_Report()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Inputs1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Inputs1.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Inputs1, Nothing, RptCboDet(1).TableName, RptCboDet(1).Selection_FieldName, RptCboDet(1).Condition, RptCboDet(1).BlankFieldCondition)

            If Asc(e.KeyChar) = 13 Then
                btn_Show.Focus()
                Show_Report()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub Show_Report()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CompCondt As String = ""
        Dim RepSTS As Boolean = False

        Try

            Profit_AND_LOSS_Calculation()
            If txt_Selection.Visible And txt_Selection.Enabled Then txt_Selection.Focus()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub Profit_AND_LOSS_Calculation()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim condt As String = ""
        Dim VouAmt As Double = 0
        Dim db_amt As Double = 0
        Dim cr_amt As Double = 0
        Dim Nr As Long = 0

        condt = Company_Condition()

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        Cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        Cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        Cmd.CommandText = "truncate table reporttemp"
        Cmd.ExecuteNonQuery()
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1105" Then '---- Ganga Weaving (Dindugal)
            Cmd.CommandText = "insert into reporttemp ( name1, currency1 ) Select a.parent_code, sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date between @fromdate and @todate and a.ledger_idno = tz.ledger_idno group by a.parent_code Having sum(tz.voucher_amount) <> 0"
            Cmd.ExecuteNonQuery()

        Else

            'Cmd.CommandText = "insert into reporttemp ( name1, currency1 ) Select a.parent_code, sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date between @companyfromdate and @todate and a.ledger_idno = tz.ledger_idno group by a.parent_code Having sum(tz.voucher_amount) <> 0"
            Cmd.CommandText = "insert into reporttemp ( name1, currency1 ) Select a.parent_code, sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date between @fromdate and @todate and a.ledger_idno = tz.ledger_idno group by a.parent_code Having sum(tz.voucher_amount) <> 0"
            Cmd.ExecuteNonQuery()

        End If




        '----OPENING STOCK

        Cmd.CommandText = "truncate table reporttempsub"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into reporttempsub ( date1, currency1 ) Select tZ.Closing_Stock_Value_Date, sum(tz.Closing_Stock_Value) from Closing_Stock_Value_Head tz where " & condt & IIf(condt <> "", " and ", "") & " tz.Closing_Stock_Value_Date <= @todate group by tZ.Closing_Stock_Value_Date Having sum(tz.Closing_Stock_Value) <> 0"
        Nr = Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into reporttempsub ( date1, currency1 ) Select tZ.voucher_date, sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " a.parent_code like '%~9~4~%' and tz.voucher_date <= @todate and a.ledger_idno = tz.ledger_idno group by tZ.voucher_date Having sum(tz.voucher_amount) <> 0"
        'Nr = Cmd.ExecuteNonQuery()


        Opn_Stock = 0
        lbl_OpeningStock_Value.Text = "0.00"
        Cmd.CommandText = "Select top 1 Currency1 as OpStockValue from ReportTempSub where date1 <= @fromdate Order by date1 desc"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                lbl_OpeningStock_Value.Text = Common_Procedures.Currency_Format(Val(Dt1.Rows(0)(0).ToString))
                If Val(lbl_OpeningStock_Value.Text) <> 0 Then db_amt = db_amt + Common_Procedures.Currency_Format(Val(CDbl(lbl_OpeningStock_Value.Text)))
                Opn_Stock = Common_Procedures.Currency_Format(Val(CDbl(lbl_OpeningStock_Value.Text)))
            End If
        End If
        Dt1.Clear()


        '----PURCHASE ACCOUNTS
        VouAmt = get_VoucherSummary("~27~18~")
        lbl_PurchaseAccValue.Text = Common_Procedures.Currency_Format(Val(-1 * VouAmt))
        If Val(lbl_PurchaseAccValue.Text) <> 0 Then db_amt = db_amt + Common_Procedures.Currency_Format(Val(CDbl(lbl_PurchaseAccValue.Text)))

        '-----DIRECT EXPENSES
        VouAmt = get_VoucherSummary("~15~18~")
        lbl_DirectExpensesValue.Text = Common_Procedures.Currency_Format(Val(-1 * VouAmt))
        If Val(lbl_DirectExpensesValue.Text) <> 0 Then db_amt = db_amt + Common_Procedures.Currency_Format(Val(CDbl(lbl_DirectExpensesValue.Text)))

        '----SALES ACCOUNTS
        VouAmt = get_VoucherSummary("~28~18~")
        lbl_SalesAccValue.Text = Common_Procedures.Currency_Format(Val(VouAmt))
        If Val(lbl_SalesAccValue.Text) <> 0 Then cr_amt = cr_amt + Common_Procedures.Currency_Format(Val(CDbl(lbl_SalesAccValue.Text)))

        '-----CLOSING STOCK
        Cls_Stock = 0
        lbl_ClosingStockValue.Text = "0.00"
        Cmd.CommandText = "Select top 1 Currency1 as OpStockValue from ReportTempSub where date1 <= @todate Order by date1 desc"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                lbl_ClosingStockValue.Text = Common_Procedures.Currency_Format(Val(Dt1.Rows(0)(0).ToString))
                If Val(lbl_ClosingStockValue.Text) <> 0 Then cr_amt = cr_amt + Common_Procedures.Currency_Format(Val(Dt1.Rows(0)(0).ToString))
                Cls_Stock = Common_Procedures.Currency_Format(Val(Dt1.Rows(0)(0).ToString))
            End If
        End If
        Dt1.Clear()


        lbl_GrossProfitValueDb.Text = "0.00"
        lbl_GrossProfitValueCr.Text = "0.00"

        lbl_GrossLossDbValue.Text = "0.00"
        lbl_GrossLossValueCr.Text = "0.00"

        lbl_GrossTotalDb.Text = "0.00"
        lbl_GrossTotalCr.Text = "0.00"

        If cr_amt > db_amt Then
            lbl_GrossLossDb.Visible = False
            lbl_GrossLossCr.Visible = False
            lbl_GrossLossDbValue.Visible = False
            lbl_GrossLossValueCr.Visible = False

            lbl_GrossProfitDb.Visible = True
            lbl_GrossprofitCr.Visible = True
            lbl_GrossProfitValueDb.Visible = True
            lbl_GrossProfitValueCr.Visible = True
            lbl_GrossProfitDb.ForeColor = Color.Green
            lbl_GrossprofitCr.ForeColor = Color.Green
            lbl_GrossProfitValueDb.ForeColor = Color.Green
            lbl_GrossProfitValueDb.ForeColor = Color.Green
            lbl_GrossTotalDb.ForeColor = Color.DarkBlue
            lbl_GrossTotalCr.ForeColor = Color.DarkBlue



            lbl_GrossProfitValueDb.Text = Common_Procedures.Currency_Format(Val(cr_amt - db_amt))
            lbl_GrossProfitValueCr.Text = Common_Procedures.Currency_Format(Val(cr_amt - db_amt))

            lbl_GrossTotalDb.Text = Common_Procedures.Currency_Format(CDbl(lbl_SalesAccValue.Text) + CDbl(lbl_ClosingStockValue.Text))
            lbl_GrossTotalCr.Text = Common_Procedures.Currency_Format(CDbl(lbl_SalesAccValue.Text) + CDbl(lbl_ClosingStockValue.Text))

        Else
            lbl_GrossProfitDb.Visible = False
            lbl_GrossprofitCr.Visible = False
            lbl_GrossProfitValueDb.Visible = False
            lbl_GrossProfitValueCr.Visible = False

            lbl_GrossLossDb.Visible = True
            lbl_GrossLossCr.Visible = True
            lbl_GrossLossDbValue.Visible = True
            lbl_GrossLossValueCr.Visible = True
            lbl_GrossLossDb.ForeColor = Color.Red
            lbl_GrossLossCr.ForeColor = Color.Red
            lbl_GrossLossDbValue.ForeColor = Color.Red
            lbl_GrossLossValueCr.ForeColor = Color.Red
            lbl_GrossTotalDb.ForeColor = Color.DarkBlue
            lbl_GrossTotalCr.ForeColor = Color.DarkBlue

            lbl_GrossLossDbValue.Text = Common_Procedures.Currency_Format(Val(db_amt - cr_amt))
            lbl_GrossLossValueCr.Text = Common_Procedures.Currency_Format(Val(db_amt - cr_amt))


            lbl_GrossTotalDb.Text = Common_Procedures.Currency_Format(CDbl(lbl_PurchaseAccValue.Text) + CDbl(lbl_DirectExpensesValue.Text))
            lbl_GrossTotalCr.Text = Common_Procedures.Currency_Format(CDbl(lbl_PurchaseAccValue.Text) + CDbl(lbl_DirectExpensesValue.Text))
        End If


        If cr_amt >= db_amt Then  '------------Gross Profit
            cr_amt = cr_amt - db_amt
            db_amt = 0
        Else                      '------------Gross Loss

            db_amt = db_amt - cr_amt
            cr_amt = 0
        End If

        '-------INDIRECT EXPENSES
        VouAmt = get_VoucherSummary("~16~18~")
        lbl_indirectExpenseValue.Text = Common_Procedures.Currency_Format(-1 * Val(VouAmt))
        If Val(lbl_indirectExpenseValue.Text) <> 0 Then db_amt = db_amt + Common_Procedures.Currency_Format(CDbl(lbl_indirectExpenseValue.Text))

        '-------INCOME (REVENUE)
        VouAmt = get_VoucherSummary("~19~18~")
        lbl_IncomeValue.Text = Common_Procedures.Currency_Format(Val(VouAmt))
        If Val(lbl_IncomeValue.Text) <> 0 Then cr_amt = cr_amt + Common_Procedures.Currency_Format(CDbl(lbl_IncomeValue.Text))

        lbl_NetProfitValueDb.Text = "0.00"
        lbl_NetLossValueCr.Text = "0.00"
        If cr_amt > db_amt Then
            lbl_NetLossCr.Visible = False
            lbl_NetLossValueCr.Visible = False
            lbl_NetProfitDb.Visible = True
            lbl_NetProfitValueDb.Visible = True
            lbl_NetProfitDb.ForeColor = Color.Green
            lbl_NetProfitValueDb.ForeColor = Color.Green

            lbl_NetProfitValueDb.Text = Common_Procedures.Currency_Format(CDbl(lbl_GrossProfitValueCr.Text) + CDbl(lbl_IncomeValue.Text) - CDbl(lbl_indirectExpenseValue.Text))

            lbl_TotalDb.Text = Common_Procedures.Currency_Format(CDbl(lbl_IncomeValue.Text) + CDbl(lbl_GrossProfitValueCr.Text))
            lbl_TotalCr.Text = Common_Procedures.Currency_Format(CDbl(lbl_IncomeValue.Text) + CDbl(lbl_GrossProfitValueCr.Text))
        Else
            lbl_NetProfitDb.Visible = False
            lbl_NetProfitValueDb.Visible = False

            lbl_NetLossCr.Visible = True
            lbl_NetLossValueCr.Visible = True

            lbl_NetLossCr.ForeColor = Color.Red
            lbl_NetLossValueCr.ForeColor = Color.Red

            lbl_NetLossValueCr.Text = Common_Procedures.Currency_Format(CDbl(lbl_GrossLossDbValue.Text) + CDbl(lbl_indirectExpenseValue.Text) - CDbl(lbl_GrossProfitValueCr.Text) - CDbl(lbl_IncomeValue.Text))

            lbl_TotalDb.Text = Common_Procedures.Currency_Format(CDbl(lbl_indirectExpenseValue.Text) + CDbl(lbl_GrossLossDbValue.Text))
            lbl_TotalCr.Text = Common_Procedures.Currency_Format(CDbl(lbl_indirectExpenseValue.Text) + CDbl(lbl_GrossLossDbValue.Text))
        End If

    End Sub

    Private Function get_VoucherSummary(ByVal grp_cd As String) As Double
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim condt As String = ""
        Dim VouAmt As Double = 0

        Cmd.Connection = con

        '----OPENING STOCK
        VouAmt = 0
        Da1 = New SqlClient.SqlDataAdapter("Select sum(currency1) from reporttemp where name1 LIKE '%" & Trim(grp_cd) & "'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                VouAmt = Val(Dt1.Rows(0)(0).ToString)
            End If
        End If
        Dt1.Clear()

        get_VoucherSummary = VouAmt

    End Function

    Private Sub btn_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        If opt_Simple.Checked = True Then
            pnl_GridView.Visible = False
            pnl_Back.Visible = True
            Show_Report()
        End If
        If opt_Details.Checked = True Then
            pnl_Back.Visible = False
            pnl_GridView.Visible = True
            get_DetailedGridReport()
        End If
    End Sub

    Private Sub Label_Selection(ByVal color As Integer)

        lbl_PurchaseAccName.ForeColor = Drawing.Color.Black
        lbl_PurchaseAccValue.ForeColor = Drawing.Color.Black
        lbl_PurchaseAccName.BackColor = Drawing.Color.White
        lbl_PurchaseAccValue.BackColor = Drawing.Color.White

        lbl_DirectExpensesName.ForeColor = Drawing.Color.Black
        lbl_DirectExpensesValue.ForeColor = Drawing.Color.Black
        lbl_DirectExpensesName.BackColor = Drawing.Color.White
        lbl_DirectExpensesValue.BackColor = Drawing.Color.White

        lbl_indirectExpenseName.ForeColor = Drawing.Color.Black
        lbl_indirectExpenseValue.ForeColor = Drawing.Color.Black
        lbl_indirectExpenseName.BackColor = Drawing.Color.White
        lbl_indirectExpenseValue.BackColor = Drawing.Color.White

        lbl_SalesAccName.ForeColor = Drawing.Color.Black
        lbl_SalesAccValue.ForeColor = Drawing.Color.Black
        lbl_SalesAccName.BackColor = Drawing.Color.White
        lbl_SalesAccValue.BackColor = Drawing.Color.White

        lbl_IncomeName.ForeColor = Drawing.Color.Black
        lbl_IncomeValue.ForeColor = Drawing.Color.Black
        lbl_IncomeName.BackColor = Drawing.Color.White
        lbl_IncomeValue.BackColor = Drawing.Color.White

        'If color = 1 Then
        '    Select Case Val(txt_Selection.Text)
        '        Case 1
        '            lbl_PurchaseAccName.ForeColor = Drawing.Color.Red
        '            lbl_PurchaseAccValue.ForeColor = Drawing.Color.Red
        '            lbl_PurchaseAccName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
        '            lbl_PurchaseAccValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)

        '        Case 2
        '            lbl_DirectExpensesName.ForeColor = Drawing.Color.Red
        '            lbl_DirectExpensesValue.ForeColor = Drawing.Color.Red
        '            lbl_DirectExpensesName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
        '            lbl_DirectExpensesValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)

        '        Case 3
        '            lbl_indirectExpenseName.ForeColor = Drawing.Color.Red
        '            lbl_indirectExpenseValue.ForeColor = Drawing.Color.Red
        '            lbl_indirectExpenseName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
        '            lbl_indirectExpenseValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)
        '        Case 4
        '            lbl_SalesAccName.ForeColor = Drawing.Color.Red
        '            lbl_SalesAccValue.ForeColor = Drawing.Color.Red
        '            lbl_SalesAccName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
        '            lbl_SalesAccValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)
        '        Case 5
        '            lbl_IncomeName.ForeColor = Drawing.Color.Red
        '            lbl_IncomeValue.ForeColor = Drawing.Color.Red
        '            lbl_IncomeName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
        '            lbl_IncomeValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)
        '    End Select
        'End If

        If color = 2 Then
            lbl_Selection.Left = Choose(Val(txt_Selection.Text), 0, 0, 0, lbl_SalesAccName.Left, lbl_IncomeName.Left) - 5
            lbl_Selection.Top = Choose(Val(txt_Selection.Text), lbl_PurchaseAccName.Top, lbl_DirectExpensesName.Top, lbl_indirectExpenseName.Top, lbl_SalesAccName.Top, lbl_IncomeName.Top) - 4

            Select Case Val(txt_Selection.Text)
                Case 1
                    lbl_PurchaseAccName.ForeColor = Drawing.Color.Red
                    lbl_PurchaseAccValue.ForeColor = Drawing.Color.Red
                    lbl_PurchaseAccName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_PurchaseAccValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)

                Case 2
                    lbl_DirectExpensesName.ForeColor = Drawing.Color.Red
                    lbl_DirectExpensesValue.ForeColor = Drawing.Color.Red
                    lbl_DirectExpensesName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_DirectExpensesValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)

                Case 3
                    lbl_indirectExpenseName.ForeColor = Drawing.Color.Red
                    lbl_indirectExpenseValue.ForeColor = Drawing.Color.Red
                    lbl_indirectExpenseName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_indirectExpenseValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 4
                    lbl_SalesAccName.ForeColor = Drawing.Color.Red
                    lbl_SalesAccValue.ForeColor = Drawing.Color.Red
                    lbl_SalesAccName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_SalesAccValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 5
                    lbl_IncomeName.ForeColor = Drawing.Color.Red
                    lbl_IncomeValue.ForeColor = Drawing.Color.Red
                    lbl_IncomeName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_IncomeValue.BackColor = Drawing.Color.FromArgb(192, 192, 255)
            End Select
        End If
    End Sub
    Private Sub get_Details(ByVal grp_code As Integer)
        Dim Grp_Name As String = ""
        Dim grp_cd As String = ""


        Select Case grp_code

            Case 1
                Grp_Name = Trim(lbl_PurchaseAccName.Text)
                grp_cd = "~27~18~"
            Case 2
                Grp_Name = Trim(lbl_DirectExpensesName.Text)
                grp_cd = "~15~18~"
            Case 3
                Grp_Name = Trim(lbl_indirectExpenseName.Text)
                grp_cd = "~16~18~"
            Case 4
                Grp_Name = Trim(lbl_SalesAccName.Text)
                grp_cd = "~28~18~"
            Case 5
                Grp_Name = Trim(lbl_IncomeName.Text)
                grp_cd = "~19~18~"

        End Select

        If Trim(grp_cd) <> "" Then

            Dim f As New Report_Details_1
            Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
            Common_Procedures.RptInputDet.ReportName = "Group Ledger - Grid"
            Common_Procedures.RptInputDet.ReportHeading = "Group Ledger"
            Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,G*"
            Common_Procedures.RptInputDet.IsGridReport = True
            f.MdiParent = MDIParent1
            f.Show()

            f.dtp_FromDate.Text = dtp_ToDate.Text
            f.dtp_ToDate.Text = dtp_ToDate.Text

            f.cbo_Inputs1.Text = Trim(cbo_Inputs1.Text)
            f.cbo_Inputs2.Text = Common_Procedures.AccountsGroup_CodeToName(con, Trim(grp_cd))
            f.cbo_Inputs3.Text = ""
            f.cbo_Inputs4.Text = ""
            f.cbo_Inputs5.Text = ""

            f.Show_Report()

            f.RptSubReport_Index = 1

            f.RptSubReportDet(f.RptSubReport_Index).ReportName = ""
            f.RptSubReportDet(f.RptSubReport_Index).ReportGroupName = ""
            f.RptSubReportDet(f.RptSubReport_Index).ReportHeading = ""
            f.RptSubReportDet(f.RptSubReport_Index).ReportInputs = ""
            f.RptSubReportDet(f.RptSubReport_Index).IsGridReport = False
            f.RptSubReportDet(f.RptSubReport_Index).CurrentRowVal = -1
            f.RptSubReportDet(f.RptSubReport_Index).TopRowVal = -1
            f.RptSubReportDet(f.RptSubReport_Index).DateInp_Value1 = #1/1/1900#
            f.RptSubReportDet(f.RptSubReport_Index).DateInp_Value2 = #1/1/1900#
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text1 = ""
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text2 = ""
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text3 = ""
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text4 = ""
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text5 = "'"

            For I = 1 To 10

                f.RptSubReportInpDet(f.RptSubReport_Index, I).PKey = ""
                f.RptSubReportInpDet(f.RptSubReport_Index, I).TableName = ""
                f.RptSubReportInpDet(f.RptSubReport_Index, I).Selection_FieldName = ""
                f.RptSubReportInpDet(f.RptSubReport_Index, I).Return_FieldName = ""
                f.RptSubReportInpDet(f.RptSubReport_Index, I).Condition = ""
                f.RptSubReportInpDet(f.RptSubReport_Index, I).Display_Name = ""
                f.RptSubReportInpDet(f.RptSubReport_Index, I).BlankFieldCondition = ""
                f.RptSubReportInpDet(f.RptSubReport_Index, I).CtrlType_Cbo_OR_Txt = ""

            Next I

            f.RptSubReportDet(f.RptSubReport_Index).ReportName = "Profit & Loss"
            f.RptSubReportDet(f.RptSubReport_Index).ReportGroupName = "Accounts"
            f.RptSubReportDet(f.RptSubReport_Index).ReportHeading = "Profit & Loss"
            f.RptSubReportDet(f.RptSubReport_Index).ReportInputs = "2DT,Z"
            f.RptSubReportDet(f.RptSubReport_Index).IsGridReport = False
            f.RptSubReportDet(f.RptSubReport_Index).CurrentRowVal = Val(txt_Selection.Text)
            f.RptSubReportDet(f.RptSubReport_Index).TopRowVal = -1

            f.RptSubReportDet(f.RptSubReport_Index).DateInp_Value1 = dtp_FromDate.Value.ToShortDateString
            f.RptSubReportDet(f.RptSubReport_Index).DateInp_Value2 = dtp_ToDate.Value.ToShortDateString
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text1 = cbo_Inputs1.Text
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text2 = ""
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text3 = ""
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text4 = ""
            f.RptSubReportDet(f.RptSubReport_Index).CboInp_Text5 = ""

            For I = 1 To 10

                f.RptSubReportInpDet(f.RptSubReport_Index, I).PKey = RptCboDet(I).PKey
                f.RptSubReportInpDet(f.RptSubReport_Index, I).TableName = RptCboDet(I).TableName
                f.RptSubReportInpDet(f.RptSubReport_Index, I).Selection_FieldName = RptCboDet(I).Selection_FieldName
                f.RptSubReportInpDet(f.RptSubReport_Index, I).Return_FieldName = RptCboDet(I).Return_FieldName
                f.RptSubReportInpDet(f.RptSubReport_Index, I).Condition = RptCboDet(I).Condition
                f.RptSubReportInpDet(f.RptSubReport_Index, I).Display_Name = RptCboDet(I).Display_Name
                f.RptSubReportInpDet(f.RptSubReport_Index, I).BlankFieldCondition = RptCboDet(I).BlankFieldCondition
                f.RptSubReportInpDet(f.RptSubReport_Index, I).CtrlType_Cbo_OR_Txt = RptCboDet(I).CtrlType_Cbo_OR_Txt

            Next I

            Me.Close()

        End If



    End Sub
    Private Sub txt_Selection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Selection.KeyPress
        If Asc(e.KeyChar) = 13 Then
            get_Details(Val(txt_Selection.Text))
        End If
    End Sub

    Private Sub txt_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Selection.KeyDown
        Call Label_Selection(1)
        If e.KeyValue = 40 Then If Val(txt_Selection.Text) < 5 Then txt_Selection.Text = Val(txt_Selection.Text) + 1
        If e.KeyValue = 38 Then If Val(txt_Selection.Text) > 1 Then txt_Selection.Text = Val(txt_Selection.Text) - 1
        If e.KeyValue = 39 Then
            If Val(txt_Selection.Text) = 1 Or Val(txt_Selection.Text) = 2 Then txt_Selection.Text = 4
            If Val(txt_Selection.Text) = 3 Then txt_Selection.Text = 5
        End If
        If e.KeyValue = 37 Then
            If Val(txt_Selection.Text) = 4 Then txt_Selection.Text = 1
            If Val(txt_Selection.Text) = 5 Then txt_Selection.Text = 3
        End If
        Call Label_Selection(2)
    End Sub

    Private Sub get_DetailedGridReport()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable

        Dim Dtbl1 As New DataTable
        Dim Dtbl2 As New DataTable
        Dim condt As String = ""
        Dim N As Long = 0
        Dim Bal As Decimal
        Dim Tot_CR As Decimal = 0, Tot_DB As Decimal = 0
        Dim Grp_Name As String = ""
        Dim grp_cd As String = ""
        Dim z As Integer = 0
        Dim supTtl As Integer = 0
        Dim cnt As Integer = 0
        Dim Sps As String = ""
        Dim vGrs_Prof_Loss_Amt As String = 0
        Dim NtTt_DB As String = 0
        Dim NtTt_CR As String = 0



        condt = Company_Condition()

        Profit_AND_LOSS_Calculation()

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        cmd.CommandText = "truncate table reporttemp"
        cmd.ExecuteNonQuery()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1105" Then '---- Ganga Weaving (Dindugal)
            cmd.CommandText = "insert into reporttemp (Name1, Name2 , meters1 , int1 ,Name3 , currency1, currency2) Select a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name, (case when sum(tV.voucher_amount) < 0 then sum(tV.voucher_amount) else 0 end), (case when sum(tV.voucher_amount) > 0 then sum(tV.voucher_amount) else 0 end) from ledger_head a, voucher_details tV ,AccountsGroup_Head tG , company_head tz where " & condt & IIf(condt <> "", " and ", "") & " tV.voucher_date between @fromdate and @todate and a.ledger_idno = tV.ledger_idno and a.AccountsGroup_IdNo = tG.AccountsGroup_IdNo AND tV.Company_IdNo = tz.Company_IdNo group by a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name Having sum(tV.voucher_amount) <> 0"
            cmd.ExecuteNonQuery()

        Else

            'cmd.CommandText = "insert into reporttemp (Name1, Name2 , meters1 , int1 ,Name3 , currency1, currency2) Select a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name, (case when sum(tV.voucher_amount) < 0 then sum(tV.voucher_amount) else 0 end), (case when sum(tV.voucher_amount) > 0 then sum(tV.voucher_amount) else 0 end) from ledger_head a, voucher_details tV ,AccountsGroup_Head tG , company_head tz where " & condt & IIf(condt <> "", " and ", "") & " tV.voucher_date between @companyfromdate and @todate and a.ledger_idno = tV.ledger_idno and a.AccountsGroup_IdNo = tG.AccountsGroup_IdNo AND tV.Company_IdNo = tz.Company_IdNo group by a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name Having sum(tV.voucher_amount) <> 0"
            cmd.CommandText = "insert into reporttemp (Name1, Name2 , meters1 , int1 ,Name3 , currency1, currency2) Select a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name, (case when sum(tV.voucher_amount) < 0 then sum(tV.voucher_amount) else 0 end), (case when sum(tV.voucher_amount) > 0 then sum(tV.voucher_amount) else 0 end) from ledger_head a, voucher_details tV ,AccountsGroup_Head tG , company_head tz where " & condt & IIf(condt <> "", " and ", "") & " tV.voucher_date between @fromdate and @todate and a.ledger_idno = tV.ledger_idno and a.AccountsGroup_IdNo = tG.AccountsGroup_IdNo AND tV.Company_IdNo = tz.Company_IdNo group by a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name Having sum(tV.voucher_amount) <> 0"
            cmd.ExecuteNonQuery()

        End If



        With dgv_PrfitAndLossDetails

            .DataSource = Nothing

            .BackgroundColor = Color.White
            .BorderStyle = BorderStyle.FixedSingle

            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .AllowUserToResizeRows = False

            .DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue
            .DefaultCellStyle.SelectionForeColor = Color.Red

            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns.Clear()

            .ColumnCount = 4

            .RowHeadersVisible = False
            .AllowUserToOrderColumns = False

            .Columns(0).HeaderText = "DESCRIPTION"
            .Columns(1).HeaderText = "DR.AMOUNT"
            .Columns(2).HeaderText = "CR.AMOUNT"
            .Columns(3).HeaderText = "ledger_idno"

            .Columns(3).Visible = False


            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).FillWeight = 200
            .Columns(1).FillWeight = 100
            .Columns(2).FillWeight = 100
            .Columns(3).FillWeight = 100

            .Columns(0).DefaultCellStyle.Alignment = 1
            .Columns(1).DefaultCellStyle.Alignment = 4
            .Columns(2).DefaultCellStyle.Alignment = 4

            .Columns(0).ReadOnly = True
            .Columns(1).ReadOnly = True
            .Columns(2).ReadOnly = True
            .Columns(3).ReadOnly = True

            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable

            .Rows.Clear()
            Tot_CR = 0
            Tot_DB = 0

            '------------Group 
            N = .Rows.Add

            For Cyc = 1 To 7 Step 1

                Select Case Cyc
                    Case 1
                        N = .Rows.Add
                        .Rows(N).Cells(0).Value = "OPENING STOCK"
                        If Opn_Stock <> 0 Then
                            'If Opn_Stock > 0 Then
                            .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Opn_Stock)))
                            'Else
                            '    .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Opn_Stock)))
                            'End If
                        End If
                        Tot_DB = Tot_DB + Math.Abs(Val(Opn_Stock))
                        GoTo Cyc_Brk
                    Case 2
                        Grp_Name = Trim(lbl_PurchaseAccName.Text)
                        grp_cd = "~27~18~"
                    Case 3
                        Grp_Name = Trim(lbl_DirectExpensesName.Text)
                        grp_cd = "~15~18~"
                    Case 4
                        Grp_Name = Trim(lbl_SalesAccName.Text)
                        grp_cd = "~28~18~"
                    Case 5
                        N = .Rows.Add
                        .Rows(N).Cells(0).Value = "CLOSING STOCK"
                        If Cls_Stock <> 0 Then
                            'If Cls_Stock < 0 Then
                            '    .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Val(-1 * Cls_Stock))
                            'Else
                            .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Cls_Stock)))
                            'End If
                        End If
                        Tot_CR = Tot_CR + Math.Abs(Val(Cls_Stock))
                        supTtl = 1
                        GoTo Cyc_Brk
                    Case 6
                        Grp_Name = Trim(lbl_indirectExpenseName.Text)
                        grp_cd = "~16~18~"
                    Case 7
                        Grp_Name = Trim(lbl_IncomeName.Text)
                        grp_cd = "~19~18~"
                End Select


                Da = New SqlClient.SqlDataAdapter("select Name1, name2, sum(currency1) as DR_AMOUNT,sum(currency2) AS CR_AMOUNT from reporttemp where name1  LIKE '%" & Trim(grp_cd) & "' group by name2, Name1 order by name2, Name1", con)
                Dtbl1 = New DataTable
                Da.Fill(Dtbl1)
                Bal = 0
                If Dtbl1.Rows.Count > 0 Then
                    For i = 0 To Dtbl1.Rows.Count - 1
                        N = .Rows.Add
                        .Rows(N).Cells(0).Value = Dtbl1.Rows(i).Item("name2").ToString
                        .Rows(N).Cells(1).Value = ""
                        .Rows(N).Cells(2).Value = ""

                        If Math.Abs(Val(Dtbl1.Rows(i).Item("CR_AMOUNT").ToString)) > Math.Abs(Val(Dtbl1.Rows(i).Item("DR_AMOUNT").ToString)) Then
                            .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl1.Rows(i).Item("CR_AMOUNT").ToString)) - Math.Abs(Val(Dtbl1.Rows(i).Item("DR_AMOUNT").ToString)))
                        Else
                            .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl1.Rows(i).Item("DR_AMOUNT").ToString)) - Math.Abs(Val(Dtbl1.Rows(i).Item("CR_AMOUNT").ToString)))
                        End If


                        .Rows(N).Cells(0).Style.ForeColor = Color.Blue

                        If Val(.Rows(N).Cells(1).Value) <> 0 Then Tot_DB = Tot_DB + Math.Abs(CDbl(.Rows(N).Cells(1).Value))
                        If Val(.Rows(N).Cells(2).Value) <> 0 Then Tot_CR = Tot_CR + Math.Abs(CDbl(.Rows(N).Cells(2).Value))


                        '----------Detail

                        Da = New SqlClient.SqlDataAdapter("select  name3, sum(currency1) as DR_AMOUNT,sum(currency2) AS CR_AMOUNT from reporttemp where name1  = '" & Trim(Dtbl1.Rows(i).Item("name1").ToString) & "' GROUP BY name3", con)
                        Dtbl2 = New DataTable
                        Da.Fill(Dtbl2)
                        Bal = 0
                        If Dtbl2.Rows.Count > 0 Then
                            For k = 0 To Dtbl2.Rows.Count - 1
                                N = .Rows.Add

                                If Val(Dtbl2.Rows(k).Item("DR_AMOUNT").ToString) <> 0 Then
                                    .Rows(N).Cells(0).Value = "  " & Trim(Dtbl2.Rows(k).Item("name3").ToString) & Space(45 - Len(Trim(Dtbl2.Rows(k).Item("name3").ToString))) & Space(15 - Len(Trim(Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl2.Rows(k).Item("DR_AMOUNT").ToString)))))) & Trim(Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl2.Rows(k).Item("DR_AMOUNT").ToString)))) & IIf(Val(Dtbl2.Rows(k).Item("DR_AMOUNT").ToString) > 0, " Cr", " Dr")
                                Else
                                    .Rows(N).Cells(0).Value = "  " & Trim(Dtbl2.Rows(k).Item("name3").ToString) & Space(45 - Len(Trim(Dtbl2.Rows(k).Item("name3").ToString))) & Space(15 - Len(Trim(Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl2.Rows(k).Item("CR_AMOUNT").ToString)))))) & Trim(Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl2.Rows(k).Item("CR_AMOUNT").ToString)))) & IIf(Val(Dtbl2.Rows(k).Item("CR_AMOUNT").ToString) > 0, " Cr", " Dr")
                                End If


                                .Rows(N).Cells(0).Style.Font = New Font("Courier New", 8, FontStyle.Regular)
                                .Rows(N).Cells(0).Style.ForeColor = Color.Black

                            Next (k)
                            Dtbl2.Clear()
                            N = .Rows.Add

                        End If

                    Next i
                    Dtbl1.Clear()

                End If

Cyc_Brk:
                If Cyc = 5 Or Cyc = 7 Then
                    .Rows.Add()

                    N = .Rows.Add
                    .Rows(N).Cells(0).Value = "TOTAL"
                    .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Tot_DB)))
                    .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Tot_CR)))
                    .Rows(N).Cells(3).Value = ""

                    N = .Rows.Count - 1
                    .Rows(N).Height = 40
                    For j = 0 To .ColumnCount - 1
                        .Rows(N).Cells(j).Style.BackColor = Color.Gray
                        .Rows(N).Cells(j).Style.ForeColor = Color.White
                    Next

                    N = .Rows.Add
                    If Cyc = 5 Then

                        If Val(Math.Abs(Tot_CR)) > Val(Math.Abs(Tot_DB)) Then
                            vGrs_Prof_Loss_Amt = Val(Math.Abs(Tot_CR)) - Val(Math.Abs(Tot_DB))
                        Else
                            vGrs_Prof_Loss_Amt = -1 * (Val(Math.Abs(Tot_CR)) - Val(Math.Abs(Tot_DB)))
                        End If

                        N = .Rows.Add

                        If Val(Tot_DB) > Val(Tot_CR) Then
                            .Rows(N).Cells(0).Value = "GROSS LOSS"
                            .Rows(N).Cells(1).Value = IIf(Math.Abs(Val(Tot_DB)) > Math.Abs(Val(Tot_CR)), Common_Procedures.Currency_Format(Val(Math.Abs(Tot_DB)) - Val(Math.Abs(Tot_CR))), "")
                            .Rows(N).Cells(2).Value = IIf(Math.Abs(Val(Tot_CR)) > Math.Abs(Val(Tot_DB)), Common_Procedures.Currency_Format(Val(Math.Abs(Tot_CR)) - Val(Math.Abs(Tot_DB))), "")
                            .Rows(N).Cells(3).Value = ""
                            Tot_DB = 0
                            Tot_CR = 0
                            If Val(.Rows(N).Cells(1).Value) <> 0 Then Tot_DB = Tot_DB + Math.Abs(CDbl(.Rows(N).Cells(1).Value))
                            If Val(.Rows(N).Cells(2).Value) <> 0 Then Tot_CR = Tot_CR + Math.Abs(CDbl(.Rows(N).Cells(2).Value))

                            .Rows(N).Cells(0).Style.ForeColor = Color.Red
                            .Rows(N).Cells(2).Style.ForeColor = Color.Red
                            N = .Rows.Add

                        Else
                            .Rows(N).Cells(0).Value = "GROSS PROFIT"
                            .Rows(N).Cells(1).Value = IIf(Math.Abs(Val(Tot_DB)) > Math.Abs(Val(Tot_CR)), Common_Procedures.Currency_Format(Val(Math.Abs(Tot_DB)) - Val(Math.Abs(Tot_CR))), "")
                            .Rows(N).Cells(2).Value = IIf(Math.Abs(Val(Tot_CR)) > Math.Abs(Val(Tot_DB)), Common_Procedures.Currency_Format(Val(Math.Abs(Tot_CR)) - Val(Math.Abs(Tot_DB))), "")
                            .Rows(N).Cells(3).Value = ""


                            Tot_CR = 0 ' IIf(Val(Math.Abs(Tot_CR)) > Val(Math.Abs(Tot_DB)), Val(Math.Abs(Tot_CR)) - Val(Math.Abs(Tot_DB)), 0)
                            Tot_DB = 0
                            If Val(.Rows(N).Cells(1).Value) <> 0 Then Tot_DB = Tot_DB + Math.Abs(CDbl(.Rows(N).Cells(1).Value))
                            If Val(.Rows(N).Cells(2).Value) <> 0 Then Tot_CR = Tot_CR + Math.Abs(CDbl(.Rows(N).Cells(2).Value))


                            .Rows(N).Cells(0).Style.ForeColor = Color.DarkGreen
                            .Rows(N).Cells(2).Style.ForeColor = Color.DarkGreen
                            N = .Rows.Add
                        End If


                    ElseIf Cyc = 7 Then


                        NtTt_DB = Val(Tot_DB)
                        NtTt_CR = Val(Tot_CR)

                        'If Val(vGrs_Prof_Loss_Amt) > 0 Then
                        '    NtTt_CR = Val(NtTt_CR) + Val(vGrs_Prof_Loss_Amt)
                        'Else
                        '    NtTt_DB = Val(NtTt_DB) + Val(vGrs_Prof_Loss_Amt)
                        'End If

                        N = .Rows.Add
                        If Val(NtTt_DB) > Val(NtTt_CR) Then
                            .Rows(N).Cells(0).Value = "NET LOSS"
                            .Rows(N).Cells(1).Value = IIf(Val(NtTt_DB) > Val(NtTt_CR), Common_Procedures.Currency_Format(Val(Math.Abs(Val(NtTt_DB))) - Val(Math.Abs(Val(NtTt_CR)))), "")
                            .Rows(N).Cells(2).Value = IIf(Val(NtTt_CR) > Val(NtTt_DB), Common_Procedures.Currency_Format(Val(Math.Abs(Val(NtTt_CR))) - Val(Math.Abs(Val(NtTt_DB)))), "")
                            .Rows(N).Cells(3).Value = ""
                            NtTt_DB = 0 'IIf(Val(NtTt_db) > Val(NtTt_CR), Common_Procedures.Currency_Format(Val(NtTt_db) - Val(NtTt_CR)), 0)
                            NtTt_CR = 0
                            .Rows(N).Cells(0).Style.ForeColor = Color.Red
                            .Rows(N).Cells(2).Style.ForeColor = Color.Red

                        Else
                            .Rows(N).Cells(0).Value = "NET PROFIT"
                            .Rows(N).Cells(1).Value = IIf(Val(NtTt_DB) > Val(NtTt_CR), Common_Procedures.Currency_Format(Val(Math.Abs(Val(NtTt_DB))) - Val(Math.Abs(Val(NtTt_CR)))), "")
                            .Rows(N).Cells(2).Value = IIf(Val(NtTt_CR) > Val(NtTt_DB), Common_Procedures.Currency_Format(Val(Math.Abs(Val(NtTt_CR))) - Val(Math.Abs(Val(NtTt_DB)))), "")
                            .Rows(N).Cells(3).Value = ""
                            NtTt_CR = 0 ' IIf(Val(NtTt_CR) > Val(NtTt_db), Common_Procedures.Currency_Format(Val(NtTt_CR) - Val(NtTt_db)), 0)
                            NtTt_DB = 0
                            .Rows(N).Cells(0).Style.ForeColor = Color.DarkGreen
                            .Rows(N).Cells(2).Style.ForeColor = Color.DarkGreen

                        End If

                    End If

                End If

            Next Cyc

            N = .Rows.Add
            N = .Rows.Add


            '.Rows(0).Cells(1).Value = IIf(Val(Tot_DB) > Val(Tot_CR), Common_Procedures.Currency_Format(Val(Tot_DB) - Val(Tot_CR)), "")
            '.Rows(0).Cells(2).Value = IIf(Val(Tot_CR) > Val(Tot_DB), Common_Procedures.Currency_Format(Val(Tot_CR) - Val(Tot_DB)), "")

            N = .Rows.Count - 1
            .Rows(N).Height = 40
            For j = 0 To .ColumnCount - 1
                .Rows(N).Cells(j).Style.BackColor = Color.DarkGray
                .Rows(N).Cells(j).Style.ForeColor = Color.Red
            Next

            .Visible = True
            .BringToFront()
            .Focus()
            If .Rows.Count > 0 Then
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub



    Private Sub opt_Simple_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_Simple.CheckedChanged
        If opt_Simple.Checked = True Then
            opt_Simple.ForeColor = Color.Blue
            opt_Details.ForeColor = Color.Black
        End If
    End Sub

    Private Sub opt_Details_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_Details.CheckedChanged
        If opt_Details.Checked = True Then
            opt_Details.ForeColor = Color.Blue
            opt_Simple.ForeColor = Color.Black
        End If
    End Sub

    Private Sub Print_Selection()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False



        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        'End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        '        PpSzSTS = True
                        '        Exit For
                        '    End If
                        'Next

                        'If PpSzSTS = False Then
                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next
                        'End If

                        PrintDocument1.Print()
                    End If

                Else
                    PrintDocument1.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If


    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim W1 As Single = 0
        Dim Cmpid As Integer = 0
        Dim ShowCompCol_STS As Boolean = False

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            ShowCompCol_STS = Common_Procedures.Show_CompanyCondition_for_Report(con)
            If ShowCompCol_STS = True Then
                Cmpid = Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Inputs1.Text)))
            Else
                Cmpid = 1
            End If

            da1 = New SqlClient.SqlDataAdapter("select * from  Company_Head  where Company_IdNo = " & Str(Val(Cmpid)), con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format1(e)



    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String = ""
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(5) As Single, ClArr(2) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        ItmNm1 = ""
        ItmNm2 = ""

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 40
            .Top = 40
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin

        End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 3
8:

        Erase LnAr
        Erase ClArr

        LnAr = New Single(4) {0, 0, 0, 0, 0}
        ClArr = New Single(2) {0, 0, 0}

        ClArr(1) = PrintWidth / 2 : ClArr(2) = PrintWidth - ClArr(1)
        TxtHgt = 18.5


        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                CurY = CurY + TxtHgt
                ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            '  Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        '' ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ''ItmNm2 = ""
                        'If Len(ItmNm1) > 18 Then
                        '    For I = 18 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 18
                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                'Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim CurY_Temp As Decimal = 0
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Sales_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " PROFIT & LOSS A/C", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "As on " & Convert.ToDateTime(dtp_ToDate.Text), LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        Try
            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "LIABILITIES", LMargin, CurY, 2, ClAr(1), p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "ASSETS", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin + 10, CurY, 0, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "D.AMOUNT", LMargin + 360, CurY, 1, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin + ClAr(1) + 10, CurY, 0, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "CR.AMOUNT", LMargin + ClAr(1) + 360, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            '-------------- Debit
            If Val(lbl_OpeningStock_Value.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Opening Stock", CDbl(lbl_OpeningStock_Value.Text), LMargin + 10, CurY, ClAr(1))
            End If
            If Val(lbl_PurchaseAccValue.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Purchase A/C", CDbl(lbl_PurchaseAccValue.Text), LMargin + 10, CurY, ClAr(1))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~27~18~", LMargin + 10, CurY)
            End If
            If Val(lbl_DirectExpensesValue.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Direct Expenses", CDbl(lbl_DirectExpensesValue.Text), LMargin + 10, CurY, ClAr(1))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~15~18~", LMargin + 10, CurY)
            End If
            If lbl_GrossProfitValueDb.Visible Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Gross Profit c/o", CDbl(lbl_GrossProfitValueDb.Text), LMargin + 10, CurY, ClAr(1))
            End If


            '---------------Credit
            CurY_Temp = CurY
            CurY = LnAr(3)

            If Val(lbl_SalesAccValue.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Sales A/C", CDbl(lbl_SalesAccValue.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~28~18~", LMargin + ClAr(1), CurY)
            End If
            If Val(lbl_ClosingStockValue.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Closing Stock", CDbl(lbl_ClosingStockValue.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
            End If
            If lbl_GrossLossValueCr.Visible Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Gross Loss c/o", CDbl(lbl_GrossLossValueCr.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
            End If

            '----------Sub total
            If CurY > CurY_Temp Then
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, ClAr(1) - 80, CurY, ClAr(1) + 40, CurY)
                e.Graphics.DrawLine(Pens.Black, PrintWidth - 80, CurY, PrintWidth + 40, CurY)

                CurY = CurY + 10
                If Val(lbl_GrossTotalDb.Text) <> 0 Then
                    Print_Heading(e, "", CDbl(lbl_GrossTotalDb.Text), LMargin + 10, CurY, ClAr(1))
                End If
                If Val(lbl_GrossTotalDb.Text) <> 0 Then
                    Print_Heading(e, "", CDbl(lbl_GrossTotalDb.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
                End If
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, ClAr(1) - 80, CurY, ClAr(1) + 40, CurY)
                e.Graphics.DrawLine(Pens.Black, PrintWidth - 80, CurY, PrintWidth + 40, CurY)
                LnAr(4) = CurY
                CurY_Temp = 0
            Else
                CurY_Temp = CurY_Temp + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, ClAr(1) - 80, CurY_Temp, ClAr(1) + 40, CurY_Temp)
                e.Graphics.DrawLine(Pens.Black, PrintWidth - 80, CurY_Temp, PrintWidth + 40, CurY_Temp)

                CurY_Temp = CurY_Temp + 10
                If Val(lbl_GrossTotalDb.Text) <> 0 Then
                    Print_Heading(e, "", CDbl(lbl_GrossTotalDb.Text), LMargin + 10, CurY_Temp, ClAr(1))
                End If
                If Val(lbl_GrossTotalDb.Text) <> 0 Then
                    Print_Heading(e, "", CDbl(lbl_GrossTotalDb.Text), LMargin + ClAr(1) + 10, CurY_Temp, ClAr(2))
                End If
                CurY_Temp = CurY_Temp + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, ClAr(1) - 80, CurY_Temp, ClAr(1) + 40, CurY_Temp)
                e.Graphics.DrawLine(Pens.Black, PrintWidth - 80, CurY_Temp, PrintWidth + 40, CurY_Temp)
                CurY = CurY_Temp
                LnAr(4) = CurY
            End If
           

            '--------Debit

            If lbl_GrossLossDbValue.Visible Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Gross Loss b/f", CDbl(lbl_GrossLossDbValue.Text), LMargin + 10, CurY, ClAr(1))
            End If

            If Val(lbl_indirectExpenseValue.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Indirect Expenses", CDbl(lbl_indirectExpenseValue.Text), LMargin + 10, CurY, ClAr(1))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~16~18~", LMargin + 10, CurY)
            End If

            If lbl_NetProfitValueDb.Visible Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Net Profit", CDbl(lbl_NetProfitValueDb.Text), LMargin + 10, CurY, ClAr(1))
            End If


            '---------- Credit
            CurY_Temp = CurY
            CurY = LnAr(4)

            If lbl_GrossProfitValueDb.Visible Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Gross Profit b/f", CDbl(lbl_GrossProfitValueDb.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
            End If
            If Val(lbl_IncomeValue.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Indirect Income", CDbl(lbl_IncomeValue.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~19~18~", LMargin + ClAr(1), CurY)
            End If
            If lbl_NetLossValueCr.Visible Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Net Loss", CDbl(lbl_NetLossValueCr.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
            End If


            If CurY > CurY_Temp Then
                CurY = CurY + 50
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
                e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(4) = CurY
            Else
                CurY_Temp = CurY_Temp + 50
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY_Temp, LMargin + ClAr(1), LnAr(2))
                e.Graphics.DrawLine(Pens.Black, PageWidth, CurY_Temp, PageWidth, LnAr(1))
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY_Temp, LMargin, LnAr(1))
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY_Temp, PageWidth, CurY_Temp)
                LnAr(4) = CurY_Temp
                CurY = CurY_Temp
            End If
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 10, CurY, 0, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(lbl_TotalDb.Text), LMargin + 360, CurY, 1, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 10, CurY, 0, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(lbl_TotalCr.Text), LMargin + ClAr(1) + 360, CurY, 1, 0, p1Font)

            CurY = CurY + 30
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(5) = CurY
        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub



    Private Sub Print_Details(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal gp_id As String, ByVal cur_x As Decimal, ByRef cur_y As Decimal)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim cmd As New SqlClient.SqlCommand
        Dim dt2 As New DataTable
        Dim dt1 As New DataTable
        Dim Tw As Integer
        Dim TXT As Double = 0
        Dim pFont As Font
        Dim p1Font As Font
        Dim condt As String = ""
        Dim C_X2 As Decimal = 0

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", dtp_ToDate.Value.Date)

        pFont = New Font("Arial", 7, FontStyle.Regular)
        p1Font = New Font("Draft 17cpi", 9, FontStyle.Regular)


        da2 = New SqlClient.SqlDataAdapter("Select AccountsGroup_Name, AccountsGroup_IdNo, Parent_Idno from AccountsGroup_Head where Parent_Idno = '~'+cast(AccountsGroup_IdNo as varchar(20))+'" & Trim(gp_id) & "' order by AccountsGroup_Name", con)
        da2.Fill(dt2)
        If dt2.Rows.Count <> 0 Then

            For d = 0 To dt2.Rows.Count - 1

                da2 = New SqlClient.SqlDataAdapter("Select sum(currency1) as SumOfAmount from reporttemp where name1 like '%" & Trim(dt2.Rows(d).Item("Parent_Idno").ToString) & "%' having sum(currency1) <> 0", con)
                da2.Fill(dt1)
                If dt1.Rows.Count <> 0 Then
                    If Not IsDBNull(dt1.Rows(0).Item("SumOfAmount").ToString) Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(dt2.Rows(d).Item("AccountsGroup_Name").ToString), cur_x + 20, cur_y, 0, 0, pFont)
                        TXT = Val(dt1.Rows(0).Item("SumOfAmount").ToString)
                        Tw = e.Graphics.MeasureString(TXT, p1Font).Width
                        C_X2 = cur_x + 250
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("SumOfAmount").ToString)), C_X2, cur_y, 1, 0, p1Font)
                        cur_y = cur_y + 19
                    End If

                End If
                dt1.Clear()
            Next d
        End If
        dt2.Clear()



        cmd.CommandText = "Select a.ledger_name, a.ledger_idno, sum(tz.voucher_amount) as total from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date <= @uptodate and a.parent_code = '" & Trim(gp_id) & "' and a.ledger_idno = tz.ledger_idno group by a.ledger_idno, a.ledger_name order by a.ledger_name"
        da2 = New SqlClient.SqlDataAdapter(cmd)
        da2.Fill(dt1)
        'da2 = New SqlClient.SqlDataAdapter("Select a.ledger_name, a.ledger_idno, sum(tz.voucher_amount) as total from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date <= @update and a.parent_code = '" & Trim(gp_id) & "' and a.ledger_idno = tz.ledger_idno group by a.ledger_idno, a.ledger_name order by a.ledger_name", con)
        'da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            For d2 = 0 To dt2.Rows.Count - 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(dt2.Rows(d2).Item("ledger_name").ToString), cur_x, cur_y, 0, 0, p1Font)

                Tw = e.Graphics.MeasureString(CDbl(dt2.Rows(d2).Item("total").ToString), pFont).Width
                C_X2 = cur_x + 200
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(dt2.Rows(d2).Item("total").ToString)), C_X2, cur_y, 1, 0, p1Font)
                cur_y = cur_y + 19
            Next
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, C_X2, cur_y, C_X2 - 100, cur_y)


    End Sub

    Private Sub Print_Heading(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal g_Name As String, ByVal Amount As Double, ByVal c_x As Decimal, ByVal c_y As Decimal, ByVal ClArr As Decimal)
        Dim Tw As Integer
        Dim pFont As Font
        Dim p1Font As Font

        pFont = New Font("Calibri", 12, FontStyle.Bold)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Trim(g_Name), c_x, c_y, 0, 0, p1Font)

        Tw = e.Graphics.MeasureString(Common_Procedures.Currency_Format(CDbl(Amount)), pFont).Width + e.Graphics.MeasureString(Trim(Amount), pFont).Width

        c_x = c_x + 350
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(CDbl(Amount)), c_x, c_y, 1, 10, pFont)
    End Sub
    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub lbl_PurchaseAccValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_PurchaseAccValue.Click, lbl_PurchaseAccName.Click
        txt_Selection.Text = 1
        Label_Selection(2)
    End Sub

    Private Sub lbl_DirectExpensesValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_DirectExpensesValue.Click, lbl_DirectExpensesName.Click
        txt_Selection.Text = 2
        Label_Selection(2)
    End Sub

    Private Sub lbl_indirectExpenseValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_indirectExpenseValue.Click, lbl_indirectExpenseName.Click
        txt_Selection.Text = 3
        Label_Selection(2)
    End Sub

    Private Sub lbl_SalesAccValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SalesAccValue.Click, lbl_SalesAccName.Click
        txt_Selection.Text = 4
        Label_Selection(2)
    End Sub

    Private Sub lbl_IncomeValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_IncomeValue.Click, lbl_IncomeName.Click
        txt_Selection.Text = 5
        Label_Selection(2)
    End Sub

    Private Sub lbl_PurchaseAccName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_PurchaseAccName.DoubleClick, lbl_PurchaseAccValue.DoubleClick
        txt_Selection.Text = 1
        Label_Selection(2)
        get_Details(1)
    End Sub

    Private Sub lbl_DirectExpensesName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_DirectExpensesName.DoubleClick, lbl_DirectExpensesValue.DoubleClick
        txt_Selection.Text = 2
        Label_Selection(2)
        get_Details(2)
    End Sub

    Private Sub lbl_indirectExpenseName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_indirectExpenseName.DoubleClick, lbl_indirectExpenseValue.DoubleClick
        txt_Selection.Text = 3
        Label_Selection(2)
        get_Details(3)
    End Sub

    Private Sub lbl_SalesAccName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SalesAccName.DoubleClick, lbl_SalesAccValue.DoubleClick
        txt_Selection.Text = 4
        Label_Selection(2)
        get_Details(4)
    End Sub

    Private Sub lbl_IncomeName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_IncomeName.DoubleClick, lbl_IncomeValue.DoubleClick
        txt_Selection.Text = 5
        Label_Selection(2)
        get_Details(5)
    End Sub

    Private Sub WeightBridge_Entry_Pending_Report_Printing()
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        PrintDocument2.DefaultPageSettings.Landscape = False
        For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                PrintDocument2.DefaultPageSettings.PaperSize = ps
                PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument2.DocumentName = "Invoice"
                    PrintDocument2.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument2.PrinterSettings.PrintFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\Report.pdf"
                    PrintDocument2.Print()

                Else

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument2.Print()
                        End If

                    Else
                        PrintDocument2.Print()

                    End If

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument2

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim Hgt As Double = 0
        'Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        'Dim PrintWidth As Single, PrintHeight As Single
        'Dim PageWidth As Single, PageHeight As Single

        PrintDocument2.DefaultPageSettings.Landscape = False

        Try

            'da1 = New SqlClient.SqlDataAdapter("select  c.* from  Company_Head c  where C.Company_IdNo = " & Str(Val(lbl_Company.Tag)), con)
            'prn_HdDt = New DataTable
            'da1.Fill(prn_HdDt)


            prn_DetMxIndx = 0
            prn_pageHeight = 0
            prn_PageNo = 0


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            da1.Dispose()


        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        PrintDocument2.DefaultPageSettings.Landscape = False
        WeightBridge_Entry_Pending_Report_Printing_Format1(e)
    End Sub

    Private Sub WeightBridge_Entry_Pending_Report_Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim HClm As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PS As Printing.PaperSize

        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single


        PrintDocument2.DefaultPageSettings.Landscape = False


        For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                PS = PrintDocument2.PrinterSettings.PaperSizes(I)
                PrintDocument2.DefaultPageSettings.PaperSize = PS
                e.PageSettings.PaperSize = PS
                Exit For
            End If
        Next

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 50  ' 50 
            .Right = 50
            .Top = 40   '30
            .Bottom = 50 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        With PrintDocument2.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        If PrintDocument2.DefaultPageSettings.Landscape = True Then
            With PrintDocument2.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If



        PrintDocument2.DefaultPageSettings.Landscape = False


        pFont = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        TxtHgt = 19.75

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClArr(0) = 0

        CurY = TMargin
        prn_pageHeight = CurY

        Dim j As Integer = 0

        For i = 0 To dgv_PrfitAndLossDetails.Columns.Count - 2
            If dgv_PrfitAndLossDetails.Columns(i).Width < 20 Or dgv_PrfitAndLossDetails.Columns(i).Visible = False Then
                ClArr(i + 1) = ClArr(j)
            Else
                If i = 0 Then
                    ClArr(i + 1) = ClArr(j) + 400 ' dgv_PrfitAndLossDetails.Columns(i).Width
                Else
                    ClArr(i + 1) = ClArr(j) + 150
                End If
                'ClArr(i + 1) = ClArr(j) + dgv_PrfitAndLossDetails.Columns(i).Width
            End If
            j = i + 1
        Next i
        prn_PageNo = prn_PageNo + 1


        WeightBridge_Entry_Pending_ReportPrinting_PageHeader(e, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, 1, 0, CurY, LnAr, ClArr)

        CurY = CurY + 10
        prn_pageHeight = CurY

        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        For i = prn_DetMxIndx To dgv_PrfitAndLossDetails.Rows.Count - 1

            'If Trim(dgv_PrfitAndLossDetails.Rows(i).Cells(7).Value.ToString) <> False Then
            '    prn_DetMxIndx = prn_DetMxIndx + 1
            '    Continue For
            'End If

            CurY = CurY + 5
            prn_pageHeight = CurY

            For j = 0 To dgv_PrfitAndLossDetails.Columns.Count - 2
                'If Trim(dgv_PrfitAndLossDetails.Rows(i).Cells(7).Value.ToString) = False Then
                'If Convert.ToBoolean(dgv_PrfitAndLossDetails.Rows(i).Cells(7).Value.ToString) = False Then
                If Not (dgv_PrfitAndLossDetails.Columns(j).Width < 20 Or dgv_PrfitAndLossDetails.Columns(j).Visible = False) Then
                    If dgv_PrfitAndLossDetails.Rows(i).Cells(0).Style.ForeColor = Color.Blue Or dgv_PrfitAndLossDetails.Rows(i).Cells(0).Style.ForeColor = Color.DarkGreen Or dgv_PrfitAndLossDetails.Rows(i).Cells(0).Style.ForeColor = Color.Red Or dgv_PrfitAndLossDetails.Rows(i).Cells(0).Style.ForeColor = Color.White Then
                        p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Else
                        p1Font = New Font("Courier New", 7, FontStyle.Regular)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Trim(dgv_PrfitAndLossDetails.Rows(i).Cells(j).Value), LMargin + ClArr(j) + 10, CurY, 0, 0, p1Font)
                End If
                'End If
                'End If


            Next j
            CurY = CurY + TxtHgt
            prn_pageHeight = CurY
            If prn_pageHeight > PrintHeight - 30 Then
                prn_pageHeight = 0
                prn_pageHeight = 0
                ' CurY = CurY + TxtHgt
                '   Common_Procedures.Print_To_PrintDocument(e, "Continued....", ClArr(7), CurY, 1, 0, pFont)
                WeightBridge_Entry_Pending_ReportPrinting_PageFooter(e, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, 1000, CurY, LnAr, ClArr, "", True)
                e.HasMorePages = True
                Return
            End If
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            prn_DetMxIndx = prn_DetMxIndx + 1
        Next i



        If prn_DetMxIndx > 0 Then
            If prn_pageHeight > PrintHeight - 30 Then
                prn_pageHeight = 0
                ' CurY = CurY + TxtHgt
                ' Common_Procedures.Print_To_PrintDocument(e, "Continued....", ClArr(7), CurY, 1, 0, pFont)
                WeightBridge_Entry_Pending_ReportPrinting_PageFooter(e, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, 1000, CurY, LnAr, ClArr, "", True)
                e.HasMorePages = True

                Return
            End If

        End If

        WeightBridge_Entry_Pending_ReportPrinting_PageFooter(e, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, 1000, CurY, LnAr, ClArr, "", True)

        e.HasMorePages = False
    End Sub

    Private Sub WeightBridge_Entry_Pending_ReportPrinting_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal HeightperPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim LedNmAr(10) As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim CompName As String = ""
        Dim CompAdd1 As String = ""
        Dim CompAdd2 As String = ""
        Dim CompCondt As String = ""


        ' prn_PageNo = prn_PageNo + 1

        CurY = TMargin
        Try

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            '  LnAr(1) = CurY

            If prn_PageNo = 1 Then


                CompName = ""
                CompAdd1 = ""
                CompAdd2 = ""

                If cbo_Inputs1.Visible = True And Trim(cbo_Inputs1.Text) <> "" And Trim(UCase(RptCboDet(1).PKey)) = "Z" Then

                    Da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name, Company_Address1, Company_Address2, Company_Address3, Company_Address4 from Company_Head Where Company_ShortName = '" & Trim(cbo_Inputs1.Text) & "' Order by Company_IdNo ", con)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            CompName = Dt.Rows(0).Item("Company_Name").ToString
                            CompAdd1 = Dt.Rows(0).Item("Company_Address1").ToString & " " & Dt.Rows(0).Item("Company_Address2").ToString
                            CompAdd2 = Dt.Rows(0).Item("Company_Address3").ToString & " " & Dt.Rows(0).Item("Company_Address4").ToString
                        End If
                    End If
                    Dt.Clear()

                Else

                    CompCondt = ""
                    If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                        CompCondt = "(Company_Type <> 'UNACCOUNT')"
                    End If

                    Da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name, Company_Address1, Company_Address2, Company_Address3, Company_Address4 from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo ", con)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            CompName = Dt.Rows(0).Item("Company_Name").ToString
                            CompAdd1 = Dt.Rows(0).Item("Company_Address1").ToString & " " & Dt.Rows(0).Item("Company_Address2").ToString
                            CompAdd2 = Dt.Rows(0).Item("Company_Address3").ToString & " " & Dt.Rows(0).Item("Company_Address4").ToString
                        End If
                    End If
                    Dt.Clear()

                End If

                CurY = CurY + TxtHgt
                prn_pageHeight = CurY
                p1Font = New Font("Calibri", 15, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, CompName, LMargin, CurY, 2, PrintWidth, p1Font)

                CurY = CurY + TxtHgt + 10
                prn_pageHeight = CurY
                p1Font = New Font("Calibri", 10, FontStyle.Regular)

                Common_Procedures.Print_To_PrintDocument(e, CompAdd1, LMargin, CurY, 2, PrintWidth, p1Font)

                CurY = CurY + TxtHgt
                prn_pageHeight = CurY

                Common_Procedures.Print_To_PrintDocument(e, CompAdd2, LMargin, CurY, 2, PrintWidth, p1Font)

                CurY = CurY + TxtHgt + 3
                prn_pageHeight = CurY
                p1Font = New Font("Calibri", 13, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "PROFIT & LOSS", LMargin, CurY, 2, PrintWidth, p1Font)

                CurY = CurY + TxtHgt
                prn_pageHeight = CurY
                p1Font = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "DATE RANGE FROM " & dtp_FromDate.Text & " TO " & dtp_ToDate.Text, LMargin, CurY, 2, PrintWidth, p1Font)

                CurY = CurY + TxtHgt
                prn_pageHeight = CurY

            End If

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            prn_pageHeight = CurY

            For i = 0 To dgv_PrfitAndLossDetails.Columns.Count - 2
                If Not (dgv_PrfitAndLossDetails.Columns(i).Width < 20 Or dgv_PrfitAndLossDetails.Columns(i).Visible = False) Then

                    Common_Procedures.Print_To_PrintDocument(e, Trim(dgv_PrfitAndLossDetails.Columns(i).HeaderText.ToString), LMargin + ClArr(i) + 5, CurY, 2, 0, p1Font)

                End If

            Next i

            CurY = CurY + 20
            prn_pageHeight = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY



        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub WeightBridge_Entry_Pending_ReportPrinting_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal HeightPerPage As Double, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal Cmp_Name As String, ByVal is_LastPage As Boolean)
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""

        Try

            CurY = CurY + TxtHgt
            prn_pageHeight = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(4), LMargin, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(4), PageWidth, LnAr(2))

            For I = 0 To dgv_PrfitAndLossDetails.Columns.Count - 2

                If dgv_PrfitAndLossDetails.Columns(I).Width < 20 Or dgv_PrfitAndLossDetails.Columns(I).Visible = False Then
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(I), LnAr(2), LMargin + ClArr(I), LnAr(4))
                Else
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(I), LnAr(2), LMargin + ClArr(I), LnAr(4))
                End If

            Next I

            CurY = CurY + 10
            prn_pageHeight = CurY

            Common_Procedures.Print_To_PrintDocument(e, "Page No." & prn_PageNo, PageWidth - 100, CurY, 2, 0, pFont)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


End Class