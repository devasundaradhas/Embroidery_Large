Public Class Unit_Creation
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private Sub clear()

        grp_Back.Enabled = True
        grp_Find.Visible = False

        Me.Height = 245  '250

        txt_IdNo.Text = ""
        txt_IdNo.ForeColor = Color.Black

        txt_Name.Text = ""
        cbo_Find.Text = ""

    End Sub

    Private Sub move_record(ByVal idno As Integer)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        da = New SqlClient.SqlDataAdapter("select unit_idno, unit_name from unit_head where unit_idno = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            txt_IdNo.Text = dt.Rows(0)("unit_idno").ToString
            txt_Name.Text = dt.Rows(0)("unit_name").ToString
        End If

        dt.Clear()

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        MessageBox.Show("This list cannot be modified.", "Restricted! ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Exit Sub

        Dim cmd As New SqlClient.SqlCommand
        Dim new_no As Integer

        clear()

        cmd.Connection = con
        cmd.CommandText = "Select max(Unit_Idno) from Unit_Head"

        new_no = 0
        If IsDBNull(cmd.ExecuteScalar()) = False Then
            new_no = Val(cmd.ExecuteScalar())
        End If

        txt_IdNo.Text = Val(new_no) + 1
        txt_IdNo.ForeColor = Color.Red

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("Select Unit_Name from Unit_Head Order by Unit_Name", con)
        da.Fill(dt)

        'cbo_Find.Items.Clear()
        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Unit_Name"

        grp_Find.Visible = True
        grp_Back.Enabled = False

        Me.Height = 445 ' 420

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        MessageBox.Show("This list cannot be modified.", "Restricted! ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Exit Sub

        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim nr As Integer
        Dim new_entry As Boolean = False

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Unit", "DOES Not SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.CommandText = "Update Unit_Head Set Unit_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(txt_Name.Text) & "' where Unit_IdNo = " & Str(Val(txt_IdNo.Text))
            cmd.Transaction = trans

        nr = cmd.ExecuteNonQuery

            If nr = 0 Then

                cmd.CommandText = "Insert into Unit_Head(Unit_IdNo, Unit_Name, sur_Name) values (" & Str(Val(txt_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Name.Text) & "') "

                cmd.ExecuteNonQuery()

                new_entry = True

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "UNIT"

            MessageBox.Show("Saved Sucessfuly", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If new_entry = True Then new_record()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If MessageBox.Show("Do you want to delete?", "FOR DELETING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Item_Head where Unit_IdNo = " & Str(Val(txt_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Unit", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from Waste_Head where Unit_IdNo = " & Str(Val(txt_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Unit", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            cmd.Connection = con
            cmd.CommandText = "delete from unit_head where unit_idno = " & Str(Val(txt_IdNo.Text))

            cmd.ExecuteNonQuery()

            dt.Dispose()
            da.Dispose()
            cmd.Dispose()

            MessageBox.Show("Deleted Sucessfully", "FOR DELETIING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        new_record()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Unit_IdNo) from Unit_Head Where Unit_IdNo <> 0", con)
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(unit_idno) from unit_head where unit_idno > " & Str(Val(txt_IdNo.Text))

            dr = cmd.ExecuteReader

            movid = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movid) <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try

            da = New SqlClient.SqlDataAdapter("select max(unit_idno) from unit_head where unit_idno < " & Str(Val(txt_IdNo.Text)), con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(unit_idno) from unit_head"

            If IsDBNull(cmd.ExecuteScalar) = False Then
                movid = Val(cmd.ExecuteScalar)
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Unit_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Width = 485
        Me.Height = 245 '250

        grp_Find.Left = 20
        grp_Find.Top = 235

        con.Open()

        movefirst_record()

    End Sub

    Private Sub Unit_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Unit_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible = True Then
                btnClose_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
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
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                            Condt = " Where Unit_Name like '" & Trim(FindStr) & "%' or Unit_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head " & Condt & " order by Unit_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Unit_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        grp_Back.Enabled = True
        grp_Find.Visible = False

        Me.Height = 245 ' 250

        txt_Name.Enabled = True
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select unit_idno from unit_head where unit_name = '" & Trim(cbo_Find.Text) & "'", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("unit_idno").ToString) = False Then
                    movid = Val(dt.Rows(0).Item("unit_idno").ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

            btnClose_Click(sender, e)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

End Class