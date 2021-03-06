Public Class SalesEntry_MultiTax
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SALES-"

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
    Private prn_Status As Integer
    Private prn_DetDt1 As New DataTable
    Private prn_DetIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_DetAr(100, 50, 10) As String
    Private prn_OriDupTri As String = ""


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
        pnl_Selection.Visible = False

        txt_DcDate.Text = ""
        txt_CashDiscPerc.Text = ""
        lbl_CashDiscAmount.Text = ""
        txt_VechileNo.Text = ""

        lbl_NetAmount.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_Assessable.Text = ""

        cbo_EntType.Text = "DIRECT"

        pnl_ItemInputs.Enabled = True
        cbo_EntType.Enabled = True
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

        cbo_EntType.Text = "DIRECT"
        cbo_TaxType.Text = "VAT"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

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
        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        '    Grid_Cell_DeSelect()
        'End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
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
        dgv_Details_Total.CurrentCell.Selected = False
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString
                cbo_Transport.Text = Common_Procedures.Transport_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_Dcno.Text = dt1.Rows(0).Item("Dc_No").ToString
                txt_DcDate.Text = dt1.Rows(0).Item("Dc_Date").ToString
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                lbl_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString

                get_TaxType_Description()

                lbl_TaxPerc1.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc").ToString), "########0.00")
                lbl_TaxAmount1.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")
                lbl_TaxPerc2.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc2").ToString), "########0.00")
                lbl_TaxAmount2.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount2").ToString), "########0.00")
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                txt_VechileNo.Text = (dt1.Rows(0).Item("Vehicle_No").ToString)
                cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on a.Unit_idno = c.Unit_idno where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Tax_Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Perc_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Amount_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Tax_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("Sales_Detail_SlNo").ToString
                        dgv_Details.Rows(n).Cells(16).Value = dt2.Rows(i).Item("Sales_Delivery_Code").ToString
                        dgv_Details.Rows(n).Cells(17).Value = dt2.Rows(i).Item("Sales_Delivery_Detail_SlNo").ToString

                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                GrossAmount_Calculation()

                'With dgv_Details_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                '    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("SubTotal_Amount").ToString), "########0.00")
                'End With

                dt2.Clear()

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

    Private Sub SalesEntry_MultiTax_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub SalesEntry_MultiTax_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.SalesEntryType)) = "LABOUR INVOICE" Then
            Pk_Condition = "LBINV-"
            lbl_Title.Text = "LABOUR INVOICE"
        Else
            Pk_Condition = "SALES-"
            lbl_Title.Text = "TAX INVOICE"
        End If

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("NO TAX")
        cbo_TaxType.Items.Add("CST")
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("VAT")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()



        cbo_EntType.Items.Clear()
        cbo_EntType.Items.Add("")
        cbo_EntType.Items.Add("DIRECT")
        cbo_EntType.Items.Add("DELIVERY")





        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Dcno.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Quantity.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPercItemwise.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscAmountItemwise.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPercItemwise.GotFocus, AddressOf ControlGotFocus

        AddHandler lbl_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_TaxPerc1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VechileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntType.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Dcno.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Quantity.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPercItemwise.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscAmountItemwise.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPercItemwise.LostFocus, AddressOf ControlLostFocus


        AddHandler lbl_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_CashDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_TaxPerc1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Description.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VechileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Quantity.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_DcDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Dcno.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscPercItemwise.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscAmountItemwise.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_TaxPercItemwise.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler lbl_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_CashDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler lbl_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler lbl_TaxPerc1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Description.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_VechileNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_DcDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Dcno.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Quantity.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_TaxPerc1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPercItemwise.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscAmountItemwise.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_TaxPercItemwise.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler txt_VechileNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub SalesEntry_MultiTax_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub SalesEntry_MultiTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        Else
            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
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
            dt2 = New DataTable
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_EntType.Text = dt2.Rows(0).Item("Entry_Type").ToString
                If dt2.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Tax_Type").ToString
                'If dt2.Rows(0).Item("Tax_Perc").ToString <> "" Then lbl_TaxPerc1.Text = Val(dt2.Rows(0).Item("Tax_Perc").ToString)
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
            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        Else
            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
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
        Dim txac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sz_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim vTot_Qty As Single = 0
        Dim vTot_DiscAMt As Single = 0
        Dim itm_GrpId As Integer = 0
        Dim VouType As String = ""



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
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If (Trim(UCase(cbo_EntType.Text)) <> "DIRECT" And Trim(UCase(cbo_EntType.Text)) <> "DELIVERY") Then
            MessageBox.Show("Invalid Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EntType.Enabled And cbo_EntType.Visible Then cbo_EntType.Focus()
            Exit Sub
        End If

        trans_id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)
        If trans_id = 0 And Trim(cbo_Transport.Text) <> "" Then
            MessageBox.Show("Invalid Transport Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Transport.Enabled Then cbo_Transport.Focus()
            Exit Sub
        End If

        saleac_id = 0
        If saleac_id = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            saleac_id = 22
            'MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        txac_id = 0
        If txac_id = 0 And Val(lbl_TaxAmount1.Text) <> 0 Then
            txac_id = 20
            'MessageBox.Show("Invalid Tax A/c", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            'Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(4).Value) <> 0 Then


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
                            MessageBox.Show("Invalid uNIT Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(3)
                                .CurrentCell.Selected = True
                            End If
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        NoCalc_Status = False
        'Total_Calculation()

        vTot_Qty = 0
        vTot_DiscAMt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_Qty = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTot_DiscAMt = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If

        If Val(vTot_Qty) = 0 Then
            MessageBox.Show("Invalid Invoice Quantity", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
            Exit Sub
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
                cmd.CommandText = "Insert into Sales_Head(Sales_Code ,             Company_IdNo         ,              Sales_No             ,                               for_OrderBy                                  , Sales_Date,           Ledger_IdNo   ,          SalesAc_IdNo      ,            TaxAc_IdNo    ,         Transport_IdNo    ,              Dc_No           ,               Dc_Date          ,             Total_Qty     ,               SubTotal_Amount           , Total_DiscountAmount, Total_TaxAmount,                Gross_Amount           ,               CashDiscount_Perc        ,               CashDiscount_Amount        ,             Assessable_Value         ,               Tax_Type          ,               Tax_Perc             ,                Tax_Amount           ,              Tax_Perc2             ,                Tax_Amount2           ,              Freight_Amount       ,              AddLess_Amount       ,               Round_Off            ,                Net_Amount                  ,              Order_No             ,                Order_Date         ,       Entry_Type  ,  Vehicle_No  ,ItemWise_DiscAmount  ) " & _
                                    " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(txac_id)) & ", " & Str(Val(trans_id)) & ",  '" & Trim(txt_Dcno.Text) & "', '" & Trim(txt_DcDate.Text) & "',  " & Str(Val(vTot_Qty)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ",          0          ,        0       , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(lbl_CashDiscAmount.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(lbl_TaxPerc1.Text)) & ", " & Str(Val(lbl_TaxAmount1.Text)) & ", " & Str(Val(lbl_TaxPerc2.Text)) & ", " & Str(Val(lbl_TaxAmount2.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & " ,   '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_OrderDate.Text) & "',  '" & Trim(cbo_EntType.Text) & "' , '" & Trim(txt_VechileNo.Text) & "' ,  " & Str(Val(vTot_DiscAMt)) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Sales_Head set Sales_Date = @SalesDate, Ledger_IdNo = " & Str(Val(led_id)) & ", SalesAc_IdNo = " & Str(Val(saleac_id)) & ", TaxAc_IdNo = " & Str(Val(txac_id)) & ",  Transport_IdNo = " & Str(Val(trans_id)) & ", Dc_No = '" & Trim(txt_Dcno.Text) & "', Dc_Date = '" & Trim(txt_DcDate.Text) & "', Vehicle_No = '" & Trim(txt_VechileNo.Text) & "' , Total_Qty = " & Str(Val(vTot_Qty)) & ", SubTotal_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(lbl_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Perc = " & Str(Val(lbl_TaxPerc1.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount1.Text)) & ", Tax_Perc2 = " & Str(Val(lbl_TaxPerc2.Text)) & ", Tax_Amount2 = " & Str(Val(lbl_TaxAmount2.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ",ItemWise_DiscAmount = " & Str(Val(vTot_DiscAMt)) & " ,  Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Entry_Type = '" & Trim(cbo_EntType.Text) & "' , Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & " , Order_No =  '" & Trim(txt_OrderNo.Text) & "' , Order_Date = '" & Trim(txt_OrderDate.Text) & "'   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
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

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" Or Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                        itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        itm_GrpId = Common_Procedures.Item_NameToItemGroupIdNo(con, .Rows(i).Cells(1).Value, tr)
                        unt_id = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        Sno = Sno + 1

                        'cmd.CommandText = "Update Sales_Details set Sales_Date= @SalesDate, Entry_Type = '" & Trim(cbo_EntType.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ",  Sl_No = " & Str(Val(Sno)) & ", Item_Idno = " & Str(Val(itm_id)) & ", ItemGroup_IdNo = " & Str(Val(itm_GrpId)) & ", Size_IdNo = " & Str(Val(Sz_id)) & " ,  Bags = " & Str(Val(.Rows(i).Cells(3).Value)) & ", Noof_Items = " & Str(Val(.Rows(i).Cells(4).Value)) & ", Unit_Idno = " & Val(unt_id) & ", Rate = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Amount = " & Str(Val(.Rows(i).Cells(5).Value)) & ",Total_Amount = " & Str(Val(.Rows(i).Cells(6).Value)) & " ,   Sales_Order_Code = '" & Trim(.Rows(i).Cells(7).Value) & "', Sales_Order_Detail_SlNo = " & Str(Val(.Rows(i).Cells(8).Value)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "' and Sales_Detail_SlNo = " & Str(Val(.Rows(i).Cells(9).Value))
                        'nr = cmd.ExecuteNonQuery()

                        'If nr = 0 Then
                        cmd.CommandText = "Insert into Sales_Details ( Sales_Code,            Company_IdNo          ,              Sales_No             ,                                              for_OrderBy                   , Sales_Date,          Ledger_IdNo    ,            SL_No     ,          Item_IdNo      , ItemGroup_IdNo         ,      Item_Description                    ,          Unit_IdNo      ,                     Noof_Items                 ,                 Rate                     ,                 Tax_Rate            ,                      Amount              ,                      Discount_Perc        ,               Discount_Amount             ,   Cash_Discount_Perc_For_All_Item          ,    Cash_Discount_Amount_For_All_Item       ,                      Tax_Perc             ,                     Tax_Amount              ,                      Total_Amount          ,               Entry_Type         ,                  Sales_Delivery_Code     ,               Sales_dELIVERY_Detail_SlNo    ) " & _
                                                " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & "," & Str(Val(itm_GrpId)) & ",'" & Trim(.Rows(i).Cells(2).Value) & "' , " & Str(Val(unt_id)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " , " & Str(Val(.Rows(i).Cells(9).Value)) & " , " & Str(Val(.Rows(i).Cells(10).Value)) & " , " & Str(Val(.Rows(i).Cells(11).Value)) & " , " & Str(Val(.Rows(i).Cells(12).Value)) & " , " & Str(Val(.Rows(i).Cells(13).Value)) & " , " & Str(Val(.Rows(i).Cells(14).Value)) & " , '" & Trim(cbo_EntType.Text) & "' , '" & Trim(.Rows(i).Cells(16).Value) & "' ,  " & Str(Val(.Rows(i).Cells(17).Value)) & " )"
                        cmd.ExecuteNonQuery()
                        '   End If

                        If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then

                            cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = Receipt_Quantity + " & Str(Val(.Rows(i).Cells(4).Value)) & " where Sales_Delivery_Code = '" & Trim(.Rows(i).Cells(16).Value) & "' and Sales_Delivery_Detail_SlNo = " & Str(Val(.Rows(i).Cells(17).Value)) & " and Ledger_IdNo = " & Str(Val(led_id))
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
                                                    "      Values                  ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",    @SalesDate, " & Str(Val(led_id)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(-1 * Val(dgv_Details.Rows(i).Cells(4).Value)) & "   )"
                            cmd.ExecuteNonQuery()
                        End If
                    End If

                Next

            End With


            If Trim(UCase(Pk_Condition)) = "LBINV-" Then
                VouType = "L.Invoice"
            Else
                VouType = "V.Sales"
            End If

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Ac_id = led_id

            cmd.CommandText = "Insert into Voucher_Head ( Voucher_Code          ,                               For_OrderByCode                              ,                 Company_IdNo     ,               Voucher_No          ,                               For_OrderBy                                 ,       Voucher_Type      , Voucher_Date ,         Debtor_Idno    ,        Creditor_Idno       ,               Total_VoucherAmount         ,           Narration                           , Indicate,                         Year_For_Report                   ,  Entry_Identification  , Voucher_Receipt_Code ) " & _
                                "          Values       ('" & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", '" & Trim(VouType) & "',  @SalesDate  , " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "',     1   , " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "',         ''           )"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details (       Voucher_Code    ,                               For_OrderByCode                              ,                 Company_IdNo     ,               Voucher_No          ,                               For_OrderBy                                  ,        Voucher_Type      , Voucher_Date, SL_No,           Ledger_IdNo  ,                           Voucher_Amount       ,    Narration                                  ,                                    Year_For_Report        ,    Entry_Identification  ) " & _
                              "            Values          ('" & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",   '" & Trim(VouType) & "',  @SalesDate ,   1  , " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "'  ) "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details (     Voucher_Code      ,                               For_OrderByCode                              ,                 Company_IdNo     ,               Voucher_No          ,                               For_OrderBy                                  ,        Voucher_Type     , Voucher_Date, SL_No,            Ledger_IdNo    ,                   Voucher_Amount                                     ,          Narration                            ,                                    Year_For_Report        ,    Entry_Identification ) " & _
                              "            Values          ('" & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",  '" & Trim(VouType) & "',   @SalesDate,    2 , " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text)) - Val(lbl_TaxAmount1.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' ) "
            cmd.ExecuteNonQuery()

            If Val(lbl_TaxAmount1.Text) <> 0 Then
                cmd.CommandText = "Insert into Voucher_Details (         Voucher_Code  ,                               For_OrderByCode                              ,                Company_IdNo      ,              Voucher_No           ,                                 For_OrderBy                                ,        Voucher_Type    , Voucher_Date, SL_No,          Ledger_IdNo     ,              Voucher_Amount         ,            Narration                          ,                                   Year_For_Report         ,    Entry_Identification ) " & _
                                  "            Values          ('" & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", '" & Trim(VouType) & "',   @SalesDate,    3 , " & Str(Val(txac_id)) & ", " & Str(Val(lbl_TaxAmount1.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(NewCode) & "' )"
                cmd.ExecuteNonQuery()
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
        Dim Sz_id As Integer = 0

        itm_id = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

        If Val(itm_id) = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        If Trim(cbo_Unit.Text) <> "" Then
            Sz_id = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)
            If Val(Sz_id) = 0 Then
                MessageBox.Show("Invalid Size", "DOES NOT ADD...", MessageBoxButtons.OK)
                If cbo_Unit.Enabled Then cbo_Unit.Focus()
                Exit Sub
            End If
        End If

        If Val(txt_Quantity.Text) = 0 Then
            MessageBox.Show("Invalid Pcs", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Quantity.Enabled Then txt_Quantity.Focus()
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
                    .Rows(i).Cells(2).Value = (txt_Description.Text)
                    .Rows(i).Cells(3).Value = cbo_Unit.Text

                    .Rows(i).Cells(4).Value = Val(txt_Quantity.Text)
                    .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(txt_TaxRate.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(lbl_Amount.Text), "########0.00")

                    .Rows(i).Cells(8).Value = Format(Val(txt_DiscPercItemwise.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(txt_DiscAmountItemwise.Text), "########0.00")

                    .Rows(i).Cells(10).Value = Format(Val(lbl_DiscPerc_ForAllItem.Text), "########0.00")
                    .Rows(i).Cells(11).Value = Format(Val(lbl_DiscAmount_ForAllItem.Text), "########0.00")

                    .Rows(i).Cells(12).Value = Format(Val(txt_TaxPercItemwise.Text), "########0.00")
                    .Rows(i).Cells(13).Value = Format(Val(lbl_TaxAmount.Text), "########0.00")
                    .Rows(i).Cells(14).Value = Format(Val(lbl_TotalAmount.Text), "########0.00")

                    MtchSTS = True

                    If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = (txt_Description.Text)
                .Rows(n).Cells(3).Value = cbo_Unit.Text

                .Rows(n).Cells(4).Value = Val(txt_Quantity.Text)
                .Rows(n).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(6).Value = Format(Val(txt_TaxRate.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(lbl_Amount.Text), "########0.00")

                .Rows(n).Cells(8).Value = Format(Val(txt_DiscPercItemwise.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(txt_DiscAmountItemwise.Text), "########0.00")

                .Rows(n).Cells(10).Value = Format(Val(lbl_DiscPerc_ForAllItem.Text), "########0.00")
                .Rows(n).Cells(11).Value = Format(Val(lbl_DiscAmount_ForAllItem.Text), "########0.00")

                .Rows(n).Cells(12).Value = Format(Val(txt_TaxPercItemwise.Text), "########0.00")
                .Rows(n).Cells(13).Value = Format(Val(lbl_TaxAmount.Text), "########0.00")
                .Rows(n).Cells(14).Value = Format(Val(lbl_TotalAmount.Text), "########0.00")

                If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If

        End With

        GrossAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_Description.Text = ""
        txt_Quantity.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        lbl_Amount.Text = ""
        txt_DiscPercItemwise.Text = ""
        txt_DiscAmountItemwise.Text = ""
        lbl_DiscPerc_ForAllItem.Text = ""
        lbl_DiscAmount_ForAllItem.Text = ""
        txt_TaxPercItemwise.Text = ""
        lbl_TaxAmount.Text = ""
        lbl_TotalAmount.Text = ""

        Grid_Cell_DeSelect()

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()


    End Sub

    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Quantity.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Quantity.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName
            vcmb_ItmNm = .Text
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
        End With
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, cbo_TaxType, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

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
            If Trim(UCase(cbo_ItemName.Text)) <> "" Then
                If Trim(UCase(vcmb_ItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
                    vcmb_ItmNm = cbo_ItemName.Text
                    get_Item_Details()
                End If
            End If

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

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        If Trim(UCase(cbo_ItemName.Text)) <> "" Then
            If Trim(UCase(vcmb_ItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
                vcmb_ItmNm = cbo_ItemName.Text
                get_Item_Details()
            End If
        End If
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
                cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
            End If
            If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
            End If
            If IsDBNull(dt.Rows(0)("Sale_TaxRate").ToString) = False Then
                txt_TaxRate.Text = dt.Rows(0)("Sale_TaxRate").ToString
            End If
            If IsDBNull(dt.Rows(0)("Tax_Percentage").ToString) = False Then
                txt_TaxPercItemwise.Text = dt.Rows(0)("Tax_Percentage").ToString
            End If

        End If
        dt.Dispose()
        da.Dispose()

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_AddLess.Focus() ' SendKeys.Send("+{TAB}")
    End Sub


    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_VechileNo, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_TaxType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_CashDiscPerc.Focus()
                End If

            Else
                If pnl_ItemInputs.Enabled = True And cbo_ItemName.Enabled And cbo_ItemName.Visible Then
                    cbo_ItemName.Focus()
                Else
                    txt_CashDiscPerc.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    txt_CashDiscPerc.Focus()

                End If

            Else

                If pnl_ItemInputs.Enabled = True And cbo_ItemName.Enabled And cbo_ItemName.Visible Then
                    cbo_ItemName.Focus()
                Else
                    txt_CashDiscPerc.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        get_TaxType_Description()
    End Sub

    Private Sub get_TaxType_Description()
        If Trim(UCase(cbo_TaxType.Text)) = "GST" Then
            lbl_TaxType1.Text = "GST"
            lbl_TaxType2.Text = "GST"
        ElseIf Trim(UCase(cbo_TaxType.Text)) = "VAT" Then
            lbl_TaxType1.Text = "VAT"
            lbl_TaxType2.Text = "VAT"
        ElseIf Trim(UCase(cbo_TaxType.Text)) = "CST" Then
            lbl_TaxType1.Text = "CST"
            lbl_TaxType2.Text = "CST"
        Else
            lbl_TaxType1.Text = "TAX"
            lbl_TaxType2.Text = "TAX"
        End If
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_EntType, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If txt_OrderNo.Enabled Then
                txt_OrderNo.Focus()

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                Else
                    txt_CashDiscPerc.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then

                If MessageBox.Show("Do you want to select Delivery?", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If txt_OrderNo.Enabled Then
                        txt_OrderNo.Focus()

                    Else
                        If dgv_Details.Rows.Count > 0 Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                        Else
                            txt_CashDiscPerc.Focus()
                        End If

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_DcDate, txt_VechileNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_VechileNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
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
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub



    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
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
        Dim DISCAMT As Single = 0

        With dgv_Details

            For I = 0 To .RowCount - 1

                If Trim(.Rows(I).Cells(1).Value) <> "" Or Val(.Rows(I).Cells(4).Value) <> 0 Then

                    dgv_Details.Rows(I).Cells(10).Value = Val(txt_CashDiscPerc.Text)

                    DISCAMT = Val(dgv_Details.Rows(I).Cells(7).Value) * Val(txt_CashDiscPerc.Text) / 100
                    dgv_Details.Rows(I).Cells(11).Value = Format(Val(DISCAMT), "#########0.00")

                End If

            Next

        End With


        GrossAmount_Calculation()
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

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub


    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : cbo_ItemName.Focus()
        If e.KeyCode = 38 Then e.Handled = True : txt_VechileNo.Focus()

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

                        txt_Quantity.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                        txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                        txt_TaxRate.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")

                        txt_DiscPercItemwise.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
                        txt_DiscAmountItemwise.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
                        lbl_DiscPerc_ForAllItem.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
                        lbl_DiscAmount_ForAllItem.Text = Format(Val(dgv_Details.CurrentRow.Cells(11).Value), "########0.00")
                        txt_TaxPercItemwise.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
                        lbl_TaxAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")
                        lbl_TotalAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(14).Value), "########0.00")

                        Exit For


                    End If

                Next

            End With

        End If
    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        With cbo_Unit
            vcmb_SizNm = Trim(.Text)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "uNIT_head", "Unit_name", "", "(Unit_IdNo = 0)")
        End With

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, cbo_ItemName, txt_Quantity, "uNIT_head", "Unit_name", "", "(Unit_IdNo = 0)")
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_Quantity, "uNIT_head", "Unit_name", "", "(Unit_IdNo = 0)")
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
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    '.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Siz_idno As Integer = 0
        Dim sqft_qty As Single = 0

        Try
            With dgv_Details
                If .Visible Then
                    If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                            .Rows(.CurrentCell.RowIndex).Cells(6).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(4).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(5).Value), "#########0.00")
                            GrossAmount_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try

    End Sub
    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If pnl_ItemInputs.Enabled = True And txt_SlNo.Enabled = True Then


            If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

                txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                txt_Description.Text = (dgv_Details.CurrentRow.Cells(2).Value)
                cbo_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)

                txt_Quantity.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                txt_TaxRate.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
                lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")

                txt_DiscPercItemwise.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
                txt_DiscAmountItemwise.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
                lbl_DiscPerc_ForAllItem.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
                lbl_DiscAmount_ForAllItem.Text = Format(Val(dgv_Details.CurrentRow.Cells(11).Value), "########0.00")
                txt_TaxPercItemwise.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
                lbl_TaxAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")
                lbl_TotalAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(14).Value), "########0.00")

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
        txt_Description.Text = ""
        txt_Quantity.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        lbl_Amount.Text = ""

        txt_DiscPercItemwise.Text = ""
        txt_DiscAmountItemwise.Text = ""
        lbl_DiscPerc_ForAllItem.Text = ""
        lbl_DiscAmount_ForAllItem.Text = ""
        txt_TaxPercItemwise.Text = ""
        lbl_TaxAmount.Text = ""
        lbl_TotalAmount.Text = ""


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
            txt_Description.Text = ""
            txt_Quantity.Text = ""
            txt_Rate.Text = ""
            txt_TaxRate.Text = ""
            lbl_Amount.Text = ""

            txt_DiscPercItemwise.Text = ""
            txt_DiscAmountItemwise.Text = ""
            lbl_DiscPerc_ForAllItem.Text = ""
            lbl_DiscAmount_ForAllItem.Text = ""
            txt_TaxPercItemwise.Text = ""
            lbl_TaxAmount.Text = ""
            lbl_TotalAmount.Text = ""


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
        Dim AssVal As Single = 0

        lbl_Amount.Text = Format(Val(txt_Quantity.Text) * Val(txt_Rate.Text), "#########0.00")

        txt_DiscAmountItemwise.Text = Format(Val(lbl_Amount.Text) * Val(txt_DiscPercItemwise.Text) / 100, "#########0.00")

        lbl_DiscPerc_ForAllItem.Text = txt_CashDiscPerc.Text
        lbl_DiscAmount_ForAllItem.Text = Format(Val(lbl_Amount.Text) * Val(lbl_DiscPerc_ForAllItem.Text) / 100, "#########0.00")

        AssVal = Val(lbl_Amount.Text) - Val(txt_DiscAmountItemwise.Text) - Val(lbl_DiscAmount_ForAllItem.Text)

        lbl_TaxAmount.Text = "0.00"
        If Trim(cbo_TaxType.Text) <> "" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
            lbl_TaxAmount.Text = Format(Val(AssVal) * Val(txt_TaxPercItemwise.Text) / 100, "#########0.00")
        End If

        lbl_TotalAmount.Text = Format(Val(lbl_Amount.Text) - Val(txt_DiscAmountItemwise.Text) - Val(lbl_DiscAmount_ForAllItem.Text) + Val(lbl_TaxAmount.Text), "########0.00")

    End Sub

    Private Sub GrossAmount_Calculation()
        Dim I As Integer
        Dim Sno As Integer
        Dim TotQty As Decimal, TotAmt As Decimal
        Dim Tot_ItmWs_DisAmt As Single = 0
        Dim Tot_CashDisAmt As Single = 0
        Dim vTaxPerc1 As Single = 0, vTaxPerc2 As Single = 0
        Dim vTaxAmt1 As Single = 0, vTaxAmt2 As Single = 0
        Dim Tot_TaxAmt As Single = 0
        Dim Tot_GrsAmt As Single = 0

        Sno = 0
        TotQty = 0
        TotAmt = 0
        Tot_ItmWs_DisAmt = 0
        Tot_CashDisAmt = 0
        Tot_TaxAmt = 0
        Tot_GrsAmt = 0
        vTaxPerc1 = 0 : vTaxPerc2 = 0
        vTaxAmt1 = 0 : vTaxAmt2 = 0


        With dgv_Details

            For I = 0 To .RowCount - 1
                Sno = Sno + 1
                dgv_Details.Rows(I).Cells(0).Value = Sno

                If Trim(.Rows(I).Cells(1).Value) <> "" Or Val(.Rows(I).Cells(4).Value) <> 0 Then

                    TotQty = TotQty + Val(dgv_Details.Rows(I).Cells(4).Value)
                    TotAmt = TotAmt + Val(dgv_Details.Rows(I).Cells(7).Value)

                    Tot_ItmWs_DisAmt = Tot_ItmWs_DisAmt + Val(dgv_Details.Rows(I).Cells(9).Value)
                    Tot_CashDisAmt = Tot_CashDisAmt + Val(dgv_Details.Rows(I).Cells(11).Value)
                    Tot_TaxAmt = Tot_TaxAmt + Val(dgv_Details.Rows(I).Cells(13).Value)
                    Tot_GrsAmt = Tot_GrsAmt + Val(dgv_Details.Rows(I).Cells(14).Value)

                    If Val(dgv_Details.Rows(I).Cells(13).Value) <> 0 Then
                        If Val(vTaxPerc1) = 0 Or Val(vTaxPerc1) = Val(dgv_Details.Rows(I).Cells(12).Value) Then
                            vTaxPerc1 = Val(dgv_Details.Rows(I).Cells(12).Value)
                            vTaxAmt1 = vTaxAmt1 + Val(dgv_Details.Rows(I).Cells(13).Value)
                        ElseIf Val(vTaxPerc2) = 0 Or Val(vTaxPerc2) = Val(dgv_Details.Rows(I).Cells(12).Value) Then
                            vTaxPerc2 = Val(dgv_Details.Rows(I).Cells(12).Value)
                            vTaxAmt2 = vTaxAmt2 + Val(dgv_Details.Rows(I).Cells(13).Value)
                        End If
                    End If

                End If

            Next

        End With


        With dgv_Details_Total
            If .Rows.Count = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotQty)
            .Rows(0).Cells(7).Value = Format(Val(TotAmt), "########0.00")

            .Rows(0).Cells(9).Value = Format(Val(Tot_ItmWs_DisAmt), "########0.00")
            .Rows(0).Cells(11).Value = Format(Val(Tot_CashDisAmt), "########0.00")
            .Rows(0).Cells(13).Value = Format(Val(Tot_TaxAmt), "########0.00")
            .Rows(0).Cells(14).Value = Format(Val(Tot_GrsAmt), "########0.00")

        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")
        lbl_CashDiscAmount.Text = Format(Val(Tot_CashDisAmt), "########0.00")
        lbl_Assessable.Text = Format(Val(TotAmt) - Val(Tot_ItmWs_DisAmt) - Val(Tot_CashDisAmt), "#########0.00")
        lbl_TaxPerc1.Text = Val(vTaxPerc1)
        lbl_TaxAmount1.Text = Format(Val(vTaxAmt1), "########0.00")
        lbl_TaxPerc2.Text = Val(vTaxPerc2)
        lbl_TaxAmount2.Text = Format(Val(vTaxAmt2), "########0.00")

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal = 0

        NtAmt = Val(lbl_Assessable.Text) + Val(lbl_TaxAmount1.Text) + Val(lbl_TaxAmount2.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

        txt_RoundOff.Text = Format(Val(lbl_NetAmount.Text) - Val(NtAmt), "#########0.00")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_Invoice.Enabled And btn_Print_Invoice.Visible Then
            btn_Print_Invoice.Focus()
        End If
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

        prn_InpOpts = ""
        If Trim(Common_Procedures.settings.CustomerCode) = "1107" Then
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. ExtraCopy                                         ", "FOR INVOICE PRINTING...", "123")
            'prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. ExtraCopy                                         5. All                         ", "FOR INVOICE PRINTING...", "5")

            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "123")

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

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
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetDt1.Clear()
        DetIndx = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sales_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' ", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Sales_Details a  where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Sales_No", con)
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

        'If Trim(Common_Procedures.settings.CustomerCode) = "1017" Then
        '    Printing_Format1(e)
        'Else
        '    Printing_Format3(e)
        'End If

        If prn_Status = 2 Then
            Printing_Format3(e)

        Else
            Printing_Format1(e)

        End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNmDesc As String = ""
        Dim ItmDescAr(20) As String

        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim m1 As Integer = 0
        Dim k As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 35
            .Bottom = 35
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        NoofItems_PerPage = 15
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 350 : ClArr(3) = 85 : ClArr(4) = 80 : ClArr(5) = 85
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNmDesc = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Item_IdNO").ToString))
                        If (prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "" Then
                            ItmNmDesc = Trim(ItmNmDesc) & "  -  " & prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString
                        End If

                        Erase ItmDescAr
                        ItmDescAr = New String(20) {}

                        m1 = -1

LOOP1:
                        If Len(ItmNmDesc) > 40 Then
                            For k = 40 To 1 Step -1
                                If Mid$(ItmNmDesc, k, 1) = " " Or Mid$(ItmNmDesc, k, 1) = "," Or Mid$(ItmNmDesc, k, 1) = "/" Or Mid$(ItmNmDesc, k, 1) = "\" Or Mid$(ItmNmDesc, k, 1) = "-" Or Mid$(ItmNmDesc, k, 1) = "." Or Mid$(ItmNmDesc, k, 1) = "&" Or Mid$(ItmNmDesc, k, 1) = "_" Then Exit For
                            Next k
                            If k = 0 Then k = 40
                            m1 = m1 + 1
                            ItmDescAr(m1) = Microsoft.VisualBasic.Left(Trim(ItmNmDesc), k)
                            'ItmDescAr(m1) = Microsoft.VisualBasic.Left(Trim(ItmNmDesc), K - 1)
                            ItmNmDesc = Microsoft.VisualBasic.Right(ItmNmDesc, Len(ItmNmDesc) - k)
                            GoTo LOOP1

                        Else

                            m1 = m1 + 1
                            ItmDescAr(m1) = ItmNmDesc

                        End If


                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(0)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1


                        For k = 1 To m1
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(k)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        Next k

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim i As Integer = 0
        Dim strHeight As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim C1 As Single, W1 As Single, S1 As Single, S2 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_ShrtName As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim strWidth As String
        Dim CurX As Single = 0
        Dim OrdNoDt As String = ""
        Dim DcNoDt As String = ""


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sales_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(EntryCode) & "'  Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Cmp_ShrtName = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_ShrtName = prn_HdDt.Rows(0).Item("Company_ShortName").ToString

        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then '---- GAJAKHARNAA TRADERS (Somanur)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1091" Then '---- Sri Arul Engineering Works
                If InStr(1, Trim(UCase(Cmp_Name)), "ARUL") > 0 Then
                    e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.Company_Logo_Arul, Drawing.Image), LMargin, CurY + 20, 150, 100)
                ElseIf InStr(1, Trim(UCase(Cmp_Name)), "AVS") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "A V S") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "A.V.S") > 0 Then
                    e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.Company_Logo_Avs, Drawing.Image), LMargin, CurY + 20, 150, 100)
                End If
            End If

            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
            ItmNm2 = ""
            If Trim(ItmNm1) <> "" Then
                ItmNm1 = "(" & Trim(ItmNm1) & ")"
                If Len(ItmNm1) > 75 Then
                    For i = 75 To 1 Step -1
                        If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                    Next i
                    If i = 0 Then i = 75
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



            CurY = CurY + strHeight - 0.5
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


            strWidth = e.Graphics.MeasureString(Cmp_PhNo & "      " & Cmp_Email, pFont).Width

            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, CurX, CurY, 0, PrintWidth, pFont)


            strWidth = e.Graphics.MeasureString(Cmp_PhNo, pFont).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "      " & Cmp_Email, CurX, CurY, 0, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1091" Then '---- Sri Arul Engineering Works (Thekkalur)
                If Trim(UCase(Pk_Condition)) = "LBINV-" Then
                    Common_Procedures.Print_To_PrintDocument(e, "LABOUR BILL", LMargin, CurY, 2, PrintWidth, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "CASH BILL", LMargin, CurY, 2, PrintWidth, p1Font)
                End If
            Else
                If Trim(UCase(Pk_Condition)) = "LBINV-" Then
                    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
                End If
            End If

            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY
        End If

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("INVOICE DATE   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            S2 = e.Graphics.MeasureString("ORDER.NO & DATE :    ", pFont).Width

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + 10, CurY, 0, 0, pFont)

            End If
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)

            End If
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            OrdNoDt = prn_HdDt.Rows(0).Item("Order_No").ToString
            'If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
            '    OrdNoDt = Trim(OrdNoDt) & "  Dt : " & Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString)
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(OrdNoDt) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(OrdNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If


            DcNoDt = prn_HdDt.Rows(0).Item("Dc_No").ToString
            'If Trim(prn_HdDt.Rows(0).Item("Dc_date").ToString) <> "" Then
            '    DcNoDt = Trim(DcNoDt) & "  Dt : " & Trim(prn_HdDt.Rows(0).Item("Dc_date").ToString)
            'End If

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(DcNoDt) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(DcNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then
                e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            Else
                e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(1))

            End If
          
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim p1Font As Font
        Dim I As Integer
        Dim BmsInWrds As String
        Dim W1 As Single = 0
        Dim CurY1 As Single = 0
        Dim Cmp_Name As String
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim Yax As Single
        Dim vprn_PckNos As String = ""
        Dim Tot_Wgt As Single = 0, Tot_Amt As Single = 0, Tot_Bgs As Single = 0, Tot_Wgt_Bag As Single = 0
        W1 = e.Graphics.MeasureString("Payment Terms : ", pFont).Width

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, " " & (prn_HdDt.Rows(0).Item("Gross_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then
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
            End If
            

            CurY = CurY - 10

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then
                CurY = CurY + TxtHgt + 1
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + 300, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If
                End If

                CurY = CurY + TxtHgt + 3 ' 7
                If Val(prn_HdDt.Rows(0).Item("ItemWise_DiscAmount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ItemWise_DiscAmount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 3
                ' CurY = CurY + TxtHgt + 15 ' 7
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_HdDt.Rows(0).Item("Tax_Type").ToString)) & " " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7 ' 7
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount2").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_HdDt.Rows(0).Item("Tax_Type").ToString)) & " " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc2").ToString)) & " %", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount2").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 3
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then

                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If
                End If

                CurY = CurY + TxtHgt + 3
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then

                    If is_LastPage = True Then

                        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                    End If
                End If

            Else

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 1
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If
                End If


                If Val(prn_HdDt.Rows(0).Item("ItemWise_DiscAmount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 3 ' 7
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ItemWise_DiscAmount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If


                ' CurY = CurY + TxtHgt + 15 ' 7
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 3
                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_HdDt.Rows(0).Item("Tax_Type").ToString)) & " " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                ' 7
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount2").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 7
                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_HdDt.Rows(0).Item("Tax_Type").ToString)) & " " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc2").ToString)) & " %", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount2").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If


                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 3
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                    End If
                End If


                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 3
                    If is_LastPage = True Then

                        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                    End If
                End If
            End If
           

            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
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
            End If
            CurY = CurY + TxtHgt + 10

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1107" Then
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(6) = CurY
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
                CurY = CurY + TxtHgt - 5
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                BmsInWrds = Replace(Trim(BmsInWrds), "", "")

                StrConv(BmsInWrds, vbProperCase)
                Common_Procedures.Print_To_PrintDocument(e, "Rupees    : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                'If Val(Common_Procedures.User.IdNo) <> 1 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
                'End If

                CurY = CurY + TxtHgt
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 5, CurY, 1, 0, pFont)
                CurY = CurY + TxtHgt + 10
            End If
          

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        '  Dim ItmNm1 As String, ItmNm2 As String
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim sno As Integer = 0
        Dim ItmNmDesc As String = ""
        Dim Unit As String = ""
        Dim ItmDescAr(20) As String
        Dim strHeight As Single = 0
        Dim W1 As Single = 0
        Dim m1 As Integer = 0
        Dim k As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

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

                    CurY = 410 ' 435 ' 450

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
                            Unit = ""
                            Unit = Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("unit_IdNO").ToString))


                            ItmNmDesc = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Item_IdNO").ToString))
                            If (prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "" Then
                                ItmNmDesc = Trim(ItmNmDesc) & "  -  " & prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString
                            End If

                            Erase ItmDescAr
                            ItmDescAr = New String(20) {}

                            m1 = -1


LOOP1:
                            If Len(ItmNmDesc) > 40 Then
                                For k = 40 To 1 Step -1
                                    If Mid$(ItmNmDesc, k, 1) = " " Or Mid$(ItmNmDesc, k, 1) = "," Or Mid$(ItmNmDesc, k, 1) = "/" Or Mid$(ItmNmDesc, k, 1) = "\" Or Mid$(ItmNmDesc, k, 1) = "-" Or Mid$(ItmNmDesc, k, 1) = "." Or Mid$(ItmNmDesc, k, 1) = "&" Or Mid$(ItmNmDesc, k, 1) = "_" Then Exit For
                                Next k
                                If k = 0 Then k = 40
                                m1 = m1 + 1
                                ItmDescAr(m1) = Microsoft.VisualBasic.Left(Trim(ItmNmDesc), k)
                                'ItmDescAr(m1) = Microsoft.VisualBasic.Left(Trim(ItmNmDesc), K - 1)
                                ItmNmDesc = Microsoft.VisualBasic.Right(ItmNmDesc, Len(ItmNmDesc) - k)
                                GoTo LOOP1

                            Else

                                m1 = m1 + 1
                                ItmDescAr(m1) = ItmNmDesc

                            End If

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("SL_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(0)), LMargin + 70, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString) & " / " & Unit, LMargin + 470, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 580, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)

                            'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("SL_No").ToString), LMargin + 40, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 80, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Size_Name").ToString, LMargin + 365, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 520, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + 610, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 730, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            For k = 1 To m1
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(k)), LMargin + 70, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            Next k

                            DetIndx = DetIndx + 1
                            prn_DetIndx = prn_DetIndx + 1
                        Loop

                    End If

                    Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, PageHeight, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then


                            If Trim(Val(prn_InpOpts)) <> "0" Then
                                DetIndx = 0
                                DetSNo = 0
                                prn_PageNo = 0

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
        Dim S As String = ""

        PageNo = PageNo + 1
        prn_Count = prn_Count + 1
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
                    prn_OriDupTri = "EXTRA COPY"
                End If

            End If
        End If

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, 10, 1, 0, pFont)
        End If

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
            CurY = TMargin + 120  ' 185 ' 200
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 560, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 720, 140, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + 620, 140, 0, 0, p1Font)
            CurY = TMargin + 180 ' 235 ' 245
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 560, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 720, 190, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 620, 190, 0, 0, pFont)

            CurY = TMargin + 220 ' 150  ' 110
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

            LM = LMargin

            CurY = TMargin + 200
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LM + 620, CurY, 0, 0, pFont)
            CurY = TMargin + 235
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LM + 620, CurY, 0, 0, pFont)


            CurY = TMargin + 270
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LM + 620, CurY, 0, 0, pFont)
            CurY = TMargin + 305
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LM + 620, CurY, 0, 0, pFont)

            CurY = TMargin + 345
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LM + 620, CurY, 0, 0, pFont)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal PageHeight As Single, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String, Rup3 As String
        Dim I As Integer
        Dim CurY As Single = 0
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim Yax As Single

        Try

            If is_LastPage = True Then

                CurY = TMargin + 760 ' 810
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL :", LMargin + 300, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + 460, CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                ' dt1.Rows(0).Item("Tax_Type").ToString()

             

                CurY = CurY + TxtHgt + 15 ' 7
                If Val(prn_HdDt.Rows(0).Item("ItemWise_DiscAmount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ItemWise_DiscAmount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                ' 7
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 7
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & " %", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7 ' 7
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_HdDt.Rows(0).Item("Tax_Type").ToString)) & " " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7 ' 7
                If Val(prn_HdDt.Rows(0).Item("Tax_Amount2").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_HdDt.Rows(0).Item("Tax_Type").ToString)) & " " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc2").ToString)) & " %", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount2").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If


                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 7
                    Common_Procedures.Print_To_PrintDocument(e, "Freight :", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 7
                If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + 300, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + 740, CurY, 1, 0, pFont)
                End If

                CurY = TMargin + 920 ' 940 
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

                If is_LastPage = True Then
                    Erase BnkDetAr
                    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                        BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                        BInc = -1
                        Yax = CurY + 10

                        Yax = Yax + TxtHgt - 10
                        'If Val(prn_PageNo) = 1 Then
                        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                        Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                        'End If

                        p1Font = New Font("Calibri", 11, FontStyle.Bold)
                        BInc = BInc + 1
                        If UBound(BnkDetAr) >= BInc Then
                            Yax = Yax + TxtHgt + 2
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
                'If Len(Rup2) > 55 Then
                '    For I = 55 To 1 Step -1
                '        If Mid$(Trim(Rup2), I, 1) = " " Then Exit For
                '    Next I
                '    If I = 0 Then I = 55
                '    Rup3 = Microsoft.VisualBasic.Right(Trim(Rup2), Len(Rup2) - I)
                '    Rup2 = Microsoft.VisualBasic.Left(Trim(Rup2), I - 1)
                'End If
                CurY = TMargin + 920 ' 975 ' 980
                Common_Procedures.Print_To_PrintDocument(e, Rup1, LMargin + 80, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + 80, CurY, 0, 0, pFont)

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub txt_TradeDiscPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_EntType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EntType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EntType, dtp_Date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_EntType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EntType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EntType, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Qty As Single, Ent_Rate As Single, Ent_PurcRet_Qty As Single
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

            Da = New SqlClient.SqlDataAdapter("select a.*,b.*, c.*, e.Unit_Name, f.Noof_Items as Ent_Sales_Quantity, f.Rate as Ent_Rate, f.Sales_Detail_SlNo as Ent_Sales_SlNo from Sales_Delivery_Details a INNER JOIN Sales_Delivery_Head b ON a.Sales_Delivery_Code = b.Sales_Delivery_Code  INNER JOIN Item_Head c ON a.Item_idno = c.Item_idno  LEFT OUTER JOIN Unit_Head e ON a.Unit_IdNo = e.Unit_IdNo LEFT OUTER JOIN Sales_Details F ON f.Sales_Code = '" & Trim(NewCode) & "' and f.Entry_Type = '" & Trim(cbo_EntType.Text) & "' and a.Sales_Delivery_Code = f.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = f.Sales_Delivery_Detail_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.Quantity  - a.Receipt_Quantity ) > 0 or f.Noof_Items > 0 ) Order by a.For_OrderBy, a.Sales_Delivery_No, a.Sales_Delivery_Detail_SlNo", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Ent_Qty = 0 : Ent_Rate = 0 : Ent_DetSlNo = 0 : Ent_PurcRet_Qty = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_SlNo").ToString) = False Then Ent_DetSlNo = Val(Dt1.Rows(i).Item("Ent_Sales_SlNo").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Sales_Quantity").ToString) = False Then Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Sales_Quantity").ToString)
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    ' If IsDBNull(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString) = False Then Ent_PurcRet_Qty = Val(Dt1.Rows(i).Item("Ent_PurcReturn_Qty").ToString)



                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Sales_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Item_name").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Item_Description").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Unit_Name").ToString
                    .Rows(n).Cells(5).Value = (Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Ent_Qty)

                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Sales_Rate").ToString), "########0.00")
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Amount").ToString), "########0.00")
                    If Val(Ent_Qty) > 0 Then
                        .Rows(n).Cells(8).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(8).Value = ""
                    End If
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Sales_Delivery_Code").ToString
                    .Rows(n).Cells(10).Value = Val(Dt1.Rows(i).Item("Sales_Delivery_Detail_SlNo").ToString)
                    .Rows(n).Cells(11).Value = Val(Ent_DetSlNo)
                    .Rows(n).Cells(12).Value = Val(Ent_Qty)
                    .Rows(n).Cells(13).Value = Val(Ent_Rate)
                    .Rows(n).Cells(14).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Sales_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Order_No").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Order_Date").ToString
                    .Rows(n).Cells(17).Value = Common_Procedures.Transport_IdNoToName(con, Val(Dt1.Rows(i).Item("Transport_IdNo").ToString))
                    .Rows(n).Cells(18).Value = (Dt1.Rows(i).Item("Vehicle_No").ToString)



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

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

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
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Ent_Qty As Single, Ent_Rate As Single

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(12).Value)

                Else
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(5).Value)

                End If

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(13).Value)

                Else
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(6).Value)

                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = Val(Ent_Qty)
                dgv_Details.Rows(n).Cells(5).Value = Val(Ent_Rate)
                dgv_Details.Rows(n).Cells(6).Value = Val(Ent_Rate) '**********************
                dgv_Details.Rows(n).Cells(7).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                dgv_Details.Rows(n).Cells(16).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(17).Value = dgv_Selection.Rows(i).Cells(10).Value

                txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(15).Value
                txt_OrderDate.Text = dgv_Selection.Rows(i).Cells(16).Value
                txt_Dcno.Text = dgv_Selection.Rows(i).Cells(1).Value
                txt_DcDate.Text = dgv_Selection.Rows(i).Cells(14).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(17).Value
                txt_VechileNo.Text = dgv_Selection.Rows(i).Cells(18).Value
                '   dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value

            End If

        Next i

        NoCalc_Status = False



        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        'txt_BillNo.Focus()
        'cbo_EntType.Enabled = False

        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
            dgv_Details.CurrentCell.Selected = True
            cbo_EntType.Enabled = False
            pnl_ItemInputs.Enabled = False
        Else
            txt_VechileNo.Focus()

        End If

    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_EntType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntType.TextChanged
        If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
            pnl_ItemInputs.Enabled = True
            dgv_Details.EditMode = DataGridViewEditMode.EditProgrammatically
            dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            txt_OrderDate.Enabled = True
            txt_OrderNo.Enabled = True
            txt_DcDate.Enabled = True
            txt_Dcno.Enabled = True
            cbo_Transport.Enabled = True
            txt_VechileNo.Enabled = True

        Else

            pnl_ItemInputs.Enabled = False
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
                            If pnl_ItemInputs.Enabled = True And cbo_ItemName.Enabled = True Then
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
            '---MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_VechileNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VechileNo.KeyDown
        If e.KeyCode = 38 Then
            cbo_Transport.Focus()
        End If
        If e.KeyCode = 40 Then
            cbo_TaxType.Focus()
        End If
    End Sub

    Private Sub txt_VechileNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VechileNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_TaxType.Focus()
        End If
    End Sub

    Private Sub txt_DiscPercItemwise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPercItemwise.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPercItemwise_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPercItemwise.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_TaxPercItemwise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPercItemwise.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
            e.Handled = True
        End If
    End Sub

    Private Sub txt_TaxPercItemwise_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_TaxPercItemwise.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_DiscAmountItemwise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscAmountItemwise.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscAmountItemwise_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscAmountItemwise.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        prn_Status = 1
        print_Invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Designs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 2
        print_Invoice()
        btn_print_Close_Click(sender, e)
    End Sub
End Class
