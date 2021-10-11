Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail

Public Class SalesEntry_Simple6_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GSALE-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private cmbItmNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetMxIndx As Integer
    Private prn_DetAr(200, 10) As String
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private Vou_Code As String = ""

    Public Shared vSmsPhoneNo As String
    Public Shared vSmsMessage As String
    Public Shared SMSProvider_SenderID As String
    Public Shared SMSProvider_Key As String
    Public Shared SMSProvider_RouteID As String
    Public Shared SMSProvider_Type As String
    Public Shared vSmsSendStatus As String
    Public Shared vSmsSendFor As String
    Private Sms_Count As Integer = 0



    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        '  pnl_Back.Enabled = True
        'pnl_Filter.Visible = True

        lbl_AmountInWords.Text = "Rupees :                                                                               "

        If Filter_Status = False Then
            'dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_Fromdate.Text = Convert.ToDateTime("01/" & Month(Now) & "/" & Year(Now))
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
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

        lbl_GrossAmount.Text = ""

        dgv_Details.Rows.Clear()

        cbo_PaymentMethod.Text = "CREDIT"
        cbo_FilterPaymentMode.Text = ""
        cbo_FilterDescription.Text = ""
        cbo_FilterEntrySts.Text = ""
        txt_ActualRate.Text = ""
        lbl_Actual_amount.Text = ""
        lbl_Actual_GrossAMount.Text = ""
        lbl_Actual_Netamount.Text = ""
        lbl_Actual_TaxAmount.Text = ""

        txt_CGST_Perc.Text = ""
        txt_SGST_Perc.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""

        lbl_receiptAmount.Text = ""

        chk_Delivery.Checked = False
        'txt_DcDate.Text = Date.Today
        cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, 22)
        cbo_TaxAc.Text = Common_Procedures.Ledger_IdNoToName(con, 20)
        txt_SlNo.Text = "1"

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

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_Cell_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*,vbh.*, b.Ledger_Name as LedgerName, c.Ledger_Name as SalesAcName, d.Ledger_Name as TaxAcName from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.SalesAc_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.TaxAc_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Voucher_Bill_Head VBH ON VBH.Entry_Identification =  A.Sales_Code  where a.Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Date").ToString
                cbo_PaymentMethod.Text = dt1.Rows(0).Item("Payment_Method").ToString

                If IsDBNull(dt1.Rows(0).Item("LedgerName").ToString) = False Then

                    If Trim(dt1.Rows(0).Item("LedgerName").ToString) <> "" Then

                        If Val(dt1.Rows(0).Item("Ledger_IdNo").ToString) <> 1 Then
                            cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString

                        Else
                            cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString

                        End If

                    Else
                        cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString

                    End If

                Else

                    cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString


                End If


                cbo_SalesAc.Text = dt1.Rows(0).Item("SalesAcName").ToString
                cbo_TaxAc.Text = dt1.Rows(0).Item("TaxAcName").ToString
                'txt_TotalQty.Text = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                lbl_receiptAmount.Text = dt1.Rows(0).Item("Vehicle_No").ToString


                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                lbl_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                txt_TaxPerc.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc").ToString), "########0.00")
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")

                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")

                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                If Val(dt1.Rows(0).Item("Delivery_Status").ToString) = 1 Then chk_Delivery.Checked = True
                txt_Advance.Text = Format(Val(dt1.Rows(0).Item("Advance_Amount").ToString), "########0.00")
                lbl_Balance.Text = Format(Val(dt1.Rows(0).Item("Balance_Amount").ToString), "########0.00")
                lbl_receiptAmount.Text = Format(Val(dt1.Rows(0).Item("Receipt_Amount").ToString), "########0.00")

                txt_DeliveryDate.Text = dt1.Rows(0).Item("Delivery_Date").ToString
                txt_Amount_Received_Date.Text = dt1.Rows(0).Item("Received_Date").ToString


                txt_CGST_Perc.Text = Format(Val(dt1.Rows(0).Item("CGst_Percentage").ToString), "########0.00")
                txt_SGST_Perc.Text = Format(Val(dt1.Rows(0).Item("SGst_Percentage").ToString), "########0.00")
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")


                '---Actual

                lbl_Actual_GrossAMount.Text = Format(Val(dt1.Rows(0).Item("Actual_Gross_Amount").ToString), "########0.00")
                lbl_Actual_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Actual_Tax_Amount").ToString), "########0.00")
                lbl_Actual_Netamount.Text = Format(Val(dt1.Rows(0).Item("Actual_Net_Amount").ToString), "########0.00")

                '--------
                'lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")


                '---Advance
                da3 = New SqlClient.SqlDataAdapter("select sum(a.amount) as AdvAmount  from Voucher_Bill_Details a LEFT OUTER JOIN Sales_Head b ON a.entry_identification = b.Sales_Code  where a.Voucher_Bill_Code = '" & Trim(dt1.Rows(0).Item("Voucher_Bill_Code").ToString) & "' and b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                dt3 = New DataTable
                da3.Fill(dt3)
                If dt3.Rows.Count > 0 Then
                    txt_Advance.Text = IIf(Val(dt3.Rows(0).Item("AdvAmount").ToString) <> 0, Format(Val(dt3.Rows(0).Item("AdvAmount").ToString), "########0.00"), "")
                End If
                dt3.Clear()
                dt3.Dispose()
                da3.Dispose()


                '---Receipt 


                da3 = New SqlClient.SqlDataAdapter("select sum(a.amount) AS RecAmount  from Voucher_Bill_Details a  where a.entry_identification <> '" & Trim(dt1.Rows(0).Item("Sales_CODE").ToString) & " ' and  a.Voucher_Bill_Code ='" & Trim(dt1.Rows(0).Item("Voucher_Bill_Code").ToString) & "'  AND a.entry_identification like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                dt3 = New DataTable
                da3.Fill(dt3)
                If dt3.Rows.Count > 0 Then
                    lbl_receiptAmount.Text = IIf(Val(dt3.Rows(0).Item("RecAmount").ToString) <> 0, Format(Val(dt3.Rows(0).Item("RecAmount").ToString), "########0.00"), "")
                End If
                dt3.Clear()
                dt3.Dispose()
                da3.Dispose()


                da2 = New SqlClient.SqlDataAdapter("select a.sl_no,a.Item_code, b.Item_Name, c.Unit_Name, a.Item_Description, a.Noof_Items, a.Rate, a.Amount,a.Advance_Amount ,a.Balance_Amount ,a.Sales_Order_Code,a.Actual_Rate ,a.Actual_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()

                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_Details.Rows.Add()
                            SNo = SNo + 1
                            dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                            dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                            dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Item_code").ToString
                            dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Unit_Name").ToString
                            dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Item_Description").ToString
                            dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                            dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Actual_Rate").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Actual_Amount").ToString), "########0.00")

                        Next i

                    End If

                    For i = 0 To .Rows.Count - 1
                        dgv_Details.Rows(n).Cells(0).Value = i + 1
                    Next

                End With

                TotalAmount_Calculation()

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                dt2.Clear()

            End If

            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            dt2.Dispose()

            da1.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub SalesEntry_Simple3_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                Get_OrderDetails()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub SalesEntry_Simple3_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1119" Then
            chk_Delivery.Visible = True
            lbl_Balance.Visible = True
            lbl_BalanceName.Visible = True
            txt_Advance.Visible = True
            lbl_Advance.Visible = True
        Else
            chk_Delivery.Visible = False
            lbl_Balance.Visible = False
            lbl_BalanceName.Visible = False
            txt_Advance.Visible = False
            lbl_Advance.Visible = False

        End If

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
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

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 28 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_SalesAc.DataSource = dt4
        cbo_SalesAc.DisplayMember = "Ledger_DisplayName"

        cbo_PaymentMethod.Items.Clear()
        cbo_PaymentMethod.Items.Add("CASH")
        cbo_PaymentMethod.Items.Add("CREDIT")

        cbo_FilterPaymentMode.Items.Clear()
        cbo_FilterPaymentMode.Items.Add("CASH")
        cbo_FilterPaymentMode.Items.Add("CREDIT")


        cbo_FilterEntrySts.Items.Clear()
        cbo_FilterEntrySts.Items.Add("")
        cbo_FilterEntrySts.Items.Add("DELIVERED")
        cbo_FilterEntrySts.Items.Add("NOT DELIVERED")
        cbo_FilterEntrySts.Items.Add("PAID")
        cbo_FilterEntrySts.Items.Add("NOT PAID")

        pnl_Filter.Visible = True
        pnl_Filter.Left = 10 '(Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = 10 '(Me.Height - pnl_Filter.Height) \ 2

        pnl_Back.Visible = False
        pnl_Back.Left = 10 '(Me.Width - pnl_Filter.Width) \ 2
        pnl_Back.Top = 10 '(Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryDate.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofItems.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ActualRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SerialNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_receiptAmount.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Advance.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Delivery.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Delete.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Pdf.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_FilterPaymentMode.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_FilterEntrySts.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_FilterDescription.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_code.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Perc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_code.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofItems.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ActualRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SerialNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_receiptAmount.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Advance.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Delivery.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Delete.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Pdf.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_FilterPaymentMode.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_FilterEntrySts.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_FilterDescription.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Perc.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoofItems.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ActualRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_receiptAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Advance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Advance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoofItems.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ActualRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_receiptAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

        SMSProvider_RouteID = Common_Procedures.settings.SMS_Provider_RouteID
        SMSProvider_Key = Common_Procedures.settings.SMS_Provider_Key
        SMSProvider_SenderID = Common_Procedures.settings.SMS_Provider_SenderID
        SMSProvider_Type = Common_Procedures.settings.SMS_Provider_Type

    End Sub

    Private Sub SalesEntry_Simple3_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub SalesEntry_Simple3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = False Then

                    Return_CurrentRow()

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

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode)) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If



            cmd.Connection = con


            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(lbl_Company.Tag) & " and b.entry_identification = '" & Trim(NewCode) & "' and b.crdr_type = 'CR' and a.voucher_bill_code = b.voucher_bill_code and a.company_idno = b.company_idno"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update voucher_bill_head set debit_amount = a.debit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(lbl_Company.Tag) & " and b.entry_identification = '" & Trim(NewCode) & "' and b.crdr_type = 'DR' and a.voucher_bill_code = b.voucher_bill_code and a.company_idno = b.company_idno"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(lbl_Company.Tag) & " and   entry_identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.ledger_idno = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
            da.Fill(dt2)
            cbo_Filter_ItemName.DataSource = dt2
            cbo_Filter_ItemName.DisplayMember = "item_name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Sales_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub


    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and  Entry_VAT_GST_Type = 'GST' Order by for_Orderby, Sales_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub


    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and  Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Sales_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub


    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and  Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, Sales_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            If pnl_Filter.Visible And FrmLdSTS = False Then
                pnl_Filter.Visible = False
                pnl_Back.Visible = True
                pnl_Back.Enabled = True
            End If
            New_Entry = True


            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", " Entry_VAT_GST_Type = 'GST' ", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select top 1 a.*, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Sales_No desc", con)
            dt2 = New DataTable
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("Payment_Method").ToString <> "" Then cbo_PaymentMethod.Text = dt2.Rows(0).Item("Payment_Method").ToString
                If dt2.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAc.Text = dt2.Rows(0).Item("SalesAcName").ToString
                If dt2.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_TaxAc.Text = dt2.Rows(0).Item("TaxAcName").ToString
                If dt2.Rows(0).Item("Tax_Perc").ToString <> "" Then txt_TaxPerc.Text = Val(dt2.Rows(0).Item("Tax_Perc").ToString)
            End If
            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            dt2.Dispose()
            da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            RefCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Invocie No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Invocie No.", "FOR NEW INVOICE INSERTION...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(InvCode) & "'", con)
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
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Invoice No.", "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim led_id As Integer = 0
        Dim saleac_id As Integer = 0
        Dim txac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim CsParNm As String
        Dim vTotQty As Single = 0
        Dim vforOrdby As Single = 0
        Dim Amt As Single = 0
        Dim L_ID As Integer = 0
        Dim Delvry_Sts As Integer = 0
        Dim VouBil As String = ""
        Dim Nr As Integer = 0



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
        CsParNm = ""
        If led_id = 0 Then
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CREDIT" Then
                MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
                Exit Sub

            Else
                led_id = 1
                CsParNm = Trim(cbo_Ledger.Text)

            End If
        End If

        If led_id = 1 And Trim(CsParNm) = "" Then
            CsParNm = "Cash"
        End If

        saleac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)


        If saleac_id = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            saleac_id = 22
            'MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        txac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TaxAc.Text)

        If txac_id = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
            txac_id = 20
            'MessageBox.Show("Invalid Tax A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If itm_id = 0 Then
                        MessageBox.Show("Invalid iTEM Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    unt_id = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(3).Value)
                    If unt_id = 0 Then
                        MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With
        Delvry_Sts = 0
        If chk_Delivery.Checked = True Then Delvry_Sts = 1
        NetAmount_Calculation()
        'TotalAmount_Calculation()

        vTotQty = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)

            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sales_Head (Entry_VAT_GST_Type ,  Sales_Code  ,              Company_IdNo        ,                Sales_No           ,             for_OrderBy    , Sales_Date,               Payment_Method          ,          Ledger_IdNo    ,        Cash_PartyName  ,          SalesAc_IdNo      ,  Tax_Type,           TaxAc_IdNo     ,               Narration           ,           Total_Qty      ,              SubTotal_Amount          , Total_DiscountAmount, Total_TaxAmount,              Gross_Amount             ,                 CashDiscount_Perc      ,              CashDiscount_Amount         ,             Assessable_Value         ,              Tax_Perc             ,                Tax_Amount           ,               AddLess_Amount       ,              Round_Off             ,             Net_Amount                               ,               Vehicle_No           ,   Delivery_Status        ,   Advance_Amount           ,     Balance_Amount               ,Receipt_Amount                       , Delivery_Date                        , Received_Date                               ,Actual_Gross_Amount                      ,  Actual_Net_Amount                   ,Actual_Tax_Amount                     ,CGst_Percentage                 ,CGst_Amount                       ,SGst_Percentage                ,SGst_Amount)  " & _
                                        " Values (             'GST'   , '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", @SalesDate, '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", '" & Trim(CsParNm) & "', " & Str(Val(saleac_id)) & ",    'VAT' , " & Str(Val(txac_id)) & ", '" & Trim(txt_Narration.Text) & "', " & Str(Val(vTotQty)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ",           0         ,       0        , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(lbl_CashDiscAmount.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ",  " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", '" & Trim(lbl_receiptAmount.Text) & "'        , " & Val(Delvry_Sts) & " ,  " & Val(txt_Advance.Text) & ", " & Val(lbl_Balance.Text) & "  ," & Val(lbl_receiptAmount.Text) & "  ,'" & Trim(txt_DeliveryDate.Text) & "' ,'" & Trim(txt_Amount_Received_Date.Text) & "'," & Val(lbl_Actual_GrossAMount.Text) & " ," & Val(lbl_Actual_Netamount.Text) & "," & Val(lbl_Actual_TaxAmount.Text) & "," & Val(txt_CGST_Perc.Text) & " ," & Val(lbl_CGST_Amount.Text) & " ," & Val(txt_SGST_Perc.Text) & "," & Val(lbl_SGST_Amount.Text) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Sales_Head set Entry_VAT_GST_Type = 'GST' ,Sales_Date = @SalesDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", Cash_PartyName = '" & Trim(CsParNm) & "',  SalesAc_IdNo = " & Str(Val(saleac_id)) & ", Tax_Type = 'VAT', TaxAc_IdNo = " & Str(Val(txac_id)) & ", Narration = '" & Trim(txt_Narration.Text) & "', Total_Qty = " & Str(Val(vTotQty)) & ", SubTotal_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Total_DiscountAmount = 0, Total_TaxAmount = 0, Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(lbl_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Tax_Perc = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ",  AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Vehicle_No = '" & Trim(lbl_receiptAmount.Text) & "',  Delivery_Status =" & Val(Delvry_Sts) & "       ,   Advance_Amount =  " & Val(txt_Advance.Text) & "  ,     Balance_Amount =" & Val(lbl_Balance.Text) & ",Receipt_Amount = " & Val(lbl_receiptAmount.Text) & " ,Delivery_Date = '" & Trim(txt_DeliveryDate.Text) & "' ,Received_Date = '" & Trim(txt_Amount_Received_Date.Text) & "',Actual_Gross_Amount  =" & Val(lbl_Actual_GrossAMount.Text) & ",  Actual_Net_Amount = " & Val(lbl_Actual_Netamount.Text) & " ,Actual_Tax_Amount = " & Val(lbl_Actual_TaxAmount.Text) & ",CGst_Percentage = " & Val(txt_CGST_Perc.Text) & " ,CGst_Amount = " & Val(lbl_CGST_Amount.Text) & " ,SGst_Percentage = " & Val(txt_SGST_Perc.Text) & "  ,SGst_Amount =" & Val(lbl_SGST_Amount.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(lbl_Company.Tag) & " and b.entry_identification = '" & Trim(NewCode) & "' and b.crdr_type = 'CR' and a.voucher_bill_code = b.voucher_bill_code and a.company_idno = b.company_idno"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update voucher_bill_head set debit_amount = a.debit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(lbl_Company.Tag) & " and b.entry_identification = '" & Trim(NewCode) & "' and b.crdr_type = 'DR' and a.voucher_bill_code = b.voucher_bill_code and a.company_idno = b.company_idno"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(lbl_Company.Tag) & " and  entry_identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        unt_id = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        If itm_id <> 0 Then

                            Sno = Sno + 1

                            cmd.CommandText = "Insert into Sales_Details ( Sales_Code,             Company_IdNo         ,               Sales_No            ,           for_OrderBy      , Sales_Date,          Ledger_IdNo    ,        Sl_No         ,          Item_IdNo      ,   Item_code                             ,        Unit_IdNo      ,                  Item_Description             ,                 Noof_Items               ,                      Rate                ,                      Amount          ,Actual_Rate                               ,Actual_Amount) " & _
                                                    " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "' , " & Str(Val(unt_id)) & ", '" & Trim(.Rows(i).Cells(4).Value) & "', " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " ," & Str(Val(.Rows(i).Cells(8).Value)) & " ," & Str(Val(.Rows(i).Cells(9).Value)) & ")"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Insert into Item_Processing_Details (    Reference_Code    ,             Company_IdNo         ,            Reference_No           ,          for_OrderBy       , Reference_Date,          Ledger_IdNo    ,           Party_Bill_No           ,            SL_No     ,          Item_IdNo      ,           Unit_IdNo     ,                         Quantity               ) " & _
                                                    " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",    @SalesDate , " & Str(Val(led_id)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(-1 * Val(.Rows(i).Cells(5).Value)) & " )"
                            cmd.ExecuteNonQuery()




                        End If

                    End If

                Next i

            End With


            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Nr = 0
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            Ac_id = 0
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                Ac_id = 1
            Else
                Ac_id = led_id
            End If

            cmd.CommandText = "Insert into Voucher_Head (     Voucher_Code            ,          For_OrderByCode   ,             Company_IdNo         ,           Voucher_No              ,             For_OrderBy    , Voucher_Type, Voucher_Date,           Debtor_Idno  ,          Creditor_Idno     ,                Total_VoucherAmount        ,         Narration                                , Indicate,       Year_For_Report                                     ,       Entry_Identification                  , Voucher_Receipt_Code ) " & _
                                " Values ('" & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales'     ,   @SalesDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ",    'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "',    1    , " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "',          ''          ) "
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details (       Voucher_Code                   ,          For_OrderByCode   ,              Company_IdNo        ,           Voucher_No              ,           For_OrderBy      , Voucher_Type, Voucher_Date, SL_No,        Ledger_IdNo     ,                       Voucher_Amount           ,              Narration                        ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              "   Values             ('" & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",  'Sales',  @SalesDate ,   1  , " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' ) "
            Nr = cmd.ExecuteNonQuery()


            Amt = Val(CSng(lbl_NetAmount.Text)) - Val(lbl_TaxAmount.Text) - Val(txt_AddLess.Text) - Val(lbl_RoundOff.Text) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text)


            cmd.CommandText = "Insert into Voucher_Details (      Voucher_Code     ,          For_OrderByCode   ,             Company_IdNo         ,           Voucher_No              ,           For_OrderBy      , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo       ,     Voucher_Amount   ,     Narration                                 ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              " Values                     ('" & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",  'Sales'    ,  @SalesDate ,   2  , " & Str(Val(saleac_id)) & ", " & Str(Val(Amt)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' ) "
            cmd.ExecuteNonQuery()

            If Val(lbl_CGST_Amount.Text) <> 0 Then
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,  Ledger_IdNo     ,             Voucher_Amount                                        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales'     ,   @SalesDate,   3  ,      25          , " & Str(Val(lbl_CGST_Amount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            If Val(lbl_SGST_Amount.Text) <> 0 Then
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,  Ledger_IdNo     ,             Voucher_Amount                                        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              " Values             ('" & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales'     ,   @SalesDate,   4  ,      26          , " & Str(Val(lbl_SGST_Amount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            'If Val(txt_Freight.Text) <> 0 Then
            '    L_ID = 9
            '    cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   4  , " & Str(Val(L_ID)) & ", " & Str(Val(txt_Freight.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            '    cmd.ExecuteNonQuery()
            'End If


            If Val(txt_AddLess.Text) <> 0 Then
                L_ID = 17
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales'     ,   @SalesDate,   5  , " & Str(Val(L_ID)) & ", " & Str(Val(txt_AddLess.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            If Val(lbl_RoundOff.Text) <> 0 Then
                L_ID = 24
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount         ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales'     ,   @SalesDate,   6  , " & Str(Val(L_ID)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If


            If Val(txt_Advance.Text) <> 0 Then
                Nr = 0
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo     ,             Voucher_Amount          ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales'     ,   @SalesDate,   7  , " & Str(Val(led_id)) & ", " & Str(Val(txt_Advance.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' ) "
                Nr = cmd.ExecuteNonQuery()
            End If





            '---Bill Posting


            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, led_id, Trim(lbl_InvoiceNo.Text), 0, CSng(Val(lbl_NetAmount.Text)), "DR", Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            '---Advance Posting



            If Val(txt_Advance.Text) <> 0 Then

                Vou_Code = Trim(Common_Procedures.get_FieldValue(con, "voucher_bill_head", "Voucher_Bill_Code", "entry_identification = '" & Trim(NewCode) & "'", lbl_Company.Tag, tr))


                Nr = 0
                cmd.CommandText = "Insert into voucher_bill_details (   Voucher_Bill_Code   ,           Company_Idno            , Voucher_Bill_Date,        Ledger_Idno      ,  entry_identification                       ,            Amount                 ,  CrDr_Type                ) " & _
                                                    " Values ( '" & Trim(Vou_Code) & "'     , " & Str(Val(lbl_Company.Tag)) & " ,  @SalesDate      , " & Str(Val(led_id)) & ", '" & Trim(NewCode) & "', " & Str(Val(txt_Advance.Text)) & ", 'CR' ) "
                Nr = cmd.ExecuteNonQuery()


                Nr = 0
                cmd.CommandText = "update voucher_bill_head set Credit_amount = Credit_amount + " & Str(Val(txt_Advance.Text)) & " where company_idno = " & Str(lbl_Company.Tag) & " and ledger_idno = " & Str(led_id) & " and Voucher_Bill_Code ='" & Trim(Vou_Code) & "'   and  entry_identification = '" & Trim(NewCode) & "'" ' and crdr_type = 'DR'"
                Nr = cmd.ExecuteNonQuery()

                If Nr = 0 Then
                    Err.Description = "Error"
                    Exit Sub
                End If
            End If




            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            move_record(lbl_InvoiceNo.Text)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim itm_id As Integer
        Dim unt_id As Integer

        If Trim(cbo_ItemName.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        itm_id = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
        If itm_id = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If


        If Trim(cbo_Unit.Text) = "" Then
            MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        unt_id = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)
        If unt_id = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        If Val(txt_NoofItems.Text) = 0 Then
            MessageBox.Show("Invalid No.of Items", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_NoofItems.Enabled Then txt_NoofItems.Focus()
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(2).Value = txt_code.Text
                    .Rows(i).Cells(3).Value = cbo_Unit.Text
                    .Rows(i).Cells(4).Value = txt_SerialNo.Text
                    .Rows(i).Cells(5).Value = Val(txt_NoofItems.Text)
                    .Rows(i).Cells(6).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(lbl_Amount.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(txt_ActualRate.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(lbl_Actual_amount.Text), "########0.00")

                    '.Rows(i).Selected = True

                    MtchSTS = True

                    'If i >= 10 Then .FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = txt_code.Text
                .Rows(n).Cells(3).Value = cbo_Unit.Text
                .Rows(n).Cells(4).Value = txt_SerialNo.Text
                .Rows(n).Cells(5).Value = Val(txt_NoofItems.Text)
                .Rows(n).Cells(6).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(lbl_Amount.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(txt_ActualRate.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(lbl_Actual_amount.Text), "########0.00")

                '.Rows(n).Selected = True

                'If n >= 10 Then .FirstDisplayedScrollingRowIndex = n - 9

            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        txt_code.Text = ""
        cbo_Unit.Text = ""
        txt_SerialNo.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        txt_ActualRate.Text = ""
        lbl_Actual_amount.Text = ""
        Grid_Cell_DeSelect()

        If txt_code.Enabled And txt_code.Visible Then txt_code.Focus()

        ' If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_NoofItems_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofItems.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoofItems.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub txt_ActualRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ActualRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_ActualRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_ActualRate.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_code, cbo_Unit, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, cbo_Unit, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_ItemName.Text) <> "" Then
                cbo_Unit.Focus()
            Else
                txt_CashDiscPerc.Focus()
            End If
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

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "item_head", "item_Name", "", "(item_idno = 0)")
            cmbItmNm = cbo_ItemName.Text
        End With
    End Sub

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)("Item_code").ToString) = False Then
                    txt_code.Text = dt.Rows(0)("Item_code").ToString
                End If
                If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
                    cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                End If
                If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                    txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
                End If

            End If
            dt.Dispose()
            da.Dispose()
        End If

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : e.SuppressKeyPress = True : txt_AddLess.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, dtp_Date, chk_Delivery, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, chk_Delivery, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10   )", "(Ledger_IdNo = 0)")
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_PaymentMethod, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PaymentMethod, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then txt_code.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Gross_Discount_Tax_Amount_Calculation()
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.TextChanged
        Gross_Discount_Tax_Amount_Calculation()
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Gross_Discount_Tax_Amount_Calculation()
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        cbo_ItemName.Text = Trim(.Rows(i).Cells(1).Value)
                        txt_code.Text = Trim(.Rows(i).Cells(2).Value)
                        cbo_Unit.Text = Trim(.Rows(i).Cells(3).Value)
                        txt_SerialNo.Text = Trim(.Rows(i).Cells(4).Value)
                        txt_NoofItems.Text = Val(.Rows(i).Cells(5).Value)
                        txt_Rate.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.00")
                        txt_ActualRate.Text = Format(Val(.Rows(i).Cells(8).Value), "########0.00")
                        lbl_Actual_amount.Text = Format(Val(.Rows(i).Cells(9).Value), "########0.00")

                        Exit For

                    End If

                Next

            End With

            If Val(txt_SlNo.Text) = 0 Then
                txt_SlNo.Text = dgv_Details.Rows.Count + 1
                txt_CashDiscPerc.Focus()
            Else
                cbo_ItemName.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, cbo_ItemName, txt_NoofItems, "unit_head", "unit_Name", "", "(unit_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_NoofItems, "unit_head", "unit_Name", "", "(unit_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub txt_SerialNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SerialNo.KeyDown
        If e.KeyCode = 40 Then btn_Add.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_SerialNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SerialNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
            'SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

        Get_OrderDetails()

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 )  ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "AccountsGroup_IdNo = 10 ", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "AccountsGroup_IdNo = 10  ", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "item_head", "item_Name", "", "(item_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, Nothing, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Sales_code As String
        Dim sno As Integer = 0
        Dim n As Integer = 0
        Dim AdvAmt As Double = 0


        Sales_code = Trim(dgv_Filter_Details.CurrentRow.Cells(18).Value)

        pnl_Back.Visible = True
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        If Sales_code <> "" Then
            move_record(Trim(Common_Procedures.get_FieldValue(con, "Sales_Head", "Sales_No", "Sales_Code ='" & Trim(Sales_code) & "'", lbl_Company.Tag)))
            Exit Sub
        Else
            new_record()
        End If

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Sms_Count = 0
        btn_Sms.Text = "SMS"
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
            txt_code.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)

            cbo_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
            txt_SerialNo.Text = Trim(dgv_Details.CurrentRow.Cells(4).Value)
            txt_NoofItems.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
            lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
            txt_ActualRate.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
            lbl_Actual_amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")

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

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        txt_code.Text = ""
        cbo_Unit.Text = ""
        txt_SerialNo.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        txt_ActualRate.Text = ""
        lbl_Actual_amount.Text = ""

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = True
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

            TotalAmount_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_ItemName.Text = ""
            txt_code.Text = ""
            cbo_Unit.Text = ""
            txt_SerialNo.Text = ""
            txt_NoofItems.Text = ""
            txt_Rate.Text = ""
            lbl_Amount.Text = ""
            txt_ActualRate.Text = ""
            lbl_Actual_amount.Text = ""


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

    Private Sub lbl_receiptAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lbl_receiptAmount.KeyPress


        If Asc(e.KeyChar) = 13 Then
            txt_Amount_Received_Date.Focus()

        End If
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_Pdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pdf.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Return_CurrentRow()
    End Sub
    Private Sub Return_CurrentRow()
        Dim NewCode As String = ""

        pnl_Back.Visible = False
        pnl_Filter.Visible = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Get_OrderDetails()

        If dgv_Filter_Details.Rows.Count > 0 Then
            For i = 0 To dgv_Filter_Details.Rows.Count - 3
                If Trim(NewCode) = Trim(dgv_Filter_Details.Rows(i).Cells(18).Value) Then

                    dgv_Filter_Details.Rows(i).Selected = True

                End If
            Next
        End If
    End Sub

    Private Sub Amount_Calculation()
        lbl_Amount.Text = Format(Val(txt_NoofItems.Text) * Val(txt_Rate.Text), "#########0.00")
        lbl_Actual_amount.Text = Format(Val(txt_NoofItems.Text) * Val(txt_ActualRate.Text), "#########0.00")

    End Sub

    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer
        Dim TotQty As Decimal
        Dim TotAmt As Decimal
        Dim TotActAmt As Decimal


        Sno = 0
        TotQty = 0
        TotAmt = 0
        TotActAmt = 0
        For i = 0 To dgv_Details.RowCount - 1
            Sno = Sno + 1
            dgv_Details.Rows(i).Cells(0).Value = Sno

            If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
                TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(5).Value)
                TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(7).Value)
                TotActAmt = TotActAmt + Val(dgv_Details.Rows(i).Cells(9).Value)
            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotQty)
            .Rows(0).Cells(7).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(TotActAmt), "########0.00")
        End With

        lbl_GrossAmount.Text = Format(TotAmt, "########0.00")
        lbl_Actual_GrossAMount.Text = Format(TotActAmt, "########0.00")

        Gross_Discount_Tax_Amount_Calculation()

    End Sub

    Private Sub TotalAmount_Calculation_Filter()
        Dim Sno As Integer
        Dim TotAdv As Decimal
        Dim TotAmt As Decimal
        Dim TotRcpt As Decimal
        Dim TotBal As Decimal


        Sno = 0
        TotBal = 0
        TotAmt = 0
        TotAdv = 0
        For i = 0 To dgv_Filter_Details.RowCount - 1

            If Val(dgv_Filter_Details.Rows(i).Cells(7).Value) <> 0 Then
                TotAmt = TotAmt + Val(dgv_Filter_Details.Rows(i).Cells(7).Value)
                TotAdv = TotAdv + Val(dgv_Filter_Details.Rows(i).Cells(8).Value)
                TotRcpt = TotRcpt + Val(dgv_Filter_Details.Rows(i).Cells(9).Value)
                TotBal = TotBal + Val(dgv_Filter_Details.Rows(i).Cells(10).Value)

            End If
        Next

        With dgv_FilterTotal
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(7).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotAdv), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(TotRcpt), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(TotBal), "########0.00")
        End With


    End Sub

    Private Sub Gross_Discount_Tax_Amount_Calculation()


        lbl_CashDiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

        lbl_Assessable.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_CashDiscAmount.Text), "#########0.00")

        lbl_TaxAmount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_TaxPerc.Text) / 100, "#########0.00")

        lbl_Actual_TaxAmount.Text = Format(Val(lbl_Actual_GrossAMount.Text) * Val(txt_TaxPerc.Text) / 100, "#########0.00")


        NetAmount_Calculation()


    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        lbl_Assessable.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_CashDiscAmount.Text) + Val(txt_AddLess.Text), "##############0.00")

        lbl_CGST_Amount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_CGST_Perc.Text) / 100, "############0.00")
        lbl_SGST_Amount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_SGST_Perc.Text) / 100, "############0.00")

        NtAmt = Val(lbl_Assessable.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text)

        'NtAmt = Val(lbl_GrossAmount.Text) - Val(lbl_CashDiscAmount.Text) + Val(lbl_TaxAmount.Text) + Val(txt_AddLess.Text)

        lbl_Actual_Netamount.Text = Val(lbl_Actual_GrossAMount.Text) + Val(lbl_Actual_TaxAmount.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")
        'lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")
        lbl_Balance.Text = Format(Val(NtAmt) - Val(txt_Advance.Text) - Val(lbl_receiptAmount.Text), "############0.00")
        'lbl_AmountInWords.Text = "Rupees :                                                                               "
        'If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
        '    lbl_AmountInWords.Text = "Rupees :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        'End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

        'Dim bytes As Byte() = PrintDocument1.Print("pdf")
        ''Dim bytes As Byte() = RptViewer.LocalReport.Render("Excel")
        'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.xls")

        ''Dim bytes As Byte() = RptViewer.LocalReport.Render("Pdf")
        ''Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.pdf")
        'fs.Write(bytes, 0, bytes.Length)
        'fs.Close()
        'MessageBox.Show("ok")

        prn_InpOpts = ""
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        Else

            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "12")

            prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Print_PDF_Status = True Then
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else

                    PrintDocument1.Print()

                    'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

                    '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                    '    'PrintDocument1.DocumentName = "c:\test1.pdf"
                    '    'PrintDocument1.Print()

                    '    'Dim bytes As Byte() = PrintDocument1.Print("pdf")
                    '    ''Dim bytes As Byte() = RptViewer.LocalReport.Render("Excel")
                    '    'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.xls")

                    '    'Dim bytes As Byte() = System.IO.File.ReadAllBytes("C:\test1.pdf")
                    '    'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.pdf")
                    '    'fs.Write(bytes, 0, bytes.Length)
                    '    'fs.Close()
                    '    'MessageBox.Show("ok")

                    '    'PrintDocument1.Print()
                    'End If

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

        Print_PDF_Status = False

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

                        If Trim(prn_DetDt.Rows(I).Item("Item_Description").ToString) <> "" Then

                            Erase BlNoAr
                            BlNoAr = New String(20) {}

                            m1 = 0
                            bln = "S/No : " & Trim(prn_DetDt.Rows(I).Item("Item_Description").ToString)

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
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            Printing_Format2(e)
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1119" Then
            Printing_Format3(e)
        Else
            Printing_Format1(e)
        End If
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

        EntryCode =Trim(Pk_Condition ) &  Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

    Private Sub Printing_Format111111(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
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
            .Left = 65
            .Right = 50
            .Top = 65
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

        TxtHgt = 21 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 9 ' 10

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

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

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

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 15, CurY, 0, 0, pFont)
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

                            If Trim(prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then

                                Erase BlNoAr
                                BlNoAr = New String(20) {}

                                m1 = 0
                                bln = "S/No : " & Trim(prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString)

LOOP1:
                                If Len(bln) > 47 Then
                                    For I = 47 To 1 Step -1
                                        If Mid$(bln, I, 1) = " " Or Mid$(bln, I, 1) = "," Or Mid$(bln, I, 1) = "/" Or Mid$(bln, I, 1) = "\" Or Mid$(bln, I, 1) = "-" Or Mid$(bln, I, 1) = "." Or Mid$(bln, I, 1) = "&" Or Mid$(bln, I, 1) = "_" Then Exit For
                                    Next I
                                    If I = 0 Then I = 47
                                    m1 = m1 + 1
                                    BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), I)
                                    'BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), I - 1)
                                    bln = Microsoft.VisualBasic.Right(bln, Len(bln) - I)
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

                                For I = 1 To m1
                                    If NoofDets > NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                        e.HasMorePages = True
                                        Return

                                    End If

                                    CurY = CurY + TxtHgt - 3
                                    p1Font = New Font("Calibri", 8, FontStyle.Regular)
                                    Common_Procedures.Print_To_PrintDocument(e, BlNoAr(I), LMargin + ClArr(1) + 20, CurY, 0, 0, p1Font)
                                    NoofDets = NoofDets + 1
                                Next

                                CurY = CurY + 2

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


            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
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
                        Common_Procedures.Print_To_PrintDocument(e, "Labour Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Labour Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
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


    Private Sub lbl_TaxAmount_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_TaxAmount.DoubleClick
        Dim VtAmt As String = ""

        VtAmt = InputBox("Enter vat Amount :", "FOR VAT AMOUNT ALTERATION....", Val(lbl_TaxAmount.Text))

        If Trim(VtAmt) <> "" Then
            If Val(VtAmt) <> 0 Then
                lbl_TaxAmount.Text = Format(Val(VtAmt), "#########0.00")
                NetAmount_Calculation()
            End If
        End If

        If txt_TaxPerc.Visible And txt_TaxPerc.Enabled Then txt_TaxPerc.Focus()

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim PpSzSTS As Boolean = False
        Dim strWidth As Single = 0

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 0
            .Bottom = 0
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        pFont = New Font("Calibri", 12, FontStyle.Regular)


        ''==========================================================================================================================
        ''==========================================================================================================================
        ''pFont = New Font("Calibri", 10, FontStyle.Regular)

        'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

        'For I = 100 To 1100 Step 300

        '    CurY = I
        '    For J = 1 To 850 Step 40

        '        CurX = J
        '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

        '        CurX = J + 20
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

        '    Next

        'Next

        'For I = 200 To 800 Step 250

        '    CurX = I
        '    For J = 1 To 1200 Step 40

        '        CurY = J
        '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        '        CurY = J + 20
        '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        '    Next

        'Next

        'e.HasMorePages = False

        'Exit Sub

        ''==========================================================================================================================
        ''==========================================================================================================================


        NoofItems_PerPage = 18

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo)

                Try

                    NoofDets = 0

                    CurY = 420

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                p1Font = New Font("Calibri", 12, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + 930, CurY, 1, 0, p1Font)

                                NoofDets = NoofDets + 1

                                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, False)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Item_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 40 Then
                                For I = 40 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 40
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString), LMargin + 22, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 75, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + 510, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 600, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 639, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + 75, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String, Led_CSTNo As String
        Dim Trans_Nm As String = ""
        Dim CurY As Single = 0
        Dim LedAr(10) As String
        Dim Indx As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_CSTNo = ""

            Led_Name = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
            'Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)
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

            'If Trim(Led_TinNo) <> "" Then
            '    Indx = Indx + 1
            '    LedAr(Indx) = "Tin No : " & Trim(Led_TinNo)
            'End If

            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            CurY = TMargin + 205
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 500, CurY, 0, 0, p1Font)

            CurY = TMargin + 205
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 683, CurY, 0, 0, pFont)

            CurY = TMargin + 230
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, LedAr(1), LMargin + 60, CurY, 0, 0, p1Font)
            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(2), LMargin + 60, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(3), LMargin + 60, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4), LMargin + 60, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(5), LMargin + 60, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(6), LMargin + 60, CurY, 0, 0, pFont)

            CurY = TMargin + 245
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + 500, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + 670, CurY, 0, 0, pFont)

            CurY = TMargin + 265
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + 500, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + 670, CurY, 0, 0, pFont)

            CurY = TMargin + 285
            Common_Procedures.Print_To_PrintDocument(e, Led_TinNo, LMargin + 500, CurY, 0, 0, pFont)

            CurY = TMargin + 305
            Common_Procedures.Print_To_PrintDocument(e, Led_CSTNo, LMargin + 500, CurY, 0, 0, pFont)

            CurY = TMargin + 325
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString, LMargin + 500, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal PageHeight As Single, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String = "", Rup2 As String = "", Rup3 As String = ""
        Dim I As Integer
        Dim CurY As Single = 0

        Try

            If is_LastPage = True Then

                CurY = TMargin + 765

                e.Graphics.DrawLine(Pens.Black, LMargin + 430, CurY, LMargin + 760, CurY)

                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 400, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + 510, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)

                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                Rup2 = ""
                Rup3 = ""
                If Len(Rup1) > 55 Then
                    For I = 55 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 55
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If

                If Len(Rup2) > 55 Then
                    For I = 55 To 1 Step -1
                        If Mid$(Trim(Rup2), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 55
                    Rup3 = Microsoft.VisualBasic.Right(Trim(Rup2), Len(Rup2) - I)
                    Rup2 = Microsoft.VisualBasic.Left(Trim(Rup2), I - 1)
                End If

                CurY = CurY + TxtHgt + 7

                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + 400, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7
                'Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 60, CurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge :", LMargin + 400, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7
                'Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 60, CurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add/Less :", LMargin + 400, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7
                'Common_Procedures.Print_To_PrintDocument(e, Rup3, LMargin + 60, CurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + 400, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                End If

                CurY = TMargin + 926
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + 775, CurY, 1, 0, p1Font)

            End If

            CurY = TMargin + 995
            Common_Procedures.Print_To_PrintDocument(e, Rup1, LMargin + 80, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 80, CurY, 0, 0, pFont)


            CurY = TMargin + 1085
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + 80, CurY, 0, 0, pFont)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font, pFont1 As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim PpSzSTS As Boolean = False
        Dim strWidth As Single = 0

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 0
            .Bottom = 0
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        pFont = New Font("Calibri", 12, FontStyle.Regular)


        ''==========================================================================================================================
        ''==========================================================================================================================
        ''pFont = New Font("Calibri", 10, FontStyle.Regular)

        ''pFont1 = New Font("Calibri", 8, FontStyle.Regular)

        ''For I = 100 To 1100 Step 300

        ''    CurY = I
        ''    For J = 1 To 850 Step 40

        ''        CurX = J
        ''        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
        ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

        ''        CurX = J + 20
        ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
        ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
        ''        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

        ''    Next

        ''Next

        ''For I = 200 To 800 Step 250

        ''    CurX = I
        ''    For J = 1 To 1200 Step 40

        ''        CurY = J
        ''        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
        ''        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        ''        CurY = J + 20
        ''        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
        ''        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        ''    Next

        ''Next

        ''e.HasMorePages = False

        ''Exit Sub

        ''==========================================================================================================================
        ''==========================================================================================================================


        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo)

                Try

                    NoofDets = 0

                    CurY = 200

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                p1Font = New Font("Calibri", 12, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + 930, CurY, 1, 0, p1Font)

                                NoofDets = NoofDets + 1

                                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, False)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Item_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 40 Then
                                For I = 40 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 40
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If



                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Sl_No").ToString), LMargin + 40, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 120, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString) & " - " & Trim(prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString), LMargin + 650, CurY, 1, 0, pFont)
                            ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 600, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 750, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 639, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)

                            CurY = CurY + TxtHgt

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + 75, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If



                    Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, CurY, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String, Led_CSTNo As String
        Dim Trans_Nm As String = ""
        Dim CurY As Single = 0
        Dim LedAr(10) As String
        Dim Indx As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_CSTNo = ""

            Led_Name = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
            'Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)
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

            'If Trim(Led_TinNo) <> "" Then
            '    Indx = Indx + 1
            '    LedAr(Indx) = "Tin No : " & Trim(Led_TinNo)
            'End If

            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            CurY = 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(1), 500, CurY, 0, 0, p1Font)
            strHeight = e.Graphics.MeasureString("A", p1Font).Height

            CurY = 20
            Common_Procedures.Print_To_PrintDocument(e, LedAr(2), 500, CurY, 0, 0, pFont)
            CurY = 40
            Common_Procedures.Print_To_PrintDocument(e, LedAr(3), 500, CurY, 0, 0, pFont)
            CurY = 60
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4), 500, CurY, 0, 0, pFont)
            'CurY = 70
            'Common_Procedures.Print_To_PrintDocument(e, LedAr(5), 500, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, LedAr(6), LMargin + 60, CurY, 0, 0, pFont)


            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            CurY = 115
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, 100, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), 630, CurY, 0, 0, pFont)



            'CurY = TMargin + 245
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + 500, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + 670, CurY, 0, 0, pFont)


            'CurY = TMargin + 265
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + 500, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + 670, CurY, 0, 0, pFont)


            'CurY = TMargin + 285
            'Common_Procedures.Print_To_PrintDocument(e, Led_TinNo, LMargin + 500, CurY, 0, 0, pFont)

            'CurY = TMargin + 305
            'Common_Procedures.Print_To_PrintDocument(e, Led_CSTNo, LMargin + 500, CurY, 0, 0, pFont)

            'CurY = TMargin + 325
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString, LMargin + 500, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal CurY As Single, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal PageHeight As Single, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String = "", Rup2 As String = "", Rup3 As String = ""
        Dim I As Integer
        ' Dim CurY As Single = 0

        Try

            If is_LastPage = True Then

                CurY = 270
                ' CurY = CurY + 10
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Discount (" & Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString) & " %)", LMargin + 520, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + 750, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin + 660, CurY, PrintWidth - 135, CurY)
                CurY = CurY - 10

                If Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount  ", LMargin + 520, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + 750, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "CGST (" & Val(prn_HdDt.Rows(0).Item("CGst_Percentage").ToString) & " %)  ", LMargin + 520, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + 750, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "SGST (" & Val(prn_HdDt.Rows(0).Item("SGst_Percentage").ToString) & " %)  ", LMargin + 520, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + 750, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "RoundOff  ", LMargin + 520, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + 750, CurY, 1, 0, pFont)
                End If

                CurY = 390
                e.Graphics.DrawLine(Pens.Black, LMargin + 20, CurY, PrintWidth - 135, CurY)

                CurY = 400
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 130, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + 600, CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + 750, CurY, 1, 0, p1Font)



                CurY = 420
                e.Graphics.DrawLine(Pens.Black, LMargin + 80, CurY, LMargin + 80, 150)
                e.Graphics.DrawLine(Pens.Black, LMargin + 530, CurY, LMargin + 530, 150)
                e.Graphics.DrawLine(Pens.Black, LMargin + 660, CurY, LMargin + 660, 150)

                If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
                    CurY = 440
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN - " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString, LMargin + 50, CurY, 0, 0, p1Font)
                End If


                'Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                'Rup2 = ""
                'Rup3 = ""
                'If Len(Rup1) > 55 Then
                '    For I = 55 To 1 Step -1
                '        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                '    Next I
                '    If I = 0 Then I = 55
                '    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                '    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                'End If

                'If Len(Rup2) > 55 Then
                '    For I = 55 To 1 Step -1
                '        If Mid$(Trim(Rup2), I, 1) = " " Then Exit For
                '    Next I
                '    If I = 0 Then I = 55
                '    Rup3 = Microsoft.VisualBasic.Right(Trim(Rup2), Len(Rup2) - I)
                '    Rup2 = Microsoft.VisualBasic.Left(Trim(Rup2), I - 1)
                'End If

                'CurY = CurY + TxtHgt + 7

                'If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + 400, CurY, 1, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                'End If

                'CurY = CurY + TxtHgt + 7
                ''Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 60, CurY, 0, 0, pFont)
                'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge :", LMargin + 400, CurY, 1, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                'End If

                'CurY = CurY + TxtHgt + 7
                ''Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 60, CurY, 0, 0, pFont)
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Add/Less :", LMargin + 400, CurY, 1, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                'End If

                'CurY = CurY + TxtHgt + 7
                ''Common_Procedures.Print_To_PrintDocument(e, Rup3, LMargin + 60, CurY, 0, 0, pFont)
                'If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + 400, CurY, 1, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                'End If

                'CurY = TMargin + 926
                'p1Font = New Font("Calibri", 14, FontStyle.Bold)
                'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + 775, CurY, 1, 0, p1Font)

            End If

            'CurY = TMargin + 995
            'Common_Procedures.Print_To_PrintDocument(e, Rup1, LMargin + 80, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 80, CurY, 0, 0, pFont)


            'CurY = TMargin + 1085
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + 80, CurY, 0, 0, pFont)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_code_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_code.KeyDown
        If e.KeyCode = 40 Then
            If Trim(UCase(txt_code.Text)) <> "" Then
                cbo_ItemName.Focus()
            Else
                txt_CashDiscPerc.Focus()
            End If
        End If
        ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_code_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_code.KeyPress

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(txt_code.Text)) <> "" Then


                If Trim(UCase(cbo_ItemName.Text)) = "" And Trim(UCase(txt_code.Text)) <> "" Then
                    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_code = '" & Trim(txt_code.Text) & "'", con)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        If IsDBNull(dt.Rows(0)("Item_name").ToString) = False Then
                            cbo_ItemName.Text = dt.Rows(0)("Item_name").ToString
                        End If
                        If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
                            cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                        End If
                        If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                            txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
                        End If

                    End If
                    dt.Dispose()
                    da.Dispose()
                End If
                cbo_ItemName.Focus()
            Else

                cbo_ItemName.Focus()

            End If
        End If

    End Sub

    Private Sub chk_Delivery_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_Delivery.KeyDown
        If e.KeyCode = 40 Then

            txt_DeliveryDate.Focus()

        End If

        If e.KeyCode = 38 Then
            cbo_PaymentMethod.Focus()
        End If

    End Sub

    Private Sub chk_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Delivery.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If txt_DeliveryDate.Visible And txt_DeliveryDate.Enabled Then
                txt_DeliveryDate.Focus()
            Else
                txt_code.Focus()
            End If

            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    dtp_Date.Focus()
            'End If

        End If
    End Sub

    Private Sub txt_Advance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Advance.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Advance_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Advance.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub Get_OrderDetails()
        Dim da As New SqlClient.SqlDataAdapter
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim SNO As Integer = 0
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                Itm_IdNo = Common_Procedures.Item_NameToIdNo(con, cbo_Filter_ItemName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Code IN (select z.Sales_Code from Sales_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            If Trim(cbo_FilterPaymentMode.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Payment_Method ='" & Trim(cbo_FilterPaymentMode.Text) & "'"
            End If


            If Trim(cbo_FilterDescription.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "b.Item_Description ='" & Trim(cbo_FilterDescription.Text) & "'"
            End If

            If Trim(cbo_FilterEntrySts.Text) <> "" Then
                If Trim(cbo_FilterEntrySts.Text) = "DELIVERED" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Delivery_Status =1"
                ElseIf Trim(cbo_FilterEntrySts.Text) = "NOT DELIVERED" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Delivery_Status =0"
                ElseIf Trim(cbo_FilterEntrySts.Text) = "PAID" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Received_Date <> ''"
                ElseIf Trim(cbo_FilterEntrySts.Text) = "NOT PAID" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Received_Date = ''"
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select a.* ,b.* ,tP.*,IH.*,UH.Unit_name ,VBH.* from Sales_Head a INNER JOIN Sales_Details B ON A.Sales_Code = B.Sales_Code LEFT OUTER JOIN Voucher_Bill_Head VBH ON VBH.Entry_Identification =  A.Sales_Code  LEFT OUTER JOIN Ledger_Head tP on b.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Item_Head IH ON b.Item_IdNo = IH.Item_IdNo LEFT OUTER JOIN Unit_Head UH ON b.Unit_IdNo = UH.Unit_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code like '" & Trim(Pk_Condition) & "%' and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            dt = New DataTable
            da.Fill(dt)

            dgv_Filter_Details.Rows.Clear()

            If dt.Rows.Count > 0 Then

                For i = 0 To dt.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    SNO = SNO + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = SNO
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Trim(dt.Rows(i).Item("Sales_No").ToString)
                    dgv_Filter_Details.Rows(n).Cells(2).Value = IIf(IsDate(Trim(dt.Rows(i).Item("Sales_Date").ToString)) = True, Format(Convert.ToDateTime(Trim(dt.Rows(i).Item("Sales_Date").ToString)), "dd/MM/yyyy"), "")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Trim(dt.Rows(i).Item("Ledger_Name").ToString)
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Trim(dt.Rows(i).Item("Ledger_PhoneNo").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Trim(dt.Rows(i).Item("Item_Name").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Trim(dt.Rows(i).Item("Item_Description").ToString)

                    dgv_Filter_Details.Rows(n).Cells(7).Value = IIf(Val(dt.Rows(i).Item("Bill_Amount").ToString) <> 0, Format(Val(dt.Rows(i).Item("Bill_Amount").ToString), "########0.00"), "")

                    '---Advance
                    da1 = New SqlClient.SqlDataAdapter("select sum(a.amount) as AdvAmount  from Voucher_Bill_Details a INNER JOIN Sales_Head b ON a.entry_identification = b.Sales_Code  where a.Voucher_Bill_Code = '" & Trim(dt.Rows(i).Item("Voucher_Bill_Code").ToString) & "' and b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.Sales_Code like '" & Trim(Pk_Condition) & "%' and b.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                    'da1 = New SqlClient.SqlDataAdapter("select sum(a.amount) as AdvAmount  from Voucher_Bill_Details a LEFT OUTER JOIN Sales_Head b ON a.entry_identification = b.Sales_Code  where a.Voucher_Bill_Code = '" & Trim(dt.Rows(i).Item("Voucher_Bill_Code").ToString) & "' and b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.Sales_Code like '" & Trim(Pk_Condition) & "%' and b.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                    dt1 = New DataTable
                    da1.Fill(dt1)
                    If dt1.Rows.Count > 0 Then
                        dgv_Filter_Details.Rows(n).Cells(8).Value = IIf(Val(dt1.Rows(0).Item("AdvAmount").ToString) <> 0, Format(Val(dt1.Rows(0).Item("AdvAmount").ToString), "########0.00"), "")
                    End If
                    dt1.Clear()
                    dt.Dispose()
                    da1.Dispose()


                    '---Receipt 

                    da1 = New SqlClient.SqlDataAdapter("select sum(a.amount) AS RecAmount  from Voucher_Bill_Details a  where a.entry_identification <>  '" & Trim(dt.Rows(i).Item("Sales_CODE").ToString) & " ' and  a.Voucher_Bill_Code = '" & Trim(dt.Rows(i).Item("Voucher_Bill_Code").ToString) & "'", con)
                    'da1 = New SqlClient.SqlDataAdapter("select sum(a.amount) AS RecAmount  from Voucher_Bill_Details a  where a.entry_identification <>  '" & Trim(dt.Rows(i).Item("Sales_CODE").ToString) & " ' and  a.Voucher_Bill_Code ='" & Trim(dt.Rows(i).Item("Voucher_Bill_Code").ToString) & "'  AND a.entry_identification like '" & Trim(Pk_Condition) & "%' and a.entry_identification like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                    dt1 = New DataTable
                    da1.Fill(dt1)
                    If dt1.Rows.Count > 0 Then
                        dgv_Filter_Details.Rows(n).Cells(9).Value = IIf(Val(dt1.Rows(0).Item("RecAmount").ToString) <> 0, Format(Val(dt1.Rows(0).Item("RecAmount").ToString), "########0.00"), "")
                    End If
                    dt1.Clear()
                    dt.Dispose()
                    da1.Dispose()

                    dgv_Filter_Details.Rows(n).Cells(10).Value = IIf((Val(dgv_Filter_Details.Rows(n).Cells(7).Value) - Val(dgv_Filter_Details.Rows(n).Cells(8).Value) - Val(dgv_Filter_Details.Rows(n).Cells(9).Value)) <> 0, Format(Val(dgv_Filter_Details.Rows(n).Cells(7).Value) - Val(dgv_Filter_Details.Rows(n).Cells(8).Value) - Val(dgv_Filter_Details.Rows(n).Cells(9).Value), "########0.00"), "")



                    If Trim(dt.Rows(i).Item("Received_Date").ToString) <> "" Then
                        dgv_Filter_Details.Rows(n).Cells(11).Value = IIf(IsDate(Trim(dt.Rows(i).Item("Received_Date").ToString)) = True, Format(Convert.ToDateTime(Trim(dt.Rows(i).Item("Received_Date").ToString)), "dd/MM/yyyy"), "")
                    End If
                    dgv_Filter_Details.Rows(n).Cells(12).Value = Trim(dt.Rows(i).Item("Payment_Method").ToString)
                    If Trim(dt.Rows(i).Item("Delivery_Date").ToString) <> "" Then
                        dgv_Filter_Details.Rows(n).Cells(13).Value = IIf(IsDate(Trim(dt.Rows(i).Item("Delivery_Date").ToString)) = True, Format(Convert.ToDateTime(Trim(dt.Rows(i).Item("Delivery_Date").ToString)), "dd/MM/yyyy"), "")
                    End If
                    dgv_Filter_Details.Rows(n).Cells(14).Value = False
                    dgv_Filter_Details.Rows(n).Cells(15).Value = ""   '
                    dgv_Filter_Details.Rows(n).Cells(16).Value = Trim(dt.Rows(i).Item("Item_Code").ToString)
                    dgv_Filter_Details.Rows(n).Cells(17).Value = Trim(dt.Rows(i).Item("Unit_name").ToString)
                    dgv_Filter_Details.Rows(n).Cells(18).Value = Trim(dt.Rows(i).Item("Sales_Code").ToString)
                Next i


                TotalAmount_Calculation_Filter()
                n = dgv_Filter_Details.Rows.Add()
                n = dgv_Filter_Details.Rows.Add()

                dgv_Filter_Details.Rows(n).Cells(6).Value = "TOTAL"
                dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dgv_FilterTotal.Rows(0).Cells(7).Value), "########0.00")
                dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dgv_FilterTotal.Rows(0).Cells(8).Value), "########0.00")
                dgv_Filter_Details.Rows(n).Cells(9).Value = Format(Val(dgv_FilterTotal.Rows(0).Cells(9).Value), "########0.00")
                dgv_Filter_Details.Rows(n).Cells(10).Value = Format(Val(dgv_FilterTotal.Rows(0).Cells(10).Value), "########0.00")

                dgv_Filter_Details.Rows(n).DefaultCellStyle.BackColor = Color.Khaki
                dgv_Filter_Details.Rows(n).DefaultCellStyle.ForeColor = Color.Black

            End If

            dt.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            dt.Dispose()
            da.Dispose()

        End Try


        chk_SMS_All.Checked = False

        If dgv_Filter_Details.Rows.Count > 0 Then
            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
        Else
            dtp_Filter_Fromdate.Focus()
        End If

    End Sub
    Private Sub txt_DeliveryDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DeliveryDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_DeliveryDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DeliveryDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_code.Focus()
        End If

    End Sub

    Private Sub txt_DeliveryDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DeliveryDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_DeliveryDate.Text = Date.Today
            txt_DeliveryDate.SelectAll()
        End If
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub btn_NewEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_NewEntry.Click
        pnl_Filter.Visible = False
        pnl_Back.Visible = True
        pnl_Back.Enabled = True
        new_record()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub lbl_receiptAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_receiptAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Amount_Received_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Amount_Received_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_Amount_Received_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Amount_Received_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Amount_Received_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Amount_Received_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_Amount_Received_Date.Text = Date.Today
            txt_Amount_Received_Date.SelectAll()
        End If
    End Sub
    Private Sub cbo_FilterDescription_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FilterDescription.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sales_Details", "Item_Description", "", "")
    End Sub

    Private Sub cbo_FilterDescription_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FilterDescription.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FilterDescription, cbo_FilterEntrySts, btn_save, "Sales_Details", "Item_Description", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_FilterDescription_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FilterDescription.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FilterDescription, btn_save, "Sales_Details", "Item_Description", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_FilterEntrySts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FilterEntrySts.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_FilterEntrySts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FilterEntrySts.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FilterEntrySts, cbo_FilterPaymentMode, cbo_FilterDescription, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_FilterEntrySts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FilterEntrySts.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FilterEntrySts, cbo_FilterDescription, "", "", "", "", True)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub dgv_Filter_Details_DataError(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs) Handles dgv_Filter_Details.DataError

        'MessageBox.Show("Error happened " & e.Context.ToString())

        If (e.Context = DataGridViewDataErrorContexts.Commit) Then
            MessageBox.Show("Commit error")
        End If
        If (e.Context = DataGridViewDataErrorContexts.CurrentCellChange) Then
            MessageBox.Show("Cell change")
        End If
        If (e.Context = DataGridViewDataErrorContexts.Parsing) Then
            MessageBox.Show("parsing error")
        End If
        If (e.Context = DataGridViewDataErrorContexts.LeaveControl) Then
            MessageBox.Show("leave control error")
        End If

        If (TypeOf (e.Exception) Is ConstraintException) Then

            Dim view As DataGridView = CType(sender, DataGridView)

            view.Rows(e.RowIndex).ErrorText = "an error"
            view.Rows(e.RowIndex).Cells(e.ColumnIndex).ErrorText = "an error"

            e.ThrowException = False
        End If
    End Sub

    Private Sub chk_SMS_All_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SMS_All.CheckedChanged


        Sms_Count = 0

        If chk_SMS_All.Checked = True Then
            If dgv_Filter_Details.Rows.Count > 0 Then
                For i = 0 To dgv_Filter_Details.Rows.Count - 3
                    dgv_Filter_Details.Rows(i).Cells(14).Value = True
                    Sms_Count = Sms_Count + 1
                Next
            End If
        Else
            If dgv_Filter_Details.Rows.Count > 0 Then
                For i = 0 To dgv_Filter_Details.Rows.Count - 3
                    dgv_Filter_Details.Rows(i).Cells(14).Value = False
                    Sms_Count = 0
                Next
            End If
        End If

        If Sms_Count = 0 Then
            btn_Sms.Text = "SMS"
        Else
            btn_Sms.Text = "SMS " & Sms_Count
        End If
    End Sub

    Private Sub btn_ReciptVoucher_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ReciptVoucher.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "Rcpt"
        '   Common_Procedures.Voucher_CR_Name = Trim(cbo_Ledger.Text)
        Common_Procedures.Voucher_Code = ""
        f.MdiParent = MDIParent1
        f.Show()
    End Sub

    Private Sub btn_Sms_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Sms.Click
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse = Nothing
        Dim url As String
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim timeout As Integer = 50000
        Dim Cn As Integer = 0


        Try
            If dgv_Filter_Details.Rows.Count > 0 Then
                For i = 0 To dgv_Filter_Details.Rows.Count - 3

                    url = ""

                    If CBool(dgv_Filter_Details.Rows(i).Cells(14).Value) = True Then

                        PhNo = dgv_Filter_Details.Rows(i).Cells(4).Value

                        smstxt = ""
                        smstxt = "Dear "
                        smstxt = smstxt & Trim(dgv_Filter_Details.Rows(i).Cells(3).Value) & ","
                        smstxt = smstxt & " Your Order Item " & Trim(dgv_Filter_Details.Rows(i).Cells(5).Value) & ""
                        smstxt = smstxt & " (" & Trim(dgv_Filter_Details.Rows(i).Cells(6).Value) & " )"
                        smstxt = smstxt & " is ready to pick. Please collect your material."

                        url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(SMSProvider_Key) & "&routeid=" & Trim(SMSProvider_RouteID) & "&type=" & Trim(SMSProvider_Type) & "&contacts=" & Trim(PhNo) & "&senderid=" & Trim(SMSProvider_SenderID) & "&msg=" & Trim(smstxt)


                        request = DirectCast(WebRequest.Create(url), HttpWebRequest)
                        request.KeepAlive = True

                        request.Timeout = timeout

                        response = DirectCast(request.GetResponse(), HttpWebResponse)


                        If Trim(UCase(response.StatusDescription)) = "OK" Then


                            Sms_Count = Sms_Count - 1
                            If Sms_Count = 0 Then
                                btn_Sms.Text = "SMS"
                            Else
                                btn_Sms.Text = "SMS " & Sms_Count
                            End If

                        Else
                            Continue For
                            'MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End If

                        response.Close()

                        response = Nothing
                        request = Nothing

                    End If


                Next

            End If


            chk_SMS_All.Checked = False

            If dgv_Filter_Details.Rows.Count > 0 Then
                For i = 0 To dgv_Filter_Details.Rows.Count - 3
                    dgv_Filter_Details.Rows(i).Cells(14).Value = False
                    Sms_Count = 0
                Next
            End If

            MessageBox.Show("SMS send successfully...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally


        End Try

    End Sub

    Private Sub dgv_Filter_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellValueChanged

    End Sub
    Private Sub DataGridView1SelectAll_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgv_Filter_Details.CurrentCellDirtyStateChanged


        RemoveHandler dgv_Filter_Details.CurrentCellDirtyStateChanged, AddressOf DataGridView1SelectAll_CurrentCellDirtyStateChanged

        If TypeOf dgv_Filter_Details.CurrentCell Is DataGridViewCheckBoxCell Then

            dgv_Filter_Details.EndEdit()

            Dim Checked As Boolean = CType(dgv_Filter_Details.CurrentCell.Value, Boolean)


            If Checked Then
                Sms_Count = Sms_Count + 1
            Else
                Sms_Count = Sms_Count - 1
            End If
        End If
        If Sms_Count = 0 Then
            btn_Sms.Text = "SMS"
        Else
            btn_Sms.Text = "SMS " & Sms_Count
        End If


        AddHandler dgv_Filter_Details.CurrentCellDirtyStateChanged, AddressOf DataGridView1SelectAll_CurrentCellDirtyStateChanged
    End Sub

    Private Sub txt_Amount_Received_Date_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Amount_Received_Date.TextChanged

    End Sub

    Private Sub chk_Delivery_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_Delivery.CheckedChanged
        If chk_Delivery.Checked = True Then
            txt_DeliveryDate.Enabled = True
        Else
            txt_DeliveryDate.Enabled = False
            txt_DeliveryDate.Text = ""
        End If
    End Sub



    Private Sub btn_SMS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS1.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", AgPNo As String = ""
        Dim Led_IdNo As Integer = 0, Agnt_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")


            smstxt = "Checking..."

            'SMS_SenderID = ""
            'SMS_Key = ""
            'SMS_RouteID = ""
            'SMS_Type = ""

            'Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)



            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.SMSProvider_SenderID = SMSProvider_SenderID
            Sms_Entry.SMSProvider_Key = SMSProvider_Key
            Sms_Entry.SMSProvider_RouteID = SMSProvider_RouteID
            Sms_Entry.SMSProvider_Type = SMSProvider_Type

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_CGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub txt_SGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub
End Class
