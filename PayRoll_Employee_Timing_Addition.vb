Public Class PayRoll_Employee_Timing_Addition
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "TIMAD-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    'Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

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

        lbl_Day.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()

        Grid_Cell_DeSelect()

        cbo_Grid_Employee.Visible = False
        cbo_Grid_Employee.Tag = -1

        cbo_Grid_InOut.Visible = False
        cbo_Grid_InOut.Tag = -1

        dtp_Grid_Time.Visible = False
        dtp_Grid_Time.Tag = -1

        'dtp_Date.Tag = ""

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

        If Me.ActiveControl.Name <> cbo_Grid_Employee.Name Then
            cbo_Grid_Employee.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_InOut.Name Then
            cbo_Grid_InOut.Visible = False
        End If
        If Me.ActiveControl.Name <> dtp_Grid_Time.Name Then
            dtp_Grid_Time.Visible = False
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
        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False

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

            da1 = New SqlClient.SqlDataAdapter("select a.* from Payroll_Timing_Addition_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Timing_Addition_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Timing_Addition_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Timing_Addition_Date").ToString

                dtp_Date.Tag = dtp_Date.Text

                lbl_Day.Text = dt1.Rows(0).Item("Day_Name").ToString

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name from Payroll_Timing_Addition_Details a INNER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo <> 0 and a.Employee_IdNo = b.Employee_IdNo  Where a.Timing_Addition_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("InOut_Type").ToString
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("InOut_Time_Text").ToString

                        Next i

                    End If

                    n = .Rows.Add()

                End With

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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Employee_Attendance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Me.Text = ""

        con.Open()

        cbo_Grid_InOut.Items.Clear()
        cbo_Grid_InOut.Items.Add("IN")
        cbo_Grid_InOut.Items.Add("OUT")

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Grid_Time.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Employee.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_InOut.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Grid_Time.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Employee.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_InOut.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

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
            'MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            dtp_Date.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(3)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Attendance_Missing_Time_Addition, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Attendance_Missing_Time_Addition, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

            cmd.CommandText = "delete from Payroll_Timing_Addition_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Payroll_Timing_Addition_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Timing_Addition_No from Payroll_Timing_Addition_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Timing_Addition_No", con)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Timing_Addition_No from Payroll_Timing_Addition_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Timing_Addition_No", con)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Timing_Addition_No from Payroll_Timing_Addition_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Timing_Addition_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Timing_Addition_No from Payroll_Timing_Addition_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Timing_Addition_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Payroll_Timing_Addition_Head", "Timing_Addition_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red
            'Employee_List()

            Dt1.Clear()


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

            Da = New SqlClient.SqlDataAdapter("select Timing_Addition_No from Payroll_Timing_Addition_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code = '" & Trim(RefCode) & "'", con)
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

            Da = New SqlClient.SqlDataAdapter("select Timing_Addition_No from Payroll_Timing_Addition_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code = '" & Trim(InvCode) & "'", con)
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
        Dim OrdBy As String = ""
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Empe_ID As Integer = 0
        Dim WrkTy_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim OT_Mins As Integer = 0
        Dim EntID As String = ""
        Dim Ot_Dbl As Double = 0
        Dim Ot_Int As Integer = 0
        Dim IODtTm As Date
        Dim DtTm1 As Date
        Dim In_Out As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Attendance_Missing_Time_Addition, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If


        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    Empe_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Empe_ID = 0 Then
                        MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    '    If Val(.Rows(i).Cells(3).Value) > 3 Then
                    '        MessageBox.Show("Invalid SHIFT", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '        If .Enabled And .Visible Then
                    '            .Focus()
                    '            .CurrentCell = .Rows(i).Cells(3)
                    '        End If
                    '        Exit Sub
                    '    End If

                    '    If Val(.Rows(i).Cells(4).Value) > 24 Then
                    '        MessageBox.Show("Invalid OT Hours", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '        If .Enabled And .Visible Then
                    '            .Focus()
                    '            .CurrentCell = .Rows(i).Cells(4)
                    '        End If
                    '        Exit Sub
                    '    End If

                End If

            Next

        End With


        NoCalc_Status = False


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Payroll_Timing_Addition_Head", "Timing_Addition_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            OrdBy = Format(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)), "#########0.00").ToString

            If New_Entry = True Then
                cmd.CommandText = "Insert into Payroll_Timing_Addition_Head (   Timing_Addition_Code   ,               Company_IdNo       ,           Timing_Addition_No  ,         for_OrderBy    , Timing_Addition_Date ,       Day_Name               ) " &
                                  "            Values                       (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdBy)) & ",      @EntryDate      , '" & Trim(lbl_Day.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Payroll_Timing_Addition_Head set Timing_Addition_Date = @EntryDate,  Day_Name = '" & Trim(lbl_Day.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Payroll_Timing_Addition_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                For i = 0 To .RowCount - 1

                    Empe_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(Empe_ID) <> 0 Then

                        If Trim(.Rows(i).Cells(3).Value) <> "" Then

                            If IsDate(.Rows(i).Cells(3).Value) = True Then

                                DtTm1 = Convert.ToDateTime(.Rows(i).Cells(3).Value)

                                In_Out = "IN"

                                IODtTm = New Date(Year(dtp_Date.Value.Date), Month(dtp_Date.Value.Date), Microsoft.VisualBasic.Day(dtp_Date.Value.Date), Hour(DtTm1), Minute(DtTm1), 0)

                                cmd.Parameters.Clear()
                                cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)
                                cmd.Parameters.AddWithValue("@InOutDateTime", IODtTm)

                                Sno = Sno + 1

                                cmd.CommandText = "Insert into Payroll_Timing_Addition_Details (         Timing_Addition_Code                ,               Company_IdNo       ,       Timing_Addition_No      ,     for_OrderBy        ,   Timing_Addition_Date,             Sl_No     ,      Employee_IdNo       ,      InOut_Type        ,                 InOut_Time_Text        ,  InOut_DateTime ) " &
                                                  "            Values                          ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(OrdBy)) & ",       @EntryDate      ,  " & Str(Val(Sno)) & ", " & Str(Val(Empe_ID)) & ",  '" & Trim(In_Out) & "', '" & Trim(.Rows(i).Cells(3).Value) & "', @InOutDateTime  ) "
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
            If InStr(1, Trim(LCase(ex.Message)), "ix_Payroll_Timing_Addition_Details_1") > 0 Then
                MessageBox.Show("Duplicate Employee Timings", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_Payroll_Timing_Addition_Details_2") > 0 Then
                MessageBox.Show("Duplicate Employee Timings", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try

            If NoCalc_Status = True Or FrmLdSTS = True Then Exit Sub

            With dgv_Details
                If .Rows.Count - 1 = e.RowIndex Then
                    If (e.ColumnIndex = 1 Or e.ColumnIndex = 2 Or e.ColumnIndex = 3) And Trim(.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) <> "" Then
                        .Rows.Add()
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try

    End Sub

    'Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
    '    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub

    'Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
    '    dgv_Details.EditingControl.BackColor = Color.Lime
    '    dgv_Details.EditingControl.ForeColor = Color.Blue
    '    dgtxt_Details.SelectAll()
    'End Sub

    'Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
    '    If dgv_Details.CurrentCell.ColumnIndex = 3 Then
    '        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
    '            e.Handled = True
    '        End If
    '    End If
    'End Sub

    'Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
    '    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
    '        dgv_Details_KeyUp(sender, e)
    '    End If
    'End Sub

    'Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
    '    Try
    '        With dgv_Details
    '            If .Rows.Count > 0 Then
    '                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
    '            End If
    '        End With

    '    Catch ex As Exception
    '        '---

    '    End Try
    'End Sub


    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0

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
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.GotFocus
        'dtp_Date.Tag = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyValue = 38 Or e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
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

                dtp_Date.Text = dtp_Date.Tag

            End If

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
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

            dtp_Date.Text = dtp_Date.Tag
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

            Cmd.CommandText = "select Timing_Addition_No from Payroll_Timing_Addition_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Timing_Addition_Date = @EntryDate"
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
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DATE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Employee, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)

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
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_InOut_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_InOut.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
        If Trim(cbo_Grid_InOut.Text) = "" Then cbo_Grid_InOut.Text = "IN"
    End Sub

    Private Sub cbo_Grid_InOut_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_InOut.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_InOut, Nothing, Nothing, "", "", "", "")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_InOut.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_InOut.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

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

    Private Sub cbo_Grid_InOut_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_InOut.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_InOut, Nothing, "", "", "", "", True)

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_InOut_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_InOut.TextChanged
        Try
            If cbo_Grid_InOut.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_InOut.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_InOut.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dtp_Grid_Time_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Grid_Time.KeyDown

        With dgv_Details

            If e.KeyValue = 38 Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                End If

            End If

        End With
    End Sub

    Private Sub dtp_Grid_Time_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Grid_Time.KeyPress

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(dgv_Details.CurrentCell.ColumnIndex).Value = Trim(dtp_Grid_Time.Text)

            With dgv_Details
                If .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                End If
            End With

        End If
    End Sub

    Private Sub dtp_Grid_Time_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Grid_Time.TextChanged
        Try
            If dtp_Grid_Time.Visible = True Then
                With dgv_Details
                    If .Visible = True Then
                        If .Rows.Count > 0 Then
                            If Val(dtp_Grid_Time.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dtp_Grid_Time.Text)
                            End If
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        Dim HH As String = ""
        Dim MM As String = ""
        Dim TT As String = ""
        Dim NoofRightKeys As String = ""


        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If (cbo_Grid_Employee.Visible = False Or Val(cbo_Grid_Employee.Tag) <> e.RowIndex) Then

                    cbo_Grid_Employee.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Employee.DataSource = Dt1
                    cbo_Grid_Employee.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Employee.Left = .Left + rect.Left
                    cbo_Grid_Employee.Top = .Top + rect.Top

                    cbo_Grid_Employee.Width = rect.Width
                    cbo_Grid_Employee.Height = rect.Height
                    cbo_Grid_Employee.Text = .CurrentCell.Value

                    cbo_Grid_Employee.Tag = Val(e.RowIndex)
                    cbo_Grid_Employee.Visible = True

                    cbo_Grid_Employee.BringToFront()
                    cbo_Grid_Employee.Focus()

                End If

            Else
                cbo_Grid_Employee.Visible = False

            End If


            If e.ColumnIndex = 2 Then

                If (cbo_Grid_InOut.Visible = False Or Val(cbo_Grid_InOut.Tag) <> e.RowIndex) Then

                    cbo_Grid_InOut.Tag = -1

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_InOut.Left = .Left + rect.Left
                    cbo_Grid_InOut.Top = .Top + rect.Top

                    cbo_Grid_InOut.Width = rect.Width
                    cbo_Grid_InOut.Height = rect.Height
                    cbo_Grid_InOut.Text = .CurrentCell.Value

                    cbo_Grid_InOut.Tag = Val(e.RowIndex)
                    cbo_Grid_InOut.Visible = True

                    cbo_Grid_InOut.BringToFront()
                    cbo_Grid_InOut.Focus()

                End If

            Else
                cbo_Grid_InOut.Visible = False

            End If

            If e.ColumnIndex = 3 Then

                If (dtp_Grid_Time.Visible = False Or Val(dtp_Grid_Time.Tag) <> e.RowIndex) Then

                    dtp_Grid_Time.Tag = -1

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    dtp_Grid_Time.Left = .Left + rect.Left
                    dtp_Grid_Time.Top = .Top + rect.Top

                    dtp_Grid_Time.Width = rect.Width
                    dtp_Grid_Time.Height = rect.Height

                    'dtp_Grid_Time.Visible = True
                    'dtp_Grid_Time.Text = "11:55 AM"
                    'dtp_Grid_Time.Focus()
                    'SendKeys.Send("{+}")

                    'HH = Microsoft.VisualBasic.Left(dtp_Grid_Time.Text, 2)
                    'MM = Microsoft.VisualBasic.Mid(dtp_Grid_Time.Text, 4, 2)
                    'TT = Microsoft.VisualBasic.Right(dtp_Grid_Time.Text, 2)

                    'NoofRightKeys = 0
                    'If Trim(UCase(TT)) = "PM" Then
                    '    NoofRightKeys = 2
                    'ElseIf Trim(Val(MM)) = 56 Then
                    '    NoofRightKeys = 1
                    'End If

                    dtp_Grid_Time.Text = ""
                    If IsDate(.CurrentCell.Value) = True Then
                        dtp_Grid_Time.Text = .CurrentCell.Value
                    End If

                    dtp_Grid_Time.Tag = Val(e.RowIndex)
                    dtp_Grid_Time.Visible = True

                    dtp_Grid_Time.BringToFront()
                    dtp_Grid_Time.Focus()

                    'If NoofRightKeys <> 0 Then
                    '    If NoofRightKeys = 1 Then
                    '        SendKeys.Send("{LEFT}")
                    '    ElseIf NoofRightKeys = 2 Then
                    '        SendKeys.Send("{LEFT}")
                    '        SendKeys.Send("{LEFT}")
                    '    End If
                    'End If

                End If

            Else
                dtp_Grid_Time.Visible = False

            End If

        End With

    End Sub


End Class