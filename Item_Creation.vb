
Imports Excel = Microsoft.Office.Interop.Excel

Public Class Item_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

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

        chk_JobWorkStatus.Checked = False
        chk_DefaultItem.Checked = False
        ' cbo_Unit.Text = Common_Procedures.Unit_IdNoToName(con, 1)

        grp_Back.Enabled = True
        grp_Filter.Visible = False
        grp_Open.Visible = False
        chk_JobWorkStatus.Checked = False
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da = New SqlClient.SqlDataAdapter("select a.Item_IdNo, a.Item_Name,a.Item_Description , a.Item_Name_Tamil ,a.MRP_Rate , a.Item_Code,A.ISDEFAULT_ITEM_FOR_AUTO_BILL, b.ItemGroup_Name, c.Unit_Name, a.Minimum_Stock, a.Tax_Percentage, a.cost_rate, a.Sale_TaxRate, a.Sales_Rate ,a.Job_Work_Status ,a.Gst_Percentage , a.Gst_Rate from Item_Head a LEFT OUTER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo where a.Item_IdNo = " & Str(Val(idno)), con)
            da.Fill(dt)


            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Item_IdNo").ToString) = False Then
                    txt_IdNo.Text = dt.Rows(0).Item("Item_IdNo").ToString
                    txt_Name.Text = dt.Rows(0).Item("Item_Name").ToString
                    txt_deccription.Text = dt.Rows(0).Item("Item_Description").ToString
                    txt_TamilName.Text = dt.Rows(0).Item("Item_Name_Tamil").ToString

                    txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
                    cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString
                    cbo_Unit.Text = dt.Rows(0).Item("Unit_Name").ToString
                    txt_MinimumStock.Text = dt.Rows(0).Item("Minimum_Stock").ToString
                    txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                    txt_CostRate.Text = dt.Rows(0).Item("Cost_Rate").ToString
                    txt_Rate.Text = dt.Rows(0).Item("Sales_Rate").ToString
                    txt_Mrp.Text = dt.Rows(0).Item("MRP_Rate").ToString
                    txt_TaxRate.Text = dt.Rows(0).Item("Sale_TaxRate").ToString
                    txt_GSTTaxPerc.Text = dt.Rows(0).Item("Gst_Percentage").ToString
                    txt_GSTRate.Text = dt.Rows(0).Item("Gst_Rate").ToString

                    If Val(dt.Rows(0).Item("Job_Work_Status").ToString) = 1 Then
                        chk_JobWorkStatus.Checked = True
                    Else
                        chk_JobWorkStatus.Checked = False
                    End If

                    If Not IsDBNull(dt.Rows(0).Item("ISDEFAULT_ITEM_FOR_AUTO_BILL")) Then
                        'MsgBox(dt.Rows(0).Item("ISDEFAULT_ITEM_FOR_AUTO_BILL"))
                        If (dt.Rows(0).Item("ISDEFAULT_ITEM_FOR_AUTO_BILL")) = True Then
                            chk_DefaultItem.Checked = True
                        Else
                            chk_DefaultItem.Checked = False
                        End If
                    End If

                End If
            End If

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

        da = New SqlClient.SqlDataAdapter("select count(*) from Item_Processing_Details where  Item_IdNo = " & Str(Val(txt_IdNo.Text)) & " and Quantity <> 0 ", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count <> 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Item", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        Try
            cmd.Connection = con
            cmd.CommandText = "delete from Item_Head where Item_IdNo = " & Str(Val(txt_IdNo.Text))
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
            da = New SqlClient.SqlDataAdapter("select min(item_idno) from item_head where item_idno <> 0 and item_idno > " & Str(Val(txt_IdNo.Text)), con)
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
            cmd.CommandText = "select max(item_idno) from item_head where item_idno <> 0 and item_idno < " & Str(Val(txt_IdNo.Text))

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

        txt_IdNo.Text = newid
        txt_IdNo.ForeColor = Color.Red

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
        Dim Job_Sts As Integer = 0



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
                    If txt_IdNo.Text <> dt1.Rows(0)("Item_IdNo").ToString Then
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

        Job_Sts = 0
        If chk_JobWorkStatus.Checked = True Then
            Job_Sts = 1
        End If

        tr = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = tr
            cmd.CommandText = "update item_head set Item_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(txt_Name.Text) & "',Item_Description = '" & Trim(txt_deccription.Text) & "' ,Item_Code = '" & Trim(txt_Code.Text) & "', Item_Name_Tamil = '" & Trim(txt_TamilName.Text) & "' , ItemGroup_IdNo = " & Str(Val(itmgrp_id)) & ", Unit_IdNo = " & Str(Val(unt_id)) & ", Minimum_Stock = " & Str(Val(txt_MinimumStock.Text)) & ", Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Sale_TaxRate = " & Str(Val(txt_TaxRate.Text)) & ", Sales_Rate = " & Str(Val(txt_Rate.Text)) & ", Cost_Rate = " & Str(Val(txt_CostRate.Text)) & " , MRP_Rate =  " & Str(Val(txt_Mrp.Text)) & " ,Job_Work_Status = " & Val(Job_Sts) & " ,Gst_Percentage = " & Val(txt_GSTTaxPerc.Text) & " ,Gst_Rate =" & Val(txt_GSTRate.Text) & " where Item_IdNo = " & Str(Val(txt_IdNo.Text))

            nr = cmd.ExecuteNonQuery

            If nr = 0 Then
                cmd.CommandText = "Insert into item_head(Item_IdNo, Item_Name, Sur_Name, Item_Code, ItemGroup_IdNo, Unit_IdNo, Minimum_Stock, Tax_Percentage, Sale_TaxRate, Sales_Rate, Cost_Rate ,Item_Name_Tamil ,MRP_Rate,Job_Work_Status ,Gst_Percentage ,Gst_Rate,Item_Description ) values (" & Str(Val(txt_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Code.Text) & "', " & Str(Val(itmgrp_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(txt_MinimumStock.Text)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(txt_TaxRate.Text)) & ", " & Str(Val(txt_Rate.Text)) & ", " & Str(Val(txt_CostRate.Text)) & " , '" & Trim(txt_TamilName.Text) & "', " & Str(Val(txt_Mrp.Text)) & "," & Val(Job_Sts) & " ," & Val(txt_GSTTaxPerc.Text) & " ," & Val(txt_GSTRate.Text) & ",'" & Trim(txt_deccription.Text) & "')"
                cmd.ExecuteNonQuery()
                new_entry = True
            End If

            cmd.CommandText = "UPDATE ITEM_HEAD SET ISDEFAULT_ITEM_FOR_AUTO_BILL = 1 WHERE ITEM_IDNO = " & Val(txt_IdNo.Text).ToString
            cmd.ExecuteNonQuery()

            cmd.CommandText = "UPDATE ITEM_HEAD SET ISDEFAULT_ITEM_FOR_AUTO_BILL = 0 WHERE NOT ITEM_IDNO = " & Val(txt_IdNo.Text).ToString
            cmd.ExecuteNonQuery()

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
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemGroup.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_ItemGroup.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
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

        If Common_Procedures.UserRight_Check_1(Me.Name, Common_Procedures.OperationType.Open) = False Then
            MsgBox("This User Is Restircetd From Opening The Form " & Me.Text)
            Me.Close()
        End If

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable


        If Trim(Common_Procedures.settings.CustomerCode) = "1003" Then
            Me.Text = "COUNT CREATION"
            Label7.Text = "COUNT CREATION"
            Label3.Text = "Item Description"
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1219" Then ' vels enterprises
            txt_deccription.Visible = True
            btn_fromExcel.Visible = True
            txt_TamilName.Visible = False
            lbl_tamilname.Text = "Description"
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

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) '- 200
        grp_Open.Top = (Me.Height - grp_Open.Height) - 20

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 50
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 50

        new_record()

        chk_JobWorkStatus.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2001" Then  'Deva
            chk_JobWorkStatus.Visible = True
            txt_Code.Width = 290
        End If

    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        cbo_Open.BackColor = Color.Lime
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

    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown
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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, Nothing, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")
        Dim Indx As Integer = -1
        Dim strFindStr As String = ""

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_Name", "", "(Unit_IdNo= 0)")
        cbo_Unit.BackColor = Color.Lime
        cbo_Unit.ForeColor = Color.Blue
        cbo_Unit.SelectionStart = 0
        cbo_Unit.SelectionLength = cbo_Unit.Text.Length

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, Nothing, Nothing, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
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
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
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
        txt_GSTRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_GSTTaxPerc.Text)) / 100), "##########0.00")
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
            txt_GSTTaxPerc.Focus()
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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, Nothing, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
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
            txt_Mrp.Focus()
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
        If e.Control = False And e.KeyValue = 17 Then
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
    Private Sub txt_deccription_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_deccription.GotFocus
        txt_deccription.BackColor = Color.Lime
        txt_deccription.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_TamilName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TamilName.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
        If e.KeyCode = 38 Then txt_Mrp.Focus()
    End Sub

    Private Sub txt_TamilName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TamilName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_deccription_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_deccription.LostFocus
        txt_deccription.BackColor = Color.White
        txt_deccription.ForeColor = Color.Black
    End Sub


    Private Sub txt_deccription_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_deccription.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
        If e.KeyCode = 38 Then txt_Mrp.Focus()
    End Sub

    Private Sub txt_deccription_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_deccription.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
    End Sub




    Private Sub txt_Mrp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Mrp.GotFocus
        txt_Mrp.BackColor = Color.Lime
        txt_Mrp.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_Mrp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Mrp.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Mrp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Mrp.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Mrp_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Mrp.LostFocus
        txt_Mrp.BackColor = Color.White
        txt_Mrp.ForeColor = Color.Black
    End Sub


    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_GSTTaxPerc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GSTTaxPerc.GotFocus
        txt_GSTTaxPerc.BackColor = Color.Lime
        txt_GSTTaxPerc.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub
    Private Sub txt_GSTTaxPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTTaxPerc.KeyDown
        If e.KeyCode = 40 Then
            txt_GSTRate.Focus()
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub
    Private Sub txt_GSTTaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GSTTaxPerc.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            txt_GSTRate.Focus()
        End If
    End Sub

    Private Sub txt_GSTTaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTTaxPerc.KeyUp
        ' txt_GSTRate.Text = Format(Val(txt_GSTRate.Text) * (100 / (100 + Val(txt_GSTTaxPerc.Text))), "#########0.00")
        txt_GSTRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_GSTTaxPerc.Text)) / 100), "########0.00")
    End Sub
    Private Sub txt_GSTTaxPerc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GSTTaxPerc.LostFocus
        txt_GSTTaxPerc.BackColor = Color.White
        txt_GSTTaxPerc.ForeColor = Color.Black
    End Sub

    Private Sub txt_GSTRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GSTRate.GotFocus
        txt_GSTRate.BackColor = Color.Lime
        txt_GSTRate.ForeColor = Color.Blue
        SendKeys.Send("{HOME}+{END}")
    End Sub

    Private Sub txt_GSTRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTRate.KeyDown
        If e.KeyCode = 40 Then
            txt_Mrp.Focus()
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub
    Private Sub txt_GSTRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GSTRate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            txt_Mrp.Focus()
        End If
    End Sub

    Private Sub txt_GSTRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTRate.KeyUp
        txt_Rate.Text = Format(Val(txt_GSTRate.Text) * (100 / (100 + Val(txt_GSTTaxPerc.Text))), "#########0.00")
    End Sub

    Private Sub txt_GSTRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GSTRate.LostFocus
        txt_GSTRate.BackColor = Color.White
        txt_GSTRate.ForeColor = Color.Black
    End Sub

    Private Sub cbo_ItemGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.TextChanged
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmGrpIdNo As Integer = 0

        ItmGrpIdNo = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(cbo_ItemGroup.Text)))
        If ItmGrpIdNo = 0 Then Exit Sub

        Try

            da = New SqlClient.SqlDataAdapter("select Item_HSN_Code, Item_GST_Percentage from ItemGroup_Head a  where ItemGroup_IdNo = " & Str(Val(ItmGrpIdNo)) & "", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Item_GST_Percentage").ToString) = False Then
                    txt_GSTTaxPerc.Text = dt.Rows(0).Item("Item_GST_Percentage").ToString
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub btn_fromExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_fromExcel.Click
        Dim FileName As String = ""
        Dim Sts1 As Boolean = False
        Dim Sts2 As Boolean = False
        Dim Sts3 As Boolean = False
        Try

            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName


            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'getExcelData_Category(FileName, Sts1)
            'getExcelData_ItemGroup(FileName, Sts2)
            'getExcelData_ItemName(FileName, Sts3)

            If Sts1 = True And Sts2 = True And Sts3 = True Then
                MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Error on Import", "FOR IMPORTING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    'Private Sub getExcelData_ItemName(ByVal FileName As String, ByRef Sts As Boolean)
    '    Dim cmd As New SqlClient.SqlCommand
    '    'Dim tr As SqlClient.SqlTransaction
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable

    '    Dim xlApp As Excel.Application
    '    Dim xlWorkBook As Excel.Workbook
    '    Dim xlWorkSheet As Excel.Worksheet

    '    Dim RowCnt As Long = 0
    '    '  Dim FileName As String = ""
    '    Dim NewCode As String = ""
    '    Dim BlerDtTm As DateTime
    '    Dim ShfId As Integer = 0
    '    Dim ItmId As Integer = 0
    '    Dim n As Integer = 0
    '    Dim Sn As Integer = 0
    '    Dim itemGRP_Id As Integer = 0
    '    Dim itemId As Integer = 0
    '    Dim Sur As String = ""
    '    Dim Cat_Id As Integer = 0

    '    Try


    '        xlApp = New Excel.Application
    '        xlWorkBook = xlApp.Workbooks.Open(FileName)
    '        xlWorkSheet = xlWorkBook.Worksheets("sheet1")

    '        With xlWorkSheet
    '            RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
    '        End With

    '        'RowCnt = xlWorkSheet.UsedRange.Rows.Count

    '        If RowCnt <= 1 Then
    '            MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub
    '        End If


    '        For i = 2 To RowCnt

    '            itemId = Val(Common_Procedures.Item_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 4).value)))


    '            If itemId <> 0 Then Continue For

    '            Sur = Common_Procedures.Remove_NonCharacters(Trim(xlWorkSheet.Cells(i, 4).value))

    '            itemGRP_Id = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 2).value)))

    '            Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 1).value)))


    '            txt_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "item_head", "Item_IdNo", "")

    '            cmd.Connection = con


    '            cmd.CommandText = "Insert into item_head(Item_IdNo              , Item_Name                                   , Sur_Name            , Item_Code                                    ,          ItemGroup_IdNo       , Unit_IdNo , Minimum_Stock  , MRP_Rate                                   ,Gst_Percentage                             ) " & _
    '                                    "values (" & Str(Val(txt_IdNo.Text)) & ", '" & Trim(xlWorkSheet.Cells(i, 4).value) & "', '" & Trim(Sur) & "', '" & Trim(xlWorkSheet.Cells(i, 3).value) & "', " & Str(Val(itemGRP_Id)) & "  ,   1       ,       0        , " & Val(xlWorkSheet.Cells(i, 7).value) & " ," & Val(xlWorkSheet.Cells(i, 9).value) & " )"
    '            cmd.ExecuteNonQuery()

    '        Next i

    '        movelast_record()


    '        xlWorkBook.Close(False, FileName)
    '        xlApp.Quit()


    '        ReleaseComObject(xlWorkSheet)
    '        ReleaseComObject(xlWorkBook)
    '        ReleaseComObject(xlApp)

    '        Sts = True
    '        'MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

    '    Catch ex As Exception

    '        Sts = False

    '        xlWorkBook.Close(False, FileName)
    '        xlApp.Quit()


    '        ReleaseComObject(xlWorkSheet)
    '        ReleaseComObject(xlWorkBook)
    '        ReleaseComObject(xlApp)

    '        ' MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try


    'End Sub
    'Private Sub getExcelData_ItemGroup(ByVal FileName As String, ByRef Sts As Boolean)
    '    Dim cmd As New SqlClient.SqlCommand
    '    'Dim tr As SqlClient.SqlTransaction
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable

    '    Dim xlApp As Excel.Application
    '    Dim xlWorkBook As Excel.Workbook
    '    Dim xlWorkSheet As Excel.Worksheet

    '    Dim RowCnt As Long = 0
    '    ' Dim FileName As String = ""
    '    Dim NewCode As String = ""
    '    Dim BlerDtTm As DateTime
    '    Dim ShfId As Integer = 0
    '    Dim ItmId As Integer = 0
    '    Dim n As Integer = 0
    '    Dim Sn As Integer = 0
    '    Dim itemGRP_Id As Integer = 0
    '    Dim Sur As String = ""
    '    Dim Cat_Id As Integer = 0
    '    Dim mxId As Integer = 0
    '    Try


    '        xlApp = New Excel.Application
    '        xlWorkBook = xlApp.Workbooks.Open(FileName)
    '        xlWorkSheet = xlWorkBook.Worksheets("sheet1")

    '        With xlWorkSheet
    '            RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
    '        End With

    '        'RowCnt = xlWorkSheet.UsedRange.Rows.Count

    '        If RowCnt <= 1 Then
    '            MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub
    '        End If


    '        For i = 2 To RowCnt


    '            itemGRP_Id = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 2).value)))

    '            If itemGRP_Id <> 0 Then Continue For

    '            Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 1).value)))

    '            mxId = Common_Procedures.get_MaxIdNo(con, "ItemGroup_Head", "itemgroup_idno", "")

    '            Sur = Common_Procedures.Remove_NonCharacters(Trim(xlWorkSheet.Cells(i, 2).value))

    '            cmd.Connection = con

    '            cmd.CommandText = "Insert into ItemGroup_Head(  itemgroup_idno     ,         itemgroup_name                       ,       Item_HSN_Code                         ,       sur_name        , Cetegory_IdNo         , Item_GST_Percentage ) " & _
    '                                                "values (" & Str(Val(mxId)) & ", '" & Trim(xlWorkSheet.Cells(i, 2).value) & "','" & Trim(xlWorkSheet.Cells(i, 8).value) & "', '" & Trim(Sur) & "' ," & Str(Val(Cat_Id)) & " ," & Str(Val(xlWorkSheet.Cells(i, 9).value)) & ")"
    '            cmd.ExecuteNonQuery()

    '        Next i

    '        movelast_record()


    '        xlWorkBook.Close(False, FileName)
    '        xlApp.Quit()


    '        ReleaseComObject(xlWorkSheet)
    '        ReleaseComObject(xlWorkBook)
    '        ReleaseComObject(xlApp)

    '        Sts = True

    '        '  MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

    '    Catch ex As Exception

    '        Sts = False

    '        xlWorkBook.Close(False, FileName)
    '        xlApp.Quit()


    '        ReleaseComObject(xlWorkSheet)
    '        ReleaseComObject(xlWorkBook)
    '        ReleaseComObject(xlApp)


    '        'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try


    '    End Sub
    'Private Sub getExcelData_Category(ByVal FileName As String, ByRef Sts As Boolean)
    '    Dim cmd As New SqlClient.SqlCommand
    '    'Dim tr As SqlClient.SqlTransaction
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable

    '    Dim xlApp As Excel.Application
    '    Dim xlWorkBook As Excel.Workbook
    '    Dim xlWorkSheet As Excel.Worksheet

    '    Dim RowCnt As Long = 0
    '    '  Dim FileName As String = ""
    '    Dim NewCode As String = ""
    '    Dim BlerDtTm As DateTime
    '    Dim ShfId As Integer = 0
    '    Dim ItmId As Integer = 0
    '    Dim n As Integer = 0
    '    Dim Sn As Integer = 0
    '    Dim Cat_Id As Integer = 0
    '    Dim Sur As String = ""
    '    Dim MxId As Integer = 0

    '    Try
    '        'OpenFileDialog1.ShowDialog()
    '        'FileName = OpenFileDialog1.FileName


    '        xlApp = New Excel.Application
    '        xlWorkBook = xlApp.Workbooks.Open(FileName)
    '        xlWorkSheet = xlWorkBook.Worksheets("sheet1")

    '        With xlWorkSheet
    '            RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
    '        End With

    '        'RowCnt = xlWorkSheet.UsedRange.Rows.Count

    '        If RowCnt <= 1 Then
    '            MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub
    '        End If


    '        For i = 2 To RowCnt


    '            Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 1).value)))

    '            If Cat_Id <> 0 Then Continue For


    '            MxId = Common_Procedures.get_MaxIdNo(con, "Cetegory_Head", "Cetegory_IdNo", "")

    '            Sur = Common_Procedures.Remove_NonCharacters(Trim(xlWorkSheet.Cells(i, 1).value))

    '            cmd.Connection = con

    '            cmd.CommandText = "Insert into Cetegory_Head(Cetegory_IdNo, Cetegory_Name, Sur_Name) values (" & Str(Val(MxId)) & ", '" & Trim(xlWorkSheet.Cells(i, 1).value) & "', '" & Trim(Sur) & "')"
    '            cmd.ExecuteNonQuery()


    '        Next i

    '        movelast_record()


    '        xlWorkBook.Close(False, FileName)
    '        xlApp.Quit()


    '        ReleaseComObject(xlWorkSheet)
    '        ReleaseComObject(xlWorkBook)
    '        ReleaseComObject(xlApp)

    '        Sts = True

    '        ' MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

    '    Catch ex As Exception

    '        Sts = False

    '        xlWorkBook.Close(False, FileName)
    '        xlApp.Quit()


    '        ReleaseComObject(xlWorkSheet)
    '        ReleaseComObject(xlWorkBook)
    '        ReleaseComObject(xlApp)

    '        'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try


    'End Sub
    Private Sub ReleaseComObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub

    Private Sub chk_JobWorkStatus_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_JobWorkStatus.CheckedChanged

    End Sub

    Private Sub cbo_ItemGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ItemGroup.SelectedIndexChanged

    End Sub

    Private Sub cbo_ItemGroup_Enter(sender As Object, e As EventArgs) Handles cbo_ItemGroup.Enter
        cbo_ItemGroup.BackColor = Color.Lime
        cbo_ItemGroup.ForeColor = Color.Blue
        cbo_ItemGroup.SelectionStart = 0
        cbo_ItemGroup.SelectionLength = cbo_ItemGroup.Text.Length
    End Sub

    Private Sub cbo_Unit_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Unit.SelectedIndexChanged

    End Sub
End Class