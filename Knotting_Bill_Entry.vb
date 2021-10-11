Public Class Knotting_Bill_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "KNTBL-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private vcmb_ItmNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_JbDetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private CFrm_STS As Integer
    Private prn_Status As Integer
    Private sqft_qty As Integer = 0

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_Amt_Op As Single, prn_Amt_Rcpt As Single, prn_Amt_CurBill As Single, prn_Amt_Balance As String
    Private prn_Amt_OpBlNo As String, prn_Amt_RcptVouNo As String

    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black
        lbl_GrossAmount.Text = ""
        lbl_TotalPavu.Text = ""

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_AmountInWords.Text = "Rupees  :  "

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Shift.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Shift.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""

            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""

            ElseIf TypeOf obj Is DateTimePicker Then
                obj.text = ""


            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl1 In grpbx.Controls
                    If TypeOf ctrl1 Is TextBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is ComboBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is DateTimePicker Then
                        ctrl1.text = ""
                    End If
                Next

            ElseIf TypeOf obj Is Panel Then
                pnl1 = obj
                If Trim(UCase(pnl1.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                    For Each ctrl2 In pnl1.Controls
                        If TypeOf ctrl2 Is TextBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is ComboBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is DataGridView Then
                            ctrl2.Rows.Clear()
                        ElseIf TypeOf ctrl2 Is DateTimePicker Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is Panel Then
                            pnl2 = ctrl2
                            If Trim(UCase(pnl2.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                                For Each ctrl3 In pnl2.Controls
                                    If TypeOf ctrl3 Is TextBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is ComboBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is DateTimePicker Then
                                        ctrl3.text = ""
                                    End If
                                Next
                            End If

                        End If

                    Next

                End If

            End If

        Next

        cbo_EntryType.Text = "KNOTTING"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        'cbo_DocumentThrough.Text = "CREDIT"
        'cbo_Type.Text = Common_Procedures.Ledger_IdNoToName(con, 22)
        'cbo_BookedBy.Text = Common_Procedures.Ledger_IdNoToName(con, 20)
        txt_SlNo.Text = "1"

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
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
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is Button Then

                If TypeOf Prec_ActCtrl Is Button Then
                    Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                    Prec_ActCtrl.ForeColor = Color.White

                Else
                    Prec_ActCtrl.BackColor = Color.White
                    Prec_ActCtrl.ForeColor = Color.Black

                End If

            End If

        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        'dgv_Details_Total.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName from Knotting_Bill_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Knotting_Bill_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Knotting_Bill_No").ToString
                dtp_InvDate.Text = dt1.Rows(0).Item("Knotting_Bill_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString
                cbo_EntryType.Text = dt1.Rows(0).Item("Entry_Type").ToString
                lbl_TotalPavu.Text = Val(dt1.Rows(0).Item("Total_Pavu").ToString)
                txt_Rate.Text = Format(Val(dt1.Rows(0).Item("Rate").ToString), "########0.00")
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                da2 = New SqlClient.SqlDataAdapter("select a.* from Knotting_Bill_Details a  where a.Knotting_Bill_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()
                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Knotting_Date").ToString), "dd-MM-yyyy")

                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Knotting_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Shift").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Ends").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Loom").ToString
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("No_Pavu").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Knotting_Code").ToString

                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Pavu").ToString)
                End With

                dt2.Clear()

            End If

            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da2.Dispose()

            dt1.Dispose()
            da1.Dispose()

            If dtp_InvDate.Visible And dtp_InvDate.Enabled Then dtp_InvDate.Focus()

        End Try

    End Sub


    Private Sub Knotting_Bill_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

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

    Private Sub Knotting_Bill_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where ledger_idno = 0 or (ledger_type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Shift_name from Shift_head order by Shift_name", con)
        da.Fill(dt2)
        cbo_Shift.DataSource = dt2
        cbo_Shift.DisplayMember = "Shift_name"

        cbo_EntryType.Items.Clear()
        cbo_EntryType.Items.Add("DIRECT")
        cbo_EntryType.Items.Add("KNOTTING")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2


        AddHandler dtp_InvDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntryType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KnotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Loom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_noPavu.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Shift.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_InvDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntryType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KnotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ends.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Loom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_noPavu.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Shift.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_InvDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KnotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Ends.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Loom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_noPavu.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_InvDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_SlNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KnotNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Ends.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Loom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_noPavu.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Knotting_Bill_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Knotting_Bill_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Close_Form()

        Try

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

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= 5 Then

                        If .CurrentCell.RowIndex >= .Rows.Count - 2 Then

                            txt_Rate.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)


                        End If


                    ElseIf .CurrentCell.ColumnIndex < 4 Then
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= 4 Then
                        If .CurrentCell.RowIndex = 0 Then
                            If pnl_DetInputs.Enabled = True And dtp_Date.Enabled = True Then
                                dtp_Date.Focus()

                            Else
                                cbo_EntryType.Focus()

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)

                        End If

                    ElseIf .CurrentCell.ColumnIndex > 5 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Knotting_Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Knotting_Invoice_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con

            cmd.CommandText = "Update Knotting_Head set Knotting_Bill_Code = '' where Knotting_Bill_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Knotting_Bill_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_InvDate.Enabled = True And dtp_InvDate.Visible = True Then dtp_InvDate.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Shift_name from Shift_head order by Shift_name", con)
            da.Fill(dt2)
            cbo_Filter_Shift.DataSource = dt2
            cbo_Filter_Shift.DisplayMember = "Shift_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Shift.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Shift.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Knotting_Bill_No from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Knotting_Bill_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Knotting_Bill_No from Knotting_Bill_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Knotting_Bill_No", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Knotting_Bill_No from Knotting_Bill_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Knotting_Bill_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Knotting_Bill_No from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Knotting_Bill_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            dtp_InvDate.Text = Date.Today.ToShortDateString

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Knotting_Bill_Head", "Knotting_Bill_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            If dtp_InvDate.Enabled And dtp_InvDate.Visible Then dtp_InvDate.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Knotting_Bill_No from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Invoice No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()


        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Knotting_Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Knotting_Invoice_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Knotting_Bill_No from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Invoice No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim Nr As Long = 0
        Dim led_id As Integer = 0
        Dim trans_id As Integer = 0
        Dim saleac_id As Integer = 0
        Dim txac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sz_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim vTot_Ends As Single = 0
        Dim vTot_Pavu As Single = 0
        Dim KntCd As String

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Knotting_Invoice_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_InvDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_InvDate.Enabled Then dtp_InvDate.Focus()
            Exit Sub
        End If

        If Not (dtp_InvDate.Value.Date >= Common_Procedures.Company_FromDate And dtp_InvDate.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_InvDate.Enabled Then dtp_InvDate.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(.Rows(i).Cells(5).Value) <> "" Or Val(.Rows(i).Cells(6).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Knot No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid Shift Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If


                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Ends", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(5).Value) = "" Then
                        MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Pavu", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(6)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        NoCalc_Status = False
        TotalPavu_Calculation()

        vTot_Ends = 0
        vTot_Pavu = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_Ends = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTot_Pavu = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Knotting_Bill_Head", "Knotting_Bill_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_InvDate.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Knotting_Bill_Head (    Knotting_Bill_Code  ,             Company_IdNo         ,              Knotting_Bill_No     ,                               for_OrderBy                                  , Knotting_Bill_Date,        Ledger_IdNo      ,             Entry_Type            ,                Total_Pavu           ,                 Rate           ,                 Gross_Amount          ,                 AddLess_Amount    ,                 Round_Off          ,                      Net_Amount     ) " & _
                                    "          Values             ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",    @EntryDate   , " & Str(Val(led_id)) & ", '" & Trim(cbo_EntryType.Text) & "', " & Str(Val(lbl_TotalPavu.Text)) & ", " & Str(Val(txt_Rate.Text)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Knotting_Bill_Head set Knotting_Bill_Date = @EntryDate, Ledger_IdNo = " & Str(Val(led_id)) & ",Entry_Type ='" & Trim(cbo_EntryType.Text) & "', Total_Pavu = " & Str(Val(lbl_TotalPavu.Text)) & ", Rate = " & Str(Val(txt_Rate.Text)) & ", Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & " , Round_Off = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Knotting_Head set Knotting_Bill_Code = '' where Knotting_Bill_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Knotting_Bill_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            With dgv_Details

                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@EntryDate", dtp_InvDate.Value.Date)
                        cmd.Parameters.AddWithValue("@KnotDate", CDate(.Rows(i).Cells(1).Value))

                        KntCd = ""
                        If Trim(UCase(cbo_EntryType.Text)) = "KNOTTING" Then
                            KntCd = Trim(.Rows(i).Cells(7).Value)
                        End If

                        cmd.CommandText = "Insert into Knotting_Bill_Details (    Knotting_Bill_Code  ,                 Company_IdNo     ,              Knotting_Bill_No     ,                               for_OrderBy                                  , Knotting_Bill_Date,      Ledger_IdNo        ,         Sl_No        , Knotting_Date,            Knotting_No                 ,                    Shift               ,                      Ends                ,                    Loom                ,                      No_Pavu             ,       Knotting_Code    ) " & _
                                          "            Values                ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",   @EntryDate      , " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ",   @KnotDate  , '" & Trim(.Rows(i).Cells(2).Value) & "', '" & Trim(.Rows(i).Cells(3).Value) & "', " & Str(Val(.Rows(i).Cells(4).Value)) & ", '" & Trim(.Rows(i).Cells(5).Value) & "', " & Str(Val(.Rows(i).Cells(6).Value)) & ",  '" & Trim(KntCd) & "' ) "
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(cbo_EntryType.Text)) = "KNOTTING" Then
                            If Trim(KntCd) <> "" Then
                                cmd.CommandText = "Update Knotting_Head set Knotting_Bill_Code = '" & Trim(NewCode) & "' where Knotting_Code = '" & Trim(KntCd) & "' and ( Ledger_IdNo = " & Str(Val(led_id)) & "  or Knotting_IdNo = " & Str(Val(led_id)) & " ) "
                                Nr = cmd.ExecuteNonQuery()
                                If Nr = 0 Then
                                    Throw New ApplicationException("Mismatch of Party & Shift Details")
                                    Exit Sub
                                End If

                            End If

                        End If

                    End If

                Next

            End With

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Ac_id = led_id
            saleac_id = 22

            cmd.CommandText = "Insert into Voucher_Head ( Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code ) " & _
                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Knot.Inv', @EntryDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Knot.Inv', @EntryDate, 1, " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Knot.Inv', @EntryDate, 2, " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            cmd.ExecuteNonQuery()

            tr.Commit()

            move_record(lbl_InvoiceNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            tr.Dispose()
            cmd.Dispose()

            If dtp_InvDate.Enabled And dtp_InvDate.Visible Then dtp_InvDate.Focus()

        End Try

    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        If Trim(txt_KnotNo.Text) = "" Then
            MessageBox.Show("Invalid Knot No.", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_KnotNo.Enabled Then txt_KnotNo.Focus()
            Exit Sub
        End If

        If Trim(cbo_Shift.Text) = "" Then
            MessageBox.Show("Invalid shift", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_Shift.Enabled Then cbo_Shift.Focus()
            Exit Sub
        End If

        If Val(txt_Ends.Text) = 0 Then
            MessageBox.Show("Invalid Ends", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Ends.Enabled Then txt_Ends.Focus()
            Exit Sub
        End If

        If Trim(txt_Loom.Text) = "" Then
            MessageBox.Show("Invalid Loom No.s", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Loom.Enabled Then txt_Loom.Focus()
            Exit Sub
        End If

        If Val(txt_noPavu.Text) = 0 Then
            MessageBox.Show("Invalid No.of Pavus", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_noPavu.Enabled Then txt_noPavu.Focus()
            Exit Sub
        End If


        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = dtp_Date.Text
                    .Rows(i).Cells(2).Value = txt_KnotNo.Text
                    .Rows(i).Cells(3).Value = cbo_Shift.Text

                    .Rows(i).Cells(4).Value = Val(txt_Ends.Text)

                    .Rows(i).Cells(5).Value = txt_Loom.Text
                    .Rows(i).Cells(6).Value = Val(txt_noPavu.Text)

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = dtp_Date.Text
                .Rows(n).Cells(2).Value = txt_KnotNo.Text
                .Rows(n).Cells(3).Value = cbo_Shift.Text
                .Rows(n).Cells(4).Value = Val(txt_Ends.Text)
                .Rows(n).Cells(5).Value = txt_Loom.Text

                .Rows(n).Cells(6).Value = Val(txt_noPavu.Text)

                .Rows(n).Selected = True

                If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If

        End With

        TotalPavu_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_Shift.Text = ""
        txt_KnotNo.Text = ""
        txt_Ends.Text = ""
        dtp_Date.Text = ""
        txt_Loom.Text = ""
        txt_noPavu.Text = ""

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub txt_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Ends.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoPavu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_noPavu.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub cbo_shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, txt_KnotNo, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If Trim(cbo_Shift.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_Rate.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(cbo_Shift.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_Rate.Focus()
            End If
        End If


    End Sub

    Private Sub dtp_InvDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_InvDate.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : e.SuppressKeyPress = True : txt_AddLess.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_InvDate, cbo_EntryType, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_EntryType, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyValue = 38 Then
            If pnl_DetInputs.Enabled = True And dtp_Date.Enabled = True Then
                dtp_Date.Focus()

            ElseIf dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                dgv_Details.CurrentCell.Selected = True

            Else
                cbo_EntryType.Focus()

            End If
        End If

        If e.KeyValue = 40 Then txt_AddLess.Focus() ' SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_InvDate.Focus()
            End If
        End If
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then
            dtp_Date.Focus()
            With dgv_Details

                For i = 0 To .Rows.Count - 2
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        dtp_Date.Text = Trim(.Rows(i).Cells(1).Value)
                        txt_KnotNo.Text = Trim(.Rows(i).Cells(2).Value)
                        cbo_Shift.Text = Val(.Rows(i).Cells(3).Value)
                        txt_Ends.Text = Val(.Rows(i).Cells(4).Value)
                        ''If Val(txt_Loom.Text) = 0 Then
                        ''    txt_Loom.Text = ""
                        ''End If
                        txt_Loom.Text = Trim(.Rows(i).Cells(5).Value)
                        txt_noPavu.Text = Val(.Rows(i).Cells(6).Value)

                        Exit For

                    End If

                Next

            End With

        End If
    End Sub


    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Knotting_Bill_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Knotting_Bill_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Knotting_Bill_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            'If Trim(cbo_Filter_Shift.Text) <> "" Then
            '    Itm_IdNo = Common_Procedures.Item_NameToIdNo(con, cbo_Filter_Shift.Text)
            'End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Trim(cbo_Shift.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Knotting_Bill_Code IN (select z.Knotting_Bill_Code from Knotting_Bill_Details z where z.Shift = '" & Trim(cbo_Shift.Text) & "') "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Knotting_Bill_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Knotting_Bill_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Knotting_Bill_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Knotting_Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Knotting_Bill_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Pavu").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.000")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            dt1.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True 'SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Shift, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Shift, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Shift, cbo_Filter_PartyName, btn_Filter_Show, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Shift, Nothing, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub


    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)
                End If
            End If
            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)
                End If
            End If
            TotalPavu_Calculation()
        End With



    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged


        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Then
                    TotalPavu_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If pnl_DetInputs.Enabled = True And txt_SlNo.Enabled = True Then

            If Trim(dgv_Details.CurrentRow.Cells(2).Value) <> "" Then

                txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                dtp_Date.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                txt_KnotNo.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
                cbo_Shift.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
                txt_Ends.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                'If Val(txt_Loom.Text) = 0 Then
                '    txt_Loom.Text = ""
                'End If
                txt_Loom.Text = Trim(dgv_Details.CurrentRow.Cells(5).Value)
                txt_noPavu.Text = Val(dgv_Details.CurrentRow.Cells(6).Value)

                If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

                '    If Val(dgv_Details.CurrentRow.Cells(4).Value) = 0 Then
                '        txt_Loom.Enabled = False
                '    Else
                '        txt_Loom.Enabled = True
                '    End If

            End If

        End If
    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next
            End If

        End With

        TotalPavu_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        dtp_Date.Text = ""
        txt_KnotNo.Text = ""
        cbo_Shift.Text = ""
        txt_Ends.Text = ""
        txt_Loom.Text = ""
        txt_noPavu.Text = ""

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index
                .Rows.RemoveAt(n)

                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next

            End With

            TotalPavu_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            dtp_Date.Text = ""
            txt_KnotNo.Text = ""
            cbo_Shift.Text = ""
            txt_Ends.Text = ""
            txt_Loom.Text = ""
            txt_noPavu.Text = ""

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub TotalPavu_Calculation()
        Dim I As Integer
        Dim Sno As Integer
        Dim TotPavu As Decimal

        Sno = 0
        TotPavu = 0

        With dgv_Details

            For I = 0 To .RowCount - 1
                Sno = Sno + 1
                dgv_Details.Rows(I).Cells(0).Value = Sno

                If Trim(.Rows(I).Cells(5).Value) <> "" Or Val(.Rows(I).Cells(6).Value) <> 0 Then

                    TotPavu = TotPavu + Val(dgv_Details.Rows(I).Cells(6).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .Rows.Count = 0 Then .Rows.Add()
            .Rows(0).Cells(6).Value = Val(TotPavu)
        End With

        lbl_TotalPavu.Text = Val(TotPavu)

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        lbl_GrossAmount.Text = Val(lbl_TotalPavu.Text) * Val(txt_Rate.Text)

        NtAmt = Val(lbl_GrossAmount.Text) + Val(txt_AddLess.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

        lbl_RoundOff.Text = Format(Val(lbl_NetAmount.Text) - Val(NtAmt), "#########0.00")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Private Sub cbo_EntryType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntryType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntryType, cbo_Ledger, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_EntryType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If pnl_DetInputs.Enabled = True And txt_SlNo.Enabled = True Then
                txt_SlNo.Focus()

                'ElseIf dgv_Details.Rows.Count > 0 Then
                '    dgv_Details.Focus()
                '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                '    dgv_Details.CurrentCell.Selected = True

            Else
                txt_AddLess.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_EntryType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntryType.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    Debug.Print("")
        'End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntryType, cbo_EntryType, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_EntryType.Text)) = "KNOTTING" Then
                If MessageBox.Show("Do you want to select Shift", "FOR SHIFT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    cbo_EntryType.Text = "KNOTTING"
                    btn_Selection_Click(sender, e)

                Else
                    cbo_EntryType.Text = "KNOTTING"
                    'If dgv_Details.Rows.Count > 0 Then
                    '    dgv_Details.Focus()
                    '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                    '    dgv_Details.CurrentCell.Selected = True

                    'Else
                    txt_Rate.Focus()

                    'End If

                End If

            Else

                If pnl_DetInputs.Enabled = True And txt_SlNo.Enabled = True Then
                    txt_SlNo.Focus()

                ElseIf dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    txt_Rate.Focus()

                End If

            End If

        End If

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedNo, Nr As Integer
        Dim NewCode As String

        LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT SHIFT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            chk_SelectAll.Checked = False

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.* from Knotting_Head a INNER JOIN Knotting_Bill_Details b ON b.Knotting_Bill_Code = '" & Trim(NewCode) & "' and a.Knotting_Code = b.Knotting_Code  where (a.Ledger_IdNo = " & Str(Val(LedNo)) & " OR a.Knotting_IdNo = " & Str(Val(LedNo)) & ") and a.Knotting_Bill_Code = '" & Trim(NewCode) & "' Order by a.Knotting_Date, a.For_OrderBy, a.Knotting_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Knotting_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Knotting_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Shift").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Ends").ToString)
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Loom").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("No_Pavu").ToString)
                    .Rows(n).Cells(7).Value = "1"
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Knotting_Code").ToString


                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()


            Da = New SqlClient.SqlDataAdapter("select a.* from Knotting_Head a  where (a.Ledger_IdNo = " & Str(Val(LedNo)) & " or a.Knotting_IdNo = " & Str(Val(LedNo)) & " ) and a.Knotting_Bill_Code = '' Order by a.Knotting_Date, a.For_OrderBy, a.Knotting_No", con)
            Dt1 = New DataTable
            Nr = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Knotting_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Knotting_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Shift").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Ends").ToString)
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Loom").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("No_Pavu").ToString)
                    .Rows(n).Cells(7).Value = ""
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Knotting_Code").ToString


                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Knotting(e.RowIndex)
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                Select_Knotting(dgv_Selection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Select_Knotting(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(7).Value) = 0 Then
                    .Rows(RwIndx).Cells(7).Value = ""
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                Else
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(7).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Blue
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Knotting(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim n As Integer
        Dim sno As Integer

        dgv_Details.Rows.Clear()

        pnl_Back.Enabled = True

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(7).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(4).Value = Val(dgv_Selection.Rows(i).Cells(4).Value)
                'If Val(dgv_Details.Rows(n).Cells(4).Value) = 0 Then
                '    dgv_Details.Rows(n).Cells(4).Value = ""
                'End If
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(6).Value = Val(dgv_Selection.Rows(i).Cells(6).Value)
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                'dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(2).Value
                'dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(7).Value

            End If

        Next

        TotalPavu_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        'If dgv_Details.Rows.Count > 0 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
        '    dgv_Details.CurrentCell.Selected = True

        'Else
        txt_Rate.Focus()

        'End If

    End Sub

    Private Sub cbo_EntryType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntryType.TextChanged
        If Trim(UCase(cbo_EntryType.Text)) = "DIRECT" Then
            pnl_DetInputs.Enabled = True
            'dgv_Details.EditMode = DataGridViewEditMode.EditProgrammatically
            'dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Else
            pnl_DetInputs.Enabled = False
            'dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
            'dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
        End If
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then
                    If Trim(UCase(cbo_EntryType.Text)) = "DIRECT" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        printing_invoice()
    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        'Dim ps As Printing.PaperSize
        Dim CmpName As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Knotting_Bill_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Knotting_Bill_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        CmpName = Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X8", 800, 800)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDocument1.Print()
                'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                '    PrintDocument1.Print()
                'End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1


                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)


                ppd.ShowDialog()


            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_JbDetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Knotting_Bill_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Knotting_Bill_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Knotting_Bill_Details a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Knotting_Bill_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da2 = New SqlClient.SqlDataAdapter("select a.* from Knotting_Head a where a.Knotting_Bill_Code = '" & Trim(NewCode) & "' Order by a.Knotting_Date, a.for_Orderby, a.Knotting_No", con)
                prn_JbDetDt = New DataTable
                da2.Fill(prn_JbDetDt)

                da2.Dispose()

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format1(e)

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X8", 800, 800)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 85 ' 25 ' 30 '60
            .Right = 100 '250
            .Top = 40 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        'pFont = New Font("Calibri", 10, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 13 ' 27

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45 : ClArr(2) = 95 : ClArr(3) = 75 : ClArr(4) = 70 : ClArr(5) = 70 : ClArr(6) = 150
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        'ClArr(0) = 0
        'ClArr(1) = 50 : ClArr(2) = 110 : ClArr(3) = 100 : ClArr(4) = 90 : ClArr(5) = 90 : ClArr(6) = 200
        'ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage - 1
                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage - 1

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                CurY = CurY - 10
                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Convert.ToDateTime(prn_DetDt.Rows(DetIndx).Item("Knotting_Date").ToString), "dd-MM-yyyy")), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Knotting_No").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Shift").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Ends").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Loom").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("No_Pavu").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 15, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim LedAr(10) As String
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim Cen1 As Single = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W1 As Single = 0, W2 As Single = 0, W3 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0, C4 As Single = 0, C5 As Single = 0, C6 As Single = 0, C7 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.* from Knotting_Bill_Details a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Knotting_Bill_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""


        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If

        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)

        End If

        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If

        Else
            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)

        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = ""

            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
            Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            Led_TinNo = ""  ' Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)

            LedAr = New String(10) {"", "", "", "", "", "", "", "", "", "", ""}

            Indx = 0

            Indx = Indx + 1
            LedAr(Indx) = Trim(Led_Name)

            If Trim(Led_Add1) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = Trim(Led_Add1)
            End If

            If Trim(Led_Add2) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = Trim(Led_Add2)
            End If

            If Trim(Led_Add3) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = Trim(Led_Add3)
            End If

            If Trim(Led_Add4) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = Trim(Led_Add4)
            End If

            'If Trim(Led_TinNo) <> "" Then
            '    Indx = Indx + 1
            '    LedAr(Indx) = "Tin No : " & Trim(Led_TinNo)
            'End If

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)     ' (PageWidth \ 2) + 100
            HdWd = PageWidth - Cen1 - LMargin

            H1 = e.Graphics.MeasureString("TO    :", pFont).Width
            W1 = e.Graphics.MeasureString("DATE    :", pFont).Width

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "BILL", LMargin + Cen1, CurY - 10, 2, HdWd, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Led_Name, LMargin + H1 + 10, CurY - 8, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY + 10, PageWidth, CurY + 10)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(2) & " " & LedAr(3), LMargin + H1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "NO.   :   " & prn_HdDt.Rows(0).Item("Knotting_Bill_No").ToString, LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE   :   " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Knotting_Bill_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + 110, CurY + 15, 0, 0, pFont)



            'Common_Procedures.Print_To_PrintDocument(e, "NO.", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Knotting_Bill_No").ToString, LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4) & " " & LedAr(5), LMargin + H1 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, LedAr(4), LMargin + H1 + 10, CurY, 0, 0, pFont)

            ''Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            ''Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            ''Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Knotting_Bill_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, LedAr(5), LMargin + H1 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Trim(Led_TinNo) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, LedAr(6), LMargin + H1 + 10, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, LnAr(3), LMargin + Cen1, LnAr(2))

            C1 = ClAr(1) + ClAr(2) - 85
            C2 = 50 + ClAr(3) + ClAr(4) - 10
            C3 = PageWidth - (C1 + C2)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "KNOT.NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SHIFT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOOM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF PAVU", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer = 0
        Dim W1 As Single = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            If is_LastPage = True Then

                prn_Amt_Op = 0
                prn_Amt_OpBlNo = ""

                prn_Amt_Rcpt = 0
                prn_Amt_RcptVouNo = ""

                prn_Amt_CurBill = 0
                prn_Amt_Balance = 0

                Calculation_Old_BalanceAmount()

                W1 = e.Graphics.MeasureString("Opening Balance", pFont).Width

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 11, FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, "AMOUNT BALANCE DETAILS", LMargin + 20, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "Total Pavu", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 85, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pavu").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 95, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Opening Balance", LMargin + 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_Amt_Op)), LMargin + W1 + 160, CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Rate", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 85, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Rate").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 95, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Knoting Charges", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 80, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt - 2
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "(B/F Bill No. :  " & Trim(prn_Amt_OpBlNo) & ")", LMargin + 30, CurY, 0, 0, pFont)

                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) < 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 80, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 80, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
                    End If

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)

                End If


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Received Amount  (-)", LMargin + 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_Amt_Rcpt)), LMargin + W1 + 160, CurY, 1, 0, pFont)

                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 100, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt - 2
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "(Rc.No. :  " & Trim(prn_Amt_RcptVouNo) & ")", LMargin + 30, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt - 2
                Common_Procedures.Print_To_PrintDocument(e, "Current Bill Amount  (+)", LMargin + 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_Amt_CurBill)), LMargin + W1 + 160, CurY, 1, 0, pFont)

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 5, PageWidth, CurY + 5)

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Balance Amount", LMargin + 15, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_Amt_Balance)), LMargin + W1 + 160, CurY, 1, 0, p1Font)

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                'p1Font = New Font("Calibri", 14, FontStyle.Bold)

                Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 15, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 15, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 15, CurY, 1, 0, p1Font)
                End If

                'If Trim(prn_Amt_OpBlNo) = "" Then
                '    CurY = CurY + TxtHgt - 2
                'End If
                'If Trim(prn_Amt_RcptVouNo) = "" Then
                '    CurY = CurY + TxtHgt - 2
                'End If

            Else

                CurY = CurY + TxtHgt

                CurY = CurY + TxtHgt + 10

                CurY = CurY + TxtHgt - 2

                CurY = CurY + TxtHgt

                CurY = CurY + TxtHgt - 2

                CurY = CurY + TxtHgt

                CurY = CurY + TxtHgt

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(6))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_amount").ToString))
                'If Len(Rup1) > 75 Then
                '    For I = 75 To 1 Step -1
                '        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                '    Next I
                '    If I = 0 Then I = 75
                '    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                '    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                'End If
            End If

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Rupees  :  " & Rup1, LMargin + 10, CurY, 0, 0, pFont)
            End If

            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt - 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(7) = CurY

            'CurY = CurY + TxtHgt - 5

            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + 5
            'Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            'CurY = CurY + TxtHgt - 15
            'p1Font = New Font("Calibri", 9, FontStyle.Regular)
            'Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

            'If is_LastPage = True Then

            '    CurY = LnAr(7)

            '    prn_Amt_Op = 0
            '    prn_Amt_OpBlNo = ""

            '    prn_Amt_Rcpt = 0
            '    prn_Amt_RcptVouNo = ""

            '    prn_Amt_CurBill = 0
            '    prn_Amt_Balance = 0

            '    Calculation_Old_BalanceAmount()

            '    W1 = e.Graphics.MeasureString("Opening Balance", pFont).Width

            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Opening Balance", LMargin + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_Amt_Op)), LMargin + W1 + 120, CurY, 1, 0, pFont)

            '    If Trim(prn_Amt_OpBlNo) <> "" Then
            '        CurY = CurY + TxtHgt - 2
            '        p1Font = New Font("Calibri", 9, FontStyle.Regular)
            '        Common_Procedures.Print_To_PrintDocument(e, "(B/F Bill No. :  " & Trim(prn_Amt_OpBlNo) & ")", LMargin + 30, CurY, 0, 0, pFont)
            '    End If

            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Received Amount  (-)", LMargin + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_Amt_Rcpt)), LMargin + W1 + 120, CurY, 1, 0, pFont)

            '    If Trim(prn_Amt_RcptVouNo) <> "" Then
            '        CurY = CurY + TxtHgt - 2
            '        p1Font = New Font("Calibri", 9, FontStyle.Regular)
            '        Common_Procedures.Print_To_PrintDocument(e, "(Rc.No. :  " & Trim(prn_Amt_RcptVouNo) & ")", LMargin + 30, CurY, 0, 0, p1Font)
            '    End If

            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Current Bill Amount  (+)", LMargin + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_Amt_CurBill)), LMargin + W1 + 120, CurY, 1, 0, pFont)

            '    CurY = CurY + TxtHgt
            '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "Balance Amount", LMargin + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_Amt_Balance)), LMargin + W1 + 120, CurY, 1, 0, p1Font)

            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Calculation_Old_BalanceAmount()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim DtHd As DataTable
        Dim i As Long
        Dim Prev_BillNo As String
        Dim Prev1_BillDt As Date, Prev2_BillDt As Date
        Dim Led_IdNo As Integer
        Dim Ent_Bill_OrdBy As Single, Prev_Bill_OrdByNo As Single
        Dim Ent_AuInc_BillNo As Long
        Dim Cmp_Cond As String
        Dim New_Code As String
        Dim Amt_OpBal As Single, Amt_Rcpt As Single, Cur_SetAmt As Single
        Dim Amt_RcptNo As String

        If New_Entry = True Then Exit Sub

        New_Code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Ent_Bill_OrdBy = 0

        Ent_AuInc_BillNo = 0

        Da = New SqlClient.SqlDataAdapter("select * from Knotting_Bill_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Knotting_Bill_Code = '" & Trim(New_Code) & "' order by a.For_orderBy", con)
        DtHd = New DataTable
        Da.Fill(DtHd)
        If DtHd.Rows.Count > 0 Then
            Ent_AuInc_BillNo = Val(DtHd.Rows(0).Item("Auto_BillNo").ToString)
        End If

        If Ent_AuInc_BillNo = 0 Then
            Da = New SqlClient.SqlDataAdapter("select  max(Auto_BillNo) from Knotting_Bill_Head", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                Ent_AuInc_BillNo = Val(Dt1.Rows(0)(0).ToString)
            End If
            Dt1.Clear()
            If Ent_AuInc_BillNo = 0 Then Ent_AuInc_BillNo = 1
        End If

        Prev_BillNo = ""
        Prev_Bill_OrdByNo = 0

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
            Cmp_Cond = " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and "
        End If

        Led_IdNo = Val(DtHd.Rows(0).Item("ledger_idno").ToString)

        Ent_Bill_OrdBy = Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
        cmd.Parameters.AddWithValue("@EntBillDate", prn_HdDt.Rows(0).Item("Knotting_Bill_Date"))

        '----   Getting Previous BillNo & BillDate 
        Prev1_BillDt = Common_Procedures.Company_FromDate
        Prev2_BillDt = Common_Procedures.Company_FromDate

        cmd.CommandText = "select a.* from Knotting_Bill_Head a where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(Led_IdNo)) & " and (a.Knotting_Bill_Date < @EntBillDate or ( a.Knotting_Bill_Date = @EntBillDate and a.for_orderby < " & Str(Val(Ent_Bill_OrdBy)) & ") or ( a.Knotting_Bill_Date = @EntBillDate and a.for_orderby = " & Str(Val(Ent_Bill_OrdBy)) & " and a.Auto_BillNo < " & Str(Val(Ent_AuInc_BillNo)) & ") ) order by a.Knotting_Bill_Date desc, a.for_orderby desc, a.Auto_BillNo desc"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            Prev_BillNo = Dt1.Rows(0).Item("Knotting_Bill_No").ToString
            Prev1_BillDt = Dt1.Rows(0).Item("Knotting_Bill_Date").ToString()
            Prev2_BillDt = DateAdd("d", 1, Prev1_BillDt)
            Prev_Bill_OrdByNo = Dt1.Rows(0).Item("for_OrderBy").ToString
            If Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Knotting_Bill_Code").ToString, 5) <> Common_Procedures.FnYearCode Then Ent_Bill_OrdBy = Ent_Bill_OrdBy + Val(Dt1.Rows(0).Item("For_OrderBy").ToString())
        End If
        Dt1.Clear()

        cmd.Parameters.AddWithValue("@Prev_BillDt1", Prev1_BillDt.Date)
        cmd.Parameters.AddWithValue("@Prev_BillDt2", Prev2_BillDt.Date)

        '----   Opening Balance for Amount

        prn_Amt_Op = 0 : prn_Amt_Rcpt = 0 : prn_Amt_CurBill = 0
        prn_Amt_OpBlNo = "" : prn_Amt_RcptVouNo = ""

        Amt_OpBal = 0
        Amt_Rcpt = 0 : Amt_RcptNo = ""

        If DateDiff("d", Prev1_BillDt, DtHd.Rows(0).Item("Knotting_Bill_Date")) = 0 And Ent_Bill_OrdBy > Prev_Bill_OrdByNo Then

            cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(Led_IdNo)) & " and a.voucher_date < @CompFromDate"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = -1 * Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            End If
            Dt1.Clear()

            cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a, voucher_head b where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(Led_IdNo)) & " and a.voucher_date between @CompFromDate and @EntBillDate and ( b.entry_identification NOT LIKE '" & Trim(Pk_Condition) & "%' or b.entry_identification is Null ) and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = Amt_OpBal - Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            End If
            Dt1.Clear()

            cmd.CommandText = "select sum(a.net_amount) as Inv_OpBalance from Knotting_Bill_Head a Where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(Led_IdNo)) & " and ( ( a.Knotting_Bill_Date >= @CompFromDate and a.Knotting_Bill_Date < @EntBillDate) or ( a.Knotting_Bill_Date = @EntBillDate and a.for_orderby < " & Str(Val(Ent_Bill_OrdBy)) & ") ) "
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Inv_OpBalance").ToString) = False Then Amt_OpBal = Amt_OpBal + Val(Dt1.Rows(0).Item("Inv_OpBalance").ToString)
            End If
            Dt1.Clear()

        Else

            cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(Led_IdNo)) & " and a.voucher_date < @Prev_BillDt2"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = -1 * Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            End If
            Dt1.Clear()

            cmd.CommandText = "select b.entry_identification, a.voucher_no, a.voucher_amount from voucher_details a, voucher_head b where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(Led_IdNo)) & " and a.voucher_date between @Prev_BillDt2 and @EntBillDate and (b.entry_identification NOT LIKE '" & Trim(Pk_Condition) & "%' or b.entry_identification is Null ) and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno order by a.voucher_date, a.For_OrderBy, a.Voucher_No, a.For_OrderByCode"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1
                    Amt_RcptNo = Trim(Amt_RcptNo) & IIf(Trim(Amt_RcptNo) <> "", ", ", "") & Trim(Dt1.Rows(i).Item("voucher_no").ToString)
                    Amt_Rcpt = Amt_Rcpt + Val(Dt1.Rows(i).Item("voucher_amount").ToString)
                Next
            End If
            Dt1.Clear()

        End If

        Cur_SetAmt = 0
        cmd.CommandText = "select * from Knotting_Bill_Head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(New_Code) & "'"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            Cur_SetAmt = Val(Dt1.Rows(0).Item("Net_Amount").ToString)
        End If
        Dt1.Clear()

        prn_Amt_Op = Val(Amt_OpBal)
        prn_Amt_OpBlNo = Trim(Prev_BillNo)

        prn_Amt_Rcpt = Val(Amt_Rcpt)
        prn_Amt_RcptVouNo = Trim(Amt_RcptNo)

        prn_Amt_CurBill = Val(Cur_SetAmt)

        prn_Amt_Balance = prn_Amt_Op - prn_Amt_Rcpt + prn_Amt_CurBill

        DtHd.Dispose()
        Dt1.Dispose()
        Da.Dispose()
        cmd.Dispose()

    End Sub



End Class