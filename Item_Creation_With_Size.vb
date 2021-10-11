Public Class Item_Creation_With_Size
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Dim new_entry As Boolean = False
    Private Sub clear()
        Dim obj As Object
        Dim ctrl As Object
        Dim gbox As GroupBox

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""
            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""
            ElseIf TypeOf obj Is GroupBox Then
                gbox = obj
                For Each ctrl In gbox.Controls
                    If TypeOf ctrl Is TextBox Then
                        ctrl.text = ""
                    ElseIf TypeOf ctrl Is ComboBox Then
                        ctrl.text = ""
                    End If
                Next
            End If
        Next

        cbo_Unit.Text = Common_Procedures.Unit_IdNoToName(con, 1)

        cbo_Size.Visible = False
        cbo_Size.Tag = -100


        grp_Back.Enabled = True
        grp_Filter.Visible = False
        grp_Open.Visible = False
        dgv_details.Rows.Clear()
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

        If Me.ActiveControl.Name <> cbo_Size.Name Then
            cbo_Size.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_details.CurrentCell.Selected = False

    End Sub
    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If
    End Sub
    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim SNo As Integer
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim n As Integer

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da = New SqlClient.SqlDataAdapter("select a.Item_IdNo, a.Item_Name, a.Item_Name_Tamil , a.Item_Code, b.ItemGroup_Name, c.Unit_Name, a.Minimum_Stock, a.Tax_Percentage, a.cost_rate, a.Sale_TaxRate, a.Sales_Rate from Item_Head a LEFT OUTER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo where a.Item_IdNo = " & Str(Val(idno)), con)
            da.Fill(dt)


            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Item_IdNo").ToString) = False Then
                    lbl_IdNo.Text = dt.Rows(0).Item("Item_IdNo").ToString
                    txt_Name.Text = dt.Rows(0).Item("Item_Name").ToString
                    Txt_TamilName.Text = dt.Rows(0).Item("Item_Name_Tamil").ToString
                    txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
                    cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString
                    cbo_Unit.Text = dt.Rows(0).Item("Unit_Name").ToString
                    txt_MinimumStock.Text = dt.Rows(0).Item("Minimum_Stock").ToString
                    txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                    txt_CostRate.Text = dt.Rows(0).Item("Cost_Rate").ToString
                    txt_Rate.Text = dt.Rows(0).Item("Sales_Rate").ToString
                    txt_TaxRate.Text = dt.Rows(0).Item("Sale_TaxRate").ToString
                End If
                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Item_Details a     Where a.Item_Idno = " & Val(lbl_IdNo.Text) & " Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Size_IdNoToName(con, Val(dt2.Rows(i).Item("Size_IdNo").ToString))
                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Piece_Box").ToString), "########0.00")
                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Purchase_Rate").ToString), "########0.00")
                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Sales_Rate").ToString), "########0.00")


                        Next i
                        For i = 0 To .RowCount - 1
                            .Rows(i).Cells(0).Value = Val(i) + 1


                        Next
                    End If

                End With

            End If
            Grid_Cell_DeSelect()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If


        If new_entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select count(*) from Item_Processing_Details where Item_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Item", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        Try
            cmd.Connection = con
            cmd.CommandText = "delete from Item_Head where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Item_Details where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select a.item_idno, a.item_name, b.unit_name, a.Sale_TaxRate from item_head a, unit_head b where a.unit_idno = b.unit_idno Order by a.item_idno", con)
        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt

        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "ITEM NAME"
        dgv_Filter.Columns(2).HeaderText = "UNIT"
        dgv_Filter.Columns(2).HeaderText = "Sales_Rate"

        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 40
        dgv_Filter.Columns(1).FillWeight = 240
        dgv_Filter.Columns(2).FillWeight = 60
        dgv_Filter.Columns(3).FillWeight = 60

        grp_Back.Enabled = False
        grp_Filter.Visible = True

        dgv_Filter.BringToFront()
        dgv_Filter.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(item_idno) from item_head where item_idno <> 0"
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

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select max(item_idno) from item_head", con)
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(item_idno) from item_head where item_idno <> 0 and item_idno > " & Str(Val(lbl_IdNo.Text)), con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(item_idno) from item_head where item_idno <> 0 and item_idno < " & Str(Val(lbl_IdNo.Text))

            dr = cmd.ExecuteReader

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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim newid As Integer = 0

        clear()

        da = New SqlClient.SqlDataAdapter("select max(item_idno) from item_head", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                newid = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        newid = newid + 1

        lbl_IdNo.Text = newid
        lbl_IdNo.ForeColor = Color.Red

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt)

        'cbo_Open.Items.Clear()

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "item_name"

        grp_Open.Visible = True
        grp_Back.Enabled = False
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim new_entry As Boolean = False
        Dim nr As Long = 0
        Dim itmgrp_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim da1 As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim sno As Integer
        Dim Sz_idno As Integer = 0

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1051" Then

            If Trim(txt_Code.Text) = "" Then
                MessageBox.Show("Invalid Item cODE", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_Code.Enabled Then txt_Code.Focus()
                Exit Sub
            End If

            If Trim(UCase(txt_Code.Text)) <> "" Then
                da1 = New SqlClient.SqlDataAdapter("select a.* from item_head a where a.item_code = '" & Trim(txt_Code.Text) & "'", con)
                dt1 = New DataTable
                da1.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    If lbl_IdNo.Text <> dt1.Rows(0)("Item_IdNo").ToString Then
                        MessageBox.Show("Duplicate Item Code", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If txt_Code.Enabled Then txt_Code.Focus()
                        Exit Sub
                    End If
                End If
                dt1.Dispose()
                da1.Dispose()
            End If

        End If

        da = New SqlClient.SqlDataAdapter("select itemgroup_idno from itemgroup_head where itemgroup_name = '" & Trim(cbo_ItemGroup.Text) & "'", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                itmgrp_id = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        dt.Clear()

        da = New SqlClient.SqlDataAdapter("select unit_idno from unit_head where unit_name = '" & Trim(cbo_Unit.Text) & "'", con)
        da.Fill(dt2)

        unt_id = 0
        If dt2.Rows.Count > 0 Then
            If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                unt_id = Val(dt2.Rows(0)(0).ToString)
            End If
        End If

        If Val(unt_id) = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = tr
            cmd.CommandText = "update item_head set Item_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(txt_Name.Text) & "', Item_Code = '" & Trim(txt_Code.Text) & "', Item_Name_Tamil = '" & Trim(Txt_TamilName.Text) & "' , ItemGroup_IdNo = " & Str(Val(itmgrp_id)) & ", Unit_IdNo = " & Str(Val(unt_id)) & ", Minimum_Stock = " & Str(Val(txt_MinimumStock.Text)) & ", Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Sale_TaxRate = " & Str(Val(txt_TaxRate.Text)) & ", Sales_Rate = " & Str(Val(txt_Rate.Text)) & ", Cost_Rate = " & Str(Val(txt_CostRate.Text)) & " where Item_IdNo = " & Str(Val(lbl_IdNo.Text))

            nr = cmd.ExecuteNonQuery

            If nr = 0 Then
                cmd.CommandText = "Insert into item_head(Item_IdNo, Item_Name, Sur_Name, Item_Code, ItemGroup_IdNo, Unit_IdNo, Minimum_Stock, Tax_Percentage, Sale_TaxRate, Sales_Rate, Cost_Rate ,Item_Name_Tamil ) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Code.Text) & "', " & Str(Val(itmgrp_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(txt_MinimumStock.Text)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(txt_TaxRate.Text)) & ", " & Str(Val(txt_Rate.Text)) & ", " & Str(Val(txt_CostRate.Text)) & " , '" & Trim(Txt_TamilName.Text) & "' )"
                cmd.ExecuteNonQuery()
                new_entry = True
            End If
            cmd.CommandText = "delete from Item_Details where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()




            With dgv_details

                Sno = 0


                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1
                        Sz_idno = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Item_Details (      Item_IdNo          ,                           Sl_No         ,       Size_IdNo      ,   Piece_Box                            ,            Purchase_Rate                 ,         Sales_rate                     ) " & _
                                                "     Values                  (   " & Val(lbl_IdNo.Text) & "    , " & Str(Val(sno)) & ", " & Val(Sz_idno) & " ," & Str(Val(.Rows(i).Cells(2).Value)) & " , " & Str(Val(.Rows(i).Cells(3).Value)) & " ," & Str(Val(.Rows(i).Cells(4).Value)) & "  ) "
                        cmd.ExecuteNonQuery()


                    End If

                Next

            End With

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "ITEM"

            If new_entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Private Sub Item_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemGroup.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemGroup.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Item_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                Call btn_CloseFilter_Click(sender, e)
                Exit Sub
            End If
            If grp_Open.Visible Then
                Call btnClose_Click(sender, e)
                Exit Sub
            End If
            Me.Close()
        End If
    End Sub

    Private Sub Item_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable


        If Trim(Common_Procedures.settings.CustomerCode) = "1003" Then
            Me.Text = "COUNT CREATION"
            Label7.Text = "COUNT CREATION"
            Label3.Text = "Item Description"
        End If

        con.Open()

        da = New SqlClient.SqlDataAdapter("select itemgroup_name from itemgroup_head order by itemgroup_name", con)
        da.Fill(dt1)

        cbo_ItemGroup.Items.Clear()

        cbo_ItemGroup.DataSource = dt1
        cbo_ItemGroup.DisplayMember = "itemgroup_name"
        'cbo_ItemGroup.ValueMember = "itemgroup_idno"

        da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)

        da.Fill(dt2)

        cbo_Unit.DataSource = dt2
        cbo_Unit.DisplayMember = "unit_name"
        'cbo_Unit.ValueMember = "unit_idno"

        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) / 2
        grp_Open.Top = (Me.Height - grp_Open.Height) / 2

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) / 2
        grp_Filter.Top = (Me.Height - grp_Filter.Height) / 2

        new_record()

    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        cbo_Open.BackColor = Color.LemonChiffon
        cbo_Open.ForeColor = Color.Blue
        cbo_Open.DroppedDown = True
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim Indx As Integer
        Dim FindStr As String

        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Open.SelectionStart <= 1 Then
                cbo_Open.Text = ""
                Exit Sub
            End If

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.Text.Length - 1)
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart - 1)
            End If

        Else

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text & e.KeyChar
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Open.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Open.SelectedText = ""
            cbo_Open.SelectedIndex = Indx
            cbo_Open.SelectionStart = FindStr.Length
            cbo_Open.SelectionLength = cbo_Open.Text.Length
        End If

        e.Handled = True

    End Sub

    Private Sub txt_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.GotFocus
        txt_Name.BackColor = Color.Lime
        txt_Name.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select item_idno from item_head where item_name = '" & Trim(cbo_Open.Text) & "'", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then
                move_record(movid)
                btnClose_Click(sender, e)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'Me.Height = 400

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        grp_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub cbo_ItemGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ItemGroup_Head", "ItemGroup_Name", " ", "(ItemGroup_IdNo = 0)")
        cbo_ItemGroup.BackColor = Color.Lime
        cbo_ItemGroup.ForeColor = Color.Blue
        cbo_ItemGroup.SelectionStart = 0
        cbo_ItemGroup.SelectionLength = cbo_ItemGroup.Text.Length
    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, Nothing, Nothing, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")
        If e.KeyValue = 38 And cbo_ItemGroup.DroppedDown = False Then
            e.Handled = True
            txt_Code.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_ItemGroup.DroppedDown = False Then
            e.Handled = True
            cbo_Unit.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_ItemGroup.DroppedDown = False Then
            cbo_ItemGroup.DroppedDown = True
        End If
    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress

        Dim Indx As Integer = -1
        Dim strFindStr As String = ""
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, Nothing, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")
        Try
            If Asc(e.KeyChar) = 8 Then
                If cbo_ItemGroup.SelectionStart <= 1 Then
                    cbo_ItemGroup.Text = ""
                    Exit Sub
                End If
                If cbo_ItemGroup.SelectionLength = 0 Then
                    strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.Text.Length - 1)
                Else
                    strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.SelectionStart - 1)
                End If

            Else

                If cbo_ItemGroup.SelectionLength = 0 Then
                    strFindStr = cbo_ItemGroup.Text & e.KeyChar
                Else
                    strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.SelectionStart) & e.KeyChar
                End If

            End If

            Indx = cbo_ItemGroup.FindString(strFindStr)

            If Indx <> -1 Then
                cbo_ItemGroup.SelectedText = ""
                cbo_ItemGroup.SelectedIndex = Indx
                cbo_ItemGroup.SelectionStart = strFindStr.Length
                cbo_ItemGroup.SelectionLength = cbo_ItemGroup.Text.Length
                e.Handled = True
            Else
                e.Handled = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        cbo_Unit.BackColor = Color.Lime
        cbo_Unit.ForeColor = Color.Blue
        cbo_Unit.SelectionStart = 0
        cbo_Unit.SelectionLength = cbo_Unit.Text.Length
    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        If e.KeyValue = 38 And cbo_Unit.DroppedDown = False Then
            e.Handled = True
            cbo_ItemGroup.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_Unit.DroppedDown = False Then
            e.Handled = True
            txt_MinimumStock.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_Unit.DroppedDown = False Then
            cbo_Unit.DroppedDown = True
        End If
    End Sub

    Private Sub txt_TaxPerc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.GotFocus
        txt_TaxPerc.BackColor = Color.Lime
        txt_TaxPerc.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_VatPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            If dgv_details.RowCount > 0 Then

                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)
            End If
        End If

    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If dgv_details.Rows.Count > 0 Then
                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click
        Dim movid As Integer = 0

        Try
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)

            If Val(movid) <> 0 Then
                move_record(movid)
                grp_Back.Enabled = True
                grp_Filter.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        grp_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Rate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.GotFocus
        txt_Rate.BackColor = Color.Lime
        txt_Rate.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
    End Sub

    Private Sub txt_TaxRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxRate.GotFocus
        txt_TaxRate.BackColor = Color.Lime
        txt_TaxRate.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyUp
        txt_Rate.Text = Format(Val(txt_TaxRate.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "########0.00")
    End Sub

    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            txt_TamilName.Focus()
        End If
    End Sub

    Private Sub txt_Code_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Code.GotFocus
        txt_Code.BackColor = Color.Lime
        txt_Code.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_Code_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Code.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Code_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Code.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_CostRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CostRate.GotFocus
        txt_CostRate.BackColor = Color.Lime
        txt_CostRate.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_CostRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CostRate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CostRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CostRate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Dim FindStr As String = ""
        Dim Indx As Integer = -1

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Unit.SelectionStart <= 1 Then
                cbo_Unit.Text = ""
                Exit Sub
            End If

            If cbo_Unit.SelectionLength = 0 Then
                FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.Text.Length - 1)
            Else
                FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.SelectionStart - 1)
            End If

        Else
            If cbo_Unit.SelectionLength = 0 Then
                FindStr = cbo_Unit.Text & e.KeyChar
            Else
                FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Unit.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Unit.SelectedText = ""
            cbo_Unit.SelectedIndex = Indx
            cbo_Unit.SelectionStart = FindStr.Length
            cbo_Unit.SelectionLength = cbo_Unit.Text.Length
        End If
        e.Handled = True

    End Sub

    Private Sub txt_TaxRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyDown
        If e.KeyCode = 40 Then
            txt_TamilName.Focus()
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.LostFocus
        txt_Name.BackColor = Color.White
        txt_Name.ForeColor = Color.Black
    End Sub

    Private Sub cbo_ItemGroup_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ItemGroup_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemGroup.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ItemGroup_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.LostFocus
        cbo_ItemGroup.BackColor = Color.White
        cbo_ItemGroup.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Open_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.LostFocus
        cbo_Open.BackColor = Color.White
        cbo_Open.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Unit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.LostFocus
        cbo_Unit.BackColor = Color.White
        cbo_Unit.ForeColor = Color.Black
    End Sub

    Private Sub txt_Code_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Code.LostFocus
        txt_Code.BackColor = Color.White
        txt_Code.ForeColor = Color.Black
    End Sub

    Private Sub txt_CostRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CostRate.LostFocus
        txt_CostRate.BackColor = Color.White
        txt_CostRate.ForeColor = Color.Black
    End Sub

    Private Sub txt_Rate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.LostFocus
        txt_Rate.BackColor = Color.White
        txt_Rate.ForeColor = Color.Black
    End Sub

    Private Sub txt_TaxPerc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.LostFocus
        txt_TaxPerc.BackColor = Color.White
        txt_TaxPerc.ForeColor = Color.Black
    End Sub

    Private Sub txt_TaxRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxRate.LostFocus
        txt_TaxRate.BackColor = Color.White
        txt_TaxRate.ForeColor = Color.Black
    End Sub

    Private Sub txt_MinimumStock_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MinimumStock.GotFocus
        txt_MinimumStock.BackColor = Color.Lime
        txt_MinimumStock.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_MinimumStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MinimumStock.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_MinimumStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumStock.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_TamilName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TamilName.GotFocus
        txt_TamilName.BackColor = Color.Lime
        txt_TamilName.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_TamilName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TamilName.KeyDown
        If e.KeyCode = 40 Then
            If dgv_details.RowCount > 0 Then

                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)
            End If
        End If
        If e.KeyCode = 38 Then txt_TaxRate.Focus()
    End Sub

    Private Sub txt_TamilName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TamilName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_details.Rows.Count > 0 Then
                dgv_details.Focus()
                dgv_details.CurrentCell = dgv_details.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub txt_TamilName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TamilName.LostFocus
        txt_TamilName.BackColor = Color.White
        txt_TamilName.ForeColor = Color.Black
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub
    Private Sub dgv_details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
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
                cbo_Size.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellLeave
        With dgv_details
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_details.CellValueChanged
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

    Private Sub dgv_details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_details.EditingControlShowing
        dgtxt_Details = CType(dgv_details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_details.LostFocus
        On Error Resume Next
        dgv_details.CurrentCell.Selected = False
    End Sub
    Private Sub dgv_details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_details.RowsAdded

        With dgv_details
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Size_Head", "Size_Name", " ", "(Size_idno = 0)")
    End Sub


    Private Sub cbo_Size_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.TextChanged
        Try
            If Val(cbo_Size.Tag) = Val(dgv_details.CurrentCell.ColumnIndex) Then
                dgv_details.Rows(Me.dgv_details.CurrentCell.RowIndex).Cells.Item(dgv_details.CurrentCell.ColumnIndex).Value = Trim(cbo_Size.Text)
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub cbo_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, Nothing, Nothing, "Size_Head", "Size_Name", "", "(Size_idno = 0)")

        With dgv_details

            If (e.KeyValue = 38 And cbo_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_TamilName.Focus()


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(4)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

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

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, Nothing, "Size_Head", "Size_Name", "", "(Size_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Size.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_TamilName.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

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
                            txt_TamilName.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(4)

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


    Private Sub txt_MinimumStock_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MinimumStock.LostFocus
        txt_MinimumStock.BackColor = Color.White
        txt_MinimumStock.ForeColor = Color.Black
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            save_record()
        Else
            txt_Name.Focus()
        End If
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub
End Class