Imports Excel = Microsoft.Office.Interop.Excel
Public Class Item_Master_Excel
    Implements Interface_MDIActions


    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean

    Private Sub clear()
        Me.Height = 290
        pnl_back.Enabled = True
        grp_Find.Visible = False
        grp_filter.Visible = False
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_Name.Text = ""
        New_Entry = False
    End Sub

    Private Sub Cetegory_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Cetegory_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_filter.Visible Then
                btn_FilterClose_Click(sender, e)
            ElseIf grp_Find.Visible Then
                btn_Open_Click(sender, e)
            Else
                Me.Close()
            End If

        End If

    End Sub

    Private Sub Cetegory_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Width = 520
        grp_Find.Left = 12
        grp_Find.Top = 250
        grp_Find.Visible = False

        grp_filter.Left = 12
        grp_filter.Top = 250
        grp_filter.Visible = False

        If Trim(Common_Procedures.settings.CustomerCode) = "1167" Then ' F fashion
            btn_fromExcel.Visible = True
        End If

        con.Open()
        new_record()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand


        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cetegory_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Cetegory_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            'da = New SqlClient.SqlDataAdapter("select count(*) from item_head where Area_IdNo = " & Str(Val(txt_IdNo.Text)), con)
            'dt = New DataTable
            'da.Fill(dt)
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        If Val(dt.Rows(0)(0).ToString) > 0 Then
            '            MessageBox.Show("Already used this Process", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            Exit Sub
            '        End If
            '    End If
            'End If

            cmd.Connection = con
            cmd.CommandText = "delete from Cetegory_Head where Cetegory_IdNo = " & Str(Val(lbl_IdNo.Text))

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
        Dim da As New SqlClient.SqlDataAdapter("select Cetegory_IdNo, Cetegory_Name from Cetegory_Head where Cetegory_IdNo <> 0 order by Cetegory_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "Cetegory NAME"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_filter.Visible = True

        pnl_back.Enabled = False

        If dgv_filter.Enabled And dgv_filter.Visible Then dgv_filter.Focus()

        Me.Height = 490

        da.Dispose()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub
    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select * from Cetegory_head a where Cetegory_idno = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Cetegory_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Cetegory_Name").ToString
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
            da = New SqlClient.SqlDataAdapter("select min(Cetegory_idno) from Cetegory_head Where Cetegory_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Cetegory_idno) from Cetegory_head Where Cetegory_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(Cetegory_idno) from Cetegory_head Where Cetegory_idno > " & Str(Val(lbl_IdNo.Text)) & " and Cetegory_idno <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Cetegory_idno) from Cetegory_head Where Cetegory_idno < " & Str(Val(lbl_IdNo.Text)) & " and Cetegory_idno <> 0", con)
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

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Cetegory_Head", "Cetegory_IdNo", "")

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Cetegory_Name from Cetegory_Head order by Cetegory_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Cetegory_Name"

        new_record()

        Me.Height = 490
        grp_Find.Visible = True
        pnl_back.Enabled = False
        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim Sur As String

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cetegory_Creation, New_Entry) = False Then Exit Sub

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

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Cetegory_Head", "Cetegory_IdNo", "", trans)

                cmd.CommandText = "Insert into Cetegory_Head(Cetegory_IdNo, Cetegory_Name, Sur_Name) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Cetegory_Head set Cetegory_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "' where Cetegory_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "Cetegory"



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

            'If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            '    If New_Entry = True Then
            '        new_record()
            '    Else
            '        move_record(lbl_IdNo.Text)
            '    End If
            'Else
            '    move_record(lbl_IdNo.Text)
            'End If
            move_record(lbl_IdNo.Text)


        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_Cetegory_head") > 0 Then
                MessageBox.Show("Duplicate Cetegory Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()


        End Try
    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save_record()

    End Sub

    Private Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub btn_FilterClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Height = 290
        pnl_back.Enabled = True
        grp_filter.Visible = False
    End Sub

    Private Sub btn_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Cetegory_IdNo from Cetegory_Head where Cetegory_Name = '" & Trim(cbo_Find.Text) & "'", con)
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

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub btn_FindClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Height = 290
        pnl_back.Enabled = True
        grp_Find.Visible = False
    End Sub

    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cetegory_Head", "Cetegory_Name", "", "")

    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "Cetegory_Head", "Cetegory_Name", "", "")

    End Sub

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "Cetegory_Head", "Cetegory_Name", "", "")
    End Sub

    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Call btn_FilterOpen_Click(sender, e)
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 13 Then
            Call btn_FilterOpen_Click(sender, e)
        End If
    End Sub


    Private Sub btn_FilterOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim movid As Integer

        movid = 0
        If IsDBNull((dgv_filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            btn_FilterClose_Click(sender, e)
        End If
    End Sub

    Private Sub btn_fromExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_fromExcel.Click
        getExcelData()
    End Sub
    Private Sub getExcelData()
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

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
        Dim Cat_Id As Integer = 0
        Dim Sur As String = ""

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


            For i = 2 To RowCnt


                Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 1).value)))

                If Cat_Id <> 0 Then Continue For


                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Cetegory_Head", "Cetegory_IdNo", "")

                Sur = Common_Procedures.Remove_NonCharacters(Trim(xlWorkSheet.Cells(i, 1).value))

                cmd.Connection = con

                cmd.CommandText = "Insert into Cetegory_Head(Cetegory_IdNo, Cetegory_Name, Sur_Name) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(xlWorkSheet.Cells(i, 1).value) & "', '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()


            Next i

            movelast_record()


            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)


            MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

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