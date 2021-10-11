Public Class Cheque_Print_Positioning
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private Sub clear()
        'Me.Height = 335  ' 327
        pnl_Back.Enabled = True
        grp_find.Visible = False
        'grp_Filter.Visible = False
        lbl_ChqNo.Text = ""
        lbl_ChqNo.ForeColor = Color.Black
        cbo_BankName.Text = ""
        txt_AccountNo.Text = ""
        cbo_Partner.Text = ""
        Cbo_PaperOrientation.Text = "P_PORTRAIT"
        txt_LeftMargin.Text = ""
        txt_TopMargin.Text = ""
        txt_AcPayee_Left.Text = ""
        txt_AcPayee_Top.Text = ""
        txt_AcPayeeWidth.Text = ""
        txt_AmountWords_Left.Text = ""
        txt_Date_Left.Text = ""
        txt_Date_Top.Text = ""
        txt_Date_width.Text = ""
        txt_PartyName_Left.Text = ""
        txt_PartyName_Top.Text = ""
        txt_PartyName_width.Text = ""
        txt_second_PartyName_Left.Text = ""
        txt_Second_PartyName_Top.Text = ""
        txt_Second_PartyName_Width.Text = ""
        txt_AmountWords_Left.Text = ""
        txt_AmountWords_Top.Text = ""
        txt_AmountWords_Width.Text = ""
        txt_Second_AmountWords_Left.Text = ""
        txt_Second_AmountWords_Top.Text = ""
        txt_Second_AmountWords_Width.Text = ""
        txt_Rs_Left.Text = ""
        txt_Rs_Top.Text = ""
        txt_Rs_Width.Text = ""
        txt_CompanyName_Left.Text = ""
        txt_CompanyName_Top.Text = ""
        txt_CompanyName_Width.Text = ""
        txt_Partner_Left.Text = ""
        txt_Partner_Top.Text = ""
        txt_Partner_Width.Text = ""
        txt_AccountNo_Left.Text = ""
        txt_AccountNo_Top.Text = ""
        txt_AccountNo_Width.Text = ""


        New_Entry = False
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
    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Count_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Count_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try



            cmd.Connection = con
            cmd.CommandText = "delete from Cheque_Print_Positioning_Head where Cheque_Print_Positioning_No = " & Str(Val(lbl_ChqNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If cbo_BankName.Enabled And cbo_BankName.Visible Then cbo_BankName.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        'Dim da As New SqlClient.SqlDataAdapter("select Cheque_Print_Positioning_No, Count_Name,Count_Description from Cheque_Print_Positioning_Head where Cheque_Print_Positioning_No <> 0 order by Cheque_Print_Positioning_No", con)
        'Dim dt As New DataTable

        'da.Fill(dt)

        'With dgv_Filter

        '    .Columns.Clear()
        '    .DataSource = dt

        '    .RowHeadersVisible = False

        '    .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        '    .Columns(0).HeaderText = "IDNO"
        '    .Columns(1).HeaderText = "NAME"
        '    .Columns(2).HeaderText = "DESCRIPTION"


        '    .Columns(0).FillWeight = 60
        '    .Columns(1).FillWeight = 160
        '    .Columns(2).FillWeight = 300


        'End With

        'new_record()

        'grp_Filter.Visible = True

        'pnl_Back.Enabled = False

        'If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        'Me.Height = 520   '    514

        'da.Dispose()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
       
        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_name as Bank_Name from Cheque_Print_Positioning_Head a INNER JOIN Ledger_head b ON a.Ledger_idNo = b.Ledger_IdNo where a.Cheque_Print_Positioning_No = " & Str(Val(idno)), con)

        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_ChqNo.Text = dt.Rows(0).Item("Cheque_Print_Positioning_No").ToString
            cbo_BankName.Text = dt.Rows(0).Item("Bank_Name").ToString
            cbo_Partner.Text = dt.Rows(0).Item("Partner").ToString
            Cbo_PaperOrientation.Text = dt.Rows(0).Item("Paper_Orientation").ToString
            txt_LeftMargin.Text = Format(Val(dt.Rows(0).Item("Left_Margin").ToString), "########0.00")
            txt_TopMargin.Text = Format(Val(dt.Rows(0).Item("Top_Margin").ToString), "########0.00")
            txt_AccountNo.Text = dt.Rows(0).Item("Account_No").ToString
            txt_AcPayee_Left.Text = Format(Val(dt.Rows(0).Item("Ac_PAyee_Left").ToString), "########0.00")
            txt_AcPayee_Top.Text = Format(Val(dt.Rows(0).Item("Ac_Payee_Top").ToString), "########0.00")
            txt_AcPayeeWidth.Text = Format(Val(dt.Rows(0).Item("Ac_Payee_width").ToString), "########0.00")
            txt_Date_Left.Text = Format(Val(dt.Rows(0).Item("Date_Left").ToString), "########0.00")
            txt_Date_Top.Text = Format(Val(dt.Rows(0).Item("Date_Top").ToString), "########0.00")
            txt_Date_width.Text = Format(Val(dt.Rows(0).Item("Date_Width").ToString), "########0.00")
            txt_PartyName_Left.Text = Format(Val(dt.Rows(0).Item("PartyName_Left").ToString), "########0.00")
            txt_PartyName_Top.Text = Format(Val(dt.Rows(0).Item("PartyName_Top").ToString), "########0.00")
            txt_PartyName_width.Text = Format(Val(dt.Rows(0).Item("PartyName_Width").ToString), "########0.00")
            txt_second_PartyName_Left.Text = Format(Val(dt.Rows(0).Item("Second_PartyName_left").ToString), "########0.00")
            txt_Second_PartyName_Top.Text = Format(Val(dt.Rows(0).Item("Second_PartyName_Top").ToString), "########0.00")
            txt_Second_PartyName_Width.Text = Format(Val(dt.Rows(0).Item("Second_partyName_Width").ToString), "########0.00")
            txt_AmountWords_Left.Text = Format(Val(dt.Rows(0).Item("AmountWords_left").ToString), "########0.00")
            txt_AmountWords_Top.Text = Format(Val(dt.Rows(0).Item("AmountWords_Top").ToString), "########0.00")
            txt_AmountWords_Width.Text = Format(Val(dt.Rows(0).Item("AmountWords_Width").ToString), "########0.00")
            txt_Second_AmountWords_Left.Text = Format(Val(dt.Rows(0).Item("second_AmountWords_Left").ToString), "########0.00")
            txt_Second_AmountWords_Top.Text = Format(Val(dt.Rows(0).Item("Second_AmountWords_Top").ToString), "########0.00")
            txt_Second_AmountWords_Width.Text = Format(Val(dt.Rows(0).Item("Second_AmountWords_Width").ToString), "########0.00")
            txt_Rs_Left.Text = Format(Val(dt.Rows(0).Item("Rupees_Left").ToString), "########0.00")
            txt_Rs_Top.Text = Format(Val(dt.Rows(0).Item("Rupees_Top").ToString), "########0.00")
            txt_Rs_Width.Text = Format(Val(dt.Rows(0).Item("Rupees_Width").ToString), "########0.00")
            txt_CompanyName_Left.Text = Format(Val(dt.Rows(0).Item("CompanyName_Left").ToString), "########0.00")
            txt_CompanyName_Top.Text = Format(Val(dt.Rows(0).Item("CompanyName_Top").ToString), "########0.00")
            txt_CompanyName_Width.Text = Format(Val(dt.Rows(0).Item("CompanyName_Width").ToString), "########0.00")
            txt_Partner_Left.Text = Format(Val(dt.Rows(0).Item("Partner_Left").ToString), "########0.00")
            txt_Partner_Top.Text = Format(Val(dt.Rows(0).Item("Partner_Top").ToString), "########0.00")
            txt_Partner_Width.Text = Format(Val(dt.Rows(0).Item("Partner_Width").ToString), "########0.00")
            txt_AccountNo_Left.Text = Format(Val(dt.Rows(0).Item("AccountNo_Left").ToString), "########0.00")
            txt_AccountNo_Top.Text = Format(Val(dt.Rows(0).Item("AccountNo_Top").ToString), "########0.00")
            txt_AccountNo_Width.Text = Format(Val(dt.Rows(0).Item("AccountNo_Width").ToString), "########0.00")

        End If








        dt.Dispose()
        da.Dispose()

        If cbo_BankName.Enabled And cbo_BankName.Visible Then cbo_BankName.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Cheque_Print_Positioning_No) from Cheque_Print_Positioning_Head Where Cheque_Print_Positioning_No <> 0", con)
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

        'Try
        da = New SqlClient.SqlDataAdapter("select max(Cheque_Print_Positioning_No) from Cheque_Print_Positioning_Head Where Cheque_Print_Positioning_No <> 0", con)
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

        'Catch ex As Exception
        'MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Cheque_Print_Positioning_No) from Cheque_Print_Positioning_Head Where Cheque_Print_Positioning_No > " & Str(Val(lbl_ChqNo.Text)) & " and Cheque_Print_Positioning_No <> 0", con)
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

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(Cheque_Print_Positioning_No) from Cheque_Print_Positioning_Head Where Cheque_Print_Positioning_No < " & Str(Val(lbl_ChqNo.Text)) & " and Cheque_Print_Positioning_No <> 0", con)
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

    Public Sub new_record() Implements Interface_MDIActions.new_record
        clear()

        New_Entry = True
        lbl_ChqNo.ForeColor = Color.Red

        lbl_ChqNo.Text = Common_Procedures.get_MaxIdNo(con, "Cheque_Print_Positioning_Head", "Cheque_Print_Positioning_No", "")

        If cbo_BankName.Enabled And cbo_BankName.Visible Then cbo_BankName.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        'Dim da As New SqlClient.SqlDataAdapter("select Count_Name from Cheque_Print_Positioning_Head order by Count_Name", con)
        'Dim dt As New DataTable

        'da.Fill(dt)

        'cbo_Find.DataSource = dt
        'cbo_Find.DisplayMember = "Count_Name"

        'new_record()

        'Me.Height = 520   ' 513
        'grp_find.Visible = True
        'pnl_Back.Enabled = False
        'If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String = ""
        Dim Bank_id As Integer

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Count_Creation, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Bank_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_BankName.Text)
        If Bank_id = 0 Then
            MessageBox.Show("Invalid Bank Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_BankName.Enabled And cbo_BankName.Visible Then cbo_BankName.Focus()
            Exit Sub
        End If


        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_ChqNo.Text = Common_Procedures.get_MaxIdNo(con, "Cheque_Print_Positioning_Head", "Cheque_Print_Positioning_No", "", trans)

                cmd.CommandText = "Insert into Cheque_Print_Positioning_Head(Cheque_Print_Positioning_No, Ledger_IdNo,Partner, Paper_Orientation,Left_Margin,Top_Margin,Account_No,Ac_Payee_Left,Ac_Payee_Top,Ac_Payee_Width,Date_Left,Date_Top,Date_width,PartyName_Left,PartyName_Top,PartyName_Width,second_PartyName_Left,Second_PartyName_Top,Second_PartyName_Width,AmountWords_Left,AmountWords_Top,AmountWords_Width,Second_AmountWords_Left,Second_AmountWords_Top,Second_AmountWords_Width,Rupees_Left,Rupees_Top,Rupees_Width,CompanyName_Left,CompanyName_Top,CompanyName_Width,Partner_Left,Partner_Top,Partner_Width,AccountNo_Left,AccountNo_Top,AccountNo_Width) values (" & Str(Val(lbl_ChqNo.Text)) & ", " & Val(Bank_id) & ", '" & Trim(cbo_Partner.Text) & "','" & Trim(Cbo_PaperOrientation.Text) & "' ," & Val(txt_LeftMargin.Text) & ", " & Val(txt_TopMargin.Text) & " , '" & Trim(txt_AccountNo.Text) & "', " & Val(txt_AcPayee_Left.Text) & " ," & Val(txt_AcPayee_Top.Text) & " ," & Val(txt_AcPayeeWidth.Text) & " , " & Val(txt_Date_Left.Text) & " ," & Val(txt_Date_Top.Text) & " ," & Val(txt_Date_width.Text) & ", " & Val(txt_PartyName_Left.Text) & "," & Val(txt_PartyName_Top.Text) & " , " & Val(txt_PartyName_width.Text) & " , " & Val(txt_second_PartyName_Left.Text) & "," & Val(txt_Second_PartyName_Top.Text) & "," & Val(txt_Second_PartyName_Width.Text) & ", " & Val(txt_AmountWords_Left.Text) & " , " & Val(txt_AmountWords_Top.Text) & " , " & Val(txt_AmountWords_Width.Text) & ", " & Val(txt_Second_AmountWords_Left.Text) & " , " & Val(txt_Second_AmountWords_Top.Text) & " , " & Val(txt_Second_AmountWords_Width.Text) & ", " & Val(txt_Rs_Left.Text) & " ," & Val(txt_Rs_Top.Text) & " ," & Val(txt_Rs_Width.Text) & " , " & Val(txt_CompanyName_Left.Text) & " ," & Val(txt_CompanyName_Top.Text) & " ," & Val(txt_CompanyName_Width.Text) & " , " & Val(txt_Partner_Left.Text) & " , " & Val(txt_Partner_Top.Text) & " , " & Val(txt_Partner_Width.Text) & " , " & Val(txt_AccountNo_Left.Text) & " , " & Val(txt_AccountNo_Top.Text) & ", " & Val(txt_AccountNo_Width.Text) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Cheque_Print_Positioning_Head set Ledger_IdNo = " & Val(Bank_id) & ",Partner = '" & Trim(cbo_Partner.Text) & "',Paper_Orientation = '" & Trim(Cbo_PaperOrientation.Text) & "' ,Left_Margin = " & Val(txt_LeftMargin.Text) & ",Top_Margin = " & Val(txt_TopMargin.Text) & " ,Account_No = '" & Trim(txt_AccountNo.Text) & "',Ac_Payee_Left = " & Val(txt_AcPayee_Left.Text) & " ,Ac_Payee_Top = " & Val(txt_AcPayee_Top.Text) & " ,Ac_Payee_Width = " & Val(txt_AcPayeeWidth.Text) & " ,Date_Left = " & Val(txt_Date_Left.Text) & " ,Date_Top = " & Val(txt_Date_Top.Text) & " ,Date_width = " & Val(txt_Date_width.Text) & ",PartyName_Left = " & Val(txt_PartyName_Left.Text) & ",PartyName_Top = " & Val(txt_PartyName_Top.Text) & " ,PartyName_Width = " & Val(txt_PartyName_width.Text) & " ,second_PartyName_Left = " & Val(txt_second_PartyName_Left.Text) & ",Second_PartyName_Top = " & Val(txt_Second_PartyName_Top.Text) & ",Second_PartyName_Width = " & Val(txt_Second_PartyName_Width.Text) & ", AmountWords_Left = " & Val(txt_AmountWords_Left.Text) & " ,AmountWords_Top = " & Val(txt_AmountWords_Top.Text) & " ,AmountWords_Width = " & Val(txt_AmountWords_Width.Text) & ",Second_AmountWords_Left = " & Val(txt_Second_AmountWords_Left.Text) & " ,Second_AmountWords_Top = " & Val(txt_Second_AmountWords_Top.Text) & " ,Second_AmountWords_Width = " & Val(txt_Second_AmountWords_Width.Text) & ",Rupees_Left = " & Val(txt_Rs_Left.Text) & " ,Rupees_Top = " & Val(txt_Rs_Top.Text) & " ,Rupees_Width = " & Val(txt_Rs_Width.Text) & " ,CompanyName_Left = " & Val(txt_CompanyName_Left.Text) & " ,CompanyName_Top = " & Val(txt_CompanyName_Top.Text) & " ,CompanyName_Width = " & Val(txt_CompanyName_Width.Text) & " ,Partner_Left = " & Val(txt_Partner_Left.Text) & " ,Partner_Top = " & Val(txt_Partner_Top.Text) & " ,Partner_Width = " & Val(txt_Partner_Width.Text) & " ,AccountNo_Left = " & Val(txt_AccountNo_Left.Text) & " ,AccountNo_Top = " & Val(txt_AccountNo_Top.Text) & ",AccountNo_Width = " & Val(txt_AccountNo_Width.Text) & " where Cheque_Print_Positioning_No = " & Str(Val(lbl_ChqNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            'Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            'Common_Procedures.Master_Return.Master_Type = "Count"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_cheque_print_positioning_head") > 0 Then
                MessageBox.Show("Duplicate Bank Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If cbo_BankName.Enabled And cbo_BankName.Visible Then cbo_BankName.Focus()


        End Try
    End Sub

    Private Sub Cheque_Print_Positioning_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BankName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_BankName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If


            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
    End Sub

    Private Sub Cheque_Print_Positioning_creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub LotNo_creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            'If grp_Filter.Visible Then
            '    btn_FilterClose_Click(sender, e)
            If grp_find.Visible Then
                btn_FindClose_Click(sender, e)
            Else
                Me.Close()
            End If

        End If
    End Sub


    Private Sub LotNo_creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        If Common_Procedures.UserRight_Check_1(Me.Name, Common_Procedures.OperationType.Open) = False Then
            MsgBox("This User Is Restircetd From Opening The Form " & Me.Text)
            Me.Close()
        End If

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        'Me.Width = 535 ' 544
        'Me.Height = 335

        grp_find.Left = 8  ' 12
        grp_find.Top = 310  '292
        grp_find.Visible = False

        'grp_Filter.Left = 8  ' 12
        'grp_Filter.Top = 310  '292
        'grp_Filter.Visible = False

        con.Open()
        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead  order by Ledger_DisplayName", con)
        da.Fill(dt)
        cbo_BankName.DataSource = dt
        cbo_BankName.DisplayMember = "Ledger_DisplayName"


        Cbo_PaperOrientation.Items.Clear()
        Cbo_PaperOrientation.Items.Add("PORTRAIT")
        Cbo_PaperOrientation.Items.Add("LANDSCAPE")

        cbo_Partner.Items.Clear()
        cbo_Partner.Items.Add("PARTNER")
        cbo_Partner.Items.Add("PROPIETOR")
        cbo_Partner.Items.Add("AUTHORISED SIGNATORY")
        cbo_Partner.Items.Add("PROPIETRIX")


        AddHandler cbo_Partner.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BankName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_PaperOrientation.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LeftMargin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TopMargin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AccountNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AcPayee_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AcPayee_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AcPayeeWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Date_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Date_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Date_width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyName_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyName_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyName_width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_second_PartyName_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Second_PartyName_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Second_PartyName_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AmountWords_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AmountWords_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AmountWords_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Second_AmountWords_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Second_AmountWords_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Second_AmountWords_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rs_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rs_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rs_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CompanyName_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CompanyName_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CompanyName_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Partner_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Partner_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Partner_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AccountNo_Left.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AccountNo_Top.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AccountNo_Width.GotFocus, AddressOf ControlGotFocus


        'AddHandler dtp_lter_Fromdate.GotFocus, AddressOf ControlGotFocus
        'AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Partner.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BankName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_PaperOrientation.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LeftMargin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TopMargin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AccountNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AcPayee_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AcPayee_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AcPayeeWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Date_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Date_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Date_width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyName_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyName_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyName_width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_second_PartyName_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Second_PartyName_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Second_PartyName_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AmountWords_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AmountWords_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AmountWords_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Second_AmountWords_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Second_AmountWords_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Second_AmountWords_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rs_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rs_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rs_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CompanyName_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CompanyName_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CompanyName_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Partner_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Partner_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Partner_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AccountNo_Left.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AccountNo_Top.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AccountNo_Width.LostFocus, AddressOf ControlLostFocus

        'AddHandler dtp_lter_Fromdate.GotFocus, AddressOf ControlGotFocus
        'AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_LeftMargin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TopMargin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AccountNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AcPayee_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AcPayee_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AcPayeeWidth.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Date_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Date_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Date_width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyName_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyName_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyName_width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_second_PartyName_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Second_PartyName_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Second_PartyName_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AmountWords_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AmountWords_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AmountWords_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Second_AmountWords_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Second_AmountWords_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Second_AmountWords_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rs_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rs_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rs_Width.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_CompanyName_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CompanyName_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CompanyName_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Partner_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Partner_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Partner_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AccountNo_Left.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AccountNo_Top.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AccountNo_Width.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_LeftMargin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TopMargin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AccountNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AcPayee_Left.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_AcPayee_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AcPayeeWidth.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Date_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Date_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Date_width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyName_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyName_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyName_width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_second_PartyName_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Second_PartyName_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Second_PartyName_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AmountWords_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AmountWords_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AmountWords_Width.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Second_AmountWords_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Second_AmountWords_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Second_AmountWords_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rs_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rs_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rs_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CompanyName_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CompanyName_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CompanyName_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Partner_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Partner_Top.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Partner_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AccountNo_Left.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AccountNo_Top.KeyPress, AddressOf TextBoxControlKeyPress






        ' Me.Top = Me.Top - 75

        new_record()

    End Sub


    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save_record()
    End Sub

    'Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click
    '    Me.Height = 335  ' 327
    '    pnl_Back.Enabled = True
    '    'grp_Filter.Visible = False
    'End Sub

    Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub btn_FindClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

    End Sub

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

    End Sub

    'Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
    '    Call btn_Filteropen_Click(sender, e)
    'End Sub

    'Private Sub btn_Filteropen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filteropen.Click
    '    Dim movid As Integer

    '    movid = 0
    '    If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
    '        movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
    '    End If

    '    If Val(movid) <> 0 Then
    '        move_record(movid)
    '        btn_FilterClose_Click(sender, e)
    '    End If
    'End Sub

    'Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
    '    If e.KeyValue = 13 Then
    '        Call btn_Filteropen_Click(sender, e)
    '    End If
    'End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub




    Private Sub txt_AccountNo_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AccountNo_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub


    Private Sub cbo_BankName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BankName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BankName, Nothing, cbo_Partner, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_BankName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BankName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BankName, cbo_Partner, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_BankName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BankName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BankName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub



    Private Sub cbo_PaperOrientation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_PaperOrientation.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_PaperOrientation, cbo_Partner, txt_LeftMargin, "", "", "", "")
    End Sub

    Private Sub cbo_PaperOrientation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_PaperOrientation.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_PaperOrientation, txt_LeftMargin, "", "", "", "")
    End Sub




    Private Sub cbo_Partner_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Partner.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Partner, cbo_BankName, Cbo_PaperOrientation, "", "", "", "")
    End Sub

    Private Sub cbo_Partne_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Partner.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Partner, Cbo_PaperOrientation, "", "", "", "")
    End Sub



    Private Sub txt_AccountNo_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AccountNo_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_AccountNo_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AccountNo_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_AcPayee_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AcPayee_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AcPayee_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AcPayee_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AcPayeeWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AcPayeeWidth.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AmountWords_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AmountWords_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AmountWords_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AmountWords_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AmountWords_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AmountWords_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CompanyName_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CompanyName_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CompanyName_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CompanyName_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CompanyName_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CompanyName_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Date_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Date_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Date_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Date_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Date_width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Date_width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Partner_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Partner_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Partner_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Partner_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Partner_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Partner_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub


    Private Sub txt_PartyName_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyName_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_PartyName_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyName_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_PartyName_width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyName_width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_RightMargin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TopMargin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rs_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rs_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rs_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rs_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rs_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rs_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Second_AmountWords_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Second_AmountWords_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Second_AmountWords_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Second_AmountWords_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Second_AmountWords_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Second_AmountWords_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_second_PartyName_Left_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_second_PartyName_Left.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Second_PartyName_Top_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Second_PartyName_Top.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Second_PartyName_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Second_PartyName_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TopMargin_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_TopMargin.TextChanged

    End Sub
End Class