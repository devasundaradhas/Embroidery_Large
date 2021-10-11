Imports System.IO

Public Class Payroll_AttendanceLog_FromMachine_Chennai
    Implements Interface_MDIActions

    'Public axCZKEM1 As New zkemkeeper.CZKEM
    Private bIsConnected = False 'the boolean value identifies whether the device is connected
    Private iMachineNumber As Integer 'the serial number of the device.After connecting the device ,this value will be changed.

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "ATTFM-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private Save_Sts As Boolean = False

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        lvLogs.Items.Clear()

        dtp_Date.Text = ""

        Save_Sts = False

        NoCalc_Status = False

        dtp_Date.Enabled = False

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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub AttendanceLog_FromMachine_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If FrmLdSTS = True Then

                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                CompCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompCondt = "Company_Type = 'ACCOUNT'"
                End If

                da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
                dt1 = New DataTable
                da.Fill(dt1)

                NoofComps = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        NoofComps = Val(dt1.Rows(0)(0).ToString)
                    End If
                End If
                dt1.Clear()

                If Val(NoofComps) = 1 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                        End If

                    End If
                    dt1.Clear()

                Else

                    Dim f As New Company_Selection
                    f.ShowDialog()

                End If

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()

                    new_record()

                Else
                    MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'Me.Close()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub AttendanceLog_FromMachine_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub AttendanceLog_FromMachine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then
                Close_Form()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub AttendanceLog_FromMachine_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()

        dtp_Date.Text = ""

        pnl_Settings_FileFrom.Visible = False
        pnl_Settings_FileFrom.Left = 60  '(Me.Width - pnl_Settings_FileFrom.Width) \ 2
        pnl_Settings_FileFrom.Top = 150 ' (Me.Height - pnl_Settings_FileFrom.Height) \ 2
        pnl_Settings_FileFrom.BringToFront()


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmpAttDate_FileFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmpCardNo_FileFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmpInOut_FileFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LineStartFrom_FileFrom.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmpAttDate_FileFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmpCardNo_FileFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmpInOut_FileFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LineStartFrom_FileFrom.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmpAttDate_FileFrom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmpCardNo_FileFrom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmpInOut_FileFrom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LineStartFrom_FileFrom.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_EmpAttDate_FileFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmpCardNo_FileFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmpInOut_FileFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LineStartFrom_FileFrom.KeyPress, AddressOf TextBoxControlKeyPress


        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Private Sub Close_Form()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "Company_Type = 'ACCOUNT'"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    Me.Close()

                End If

            Else

                Me.Close()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim sno As Integer = 0
        Dim n As Integer = 0
        Dim iGLCount = 0
        Dim lvItem As New ListViewItem("Items", 0)


        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            con1.Open()
            da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", con1)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                txt_EmpAttDate_FileFrom.Text = dt1.Rows(0).Item("FileFrom_EmpDate_indx").ToString
                txt_EmpCardNo_FileFrom.Text = dt1.Rows(0).Item("FileFrom_EmpCode_indx").ToString
                txt_EmpInOut_FileFrom.Text = dt1.Rows(0).Item("FileFrom_EmpInOutMode_indx").ToString
                txt_LineStartFrom_FileFrom.Text = dt1.Rows(0).Item("FileFrom_LineStartFrom").ToString
            End If
            dt1.Dispose()
            da1.Dispose()
            con1.Close()

            da1 = New SqlClient.SqlDataAdapter("select a.* from Payroll_AttendanceLog_FromMachine_Head a Where a.AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                dtp_Date.Enabled = False

                lbl_RefNo.Text = dt1.Rows(0).Item("AttendanceLog_FromMachine_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("AttendanceLog_FromMachine_Date").ToString
            End If

            lvLogs.Items.Clear()

            da2 = New SqlClient.SqlDataAdapter("Select a.* from Payroll_AttendanceLog_FromMachine_Details a Where a.AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            With lvLogs

                sno = 0
                n = 0
                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = n + 1

                        lvItem = lvLogs.Items.Add(n.ToString())
                        lvItem.SubItems.Add(dt2.Rows(i).Item("Employee_CardNo").ToString)
                        lvItem.SubItems.Add("")  '--idwVerifyMode.ToString() 
                        lvItem.SubItems.Add(Val(dt2.Rows(i).Item("IN_Out").ToString))
                        lvItem.SubItems.Add(dt2.Rows(i).Item("INOut_DateTime_Text").ToString)
                        lvItem.SubItems.Add(Year(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Month(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Microsoft.VisualBasic.DateAndTime.Day(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Hour(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Minute(dt2.Rows(i).Item("INOut_DateTime")) & "~" & Second(dt2.Rows(i).Item("INOut_DateTime")))
                        lvItem.SubItems.Add("")

                    Next i

                End If
            End With

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""


        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Attendance_Log_From_Machine, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Attendance_Log_From_Machine, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If Pnl_Back.Enabled = False Then
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

            NewCode = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Payroll_AttendanceLog_FromMachine_Details Where AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "'"
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
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, AttendanceLog_FromMachine_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where for_orderby > " & Str(Val(OrdByNo)) & " and AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, AttendanceLog_FromMachine_No", con)
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

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where for_orderby < " & Str(Val(OrdByNo)) & " and AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, AttendanceLog_FromMachine_No desc", con)
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

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, AttendanceLog_FromMachine_No desc", con)
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

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Payroll_AttendanceLog_FromMachine_Head", "AttendanceLog_FromMachine_Code", "For_OrderBy", "", 0, Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            dtp_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, AttendanceLog_FromMachine_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("AttendanceLog_FromMachine_Date").ToString <> "" Then dtp_Date.Text = dt1.Rows(0).Item("AttendanceLog_FromMachine_Date").ToString
                End If
            End If
            dt1.Clear()
            da.Dispose()


            con1.Open()
            da = New SqlClient.SqlDataAdapter("select  * from Settings_Head ", con1)
            dt1 = New DataTable
            da.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                txt_EmpAttDate_FileFrom.Text = dt1.Rows(0).Item("FileFrom_EmpDate_indx").ToString
                txt_EmpCardNo_FileFrom.Text = dt1.Rows(0).Item("FileFrom_EmpCode_indx").ToString
                txt_EmpInOut_FileFrom.Text = dt1.Rows(0).Item("FileFrom_EmpInOutMode_indx").ToString
                txt_LineStartFrom_FileFrom.Text = dt1.Rows(0).Item("FileFrom_LineStartFrom").ToString
            End If
            dt1.Dispose()
            da.Dispose()
            con1.Close()

            dtp_Date.Enabled = True

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.AttendanceLog_FromMachine_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.AttendanceLog_FromMachine_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Led1_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim EntID As String = ""
        Dim Delv_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Led_type As String = ""
        Dim VouBil As String = ""
        Dim IO_DtTm_Arr() As String
        Dim InOut_Date As DateTime
        Dim movno As String = ""

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Attendance_Log_From_Machine, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Trim(lbl_RefNo.Text) = "" Then
            MessageBox.Show("Invalid Reference No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Payroll_AttendanceLog_FromMachine_Head", "AttendanceLog_FromMachine_Code", "For_OrderBy", "", 0, Common_Procedures.FnYearCode, tr)

                NewCode = Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If




            If New_Entry = True Then
                cmd.CommandText = "Insert into Payroll_AttendanceLog_FromMachine_Head(AttendanceLog_FromMachine_Code, AttendanceLog_FromMachine_No, for_OrderBy, AttendanceLog_FromMachine_Date ) Values ('" & Trim(NewCode) & "', '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate  )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Payroll_AttendanceLog_FromMachine_Head set AttendanceLog_FromMachine_Date = @EntryDate  Where  AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Payroll_AttendanceLog_FromMachine_Details Where AttendanceLog_FromMachine_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            If lvLogs.Items.Count > 0 Then
                For i = 0 To lvLogs.Items.Count - 1

                    If Val(lvLogs.Items(i).SubItems(1).Text) <> 0 Then

                        IO_DtTm_Arr = Split(Trim(lvLogs.Items(i).SubItems(5).Text), "~")

                        InOut_Date = New DateTime(Val(IO_DtTm_Arr(0)), Val(IO_DtTm_Arr(1)), Val(IO_DtTm_Arr(2)), Val(IO_DtTm_Arr(3)), Val(IO_DtTm_Arr(4)), Val(IO_DtTm_Arr(5)))

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)
                        cmd.Parameters.AddWithValue("@AttenDateTime", InOut_Date)

                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Payroll_AttendanceLog_FromMachine_Details ( AttendanceLog_FromMachine_Code , AttendanceLog_FromMachine_No  ,                               for_OrderBy                              ,   AttendanceLog_FromMachine_Date ,          Sl_No        ,                  Employee_CardNo                ,                  IN_Out                          ,                    INOut_DateTime_Text            , INOut_DateTime   ) " & _
                                            "          Values                                    (   '" & Trim(NewCode) & "'      , '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",               @EntryDate         ,  " & Str(Val(Sno)) & ", '" & Trim(lvLogs.Items(i).SubItems(1).Text) & "',  '" & Trim(lvLogs.Items(i).SubItems(3).Text) & "',   '" & Trim(lvLogs.Items(i).SubItems(4).Text) & "',  @AttenDateTime  ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next
            Else

                MessageBox.Show("Log details Not Found..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                tr.Rollback()
                Exit Sub
            End If



            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If

            Else
                move_record(lbl_RefNo.Text)

            End If

            Save_Sts = True

            Save_FileFrom_Settings()

        Catch ex As Exception

            tr.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), LCase("ix_payroll_attendancelog_frommachine_details")) > 0 Then
                MessageBox.Show("Duplicate In out Timing", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), LCase("ix_payroll_attendancelog_frommachine_head")) > 0 Then
                MessageBox.Show("Duplicate Log Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else

                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If


        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()



        End Try

    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        'If txtIP.Text.Trim() = "" Or txtPort.Text.Trim() = "" Then
        '    MsgBox("IP and Port cannot be null", MsgBoxStyle.Exclamation, "Error")
        '    Return
        'End If
        'Dim idwErrorCode As Integer
        'Cursor = Cursors.WaitCursor
        'If btnConnect.Text = "Disconnect" Then
        '    axCZKEM1.Disconnect()
        '    bIsConnected = False
        '    btnConnect.Text = "Connect"
        '    lblState.Text = "Current State:Disconnected"
        '    Cursor = Cursors.Default
        '    Return
        'End If

        'bIsConnected = axCZKEM1.Connect_Net(txtIP.Text.Trim(), Convert.ToInt32(txtPort.Text.Trim()))
        'If bIsConnected = True Then
        '    btnConnect.Text = "Disconnect"
        '    btnConnect.Refresh()
        '    lblState.Text = "Current State:Connected"
        '    iMachineNumber = 1 'In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
        '    axCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
        'Else
        '    axCZKEM1.GetLastError(idwErrorCode)
        '    MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        'End If
        'Cursor = Cursors.Default

    End Sub

    Private Sub btnRsConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRsConnect.Click
        'If cbPort.Text.Trim() = "" Or cbBaudRate.Text.Trim() = "" Or txtMachineSN.Text.Trim() = "" Then
        '    MsgBox("Port,BaudRate and MachineSN cannot be null", MsgBoxStyle.Exclamation, "Error")
        '    Return
        'End If
        'Dim idwErrorCode As Integer

        ''accept serialport number from string like "COMi"
        'Dim iPort As Integer
        ''Dim sPort = cbPort.Text.Trim()
        'Dim sPort As String = cbPort.Text.Trim()
        'For iPort = 1 To 9
        '    If sPort.IndexOf(iPort.ToString()) > -1 Then
        '        Exit For
        '    End If
        'Next

        'Cursor = Cursors.WaitCursor
        'If btnRsConnect.Text = "Disconnect" Then
        '    AxCZKEM1.Disconnect()
        '    bIsConnected = False
        '    btnRsConnect.Text = "Connect"
        '    lblState.Text = "Current State:Disconnected"
        '    Cursor = Cursors.Default
        '    Return
        'End If

        'iMachineNumber = Convert.ToInt32(txtMachineSN.Text.Trim()) '//when you are using the serial port communication,you can distinguish different devices by their serial port number.
        'bIsConnected = AxCZKEM1.Connect_Com(iPort, iMachineNumber, Convert.ToInt32(cbBaudRate.Text.Trim()))

        'If bIsConnected = True Then
        '    btnRsConnect.Text = "Disconnect"
        '    btnRsConnect.Refresh()
        '    lblState.Text = "Current State:Connected"
        '    AxCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
        'Else
        '    AxCZKEM1.GetLastError(idwErrorCode)
        '    MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        'End If
        'Cursor = Cursors.Default
    End Sub

    'If your device supports the USBCLient, you can refer to this.
    'Not all series devices can support this kind of connection.Please make sure your device supports USBClient.
    'Connect the device via the virtual serial port created by USBClient
    Private Sub btnUSBConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUSBConnect.Click
        'Dim idwErrorCode As Integer

        'Cursor = Cursors.WaitCursor
        'If btnUSBConnect.Text = "Disconnect" Then
        '    AxCZKEM1.Disconnect()
        '    bIsConnected = False
        '    btnUSBConnect.Text = "Connect"
        '    lblState.Text = "Current State:Disconnected"
        '    Cursor = Cursors.Default
        '    Return
        'End If

        'Dim sCom As String = ""
        'Dim bSearch As Boolean
        'Dim usbcom As New SearchforUSBCom 'new
        'bSearch = usbcom.SearchforCom(sCom)

        'If bSearch = False Then
        '    MsgBox("Can not find the virtual serial port that can be used", MsgBoxStyle.Exclamation, "Error")
        '    Cursor = Cursors.Default
        '    Return
        'End If

        'Dim iPort As Integer
        'For iPort = 1 To 9
        '    If sCom.IndexOf(iPort.ToString()) > -1 Then
        '        Exit For
        '    End If
        'Next

        'iMachineNumber = Convert.ToInt32(txtMachineSN2.Text.Trim())
        'If iMachineNumber = 0 Or iMachineNumber > 255 Then
        '    MsgBox("The Machine Number is invalid!", MsgBoxStyle.Exclamation, "Error")
        '    Cursor = Cursors.Default
        '    Return
        'End If

        'Dim iBaudRate = 115200 '115200 is one possible baudrate value(its value cannot be 0)
        'bIsConnected = AxCZKEM1.Connect_Com(iPort, iMachineNumber, iBaudRate)

        'If bIsConnected = True Then
        '    btnUSBConnect.Text = "Disconnect"
        '    btnUSBConnect.Refresh()
        '    lblState.Text = "Current State:Connected"
        '    AxCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want 
        'Else
        '    AxCZKEM1.GetLastError(idwErrorCode)
        '    MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        'End If
        'Cursor = Cursors.Default
    End Sub

    Private Sub btnGetGeneralLogData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetGeneralLogData.Click
        If Trim(txt_FileName.Text) <> "" Then
            btn_GenerateLogFromFle_Click(sender, e)
            Exit Sub
        Else
            If Get_LogDetails() = True Then
                Exit Sub
            End If
        End If


        If bIsConnected = False Then
            MsgBox("Please connect the device first", MsgBoxStyle.Exclamation, "Error")
            Return
        End If

        Dim sdwEnrollNumber As String = ""
        Dim idwVerifyMode As Integer = 0
        Dim idwInOutMode As Integer = 0
        Dim idwYear As Integer = 0
        Dim idwMonth As Integer = 0
        Dim idwDay As Integer = 0
        Dim idwHour As Integer = 0
        Dim idwMinute As Integer = 0
        Dim idwSecond As Integer = 0
        Dim idwWorkcode As Integer = 0

        Dim idwErrorCode As Integer = 0
        Dim iGLCount = 0
        Dim lvItem As New ListViewItem("Items", 0)
        Dim InOut_Date As String = ""
        Dim InOut_DateTime As Date
        'Dim dttm As DateTime


        InOut_DateTime = #1/1/1979#

        Cursor = Cursors.WaitCursor
        lvLogs.Items.Clear()

        'AxCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
        'If axCZKEM1.ReadGeneralLogData(iMachineNumber) Then 'read all the attendance records to the memory
        '    'get records from the memory
        '    While axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, sdwEnrollNumber, idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond, idwWorkcode)

        '        InOut_DateTime = New DateTime(Val(idwYear.ToString()), Val(idwMonth.ToString()), Val(idwDay.ToString()))
        '        'InOut_Date = idwYear.ToString() & "-" + idwMonth.ToString() & "-" & idwDay.ToString() ' & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString()
        '        'InOut_DateTime = CDate(InOut_Date)

        '        If DateDiff(DateInterval.Day, dtp_Date.Value.Date, InOut_DateTime) = 0 Then
        '            iGLCount += 1
        '            lvItem = lvLogs.Items.Add(iGLCount.ToString())
        '            lvItem.SubItems.Add(sdwEnrollNumber)
        '            lvItem.SubItems.Add(idwVerifyMode.ToString())
        '            lvItem.SubItems.Add(idwInOutMode.ToString())
        '            lvItem.SubItems.Add(idwDay.ToString() & "-" + idwMonth.ToString() & "-" & idwYear.ToString() & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString())
        '            lvItem.SubItems.Add(idwYear.ToString() & "~" + idwMonth.ToString() & "~" & idwDay.ToString() & "~" & idwHour.ToString() & "~" & idwMinute.ToString() & "~" & idwSecond.ToString())
        '            'lvItem.SubItems.Add(idwWorkcode.ToString())
        '        End If

        '    End While

        '    Cursor = Cursors.Default

        '    MsgBox("Attendance Log Completed", MsgBoxStyle.OkCancel, "SUCESSFULLY COMPLETED...")

        'Else

        '    Cursor = Cursors.Default
        '    axCZKEM1.GetLastError(idwErrorCode)
        '    If idwErrorCode <> 0 Then
        '        MsgBox("Reading data from terminal failed,ErrorCode: " & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        '    Else
        '        MsgBox("No data from terminal returns!", MsgBoxStyle.Exclamation, "Error")
        '    End If

        'End If

        'AxCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
        'Cursor = Cursors.Default
    End Sub

    'Get the count of attendance records in from ternimal.
    Private Sub btnGetDeviceStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetDeviceStatus.Click
        'If bIsConnected = False Then
        '    MsgBox("Please connect the device first", MsgBoxStyle.Exclamation, "Error")
        '    Return
        'End If
        'Dim idwErrorCode As Integer
        'Dim iValue = 0

        'AxCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
        'If AxCZKEM1.GetDeviceStatus(iMachineNumber, 6, iValue) = True Then 'Here we use the function "GetDeviceStatus" to get the record's count.The parameter "Status" is 6.
        '    MsgBox("The count of the AttLogs in the device is " + iValue.ToString(), MsgBoxStyle.Information, "Success")
        'Else
        '    AxCZKEM1.GetLastError(idwErrorCode)
        '    MsgBox("Operation failed,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        'End If

        'AxCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
    End Sub

    'Clear all attendance records from terminal.
    Private Sub btnClearGLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearGLog.Click

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "CL#3222" Then
            MessageBox.Show("Invalid Password", "CLEAR LOG FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If bIsConnected = False Then
            MsgBox("Please connect the device first", MsgBoxStyle.Exclamation, "Error")
            Return
        End If
        Dim idwErrorCode As Integer = 0

        'lvLogs.Items.Clear()
        'axCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
        'If axCZKEM1.ClearGLog(iMachineNumber) = True Then
        '    axCZKEM1.RefreshData(iMachineNumber) 'the data in the device should be refreshed
        '    MsgBox("All att Logs have been cleared from teiminal!", MsgBoxStyle.Information, "Success")
        'Else
        '    axCZKEM1.GetLastError(idwErrorCode)
        '    MsgBox("Operation failed,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        'End If

        'axCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
    End Sub

    Private Sub btn_SelectFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SelectFile.Click
        OpenFileDialog1.ShowDialog()
        txt_FileName.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub btn_GenerateLogFromFle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_GenerateLogFromFile.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim fs As FileStream
        Dim r As StreamReader
        Dim Path As String = ""
        Dim Str1 As String = ""
        Dim Arr(50) As String
        Dim Emp_Code As String = ""
        Dim Att_Dt As String = ""
        Dim Att_Tm As String = ""
        Dim Sn As Integer = 0
        Dim Cur_Date As String = Format(dtp_Date.Value, "yyyy/MM/dd")
        Dim Att_Date As String
        Dim EmpData() As String
        Dim InOutMode As Integer = 0
        Dim EmpCd_Indx As Integer
        Dim EmpMd_Indx As Integer
        Dim EmpDt_Indx As Integer
        Dim LineStart As Integer = 0
        Dim Heading_Sts As Boolean = False

        Path = Trim(txt_FileName.Text)



        If Trim(txt_FileName.Text) = "" Then
            If Get_LogDetails() = True Then
                Exit Sub
            End If
        End If


        If Trim(txt_FileName.Text) = "" Then
            MessageBox.Show("Invalid File ...", "INVALID FILE NAME", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        If File.Exists(Path) = False Then
            MessageBox.Show("Invalid File...", "INVALID FILE NAME", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        Else



            con1.Open()

            da = New SqlClient.SqlDataAdapter("select * from Settings_Head", con1)
            dt = New DataTable
            da.Fill(dt)
            EmpCd_Indx = 0
            EmpMd_Indx = 0
            EmpDt_Indx = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("FileFrom_EmpDate_indx").ToString()) = False Then
                    EmpDt_Indx = dt.Rows(0).Item("FileFrom_EmpDate_indx").ToString()
                End If
                If IsDBNull(dt.Rows(0).Item("FileFrom_EmpCode_indx").ToString()) = False Then
                    EmpCd_Indx = dt.Rows(0).Item("FileFrom_EmpCode_indx").ToString()
                End If
                If IsDBNull(dt.Rows(0).Item("FileFrom_EmpInOutMode_indx").ToString()) = False Then
                    EmpMd_Indx = dt.Rows(0).Item("FileFrom_EmpInOutMode_indx").ToString()
                End If
                If IsDBNull(dt.Rows(0).Item("FileFrom_LineStartFrom").ToString()) = False Then
                    LineStart = dt.Rows(0).Item("FileFrom_LineStartFrom").ToString()
                End If
            End If
            dt.Clear()
            da.Dispose()
            con1.Close()

            '-----Checking Previous date log details

            If Check_PreviousdateLog(Cur_Date, EmpDt_Indx, EmpCd_Indx, EmpMd_Indx, LineStart) = False Then
                MessageBox.Show("Previous date Log Details Pending...", "GET PREVIOUS DATE LOG", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Exit Sub
            End If


            'sleep(1000)
            fs = New FileStream(Path, FileMode.Open, FileAccess.Read)
            r = New StreamReader(fs)

            lvLogs.Items.Clear()

            Sn = 0


            Try
                Do
                    Str1 = ""

                    If Heading_Sts = False Then
                        If LineStart > 1 Then
                            For L = 1 To LineStart - 1 Step 1
                                Heading_Sts = True
                                Str1 = r.ReadLine             '---Skip the Heading
                            Next
                        End If
                    End If

                    Str1 = r.ReadLine


                    EmpData = Split(Str1, vbTab)

                    If Str1 = "" Then
                        Exit Sub
                    End If


                    Emp_Code = EmpData(EmpCd_Indx)
                    InOutMode = Val(EmpData(EmpMd_Indx))

                    If IsDate(EmpData(EmpDt_Indx)) = True Then
                        Att_Dt = EmpData(EmpDt_Indx)
                    End If

                    Att_Date = Format(Convert.ToDateTime(Att_Dt), "yyyy/MM/dd")

                    If Trim(Emp_Code) <> "" And Trim(Att_Dt) <> "" Then

                        Att_Date = Replace(Att_Date, "-", "/")

                        If Cur_Date = Att_Date Then
                            Sn = Sn + 1

                            Put_To_Data(Sn, Emp_Code, Att_Dt, InOutMode)

                        End If


                    Else
                        r.Close()
                        fs.Close()
                        r.Dispose()
                        fs.Dispose()

                        Exit Sub
                    End If

                Loop Until r Is Nothing

                '---
            Catch ex As Exception

                MessageBox.Show(ex.Message, "FOR OPENING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Finally
                r.Close()
                fs.Close()
                r.Dispose()
                fs.Dispose()
                'btn_save.Focus()
            End Try


        End If



    End Sub
    Private Sub Put_To_Data(ByVal Sn As Integer, ByVal Empcode As String, ByVal dt As String, ByVal InOut_Mode As Integer)
        Dim sno As Integer = 0
        Dim n As Integer = 0
        Dim lvItem As New ListViewItem("Items", 0)


        Try
            With lvLogs



                lvItem = lvLogs.Items.Add(Sn)

                lvItem.SubItems.Add(Empcode)

                lvItem.SubItems.Add("")  '--idwVerifyMode.ToString() 

                lvItem.SubItems.Add(InOut_Mode)

                lvItem.SubItems.Add(dt)

                lvItem.SubItems.Add(Year(dt) & "~" & Month(dt) & "~" & Microsoft.VisualBasic.DateAndTime.Day(dt) & "~" & Hour(dt) & "~" & Minute(dt) & "~" & Second(dt))

                lvItem.SubItems.Add("")



            End With
        Catch ex As Exception

        End Try

    End Sub

    Private Sub lbl_btn_CloseSettings_FileFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_btn_CloseSettings_FileFrom.Click
        Pnl_Back.Enabled = True
        pnl_Settings_FileFrom.Visible = False
    End Sub

    Private Sub btn_Settings_FileFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Settings_FileFrom.Click
        Pnl_Back.Enabled = False
        pnl_Settings_FileFrom.Visible = True
        Save_Sts = False
    End Sub


    Private Sub lbl_btn_CloseSettings_FileFrom_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_btn_CloseSettings_FileFrom.MouseHover
        lbl_btn_CloseSettings_FileFrom.ForeColor = Color.Red
    End Sub

    Private Sub lbl_btn_CloseSettings_FileFrom_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_btn_CloseSettings_FileFrom.MouseLeave
        lbl_btn_CloseSettings_FileFrom.ForeColor = Color.White
    End Sub

    Private Sub txt_EmpAttDate_FileFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmpAttDate_FileFrom.KeyPress


    End Sub

    Private Sub lbl_btn_CloseFileFromSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_btn_CloseFileFromSettings.Click
        Pnl_Back.Enabled = True
        pnl_Settings_FileFrom.Visible = False
    End Sub

    Private Sub lbl_btn_SaveFileFromSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_btn_SaveFileFromSettings.Click
        Save_FileFrom_Settings()
        lbl_btn_CloseFileFromSettings_Click(sender, e)
    End Sub
    Private Sub Save_FileFrom_Settings()
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim CC_Leng As Integer = 0


        con1.Open()
        trans = con1.BeginTransaction

        Try
            cmd.Connection = con1
            cmd.Transaction = trans

            cmd.CommandText = "update Settings_Head set FileFrom_EmpDate_indx = " & Trim(txt_EmpAttDate_FileFrom.Text) & " ,FileFrom_EmpCode_indx =" & Trim(txt_EmpCardNo_FileFrom.Text) & ",FileFrom_EmpInOutMode_indx =" & Trim(txt_EmpInOut_FileFrom.Text) & ", FileFrom_LineStartFrom = " & Trim(txt_LineStartFrom_FileFrom.Text) & ""
            cmd.ExecuteNonQuery()


            trans.Commit()

            If Save_Sts = True Then Exit Sub
            MessageBox.Show("Upadated Successfully", "FOR UPDATE", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES NOT Update", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Me.Close()
            ' Common_Procedures.vShowEntrance_Status_ForCC = True
            '  MDIParent1.Close()
            ' Entrance.Show()
            con1.Close()
        End Try
    End Sub

    Private Sub lbl_btn_ResetDefault_Filefrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_btn_ResetDefault_Filefrom.Click
        txt_EmpCardNo_FileFrom.Text = 2
        txt_EmpInOut_FileFrom.Text = 4
        txt_EmpAttDate_FileFrom.Text = 5
        txt_LineStartFrom_FileFrom.Text = 2

        Save_FileFrom_Settings()

        Pnl_Back.Enabled = True
        pnl_Settings_FileFrom.Visible = False
    End Sub

    Private Sub txt_LineStartFrom_FileFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LineStartFrom_FileFrom.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Pnl_Back.Enabled = True
            pnl_Settings_FileFrom.Visible = False
            btn_save.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress

        If Asc(e.KeyChar) = 13 Then
            Get_LogDetails()
        End If
    End Sub
    Private Function Get_LogDetails() As Boolean
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String
        Dim cmd As New SqlClient.SqlCommand
        Dim Dtt As Date = dtp_Date.Value.Date

        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@LogDate", dtp_Date.Value.Date)



        cmd.CommandText = "select AttendanceLog_FromMachine_No from Payroll_AttendanceLog_FromMachine_Head where AttendanceLog_FromMachine_Date = @LogDate  "
        Da = New SqlClient.SqlDataAdapter(cmd)
        Da.Fill(Dt)

        movno = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                movno = Trim(Dt.Rows(0)(0).ToString)

            End If
        End If

        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        If Val(movno) <> 0 Then

            move_record(movno)
            Return True
        Else
            new_record()
            dtp_Date.Value = Dtt
            Return False
        End If

        Get_LogDetails = True

    End Function


    Private Sub dtp_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.LostFocus

        'Get_LogDetails()
    End Sub

    Private Function Check_PreviousdateLog(ByVal Dttm As Date, ByVal Emp_dt_indx As Integer, ByVal Emp_Cd_indx As Integer, ByVal Emp_Md_indx As Integer, ByVal Linestart_from As Integer) As Boolean
        Dim fs As FileStream
        Dim r As StreamReader
        Dim Path As String = ""
        Dim Str1 As String = ""
        Dim Heading_Sts As Boolean = False
        Dim EmpData() As String
        Dim Emp_Code As String = ""
        Dim InOutMode As String = ""
        Dim Att_Dt As String = ""
        Dim Att_Date As String
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim LOG_STS As Boolean = False
        Dim ENT_STS As Boolean = False
        Dim NEW_STS As Boolean = False
        Dim cmd As New SqlClient.SqlCommand

        Try

            Path = Trim(txt_FileName.Text)

            fs = New FileStream(Path, FileMode.Open)
            r = New StreamReader(fs)

            lvLogs.Items.Clear()


            Dttm = DateAdd(DateInterval.Day, -1, Dttm)

            Do
                Str1 = ""

                If Heading_Sts = False Then
                    If Linestart_from > 1 Then
                        For L = 1 To Linestart_from - 1 Step 1
                            Heading_Sts = True
                            Str1 = r.ReadLine             '---Skip the Heading
                        Next
                    End If
                End If

                Str1 = r.ReadLine

                EmpData = Split(Str1, vbTab)

                If Str1 = "" Then
                    'r.Close()
                    'fs.Close()
                    'r.Dispose()
                    'fs.Dispose()

                    GoTo LOOP1
                End If


                Emp_Code = EmpData(Emp_Cd_indx)
                InOutMode = Val(EmpData(Emp_Md_indx))

                If IsDate(EmpData(Emp_dt_indx)) = True Then
                    Att_Dt = EmpData(Emp_dt_indx)
                End If

                Att_Date = Format(Convert.ToDateTime(Att_Dt), "yyyy/MM/dd")

                If Trim(Emp_Code) <> "" And Trim(Att_Dt) <> "" Then

                    Att_Date = Replace(Att_Date, "-", "/")

                    If Dttm = Att_Date Then

                        LOG_STS = True

                        'r.Close()
                        'fs.Close()
                        'r.Dispose()
                        'fs.Dispose()

                        GoTo LOOP1

                    End If

                Else
                    'r.Close()
                    'fs.Close()
                    'r.Dispose()
                    'fs.Dispose()

                End If

            Loop Until r Is Nothing

LOOP1:

            r.Close()
            fs.Close()
            r.Dispose()
            fs.Dispose()

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDateTime", Dttm)

            ''--Checking Record is found or not
            cmd.CommandText = "select *  from Payroll_AttendanceLog_FromMachine_Head a  where  AttendanceLog_FromMachine_Date = @EntryDateTime "
            da1 = New SqlClient.SqlDataAdapter(cmd)
            dt3 = New DataTable
            da1.Fill(dt3)

            If dt3.Rows.Count > 0 Then

                ENT_STS = True

            End If
            dt3.Clear()
            dt3.Dispose()
            da1.Dispose()

            ''--Checking Empty Record or Not
            cmd.CommandText = "select *  from Payroll_AttendanceLog_FromMachine_Head a  "
            da1 = New SqlClient.SqlDataAdapter(cmd)
            dt3 = New DataTable
            da1.Fill(dt3)

            NEW_STS = True
            If dt3.Rows.Count > 0 Then

                NEW_STS = False

            End If
            dt3.Clear()
            dt3.Dispose()
            da1.Dispose()

            If NEW_STS = False Then
                If LOG_STS = True And ENT_STS = False Then
                    Return False
                    Exit Function

                ElseIf LOG_STS = False And ENT_STS = False Then
                    Return True
                    Exit Function

                End If

            Else

                Return True
            End If

        Catch ex As Exception
            '----

        Finally

        End Try

        Check_PreviousdateLog = True

    End Function
End Class