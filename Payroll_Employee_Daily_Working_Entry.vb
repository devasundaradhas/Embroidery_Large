Public Class Payroll_Employee_Daily_Working_Entry
    Implements Interface_MDIActions


    Private FnYrCode As String = ""
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private FrmLdSTS As Boolean = False
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private Sub clear()

        pnl_Back.Enabled = True
        New_Entry = False

        msk_StartTime_emp.Text = ""
        msk_EndTime_emp.Text = ""
        rtxt_Work_Description.Text = ""
        dtp_Date.Text = ""
        lbl_RefNo.Text = ""
        cbo_Employee_Selection.Text = ""

    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is RichTextBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen ' Color.MistyRose ' Color.lime
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
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Or TypeOf Prec_ActCtrl Is RichTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Me.ActiveControl Is RichTextBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
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


    Private Sub msk_EndTime_emp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_EndTime_emp.GotFocus
        msk_EndTime_emp.SelectAll()
    End Sub

    Private Sub msk_StartTime_emp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_StartTime_emp.GotFocus
        msk_StartTime_emp.SelectAll()
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Employee_Selection.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Employee_Selection.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Employee_Selection, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_Employee_Selection.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            dtp_Date.Focus()
        End If

        If (e.KeyValue = 40 And cbo_Employee_Selection.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            msk_StartTime_emp.Focus()
        End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Employee_Selection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Employee_Selection, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            msk_StartTime_emp.Focus()
        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Employee_Selection.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Employee_Selection.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Payroll_Employee_Daily_Working_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Employee_Selection.Name)) And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Employee_Selection.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            FrmLdSTS = True
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
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False


    End Sub

    Private Sub Payroll_Employee_Daily_Working_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close ?", "FOR CLOSE....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Payroll_Employee_Daily_Working_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        con.Open()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_StartTime_emp.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_EndTime_emp.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Employee_Selection.GotFocus, AddressOf ControlGotFocus
        AddHandler rtxt_Work_Description.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_StartTime_emp.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_EndTime_emp.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Employee_Selection.LostFocus, AddressOf ControlLostFocus
        AddHandler rtxt_Work_Description.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_StartTime_emp.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_EndTime_emp.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_Ledger.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler rtxt_Work_Description.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_StartTime_emp.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_EndTime_emp.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_Ledger.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler rtxt_Work_Description.KeyPress, AddressOf TextBoxControlKeyPress



        new_record()

    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        con.Close()
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub


    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim Emp_id As Integer = 0
        Dim DbtAc_id As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""

        Dim IODtTm As Date
        Dim DtTm1 As Date

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        Emp_id = Common_Procedures.Employee_NameToIdNo(con, cbo_Employee_Selection.Text)

        If Emp_id = 0 Then
            MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Employee_Selection.Enabled Then cbo_Employee_Selection.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction


        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Daily_Working_Head", "Reference_CODE", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            cmd.Connection = con
            cmd.Transaction = tr



            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EmpDate", dtp_Date.Value.Date)

            DtTm1 = Convert.ToDateTime(msk_StartTime_emp.Text)
            IODtTm = New Date(Year(dtp_Date.Value.Date), Month(dtp_Date.Value.Date), Microsoft.VisualBasic.Day(dtp_Date.Value.Date), Hour(DtTm1), Minute(DtTm1), 0)
            cmd.Parameters.AddWithValue("@StartTime", IODtTm)

            DtTm1 = Convert.ToDateTime(msk_EndTime_emp.Text)
            IODtTm = New Date(Year(dtp_Date.Value.Date), Month(dtp_Date.Value.Date), Microsoft.VisualBasic.Day(dtp_Date.Value.Date), Hour(DtTm1), Minute(DtTm1), 0)
            cmd.Parameters.AddWithValue("@EndTime", IODtTm)


            Emp_id = Common_Procedures.Employee_NameToIdNo(con, cbo_Employee_Selection.Text, tr)
            If New_Entry = True Then


                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Daily_Working_Head", "Reference_Code", "For_OrderBy", "", Val(lbl_Company.Tag), FnYrCode, tr)

                cmd.CommandText = "Insert into PayRoll_Employee_Daily_Working_Head(Reference_Code,Reference_No,Reference_Date, Employee_IdNo, Start_Time,Start_Time_Text, End_Time,End_Time_Text,Work_Description,For_OrderBy ,Company_IdNo) Values ('" & Trim(NewCode) & "','" & Trim(lbl_RefNo.Text) & "',@EmpDate ," & Val(Emp_id) & ",@StartTime,'" & Trim(msk_StartTime_emp.Text) & "',@EndTime,'" & Trim(msk_EndTime_emp.Text) & "', '" & Trim(rtxt_Work_Description.Text) & "'," & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & "," & Val(lbl_Company.Tag) & " )"

                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update  PayRoll_Employee_Daily_Working_Head set Reference_Date =@EmpDate, Start_Time=@StartTime,Start_Time_Text='" & Trim(msk_StartTime_emp.Text) & "', End_Time=@EndTime, End_Time_Text='" & Trim(msk_EndTime_emp.Text) & "', Work_Description = '" & Trim(rtxt_Work_Description.Text) & "'  where Employee_IdNo =" & Val(Emp_id) & "  "
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

      

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

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub
    Private Sub getHourFromMinitues(ByVal Time1 As String)


        If Val(Microsoft.VisualBasic.Left(Time1, 2)) >= 24 Then MessageBox.Show("Time should not greater than 24", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        If Val(Microsoft.VisualBasic.Right(Time1, 2)) >= 60 Then MessageBox.Show("Minutes should not greater than 60", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "delete from PayRoll_Employee_Daily_Working_Head Where  Reference_Code = '" & Trim(NewCode) & "' "

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Reference_no  from PayRoll_Employee_Daily_Working_Head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Reference_No", con)
            
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
         
            da = New SqlClient.SqlDataAdapter("select top 1 Reference_no from PayRoll_Employee_Daily_Working_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Reference_No desc", con)

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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer
        Dim OrdByNo As Integer

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Reference_no from PayRoll_Employee_Daily_Working_Head Where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Reference_No", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer
        Dim OrdByNo As Integer
        Try
            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1  Reference_no from PayRoll_Employee_Daily_Working_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Reference_No desc", con)
            da.Fill(dt)



            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        clear()

        New_Entry = True
        lbl_RefNo.ForeColor = Color.Red

        lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Daily_Working_Head", "Reference_CODE", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

        lbl_RefNo.ForeColor = Color.Red


        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub
    Public Sub move_record(ByVal idno As Integer)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            'cbo_ItemName.Text = Common_Procedures.Item_IdNoToName(con, Itmidno)
            'cbo_Size.Text = Common_Procedures.Size_IdNoToName(con, sizidno)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.Reference_No, a.Reference_Date, b.Employee_Name , a.Start_Time_Text , a.End_Time_Text, a.Work_Description from PayRoll_Employee_Daily_Working_Head a LEFT OUTER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  where a.Reference_No = " & Str(Val(idno)) & "  Order by a.Reference_Date, a.For_OrderBy, a.Reference_No", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            lbl_RefNo.ForeColor = Color.Black


            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Reference_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Reference_Date").ToString
                cbo_Employee_Selection.Text = dt1.Rows(0).Item("Employee_Name").ToString
                msk_StartTime_emp.Text = dt1.Rows(0).Item("Start_Time_Text").ToString
                msk_EndTime_emp.Text = dt1.Rows(0).Item("End_Time_Text").ToString
                rtxt_Work_Description.Text = dt1.Rows(0).Item("Work_Description").ToString
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

   
    Private Sub msk_StartTime_emp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_StartTime_emp.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        getHourFromMinitues(msk_StartTime_emp.Text)

    End Sub
End Class