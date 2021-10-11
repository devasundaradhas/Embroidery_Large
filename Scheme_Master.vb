Imports Excel = Microsoft.Office.Interop.Excel

Public Class Scheme_Master

    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double


    Private Sub clear()

        pnl_back.Enabled = True
        grp_Open.Visible = False
        ''grp_Filter.Visible = False
        cbo_ItemName.Visible = False

        cbo_CategoryName.Text = ""

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_SchemeName.Text = ""
        dgv_details.Rows.Clear()
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

        If Me.ActiveControl.Name <> cbo_ItemName.Name Then
            cbo_ItemName.Visible = False
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
        dgv_details.CurrentCell.Selected = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try


            cmd.Connection = con
            cmd.CommandText = "delete from Scheme_Details where Scheme_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            cmd.Connection = con
            cmd.CommandText = "delete from Scheme_Head where Scheme_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_SchemeName.Enabled And txt_SchemeName.Visible Then txt_SchemeName.Focus()

        End Try
    End Sub

    'Public Sub filter_record() Implements Interface_MDIActions.filter_record
    '    Dim da As New SqlClient.SqlDataAdapter("select count_IdNo, Count_Name,Count_Description from Count_Head where Count_IdNo <> 0 order by Count_IdNo", con)
    '    Dim dt As New DataTable

    '    da.Fill(dt)

    '    With dgv_Filter

    '        .Columns.Clear()
    '        .DataSource = dt

    '        .RowHeadersVisible = False

    '        .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    '        .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

    '        .Columns(0).HeaderText = "IDNO"
    '        .Columns(1).HeaderText = "NAME"
    '        .Columns(2).HeaderText = "DESCRIPTION"


    '        .Columns(0).FillWeight = 60
    '        .Columns(1).FillWeight = 160
    '        .Columns(2).FillWeight = 300


    '    End With

    '    new_record()

    '    grp_Filter.Visible = True

    '    pnl_back.Enabled = False

    '    If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

    '    Me.Height = 514

    '    da.Dispose()
    'End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim sno As Integer, n As Integer


        If Val(idno) = 0 Then Exit Sub

        clear()


        da = New SqlClient.SqlDataAdapter("select a.* ,b.* from Scheme_Head a left outer join Cetegory_Head b On a.Cetegory_IdNo = b.Cetegory_IdNo  where Scheme_IdNo = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Scheme_IdNo").ToString
            txt_SchemeName.Text = dt.Rows(0).Item("Scheme_Name").ToString
            cbo_CategoryName.Text = dt.Rows(0).Item("Cetegory_Name").ToString
        


            da = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name  from Scheme_Details a, Item_Head b where a.Scheme_IdNo = " & Str(Val(idno)) & " and a.Item_idno = b.Item_idno Order by a.sl_no", con)
            da.Fill(dt2)

            dgv_details.Rows.Clear()
            sno = 0

            If dt2.Rows.Count > 0 Then
                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_details.Rows.Add()
                    sno = sno + 1
                    dgv_details.Rows(n).Cells(0).Value = Val(sno)
                    dgv_details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                    dgv_details.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Discount_Percentage")), "#########0.000")
                    dgv_details.Rows(n).Cells(3).Value = IIf(InStr(Trim(dt2.Rows(i).Item("Primary_StartDate_Text")), "1990") > 0, "", Trim(dt2.Rows(i).Item("Primary_StartDate_Text")))
                    dgv_details.Rows(n).Cells(4).Value = IIf(InStr(Trim(dt2.Rows(i).Item("Primary_EndDate_Text")), "1990") > 0, "", Trim(dt2.Rows(i).Item("Primary_EndDate_Text")))
                    dgv_details.Rows(n).Cells(5).Value = IIf(InStr(Trim(dt2.Rows(i).Item("Secondary_StartDate_Text")), "1990") > 0, "", Trim(dt2.Rows(i).Item("Secondary_StartDate_Text")))
                    dgv_details.Rows(n).Cells(6).Value = IIf(InStr(Trim(dt2.Rows(i).Item("Secondary_EndDate_Text")), "1990") > 0, "", Trim(dt2.Rows(i).Item("Secondary_EndDate_Text")))


                Next i

                For i = 0 To dgv_details.RowCount - 1
                    dgv_details.Rows(i).Cells(0).Value = Val(i) + 1
                Next

            End If


        End If
        Grid_Cell_DeSelect()
        dt.Dispose()
        da.Dispose()

        If txt_SchemeName.Enabled And txt_SchemeName.Visible Then txt_SchemeName.Focus()
    End Sub
    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Scheme_IdNo) from Scheme_Head Where Scheme_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Scheme_IdNo) from Scheme_Head Where Scheme_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(Scheme_IdNo) from Scheme_Head Where Scheme_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Scheme_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Scheme_IdNo) from Scheme_Head Where Scheme_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Scheme_IdNo <> 0", con)
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

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Scheme_Head", "Scheme_IdNo", "")

        If txt_SchemeName.Enabled And txt_SchemeName.Visible Then txt_SchemeName.Focus()
    End Sub

    'Public Sub open_record() Implements Interface_MDIActions.open_record
    '    Dim da As New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
    '    Dim dt As New DataTable

    '    da.Fill(dt)

    '    cbo_Find.DataSource = dt
    '    cbo_Find.DisplayMember = "Count_Name"

    '    new_record()

    '    Me.Height = 513
    '    grp_find.Visible = True
    '    pnl_back.Enabled = False
    '    If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    'End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String
        Dim Itm_id As Integer
        Dim SNo As Integer
        Dim Cat_Id As Integer = 0
        Dim Temp_dttm As DateTime = "01-01-1990"

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_SchemeName.Text) = "" Then
            MessageBox.Show("Invalid Scheme Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_SchemeName.Enabled Then txt_SchemeName.Focus()
            Exit Sub
        End If

        Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(cbo_CategoryName.Text)))


        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_SchemeName.Text))

        With dgv_details
            For i = 0 To dgv_details.RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(2).Value) <> 0 Then

                    Itm_id = Val(Common_Procedures.Item_NameToIdNo(con, Trim(dgv_details.Rows(i).Cells(1).Value)))
                    If Itm_id = 0 Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_details.Enabled And dgv_details.Visible Then
                            dgv_details.Focus()
                            dgv_details.CurrentCell = dgv_details.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If
                End If
            Next
        End With
        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans

          
            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Scheme_Head", "Scheme_IdNo", "", trans)

                cmd.CommandText = "Insert into Scheme_Head(Scheme_IdNo, Scheme_Name, Sur_Name ,Cetegory_IdNo ) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_SchemeName.Text) & "', '" & Trim(Sur) & "' , " & Str(Val(Cat_Id)) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Scheme_Head set   Scheme_Name = '" & Trim(txt_SchemeName.Text) & "', Sur_Name = '" & Trim(Sur) & "', Cetegory_IdNo = " & Str(Val(Cat_Id)) & " Where Scheme_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Scheme_Details where Scheme_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_details
                SNo = 0
                For i = 0 To .RowCount - 1
                    Itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, trans)
                    If Val(Itm_id) <> 0 Then
                        SNo = SNo + 1

                        cmd.Parameters.Clear()
                        If IsDate(dgv_details.Rows(i).Cells(3).Value) Then
                            cmd.Parameters.AddWithValue("@pstartdate", Convert.ToDateTime(dgv_details.Rows(i).Cells(3).Value))
                        Else
                            cmd.Parameters.AddWithValue("@pstartdate", Temp_dttm)
                        End If

                        If IsDate(dgv_details.Rows(i).Cells(4).Value) Then
                            cmd.Parameters.AddWithValue("@penddate", Convert.ToDateTime(dgv_details.Rows(i).Cells(4).Value))
                        Else
                            cmd.Parameters.AddWithValue("@penddate", Temp_dttm)
                        End If

                        If IsDate(dgv_details.Rows(i).Cells(5).Value) Then
                            cmd.Parameters.AddWithValue("@sstartdate", Convert.ToDateTime(dgv_details.Rows(i).Cells(5).Value))
                        Else
                            cmd.Parameters.AddWithValue("@sstartdate", Temp_dttm)
                        End If

                        If IsDate(dgv_details.Rows(i).Cells(6).Value) Then
                            cmd.Parameters.AddWithValue("@senddate", Convert.ToDateTime(dgv_details.Rows(i).Cells(6).Value))
                        Else
                            cmd.Parameters.AddWithValue("@senddate", Temp_dttm)
                        End If

                        cmd.CommandText = "Insert into Scheme_Details(Scheme_IdNo             , sl_no                , Item_IdNo               ,Discount_Percentage                        ,Primary_StartDate , Primary_EndDate ,   Secondary_StartDate , Secondary_EndDate ,Primary_StartDate_Text                 ,   Primary_EndDate_Text                 ,Secondary_StartDate_Text                ,Secondary_EndDate_Text ) " & _
                                                      "values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SNo)) & ", " & Str(Val(Itm_id)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & " ,@pstartdate       , @penddate       ,  @sstartdate          , @senddate         ,'" & Trim(.Rows(i).Cells(3).Value) & "','" & Trim(.Rows(i).Cells(4).Value) & "' ,'" & Trim(.Rows(i).Cells(5).Value) & "' ,'" & Trim(.Rows(i).Cells(6).Value) & "')"
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End With

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_SchemeName.Text)
            Common_Procedures.Master_Return.Master_Type = "SCHEME"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_Scheme_Head") > 0 Then
                MessageBox.Show("Duplicate Scheme Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_SchemeName.Enabled And txt_SchemeName.Visible Then txt_SchemeName.Focus()
        End Try
    End Sub




    'Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FilterClose.Click
    '    Me.Height = 327
    '    pnl_back.Enabled = True
    '    grp_Filter.Visible = False
    'End Sub

    'Private Sub btn_FindOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindOpen.Click
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim movid As Integer

    '    da = New SqlClient.SqlDataAdapter("select Count_IdNo from Count_Head where Count_Name= '" & Trim(cbo_Find.Text) & "'", con)
    '    da.Fill(dt)

    '    movid = 0
    '    If dt.Rows.Count > 0 Then
    '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
    '            movid = Val(dt.Rows(0)(0).ToString)
    '        End If
    '    End If

    '    dt.Dispose()
    '    da.Dispose()

    '    If movid <> 0 Then
    '        move_record(movid)
    '    Else
    '        new_record()
    '    End If

    '    btn_FilterClose_Click(sender, e)
    'End Sub


    'Private Sub btn_FindClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_FindClose.Click

    '    pnl_back.Enabled = True
    '    grp_find.Visible = False
    '    Me.Height = 327
    'End Sub

    'Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
    '    Try
    '        With cbo_Find
    '            If e.KeyValue = 38 And .DroppedDown = False Then
    '                e.Handled = True
    '                'SendKeys.Send("+{TAB}")
    '            ElseIf e.KeyValue = 40 And .DroppedDown = False Then
    '                e.Handled = True
    '                'SendKeys.Send("{TAB}")
    '            ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
    '                .DroppedDown = True
    '            End If
    '        End With

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try
    'End Sub

    'Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim Condt As String
    '    Dim FindStr As String

    '    Try

    '        With cbo_Find

    '            If Asc(e.KeyChar) <> 27 Then

    '                If Asc(e.KeyChar) = 13 Then

    '                    If Trim(.Text) <> "" Then
    '                        If .DroppedDown = True Then
    '                            If Trim(.SelectedText) <> "" Then
    '                                .Text = .SelectedText
    '                            Else
    '                                If .Items.Count > 0 Then
    '                                    .SelectedIndex = 0
    '                                    .SelectedItem = .Items(0)
    '                                    .Text = .GetItemText(.SelectedItem)
    '                                End If
    '                            End If
    '                        End If
    '                    End If

    '                    Call btn_FindOpen_Click(sender, e)

    '                Else

    '                    Condt = ""
    '                    FindStr = ""

    '                    If Asc(e.KeyChar) = 8 Then
    '                        If .SelectionStart <= 1 Then
    '                            .Text = ""
    '                        End If

    '                        If Trim(.Text) <> "" Then
    '                            If .SelectionLength = 0 Then
    '                                FindStr = .Text.Substring(0, .Text.Length - 1)
    '                            Else
    '                                FindStr = .Text.Substring(0, .SelectionStart - 1)
    '                            End If
    '                        End If

    '                    Else
    '                        If .SelectionLength = 0 Then
    '                            FindStr = .Text & e.KeyChar
    '                        Else
    '                            FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
    '                        End If

    '                    End If

    '                    FindStr = LTrim(FindStr)

    '                    If Trim(FindStr) <> "" Then
    '                        Condt = " Where Count_Name like '" & Trim(FindStr) & "%' or Count_Name like '% " & Trim(FindStr) & "%' "
    '                    End If

    '                    da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head " & Condt & " order by Count_Name", con)
    '                    da.Fill(dt)

    '                    .DataSource = dt
    '                    .DisplayMember = "Count_Name"

    '                    .Text = FindStr

    '                    .SelectionStart = FindStr.Length

    '                    e.Handled = True

    '                End If

    '            End If

    '        End With

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    da.Dispose()
    'End Sub

    ''Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
    ''    Call btn_Filteropen_Click(sender, e)
    ''End Sub

    ''Private Sub btn_Filteropen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filteropen.Click
    ''    Dim movid As Integer

    ''    movid = 0
    ''    If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
    ''        movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
    ''    End If

    ''    If Val(movid) <> 0 Then
    ''        move_record(movid)
    ''        btn_FilterClose_Click(sender, e)
    ''    End If
    ''End Sub

    'Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
    '    If e.KeyValue = 13 Then
    '        Call btn_Filteropen_Click(sender, e)
    '    End If
    'End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()

    End Sub

    Private Sub txt_SchemeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SchemeName.KeyDown
        If e.KeyValue = 40 Then
            cbo_CategoryName.Focus()
        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_SchemeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SchemeName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_CategoryName.Focus()
        End If
    End Sub

    
    Private Sub dtp_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 40 Then
            dgv_details.Focus()
            dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)
            dgv_details.CurrentCell.Selected = True

        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            dgv_details.Focus()
            dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)
            dgv_details.CurrentCell.Selected = True
        End If
    End Sub




    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Scheme_Name from Scheme_Head order by Scheme_Name", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Scheme_Name"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        pnl_back.Enabled = False

    End Sub

    Private Sub Price_List_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Price_List_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            '  If grp_filter.Visible Then
            'btn_FilterClose_Click(sender, e)
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)

            Else
                Me.Close()
            End If

        End If


    End Sub
    Private Sub Price_List_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If


    End Sub
    Private Sub Price_List_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        con.Open()
        da = New SqlClient.SqlDataAdapter("select Item_Name from Item_Head order by Item_Name", con)
        da.Fill(dt)
        cbo_ItemName.DataSource = dt
        cbo_ItemName.DisplayMember = "Item_Name"
        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) / 2
        grp_Open.Top = (Me.Height - grp_Open.Height) / 2

        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CategoryName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SchemeName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CategoryName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SchemeName.LostFocus, AddressOf ControlLostFocus

        'grp_Filter.Visible = False
        'grp_Filter.Left = (Me.Width - grp_Filter.Width) - 100
        'grp_Filter.Top = (Me.Height - grp_Filter.Height) - 100
        new_record()
    End Sub

    Private Sub btn_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub
    Private Sub dgv_countdetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellEndEdit
        dgv_pricelistdetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_countdetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        With dgv_details

            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_ItemName.Visible = False Or Val(cbo_ItemName.Tag) <> e.RowIndex Then

                    cbo_ItemName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Item_Name from Item_Head order by Item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_ItemName.DataSource = Dt1
                    cbo_ItemName.DisplayMember = "Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_ItemName.Left = .Left + rect.Left
                    cbo_ItemName.Top = .Top + rect.Top

                    cbo_ItemName.Width = rect.Width
                    cbo_ItemName.Height = rect.Height
                    cbo_ItemName.Text = .CurrentCell.Value

                    cbo_ItemName.Tag = Val(e.RowIndex)
                    cbo_ItemName.Visible = True

                    cbo_ItemName.BringToFront()
                    cbo_ItemName.Focus()

                End If

            Else
                cbo_ItemName.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_pricelistdetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellLeave
        With dgv_details
            If .CurrentCell.ColumnIndex = 2 Then
                .CurrentRow.Cells(2).Value = Format(Val(.CurrentRow.Cells(2).Value), "#########0.00")
            End If
        End With
    End Sub

    Private Sub dgv_countdetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellValueChanged
        On Error Resume Next
        'With dgv_PriceListdetails
        '    If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then
        '        .CurrentRow.Cells(4).Value = 0
        '        If Val(.CurrentRow.Cells(3).Value) <> 0 Then
        '            .CurrentRow.Cells(4).Value = Format(Val(.CurrentRow.Cells(2).Value) / Val(.CurrentRow.Cells(3).Value), "#########0.000")
        '        End If
        '    End If
        'End With
    End Sub

    Private Sub dgv_countdetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_details.EditingControlShowing
        dgtxt_Details = CType(dgv_details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    'Private Sub dgv_countdetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PriceListdetails.KeyDown
    '    On Error Resume Next

    '    With dgv_PriceListdetails
    '        If e.KeyCode = Keys.Up Then
    '            If .CurrentRow.Index = 0 Then
    '                txt_PriceListName.Focus()
    '            End If
    '        End If

    '        If e.KeyCode = Keys.Left Then
    '            If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 1 Then
    '                txt_PriceListName.Focus()
    '            End If
    '        End If

    '        If e.KeyCode = Keys.Enter Then

    '            If .CurrentRow.Index = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
    '                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '                    save_record()
    '                End If

    '            Else
    '                e.SuppressKeyPress = True
    '                e.Handled = True
    '                SendKeys.Send("{Tab}")

    '            End If

    '        End If

    '    End With

    'End Sub

    'Private Sub dgv_PriceListdetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgv_PriceListdetails.KeyPress
    '    On Error Resume Next
    '    ' If dgv_PriceListdetails.CurrentCell.ColumnIndex = 2 Then
    '    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    '    ' End If
    'End Sub



    Private Sub dgv_countdetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_details.RowsAdded

        With dgv_details
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub







    Private Sub cbo_count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.TextChanged
        Try
            If Val(cbo_ItemName.Tag) = Val(dgv_details.CurrentCell.ColumnIndex) Then
                dgv_details.Rows(Me.dgv_details.CurrentCell.RowIndex).Cells.Item(dgv_details.CurrentCell.ColumnIndex).Value = Trim(cbo_ItemName.Text)
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        pnl_back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim movid As Integer

        If Trim(cbo_Open.Text) = "" Then
            MessageBox.Show("Invalid Scheme Name", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Open.Enabled Then cbo_Open.Focus()
            Exit Sub
        End If

        movid = Common_Procedures.Scheme_NameToIdNo(con, cbo_Open.Text)
        If movid <> 0 Then move_record(movid)

        pnl_back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Try
            With cbo_Open
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


    'Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
    '    Dim dgv1 As New DataGridView

    '    On Error Resume Next

    '    If ActiveControl.Name = dgv_PriceListdetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

    '        dgv1 = Nothing

    '        If ActiveControl.Name = dgv_PriceListdetails.Name Then
    '            dgv1 = dgv_PriceListdetails

    '        ElseIf dgv_PriceListdetails.IsCurrentRowDirty = True Then
    '            dgv1 = dgv_PriceListdetails

    '        ElseIf pnl_back.Enabled = True Then
    '            dgv1 = dgv_PriceListdetails

    '        End If

    '        If IsNothing(dgv1) = False Then

    '            With dgv1


    '                If keyData = Keys.Enter Or keyData = Keys.Down Then
    '                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
    '                        If .CurrentCell.RowIndex = .RowCount - 1 Then
    '                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '                                save_record()
    '                            End If
    '                        Else
    '                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

    '                        End If

    '                    Else
    '                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
    '                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '                                save_record()
    '                            End If
    '                        Else
    '                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

    '                        End If
    '                    End If
    '                    Return True

    '                ElseIf keyData = Keys.Up Then

    '                    If .CurrentCell.ColumnIndex <= 1 Then
    '                        If .CurrentCell.RowIndex = 0 Then
    '                            txt_PriceListName.Focus()

    '                        Else
    '                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

    '                        End If

    '                    Else
    '                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

    '                    End If

    '                    Return True

    '                Else
    '                    Return MyBase.ProcessCmdKey(msg, keyData)

    '                End If

    '            End With

    '        'Else

    '        '    Return MyBase.ProcessCmdKey(msg, keyData)

    '        'End If

    '    Else

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    End If

    'End Function
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next


        If ActiveControl.Name = dgv_details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_details.Name Then
                dgv1 = dgv_details

            ElseIf dgv_details.IsCurrentRowDirty = True Then
                dgv1 = dgv_details

            Else
                dgv1 = dgv_details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            txt_SchemeName.Focus()

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


    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Scheme_Head", "Scheme_Name", "", "(Scheme_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)

        End If
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, Nothing, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        With dgv_details

            If (e.KeyValue = 38 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_SchemeName.Focus()


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_ItemName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_SchemeName.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If

    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub
    Private Sub cbo_CategoryName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CategoryName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cetegory_Head", "Cetegory_Name", "", "(Cetegory_IdNo = 0)")
    End Sub

    Private Sub cbo_CategoryName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CategoryName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CategoryName, txt_SchemeName, Nothing, "Cetegory_Head", "Cetegory_Name", "", "(Cetegory_IdNo = 0)")
        If e.KeyValue = 40 Then
            dgv_details.Focus()
            dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)
            dgv_details.CurrentCell.Selected = True

        End If

    End Sub

    Private Sub cbo_CategoryName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CategoryName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CategoryName, Nothing, "Cetegory_Head", "Cetegory_Name", "", "(Cetegory_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            dgv_details.Focus()
            dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)
            dgv_details.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Import.Click
        getExcelData()
    End Sub
    Private Sub getExcelData()
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim BlerDtTm As DateTime
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0

        Try
            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName

            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            dgv_details.Rows.Clear()

            For i = 2 To RowCnt



                ItmId = Common_Procedures.Item_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 1).value))

                n = dgv_details.Rows.Add()
                Sn = Sn + 1

                If ItmId = 0 Then
                    dgv_details.Rows(n).DefaultCellStyle.BackColor = Color.LightSalmon
                End If

                dgv_details.Rows(n).Cells(0).Value = Sn
                dgv_details.Rows(n).Cells(1).Value = Trim(xlWorkSheet.Cells(i, 1).value)
                dgv_details.Rows(n).Cells(2).Value = Val(xlWorkSheet.Cells(i, 2).value)

                dgv_details.Rows(n).Cells(3).Value = Trim(xlWorkSheet.Cells(i, 3).value)
                dgv_details.Rows(n).Cells(4).Value = Trim(xlWorkSheet.Cells(i, 4).value)
                dgv_details.Rows(n).Cells(5).Value = Trim(xlWorkSheet.Cells(i, 5).value)
                dgv_details.Rows(n).Cells(6).Value = Trim(xlWorkSheet.Cells(i, 6).value)








            Next i



            xlWorkBook.Close(False, FileName)
            xlApp.Quit()
          

            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)
           

            MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
           
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub ReleaseComObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub
End Class