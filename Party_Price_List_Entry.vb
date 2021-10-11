Public Class Party_Price_List_Entry

    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private vcmb_SizNm As String

    Private Sub clear()

        pnl_back.Enabled = True
        grp_Open.Visible = False
        ''grp_Filter.Visible = False
        cbo_ItemName.Visible = False
        cbo_Size.Visible = False
        cbo_PartyName.Text = ""
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        dgv_PriceListdetails.Rows.Clear()
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
        If Me.ActiveControl.Name <> cbo_Size.Name Then
            cbo_Size.Visible = False
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
        dgv_PriceListdetails.CurrentCell.Selected = False

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
            cmd.CommandText = "delete from Price_List_Details where Price_List_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            cmd.Connection = con
            cmd.CommandText = "delete from Price_List_Head where Price_List_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()

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

        ''da = New SqlClient.SqlDataAdapter("select a.*, b.count_name as stock_undername from Count_head a LEFT OUTER JOIN count_head b ON a.Count_StockUnder_IdNo = b.count_idno where a.Count_idno = " & Str(Val(idno)), con)
        da = New SqlClient.SqlDataAdapter("select * from Price_List_Head where Price_List_IdNo = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Price_List_IdNo").ToString
            cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0).Item("Ledger_IdNo").ToString))
            da = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name  from Price_List_Details a, Item_Head b where a.Price_List_IdNo = " & Str(Val(idno)) & " and a.Item_idno = b.Item_idno Order by a.sl_no", con)
            da.Fill(dt2)

            dgv_PriceListdetails.Rows.Clear()
            sno = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_PriceListdetails.Rows.Add()

                    sno = sno + 1
                    dgv_PriceListdetails.Rows(n).Cells(0).Value = Val(sno)
                    dgv_PriceListdetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                    dgv_PriceListdetails.Rows(n).Cells(2).Value = Common_Procedures.Size_IdNoToName(con, Val(dt2.Rows(i).Item("Size_IdNo").ToString))
                    dgv_PriceListdetails.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Rate")), "#########0.000")


                Next i

                For i = 0 To dgv_PriceListdetails.RowCount - 1
                    dgv_PriceListdetails.Rows(i).Cells(0).Value = Val(i) + 1


                Next

            End If


        End If
        Grid_Cell_DeSelect()
        dt.Dispose()
        da.Dispose()

        If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
    End Sub
    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Price_List_IdNo) from Price_List_Head Where Price_List_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Price_List_IdNo) from Price_List_Head Where Price_List_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select min(Price_List_IdNo) from Price_List_Head Where Price_List_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and Price_List_IdNo <> 0", con)
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
            da = New SqlClient.SqlDataAdapter("select max(Price_List_IdNo) from Price_List_Head Where Price_List_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Price_List_IdNo <> 0", con)
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

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Price_List_Head", "Price_List_IdNo", "")

        If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
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
        Dim Led_ID As Integer = 0
        Dim Size_ID As Integer = 0
        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_PartyName.Text) = "" Then
            MessageBox.Show("Invalid PriceListName", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(cbo_PartyName.Text))

        With dgv_PriceListdetails
            For i = 0 To dgv_PriceListdetails.RowCount - 1
                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(2).Value) <> 0 Then

                    If Trim(dgv_PriceListdetails.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PriceListdetails.Enabled And dgv_PriceListdetails.Visible Then
                            dgv_PriceListdetails.Focus()
                            dgv_PriceListdetails.CurrentCell = dgv_PriceListdetails.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If
                End If
            Next
        End With
        Led_ID = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_PartyName.Text))

        trans = con.BeginTransaction
        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Price_List_Head", "Price_List_IdNo", "", trans)

                cmd.CommandText = "Insert into Price_List_Head(Price_List_IdNo,Price_List_Name, Ledger_IdNo, Sur_Name) values (" & Str(Val(lbl_IdNo.Text)) & ",'', " & Str(Val(Led_ID)) & ", '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Price_List_Head set Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sur_Name = '" & Trim(Sur) & "' Where Price_List_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Price_List_Details where Price_List_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_PriceListdetails
                SNo = 0
                For i = 0 To .RowCount - 1
                    Itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, trans)
                    Size_ID = Common_Procedures.Size_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value), trans)
                    If Val(Itm_id) <> 0 Then
                        SNo = SNo + 1
                        cmd.CommandText = "Insert into Price_List_Details(Price_List_IdNo, sl_no, Item_IdNo,Size_IdNo,Rate) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SNo)) & ", " & Str(Val(Itm_id)) & ", " & Str(Val(Size_ID)) & "," & Str(Val(.Rows(i).Cells(3).Value)) & ")"
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End With

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(cbo_PartyName.Text)
            Common_Procedures.Master_Return.Master_Type = "PRICELIST"

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_price_list_head") > 0 Then
                MessageBox.Show("Duplicate PriceList Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
        End Try
    End Sub



    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Price_List_Name from Price_List_Head order by Price_List_Name", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Price_List_Name"

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
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus

        'grp_Filter.Visible = False
        'grp_Filter.Left = (Me.Width - grp_Filter.Width) - 100
        'grp_Filter.Top = (Me.Height - grp_Filter.Height) - 100
        new_record()
    End Sub

    Private Sub btn_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_PriceListdetails.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub
    Private Sub dgv_countdetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PriceListdetails.CellEndEdit
        dgv_pricelistdetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_countdetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PriceListdetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        With dgv_PriceListdetails

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
            ElseIf e.ColumnIndex = 2 Then

                If cbo_Size.Visible = False Or Val(cbo_Size.Tag) <> e.RowIndex Then

                    cbo_Size.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Size_Name from Size_Head order by Size_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Size.DataSource = Dt1
                    cbo_Size.DisplayMember = "Size_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Size.Left = .Left + rect.Left
                    cbo_Size.Top = .Top + rect.Top

                    cbo_Size.Width = rect.Width
                    cbo_Size.Height = rect.Height
                    cbo_Size.Text = .CurrentCell.Value

                    cbo_Size.Tag = Val(e.RowIndex)
                    cbo_Size.Visible = True

                    cbo_Size.BringToFront()
                    cbo_Size.Focus()

                End If

            Else
                cbo_ItemName.Visible = False
                cbo_Size.Visible = False
            End If
        End With
    End Sub

    Private Sub dgv_pricelistdetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PriceListdetails.CellLeave
        With dgv_PriceListdetails
            If .CurrentCell.ColumnIndex = 3 Then
                .CurrentRow.Cells(3).Value = Format(Val(.CurrentRow.Cells(3).Value), "#########0.000")
            End If
        End With
    End Sub

    Private Sub dgv_countdetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PriceListdetails.EditingControlShowing
        dgtxt_Details = CType(dgv_PriceListdetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub


    Private Sub dgv_countdetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PriceListdetails.RowsAdded

        With dgv_PriceListdetails
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub cbo_count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.TextChanged
        Try
            If Val(cbo_ItemName.Tag) = Val(dgv_PriceListdetails.CurrentCell.ColumnIndex) Then
                dgv_PriceListdetails.Rows(Me.dgv_PriceListdetails.CurrentCell.RowIndex).Cells.Item(dgv_PriceListdetails.CurrentCell.ColumnIndex).Value = Trim(cbo_ItemName.Text)
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
            MessageBox.Show("Invalid PriceList Name", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Open.Enabled Then cbo_Open.Focus()
            Exit Sub
        End If

        movid = Common_Procedures.Price_List_NameToIdNo(con, cbo_Open.Text)
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


    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next


        If ActiveControl.Name = dgv_PriceListdetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_PriceListdetails.Name Then
                dgv1 = dgv_PriceListdetails

            ElseIf dgv_PriceListdetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PriceListdetails

            Else
                dgv1 = dgv_PriceListdetails

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
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
                            cbo_PartyName.Focus()

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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Price_List_Head", "Price_List_Name", "", "(Price_List_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)

        End If
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, Nothing, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        With dgv_PriceListdetails

            If (e.KeyValue = 38 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    cbo_PartyName.Focus()


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_PriceListdetails
                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_ItemName.Text)

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

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
    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If e.KeyValue = 40 And cbo_PartyName.DroppedDown = False Then
            dgv_PriceListdetails.Focus()
            dgv_PriceListdetails.CurrentCell = dgv_PriceListdetails.Rows(0).Cells(1)
            dgv_PriceListdetails.CurrentCell.Selected = True

        End If
        If e.KeyValue = 38 And cbo_PartyName.DroppedDown = False Then
            SendKeys.Send("+{TAB}")
        End If

    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            dgv_PriceListdetails.Focus()
            dgv_PriceListdetails.CurrentCell = dgv_PriceListdetails.Rows(0).Cells(1)
            dgv_PriceListdetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.GotFocus
        With cbo_Size
            vcmb_SizNm = Trim(.Text)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "size_head", "size_name", "", "(Size_IdNo = 0)")
        End With
    End Sub

    Private Sub cbo_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, Nothing, Nothing, "size_head", "size_name", "", "(size_idno = 0)")

        With dgv_PriceListdetails

            If (e.KeyValue = 38 And cbo_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With
    End Sub

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, Nothing, "size_head", "size_name", "", "(size_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_PriceListdetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_Size.Text)

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
End Class