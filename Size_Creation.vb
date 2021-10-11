Public Class Size_Creation
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean

    Private Sub Size_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Width = 475
        grp_Find.Left = 6
        grp_Find.Top = 250
        grp_Find.Visible = False

        grp_Filter.Left = 6
        grp_Filter.Top = 250
        grp_Filter.Visible = False
        con.Open()
        new_record()
    End Sub

    Private Sub Size_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Size_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_FilterClose_Click(sender, e)
            ElseIf grp_Find.Visible Then
                btn_FindOpen_Click(sender, e)
            Else
                Me.Close()
            End If

        End If
    End Sub

    Private Sub clear()
        Me.Height = 275
        pnl_back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_Name.Text = ""
        txt_Sqft.Text = ""
        New_Entry = False
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Size_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Size_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            cmd.Connection = con
            cmd.CommandText = "delete from Size_Head where Size_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--
    End Sub
    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '--
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Size_IdNo ,Size_Name from Size_Head where Size_IdNo <> 0 order by Size_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "BRAND NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True

        pnl_back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 510

        da.Dispose()
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select * from Size_Head a where Size_IdNo = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Size_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Size_Name").ToString
            txt_Sqft.Text = Val(dt.Rows(0).Item("Total_Sqft").ToString)
            If Val(txt_Sqft.Text) = 0 Then
                txt_Sqft.Text = ""
            End If
        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Size_IdNo) from Size_Head Where Size_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Size_IdNo) from Size_Head Where Size_IdNo <> 0", con)
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
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Size_IdNo) from Size_Head Where Size_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Size_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Size_IdNo) from Size_Head Where Size_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Size_IdNo <> 0", con)
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
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Size_Head", "Size_IdNo", "")

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Size_Name from Size_Head order by Size_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Size_Name"

        new_record()

        Me.Height = 510
        grp_Find.Visible = True
        pnl_back.Enabled = False
        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    End Sub


    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Size_Creation, New_Entry) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Size_Head", "Size_IdNo", "", trans)

                cmd.CommandText = "Insert into Size_Head(Size_IdNo, Size_Name, Total_Sqft , Sur_Name) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', " & Str(Val(txt_Sqft.Text)) & ", '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Size_Head set Size_Name = '" & Trim(txt_Name.Text) & "', Total_Sqft = " & Str(Val(txt_Sqft.Text)) & " , Sur_Name = '" & Trim(Sur) & "' where Size_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "Brand"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_Size_Head") > 0 Then
                MessageBox.Show("Duplicate Brand Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()


        End Try
    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Size_IdNo from Size_Head where Size_Name = '" & Trim(cbo_Find.Text) & "'", con)
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

        btn_FilterClose_Click(sender, e)
    End Sub

    Private Sub btn_FindClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindClose.Click

        Me.Height = 275
        pnl_back.Enabled = True
        grp_Find.Visible = False
    End Sub

    Private Sub btn_FilterOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterOpen.Click
        Dim movid As Integer
        movid = 0
        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            btn_FilterClose_Click(sender, e)
        End If
    End Sub

    Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click
        Me.Height = 275
        pnl_back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub txt_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.GotFocus
        txt_Name.BackColor = Color.PaleGreen
        txt_Name.ForeColor = Color.Blue
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If (e.KeyValue = 38) Then
            btn_Close.Focus()
        End If
        If (e.KeyValue = 40) Then
            txt_Sqft.Focus()
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
         If Asc(e.KeyChar) = 13 Then
            txt_Sqft.Focus()
        End If
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "Size_Head", "Size_Name", "", "")
    End Sub


    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "Size_Head", "Size_Name", "", "")
        If Asc(e.KeyChar) = 13 Then
            btn_FindOpen_Click(sender, e)
        End If
    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_FilterOpen_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_FilterOpen_Click(sender, e)
        End If
    End Sub
    Private Sub txt_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.LostFocus
        txt_Name.BackColor = Color.White
        txt_Name.ForeColor = Color.Black
    End Sub

    Private Sub txt_Sqft_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Sqft.GotFocus
        txt_Name.BackColor = Color.PaleGreen
        txt_Name.ForeColor = Color.Blue
    End Sub

    Private Sub txt_Sqft_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Sqft.KeyDown
        If (e.KeyValue = 38) Then
            txt_Name.Focus()
        End If
        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_Sqft_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Sqft.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_Sqft_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Sqft.LostFocus
        txt_Name.BackColor = Color.White
        txt_Name.ForeColor = Color.Black
    End Sub
End Class