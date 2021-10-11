Public Class CompanyGroup_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private CompgrpCondt As String = ""

    Private Sub clear()

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False

        New_Entry = False
        Insert_Entry = False

        txt_IdNo.Text = ""
        txt_IdNo.ForeColor = Color.Black

        Me.Height = 290

        txt_Name.Text = ""

        If Month(Date.Today) >= 4 Then
            dtp_FromDate.Text = "01/04/" & Year(Date.Today)
            dtp_ToDate.Text = "31/03/" & Year(Date.Today) + 1
        Else
            dtp_FromDate.Text = "01/04/" & Year(Date.Today) - 1
            dtp_ToDate.Text = "31/03/" & Year(Date.Today)
        End If

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader


        If Val(idno) = 0 Then Exit Sub

        clear()

        Try
            cmd.Connection = con
            cmd.CommandText = "select * from CompanyGroup_Head where " & CompgrpCondt & IIf(Trim(CompgrpCondt) <> "", " and ", "") & " CompanyGroup_IdNo = " & Str(Val(idno))

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read() Then
                    txt_IdNo.Text = dr.Item("CompanyGroup_IdNo").ToString()
                    txt_Name.Text = dr.Item("CompanyGroup_Name").ToString()
                    dtp_FromDate.Text = Format(Convert.ToDateTime(dr("From_Date").ToString()), "dd-MM-") & Microsoft.VisualBasic.Left(dr("Financial_Range").ToString(), 4)
                    dtp_ToDate.Text = Format(Convert.ToDateTime(dr("To_Date").ToString()), "dd-MM-") & Microsoft.VisualBasic.Right(dr("Financial_Range").ToString(), 4)
                End If
            End If

            dr.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Finally
            If txt_IdNo.Enabled And txt_IdNo.Visible Then txt_IdNo.Focus()

        End Try

    End Sub

    Private Sub CompanyGroup_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub ItemGroup_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btn_CloseFind_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub CompanyGroup_Creation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F2 Then
            new_record()
        ElseIf e.KeyCode = Keys.F3 Then
            save_record()
        ElseIf e.KeyCode = Keys.F4 Then
            open_record()
        ElseIf e.KeyCode = Keys.F5 Then
            movefirst_record()
        ElseIf e.KeyCode = Keys.F6 Then
            movenext_record()
        ElseIf e.KeyCode = Keys.F7 Then
            moveprevious_record()
        ElseIf e.KeyCode = Keys.F8 Then
            movelast_record()
        ElseIf e.KeyCode = Keys.F9 Then
            delete_record()
        ElseIf e.KeyCode = Keys.F12 Then
            Me.Close()
        End If
    End Sub

    Private Sub ItemGroup_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Height = 290
        con.Open()

        CompgrpCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompgrpCondt = "(CompanyGroup_Type <> 'UNACCOUNT')"
        End If

        new_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim DBName As String = ""

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSDCG222" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Are You Sure to Delete, If Deleted can't get data's back?", "FOR DELETE CONFIRMATION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            DBName = Common_Procedures.get_Company_DataBaseName(Trim(Val(txt_IdNo.Text)))

            cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
            cn1.Open()

            cmd.Connection = cn1

            cmd.CommandText = "drop database " & Trim(DBName)
            cmd.ExecuteNonQuery()

            cn1.Close()

            cmd.Connection = con
            cmd.CommandText = "delete from CompanyGroup_Head where CompanyGroup_IdNo = " & Str(Val(txt_IdNo.Text))

            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Exit Sub

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select CompanyGroup_IdNo, CompanyGroup_Name from CompanyGroup_Head order by CompanyGroup_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "COMPANY GROUP NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True
        grp_Filter.Left = grp_Find.Left
        grp_Filter.Top = grp_Find.Top

        pnl_Back.Enabled = False
        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 500

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select min(CompanyGroup_IdNo) from CompanyGroup_Head where " & CompgrpCondt & IIf(Trim(CompgrpCondt) <> "", " and ", "") & " CompanyGroup_IdNo <> 0", con)
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
            Exit Sub

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(CompanyGroup_IdNo) from CompanyGroup_Head where  " & CompgrpCondt & IIf(Trim(CompgrpCondt) <> "", " and ", "") & " CompanyGroup_IdNo <> 0", con)
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
            Exit Sub

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select min(CompanyGroup_IdNo) from CompanyGroup_Head where  " & CompgrpCondt & IIf(Trim(CompgrpCondt) <> "", " and ", "") & " CompanyGroup_IdNo > " & Str(Val(txt_IdNo.Text)) & " and CompanyGroup_IdNo <> 0", con)
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
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(CompanyGroup_IdNo) from CompanyGroup_Head where  " & CompgrpCondt & IIf(Trim(CompgrpCondt) <> "", " and ", "") & " CompanyGroup_IdNo < " & Str(Val(txt_IdNo.Text)) & " and CompanyGroup_IdNo <> 0", con)
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
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim newno As Integer

        clear()

        New_Entry = True

        txt_IdNo.ForeColor = Color.Red

        cmd.Connection = con
        cmd.CommandText = "select max(CompanyGroup_IdNo) from CompanyGroup_Head"

        dr = cmd.ExecuteReader

        newno = 0
        If dr.HasRows Then
            If dr.Read() Then
                If IsDBNull(dr(0).ToString) = False Then
                    newno = Val(dr(0).ToString)
                End If
            End If
        End If
        dr.Close()

        txt_IdNo.Text = Val(newno) + 1

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select CompanyGroup_Name from CompanyGroup_Head order by CompanyGroup_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "CompanyGroup_Name"

        new_record()

        grp_Find.Visible = True
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        Me.Height = 450

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---MessageBox.Show("no insert")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---MessageBox.Show("no printing")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim FnRange As String
        Dim Pth As String
        Dim DbName As String
    
        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Company Group Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_FromDate.Text) = False Then
            MessageBox.Show("Invalid Financial From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
            Exit Sub
        End If

        If IsDate(dtp_ToDate.Text) = False Then
            MessageBox.Show("Invalid Financial To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
            Exit Sub
        End If

        pth = Trim(Common_Procedures.AppPath) & "\script.SQL"

        If System.IO.File.Exists(Pth) = False Then
            MessageBox.Show("Invalid script file", "DOES NOT CREATE COMPANY GROUP...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        FnRange = Year(Convert.ToDateTime(dtp_FromDate.Text.ToString)) & "-" & Year(Convert.ToDateTime(dtp_ToDate.Text.ToString))

        trans = con.BeginTransaction

        Try
            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)
            cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)

            cmd.Transaction = trans

            If New_Entry = True Then

                cmd.CommandText = "Insert into CompanyGroup_Head(CompanyGroup_IdNo, CompanyGroup_Name, From_Date, To_Date, Financial_Range) values (" & Str(Val(txt_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', @FromDate, @ToDate, '" & Trim(FnRange) & "')"


            Else
                cmd.CommandText = "update CompanyGroup_Head set CompanyGroup_Name = '" & Trim(txt_Name.Text) & "', From_Date = @FromDate, To_Date = @ToDate, Financial_Range = '" & Trim(FnRange) & "' where CompanyGroup_IdNo = " & Str(Val(txt_IdNo.Text))

            End If
            cmd.ExecuteNonQuery()

            trans.Commit()

            If New_Entry = True Then

                DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(txt_IdNo.Text)))

                Call CreateNewDb_For_CompanyGroup(DbName)

            End If

            cmd.Dispose()

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        da = New SqlClient.SqlDataAdapter("select CompanyGroup_IdNo from CompanyGroup_Head where CompanyGroup_Name = '" & Trim(cbo_Find.Text) & "'", con)
        da.Fill(dt)

        movid = 0
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movid = Val(dt.Rows(0)(0).ToString)

            End If
        End If

        If movid <> 0 Then
            move_record(movid)
        Else
            new_record()
        End If

        btn_CloseFind_Click(sender, e)

    End Sub

    Private Sub btn_CloseFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFind.Click
        Me.Height = 290
        pnl_Back.Enabled = True
        grp_Find.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
        With cbo_Find
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With

    End Sub

    Private Sub cbo_Find_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.LostFocus
        With cbo_Find
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub


    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Try
            With cbo_Find
                If e.KeyValue = 38 And .DroppedDown = False Then
                    'e.Handled = True
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    'e.Handled = True
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

                        If Trim(FindStr) <> "" Then
                            Condt = " Where CompanyGroup_Name like '" & FindStr & "%' or CompanyGroup_Name like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select CompanyGroup_Name from CompanyGroup_Head " & Condt & " order by CompanyGroup_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "CompanyGroup_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'If Asc(e.KeyChar) = 13 Then
        '    btn_Find_Click(sender, e)
        'End If
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click

        txt_Name.Enabled = True
        btn_Save.Enabled = True
        btn_Close.Enabled = True
        grp_Filter.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        Me.Height = 197

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

    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub dtp_ToDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_ToDate.KeyUp
        If e.KeyCode = Keys.Up Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub dtp_FromDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FromDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub dtp_FromDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FromDate.KeyUp
        If e.KeyCode = Keys.Up Then
            SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        Dim K As Integer

        If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
            K = Asc(e.KeyChar)
            K = K - 32
            e.KeyChar = Chr(K)
        End If

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyUp
        If e.KeyCode = Keys.Up Then
            SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub CreateNewDb_For_CompanyGroup(ByVal DataBaseName As String)
        Dim CnMas As SqlClient.SqlConnection
        Dim Cn2 As SqlClient.SqlConnection
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Nr As Integer
        Dim Conn_String As String
        Dim FnRange As String

        CnMas = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
        CnMas.Open()

        Da1 = New SqlClient.SqlDataAdapter("select * from sysdatabases where name = '" & Trim(DataBaseName) & "'", CnMas)
        Dt1 = New DataTable
        Da1.Fill(Dt1)


        If Dt1.Rows.Count = 0 Then

            cmd.Connection = CnMas

            cmd.CommandText = "create database " & Trim(DataBaseName)
            cmd.ExecuteNonQuery()

            Conn_String = Common_Procedures.Create_Sql_ConnectionString(Trim(DataBaseName))
            'If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "WIN" Then
            '    Conn_String = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(DataBaseName) & ";Integrated Security=True"
            'Else
            '    Conn_String = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(DataBaseName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False"
            'End If

            Cn2 = New SqlClient.SqlConnection(Conn_String)
            Cn2.Open()

            Call Run_database_Script(Cn2)

            FieldsCheck.vFldsChk_From_CompGroupCreation_Status = True
            FieldsCheck.FieldsCheck_All(Cn2, Me)
            FieldsCheck.vFldsChk_All_Status = False
            FieldsCheck.vFldsChk_From_CompGroupCreation_Status = False

            Call Common_Procedures.Default_GroupHead_Updation(Cn2)
            Call Common_Procedures.Default_LedgerHead_Updation(Cn2)
            Call Common_Procedures.Default_Master_Updation(Cn2)


            cmd.Connection = Cn2

            FnRange = Year(Convert.ToDateTime(dtp_FromDate.Text.ToString)) & "-" & Year(Convert.ToDateTime(dtp_ToDate.Text.ToString))

            cmd.CommandText = "Update FinancialRange_Head set Financial_Range = '" & Trim(FnRange) & "'"
            Nr = cmd.ExecuteNonQuery()
            If Nr = 0 Then
                cmd.CommandText = "Insert into FinancialRange_Head(Financial_Range) values ('" & Trim(FnRange) & "')"
                cmd.ExecuteNonQuery()
            End If

            Cn2.Close()


        End If

        CnMas.Close()
        CnMas = Nothing
        Cn2 = Nothing

    End Sub

    Private Sub Run_database_Script(ByVal Cn2 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim pth As String
        Dim SqlStr As String = ""
        Dim ar() As String
        Dim fs As System.IO.FileStream
        Dim r As System.IO.StreamReader
        Dim i As Integer

        Try

            pth = Trim(Common_Procedures.AppPath) & "\script.SQL"

            If System.IO.File.Exists(pth) = True Then
                fs = New System.IO.FileStream(pth, System.IO.FileMode.Open)
                r = New System.IO.StreamReader(fs)
                SqlStr = r.ReadToEnd
                r.Close()
                fs.Close()
                r.Dispose()
                fs.Dispose()
            Else
                MessageBox.Show("Invalid script file", "DOES NOT CREATE NEW COMPANY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            If Trim(SqlStr) <> "" Then

                cmd.Connection = Cn2

                ar = Split(SqlStr, "GO")

                For i = 0 To UBound(ar)
                    If Trim(ar(i)) <> "" Then

                        cmd.CommandText = Trim(ar(i))
                        cmd.ExecuteNonQuery()

                    End If
                Next

            Else
                MessageBox.Show("Invalid database script", "DOES NOT CREATE NEW COMPANY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            cmd.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CREATE NEW COMPANY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Private Sub dtp_ToDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_ToDate.ValueChanged

    End Sub
End Class