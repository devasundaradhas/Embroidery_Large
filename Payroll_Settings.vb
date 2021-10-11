Public Class Payroll_Settings

    Implements Interface_MDIActions
    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean
    Private Prec_ActCtrl As New Control

    Private vcbo_KeyDwnVal As Double
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim chkbx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime ' Color.MistyRose ' Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
            chkbx = Me.ActiveControl
            chkbx.SelectAll()

        End If



        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Nr As Integer = 0
        Dim DaySht As Single = 0
        Dim NghtSht As Single = 0

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Transport_Creation, True, New_Entry, False, False) = False Then Exit Sub


        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "Delete from PayRoll_Settings  "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into PayRoll_Settings(      Employee_IdNo                           ,      Basic_Salary                              ,              Total_Days                         ,                      Net_Pay              ,                      No_Of_Attendance_Days           ,                       From_W_off_CR          ,              From_Cl_For_Leave                   ,            From_SL_For_Leave                 ,      Festival_Holidays                           ,                      Total_Leave_Days                     ,                       No_Of_Leave                 ,        Attendance_On_W_Off_FH                ,                      Op_W_Off_CR       ,                      Add_W_Off_CR            ,        Less_W_Off_CR                              ,                      Total_W_Off_CR            ,        OP_CL_CR_Days                      ,               Less_CL_CR_Days                      ,                      Total_Cl_CR_Days            ,        OP_SL_CR_Days                             ,         Less_SL_CR_Days                     ,          TOtal_SL_CR_Days                       ,       Salary_Days                                  ,        Basic_Pay                            ,           OT_Hours                             ,           OT_Pay_Hours                                    ,                OT_Salary                          ,                          D_A                  ,              Earning                           ,         H_R_A                          ,                    Conveyance                     ,        Washing                              ,      Entertainment                                    ,                     Maintenance                   ,               Other_Addition                         ,Other_Addition2                                       ,Other_Addition3                                      ,         Incentive_Amount                        ,               Total_Addition                      ,                     Mess                 ,                           Medical            ,                  Store                      ,                   ESI                 ,                P_F                          ,                      E_P_F              ,                  Pension_Scheme                     ,                      Other_Deduction                 ,                Total_Deduction                        ,                   Attendance_Incentive           ,                     Net_Salary                  ,              Total_Advance                         ,              Minus_Advance                       ,         Balance_Advance                              ,        Salary_Advance                              ,       Salary_Pending                                ,             Net_pay_Amount                           ,    Day_For_Bonus                         ,                     Earning_For_Bonus                       ,                       OT_Minutes            ,Provision                                     ,late_Mins                                      ,Late_Hours_Salary                                         ,Add_Caption1                         ,Add_Caption2                        ,Add_Caption3                         ,Add_Caption4                        ,Add_Caption5                          ,Add_Caption6                         ,Add_Caption7                         ,Add_Caption8                         ,Week_Off_Allowance                                     ,Ded_Caption1                        ,Ded_Caption2                        ) " & _
                                                " Values ( " & IIf(chk_Employee.Checked = True, 1, 0) & " ," & IIf(chk_BasicSalary.Checked = True, 1, 0) & "," & IIf(chk_TotalDays.Checked = True, 1, 0) & " ," & IIf(chk_NetPay.Checked = True, 1, 0) & ", " & IIf(chk_AttendanceDays.Checked = True, 1, 0) & " ," & IIf(chk_Cr_Leave.Checked = True, 1, 0) & " ," & IIf(chk_Cl_Leave.Checked = True, 1, 0) & " ," & IIf(chk_Sl_Leave.Checked = True, 1, 0) & " ," & IIf(chk_Festivldays.Checked = True, 1, 0) & " , " & IIf(chk_Total_Festival_Days.Checked = True, 1, 0) & " ," & IIf(chk_No_Of_Leave.Checked = True, 1, 0) & " ," & IIf(chk_Att_Fh.Checked = True, 1, 0) & " ," & IIf(chk_Op_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Add_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Less_W_Cr.Checked = True, 1, 0) & " , " & IIf(chk_Total_W_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Op_Cl_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Less_Cl_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Total_Cl_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Op_Sl_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Less_Sl_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Total_Sl_Cr.Checked = True, 1, 0) & " ," & IIf(chk_Salary_Days.Checked = True, 1, 0) & " ," & IIf(Chk_BasicPay.Checked = True, 1, 0) & " ," & IIf(chk_Ot_Hours.Checked = True, 1, 0) & " ," & IIf(chk_Ot_Salary_Hours.Checked = True, 1, 0) & " ," & IIf(chk_Ot_Salary.Checked = True, 1, 0) & " ,        " & IIf(chk_Da.Checked = True, 1, 0) & " ," & IIf(chk_Earnings.Checked = True, 1, 0) & " ," & IIf(chk_Hra.Checked = True, 1, 0) & " ," & IIf(chk_Conveyance.Checked = True, 1, 0) & " ," & IIf(chk_Washings.Checked = True, 1, 0) & " ," & IIf(chk_Entertainments.Checked = True, 1, 0) & " ," & IIf(chk_Maintenance.Checked = True, 1, 0) & "       ," & IIf(chk_Other_Addition1.Checked = True, 1, 0) & " ," & IIf(chk_Other_Addition2.Checked = True, 1, 0) & " ," & IIf(chk_Other_Addition3.Checked = True, 1, 0) & " ," & IIf(chk_Incentive.Checked = True, 1, 0) & " ," & IIf(chk_Total_Additon.Checked = True, 1, 0) & " ," & IIf(chk_Mess.Checked = True, 1, 0) & " ," & IIf(chk_Medical.Checked = True, 1, 0) & " ," & IIf(chk_Store.Checked = True, 1, 0) & " ," & IIf(chk_ESI.Checked = True, 1, 0) & " ," & IIf(chk_Pf.Checked = True, 1, 0) & " ," & IIf(chk_Epf.Checked = True, 1, 0) & " ," & IIf(chk_Pension_Scheme.Checked = True, 1, 0) & " ," & IIf(chk_Other_Deduction.Checked = True, 1, 0) & " ," & IIf(chk_Total_Deduction.Checked = True, 1, 0) & " ," & IIf(chk_Attincentive.Checked = True, 1, 0) & " ," & IIf(chk_Net_Salary.Checked = True, 1, 0) & " ," & IIf(chk_Total_Advance.Checked = True, 1, 0) & " ," & IIf(chk_Less_Advance.Checked = True, 1, 0) & " ," & IIf(chk_BalanceAdvance.Checked = True, 1, 0) & " ," & IIf(chk_Salary_Advance.Checked = True, 1, 0) & " ," & IIf(chk_Salary_Pending.Checked = True, 1, 0) & " ," & IIf(chk_Salary_Net_Pay.Checked = True, 1, 0) & " ," & IIf(chk_DayBonus.Checked = True, 1, 0) & " ," & IIf(chk_Earning_For_Bonus.Checked = True, 1, 0) & " ," & IIf(chk_OT_Minutes.Checked = True, 1, 0) & " ," & IIf(chk_Provision.Checked = True, 1, 0) & "," & IIf(chk_LateMins.Checked = True, 1, 0) & " ," & IIf(chk_LateHoursSalary.Checked = True, 1, 0) & ", '" & Trim(txt_addCaption1.Text) & "' ,'" & Trim(txt_addCaption2.Text) & "','" & Trim(txt_addCaption3.Text) & "','" & Trim(txt_addCaption4.Text) & "','" & Trim(txt_addCaption5.Text) & "' ,'" & Trim(txt_addCaption6.Text) & "' ,'" & Trim(txt_addCaption7.Text) & "' ,'" & Trim(txt_addCaption8.Text) & "' ," & IIf(chk_WeekOff_Allowance.Checked = True, 1, 0) & ",'" & Trim(txt_dedCaption1.Text) & "','" & Trim(txt_dedCaption2.Text) & "' )"
            cmd.ExecuteNonQuery()


            trans.Commit()

            Display_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)


        Catch ex As Exception
            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally



        End Try
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub Settings_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Payroll_Settings_HelpButtonClicked(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.HelpButtonClicked

    End Sub

    Private Sub Settings_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'AddHandler chk_Employee.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_BasicSalary.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_TotalDays.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_NetPay.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_AttendanceDays.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Cr_Leave.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Cl_Leave.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Sl_Leave.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Festivldays.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Total_Festival_Days.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_No_Of_Leave.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Att_Fh.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Op_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Add_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Less_W_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Total_W_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Op_Cl_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Less_Cl_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Total_Cl_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Op_Sl_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Less_Sl_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Total_Sl_Cr.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Salary_Days.GotFocus, AddressOf ControlGotFocus
        'AddHandler Chk_BasicPay.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Ot_Hours.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Ot_Salary.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Ot_Salary_Hours.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Da.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Earnings.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Hra.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Conveyance.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Washings.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Entertainments.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Maintenance.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Other_Addition1.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Other_Addition2.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Other_Addition3.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Incentive.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Total_Additon.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Mess.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Medical.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Store.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_ESI.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Pf.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Epf.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Pension_Scheme.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Other_Deduction.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Total_Deduction.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Attincentive.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Net_Salary.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Total_Advance.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Less_Advance.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_BalanceAdvance.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Salary_Advance.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Salary_Pending.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_NetPay.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_DayBonus.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Earning_For_Bonus.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_OT_Minutes.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_Provision.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_LateMins.GotFocus, AddressOf ControlGotFocus
        'AddHandler chk_LateHoursSalary.GotFocus, AddressOf ControlGotFocus


        'AddHandler chk_Employee.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_BasicSalary.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_TotalDays.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_NetPay.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_AttendanceDays.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Cr_Leave.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Cl_Leave.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Sl_Leave.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Festivldays.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Total_Festival_Days.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_No_Of_Leave.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Att_Fh.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Op_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Add_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Less_W_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Total_W_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Op_Cl_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Less_Cl_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Total_Cl_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Op_Sl_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Less_Sl_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Total_Sl_Cr.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Salary_Days.LostFocus, AddressOf ControlLostFocus
        'AddHandler Chk_BasicPay.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Ot_Hours.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Ot_Salary.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Ot_Salary_Hours.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Da.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Earnings.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Hra.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Conveyance.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Washings.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Entertainments.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Maintenance.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Other_Addition1.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Other_Addition2.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Other_Addition3.LostFocus, AddressOf ControlLostFocus

        'AddHandler chk_Incentive.LostFocus, AddressOf ControlLostFocus

        'AddHandler chk_Total_Additon.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Mess.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Medical.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Store.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_ESI.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Pf.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Epf.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Pension_Scheme.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Other_Deduction.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Total_Deduction.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Attincentive.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Net_Salary.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Total_Advance.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Less_Advance.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_BalanceAdvance.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Salary_Advance.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Salary_Pending.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_NetPay.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_DayBonus.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Earning_For_Bonus.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_OT_Minutes.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_Provision.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_LateMins.LostFocus, AddressOf ControlLostFocus
        'AddHandler chk_LateHoursSalary.LostFocus, AddressOf ControlLostFocus




        AddHandler chk_Employee.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_BasicSalary.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_TotalDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_NetPay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_AttendanceDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Cr_Leave.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Cl_Leave.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Sl_Leave.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Festivldays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Total_Festival_Days.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_No_Of_Leave.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Att_Fh.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Op_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Add_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Less_W_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Total_W_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Op_Cl_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Less_Cl_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Total_Cl_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Op_Sl_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Less_Sl_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Total_Sl_Cr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Salary_Days.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Chk_BasicPay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Ot_Hours.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Ot_Salary.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Ot_Salary_Hours.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Da.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Earnings.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Hra.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Conveyance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Washings.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Entertainments.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Maintenance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Other_Addition1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Other_Addition2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Other_Addition3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Incentive.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Total_Additon.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Mess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Medical.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Store.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_ESI.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Pf.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Epf.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Pension_Scheme.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Other_Deduction.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Total_Deduction.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Attincentive.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Net_Salary.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Total_Advance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Less_Advance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_BalanceAdvance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Salary_Advance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Salary_Pending.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_NetPay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_DayBonus.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Earning_For_Bonus.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_OT_Minutes.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Provision.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_LateMins.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_LateHoursSalary.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler chk_Employee.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_BasicSalary.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_TotalDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_NetPay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_AttendanceDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Cr_Leave.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Cl_Leave.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Sl_Leave.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Festivldays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Total_Festival_Days.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_No_Of_Leave.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Att_Fh.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Op_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Add_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Less_W_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Total_W_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Op_Cl_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Less_Cl_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Total_Cl_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Op_Sl_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Less_Sl_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Total_Sl_Cr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Salary_Days.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Chk_BasicPay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Ot_Hours.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Ot_Salary.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Ot_Salary_Hours.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Da.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Earnings.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Hra.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Conveyance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Washings.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Entertainments.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Maintenance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Other_Addition1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Other_Addition2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Other_Addition3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Incentive.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Total_Additon.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Mess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Medical.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Store.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_ESI.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Pf.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Epf.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Pension_Scheme.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Other_Deduction.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Total_Deduction.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Attincentive.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Net_Salary.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Total_Advance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Less_Advance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_BalanceAdvance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Salary_Advance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Salary_Pending.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_NetPay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_DayBonus.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Earning_For_Bonus.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_OT_Minutes.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Provision.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_LateMins.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_LateHoursSalary.KeyPress, AddressOf TextBoxControlKeyPress

        con.Open()
        Display_record()


        Dim tp As System.Windows.Forms.ToolTip

        tp = New System.Windows.Forms.ToolTip()

        tp.SetToolTip(Me.txt_addCaption1, "Other Additions Title")
        tp.SetToolTip(Me.txt_addCaption2, "Other Additions Title")
        tp.SetToolTip(Me.txt_addCaption3, "Other Additions Title")
        tp.SetToolTip(Me.txt_addCaption4, "Other Additions Title")
        tp.SetToolTip(Me.txt_addCaption5, "Other Additions Title")
        tp.SetToolTip(Me.txt_addCaption6, "Other Additions Title")
        tp.SetToolTip(Me.txt_addCaption7, "Other Additions Title")
        tp.SetToolTip(Me.txt_addCaption8, "Other Additions Title")

        tp.SetToolTip(Me.txt_dedCaption1, "Other Deduction Title")
        tp.SetToolTip(Me.txt_dedCaption2, "Other Deduction Title")


        If Common_Procedures.settings.WeekOff_Allowance_Fixed_Status = 1 Then

            txt_addCaption7.Text = "SNACKS ALLOW"


            txt_addCaption7.Enabled = False
        End If



    End Sub

    Public Sub Display_record()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Sl_No As Integer = 0
        Dim n As Integer = 0

        Try

            da = New SqlClient.SqlDataAdapter("select * from PayRoll_Settings order by Auto_SlNo", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                lbl_RefNo.Text = dt.Rows(0).Item("Auto_SlNo").ToString

                chk_Employee.Checked = IIf(dt.Rows(0).Item("Employee_IdNo").ToString = 1, True, False)
                chk_BasicSalary.Checked = IIf(dt.Rows(0).Item("Basic_Salary").ToString = 1, True, False)
                chk_TotalDays.Checked = IIf(dt.Rows(0).Item("Total_Days").ToString = 1, True, False)
                chk_NetPay.Checked = IIf(dt.Rows(0).Item("Net_Pay").ToString = 1, True, False)
                chk_AttendanceDays.Checked = IIf(dt.Rows(0).Item("No_Of_Attendance_Days").ToString = 1, True, False)
                chk_Cr_Leave.Checked = IIf(dt.Rows(0).Item("From_W_Off_CR").ToString = 1, True, False)
                chk_Cl_Leave.Checked = IIf(dt.Rows(0).Item("From_CL_For_Leave").ToString = 1, True, False)
                chk_Sl_Leave.Checked = IIf(dt.Rows(0).Item("From_SL_For_Leave").ToString = 1, True, False)
                chk_Festivldays.Checked = IIf(dt.Rows(0).Item("Festival_Holidays").ToString = 1, True, False)
                chk_Total_Festival_Days.Checked = IIf(dt.Rows(0).Item("Total_Leave_Days").ToString = 1, True, False)
                chk_No_Of_Leave.Checked = IIf(dt.Rows(0).Item("No_Of_Leave").ToString = 1, True, False)
                chk_Att_Fh.Checked = IIf(dt.Rows(0).Item("Attendance_On_W_Off_Fh").ToString = 1, True, False)
                chk_Op_Cr.Checked = IIf(dt.Rows(0).Item("Op_W_Off_CR").ToString = 1, True, False)
                chk_Add_Cr.Checked = IIf(dt.Rows(0).Item("Add_W_Off_CR").ToString = 1, True, False)
                chk_Less_W_Cr.Checked = IIf(dt.Rows(0).Item("Less_W_Off_CR").ToString = 1, True, False)
                chk_Total_W_Cr.Checked = IIf(dt.Rows(0).Item("Total_W_Off_CR").ToString = 1, True, False)
                chk_Op_Cl_Cr.Checked = IIf(dt.Rows(0).Item("Op_CL_CR_Days").ToString = 1, True, False)
                chk_Less_Cl_Cr.Checked = IIf(dt.Rows(0).Item("Less_CL_CR_Days").ToString = 1, True, False)
                chk_Total_Cl_Cr.Checked = IIf(dt.Rows(0).Item("Total_Cl_CR_Days").ToString = 1, True, False)
                chk_Op_Sl_Cr.Checked = IIf(dt.Rows(0).Item("Op_SL_CR_DAys").ToString = 1, True, False)
                chk_Less_Sl_Cr.Checked = IIf(dt.Rows(0).Item("Less_SL_CR_DAys").ToString = 1, True, False)
                chk_Total_Sl_Cr.Checked = IIf(dt.Rows(0).Item("Total_SL_CR_DAys").ToString = 1, True, False)
                chk_Salary_Days.Checked = IIf(dt.Rows(0).Item("Salary_Days").ToString = 1, True, False)
                Chk_BasicPay.Checked = IIf(dt.Rows(0).Item("Basic_Pay").ToString = 1, True, False)
                chk_Ot_Hours.Checked = IIf(dt.Rows(0).Item("OT_Hours").ToString = 1, True, False)
                chk_Ot_Salary_Hours.Checked = IIf(dt.Rows(0).Item("Ot_Pay_Hours").ToString = 1, True, False)
                chk_Ot_Salary.Checked = IIf(dt.Rows(0).Item("OT_Salary").ToString = 1, True, False)
                chk_Da.Checked = IIf(dt.Rows(0).Item("D_A").ToString = 1, True, False)
                chk_Earnings.Checked = IIf(dt.Rows(0).Item("Earning").ToString = 1, True, False)
                chk_Hra.Checked = IIf(dt.Rows(0).Item("H_R_A").ToString = 1, True, False)
                chk_Conveyance.Checked = IIf(dt.Rows(0).Item("Conveyance").ToString = 1, True, False)
                chk_Washings.Checked = IIf(dt.Rows(0).Item("Washing").ToString = 1, True, False)
                chk_Entertainments.Checked = IIf(dt.Rows(0).Item("Entertainment").ToString = 1, True, False)
                chk_Maintenance.Checked = IIf(dt.Rows(0).Item("Maintenance").ToString = 1, True, False)
                chk_Other_Addition1.Checked = IIf(dt.Rows(0).Item("Other_Addition").ToString = 1, True, False)
                chk_Other_Addition2.Checked = IIf(dt.Rows(0).Item("Other_Addition2").ToString = 1, True, False)
                chk_Other_Addition3.Checked = IIf(dt.Rows(0).Item("Other_Addition3").ToString = 1, True, False)
                chk_Incentive.Checked = IIf(dt.Rows(0).Item("Incentive_Amount").ToString = 1, True, False)
                chk_WeekOff_Allowance.Checked = IIf(dt.Rows(0).Item("Week_Off_Allowance").ToString = 1, True, False)
                chk_Total_Additon.Checked = IIf(dt.Rows(0).Item("Total_Addition").ToString = 1, True, False)
                chk_Mess.Checked = IIf(dt.Rows(0).Item("Mess").ToString = 1, True, False)
                chk_Medical.Checked = IIf(dt.Rows(0).Item("Medical").ToString = 1, True, False)
                chk_Store.Checked = IIf(dt.Rows(0).Item("Store").ToString = 1, True, False)
                chk_ESI.Checked = IIf(dt.Rows(0).Item("ESI").ToString = 1, True, False)
                chk_Pf.Checked = IIf(dt.Rows(0).Item("P_F").ToString = 1, True, False)
                chk_Epf.Checked = IIf(dt.Rows(0).Item("E_P_F").ToString = 1, True, False)
                chk_Pension_Scheme.Checked = IIf(dt.Rows(0).Item("Pension_Scheme").ToString = 1, True, False)
                chk_Other_Deduction.Checked = IIf(dt.Rows(0).Item("Other_Deduction").ToString = 1, True, False)
                chk_Total_Deduction.Checked = IIf(dt.Rows(0).Item("Total_Deduction").ToString = 1, True, False)
                chk_Attincentive.Checked = IIf(dt.Rows(0).Item("Attendance_Incentive").ToString = 1, True, False)
                chk_Net_Salary.Checked = IIf(dt.Rows(0).Item("Net_Salary").ToString = 1, True, False)
                chk_Total_Advance.Checked = IIf(dt.Rows(0).Item("Total_Advance").ToString = 1, True, False)
                chk_Less_Advance.Checked = IIf(dt.Rows(0).Item("Minus_Advance").ToString = 1, True, False)
                chk_BalanceAdvance.Checked = IIf(dt.Rows(0).Item("Balance_Advance").ToString = 1, True, False)
                chk_Salary_Advance.Checked = IIf(dt.Rows(0).Item("Salary_Advance").ToString = 1, True, False)
                chk_Salary_Pending.Checked = IIf(dt.Rows(0).Item("Salary_Pending").ToString = 1, True, False)
                chk_Salary_Net_Pay.Checked = IIf(dt.Rows(0).Item("Net_Pay_Amount").ToString = 1, True, False)
                chk_DayBonus.Checked = IIf(dt.Rows(0).Item("Day_For_Bonus").ToString = 1, True, False)
                chk_Earning_For_Bonus.Checked = IIf(dt.Rows(0).Item("Earning_For_Bonus").ToString = 1, True, False)
                chk_OT_Minutes.Checked = IIf(dt.Rows(0).Item("OT_Minutes").ToString = 1, True, False)

                chk_Provision.Checked = IIf(dt.Rows(0).Item("Provision").ToString = 1, True, False)
                chk_LateMins.Checked = IIf(dt.Rows(0).Item("late_Mins").ToString = 1, True, False)
                chk_LateHoursSalary.Checked = IIf(dt.Rows(0).Item("Late_Hours_Salary").ToString = 1, True, False)




                txt_addCaption1.Text = (dt.Rows(0).Item("Add_Caption1").ToString)
                txt_addCaption2.Text = (dt.Rows(0).Item("Add_Caption2").ToString)
                txt_addCaption3.Text = (dt.Rows(0).Item("Add_Caption3").ToString)
                txt_addCaption4.Text = (dt.Rows(0).Item("Add_Caption4").ToString)
                txt_addCaption5.Text = (dt.Rows(0).Item("Add_Caption5").ToString)
                txt_addCaption6.Text = (dt.Rows(0).Item("Add_Caption6").ToString)
                txt_addCaption7.Text = (dt.Rows(0).Item("Add_Caption7").ToString)
                txt_addCaption8.Text = (dt.Rows(0).Item("Add_Caption8").ToString)

                txt_dedCaption1.Text = (dt.Rows(0).Item("Ded_Caption1").ToString)
                txt_dedCaption2.Text = (dt.Rows(0).Item("Ded_Caption2").ToString)
               
            End If

            dt.Dispose()
            da.Dispose()
        Catch ex As Exception
            '
        End Try

        Chk_Color()


    End Sub



    Private Sub chk_OT_Minutes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_OT_Minutes.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    'Private Sub txt_NightShift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
    '        e.Handled = True
    '    End If
    '    If Asc(e.KeyChar) = 13 Then
    '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '            save_record()
    '        End If
    '    End If
    'End Sub


    Private Sub txt_addCaption1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_addCaption1.TextChanged
        chk_Conveyance.Text = txt_addCaption1.Text
    End Sub

    Private Sub txt_addCaption2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_addCaption2.TextChanged
        chk_Washings.Text = txt_addCaption2.Text
    End Sub
    Private Sub txt_addCaption3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_addCaption3.TextChanged
        chk_Entertainments.Text = txt_addCaption3.Text
    End Sub

    Private Sub txt_addCaption4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_addCaption4.TextChanged
        chk_Maintenance.Text = txt_addCaption4.Text
    End Sub

    Private Sub txt_addCaption5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_addCaption5.TextChanged
        chk_Provision.Text = txt_addCaption5.Text
    End Sub

    Private Sub txt_addCaption6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_addCaption6.TextChanged
        chk_Other_Addition2.Text = txt_addCaption6.Text
    End Sub

    Private Sub txt_addCaption7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_addCaption7.TextChanged
        chk_Other_Addition3.Text = txt_addCaption7.Text
    End Sub

    Private Sub btn_Reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Reset.Click

        chk_Cr_Leave.Checked = False
        chk_Cl_Leave.Checked = False
        chk_Sl_Leave.Checked = False
        chk_Festivldays.Checked = False
        chk_Att_Fh.Checked = False
        chk_Op_Cr.Checked = False
        chk_Add_Cr.Checked = False
        chk_Less_W_Cr.Checked = False
        chk_Op_Cl_Cr.Checked = False
        chk_Less_Cl_Cr.Checked = False
        chk_Total_Cl_Cr.Checked = False
        chk_Op_Sl_Cr.Checked = False
        chk_Less_Sl_Cr.Checked = False
        chk_Total_Sl_Cr.Checked = False
        chk_Conveyance.Checked = False
        chk_Washings.Checked = False
        chk_Entertainments.Checked = False
        chk_Maintenance.Checked = False
        chk_Provision.Checked = False
        chk_Other_Addition2.Checked = False
        chk_Other_Addition3.Checked = False
        chk_Other_Addition1.Checked = False
        chk_Incentive.Checked = False
        chk_WeekOff_Allowance.Checked = False
        chk_Medical.Checked = False
        chk_Store.Checked = False
        chk_Other_Deduction.Checked = False
        chk_Attincentive.Checked = False
        chk_Salary_Pending.Checked = False
        chk_LateMins.Checked = False
        chk_LateHoursSalary.Checked = False
        chk_BasicSalary.Checked = False
        chk_Total_W_Cr.Checked = False


        chk_Salary_Days.Checked = True
        Chk_BasicPay.Checked = True
        chk_Ot_Hours.Checked = True
        chk_Ot_Salary_Hours.Checked = True
        chk_Ot_Salary.Checked = True
        chk_Da.Checked = True
        chk_Hra.Checked = True
        chk_Total_Additon.Checked = True
        chk_Mess.Checked = True
        chk_ESI.Checked = True
        chk_Pf.Checked = True
        chk_Epf.Checked = True
        chk_Pension_Scheme.Checked = True
        chk_Total_Deduction.Checked = True
        chk_Total_Advance.Checked = True
        chk_Less_Advance.Checked = True
        chk_BalanceAdvance.Checked = True
        chk_Salary_Advance.Checked = True
        chk_Net_Salary.Checked = True
        chk_Total_Festival_Days.Checked = True
        chk_No_Of_Leave.Checked = True
        chk_Salary_Net_Pay.Checked = True
        chk_Earnings.Checked = True

        save_record()

    End Sub


    Private Sub chk_Add_Cr_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Add_Cr.CheckedChanged, chk_Att_Fh.CheckedChanged, chk_AttendanceDays.CheckedChanged, chk_Attincentive.CheckedChanged, chk_BalanceAdvance.CheckedChanged, Chk_BasicPay.CheckedChanged, chk_BasicSalary.CheckedChanged, chk_Cl_Leave.CheckedChanged, chk_Conveyance.CheckedChanged, chk_Cr_Leave.CheckedChanged, _
      chk_Da.CheckedChanged, chk_DayBonus.CheckedChanged, chk_Earning_For_Bonus.CheckedChanged, chk_Earnings.CheckedChanged, chk_Entertainments.CheckedChanged, chk_Epf.CheckedChanged, chk_ESI.CheckedChanged, chk_Festivldays.CheckedChanged, chk_Hra.CheckedChanged, chk_Incentive.CheckedChanged, chk_LateHoursSalary.CheckedChanged, chk_LateMins.CheckedChanged, chk_Less_Advance.CheckedChanged, chk_Less_Cl_Cr.CheckedChanged, _
      chk_Less_Sl_Cr.CheckedChanged, chk_Less_W_Cr.CheckedChanged, chk_Maintenance.CheckedChanged, chk_Medical.CheckedChanged, chk_Mess.CheckedChanged, chk_Net_Salary.CheckedChanged, chk_NetPay.CheckedChanged, chk_No_Of_Leave.CheckedChanged, chk_Op_Cl_Cr.CheckedChanged, chk_Op_Cr.CheckedChanged, chk_Op_Sl_Cr.CheckedChanged, chk_Ot_Hours.CheckedChanged, chk_OT_Minutes.CheckedChanged, chk_Ot_Salary.CheckedChanged, chk_Ot_Salary_Hours.CheckedChanged, _
      chk_Other_Addition1.CheckedChanged, chk_Other_Addition2.CheckedChanged, chk_Other_Addition3.CheckedChanged, chk_Other_Deduction.CheckedChanged, chk_Pension_Scheme.CheckedChanged, chk_Pf.CheckedChanged, chk_Provision.CheckedChanged, chk_Salary_Advance.CheckedChanged, chk_Salary_Days.CheckedChanged, chk_Salary_Net_Pay.CheckedChanged, chk_Salary_Pending.CheckedChanged, chk_Sl_Leave.CheckedChanged, chk_Store.CheckedChanged, chk_Total_Additon.CheckedChanged, _
      chk_Total_Advance.CheckedChanged, chk_Total_Cl_Cr.CheckedChanged, chk_Total_Deduction.CheckedChanged, chk_Total_Festival_Days.CheckedChanged, chk_Total_Sl_Cr.CheckedChanged, chk_Total_W_Cr.CheckedChanged, chk_TotalDays.CheckedChanged, chk_Washings.CheckedChanged, chk_WeekOff_Allowance.CheckedChanged

        Dim chkbx As CheckBox

        On Error Resume Next

        chkbx = Me.ActiveControl

        If TypeOf Me.ActiveControl Is CheckBox Then

            If chkbx.Checked = True Then

                Me.ActiveControl.BackColor = Color.OrangeRed
                Me.ActiveControl.ForeColor = Color.White

            Else
                Me.ActiveControl.BackColor = Color.WhiteSmoke
                Me.ActiveControl.ForeColor = Color.Maroon

            End If

        End If



    End Sub
    Private Sub Chk_Color()

        Dim allChk As New List(Of Control)

        For Each chk As CheckBox In FindControl_to_List(allChk, Me, GetType(CheckBox))

            If chk.Checked = True Then

                chk.BackColor = Color.OrangeRed
                chk.ForeColor = Color.White
            Else
                chk.BackColor = Color.WhiteSmoke
                chk.ForeColor = Color.Maroon

            End If

        Next


    End Sub
  
    Public Shared Function FindControl_to_List(ByVal list As List(Of Control), ByVal Frm As Control, ByVal ctrlType As System.Type) As List(Of Control)

        If Frm Is Nothing Then Return list

        If Frm.GetType Is ctrlType Then

            list.Add(Frm)

        End If

        For Each Ctrls As Control In Frm.Controls

            FindControl_to_List(list, Ctrls, ctrlType)

        Next

        Return list
    End Function

    Private Sub txt_dedCaption1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_dedCaption1.TextChanged
        chk_Medical.Text = txt_dedCaption1.Text
    End Sub

    Private Sub txt_dedCaption2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_dedCaption2.TextChanged
        chk_Store.Text = txt_dedCaption2.Text
    End Sub
End Class