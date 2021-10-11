Public Class Payroll_Category_Creation

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private dgv_ActCtrlName As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Displaying As Boolean = False
    Public previlege As String

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True


        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        txt_Name.Text = ""
        msk_InTimeshift1.Text = ""
        msk_InTimeShift2.Text = ""
        msk_inTimeShift3.Text = ""
        txt_lunchMiniutes.Text = ""
        cbo_WeekOff.Text = "FIXED"
        chk_ot.Checked = False
        chk_TimeDelay.Checked = False
        cbo_AttendanceLeave.Text = "LEAVE"
        chk_Attendance_Ot.Checked = False
        chk_Attendance_Incentive.Checked = False
        msk_OutTime_Shift1.Text = ""
        msk_OutTime_Shift2.Text = ""
        msk_OutTime_Shift3.Text = ""
        cbo_Monthly_Shift.Text = "SHIFT"
        txt_OtAllowed_Minute.Text = ""
        txt_MinimumDelay.Text = ""
        Chk_FestivalHolidays.Checked = False
        txt_Incentive_Amount.Text = ""

        msk_Working_Hours_Shift1.Text = ""
        msk_Working_Hours_Shift2.Text = ""
        msk_Working_Hours_Shift3.Text = ""

        txt_NoofDaye_Monthly.Text = ""


        chk_WeekOffCredit.Checked = False
        chk_WeekOff_Allowance.Checked = False
        txt_LessMinuteDelay.Text = ""

        chk_Festival_Holiday_OtSalary.Checked = False
        chk_Production.Checked = False
        txt_Incentive_Amount_Days.Text = ""

        chk_LeaveSalaryLess.Checked = True

        txt_AttnIncenRange1_FromDays.Text = ""
        txt_AttnIncenRange1_ToDays.Text = ""
        txt_AttnIncenRange2_FromDays.Text = ""
        txt_AttnIncenRange2_ToDays.Text = ""

        chk_CL.Checked = False
        chk_SL.Checked = False
        cbo_CLArrearForMonth.Text = "SALARY"
        cbo_SLArrearForMonth.Text = "SALARY"
        cbo_CLArrearForYear.Text = "SALARY"
        cbo_SLArrearForYear.Text = "SALARY"

        txt_NoofDaye_Monthly.Enabled = False
        cbo_AttendanceLeave.Enabled = False
        chk_LeaveSalaryLess.Enabled = False

        msk_Min_Time_Half_Shift_1.Text = "0"
        msk_Min_Time_Half_Shift_2.Text = "0"
        msk_Min_Time_Half_Shift_3.Text = "0"

        msk_Min_Time_One_Shift_1.Text = "0"
        msk_Min_Time_One_Shift_2.Text = "0"
        msk_Min_Time_One_Shift_3.Text = "0"

        dgv_ActCtrlName = ""

        dgv_details.Rows.Clear()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
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
        If Not IsNothing(dgv_details.CurrentCell) Then dgv_details.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)

        Displaying = True

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim SNo As Integer = 0
        Dim i As Integer = 0, n As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try
            da = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Category_Head a  where a.Category_IdNo = " & Str(Val(idno)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                lbl_IdNo.Text = dt.Rows(0).Item("Category_IdNo").ToString
                txt_Name.Text = dt.Rows(0).Item("Category_Name").ToString
                msk_InTimeshift1.Text = dt.Rows(0).Item("Shift1_In_Time").ToString
                msk_InTimeShift2.Text = dt.Rows(0).Item("Shift2_In_Time").ToString
                msk_inTimeShift3.Text = dt.Rows(0).Item("Shift3_In_Time").ToString
                txt_lunchMiniutes.Text = Val(dt.Rows(0).Item("Lunch_Minutes").ToString)
                cbo_WeekOff.Text = dt.Rows(0).Item("Fixed_Rotation").ToString
                If Val(dt.Rows(0).Item("OT_Allowed").ToString) = 1 Then
                    chk_ot.Checked = True
                End If
                If Val(dt.Rows(0).Item("Time_Delay").ToString) = 1 Then
                    chk_TimeDelay.Checked = True
                End If
                cbo_AttendanceLeave.Text = dt.Rows(0).Item("Attendance_Leave").ToString
              
                If Val(dt.Rows(0).Item("Week_Attendance_Ot").ToString) = 1 Then
                    chk_Attendance_Ot.Checked = True
                End If
                If Val(dt.Rows(0).Item("Attendance_Incentive").ToString) = 1 Then
                    chk_Attendance_Incentive.Checked = True
                End If
                msk_OutTime_Shift1.Text = dt.Rows(0).Item("Shift1_Out_Time").ToString
                msk_OutTime_Shift2.Text = dt.Rows(0).Item("Shift2_Out_Time").ToString
                msk_OutTime_Shift3.Text = dt.Rows(0).Item("Shift3_Out_Time").ToString
                cbo_Monthly_Shift.Text = dt.Rows(0).Item("Monthly_Shift").ToString
                txt_OtAllowed_Minute.Text = Val(dt.Rows(0).Item("OT_Allowed_After_Minutes").ToString)
                txt_MinimumDelay.Text = Val(dt.Rows(0).Item("Minimum_Delay").ToString)
                If Val(dt.Rows(0).Item("Festival_Holidays").ToString) = 1 Then
                    Chk_FestivalHolidays.Checked = True
                End If
                txt_Incentive_Amount.Text = Format(Val(dt.Rows(0).Item("Incentive_Amount").ToString), "########0.00")
                msk_Working_Hours_Shift1.Text = Format(Val(dt.Rows(0).Item("Shift1_Working_Hours").ToString), "#######00.00")
                msk_Working_Hours_Shift2.Text = Format(Val(dt.Rows(0).Item("Shift2_Working_Hours").ToString), "#######00.00")
                msk_Working_Hours_Shift3.Text = Format(Val(dt.Rows(0).Item("Shift3_Working_Hours").ToString), "#######00.00")
                txt_NoofDaye_Monthly.Text = Val(dt.Rows(0).Item("No_Days_Month_Wages").ToString)

                txt_AttnIncenRange1_FromDays.Text = Val(dt.Rows(0).Item("Att_Incentive_FromDays_Range1").ToString)
                txt_AttnIncenRange1_ToDays.Text = Val(dt.Rows(0).Item("Att_Incentive_ToDays_Range1").ToString)
                txt_AttnIncenRange2_FromDays.Text = Val(dt.Rows(0).Item("Att_Incentive_FromDays_Range2").ToString)
                txt_AttnIncenRange2_ToDays.Text = Val(dt.Rows(0).Item("Att_Incentive_ToDays_Range2").ToString)


                If Val(dt.Rows(0).Item("Week_Off_Credit").ToString) = 1 Then
                    chk_WeekOffCredit.Checked = True
                End If

                If Val(dt.Rows(0).Item("Week_Off_Allowance").ToString) = 1 Then
                    chk_WeekOff_Allowance.Checked = True
                End If

                txt_LessMinuteDelay.Text = Val(dt.Rows(0).Item("Less_Minute_Delay").ToString)

                If Val(dt.Rows(0).Item("Leave_Salary_Less").ToString) = 0 Then
                    chk_LeaveSalaryLess.Checked = False
                End If
                If Val(dt.Rows(0).Item("CL_Leave").ToString) = 1 Then
                    chk_CL.Checked = True
                End If
                If Val(dt.Rows(0).Item("SL_Leave").ToString) = 1 Then
                    chk_SL.Checked = True
                End If
                cbo_CLArrearForMonth.Text = Trim(dt.Rows(0).Item("CL_Arrear_Type").ToString)
                cbo_SLArrearForMonth.Text = Trim(dt.Rows(0).Item("SL_Arrear_Type").ToString)

                cbo_CLArrearForYear.Text = Trim(dt.Rows(0).Item("CL_Arrear_Type_Year").ToString)
                cbo_SLArrearForYear.Text = Trim(dt.Rows(0).Item("SL_Arrear_Type_Year").ToString)

                If Val(dt.Rows(0).Item("Festival_Holidays_OT_Salary").ToString) = 1 Then
                    chk_Festival_Holiday_OtSalary.Checked = True
                End If
                If Val(dt.Rows(0).Item("Production_Incentive").ToString) = 1 Then
                    chk_Production.Checked = True
                End If

             

                txt_Incentive_Amount_Days.Text = Format(Val(dt.Rows(0).Item("Incentive_Amount_Days").ToString), "########0.00")

                '----- ADDED BY DEVA

                If Not IsDBNull(dt.Rows(0).Item("Min_Minutes_One_Shift_1")) Then
                    msk_Min_Time_One_Shift_1.Text = dt.Rows(0).Item("Min_Minutes_One_Shift_1").ToString
                End If

                If Not IsDBNull(dt.Rows(0).Item("Min_Minutes_Half_Shift_1")) Then
                    msk_Min_Time_Half_Shift_1.Text = dt.Rows(0).Item("Min_Minutes_Half_Shift_1").ToString
                End If

                If Not IsDBNull(dt.Rows(0).Item("Min_Minutes_One_Shift_2")) Then
                    msk_Min_Time_One_Shift_2.Text = dt.Rows(0).Item("Min_Minutes_One_Shift_2").ToString
                End If

                If Not IsDBNull(dt.Rows(0).Item("Min_Minutes_Half_Shift_2")) Then
                    msk_Min_Time_Half_Shift_2.Text = dt.Rows(0).Item("Min_Minutes_Half_Shift_2").ToString
                End If

                If Not IsDBNull(dt.Rows(0).Item("Min_Minutes_One_Shift_3")) Then
                    msk_Min_Time_One_Shift_3.Text = dt.Rows(0).Item("Min_Minutes_One_Shift_3").ToString
                End If

                If Not IsDBNull(dt.Rows(0).Item("Min_Minutes_Half_Shift_3")) Then
                    msk_Min_Time_Half_Shift_3.Text = dt.Rows(0).Item("Min_Minutes_Half_Shift_3").ToString
                End If

                '----------------------

                da2 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Category_Details a where a.Category_IdNo = " & Str(Val(idno)) & " Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_details.Rows.Add()

                        SNo = SNo + 1
                        dgv_details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("To_Attendance").ToString
                        dgv_details.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "###############0.00")

                    Next i
                    For i = 0 To dgv_details.RowCount - 1
                        dgv_details.Rows(i).Cells(0).Value = Val(i) + 1
                    Next
                End If

                dt2.Dispose()
                da2.Dispose()
            Else
                new_record()

            End If
            dgv_ActCtrlName = ""
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt.Dispose()
            da.Dispose()

            dt2.Dispose()
            da2.Dispose()

            Grid_Cell_DeSelect()
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Displaying = False
        End Try


    End Sub



    Private Sub PayRoll_Category_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Me.Height = 296 ' 197
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        con.Open()

        cbo_WeekOff.Items.Clear()
        cbo_WeekOff.Items.Add(" ")
        cbo_WeekOff.Items.Add("FIXED")
        cbo_WeekOff.Items.Add("ROTATION")

        cbo_AttendanceLeave.Items.Clear()
        cbo_AttendanceLeave.Items.Add(" ")
        cbo_AttendanceLeave.Items.Add("ATTENDANCE")
        cbo_AttendanceLeave.Items.Add("LEAVE")

        cbo_Monthly_Shift.Items.Clear()
        cbo_Monthly_Shift.Items.Add(" ")
        cbo_Monthly_Shift.Items.Add("MONTH")
        cbo_Monthly_Shift.Items.Add("SHIFT")


        cbo_CLArrearForMonth.Items.Clear()
        cbo_CLArrearForMonth.Items.Add(" ")
        cbo_CLArrearForMonth.Items.Add("SALARY")
        cbo_CLArrearForMonth.Items.Add("ELIMINATE")
        cbo_CLArrearForMonth.Items.Add("CARRY ON")

        cbo_SLArrearForMonth.Items.Clear()
        cbo_SLArrearForMonth.Items.Add(" ")
        cbo_SLArrearForMonth.Items.Add("SALARY")
        cbo_SLArrearForMonth.Items.Add("ELIMINATE")
        cbo_SLArrearForMonth.Items.Add("CARRY ON")


        cbo_CLArrearForYear.Items.Clear()
        cbo_CLArrearForYear.Items.Add(" ")
        cbo_CLArrearForYear.Items.Add("SALARY")
        cbo_CLArrearForYear.Items.Add("ELIMINATE")
        cbo_CLArrearForYear.Items.Add("CARRY ON")

        cbo_SLArrearForYear.Items.Clear()
        cbo_SLArrearForYear.Items.Add(" ")
        cbo_SLArrearForYear.Items.Add("SALARY")
        cbo_SLArrearForYear.Items.Add("ELIMINATE")
        cbo_SLArrearForYear.Items.Add("CARRY ON")

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_InTimeshift1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AttendanceLeave.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_InTimeShift2.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_inTimeShift3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeekOff.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_lunchMiniutes.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OutTime_Shift2.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Working_Hours_Shift2.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_ot.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_TimeDelay.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AttendanceLeave.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Attendance_Ot.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Attendance_Incentive.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OutTime_Shift1.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OutTime_Shift2.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OutTime_Shift3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Monthly_Shift.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_OtAllowed_Minute.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MinimumDelay.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_FestivalHolidays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Incentive_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Working_Hours_Shift1.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Working_Hours_Shift2.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Working_Hours_Shift3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofDaye_Monthly.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_WeekOffCredit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LessMinuteDelay.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Production.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Festival_Holiday_OtSalary.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Incentive_Amount_Days.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AttnIncenRange1_FromDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AttnIncenRange2_FromDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AttnIncenRange1_ToDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AttnIncenRange2_ToDays.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_CL.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_SL.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CLArrearForMonth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SLArrearForMonth.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_CLArrearForYear.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SLArrearForYear.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_WeekOff_Allowance.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_InTimeshift1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AttendanceLeave.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_InTimeShift2.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_inTimeShift3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WeekOff.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_lunchMiniutes.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AttendanceLeave.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_ot.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_TimeDelay.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Attendance_Ot.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Attendance_Incentive.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_OutTime_Shift1.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_OutTime_Shift2.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_OutTime_Shift3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Monthly_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OtAllowed_Minute.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinimumDelay.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_FestivalHolidays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Incentive_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Working_Hours_Shift1.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Working_Hours_Shift2.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Working_Hours_Shift3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofDaye_Monthly.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_WeekOffCredit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LessMinuteDelay.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinimumDelay.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Production.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Festival_Holiday_OtSalary.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Incentive_Amount_Days.LostFocus, AddressOf ControlLostFocus
        ' AddHandler txt_BankAcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AttnIncenRange1_FromDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AttnIncenRange2_FromDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AttnIncenRange1_ToDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AttnIncenRange2_ToDays.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_CL.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_SL.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CLArrearForMonth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SLArrearForMonth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CLArrearForYear.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SLArrearForYear.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_WeekOff_Allowance.LostFocus, AddressOf ControlLostFocus




        ' AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_InTimeshift1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_InTimeShift2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_inTimeShift3.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_lunchMiniutes.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_ot.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_TimeDelay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Attendance_Ot.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler chk_Attendance_Incentive.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_OutTime_Shift1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_OutTime_Shift2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_OutTime_Shift3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OtAllowed_Minute.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MinimumDelay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Chk_FestivalHolidays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Incentive_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Working_Hours_Shift1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Working_Hours_Shift2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Working_Hours_Shift3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoofDaye_Monthly.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LessMinuteDelay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_WeekOffCredit.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_Production.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler chk_Festival_Holiday_OtSalary.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_LeaveSalaryLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Incentive_Amount_Days.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AttnIncenRange1_FromDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AttnIncenRange2_FromDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AttnIncenRange1_ToDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AttnIncenRange2_ToDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_CL.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_SL.KeyDown, AddressOf TextBoxControlKeyDown


        ' AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler msk_InTimeshift1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_InTimeShift2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_inTimeShift3.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_lunchMiniutes.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_ot.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_TimeDelay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Attendance_Ot.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler chk_Attendance_Incentive.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_OutTime_Shift1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_OutTime_Shift2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_OutTime_Shift3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Chk_FestivalHolidays.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler chk_Festival_Holiday_OtSalary.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_WeekOffCredit.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_Production.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_OtAllowed_Minute.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MinimumDelay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Incentive_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Working_Hours_Shift1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Working_Hours_Shift2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Working_Hours_Shift3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoofDaye_Monthly.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LessMinuteDelay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_LeaveSalaryLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AttnIncenRange1_FromDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AttnIncenRange2_FromDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AttnIncenRange1_ToDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AttnIncenRange2_ToDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_CL.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_SL.KeyPress, AddressOf TextBoxControlKeyPress

        new_record()

    End Sub

    Private Sub PayRoll_Category_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            'If grp_Find.Visible Then
            '    btnClose_Click(sender, e)
            'ElseIf grp_Filter.Visible Then
            '    btn_CloseFilter_Click(sender, e)
            'Else
            Me.Close()
        End If
        'End If
    End Sub

    Private Sub PayRoll_Category_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("D") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If


        Dim cmd As New SqlClient.SqlCommand


        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Category_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Category_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            cmd.Connection = con
            cmd.CommandText = "delete from PayRoll_Category_Head where Category_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from PayRoll_Category_Details where Category_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        'Dim da As New SqlClient.SqlDataAdapter("select Category_IdNo, Bag_Type_Name,Weight_Bag from PayRoll_Category_Head where Category_IdNo <> 0 order by Category_IdNo", con)
        'Dim dt As New DataTable

        'da.Fill(dt)

        'With dgv_Filter

        '    .Columns.Clear()
        '    .DataSource = dt

        '    .RowHeadersVisible = False

        '    .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        '    .Columns(0).HeaderText = "IDNO"
        '    .Columns(1).HeaderText = "BAGTYPE NAME"
        '    .Columns(2).HeaderText = "WEIGHT BAG"

        '    .Columns(0).FillWeight = 40
        '    .Columns(1).FillWeight = 160
        '    .Columns(2).FillWeight = 80

        'End With

        'new_record()

        'grp_Filter.Visible = True
        'grp_Filter.Left = grp_Find.Left
        'grp_Filter.Top = grp_Find.Top

        'pnl_Back.Enabled = False

        'If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        'Me.Height = 595 ' 400

        'dt.Dispose()
        'da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Category_IdNo) from PayRoll_Category_Head Where Category_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(Category_IdNo) from PayRoll_Category_Head Where Category_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select min(Category_IdNo) from PayRoll_Category_Head Where Category_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Category_IdNo <> 0", con)
            da.Fill(dt)


            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(Category_IdNo) from PayRoll_Category_Head where Category_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Category_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record


        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("A") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        clear()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "PayRoll_Category_Head", "Category_IdNo", "")



        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        'Dim da As New SqlClient.SqlDataAdapter("select Bag_Type_Name from PayRoll_Category_Head order by Bag_Type_Name", con)
        'Dim dt As New DataTable

        'da.Fill(dt)

        'cbo_Find.DataSource = dt
        'cbo_Find.DisplayMember = "Bag_Type_Name"

        'new_record()

        'grp_Find.Visible = True
        'pnl_Back.Enabled = False

        'If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        'Me.Height = 521 ' 355

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--- No Printing
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
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String
        Dim WrkTy_id As Integer = 0
        Dim SNo As Integer = 0
        Dim vTmDey As Integer = 0, vFhDa As Integer = 0
        Dim vOT As Integer = 0, vAttOT As Integer = 0
        Dim vAttIC As Integer = 0, vFhld As Integer = 0
        Dim vWekCd As Integer = 0, vProd As Integer = 0
        Dim vLeaSal As Integer = 1
        Dim vWkOf_allow As Integer = 0
        Dim vCL As Integer = 0, vSL As Integer = 0
        Dim Sht1_Wrk_Mins As Integer = 0
        Dim Sht1_Wrk_Hrs As Integer = 0
        Dim Sht2_Wrk_Mins As Integer = 0
        Dim Sht2_Wrk_Hrs As Integer = 0
        Dim Sht3_Wrk_Mins As Integer = 0
        Dim Sht3_Wrk_Hrs As Integer = 0
        Dim ShftRTn As Integer = 0

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Category_Creation, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub
        End If

        If Val(Common_Procedures.settings.PAYROLLENTRY_Attendance_In_Hours_Status) = 1 Then
            If Val(msk_Working_Hours_Shift1.Text) = 0 Or Val(msk_Working_Hours_Shift2.Text) = 0 Or Val(msk_Working_Hours_Shift3.Text) = 0 Then
                MessageBox.Show("Invalid Shift Time", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_InTimeshift1.Enabled And msk_InTimeshift1.Visible Then msk_InTimeshift1.Focus()
                Exit Sub
            End If
        End If

        If Trim(cbo_Monthly_Shift.Text) = "" Then
            MessageBox.Show("Invalid Salary Type", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Monthly_Shift.Enabled And cbo_Monthly_Shift.Visible Then cbo_Monthly_Shift.Focus()
            Exit Sub
        End If

        If Val(msk_Working_Hours_Shift1.Text) = 0 And Val(msk_Working_Hours_Shift2.Text) = 0 And Val(msk_Working_Hours_Shift3.Text) = 0 Then
            MessageBox.Show("Invalid Shift Time", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_InTimeshift1.Enabled And msk_InTimeshift1.Visible Then msk_InTimeshift1.Focus()
            Exit Sub
        End If

        If Val(txt_NoofDaye_Monthly.Text) = 0 And Trim(cbo_Monthly_Shift.Text) = "MONTH" Then
            MessageBox.Show("Invalid No.of Days for Month Salary", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_NoofDaye_Monthly.Enabled And txt_NoofDaye_Monthly.Visible Then txt_NoofDaye_Monthly.Focus()
            Exit Sub
        End If

        If Val(msk_Working_Hours_Shift1.Text) > 0 Then

            'If Val(msk_Min_Time_Half_Shift_1.Text) = 0 Then
            '    MsgBox("Please Enter the Minimum Minutes of Work Required For 'Half' Shift")
            '    If msk_Min_Time_Half_Shift_1.Enabled And msk_Min_Time_Half_Shift_1.Visible Then
            '        msk_Min_Time_Half_Shift_1.Focus()
            '    End If
            'End If

            'If Val(msk_Min_Time_One_Shift_1.Text) = 0 Then
            '    MsgBox("Please Enter the Minimum Minutes of Work Required For 'One' Shift")
            '    If msk_Min_Time_One_Shift_1.Enabled And msk_Min_Time_One_Shift_1.Visible Then
            '        msk_Min_Time_One_Shift_1.Focus()
            '    End If
            'End If

        Else
            msk_Min_Time_Half_Shift_1.Text = "0"
            msk_Min_Time_One_Shift_1.Text = "0"

        End If

        If Val(msk_Working_Hours_Shift2.Text) > 0 Then

            'If Val(msk_Min_Time_Half_Shift_2.Text) = 0 Then
            '    MsgBox("Please Enter the Minimum Minutes of Work Required For 'Half' Shift")
            '    If msk_Min_Time_Half_Shift_2.Enabled And msk_Min_Time_Half_Shift_2.Visible Then
            '        msk_Min_Time_Half_Shift_2.Focus()
            '    End If
            'End If

            'If Val(msk_Min_Time_One_Shift_2.Text) = 0 Then
            '    MsgBox("Please Enter the Minimum Minutes of Work Required For 'One' Shift")
            '    If msk_Min_Time_One_Shift_2.Enabled And msk_Min_Time_One_Shift_2.Visible Then
            '        msk_Min_Time_One_Shift_2.Focus()
            '    End If
            'End If

        Else
            msk_Min_Time_Half_Shift_2.Text = "0"
            msk_Min_Time_One_Shift_2.Text = "0"

        End If

        If Val(msk_Working_Hours_Shift3.Text) > 0 Then

            'If Val(msk_Min_Time_Half_Shift_3.Text) = 0 Then
            '    MsgBox("Please Enter the Minimum Minutes of Work Required For 'Half' Shift")
            '    If msk_Min_Time_Half_Shift_3.Enabled And msk_Min_Time_Half_Shift_3.Visible Then
            '        msk_Min_Time_Half_Shift_3.Focus()
            '    End If
            'End If

            'If Val(msk_Min_Time_One_Shift_3.Text) = 0 Then
            '    MsgBox("Please Enter the Minimum Minutes of Work Required For 'One' Shift")
            '    If msk_Min_Time_One_Shift_3.Enabled And msk_Min_Time_One_Shift_3.Visible Then
            '        msk_Min_Time_One_Shift_3.Focus()
            '    End If
            'End If

        Else
            msk_Min_Time_Half_Shift_3.Text = "0"
            msk_Min_Time_One_Shift_3.Text = "0"

        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        vFhld = 0
        If Chk_FestivalHolidays.Checked = True Then vFhld = 1

        vOT = 0
        If chk_ot.Checked = True Then vOT = 1

        vAttIC = 0
        If chk_Attendance_Incentive.Checked = True Then vAttIC = 1

        vWekCd = 0
        If chk_WeekOffCredit.Checked = True Then vWekCd = 1

        vProd = 0
        If chk_Production.Checked = True Then vProd = 1

        vFhDa = 0
        If chk_Festival_Holiday_OtSalary.Checked = True Then vFhDa = 1


        vWkOf_allow = 0
        If chk_WeekOff_Allowance.Checked = True Then vWkOf_allow = 1


        vAttOT = 0
        If chk_Attendance_Ot.Checked = True Then vAttOT = 1

        vTmDey = 0
        If chk_TimeDelay.Checked = True Then vTmDey = 1


        vLeaSal = 1
        If chk_LeaveSalaryLess.Checked = False Then vLeaSal = 0

        vCL = 0
        If chk_CL.Checked = True Then vCL = 1
        vSL = 0
        If chk_SL.Checked = True Then vSL = 1



        Sht1_Wrk_Hrs = Val(msk_Working_Hours_Shift1.Text)
        Sht1_Wrk_Mins = Sht1_Wrk_Hrs * 60
        Sht1_Wrk_Mins = Sht1_Wrk_Mins + ((Val(msk_Working_Hours_Shift1.Text) - Sht1_Wrk_Hrs) * 100)

        Sht2_Wrk_Hrs = Val(msk_Working_Hours_Shift2.Text)
        Sht2_Wrk_Mins = Sht2_Wrk_Hrs * 60
        Sht2_Wrk_Mins = Sht2_Wrk_Mins + ((Val(msk_Working_Hours_Shift2.Text) - Sht2_Wrk_Hrs) * 100)

        Sht3_Wrk_Hrs = Val(msk_Working_Hours_Shift3.Text)
        Sht3_Wrk_Mins = Sht3_Wrk_Hrs * 60
        Sht3_Wrk_Mins = Sht3_Wrk_Mins + ((Val(msk_Working_Hours_Shift3.Text) - Sht2_Wrk_Hrs) * 100)


        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "PayRoll_Category_Head", "Category_IdNo", "", trans)

                cmd.CommandText = "Insert into PayRoll_Category_Head ( Category_IdNo, Category_Name, sur_name, Shift1_In_Time, Shift2_In_Time, Shift3_In_Time, Lunch_Minutes, " &
                    "Fixed_Rotation, OT_Allowed, Time_Delay, Attendance_Leave, Week_Attendance_OT, Attendance_Incentive, Shift1_Out_Time, Shift2_Out_Time, Shift3_Out_Time," &
                    "Monthly_Shift, OT_Allowed_After_Minutes, Minimum_Delay, Festival_Holidays, Incentive_Amount, Shift1_Working_Hours, Shift2_Working_Hours, Shift3_Working_Hours," &
                    "No_Days_Month_Wages, Week_Off_Credit, Less_Minute_Delay, Production_Incentive, Festival_Holidays_Ot_Salary, Incentive_Amount_Days, Leave_Salary_Less," &
                    "Att_Incentive_FromDays_Range1, Att_Incentive_ToDays_Range1, Att_Incentive_FromDays_Range2, Att_Incentive_ToDays_Range2, CL_Leave, SL_Leave, CL_Arrear_Type, " &
                    "SL_Arrear_Type ,CL_Arrear_Type_Year , SL_Arrear_Type_Year, Shift1_Working_Minutes ,Shift2_Working_Minutes ,Shift3_Working_Minutes  , Week_Off_Allowance , " &
                    "Min_Minutes_One_Shift_1,Min_Minutes_Half_Shift_1,Min_Minutes_One_Shift_2,Min_Minutes_Half_Shift_2,Min_Minutes_One_Shift_3,Min_Minutes_Half_Shift_3) " &
                    "Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "', '" & Trim(msk_InTimeshift1.Text) & "'," &
                    "'" & Trim(msk_InTimeShift2.Text) & "', '" & Trim(msk_inTimeShift3.Text) & "', " & Val(txt_lunchMiniutes.Text) & ", '" & Trim(cbo_WeekOff.Text) & "', " &
                    Val(vOT).ToString & ", " & Val(vTmDey) & ", '" & Trim(cbo_AttendanceLeave.Text) & "', " & Val(vAttOT) & ", " & Val(vAttIC) & ", '" & Trim(msk_OutTime_Shift1.Text) & "'" &
                    ", '" & Trim(msk_OutTime_Shift2.Text) & "', '" & Trim(msk_OutTime_Shift3.Text) & "', '" & Trim(cbo_Monthly_Shift.Text) & "', " & Val(txt_OtAllowed_Minute.Text) &
                    ", " & Val(txt_MinimumDelay.Text) & ", " & Val(vFhld) & ", " & Val(txt_Incentive_Amount.Text) & ", '" & Trim(msk_Working_Hours_Shift1.Text) & "'" &
                    ", '" & Trim(msk_Working_Hours_Shift2.Text) & "', '" & Trim(msk_Working_Hours_Shift3.Text) & "', " & Val(txt_NoofDaye_Monthly.Text) & "," &
                     Val(vWekCd).ToString & ", " & Val(txt_LessMinuteDelay.Text) & ", " & Val(vProd) & ", " & Val(vFhDa) & ", " & Val(txt_Incentive_Amount_Days.Text) &
                    "," & Val(vLeaSal) & ", " & Val(txt_AttnIncenRange1_FromDays.Text) & ", " & Val(txt_AttnIncenRange1_ToDays.Text) & ", " & Val(txt_AttnIncenRange2_FromDays.Text) &
                    ", " & Val(txt_AttnIncenRange2_ToDays.Text) & ", " & Val(vCL) & ", " & Val(vSL) & ", '" & Trim(cbo_CLArrearForMonth.Text) & "'" &
                    ", '" & Trim(cbo_SLArrearForMonth.Text) & "','" & Trim(cbo_CLArrearForYear.Text) & "', '" & Trim(cbo_SLArrearForYear.Text) & "' ," & Val(Sht1_Wrk_Mins) &
                    " ," & Val(Sht2_Wrk_Mins) & "," & Val(Sht3_Wrk_Mins) & "," & Val(vWkOf_allow) &
                    " ," & Val(msk_Min_Time_One_Shift_1.Text).ToString & "," & Val(msk_Min_Time_Half_Shift_1.Text).ToString & "," & Val(msk_Min_Time_One_Shift_2.Text).ToString & "," &
                    Val(msk_Min_Time_Half_Shift_2.Text).ToString & "," & Val(msk_Min_Time_One_Shift_3.Text).ToString & "," & Val(msk_Min_Time_Half_Shift_3.Text).ToString & ")"

                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update PayRoll_Category_Head set Category_Name = '" & Trim(txt_Name.Text) & "', sur_name = '" & Trim(Sur) & "'," &
                    "Shift1_In_Time = '" & Trim(msk_InTimeshift1.Text) & "' , Shift2_In_Time = '" & Trim(msk_InTimeShift2.Text) & "' " &
                    ",Shift3_In_Time = '" & Trim(msk_inTimeShift3.Text) & "' ,Lunch_Minutes = " & Val(txt_lunchMiniutes.Text) & " , Fixed_Rotation = '" & Trim(cbo_WeekOff.Text) & "'" &
                    ",OT_Allowed = " & Val(vOT) & " ,Time_Delay = " & Val(vTmDey) & " ,Attendance_Leave = '" & Trim(cbo_AttendanceLeave.Text) & "' " &
                    ",Week_Attendance_OT = " & Val(vAttOT) & " ,Attendance_Incentive = " & Val(vAttIC) & " ,Shift1_Out_Time = '" & Trim(msk_OutTime_Shift1.Text) & "' " &
                    ",Shift2_Out_Time = '" & Trim(msk_OutTime_Shift2.Text) & "' ,Shift3_Out_Time = '" & Trim(msk_OutTime_Shift3.Text) & "' " &
                    ", Monthly_Shift = '" & Trim(cbo_Monthly_Shift.Text) & "' ,OT_Allowed_After_Minutes = " & Val(txt_OtAllowed_Minute.Text) &
                    " ,Minimum_Delay = " & Val(txt_MinimumDelay.Text) & " ,Festival_Holidays = " & Val(vFhld) & ",Incentive_Amount = " & Val(txt_Incentive_Amount.Text) &
                    " ,Shift1_Working_Hours = '" & Trim(msk_Working_Hours_Shift1.Text) & "' ,Shift2_Working_Hours = '" & Trim(msk_Working_Hours_Shift2.Text) & "' " &
                    ",Shift3_Working_Hours = '" & Trim(msk_Working_Hours_Shift3.Text) & "' ,No_Days_Month_Wages = " & Val(txt_NoofDaye_Monthly.Text) &
                    " ,Week_Off_Credit = " & Val(vWekCd) & ",Less_Minute_Delay = " & Val(txt_LessMinuteDelay.Text) & " ,Production_Incentive = " & Val(vProd) &
                    " ,Festival_Holidays_Ot_Salary = " & Val(vFhDa) & " ,Incentive_Amount_Days = " & Val(txt_Incentive_Amount_Days.Text) &
                    " , Leave_Salary_Less = " & Val(vLeaSal) & " ,Att_Incentive_FromDays_Range1 = " & Val(txt_AttnIncenRange1_FromDays.Text) &
                    " ,Att_Incentive_ToDays_Range1 = " & Val(txt_AttnIncenRange1_ToDays.Text) & " ,Att_Incentive_FromDays_Range2 =  " & Val(txt_AttnIncenRange2_FromDays.Text) &
                    " ,Att_Incentive_ToDays_Range2 = " & Val(txt_AttnIncenRange2_ToDays.Text) & ", CL_Leave =" & Val(vCL) & ",SL_Leave =" & Val(vSL) &
                    " ,CL_Arrear_Type ='" & Trim(cbo_CLArrearForMonth.Text) & "' ,SL_Arrear_Type ='" & Trim(cbo_SLArrearForMonth.Text) & "'" &
                    " ,CL_Arrear_Type_Year ='" & Trim(cbo_CLArrearForYear.Text) & "' ,  SL_Arrear_Type_Year = '" & Trim(cbo_SLArrearForYear.Text) & "'" &
                    " ,Shift1_Working_Minutes =" & Val(Sht1_Wrk_Mins) & " ,Shift2_Working_Minutes  =" & Val(Sht2_Wrk_Mins) & ",Shift3_Working_Minutes =" & Val(Sht3_Wrk_Mins) &
                    " ,Week_Off_Allowance = " & Val(vWkOf_allow) & ",  Min_Minutes_One_Shift_1 = " & Val(msk_Min_Time_One_Shift_1.Text).ToString &
                    " , Min_Minutes_Half_Shift_1 = " & Val(msk_Min_Time_Half_Shift_1.Text).ToString & ",  Min_Minutes_One_Shift_2 = " & Val(msk_Min_Time_One_Shift_2.Text).ToString &
                    " , Min_Minutes_Half_Shift_2 = " & Val(msk_Min_Time_Half_Shift_2.Text).ToString & ",  Min_Minutes_One_Shift_3 = " & Val(msk_Min_Time_One_Shift_3.Text).ToString &
                    " , Min_Minutes_Half_Shift_3 = " & Val(msk_Min_Time_Half_Shift_3.Text).ToString & " where Category_IdNo = " & Str(Val(lbl_IdNo.Text))


                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from PayRoll_Category_Details where Category_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_details
                SNo = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then


                        SNo = SNo + 1


                        cmd.CommandText = "Insert into PayRoll_Category_Details (             Category_IdNo      ,            sl_no             ,                To_Attendance        ,        Amount                   ) " &
                                                "       Values                          ( " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SNo)) & ", " & Val(.Rows(i).Cells(1).Value) & ",  " & Val(.Rows(i).Cells(2).Value) & "    ) "
                        cmd.ExecuteNonQuery()

                    End If


                Next

            End With

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "CATEGORY"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If
            Else
                move_record(lbl_IdNo.Text)
            End If

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_payRoll_category_head") > 0 Then
                MessageBox.Show("Duplicate Category Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown

        If e.KeyCode = 40 Then
            cbo_Monthly_Shift.Focus()
        End If

    End Sub





    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            cbo_Monthly_Shift.Focus()
        End If

    End Sub


    Private Sub cbo_Weekoff_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeekOff.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WeekOff, txt_lunchMiniutes, chk_ot, "", "", "", "")


    End Sub

    Private Sub cbo_WeekOff_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeekOff.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WeekOff, chk_ot, "", "", "", "")

    End Sub

    Private Sub msk_InTimeshift1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_InTimeshift1.KeyDown
        If e.KeyCode = 38 Then
            txt_Name.Focus()
        End If
        If e.KeyCode = 40 Then
            msk_OutTime_Shift1.Focus()
        End If
        msk_Working_Hours_Shift1.Text = getHourFromMinitues(msk_InTimeshift1.Text, msk_OutTime_Shift1.Text)
    End Sub

    Private Sub msk_InTimeshift1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_InTimeshift1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift1.Text = getHourFromMinitues(msk_InTimeshift1.Text, msk_OutTime_Shift1.Text)
    End Sub

    Private Sub msk_InTimeShift2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_InTimeShift2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift2.Text = getHourFromMinitues(msk_InTimeShift2.Text, msk_OutTime_Shift2.Text)
    End Sub

    Private Sub msk_inTimeShift3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_inTimeShift3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift3.Text = getHourFromMinitues(msk_inTimeShift3.Text, msk_OutTime_Shift3.Text)
    End Sub

    Private Sub txt_lunchMiniutes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_lunchMiniutes.KeyDown


        If (e.KeyValue) = 38 Then
            msk_OutTime_Shift3.Focus()
        End If


        If (e.KeyValue) = 40 Then
            If Trim(cbo_Monthly_Shift.Text) = "MONTH" Then
                txt_NoofDaye_Monthly.Focus()
            Else
                tab_main.SelectTab(1)
                chk_ot.Focus()
            End If
        End If


    End Sub

    Private Sub txt_lunchMinutes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_lunchMiniutes.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_Monthly_Shift.Text) = "MONTH" Then
                txt_NoofDaye_Monthly.Focus()
            Else
                tab_main.SelectTab(1)
                chk_ot.Focus()
            End If
        End If

    End Sub

    Private Sub msk_OutTime_Shift1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_OutTime_Shift1.KeyDown
        msk_Working_Hours_Shift1.Text = getHourFromMinitues(msk_InTimeshift1.Text, msk_OutTime_Shift1.Text)
    End Sub

    Private Sub msk_OutTime_Shift1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_OutTime_Shift1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift1.Text = getHourFromMinitues(msk_InTimeshift1.Text, msk_OutTime_Shift1.Text)
    End Sub



    Private Sub msk_OutTime_Shift2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_OutTime_Shift2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift2.Text = getHourFromMinitues(msk_InTimeShift2.Text, msk_OutTime_Shift2.Text)
    End Sub

    Private Sub txt_MinimumDelay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumDelay.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub msk_OutTime_Shift3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_OutTime_Shift3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        msk_Working_Hours_Shift3.Text = getHourFromMinitues(msk_inTimeShift3.Text, msk_OutTime_Shift3.Text)
    End Sub

    Private Sub txt_LessMiniuteDelay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LessMinuteDelay.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Incentive_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Incentive_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Incentive_Amount_Days_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Incentive_Amount_Days.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txt_OtAllowed_After_Miniute_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OtAllowed_Minute.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_opAmt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofDaye_Monthly.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub cbo_Monthly_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Monthly_Shift.GotFocus
        cbo_Monthly_Shift.Tag = cbo_Monthly_Shift.Text
    End Sub

    Private Sub cbo_Monthly_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Monthly_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Monthly_Shift, txt_lunchMiniutes, Nothing, "", "", "", "")


        If (e.KeyValue = 38 And cbo_Monthly_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            txt_Name.Focus()
        End If
        If (e.KeyValue = 40 And cbo_Monthly_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            'tab_main.SelectTab(1)
            'chk_WeekOffCredit.Focus()

            tab_main.SelectTab(0)
            msk_InTimeshift1.Focus()

        End If
    End Sub

    Private Sub cbo_Monthly_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Monthly_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Monthly_Shift, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then

            tab_main.SelectTab(0)
            msk_InTimeshift1.Focus()


            If Trim(UCase(cbo_Monthly_Shift.Tag)) <> Trim(UCase(cbo_Monthly_Shift.Text)) Then
                If Trim(UCase(cbo_Monthly_Shift.Text)) = "SHIFT" Then
                    txt_NoofDaye_Monthly.Text = ""
                    cbo_AttendanceLeave.Text = "ATTENDANCE"
                    chk_LeaveSalaryLess.Checked = True


                    txt_NoofDaye_Monthly.Enabled = False
                    cbo_AttendanceLeave.Enabled = False
                    chk_LeaveSalaryLess.Enabled = False

                Else
                    If Val(txt_NoofDaye_Monthly.Text) = 0 Then
                        txt_NoofDaye_Monthly.Text = "26"
                        cbo_AttendanceLeave.Text = "LEAVE"
                    End If

                    txt_NoofDaye_Monthly.Enabled = True
                    cbo_AttendanceLeave.Enabled = True
                    chk_LeaveSalaryLess.Enabled = True


                End If
            End If
        End If
    End Sub

    Private Sub cbo_AttendanceLeave_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AttendanceLeave.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AttendanceLeave, txt_NoofDaye_Monthly, Nothing, "", "", "", "")
        If (e.KeyCode = 40 And cbo_AttendanceLeave.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            tab_main.SelectTab(1)
            chk_LeaveSalaryLess.Focus()
        End If
    End Sub

    Private Sub cbo_AttendanceLeave_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AttendanceLeave.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AttendanceLeave, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            tab_main.SelectTab(1)
            chk_LeaveSalaryLess.Focus()
        End If
    End Sub


    Private Sub cbo_CLArrear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CLArrearForMonth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CLArrearForMonth, chk_SL, cbo_CLArrearForYear, "", "", "", "")
    End Sub

    Private Sub cbo_CLArrear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CLArrearForMonth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CLArrearForMonth, cbo_CLArrearForYear, "", "", "", "")
    End Sub

    Private Sub cbo_SLArrear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SLArrearForMonth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SLArrearForMonth, cbo_CLArrearForYear, cbo_SLArrearForYear, "", "", "", "")
    End Sub

    Private Sub cbo_SLArrear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SLArrearForMonth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SLArrearForMonth, cbo_SLArrearForYear, "", "", "", "")
    End Sub


    Private Sub cbo_CLArrearForYear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CLArrearForYear.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CLArrearForYear, cbo_SLArrearForMonth, chk_SL, "", "", "", "")
    End Sub

    Private Sub cbo_CLArrearForYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CLArrearForYear.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CLArrearForYear, chk_SL, "", "", "", "")
    End Sub

    Private Sub cbo_SLArrearForYear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SLArrearForYear.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SLArrearForYear, cbo_CLArrearForMonth, Chk_FestivalHolidays, "", "", "", "")
    End Sub

    Private Sub cbo_SLArrearForYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SLArrearForYear.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SLArrearForMonth, Chk_FestivalHolidays, "", "", "", "")
    End Sub
    Function getHourFromMinitues(ByVal inTime As String, ByVal outTime As String)

        Dim Dt1 As Date, Dt2 As Date
        Dim TotMins As Double
        Dim H As Double, m As Double, Hrs As Double

        If Val(Microsoft.VisualBasic.Left(inTime, 2)) >= 24 Then MessageBox.Show("Time should not greater than 24", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Right(inTime, 2)) >= 60 Then MessageBox.Show("Minutes should not greater than 60", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Left(outTime, 2)) >= 24 Then MessageBox.Show("Time should not greater than 24", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Right(outTime, 2)) >= 60 Then MessageBox.Show("Minutes should not greater than 60", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        If Trim(inTime) <> "" And Trim(outTime) <> "" Then
            'If Microsoft.VisualBasic.Len(Trim(outTime)) = 4 Then
            '    outTime = Trim(outTime) & Microsoft.VisualBasic.Right(Trim(inTime), 1)
            'End If
            If IsDate(inTime) And IsDate(outTime) Then
                If IsDate(Convert.ToDateTime(inTime)) And IsDate(Convert.ToDateTime(outTime)) Then

                    Dt1 = Convert.ToDateTime(inTime)
                    Dt2 = Convert.ToDateTime(outTime)

                    If Convert.ToDateTime(outTime) > Convert.ToDateTime(inTime) Then
                        TotMins = DateDiff("n", Dt1, Dt2)
                    Else

                        Dt2 = CDate(DateAdd("d", 1, Dt2))
                        TotMins = DateDiff("n", Dt1, Dt2)
                    End If

                    H = TotMins \ 60
                    m = TotMins - (H * 60)
                    Hrs = H & "." & Format(m, "00")
                End If
            End If
        End If

        Return Hrs
    End Function


    Private Sub chk_CL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_CL.CheckedChanged
        If chk_CL.Checked = True Then
            cbo_CLArrearForMonth.Enabled = True
            cbo_CLArrearForYear.Enabled = True
        Else
            cbo_CLArrearForMonth.Text = ""
            cbo_CLArrearForMonth.Enabled = False
            cbo_CLArrearForYear.Enabled = False
        End If

    End Sub

    Private Sub chk_SL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_SL.CheckedChanged
        If chk_SL.Checked = True Then
            cbo_SLArrearForMonth.Enabled = True
            cbo_SLArrearForYear.Enabled = True
        Else
            cbo_SLArrearForMonth.Text = ""
            cbo_SLArrearForMonth.Enabled = False
            cbo_SLArrearForYear.Enabled = False
        End If

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub
    Private Sub dgv_details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_details.EditingControlShowing
        dgtxt_Details = CType(dgv_details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgv_Releavedetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_details.LostFocus
        On Error Resume Next
        dgv_details.CurrentCell.Selected = False
    End Sub
    Private Sub dgv_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_details.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_details
                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

            End With
        End If

    End Sub
    Private Sub dgv_details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_details.RowsAdded
        Dim n As Integer

        With dgv_details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing


            If ActiveControl.Name = dgv_details.Name Then
                dgv1 = dgv_details

            ElseIf dgv_details.IsCurrentRowDirty = True Then
                dgv1 = dgv_details


            ElseIf dgv_ActCtrlName = dgv_details.Name Then
                dgv1 = dgv_details

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_details
         
            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_details.Name Then



                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                        save_record()
                                    Else
                                        txt_Name.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If
                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_Details.Text) = 0)) Then
                                    For i = 0 To .Columns.Count - 1
                                        .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                    Next

                                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                        save_record()
                                    Else
                                        txt_Name.Focus()
                                    End If



                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                End If
                            End If
                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    tab_main.SelectTab(2)
                                    chk_Attendance_Incentive.Focus()


                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                                End If




                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                  

                    End If
                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If
    End Function
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActCtrlName = dgv_details.Name
        dgv_details.EditingControl.BackColor = Color.Lime
        dgv_details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub chk_Festival_Holiday_OtSalary_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_Festival_Holiday_OtSalary.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then

            chk_WeekOffCredit.Focus()

         
        End If
    End Sub

    Private Sub chk_Festival_Holiday_OtSalary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Festival_Holiday_OtSalary.KeyPress
        If Asc(e.KeyChar) = 13 Then

            chk_WeekOffCredit.Focus()

        End If
    End Sub

    Private Sub chk_Attendance_Incentive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Attendance_Incentive.CheckedChanged
        If chk_Attendance_Incentive.Checked = True Then
            dgv_details.Enabled = True

        Else
            dgv_details.Enabled = False

        End If
    End Sub

    Private Sub chk_Attendance_Incentive_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_Attendance_Incentive.KeyDown
       
        If e.KeyCode = 38 Then
            tab_main.SelectTab(1)
            chk_Festival_Holiday_OtSalary.Focus()
        End If
        If e.KeyCode = 40 Then
            If dgv_details.Rows.Count > 0 Then

                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)

            End If
        End If
    End Sub

    Private Sub chk_Attendance_Incentive_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Attendance_Incentive.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If chk_Attendance_Incentive.Checked = True Then
                If dgv_details.Rows.Count > 0 Then


                    dgv_details.Focus()
                    dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)

                End If
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

            End If
          
        End If
    End Sub
    Private Sub chk_WeekOff_Allowance_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_WeekOff_Allowance.KeyDown
        If e.KeyCode = 40 Then
            tab_main.SelectTab(2)
            chk_Attendance_Incentive.Focus()
        End If
    End Sub

    Private Sub chk_WeekOff_Allowance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_WeekOff_Allowance.KeyPress
        If Asc(e.KeyChar) = 13 Then
            tab_main.SelectTab(2)
            chk_Attendance_Incentive.Focus()
        End If


    End Sub

    Private Sub msk_InTimeshift1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_InTimeshift1.TextChanged
        msk_Working_Hours_Shift1.Text = getHourFromMinitues(msk_InTimeshift1.Text, msk_OutTime_Shift1.Text)
    End Sub

    Private Sub msk_InTimeShift2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_InTimeShift2.TextChanged
        msk_Working_Hours_Shift2.Text = getHourFromMinitues(msk_InTimeShift2.Text, msk_OutTime_Shift2.Text)
    End Sub

    Private Sub msk_inTimeShift3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_inTimeShift3.TextChanged
        msk_Working_Hours_Shift3.Text = getHourFromMinitues(msk_inTimeShift3.Text, msk_OutTime_Shift3.Text)
    End Sub

    Private Sub cbo_Monthly_Shift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Monthly_Shift.TextChanged

        If Not Displaying Then
            If Trim(UCase(cbo_Monthly_Shift.Tag)) <> Trim(UCase(cbo_Monthly_Shift.Text)) Then
                If Trim(UCase(cbo_Monthly_Shift.Text)) = "SHIFT" Then
                    txt_NoofDaye_Monthly.Text = ""
                    cbo_AttendanceLeave.Text = "ATTENDANCE"
                    chk_LeaveSalaryLess.Checked = True


                    txt_NoofDaye_Monthly.Enabled = False
                    cbo_AttendanceLeave.Enabled = False
                    chk_LeaveSalaryLess.Enabled = False

                Else

                    If Val(txt_NoofDaye_Monthly.Text) = 0 Then
                        txt_NoofDaye_Monthly.Text = "26"
                        cbo_AttendanceLeave.Text = "LEAVE"
                    End If

                    txt_NoofDaye_Monthly.Enabled = True
                    cbo_AttendanceLeave.Enabled = True
                    chk_LeaveSalaryLess.Enabled = True

                End If
            End If
        End If
    End Sub

    Private Sub cbo_CLArrearForMonth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CLArrearForMonth.TextChanged

        If Trim(cbo_CLArrearForMonth.Text) = "CARRY ON" Then
            cbo_CLArrearForYear.Enabled = True
        Else
            cbo_CLArrearForYear.Enabled = False
            cbo_CLArrearForYear.Text = "ELIMINATE"
        End If

    End Sub

    Private Sub cbo_SLArrearForMonth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SLArrearForMonth.TextChanged
        If Trim(cbo_SLArrearForMonth.Text) = "CARRY ON" Then
            cbo_SLArrearForYear.Enabled = True
        Else
            cbo_SLArrearForYear.Enabled = False
            cbo_SLArrearForYear.Text = "ELIMINATE"
        End If
    End Sub

    Private Sub msk_OutTime_Shift1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_OutTime_Shift1.TextChanged
        msk_Working_Hours_Shift1.Text = getHourFromMinitues(msk_InTimeshift1.Text, msk_OutTime_Shift1.Text)
    End Sub

    Private Sub msk_OutTime_Shift2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_OutTime_Shift2.TextChanged
        msk_Working_Hours_Shift2.Text = getHourFromMinitues(msk_InTimeShift2.Text, msk_OutTime_Shift2.Text)
    End Sub

    Private Sub msk_OutTime_Shift3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_OutTime_Shift3.TextChanged
        msk_Working_Hours_Shift3.Text = getHourFromMinitues(msk_inTimeShift3.Text, msk_OutTime_Shift3.Text)
    End Sub

    Private Sub msk_Working_Hours_Shift1_MaskInputRejected(sender As System.Object, e As System.Windows.Forms.MaskInputRejectedEventArgs) Handles msk_Working_Hours_Shift1.MaskInputRejected

    End Sub

    Private Sub msk_Min_Time_One_Shift_1_GotFocus(sender As Object, e As System.EventArgs) Handles msk_Min_Time_One_Shift_1.GotFocus
        msk_Min_Time_One_Shift_1.SelectionStart = 0
        msk_Min_Time_One_Shift_1.SelectionLength = Len(msk_Min_Time_One_Shift_1.Text)
    End Sub

    Private Sub msk_Min_Time_One_Shift_2_GotFocus(sender As Object, e As System.EventArgs) Handles msk_Min_Time_One_Shift_2.GotFocus
        msk_Min_Time_One_Shift_2.SelectionStart = 0
        msk_Min_Time_One_Shift_2.SelectionLength = Len(msk_Min_Time_One_Shift_2.Text)
    End Sub

    Private Sub msk_Min_Time_One_Shift_3_GotFocus(sender As Object, e As System.EventArgs) Handles msk_Min_Time_One_Shift_3.GotFocus
        msk_Min_Time_One_Shift_3.SelectionStart = 0
        msk_Min_Time_One_Shift_3.SelectionLength = Len(msk_Min_Time_One_Shift_3.Text)
    End Sub

    Private Sub msk_Min_Time_Half_Shift_1_GotFocus(sender As Object, e As System.EventArgs) Handles msk_Min_Time_Half_Shift_1.GotFocus
        msk_Min_Time_Half_Shift_1.SelectionStart = 0
        msk_Min_Time_Half_Shift_1.SelectionLength = Len(msk_Min_Time_Half_Shift_1.Text)
    End Sub

    Private Sub msk_Min_Time_Half_Shift_2_GotFocus(sender As Object, e As System.EventArgs) Handles msk_Min_Time_Half_Shift_2.GotFocus
        msk_Min_Time_Half_Shift_2.SelectionStart = 0
        msk_Min_Time_Half_Shift_2.SelectionLength = Len(msk_Min_Time_Half_Shift_2.Text)
    End Sub

    Private Sub msk_Min_Time_Half_Shift_3_GotFocus(sender As Object, e As System.EventArgs) Handles msk_Min_Time_Half_Shift_3.GotFocus
        msk_Min_Time_Half_Shift_3.SelectionStart = 0
        msk_Min_Time_Half_Shift_3.SelectionLength = Len(msk_Min_Time_Half_Shift_3.Text)
    End Sub

End Class