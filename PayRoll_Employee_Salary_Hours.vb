Public Class PayRoll_Employee_Salary_Hours

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EMSAH-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Public Sub New()
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

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, 1)
        dtp_FromDate.Text = ""
        cbo_Month.Text = ""
        dtp_ToDate.Text = ""
        txt_FestivalDays.Text = ""
        txt_TotalDays.Text = ""

        dgv_Details.Rows.Clear()

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

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Salary_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Salary_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Salary_Date").ToString
                cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, Val(dt1.Rows(0).Item("Salary_Payment_Type_IdNo").ToString))
                cbo_Month.Text = Common_Procedures.Month_IdNoToName(con, Val(dt1.Rows(0).Item("Month_IdNo").ToString))
                dtp_FromDate.Text = dt1.Rows(0).Item("From_Date").ToString
                dtp_ToDate.Text = dt1.Rows(0).Item("To_Date").ToString
                txt_TotalDays.Text = Val(dt1.Rows(0).Item("Total_Days").ToString)
                txt_FestivalDays.Text = Val(dt1.Rows(0).Item("Festival_Days").ToString)

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name from PayRoll_Salary_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  Where a.Salary_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
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

                            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Salary_Shift").ToString)
                            If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""

                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Working_Hours").ToString)
                            If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Basic_Salary").ToString)
                            If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                            .Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Ot_Hours").ToString)
                            If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""

                            .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Ot_Pay_Hours").ToString)
                            If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                            .Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Ot_Salary").ToString)
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                            .Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("Incentive_Amount").ToString)
                            If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                            .Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Total_Salary").ToString)
                            If Val(.Rows(n).Cells(9).Value) = 0 Then .Rows(n).Cells(9).Value = ""

                            .Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Mess").ToString)
                            If Val(.Rows(n).Cells(10).Value) = 0 Then .Rows(n).Cells(10).Value = ""

                            .Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Advance").ToString)
                            If Val(.Rows(n).Cells(11).Value) = 0 Then .Rows(n).Cells(11).Value = ""

                            .Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("Minus_Advance").ToString)
                            If Val(.Rows(n).Cells(12).Value) = 0 Then .Rows(n).Cells(12).Value = ""

                            .Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("Balance_Advance").ToString)
                            If Val(.Rows(n).Cells(13).Value) = 0 Then .Rows(n).Cells(13).Value = ""

                            .Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Net_Salary").ToString), "#########0.00")
                            If Val(.Rows(n).Cells(14).Value) = 0 Then .Rows(n).Cells(14).Value = ""

                            .Rows(n).Cells(15).Value = Val(dt2.Rows(i).Item("OT_Minutes").ToString)
                            If Val(.Rows(n).Cells(15).Value) = 0 Then .Rows(n).Cells(15).Value = ""

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

            End If

            Grid_Cell_DeSelect()

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

        Dim n As Integer = 0
        Dim wrk_Hrs As Integer = 0
        Dim OT_wrk_dys As Double = 0
        Dim Incen As Double = 0
        Dim Salary As Double = 0
        Dim Sal_Shft As Double = 0
        Dim Bas_Sal As Double = 0
        Dim OT_Sal_Shft As Double = 0
        Dim OT_Salary As Double = 0
        Dim Amt_OpBal As Double
        Dim Cmp_Cond As String = ""
        Dim mins_Adv As Double = 0
        Dim mess_Ded As Double = 0
        Dim OT_Mins As Integer = 0
        Dim Ot_Dbl As Double = 0
        Dim Ot_Int As Integer = 0
        Dim Ot_minVal As Integer = 0
        Dim Net_Salary As Double = 0
        Dim Sal_Mins As Integer = 0
        Dim Sht_Mins As Integer = 0
        Dim Sht_Dbl As Double = 0
        Dim Sht_Int As Integer = 0
        Dim Sht_minVal As Integer = 0
        Dim Shft_Hrs As Single = 0
        Dim h As Integer = 0
        Dim min As Single = 0
        Dim shft_Mins As Integer = 0


        Dim SNo As Integer

        Dim SalPymtTyp_IdNo As Integer = 0


        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
        cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

        SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

        cmd.CommandText = "select a.Employee_Name, a.Employee_IdNo, a.Shift_Day_Month, b.No_Days_Month_Wages from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo <> 0 and a.Category_IdNo = b.Category_IdNo Where a.Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & " and a.Join_DateTime <= @ToDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @ToDate ) ) "
        da1 = New SqlClient.SqlDataAdapter(cmd)
        dt1 = New DataTable
        da1.Fill(dt1)

        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    n = dgv_Details.Rows.Add()

                    SNo = SNo + 1

                    Amt_OpBal = 0

                    cmd.CommandText = "select sum(a.Amount) as Op_Balance from PayRoll_Employee_Payment_Head a where a.Employee_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and a.Employee_Payment_Date <= @ToDate and a.Advance_Salary = 'ADVANCE'"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = Val(dt4.Rows(0).Item("Op_Balance").ToString)
                    End If
                    dt4.Clear()

                    cmd.CommandText = "select Sum(Advance_Deduction_Amount) as Lessadv from PayRoll_Employee_Deduction_Head where Employee_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and Employee_Deduction_Date < @fromdate"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    dt4 = New DataTable
                    Da.Fill(dt4)
                    If dt4.Rows.Count > 0 Then
                        If IsDBNull(dt4.Rows(0).Item("Lessadv").ToString) = False Then Amt_OpBal = Amt_OpBal - Val(dt4.Rows(0).Item("Lessadv").ToString)
                    End If
                    dt4.Clear()

                    mins_Adv = 0
                    cmd.CommandText = "select Sum(Advance_Deduction_Amount) as adv from PayRoll_Employee_Deduction_Head where Employee_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and Employee_Deduction_Date between @fromdate and @toDate "
                    da5 = New SqlClient.SqlDataAdapter(cmd)
                    dt5 = New DataTable
                    da5.Fill(dt5)
                    If dt5.Rows.Count > 0 Then
                        If IsDBNull(dt5.Rows(0).Item("adv").ToString) = False Then
                            mins_Adv = Format(Val(dt5.Rows(0).Item("adv").ToString), "########0.00")
                        End If
                    End If
                    dt5.Clear()

                    wrk_Hrs = 0
                    Incen = 0
                    OT_Mins = 0
                    Sht_Mins = 0
                    cmd.CommandText = "select sum(a.Shift_Hours) as WRKING_Hours ,sum(a.Shift_minutes) as Sht_Mins, Sum(a.Incentive_Amount) as Incen ,  Sum(A.OT_Minutes) as Ot_Mins from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = B.Employee_IdNo where a.Employee_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and Employee_Attendance_Date between @fromdate and @toDate "
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("WRKING_Hours").ToString) = False Then
                            wrk_Hrs = Val(dt2.Rows(0).Item("WRKING_Hours").ToString)
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Sht_Mins").ToString) = False Then
                            Sht_Mins = Val(dt2.Rows(0).Item("Sht_mins").ToString)
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Incen").ToString) = False Then
                            Incen = Format(Val(dt2.Rows(0).Item("Incen").ToString), "########0.00")
                        End If
                        If IsDBNull(dt2.Rows(0).Item("Ot_Mins").ToString) = False Then
                            OT_Mins = Val(dt2.Rows(0).Item("Ot_Mins").ToString)
                        End If
                    End If
                    dt2.Clear()
                    cmd.CommandText = "select b.Shift1_Working_Hours from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo = b.Category_IdNo where Employee_IdNo = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString))
                    da2 = New SqlClient.SqlDataAdapter(cmd)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    Sal_Mins = 0
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("Shift1_Working_Hours").ToString) = False Then
                            Shft_Hrs = Val(dt2.Rows(0).Item("Shift1_Working_Hours").ToString)
                            h = Fix(Shft_Hrs)
                            Sal_Mins = (Val(Shft_Hrs) - h) * 100
                            Sal_Mins = (h * 60) + Sal_Mins
                        End If
                    End If


                    cmd.CommandText = "SELECT TOP 1 * from PayRoll_Employee_Salary_Details a Where a.employee_idno = " & Str(Val(dt1.Rows(i).Item("Employee_IdNo").ToString)) & " and ( ( @FromDate < (select min(y.From_DateTime) from PayRoll_Employee_Salary_Details y where y.employee_idno = a.employee_idno )) or (@FromDate BETWEEN a.From_DateTime and a.To_DateTime) or ( @FromDate >= (select max(z.From_DateTime) from PayRoll_Employee_Salary_Details z where z.employee_idno = a.employee_idno ))) order by a.From_DateTime desc"
                    da3 = New SqlClient.SqlDataAdapter(cmd)
                    dt3 = New DataTable
                    da3.Fill(dt3)

                    Salary = 0
                    OT_Sal_Shft = 0
                    OT_Salary = 0
                    mess_Ded = 0

                    If dt3.Rows.Count > 0 Then
                        If IsDBNull(dt3.Rows(0).Item("For_Salary").ToString) = False Then
                            Salary = Format(Val(dt3.Rows(0).Item("For_Salary").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("O_T").ToString) = False Then
                            OT_Sal_Shft = Format(Val(dt3.Rows(0).Item("O_T").ToString), "########0.00")
                        End If
                        If IsDBNull(dt3.Rows(0).Item("MessDeduction").ToString) = False Then
                            mess_Ded = Format(Val(dt3.Rows(0).Item("MessDeduction").ToString), "########0.00")
                        End If
                    End If
                    dt3.Clear()


                    Sal_Shft = 0
                    If Trim(UCase(dt1.Rows(i).Item("Shift_Day_Month").ToString)) = "MONTH" Then
                        If Val(dt1.Rows(i).Item("No_Days_Month_Wages").ToString) <> 0 Then
                            Sal_Shft = Format(Salary / Val(dt1.Rows(i).Item("No_Days_Month_Wages").ToString), "########0.00")
                        Else
                            Sal_Shft = Format(Salary / 26, "########0.00")
                        End If

                    Else
                        Sal_Shft = Salary

                    End If
                    ' Sal_Mins = Val(Sal_Shft) / Val(wrk_Hrs * 60)

                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Employee_Name").ToString

                    .Rows(n).Cells(2).Value = Val(Sal_Shft)
                    If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""


                    Sht_Int = Int(Sht_Mins / 60)
                    Sht_minVal = Sht_Int * 60
                    Sht_Dbl = (Sht_Mins - Sht_minVal) / 100

                    .Rows(n).Cells(3).Value = Format(Sht_Dbl + Sht_Int, "#########0.00")
                    If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(5).Value = ""


                    Bas_Sal = Format(Val(Sht_Mins) * IIf(Sal_Mins <> 0, Val(Sal_Shft / (Sal_Mins)), 0), "#########0.00")

                    .Rows(n).Cells(4).Value = Val(Bas_Sal)
                    If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                    Ot_Int = Int(OT_Mins / 60)
                    Ot_minVal = Ot_Int * 60
                    Ot_Dbl = (OT_Mins - Ot_minVal) / 100

                    .Rows(n).Cells(5).Value = Format(Ot_Dbl + Ot_Int, "#########0.00")
                    If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""

                    .Rows(n).Cells(6).Value = Format(IIf(Sal_Mins <> 0, (OT_Sal_Shft / Sal_Mins) * 60, 0), "##########0.00") ' Val(OT_Sal_Shft)
                    If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                    OT_Salary = Format(OT_Mins * IIf(Sal_Mins <> 0, (OT_Sal_Shft / Sal_Mins), 0), "##########0.00")
                    .Rows(n).Cells(7).Value = Val(OT_Salary)
                    If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                    .Rows(n).Cells(8).Value = Val(Incen)
                    If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                    .Rows(n).Cells(9).Value = Val(Val(.Rows(n).Cells(4).Value) + Val(.Rows(n).Cells(7).Value) + Val(.Rows(n).Cells(8).Value))
                    If Val(.Rows(n).Cells(9).Value) = 0 Then .Rows(n).Cells(9).Value = ""

                    .Rows(n).Cells(10).Value = Val(mess_Ded)
                    If Val(.Rows(n).Cells(10).Value) = 0 Then .Rows(n).Cells(10).Value = ""

                    .Rows(n).Cells(11).Value = Val(Amt_OpBal)
                    If Val(.Rows(n).Cells(11).Value) = 0 Then .Rows(n).Cells(11).Value = ""

                    .Rows(n).Cells(12).Value = Val(mins_Adv)
                    If Val(.Rows(n).Cells(12).Value) = 0 Then .Rows(n).Cells(12).Value = ""

                    .Rows(n).Cells(13).Value = Val(Amt_OpBal - mins_Adv)
                    If Val(.Rows(n).Cells(13).Value) = 0 Then .Rows(n).Cells(13).Value = ""

                    Net_Salary = Format(Val(.Rows(n).Cells(9).Value) - mess_Ded - mins_Adv, "##########0")
                    .Rows(n).Cells(14).Value = Format(Net_Salary, "#########0.00")
                    If Val(.Rows(n).Cells(14).Value) = 0 Then .Rows(n).Cells(14).Value = ""

                    .Rows(n).Cells(15).Value = OT_Mins
                    If Val(.Rows(n).Cells(15).Value) = 0 Then .Rows(n).Cells(15).Value = ""

                Next i

            End If
            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()
        End With

        Grid_Cell_DeSelect()

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

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentType.GotFocus, AddressOf ControlGotFocus
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
        AddHandler dtp_FromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Month.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FestivalDays.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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

            cmd.CommandText = "delete from PayRoll_Salary_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_No from PayRoll_Salary_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_No desc", con)
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

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Salary_Head", "Salary_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

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

            Da = New SqlClient.SqlDataAdapter("select Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "'", con)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Salary_No from PayRoll_Salary_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(Pk_Condition) & Trim(InvCode) & "'", con)
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
        Dim Mon_Wek As String = "", VouNarr As String = ""
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

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

        cmd.CommandText = "select * from PayRoll_Salary_Head where Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & " and Salary_Code <> '" & Trim(Pk_Condition) & Trim(NewCode) & "' and ( (@SalaryFromDate Between From_Date and To_Date) or (@SalaryToDate Between From_Date and To_Date) )"
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

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Or Val(.Rows(i).Cells(10).Value) <> 0 Or Val(.Rows(i).Cells(13).Value) <> 0 Then

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

        VouNarr = ""

        If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
            VouNarr = "Salary for Week " & dtp_FromDate.Text & " to " & dtp_ToDate.Text

        Else
            VouNarr = "Salary for Month " & cbo_Month.Text

        End If

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


            If New_Entry = True Then
                cmd.CommandText = "Insert into PayRoll_Salary_Head (     Salary_Code        ,               Company_IdNo       ,           Salary_No           ,                               for_OrderBy                              ,   Salary_Date ,       Salary_Payment_Type_IdNo   ,          Month_IdNo  ,  From_Date,  To_Date,             Total_Days              ,                  Festival_Days           ) " & _
                                    "     Values           ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @SalaryDate  , " & Str(Val(SalPymtTyp_IdNo)) & ", " & Val(Mth_IDNo) & ", @FromDate , @ToDate , " & Str(Val(txt_TotalDays.Text)) & ",  " & Str(Val(txt_FestivalDays.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update PayRoll_Salary_Head set Salary_Date = @SalaryDate, Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ",  Month_IdNo = " & Val(Mth_IDNo) & ", From_Date = @FromDate  ,  To_Date =  @ToDate   , Total_Days = " & Str(Val(txt_TotalDays.Text)) & ", Festival_Days = " & Str(Val(txt_FestivalDays.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from PayRoll_Salary_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "' and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%/" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(Emp_ID) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into PayRoll_Salary_Details ( Salary_Code              ,               Company_IdNo       ,            Salary_No          ,                               for_OrderBy                              ,   Salary_Date,            Sl_No     ,        Employee_IdNo    ,           Salary_Shift                   ,             Working_Hours      ,                      Basic_Salary        ,                      Ot_Hours            ,                       Ot_Pay_Hours        ,                      Ot_Salary           ,                      Incentive_Amount    ,                      Total_Salary        ,                       Mess                 ,                      Advance              ,                      Minus_Advance        ,                      Balance_Advance      ,                      Net_Salary           ,                      OT_Minutes            ) " & _
                                            "     Values              (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @SalaryDate , " & Str(Val(Sno)) & ", " & Str(Val(Emp_ID)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ",  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ",  " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & ", " & Str(Val(.Rows(i).Cells(15).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        If Val(.Rows(i).Cells(14).Value) <> 0 Then
                            If Val(.Rows(i).Cells(14).Value) < 0 Then
                                vLed_IdNos = Common_Procedures.CommonLedger.Salary_Ac & "|" & Emp_ID

                            Else
                                vLed_IdNos = Emp_ID & "|" & Common_Procedures.CommonLedger.Salary_Ac

                            End If
                            vVou_Amts = Math.Abs(Val(.Rows(i).Cells(14).Value) + Val(.Rows(i).Cells(12).Value)) & "|" & -1 * Math.Abs(Val(.Rows(i).Cells(14).Value) + Val(.Rows(i).Cells(12).Value))

                            If Common_Procedures.Voucher_Updation(con, "Emp.Sal", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(Val(Emp_ID)) & "/" & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, Trim(VouNarr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                                Throw New ApplicationException(ErrMsg)
                                Exit Sub
                            End If

                        End If

                    End If

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentType, dtp_Date, cbo_Month, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")
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

                            cbo_Month.Enabled = False

                        Else
                            cbo_Month.Enabled = True
                            dtp_FromDate.Enabled = False
                            dtp_ToDate.Enabled = False

                            dtp_FromDate.Text = ""
                            dtp_ToDate.Text = ""

                            cbo_Month.Focus()

                        End If

                    Else

                        If Trim(UCase(Mon_Wek)) = "WEEKLY" Then
                            dtp_FromDate.Enabled = True
                            dtp_ToDate.Enabled = True

                            dtp_FromDate.Focus()

                            cbo_Month.Enabled = False

                        Else
                            cbo_Month.Enabled = True
                            dtp_FromDate.Enabled = False
                            dtp_ToDate.Enabled = False

                            cbo_Month.Focus()

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Month, cbo_PaymentType, dtp_FromDate, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_Month_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Month.KeyPress
        Dim dttm As Date
        Dim Mth_ID As Integer = 0

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Month, Nothing, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")

            If Asc(e.KeyChar) = 13 And Trim(cbo_Month.Text) <> "" Then

                If Trim(UCase(cbo_Month.Tag)) <> Trim(UCase(cbo_Month.Text)) Then

                    Mth_ID = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

                    dttm = New DateTime(IIf(Mth_ID >= 4, Year(Common_Procedures.Company_FromDate), Year(Common_Procedures.Company_ToDate)), Mth_ID, 1)

                    dtp_FromDate.Text = dttm

                    dttm = DateAdd("M", 1, dttm)
                    dttm = DateAdd("d", -1, dttm)

                    dtp_ToDate.Text = dttm

                    get_PayRoll_Salary_Details()


                End If

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
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
    End Sub

    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ToDate.KeyPress
        'Dim DtTm As Date
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try
            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Or Trim(UCase(dtp_ToDate.Tag)) <> Trim(UCase(dtp_ToDate.Text)) Then

                    'SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

                    'Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

                    'If Trim(UCase(Mon_Wek)) = "WEEKLY" Then

                    '    DtTm = dtp_FromDate.Value.Date

                    '    DtTm = DateAdd("d", 6, DtTm)

                    '    dtp_ToDate.Text = DtTm

                    'End If

                    get_PayRoll_Salary_Details()

                End If

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
                txt_TotalDays.Text = DateDiff("d", dtp_FromDate.Text, dtp_ToDate.Text) + 1
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE TODATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dtp_FromDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_FromDate.GotFocus
        dtp_FromDate.Tag = dtp_FromDate.Text
    End Sub

    Private Sub dtp_FromDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FromDate.KeyPress
        Dim DtTm As Date
        Dim Mon_Wek As String = ""
        Dim SalPymtTyp_IdNo As Integer = 0

        Try

            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_FromDate.Tag)) <> Trim(UCase(dtp_FromDate.Text)) Then

                    SalPymtTyp_IdNo = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)

                    Mon_Wek = Common_Procedures.get_FieldValue(con, "PayRoll_Salary_Payment_Type_Head", "Monthly_Weekly", "(Salary_Payment_Type_IdNo = " & Str(Val(SalPymtTyp_IdNo)) & ")")

                    If Trim(UCase(Mon_Wek)) = "WEEKLY" Then

                        DtTm = dtp_FromDate.Value.Date

                        DtTm = DateAdd("d", 6, DtTm)

                        dtp_ToDate.Text = DtTm

                    End If

                    get_PayRoll_Salary_Details()

                End If
                txt_TotalDays.Text = DateDiff("d", dtp_FromDate.Text, dtp_ToDate.Text) + 1
            End If




        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE FROMDATE KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
    End Sub


End Class