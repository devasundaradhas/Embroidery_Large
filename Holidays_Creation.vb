Public Class Holidays_Creation

    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Public previlege As String

    Private Sub clear()

        dgv_Details.Rows.Clear()

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

   
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("D") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim cmd As New SqlClient.SqlCommand
        Dim Nr As Integer = 0

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Holiday_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Holiday_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        Try


            cmd.Connection = con

            cmd.CommandText = "delete from Holiday_Details where Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.Dispose()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            move_record(1)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            '    With dgv_Details

            '        For i = 0 To .RowCount - 1

            '            If dgv_Details.Enabled And dgv_Details.Visible Then
            '                dgv_Details.Focus()
            '                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
            '            End If
            '        Next
            '    End With
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
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim sno As Integer, n As Integer


        '  If Val(idno) = 0 Then Exit Sub

        clear()
    
     

        da = New SqlClient.SqlDataAdapter("select * from Holiday_Details  Order by sl_no", con)
            da.Fill(dt2)

            dgv_Details.Rows.Clear()
            sno = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Details.Rows.Add()

                    sno = sno + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = Format(dt2.Rows(i).Item("HolidayDateTime"), "dd/MM/yyyy")
                    dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Reason").ToString

                Next i

                For i = 0 To dgv_Details.RowCount - 1
                    dgv_Details.Rows(i).Cells(0).Value = Val(i) + 1


                Next

            End If

        dt2.Dispose()
        da.Dispose()

        With dgv_Details

            For i = 0 To .RowCount - 1

                If dgv_Details.Enabled And dgv_Details.Visible Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                End If
            Next
        End With
    End Sub
    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim movid As Integer = 0

        'Try
        '    da = New SqlClient.SqlDataAdapter("select min(Holiday_IdNo) from Holiday_Head Where Holiday_IdNo <> 0", con)
        '    da.Fill(dt)

        '    movid = 0
        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            movid = Val(dt.Rows(0)(0).ToString)
        '        End If
        '    End If

        '    If Val(movid) <> 0 Then move_record(movid)

        '    dt.Dispose()
        '    da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim movid As Integer = 0

        'Try
        '    da = New SqlClient.SqlDataAdapter("select max(Holiday_IdNo) from Holiday_Details Where Holiday_IdNo <> 0", con)
        '    da.Fill(dt)

        '    movid = 0
        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            movid = Val(dt.Rows(0)(0).ToString)
        '        End If
        '    End If

        '    If Val(movid) <> 0 Then move_record(movid)

        '    dt.Dispose()
        '    da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim movid As Integer = 0

        'Try
        '    da = New SqlClient.SqlDataAdapter("select min(Holiday_IdNo) from Holiday_Head Where Holiday_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Holiday_IdNo <> 0", con)
        '    da.Fill(dt)

        '    movid = 0
        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            movid = Val(dt.Rows(0)(0).ToString)
        '        End If
        '    End If

        '    If Val(movid) <> 0 Then move_record(movid)

        '    dt.Dispose()
        '    da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim movid As Integer = 0

        'Try
        '    da = New SqlClient.SqlDataAdapter("select max(Holiday_IdNo) from Holiday_Head Where Holiday_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Holiday_IdNo <> 0", con)
        '    da.Fill(dt)

        '    movid = 0
        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            movid = Val(dt.Rows(0)(0).ToString)
        '        End If
        '    End If

        '    If Val(movid) <> 0 Then move_record(movid)

        '    dt.Dispose()
        '    da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record



        ' New_Entry = True
        'lbl_IdNo.ForeColor = Color.Red

        'lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Holiday_Head", "Holiday_IdNo", "")

        'With dgv_Details

        '    For i = 0 To .RowCount - 1

        '        If dgv_Details.Enabled And dgv_Details.Visible Then
        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
        '        End If
        '    Next
        'End With
    End Sub

    

    Public Sub print_record() Implements Interface_MDIActions.print_record

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
        Dim Sur As String = ""
        Dim SNo As Integer = 0
        Dim Nr As Integer = 0
        Dim vDttm As Date

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Holiday_Creation, New_Entry) = False Then Exit Sub




        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "delete from Holiday_Details where Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            With dgv_Details
                SNo = 0
                For i = 0 To .RowCount - 1

                    SNo = SNo + 1

                    If .Rows(i).Cells(1).Value <> "" And .Rows(i).Cells(2).Value <> "" Then

                        If IsDate(.Rows(i).Cells(1).Value.ToString) = True Then
                            vDttm = .Rows(i).Cells(1).Value.ToString
                            cmd.Parameters.Clear()
                            cmd.Parameters.AddWithValue("@Date", vDttm)

                            cmd.CommandText = "Insert into Holiday_Details(Year_Code , Sl_No ,HolidayDateTime , Holiday_Date, Reason) values ('" & Trim(Common_Procedures.FnYearCode) & "', " & Str(Val(SNo)) & ", @Date ,'" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "' )"
                            cmd.ExecuteNonQuery()

                        Else

                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                            MessageBox.Show("Invalid Date", "DOES NOT SAVE....", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            trans.Rollback()
                            Exit Sub

                        End If


                    End If

                Next

            End With

            ' End If





            trans.Commit()

            'Common_Procedures.Master_Return.Return_Value = Trim(txt_MillName.Text)
            'Common_Procedures.Master_Return.Master_Type = "Mill"

            ' If New_Entry = True Then new_record()
            move_record(0)
            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_holiday_details") > 0 Then
                MessageBox.Show("Duplicate Holiday Date", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            'With dgv_Details

            '    For i = 0 To .RowCount - 1

            '        If dgv_Details.Enabled And dgv_Details.Visible Then
            '            dgv_Details.Focus()
            '            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
            '        End If
            '    Next
            'End With
        End Try
    End Sub






    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
      

    End Sub

    Private Sub Holiday_Details_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Holiday_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

               
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub

                    Else
                    Me.Close()

                    End If

                End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Holiday_Details_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        con.Open()
        
        move_record(0)
    End Sub

    'Private Sub btn_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
    '    save_record()
    'End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub
    

    Private Sub dgv_countdetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        With dgv_Details

            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If

           

        End With
    End Sub

   
    Private Sub dgv_details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next


        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

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


                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

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
    



    Private Sub dgv_countdetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded

        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try

            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
End Class