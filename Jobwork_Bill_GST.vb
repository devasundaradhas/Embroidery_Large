Public Class Jobwork_Bill_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GSALE-"
    Private Pk_Condition_TDS As String = "GSALT-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private vcmb_ItmNm As String
    Private vcmb_SizNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetMxIndx As Integer

    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_DetDt1 As New DataTable
    Private prn_DetIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_DetAr(100, 50, 10) As String
    Private prn_OriDupTri As String = ""
    Private prn_Status As Integer = 0

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
        pnl_Selection.Visible = False
        pnl_GSTTax_Details.Visible = False

        txt_DcDate.Text = ""
        txt_CashDiscPerc.Text = ""
        txt_CashDiscAmount.Text = ""
        txt_VechileNo.Text = ""
        cbo_OnAccount.Text = ""
        cbo_DeliveryTo.Text = ""
        txt_place_Supply.Text = ""
        lbl_NetAmount.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_Assessable.Text = ""

        cbo_EntType.Text = "DIRECT"
        txt_NoOfBundels.Text = ""

        Panel2.Enabled = True
        cbo_EntType.Enabled = True
        lbl_AmountInWords.Text = "Rupees  :  "

        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        lbl_Grid_GstPerc.Text = ""
        cbo_RateFor.Text = "PC"

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


        cbo_EntType.Text = "DIRECT"
        dgv_GSTTax_Details.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Add()


        cbo_PaymentMethod.Text = "CREDIT"
        cbo_TaxType.Text = "GST"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        lbl_GrossAmount.Text = ""
        lbl_NetAmount.Text = ""
        lbl_NetAmount_TDS.Text = ""
        lbl_TdsAmount.Text = ""

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

        Grid_Cell_DeSelect()
        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        '    Grid_Cell_DeSelect()
        'End If

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

                cbo_Transport.Text = Common_Procedures.Transport_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_OnAccount.Text = Common_Procedures.Ledger_IdnoToAlaisName(con, Val(dt1.Rows(0).Item("OnAc_IdNo").ToString))
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdnoToAlaisName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_Dcno.Text = dt1.Rows(0).Item("Dc_No").ToString

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Then  '---  GOLDEN TEXTILE
                    txt_DcDate.Text = dt1.Rows(0).Item("Party_Dc_No").ToString
                Else
                    txt_DcDate.Text = dt1.Rows(0).Item("Dc_Date").ToString
                End If

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                'cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                txt_VechileNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString

                txt_Electronic_RefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                cbo_TransportMode.Text = dt1.Rows(0).Item("Transportation_Mode").ToString
                txt_DateTime_Of_Supply.Text = dt1.Rows(0).Item("Date_Time_Of_Supply").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Entry_GST_Tax_Type").ToString
                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")
                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGst_Amount").ToString), "########0.00")
                txt_place_Supply.Text = dt1.Rows(0).Item("Place_Of_Supply").ToString

                txt_TDS_Percentage.Text = Format(Val(dt1.Rows(0).Item("Tds_Percentage").ToString), "########0.00")
                lbl_TdsAmount.Text = Format(Val(dt1.Rows(0).Item("Tds_Amount").ToString), "########0.00")
                lbl_NetAmount_TDS.Text = Format(Val(dt1.Rows(0).Item("Net_Amount_Tds").ToString), "########0.00")

                txt_NoOfBundels.Text = Val(dt1.Rows(0).Item("NoOf_Bundle").ToString)

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name ,sz.size_name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on a.Unit_idno = c.Unit_idno  LEFT OUTER JOIN Size_Head Sz ON a.Size_Idno = sz.size_idno where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Item_Description").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Unit_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Trim(dt2.Rows(i).Item("Rate_For").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Sales_Delivery_Code").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Sales_Delivery_Detail_SlNo").ToString
                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Sales_Detail_SlNo").ToString

                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Perc_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Amount_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Assessable_Value").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("HSN_Code").ToString
                        dgv_Details.Rows(n).Cells(16).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(17).Value = dt2.Rows(i).Item("Order_No").ToString
                        dgv_Details.Rows(n).Cells(18).Value = dt2.Rows(i).Item("Order_Date").ToString

                        dgv_Details.Rows(n).Cells(17).Value = dt2.Rows(i).Item("Challan_No").ToString
                        dgv_Details.Rows(n).Cells(18).Value = dt2.Rows(i).Item("Challan_Date").ToString

                    Next i

                End If

                TotalAmount_Calculation()
                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)


                dt2.Clear()
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
            End If

            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da2.Dispose()

            dt1.Dispose()
            da1.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub SalesEntry_YarnBill_GST_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_OnAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_OnAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RateFor.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_RateFor.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub SalesEntry_YarnBill_GST_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.SalesEntryType)) = "LABOUR INVOICE" Then
            Pk_Condition = "LBINV-"
            lbl_Title.Text = "LABOUR INVOICE"
        Else
            Pk_Condition = "GSALE-"
            lbl_Title.Text = "TAX INVOICE"
        End If


        txt_Weight.Visible = True
        dgv_Details.Columns(5).Visible = True
        dgv_Details_Total.Columns(5).Visible = True
        lbl_WeightCap.Visible = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1141" Then  '---  SRI SABARI TRADERS (TIRUPUR) 
            txt_Weight.Visible = False
            lbl_WeightCap.Visible = False
            dgv_Details.Columns(6).Visible = False
            dgv_Details_Total.Columns(6).Visible = False

            lbl_QuantityCap.Left = lbl_WeightCap.Left - 30
            txt_Quantity.Width = txt_Quantity.Width + txt_Weight.Width
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1174" Then  '---  WINTECH
            dgv_Details.Columns(4).HeaderText = "SIZE"
            
        End If

        lbl_DcDate.Text = "DC Date"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Then  '---  GOLDEN TEXTILE
            lbl_DcDate.Text = "Party DC NO"
        End If

        cbo_PaymentMethod.Items.Clear()
        cbo_PaymentMethod.Items.Add("CASH")
        cbo_PaymentMethod.Items.Add("CREDIT")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("NO TAX")
        cbo_TaxType.Items.Add("GST")

        cbo_RateFor.Items.Clear()
        cbo_RateFor.Items.Add("")
        cbo_RateFor.Items.Add("PC")
        cbo_RateFor.Items.Add("KG")


        cbo_TransportMode.Items.Clear()
     

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        cbo_EntType.Items.Clear()
        cbo_EntType.Items.Add("")
        cbo_EntType.Items.Add("DIRECT")
        cbo_EntType.Items.Add("DELIVERY")


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OnAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_place_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Dcno.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Quantity.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Electronic_RefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateTime_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VechileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfBundels.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Dcno.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OnAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_place_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Quantity.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Electronic_RefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateTime_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Description.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VechileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfBundels.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Electronic_RefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DateTime_Of_Supply.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Quantity.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_place_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Dcno.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Description.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VechileNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_place_Supply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Dcno.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Quantity.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VechileNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Electronic_RefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateTime_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

        If Trim(Common_Procedures.settings.CustomerCode) = "1502" Or Trim(Common_Procedures.settings.CustomerCode) = "1504" Or Trim(Common_Procedures.settings.CustomerCode) = "1505" Then ' Create Prints ,GOLDEN TEXTILE PRINTER . PRINT TECH
            lbl_NetAmountCaption.Text = "Bill AMount"

            lbl_NetAmount_TDS.Visible = True
            lbl_TDS_PerCaption.Visible = True
            lbl_TdsAmount.Visible = True
            lbl_NetAmountcaptionTDS.Visible = True
            lbl_TdsCaption.Visible = True
            lbl_TdsCaption.Text = "TDS Amount"
            txt_NoOfBundels.Visible = False
            txt_TDS_Percentage.Visible = True

            cbo_RateFor.Visible = False
            Label9.Visible = False
            txt_Quantity.Left = cbo_RateFor.Left
            txt_Quantity.Width = txt_Quantity.Width + cbo_RateFor.Width
            txt_Weight.Visible = False
            lbl_WeightCap.Visible = False
            Label11.Left = Label11.Left - 50
            txt_Rate.Left = txt_Weight.Left
            txt_Rate.Width = txt_Rate.Width + txt_Weight.Width - 50
            lbl_Amount.Left = lbl_Amount.Left - 50
            lbl_Amount.Width = lbl_Amount.Width + 50
        End If
        If Trim(Common_Procedures.settings.CustomerCode) = "1174" Then ' Win Tec
            txt_NoOfBundels.Visible = True
            lbl_TdsCaption.Visible = True
            lbl_TdsCaption.Text = "No Of Bundles"
            txt_TDS_Percentage.Visible = False
            lbl_TdsAmount.Visible = False
            lbl_TDS_PerCaption.Visible = False

        End If

    End Sub

    Private Sub SalesEntry_YarnBill_GST_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub SalesEntry_YarnBill_GST_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_GSTTax_Details.Visible = True Then
                    btn_Close_GSTTax_Details_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        If Pk_Condition = "LBINV-" Then
            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        Else
            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If


        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con

            cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = a.Receipt_Quantity - b.Noof_Items from Sales_dELIVERY_Details a, Sales_Details b where b.Sales_Code = '" & Trim(NewCode) & "' and b.Entry_Type = 'DELIVERY' and a.Sales_Delivery_Code = b.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = b.Sales_Delivery_Detail_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
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

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then


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
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No"
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            cmd.CommandText = "select Sales_No from Sales_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc"

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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "(Sales_Code like '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select  a.* from Sales_Head a  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Sales_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString
                If dt2.Rows(0).Item("Payment_Method").ToString <> "" Then cbo_PaymentMethod.Text = dt2.Rows(0).Item("Payment_Method").ToString
                If dt2.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_EntType.Text = dt2.Rows(0).Item("Entry_Type").ToString
                If dt2.Rows(0).Item("Tds_Percentage").ToString <> "" Then txt_TDS_Percentage.Text = dt2.Rows(0).Item("Tds_Percentage").ToString

            End If

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
                MessageBox.Show("Invoice No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        If Pk_Condition = "LBINV-" Then
            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        Else
            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        End If

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR INSERTION...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

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
                    MessageBox.Show("Invalid Invoice No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim OnAcc_id As Integer = 0
        Dim DelTo_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sz_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim vTot_Qty As Single = 0
        Dim vTot_Wgt As Single = 0
        Dim itm_GrpId As Integer = 0
        Dim VouType As String = ""
        Dim CsParNm As String
        Dim QtyOrWgt As Integer = 0


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Pk_Condition = "LBINV-" Then
            If Common_Procedures.UserRight_Check(Common_Procedures.UR.Labour_Sales_Entry, New_Entry) = False Then Exit Sub
        Else
            If Common_Procedures.UserRight_Check(Common_Procedures.UR.Tax_Sales_Entry, New_Entry) = False Then Exit Sub
        End If


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        ' Sz_id = Common_Procedures.Size_NameToIdNo(con, Trim(cbo_RateFor.Text))

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

        If (Trim(UCase(cbo_EntType.Text)) <> "DIRECT" And Trim(UCase(cbo_EntType.Text)) <> "DELIVERY") Then
            MessageBox.Show("Invalid Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If

        trans_id = Common_Procedures.Transport_NameToIdNo(con, Trim(cbo_Transport.Text))
        If trans_id = 0 And Trim(cbo_Transport.Text) <> "" Then
            MessageBox.Show("Invalid Transport Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Transport.Enabled Then cbo_Transport.Focus()
            Exit Sub
        End If
        OnAcc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_OnAccount.Text)
        DelTo_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        saleac_id = 0
        If saleac_id = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            saleac_id = 22
            'MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If itm_id = 0 Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(3).Value) <> "" Then
                        unt_id = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(3).Value)
                        If unt_id = 0 Then
                            MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(3)
                                .CurrentCell.Selected = True
                            End If
                            Exit Sub
                        End If
                    End If
    
                    If Val(.Rows(i).Cells(5).Value) = 0 And Val(.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Quantity / Weight", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(7)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        Amount_Calculation(True)

        vTot_Qty = 0 : vTot_Wgt = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTot_Qty = Val(dgv_Details_Total.Rows(0).Cells(5).Value)
            vTot_Wgt = Val(dgv_Details_Total.Rows(0).Cells(6).Value)
        End If

        If Trim(Common_Procedures.settings.CustomerCode) <> "1503" Then
            If Val(vTot_Qty) = 0 Then
                MessageBox.Show("Invalid Invoice Quantity", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
                Exit Sub
            End If
        End If
       


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "(Sales_Code LIKE '" & Trim(Pk_Condition) & "%' )", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                If Trim(txt_DateTime_Of_Supply.Text) = "" Then txt_DateTime_Of_Supply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")

                cmd.CommandText = "Insert into Sales_Head(Sales_Code ,             Company_IdNo         ,              Sales_No             ,                               for_OrderBy                                  , Sales_Date,           Payment_Method          ,       Ledger_IdNo   ,       Cash_PartyName  ,        SalesAc_IdNo      ,      Transport_IdNo    ,              Dc_No           ,               Dc_Date          ,             Total_Qty     ,               SubTotal_Amount                       , Total_DiscountAmount, Total_TaxAmount,                Gross_Amount           ,               CashDiscount_Perc        ,               CashDiscount_Amount        ,             Assessable_Value         ,                        Freight_Amount         ,              AddLess_Amount       ,               Round_Off            ,                Net_Amount                  ,              Order_No             ,                Order_Date         ,       Entry_Type                          ,     Electronic_Reference_No   ,               Transportation_Mode     ,               Date_Time_Of_Supply          ,              Entry_GST_Tax_Type ,                 CGst_Amount          ,                 SGst_Amount          ,               IGst_Amount   ,Vehicle_No                 ,       Place_Of_Supply             ,    OnAc_IdNo      , Total_Weight               ,  DeliveryTo_IdNo                      ,Tds_Percentage                      ,Tds_Amount                     ,Net_Amount_Tds                     ,Party_Dc_No                     ,NoOf_Bundle) " & _
                                    " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate,'" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ",'" & Trim(CsParNm) & "', " & Str(Val(saleac_id)) & ", " & Str(Val(trans_id)) & ",  '" & Trim(txt_Dcno.Text) & "', '" & Trim(txt_DcDate.Text) & "',  " & Str(Val(vTot_Qty)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ",          0          ,        0       , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & " ,   '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_OrderDate.Text) & "',  '" & Trim(cbo_EntType.Text) & "','" & Trim(txt_Electronic_RefNo.Text) & "', '" & Trim(cbo_TransportMode.Text) & "', '" & Trim(txt_DateTime_Of_Supply.Text) & "', '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & " ,'" & Trim(txt_VechileNo.Text) & "','" & Trim(txt_place_Supply.Text) & "'," & Val(OnAcc_id) & "  ," & Val(vTot_Wgt) & " ," & Val(DelTo_id) & "," & Val(txt_TDS_Percentage.Text) & "," & Val(lbl_TdsAmount.Text) & "," & Val(lbl_NetAmount_TDS.Text) & ",'" & Trim(txt_DcDate.Text) & "' , " & Val(txt_NoOfBundels.Text) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Sales_Head set Sales_Date = @SalesDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", Cash_PartyName = '" & Trim(CsParNm) & "', SalesAc_IdNo = " & Str(Val(saleac_id)) & ", Transport_IdNo = " & Str(Val(trans_id)) & ", Dc_No = '" & Trim(txt_Dcno.Text) & "', Dc_Date = '" & Trim(txt_DcDate.Text) & "', Total_Qty = " & Str(Val(vTot_Qty)) & ", SubTotal_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ",Vehicle_No='" & Trim(txt_VechileNo.Text) & "', Entry_Type = '" & Trim(cbo_EntType.Text) & "' , Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & " , Order_No =  '" & Trim(txt_OrderNo.Text) & "' , Order_Date = '" & Trim(txt_OrderDate.Text) & "', Electronic_Reference_No = '" & Trim(txt_Electronic_RefNo.Text) & "' ,  Transportation_Mode = '" & Trim(cbo_TransportMode.Text) & "'  ,  Date_Time_Of_Supply = '" & Trim(txt_DateTime_Of_Supply.Text) & "'  , Entry_GST_Tax_Type = '" & Trim(cbo_TaxType.Text) & "',  CGst_Amount = " & Str(Val(lbl_CGstAmount.Text)) & " , SGst_Amount = " & Str(Val(lbl_SGstAmount.Text)) & " , IGst_Amount = " & Str(Val(lbl_IGstAmount.Text)) & " ,Place_Of_Supply = '" & Trim(txt_place_Supply.Text) & "',OnAc_IdNo = " & Val(OnAcc_id) & "  ,Total_Weight = " & Val(vTot_Wgt) & " ,DeliveryTo_IdNo = " & Val(DelTo_id) & ",Tds_Percentage  =" & Val(txt_TDS_Percentage.Text) & ",Tds_Amount =" & Val(lbl_TdsAmount.Text) & "   ,Net_Amount_Tds = " & Val(lbl_NetAmount_TDS.Text) & " ,Party_Dc_No = '" & Trim(txt_DcDate.Text) & "' , NoOf_Bundle = " & Val(txt_NoOfBundels.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = a.Receipt_Quantity - b.Noof_Items from Sales_dELIVERY_Details a, Sales_Details b where b.Sales_Code = '" & Trim(NewCode) & "' and b.Entry_Type = 'DELIVERY' and a.Sales_Delivery_Code = b.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = b.Sales_Delivery_Detail_SlNo"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0
            Dim nr As Integer
            With dgv_Details

                For i = 0 To dgv_Details.RowCount - 1

                    If Trim(Common_Procedures.settings.CustomerCode) <> "1503" Then
                        QtyOrWgt = 5
                    Else
                        QtyOrWgt = 6
                    End If

                    If Val(dgv_Details.Rows(i).Cells(QtyOrWgt).Value) <> 0 Then

                        itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        itm_GrpId = Common_Procedures.Item_NameToItemGroupIdNo(con, .Rows(i).Cells(1).Value, tr)
                        unt_id = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        'Sz_id = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Sales_Details ( Sales_Code,            Company_IdNo          ,              Sales_No             ,                                              for_OrderBy                   , Sales_Date,          Ledger_IdNo    ,            Sl_No     ,          Item_IdNo      ,     ItemGroup_IdNo         ,           Item_Description             ,          Unit_IdNo      ,                       Noof_Items         ,                      Weight              ,                      Rate                ,                      Amount               ,                      Total_Amount         ,               Entry_Type         ,                    Sales_Delivery_Code  ,               Sales_dELIVERY_Detail_SlNo  ,      Cash_Discount_Perc_For_All_Item     ,       Cash_Discount_Amount_For_All_Item   ,              Assessable_Value             ,                    HSN_Code             ,                      Tax_Perc              , Rate_For                                ,Order_No                                ,Order_Date                               ,Challan_No                              , Challan_Date) " & _
                                                " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(itm_GrpId)) & ",'" & Trim(.Rows(i).Cells(2).Value) & "' , " & Str(Val(unt_id)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " , " & Str(Val(.Rows(i).Cells(8).Value)) & " , '" & Trim(cbo_EntType.Text) & "' , '" & Trim(.Rows(i).Cells(9).Value) & "' ,  " & Str(Val(.Rows(i).Cells(10).Value)) & "," & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & ", '" & Trim(.Rows(i).Cells(15).Value) & "', " & Str(Val(.Rows(i).Cells(16).Value)) & " , '" & Trim(.Rows(i).Cells(4).Value) & "','" & Trim(.Rows(i).Cells(17).Value) & "','" & Trim(.Rows(i).Cells(18).Value) & "' ,'" & Trim(.Rows(i).Cells(19).Value) & "','" & Trim(.Rows(i).Cells(20).Value) & "') "
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then

                            cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = Receipt_Quantity + " & Str(Val(.Rows(i).Cells(5).Value)) & " where Sales_Delivery_Code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Sales_Delivery_Detail_SlNo = " & Str(Val(.Rows(i).Cells(10).Value)) & " and Ledger_IdNo = " & Str(Val(led_id))
                            nr = cmd.ExecuteNonQuery()

                            If nr = 0 Then
                                tr.Rollback()
                                MessageBox.Show("Mismatch of Delivery and Party details", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                                Exit Sub
                            End If

                        End If
                        If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                            cmd.CommandText = "Insert into Item_Processing_Details (  Reference_Code      ,               Company_IdNo       ,            Reference_No           ,                                 for_OrderBy                                , Reference_Date,        Ledger_IdNo     ,            Party_Bill_No          ,           SL_No      ,           Item_IdNo     ,            Unit_IdNo    ,                    Quantity                               ) " & _
                                                    "      Values                  ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",    @SalesDate, " & Str(Val(led_id)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(-1 * Val(dgv_Details.Rows(i).Cells(5).Value)) & "   )"
                            cmd.ExecuteNonQuery()
                        End If
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

            If Trim(UCase(Pk_Condition)) = "LBINV-" Then
                VouType = "Gst.Invoice"
            Else
                VouType = "Gst.Sales"
            End If

            Dim vVouPos_IdNos As String = "", vVouPos_Amts As String = "", vVouPos_ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                AcPos_ID = 1
            Else
                AcPos_ID = led_id
            End If

            Dim vNetAmt As String = Format(Val(CSng(lbl_NetAmount.Text)), "#############0.00")

            '---GST
            vVouPos_IdNos = AcPos_ID & "|" & saleac_id & "|25|26|27|9|17|24"

            vVouPos_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(txt_RoundOff.Text)) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(txt_Freight.Text) & "|" & Val(txt_AddLess.Text) & "|" & Val(txt_RoundOff.Text)

            If Common_Procedures.Voucher_Updation(con, Trim(VouType), Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Bill No . : " & Trim(lbl_InvoiceNo.Text), vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If


            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition_TDS) & Trim(NewCode), tr)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1502" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1505" Then '---- Create pRints ,GOLDEN TEXTILE PRINTER ,PRINT TECH
                vVouPos_IdNos = ""
                vVouPos_Amts = ""
                vVouPos_ErrMsg = ""

                vVouPos_IdNos = AcPos_ID & "|" & Val(Common_Procedures.CommonLedger.TDS_Charges_Ac)
                vVouPos_Amts = Val(CSng(lbl_TdsAmount.Text)) & "|" & -1 * Val(CSng(lbl_TdsAmount.Text))

                If Common_Procedures.Voucher_Updation(con, "Job.Tds", Val(lbl_Company.Tag), Trim(Pk_Condition_TDS) & Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "", vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                    Throw New ApplicationException(vVouPos_ErrMsg)
                End If
            End If


            tr.Commit()

            move_record(lbl_InvoiceNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Err.Description, "CK_Sales_Delivery_Details_1") > 0 Then
                MessageBox.Show("Invalid Invoice Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Err.Description, "CK_Sales_Delivery_Details_2") > 0 Then
                MessageBox.Show("Invalid Invoice Quantity, Invoice Quantity must be lesser than Delivery Quantity", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

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
        Dim Unit_id As Integer = 0
        Dim Sz_id As Integer = 0
        itm_id = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

        If Val(itm_id) = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        If Trim(cbo_Unit.Text) <> "" Then
            Unit_id = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)
            If Val(Unit_id) = 0 Then
                MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
                If cbo_Unit.Enabled Then cbo_Unit.Focus()
                Exit Sub
            End If
        End If
        If Val(txt_Quantity.Text) = 0 And Val(txt_Weight.Text) = 0 Then
            MessageBox.Show("Invalid Quantity or Weight", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If

        If Val(lbl_Amount.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
            If txt_Rate.Enabled Then txt_Rate.Focus()
            Exit Sub
        End If


        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(2).Value = (txt_Description.Text)
                    .Rows(i).Cells(3).Value = cbo_Unit.Text
                    .Rows(i).Cells(4).Value = Trim(cbo_RateFor.Text)
                    .Rows(i).Cells(5).Value = Val(txt_Quantity.Text)
                    .Rows(i).Cells(6).Value = Format(Val(txt_Weight.Text), "########0.000")
                    .Rows(i).Cells(7).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(lbl_Amount.Text), "########0.00")
                    .Rows(i).Cells(12).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(13).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")

                    .Rows(i).Cells(14).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                    .Rows(i).Cells(15).Value = lbl_Grid_HsnCode.Text
                    .Rows(i).Cells(16).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")


                    MtchSTS = True

                    ' If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = txt_Description.Text
                .Rows(n).Cells(3).Value = cbo_Unit.Text
                .Rows(n).Cells(4).Value = cbo_RateFor.Text
                .Rows(n).Cells(5).Value = Val(txt_Quantity.Text)
                .Rows(n).Cells(6).Value = Format(Val(txt_Weight.Text), "########0.000")
                .Rows(n).Cells(7).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(lbl_Amount.Text), "########0.00")
                .Rows(n).Cells(12).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(13).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")

                .Rows(n).Cells(14).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                .Rows(n).Cells(15).Value = lbl_Grid_HsnCode.Text
                .Rows(n).Cells(16).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")

                '.Rows(n).Selected = True

                ' If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If

        End With

        TotalAmount_Calculation()
        'GrossAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        cbo_RateFor.Text = ""
        txt_Description.Text = ""
        txt_Weight.Text = ""
        txt_Quantity.Text = ""
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

    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Quantity.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Quantity.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus

        With cbo_ItemName

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
            cbo_ItemName.Tag = cbo_ItemName.Text
        End With

        cbo_ItemName.Tag = cbo_ItemName.Text

    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, cbo_OnAccount, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_ItemName.Text) <> "" Then
                cbo_Unit.Focus()
            Else
                txt_CashDiscPerc.Focus()
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
                txt_CashDiscPerc.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1079" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1103" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1137" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1149" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1501" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1502" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1503" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1505" Then
                Dim f As New Item_Creation_With_Size

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            Else
                Dim f As New Item_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()

            End If

            

        End If
    End Sub



    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            get_Item_Unit_Rate_TaxPerc()
        End If
        Get_PriceList_Rate()
    End Sub

    Private Sub get_Item_Unit_Rate_TaxPerc()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            '  cbo_ItemName.Tag = cbo_ItemName.Text
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
                    cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                End If
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
        If e.KeyCode = 38 Then e.Handled = True : txt_AddLess.Focus() ' SendKeys.Send("+{TAB}")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_EntType, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If txt_OrderNo.Enabled Then
                txt_OrderNo.Focus()

            Else
                txt_Electronic_RefNo.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                cbo_Ledger.Tag = cbo_Ledger.Text
                Amount_Calculation(True)
            End If
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then

                If MessageBox.Show("Do you want to select Delivery?", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If txt_OrderNo.Enabled Then
                        txt_OrderNo.Focus()

                    Else
                        txt_Electronic_RefNo.Focus()

                    End If

                End If

            Else
                txt_OrderNo.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_DcDate, txt_Electronic_RefNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Electronic_RefNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
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

    Private Sub txt_AddLess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess.KeyDown
        If e.KeyCode = 40 Then
            If txt_TDS_Percentage.Visible And txt_TDS_Percentage.Enabled Then
                txt_TDS_Percentage.Focus()
            ElseIf txt_NoOfBundels.Visible And txt_NoOfBundels.Enabled Then
                txt_NoOfBundels.Focus()
            Else
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            End If
            
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub



    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If txt_TDS_Percentage.Visible And txt_TDS_Percentage.Enabled Then
                txt_TDS_Percentage.Focus()
            ElseIf txt_NoOfBundels.Visible And txt_NoOfBundels.Enabled Then
                txt_NoOfBundels.Focus()
            Else
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            End If
            
        End If
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    cbo_Ledger.Focus()
                End If

            Else
                cbo_ItemName.Focus()

            End If
        End If
        ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress

        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True
    End Sub



    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Amount_Calculation(True)
    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True


    End Sub

    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : cbo_ItemName.Focus()
        If e.KeyCode = 38 Then e.Handled = True : cbo_OnAccount.Focus()

    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then
            cbo_ItemName.Focus()
            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                        cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                        txt_Description.Text = (dgv_Details.CurrentRow.Cells(2).Value)
                        cbo_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
                        cbo_RateFor.Text = Trim(dgv_Details.CurrentRow.Cells(4).Value)
                        txt_Quantity.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
                        txt_Weight.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.000")
                        txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
                        lbl_Grid_DiscPerc.Text = Format(Val(.Rows(i).Cells(12).Value), "########0.00")
                        lbl_Grid_DiscAmount.Text = Format(Val(.Rows(i).Cells(13).Value), "########0.00")

                        lbl_Grid_AssessableValue.Text = Format(Val(.Rows(i).Cells(14).Value), "########0.00")
                        lbl_Grid_HsnCode.Text = Trim(dgv_Details.CurrentRow.Cells(15).Value)
                        lbl_Grid_GstPerc.Text = Format(Val(.Rows(i).Cells(16).Value), "########0.00")

                        Exit For


                    End If

                Next

            End With

        End If
    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        With cbo_Unit

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "uNIT_head", "Unit_name", "", "(Unit_IdNo = 0)")
        End With

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, Nothing, Nothing, "uNIT_head", "Unit_name", "", "(Unit_IdNo = 0)")

        If e.KeyCode = 38 And cbo_Unit.DroppedDown = False Then
            e.Handled = True
            cbo_ItemName.Focus()
        End If
        If e.KeyCode = 40 And cbo_Unit.DroppedDown = False Then
            If cbo_RateFor.Visible And cbo_RateFor.Enabled Then
                cbo_RateFor.Focus()
            ElseIf txt_Quantity.Visible And txt_Quantity.Enabled Then
                txt_Quantity.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, Nothing, "uNIT_head", "Unit_name", "", "(Unit_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_RateFor.Visible And cbo_RateFor.Enabled Then
                cbo_RateFor.Focus()
            ElseIf txt_Quantity.Visible And txt_Quantity.Enabled Then
                txt_Quantity.Focus()
            End If
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



    Private Sub cbo_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor.GotFocus
        With cbo_Unit

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
        End With

    End Sub

    Private Sub cbo_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RateFor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RateFor, cbo_Unit, txt_Quantity, "", "", "", "")
    End Sub

    Private Sub cbo_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RateFor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RateFor, txt_Quantity, "", "", "", "")
    End Sub

    Private Sub cbo_RateFor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RateFor.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RateFor.Name
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

            da = New SqlClient.SqlDataAdapter("select a.Sales_No, a.Sales_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
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
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    '.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
            If .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Siz_idno As Integer = 0
        Dim sqft_qty As Single = 0

        Try
            With dgv_Details
                If .Visible Then
                    If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                        If e.ColumnIndex = 4 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 5 Then
                            If Val(.Rows(e.RowIndex).Cells(5).Value) <> 0 Then
                                .Rows(e.RowIndex).Cells(8).Value = Format(Val(.Rows(e.RowIndex).Cells(5).Value) * Val(.Rows(e.RowIndex).Cells(7).Value), "#########0.00")
                            Else
                                .Rows(e.RowIndex).Cells(8).Value = Format(Val(.Rows(e.RowIndex).Cells(6).Value) * Val(.Rows(e.RowIndex).Cells(7).Value), "#########0.00")
                            End If
                            TotalAmount_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Panel2.Enabled = True And txt_SlNo.Enabled = True Then

            If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

                txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                txt_Description.Text = (dgv_Details.CurrentRow.Cells(2).Value)
                cbo_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
                cbo_RateFor.Text = Trim(dgv_Details.CurrentRow.Cells(4).Value)
                txt_Quantity.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
                txt_Weight.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.000")
                txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
                lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
                lbl_Grid_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
                lbl_Grid_DiscAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")

                lbl_Grid_AssessableValue.Text = Format(Val(dgv_Details.CurrentRow.Cells(14).Value), "########0.00")
                lbl_Grid_HsnCode.Text = Trim(dgv_Details.CurrentRow.Cells(15).Value)
                lbl_Grid_GstPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(16).Value), "########0.00")

                If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

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

        GrossAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        cbo_RateFor.Text = "PC"
        txt_Description.Text = ""
        txt_Weight.Text = ""
        txt_Quantity.Text = ""
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

            GrossAmount_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_ItemName.Text = ""
            cbo_Unit.Text = ""
            cbo_RateFor.Text = "PC"
            txt_Description.Text = ""
            txt_Weight.Text = ""
            txt_Quantity.Text = ""
            txt_Rate.Text = ""
            lbl_Amount.Text = ""
            lbl_Grid_DiscPerc.Text = ""
            lbl_Grid_DiscAmount.Text = ""
            lbl_Grid_AssessableValue.Text = ""
            lbl_Grid_GstPerc.Text = ""
            lbl_Grid_HsnCode.Text = ""
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





    Private Sub txt_DcDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DcDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 38 Then
            txt_Dcno.Focus()
        End If
        If e.KeyCode = 40 Then

            txt_DcDate.Focus()
        End If
    End Sub

    Private Sub txt_DcDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DcDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub txt_DcDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DcDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_DcDate.Text = Date.Today
            txt_DcDate.SelectionStart = txt_DcDate.Text.Length
        End If
    End Sub



    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub


    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub Amount_Calculation()
        If Trim(cbo_RateFor.Text) = "KG" Then
            lbl_Amount.Text = Format(Val(txt_Weight.Text) * Val(txt_Rate.Text), "#########0.00")
        Else
            lbl_Amount.Text = Format(Val(txt_Quantity.Text) * Val(txt_Rate.Text), "#########0.00")
        End If
    End Sub

    Private Sub GrossAmount_Calculation()
        Dim I As Integer
        Dim Sno As Integer
        Dim TotQty As Decimal, TotAmt As Decimal, TotWgt As Decimal

        Sno = 0
        TotQty = 0
        TotAmt = 0
        TotWgt = 0
        With dgv_Details

            For I = 0 To .RowCount - 1
                Sno = Sno + 1
                dgv_Details.Rows(I).Cells(0).Value = Sno

                If Trim(.Rows(I).Cells(1).Value) <> "" Or Val(.Rows(I).Cells(5).Value) <> 0 Or Val(.Rows(I).Cells(6).Value) <> 0 Then
                    TotWgt = TotWgt + Val(dgv_Details.Rows(I).Cells(5).Value)
                    TotQty = TotQty + Val(dgv_Details.Rows(I).Cells(6).Value)
                    TotAmt = TotAmt + Val(dgv_Details.Rows(I).Cells(8).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .Rows.Count = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotWgt)
            .Rows(0).Cells(6).Value = Val(TotQty)
            .Rows(0).Cells(8).Value = Format(Val(TotAmt), "########0.00")
        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")

        NetAmount_Calculation()

    End Sub

    Public Sub print_Invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Sales_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If


            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If prn_Status <> 1 Then
            prn_InpOpts = ""
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")
        End If

        If Val(prn_Status) = 1 Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X12", 800, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    'e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else
                    'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    '    PrintDocument1.Print()
                    'End If

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument1.Print()
                        End If

                    Else
                        PrintDocument1.Print()

                    End If
                End If


            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Print_PDF_Status = False
    End Sub


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetDt1.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, b.Pan_No Ledger_PanNo, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, g.Ledger_Name as DelName ,g.Ledger_Address1 as DelAdd1 ,g.Ledger_Address2 as DelAdd2, g.Ledger_Address3 as DelAdd3 ,g.Ledger_Address4 as DelAdd4, g.Ledger_GSTinNo as DelGSTinNo, g.Pan_No as DelPanNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo LEFT OUTER JOIN Ledger_Head g ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = g.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on g.State_IdNo = DSH.State_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                Debug.Print(Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString))
                Debug.Print(Trim(prn_HdDt.Rows(0).Item("DelPanNo").ToString))
                da2 = New SqlClient.SqlDataAdapter("select a.* from Sales_Details a   where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Sales_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1194" Then      ' VINAYAGA ENGINEERING (CBE)
            Printing_GST_Format4(e)
        Else
            Printing_GST_Format3(e)
        End If







        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1002" Then
        '    Printing_GST_Format2(e)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1501" Then ' SUBI EXPORTS 
        '    Printing_GST_Format7(e)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1502" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1505" Then ' CREATIVE PRINTS , PRINT TECH
        '    Printing_GST_Format3(e)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1503" Then ' LEO GEM KNIT (TIRUPUR)
        '    Printing_GST_Format4(e)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Then ' GOLDEN TEXTILE PRINTERS
        '    Printing_GST_Format6(e)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1142" Then ' SLP - HANGER
        '    Printing_GST_Format5(e)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1174" Then ' WIN TEC
        '    Printing_GST_Format8(e)
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1506" Then ' ITECH - SPECIAL FORMAT
        '    Printing_GST_Format9(e)
        'Else
        '    Printing_GST_Format3(e)
        'End If
        ''Else
        '    Printing_GST_Format1(e)
        'End If


    End Sub

    Private Sub txt_TradeDiscPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_EntType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntType, dtp_Date, cbo_PaymentMethod, "", "", "", "")
    End Sub

    Private Sub cbo_EntType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntType, cbo_PaymentMethod, "", "", "", "")
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Qty As Single, Ent_Rate As Single, Ent_PurcRet_Qty As Single, Ent_Weight As Single
        Dim Ent_DetSlNo As Long

        If Trim(UCase(cbo_EntType.Text)) <> "DELIVERY" Then
            MessageBox.Show("Invalid Type", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0
            Da = New SqlClient.SqlDataAdapter("select a.*,b.*, c.*,d.*,a.Order_No as ord_no,a.Order_Date as ord_date , e.Unit_Name, f.Weight as Ent_Sales_Weight ,f.Noof_Items as Ent_Sales_Quantity, f.Rate as Ent_Rate, f.Sales_Detail_SlNo as Ent_Sales_SlNo from Sales_Delivery_Details a INNER JOIN Sales_Delivery_Head b ON a.Sales_Delivery_Code = b.Sales_Delivery_Code  INNER JOIN Item_Head c ON a.Item_idno = c.Item_idno  INNER JOIN ItemGroup_Head d ON c.ItemGroup_IdNo = d.ItemGroup_IdNo  LEFT OUTER JOIN Unit_Head e ON a.Unit_IdNo = e.Unit_IdNo LEFT OUTER JOIN Sales_Details F ON f.Sales_Code = '" & Trim(NewCode) & "' and f.Entry_Type = '" & Trim(cbo_EntType.Text) & "' and a.Sales_Delivery_Code = f.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = f.Sales_Delivery_Detail_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.Quantity  - a.Receipt_Quantity ) > 0 or f.Noof_Items > 0 ) Order by a.For_OrderBy, a.Sales_Delivery_No, a.Sales_Delivery_Detail_SlNo", con)

            '            Da = New SqlClient.SqlDataAdapter("select a.*,b.*, c.*,d.*, e.Unit_Name, f.Noof_Items as Ent_Sales_Quantity, f.Rate as Ent_Rate, f.Sales_Detail_SlNo as Ent_Sales_SlNo from Sales_Delivery_Details a INNER JOIN Sales_Delivery_Head b ON a.Sales_Delivery_Code = b.Sales_Delivery_Code  INNER JOIN Item_Head c ON a.Item_idno = c.Item_idno  INNER JOIN ItemGroup_Head d ON c.ItemGroup_IdNo = d.ItemGroup_IdNo  LEFT OUTER JOIN Unit_Head e ON a.Unit_IdNo = e.Unit_IdNo LEFT OUTER JOIN Sales_Details F ON f.Sales_Code = '" & Trim(NewCode) & "' and f.Entry_Type = '" & Trim(cbo_EntType.Text) & "' and a.Sales_Delivery_Code = f.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = f.Sales_Delivery_Detail_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.Quantity  - a.Receipt_Quantity ) > 0 or f.Noof_Items > 0 ) Order by a.For_OrderBy, a.Sales_Delivery_No, a.Sales_Delivery_Detail_SlNo", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Ent_Qty = 0 : Ent_Rate = 0 : Ent_DetSlNo = 0 : Ent_PurcRet_Qty = 0 : Ent_Weight = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_SlNo").ToString) = False Then Ent_DetSlNo = Val(Dt1.Rows(i).Item("Ent_Sales_SlNo").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_Quantity").ToString) = False Then Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Sales_Quantity").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_Weight").ToString) = False Then Ent_Weight = Val(Dt1.Rows(i).Item("Ent_Sales_Weight").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    ' If IsDBNull(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString) = False Then Ent_PurcRet_Qty = Val(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString)



                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Sales_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Item_name").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Item_Description").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Unit_Name").ToString
                    '    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Rate_For").ToString

                    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1503" Then 'LEO GEM KNIT
                    '    .Columns(5).HeaderText = "WEIGHT"
                    '    .Rows(n).Cells(6).Value = Format((Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Ent_Qty), "#########0.000")
                    'Else
                    '    .Rows(n).Cells(6).Value = (Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Ent_Qty)
                    'End If

                    .Rows(n).Cells(6).Value = Format((Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Ent_Qty), "#########0.000")
                    .Rows(n).Cells(7).Value = Format((Val(Dt1.Rows(i).Item("Weight").ToString) + Ent_Weight), "#########0.000")

                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "########0.00")
                    .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(i).Item("Amount").ToString), "########0.00")

                    If Val(Ent_Qty) > 0 Or Val(Ent_Weight) > 0 Then
                        .Rows(n).Cells(10).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next
                    Else
                        .Rows(n).Cells(10).Value = ""
                    End If


                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Sales_Delivery_Code").ToString
                    .Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("Sales_Delivery_Detail_SlNo").ToString)
                    .Rows(n).Cells(13).Value = Val(Ent_DetSlNo)
                    .Rows(n).Cells(14).Value = Val(Ent_Qty)
                    .Rows(n).Cells(15).Value = Val(Ent_Weight)
                    .Rows(n).Cells(16).Value = Val(Ent_Rate)
                    .Rows(n).Cells(17).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sales_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Ord_No").ToString
                    .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Ord_Date").ToString
                    .Rows(n).Cells(20).Value = Common_Procedures.Transport_IdNoToName(con, Val(Dt1.Rows(i).Item("Transport_IdNo").ToString))
                    .Rows(n).Cells(21).Value = (Dt1.Rows(i).Item("Vehicle_No").ToString)
                    .Rows(n).Cells(22).Value = (Dt1.Rows(i).Item("Item_HSN_Code").ToString)
                    .Rows(n).Cells(23).Value = (Dt1.Rows(i).Item("Item_GST_Percentage").ToString)
                    .Rows(n).Cells(24).Value = (Dt1.Rows(i).Item("Challan_No").ToString)
                    .Rows(n).Cells(25).Value = (Dt1.Rows(i).Item("Challan_Date").ToString)

                Next

            End If
            Dt1.Clear()

            If .Rows.Count = 0 Then
                n = .Rows.Add()
                .Rows(n).Cells(0).Value = "1"
            End If

        End With

        pnl_Selection.Visible = True
        pnl_Selection.BringToFront()
        pnl_Back.Enabled = False

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

                .Rows(RwIndx).Cells(10).Value = (Val(.Rows(RwIndx).Cells(10).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(10).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(10).Value = ""

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
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Ent_Qty As Single, Ent_Rate As Single, Ent_Weight As Single

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(10).Value) = 1 Then

                If Val(dgv_Selection.Rows(i).Cells(14).Value) <> 0 Then
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(14).Value)
                Else
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(6).Value)
                End If

                If Val(dgv_Selection.Rows(i).Cells(15).Value) <> 0 Then
                    Ent_Weight = Val(dgv_Selection.Rows(i).Cells(18).Value)
                Else
                    Ent_Weight = Val(dgv_Selection.Rows(i).Cells(7).Value)
                End If


                If Val(dgv_Selection.Rows(i).Cells(16).Value) <> 0 Then
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(16).Value)
                Else
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(8).Value)
                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value

                dgv_Details.Rows(n).Cells(5).Value = Val(Ent_Qty)

                dgv_Details.Rows(n).Cells(6).Value = Val(Ent_Weight)

                dgv_Details.Rows(n).Cells(7).Value = Val(Ent_Rate)


                If Ent_Qty <> 0 Then
                    dgv_Details.Rows(n).Cells(8).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                Else
                    dgv_Details.Rows(n).Cells(8).Value = Format(Val(Ent_Weight) * Val(Ent_Rate), "##########0.00")
                End If


                '      dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(9).Value

                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(11).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(14).Value = Format((Val(Ent_Qty) * Val(Ent_Rate)) - Val(dgv_Details.Rows(n).Cells(13).Value), "##########0.00")
                dgv_Details.Rows(n).Cells(15).Value = dgv_Selection.Rows(i).Cells(22).Value
                dgv_Details.Rows(n).Cells(16).Value = dgv_Selection.Rows(i).Cells(23).Value

                dgv_Details.Rows(n).Cells(17).Value = dgv_Selection.Rows(i).Cells(18).Value
                dgv_Details.Rows(n).Cells(18).Value = dgv_Selection.Rows(i).Cells(19).Value

                dgv_Details.Rows(n).Cells(19).Value = dgv_Selection.Rows(i).Cells(24).Value
                dgv_Details.Rows(n).Cells(20).Value = dgv_Selection.Rows(i).Cells(25).Value


                txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(18).Value
                txt_OrderDate.Text = dgv_Selection.Rows(i).Cells(19).Value
                txt_Dcno.Text = dgv_Selection.Rows(i).Cells(1).Value
                txt_DcDate.Text = dgv_Selection.Rows(i).Cells(17).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(20).Value
                txt_VechileNo.Text = dgv_Selection.Rows(i).Cells(21).Value



                '   dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value

            End If

        Next i

        NoCalc_Status = False



        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        'txt_BillNo.Focus()
        'cbo_EntType.Enabled = False

        If txt_Electronic_RefNo.Enabled And txt_Electronic_RefNo.Visible Then txt_Electronic_RefNo.Focus()

    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_EntType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntType.TextChanged
        If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
            Panel2.Enabled = True
            dgv_Details.EditMode = DataGridViewEditMode.EditProgrammatically
            dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            txt_OrderDate.Enabled = True
            txt_OrderNo.Enabled = True
            txt_DcDate.Enabled = True
            txt_Dcno.Enabled = True
            cbo_Transport.Enabled = True
            txt_VechileNo.Enabled = True

        Else

            Panel2.Enabled = False
            dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
            dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
            txt_OrderDate.Enabled = False
            txt_OrderNo.Enabled = False
            txt_DcDate.Enabled = False
            txt_Dcno.Enabled = False
            cbo_Transport.Enabled = False
            txt_VechileNo.Enabled = False

        End If
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = Nothing
        If dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5 Or dgv_Details.CurrentCell.ColumnIndex = 6 Then
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
                    If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
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

                If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
                    e.Handled = True
                End If
                If .CurrentCell.ColumnIndex = 5 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = 6 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = 7 Then
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

                    If .CurrentCell.ColumnIndex >= 6 Then

                        If .CurrentCell.RowIndex >= .Rows.Count - 1 Then

                            txt_CashDiscPerc.Focus()

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
                            If Panel2.Enabled = True And cbo_ItemName.Enabled = True Then
                                cbo_ItemName.Focus()

                            Else
                                cbo_Ledger.Focus()

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




    Private Sub txt_Description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Description.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .CurrentCell.RowIndex >= 0 And .CurrentCell.ColumnIndex >= 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, cbo_EntType, cbo_Ledger, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, cbo_Ledger, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, txt_VechileNo, txt_DateTime_Of_Supply, "", "", "", "")
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



    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Amount_Calculation(True)
        End If
        Get_PriceList_Rate()
    End Sub
    Private Sub cbo_PaymentMethod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.LostFocus
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If
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

                'If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                '    For i = 0 To dgv_Details.RowCount - 1
                '        ItmIdNo = Common_Procedures.Item_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                '    Next
                ' Else

                ItmIdNo = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
                '  End If

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
    '***** GST END *****

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_DateTime_Of_Supply, txt_place_Supply, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_place_Supply, "", "", "", "", True)

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

        '***** GST START *****

        If FrmLdSTS = True Then Exit Sub

        If GridAll_Row_STS = True Then

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        ItmIdNo = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                        If ItmIdNo <> 0 Then

                            .Rows(i).Cells(15).Value = ""
                            .Rows(i).Cells(16).Value = ""

                            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                                dt = New DataTable
                                da.Fill(dt)
                                If dt.Rows.Count > 0 Then

                                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                                        .Rows(i).Cells(15).Value = dt.Rows(0)("Item_HSN_Code").ToString
                                    End If
                                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                                        .Rows(i).Cells(16).Value = Format(Val(dt.Rows(0)("Item_GST_Percentage").ToString), "#########0.00")
                                    End If

                                End If
                                dt.Clear()

                            End If

                            If Val(.Rows(i).Cells(5).Value) <> 0 Then
                                .Rows(i).Cells(8).Value = Format(Val(.Rows(i).Cells(5).Value) * Val(.Rows(i).Cells(7).Value), "#########0.00")
                            Else
                                .Rows(i).Cells(8).Value = Format(Val(.Rows(i).Cells(6).Value) * Val(.Rows(i).Cells(7).Value), "#########0.00")
                            End If

                            .Rows(i).Cells(12).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
                            .Rows(i).Cells(13).Value = Format(Val(.Rows(i).Cells(8).Value) * Val(.Rows(i).Cells(12).Value) / 100, "#########0.00")
                            .Rows(i).Cells(14).Value = Format(Val(.Rows(i).Cells(8).Value) - Val(.Rows(i).Cells(13).Value), "#########0.00")

                        End If

                    End If

                Next

            End With

            TotalAmount_Calculation()

        Else
            If Trim(cbo_RateFor.Text) = "KG" Then
                lbl_Amount.Text = Format(Val(txt_Weight.Text) * Val(txt_Rate.Text), "#########0.00")
            Else
                lbl_Amount.Text = Format(Val(txt_Quantity.Text) * Val(txt_Rate.Text), "#########0.00")
            End If

            lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
            lbl_Grid_DiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(lbl_Grid_DiscPerc.Text) / 100, "#########0.00")
            lbl_Grid_AssessableValue.Text = Format(Val(lbl_Amount.Text) - Val(lbl_Grid_DiscAmount.Text), "#########0.00")

        End If

        '***** GST END *****

    End Sub

    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer = 0
        Dim TotWgt As Decimal = 0
        Dim TotQty As Decimal = 0
        Dim TotGrsAmt As Decimal = 0
        Dim TotDiscAmt As Decimal = 0
        Dim TotAssval As Decimal = 0
        Dim TotCGstAmt As Decimal = 0
        Dim TotSGstAmt As Decimal = 0
        Dim TotIGstAmt As Decimal = 0

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotWgt = 0
        TotQty = 0
        TotGrsAmt = 0
        TotDiscAmt = 0
        TotAssval = 0

        For i = 0 To dgv_Details.RowCount - 1

            Sno = Sno + 1

            dgv_Details.Rows(i).Cells(0).Value = Sno

            If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" Or Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Then

                TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(5).Value)
                TotWgt = TotWgt + Val(dgv_Details.Rows(i).Cells(6).Value)
                TotGrsAmt = TotGrsAmt + Val(dgv_Details.Rows(i).Cells(8).Value)

                '***** GST START *****
                TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(13).Value)
                TotAssval = TotAssval + Val(dgv_Details.Rows(i).Cells(14).Value)
                '***** GST END *****

            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotQty)
            .Rows(0).Cells(6).Value = Format(Val(TotWgt), "############0.000")

            .Rows(0).Cells(8).Value = Format(Val(TotGrsAmt), "########0.00")
            '***** GST START *****
            .Rows(0).Cells(13).Value = Format(Val(TotDiscAmt), "########0.00")
            .Rows(0).Cells(14).Value = Format(Val(TotAssval), "########0.00")
            '***** GST END *****
        End With

        lbl_GrossAmount.Text = Format(TotGrsAmt, "########0.00")
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

        NetAmount_Calculation()

        '***** GST END *****

    End Sub

    Private Sub NetAmount_Calculation()

        Dim NtAmt As Decimal
        Dim NtAmt_TDS As Decimal

        If FrmLdSTS = True Then Exit Sub

        lbl_Assessable.Text = Val(lbl_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        NtAmt = Val(lbl_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

    
        If Trim(Common_Procedures.settings.CustomerCode) <> "1502" And Trim(Common_Procedures.settings.CustomerCode) <> "1504" And Trim(Common_Procedures.settings.CustomerCode) <> "1505" Then 'Create prints ,GOLDEN TEXTILED , PRINT TECH
            lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))
        End If

        lbl_TdsAmount.Text = Format(Val(lbl_NetAmount.Text) * Val(txt_TDS_Percentage.Text) / 100, "#################0")
        NtAmt_TDS = Val(lbl_NetAmount.Text) - Val(lbl_TdsAmount.Text)
        If NtAmt_TDS <> 0 Then lbl_NetAmount_TDS.Text = Common_Procedures.Currency_Format(Format(Val(NtAmt_TDS), "#############0"))


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

                        If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(15).Value) <> "" And Val(.Rows(i).Cells(16).Value) <> 0 Then
                            n = 0
                            cmd.CommandText = "Insert into EntryTemp (                    Name1                ,                   Currency1            ,                       Currency2                                      ) " & _
                                              "            Values    ( '" & Trim(.Rows(i).Cells(15).Value) & "', " & (Val(.Rows(i).Cells(16).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value) + AssVal_Frgt_Othr_Charges) & " ) "
                            n = cmd.ExecuteNonQuery()

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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Weight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub cbo_OnAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OnAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 )  ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_OnAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAccount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OnAccount, txt_place_Supply, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 )  ", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_OnAccount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                Else
                    txt_CashDiscPerc.Focus()
                End If


            Else
                cbo_ItemName.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_OnAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OnAccount.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OnAccount, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 )  ", "(Ledger_IdNo = 0)")
            If Asc(e.KeyChar) = 13 Then


                If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                    If dgv_Details.RowCount > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    Else
                        txt_CashDiscPerc.Focus()
                    End If


                Else
                    cbo_ItemName.Focus()

                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cbo_DelTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, txt_Electronic_RefNo, txt_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateTime_Of_Supply.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_DateTime_Of_Supply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")
        End If
    End Sub

    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
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

        Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
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

    Private Sub Printing_GST_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
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
        Dim szNm1 As String, szNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

        If Trim(Common_Procedures.settings.CustomerCode) = "1142" Then  ' slp (Tirupur)  (DOTMATRIX)
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X12", 800, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If
 
        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0 '30 
            .Right = 80 '60
            .Top = 5
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 19 


        NoofItems_PerPage = 8
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0

        ClArr(1) = Val(35)
        ClArr(2) = 240 : ClArr(3) = 80 : ClArr(4) = 60 : ClArr(5) = 0 : ClArr(6) = 80 : ClArr(7) = 60 : ClArr(8) = 75
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
                            ItmNm2 = ""
                            If Len(ItmNm1) > 28 Then
                                For I = 28 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 28
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
                            End If


                            'szNm1 = Trim(Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString)))
                            'szNm2 = ""
                            'If Len(szNm1) > 8 Then
                            '    For I = 8 To 1 Step -1
                            '        If Mid$(Trim(szNm1), I, 1) = " " Or Mid$(Trim(szNm1), I, 1) = "," Or Mid$(Trim(szNm1), I, 1) = "." Or Mid$(Trim(szNm1), I, 1) = "-" Or Mid$(Trim(szNm1), I, 1) = "/" Or Mid$(Trim(szNm1), I, 1) = "_" Or Mid$(Trim(szNm1), I, 1) = "(" Or Mid$(Trim(szNm1), I, 1) = ")" Or Mid$(Trim(szNm1), I, 1) = "\" Or Mid$(Trim(szNm1), I, 1) = "[" Or Mid$(Trim(szNm1), I, 1) = "]" Or Mid$(Trim(szNm1), I, 1) = "{" Or Mid$(Trim(szNm1), I, 1) = "}" Then Exit For
                            '    Next I
                            '    If I = 0 Then I = 8
                            '    szNm2 = Microsoft.VisualBasic.Right(Trim(szNm1), Len(szNm1) - I)
                            '    szNm1 = Microsoft.VisualBasic.Left(Trim(szNm1), I - 1)
                            'End If

                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, szNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)

                            If Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Or Trim(szNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, szNm2, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim Inv_No As String = "", InvSubNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1


        prn_OriDupTri = ""
        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count <= Len(Trim(prn_InpOpts)) Then

        '        S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

        '        If Val(S) = 1 Then
        '            prn_OriDupTri = "ORIGINAL FOR BUYER"
        '        ElseIf Val(S) = 2 Then
        '            prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
        '        ElseIf Val(S) = 3 Then
        '            prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
        '        ElseIf Val(S) = 4 Then
        '            prn_OriDupTri = "EXTRA COPY"
        '        Else
        '            If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
        '                prn_OriDupTri = Trim(prn_InpOpts)
        '            End If
        '        End If

        '    End If

        'End If

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        '   If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
        ' Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        Cmp_StateCode = "CODE : 33"
        '  End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 22, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
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
        ' Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        ' strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        ' CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCode, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateCode, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        ' Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        ' vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"
        vHeading = "INVOICE"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try




            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side

            Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")


            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "REF. No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
         
            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString & " Date :" & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)


            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            ' Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            End If
            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                End If
                If Trim(vDelvPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If
            End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

            '    'If Trim(prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If

            'End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    'If Trim(prn_HdDt.Rows(0).Item("DelPanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 25, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DelState_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 40, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 80, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 80, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
            ' Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UOM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 20
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            '  Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1 - 40, Y2)
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If
            End If


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            'Y1 = CurY
            'Y2 = CurY + TxtHgt - 0.5
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)



            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            p2Font = New Font("Webdings", 8, FontStyle.Bold)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction.", LMargin + 35, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Intrest @ 20% p.a will be charges on the bill not paid", LMargin + 35, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "within 30 days from the date of Invoice.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Once goods sold Cannot be taken back.", LMargin + 35, CurY, 0, 0, pFont)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)

            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST PAYABLE ON REVERSE CHARGE", LMargin + C1 - 40, CurY + 3, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "Arbitration Council of Tirupur and award passed will be binding on us.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 40, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt + TxtHgt - 15
            'Common_Procedures.FillRegionRectangle(e, LMargin + C1 - 40, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 - 30, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6) + 40), CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            Y1 = CurY + 0.55
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            ' Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 40, Y2)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 60, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 40, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 40, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above true and correct", PageWidth - 10, CurY + 5, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            '
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            LnAr(14) = CurY


            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format1_PageFooter1111(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
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

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)
            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "All dispute arising out of this transaction/Contract will be reffered to ", LMargin + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "institutional arbitration before Arbitration Council of Tirupur as per the", LMargin + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "rules and regulations of tirupur as per the rules and regulations of", LMargin + 20, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 + 10, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Arbitration Council of Tirupur and award passed will be binding on us.", LMargin + 20, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, Y2)


            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 10, CurY, 1, 0, pFont)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt - 0.5
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)

            Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)




            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If









            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            LnAr(14) = CurY





            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
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
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

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
            .Top = 25 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.5 ' 19 

        NoofItems_PerPage = 11
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35)
        ClArr(2) = 230 : ClArr(3) = 85 : ClArr(4) = 50 : ClArr(5) = 80 : ClArr(6) = 60 : ClArr(7) = 90
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
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
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                            If Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Else

                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            End If



                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0


                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
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

        p1Font = New Font("Calibri", 30, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
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

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

        Dim vHeading As String = ""

        vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

            W1 = e.Graphics.MeasureString("INVOICE DATE :", pFont).Width
            S1 = e.Graphics.MeasureString("PO No    :   ", pFont).Width

            CurY1 = CurY + 11

            'Left Side
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1002" Then '---- Peacock Traders(Tirupur)
                Common_Procedures.Print_To_PrintDocument(e, "TPR-" & prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + S1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + C2 + S1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + C2 + S1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt + 1
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C2 + S1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C2 + S1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If


            'Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString & " Date :" & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            'End If

            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            End If
            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                End If
                If Trim(vDelvPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 25, CurY, 0, 0, pFont)
            End If


            If Trim(prn_HdDt.Rows(0).Item("DelState_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 40, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 30, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 80, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 80, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 10 + 0.5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If
            End If


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If



            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            p2Font = New Font("Webdings", 8, FontStyle.Bold)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from the invoice date.", LMargin + 35, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "All dispute arising out of this transaction/Contract will be reffered to ", LMargin + 35, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "institutional arbitration before Arbitration Council of Tirupur as per the", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "rules and regulations of tirupur as per the rules and regulations of", LMargin + 35, CurY, 0, 0, pFont)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Arbitration Council of Tirupur and award passed will be binding on us.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt + TxtHgt - 15
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 + 10, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, Y2)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            LnAr(14) = CurY


            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String, ItmNm4 As String, ItmNm5 As String
        Dim szNm1 As String, szNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

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
            .Top = 20 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 19 

        If Trim(Common_Procedures.settings.CustomerCode) = "1504" Then ' GOLDEN TEXTILE PRINTERS
            NoofItems_PerPage = 10
        Else
            NoofItems_PerPage = 8
        End If


        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35)
        ClArr(2) = 330 : ClArr(3) = 65 : ClArr(4) = 50 : ClArr(5) = 0 : ClArr(6) = 55 : ClArr(7) = 45 : ClArr(8) = 60
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
                            ItmNm2 = ""
                            ItmNm3 = ""
                            ItmNm4 = ""
                            ItmNm5 = ""
                            If Len(ItmNm1) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
                            End If


                            If Len(ItmNm2) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                            End If

                            If Len(ItmNm3) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                            End If

                            If Len(ItmNm4) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = "." Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "/" Or Mid$(Trim(ItmNm4), I, 1) = "_" Or Mid$(Trim(ItmNm4), I, 1) = "(" Or Mid$(Trim(ItmNm4), I, 1) = ")" Or Mid$(Trim(ItmNm4), I, 1) = "\" Or Mid$(Trim(ItmNm4), I, 1) = "[" Or Mid$(Trim(ItmNm4), I, 1) = "]" Or Mid$(Trim(ItmNm4), I, 1) = "{" Or Mid$(Trim(ItmNm4), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm5 = Microsoft.VisualBasic.Right(Trim(ItmNm4), Len(ItmNm4) - I)
                                ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm4), I - 1)
                            End If


                            'szNm1 = ItmNm2
                            'szNm2 = ""
                            'If Len(szNm1) > 35 Then
                            '    For I = 35 To 1 Step -1
                            '        If Mid$(Trim(szNm1), I, 1) = " " Or Mid$(Trim(szNm1), I, 1) = "," Or Mid$(Trim(szNm1), I, 1) = "." Or Mid$(Trim(szNm1), I, 1) = "-" Or Mid$(Trim(szNm1), I, 1) = "/" Or Mid$(Trim(szNm1), I, 1) = "_" Or Mid$(Trim(szNm1), I, 1) = "(" Or Mid$(Trim(szNm1), I, 1) = ")" Or Mid$(Trim(szNm1), I, 1) = "\" Or Mid$(Trim(szNm1), I, 1) = "[" Or Mid$(Trim(szNm1), I, 1) = "]" Or Mid$(Trim(szNm1), I, 1) = "{" Or Mid$(Trim(szNm1), I, 1) = "}" Then Exit For
                            '    Next I
                            '    If I = 0 Then I = 35
                            '    szNm2 = Microsoft.VisualBasic.Right(Trim(szNm1), Len(szNm1) - I)
                            '    szNm1 = Microsoft.VisualBasic.Left(Trim(szNm1), I - 1)
                            'End If


                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            '  Common_Procedures.Print_To_PrintDocument(e, szNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)

                            If Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 2, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm3) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm4) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm5) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm5), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            'If Trim(szNm1) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm1), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If

                            'If Trim(szNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim Inv_No As String = "", InvSubNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 60, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        '   If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
        ' Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        Cmp_StateCode = "CODE : 33"
        '  End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 22, FontStyle.Bold)

    
        If Trim(Common_Procedures.settings.CustomerCode) = "1502" Then
            'Dim folder As String = Common_Procedures.AppPath & "\Images\"
            'Dim filename As String = System.IO.Path.Combine(folder, "CREAT_LOGO.jpg")

            e.Graphics.DrawImage(DirectCast(Image.FromFile(Common_Procedures.AppPath & "\Images\CREAT_LOGO.jpg"), Drawing.Image), LMargin + 24, CurY - 5, 100, 120)
            'e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.CREAT_LOGO, Drawing.Image), LMargin + 24, CurY - 5, 100, 120)
        End If
        If Trim(Common_Procedures.settings.CustomerCode) = "1505" Then
            'Dim folder As String = Common_Procedures.AppPath & "\Images\"
            'Dim filename As String = System.IO.Path.Combine(folder, "CREAT_LOGO.jpg")

            e.Graphics.DrawImage(DirectCast(Image.FromFile(Common_Procedures.AppPath & "\Images\print_tech_logo.PNG"), Drawing.Image), LMargin + 24, CurY - 5, 100, 120)
            'e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.CREAT_LOGO, Drawing.Image), LMargin + 24, CurY - 5, 100, 120)
        End If

        Common_Procedures.Print_To_PrintDocument_Red(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If


        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
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
        ' Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        ' strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        ' CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCode, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateCode, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        ' vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"
        vHeading = "INVOICE"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try




            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) - 40
            C2 = PrintWidth / 2
            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side

            Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")


            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


        
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Then ' GOLDEN TEXTILE PRINTERS

                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString)
                ItmNm2 = ""
                If Trim(ItmNm1) <> "" Then
                    ItmNm1 = "(" & Trim(ItmNm1) & ")"
                    If Len(ItmNm1) > 30 Then
                        For i = 30 To 1 Step -1
                            If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                        Next i
                        If i = 0 Then i = 30
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
                    End If
                End If

                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
                If Trim(ItmNm2) <> "" Then
                    CurY1 = CurY1 + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
                End If
                


                'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
                'End If

            Else

                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

                If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
                End If

            End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString & " Date :" & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 5

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1502" Then ' CREAT PRINTS
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & IIf(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString <> "", ", " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, ""), LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString & IIf(prn_HdDt.Rows(0).Item("DelAdd4").ToString <> "", ", " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, ""), LMargin + C2 + 10, CurY, 0, 0, pFont)
            Else
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            End If
            
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            End If
            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                End If
                If Trim(vDelvPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If
            End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

            '    'If Trim(prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If

            'End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    'If Trim(prn_HdDt.Rows(0).Item("DelPanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, C2, CurY, 1, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DelState_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, PageWidth - 20, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, C2 - 80, LnAr(4), C2 - 80, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, C2 - 30, LnAr(4), C2 - 30, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, PageWidth - 50, LnAr(4), PageWidth - 50, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim CurY2 As Single = 0
        Dim TXT_hgt1 As Single = 0
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1 - 55, Y2)
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + C1 - 40, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + C1 - 40, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + C1 - 40, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + C1 - 40, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                End If
            End If


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) - 60, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 - 40, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 70, CurY, LMargin + ClAr(1) + ClAr(2) - 70, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            'Y1 = CurY
            'Y2 = CurY + TxtHgt - 0.5
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)


            p1Font = New Font("Calibri", 8, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            p2Font = New Font("Webdings", 7, FontStyle.Bold)

            CurY2 = CurY + TxtHgt

            TXT_hgt1 = TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction Only.", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Payments should be made by A/C payee cheque or DD only.", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "This bill is payable in full on demand at time.", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Any duspute(s) arising out of this transaction/contract will be referred to institutional arbitration", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "before the arbitration council of Tirupur as per its rules and regulations and the award passed ", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "will be binding on us", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We can't accept the complaints after stiching, inform the complaints before stiching...............", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Payment should be made with in 30 days from the bill date, If the payment is across from us the ", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "due date 5%/pm interest will be chargeable.", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Any defect found in printing should be informed formally through mail with in 7 days from the ", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "date of delivery.", LMargin + 25, CurY2, 0, 0, p1Font)


            
            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If



            CurY = CurY + TxtHgt - 15
            ' p2Font = New Font("Webdings", 8, FontStyle.Bold)
            ' Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction Only.", LMargin + 35, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)


            'Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            ' Common_Procedures.Print_To_PrintDocument(e, "Payments should be made by A/C payee cheque or DD only.", LMargin + 35, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "This bill is payable in full on demand at time.", LMargin + 35, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Once goods sold Cannot be taken back.", LMargin + 35, CurY, 0, 0, p1Font)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

           
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST Payable on Reverse Charge", LMargin + C1 - 50, CurY + 3, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "Arbitration Council of Tirupur and award passed will be binding on us.", LMargin + 35, CurY, 0, 0, pFont)


            If CurY2 > CurY Then CurY = CurY2

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt + TxtHgt - 15
            Common_Procedures.FillRegionRectangle(e, LMargin + C1 - 55, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 - 30, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 2, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6) + 40), CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            Y1 = CurY + 0.55
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 50, Y2)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 40, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 50, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If
            p1Font = New Font("Calibri", 7, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above true and correct", PageWidth - 10, CurY + 5, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            '
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            LnAr(14) = CurY


            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 50, CurY, LMargin + ClAr(1) + ClAr(2) - 50, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
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
        Dim szNm1 As String, szNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

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
            .Top = 20 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 19 


        NoofItems_PerPage = 14 '8

        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35)
        ClArr(2) = 70 : ClArr(3) = 220 : ClArr(4) = 70 : ClArr(5) = 50 : ClArr(6) = 50 : ClArr(7) = 75 : ClArr(8) = 80
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))


        'ClArr(2) = 210 : ClArr(3) = 70 : ClArr(4) = 50 : ClArr(5) = 50 : ClArr(6) = 80 : ClArr(7) = 80 : ClArr(8) = 50
        'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
                            ItmNm2 = ""
                            If Len(ItmNm1) > 28 Then
                                For I = 28 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 28
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
                            End If


                            szNm1 = Trim((prn_DetDt.Rows(DetIndx).Item("Challan_No").ToString))
                            szNm2 = ""
                            If Len(szNm1) > 15 Then
                                For I = 15 To 1 Step -1
                                    If Mid$(Trim(szNm1), I, 1) = " " Or Mid$(Trim(szNm1), I, 1) = "," Or Mid$(Trim(szNm1), I, 1) = "." Or Mid$(Trim(szNm1), I, 1) = "-" Or Mid$(Trim(szNm1), I, 1) = "/" Or Mid$(Trim(szNm1), I, 1) = "_" Or Mid$(Trim(szNm1), I, 1) = "(" Or Mid$(Trim(szNm1), I, 1) = ")" Or Mid$(Trim(szNm1), I, 1) = "\" Or Mid$(Trim(szNm1), I, 1) = "[" Or Mid$(Trim(szNm1), I, 1) = "]" Or Mid$(Trim(szNm1), I, 1) = "{" Or Mid$(Trim(szNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 15
                                szNm2 = Microsoft.VisualBasic.Right(Trim(szNm1), Len(szNm1) - I)
                                szNm1 = Microsoft.VisualBasic.Left(Trim(szNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            p1Font = New Font("Calibri", 8, FontStyle.Bold)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(szNm1), LMargin + ClArr(1) + 2, CurY + 5, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, IIf(Trim((prn_DetDt.Rows(DetIndx).Item("Order_No").ToString)) <> "", "PO : " & Trim((prn_DetDt.Rows(DetIndx).Item("Order_No").ToString)), "") & " " & IIf(Trim((prn_DetDt.Rows(DetIndx).Item("Order_Date").ToString)) <> "", " / Date : " & Trim((prn_DetDt.Rows(DetIndx).Item("Order_Date").ToString)), ""), LMargin + ClArr(1) + ClArr(2) + 5, CurY + 5, 0, 0, p1Font)

                            CurY = CurY + TxtHgt
                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Trim(szNm2), LMargin + ClArr(1) + 5, CurY, 0, 0, p1Font)

                            Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetDt.Rows(DetIndx).Item("Challan_Date").ToString)), LMargin + ClArr(1) + 3, CurY + 3, 0, 0, p1Font)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), "############0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 3, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 3, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 3, CurY, 1, 0, pFont)


                          


                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim Inv_No As String = "", InvSubNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        '   If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
        ' Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        Cmp_StateCode = "CODE : 33"
        '  End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 22, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
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
        ' Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        ' strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        ' CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCode, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateCode, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        ' vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"
        vHeading = "INVOICE"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try




            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side

            Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

            p1Font = New Font("Calibri", 15, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + C2 + W1 + 30, CurY1 - 5, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
           
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
            '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            'End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)


            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)


           
            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "DC Date", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
            '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            'End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString & " Date :" & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

     




            'CurY = CurY1 + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'Y1 = CurY + 0.5
            'Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO and SHIPPED TO)", LMargin, CurY, 2, C2, p1Font)

            ' CurY = CurY1 + TxtHgt + 5


            ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt + 5

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

         

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 25, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 35, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, LnAr(3))
          
            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 20
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "CHALLAN", LMargin + ClAr(1), CurY - 3, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY + TxtHgt, 2, ClAr(2), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1) + +ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("RATE_FOR").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1 - 50, Y2)
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If
            End If


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 50, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 60, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 60, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            'Y1 = CurY
            'Y2 = CurY + TxtHgt - 0.5
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)



            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            p2Font = New Font("Webdings", 8, FontStyle.Bold)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to COIMBATORE  Jurisdiction.", LMargin + 35, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            'Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Intrest @ 20% p.a will be charges on the bill not paid", LMargin + 35, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "within 30 days from the date of Invoice.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            '   Common_Procedures.Print_To_PrintDocument(e, "Once goods sold Cannot be taken back.", LMargin + 35, CurY, 0, 0, pFont)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST Payable on Reverse Charge", LMargin + C1 - 35, CurY + 3, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "Arbitration Council of Tirupur and award passed will be binding on us.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 50, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt + TxtHgt - 15
            Common_Procedures.FillRegionRectangle(e, LMargin + C1 - 50, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 - 30, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6) + 40), CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            Y1 = CurY + 0.55
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50, Y2)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 60, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If
            p1Font = New Font("Calibri", 7.5, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above true and correct", PageWidth - 10, CurY + 5, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            '
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            LnAr(14) = CurY


            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_TDS_Percentage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TDS_Percentage.KeyDown
        If e.KeyCode = 40 Then

            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If



        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_TDS_Percentage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TDS_Percentage.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If


        End If
    End Sub

    Private Sub txt_TDS_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TDS_Percentage.TextChanged
       NetAmount_Calculation

    End Sub

    Private Sub Printing_GST_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String, ItmNm4 As String, ItmNm5 As String
        Dim szNm1 As String, szNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

        'If Val(prn_Status) <> 0 Then
        '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X12", 800, 1200)
        '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        'Else

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next
        ' End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 30 '60
            .Right = 60
            .Top = 20 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 19 


        NoofItems_PerPage = 8
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35)
        If Trim(Common_Procedures.settings.CustomerCode) = "1142" Then  ' slp (Tirupur)  
            ClArr(2) = 280 : ClArr(3) = 65 : ClArr(4) = 45 : ClArr(5) = 0 : ClArr(6) = 55 : ClArr(7) = 90 : ClArr(8) = 65
            ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))
        Else
            ClArr(2) = 280 : ClArr(3) = 65 : ClArr(4) = 45 : ClArr(5) = 0 : ClArr(6) = 90 : ClArr(7) = 55 : ClArr(8) = 65
            ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        End If
      
        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format5_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
                            ItmNm2 = ""
                            ItmNm3 = ""
                            ItmNm4 = ""
                            ItmNm5 = ""
                            If Len(ItmNm1) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
                            End If


                            If Len(ItmNm2) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                            End If

                            If Len(ItmNm3) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                            End If

                            If Len(ItmNm4) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = "." Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "/" Or Mid$(Trim(ItmNm4), I, 1) = "_" Or Mid$(Trim(ItmNm4), I, 1) = "(" Or Mid$(Trim(ItmNm4), I, 1) = ")" Or Mid$(Trim(ItmNm4), I, 1) = "\" Or Mid$(Trim(ItmNm4), I, 1) = "[" Or Mid$(Trim(ItmNm4), I, 1) = "]" Or Mid$(Trim(ItmNm4), I, 1) = "{" Or Mid$(Trim(ItmNm4), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm5 = Microsoft.VisualBasic.Right(Trim(ItmNm4), Len(ItmNm4) - I)
                                ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm4), I - 1)
                            End If


                            szNm1 = Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString))
                            szNm2 = ""
                            If Len(szNm1) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(szNm1), I, 1) = " " Or Mid$(Trim(szNm1), I, 1) = "," Or Mid$(Trim(szNm1), I, 1) = "." Or Mid$(Trim(szNm1), I, 1) = "-" Or Mid$(Trim(szNm1), I, 1) = "/" Or Mid$(Trim(szNm1), I, 1) = "_" Or Mid$(Trim(szNm1), I, 1) = "(" Or Mid$(Trim(szNm1), I, 1) = ")" Or Mid$(Trim(szNm1), I, 1) = "\" Or Mid$(Trim(szNm1), I, 1) = "[" Or Mid$(Trim(szNm1), I, 1) = "]" Or Mid$(Trim(szNm1), I, 1) = "{" Or Mid$(Trim(szNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                szNm2 = Microsoft.VisualBasic.Right(Trim(szNm1), Len(szNm1) - I)
                                szNm1 = Microsoft.VisualBasic.Left(Trim(szNm1), I - 1)
                            End If


                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            If Trim(Common_Procedures.settings.CustomerCode) = "1142" Then  ' slp (Tirupur)  
                                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, pFont)

                            Else
                                Common_Procedures.Print_To_PrintDocument(e, szNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, pFont)

                            End If

                            If Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                            End If

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Or Trim(szNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, szNm2, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm3) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm4) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm5) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm5), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            'If Trim(szNm1) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm1), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If

                            'If Trim(szNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format5_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim Inv_No As String = "", InvSubNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        '   If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
        ' Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        Cmp_StateCode = "CODE : 33"
        '  End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        If Trim(Common_Procedures.settings.CustomerCode) = "1501" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SUBI_LOGO1, Drawing.Image), LMargin + 24, CurY, 90, 120)

        End If


        p1Font = New Font("Calibri", 22, FontStyle.Bold)

        If Val(prn_Status) = 0 Then
            Common_Procedures.Print_To_PrintDocument_Green(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
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
        ' Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        ' strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        ' CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCode, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateCode, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        If Val(prn_Status) = 0 Then
            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
        End If

        ' vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"
        vHeading = "INVOICE"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side

            Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")


            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "REF. No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString & " Date :" & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

          
            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            If Val(prn_Status) = 0 Then
                Y1 = CurY + 0.5
                Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
                Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            End If
           

            CurY1 = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            End If
            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                End If
                If Trim(vDelvPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If
            End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

            '    'If Trim(prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If

            'End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    'If Trim(prn_HdDt.Rows(0).Item("DelPanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, C2, CurY, 1, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DelState_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, PageWidth - 15, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, C2 - 80, LnAr(4), C2 - 80, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, C2 - 20, LnAr(4), C2 - 20, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(3))

            If Val(prn_Status) = 0 Then
                Y1 = CurY + 0.5
                Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
                Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            End If

             '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            If Trim(Common_Procedures.settings.CustomerCode) = "1142" Then  ' slp (Tirupur)  
                Common_Procedures.Print_To_PrintDocument(e, "UOM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            End If
         
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format5_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_Status) = 0 Then
                Y1 = CurY + 0.5
                Y2 = CurY + TxtHgt - 15 + TxtHgt
                Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1 - 60, Y2)
            End If
          
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If
            End If


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) - 65, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 95, CurY, LMargin + ClAr(1) + ClAr(2) - 95, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            'Y1 = CurY
            'Y2 = CurY + TxtHgt - 0.5
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)



            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            p2Font = New Font("Webdings", 8, FontStyle.Bold)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction.", LMargin + 35, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Interest @ 20% p.a will be charged on the bill not paid", LMargin + 35, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "within 30 days from the date of Invoice.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Once goods sold Cannot be taken back.", LMargin + 35, CurY, 0, 0, pFont)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST Payable on Reverse Charge", LMargin + C1 - 35, CurY + 3, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "Arbitration Council of Tirupur and award passed will be binding on us.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            If Val(prn_Status) = 0 Then
                Y1 = CurY + 0.5
                Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt + TxtHgt - 15
                Common_Procedures.FillRegionRectangle(e, LMargin + C1 - 60, Y1, PageWidth, Y2)
            End If
           
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 - 30, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6) + 40), CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            If Val(prn_Status) = 0 Then
                Y1 = CurY + 0.55
                Y2 = CurY + TxtHgt - 15 + TxtHgt
                Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 40, Y2)
            End If
           
            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 60, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 80, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 80, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If
            p1Font = New Font("Calibri", 7.5, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above true and correct", PageWidth - 10, CurY + 5, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            '
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            LnAr(14) = CurY


            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 35, CurY, LMargin + ClAr(1) + ClAr(2) - 35, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format6(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String, ItmNm4 As String, ItmNm5 As String
        Dim szNm1 As String, szNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

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
            .Top = 20 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 19 


        NoofItems_PerPage = 15 '13



        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35)
        ClArr(2) = 330 : ClArr(3) = 65 : ClArr(4) = 50 : ClArr(5) = 0 : ClArr(6) = 55 : ClArr(7) = 45 : ClArr(8) = 60
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format6_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
                            ItmNm2 = ""
                            ItmNm3 = ""
                            ItmNm4 = ""
                            ItmNm5 = ""
                            If Len(ItmNm1) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
                            End If


                            If Len(ItmNm2) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                            End If

                            If Len(ItmNm3) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                            End If

                            If Len(ItmNm4) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = "." Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "/" Or Mid$(Trim(ItmNm4), I, 1) = "_" Or Mid$(Trim(ItmNm4), I, 1) = "(" Or Mid$(Trim(ItmNm4), I, 1) = ")" Or Mid$(Trim(ItmNm4), I, 1) = "\" Or Mid$(Trim(ItmNm4), I, 1) = "[" Or Mid$(Trim(ItmNm4), I, 1) = "]" Or Mid$(Trim(ItmNm4), I, 1) = "{" Or Mid$(Trim(ItmNm4), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm5 = Microsoft.VisualBasic.Right(Trim(ItmNm4), Len(ItmNm4) - I)
                                ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm4), I - 1)
                            End If


                            'szNm1 = ItmNm2
                            'szNm2 = ""
                            'If Len(szNm1) > 35 Then
                            '    For I = 35 To 1 Step -1
                            '        If Mid$(Trim(szNm1), I, 1) = " " Or Mid$(Trim(szNm1), I, 1) = "," Or Mid$(Trim(szNm1), I, 1) = "." Or Mid$(Trim(szNm1), I, 1) = "-" Or Mid$(Trim(szNm1), I, 1) = "/" Or Mid$(Trim(szNm1), I, 1) = "_" Or Mid$(Trim(szNm1), I, 1) = "(" Or Mid$(Trim(szNm1), I, 1) = ")" Or Mid$(Trim(szNm1), I, 1) = "\" Or Mid$(Trim(szNm1), I, 1) = "[" Or Mid$(Trim(szNm1), I, 1) = "]" Or Mid$(Trim(szNm1), I, 1) = "{" Or Mid$(Trim(szNm1), I, 1) = "}" Then Exit For
                            '    Next I
                            '    If I = 0 Then I = 35
                            '    szNm2 = Microsoft.VisualBasic.Right(Trim(szNm1), Len(szNm1) - I)
                            '    szNm1 = Microsoft.VisualBasic.Left(Trim(szNm1), I - 1)
                            'End If


                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            '  Common_Procedures.Print_To_PrintDocument(e, szNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)

                            If Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 2, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm3) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm4) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm5) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm5), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            'If Trim(szNm1) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm1), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If

                            'If Trim(szNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format6_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim CurY2 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim Inv_No As String = "", InvSubNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 60, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        '   If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
        ' Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        Cmp_StateCode = "CODE : 33"
        '  End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 22, FontStyle.Bold)

        If Trim(Common_Procedures.settings.CustomerCode) = "1502" Then
            'Dim folder As String = Common_Procedures.AppPath & "\Images\"
            'Dim filename As String = System.IO.Path.Combine(folder, "CREAT_LOGO.jpg")

            e.Graphics.DrawImage(DirectCast(Image.FromFile(Common_Procedures.AppPath & "\Images\CREAT_LOGO.jpg"), Drawing.Image), LMargin + 24, CurY - 5, 100, 120)
            'e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.CREAT_LOGO, Drawing.Image), LMargin + 24, CurY - 5, 100, 120)
        End If

        Common_Procedures.Print_To_PrintDocument_Red(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If


        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
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
        ' Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        ' strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        ' CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCode, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateCode, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' If Val(prn_Status) <> 0 Then
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
        '  End If

        ' vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"
        vHeading = "INVOICE"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try




            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) - 40
            C2 = PrintWidth / 2
            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N):", pFont).Width
            W2 = e.Graphics.MeasureString("Invoice Date ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY - 10 ' + 10
            CurY2 = CurY ' + 10

            'Left Side

            Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + C2 + W2 + 30, CurY1 - 5, 0, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "OR No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, PrintWidth - 10, CurY1 - 3, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
            '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            'End If
            ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)




            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString)
            ItmNm2 = ""
            If Trim(ItmNm1) <> "" Then
                ItmNm1 = "(" & Trim(ItmNm1) & ")"
                If Len(ItmNm1) > 45 Then
                    For i = 45 To 1 Step -1
                        If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 45
                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
                End If
            End If


            Common_Procedures.Print_To_PrintDocument(e, "OUR DC No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            If Trim(ItmNm2) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Party_Dc_No").ToString)
            ItmNm2 = ""
            If Trim(ItmNm1) <> "" Then
                ItmNm1 = "(" & Trim(ItmNm1) & ")"
                If Len(ItmNm1) > 45 Then
                    For i = 45 To 1 Step -1
                        If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 45
                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
                End If
            End If


            Common_Procedures.Print_To_PrintDocument(e, "PARTY DC No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            If Trim(ItmNm2) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + C2 + W2 + 30, CurY1, 0, 0, pFont)
            End If




            'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
            '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            'End If


            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString & " Date :" & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            'CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            'End If

            CurY1 = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, CurY1, PageWidth, CurY1)

            Y1 = CurY2 + 0.5
            Y2 = CurY2 + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, C2 + 30, Y2)


            CurY2 = CurY2 + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF COMPANY  (BILLED TO)", LMargin, CurY2, 2, C2, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY2 = CurY2 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY2, C2 + 30, CurY2)
            LnAr(3) = CurY2
            CurY2 = CurY2 + 5

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY2, 0, 0, p1Font)
            '  Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)


            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY2, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY2, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1502" Then ' GOLDEN TEXTILES
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & IIf(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString <> "", ", " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, ""), LMargin + 10, CurY2, 0, 0, pFont)
                '   Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString & IIf(prn_HdDt.Rows(0).Item("DelAdd4").ToString <> "", ", " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, ""), LMargin + C2 + 10, CurY, 0, 0, pFont)
            Else
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY2, 0, 0, pFont)
                '  Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY2, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            End If

            CurY2 = CurY2 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY2, C2 + 30, CurY2)
            CurY2 = CurY2 + TxtHgt - 12



            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY2, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY2, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY2, 0, PrintWidth, pFont)
                End If

            End If

            'If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
            '    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            'Else
            '    vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            'End If
            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
            '    If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    End If
            '    If Trim(vDelvPanNo) <> "" Then
            '        strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '        Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

            '    'If Trim(prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If

            'End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    'If Trim(prn_HdDt.Rows(0).Item("DelPanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If
            'End If

            CurY2 = CurY2 + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY2, C2 + 30, CurY2)
            LnAr(3) = CurY2
            CurY2 = CurY2 + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY2, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY2, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY2, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, C2, CurY2, 1, 0, pFont)
            End If

            'If Trim(prn_HdDt.Rows(0).Item("DelState_Name").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, PageWidth - 20, CurY, 1, 0, pFont)
            'End If

            CurY2 = CurY2 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY2, PageWidth, CurY2)
            LnAr(4) = CurY2

        
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, C2 - 80, LnAr(4), C2 - 80, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, C2 - 30, LnAr(4), C2 - 30, LnAr(3))
            '  e.Graphics.DrawLine(Pens.Black, PageWidth - 50, LnAr(4), PageWidth - 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(3))

            CurY = CurY2

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format6_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim CurY2 As Single = 0
        Dim TXT_hgt1 As Single = 0
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1 - 55, Y2)
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + C1 - 40, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + C1 - 40, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + C1 - 40, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + C1 - 40, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                End If
            End If


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) - 60, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 - 40, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 70, CurY, LMargin + ClAr(1) + ClAr(2) - 70, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            'Y1 = CurY
            'Y2 = CurY + TxtHgt - 0.5
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            p1Font = New Font("Calibri", 8, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            p2Font = New Font("Webdings", 7, FontStyle.Bold)
            CurY2 = CurY + TxtHgt
            TXT_hgt1 = TxtHgt - 6
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction Only.", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Payments should be made by A/C payee cheque or DD only.", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "This bill is payable in full on demand at time.", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Any duspute(s) arising out of this transaction/contract will be referred to institutional arbitration", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "before the arbitration council of Tirupur as per its rules and regulations and the award passed ", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "will be binding on us", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We can't accept the complaints after stiching, inform the complaints before stiching...............", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "We are responsible for fabric/cut-bits only in the form delivered by us. We will not take  ", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "responsibility after further manufacturing / processing is done.", LMargin + 25, CurY2, 0, 0, p1Font)
            CurY2 = CurY2 + TXT_hgt1
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY2, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "If the payment is across from us the due date 5%/pm interest will be chargeable.", LMargin + 25, CurY2, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If



            CurY = CurY + TxtHgt - 15
            ' p2Font = New Font("Webdings", 8, FontStyle.Bold)
            ' Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction Only.", LMargin + 35, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)


            'Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            ' Common_Procedures.Print_To_PrintDocument(e, "Payments should be made by A/C payee cheque or DD only.", LMargin + 35, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "This bill is payable in full on demand at time.", LMargin + 35, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Once goods sold Cannot be taken back.", LMargin + 35, CurY, 0, 0, p1Font)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

          
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST Payable on Reverse Charge", LMargin + C1 - 50, CurY + 3, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "Arbitration Council of Tirupur and award passed will be binding on us.", LMargin + 35, CurY, 0, 0, pFont)

            If CurY2 > CurY Then CurY = CurY2

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt + TxtHgt - 15
            Common_Procedures.FillRegionRectangle(e, LMargin + C1 - 55, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 - 30, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 2, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6) + 40), CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            Y1 = CurY + 0.55
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 50, Y2)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 40, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 50, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If
            p1Font = New Font("Calibri", 7, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above true and correct", PageWidth - 10, CurY + 5, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            '
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            LnAr(14) = CurY


            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 50, CurY, LMargin + ClAr(1) + ClAr(2) - 50, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Print_PrePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_DOTMATRIX.Click
        prn_Status = 1

        print_Invoice()
        btn_print_Close_Click(sender, e)

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1142" Then ' SLP (DOTMATRIX)
            pnl_Print.Visible = True
            pnl_Back.Enabled = False

            If btn_Print_DOTMATRIX.Enabled And btn_Print_DOTMATRIX.Visible Then
                btn_Print_DOTMATRIX.Focus()
            End If
        Else
            print_Invoice()
        End If
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_SIMPLE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_LASER.Click
        prn_Status = 0
        print_Invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub Printing_GST_Format7(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String, ItmNm4 As String, ItmNm5 As String
        Dim szNm1 As String, szNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

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
            .Top = 20 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 19 


        NoofItems_PerPage = 8
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35)
        ClArr(2) = 280 : ClArr(3) = 65 : ClArr(4) = 45 : ClArr(5) = 0 : ClArr(6) = 90 : ClArr(7) = 55 : ClArr(8) = 65
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format7_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format7_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
                            ItmNm2 = ""
                            ItmNm3 = ""
                            ItmNm4 = ""
                            ItmNm5 = ""
                            If Len(ItmNm1) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
                            End If


                            If Len(ItmNm2) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                            End If

                            If Len(ItmNm3) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                            End If

                            If Len(ItmNm4) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = "." Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "/" Or Mid$(Trim(ItmNm4), I, 1) = "_" Or Mid$(Trim(ItmNm4), I, 1) = "(" Or Mid$(Trim(ItmNm4), I, 1) = ")" Or Mid$(Trim(ItmNm4), I, 1) = "\" Or Mid$(Trim(ItmNm4), I, 1) = "[" Or Mid$(Trim(ItmNm4), I, 1) = "]" Or Mid$(Trim(ItmNm4), I, 1) = "{" Or Mid$(Trim(ItmNm4), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm5 = Microsoft.VisualBasic.Right(Trim(ItmNm4), Len(ItmNm4) - I)
                                ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm4), I - 1)
                            End If


                            szNm1 = Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString))
                            szNm2 = ""
                            If Len(szNm1) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(szNm1), I, 1) = " " Or Mid$(Trim(szNm1), I, 1) = "," Or Mid$(Trim(szNm1), I, 1) = "." Or Mid$(Trim(szNm1), I, 1) = "-" Or Mid$(Trim(szNm1), I, 1) = "/" Or Mid$(Trim(szNm1), I, 1) = "_" Or Mid$(Trim(szNm1), I, 1) = "(" Or Mid$(Trim(szNm1), I, 1) = ")" Or Mid$(Trim(szNm1), I, 1) = "\" Or Mid$(Trim(szNm1), I, 1) = "[" Or Mid$(Trim(szNm1), I, 1) = "]" Or Mid$(Trim(szNm1), I, 1) = "{" Or Mid$(Trim(szNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                szNm2 = Microsoft.VisualBasic.Right(Trim(szNm1), Len(szNm1) - I)
                                szNm1 = Microsoft.VisualBasic.Left(Trim(szNm1), I - 1)
                            End If


                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, szNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, pFont)

                            If Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                            End If

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Or Trim(szNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, szNm2, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm3) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm4) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm5) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm5), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            'If Trim(szNm1) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm1), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If

                            'If Trim(szNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format7_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format7_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim Inv_No As String = "", InvSubNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        '   If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
        ' Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        Cmp_StateCode = "CODE : 33"
        '  End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        If Trim(Common_Procedures.settings.CustomerCode) = "1501" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SUBI_LOGO1, Drawing.Image), LMargin + 24, CurY, 90, 120)

        End If


        p1Font = New Font("Calibri", 22, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1501" Then
            Common_Procedures.Print_To_PrintDocument_Green(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If



        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
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
        ' Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        ' strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        ' CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCode, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateCode, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        ' vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"
        vHeading = "INVOICE"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try




            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side

            Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")


            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString & " Date :" & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            End If
            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
                If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                End If
                If Trim(vDelvPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If
            End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

            '    'If Trim(prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If

            'End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    'If Trim(prn_HdDt.Rows(0).Item("DelPanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If
            'End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, C2, CurY, 1, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DelState_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, PageWidth - 20, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, C2 - 80, LnAr(4), C2 - 80, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, C2 - 30, LnAr(4), C2 - 30, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 160, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format7_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1 - 60, Y2)
            LnAr(6) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + C1 - 30, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If
            End If


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) - 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 35, CurY, LMargin + ClAr(1) + ClAr(2) - 35, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            'Y1 = CurY
            'Y2 = CurY + TxtHgt - 0.5
            'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)



            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            p2Font = New Font("Webdings", 8, FontStyle.Bold)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction.", LMargin + 35, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Intrest @ 20% p.a will be charges on the bill not paid", LMargin + 35, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "within 30 days from the date of Invoice.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Once goods sold Cannot be taken back.", LMargin + 35, CurY, 0, 0, pFont)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, PageWidth, CurY)

            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST Payable on Reverse Charge", LMargin + C1 - 35, CurY + 3, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "Arbitration Council of Tirupur and award passed will be binding on us.", LMargin + 35, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 60, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt + TxtHgt - 15
            Common_Procedures.FillRegionRectangle(e, LMargin + C1 - 60, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 - 30, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6) + 40), CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            Y1 = CurY + 0.55
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 40, Y2)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 60, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 40, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 40, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If
            p1Font = New Font("Calibri", 7.5, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above true and correct", PageWidth - 10, CurY + 5, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            '
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(8) + ClAr(7) + ClAr(6)), CurY)
            LnAr(14) = CurY


            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Size_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor.LostFocus
      
        Get_PriceList_Rate()
    End Sub

    Private Sub Get_PriceList_Rate()
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim Led_ID As Integer
        Dim Itm_ID As Integer
        Dim Siz_ID As Integer


        If Trim(Common_Procedures.settings.CustomerCode) <> "1174" Then Exit Sub

        Try
            Led_ID = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text)))
            Itm_ID = Val(Common_Procedures.Item_NameToIdNo(con, Trim(cbo_ItemName.Text)))
            ' Siz_ID = Val(Common_Procedures.Size_NameToIdNo(con, Trim(cbo_RateFor.Text)))

            If Led_ID = 0 Or Itm_ID = 0 Or Siz_ID = 0 Then Exit Sub


            cmd.Connection = con
            cmd.CommandText = "select a.Rate  FROM Price_List_Details A  LEFT OUTER JOIN Price_List_Head B ON A.Price_List_IdNo =B.Price_List_IdNo WHERE b.Ledger_IdNo  = " & Val(Led_ID) & " and a.Item_IdNo = " & Val(Itm_ID) & " and a.Size_IdNo = " & Val(Siz_ID) & ""
            dr = cmd.ExecuteReader


            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then

                        txt_Rate.Text = Val(dr(0).ToString)

                    End If
                End If
            End If

            dr.Close()



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Printing_GST_Format8(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String, ItmNm4 As String, ItmNm5 As String
        Dim szNm1 As String, szNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

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
            .Top = 20 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 19 


        NoofItems_PerPage = 15

        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If
        If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
            NoofItems_PerPage = NoofItems_PerPage - 2
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(40)
        ClArr(2) = 260 : ClArr(3) = 75 : ClArr(4) = 55 : ClArr(5) = 0 : ClArr(6) = 65 : ClArr(7) = 75 : ClArr(8) = 65
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format8_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format8_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
                            ItmNm2 = ""
                            ItmNm3 = ""
                            ItmNm4 = ""
                            ItmNm5 = ""
                            If Len(ItmNm1) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
                            End If


                            If Len(ItmNm2) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                            End If

                            If Len(ItmNm3) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                            End If

                            If Len(ItmNm4) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = "." Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "/" Or Mid$(Trim(ItmNm4), I, 1) = "_" Or Mid$(Trim(ItmNm4), I, 1) = "(" Or Mid$(Trim(ItmNm4), I, 1) = ")" Or Mid$(Trim(ItmNm4), I, 1) = "\" Or Mid$(Trim(ItmNm4), I, 1) = "[" Or Mid$(Trim(ItmNm4), I, 1) = "]" Or Mid$(Trim(ItmNm4), I, 1) = "{" Or Mid$(Trim(ItmNm4), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm5 = Microsoft.VisualBasic.Right(Trim(ItmNm4), Len(ItmNm4) - I)
                                ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm4), I - 1)
                            End If


                            szNm1 = Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString))
                            szNm2 = ""
                            If Len(szNm1) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(szNm1), I, 1) = " " Or Mid$(Trim(szNm1), I, 1) = "," Or Mid$(Trim(szNm1), I, 1) = "." Or Mid$(Trim(szNm1), I, 1) = "-" Or Mid$(Trim(szNm1), I, 1) = "/" Or Mid$(Trim(szNm1), I, 1) = "_" Or Mid$(Trim(szNm1), I, 1) = "(" Or Mid$(Trim(szNm1), I, 1) = ")" Or Mid$(Trim(szNm1), I, 1) = "\" Or Mid$(Trim(szNm1), I, 1) = "[" Or Mid$(Trim(szNm1), I, 1) = "]" Or Mid$(Trim(szNm1), I, 1) = "{" Or Mid$(Trim(szNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                szNm2 = Microsoft.VisualBasic.Right(Trim(szNm1), Len(szNm1) - I)
                                szNm1 = Microsoft.VisualBasic.Left(Trim(szNm1), I - 1)
                            End If


                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Size_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Size_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, szNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, pFont)

                            If Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                            End If

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Or Trim(szNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, szNm2, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm3) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm4) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm5) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm5), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            'If Trim(szNm1) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm1), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If

                            'If Trim(szNm2) <> "" Then
                            '    CurY = CurY + TxtHgt - 5
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(szNm2), LMargin + ClArr(1) + 30, CurY, 0, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            'End If


                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format8_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format8_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim Inv_No As String = "", InvSubNo As String = ""
        Dim Ph_No(10) As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateNm = "STATE : " & prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
            '  Cmp_StateCode = "CODE : 33"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 15

        If Trim(Common_Procedures.settings.CustomerCode) = "1174" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.WIN_TEC_LOGO, Drawing.Image), (PrintWidth / 2) - 60, CurY, 200, 90)
        End If

        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & Trim(Cmp_GSTIN_No), LMargin + 10, CurY, 0, 0, p1Font)




        Erase Ph_No
        Dim Inc As Integer = 0
        If Trim(Cmp_PhNo) <> "" Then
            Ph_No = Split(Trim(Cmp_PhNo), ",")

            Inc = -1

            Inc = Inc + 1
            If UBound(Ph_No) >= Inc Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ph_No(Inc)), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            Inc = Inc + 1
            If UBound(Ph_No) >= Inc Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ph_No(Inc)), PageWidth - 10, CurY + TxtHgt, 1, 0, pFont)
            End If

            Inc = Inc + 1
            If UBound(Ph_No) >= Inc Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ph_No(Inc)), PageWidth - 10, CurY + TxtHgt + TxtHgt, 1, 0, pFont)
            End If

            Inc = Inc + 1
            If UBound(Ph_No) >= Inc Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(Ph_No(Inc)), PageWidth - 10, CurY + TxtHgt + TxtHgt + TxtHgt, 1, 0, pFont)
            End If

        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Email), LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt




        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt - 5


        p1Font = New Font("Times New Roman", 22, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1501" Then
            Common_Procedures.Print_To_PrintDocument_Green(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If



        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        'ItmNm2 = ""
        'If Trim(ItmNm1) <> "" Then
        '    ItmNm1 = "(" & Trim(ItmNm1) & ")"
        '    If Len(ItmNm1) > 85 Then
        '        For i = 85 To 1 Step -1
        '            If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
        '        Next i
        '        If i = 0 Then i = 85
        '        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
        '        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
        '    End If
        'End If

        'If Trim(ItmNm1) <> "" Then
        '    CurY = CurY + strHeight - 1
        '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
        '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'End If

        'If Trim(ItmNm2) <> "" Then
        '    CurY = CurY + strHeight - 1
        '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
        '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'End If

        CurY = CurY + strHeight - 5
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm & "  / " & Cmp_StateCode, LMargin, CurY, 2, PrintWidth, pFont)

        '***** GST START *****



        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        ' vHeading = Trim(UCase(prn_HdDt.Rows(0).Item("Payment_Method").ToString)) & " INVOICE"
        vHeading = "INVOICE"

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, vHeading, LMargin, CurY, 2, PrintWidth, p1Font)


        Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
        InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")
        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO. : " & Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + 10, CurY + 3, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY + 3, 1, 0, p1Font)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try




            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 55

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF BUYER  ", LMargin, CurY, 2, C2, p1Font)

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)       :", pFont).Width



            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + C2, CurY)
            'LnAr(3) = CurY

            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            '  Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)


            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + C2, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " :", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If



            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + C2, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " State", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, C2, CurY, 1, 0, pFont)
            End If

            'Right Side 
            CurY1 = CurY1 + 10
            Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

            p1Font = New Font("Calibri", 15, FontStyle.Bold)

            'Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + C2 + W1 + 20, CurY1 - 5, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString(), LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Transport_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString(), LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Place of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
           Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "No Of Bundles ", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("NoOf_Bundle").ToString, LMargin + C2 + W1 + 20, CurY1, 0, 0, pFont)



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, C2 - 80, LnAr(4), C2 - 80, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, C2 - 30, LnAr(4), C2 - 30, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format8_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                'If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                'Else
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                'End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
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


            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            CurY1 = CurY
            'Left Side
            CurY1 = CurY1 + 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY1, 0, 0, p1Font)
         
            p2Font = New Font("Webdings", 8, FontStyle.Bold)

            CurY1 = CurY1 + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY1, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur Jurisdiction.", LMargin + 35, CurY1 - 3, 0, 0, pFont)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY1, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Intrest @ 20% p.a will be charges on the bill not paid", LMargin + 35, CurY1 - 3, 0, 0, pFont)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "within 30 days from the date of Invoice.", LMargin + 35, CurY1 - 3, 0, 0, pFont)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 20, CurY1, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, "Once goods sold Cannot be taken back.", LMargin + 35, CurY1 - 3, 0, 0, pFont)


            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt - 15
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + C1 - 50, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt - 15
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + C1 - 50, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            End If

            CurY = CurY + TxtHgt - 15
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + C1 - 50, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + C1 - 50, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                End If
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
                LnAr(9) = CurY
            End If

            CurY = CurY + TxtHgt - 15            
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 - 50, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

        

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 - 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 - 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

           
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
          
            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 - 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 - 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then

                CurY = CurY + TxtHgt

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 - 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

          

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                CurY = CurY + TxtHgt

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 - 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST Payable on Reverse Charge", LMargin + C1 - 60, CurY + 6, 0, 0, p1Font)


            If CurY1 > CurY Then CurY = CurY1

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 60, CurY, PageWidth, CurY)

            'Y1 = CurY + 0.5
            'Y2 = CurY + TxtHgt + TxtHgt - 15 + TxtHgt + TxtHgt - 15
            'Common_Procedures.FillRegionRectangle(e, LMargin + C1 - 60, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 - 50, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR : " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(14) = CurY
            End If
           
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Bank Details : " & BankNm1 & ", " & BankNm2 & ", " & BankNm3 & ", " & BankNm4, LMargin + 10, CurY + 5, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(14) = CurY

            p1Font = New Font("Calibri", 7.5, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above true and correct", PageWidth - 10, CurY + 5, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt - 5
            '
        


            'CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + 35, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_NoOfBundels_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NoOfBundels.KeyDown
        If e.KeyCode = 40 Then
            btn_save.Focus()
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_NoOfBundels_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfBundels.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
           
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
        End If


    End Sub

 Private Sub Printing_GST_Format9(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String, ItmNm4 As String, ItmNm5 As String
        Dim szNm1 As String, szNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

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
            .Top = 20 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.75 ' 19 

        NoofItems_PerPage = 16



        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = Val(35)
        ClArr(2) = 360 : ClArr(3) = 85 : ClArr(4) = 0 : ClArr(5) = 0 : ClArr(6) = 60 : ClArr(7) = 0 : ClArr(8) = 90
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format9_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_GST_Format9_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If


                            ItmNm1 = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNm1 = Trim(ItmNm1) & "  -  " & prn_DetDt.Rows(DetIndx).Item("Item_Description").ToString
                            End If
                            ItmNm2 = ""
                            ItmNm3 = ""
                            ItmNm4 = ""
                            ItmNm5 = ""
                            If Len(ItmNm1) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
                            End If


                            If Len(ItmNm2) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                            End If

                            If Len(ItmNm3) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                            End If

                            If Len(ItmNm4) > 44 Then
                                For I = 44 To 1 Step -1
                                    If Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = "." Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "/" Or Mid$(Trim(ItmNm4), I, 1) = "_" Or Mid$(Trim(ItmNm4), I, 1) = "(" Or Mid$(Trim(ItmNm4), I, 1) = ")" Or Mid$(Trim(ItmNm4), I, 1) = "\" Or Mid$(Trim(ItmNm4), I, 1) = "[" Or Mid$(Trim(ItmNm4), I, 1) = "]" Or Mid$(Trim(ItmNm4), I, 1) = "{" Or Mid$(Trim(ItmNm4), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 44
                                ItmNm5 = Microsoft.VisualBasic.Right(Trim(ItmNm4), Len(ItmNm4) - I)
                                ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm4), I - 1)
                            End If


                            CurY = CurY + TxtHgt

                            DetSNo = DetSNo + 1

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                      
                            If Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                            End If

                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 2, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm3) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm4) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            If Trim(ItmNm5) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm5), LMargin + ClArr(1) + 20, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format9_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Val(prn_InpOpts) <> "0" Then
                                prn_DetIndx = 0
                                prn_DetSNo = 0
                                prn_PageNo = 0

                                DetIndx = 0
                                DetSNo = 0

                                e.HasMorePages = True
                                Return
                            End If

                        End If
                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format9_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0, S1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim CurY1 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vDelvPanNo As String = ""
        Dim vLedPanNo As String = ""
        Dim vHeading As String = ""
        Dim Inv_No As String = "", InvSubNo As String = ""
        Dim Cmp_Head_Width As Single = 0
        Dim BnkDetAr() As String
        Dim BInc As Integer
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code =  '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR BUYER"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If

        End If

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


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        LnAr(1) = CurY

        Cmp_Head_Width = PageWidth - 250
        '__________RIGHT SIDE_________
        CurY1 = CurY
        Y1 = CurY
        p1Font = New Font("Calibri", 22, FontStyle.Bold)
        Common_Procedures.FillRegionRectangle(e, Cmp_Head_Width, TMargin, PageWidth, CurY + 2 * (TxtHgt))
        Common_Procedures.Print_To_PrintDocument_Red(e, "TAX INVOICE", Cmp_Head_Width, CurY1, 2, PageWidth - Cmp_Head_Width, p1Font)

        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        W1 = e.Graphics.MeasureString("INVOICE NO", p1Font).Width

        CurY1 = CurY1 + TxtHgt
        CurY1 = CurY1 + TxtHgt
        e.Graphics.DrawLine(Pens.Black, Cmp_Head_Width, CurY1, PageWidth, CurY1)

        CurY1 = CurY1 + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO ", Cmp_Head_Width + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", Cmp_Head_Width + W1 + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, Cmp_Head_Width + W1 + 30, CurY1, 0, 0, p1Font)

        CurY1 = CurY1 + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "DATE", Cmp_Head_Width + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", Cmp_Head_Width + W1 + 10, CurY1, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, Cmp_Head_Width + W1 + 30, CurY1, 0, 0, p1Font)

        CurY1 = CurY1 + TxtHgt
        CurY1 = CurY1 + TxtHgt + 5
        If PageNo <= 1 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), Cmp_Head_Width, CurY1, 2, PageWidth - Cmp_Head_Width, pFont)
        End If
        CurY1 = 0
        '__________LEFT SIDE__________
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = "" : Cmp_PanNo = "" : Cmp_Email = "" : Cmp_PanCap = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        '***** GST START *****
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        Cmp_StateCode = "CODE : 33"
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 22, FontStyle.Bold)
        e.Graphics.DrawImage(DirectCast(Image.FromFile(Common_Procedures.AppPath & "\Images\ITECH_LOGO.PNG"), Drawing.Image), Cmp_Head_Width / 2, CurY - 5, 100, 40)
    
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If


        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, Cmp_Head_Width, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, Cmp_Head_Width, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, Cmp_Head_Width, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, Cmp_Head_Width, pFont)

        '***** GST START *****

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "  /  " & Cmp_Email), LMargin, CurY, 2, Cmp_Head_Width, pFont)

        CurY = CurY + TxtHgt + 5
        LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_GSTIN_Cap & Cmp_GSTIN_No), LMargin, CurY, 2, Cmp_Head_Width / 2, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PanCap & Cmp_PanNo), Cmp_Head_Width / 2, CurY, 2, Cmp_Head_Width / 2, p1Font)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, Cmp_Head_Width, CurY, Cmp_Head_Width, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "BANK :" & Trim(BankNm1) & "       BRANCH :" & Trim(BankNm2) & "       A/C :" & Trim(BankNm3) & "       IFSC :" & Trim(BankNm4), LMargin, CurY, 2, PageWidth, p1Font)
        CurY = CurY + TxtHgt
        LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

   
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        Try

            BlockInvNoY = CurY
            C2 = ClAr(1) + ClAr(2) + ClAr(3) - 40
            C2 = PrintWidth / 2
            W1 = e.Graphics.MeasureString("Reverse Charge  :", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :   ", pFont).Width

            CurY1 = CurY + 10

            'Left Side

            Inv_No = prn_HdDt.Rows(0).Item("Sales_No").ToString
            InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")


            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo), LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCode, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
       
            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

    
            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY = CurY1 + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECIEVER  (BILLED TO)", LMargin, CurY, 2, PageWidth, p1Font)
            CurY = CurY + TxtHgt + 10


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 5

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin, CurY, 2, PageWidth, p1Font)
         

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin, CurY, 2, PageWidth, p1Font)
        
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin, CurY, 2, PageWidth, p1Font)
          

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin, CurY, 2, PageWidth, p1Font)
        
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin, CurY, 2, PageWidth, p1Font)
         

            CurY = CurY + TxtHgt + 5
            LnAr(3) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12


            vLedPanNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN  :" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin, CurY, 2, PageWidth / 2, p1Font)
                End If


                strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "PAN NO : " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, PageWidth / 2, CurY, 2, PageWidth / 2, p1Font)


            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(3))

            Y1 = CurY + 1
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)
            '***** GST START *****
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
           Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format9_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font, p2Font As Font
        Dim BmsInWrds As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0, C1 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        'Dim BInc As Integer
        'Dim BnkDetAr() As String
        Dim CurY2 As Single = 0
        Dim TXT_hgt1 As Single = 0
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "###########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If


                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 - 40, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15

            p1Font = New Font("Calibri", 8, FontStyle.Regular)
            p2Font = New Font("Webdings", 7, FontStyle.Bold)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
         
            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) - 70, CurY, LMargin + ClAr(1) - 70, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Total  Amount : GST  ", LMargin + C1 - 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "GST Payable on Reverse Charge", LMargin + C1 - 40, CurY + 3, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 55, CurY, PageWidth, CurY)
          

            If CurY2 > CurY Then CurY = CurY2

            Y1 = CurY + 1
            Y2 = CurY + TxtHgt + TxtHgt - 10 + TxtHgt + TxtHgt
         
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 - 30, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 2, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY)
            LnAr(10) = CurY


            CurY = CurY + TxtHgt - 15
           CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        
            CurY = CurY + TxtHgt
           p1Font = New Font("Calibri", 12, FontStyle.Bold)
            e.Graphics.DrawImage(DirectCast(Image.FromFile(Common_Procedures.AppPath & "\Images\ITECH_FOOTER_LOGO.PNG"), Drawing.Image), PageWidth - 150, CurY - 5, 90, 18)
            Common_Procedures.Print_To_PrintDocument(e, "RUPEES ON WORDS : ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            LnAr(14) = CurY
            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt - 5

     
            CurY = CurY + TxtHgt
        
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATURE", PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10
           e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub lbl_SGstAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_SGstAmount.TextChanged
        ';;
    End Sub
End Class
