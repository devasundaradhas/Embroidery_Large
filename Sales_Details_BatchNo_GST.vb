Public Class Sales_Details_BatchNo_GST


    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    'Private Pk_Condition As String = "SALBT-"
    'Private Pk_Condition1 As String = "SALCB-"

    Private Pk_Condition As String = "GSALE-"
    Private NoCalc_Status As Boolean = False
    Private Batch_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private dgv_ActiveCtrl_Name As String
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_TaxDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BatchDetails As New DataGridViewTextBoxEditingControl
    Private cmbItmNm As String
    Private txtItmCd As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer

    Private prn_DetMxIndx As Integer
    Private prn_DetAr(200, 10) As String

    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer


    Private Sub clear()


        NoCalc_Status = True
        Batch_Status = True
        New_Entry = False
        Insert_Entry = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Tax.Visible = False
        pnl_BatchSelection_ToolTip.Visible = False
        pnl_Selection.Visible = False

        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        cbo_PaymentMethod.Text = ""
        cbo_TaxType.Text = ""
        txt_SlNo.Text = ""
        txt_Code.Text = ""
        cbo_ItemName.Text = ""

        lbl_Unit.Text = ""
        txt_NoofItems.Text = ""


        txt_Rate.Text = ""
        lbl_Sales_Price.Text = ""
        lbl_Manufacture_Day.Text = ""
        lbl_Manufacture_Year.Text = ""
        txt_Mfg_Date.Text = ""
        lbl_Mrp_Rate.Text = ""
        txt_Exp_date.Text = ""
        lbl_Expiray_Day.Text = ""
        lbl_Expiry_Year.Text = ""
        lbl_ExpiryMonth.Text = ""
        lbl_Expiray_Period_Days.Text = ""
        txt_DiscPerc.Text = ""
        lbl_Manufacture_Month.Text = ""

        txt_TaxPerc.Text = ""
        txt_Amount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscAmount.Text = ""
        txt_DiscountAmountItem.Text = ""
        txt_TaxAmount.Text = ""
        txt_GrossAmount.Text = ""
        txt_DisPerc_Item.Text = ""
        lbl_AmountInWords.Text = "Amount In Words : "
        '  lbl_NetAmount.Text = "0.00"

        txt_TotalQty.Text = ""
        lbl_Batch_No.Text = ""
        txt_TotalDiscAmount.Text = ""

        txt_CashDiscAmount.Text = ""
        txt_CashDiscPerc.Text = ""
        txt_AddLessAmount.Text = ""
        txt_RoundOff.Text = ""
        txt_NetAmount.Text = ""

        txt_ReceivedAmount.Text = ""
        lbl_BalanceAmount.Text = ""
        lbl_TotalTaxAmount.Text = ""

        lbl_AmountInWords.Text = "Amount In Words : "
        txt_Freight.Text = ""
        txt_Details_Slno.Text = ""

        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""
        lbl_Grid_AssessableValue.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_Purchase.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            cbo_Filter_Purchase.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If



        dgv_Details.Rows.Clear()


        cbo_PaymentMethod.Text = "CREDIT"
        cbo_TaxType.Text = "NO TAX"

        txt_SlNo.Text = "1"
        txt_Details_Slno.Text = "1"

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Tax_Details.Rows.Clear()
        dgv_Tax_Total_Details.Rows.Clear()
        dgv_Tax_Total_Details.Rows.Add()

        NoCalc_Status = False
        Batch_Status = False





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
        'If Me.ActiveControl.Name <> lbl_Unit.Name Then
        '    lbl_Unit.Visible = False
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
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
        dgv_Tax_Details.CurrentCell.Selected = False
        dgv_Tax_Total_Details.CurrentCell.Selected = False

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer


        NoCalc_Status = True
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Date").ToString
                cbo_PaymentMethod.Text = dt1.Rows(0).Item("Payment_Method").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString

                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_TotalQty.Text = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                txt_Aessableamount.Text = Format(Val(dt1.Rows(0).Item("Aessable_Amount").ToString), "########0.00")
                txt_TotalDiscAmount.Text = Format(Val(dt1.Rows(0).Item("Total_DiscountAmount_item").ToString), "########0.00")
                lbl_TotalTaxAmount.Text = Format(Val(dt1.Rows(0).Item("Total_TaxAmount").ToString), "########0.00")
                txt_TotalGrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                txt_AddLessAmount.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                txt_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                ' txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess_Name.Text = Trim(dt1.Rows(0).Item("AddLess_Name").ToString)
                txt_Freight_Name.Text = Trim(dt1.Rows(0).Item("Freight_Name").ToString)

                lbl_Assessable.Text = Trim(dt1.Rows(0).Item("Assessable_Value").ToString)
                lbl_CGstAmount.Text = Trim(dt1.Rows(0).Item("CGst_Amount").ToString)
                lbl_SGstAmount.Text = Trim(dt1.Rows(0).Item("SGst_Amount").ToString)
                lbl_IGstAmount.Text = Trim(dt1.Rows(0).Item("IGst_Amount").ToString)

                da2 = New SqlClient.SqlDataAdapter("select a.*,b.Item_Name,c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Code").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Item_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Unit_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate_Tax").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Tax_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("MRP_Rate").ToString), "########0")
                        dgv_Details.Rows(n).Cells(16).Value = Format(Val(dt2.Rows(i).Item("Sales_Price").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(17).Value = (dt2.Rows(i).Item("Batch_Serial_No").ToString)

                        If IsDate(dt2.Rows(i).Item("Manufacture_Date").ToString) = True Then
                            If DateDiff(DateInterval.Day, Convert.ToDateTime("01/01/1900"), dt2.Rows(i).Item("Manufacture_Date")) > 0 Then
                                dgv_Details.Rows(n).Cells(18).Value = Val(dt2.Rows(i).Item("Manufacture_Day").ToString)
                                dgv_Details.Rows(n).Cells(19).Value = Common_Procedures.Month_IdNoToShortName(con, Val(dt2.Rows(i).Item("Manufacture_Month_IdNo").ToString))
                                dgv_Details.Rows(n).Cells(20).Value = Val(dt2.Rows(i).Item("Manufacture_Year").ToString)
                                dgv_Details.Rows(n).Cells(21).Value = (dt2.Rows(i).Item("Manufacture_Date").ToString)
                            End If
                        End If

                        dgv_Details.Rows(n).Cells(22).Value = Val(dt2.Rows(i).Item("Expiry_Period_Days").ToString)

                        If IsDate(dt2.Rows(i).Item("Expiry_Date").ToString) = True Then
                            If DateDiff(DateInterval.Day, Convert.ToDateTime("01/01/1900"), dt2.Rows(i).Item("Expiry_Date")) > 0 Then
                                dgv_Details.Rows(n).Cells(23).Value = Val(dt2.Rows(i).Item("Expiry_Day").ToString)
                                dgv_Details.Rows(n).Cells(24).Value = Common_Procedures.Month_IdNoToShortName(con, Val(dt2.Rows(i).Item("Expiry_Month_IdNo").ToString))
                                dgv_Details.Rows(n).Cells(25).Value = Val(dt2.Rows(i).Item("Expiry_Year").ToString)
                                dgv_Details.Rows(n).Cells(26).Value = (dt2.Rows(i).Item("Expiry_Date").ToString)
                            End If
                        End If


                        dgv_Details.Rows(n).Cells(27).Value = Format(Val(dt2.Rows(i).Item("Assessable_Value").ToString), "############0.00")
                        dgv_Details.Rows(n).Cells(28).Value = Trim(dt2.Rows(i).Item("HSN_Code").ToString)
                        dgv_Details.Rows(n).Cells(29).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "############0.00")


                        ' dgv_Details.Rows(n).Cells(26).Value = (dt2.Rows(i).Item("Detail_SlNo").ToString)
                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                Total_Calculation()

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()


                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Purchase_Tax_Details a  Where a.Purchase_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt3 = New DataTable
                da2.Fill(dt3)

                With dgv_Tax_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Format(Val(dt3.Rows(i).Item("Gross_Amount").ToString), "############0.00")
                            .Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Discount_Amount").ToString), "############0.00")
                            .Rows(n).Cells(3).Value = Format(Val(dt3.Rows(i).Item("Aessable_Amount").ToString), "############0.00")
                            .Rows(n).Cells(4).Value = Format(Val(dt3.Rows(i).Item("Tax_Perc").ToString), "############0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt3.Rows(i).Item("Tax_Amount").ToString), "############0.00")



                        Next i

                    End If

                End With


            End If

            Grid_Cell_DeSelect()

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        NoCalc_Status = False
        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Sales_Entry_BatchNo_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim dt1 As New DataTable


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Sales_Entry_BatchNo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        FrmLdSTS = True

        Me.Text = ""

        con.Open()

        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Else
            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        End If

        txt_GrossAmount.Enabled = False


        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"


        If Trim(Common_Procedures.settings.CustomerCode) = "1108" Then

            txt_Code.Visible = False
            lbl_Code.Visible = False
            cbo_ItemName.Left = 41
            cbo_ItemName.Width = 288
            dgv_Details.Columns(1).Visible = False
            dgv_Details_Total.Columns(1).Visible = False
        Else
            txt_Code.Visible = True
            lbl_Code.Visible = True
            cbo_ItemName.Left = 110
            cbo_ItemName.Width = 225
            dgv_Details.Columns(1).Visible = True
            dgv_Details_Total.Columns(1).Visible = True
            'dgv_Details.Columns(1).left = 90
        End If



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_BatchSelection_ToolTip.Visible = False
        pnl_BatchSelection_ToolTip.Left = 260
        pnl_BatchSelection_ToolTip.Top = 200

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = (Me.Height - pnl_Tax.Height) \ 2
        pnl_Tax.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()


        pnl_PreviousBillDetails.Visible = False
        pnl_PreviousBillDetails.Left = 10
        pnl_PreviousBillDetails.Top = 400
        pnl_PreviousBillDetails.BringToFront()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("NO TAX")
        cbo_TaxType.Items.Add("GST")

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofItems.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReceivedAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_BalanceAmount.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Mfg_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountAmountItem.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RateWith_Tax.GotFocus, AddressOf ControlGotFocus

        AddHandler lbl_Sales_Price.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Batch_No.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Manufacture_Day.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Manufacture_Month.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Manufacture_Year.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Expiray_Period_Days.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Expiray_Day.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_ExpiryMonth.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Expiry_Year.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_TotalQty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Aessableamount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler dgv_Details.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalGrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLessAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RoundOff.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NetAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Purchase.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrossAmount.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Delete.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReceivedAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_BalanceAmount.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofItems.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Mfg_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscountAmountItem.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RateWith_Tax.LostFocus, AddressOf ControlLostFocus

        AddHandler lbl_Sales_Price.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Batch_No.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Manufacture_Day.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Manufacture_Month.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Manufacture_Year.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Expiray_Period_Days.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Expiray_Day.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_ExpiryMonth.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Expiry_Year.LostFocus, AddressOf ControlLostFocus



        AddHandler txt_TotalQty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Aessableamount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler dgv_Details.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalGrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLessAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RoundOff.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NetAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Purchase.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Delete.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoofItems.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReceivedAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RateWith_Tax.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler lbl_Mrp_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLessAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Sales_Price.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Batch_No.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Manufacture_Day.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Manufacture_Year.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Expiray_Period_Days.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Expiray_Day.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Expiry_Year.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_TotalQty.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Aessableamount.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_TotalDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalGrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_AddLessAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RoundOff.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NetAmount.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReceivedAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoofItems.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Mrp_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler lbl_Sales_Price.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Manufacture_Day.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Batch_No.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalQty.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Aessableamount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalGrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLessAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RoundOff.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NetAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Manufacture_Year.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Expiray_Period_Days.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Expiray_Day.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Expiry_Year.KeyPress, AddressOf TextBoxControlKeyPress


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

    Private Sub Sales_Entry_BatchNo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Sales_Entry_BatchNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub


                ElseIf pnl_Tax.Visible = True Then
                    btn_Tax_Close_Click(sender, e)
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)


            cmd.CommandText = "Update Item_Stock_Selection_Processing_Details Set Outward_Quantity = a.Outward_Quantity - b.Noof_Items from Item_Stock_Selection_Processing_Details a, Sales_Details b where Sales_Code = '" & Trim(NewCode) & "' AND a.Item_idNo = b.Item_IdNo and a.Batch_No =b.Batch_Serial_No and a.Manufactured_Date=b.Manufacture_DAte and a.Expiry_Date = b.Expiry_Date and a.Mrp_Rate = b.Mrp_Rate"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Purchase_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
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
            cbo_Filter_Purchase.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            cbo_Filter_Purchase.SelectedIndex = -1
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
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby, Sales_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby, Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Sales_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby desc, Sales_No desc"

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
        Dim da As New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type ='GST' Order by for_Orderby desc, Sales_No desc", con)
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

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "Entry_VAT_GST_Type ='GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red



            da = New SqlClient.SqlDataAdapter("select a.Payment_Method,  a.Tax_Type from Sales_Head a  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Entry_VAT_GST_Type ='GST' Order by a.for_Orderby desc, a.Sales_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                cbo_PaymentMethod.Text = dt2.Rows(0).Item("Payment_Method").ToString
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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "' and Entry_VAT_GST_Type ='GST'"
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
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
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
        Dim TxAc_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Man_Mntn_id As Integer = 0
        Dim Exp_Mnth_id As Integer = 0
        Dim Man_dte As String = ""
        Dim Exp_dte As String = ""
        Dim Slno As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim Amt As Single = 0
        Dim Bat_Qty As Single = 0
        Dim TxAmt_Diff As Single = 0, TotTxAmt As Single = 0
        Dim VouBil As String = ""
        Dim saleac_id As Integer = 0
        Dim vforOrdby As Single = 0

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

        Amount_Calculation(True)

        NoCalc_Status = False

        Total_Calculation()
        Dim Tot_Qty As Single = 0
        Dim Tot_SubAmt As Single = 0
        Dim Tot_DisAmt As Single = 0
        If dgv_Details_Total.RowCount > 0 Then
            Tot_Qty = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            Tot_SubAmt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            Tot_DisAmt = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If








        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_InvoiceNo.Text)

                lbl_InvoiceNo.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)



            If New_Entry = True Then

                cmd.CommandText = "Insert into Sales_Head( Sales_Code, Company_IdNo, Sales_No, for_OrderBy, Sales_Date, Payment_Method, Ledger_IdNo,  Tax_Type,  Total_Qty,SubTotal_Amount , Aessable_Amount, Total_DiscountAmount_item, Total_TaxAmount, Gross_Amount, CashDiscount_Perc, CashDiscount_Amount, AddLess_Amount, Round_Off, Net_Amount, Freight_Amount,Freight_Name,AddLess_Name, Received_Amount   ,  Balance_Amount ,Total_DiscountAmount ,Entry_VAT_GST_Type ,Assessable_Value , CGst_Amount ,SGst_Amount , IGst_Amount ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", '" & Trim(cbo_TaxType.Text) & "',  " & Str(Val(Tot_Qty)) & "," & Str(Val(Tot_SubAmt)) & ", " & Str(Val(txt_Aessableamount.Text)) & ", " & Str(Val(txt_TotalDiscAmount.Text)) & ", " & Str(Val(lbl_TotalTaxAmount.Text)) & ", " & Str(Val(txt_TotalGrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_AddLessAmount.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(txt_NetAmount.Text)) & " ,  " & Str(Val(txt_Freight.Text)) & ",'" & Trim(txt_Freight_Name.Text) & "' ,'" & Trim(txt_AddLess_Name.Text) & "' ," & Val(txt_ReceivedAmount.Text) & " , " & Val(lbl_BalanceAmount.Text) & "," & Val(Tot_DisAmt) & " , 'GST' , " & Val(lbl_Assessable.Text) & " , " & Val(lbl_CGstAmount.Text) & " , " & Val(lbl_SGstAmount.Text) & " , " & Val(lbl_IGstAmount.Text) & ")"

                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Sales_Head set Sales_Date = @SalesDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ",  Tax_Type = '" & Trim(cbo_TaxType.Text) & "',Freight_Name = '" & Trim(txt_Freight_Name.Text) & "' ,Freight_Amount = " & Str(Val(txt_Freight.Text)) & "  ,  Total_Qty = " & Str(Val(Tot_Qty)) & ",SubTotal_Amount = " & Str(Val(Tot_SubAmt)) & ",  Aessable_Amount = " & Str(Val(txt_Aessableamount.Text)) & ", Total_DiscountAmount_item = " & Str(Val(txt_TotalDiscAmount.Text)) & ", Total_TaxAmount = " & Str(Val(lbl_TotalTaxAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_TotalGrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ",AddLess_Name ='" & Trim(txt_AddLess_Name.Text) & "' , AddLess_Amount = " & Str(Val(txt_AddLessAmount.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(txt_NetAmount.Text)) & " ,Received_Amount = " & Val(txt_ReceivedAmount.Text) & " , Balance_Amount = " & Val(lbl_BalanceAmount.Text) & ",Total_DiscountAmount =" & Val(Tot_DisAmt) & ",Entry_VAT_GST_Type = 'GST' ,Assessable_Value = " & Val(lbl_Assessable.Text) & " , CGst_Amount =" & Val(lbl_CGstAmount.Text) & " ,SGst_Amount =" & Val(lbl_SGstAmount.Text) & " , IGst_Amount =" & Val(lbl_IGstAmount.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()



                cmd.CommandText = "Update Item_Stock_Selection_Processing_Details Set Outward_Quantity = a.Outward_Quantity - b.Noof_Items from Item_Stock_Selection_Processing_Details a, Sales_Details b where Sales_Code = '" & Trim(NewCode) & "' AND  a.Item_idNo = b.Item_IdNo and a.Batch_No =b.Batch_Serial_No and a.Manufactured_Date=b.Manufacture_DAte and a.Expiry_Date = b.Expiry_Date and a.Mrp_Rate = b.Mrp_Rate"
                cmd.ExecuteNonQuery()

            End If

            'TxAmt_Diff = 0

            'TotTxAmt = 0
            'For i = 0 To dgv_Details.RowCount - 1

            '    If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then
            '        TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(12).Value)
            '    End If

            'Next

            TxAmt_Diff = Format(Val(lbl_TotalTaxAmount.Text) - Val(TotTxAmt), "#########0.00")

            cmd.CommandText = "Delete from Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Purchase_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0
            Slno = 0

            For i = 0 To dgv_Details.RowCount - 1

                itm_id = 0
                unt_id = 0

                itm_id = Val(Common_Procedures.Item_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(2).Value), tr))

                If itm_id <> 0 Then

                    unt_id = Val(Common_Procedures.Unit_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(3).Value), tr))

                    Man_Mntn_id = Common_Procedures.Month_ShortNameToIdNo(con, dgv_Details.Rows(i).Cells(19).Value, tr)
                    Exp_Mnth_id = Common_Procedures.Month_ShortNameToIdNo(con, dgv_Details.Rows(i).Cells(24).Value, tr)


                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)

                    If Val(dgv_Details.Rows(i).Cells(18).Value) = 0 Then dgv_Details.Rows(i).Cells(18).Value = 1

                    Man_dte = (Val(dgv_Details.Rows(i).Cells(18).Value)) & "/" & Val(Man_Mntn_id) & "/" & Val(dgv_Details.Rows(i).Cells(20).Value)

                    If IsDate(Man_dte) = False Then
                        Man_dte = "1/1/1900"
                    End If

                    If IsDate(Man_dte) = True Then
                        cmd.Parameters.AddWithValue("@ManufactureDate", Convert.ToDateTime(Man_dte))
                    End If

                    '  cmd.Parameters.AddWithValue("@ManufactureDate", Man_dte.Date)

                    If Val(dgv_Details.Rows(i).Cells(23).Value) = 0 Then dgv_Details.Rows(i).Cells(23).Value = 1


                    Exp_dte = (Val(dgv_Details.Rows(i).Cells(23).Value)) & "/" & Val(Exp_Mnth_id) & "/" & Val(dgv_Details.Rows(i).Cells(25).Value)

                    If IsDate(Exp_dte) = False Then
                        Exp_dte = "1/1/1900"
                    End If

                    If IsDate(Exp_dte) = True Then
                        cmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(Exp_dte))
                    End If



                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Sales_Details(Sales_Code         , Company_IdNo                     , Sales_No                          , for_OrderBy                                                               , Sales_Date , Ledger_IdNo              , SL_No                ,  Item_Code                                        , Item_IdNo              , Unit_IdNo               , Noof_Items                                          , Rate                                                ,Rate_Tax                                       , Amount                                              , Discount_Perc                                       , Discount_Amount                                          ,Discount_Perc_Item                                     , Discount_Amount_Item                                 ,Tax_Amount ,Total_Amount, MRP_Rate ,Sales_Price, Batch_Serial_No,Manufacture_Day  ,Manufacture_Month_IdNo , Manufacture_Year , Manufacture_Date ,Expiry_Period_Days  ,Expiry_Day   ,  Expiry_Month_IdNo  , Expiry_Year,Expiry_Date  ,   Assessable_Value  , HSN_Code , Tax_Perc ) " & _
                                                    "Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate , " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ",'" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "', " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(10).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(11).Value)) & "," & Str(Val(dgv_Details.Rows(i).Cells(13).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & "," & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & "," & Val(dgv_Details.Rows(i).Cells(16).Value) & ",'" & Trim(dgv_Details.Rows(i).Cells(17).Value) & "'," & Str(Val(dgv_Details.Rows(i).Cells(18).Value)) & "," & (Man_Mntn_id) & "," & Str(Val(dgv_Details.Rows(i).Cells(20).Value)) & "," & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & " ," & Str(Val(dgv_Details.Rows(i).Cells(22).Value)) & "," & Str(Val(dgv_Details.Rows(i).Cells(23).Value)) & "," & Val(Exp_Mnth_id) & ",    " & Str(Val(dgv_Details.Rows(i).Cells(25).Value)) & "," & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & "  ," & Str(Val(dgv_Details.Rows(i).Cells(27).Value)) & ", '" & Trim(dgv_Details.Rows(i).Cells(28).Value) & "', " & Str(Val(dgv_Details.Rows(i).Cells(29).Value)) & " )"
                    cmd.ExecuteNonQuery()
                    nr = 0
                    cmd.CommandText = "Insert into Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Party_Bill_No, SL_No, Item_IdNo, Unit_IdNo, Quantity) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", '" & Trim(txt_BillNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Val(dgv_Details.Rows(i).Cells(4).Value) & " )"
                    nr = cmd.ExecuteNonQuery()

                    '    If Trim(dgv_Details.Rows(i).Cells(17).Value) <> "" Then
                    nr = 0

                    cmd.CommandText = "Update Item_Stock_Selection_Processing_Details Set Outward_Quantity = Outward_Quantity + " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " where Item_IdNo = " & Val(itm_id) & " and Batch_NO = '" & Trim(dgv_Details.Rows(i).Cells(17).Value) & "' and  Manufactured_Date= " & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & "  and  Expiry_Date= " & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & " and Mrp_Rate = " & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & ""
                    nr = cmd.ExecuteNonQuery()

                    If nr = 0 Then
                        tr.Rollback()
                        MessageBox.Show("Mismatch of Item and BatchNo details", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                        Exit Sub
                    End If
                    'End If



                End If


            Next


            Dim Slno1 As Integer

            With dgv_Tax_Details

                Slno1 = 0


                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        Slno1 = Slno1 + 1


                        cmd.CommandText = "Insert into Purchase_Tax_Details (    Purchase_Code                    ,      Company_IdNo              ,      Purchase_No                               , for_OrderBy                                                                   ,     Purchase_Date              ,     Ledger_IdNo     ,      Sl_No             ,   Item_IdNo  ,       Gross_Amount                        ,          Discount_Amount                ,       Aessable_Amount                   ,   Tax_Perc                                  , Tax_Amount                       ) " & _
                                                "     Values                  ( '" & Trim(NewCode) & "'                , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "'            , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",    @SalesDate             ,  " & Val(led_id) & " , " & Str(Val(Slno1)) & ", " & Val(itm_id) & ",  " & Val(.Rows(i).Cells(1).Value) & "  , " & Str(Val(.Rows(i).Cells(2).Value)) & " ," & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & " ," & Str(Val(.Rows(i).Cells(5).Value)) & ") "
                        cmd.ExecuteNonQuery()
                    End If




                Next

            End With

            nr = 0
            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            nr = cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Ac_id = 0
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                Ac_id = 1
            Else
                Ac_id = led_id
            End If

            saleac_id = 22

            Ac_id = 0
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                Ac_id = 1
            Else
                Ac_id = led_id
            End If

            cmd.CommandText = "Insert into Voucher_Head (     Voucher_Code            ,          For_OrderByCode                                                     ,             Company_IdNo         ,           Voucher_No              ,             For_OrderBy                                                      , Voucher_Type, Voucher_Date,           Debtor_Idno  ,          Creditor_Idno     ,                Total_VoucherAmount        ,         Narration                                , Indicate,       Year_For_Report                                     ,       Entry_Identification                  , Voucher_Receipt_Code ) " & _
                                                     " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Sales' ,   @SalesDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(txt_NetAmount.Text))) & ",    'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "',    1    , " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "',          ''          ) "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details (       Voucher_Code                   ,          For_OrderByCode                                                        ,              Company_IdNo        ,           Voucher_No              ,           For_OrderBy                                                       , Voucher_Type, Voucher_Date, SL_No,        Ledger_IdNo     ,                       Voucher_Amount           ,              Narration                        ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              "   Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",  'Sales',  @SalesDate ,   1  , " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(txt_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            cmd.ExecuteNonQuery()

            Amt = Val(CSng(txt_NetAmount.Text)) - Val(txt_TaxAmount.Text) - Val(txt_Freight.Text) - Val(txt_AddLessAmount.Text) - Val(txt_RoundOff.Text)

            cmd.CommandText = "Insert into Voucher_Details (      Voucher_Code                  ,          For_OrderByCode                                                      ,             Company_IdNo         ,           Voucher_No              ,           For_OrderBy                                                         , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo       ,     Voucher_Amount   ,     Narration                                 ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",  'Sales',  @SalesDate ,   2  , " & Str(Val(saleac_id)) & ", " & Str(Val(Amt)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            cmd.ExecuteNonQuery()

            If Val(txt_TaxAmount.Text) <> 0 Then
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode                                                       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy                                                        , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo     ,             Voucher_Amount          ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Sales' ,   @SalesDate,   3  , " & Str(Val(TxAc_id)) & ", " & Str(Val(txt_TaxAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_Freight.Text) <> 0 Then
                L_id = 9
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode                                                           ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy                                                          , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Sales' ,   @SalesDate,   4  , " & Str(Val(L_id)) & ", " & Str(Val(txt_Freight.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_AddLessAmount.Text) <> 0 Then
                L_id = 17
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode                                                           ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   5  , " & Str(Val(L_id)) & ", " & Str(Val(txt_AddLessAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_RoundOff.Text) <> 0 Then
                L_id = 24
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode                                                           ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount         ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   6  , " & Str(Val(L_id)) & ", " & Str(Val(txt_RoundOff.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If


            '---Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, led_id, Trim(lbl_Batch_No.Text), 0, Val(CSng(txt_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If


            tr.Commit()

            If New_Entry = True Then
                move_record(lbl_InvoiceNo.Text)
                'new_record()
            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Err.Description, "CK_Item_Stock_Selection_Processing_Details_1") > 0 Then
                MessageBox.Show("Invalid Inward Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf InStr(1, Err.Description, "CK_Item_Stock_Selection_Processing_Details_2") > 0 Then
                MessageBox.Show("Invalid Outward QuantityMust be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf InStr(1, Err.Description, "CK_Item_Stock_Selection_Processing_Details_3") > 0 Then
                MessageBox.Show("Invalid Inward Quantity, Inward Quantity must be lesser than Outward Quantity", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If


        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName
            cmbItmNm = cbo_ItemName.Text
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "item_head", "item_Name", "", "(item_idno = 0)")
        End With

    End Sub
    Private Sub get_Item_Details()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0

        ItmIdNo = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

        da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_idno = " & Str(Val(ItmIdNo)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then

            If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
                lbl_Unit.Text = dt.Rows(0)("unit_name").ToString
            End If
            If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
            End If

            If IsDBNull(dt.Rows(0)("Tax_Percentage").ToString) = False Then
                txt_TaxPerc.Text = dt.Rows(0)("Tax_Percentage").ToString
            End If
            get_Item_Tax(False)
        End If
        dt.Dispose()
        da.Dispose()

    End Sub
    Private Sub get_Item_Tax(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0

        Try

            If FrmLdSTS = True Then Exit Sub

            lbl_Grid_GstPerc.Text = ""
            lbl_Grid_HsnCode.Text = ""

            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

             
                ItmIdNo = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)


                lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")

                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                        lbl_Grid_HsnCode.Text = dt.Rows(0)("Item_HSN_Code").ToString
                    End If
                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                        lbl_Grid_GstPerc.Text = dt.Rows(0)("Item_GST_Percentage").ToString
                    End If
                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            End If

            Amount_Calculation(False)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT GET ITEM TAX DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

        If Trim(lbl_Unit.Text) = "" Then
            MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If
        If Val(txt_NoofItems.Text) = 0 And Val(lbl_Mrp_Rate.Text) = 0 Then
            MessageBox.Show("Invalid No.of Items", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_NoofItems.Enabled Then txt_NoofItems.Focus()
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 Then
            If Val(txt_NoofItems.Text) <> 0 Then
                MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
                If txt_Rate.Enabled Then txt_Rate.Focus()
                Exit Sub
            End If
        End If
        MtchSTS = False



        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then
                    .Rows(i).Cells(1).Value = txt_Code.Text
                    .Rows(i).Cells(2).Value = cbo_ItemName.Text
                    .Rows(i).Cells(3).Value = lbl_Unit.Text
                    .Rows(i).Cells(4).Value = Val(txt_NoofItems.Text)
                    .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(txt_RateWith_Tax.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(txt_Amount.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(txt_DiscAmount.Text), "########0.00")
                    .Rows(i).Cells(10).Value = Format(Val(txt_DisPerc_Item.Text), "########0.00")
                    .Rows(i).Cells(11).Value = Format(Val(txt_DiscountAmountItem.Text), "########0.00")
                    .Rows(i).Cells(12).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                    .Rows(i).Cells(13).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                    .Rows(i).Cells(14).Value = Format(Val(txt_GrossAmount.Text), "########0.00")
                    .Rows(i).Cells(15).Value = Val(lbl_Mrp_Rate.Text)
                    .Rows(i).Cells(16).Value = Format(Val(lbl_Sales_Price.Text), "########0.00")
                    .Rows(i).Cells(17).Value = (lbl_Batch_No.Text)
                    .Rows(i).Cells(18).Value = Val(lbl_Manufacture_Day.Text)
                    .Rows(i).Cells(19).Value = (lbl_Manufacture_Month.Text)
                    .Rows(i).Cells(20).Value = Val(lbl_Manufacture_Year.Text)
                    .Rows(i).Cells(21).Value = (txt_Mfg_Date.Text)
                    .Rows(i).Cells(22).Value = Val(lbl_Expiray_Period_Days.Text)
                    .Rows(i).Cells(23).Value = Val(lbl_Expiray_Day.Text)
                    .Rows(i).Cells(24).Value = (lbl_ExpiryMonth.Text)
                    .Rows(i).Cells(25).Value = Val(lbl_Expiry_Year.Text)
                    .Rows(i).Cells(26).Value = (txt_Exp_date.Text)

                    .Rows(i).Cells(27).Value = Val(lbl_Grid_AssessableValue.Text)
                    .Rows(i).Cells(28).Value = Trim(lbl_Grid_HsnCode.Text)
                    .Rows(i).Cells(29).Value = Val(lbl_Grid_GstPerc.Text)

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 10 Then .FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = txt_Code.Text
                .Rows(n).Cells(2).Value = cbo_ItemName.Text
                .Rows(n).Cells(3).Value = lbl_Unit.Text
                .Rows(n).Cells(4).Value = Val(txt_NoofItems.Text)
                .Rows(n).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(6).Value = Format(Val(txt_RateWith_Tax.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(txt_Amount.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(txt_DiscAmount.Text), "########0.00")
                .Rows(n).Cells(10).Value = Format(Val(txt_DisPerc_Item.Text), "########0.00")
                .Rows(n).Cells(11).Value = Format(Val(txt_DiscountAmountItem.Text), "########0.00")
                .Rows(n).Cells(12).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                .Rows(n).Cells(13).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                .Rows(n).Cells(14).Value = Format(Val(txt_GrossAmount.Text), "########0.00")
                .Rows(n).Cells(15).Value = Val(lbl_Mrp_Rate.Text)
                .Rows(n).Cells(16).Value = Format(Val(lbl_Sales_Price.Text), "########0.00")
                .Rows(n).Cells(17).Value = (lbl_Batch_No.Text)
                .Rows(n).Cells(18).Value = Val(lbl_Manufacture_Day.Text)
                .Rows(n).Cells(19).Value = (lbl_Manufacture_Month.Text)
                .Rows(n).Cells(20).Value = Val(lbl_Manufacture_Year.Text)
                .Rows(n).Cells(21).Value = (txt_Mfg_Date.Text)
                .Rows(n).Cells(22).Value = Val(lbl_Expiray_Period_Days.Text)
                .Rows(n).Cells(23).Value = Val(lbl_Expiray_Day.Text)
                .Rows(n).Cells(24).Value = (lbl_ExpiryMonth.Text)
                .Rows(n).Cells(25).Value = Val(lbl_Expiry_Year.Text)
                .Rows(n).Cells(26).Value = (txt_Exp_date.Text)

                .Rows(n).Cells(27).Value = Val(lbl_Grid_AssessableValue.Text)
                .Rows(n).Cells(28).Value = Trim(lbl_Grid_HsnCode.Text)
                .Rows(n).Cells(29).Value = Val(lbl_Grid_GstPerc.Text)

                .Rows(n).Selected = True

                If n >= 10 Then .FirstDisplayedScrollingRowIndex = n - 9

            End If

        End With

        GrossAmount_Calculation()


        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        txt_Code.Text = ""
        cbo_ItemName.Text = ""
        lbl_Unit.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_RateWith_Tax.Text = ""
        txt_Amount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscAmount.Text = ""
        txt_DisPerc_Item.Text = ""
        txt_DiscountAmountItem.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_GrossAmount.Text = ""
        lbl_Mrp_Rate.Text = ""
        lbl_Sales_Price.Text = ""
        lbl_Batch_No.Text = ""
        lbl_Manufacture_Day.Text = ""
        lbl_Manufacture_Month.Text = ""
        lbl_Manufacture_Year.Text = ""
        txt_Mfg_Date.Text = ""
        lbl_Expiray_Period_Days.Text = ""
        lbl_Expiray_Day.Text = ""
        lbl_ExpiryMonth.Text = ""
        lbl_Expiry_Year.Text = ""
        txt_Exp_date.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_GstPerc.Text = ""
        lbl_Grid_HsnCode.Text = ""


        'dgv_Batch_details.Rows.Clear()

        If txt_Code.Visible = True Then
            If txt_Code.Enabled And txt_Code.Visible Then txt_Code.Focus()
        Else
            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
        End If


    End Sub

    Private Sub txt_NoofItems_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofItems.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoofItems.TextChanged
        Call Amount_Calculation(False)
    End Sub




    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        ' txt_Mfg_Date.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
        Amount_Calculation()
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        ' txt_RateWith_Tax.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "########0.00")
        Call Amount_Calculation(False)
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

    Private Sub txt_TaxPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            txt_DiscPerc.Focus()
        End If
        If e.KeyCode = 40 Then
            If btn_Add.Enabled Then btn_Add.Focus()
        End If

    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)

        End If

    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        'txt_Mfg_Date.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
        'Amount_Calculation()
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
        'Dim da As SqlClient.SqlDataAdapter
        'Dim dt As New DataTable

        'If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
        '    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a, unit_head b where a.item_name = '" & Trim(cbo_ItemName.Text) & "' and a.unit_idno = b.unit_idno", con)
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then
        '        lbl_Unit.Text = dt.Rows(0)("unit_name").ToString
        '        txt_Rate.Text = dt.Rows(0)("Cost_Rate").ToString
        '        txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
        '        lbl_Sales_Price.Text = dt.Rows(0)("Sales_Rate").ToString
        '        txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
        '        lbl_Mrp_Rate.Text = dt.Rows(0)("MRP_Rate").ToString
        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If

        If Trim(UCase(cbo_ItemName.Text)) <> "" Then
            If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
                cmbItmNm = cbo_ItemName.Text
                get_Item_Details()
            End If
        End If

    End Sub


    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")


    End Sub

    Private Sub txt_ReceivedAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ReceivedAmount.KeyDown
        'If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub





    Private Sub txt_ReceivedAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ReceivedAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_AddLessAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLessAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLessAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TotalDiscAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TotalDiscAmount.KeyDown
        If e.KeyCode = 40 Then
            txt_CashDiscPerc.Focus()
        End If
        If e.KeyCode = 38 Then
            txt_Code.Focus()
        End If
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_TotalGrossAmount.TextChanged
        NetAmount_Calculation()
    End Sub



    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        .Rows(i).Cells(1).Value = txt_Code.Text
                        .Rows(i).Cells(2).Value = cbo_ItemName.Text
                        .Rows(i).Cells(3).Value = lbl_Unit.Text
                        .Rows(i).Cells(4).Value = Val(txt_NoofItems.Text)
                        .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                        .Rows(i).Cells(6).Value = Format(Val(txt_RateWith_Tax.Text), "########0.00")
                        .Rows(i).Cells(7).Value = Format(Val(txt_Amount.Text), "########0.00")
                        .Rows(i).Cells(8).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                        .Rows(i).Cells(9).Value = Format(Val(txt_DiscAmount.Text), "########0.00")
                        .Rows(i).Cells(10).Value = Format(Val(txt_DisPerc_Item.Text), "########0.00")
                        .Rows(i).Cells(11).Value = Format(Val(txt_DiscountAmountItem.Text), "########0.00")
                        .Rows(i).Cells(12).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                        .Rows(i).Cells(13).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                        .Rows(i).Cells(14).Value = Format(Val(txt_GrossAmount.Text), "########0.00")
                        .Rows(i).Cells(15).Value = Val(lbl_Mrp_Rate.Text)
                        .Rows(i).Cells(16).Value = Format(Val(lbl_Sales_Price.Text), "########0.00")
                        .Rows(i).Cells(17).Value = (lbl_Batch_No.Text)
                        .Rows(i).Cells(18).Value = Val(lbl_Manufacture_Day.Text)
                        .Rows(i).Cells(19).Value = (lbl_Manufacture_Month.Text)
                        .Rows(i).Cells(20).Value = Val(lbl_Manufacture_Year.Text)
                        .Rows(i).Cells(21).Value = (txt_Mfg_Date.Text)
                        .Rows(i).Cells(22).Value = Val(lbl_Expiray_Period_Days.Text)
                        .Rows(i).Cells(23).Value = Val(lbl_Expiray_Day.Text)
                        .Rows(i).Cells(24).Value = Val(lbl_ExpiryMonth.Text)
                        .Rows(i).Cells(25).Value = Val(lbl_Expiry_Year.Text)
                        .Rows(i).Cells(26).Value = (txt_Exp_date.Text)

                        .Rows(i).Cells(27).Value = Format(Val(lbl_Grid_AssessableValue.Text), "############0.00")
                        .Rows(i).Cells(28).Value = Trim(lbl_Grid_HsnCode.Text)
                        .Rows(i).Cells(29).Value = Format((lbl_Grid_GstPerc.Text), "#############0.00")

                        Exit For

                    End If

                Next

            End With

            SendKeys.Send("{TAB}")

        End If
    End Sub



    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Mfg_Date.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    'Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Mfg_Date.KeyUp
    '    txt_Rate.Text = Format(Val(txt_Mfg_Date.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
    '    Amount_Calculation()
    'End Sub

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

    End Sub




    Private Sub Amount_Calculation()
        Dim totDisc As Decimal

        If FrmLdSTS = True Then Exit Sub

        If Trim(txt_Rate.Text) = "" Then
            txt_Rate.Text = Format((Val(txt_RateWith_Tax.Text)) - (Val(txt_RateWith_Tax.Text) * (Val(txt_TaxPerc.Text) / (100 + Val(txt_TaxPerc.Text)))), "#####0.00")

        Else
            txt_RateWith_Tax.Text = Format(Val(txt_Rate.Text) + (Val(txt_Rate.Text) * Val(txt_TaxPerc.Text) / 100), "######0.00")
        End If
        txt_Amount.Text = Format(Val(txt_NoofItems.Text) * Val(txt_Rate.Text), "#########0.00")

        txt_DiscAmount.Text = Format(Val(txt_NoofItems.Text) * ((Val(txt_Rate.Text) - Val(txt_DiscountAmountItem.Text)) * Val(txt_DiscPerc.Text) / 100), "#########0.00")


        txt_TaxAmount.Text = "0.00"
        If Trim(cbo_TaxType.Text) <> "" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
            totDisc = (Val(txt_DiscountAmountItem.Text) * Val(txt_NoofItems.Text))
            txt_TaxAmount.Text = Format(((Val(txt_Amount.Text) - totDisc - Val(txt_DiscAmount.Text)) * Val(txt_TaxPerc.Text) / 100), "#########0.00")
        End If

        txt_GrossAmount.Text = Format(Val(txt_Amount.Text) - (Val(txt_DiscountAmountItem.Text) * Val(txt_NoofItems.Text)) - Val(txt_DiscAmount.Text) + Val(txt_TaxAmount.Text), "########0.00")


    End Sub

    Private Sub Amount_Calculation(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0

        '***** GST START *****

        If FrmLdSTS = True Then Exit Sub

        If GridAll_Row_STS = True Then

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        ItmIdNo = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                        If ItmIdNo <> 0 Then

                            .Rows(i).Cells(28).Value = ""
                            .Rows(i).Cells(29).Value = ""

                            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                                dt = New DataTable
                                da.Fill(dt)
                                If dt.Rows.Count > 0 Then
                                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                                        .Rows(i).Cells(28).Value = Format(Val(dt.Rows(0)("Item_GST_Percentage").ToString), "#########0.00")
                                    End If
                                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                                        .Rows(i).Cells(29).Value = dt.Rows(0)("Item_HSN_Code").ToString
                                    End If
                                End If
                                dt.Clear()

                            End If

                            .Rows(i).Cells(7).Value = Format(Val(.Rows(i).Cells(4).Value) * Val(.Rows(i).Cells(5).Value), "#########0.00")


                            .Rows(i).Cells(9).Value = Format(Val(.Rows(i).Cells(7).Value) * .Rows(i).Cells(8).Value / 100, "#########0.00")


                            .Rows(i).Cells(10).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
                            .Rows(i).Cells(11).Value = Format(Val(.Rows(i).Cells(6).Value) * Val(.Rows(i).Cells(10).Value) / 100, "#########0.00")


                            .Rows(i).Cells(12).Value = Format(Val(.Rows(i).Cells(28).Value), "#########0.00")
                            .Rows(i).Cells(13).Value = Format(Val(.Rows(i).Cells(6).Value) * Val(.Rows(i).Cells(12).Value) / 100, "#########0.00")


                            .Rows(i).Cells(14).Value = Format(Val(.Rows(i).Cells(7).Value) - Val(.Rows(i).Cells(9).Value) - Val(.Rows(i).Cells(11).Value), "#########0.00")

                            .Rows(i).Cells(27).Value = Format(Val(.Rows(i).Cells(7).Value) - Val(.Rows(i).Cells(9).Value) - Val(.Rows(i).Cells(11).Value), "#########0.00")
                        End If

                    End If

                Next

            End With

            Total_Calculation()

        Else

            txt_Amount.Text = Format(Val(txt_NoofItems.Text) * Val(txt_Rate.Text), "#########0.00")
            lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
            lbl_Grid_DiscAmount.Text = Format(Val(txt_Amount.Text) * Val(lbl_Grid_DiscPerc.Text) / 100, "#########0.00")
            lbl_Grid_AssessableValue.Text = Format(Val(txt_Amount.Text) - Val(lbl_Grid_DiscAmount.Text), "#########0.00")

        End If

        '***** GST END *****

    End Sub


    Private Sub GrossAmount_Calculation()
        Dim Sno As Integer
        Dim TotQty As Decimal, TotSubAmt As Decimal, TotDiscAmt As Decimal, TotTxAmt As Decimal, TotAmt As Decimal
        Dim TotDisAmtItm As Integer = 0

        If FrmLdSTS = True Then Exit Sub
        Sno = 0
        TotQty = 0
        TotSubAmt = 0
        TotDiscAmt = 0
        TotTxAmt = 0
        TotAmt = 0
        For i = 0 To dgv_Details.RowCount - 1
            Sno = Sno + 1
            dgv_Details.Rows(i).Cells(0).Value = Sno

            TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(4).Value)
            TotSubAmt = TotSubAmt + Val(dgv_Details.Rows(i).Cells(7).Value)
            TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(9).Value) + (Val(dgv_Details.Rows(i).Cells(11).Value) * Val(dgv_Details.Rows(i).Cells(4).Value))
            TotDisAmtItm = TotTxAmt + Val(dgv_Details.Rows(i).Cells(11).Value)
            TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(13).Value)
            TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(14).Value)


        Next

        ' txt_TotalQty.Text = Val(TotQty)
        ' txt_Aessableamount.Text = Format(TotSubAmt, "########0.00")
        txt_TotalDiscAmount.Text = Format(TotDisAmtItm, "########0.00")
        lbl_TotalTaxAmount.Text = Format(TotTxAmt, "########0.00")
        txt_TotalGrossAmount.Text = Format(TotAmt, "########0.00")

        Total_Calculation()
    End Sub
    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotQty As Decimal, TotSubAmt As Decimal, TotDiscAmt As Decimal, TotTxAmt As Decimal, TotAmt As Decimal, TotAssAmt As Decimal
        Dim TotDisAmtItm As Integer = 0
        Dim TotAssval As Decimal = 0
        Dim TotCGstAmt As Decimal = 0
        Dim TotSGstAmt As Decimal = 0
        Dim TotIGstAmt As Decimal = 0

        Sno = 0
        TotQty = 0
        TotSubAmt = 0
        TotDiscAmt = 0
        TotTxAmt = 0
        TotAmt = 0
        TotAssAmt = 0

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                Sno = Sno + 1
                dgv_Details.Rows(i).Cells(0).Value = Sno

                TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(4).Value)
                TotSubAmt = TotSubAmt + Val(dgv_Details.Rows(i).Cells(7).Value)
                TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(9).Value) + (Val(dgv_Details.Rows(i).Cells(11).Value) * Val(dgv_Details.Rows(i).Cells(4).Value))
                TotDisAmtItm = TotDisAmtItm + Val(dgv_Details.Rows(i).Cells(11).Value)
                TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(13).Value)
                TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(14).Value) '- Val(dgv_Details.Rows(i).Cells(9).Value)

                TotAssAmt = TotAssAmt + Val(dgv_Details.Rows(i).Cells(27).Value)
            Next
        End With


        With dgv_Details_Total
            If dgv_Details_Total.RowCount <= 0 Then dgv_Details_Total.Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotQty)
            .Rows(0).Cells(7).Value = Format(Val(TotSubAmt), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(TotDiscAmt), "########0.00")
            .Rows(0).Cells(11).Value = Format(Val(TotDisAmtItm), "########0.00")
            .Rows(0).Cells(13).Value = Format(Val(TotTxAmt), "########0.00")
            .Rows(0).Cells(14).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(27).Value = Format(Val(TotAssAmt), "########0.00")
        End With

        txt_TotalGrossAmount.Text = Format(Val(TotAmt), "########0.00")
        txt_CashDiscAmount.Text = Format(Val(TotDisAmtItm), "########0.00")

        Get_HSN_CodeWise_GSTTax_Details()

        TotAssval = 0
        TotCGstAmt = 0
        TotSGstAmt = 0
        TotIGstAmt = 0
        With dgv_GSTTax_Details_Total
            If .RowCount > 0 Then
                TotAssval = Val(.Rows(0).Cells(2).Value)
                TotCGstAmt = Val(.Rows(0).Cells(4).Value)
                TotSGstAmt = Val(.Rows(0).Cells(6).Value)
                TotIGstAmt = Val(.Rows(0).Cells(8).Value)
            End If
        End With

        lbl_Assessable.Text = Format(TotAssval, "########0.00")
        lbl_CGstAmount.Text = Format(TotCGstAmt, "########0.00")
        lbl_SGstAmount.Text = Format(TotSGstAmt, "########0.00")
        lbl_IGstAmount.Text = Format(TotIGstAmt, "########0.00")


        NetAmount_Calculation()
    End Sub
    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        If FrmLdSTS = True Then Exit Sub

        txt_CashDiscAmount.Text = Format(Val(txt_TotalGrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

        '  txt_Aessableamount.Text = Format(Val(txt_TotalGrossAmount.Text) - Val(txt_CashDiscAmount.Text), "########0.00")


        NtAmt = Val(txt_TotalGrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLessAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text)


        txt_NetAmount.Text = Format(Val(NtAmt), "#########0")


        txt_RoundOff.Text = Format(Val(CSng(txt_NetAmount.Text)) - Val(NtAmt), "#########0.00")


        lbl_BalanceAmount.Text = Format(Val(NtAmt) - Val(txt_ReceivedAmount.Text), "###########0.00")



        lbl_AmountInWords.Text = "Amount In Words : "
        If Val(txt_NetAmount.Text) <> 0 Then
            lbl_AmountInWords.Text = "Amount In Words : " & Common_Procedures.Rupees_Converstion(Val(txt_NetAmount.Text))
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sales_Code = '" & Trim(NewCode) & "'", con)
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
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "12")

        If Trim(prn_InpOpts) = "" Then Exit Sub

        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")



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
        Dim I As Integer, K As Integer
        Dim ItmNm1 As String, ItmNm2 As String
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0 '1
        DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_Count = 0


        Erase prn_DetAr

        prn_DetAr = New String(200, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then

                    prn_DetMxIndx = 0
                    For I = 0 To prn_DetDt.Rows.Count - 1

                        ItmNm1 = Trim(prn_DetDt.Rows(I).Item("Item_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For K = 30 To 1 Step -1
                                If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                            Next K
                            If K = 0 Then K = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K - 1)
                        End If

                        prn_DetMxIndx = prn_DetMxIndx + 1
                        prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(I) + 1)
                        prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm1)
                        prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt.Rows(I).Item("Noof_Items").ToString)
                        prn_DetAr(prn_DetMxIndx, 4) = prn_DetDt.Rows(I).Item("Unit_Name").ToString
                        prn_DetAr(prn_DetMxIndx, 5) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Rate").ToString), "########0.00"))
                        prn_DetAr(prn_DetMxIndx, 6) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Amount").ToString), "########0.00"))
                        prn_DetAr(prn_DetMxIndx, 7) = ""

                        If Trim(ItmNm2) <> "" Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = ""
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm2)
                            prn_DetAr(prn_DetMxIndx, 3) = ""
                            prn_DetAr(prn_DetMxIndx, 4) = ""
                            prn_DetAr(prn_DetMxIndx, 5) = ""
                            prn_DetAr(prn_DetMxIndx, 6) = ""
                            prn_DetAr(prn_DetMxIndx, 7) = "ITEM_2ND_LINE"
                        End If

                        If Trim(prn_DetDt.Rows(I).Item("Batch_Serial_No").ToString) <> "" Then

                            Erase BlNoAr
                            BlNoAr = New String(20) {}

                            m1 = 0
                            bln = "S/No : " & Trim(prn_DetDt.Rows(I).Item("Batch_Serial_No").ToString)

LOOP1:
                            If Len(bln) > 47 Then
                                For K = 47 To 1 Step -1
                                    If Mid$(bln, K, 1) = " " Or Mid$(bln, K, 1) = "," Or Mid$(bln, K, 1) = "/" Or Mid$(bln, K, 1) = "\" Or Mid$(bln, K, 1) = "-" Or Mid$(bln, K, 1) = "." Or Mid$(bln, K, 1) = "&" Or Mid$(bln, K, 1) = "_" Then Exit For
                                Next K
                                If K = 0 Then K = 47
                                m1 = m1 + 1
                                BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), K)
                                'BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), K - 1)
                                bln = Microsoft.VisualBasic.Right(bln, Len(bln) - K)
                                If Len(bln) <= 47 Then
                                    m1 = m1 + 1
                                    BlNoAr(m1) = bln
                                Else
                                    GoTo LOOP1
                                End If

                            Else
                                m1 = m1 + 1
                                BlNoAr(m1) = bln

                            End If

                            For K = 1 To m1
                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_DetAr(prn_DetMxIndx, 1) = ""
                                prn_DetAr(prn_DetMxIndx, 2) = Trim(BlNoAr(K))
                                prn_DetAr(prn_DetMxIndx, 3) = ""
                                prn_DetAr(prn_DetMxIndx, 4) = ""
                                prn_DetAr(prn_DetMxIndx, 5) = ""
                                prn_DetAr(prn_DetMxIndx, 6) = ""
                                prn_DetAr(prn_DetMxIndx, 7) = "SERIALNO"
                            Next K

                        End If

                    Next I

                End If

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            da2.Dispose()

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format1(e)

    End Sub


    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 50
            .Top = 40 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
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

        TxtHgt = 18.5  ' 21 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 22  '20 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45
        ClArr(2) = 305 : ClArr(3) = 90 : ClArr(4) = 60 : ClArr(5) = 90
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        'ClArr(0) = 0
        'ClArr(1) = 55
        'ClArr(2) = 290 : ClArr(3) = 100 : ClArr(4) = 70 : ClArr(5) = 95
        'ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetMxIndx > 0 Then

                        Do While DetIndx <= prn_DetMxIndx

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt - 5

                            If DetIndx <> 1 And Val(prn_DetAr(DetIndx, 1)) <> 0 Then
                                CurY = CurY + 2
                            End If

                            If Trim(prn_DetAr(DetIndx, 2)) <> "" And Trim(prn_DetAr(DetIndx, 7)) = "SERIALNO" Then
                                CurY = CurY + 3
                                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 15, CurY, 0, 0, p1Font)

                            ElseIf Trim(prn_DetAr(DetIndx, 2)) <> "" And Trim(prn_DetAr(DetIndx, 7)) = "ITEM_2ND_LINE" Then
                                CurY = CurY + 3
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 15, CurY, 0, 0, pFont)

                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            End If

                            NoofDets = NoofDets + 1

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 1
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
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
    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim PnAr() As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim strWidth As String
        Dim CurX As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                    PrintDocument1.DefaultPageSettings.Color = True
                    PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
                    e.PageSettings.Color = True

                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                End If

            End If
        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        p1Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        'CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)
        If Val(prn_HdDt.Rows(0).Item("Ro_Division_Status").ToString) = 1 Then
            Cmp_Name = Trim(Cmp_Name) & " (RO Division)"
        End If

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) '& IIf(Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)

        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " , " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        End If

        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) '& IIf(Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & "," & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If
        Else
            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        End If

        If Val(prn_HdDt.Rows(0).Item("Ro_Division_Status").ToString) = 1 Then
            Cmp_PhNo = "PHONE : 99426 17009"
        Else
            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
            End If
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1011" Then '---- Chellam Batteries (Thekkalur)
            If Trim(UCase(prn_OriDupTri)) = "ORIGINAL" Then
                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.company_logo1, Drawing.Image), LMargin + 20, CurY, 75, 75)
            Else
                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.company_logo3, Drawing.Image), LMargin + 20, CurY, 75, 75)
            End If

            If Val(prn_HdDt.Rows(0).Item("Ro_Division_Status").ToString) = 0 Then
                If Trim(UCase(prn_OriDupTri)) = "ORIGINAL" Then
                    e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.company_logo2, Drawing.Image), PageWidth - 20 - 145, CurY + 10, 160, 58)
                Else
                    e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.company_logo4, Drawing.Image), PageWidth - 20 - 145, CurY + 10, 160, 58)
                End If

                'e.Graphics.DrawImage(DirectCast(Resources.GetObject("tsoft1.ico"), Drawing.Bitmap), 95, e.PageBounds.Height - 87)
                'CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)

                Common_Procedures.Print_To_PrintDocument(e, "CHELLAM amco", PageWidth - 10, CurY + 5, 1, 0, pFont)
            End If

        End If

        'If Trim(Cmp_Desc) <> "" Then
        '    CurY = CurY + strHeight
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Desc, LMargin, CurY, 2, PrintWidth, pFont)

        '    CurY = CurY + TxtHgt

        'Else

        CurY = CurY + strHeight

        'End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        strWidth = e.Graphics.MeasureString(Cmp_PhNo & "      " & Cmp_Email, pFont).Width

        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, CurX, CurY, 0, PrintWidth, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, CurX, CurY, 0, PrintWidth, p1Font)

        End If

        strWidth = e.Graphics.MeasureString(Cmp_PhNo, pFont).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "          " & Cmp_Email, CurX, CurY, 0, PrintWidth, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "      " & Cmp_Email, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                If UBound(PnAr) >= 5 Then Led_TinNo = Trim(PnAr(5))

            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                Led_PhNo = Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)


                'Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                'Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                'Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
                'Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)

            End If

            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            'If Trim(Led_Add4) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_Add4
            'End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = "Phone No : " & Led_PhNo
            End If

            If Trim(Led_TinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = "Tin No : " & Led_TinNo
            End If



            Cen1 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("INVOICE DATE:", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, CurY + 10, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 10, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, CurY + 10, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            ' e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY + 25, PageWidth, CurY + 25)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Dc No.", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, CurY + 15, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Dc Date", LMargin + Cen1 + 10, CurY + 25, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 25, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + Cen1 + W1 + 30, CurY + 25, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
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
        Dim BInc As Integer
        Dim BnkDetAr() As String
        'Dim Cmp_Desc As String
        Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt - 3
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))


            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                End If

            End If

            CurY = CurY - 10


            If Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Discount Amount (-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_TaxAmount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Tax Amount (+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_TaxAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "VAT @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt + 5
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then

                CurY = CurY + TxtHgt + 5
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AddLess_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AddLess_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                End If
            End If

            CurY = CurY + TxtHgt + 5

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
                'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + TxtHgt + 10
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

            CurY = CurY + TxtHgt - 12
            Common_Procedures.Print_To_PrintDocument(e, "Rupees : " & Rup1, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 10

            'Cmp_Desc = ""
            'If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            '    Cmp_Desc = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
            'End If

            'If Val(prn_HdDt.Rows(0).Item("Ro_Division_Status").ToString) = 1 Then
            '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "COMPLETE WATER TREATMENT", LMargin + 10, CurY, 0, 0, p1Font)

            'Else
            '    p1Font = New Font("Calibri", 14, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "Authorised Distributor for", LMargin + 10, CurY, 0, 0, pFont)
            '    w1 = e.Graphics.MeasureString("Authirised Distributor  for", pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "amco", LMargin + w1 + 10, CurY - 3, 0, 0, p1Font)
            '    w2 = e.Graphics.MeasureString("amco", p1Font).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "Batteries", LMargin + w1 + w2 + 10, CurY, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_Desc, LMargin + 10, CurY, 0, 0, p1Font)
            'End If

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            Jurs = Common_Procedures.settings.Jurisdiction
            If Trim(Jurs) = "" Then Jurs = "Tirupur"

            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Jurs & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            End If

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
        Dim Led_IdNo As Integer, Itm_IdNo As Integer, Pur_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0
            Pur_IdNo = 0
            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_Purchase.Text) <> "" Then
                Pur_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_Purchase.Text)
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Code IN (select z.Sales_Code from Sales_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If
            If Val(Pur_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.PurchaseAc_IdNo = " & Str(Val(Pur_IdNo))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    '  dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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


    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            With dgv_Details
                If .Visible Then

                    If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Then
                        '    .Rows(e.RowIndex).Cells(7).Value = Format(Val(.Rows(e.RowIndex).Cells(4).Value) * Val(.Rows(e.RowIndex).Cells(5).Value), "#########0.00")
                        Total_Calculation()

                    End If
                End If

            End With
        Catch ex As Exception

        End Try
        
    End Sub





    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(2).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            txt_Code.Text = (dgv_Details.CurrentRow.Cells(1).Value)
            cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            lbl_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
            txt_NoofItems.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
            txt_RateWith_Tax.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
            txt_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
            txt_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
            txt_DiscAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
            txt_DisPerc_Item.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
            txt_DiscountAmountItem.Text = Format(Val(dgv_Details.CurrentRow.Cells(11).Value), "########0.00")
            txt_TaxPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
            txt_TaxAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")
            txt_GrossAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(14).Value), "########0.00")
            lbl_Mrp_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(15).Value), "#########0.00")
            lbl_Sales_Price.Text = Format(Val(dgv_Details.CurrentRow.Cells(16).Value), "#########0.00")
            lbl_Batch_No.Text = (dgv_Details.CurrentRow.Cells(17).Value)
            lbl_Manufacture_Day.Text = Val(dgv_Details.CurrentRow.Cells(18).Value)
            lbl_Manufacture_Month.Text = (dgv_Details.CurrentRow.Cells(19).Value)
            lbl_Manufacture_Year.Text = Val(dgv_Details.CurrentRow.Cells(20).Value)
            txt_Mfg_Date.Text = Val(dgv_Details.CurrentRow.Cells(21).Value)
            lbl_Expiray_Period_Days.Text = Val(dgv_Details.CurrentRow.Cells(22).Value)
            lbl_Expiray_Day.Text = Val(dgv_Details.CurrentRow.Cells(23).Value)
            lbl_ExpiryMonth.Text = (dgv_Details.CurrentRow.Cells(24).Value)
            lbl_Expiry_Year.Text = Val(dgv_Details.CurrentRow.Cells(25).Value)
            txt_Exp_date.Text = (dgv_Details.CurrentRow.Cells(26).Value)

            lbl_Grid_AssessableValue.Text = Format((dgv_Details.CurrentRow.Cells(27).Value), "############0.00")
            lbl_Grid_HsnCode.Text = (dgv_Details.CurrentRow.Cells(28).Value)
            lbl_Grid_GstPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(29).Value), "###########0.00")



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
        Total_Calculation()
        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        txt_Code.Text = ""
        cbo_ItemName.Text = ""
        lbl_Unit.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_RateWith_Tax.Text = ""
        txt_Amount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscAmount.Text = ""
        txt_DisPerc_Item.Text = ""
        txt_DiscountAmountItem.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_GrossAmount.Text = ""
        lbl_Mrp_Rate.Text = ""
        lbl_Sales_Price.Text = ""
        lbl_Batch_No.Text = ""
        lbl_Manufacture_Day.Text = ""
        lbl_Manufacture_Month.Text = ""
        lbl_Manufacture_Year.Text = ""
        txt_Mfg_Date.Text = ""
        lbl_Expiray_Period_Days.Text = ""
        lbl_Expiray_Day.Text = ""
        lbl_ExpiryMonth.Text = ""
        lbl_Expiry_Year.Text = ""
        txt_Exp_date.Text = ""


        If txt_Code.Visible = True Then
            If txt_Code.Enabled And txt_Code.Visible Then txt_Code.Focus()
        Else
            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
        End If


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
            Total_Calculation()
            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            txt_Code.Text = ""
            cbo_ItemName.Text = ""
            lbl_Unit.Text = ""
            txt_NoofItems.Text = ""
            txt_Rate.Text = ""
            txt_RateWith_Tax.Text = ""
            txt_Amount.Text = ""
            txt_DiscPerc.Text = ""
            txt_DiscAmount.Text = ""
            txt_DisPerc_Item.Text = ""
            txt_DiscountAmountItem.Text = ""
            txt_TaxPerc.Text = ""
            txt_TaxAmount.Text = ""
            txt_GrossAmount.Text = ""
            lbl_Mrp_Rate.Text = ""
            lbl_Sales_Price.Text = ""
            lbl_Batch_No.Text = ""
            lbl_Manufacture_Day.Text = ""
            lbl_Manufacture_Month.Text = ""
            lbl_Manufacture_Year.Text = ""
            txt_Mfg_Date.Text = ""
            lbl_Expiray_Period_Days.Text = ""
            lbl_Expiray_Day.Text = ""
            lbl_ExpiryMonth.Text = ""
            lbl_Expiry_Year.Text = ""
            txt_Exp_date.Text = ""

            If txt_Code.Visible = True Then
                If txt_Code.Enabled And txt_Code.Visible Then txt_Code.Focus()
            Else
                If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
            End If


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

    Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.LostFocus
        Try

            If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
                cbo_TaxType.Tag = cbo_TaxType.Text
                Amount_Calculation(True)
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
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
            ItemDiscAmt = Val(dgv_Details.Rows(i).Cells(10).Value)
            TxPerc = Val(dgv_Details.Rows(i).Cells(11).Value)

            TxAmt = 0
            If Trim(cbo_TaxType.Text) <> "" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
                TxAmt = Format(((Val(SubAmt) - Val(ItemDiscAmt) - Val(DiscAmt)) * Val(TxPerc) / 100), "#########0.00")
            End If

            TotAmt = Val(SubAmt) - Val(DiscAmt) + Val(TxAmt)

            dgv_Details.Rows(i).Cells(12).Value = Trim(Format(Val(TxAmt), "#########0.00"))
            dgv_Details.Rows(i).Cells(13).Value = Trim(Format(Val(TotAmt), "#########0.00"))

        Next

        GrossAmount_Calculation()

    End Sub


    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_PaymentMethod, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")


    End Sub



    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                cbo_Ledger.Tag = cbo_Ledger.Text
                Amount_Calculation(True)
            End If
        End If


      

    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_Ledger, txt_SlNo, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_SlNo, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
                cbo_TaxType.Tag = cbo_TaxType.Text
                Amount_Calculation(True)
            End If
        End If
      

    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, Nothing, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_Code.Visible = True Then
                txt_Code.Focus()
            Else
                cbo_TaxType.Focus()
            End If

        End If

        If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If Trim(cbo_ItemName.Text) <> "" Then
                If MessageBox.Show("Do you want to select Quotation?", "FOR QUOTATION SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_NoofItems.Focus()

                End If

            Else
                txt_TotalDiscAmount.Focus()
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

            If Trim(UCase(cbo_ItemName.Text)) <> "" Then
                If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
                    cmbItmNm = cbo_ItemName.Text
                    get_Item_Details()
                End If
            End If
            If Trim(cbo_ItemName.Text) <> "" Then
                If MessageBox.Show("Do you want to select Batch Details?", "FOR BATCH SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_NoofItems.Focus()

                End If

            Else
                txt_TotalDiscAmount.Focus()
            End If
        End If

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
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, cbo_Filter_Purchase, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, cbo_Filter_Purchase, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub


    Private Sub lbl_TotalTaxAmount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_TotalTaxAmount.Click
        Dim VtAmt As String = ""

        VtAmt = InputBox("Enter vat Amount :", "FOR VAT AMOUNT ALTERATION....", Val(lbl_TotalTaxAmount.Text))

        If Trim(VtAmt) <> "" Then
            If Val(VtAmt) <> 0 Then
                lbl_TotalTaxAmount.Text = Format(Val(VtAmt), "#########0.00")

                txt_TotalGrossAmount.Text = Format(Val(txt_Aessableamount.Text) - Val(txt_TotalDiscAmount.Text) + Val(lbl_TotalTaxAmount.Text), "########0.00")

                NetAmount_Calculation()
            End If
        End If

        If txt_CashDiscPerc.Visible And txt_CashDiscPerc.Enabled Then txt_CashDiscPerc.Focus()

    End Sub


    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub



    Private Sub txt_SubAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GrossAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_SubAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GrossAmount.LostFocus
        If Val(txt_Rate.Text) = 0 Then
            If Val(txt_NoofItems.Text) <> 0 Then
                txt_Rate.Text = Val(txt_GrossAmount.Text) / Val(txt_NoofItems.Text)
            End If
        End If
    End Sub




    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_Tax_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Tax_Details.Name Then
                dgv1 = dgv_Tax_Details

            ElseIf dgv_Tax_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Tax_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Tax_Details.Name Then
                dgv1 = dgv_Tax_Details


            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_Tax_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                TaxClose_Selection()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                TaxClose_Selection()
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                TaxClose_Selection()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If



                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If





                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function
    Private Sub dgv_Tax_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Tax_Details.CellEndEdit
        dgv_Tax_Details_CellLeave(sender, e)
        Total_TaxCalculation()
    End Sub

    Private Sub dgv_Tax_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Tax_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable

        ' Dim Rect As Rectangle

        With dgv_Tax_Details
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



        End With

    End Sub

    Private Sub dgv_Tax_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Tax_Details.CellLeave
        With dgv_Tax_Details

            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

        End With
    End Sub

    Private Sub dgv_Tax_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Tax_Details.CellValueChanged
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        With dgv_Tax_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 5 Then


                    Total_TaxCalculation()

                End If

            End If
        End With
    End Sub
    Private Sub dgv_Tax_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Tax_Details.EditingControlShowing
        dgtxt_TaxDetails = Nothing

        dgtxt_TaxDetails = CType(dgv_Tax_Details.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgtxt_TaxDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_TaxDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Tax_Details.Name
        dgv_Tax_Details.EditingControl.BackColor = Color.Lime
        dgv_Tax_Details.EditingControl.ForeColor = Color.Blue
        dgv_Tax_Details.SelectAll()
    End Sub

    Private Sub dgtxt_TaxDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_TaxDetails.KeyDown
        With dgv_Tax_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then

                    'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Or Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                    '        e.Handled = True
                    '        e.SuppressKeyPress = True
                    '    End If
                    'End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_TaxDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_TaxDetails.KeyPress

        With dgv_Tax_Details
            If .Visible Then

                ' If .CurrentCell.ColumnIndex = 3 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

                ' If

            End If
        End With

    End Sub
    Private Sub dgtxt_TaxDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_TaxDetails.TextChanged
        Try
            With dgv_Tax_Details

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_TaxDetails.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dgtxt_TaxDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_TaxDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Tax_Details_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_Tax_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Tax_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Tax_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Tax_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Tax_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                Total_TaxCalculation()

            End With

        End If

    End Sub

    Private Sub dgv_Tax_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Tax_Details.LostFocus
        On Error Resume Next
        dgv_Tax_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Tax_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Tax_Details.RowsAdded
        Dim n As Integer

        With dgv_Tax_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub
    Private Sub Total_TaxCalculation()
        Dim Sno As Integer
        Dim TotGrsAmt As Single
        Dim TotDisAmt As Single
        Dim TotAssAmt As Single
        Dim TotTaxAmt As Single
        If NoCalc_Status = True Then Exit Sub

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotGrsAmt = 0 : TotDisAmt = 0 : TotAssAmt = 0 : TotTaxAmt = 0

        With dgv_Tax_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    ' TotGdStQty = TotGdStQty + Val(.Rows(i).Cells(2).Value())
                    TotGrsAmt = TotGrsAmt + Val(.Rows(i).Cells(1).Value())
                    TotDisAmt = TotDisAmt + Val(.Rows(i).Cells(2).Value())
                    TotAssAmt = TotAssAmt + Val(.Rows(i).Cells(3).Value())
                    TotTaxAmt = TotTaxAmt + Val(.Rows(i).Cells(5).Value())

                End If

            Next i

        End With


        With dgv_Tax_Total_Details
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Format(Val(TotGrsAmt), "#########0.00")
            .Rows(0).Cells(2).Value = Format(Val(TotDisAmt), "############0.00")
            .Rows(0).Cells(3).Value = Format(Val(TotAssAmt), "##############0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotTaxAmt), "#############0.00")
        End With

    End Sub


    Private Sub btn_Tax_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Tax_Details.Click
        Dim n As Integer
        ' Dim disAmt As Integer
        dgv_Tax_Details.Rows.Clear()
        With dgv_Details
            If dgv_Details.Rows.Count > 0 Then
                For i = 0 To .RowCount - 1
                    n = dgv_Tax_Details.Rows.Add()

                    dgv_Tax_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Tax_Details.Rows(n).Cells(1).Value = .Rows(i).Cells(7).Value
                    dgv_Tax_Details.Rows(n).Cells(2).Value = .Rows(i).Cells(9).Value
                    dgv_Tax_Details.Rows(n).Cells(3).Value = Format(Val(.Rows(i).Cells(7).Value) - Val(.Rows(i).Cells(9).Value), "###########0.00")
                    dgv_Tax_Details.Rows(n).Cells(4).Value = .Rows(i).Cells(12).Value
                    dgv_Tax_Details.Rows(n).Cells(5).Value = .Rows(i).Cells(13).Value
                Next
            End If

        End With


        pnl_Back.Enabled = False
        pnl_Tax.Visible = True
        If dgv_Tax_Details.RowCount > 0 Then
            dgv_Tax_Details.Focus()
            dgv_Tax_Details.CurrentCell = dgv_Tax_Details.Rows(0).Cells(1)
        End If
    End Sub



    Private Sub txt_Batch_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_Batch_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_AlphaNumericOnlyWithSlash(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub


    Private Sub TaxClose_Selection()
        'Dim vBatch_Nos As String = ""
        'Dim Batch_No As String = ""

        'For i = 0 To dgv_Batch_Details.RowCount - 1

        '    If Trim(dgv_Batch_Details.Rows(i).Cells(1).Value) <> "" Then
        '        vBatch_Nos = Trim(vBatch_Nos) & IIf(Trim(vBatch_Nos) <> "", ", ", "") & Trim(dgv_Batch_Details.Rows(i).Cells(1).Value)
        '    End If
        '    Batch_No = Trim(vBatch_Nos)
        '    txt_Batch_No.Text = (Batch_No)
        'Next


        pnl_Tax.Visible = False
        pnl_Back.Enabled = True
        If txt_Freight.Visible And txt_Freight.Enabled Then txt_Freight.Focus()
    End Sub


    Private Sub txt_Code_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Code.GotFocus
        With txt_Code
            txtItmCd = txt_Code.Text

        End With

    End Sub


    Private Sub txt_Code_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Code.KeyDown
        If e.KeyCode = 40 Then
            If Trim(txt_Code.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_TotalDiscAmount.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Code_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Code.KeyPress
        If Asc(e.KeyChar) = 13 Then


            If Trim(txt_Code.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_TotalDiscAmount.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Tax_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Tax_Close.Click
        TaxClose_Selection()
    End Sub

    Private Sub cbo_Filter_Purchase_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Purchase.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Purchase_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Purchase.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Purchase, cbo_Filter_ItemName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Purchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Purchase.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Purchase, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub ExpirayDate_Calculation()
        Dim cmd As New SqlClient.SqlCommand
        Dim dte As Date
        Dim Man_dte As DateTime
        Dim Man_Mnth_id As Integer = 0
        Dim Exp_Day As Integer = 0
        Dim Exp_mnth As Integer = 0


        Man_Mnth_id = Common_Procedures.Month_ShortNameToIdNo(con, lbl_Manufacture_Month.Text)
        If Val(lbl_Manufacture_Day.Text) <> 0 And Val(Man_Mnth_id) <> 0 And Val(lbl_Manufacture_Year.Text) <> 0 Then
            Man_dte = Val(lbl_Manufacture_Day.Text) & "/" & Val(Man_Mnth_id) & "/" & Val(lbl_Manufacture_Year.Text)

            Exp_Day = Val(lbl_Expiray_Period_Days.Text)
            dte = DateAdd(DateInterval.Day, Exp_Day, Man_dte)
            lbl_Expiray_Day.Text = dte.Day.ToString
            Exp_mnth = dte.Month.ToString
            lbl_ExpiryMonth.Text = Common_Procedures.Month_IdNoToShortName(con, Exp_mnth)
            lbl_Expiry_Year.Text = dte.Year.ToString
        End If
    End Sub



    Private Sub txt_Code_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Code.LostFocus
        'Dim da As SqlClient.SqlDataAdapter
        'Dim dt As New DataTable

        'If Trim(UCase(txtItmCd)) <> Trim(UCase(txt_Code.Text)) Then
        '    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a, unit_head b where a.item_Code = '" & Trim(txt_Code.Text) & "' and a.unit_idno = b.unit_idno", con)
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then
        '        cbo_ItemName.Text = dt.Rows(0)("Item_name").ToString
        '        lbl_Unit.Text = dt.Rows(0)("unit_name").ToString
        '        txt_Rate.Text = dt.Rows(0)("Cost_Rate").ToString
        '        'txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
        '        lbl_Sales_Price.Text = dt.Rows(0)("Sales_Rate").ToString
        '        txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
        '        lbl_Mrp_Rate.Text = dt.Rows(0)("MRP_Rate").ToString
        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If

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





    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        ' Dim LedIdNo As Integer
        Dim Itm_Id As Integer = 0
        Dim NewCode As String
        Dim Ent_Qty As Single, Ent_Rate As Single, Ent_PurcRet_Qty As Single
        Dim Ent_DetSlNo As Long



        Itm_Id = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

        If Itm_Id = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*,b.*,c.Unit_Name ,f.Noof_Items as Ent_Sales_Quantity from Item_Stock_Selection_Processing_Details a   INNER JOIN Item_Head b ON a.Item_idno = b.Item_idno INNER JOIN unit_Head c ON b.Unit_idno = c.Unit_idno LEFT OUTER JOIN Sales_Details F ON f.Sales_Code = '" & Trim(NewCode) & "'  and a.Item_Idno = f.Item_Idno and a.Batch_No = f.Batch_Serial_No and a.Manufactured_Date = f.Manufacture_Date and a.Expiry_Date = f.Expiry_Date and a.Mrp_Rate = f.Mrp_Rate Where a.Item_idno = " & Str(Val(Itm_Id)) & " and ( (a.Inward_Quantity  - a.Outward_Quantity ) > 0  or f.Noof_Items > 0 ) Order by  a.Item_idno", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Ent_Qty = 0 : Ent_Rate = 0 : Ent_DetSlNo = 0 : Ent_PurcRet_Qty = 0

                    'If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_SlNo").ToString) = False Then Ent_DetSlNo = Val(Dt1.Rows(i).Item("Ent_Sales_SlNo").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_Quantity").ToString) = False Then Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Sales_Quantity").ToString)



                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = (Val(Dt1.Rows(i).Item("Inward_Quantity").ToString) - Val(Dt1.Rows(i).Item("Outward_Quantity").ToString) + Ent_Qty)
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Mrp_Rate").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Sales_Rate").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Batch_No").ToString
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Manufactured_Day").ToString)
                    .Rows(n).Cells(6).Value = Common_Procedures.Month_IdNoToShortName(con, Val(Dt1.Rows(i).Item("Manufactured_Month_IdNo").ToString))
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Manufactured_Year").ToString)

                    If Val(Ent_Qty) > 0 Then
                        .Rows(n).Cells(8).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(8).Value = ""
                    End If
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Expiry_Period_Days").ToString
                    .Rows(n).Cells(10).Value = Val(Dt1.Rows(i).Item("Expiry_Day").ToString)
                    .Rows(n).Cells(11).Value = Common_Procedures.Month_IdNoToShortName(con, Val(Dt1.Rows(i).Item("Expiry_Month_IdNo").ToString))
                    .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("Expiry_Year").ToString)
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Unit_name").ToString
                    .Rows(n).Cells(14).Value = Format(Val(Dt1.Rows(i).Item("Purchase_Rate").ToString), "#############0.00")
                    .Rows(n).Cells(15).Value = Format(Val(Dt1.Rows(i).Item("Tax_Percentage").ToString), "#############0.00")
                    '.Rows(n).Cells(13).Value = Val(Ent_Rate)
                    '.Rows(n).Cells(14).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sales_Quotation_Date").ToString), "dd-MM-yyyy")
                    ''.Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Order_No").ToString
                    '.Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Order_Date").ToString
                    '.Rows(n).Cells(17).Value = Common_Procedures.Transport_IdNoToName(con, Val(Dt1.Rows(i).Item("Transport_IdNo").ToString))
                    '.Rows(n).Cells(18).Value = (Dt1.Rows(i).Item("Vehicle_No").ToString)



                Next

            End If
            Dt1.Clear()

            'If .Rows.Count = 0 Then
            '    n = .Rows.Add()
            '    .Rows(n).Cells(0).Value = "1"
            'End If

        End With

        pnl_Selection.Visible = True
        pnl_Selection.BringToFront()
        pnl_Back.Enabled = False

        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Grid_Selection(e.RowIndex)
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection


            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then
                    Put_SeclectionValue_ToEntry()
                    pnl_Back.Enabled = True
                    pnl_Selection.Visible = False
                    If txt_NoofItems.Visible And txt_NoofItems.Enabled Then txt_NoofItems.Focus()
                End If



                If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(8).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
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
        ' dgv_Details.Rows.Clear()

        Put_SeclectionValue_ToEntry()

    End Sub

    Private Sub Put_SeclectionValue_ToEntry()
        Dim i As Integer, n As Integer
        Dim sno As Integer
        '  Dim Ent_Qty As Single, Ent_Rate As Single
        Dim vQT_Nos As String = ""
        Dim Quot_No As String = ""



        NoCalc_Status = True

        sno = 0
        vQT_Nos = ""

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1

                txt_NoofItems.Text = dgv_Selection.Rows(i).Cells(1).Value
                lbl_Mrp_Rate.Text = dgv_Selection.Rows(i).Cells(2).Value
                lbl_Sales_Price.Text = dgv_Selection.Rows(i).Cells(3).Value
                lbl_Batch_No.Text = dgv_Selection.Rows(i).Cells(4).Value
                lbl_Manufacture_Day.Text = dgv_Selection.Rows(i).Cells(5).Value
                lbl_Manufacture_Month.Text = dgv_Selection.Rows(i).Cells(6).Value
                lbl_Manufacture_Year.Text = dgv_Selection.Rows(i).Cells(7).Value
                lbl_Expiray_Period_Days.Text = dgv_Selection.Rows(i).Cells(9).Value
                lbl_Expiray_Day.Text = dgv_Selection.Rows(i).Cells(10).Value
                lbl_ExpiryMonth.Text = dgv_Selection.Rows(i).Cells(11).Value
                lbl_Expiry_Year.Text = dgv_Selection.Rows(i).Cells(12).Value
                lbl_Unit.Text = dgv_Selection.Rows(i).Cells(13).Value
                txt_TaxPerc.Text = dgv_Selection.Rows(i).Cells(15).Value
                txt_Rate.Text = dgv_Selection.Rows(i).Cells(3).Value
                'dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                'dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                'dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                'dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                'dgv_Details.Rows(n).Cells(4).Value = Val(Ent_Qty)
                'dgv_Details.Rows(n).Cells(5).Value = Val(Ent_Rate)
                'dgv_Details.Rows(n).Cells(6).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                'dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                'dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value

                'txt_Dcno.Text = (Quot_No)
                'txt_DcDate.Text = dgv_Selection.Rows(i).Cells(14).Value

                '   dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value

            End If

        Next i

        NoCalc_Status = False

        GrossAmount_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_NoofItems.Visible And txt_NoofItems.Enabled Then txt_NoofItems.Focus()

    End Sub


    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub txt_RateWith_Tax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RateWith_Tax.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub


    Private Sub txt_NetAmount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NetAmount.KeyUp
        NetAmount_Calculation()
    End Sub

    Private Sub txt_ReceivedAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ReceivedAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_Mrp_Rate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_Mrp_Rate.Click

    End Sub

    Private Sub txt_TotalDiscAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TotalDiscAmount.LostFocus
        Dim n As Integer
        dgv_Tax_Details.Rows.Clear()
        With dgv_Details
            If dgv_Details.Rows.Count > 0 Then
                For i = 0 To .RowCount - 1
                    n = dgv_Tax_Details.Rows.Add()

                    dgv_Tax_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Tax_Details.Rows(n).Cells(1).Value = .Rows(i).Cells(14).Value
                    dgv_Tax_Details.Rows(n).Cells(2).Value = .Rows(i).Cells(9).Value
                    dgv_Tax_Details.Rows(n).Cells(3).Value = Format(Val(.Rows(i).Cells(14).Value) - Val(.Rows(i).Cells(9).Value), "###########0.00")
                    dgv_Tax_Details.Rows(n).Cells(4).Value = .Rows(i).Cells(12).Value
                    dgv_Tax_Details.Rows(n).Cells(5).Value = .Rows(i).Cells(13).Value
                Next
            End If

        End With
    End Sub

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub txt_Rate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.LostFocus
        pnl_PreviousBillDetails.Visible = False
        txt_Rate.Text = Format(Val(txt_Rate.Text), "#####0.00")
    End Sub
    Private Sub txt_Rate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.GotFocus
        GetPreviousBillDetails()
    End Sub
    Private Sub GetPreviousBillDetails()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim n As Integer = 0
        Dim Led_Id As Integer = 0
        Dim Itm_Id As Integer = 0


        Led_Id = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text))
        Itm_Id = Common_Procedures.Item_NameToIdNo(con, Trim(cbo_ItemName.Text))

        If Led_Id = 0 And Itm_Id = 0 Then
            pnl_PreviousBillDetails.Visible = False
            Exit Sub
        End If

        da1 = New SqlClient.SqlDataAdapter("select top 5 Sales_No,Sales_Date,Rate  from Sales_Details a where Ledger_IdNo = " & Led_Id & " and Item_IdNo = " & Itm_Id & "Order By Sales_Date desc", con)
        da1.Fill(dt1)


        With dgv_PreviousBillDetails
            .Rows.Clear()

            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0).Item("Sales_No").ToString) = False Then

                    If Trim(dt1.Rows(0).Item("Sales_No").ToString) <> "" Then

                        For i = 0 To dt1.Rows.Count - 1
                            n = dgv_PreviousBillDetails.Rows.Add()
                            dgv_PreviousBillDetails.Rows(n).Cells(0).Value = dt1.Rows(i).Item("Sales_No").ToString
                            dgv_PreviousBillDetails.Rows(n).Cells(1).Value = FormatDateTime(Convert.ToDateTime(dt1.Rows(i).Item("Sales_Date").ToString), DateFormat.ShortDate)
                            dgv_PreviousBillDetails.Rows(n).Cells(2).Value = dt1.Rows(i).Item("Rate").ToString

                        Next


                    End If
                End If

                pnl_PreviousBillDetails.Visible = True
                pnl_PreviousBillDetails.BringToFront()

            End If


        End With

    End Sub
    Private Sub Get_HSN_CodeWise_GSTTax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim AssVal_Frgt_Othr_Charges As Double = 0
        Dim LedIdNo As Integer = 0
        Dim ItmIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Then Exit Sub

            LedIdNo = 0
            InterStateStatus = False
            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

            End If

            AssVal_Frgt_Othr_Charges = Val(txt_AddLessAmount.Text)

            cmd.Connection = con

            cmd.CommandText = "Truncate table EntryTemp"
            cmd.ExecuteNonQuery()

            With dgv_Details

                If .Rows.Count > 0 Then
                    For i = 0 To .Rows.Count - 1

                        If Trim(.Rows(i).Cells(2).Value) <> "" And Trim(.Rows(i).Cells(28).Value) <> "" And Val(.Rows(i).Cells(27).Value) <> 0 Then

                            cmd.CommandText = "Insert into EntryTemp (                    Name1                ,                   Currency1            ,                       Currency2                                      ) " & _
                                              "            Values    ( '" & Trim(.Rows(i).Cells(28).Value) & "', " & (Val(.Rows(i).Cells(29).Value)) & ", " & Str(Val(.Rows(i).Cells(27).Value) + AssVal_Frgt_Othr_Charges) & " ) "
                            cmd.ExecuteNonQuery()

                            AssVal_Frgt_Othr_Charges = 0

                        End If

                    Next
                End If
            End With

            With dgv_GSTTax_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from EntryTemp group by name1, Currency1 Having sum(Currency2) <> 0 ", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Sno
                        .Rows(n).Cells(1).Value = dt.Rows(i).Item("HSN_Code").ToString

                        .Rows(n).Cells(2).Value = Format(Val(dt.Rows(i).Item("Assessable_Value").ToString), "############0.00")
                        If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""

                        .Rows(n).Cells(3).Value = ""
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(7).Value = ""
                        If InterStateStatus = True Then
                            .Rows(n).Cells(7).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString), "######0.00")
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                        Else
                            .Rows(n).Cells(3).Value = Format(Val(dt.Rows(i)("GST_Percentage").ToString) / 2, "#########0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt.Rows(i)("GST_Percentage").ToString) / 2, "#########0.00")

                        End If

                        .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                        .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "############0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                    Next i

                End If

                dt.Clear()
                dt.Dispose()
                da.Dispose()

            End With

            Total_GSTTax_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub
    Private Sub Total_GSTTax_Calculation()
        Dim Sno As Integer
        Dim TotAss_Val As Single
        Dim TotCGST_amt As Single
        Dim TotSGST_amt As Double
        Dim TotIGST_amt As Double



        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotAss_Val = 0 : TotCGST_amt = 0 : TotSGST_amt = 0 : TotIGST_amt = 0

        With dgv_GSTTax_Details

            For i = 0 To .RowCount - 1

                Sno = Sno + 1

                .Rows(i).Cells(0).Value = Sno

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotAss_Val = TotAss_Val + Val(.Rows(i).Cells(2).Value())
                    TotCGST_amt = TotCGST_amt + Val(.Rows(i).Cells(4).Value())
                    TotSGST_amt = TotSGST_amt + Val(.Rows(i).Cells(6).Value())
                    TotIGST_amt = TotIGST_amt + Val(.Rows(i).Cells(8).Value())

                End If

            Next i

        End With


        With dgv_GSTTax_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(TotAss_Val), "########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotCGST_amt), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotSGST_amt), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotIGST_amt), "########0.00")
        End With


    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Amount_Calculation(True)
        End If
    End Sub

    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        Amount_Calculation(True)
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub btn_GSTTax_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_GSTTax_Details.Click
        pnl_GSTTax_Details.Visible = True
        pnl_Back.Enabled = False
        pnl_GSTTax_Details.Focus()
    End Sub

    Private Sub btn_Close_GSTTax_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_GSTTax_Details.Click
        pnl_Back.Enabled = True
        pnl_GSTTax_Details.Visible = False
    End Sub
End Class