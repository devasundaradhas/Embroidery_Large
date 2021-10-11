Public Class Payroll_Salary_Entry_Details

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EMSAD-"
    Private Pk_Condition2 As String = "AVLSD-"
    Private Pk_Condition3 As String = "AVLDD-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Dim dgv1 As New DataGridView
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private ESI_MAX_SHFT_WAGES As Single = 0
    Private EPF_MAX_BASICPAY As Single = 0
    Private EPF_MAX_VALUE As Single = 0
    Private rowsToPrint As Queue(Of DataGridViewRow)
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private DetSNo As Integer
    Private DetIndx As Integer

    Public previlege As String


    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_PrintEmployee_Details.Visible = False
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, 1)
        cbo_Category.Text = ""
        dtp_FromDate.Text = ""
        cbo_Month.Text = ""
        dtp_ToDate.Text = ""
        txt_FestivalDays.Text = ""
        txt_TotalDays.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Print_Details.Rows.Clear()
        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If


        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(44, 61, 90)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Print_Details.CurrentCell) Then dgv_Print_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Salary_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Salary_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Salary_Date").ToString
                cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, Val(dt1.Rows(0).Item("Salary_Payment_Type_IdNo").ToString))
                cbo_Category.Text = Common_Procedures.Category_IdNoToName(con, Val(dt1.Rows(0).Item("Category_IdNo").ToString))
                cbo_Month.Text = Common_Procedures.Month_IdNoToName(con, Val(dt1.Rows(0).Item("Month_IdNo").ToString))
                dtp_FromDate.Text = dt1.Rows(0).Item("From_Date").ToString
                dtp_ToDate.Text = dt1.Rows(0).Item("To_Date").ToString
                dtp_Advance_UpToDate.Text = dt1.Rows(0).Item("Advance_UptoDate").ToString
                txt_TotalDays.Text = Val(dt1.Rows(0).Item("Total_Days").ToString)
                txt_FestivalDays.Text = Val(dt1.Rows(0).Item("Festival_Days").ToString)

                If Not IsDBNull(dt1.Rows(0).Item("Salary_Year")) Then
                    cbo_Year.Text = dt1.Rows(0).Item("Salary_Year")
                Else
                    cbo_Year.Text = CDate(dt1.Rows(0).Item("From_Date").ToString).Year.ToString
                End If

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name,b.Card_No from PayRoll_Salary_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  Where a.Salary_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Employee_Name").ToString

                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Card_No").ToString

                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Basic_Salary").ToString), "########0.00")
                            If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Days").ToString)
                            If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Pay").ToString), "########0.00")
                            If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""

                            .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("No_Of_Attendance_Days").ToString)
                            If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                            .Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("From_W_Off_CR").ToString)
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                            .Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("From_CL_For_Leave").ToString)
                            If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                            .Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("From_SL_For_Leave").ToString)
                            If Val(.Rows(n).Cells(9).Value) = 0 Then .Rows(n).Cells(9).Value = ""


                            .Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Festival_Holidays").ToString)
                            If Val(.Rows(n).Cells(10).Value) = 0 Then .Rows(n).Cells(10).Value = ""

                            .Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Total_Leave_Days").ToString)
                            If Val(.Rows(n).Cells(11).Value) = 0 Then .Rows(n).Cells(11).Value = ""


                            .Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("No_Of_Leave").ToString)
                            If Val(.Rows(n).Cells(12).Value) = 0 Then .Rows(n).Cells(12).Value = ""

                            .Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("Attendance_On_W_Off_Fh").ToString)
                            If Val(.Rows(n).Cells(13).Value) = 0 Then .Rows(n).Cells(13).Value = ""

                            .Rows(n).Cells(14).Value = Val(dt2.Rows(i).Item("Op_W_Off_CR").ToString)
                            If Val(.Rows(n).Cells(14).Value) = 0 Then .Rows(n).Cells(14).Value = ""

                            .Rows(n).Cells(15).Value = Val(dt2.Rows(i).Item("Add_W_Off_CR").ToString)
                            If Val(.Rows(n).Cells(15).Value) = 0 Then .Rows(n).Cells(15).Value = ""

                            .Rows(n).Cells(16).Value = Val(dt2.Rows(i).Item("Less_W_Off_CR").ToString)
                            If Val(.Rows(n).Cells(16).Value) = 0 Then .Rows(n).Cells(16).Value = ""

                            .Rows(n).Cells(17).Value = Val(dt2.Rows(i).Item("Total_W_Off_CR").ToString)
                            If Val(.Rows(n).Cells(17).Value) = 0 Then .Rows(n).Cells(17).Value = ""

                            .Rows(n).Cells(18).Value = Val(dt2.Rows(i).Item("Op_CL_CR_Days").ToString)
                            If Val(.Rows(n).Cells(18).Value) = 0 Then .Rows(n).Cells(18).Value = ""

                            .Rows(n).Cells(19).Value = Val(dt2.Rows(i).Item("Less_CL_CR_Days").ToString)
                            If Val(.Rows(n).Cells(19).Value) = 0 Then .Rows(n).Cells(19).Value = ""

                            .Rows(n).Cells(20).Value = Val(dt2.Rows(i).Item("Total_Cl_CR_Days").ToString)
                            If Val(.Rows(n).Cells(20).Value) = 0 Then .Rows(n).Cells(20).Value = ""

                            .Rows(n).Cells(21).Value = Val(dt2.Rows(i).Item("Op_SL_CR_DAys").ToString)
                            If Val(.Rows(n).Cells(21).Value) = 0 Then .Rows(n).Cells(21).Value = ""

                            .Rows(n).Cells(22).Value = Val(dt2.Rows(i).Item("Less_SL_CR_DAys").ToString)
                            If Val(.Rows(n).Cells(22).Value) = 0 Then .Rows(n).Cells(22).Value = ""

                            .Rows(n).Cells(23).Value = Val(dt2.Rows(i).Item("Total_SL_CR_DAys").ToString)
                            If Val(.Rows(n).Cells(23).Value) = 0 Then .Rows(n).Cells(23).Value = ""

                            .Rows(n).Cells(24).Value = Val(dt2.Rows(i).Item("Salary_Days").ToString)
                            If Val(.Rows(n).Cells(24).Value) = 0 Then .Rows(n).Cells(24).Value = ""

                            .Rows(n).Cells(25).Value = Format(Val(dt2.Rows(i).Item("Basic_Pay").ToString), "########0.00")
                            If Val(.Rows(n).Cells(25).Value) = 0 Then .Rows(n).Cells(25).Value = ""

                            .Rows(n).Cells(26).Value = Val(dt2.Rows(i).Item("OT_Hours").ToString)
                            If Val(.Rows(n).Cells(26).Value) = 0 Then .Rows(n).Cells(26).Value = ""

                            .Rows(n).Cells(27).Value = Format(Val(dt2.Rows(i).Item("Ot_Pay_Hours").ToString), "########0.00")
                            If Val(.Rows(n).Cells(27).Value) = 0 Then .Rows(n).Cells(27).Value = ""

                            .Rows(n).Cells(28).Value = Format(Val(dt2.Rows(i).Item("OT_Salary").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(28).Value) = 0 Then .Rows(n).Cells(28).Value = ""

                            .Rows(n).Cells(29).Value = Format(Val(dt2.Rows(i).Item("D_A").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(29).Value) = 0 Then .Rows(n).Cells(29).Value = ""

                            .Rows(n).Cells(30).Value = Format(Val(dt2.Rows(i).Item("Earning").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(30).Value) = 0 Then .Rows(n).Cells(30).Value = ""

                            .Rows(n).Cells(31).Value = Format(Val(dt2.Rows(i).Item("H_R_A").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(31).Value) = 0 Then .Rows(n).Cells(31).Value = ""

                            .Rows(n).Cells(32).Value = Format(Val(dt2.Rows(i).Item("Conveyance").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(32).Value) = 0 Then .Rows(n).Cells(32).Value = ""

                            .Rows(n).Cells(33).Value = Format(Val(dt2.Rows(i).Item("Washing").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(33).Value) = 0 Then .Rows(n).Cells(33).Value = ""

                            .Rows(n).Cells(34).Value = Format(Val(dt2.Rows(i).Item("Entertainment").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(34).Value) = 0 Then .Rows(n).Cells(34).Value = ""

                            .Rows(n).Cells(35).Value = Format(Val(dt2.Rows(i).Item("Maintenance").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(35).Value) = 0 Then .Rows(n).Cells(35).Value = ""

                            .Rows(n).Cells(36).Value = Format(Val(dt2.Rows(i).Item("Provision").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(36).Value) = 0 Then .Rows(n).Cells(36).Value = ""

                            .Rows(n).Cells(37).Value = Format(Val(dt2.Rows(i).Item("Other_Addition1").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(37).Value) = 0 Then .Rows(n).Cells(37).Value = ""

                            .Rows(n).Cells(38).Value = Format(Val(dt2.Rows(i).Item("Other_Addition2").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(38).Value) = 0 Then .Rows(n).Cells(38).Value = ""

                            .Rows(n).Cells(39).Value = Format(Val(dt2.Rows(i).Item("Other_Addition").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(39).Value) = 0 Then .Rows(n).Cells(39).Value = ""

                            .Rows(n).Cells(40).Value = Format(Val(dt2.Rows(i).Item("Incentive_Amount").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(40).Value) = 0 Then .Rows(n).Cells(40).Value = ""


                            .Rows(n).Cells(41).Value = Format(Val(dt2.Rows(i).Item("Week_Off_Allowance").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(41).Value) = 0 Then .Rows(n).Cells(41).Value = ""


                            .Rows(n).Cells(42).Value = Format(Val(dt2.Rows(i).Item("Total_Addition").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(42).Value) = 0 Then .Rows(n).Cells(42).Value = ""

                            .Rows(n).Cells(43).Value = Format(Val(dt2.Rows(i).Item("Mess").ToString), "########0.00")
                            If Val(.Rows(n).Cells(43).Value) = 0 Then .Rows(n).Cells(43).Value = ""

                            .Rows(n).Cells(44).Value = Format(Val(dt2.Rows(i).Item("Medical").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(44).Value) = 0 Then .Rows(n).Cells(44).Value = ""

                            .Rows(n).Cells(45).Value = Format(Val(dt2.Rows(i).Item("Store").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(45).Value) = 0 Then .Rows(n).Cells(45).Value = ""

                            .Rows(n).Cells(46).Value = Format(Val(dt2.Rows(i).Item("ESI").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(46).Value) = 0 Then .Rows(n).Cells(46).Value = ""

                            .Rows(n).Cells(47).Value = Format(Val(dt2.Rows(i).Item("P_F").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(47).Value) = 0 Then .Rows(n).Cells(47).Value = ""

                            .Rows(n).Cells(48).Value = Format(Val(dt2.Rows(i).Item("E_P_F").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(48).Value) = 0 Then .Rows(n).Cells(48).Value = ""

                            .Rows(n).Cells(49).Value = Format(Val(dt2.Rows(i).Item("Pension_Scheme").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(49).Value) = 0 Then .Rows(n).Cells(49).Value = ""

                            .Rows(n).Cells(50).Value = Format(Val(dt2.Rows(i).Item("late_Mins").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(50).Value) = 0 Then .Rows(n).Cells(50).Value = ""

                            .Rows(n).Cells(51).Value = Format(Val(dt2.Rows(i).Item("Late_Hours_Salary").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(51).Value) = 0 Then .Rows(n).Cells(51).Value = ""

                            .Rows(n).Cells(52).Value = Format(Val(dt2.Rows(i).Item("Other_Deduction").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(52).Value) = 0 Then .Rows(n).Cells(52).Value = ""

                            .Rows(n).Cells(53).Value = Format(Val(dt2.Rows(i).Item("Total_Deduction").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(53).Value) = 0 Then .Rows(n).Cells(53).Value = ""

                            .Rows(n).Cells(54).Value = Format(Val(dt2.Rows(i).Item("Attendance_Incentive").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(54).Value) = 0 Then .Rows(n).Cells(54).Value = ""

                            .Rows(n).Cells(55).Value = Format(Val(dt2.Rows(i).Item("Net_Salary").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(55).Value) = 0 Then .Rows(n).Cells(55).Value = ""

                            .Rows(n).Cells(56).Value = Format(Val(dt2.Rows(i).Item("Total_Advance").ToString), "#######0.00")
                            If Val(.Rows(n).Cells(56).Value) = 0 Then .Rows(n).Cells(56).Value = ""

                            .Rows(n).Cells(57).Value = Format(Val(dt2.Rows(i).Item("Minus_Advance").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(57).Value) = 0 Then .Rows(n).Cells(57).Value = ""

                            .Rows(n).Cells(58).Value = Format(Val(dt2.Rows(i).Item("Balance_Advance").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(58).Value) = 0 Then .Rows(n).Cells(58).Value = ""

                            .Rows(n).Cells(59).Value = Format(Val(dt2.Rows(i).Item("Salary_Advance").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(59).Value) = 0 Then .Rows(n).Cells(59).Value = ""

                            .Rows(n).Cells(60).Value = Format(Val(dt2.Rows(i).Item("Salary_Pending").ToString), "###########0.00")
                            If Val(.Rows(n).Cells(60).Value) = 0 Then .Rows(n).Cells(60).Value = ""

                            .Rows(n).Cells(61).Value = Format(Val(dt2.Rows(i).Item("Net_Pay_Amount").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(61).Value) = 0 Then .Rows(n).Cells(61).Value = ""

                            .Rows(n).Cells(62).Value = Val(dt2.Rows(i).Item("Day_For_Bonus").ToString)
                            If Val(.Rows(n).Cells(62).Value) = 0 Then .Rows(n).Cells(62).Value = ""

                            .Rows(n).Cells(63).Value = Val(dt2.Rows(i).Item("Earning_For_Bonus").ToString)
                            If Val(.Rows(n).Cells(63).Value) = 0 Then .Rows(n).Cells(63).Value = ""


                            .Rows(n).Cells(64).Value = Val(dt2.Rows(i).Item("OT_Minutes").ToString)
                            If Val(.Rows(n).Cells(64).Value) = 0 Then .Rows(n).Cells(64).Value = ""

                            .Rows(n).Cells(65).Value = Val(dt2.Rows(i).Item("Add_CL_Leaves").ToString)
                            If Val(.Rows(n).Cells(65).Value) = 0 Then .Rows(n).Cells(65).Value = ""

                            .Rows(n).Cells(66).Value = Val(dt2.Rows(i).Item("Add_SL_Leaves").ToString)
                            If Val(.Rows(n).Cells(66).Value) = 0 Then .Rows(n).Cells(66).Value = ""

                            .Rows(n).Cells(67).Value = Val(dt2.Rows(i).Item("Leave_Salary_Less").ToString)
                            If Val(.Rows(n).Cells(67).Value) = 0 Then .Rows(n).Cells(67).Value = ""

                            .Rows(n).Cells(68).Value = Val(dt2.Rows(i).Item("Actual_Salary").ToString)
                            If Val(.Rows(n).Cells(68).Value) = 0 Then .Rows(n).Cells(68).Value = ""

                            .Rows(n).Cells(69).Value = Val(dt2.Rows(i).Item("Opening_Advance").ToString)
                            If Val(.Rows(n).Cells(69).Value) = 0 Then .Rows(n).Cells(69).Value = ""

                            .Rows(n).Cells(70).Value = False
                            If Val(dt2.Rows(i).Item("Signature_Status").ToString) = 1 Then
                                .Rows(n).Cells(70).Value = True
                            End If

                            .Rows(n).Cells(71).Value = Format(Val(dt2.Rows(i).Item("ESI_AUDIT").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(71).Value) = 0 Then .Rows(n).Cells(71).Value = ""

                            .Rows(n).Cells(72).Value = Format(Val(dt2.Rows(i).Item("PF_AUDIT").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(72).Value) = 0 Then .Rows(n).Cells(72).Value = ""

                            .Rows(n).Cells(73).Value = Format(Val(dt2.Rows(i).Item("E_P_F_AUDIT").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(73).Value) = 0 Then .Rows(n).Cells(73).Value = ""

                            .Rows(n).Cells(74).Value = Format(Val(dt2.Rows(i).Item("OT_ESI").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(74).Value) = 0 Then .Rows(n).Cells(74).Value = ""

                            .Rows(n).Cells(75).Value = Format(Val(dt2.Rows(i).Item("SALARY_OT_ESI").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(75).Value) = 0 Then .Rows(n).Cells(75).Value = ""

                            .Rows(n).Cells(76).Value = Format(Val(dt2.Rows(i).Item("E_P_S_AUDIT").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(76).Value) = 0 Then .Rows(n).Cells(76).Value = ""

                            .Rows(n).Cells(77).Value = Format(Val(dt2.Rows(i).Item("PF_Credit_Amount").ToString), "##########0.00")
                            If Val(.Rows(n).Cells(77).Value) = 0 Then .Rows(n).Cells(77).Value = ""

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

            End If

            Grid_Cell_DeSelect()

            ShowOrHideColumns()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            TotalNettPay()

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

        NoCalc_Status = False
    End Sub

    Private Sub get_PayRoll_Salary_Details()

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim da5 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable


        Dim vEmp_IdNo As Integer = 0
        Dim vCatgry_IdNo As Integer = 0
        Dim vEmpCatgry_IdNo As Integer = 0
        Dim n As Integer = 0
        Dim vNoOf_Wrk_Dys_Frm_MessAtt As Integer = 0
        Dim Late_Mins As Double = 0
        Dim Late_Hours As Double = 0
        Dim vNoOf_Wrkd_Dys As Double = 0
        Dim OT_wrk_dys As Double = 0
        Dim vIncenAmt_FromAtt As Double = 0
        Dim vMas_BasSal_PerShift_PerMonth As Double = 0
        Dim vSal_Shift As Double = 0
        Dim Bas_Sal As Double = 0
        Dim OT_Sal_Shft As Double = 0
        Dim OT_Salary As Double = 0
        Dim PerLeave_Ded As Double
        Dim Amt_OpBal As Double
        Dim Cmp_Cond As String = ""
        Dim mins_Adv As Double = 0
        Dim mess_Ded As Double = 0

        Dim DA_Amt As Double = 0, DA_Shft As Double = 0
        Dim HRA_Amt As Double = 0
        Dim Convey_Salary_Amt As Double = 0
        Dim Convey_PF_Amt As Double = 0
        Dim Washing_Amt As Double = 0
        Dim Entertainment_Amt As Double = 0
        Dim Prrovision_Amt As Double = 0
        Dim Maintain_Amt As Double = 0
        Dim Other_add1_Amt As Double = 0
        Dim Other_add2_Amt As Double = 0
        Dim vMas_WeekOff_Allow_Amt_PerDay As Double = 0
        Dim CL_Leaves As Double = 0
        Dim SL_Leaves As Double = 0
        Dim CL_Leaves_Current As Double = 0
        Dim SL_Leaves_Current As Double = 0
        Dim Less_CL_Leaves As Double = 0
        Dim Less_SL_Leaves As Double = 0

        Dim Advance_Ded_Entry As Double = 0
        Dim Mess_Ded_Entry As Double = 0
        Dim Medical_Ded_Entry As Double = 0
        Dim Store_Ded_Entry As Double = 0
        Dim Others_Add_Ded_Entry As Double = 0
        Dim Others_Ded_Ded_Entry As Double = 0

        Dim H As Long = 0, M As Long = 0

        Dim OTHrs As String = ""
        Dim OT_Mins As Long = 0, Tot_OTMins As Long = 0
        Dim Ot_Dbl As Double = 0
        Dim Ot_Int As Long = 0
        Dim Ot_minVal As Long = 0

        Dim Incentive As Single

        Dim PerLeaveHrs As String = ""                'PerLeave = Permission Leave
        Dim PerLeave_Mins As Long = 0, Tot_PerLeaveMins As Long = 0
        Dim PerLeave_Dbl As Double = 0
        Dim PerLeave_Int As Long = 0
        Dim PerLeave_minVal As Long = 0

        Dim Net_Salary As Double = 0
        Dim Net_Pay As Double = 0
        Dim Salary_Pending As Double = 0
        Dim Ttl_Days As Double = 0
        Dim SNo As Long = 0, Nr As Long = 0

        Dim SalPymtTyp_IdNo As Integer = 0

        '  Dim PrevEnt_RefDate1 As Date, PrevEnt_RefDate2 As Date
        Dim PrevEnt_RefNo As String = ""
        Dim EntOrdBy As Single = 0, PrevEnt_OrdBy As Single = 0
        Dim AdvDtTm As Date
        Dim NewCode As String = ""
        Dim Shft_Hours As Double = 0
        Dim Shft_Mins As Double = 0
        Dim Sal_advance As Double = 0
        Dim Att_Incentive As Double = 0
        Dim Cat_Idno As Integer = 0
        Dim CL_STS As Integer
        Dim SL_STS As Integer
        Dim thisMonth As Integer = 0
        Dim dtc As Date
        Dim Tot_LeaveDys_In_Mnth As Double = 0
        Dim Tot_WeekOff_Days As Double = 0
        Dim Tot_FH_Dys As Double = 0
        Dim vNoOf_Att_Dys_In_FH As Double = 0
        Dim vNoOf_FH_Dys_On_WkOff As Single = 0
        Dim vNoOf_Att_Dys_In_WkOff As Double = 0
        Dim vNoOf_FH_Dys_For_Sal As Single = 0
        Dim Tot_Noof_WrkdShft_Frm_Att As Single = 0
        Dim WeekOff_ADD_Opening As Single = 0
        Dim WeekOff_LESS_Opening As Single = 0
        Dim Dys As Integer = 0
        Dim Late_sts As Boolean = False
        Dim Late_Minimum_Mins As Integer = 0
        Dim Late_Deduct_per_Mins As Single = 0
        Dim vNet_LeaveDys_In_Mnth As Single
        Dim vNo_Days_InMonth_for_MonthlyWages As Double = 0
        Dim BASIC_SALARY_FOR_PF_CALCULATION As Double = 0
        Dim Less_Advance_Col_Edit_STS As Boolean = True
        Dim vTotErngs_FOR_ESI As String = 0
        Dim vPFSTS_Sal As Integer = 0
        Dim vESISTS_Sal As Integer = 0
        Dim vPFSTS_Audit As Integer = 0
        Dim vESISTS_Audit As Integer = 0
        Dim weekof_mins As Integer = 0
        Dim OT_SALARY_ESI As Double
        Dim Salary_plus_ot_esi As Double

        If FrmLdSTS = True Then Exit Sub

        Less_Advance_Col_Edit_STS = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NoCalc_Status = True

        ESI_MAX_SHFT_WAGES = 0
        EPF_MAX_BASICPAY = 0

        da1 = New SqlClient.SqlDataAdapter("select * from " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..Settings_Head", con)
        dt1 = New DataTable
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0).Item("Basic_Wages_For_Esi").ToString()) = False Then
                ESI_MAX_SHFT_WAGES = Val(dt1.Rows(0).Item("Basic_Wages_For_Esi").ToString)
            End If
            If IsDBNull(dt1.Rows(0).Item("Basic_Pay_For_Epf").ToString()) = False Then
                EPF_MAX_BASICPAY = Val(dt1.Rows(0).Item("Basic_Pay_For_Epf").ToString)
            End If
        End If

        dt1.Dispose()
        da1.Dispose()

        'If EPF_MAX_BASICPAY = 0 Then EPF_MAX_BASICPAY = 15000

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con

        btn_Calculation_Salary.BackColor = Color.Blue
        Application.DoEvents()

        SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

        vCatgry_IdNo = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)

        EntOrdBy = Val(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text)))

        txt_TotalDays.Text = DateDiff(DateInterval.Day, dtp_FromDate.Value.Date, dtp_ToDate.Value.Date) + 1


        cmd.CommandText = "Truncate table EntryTemp"
        cmd.ExecuteNonQuery()

        '---Day Name from Previous Month To Next Month

        dtc = dtp_FromDate.Value.Date.AddMonths(-1)

        Do While (dtc <= dtp_ToDate.Value.Date.AddMonths(1))
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@Date", dtc)
            cmd.CommandText = ("Insert into EntryTemp(Date1, name1) values (@Date, '" & Trim(UCase(Format(dtc, "dddd"))) & "')")

            cmd.ExecuteNonQuery()
            dtc = Format(dtc.AddDays(1), "dd/MM/yyyy")
        Loop



        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
        cmd.Parameters.AddWithValue("@SalaryDate", dtp_Date.Value.Date)
        cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)


        '====== No of Festival Holidays In this Month  ========== 

        txt_FestivalDays.Text = ""

        cmd.CommandText = "select count(*) as NoOf_FH_Days from Holiday_Details where HolidayDateTime between @FromDate and @ToDate "
        Da = New SqlClient.SqlDataAdapter(cmd)
        dt4 = New DataTable
        Da.Fill(dt4)
        If dt4.Rows.Count > 0 Then
            If IsDBNull(dt4.Rows(0).Item("NoOf_FH_Days").ToString) = False Then
                txt_FestivalDays.Text = Val(dt4.Rows(0).Item("NoOf_FH_Days").ToString)
            End If
        End If
        dt4.Clear()

        Dim vSQLCondt As String = ""

        vSQLCondt = "" ' " (a.Employee_IdNo = 144 or a.Employee_IdNo = 212 or a.Employee_IdNo = 210 or a.Employee_IdNo = 145 or a.Employee_IdNo = 230 or a.Employee_IdNo = 225 or a.Employee_IdNo = 288) "

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then
            vSQLCondt = Trim(vSQLCondt) & IIf(Trim(vSQLCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If
        If Val(SalPymtTyp_IdNo) <> 0 Then
            vSQLCondt = Trim(vSQLCondt) & IIf(Trim(vSQLCondt) <> "", " and ", "") & " a.Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo))
        End If
        If Val(vCatgry_IdNo) <> 0 Then
            vSQLCondt = Trim(vSQLCondt) & IIf(Trim(vSQLCondt) <> "", " and ", "") & " a.Category_IdNo = " & Str(Val(vCatgry_IdNo))
        End If


        cmd.CommandText = "select a.*, b.* from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo <> 0 and a.Category_IdNo = b.Category_IdNo Where " & vSQLCondt & IIf(vSQLCondt <> "", " and ", "") & " a.Join_DateTime <= @ToDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @FromDate ) ) order by a.Employee_Name"

        da1 = New SqlClient.SqlDataAdapter(cmd)
        dt1 = New DataTable
        da1.Fill(dt1)

        ' MsgBox(dt1.Rows.Count)

        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                '---Progress Bar
                pnl_ProgressBar.Visible = True

                ProgBar1.Minimum = 0
                ProgBar1.Maximum = dt1.Rows.Count - 1

                '------------

                For i = 0 To dt1.Rows.Count - 1

                    lbl_ProPerc.Text = CInt((100 / Val(dt1.Rows.Count)) * i) & "%"
                    Application.DoEvents()
                    ProgBar1.Value = i

                    vEmp_IdNo = Val(dt1.Rows(i).Item("Employee_IdNo").ToString)
                    vEmpCatgry_IdNo = Val(dt1.Rows(i).Item("Category_IdNo").ToString)


                    vNo_Days_InMonth_for_MonthlyWages = 26
                    If Val(Common_Procedures.settings.NoOfDays_For_Month_Wages_Take_TotalDays_In_Month) = 1 Then
                        vNo_Days_InMonth_for_MonthlyWages = Val(txt_TotalDays.Text)

                    Else

                        If Val(dt1.Rows(i).Item("No_Days_Month_Wages").ToString) <> 0 Then
                            vNo_Days_InMonth_for_MonthlyWages = Val(dt1.Rows(i).Item("No_Days_Month_Wages").ToString)
                        End If

                    End If

                    'If Val(vEmp_IdNo) = 539 Then
                    '    Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
                    'End If

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("@SalaryDate", dtp_Date.Value.Date)
                    cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

                    If dtp_Advance_UpToDate.Visible = True Then
                        cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_Advance_UpToDate.Value.Date)
                    Else
                        cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_ToDate.Value.Date)
                    End If

                    thisMonth = 0
                    If dtp_Date.Value.Date > Common_Procedures.Company_FromDate And dtp_Date.Value.Date < Common_Procedures.Company_ToDate And Trim(cbo_Month.Text) = "MARCH" Then
                        thisMonth = Month(dtp_Date.Value.Date)
                    End If

                    Late_sts = IIf(Val(dt1.Rows(i).Item("Time_Delay").ToString) = 1, True, False)
                    Late_Minimum_Mins = Val(dt1.Rows(i).Item("Minimum_Delay").ToString)
                    Late_Deduct_per_Mins = Val(dt1.Rows(i).Item("Less_Minute_Delay").ToString)

                    '----Calculating No Of Leave days For Monthly wages

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp( Int1             , Meters1 ) " & _
                                                  " Select      a.Employee_IdNo , 0      from PayRoll_Employee_Head a Where a.Employee_IdNo =" & Str(Val(vEmp_IdNo))
                    Nr = cmd.ExecuteNonQuery()

                    '----getting No Of days absent from attendance
                    cmd.CommandText = "Insert into ReportTemp( Int1                ,         Meters1        ) " & _
                                                  " Select      a.Employee_IdNo    , count(a.No_Of_Shift)   from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c  Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift = 0 and b.shift_Day_Month = 'MONTH' and a.Employee_IdNo = b.Employee_IdNo and a.Employee_Attendance_Date = c.Date1 and b.Week_Off <> c.Name1 and a.Employee_Attendance_Date NOT IN (select z1.HolidayDateTime from Holiday_Details z1) group by a.Employee_IdNo"
                    Nr = cmd.ExecuteNonQuery()

                    '--- Suppose attendance not entered on that date
                    cmd.CommandText = "Insert into ReportTemp(Int1        ,   Meters1 )    " & _
                                                "select    a.Employee_IdNo, count(*)  from PayRoll_Employee_Head a, EntryTemp b where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and b.Date1 Between @FromDate and @ToDate and b.Date1 NOT IN (select z1.Employee_Attendance_Date from PayRoll_Employee_Attendance_Details z1 where z1.Employee_Attendance_Date between @FromDate and @ToDate and z1.Employee_IdNo = a.Employee_IdNo) and a.Week_Off <> b.name1 and b.Date1 NOT IN (select z2.HolidayDateTime from Holiday_Details z2) group by a.Employee_IdNo"
                    Nr = cmd.ExecuteNonQuery()

                    '----getting No Of days greater than 1 (ie 1.5 shift) and reducing it in leave
                    cmd.CommandText = "Insert into ReportTemp( Int1                ,         Meters1        ) " & _
                                                  " Select      a.Employee_IdNo    , (1 - a.No_Of_Shift)   from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift > 1 and b.shift_Day_Month = 'MONTH' and a.Employee_IdNo = b.Employee_IdNo and a.Employee_Attendance_Date = c.Date1 and b.Week_Off <> c.Name1 and a.Employee_Attendance_Date NOT IN (select z1.HolidayDateTime from Holiday_Details z1) "
                    cmd.ExecuteNonQuery()

                    '----getting No Of days Lesser than 1(ie half shift) and adding it in leave
                    cmd.CommandText = "Insert into ReportTemp( Int1                ,         Meters1        ) " & _
                                                  " Select      a.Employee_IdNo    , (1 - a.No_Of_Shift)   from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and (a.No_Of_Shift > 0 and a.No_Of_Shift < 1) and b.shift_Day_Month = 'MONTH' and a.Employee_IdNo = b.Employee_IdNo and a.Employee_Attendance_Date = c.Date1 and b.Week_Off <> c.Name1 and a.Employee_Attendance_Date NOT IN (select z1.HolidayDateTime from Holiday_Details z1) "
                    Nr = cmd.ExecuteNonQuery()

                    '---- No.Of.leave - for month Wages

                    Tot_LeaveDys_In_Mnth = 0

                    cmd.CommandText = "select sum(Meters1) as NoOfLeave from Reporttemp Where int1 = " & Str(Val(vEmp_IdNo))
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)

                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0).Item("NoOfLeave").ToString) = False Then
                            Tot_LeaveDys_In_Mnth = Val(dt4.Rows(0).Item("NoOfLeave").ToString)
                        End If
                    End If
                    dt4.Clear()

                    '======Getting Total WeekOff Days fo this employee ========== 

                    Tot_WeekOff_Days = 0

                    cmd.CommandText = "select count(*) AS WeekOff from PayRoll_Employee_Head a, EntryTemp b where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and " & _
                                            " b.Date1 Between @FromDate and @ToDate and a.Week_Off = b.name1"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0).Item("WeekOff").ToString) = False Then
                            Tot_WeekOff_Days = Str(Val(dt4.Rows(0).Item("WeekOff").ToString))
                        End If
                    End If
                    dt4.Clear()


                    '====== No of Festival Holidays In this Month  ========== 

                    Tot_FH_Dys = 0
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '----Southern Cot Spinners
                        cmd.CommandText = "Select count(*) as NoOf_FH_Days from PayRoll_Employee_Head a, Holiday_Details b Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and b.HolidayDateTime between @FromDate and @ToDate and a.Employee_IdNo IN (select sq1.Employee_IdNo from PayRoll_Employee_Attendance_Details sq1 where sq1.Employee_Attendance_Date = (select max(z1.Date1) from EntryTemp z1 Where z1.Date1 between @FromDate and @ToDate and z1.Name1 <> a.Week_Off and z1.Date1 Between @FromDate and dateadd(dd, -1, b.HolidayDateTime)) and sq1.No_Of_Shift > 0)  and a.Employee_IdNo IN (select sq2.Employee_IdNo from PayRoll_Employee_Attendance_Details sq2 where sq2.Employee_Attendance_Date = (select min(z2.Date1) from EntryTemp z2 Where z2.Date1 between @FromDate and @ToDate and a.Week_Off <> z2.Name1 and z2.Date1 Between dateadd(dd, 1, b.HolidayDateTime) and @ToDate) and sq2.No_Of_Shift > 0)"
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)
                        If dt4.Rows.Count > 0 Then
                            If IsDBNull(dt4.Rows(0).Item("NoOf_FH_Days").ToString) = False Then
                                Tot_FH_Dys = Str(Val(dt4.Rows(0).Item("NoOf_FH_Days").ToString))
                            End If
                        End If
                        dt4.Clear()

                    Else
                        Tot_FH_Dys = Val(txt_FestivalDays.Text)

                    End If


                    '====== No of Festival Holidays In WeekOf for this Employee ==========

                    vNoOf_FH_Dys_On_WkOff = 0
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '----Southern Cot Spinners
                        cmd.CommandText = "Select count(*) as NoOf_FHDays_In_WeekOFF from PayRoll_Employee_Head a, Holiday_Details b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and b.HolidayDateTime between @FromDate and @ToDate and c.Date1 between @FromDate and @ToDate and c.Name1 = a.Week_Off and b.HolidayDateTime = c.Date1 and a.Employee_IdNo IN (select sq1.Employee_IdNo from PayRoll_Employee_Attendance_Details sq1 where sq1.Employee_Attendance_Date = (select max(z1.Date1) from EntryTemp z1 Where z1.Date1 between @FromDate and @ToDate and z1.Name1 <> a.Week_Off and z1.Date1 Between @FromDate and dateadd(dd, -1, b.HolidayDateTime)) and sq1.No_Of_Shift > 0)  and a.Employee_IdNo IN (select sq2.Employee_IdNo from PayRoll_Employee_Attendance_Details sq2 where sq2.Employee_Attendance_Date = (select min(z2.Date1) from EntryTemp z2 Where z2.Date1 between @FromDate and @ToDate and a.Week_Off <> z2.Name1 and z2.Date1 Between dateadd(dd, 1, b.HolidayDateTime) and @ToDate) and sq2.No_Of_Shift > 0)"
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)
                        If dt4.Rows.Count > 0 Then
                            If IsDBNull(dt4.Rows(0).Item("NoOf_FHDays_In_WeekOFF").ToString) = False Then
                                vNoOf_FH_Dys_On_WkOff = Str(Val(dt4.Rows(0).Item("NoOf_FHDays_In_WeekOFF").ToString))
                            End If
                        End If
                        dt4.Clear()

                    Else
                        cmd.CommandText = "select count(*) as NoOf_FHDays_In_WeekOFF from PayRoll_Employee_Head a, Holiday_Details b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and b.HolidayDateTime between @FromDate and @ToDate and c.Date1 between @FromDate and @ToDate and c.Name1 = a.Week_Off and b.HolidayDateTime = c.Date1"
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)
                        If dt4.Rows.Count > 0 Then
                            If IsDBNull(dt4.Rows(0).Item("NoOf_FHDays_In_WeekOFF").ToString) = False Then
                                vNoOf_FH_Dys_On_WkOff = Str(Val(dt4.Rows(0).Item("NoOf_FHDays_In_WeekOFF").ToString))
                            End If
                        End If
                        dt4.Clear()

                    End If

                    Tot_FH_Dys = Tot_FH_Dys - vNoOf_FH_Dys_On_WkOff


                    '====== No of days Present in Festival Holidays for this Employee and not in week off ==========

                    vNoOf_Att_Dys_In_FH = 0
                    cmd.CommandText = "Select sum(No_Of_Shift) as Attandance_In_FH_Days from PayRoll_Employee_Attendance_Details a, Holiday_Details b, PayRoll_Employee_Head c, EntryTemp d Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and b.HolidayDateTime between @FromDate and @ToDate and a.Employee_IdNo = c.Employee_IdNo and a.Employee_Attendance_Date = b.HolidayDateTime and d.Date1 between @FromDate and @ToDate and a.Employee_Attendance_Date = d.Date1 and c.Week_Off <> d.Name1 "
                    'cmd.CommandText = "Select count(*) as Attandance_In_FH_Days from PayRoll_Employee_Attendance_Details a, Holiday_Details b, PayRoll_Employee_Head c, EntryTemp d Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and b.HolidayDateTime between @FromDate and @ToDate and a.Employee_IdNo = c.Employee_IdNo and a.Employee_Attendance_Date = b.HolidayDateTime and d.Date1 between @FromDate and @ToDate and a.Employee_Attendance_Date = d.Date1 and c.Week_Off <> d.Name1 "
                    'cmd.CommandText = "Select count(*) as Attandance_In_FH_Days from PayRoll_Employee_Attendance_Details a, Holiday_Details b Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and b.HolidayDateTime between @FromDate and @ToDate and a.Employee_Attendance_Date = b.HolidayDateTime"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0).Item("Attandance_In_FH_Days").ToString) = False Then
                            vNoOf_Att_Dys_In_FH = Str(Val(dt4.Rows(0).Item("Attandance_In_FH_Days").ToString))
                        End If
                    End If
                    dt4.Clear()

                    '====== No of days Present in WeekOff for this Employee, if weekoff and fh in same it is taken in week of days onlys ==========

                    vNoOf_Att_Dys_In_WkOff = 0
                    cmd.CommandText = "Select sum(No_Of_Shift) as Attandance_In_WeekOff_Days from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and a.Employee_IdNo = b.Employee_IdNo and c.Date1 between @FromDate and @ToDate and a.Employee_Attendance_Date = c.Date1 and b.Week_Off = c.Name1 "
                    'cmd.CommandText = "Select count(*) as Attandance_In_WeekOff_Days from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and a.Employee_IdNo = b.Employee_IdNo and c.Date1 between @FromDate and @ToDate and a.Employee_Attendance_Date = c.Date1 and b.Week_Off = c.Name1 "
                    'cmd.CommandText = "Select count(*) as Attandance_In_WeekOff_Days from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and a.Employee_IdNo = b.Employee_IdNo and c.Date1 between @FromDate and @ToDate and a.Employee_Attendance_Date = c.Date1 and b.Week_Off = c.Name1 and a.Employee_Attendance_Date NOT IN (select z1.HolidayDateTime from Holiday_Details z1) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0).Item("Attandance_In_WeekOff_Days").ToString) = False Then
                            vNoOf_Att_Dys_In_WkOff = Str(Val(dt4.Rows(0).Item("Attandance_In_WeekOff_Days").ToString))
                        End If
                    End If
                    dt4.Clear()


                    '====== taking Opening Weekoff from Previous Month Salary Details ==========
                    WeekOff_ADD_Opening = 0
                    WeekOff_LESS_Opening = 0

                    If Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 1 Then   '---- CarryOn Previous Month WeekOff
                        cmd.CommandText = "select sum(a.Add_W_Off_CR) as Add_WeekOff_Opening ,sum(a.Less_W_Off_CR) as Less_WeekOff_Opening  from PayRoll_Salary_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Salary_Date < @fromdate  and  a.Salary_Date >= @CompFromDate "
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)
                        If dt4.Rows.Count > 0 Then
                            If IsDBNull(dt4.Rows(0).Item("Add_WeekOff_Opening").ToString) = False Then
                                If Val(dt4.Rows(0).Item("Add_WeekOff_Opening").ToString) <> 0 Then
                                    WeekOff_ADD_Opening = Str(Val(dt4.Rows(0).Item("Add_WeekOff_Opening").ToString))
                                End If
                            End If
                            If IsDBNull(dt4.Rows(0).Item("Less_WeekOff_Opening").ToString) = False Then
                                If Val(dt4.Rows(0).Item("Less_WeekOff_Opening").ToString) <> 0 Then
                                    WeekOff_LESS_Opening = Str(Val(dt4.Rows(0).Item("Less_WeekOff_Opening").ToString))
                                End If
                            End If

                        End If

                    End If
                    dt4.Clear()


                    '-----Current Month CL ,SL Leave  Status
                    CL_STS = Val(dt1.Rows(i).Item("CL_Leave").ToString)
                    SL_STS = Val(dt1.Rows(i).Item("SL_Leave").ToString)

                    CL_Leaves = 0
                    SL_Leaves = 0
                    CL_Leaves_Current = 0
                    SL_Leaves_Current = 0
                    Less_CL_Leaves = 0
                    Less_SL_Leaves = 0

                    '---CL ,SL Opening From Opening Entry
                    cmd.CommandText = "select sum(a.Opening_CL_Leaves) as CL_Opening  ,sum(a.Opening_ML_Leaves) as SL_Opening from PayRoll_Employee_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo))
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                            If Trim(dt1.Rows(i).Item("CL_Arrear_Type_Year").ToString) = "CARRY ON" Or Trim(dt1.Rows(i).Item("CL_Arrear_Type_Year").ToString) = "SALARY" Then  '-----Opening CL Leaves Carry on 
                                If Val(dt4.Rows(0).Item("CL_Opening").ToString) <> 0 Then
                                    If CL_STS <> 0 Then
                                        CL_Leaves = Str(Val(dt4.Rows(0).Item("CL_Opening").ToString))
                                    Else
                                        CL_Leaves = 0
                                    End If
                                Else
                                    CL_Leaves = 0
                                End If

                            End If
                            If Trim(dt1.Rows(i).Item("SL_Arrear_Type_Year").ToString) = "CARRY ON" Or Trim(dt1.Rows(i).Item("SL_Arrear_Type_Year").ToString) = "SALARY" Then
                                If Val(dt4.Rows(0).Item("SL_Opening").ToString) <> 0 Then
                                    If SL_STS <> 0 Then
                                        SL_Leaves = Str(Val(dt4.Rows(0).Item("SL_Opening").ToString))
                                    Else
                                        SL_Leaves = 0
                                    End If

                                End If
                            End If

                        End If
                    End If
                    dt4.Clear()


                    '-----Opening CL ,SL from Previous Month
                    cmd.CommandText = "select sum(a.Add_CL_Leaves) as CL_Opening  ,sum(a.Add_SL_Leaves) as SL_Opening , sum(a.Less_CL_CR_Days) as UsedCL ,sum(a.Less_SL_CR_Days) as UsedSL from PayRoll_Salary_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Salary_Date < @fromdate  and  a.Salary_Date >= @CompFromDate "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If Trim(dt1.Rows(i).Item("CL_Arrear_Type").ToString) = "CARRY ON" Then   '----CarryOn Previous Month CL
                            If IsDBNull(dt4.Rows(0).Item("CL_Opening").ToString) = False Then
                                If Val(dt4.Rows(0).Item("CL_Opening").ToString) <> 0 Then
                                    If CL_STS <> 0 Then
                                        CL_Leaves = CL_Leaves + Str(Val(dt4.Rows(0).Item("CL_Opening").ToString))
                                    Else
                                        CL_Leaves = 0
                                    End If
                                End If
                            End If
                            If IsDBNull(dt4.Rows(0).Item("UsedCL").ToString) = False Then
                                If Val(dt4.Rows(0).Item("UsedCL").ToString) <> 0 Then
                                    If CL_STS <> 0 Then
                                        Less_CL_Leaves = Str(Val(dt4.Rows(0).Item("UsedCL").ToString))
                                    Else
                                        Less_CL_Leaves = 0
                                    End If
                                End If
                            End If
                        Else
                            CL_Leaves = 0
                            Less_CL_Leaves = 0

                        End If

                        If Trim(dt1.Rows(i).Item("SL_Arrear_Type").ToString) = "CARRY ON" Then
                            If IsDBNull(dt4.Rows(0).Item("SL_Opening").ToString) = False Then
                                If Val(dt4.Rows(0).Item("SL_Opening").ToString) <> 0 Then
                                    If SL_STS <> 0 Then
                                        SL_Leaves = SL_Leaves + Str(Val(dt4.Rows(0).Item("SL_Opening").ToString))
                                    Else
                                        SL_Leaves = 0
                                    End If
                                End If
                            End If

                            If IsDBNull(dt4.Rows(0).Item("UsedSL").ToString) = False Then
                                If Val(dt4.Rows(0).Item("UsedSL").ToString) <> 0 Then
                                    If SL_STS <> 0 Then
                                        Less_SL_Leaves = Str(Val(dt4.Rows(0).Item("UsedSL").ToString))
                                    Else
                                        Less_SL_Leaves = 0
                                    End If
                                End If
                            End If

                        Else
                            SL_Leaves = 0
                            Less_SL_Leaves = 0

                        End If

                    End If
                    dt4.Clear()

                    If Val(vEmp_IdNo) = 136 Then
                        Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
                    End If

                    AdvDtTm = #1/1/1990#

                    cmd.CommandText = "Select b.Advance_UptoDate from PayRoll_Salary_Details a INNER JOIN PayRoll_Salary_Head b ON a.Salary_Code = b.Salary_Code Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and (a.Salary_Date < @SalaryDate or (a.Salary_Date = @SalaryDate and a.for_OrderBy < " & Str(Val(EntOrdBy)) & ") ) order by  b.Advance_UptoDate desc ,a.Salary_Date desc"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)

                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                            If IsDate(dt4.Rows(0)(0).ToString) = True Then
                                AdvDtTm = dt4.Rows(0)(0)
                            End If
                        End If
                    End If
                    dt4.Clear()

                    AdvDtTm = DateAdd(DateInterval.Day, 1, AdvDtTm)
                    cmd.Parameters.AddWithValue("@PreviousAdvanceDate", AdvDtTm)

                    Amt_OpBal = 0

                    '---- Opening Advance Amount

                    cmd.CommandText = " Select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and " & _
                                      " a.voucher_date <= @AdvanceUpToDate and a.ledger_idno = b.ledger_idno and a.company_idno = tZ.company_idno " & _
                                      " and not a.Entry_Identification = '" & Trim(Pk_Condition) & Trim(Val(vEmp_IdNo)) & "/" & Trim(NewCode) & "'" & _
                                      " and not a.Entry_Identification = '" & Trim(Pk_Condition3) & Trim(Val(vEmp_IdNo)) & "/" & Trim(NewCode) & "'" & _
                                      " and (a.Voucher_Code LIKE 'ADVOP-%' or a.Voucher_Code LIKE 'EADPY-%' or a.Voucher_Code LIKE 'ADVLS-%' or a.Voucher_Code LIKE 'AVLDD-%')"


                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                            If Val(dt4.Rows(0)(0).ToString) < 0 Then
                                Amt_OpBal = Format(-1 * Val(dt4.Rows(0)(0).ToString), "##########0.00")
                            End If
                        End If
                    End If
                    dt4.Clear()


                    Salary_Pending = 0
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL

                        '-----Salary Pending for previous Month
                        cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Pending from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <>0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Voucher_Date < @PreviousAdvanceDate and  (a.Voucher_Code NOT LIKE 'ADVOP-%' and a.Voucher_Code NOT LIKE 'EADPY-%' and a.Voucher_Code NOT LIKE 'ADVLS-%' and a.Voucher_Code NOT LIKE 'ESAPY-%' and a.Voucher_Code NOT LIKE 'AVLSD-%') "
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)
                        If dt4.Rows.Count > 0 Then
                            If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                                'If Val(dt4.Rows(0).Item("Sal_Pending").ToString) > 0 Then
                                Salary_Pending = Format(Math.Abs(Val(dt4.Rows(0)(0).ToString)), "##########0.00")
                                'End If
                            End If
                        End If
                        dt4.Clear()


                        cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Paid_Amt from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Voucher_Date between @PreviousAdvanceDate and @ToDate and a.Entry_Identification LIKE 'ESLPY-%'"
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)
                        If dt4.Rows.Count > 0 Then
                            If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                                'If Val(dt4.Rows(0).Item("cr_Amt").ToString) > 0 Then
                                Salary_Pending = Format((Salary_Pending + Math.Abs(Val(dt4.Rows(0)(0).ToString))), "##########0.00")
                                'End If
                            End If
                        End If
                        dt4.Clear()

                    End If


                    Sal_advance = 0
                    cmd.CommandText = "Select sum(a.Amount) as Sal_Advance from PayRoll_Employee_Payment_Head a Where  a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Payment_Date between @PreviousAdvanceDate and @AdvanceUpToDate and a.Advance_Salary = 'SALARYADVANCE'"
                    'cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Advance from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Voucher_Date between @PreviousAdvanceDate and @AdvanceUpToDate and a.Voucher_Amount < 0 and a.Entry_Identification LIKE 'ESAPY-%'"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                            Sal_advance = Format(Math.Abs(Val(dt4.Rows(0).Item("Sal_Advance").ToString)), "##########0.00")
                        End If
                    End If
                    dt4.Clear()

                    'Amt_OpBal = Amt_OpBal - Sal_advance

                    '====== taking Working Days, Daily Incentive amount, Mess Attendance from Employee Attendance ==========

                    Tot_Noof_WrkdShft_Frm_Att = 0
                    vIncenAmt_FromAtt = 0
                    vNoOf_Wrk_Dys_Frm_MessAtt = 0

                    cmd.CommandText = "select sum(a.No_Of_Shift) as Noof_Working_Days, sum(a.Mess_Attendance) as Mess_AttDays, Sum(a.Incentive_Amount) as IncenAmt from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("Noof_Working_Days").ToString) = False Then
                            Tot_Noof_WrkdShft_Frm_Att = Val(dt2.Rows(0).Item("Noof_Working_Days").ToString)
                        End If
                        'If IsDBNull(dt2.Rows(0).Item("IncenAmt").ToString) = False Then
                        'vIncenAmt_FromAtt = Format(Val(dt2.Rows(0).Item("IncenAmt").ToString), "########0.00")
                        'End If
                        If IsDBNull(dt2.Rows(0).Item("Mess_AttDays").ToString) = False Then
                            vNoOf_Wrk_Dys_Frm_MessAtt = Val(dt2.Rows(0).Item("Mess_AttDays").ToString)
                        End If
                    End If
                    dt2.Clear()


                    '------Late Mins
                    Late_Mins = 0
                    cmd.CommandText = "select SUM(A.Late_Minutes) AS LATE_MINS from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo where A.Late_Minutes > ( SELECT sum(zs1.Minimum_Delay) FROM PayRoll_Category_Head zs1 where zs1.Category_IdNo = " & Str(Val(vEmpCatgry_IdNo)) & ") and a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Attendance_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("LATE_MINS").ToString) = False Then
                            Late_Mins = Val(dt2.Rows(0).Item("LATE_MINS").ToString)
                        End If
                    End If
                    dt2.Clear()

                    '-------Ot mins

                    'OT_Mins = 0
                    'cmd.CommandText = "select Sum(A.OT_Minutes) as Ot_Mins from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where A.OT_Minutes > ( SELECT sum(zs1.OT_Allowed_After_Minutes) FROM PayRoll_Category_Head zs1  where zs1.Category_IdNo =  " & Str(Val(vEmpCatgry_IdNo)) & " ) and  a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Attendance_Date between @fromdate and @toDate "
                    'da2 = New SqlClient.SqlDataAdapter(cmd)
                    'dt2 = New DataTable
                    'da2.Fill(dt2)
                    'If dt2.Rows.Count > 0 Then
                    '    If IsDBNull(dt2.Rows(0).Item("Ot_Mins").ToString) = False Then
                    '        OT_Mins = Val(dt2.Rows(0).Item("Ot_Mins").ToString)
                    '    End If
                    'End If
                    'dt2.Clear()

                    '-------Ot mins FROM OT ENTRY

                    OT_Mins = 0
                    cmd.CommandText = "select Sum(A.OT_Minutes) as Ot_Mins from Payroll_Employee_OverTime_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where A.OT_Minutes > ( SELECT sum(zs1.OT_Allowed_After_Minutes) FROM PayRoll_Category_Head zs1  where zs1.Category_IdNo =  " & Str(Val(vEmpCatgry_IdNo)) & " ) and  a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Timing_OverTime_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("Ot_Mins").ToString) = False Then
                            OT_Mins = Val(dt2.Rows(0).Item("Ot_Mins").ToString)
                        End If
                    End If
                    dt2.Clear()

                    '------INCENTIVE FROM OT ENTRY

                    Incentive = 0
                    cmd.CommandText = "select Sum(A.PermissionLeaveTime_Minutes) as Permission_Minutes from Payroll_Employee_PermissionLeaveTime_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Timing_PermissionLeaveTime_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("Permission_Minutes").ToString) = False Then
                            PerLeave_Mins = Val(dt2.Rows(0).Item("Permission_Minutes").ToString)
                        End If
                    End If
                    dt2.Clear()

                    '------PERMISSION LEAVE FROM OT ENTRY

                    Incentive = 0
                    cmd.CommandText = "select Sum(A.Incentive_Amount) as Incentive from Payroll_Employee_Incentive_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Incentive_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("Incentive").ToString) = False Then
                            Incentive = Val(dt2.Rows(0).Item("Incentive").ToString)
                        End If
                    End If
                    dt2.Clear()

                    cmd.CommandText = "truncate table EntryTemp_Simple"
                    cmd.ExecuteNonQuery()
                    Nr = 0
                    cmd.CommandText = "Insert into EntryTemp_Simple(  Int1         ,       Currency1                                 ,       Currency2     ,        Currency3           ,   Currency4           ,       Currency5                  ,       Currency6                            ) " & _
                                        "select                     a.Employee_IdNo, sum(a.Advance_Deduction_Amount) as advance_ded  , sum(a.Mess) as Mess , sum(a.Medical) as Medical  , sum(a.Store) as Store , sum(a.Other_Addition) Others_Add , sum(a.Other_Deduction_Amount) as others_Ded  from PayRoll_Employee_Deduction_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Deduction_Date between @fromdate and @toDate group by a.Employee_IdNo"
                    Nr = cmd.ExecuteNonQuery()

                    Nr = 0
                    cmd.CommandText = "Insert into EntryTemp_Simple(  Int1         ,       Currency1                                 ,       Currency2     ,        Currency3           ,   Currency4           ,       Currency5                  ,       Currency6                            ) " & _
                                        "select                     a.Employee_IdNo, sum(a.Advance_Deduction_Amount) as advance_ded  , sum(a.Mess) as Mess , sum(a.Medical) as Medical  , sum(a.Store) as Store , sum(a.Other_Addition) Others_Add , sum(a.Other_Deduction) as others_Ded  from PayRoll_Employee_Deduction_Head a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Deduction_Date between @fromdate and @toDate  group by a.Employee_IdNo"
                    Nr = cmd.ExecuteNonQuery()

                    '----- Advance Deduction , Mess Amount , Medical amount , Store Amount , Other Additions and Otehr Deduction from Addition/Deduction Entry
                    cmd.CommandText = "select sum(Currency1) as advance_ded  , sum(Currency2) as Mess , sum(Currency3) as Medical  ,sum(Currency4) as Store  ,sum(Currency5) Others_Add , sum(Currency6) as others_Ded  from EntryTemp_Simple "
                    'cmd.CommandText = "select sum(a.Advance_Deduction_Amount) as advance_ded  , sum(a.Mess) as Mess , sum(a.Medical) as Medical  ,sum(a.Store) as Store  ,sum(a.Other_Addition) Others_Add ,sum(a.Other_Deduction) as others_Ded  from PayRoll_Employee_Deduction_Head a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Deduction_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("advance_ded").ToString) = False Then
                            Advance_Ded_Entry = Val(dt2.Rows(0).Item("advance_ded").ToString)
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Mess").ToString) = False Then
                            Mess_Ded_Entry = Val(dt2.Rows(0).Item("Mess").ToString)
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Medical").ToString) = False Then
                            Medical_Ded_Entry = Format(Val(dt2.Rows(0).Item("Medical").ToString), "########0.00")
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Store").ToString) = False Then
                            Store_Ded_Entry = Val(dt2.Rows(0).Item("Store").ToString)
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Others_Add").ToString) = False Then
                            Others_Add_Ded_Entry = Val(dt2.Rows(0).Item("Others_Add").ToString)
                        End If
                        If IsDBNull(dt2.Rows(0).Item("others_Ded").ToString) = False Then
                            Others_Ded_Ded_Entry = Val(dt2.Rows(0).Item("others_Ded").ToString)
                        End If
                    End If
                    dt2.Clear()


                    '----getting salary details from Employee Head
                    '----Salary Per Day/Shift ,OT Salary , DA ,HRA , Mess Deduction , Conveyance ,Washing ,Maintanance , Entertainment  ,CL , SL leaves

                    vMas_BasSal_PerShift_PerMonth = 0
                    OT_Sal_Shft = 0
                    OT_Salary = 0
                    mess_Ded = 0
                    DA_Amt = 0
                    HRA_Amt = 0
                    Convey_PF_Amt = 0
                    Convey_Salary_Amt = 0
                    Washing_Amt = 0
                    Maintain_Amt = 0
                    Other_add1_Amt = 0
                    Other_add2_Amt = 0
                    vMas_WeekOff_Allow_Amt_PerDay = 0
                    Entertainment_Amt = 0
                    Prrovision_Amt = 0

                    cmd.CommandText = "SELECT TOP 1 * from PayRoll_Employee_Salary_Details a Where a.employee_idno = " & Str(Val(vEmp_IdNo)) & " and ( ( @FromDate < (select min(y.From_DateTime) from PayRoll_Employee_Salary_Details y where y.employee_idno = a.employee_idno )) or (@FromDate BETWEEN a.From_DateTime and a.To_DateTime) or ( @FromDate >= (select max(z.From_DateTime) from PayRoll_Employee_Salary_Details z where z.employee_idno = a.employee_idno ))) order by a.From_DateTime desc"
                    da3 = New SqlClient.SqlDataAdapter(cmd)
                    dt3 = New DataTable
                    da3.Fill(dt3)

                    If dt3.Rows.Count > 0 Then

                        If IsDBNull(dt3.Rows(0).Item("For_Salary").ToString) = False Then
                            vMas_BasSal_PerShift_PerMonth = Format(Val(dt3.Rows(0).Item("For_Salary").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("O_T").ToString) = False Then
                            OT_Sal_Shft = Format(Val(dt3.Rows(0).Item("O_T").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("MessDeduction").ToString) = False Then
                            mess_Ded = Format(Val(dt3.Rows(0).Item("MessDeduction").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("D_A").ToString) = False Then
                            DA_Amt = Format(Val(dt3.Rows(0).Item("D_A").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("H_R_A").ToString) = False Then
                            HRA_Amt = Format(Val(dt3.Rows(0).Item("H_R_A").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("Conveyance_Esi_Pf").ToString) = False Then
                            Convey_PF_Amt = Format(Val(dt3.Rows(0).Item("Conveyance_Esi_Pf").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("Conveyance_Salary").ToString) = False Then
                            Convey_Salary_Amt = Format(Val(dt3.Rows(0).Item("Conveyance_Salary").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("Washing").ToString) = False Then
                            Washing_Amt = Format(Val(dt3.Rows(0).Item("Washing").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("Maintenance").ToString) = False Then
                            Maintain_Amt = Format(Val(dt3.Rows(0).Item("Maintenance").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("Entertainment").ToString) = False Then
                            Entertainment_Amt = Format(Val(dt3.Rows(0).Item("Entertainment").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("Provision").ToString) = False Then
                            Prrovision_Amt = Format(Val(dt3.Rows(0).Item("Provision").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("Other_Addition1").ToString) = False Then
                            Other_add1_Amt = Format(Val(dt3.Rows(0).Item("Other_Addition1").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("Other_Addition2").ToString) = False Then
                            Other_add2_Amt = Format(Val(dt3.Rows(0).Item("Other_Addition2").ToString), "########0.00")
                        End If

                        If IsDBNull(dt3.Rows(0).Item("Week_Off_Allowance").ToString) = False Then
                            vMas_WeekOff_Allow_Amt_PerDay = Format(Val(dt3.Rows(0).Item("Week_Off_Allowance").ToString), "########0.00")
                        End If

                        If IsDBNull(dt3.Rows(0).Item("Other_Deduction1").ToString) = False Then
                            Others_Ded_Ded_Entry = Val(Others_Ded_Ded_Entry) + Format(Val(dt3.Rows(0).Item("Other_Deduction1").ToString), "########0.00")
                        End If

                        If CL_STS <> 0 Then
                            If IsDBNull(dt3.Rows(0).Item("CL").ToString) = False Then
                                CL_Leaves_Current = Format(Val(dt3.Rows(0).Item("CL").ToString), "########0")
                            End If
                        Else
                            CL_Leaves_Current = 0
                        End If
                        If SL_STS <> 0 Then
                            If IsDBNull(dt3.Rows(0).Item("SL").ToString) = False Then
                                SL_Leaves_Current = Format(Val(dt3.Rows(0).Item("SL").ToString), "########0.00")
                            End If
                        Else
                            SL_Leaves_Current = 0
                        End If

                    End If
                    dt3.Clear()



                    '====== Calculating Total Days ==========

                    If Val(vEmp_IdNo) = 118 Then
                        Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
                    End If


                    vNoOf_Wrkd_Dys = 0

                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then

                        If Tot_Noof_WrkdShft_Frm_Att > 0 Then

                            vNoOf_Wrkd_Dys = Tot_Noof_WrkdShft_Frm_Att

                            If Val(dt1.Rows(i).Item("Leave_Salary_Less").ToString) = 1 Then

                                If Trim(UCase(dt1.Rows(i).Item("Attendance_Leave").ToString)) = "ATTENDANCE" Then

                                    vNoOf_Wrkd_Dys = Tot_Noof_WrkdShft_Frm_Att

                                Else

                                    vNoOf_Wrkd_Dys = vNo_Days_InMonth_for_MonthlyWages - Tot_LeaveDys_In_Mnth
                                    'vNoOf_Wrkd_Dys = Val(txt_TotalDays.Text) - Tot_FH_Dys - Tot_LeaveDys_In_Mnth - Tot_WeekOff_Days
                                    'vNoOf_Wrkd_Dys = vNo_Days_InMonth_for_MonthlyWages - Tot_LeaveDys_In_Mnth - Tot_WeekOff_Days
                                    dgv_Details.Columns(4).HeaderText = "Pay. Days"

                                End If

                            End If

                        End If

                        If Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 0 And Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 0 And Val(dt1.Rows(i).Item("Week_Off_Allowance").ToString) = 0 Then
                            vNoOf_Wrkd_Dys = vNoOf_Wrkd_Dys + vNoOf_Att_Dys_In_WkOff
                        End If

                        '---Attendance Days
                        If Val(dt1.Rows(i).Item("Festival_Holidays_OT_Salary").ToString) = 0 Then
                            vNoOf_Wrkd_Dys = vNoOf_Wrkd_Dys + vNoOf_Att_Dys_In_FH
                        End If



                    Else

                        vNoOf_Wrkd_Dys = Tot_Noof_WrkdShft_Frm_Att

                        If Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 1 Or Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 1 Or Val(dt1.Rows(i).Item("Week_Off_Allowance").ToString) = 1 Then
                            vNoOf_Wrkd_Dys = vNoOf_Wrkd_Dys - vNoOf_Att_Dys_In_WkOff
                        End If

                        '---Attendance Days
                        If Val(dt1.Rows(i).Item("Festival_Holidays_OT_Salary").ToString) = 1 Then
                            vNoOf_Wrkd_Dys = vNoOf_Wrkd_Dys - vNoOf_Att_Dys_In_FH
                        End If


                    End If

                    If vNoOf_Wrkd_Dys < 0 Then vNoOf_Wrkd_Dys = 0

                    '============ Festival Holiday ========== 
                    vNoOf_FH_Dys_For_Sal = 0
                    If Val(dt1.Rows(i).Item("Festival_Holidays").ToString) = 1 Then
                        vNoOf_FH_Dys_For_Sal = Tot_FH_Dys '- vNoOf_FH_Dys_On_WkOff
                    End If

                    '---------Total Days

                    Ttl_Days = vNoOf_Wrkd_Dys + vNoOf_FH_Dys_For_Sal

                    vSal_Shift = 0
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        vSal_Shift = Format(vMas_BasSal_PerShift_PerMonth / vNo_Days_InMonth_for_MonthlyWages, "########0.00")

                    Else
                        vSal_Shift = vMas_BasSal_PerShift_PerMonth

                    End If


                    If Common_Procedures.settings.CustomerCode = "1117" And Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then

                        'Tot_LeaveDys_In_Mnth
                        'vNo_Days_InMonth_for_MonthlyWages
                        'CL_Leaves_Current

                        If (Tot_LeaveDys_In_Mnth - vNoOf_Att_Dys_In_WkOff - vNoOf_Att_Dys_In_FH) > CL_Leaves_Current Then
                            Ttl_Days = Tot_Noof_WrkdShft_Frm_Att
                        Else
                            Ttl_Days = FormatNumber(Val(txt_TotalDays.Text), 2)
                        End If

                    End If

                    '============  STARTED DISPLAYING  ==========

                    n = dgv_Details.Rows.Add()

                    SNo = SNo + 1
                    If Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 1 Then
                        dgv_Details.Rows(n).Cells(15).ReadOnly = False
                    Else
                        dgv_Details.Rows(n).Cells(15).ReadOnly = True
                    End If

                    '---------
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Employee_Name").ToString

                    .Rows(n).Cells(2).Value = dt1.Rows(i).Item("Card_No").ToString

                    '--Basic Salary

                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then

                        If Trim(UCase(dt1.Rows(i).Item("Attendance_Leave").ToString)) = "ATTENDANCE" Then
                            vNet_LeaveDys_In_Mnth = Tot_LeaveDys_In_Mnth - vNoOf_FH_Dys_For_Sal - IIf(Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 0, vNoOf_Att_Dys_In_WkOff, 0) - IIf(Val(dt1.Rows(i).Item("Festival_Holidays_OT_Salary").ToString) = 0, vNoOf_Att_Dys_In_FH, 0)
                        Else
                            vNet_LeaveDys_In_Mnth = Tot_LeaveDys_In_Mnth - IIf(Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 0, vNoOf_Att_Dys_In_WkOff, 0) - IIf(Val(dt1.Rows(i).Item("Festival_Holidays_OT_Salary").ToString) = 0, vNoOf_Att_Dys_In_FH, 0)
                        End If

                        Bas_Sal = Format(vMas_BasSal_PerShift_PerMonth - (vSal_Shift * vNet_LeaveDys_In_Mnth), "###########0")

                        If Common_Procedures.settings.CustomerCode = "1117" Then
                            If txt_TotalDays.Text = Ttl_Days Then
                                Bas_Sal = Format(vMas_BasSal_PerShift_PerMonth, "###########0")
                            Else
                                Bas_Sal = Format(Ttl_Days * vSal_Shift, "###########0")
                            End If
                        End If

                    Else

                        vNet_LeaveDys_In_Mnth = Tot_LeaveDys_In_Mnth
                        Bas_Sal = Format(Ttl_Days * vSal_Shift, "###########0")

                    End If

                    .Rows(n).Cells(3).Value = Format(Val(Bas_Sal), "##########0.00")
                    If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

                    '---Total Days
                    .Rows(n).Cells(4).Value = Val(Ttl_Days)
                    If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then

                        If Common_Procedures.settings.CustomerCode = "1117" Then
                            'If Val(dt1.Rows(i).Item("Leave_Salary_Less").ToString) = 1 Then
                            Dim Act_Days_Worked_Mnth_Sal As Single = 0
                            Act_Days_Worked_Mnth_Sal = vNo_Days_InMonth_for_MonthlyWages
                            .Rows(n).Cells(6).Value = Act_Days_Worked_Mnth_Sal.ToString
                            'End If
                        Else

                            If Val(dt1.Rows(i).Item("Leave_Salary_Less").ToString) = 1 Then
                                Dim Act_Days_Worked_Mnth_Sal As Single = 0
                                Act_Days_Worked_Mnth_Sal = vNo_Days_InMonth_for_MonthlyWages - Tot_WeekOff_Days - Tot_LeaveDys_In_Mnth
                                .Rows(n).Cells(6).Value = Act_Days_Worked_Mnth_Sal.ToString
                            End If
                        End If
                    End If

                    '---Net Pay - Will be updated at last
                    .Rows(n).Cells(5).Value = ""

                    '----WORKING DAYS
                    '.Rows(n).Cells(6).Value = Val(vNoOf_Wrkd_Dys)
                    'If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                    '---From Weekoff Credit for leave
                    .Rows(n).Cells(7).Value = 0
                    If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                    '---From CL Credit for leave
                    .Rows(n).Cells(8).Value = 0
                    If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                    '---From SL Credit for leave
                    .Rows(n).Cells(9).Value = 0
                    If Val(.Rows(n).Cells(9).Value) = 0 Then .Rows(n).Cells(9).Value = ""

                    '---Festival Holidays
                    .Rows(n).Cells(10).Value = Val(vNoOf_FH_Dys_For_Sal)
                    If Val(.Rows(n).Cells(10).Value) = 0 Then .Rows(n).Cells(10).Value = ""

                    '---Total Days  = Salary From Date to salary To date
                    .Rows(n).Cells(11).Value = Val(Ttl_Days)
                    If Val(.Rows(n).Cells(11).Value) = 0 Then .Rows(n).Cells(11).Value = ""

                    '---No of Leaves
                    .Rows(n).Cells(12).Value = Val(Tot_LeaveDys_In_Mnth)
                    If Val(.Rows(n).Cells(12).Value) = 0 Then .Rows(n).Cells(12).Value = ""

                    '---Attendance on Weekoff  / Festival Holidays
                    .Rows(n).Cells(13).Value = Val(vNoOf_Att_Dys_In_WkOff) + Val(vNoOf_Att_Dys_In_FH)
                    If Val(.Rows(n).Cells(13).Value) = 0 Then .Rows(n).Cells(13).Value = ""

                    '---Opening Weekoff Credit
                    .Rows(n).Cells(14).Value = Val(WeekOff_ADD_Opening) - Val(WeekOff_LESS_Opening)
                    If Val(.Rows(n).Cells(14).Value) = 0 Then .Rows(n).Cells(14).Value = ""

                    '---Add Weekoff Credit
                    .Rows(n).Cells(15).Value = ""
                    If Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 1 Then
                        .Rows(n).Cells(15).Value = vNoOf_Att_Dys_In_WkOff
                    End If
                    If Val(.Rows(n).Cells(15).Value) = 0 Then .Rows(n).Cells(15).Value = ""

                    '---Less Weekoff Credit
                    .Rows(n).Cells(16).Value = 0
                    If Val(.Rows(n).Cells(16).Value) = 0 Then .Rows(n).Cells(16).Value = ""

                    '---Total Weekoff Credit =  (Opening Weekoff  + Add Weekoff - Less Weekoff)
                    .Rows(n).Cells(17).Value = Val(.Rows(n).Cells(14).Value) + Val(.Rows(n).Cells(15).Value) - Val(.Rows(n).Cells(16).Value)
                    If Val(.Rows(n).Cells(17).Value) = 0 Then .Rows(n).Cells(17).Value = ""


                    '---Opening CL Credit Days  (Opening + current Month) -Prev Month Used CL 
                    .Rows(n).Cells(18).Value = 0
                    If Val(dt1.Rows(i).Item("CL_Leave").ToString) = 1 Then
                        .Rows(n).Cells(18).Value = Val(CL_Leaves) + Val(CL_Leaves_Current) - Val(Less_CL_Leaves)
                    End If
                    If Val(.Rows(n).Cells(18).Value) = 0 Then .Rows(n).Cells(18).Value = ""

                    '---Less CL Credit Days
                    .Rows(n).Cells(19).Value = 0
                    If Val(.Rows(n).Cells(19).Value) = 0 Then .Rows(n).Cells(19).Value = ""

                    '---Total CL Credit Days =  (Opening CL Credit Days  - Less CL Credit Days)
                    .Rows(n).Cells(20).Value = 0
                    If Val(dt1.Rows(i).Item("CL_Leave").ToString) = 1 Then
                        .Rows(n).Cells(20).Value = Val(.Rows(n).Cells(18).Value) - Val(.Rows(n).Cells(19).Value)
                    End If
                    If Val(.Rows(n).Cells(20).Value) = 0 Then .Rows(n).Cells(20).Value = ""


                    '---Opening SL Credit Days   (Opening + current Month) -Prev Month Used SL  
                    .Rows(n).Cells(21).Value = ""
                    If Val(dt1.Rows(i).Item("SL_Leave").ToString) = 1 Then
                        .Rows(n).Cells(21).Value = Val(SL_Leaves) + Val(SL_Leaves_Current) - Val(Less_SL_Leaves)
                    End If
                    If Val(.Rows(n).Cells(21).Value) = 0 Then .Rows(n).Cells(21).Value = ""

                    '---Less SL Credit Days
                    .Rows(n).Cells(22).Value = 0
                    If Val(.Rows(n).Cells(22).Value) = 0 Then .Rows(n).Cells(22).Value = ""

                    '---Total SL Credit Days =  (Opening SL Credit Days  - Less SL Credit Days)
                    .Rows(n).Cells(23).Value = ""
                    If Val(dt1.Rows(i).Item("SL_Leave").ToString) = 1 Then
                        .Rows(n).Cells(23).Value = Val(.Rows(n).Cells(21).Value) - Val(.Rows(n).Cells(22).Value)
                    End If
                    If Val(.Rows(n).Cells(23).Value) = 0 Then .Rows(n).Cells(23).Value = ""

                    '---Salary/Days
                    .Rows(n).Cells(24).Value = Val(vSal_Shift)
                    If Val(.Rows(n).Cells(24).Value) = 0 Then .Rows(n).Cells(24).Value = ""

                    '---Basic Pay 

                    .Rows(n).Cells(25).Value = Format(Bas_Sal, "###########0.00")
                    If Val(.Rows(n).Cells(25).Value) = 0 Then .Rows(n).Cells(25).Value = ""

                    '========== OT ====== 

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
                        '----getting Shift Hours
                        If Val(dt1.Rows(i).Item("Shift1_Working_Hours").ToString) <> 0 Then
                            Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift1_Working_Hours").ToString), "##########0.00")
                        ElseIf Val(dt1.Rows(i).Item("Shift2_Working_Hours").ToString) <> 0 Then
                            Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift2_Working_Hours").ToString), "##########0.00")
                        ElseIf Val(dt1.Rows(i).Item("Shift3_Working_Hours").ToString) <> 0 Then
                            Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift3_Working_Hours").ToString), "##########0.00")
                        Else
                            Shft_Hours = 8
                        End If

                        '-----Hours To Minutes

                        H = Int(Shft_Hours)
                        M = (Shft_Hours - H) * 100
                        Shft_Mins = (H * 60) + M


                        Tot_OTMins = 0
                        '--------------If OT allowed in attandance Entry
                        If Val(dt1.Rows(i).Item("OT_Allowed").ToString) = 1 And OT_Mins > Val(dt1.Rows(i).Item("OT_Allowed_After_Minutes").ToString) Then
                            Tot_OTMins = Tot_OTMins + OT_Mins
                        End If
                        '--------------Festival Holiday Attendance in OT Salary  
                        If Val(dt1.Rows(i).Item("Festival_Holidays_OT_Salary").ToString) = 1 Then
                            Tot_OTMins = Tot_OTMins + (Shft_Mins * vNoOf_Att_Dys_In_FH)
                        End If
                        '-------------Weekoff Attendance in OT Salary
                        If Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 1 Then
                            Tot_OTMins = Tot_OTMins + (Shft_Mins * vNoOf_Att_Dys_In_WkOff)
                            weekof_mins = (Shft_Mins * vNoOf_Att_Dys_In_WkOff)
                        End If

                        '-----Minutes to Hour 

                        H = Tot_OTMins \ 60
                        M = Tot_OTMins - (H * 60)
                        OTHrs = H & "." & Format(M, "00")

                        '---OT Hours

                        .Rows(n).Cells(26).Value = Format(Val(OTHrs), "#######0.00")
                        If Val(.Rows(n).Cells(26).Value) = 0 Then .Rows(n).Cells(26).Value = ""

                        '----OT Salary Per Hour  (OT salary Per Shift / Shift Mins * 60)

                        .Rows(n).Cells(27).Value = Format((OT_Sal_Shft / Shft_Mins) * 60, "#######0.00")
                        If Val(.Rows(n).Cells(27).Value) = 0 Then .Rows(n).Cells(27).Value = ""

                        '---- OT Salary 
                        '--------------Festival Holiday Attendance in OT Salary

                        OT_Salary = Tot_OTMins * (OT_Sal_Shft / Shft_Mins)
                        .Rows(n).Cells(28).Value = Format(Val(OT_Salary), "#########0.00")
                        If Val(.Rows(n).Cells(28).Value) = 0 Then .Rows(n).Cells(28).Value = ""

                    Else
                        If Val(dt1.Rows(i).Item("Shift1_Working_Hours").ToString) <> 0 Then
                            Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift1_Working_Hours").ToString), "##########0.00")
                        ElseIf Val(dt1.Rows(i).Item("Shift2_Working_Hours").ToString) <> 0 Then
                            Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift2_Working_Hours").ToString), "##########0.00")
                        ElseIf Val(dt1.Rows(i).Item("Shift3_Working_Hours").ToString) <> 0 Then
                            Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift3_Working_Hours").ToString), "##########0.00")
                        Else
                            Shft_Hours = 8
                        End If

                        '-----Hours To Minutes
                        H = Int(Shft_Hours)
                        M = (Shft_Hours - H) * 100
                        Shft_Mins = (H * 60) + M

                        Tot_OTMins = 0
                        '--------------If OT allowed in attandance Entry
                        If Val(dt1.Rows(i).Item("OT_Allowed").ToString) = 1 And OT_Mins > Val(dt1.Rows(i).Item("OT_Allowed_After_Minutes").ToString) Then
                            Tot_OTMins = Tot_OTMins + OT_Mins
                        End If

                        If Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 1 Then
                            Tot_OTMins = Tot_OTMins + (Shft_Mins * vNoOf_Att_Dys_In_WkOff)
                        End If

                        '--------------Festival Holiday Attendance in OT Salary  
                        If Val(dt1.Rows(i).Item("Festival_Holidays_OT_Salary").ToString) = 1 Then
                            Tot_OTMins = Tot_OTMins + (Shft_Mins * vNoOf_Att_Dys_In_FH)
                        End If


                        '-----Minutes to Hour 
                        H = Tot_OTMins \ 60
                        M = Tot_OTMins - (H * 60)
                        OTHrs = H & "." & Format(M, "00")

                        '---OT Hours
                        .Rows(n).Cells(26).Value = Format(Val(OTHrs), "#######0.00")
                        If Val(.Rows(n).Cells(26).Value) = 0 Then .Rows(n).Cells(26).Value = ""

                        '----OT Salary Per Hour  (OT salary Per Shift / Shift Mins * 60)
                        .Rows(n).Cells(27).Value = Format((OT_Sal_Shft / Shft_Mins) * 60, "#######0.00")
                        If Val(.Rows(n).Cells(27).Value) = 0 Then .Rows(n).Cells(27).Value = ""

                        '---- OT Salary 
                        '--------------Festival Holiday Attendance in OT Salary
                        OT_Salary = Format(Tot_OTMins * (OT_Sal_Shft / Shft_Mins), "##########0")
                        .Rows(n).Cells(28).Value = Format(Val(OT_Salary), "#########0.00")
                        If Val(.Rows(n).Cells(28).Value) = 0 Then .Rows(n).Cells(28).Value = ""


                    End If

                    '=========================



                    '--------DA
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        DA_Shft = Format(DA_Amt / vNo_Days_InMonth_for_MonthlyWages, "########0.000000")
                    Else
                        DA_Shft = DA_Amt
                    End If
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(29).Value = Format(Math.Ceiling(DA_Amt - (DA_Shft * vNet_LeaveDys_In_Mnth)), "#########0.00")
                    Else
                        .Rows(n).Cells(29).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(DA_Shft)), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(29).Value) = 0 Then .Rows(n).Cells(29).Value = ""

                    '--------Earnings  (Basic pay + DA)
                    .Rows(n).Cells(30).Value = Format(Val(.Rows(n).Cells(25).Value) + Val(.Rows(n).Cells(29).Value), "#######0.00")
                    If Val(.Rows(n).Cells(30).Value) = 0 Then .Rows(n).Cells(30).Value = ""

                    '--------HRA
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
                        If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                            .Rows(n).Cells(31).Value = Format(Val(HRA_Amt), "#######0.00")
                        Else
                            .Rows(n).Cells(31).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(HRA_Amt)), "#######0.00")
                        End If
                    Else
                        .Rows(n).Cells(31).Value = Format(Val(HRA_Amt), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(31).Value) = 0 Then .Rows(n).Cells(31).Value = ""

                    '--------Conveyance salary ------Travel Allowance
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(32).Value = Format(Val(Convey_Salary_Amt), "#######0.00")
                    Else
                        .Rows(n).Cells(32).Value = Format(Math.Ceiling(Val(Tot_Noof_WrkdShft_Frm_Att) * Val(Convey_Salary_Amt)), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(32).Value) = 0 Then .Rows(n).Cells(32).Value = ""

                    '--------Washing
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(33).Value = Format(Val(Washing_Amt), "#######0.00")
                    Else
                        .Rows(n).Cells(33).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Washing_Amt)), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(33).Value) = 0 Then .Rows(n).Cells(33).Value = ""

                    '--------Entertainment
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(34).Value = Format(Val(Entertainment_Amt), "#######0.00")
                    Else
                        .Rows(n).Cells(34).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Entertainment_Amt)), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(34).Value) = 0 Then .Rows(n).Cells(34).Value = ""


                    '--------Maintanance
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(35).Value = Format(Val(Maintain_Amt), "#######0.00")
                    Else
                        .Rows(n).Cells(35).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Maintain_Amt)), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(35).Value) = 0 Then .Rows(n).Cells(35).Value = ""


                    '--------Provision
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(36).Value = Format(Val(Prrovision_Amt), "#######0.00")
                    Else
                        .Rows(n).Cells(36).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Prrovision_Amt)), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(36).Value) = 0 Then .Rows(n).Cells(36).Value = ""


                    '---Other Addition 1
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(37).Value = Format(Val(Other_add1_Amt), "#######0.00")
                    Else
                        .Rows(n).Cells(37).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Other_add1_Amt)), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(37).Value) = 0 Then .Rows(n).Cells(37).Value = ""

                    '---Other Addition 2
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(38).Value = Format(Val(Other_add2_Amt), "#######0.00")
                    Else
                        .Rows(n).Cells(38).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Other_add2_Amt)), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(38).Value) = 0 Then .Rows(n).Cells(38).Value = ""

                    '--------Other Addition

                    '-----if CL year arrear type is "salary" and Salary month is march then that salary amount go to other Addition
                    If Trim(dt1.Rows(i).Item("CL_Arrear_Type_Year").ToString) = "SALARY" And Val(thisMonth) = 3 Then
                        Others_Add_Ded_Entry = Others_Add_Ded_Entry + (vSal_Shift * (CL_Leaves + CL_Leaves_Current))
                    End If

                    '-----if SL year arrear type is "salary" and Salary month is march then that salary amount go to other Addition
                    If Trim(dt1.Rows(i).Item("SL_Arrear_Type_Year").ToString) = "SALARY" And Val(thisMonth) = 3 Then
                        Others_Add_Ded_Entry = Others_Add_Ded_Entry + (vSal_Shift * (SL_Leaves + SL_Leaves_Current))
                    End If

                    '-----if CL month arrear type is "salary"  then that salary amount go to other Addition
                    If Trim(dt1.Rows(i).Item("CL_Arrear_Type").ToString) = "SALARY" Then
                        Others_Add_Ded_Entry = Others_Add_Ded_Entry + (vSal_Shift * (CL_Leaves_Current))
                    End If

                    '-----if SL month arrear type is "salary"  then that salary amount go to other Addition
                    If Trim(dt1.Rows(i).Item("SL_Arrear_Type").ToString) = "SALARY" Then
                        Others_Add_Ded_Entry = Others_Add_Ded_Entry + (vSal_Shift * (SL_Leaves_Current))
                    End If
                    .Rows(n).Cells(39).Value = Format(Math.Ceiling(Val(Others_Add_Ded_Entry)), "#######0.00")
                    If Val(.Rows(n).Cells(39).Value) = 0 Then .Rows(n).Cells(39).Value = ""


                    '--------Incentives from attendance
                    .Rows(n).Cells(40).Value = Format(Val(Incentive), "#########0.00")
                    If Val(.Rows(n).Cells(40).Value) = 0 Then .Rows(n).Cells(40).Value = ""


                    '------Week off Allowance
                    .Rows(n).Cells(41).Value = ""
                    If Val(dt1.Rows(i).Item("Week_Off_Allowance").ToString) = 1 Then
                        .Rows(n).Cells(41).Value = Format(Math.Ceiling(vMas_WeekOff_Allow_Amt_PerDay * vNoOf_Att_Dys_In_WkOff), "#######0.00")
                    End If
                    If Val(.Rows(n).Cells(41).Value) = 0 Then .Rows(n).Cells(41).Value = ""

                    '--------Total Addition  =  (DA + HRA + COVEYANCE + WASHING + ENTETAINMENT + MAINTANACE +PROVISION + OTHER ADDITION + INCETIVES)

                    .Rows(n).Cells(42).Value = Format(Val(.Rows(n).Cells(29).Value) + Val(.Rows(n).Cells(31).Value) + Val(.Rows(n).Cells(32).Value) + Val(.Rows(n).Cells(33).Value) + Val(.Rows(n).Cells(34).Value) + Val(.Rows(n).Cells(35).Value) + Val(.Rows(n).Cells(36).Value) + Val(.Rows(n).Cells(37).Value) + Val(.Rows(n).Cells(38).Value) + Val(.Rows(n).Cells(39).Value) + Val(.Rows(n).Cells(40).Value) + Val(.Rows(n).Cells(41).Value), "#######0.00")
                    If Val(.Rows(n).Cells(42).Value) = 0 Then .Rows(n).Cells(42).Value = ""


                    '--------Mess

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
                        .Rows(n).Cells(43).Value = Format(Val(Mess_Ded_Entry), "#######0.00")
                    Else
                        If Mess_Ded_Entry <> 0 Then
                            .Rows(n).Cells(43).Value = Format(Val(Mess_Ded_Entry), "#######0.00")
                        Else
                            .Rows(n).Cells(43).Value = Format(Math.Ceiling(Val(mess_Ded) * Val(vNoOf_Wrk_Dys_Frm_MessAtt)), "#######0.00")
                        End If
                    End If
                    If Val(.Rows(n).Cells(43).Value) = 0 Then .Rows(n).Cells(43).Value = ""

                    '--------Medical
                    .Rows(n).Cells(44).Value = Format(Val(Medical_Ded_Entry), "#######0.00")
                    If Val(.Rows(n).Cells(44).Value) = 0 Then .Rows(n).Cells(44).Value = ""

                    '--------Store 
                    .Rows(n).Cells(45).Value = Format(Val(Store_Ded_Entry), "#######0.00")
                    If Val(.Rows(n).Cells(45).Value) = 0 Then .Rows(n).Cells(45).Value = ""

                    vPFSTS_Sal = 0
                    vESISTS_Sal = 0
                    vPFSTS_Audit = 0
                    vESISTS_Audit = 0

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
                        vPFSTS_Sal = Val(dt1.Rows(i).Item("Pf_Salary").ToString)
                        vESISTS_Sal = Val(dt1.Rows(i).Item("Esi_Salary").ToString)

                        vPFSTS_Audit = Val(dt1.Rows(i).Item("Pf_Status").ToString)
                        vESISTS_Audit = Val(dt1.Rows(i).Item("Esi_Status").ToString)

                    Else
                        vPFSTS_Sal = Val(dt1.Rows(i).Item("Pf_Status").ToString)
                        vESISTS_Sal = Val(dt1.Rows(i).Item("Esi_Status").ToString)

                        vPFSTS_Audit = Val(dt1.Rows(i).Item("Pf_Status").ToString)
                        vESISTS_Audit = Val(dt1.Rows(i).Item("Esi_Status").ToString)
                    End If

                    'esi


                    '============================= ESI - PF - SALARY ==================================
                    '--------ESI  1.75 %
                    .Rows(n).Cells(46).Value = ""
                    If vESISTS_Sal = 1 Then
                        vTotErngs_FOR_ESI = Val(.Rows(n).Cells(25).Value) + Val(.Rows(n).Cells(42).Value) - Val(.Rows(n).Cells(33).Value)
                        If Val(dt1.Rows(i).Item("Esi_For_OTSalary_Status").ToString) = 1 Then
                            vTotErngs_FOR_ESI = vTotErngs_FOR_ESI + Val(.Rows(n).Cells(28).Value)
                        End If
                        If Val(vSal_Shift) > Val(ESI_MAX_SHFT_WAGES) And Val(vTotErngs_FOR_ESI) < 25000 Then
                            '----If Shift Salary graterthan 100 then ESI allowed
                            .Rows(n).Cells(46).Value = Format(Math.Round(Val(vTotErngs_FOR_ESI) * 1.75 / 100), "#########0.00")
                        End If
                    End If
                    If Val(.Rows(n).Cells(46).Value) = 0 Then .Rows(n).Cells(46).Value = ""


                    .Rows(n).Cells(47).Value = ""
                    .Rows(n).Cells(48).Value = ""
                    .Rows(n).Cells(49).Value = ""
                    .Rows(n).Cells(77).Value = ""

                    If vPFSTS_Sal = 1 Then

                        '--------PF  ( 12 % ) - Management_Contribution_Perc
                        If Val(.Rows(n).Cells(30).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then
                            .Rows(n).Cells(47).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 12 / 100), "#########0.00")

                        Else
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)    '---SPINNING MILL
                                BASIC_SALARY_FOR_PF_CALCULATION = Val(.Rows(n).Cells(25).Value) * 70 / 100
                                .Rows(n).Cells(47).Value = Format(Math.Ceiling(Val(BASIC_SALARY_FOR_PF_CALCULATION) * 12 / 100), "#########0.00")

                            Else
                                .Rows(n).Cells(47).Value = Format(Math.Ceiling(Val(.Rows(n).Cells(30).Value) * 12 / 100), "#########0.00")

                            End If

                        End If

                        '--------EPF  (8.33 %)
                        '-------Basic Pay Graterthan 6500 than EPF value is 541 only allowed
                        '-------Basic Pay Graterthan 15000 than EPF value is 1249.5 only allowed

                        If Val(.Rows(n).Cells(30).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then
                            .Rows(n).Cells(48).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 8.33 / 100), "#########0.00")
                        Else
                            .Rows(n).Cells(48).Value = Format(Math.Round(Val(.Rows(n).Cells(30).Value) * 8.33 / 100), "#########0.00")
                        End If

                        '--------Pension  (3.67 %) 

                        .Rows(n).Cells(49).Value = Format(Val(.Rows(n).Cells(47).Value) - Val(.Rows(n).Cells(48).Value), "#########0.00")


                        If Val(dt1.Rows(i).Item("PF_Credit_Status").ToString) = 1 Then
                            .Rows(n).Cells(77).Value = Format(Val(.Rows(n).Cells(47).Value), "#########0.00")
                        End If

                    End If

                    If Val(.Rows(n).Cells(47).Value) = 0 Then .Rows(n).Cells(47).Value = ""
                    If Val(.Rows(n).Cells(48).Value) = 0 Then .Rows(n).Cells(48).Value = ""
                    If Val(.Rows(n).Cells(49).Value) = 0 Then .Rows(n).Cells(49).Value = ""
                    If Val(.Rows(n).Cells(77).Value) = 0 Then .Rows(n).Cells(77).Value = ""

                    '--------LATE MINS

                    'Ot_Int = Int(Late_Mins / 60)
                    'Ot_minVal = Ot_Int * 60
                    'Late_Hours = Ot_Int + ((Late_Mins - Ot_minVal) / 100)

                    '.Rows(n).Cells(50).Value = Format(Val(Late_Hours), "#######0.00")
                    'If Val(.Rows(n).Cells(50).Value) = 0 Then .Rows(n).Cells(50).Value = ""

                    ''--------LATE HOURS SALARY

                    '.Rows(n).Cells(51).Value = 0
                    'If Late_sts = True Then
                    '    .Rows(n).Cells(51).Value = Format(Math.Ceiling(Val(Late_Mins) * (Val(vSal_Shift) / Val(Shft_Mins))), "#######0.00")
                    'End If
                    'If Val(.Rows(n).Cells(51).Value) = 0 Then .Rows(n).Cells(51).Value = ""

                    '-----Minutes to Hour 

                    H = PerLeave_Mins \ 60
                    M = PerLeave_Mins - (H * 60)

                    PerLeaveHrs = H & "." & Format(M, "00")

                    '---Permission Leave Hours

                    .Rows(n).Cells(50).Value = Format(Val(PerLeaveHrs), "#######0.00")
                    If Val(.Rows(n).Cells(50).Value) = 0 Then .Rows(n).Cells(50).Value = ""

                    '----OT Salary Per Hour  (OT salary Per Shift / Shift Mins * 60)
                    '.Rows(n).Cells(27).Value = Format((OT_Sal_Shft / Shft_Mins) * 60, "#######0.00")
                    'If Val(.Rows(n).Cells(27).Value) = 0 Then .Rows(n).Cells(27).Value = ""

                    '---- OT Salary 
                    '--------------Festival Holiday Attendance in OT Salary

                    PerLeave_Ded = PerLeave_Mins * (OT_Sal_Shft / Shft_Mins)
                    .Rows(n).Cells(51).Value = Format(Val(PerLeave_Ded), "#########0.00")
                    If Val(.Rows(n).Cells(51).Value) = 0 Then .Rows(n).Cells(51).Value = ""

                    '--------Other Deduction
                    '--------------------------Leave Salary Less 

                    'If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                    '    If Val(dt1.Rows(i).Item("Leave_Salary_Less").ToString) = 1 Then
                    '        '.Rows(n).Cells(52).Value = Format(Val(Others_Ded_Ded_Entry) + vSal_Shift * (Tot_LeaveDys_In_Mnth - (Val(.Rows(n).Cells(19).Value) + Val(.Rows(n).Cells(22).Value))), "#######0.00")
                    '        .Rows(n).Cells(52).Value = Format(Val(Others_Ded_Ded_Entry) + vSal_Shift * ((Val(.Rows(n).Cells(19).Value) + Val(.Rows(n).Cells(22).Value))), "#######0.00")
                    '    Else
                    '        .Rows(n).Cells(52).Value = Format(Val(Others_Ded_Ded_Entry), "#######0.00")
                    '    End If
                    'Else
                    .Rows(n).Cells(52).Value = Format(Val(Others_Ded_Ded_Entry), "#######0.00")
                    'End If

                    If Val(.Rows(n).Cells(52).Value) = 0 Then .Rows(n).Cells(52).Value = ""


                    '--------Total Deduction  =  (MESS+ MEDICAL + STORE + ESI + PF  LATE HOUR SALARY +   OTHER DEDUCTION )

                    .Rows(n).Cells(53).Value = Format(Val(.Rows(n).Cells(43).Value) + Val(.Rows(n).Cells(44).Value) + Val(.Rows(n).Cells(45).Value) + Val(.Rows(n).Cells(46).Value) + Val(.Rows(n).Cells(47).Value) + Val(.Rows(n).Cells(51).Value) + Val(.Rows(n).Cells(52).Value), "#######0.00")
                    If Val(.Rows(n).Cells(53).Value) = 0 Then .Rows(n).Cells(53).Value = ""


                    '--------Attendance Incetive

                    Att_Incentive = 0
                    If Tot_LeaveDys_In_Mnth >= 0 Then
                        cmd.CommandText = "select a.Amount as Att_IncentiveAmount from PayRoll_Category_Details a Where a.Category_IdNo <> 0 and a.Category_IdNo = " & Str(Val(vEmpCatgry_IdNo)) & " and a.To_Attendance = " & Str(Val(Tot_LeaveDys_In_Mnth))
                        da2 = New SqlClient.SqlDataAdapter(cmd)
                        dt2 = New DataTable
                        da2.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            If IsDBNull(dt2.Rows(0).Item("Att_IncentiveAmount").ToString) = False Then
                                Att_Incentive = Val(dt2.Rows(0).Item("Att_IncentiveAmount").ToString)
                            End If
                        End If
                        dt2.Clear()
                    End If

                    .Rows(n).Cells(54).Value = Format(Val(Att_Incentive), "#######0.00")
                    If Val(.Rows(n).Cells(54).Value) = 0 Then .Rows(n).Cells(54).Value = ""

                    '--------Net Salary  = (BASIC SALARY + OT SALARY + TOTAL ADDITIONS - TOTAL DEDUTIONS +Attendance Incetive )
                    Net_Salary = Val(.Rows(n).Cells(3).Value) + Val(.Rows(n).Cells(28).Value) + Val(.Rows(n).Cells(42).Value) - Val(.Rows(n).Cells(53).Value) + Val(.Rows(n).Cells(54).Value)

                    .Rows(n).Cells(55).Value = Format(Net_Salary, "##########0.00")
                    If Val(.Rows(n).Cells(55).Value) = 0 Then .Rows(n).Cells(55).Value = ""

                    '-----Less Advance
                    mins_Adv = 0
                    If Val(Advance_Ded_Entry) <> 0 Then

                        mins_Adv = Advance_Ded_Entry
                        Less_Advance_Col_Edit_STS = False

                    Else


                        'cmd.CommandText = "Select sum(a.Minus_Advance) as Ent_MinusAdvance from PayRoll_Salary_Details a Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Salary_Code = '" & Trim(NewCode) & "'"
                        cmd.CommandText = "Select sum(a.Current_EMI) from Loan_EMI_Settings a Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo))
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        dt4 = New DataTable
                        Da.Fill(dt4)
                        If dt4.Rows.Count > 0 Then
                            If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                                mins_Adv = Format(Math.Abs(Val(dt4.Rows(0).Item(0).ToString)), "##########0.00")
                            End If
                        End If
                        dt4.Clear()

                    End If

                    '-----OPENING ADVANCE
                    .Rows(n).Cells(69).Value = Format(Val(Amt_OpBal), "#########0.00")
                    If Val(.Rows(n).Cells(69).Value) = 0 Then .Rows(n).Cells(69).Value = ""

                    'If Val(vEmp_IdNo) = 118 Then
                    '    Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
                    'End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL

                        '-----Total Advance + SALARY ADVANCE
                        .Rows(n).Cells(56).Value = Format(Val(Amt_OpBal) + Val(Sal_advance) + Val(Salary_Pending), "#########0.00")
                        If Val(.Rows(n).Cells(56).Value) = 0 Then .Rows(n).Cells(56).Value = ""

                        '-----Less Advance

                        .Rows(n).Cells(57).Value = Format(Val(mins_Adv), "########0.00")
                        If Val(.Rows(n).Cells(57).Value) = 0 Then .Rows(n).Cells(57).Value = ""

                        '------Balance Advance   ((Total Advance (OP + Previous-Salary_Payment_Pending + SALARY ADVANCE) - Less Advance)

                        .Rows(n).Cells(58).Value = Format(Val(.Rows(n).Cells(56).Value) - Val(.Rows(n).Cells(57).Value), "########0.00")
                        If Val(.Rows(n).Cells(58).Value) = 0 And Val(.Rows(n).Cells(56).Value) = 0 Then
                            .Rows(n).Cells(57).Value = ""
                        End If
                        If Val(.Rows(n).Cells(58).Value) = 0 Then .Rows(n).Cells(58).Value = ""

                        '-----Salary Advance

                        .Rows(n).Cells(59).Value = Format(Val(Sal_advance), "#########0.00")
                        If Val(.Rows(n).Cells(59).Value) = 0 Then .Rows(n).Cells(59).Value = ""

                        '----- Salary Pending

                        .Rows(n).Cells(60).Value = Format(Val(Salary_Pending), "#########0.00")
                        If Val(.Rows(n).Cells(60).Value) = 0 Then .Rows(n).Cells(60).Value = ""

                        '----- Net Pay     Net salary - Advance Deduction - Salary advance  + salary Pending
                        Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)), "##########0.00")

                    Else

                        '-----Total Advance
                        .Rows(n).Cells(56).Value = Format(Val(Amt_OpBal), "#########0.00")
                        If Val(.Rows(n).Cells(56).Value) = 0 Then .Rows(n).Cells(56).Value = ""


                        '-----Less Advance
                        .Rows(n).Cells(57).Value = Format(Val(mins_Adv), "########0.00")
                        If Val(.Rows(n).Cells(57).Value) = 0 Then .Rows(n).Cells(57).Value = ""


                        '------Balance Advance   (Total Advance  - Less Advance)
                        .Rows(n).Cells(58).Value = Format(Val(Amt_OpBal) - Val(mins_Adv), "########0.00")
                        If Val(.Rows(n).Cells(58).Value) = 0 And Val(.Rows(n).Cells(56).Value) = 0 Then
                            .Rows(n).Cells(57).Value = ""
                        End If
                        If Val(.Rows(n).Cells(58).Value) = 0 Then .Rows(n).Cells(58).Value = ""

                        '-----Salary Advance
                        .Rows(n).Cells(59).Value = Format(Val(Sal_advance), "#########0.00")
                        If Val(.Rows(n).Cells(59).Value) = 0 Then .Rows(n).Cells(59).Value = ""


                        '----- Salary Pending

                        .Rows(n).Cells(60).Value = Format(Val(Salary_Pending), "#########0.00")
                        If Val(.Rows(n).Cells(60).Value) = 0 Then .Rows(n).Cells(60).Value = ""

                        '----- Net Pay     Net salary - Advance Deduction - Salary advance  + salary Pending
                        Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value), "##########0.00")
                        'Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value) + Val(.Rows(n).Cells(60).Value), "##########0.00")

                    End If


                    '.Rows(n).Cells(61).Value = Format(Net_Pay, "#########0")
                    '.Rows(n).Cells(5).Value = Format(Net_Pay, "#########0")
                    'If Val(.Rows(n).Cells(61).Value) = 0 Then
                    '    If Val(.Rows(n).Cells(61).Value) = 0 Then
                    '        .Rows(n).Cells(61).Value = ""
                    '        .Rows(n).Cells(5).Value = ""
                    '    End If
                    'End If


                    '-----Day Of Bonus
                    .Rows(n).Cells(62).Value = Val(vNoOf_Wrkd_Dys)
                    If Val(.Rows(n).Cells(62).Value) = 0 Then .Rows(n).Cells(62).Value = ""

                    '-----Earnings For Bonus
                    .Rows(n).Cells(63).Value = 0
                    If Val(.Rows(n).Cells(63).Value) = 0 Then .Rows(n).Cells(63).Value = ""

                    '-----OT Mins
                    .Rows(n).Cells(64).Value = OT_Mins
                    If Val(.Rows(n).Cells(64).Value) = 0 Then .Rows(n).Cells(64).Value = ""


                    '-----Add CL Leave
                    .Rows(n).Cells(65).Value = Val(CL_Leaves_Current)
                    If Val(.Rows(n).Cells(65).Value) = 0 Then .Rows(n).Cells(65).Value = ""


                    '-----Add SL Leave
                    .Rows(n).Cells(66).Value = Val(SL_Leaves_Current)
                    If Val(.Rows(n).Cells(66).Value) = 0 Then .Rows(n).Cells(66).Value = ""


                    '-----LOP STATUS  (Loss of Pay)
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then

                        .Rows(n).Cells(67).Value = Val(dt1.Rows(i).Item("Leave_Salary_Less").ToString)

                    Else
                        .Rows(n).Cells(67).Value = 0

                    End If

                    '-----Actual Salary
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        .Rows(n).Cells(68).Value = vMas_BasSal_PerShift_PerMonth
                    Else
                        .Rows(n).Cells(68).Value = vMas_BasSal_PerShift_PerMonth
                    End If
                    If Val(.Rows(n).Cells(68).Value) = 0 Then .Rows(n).Cells(68).Value = ""

                    'If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                    '    .Rows(n).Cells(68).Value = vSal_Shift * (Val(txt_TotalDays.Text) - Tot_WeekOff_Days)
                    'Else
                    '    .Rows(n).Cells(68).Value = vSal_Shift * Val(txt_TotalDays.Text)
                    'End If
                    'If Val(.Rows(n).Cells(68).Value) = 0 Then .Rows(n).Cells(68).Value = ""

                    .Rows(n).Cells(70).Value = True
                    cmd.CommandText = "Select * from PayRoll_Salary_Details a Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Salary_Code = '" & Trim(NewCode) & "'"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0).Item("Signature_Status").ToString) = False Then
                            If Val(dt4.Rows(0).Item("Signature_Status").ToString) = 0 Then
                                .Rows(n).Cells(70).Value = False
                            End If
                        End If
                    End If
                    dt4.Clear()


                    '============================= ESI - PF - SALARY ==================================
                    '--------ESI  1.75 %
                    .Rows(n).Cells(71).Value = ""
                    .Rows(n).Cells(72).Value = ""
                    .Rows(n).Cells(73).Value = ""
                    .Rows(n).Cells(76).Value = ""

                    If vESISTS_Audit = 1 Then

                        vTotErngs_FOR_ESI = Val(.Rows(n).Cells(25).Value) + Val(.Rows(n).Cells(42).Value) - Val(.Rows(n).Cells(33).Value)
                        If Val(dt1.Rows(i).Item("Esi_For_OTSalary_Status").ToString) = 1 Then
                            vTotErngs_FOR_ESI = vTotErngs_FOR_ESI + Val(.Rows(n).Cells(28).Value)
                        End If

                        If Val(vSal_Shift) > Val(ESI_MAX_SHFT_WAGES) And Val(vTotErngs_FOR_ESI) < 25000 Then
                            '----If Shift Salary graterthan 100 then ESI allowed
                            .Rows(n).Cells(71).Value = Format(Val(vTotErngs_FOR_ESI) * 1.75 / 100, "#########0.00")
                        End If
                    End If

                    If vPFSTS_Audit = 1 Then

                        '--------PF  ( 12 % ) - Management_Contribution_Perc

                        If Val(.Rows(n).Cells(30).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then
                            .Rows(n).Cells(72).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 12 / 100), "#########0.00")

                        Else
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '----- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  '---- SPINNING MILL
                                BASIC_SALARY_FOR_PF_CALCULATION = Val(.Rows(n).Cells(25).Value) * 70 / 100
                                .Rows(n).Cells(72).Value = Format(Math.Ceiling(Val(BASIC_SALARY_FOR_PF_CALCULATION) * 12 / 100), "#########0.00")

                            Else
                                .Rows(n).Cells(72).Value = Format(Math.Ceiling(Val(.Rows(n).Cells(30).Value) * 12 / 100), "#########0.00")

                            End If

                        End If

                        '--------EPF  (8.33 %)

                        '-------Basic Pay Graterthan 6500 than EPF value is 541 only allowed
                        '-------Basic Pay Graterthan 15000 than EPF value is 1249.5 only allowed
                        If Val(.Rows(n).Cells(30).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then
                            .Rows(n).Cells(73).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 8.33 / 100), "#########0.00")
                        Else
                            .Rows(n).Cells(73).Value = Format(Math.Ceiling(Val(.Rows(n).Cells(30).Value) * 8.33 / 100), "#########0.00")
                        End If

                        .Rows(n).Cells(76).Value = Format(Val(.Rows(n).Cells(72).Value) - Val(.Rows(n).Cells(73).Value), "#############0.00")

                    End If
                    If Val(.Rows(n).Cells(71).Value) = 0 Then .Rows(n).Cells(71).Value = ""
                    If Val(.Rows(n).Cells(72).Value) = 0 Then .Rows(n).Cells(72).Value = ""
                    If Val(.Rows(n).Cells(73).Value) = 0 Then .Rows(n).Cells(73).Value = ""
                    If Val(.Rows(n).Cells(76).Value) = 0 Then .Rows(n).Cells(76).Value = ""

                    '=================================================================================================================================

                    '---SALARYESI + OT SALARY ESI
                    OT_SALARY_ESI = 0
                    .Rows(n).Cells(74).Value = ""
                    'If vESISTS_Sal = 1 Then
                    '    OT_SALARY_ESI = Format(Val(OT_Salary) * 1.75 / 100, "#########0.00")
                    '    .Rows(n).Cells(74).Value = Format(Val(OT_SALARY_ESI), "#########0.00")
                    '    If Val(.Rows(n).Cells(74).Value) = 0 Then .Rows(n).Cells(74).Value = ""
                    'End If


                    Salary_plus_ot_esi = 0
                    'If Val(dt1.Rows(i).Item("Esi_Salary").ToString) = 1 Then
                    Salary_plus_ot_esi = Format(Val(.Rows(n).Cells(46).Value) + Val(.Rows(n).Cells(74).Value), "#############0")
                    'ElseIf Val(dt1.Rows(i).Item("Esi_Status").ToString) = 1 Then
                    '    Salary_plus_ot_esi = Format(Val(.Rows(n).Cells(71).Value) + Val(.Rows(n).Cells(74).Value), "#############0")
                    'End If
                    .Rows(n).Cells(75).Value = Format(Val(Salary_plus_ot_esi), "#########0.00")
                    If Val(.Rows(n).Cells(75).Value) = 0 Then .Rows(n).Cells(75).Value = ""

                    '==============

                    Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value) + Val(.Rows(n).Cells(77).Value), "##########0.00")
                    'Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value) - Val(.Rows(n).Cells(74).Value), "##########0.00")

                    .Rows(n).Cells(61).Value = Format(Net_Pay, "#########0")
                    .Rows(n).Cells(5).Value = Format(Net_Pay, "#########0")
                    If Val(.Rows(n).Cells(61).Value) = 0 Then
                        If Val(.Rows(n).Cells(61).Value) = 0 Then
                            .Rows(n).Cells(61).Value = ""
                            .Rows(n).Cells(5).Value = ""
                        End If
                    End If

                Next i

                pnl_ProgressBar.Visible = False


            End If
            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            btn_Calculation_Salary.BackColor = Color.DeepPink

        End With

        Grid_Cell_DeSelect()

        ShowOrHideColumns()

        TotalNettPay()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
            dgv_Details.Columns(57).ReadOnly = False
            If Less_Advance_Col_Edit_STS = False Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
                dgv_Details.Columns(57).ReadOnly = True
            End If
        End If

        NoCalc_Status = False

    End Sub

    Private Sub Salary_Payment_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Salary_Payment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        dgv_Details.Columns(57).ReadOnly = False
        dgv_Details.Columns(77).Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
            dgv_Details.Columns(6).ReadOnly = False
            dgv_Details.Columns(7).ReadOnly = False
            dgv_Details.Columns(8).ReadOnly = False
            dgv_Details.Columns(9).ReadOnly = False
            dgv_Details.Columns(10).ReadOnly = False
            dgv_Details.Columns(26).ReadOnly = False
            dgv_Details.Columns(57).ReadOnly = True
            dgv_Details.Columns(77).Visible = True
        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()
        pnl_PrintEmployee_Details.Visible = False
        pnl_PrintEmployee_Details.Left = (Me.Width - pnl_PrintEmployee_Details.Width) \ 2
        pnl_PrintEmployee_Details.Top = (Me.Height - pnl_PrintEmployee_Details.Height) \ 2
        pnl_PrintEmployee_Details.BringToFront()

        Get_Fixed_Valus_From_Settings_Head()


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Employee.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Category.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FromDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Month.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FestivalDays.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FilterBillNo.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Category.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Month.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FestivalDays.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Employee.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FilterBillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FromDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FilterBillNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Month.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalDays.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_FestivalDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FromDate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Month.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FilterBillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_FestivalDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0



        Filter_Status = False
        FrmLdSTS = True
        new_record()

        ShowOrHideColumns()

    End Sub

    Private Sub Salary_Payment_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Salary_Payment_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Close_Form()

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            lbl_Company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)
            lbl_Company.Tag = Val(Common_Procedures.CompIdNo)
            Me.Text = lbl_Company.Text
            If Val(Common_Procedures.CompIdNo) = 0 Then

                Me.Close()

            Else

                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("D") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Salary_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Salary_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from PayRoll_Salary_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)

            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            txt_FilterBillNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where for_orderby > " & Str(Format(Val(OrdByNo), "########.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where for_orderby < " & Str(Format(Val(OrdByNo), "########0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("A") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Salary_Head", "Salary_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

            ShowOrHideColumns()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(RefCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("I") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(InvCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("A") And Not UCase(previlege).Contains("E") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        If Not New_Entry Then
            If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("E") Then
                MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
                Exit Sub
            End If
        End If

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Emp_ID As Integer = 0
        Dim Mth_IDNo As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0
        Dim vCatgry_IdNo As Integer = 0
        Dim Mon_Wek As String = "", VouNarr As String = ""
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
        Dim Sal_Amt As Single = 0
        Dim r As String

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Salary_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)
        If Val(SalPymtTyp_IdNo) = 0 Then
            MessageBox.Show("Invalid Payment Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PaymentType.Enabled And cbo_PaymentType.Visible Then cbo_PaymentType.Focus()
            Exit Sub
        End If

        vCatgry_IdNo = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)

        'If Val(vCatgry_IdNo) = 0 Then
        '    MessageBox.Show("Invalid Category", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Category.Enabled And cbo_Category.Visible Then cbo_Category.Focus()
        '    Exit Sub
        'End If

        Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

        Mth_IDNo = 0
        If Trim(UCase(Mon_Wek)) <> "WEEKLY" Then
            Mth_IDNo = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)
            If Val(Mth_IDNo) = 0 Then
                MessageBox.Show("Invalid Month", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Month.Enabled And cbo_Month.Visible Then cbo_Month.Focus()
                Exit Sub
            End If
        End If

        If IsDate(dtp_FromDate.Text) = False Then
            MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_FromDate.Enabled And dtp_FromDate.Visible Then dtp_FromDate.Focus()
            Exit Sub
        End If

        If IsDate(dtp_ToDate.Text) = False Then
            MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_ToDate.Enabled And dtp_ToDate.Visible Then dtp_ToDate.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con

        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@SalaryFromDate", dtp_FromDate.Value.Date)

        cmd.Parameters.AddWithValue("@SalaryToDate", dtp_ToDate.Value.Date)

        cmd.CommandText = "select * from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & " and Salary_Code <> '" & Trim(NewCode) & "' and ( (@SalaryFromDate Between From_Date and To_Date) or (@SalaryToDate Between From_Date and To_Date) )" & _
                          " and Salary_Payment_Type_IdNo = " & Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text) & " and Category_IdNo = " & Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)

        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            MessageBox.Show("Invalid From (or) To date " & Chr(13) & "Already Salary Entry prepared for this Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_ToDate.Enabled And dtp_ToDate.Visible Then dtp_ToDate.Focus()
            Exit Sub
        End If
        Dt1.Clear()

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(25).Value) <> 0 Or Val(.Rows(i).Cells(36).Value) <> 0 Or Val(.Rows(i).Cells(38).Value) <> 0 Or Val(.Rows(i).Cells(52).Value) <> 0 Then

                    Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Emp_ID = 0 Then
                        MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Salary_Head", "Salary_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalaryDate", dtp_Date.Value.Date)

            cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)

            cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

            If dtp_Advance_UpToDate.Visible = True Then
                cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_Advance_UpToDate.Value.Date)
            Else
                cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_ToDate.Value.Date)
            End If

            If New_Entry = True Then
                cmd.CommandText = "Insert into PayRoll_Salary_Head (     Salary_Code        ,               Company_IdNo       ,           Salary_No           ,                               for_OrderBy                              ,   Salary_Date ,       Salary_Payment_Type_IdNo   ,             Category_IdNo     ,         Month_IdNo   ,  From_Date,  To_Date,  Advance_UptoDate,                 Total_Days          ,                  Festival_Days           ,     Salary_Year) " & _
                                    "          Values              ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @SalaryDate  , " & Str(Val(SalPymtTyp_IdNo)) & ", " & Str(Val(vCatgry_IdNo)) & ", " & Val(Mth_IDNo) & ", @FromDate , @ToDate , @AdvanceUpToDate , " & Str(Val(txt_TotalDays.Text)) & ",  " & Str(Val(txt_FestivalDays.Text)) & " ,    '" & cbo_Year.Text & "') "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update PayRoll_Salary_Head set Salary_Date = @SalaryDate, Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ",  Category_IdNo = " & Str(Val(vCatgry_IdNo)) & ", Month_IdNo = " & Val(Mth_IDNo) & ", From_Date = @FromDate, To_Date =  @ToDate, Advance_UptoDate =  @AdvanceUpToDate, Total_Days = " & Str(Val(txt_TotalDays.Text)) & ", Festival_Days = " & Str(Val(txt_FestivalDays.Text)) & ", Salary_Year = '" & cbo_Year.Text & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from PayRoll_Salary_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition3) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            VouNarr = ""


            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(Emp_ID) <> 0 Then

                        Sno = Sno + 1

                        r = 0

                        If .Rows(i).Cells(70).Value = True Then r = 1

                        If Trim(r) = 0 Then r = 0
                        Dim OT_HR As Integer
                        Dim OT_MIN As Integer
                        Dim OT_HALF_HR As Double
                        'OT HALF HOUR
                        OT_HR = Val(.Rows(i).Cells(26).Value)
                        OT_MIN = OT_HR * 60
                        OT_HALF_HR = (OT_MIN + ((Val(.Rows(i).Cells(26).Value) - OT_HR) * 100)) / 2

                        OT_HR = OT_HALF_HR / 60
                        OT_MIN = OT_HALF_HR - (OT_HR * 60)
                        OT_HALF_HR = OT_HR + IIf(OT_MIN > 0, (OT_MIN * 0.01), 0)


                        cmd.CommandText = "Insert into PayRoll_Salary_Details (        Salary_Code     ,               Company_IdNo       ,            Salary_No          ,                               for_OrderBy                              ,   Salary_Date,            Sl_No     ,        Employee_IdNo    ,               Card_No                    ,           Basic_Salary                  ,              Total_Days                  ,                      Net_Pay             ,                 No_Of_Attendance_Days    ,                       From_W_off_CR       ,              From_Cl_For_Leave           ,              From_SL_For_Leave           ,      Festival_Holidays                   ,                      Total_Leave_Days       ,                       No_Of_Leave         ,        Attendance_On_W_Off_FH             ,                      Op_W_Off_CR          ,                      Add_W_Off_CR          ,        Less_W_Off_CR                      ,                      Total_W_Off_CR       ,        OP_CL_CR_Days                       ,               Less_CL_CR_Days             ,                      Total_Cl_CR_Days     ,        OP_SL_CR_Days                       ,         Less_SL_CR_Days                     ,          TOtal_SL_CR_Days                  ,       Salary_Days                          ,        Basic_Pay                          ,           OT_Hours                        ,OT_HOURS_HALF           ,           OT_Pay_Hours                   ,                OT_Salary                  ,                          D_A             ,              Earning                      ,         H_R_A                            ,                    Conveyance             ,        Washing                            ,      Entertainment                        ,                     Maintenance           , Provision                                 ,               Other_Addition1            ,  Other_Addition2                          ,    Other_Addition                         , Incentive_Amount                          ,   Week_Off_Allowance                     ,             Total_Addition               ,                     Mess                   ,                           Medical         ,                  Store                    ,                   ESI                     ,                P_F                        ,                      E_P_F                ,                  Pension_Scheme           ,  Late_Mins                               ,     Late_Hours_Salary                    ,                     Other_Deduction      ,                Total_Deduction           ,                   Attendance_Incentive   ,                     Net_Salary                 ,              Total_Advance               ,              Minus_Advance               ,         Balance_Advance                    ,        Salary_Advance                     ,         Salary_Pending                    ,             Net_pay_Amount                    ,    Day_For_Bonus                         ,                     Earning_For_Bonus     ,                       OT_Minutes         , Add_CL_Leaves                              ,                     Add_SL_Leaves          ,                      Leave_Salary_Less     ,                      Actual_Salary        ,                     Opening_Advance       ,   Signature_Status ,                      ESI_AUDIT             ,                      PF_AUDIT              ,                      E_P_F_AUDIT          ,                      OT_ESI               ,                      SALARY_OT_ESI        ,                       E_P_S_AUDIT          ,                      PF_Credit_Amount      ) " & _
                                          "            Values                 ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @SalaryDate , " & Str(Val(Sno)) & ", " & Str(Val(Emp_ID)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & "," & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ",  " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ",  " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & ", " & Str(Val(.Rows(i).Cells(15).Value)) & " , " & Str(Val(.Rows(i).Cells(16).Value)) & ", " & Str(Val(.Rows(i).Cells(17).Value)) & ",  " & Str(Val(.Rows(i).Cells(18).Value)) & ", " & Str(Val(.Rows(i).Cells(19).Value)) & ", " & Str(Val(.Rows(i).Cells(20).Value)) & ",  " & Str(Val(.Rows(i).Cells(21).Value)) & ",   " & Str(Val(.Rows(i).Cells(22).Value)) & ",  " & Str(Val(.Rows(i).Cells(23).Value)) & ",  " & Str(Val(.Rows(i).Cells(24).Value)) & ", " & Str(Val(.Rows(i).Cells(25).Value)) & ", " & Str(Val(.Rows(i).Cells(26).Value)) & "," & Val(OT_HALF_HR) & " , " & Str(Val(.Rows(i).Cells(27).Value)) & "," & Str(Val(.Rows(i).Cells(28).Value)) & "," & Str(Val(.Rows(i).Cells(29).Value)) & ", " & Str(Val(.Rows(i).Cells(30).Value)) & "," & Str(Val(.Rows(i).Cells(31).Value)) & ", " & Str(Val(.Rows(i).Cells(32).Value)) & ", " & Str(Val(.Rows(i).Cells(33).Value)) & ", " & Str(Val(.Rows(i).Cells(34).Value)) & ", " & Str(Val(.Rows(i).Cells(35).Value)) & "," & Str(Val(.Rows(i).Cells(36).Value)) & " ," & Str(Val(.Rows(i).Cells(37).Value)) & "," & Str(Val(.Rows(i).Cells(38).Value)) & " ," & Str(Val(.Rows(i).Cells(39).Value)) & " , " & Str(Val(.Rows(i).Cells(40).Value)) & "," & Str(Val(.Rows(i).Cells(41).Value)) & "," & Str(Val(.Rows(i).Cells(42).Value)) & "," & Str(Val(.Rows(i).Cells(43).Value)) & "  , " & Str(Val(.Rows(i).Cells(44).Value)) & ", " & Str(Val(.Rows(i).Cells(45).Value)) & ", " & Str(Val(.Rows(i).Cells(46).Value)) & ", " & Str(Val(.Rows(i).Cells(47).Value)) & "," & Str(Val(.Rows(i).Cells(48).Value)) & ", " & Str(Val(.Rows(i).Cells(49).Value)) & " ," & Str(Val(.Rows(i).Cells(50).Value)) & "," & Str(Val(.Rows(i).Cells(51).Value)) & "," & Str(Val(.Rows(i).Cells(52).Value)) & "," & Str(Val(.Rows(i).Cells(53).Value)) & ", " & Str(Val(.Rows(i).Cells(54).Value)) & ",     " & Str(Val(.Rows(i).Cells(55).Value)) & "," & Str(Val(.Rows(i).Cells(56).Value)) & ", " & Str(Val(.Rows(i).Cells(57).Value)) & "," & Str(Val(.Rows(i).Cells(58).Value)) & " , " & Str(Val(.Rows(i).Cells(59).Value)) & ", " & Str(Val(.Rows(i).Cells(60).Value)) & ",    " & Str(Val(.Rows(i).Cells(61).Value)) & " ," & Str(Val(.Rows(i).Cells(62).Value)) & "," & Str(Val(.Rows(i).Cells(63).Value)) & " ," & Str(Val(.Rows(i).Cells(64).Value)) & "," & Str(Val(.Rows(i).Cells(65).Value)) & " ," & Str(Val(.Rows(i).Cells(66).Value)) & "  , " & Str(Val(.Rows(i).Cells(67).Value)) & " , " & Str(Val(.Rows(i).Cells(68).Value)) & " , " & Str(Val(.Rows(i).Cells(69).Value)) & ", '" & Trim(r) & "'  , " & Str(Val(.Rows(i).Cells(71).Value)) & " , " & Str(Val(.Rows(i).Cells(72).Value)) & " , " & Str(Val(.Rows(i).Cells(73).Value)) & ", " & Str(Val(.Rows(i).Cells(74).Value)) & ", " & Str(Val(.Rows(i).Cells(75).Value)) & ", " & Str(Val(.Rows(i).Cells(76).Value)) & " , " & Str(Val(.Rows(i).Cells(77).Value)) & " ) "
                        cmd.ExecuteNonQuery()



                        'If Val(Emp_ID) = 136 Then
                        '    Debug.Print(Emp_ID)
                        'End If

                        Sal_Amt = Val(.Rows(i).Cells(61).Value)

                        If Val(Sal_Amt) <> 0 Then

                            'If Val(Sal_Amt) < 0 Then
                            'vLed_IdNos = Common_Procedures.CommonLedger.Salary_Ac & "|" & Emp_ID

                            'Else
                            vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.Salary_Ac
                            'End If

                            'vVou_Amts = Format(Math.Abs(Val(Sal_Amt)) + Math.Abs(Val(Val(.Rows(i).Cells(57).Value))) + Math.Abs(Val(Val(.Rows(i).Cells(59).Value)) - Math.Abs(Val(.Rows(i).Cells(43).Value))), "#########0.00") & "|" & Format(-1 * (Math.Abs(Val(Sal_Amt)) + Math.Abs(Val(Val(.Rows(i).Cells(57).Value))) + Math.Abs(Val(Val(.Rows(i).Cells(59).Value))) - Math.Abs(Val(.Rows(i).Cells(43).Value))), "#########0.00")
                            'If Sal_Amt + Val(.Rows(i).Cells(43).Value) >= 0 Then

                            'vVou_Amts = Format(Math.Abs(Val(Sal_Amt)) + Math.Abs(Val(.Rows(i).Cells(43).Value)), "#########0.00") & "|" & Format(-1 * (Math.Abs(Val(Sal_Amt)) + Math.Abs(Val(.Rows(i).Cells(43).Value))), "#########0.00")
                            vVou_Amts = Format(Val(Sal_Amt) + Val(.Rows(i).Cells(43).Value), "#########0.00") & "|" & Format(-1 * (Val(Sal_Amt) + Val(.Rows(i).Cells(43).Value)), "#########0.00")

                            'Else
                            '    vVou_Amts = Format(-1 * (Val(Sal_Amt)) + Math.Abs(Val(.Rows(i).Cells(43).Value)), "#########0.00") & "|" & Format(Val(Sal_Amt) + Math.Abs(Val(.Rows(i).Cells(43).Value)), "#########0.00")
                            'End If

                            If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                                VouNarr = "Salary for Week " & dtp_FromDate.Text & " to " & dtp_ToDate.Text

                            Else
                                VouNarr = "Salary for Month " & cbo_Month.Text

                            End If

                            If Common_Procedures.Voucher_Updation(con, "Emp.Sal", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                                Throw New ApplicationException(ErrMsg)
                                Exit Sub
                            End If

                        End If

                        If Val(.Rows(i).Cells(57).Value) = 0 Then
                            NoCalc_Status = True
                            .Rows(i).Cells(57).Value = 0
                            NoCalc_Status = False
                        End If

                        If Val(.Rows(i).Cells(57).Value) <> 0 Then

                            If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                                VouNarr = "Loan Deduction for Week " & dtp_FromDate.Text & " to " & dtp_ToDate.Text

                            Else
                                VouNarr = "Loan Deduction for Month " & cbo_Month.Text

                            End If

                            If (Val(.Rows(i).Cells(57).Value)) < 0 Then
                                vLed_IdNos = Common_Procedures.CommonLedger.ADVANCE_DEDUCTION_AC & "|" & Emp_ID

                            Else
                                vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.ADVANCE_DEDUCTION_AC
                            End If

                            vVou_Amts = Math.Abs(Val(.Rows(i).Cells(57).Value)) & "|" & -1 * Math.Abs(Val(.Rows(i).Cells(57).Value))

                            If Common_Procedures.Voucher_Updation(con, "Loan Deduction", Val(lbl_Company.Tag), Trim(Pk_Condition3) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                                Throw New ApplicationException(ErrMsg)
                                Exit Sub
                            End If

                        End If

                        '----------------

                        If Val(.Rows(i).Cells(59).Value) = 0 Then
                            NoCalc_Status = True
                            .Rows(i).Cells(59).Value = 0
                            NoCalc_Status = False
                        End If

                        If Val(.Rows(i).Cells(59).Value) <> 0 Then

                            If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                                VouNarr = "Sal. Adv. Ded. for Week " & dtp_FromDate.Text & " to " & dtp_ToDate.Text

                            Else
                                VouNarr = "Sal. Adv. Ded. for Month " & cbo_Month.Text

                            End If

                            If (Val(.Rows(i).Cells(59).Value)) < 0 Then
                                vLed_IdNos = Common_Procedures.CommonLedger.ADVANCE_DEDUCTION_AC & "|" & Emp_ID

                            Else
                                vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.ADVANCE_DEDUCTION_AC

                            End If

                            vVou_Amts = 1 * Math.Abs(Val(.Rows(i).Cells(59).Value)) & "|" & -1 * Math.Abs(Val(.Rows(i).Cells(59).Value))

                            If Common_Procedures.Voucher_Updation(con, "Sal. Adv. Deduction", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                                Throw New ApplicationException(ErrMsg)
                                Exit Sub
                            End If

                        End If

                    End If

                    '----------------

                    'If Val(.Rows(i).Cells(43).Value) = 0 Then
                    '    NoCalc_Status = True
                    '    .Rows(i).Cells(43).Value = 0
                    '    NoCalc_Status = False
                    'End If

                    'If Val(.Rows(i).Cells(43).Value) <> 0 Then

                    '    If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                    '        VouNarr = "Mess. Ded. for Week " & dtp_FromDate.Text & " to " & dtp_ToDate.Text

                    '    Else
                    '        VouNarr = "Mess. Ded. for Month " & cbo_Month.Text

                    '    End If

                    '    If (Val(.Rows(i).Cells(43).Value)) < 0 Then
                    '        vLed_IdNos = Common_Procedures.CommonLedger.ADVANCE_DEDUCTION_AC & "|" & Emp_ID

                    '    Else
                    '        vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.ADVANCE_DEDUCTION_AC
                    '    End If

                    '    vVou_Amts = 1 * Math.Abs(Val(.Rows(i).Cells(43).Value)) & "|" & -1 * Math.Abs(Val(.Rows(i).Cells(43).Value))

                    '    If Common_Procedures.Voucher_Updation(con, "Mess. Deduction", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    '        Throw New ApplicationException(ErrMsg)
                    '        Exit Sub
                    '    End If

                    'End If



                    '------------------





                Next

            End With

            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_PaymentType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")
        cbo_PaymentType.Tag = cbo_PaymentType.Text
    End Sub

    Private Sub cbo_PaymentType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentType, dtp_Date, cbo_Category, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")
    End Sub

    Private Sub cbo_PaymentType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentType.KeyPress
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentType, Nothing, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                If Trim(cbo_PaymentType.Text) <> "" Then

                    SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

                    Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

                    If Trim(UCase(cbo_PaymentType.Text)) <> Trim(UCase(cbo_PaymentType.Tag)) Then

                        If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                            dtp_FromDate.Enabled = True
                            dtp_ToDate.Enabled = True

                            cbo_Month.Text = ""

                            dtp_FromDate.Focus()

                            cbo_Category.Enabled = False

                        Else
                            cbo_Month.Enabled = True
                            dtp_FromDate.Enabled = False
                            dtp_ToDate.Enabled = False

                            dtp_FromDate.Text = ""
                            dtp_ToDate.Text = ""

                            cbo_Category.Focus()

                        End If

                    Else

                        If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                            dtp_FromDate.Enabled = True
                            dtp_ToDate.Enabled = True

                            dtp_FromDate.Focus()

                            cbo_Category.Enabled = False

                        Else
                            cbo_Month.Enabled = True
                            dtp_FromDate.Enabled = False
                            dtp_ToDate.Enabled = False

                            cbo_Category.Focus()

                        End If

                    End If

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE PAYMENTTYPE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Month_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Month.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
        cbo_Month.Tag = cbo_Month.Text
    End Sub

    Private Sub cbo_Month_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Month.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Month, cbo_Category, dtp_FromDate, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_Month_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Month.KeyPress
        Dim dttm As Date
        Dim Mth_ID As Integer = 0

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Month, Nothing, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")

            If Asc(e.KeyChar) = 13 And Trim(cbo_Month.Text) <> "" And Trim(cbo_Year.Text) <> "" Then

                If Trim(UCase(cbo_Month.Tag)) <> Trim(UCase(cbo_Month.Text)) Then

                    Mth_ID = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

                    dttm = New DateTime(cbo_Year.Text, Mth_ID, 1)

                    dtp_FromDate.Text = dttm

                    dttm = DateAdd("M", 1, dttm)
                    dttm = DateAdd("d", -1, dttm)

                    dtp_ToDate.Text = dttm

                    get_PayRoll_Salary_Details()


                End If

                If dtp_Advance_UpToDate.Visible And dtp_Advance_UpToDate.Enabled Then
                    dtp_Advance_UpToDate.Focus()

                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "ERROR WHILE MONTH KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Salary_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Salary_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Salary_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If


            If Trim(txt_FilterBillNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bill_No = '" & Trim(txt_FilterBillNo.Text) & "' "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*,  c.Ledger_Name as PartyName from PayRoll_Salary_Head a  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Salary_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from PayRoll_Salary_Head a INNER JOIN PayRoll_Salary_Details b ON a.Salary_Code = b.Salary_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Salary_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Salary_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Salary_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String = ""

        Try
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE OPEN FILTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub


    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim Emp_ID As Integer = 0
        Dim vPFSTS_Sal As Integer = 0
        Dim vESISTS_Sal As Integer = 0
        Dim vPFSTS_Audit As Integer = 0
        Dim vESISTS_Audit As Integer = 0
        Dim vESI_FOR_OT_STS As Integer = 0
        Dim vTotErngs_FOR_ESI As String = ""

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            If Common_Procedures.settings.CustomerCode = "1117" Then

                With dgv_Details

                    If e.ColumnIndex = 43 Or e.ColumnIndex = 57 Or e.ColumnIndex = 59 Then

                        .CurrentRow.Cells(55).Value = Format(Val(.CurrentRow.Cells(3).Value) + Val(.CurrentRow.Cells(28).Value) + Val(.CurrentRow.Cells(42).Value) - Val(.CurrentRow.Cells(43).Value) + Val(.CurrentRow.Cells(54).Value), "#########0.00")
                        .CurrentRow.Cells(58).Value = Format((Val(.CurrentRow.Cells(56).Value) - Val(.CurrentRow.Cells(57).Value)), "##########0.00")
                        .CurrentRow.Cells(61).Value = Format((((Val(.CurrentRow.Cells(55).Value) - Val(.CurrentRow.Cells(57).Value))) - Val(.CurrentRow.Cells(59).Value)), "##########0.00")


                        .CurrentRow.Cells(5).Value = Format(Val(.CurrentRow.Cells(61).Value), "###########0.00")

                    End If


                End With

                TotalNettPay()
                Exit Sub

            End If

            '-------------

            With dgv_Details

                If .Visible Then

                    If .Rows.Count > 0 Then

                        Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .CurrentRow.Cells(1).Value)
                        If Val(Emp_ID) = 0 Then Exit Sub

                        If e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Then
                            .CurrentRow.Cells(11).Value = Val(.CurrentRow.Cells(6).Value) + Val(.CurrentRow.Cells(7).Value) + Val(.CurrentRow.Cells(8).Value) + Val(.CurrentRow.Cells(9).Value) + Val(.CurrentRow.Cells(10).Value)
                            .CurrentRow.Cells(4).Value = Val(.CurrentRow.Cells(11).Value)
                        End If

                        If e.ColumnIndex = 7 Then
                            .CurrentRow.Cells(16).Value = Val(.CurrentRow.Cells(7).Value)
                        End If

                        If e.ColumnIndex = 8 Or e.ColumnIndex = 9 Then
                            '---Other Deduction =  Other Deduction + (salary per day * (No of Leaves - (CL for Leave + SL for Leave)))
                            If Val(.CurrentRow.Cells(67).Value) = 1 Then
                                .CurrentRow.Cells(52).Value = Val(.CurrentRow.Cells(68).Value) - ((Val(.CurrentRow.Cells(24).Value) * (Val(.CurrentRow.Cells(12).Value) - (Val(.CurrentRow.Cells(8).Value) + Val(.CurrentRow.Cells(9).Value)))))
                            End If
                            .CurrentRow.Cells(22).Value = Val(.CurrentRow.Cells(9).Value)
                            .CurrentRow.Cells(19).Value = Val(.CurrentRow.Cells(8).Value)
                        End If

                        If e.ColumnIndex = 11 Then
                            .CurrentRow.Cells(25).Value = Format(Val(.CurrentRow.Cells(11).Value) * Val(.CurrentRow.Cells(24).Value), "###########0")
                            .CurrentRow.Cells(25).Value = Format(Val(.CurrentRow.Cells(25).Value), "###########0.00")
                            .CurrentRow.Cells(3).Value = .CurrentRow.Cells(25).Value
                        End If

                        If e.ColumnIndex = 14 Or e.ColumnIndex = 15 Or e.ColumnIndex = 16 Then
                            .CurrentRow.Cells(17).Value = Val(.CurrentRow.Cells(14).Value) + Val(.CurrentRow.Cells(15).Value) - Val(.CurrentRow.Cells(16).Value)
                        End If

                        If e.ColumnIndex = 18 Or e.ColumnIndex = 19 Then
                            .CurrentRow.Cells(20).Value = Val(.CurrentRow.Cells(18).Value) - Val(.CurrentRow.Cells(19).Value)
                        End If

                        If e.ColumnIndex = 21 Or e.ColumnIndex = 22 Then
                            .CurrentRow.Cells(23).Value = Val(.CurrentRow.Cells(21).Value) - Val(.CurrentRow.Cells(22).Value)
                        End If

                        If e.ColumnIndex = 25 Then
                            .CurrentRow.Cells(30).Value = Format(Val(.CurrentRow.Cells(25).Value) + Val(.CurrentRow.Cells(29).Value), "###########0.00")
                        End If

                        If e.ColumnIndex = 26 Then
                            .CurrentRow.Cells(28).Value = Format(Val(.CurrentRow.Cells(26).Value) * Val(.CurrentRow.Cells(27).Value), "###########0")
                        End If

                        'esi
                        vPFSTS_Sal = 0
                        vESISTS_Sal = 0
                        vPFSTS_Audit = 0
                        vESISTS_Audit = 0
                        vESI_FOR_OT_STS = 0

                        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo <> 0 and a.Category_IdNo = b.Category_IdNo Where a.Employee_idno = " & Str(Val(Emp_ID)), con)
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
                                vPFSTS_Sal = Val(Dt1.Rows(0).Item("Pf_Salary").ToString)
                                vESISTS_Sal = Val(Dt1.Rows(0).Item("Esi_Salary").ToString)

                                vPFSTS_Audit = Val(Dt1.Rows(0).Item("Pf_Status").ToString)
                                vESISTS_Audit = Val(Dt1.Rows(0).Item("Esi_Status").ToString)

                            Else

                                vPFSTS_Sal = Val(Dt1.Rows(0).Item("Pf_Status").ToString)
                                vESISTS_Sal = Val(Dt1.Rows(0).Item("Esi_Status").ToString)

                                vPFSTS_Audit = Val(Dt1.Rows(0).Item("Pf_Status").ToString)
                                vESISTS_Audit = Val(Dt1.Rows(0).Item("Esi_Status").ToString)

                            End If

                            vESI_FOR_OT_STS = Val(Dt1.Rows(0).Item("Esi_For_OTSalary_Status").ToString)

                        End If
                        Dt1.Clear()


                        '============================= ESI - PF - SALARY ==================================
                        '--------ESI  1.75 %

                        If e.ColumnIndex = 25 Or e.ColumnIndex = 28 Or e.ColumnIndex = 42 Then

                            vTotErngs_FOR_ESI = Format(Val(.CurrentRow.Cells(25).Value) + Val(.CurrentRow.Cells(42).Value) - Val(.CurrentRow.Cells(33).Value), "#########0.00")
                            If vESI_FOR_OT_STS = 1 Then
                                vTotErngs_FOR_ESI = vTotErngs_FOR_ESI + Val(.CurrentRow.Cells(28).Value)
                            End If

                            If vESISTS_Sal = 1 Then

                                If Val(.CurrentRow.Cells(24).Value) > Val(ESI_MAX_SHFT_WAGES) And Val(vTotErngs_FOR_ESI) < 25000 Then
                                    '----If Shift Salary graterthan 100 then ESI allowed
                                    .CurrentRow.Cells(46).Value = Format(Math.Round(Val(vTotErngs_FOR_ESI) * 1.75 / 100), "#########0.00")

                                End If
                                .CurrentRow.Cells(74).Value = ""
                                .CurrentRow.Cells(75).Value = Format(Val(.CurrentRow.Cells(46).Value) + Val(.CurrentRow.Cells(74).Value), "#############0")

                            Else
                                .CurrentRow.Cells(46).Value = ""
                                .CurrentRow.Cells(74).Value = ""
                                .CurrentRow.Cells(75).Value = ""


                            End If

                            If vESISTS_Audit = 1 Then

                                If Val(.CurrentRow.Cells(24).Value) > Val(ESI_MAX_SHFT_WAGES) And Val(vTotErngs_FOR_ESI) < 25000 Then
                                    '----If Shift Salary graterthan 100 then ESI allowed
                                    .CurrentRow.Cells(71).Value = Format(Math.Round(Val(vTotErngs_FOR_ESI) * 1.75 / 100), "#########0.00")

                                End If

                            Else
                                .CurrentRow.Cells(71).Value = ""

                            End If

                        End If


                        '---PF 12 %
                        If e.ColumnIndex = 25 Or e.ColumnIndex = 30 Then

                            If Val(.CurrentRow.Cells(25).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then

                                If vPFSTS_Sal = 1 Then
                                    .CurrentRow.Cells(47).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 12 / 100), "#########0.00")
                                    .CurrentRow.Cells(48).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 8.33 / 100), "#########0.00")
                                End If

                                If vPFSTS_Audit = 1 Then
                                    .CurrentRow.Cells(72).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 12 / 100), "#########0.00")
                                    .CurrentRow.Cells(73).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 8.33 / 100), "#########0.00")
                                End If


                            Else

                                If vPFSTS_Sal = 1 Then
                                    .CurrentRow.Cells(47).Value = Format(Math.Ceiling(Val(.CurrentRow.Cells(30).Value) * 12 / 100), "#########0.00")
                                    .CurrentRow.Cells(48).Value = Format(Math.Ceiling(Val(.CurrentRow.Cells(30).Value) * 8.33 / 100), "#########0.00")
                                End If
                                If vPFSTS_Audit = 1 Then
                                    .CurrentRow.Cells(72).Value = Format(Math.Ceiling(Val(.CurrentRow.Cells(30).Value) * 12 / 100), "#########0.00")
                                    .CurrentRow.Cells(73).Value = Format(Math.Ceiling(Val(.CurrentRow.Cells(30).Value) * 8.33 / 100), "#########0.00")
                                End If

                            End If

                            .CurrentRow.Cells(49).Value = Format(Val(.CurrentRow.Cells(47).Value) - Val(.CurrentRow.Cells(48).Value), "#########0.00")
                            .CurrentRow.Cells(76).Value = Format(Val(.CurrentRow.Cells(72).Value) - Val(.CurrentRow.Cells(73).Value), "#########0.00")

                        End If


                        '---Additions
                        If e.ColumnIndex = 29 Or e.ColumnIndex = 31 Or e.ColumnIndex = 32 Or e.ColumnIndex = 33 Or e.ColumnIndex = 34 Or e.ColumnIndex = 35 Or e.ColumnIndex = 36 Or e.ColumnIndex = 37 Or e.ColumnIndex = 38 Or e.ColumnIndex = 39 Or e.ColumnIndex = 40 Or e.ColumnIndex = 41 Then
                            .CurrentRow.Cells(42).Value = Val(.CurrentRow.Cells(29).Value) + Val(.CurrentRow.Cells(31).Value) + Val(.CurrentRow.Cells(32).Value) + Val(.CurrentRow.Cells(33).Value) + Val(.CurrentRow.Cells(34).Value) + Val(.CurrentRow.Cells(35).Value) + Val(.CurrentRow.Cells(36).Value) + Val(.CurrentRow.Cells(37).Value) + Val(.CurrentRow.Cells(38).Value) + Val(.CurrentRow.Cells(39).Value) + Val(.CurrentRow.Cells(40).Value) + Val(.CurrentRow.Cells(41).Value)
                        End If

                        '---Deductions
                        If e.ColumnIndex = 43 Or e.ColumnIndex = 44 Or e.ColumnIndex = 45 Or e.ColumnIndex = 46 Or e.ColumnIndex = 47 Or e.ColumnIndex = 50 Or e.ColumnIndex = 51 Or e.ColumnIndex = 52 Then
                            .CurrentRow.Cells(53).Value = Val(.CurrentRow.Cells(43).Value) + Val(.CurrentRow.Cells(44).Value) + Val(.CurrentRow.Cells(45).Value) + Val(.CurrentRow.Cells(46).Value) + Val(.CurrentRow.Cells(47).Value) + Val(.CurrentRow.Cells(51).Value) + Val(.CurrentRow.Cells(52).Value)
                        End If

                        '--- Net Salary
                        If e.ColumnIndex = 3 Or e.ColumnIndex = 6 Or e.ColumnIndex = 28 Or e.ColumnIndex = 42 Or e.ColumnIndex = 53 Or e.ColumnIndex = 54 Then
                            If Val(.CurrentRow.Cells(6).Value) <> 0 Then
                                .CurrentRow.Cells(55).Value = Format(Val(.CurrentRow.Cells(3).Value) + Val(.CurrentRow.Cells(28).Value) + Val(.CurrentRow.Cells(42).Value) - Val(.CurrentRow.Cells(53).Value) + Val(.CurrentRow.Cells(54).Value), "#########0.00")
                            Else
                                .CurrentRow.Cells(55).Value = ""
                            End If

                        End If

                        '--- Net Pay
                        If e.ColumnIndex = 6 Or e.ColumnIndex = 55 Or e.ColumnIndex = 56 Or e.ColumnIndex = 57 Or e.ColumnIndex = 59 Or e.ColumnIndex = 60 Then


                            If Val(.CurrentRow.Cells(6).Value) <> 0 Then
                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL
                                    .CurrentRow.Cells(61).Value = Format(Val(.CurrentRow.Cells(55).Value) - Val(.CurrentRow.Cells(57).Value), "##########0.00")
                                Else
                                    '.CurrentRow.Cells(61).Value = Format((((Val(.CurrentRow.Cells(55).Value) - Val(.CurrentRow.Cells(57).Value))) - Val(.CurrentRow.Cells(59).Value)) + Val(.CurrentRow.Cells(60).Value), "##########0.00")
                                    .CurrentRow.Cells(61).Value = Format((((Val(.CurrentRow.Cells(55).Value) - Val(.CurrentRow.Cells(57).Value))) - Val(.CurrentRow.Cells(59).Value)), "##########0.00")
                                End If

                            Else
                                .CurrentRow.Cells(61).Value = ""

                            End If

                        End If

                        .CurrentRow.Cells(5).Value = Format(Val(.CurrentRow.Cells(61).Value), "###########0.00")
                        .CurrentRow.Cells(58).Value = Format(Val(.CurrentRow.Cells(56).Value) - Val(.CurrentRow.Cells(57).Value), "#########0.00")

                    End If

                End If


            End With

            'Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value) + Val(.Rows(n).Cells(77).Value), "##########0.00")
            'Val(.Rows(n).Cells(3).Value) + Val(.Rows(n).Cells(28).Value) + Val(.Rows(n).Cells(42).Value) - Val(.Rows(n).Cells(53).Value) + Val(.Rows(n).Cells(54).Value)



        Catch ex As NullReferenceException

            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException

            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                End With

            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv1 = Nothing
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_FestivalDays_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FestivalDays.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True

            Else
                btn_save.Focus()

            End If
        End If

        If e.KeyValue = 38 Then txt_TotalDays.Focus()

    End Sub

    Private Sub txt_TotalDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TotalDays.LostFocus
        txt_TotalDays.Text = Format(Val(txt_TotalDays.Text), "#########0.00")
    End Sub

    Private Sub txt_FestivalDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FestivalDays.LostFocus
        txt_FestivalDays.Text = Format(Val(txt_FestivalDays.Text), "#########0.00")
    End Sub

    Private Sub txt_FestivalDays_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FestivalDays.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.RowCount > 0 Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub txt_VatAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TotalDays.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub dtp_ToDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ToDate.GotFocus
        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
        dtp_Advance_UpToDate.Tag = dtp_Advance_UpToDate.Text
    End Sub

    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ToDate.KeyPress
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try
            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Or Trim(UCase(dtp_Advance_UpToDate.Tag)) <> Trim(UCase(dtp_Advance_UpToDate.Text)) Then

                    get_PayRoll_Salary_Details()

                End If

                If dtp_Advance_UpToDate.Visible And dtp_Advance_UpToDate.Enabled Then
                    dtp_Advance_UpToDate.Focus()

                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TODATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dtp_FromDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_FromDate.GotFocus
        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
        dtp_Advance_UpToDate.Tag = dtp_Advance_UpToDate.Text
    End Sub

    Private Sub dtp_FromDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FromDate.KeyPress
        Dim DtTm As Date
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try

            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Or Trim(UCase(dtp_Advance_UpToDate.Tag)) <> Trim(UCase(dtp_Advance_UpToDate.Text)) Then

                    SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

                    Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

                    If Trim(UCase(Mon_Wek)) = "WEEKLY" Then

                        DtTm = dtp_FromDate.Value.Date

                        DtTm = DateAdd("d", 6, DtTm)

                        dtp_ToDate.Text = DtTm

                    End If

                    get_PayRoll_Salary_Details()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE FROMDATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then

        pnl_Back.Enabled = False
        pnl_PrintEmployee_Details.Visible = True
        printEmployee_Selection()
        If btn_Print_Employee.Enabled Then btn_Print_Employee.Focus()

        'Else
        '    printing_Salary()
        'End If

    End Sub

    Private Sub btn_Calculation_Salary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Calculation_Salary.Click
        get_PayRoll_Salary_Details()
    End Sub

    Private Sub dtp_Advance_UpToDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Advance_UpToDate.GotFocus
        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
        dtp_Advance_UpToDate.Tag = dtp_Advance_UpToDate.Text
    End Sub

    Private Sub dtp_Advance_UpToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Advance_UpToDate.KeyPress
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try
            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Or Trim(UCase(dtp_Advance_UpToDate.Tag)) <> Trim(UCase(dtp_Advance_UpToDate.Text)) Then

                    get_PayRoll_Salary_Details()
                    dgv_Details.Focus()

                End If

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    'dtp_Date.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(43)
                    dgv_Details.Focus()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE ADVANCE UPTO DATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv1 = dgv_Details
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        '  If .CurrentCell.ColumnIndex = 50 Or .CurrentCell.ColumnIndex = 51 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                        'End If

                    End If

                End If
            End With

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_SalaryList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SalaryList.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Salary Register Simple1"
        Common_Procedures.RptInputDet.ReportHeading = "Salary Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON,PT"
        f.MdiParent = MDIParent1
        f.Show()
        f.dtp_FromDate.Text = dtp_Date.Text
        f.dtp_ToDate.Text = dtp_Date.Text



    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                End With

            End If



        Catch ex As Exception

            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_Details_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Leave
        dgv1 = Nothing
    End Sub


    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged

        Try

            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                    End If
                End If
            End With


        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub printing_Salary()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String = ""
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        'Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            'OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            'OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            'With dgv_Print_Details

            'For i = 0 To .RowCount - 1

            'If Val(.Rows(i).Cells(2)) = "1" Then

            da1 = New SqlClient.SqlDataAdapter("select * from PayRoll_Salary_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and Salary_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_orderby, Salary_No", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            ' End If

            ' Next




            ' End With





            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next

        'Else

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 4X6", 400, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.Landscape = True


        'End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        Dim pkCustomSize2 As New System.Drawing.Printing.PaperSize("PAPER 4X6", 400, 600)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize2
                        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize2
                        PrintDocument1.DefaultPageSettings.Landscape = True

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
        Dim NewCode As String
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim Emp_id As Integer = 0
        Dim vSelc_EmpIDNOS As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        'prn_Prev_HeadIndx = -100
        'prn_HeadIndx = 0

        prn_DetIndx = 0
        prn_DetSNo = 0


        Try

            vSelc_EmpIDNOS = ""
            For i = 0 To dgv_Print_Details.RowCount - 1

                If Val(dgv_Print_Details.Rows(i).Cells(2).Value) = 1 Then
                    Emp_id = Common_Procedures.Employee_NameToIdNo(con, dgv_Print_Details.Rows(i).Cells(1).Value)
                    vSelc_EmpIDNOS = Trim(vSelc_EmpIDNOS) & IIf(Trim(vSelc_EmpIDNOS) <> "", ", ", "") & Trim(Val(Emp_id))

                End If


            Next
            If Trim(vSelc_EmpIDNOS) <> "" Then
                vSelc_EmpIDNOS = "(" & Trim(vSelc_EmpIDNOS) & ")"
            Else
                vSelc_EmpIDNOS = "(-9999)"
            End If


            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, mh.* from PayRoll_Salary_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Month_Head mh ON mh.month_IdNo = a.Month_IdNo  Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code ='" & Trim(NewCode) & "' Order by a.for_orderby, a.Salary_No", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)


            If prn_HdDt.Rows.Count > 0 Then


                da2 = New SqlClient.SqlDataAdapter("select a.*, c.* ,dh.*  from PayRoll_Salary_Details a LEFT OUTER JOIN PayRoll_Employee_Head c ON a.Employee_IdNo = c.Employee_IdNo LEFT OUTER JOIN Department_Head Dh ON c.Department_IdNo = dh.Department_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code = '" & Trim(NewCode) & "' and a.Employee_Idno IN " & vSelc_EmpIDNOS & " Order by a.sl_no", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, c.*   from PayRoll_Salary_Details a LEFT OUTER JOIN PayRoll_Employee_Head c ON a.Employee_IdNo = c.Employee_IdNo    where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)



            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If


            da1.Dispose()


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then
            Printing_Format2(e)
        Else
            'Printing_Format1(e)
            Printing_Format3(e)
        End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String = ""
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim p1Font As Font
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    Debug.Print(ps.PaperName)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
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
        NoofItems_PerPage = 1 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 450
        ClArr(2) = PageWidth - (LMargin + ClArr(1))

        TxtHgt = 17 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntryCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            'CurY = CurY + TxtHgt

                            'Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, p1Font)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_Name").ToString)
                        ''ItmNm2 = ""
                        ''If Len(ItmNm1) > 25 Then
                        ''    For I = 25 To 1 Step -1
                        ''        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        ''    Next I
                        ''    If I = 0 Then I = 25
                        ''    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ''    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        ''End If

                        CurY = CurY + TxtHgt

                        ''SNo = SNo + 1
                        pFont = New Font("Baamini", 9, FontStyle.Regular)
                        p1Font = New Font("Calibri", 9, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "thuj;jpd; Fie;j gl;r rk;gsk; ", LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Advance", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + ClArr(1) + 110, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, "Kd;gzk; ", LMargin + ClArr(1) + 120, CurY + 2, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Earning").ToString, LMargin + ClArr(1) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_Advance").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                        CurY = CurY + TxtHgt - 8

                        Common_Procedures.Print_To_PrintDocument(e, "Miniumum Wages Earned in the Week", LMargin + 10, CurY, 0, 0, p1Font)
                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "thuj;jpd; nkhj;j ,jug;gbfs; ", LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "P.F", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + ClArr(1) + 110, CurY, 0, 0, p1Font)

                        Common_Procedures.Print_To_PrintDocument(e, "gp.vg;.", LMargin + ClArr(1) + 120, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, "@ 12 %", LMargin + ClArr(1) + 170, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString, LMargin + ClArr(1) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("P_F").ToString, PageWidth - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt - 8

                        Common_Procedures.Print_To_PrintDocument(e, "Total other allowance in the Week", LMargin + 10, CurY, 0, 0, p1Font)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "xU kzp Neuj;jpw;Fhpa $Ljy; rk;gsk; ", LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "E.S.I", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + ClArr(1) + 110, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, ",.v];.I ", LMargin + ClArr(1) + 120, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "@ 1.75 %", LMargin + ClArr(1) + 170, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ot_Pay_Hours").ToString, LMargin + ClArr(1) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ESI").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                        CurY = CurY + TxtHgt - 8
                        Common_Procedures.Print_To_PrintDocument(e, "OT Wages Hour", LMargin + 10, CurY, 0, 0, p1Font)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "thuj;jpd; Ntiy nra;j $Ljy; kzp Neuk; ", LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Other Deductions", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + ClArr(1) + 110, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, ",ju gpbj;jq;fs; ", LMargin + ClArr(1) + 120, CurY + 2, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ot_Hours").ToString, LMargin + ClArr(1) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Other_Deduction").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                        CurY = CurY + TxtHgt - 8
                        Common_Procedures.Print_To_PrintDocument(e, "Total OT Hours Worked in the week", LMargin + 10, CurY, 0, 0, p1Font)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "$Ljy; Neuj;jpw;fhd nkhj;j rk;gsk; ", LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "P.Tax / L.W.F", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ot_Salary").ToString, LMargin + ClArr(1) - 10, CurY, 1, 0, pFont)
                        CurY = CurY + TxtHgt - 8
                        Common_Procedures.Print_To_PrintDocument(e, "Wages for OT Hours Worked ", LMargin + 10, CurY, 0, 0, p1Font)


                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)



            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False


    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1, S1, W1 As Single
        Dim Sary As Single, TotSalry As Single

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SALARY DETAILS", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Baamini", 9, FontStyle.Regular)
            pFont = New Font("Calibri ", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "chpkk; vz; ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LICENSE NO ", LMargin + 150, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 240, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Salary_No").ToString, LMargin + 250, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "rk;gs fhyk;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("From_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + 90, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Kjy;", LMargin + ClAr(1) + 160, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("To_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + 210, CurY, 0, 0, pFont)
            e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY - 5, LMargin + ClAr(1) - 20, CurY - 5)
            LnAr(11) = CurY

            Common_Procedures.Print_To_PrintDocument(e, "rk;gs urPJ", LMargin + 360, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt



            Common_Procedures.Print_To_PrintDocument(e, "Nlhf;fd; vz;", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Token No ", LMargin + 150, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 240, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & prn_DetDt.Rows(prn_DetIndx).Item("Card_No").ToString, LMargin + 250, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "je;ij ngah;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ": " & prn_DetDt.Rows(prn_DetIndx).Item("Father_Husband").ToString, LMargin + ClAr(1) + 110, CurY, 0, 0, pFont)
            e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY, LMargin + ClAr(1) - 20, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY, LMargin + 350, LnAr(11) - 5)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 20, CurY, LMargin + ClAr(1) - 20, LnAr(11) - 5)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY - 5, PageWidth, CurY - 5)
            LnAr(12) = CurY
            Common_Procedures.Print_To_PrintDocument(e, "njhopyhspapad; ngah;", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Employee Name :  " & prn_DetDt.Rows(prn_DetIndx).Item("Employee_Name").ToString, LMargin + 150, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "mbg;gil rk;gsk;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Basic", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Basic_Salary").ToString), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "gzp", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Designation  " & prn_DetDt.Rows(prn_DetIndx).Item("Designation").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "gQ;rg;gb", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DA", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("D_A").ToString), "############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "gpwe;j Njjp", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DOB :" & prn_DetDt.Rows(0).Item("Date_Birth").ToString, LMargin + 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "gp.vg;.vz;", LMargin + 220, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PF NO : " & prn_DetDt.Rows(0).Item("Pf_No").ToString, LMargin + 300, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ",jug;gbfs;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Allowances", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            Dim Rndof As Single = 0
            Sary = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Basic_Salary").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("D_A").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString), "##########0")
            TotSalry = Format(Val(Sary), "##########0")
            TotSalry = Common_Procedures.Currency_Format(Val(TotSalry))

            Rndof = Format(Val(CSng(TotSalry)) - Val(Sary), "#########0.00")


            Common_Procedures.Print_To_PrintDocument(e, "Nrh;e;j Njjp", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DOJ : " & prn_DetDt.Rows(prn_DetIndx).Item("Join_Date").ToString, LMargin + 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ",.v];.I.vz;", LMargin + 220, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "ESI NO : " & prn_DetDt.Rows(prn_DetIndx).Item("Esi_No").ToString, LMargin + 300, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "tl;lkhf;fy;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Roundoff", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & Rndof, PageWidth - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 210, CurY, PageWidth, CurY)

            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(Sary), "###########0.00"), PageWidth - 10, CurY + 5, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "thuj;jpd; Ntiy nra;j ehl;fs;", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_HdDt.Rows(0).Item("Total_Days").ToString), "###########"), LMargin + 210, CurY, 0, 0, pFont)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, PageWidth, CurY)
            LnAr(13) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(12) - 5)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 210, CurY, LMargin + ClAr(1) + 210, LnAr(12) - 5)
            CurY = CurY + TxtHgt - 8
            Common_Procedures.Print_To_PrintDocument(e, "Total Days Worked    ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E.S.I.Days :  " & Format(Val(prn_HdDt.Rows(0).Item("Total_Days").ToString), "############0"), LMargin + 200, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 13
            Common_Procedures.Print_To_PrintDocument(e, "<l;ba rk;gsk;", LMargin, CurY, 2, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "gpbj;jq;fs;", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
            prn_DetIndx = prn_DetIndx + 1
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Total Salary", LMargin + 50, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 120, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Total Dedn", LMargin + 460, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 530, CurY, 0, 0, p1Font)
        ' Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Net_Salary").ToString, LMargin + ClAr(1) - 10, CurY, 1, 0, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_Deduction").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt - 15
        pFont = New Font("Baamini", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "nkhj;j rk;gsk; ", LMargin + 130, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "nkhj;j gpbj;jk; ", LMargin + 540, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))



        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(6) = CurY

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SALARY PAID", LMargin + 150, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt - 15
        pFont = New Font("Baamini", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "toq;fg;gl;l rk;gsk; ", LMargin + 250, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 9, FontStyle.Regular)

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        pFont = New Font("Baamini", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "njhopyhspapd; ifnahg;gk; ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 200, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "rk;gs Njjp ", LMargin + 300, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 400, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Employee Signature", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Salary Paid on :", LMargin + 300, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub ShowOrHideColumns()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try
            'dgv_Details.Columns(1).Visible = False  
            'dgv_Details.Columns(2).Visible = False
            'dgv_Details.Columns(3).Visible = False
            'dgv_Details.Columns(4).Visible = False
            'dgv_Details.Columns(5).Visible = False
            dgv_Details.Columns(7).Visible = False
            dgv_Details.Columns(8).Visible = False
            dgv_Details.Columns(9).Visible = False
            dgv_Details.Columns(10).Visible = False
            dgv_Details.Columns(11).Visible = False
            dgv_Details.Columns(12).Visible = False
            dgv_Details.Columns(13).Visible = False
            dgv_Details.Columns(14).Visible = False
            dgv_Details.Columns(15).Visible = False
            dgv_Details.Columns(16).Visible = False
            dgv_Details.Columns(17).Visible = False
            dgv_Details.Columns(18).Visible = False
            dgv_Details.Columns(19).Visible = False
            dgv_Details.Columns(20).Visible = False
            dgv_Details.Columns(21).Visible = False
            dgv_Details.Columns(22).Visible = False
            dgv_Details.Columns(23).Visible = False
            dgv_Details.Columns(24).Visible = False
            dgv_Details.Columns(25).Visible = False
            dgv_Details.Columns(26).Visible = False
            dgv_Details.Columns(27).Visible = False
            dgv_Details.Columns(28).Visible = False
            dgv_Details.Columns(29).Visible = False
            dgv_Details.Columns(30).Visible = False
            dgv_Details.Columns(31).Visible = False
            dgv_Details.Columns(32).Visible = False
            dgv_Details.Columns(33).Visible = False
            dgv_Details.Columns(34).Visible = False
            dgv_Details.Columns(35).Visible = False
            dgv_Details.Columns(36).Visible = False
            dgv_Details.Columns(37).Visible = False
            dgv_Details.Columns(38).Visible = False
            dgv_Details.Columns(39).Visible = False
            dgv_Details.Columns(40).Visible = False
            dgv_Details.Columns(41).Visible = False
            dgv_Details.Columns(42).Visible = False
            dgv_Details.Columns(43).Visible = False
            dgv_Details.Columns(44).Visible = False
            dgv_Details.Columns(45).Visible = False
            dgv_Details.Columns(46).Visible = False
            dgv_Details.Columns(47).Visible = False
            dgv_Details.Columns(48).Visible = False
            dgv_Details.Columns(49).Visible = False
            dgv_Details.Columns(50).Visible = False
            dgv_Details.Columns(51).Visible = False
            dgv_Details.Columns(52).Visible = False
            dgv_Details.Columns(53).Visible = False
            dgv_Details.Columns(54).Visible = False
            dgv_Details.Columns(55).Visible = False
            'dgv_Details.Columns(56).Visible = False
            'dgv_Details.Columns(57).Visible = False
            'dgv_Details.Columns(58).Visible = False
            dgv_Details.Columns(59).Visible = False
            dgv_Details.Columns(60).Visible = False
            dgv_Details.Columns(61).Visible = False

            dgv_Details.Columns(1).Visible = False

            da = New SqlClient.SqlDataAdapter("select top 1 a.* from PayRoll_Settings a ", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0).Item("Employee_IdNo").ToString) = False Then
                    If Val(dt.Rows(0).Item("Employee_IdNo")) = 1 Then
                        dgv_Details.Columns(1).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Basic_Salary").ToString) = False Then
                    If Val(dt.Rows(0).Item("Basic_Salary")) = 1 Then
                        dgv_Details.Columns(3).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Total_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("Total_Days")) = 1 Then
                        dgv_Details.Columns(4).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Net_Pay").ToString) = False Then
                    If Val(dt.Rows(0).Item("Net_Pay")) = 1 Then
                        dgv_Details.Columns(5).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("No_Of_Attendance_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("No_Of_Attendance_Days")) = 1 Then
                        dgv_Details.Columns(6).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("From_W_off_CR").ToString) = False Then
                    If Val(dt.Rows(0).Item("From_W_off_CR")) = 1 Then
                        dgv_Details.Columns(7).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("From_Cl_For_Leave").ToString) = False Then
                    If Val(dt.Rows(0).Item("From_Cl_For_Leave")) = 1 Then
                        dgv_Details.Columns(8).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("From_SL_For_Leave").ToString) = False Then
                    If Val(dt.Rows(0).Item("From_SL_For_Leave")) = 1 Then
                        dgv_Details.Columns(9).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Festival_Holidays").ToString) = False Then
                    If Val(dt.Rows(0).Item("Festival_Holidays")) = 1 Then
                        dgv_Details.Columns(10).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Total_Leave_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("Total_Leave_Days")) = 1 Then
                        dgv_Details.Columns(11).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("No_Of_Leave").ToString) = False Then
                    If Val(dt.Rows(0).Item("No_Of_Leave")) = 1 Then
                        dgv_Details.Columns(12).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Attendance_On_W_Off_FH").ToString) = False Then
                    If Val(dt.Rows(0).Item("Attendance_On_W_Off_FH")) = 1 Then
                        dgv_Details.Columns(13).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Op_W_Off_CR").ToString) = False Then
                    If Val(dt.Rows(0).Item("Op_W_Off_CR")) = 1 Then
                        dgv_Details.Columns(14).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Add_W_Off_CR").ToString) = False Then
                    If Val(dt.Rows(0).Item("Add_W_Off_CR")) = 1 Then
                        dgv_Details.Columns(15).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Less_W_Off_CR").ToString) = False Then
                    If Val(dt.Rows(0).Item("Less_W_Off_CR")) = 1 Then
                        dgv_Details.Columns(16).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Total_W_Off_CR").ToString) = False Then
                    If Val(dt.Rows(0).Item("Total_W_Off_CR")) = 1 Then
                        dgv_Details.Columns(17).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("OP_CL_CR_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("OP_CL_CR_Days")) = 1 Then
                        dgv_Details.Columns(18).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Less_CL_CR_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("Less_CL_CR_Days")) = 1 Then
                        dgv_Details.Columns(19).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Total_CL_CR_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("Total_CL_CR_Days")) = 1 Then
                        dgv_Details.Columns(20).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("OP_SL_CR_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("OP_SL_CR_Days")) = 1 Then
                        dgv_Details.Columns(21).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Less_SL_CR_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("Less_SL_CR_Days")) = 1 Then
                        dgv_Details.Columns(22).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Total_SL_CR_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("Total_SL_CR_Days")) = 1 Then
                        dgv_Details.Columns(23).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Salary_Days").ToString) = False Then
                    If Val(dt.Rows(0).Item("Salary_Days")) = 1 Then
                        dgv_Details.Columns(24).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Basic_Pay").ToString) = False Then
                    If Val(dt.Rows(0).Item("Basic_Pay")) = 1 Then
                        dgv_Details.Columns(25).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("OT_Hours").ToString) = False Then
                    If Val(dt.Rows(0).Item("OT_Hours")) = 1 Then
                        dgv_Details.Columns(26).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Ot_Pay_Hours").ToString) = False Then
                    If Val(dt.Rows(0).Item("Ot_Pay_Hours")) = 1 Then
                        dgv_Details.Columns(27).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Ot_Salary").ToString) = False Then
                    If Val(dt.Rows(0).Item("Ot_Salary")) = 1 Then
                        dgv_Details.Columns(28).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("D_A").ToString) = False Then
                    If Val(dt.Rows(0).Item("D_A")) = 1 Then
                        dgv_Details.Columns(29).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Earning").ToString) = False Then
                    If Val(dt.Rows(0).Item("Earning")) = 1 Then
                        dgv_Details.Columns(30).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("H_R_A").ToString) = False Then
                    If Val(dt.Rows(0).Item("H_R_A")) = 1 Then
                        dgv_Details.Columns(31).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Conveyance").ToString) = False Then
                    If Val(dt.Rows(0).Item("Conveyance")) = 1 Then
                        dgv_Details.Columns(32).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Washing").ToString) = False Then
                    If Val(dt.Rows(0).Item("Washing")) = 1 Then
                        dgv_Details.Columns(33).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Entertainment").ToString) = False Then
                    If Val(dt.Rows(0).Item("Entertainment")) = 1 Then
                        dgv_Details.Columns(34).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Maintenance").ToString) = False Then
                    If Val(dt.Rows(0).Item("Maintenance")) = 1 Then
                        dgv_Details.Columns(35).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Provision").ToString) = False Then
                    If Val(dt.Rows(0).Item("Provision")) = 1 Then
                        dgv_Details.Columns(36).Visible = True
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("Other_Addition2").ToString) = False Then
                    If Val(dt.Rows(0).Item("Other_Addition2")) = 1 Then
                        dgv_Details.Columns(37).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Other_Addition3").ToString) = False Then
                    If Val(dt.Rows(0).Item("Other_Addition3")) = 1 Then
                        dgv_Details.Columns(38).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Other_Addition").ToString) = False Then
                    If Val(dt.Rows(0).Item("Other_Addition")) = 1 Then
                        dgv_Details.Columns(39).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Incentive_Amount").ToString) = False Then
                    If Val(dt.Rows(0).Item("Incentive_Amount")) = 1 Then
                        dgv_Details.Columns(40).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Week_Off_Allowance").ToString) = False Then
                    If Val(dt.Rows(0).Item("Week_Off_Allowance")) = 1 Then
                        dgv_Details.Columns(41).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Total_Addition").ToString) = False Then
                    If Val(dt.Rows(0).Item("Total_Addition")) = 1 Then
                        dgv_Details.Columns(42).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Mess").ToString) = False Then
                    If Val(dt.Rows(0).Item("Mess")) = 1 Then
                        dgv_Details.Columns(43).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Medical").ToString) = False Then
                    If Val(dt.Rows(0).Item("Medical")) = 1 Then
                        dgv_Details.Columns(44).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Store").ToString) = False Then
                    If Val(dt.Rows(0).Item("Store")) = 1 Then
                        dgv_Details.Columns(45).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("ESI").ToString) = False Then
                    If Val(dt.Rows(0).Item("ESI")) = 1 Then
                        dgv_Details.Columns(46).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("P_F").ToString) = False Then
                    If Val(dt.Rows(0).Item("P_F")) = 1 Then
                        dgv_Details.Columns(47).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("E_P_F").ToString) = False Then
                    If Val(dt.Rows(0).Item("E_P_F")) = 1 Then
                        dgv_Details.Columns(48).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Pension_Scheme").ToString) = False Then
                    If Val(dt.Rows(0).Item("Pension_Scheme")) = 1 Then
                        dgv_Details.Columns(49).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("late_Mins").ToString) = False Then
                    If Val(dt.Rows(0).Item("late_Mins")) = 1 Then
                        dgv_Details.Columns(50).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Late_Hours_Salary").ToString) = False Then
                    If Val(dt.Rows(0).Item("Late_Hours_Salary")) = 1 Then
                        dgv_Details.Columns(51).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Other_Deduction").ToString) = False Then
                    If Val(dt.Rows(0).Item("Other_Deduction")) = 1 Then
                        dgv_Details.Columns(52).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Total_Deduction").ToString) = False Then
                    If Val(dt.Rows(0).Item("Total_Deduction")) = 1 Then
                        dgv_Details.Columns(53).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Attendance_Incentive").ToString) = False Then
                    If Val(dt.Rows(0).Item("Attendance_Incentive")) = 1 Then
                        dgv_Details.Columns(54).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Net_Salary").ToString) = False Then
                    If Val(dt.Rows(0).Item("Net_Salary")) = 1 Then
                        dgv_Details.Columns(55).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Total_Advance").ToString) = False Then
                    If Val(dt.Rows(0).Item("Total_Advance")) = 1 Then
                        dgv_Details.Columns(56).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Minus_Advance").ToString) = False Then
                    If Val(dt.Rows(0).Item("Minus_Advance")) = 1 Then
                        dgv_Details.Columns(57).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Balance_Advance").ToString) = False Then
                    If Val(dt.Rows(0).Item("Balance_Advance")) = 1 Then
                        dgv_Details.Columns(58).Visible = True
                    End If
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL
                    If IsDBNull(dt.Rows(0).Item("Salary_Advance").ToString) = False Then
                        If Val(dt.Rows(0).Item("Salary_Advance")) = 1 Then
                            dgv_Details.Columns(59).Visible = True
                        End If
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("Salary_Pending").ToString) = False Then
                    If Val(dt.Rows(0).Item("Salary_Pending")) = 1 Then
                        dgv_Details.Columns(60).Visible = True
                    End If
                End If
                If IsDBNull(dt.Rows(0).Item("Net_Pay_Amount").ToString) = False Then
                    If Val(dt.Rows(0).Item("Net_Pay_Amount")) = 1 Then
                        dgv_Details.Columns(61).Visible = True
                    End If
                End If
            End If



            dt.Clear()

            Get_Columns_Head_Name()


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try '
    End Sub
    Private Sub Get_Columns_Head_Name()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 * from PayRoll_Settings order by Auto_SlNo", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then

                    dgv_Details.Columns(32).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption1").ToString))
                    dgv_Details.Columns(33).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption2").ToString))
                    dgv_Details.Columns(34).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption3").ToString))
                    dgv_Details.Columns(35).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption4").ToString))
                    dgv_Details.Columns(36).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption5").ToString))
                    dgv_Details.Columns(37).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption6").ToString))
                    dgv_Details.Columns(38).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption7").ToString))

                    dgv_Details.Columns(44).HeaderText = UCase(Trim(dt.Rows(0).Item("Ded_Caption1").ToString))
                    dgv_Details.Columns(45).HeaderText = UCase(Trim(dt.Rows(0).Item("Ded_Caption2").ToString))


                    dgv_Details.Columns(32).DefaultCellStyle.BackColor = Color.Bisque
                    dgv_Details.Columns(33).DefaultCellStyle.BackColor = Color.Bisque
                    dgv_Details.Columns(34).DefaultCellStyle.BackColor = Color.Bisque
                    dgv_Details.Columns(35).DefaultCellStyle.BackColor = Color.Bisque
                    dgv_Details.Columns(36).DefaultCellStyle.BackColor = Color.Bisque
                    dgv_Details.Columns(37).DefaultCellStyle.BackColor = Color.Bisque
                    dgv_Details.Columns(38).DefaultCellStyle.BackColor = Color.Bisque
                    'dgv_Details.Columns(40).DefaultCellStyle.BackColor = Color.Bisque
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub


    Private Sub btn_FindCardNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_FindCardNo.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim inpno As String

        Try

            inpno = InputBox("Enter Card No.", "FOR CARD NO FINDING...")

            If Trim(inpno) <> "" Then

                If dgv_Details.RowCount > 0 Then

                    For I = 0 To dgv_Details.RowCount - 1

                        If Trim(dgv_Details.Rows(I).Cells(2).Value) = Trim(inpno) Then

                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(I).Cells(1)
                            dgv_Details.CurrentCell.Selected = True

                            Exit Sub
                        End If

                    Next
                    MessageBox.Show("Card No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("Card No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Private Sub Payroll_Salary_Entry_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "F" Then

            btn_FindCardNo_Click(sender, e)

        End If
    End Sub

    Private Sub cbo_Category_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Category.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
        cbo_Category.Tag = cbo_Category.Text
    End Sub

    Private Sub cbo_Category_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Category.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Category, cbo_PaymentType, cbo_Year, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
    End Sub

    Private Sub cbo_Category_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Category.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Category, cbo_Year, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Get_Fixed_Valus_From_Settings_Head()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        ESI_MAX_SHFT_WAGES = 0
        EPF_MAX_BASICPAY = 0
        Da1 = New SqlClient.SqlDataAdapter("select * from " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..Settings_Head", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Basic_Wages_For_Esi").ToString()) = False Then
                ESI_MAX_SHFT_WAGES = Val(Dt1.Rows(0).Item("Basic_Wages_For_Esi").ToString)
            End If
            If IsDBNull(Dt1.Rows(0).Item("Basic_Pay_For_Epf").ToString()) = False Then
                EPF_MAX_BASICPAY = Val(Dt1.Rows(0).Item("Basic_Pay_For_Epf").ToString)
            End If
        End If
        Dt1.Dispose()
        Da1.Dispose()
    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String = ""
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim p1Font As Font
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single, LcurY As Single, RcurY As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim p2font As Font
        Dim p3font As Font
        Dim nprntot As Single
        Dim nOTsal As Single, nTotAdv As Single, nOTEsi As Single

        Dim LftNofDet As Integer = 0
        Dim RgtNofDet As Integer = 0
        Dim nHra As Single = 0
        Dim nWash As Single = 0
        Dim nConvey As Single = 0
        Dim nEnter As Single = 0
        Dim nOtherAdd As Single = 0


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 4X6", 400, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.Landscape = True

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15
            .Right = 20
            .Top = 20 ' 20
            .Bottom = 10
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 8, FontStyle.Regular)

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
                PageWidth = .Height - TMargin - 25
                PageHeight = .Width - RMargin - 10
            End With
            'With PrintDocument1.DefaultPageSettings.Margins
            '    .Left = 15
            '    .Right = TMargin
            '    .Top = 40 ' 20
            '    .Bottom = 10
            '    LMargin = .Left
            '    RMargin = .Right
            '    TMargin = .Top
            '    BMargin = .Bottom
            'End With
        End If
        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 200 : ClArr(2) = 95 : ClArr(3) = 100
        'ClArr(2) = PageWidth - (LMargin + ClArr(1))
        ClArr(4) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3))

        'TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        TxtHgt = 15.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntryCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)



                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    If prn_DetIndx <= prn_DetDt.Rows.Count - 1 Then




                        p2font = New Font("Calibri", 8, FontStyle.Bold)
                        p3font = New Font("Calibri", 9, FontStyle.Bold)
                        CurY = CurY + TxtHgt

                        p1Font = New Font("Calibri", 7, FontStyle.Regular)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Days").ToString) > 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "No Of Present", LMargin + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Days").ToString), "#######0") & " Days", LMargin + ClArr(1) + 50, CurY, 1, 0, p1Font)
                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Hours").ToString) > 0 Then
                            CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "OT Wages Hours", LMargin + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Hours").ToString), "#######0.0") & " Hours", LMargin + ClArr(1) + 50, CurY, 1, 0, p1Font)
                        End If

                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(6) = CurY
                        Common_Procedures.Print_To_PrintDocument(e, "Earnings", LMargin + 10, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + ClArr(1) - 15, CurY, 0, 0, p2font)

                        Common_Procedures.Print_To_PrintDocument(e, "Deductions", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 90, CurY, 0, 0, p2font)


                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(7) = CurY

                        NoofDets = 0
                        LftNofDet = 0
                        RgtNofDet = 0


                        LcurY = CurY + 3
                        nHra = Val(prn_DetDt.Rows(prn_DetIndx).Item("H_R_A").ToString)
                        nConvey = Val(prn_DetDt.Rows(prn_DetIndx).Item("Conveyance").ToString)
                        nWash = Val(prn_DetDt.Rows(prn_DetIndx).Item("Washing").ToString)
                        nEnter = Val(prn_DetDt.Rows(prn_DetIndx).Item("Entertainment")).ToString
                        nOtherAdd = Val(prn_DetDt.Rows(prn_DetIndx).Item("Other_Addition")).ToString

                        Common_Procedures.Print_To_PrintDocument(e, "Earnings", LMargin + 10, LcurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Earning").ToString) + Val(nHra) + Val(nConvey) + Val(nWash) + Val(nEnter) + Val(nOtherAdd), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        LftNofDet = LftNofDet + 1

                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("H_R_A").ToString) > 0 Then
                        'CurY = LcurY + TxtHgt
                        ' Common_Procedures.Print_To_PrintDocument(e, "HRA ", LMargin + 10, LcurY, 0, 0, p1Font)
                        '   Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("H_R_A").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)

                        ' End If

                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("Conveyance").ToString) > 0 Then
                        'LcurY = LcurY + TxtHgt
                        ' Common_Procedures.Print_To_PrintDocument(e, "Conveyance ", LMargin + 10, LcurY, 0, 0, p1Font)
                        ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Conveyance").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        '  LftNofDet = LftNofDet + 1
                        '   End If

                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("washing").ToString) > 0 Then
                        '    LcurY = LcurY + TxtHgt
                        '    Common_Procedures.Print_To_PrintDocument(e, "Washing ", LMargin + 10, LcurY, 0, 0, p1Font)
                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("washing").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        '    LftNofDet = LftNofDet + 1
                        'End If


                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("Entertainment").ToString) > 0 Then
                        '    LcurY = LcurY + TxtHgt
                        '    Common_Procedures.Print_To_PrintDocument(e, "Entertainment ", LMargin + 10, LcurY, 0, 0, p1Font)
                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Entertainment").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        '    LftNofDet = LftNofDet + 1
                        'End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Salary").ToString) > 0 Then
                            LcurY = LcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Extra Time Salary ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Salary").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                            LftNofDet = LftNofDet + 1
                        End If


                        '----DEDUCTION DETAILS
                        RcurY = CurY + 3

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Mess").ToString) > 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Mess", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("mess").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                            RgtNofDet = RgtNofDet + 1
                        End If
                        'Common_Procedures.Print_To_PrintDocument(e, "Miniumum Wages Earned in the Week", LMargin + 10, CurY, 0, 0, p1Font)


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Medical").ToString) > 0 Then
                            RcurY = RcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Medical", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Medical").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                            RgtNofDet = RgtNofDet + 1
                        End If



                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("SALARY_OT_ESI").ToString) > 0 Then
                            RcurY = RcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "ESI @ 1.75%", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("SALARY_OT_ESI").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                            RgtNofDet = RgtNofDet + 1
                        End If



                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("P_F").ToString) > 0 Then
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("PF_Credit_Amount").ToString) = 0 Then
                                RcurY = RcurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "P F 12%", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("P_F").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                                RgtNofDet = RgtNofDet + 1
                            End If
                        End If









                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Store").ToString) > 0 Then
                            RcurY = RcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Store", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Store").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                            RgtNofDet = RgtNofDet + 1
                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Minus_Advance").ToString) > 0 Then
                            RcurY = RcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Advance Deduction", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Minus_Advance").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                            RgtNofDet = RgtNofDet + 1
                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Late_Hours_Salary").ToString) > 0 Then
                            RcurY = RcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Late Hours Salary", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Late_Hours_Salary").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                            RgtNofDet = RgtNofDet + 1
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Other_Deduction").ToString) > 0 Then
                            RcurY = RcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Other Deduction", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Other_Deduction").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                            RgtNofDet = RgtNofDet + 1
                        End If

                        NoofDets = IIf(RgtNofDet > LftNofDet, RgtNofDet, LftNofDet)
                        CurY = IIf(RcurY > LcurY, RcurY, LcurY)

                        For I = NoofDets + 1 To NoofItems_PerPage
                            CurY = CurY + TxtHgt
                        Next

                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(7) = CurY
                        nprntot = Val(prn_DetDt.Rows(prn_DetIndx).Item("Earning").ToString)
                        nOTsal = Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Salary").ToString)
                        Common_Procedures.Print_To_PrintDocument(e, "Total Earnings ", LMargin + 10, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString + Val(nprntot) + Val(nOTsal))), LMargin + ClArr(1) + 50, CurY, 1, 0, p2font) 'LMargin + ClArr(1) - 15, CurY, 0, 0, p2font)
                        nTotAdv = Val(prn_DetDt.Rows(prn_DetIndx).Item("Minus_Advance").ToString)
                        nOTEsi = Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_ESI").ToString)
                        Common_Procedures.Print_To_PrintDocument(e, "Total Deductions", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Deduction").ToString) - Val(prn_DetDt.Rows(prn_DetIndx).Item("PF_Credit_Amount").ToString) + Val(nTotAdv) + Val(nOTEsi)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, CurY, 1, 0, p2font)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Net_Pay").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, CurY, 1, 0, p3font)
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(8) = CurY


                        Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                        prn_DetIndx = prn_DetIndx + 1

                        If prn_DetIndx <= prn_DetDt.Rows.Count - 1 Then
                            e.HasMorePages = True
                            Return
                        End If

                    End If


                End If



            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False


    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim p2Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1, S1, W1 As Single
        Dim I As Integer
        Dim ItmNm1 As String, ItmNm2 As String

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        p2Font = New Font("Calibri", 8, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 5
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt - 5
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        If Val(prn_HdDt.Rows(0).Item("Month_IdNo").ToString) >= 4 Then
            Common_Procedures.Print_To_PrintDocument(e, "Wage Slip (With form 25-B) for the month of " & prn_HdDt.Rows(0).Item("Month_Name").ToString & " - " & Trim(Year(Common_Procedures.Company_FromDate)), LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Wage Slip (With form 25-B) for the month of " & prn_HdDt.Rows(0).Item("Month_Name").ToString & " - " & Trim(Year(Common_Procedures.Company_ToDate)), LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        CurY = CurY + TxtHgt - 3
        Common_Procedures.Print_To_PrintDocument(e, "FORM No.VI (See Sub Rule7) FORM NO.15(Prescibed under Rule 87 and 88)", LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            'CurY = CurY + TxtHgt - 5
            p1Font = New Font("Baamini", 9, FontStyle.Regular)
            pFont = New Font("Calibri ", 7, FontStyle.Regular)
            'Common_Procedures.Print_To_PrintDocument(e, "chpkk; vz; ", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "LICENSE NO ", LMargin + 150, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "LICENSE NO ", LMargin + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 108.3, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Salary_No").ToString, LMargin + 115, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "rk;gs fhyk;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("From_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + 90, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Kjy;", LMargin + ClAr(1) + 160, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("To_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + 210, CurY, 0, 0, pFont)
            'e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY - 5, LMargin + ClAr(1) - 20, CurY - 5)
            LnAr(11) = CurY

            'Common_Procedures.Print_To_PrintDocument(e, "rk;gs urPJ", LMargin + 360, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt



            'Common_Procedures.Print_To_PrintDocument(e, "Nlhf;fd; vz;", LMargin + 10, CurY, 0, 0, p1Font)
            ' Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Token No ", LMargin + 150, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Token No ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) - 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("Card_No").ToString, LMargin + 100, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pf_No").ToString) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "PF Number  ", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(0).Item("Pf_No").ToString, LMargin + ClAr(1) + ClAr(2) + 130, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "je;ij ngah;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, ": " & prn_DetDt.Rows(prn_DetIndx).Item("Father_Husband").ToString, LMargin + ClAr(1) + 110, CurY, 0, 0, pFont)

            'e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY, LMargin + ClAr(1) - 20, CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY, LMargin + 350, LnAr(11) - 5)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 20, CurY, LMargin + ClAr(1) - 20, LnAr(11) - 5)
            CurY = CurY + TxtHgt
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY - 5, PageWidth, CurY - 5)
            LnAr(12) = CurY
            'Common_Procedures.Print_To_PrintDocument(e, "njhopyhspapad; ngah;", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Employee Name :  " & prn_DetDt.Rows(prn_DetIndx).Item("Employee_Name").ToString, LMargin + 150, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "Employee Name :  " & prn_DetDt.Rows(prn_DetIndx).Item("Employee_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_MainName").ToString)
            ItmNm2 = ""
            If Len(ItmNm1) > 25 Then
                For I = 25 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 25
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If


            Common_Procedures.Print_To_PrintDocument(e, "Employee Name ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) - 110, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, " " & (ItmNm1), LMargin + 100, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("Employee_Name").ToString, LMargin + 100, CurY, 0, 0, pFont)

            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Esi_No").ToString) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ESI Number  ", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("Esi_No").ToString, LMargin + ClAr(1) + ClAr(2) + 130, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "mbg;gil rk;gsk;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Basic", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Basic_Salary").ToString), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "gzp", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Department ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) - 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("Department_Name").ToString, LMargin + 100, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Designation  " & prn_DetDt.Rows(prn_DetIndx).Item("Designation").ToString, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "gQ;rg;gb", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "DA", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("D_A").ToString), "############0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            If Trim(prn_DetDt.Rows(prn_DetIndx).Item("PAN_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Income Tax Number(PAN)  ", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("PAN_No").ToString, LMargin + ClAr(1) + ClAr(2) + 130, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "gpwe;j Njjp", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 100, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DOB :" & prn_DetDt.Rows(0).Item("Date_Birth").ToString, LMargin + 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Date Of Joining ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) - 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(0).Item("Join_Date").ToString, LMargin + 100, CurY, 0, 0, pFont)

            If Trim(prn_DetDt.Rows(prn_DetIndx).Item("UAN_NO").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Universal Account Number(UAN)  ", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("UAN_NO").ToString, LMargin + ClAr(1) + ClAr(2) + 130, CurY, 0, 0, pFont)
            End If
            ' uan no and pan no need to add



            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "gp.vg;.vz;", LMargin + 220, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, ",jug;gbfs;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Allowances", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            'CurY = CurY + TxtHgt
            Dim Rndof As Single = 0

            'Sary = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Basic_Salary").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("D_A").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString), "##########0")
            'TotSalry = Format(Val(Sary), "##########0")
            'TotSalry = Common_Procedures.Currency_Format(Val(TotSalry))

            ' Rndof = Format(Val(CSng(TotSalry)) - Val(Sary), "#########0.00")


            'Common_Procedures.Print_To_PrintDocument(e, "Nrh;e;j Njjp", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 100, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DOJ : " & prn_DetDt.Rows(prn_DetIndx).Item("Join_Date").ToString, LMargin + 110, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ",.v];.I.vz;", LMargin + 220, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "ESI NO : " & prn_DetDt.Rows(prn_DetIndx).Item("Esi_No").ToString, LMargin + 300, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "tl;lkhf;fy;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Roundoff", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Rndof, PageWidth - 10, CurY, 1, 0, pFont)


            'CurY = CurY + TxtHgt

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 210, CurY, PageWidth, CurY)

            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(Sary), "###########0.00"), PageWidth - 10, CurY + 5, 1, 0, pFont)

            'CurY = CurY + TxtHgt + 5
            'Common_Procedures.Print_To_PrintDocument(e, "thuj;jpd; Ntiy nra;j ehl;fs;", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_HdDt.Rows(0).Item("Total_Days").ToString), "###########"), LMargin + 210, CurY, 0, 0, pFont)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, PageWidth, CurY)
            'LnAr(13) = CurY
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(12) - 5)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 210, CurY, LMargin + ClAr(1) + 210, LnAr(12) - 5)
            'CurY = CurY + TxtHgt - 8
            'Common_Procedures.Print_To_PrintDocument(e, "Total Days Worked    ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "E.S.I.Days :  " & Format(Val(prn_HdDt.Rows(0).Item("Total_Days").ToString), "############0"), LMargin + 200, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            Common_Procedures.Print_To_PrintDocument(e, "Attendance Details", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Value", LMargin + ClAr(1) + 20, CurY, 1, 0, p2Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 40, LnAr(3), LMargin + ClAr(1) + ClAr(2) - 40, LnAr(2))

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "<l;ba rk;gsk;", LMargin, CurY, 2, ClAr(1), p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "gpbj;jq;fs;", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)



            CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 30, PageWidth, ClAr(1))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 30, LnAr(5), LMargin + ClAr(1) - 30, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - ClAr(1) - ClAr(2) + 10, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub


    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)

        Dim i As Integer = 0
        Dim Cmp_Name As String
        Dim p1Font As Font

        'Dim p2Font As Font
        'Dim p3Font As Font

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        CurY = CurY + TxtHgt - 10

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 40, LnAr(5), LMargin + ClAr(1) - 40, LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 40, LnAr(5), LMargin + ClAr(1) + ClAr(2) - 40, LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + 450, LnAr(5), LMargin + 450, LnAr(6))


        CurY = CurY + TxtHgt - 8


        'CurY = CurY + TxtHgt - 10
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(6) = CurY

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(6) = CurY

        p1Font = New Font("Calibri", 9, FontStyle.Regular)

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        pFont = New Font("Baamini", 9, FontStyle.Regular)
        CurY = CurY + TxtHgt - 6
        Common_Procedures.Print_To_PrintDocument(e, "Employee Signature", LMargin + 10, CurY, 0, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "Salary Paid on :", LMargin + 300, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
    End Sub
    Private Sub btn_Close_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PrintEmployee.Click
        pnl_Back.Enabled = True
        pnl_PrintEmployee_Details.Visible = False
    End Sub
    Private Sub btn_Cancel_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintEmployee.Click
        btn_Close_PrintRange_Click(sender, e)
    End Sub

    Private Sub btn_Print_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Employee.Click
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim I As Integer = 0
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0

        Try

            'If Trim(cbo_Employee.Text) = "" Then
            '    MessageBox.Show("Invalid Employee Name", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    cbo_Employee.Focus()
            '    Exit Sub
            'End If



            btn_Close_PrintRange_Click(sender, e)

            printing_Salary()

        Catch ex As Exception
            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT PRINT...")

        Finally
            dt1.Dispose()
            da1.Dispose()

        End Try

    End Sub

    Private Sub cbo_Employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Employee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Employee.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Employee, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

    End Sub



    Private Sub cbo_Employee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Employee.KeyPress

        Dim Emp_Id As Integer = 0
        Dim Emp_Id1 As Integer = 0
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Employee, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Print_Details

                For i = 0 To .RowCount - 1
                    Emp_Id = Common_Procedures.Employee_NameToIdNo(con, cbo_Employee.Text)
                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Emp_Id1 = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)
                        If Val(Emp_Id) = Val(Emp_Id1) Then

                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(0)
                            End If
                            Exit Sub
                        End If

                    End If
                Next

            End With

        End If

    End Sub

    Private Sub printEmployee_Selection()

        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim empidno As Integer = 0

        dgv_Print_Details.Rows.Clear()

        For i = 0 To dgv_Details.RowCount - 1

            n = dgv_Print_Details.Rows.Add()
            SNo = SNo + 1
            dgv_Print_Details.Rows(n).Cells(0).Value = Val(SNo)
            dgv_Print_Details.Rows(n).Cells(1).Value = dgv_Details.Rows(i).Cells(1).Value
            dgv_Print_Details.Rows(n).Cells(2).Value = "1"

            'Common_Procedures.Employee_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
            empidno = 0
            dgv_Print_Details.Rows(n).Cells(3).Value = empidno

        Next

    End Sub

    Private Sub btn_Print_Select_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Select.Click
        If dgv_Print_Details.Rows.Count > 0 Then
            For i = 0 To dgv_Print_Details.Rows.Count - 1
                dgv_Print_Details.Rows(i).Cells(2).Value = 1
            Next
        End If
    End Sub

    Private Sub btn_Print_Deselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Deselect.Click
        If dgv_Print_Details.Rows.Count > 0 Then
            For i = 0 To dgv_Print_Details.Rows.Count - 1
                dgv_Print_Details.Rows(i).Cells(2).Value = ""
            Next
        End If
    End Sub

    Private Sub dgv_Print_Details_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Print_Details.CellClick
        Select_Print(e.RowIndex)
    End Sub


    'End Sub

    Private Sub Select_Print(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Print_Details

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(2).Value = (Val(.Rows(RwIndx).Cells(2).Value) + 1) Mod 2
                If Val(.Rows(RwIndx).Cells(2).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next


                Else
                    .Rows(RwIndx).Cells(2).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


            End If

        End With

    End Sub

    Private Sub dgv_Print_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Print_Details.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Print_Details.CurrentCell.RowIndex >= 0 Then

                n = dgv_Print_Details.CurrentCell.RowIndex

                Select_Print(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String = ""
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim p1Font As Font
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single, LcurY As Single, RcurY As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim p2font As Font
        Dim p3font As Font
        Dim nprntot As Single
        Dim nOTsal As Single, nTotAdv As Single, nOTEsi As Single



        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 4X6", 400, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.Landscape = True



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15
            .Right = 20
            .Top = 20 ' 20
            .Bottom = 10
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 8, FontStyle.Regular)

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
                PageWidth = .Height - TMargin - 25
                PageHeight = .Width - RMargin - 10
            End With

            'With PrintDocument1.DefaultPageSettings.Margins
            '    .Left = 15
            '    .Right = TMargin
            '    .Top = 40 ' 20
            '    .Bottom = 10
            '    LMargin = .Left
            '    RMargin = .Right
            '    TMargin = .Top
            '    BMargin = .Bottom
            'End With
        End If
        NoofItems_PerPage = 1

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 200 : ClArr(2) = 95 : ClArr(3) = 100
        'ClArr(2) = PageWidth - (LMargin + ClArr(1))
        ClArr(4) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3))

        'TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        TxtHgt = 15.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntryCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            'CurY = CurY + TxtHgt

                            'Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, p1Font)

                            NoofDets = NoofDets + 1

                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1




                        p2font = New Font("Calibri", 8, FontStyle.Bold)
                        p3font = New Font("Calibri", 8, FontStyle.Bold)
                        CurY = CurY + TxtHgt

                        ''SNo = SNo + 1
                        pFont = New Font("Baamini", 9, FontStyle.Regular)
                        p1Font = New Font("Calibri", 7, FontStyle.Regular)
                        'CurY = CurY + TxtHgt

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Days").ToString) > 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, "Payable Shifts/Days :", LMargin + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Days").ToString), "#######0.00") & " Shifts/Days", LMargin + ClArr(1) + 50, CurY, 1, 0, p2font)

                            Dim Sal_Per_Day As String = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Salary_Days").ToString), "#######0.00")

                            If prn_DetDt.Rows(prn_DetIndx).Item("Salary_Days") <> prn_DetDt.Rows(prn_DetIndx).Item("Actual_Salary") Then
                                Sal_Per_Day = Sal_Per_Day + "( / Month : " + Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Actual_Salary").ToString), "#######0.00") & ")"
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, "Salary / Day (Month) Rs. :", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Sal_Per_Day, PageWidth - 10, CurY, 1, 0, p2font)

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Attendance_Days").ToString) > 0 Then
                            CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Actual Days Present :", LMargin + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Attendance_Days").ToString), "#######0.00") & " Shifts/Days", LMargin + ClArr(1) + 50, CurY, 1, 0, p1Font)
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Attendance_On_W_Off_FH").ToString) > 0 Then
                            CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Attendance On WO/PH/LH", LMargin + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Attendance_On_W_Off_FH").ToString), "#######0.00") & " Shifts/Days", LMargin + ClArr(1) + 50, CurY, 1, 0, p2font)
                        End If

                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(6) = CurY

                        Common_Procedures.Print_To_PrintDocument(e, "Earnings", LMargin + 10, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + ClArr(1) - 15, CurY, 0, 0, p2font)

                        Common_Procedures.Print_To_PrintDocument(e, "Deductions", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 90, CurY, 0, 0, p2font)


                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(7) = CurY

                        LcurY = CurY + 3
                        Common_Procedures.Print_To_PrintDocument(e, "Basic Pay", LMargin + 10, LcurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Earning").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p2font)
                        LcurY = LcurY + 12

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("H_R_A").ToString) > 0 Then

                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "HRA ", LMargin + 10, LcurY, 0, 0, p1Font)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("H_R_A").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)

                            LcurY = LcurY + 12

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Incentive_Amount").ToString) > 0 Then

                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Incentive Amount ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Incentive_Amount").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)

                            LcurY = LcurY + 12

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Conveyance").ToString) > 0 Then

                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Conveyance ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Conveyance").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)

                            LcurY = LcurY + 12

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("washing").ToString) > 0 Then

                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Washing ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("washing").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)

                            LcurY = LcurY + 12

                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Entertainment").ToString) > 0 Then
                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Entertainment ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Entertainment").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)

                            LcurY = LcurY + 12

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Salary").ToString) > 0 Then

                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Over Time Salary ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Salary").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)

                            LcurY = LcurY + 12

                        End If


                        '-------------------------

                        RcurY = CurY + 3
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Mess").ToString) > 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, "Mess", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("mess").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)

                        End If

                        'Common_Procedures.Print_To_PrintDocument(e, "Miniumum Wages Earned in the Week", LMargin + 10, CurY, 0, 0, p1Font)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Medical").ToString) > 0 Then

                            'CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Medical", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Medical").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)

                            RcurY = RcurY + 12

                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("SALARY_OT_ESI").ToString) > 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, "ESI 1.75% & OT ESI", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("SALARY_OT_ESI").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)

                            RcurY = RcurY + 12

                        End If



                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("P_F").ToString) > 0 Then

                            'CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "P F 12%", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("P_F").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)

                            RcurY = RcurY + 12

                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Store").ToString) > 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, "Store", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Store").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)

                            RcurY = RcurY + 12

                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Salary_Advance").ToString) > 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, "Salary Advance", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Salary_Advance").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                            nTotAdv = nTotAdv + Val(prn_DetDt.Rows(prn_DetIndx).Item("Salary_Advance").ToString)

                            RcurY = RcurY + 12

                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Late_Hours_Salary").ToString) > 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, "Permission Leave / Late Hours Deduction ", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Late_Hours_Salary").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)

                            RcurY = RcurY + 12

                        End If


                        If LcurY > RcurY Then
                            CurY = TxtHgt + LcurY
                        Else
                            CurY = TxtHgt + RcurY
                        End If


                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        LnAr(4) = CurY

                        nprntot = Val(prn_DetDt.Rows(prn_DetIndx).Item("Earning").ToString)
                        nOTsal = Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Salary").ToString)

                        Common_Procedures.Print_To_PrintDocument(e, "Total Earnings ", LMargin + 10, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString + Val(nprntot) + Val(nOTsal))), LMargin + ClArr(1) + 50, CurY, 1, 0, p2font) 'LMargin + ClArr(1) - 15, CurY, 0, 0, p2font)

                        nTotAdv = Val(prn_DetDt.Rows(prn_DetIndx).Item("Salary_Advance").ToString)
                        nOTEsi = Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_ESI").ToString)

                        Common_Procedures.Print_To_PrintDocument(e, "Total Deductions", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Deduction").ToString + Val(nTotAdv) + Val(nOTEsi))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, CurY, 1, 0, p2font)

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Net_Pay").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, CurY, 1, 0, p3font)
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(8) = CurY


                        CurY = CurY + TxtHgt


                        NoofDets = NoofDets + 1


                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)



            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False


    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        Dim p1Font As Font
        Dim p2Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1, S1, W1 As Single
        Dim Sary As Single, TotSalry As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim I As Integer

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        p2Font = New Font("Calibri", 8, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            'CurY = CurY + TxtHgt - 5
            p1Font = New Font("Baamini", 9, FontStyle.Regular)
            pFont = New Font("Calibri ", 7, FontStyle.Regular)

            'Common_Procedures.Print_To_PrintDocument(e, "chpkk; vz; ", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "LICENSE NO ", LMargin + 150, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "LICENSE NO ", LMargin + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 108.3, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Salary_No").ToString, LMargin + 115, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "rk;gs fhyk;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("From_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + 90, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Kjy;", LMargin + ClAr(1) + 160, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("To_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + 210, CurY, 0, 0, pFont)
            'e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY - 5, LMargin + ClAr(1) - 20, CurY - 5)
            LnAr(11) = CurY

            'Common_Procedures.Print_To_PrintDocument(e, "rk;gs urPJ", LMargin + 360, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt



            'Common_Procedures.Print_To_PrintDocument(e, "Nlhf;fd; vz;", LMargin + 10, CurY, 0, 0, p1Font)
            ' Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Token No ", LMargin + 150, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Token No ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) - 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("Card_No").ToString, LMargin + 100, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pf_No").ToString) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "PF Number  ", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(0).Item("Pf_No").ToString, LMargin + ClAr(1) + ClAr(2) + 130, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "je;ij ngah;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, ": " & prn_DetDt.Rows(prn_DetIndx).Item("Father_Husband").ToString, LMargin + ClAr(1) + 110, CurY, 0, 0, pFont)

            'e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY, LMargin + ClAr(1) - 20, CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin + 350, CurY, LMargin + 350, LnAr(11) - 5)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 20, CurY, LMargin + ClAr(1) - 20, LnAr(11) - 5)
            CurY = CurY + TxtHgt
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY - 5, PageWidth, CurY - 5)
            LnAr(12) = CurY
            'Common_Procedures.Print_To_PrintDocument(e, "njhopyhspapad; ngah;", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Employee Name :  " & prn_DetDt.Rows(prn_DetIndx).Item("Employee_Name").ToString, LMargin + 150, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "Employee Name :  " & prn_DetDt.Rows(prn_DetIndx).Item("Employee_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_MainName").ToString)
            ItmNm2 = ""
            If Len(ItmNm1) > 25 Then
                For I = 25 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 25
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If


            Common_Procedures.Print_To_PrintDocument(e, "Employee Name ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) - 110, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, " " & (ItmNm1), LMargin + 100, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("Employee_Name").ToString, LMargin + 100, CurY, 0, 0, pFont)

            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Esi_No").ToString) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ESI Number  ", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("Esi_No").ToString, LMargin + ClAr(1) + ClAr(2) + 130, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "mbg;gil rk;gsk;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Basic", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Basic_Salary").ToString), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "gzp", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 140, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Department ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) - 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("Department_Name").ToString, LMargin + 100, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Designation  " & prn_DetDt.Rows(prn_DetIndx).Item("Designation").ToString, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "gQ;rg;gb", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "DA", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("D_A").ToString), "############0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            If Trim(prn_DetDt.Rows(prn_DetIndx).Item("PAN_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Income Tax Number(PAN)  ", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("PAN_No").ToString, LMargin + ClAr(1) + ClAr(2) + 130, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "gpwe;j Njjp", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 100, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DOB :" & prn_DetDt.Rows(0).Item("Date_Birth").ToString, LMargin + 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Date Of Joining ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) - 110, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(0).Item("Join_Date").ToString, LMargin + 100, CurY, 0, 0, pFont)

            If Trim(prn_DetDt.Rows(prn_DetIndx).Item("UAN_NO").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Universal Account Number(UAN)  ", LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_DetDt.Rows(prn_DetIndx).Item("UAN_NO").ToString, LMargin + ClAr(1) + ClAr(2) + 130, CurY, 0, 0, pFont)
            End If
            ' uan no and pan no need to add



            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "gp.vg;.vz;", LMargin + 220, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, ",jug;gbfs;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Allowances", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            'CurY = CurY + TxtHgt
            Dim Rndof As Single = 0

            'Sary = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Basic_Salary").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("D_A").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString), "##########0")
            'TotSalry = Format(Val(Sary), "##########0")
            'TotSalry = Common_Procedures.Currency_Format(Val(TotSalry))

            ' Rndof = Format(Val(CSng(TotSalry)) - Val(Sary), "#########0.00")


            'Common_Procedures.Print_To_PrintDocument(e, "Nrh;e;j Njjp", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "/ ", LMargin + 100, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DOJ : " & prn_DetDt.Rows(prn_DetIndx).Item("Join_Date").ToString, LMargin + 110, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ",.v];.I.vz;", LMargin + 220, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "ESI NO : " & prn_DetDt.Rows(prn_DetIndx).Item("Esi_No").ToString, LMargin + 300, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "tl;lkhf;fy;", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Roundoff", LMargin + ClAr(1) + 130, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + 200, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Rndof, PageWidth - 10, CurY, 1, 0, pFont)


            'CurY = CurY + TxtHgt

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 210, CurY, PageWidth, CurY)

            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(Sary), "###########0.00"), PageWidth - 10, CurY + 5, 1, 0, pFont)

            'CurY = CurY + TxtHgt + 5
            'Common_Procedures.Print_To_PrintDocument(e, "thuj;jpd; Ntiy nra;j ehl;fs;", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_HdDt.Rows(0).Item("Total_Days").ToString), "###########"), LMargin + 210, CurY, 0, 0, pFont)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, PageWidth, CurY)
            'LnAr(13) = CurY
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(12) - 5)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 210, CurY, LMargin + ClAr(1) + 210, LnAr(12) - 5)
            'CurY = CurY + TxtHgt - 8
            'Common_Procedures.Print_To_PrintDocument(e, "Total Days Worked    ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "E.S.I.Days :  " & Format(Val(prn_HdDt.Rows(0).Item("Total_Days").ToString), "############0"), LMargin + 200, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            Common_Procedures.Print_To_PrintDocument(e, "Attendance Details", LMargin + 10, CurY, 0, 0, p2Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Value", LMargin + 450 - 220, CurY, 1, 0, p2Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 40, LnAr(3), LMargin + ClAr(1) + ClAr(2) - 40, LnAr(2))

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "<l;ba rk;gsk;", LMargin, CurY, 2, ClAr(1), p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "gpbj;jq;fs;", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)



            CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 30, PageWidth, ClAr(1))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 30, LnAr(5), LMargin + ClAr(1) - 30, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - ClAr(1) - ClAr(2) - 15, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)

        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim p2Font As Font

        p2Font = New Font("Calibri", 10, FontStyle.Bold)

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
            prn_DetIndx = prn_DetIndx + 1
        Next


        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        '


        'CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        CurY = CurY + TxtHgt - 10

        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Net_Salary").ToString, LMargin + 120, CurY, 0, 0, p2Font)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 30, LnAr(5), LMargin + ClAr(1) - 30, LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 40, LnAr(5), LMargin + ClAr(1) + ClAr(2) - 40, LnAr(3))

        'e.Graphics.DrawLine(Pens.Black, LMargin + 400, LnAr(5), LMargin + 400, LnAr(6))

        e.Graphics.DrawLine(Pens.Black, LMargin + 450, LnAr(5), LMargin + 450, LnAr(6))

        'Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 120, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + 460, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 530, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt - 8
        'pFont = New Font("Baamini", 9, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "nkhj;j rk;gsk; ", LMargin + 130, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "nkhj;j gpbj;jk; ", LMargin + 540, CurY, 0, 0, pFont)


        'CurY = CurY + TxtHgt - 10
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(6) = CurY





        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(6) = CurY


        p1Font = New Font("Calibri", 9, FontStyle.Regular)

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        pFont = New Font("Baamini", 9, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "njhopyhspapd; ifnahg;gk; ", LMargin + 10, CurY, 0, 0, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 200, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "rk;gs Njjp ", LMargin + 300, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 400, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt - 6
        Common_Procedures.Print_To_PrintDocument(e, "Employee Signature", LMargin + 10, CurY, 0, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "Salary Paid on :", LMargin + 300, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_Print_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub TotalNettPay()

        Dim TNetSal As Double = 0

        For I As Integer = 0 To dgv_Details.RowCount - 1

            TNetSal = TNetSal + Val(dgv_Details.Rows(I).Cells(5).Value)

        Next

        txt_Total_NetPay.Text = FormatNumber(TNetSal, 2, TriState.False, TriState.False, TriState.False)

    End Sub


    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        Try

            ProcessCmdKey = True

            If dgv1.Name <> dgv_Details.Name Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

        'On Error Resume Next

        With dgv_Details

            If keyData = Keys.Enter Then

                If .CurrentCell.ColumnIndex < .ColumnCount - 1 Then

                    For I As Integer = .CurrentCell.ColumnIndex + 1 To .ColumnCount - 1

                        If .Columns(I).ReadOnly = False And .Columns(I).Visible Then

                                .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(I)


                                Exit Function
                        End If


                    Next

                    If .CurrentRow.Index < .RowCount - 1 Then

                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                        For I As Integer = .CurrentCell.ColumnIndex + 1 To .ColumnCount - 1

                            If .Columns(I).ReadOnly = False And .Columns(I).Visible Then

                                    .Focus()
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(I)


                                    Exit Function
                            End If

                        Next

                    Else

                        Dim C As Integer = MsgBox("Do you want to Save the Salary Generated/Entered ?", vbYesNo)

                        If C = vbYes Then
                            save_record()
                        End If

                    End If

                End If

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function

            End If


        End With

        Catch ex As Exception
            Return MyBase.ProcessCmdKey(msg, keyData)
        End Try

    End Function


    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.GotFocus
        dgv1 = dgv_Details
    End Sub

    Private Sub cbo_Year_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Year.GotFocus

        Dim PrevVal As String = cbo_Year.Text
        cbo_Year.Items.Clear()

        For I As Integer = 2017 To 2099

            cbo_Year.Items.Add(I.ToString)

        Next

        cbo_Year.Text = PrevVal

    End Sub

    Private Sub cbo_Year_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Year.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Year, cbo_Category, cbo_Month, "", "", "", "")
    End Sub

    Private Sub cbo_Year_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Year.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Year, cbo_Month, "", "", "", "")
    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub pnl_Back_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Back.Paint

    End Sub
End Class