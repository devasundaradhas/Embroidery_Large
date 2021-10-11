Public Class Item_OpeningStock
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private cmbItmNm As String
    Private cmbszNm As String

    Private Sub clear()

        pnl_Back.Enabled = True

        cmbItmNm = ""
        cmbszNm = ""

        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        cbo_Size.Text = ""

        txt_OpStock.Text = ""

    End Sub

    Private Sub move_record(ByVal Itmidno As String, ByVal sizidno As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Val(Itmidno) = 0 Then Exit Sub

        clear()

        Try

            cbo_ItemName.Text = Common_Procedures.Item_IdNoToName(con, Itmidno)
            cbo_Size.Text = Common_Procedures.Size_IdNoToName(con, sizidno)

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(Itmidno)) & "/"

            da1 = New SqlClient.SqlDataAdapter("select a.Quantity, b.Item_Name, c.Unit_Name , d.Size_Name from Item_Processing_Details a INNER JOIN Item_Head b ON a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c ON b.Unit_IdNo = c.Unit_IdNo LEFT OUTER JOIN Size_Head d ON a.Size_IdNo = d.Size_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_IdNo = " & Str(Val(Itmidno)) & " and a.Size_IdNo = " & Str(Val(sizidno)) & " and a.Reference_Code LIKE '" & Trim(NewCode) & "%' Order by a.Reference_Date, a.For_OrderBy, a.Reference_No", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                cbo_ItemName.Text = dt1.Rows(0).Item("Item_Name").ToString
                cbo_Unit.Text = dt1.Rows(0).Item("Unit_Name").ToString
                cbo_Size.Text = dt1.Rows(0).Item("Size_Name").ToString
                txt_OpStock.Text = Val(dt1.Rows(0).Item("Quantity").ToString)
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_ItemName.Visible And cbo_ItemName.Enabled Then cbo_ItemName.Focus()

    End Sub

    Private Sub Item_OpeningStock_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""

            If FrmLdSTS = True Then
                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Item_OpeningStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1079" Then

            lbl_OpnStk.Left = 10
            lbl_OpnStk.Top = 161
            txt_OpStock.Left = 112
            txt_OpStock.Top = 159

            lbl_Size.Left = 10
            lbl_Size.Top = 119
            cbo_Size.Left = 112
            cbo_Size.Top = 117

            lbl_Size.Visible = True
            cbo_Size.Visible = True
            cbo_Size.Enabled = True

        Else

            lbl_OpnStk.Left = 10
            lbl_OpnStk.Top = 119
            txt_OpStock.Left = 112
            txt_OpStock.Top = 117

            lbl_Size.Visible = False
            cbo_Size.Visible = False
            cbo_Size.Enabled = False

        End If


        Me.Text = ""
        Me.BackColor = Color.FromArgb(203, 213, 228)
        pnl_Back.BackColor = Me.BackColor

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        Da.Fill(Dt1)
        cbo_ItemName.DataSource = Dt1
        cbo_ItemName.DisplayMember = "item_name"

        Da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        Da.Fill(Dt2)
        cbo_Unit.DataSource = Dt2
        cbo_Unit.DisplayMember = "unit_name"


        Da = New SqlClient.SqlDataAdapter("select Size_Name from Size_head order by Size_Name", con)
        Da.Fill(Dt3)
        cbo_Size.DataSource = Dt3
        cbo_Size.DisplayMember = "Size_Name"


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Purchase_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub Purchase_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                lbl_Company.Tag = 0
                lbl_Company.Text = ""
                Me.Text = ""
                Common_Procedures.CompIdNo = 0


                lbl_Company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)
                Me.Text = lbl_Company.Text
                If Val(Common_Procedures.CompIdNo) = 0 Then

                    Me.Close()

                Else

                    new_record()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Itm_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Nr As Integer
        Dim NewCode As String

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            Itm_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
            Siz_ID = Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text)

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(Itm_ID)) & "/"

            cmd.Connection = con

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_IdNo = " & Str(Val(Itm_ID)) & " and Size_IdNo = " & Str(Val(Siz_ID)) & " and Reference_Code LIKE '" & Trim(NewCode) & "%'"
            Nr = cmd.ExecuteNonQuery()

            If Nr = 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else

                new_record()

                MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_ItemName.Enabled = True And cbo_ItemName.Visible = True Then cbo_ItemName.Focus()
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '----
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '----
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '----
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '----
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Try
            clear()

            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '----
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Nr As Long = 0
        Dim Itm_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim OpDate As Date
        Dim OpYrCode As String

        Itm_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
        If Itm_ID = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        Unt_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
        Siz_ID = 0
        If cbo_Size.Visible = True Then
            Siz_ID = Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text)
            If Siz_ID = 0 Then
                MessageBox.Show("Invalid Size", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Size.Enabled Then cbo_Size.Focus()
                Exit Sub
            End If
        End If

        tr = con.BeginTransaction

        Try

            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(Itm_ID)) & "/" & Trim(OpYrCode)

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate.Date)

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_IdNo = " & Str(Val(Itm_ID)) & " and Size_IdNo = " & Str(Val(Siz_ID)) & " and Reference_Code LIKE '" & Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(Itm_ID)) & "/%'"
            Nr = cmd.ExecuteNonQuery()

            Sno = Siz_ID
            cmd.CommandText = "Insert into Item_Processing_Details(Reference_Code       , Company_IdNo                  , Reference_No            , for_OrderBy                                                   ,   Reference_Date  , Ledger_IdNo, Party_Bill_No ,             SL_No       ,        Item_IdNo        ,     Size_IdNo           ,         Unit_IdNo       ,               Quantity             ) " & _
                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Itm_ID) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Itm_ID))) & ",  @OpeningDate     , 0          , ''            , " & Str(Val(Siz_ID)) & ", " & Str(Val(Itm_ID)) & ", " & Str(Val(Siz_ID)) & ", " & Str(Val(Unt_ID)) & ", " & Str(Val(txt_OpStock.Text)) & " )"
            cmd.ExecuteNonQuery()

            tr.Commit()

            move_record(Itm_ID, Siz_ID)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
            cmbItmNm = Trim(cbo_ItemName.Text)
        End With

    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        Try
            With cbo_ItemName
                If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    If cbo_Size.Visible = True And cbo_Size.Enabled = True Then
                        cbo_Size.Focus()
                    Else
                        txt_OpStock.Focus()
                    End If

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Itm_ID As Integer = 0
        Dim Siz_ID As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then

                Itm_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

                If cbo_Size.Visible = False Then
                    move_record(Val(Itm_ID), 0)
                End If

                If Trim(cbo_Unit.Text) = "" Then
                    da = New SqlClient.SqlDataAdapter("select b.unit_name from item_head a, unit_head b where a.item_name = '" & Trim(cbo_ItemName.Text) & "' and a.unit_idno = b.unit_idno", con)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                    End If
                    dt.Dispose()
                    da.Dispose()
                End If
                If cbo_Size.Visible = True And cbo_Size.Enabled = True Then
                    cbo_Size.Focus()
                Else
                    txt_OpStock.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        cmbItmNm = Trim(cbo_ItemName.Text)
        cbo_ItemName.BackColor = Color.White
        cbo_ItemName.ForeColor = Color.Black
    End Sub

    Private Sub txt_OpStock_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_OpStock.GotFocus
        txt_OpStock.BackColor = Color.Lime
        txt_OpStock.ForeColor = Color.Black
    End Sub

    Private Sub txt_OpStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OpStock.KeyDown
        'If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            If cbo_Size.Enabled = True Then
                e.Handled = True
                cbo_Size.Focus()
            Else
                e.Handled = True
                cbo_ItemName.Focus()
                'SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txt_OpStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OpStock.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.GotFocus
        With cbo_Size
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Size.Text.Length
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")
            cmbszNm = Trim(cbo_Size.Text)
        End With
    End Sub

    Private Sub cbo_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, cbo_ItemName, txt_OpStock, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")

    End Sub

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Itm_ID As Integer = 0
        Dim Siz_ID As Integer = 0


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, Nothing, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Or Trim(UCase(cmbszNm)) <> Trim(UCase(cbo_Size.Text)) Then

                Itm_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

                Siz_ID = Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text)

                move_record(Val(Itm_ID), Val(Siz_ID))

                If Trim(cbo_Unit.Text) = "" Then
                    da = New SqlClient.SqlDataAdapter("select b.unit_name from item_head a, unit_head b where a.item_name = '" & Trim(cbo_ItemName.Text) & "' and a.unit_idno = b.unit_idno", con)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                    End If
                    dt.Dispose()
                    da.Dispose()
                End If


                txt_OpStock.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Size_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.LostFocus
        cmbszNm = Trim(cbo_Size.Text)
        cbo_Size.BackColor = Color.White
        cbo_Size.ForeColor = Color.Black
    End Sub

    Private Sub txt_OpStock_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_OpStock.LostFocus
        txt_OpStock.BackColor = Color.White
        txt_OpStock.ForeColor = Color.Black
    End Sub
End Class