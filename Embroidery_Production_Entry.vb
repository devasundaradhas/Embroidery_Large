Public Class Embroidery_Production_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GPROD-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private vcmb_ItmNm As String
    Private vcmb_SizNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private CFrm_STS As Integer
    Private prn_Status As Integer
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private InvPrintFrmt As String = ""
    Private InvPrintFrmt_Letter As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_DetSNo As Integer
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Order_Disp_Cond As String = ""

    Dim Displaying_Saved_Qty As Boolean = False


    Private Sub clear()

        New_Entry = False
        Insert_Entry = False
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Print.Visible = False

        Panel2.Enabled = True

        cbo_Framer.Text = ""
        cbo_Operator.Text = ""
        lbl_Grid_Design.Text = ""
        cbo_Machine.Text = ""
        cbo_shift.Text = ""
        lbl_Grid_Design.Text = ""
        txt_NoOfHeads.Text = ""
        txt_NoOfPcs.Text = ""
        txt_NoOfStiches.Text = ""
        cbo_Ledger.Text = ""
        Cbo_OrderCode.Text = ""
        cbo_JobNo.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        txt_SlNo.Text = ""
        txt_Stch_Pcs.Text = ""
        txt_Remarks.Text = ""
        cbo_colour.Text = ""
        cbo_Size.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            dgv_Filter_Details.Rows.Clear()
        End If

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_Filter_Framer.Text = ""
        cbo_Filter_Machine.Text = ""
        cbo_Filter_Operator.Text = ""
        cbo_Filter_Shift.Text = ""
        cbo_InCharge.Text = ""
        cbo_Filter_Incharge.Text = ""
        txt_Uom.Text = ""
        txt_SlNo.Text = "1"

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskbox As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskbox = Me.ActiveControl
            mskbox.SelectionStart = -1
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
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            End If
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

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Production_Head a where a.Production_Code = '" & Trim(NewCode) & "' ", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Production_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Production_Date").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_Machine.Text = Common_Procedures.Machine_IdNoToName(con, Val(dt1.Rows(0).Item("Machine_IdNo").ToString))
                cbo_Operator.Text = Common_Procedures.Employee_IdNoToName(con, Val(dt1.Rows(0).Item("Operator_IdNo").ToString))
                cbo_Framer.Text = Common_Procedures.Employee_IdNoToName(con, Val(dt1.Rows(0).Item("Framer_IdNo").ToString))
                cbo_InCharge.Text = Common_Procedures.Employee_IdNoToName(con, Val(dt1.Rows(0).Item("Incharge_IdNo").ToString))
                cbo_shift.Text = dt1.Rows(0).Item("Shift").ToString

            End If

            da2 = New SqlClient.SqlDataAdapter("select a.*,b.Unit_IdNo as Unit_No from Production_Details a inner Join Order_Program_Head b on a.OrderCode_forSelection = b.OrderCode_forSelection where a.Production_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_Details.Rows.Clear()

            SNo = 0


            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Details.Rows.Add()
                    SNo = SNo + 1

                    dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Ordercode_forSelection").ToString
                    dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Design").ToString
                    dgv_Details.Rows(n).Cells(3).Value = Common_Procedures.Colour_IdNoToName(con, Val(dt2.Rows(i).Item("Colour_IdNo").ToString))
                    dgv_Details.Rows(n).Cells(4).Value = Common_Procedures.Size_IdNoToName(con, Val(dt2.Rows(i).Item("Size_IdNo").ToString))
                    dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("StchsPr_Pcs").ToString)
                    dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Head").ToString)
                    dgv_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Stiches").ToString)
                    dgv_Details.Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("Pieces").ToString)
                    dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Order_No").ToString
                    dgv_Details.Rows(n).Cells(12).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("Ledger_IdNo").ToString))
                    dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Job_No").ToString

                    If Not IsDBNull(dt2.Rows(i).Item("Unit_No")) Then
                        dgv_Details.Rows(n).Cells(14).Value = Common_Procedures.Unit_IdNoToName(con, dt2.Rows(i).Item("Unit_No"))
                    End If
                Next i



            End If

            For i = 0 To dgv_Details.Rows.Count - 1
                dgv_Details.Rows(n).Cells(0).Value = i + 1
            Next

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Heads").ToString)
                .Rows(0).Cells(7).Value = Val(dt1.Rows(0).Item("Total_Stchs").ToString)
                .Rows(0).Cells(8).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Amt").ToString), "############0.00")
            End With

            txt_SlNo.Text = dgv_Details.Rows.Count + 1

            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt2.Dispose()
            da2.Dispose()

            dt1.Dispose()
            da1.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub Invoice_Garments_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Machine.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MACHINE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Machine.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Operator.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Operator.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Framer.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Framer.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BRAND" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Invoice_Garments_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'dtp_Date.MaxDate = Common_Procedures.settings.Validation_End_Date

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

        da = New SqlClient.SqlDataAdapter("select a.Machine_Name from Machine_Head a order by a.Machine_Name", con)
        da.Fill(dt1)

        cbo_Machine.DataSource = dt1
        cbo_Machine.DisplayMember = "Machine_Name"

        'cbo_Filter_Machine.DataSource = dt1
        'cbo_Filter_Machine.DisplayMember = "Machine_Name"

        da = New SqlClient.SqlDataAdapter("select a.Employee_Name from Payroll_Employee_Head a order by a.Employee_Name", con)
        da.Fill(dt2)

        cbo_Operator.DataSource = dt2
        cbo_Operator.DisplayMember = "Employee_Name"

        da = New SqlClient.SqlDataAdapter("select a.Employee_Name from Payroll_Employee_Head a order by a.Employee_Name", con)
        da.Fill(dt3)

        cbo_shift.Items.Clear()
        cbo_shift.Items.Add("")
        cbo_shift.Items.Add("DAY")
        cbo_shift.Items.Add("NIGHT")

        cbo_Filter_Shift.Items.Clear()
        cbo_Filter_Shift.Items.Add("")
        cbo_Filter_Shift.Items.Add("DAY")
        cbo_Filter_Shift.Items.Add("NIGHT")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Framer.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Machine.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Operator.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Framer.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Machine.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Operator.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_OrderCode.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_JobNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Stch_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfHeads.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfStiches.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Grid_Design.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_InCharge.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Framer.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Machine.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Operator.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Framer.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Machine.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Operator.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_OrderCode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_JobNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Stch_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfHeads.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfStiches.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Grid_Design.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_InCharge.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Stch_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfHeads.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfPcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfStiches.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Stch_Pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfHeads.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfPcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfStiches.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True


        dgv_Details.Columns(11).DisplayIndex = 2
        dgv_Details.Columns(13).DisplayIndex = 3
        dgv_Details.Columns(14).DisplayIndex = 11

        dgv_Details_Total.Columns(11).DisplayIndex = 2
        dgv_Details_Total.Columns(13).DisplayIndex = 3
        dgv_Details_Total.Columns(14).DisplayIndex = 11

        new_record()

    End Sub

    Private Sub Invoice_Garments_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Invoice_Garments_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""

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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con


            cmd.CommandText = "delete from Production_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable


            'da = New SqlClient.SqlDataAdapter("select a.Machine_Name from Machine_Head a order by a.Machine_Name", con)
            'da.Fill(dt1)

            'cbo_Filter_Machine.DataSource = dt1
            'cbo_Filter_Machine.DisplayMember = "Machine_Name"

            'da = New SqlClient.SqlDataAdapter("select a.Employee_Name from Payroll_Employee_Head a order by a.Employee_Name", con)
            'da.Fill(dt2)

            'cbo_Filter_Operator.DataSource = dt2
            'cbo_Filter_Operator.DisplayMember = "Employee_Name"

            'da = New SqlClient.SqlDataAdapter("select a.Employee_Name from Payroll_Employee_Head a order by a.Employee_Name", con)
            'da.Fill(dt3)

            'cbo_Filter_Framer.DataSource = dt3
            'cbo_Filter_Framer.DisplayMember = "Employee_Name"

            'cbo_Filter_Framer.Text = ""
            'cbo_Filter_Machine.Text = ""
            'cbo_Filter_Operator.Text = ""
            'cbo_Filter_Shift.Text = ""

            'cbo_Filter_PartyN.SelectedIndex = -1
            'cbo_Filter_ItemName.SelectedIndex = -1
            'dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False

        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select Production_No from Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Production_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select Production_No from Production_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Production_No", con)
            da.Fill(dt)

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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select Production_No from Production_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Production_No desc"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If
            dr.Close()

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select Production_No from Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Production_No desc", con)
        Dim dt As New DataTable
        Dim movno As String

        Try
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If movno <> "" Then move_record(movno)

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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Production_Head", "Production_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red
            msk_Date.Text = Date.Today.ToShortDateString
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Production_No from Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Production_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Invoice No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR INSERTION...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Production_No from Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Production_Code = '" & Trim(NewCode) & "'"
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
                    lbl_RefNo.Text = Trim(UCase(inpno))

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

        Dim Machine_ID As Integer = 0
        Dim Operator_ID As Integer = 0
        Dim Framer_ID As Integer = 0
        Dim Incharge_ID As Integer = 0

        Dim Led_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotHead As Single = 0
        Dim vTotStchs As Single = 0
        Dim vTotPcs As Single = 0
        Dim vTotAmt As Single = 0
        Dim vforOrdby As Single = 0
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Production_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text))
        Machine_ID = Common_Procedures.Machine_NameToIdNo(con, Trim(cbo_Machine.Text))
        Operator_ID = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_Operator.Text))
        Framer_ID = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_Framer.Text))
        Incharge_ID = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_InCharge.Text))

        'If Led_ID = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    cbo_Ledger.Focus()
        '    Exit Sub
        'End If
        If Machine_ID = 0 Then
            MessageBox.Show("Invalid Machine Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cbo_Machine.Focus()
            Exit Sub
        End If
        If Operator_ID = 0 Then
            MessageBox.Show("Invalid Operator Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cbo_Operator.Focus()
            Exit Sub
        End If
        If Framer_ID = 0 Then
            MessageBox.Show("Invalid Framer Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cbo_Framer.Focus()
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(.Rows(i).Cells(2).Value) <> "" Or Val(.Rows(i).Cells(8).Value) <> 0 Then


                    'If dgv_Details.Rows(i).Cells(1).Value = "" Then
                    '    MessageBox.Show("Invalid Order No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(1)
                    '    End If
                    '    Exit Sub
                    'End If

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Design", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If
                Else
                    If Val(.Rows(i).Cells(10).Value) = 0 Then
                        MessageBox.Show("Invalid Amount", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(10)
                        End If
                        Exit Sub
                    Else
                        MessageBox.Show("Invalid DESIGN", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                    End If

                End If

            Next

        End With

        NoCalc_Status = False
        'Total_Calculation()

        vTotHead = 0 : vTotStchs = 0 : vTotPcs = 0 : vTotAmt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotHead = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotStchs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vTotAmt = Format(Val(dgv_Details_Total.Rows(0).Cells(10).Value()), "##############0.00")
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Production_Head", "Production_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr


            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ProductionDate", dtp_Date.Value.Date)

            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into Production_Head (    Production_Code     ,               Company_IdNo            ,       Production_No           ,         for_OrderBy      ,    Production_Date    ,            Shift               ,        Machine_IdNo   ,       Operator_IdNo                 ,       Framer_IdNo     ,         Total_Heads         ,       Total_Stchs         ,       Total_Pcs       ,       Total_Amt           ,           Remarks                         ,Incharge_IdNo) " &
                                                      " Values (  '" & Trim(NewCode) & "',     " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(vforOrdby)) & ",   @ProductionDate    ,'" & Trim(cbo_shift.Text) & "'  ," & Str(Val(Machine_ID)) & "," & Str(Val(Operator_ID)) & "," & Str(Val(Framer_ID)) & "," & Str(Val(vTotHead)) & "," & Str(Val(vTotStchs)) & "," & Str(Val(vTotPcs)) & "," & Str(Val(vTotAmt)) & ",'" & Trim(txt_Remarks.Text) & "'             ," & Incharge_ID.ToString & ")"
                cmd.ExecuteNonQuery()
            Else
                cmd.CommandText = "Update Production_Head set  Production_Date = @ProductionDate,Shift='" & Trim(cbo_shift.Text) & "', Machine_IdNo=" & Str(Val(Machine_ID)) & ", Operator_IdNo=" & Str(Val(Operator_ID)) & ", Framer_IdNo=" & Str(Val(Framer_ID)) & ", Total_Heads=" & Str(Val(vTotHead)) & ", Total_Stchs=" & Str(Val(vTotStchs)) & ", Total_Pcs=" & Str(Val(vTotPcs)) & ", Total_Amt=" & Str(Val(vTotAmt)) & ", Remarks='" & Trim(txt_Remarks.Text) & "' ,Incharge_IdNo = " & Incharge_ID.ToString & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Production_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_Details

                Sno = 0

                For i = 0 To .Rows.Count - 1

                    If Trim(.Rows(i).Cells(2).Value) <> "" Then

                        'If Led_ID <> 0 Then

                        Sno = Sno + 1
                        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Siz_ID = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        cmd.CommandText = "Insert into Production_Details ( Production_Code     ,       Company_IdNo                ,          Production_No       ,        for_OrderBy         , Production_Date,        Sl_No         ,                 Order_No            ,        Design                             , Colour_IdNo      ,      Size_IdNo       ,          StchsPr_Pcs                     ,               Head                    ,           Stiches                         ,                Pieces                     ,       Rate                                ,                      Amount              ,   Ordercode_forSelection                    ,Ledger_IdNo                                                                               ,Job_No                                  ) " &
                                                                " Values ('" & Trim(NewCode) & "'," & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(vforOrdby)) & ", @ProductionDate, " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(11).Value) & "','" & Trim(.Rows(i).Cells(2).Value) & "',   " & Val(Clr_ID) & " , " & Val(Siz_ID) & " ," & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ",  " & Str(Val(.Rows(i).Cells(9).Value)) & " ," & Str(Val(.Rows(i).Cells(10).Value)) & " ,'" & Trim(.Rows(i).Cells(1).Value) & "', " & Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(12).Value), tr) & ",'" & Trim(.Rows(i).Cells(13).Value) & "')"
                        cmd.ExecuteNonQuery()


                    End If

                Next i

            End With

            tr.Commit()

            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            tr.Dispose()
            cmd.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try



    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click

        Dim n As Integer
        Dim MtchSTS As Boolean

        If Trim(cbo_Ledger.Text) = "" Then
            MessageBox.Show("Invalid Order No", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If
        If Trim(Cbo_OrderCode.Text) = "" Then
            MessageBox.Show("Invalid Order No", "DOES NOT ADD...", MessageBoxButtons.OK)
            If Cbo_OrderCode.Enabled Then Cbo_OrderCode.Focus()
            Exit Sub
        End If
        If Trim(lbl_Grid_Design.Text) = "" Then
            MessageBox.Show("Invalid Design ", "DOES NOT ADD...", MessageBoxButtons.OK)
            If lbl_Grid_Design.Enabled Then lbl_Grid_Design.Focus()
            Exit Sub
        End If
        'If Val(txt_NoOfHeads.Text) = 0 Then
        '    MessageBox.Show("Invalid No of Heads ", "DOES NOT ADD...", MessageBoxButtons.OK)
        '    If txt_NoOfHeads.Enabled Then txt_NoOfHeads.Focus()
        '    Exit Sub
        'End If
        'If Val(txt_NoOfStiches.Text) = 0 Then
        '    MessageBox.Show("Invalid No of Stiches ", "DOES NOT ADD...", MessageBoxButtons.OK)
        '    If txt_NoOfStiches.Enabled Then txt_NoOfStiches.Focus()
        '    Exit Sub
        'End If
        If Val(txt_NoOfPcs.Text) = 0 Then
            MessageBox.Show("Invalid No of Pcs ", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_NoOfPcs.Enabled Then txt_NoOfPcs.Focus()
            Exit Sub
        End If
        'If Val(txt_Rate.Text) = 0 Then
        '    MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
        '    If txt_Rate.Enabled Then txt_Rate.Focus()
        '    Exit Sub
        'End If
        'If Val(lbl_Amount.Text) = 0 Then
        '    MessageBox.Show("Invalid Amount", "DOES NOT ADD...", MessageBoxButtons.OK)
        '    If txt_Rate.Enabled Then txt_Rate.Focus()
        '    Exit Sub
        'End If


        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = Cbo_OrderCode.Text
                    .Rows(i).Cells(2).Value = lbl_Grid_Design.Text
                    .Rows(i).Cells(3).Value = cbo_colour.Text
                    .Rows(i).Cells(4).Value = cbo_Size.Text
                    .Rows(i).Cells(5).Value = Val(txt_Stch_Pcs.Text)
                    .Rows(i).Cells(6).Value = Val(txt_NoOfHeads.Text)
                    .Rows(i).Cells(7).Value = Val(txt_NoOfStiches.Text)
                    .Rows(i).Cells(8).Value = Val(txt_NoOfPcs.Text)
                    .Rows(i).Cells(9).Value = Val(txt_Rate.Text)
                    .Rows(i).Cells(10).Value = Format(Val(lbl_Amount.Text), "########0.00")
                    .Rows(i).Cells(11).Value = lbl_OrderNo.Text
                    .Rows(i).Cells(12).Value = cbo_Ledger.Text
                    .Rows(i).Cells(13).Value = cbo_JobNo.Text
                    .Rows(i).Cells(14).Value = txt_Uom.Text
                    .Rows(i).Selected = True

                    MtchSTS = True
                    Displaying_Saved_Qty = False

                    If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()

                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = Cbo_OrderCode.Text
                .Rows(n).Cells(2).Value = lbl_Grid_Design.Text
                .Rows(n).Cells(3).Value = cbo_colour.Text
                .Rows(n).Cells(4).Value = cbo_Size.Text
                .Rows(n).Cells(5).Value = Val(txt_Stch_Pcs.Text)
                .Rows(n).Cells(6).Value = Val(txt_NoOfHeads.Text)
                .Rows(n).Cells(7).Value = Val(txt_NoOfStiches.Text)
                .Rows(n).Cells(8).Value = Val(txt_NoOfPcs.Text)
                .Rows(n).Cells(9).Value = Val(txt_Rate.Text)
                .Rows(n).Cells(10).Value = Format(Val(lbl_Amount.Text), "########0.00")
                .Rows(n).Cells(11).Value = lbl_OrderNo.Text
                .Rows(n).Cells(12).Value = cbo_Ledger.Text
                .Rows(n).Cells(13).Value = cbo_JobNo.Text
                .Rows(n).Cells(14).Value = txt_Uom.Text

                Displaying_Saved_Qty = False

                .Rows(n).Selected = True

                If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If

        End With



        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        lbl_Grid_Design.Text = ""
        cbo_colour.Text = ""
        cbo_Size.Text = ""
        txt_Stch_Pcs.Text = ""
        txt_Rate.Text = ""
        txt_NoOfHeads.Text = ""
        txt_NoOfPcs.Text = ""
        txt_NoOfStiches.Text = ""
        Cbo_OrderCode.Text = ""
        cbo_JobNo.Text = ""
        lbl_Amount.Text = ""
        lbl_OrderNo.Text = ""
        cbo_Ledger.Text = ""

        If cbo_Ledger.Enabled And Cbo_OrderCode.Visible Then cbo_Ledger.Focus()

    End Sub



    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation()
    End Sub

    'Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

    '    With lbl_Grid_Design
    '        vcmb_ItmNm = Trim(.Text)
    '        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Production_Details", "Design", "", "")
    '    End With

    'End Sub

    'Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

    '    vcbo_KeyDwnVal = e.KeyValue

    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, lbl_Grid_Design, Cbo_OrderCode, cbo_colour, "Production_Details", "Design", "", "")



    'End Sub

    'Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, lbl_Grid_Design, Nothing, "Production_Details", "Design", "", "", False)

    '    If Asc(e.KeyChar) = 13 Then
    '        cbo_colour.Focus()
    '    End If


    'End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = lbl_Grid_Design.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()



        End If
    End Sub





    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        cbo_Ledger.Tag = cbo_Ledger.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, txt_SlNo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And Cbo_OrderCode.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_Ledger.Text) <> "" Then
                Cbo_OrderCode.Focus()
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Cbo_OrderCode, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_Ledger.Text) <> "" Then
                Cbo_OrderCode.Focus()
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub



    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : cbo_Ledger.Focus()
        If e.KeyCode = 38 Then e.Handled = True : cbo_Framer.Focus()

    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then
            cbo_Ledger.Focus()
            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                        lbl_Grid_Design.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
                        cbo_colour.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
                        cbo_Size.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                        txt_Stch_Pcs.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
                        txt_NoOfHeads.Text = Val(dgv_Details.CurrentRow.Cells(6).Value)
                        txt_NoOfStiches.Text = Val(dgv_Details.CurrentRow.Cells(7).Value)
                        txt_NoOfPcs.Text = Val(dgv_Details.CurrentRow.Cells(8).Value)
                        txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")

                        lbl_OrderNo.Text = Trim(.Rows(i).Cells(1).Value)
                        Cbo_OrderCode.Text = Trim(.Rows(i).Cells(11).Value)
                        'txt_box.Text = Val(.Rows(i).Cells(3).Value)
                        'txt_Rate.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.00")
                        'lbl_Amount.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.00")

                        Exit For

                    End If

                Next

            End With

        End If
    End Sub



    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Machine_IdNo As Integer, Framer_IdNo As Integer, Operator_IdNo As Integer, Incharge_IdNo As Integer
        Dim Condt As String = ""
        Dim Shift As String = ""
        Try


            Condt = ""
            Led_IdNo = 0
            Machine_IdNo = 0
            Framer_IdNo = 0
            Operator_IdNo = 0
            Incharge_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Production_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "dd-MMM-yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "dd-MMM-yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Production_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "dd-MMM-yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Production_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "dd-MMM-yyyy")) & "' "
            End If

            If cbo_Filter_Shift.Text <> "" Then
                Shift = Trim(cbo_Filter_Shift.Text)
            End If

            Machine_IdNo = Common_Procedures.Machine_NameToIdNo(con, Trim(cbo_Filter_Machine.Text))
            Framer_IdNo = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_Filter_Framer.Text))
            Operator_IdNo = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_Filter_Operator.Text))
            Incharge_IdNo = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_Filter_Incharge.Text))

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Machine_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Machine_IdNo = " & Str(Val(Machine_IdNo))
            End If

            If Val(Framer_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Framer_IdNo = " & Str(Val(Framer_IdNo))
            End If

            If Val(Operator_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Operator_IdNo = " & Str(Val(Operator_IdNo))
            End If

            If Val(Incharge_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Incharge_IdNo = " & Str(Val(Incharge_IdNo))
            End If

            If Shift <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Shift = '" & Trim(Shift) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*,a.Production_No, a.Production_Date from Production_Head a where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Production_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Production_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Production_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Production_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Trim(dt2.Rows(i).Item("Shift").ToString)
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Common_Procedures.Machine_IdNoToName(con, Val(dt2.Rows(i).Item("Machine_IdNo").ToString))
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Common_Procedures.Employee_IdNoToName(con, Val(dt2.Rows(i).Item("InCharge_IdNo").ToString))
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Common_Procedures.Employee_IdNoToName(con, Val(dt2.Rows(i).Item("Operator_IdNo").ToString))
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Common_Procedures.Employee_IdNoToName(con, Val(dt2.Rows(i).Item("Framer_IdNo").ToString))
                Next i

            End If

            'Production_Code, Ledger_IdNo, Company_IdNo, Production_No, for_OrderBy, Production_Date, Shift, Machine_IdNo, Operator_IdNo, Framer_IdNo, Total_Heads, Total_Stchs, Total_Pcs, Total_Amt

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


    Private Sub Open_FilterEntry()

        Dim movno As String

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        If Not IsNothing(dgv_Details.CurrentCell) Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt As New DataTable
            Dim Siz_idno As Integer = 0
            Dim sqft_qty As Single = 0

            Try
                With dgv_Details
                    If .Visible Then
                        'If Trim(UCase(cbo_EntType.Text)) = "ORDER" Then

                        If .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Then
                            .Rows(.CurrentCell.RowIndex).Cells(10).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value), "#########0.00")

                        End If
                        'End If
                    End If
                End With

            Catch ex As Exception
                '------

            End Try
        End If

    End Sub
    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Panel2.Enabled = True And txt_SlNo.Enabled = True Then


            If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

                txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                Cbo_OrderCode.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                cbo_JobNo.Text = Trim(dgv_Details.CurrentRow.Cells(13).Value)
                lbl_Grid_Design.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
                cbo_colour.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
                cbo_Size.Text = Trim(dgv_Details.CurrentRow.Cells(4).Value)
                cbo_Ledger.Text = Trim(dgv_Details.CurrentRow.Cells(12).Value)
                QuantityDetails()
                txt_Stch_Pcs.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
                txt_NoOfHeads.Text = Val(dgv_Details.CurrentRow.Cells(6).Value)
                txt_NoOfStiches.Text = Val(dgv_Details.CurrentRow.Cells(7).Value)
                txt_NoOfPcs.Text = Val(dgv_Details.CurrentRow.Cells(8).Value)
                txt_Rate.Text = Val(dgv_Details.CurrentRow.Cells(9).Value)
                lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
                lbl_OrderNo.Text = Trim(dgv_Details.CurrentRow.Cells(11).Value)
                txt_Uom.Text = dgv_Details.CurrentRow.Cells(14).Value

                If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

                Displaying_Saved_Qty = True

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

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        lbl_Grid_Design.Text = ""
        cbo_colour.Text = ""
        cbo_Size.Text = ""
        txt_Stch_Pcs.Text = ""
        txt_Rate.Text = ""
        txt_NoOfHeads.Text = ""
        txt_NoOfPcs.Text = ""
        txt_NoOfStiches.Text = ""
        Cbo_OrderCode.Text = ""
        lbl_Amount.Text = ""
        lbl_OrderNo.Text = ""

        If Cbo_OrderCode.Enabled And Cbo_OrderCode.Visible Then Cbo_OrderCode.Focus()

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



            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            lbl_Grid_Design.Text = ""
            cbo_colour.Text = ""
            cbo_Size.Text = ""
            txt_Stch_Pcs.Text = ""
            txt_Rate.Text = ""
            txt_NoOfHeads.Text = ""
            txt_NoOfPcs.Text = ""
            txt_NoOfStiches.Text = ""
            Cbo_OrderCode.Text = ""
            lbl_Amount.Text = ""
            lbl_OrderNo.Text = ""

            If Cbo_OrderCode.Enabled And Cbo_OrderCode.Visible Then Cbo_OrderCode.Focus()

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

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        prn_Status = 2
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 1
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub Amount_Calculation()
        ' txt_NoOfStiches.Text = Val(txt_Stch_Pcs.Text) * Val(txt_NoOfPcs.Text)
        lbl_Amount.Text = Format(Val(txt_NoOfPcs.Text) * Val(txt_Rate.Text), "#########0.00")

        Dim vTotHead As Single = 0
        Dim vTotStchs As Single = 0
        Dim vTotPcs As Single = 0
        Dim vTotAmt As Single = 0

        vTotHead = 0 : vTotStchs = 0 : vTotPcs = 0 : vTotAmt = 0

        For i = 0 To dgv_Details.RowCount - 1
            vTotHead = vTotHead + Val(dgv_Details.Rows(i).Cells(6).Value())
            vTotStchs = vTotStchs + Val(dgv_Details.Rows(i).Cells(7).Value())
            vTotPcs = vTotPcs + Val(dgv_Details.Rows(i).Cells(8).Value())
            vTotAmt = vTotAmt + Val(dgv_Details.Rows(i).Cells(10).Value())
        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(6).Value = vTotHead
            .Rows(0).Cells(7).Value = vTotStchs
            .Rows(0).Cells(8).Value = vTotPcs
            .Rows(0).Cells(10).Value = Format(vTotAmt, "##############0.00")
        End With
    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record


        InvPrintFrmt = Common_Procedures.settings.InvoicePrint_Format

        prn_Status = 2
        printing_invoice()

    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim CmpName As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Production_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Production_Code = '" & Trim(NewCode) & "'", con)
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

        If InvPrintFrmt_Letter <> 1 Then
            If prn_Status <> 1 Then
                prn_InpOpts = ""
                If Trim(UCase(InvPrintFrmt)) <> "FORMAT-6" Then
                    prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "     1. Original" & Space(5) & "    2. Duplicate" & Chr(13) & "     3. Triplicate" & Space(3) & "   4. Transport Copy" & Chr(13) & "     5. Extra Copy  " & Space(1) & "6.All", "FOR INVOICE PRINTING...", "12345")
                    prn_InpOpts = Replace(Trim(prn_InpOpts), "6", "12345")
                End If
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" And prn_Status = 1 Then
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" And (Microsoft.VisualBasic.Left(Trim(UCase(CmpName)), 3) = "BNC" And Microsoft.VisualBasic.InStr(1, Trim(UCase(CmpName)), "GARMENT") > 0) Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        Else

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Then
                    PrintDocument1.Print()

                Else
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1


                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                ppd.Document.DefaultPageSettings.PaperSize = pkCustomSize1

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* ,D.*,E.Ledger_Name AS agent_name from Production_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo LEFT OUTER JOIN  Transport_Head D ON A.Transport_IdNo = D.Transport_IdNo LEFT OUTER JOIN Ledger_Head E ON A.Agent_idno = E.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Production_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name, IG.ItemGroup_Name from Production_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno LEFT OUTER JOIN  ItemGroup_Head IG ON b.ItemGroup_IdNo = IG.ItemGroup_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Production_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

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

        Printing_Format2(e)

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim QlNm1 As String, QlNm2 As String
        Dim ps As Printing.PaperSize

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 30 '60
            .Right = 60
            .Top = 20 '40 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        TxtHgt = 18.9 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 17 ' 13  ' 12

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(130)
        ClArr(2) = 280 : ClArr(3) = 60 : ClArr(4) = 90 : ClArr(5) = 70
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            QlNm1 = "100% COTTON GOODS HOSIERY"
                            QlNm2 = ""
                            If Len(QlNm1) > 15 Then
                                For I = 15 To 1 Step -1
                                    If Mid$(Trim(QlNm1), I, 1) = " " Or Mid$(Trim(QlNm1), I, 1) = "," Or Mid$(Trim(QlNm1), I, 1) = "." Or Mid$(Trim(QlNm1), I, 1) = "-" Or Mid$(Trim(QlNm1), I, 1) = "/" Or Mid$(Trim(QlNm1), I, 1) = "_" Or Mid$(Trim(QlNm1), I, 1) = "(" Or Mid$(Trim(QlNm1), I, 1) = ")" Or Mid$(Trim(QlNm1), I, 1) = "\" Or Mid$(Trim(QlNm1), I, 1) = "[" Or Mid$(Trim(QlNm1), I, 1) = "]" Or Mid$(Trim(QlNm1), I, 1) = "{" Or Mid$(Trim(QlNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 15
                                QlNm2 = Microsoft.VisualBasic.Right(Trim(QlNm1), Len(QlNm1) - I)
                                QlNm1 = Microsoft.VisualBasic.Left(Trim(QlNm1), I - 1)
                            End If


                            ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Item_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 30 Then
                                For I = 30 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 30
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1
                            If DetIndx = 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(QlNm1), LMargin + 10, CurY, 0, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, "", LMargin + 10, CurY, 0, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), "########0"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)


                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(QlNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                If DetIndx = 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(QlNm2), LMargin + 10, CurY, 0, 0, pFont)
                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, "", LMargin + 10, CurY, 0, 0, pFont)
                                End If
                                NoofDets = NoofDets + 1
                            End If
                            DetIndx = DetIndx + 1

                        Loop

                    End If


                    Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)
                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0
                                DetIndx = 0
                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Public Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String, Led_CSTNo As String
        Dim LedAr(10) As String
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim Cen1 As Single = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W1 As Single = 0, W2 As Single = 0, W3 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim Yinc As Integer = 0
        Dim Cmp_Email As String = ""
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Production_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Production_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        If PageNo = 1 Then
            prn_Count = prn_Count + 1
        End If


        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "TRANSPORT COPY"
                ElseIf Val(S) = 5 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Email = ""


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
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "Email : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.Balaji_Graments_Logo, Drawing.Image), LMargin + 15, CurY - 70, 150, 100)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, LMargin, CurY, 2, PrintWidth, pFont)


        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'p1Font = New Font("Calibri", 22, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY + 2, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY + 2, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try


            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_CSTNo = ""

            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)   ' Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)
            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
            Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            Led_CSTNo = Trim(prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString)

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

            '' If Trim(Led_TinNo) <> "" Then
            Indx = Indx + 1
            LedAr(Indx) = "TIN No : " & Trim(Led_TinNo)
            '' End If
            '   If Trim(Led_CSTNo) <> "" Then
            Indx = Indx + 1
            LedAr(Indx) = "CST No : " & Trim(Led_CSTNo)
            '   End If

            Cen1 = ClAr(1) + ClAr(2) + (ClAr(3) / 2) - 50
            HdWd = PageWidth - Cen1 - LMargin

            H1 = e.Graphics.MeasureString("TO    :", pFont).Width
            W1 = e.Graphics.MeasureString("ORDER DATE:", pFont).Width
            W2 = e.Graphics.MeasureString("ORDER DATE:", pFont).Width + 50
            Yinc = 5

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            'p1Font = New Font("Calibri", 18, FontStyle.Bold)
            ''p1Font = New Font("Calibri", 22, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + Cen1, CurY - 10, 2, HdWd, p1Font)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO.", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Production_No").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Dt.", LMargin + Cen1 + W1 + W2, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 20, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Production_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + W2 + 30, CurY + Yinc, 0, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Led_Name, LMargin + H1 + 10, CurY, 0, 0, p1Font)


            Common_Procedures.Print_To_PrintDocument(e, "ORDER NO.", LMargin + Cen1 + 10, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + Cen1 + W1 + 30, CurY + Yinc, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Dt.", LMargin + Cen1 + W1 + W2, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 20, CurY + Yinc, 0, 0, pFont)
            If IsDate(prn_HdDt.Rows(0).Item("Order_Date").ToString) Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Order_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + W2 + 30, CurY + Yinc, 0, 0, pFont)
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(2), LMargin + H1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "LR NO.", LMargin + Cen1 + 10, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + Cen1 + W1 + 30, CurY + Yinc, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Dt.", LMargin + Cen1 + W1 + W2, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 20, CurY + Yinc, 0, 0, pFont)
            If IsDate(prn_HdDt.Rows(0).Item("Lr_Date").ToString) Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Lr_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + W2 + 30, CurY + Yinc, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(3), LMargin + H1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "AGENT", LMargin + Cen1 + 10, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("agent_name").ToString, LMargin + Cen1 + W1 + 30, CurY + Yinc, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4), LMargin + H1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "LORRY/TRAIN", LMargin + Cen1 + 10, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + Cen1 + W1 + 30, CurY + Yinc, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "TO", LMargin + Cen1 + W1 + W2, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 20, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + Cen1 + W1 + W2 + 30, CurY + Yinc, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(5), LMargin + H1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + Cen1 + 10, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 20, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weight").ToString, LMargin + Cen1 + W1 + 30, CurY + Yinc, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "CHARGES", LMargin + Cen1 + W1 + W2, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + W2 + 60, CurY + Yinc, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString, LMargin + Cen1 + W1 + W2 + 60, CurY + Yinc, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(6), LMargin + H1 + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(7), LMargin + H1 + 10, CurY, 0, 0, pFont)




            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, LnAr(3), LMargin + Cen1, LnAr(2))

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "DOCUMENTS THROUGH : ", LMargin + 10, CurY, 0, PrintWidth, p1Font)
            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Document_Through").ToString, LMargin + 170, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PATICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim CurY1 As Single = 0
        Dim Str1 As String = ""
        Dim Str2 As String = ""
        Dim Juris As String = ""
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            Common_Procedures.Print_To_PrintDocument(e, "BUNDLES : " & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "######0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(6), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(6), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            CurY1 = CurY
            CurY = CurY + TxtHgt

            CurY1 = CurY1 + 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Erase BnkDetAr
            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                BInc = -1

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm2 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm3 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm4 = Trim(BnkDetAr(BInc))
                End If


            End If
            Common_Procedures.Print_To_PrintDocument(e, BankNm1 & " " & BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3 & " " & BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)

            If BankNm1 <> "" Then
                CurY1 = CurY1 + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY1)

            End If

            If Val(prn_HdDt.Rows(0).Item("Against_CForm_Status").ToString) = 1 Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                CurY1 = CurY1 - 10
                Common_Procedures.Print_To_PrintDocument(e, "Sales Against 'C' Form.", LMargin + 10, CurY1, 0, 0, p1Font)
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Underline)
            CurY1 = CurY1 + 30
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions", LMargin + 10, CurY1, 0, 0, p1Font)

            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "* Excise Duty is not Charged for this Consignment Since exempted under SSI ", LMargin + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "exemption scheme vide Notification No. 8/2003 and 9/2003 dt 01.03.2013 *", LMargin + 10, CurY1, 0, 0, p1Font)

            Juris = Trim(Common_Procedures.settings.Jurisdiction)
            If Trim(Juris) = "" Then Juris = "TIRUPUR"


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Any Disputes subject to " & Juris & " Jurisdiction.", LMargin + 10, CurY1, 0, 0, p1Font)




            If Trim(prn_HdDt.Rows(0).Item("Booked_By").ToString) <> "" Then     ' ---Payment Terms
                p1Font = New Font("Calibri", 9, FontStyle.Bold)
                CurY1 = CurY1 + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Payment Terms : " & Trim(prn_HdDt.Rows(0).Item("Booked_By").ToString), LMargin + 10, CurY1, 0, 0, p1Font)
            End If


            If Val(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Trade Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("Extra_Charges").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Extra_Copies").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 10
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 10
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 15

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)

            If CurY1 > CurY Then
                CurY = CurY1
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            LnAr(7) = CurY


            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            'p1Font = New Font("Calibri", 15, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "E&OE", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Rupees : " & Rup1, LMargin + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)

            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + 15, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Qty As Single, Ent_Rate As Single, Ent_PurcRet_Qty As Single
        Dim Ent_DetSlNo As Long

        ''If Trim(UCase(cbo_EntType.Text)) <> "ORDER" Then
        ''    MessageBox.Show("Invalid Type", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ''    If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
        ''    Exit Sub
        ''End If

        'LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
        '    Exit Sub
        'End If

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'With dgv_Selection

        '    .Rows.Clear()

        '    SNo = 0

        '    Da = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, e.Size_Name, f.Noof_Items as Ent_Production_Quantity, f.Rate as Ent_Rate, f.Production_Detail_SlNo as Ent_Production_SlNo from Production_Order_Details a INNER JOIN Item_Head b ON a.Item_idno = b.Item_idno  LEFT OUTER JOIN Size_Head e ON a.Size_IdNo = e.Size_IdNo LEFT OUTER JOIN Production_Details F ON f.Production_Code = '" & Trim(NewCode) & "' and f.Entry_Type = '" & Trim(cbo_EntType.Text) & "' and a.Production_Order_Code = f.Production_Order_Code and a.Production_Order_Detail_SlNo = f.Production_Order_Detail_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.Noof_Items  - a.Production_Items ) > 0 or f.Noof_Items > 0 ) Order by a.For_OrderBy, a.Production_Order_No, a.Production_Order_Detail_SlNo", con)
        '    Dt1 = New DataTable
        '    Da.Fill(Dt1)

        '    If Dt1.Rows.Count > 0 Then

        '        For i = 0 To Dt1.Rows.Count - 1

        '            Ent_Qty = 0 : Ent_Rate = 0 : Ent_DetSlNo = 0 : Ent_PurcRet_Qty = 0

        '            If IsDBNull(Dt1.Rows(i).Item("Ent_Production_SlNo").ToString) = False Then Ent_DetSlNo = Val(Dt1.Rows(i).Item("Ent_Production_SlNo").ToString)
        '            If IsDBNull(Dt1.Rows(i).Item("Ent_Production_Quantity").ToString) = False Then Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Production_Quantity").ToString)
        '            If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
        '            ' If IsDBNull(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString) = False Then Ent_PurcRet_Qty = Val(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString)

        '            If (Val(Dt1.Rows(i).Item("Noof_Items").ToString) - Val(Dt1.Rows(i).Item("Production_Items").ToString) + Ent_Qty) > 0 Then

        '                n = .Rows.Add()

        '                SNo = SNo + 1

        '                .Rows(n).Cells(0).Value = Val(SNo)

        '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Production_Order_No").ToString
        '                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Item_name").ToString
        '                .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Size_Name").ToString
        '                .Rows(n).Cells(4).Value = (Val(Dt1.Rows(i).Item("Noof_Items").ToString) - Val(Dt1.Rows(i).Item("Production_Items").ToString) + Ent_Qty)
        '                .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "########0.00")
        '                If Val(Ent_Qty) > 0 Then
        '                    .Rows(n).Cells(6).Value = "1"
        '                Else
        '                    .Rows(n).Cells(6).Value = ""
        '                End If
        '                .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Production_Order_Code").ToString
        '                .Rows(n).Cells(8).Value = Val(Dt1.Rows(i).Item("Production_Order_Detail_SlNo").ToString)
        '                .Rows(n).Cells(9).Value = Val(Ent_DetSlNo)
        '                .Rows(n).Cells(10).Value = Val(Ent_Qty)
        '                .Rows(n).Cells(11).Value = Val(Ent_Rate)
        '                .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("Bags").ToString)

        '                If Val(Ent_Qty) > 0 Then

        '                    For j = 0 To .ColumnCount - 1
        '                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
        '                    Next

        '                End If

        '            End If

        '        Next

        '    End If
        '    Dt1.Clear()

        '    If .Rows.Count = 0 Then
        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = "1"
        '    End If

        'End With

        'pnl_Selection.Visible = True
        'pnl_Selection.BringToFront()
        'pnl_Back.Enabled = False

        dgv_Selection.Focus()
        dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        dgv_Selection.CurrentCell.Selected = True

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Grid_Selection(e.RowIndex)
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(4).Value) = 0 And Trim(.Rows(RwIndx).Cells(7).Value) = "" Then Exit Sub

                'If Val(.Rows(RwIndx).Cells(14).Value) <> 0 Then
                '    MessageBox.Show("Already some items returned, cannot de-select.", "DOES NOT DE-SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    Exit Sub
                'End If

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 0 Then

                    .Rows(RwIndx).Cells(6).Value = ""

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

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        With dgv_Selection

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                    e.Handled = True
                    Grid_Selection(dgv_Selection.CurrentCell.RowIndex)
                End If
            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Ent_Qty As Single, Ent_Rate As Single

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

                If Val(dgv_Selection.Rows(i).Cells(10).Value) <> 0 Then
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(10).Value)

                Else
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(4).Value)

                End If

                If Val(dgv_Selection.Rows(i).Cells(10).Value) <> 0 Then
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(10).Value)

                Else
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(5).Value)

                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(4).Value = Val(Ent_Qty)
                dgv_Details.Rows(n).Cells(5).Value = Val(Ent_Rate)
                dgv_Details.Rows(n).Cells(6).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value
                '   dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value

            End If

        Next i

        NoCalc_Status = False



        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        'txt_BillNo.Focus()
        'cbo_EntType.Enabled = False

        'If dgv_Details.Rows.Count > 0 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
        '    dgv_Details.CurrentCell.Selected = True
        '    cbo_EntType.Enabled = False
        '    Panel2.Enabled = False
        'Else
        '    txt_TradeDiscPerc.Focus()

        'End If

    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub


    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = Nothing
        If dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5 Then
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        End If
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then
                    'If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                    '    e.Handled = True
                    '    e.SuppressKeyPress = True
                    'End If
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Siz_idno As Integer = 0
        Dim sqft_qty As Single = 0


        With dgv_Details
            If .Visible Then

                'If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                '    e.Handled = True
                'End If
                If .CurrentCell.ColumnIndex = 4 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = 5 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
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

                        If .CurrentCell.RowIndex >= .Rows.Count - 1 Then

                            'txt_AddLess.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(4)


                        End If


                    ElseIf .CurrentCell.ColumnIndex < 4 Then
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(4)

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= 4 Then
                        If .CurrentCell.RowIndex = 0 Then
                            If Panel2.Enabled = True And lbl_Grid_Design.Enabled = True Then
                                lbl_Grid_Design.Focus()

                            Else
                                'cbo_EntType.Focus()

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

    Private Sub cbo_ItemName_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        getItemDetails()
    End Sub

    Private Sub cbo_ItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        getItemDetails()

    End Sub

    Private Sub cbo_Size_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        getItemDetails()
    End Sub

    Private Sub cbo_Size_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        getItemDetails()
    End Sub

    Private Sub cbo_Size_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        getItemDetails()
    End Sub
    Private Sub getItemDetails()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable


        Try
            'If (Trim(UCase(vcmb_ItmNm)) <> Trim(UCase(cbo_ItemName.Text)) And Trim(cbo_ItemName.Text) <> "" And Trim(cbo_ItemName.Text) <> "System.Data.DataRowView") Or (Trim(vcmb_SizNm) <> Trim(cbo_Size.Text) And Trim(cbo_Size.Text) <> "" And Trim(cbo_Size.Text) <> "System.Data.DataRowView") Then

            '    da1 = New SqlClient.SqlDataAdapter("select sum(Quantity) as stock from Item_Processing_Details where Item_IdNo = " & Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text) & " and  Size_Idno =" & Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text), con)
            '    da1.Fill(dt1)
            '    lbl_ItemStock.Text = ""
            '    If dt1.Rows.Count > 0 Then
            '        lbl_ItemStock.Text = Val(dt1.Rows(0).Item("stock").ToString)
            '    End If
            '    dt1.Clear()

            '    dt1.Dispose()
            '    da1.Dispose()

            '    da1 = New SqlClient.SqlDataAdapter("select  Piece_Box ,Production_rate from Item_Details where Item_IdNo = " & Common_Procedures.Item_NameToIdNo(con, Trim(cbo_ItemName.Text)) & " and  Size_IdNo =" & Common_Procedures.Size_NameToIdNo(con, Trim(cbo_Size.Text)), con)
            '    da1.Fill(dt1)

            '    If dt1.Rows.Count > 0 Then
            '        txt_Pc_Box.Text = Val(dt1.Rows(0).Item("Piece_Box").ToString)
            '        txt_Rate.Text = Val(dt1.Rows(0).Item("Production_rate").ToString)
            '    End If
            '    dt1.Clear()

            '    dt1.Dispose()
            '    da1.Dispose()
            'End If


        Catch ex As Exception

        End Try
    End Sub


    Private Sub cbo_Framer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Framer.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Framer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Framer.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Framer, cbo_Operator, txt_SlNo, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If e.KeyValue = 40 And cbo_Framer.DroppedDown = False Then
            e.Handled = True
            On Error Resume Next
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_Framer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Framer.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Framer, txt_SlNo, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 And cbo_Framer.DroppedDown = False Then
        '    e.Handled = True
        '    txt_SlNo.Focus()
        'End If
    End Sub

    Private Sub cbo_Framer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Framer.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Framer.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Machine_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Machine.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Machine_Head", "Machine_Name", "", "(Machine_IdNo = 0)")
    End Sub

    Private Sub cbo_Machine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Machine.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Machine, cbo_InCharge, cbo_Operator, "Machine_Head", "Machine_Name", "", "(Machine_IdNo = 0)")
    End Sub

    Private Sub cbo_Machine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Machine.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Machine, cbo_Operator, "Machine_Head", "Machine_Name", "", "(Machine_IdNo = 0)")
    End Sub

    Private Sub cbo_Machine_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Machine.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Machine_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Machine.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Operator_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Operator.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Operator_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Operator.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Operator, cbo_Machine, cbo_Framer, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Operator_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Operator.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Operator, cbo_Framer, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Operator_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Operator.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Operator.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_shift, cbo_Ledger, cbo_InCharge, "", "", "", "")
    End Sub

    Private Sub cbo_shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_shift, cbo_InCharge, "", "", "", "")
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

        If e.KeyValue = 40 Then
            cbo_shift.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            cbo_shift.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

        If IsDate(msk_Date.Text) = True Then

            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

            If CDate(Common_Procedures.settings.Validation_End_Date) > Common_Procedures.settings.Validation_End_Date Then
                msk_Date.Text = Format(Common_Procedures.settings.Validation_End_Date, "dd-MM-yyyy")
                MsgBox("Your Trial Period Is Over")
            End If


        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        msk_Date.Text = ""
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub cbo_Filter_Framer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Framer.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Framer, dtp_Filter_ToDate, btn_Filter_Show, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Framer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Framer.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Framer, Nothing, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_NoOfPcs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.TextChanged

        Amount_Calculation()

        If (Val(txt_NoOfPcs.Text) > Val(b2.Text)) Or Val(txt_NoOfPcs.Text) = 0 Then
            txt_NoOfPcs.BackColor = Color.Red
            txt_NoOfPcs.ForeColor = Color.Yellow
            btn_Add.Enabled = False
            Beep()
        Else
            txt_NoOfPcs.BackColor = Color.White
            txt_NoOfPcs.ForeColor = Color.Black
            btn_Add.Enabled = True
        End If

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False

    End Sub

    Private Sub cbo_Filter_Machine_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Machine.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Machine_Head", "Machine_Name", "", "(Machine_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Machine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Machine.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Machine, cbo_Filter_Shift, cbo_Filter_Incharge, "Machine_Head", "Machine_Name", "", "(Machine_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Machine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Machine.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Machine, cbo_Filter_Incharge, "Machine_Head", "Machine_Name", "", "(Machine_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Shift, dtp_Filter_ToDate, cbo_Filter_Machine, "", "", "", "")
    End Sub

    Private Sub cbo_Filter_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Shift, Nothing, "", "", "", "")
    End Sub

    Private Sub cbo_Filter_Operator_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Operator.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Operator, cbo_Filter_Machine, cbo_Filter_Framer, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Operator_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Operator.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Operator, cbo_Filter_Framer, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub txt_NoOfHeads_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoOfHeads.TextChanged
        txt_NoOfStiches.Text = Format(Val(txt_Stch_Pcs.Text) * Val(txt_NoOfHeads.Text), "#######0")
    End Sub

    Private Sub txt_Stch_Pcs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Stch_Pcs.TextChanged
        txt_NoOfStiches.Text = Format(Val(txt_Stch_Pcs.Text) * Val(txt_NoOfHeads.Text), "#######0")
    End Sub

    Private Sub txt_NoOfPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfPcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoOfStiches_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfStiches.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Cbo_OrderCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_OrderCode.GotFocus


    End Sub

    Private Sub Cbo_OrderCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_OrderCode.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_OrderCode, cbo_Ledger, cbo_JobNo, "Order_Program_Head", "Ordercode_forSelection", " Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, ""), "")

    End Sub

    Private Sub Cbo_OrderCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_OrderCode.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_OrderCode, cbo_JobNo, "Order_Program_Head", "Ordercode_forSelection", " Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, ""), "")

    End Sub

    Private Sub Cbo_OrderCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_OrderCode.LostFocus


    End Sub

    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "size_head", "size_name", "Size_IdNo In (Select Size_IdNo from Simple_Receipt_Details " &
                                                               " Where OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Colour_IdNo = " &
                                                               Common_Procedures.Colour_NameToIdNo(con, cbo_colour.Text) & ")", "(Size_IdNo = 0)")
    End Sub

    Private Sub cbo_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, cbo_colour, txt_Stch_Pcs, "size_head", "size_name", "Size_IdNo In (Select Size_IdNo from Simple_Receipt_Details " &
                                                               " Where OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Colour_IdNo = " &
                                                               Common_Procedures.Colour_NameToIdNo(con, cbo_colour.Text) & ")", "(size_idno = 0)")
    End Sub

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, txt_Stch_Pcs, "size_head", "size_name", "Size_IdNo In (Select Size_IdNo from Simple_Receipt_Details " &
                                                               " Where OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Colour_IdNo = " &
                                                               Common_Procedures.Colour_NameToIdNo(con, cbo_colour.Text) & ")", "(size_idno = 0)")
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
    Private Sub cbo_colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_colour.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_head", "Colour_name", "Colour_IdNo In (Select Colour_IdNo from Simple_Receipt_Details " &
                                                               " Where OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "')", "(Colour_IdNo = 0)")

        cbo_colour.Tag = cbo_colour.Text

    End Sub

    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_colour, lbl_Grid_Design, cbo_Size, "Colour_head", "Colour_name", "Colour_IdNo In (Select Colour_IdNo from Simple_Receipt_Details " &
                                                               " Where OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "')", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_colour, cbo_Size, "Colour_head", "Colour_name", "Colour_IdNo In (Select Colour_IdNo from Simple_Receipt_Details " &
                                                               " Where OrderCode_forSelection = '" & Cbo_OrderCode.Text & "'  and Job_No = '" & cbo_JobNo.Text & "')", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub QuantityDetails(Optional ForceDisplayQty As Boolean = True)

        t1.Text = Common_Procedures.get_FieldValue(con, "Simple_Receipt_Details", "sum(Quantity)", "OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Simple_Receipt_Code Like 'EMREC%'", Common_Procedures.CompIdNo)
        t2.Text = Common_Procedures.get_FieldValue(con, "Simple_Receipt_Details", "sum(Quantity)", "OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Simple_Receipt_Code Like 'EMREC%' " &
                                                   " AND Colour_IdNo = '" & Common_Procedures.Colour_NameToIdNo(con, cbo_colour.Text) & "' and Size_IdNo = '" & Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text) & "'", Common_Procedures.CompIdNo)

        s1.Text = Common_Procedures.get_FieldValue(con, "Embroidery_Jobwork_Delivery_Details", "sum(Quantity)", "OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Embroidery_Jobwork_Delivery_Code Like 'EMJDC-%'", Common_Procedures.CompIdNo)
        s2.Text = Common_Procedures.get_FieldValue(con, "Embroidery_Jobwork_Delivery_Details", "sum(Quantity)", "OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Embroidery_Jobwork_Delivery_Code Like 'EMJDC-%' " &
                                                        " AND Colour_IdNo = '" & Common_Procedures.Colour_NameToIdNo(con, cbo_colour.Text) & "' and Size_IdNo = '" & Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text) & "'", Common_Procedures.CompIdNo)

        p1.Text = Common_Procedures.get_FieldValue(con, "Production_Details", "sum(Pieces)", "OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Production_Code Like 'GPROD-%' AND NOT PRODUCTION_CODE = " &
                                                        " '" & Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "'", Common_Procedures.CompIdNo)
        p2.Text = Common_Procedures.get_FieldValue(con, "Production_Details", "sum(Pieces)", "OrderCode_forSelection = '" & Cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNo.Text & "' and Production_Code Like 'GPROD-%' " &
                                                   " AND Colour_IdNo = '" & Common_Procedures.Colour_NameToIdNo(con, cbo_colour.Text) & "' and Size_IdNo = '" & Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text) & "' and Production_Code Like 'GPROD-%' AND NOT PRODUCTION_CODE = " &
                                                   " '" & Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "'", Common_Procedures.CompIdNo)

        pa1.Text = "0"
        pa2.Text = "0"

        With dgv_Details
            For I As Int16 = 0 To .Rows.Count - 1
                If Val(.Rows(I).Cells(0).Value) <> Val(txt_SlNo.Text) Then
                    If Cbo_OrderCode.Text = .Rows(I).Cells(1).Value Then
                        pa1.Text = Val(pa1.Text) + .Rows(I).Cells(8).Value
                        If cbo_colour.Text = .Rows(I).Cells(3).Value And cbo_Size.Text = .Rows(I).Cells(4).Value Then
                            pa2.Text = Val(pa2.Text) + .Rows(I).Cells(8).Value
                        End If
                    End If
                End If
            Next
        End With



        If Val(txt_NoOfPcs.Text) = 0 Or New_Entry = True Or ForceDisplayQty Then
            txt_NoOfPcs.Text = b2.Text
        End If

        If (Val(txt_NoOfPcs.Text) > Val(b2.Text)) Or Val(txt_NoOfPcs.Text) = 0 Then
            If Val(txt_NoOfPcs.Text) <> 0 Then
                txt_NoOfPcs.BackColor = Color.Red
                txt_NoOfPcs.ForeColor = Color.Yellow
            End If

            btn_Add.Enabled = False
            Beep()

        Else
            txt_NoOfPcs.BackColor = Color.White
            txt_NoOfPcs.ForeColor = Color.Black
            btn_Add.Enabled = True
        End If

        If Val(txt_NoOfPcs.Text) = 0 Then
            txt_NoOfPcs.BackColor = Color.White
            txt_NoOfPcs.ForeColor = Color.Black
        End If

    End Sub

    Private Sub t1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles t1.TextChanged

        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(p1.Text) - Val(pa1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(p2.Text) - Val(pa2.Text)

    End Sub


    Private Sub t2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles t2.TextChanged

        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(p1.Text) - Val(pa1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(p2.Text) - Val(pa2.Text)

    End Sub

    Private Sub s1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles s1.TextChanged

        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(p1.Text) - Val(pa1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(p2.Text) - Val(pa2.Text)

    End Sub

    Private Sub s2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles s2.TextChanged

        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(p1.Text) - Val(pa1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(p2.Text) - Val(pa2.Text)

    End Sub

    Private Sub p1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles p1.TextChanged

        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(p1.Text) - Val(pa1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(p2.Text) - Val(pa2.Text)

    End Sub

    Private Sub p2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles p2.TextChanged

        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(p1.Text) - Val(pa1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(p2.Text) - Val(pa2.Text)

    End Sub

    Private Sub btn_DelPending_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_DelPending.Click
        QuantityDetails()
        grp_Quantity_Details.Visible = True
        dgv_Details.Enabled = False
    End Sub

    Private Sub btn_Close_Quantity_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Quantity_Details.Click
        grp_Quantity_Details.Visible = False
        dgv_Details.Enabled = True
    End Sub

    Private Sub Cbo_OrderCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_OrderCode.SelectedIndexChanged

    End Sub

    Private Sub dgv_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub b1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles b1.TextChanged

        If Val(txt_NoOfPcs.Text) > Val(b1.Text) Or Val(txt_NoOfPcs.Text) > Val(b2.Text) Then
            txt_NoOfPcs.BackColor = Color.Red
            txt_NoOfPcs.ForeColor = Color.Yellow
            Beep()
        Else
            txt_NoOfPcs.BackColor = Color.White
            txt_NoOfPcs.ForeColor = Color.Black
        End If

        'If Displaying_Saved_Qty = False Then
        txt_NoOfPcs.Text = b2.Text
        'End If

    End Sub

    Private Sub b2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles b2.TextChanged
        If Val(txt_NoOfPcs.Text) > Val(b1.Text) Or Val(txt_NoOfPcs.Text) > Val(b2.Text) Then
            txt_NoOfPcs.BackColor = Color.Red
            txt_NoOfPcs.ForeColor = Color.Yellow
            Beep()
        Else
            txt_NoOfPcs.BackColor = Color.White
            txt_NoOfPcs.ForeColor = Color.Black
        End If
    End Sub

    Private Sub chk_ShowOnlyActiveOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_ShowOnlyActiveOrders.CheckedChanged
        If chk_ShowOnlyActiveOrders.Checked Then
            Order_Disp_Cond = "Close_Status = 0"
        Else
            Order_Disp_Cond = ""
        End If
    End Sub

    Private Sub cbo_colour_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_colour.LostFocus

        If cbo_colour.Tag <> cbo_colour.Text Then

            cbo_Size.Text = ""
            'txt_Stch_Pcs.Text = ""
            txt_NoOfHeads.Text = ""
            txt_NoOfPcs.Text = ""
            txt_NoOfStiches.Text = ""
            lbl_Amount.Text = ""

        End If

        QuantityDetails(True)

    End Sub

    Private Sub cbo_colour_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_colour.SelectedIndexChanged

    End Sub

    Private Sub lbl_Grid_Design_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub lbl_Grid_Design_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        QuantityDetails(True)

    End Sub

    Private Sub cbo_Size_LostFocus1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.LostFocus
        QuantityDetails(True)

    End Sub

    Private Sub cbo_Size_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Size.SelectedIndexChanged

    End Sub

    Private Sub cbo_Operator_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Operator.SelectedIndexChanged

    End Sub

    Private Sub cbo_Machine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Machine.SelectedIndexChanged

    End Sub

    Private Sub cbo_shift_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_shift.SelectedIndexChanged

    End Sub

    Private Sub cbo_InCharge_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_InCharge.SelectedIndexChanged

    End Sub

    Private Sub cbo_InCharge_GotFocus(sender As Object, e As EventArgs) Handles cbo_InCharge.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_InCharge_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_InCharge.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_InCharge, cbo_shift, cbo_Machine, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_InCharge_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_InCharge.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_InCharge, cbo_Machine, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_InCharge_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_InCharge.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_InCharge.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub pa1_TextChanged(sender As Object, e As EventArgs) Handles pa1.TextChanged
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(p1.Text) - Val(pa1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(p2.Text) - Val(pa2.Text)
    End Sub

    Private Sub pa2_TextChanged(sender As Object, e As EventArgs) Handles pa2.TextChanged
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(p1.Text) - Val(pa1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(p2.Text) - Val(pa2.Text)
    End Sub

    Private Sub cbo_Ledger_LostFocus(sender As Object, e As EventArgs) Handles cbo_Ledger.LostFocus
        If cbo_Ledger.Tag <> cbo_Ledger.Text Then

            lbl_Grid_Design.Text = ""
            lbl_OrderNo.Text = ""
            cbo_colour.Text = ""
            cbo_Size.Text = ""
            txt_Stch_Pcs.Text = ""
            txt_Rate.Text = ""
            txt_NoOfHeads.Text = ""
            txt_NoOfPcs.Text = ""
            txt_NoOfStiches.Text = ""
            Cbo_OrderCode.Text = ""
            lbl_Amount.Text = ""

        End If
    End Sub

    Private Sub cbo_Filter_Incharge_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_Incharge.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Incharge, cbo_Filter_Machine, cbo_Filter_Operator, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Incharge_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_Incharge.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Incharge, cbo_Filter_Operator, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Machine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Filter_Machine.SelectedIndexChanged

    End Sub

    Private Sub cbo_Filter_Ledger_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbo_Framer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Framer.SelectedIndexChanged

    End Sub

    Private Sub cbo_Filter_Incharge_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Filter_Incharge.SelectedIndexChanged

    End Sub

    Private Sub cbo_Filter_Incharge_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_Incharge.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub pnl_Filter_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Filter.Paint

    End Sub

    Private Sub cbo_Filter_Operator_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Filter_Operator.SelectedIndexChanged

    End Sub

    Private Sub cbo_Filter_Operator_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_Operator.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Framer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Filter_Framer.SelectedIndexChanged

    End Sub

    Private Sub cbo_Filter_Framer_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_Framer.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Payroll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub



    Private Sub cbo_JobNo_Enter(sender As Object, e As EventArgs) Handles cbo_JobNo.Enter

        cbo_JobNo.Tag = cbo_JobNo.Text

        Try

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "OrderJobNo_Head", "OrderJobNo_Name", " OrderNo_Name = '" & Cbo_OrderCode.Text & "'", "")

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_JobNo_Leave(sender As Object, e As EventArgs) Handles cbo_JobNo.Leave

        If cbo_JobNo.Tag <> cbo_JobNo.Text Then

            cbo_colour.Text = ""
            cbo_Size.Text = ""
            txt_Stch_Pcs.Text = ""
            txt_Rate.Text = ""
            txt_NoOfHeads.Text = ""
            txt_NoOfPcs.Text = ""
            txt_NoOfStiches.Text = ""
            lbl_Amount.Text = ""

        End If

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        'Dim gstrate As Double

        If Trim(UCase(Cbo_OrderCode.Text)) <> "" Then

            da = New SqlClient.SqlDataAdapter("select a.*,b.Production_Cost from Order_Program_Head a left outer join Production_Cost b on a.OrderCode_forSelection = b.UID " &
                                              " where a.Ordercode_forSelection = '" & Trim(Cbo_OrderCode.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0)("Design").ToString) = False Then
                    lbl_Grid_Design.Text = dt.Rows(0)("Design").ToString
                End If

                If IsDBNull(dt.Rows(0)("StchsPr_Pcs").ToString) = False Then
                    txt_Stch_Pcs.Text = dt.Rows(0)("StchsPr_Pcs").ToString
                End If
                If IsDBNull(dt.Rows(0)("Production_Cost").ToString) = False Then
                    txt_Rate.Text = dt.Rows(0)("Production_Cost").ToString
                End If
                If IsDBNull(dt.Rows(0)("Order_Program_No").ToString) = False Then
                    lbl_OrderNo.Text = dt.Rows(0)("Order_Program_No").ToString
                End If

            End If
            dt.Dispose()
            da.Dispose()
        End If

        QuantityDetails(True)

    End Sub

    Private Sub cbo_JobNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_JobNo.SelectedIndexChanged

    End Sub

    Private Sub cbo_JobNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_JobNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_JobNo, Cbo_OrderCode, cbo_colour, "OrderJobNo_Head", "OrderJobNo_Name", " OrderNo_Name = '" & Cbo_OrderCode.Text & "'", "")
    End Sub

    Private Sub Cbo_OrderCode_Enter(sender As Object, e As EventArgs) Handles Cbo_OrderCode.Enter

        Try

            Cbo_OrderCode.Tag = Cbo_OrderCode.Text
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Order_Program_Head", "Ordercode_forSelection", " Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, ""), "")

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_JobNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_JobNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_JobNo, cbo_colour, "OrderJobNo_Head", "OrderJobNo_Name", " OrderNo_Name = '" & Cbo_OrderCode.Text & "'", "")
    End Sub

    Private Sub cbo_Ledger_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Ledger.SelectedIndexChanged

    End Sub

    Private Sub Cbo_OrderCode_Leave(sender As Object, e As EventArgs) Handles Cbo_OrderCode.Leave

        If Cbo_OrderCode.Tag <> Cbo_OrderCode.Text Then

            cbo_JobNo.Text = ""
            cbo_colour.Text = ""
            cbo_Size.Text = ""
            txt_Stch_Pcs.Text = ""
            txt_Rate.Text = ""
            txt_NoOfHeads.Text = ""
            txt_NoOfPcs.Text = ""
            txt_NoOfStiches.Text = ""
            lbl_Amount.Text = ""
            txt_Uom.Text = ""

        End If

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        'Dim gstrate As Double

        If Trim(UCase(Cbo_OrderCode.Text)) <> "" Then

            da = New SqlClient.SqlDataAdapter("select a.*,b.Production_Cost from Order_Program_Head a left outer join Production_Cost b on a.OrderCode_forSelection = b.UID " &
                                              " where a.Ordercode_forSelection = '" & Trim(Cbo_OrderCode.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0)("Design").ToString) = False Then
                    lbl_Grid_Design.Text = dt.Rows(0)("Design").ToString
                End If
                'If IsDBNull(dt.Rows(0)("Colour_Idno").ToString) = False Then
                '    cbo_colour.Text = Common_Procedures.Colour_IdNoToName(con, Val(dt.Rows(0).Item("COlour_IdNo").ToString))
                'End If
                'If IsDBNull(dt.Rows(0)("Size_Idno").ToString) = False Then
                '    cbo_Size.Text = Common_Procedures.Size_IdNoToName(con, Val(dt.Rows(0).Item("Size_IdNo").ToString))
                'End If
                If IsDBNull(dt.Rows(0)("StchsPr_Pcs").ToString) = False Then
                    txt_Stch_Pcs.Text = dt.Rows(0)("StchsPr_Pcs").ToString
                End If
                If IsDBNull(dt.Rows(0)("Production_Cost").ToString) = False Then
                    txt_Rate.Text = dt.Rows(0)("Production_Cost").ToString
                End If
                If IsDBNull(dt.Rows(0)("Order_Program_No").ToString) = False Then
                    lbl_OrderNo.Text = dt.Rows(0)("Order_Program_No").ToString
                End If
                If IsDBNull(dt.Rows(0)("Unit_IdNo").ToString) = False Then
                    txt_Uom.Text = Common_Procedures.Unit_IdNoToName(con, dt.Rows(0)("Unit_IdNo"))
                End If
            End If
            dt.Dispose()
            da.Dispose()
        End If

        QuantityDetails(True)

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class
