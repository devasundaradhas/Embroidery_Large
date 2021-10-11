Public Class Payroll_Bonus_Entry_Details

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "BONUS-"
    'Private Pk_Condition2 As String = "AVLSD-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

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

    Dim MonthCnt As Integer = 14
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

        dtp_ToDate.Text = ""
        txt_BonusRate.Text = ""
        txt_MinAttendance.Text = ""
        txt_MaxShifts.Text = ""

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
        dgv_Details.CurrentCell.Selected = False
        dgv_Print_Details.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Bonus_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bonus_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Bonus_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bonus_Date").ToString
                cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, Val(dt1.Rows(0).Item("Salary_Payment_Type_IdNo").ToString))
                cbo_Category.Text = Common_Procedures.Category_IdNoToName(con, Val(dt1.Rows(0).Item("Category_IdNo").ToString))
                'cbo_Month.Text = Common_Procedures.Month_IdNoToName(con, Val(dt1.Rows(0).Item("Month_IdNo").ToString))
                dtp_FromDate.Text = dt1.Rows(0).Item("From_Date").ToString
                dtp_ToDate.Text = dt1.Rows(0).Item("To_Date").ToString

                txt_MaxShifts.Text = Val(dt1.Rows(0).Item("Max_Shifts").ToString)
                txt_MinShifts.Text = Val(dt1.Rows(0).Item("Min_Shifts").ToString)
                txt_MinAttendance.Text = Val(dt1.Rows(0).Item("Min_Att_Reqd").ToString)
                chk_ExcludeWO.Checked = IIf(dt1.Rows(0).Item("Exclude_WO") = True, True, False)
                chk_ExcludePH_LH.Checked = IIf(dt1.Rows(0).Item("Exclude_PH_LH") = True, True, False)
                txt_BonusRate.Text = Val(dt1.Rows(0).Item("Bonus_Rate").ToString)

                For i = 0 To 13
                    dgv_Details.Columns(11 + i).Visible = False
                    If Not IsDBNull(dt1.Rows(0).Item(13 + i)) Then
                        dgv_Details.Columns(11 + i).HeaderText = dt1.Rows(0).Item(13 + i)
                        If IsDate("1-" + Join(Split(dgv_Details.Columns(11 + i).HeaderText, " "), "")) Then
                            dgv_Details.Columns(11 + i).Visible = True
                        End If
                    End If
                Next

                'txt_MinShifts.Text = Val(dt1.Rows(0).Item("Min_Shifts").ToString)

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name,b.Card_No from PayRoll_Bonus_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  Where a.Bonus_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Employee_IdNo").ToString
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("Card_No").ToString

                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("Tot_Shifts").ToString
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Tot_Att").ToString
                            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Wage_Per_Day").ToString

                            .Rows(n).Cells(7).Value = dt2.Rows(i).Item("Total_Earnings").ToString
                            .Rows(n).Cells(8).Value = dt2.Rows(i).Item("Bonus_Rate").ToString
                            .Rows(n).Cells(9).Value = dt2.Rows(i).Item("Bonus_Earned").ToString
                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("Bonus_Finalised").ToString

                            For J As Integer = 0 To 13
                                .Rows(n).Cells(11 + J).Value = FormatNumber(dt2.Rows(i).Item(6 + J), 2)
                            Next J

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

            End If

            TotalBonus()
            Grid_Cell_DeSelect()

            'ShowOrHideColumns()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

        NoCalc_Status = False
    End Sub

    Private Sub get_Salary_Bonus_Details()

        'Dim cmd As New SqlClient.SqlCommand
        'Dim Da As New SqlClient.SqlDataAdapter
        'Dim Dt As New DataTable
        'Dim da1 As New SqlClient.SqlDataAdapter
        'Dim da2 As New SqlClient.SqlDataAdapter
        'Dim da3 As New SqlClient.SqlDataAdapter
        'Dim da4 As New SqlClient.SqlDataAdapter
        'Dim da5 As New SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim dt2 As New DataTable
        'Dim dt3 As New DataTable
        'Dim dt4 As New DataTable
        'Dim dt5 As New DataTable


        'Dim vEmp_IdNo As Integer = 0
        'Dim vCatgry_IdNo As Integer = 0
        'Dim vEmpCatgry_IdNo As Integer = 0
        'Dim n As Integer = 0
        'Dim vNoOf_Wrk_Dys_Frm_MessAtt As Integer = 0
        'Dim Late_Mins As Double = 0
        'Dim Late_Hours As Double = 0
        'Dim vNoOf_Wrkd_Dys As Double = 0
        'Dim OT_wrk_dys As Double = 0
        'Dim vIncenAmt_FromAtt As Double = 0
        'Dim vMas_BasSal_PerShift_PerMonth As Double = 0
        'Dim vSal_Shift As Double = 0
        'Dim Bas_Sal As Double = 0
        'Dim OT_Sal_Shft As Double = 0
        'Dim OT_Bonus As Double = 0
        'Dim Amt_OpBal As Double
        'Dim Cmp_Cond As String = ""
        'Dim mins_Adv As Double = 0
        'Dim mess_Ded As Double = 0

        'Dim DA_Amt As Double = 0, DA_Shft As Double = 0
        'Dim HRA_Amt As Double = 0
        'Dim Convey_Bonus_Amt As Double = 0
        'Dim Convey_PF_Amt As Double = 0
        'Dim Washing_Amt As Double = 0
        'Dim Entertainment_Amt As Double = 0
        'Dim Prrovision_Amt As Double = 0
        'Dim Maintain_Amt As Double = 0
        'Dim Other_add1_Amt As Double = 0
        'Dim Other_add2_Amt As Double = 0
        'Dim vMas_WeekOff_Allow_Amt_PerDay As Double = 0
        'Dim CL_Leaves As Double = 0
        'Dim SL_Leaves As Double = 0
        'Dim CL_Leaves_Current As Double = 0
        'Dim SL_Leaves_Current As Double = 0
        'Dim Less_CL_Leaves As Double = 0
        'Dim Less_SL_Leaves As Double = 0

        'Dim Advance_Ded_Entry As Double = 0
        'Dim Mess_Ded_Entry As Double = 0
        'Dim Medical_Ded_Entry As Double = 0
        'Dim Store_Ded_Entry As Double = 0
        'Dim Others_Add_Ded_Entry As Double = 0
        'Dim Others_Ded_Ded_Entry As Double = 0

        'Dim H As Long = 0, M As Long = 0
        'Dim OTHrs As String = ""
        'Dim OT_Mins As Long = 0, Tot_OTMins As Long = 0
        'Dim Ot_Dbl As Double = 0
        'Dim Ot_Int As Long = 0
        'Dim Ot_minVal As Long = 0
        'Dim Net_Bonus As Double = 0
        'Dim Net_Pay As Double = 0
        'Dim Bonus_Pending As Double = 0
        'Dim Ttl_Days As Double = 0
        'Dim SNo As Long = 0, Nr As Long = 0

        'Dim SalPymtTyp_IdNo As Integer = 0

        ''  Dim PrevEnt_RefDate1 As Date, PrevEnt_RefDate2 As Date
        'Dim PrevEnt_RefNo As String = ""
        'Dim EntOrdBy As Single = 0, PrevEnt_OrdBy As Single = 0
        'Dim AdvDtTm As Date
        'Dim NewCode As String = ""
        'Dim Shft_Hours As Double = 0
        'Dim Shft_Mins As Double = 0
        'Dim Sal_advance As Double = 0
        'Dim Att_Incentive As Double = 0
        'Dim Cat_Idno As Integer = 0
        'Dim CL_STS As Integer
        'Dim SL_STS As Integer
        'Dim thisMonth As Integer = 0
        'Dim dtc As Date
        'Dim Tot_LeaveDys_In_Mnth As Double = 0
        'Dim Tot_WeekOff_Days As Double = 0
        'Dim Tot_FH_Dys As Double = 0
        'Dim vNoOf_Att_Dys_In_FH As Double = 0
        'Dim vNoOf_FH_Dys_On_WkOff As Single = 0
        'Dim vNoOf_Att_Dys_In_WkOff As Double = 0
        'Dim vNoOf_FH_Dys_For_Sal As Single = 0
        'Dim Tot_Noof_WrkdShft_Frm_Att As Single = 0
        'Dim WeekOff_ADD_Opening As Single = 0
        'Dim WeekOff_LESS_Opening As Single = 0
        'Dim Dys As Integer = 0
        'Dim Late_sts As Boolean = False
        'Dim Late_Minimum_Mins As Integer = 0
        'Dim Late_Deduct_per_Mins As Single = 0
        'Dim vNet_LeaveDys_In_Mnth As Single
        'Dim vNo_Days_InMonth_for_MonthlyWages As Double = 0
        'Dim Basic_Salary_FOR_PF_CALCULATION As Double = 0
        'Dim Less_Advance_Col_Edit_STS As Boolean = True
        'Dim vTotErngs_FOR_ESI As String = 0
        'Dim vPFSTS_Sal As Integer = 0
        'Dim vESISTS_Sal As Integer = 0
        'Dim vPFSTS_Audit As Integer = 0
        'Dim vESISTS_Audit As Integer = 0
        'Dim weekof_mins As Integer = 0
        'Dim OT_Bonus_ESI As Double
        'Dim Bonus_plus_ot_esi As Double

        'If FrmLdSTS = True Then Exit Sub

        'Less_Advance_Col_Edit_STS = True

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'NoCalc_Status = True

        'ESI_MAX_SHFT_WAGES = 0
        'EPF_MAX_BASICPAY = 0
        'da1 = New SqlClient.SqlDataAdapter("select * from " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..Settings_Head", con)
        'dt1 = New DataTable
        'da1.Fill(dt1)
        'If dt1.Rows.Count > 0 Then
        '    If IsDBNull(dt1.Rows(0).Item("Basic_Wages_For_Esi").ToString()) = False Then
        '        ESI_MAX_SHFT_WAGES = Val(dt1.Rows(0).Item("Basic_Wages_For_Esi").ToString)
        '    End If
        '    If IsDBNull(dt1.Rows(0).Item("Basic_Pay_For_Epf").ToString()) = False Then
        '        EPF_MAX_BASICPAY = Val(dt1.Rows(0).Item("Basic_Pay_For_Epf").ToString)
        '    End If
        'End If
        'dt1.Dispose()
        'da1.Dispose()

        'If EPF_MAX_BASICPAY = 0 Then EPF_MAX_BASICPAY = 15000

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'cmd.Connection = con

        ''btn_Calculation_Bonus.BackColor = Color.Blue
        'Application.DoEvents()

        'SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

        'vCatgry_IdNo = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)

        'EntOrdBy = Val(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text)))

        ''txt_TotalDays.Text = DateDiff(DateInterval.Day, dtp_FromDate.Value.Date, dtp_ToDate.Value.Date) + 1


        'cmd.CommandText = "Truncate table EntryTemp"
        'cmd.ExecuteNonQuery()

        ''---Day Name from Previous Month To Next Month

        'dtc = dtp_FromDate.Value.Date.AddMonths(-1)

        'Do While (dtc <= dtp_ToDate.Value.Date.AddMonths(1))
        '    cmd.Parameters.Clear()
        '    cmd.Parameters.AddWithValue("@Date", dtc)
        '    cmd.CommandText = ("Insert into EntryTemp(Date1, name1) values (@Date, '" & Trim(UCase(Format(dtc, "dddd"))) & "')")

        '    cmd.ExecuteNonQuery()
        '    dtc = Format(dtc.AddDays(1), "dd/MM/yyyy")
        'Loop



        'cmd.Parameters.Clear()
        'cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
        'cmd.Parameters.AddWithValue("@BonusDate", dtp_Date.Value.Date)
        'cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
        'cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)


        ''====== No of Festival Holidays In this Month  ========== 

        ''txt_FestivalDays.Text = ""

        'cmd.CommandText = "select count(*) as NoOf_FH_Days from Holiday_Details where HolidayDateTime between @FromDate and @ToDate "
        'Da = New SqlClient.SqlDataAdapter(cmd)
        'dt4 = New DataTable
        'Da.Fill(dt4)
        'If dt4.Rows.Count > 0 Then
        '    If IsDBNull(dt4.Rows(0).Item("NoOf_FH_Days").ToString) = False Then
        '        'txt_FestivalDays.Text = Val(dt4.Rows(0).Item("NoOf_FH_Days").ToString)
        '    End If
        'End If
        'dt4.Clear()

        'Dim vSQLCondt As String = ""

        'vSQLCondt = "" ' " (a.Employee_IdNo = 144 or a.Employee_IdNo = 212 or a.Employee_IdNo = 210 or a.Employee_IdNo = 145 or a.Employee_IdNo = 230 or a.Employee_IdNo = 225 or a.Employee_IdNo = 288) "
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then
        '    vSQLCondt = Trim(vSQLCondt) & IIf(Trim(vSQLCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        'End If
        'If Val(SalPymtTyp_IdNo) <> 0 Then
        '    vSQLCondt = Trim(vSQLCondt) & IIf(Trim(vSQLCondt) <> "", " and ", "") & " a.Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo))
        'End If
        'If Val(vCatgry_IdNo) <> 0 Then
        '    vSQLCondt = Trim(vSQLCondt) & IIf(Trim(vSQLCondt) <> "", " and ", "") & " a.Category_IdNo = " & Str(Val(vCatgry_IdNo))
        'End If


        'cmd.CommandText = "select a.*, b.* from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo <> 0 and a.Category_IdNo = b.Category_IdNo Where " & vSQLCondt & IIf(vSQLCondt <> "", " and ", "") & " a.Join_DateTime <= @ToDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @FromDate ) ) order by a.Employee_Name"
        'da1 = New SqlClient.SqlDataAdapter(cmd)
        'dt1 = New DataTable
        'da1.Fill(dt1)

        'With dgv_Details

        '    .Rows.Clear()
        '    SNo = 0

        '    If dt1.Rows.Count > 0 Then

        '        '---Progress Bar
        '        pnl_ProgressBar.Visible = True

        '        ProgBar1.Minimum = 0
        '        ProgBar1.Maximum = dt1.Rows.Count - 1

        '        '------------

        '        For i = 0 To dt1.Rows.Count - 1

        '            lbl_ProPerc.Text = CInt((100 / Val(dt1.Rows.Count)) * i) & "%"
        '            Application.DoEvents()
        '            ProgBar1.Value = i

        '            vEmp_IdNo = Val(dt1.Rows(i).Item("Employee_IdNo").ToString)
        '            vEmpCatgry_IdNo = Val(dt1.Rows(i).Item("Category_IdNo").ToString)


        '            vNo_Days_InMonth_for_MonthlyWages = 26
        '            If Val(Common_Procedures.settings.NoOfDays_For_Month_Wages_Take_TotalDays_In_Month) = 1 Then
        '                'vNo_Days_InMonth_for_MonthlyWages = Val(txt_TotalDays.Text)

        '            Else

        '                If Val(dt1.Rows(i).Item("No_Days_Month_Wages").ToString) <> 0 Then
        '                    vNo_Days_InMonth_for_MonthlyWages = Val(dt1.Rows(i).Item("No_Days_Month_Wages").ToString)
        '                End If

        '            End If

        '            'If Val(vEmp_IdNo) = 539 Then
        '            '    Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
        '            'End If

        '            cmd.Parameters.Clear()
        '            cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
        '            cmd.Parameters.AddWithValue("@BonusDate", dtp_Date.Value.Date)
        '            cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
        '            cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

        '            'If dtp_Advance_UpToDate.Visible = True Then
        '            '    cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_Advance_UpToDate.Value.Date)
        '            'Else
        '            '    cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_ToDate.Value.Date)
        '            'End If

        '            thisMonth = 0
        '            'If dtp_Date.Value.Date > Common_Procedures.Company_FromDate And dtp_Date.Value.Date < Common_Procedures.Company_ToDate And Trim(cbo_Month.Text) = "MARCH" Then
        '            '    thisMonth = Month(dtp_Date.Value.Date)
        '            'End If

        '            Late_sts = IIf(Val(dt1.Rows(i).Item("Time_Delay").ToString) = 1, True, False)
        '            Late_Minimum_Mins = Val(dt1.Rows(i).Item("Minimum_Delay").ToString)
        '            Late_Deduct_per_Mins = Val(dt1.Rows(i).Item("Less_Minute_Delay").ToString)

        '            '----Calculating No Of Leave days For Monthly wages

        '            cmd.CommandText = "Truncate table ReportTemp"
        '            cmd.ExecuteNonQuery()

        '            cmd.CommandText = "Insert into ReportTemp( Int1             , Meters1 ) " & _
        '                                          " Select      a.Employee_IdNo , 0      from PayRoll_Employee_Head a Where a.Employee_IdNo =" & Str(Val(vEmp_IdNo))
        '            Nr = cmd.ExecuteNonQuery()

        '            '----getting No Of days absent from attendance
        '            cmd.CommandText = "Insert into ReportTemp( Int1                ,         Meters1        ) " & _
        '                                          " Select      a.Employee_IdNo    , count(a.No_Of_Shift)   from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c  Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift = 0 and b.shift_Day_Month = 'MONTH' and a.Employee_IdNo = b.Employee_IdNo and a.Employee_Attendance_Date = c.Date1 and b.Week_Off <> c.Name1 and a.Employee_Attendance_Date NOT IN (select z1.HolidayDateTime from Holiday_Details z1) group by a.Employee_IdNo"
        '            Nr = cmd.ExecuteNonQuery()

        '            '--- Suppose attendance not entered on that date
        '            cmd.CommandText = "Insert into ReportTemp(Int1        ,   Meters1 )    " & _
        '                                        "select    a.Employee_IdNo, count(*)  from PayRoll_Employee_Head a, EntryTemp b where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and b.Date1 Between @FromDate and @ToDate and b.Date1 NOT IN (select z1.Employee_Attendance_Date from PayRoll_Employee_Attendance_Details z1 where z1.Employee_Attendance_Date between @FromDate and @ToDate and z1.Employee_IdNo = a.Employee_IdNo) and a.Week_Off <> b.name1 and b.Date1 NOT IN (select z2.HolidayDateTime from Holiday_Details z2) group by a.Employee_IdNo"
        '            Nr = cmd.ExecuteNonQuery()

        '            '----getting No Of days greater than 1 (ie 1.5 shift) and reducing it in leave
        '            cmd.CommandText = "Insert into ReportTemp( Int1                ,         Meters1        ) " & _
        '                                          " Select      a.Employee_IdNo    , (1 - a.No_Of_Shift)   from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift > 1 and b.shift_Day_Month = 'MONTH' and a.Employee_IdNo = b.Employee_IdNo and a.Employee_Attendance_Date = c.Date1 and b.Week_Off <> c.Name1 and a.Employee_Attendance_Date NOT IN (select z1.HolidayDateTime from Holiday_Details z1) "
        '            cmd.ExecuteNonQuery()

        '            '----getting No Of days Lesser than 1(ie half shift) and adding it in leave
        '            cmd.CommandText = "Insert into ReportTemp( Int1                ,         Meters1        ) " & _
        '                                          " Select      a.Employee_IdNo    , (1 - a.No_Of_Shift)   from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and (a.No_Of_Shift > 0 and a.No_Of_Shift < 1) and b.shift_Day_Month = 'MONTH' and a.Employee_IdNo = b.Employee_IdNo and a.Employee_Attendance_Date = c.Date1 and b.Week_Off <> c.Name1 and a.Employee_Attendance_Date NOT IN (select z1.HolidayDateTime from Holiday_Details z1) "
        '            Nr = cmd.ExecuteNonQuery()

        '            '---- No.Of.leave - for month Wages

        '            Tot_LeaveDys_In_Mnth = 0

        '            cmd.CommandText = "select sum(Meters1) as NoOfLeave from Reporttemp Where int1 = " & Str(Val(vEmp_IdNo))
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0).Item("NoOfLeave").ToString) = False Then
        '                    Tot_LeaveDys_In_Mnth = Val(dt4.Rows(0).Item("NoOfLeave").ToString)
        '                End If
        '            End If
        '            dt4.Clear()

        '            '======Getting Total WeekOff Days fo this employee ========== 

        '            Tot_WeekOff_Days = 0

        '            cmd.CommandText = "select count(*) AS WeekOff from PayRoll_Employee_Head a, EntryTemp b where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and " & _
        '                                    " b.Date1 Between @FromDate and @ToDate and a.Week_Off = b.name1"
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0).Item("WeekOff").ToString) = False Then
        '                    Tot_WeekOff_Days = Str(Val(dt4.Rows(0).Item("WeekOff").ToString))
        '                End If
        '            End If
        '            dt4.Clear()


        '            '====== No of Festival Holidays In this Month  ========== 

        '            Tot_FH_Dys = 0
        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '----Southern Cot Spinners
        '                cmd.CommandText = "Select count(*) as NoOf_FH_Days from PayRoll_Employee_Head a, Holiday_Details b Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and b.HolidayDateTime between @FromDate and @ToDate and a.Employee_IdNo IN (select sq1.Employee_IdNo from PayRoll_Employee_Attendance_Details sq1 where sq1.Employee_Attendance_Date = (select max(z1.Date1) from EntryTemp z1 Where z1.Date1 between @FromDate and @ToDate and z1.Name1 <> a.Week_Off and z1.Date1 Between @FromDate and dateadd(dd, -1, b.HolidayDateTime)) and sq1.No_Of_Shift > 0)  and a.Employee_IdNo IN (select sq2.Employee_IdNo from PayRoll_Employee_Attendance_Details sq2 where sq2.Employee_Attendance_Date = (select min(z2.Date1) from EntryTemp z2 Where z2.Date1 between @FromDate and @ToDate and a.Week_Off <> z2.Name1 and z2.Date1 Between dateadd(dd, 1, b.HolidayDateTime) and @ToDate) and sq2.No_Of_Shift > 0)"
        '                Da = New SqlClient.SqlDataAdapter(cmd)
        '                dt4 = New DataTable
        '                Da.Fill(dt4)
        '                If dt4.Rows.Count > 0 Then
        '                    If IsDBNull(dt4.Rows(0).Item("NoOf_FH_Days").ToString) = False Then
        '                        Tot_FH_Dys = Str(Val(dt4.Rows(0).Item("NoOf_FH_Days").ToString))
        '                    End If
        '                End If
        '                dt4.Clear()

        '            Else
        '                'Tot_FH_Dys = Val(txt_FestivalDays.Text)

        '            End If


        '            '====== No of Festival Holidays In WeekOf for this Employee ==========

        '            vNoOf_FH_Dys_On_WkOff = 0
        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '----Southern Cot Spinners
        '                cmd.CommandText = "Select count(*) as NoOf_FHDays_In_WeekOFF from PayRoll_Employee_Head a, Holiday_Details b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and b.HolidayDateTime between @FromDate and @ToDate and c.Date1 between @FromDate and @ToDate and c.Name1 = a.Week_Off and b.HolidayDateTime = c.Date1 and a.Employee_IdNo IN (select sq1.Employee_IdNo from PayRoll_Employee_Attendance_Details sq1 where sq1.Employee_Attendance_Date = (select max(z1.Date1) from EntryTemp z1 Where z1.Date1 between @FromDate and @ToDate and z1.Name1 <> a.Week_Off and z1.Date1 Between @FromDate and dateadd(dd, -1, b.HolidayDateTime)) and sq1.No_Of_Shift > 0)  and a.Employee_IdNo IN (select sq2.Employee_IdNo from PayRoll_Employee_Attendance_Details sq2 where sq2.Employee_Attendance_Date = (select min(z2.Date1) from EntryTemp z2 Where z2.Date1 between @FromDate and @ToDate and a.Week_Off <> z2.Name1 and z2.Date1 Between dateadd(dd, 1, b.HolidayDateTime) and @ToDate) and sq2.No_Of_Shift > 0)"
        '                Da = New SqlClient.SqlDataAdapter(cmd)
        '                dt4 = New DataTable
        '                Da.Fill(dt4)
        '                If dt4.Rows.Count > 0 Then
        '                    If IsDBNull(dt4.Rows(0).Item("NoOf_FHDays_In_WeekOFF").ToString) = False Then
        '                        vNoOf_FH_Dys_On_WkOff = Str(Val(dt4.Rows(0).Item("NoOf_FHDays_In_WeekOFF").ToString))
        '                    End If
        '                End If
        '                dt4.Clear()

        '            Else
        '                cmd.CommandText = "select count(*) as NoOf_FHDays_In_WeekOFF from PayRoll_Employee_Head a, Holiday_Details b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and b.HolidayDateTime between @FromDate and @ToDate and c.Date1 between @FromDate and @ToDate and c.Name1 = a.Week_Off and b.HolidayDateTime = c.Date1"
        '                Da = New SqlClient.SqlDataAdapter(cmd)
        '                dt4 = New DataTable
        '                Da.Fill(dt4)
        '                If dt4.Rows.Count > 0 Then
        '                    If IsDBNull(dt4.Rows(0).Item("NoOf_FHDays_In_WeekOFF").ToString) = False Then
        '                        vNoOf_FH_Dys_On_WkOff = Str(Val(dt4.Rows(0).Item("NoOf_FHDays_In_WeekOFF").ToString))
        '                    End If
        '                End If
        '                dt4.Clear()

        '            End If

        '            Tot_FH_Dys = Tot_FH_Dys - vNoOf_FH_Dys_On_WkOff


        '            '====== No of days Present in Festival Holidays for this Employee and not in week off ==========

        '            vNoOf_Att_Dys_In_FH = 0
        '            cmd.CommandText = "Select count(*) as Attandance_In_FH_Days from PayRoll_Employee_Attendance_Details a, Holiday_Details b, PayRoll_Employee_Head c, EntryTemp d Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and b.HolidayDateTime between @FromDate and @ToDate and a.Employee_IdNo = c.Employee_IdNo and a.Employee_Attendance_Date = b.HolidayDateTime and d.Date1 between @FromDate and @ToDate and a.Employee_Attendance_Date = d.Date1 and c.Week_Off <> d.Name1 "
        '            'cmd.CommandText = "Select count(*) as Attandance_In_FH_Days from PayRoll_Employee_Attendance_Details a, Holiday_Details b Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and b.HolidayDateTime between @FromDate and @ToDate and a.Employee_Attendance_Date = b.HolidayDateTime"
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0).Item("Attandance_In_FH_Days").ToString) = False Then
        '                    vNoOf_Att_Dys_In_FH = Str(Val(dt4.Rows(0).Item("Attandance_In_FH_Days").ToString))
        '                End If
        '            End If
        '            dt4.Clear()

        '            '====== No of days Present in WeekOff for this Employee, if weekoff and fh in same it is taken in week of days onlys ==========

        '            vNoOf_Att_Dys_In_WkOff = 0
        '            cmd.CommandText = "Select count(*) as Attandance_In_WeekOff_Days from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and a.Employee_IdNo = b.Employee_IdNo and c.Date1 between @FromDate and @ToDate and a.Employee_Attendance_Date = c.Date1 and b.Week_Off = c.Name1 "
        '            'cmd.CommandText = "Select count(*) as Attandance_In_WeekOff_Days from PayRoll_Employee_Attendance_Details a, PayRoll_Employee_Head b, EntryTemp c Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @FromDate and @ToDate and a.No_Of_Shift <> 0 and a.Employee_IdNo = b.Employee_IdNo and c.Date1 between @FromDate and @ToDate and a.Employee_Attendance_Date = c.Date1 and b.Week_Off = c.Name1 and a.Employee_Attendance_Date NOT IN (select z1.HolidayDateTime from Holiday_Details z1) "
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0).Item("Attandance_In_WeekOff_Days").ToString) = False Then
        '                    vNoOf_Att_Dys_In_WkOff = Str(Val(dt4.Rows(0).Item("Attandance_In_WeekOff_Days").ToString))
        '                End If
        '            End If
        '            dt4.Clear()


        '            '====== taking Opening Weekoff from Previous Month Bonus Details ==========
        '            WeekOff_ADD_Opening = 0
        '            WeekOff_LESS_Opening = 0

        '            If Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 1 Then   '---- CarryOn Previous Month WeekOff
        '                cmd.CommandText = "select sum(a.Add_W_Off_CR) as Add_WeekOff_Opening ,sum(a.Less_W_Off_CR) as Less_WeekOff_Opening  from PayRoll_Bonus_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Bonus_Date < @fromdate  and  a.Bonus_Date >= @CompFromDate "
        '                Da = New SqlClient.SqlDataAdapter(cmd)
        '                dt4 = New DataTable
        '                Da.Fill(dt4)
        '                If dt4.Rows.Count > 0 Then
        '                    If IsDBNull(dt4.Rows(0).Item("Add_WeekOff_Opening").ToString) = False Then
        '                        If Val(dt4.Rows(0).Item("Add_WeekOff_Opening").ToString) <> 0 Then
        '                            WeekOff_ADD_Opening = Str(Val(dt4.Rows(0).Item("Add_WeekOff_Opening").ToString))
        '                        End If
        '                    End If
        '                    If IsDBNull(dt4.Rows(0).Item("Less_WeekOff_Opening").ToString) = False Then
        '                        If Val(dt4.Rows(0).Item("Less_WeekOff_Opening").ToString) <> 0 Then
        '                            WeekOff_LESS_Opening = Str(Val(dt4.Rows(0).Item("Less_WeekOff_Opening").ToString))
        '                        End If
        '                    End If

        '                End If

        '            End If
        '            dt4.Clear()


        '            '-----Current Month CL ,SL Leave  Status
        '            CL_STS = Val(dt1.Rows(i).Item("CL_Leave").ToString)
        '            SL_STS = Val(dt1.Rows(i).Item("SL_Leave").ToString)

        '            CL_Leaves = 0
        '            SL_Leaves = 0
        '            CL_Leaves_Current = 0
        '            SL_Leaves_Current = 0
        '            Less_CL_Leaves = 0
        '            Less_SL_Leaves = 0

        '            '---CL ,SL Opening From Opening Entry
        '            cmd.CommandText = "select sum(a.Opening_CL_Leaves) as CL_Opening  ,sum(a.Opening_ML_Leaves) as SL_Opening from PayRoll_Employee_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo))
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
        '                    If Trim(dt1.Rows(i).Item("CL_Arrear_Type_Year").ToString) = "CARRY ON" Or Trim(dt1.Rows(i).Item("CL_Arrear_Type_Year").ToString) = "Bonus" Then  '-----Opening CL Leaves Carry on 
        '                        If Val(dt4.Rows(0).Item("CL_Opening").ToString) <> 0 Then
        '                            If CL_STS <> 0 Then
        '                                CL_Leaves = Str(Val(dt4.Rows(0).Item("CL_Opening").ToString))
        '                            Else
        '                                CL_Leaves = 0
        '                            End If
        '                        Else
        '                            CL_Leaves = 0
        '                        End If

        '                    End If
        '                    If Trim(dt1.Rows(i).Item("SL_Arrear_Type_Year").ToString) = "CARRY ON" Or Trim(dt1.Rows(i).Item("SL_Arrear_Type_Year").ToString) = "Bonus" Then
        '                        If Val(dt4.Rows(0).Item("SL_Opening").ToString) <> 0 Then
        '                            If SL_STS <> 0 Then
        '                                SL_Leaves = Str(Val(dt4.Rows(0).Item("SL_Opening").ToString))
        '                            Else
        '                                SL_Leaves = 0
        '                            End If

        '                        End If
        '                    End If

        '                End If
        '            End If
        '            dt4.Clear()


        '            '-----Opening CL ,SL from Previous Month
        '            cmd.CommandText = "select sum(a.Add_CL_Leaves) as CL_Opening  ,sum(a.Add_SL_Leaves) as SL_Opening , sum(a.Less_CL_CR_Days) as UsedCL ,sum(a.Less_SL_CR_Days) as UsedSL from PayRoll_Bonus_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Bonus_Date < @fromdate  and  a.Bonus_Date >= @CompFromDate "
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If Trim(dt1.Rows(i).Item("CL_Arrear_Type").ToString) = "CARRY ON" Then   '----CarryOn Previous Month CL
        '                    If IsDBNull(dt4.Rows(0).Item("CL_Opening").ToString) = False Then
        '                        If Val(dt4.Rows(0).Item("CL_Opening").ToString) <> 0 Then
        '                            If CL_STS <> 0 Then
        '                                CL_Leaves = CL_Leaves + Str(Val(dt4.Rows(0).Item("CL_Opening").ToString))
        '                            Else
        '                                CL_Leaves = 0
        '                            End If
        '                        End If
        '                    End If
        '                    If IsDBNull(dt4.Rows(0).Item("UsedCL").ToString) = False Then
        '                        If Val(dt4.Rows(0).Item("UsedCL").ToString) <> 0 Then
        '                            If CL_STS <> 0 Then
        '                                Less_CL_Leaves = Str(Val(dt4.Rows(0).Item("UsedCL").ToString))
        '                            Else
        '                                Less_CL_Leaves = 0
        '                            End If
        '                        End If
        '                    End If
        '                Else
        '                    CL_Leaves = 0
        '                    Less_CL_Leaves = 0

        '                End If

        '                If Trim(dt1.Rows(i).Item("SL_Arrear_Type").ToString) = "CARRY ON" Then
        '                    If IsDBNull(dt4.Rows(0).Item("SL_Opening").ToString) = False Then
        '                        If Val(dt4.Rows(0).Item("SL_Opening").ToString) <> 0 Then
        '                            If SL_STS <> 0 Then
        '                                SL_Leaves = SL_Leaves + Str(Val(dt4.Rows(0).Item("SL_Opening").ToString))
        '                            Else
        '                                SL_Leaves = 0
        '                            End If
        '                        End If
        '                    End If

        '                    If IsDBNull(dt4.Rows(0).Item("UsedSL").ToString) = False Then
        '                        If Val(dt4.Rows(0).Item("UsedSL").ToString) <> 0 Then
        '                            If SL_STS <> 0 Then
        '                                Less_SL_Leaves = Str(Val(dt4.Rows(0).Item("UsedSL").ToString))
        '                            Else
        '                                Less_SL_Leaves = 0
        '                            End If
        '                        End If
        '                    End If

        '                Else
        '                    SL_Leaves = 0
        '                    Less_SL_Leaves = 0

        '                End If

        '            End If
        '            dt4.Clear()

        '            If Val(vEmp_IdNo) = 136 Then
        '                Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
        '            End If

        '            AdvDtTm = #1/1/1990#
        '            cmd.CommandText = "Select b.Advance_UptoDate from PayRoll_Bonus_Details a INNER JOIN PayRoll_Bonus_Head b ON a.Bonus_Code = b.Bonus_Code Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and (a.Bonus_Date < @BonusDate or (a.Bonus_Date = @BonusDate and a.for_OrderBy < " & Str(Val(EntOrdBy)) & ") ) order by  b.Advance_UptoDate desc ,a.Bonus_Date desc"
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
        '                    If IsDate(dt4.Rows(0)(0).ToString) = True Then
        '                        AdvDtTm = dt4.Rows(0)(0)
        '                    End If
        '                End If
        '            End If
        '            dt4.Clear()

        '            AdvDtTm = DateAdd(DateInterval.Day, 1, AdvDtTm)
        '            cmd.Parameters.AddWithValue("@PreviousAdvanceDate", AdvDtTm)

        '            Amt_OpBal = 0

        '            '---- Opening Advance Amount
        '            cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.voucher_date < @PreviousAdvanceDate and a.Entry_Identification <> '" & Trim(Pk_Condition) & Trim(Val(vEmp_IdNo)) & "/" & Trim(NewCode) & "' and a.Entry_Identification <> '" & Trim(Pk_Condition2) & Trim(Val(vEmp_IdNo)) & "/" & Trim(NewCode) & "' and a.ledger_idno = b.ledger_idno and a.company_idno = tZ.company_idno and (a.Voucher_Code LIKE 'ADVOP-%' or a.Voucher_Code LIKE 'EADPY-%' or a.Voucher_Code LIKE 'ADVLS-%' or a.Voucher_Code LIKE 'ESAPY-%' or a.Voucher_Code LIKE 'AVLSD-%')"
        '            'cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.voucher_date <= @AdvanceUpToDate and a.Entry_Identification <> '" & Trim(Pk_Condition) & Trim(Val(vEmp_IdNo)) & "/" & Trim(NewCode) & "' and a.Entry_Identification <> '" & Trim(Pk_Condition2) & Trim(Val(vEmp_IdNo)) & "/" & Trim(NewCode) & "' and a.ledger_idno = b.ledger_idno and a.company_idno = tZ.company_idno and (a.Voucher_Code LIKE 'ADVOP-%' or a.Voucher_Code LIKE 'EADPY-%' or a.Voucher_Code LIKE 'ADVLS-%' or a.Voucher_Code LIKE 'ESAPY-%' or a.Voucher_Code LIKE 'AVLSD-%')"
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
        '                    If Val(dt4.Rows(0)(0).ToString) < 0 Then
        '                        Amt_OpBal = Format(-1 * Val(dt4.Rows(0)(0).ToString), "##########0.00")
        '                    End If
        '                End If
        '            End If
        '            dt4.Clear()


        '            Bonus_Pending = 0
        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL

        '                '-----Bonus Pending for previous Month
        '                cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Pending from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <>0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Voucher_Date < @PreviousAdvanceDate and  (a.Voucher_Code NOT LIKE 'ADVOP-%' and a.Voucher_Code NOT LIKE 'EADPY-%' and a.Voucher_Code NOT LIKE 'ADVLS-%' and a.Voucher_Code NOT LIKE 'ESAPY-%' and a.Voucher_Code NOT LIKE 'AVLSD-%') "
        '                Da = New SqlClient.SqlDataAdapter(cmd)
        '                dt4 = New DataTable
        '                Da.Fill(dt4)
        '                If dt4.Rows.Count > 0 Then
        '                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
        '                        'If Val(dt4.Rows(0).Item("Sal_Pending").ToString) > 0 Then
        '                        Bonus_Pending = Format(Math.Abs(Val(dt4.Rows(0)(0).ToString)), "##########0.00")
        '                        'End If
        '                    End If
        '                End If
        '                dt4.Clear()


        '                cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Paid_Amt from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Voucher_Date between @PreviousAdvanceDate and @ToDate and a.Entry_Identification LIKE 'ESLPY-%'"
        '                Da = New SqlClient.SqlDataAdapter(cmd)
        '                dt4 = New DataTable
        '                Da.Fill(dt4)
        '                If dt4.Rows.Count > 0 Then
        '                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
        '                        'If Val(dt4.Rows(0).Item("cr_Amt").ToString) > 0 Then
        '                        Bonus_Pending = Format((Bonus_Pending + Math.Abs(Val(dt4.Rows(0)(0).ToString))), "##########0.00")
        '                        'End If
        '                    End If
        '                End If
        '                dt4.Clear()

        '            End If


        '            Sal_advance = 0
        '            cmd.CommandText = "Select sum(a.Amount) as Sal_Advance from PayRoll_Employee_Payment_Head a Where  a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Payment_Date between @PreviousAdvanceDate and @AdvanceUpToDate and a.Advance_Bonus <> 'Bonus'"
        '            'cmd.CommandText = "Select sum(a.Voucher_Amount) as Sal_Advance from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Voucher_Date between @PreviousAdvanceDate and @AdvanceUpToDate and a.Voucher_Amount < 0 and a.Entry_Identification LIKE 'ESAPY-%'"
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
        '                    Sal_advance = Format(Math.Abs(Val(dt4.Rows(0).Item("Sal_Advance").ToString)), "##########0.00")
        '                End If
        '            End If
        '            dt4.Clear()

        '            'Amt_OpBal = Amt_OpBal - Sal_advance

        '            '====== taking Working Days, Daily Incentive amount, Mess Attendance from Employee Attendance ==========

        '            Tot_Noof_WrkdShft_Frm_Att = 0
        '            vIncenAmt_FromAtt = 0
        '            vNoOf_Wrk_Dys_Frm_MessAtt = 0

        '            cmd.CommandText = "select sum(a.No_Of_Shift) as Noof_Working_Days, sum(a.Mess_Attendance) as Mess_AttDays, Sum(a.Incentive_Amount) as IncenAmt from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Employee_Attendance_Date between @fromdate and @toDate "
        '            da2 = New SqlClient.SqlDataAdapter(cmd)
        '            dt2 = New DataTable
        '            da2.Fill(dt2)
        '            If dt2.Rows.Count > 0 Then
        '                If IsDBNull(dt2.Rows(0).Item("Noof_Working_Days").ToString) = False Then
        '                    Tot_Noof_WrkdShft_Frm_Att = Val(dt2.Rows(0).Item("Noof_Working_Days").ToString)
        '                End If
        '                If IsDBNull(dt2.Rows(0).Item("IncenAmt").ToString) = False Then
        '                    vIncenAmt_FromAtt = Format(Val(dt2.Rows(0).Item("IncenAmt").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt2.Rows(0).Item("Mess_AttDays").ToString) = False Then
        '                    vNoOf_Wrk_Dys_Frm_MessAtt = Val(dt2.Rows(0).Item("Mess_AttDays").ToString)
        '                End If
        '            End If
        '            dt2.Clear()


        '            '------Late Mins
        '            Late_Mins = 0
        '            cmd.CommandText = "select SUM(A.Late_Minutes) AS LATE_MINS from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo where A.Late_Minutes > ( SELECT sum(zs1.Minimum_Delay) FROM PayRoll_Category_Head zs1 where zs1.Category_IdNo = " & Str(Val(vEmpCatgry_IdNo)) & ") and a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Attendance_Date between @fromdate and @toDate "
        '            da2 = New SqlClient.SqlDataAdapter(cmd)
        '            dt2 = New DataTable
        '            da2.Fill(dt2)
        '            If dt2.Rows.Count > 0 Then
        '                If IsDBNull(dt2.Rows(0).Item("LATE_MINS").ToString) = False Then
        '                    Late_Mins = Val(dt2.Rows(0).Item("LATE_MINS").ToString)
        '                End If
        '            End If
        '            dt2.Clear()

        '            ''-------Ot mins
        '            'OT_Mins = 0
        '            'cmd.CommandText = "select Sum(A.OT_Minutes) as Ot_Mins from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where A.OT_Minutes > ( SELECT sum(zs1.OT_Allowed_After_Minutes) FROM PayRoll_Category_Head zs1  where zs1.Category_IdNo =  " & Str(Val(vEmpCatgry_IdNo)) & " ) and  a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Attendance_Date between @fromdate and @toDate "
        '            'da2 = New SqlClient.SqlDataAdapter(cmd)
        '            'dt2 = New DataTable
        '            'da2.Fill(dt2)
        '            'If dt2.Rows.Count > 0 Then
        '            '    If IsDBNull(dt2.Rows(0).Item("Ot_Mins").ToString) = False Then
        '            '        OT_Mins = Val(dt2.Rows(0).Item("Ot_Mins").ToString)
        '            '    End If
        '            'End If
        '            'dt2.Clear()

        '            '-------Ot mins FROM OT ENTRY
        '            OT_Mins = 0
        '            cmd.CommandText = "select Sum(A.OT_Minutes) as Ot_Mins from Payroll_Employee_OverTime_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where A.OT_Minutes > ( SELECT sum(zs1.OT_Allowed_After_Minutes) FROM PayRoll_Category_Head zs1  where zs1.Category_IdNo =  " & Str(Val(vEmpCatgry_IdNo)) & " ) and  a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Timing_OverTime_Date between @fromdate and @toDate "
        '            da2 = New SqlClient.SqlDataAdapter(cmd)
        '            dt2 = New DataTable
        '            da2.Fill(dt2)
        '            If dt2.Rows.Count > 0 Then
        '                If IsDBNull(dt2.Rows(0).Item("Ot_Mins").ToString) = False Then
        '                    OT_Mins = Val(dt2.Rows(0).Item("Ot_Mins").ToString)
        '                End If
        '            End If
        '            dt2.Clear()



        '            cmd.CommandText = "truncate table EntryTemp_Simple"
        '            cmd.ExecuteNonQuery()
        '            Nr = 0
        '            cmd.CommandText = "Insert into EntryTemp_Simple(  Int1         ,       Currency1                                 ,       Currency2     ,        Currency3           ,   Currency4           ,       Currency5                  ,       Currency6                            ) " & _
        '                                "select                     a.Employee_IdNo, sum(a.Advance_Deduction_Amount) as advance_ded  , sum(a.Mess) as Mess , sum(a.Medical) as Medical  , sum(a.Store) as Store , sum(a.Other_Addition) Others_Add , sum(a.Other_Deduction_Amount) as others_Ded  from PayRoll_Employee_Deduction_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Deduction_Date between @fromdate and @toDate group by a.Employee_IdNo"
        '            Nr = cmd.ExecuteNonQuery()

        '            Nr = 0
        '            cmd.CommandText = "Insert into EntryTemp_Simple(  Int1         ,       Currency1                                 ,       Currency2     ,        Currency3           ,   Currency4           ,       Currency5                  ,       Currency6                            ) " & _
        '                                "select                     a.Employee_IdNo, sum(a.Advance_Deduction_Amount) as advance_ded  , sum(a.Mess) as Mess , sum(a.Medical) as Medical  , sum(a.Store) as Store , sum(a.Other_Addition) Others_Add , sum(a.Other_Deduction) as others_Ded  from PayRoll_Employee_Deduction_Head a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Deduction_Date between @fromdate and @toDate  group by a.Employee_IdNo"
        '            Nr = cmd.ExecuteNonQuery()

        '            '----- Advance Deduction , Mess Amount , Medical amount , Store Amount , Other Additions and Otehr Deduction from Addition/Deduction Entry
        '            cmd.CommandText = "select sum(Currency1) as advance_ded  , sum(Currency2) as Mess , sum(Currency3) as Medical  ,sum(Currency4) as Store  ,sum(Currency5) Others_Add , sum(Currency6) as others_Ded  from EntryTemp_Simple "
        '            'cmd.CommandText = "select sum(a.Advance_Deduction_Amount) as advance_ded  , sum(a.Mess) as Mess , sum(a.Medical) as Medical  ,sum(a.Store) as Store  ,sum(a.Other_Addition) Others_Add ,sum(a.Other_Deduction) as others_Ded  from PayRoll_Employee_Deduction_Head a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and Employee_Deduction_Date between @fromdate and @toDate "
        '            da2 = New SqlClient.SqlDataAdapter(cmd)
        '            dt2 = New DataTable
        '            da2.Fill(dt2)
        '            If dt2.Rows.Count > 0 Then
        '                If IsDBNull(dt2.Rows(0).Item("advance_ded").ToString) = False Then
        '                    Advance_Ded_Entry = Val(dt2.Rows(0).Item("advance_ded").ToString)
        '                End If
        '                If IsDBNull(dt2.Rows(0).Item("Mess").ToString) = False Then
        '                    Mess_Ded_Entry = Val(dt2.Rows(0).Item("Mess").ToString)
        '                End If
        '                If IsDBNull(dt2.Rows(0).Item("Medical").ToString) = False Then
        '                    Medical_Ded_Entry = Format(Val(dt2.Rows(0).Item("Medical").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt2.Rows(0).Item("Store").ToString) = False Then
        '                    Store_Ded_Entry = Val(dt2.Rows(0).Item("Store").ToString)
        '                End If
        '                If IsDBNull(dt2.Rows(0).Item("Others_Add").ToString) = False Then
        '                    Others_Add_Ded_Entry = Val(dt2.Rows(0).Item("Others_Add").ToString)
        '                End If
        '                If IsDBNull(dt2.Rows(0).Item("others_Ded").ToString) = False Then
        '                    Others_Ded_Ded_Entry = Val(dt2.Rows(0).Item("others_Ded").ToString)
        '                End If
        '            End If
        '            dt2.Clear()


        '            '----getting Bonus details from Employee Head
        '            '----Bonus Per Day/Shift ,OT Bonus , DA ,HRA , Mess Deduction , Conveyance ,Washing ,Maintanance , Entertainment  ,CL , SL leaves

        '            vMas_BasSal_PerShift_PerMonth = 0
        '            OT_Sal_Shft = 0
        '            OT_Bonus = 0
        '            mess_Ded = 0
        '            DA_Amt = 0
        '            HRA_Amt = 0
        '            Convey_PF_Amt = 0
        '            Convey_Bonus_Amt = 0
        '            Washing_Amt = 0
        '            Maintain_Amt = 0
        '            Other_add1_Amt = 0
        '            Other_add2_Amt = 0
        '            vMas_WeekOff_Allow_Amt_PerDay = 0
        '            Entertainment_Amt = 0
        '            Prrovision_Amt = 0

        '            cmd.CommandText = "SELECT TOP 1 * from PayRoll_Employee_Salary_Details a Where a.employee_idno = " & Str(Val(vEmp_IdNo)) & " and ( ( @FromDate < (select min(y.From_DateTime) from PayRoll_Employee_Salary_Details y where y.employee_idno = a.employee_idno )) or (@FromDate BETWEEN a.From_DateTime and a.To_DateTime) or ( @FromDate >= (select max(z.From_DateTime) from PayRoll_Employee_Salary_Details z where z.employee_idno = a.employee_idno ))) order by a.From_DateTime desc"
        '            da3 = New SqlClient.SqlDataAdapter(cmd)
        '            dt3 = New DataTable
        '            da3.Fill(dt3)

        '            If dt3.Rows.Count > 0 Then

        '                If IsDBNull(dt3.Rows(0).Item("For_Salary").ToString) = False Then
        '                    vMas_BasSal_PerShift_PerMonth = Format(Val(dt3.Rows(0).Item("For_Salary").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("O_T").ToString) = False Then
        '                    OT_Sal_Shft = Format(Val(dt3.Rows(0).Item("O_T").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("MessDeduction").ToString) = False Then
        '                    mess_Ded = Format(Val(dt3.Rows(0).Item("MessDeduction").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("D_A").ToString) = False Then
        '                    DA_Amt = Format(Val(dt3.Rows(0).Item("D_A").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("H_R_A").ToString) = False Then
        '                    HRA_Amt = Format(Val(dt3.Rows(0).Item("H_R_A").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("Conveyance_Esi_Pf").ToString) = False Then
        '                    Convey_PF_Amt = Format(Val(dt3.Rows(0).Item("Conveyance_Esi_Pf").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("Conveyance_Bonus").ToString) = False Then
        '                    Convey_Bonus_Amt = Format(Val(dt3.Rows(0).Item("Conveyance_Bonus").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("Washing").ToString) = False Then
        '                    Washing_Amt = Format(Val(dt3.Rows(0).Item("Washing").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("Maintenance").ToString) = False Then
        '                    Maintain_Amt = Format(Val(dt3.Rows(0).Item("Maintenance").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("Entertainment").ToString) = False Then
        '                    Entertainment_Amt = Format(Val(dt3.Rows(0).Item("Entertainment").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("Provision").ToString) = False Then
        '                    Prrovision_Amt = Format(Val(dt3.Rows(0).Item("Provision").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("Other_Addition1").ToString) = False Then
        '                    Other_add1_Amt = Format(Val(dt3.Rows(0).Item("Other_Addition1").ToString), "########0.00")
        '                End If
        '                If IsDBNull(dt3.Rows(0).Item("Other_Addition2").ToString) = False Then
        '                    Other_add2_Amt = Format(Val(dt3.Rows(0).Item("Other_Addition2").ToString), "########0.00")
        '                End If

        '                If IsDBNull(dt3.Rows(0).Item("Week_Off_Allowance").ToString) = False Then
        '                    vMas_WeekOff_Allow_Amt_PerDay = Format(Val(dt3.Rows(0).Item("Week_Off_Allowance").ToString), "########0.00")
        '                End If

        '                If IsDBNull(dt3.Rows(0).Item("Other_Deduction1").ToString) = False Then
        '                    Others_Ded_Ded_Entry = Val(Others_Ded_Ded_Entry) + Format(Val(dt3.Rows(0).Item("Other_Deduction1").ToString), "########0.00")
        '                End If

        '                If CL_STS <> 0 Then
        '                    If IsDBNull(dt3.Rows(0).Item("CL").ToString) = False Then
        '                        CL_Leaves_Current = Format(Val(dt3.Rows(0).Item("CL").ToString), "########0")
        '                    End If
        '                Else
        '                    CL_Leaves_Current = 0
        '                End If
        '                If SL_STS <> 0 Then
        '                    If IsDBNull(dt3.Rows(0).Item("SL").ToString) = False Then
        '                        SL_Leaves_Current = Format(Val(dt3.Rows(0).Item("SL").ToString), "########0.00")
        '                    End If
        '                Else
        '                    SL_Leaves_Current = 0
        '                End If

        '            End If
        '            dt3.Clear()



        '            '====== Calculating Total Days ==========

        '            If Val(vEmp_IdNo) = 118 Then
        '                Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
        '            End If


        '            vNoOf_Wrkd_Dys = 0
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then

        '                If Tot_Noof_WrkdShft_Frm_Att > 0 Then

        '                    vNoOf_Wrkd_Dys = Tot_Noof_WrkdShft_Frm_Att

        '                    If Val(dt1.Rows(i).Item("Leave_Bonus_Less").ToString) = 1 Then

        '                        If Trim(UCase(dt1.Rows(i).Item("Attendance_Leave").ToString)) = "ATTENDANCE" Then
        '                            vNoOf_Wrkd_Dys = Tot_Noof_WrkdShft_Frm_Att

        '                        Else

        '                            vNoOf_Wrkd_Dys = vNo_Days_InMonth_for_MonthlyWages - Tot_LeaveDys_In_Mnth
        '                            'vNoOf_Wrkd_Dys = Val(txt_TotalDays.Text) - Tot_FH_Dys - Tot_LeaveDys_In_Mnth - Tot_WeekOff_Days
        '                            ''vNoOf_Wrkd_Dys = vNo_Days_InMonth_for_MonthlyWages - Tot_LeaveDys_In_Mnth - Tot_WeekOff_Days

        '                        End If

        '                    End If

        '                End If

        '                If Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 0 And Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 0 And Val(dt1.Rows(i).Item("Week_Off_Allowance").ToString) = 0 Then
        '                    vNoOf_Wrkd_Dys = vNoOf_Wrkd_Dys + vNoOf_Att_Dys_In_WkOff
        '                End If

        '                '---Attendance Days
        '                If Val(dt1.Rows(i).Item("Festival_Holidays_OT_Bonus").ToString) = 0 Then
        '                    vNoOf_Wrkd_Dys = vNoOf_Wrkd_Dys + vNoOf_Att_Dys_In_FH
        '                End If


        '            Else

        '                vNoOf_Wrkd_Dys = Tot_Noof_WrkdShft_Frm_Att

        '                If Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 1 Or Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 1 Or Val(dt1.Rows(i).Item("Week_Off_Allowance").ToString) = 1 Then
        '                    vNoOf_Wrkd_Dys = vNoOf_Wrkd_Dys - vNoOf_Att_Dys_In_WkOff
        '                End If

        '                '---Attendance Days
        '                If Val(dt1.Rows(i).Item("Festival_Holidays_OT_Bonus").ToString) = 1 Then
        '                    vNoOf_Wrkd_Dys = vNoOf_Wrkd_Dys - vNoOf_Att_Dys_In_FH
        '                End If


        '            End If

        '            If vNoOf_Wrkd_Dys < 0 Then vNoOf_Wrkd_Dys = 0

        '            '============ Festival Holiday ========== 
        '            vNoOf_FH_Dys_For_Sal = 0
        '            If Val(dt1.Rows(i).Item("Festival_Holidays").ToString) = 1 Then
        '                vNoOf_FH_Dys_For_Sal = Tot_FH_Dys '- vNoOf_FH_Dys_On_WkOff
        '            End If

        '            '---------Total Days
        '            Ttl_Days = vNoOf_Wrkd_Dys + vNoOf_FH_Dys_For_Sal

        '            vSal_Shift = 0
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                vSal_Shift = Format(vMas_BasSal_PerShift_PerMonth / vNo_Days_InMonth_for_MonthlyWages, "########0.00")

        '            Else
        '                vSal_Shift = vMas_BasSal_PerShift_PerMonth

        '            End If

        '            '============  STARTED DISPLAYING  ==========

        '            n = dgv_Details.Rows.Add()

        '            SNo = SNo + 1
        '            If Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 1 Then
        '                dgv_Details.Rows(n).Cells(15).ReadOnly = False
        '            Else
        '                dgv_Details.Rows(n).Cells(15).ReadOnly = True
        '            End If

        '            '---------
        '            .Rows(n).Cells(0).Value = Val(SNo)

        '            .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Employee_Name").ToString

        '            .Rows(n).Cells(2).Value = dt1.Rows(i).Item("Card_No").ToString

        '            '--Basic Bonus

        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then

        '                If Trim(UCase(dt1.Rows(i).Item("Attendance_Leave").ToString)) = "ATTENDANCE" Then
        '                    vNet_LeaveDys_In_Mnth = Tot_LeaveDys_In_Mnth - vNoOf_FH_Dys_For_Sal - IIf(Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 0, vNoOf_Att_Dys_In_WkOff, 0) - IIf(Val(dt1.Rows(i).Item("Festival_Holidays_OT_Bonus").ToString) = 0, vNoOf_Att_Dys_In_FH, 0)
        '                Else
        '                    vNet_LeaveDys_In_Mnth = Tot_LeaveDys_In_Mnth - IIf(Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 0, vNoOf_Att_Dys_In_WkOff, 0) - IIf(Val(dt1.Rows(i).Item("Festival_Holidays_OT_Bonus").ToString) = 0, vNoOf_Att_Dys_In_FH, 0)
        '                End If

        '                Bas_Sal = Format(vMas_BasSal_PerShift_PerMonth - (vSal_Shift * vNet_LeaveDys_In_Mnth), "###########0")

        '            Else
        '                vNet_LeaveDys_In_Mnth = Tot_LeaveDys_In_Mnth
        '                Bas_Sal = Format(Ttl_Days * vSal_Shift, "###########0")

        '            End If

        '            .Rows(n).Cells(3).Value = Format(Val(Bas_Sal), "##########0.00")
        '            If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

        '            '---Total Days
        '            .Rows(n).Cells(4).Value = Val(Ttl_Days)
        '            If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

        '            '---Net Pay - Will be updated at last
        '            .Rows(n).Cells(5).Value = ""

        '            '----WORKING DAYS
        '            .Rows(n).Cells(6).Value = Val(vNoOf_Wrkd_Dys)
        '            If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

        '            '---From Weekoff Credit for leave
        '            .Rows(n).Cells(7).Value = 0
        '            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

        '            '---From CL Credit for leave
        '            .Rows(n).Cells(8).Value = 0
        '            If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

        '            '---From SL Credit for leave
        '            .Rows(n).Cells(9).Value = 0
        '            If Val(.Rows(n).Cells(9).Value) = 0 Then .Rows(n).Cells(9).Value = ""

        '            '---Festival Holidays
        '            .Rows(n).Cells(10).Value = Val(vNoOf_FH_Dys_For_Sal)
        '            If Val(.Rows(n).Cells(10).Value) = 0 Then .Rows(n).Cells(10).Value = ""

        '            '---Total Days  = Bonus From Date to Bonus To date
        '            .Rows(n).Cells(11).Value = Val(Ttl_Days)
        '            If Val(.Rows(n).Cells(11).Value) = 0 Then .Rows(n).Cells(11).Value = ""

        '            '---No of Leaves
        '            .Rows(n).Cells(12).Value = Val(Tot_LeaveDys_In_Mnth)
        '            If Val(.Rows(n).Cells(12).Value) = 0 Then .Rows(n).Cells(12).Value = ""

        '            '---Attendance on Weekoff  / Festival Holidays
        '            .Rows(n).Cells(13).Value = Val(vNoOf_Att_Dys_In_WkOff) + Val(vNoOf_Att_Dys_In_FH)
        '            If Val(.Rows(n).Cells(13).Value) = 0 Then .Rows(n).Cells(13).Value = ""

        '            '---Opening Weekoff Credit
        '            .Rows(n).Cells(14).Value = Val(WeekOff_ADD_Opening) - Val(WeekOff_LESS_Opening)
        '            If Val(.Rows(n).Cells(14).Value) = 0 Then .Rows(n).Cells(14).Value = ""

        '            '---Add Weekoff Credit
        '            .Rows(n).Cells(15).Value = ""
        '            If Val(dt1.Rows(i).Item("Week_Off_Credit").ToString) = 1 Then
        '                .Rows(n).Cells(15).Value = vNoOf_Att_Dys_In_WkOff
        '            End If
        '            If Val(.Rows(n).Cells(15).Value) = 0 Then .Rows(n).Cells(15).Value = ""

        '            '---Less Weekoff Credit
        '            .Rows(n).Cells(16).Value = 0
        '            If Val(.Rows(n).Cells(16).Value) = 0 Then .Rows(n).Cells(16).Value = ""

        '            '---Total Weekoff Credit =  (Opening Weekoff  + Add Weekoff - Less Weekoff)
        '            .Rows(n).Cells(17).Value = Val(.Rows(n).Cells(14).Value) + Val(.Rows(n).Cells(15).Value) - Val(.Rows(n).Cells(16).Value)
        '            If Val(.Rows(n).Cells(17).Value) = 0 Then .Rows(n).Cells(17).Value = ""


        '            '---Opening CL Credit Days  (Opening + current Month) -Prev Month Used CL 
        '            .Rows(n).Cells(18).Value = 0
        '            If Val(dt1.Rows(i).Item("CL_Leave").ToString) = 1 Then
        '                .Rows(n).Cells(18).Value = Val(CL_Leaves) + Val(CL_Leaves_Current) - Val(Less_CL_Leaves)
        '            End If
        '            If Val(.Rows(n).Cells(18).Value) = 0 Then .Rows(n).Cells(18).Value = ""

        '            '---Less CL Credit Days
        '            .Rows(n).Cells(19).Value = 0
        '            If Val(.Rows(n).Cells(19).Value) = 0 Then .Rows(n).Cells(19).Value = ""

        '            '---Total CL Credit Days =  (Opening CL Credit Days  - Less CL Credit Days)
        '            .Rows(n).Cells(20).Value = 0
        '            If Val(dt1.Rows(i).Item("CL_Leave").ToString) = 1 Then
        '                .Rows(n).Cells(20).Value = Val(.Rows(n).Cells(18).Value) - Val(.Rows(n).Cells(19).Value)
        '            End If
        '            If Val(.Rows(n).Cells(20).Value) = 0 Then .Rows(n).Cells(20).Value = ""


        '            '---Opening SL Credit Days   (Opening + current Month) -Prev Month Used SL  
        '            .Rows(n).Cells(21).Value = ""
        '            If Val(dt1.Rows(i).Item("SL_Leave").ToString) = 1 Then
        '                .Rows(n).Cells(21).Value = Val(SL_Leaves) + Val(SL_Leaves_Current) - Val(Less_SL_Leaves)
        '            End If
        '            If Val(.Rows(n).Cells(21).Value) = 0 Then .Rows(n).Cells(21).Value = ""

        '            '---Less SL Credit Days
        '            .Rows(n).Cells(22).Value = 0
        '            If Val(.Rows(n).Cells(22).Value) = 0 Then .Rows(n).Cells(22).Value = ""

        '            '---Total SL Credit Days =  (Opening SL Credit Days  - Less SL Credit Days)
        '            .Rows(n).Cells(23).Value = ""
        '            If Val(dt1.Rows(i).Item("SL_Leave").ToString) = 1 Then
        '                .Rows(n).Cells(23).Value = Val(.Rows(n).Cells(21).Value) - Val(.Rows(n).Cells(22).Value)
        '            End If
        '            If Val(.Rows(n).Cells(23).Value) = 0 Then .Rows(n).Cells(23).Value = ""

        '            '---Bonus/Days
        '            .Rows(n).Cells(24).Value = Val(vSal_Shift)
        '            If Val(.Rows(n).Cells(24).Value) = 0 Then .Rows(n).Cells(24).Value = ""

        '            '---Basic Pay 

        '            .Rows(n).Cells(25).Value = Format(Bas_Sal, "###########0.00")
        '            If Val(.Rows(n).Cells(25).Value) = 0 Then .Rows(n).Cells(25).Value = ""

        '            '========== OT ====== 

        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
        '                '----getting Shift Hours
        '                If Val(dt1.Rows(i).Item("Shift1_Working_Hours").ToString) <> 0 Then
        '                    Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift1_Working_Hours").ToString), "##########0.00")
        '                ElseIf Val(dt1.Rows(i).Item("Shift2_Working_Hours").ToString) <> 0 Then
        '                    Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift2_Working_Hours").ToString), "##########0.00")
        '                ElseIf Val(dt1.Rows(i).Item("Shift3_Working_Hours").ToString) <> 0 Then
        '                    Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift3_Working_Hours").ToString), "##########0.00")
        '                Else
        '                    Shft_Hours = 8
        '                End If

        '                '-----Hours To Minutes
        '                H = Int(Shft_Hours)
        '                M = (Shft_Hours - H) * 100
        '                Shft_Mins = (H * 60) + M


        '                Tot_OTMins = 0
        '                '--------------If OT allowed in attandance Entry
        '                If Val(dt1.Rows(i).Item("OT_Allowed").ToString) = 1 And OT_Mins > Val(dt1.Rows(i).Item("OT_Allowed_After_Minutes").ToString) Then
        '                    Tot_OTMins = Tot_OTMins + OT_Mins
        '                End If
        '                '--------------Festival Holiday Attendance in OT Bonus  
        '                If Val(dt1.Rows(i).Item("Festival_Holidays_OT_Bonus").ToString) = 1 Then
        '                    Tot_OTMins = Tot_OTMins + (Shft_Mins * vNoOf_Att_Dys_In_FH)
        '                End If
        '                '-------------Weekoff Attendance in OT Bonus
        '                If Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 1 Then
        '                    Tot_OTMins = Tot_OTMins + (Shft_Mins * vNoOf_Att_Dys_In_WkOff)
        '                    weekof_mins = (Shft_Mins * vNoOf_Att_Dys_In_WkOff)
        '                End If

        '                '-----Minutes to Hour 
        '                H = Tot_OTMins \ 60
        '                M = Tot_OTMins - (H * 60)
        '                OTHrs = H & "." & Format(M, "00")

        '                '---OT Hours
        '                .Rows(n).Cells(26).Value = Format(Val(OTHrs), "#######0.00")
        '                If Val(.Rows(n).Cells(26).Value) = 0 Then .Rows(n).Cells(26).Value = ""

        '                '----OT Bonus Per Hour  (OT Bonus Per Shift / Shift Mins * 60)
        '                .Rows(n).Cells(27).Value = Format((OT_Sal_Shft / Shft_Mins) * 60, "#######0.00")
        '                If Val(.Rows(n).Cells(27).Value) = 0 Then .Rows(n).Cells(27).Value = ""

        '                '---- OT Bonus 
        '                '--------------Festival Holiday Attendance in OT Bonus
        '                OT_Bonus = Tot_OTMins * (OT_Sal_Shft / Shft_Mins)
        '                .Rows(n).Cells(28).Value = Format(Val(OT_Bonus), "#########0.00")
        '                If Val(.Rows(n).Cells(28).Value) = 0 Then .Rows(n).Cells(28).Value = ""

        '            Else
        '                If Val(dt1.Rows(i).Item("Shift1_Working_Hours").ToString) <> 0 Then
        '                    Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift1_Working_Hours").ToString), "##########0.00")
        '                ElseIf Val(dt1.Rows(i).Item("Shift2_Working_Hours").ToString) <> 0 Then
        '                    Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift2_Working_Hours").ToString), "##########0.00")
        '                ElseIf Val(dt1.Rows(i).Item("Shift3_Working_Hours").ToString) <> 0 Then
        '                    Shft_Hours = Format(Val(dt1.Rows(i).Item("Shift3_Working_Hours").ToString), "##########0.00")
        '                Else
        '                    Shft_Hours = 8
        '                End If

        '                '-----Hours To Minutes
        '                H = Int(Shft_Hours)
        '                M = (Shft_Hours - H) * 100
        '                Shft_Mins = (H * 60) + M

        '                Tot_OTMins = 0
        '                '--------------If OT allowed in attandance Entry
        '                If Val(dt1.Rows(i).Item("OT_Allowed").ToString) = 1 And OT_Mins > Val(dt1.Rows(i).Item("OT_Allowed_After_Minutes").ToString) Then
        '                    Tot_OTMins = Tot_OTMins + OT_Mins
        '                End If

        '                If Val(dt1.Rows(i).Item("Week_Attendance_Ot").ToString) = 1 Then
        '                    Tot_OTMins = Tot_OTMins + (Shft_Mins * vNoOf_Att_Dys_In_WkOff)
        '                End If

        '                '--------------Festival Holiday Attendance in OT Bonus  
        '                If Val(dt1.Rows(i).Item("Festival_Holidays_OT_Bonus").ToString) = 1 Then
        '                    Tot_OTMins = Tot_OTMins + (Shft_Mins * vNoOf_Att_Dys_In_FH)
        '                End If


        '                '-----Minutes to Hour 
        '                H = Tot_OTMins \ 60
        '                M = Tot_OTMins - (H * 60)
        '                OTHrs = H & "." & Format(M, "00")

        '                '---OT Hours
        '                .Rows(n).Cells(26).Value = Format(Val(OTHrs), "#######0.00")
        '                If Val(.Rows(n).Cells(26).Value) = 0 Then .Rows(n).Cells(26).Value = ""

        '                '----OT Bonus Per Hour  (OT Bonus Per Shift / Shift Mins * 60)
        '                .Rows(n).Cells(27).Value = Format((OT_Sal_Shft / Shft_Mins) * 60, "#######0.00")
        '                If Val(.Rows(n).Cells(27).Value) = 0 Then .Rows(n).Cells(27).Value = ""

        '                '---- OT Bonus 
        '                '--------------Festival Holiday Attendance in OT Bonus
        '                OT_Bonus = Format(Tot_OTMins * (OT_Sal_Shft / Shft_Mins), "##########0")
        '                .Rows(n).Cells(28).Value = Format(Val(OT_Bonus), "#########0.00")
        '                If Val(.Rows(n).Cells(28).Value) = 0 Then .Rows(n).Cells(28).Value = ""


        '            End If

        '            '=========================



        '            '--------DA
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                DA_Shft = Format(DA_Amt / vNo_Days_InMonth_for_MonthlyWages, "########0.000000")
        '            Else
        '                DA_Shft = DA_Amt
        '            End If
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(29).Value = Format(Math.Ceiling(DA_Amt - (DA_Shft * vNet_LeaveDys_In_Mnth)), "#########0.00")
        '            Else
        '                .Rows(n).Cells(29).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(DA_Shft)), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(29).Value) = 0 Then .Rows(n).Cells(29).Value = ""

        '            '--------Earnings  (Basic pay + DA)
        '            .Rows(n).Cells(30).Value = Format(Val(.Rows(n).Cells(25).Value) + Val(.Rows(n).Cells(29).Value), "#######0.00")
        '            If Val(.Rows(n).Cells(30).Value) = 0 Then .Rows(n).Cells(30).Value = ""

        '            '--------HRA
        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
        '                If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                    .Rows(n).Cells(31).Value = Format(Val(HRA_Amt), "#######0.00")
        '                Else
        '                    .Rows(n).Cells(31).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(HRA_Amt)), "#######0.00")
        '                End If
        '            Else
        '                .Rows(n).Cells(31).Value = Format(Val(HRA_Amt), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(31).Value) = 0 Then .Rows(n).Cells(31).Value = ""

        '            '--------Conveyance Bonus ------Travel Allowance
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(32).Value = Format(Val(Convey_Bonus_Amt), "#######0.00")
        '            Else
        '                .Rows(n).Cells(32).Value = Format(Math.Ceiling(Val(Tot_Noof_WrkdShft_Frm_Att) * Val(Convey_Bonus_Amt)), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(32).Value) = 0 Then .Rows(n).Cells(32).Value = ""

        '            '--------Washing
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(33).Value = Format(Val(Washing_Amt), "#######0.00")
        '            Else
        '                .Rows(n).Cells(33).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Washing_Amt)), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(33).Value) = 0 Then .Rows(n).Cells(33).Value = ""

        '            '--------Entertainment
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(34).Value = Format(Val(Entertainment_Amt), "#######0.00")
        '            Else
        '                .Rows(n).Cells(34).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Entertainment_Amt)), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(34).Value) = 0 Then .Rows(n).Cells(34).Value = ""


        '            '--------Maintanance
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(35).Value = Format(Val(Maintain_Amt), "#######0.00")
        '            Else
        '                .Rows(n).Cells(35).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Maintain_Amt)), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(35).Value) = 0 Then .Rows(n).Cells(35).Value = ""


        '            '--------Provision
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(36).Value = Format(Val(Prrovision_Amt), "#######0.00")
        '            Else
        '                .Rows(n).Cells(36).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Prrovision_Amt)), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(36).Value) = 0 Then .Rows(n).Cells(36).Value = ""


        '            '---Other Addition 1
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(37).Value = Format(Val(Other_add1_Amt), "#######0.00")
        '            Else
        '                .Rows(n).Cells(37).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Other_add1_Amt)), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(37).Value) = 0 Then .Rows(n).Cells(37).Value = ""

        '            '---Other Addition 2
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(38).Value = Format(Val(Other_add2_Amt), "#######0.00")
        '            Else
        '                .Rows(n).Cells(38).Value = Format(Math.Ceiling(Val(Ttl_Days) * Val(Other_add2_Amt)), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(38).Value) = 0 Then .Rows(n).Cells(38).Value = ""

        '            '--------Other Addition

        '            '-----if CL year arrear type is "Bonus" and Bonus month is march then that Bonus amount go to other Addition
        '            If Trim(dt1.Rows(i).Item("CL_Arrear_Type_Year").ToString) = "Bonus" And Val(thisMonth) = 3 Then
        '                Others_Add_Ded_Entry = Others_Add_Ded_Entry + (vSal_Shift * (CL_Leaves + CL_Leaves_Current))
        '            End If

        '            '-----if SL year arrear type is "Bonus" and Bonus month is march then that Bonus amount go to other Addition
        '            If Trim(dt1.Rows(i).Item("SL_Arrear_Type_Year").ToString) = "Bonus" And Val(thisMonth) = 3 Then
        '                Others_Add_Ded_Entry = Others_Add_Ded_Entry + (vSal_Shift * (SL_Leaves + SL_Leaves_Current))
        '            End If

        '            '-----if CL month arrear type is "Bonus"  then that Bonus amount go to other Addition
        '            If Trim(dt1.Rows(i).Item("CL_Arrear_Type").ToString) = "Bonus" Then
        '                Others_Add_Ded_Entry = Others_Add_Ded_Entry + (vSal_Shift * (CL_Leaves_Current))
        '            End If

        '            '-----if SL month arrear type is "Bonus"  then that Bonus amount go to other Addition
        '            If Trim(dt1.Rows(i).Item("SL_Arrear_Type").ToString) = "Bonus" Then
        '                Others_Add_Ded_Entry = Others_Add_Ded_Entry + (vSal_Shift * (SL_Leaves_Current))
        '            End If
        '            .Rows(n).Cells(39).Value = Format(Math.Ceiling(Val(Others_Add_Ded_Entry)), "#######0.00")
        '            If Val(.Rows(n).Cells(39).Value) = 0 Then .Rows(n).Cells(39).Value = ""


        '            '--------Incentives from attendance
        '            .Rows(n).Cells(40).Value = Format(Val(vIncenAmt_FromAtt), "#########0.00")
        '            If Val(.Rows(n).Cells(40).Value) = 0 Then .Rows(n).Cells(40).Value = ""


        '            '------Week off Allowance
        '            .Rows(n).Cells(41).Value = ""
        '            If Val(dt1.Rows(i).Item("Week_Off_Allowance").ToString) = 1 Then
        '                .Rows(n).Cells(41).Value = Format(Math.Ceiling(vMas_WeekOff_Allow_Amt_PerDay * vNoOf_Att_Dys_In_WkOff), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(41).Value) = 0 Then .Rows(n).Cells(41).Value = ""

        '            '--------Total Addition  =  (DA + HRA + COVEYANCE + WASHING + ENTETAINMENT + MAINTANACE +PROVISION + OTHER ADDITION + INCETIVES)
        '            .Rows(n).Cells(42).Value = Format(Val(.Rows(n).Cells(29).Value) + Val(.Rows(n).Cells(31).Value) + Val(.Rows(n).Cells(32).Value) + Val(.Rows(n).Cells(33).Value) + Val(.Rows(n).Cells(34).Value) + Val(.Rows(n).Cells(35).Value) + Val(.Rows(n).Cells(36).Value) + Val(.Rows(n).Cells(37).Value) + Val(.Rows(n).Cells(38).Value) + Val(.Rows(n).Cells(39).Value) + Val(.Rows(n).Cells(40).Value) + Val(.Rows(n).Cells(41).Value), "#######0.00")
        '            If Val(.Rows(n).Cells(42).Value) = 0 Then .Rows(n).Cells(42).Value = ""


        '            '--------Mess
        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
        '                .Rows(n).Cells(43).Value = Format(Val(Mess_Ded_Entry), "#######0.00")
        '            Else
        '                If Mess_Ded_Entry <> 0 Then
        '                    .Rows(n).Cells(43).Value = Format(Val(Mess_Ded_Entry), "#######0.00")
        '                Else
        '                    .Rows(n).Cells(43).Value = Format(Math.Ceiling(Val(mess_Ded) * Val(vNoOf_Wrk_Dys_Frm_MessAtt)), "#######0.00")
        '                End If
        '            End If
        '            If Val(.Rows(n).Cells(43).Value) = 0 Then .Rows(n).Cells(43).Value = ""

        '            '--------Medical
        '            .Rows(n).Cells(44).Value = Format(Val(Medical_Ded_Entry), "#######0.00")
        '            If Val(.Rows(n).Cells(44).Value) = 0 Then .Rows(n).Cells(44).Value = ""

        '            '--------Store 
        '            .Rows(n).Cells(45).Value = Format(Val(Store_Ded_Entry), "#######0.00")
        '            If Val(.Rows(n).Cells(45).Value) = 0 Then .Rows(n).Cells(45).Value = ""

        '            vPFSTS_Sal = 0
        '            vESISTS_Sal = 0
        '            vPFSTS_Audit = 0
        '            vESISTS_Audit = 0
        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
        '                vPFSTS_Sal = Val(dt1.Rows(i).Item("Pf_Bonus").ToString)
        '                vESISTS_Sal = Val(dt1.Rows(i).Item("Esi_Bonus").ToString)

        '                vPFSTS_Audit = Val(dt1.Rows(i).Item("Pf_Status").ToString)
        '                vESISTS_Audit = Val(dt1.Rows(i).Item("Esi_Status").ToString)

        '            Else
        '                vPFSTS_Sal = Val(dt1.Rows(i).Item("Pf_Status").ToString)
        '                vESISTS_Sal = Val(dt1.Rows(i).Item("Esi_Status").ToString)

        '                vPFSTS_Audit = Val(dt1.Rows(i).Item("Pf_Status").ToString)
        '                vESISTS_Audit = Val(dt1.Rows(i).Item("Esi_Status").ToString)
        '            End If

        '            'esi


        '            '============================= ESI - PF - Bonus ==================================
        '            '--------ESI  1.75 %
        '            .Rows(n).Cells(46).Value = ""
        '            If vESISTS_Sal = 1 Then
        '                vTotErngs_FOR_ESI = Val(.Rows(n).Cells(25).Value) + Val(.Rows(n).Cells(42).Value) - Val(.Rows(n).Cells(33).Value)
        '                If Val(dt1.Rows(i).Item("Esi_For_OTBonus_Status").ToString) = 1 Then
        '                    vTotErngs_FOR_ESI = vTotErngs_FOR_ESI + Val(.Rows(n).Cells(28).Value)
        '                End If
        '                If Val(vSal_Shift) > Val(ESI_MAX_SHFT_WAGES) And Val(vTotErngs_FOR_ESI) < 25000 Then
        '                    '----If Shift Bonus graterthan 100 then ESI allowed
        '                    .Rows(n).Cells(46).Value = Format(Math.Round(Val(vTotErngs_FOR_ESI) * 1.75 / 100), "#########0.00")
        '                End If
        '            End If
        '            If Val(.Rows(n).Cells(46).Value) = 0 Then .Rows(n).Cells(46).Value = ""


        '            .Rows(n).Cells(47).Value = ""
        '            .Rows(n).Cells(48).Value = ""
        '            .Rows(n).Cells(49).Value = ""
        '            .Rows(n).Cells(77).Value = ""
        '            If vPFSTS_Sal = 1 Then

        '                '--------PF  ( 12 % ) - Management_Contribution_Perc
        '                If Val(.Rows(n).Cells(30).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then
        '                    .Rows(n).Cells(47).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 12 / 100), "#########0.00")

        '                Else
        '                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)    '---SPINNING MILL
        '                        Basic_Salary_FOR_PF_CALCULATION = Val(.Rows(n).Cells(25).Value) * 70 / 100
        '                        .Rows(n).Cells(47).Value = Format(Math.Ceiling(Val(Basic_Salary_FOR_PF_CALCULATION) * 12 / 100), "#########0.00")

        '                    Else
        '                        .Rows(n).Cells(47).Value = Format(Math.Ceiling(Val(.Rows(n).Cells(30).Value) * 12 / 100), "#########0.00")

        '                    End If

        '                End If

        '                '--------EPF  (8.33 %)
        '                '-------Basic Pay Graterthan 6500 than EPF value is 541 only allowed
        '                '-------Basic Pay Graterthan 15000 than EPF value is 1249.5 only allowed
        '                If Val(.Rows(n).Cells(30).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then
        '                    .Rows(n).Cells(48).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 8.33 / 100), "#########0.00")
        '                Else
        '                    .Rows(n).Cells(48).Value = Format(Math.Round(Val(.Rows(n).Cells(30).Value) * 8.33 / 100), "#########0.00")
        '                End If

        '                '--------Pension  (3.67 %) 
        '                .Rows(n).Cells(49).Value = Format(Val(.Rows(n).Cells(47).Value) - Val(.Rows(n).Cells(48).Value), "#########0.00")


        '                If Val(dt1.Rows(i).Item("PF_Credit_Status").ToString) = 1 Then
        '                    .Rows(n).Cells(77).Value = Format(Val(.Rows(n).Cells(47).Value), "#########0.00")
        '                End If

        '            End If

        '            If Val(.Rows(n).Cells(47).Value) = 0 Then .Rows(n).Cells(47).Value = ""
        '            If Val(.Rows(n).Cells(48).Value) = 0 Then .Rows(n).Cells(48).Value = ""
        '            If Val(.Rows(n).Cells(49).Value) = 0 Then .Rows(n).Cells(49).Value = ""
        '            If Val(.Rows(n).Cells(77).Value) = 0 Then .Rows(n).Cells(77).Value = ""


        '            '--------LATE MINS

        '            Ot_Int = Int(Late_Mins / 60)
        '            Ot_minVal = Ot_Int * 60
        '            Late_Hours = Ot_Int + ((Late_Mins - Ot_minVal) / 100)

        '            .Rows(n).Cells(50).Value = Format(Val(Late_Hours), "#######0.00")
        '            If Val(.Rows(n).Cells(50).Value) = 0 Then .Rows(n).Cells(50).Value = ""


        '            '--------LATE HOURS Bonus

        '            .Rows(n).Cells(51).Value = 0
        '            If Late_sts = True Then
        '                .Rows(n).Cells(51).Value = Format(Math.Ceiling(Val(Late_Mins) * (Val(vSal_Shift) / Val(Shft_Mins))), "#######0.00")
        '            End If
        '            If Val(.Rows(n).Cells(51).Value) = 0 Then .Rows(n).Cells(51).Value = ""



        '            '--------Other Deduction
        '            '--------------------------Leave Bonus Less 
        '            'If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '            '    If Val(dt1.Rows(i).Item("Leave_Bonus_Less").ToString) = 1 Then
        '            '        '.Rows(n).Cells(52).Value = Format(Val(Others_Ded_Ded_Entry) + vSal_Shift * (Tot_LeaveDys_In_Mnth - (Val(.Rows(n).Cells(19).Value) + Val(.Rows(n).Cells(22).Value))), "#######0.00")
        '            '        .Rows(n).Cells(52).Value = Format(Val(Others_Ded_Ded_Entry) + vSal_Shift * ((Val(.Rows(n).Cells(19).Value) + Val(.Rows(n).Cells(22).Value))), "#######0.00")
        '            '    Else
        '            '        .Rows(n).Cells(52).Value = Format(Val(Others_Ded_Ded_Entry), "#######0.00")
        '            '    End If
        '            'Else
        '            .Rows(n).Cells(52).Value = Format(Val(Others_Ded_Ded_Entry), "#######0.00")
        '            'End If

        '            If Val(.Rows(n).Cells(52).Value) = 0 Then .Rows(n).Cells(52).Value = ""


        '            '--------Total Deduction  =  (MESS+ MEDICAL + STORE + ESI + PF  LATE HOUR Bonus +   OTHER DEDUCTION )
        '            .Rows(n).Cells(53).Value = Format(Val(.Rows(n).Cells(43).Value) + Val(.Rows(n).Cells(44).Value) + Val(.Rows(n).Cells(45).Value) + Val(.Rows(n).Cells(46).Value) + Val(.Rows(n).Cells(47).Value) + Val(.Rows(n).Cells(51).Value) + Val(.Rows(n).Cells(52).Value), "#######0.00")
        '            If Val(.Rows(n).Cells(53).Value) = 0 Then .Rows(n).Cells(53).Value = ""


        '            '--------Attendance Incetive

        '            Att_Incentive = 0
        '            If Tot_LeaveDys_In_Mnth >= 0 Then
        '                cmd.CommandText = "select a.Amount as Att_IncentiveAmount from PayRoll_Category_Details a Where a.Category_IdNo <> 0 and a.Category_IdNo = " & Str(Val(vEmpCatgry_IdNo)) & " and a.To_Attendance = " & Str(Val(Tot_LeaveDys_In_Mnth))
        '                da2 = New SqlClient.SqlDataAdapter(cmd)
        '                dt2 = New DataTable
        '                da2.Fill(dt2)
        '                If dt2.Rows.Count > 0 Then
        '                    If IsDBNull(dt2.Rows(0).Item("Att_IncentiveAmount").ToString) = False Then
        '                        Att_Incentive = Val(dt2.Rows(0).Item("Att_IncentiveAmount").ToString)
        '                    End If
        '                End If
        '                dt2.Clear()
        '            End If

        '            .Rows(n).Cells(54).Value = Format(Val(Att_Incentive), "#######0.00")
        '            If Val(.Rows(n).Cells(54).Value) = 0 Then .Rows(n).Cells(54).Value = ""

        '            '--------Net Bonus  = (BASIC Bonus + OT Bonus + TOTAL ADDITIONS - TOTAL DEDUTIONS +Attendance Incetive )
        '            Net_Bonus = Val(.Rows(n).Cells(3).Value) + Val(.Rows(n).Cells(28).Value) + Val(.Rows(n).Cells(42).Value) - Val(.Rows(n).Cells(53).Value) + Val(.Rows(n).Cells(54).Value)

        '            .Rows(n).Cells(55).Value = Format(Net_Bonus, "##########0.00")
        '            If Val(.Rows(n).Cells(55).Value) = 0 Then .Rows(n).Cells(55).Value = ""

        '            '-----Less Advance
        '            mins_Adv = 0
        '            If Val(Advance_Ded_Entry) <> 0 Then
        '                mins_Adv = Advance_Ded_Entry
        '                Less_Advance_Col_Edit_STS = False

        '            Else
        '                cmd.CommandText = "Select sum(a.Minus_Advance) as Ent_MinusAdvance from PayRoll_Bonus_Details a Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Bonus_Code = '" & Trim(NewCode) & "'"
        '                Da = New SqlClient.SqlDataAdapter(cmd)
        '                dt4 = New DataTable
        '                Da.Fill(dt4)
        '                If dt4.Rows.Count > 0 Then
        '                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
        '                        mins_Adv = Format(Math.Abs(Val(dt4.Rows(0).Item("Ent_MinusAdvance").ToString)), "##########0.00")
        '                    End If
        '                End If
        '                dt4.Clear()

        '            End If

        '            '-----OPENING ADVANCE
        '            .Rows(n).Cells(69).Value = Format(Val(Amt_OpBal), "#########0.00")
        '            If Val(.Rows(n).Cells(69).Value) = 0 Then .Rows(n).Cells(69).Value = ""

        '            'If Val(vEmp_IdNo) = 118 Then
        '            '    Debug.Print(dt1.Rows(i).Item("Employee_Name").ToString)
        '            'End If

        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL

        '                '-----Total Advance + Bonus ADVANCE
        '                .Rows(n).Cells(56).Value = Format(Val(Amt_OpBal) + Val(Sal_advance) + Val(Bonus_Pending), "#########0.00")
        '                If Val(.Rows(n).Cells(56).Value) = 0 Then .Rows(n).Cells(56).Value = ""


        '                '-----Less Advance
        '                .Rows(n).Cells(57).Value = Format(Val(mins_Adv), "########0.00")
        '                If Val(.Rows(n).Cells(57).Value) = 0 Then .Rows(n).Cells(57).Value = ""


        '                '------Balance Advance   ((Total Advance (OP + Previous-Bonus_Payment_Pending + Bonus ADVANCE) - Less Advance)
        '                .Rows(n).Cells(58).Value = Format(Val(.Rows(n).Cells(56).Value) - Val(.Rows(n).Cells(57).Value), "########0.00")
        '                If Val(.Rows(n).Cells(58).Value) = 0 And Val(.Rows(n).Cells(56).Value) = 0 Then
        '                    .Rows(n).Cells(57).Value = ""
        '                End If
        '                If Val(.Rows(n).Cells(58).Value) = 0 Then .Rows(n).Cells(58).Value = ""

        '                '-----Bonus Advance
        '                .Rows(n).Cells(59).Value = Format(Val(Sal_advance), "#########0.00")
        '                If Val(.Rows(n).Cells(59).Value) = 0 Then .Rows(n).Cells(59).Value = ""

        '                '----- Bonus Pending
        '                .Rows(n).Cells(60).Value = Format(Val(Bonus_Pending), "#########0.00")
        '                If Val(.Rows(n).Cells(60).Value) = 0 Then .Rows(n).Cells(60).Value = ""

        '                '----- Net Pay     Net Bonus - Advance Deduction - Bonus advance  + Bonus Pending
        '                Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)), "##########0.00")

        '            Else

        '                '-----Total Advance
        '                .Rows(n).Cells(56).Value = Format(Val(Amt_OpBal), "#########0.00")
        '                If Val(.Rows(n).Cells(56).Value) = 0 Then .Rows(n).Cells(56).Value = ""


        '                '-----Less Advance
        '                .Rows(n).Cells(57).Value = Format(Val(mins_Adv), "########0.00")
        '                If Val(.Rows(n).Cells(57).Value) = 0 Then .Rows(n).Cells(57).Value = ""


        '                '------Balance Advance   (Total Advance  - Less Advance)
        '                .Rows(n).Cells(58).Value = Format(Val(Amt_OpBal) - Val(mins_Adv), "########0.00")
        '                If Val(.Rows(n).Cells(58).Value) = 0 And Val(.Rows(n).Cells(56).Value) = 0 Then
        '                    .Rows(n).Cells(57).Value = ""
        '                End If
        '                If Val(.Rows(n).Cells(58).Value) = 0 Then .Rows(n).Cells(58).Value = ""

        '                '-----Bonus Advance
        '                .Rows(n).Cells(59).Value = Format(Val(Sal_advance), "#########0.00")
        '                If Val(.Rows(n).Cells(59).Value) = 0 Then .Rows(n).Cells(59).Value = ""


        '                '----- Bonus Pending

        '                .Rows(n).Cells(60).Value = Format(Val(Bonus_Pending), "#########0.00")
        '                If Val(.Rows(n).Cells(60).Value) = 0 Then .Rows(n).Cells(60).Value = ""

        '                '----- Net Pay     Net Bonus - Advance Deduction - Bonus advance  + Bonus Pending
        '                Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value), "##########0.00")
        '                'Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value) + Val(.Rows(n).Cells(60).Value), "##########0.00")

        '            End If


        '            '.Rows(n).Cells(61).Value = Format(Net_Pay, "#########0")
        '            '.Rows(n).Cells(5).Value = Format(Net_Pay, "#########0")
        '            'If Val(.Rows(n).Cells(61).Value) = 0 Then
        '            '    If Val(.Rows(n).Cells(61).Value) = 0 Then
        '            '        .Rows(n).Cells(61).Value = ""
        '            '        .Rows(n).Cells(5).Value = ""
        '            '    End If
        '            'End If


        '            '-----Day Of Bonus
        '            .Rows(n).Cells(62).Value = Val(vNoOf_Wrkd_Dys)
        '            If Val(.Rows(n).Cells(62).Value) = 0 Then .Rows(n).Cells(62).Value = ""

        '            '-----Earnings For Bonus
        '            .Rows(n).Cells(63).Value = 0
        '            If Val(.Rows(n).Cells(63).Value) = 0 Then .Rows(n).Cells(63).Value = ""

        '            '-----OT Mins
        '            .Rows(n).Cells(64).Value = OT_Mins
        '            If Val(.Rows(n).Cells(64).Value) = 0 Then .Rows(n).Cells(64).Value = ""


        '            '-----Add CL Leave
        '            .Rows(n).Cells(65).Value = Val(CL_Leaves_Current)
        '            If Val(.Rows(n).Cells(65).Value) = 0 Then .Rows(n).Cells(65).Value = ""


        '            '-----Add SL Leave
        '            .Rows(n).Cells(66).Value = Val(SL_Leaves_Current)
        '            If Val(.Rows(n).Cells(66).Value) = 0 Then .Rows(n).Cells(66).Value = ""


        '            '-----LOP STATUS  (Loss of Pay)
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then

        '                .Rows(n).Cells(67).Value = Val(dt1.Rows(i).Item("Leave_Bonus_Less").ToString)

        '            Else
        '                .Rows(n).Cells(67).Value = 0

        '            End If

        '            '-----Actual Bonus
        '            If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '                .Rows(n).Cells(68).Value = vMas_BasSal_PerShift_PerMonth
        '            Else
        '                .Rows(n).Cells(68).Value = vMas_BasSal_PerShift_PerMonth
        '            End If
        '            If Val(.Rows(n).Cells(68).Value) = 0 Then .Rows(n).Cells(68).Value = ""

        '            'If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
        '            '    .Rows(n).Cells(68).Value = vSal_Shift * (Val(txt_TotalDays.Text) - Tot_WeekOff_Days)
        '            'Else
        '            '    .Rows(n).Cells(68).Value = vSal_Shift * Val(txt_TotalDays.Text)
        '            'End If
        '            'If Val(.Rows(n).Cells(68).Value) = 0 Then .Rows(n).Cells(68).Value = ""

        '            .Rows(n).Cells(70).Value = True
        '            cmd.CommandText = "Select * from PayRoll_Bonus_Details a Where a.Employee_IdNo = " & Str(Val(vEmp_IdNo)) & " and a.Bonus_Code = '" & Trim(NewCode) & "'"
        '            Da = New SqlClient.SqlDataAdapter(cmd)
        '            dt4 = New DataTable
        '            Da.Fill(dt4)
        '            If dt4.Rows.Count > 0 Then
        '                If IsDBNull(dt4.Rows(0).Item("Signature_Status").ToString) = False Then
        '                    If Val(dt4.Rows(0).Item("Signature_Status").ToString) = 0 Then
        '                        .Rows(n).Cells(70).Value = False
        '                    End If
        '                End If
        '            End If
        '            dt4.Clear()


        '            '============================= ESI - PF - Bonus ==================================
        '            '--------ESI  1.75 %
        '            .Rows(n).Cells(71).Value = ""
        '            .Rows(n).Cells(72).Value = ""
        '            .Rows(n).Cells(73).Value = ""
        '            .Rows(n).Cells(76).Value = ""

        '            If vESISTS_Audit = 1 Then

        '                vTotErngs_FOR_ESI = Val(.Rows(n).Cells(25).Value) + Val(.Rows(n).Cells(42).Value) - Val(.Rows(n).Cells(33).Value)
        '                If Val(dt1.Rows(i).Item("Esi_For_OTBonus_Status").ToString) = 1 Then
        '                    vTotErngs_FOR_ESI = vTotErngs_FOR_ESI + Val(.Rows(n).Cells(28).Value)
        '                End If

        '                If Val(vSal_Shift) > Val(ESI_MAX_SHFT_WAGES) And Val(vTotErngs_FOR_ESI) < 25000 Then
        '                    '----If Shift Bonus graterthan 100 then ESI allowed
        '                    .Rows(n).Cells(71).Value = Format(Val(vTotErngs_FOR_ESI) * 1.75 / 100, "#########0.00")
        '                End If
        '            End If

        '            If vPFSTS_Audit = 1 Then

        '                '--------PF  ( 12 % ) - Management_Contribution_Perc

        '                If Val(.Rows(n).Cells(30).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then
        '                    .Rows(n).Cells(72).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 12 / 100), "#########0.00")

        '                Else
        '                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '----- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  '---- SPINNING MILL
        '                        Basic_Salary_FOR_PF_CALCULATION = Val(.Rows(n).Cells(25).Value) * 70 / 100
        '                        .Rows(n).Cells(72).Value = Format(Math.Ceiling(Val(Basic_Salary_FOR_PF_CALCULATION) * 12 / 100), "#########0.00")

        '                    Else
        '                        .Rows(n).Cells(72).Value = Format(Math.Ceiling(Val(.Rows(n).Cells(30).Value) * 12 / 100), "#########0.00")

        '                    End If

        '                End If

        '                '--------EPF  (8.33 %)

        '                '-------Basic Pay Graterthan 6500 than EPF value is 541 only allowed
        '                '-------Basic Pay Graterthan 15000 than EPF value is 1249.5 only allowed
        '                If Val(.Rows(n).Cells(30).Value) >= Val(EPF_MAX_BASICPAY) And Val(EPF_MAX_BASICPAY) > 0 Then
        '                    .Rows(n).Cells(73).Value = Format(Math.Ceiling(Val(EPF_MAX_BASICPAY) * 8.33 / 100), "#########0.00")
        '                Else
        '                    .Rows(n).Cells(73).Value = Format(Math.Ceiling(Val(.Rows(n).Cells(30).Value) * 8.33 / 100), "#########0.00")
        '                End If

        '                .Rows(n).Cells(76).Value = Format(Val(.Rows(n).Cells(72).Value) - Val(.Rows(n).Cells(73).Value), "#############0.00")

        '            End If
        '            If Val(.Rows(n).Cells(71).Value) = 0 Then .Rows(n).Cells(71).Value = ""
        '            If Val(.Rows(n).Cells(72).Value) = 0 Then .Rows(n).Cells(72).Value = ""
        '            If Val(.Rows(n).Cells(73).Value) = 0 Then .Rows(n).Cells(73).Value = ""
        '            If Val(.Rows(n).Cells(76).Value) = 0 Then .Rows(n).Cells(76).Value = ""

        '            '=================================================================================================================================

        '            '---BonusESI + OT Bonus ESI
        '            OT_Bonus_ESI = 0
        '            .Rows(n).Cells(74).Value = ""
        '            'If vESISTS_Sal = 1 Then
        '            '    OT_Bonus_ESI = Format(Val(OT_Bonus) * 1.75 / 100, "#########0.00")
        '            '    .Rows(n).Cells(74).Value = Format(Val(OT_Bonus_ESI), "#########0.00")
        '            '    If Val(.Rows(n).Cells(74).Value) = 0 Then .Rows(n).Cells(74).Value = ""
        '            'End If


        '            Bonus_plus_ot_esi = 0
        '            'If Val(dt1.Rows(i).Item("Esi_Bonus").ToString) = 1 Then
        '            Bonus_plus_ot_esi = Format(Val(.Rows(n).Cells(46).Value) + Val(.Rows(n).Cells(74).Value), "#############0")
        '            'ElseIf Val(dt1.Rows(i).Item("Esi_Status").ToString) = 1 Then
        '            '    Bonus_plus_ot_esi = Format(Val(.Rows(n).Cells(71).Value) + Val(.Rows(n).Cells(74).Value), "#############0")
        '            'End If
        '            .Rows(n).Cells(75).Value = Format(Val(Bonus_plus_ot_esi), "#########0.00")
        '            If Val(.Rows(n).Cells(75).Value) = 0 Then .Rows(n).Cells(75).Value = ""

        '            '==============

        '            Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value) + Val(.Rows(n).Cells(77).Value), "##########0.00")
        '            'Net_Pay = Format((Val(.Rows(n).Cells(55).Value) - Val(.Rows(n).Cells(57).Value)) - Val(.Rows(n).Cells(59).Value) - Val(.Rows(n).Cells(74).Value), "##########0.00")

        '            .Rows(n).Cells(61).Value = Format(Net_Pay, "#########0")
        '            .Rows(n).Cells(5).Value = Format(Net_Pay, "#########0")
        '            If Val(.Rows(n).Cells(61).Value) = 0 Then
        '                If Val(.Rows(n).Cells(61).Value) = 0 Then
        '                    .Rows(n).Cells(61).Value = ""
        '                    .Rows(n).Cells(5).Value = ""
        '                End If
        '            End If

        '        Next i

        '        pnl_ProgressBar.Visible = False


        '    End If
        '    dt1.Dispose()
        '    da1.Dispose()

        '    dt2.Dispose()
        '    da2.Dispose()

        '    'btn_Calculation_Bonus.BackColor = Color.DeepPink

        'End With

        'Grid_Cell_DeSelect()

        ''ShowOrHideColumns()

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
        '    dgv_Details.Columns(57).ReadOnly = False
        '    If Less_Advance_Col_Edit_STS = False Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
        '        dgv_Details.Columns(57).ReadOnly = True
        '    End If
        'End If

        'NoCalc_Status = False

    End Sub


    Private Sub get_PayRoll_Bonus_Details()

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

        Dim TotShifts As Single = 0

        Dim n As Integer
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NoCalc_Status = True

        ESI_MAX_SHFT_WAGES = 0
        EPF_MAX_BASICPAY = 0

        ' 
        ' 
        Dim Selection_Cond As String = ""

        If chk_ExcludeWO.Checked Then
            If Len(Trim(Selection_Cond)) > 0 Then
                Selection_Cond = Selection_Cond + " And "
            End If
            Selection_Cond = " And Not datepart(dw,A.Employee_Attendance_Date) = 1"
        End If

        If chk_ExcludePH_LH.Checked Then
            If Len(Trim(Selection_Cond)) > 0 Then
                Selection_Cond = Selection_Cond + " And "
            End If
            Selection_Cond = Selection_Cond + " Not A.Employee_Attendance_Date In (Select HolidayDateTime From Holiday_Details) "
        End If

        da1 = New SqlClient.SqlDataAdapter("select E.Employee_Name,E.Employee_IdNo, E.Card_No,S.For_Salary,Sum(A.No_Of_Shift),Count(DISTINCT A.Employee_Attendance_Date),E.Shift_Day_Month,C.No_Days_Month_Wages " & _
                                           " From Payroll_Employee_Head E Inner Join PayRoll_Employee_Attendance_Details A On E.Employee_IdNo = A.Employee_IdNo  " & _
                                           " AND A.Employee_Attendance_Date >= '" & Format(dtp_FromDate.Value, "dd-MMM-yyyy") & "' and  A.Employee_Attendance_Date <= '" & Format(dtp_ToDate.Value, "dd-MMM-yyyy") & "' and A.No_Of_Shift > 0 " & _
                                           Selection_Cond & _
                                           " inner Join PayRoll_Employee_Salary_Details S On  E.Employee_IdNo = S.Employee_IdNo And S.From_DateTime = ( Select Max(From_DateTime) From PayRoll_Employee_Salary_Details s1 Where s1.Employee_IdNo = S.Employee_IdNo And s1.From_DateTime <= '" & Format(dtp_ToDate.Value, "dd-MMM-yyyy") & "') " & _
                                           " inner join Payroll_Category_Head C On E.Category_IdNo = C.Category_IdNo AND C.CATEGORY_IDNO = " & Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text) & _
                                           " Group by E.Employee_Name,E.Employee_IdNo,S.For_Salary, E.Card_No,E.Shift_Day_Month,C.No_Days_Month_Wages " & _
                                           " Having Sum(A.No_Of_Shift) > 0 ", con)

        dt1 = New DataTable
        da1.Fill(dt1)

        dgv_Details.Rows.Clear()

        If dt1.Rows.Count > 0 Then

            For z = 0 To dt1.Rows.Count - 1

                n = dgv_Details.Rows.Add()

                dgv_Details.Item(0, n).Value = (n + 1).ToString
                dgv_Details.Item(1, n).Value = dt1.Rows(n).Item(0)
                dgv_Details.Item(2, n).Value = dt1.Rows(n).Item(1)
                dgv_Details.Item(3, n).Value = dt1.Rows(n).Item(2)
                dgv_Details.Item(5, n).Value = FormatNumber(dt1.Rows(n).Item(5), 0)

                If InStr(UCase(Trim(dt1.Rows(n).Item(6))), "MONTH", CompareMethod.Text) > 0 Then
                    dgv_Details.Item(6, n).Value = FormatNumber(dt1.Rows(n).Item(3) / dt1.Rows(n).Item(7), 2, TriState.False, TriState.False, TriState.False)
                Else
                    dgv_Details.Item(6, n).Value = FormatNumber(dt1.Rows(n).Item(3), 2, TriState.False, TriState.False, TriState.False)
                End If
                Selection_Cond = ""

                'Selection_Cond = " And Employee_IdNo In (Select Employee_IdNo From Payroll_Employee_Head Where "

                If chk_ExcludeWO.Checked Then
                    If Len(Trim(Selection_Cond)) > 0 Then
                        Selection_Cond = Selection_Cond + " And "
                    End If
                    Selection_Cond = " And Not datepart(dw,P.Employee_Attendance_Date) = 1"
                End If

                If chk_ExcludePH_LH.Checked Then
                    If Len(Trim(Selection_Cond)) > 0 Then
                        Selection_Cond = Selection_Cond + " And "
                    End If
                    Selection_Cond = Selection_Cond + " Not P.Employee_Attendance_Date In (Select HolidayDateTime From Holiday_Details) "
                End If

                If Val(txt_MaxShifts.Text) = 0 And Val(txt_MinShifts.Text) = 0 Then
                    da2 = New SqlClient.SqlDataAdapter("select datepart(month,p.Employee_Attendance_Date) as Mnth ,datepart(year,p.Employee_Attendance_Date) as Yr ,Sum(Isnull(No_Of_Shift,0)) From PayRoll_Employee_Attendance_Details P " & _
                                                       " Where Employee_Attendance_Date >= '" & Format(dtp_FromDate.Value, "dd-MMM-yyyy") & "' and  Employee_Attendance_Date <= '" & Format(dtp_ToDate.Value, "dd-MMM-yyyy") & "' and Employee_IdNo = " & CStr(Val(dgv_Details.Item(2, n).Value)) & IIf(Len(Trim(Selection_Cond)) > 0, Selection_Cond, "") & _
                                                       " Group by datepart(month,p.Employee_Attendance_Date),datepart(year,p.Employee_Attendance_Date) order by convert(datetime,('1-' + convert(varchar,datepart(month,p.Employee_Attendance_Date)) + '-' + convert(varchar,datepart(year,p.Employee_Attendance_Date))))", con)
                ElseIf Val(txt_MaxShifts.Text) <> 0 And Val(txt_MinShifts.Text) = 0 Then
                    da2 = New SqlClient.SqlDataAdapter("select datepart(month,p.Employee_Attendance_Date) as Mnth ,datepart(year,p.Employee_Attendance_Date) as Yr ,Sum(Isnull(No_Of_Shift,0)) From PayRoll_Employee_Attendance_Details P " & IIf(Len(Trim(Selection_Cond)) > 0, Selection_Cond, "") & _
                                                       " Where Employee_Attendance_Date >= '" & Format(dtp_FromDate.Value, "dd-MMM-yyyy") & "' and  Employee_Attendance_Date <= '" & Format(dtp_ToDate.Value, "dd-MMM-yyyy") & "' and Employee_IdNo = " & CStr(Val(dgv_Details.Item(2, n).Value)) & " and No_Of_Shift <= " & CStr(Val(txt_MaxShifts.Text)) & IIf(Len(Trim(Selection_Cond)) > 0, Selection_Cond, "") & _
                                                       " Group by datepart(month,p.Employee_Attendance_Date),datepart(year,p.Employee_Attendance_Date) order by convert(datetime,('1-' + convert(varchar,datepart(month,p.Employee_Attendance_Date)) + '-' + convert(varchar,datepart(year,p.Employee_Attendance_Date))))", con)
                ElseIf Val(txt_MaxShifts.Text) = 0 And Val(txt_MinShifts.Text) <> 0 Then
                    da2 = New SqlClient.SqlDataAdapter("select datepart(month,p.Employee_Attendance_Date) as Mnth ,datepart(year,p.Employee_Attendance_Date) as Yr ,Sum(Isnull(No_Of_Shift,0)) From PayRoll_Employee_Attendance_Details P " & IIf(Len(Trim(Selection_Cond)) > 0, Selection_Cond, "") & _
                                                      " Where Employee_Attendance_Date >= '" & Format(dtp_FromDate.Value, "dd-MMM-yyyy") & "' and  Employee_Attendance_Date <= '" & Format(dtp_ToDate.Value, "dd-MMM-yyyy") & "' and Employee_IdNo = " & CStr(Val(dgv_Details.Item(2, n).Value)) & " and No_Of_Shift >= " & CStr(Val(txt_MinShifts.Text)) & IIf(Len(Trim(Selection_Cond)) > 0, Selection_Cond, "") & _
                                                      " Group by datepart(month,p.Employee_Attendance_Date),datepart(year,p.Employee_Attendance_Date) order by convert(datetime,('1-' + convert(varchar,datepart(month,p.Employee_Attendance_Date)) + '-' + convert(varchar,datepart(year,p.Employee_Attendance_Date))))", con)
                Else
                    da2 = New SqlClient.SqlDataAdapter("select datepart(month,p.Employee_Attendance_Date) as Mnth ,datepart(year,p.Employee_Attendance_Date) as Yr ,Sum(Isnull(No_Of_Shift,0)) From PayRoll_Employee_Attendance_Details P " & _
                                                     " Where Employee_Attendance_Date >= '" & Format(dtp_FromDate.Value, "dd-MMM-yyyy") & "' and  Employee_Attendance_Date <= '" & Format(dtp_ToDate.Value, "dd-MMM-yyyy") & "' and Employee_IdNo = " & CStr(Val(dgv_Details.Item(2, n).Value)) & " and No_Of_Shift >= " & CStr(Val(txt_MinShifts.Text)) & " and No_Of_Shift <= " & CStr(Val(txt_MaxShifts.Text)) & IIf(Len(Trim(Selection_Cond)) > 0, Selection_Cond, "") & _
                                                     " Group by datepart(month,p.Employee_Attendance_Date),datepart(year,p.Employee_Attendance_Date) order by convert(datetime,('1-' + convert(varchar,datepart(month,p.Employee_Attendance_Date)) + '-' + convert(varchar,datepart(year,p.Employee_Attendance_Date))))", con)
                End If



                dt2 = New DataTable
                da2.Fill(dt2)

                Dim StPos As Integer = 0
                TotShifts = 0

                If dt2.Rows.Count > 0 Then

                    For i As Integer = 0 To dt2.Rows.Count - 1

                        For J As Integer = 11 To 11 + MonthCnt - 1

                            If IsDate(Join(Split(dgv_Details.Columns(J).HeaderText, " "), "")) Then
                                If Month(CDate(Join(Split(dgv_Details.Columns(J).HeaderText, " "), ""))) = dt2.Rows(i).Item(0) And Year(CDate(Join(Split(dgv_Details.Columns(J).HeaderText, " "), ""))) = dt2.Rows(i).Item(1) Then

                                    dgv_Details.Rows(n).Cells(J).Value = FormatNumber(dt2.Rows(i).Item(2), 2, TriState.False, TriState.False, TriState.False)
                                    TotShifts = TotShifts + dt2.Rows(i).Item(2)

                                End If
                            End If

                        Next

                    Next

                End If

                If Val(txt_MaxShifts.Text) <> 0 And Val(txt_MinShifts.Text) = 0 Then

                    da2 = New SqlClient.SqlDataAdapter("select datepart(month,p.Employee_Attendance_Date) as Mnth ,datepart(year,p.Employee_Attendance_Date) as Yr ,COUNT(No_Of_Shift)* " & CStr(Val(txt_MaxShifts.Text)) & " From PayRoll_Employee_Attendance_Details P " & _
                                                       " Where Employee_Attendance_Date >= '" & Format(dtp_FromDate.Value, "dd-MMM-yyyy") & "' and  Employee_Attendance_Date <= '" & Format(dtp_ToDate.Value, "dd-MMM-yyyy") & "' and Employee_IdNo = " & CStr(Val(dgv_Details.Item(2, n).Value)) & " and No_Of_Shift >" & CStr(Val(txt_MaxShifts.Text)) & IIf(Len(Trim(Selection_Cond)) > 0, Selection_Cond, "") & _
                                                       " Group by datepart(month,p.Employee_Attendance_Date),datepart(year,p.Employee_Attendance_Date) order by convert(datetime,('1-' + convert(varchar,datepart(month,p.Employee_Attendance_Date)) + '-' + convert(varchar,datepart(year,p.Employee_Attendance_Date))))", con)




                    dt2 = New DataTable
                    da2.Fill(dt2)

                    StPos = 0

                    If dt2.Rows.Count > 0 Then

                        For i As Integer = 0 To dt2.Rows.Count - 1

                            For J As Integer = 10 To 10 + MonthCnt - 1

                                If IsDate(Join(Split(dgv_Details.Columns(J).HeaderText, " "), "")) Then
                                    If Month(CDate(Join(Split(dgv_Details.Columns(J).HeaderText, " "), ""))) = dt2.Rows(i).Item(0) And Year(CDate(Join(Split(dgv_Details.Columns(J).HeaderText, " "), ""))) = dt2.Rows(i).Item(1) Then

                                        dgv_Details.Rows(n).Cells(J).Value = FormatNumber(Val(dgv_Details.Rows(i).Cells(J).Value) + dt2.Rows(i).Item(2), 2, TriState.False, TriState.False, TriState.False)
                                        TotShifts = TotShifts + dt2.Rows(i).Item(2)

                                    End If
                                End If
                            Next

                        Next

                    End If

                End If

                dgv_Details.Item(4, n).Value = FormatNumber(TotShifts, 2, TriState.False, TriState.False, TriState.False)
                dgv_Details.Item(8, n).Value = Val(txt_BonusRate.Text)

                If Val(dgv_Details.Item(5, n).Value) > Val(txt_MinAttendance.Text) Then
                    dgv_Details.Item(7, n).Value = FormatNumber((dgv_Details.Item(4, n).Value) * Val(dgv_Details.Item(6, n).Value), 2, TriState.False, TriState.False, TriState.False)
                    dgv_Details.Item(9, n).Value = FormatNumber((dgv_Details.Item(7, n).Value) * Val(dgv_Details.Item(8, n).Value) / 100, 2, TriState.False, TriState.False, TriState.False)
                End If

                pnl_ProgressBar.Visible = True
                ProgBar1.Visible = True
                ProgBar1.Value = (z + 1) / dt1.Rows.Count * 100
                lbl_ProPerc.Text = (ProgBar1.Value).ToString & "%"

            Next

        End If

        dt1.Dispose()
        da1.Dispose()

        NoCalc_Status = False

        pnl_ProgressBar.Visible = False
        ProgBar1.Visible = False
        ProgBar1.Value = 0
        lbl_ProPerc.Text = (ProgBar1.Value).ToString & "%"

    End Sub

    Private Sub Bonus_Payment_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

    Private Sub Bonus_Payment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        'dgv_Details.Columns(57).ReadOnly = False
        'dgv_Details.Columns(77).Visible = False

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
        AddHandler dtp_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BonusRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MaxShifts.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MinAttendance.GotFocus, AddressOf ControlGotFocus
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
        AddHandler dtp_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BonusRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MaxShifts.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinAttendance.LostFocus, AddressOf ControlLostFocus
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
        AddHandler txt_BonusRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MinAttendance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MaxShifts.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FromDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BonusRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FilterBillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MinAttendance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MaxShifts.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0



        Filter_Status = False
        FrmLdSTS = True
        new_record()

        'ShowOrHideColumns()

    End Sub

    Private Sub Bonus_Payment_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Bonus_Payment_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Bonus_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Bonus_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition2) & "%/" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from PayRoll_Bonus_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Bonus_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bonus_No from PayRoll_Bonus_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bonus_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bonus_No from PayRoll_Bonus_Head where for_orderby > " & Str(Format(Val(OrdByNo), "########.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bonus_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bonus_No from PayRoll_Bonus_Head where for_orderby < " & Str(Format(Val(OrdByNo), "########0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bonus_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Bonus_No from PayRoll_Bonus_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bonus_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Bonus_Head", "Bonus_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

            'ShowOrHideColumns()

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

            Da = New SqlClient.SqlDataAdapter("select Bonus_No from PayRoll_Bonus_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code = '" & Trim(RefCode) & "'", con)
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

            Da = New SqlClient.SqlDataAdapter("select Bonus_No from PayRoll_Bonus_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code = '" & Trim(InvCode) & "'", con)
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

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Bonus_Entry, New_Entry) = False Then Exit Sub

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
        'If Trim(UCase(Mon_Wek)) <> "WEEKLY" Then
        '    Mth_IDNo = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)
        '    If Val(Mth_IDNo) = 0 Then
        '        MessageBox.Show("Invalid Month", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If cbo_Month.Enabled And cbo_Month.Visible Then cbo_Month.Focus()
        '        Exit Sub
        '    End If
        'End If

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

        cmd.Parameters.AddWithValue("@BonusFromDate", dtp_FromDate.Value.Date)

        cmd.Parameters.AddWithValue("@BonusToDate", dtp_ToDate.Value.Date)

        cmd.CommandText = "select * from PayRoll_Bonus_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & " and Category_IdNo = " & Str(Val(Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text))) & " and Bonus_Code <> '" & Trim(NewCode) & "' and ( (@BonusFromDate Between From_Date and To_Date) or (@BonusToDate Between From_Date and To_Date) )"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            MessageBox.Show("Invalid From (or) To date " & Chr(13) & "Already Bonus Entry prepared for this Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_ToDate.Enabled And dtp_ToDate.Visible Then dtp_ToDate.Focus()
            Exit Sub
        End If
        Dt1.Clear()

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(9).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Then

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

        'Try

        If Insert_Entry = True Or New_Entry = False Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Else

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Bonus_Head", "Bonus_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        End If

        cmd.Connection = con
        cmd.Transaction = tr

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@BonusDate", dtp_Date.Value.Date)

        cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)

        cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

        'If dtp_Advance_UpToDate.Visible = True Then
        'cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_Advance_UpToDate.Value.Date)
        'Else
        'cmd.Parameters.AddWithValue("@AdvanceUpToDate", dtp_ToDate.Value.Date)
        'End If

        If New_Entry = True Then
            cmd.CommandText = "Insert into PayRoll_Bonus_Head (     Bonus_Code        ,               Company_IdNo       ,           Bonus_No             ,                               for_OrderBy                               ,   Bonus_Date ,       Salary_Payment_Type_IdNo   ,             Category_IdNo     ,     From_Date,  To_Date ,  Max_Shifts                         , Min_Shifts                           ,Min_Att_Reqd                             ,Exclude_WO                                  , Exclude_PH_LH                                   , Bonus_rate                           ,M1                                          ,M2                                          ,M3                                          ,M4                                          ,M5                                          ,M6                                          ,M7                                          ,M8                                          ,M9                                          ,M10                                          ,M11                                        ,M12                                          ,M13                                            ,M14                                          )" & _
                                "          Values              ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @BonusDate  , " & Str(Val(SalPymtTyp_IdNo)) & ", " & Str(Val(vCatgry_IdNo)) & ",  @FromDate   , @ToDate  ," & CStr(Val(txt_MaxShifts.Text)) & "," & CStr(Val(txt_MinShifts.Text)) & " ," & CStr(Val(txt_MinAttendance.Text)) & "," & IIf(chk_ExcludeWO.Checked, "1", "0") & "," & IIf(chk_ExcludePH_LH.Checked, "1", "0") & "  , " & CStr(Val(txt_BonusRate.Text)) & ",'" & dgv_Details.Columns(11).HeaderText & "','" & dgv_Details.Columns(12).HeaderText & "','" & dgv_Details.Columns(13).HeaderText & "','" & dgv_Details.Columns(14).HeaderText & "','" & dgv_Details.Columns(15).HeaderText & "','" & dgv_Details.Columns(16).HeaderText & "','" & dgv_Details.Columns(17).HeaderText & "','" & dgv_Details.Columns(18).HeaderText & "','" & dgv_Details.Columns(19).HeaderText & "','" & dgv_Details.Columns(20).HeaderText & "','" & dgv_Details.Columns(21).HeaderText & "','" & dgv_Details.Columns(22).HeaderText & "'   ,'" & dgv_Details.Columns(23).HeaderText & "' ,'" & dgv_Details.Columns(24).HeaderText & "' ) "
            cmd.ExecuteNonQuery()

        Else

            cmd.CommandText = "Update PayRoll_Bonus_Head set Bonus_Date = @BonusDate, Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ",  Category_IdNo = " & Str(Val(vCatgry_IdNo)) & ", From_Date = @FromDate, To_Date =  @ToDate  " & _
                              ",Max_Shifts = " & CStr(Val(txt_MinShifts.Text)) & ", Min_Shifts = " & CStr(Val(txt_MaxShifts.Text)) & " ,Min_Att_Reqd  = " & CStr(Val(txt_MinAttendance.Text)) & " ,Exclude_WO = " & IIf(chk_ExcludeWO.Checked, "1", "0") & ", Exclude_PH_LH = " & IIf(chk_ExcludePH_LH.Checked, "1", "0") & " , Bonus_rate = " & Val(txt_BonusRate.Text).ToString & _
                              ",M1 = '" & dgv_Details.Columns(11).HeaderText & "',M2    = '" & dgv_Details.Columns(12).HeaderText & "',M3   = '" & dgv_Details.Columns(13).HeaderText & "',M4  = '" & dgv_Details.Columns(14).HeaderText & "',M5  = '" & dgv_Details.Columns(15).HeaderText & "'   ,M6 = '" & dgv_Details.Columns(16).HeaderText & "'    ,M7 = '" & dgv_Details.Columns(17).HeaderText & "'" & _
                              ",M8 = '" & dgv_Details.Columns(18).HeaderText & "',M9    = '" & dgv_Details.Columns(19).HeaderText & "',M10  = '" & dgv_Details.Columns(20).HeaderText & "',M11 = '" & dgv_Details.Columns(21).HeaderText & "',M12 = '" & dgv_Details.Columns(22).HeaderText & "',M13 = '" & dgv_Details.Columns(23).HeaderText & "',M14 = '" & dgv_Details.Columns(24).HeaderText & "'" & _
                              "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code = '" & Trim(NewCode) & "'"
            MsgBox(cmd.CommandText)
            cmd.ExecuteNonQuery()

        End If

        cmd.CommandText = "Delete from PayRoll_Bonus_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bonus_Code = '" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()


        VouNarr = ""


        With dgv_Details

            Sno = 0
            For i = 0 To .RowCount - 1

                Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                If Val(Emp_ID) <> 0 Then

                    Sno = Sno + 1

                    r = 0




                    cmd.CommandText = "Insert into PayRoll_Bonus_Details (        Bonus_Code     ,               Company_IdNo       ,            Bonus_No          ,                               for_OrderBy                              ,             Sl_No     ,        Employee_IdNo        ,M1                                                    ,M2                                                    ,M3                                                   ,M4                                                     ,M5                                                    ,M6                                                    ,M7                                                    ,M8                                                    ,M9                                                    ,M10                                                   ,M11                                                   ,M12                                                      ,M13                                                    ,M14                                               ,Tot_Shifts                                               , Tot_Att                                                 ,Wage_Per_day                                             ,Total_Earnings                                            , Bonus_Rate                                               , Bonus_Earned                                             ,Bonus_Finalised) " & _
                                      "            Values                 ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  " & Str(Val(Sno)) & ", " & Str(Val(Emp_ID)) & " ," & CStr(Val(dgv_Details.Rows(i).Cells(11).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(12).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(13).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(14).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(15).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(16).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(17).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(18).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(19).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(20).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(21).Value)) & "," & CStr(Val(dgv_Details.Rows(i).Cells(22).Value)) & "   ," & CStr(Val(dgv_Details.Rows(i).Cells(23).Value)) & " ," & CStr(Val(dgv_Details.Rows(i).Cells(24).Value)) & "," & Val(dgv_Details.Rows(i).Cells(4).Value).ToString & "," & Val(dgv_Details.Rows(i).Cells(5).Value).ToString & "," & Val(dgv_Details.Rows(i).Cells(6).Value).ToString & "," & Val(dgv_Details.Rows(i).Cells(7).Value).ToString & "," & Val(dgv_Details.Rows(i).Cells(8).Value).ToString & "," & Val(dgv_Details.Rows(i).Cells(9).Value).ToString & "," & Val(dgv_Details.Rows(i).Cells(10).Value).ToString & "  ) "
                    cmd.ExecuteNonQuery()



                    'If Val(Emp_ID) = 136 Then
                    '    Debug.Print(Emp_ID)
                    'End If

                    Sal_Amt = Val(.Rows(i).Cells(10).Value)

                    If Val(Sal_Amt) <> 0 Then

                        If Val(Sal_Amt) < 0 Then
                            vLed_IdNos = Common_Procedures.CommonLedger.Salary_Ac & "|" & Emp_ID

                        Else
                            vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.Salary_Ac
                        End If

                        'vVou_Amts = Format(Math.Abs(Val(Sal_Amt)) + Math.Abs(Val(Val(.Rows(i).Cells(57).Value))) + Math.Abs(Val(Val(.Rows(i).Cells(59).Value)) - Math.Abs(Val(.Rows(i).Cells(43).Value))), "#########0.00") & "|" & Format(-1 * (Math.Abs(Val(Sal_Amt)) + Math.Abs(Val(Val(.Rows(i).Cells(57).Value))) + Math.Abs(Val(Val(.Rows(i).Cells(59).Value))) - Math.Abs(Val(.Rows(i).Cells(43).Value))), "#########0.00")
                        vVou_Amts = Format(Math.Abs(Val(Sal_Amt)), "#########0.00") & "|" & Format(-1 * (Math.Abs(Val(Sal_Amt))), "#########0.00")

                        'If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                        VouNarr = "Bonus for Period " & dtp_FromDate.Text & " to " & dtp_ToDate.Text

                        'Else
                        'VouNarr = "Bonus for Month " & cbo_Month.Text

                        'End If

                        If Common_Procedures.Voucher_Updation(con, "Emp.Bonus", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                            Throw New ApplicationException(ErrMsg)
                            Exit Sub
                        End If

                    End If


                    '----------------


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


        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally

        '    Dt1.Dispose()
        '    Da.Dispose()
        '    cmd.Dispose()
        '    tr.Dispose()
        '    Dt1.Clear()

        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        'End Try

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
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentType, cbo_Category, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")

            'If Asc(e.KeyChar) = 13 Then

            '    If Trim(cbo_PaymentType.Text) <> "" Then

            '        SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

            '        Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

            '        If Trim(UCase(cbo_PaymentType.Text)) <> Trim(UCase(cbo_PaymentType.Tag)) Then

            '            If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
            '                dtp_FromDate.Enabled = True
            '                dtp_ToDate.Enabled = True

            '                'cbo_Month.Text = ""

            '                dtp_FromDate.Focus()

            '                cbo_Category.Enabled = False

            '            Else
            '                'cbo_Month.Enabled = True
            '                dtp_FromDate.Enabled = False
            '                dtp_ToDate.Enabled = False

            '                dtp_FromDate.Text = ""
            '                dtp_ToDate.Text = ""

            '                cbo_Category.Focus()

            '            End If

            '        Else

            '            If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
            '                dtp_FromDate.Enabled = True
            '                dtp_ToDate.Enabled = True

            '                dtp_FromDate.Focus()

            '                cbo_Category.Enabled = False

            '            Else
            '                'cbo_Month.Enabled = True
            '                dtp_FromDate.Enabled = False
            '                dtp_ToDate.Enabled = False

            '                cbo_Category.Focus()

            '            End If

            '        End If

            '    End If

            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE PAYMENTTYPE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Month_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
        'cbo_Month.Tag = cbo_Month.Text
    End Sub

    Private Sub cbo_Month_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        vcbo_KeyDwnVal = e.KeyValue
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Month, cbo_Category, dtp_FromDate, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_Month_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim dttm As Date
        Dim Mth_ID As Integer = 0

        Try

            'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Month, Nothing, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")

            'If Asc(e.KeyChar) = 13 And Trim(cbo_Month.Text) <> "" Then

            '    If Trim(UCase(cbo_Month.Tag)) <> Trim(UCase(cbo_Month.Text)) Then

            '        'Mth_ID = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

            '        dttm = New DateTime(IIf(Mth_ID >= 4, Year(Common_Procedures.Company_FromDate), Year(Common_Procedures.Company_ToDate)), Mth_ID, 1)

            '        dtp_FromDate.Text = dttm

            '        dttm = DateAdd("M", 1, dttm)
            '        dttm = DateAdd("d", -1, dttm)

            '        dtp_ToDate.Text = dttm

            '        get_PayRoll_Bonus_Details()


            '    End If

            '    If dtp_Advance_UpToDate.Visible And dtp_Advance_UpToDate.Enabled Then
            '        dtp_Advance_UpToDate.Focus()

            '    Else
            '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '            save_record()
            '        Else
            '            dtp_Date.Focus()
            '        End If

            '    End If

            'End If

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
                Condt = "a.Bonus_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Bonus_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bonus_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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

            da = New SqlClient.SqlDataAdapter("select a.*,  c.Ledger_Name as PartyName from PayRoll_Bonus_Head a  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bonus_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bonus_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from PayRoll_Bonus_Head a INNER JOIN PayRoll_Bonus_Details b ON a.Bonus_Code = b.Bonus_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bonus_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bonus_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Bonus_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bonus_Date").ToString), "dd-MM-yyyy")
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

    
    End Sub

    Private Sub TotalBonus()

        Dim TBonus As Double = 0

        For I As Integer = 0 To dgv_Details.RowCount - 1

            TBonus = TBonus + Val(dgv_Details.Rows(I).Cells(10).Value)

        Next

        txt_TotalBonus.Text = FormatNumber(TBonus, 2, TriState.False, TriState.False, TriState.False)

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
        dgv_Details.CurrentCell.Selected = False
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

    Private Sub txt_FestivalDays_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True

            Else
                btn_save.Focus()

            End If
        End If

        ' If e.KeyValue = 38 Then txt_TotalDays.Focus()

    End Sub

    Private Sub txt_TotalDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'txt_TotalDays.Text = Format(Val(txt_TotalDays.Text), "#########0.00")
    End Sub

    Private Sub txt_FestivalDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        ' txt_FestivalDays.Text = Format(Val(txt_FestivalDays.Text), "#########0.00")
    End Sub

    Private Sub txt_FestivalDays_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.RowCount > 0 Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub txt_VatAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub dtp_ToDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ToDate.GotFocus

        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
        'dtp_Advance_UpToDate.Tag = dtp_Advance_UpToDate.Text

    End Sub


    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ToDate.KeyPress

        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        'Try
        If Asc(e.KeyChar) = 13 Then

            If DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value) > 13 Then
                MsgBox("Time Interval Is Too Long To Generate Bonus. Cannot Continue")
                Exit Sub
            End If

            If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Then

                For I As Integer = 11 To 24

                    dgv_Details.Columns(I).HeaderText = ""
                    dgv_Details.Columns(I).Visible = False

                Next

                MonthCnt = 0

                If DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value) >= 0 Then

                    For I As Integer = 0 To DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value)

                        dgv_Details.Columns(I + 11).HeaderText = Format(DateAdd(DateInterval.Month, I, dtp_FromDate.Value), "MMM- yyyy")
                        dgv_Details.Columns(I + 11).Visible = True
                        MonthCnt = MonthCnt + 1

                    Next

                End If

                dtp_FromDate.Tag = dtp_FromDate.Text
                dtp_ToDate.Tag = dtp_ToDate.Text

            End If

        End If

        'Catch ex As Exception

        'MessageBox.Show(ex.Message, "ERROR WHILE TODATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub dtp_FromDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_FromDate.GotFocus
        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
    End Sub

    Private Sub dtp_FromDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FromDate.KeyPress


        'Try

        If Asc(e.KeyChar) = 13 Then
            If DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value) > 13 Then
                MsgBox("Time Interval Is Too Long To Generate Bonus. Cannot Continue")
                Exit Sub
            End If


            If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Then

                'SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

                For I As Integer = 11 To 24

                    dgv_Details.Columns(I).HeaderText = ""
                    dgv_Details.Columns(I).Visible = False

                Next

                MonthCnt = 0

                If DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value) >= 0 Then

                    For I As Integer = 0 To DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value)

                        dgv_Details.Columns(I + 11).HeaderText = Format(DateAdd(DateInterval.Month, I, dtp_FromDate.Value), "MMM- yyyy")
                        dgv_Details.Columns(I + 11).Visible = True
                        MonthCnt = MonthCnt + 1

                    Next

                End If

            End If

            dtp_FromDate.Tag = dtp_FromDate.Text
            dtp_ToDate.Tag = dtp_ToDate.Text

        End If

        'Catch ex As Exception

        'MessageBox.Show(ex.Message, "ERROR WHILE FROMDATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then
        pnl_Back.Enabled = False
        pnl_PrintEmployee_Details.Visible = True
        printEmployee_Selection()
        If btn_Print_Employee.Enabled Then btn_Print_Employee.Focus()
        'Else
        '    printing_Bonus()
        'End If

    End Sub

    Private Sub btn_Calculation_Bonus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        get_PayRoll_Bonus_Details()
    End Sub

    Private Sub dtp_Advance_UpToDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtp_FromDate.Tag = dtp_FromDate.Text
        dtp_ToDate.Tag = dtp_ToDate.Text
        'dtp_Advance_UpToDate.Tag = dtp_Advance_UpToDate.Text
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

    Private Sub btn_BonusList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Bonus Register Simple1"
        Common_Procedures.RptInputDet.ReportHeading = "Bonus Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON,PT"
        f.MdiParent = MDIParent1
        f.Show()
        f.dtp_FromDate.Text = dtp_Date.Text
        f.dtp_ToDate.Text = dtp_Date.Text

        'f.cbo_Inputs2.Text = cbo_Month.Text
        'f.cbo_Inputs3.Text = cbo_PaymentType.Text
        'f.Show_Report()

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


    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try

            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                    End If
                End If
            End With

            'With dgv_Details
            '    If .Visible Then
            '        If .Rows.Count > 0 Then
            '            If .CurrentCell.ColumnIndex = 50 Or .CurrentCell.ColumnIndex = 52 Then
            '                If Val(.CurrentRow.Cells(50).Value) <> 0 Or Val(.CurrentRow.Cells(52).Value) <> 0 Then

            '                    .CurrentRow.Cells(54).Value = Format((((Val(.CurrentRow.Cells(48).Value) - Val(.CurrentRow.Cells(50).Value))) - Val(.CurrentRow.Cells(52).Value)) + Val(.CurrentRow.Cells(53).Value), "##########0.00")
            '                    .CurrentRow.Cells(51).Value = Format(Val(.CurrentRow.Cells(49).Value) - Val(.CurrentRow.Cells(50).Value), "#########0.00")

            '                End If

            '            End If

            '        End If

            '    End If

            'End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub printing_Bonus()
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

            da1 = New SqlClient.SqlDataAdapter("select * from PayRoll_Bonus_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and Bonus_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_orderby, Bonus_No", con)
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


            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, mh.* from PayRoll_Bonus_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Month_Head mh ON mh.month_IdNo = a.Month_IdNo  Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bonus_Code ='" & Trim(NewCode) & "' Order by a.for_orderby, a.Bonus_No", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)


            If prn_HdDt.Rows.Count > 0 Then


                da2 = New SqlClient.SqlDataAdapter("select a.*, c.* ,dh.*  from PayRoll_Bonus_Details a LEFT OUTER JOIN PayRoll_Employee_Head c ON a.Employee_IdNo = c.Employee_IdNo LEFT OUTER JOIN Department_Head Dh ON c.Department_IdNo = dh.Department_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bonus_Code = '" & Trim(NewCode) & "' and a.Employee_Idno IN " & vSelc_EmpIDNOS & " Order by a.sl_no", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, c.*   from PayRoll_Bonus_Details a LEFT OUTER JOIN PayRoll_Employee_Head c ON a.Employee_IdNo = c.Employee_IdNo    where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bonus_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ot_Bonus").ToString, LMargin + ClArr(1) - 10, CurY, 1, 0, pFont)
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
        Common_Procedures.Print_To_PrintDocument(e, "Bonus DETAILS", LMargin, CurY, 2, PrintWidth, p1Font)
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
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Bonus_No").ToString, LMargin + 250, CurY, 0, 0, pFont)
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
        Common_Procedures.Print_To_PrintDocument(e, "Total Bonus", LMargin + 50, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 120, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Total Dedn", LMargin + 460, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + 530, CurY, 0, 0, p1Font)
        ' Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Net_Bonus").ToString, LMargin + ClAr(1) - 10, CurY, 1, 0, pFont)
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
        Common_Procedures.Print_To_PrintDocument(e, "Bonus PAID", LMargin + 150, CurY, 0, 0, p1Font)
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

        Common_Procedures.Print_To_PrintDocument(e, "Bonus Paid on :", LMargin + 300, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

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

    Private Sub Payroll_Bonus_Entry_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Category, cbo_PaymentType, dtp_FromDate, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
    End Sub

    Private Sub cbo_Category_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Category.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Category, dtp_FromDate, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
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

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Bonus").ToString) > 0 Then
                            LcurY = LcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Extra Time Bonus ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Bonus").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
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



                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bonus_OT_ESI").ToString) > 0 Then
                            RcurY = RcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "ESI @ 1.75%", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Bonus_OT_ESI").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
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


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Late_Hours_Bonus").ToString) > 0 Then
                            RcurY = RcurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Late Hours Bonus", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Late_Hours_Bonus").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
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
                        nOTsal = Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Bonus").ToString)
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
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Bonus_No").ToString, LMargin + 115, CurY, 0, 0, pFont)
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

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 30, LnAr(5), LMargin + ClAr(1) - 30, LnAr(3))

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

        'Common_Procedures.Print_To_PrintDocument(e, "Bonus Paid on :", LMargin + 300, CurY, 0, 0, p1Font)
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

            printing_Bonus()

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

            empidno = 0 ' Common_Procedures.Employee_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
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
                        p3font = New Font("Calibri", 9, FontStyle.Bold)
                        CurY = CurY + TxtHgt

                        ''SNo = SNo + 1
                        pFont = New Font("Baamini", 9, FontStyle.Regular)
                        p1Font = New Font("Calibri", 7, FontStyle.Regular)
                        'CurY = CurY + TxtHgt
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

                        LcurY = CurY + 3
                        Common_Procedures.Print_To_PrintDocument(e, "Basic Pay", LMargin + 10, LcurY, 0, 0, p1Font)
                        ''Common_Procedures.Print_To_PrintDocument(e, "Advance", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        ''Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + ClArr(1) + 110, CurY, 0, 0, pFont)
                        ''Common_Procedures.Print_To_PrintDocument(e, "Basic Pay", LMargin + ClArr(1) + 120, CurY + 2, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Earning").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        ''Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Total_Advance").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                        'CurY = CurY + TxtHgt - 8

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("H_R_A").ToString) > 0 Then
                            LcurY = LcurY + 12
                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "HRA ", LMargin + 10, LcurY, 0, 0, p1Font)

                            'Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + ClArr(1) + 110, CurY, 0, 0, p1Font)

                            'Common_Procedures.Print_To_PrintDocument(e, "gp.vg;.", LMargin + ClArr(1) + 120, CurY, 0, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, "@ 12 %", LMargin + ClArr(1) + 170, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("H_R_A").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Conveyance").ToString) > 0 Then
                            LcurY = LcurY + 12
                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Conveyance ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Conveyance").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("washing").ToString) > 0 Then
                            LcurY = LcurY + 12
                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Washing ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("washing").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Entertainment").ToString) > 0 Then
                            LcurY = LcurY + 12
                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Entertainment ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Entertainment").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Bonus").ToString) > 0 Then
                            LcurY = LcurY + 12
                            'CurY = CurY + TxtHgt - 3
                            Common_Procedures.Print_To_PrintDocument(e, "Over Time Bonus ", LMargin + 10, LcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Bonus").ToString), "#######0.00"), LMargin + ClArr(1) + 50, LcurY, 1, 0, p1Font)
                        End If


                        RcurY = CurY + 3
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Mess").ToString) > 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, "Mess", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("mess").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                        End If
                        'Common_Procedures.Print_To_PrintDocument(e, "Miniumum Wages Earned in the Week", LMargin + 10, CurY, 0, 0, p1Font)


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Medical").ToString) > 0 Then
                            RcurY = RcurY + 12
                            'CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Medical", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Medical").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                        End If

                        'CurY = CurY + TxtHgt - 8

                        'Common_Procedures.Print_To_PrintDocument(e, "Total other allowance in the Week", LMargin + 10, CurY, 0, 0, p1Font)


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bonus_OT_ESI").ToString) > 0 Then
                            RcurY = RcurY + 12
                            Common_Procedures.Print_To_PrintDocument(e, "ESI 1.75% & OT ESI", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Bonus_OT_ESI").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                        End If



                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("P_F").ToString) > 0 Then
                            RcurY = RcurY + 12
                            'CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "P F 12%", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("P_F").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                        End If








                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("E_P_F").ToString) > 0 Then
                        '    CurY = CurY + TxtHgt
                        '    Common_Procedures.Print_To_PrintDocument(e, "EPF 8.33%", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p1Font)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("E_P_F").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 65, CurY, 1, 0, p1Font)
                        'End If
                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("Store").ToString) > 0 Then
                        '    'CurY = CurY + TxtHgt
                        '    Common_Procedures.Print_To_PrintDocument(e, "Store", LMargin + ClArr(1) - 137, CurY, 1, 0, p1Font)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Store").ToString, LMargin + ClArr(1) - 40, CurY, 0, 0, p1Font)
                        'End If

                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pension_Scheme").ToString) > 0 Then
                        '    CurY = CurY + TxtHgt
                        '    Common_Procedures.Print_To_PrintDocument(e, "Pension Scheme", LMargin + ClArr(1) - 170, CurY, 0, 0, p1Font)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pension_Scheme").ToString, LMargin + ClArr(1) - 40, CurY, 0, 0, p1Font)
                        'End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Store").ToString) > 0 Then
                            RcurY = RcurY + 12
                            Common_Procedures.Print_To_PrintDocument(e, "Store", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Store").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Minus_Advance").ToString) > 0 Then
                            RcurY = RcurY + 12
                            Common_Procedures.Print_To_PrintDocument(e, "Advance Deduction", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Minus_Advance").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Late_Hours_Bonus").ToString) > 0 Then
                            RcurY = RcurY + 12
                            Common_Procedures.Print_To_PrintDocument(e, "Late Hours Bonus", LMargin + ClArr(1) + ClArr(2) - 35, RcurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Late_Hours_Bonus").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, RcurY, 1, 0, p1Font)
                        End If

                        'CurY = CurY + TxtHgt
                        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Bonus").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Minus_Advance").ToString) > 0 Then
                            CurY = RcurY + TxtHgt
                        ElseIf Val(prn_DetDt.Rows(prn_DetIndx).Item("Minus_Advance").ToString) > 0 Then
                            CurY = RcurY + TxtHgt
                        Else
                            CurY = LcurY + TxtHgt
                        End If


                        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(4) = CurY
                        nprntot = Val(prn_DetDt.Rows(prn_DetIndx).Item("Earning").ToString)
                        nOTsal = Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_Bonus").ToString)
                        Common_Procedures.Print_To_PrintDocument(e, "Total Earnings ", LMargin + 10, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Addition").ToString + Val(nprntot) + Val(nOTsal))), LMargin + ClArr(1) + 50, CurY, 1, 0, p2font) 'LMargin + ClArr(1) - 15, CurY, 0, 0, p2font)
                        nTotAdv = Val(prn_DetDt.Rows(prn_DetIndx).Item("Minus_Advance").ToString)
                        nOTEsi = Val(prn_DetDt.Rows(prn_DetIndx).Item("OT_ESI").ToString)
                        Common_Procedures.Print_To_PrintDocument(e, "Total Deductions", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Deduction").ToString + Val(nTotAdv) + Val(nOTEsi))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, CurY, 1, 0, p2font)
                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClArr(1) + ClArr(2) - 35, CurY, 0, 0, p2font)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Net_Pay").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 130, CurY, 1, 0, p3font)
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(8) = CurY

                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("Entertainment").ToString) > 0 Then
                        '    CurY = CurY + TxtHgt
                        '    Common_Procedures.Print_To_PrintDocument(e, "ENTERTIANMENT ", LMargin + 10, CurY, 0, 0, p1Font)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Entertainment").ToString, LMargin + ClArr(1) - 200, CurY, 1, 0, p1Font)
                        'End If

                        CurY = CurY + TxtHgt
                        'Common_Procedures.Print_To_PrintDocument(e, "xU kzp Neuj;jpw;Fhpa $Ljy; rk;gsk; ", LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "E.S.I", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + ClArr(1) + 110, CurY, 0, 0, p1Font)
                        ' Common_Procedures.Print_To_PrintDocument(e, ",.v];.I ", LMargin + ClArr(1) + 120, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "@ 1.75 %", LMargin + ClArr(1) + 170, CurY, 0, 0, p1Font)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ESI").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                        'CurY = CurY + TxtHgt - 8


                        'CurY = CurY + TxtHgt
                        'Common_Procedures.Print_To_PrintDocument(e, "thuj;jpd; Ntiy nra;j $Ljy; kzp Neuk; ", LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "Other Deductions", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, "/", LMargin + ClArr(1) + 110, CurY, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, ",ju gpbj;jq;fs; ", LMargin + ClArr(1) + 120, CurY + 2, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ot_Hours").ToString, LMargin + ClArr(1) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Other_Deduction").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                        'CurY = CurY + TxtHgt - 8
                        'Common_Procedures.Print_To_PrintDocument(e, "Total OT Hours Worked in the week", LMargin + 10, CurY, 0, 0, p1Font)

                        'CurY = CurY + TxtHgt
                        'Common_Procedures.Print_To_PrintDocument(e, "$Ljy; Neuj;jpw;fhd nkhj;j rk;gsk; ", LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "P.Tax / L.W.F", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ot_Bonus").ToString, LMargin + ClArr(1) - 10, CurY, 1, 0, pFont)
                        'CurY = CurY + TxtHgt - 8
                        'Common_Procedures.Print_To_PrintDocument(e, "Wages for OT Hours Worked ", LMargin + 10, CurY, 0, 0, p1Font)


                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If

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

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 8, FontStyle.Bold)
        If Val(prn_HdDt.Rows(0).Item("Month_IdNo").ToString) >= 4 Then
            Common_Procedures.Print_To_PrintDocument(e, "Wage Slip (With form 25-B) for the month of " & prn_HdDt.Rows(0).Item("Month_Name").ToString & " - " & Trim(Year(Common_Procedures.Company_FromDate)), LMargin + 110, CurY, 0, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Wage Slip (With form 25-B) for the month of " & prn_HdDt.Rows(0).Item("Month_Name").ToString & " - " & Trim(Year(Common_Procedures.Company_ToDate)), LMargin + 110, CurY, 0, PrintWidth, p1Font)
        End If

        CurY = CurY + TxtHgt - 5
        Common_Procedures.Print_To_PrintDocument(e, "FORM No.VI (See Sub Rule7) FORM NO.15(Prescibed under Rule 87 and 88)", LMargin + 85, CurY, 0, PrintWidth, p1Font)

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
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Bonus_No").ToString, LMargin + 115, CurY, 0, 0, pFont)
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
            Common_Procedures.Print_To_PrintDocument(e, "Value", LMargin + 450 - 220, CurY, 1, 0, p2Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 40, LnAr(3), LMargin + ClAr(1) + ClAr(2) - 40, LnAr(2))

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "<l;ba rk;gsk;", LMargin, CurY, 2, ClAr(1), p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "gpbj;jq;fs;", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)



            CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 30, PageWidth, ClAr(1))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 30, LnAr(5), LMargin + ClAr(1) - 30, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - ClAr(1) - ClAr(2) - 15, CurY)
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

        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Net_Bonus").ToString, LMargin + 120, CurY, 0, 0, p2Font)
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

        'Common_Procedures.Print_To_PrintDocument(e, "Bonus Paid on :", LMargin + 300, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
    End Sub

    Private Sub txt_BonusRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BonusRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_BonusRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_BonusRate.LostFocus
        txt_BonusRate.Text = Format(Val(txt_BonusRate.Text), "#########0.00")
    End Sub

    'Private Sub dgv_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    'End Sub

    'Private Sub dtp_FromDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_FromDate.ValueChanged

    'End Sub

    'Private Sub cbo_Category_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Category.SelectedIndexChanged

    'End Sub

    'Private Sub dtp_ToDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_ToDate.ValueChanged

    'End Sub

    'Private Sub txt_BonusRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_BonusRate.TextChanged

    'End Sub

    Private Sub txt_BonusRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_BonusRate.TextChanged

    End Sub

    Private Sub txt_MaxShifts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MaxShifts.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_MinAttendance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinAttendance.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_MinAttendance_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MinAttendance.LostFocus
        txt_MinAttendance.Text = Format(Val(txt_MinAttendance.Text), "#########0.00")
    End Sub

    Private Sub txt_MaxShifts_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MaxShifts.LostFocus
        txt_MaxShifts.Text = Format(Val(txt_MaxShifts.Text), "#########0.00")
    End Sub


    Private Sub chk_ExcludeWO_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_ExcludeWO.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub chk_ExcludePH_LH_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_ExcludePH_LH.CheckedChanged

    End Sub

    Private Sub chk_ExcludePH_LH_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_ExcludePH_LH.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub btn_Compute_Bonus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Compute_Bonus.Click
        get_PayRoll_Bonus_Details()
        TotalBonus()
    End Sub

    Private Sub dtp_FromDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_FromDate.LostFocus

        'Try

        '    'If Asc(e.KeyChar) = 13 Then
        '    If DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value) > 13 Then
        '        MsgBox("Time Interval Is Too Long To Generate Bonus. Cannot Continue")
        '        dtp_ToDate.Focus()
        '    End If


        '    If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Then

        '        'SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

        '        For I As Integer = 11 To 24

        '            dgv_Details.Columns(I).HeaderText = ""
        '            dgv_Details.Columns(I).Visible = False

        '        Next

        '        MonthCnt = 0

        '        If DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value) >= 0 Then

        '            For I As Integer = 0 To DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value)

        '                dgv_Details.Columns(I + 11).HeaderText = Format(DateAdd(DateInterval.Month, I, dtp_FromDate.Value), "MMM- yyyy")
        '                dgv_Details.Columns(I + 11).Visible = True
        '                MonthCnt = MonthCnt + 1

        '            Next

        '        End If

        '    End If

        '    ' End If

        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "ERROR WHILE FROMDATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub dtp_FromDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_FromDate.ValueChanged

    End Sub

    Private Sub dtp_ToDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ToDate.LostFocus

        'Dim Mon_Wek As String = ""
        'Dim SalPymtTyp_IdNo As Integer = 0

        ''Try
        ''If Asc(e.KeyChar) = 13 Then

        'If DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value) > 13 Then
        '    MsgBox("Time Interval Is Too Long To Generate Bonus. Cannot Continue")
        '    txt_BonusRate.Focus()
        'End If

        'If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Then

        '    For I As Integer = 11 To 24

        '        dgv_Details.Columns(I).HeaderText = ""
        '        dgv_Details.Columns(I).Visible = False

        '    Next

        '    MonthCnt = 0

        '    If DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value) >= 0 Then

        '        For I As Integer = 0 To DateDiff(DateInterval.Month, dtp_FromDate.Value, dtp_ToDate.Value)

        '            dgv_Details.Columns(I + 11).HeaderText = Format(DateAdd(DateInterval.Month, I, dtp_FromDate.Value), "MMM- yyyy")
        '            dgv_Details.Columns(I + 11).Visible = True
        '            MonthCnt = MonthCnt + 1

        '        Next

        '    End If

        'End If

        ' End If


    End Sub

    Private Sub dtp_ToDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_ToDate.ValueChanged

    End Sub

    Private Sub dgv_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub cbo_PaymentType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_PaymentType.SelectedIndexChanged

    End Sub

    Private Sub cbo_Category_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Category.SelectedIndexChanged

    End Sub

    Private Sub btn_BonusList_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BonusList.Click

    End Sub
End Class
