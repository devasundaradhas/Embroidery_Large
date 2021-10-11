Public Class Balance_Sheet
    Implements Interface_MDIActions
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private Opn_Stock As Double = 0
    Private Cls_Stock As Double = 0
    Private Net_Profit As Double = 0
    Private Net_Loss As Double = 0
    Private FrmLdSTS As Boolean = False
    Private prn_pageHeight As Double = 0
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer = 0


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
    Private RptCboDet(10) As Report_ComboDetails

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer = 0


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

        lbl_CapitalAcc.Text = "0.00"
        lbl_CurrentLiabilities.Text = "0.00"
        lbl_LoansLiabilities.Text = "0.00"
        lbl_BranchDivisions.Text = "0.00"
        lbl_FixedAssets.Text = "0.00"
        lbl_Investments.Text = "0.00"
        lbl_CurrentAssets.Text = "0.00"
        lbl_SuspenseAcc.Text = "0.00"
        lbl_MiscExpenses.Text = "0.00"
        lbl_TotalLiabilities.Text = "0.00"
        lbl_TotalAssets.Text = "0.00"

        lbl_NetProfit.Visible = False
        lbl_NetProfitName.Visible = False
        lbl_Netloss.Visible = False
        lbl_NetLossName.Visible = False
        lbl_OpeningDiffDB.Visible = False
        lbl_OpeningDiffCR.Visible = False
        lbl_OpeningDiffNameDB.Visible = False
        lbl_OpeningDiffNameCR.Visible = False
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


    Public Sub save_record() Implements Interface_MDIActions.save_record
        '-----------------
    End Sub



    Public Sub Show_Report()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CompCondt As String = ""
        Dim RepSTS As Boolean = False

        Try

            Balance_Sheet_Calculation()
            If txt_Selection.Visible And txt_Selection.Enabled Then txt_Selection.Focus()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub



    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub Balance_Sheet_Calculation()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim condt As String = ""
        Dim VouAmt As Double = 0
        Dim db_amt As Double = 0
        Dim cr_amt As Double = 0

        condt = Company_Condition()

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        Cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        Cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        Cmd.CommandText = "truncate table reporttemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into reporttemp ( name1, currency1 ) Select a.parent_code, sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date <= @todate and a.ledger_idno = tz.ledger_idno group by a.parent_code Having sum(tz.voucher_amount) <> 0"
        Cmd.ExecuteNonQuery()


        '----CAPITAL ACCOUNTS
        VouAmt = get_VoucherSummary("~2~")
        lbl_CapitalAcc.Text = Common_Procedures.Currency_Format(Val(VouAmt))
        If Val(lbl_CapitalAcc.Text) <> 0 Then db_amt = db_amt + Common_Procedures.Currency_Format(CDbl(lbl_CapitalAcc.Text))


        '----LOAN (LIABILITIES)
        VouAmt = get_VoucherSummary("~21~")
        lbl_LoansLiabilities.Text = Common_Procedures.Currency_Format(Val(VouAmt))
        If Val(lbl_LoansLiabilities.Text) <> 0 Then db_amt = db_amt + Common_Procedures.Currency_Format(CDbl(lbl_LoansLiabilities.Text))

        '-----CURRENT LIABILITIES
        VouAmt = get_VoucherSummary("~11~")
        lbl_CurrentLiabilities.Text = Common_Procedures.Currency_Format(Val(VouAmt))
        If Val(lbl_CurrentLiabilities.Text) <> 0 Then db_amt = db_amt + Common_Procedures.Currency_Format(CDbl(lbl_CurrentLiabilities.Text))

        '----BRANCH / DIVISIONS
        VouAmt = get_VoucherSummary("~1~")
        lbl_BranchDivisions.Text = Common_Procedures.Currency_Format(Val(VouAmt))
        If Val(lbl_BranchDivisions.Text) <> 0 Then db_amt = db_amt + Common_Procedures.Currency_Format(CDbl(lbl_BranchDivisions.Text))

        '-----FIXED ASSETS
        VouAmt = get_VoucherSummary("~17~")
        lbl_FixedAssets.Text = Common_Procedures.Currency_Format(-1 * Val(VouAmt))
        If Val(lbl_FixedAssets.Text) <> 0 Then cr_amt = cr_amt + Common_Procedures.Currency_Format(CDbl(lbl_FixedAssets.Text))

        '-----INVESTMENTS
        VouAmt = get_VoucherSummary("~20~")
        lbl_Investments.Text = Common_Procedures.Currency_Format(-1 * Val(VouAmt))
        If Val(lbl_Investments.Text) <> 0 Then cr_amt = cr_amt + Common_Procedures.Currency_Format(CDbl(lbl_Investments.Text))

        '-----CURRENT ASSETS
        VouAmt = Voucher_Summary_ForCurrentAsset()
        lbl_CurrentAssets.Text = Common_Procedures.Currency_Format(-1 * Val(VouAmt))
        If Val(lbl_CurrentAssets.Text) <> 0 Then cr_amt = cr_amt + Common_Procedures.Currency_Format(CDbl(lbl_CurrentAssets.Text))

        '-----SUSPENSE A/C
        VouAmt = get_VoucherSummary("~29~")
        lbl_SuspenseAcc.Text = Common_Procedures.Currency_Format(-1 * Val(VouAmt))
        If Val(lbl_SuspenseAcc.Text) <> 0 Then cr_amt = cr_amt + Common_Procedures.Currency_Format(CDbl(lbl_SuspenseAcc.Text))

        '-----MISC.EXPENSES (ASSET)
        VouAmt = get_VoucherSummary("~26~")
        lbl_MiscExpenses.Text = Common_Procedures.Currency_Format(-1 * Val(VouAmt))
        If Val(lbl_MiscExpenses.Text) <> 0 Then cr_amt = cr_amt + Common_Procedures.Currency_Format(CDbl(lbl_MiscExpenses.Text))


        Profit_AND_LOSS_Calculation()


        'VouAmt = 0
        'VouAmt = NetProfitOrLoss_Calculation()
        'If VouAmt >= 0 Then
        '    lbl_Netloss.Visible = False
        '    lbl_NetLossName.Visible = False

        '    lbl_NetProfitName.Visible = True
        '    lbl_NetProfit.Visible = True
        '    lbl_NetProfit.ForeColor = Color.Green
        '    lbl_NetProfitName.ForeColor = Color.Green

        '    Net_Profit = Common_Procedures.Currency_Format(Val(VouAmt))
        '    lbl_NetProfit.Text = Common_Procedures.Currency_Format(Val(VouAmt))

        'Else
        '    lbl_NetProfitName.Visible = False
        '    lbl_NetProfit.Visible = False

        '    lbl_Netloss.Visible = True
        '    lbl_NetLossName.Visible = True
        '    lbl_Netloss.ForeColor = Color.Red
        '    lbl_NetLossName.ForeColor = Color.Red

        '    Net_Loss = Common_Procedures.Currency_Format(Val(VouAmt))
        '    lbl_Netloss.Text = Common_Procedures.Currency_Format(Val(VouAmt))

        'End If

        '----TOTAL AND OPENING DIFF CALCULATION

        lbl_TotalLiabilities.Text = 0
        lbl_TotalAssets.Text = 0
        lbl_OpeningDiffCR.Text = 0
        lbl_OpeningDiffDB.Text = 0

        lbl_TotalLiabilities.Text = Common_Procedures.Currency_Format(CDbl(lbl_CapitalAcc.Text) + CDbl(lbl_LoansLiabilities.Text) + CDbl(lbl_CurrentLiabilities.Text) + CDbl(lbl_BranchDivisions.Text) + CDbl(lbl_NetProfit.Text) + CDbl(lbl_OpeningDiffDB.Text))
        lbl_TotalAssets.Text = Common_Procedures.Currency_Format(CDbl(lbl_FixedAssets.Text) + CDbl(lbl_Investments.Text) + CDbl(lbl_CurrentAssets.Text) + CDbl(lbl_SuspenseAcc.Text) + CDbl(lbl_MiscExpenses.Text) + CDbl(lbl_Netloss.Text) + CDbl(lbl_OpeningDiffCR.Text))

        If CDbl(lbl_TotalLiabilities.Text) > CDbl(lbl_TotalAssets.Text) Then

            lbl_OpeningDiffCR.Text = Common_Procedures.Currency_Format(CDbl(lbl_TotalLiabilities.Text) - CDbl(lbl_TotalAssets.Text))
            lbl_TotalAssets.Text = Common_Procedures.Currency_Format(CDbl(lbl_TotalLiabilities.Text))
            lbl_OpeningDiffDB.Visible = False
            lbl_OpeningDiffNameDB.Visible = False
            lbl_OpeningDiffNameCR.Visible = True
            lbl_OpeningDiffCR.Visible = True

        ElseIf CDbl(lbl_TotalLiabilities.Text) < CDbl(lbl_TotalAssets.Text) Then

            lbl_OpeningDiffDB.Text = Common_Procedures.Currency_Format(CDbl(lbl_TotalAssets.Text) - CDbl(lbl_TotalLiabilities.Text))
            lbl_TotalLiabilities.Text = Common_Procedures.Currency_Format(CDbl(lbl_TotalAssets.Text))
            lbl_OpeningDiffDB.Visible = True
            lbl_OpeningDiffNameDB.Visible = True
            lbl_OpeningDiffNameCR.Visible = False
            lbl_OpeningDiffCR.Visible = False

        Else
            lbl_OpeningDiffDB.Visible = False
            lbl_OpeningDiffNameDB.Visible = False
            lbl_OpeningDiffNameCR.Visible = False
            lbl_OpeningDiffCR.Visible = False

        End If

    End Sub

    Private Function NetProfitOrLoss_Calculation() As Double

        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim condt As String = ""
        Dim VouAmt As Double = 0

        condt = Company_Condition()
        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        Cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        Cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)


        Cmd.CommandText = "truncate table reporttemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into reporttemp ( name1, currency1 ) Select a.parent_code, sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date <= @todate and a.ledger_idno = tz.ledger_idno group by a.parent_code Having sum(tz.voucher_amount) <> 0"
        Cmd.ExecuteNonQuery()

        VouAmt = 0
        Cmd.CommandText = "Select sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & "  tz.voucher_date between @fromdate  and @todate and ( a.parent_code like '%~18~%' ) and a.ledger_idno = tz.ledger_idno"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                VouAmt = VouAmt + Val(Dt1.Rows(0)(0).ToString)
            End If
        End If
        Dt1.Clear()

        Cmd.CommandText = "Select sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date < @fromdate and a.parent_code like '%~9~4~%' and a.ledger_idno = tz.ledger_idno"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                VouAmt = VouAmt + Val(Dt1.Rows(0)(0).ToString)
            End If
        End If
        Dt1.Clear()

        Cmd.CommandText = "Select sum(Closing_Stock_Value) from Closing_Stock_Value_Head tz where " & condt & IIf(condt <> "", " and ", "") & " tz.Closing_Stock_Value_Date <= @todate"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                VouAmt = VouAmt + Val(Dt1.Rows(0)(0).ToString)
            End If
        End If
        Dt1.Clear()

        Cmd.CommandText = "Select sum(voucher_amount) from voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.ledger_idno = 13 and voucher_date <= @todate    and year_for_report < @todate"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                VouAmt = VouAmt + Val(Dt1.Rows(0)(0).ToString)
            End If
        End If
        Dt1.Clear()


        NetProfitOrLoss_Calculation = VouAmt

    End Function

    Private Sub Profit_AND_LOSS_Calculation()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim condt As String = ""
        Dim VouAmt As Double = 0
        Dim db_amt As Double = 0
        Dim cr_amt As Double = 0

        condt = Company_Condition()

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        Cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        Cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        Cmd.CommandText = "truncate table reporttemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into reporttemp ( name1, currency1 ) Select a.parent_code, sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " tz.voucher_date between @companyfromdate and @todate and a.ledger_idno = tz.ledger_idno group by a.parent_code Having sum(tz.voucher_amount) <> 0"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "truncate table reporttempsub"
        Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "insert into reporttempsub ( date1, currency1 ) Select tZ.voucher_date, sum(tz.voucher_amount) from ledger_head a, voucher_details tz where " & condt & IIf(condt <> "", " and ", "") & " a.parent_code like '%~9~4~%' and tz.voucher_date <= @todate and a.ledger_idno = tz.ledger_idno group by tZ.voucher_date Having sum(tz.voucher_amount) <> 0"
        'Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into reporttempsub ( date1, currency1 ) Select tZ.Closing_Stock_Value_Date, sum(tz.Closing_Stock_Value) from Closing_Stock_Value_Head tz where " & condt & IIf(condt <> "", " and ", "") & " tz.Closing_Stock_Value_Date <= @todate group by tZ.Closing_Stock_Value_Date Having sum(tz.Closing_Stock_Value) <> 0"
        Cmd.ExecuteNonQuery()

        '----OPENING STOCK
        Cmd.CommandText = "Select top 1 Currency1 as OpStockValue from ReportTempSub where date1 <= @fromdate Order by date1 desc"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then

                db_amt = db_amt + Common_Procedures.Currency_Format(Val(Dt1.Rows(0)(0).ToString))

            End If
        End If
        Dt1.Clear()


        '----PURCHASE ACCOUNTS
        VouAmt = get_VoucherSummary("~27~18~")
        db_amt = db_amt + Common_Procedures.Currency_Format(Val(-1 * VouAmt))

        '-----DIRECT EXPENSES
        VouAmt = get_VoucherSummary("~15~18~")
        db_amt = db_amt + Common_Procedures.Currency_Format(Val(-1 * VouAmt))

        '----SALES ACCOUNTS
        VouAmt = get_VoucherSummary("~28~18~")
        cr_amt = cr_amt + Common_Procedures.Currency_Format(Val(VouAmt))

        '-----CLOSING STOCK
        Cmd.CommandText = "Select top 1 Currency1 as OpStockValue from ReportTempSub where date1 <= @todate Order by date1 desc"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        Cls_Stock = 0
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                cr_amt = cr_amt + Common_Procedures.Currency_Format(Val(Dt1.Rows(0)(0).ToString))
                Cls_Stock = Common_Procedures.Currency_Format(Val(Dt1.Rows(0)(0).ToString))
            End If
        End If
        Dt1.Clear()

        If cr_amt >= db_amt Then  '------------Gross Profit
            cr_amt = cr_amt - db_amt
            db_amt = 0
        Else                      '------------Gross Loss

            db_amt = db_amt - cr_amt
            cr_amt = 0
        End If

        '-------INDIRECT EXPENSES
        VouAmt = get_VoucherSummary("~16~18~")
        db_amt = db_amt + Common_Procedures.Currency_Format(-1 * Val(VouAmt))

        '-------INCOME (REVENUE)
        VouAmt = get_VoucherSummary("~19~18~")
        cr_amt = cr_amt + Common_Procedures.Currency_Format(Val(VouAmt))



        If cr_amt > db_amt Then
            lbl_Netloss.Visible = False
            lbl_NetLossName.Visible = False


            lbl_NetProfitName.Visible = True
            lbl_NetProfit.Visible = True
            lbl_NetProfit.ForeColor = Color.Green
            lbl_NetProfitName.ForeColor = Color.Green

            Net_Profit = Common_Procedures.Currency_Format(Val(cr_amt - db_amt))
            lbl_NetProfit.Text = Common_Procedures.Currency_Format(Val(cr_amt - db_amt))


        Else
            lbl_NetProfitName.Visible = False
            lbl_NetProfit.Visible = False

            lbl_Netloss.Visible = True
            lbl_NetLossName.Visible = True
            lbl_Netloss.ForeColor = Color.Red
            lbl_NetLossName.ForeColor = Color.Red

            Net_Loss = Common_Procedures.Currency_Format(Val(db_amt - cr_amt))
            lbl_Netloss.Text = Common_Procedures.Currency_Format(Val(db_amt - cr_amt))

        End If

    End Sub


    Private Function Voucher_Summary_ForCurrentAsset() As Double
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim condt As String = ""
        Dim VouAmt As Double = 0
        Dim OPN As Double = 0
        Dim Nr As Integer = 0

        Cmd.Connection = con


        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        Cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

        VouAmt = 0
        Da1 = New SqlClient.SqlDataAdapter("Select sum(currency1) from reporttemp where name1 LIKE '%~4~' and name1 NOT LIKE '%~9~4~'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                VouAmt = Val(Dt1.Rows(0)(0).ToString)
            Else
                VouAmt = 0
            End If
        End If
        Dt1.Clear()

        'Da1 = New SqlClient.SqlDataAdapter("Select sum(currency1) from reporttemp where name1 LIKE '%~9~4~'", con)
        'Dt1 = New DataTable
        'Da1.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        OPN = Val(Dt1.Rows(0)(0).ToString)
        '        VouAmt = VouAmt - Val(Dt1.Rows(0)(0).ToString)
        '    End If
        'End If
        'Dt1.Clear()

        Cmd.CommandText = "truncate table EntryTemp "
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into EntryTemp ( Int1, Int2, date1, currency1 ) Select a.Company_IdNo, 12, a.Closing_Stock_Value_Date, sum(a.Closing_Stock_Value) from Closing_Stock_Value_Head a, Company_Head tZ where " & condt & IIf(condt <> "", " and ", "") & " a.Closing_Stock_Value_Date <= @todate and a.Company_IdNo = tZ.Company_IdNo group by a.Company_IdNo, a.Closing_Stock_Value_Date Having sum(a.Closing_Stock_Value) <> 0"
        'Cmd.CommandText = "insert into EntryTemp ( Int1, Int2, date1, currency1 ) Select a.Company_IdNo, 12, a.Closing_Stock_Value_Date, sum(a.Closing_Stock_Value) from Closing_Stock_Value_Head a, Company_Head tZ where " & condt & IIf(condt <> "", " and ", "") & " a.Closing_Stock_Value_Date <= @fromdate and a.Company_IdNo = tZ.Company_IdNo group by a.Company_IdNo, a.Closing_Stock_Value_Date Having sum(a.Closing_Stock_Value) <> 0"
        Nr = Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Select top 1 Currency1 from EntryTemp where date1 <= @todate Order by date1 desc"
        'Cmd.CommandText = "Select top 1 Currency1 from EntryTemp where date1 <= @fromdate Order by date1 desc"
        'Cmd.CommandText = "Select sum(Closing_Stock_Value) from Closing_Stock_Value_Head tz where " & condt & IIf(condt <> "", " and ", "") & " tz.Closing_Stock_Value_Date <= @todate"
        Da1 = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        Cls_Stock = 0
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                Cls_Stock = Val(Dt1.Rows(0)(0).ToString)
                VouAmt = VouAmt - Val(Dt1.Rows(0)(0).ToString)
            End If
        End If
        Dt1.Clear()

        Voucher_Summary_ForCurrentAsset = VouAmt

    End Function

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

    Private Sub Balance_Sheet_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
       
        FrmLdSTS = False

    End Sub

    Private Sub Balance_Sheet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub


    Private Sub Balance_Sheet_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ShowCompCol_STS As Boolean = True

        con.Open()

        Me.Left = 0
        Me.Top = 0
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width - 10
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height - 90

        pnl_GridView.Location = New Point(1, 96)
        pnl_GridView.Visible = False

        opt_Simple.Checked = True
        AddHandler cbo_Inputs1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Inputs1.LostFocus, AddressOf ControlLostFocus

        ShowCompCol_STS = Common_Procedures.Show_CompanyCondition_for_Report(con)
        If ShowCompCol_STS = False Then
            lbl_Inputs1.Visible = False
            cbo_Inputs1.Visible = False
        Else
            lbl_Inputs1.Visible = True
            cbo_Inputs1.Visible = True
        End If

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

        FrmLdSTS = True

    End Sub

    Private Function Company_Condition() As String
        Dim Condt As String = ""
        Dim ShowCompCol_STS As Boolean = False


        If cbo_Inputs1.Visible = True Then
            If cbo_Inputs1.Visible = True And Trim(cbo_Inputs1.Text) <> "" Then
                Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " tZ.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Inputs1.Text)))
            End If
        End If
        Company_Condition = Condt
    End Function

    Private Sub dtp_FromDate_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FromDate.KeyDown
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

    Private Sub dtp_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_ToDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Inputs1.Visible And cbo_Inputs1.Enabled Then
                If cbo_Inputs1.Visible And cbo_Inputs1.Enabled Then
                    cbo_Inputs1.Focus()
                Else
                    Show_Report()
                    txt_Selection.Focus()
                End If
            Else
                btn_Show.Focus()
                Show_Report()
            End If
        End If
    End Sub

    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_Inputs1.Visible And cbo_Inputs1.Enabled Then
                If cbo_Inputs1.Visible And cbo_Inputs1.Enabled Then
                    cbo_Inputs1.Focus()
                Else
                    Show_Report()
                    txt_Selection.Focus()
                End If
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

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Inputs1, Nothing, Nothing, Common_Procedures.RptCboDet(1).TableName, Common_Procedures.RptCboDet(1).Selection_FieldName, Common_Procedures.RptCboDet(1).Condition, Common_Procedures.RptCboDet(1).BlankFieldCondition)

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

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Inputs1, Nothing, Common_Procedures.RptCboDet(1).TableName, Common_Procedures.RptCboDet(1).Selection_FieldName, Common_Procedures.RptCboDet(1).Condition, Common_Procedures.RptCboDet(1).BlankFieldCondition)

            If Asc(e.KeyChar) = 13 Then
                btn_Show.Focus()
                Show_Report()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Label_Selection(ByVal color As Integer)

        lbl_CapitalAcc.ForeColor = Drawing.Color.Black
        lbl_CapitalAccName.ForeColor = Drawing.Color.Black
        lbl_CapitalAcc.BackColor = Drawing.Color.White
        lbl_CapitalAccName.BackColor = Drawing.Color.White

        lbl_LoansLiabilities.ForeColor = Drawing.Color.Black
        lbl_LoansLiabilitiesName.ForeColor = Drawing.Color.Black
        lbl_LoansLiabilities.BackColor = Drawing.Color.White
        lbl_LoansLiabilitiesName.BackColor = Drawing.Color.White

        lbl_CurrentLiabilities.ForeColor = Drawing.Color.Black
        lbl_CurrentLiabilitiesName.ForeColor = Drawing.Color.Black
        lbl_CurrentLiabilities.BackColor = Drawing.Color.White
        lbl_CurrentLiabilitiesName.BackColor = Drawing.Color.White

        lbl_BranchDivisions.ForeColor = Drawing.Color.Black
        lbl_BranchDivisionsName.ForeColor = Drawing.Color.Black
        lbl_BranchDivisions.BackColor = Drawing.Color.White
        lbl_BranchDivisionsName.BackColor = Drawing.Color.White

        lbl_FixedAssets.ForeColor = Drawing.Color.Black
        lbl_FixedAssetsName.ForeColor = Drawing.Color.Black
        lbl_FixedAssets.BackColor = Drawing.Color.White
        lbl_FixedAssetsName.BackColor = Drawing.Color.White

        lbl_Investments.ForeColor = Drawing.Color.Black
        lbl_InvestmentsName.ForeColor = Drawing.Color.Black
        lbl_Investments.BackColor = Drawing.Color.White
        lbl_InvestmentsName.BackColor = Drawing.Color.White

        lbl_CurrentAssets.ForeColor = Drawing.Color.Black
        lbl_CurrentAssetsName.ForeColor = Drawing.Color.Black
        lbl_CurrentAssets.BackColor = Drawing.Color.White
        lbl_CurrentAssetsName.BackColor = Drawing.Color.White

        lbl_SuspenseAcc.ForeColor = Drawing.Color.Black
        lbl_SuspenseAccName.ForeColor = Drawing.Color.Black
        lbl_SuspenseAcc.BackColor = Drawing.Color.White
        lbl_SuspenseAccName.BackColor = Drawing.Color.White

        lbl_MiscExpenses.ForeColor = Drawing.Color.Black
        lbl_MiscExpensesName.ForeColor = Drawing.Color.Black
        lbl_MiscExpenses.BackColor = Drawing.Color.White
        lbl_MiscExpensesName.BackColor = Drawing.Color.White

       
        If color = 2 Then
            lbl_Selection.Left = Choose(Val(txt_Selection.Text), 0, 0, 0, 0, lbl_FixedAssetsName.Left - 22, lbl_InvestmentsName.Left - 22, lbl_CurrentAssetsName.Left - 22, lbl_SuspenseAccName.Left - 22, lbl_MiscExpensesName.Left - 22)
            lbl_Selection.Top = Choose(Val(txt_Selection.Text), lbl_CapitalAccName.Top, lbl_LoansLiabilitiesName.Top, lbl_CurrentLiabilitiesName.Top, lbl_BranchDivisionsName.Top, lbl_FixedAssetsName.Top, lbl_InvestmentsName.Top, lbl_CurrentAssetsName.Top, lbl_SuspenseAccName.Top, lbl_MiscExpensesName.Top) - 4

            Select Case Val(txt_Selection.Text)
                Case 1
                    
                    lbl_CapitalAcc.ForeColor = Drawing.Color.Red
                    lbl_CapitalAccName.ForeColor = Drawing.Color.Red
                    lbl_CapitalAcc.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_CapitalAccName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 2
                    lbl_LoansLiabilities.ForeColor = Drawing.Color.Red
                    lbl_LoansLiabilitiesName.ForeColor = Drawing.Color.Red
                    lbl_LoansLiabilities.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_LoansLiabilitiesName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 3
                    lbl_CurrentLiabilities.ForeColor = Drawing.Color.Red
                    lbl_CurrentLiabilitiesName.ForeColor = Drawing.Color.Red
                    lbl_CurrentLiabilities.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_CurrentLiabilitiesName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 4
                    lbl_BranchDivisions.ForeColor = Drawing.Color.Red
                    lbl_BranchDivisionsName.ForeColor = Drawing.Color.Red
                    lbl_BranchDivisions.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_BranchDivisionsName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 5
                    lbl_FixedAssets.ForeColor = Drawing.Color.Red
                    lbl_FixedAssetsName.ForeColor = Drawing.Color.Red
                    lbl_FixedAssets.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_FixedAssetsName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 6
                    lbl_Investments.ForeColor = Drawing.Color.Red
                    lbl_InvestmentsName.ForeColor = Drawing.Color.Red
                    lbl_Investments.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_InvestmentsName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 7
                    lbl_CurrentAssets.ForeColor = Drawing.Color.Red
                    lbl_CurrentAssetsName.ForeColor = Drawing.Color.Red
                    lbl_CurrentAssets.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_CurrentAssetsName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 8
                    lbl_SuspenseAcc.ForeColor = Drawing.Color.Red
                    lbl_SuspenseAccName.ForeColor = Drawing.Color.Red
                    lbl_SuspenseAcc.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_SuspenseAccName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                Case 9
                    lbl_MiscExpenses.ForeColor = Drawing.Color.Red
                    lbl_MiscExpensesName.ForeColor = Drawing.Color.Red
                    lbl_MiscExpenses.BackColor = Drawing.Color.FromArgb(192, 192, 255)
                    lbl_MiscExpensesName.BackColor = Drawing.Color.FromArgb(192, 192, 255)
            End Select
        End If
    End Sub

    Private Sub get_Details(ByVal grp_code As Integer)
        Dim Grp_Name As String = ""
        Dim grp_cd As String = ""


        Select Case grp_code
            Case 1
                '---Capital a/c
                grp_cd = "~2~"
            Case 2
                '---Loans 
                grp_cd = "~21~"
            Case 3
                '---Current Liabilities
                grp_cd = "~11~"
            Case 4
                '---Branch & Divisions
                grp_cd = "~1~"
            Case 5
                '---Fixed Assets
                grp_cd = "~17~"
            Case 6
                '---Investments
                grp_cd = "~20~"
            Case 7
                '---Current Assets
                grp_cd = "~4~"
            Case 8
                '---Suspense A/c
                grp_cd = "~29~"
            Case 9
                '---Misc.Exp
                grp_cd = "~26~"

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

            f.RptSubReportDet(f.RptSubReport_Index).ReportName = "Balance Sheet"
            f.RptSubReportDet(f.RptSubReport_Index).ReportGroupName = "Accounts"
            f.RptSubReportDet(f.RptSubReport_Index).ReportHeading = "Balance Sheet"
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



    Private Sub txt_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Selection.KeyUp
        Label_Selection(1)
        If e.KeyValue = 40 Then If Val(txt_Selection.Text) < 9 Then txt_Selection.Text = Val(txt_Selection.Text) + 1
        If e.KeyValue = 38 Then If Val(txt_Selection.Text) > 1 Then txt_Selection.Text = Val(txt_Selection.Text) - 1
        If e.KeyValue = 39 Then
            If Val(txt_Selection.Text) = 1 Then txt_Selection.Text = 5
            If Val(txt_Selection.Text) = 2 Then txt_Selection.Text = 6
            If Val(txt_Selection.Text) = 3 Then txt_Selection.Text = 7
            If Val(txt_Selection.Text) = 4 Then txt_Selection.Text = 8
        End If
        If e.KeyValue = 37 Then
            If Val(txt_Selection.Text) = 5 Then txt_Selection.Text = 1
            If Val(txt_Selection.Text) = 6 Then txt_Selection.Text = 2
            If Val(txt_Selection.Text) = 7 Then txt_Selection.Text = 3
            If Val(txt_Selection.Text) = 8 Or Val(txt_Selection.Text) = 9 Then txt_Selection.Text = 4
        End If
        Label_Selection(2)
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
        Dim Tot_DB As Decimal = 0, Tot_CR As Decimal = 0
        Dim Grp_Name As String = ""
        Dim grp_cd As String = ""
        Dim z As Integer = 0
        Dim supTtl As Integer = 0
        Dim cnt As Integer = 0
        Dim Sps As String = ""
        Dim VouAmt As Double = 0
        Dim RwNo_OpDif As Integer = -1


        condt = Company_Condition()

        Balance_Sheet_Calculation()

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        cmd.CommandText = "truncate table reporttemp"
        cmd.ExecuteNonQuery()


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1105" Then '---- Ganga Weaving (Dindugal)
        cmd.CommandText = "insert into reporttemp (Name1, Name2 , meters1 , int1 ,Name3 , currency1, currency2) Select a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name, (case when sum(tV.voucher_amount) < 0 then sum(tV.voucher_amount) else 0 end), (case when sum(tV.voucher_amount) > 0 then sum(tV.voucher_amount) else 0 end) from ledger_head a, voucher_details tV ,AccountsGroup_Head tG , company_head tz where " & condt & IIf(condt <> "", " and ", "") & " tV.voucher_date <= @todate and a.parent_code NOT LIKE '%~9~4~' and a.ledger_idno = tV.ledger_idno and a.AccountsGroup_IdNo = tG.AccountsGroup_IdNo AND tV.Company_IdNo = tz.Company_IdNo group by a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name Having sum(tV.voucher_amount) <> 0"
        cmd.ExecuteNonQuery()

        'Else
        '    cmd.CommandText = "insert into reporttemp (Name1, Name2 , meters1 , int1 ,Name3 , currency1, currency2) Select a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name, (case when sum(tV.voucher_amount) < 0 then sum(tV.voucher_amount) else 0 end), (case when sum(tV.voucher_amount) > 0 then sum(tV.voucher_amount) else 0 end) from ledger_head a, voucher_details tV ,AccountsGroup_Head tG , company_head tz where " & condt & IIf(condt <> "", " and ", "") & " tV.voucher_date between @companyfromdate and @todate and a.parent_code NOT LIKE '%~9~4~' and a.ledger_idno = tV.ledger_idno and a.AccountsGroup_IdNo = tG.AccountsGroup_IdNo AND tV.Company_IdNo = tz.Company_IdNo group by a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name Having sum(tV.voucher_amount) <> 0"
        '    cmd.ExecuteNonQuery()

        'End If

        VouAmt = Voucher_Summary_ForCurrentAsset()
        If Val(Cls_Stock) <> 0 Then
            cmd.CommandText = "insert into reporttemp (Name1, Name2 , meters1 , int1 ,Name3 , currency1, currency2) Select a.parent_code ,tG.AccountsGroup_Name, tG.Order_Position, a.ledger_idno, a.ledger_name, " & IIf(Val(Cls_Stock) > 0, Val(Cls_Stock), 0) & " ,  " & IIf(Val(Cls_Stock) < 0, Val(Cls_Stock), 0) & " from ledger_head a, AccountsGroup_Head tG where a.ledger_idno = 12 and a.parent_code LIKE '%~4~' and a.AccountsGroup_IdNo = tG.AccountsGroup_IdNo"
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
            .Columns(3).HeaderText = ""

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
            Tot_DB = 0
            Tot_CR = 0


            N = .Rows.Add

            For Cyc = 1 To 10 Step 1

                Select Case Cyc
                    Case 1
                        '---Capital a/c
                        grp_cd = "~2~"
                    Case 2
                        '---Loans 
                        grp_cd = "~21~"
                    Case 3
                        '---Current Liabilities
                        grp_cd = "~11~"
                    Case 4
                        '---Branch & Divisions
                        grp_cd = "~1~"
                    Case 5
                        '---Fixed Assets
                        grp_cd = "~17~"
                    Case 6
                        '---Investments
                        grp_cd = "~20~"
                    Case 7
                        '---Current Assets
                        'VouAmt = Voucher_Summary_ForCurrentAsset()
                        'N = .Rows.Add
                        '.Rows(N).Cells(0).Value = "CLOSING STOCK"
                        'If Val(Cls_Stock) < 0 Then
                        '    .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Cls_Stock)))
                        '    Tot_CR = Tot_CR + Math.Abs(CDbl(.Rows(N).Cells(2).Value))
                        'ElseIf Val(Cls_Stock) > 0 Then
                        '    .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Val(Cls_Stock))
                        '    Tot_DB = Tot_DB + Math.Abs(CDbl(.Rows(N).Cells(1).Value))
                        'End If
                        '.Rows(N).Cells(0).Style.ForeColor = Color.Blue
                        'N = .Rows.Add

                        grp_cd = "~4~"

                    Case 8
                        '---Suspense A/c
                        grp_cd = "~29~"
                    Case 9
                        '---Misc.Exp
                        grp_cd = "~26~"
                    Case 10
                        '---NET PROFIT OR NET LOSS
                        N = .Rows.Add
                        If Net_Profit > 0 Then
                            .Rows(N).Cells(0).Value = "NET PROFIT"
                            .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Val(Net_Profit))
                            Tot_CR = Tot_CR + Math.Abs(CDbl(.Rows(N).Cells(2).Value))
                            .Rows(N).Cells(0).Style.ForeColor = Color.DarkGreen
                            .Rows(N).Cells(1).Style.ForeColor = Color.DarkGreen
                            .Rows(N).Cells(2).Style.ForeColor = Color.DarkGreen
                        Else
                            .Rows(N).Cells(0).Value = "NET LOSS"
                            .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Net_Loss)))
                            Tot_DB = Tot_DB + Math.Abs(CDbl(.Rows(N).Cells(1).Value))
                            .Rows(N).Cells(0).Style.ForeColor = Color.Red
                            .Rows(N).Cells(1).Style.ForeColor = Color.Red
                            .Rows(N).Cells(2).Style.ForeColor = Color.Red
                        End If
                        N = .Rows.Add
                End Select

                '------------Group 
                Da = New SqlClient.SqlDataAdapter("select Name1, name2, sum(currency1) as DR_AMOUNT,sum(currency2) AS CR_AMOUNT from reporttemp where name1  LIKE '%" & Trim(grp_cd) & "' group by name2, Name1 order by name2, Name1", con)
                Dtbl1 = New DataTable
                Da.Fill(Dtbl1)
                Bal = 0
                If Dtbl1.Rows.Count > 0 Then
                    For i = 0 To Dtbl1.Rows.Count - 1
                        N = .Rows.Add
                        .Rows(N).Cells(0).Value = Dtbl1.Rows(i).Item("name2").ToString

                        If Math.Abs(Val(Dtbl1.Rows(i).Item("CR_AMOUNT").ToString)) > Math.Abs(Val(Dtbl1.Rows(i).Item("DR_AMOUNT").ToString)) Then
                            .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl1.Rows(i).Item("CR_AMOUNT").ToString)) - Math.Abs(Val(Dtbl1.Rows(i).Item("DR_AMOUNT").ToString)))
                            Tot_CR = Tot_CR + Math.Abs(CDbl(.Rows(N).Cells(2).Value))
                        Else
                            .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl1.Rows(i).Item("DR_AMOUNT").ToString)) - Math.Abs(Val(Dtbl1.Rows(i).Item("CR_AMOUNT").ToString)))
                            Tot_DB = Tot_DB + Math.Abs(CDbl(.Rows(N).Cells(1).Value))
                        End If
                        .Rows(N).Cells(0).Style.ForeColor = Color.Blue


                        '----------Detail

                        Da = New SqlClient.SqlDataAdapter("select  name3, sum(currency1) as DR_AMOUNT,sum(currency2) AS CR_AMOUNT from reporttemp where name1  = '" & Trim(Dtbl1.Rows(i).Item("name1").ToString) & "' GROUP BY name3", con)
                        Dtbl2 = New DataTable
                        Da.Fill(Dtbl2)
                        Bal = 0
                        If Dtbl2.Rows.Count > 0 Then
                            For k = 0 To Dtbl2.Rows.Count - 1
                                N = .Rows.Add
                                If Val(Dtbl2.Rows(k).Item("DR_AMOUNT").ToString) <> 0 Then
                                    .Rows(N).Cells(0).Value = "  " & Trim(Dtbl2.Rows(k).Item("name3").ToString) & Space(38 - Len(Microsoft.VisualBasic.Left(Trim(Dtbl2.Rows(k).Item("name3").ToString), 38))) & Space(15 - Len(Trim(Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl2.Rows(k).Item("DR_AMOUNT").ToString)))))) & Trim(Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl2.Rows(k).Item("DR_AMOUNT").ToString)))) & IIf(Val(Dtbl2.Rows(k).Item("DR_AMOUNT").ToString) > 0, " Cr", " Dr")
                                Else
                                    .Rows(N).Cells(0).Value = "  " & Trim(Dtbl2.Rows(k).Item("name3").ToString) & Space(38 - Len(Microsoft.VisualBasic.Left(Trim(Dtbl2.Rows(k).Item("name3").ToString), 38))) & Space(15 - Len(Trim(Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl2.Rows(k).Item("CR_AMOUNT").ToString)))))) & Trim(Common_Procedures.Currency_Format(Math.Abs(Val(Dtbl2.Rows(k).Item("CR_AMOUNT").ToString)))) & IIf(Val(Dtbl2.Rows(k).Item("CR_AMOUNT").ToString) > 0, " Cr", " Dr")
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

                If Cyc = 10 Then
                    RwNo_OpDif = -1
                    If Math.Abs(Val(Tot_DB)) <> Math.Abs(Val(Tot_CR)) Then
                        N = .Rows.Add

                        If Math.Abs(Val(Tot_DB)) > Math.Abs(Val(Tot_CR)) Then
                            .Rows(N).Cells(0).Value = "OPENING DIFFERENCE"
                            .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Tot_DB)) - Math.Abs(Val(Tot_CR)))
                        Else
                            .Rows(N).Cells(0).Value = "OPENING DIFFERENCE"
                            .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Tot_CR)) - Math.Abs(Val(Tot_DB)))
                        End If
                        .Rows(N).Cells(0).Style.ForeColor = Color.Red
                        .Rows(N).Cells(1).Style.ForeColor = Color.Red
                        .Rows(N).Cells(2).Style.ForeColor = Color.Red


                        RwNo_OpDif = N

                        N = .Rows.Add

                    End If

                    N = .Rows.Add

                    .Rows(N).Cells(0).Value = "TOTAL"
                    If RwNo_OpDif > 0 Then
                        .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Tot_DB) + Val(CDbl(.Rows(RwNo_OpDif).Cells(1).Value))))
                        .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Tot_CR) + Val(CDbl(.Rows(RwNo_OpDif).Cells(2).Value))))
                    Else
                        .Rows(N).Cells(1).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Tot_DB)))
                        .Rows(N).Cells(2).Value = Common_Procedures.Currency_Format(Math.Abs(Val(Tot_CR)))
                    End If


                    .Rows(N).Cells(3).Value = ""
                    Tot_DB = 0 : Tot_CR = 0
                    N = .Rows.Count - 1
                    .Rows(N).Height = 40
                    For j = 0 To .ColumnCount - 1
                        .Rows(N).Cells(j).Style.BackColor = Color.Gray
                        .Rows(N).Cells(j).Style.ForeColor = Color.White
                    Next
                End If

            Next Cyc

            N = .Rows.Add

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
            'pnl_GridView.Visible = False
            'pnl_Back.Visible = True
            'Show_Report()
        End If
    End Sub

    Private Sub opt_Details_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_Details.CheckedChanged
        If opt_Details.Checked = True Then
            opt_Details.ForeColor = Color.Blue
            opt_Simple.ForeColor = Color.Black
            'pnl_Back.Visible = False
            'pnl_GridView.Visible = True
            'get_DetailedGridReport()
        End If
    End Sub

   
    Public Sub print_record() Implements Interface_MDIActions.print_record
        If opt_Simple.Checked = True Then
        Print_Selection()

        ElseIf opt_Details.Checked = True Then
            WeightBridge_Entry_Pending_Report_Printing()

        End If
    End Sub

    Private Sub Print_Selection()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

       
      
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                PrintDocument1.DefaultPageSettings.PaperSize = ps
                                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                PpSzSTS = True
                                Exit For
                            End If
                        Next

                        If PpSzSTS = False Then
                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next
                        End If

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
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

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
        Common_Procedures.Print_To_PrintDocument(e, " BALANCE SHEET", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "As on " & Convert.ToDateTime(dtp_ToDate.Text), LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        Try
            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "LIABILITIES", LMargin, CurY, 2, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "ASSETS", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)
          
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            '---Liabilities
            If Val(lbl_CapitalAcc.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Capital Account", CDbl(lbl_CapitalAcc.Text), LMargin + 10, CurY, ClAr(1))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~2~", LMargin + 10, CurY, 1)
            End If
            If Val(lbl_LoansLiabilities.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Loans (Liability)", CDbl(lbl_LoansLiabilities.Text), LMargin + 10, CurY, ClAr(1))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~21~", LMargin + 10, CurY, 1)
            End If
            If Val(lbl_CurrentLiabilities.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Current Liabilities", CDbl(lbl_CurrentLiabilities.Text), LMargin + 10, CurY, ClAr(1))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~11~", LMargin + 10, CurY, 1)
            End If
            If Val(lbl_BranchDivisions.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Branch And Division", CDbl(lbl_BranchDivisions.Text), LMargin + 10, CurY, ClAr(1))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~1~", LMargin + 10, CurY, 1)
            End If
            If lbl_NetProfit.Visible Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Net Profit", CDbl(lbl_NetProfit.Text), LMargin + 10, CurY, ClAr(1))
            End If
            If Val(lbl_OpeningDiffDB.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "[Opening Diff]", CDbl(lbl_OpeningDiffDB.Text), LMargin + 10, CurY, ClAr(1))
            End If
            CurY_Temp = CurY
            CurY = LnAr(3)
            '---- Assets
            If Val(lbl_FixedAssets.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Fixed Assets", CDbl(lbl_FixedAssets.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~17~", LMargin + ClAr(1), CurY, -1)
            End If
            If Val(lbl_Investments.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Investments", CDbl(lbl_Investments.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~20~", LMargin + ClAr(1), CurY, -1)
            End If
            If Val(lbl_CurrentAssets.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Current Assets", CDbl(lbl_CurrentAssets.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~4~", LMargin + ClAr(1), CurY, -1)
            End If
            If Val(lbl_MiscExpenses.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Misc. Expenses", CDbl(lbl_MiscExpenses.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~26~", LMargin + ClAr(1), CurY, -1)
            End If
            If Val(lbl_SuspenseAcc.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Misc. Expenses", CDbl(lbl_SuspenseAcc.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
                CurY = CurY + TxtHgt + 10
                Print_Details(e, "~29~", LMargin + ClAr(1), CurY, -1)
            End If
            If lbl_Netloss.Visible Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "Net Loss", CDbl(lbl_Netloss.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
            End If
            If Val(lbl_OpeningDiffCR.Text) <> 0 Then
                CurY = CurY + TxtHgt + 10
                Print_Heading(e, "[Opening Diff]", CDbl(lbl_OpeningDiffCR.Text), LMargin + ClAr(1) + 10, CurY, ClAr(2))
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
            Common_Procedures.Print_To_PrintDocument(e, Trim(lbl_TotalLiabilities.Text), LMargin + 360, CurY, 1, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 10, CurY, 0, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(lbl_TotalAssets.Text), LMargin + ClAr(1) + 360, CurY, 1, 0, p1Font)

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

   

    Private Sub Print_Details(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal gp_id As String, ByVal cur_x As Decimal, ByRef cur_y As Decimal, ByVal CrDr As Integer)
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
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(CrDr * Val(dt1.Rows(0).Item("SumOfAmount").ToString)), C_X2, cur_y, 1, 0, p1Font)
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
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(CrDr * Val(dt2.Rows(d2).Item("total").ToString)), C_X2, cur_y, 1, 0, p1Font)
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

    Private Sub lbl_CapitalAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_CapitalAcc.Click, lbl_CapitalAccName.Click
        txt_Selection.Text = 1
        Label_Selection(2)
    End Sub

    Private Sub lbl_LoansLiabilities_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_LoansLiabilities.Click, lbl_LoansLiabilitiesName.Click
        txt_Selection.Text = 2
        Label_Selection(2)
    End Sub

    Private Sub lbl_CurrentLiabilities_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_CurrentLiabilities.Click, lbl_CurrentLiabilitiesName.Click
        txt_Selection.Text = 3
        Label_Selection(2)
    End Sub

    Private Sub lbl_BranchDivisions_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_BranchDivisions.Click, lbl_BranchDivisionsName.Click
        txt_Selection.Text = 4
        Label_Selection(2)
    End Sub

    Private Sub lbl_FixedAssets_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_FixedAssets.Click, lbl_FixedAssetsName.Click
        txt_Selection.Text = 5
        Label_Selection(2)
    End Sub

    Private Sub lbl_Investments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Investments.Click, lbl_InvestmentsName.Click
        txt_Selection.Text = 6
        Label_Selection(2)
    End Sub

    Private Sub lbl_CurrentAssets_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_CurrentAssets.Click, lbl_CurrentAssetsName.Click
        txt_Selection.Text = 7
        Label_Selection(2)
    End Sub

    Private Sub lbl_SuspenseAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SuspenseAcc.Click, lbl_SuspenseAccName.Click
        txt_Selection.Text = 8
        Label_Selection(2)
    End Sub

    Private Sub lbl_MiscExpenses_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_MiscExpenses.Click, lbl_MiscExpensesName.Click
        txt_Selection.Text = 9
        Label_Selection(2)
    End Sub

    Private Sub lbl_CapitalAcc_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_CapitalAcc.DoubleClick, lbl_CapitalAccName.DoubleClick
        txt_Selection.Text = 1
        Label_Selection(2)
        get_Details(1)
    End Sub

    Private Sub lbl_LoansLiabilities_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_LoansLiabilities.DoubleClick, lbl_LoansLiabilitiesName.DoubleClick
        txt_Selection.Text = 2
        Label_Selection(2)
        get_Details(2)
    End Sub

    Private Sub lbl_CurrentLiabilities_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_CurrentLiabilities.DoubleClick, lbl_CurrentLiabilitiesName.DoubleClick
        txt_Selection.Text = 3
        Label_Selection(2)
        get_Details(3)
    End Sub

    Private Sub lbl_BranchDivisionsName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_BranchDivisions.DoubleClick, lbl_BranchDivisionsName.DoubleClick
        txt_Selection.Text = 4
        Label_Selection(2)
        get_Details(4)
    End Sub

    Private Sub lbl_FixedAssets_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_FixedAssets.DoubleClick, lbl_FixedAssetsName.DoubleClick
        txt_Selection.Text = 5
        Label_Selection(2)
        get_Details(5)
    End Sub

    Private Sub lbl_Investments_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Investments.DoubleClick, lbl_InvestmentsName.DoubleClick
        txt_Selection.Text = 6
        Label_Selection(2)
        get_Details(6)
    End Sub

    Private Sub lbl_CurrentAssets_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_CurrentAssets.DoubleClick, lbl_CurrentAssetsName.DoubleClick
        txt_Selection.Text = 7
        Label_Selection(2)
        get_Details(7)
    End Sub

    Private Sub lbl_SuspenseAcc_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SuspenseAcc.DoubleClick, lbl_SuspenseAccName.DoubleClick
        txt_Selection.Text = 8
        Label_Selection(2)
        get_Details(8)
    End Sub

    Private Sub lbl_MiscExpenses_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_MiscExpenses.DoubleClick, lbl_MiscExpensesName.DoubleClick
        txt_Selection.Text = 9
        Label_Selection(2)
        get_Details(9)
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
        Dim i As Integer = 0, j As Integer = 0
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



        'ClArr(0) = 450 : ClArr(1) = 180
        'ClArr(2) = PageWidth - (LMargin + ClArr(0) + ClArr(1))

        For i = 0 To dgv_PrfitAndLossDetails.Columns.Count - 2
            If dgv_PrfitAndLossDetails.Columns(i).Width < 20 Or dgv_PrfitAndLossDetails.Columns(i).Visible = False Then
                ClArr(i + 1) = ClArr(j)
            Else
                If i = 0 Then
                    ClArr(i + 1) = ClArr(j) + 400 ' dgv_PrfitAndLossDetails.Columns(i).Width
                Else
                    ClArr(i + 1) = ClArr(j) + 150
                End If

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
                Common_Procedures.Print_To_PrintDocument(e, "BALANCE SHEET", LMargin, CurY, 2, PrintWidth, p1Font)

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