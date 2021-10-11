Public Class Agent_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private Sub clear()
        lbl_idno.Text = ""
        lbl_idno.ForeColor = Color.Black
        txt_Alaisname.Text = ""
        txt_Name.Text = ""
        cbo_area.Text = ""
        txt_Address1.Text = ""
        txt_address2.Text = ""
        txt_address3.Text = ""
        txt_address4.Text = ""
        txt_phoneno.Text = ""
        txt_emailid.Text = ""
        cbo_group.Text = ""
        txt_pan.Text = ""
        cbo_open.Text = ""
        Panel_back.Enabled = True
        grp_open.Visible = False
        grp_Filter.Visible = False
        New_Entry = False
    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
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

        'If Me.ActiveControl.Name <> dgv_Filter.Name Then
        '    Grid_Cell_DeSelect()
        'End If

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

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
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
    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select  a.*, b.AccountsGroup_Name,c.Area_Name from ledger_head a LEFT OUTER JOIN AccountsGroup_Head b ON a.AccountsGroup_IdNo = b.AccountsGroup_IdNo LEFT OUTER JOIN Area_Head c ON a.Area_IdNo=c.Area_IdNo where a.ledger_idno = " & Str(Val(idno)) & "   and a.Ledger_Type='AGENT'", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_idno.Text = dt.Rows(0).Item("Ledger_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Ledger_MainName").ToString
            txt_alaisname.Text = dt.Rows(0).Item("Ledger_AlaisName").ToString
            cbo_Area.Text = dt.Rows(0)("Area_Name").ToString
            cbo_group.Text = dt.Rows(0)("AccountsGroup_Name").ToString
            txt_Address1.Text = dt.Rows(0)("Ledger_Address1").ToString
            txt_Address2.Text = dt.Rows(0)("Ledger_Address2").ToString
            txt_Address3.Text = dt.Rows(0)("Ledger_Address3").ToString
            txt_Address4.Text = dt.Rows(0)("Ledger_Address4").ToString
            txt_PhoneNo.Text = dt.Rows(0)("Ledger_PhoneNo").ToString
            txt_emailid.Text = dt.Rows(0)("Ledger_Emailid").ToString
            txt_Pan.Text = dt.Rows(0)("Pan_No").ToString
          
        End If



        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim newid As Integer = 0

        clear()
        lbl_idno.ForeColor = Color.Red
        New_Entry = True

        lbl_idno.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "")

        If Val(lbl_idno.Text) < 101 Then lbl_idno.Text = 101

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()


    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Agent_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Agent_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where  Ledger_Idno = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Item_Processing_Details where DeliveryTo_StockIdNo = " & Str(Val(lbl_idno.Text)) & " or ReceivedFrom_StockIdNo = " & Str(Val(lbl_idno.Text)) & " or Delivery_PartyIdNo = " & Str(Val(lbl_idno.Text)) & " or Received_PartyIdNo = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Pavu_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_idno.Text)) & " or ReceivedFrom_Idno = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Ledger_IdNo = " & Str(Val(lbl_idno.Text)) & " or StockAt_IdNo = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Yarn_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_idno.Text)) & " or ReceivedFrom_Idno = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from AgentCommission_Processing_Details where Agent_IdNo = " & Str(Val(lbl_idno.Text)) & " or Ledger_IdNo = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con


            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_idno.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ledger_head where ledger_idno = " & Str(Val(lbl_idno.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()
            dt.Dispose()
            da.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select ledger_idno, ledger_Name from ledger_head where ledger_Type='AGENT' and ledger_idno <> 0 order by ledger_idno", con)
        Dim dt As New DataTable

        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt
        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = " IDNO"
        dgv_Filter.Columns(1).HeaderText = "AGENT NAME"

        dgv_Filter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 30
        dgv_Filter.Columns(1).FillWeight = 165

        grp_Filter.Visible = True
        grp_Filter.BringToFront()
        dgv_Filter.Focus()

        Panel_back.Enabled = False

        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(Ledger_IdNo) from Ledger_Head WHERE Ledger_Type = 'AGENT' and Ledger_IdNo<>0"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno) from ledger_head Where Ledger_Type='AGENT' and ledger_idno<>0"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand("select min(ledger_idno) from ledger_head where Ledger_Type = 'AGENT'and ledger_idno<>0 and ledger_idno > " & Str(Val(lbl_idno.Text)), con)
        Dim movid As Integer = 0

        Try
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno ) from ledger_head where Ledger_Type='AGENT'and ledger_idno<>0 and ledger_idno < " & Str((lbl_idno.Text)) & ""
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt)
        cbo_open.DataSource = dt
        cbo_open.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_open.Visible = True
        grp_open.BringToFront()
        If cbo_open.Enabled And cbo_open.Visible Then cbo_open.Focus()
        Panel_back.Enabled = False


    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '    'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '    'MessageBox.Show("Ledger creation  -  print")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim nr As Long = 0
        Dim acgrp_idno As Integer = 0
        Dim ar_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        Dim Sno As Integer = 0

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Agent_Creation, New_Entry) = False Then Exit Sub

        If Panel_back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        acgrp_idno = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_group.Text)
        If acgrp_idno = 0 Then
            MessageBox.Show("Invalid Accounts Group", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_group.Enabled Then cbo_group.Focus()
            Exit Sub
        End If

        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)
        ar_idno = Common_Procedures.Area_NameToIdNo(con, cbo_Area.Text)

        LedName = Trim(txt_Name.Text)
        If Val(ar_idno) <> 0 Then
            LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
        End If

        SurName = Common_Procedures.Remove_NonCharacters(LedName)


        trans = con.BeginTransaction

        Try
            cmd.Transaction = trans

            cmd.Connection = con

            If New_Entry = True Then
                lbl_idno.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", trans)
                If Val(lbl_idno.Text) < 101 Then lbl_idno.Text = 101

                cmd.CommandText = "Insert into ledger_head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName,Ledger_AlaisName,Area_IdNo, AccountsGroup_IdNo, Parent_Code, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_PhoneNo,  Ledger_Type,Ledger_Emailid ,Pan_No ) Values (" & Str(Val(lbl_idno.Text)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "','" & Trim(txt_Alaisname.Text) & "'," & Str(Val(ar_idno)) & ", " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_address2.Text) & "', '" & Trim(txt_address3.Text) & "', '" & Trim(txt_address4.Text) & "', '" & Trim(txt_phoneno.Text) & "', 'AGENT','" & Trim(txt_emailid.Text) & "','" & Trim(txt_pan.Text) & "')"

            Else
                cmd.CommandText = "Update ledger_head set Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Ledger_MainName = '" & Trim(txt_Name.Text) & "',Ledger_Alaisname = '" & Trim(txt_Alaisname.Text) & "', Area_IdNo = " & Str(Val(ar_idno)) & ", AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & ", Parent_Code = '" & Trim(Parnt_CD) & "',  Ledger_Address1 = '" & Trim(txt_Address1.Text) & "', Ledger_Address2 = '" & Trim(txt_address2.Text) & "', Ledger_Address3 = '" & Trim(txt_address3.Text) & "', Ledger_Address4 = '" & Trim(txt_address4.Text) & "', Ledger_Type = 'AGENT' , Ledger_PhoneNo = '" & Trim(txt_phoneno.Text) & "',Ledger_Emailid = '" & Trim(txt_emailid.Text) & "',Pan_No = '" & Trim(txt_pan.Text) & "' where Ledger_IdNo = " & Str(Val(lbl_idno.Text))

            End If

            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_idno.Text))
            cmd.ExecuteNonQuery()

            LedArName = Trim(txt_Name.Text)
            If Val(ar_idno) <> 0 Then
                LedArName = Trim(txt_Name.Text) & " (" & Trim(cbo_area.Text) & ")"
            End If

            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type ) Values (" & Str(Val(lbl_idno.Text)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", 'AGENT')"
            cmd.ExecuteNonQuery()

            If Trim(txt_Alaisname.Text) <> "" Then
                LedArName = Trim(txt_Alaisname.Text)
                If Val(ar_idno) <> 0 Then
                    LedArName = Trim(txt_Alaisname.Text) & " (" & Trim(cbo_area.Text) & ")"
                End If

                cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type ) Values (" & Str(Val(lbl_idno.Text)) & ", 2, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", 'AGENT')"
                cmd.ExecuteNonQuery()

            End If
            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "AGENT"

            If New_Entry = True Then new_record()

            MessageBox.Show("Sucessfully Saved", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_ledger_head") > 0 Then
                MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_alaishead") > 0 Then
                MessageBox.Show("Duplicate Ledger Alais Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Exit Sub

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub Ledger_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs)
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
    End Sub

    Private Sub Agent_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        con.Open()

        'da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head Order by AccountsGroup_Name", con)
        'da.Fill(dt1)
        'cbo_group.Items.Clear()
        'cbo_group.DataSource = dt1
        'cbo_group.DisplayMember = "AccountsGroup_Name"

        'da = New SqlClient.SqlDataAdapter("select Area_Name from Area_Head Order by Area_Name", con)
        'da.Fill(dt2)
        'cbo_Area.Items.Clear()
        'cbo_Area.DataSource = dt2
        'cbo_Area.DisplayMember = "Area_Name"

        da.Dispose()

        grp_open.Visible = False
        grp_open.Left = (Me.Width - grp_open.Width) - 50
        grp_open.Top = (Me.Height - grp_open.Height) - 50

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 30
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 50



        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Alaisname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_area.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_group.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_open.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_phoneno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emailid.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_pan.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Alaisname.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_area.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_group.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_open.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_phoneno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emailid.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_pan.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Alaisname.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_phoneno.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emailid.KeyDown, AddressOf TextBoxControlKeyDown

        ' AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Alaisname.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_phoneno.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emailid.KeyPress, AddressOf TextBoxControlKeyPress

        new_record()

    End Sub

    Private Sub Agent_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Agent_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            ElseIf grp_open.Visible Then
                btn_find_close_Click(sender, e)
            Else
                Me.Close()
            End If

        End If
    End Sub

    Private Sub btn_find_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Panel_back.Enabled = True
        grp_open.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim cmd As New SqlClient.SqlCommand

        Dim movid As Integer


        movid = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_open.Text)



        If movid <> 0 Then move_record(movid)

        Panel_back.Enabled = True
        grp_open.Visible = False
        grp_Filter.Visible = False
    End Sub

    Private Sub cbo_open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

   Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_open, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_open, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            Call btn_Find_Click(sender, e)
        End If
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        Panel_back.Enabled = True
        grp_Filter.Visible = False
    End Sub



    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_OpenFilter_Click(sender, e)
    End Sub


    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        Dim K As Integer

        If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
            K = Asc(e.KeyChar)
            K = K - 32
            e.KeyChar = Chr(K)
        End If

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_group_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_group.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "AccountsGroup_Head", "AccountsGroup_Name", "", "")

    End Sub


    Private Sub cbo_Group_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_group.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_group, cbo_area, txt_Address1, "AccountsGroup_Head", "AccountsGroup_Name", "", "")


    End Sub

    Private Sub cbo_group_Keypress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_group.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_group, txt_Address1, "AccountsGroup_Head", "AccountsGroup_Name", "", "")

    End Sub

    Private Sub cbo_area_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_area.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Area_Head", "Area_Name", "", "")

    End Sub


    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_area.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_area, txt_Alaisname, cbo_group, "Area_Head", "Area_Name", "", "")
    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_area.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_area, cbo_group, "Area_Head", "Area_Name", "", "")

    End Sub

    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_area.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_area.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click


        Dim movid As Integer

        movid = 0
        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            btn_CloseFilter_Click(sender, e)

        End If
    End Sub

    Private Sub Agent_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

    End Sub

    Private Sub Agent_Creation_FormClosed1(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub


    Private Sub Agent_Creation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = Keys.OemQuotes Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub


    Private Sub txt_pan_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_pan.KeyDown
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
        If e.KeyValue = 38 Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_pan_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_pan.KeyPress
         If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If

        End If
    End Sub

    Private Sub Agent_Creation_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            ElseIf grp_open.Visible Then
                btn_find_close_Click(sender, e)
            Else
                Me.Close()
            End If

        End If
    End Sub

    Private Sub Agent_Creation_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        con.Open()

        'da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head Order by AccountsGroup_Name", con)
        'da.Fill(dt1)
        'cbo_group.Items.Clear()
        'cbo_group.DataSource = dt1
        'cbo_group.DisplayMember = "AccountsGroup_Name"

        'da = New SqlClient.SqlDataAdapter("select Area_Name from Area_Head Order by Area_Name", con)
        'da.Fill(dt2)
        'cbo_Area.Items.Clear()
        'cbo_Area.DataSource = dt2
        'cbo_Area.DisplayMember = "Area_Name"

        da.Dispose()

        grp_open.Visible = False
        grp_open.Left = (Me.Width - grp_open.Width) - 50
        grp_open.Top = (Me.Height - grp_open.Height) - 50

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 30
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 50



        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Alaisname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_area.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_group.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_open.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_phoneno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emailid.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_pan.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Alaisname.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_area.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_group.LostFocus, AddressOf ControlLostFocus
         AddHandler cbo_open.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_phoneno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emailid.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_pan.LostFocus, AddressOf ControlLostFocus
       
        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Alaisname.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_phoneno.KeyDown, AddressOf TextBoxControlKeyDown
         AddHandler txt_emailid.KeyDown, AddressOf TextBoxControlKeyDown
     
        ' AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Alaisname.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_phoneno.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emailid.KeyPress, AddressOf TextBoxControlKeyPress
       
        new_record()
    End Sub

 
End Class