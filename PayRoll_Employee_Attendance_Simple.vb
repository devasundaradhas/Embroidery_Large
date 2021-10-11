Public Class PayRoll_Employee_Attendance_Simple

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Emp_ID As Integer

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private PK_Condition As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Public previlege As String

    Public Sub New()

        FrmLdSTS = True
        'This call is required by the designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call.
        clear()

    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        lbl_Day.Text = ""
        dtp_Date.Text = ""
        lbl_Day.Text = Trim(Format(dtp_Date.Value, "dddddd"))

        dgv_Details.Rows.Clear()

        Grid_Cell_DeSelect()

        'dtp_Date.Enabled = False
        Panel2.Enabled = True

        grp_Open.Visible = False
        cbo_Employee_Name.Text = ""
        txt_Noof_Shift.Text = ""
        txt_Ot_Hours.Text = ""
        txt_MessAttendance.Text = ""
        txt_Incentive_Amount.Text = ""
        lbl_Category.Text = ""
        txt_SlNo.Text = "1"
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
            If TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(44, 61, 90)
                Prec_ActCtrl.ForeColor = Color.White
            Else
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Attendance_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Employee_Attendance_Code = '" & Trim(PK_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Employee_Attendance_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Employee_Attendance_Date").ToString

                lbl_Day.Text = dt1.Rows(0).Item("Day_Name").ToString


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name, c.Category_Name from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON b.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo LEFT OUTER JOIN PayRoll_Category_Head c ON c.Category_IdNo <> 0 and b.Category_IdNo = c.Category_IdNo Where a.Employee_Attendance_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Category_Name").ToString
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("No_Of_Shift").ToString)
                            If Val(dt2.Rows(i).Item("OT_Hours").ToString) <> 0 Then
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("OT_Hours").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Permission_Absence_Duration").ToString) <> 0 Then
                                .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Permission_Absence_Duration").ToString), "########0.00")
                            End If
                            .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Mess_Attendance").ToString)
                            If Val(dt2.Rows(i).Item("Incentive_Amount").ToString) <> 0 Then
                                .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Incentive_Amount").ToString), "########0.00")
                            End If
                        Next i

                    End If
                End With

            Else
                new_record()
            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Private Sub get_EmployeeList()
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim SNo As Integer
        Dim CompIDCondt As String = ""

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@AttDate", dtp_Date.Value.Date)

        Cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
        Cmd.Parameters.AddWithValue("@FromDate", dtp_Date.Value.Date)
        Cmd.Parameters.AddWithValue("@ToDate", dtp_Date.Value.Date)

        CompIDCondt = ""
        CompIDCondt = "(a.company_idno = 0 or a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"


        Cmd.CommandText = "truncate table EntryTemp_Simple"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into EntryTemp_Simple(Int1, Currency1) select tA.employee_idno, (SELECT TOP 1 MessDeduction from PayRoll_Employee_Salary_Details a Where a.employee_idno = tA.employee_idno and ( ( @FromDate < (select min(y.From_DateTime) from PayRoll_Employee_Salary_Details y where y.employee_idno = a.employee_idno )) or (@FromDate BETWEEN a.From_DateTime and a.To_DateTime) or ( @FromDate >= (select max(z.From_DateTime) from PayRoll_Employee_Salary_Details z where z.employee_idno = a.employee_idno ))) order by a.From_DateTime desc) as MessAmount from  PayRoll_Employee_Head tA"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "select a.*, b.Category_Name, c.Currency1 as MessDeuctionAmount from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo = b.Category_IdNo  LEFT OUTER JOIN EntryTemp_Simple c ON a.employee_idno = c.Int1 Where " & CompIDCondt & IIf(CompIDCondt <> "", " and ", "") & " a.Join_DateTime <= @AttDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @AttDate ) ) Order by Employee_Name"
        da1 = New SqlClient.SqlDataAdapter(Cmd)
        dt1 = New DataTable
        da1.Fill(dt1)

        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Employee_Name").ToString
                    .Rows(n).Cells(2).Value = dt1.Rows(i).Item("Category_Name").ToString
                    .Rows(n).Cells(3).Value = "1"
                    If IsDBNull(dt1.Rows(i).Item("MessDeuctionAmount").ToString) = False Then
                        If Val(dt1.Rows(i).Item("MessDeuctionAmount").ToString) <> 0 Then
                            .Rows(n).Cells(5).Value = "1"
                        End If
                    End If


                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then
                        .Rows(n).Cells(3).Value = ""
                        .Rows(n).Cells(5).Value = ""

                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then
                        .Rows(n).Cells(3).Value = ""
                        .Rows(n).Cells(5).Value = ""
                        If Val(dt1.Rows(i).Item("company_idno").ToString) = Val(lbl_Company.Tag) Then
                            .Rows(n).Cells(3).Value = 1
                            .Rows(n).Cells(5).Value = 0
                            If IsDBNull(dt1.Rows(i).Item("MessDeuctionAmount").ToString) = False Then
                                If Val(dt1.Rows(i).Item("MessDeuctionAmount").ToString) <> 0 Then
                                    .Rows(n).Cells(5).Value = "1"
                                End If
                            End If
                        End If

                    End If
                Next i

            End If

            Grid_Cell_DeSelect()

        End With
    End Sub

    Private Sub Employee_Attendance_Simple_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Employee_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Employee_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Employee_Attendance_Simple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable


        Me.Text = ""

        con.Open()


        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 100
        grp_Open.Top = (Me.Height - grp_Open.Height) - 100

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then
            dgv_Details.Columns(6).Visible = True

            dgv_Details.Columns(1).Width = 245
            dgv_Details.Columns(2).Width = 100
            dgv_Details.Columns(3).Width = 80
            dgv_Details.Columns(4).Width = 100
            dgv_Details.Columns(5).Width = 100
            dgv_Details.Columns(6).Width = 80

        End If


        Pnl_AbsentList.Visible = False
        Pnl_AbsentList.Left = 28
        Pnl_AbsentList.Top = 80
        Pnl_AbsentList.BringToFront()

        'Common_Procedures.get_VehicleNo_From_All_Entries(con)

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Employee_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Incentive_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ot_Hours.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Permission.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MessAttendance.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noof_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Employee_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Incentive_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ot_Hours.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Permission.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noof_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MessAttendance.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Noof_Shift.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Ot_Hours.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Permission.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MessAttendance.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_MessAttendance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Noof_Shift.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Permission.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Ot_Hours.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Employee_Attendance_Simple_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Employee_Attendance_Simple_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                'If pnl_Filter.Visible = True Then
                '    btn_Filter_Close_Click(sender, e)
                '    Exit Sub
                'Else
                Close_Form()

            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next


        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= 3 Then
                        'If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(3)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If

                        Else
                            If .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            Else
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                        save_record()
                                    Else
                                        dtp_Date.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(3)

                                End If

                            End If

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 3 Then
                        If .CurrentCell.RowIndex = 0 Then
                            dtp_Date.Focus()

                        Else
                            If .Columns(6).Visible = True Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(3)
                            End If

                        End If

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                    End If

                    Return True



                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Attendance_Manual, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Attendance_Manual, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

            cmd.CommandText = "delete from PayRoll_Employee_Attendance_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(NewCode) & "'"
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        ' 0 or Employee_Name
        da = New SqlClient.SqlDataAdapter("select Employee_Name from Payroll_Employee_Head where Employee_IdNo  =  " & Str(Val(Emp_ID)) & " order by Employee_Name", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Employee_Name"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        pnl_Back.Enabled = False

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(PK_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Employee_Attendance_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            'OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            cmd.CommandText = "select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where Employee_Attendance_Date > @EntryDate and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(PK_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by Employee_Attendance_Date, for_Orderby, Employee_Attendance_No"
            da = New SqlClient.SqlDataAdapter(cmd)
            'da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(PK_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by Employee_Attendance_Date, for_Orderby, Employee_Attendance_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            'OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            cmd.CommandText = "select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where Employee_Attendance_Date < @EntryDate and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(PK_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by Employee_Attendance_Date desc, for_Orderby desc, Employee_Attendance_No desc"
            da = New SqlClient.SqlDataAdapter(cmd)
            'da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(PK_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by Employee_Attendance_Date desc, for_Orderby desc, Employee_Attendance_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(PK_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Employee_Attendance_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Attendance_Head", "Employee_Attendance_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

            get_EmployeeList()

            Grid_Cell_DeSelect()

            dtp_Date.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

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

            Da = New SqlClient.SqlDataAdapter("select Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0

        Dim WrkTy_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim OT_Mins As Integer = 0
        Dim EntID As String = ""
        Dim Ot_Dbl As Double = 0
        Dim Ot_Int As Integer = 0
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If



        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Attendance_Manual, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If



        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        ' 

        Da = New SqlClient.SqlDataAdapter("Select Employee_Attendance_No from Payroll_Employee_Attendance_Head where Employee_Attendance_Date ='" & Format(dtp_Date.Value, "dd-MMM-yyyy") & "' and Not Employee_Attendance_No = " & Val(lbl_RefNo.Text).ToString, con)
        Dim dt As New DataTable
        Da.Fill(dt)

        If dt.Rows.Count > 0 Then
            MessageBox.Show("Attendance for this Date Is found in another Entry ( Entry No. :  " & dt.Rows(0).Item(0).ToString & "). Cannot Save !", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Then

                    Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Emp_ID = 0 Then
                        MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If


                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL
                        If Val(.Rows(i).Cells(3).Value) > 3 Then
                            MessageBox.Show("Invalid SHIFT", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(3)
                            End If
                            Exit Sub
                        End If
                    End If


                    If Val(.Rows(i).Cells(6).Value) > 3 Then
                        MessageBox.Show("Invalid MESS ATTANDANCE", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(4).Value) > 24 Then
                        MessageBox.Show("Invalid OT Hours", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        NoCalc_Status = False

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Attendance_Head", "Employee_Attendance_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into PayRoll_Employee_Attendance_Head ( Employee_Attendance_Code ,               Company_IdNo       ,       Employee_Attendance_No  ,                               for_OrderBy                              , Employee_Attendance_Date,               Day_Name            ) " &
                                    "           Values                  (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate         , '" & Trim(lbl_Day.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update PayRoll_Employee_Attendance_Head set Employee_Attendance_Date = @EntryDate, Day_Name = '" & Trim(lbl_Day.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from PayRoll_Employee_Attendance_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(Emp_ID) <> 0 Then

                        Sno = Sno + 1

                        Ot_Int = Int(Val(.Rows(i).Cells(4).Value))

                        Ot_Dbl = Val(.Rows(i).Cells(4).Value) - Val(Ot_Int)

                        OT_Mins = Val((Ot_Int) * 60) + Val(Ot_Dbl * 100)

                        cmd.CommandText = "Insert into PayRoll_Employee_Attendance_Details ( Employee_Attendance_Code ,               Company_IdNo       ,     Employee_Attendance_No    ,                               for_OrderBy                              , Employee_Attendance_Date,             Sl_No     ,        Employee_IdNo    ,                      No_Of_Shift         ,                      OT_Hours            ,           OT_Minutes     ,            Permission_Absence_Duration   ,       Mess_Attendance            ,  Incentive_Amount ) " &
                                          "            Values                      (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate         ,  " & Str(Val(Sno)) & "         , " & Str(Val(Emp_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(OT_Mins)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & " ,  " & Str(Val(.Rows(i).Cells(7).Value)) & " ) "
                        cmd.ExecuteNonQuery()


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

            If InStr(1, ex.Message, "IX_PayRoll_Employee_Attendance_Head") > 0 Then
                MessageBox.Show("Dupliacate Attendance Date", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, ex.Message, "IX_PayRoll_Employee_Attendance_Details") > 0 Then
                MessageBox.Show("Dupliacate employee for this Attendance Date", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


        End Try

    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        If FrmLdSTS = True Then Exit Sub
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details
                If .Rows.Count > 0 Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                        Else
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                        End If
                    End If

                End If

            End With
        Catch ex As Exception
            '----
        End Try

    End Sub


    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            If FrmLdSTS = True Then Exit Sub
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .EditingControl.BackColor = Color.Lime
                    .EditingControl.ForeColor = Color.Blue
                    dgtxt_Details.SelectAll()
                End If
            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    'Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
    '    vcbo_KeyDwnVal = e.KeyValue

    'End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details
                    If .Rows.Count >= 0 Then

                        n = .CurrentRow.Index

                        If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.GotFocus
        dtp_Date.Tag = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        Try
            If e.KeyValue = 40 Then
                txt_SlNo.Focus()

            End If

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""

        Try

            If Asc(e.KeyChar) = 13 Then

                Cmd.Connection = con

                Cmd.Parameters.Clear()
                If IsDate(dtp_Date.Text) = True Then
                    Cmd.Parameters.AddWithValue("@EntryDate", CDate(dtp_Date.Text))
                Else
                    Cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)
                End If


                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Trim(UCase(dtp_Date.Tag)) <> Trim(UCase(dtp_Date.Text)) Then

                    Cmd.CommandText = "select Employee_Attendance_No from PayRoll_Employee_Attendance_Head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Date = @EntryDate"
                    Da = New SqlClient.SqlDataAdapter(Cmd)
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
                        get_EmployeeList()
                    End If

                Else

                    Cmd.CommandText = "select Employee_Attendance_No from PayRoll_Employee_Attendance_Head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Date = @EntryDate and Employee_Attendance_Code <> '" & Trim(NewCode) & "'"
                    Da = New SqlClient.SqlDataAdapter(Cmd)
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
                        get_EmployeeList()
                    End If

                End If

                txt_SlNo.Focus()

            End If

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dtp_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.LostFocus
        Try
            lbl_Day.Text = Trim(Format(dtp_Date.Value, "dddddd"))
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dtp_Date_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.SizeChanged

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        Try
            lbl_Day.Text = Trim(Format(dtp_Date.Value, "dddddd"))
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
    End Sub

    Private Sub btn_AbsentList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AbsentList.Click
        Dim n As Integer
        Dim sno As Integer

        dgv_AbsentList.Rows.Clear()
        sno = 0
        If dgv_Details.Rows.Count > 0 Then

            For i = 0 To dgv_Details.Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(3).Value) = 0 Then 'And Val(dgv_Details.Rows(i).Cells(6).Value) = 0 Then

                    n = dgv_AbsentList.Rows.Add()
                    sno = sno + 1

                    dgv_AbsentList.Rows(n).Cells(0).Value = sno
                    dgv_AbsentList.Rows(n).Cells(1).Value = dgv_Details.Rows(i).Cells(1).Value

                End If

            Next i

        End If

        pnl_Back.Enabled = False
        Pnl_AbsentList.Visible = True

    End Sub
    Private Sub btn_absent_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pnl_Back.Enabled = True
        Pnl_AbsentList.Visible = False
    End Sub

    Private Sub dgv_AbsentList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_AbsentList.DoubleClick
        If dgv_Details.Rows.Count > 0 Then

            For i = 0 To dgv_Details.Rows.Count - 1
                If Trim(dgv_AbsentList.Rows(dgv_AbsentList.CurrentRow.Index).Cells(1).Value) = (dgv_Details.Rows(i).Cells(1).Value) Then
                    pnl_Back.Enabled = True
                    Pnl_AbsentList.Visible = False
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    dgv_Details.CurrentCell.Selected = True


                End If
            Next
        End If
    End Sub

    Private Sub btn_Close_AbsentLst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_AbsentLst.Click
        pnl_Back.Enabled = True
        Pnl_AbsentList.Visible = False
    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            Call btn_Find_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Employee_IdNo from PayRoll_Employee_Head where Employee_Name = '" & Trim(cbo_Open.Text) & "'", con)
        da.Fill(dt)

        movid = 0
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movid = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        dt.Dispose()
        da.Dispose()

        If movid <> 0 Then
            GetName(movid)
        Else
            new_record()
        End If

        btn_CloseOpen_Click(sender, e)
    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        pnl_Back.Enabled = True
        grp_Open.Visible = False
        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
    End Sub

    Private Sub GetName(ByVal no As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim n As Integer
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        'clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        '"select a.sl_No, eh.Employee_Name,c.Category_Name from PayRoll_Employee_Attendance_Details a inner join Employee_Head eh on a.Employee_IdNo = eh.employee_IdNo LEFT OUTER JOIN PayRoll_Category_Head c ON c.Category_IdNo where eh.Employee_IdNo = " & Str(Val(Emp_ID)) & "", con

        da = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name, c.Category_Name from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON b.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo LEFT OUTER JOIN PayRoll_Category_Head c ON c.Category_IdNo <> 0 and b.Category_IdNo = c.Category_IdNo Where b.Employee_Name = '" & (cbo_Open.Text) & "' and a.Employee_attendance_code = '" & Trim(NewCode) & "'", con)
        da.Fill(dt)
        With dgv_Details
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1

                    n = .Rows.Add()
                    no = no + 1

                    .Rows(n).Cells(0).Value = Val(no)
                    .Rows(n).Cells(1).Value = dt.Rows(i).Item("Employee_Name").ToString
                    .Rows(n).Cells(2).Value = dt.Rows(i).Item("Category_Name").ToString
                    .Rows(n).Cells(3).Value = Val(dt.Rows(i).Item("No_Of_Shift").ToString)
                    If Val(dt.Rows(i).Item("OT_Hours").ToString) <> 0 Then
                        .Rows(n).Cells(4).Value = Format(Val(dt.Rows(i).Item("OT_Hours").ToString), "########0.00")
                    End If
                    If Val(dt.Rows(i).Item("Incentive_Amount").ToString) <> 0 Then
                        .Rows(n).Cells(5).Value = Format(Val(dt.Rows(i).Item("Incentive_Amount").ToString), "########0.00")
                    End If
                    .Rows(n).Cells(6).Value = Val(dt.Rows(i).Item("Mess_Attendance").ToString)
                    .Focus()
                    .CurrentCell = .Rows(0).Cells(1)
                    'dgv_Details.Focus()
                    'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                Next i
            End If
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        End With

        dt.Dispose()
        da.Dispose()

        'If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub
    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Panel2.Enabled = True And txt_SlNo.Enabled = True Then


            If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

                txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                cbo_Employee_Name.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                lbl_Category.Text = (dgv_Details.CurrentRow.Cells(2).Value)
                '  cbo_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
                txt_Noof_Shift.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
                txt_Ot_Hours.Text = Format(Val(dgv_Details.CurrentRow.Cells(4).Value), "########0.00")
                txt_Permission.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                txt_MessAttendance.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
                txt_Incentive_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
                If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

            End If
        End If
    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next
            End If

        End With


        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_Employee_Name.Text = ""
        txt_MessAttendance.Text = ""
        txt_Noof_Shift.Text = ""
        txt_Ot_Hours.Text = ""
        lbl_Category.Text = ""
        txt_Incentive_Amount.Text = ""

        If cbo_Employee_Name.Enabled And cbo_Employee_Name.Visible Then cbo_Employee_Name.Focus()

    End Sub
    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : cbo_Employee_Name.Focus()
        If e.KeyCode = 38 Then e.Handled = True : dtp_Date.Focus()

    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress

        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then
            cbo_Employee_Name.Focus()
            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                        cbo_Employee_Name.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                        lbl_Category.Text = (dgv_Details.CurrentRow.Cells(2).Value)


                        txt_Noof_Shift.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
                        txt_Ot_Hours.Text = (dgv_Details.CurrentRow.Cells(4).Value)
                        txt_MessAttendance.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                        txt_Incentive_Amount.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.00")

                        Exit For


                    End If

                Next

            End With

        End If

    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim Emp_id As Integer = 0
        Dim Sz_id As Integer = 0

        Emp_id = Common_Procedures.Employee_NameToIdNo(con, cbo_Employee_Name.Text)

        If Val(Emp_id) = 0 Then
            MessageBox.Show("Invalid Employee Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_Employee_Name.Enabled Then cbo_Employee_Name.Focus()
            Exit Sub
        End If




        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL
            If Val(txt_Noof_Shift.Text) > 3 Then
                MessageBox.Show("Invalid Shift", "DOES NOT ADD...", MessageBoxButtons.OK)
                If txt_Noof_Shift.Enabled Then txt_Noof_Shift.Focus()
                Exit Sub
            End If
        End If
        If Val(txt_MessAttendance.Text) > 3 Then
            MessageBox.Show("Invalid Mess Attendance", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_MessAttendance.Enabled Then txt_MessAttendance.Focus()
            Exit Sub
        End If
        If Val(txt_Ot_Hours.Text) > 24 Then
            MessageBox.Show("Invalid OT Hours", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Ot_Hours.Enabled Then txt_Ot_Hours.Focus()
            Exit Sub
        End If




        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Trim(UCase(cbo_Employee_Name.Text)) = Trim(UCase(dgv_Details.Rows(i).Cells(1).Value)) Then

                    .Rows(i).Cells(1).Value = cbo_Employee_Name.Text
                    .Rows(i).Cells(2).Value = (lbl_Category.Text)
                    .Rows(i).Cells(3).Value = Val(txt_Noof_Shift.Text)

                    If Val(.Rows(i).Cells(3).Value) = 0 Then .Rows(i).Cells(3).Value = ""

                    .Rows(i).Cells(4).Value = Format(Val(txt_Ot_Hours.Text), "########0.00")
                    If Val(.Rows(i).Cells(4).Value) = 0 Then .Rows(i).Cells(4).Value = ""

                    .Rows(i).Cells(5).Value = Format(Val(txt_Permission.Text), "########0.00")
                    If Val(.Rows(i).Cells(5).Value) = 0 Then .Rows(i).Cells(5).Value = ""

                    .Rows(i).Cells(6).Value = Format(Val(txt_MessAttendance.Text), "########0.00")
                    If Val(.Rows(i).Cells(6).Value) = 0 Then .Rows(i).Cells(6).Value = ""

                    .Rows(i).Cells(6).Value = Format(Val(txt_MessAttendance.Text), "########0.00")
                    If Val(.Rows(i).Cells(6).Value) = 0 Then .Rows(i).Cells(6).Value = ""

                    .Rows(i).Cells(7).Value = Format(Val(txt_Incentive_Amount.Text), "########0.00")
                    If Val(.Rows(i).Cells(7).Value) = 0 Then .Rows(i).Cells(7).Value = ""


                    MtchSTS = True

                    cbo_Employee_Name.Focus()
                    ' If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_Employee_Name.Text
                .Rows(n).Cells(2).Value = (lbl_Category.Text)

                .Rows(n).Cells(3).Value = Val(txt_Noof_Shift.Text)
                If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""
                .Rows(n).Cells(4).Value = Format(Val(txt_Ot_Hours.Text), "########0.00")
                If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""
                .Rows(n).Cells(5).Value = Format(Val(txt_Permission.Text), "########0.00")
                If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                .Rows(n).Cells(5).Value = Format(Val(txt_MessAttendance.Text), "########0.00")
                If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                .Rows(n).Cells(6).Value = Format(Val(txt_Incentive_Amount.Text), "########0.00")
                If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                '.Rows(n).Selected = True

                cbo_Employee_Name.Focus()

                ' If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If

        End With



        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_Employee_Name.Text = ""
        txt_Incentive_Amount.Text = ""
        txt_MessAttendance.Text = ""
        txt_Noof_Shift.Text = ""
        txt_Ot_Hours.Text = ""
        lbl_Category.Text = ""

        Grid_Cell_DeSelect()
        If cbo_Employee_Name.Enabled And cbo_Employee_Name.Visible Then cbo_Employee_Name.Focus()


    End Sub
    Private Sub cbo_Employee_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Employee_Name.GotFocus



        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        cbo_Employee_Name.Tag = cbo_Employee_Name.Text
    End Sub

    Private Sub cbo_Employee_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Employee_Name.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Employee_Name, txt_SlNo, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Employee_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_Employee_Name.Text) <> "" Then
                txt_Noof_Shift.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then

                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    dgv_Details.CurrentCell.Selected = True

                Else

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If
            End If
        End If

    End Sub

    Private Sub cbo_Employee_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Employee_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Employee_Name, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(cbo_Employee_Name.Text) <> "" Then
                SendKeys.Send("{TAB}")

                With dgv_Details

                    For i = 0 To .Rows.Count - 1
                        If Trim(dgv_Details.Rows(i).Cells(1).Value) = Trim(cbo_Employee_Name.Text) Then

                            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                            cbo_Employee_Name.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                            lbl_Category.Text = (dgv_Details.CurrentRow.Cells(2).Value)


                            txt_Noof_Shift.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
                            txt_Ot_Hours.Text = (dgv_Details.CurrentRow.Cells(4).Value)
                            txt_MessAttendance.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                            txt_Incentive_Amount.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.00")

                            Exit For


                        End If

                    Next

                End With
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    dgv_Details.CurrentCell.Selected = True

                Else

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If
            End If
        End If


    End Sub

    Private Sub cbo_Employee_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Employee_Name.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then


            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Employee_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()


        End If
    End Sub

    Private Sub cbo_Employee_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Employee_Name.LostFocus
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim SNo As Integer = 0
        Dim Emp_Id As Integer = 0

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@AttDate", dtp_Date.Value.Date)
        Emp_Id = Common_Procedures.Employee_NameToIdNo(con, cbo_Employee_Name.Text)
        If Emp_Id <> 0 Then
            If Trim(UCase(cbo_Employee_Name.Tag)) <> Trim(UCase(cbo_Employee_Name.Text)) Then

                For i = 0 To dgv_Details.Rows.Count - 1
                    If Trim(UCase(cbo_Employee_Name.Tag)) = Trim(UCase(dgv_Details.Rows(i).Cells(1).Value)) Then
                        lbl_Category.Text = dgv_Details.Rows(i).Cells(2).Value
                        txt_Noof_Shift.Text = dgv_Details.Rows(i).Cells(3).Value
                        txt_Ot_Hours.Text = dgv_Details.Rows(i).Cells(4).Value
                        txt_MessAttendance.Text = dgv_Details.Rows(i).Cells(5).Value
                        txt_Incentive_Amount.Text = dgv_Details.Rows(i).Cells(6).Value
                        Exit For
                    End If
                Next

                'Cmd.CommandText = "select a.*, b.Category_Name from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo = b.Category_IdNo where a.Join_DateTime <= @AttDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @AttDate ) ) and EmployeE_Idno = " & Val(Emp_Id) & ""
                'da1 = New SqlClient.SqlDataAdapter(Cmd)
                'dt1 = New DataTable
                'da1.Fill(dt1)

                'If dt1.Rows.Count > 0 Then

                '    SNo = SNo + 1

                '    lbl_Category.Text = dt1.Rows(0).Item("Category_Name").ToString

                '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then
                '        txt_Noof_Shift.Text = ""
                '        txt_MessAttendance.Text = ""
                '    Else
                '        txt_Noof_Shift.Text = 1
                '        txt_MessAttendance.Text = 1
                '    End If

                'End If

            End If

        End If
        Grid_Cell_DeSelect()


    End Sub

    Private Sub txt_Incentive_Amount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Incentive_Amount.KeyDown
        If e.KeyCode = 38 Then
            txt_MessAttendance.Focus()
        End If
        If e.KeyCode = 40 Then

            btn_Add_Click(sender, e)

        End If
    End Sub

    Private Sub txt_Incentive_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Incentive_Amount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details
                    If .Rows.Count >= 0 Then

                        n = .CurrentRow.Index

                        If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Noof_Shift_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Noof_Shift.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub txt_Noof_Shift_TextChanged(sender As Object, e As EventArgs) Handles txt_Noof_Shift.TextChanged

    End Sub

    Private Sub txt_SlNo_TextChanged(sender As Object, e As EventArgs) Handles txt_SlNo.TextChanged

    End Sub

    Private Sub cbo_Employee_Name_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Employee_Name.SelectedIndexChanged

    End Sub
End Class