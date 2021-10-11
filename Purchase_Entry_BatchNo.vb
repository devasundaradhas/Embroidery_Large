Public Class Purchase_Entry_BatchNo

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    'Private Pk_Condition As String = "PURBT-"
    'Private Pk_Condition1 As String = "PURCB-"
    Private Pk_Condition As String = "PURCS-"
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

    Private Sub clear()

       
        NoCalc_Status = True
        Batch_Status = True
        New_Entry = False
        Insert_Entry = False

        lbl_PurchaseNo.Text = ""
        lbl_PurchaseNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Tax.Visible = False
        pnl_Batch.Visible = False
        pnl_BatchSelection_ToolTip.Visible = False

        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        cbo_PurchaseAc.Text = ""
        cbo_PaymentMethod.Text = ""
        cbo_TaxType.Text = ""
        txt_SlNo.Text = ""
        txt_Code.Text = ""
        cbo_ItemName.Text = ""

        lbl_Unit.Text = ""
        txt_NoofItems.Text = ""


        txt_Rate.Text = ""
        txt_Sales_Price.Text = ""
        txt_Manufacture_Day.Text = ""
        txt_Manufacture_Year.Text = ""
        txt_Mfg_Date.Text = ""
        txt_Mrp.Text = ""
        txt_Exp_date.Text = ""
        txt_Expiray_Day.Text = ""
        txt_Expiry_Year.Text = ""
        cbo_ExpiryMonth.Text = ""
        txt_Expiray_Period_Days.Text = ""
        txt_DiscPerc.Text = ""
        cbo_Manufacture_Month.Text = ""

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
        txt_Batch_No.Text = ""
        txt_TotalDiscAmount.Text = ""

        txt_CashDiscAmount.Text = ""
        txt_CashDiscPerc.Text = ""
        txt_AddLessAmount.Text = ""
        txt_RoundOff.Text = ""
        txt_NetAmount.Text = ""

        txt_BillNo.Text = ""

        lbl_TotalTaxAmount.Text = ""

        lbl_AmountInWords.Text = "Amount In Words : "
        txt_Freight.Text = ""
        txt_Details_Slno.Text = ""

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

        cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, 21)
        cbo_PaymentMethod.Text = "CREDIT"
        cbo_TaxType.Text = "NO TAX"
        txt_SlNo.Text = "1"
        txt_Details_Slno.Text = "1"

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Tax_Details.Rows.Clear()
        dgv_Tax_Total_Details.Rows.Clear()
        dgv_Tax_Total_Details.Rows.Add()

        dgv_Batch_Selection.Rows.Clear()
        dgv_Batch_details.Rows.Clear()
        dgv_Total_Batch.Rows.Add()
        dgv_Batch_Total_details.Rows.Clear()
        dgv_Batch_Total_details.Rows.Add()


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
        dgv_Batch_Selection.CurrentCell.Selected = False
        dgv_Tax_Details.CurrentCell.Selected = False
        dgv_Tax_Total_Details.CurrentCell.Selected = False
        dgv_Batch_Total_details.CurrentCell.Selected = False
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
        Dim LockSTS As Boolean

        NoCalc_Status = True
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
                txt_Aessableamount.Text = Format(Val(dt1.Rows(0).Item("Aessable_Amount").ToString), "########0.00")
                txt_TotalDiscAmount.Text = Format(Val(dt1.Rows(0).Item("Total_DiscountAmount_item").ToString), "########0.00")
                lbl_TotalTaxAmount.Text = Format(Val(dt1.Rows(0).Item("Total_TaxAmount").ToString), "########0.00")
                txt_TotalGrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                txt_AddLessAmount.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                txt_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess_Name.Text = Trim(dt1.Rows(0).Item("AddLess_Name").ToString)
                txt_Freight_Name.Text = Trim(dt1.Rows(0).Item("Freight_Name").ToString)

                da2 = New SqlClient.SqlDataAdapter("select a.*,b.Item_Name,c.Unit_Name from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Tax_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Mrp").ToString), "########0")
                        dgv_Details.Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Sales_Price").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(16).Value = (dt2.Rows(i).Item("Batch_Serial_No").ToString)

                        If IsDate(dt2.Rows(i).Item("Manufacture_Date").ToString) = True Then

                            If DateDiff(DateInterval.Day, Convert.ToDateTime("01/01/1900"), dt2.Rows(i).Item("Manufacture_Date")) > 0 Then

                                dgv_Details.Rows(n).Cells(17).Value = Val(dt2.Rows(i).Item("Manufacture_Day").ToString)
                                dgv_Details.Rows(n).Cells(18).Value = Common_Procedures.Month_IdNoToShortName(con, Val(dt2.Rows(i).Item("Manufacture_Month_IdNo").ToString))
                                dgv_Details.Rows(n).Cells(19).Value = Val(dt2.Rows(i).Item("Manufacture_Year").ToString)
                                dgv_Details.Rows(n).Cells(20).Value = (dt2.Rows(i).Item("Manufacture_Date").ToString)

                            End If
                        End If
                        dgv_Details.Rows(n).Cells(21).Value = Val(dt2.Rows(i).Item("Expiry_Period_Days").ToString)

                        If IsDate(dt2.Rows(i).Item("Expiry_Date").ToString) = True Then

                            If DateDiff(DateInterval.Day, Convert.ToDateTime("01/01/1900"), dt2.Rows(i).Item("Expiry_Date")) > 0 Then

                                dgv_Details.Rows(n).Cells(22).Value = Val(dt2.Rows(i).Item("Expiry_Day").ToString)
                                dgv_Details.Rows(n).Cells(23).Value = Common_Procedures.Month_IdNoToShortName(con, Val(dt2.Rows(i).Item("Expiry_Month_IdNo").ToString))
                                dgv_Details.Rows(n).Cells(24).Value = Val(dt2.Rows(i).Item("Expiry_Year").ToString)
                                dgv_Details.Rows(n).Cells(25).Value = (dt2.Rows(i).Item("Expiry_Date").ToString)
                            End If
                        End If

                        dgv_Details.Rows(n).Cells(26).Value = (dt2.Rows(i).Item("Detail_SlNo").ToString)
                        '  dgv_Details.Rows(n).Cells(27).Value = (dt2.Rows(i).Item("Outward_Quantity").ToString)

                        If Val(dgv_Details.Rows(n).Cells(27).Value) <> 0 Then
                            LockSTS = True

                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(i).Cells(j).Style.BackColor = Color.LightGray
                            Next

                        End If

                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                Total_Calculation()

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Purchase_BatchNo_Details a  Where a.Purchase_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt3 = New DataTable
                da2.Fill(dt3)


                With dgv_Batch_details

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(dt3.Rows(i).Item("Detail_SlNo").ToString)
                            .Rows(n).Cells(1).Value = (dt3.Rows(i).Item("Batch_No").ToString)
                            .Rows(n).Cells(2).Value = Val(dt3.Rows(i).Item("Quantity").ToString)



                        Next i

                    End If

                End With

                NoCalc_Status = False
                Total_BatchCalculation()
                NoCalc_Status = True
                dt3.Clear()
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

                NoCalc_Status = False
                Total_BatchCalculation()
                NoCalc_Status = True

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

    Private Sub Purchase_Entry_BatchNo_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    lbl_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If




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

    Private Sub Purchase_Entry_BatchNo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        txt_GrossAmount.Enabled = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1108" Then
            txt_Code.Visible = False
            lbl_Code.Visible = False
            cbo_ItemName.Left = 41
            cbo_ItemName.Width = 288
            dgv_Details.Columns(1).Visible = False
            dgv_Details_Total.Columns(1).Visible = False
        Else
            txt_Code.Visible = True
            lbl_Code.Visible = False
            cbo_ItemName.Left = 110
            cbo_ItemName.Width = 225
            dgv_Details.Columns(1).Visible = True
            dgv_Details_Total.Columns(1).Visible = True
        End If

        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"


        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 27) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_PurchaseAc.DataSource = dt4
        cbo_PurchaseAc.DisplayMember = "Ledger_DisplayName"



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_BatchSelection_ToolTip.Visible = False
        pnl_BatchSelection_ToolTip.Left = 260
        pnl_BatchSelection_ToolTip.Top = 200

        pnl_Batch.Visible = False
        pnl_Batch.Left = (Me.Width - pnl_Batch.Width) \ 2
        pnl_Batch.Top = (Me.Height - pnl_Batch.Height) \ 2
        pnl_Batch.BringToFront()

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = (Me.Height - pnl_Tax.Height) \ 2
        pnl_Tax.BringToFront()

        pnl_PreviousBillDetails.Visible = False
        pnl_PreviousBillDetails.Left = 10
        pnl_PreviousBillDetails.Top = 400
        pnl_PreviousBillDetails.BringToFront()


        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus
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

        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Mfg_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountAmountItem.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Mrp.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Sales_Price.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Batch_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Manufacture_Day.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Manufacture_Month.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Manufacture_Year.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Expiray_Period_Days.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Expiray_Day.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ExpiryMonth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Expiry_Year.GotFocus, AddressOf ControlGotFocus


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


        '   AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        '   AddHandler btn_Delete.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Name.LostFocus, AddressOf ControlLostFocus

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
        AddHandler txt_Mrp.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Sales_Price.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Batch_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Manufacture_Day.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Manufacture_Month.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Manufacture_Year.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Expiray_Period_Days.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Expiray_Day.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ExpiryMonth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Expiry_Year.LostFocus, AddressOf ControlLostFocus



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


        ' AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        'AddHandler btn_Delete.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoofItems.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscAmount.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Mrp.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Sales_Price.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Batch_No.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Manufacture_Day.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Manufacture_Year.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Expiray_Period_Days.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Expiray_Day.KeyDown, AddressOf TextBoxControlKeyDown
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
        'AddHandler txt_Code.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoofItems.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Mrp.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Sales_Price.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Manufacture_Day.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Batch_No.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalQty.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Aessableamount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalGrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_AddLessAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RoundOff.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NetAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Manufacture_Year.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Expiray_Period_Days.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Expiray_Day.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub Purchase_Entry_BatchNo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Purchase_Entry_BatchNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Batch.Visible = True Then
                    btn_Batch_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Tax.Visible = True Then
                    btn_Tax_Close_Click(sender, e)
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


            cmd.CommandText = "Update Item_Stock_Selection_Processing_Details Set Inward_Quantity = a.Inward_Quantity - b.Noof_Items from Item_Stock_Selection_Processing_Details a, Purchase_Details b where Purchase_Code = '" & Trim(NewCode) & "' and a.Item_idNo = b.Item_IdNo and a.Batch_No =b.Batch_Serial_No and a.Manufactured_Date=b.Manufacture_DAte and a.Expiry_Date = b.Expiry_Date and a.Mrp_Rate = b.Mrp"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Purchase_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Purchase_BatchNo_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Purchase_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
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
            cmd.CommandText = "select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Purchase_No"
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

            da = New SqlClient.SqlDataAdapter("select Purchase_No from Purchase_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Purchase_No", con)
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
            cmd.CommandText = "select Purchase_No from Purchase_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Purchase_No desc"

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
        Dim da As New SqlClient.SqlDataAdapter("select Purchase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Purchase_No desc", con)
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

            lbl_PurchaseNo.Text = Common_Procedures.get_MaxCode(con, "Purchase_Head", "Purchase_Code", "For_OrderBy", "(Purchase_Code like '%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

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
        Dim Man_Mntn_id As Integer = 0
        Dim Exp_Mnth_id As Integer = 0
        Dim Man_dte As String = ""
        Dim Exp_dte As String = ""
        Dim Slno As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim Amt As Single = 0
        Dim Bat_Qty As Single = 0
        Dim TotBat_Qty As Single = 0
        Dim TempItemIDNo As Integer = 0
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
            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            da = New SqlClient.SqlDataAdapter("select * from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' and Ledger_IdNo = " & Str(Val(led_id)) & " and Bill_No = '" & Trim(txt_BillNo.Text) & "' and Purchase_Code <> '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Bill No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
                Exit Sub
            End If
            dt1.Clear()
        End If


        NoCalc_Status = False

        Total_BatchCalculation()

        Dim TotBatc_Qty As Single = 0
        If dgv_Batch_Total_details.RowCount > 0 Then
            TotBatc_Qty = Val(dgv_Batch_Total_details.Rows(0).Cells(2).Value())

        End If
        Total_Calculation()
        Dim Tot_Qty As Single = 0
        Dim Tot_SubAmt As Single = 0
        Dim Tot_DisAmt As Single = 0
        If dgv_Details_Total.RowCount > 0 Then
            Tot_Qty = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            Tot_SubAmt = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            Tot_DisAmt = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" And Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
                TotBat_Qty = 0
                For J = 0 To dgv_Batch_details.RowCount - 1
                    If Val(dgv_Batch_details.Rows(J).Cells(0).Value) = Val(dgv_Details.Rows(i).Cells(26).Value) And Val(dgv_Batch_details.Rows(J).Cells(2).Value) <> 0 Then

                        TotBat_Qty = TotBat_Qty + Val(dgv_Batch_details.Rows(J).Cells(2).Value)
                    End If
                Next
                If Val(TotBat_Qty) <> 0 Then
                    If Val(dgv_Details.Rows(i).Cells(4).Value) <> Val(TotBat_Qty) Then
                        MessageBox.Show("Mismatch Of Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                        End If
                        Exit Sub
                    End If

                End If

            End If

        Next



        Dim TotBatQty As Single = 0


     
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


            'cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate Table EntryTempSub"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into Purchase_Head(Purchase_Code, Company_IdNo, Purchase_No, for_OrderBy, Purchase_Date, Payment_Method, Ledger_IdNo, PurchaseAc_IdNo, Tax_Type,  Total_Qty,SubTotal_Amount , Aessable_Amount, Total_DiscountAmount_item, Total_TaxAmount, Gross_Amount, CashDiscount_Perc, CashDiscount_Amount, AddLess_Amount, Round_Off, Net_Amount,Bill_No , Freight_Amount,Freight_Name,AddLess_Name,Total_DiscountAmount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(purcac_id)) & ", '" & Trim(cbo_TaxType.Text) & "',  " & Str(Val(Tot_Qty)) & "," & Str(Val(Tot_SubAmt)) & ", " & Str(Val(txt_Aessableamount.Text)) & ", " & Str(Val(txt_TotalDiscAmount.Text)) & ", " & Str(Val(lbl_TotalTaxAmount.Text)) & ", " & Str(Val(txt_TotalGrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_AddLessAmount.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(txt_NetAmount.Text)) & " , '" & Trim(txt_BillNo.Text) & "' , " & Str(Val(txt_Freight.Text)) & ",'" & Trim(txt_Freight_Name.Text) & "' ,'" & Trim(txt_AddLess_Name.Text) & "'," & Val(Tot_DisAmt) & " )"

                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Purchase_Head set Purchase_Date = @PurchaseDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", PurchaseAc_IdNo = " & Str(Val(purcac_id)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "',Freight_Name = '" & Trim(txt_Freight_Name.Text) & "' ,Freight_Amount = " & Str(Val(txt_Freight.Text)) & "  ,  Total_Qty = " & Str(Val(Tot_Qty)) & ", Aessable_Amount = " & Str(Val(txt_Aessableamount.Text)) & ", Total_DiscountAmount_item = " & Str(Val(txt_TotalDiscAmount.Text)) & ", Total_TaxAmount = " & Str(Val(lbl_TotalTaxAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_TotalGrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ",AddLess_Name ='" & Trim(txt_AddLess_Name.Text) & "' , AddLess_Amount = " & Str(Val(txt_AddLessAmount.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(txt_NetAmount.Text)) & " ,Bill_No = '" & Trim(txt_BillNo.Text) & "',SubTotal_Amount  =" & Val(Tot_SubAmt) & "  , Total_DiscountAmount = " & Val(Tot_DisAmt) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Item_Stock_Selection_Processing_Details Set Inward_Quantity = a.Inward_Quantity - b.Noof_Items from Item_Stock_Selection_Processing_Details a, Purchase_Details b where  a.Item_idNo = b.Item_IdNo and a.Batch_No =b.Batch_Serial_No and a.Manufactured_Date=b.Manufacture_DAte and a.Expiry_Date = b.Expiry_Date and a.Mrp_Rate = b.Mrp"
                'cmd.ExecuteNonQuery()

                da = New SqlClient.SqlDataAdapter("select * from Purchase_BatchNo_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'", con)
                da.SelectCommand.Transaction = tr
                dt1 = New DataTable
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then
                    cmd.CommandText = "Insert into EntryTempSub(Int1     ,Int2       , Name1      , Weight1) " & _
                                             "Select           Item_idNo ,Detail_SlNo, Batch_No   ,sum(Quantity)  from  Purchase_BatchNo_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "' GROUP BY Item_idNo ,Detail_SlNo , Batch_No "

                    cmd.ExecuteNonQuery()
                Else

                    cmd.CommandText = "Insert into EntryTempSub(Int1      ,  Int2       ,  Name1             , Weight1) " & _
                                             "Select           Item_IdNo  ,Detail_SlNo  , Batch_Serial_No    , sum(Noof_Items)  from  Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "' GROUP BY Item_idNo ,Detail_SlNo , Batch_Serial_No "

                    cmd.ExecuteNonQuery()
                End If
                dt1.Clear()
                da.Dispose()
           
               
            End If


            cmd.CommandText = "Delete from Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Purchase_BatchNo_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
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

                    Man_Mntn_id = Common_Procedures.Month_ShortNameToIdNo(con, dgv_Details.Rows(i).Cells(18).Value, tr)
                    Exp_Mnth_id = Common_Procedures.Month_ShortNameToIdNo(con, dgv_Details.Rows(i).Cells(23).Value, tr)


                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@PurchaseDate", dtp_Date.Value.Date)

                    If Val(dgv_Details.Rows(i).Cells(17).Value) = 0 Then dgv_Details.Rows(i).Cells(17).Value = 1

                    Man_dte = (Val(dgv_Details.Rows(i).Cells(17).Value)) & "/" & Val(Man_Mntn_id) & "/" & Val(dgv_Details.Rows(i).Cells(19).Value)

                    If IsDate(Man_dte) = False Then
                        Man_dte = "1/1/1900"
                    End If

                    If IsDate(Man_dte) = True Then
                        cmd.Parameters.AddWithValue("@ManufactureDate", Convert.ToDateTime(Man_dte))
                    End If


                    If Val(dgv_Details.Rows(i).Cells(22).Value) = 0 Then dgv_Details.Rows(i).Cells(22).Value = 1

                    Exp_dte = (Val(dgv_Details.Rows(i).Cells(22).Value)) & "/" & Val(Exp_Mnth_id) & "/" & Val(dgv_Details.Rows(i).Cells(24).Value)

                    If IsDate(Exp_dte) = False Then
                        Exp_dte = "1/1/1900"
                    End If

                    If IsDate(Exp_dte) = True Then
                        cmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(Exp_dte))
                    End If



                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Purchase_Details(Purchase_Code, Company_IdNo, Purchase_No, for_OrderBy, Purchase_Date, Ledger_IdNo, SL_No,  Item_Code , Item_IdNo, Unit_IdNo, Noof_Items, Rate, Amount  ,Discount_Perc  , Discount_Amount  ,Discount_Perc_Item, Discount_Amount_Item,Tax_Perc,Tax_Amount ,Total_Amount, Mrp ,Sales_Price, Batch_Serial_No,Manufacture_Day  ,Manufacture_Month_IdNo , Manufacture_Year , Manufacture_Date ,Expiry_Period_Days  ,Expiry_Day   ,  Expiry_Month_IdNo  , Expiry_Year,Expiry_Date      ,   Detail_SlNo) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ",'" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "', " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(10).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(11).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(12).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(13).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & "," & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & ",'" & Trim(dgv_Details.Rows(i).Cells(16).Value) & "'," & Str(Val(dgv_Details.Rows(i).Cells(17).Value)) & "," & (Man_Mntn_id) & "," & Str(Val(dgv_Details.Rows(i).Cells(19).Value)) & "," & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & " ," & Str(Val(dgv_Details.Rows(i).Cells(21).Value)) & "," & Str(Val(dgv_Details.Rows(i).Cells(22).Value)) & "," & Val(Exp_Mnth_id) & "," & Str(Val(dgv_Details.Rows(i).Cells(24).Value)) & ", " & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & "  ,   " & Str(Val(dgv_Details.Rows(i).Cells(26).Value)) & ")"
                    nr = cmd.ExecuteNonQuery()

                    nr = 0
                    cmd.CommandText = "Insert into Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Party_Bill_No, SL_No, Item_IdNo, Unit_IdNo, Quantity) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, " & Str(Val(led_id)) & ", '" & Trim(txt_BillNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Val(dgv_Details.Rows(i).Cells(4).Value) & " )"
                    nr = cmd.ExecuteNonQuery()

                    TempItemIDNo = 0
                    If Trim(dgv_Details.Rows(i).Cells(16).Value) = "" Then   '---------With out batch nos

                        cmd.CommandText = "Update Item_Stock_Selection_Processing_Details set Manufactured_Day =" & Val(dgv_Details.Rows(i).Cells(17).Value) & "   ,Manufactured_Month_IdNo = " & Val(Man_Mntn_id) & " ,Manufactured_Year= " & Val(dgv_Details.Rows(i).Cells(19).Value) & "  ,Expiry_Period_Days =  " & Val(dgv_Details.Rows(i).Cells(21).Value) & " ,Expiry_Day = " & Val(dgv_Details.Rows(i).Cells(22).Value) & " ,Expiry_Month_IdNo =" & Str(Val(Exp_Mnth_id)) & ",Expiry_Year= " & Val(dgv_Details.Rows(i).Cells(24).Value) & ",  Purchase_Rate = " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & " ,Sales_Rate =" & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & " ,Inward_Quantity = Inward_Quantity +" & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " where  Item_IdNo  = " & Val(itm_id) & " and Batch_No ='' and  Manufactured_Date= " & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & "  and  Expiry_Date= " & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & " and Mrp_Rate = " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & ""
                        nr = cmd.ExecuteNonQuery()

                        If nr = 0 Then

                            cmd.CommandText = "Insert into Item_Stock_Selection_Processing_Details (    Item_IdNo      ,    Batch_No        ,      Manufactured_Day                              ,   Manufactured_Month_IdNo ,     Manufactured_Year                               ,     Manufactured_Date                                           , Expiry_Period_Days                            ,     Expiry_Day                                  ,   Expiry_Month_IdNo       ,     Expiry_Year                                ,   Expiry_Date                                             ,      Purchase_Rate                              ,                  Mrp_Rate                               ,  Sales_Rate                                              ,   Inward_Quantity                                      ) " & _
                                                          "     Values                  (       " & Val(itm_id) & "      , ''               , " & Val(dgv_Details.Rows(i).Cells(17).Value) & "   , " & Str(Val(Man_Mntn_id)) & ",  " & Val(dgv_Details.Rows(i).Cells(19).Value) & " , " & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & ", " & Val(dgv_Details.Rows(i).Cells(21).Value) & " ," & Val(dgv_Details.Rows(i).Cells(22).Value) & ", " & Str(Val(Exp_Mnth_id)) & ", " & Val(dgv_Details.Rows(i).Cells(24).Value) & ", " & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & ", " & Val(dgv_Details.Rows(i).Cells(5).Value) & " , " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & "," & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & "  ) "
                            nr = cmd.ExecuteNonQuery()

                            TempItemIDNo = itm_id
                        End If

                    End If
                  

                    With dgv_Batch_details                 ' --------With batch no

                        If .RowCount > 0 Then
                            For j = 0 To .RowCount - 1

                                If Val(.Rows(j).Cells(0).Value) = Val(dgv_Details.Rows(i).Cells(26).Value) And Val(.Rows(j).Cells(2).Value) <> 0 Then

                                    Slno = Slno + 1


                                    cmd.CommandText = "Insert into Purchase_BatchNo_Details (    Purchase_Code      ,      Company_IdNo                ,      Purchase_No                    , for_OrderBy                                                                 ,     Purchase_Date     ,     Ledger_IdNo      , Detail_SlNo                        ,     Sl_No             ,   Item_IdNo         ,     Batch_No                              ,   Quantity                               ) " & _
                                                            "     Values                  ( '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ",    @PurchaseDate      ,  " & Val(led_id) & " ," & Val(.Rows(j).Cells(0).Value) & ", " & Str(Val(Slno)) & ", " & Val(itm_id) & " ,  '" & Trim(.Rows(j).Cells(1).Value) & "'  , " & Str(Val(.Rows(j).Cells(2).Value)) & "    ) "
                                    cmd.ExecuteNonQuery()


                                    cmd.CommandText = "Update Item_Stock_Selection_Processing_Details set Manufactured_Day =" & Val(dgv_Details.Rows(i).Cells(17).Value) & "   ,Manufactured_Month_IdNo = " & Val(Man_Mntn_id) & " ,Manufactured_Year= " & Val(dgv_Details.Rows(i).Cells(19).Value) & "  ,Expiry_Period_Days =  " & Val(dgv_Details.Rows(i).Cells(21).Value) & " ,Expiry_Day = " & Val(dgv_Details.Rows(i).Cells(22).Value) & " ,Expiry_Month_IdNo =" & Str(Val(Exp_Mnth_id)) & ",Expiry_Year= " & Val(dgv_Details.Rows(i).Cells(24).Value) & ",  Purchase_Rate = " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & " ,Sales_Rate =" & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & " ,Inward_Quantity = Inward_Quantity +" & Str(Val(.Rows(j).Cells(2).Value)) & " where  Item_IdNo  = " & Val(itm_id) & " and Batch_No ='" & Trim(.Rows(j).Cells(1).Value) & "' and  Manufactured_Date= " & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & "  and  Expiry_Date= " & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & " and Mrp_Rate = " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & ""
                                    nr = cmd.ExecuteNonQuery()

                                    If nr = 0 Then

                                        cmd.CommandText = "Insert into Item_Stock_Selection_Processing_Details (    Item_IdNo      ,          Batch_No                         ,      Manufactured_Day                              ,   Manufactured_Month_IdNo ,     Manufactured_Year                               ,     Manufactured_Date                                           , Expiry_Period_Days                            ,     Expiry_Day                                  ,   Expiry_Month_IdNo       ,     Expiry_Year                                ,   Expiry_Date                                             ,      Purchase_Rate                              ,                  Mrp_Rate                               ,  Sales_Rate                                         ,   Inward_Quantity                                      ) " & _
                                                                      "     Values                  (       " & Val(itm_id) & "      , '" & Trim(.Rows(j).Cells(1).Value) & "' , " & Val(dgv_Details.Rows(i).Cells(17).Value) & "   , " & Str(Val(Man_Mntn_id)) & ",  " & Val(dgv_Details.Rows(i).Cells(19).Value) & " , " & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & ", " & Val(dgv_Details.Rows(i).Cells(21).Value) & " ," & Val(dgv_Details.Rows(i).Cells(22).Value) & ", " & Str(Val(Exp_Mnth_id)) & ", " & Val(dgv_Details.Rows(i).Cells(24).Value) & ", " & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & ", " & Val(dgv_Details.Rows(i).Cells(5).Value) & " , " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & "," & Str(Val(.Rows(j).Cells(2).Value)) & "  ) "
                                        nr = cmd.ExecuteNonQuery()
                                    End If


                                End If

                            Next j


                        Else

                            cmd.CommandText = "Update Item_Stock_Selection_Processing_Details set Manufactured_Day =" & Val(dgv_Details.Rows(i).Cells(17).Value) & "   ,Manufactured_Month_IdNo = " & Val(Man_Mntn_id) & " ,Manufactured_Year= " & Val(dgv_Details.Rows(i).Cells(19).Value) & "  ,Expiry_Period_Days =  " & Val(dgv_Details.Rows(i).Cells(21).Value) & " ,Expiry_Day = " & Val(dgv_Details.Rows(i).Cells(22).Value) & " ,Expiry_Month_IdNo =" & Str(Val(Exp_Mnth_id)) & ",Expiry_Year= " & Val(dgv_Details.Rows(i).Cells(24).Value) & ",  Purchase_Rate = " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & " ,Sales_Rate =" & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & " ,Inward_Quantity = Inward_Quantity +" & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " where  Item_IdNo  = " & Val(itm_id) & " and Batch_No ='' and  Manufactured_Date= " & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & "  and  Expiry_Date= " & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & " and Mrp_Rate = " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & ""
                            nr = cmd.ExecuteNonQuery()

                            If nr = 0 Then

                                cmd.CommandText = "Insert into Item_Stock_Selection_Processing_Details (    Item_IdNo      ,    Batch_No        ,      Manufactured_Day                              ,   Manufactured_Month_IdNo ,     Manufactured_Year                               ,     Manufactured_Date                                           , Expiry_Period_Days                            ,     Expiry_Day                                  ,   Expiry_Month_IdNo       ,     Expiry_Year                                ,   Expiry_Date                                             ,      Purchase_Rate                              ,                  Mrp_Rate                               ,  Sales_Rate                                              ,   Inward_Quantity                                      ) " & _
                                                              "     Values                  (       " & Val(itm_id) & "      , ''               , " & Val(dgv_Details.Rows(i).Cells(17).Value) & "   , " & Str(Val(Man_Mntn_id)) & ",  " & Val(dgv_Details.Rows(i).Cells(19).Value) & " , " & IIf(IsDate(Man_dte) = True, "@ManufactureDate", "Null") & ", " & Val(dgv_Details.Rows(i).Cells(21).Value) & " ," & Val(dgv_Details.Rows(i).Cells(22).Value) & ", " & Str(Val(Exp_Mnth_id)) & ", " & Val(dgv_Details.Rows(i).Cells(24).Value) & ", " & IIf(IsDate(Exp_dte) = True, "@ExpiryDate", "Null") & ", " & Val(dgv_Details.Rows(i).Cells(5).Value) & " , " & Str(Val(dgv_Details.Rows(i).Cells(14).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(15).Value)) & "," & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & "  ) "
                                nr = cmd.ExecuteNonQuery()
                            End If

                        End If



                    End With
                End If

            Next i

            Dim Slno1 As Integer

            With dgv_Tax_Details

                Slno1 = 0


                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        Slno1 = Slno1 + 1


                        cmd.CommandText = "Insert into Purchase_Tax_Details (    Purchase_Code                    ,      Company_IdNo              ,      Purchase_No                               , for_OrderBy                                                                   ,     Purchase_Date              ,     Ledger_IdNo     ,      Sl_No             ,   Item_IdNo  ,       Gross_Amount                        ,          Discount_Amount                ,       Aessable_Amount                   ,   Tax_Perc                                  , Tax_Amount                       ) " & _
                                                "     Values                  ( '" & Trim(NewCode) & "'                , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "'            , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ",    @PurchaseDate             ,  " & Val(led_id) & " , " & Str(Val(Slno1)) & ", " & Val(itm_id) & ",  " & Val(.Rows(i).Cells(1).Value) & "  , " & Str(Val(.Rows(i).Cells(2).Value)) & " ," & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & " ," & Str(Val(.Rows(i).Cells(5).Value)) & ") "
                        cmd.ExecuteNonQuery()
                    End If




                Next

            End With


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
                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'PurcB', @PurchaseDate, " & Str(Val(purcac_id)) & ", " & Str(Val(Ac_id)) & ", " & Str(Val(txt_NetAmount.Text)) & ", 'Bill No . : " & Trim(lbl_PurchaseNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'PurcB', @PurchaseDate, 1, " & Str(Val(Ac_id)) & ", " & Str(Val(txt_NetAmount.Text)) & ", 'Bill No . : " & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            cmd.ExecuteNonQuery()

            Amt = Val(txt_NetAmount.Text) + Val(txt_TotalDiscAmount.Text) - Val(lbl_TotalTaxAmount.Text) + Val(txt_CashDiscAmount.Text) - Val(txt_AddLessAmount.Text) - Val(txt_RoundOff.Text)

            cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'PurcB', @PurchaseDate, 2, " & Str(Val(purcac_id)) & ", " & Str(-1 * Val(Amt)) & ", 'Bill No . : " & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            cmd.ExecuteNonQuery()

            If Val(lbl_TotalTaxAmount.Text) <> 0 Then

                TxAc_id = 20

                cmd.CommandText = "Insert into Voucher_Details (                   Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                                            " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'PurcB', @PurchaseDate, 3, " & Str(Val(TxAc_id)) & ", " & Str(-1 * Val(lbl_TotalTaxAmount.Text)) & ", 'Bill No . : " & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_TotalDiscAmount.Text) <> 0 Or Val(txt_CashDiscAmount.Text) <> 0 Or Val(txt_AddLessAmount.Text) <> 0 Then

                L_id = 17
                Amt = -1 * (Val(txt_TotalDiscAmount.Text) + Val(txt_CashDiscAmount.Text))

                cmd.CommandText = "Insert into Voucher_Details (                   Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                                            " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'PurcB', @PurchaseDate, 4, " & Str(Val(L_id)) & ", " & Str(-1 * Val(Amt)) & ", 'Bill No . : " & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_AddLessAmount.Text) <> 0 Then

                L_id = 17
                Amt = Val(txt_AddLessAmount.Text)

                cmd.CommandText = "Insert into Voucher_Details (                   Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                                            " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'PurcB', @PurchaseDate, 5, " & Str(Val(L_id)) & ", " & Str(-1 * Val(Amt)) & ", 'Bill No . : " & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_RoundOff.Text) <> 0 Then

                L_id = 24

                cmd.CommandText = "Insert into Voucher_Details (                   Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                                            " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", 'PurcB', @PurchaseDate, 6, " & Str(Val(L_id)) & ", " & Str(-1 * Val(txt_RoundOff.Text)) & ", 'Bill No . : " & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()

            End If

        
            ' If TempItemIDNo <> itm_id Then
            cmd.CommandText = "Update Item_Stock_Selection_Processing_Details Set Inward_Quantity = a.Inward_Quantity - b.Weight1 from Item_Stock_Selection_Processing_Details a, EntryTempSub b where  a.Item_idNo = b.Int1 and a.Batch_No =b.Name1 "
            nr = cmd.ExecuteNonQuery()

            '  End If

            cmd.CommandText = "Delete from Item_Stock_Selection_Processing_Details Where Inward_Quantity = 0 and OutWard_Quantity = 0 "
            cmd.ExecuteNonQuery()



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
            If InStr(1, Err.Description, "CK_Item_Stock_Selection_Processing_Details_1") > 0 Then
                MessageBox.Show("Invalid Inward Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Err.Description, "CK_Item_Stock_Selection_Processing_Details_2") > 0 Then
                MessageBox.Show("Invalid Outward Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, LCase(Err.Description), "ix_purchase_batchno_details_1") > 0 Then
                MessageBox.Show("Dublicate Batch No", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Err.Description, "CK_Item_Stock_Selection_Processing_Details_3") > 0 Then
                MessageBox.Show("Invalid Outward Quantity, Outward Quantity must be lesser than InWard Quantity", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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
        If Val(txt_NoofItems.Text) = 0 And Val(txt_Mrp.Text) = 0 Then
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
        Dim TotBatc_Qty As Single = 0


        'Total_BatchCalculation()
        'If dgv_Batch_Total_details.RowCount > 0 Then
        '    TotBatc_Qty = Val(dgv_Batch_Total_details.Rows(0).Cells(2).Value())

        'End If
        'If Val(TotBatc_Qty) <> Val(txt_NoofItems.Text) Then
        '    MessageBox.Show("Mismatch of Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_NoofItems.Enabled And txt_NoofItems.Visible Then txt_NoofItems.Focus()
        '    Exit Sub
        'End If


        MtchSTS = False



        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then
                    .Rows(i).Cells(1).Value = txt_Code.Text
                    .Rows(i).Cells(2).Value = cbo_ItemName.Text
                    .Rows(i).Cells(3).Value = lbl_Unit.Text
                    .Rows(i).Cells(4).Value = Val(txt_NoofItems.Text)
                    .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(txt_Amount.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(txt_DiscAmount.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(txt_DisPerc_Item.Text), "########0.00")
                    .Rows(i).Cells(10).Value = Format(Val(txt_DiscountAmountItem.Text), "########0.00")
                    .Rows(i).Cells(11).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                    .Rows(i).Cells(12).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                    .Rows(i).Cells(13).Value = Format(Val(txt_GrossAmount.Text), "########0.00")
                    .Rows(i).Cells(14).Value = Val(txt_Mrp.Text)
                    .Rows(i).Cells(15).Value = Format(Val(txt_Sales_Price.Text), "########0.00")
                    .Rows(i).Cells(16).Value = (txt_Batch_No.Text)
                    .Rows(i).Cells(17).Value = Val(txt_Manufacture_Day.Text)
                    .Rows(i).Cells(18).Value = (cbo_Manufacture_Month.Text)
                    .Rows(i).Cells(19).Value = Val(txt_Manufacture_Year.Text)
                    .Rows(i).Cells(20).Value = (txt_Mfg_Date.Text)
                    .Rows(i).Cells(21).Value = Val(txt_Expiray_Period_Days.Text)
                    .Rows(i).Cells(22).Value = Val(txt_Expiray_Day.Text)
                    .Rows(i).Cells(23).Value = (cbo_ExpiryMonth.Text)
                    .Rows(i).Cells(24).Value = Val(txt_Expiry_Year.Text)
                    .Rows(i).Cells(25).Value = (txt_Exp_date.Text)
                    .Rows(i).Cells(26).Value = (txt_Details_Slno.Text)
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
                .Rows(n).Cells(6).Value = Format(Val(txt_Amount.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(txt_DiscAmount.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(txt_DisPerc_Item.Text), "########0.00")
                .Rows(n).Cells(10).Value = Format(Val(txt_DiscountAmountItem.Text), "########0.00")
                .Rows(n).Cells(11).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                .Rows(n).Cells(12).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                .Rows(n).Cells(13).Value = Format(Val(txt_GrossAmount.Text), "########0.00")
                .Rows(n).Cells(14).Value = Val(txt_Mrp.Text)
                .Rows(n).Cells(15).Value = Format(Val(txt_Sales_Price.Text), "########0.00")
                .Rows(n).Cells(16).Value = (txt_Batch_No.Text)
                .Rows(n).Cells(17).Value = Val(txt_Manufacture_Day.Text)
                .Rows(n).Cells(18).Value = (cbo_Manufacture_Month.Text)
                .Rows(n).Cells(19).Value = Val(txt_Manufacture_Year.Text)
                .Rows(n).Cells(20).Value = (txt_Mfg_Date.Text)
                .Rows(n).Cells(21).Value = Val(txt_Expiray_Period_Days.Text)
                .Rows(n).Cells(22).Value = Val(txt_Expiray_Day.Text)
                .Rows(n).Cells(23).Value = (cbo_ExpiryMonth.Text)
                .Rows(n).Cells(24).Value = Val(txt_Expiry_Year.Text)
                .Rows(n).Cells(25).Value = (txt_Exp_date.Text)
                .Rows(n).Cells(26).Value = (txt_Details_Slno.Text)
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
        txt_Amount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscAmount.Text = ""
        txt_DisPerc_Item.Text = ""
        txt_DiscountAmountItem.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_GrossAmount.Text = ""
        txt_Mrp.Text = ""
        txt_Sales_Price.Text = ""
        txt_Batch_No.Text = ""
        txt_Manufacture_Day.Text = ""
        cbo_Manufacture_Month.Text = ""
        txt_Manufacture_Year.Text = ""
        txt_Mfg_Date.Text = ""
        txt_Expiray_Period_Days.Text = ""
        txt_Expiray_Day.Text = ""
        cbo_ExpiryMonth.Text = ""
        txt_Expiry_Year.Text = ""
        txt_Exp_date.Text = ""
        txt_Details_Slno.Text = dgv_Details.Rows.Count + 1

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
        Call Amount_Calculation()
    End Sub



    'Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
    '    If e.KeyCode = 38 Then
    '        txt_NoofItems.Focus()
    '    End If
    '    If e.KeyCode = 40 Then
    '        If txt_SubAmount.Enabled = True Then
    '            txt_SubAmount.Focus()
    '        Else
    '            txt_DiscAmount.Focus()
    '        End If
    '    End If
    'End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    If txt_SubAmount.Enabled = True Then
        '        txt_SubAmount.Focus()
        '    Else
        '        txt_DiscAmount.Focus()
        '    End If
        'End If
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        '  txt_Mfg_Date.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
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

            txt_Mrp.Focus()
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
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a, unit_head b where a.item_name = '" & Trim(cbo_ItemName.Text) & "' and a.unit_idno = b.unit_idno", con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                lbl_Unit.Text = dt.Rows(0)("unit_name").ToString
                txt_Rate.Text = dt.Rows(0)("Cost_Rate").ToString
                txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
                txt_Sales_Price.Text = dt.Rows(0)("Sales_Rate").ToString
                txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                txt_Mrp.Text = dt.Rows(0)("MRP_Rate").ToString
            End If
            dt.Dispose()
            da.Dispose()
        End If



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

    Private Sub txt_AddLessAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLessAmount.KeyDown
        'If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub





    Private Sub txt_AddLessAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLessAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
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
        NetAmount_Calculation()
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
                        .Rows(i).Cells(6).Value = Format(Val(txt_Amount.Text), "########0.00")
                        .Rows(i).Cells(7).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                        .Rows(i).Cells(8).Value = Format(Val(txt_DiscAmount.Text), "########0.00")
                        .Rows(i).Cells(9).Value = Format(Val(txt_DisPerc_Item.Text), "########0.00")
                        .Rows(i).Cells(10).Value = Format(Val(txt_DiscountAmountItem.Text), "########0.00")
                        .Rows(i).Cells(11).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                        .Rows(i).Cells(12).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                        .Rows(i).Cells(13).Value = Format(Val(txt_GrossAmount.Text), "########0.00")
                        .Rows(i).Cells(14).Value = Val(txt_Mrp.Text)
                        .Rows(i).Cells(15).Value = Format(Val(txt_Sales_Price.Text), "########0.00")
                        .Rows(i).Cells(16).Value = (txt_Batch_No.Text)
                        .Rows(i).Cells(17).Value = Val(txt_Manufacture_Day.Text)
                        .Rows(i).Cells(18).Value = (cbo_Manufacture_Month.Text)
                        .Rows(i).Cells(19).Value = Val(txt_Manufacture_Year.Text)
                        .Rows(i).Cells(20).Value = (txt_Mfg_Date.Text)
                        .Rows(i).Cells(21).Value = Val(txt_Expiray_Period_Days.Text)
                        .Rows(i).Cells(22).Value = Val(txt_Expiray_Day.Text)
                        .Rows(i).Cells(23).Value = Val(cbo_ExpiryMonth.Text)
                        .Rows(i).Cells(24).Value = Val(txt_Expiry_Year.Text)
                        .Rows(i).Cells(25).Value = (txt_Exp_date.Text)
                        .Rows(i).Cells(26).Value = (txt_Details_Slno.Text)

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

        txt_Amount.Text = Val(txt_NoofItems.Text) * Val(txt_Rate.Text)

        '-- txt_DiscountAmount.Text = Format(Val(txt_NoofItems.Text) * (Val(txt_Rate.Text) * Val(txt_DiscPerc.Text) / 100), "#########0.00")  '---30-NOV-2016
        txt_DiscAmount.Text = Format(Val(txt_NoofItems.Text) * ((Val(txt_Rate.Text) - Val(txt_DiscountAmountItem.Text)) * Val(txt_DiscPerc.Text) / 100), "#########0.00")


        txt_TaxAmount.Text = "0.00"
        If Trim(cbo_TaxType.Text) <> "" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
            totDisc = (Val(txt_DiscountAmountItem.Text) * Val(txt_NoofItems.Text))
            txt_TaxAmount.Text = Format(((Val(txt_Amount.Text) - totDisc - Val(txt_DiscAmount.Text)) * Val(txt_TaxPerc.Text) / 100), "#########0.00")
        End If

        txt_GrossAmount.Text = Format(Val(txt_Amount.Text) - (Val(txt_DiscountAmountItem.Text) * Val(txt_NoofItems.Text)) - Val(txt_DiscAmount.Text) + Val(txt_TaxAmount.Text), "########0.00")


    End Sub

    Private Sub GrossAmount_Calculation()
        Dim Sno As Integer
        Dim TotQty As Decimal, TotSubAmt As Decimal, TotDiscAmt As Decimal, TotTxAmt As Decimal, TotAmt As Decimal
        Dim TotDisAmtItm As Integer = 0
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
            TotSubAmt = TotSubAmt + Val(dgv_Details.Rows(i).Cells(6).Value)
            TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(8).Value) + (Val(dgv_Details.Rows(i).Cells(10).Value) * Val(dgv_Details.Rows(i).Cells(4).Value))
            TotDisAmtItm = TotTxAmt + Val(dgv_Details.Rows(i).Cells(10).Value)
            TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(12).Value)
            TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(13).Value) '- Val(dgv_Details.Rows(i).Cells(9).Value)


        Next

        ' txt_TotalQty.Text = Val(TotQty)
        ' txt_Aessableamount.Text = Format(TotSubAmt, "########0.00")
        txt_TotalDiscAmount.Text = Format(TotDisAmtItm, "########0.00")
        lbl_TotalTaxAmount.Text = Format(TotTxAmt, "########0.00")
        txt_TotalGrossAmount.Text = Format(TotAmt, "########0.00")

    End Sub
    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotQty As Decimal, TotSubAmt As Decimal, TotDiscAmt As Decimal, TotTxAmt As Decimal, TotAmt As Decimal
        Dim TotDisAmtItm As Integer = 0
        Sno = 0
        TotQty = 0
        TotSubAmt = 0
        TotDiscAmt = 0
        TotTxAmt = 0
        TotAmt = 0
        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                Sno = Sno + 1
                dgv_Details.Rows(i).Cells(0).Value = Sno

                TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(4).Value)
                TotSubAmt = TotSubAmt + Val(dgv_Details.Rows(i).Cells(6).Value)
                TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(8).Value) + (Val(dgv_Details.Rows(i).Cells(10).Value) * Val(dgv_Details.Rows(i).Cells(4).Value))
                TotDisAmtItm = TotTxAmt + Val(dgv_Details.Rows(i).Cells(10).Value)
                TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(12).Value)
                TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(13).Value) '- Val(dgv_Details.Rows(i).Cells(9).Value)

            Next
        End With

        With dgv_Details_Total
            If dgv_Details_Total.RowCount <= 0 Then dgv_Details_Total.Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotQty)
            .Rows(0).Cells(6).Value = Format(Val(TotSubAmt), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotDiscAmt), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(TotDisAmtItm), "########0.00")
            .Rows(0).Cells(12).Value = Format(Val(TotTxAmt), "########0.00")
            .Rows(0).Cells(13).Value = Format(Val(TotAmt), "########0.00")
        End With
    End Sub
    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        txt_CashDiscAmount.Text = Format(Val(txt_TotalGrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")
        txt_Aessableamount.Text = Format(Val(txt_TotalGrossAmount.Text) - Val(txt_CashDiscAmount.Text), "########0.00")


        NtAmt = Val(txt_Aessableamount.Text) + Val(txt_Freight.Text) + Val(txt_AddLessAmount.Text)

        txt_NetAmount.Text = Format(Val(NtAmt), "#########0")
        'txt_NetAmount.Text = Common_Procedures.Currency_Format(Val(txt_NetAmount.Text))

        txt_RoundOff.Text = Format(Val(CSng(txt_NetAmount.Text)) - Val(NtAmt), "#########0.00")


        'NtAmt = Val(txt_TotalGrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLessAmount.Text)

        'txt_RoundOff.Text = Format(Format(Val(NtAmt), "#########0") - Val(NtAmt), "#########0.00")

        'txt_NetAmount.Text = Format(Val(txt_Freight.Text) + Val(txt_TotalGrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLessAmount.Text) + Val(txt_RoundOff.Text), "########0.00")

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

        NoofItems_PerPage = 14

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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
                Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
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
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00")), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
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
        Dim Led_IdNo As Integer, Itm_IdNo As Integer, Pur_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0
            Pur_IdNo = 0
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Purchase_Code IN (select z.Purchase_Code from Purchase_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If
            If Val(Pur_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.PurchaseAc_IdNo = " & Str(Val(Pur_IdNo))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Purchase_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Purchase_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Purchase_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Purchase_Date").ToString), "dd-MM-yyyy")
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

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        With dgv_Details
            If Val(.Rows(e.RowIndex).Cells(26).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, 26)
                'If e.RowIndex = 0 Then
                '    .Rows(e.RowIndex).Cells(15).Value = 1
                'Else
                '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
                'End If
            End If
        End With
    End Sub





    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(2).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            txt_Code.Text = (dgv_Details.CurrentRow.Cells(1).Value)
            cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            lbl_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
            txt_NoofItems.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
            txt_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
            txt_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
            txt_DiscAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
            txt_DisPerc_Item.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
            txt_DiscountAmountItem.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
            txt_TaxPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(11).Value), "########0.00")
            txt_TaxAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
            txt_GrossAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")
            txt_Mrp.Text = Format(Val(dgv_Details.CurrentRow.Cells(14).Value), "#########0.00")
            txt_Sales_Price.Text = Format(Val(dgv_Details.CurrentRow.Cells(15).Value), "#########0.00")
            txt_Batch_No.Text = (dgv_Details.CurrentRow.Cells(16).Value)
            txt_Manufacture_Day.Text = Val(dgv_Details.CurrentRow.Cells(17).Value)
            cbo_Manufacture_Month.Text = (dgv_Details.CurrentRow.Cells(18).Value)
            txt_Manufacture_Year.Text = Val(dgv_Details.CurrentRow.Cells(19).Value)
            txt_Mfg_Date.Text = Val(dgv_Details.CurrentRow.Cells(20).Value)
            txt_Expiray_Period_Days.Text = Val(dgv_Details.CurrentRow.Cells(21).Value)
            txt_Expiray_Day.Text = Val(dgv_Details.CurrentRow.Cells(22).Value)
            cbo_ExpiryMonth.Text = (dgv_Details.CurrentRow.Cells(23).Value)
            txt_Expiry_Year.Text = Val(dgv_Details.CurrentRow.Cells(24).Value)
            txt_Exp_date.Text = (dgv_Details.CurrentRow.Cells(25).Value)

            txt_Details_Slno.Text = (dgv_Details.CurrentRow.Cells(26).Value)


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
                    If Val(.Rows(n).Cells(26).Value) = 0 Then
                        If n = 0 Then
                            .Rows(n).Cells(26).Value = 1
                        Else
                            .Rows(n).Cells(26).Value = Val(.Rows(n - 1).Cells(26).Value) + 1
                        End If
                    End If
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
        txt_Amount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscAmount.Text = ""
        txt_DisPerc_Item.Text = ""
        txt_DiscountAmountItem.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_GrossAmount.Text = ""
        txt_Mrp.Text = ""
        txt_Sales_Price.Text = ""
        txt_Batch_No.Text = ""
        txt_Manufacture_Day.Text = ""
        cbo_Manufacture_Month.Text = ""
        txt_Manufacture_Year.Text = ""
        txt_Mfg_Date.Text = ""
        txt_Expiray_Period_Days.Text = ""
        txt_Expiray_Day.Text = ""
        cbo_ExpiryMonth.Text = ""
        txt_Expiry_Year.Text = ""
        txt_Exp_date.Text = ""
        txt_Details_Slno.Text = dgv_Details.Rows.Count + 1

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
            txt_Amount.Text = ""
            txt_DiscPerc.Text = ""
            txt_DiscAmount.Text = ""
            txt_DisPerc_Item.Text = ""
            txt_DiscountAmountItem.Text = ""
            txt_TaxPerc.Text = ""
            txt_TaxAmount.Text = ""
            txt_GrossAmount.Text = ""
            txt_Mrp.Text = ""
            txt_Sales_Price.Text = ""
            txt_Batch_No.Text = ""
            txt_Manufacture_Day.Text = ""
            cbo_Manufacture_Month.Text = ""
            txt_Manufacture_Year.Text = ""
            txt_Mfg_Date.Text = ""
            txt_Expiray_Period_Days.Text = ""
            txt_Expiray_Day.Text = ""
            cbo_ExpiryMonth.Text = ""
            txt_Expiry_Year.Text = ""
            txt_Exp_date.Text = ""
            txt_Details_Slno.Text = dgv_Details.Rows.Count + 1
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
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        End If

    End Sub



    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, txt_BillNo, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
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
                SendKeys.Send("{TAB}")
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


            If Trim(cbo_ItemName.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_TotalDiscAmount.Focus()
            End If
        End If

    End Sub


    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, cbo_Ledger, txt_BillNo, "", "", "", "")
    End Sub

    Private Sub cbo_paymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, txt_BillNo, "", "", "", "")
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

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_PaymentMethod.Focus()
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

                txt_TotalGrossAmount.Text = Format(Val(txt_Aessableamount.Text) - Val(txt_TotalDiscAmount.Text) + Val(lbl_TotalTaxAmount.Text), "########0.00")

                NetAmount_Calculation()
            End If
        End If

        If txt_CashDiscPerc.Visible And txt_CashDiscPerc.Enabled Then txt_CashDiscPerc.Focus()

    End Sub

    Private Sub txt_Filter_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_Filter_ItemName.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : btn_Filter_Show.Focus()  ' SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Filter_BillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    'Private Sub txt_DiscAmountItemwise_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscAmount.KeyDown
    '    If e.KeyCode = 38 Then
    '        If txt_SubAmount.Enabled = True Then
    '            txt_SubAmount.Focus()
    '        Else
    '            txt_Rate.Focus()
    '        End If
    '    End If
    '    If e.KeyCode = 40 Then
    '        If txt_CashDiscPerc.Enabled Then txt_CashDiscPerc.Focus()
    '    End If
    'End Sub

    Private Sub txt_Expiry_Year_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Expiry_Year.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            cbo_ExpiryMonth.Focus()
            '  If txt_TaxPerc.Visible And txt_TaxPerc.Enabled Then txt_TaxPerc.Focus()
        End If
        If e.KeyCode = 40 Then
            If btn_Add.Enabled Then btn_Add.Focus()
        End If
    End Sub

    Private Sub txt_Expiry_Year_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Expiry_Year.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    'Private Sub txt_SubAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SubAmount.KeyDown
    '    If e.KeyCode = 38 Then
    '        txt_Rate.Focus()
    '    End If
    '    If e.KeyCode = 40 Then
    '        If txt_DiscAmount.Enabled = True Then txt_DiscAmount.Focus()
    '    End If
    'End Sub

    Private Sub txt_SubAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GrossAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    txt_DiscAmount.Focus()
        'End If

    End Sub

    Private Sub txt_SubAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GrossAmount.LostFocus
        '   If Val(txt_Rate.Text) = 0 Then
        If Val(txt_NoofItems.Text) <> 0 Then
            txt_Rate.Text = Val(txt_GrossAmount.Text) / Val(txt_NoofItems.Text)
        End If
        ' End If
    End Sub



    Private Sub cbo_Manufacture_Month_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Manufacture_Month.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Month_Head", "Month_ShortName", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_Manufacture_Month_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Manufacture_Month.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Manufacture_Month, txt_Manufacture_Day, txt_Manufacture_Year, "Month_Head", "Month_ShortName", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_Manufacture_Month_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Manufacture_Month.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Manufacture_Month, txt_Manufacture_Year, "Month_Head", "Month_ShortName", "", "(Month_IdNo = 0)")
    End Sub
    Private Sub cbo_ExpiryMonth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ExpiryMonth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Month_Head", "Month_ShortName", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_ExpiryMonth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ExpiryMonth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ExpiryMonth, txt_Expiray_Day, txt_Expiry_Year, "Month_Head", "Month_ShortName", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_ExpiryMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ExpiryMonth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ExpiryMonth, txt_Expiry_Year, "Month_Head", "Month_ShortName", "", "(Month_IdNo = 0)")
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_Tax_Details.Name Or ActiveControl.Name = dgv_Batch_Selection.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Tax_Details.Name Then
                dgv1 = dgv_Tax_Details

            ElseIf dgv_Tax_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Tax_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Tax_Details.Name Then
                dgv1 = dgv_Tax_Details


            ElseIf ActiveControl.Name = dgv_Batch_Selection.Name Then
                dgv1 = dgv_Batch_Selection

            ElseIf dgv_Batch_Selection.IsCurrentRowDirty = True Then
                dgv1 = dgv_Batch_Selection

            ElseIf dgv_ActiveCtrl_Name = dgv_Batch_Selection.Name Then
                dgv1 = dgv_Batch_Selection


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



                ElseIf dgv1.Name = dgv_Batch_Selection.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                BatchClose_Selection()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If


                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                BatchClose_Selection()
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                BatchClose_Selection()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If
                            'ElseIf .CurrentCell.ColumnIndex = 3 Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(1)


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

    Private Sub dgv_Batch_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Batch_Selection.CellEndEdit
        dgv_Batch_Details_CellLeave(sender, e)
        Total_BatchCalculation()
    End Sub

    Private Sub dgv_Batch_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Batch_Selection.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable

        ' Dim Rect As Rectangle

        With dgv_Batch_Selection
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If






        End With

    End Sub

    Private Sub dgv_Batch_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Batch_Selection.CellLeave
        With dgv_Batch_Selection
            'If .CurrentCell.ColumnIndex = 8 Then
            '    .Rows(.CurrentRow.Index).Cells(8).Value = Val(dgv_Accessories_Details.Rows(dgv_Accessories_Details.CurrentRow.Index).Cells(2).Value)
            'End If

        End With
    End Sub

    Private Sub dgv_Batch_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Batch_Selection.CellValueChanged
        On Error Resume Next

        With dgv_Batch_Selection
            If .Visible Then


                If .CurrentCell.ColumnIndex = 2 Then


                    Total_BatchCalculation()

                End If
                If .CurrentCell.ColumnIndex = 1 And .Rows(.CurrentRow.Index).Cells(1).Value <> "" Then
                    .Rows(.CurrentRow.Index).Cells(2).Value = "1"
                End If


            End If
        End With
    End Sub
    Private Sub dgv_Batch_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Batch_Selection.EditingControlShowing
        dgtxt_BatchDetails = Nothing

        dgtxt_BatchDetails = CType(dgv_Batch_Selection.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgtxt_BatchDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BatchDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Batch_Selection.Name
        dgv_Batch_Selection.EditingControl.BackColor = Color.Lime
        dgv_Batch_Selection.EditingControl.ForeColor = Color.Blue
        dgv_Batch_Selection.SelectAll()
    End Sub

    Private Sub dgtxt_BatchDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BatchDetails.KeyDown
        With dgv_Batch_Selection
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

    Private Sub dgtxt_BatchDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BatchDetails.KeyPress

        With dgv_Batch_Selection
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Then
                    If Common_Procedures.Accept_AlphaNumericOnlyWithSlash(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
                
            End If
        End With

    End Sub
    Private Sub dgtxt_BatchDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BatchDetails.TextChanged
        Try
            With dgv_Batch_Selection

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_BatchDetails.Text)

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


    Private Sub dgtxt_BatchDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BatchDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Batch_Selection_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_Batch_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Batch_Selection.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Batch_Selection_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Batch_Selection.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Batch_Selection

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                Total_BatchCalculation()

            End With

        End If

    End Sub

    Private Sub dgv_Batch_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Batch_Selection.LostFocus
        On Error Resume Next
        dgv_Batch_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Batch_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Batch_Selection.RowsAdded
        Dim n As Integer

        With dgv_Batch_Selection
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub


    Private Sub Total_BatchCalculation()
        Dim Sno As Integer

        Dim TotQty As Single

        '  If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotQty = 0

        With dgv_Batch_Selection
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotQty = TotQty + Val(.Rows(i).Cells(2).Value())


                End If

            Next i

        End With


        With dgv_Batch_Total_details
            If .RowCount = 0 Then .Rows.Add()


            .Rows(0).Cells(2).Value = Val(TotQty)
        End With

    End Sub
    Private Sub Total_BatchQtyCalculation()
        Dim Sno As Integer

        Dim TotQty As Single

        '  If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotQty = 0

        With dgv_Batch_details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotQty = TotQty + Val(.Rows(i).Cells(2).Value())


                End If

            Next i

        End With


        With dgv_Total_Batch
            If .RowCount = 0 Then .Rows.Add()


            .Rows(0).Cells(2).Value = Val(TotQty)
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
                    dgv_Tax_Details.Rows(n).Cells(1).Value = .Rows(i).Cells(13).Value
                    dgv_Tax_Details.Rows(n).Cells(2).Value = .Rows(i).Cells(8).Value
                    dgv_Tax_Details.Rows(n).Cells(3).Value = Format(Val(.Rows(i).Cells(13).Value) - Val(.Rows(i).Cells(8).Value), "###########0.00")
                    dgv_Tax_Details.Rows(n).Cells(4).Value = .Rows(i).Cells(11).Value
                    dgv_Tax_Details.Rows(n).Cells(5).Value = .Rows(i).Cells(12).Value
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

    Private Sub pnl_Back_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnl_Back.Paint

    End Sub

    Private Sub txt_Batch_No_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Batch_No.Enter

    End Sub

    Private Sub txt_Batch_No_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Batch_No.GotFocus
        pnl_BatchSelection_ToolTip.Visible = True
        '  Batch_Status = False
        BatchNo_Selection()
        ' Batch_Status = True
    End Sub

    Private Sub txt_Batch_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Batch_No.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_Batch_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Batch_No.KeyPress
        If Common_Procedures.Accept_AlphaNumericOnlyWithSlash(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Batch_No_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Batch_No.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            '   If (dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5) And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
            BatchNo_Selection()
        End If
        ' End If

    End Sub


    Private Sub txt_Batch_No_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Batch_No.LostFocus
       
        pnl_BatchSelection_ToolTip.Visible = False
    End Sub
    Private Sub BatchNo_Selection()
        Dim Det_SLNo As Integer
        Dim n As Integer, SNo As Integer
        Dim Sht_ID As Integer = 0
        Dim Mch_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        If Batch_Status = True Then Exit Sub
        Try


            Det_SLNo = Val(txt_Details_Slno.Text)

            With dgv_Batch_Selection

                SNo = 0
                .Rows.Clear()

                For i = 0 To dgv_Batch_details.RowCount - 1
                    If Det_SLNo = Val(dgv_Batch_details.Rows(i).Cells(0).Value) Then

                        SNo = SNo + 1

                        n = .Rows.Add()
                        .Rows(n).Cells(0).Value = SNo
                        .Rows(n).Cells(1).Value = Trim(dgv_Batch_details.Rows(i).Cells(1).Value)
                        .Rows(n).Cells(2).Value = Trim(dgv_Batch_details.Rows(i).Cells(2).Value)
                        '.Rows(n).Cells(3).Value = Val(dgv_StoppageDetails.Rows(i).Cells(3).Value)

                    End If
                Next i
            End With

            Total_BatchCalculation()

            pnl_Batch.Visible = True
            pnl_Back.Enabled = False
            dgv_Batch_Selection.Focus()
            If dgv_Batch_Selection.Rows.Count > 0 Then
                dgv_Batch_Selection.CurrentCell = dgv_Batch_Selection.Rows(0).Cells(1)
                ' dgv_StoppageSelection.CurrentCell.Selected = True
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BATCH...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    
    Private Sub btn_Batch_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Batch_Close.Click
        BatchClose_Selection()

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

    Private Sub Get_BatchDetails()
       

    End Sub

   

    Private Sub BatchClose_Selection()
        Dim cmd As New SqlClient.SqlCommand
        Dim I As Integer
        Dim Det_SLNo As Integer = 0
        Dim n As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim vtot As Long = 0
        Dim Sht_Mns As Long = 0
        Dim Sht_ID As Integer = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vBatch_Nos As String = ""
        Dim Batch_No As String = ""
        Det_SLNo = Val(txt_Details_Slno.Text)

        cmd.Connection = con
        With dgv_Batch_details


LOOP1:
            For I = 0 To .RowCount - 1

                If Val(.Rows(I).Cells(0).Value) = Val(Det_SLNo) Then

                    If I = .Rows.Count - 1 Then
                        For J = 0 To .ColumnCount - 1
                            .Rows(I).Cells(J).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(I)

                    End If

                    GoTo LOOP1

                End If

            Next I

            For I = 0 To dgv_Batch_Selection.RowCount - 1

                If Trim(dgv_Batch_Selection.Rows(I).Cells(1).Value) <> "" And Val(dgv_Batch_Selection.Rows(I).Cells(2).Value) <> 0 Then

                    n = .Rows.Add()

                    .Rows(n).Cells(0).Value = Val(Det_SLNo)
                    .Rows(n).Cells(1).Value = dgv_Batch_Selection.Rows(I).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_Batch_Selection.Rows(I).Cells(2).Value
                    ' .Rows(n).Cells(3).Value = Val(dgv_StoppageSelection.Rows(I).Cells(3).Value)

                End If
            Next I

        End With

        Total_BatchCalculation()
        Dim TotQty As Single = 0
        If Val(dgv_Batch_Total_details.Rows(0).Cells(2).Value) <> 0 Then

            TotQty = TotQty + Val(dgv_Batch_Total_details.Rows(0).Cells(2).Value())


        End If
        If Val(TotQty) <> 0 Then txt_NoofItems.Text = Val(TotQty)

        For I = 0 To dgv_Batch_Selection.RowCount - 1

            If Trim(dgv_Batch_Selection.Rows(I).Cells(1).Value) <> "" Then
                vBatch_Nos = Trim(vBatch_Nos) & IIf(Trim(vBatch_Nos) <> "", ", ", "") & Trim(dgv_Batch_Selection.Rows(I).Cells(1).Value)
            End If
            Batch_No = Trim(vBatch_Nos)
            txt_Batch_No.Text = (Batch_No)
        Next

        For I = 0 To dgv_Details.Rows.Count - 1
            If Val(dgv_Details.Rows(I).Cells(26).Value) = 0 Then
                Set_Max_DetailsSlNo(I, 26)
            End If
        Next

        pnl_Back.Enabled = True
        pnl_Batch.Visible = False


        If txt_Manufacture_Day.Enabled And txt_Manufacture_Day.Visible Then
            '
            txt_Manufacture_Day.Focus()

        End If

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

    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
                End If
            Next
            .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        End With

    End Sub
    Private Sub ExpirayDate_Calculation()
        Dim cmd As New SqlClient.SqlCommand
        Dim dte As Date
        Dim Man_dte As DateTime
        Dim Man_Mnth_id As Integer = 0
        Dim Exp_Day As Integer = 0
        Dim Exp_mnth As Integer = 0


        Man_Mnth_id = Common_Procedures.Month_ShortNameToIdNo(con, cbo_Manufacture_Month.Text)
        If Val(txt_Manufacture_Day.Text) <> 0 And Val(Man_Mnth_id) <> 0 And Val(txt_Manufacture_Year.Text) <> 0 Then
            Man_dte = Val(txt_Manufacture_Day.Text) & "/" & Val(Man_Mnth_id) & "/" & Val(txt_Manufacture_Year.Text)

            Exp_Day = Val(txt_Expiray_Period_Days.Text)
            dte = DateAdd(DateInterval.Day, Exp_Day, Man_dte)
            txt_Expiray_Day.Text = dte.Day.ToString
            Exp_mnth = dte.Month.ToString
            cbo_ExpiryMonth.Text = Common_Procedures.Month_IdNoToShortName(con, Exp_mnth)
            txt_Expiry_Year.Text = dte.Year.ToString
        End If
    End Sub

    
    Private Sub txt_Expiray_Period_Days_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Expiray_Period_Days.TextChanged
        ExpirayDate_Calculation()
    End Sub

   
   
    Private Sub txt_Manufacture_Day_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Manufacture_Day.KeyDown
        If e.KeyCode = 38 Then
            Batch_Status = True
            '  txt_Batch_No.Focus()


        End If
    End Sub

    Private Sub txt_Sales_Price_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Sales_Price.KeyDown
        If e.KeyCode = 40 Then
            Batch_Status = False
            ' txt_Batch_No.Focus()


        End If
    End Sub

    Private Sub txt_Sales_Price_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Sales_Price.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Batch_Status = False
        End If
    End Sub

    Private Sub txt_Code_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Code.LostFocus
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Trim(UCase(txtItmCd)) <> Trim(UCase(txt_Code.Text)) Then
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a, unit_head b where a.item_Code = '" & Trim(txt_Code.Text) & "' and a.unit_idno = b.unit_idno", con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                cbo_ItemName.Text = dt.Rows(0)("Item_name").ToString
                lbl_Unit.Text = dt.Rows(0)("unit_name").ToString
                txt_Rate.Text = dt.Rows(0)("Cost_Rate").ToString
                'txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
                txt_Sales_Price.Text = dt.Rows(0)("Sales_Rate").ToString
                txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                txt_Mrp.Text = dt.Rows(0)("MRP_Rate").ToString
            End If
            dt.Dispose()
            da.Dispose()
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

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Batch_Status = False
        BatchNo_Selection()
        Batch_Status = True
    End Sub

   
    Private Sub txt_Manufacture_Day_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Manufacture_Day.LostFocus
        'If Val(txt_Manufacture_Day.Text) = 0 And Trim(cbo_Manufacture_Month.Text) = "" And Trim(txt_Manufacture_Year.Text) = "" Then
        '    txt_Manufacture_Day.Text = "1"
        'End If
    End Sub


    Private Sub txt_TotalDiscAmount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TotalDiscAmount.LostFocus
        Dim n As Integer
        dgv_Tax_Details.Rows.Clear()
        With dgv_Details
            If dgv_Details.Rows.Count > 0 Then
                For i = 0 To .RowCount - 1
                    n = dgv_Tax_Details.Rows.Add()

                    dgv_Tax_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Tax_Details.Rows(n).Cells(1).Value = .Rows(i).Cells(13).Value
                    dgv_Tax_Details.Rows(n).Cells(2).Value = .Rows(i).Cells(8).Value
                    dgv_Tax_Details.Rows(n).Cells(3).Value = Format(Val(.Rows(i).Cells(13).Value) - Val(.Rows(i).Cells(8).Value), "###########0.00")
                    dgv_Tax_Details.Rows(n).Cells(4).Value = .Rows(i).Cells(11).Value
                    dgv_Tax_Details.Rows(n).Cells(5).Value = .Rows(i).Cells(12).Value
                Next
            End If

        End With
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

        da1 = New SqlClient.SqlDataAdapter("select top 5 Purchase_No,Purchase_Date,Rate  from Purchase_Details a where Ledger_IdNo = " & Led_Id & " and Item_IdNo = " & Itm_Id & "Order By Purchase_Date desc", con)
        da1.Fill(dt1)


        With dgv_PreviousBillDetails
            .Rows.Clear()

            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0).Item("Purchase_No").ToString) = False Then

                    If Trim(dt1.Rows(0).Item("Purchase_No").ToString) <> "" Then

                        For i = 0 To dt1.Rows.Count - 1
                            n = dgv_PreviousBillDetails.Rows.Add()
                            dgv_PreviousBillDetails.Rows(n).Cells(0).Value = dt1.Rows(i).Item("Purchase_No").ToString
                            dgv_PreviousBillDetails.Rows(n).Cells(1).Value = FormatDateTime(Convert.ToDateTime(dt1.Rows(i).Item("Purchase_Date").ToString), DateFormat.ShortDate)
                            dgv_PreviousBillDetails.Rows(n).Cells(2).Value = dt1.Rows(i).Item("Rate").ToString

                        Next


                    End If
                End If

                pnl_PreviousBillDetails.Visible = True
                pnl_PreviousBillDetails.BringToFront()

            End If


        End With

    End Sub

    Private Sub txt_Manufacture_Year_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Manufacture_Year.LostFocus
        If Val(txt_Manufacture_Day.Text) = 0 And Trim(cbo_Manufacture_Month.Text) = "" And Trim(txt_Manufacture_Year.Text) = "" Then
            txt_Manufacture_Day.Text = "1"
        End If
    End Sub

    Private Sub Label39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label39.Click

    End Sub
End Class