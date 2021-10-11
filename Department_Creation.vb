Public Class Department_Creation



    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Public previlege As String

    Private Sub CLEAR()

            Me.Height = 269
            pnl_Back.Enabled = True
            grp_Open.Visible = False
            grp_Filter.Visible = False

            lbl_IdNo.Text = ""
            lbl_IdNo.ForeColor = Color.Black
            txt_Name.Text = ""

            New_Entry = False

        End Sub
        Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim txtbx As TextBox
            Dim combobx As ComboBox

            On Error Resume Next

            If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
                Me.ActiveControl.BackColor = Color.SpringGreen ' Color.MistyRose ' Color.PaleGreen
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

        Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

            On Error Resume Next

            If IsNothing(Prec_ActCtrl) = False Then
                If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                    Prec_ActCtrl.BackColor = Color.LightBlue
                    Prec_ActCtrl.ForeColor = Color.Blue
                End If
            End If

        End Sub



        Public Sub move_record(ByVal idno As Integer)
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt As New DataTable

            If Val(idno) = 0 Then Exit Sub

            CLEAR()

        da = New SqlClient.SqlDataAdapter("select a.* from Department_Head a where a.Department_IdNo = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Department_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Department_Name").ToString
        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Private Sub Department_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        grp_Open.Left = 6
        grp_Open.Top = 250
        grp_Open.Visible = False

        grp_Filter.Left = 6
        grp_Filter.Top = 250
        grp_Filter.Visible = False


        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_CloseFilter.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_CloseOpen.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_CloseFilter.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_CloseOpen.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Save.LostFocus, AddressOf ControlLostFocus







        con.Open()
        Me.Top = Me.Top - 100
        new_record()
    End Sub

    Private Sub Department_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Department_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            ElseIf grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            Else
                Me.Close()
            End If

        End If

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("D") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim cmd As New SqlClient.SqlCommand

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Department_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Department_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try


            cmd.Connection = con
            cmd.CommandText = "delete from Department_Head where Department_IdNo = " & Str(Val(lbl_IdNo.Text))

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

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Department_IdNo, Department_Name from Department_Head where Department_IdNo <> 0 order by Department_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "DEPARTMENT NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 540

        da.Dispose()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Department_IdNo) from Department_Head Where Department_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Department_IdNo) from Department_Head Where Department_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(Department_IdNo) from Department_Head Where Department_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Department_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Department_IdNo) from Department_Head Where Department_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Department_IdNo <> 0", con)
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

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("A") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        CLEAR()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Department_Head", "Department_IdNo", "")

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Department_Name"

        new_record()

        Me.Height = 500
        grp_Open.Visible = True
        pnl_Back.Enabled = False
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
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

        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Department_Creation, New_Entry) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Department_Head", "Department_IdNo", "", trans)

                cmd.CommandText = "Insert into Department_Head(Department_IdNo, Department_Name, sur_name) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Department_Head set Department_Name = '" & Trim(txt_Name.Text) & "', sur_name = '" & Trim(Sur) & "' where Department_IdNo = " & Str(Val(lbl_IdNo.Text)) & ""
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "DEPARTMENT"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_department_head") > 0 Then
                MessageBox.Show("Duplicate Department Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()


        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        Me.Height = 269
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Department_IdNo from Department_Head where Department_Name = '" & Trim(cbo_Open.Text) & "'", con)
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

        btn_CloseOpen_Click(sender, e)

    End Sub


    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "Department_Head", "Department_Name", "", "(Department_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Department_Head", "Department_Name", "", "(Department_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            Call btn_Open_Click(sender, e)
        End If

    End Sub

        Private Sub btn_CloseFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
            Me.Height = 269
            pnl_Back.Enabled = True
            grp_Filter.Visible = False
        End Sub

        Private Sub btn_Filter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter.Click
            Dim idno As Integer

            idno = Val(dgv_Filter.CurrentRow.Cells(0).Value)

            If Val(idno) <> 0 Then
                move_record(idno)
                pnl_Back.Enabled = True
                grp_Filter.Visible = False
            End If
        End Sub

        Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
            Call btn_Filter_Click(sender, e)
        End Sub

        Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
            If e.KeyValue = 13 Then
                Call btn_Filter_Click(sender, e)
            End If
        End Sub






    End Class