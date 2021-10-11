Public Class PayRoll_Employee_Attendance_From_Machine
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "ATTMC-"
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
    Public previlege As String

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

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        lbl_Day.Text = Trim(Format(dtp_Date.Value, "dddddd"))

        dgv_Details.Rows.Clear()

        Grid_Cell_DeSelect()

        cbo_Grid_Shift.Visible = False
        cbo_Grid_Shift.Tag = -1
        cbo_Grid_Employee.Visible = False
        cbo_Grid_Employee.Tag = -1

        dtp_Date.Tag = ""

        NoCalc_Status = False

        dtp_Date.Enabled = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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

        If Me.ActiveControl.Name <> cbo_Grid_Shift.Name Then
            cbo_Grid_Shift.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Employee.Name Then
            cbo_Grid_Employee.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
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
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim a() As String

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Attendance_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.ForeColor = Color.Black
                dtp_Date.Enabled = False
                lbl_RefNo.Text = dt1.Rows(0).Item("Employee_Attendance_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Employee_Attendance_Date").ToString

                lbl_Day.Text = dt1.Rows(0).Item("Day_Name").ToString

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name,c.Shift_Name,d.Category_Name from PayRoll_Employee_Attendance_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo LEFT OUTER JOIN Shift_Head c ON a.Shift_IdNo = c.Shift_IdNo LEFT OUTER JOIN PayRoll_Category_Head D ON a.Category_IdNo = d.Category_IdNo Where a.Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("Shift_Name").ToString
                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("In_Out_Timings").ToString
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("No_Of_Hours").ToString), "########0.00")
                            If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                            .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Add_Less_Minutes").ToString)
                            If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Shift_Hours").ToString), "########0.00")
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""
                            .Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("No_Of_Shift").ToString)
                            If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""
                            .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("OT_Hours").ToString), "########0.00")
                            If Val(.Rows(n).Cells(9).Value) = 0 Then .Rows(n).Cells(9).Value = ""
                            .Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Late_Hours").ToString), "########0.00")
                            If Val(.Rows(n).Cells(10).Value) = 0 Then .Rows(n).Cells(10).Value = ""
                            .Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("EarlyOut_Hours").ToString), "########0.00")
                            If Val(.Rows(n).Cells(11).Value) = 0 Then .Rows(n).Cells(11).Value = ""

                            a = Split(.Rows(n).Cells(4).Value, ",")
                            If (UBound(a)) Mod 2 = 0 Then
                                dgv_Details.Rows(n).Cells(4).Style.ForeColor = Color.Red
                            End If


                        Next i

                    End If
                End With

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name from PayRoll_Attendance_Timing_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo  Where a.Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_TimeDetails

                    .Rows.Clear()

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(0).Value = dt2.Rows(i).Item("Employee_Name").ToString
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("InOut_DateTime")

                        Next i

                    End If

                End With

            Else
                new_record()

            End If

            Grid_Cell_DeSelect()

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try


        dtp_Date.Tag = ""

        NoCalc_Status = False

    End Sub

    Private Sub Employee_Attendance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Employee.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Employee.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Employee_Attendance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()


        Pnl_AbsentList.Visible = False
        Pnl_AbsentList.Left = 28
        Pnl_AbsentList.Top = 80
        Pnl_AbsentList.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Employee.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Employee.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Shift.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

        dgv_Details.Columns(7).ReadOnly = True
        dgv_Details.Columns(8).ReadOnly = True
        dgv_Details.Columns(9).ReadOnly = True
        dgv_Details.Columns(10).ReadOnly = True
        dgv_Details.Columns(11).ReadOnly = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1201" Then '----Deva
            dgv_Details.Columns(7).ReadOnly = False
            dgv_Details.Columns(8).ReadOnly = False
            dgv_Details.Columns(9).ReadOnly = False
            dgv_Details.Columns(10).ReadOnly = False
            dgv_Details.Columns(11).ReadOnly = False

        End If

    End Sub

    Private Sub Employee_Attendance_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Employee_Attendance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                'If pnl_Filter.Visible = True Then
                '    btn_Filter_Close_Click(sender, e)
                '    Exit Sub
                'Else
                Close_Form()

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= 6 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(6)    '.Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(6)    '.Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 6 Then
                        If .CurrentCell.RowIndex = 0 Then
                            dtp_Date.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)

                        End If

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(6)  ' .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Attendance_Machine, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Attendance_Machine, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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

        'If New_Entry = True Then
        '    MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from PayRoll_Attendance_Timing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Employee_Attendance_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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


    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Employee_Attendance_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Employee_Attendance_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Employee_Attendance_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Employee_Attendance_No desc", con)
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

        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Attendance_Head", "Employee_Attendance_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red
            get_EmployeeList()

            Dt1.Clear()

            dtp_Date.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            Da = New SqlClient.SqlDataAdapter("select Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "'", con)
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

            Da = New SqlClient.SqlDataAdapter("select Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(InvCode) & "'", con)
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
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Cate_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Empe_ID As Integer = 0
        Dim Sft_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim OT_Mins As Long = 0
        Dim EntID As String = ""
        Dim Ot_Dbl As Double = 0
        Dim Ot_Int As Long = 0
        Dim Sht_Mins As Long = 0
        Dim Sht_Dbl As Double = 0
        Dim Sht_Int As Long = 0
        Dim No_Mins As Long = 0
        Dim No_Dbl As Double = 0
        Dim No_Int As Long = 0
        Dim Lt_Int As Long = 0
        Dim Lt_Dbl As Double = 0
        Dim Lt_Mins As Long = 0
        Dim eOut_Int As Long = 0
        Dim eOut_Dbl As Double = 0
        Dim eOut_Mins As Long = 0
        'Dim DtTm1 As Date
        Dim IODtTm As Date
        Dim OrdBy As String = ""
        Dim Err_EmpNm As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Attendance_Machine, New_Entry) = False Then Exit Sub

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


        With dgv_Details

            For i = 0 To .RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Then
                    Empe_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)

                    If Empe_ID = 0 Then
                        MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) < 0 Then
                        MessageBox.Show("Invalid No.of Hours", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(7).Value) < 0 Then
                        MessageBox.Show("Invalid Shift Hours", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(7)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(8).Value) < 0 Then
                        MessageBox.Show("Invalid No.of Shift", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(8)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(9).Value) < 0 Then
                        MessageBox.Show("Invalid OT Hours", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(9)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(10).Value) < 0 Then
                        MessageBox.Show("Invalid Late Hours", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(10)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(11).Value) < 0 Then
                        MessageBox.Show("Invalid Early Our Hours", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(11)
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

            OrdBy = Format(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)), "#########0.00").ToString

            If New_Entry = True Then

                cmd.CommandText = "Insert into PayRoll_Employee_Attendance_Head (       Employee_Attendance_Code ,               Company_IdNo       ,           Employee_Attendance_No    ,    for_OrderBy    , Employee_Attendance_Date   ,  Day_Name                  ) " &
                                    "     Values                  (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdBy)) & ",      @EntryDate    , '" & Trim(lbl_Day.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update PayRoll_Employee_Attendance_Head set Employee_Attendance_Date = @EntryDate,  Day_Name = '" & Trim(lbl_Day.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from PayRoll_Employee_Attendance_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Err_EmpNm = ""

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Sno = Sno + 1

                        Err_EmpNm = .Rows(i).Cells(1).Value

                        Empe_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Cate_Id = Common_Procedures.Category_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Sft_ID = Common_Procedures.Shift_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        No_Int = Int(Val(.Rows(i).Cells(5).Value))
                        No_Dbl = Val(.Rows(i).Cells(5).Value) - Val(No_Int)
                        No_Mins = (Val(No_Int) * 60) + (Val(No_Dbl) * 100)

                        Sht_Int = Int(Val(.Rows(i).Cells(7).Value))
                        Sht_Dbl = Val(.Rows(i).Cells(7).Value) - Val(Sht_Int)
                        Sht_Mins = (Val(Sht_Int) * 60) + (Val(Sht_Dbl) * 100)

                        Ot_Int = Int(Val(.Rows(i).Cells(9).Value))
                        Ot_Dbl = Val(.Rows(i).Cells(9).Value) - Val(Ot_Int)
                        OT_Mins = (Val(Ot_Int) * 60) + (Val(Ot_Dbl) * 100)

                        Lt_Int = Int(Val(.Rows(i).Cells(10).Value))
                        Lt_Dbl = Val(.Rows(i).Cells(10).Value) - Val(Lt_Int)
                        Lt_Mins = (Val(Lt_Int) * 60) + (Val(Lt_Dbl) * 100)

                        eOut_Int = Int(Val(.Rows(i).Cells(11).Value))
                        eOut_Dbl = Val(.Rows(i).Cells(11).Value) - Val(eOut_Int)
                        eOut_Mins = (Val(eOut_Int) * 60) + (Val(eOut_Dbl) * 100)

                        cmd.CommandText = "Insert into PayRoll_Employee_Attendance_Details (              Employee_Attendance_Code         ,               Company_IdNo       ,   Employee_Attendance_No      ,         for_OrderBy    ,   Employee_Attendance_Date  ,             Sl_No     ,    Employee_IdNo         ,       Category_IdNo  ,        Shift_IdNo      ,                 In_Out_Timings          ,                      No_Of_Hours         ,        No_Of_Minutes      ,              Add_Less_Minutes             ,               Shift_Hours                ,     Shift_Minutes         ,                      No_Of_Shift         ,                      OT_Hours             ,           OT_Minutes     ,                      Late_Hours            ,         Late_Minutes     ,                      EarlyOut_Hours        ,        EarlyOut_Minutes     ) " &
                                        "              Values                              (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdBy)) & ",       @EntryDate            ,  " & Str(Val(Sno)) & ", " & Str(Val(Empe_ID)) & ",  " & Val(Cate_Id) & "," & Str(Val(Sft_ID)) & ",  '" & Trim(.Rows(i).Cells(4).Value) & "', " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(No_Mins)) & " ,  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(Sht_Mins)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & " , " & Str(Val(OT_Mins)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & " , " & Str(Val(Lt_Mins)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & " , " & Str(Val(eOut_Mins)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            Err_EmpNm = ""

            cmd.CommandText = "Delete from PayRoll_Attendance_Timing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_TimeDetails

                Sno = 0

                For i = 0 To .RowCount - 1

                    Err_EmpNm = .Rows(i).Cells(0).Value

                    Empe_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(0).Value, tr)

                    If Val(Empe_ID) <> 0 Then

                        If Trim(.Rows(i).Cells(1).Value) <> "" Then

                            If IsDate(.Rows(i).Cells(1).Value) = True Then

                                IODtTm = Convert.ToDateTime(.Rows(i).Cells(1).Value)
                                'DtTm1 = Convert.ToDateTime(.Rows(i).Cells(1).Value)
                                'IODtTm = New Date(Year(dtp_Date.Value.Date), Month(dtp_Date.Value.Date), Microsoft.VisualBasic.Day(DtTm1), Hour(DtTm1), Minute(DtTm1), 0)

                                cmd.Parameters.Clear()
                                cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)
                                cmd.Parameters.AddWithValue("@InOutDateTime", IODtTm)

                                Sno = Sno + 1

                                cmd.CommandText = "Insert into PayRoll_Attendance_Timing_Details (         Employee_Attendance_Code            ,               Company_IdNo       ,   Employee_Attendance_No      ,     for_OrderBy        ,   Employee_Attendance_Date,             Sl_No     ,      Employee_IdNo       ,  InOut_Type ,                 InOut_Time_Text        ,  InOut_DateTime  ) " &
                                                  "            Values                            ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdBy)) & ",       @EntryDate          ,  " & Str(Val(Sno)) & ", " & Str(Val(Empe_ID)) & ",      ''     , '" & Trim(.Rows(i).Cells(1).Value) & "', @InOutDateTime   ) "
                                cmd.ExecuteNonQuery()

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
            If InStr(1, Trim(LCase(ex.Message)), "ix_payroll_employee_attendance_head") > 0 Then
                MessageBox.Show("Duplicate Attendance Date", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_payroll_attendance_timing_details_1") > 0 Then
                MessageBox.Show("Duplicate Employee Timings in this date for " & Err_EmpNm, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_payroll_attendance_timing_details_2") > 0 Then
                MessageBox.Show("Duplicate Employee Timings in this entry for " & Err_EmpNm, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

    Private Sub dgv_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellDoubleClick
        If e.ColumnIndex = 4 Then
            MessageBox.Show(dgv_Details.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, "PUNCHING LOGS OF " & dgv_Details.Rows(e.RowIndex).Cells(1).Value, MessageBoxButtons.OKCancel)
        End If
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            If NoCalc_Status = True Or FrmLdSTS = True Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If e.ColumnIndex = 6 Then
                        Calculation_Working_Hours_Shift_OT(e.RowIndex, e.ColumnIndex)
                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        'Dim n As Integer = 0
        'With dgv_Details
        '    n = .RowCount
        '    .Rows(n - 1).Cells(0).Value = Val(n)
        'End With
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

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.GotFocus
        dtp_Date.Tag = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown

        If e.KeyValue = 38 Or e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                'dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            If Trim(UCase(dtp_Date.Tag)) <> Trim(UCase(dtp_Date.Text)) Then
                dtp_Date.Tag = dtp_Date.Text
                get_DateDetails()
                dtp_Date.Tag = dtp_Date.Text
            End If

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                'dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        lbl_Day.Text = Trim(Format(dtp_Date.Value, "dddddd"))
    End Sub

    Private Sub dtp_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.LostFocus
        lbl_Day.Text = Trim(Format(dtp_Date.Value, "dddddd"))
        If Trim(UCase(dtp_Date.Tag)) <> Trim(UCase(dtp_Date.Text)) Then
            dtp_Date.Tag = dtp_Date.Text
            get_DateDetails()
            dtp_Date.Tag = dtp_Date.Text
        End If
    End Sub

    Public Sub get_DateDetails()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""

        Try

            Cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            Cmd.CommandText = "select Employee_Attendance_No from PayRoll_Employee_Attendance_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Attendance_Date = @EntryDate"
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
                get_AttendanceTimings_from_Machine()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DATE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Private Sub cbo_Grid_Employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Employee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Employee.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Employee, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If .CurrentCell.RowIndex = 0 Then
                    dtp_Date.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)

                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_Employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Employee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Employee.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim WrkTy_Nm As String
        Dim Empe_idno As Integer = 0
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Employee, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                .Rows(.CurrentRow.Index).Cells(2).Value = ""


                Empe_idno = Common_Procedures.Employee_NameToIdNo(con, Trim(.Rows(.CurrentRow.Index).Cells(1).Value))

                da = New SqlClient.SqlDataAdapter("select a.*,b.Working_Type_Name from PayRoll_Employee_Head a INNER JOIN Working_Type_Head b ON a.Working_Type_IdNo = b.Working_Type_IdNo where Employee_Idno = " & Str(Val(Empe_idno)), con)
                dt = New DataTable
                da.Fill(dt)

                WrkTy_Nm = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        WrkTy_Nm = Trim(dt.Rows(0).Item("Working_Type_Name").ToString)
                    End If
                End If
                dt.Clear()

                .Rows(.CurrentRow.Index).Cells(2).Value = Trim(WrkTy_Nm)

                dt.Dispose()
                da.Dispose()

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_Employee.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)
                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Employee_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Employee.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Employee.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Grid_Employee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Employee.TextChanged
        Try
            If cbo_Grid_Employee.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Employee.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Employee.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        'Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            'If e.ColumnIndex = 1 Then

            '    If (cbo_Employee.Visible = False Or Val(cbo_Employee.Tag) <> e.RowIndex) Then

            '        cbo_Employee.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)
            '        cbo_Employee.DataSource = Dt1
            '        cbo_Employee.DisplayMember = "Employee_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Employee.Left = .Left + rect.Left
            '        cbo_Employee.Top = .Top + rect.Top

            '        cbo_Employee.Width = rect.Width
            '        cbo_Employee.Height = rect.Height
            '        cbo_Employee.Text = .CurrentCell.Value

            '        cbo_Employee.Tag = Val(e.RowIndex)
            '        cbo_Employee.Visible = True

            '        cbo_Employee.BringToFront()
            '        cbo_Employee.Focus()

            '    End If

            'Else
            '    cbo_Employee.Visible = False

            'End If

            'If e.ColumnIndex = 3 Then

            '    If cbo_Shift.Visible = False Or Val(cbo_Shift.Tag) <> e.RowIndex Then

            '        cbo_Shift.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)
            '        cbo_Shift.DataSource = Dt1
            '        cbo_Shift.DisplayMember = "Shift_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Shift.Left = .Left + rect.Left
            '        cbo_Shift.Top = .Top + rect.Top

            '        cbo_Shift.Width = rect.Width
            '        cbo_Shift.Height = rect.Height
            '        cbo_Shift.Text = .CurrentCell.Value

            '        cbo_Shift.Tag = Val(e.RowIndex)
            '        cbo_Shift.Visible = True

            '        cbo_Shift.BringToFront()
            '        cbo_Shift.Focus()

            '    End If

            'Else
            '    cbo_Shift.Visible = False

            'End If


        End With

    End Sub

    Private Sub cbo_Grid_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Shift, Nothing, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Shift, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Shift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Shift.TextChanged
        Try
            If cbo_Grid_Shift.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Shift.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Shift.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub get_EmployeeList()
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim SNo As Integer

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@AttDate", dtp_Date.Value.Date)


        Cmd.CommandText = "select a.*, b.Category_Name from PayRoll_Employee_Head a LEFT OUTER JOIN PayRoll_Category_Head b ON a.Category_IdNo  = b.Category_IdNo where a.Join_DateTime <= @AttDate and (a.Date_Status = 0 or (a.Date_Status = 1 and a.Releave_DateTime >= @AttDate ) )"
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


                Next i

            End If

            Grid_Cell_DeSelect()

        End With
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = dgtxt_Details.Text
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_GetFromMachine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_GetFromMachine.Click
        get_AttendanceTimings_from_Machine()
        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
    End Sub

    Private Sub get_AttendanceTimings_from_Machine()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim Sh As Double = 0
        Dim I As Integer = 0
        Dim InOut_Count As Integer = 0
        Dim TotTm As Double = 0, Lt As Double = 0, Ot As Double = 0
        Dim H As Double = 0, m As Double = 0
        Dim Ad_Mnts As Double = 0, Ls_Mnts As Double = 0
        Dim WrkTm As Double = 0

        Dim Condt1 As String = ""
        Dim InOutTimeArr(100) As String
        Dim Ind As Integer
        Dim EmpCode As String

        Dim DtTm1 As Date, DtTm2 As Date

        Dim Frm_Date1 As Date
        Dim To_Date2 As Date

        If IsDate(dtp_Date.Value.Date) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If


        NoCalc_Status = True

        Cmd.Connection = con

        Cmd.CommandText = "truncate table EntryTemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Truncate table EntryTempSub"
        Cmd.ExecuteNonQuery()

        DtTm1 = dtp_Date.Value.Date
        DtTm2 = DateAdd(DateInterval.Day, 1, DtTm1)

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@EntryDate", DtTm1)
        Cmd.Parameters.AddWithValue("@NextDate", DtTm2)

        '********** Inserting from the Manual Timing Addition for CurrentDay
        Cmd.CommandText = "Insert into EntryTemp(Name1, Date1, Name2) select b.Card_No, a.InOut_DateTime, a.InOut_Type from PayRoll_Employee_Head b LEFT OUTER JOIN Payroll_Timing_Addition_Details a ON a.Timing_Addition_Code LIKE 'TIMAD-%' and a.Timing_Addition_Date = @EntryDate and b.Employee_IdNo = a.Employee_IdNo Where b.Join_DateTime <= @EntryDate and (b.Date_Status = 0 or (b.Date_Status = 1 and b.Releave_DateTime >= @EntryDate ) )"
        Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Insert into EntryTemp(Name1, Date1, Name2) select b.Card_No, a.InOut_DateTime, a.InOut_Type from Payroll_Timing_Addition_Details a, PayRoll_Employee_Head b where a.Timing_Addition_Code LIKE 'TIMAD-%' and a.Timing_Addition_Date = @EntryDate and a.Employee_IdNo = b.Employee_IdNo"
        'Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into EntryTempSub(Name1, Date1, Name2) select b.Card_No, InOut_DateTime, a.InOut_Type from Payroll_Timing_Addition_Details a, PayRoll_Employee_Head b where a.Timing_Addition_Code LIKE 'TIMAD-%' and a.Timing_Addition_Date = @NextDate and a.Employee_IdNo = b.Employee_IdNo"
        Cmd.ExecuteNonQuery()

        DtTm1 = dtp_Date.Value.Date

        Frm_Date1 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 0, 0, 1)
        DtTm1 = DateAdd(DateInterval.Day, 1, DtTm1)
        To_Date2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 11, 0, 0)

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@FromDate", Frm_Date1)
        Cmd.Parameters.AddWithValue("@ToDate", To_Date2)

        Condt1 = "(INOut_DateTime BetWeen @FromDate And @ToDate)"

        Cmd.CommandText = "select * from Payroll_AttendanceLog_FromMachine_Details where " & Condt1 & " Order by Employee_CardNo, INOut_DateTime"
        Da = New SqlClient.SqlDataAdapter(Cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                DtTm1 = Dt1.Rows(I).Item("INOut_DateTime")
                DtTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1))
                'DtTm2 = DateAdd(DateInterval.Day, 1, DtTm2)

                Cmd.Parameters.Clear()
                Cmd.Parameters.AddWithValue("@IO_DateTime", DtTm1)

                If DateDiff(DateInterval.Day, dtp_Date.Value.Date, DtTm2) = 0 Then
                    Cmd.CommandText = "Insert into EntryTemp(Name1, Date1, Name2) values ('" & Trim(Dt1.Rows(I).Item("Employee_CardNo").ToString) & "', @IO_DateTime, '" & Trim(Dt1.Rows(I).Item("IN_Out").ToString) & "')"
                    Cmd.ExecuteNonQuery()

                ElseIf DateDiff(DateInterval.Day, dtp_Date.Value.Date, DtTm2) = 1 Then
                    Cmd.CommandText = "Insert into EntryTempSub(Name1, Date1, Name2) values ('" & Trim(Dt1.Rows(I).Item("Employee_CardNo").ToString) & "', @IO_DateTime, '" & Trim(Dt1.Rows(I).Item("IN_Out").ToString) & "')"
                    Cmd.ExecuteNonQuery()

                End If

            Next I

        End If
        Dt1.Clear()

        Cmd.Parameters.Clear()

        DtTm1 = dtp_Date.Value.Date
        DtTm2 = DateAdd(DateInterval.Day, -1, DtTm1)

        Cmd.Parameters.AddWithValue("@PreviousDate", DtTm2)


        '********** getting previous_day attendance
        Cmd.CommandText = "Truncate Table EntryTemp_Simple"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into EntryTemp_Simple(Name1, Date1, Date2) Select b.Card_No, a.Employee_Attendance_Date, a.InOut_DateTime from PayRoll_Attendance_Timing_Details a, PayRoll_Employee_Head b where a.Employee_Attendance_Date = @PreviousDate and a.Employee_IdNo = b.Employee_IdNo"
        Cmd.ExecuteNonQuery()


        '********** Deleting the Timing , if it is already included in Previous Day
        Cmd.CommandText = "delete from EntryTemp where Date1 IN (Select sq1.date2 from EntryTemp_Simple sq1 where sq1.Name1 = EntryTemp.Name1)"
        Cmd.ExecuteNonQuery()

        '********** Adding Entries Upto NextDay 5:00 A.M -  (Only as InTime if has no timings in CurrDate)
        Cmd.Parameters.Clear()

        DtTm1 = dtp_Date.Value.Date
        DtTm1 = DateAdd(DateInterval.Day, 1, DtTm1)
        DtTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 5, 0, 0)

        Cmd.Parameters.AddWithValue("@NextDate", DtTm2)

        Cmd.CommandText = "Truncate Table EntryTemp_Simple"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into EntryTemp_Simple ( Name1, Date1, Name2 ) select a.Name1, a.Date1, a.Name2 from EntryTempSub a where a.Date1 <= @NextDate and (Select COUNT(*) from EntryTemp sq1 where sq1.int1 = a.Int1) = 0"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into EntryTemp(Name1, Date1, Name2) select Name1, Date1, Name2 from EntryTemp_Simple"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "delete from EntryTempSub Where Date1 IN (Select sq1.date1 from EntryTemp_Simple sq1 where sq1.Name1 = EntryTempSub.Name1)"
        Cmd.ExecuteNonQuery()


        '********** Displaying Timing from EntryTemp....

        EmpCode = ""
        Ind = 0
        InOut_Count = 0
        Erase InOutTimeArr

        InOutTimeArr = New String(100) {}

        dgv_Details.Rows.Clear()
        dgv_TimeDetails.Rows.Clear()

        Da = New SqlClient.SqlDataAdapter("Select b.Employee_Name, a.Name1 as EmpTicketNo, a.Date1 as InOutTiming, a.Name2 as In_Out from EntryTemp a LEFT OUTER JOIN PayRoll_Employee_Head b ON a.Name1 = b.card_no WHERE B.Date_Status = 0 order by b.Employee_Name, a.Name1, a.Date1", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                'If Trim(UCase(Dt1.Rows(I).Item("EmpTicketNo").ToString)) = "21" Then
                '    Debug.Print("21")
                'End If

                If Trim(EmpCode) <> "" Then

                    If Trim(UCase(EmpCode)) <> Trim(UCase(Dt1.Rows(I).Item("EmpTicketNo").ToString)) Then

                        If (InOut_Count Mod 2) = 1 Then Get_OutTime_From_NextDate(EmpCode, Ind, InOutTimeArr)
                        Employee_InOut_Update(EmpCode, Ind, InOutTimeArr)
                        InOut_Count = 0 : Ind = 0
                        Erase InOutTimeArr
                        InOutTimeArr = New String(100) {}

                    End If

                End If


                If Dt1.Rows(I).Item("InOutTiming").ToString <> "" Then

                    If IsDate(Dt1.Rows(I).Item("InOutTiming")) Then

LOOP1:
                        Ind = Ind + 1
                        If Trim(InOutTimeArr(Ind)) = "" Then

                            If Ind = 1 Then
                                InOutTimeArr(Ind) = Trim(Dt1.Rows(I).Item("InOutTiming").ToString)
                                InOut_Count = InOut_Count + 1

                            Else
                                'DtTm3 = Trim(InOutTimeArr(Ind - 1))
                                'DtTm4 = Trim(Dt1.Rows(I).Item("InOutTiming").ToString)
                                'If DateDiff("n", DT3, DT4) > 1 Then
                                InOutTimeArr(Ind) = Trim(Dt1.Rows(I).Item("InOutTiming").ToString)
                                InOut_Count = InOut_Count + 1
                                'Else
                                '    Ind = Ind - 1
                                'End If

                            End If

                        Else
                            GoTo LOOP1

                        End If

                    End If

                End If

                EmpCode = Dt1.Rows(I).Item("EmpTicketNo").ToString

            Next I

            If Trim(EmpCode) <> "" Then
                If (InOut_Count Mod 2) = 1 Then Get_OutTime_From_NextDate(EmpCode, Ind, InOutTimeArr)
                Employee_InOut_Update(EmpCode, Ind, InOutTimeArr)
                InOut_Count = 0 : Ind = 0
                Erase InOutTimeArr
                InOutTimeArr = New String(100) {}
            End If

        End If
        Dt1.Clear()

        For I = 0 To dgv_Details.Rows.Count - 1
            dgv_Details.Rows(I).Cells(0).Value = I + 1
        Next I

        Grid_Cell_DeSelect()

        NoCalc_Status = False

    End Sub

    Private Sub Get_OutTime_From_NextDate(ByVal EmpCode As String, ByRef Ind As Integer, ByVal InOutTimeArr() As String)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim OutTime_From_NextDate As String

        OutTime_From_NextDate = ""

        Da = New SqlClient.SqlDataAdapter("select * from EntryTempSub where Name1 = '" & Trim(EmpCode) & "' Order by Date1", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If Dt1.Rows(0).Item("Date1").ToString <> "" Then

                If IsDate(Dt1.Rows(0).Item("Date1")) Then

                    OutTime_From_NextDate = Trim(Dt1.Rows(0).Item("Date1").ToString)

                End If

            End If

        End If

        If Trim(OutTime_From_NextDate) <> "" Then

            Ind = Ind + 1
            If Trim(InOutTimeArr(Ind)) = "" Then

                If Ind = 1 Then
                    InOutTimeArr(Ind) = Trim(OutTime_From_NextDate)

                Else
                    'DtTm3 = Trim(InOutTimeArr(Ind - 1))
                    'DtTm4 = Trim(OutTime_From_NextDate)
                    'If DateDiff("n", DtTm3, DtTm4) > 1 Then
                    InOutTimeArr(Ind) = Trim(OutTime_From_NextDate)
                    'End If
                End If

            End If

        End If

    End Sub

    Private Sub Employee_InOut_Update(ByVal EmpCode As String, ByRef Ind As Integer, ByVal InOutTimeArr() As String)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EmpNM As String = "", Catg_Nm As String = ""
        Dim catg_id As Integer = 0
        Dim ShftInTm As Date, ShftInTm1 As Date, ShftInTm2 As Date, ShftInTm3 As Date
        Dim ShftOutTm As Date, ShftOutTm1 As Date, ShftOutTm2 As Date, ShftOutTm3 As Date
        Dim DtTm1 As Date, DtTm2 As Date, DtTm3 As Date
        Dim InDtTm1 As Date, LastOutDtTm As Date, PrevOutDtTm As Date
        Dim Shift_Minutes_1 As Single = 0
        Dim Shift_Minutes_2 As Single = 0
        Dim Shift_Minutes_3 As Single = 0
        Dim SalTyp_ShftMnth As String = ""
        Dim OTAllowSTS As Integer = 0
        Dim HalfShtMns As Double = 0, OTAllowMnts As Double = 0, NoofShfts As Double = 0
        Dim Mins As Double = 0, TotMins As Double = 0, BalMins As Double = 0
        Dim TotInMins As Double = 0, TotOutMins As Double = 0
        Dim TotHrs As String = ""
        Dim DisInTime(5) As String, DisOutTime(5) As String, DispInOutStr As String = ""
        Dim OtHrs As Double = 0, OTMnts As Double = 0
        Dim InHrs As Double = 0, OutHrs As Double = 0
        Dim SftHrs As Double = 0, SftMnts As Double = 0, LateHrs As Double = 0, LateMns As Double = 0
        Dim eOutHrs As Double = 0, eOutMns As Double = 0
        Dim ActShftMnts As Double = 0
        Dim Sno As Integer = 0
        Dim n As Long = 0
        Dim H As Long = 0, M As Long = 0
        Dim ShftNm As String = ""
        Dim ShftID As Integer = 0
        Dim j As Integer = 0
        Dim a() As String
        Dim strLastOutTm As String = ""
        Dim vInHrs_WrkdHrsS_STS As Integer = 0
        Dim FirstInTime As Date, LastOutTime As Date
        Dim Out_mins_1 As Single = 0
        'If Trim(EmpCode) = "11" Then
        '    Debug.Print(Trim(EmpCode))
        'End If

        EmpNM = Common_Procedures.get_FieldValue(con, "PayRoll_Employee_Head", "Employee_Name", "(Card_No = '" & Trim(EmpCode) & "')")
        catg_id = 0
        If Trim(EmpCode) <> "" Then
            catg_id = Val(Common_Procedures.get_FieldValue(con, "PayRoll_Employee_Head", "category_idno", "(Card_No = '" & Trim(EmpCode) & "')"))
        End If

        Catg_Nm = Common_Procedures.Category_IdNoToName(con, catg_id)

        DtTm1 = dtp_Date.Value.Date

        Da = New SqlClient.SqlDataAdapter("select * from PayRoll_Category_Head where Category_IdNo = " & Str(Val(catg_id)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then


            ShftInTm1 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift1_In_Time").ToString, 2), Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift1_In_Time").ToString, 2), 0)
            ShftOutTm1 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift1_Out_Time").ToString, 2), Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift1_Out_Time").ToString, 2), 0)

            If Dt1.Rows(0).Item("Shift2_In_Time").ToString <> "" Then

                If IsDate(Dt1.Rows(0).Item("Shift2_In_Time").ToString) = True And Val(Dt1.Rows(0).Item("Shift2_In_Time").ToString) <> 0 Then

                    ShftInTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift2_In_Time").ToString, 2), Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift2_In_Time").ToString, 2), 0)

                    If Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2) > 23 And Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2) > 59 Then

                        ShftOutTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1) + 1, Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2), Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2), 0)

                    ElseIf Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift2_In_Time").ToString, 2) > Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2) Then

                        ShftOutTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1) + 1, Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2), Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2), 0)

                    Else
                        ShftOutTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2), Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift2_Out_Time").ToString, 2), 0)

                    End If

                Else
                    ShftInTm2 = New DateTime(2100, 1, 1, 16, 1, 0)
                    ShftOutTm2 = New DateTime(2100, 1, 1, 23, 59, 59)

                    'ShftInTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 16, 1, 0)
                    'ShftOutTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 23, 59, 59)

                End If

            Else
                ShftInTm2 = New DateTime(2100, 1, 1, 16, 1, 0)
                ShftOutTm2 = New DateTime(2100, 1, 1, 23, 59, 59)

                'ShftInTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 16, 1, 0)
                'ShftOutTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 23, 59, 59)

            End If
            

            If Dt1.Rows(0).Item("Shift3_In_Time").ToString = "" Then
                If IsDate(Dt1.Rows(0).Item("Shift3_In_Time").ToString) = True And Val(Dt1.Rows(0).Item("Shift3_In_Time").ToString) <> 0 Then
                    DtTm1 = DateAdd(DateInterval.Day, 1, DtTm1)
                    ShftInTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift3_In_Time").ToString, 2), Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift3_In_Time").ToString, 2), 0)
                    ShftOutTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Microsoft.VisualBasic.Left(Dt1.Rows(0).Item("Shift3_Out_Time").ToString, 2), Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Shift3_Out_Time").ToString, 2), 0)
                Else
                    ShftInTm3 = New DateTime(2100, 1, 1, 0, 1, 0)
                    ShftOutTm3 = New DateTime(2100, 1, 1, 8, 0, 0)

                    'DtTm1 = DateAdd(DateInterval.Day, 1, DtTm1)
                    'ShftInTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 0, 1, 0)
                    'ShftOutTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 8, 0, 0)

                End If

            Else
                ShftInTm3 = New DateTime(2100, 1, 1, 0, 1, 0)
                ShftOutTm3 = New DateTime(2100, 1, 1, 8, 0, 0)

                'DtTm1 = DateAdd(DateInterval.Day, 1, DtTm1)
                'ShftInTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 0, 1, 0)
                'ShftOutTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 8, 0, 0)

            End If

        End If
        Da.Dispose()
        Dt1.Clear()

        'ShftInTm1 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 8, 1, 0)
        'ShftOutTm1 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 16, 0, 0)

        'ShftInTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 16, 1, 0)
        'ShftOutTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 23, 59, 59)

        'DtTm1 = DateAdd(DateInterval.Day, 1, DtTm1)
        'ShftInTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 0, 1, 0)
        'ShftOutTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), 8, 0, 0)

        Shift_Minutes_1 = 0
        Shift_Minutes_2 = 0
        Shift_Minutes_3 = 0

        SalTyp_ShftMnth = "" : OTAllowSTS = 0 : OTAllowMnts = 0
        vInHrs_WrkdHrsS_STS = 0
        Da = New SqlClient.SqlDataAdapter("select * from PayRoll_Category_Head where Category_IdNo = " & Str(Val(catg_id)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            Shift_Minutes_1 = Val(Dt1.Rows(0).Item("Shift1_Working_Minutes").ToString)
            Shift_Minutes_2 = Val(Dt1.Rows(0).Item("Shift2_Working_Minutes").ToString)
            Shift_Minutes_3 = Val(Dt1.Rows(0).Item("Shift3_Working_Minutes").ToString)

            If Dt1.Rows(0).Item("Shift1_In_DateTime").ToString <> "" Then
                If IsDate(Dt1.Rows(0).Item("Shift1_In_DateTime")) = True Then
                    DtTm1 = dtp_Date.Value.Date
                    DtTm2 = Dt1.Rows(0).Item("Shift1_In_DateTime")
                    DtTm3 = Dt1.Rows(0).Item("Shift1_Out_DateTime")
                    ShftInTm1 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Hour(DtTm2), Minute(DtTm2), 0)
                    ShftOutTm1 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Hour(DtTm3), Minute(DtTm3), 0)
                End If
            End If

            If Dt1.Rows(0).Item("Shift2_In_DateTime").ToString <> "" Then
                If IsDate(Dt1.Rows(0).Item("Shift2_In_DateTime")) = True Then
                    DtTm1 = dtp_Date.Value.Date
                    DtTm2 = Dt1.Rows(0).Item("Shift2_In_DateTime")
                    DtTm3 = Dt1.Rows(0).Item("Shift2_Out_DateTime")
                    ShftInTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Hour(DtTm2), Minute(DtTm2), 0)
                    ShftOutTm2 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Hour(DtTm3), Minute(DtTm3), 0)
                End If
            End If

            If Dt1.Rows(0).Item("Shift3_In_DateTime").ToString <> "" Then
                If IsDate(Dt1.Rows(0).Item("Shift3_In_DateTime")) = True Then
                    DtTm1 = dtp_Date.Value.Date
                    DtTm1 = DateAdd(DateInterval.Day, 1, DtTm1)
                    DtTm2 = Dt1.Rows(0).Item("Shift3_In_DateTime")
                    DtTm3 = Dt1.Rows(0).Item("Shift3_Out_DateTime")
                    ShftInTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Hour(DtTm2), Minute(DtTm2), 0)
                    ShftOutTm3 = New DateTime(Year(DtTm1), Month(DtTm1), Microsoft.VisualBasic.Day(DtTm1), Hour(DtTm3), Minute(DtTm3), 0)
                End If
            End If

            OTAllowSTS = Val(Dt1.Rows(0).Item("OT_Allowed").ToString)
            OTAllowMnts = Val(Dt1.Rows(0).Item("OT_Allowed_After_Minutes").ToString)
            SalTyp_ShftMnth = Dt1.Rows(0).Item("Monthly_Shift").ToString
            vInHrs_WrkdHrsS_STS = Val(Dt1.Rows(0).Item("Office_TotalInHours_As_WorkedHours_Status").ToString)

        End If
        Dt1.Clear()

        TotMins = 0
        TotInMins = 0
        TotOutMins = 0
        DispInOutStr = ""
        strLastOutTm = ""



        For I = 1 To Ind Step 2

            If Trim(InOutTimeArr(I)) <> "" And Trim(InOutTimeArr(I + 1)) <> "" Then

                If IsDate(InOutTimeArr(I)) And IsDate(InOutTimeArr(I + 1)) Then

                    DtTm1 = Trim(InOutTimeArr(I))
                    DtTm2 = Trim(InOutTimeArr(I + 1))

                    TotInMins = TotInMins + DateDiff(DateInterval.Minute, DtTm1, DtTm2)
                    DispInOutStr = DispInOutStr & IIf(DispInOutStr <> "", ", ", "") & Trim(Format(Convert.ToDateTime(InOutTimeArr(I)), "HH:mm").ToString()) & ", " & Trim(Format(Convert.ToDateTime(InOutTimeArr(I + 1)), "HH:mm").ToString())

                    n = dgv_TimeDetails.Rows.Add()
                    dgv_TimeDetails.Rows(n).Cells(0).Value = EmpNM
                    dgv_TimeDetails.Rows(n).Cells(1).Value = InOutTimeArr(I)

                    n = dgv_TimeDetails.Rows.Add()
                    dgv_TimeDetails.Rows(n).Cells(0).Value = EmpNM
                    dgv_TimeDetails.Rows(n).Cells(1).Value = InOutTimeArr(I + 1)

                    strLastOutTm = Trim(InOutTimeArr(I + 1))

                    If I <> 1 Then
                        TotOutMins = TotOutMins + DateDiff(DateInterval.Minute, PrevOutDtTm, DtTm1)
                    End If

                    PrevOutDtTm = DtTm2

                    If I = 1 Then FirstInTime = DtTm1
                    LastOutTime = DtTm2

                End If

            ElseIf Trim(InOutTimeArr(I)) <> "" Then
                If IsDate(InOutTimeArr(I)) Then

                    DispInOutStr = DispInOutStr & IIf(DispInOutStr <> "", ", ", "") & Trim(Format(Convert.ToDateTime(InOutTimeArr(I)), "HH:mm").ToString())

                    n = dgv_TimeDetails.Rows.Add()
                    dgv_TimeDetails.Rows(n).Cells(0).Value = EmpNM
                    dgv_TimeDetails.Rows(n).Cells(1).Value = InOutTimeArr(I)

                    DtTm1 = Trim(InOutTimeArr(I))

                    If I <> 1 Then
                        TotOutMins = TotOutMins + DateDiff(DateInterval.Minute, PrevOutDtTm, DtTm1)
                    End If

                    LastOutTime = DtTm2

                End If

            End If

        Next

        TotMins = 0
        LateHrs = 0 : LateMns = 0
        eOutHrs = 0 : eOutMns = 0

        InDtTm1 = ShftInTm1
        LastOutDtTm = ShftOutTm1
        If Trim(InOutTimeArr(1)) <> "" Then
            If IsDate(InOutTimeArr(1)) Then InDtTm1 = Trim(InOutTimeArr(1))
        End If
        If Trim(strLastOutTm) <> "" Then
            If IsDate(strLastOutTm) Then LastOutDtTm = Trim(strLastOutTm)
        End If

        If vInHrs_WrkdHrsS_STS = 1 Then
            TotMins = DateDiff(DateInterval.Minute, FirstInTime, LastOutTime)
        Else
            TotMins = TotInMins
        End If

        H = TotMins \ 60
        M = TotMins - (H * 60)
        TotHrs = H & "." & Format(M, "00")

        ShftNm = 0
        ShftID = 0
        If DateDiff("n", ShftInTm3, InDtTm1) >= 0 Then
            ShftNm = Common_Procedures.get_FieldValue(con, "Shift_Head", "Shift_Name", "(Shift_IdNo = 3)")
            If Trim(ShftNm) = "" Then ShftNm = "3rd Shift"
            ShftInTm = ShftInTm3
            ShftOutTm = ShftOutTm3
            ShftID = 3
        ElseIf DateDiff("n", ShftInTm2, InDtTm1) >= 0 And DateDiff(DateInterval.Minute, ShftInTm3, InDtTm1) < 0 Then
            ShftNm = Common_Procedures.get_FieldValue(con, "Shift_Head", "Shift_Name", "(Shift_IdNo = 2)")
            If Trim(ShftNm) = "" Then ShftNm = "2nd Shift"
            ShftInTm = ShftInTm2
            ShftOutTm = ShftOutTm2
            ShftID = 2
        Else
            ShftNm = Common_Procedures.get_FieldValue(con, "Shift_Head", "Shift_Name", "(Shift_IdNo = 1)")
            If Trim(ShftNm) = "" Then ShftNm = "1st Shift"
            ShftInTm = ShftInTm1
            ShftOutTm = ShftOutTm1
            ShftID = 1
        End If

        If catg_id > 0 Then
            DtTm1 = New DateTime(Year(ShftInTm), Month(LastOutDtTm), Microsoft.VisualBasic.Day(LastOutDtTm), Hour(LastOutDtTm), Minute(LastOutDtTm), 0)
            LateMns = DateDiff(DateInterval.Minute, ShftInTm, InDtTm1)
            If LateMns > 0 Then
                H = LateMns \ 60
                M = LateMns - (H * 60)
                LateHrs = H & "." & Format(M, "00")
            Else
                LateMns = 0
            End If

            DtTm1 = New DateTime(Year(LastOutDtTm), Month(LastOutDtTm), Microsoft.VisualBasic.Day(LastOutDtTm), Hour(LastOutDtTm), Minute(LastOutDtTm), 0)
            eOutMns = Math.Abs(DateDiff(DateInterval.Minute, ShftOutTm, DtTm1))

            If ShftOutTm > DtTm1 Then
                If eOutMns > 0 Then
                    H = eOutMns \ 60
                    M = eOutMns - (H * 60)
                    eOutHrs = H & "." & Format(M, "00")
                Else
                    eOutMns = 0
                End If
            Else
                eOutMns = 0
            End If
          
        End If

        Sno = Sno + 1

        OtHrs = 0 : OTMnts = 0 : SftHrs = 0 : SftMnts = 0 : NoofShfts = 0
        If TotMins > 0 Then

            If ShftID = 3 Then
                ActShftMnts = Val(Shift_Minutes_3)
            ElseIf ShftID = 2 Then
                ActShftMnts = Val(Shift_Minutes_2)
            Else
                ActShftMnts = Val(Shift_Minutes_1)
            End If
            If Val(ActShftMnts) = 0 Then ActShftMnts = Val(Shift_Minutes_1)
            HalfShtMns = Int(ActShftMnts / 2)

            If Trim(UCase(SalTyp_ShftMnth)) = "SHIFT" Then
                NoofShfts = Int(TotMins / ActShftMnts)
                SftMnts = (NoofShfts * ActShftMnts)
                BalMins = TotMins - (NoofShfts * ActShftMnts)
                If Val(BalMins) >= HalfShtMns Then NoofShfts = NoofShfts + 0.5

            Else
                If Val(TotMins) >= HalfShtMns Then NoofShfts = 1 Else NoofShfts = 0.5

            End If



            If OTAllowSTS = 1 Then

                If Val(Common_Procedures.settings.OT_Allowed_Only_After_ShiftOut_Time_Status) = 1 Then

                    '-----------------------OT only after shift out time

                    Out_mins_1 = 0
                    Out_mins_1 = Math.Abs(DateDiff(DateInterval.Minute, ShftOutTm, DtTm1))
                    OTMnts = Out_mins_1


                Else

                    '-----------------------OT after and before shift times

                    If Trim(SalTyp_ShftMnth) = "SHIFT" Then
                        SftMnts = (NoofShfts * ActShftMnts)
                        OTMnts = TotMins - SftMnts

                    Else
                        If Val(TotMins) >= ActShftMnts Then
                            SftMnts = (NoofShfts * ActShftMnts)
                            OTMnts = TotMins - SftMnts

                        Else
                            SftMnts = TotMins

                        End If

                    End If

                End If

            Else

                SftMnts = TotMins

            End If

        End If

        H = SftMnts \ 60
        M = SftMnts - (H * 60)
        SftHrs = H & "." & Format(M, "00")

        H = OTMnts \ 60
        M = OTMnts - (H * 60)
        OtHrs = H & "." & Format(M, "00")

        H = TotInMins \ 60
        M = TotInMins - (H * 60)
        InHrs = H & "." & Format(M, "00")

        H = TotOutMins \ 60
        M = TotOutMins - (H * 60)
        OutHrs = H & "." & Format(M, "00")

        If EmpNM <> "" Or EmpCode <> "" Then

            n = dgv_Details.Rows.Add()
            dgv_Details.Rows(n).Cells(0).Value = Sno
            dgv_Details.Rows(n).Cells(1).Value = IIf(EmpNM <> "", EmpNM, EmpCode)
            dgv_Details.Rows(n).Cells(2).Value = Catg_Nm
            dgv_Details.Rows(n).Cells(3).Value = ShftNm
            dgv_Details.Rows(n).Cells(4).Value = DispInOutStr
            dgv_Details.Rows(n).Cells(5).Value = Format(Val(TotHrs), "######0.00")
            If Val(dgv_Details.Rows(n).Cells(5).Value) = 0 Then dgv_Details.Rows(n).Cells(5).Value = ""
            dgv_Details.Rows(n).Cells(6).Value = ""
            dgv_Details.Rows(n).Cells(7).Value = Format(Val(SftHrs), "######0.00")
            If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then dgv_Details.Rows(n).Cells(7).Value = ""
            dgv_Details.Rows(n).Cells(8).Value = Val(NoofShfts)
            If Val(dgv_Details.Rows(n).Cells(8).Value) = 0 Then dgv_Details.Rows(n).Cells(8).Value = ""
            dgv_Details.Rows(n).Cells(9).Value = Val(OtHrs)
            If Val(dgv_Details.Rows(n).Cells(9).Value) = 0 Then dgv_Details.Rows(n).Cells(9).Value = ""
            dgv_Details.Rows(n).Cells(10).Value = Format(Val(LateHrs), "#####0.00")
            If Val(dgv_Details.Rows(n).Cells(10).Value) = 0 Then dgv_Details.Rows(n).Cells(10).Value = ""
            dgv_Details.Rows(n).Cells(11).Value = Format(Val(eOutHrs), "#####0.00")
            If Val(dgv_Details.Rows(n).Cells(11).Value) = 0 Then dgv_Details.Rows(n).Cells(11).Value = ""
            dgv_Details.Rows(n).Cells(12).Value = Val(InHrs)
            If Val(dgv_Details.Rows(n).Cells(12).Value) = 0 Then dgv_Details.Rows(n).Cells(12).Value = ""
            dgv_Details.Rows(n).Cells(13).Value = Val(OutHrs)
            If Val(dgv_Details.Rows(n).Cells(13).Value) = 0 Then dgv_Details.Rows(n).Cells(13).Value = ""

            a = Split(DispInOutStr, ",")
            If (UBound(a)) Mod 2 = 0 Then
                dgv_Details.Rows(n).Cells(4).Style.ForeColor = Color.Red
            End If

        End If

        Ind = 0
        Erase InOutTimeArr
        InOutTimeArr = New String(100) {}

    End Sub

    Private Sub Calculation_Working_Hours_Shift_OT(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim OtHrs As Double = 0, OTMnts As Double = 0
        Dim SftHrs As Double = 0, SftMnts As Double = 0
        Dim H As Long = 0, M As Long = 0, Mins As Long = 0, TotMins As Long = 0
        Dim Hrs As String = "", SalTyp_ShftMnth As String = ""
        Dim TtMn As Long = 0
        Dim OTAllowSTS As Integer = 0
        Dim OTAllowMnts As Double = 0, NoofShfts As Double = 0, HalfShtMns As Double = 0
        Dim ActShftMnts As Double = 0, BalMins As Double = 0
        Dim Catg_ID As Integer = 0
        Dim Shift_Minutes_1 As Long = 0
        Dim Shift_Minutes_2 As Long = 0
        Dim Shift_Minutes_3 As Long = 0
        Dim ShftNm As String = ""
        Dim ShftID As Integer = 0

        If NoCalc_Status = True Or FrmLdSTS = True Then Exit Sub

        OtHrs = 0 : OTMnts = 0 : SftHrs = 0 : SftMnts = 0

        Hrs = Int(Val(dgv_Details.Rows(CurRow).Cells(5).Value))
        Mins = (Val(dgv_Details.Rows(CurRow).Cells(5).Value) - Val(Hrs)) * 100
        TotMins = (Val(Hrs) * 60) + Val(Mins) + Val(dgv_Details.Rows(CurRow).Cells(6).Value)

        Shift_Minutes_1 = 0
        Shift_Minutes_2 = 0
        Shift_Minutes_3 = 0

        SalTyp_ShftMnth = ""
        OTAllowSTS = 0 : OTAllowMnts = 0

        Catg_ID = Common_Procedures.Category_NameToIdNo(con, dgv_Details.Rows(CurRow).Cells(2).Value)

        Da = New SqlClient.SqlDataAdapter("select * from PayRoll_Category_Head Where Category_IdNo = " & Str(Val(Catg_ID)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            Shift_Minutes_1 = Val(Dt1.Rows(0).Item("Shift1_Working_Minutes").ToString)
            Shift_Minutes_2 = Val(Dt1.Rows(0).Item("Shift2_Working_Minutes").ToString)
            Shift_Minutes_3 = Val(Dt1.Rows(0).Item("Shift3_Working_Minutes").ToString)

            OTAllowSTS = Val(Dt1.Rows(0).Item("OT_Allowed").ToString)
            OTAllowMnts = Val(Dt1.Rows(0).Item("OT_Allowed_After_Minutes").ToString)
            SalTyp_ShftMnth = Dt1.Rows(0).Item("Monthly_Shift").ToString

        End If
        Dt1.Clear()

        ShftNm = dgv_Details.Rows(CurRow).Cells(3).Value
        ShftID = Common_Procedures.Shift_NameToIdNo(con, ShftNm)

        OtHrs = 0 : OTMnts = 0 : SftHrs = 0 : SftMnts = 0 : NoofShfts = 0
        If TotMins > 0 Then

            If ShftID = 3 Then
                ActShftMnts = Val(Shift_Minutes_3)
            ElseIf ShftID = 2 Then
                ActShftMnts = Val(Shift_Minutes_2)
            Else
                ActShftMnts = Val(Shift_Minutes_1)
            End If
            If Val(ActShftMnts) = 0 Then ActShftMnts = Val(Shift_Minutes_1)
            HalfShtMns = Int(ActShftMnts / 2)

            If Trim(UCase(SalTyp_ShftMnth)) = "SHIFT" Then
                NoofShfts = Int(TotMins / ActShftMnts)
                SftMnts = (NoofShfts * ActShftMnts)
                BalMins = TotMins - (NoofShfts * ActShftMnts)
                If Val(BalMins) >= HalfShtMns Then NoofShfts = NoofShfts + 0.5

            Else
                If Val(TotMins) >= HalfShtMns Then NoofShfts = 1 Else NoofShfts = 0.5

            End If

            If OTAllowSTS = 1 Then
                If Trim(UCase(SalTyp_ShftMnth)) = "SHIFT" Then
                    SftMnts = (NoofShfts * ActShftMnts)
                    OTMnts = TotMins - SftMnts
                Else
                    If Val(TotMins) >= ActShftMnts Then
                        SftMnts = (NoofShfts * ActShftMnts)
                        OTMnts = TotMins - SftMnts
                    Else
                        SftMnts = TotMins
                    End If
                End If

            Else
                SftMnts = TotMins

            End If

        End If

        H = SftMnts \ 60
        m = SftMnts - (H * 60)
        SftHrs = H & "." & Format(m, "00")

        dgv_Details.Rows(CurRow).Cells(7).Value = Format(Val(SftHrs), "00.00")
        If Val(dgv_Details.Rows(CurRow).Cells(7).Value) = 0 Then dgv_Details.Rows(CurRow).Cells(7).Value = ""

        dgv_Details.Rows(CurRow).Cells(8).Value = Val(NoofShfts)
        If Val(dgv_Details.Rows(CurRow).Cells(8).Value) = 0 Then dgv_Details.Rows(CurRow).Cells(8).Value = ""

        H = OTMnts \ 60
        m = OTMnts - (H * 60)
        OtHrs = H & "." & Format(m, "00")

        dgv_Details.Rows(CurRow).Cells(9).Value = Format(Val(OtHrs), "00.00")
        If Val(dgv_Details.Rows(CurRow).Cells(9).Value) = 0 Then dgv_Details.Rows(CurRow).Cells(9).Value = ""

    End Sub

    Private Sub btn_AbsentList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AbsentList.Click
        Dim n As Integer
        Dim sno As Integer

        dgv_AbsentList.Rows.Clear()
        sno = 0
        If dgv_Details.Rows.Count > 0 Then

            For i = 0 To dgv_Details.Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(5).Value) = 0 And Val(dgv_Details.Rows(i).Cells(6).Value) = 0 Then

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
    Private Sub btn_absent_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_absent_Close.Click
        pnl_Back.Enabled = True
        Pnl_AbsentList.Visible = False
    End Sub

    Private Sub btn_LatecomersList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_LatecomersList.Click
        Dim n As Integer
        Dim sno As Integer

        dgv_AbsentList.Rows.Clear()
        sno = 0
        If dgv_Details.Rows.Count > 0 Then

            For i = 0 To dgv_Details.Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(10).Value) <> 0 Then

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
End Class