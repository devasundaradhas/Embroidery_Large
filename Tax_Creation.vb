Public Class Tax_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False

        Me.Height = 262  ' 284

        txt_IdNo.Text = ""
        txt_IdNo.ForeColor = Color.Black

        txt_Name.Text = ""

        cbo_TaxLedgerAc.Text = ""
        cbo_Find.Text = ""
        'dgv_Filter.Rows.Clear()

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try
            cmd.Connection = con
            cmd.CommandText = "select a.*, b.Ledger_Name from Tax_Head a LEFT OUTER JOIN Ledger_Head b ON a.Tax_Ledger_Ac_IdNo = b.Ledger_IdNo where a.Tax_IdNo = " & Str(Val(idno))

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read() Then
                    txt_IdNo.Text = dr("Tax_IdNo").ToString()
                    txt_Name.Text = dr("Tax_Name").ToString()
                    cbo_TaxLedgerAc.Text = dr("Ledger_Name").ToString()
                End If

            End If

            dr.Close()

            cmd.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()

            If txt_IdNo.Enabled And txt_IdNo.Visible Then txt_IdNo.Focus()

        End Try

    End Sub

    Private Sub Tax_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Height = 262  '284

        con.Open()

        new_record()

    End Sub

    Private Sub Tax_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Tax_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btnClose_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from item_head where Tax_IdNo = " & Str(Val(txt_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this ItemGroup", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Sales_Details where Tax_IdNo = " & Str(Val(txt_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this ItemGroup", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con
            cmd.CommandText = "delete from Tax_Head where Tax_IdNo = " & Str(Val(txt_IdNo.Text))

            cmd.ExecuteNonQuery()

            dt.Dispose()
            da.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Tax_IdNo, Tax_Name from Tax_Head where Tax_IdNo <> 0 order by Tax_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "ITEM GROUP NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True
        grp_Filter.Left = grp_Find.Left
        grp_Filter.Top = grp_Find.Top

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 520 ' 400

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Tax_IdNo) from Tax_Head Where Tax_IdNo <> 0", con)
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
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(Tax_IdNo) from Tax_Head WHERE Tax_IdNo <> 0"

            dr = cmd.ExecuteReader

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(Tax_IdNo) from Tax_Head where Tax_IdNo > " & Str(Val(txt_IdNo.Text))

            dr = cmd.ExecuteReader()

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(Tax_IdNo) from Tax_Head where Tax_IdNo < " & Str(Val(txt_IdNo.Text)) & " and Tax_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        'Dim cmd As New SqlClient.SqlCommand
        'Dim dr As SqlClient.SqlDataReader
        'Dim newno As Integer

        clear()

        New_Entry = True
        txt_IdNo.ForeColor = Color.Red

        txt_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Tax_Head", "Tax_IdNo", "")

        'cmd.Connection = con
        'cmd.CommandText = "select max(Tax_IdNo) from Tax_Head"

        'dr = cmd.ExecuteReader

        'newno = 0
        'If dr.HasRows Then
        '    If dr.Read() Then
        '        If IsDBNull(dr(0).ToString) = False Then
        '            newno = Val(dr(0).ToString)
        '        End If
        '    End If
        'End If

        'dr.Close()
        'cmd.Dispose()

        'txt_IdNo.Text = Val(newno) + 1

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Tax_Name from Tax_Head order by Tax_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Tax_Name"

        new_record()

        grp_Find.Visible = True
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        Me.Height = 465  '480 

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--- No Printing
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String
        Dim TaxAc_IdNo As Integer = 0

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            txt_Name.Focus()
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        TaxAc_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_TaxLedgerAc.Text))
        If TaxAc_IdNo = 0 Then
            MessageBox.Show("Invalid Tax Ledger Account Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_TaxLedgerAc.Focus()
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                txt_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Tax_Head", "Tax_IdNo", "", trans)

                cmd.CommandText = "Insert into Tax_Head ( Tax_IdNo, Tax_Name , sur_name , Tax_Ledger_Ac_IdNo ) Values (" & Str(Val(txt_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "' ," & Str(Val(TaxAc_IdNo)) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Tax_Head set Tax_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "', Tax_Ledger_Ac_IdNo =  " & Str(Val(TaxAc_IdNo)) & " Where Tax_IdNo = " & Str(Val(txt_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "TAX"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "duplicate_itemgrouphead_groupname") > 0 Then
                MessageBox.Show("Duplicate Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Tax_IdNo from Tax_Head where Tax_Name = '" & Trim(cbo_Find.Text) & "'", con)
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
            move_record(movid)
        Else
            new_record()
        End If

        btnClose_Click(sender, e)

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Height = 262  '284
        pnl_Back.Enabled = True
        grp_Find.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
        'cbo_Find.DroppedDown = True
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Try
            With cbo_Find
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Find

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        btn_Find_Click(sender, e)

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Tax_Name like '" & Trim(FindStr) & "%' or Tax_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Tax_Name from Tax_Head " & Condt & " order by Tax_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Tax_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'If Asc(e.KeyChar) = 13 Then
        '    btn_Find_Click(sender, e)
        'End If

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        Me.Height = 262  '284

    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
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

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyCode = Keys.Enter Then
            btn_Open_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then e.SuppressKeyPress = True : e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then e.SuppressKeyPress = True : e.Handled = True : cbo_TaxLedgerAc.Focus()
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_TaxLedgerAc.Focus()
        End If
    End Sub

    Private Sub cbo_TaxLedgerAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxLedgerAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_TaxLedgerAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxLedgerAc.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxLedgerAc, txt_Name, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
            If (e.KeyValue = 40 And cbo_TaxLedgerAc.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.SuppressKeyPress = True : e.Handled = True
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TaxLedgerAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxLedgerAc.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxLedgerAc, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
            If Asc(e.KeyChar) = 13 Then
                e.Handled = True
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_TaxLedgerAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxLedgerAc.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 Then
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_TaxLedgerAc.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

End Class