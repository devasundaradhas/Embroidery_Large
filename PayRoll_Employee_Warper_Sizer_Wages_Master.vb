Public Class PayRoll_Employee_Warper_Sizer_Wages_Master

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EMPWG-"
    Private Pk_Condition2 As String = "YPAGC-"
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


        lbl_EmpNo.Text = ""
        lbl_EmpNo.ForeColor = Color.Black


        cbo_EmployeeName.Text = ""

        txt_FrontWarper.Text = "0.00"
        txt_BackWarper.Text = "0.00"
        txt_Helper.Text = "0.00"
        txt_FrontSizer.Text = "0.00"
        txt_BackSizer.Text = "0.00"
        txt_Boiler.Text = "0.00"
        txt_Cooker.Text = "0.00"

        dgv_Details.Rows.Clear()


        Grid_Cell_DeSelect()

        cbo_Shift.Visible = False


        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
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

        If Me.ActiveControl.Name <> cbo_Shift.Name Then
            cbo_Shift.Visible = False
        End If




        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
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

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Wages_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Employee_Wages_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_EmpNo.Text = dt1.Rows(0).Item("Employee_Wages_No").ToString
                cbo_EmployeeName.Text = Common_Procedures.Employee_IdNoToName(con, Val(dt1.Rows(0).Item("Employee_IdNo").ToString))
                txt_FrontWarper.Text = Format(Val(dt1.Rows(0).Item("Front_Warper").ToString), "#########0.00")
                txt_BackWarper.Text = Format(Val(dt1.Rows(0).Item("Back_Warper").ToString), "#########0.00")
                txt_Helper.Text = Format(Val(dt1.Rows(0).Item("Helper").ToString), "#########0.00")
                txt_FrontSizer.Text = Format(Val(dt1.Rows(0).Item("Front_Sizer").ToString), "#########0.00")
                txt_BackSizer.Text = Format(Val(dt1.Rows(0).Item("Back_Sizer").ToString), "#########0.00")
                txt_Boiler.Text = Format(Val(dt1.Rows(0).Item("Boiler").ToString), "#########0.00")
                txt_Cooker.Text = Format(Val(dt1.Rows(0).Item("Cooker").ToString), "#########0.00")
                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Shift_Name from PayRoll_Employee_Wages_Details a INNER JOIN Shift_Head b ON a.Shift_IdNo = b.Shift_IdNo  Where a.Employee_Wages_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Shift_Name").ToString
                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Weight_From").ToString), "########0.000")
                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Weight_To").ToString), "########0.000")
                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Front_Sizing_Wages").ToString), "########0.000")
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Back_Sizing_Wages").ToString), "########0.000")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Boiler_Wages").ToString), "########0.000")
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Cooker_Wages").ToString), "########0.000")

                        Next i

                    End If

                    '  If .RowCount = 0 Then .Rows.Add()

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

            If cbo_EmployeeName.Visible And cbo_EmployeeName.Enabled Then cbo_EmployeeName.Focus()

        End Try

        NoCalc_Status = False



    End Sub

    Private Sub Employee_Warper_Sizer_Wages_Master_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EmployeeName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EmployeeName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Shift.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SHIFT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Shift.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Employee_Warper_Sizer_Wages_Master_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Me.Text = ""

        con.Open()

        ' Common_Procedures.get_CashPartyName_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", con)
        da.Fill(dt1)
        cbo_EmployeeName.DataSource = dt1
        cbo_EmployeeName.DisplayMember = "Employee_Name"


        da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head  order by Shift_Name", con)
        da.Fill(dt2)
        cbo_Shift.DataSource = dt2
        cbo_Shift.DisplayMember = "Shift_Name"

        AddHandler cbo_EmployeeName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FrontWarper.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BackWarper.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Helper.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BackSizer.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FrontSizer.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Boiler.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cooker.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_EmployeeName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FrontWarper.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BackWarper.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Helper.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BackSizer.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FrontSizer.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Boiler.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cooker.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_FrontWarper.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BackWarper.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Helper.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BackSizer.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FrontSizer.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Boiler.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_FrontWarper.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BackWarper.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Helper.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BackSizer.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FrontSizer.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Boiler.KeyPress, AddressOf TextBoxControlKeyPress




        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Employee_Warper_Sizer_Wages_Master_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Employee_Warper_Sizer_Wages_Master_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then



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

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                cbo_EmployeeName.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                cbo_EmployeeName.Focus()
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            txt_Cooker.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EmpNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans




            cmd.CommandText = "delete from PayRoll_Employee_Wages_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from PayRoll_Employee_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code = '" & Trim(NewCode) & "'"
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

            If cbo_EmployeeName.Enabled = True And cbo_EmployeeName.Visible = True Then cbo_EmployeeName.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record


    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Wages_No from PayRoll_Employee_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Employee_Wages_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_EmpNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Wages_No from PayRoll_Employee_Wages_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Employee_Wages_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_EmpNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Wages_No from PayRoll_Employee_Wages_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Employee_Wages_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Employee_Wages_No from PayRoll_Employee_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Employee_Wages_No desc", con)
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
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_EmpNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Wages_Head", "Employee_Wages_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_EmpNo.ForeColor = Color.Red



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If cbo_EmployeeName.Enabled And cbo_EmployeeName.Visible Then cbo_EmployeeName.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Emp No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Employee_Wages_No from PayRoll_Employee_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Emp No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Emp No.", "FOR NEW EMP NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Employee_Wages_No from PayRoll_Employee_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Rmp No.", "DOES NOT INSERT NEW EMP...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_EmpNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW EMP...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Sft_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        ' Dim vTotNoofs As Single, vTotQty As Single, vTotAmt As Single

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If



        Emp_ID = Common_Procedures.Employee_NameToIdNo(con, cbo_EmployeeName.Text)
        If Val(Emp_ID) = 0 Then
            MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EmployeeName.Enabled And cbo_EmployeeName.Visible Then cbo_EmployeeName.Focus()
            Exit Sub
        End If



        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    Sft_ID = Common_Procedures.Shift_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Sft_ID = 0 Then
                        MessageBox.Show("Invalid Shift ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If



                    '    If Val(.Rows(i).Cells(5).Value) = 0 Then
                    '        MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '        If .Enabled And .Visible Then
                    '            .Focus()
                    '            .CurrentCell = .Rows(i).Cells(5)
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
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EmpNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_EmpNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Wages_Head", "Employee_Wages_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_EmpNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr


            If New_Entry = True Then
                cmd.CommandText = "Insert into PayRoll_Employee_Wages_Head (       Employee_Wages_Code ,               Company_IdNo       ,           Employee_Wages_No    ,                            for_OrderBy                           ,         Employee_IdNo      ,       Front_Warper              ,           Back_Warper                     ,            Helper                    , Front_Sizer                          ,                  Back_Sizer          ,             Boiler                ,         Cooker        ) " & _
                                    "     Values                  (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_EmpNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_EmpNo.Text))) & ",  " & Str(Val(Emp_ID)) & ", " & Val(txt_FrontWarper.Text) & " , " & Str(Val(txt_BackWarper.Text)) & ",     " & Str(Val(txt_Helper.Text)) & " , " & Str(Val(txt_FrontSizer.Text)) & " , " & Str(Val(txt_BackSizer.Text)) & " , " & Str(Val(txt_Boiler.Text)) & "  , " & Str(Val(txt_Cooker.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update PayRoll_Employee_Wages_Head set Employee_IdNo = " & Str(Val(Emp_ID)) & ", Front_Warper = " & Trim(txt_FrontWarper.Text) & ",  Back_Warper = " & Str(Val(txt_BackWarper.Text)) & ", Helper = " & Str(Val(txt_Helper.Text)) & " , Front_Sizer = " & Trim(txt_FrontSizer.Text) & ",  Back_Sizer = " & Str(Val(txt_BackSizer.Text)) & ", Boiler = " & Str(Val(txt_Boiler.Text)) & " , Cooker = " & Str(Val(txt_Cooker.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "Delete from PayRoll_Employee_Wages_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()




            With dgv_Details

                Sno = 0
                'YrnClthNm = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Sft_ID = Common_Procedures.Shift_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)


                        cmd.CommandText = "Insert into PayRoll_Employee_Wages_Details ( Employee_Wages_Code ,               Company_IdNo       ,   Employee_Wages_No    ,                     for_OrderBy                                            ,               Sl_No     ,              Shift_IdNo        ,                     Weight_From                   ,                 Weight_To               ,         Front_Sizing_Wages         ,                    Back_Sizing_Wages               ,                 Boiler_Wages   ,       Cooker_Wages                 ) " & _
                                            "     Values                 (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_EmpNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_EmpNo.Text))) & " ,  " & Str(Val(Sno)) & ", " & Str(Val(Sft_ID)) & ",  " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ",  " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " , " & Str(Val(.Rows(i).Cells(7).Value)) & " ) "
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
                    move_record(lbl_EmpNo.Text)
                End If
            Else
                move_record(lbl_EmpNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If cbo_EmployeeName.Enabled And cbo_EmployeeName.Visible Then cbo_EmployeeName.Focus()


        End Try

    End Sub


    Private Sub cbo_Employee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EmployeeName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EmployeeName, Nothing, txt_FrontWarper, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_EmployeeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EmployeeName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EmployeeName, txt_FrontWarper, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Shift.Visible = False Or Val(cbo_Shift.Tag) <> e.RowIndex Then

                    cbo_Shift.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Shift.DataSource = Dt1
                    cbo_Shift.DisplayMember = "Shift_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Shift.Left = .Left + rect.Left
                    cbo_Shift.Top = .Top + rect.Top

                    cbo_Shift.Width = rect.Width
                    cbo_Shift.Height = rect.Height
                    cbo_Shift.Text = .CurrentCell.Value

                    cbo_Shift.Tag = Val(e.RowIndex)
                    cbo_Shift.Visible = True

                    cbo_Shift.BringToFront()
                    cbo_Shift.Focus()

                End If

            Else
                cbo_Shift.Visible = False

            End If



        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        'On Error Resume Next

        'With dgv_Details
        '    If .Visible Then
        '        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
        '            Quantity_Calculation(e.RowIndex, e.ColumnIndex)
        '            Amount_Calculation(e.RowIndex, e.ColumnIndex)
        '        End If
        '    End If
        'End With

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub


    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

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
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_EmployeeName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EmployeeName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EmployeeName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub






    Private Sub txt_Helper_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Helper.LostFocus
        txt_Helper.Text = Format(Val(txt_Helper.Text), "#########0.00")
    End Sub
    Private Sub txt_FrontWarper_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FrontWarper.LostFocus
        txt_FrontWarper.Text = Format(Val(txt_FrontWarper.Text), "#########0.00")
    End Sub


    Private Sub txt_Frontwarper_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FrontWarper.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_BackWarper_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BackWarper.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub








    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub







    Private Sub cbo_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, Nothing, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_Cooker.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_EmployeeName.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Shift.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_EmployeeName.Focus()
                    End If
                Else
                    .Focus()

                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If


    End Sub

    Private Sub cbo_Shift_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    Dim f As New Shift_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_Shift.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()
        'End If

    End Sub



    Private Sub cbo_Grid_Shift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.TextChanged
        Try
            If cbo_Shift.Visible Then
                With dgv_Details
                    If Val(cbo_Shift.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Shift.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub




    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub



    Private Sub txt_BackWarper_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_BackWarper.LostFocus
        txt_BackWarper.Text = Format(Val(txt_BackWarper.Text), "#########0.00")
    End Sub


    Private Sub txt_FrontSizer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FrontSizer.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Boiler_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Boiler.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_BackSizer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BackSizer.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Cooker_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Cooker.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True



            End If
        End If

        If e.KeyValue = 38 Then txt_Boiler.Focus()
    End Sub

    Private Sub txt_Cooker_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cooker.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_BackSizer_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_BackSizer.LostFocus
        txt_BackSizer.Text = Format(Val(txt_BackSizer.Text), "#########0.00")
    End Sub

    Private Sub txt_Cooker_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Cooker.LostFocus
        txt_Cooker.Text = Format(Val(txt_Cooker.Text), "#########0.00")
    End Sub

    Private Sub txt_FrontSizer_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FrontSizer.LostFocus
        txt_FrontSizer.Text = Format(Val(txt_FrontSizer.Text), "#########0.00")
    End Sub

    Private Sub txt_Boiler_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Boiler.LostFocus
        txt_Boiler.Text = Format(Val(txt_Boiler.Text), "#########0.00")
    End Sub
End Class