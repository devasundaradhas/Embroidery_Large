Public Class Invoice_Garments_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GINVG-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private vcmb_ItmNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private CFrm_STS As Integer
    Private prn_Status As Integer
    Private prn_DetIndx As Integer
    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Print.Visible = False
        pnl_GSTTax_Details.Visible = False
        txt_LrDate.Text = ""

        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""

        lbl_AmountInWords.Text = "Rupees  :  "

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Then
            chk_No_CForm.Checked = False
        Else
            chk_No_CForm.Checked = True
        End If

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_GSTTax_Details.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Add()

        cbo_PaymentMethod.Text = "CREDIT"

        cbo_TaxType.Text = "GST"
        txt_GrossAmount.Text = ""
        lbl_NetAmount.Text = ""
        'cbo_DocumentThrough.Text = "CREDIT"
        'cbo_Destination.Text = Common_Procedures.Ledger_IdNoToName(con, 22)
        'cbo_BookedBy.Text = Common_Procedures.Ledger_IdNoToName(con, 20)
        txt_SlNo.Text = "1"

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
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

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_Cell_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
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
        If FrmLdSTS = True Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sales_Code = '" & Trim(NewCode) & "'", con)
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
                cbo_Destination.Text = dt1.Rows(0).Item("Despatch_To").ToString
                cbo_BookedBy.Text = dt1.Rows(0).Item("Booked_By").ToString
                cbo_Transport.Text = Common_Procedures.Transport_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                If Val(dt1.Rows(0).Item("Freight_ToPay_Amount").ToString) <> 0 Then
                    txt_Freight_ToPay.Text = dt1.Rows(0).Item("Freight_ToPay_Amount").ToString
                End If
                cbo_DocumentThrough.Text = dt1.Rows(0).Item("Document_Through").ToString

                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_Weight.Text = Format(Val(dt1.Rows(0).Item("Weight").ToString), "########0.000")
                If Val(dt1.Rows(0).Item("Against_CForm_Status").ToString) = 1 Then chk_No_CForm.Checked = True


                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                txt_LrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString

                txt_Noof_Bags.Text = dt1.Rows(0).Item("Total_Bags").ToString

                txt_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")

                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                txt_Electronic_RefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                cbo_TransportMode.Text = dt1.Rows(0).Item("Transportation_Mode").ToString
                txt_DateTime_Of_Supply.Text = dt1.Rows(0).Item("Date_Time_Of_Supply").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Entry_GST_Tax_Type").ToString
                txt_TaxPerc.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc").ToString), "########0.00")
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")
                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")
                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGst_Amount").ToString), "########0.00")


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.Size_idno = c.Size_idno where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()
                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Size_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Perc_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Amount_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Assessable_Value").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("HSN_Code").ToString
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                    Next i

                End If
                TotalAmount_Calculation()
                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                da1 = New SqlClient.SqlDataAdapter("Select a.* from Sales_GST_Tax_Details a Where a.Sales_Code = '" & Trim(NewCode) & "' ", con)
                dt4 = New DataTable
                da1.Fill(dt4)

                With dgv_GSTTax_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = SNo
                            .Rows(n).Cells(1).Value = Trim(dt4.Rows(i).Item("HSN_Code").ToString)
                            .Rows(n).Cells(2).Value = IIf(Val(dt4.Rows(i).Item("Taxable_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("Taxable_Amount").ToString), "############0.00"), "")
                            .Rows(n).Cells(3).Value = IIf(Val(dt4.Rows(i).Item("CGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("CGST_Percentage").ToString), "")
                            .Rows(n).Cells(4).Value = IIf(Val(dt4.Rows(i).Item("CGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("CGST_Amount").ToString), "##########0.00"), "")
                            .Rows(n).Cells(5).Value = IIf(Val(dt4.Rows(i).Item("SGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("SGST_Percentage").ToString), "")
                            .Rows(n).Cells(6).Value = IIf(Val(dt4.Rows(i).Item("SGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("SGST_Amount").ToString), "###########0.00"), "")
                            .Rows(n).Cells(7).Value = IIf(Val(dt4.Rows(i).Item("IGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("IGST_Percentage").ToString), "")
                            .Rows(n).Cells(8).Value = IIf(Val(dt4.Rows(i).Item("IGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("IGST_Amount").ToString), "###########0.00"), "")
                        Next i

                    End If

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

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub Invoice_Garments_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
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

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"

        da = New SqlClient.SqlDataAdapter("select size_name from size_head order by size_name", con)
        da.Fill(dt3)
        cbo_Size.DataSource = dt3
        cbo_Size.DisplayMember = "size_name"

        da = New SqlClient.SqlDataAdapter("select Transport_Name from Transport_Head order by Transport_Name", con)
        da.Fill(dt4)
        cbo_Transport.DataSource = dt4
        cbo_Transport.DisplayMember = "Transport_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Despatch_To) from Sales_Head order by Despatch_To", con)
        da.Fill(dt5)
        cbo_Destination.DataSource = dt5
        cbo_Destination.DisplayMember = "Despatch_To"

        da = New SqlClient.SqlDataAdapter("select distinct(Booked_By) from Sales_Head order by Booked_By", con)
        da.Fill(dt6)
        cbo_BookedBy.DataSource = dt6
        cbo_BookedBy.DisplayMember = "Booked_By"

        da = New SqlClient.SqlDataAdapter("select distinct(Document_Through) from Sales_Head order by Document_Through", con)
        da.Fill(dt7)
        cbo_DocumentThrough.DataSource = dt7
        cbo_DocumentThrough.DisplayMember = "Document_Through"

        cbo_PaymentMethod.Items.Clear()
        cbo_PaymentMethod.Items.Add("CASH")
        cbo_PaymentMethod.Items.Add("CREDIT")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("NO TAX")
        cbo_TaxType.Items.Add("GST")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Then
            lbl_NoofBundles_Caption.Text = "No.of Bags"
        Else
            lbl_NoofBundles_Caption.Text = "No.of Bundles"
        End If

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Destination.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BookedBy.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_ToPay.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DocumentThrough.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noof_Bags.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateTime_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Electronic_RefNo.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Destination.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BookedBy.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_ToPay.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DocumentThrough.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noof_Bags.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Electronic_RefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateTime_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Noof_Bags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight_ToPay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DateTime_Of_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Electronic_RefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Electronic_RefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateTime_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight_ToPay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Noof_Bags.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
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

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_GSTTax_Details.Visible = True Then
                    btn_Close_GSTTax_Details_Click(sender, e)
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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code LIKE '" & Trim(Pk_Condition) & "%'   AND Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString

                    End If
                End If
            End If

            dr.Close()

            If movno <> 0 Then move_record(movno)

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

            da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code LIKE '" & Trim(Pk_Condition) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No", con)
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
            cmd.CommandText = "select Sales_No from Sales_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code LIKE '" & Trim(Pk_Condition) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc"

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
        Dim da As New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code LIKE '" & Trim(Pk_Condition) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc", con)
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

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "Sales_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select a.Document_Through, a.Tax_Type, a.Tax_Perc, a.Against_CForm_Status,a.Entry_GST_Tax_Type,a.Payment_Method, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Sales_Code LIKE '" & Trim(Pk_Condition) & "%' Order by a.for_Orderby desc, a.Sales_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString
                If dt2.Rows(0).Item("Document_Through").ToString <> "" Then cbo_DocumentThrough.Text = dt2.Rows(0).Item("Document_Through").ToString
                If dt2.Rows(0).Item("Payment_Method").ToString <> "" Then cbo_PaymentMethod.Text = dt2.Rows(0).Item("Payment_Method").ToString
                If dt2.Rows(0).Item("Tax_Perc").ToString <> "" Then txt_TaxPerc.Text = Val(dt2.Rows(0).Item("Tax_Perc").ToString)
                If dt2.Rows(0).Item("Against_CForm_Status").ToString <> "" Then
                    If Val(dt2.Rows(0).Item("Against_CForm_Status").ToString) = 1 Then chk_No_CForm.Checked = True
                End If
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

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Sales_Code LIKE '" & Trim(Pk_Condition) & "%' and Sales_Code = '" & Trim(NewCode) & "'"
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code LIKE '" & Trim(Pk_Condition) & "%' and Sales_Code = '" & Trim(NewCode) & "'"
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
        Dim led_id As Integer = 0
        Dim trans_id As Integer = 0
        Dim saleac_id As Integer = 0
        Dim txac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sz_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim vTot_Qty As Single = 0
        Dim CsParNm As String
        Dim VouBil As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sales_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

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
                MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

        trans_id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)
        If trans_id = 0 And Trim(cbo_Transport.Text) <> "" Then
            MessageBox.Show("Invalid Transport Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Transport.Enabled Then cbo_Transport.Focus()
            Exit Sub
        End If

        saleac_id = 0
        If saleac_id = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            saleac_id = 22
            'MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        txac_id = 0
        If txac_id = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
            txac_id = 20
            'MessageBox.Show("Invalid Tax A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(3).Value) <> 0 Then


                    itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If itm_id = 0 Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(2).Value) <> "" Then
                        Sz_id = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(2).Value)
                        If Sz_id = 0 Then
                            MessageBox.Show("Invalid Size Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(2)
                                .CurrentCell.Selected = True
                            End If
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(3).Value) = 0 Then
                        MessageBox.Show("Invalid Pcs", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        Amount_Calculation(True)
        'Total_Calculation()

        vTot_Qty = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTot_Qty = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
        End If

        If Val(vTot_Qty) = 0 Then
            MessageBox.Show("Invalid Invoice Pcs", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
            Exit Sub
        End If

        CFrm_STS = 0
        If chk_No_CForm.Checked = True Then CFrm_STS = 1


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "sales_Code like '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)

            If New_Entry = True Then


                cmd.CommandText = "Insert into Sales_Head(Entry_VAT_GST_Type , Sales_Code ,             Company_IdNo         ,              Sales_No             ,                               for_OrderBy                                  , Sales_Date,    Payment_Method                    ,        Ledger_IdNo   ,    Cash_PartyName  ,         SalesAc_IdNo      ,            TaxAc_IdNo    ,              Despatch_To            ,              Booked_By           ,         Transport_IdNo    ,            Freight_ToPay_Amount         ,              Document_Through           ,              Lr_No           ,               Lr_Date          ,               Total_Bags            ,             Total_Qty     ,               SubTotal_Amount         , Total_DiscountAmount, Total_TaxAmount,                Gross_Amount           ,               CashDiscount_Perc        ,               CashDiscount_Amount        ,             Assessable_Value         ,        Tax_Type          ,               Tax_Perc            ,                Tax_Amount           ,              Freight_Amount       ,              AddLess_Amount       ,               Round_Off            ,                Net_Amount                  ,              Order_No             ,                Order_Date         ,                Weight             , Against_CForm_Status      , Electronic_Reference_No   ,               Transportation_Mode     ,               Date_Time_Of_Supply          ,              Entry_GST_Tax_Type ,                 CGst_Amount          ,                 SGst_Amount          ,               IGst_Amount               ) " & _
                                    " Values (                   'GST'          ,  '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate,'" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ",'" & Trim(CsParNm) & "', " & Str(Val(saleac_id)) & ", " & Str(Val(txac_id)) & ", '" & Trim(cbo_Destination.Text) & "', '" & Trim(cbo_BookedBy.Text) & "', " & Str(Val(trans_id)) & ", " & Str(Val(txt_Freight_ToPay.Text)) & ", '" & Trim(cbo_DocumentThrough.Text) & "', '" & Trim(txt_LrNo.Text) & "', '" & Trim(txt_LrDate.Text) & "', " & Str(Val(txt_Noof_Bags.Text)) & ", " & Str(Val(vTot_Qty)) & ", " & Str(Val(txt_GrossAmount.Text)) & ",          0          ,        0       , " & Str(Val(txt_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", 'VAT'            , " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & " ,   '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_OrderDate.Text) & "', " & Str(Val(txt_Weight.Text)) & " ,   " & Str(Val(CFrm_STS)) & ", '" & Trim(txt_Electronic_RefNo.Text) & "', '" & Trim(cbo_TransportMode.Text) & "', '" & Trim(txt_DateTime_Of_Supply.Text) & "', '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Sales_Head set  Entry_VAT_GST_Type = 'GST' ,Sales_Date = @SalesDate,  Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "',Ledger_IdNo = " & Str(Val(led_id)) & ",Cash_PartyName = '" & Trim(CsParNm) & "', SalesAc_IdNo = " & Str(Val(saleac_id)) & ", TaxAc_IdNo = " & Str(Val(txac_id)) & ", Despatch_To = '" & Trim(cbo_Destination.Text) & "', Booked_By = '" & Trim(cbo_BookedBy.Text) & "', Transport_IdNo = " & Str(Val(trans_id)) & ", Freight_ToPay_Amount = " & Str(Val(txt_Freight_ToPay.Text)) & ", Document_Through = '" & Trim(cbo_DocumentThrough.Text) & "', Lr_No = '" & Trim(txt_LrNo.Text) & "', Lr_Date = '" & Trim(txt_LrDate.Text) & "', Total_Bags = " & Str(Val(txt_Noof_Bags.Text)) & ", Total_Qty = " & Str(Val(vTot_Qty)) & ", SubTotal_Amount = " & Str(Val(txt_GrossAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Tax_Type = 'VAT', Tax_Perc = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & " , Order_No =  '" & Trim(txt_OrderNo.Text) & "' , Order_Date = '" & Trim(txt_OrderDate.Text) & "',   Weight = " & Str(Val(txt_Weight.Text)) & " , Against_CForm_Status =   " & Str(Val(CFrm_STS)) & " ,Electronic_Reference_No = '" & Trim(txt_Electronic_RefNo.Text) & "' ,  Transportation_Mode = '" & Trim(cbo_TransportMode.Text) & "'  ,  Date_Time_Of_Supply = '" & Trim(txt_DateTime_Of_Supply.Text) & "'  , Entry_GST_Tax_Type = '" & Trim(cbo_TaxType.Text) & "',  CGst_Amount = " & Str(Val(lbl_CGstAmount.Text)) & " , SGst_Amount = " & Str(Val(lbl_SGstAmount.Text)) & " , IGst_Amount = " & Str(Val(lbl_IGstAmount.Text)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            With dgv_Details

                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then

                        itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        unt_id = Common_Procedures.get_FieldValue(con, "Item_Head", "Unit_IdNo", "(Item_IdNo = " & Str(Val(itm_id)) & ")", , tr)

                        Sz_id = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Sales_Details ( Sales_Code,            Company_IdNo          ,              Sales_No             ,                                              for_OrderBy                   , Sales_Date,          Ledger_IdNo    ,            SL_No     ,          Item_IdNo      ,          Unit_IdNo      ,            Size_IdNo   ,               Noof_Items                 ,                 Rate                     ,                 Amount                   ,                    Total_Amount          ,   Cash_Discount_Perc_For_All_Item    ,       Cash_Discount_Amount_For_All_Item  ,              Assessable_Value            ,                      HSN_Code           ,                      Tax_Perc              ) " & _
                                                " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(Sz_id)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", '" & Trim(.Rows(i).Cells(9).Value) & "', " & Str(Val(.Rows(i).Cells(10).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Item_Processing_Details (  Reference_Code      ,               Company_IdNo       ,            Reference_No           ,                                 for_OrderBy                                , Reference_Date,        Ledger_IdNo     ,            Party_Bill_No          ,           SL_No      ,           Item_IdNo     ,            Unit_IdNo    ,                    Quantity                               ,          Size_IdNo ) " & _
                                                " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",    @SalesDate, " & Str(Val(led_id)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(-1 * Val(dgv_Details.Rows(i).Cells(3).Value)) & " ,   " & Str(Val(Sz_id)) & "  )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With
            cmd.CommandText = "Delete from Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_GSTTax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Sales_GST_Tax_Details   (        Sales_Code      ,               Company_IdNo       ,                Sales_No           ,                               for_OrderBy                                  , Sales_Date ,         Ledger_IdNo     ,            Sl_No     ,                    HSN_Code            ,                      Taxable_Amount      ,                      CGST_Percentage     ,                      CGST_Amount         ,                      SGST_Percentage      ,                      SGST_Amount         ,                      IGST_Percentage     ,                      IGST_Amount          ) " & _
                                            "          Values                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate , " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            Dim vVouPos_IdNos As String = "", vVouPos_Amts As String = "", vVouPos_ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                AcPos_ID = 1
            Else
                AcPos_ID = led_id
            End If

            Dim vNetAmt As String = Format(Val(CSng(lbl_NetAmount.Text)), "#############0.00")

            '---GST
            vVouPos_IdNos = AcPos_ID & "|" & saleac_id & "|" & txac_id & "|" & "25|26|27|9|17|24"

            vVouPos_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(lbl_TaxAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(txt_RoundOff.Text)) & "|" & Val(lbl_TaxAmount.Text) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(txt_Freight.Text) & "|" & Val(txt_AddLess.Text) & "|" & Val(txt_RoundOff.Text)

            If Common_Procedures.Voucher_Updation(con, "Gst.Sales", Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Bill No . : " & Trim(lbl_InvoiceNo.Text), vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If

            '---Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, led_id, Trim(lbl_InvoiceNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'Ac_id = led_id

            'cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " & _
            '                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SaleSV', @SalesDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SaleSV', @SalesDate, 1, " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SaleSV', @SalesDate, 2, " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text)) - Val(lbl_VatAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'If Val(lbl_TaxAmount.Text) <> 0 Then
            '    cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SaleSV', @SalesDate, 3, " & Str(Val(txac_id)) & ", " & Str(Val(lbl_VatAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            '    cmd.ExecuteNonQuery()
            'End If

            tr.Commit()

            move_record(lbl_InvoiceNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            tr.Dispose()
            cmd.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try



    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim itm_id As Integer = 0
        Dim Sz_id As Integer = 0

        itm_id = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

        If Val(itm_id) = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        If Trim(cbo_Size.Text) <> "" Then
            Sz_id = Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text)
            If Val(Sz_id) = 0 Then
                MessageBox.Show("Invalid Size", "DOES NOT ADD...", MessageBoxButtons.OK)
                If cbo_Size.Enabled Then cbo_Size.Focus()
                Exit Sub
            End If
        End If

        If Val(txt_Pcs.Text) = 0 Then
            MessageBox.Show("Invalid Pcs", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Pcs.Enabled Then txt_Pcs.Focus()
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If

        If Val(lbl_Amount.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(2).Value = cbo_Size.Text
                    .Rows(i).Cells(3).Value = Val(txt_Pcs.Text)
                    .Rows(i).Cells(4).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(5).Value = Format(Val(lbl_Amount.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")

                    .Rows(i).Cells(8).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                    .Rows(i).Cells(9).Value = lbl_Grid_HsnCode.Text
                    .Rows(i).Cells(10).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")
                    ' .Rows(i).Selected = True

                    MtchSTS = True

                    ' If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = cbo_Size.Text
                .Rows(n).Cells(3).Value = Val(txt_Pcs.Text)
                .Rows(n).Cells(4).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(5).Value = Format(Val(lbl_Amount.Text), "########0.00")
                .Rows(n).Cells(6).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")

                .Rows(n).Cells(8).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                .Rows(n).Cells(9).Value = lbl_Grid_HsnCode.Text
                .Rows(n).Cells(10).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")
                '.Rows(n).Selected = True

                'If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Size.Text = ""
        txt_Pcs.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""
        Grid_Cell_DeSelect()
        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Pcs.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus

        With cbo_ItemName

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
            cbo_ItemName.Tag = cbo_ItemName.Text
        End With

    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_SlNo, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_ItemName.Text) <> "" Then
                cbo_Size.Focus()
            Else
                cbo_TaxType.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
                get_Item_Unit_Rate_TaxPerc()
            End If

            If Trim(cbo_ItemName.Text) <> "" Then
                SendKeys.Send("{TAB}")
            Else
                txt_Freight.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
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
        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            get_Item_Unit_Rate_TaxPerc()
        End If
    End Sub
    Private Sub get_Item_Unit_Rate_TaxPerc()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            cbo_ItemName.Tag = cbo_ItemName.Text
            da = New SqlClient.SqlDataAdapter("select a.*  from item_head a  where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                    txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
                End If
                get_Item_Tax(False)
            End If
            dt.Dispose()
            da.Dispose()
        End If

    End Sub
    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_Freight.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_DocumentThrough_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DocumentThrough.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DocumentThrough, txt_OrderDate, cbo_PaymentMethod, "", "", "", "")
    End Sub

    Private Sub cbo_DocumentThrough_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DocumentThrough.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DocumentThrough, cbo_PaymentMethod, "", "", "", "")
    End Sub



    Private Sub cbo_BookedBy_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BookedBy.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BookedBy, txt_LrDate, txt_Noof_Bags, "", "", "", "")
    End Sub

    Private Sub cbo_BookedBy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BookedBy.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BookedBy, txt_Noof_Bags, "", "", "", "")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If

        cbo_Ledger.Tag = cbo_Ledger.Text
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                cbo_Ledger.Tag = cbo_Ledger.Text
                Amount_Calculation(True)
            End If
        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_Destination, txt_LrNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_LrNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Transport_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Destination_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Destination.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Destination, cbo_PaymentMethod, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub cbo_Destination_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Destination.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Destination, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Trim(cbo_TaxType.Text) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then
            e.Handled = True

        Else
            If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True

        End If
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_GrossAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Noof_Bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Noof_Bags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : cbo_ItemName.Focus()
        If e.KeyCode = 38 Then e.Handled = True : txt_Freight_ToPay.Focus()

    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then
            cbo_ItemName.Focus()
            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        cbo_ItemName.Text = Trim(.Rows(i).Cells(1).Value)
                        cbo_Size.Text = Trim(.Rows(i).Cells(2).Value)
                        txt_Pcs.Text = Val(.Rows(i).Cells(3).Value)
                        txt_Rate.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.00")
                        lbl_Grid_DiscPerc.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.00")
                        lbl_Grid_DiscAmount.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.00")

                        lbl_Grid_AssessableValue.Text = Format(Val(.Rows(i).Cells(8).Value), "########0.00")
                        lbl_Grid_HsnCode.Text = Trim(dgv_Details.CurrentRow.Cells(9).Value)
                        lbl_Grid_GstPerc.Text = Format(Val(.Rows(i).Cells(10).Value), "########0.00")
                        Exit For

                    End If

                Next

            End With

        End If
    End Sub

    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "size_head", "size_name", "", "(Size_IdNo = 0)")
    End Sub

    Private Sub cbo_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, cbo_ItemName, txt_Pcs, "size_head", "size_name", "", "(size_idno = 0)")
    End Sub

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, txt_Pcs, "size_head", "size_name", "", "(size_idno = 0)")
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Code IN (select z.Sales_Code from Sales_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Sales_No, a.Sales_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

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

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
            cbo_Size.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            txt_Pcs.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(4).Value), "########0.00")
            lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
            lbl_Grid_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
            lbl_Grid_DiscAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")

            lbl_Grid_AssessableValue.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
            lbl_Grid_HsnCode.Text = Trim(dgv_Details.CurrentRow.Cells(9).Value)
            lbl_Grid_GstPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
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
        cbo_Size.Text = ""
        txt_Pcs.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_GstPerc.Text = ""
        lbl_Grid_HsnCode.Text = ""
        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

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
            cbo_Size.Text = ""
            txt_Pcs.Text = ""
            txt_Rate.Text = ""
            lbl_Amount.Text = ""
            lbl_Grid_DiscPerc.Text = ""
            lbl_Grid_DiscAmount.Text = ""
            lbl_Grid_AssessableValue.Text = ""
            lbl_Grid_GstPerc.Text = ""
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

    Private Sub txt_OrderDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_OrderDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_OrderDate.Text = Date.Today
            txt_OrderDate.SelectionStart = txt_OrderDate.Text.Length
        End If
    End Sub

    Private Sub txt_LrNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LrNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : txt_LrDate.Focus()
        If e.KeyCode = 38 Then e.Handled = True : cbo_Transport.Focus()
    End Sub

    Private Sub txt_LrNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LrNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_LrDate.Focus()
        End If
    End Sub

    Private Sub txt_LrDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LrDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then e.Handled = True : cbo_BookedBy.Focus()
        If e.KeyCode = 38 Then e.Handled = True : txt_LrNo.Focus()
    End Sub

    Private Sub txt_LrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_LrDate.Text = Date.Today
            txt_LrDate.SelectionStart = txt_LrDate.Text.Length
        End If
    End Sub

    Private Sub txt_Electronic_RefNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Electronic_RefNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : txt_SlNo.Focus()
        If e.KeyCode = 38 Then e.Handled = True : txt_DateTime_Of_Supply.Focus()
    End Sub

    Private Sub txt_Freight_ToPay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight_ToPay.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    txt_SlNo.Focus()
        'End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
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

        lbl_Amount.Text = Format(Val(txt_Pcs.Text) * Val(txt_Rate.Text), "#########0.00")

    End Sub

    Private Sub GrossAmount_Calculation()
        Dim I As Integer
        Dim Sno As Integer
        Dim TotQty As Decimal, TotAmt As Decimal

        Sno = 0
        TotQty = 0
        TotAmt = 0

        With dgv_Details

            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                dgv_Details.Rows(i).Cells(0).Value = Sno

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(3).Value) <> 0 Then

                    TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(3).Value)
                    TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(5).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .Rows.Count = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotQty)
            .Rows(0).Cells(5).Value = Format(Val(TotAmt), "########0.00")
        End With

        txt_GrossAmount.Text = Format(Val(TotAmt), "########0.00")

        NetAmount_Calculation()

    End Sub

    'Private Sub NetAmount_Calculation()
    '    Dim NtAmt As Decimal

    '    txt_CashDiscAmount.Text = Format(Val(txt_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

    '    lbl_Assessable.Text = Format(Val(txt_GrossAmount.Text) - Val(txt_CashDiscAmount.Text), "#########0.00")

    '    lbl_VatAmount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_VatPerc.Text) / 100, "#########0.00")

    '    NtAmt = Val(txt_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(lbl_VatAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

    '    lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

    '    txt_RoundOff.Text = Format(Val(lbl_NetAmount.Text) - Val(NtAmt), "#########0.00")

    '    lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

    '    lbl_AmountInWords.Text = "Rupees  :  "
    '    If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
    '        lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
    '    End If

    'End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Then
        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_Preprint.Enabled And btn_Print_Preprint.Visible Then
            btn_Print_Preprint.Focus()
        End If

        'Else
        '    printing_invoice()

        'End If
    End Sub

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim CmpName As String

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

        CmpName = Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)



            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Then
            'Printing_Format5Gst(e)
            If prn_Status = 1 Then
                'If Microsoft.VisualBasic.Left(Trim(UCase(prn_HdDt.Rows(0).Item("Company_Name").ToString)), 3) = "BNC" And Microsoft.VisualBasic.InStr(1, Trim(UCase(prn_HdDt.Rows(0).Item("Company_Name").ToString)), "GARMENT") > 0 Then
                'Printing_Format1(e)
                Printing_Format5Gst(e)
            Else
                'Printing_Format2(e)
                Printing_Format5Gst(e)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Then

            If prn_Status = 1 Then

                Printing_Format3(e)

            Else
                Printing_Format4(e)

            End If

        Else
            Printing_Format2(e)

        End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ps As Printing.PaperSize

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
            If ps.Width = 900 And ps.Height = 1200 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 0
            .Bottom = 0
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top + 50
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
        'pFont = New Font("Calibri", 10, FontStyle.Regular)

        pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        NoofItems_PerPage = 18

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo)

                Try

                    NoofDets = 0

                    CurY = 425

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                p1Font = New Font("Calibri", 12, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + 760, CurY, 1, 0, p1Font)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, False)

                                e.HasMorePages = True
                                Return

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

                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 40, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + 350, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 500, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + 585, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + 40, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Trans_Nm As String = ""
        Dim CurY As Single = 0
        Dim LedAr(10) As String
        Dim Indx As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = ""

            Led_Name = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
            'Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)
            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
            Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)

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

            If Trim(Led_TinNo) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = "Tin No : " & Trim(Led_TinNo)
            End If
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            CurY = TMargin + 125  ' 130
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 591, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 720, 140, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 620, 140, 0, 0, p1Font)
            CurY = TMargin + 180
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 590, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 720, 190, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 620, 190, 0, 0, pFont)

            CurY = TMargin + 105  ' 110
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

            'CurY = TMargin + 110
            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, Led_Name, LMargin + 60, CurY, 0, 0, p1Font)
            'strHeight = e.Graphics.MeasureString("A", p1Font).Height
            'CurY = CurY + strHeight - 0.5
            'Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + 60, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 0.5
            'Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + 60, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 0.5
            'Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + 60, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 0.5
            'Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + 60, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 0.5
            'If Trim(Led_TinNo) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & Led_TinNo, LMargin + 60, CurY, 0, 0, pFont)
            'End If

            CurY = TMargin + 220 ' 228
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + 100, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + 400, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Booked_By").ToString, LMargin + 660, CurY, 0, 0, pFont)

            CurY = TMargin + 250 ' 259
            Trans_Nm = Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString))
            Common_Procedures.Print_To_PrintDocument(e, Trans_Nm, LMargin + 100, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString), "#########0.00"), LMargin + 400, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Document_Through").ToString, LMargin + 660, CurY, 0, 0, pFont)

            CurY = TMargin + 280
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + 100, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + 400, CurY, 0, 0, pFont)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal PageHeight As Single, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String, Rup3 As String
        Dim I As Integer
        Dim CurY As Single = 0

        Try

            If is_LastPage = True Then

                CurY = TMargin + 785

                e.Graphics.DrawLine(Pens.Black, LMargin + 502, CurY, LMargin + 592, CurY)

                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 250, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + 585, CurY, 1, 0, pFont)

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

                CurY = TMargin + 840
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt + 7
                Common_Procedures.Print_To_PrintDocument(e, Rup1, LMargin + 60, CurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + 500, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, "VAT @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + 500, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7
                Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 60, CurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight :", LMargin + 500, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7
                Common_Procedures.Print_To_PrintDocument(e, Rup3, LMargin + 60, CurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + 500, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                End If

                CurY = TMargin + 957 ' 975
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + 760, CurY, 1, 0, p1Font)

            End If
            'p1Font = New Font("Calibri", 8, FontStyle.Regular)
            'Common_Procedures.Print_To_PrintDocument(e, ".End.", LMargin + 10, PageHeight - 15, 0, 0, p1Font)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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
            .Top = 40 ' 60
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

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 13  ' 12

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(55)
        ClArr(2) = 290 : ClArr(3) = 90 : ClArr(4) = 85 : ClArr(5) = 95
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

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

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 25, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
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

                    Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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


            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = ""

            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)   ' Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)
            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
            Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)

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

            If Trim(Led_TinNo) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = "Tin No : " & Trim(Led_TinNo)
            End If

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2)
            HdWd = PageWidth - Cen1 - LMargin

            H1 = e.Graphics.MeasureString("TO    :", pFont).Width
            W1 = e.Graphics.MeasureString("DATE    :", pFont).Width

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            'p1Font = New Font("Calibri", 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + Cen1, CurY - 10, 2, HdWd, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Led_Name, LMargin + H1 + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY + 10, PageWidth, CurY + 10)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(2), LMargin + H1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO.", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "NO.    :  " & prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(3), LMargin + H1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4), LMargin + H1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DATE    :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(5), LMargin + H1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(Led_TinNo) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAr(6), LMargin + H1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, LnAr(3), LMargin + Cen1, LnAr(2))

            C1 = ClAr(1) + ClAr(2) - 85
            C2 = 50 + ClAr(3) + ClAr(4) - 10
            C3 = PageWidth - (C1 + C2)

            W1 = e.Graphics.MeasureString("Destination    :", pFont).Width
            W2 = e.Graphics.MeasureString("No. of Bags   :", pFont).Width
            W3 = e.Graphics.MeasureString("Document Through  :", pFont).Width


            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Destination", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "B.Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Booked by", LMargin + C1 + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Booked_By").ToString, LMargin + C1 + C2 + W3 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Trans_Nm = Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString))
            Common_Procedures.Print_To_PrintDocument(e, "Trans Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trans_Nm, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "To pay Rs.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString), "#########0.00"), LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Document Through", LMargin + C1 + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Document_Through").ToString, LMargin + C1 + C2 + W3 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "L.R.No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "No.of Bags", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If
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

            'CurY = CurY + TxtHgt - 10 + 10
            'Common_Procedures.Print_To_PrintDocument(e, "Assessable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'End If

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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Against_CForm_Status").ToString) <> 0 Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Against C Form", LMargin + 20, CurY, 0, 0, p1Font)
            End If

            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            'p1Font = New Font("Calibri", 15, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
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

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Rupees : " & Rup1, LMargin + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

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
        Dim ps As Printing.PaperSize
        Dim sno As Integer = 0
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
            If ps.Width = 900 And ps.Height = 1200 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 0  ' 50
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

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 10, FontStyle.Regular)

        pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        NoofItems_PerPage = 16

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo)

                Try

                    NoofDets = 0

                    CurY = 430 ' 435 ' 450

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                p1Font = New Font("Calibri", 12, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + 760, CurY, 1, 0, p1Font)

                                NoofDets = NoofDets + 1

                                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, False)

                                e.HasMorePages = True
                                Return

                            End If
                            sno = sno + 1
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

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("SL_No").ToString), LMargin + 52, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 95, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + 390, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 520, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + 610, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)


                            'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("SL_No").ToString), LMargin + 40, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 80, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + 365, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 520, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + 610, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 730, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + 95, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, True)


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
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Trans_Nm As String = "", TransNm1 As String = "", TransNm2 As String = ""
        Dim CurY As Single = 0
        Dim LedAr(10) As String
        Dim Indx As Integer = 0
        Dim W1 As Single = 0, W2 As Single = 0, W3 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim LM As Single
        Dim i As Integer = 0

        PageNo = PageNo + 1

        CurY = TMargin

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = ""

            Led_Name = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
            'Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)
            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
            Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)

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

            If Trim(Led_TinNo) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = "Tin No : " & Trim(Led_TinNo)
            End If
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            CurY = TMargin + 196  ' 185 ' 200
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 620, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 720, 140, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 620, 140, 0, 0, p1Font)
            CurY = TMargin + 242 ' 235 ' 245
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 620, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 720, 190, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 620, 190, 0, 0, pFont)

            CurY = TMargin + 165 ' 150  ' 110
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/S " & LedAr(1), LMargin + 60, CurY, 0, 0, p1Font)
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

            'CurY = TMargin + 110
            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, Led_Name, LMargin + 60, CurY, 0, 0, p1Font)
            'strHeight = e.Graphics.MeasureString("A", p1Font).Height
            'CurY = CurY + strHeight - 0.5
            'Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + 60, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 0.5
            'Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + 60, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 0.5
            'Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + 60, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 0.5
            'Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + 60, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt - 0.5
            'If Trim(Led_TinNo) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & Led_TinNo, LMargin + 60, CurY, 0, 0, pFont)
            'End If

            CurY = TMargin + 280 ' 270 ' 280 

            W1 = e.Graphics.MeasureString("Destination  :", pFont).Width
            W2 = e.Graphics.MeasureString("Document Through  :", pFont).Width
            'W2 = e.Graphics.MeasureString("Freight To pay Rs.:", pFont).Width
            W3 = e.Graphics.MeasureString("No.of Bundles  :", pFont).Width

            LM = LMargin + 10

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Order No", LM + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LM + W1 + 45, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Order Date", LM + 270 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + 270 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LM + 270 + W2 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Weight", LM + 530 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + 530 + W3 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Weight").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Weight").ToString), "#########0.000"), LM + 530 + W3 + 25, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Destination", LM + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LM + W1 + 45, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "L.R.No.", LM + 270 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + 270 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LM + 270 + W2 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "L.R Date", LM + 530 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + 530 + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LM + 530 + W3 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5

            Trans_Nm = Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString))

            TransNm1 = Trans_Nm
            TransNm2 = ""
            If Len(TransNm1) > 18 Then
                For i = 18 To 1 Step -1
                    If Mid$(Trim(TransNm1), i, 1) = " " Then Exit For
                Next i
                If i = 0 Then i = 18
                TransNm2 = Microsoft.VisualBasic.Right(Trim(TransNm1), Len(TransNm1) - i)
                TransNm1 = Microsoft.VisualBasic.Left(Trim(TransNm1), i - 1)
            End If


            Common_Procedures.Print_To_PrintDocument(e, "Trans Name", LM + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, TransNm1, LM + W1 + 45, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Freight To pay Rs.", LM + 270 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + 270 + W2 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString), "#########0.00"), LM + 270 + W2 + 25, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "No.of Bundles", LM + 530 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + 530 + W3 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LM + 530 + W3 + 25, CurY, 0, 0, pFont)
            End If

            If Trim(TransNm2) <> "" Then
                CurY = CurY + TxtHgt - 8
                Common_Procedures.Print_To_PrintDocument(e, TransNm2, LM + W1 + 45, CurY, 0, 0, pFont)
                CurY = CurY - 5
            End If

            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Booked by", LM + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Booked_By").ToString, LM + W1 + 45, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Documents Through", LM + 270 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LM + 270 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Document_Through").ToString, LM + 270 + W2 + 25, CurY, 0, 0, pFont)
            '***

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal PageHeight As Single, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String, Rup3 As String
        Dim I As Integer
        Dim CurY As Single = 0

        Try

            If is_LastPage = True Then

                CurY = TMargin + 812 ' 810

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + 610, CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)

                CurY = CurY + 10
                CurY = CurY + TxtHgt + 15 ' 7
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "VAT @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + 500, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight :", LMargin + 500, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7
                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + 500, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = TMargin + 940 ' 940 
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + 740, CurY, 1, 0, p1Font)


                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                Rup2 = ""
                Rup3 = ""
                If Len(Rup1) > 75 Then
                    For I = 75 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 75
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If

                'If Len(Rup2) > 55 Then
                '    For I = 55 To 1 Step -1
                '        If Mid$(Trim(Rup2), I, 1) = " " Then Exit For
                '    Next I
                '    If I = 0 Then I = 55
                '    Rup3 = Microsoft.VisualBasic.Right(Trim(Rup2), Len(Rup2) - I)
                '    Rup2 = Microsoft.VisualBasic.Left(Trim(Rup2), I - 1)
                'End If
                CurY = TMargin + 990 ' 975 ' 980
                Common_Procedures.Print_To_PrintDocument(e, Rup1, LMargin + 120, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 120, CurY, 0, 0, pFont)

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ps As Printing.PaperSize

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 30 '60
            .Right = 60
            .Top = 50 ' 60
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

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 12 ' 12

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(55)
        ClArr(2) = 290 : ClArr(3) = 90 : ClArr(4) = 85 : ClArr(5) = 95
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

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

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 25, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
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

                    Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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


            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = ""

            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)   ' Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString)
            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
            Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)

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

            If Trim(Led_TinNo) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = "Tin No : " & Trim(Led_TinNo)
            End If

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2)
            HdWd = PageWidth - Cen1 - LMargin

            H1 = e.Graphics.MeasureString("TO    :", pFont).Width
            W1 = e.Graphics.MeasureString("DATE    :", pFont).Width

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            'p1Font = New Font("Calibri", 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin + Cen1, CurY - 10, 2, HdWd, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Led_Name, LMargin + H1 + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY + 10, PageWidth, CurY + 10)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(2), LMargin + H1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO.", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "NO.    :  " & prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(3), LMargin + H1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4), LMargin + H1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DATE    :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(5), LMargin + H1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(Led_TinNo) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAr(6), LMargin + H1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, LnAr(3), LMargin + Cen1, LnAr(2))

            C1 = ClAr(1) + ClAr(2) - 85
            C2 = 50 + ClAr(3) + ClAr(4) + 30  ' - 10
            C3 = PageWidth - (C1 + C2)

            W1 = e.Graphics.MeasureString("Destination  :", pFont).Width
            W2 = e.Graphics.MeasureString("Document Through  :", pFont).Width
            'W2 = e.Graphics.MeasureString("Freight To pay Rs.:", pFont).Width
            W3 = e.Graphics.MeasureString("No.of Bundles  :", pFont).Width


            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Order No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Order Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Weight", LMargin + C1 + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + W3 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Weight").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Weight").ToString), "#########0.000"), LMargin + C1 + C2 + W3 + 25, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Destination", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "L.R.No.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "L.R Date", LMargin + C1 + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + C2 + W3 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Trans_Nm = Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString))
            Common_Procedures.Print_To_PrintDocument(e, "Trans Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trans_Nm, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Freight To pay Rs.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString), "#########0.00"), LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "No.of Bundles", LMargin + C1 + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + W3 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + C1 + C2 + W3 + 25, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "Booked by", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Booked_By").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Documents Through", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Document_Through").ToString, LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer

        Try

            For I = NoofDets + 1 To (NoofItems_PerPage + 1)
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If
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

            'CurY = CurY + TxtHgt - 10 + 10
            'Common_Procedures.Print_To_PrintDocument(e, "Assessable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'End If

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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            'p1Font = New Font("Calibri", 15, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10

            If Val(prn_HdDt.Rows(0).Item("Against_CForm_Status").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Against C Form", LMargin + 20, CurY, 0, 0, p1Font)
            End If

            p1Font = New Font("Calibri", 13, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
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

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Rupees : " & Rup1, LMargin + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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

            AssVal_Frgt_Othr_Charges = Val(txt_Freight.Text)

            cmd.Connection = con

            cmd.CommandText = "Truncate table EntryTemp"
            cmd.ExecuteNonQuery()

            With dgv_Details

                If .Rows.Count > 0 Then
                    For i = 0 To .Rows.Count - 1

                        If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(9).Value) <> "" And Val(.Rows(i).Cells(10).Value) <> 0 Then

                            cmd.CommandText = "Insert into EntryTemp (                    Name1                ,                   Currency1            ,                       Currency2                                      ) " & _
                                              "            Values    ( '" & Trim(.Rows(i).Cells(9).Value) & "', " & (Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value) + AssVal_Frgt_Othr_Charges) & " ) "
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

    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, cbo_DocumentThrough, cbo_Destination, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, cbo_Destination, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub cbo_PaymentMethod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.LostFocus
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If
    End Sub

    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, cbo_TaxType, txt_DateTime_Of_Supply, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, txt_DateTime_Of_Supply, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_Freight_ToPay, cbo_TransportMode, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_TransportMode, "", "", "", "", True)
            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
                    cbo_TaxType.Tag = cbo_TaxType.Text
                    Amount_Calculation(True)
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        Amount_Calculation(True)
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub Amount_Calculation(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0



        If FrmLdSTS = True Then Exit Sub

        If GridAll_Row_STS = True Then

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        ItmIdNo = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                        If ItmIdNo <> 0 Then

                            .Rows(i).Cells(9).Value = ""
                            .Rows(i).Cells(10).Value = ""

                            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                                dt = New DataTable
                                da.Fill(dt)
                                If dt.Rows.Count > 0 Then

                                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                                        .Rows(i).Cells(9).Value = dt.Rows(0)("Item_HSN_Code").ToString
                                    End If
                                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                                        .Rows(i).Cells(10).Value = Format(Val(dt.Rows(0)("Item_GST_Percentage").ToString), "#########0.00")
                                    End If

                                End If
                                dt.Clear()

                            End If

                            .Rows(i).Cells(5).Value = Format(Val(.Rows(i).Cells(3).Value) * Val(.Rows(i).Cells(4).Value), "#########0.00")
                            .Rows(i).Cells(6).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
                            .Rows(i).Cells(7).Value = Format(Val(.Rows(i).Cells(5).Value) * Val(.Rows(i).Cells(6).Value) / 100, "#########0.00")
                            .Rows(i).Cells(8).Value = Format(Val(.Rows(i).Cells(5).Value) - Val(.Rows(i).Cells(7).Value), "#########0.00")

                        End If

                    End If

                Next

            End With

            TotalAmount_Calculation()

        Else

            lbl_Amount.Text = Format(Val(txt_Pcs.Text) * Val(txt_Rate.Text), "#########0.00")
            lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
            lbl_Grid_DiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(lbl_Grid_DiscPerc.Text) / 100, "#########0.00")
            lbl_Grid_AssessableValue.Text = Format(Val(lbl_Amount.Text) - Val(lbl_Grid_DiscAmount.Text), "#########0.00")

        End If



    End Sub
    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer = 0
        Dim TotQty As Decimal = 0
        Dim TotGrsAmt As Decimal = 0
        Dim TotDiscAmt As Decimal = 0
        Dim TotAssval As Decimal = 0
        Dim TotCGstAmt As Decimal = 0
        Dim TotSGstAmt As Decimal = 0
        Dim TotIGstAmt As Decimal = 0

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotQty = 0
        TotGrsAmt = 0
        TotDiscAmt = 0
        TotAssval = 0

        For i = 0 To dgv_Details.RowCount - 1

            Sno = Sno + 1

            dgv_Details.Rows(i).Cells(0).Value = Sno

            If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then
                TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(3).Value)
                TotGrsAmt = TotGrsAmt + Val(dgv_Details.Rows(i).Cells(5).Value)

                '***** GST START *****
                TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(7).Value)
                TotAssval = TotAssval + Val(dgv_Details.Rows(i).Cells(8).Value)
                '***** GST END *****

            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotQty)
            .Rows(0).Cells(5).Value = Format(Val(TotGrsAmt), "########0.00")
            '***** GST START *****
            .Rows(0).Cells(7).Value = Format(Val(TotDiscAmt), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotAssval), "########0.00")
            '***** GST END *****
        End With

        txt_GrossAmount.Text = Format(TotGrsAmt, "########0.00")
        txt_CashDiscAmount.Text = Format(TotDiscAmt, "########0.00")

        '***** GST START *****
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

        Gross_Discount_Tax_Amount_Calculation()

        '***** GST END *****

    End Sub

    Private Sub Gross_Discount_Tax_Amount_Calculation()
        '***** GST START *****
        If FrmLdSTS = True Then Exit Sub

        lbl_TaxAmount.Text = ""
        If Trim(UCase(cbo_TaxType.Text)) = "VAT" Then
            lbl_TaxAmount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_TaxPerc.Text) / 100, "#########0.00")
        End If
        NetAmount_Calculation()
        '***** GST END *****
    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        If FrmLdSTS = True Then Exit Sub

        NtAmt = Val(txt_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(lbl_TaxAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        txt_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")

        lbl_AmountInWords.Text = "Rupees :                                                                               "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees : : " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

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

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Amount_Calculation(True)
        End If
    End Sub

    Private Sub txt_TaxPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.TextChanged
        Gross_Discount_Tax_Amount_Calculation()
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

    Private Sub txt_Electronic_RefNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Electronic_RefNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_SlNo.Focus()
        End If
    End Sub

    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer, NoofDets As Integer
        Dim p1Font As Font
        Dim p2Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim NoofItems_Increment As Integer
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String

        Try

            TxtHgt = TxtHgt - 1

            p2Font = New Font("Calibri", 9, FontStyle.Regular)

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 140 : SubClAr(2) = 130 : SubClAr(3) = 60 : SubClAr(4) = 95 : SubClAr(5) = 60 : SubClAr(6) = 90 : SubClAr(7) = 60
            SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0

                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

                    ItmNm2 = ""
                    If Len(ItmNm1) > 40 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 40
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If



                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)
                    prn_DetIndx = prn_DetIndx + 1
                Loop

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            BmsInWrds = ""
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")


            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount (In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub


    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            NoofHsnCodes = Dt1.Rows.Count
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da.Dispose()

        get_GST_Noof_HSN_Codes_For_Printing = NoofHsnCodes

    End Function


    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If Val(Dt2.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                        TaxPerc = Val(Dt2.Rows(0).Item("IGST_Percentage").ToString)
                    Else
                        TaxPerc = Val(Dt2.Rows(0).Item("CGST_Percentage").ToString)
                    End If
                End If
                Dt2.Clear()

            End If
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

        get_GST_Tax_Percentage_For_Printing = Format(Val(TaxPerc), "#########0.00")

    End Function
    Private Sub Printing_Format5Gst(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
            .Top = 40 ' 60
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

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 10  ' 12

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35)
        ClArr(2) = 230 : ClArr(3) = 90 : ClArr(4) = 75 : ClArr(5) = 50 : ClArr(6) = 80 : ClArr(7) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format5Gst_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format5Gst_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(DetIndx).Item("Item_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 28 Then
                                For I = 28 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 28
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 20, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 30, CurY, 1, 0, pFont)

                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)


                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format5Gst_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format5Gst_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
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
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        p1Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        'CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

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
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST END *****

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                '***** GST START *****
                If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
                If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
                If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))
                '***** GST END *****

            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                '***** GST START *****
                Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

                Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
                '***** GST END *****

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
            '***** GST START *****
            If Trim(Led_State) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_State
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If

            If Trim(Led_GSTTinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_GSTTinNo
            End If

            'If Trim(Led_TinNo) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_TinNo
            'End If
            '***** GST END *****

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

            '***** GST START *****
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '***** GST END *****


            '------------------- Invoice No Block

            '***** GST START *****
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)


            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Electronic Ref.No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            End If


            BlockInvNoY = BlockInvNoY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY + 10, PageWidth, BlockInvNoY + 10)

            BlockInvNoY = BlockInvNoY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Order No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If



            BlockInvNoY = BlockInvNoY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Order Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If


            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Issue", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + Cen1 + W1 + 50, BlockInvNoY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, LnAr(3), LMargin + Cen1, LnAr(2))

            C1 = ClAr(1) + ClAr(2) - 40
            C2 = 50 + ClAr(3) + ClAr(4) - 10
            C3 = PageWidth - (C1 + C2)

            W1 = e.Graphics.MeasureString("Destination    :", pFont).Width
            W2 = e.Graphics.MeasureString("No. of Bags   :", pFont).Width
            W3 = e.Graphics.MeasureString("Document Through  :", pFont).Width


            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Destination", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Lr.Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Booked by", LMargin + C1 + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Booked_By").ToString, LMargin + C1 + C2 + W3 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            Trans_Nm = Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString))
            Common_Procedures.Print_To_PrintDocument(e, "Trans Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trans_Nm, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "To pay Rs.", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_ToPay_Amount").ToString), "#########0.00"), LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Document Through", LMargin + C1 + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + C2 + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Document_Through").ToString, LMargin + C1 + C2 + W3 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "L.R.No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "No.of Bags", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + C1 + W2 + 25, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format5Gst_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BnkDetAr() As String
        Dim BInc As Integer
        Dim CurY1 As Integer = 0
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            Else
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

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

            'CurY = CurY + TxtHgt - 10 + 10
            'Common_Procedures.Print_To_PrintDocument(e, "Assessable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'End If


            CurY1 = CurY
            CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 10, CurY1, 2, 0, p1Font)

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 20, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 20, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 20, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 20, CurY1, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Against_CForm_Status").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt + 10
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Against C Form", LMargin + 20, CurY1, 0, 0, p1Font)
            End If

            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                End If
            End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If CurY1 > CurY Then CurY = CurY1

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            End If

      
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

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

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            If vNoofHsnCodes <> 0 Then
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            End If




            CurY = CurY + TxtHgt - 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)



            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

End Class
