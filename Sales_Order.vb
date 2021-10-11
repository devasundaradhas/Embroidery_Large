Public Class Sales_Order_entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SALOR-"

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

        txt_LrDate.Text = ""
        txt_CashDiscPerc.Text = ""
        txt_CashDiscAmount.Text = ""
        txt_TradeDiscPerc.Text = ""
        txt_TradeDiscAmount.Text = ""
        lbl_NetAmount.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_Assessable.Text = ""

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

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        txt_OrderNo.Enabled = True
        txt_OrderNo.BackColor = Color.White

        txt_OrderDate.Enabled = True
        txt_OrderDate.BackColor = Color.White

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        'cbo_DocumentThrough.Text = "CREDIT"
        'cbo_Destination.Text = Common_Procedures.Ledger_IdNoToName(con, 22)
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
        Dim LockSTS As Boolean

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName from Sales_Order_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sales_Order_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_Order_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Order_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString
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

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_VatPerc.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc").ToString), "########0.00")
                lbl_VatAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")

                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                txt_TradeDiscPerc.Text = Format(Val(dt1.Rows(0).Item("Extra_Charges").ToString), "########0.00")
                txt_TradeDiscAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Extra_Copies").ToString), "########0.00")

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Order_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.Size_idno = c.Size_idno where a.Sales_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Bags").ToString)

                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Sales_Order_Detail_SlNo").ToString
                        dgv_Details.Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("Sales_Items").ToString)


                        If Val(dgv_Details.Rows(n).Cells(8).Value) <> 0 Then
                            LockSTS = True

                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(i).Cells(j).Style.BackColor = Color.LightGray
                            Next

                        End If

                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("SubTotal_Amount").ToString), "########0.00")
                End With

                dt2.Clear()

            End If

            dt1.Clear()

            Grid_Cell_DeSelect()

            If LockSTS = True Then
                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                txt_OrderDate.Enabled = False
                txt_OrderDate.BackColor = Color.LightGray

                txt_OrderNo.Enabled = False
                txt_OrderNo.BackColor = Color.LightGray

            End If

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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
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

        da = New SqlClient.SqlDataAdapter("select distinct(Despatch_To) from Sales_Order_Head order by Despatch_To", con)
        da.Fill(dt5)
        cbo_Destination.DataSource = dt5
        cbo_Destination.DisplayMember = "Despatch_To"

        da = New SqlClient.SqlDataAdapter("select distinct(Booked_By) from Sales_Order_Head order by Booked_By", con)
        da.Fill(dt6)
        cbo_BookedBy.DataSource = dt6
        cbo_BookedBy.DisplayMember = "Booked_By"

        da = New SqlClient.SqlDataAdapter("select distinct(Document_Through) from Sales_Order_Head order by Document_Through", con)
        da.Fill(dt7)
        cbo_DocumentThrough.DataSource = dt7
        cbo_DocumentThrough.DisplayMember = "Document_Through"

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("CST")
        cbo_TaxType.Items.Add("VAT")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()


        lbl_NoofBundles_Caption.Text = "No.of Bags"


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
        AddHandler txt_box.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatPerc.GotFocus, AddressOf ControlGotFocus
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
        AddHandler txt_Pc_Box.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TradeDiscPerc.GotFocus, AddressOf ControlGotFocus


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
        AddHandler txt_box.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatPerc.LostFocus, AddressOf ControlLostFocus
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
        AddHandler txt_Pc_Box.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TradeDiscPerc.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Noof_Bags.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_box.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VatPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pc_Box.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TradeDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_box.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VatPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pc_Box.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TradeDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress

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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
       
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Sales_Items) from Sales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Alreay some quantity purchased against this PO", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr

            'cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Order_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
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
            cmd.CommandText = "select Sales_Order_No from Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_Order_No"
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

            If movno <> "" Then move_record(movno)

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

            da = New SqlClient.SqlDataAdapter("select Sales_Order_No from Sales_Order_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_Order_No", con)
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
            cmd.CommandText = "select Sales_Order_No from Sales_Order_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Order_No desc"

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
        Dim da As New SqlClient.SqlDataAdapter("select Sales_Order_No from Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Order_No desc", con)
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

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Order_Head", "Sales_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select a.Document_Through, a.Tax_Type, a.Tax_Perc, a.Against_CForm_Status, c.ledger_name as TaxAcName from Sales_Order_Head a  LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Sales_Order_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("Document_Through").ToString <> "" Then cbo_DocumentThrough.Text = dt2.Rows(0).Item("Document_Through").ToString
                If dt2.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Tax_Type").ToString
                If dt2.Rows(0).Item("Tax_Perc").ToString <> "" Then txt_VatPerc.Text = Val(dt2.Rows(0).Item("Tax_Perc").ToString)
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
            cmd.CommandText = "select Sales_Order_No from Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code = '" & Trim(NewCode) & "'"
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
            cmd.CommandText = "select Sales_Order_No from Sales_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code = '" & Trim(NewCode) & "'"
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
        Dim NR As Integer = 0
        Dim Sales_OrderSelection_Code As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sales_Order_Entry, New_Entry) = False Then Exit Sub

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
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
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
            'MessageBox.Show("Invalid Sales_Order A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_Sales_OrderAc.Enabled Then cbo_Sales_OrderAc.Focus()
            'Exit Sub
        End If

        txac_id = 0
        If txac_id = 0 And Val(lbl_VatAmount.Text) <> 0 Then
            txac_id = 20
            'MessageBox.Show("Invalid Tax A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_Sales_OrderAc.Enabled Then cbo_Sales_OrderAc.Focus()
            'Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(4).Value) <> 0 Then


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

                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Boxs", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

        If dgv_Details_Total.RowCount > 0 Then
            vTot_Qty = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
        End If

        If Val(vTot_Qty) = 0 Then
            MessageBox.Show("Invalid Invoice Boxs", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()
            Exit Sub
        End If

        CFrm_STS = 0
        If chk_No_CForm.Checked = True Then CFrm_STS = 1


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Order_Head", "Sales_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


          

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@Sales_OrderDate", dtp_Date.Value.Date)



            If New_Entry = True Then
                cmd.CommandText = "Insert into Sales_Order_Head(Sales_Order_Code ,             Company_IdNo         ,              Sales_Order_No       ,                               for_OrderBy                                  , Sales_Order_Date,           Ledger_IdNo   ,    Sales_OrderAc_IdNo      ,            TaxAc_IdNo    ,              Despatch_To            ,              Booked_By           ,         Transport_IdNo    ,            Freight_ToPay_Amount         ,              Document_Through           ,              Lr_No           ,               Lr_Date          ,               Total_Bags            ,             Total_Qty     ,               SubTotal_Amount         , Total_DiscountAmount, Total_TaxAmount,                Gross_Amount           ,               CashDiscount_Perc        ,               CashDiscount_Amount        ,             Assessable_Value         ,               Tax_Type          ,               Tax_Perc            ,                Tax_Amount           ,              Freight_Amount       ,              AddLess_Amount       ,               Round_Off            ,                Net_Amount                  ,              Order_No             ,                Order_Date         ,                Weight             , Against_CForm_Status         , Extra_Charges                              ,   Total_Extra_Copies                        ,Sales_Order_Selection_Code ) " & _
                                                " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @Sales_OrderDate, " & Str(Val(led_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(txac_id)) & ", '" & Trim(cbo_Destination.Text) & "', '" & Trim(cbo_BookedBy.Text) & "', " & Str(Val(trans_id)) & ", " & Str(Val(txt_Freight_ToPay.Text)) & ", '" & Trim(cbo_DocumentThrough.Text) & "', '" & Trim(txt_LrNo.Text) & "', '" & Trim(txt_LrDate.Text) & "', " & Str(Val(txt_Noof_Bags.Text)) & ", " & Str(Val(vTot_Qty)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ",          0          ,        0       , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(txt_VatPerc.Text)) & ", " & Str(Val(lbl_VatAmount.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & " ,   '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_OrderDate.Text) & "', " & Str(Val(txt_Weight.Text)) & " ,   " & Str(Val(CFrm_STS)) & " ,   " & Str(Val(txt_TradeDiscPerc.Text)) & " ,   " & Str(Val(txt_TradeDiscAmount.Text)) & ",'" & Sales_OrderSelection_Code & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Sales_Order_Head set Sales_Order_Date = @Sales_OrderDate, Ledger_IdNo = " & Str(Val(led_id)) & ", Sales_OrderAc_IdNo = " & Str(Val(saleac_id)) & ", TaxAc_IdNo = " & Str(Val(txac_id)) & ", Despatch_To = '" & Trim(cbo_Destination.Text) & "', Booked_By = '" & Trim(cbo_BookedBy.Text) & "', Transport_IdNo = " & Str(Val(trans_id)) & ", Freight_ToPay_Amount = " & Str(Val(txt_Freight_ToPay.Text)) & ", Document_Through = '" & Trim(cbo_DocumentThrough.Text) & "', Lr_No = '" & Trim(txt_LrNo.Text) & "', Lr_Date = '" & Trim(txt_LrDate.Text) & "', Total_Bags = " & Str(Val(txt_Noof_Bags.Text)) & ", Total_Qty = " & Str(Val(vTot_Qty)) & ", SubTotal_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Perc = " & Str(Val(txt_VatPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_VatAmount.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ",Extra_Charges =  " & Str(Val(txt_TradeDiscPerc.Text)) & ",Total_Extra_Copies =  " & Str(Val(txt_TradeDiscAmount.Text)) & ",   Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & " , Order_No =  '" & Trim(txt_OrderNo.Text) & "' , Order_Date = '" & Trim(txt_OrderDate.Text) & "',   Weight = " & Str(Val(txt_Weight.Text)) & " , Against_CForm_Status =   " & Str(Val(CFrm_STS)) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Sales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code = '" & Trim(NewCode) & "' AND Sales_Items = 0 "
            cmd.ExecuteNonQuery()

            Sno = 0

            With dgv_Details

                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                        itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        unt_id = Common_Procedures.get_FieldValue(con, "Item_Head", "Unit_IdNo", "(Item_IdNo = " & Str(Val(itm_id)) & ")", , tr)

                        Sz_id = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Update Sales_Details set Sales_Date= @Sales_OrderDate,  Ledger_IdNo = " & Str(Val(led_id)) & ",  Sl_No = " & Str(Val(Sno)) & ", Item_Idno = " & Str(Val(itm_id)) & ", Size_IdNo = " & Str(Val(Sz_id)) & " ,  Bags = " & Str(Val(.Rows(i).Cells(3).Value)) & ", Noof_Items = " & Str(Val(.Rows(i).Cells(4).Value)) & ", Unit_Idno = " & Val(unt_id) & ", Rate = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Amount = " & Str(Val(.Rows(i).Cells(5).Value)) & ",Total_Amount = " & Str(Val(.Rows(i).Cells(6).Value)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Order_Code = '" & Trim(NewCode) & "' and Sales_Order_Detail_SlNo = " & Str(Val(.Rows(i).Cells(7).Value))
                        NR = cmd.ExecuteNonQuery()


                        If NR = 0 And Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
                            cmd.CommandText = "Insert into Sales_Order_Details ( Sales_Order_Code,            Company_IdNo          ,              Sales_Order_No             ,                                              for_OrderBy                   , Sales_Order_Date,          Ledger_IdNo    ,            SL_No     ,          Item_IdNo      ,          Unit_IdNo      ,            Size_IdNo   ,   Bags  ,             Noof_Items                 ,                 Rate                     ,                 Amount                   ,                    Total_Amount           ) " & _
                                                    " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @Sales_OrderDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(Sz_id)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & " ," & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " )"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'Ac_id = led_id

            'cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " & _
            '                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Sales_OrderV', @Sales_OrderDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Sales_OrderV', @Sales_OrderDate, 1, " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Sales_OrderV', @Sales_OrderDate, 2, " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text)) - Val(lbl_VatAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'If Val(lbl_VatAmount.Text) <> 0 Then
            '    cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Sales_OrderV', @Sales_OrderDate, 3, " & Str(Val(txac_id)) & ", " & Str(Val(lbl_VatAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
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

        If Val(txt_box.Text) = 0 Then
            MessageBox.Show("Invalid Pcs", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_box.Enabled Then txt_box.Focus()
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
                    .Rows(i).Cells(3).Value = Val(txt_Pc_Box.Text)
                    .Rows(i).Cells(4).Value = Val(txt_box.Text)
                    .Rows(i).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(lbl_Amount.Text), "########0.00")

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = cbo_Size.Text
                .Rows(n).Cells(3).Value = Val(txt_Pc_Box.Text)
                .Rows(n).Cells(4).Value = Val(txt_box.Text)
                .Rows(n).Cells(5).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(6).Value = Format(Val(lbl_Amount.Text), "########0.00")

                .Rows(n).Selected = True

                If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If

        End With

        GrossAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Size.Text = ""
        txt_Pc_Box.Text = ""
        txt_box.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_box.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_box.TextChanged
        Call Amount_Calculation()
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

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus

        With cbo_ItemName
            vcmb_ItmNm = Trim(.Text)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
        End With

    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_SlNo, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_ItemName.Text) <> "" Then
                cbo_Size.Focus()
            Else
                txt_TradeDiscPerc.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_ItemName.Text) <> "" Then
                cbo_Size.Focus()
            Else
                txt_TradeDiscPerc.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1079" Then
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
        'Dim da As SqlClient.SqlDataAdapter
        'Dim dt As New DataTable

        'If Trim(UCase(vcmb_ItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
        '    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a, unit_head b where a.item_name = '" & Trim(cbo_ItemName.Text) & "' and a.unit_idno = b.unit_idno", con)
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then
        '        cbo_Size.Text = dt.Rows(0)("unit_name").ToString
        '        txt_Rate.Text = dt.Rows(0)("Sales_Order_Rate").ToString
        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_Freight.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_DocumentThrough_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DocumentThrough.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DocumentThrough, txt_OrderDate, cbo_Destination, "", "", "", "")
    End Sub

    Private Sub cbo_DocumentThrough_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DocumentThrough.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DocumentThrough, cbo_Destination, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_ItemName, txt_VatPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_VatPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        If Trim(UCase(cbo_TaxType.Text)) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then txt_VatPerc.Text = ""
    End Sub

    Private Sub cbo_BookedBy_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BookedBy.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BookedBy, txt_LrDate, txt_Noof_Bags, "", "", "", "")
    End Sub

    Private Sub cbo_BookedBy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BookedBy.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BookedBy, txt_Noof_Bags, "", "", "", "")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Destination, cbo_DocumentThrough, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub cbo_Destination_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Destination.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Destination, cbo_Transport, "", "", "", "")
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatPerc.KeyPress
        If Trim(cbo_TaxType.Text) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then
            e.Handled = True

        Else
            If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True

        End If
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_VatPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
                        txt_box.Text = Val(.Rows(i).Cells(3).Value)
                        txt_Rate.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, cbo_ItemName, txt_Pc_Box, "size_head", "size_name", "", "(size_idno = 0)")
    End Sub

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, txt_Pc_Box, "size_head", "size_name", "", "(size_idno = 0)")
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
                Condt = " a.Sales_Order_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Sales_Order_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Order_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Order_Code IN (select z.Sales_Order_Code from Sales_Order_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Sales_Order_No, a.Sales_Order_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Sales_Order_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Order_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_Order_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sales_Order_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Order_Date").ToString), "dd-MM-yyyy")
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
            txt_Pc_Box.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
            txt_box.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
            lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")

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
        txt_Pc_Box.Text = ""
        txt_box.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""

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
            cbo_Size.Text = ""
            txt_Pc_Box.Text = ""
            txt_box.Text = ""
            txt_Rate.Text = ""
            lbl_Amount.Text = ""

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

    Private Sub txt_Freight_ToPay_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight_ToPay.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : txt_SlNo.Focus()
        If e.KeyCode = 38 Then e.Handled = True : txt_Weight.Focus()
    End Sub

    Private Sub txt_Freight_ToPay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight_ToPay.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_SlNo.Focus()
        End If
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

        lbl_Amount.Text = Format(Val(txt_box.Text) * Val(txt_Rate.Text), "#########0.00")

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

                If Trim(.Rows(I).Cells(1).Value) <> "" Or Val(.Rows(I).Cells(4).Value) <> 0 Then

                    TotQty = TotQty + Val(dgv_Details.Rows(I).Cells(4).Value)
                    TotAmt = TotAmt + Val(dgv_Details.Rows(I).Cells(6).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .Rows.Count = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotQty)
            .Rows(0).Cells(6).Value = Format(Val(TotAmt), "########0.00")
        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        txt_TradeDiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_TradeDiscPerc.Text) / 100, "#########0.00")

        txt_CashDiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

        lbl_Assessable.Text = Format(Val(lbl_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) - Val(txt_TradeDiscAmount.Text), "#########0.00")

        lbl_VatAmount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_VatPerc.Text) / 100, "#########0.00")

        NtAmt = Val(lbl_GrossAmount.Text) - Val(txt_TradeDiscAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(lbl_VatAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

        txt_RoundOff.Text = Format(Val(lbl_NetAmount.Text) - Val(NtAmt), "#########0.00")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Sales_Order_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sales_Order_Code = '" & Trim(NewCode) & "'", con)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sales_Order_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Order_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Order_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Then

            If prn_Status = 1 Then
                'If Microsoft.VisualBasic.Left(Trim(UCase(prn_HdDt.Rows(0).Item("Company_Name").ToString)), 3) = "BNC" And Microsoft.VisualBasic.InStr(1, Trim(UCase(prn_HdDt.Rows(0).Item("Company_Name").ToString)), "GARMENT") > 0 Then
                Printing_Format1(e)

            Else
                Printing_Format2(e)

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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + 591, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + 720, 140, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + 620, 140, 0, 0, p1Font)
            CurY = TMargin + 180
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + 590, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + 720, 190, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + 620, 190, 0, 0, pFont)

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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Order_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Order_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "NO.    :  " & prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(3), LMargin + H1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4), LMargin + H1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DATE    :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, pFont)

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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + 620, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + 720, 140, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + 620, 140, 0, 0, p1Font)
            CurY = TMargin + 242 ' 235 ' 245
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + 620, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + 720, 190, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + 620, 190, 0, 0, pFont)

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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Size_Name from Sales_Order_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Size_Head c on a.size_idno = c.size_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Order_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "NO.    :  " & prn_HdDt.Rows(0).Item("Sales_Order_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(3), LMargin + H1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4), LMargin + H1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + Cen1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 15, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 25, CurY + 15, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DATE    :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Order_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, pFont)

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


    Private Sub txt_TradeDiscPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TradeDiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub
End Class
