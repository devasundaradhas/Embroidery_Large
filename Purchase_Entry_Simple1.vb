Public Class Purchase_Entry_Simple1
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PURCS-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private cmbItmNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer

    Private Sub clear()

        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False

        lbl_PurchaseNo.Text = ""
        lbl_PurchaseNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_AvailableStock.Tag = 0
        lbl_AvailableStock.Text = ""

        lbl_TotalTaxAmount.Text = ""

        lbl_AmountInWords.Text = "Amount In Words : "
        txt_Freight.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            txt_Filter_BillNo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
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

        dgv_Details.Rows.Clear()

        cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, 21)
        cbo_PaymentMethod.Text = "CREDIT"
        cbo_TaxType.Text = "NO TAX"
        txt_SlNo.Text = "1"



        'New_Entry = False
        'Insert_Entry = False

        'lbl_PurchaseNo.Text = ""
        'lbl_PurchaseNo.ForeColor = Color.Black

        'pnl_Back.Enabled = True
        'pnl_Filter.Visible = False

        'lbl_AvailableStock.Tag = 0
        'lbl_AvailableStock.Text = ""

        'lbl_PurchaseNo.Text = ""
        'lbl_PurchaseNo.ForeColor = Color.Black

        'dtp_Date.Text = ""
        'cbo_Ledger.Text = ""
        'cbo_PurchaseAc.Text = ""
        'cbo_PaymentMethod.Text = ""
        'cbo_TaxType.Text = ""
        'txt_SlNo.Text = ""
        'cbo_ItemName.Text = ""

        'cbo_Unit.Text = ""
        'txt_NoofItems.Text = ""


        'txt_Rate.Text = ""

        'txt_TaxRate.Text = ""

        'txt_DiscPerc.Text = ""


        'txt_TaxPerc.Text = ""
        'txt_Amount.Text = ""

        'txt_SubAmount.Text = ""
        'txt_DiscountAmount.Text = ""
        'txt_TaxAmount.Text = ""
        'txt_GrossAmount.Text = ""

        'lbl_AmountInWords.Text = "Amount In Words : "
        ''  lbl_NetAmount.Text = "0.00"

        'txt_TotalQty.Text = ""
        'txt_SubTotal.Text = ""
        'txt_TotalDiscAmount.Text = ""
        'txt_TotalTaxAmount.Text = ""
        'txt_CashDiscAmount.Text = ""
        'txt_CashDiscPerc.Text = ""
        'txt_AddLessAmount.Text = ""
        'txt_RoundOff.Text = ""
        'txt_NetAmount.Text = ""
        'txt_Narration.Text = ""
        'txt_BillNo.Text = ""

        'dgv_Details.Rows.Clear()


        'If Filter_Status = False Then
        '    dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
        '    dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
        '    cbo_Filter_PartyName.Text = ""
        '    cbo_Filter_ItemName.Text = ""

        '    cbo_Filter_PartyName.SelectedIndex = -1
        '    cbo_Filter_ItemName.SelectedIndex = -1
        '    dgv_Filter_Details.Rows.Clear()
        'End If

        'Grid_Cell_DeSelect()



        'dgv_Details.Rows.Clear()

        'cbo_PaymentMethod.Text = "CREDIT"
        'cbo_TaxType.Text = "NO TAX"
        'txt_SlNo.Text = "1"

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

        'If Me.ActiveControl.Name <> cbo_ItemName.Name Then
        '    cbo_ItemName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_Unit.Name Then
        '    cbo_Unit.Visible = False
        'End If




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
        dgv_Details.CurrentCell.Selected = False

        dgv_Filter_Details.CurrentCell.Selected = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Ledger_Name as PurchaseAcName from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.PurchaseAc_IdNo = c.Ledger_IdNo where a.Purchase_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_PurchaseNo.Text = dt1.Rows(0).Item("Purchase_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Purchase_Date").ToString
                cbo_PaymentMethod.Text = dt1.Rows(0).Item("Payment_Method").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                cbo_PurchaseAc.Text = dt1.Rows(0).Item("PurchaseAcName").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_TotalQty.Text = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                txt_SubTotal.Text = Format(Val(dt1.Rows(0).Item("SubTotal_Amount").ToString), "########0.00")
                txt_TotalDiscAmount.Text = Format(Val(dt1.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00")
                lbl_TotalTaxAmount.Text = Format(Val(dt1.Rows(0).Item("Total_TaxAmount").ToString), "########0.00")
                txt_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                txt_AddLessAmount.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                txt_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_Narration.Text = Trim(dt1.Rows(0).Item("Narration").ToString)

                da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Discount_Amount_Item ,a.Tax_Perc, a.Tax_Amount, a.Total_Amount,a.Free_Qty from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Unit_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Tax_Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Tax_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Free_Qty").ToString), "########0")
                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            Grid_Cell_DeSelect()

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Purchase_Entry_Simple_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim dt1 As New DataTable


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Purchase_Entry_Simple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Me.Text = ""

        con.Open()

        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Else
            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        End If

        txt_SubAmount.Enabled = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1092" Or Trim(Common_Procedures.settings.CustomerCode) = "1095" Then
            txt_SubAmount.Enabled = True
        End If

        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"

        da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        da.Fill(dt3)
        cbo_Unit.DataSource = dt3
        cbo_Unit.DisplayMember = "unit_name"

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 27) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_PurchaseAc.DataSource = dt4
        cbo_PurchaseAc.DisplayMember = "Ledger_DisplayName"

        lbl_Freight.Visible = False
        txt_Freight.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1071" Then
            lbl_Freight.Visible = True
            txt_Freight.Visible = True
        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("NO TAX")

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofItems.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscAmountItemwise.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FreeQty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalQty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SubTotal.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler dgv_Details.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLessAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RoundOff.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NetAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SubAmount.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Delete.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofItems.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_SubAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscountAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscAmountItemwise.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FreeQty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalQty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SubTotal.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler dgv_Details.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLessAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RoundOff.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NetAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_BillNo.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Delete.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoofItems.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_TaxRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_DiscAmountItemwise.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_TotalQty.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SubTotal.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLessAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RoundOff.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NetAmount.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_SlNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoofItems.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_TaxRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscAmountItemwise.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalQty.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SubTotal.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLessAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RoundOff.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NetAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress

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

    Private Sub Purchase_Entry_Simple_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Purchase_Entry_Simple_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)


            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Reference_Code, Reference_Date, Company_Idno, Item_IdNo ) " & _
                                  " Select                               Reference_Code, Reference_Date, Company_IdNo, Item_IdNo from Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Purchase_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then
                If Common_Procedures.Check_Negative_Stock_Status(con, tr) = True Then Exit Sub
            End If

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

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

            da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
            da.Fill(dt2)
            cbo_Filter_ItemName.DataSource = dt2
            cbo_Filter_ItemName.DisplayMember = "item_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            txt_Filter_BillNo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

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
            cmd.CommandText = "select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Gst_Tax_Type <> 'GST' Order by for_Orderby, Purchase_No"
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

            If Val(movno) <> 0 Then move_record(movno)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_PurchaseNo.Text))

            da = New SqlClient.SqlDataAdapter("select Purchase_No from Purchase_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Gst_Tax_Type <> 'GST'  Order by for_Orderby, Purchase_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select Purchase_No from Purchase_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Gst_Tax_Type <> 'GST' Order by for_Orderby desc, Purchase_No desc"

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
        Dim da As New SqlClient.SqlDataAdapter("select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Gst_Tax_Type <> 'GST' Order by for_Orderby desc, Purchase_No desc", con)
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

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Gst_Tax_Type <> 'GST' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_PurchaseNo.Text = NewID
            lbl_PurchaseNo.ForeColor = Color.Red


            da = New SqlClient.SqlDataAdapter("select a.Payment_Method, b.ledger_name as PurchaseAcName, a.Tax_Type from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Purchase_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                cbo_PaymentMethod.Text = dt2.Rows(0).Item("Payment_Method").ToString
                cbo_PurchaseAc.Text = dt2.Rows(0).Item("PurchaseAcName").ToString
                cbo_TaxType.Text = dt2.Rows(0).Item("Tax_Type").ToString
            End If

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

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

            inpno = InputBox("Enter Purchase No.", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "' and Entry_Gst_Tax_Type <> 'GST'"
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
                MessageBox.Show("Purchase No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter New Purchase No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Purchase No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_PurchaseNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim L_id As Integer = 0
        Dim purcac_id As Integer = 0
        Dim TxAc_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim Amt As Single = 0
        Dim TxAmt_Diff As Single = 0, TotTxAmt As Single = 0
        Dim VouBil As String = ""

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 And Trim(UCase(cbo_PaymentMethod.Text)) = "CREDIT" Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        purcac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)
        If purcac_id = 0 And Val(txt_NetAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BillNo.Enabled Then txt_BillNo.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            da = New SqlClient.SqlDataAdapter("select * from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Ledger_IdNo = " & Str(Val(led_id)) & " and Bill_No = '" & Trim(txt_BillNo.Text) & "' and Purchase_Code <> '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Bill No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
                Exit Sub
            End If
            dt1.Clear()
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt4)

                NewNo = 0
                If dt4.Rows.Count > 0 Then
                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        NewNo = Int(Val(dt4.Rows(0)(0).ToString))
                        NewNo = Val(NewNo) + 1
                    End If
                End If
                dt4.Clear()
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_PurchaseNo.Text)

                lbl_PurchaseNo.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurchaseDate", dtp_Date.Value.Date)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into Purchase_Head(Purchase_Code, Company_IdNo, Purchase_No, for_OrderBy, Purchase_Date, Payment_Method, Ledger_IdNo, PurchaseAc_IdNo, Tax_Type, Narration, Total_Qty, SubTotal_Amount, Total_DiscountAmount, Total_TaxAmount, Gross_Amount, CashDiscount_Perc, CashDiscount_Amount, AddLess_Amount, Round_Off, Net_Amount,Bill_No , Freight_Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(purcac_id)) & ", '" & Trim(cbo_TaxType.Text) & "', '" & Trim(txt_Narration.Text) & "', " & Str(Val(txt_TotalQty.Text)) & ", " & Str(Val(txt_SubTotal.Text)) & ", " & Str(Val(txt_TotalDiscAmount.Text)) & ", " & Str(Val(lbl_TotalTaxAmount.Text)) & ", " & Str(Val(txt_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_AddLessAmount.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(txt_NetAmount.Text)) & " , '" & Trim(txt_BillNo.Text) & "' , " & Str(Val(txt_Freight.Text)) & ")"

                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Purchase_Head set Purchase_Date = @PurchaseDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", PurchaseAc_IdNo = " & Str(Val(purcac_id)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Narration = '" & Trim(txt_Narration.Text) & "',Freight_Amount = " & Str(Val(txt_Freight.Text)) & "  ,  Total_Qty = " & Str(Val(txt_TotalQty.Text)) & ", SubTotal_Amount = " & Str(Val(txt_SubTotal.Text)) & ", Total_DiscountAmount = " & Str(Val(txt_TotalDiscAmount.Text)) & ", Total_TaxAmount = " & Str(Val(lbl_TotalTaxAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLessAmount.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(txt_NetAmount.Text)) & " ,Bill_No = '" & Trim(txt_BillNo.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Reference_Code, Reference_Date, Company_Idno, Item_IdNo ) " & _
                                      " Select                               Reference_Code, Reference_Date, Company_IdNo, Item_IdNo from Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If

            TxAmt_Diff = 0

            TotTxAmt = 0
            For i = 0 To dgv_Details.RowCount - 1

                If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then
                    TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(10).Value)
                End If

            Next

            TxAmt_Diff = Format(Val(lbl_TotalTaxAmount.Text) - Val(TotTxAmt), "#########0.00")

            cmd.CommandText = "Delete from Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            For i = 0 To dgv_Details.RowCount - 1

                itm_id = 0
                unt_id = 0

                da = New SqlClient.SqlDataAdapter("select item_idno from item_head where item_name = '" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "'", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt3)

                If dt3.Rows.Count > 0 Then
                    If IsDBNull(dt3.Rows(0)(0).ToString) = False Then
                        itm_id = Val(dt3.Rows(0)(0).ToString)
                    End If
                End If

                dt3.Clear()

                'If itm_id <> 0 And Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 And Val(dgv_Details.Rows(i).Cells(12).Value) <> 0 Then
                If itm_id <> 0 Then
                    da = New SqlClient.SqlDataAdapter("select unit_idno from unit_head where unit_name = '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "'", con)
                    da.SelectCommand.Transaction = tr
                    da.Fill(dt5)

                    If dt5.Rows.Count > 0 Then
                        If IsDBNull(dt5.Rows(0)(0).ToString) = False Then
                            unt_id = Val(dt5.Rows(0)(0).ToString)
                        End If
                    End If

                    dt5.Clear()

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Purchase_Details(Purchase_Code, Company_IdNo, Purchase_No, for_OrderBy, Purchase_Date, Ledger_IdNo, SL_No, Item_IdNo, Unit_IdNo, Noof_Items, Rate, Tax_Rate, Amount, Discount_Amount_Item,Discount_Perc, Discount_Amount ,Tax_Perc, Tax_Amount, Total_Amount, TaxAmount_Difference,Free_Qty) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(3).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(10).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(11).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(12).Value)) & ", " & Str(Val(TxAmt_Diff)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(13).Value)) & ")"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Party_Bill_No, SL_No, Item_IdNo, Unit_IdNo, Quantity) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, " & Str(Val(led_id)) & ", '" & Trim(txt_BillNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(3).Value) + Val(dgv_Details.Rows(i).Cells(13).Value)) & " )"
                    cmd.ExecuteNonQuery()

                    TxAmt_Diff = 0

                End If

            Next

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            Ac_id = 0
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                Ac_id = 1
            Else
                Ac_id = led_id
            End If

            cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " & _
                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'Purch', @PurchaseDate, " & Str(Val(purcac_id)) & ", " & Str(Val(Ac_id)) & ", " & Str(Val(txt_NetAmount.Text)) & ", 'Bill No . : " & Trim(txt_BillNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'Purch', @PurchaseDate, 1, " & Str(Val(Ac_id)) & ", " & Str(Val(txt_NetAmount.Text)) & ", 'Bill No . : " & Trim(txt_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            cmd.ExecuteNonQuery()

            Amt = Val(txt_NetAmount.Text) + Val(txt_TotalDiscAmount.Text) - Val(lbl_TotalTaxAmount.Text) + Val(txt_CashDiscAmount.Text) - Val(txt_AddLessAmount.Text) - Val(txt_RoundOff.Text)

            cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'Purch', @PurchaseDate, 2, " & Str(Val(purcac_id)) & ", " & Str(-1 * Val(Amt)) & ", 'Bill No . : " & Trim(txt_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            cmd.ExecuteNonQuery()

            If Val(lbl_TotalTaxAmount.Text) <> 0 Then

                TxAc_id = 20

                cmd.CommandText = "Insert into Voucher_Details (                   Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                                            " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'Purch', @PurchaseDate, 3, " & Str(Val(TxAc_id)) & ", " & Str(-1 * Val(lbl_TotalTaxAmount.Text)) & ", 'Bill No . : " & Trim(txt_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_TotalDiscAmount.Text) <> 0 Or Val(txt_CashDiscAmount.Text) <> 0 Or Val(txt_AddLessAmount.Text) <> 0 Then

                L_id = 17
                Amt = -1 * (Val(txt_TotalDiscAmount.Text) + Val(txt_CashDiscAmount.Text))

                cmd.CommandText = "Insert into Voucher_Details (                   Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                                            " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'Purch', @PurchaseDate, 4, " & Str(Val(L_id)) & ", " & Str(-1 * Val(Amt)) & ", 'Bill No . : " & Trim(txt_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_AddLessAmount.Text) <> 0 Then

                L_id = 17
                Amt = Val(txt_AddLessAmount.Text)

                cmd.CommandText = "Insert into Voucher_Details (                   Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                                            " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'Purch', @PurchaseDate, 5, " & Str(Val(L_id)) & ", " & Str(-1 * Val(Amt)) & ", 'Bill No . : " & Trim(txt_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_RoundOff.Text) <> 0 Then

                L_id = 24

                cmd.CommandText = "Insert into Voucher_Details (                   Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                                            " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'Purch', @PurchaseDate, 6, " & Str(Val(L_id)) & ", " & Str(-1 * Val(txt_RoundOff.Text)) & ", 'Bill No . : " & Trim(txt_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()

            End If

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

                If Common_Procedures.Check_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If


            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, led_id, Trim(txt_BillNo.Text), 0, Val(CSng(txt_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            If New_Entry = True Then
                move_record(lbl_PurchaseNo.Text)
                'new_record()
            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName
            cmbItmNm = cbo_ItemName.Text
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "item_head", "item_Name", "", "(item_idno = 0)")
        End With
        Show_Item_CurrentStock()
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim itm_id As Integer = 0

        If Trim(cbo_ItemName.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        If Trim(cbo_Unit.Text) = "" Then
            MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If
        If Val(txt_NoofItems.Text) = 0 And Val(txt_FreeQty.Text) = 0 Then
            MessageBox.Show("Invalid No.of Items", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_NoofItems.Enabled Then txt_NoofItems.Focus()
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 And Val(txt_TaxRate.Text) = 0 Then
            If Val(txt_NoofItems.Text) <> 0 Then
                MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
                If txt_Rate.Enabled Then txt_Rate.Focus()
                Exit Sub
            End If
        End If

        'If Val(txt_Amount.Text) = 0 Then
        '    If Val(txt_FreeQty.Text) = 0 Then
        '        MessageBox.Show("Invalid Amount", "DOES NOT ADD...", MessageBoxButtons.OK)
        '        If txt_Amount.Enabled Then txt_Amount.Focus()
        '        Exit Sub
        '    End If
        'End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(2).Value = cbo_Unit.Text
                    .Rows(i).Cells(3).Value = Val(txt_NoofItems.Text)
                    .Rows(i).Cells(4).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(5).Value = Format(Val(txt_TaxRate.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(txt_SubAmount.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(txt_DiscAmountItemwise.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(txt_DiscountAmount.Text), "########0.00")
                    .Rows(i).Cells(10).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                    .Rows(i).Cells(11).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                    .Rows(i).Cells(12).Value = Format(Val(txt_Amount.Text), "########0.00")
                    .Rows(i).Cells(13).Value = Val(txt_FreeQty.Text)

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 10 Then .FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = cbo_Unit.Text
                .Rows(n).Cells(3).Value = Val(txt_NoofItems.Text)
                .Rows(n).Cells(4).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(5).Value = Format(Val(txt_TaxRate.Text), "########0.00")
                .Rows(n).Cells(6).Value = Format(Val(txt_SubAmount.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(txt_DiscAmountItemwise.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(txt_DiscountAmount.Text), "########0.00")
                .Rows(n).Cells(10).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                .Rows(n).Cells(11).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                .Rows(n).Cells(12).Value = Format(Val(txt_Amount.Text), "########0.00")
                .Rows(n).Cells(13).Value = Val(txt_FreeQty.Text)
                .Rows(n).Selected = True

                If n >= 10 Then .FirstDisplayedScrollingRowIndex = n - 9

            End If

        End With

        GrossAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        txt_SubAmount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscountAmount.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_Amount.Text = ""
        txt_DiscAmountItemwise.Text = ""
        txt_FreeQty.Text = ""
        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_NoofItems_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoofItems.GotFocus
        Show_Item_CurrentStock()
    End Sub

    Private Sub txt_NoofItems_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofItems.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoofItems.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_Rate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.GotFocus
        Show_Item_CurrentStock()
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 38 Then
            txt_NoofItems.Focus()
        End If
        If e.KeyCode = 40 Then
            If txt_SubAmount.Enabled = True Then
                txt_SubAmount.Focus()
            Else
                txt_DiscAmountItemwise.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If txt_SubAmount.Enabled = True Then
                txt_SubAmount.Focus()
            Else
                txt_DiscAmountItemwise.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
        Amount_Calculation()
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPerc.KeyUp
        ' txt_DiscAmountItemwise.Text = Format(Val(txt_Amount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If txt_FreeQty.Visible And txt_FreeQty.Enabled Then
                txt_FreeQty.Focus()
            Else
                btn_Add_Click(sender, e)
            End If
        End If

    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
        Amount_Calculation()
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

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a, unit_head b where a.item_name = '" & Trim(cbo_ItemName.Text) & "' and a.unit_idno = b.unit_idno", con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                txt_Rate.Text = dt.Rows(0)("Cost_Rate").ToString
                txt_TaxRate.Text = dt.Rows(0)("Sale_TaxRate").ToString
                txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
            End If
            dt.Dispose()
            da.Dispose()
        End If



    End Sub

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.GotFocus
        Show_Item_CurrentStock()
    End Sub



    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        End If

    End Sub


    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus

        Show_Item_CurrentStock()
    End Sub





    Private Sub txt_AddLessAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLessAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLessAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_GrossAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SlNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SlNo.GotFocus
        Show_Item_CurrentStock()
    End Sub



    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        cbo_ItemName.Text = Trim(.Rows(i).Cells(1).Value)
                        cbo_Unit.Text = Trim(.Rows(i).Cells(2).Value)
                        txt_NoofItems.Text = Val(.Rows(i).Cells(3).Value)
                        txt_Rate.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.00")
                        txt_TaxRate.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.00")
                        txt_SubAmount.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.00")
                        txt_DiscAmountItemwise.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.00")
                        txt_DiscPerc.Text = Format(Val(.Rows(i).Cells(8).Value), "########0.00")
                        txt_DiscountAmount.Text = Format(Val(.Rows(i).Cells(9).Value), "########0.00")
                        txt_TaxPerc.Text = Format(Val(.Rows(i).Cells(10).Value), "########0.00")
                        txt_TaxAmount.Text = Format(Val(.Rows(i).Cells(11).Value), "########0.00")
                        txt_Amount.Text = Format(Val(.Rows(i).Cells(12).Value), "########0.00")
                        txt_FreeQty.Text = Format(Val(.Rows(i).Cells(13).Value), "########0")

                        Exit For

                    End If

                Next

            End With

            SendKeys.Send("{TAB}")

        End If
    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus

        Show_Item_CurrentStock()
    End Sub





    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyUp
        txt_Rate.Text = Format(Val(txt_TaxRate.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
        Amount_Calculation()
    End Sub

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        'If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
    End Sub


    Private Sub Amount_Calculation()
        Dim totDisc As Decimal

        txt_SubAmount.Text = Val(txt_NoofItems.Text) * Val(txt_Rate.Text)

        '-- txt_DiscountAmount.Text = Format(Val(txt_NoofItems.Text) * (Val(txt_Rate.Text) * Val(txt_DiscPerc.Text) / 100), "#########0.00")  '---30-NOV-2016
        txt_DiscountAmount.Text = Format(Val(txt_NoofItems.Text) * ((Val(txt_Rate.Text) - Val(txt_DiscAmountItemwise.Text)) * Val(txt_DiscPerc.Text) / 100), "#########0.00")


        txt_TaxAmount.Text = "0.00"
        If Trim(cbo_TaxType.Text) <> "" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
            totDisc = (Val(txt_DiscAmountItemwise.Text) * Val(txt_NoofItems.Text))
            txt_TaxAmount.Text = Format(((Val(txt_SubAmount.Text) - totDisc - Val(txt_DiscountAmount.Text)) * Val(txt_TaxPerc.Text) / 100), "#########0.00")
        End If

        txt_Amount.Text = Format(Val(txt_SubAmount.Text) - (Val(txt_DiscAmountItemwise.Text) * Val(txt_NoofItems.Text)) - Val(txt_DiscountAmount.Text) + Val(txt_TaxAmount.Text), "########0.00")


    End Sub

    Private Sub GrossAmount_Calculation()
        Dim Sno As Integer
        Dim TotQty As Decimal, TotSubAmt As Decimal, TotDiscAmt As Decimal, TotTxAmt As Decimal, TotAmt As Decimal
        Dim ToFree_qty As Integer = 0
        Sno = 0
        TotQty = 0
        TotSubAmt = 0
        TotDiscAmt = 0
        TotTxAmt = 0
        TotAmt = 0
        For i = 0 To dgv_Details.RowCount - 1
            Sno = Sno + 1
            dgv_Details.Rows(i).Cells(0).Value = Sno

            TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(3).Value)
            TotSubAmt = TotSubAmt + Val(dgv_Details.Rows(i).Cells(6).Value)
            TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(9).Value) + (Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(3).Value))
            TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(11).Value)
            TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(12).Value) '- Val(dgv_Details.Rows(i).Cells(9).Value)
            ToFree_qty = ToFree_qty + Val(dgv_Details.Rows(i).Cells(13).Value)

        Next

        txt_TotalQty.Text = Val(TotQty)
        txt_SubTotal.Text = Format(TotSubAmt, "########0.00")
        txt_TotalDiscAmount.Text = Format(TotDiscAmt, "########0.00")
        lbl_TotalTaxAmount.Text = Format(TotTxAmt, "########0.00")
        txt_GrossAmount.Text = Format(TotAmt, "########0.00")

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        txt_CashDiscAmount.Text = Format(Val(txt_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

        NtAmt = Val(txt_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLessAmount.Text)

        txt_RoundOff.Text = Format(Format(Val(NtAmt), "#########0") - Val(NtAmt), "#########0.00")

        txt_NetAmount.Text = Format(Val(txt_Freight.Text) + Val(txt_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLessAmount.Text) + Val(txt_RoundOff.Text), "########0.00")

        lbl_AmountInWords.Text = "Amount In Words : "
        If Val(txt_NetAmount.Text) <> 0 Then
            lbl_AmountInWords.Text = "Amount In Words : " & Common_Procedures.Rupees_Converstion(Val(txt_NetAmount.Text))
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Purchase_Code = '" & Trim(NewCode) & "'", con)
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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If

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
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo , C.* from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo where a.Purchase_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "" Then
        Printing_Format1(e)
        'End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50
            .Top = 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 15

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(55)
        ClArr(2) = 290 : ClArr(3) = 100 : ClArr(4) = 70 : ClArr(5) = 95
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        CurY = TMargin

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        'Cmp_Name = "TSOFT SOLUTIONS"
        'Cmp_Add1 = "4, IIIrd floor, R.A Tower"
        'Cmp_Add2 = "P.N Road, Tirupur - 2."
        'Cmp_PhNo = "PHONE : 96293 37417"
        'Cmp_TinNo = "TIN NO. : 33554488556"
        'Cmp_CstNo = "CST NO. : 998875 Dt. 01-04-2015"

        'If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
        '    Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
        '    Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        'End If


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Total_TaxAmount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            DetSNo = DetSNo + 1


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

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 25, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

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

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        'Cmp_Name = "TSOFT SOLUTIONS"
        'Cmp_Add1 = "4, IIIrd floor, R.A Tower"
        'Cmp_Add2 = "P.N Road, Tirupur - 2."
        'Cmp_PhNo = "PHONE : 96293 37417"
        'Cmp_TinNo = "TIN NO. : 33554488556"
        'Cmp_CstNo = "CST NO. : 998875 Dt. 01-04-2015"

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)
        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) '& IIf(Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        End If
        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) '& IIf(Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY - 10, 2, ClAr(4) + ClAr(5) + ClAr(6), p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 10, PageWidth, CurY + 10)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO.    :  " & prn_HdDt.Rows(0).Item("Purchase_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE    :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Purchase_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "     Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

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
        Dim I As Integer

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Sub Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_TaxAmount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, StrConv(Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString), vbProperCase), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_TaxAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))

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

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Rupees : " & Rup1, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & (prn_HdDt.Rows(0).Item("Company_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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
                Condt = "a.Purchase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Purchase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Purchase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                da = New SqlClient.SqlDataAdapter("select item_idno from item_head where item_name = '" & Trim(cbo_Filter_ItemName.Text) & "'", con)
                da.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                        Itm_IdNo = Val(dt2.Rows(0)(0).ToString)
                    End If
                End If

                dt2.Clear()
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Purchase_Code IN (select z.Purchase_Code from Purchase_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            If Trim(txt_Filter_BillNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bill_No = '" & Trim(txt_Filter_BillNo.Text) & "' "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Purchase_No, a.Purchase_Date, a.Bill_No, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Purchase_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Purchase_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Purchase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        If dgv_Filter_Details.Rows.Count > 0 Then
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

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



    Private Sub cbo_PaymentMethod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.LostFocus

        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If

    End Sub

    Private Sub cbo_PurchaseAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub



    Private Sub cbo_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
            cbo_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            txt_NoofItems.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(4).Value), "########0.00")
            txt_TaxRate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
            txt_SubAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
            txt_DiscAmountItemwise.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
            txt_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
            txt_DiscountAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
            txt_TaxPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
            txt_TaxAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(11).Value), "########0.00")
            txt_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
            txt_FreeQty.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0")

            If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

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

        GrossAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        txt_SubAmount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscountAmount.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_Amount.Text = ""
        txt_DiscAmountItemwise.Text = ""

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .Rows.Count - 1
                        .Rows(n).Cells(0).Value = i + 1
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

            GrossAmount_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_ItemName.Text = ""
            cbo_Unit.Text = ""
            txt_NoofItems.Text = ""
            txt_Rate.Text = ""
            txt_TaxRate.Text = ""
            txt_SubAmount.Text = ""
            txt_DiscPerc.Text = ""
            txt_DiscountAmount.Text = ""
            txt_DiscAmountItemwise.Text = ""
            txt_TaxPerc.Text = ""
            txt_TaxAmount.Text = ""
            txt_Amount.Text = ""

            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

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

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        Dim SubAmt As Single
        Dim DiscAmt As Single
        Dim TxPerc As Single
        Dim TxAmt As Single
        Dim ItemDiscAmt As Single
        Dim TotAmt As Single

        For i = 0 To dgv_Details.RowCount - 1
            SubAmt = Val(dgv_Details.Rows(i).Cells(6).Value)
            DiscAmt = Val(dgv_Details.Rows(i).Cells(8).Value)
            ItemDiscAmt = Val(dgv_Details.Rows(i).Cells(9).Value)
            TxPerc = Val(dgv_Details.Rows(i).Cells(10).Value)

            TxAmt = 0
            If Trim(cbo_TaxType.Text) <> "" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
                TxAmt = Format(((Val(SubAmt) - Val(ItemDiscAmt) - Val(DiscAmt)) * Val(TxPerc) / 100), "#########0.00")
            End If

            TotAmt = Val(SubAmt) - Val(DiscAmt) + Val(TxAmt)

            dgv_Details.Rows(i).Cells(11).Value = Trim(Format(Val(TxAmt), "#########0.00"))
            dgv_Details.Rows(i).Cells(12).Value = Trim(Format(Val(TotAmt), "#########0.00"))

        Next

        GrossAmount_Calculation()

    End Sub

    Private Sub Show_Item_CurrentStock()
        Dim vItemID As Integer
        Dim CurStk As Decimal

        If Trim(cbo_ItemName.Text) <> "" Then
            vItemID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
            If Val(lbl_AvailableStock.Tag) <> Val(vItemID) Then
                lbl_AvailableStock.Tag = 0
                lbl_AvailableStock.Text = ""
                If Val(vItemID) <> 0 Then
                    CurStk = Common_Procedures.get_Item_CurrentStock(con, Val(lbl_Company.Tag), vItemID)
                    lbl_AvailableStock.Tag = vItemID
                    lbl_AvailableStock.Text = CurStk
                End If
            End If

        Else
            lbl_AvailableStock.Tag = 0
            lbl_AvailableStock.Text = ""

        End If
    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_PaymentMethod, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_PaymentMethod, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        End If

    End Sub



    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PurchaseAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, cbo_Ledger, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_PurchaseAc, txt_SlNo, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_SlNo, "", "", "", "")
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_SlNo, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_ItemName.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_BillNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        If Asc(e.KeyChar) = 13 Then
            Show_Item_CurrentStock()

            If Trim(cbo_ItemName.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_BillNo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, cbo_ItemName, txt_NoofItems, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_NoofItems, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
    End Sub

    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_paymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, txt_Filter_BillNo, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, txt_Filter_BillNo, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_ItemName.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub lbl_TotalTaxAmount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_TotalTaxAmount.Click
        Dim VtAmt As String = ""

        VtAmt = InputBox("Enter vat Amount :", "FOR VAT AMOUNT ALTERATION....", Val(lbl_TotalTaxAmount.Text))

        If Trim(VtAmt) <> "" Then
            If Val(VtAmt) <> 0 Then
                lbl_TotalTaxAmount.Text = Format(Val(VtAmt), "#########0.00")

                txt_GrossAmount.Text = Format(Val(txt_SubTotal.Text) - Val(txt_TotalDiscAmount.Text) + Val(lbl_TotalTaxAmount.Text), "########0.00")

                NetAmount_Calculation()
            End If
        End If

        If txt_CashDiscPerc.Visible And txt_CashDiscPerc.Enabled Then txt_CashDiscPerc.Focus()

    End Sub

    Private Sub txt_Filter_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Filter_BillNo.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_Filter_ItemName.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : btn_Filter_Show.Focus()  ' SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Filter_BillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_BillNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_DiscAmountItemwise_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscAmountItemwise.KeyDown
        If e.KeyCode = 38 Then
            If txt_SubAmount.Enabled = True Then
                txt_SubAmount.Focus()
            Else
                txt_Rate.Focus()
            End If
        End If
        If e.KeyCode = 40 Then
            If txt_CashDiscPerc.Enabled Then txt_CashDiscPerc.Focus()
        End If
    End Sub

    Private Sub txt_FreeQty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FreeQty.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            If txt_TaxPerc.Visible And txt_TaxPerc.Enabled Then txt_TaxPerc.Focus()
        End If
        If e.KeyCode = 40 Then
            If btn_Add.Enabled Then btn_Add.Focus()
        End If
    End Sub

    Private Sub txt_FreeQty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FreeQty.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub txt_SubAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SubAmount.KeyDown
        If e.KeyCode = 38 Then
            txt_Rate.Focus()
        End If
        If e.KeyCode = 40 Then
            If txt_DiscAmountItemwise.Enabled = True Then txt_DiscAmountItemwise.Focus()
        End If
    End Sub

    Private Sub txt_SubAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SubAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_DiscAmountItemwise.Focus()
        End If

    End Sub

    Private Sub txt_SubAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SubAmount.LostFocus
        '   If Val(txt_Rate.Text) = 0 Then
        If Val(txt_NoofItems.Text) <> 0 Then
            txt_Rate.Text = Val(txt_SubAmount.Text) / Val(txt_NoofItems.Text)
        End If
        ' End If
    End Sub

End Class