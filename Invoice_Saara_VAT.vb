Public Class Invoice_Saara_VAT
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Pk_Condition As String = "INVSA-"
    Private PkCondition_Gst As String = "GINVS-"

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

    Private BrnTfer_STS As Integer = 0
    Private Agst_STS As Integer = 0

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

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_AmountInWords.Text = "Rupees :                                                                               "
        Chk_BranchTransfer.Checked = False
        chk_AgainstCForm.Checked = False
        cbo_TaxType.Text = "-NIL-"

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

        lbl_GrossAmount.Text = ""

        dgv_Details.Rows.Clear()

        cbo_Type.Text = "DIRECT"
        cbo_PaymentMethod.Text = "CREDIT"
        'txt_DcDate.Text = Date.Today
        cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, 22)
        cbo_TaxAc.Text = Common_Procedures.Ledger_IdNoToName(con, 20)
        txt_SlNo.Text = "1"
        cbo_TaxType.Enabled = True
        txt_TaxPerc.Enabled = True

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
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName, c.Ledger_Name as SalesAcName, d.Ledger_Name as TaxAcName,e.Ledger_Name as OnAcName from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.SalesAc_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.TaxAc_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.OnAc_IdNo = e.Ledger_IdNo where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_DcNo.Text = dt1.Rows(0).Item("Dc_No").ToString
                txt_DcDate.Text = dt1.Rows(0).Item("Dc_Date").ToString
                cbo_SalesAc.Text = dt1.Rows(0).Item("SalesAcName").ToString
                cbo_TaxAc.Text = dt1.Rows(0).Item("TaxAcName").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString
                txt_NoofBundle.Text = dt1.Rows(0).Item("NoOf_Bundle").ToString

                cbo_OnAccount.Text = dt1.Rows(0).Item("OnAcName").ToString
                If Val(dt1.Rows(0).Item("Branch_Transfer_Status").ToString) = 1 Then Chk_BranchTransfer.Checked = True
                If Val(dt1.Rows(0).Item("Against_CForm_Status").ToString) = 1 Then chk_AgainstCForm.Checked = True
                'txt_TotalQty.Text = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_PaymentTerms.Text = dt1.Rows(0).Item("Payment_Terms").ToString

                lbl_DeliveryCode.Text = dt1.Rows(0).Item("Delivery_Code").ToString

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                lbl_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"
                txt_TaxPerc.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc").ToString), "########0.00")
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")

                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")

                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                'lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name,c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            dgv_Details.Rows(n).Cells(2).Value = Common_Procedures.Colour_IdNoToName(con, (dt2.Rows(i).Item("Colour_IdNo").ToString))
                            dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Rolls").ToString)
                            dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Weight_Roll").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Unit_Name").ToString
                            dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Serial_No").ToString
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

            Else

                new_record()

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

    Private Sub Invoice_Saara_VAT_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_UnitName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_UnitName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_OnAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_OnAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Invoice_Saara_VAT_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"

        da = New SqlClient.SqlDataAdapter("select Unit_name from Unit_head order by Unit_name", con)
        da.Fill(dt3)
        cbo_UnitName.DataSource = dt3
        cbo_UnitName.DisplayMember = "Unit_name"

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 28 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_SalesAc.DataSource = dt4
        cbo_SalesAc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select distinct(Serial_No) from Sales_Details order by Serial_No", con)
        da.Fill(dt5)
        cbo_Remarks.DataSource = dt5
        cbo_Remarks.DisplayMember = "Serial_No"


        cbo_PaymentMethod.Items.Clear()
        cbo_PaymentMethod.Items.Add("CASH")
        cbo_PaymentMethod.Items.Add("CREDIT")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("VAT")
        cbo_TaxType.Items.Add("CST")

        cbo_Type.Items.Add("")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("DELIVERY")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_UnitName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_colour.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rolls.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WgtRolls.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_AgainstCForm.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_BranchTransfer.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OnAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PaymentTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Remarks.GotFocus, AddressOf ControlGotFocus
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
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofBundle.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_NoofBundle.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_UnitName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OnAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_colour.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rolls.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WgtRolls.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_AgainstCForm.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_BranchTransfer.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PaymentTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Remarks.LostFocus, AddressOf ControlLostFocus
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
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler cbo_colour.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rolls.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WgtRolls.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PaymentTerms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_AgainstCForm.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Chk_BranchTransfer.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VehicleNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoofBundle.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_NoofBundle.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcDate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler cbo_colour.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rolls.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WgtRolls.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PaymentTerms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_AgainstCForm.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Chk_BranchTransfer.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VehicleNo.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub Invoice_Saara_VAT_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Invoice_Saara_VAT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Saara_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Invoice_Saara_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


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

            cmd.CommandText = "Update Delivery_Head set Invoice_Code = '' Where Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code not like '" & Trim(PkCondition_Gst) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code not like '" & Trim(PkCondition_Gst) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code not like '" & Trim(PkCondition_Gst) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code not like '" & Trim(PkCondition_Gst) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc", con)
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

            New_Entry = True

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "Sales_Code not like '" & Trim(PkCondition_Gst) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select top 1 a.*, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Sales_No desc", con)
            dt2 = New DataTable
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("Payment_Method").ToString <> "" Then cbo_PaymentMethod.Text = dt2.Rows(0).Item("Payment_Method").ToString
                If dt2.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAc.Text = dt2.Rows(0).Item("SalesAcName").ToString
                If dt2.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_TaxAc.Text = dt2.Rows(0).Item("TaxAcName").ToString
                If dt2.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Tax_Type").ToString
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

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "'", con)
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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Saara_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Invoice_Saara_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Invocie No.", "FOR NEW INVOICE INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(InvCode) & "'", con)
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
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim CsParNm As String
        Dim vTotAmt As Single = 0, vTotMtr As Single = 0, vTotWgt As Single = 0, vTotRls As Single = 0
        Dim vforOrdby As Single = 0
        Dim Amt As Single = 0
        Dim L_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim OnAc_ID As Integer = 0
        Dim colr_id As Integer = 0

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Invoice_Saara_Entry, New_Entry) = False Then Exit Sub


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

        OnAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_OnAccount.Text)

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If itm_id = 0 Then
                        MessageBox.Show("Invalid ITEM Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    'If Val(.Rows(i).Cells(5).Value) = 0 Then
                    '    MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(5)
                    '    End If
                    '    Exit Sub
                    'End If

                    Unt_ID = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(6).Value)
                    If Unt_ID = 0 Then
                        MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(6)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(7)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        NetAmount_Calculation()
        'TotalAmount_Calculation()

        vTotAmt = 0 : vTotMtr = 0 : vTotRls = 0 : vTotWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotRls = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotMtr = Format(Val(dgv_Details_Total.Rows(0).Cells(5).Value()), "########0.00")
            vTotWgt = Format(Val(dgv_Details_Total.Rows(0).Cells(7).Value()), "########0.00")
            vTotAmt = Format(Val(dgv_Details_Total.Rows(0).Cells(9).Value()), "########0.00")
        End If


        BrnTfer_STS = 0
        If Chk_BranchTransfer.Checked = True Then BrnTfer_STS = 1

        Agst_STS = 0
        If chk_AgainstCForm.Checked = True Then Agst_STS = 1


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "Sales_Code not like '" & Trim(PkCondition_Gst) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)

            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sales_Head (  Sales_Code  ,              Company_IdNo        ,                Sales_No           ,             for_OrderBy    , Sales_Date,               Payment_Method          ,          Ledger_IdNo    ,        Cash_PartyName  ,             Order_No            ,              Order_Date           ,      Dc_No            ,              Dc_Date           ,                         SalesAc_IdNo      ,           Tax_Type        ,            TaxAc_IdNo     ,                                       Narration           ,   Total_Rolls        ,          Total_Meters         ,                 Total_Qty      ,              SubTotal_Amount          , Total_DiscountAmount, Total_TaxAmount,              Gross_Amount             ,                 CashDiscount_Perc      ,              CashDiscount_Amount         ,             Assessable_Value         ,           Tax_Perc             ,                Tax_Amount           ,              Freight_Amount       ,              AddLess_Amount       ,              Round_Off             ,             Net_Amount                               ,               Vehicle_No          ,              Payment_Terms          ,  OnAc_IdNo     ,          Against_Cform_Status    , Branch_Transfer_Status  , Selection_Type        , NoOf_Bundle ) " & _
                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", @SalesDate, '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", '" & Trim(CsParNm) & "', '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_OrderDate.Text) & "', '" & Trim(txt_DcNo.Text) & "', '" & Trim(txt_DcDate.Text) & "', " & Str(Val(saleac_id)) & ",    '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(txac_id)) & ", '" & Trim(txt_Narration.Text) & "', " & Str(Val(vTotRls)) & "," & Str(Val(vTotMtr)) & "," & Str(Val(vTotWgt)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ",           0         ,       0        , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(lbl_CashDiscAmount.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", '" & Trim(txt_VehicleNo.Text) & "', '" & Trim(txt_PaymentTerms.Text) & "', " & Str(Val(OnAc_ID)) & "," & Str(Val(Agst_STS)) & ", " & Str(Val(BrnTfer_STS)) & " , '" & Trim(cbo_Type.Text) & "',  '" & Trim(txt_NoofBundle.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Sales_Head set Sales_Date = @SalesDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", Cash_PartyName = '" & Trim(CsParNm) & "', Order_No = '" & Trim(txt_OrderNo.Text) & "', Order_Date = '" & Trim(txt_OrderDate.Text) & "', Dc_No = '" & Trim(txt_DcNo.Text) & "', Dc_Date = '" & Trim(txt_DcDate.Text) & "', SalesAc_IdNo = " & Str(Val(saleac_id)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', TaxAc_IdNo = " & Str(Val(txac_id)) & ", Narration = '" & Trim(txt_Narration.Text) & "',Total_Rolls = " & Str(Val(vTotRls)) & ", Total_Meters = " & Str(Val(vTotMtr)) & ", Total_Qty = " & Str(Val(vTotWgt)) & ", SubTotal_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Total_DiscountAmount = 0, Total_TaxAmount = 0, Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(lbl_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Tax_Perc = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", NoOf_Bundle = '" & Trim(txt_NoofBundle.Text) & "', Vehicle_No = '" & Trim(txt_VehicleNo.Text) & "', Payment_Terms = '" & Trim(txt_PaymentTerms.Text) & "',OnAc_IdNo   = " & Str(Val(OnAc_ID)) & " , Against_Cform_Status = " & Str(Val(Agst_STS)) & "   , Branch_Transfer_Status = " & Str(Val(BrnTfer_STS)) & " ,  Selection_Type = '" & Trim(cbo_Type.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Delivery_Head set Invoice_Code = '' Where Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                cmd.CommandText = "Update Delivery_Head set Invoice_Code = '" & Trim(NewCode) & "' Where Delivery_Code = '" & Trim(lbl_DeliveryCode.Text) & "'"
                cmd.ExecuteNonQuery()
            End If

            With dgv_Details

                Sno = 0

                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(7).Value) <> 0 Then

                        itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        colr_id = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Unt_ID = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)

                        If itm_id <> 0 Then

                            Sno = Sno + 1

                            cmd.CommandText = "Insert into Sales_Details ( Sales_Code,             Company_IdNo         ,               Sales_No            ,           for_OrderBy      , Sales_Date,          Ledger_IdNo    ,        Sl_No         ,                                    Item_IdNo      ,             Colour_IdNo                   ,              Rolls                 ,                 Weight_Roll              ,                           Meters                     , Unit_IdNo         , Weight                        ,                 Rate                ,                      Amount              ,                      Total_Amount                            ,       Serial_No ) " & _
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(colr_id)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & "," & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Unt_ID)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ," & Str(Val(.Rows(i).Cells(9).Value)) & "," & Str(Val(.Rows(i).Cells(9).Value)) & ", '" & Trim(.Rows(i).Cells(10).Value) & "')"
                            cmd.ExecuteNonQuery()

                            If Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
                                cmd.CommandText = "Insert into Item_Processing_Details (    Reference_Code    ,             Company_IdNo         ,            Reference_No           ,          for_OrderBy       , Reference_Date,          Ledger_IdNo    ,           Party_Bill_No           ,            SL_No     ,          Item_IdNo      ,     Unit_IdNo           ,                     Quantity               ) " & _
                                                  " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",    @SalesDate , " & Str(Val(led_id)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(Unt_ID)) & ", " & Str(-1 * Val(.Rows(i).Cells(6).Value)) & " )"
                                cmd.ExecuteNonQuery()
                            End If


                        End If

                    End If

                Next i

            End With

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Ac_id = 0
            If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
                Ac_id = 1
            ElseIf Val(OnAc_ID) <> 0 Then
                Ac_id = OnAc_ID
            Else
                Ac_id = led_id
            End If

            cmd.CommandText = "Insert into Voucher_Head (     Voucher_Code            ,          For_OrderByCode   ,             Company_IdNo         ,           Voucher_No              ,             For_OrderBy    , Voucher_Type, Voucher_Date,           Debtor_Idno  ,          Creditor_Idno     ,                Total_VoucherAmount        ,         Narration                                , Indicate,       Year_For_Report                                     ,       Entry_Identification                  , Voucher_Receipt_Code ) " & _
                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ",    'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "',    1    , " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "',          ''          ) "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details (       Voucher_Code                   ,          For_OrderByCode   ,              Company_IdNo        ,           Voucher_No              ,           For_OrderBy      , Voucher_Type, Voucher_Date, SL_No,        Ledger_IdNo     ,                       Voucher_Amount           ,              Narration                        ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              "   Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",  'Sales',  @SalesDate ,   1  , " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            cmd.ExecuteNonQuery()

            Amt = Val(CSng(lbl_NetAmount.Text)) - Val(lbl_TaxAmount.Text) - Val(txt_Freight.Text) - Val(txt_AddLess.Text) - Val(lbl_RoundOff.Text)

            cmd.CommandText = "Insert into Voucher_Details (      Voucher_Code                  ,          For_OrderByCode   ,             Company_IdNo         ,           Voucher_No              ,           For_OrderBy      , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo       ,     Voucher_Amount   ,     Narration                                 ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ",  'Sales',  @SalesDate ,   2  , " & Str(Val(saleac_id)) & ", " & Str(Val(Amt)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            cmd.ExecuteNonQuery()

            If Val(lbl_TaxAmount.Text) <> 0 Then
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo     ,             Voucher_Amount          ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   3  , " & Str(Val(txac_id)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_Freight.Text) <> 0 Then
                L_ID = 9
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   4  , " & Str(Val(L_ID)) & ", " & Str(Val(txt_Freight.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_AddLess.Text) <> 0 Then
                L_ID = 17
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount        ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   5  , " & Str(Val(L_ID)) & ", " & Str(Val(txt_AddLess.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
            End If

            If Val(lbl_RoundOff.Text) <> 0 Then
                L_ID = 24
                cmd.CommandText = "Insert into Voucher_Details ( Voucher_Code                       ,      For_OrderByCode       ,         Company_IdNo             ,           Voucher_No              ,            For_OrderBy     , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo  ,             Voucher_Amount         ,         Narration                             ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @SalesDate,   6  , " & Str(Val(L_ID)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()
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
        Dim Unt_id As Integer
        Dim i As Integer

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


        Unt_id = Common_Procedures.Unit_NameToIdNo(con, cbo_UnitName.Text)
        If Unt_id = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_UnitName.Enabled Then cbo_UnitName.Focus()
            Exit Sub
        End If

        If Val(txt_Weight.Text) = 0 Then
            MessageBox.Show("Invalid Weight", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Weight.Enabled Then txt_Weight.Focus()
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(2).Value = cbo_colour.Text
                    .Rows(i).Cells(3).Value = Val(txt_Rolls.Text)
                    .Rows(i).Cells(4).Value = Format(Val(txt_WgtRolls.Text), "########0.000")
                    .Rows(i).Cells(5).Value = Format(Val(txt_Meters.Text), "########0.00")
                    .Rows(i).Cells(6).Value = cbo_UnitName.Text
                    .Rows(i).Cells(7).Value = Format(Val(txt_Weight.Text), "########0.000")
                    .Rows(i).Cells(8).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(lbl_Amount.Text), "########0.00")
                    .Rows(i).Cells(10).Value = cbo_Remarks.Text
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
                .Rows(n).Cells(2).Value = cbo_colour.Text
                .Rows(n).Cells(3).Value = Val(txt_Rolls.Text)
                .Rows(n).Cells(4).Value = Format(Val(txt_WgtRolls.Text), "########0.000")
                .Rows(n).Cells(5).Value = Format(Val(txt_Meters.Text), "########0.00")
                .Rows(i).Cells(6).Value = cbo_UnitName.Text
                .Rows(n).Cells(7).Value = Format(Val(txt_Weight.Text), "########0.000")
                .Rows(n).Cells(8).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(lbl_Amount.Text), "########0.00")
                .Rows(i).Cells(10).Value = cbo_Remarks.Text
                '.Rows(n).Selected = True

                'If n >= 10 Then .FirstDisplayedScrollingRowIndex = n - 9

            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_UnitName.Text = ""
        cbo_colour.Text = ""
        txt_Rolls.Text = ""
        txt_WgtRolls.Text = ""
        txt_Meters.Text = ""
        txt_Weight.Text = ""
        txt_Rate.Text = ""
        cbo_Remarks.Text = ""
        lbl_Amount.Text = ""
        Grid_Cell_DeSelect()

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_NoofItems_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Weight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Weight.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_SlNo, cbo_UnitName, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
                If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
                    cbo_UnitName.Text = dt.Rows(0)("unit_name").ToString
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
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, dtp_Date, cbo_Type, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, cbo_Type, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        End If
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Type, txt_OrderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                If Trim(cbo_Type.Text) = "DELIVERY" Then

                    If MessageBox.Show("Do You Want To Select Delivery:", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        btn_Selection_Click(sender, e)

                    Else

                        txt_OrderNo.Focus()
                    End If
                Else
                    txt_OrderNo.Focus()

                End If
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_ItemName.Focus() ' SendKeys.Send("+{TAB}")
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
                        cbo_colour.Text = Trim(.Rows(i).Cells(2).Value)
                        txt_Rolls.Text = Trim(.Rows(i).Cells(3).Value)
                        txt_WgtRolls.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.00")
                        txt_Meters.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.00")
                        cbo_UnitName.Text = Trim(.Rows(i).Cells(6).Value)
                        txt_Weight.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.00")
                        txt_Rate.Text = Format(Val(.Rows(i).Cells(8).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(.Rows(i).Cells(9).Value), "########0.00")
                        cbo_Remarks.Text = Trim(.Rows(i).Cells(10).Value)

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
    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_PaymentMethod, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", " ", "")

        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

            txt_SlNo.ReadOnly = True
            '  txt_Rate.ReadOnly = True
            txt_Rolls.ReadOnly = True
            txt_Weight.ReadOnly = True
            txt_WgtRolls.ReadOnly = True
            txt_Meters.ReadOnly = True
            cbo_ItemName.Enabled = False
            cbo_colour.Enabled = False
            cbo_UnitName.Enabled = False

        Else

            txt_SlNo.ReadOnly = False
            '  txt_Rate.ReadOnly = False
            txt_Rolls.ReadOnly = False
            txt_Weight.ReadOnly = False
            txt_WgtRolls.ReadOnly = False
            txt_Meters.ReadOnly = False
            cbo_ItemName.Enabled = True
            cbo_colour.Enabled = True
            cbo_UnitName.Enabled = True
        End If
    End Sub

    Private Sub cbo_UnitName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UnitName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "unit_head", "unit_Name", "", "(unit_idno = 0)")

    End Sub

    Private Sub cbo_UnitName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_UnitName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_UnitName, txt_Meters, txt_Weight, "unit_head", "unit_Name", "", "(unit_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_UnitName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_UnitName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_UnitName, txt_Weight, "unit_head", "unit_Name", "", "(unit_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub cbo_UnitName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_UnitName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_UnitName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then btn_Add.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub
    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
            'SendKeys.Send("{TAB}")
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Code IN (select z.Sales_Code from Sales_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Sales_No, a.Sales_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            dt2 = New DataTable
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
            dt1.Dispose()
            dt2.Dispose()
            da.Dispose()

        End Try

        If dgv_Filter_Details.Rows.Count > 0 Then
            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
        Else
            dtp_Filter_Fromdate.Focus()
        End If

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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



    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
            cbo_colour.Text = dgv_Details.CurrentRow.Cells(2).Value
            txt_Rolls.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
            txt_WgtRolls.Text = Format(Val(dgv_Details.CurrentRow.Cells(4).Value), "########0.00")
            txt_Meters.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
            cbo_UnitName.Text = Trim(dgv_Details.CurrentRow.Cells(6).Value)
            txt_Weight.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
            lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
            cbo_Remarks.Text = Trim(dgv_Details.CurrentRow.Cells(10).Value)

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
        cbo_UnitName.Text = ""
        cbo_colour.Text = ""
        txt_Rolls.Text = ""
        txt_WgtRolls.Text = ""
        txt_Meters.Text = ""
        txt_Weight.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        cbo_Remarks.Text = ""

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
            cbo_UnitName.Text = ""
            cbo_Remarks.Text = ""
            cbo_colour.Text = ""
            txt_Rolls.Text = ""
            txt_WgtRolls.Text = ""
            txt_Meters.Text = ""
            txt_Weight.Text = ""
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

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        If e.KeyCode = 40 Then btn_save.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
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
        Me.Close()
    End Sub

    Private Sub Amount_Calculation()

        If Trim(UCase(cbo_UnitName.Text)) = "KG" Then
            lbl_Amount.Text = Format(Val(txt_Weight.Text) * Val(txt_Rate.Text), "#########0.00")

        ElseIf Trim(UCase(cbo_UnitName.Text)) = "ROLL" Then

            lbl_Amount.Text = Format(Val(txt_Rolls.Text) * Val(txt_Rate.Text), "#########0.00")

        ElseIf Trim(UCase(cbo_UnitName.Text)) = "METER" Then

            lbl_Amount.Text = Format(Val(txt_Meters.Text) * Val(txt_Rate.Text), "#########0.00")

        End If
    End Sub

    Private Sub Weight_Calculation()
        txt_Weight.Text = Format(Val(txt_Rolls.Text) * Val(txt_WgtRolls.Text), "#########0.000")
    End Sub

    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer
        Dim TotWgt As Decimal
        Dim TotAmt As Decimal
        Dim TotRls As Decimal
        Dim TotMtr As Decimal
        Sno = 0
        TotWgt = 0
        TotAmt = 0
        For i = 0 To dgv_Details.RowCount - 1
            Sno = Sno + 1
            dgv_Details.Rows(i).Cells(0).Value = Sno

            If Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                TotRls = TotRls + Val(dgv_Details.Rows(i).Cells(3).Value)
                TotMtr = TotMtr + Val(dgv_Details.Rows(i).Cells(5).Value)
                TotWgt = TotWgt + Val(dgv_Details.Rows(i).Cells(7).Value)
                TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(9).Value)
            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotRls)
            .Rows(0).Cells(5).Value = Format(Val(TotMtr), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(TotWgt), "########0.000")
            .Rows(0).Cells(9).Value = Format(Val(TotAmt), "########0.00")
        End With

        lbl_GrossAmount.Text = Format(TotAmt, "########0.00")

        Gross_Discount_Tax_Amount_Calculation()

    End Sub

    Private Sub Gross_Discount_Tax_Amount_Calculation()

        lbl_CashDiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

        lbl_Assessable.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_CashDiscAmount.Text), "#########0.00")

        lbl_TaxAmount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_TaxPerc.Text) / 100, "#########0.00")

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        NtAmt = Val(lbl_GrossAmount.Text) - Val(lbl_CashDiscAmount.Text) + Val(lbl_TaxAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")

        lbl_AmountInWords.Text = "Rupees :                                                                               "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees : : " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        Else

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

        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Print_PDF_Status = True Then
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
                    'PrintDocument1.Print()

                    ' ''PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    ' ''If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

                    ' ''    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                    ' ''    'PrintDocument1.DocumentName = "c:\test1.pdf"
                    ' ''    'PrintDocument1.Print()

                    ' ''    'Dim bytes As Byte() = PrintDocument1.Print("pdf")
                    ' ''    ''Dim bytes As Byte() = RptViewer.LocalReport.Render("Excel")
                    ' ''    'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.xls")

                    ' ''    'Dim bytes As Byte() = System.IO.File.ReadAllBytes("C:\test1.pdf")
                    ' ''    'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.pdf")
                    ' ''    'fs.Write(bytes, 0, bytes.Length)
                    ' ''    'fs.Close()
                    ' ''    'MessageBox.Show("ok")

                    ' ''    'PrintDocument1.Print()
                    ' ''End If

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on A.unit_idno = c.unit_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

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
        Dim pFont As Font
        Dim p1Font As Font
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
        Dim clr_nm As String = ""
        Dim BlNoAr(20) As String
        Dim vItmDesc As String = "", vItmDesc1 As String = "", vItmDesc2 As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30  ' 65
            .Right = 45
            .Top = 40
            .Bottom = 40
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

        NoofItems_PerPage = 14 ' 10

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 40
        ClArr(2) = 285 : ClArr(3) = 120 : ClArr(4) = 56 : ClArr(5) = 100
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

                'If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

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
                            If Len(ItmNm1) > 35 Then
                                For I = 35 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt + 10

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            If Trim(UCase(prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString)) = "KG" Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)

                            ElseIf Trim(UCase(prn_DetDt.Rows(0).Item("Unit_Name").ToString)) = "METER" Then

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)

                            ElseIf Trim(UCase(prn_DetDt.Rows(0).Item("Unit_Name").ToString)) = "ROLL" Then

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rolls").ToString), "########0"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)

                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            clr_nm = ""
                            If Trim(ItmNm2) <> "" Or Val(prn_DetDt.Rows(DetIndx).Item("Rolls").ToString) <> 0 Or Val(Trim(prn_DetDt.Rows(DetIndx).Item("gsm").ToString)) <> 0 Or Val(prn_DetDt.Rows(DetIndx).Item("Weight_Roll").ToString) <> 0 Or Val(prn_DetDt.Rows(DetIndx).Item("Meters").ToString) <> 0 Or Trim(prn_DetDt.Rows(DetIndx).Item("Serial_No").ToString) <> "" Then

                                vItmDesc = ""


                                clr_nm = Common_Procedures.Colour_IdNoToName(con, Trim(prn_DetDt.Rows(DetIndx).Item("Colour_IdNo").ToString))
                                If clr_nm <> "" Then
                                    vItmDesc = Trim(vItmDesc) & IIf(Trim(vItmDesc) <> "", ", ", "") & Trim(clr_nm)
                                End If

                                If Val(prn_DetDt.Rows(DetIndx).Item("Weight_Roll").ToString) <> 0 Then
                                    vItmDesc = Trim(vItmDesc) & IIf(Trim(vItmDesc) <> "", ", ", "") & Trim(Val(Trim(prn_DetDt.Rows(DetIndx).Item("Weight_Roll").ToString))) & " Kg/Roll"
                                End If

                                If Val(prn_DetDt.Rows(DetIndx).Item("Meters").ToString) <> 0 Then
                                    vItmDesc = Trim(vItmDesc) & IIf(Trim(vItmDesc) <> "", ", ", "") & Trim(Val(Trim(prn_DetDt.Rows(DetIndx).Item("Meters").ToString))) & " Mtr"
                                End If

                                If Trim(prn_DetDt.Rows(DetIndx).Item("Serial_No").ToString) <> "" Then
                                    vItmDesc = Trim(vItmDesc) & IIf(Trim(vItmDesc) <> "", ", ", "") & Trim(prn_DetDt.Rows(DetIndx).Item("Serial_No").ToString)
                                End If

                                If Trim(vItmDesc) <> "" Then
                                    vItmDesc = "(" & Trim(vItmDesc) & ")"
                                End If

                                vItmDesc1 = Trim(vItmDesc)
                                vItmDesc2 = ""
                                If Len(vItmDesc1) > 45 Then
                                    For I = 45 To 1 Step -1
                                        If Mid$(Trim(vItmDesc1), I, 1) = " " Or Mid$(Trim(vItmDesc1), I, 1) = "," Or Mid$(Trim(vItmDesc1), I, 1) = "." Or Mid$(Trim(vItmDesc1), I, 1) = "-" Or Mid$(Trim(vItmDesc1), I, 1) = "/" Or Mid$(Trim(vItmDesc1), I, 1) = "_" Or Mid$(Trim(vItmDesc1), I, 1) = "(" Or Mid$(Trim(vItmDesc1), I, 1) = ")" Or Mid$(Trim(vItmDesc1), I, 1) = "\" Or Mid$(Trim(vItmDesc1), I, 1) = "[" Or Mid$(Trim(vItmDesc1), I, 1) = "]" Or Mid$(Trim(vItmDesc1), I, 1) = "{" Or Mid$(Trim(vItmDesc1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 45
                                    vItmDesc2 = Microsoft.VisualBasic.Right(Trim(vItmDesc1), Len(vItmDesc1) - I)
                                    vItmDesc1 = Microsoft.VisualBasic.Left(Trim(vItmDesc1), I - 1)
                                End If


                                If Trim(UCase(prn_DetDt.Rows(0).Item("Unit_Name").ToString)) <> "ROLL" Then
                                    If Trim(ItmNm2) <> "" Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 30, CurY - 5, 0, 0, pFont)
                                        NoofDets = NoofDets + 1

                                        If Val(prn_DetDt.Rows(DetIndx).Item("Rolls").ToString) <> 0 Then
                                            Common_Procedures.Print_To_PrintDocument(e, "(" & Val(prn_DetDt.Rows(DetIndx).Item("Rolls").ToString) & " Rolls)", LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                        End If

                                    End If

                                End If

                                CurY = CurY + TxtHgt
                                p1Font = New Font("Calibri", 9, FontStyle.Italic)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vItmDesc1), LMargin + ClArr(1) + 15, CurY, 0, 0, p1Font)

                                If Trim(ItmNm2) = "" Then
                                    If Val(prn_DetDt.Rows(DetIndx).Item("Rolls").ToString) <> 0 Then
                                        Common_Procedures.Print_To_PrintDocument(e, "(" & Val(prn_DetDt.Rows(DetIndx).Item("Rolls").ToString) & " Rolls)", LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                    End If
                                End If

                                NoofDets = NoofDets + 1

                                If Trim(vItmDesc2) <> "" Then
                                    CurY = CurY + TxtHgt
                                    p1Font = New Font("Calibri", 10, FontStyle.Regular)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(vItmDesc2), LMargin + ClArr(1) + 15, CurY, 0, 0, p1Font)
                                    NoofDets = NoofDets + 1
                                End If

                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)


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

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String, Led_cstNo As String
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
        'Dim strWidth As String
        Dim CurX As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "' Order by a.sl_no", con)
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
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
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

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) '& IIf(Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & "," & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        End If

        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) '& IIf(Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If
        Else
            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Arial Black", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.Saara_Creations_logo, Drawing.Image), LMargin + 20, CurY + 30, 150, 75)

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_cstNo = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                If UBound(PnAr) >= 5 Then Led_TinNo = Trim(PnAr(5))
                If UBound(PnAr) >= 6 Then Led_cstNo = Trim(PnAr(6))

            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                Led_cstNo = Trim(prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString)

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

            'If Trim(Led_PhNo) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = "Phone No : " & Led_PhNo
            'End If

            If Trim(Led_TinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = "TIN NO : " & Led_TinNo
            End If

            If Trim(Led_cstNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = "CST NO : " & Led_cstNo
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = "Phone No : " & Led_PhNo
            End If

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("INVOICE DATE:", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

            If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MMM-yyyy"), LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MMM-yyyy"), LMargin + Cen1 + W1 + 30, CurY + 10, 0, 0, pFont)

            End If


            'Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MMM-yyyy"), LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY + 25, PageWidth, CurY + 25)
            Else
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY + 40, PageWidth, CurY + 40)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, "DC No.", LMargin + Cen1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, CurY + 10, 0, 0, pFont)

            Else

                Common_Procedures.Print_To_PrintDocument(e, "DC No.", LMargin + Cen1 + 10, CurY + 35, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 35, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, CurY + 35, 0, 0, pFont)

            End If
            'Common_Procedures.Print_To_PrintDocument(e, "DC No.", LMargin + Cen1 + 10, CurY + 10, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 10, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + Cen1 + W1 + 30, CurY + 10, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DC Date", LMargin + Cen1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + Cen1 + W1 + 30, CurY + 10, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "DC Date", LMargin + Cen1 + 10, CurY + 45, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 45, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + Cen1 + W1 + 30, CurY + 45, 0, 0, pFont)

            End If


            'Common_Procedures.Print_To_PrintDocument(e, "DC Date", LMargin + Cen1 + 10, CurY + 10, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 10, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + Cen1 + W1 + 30, CurY + 10, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PO No.", LMargin + Cen1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + Cen1 + W1 + 30, CurY + 10, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PO Date", LMargin + Cen1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + Cen1 + W1 + 30, CurY + 10, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + Cen1 - 20, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
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
        'Dim BInc As Integer
        'Dim BnkDetAr() As String
        'Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim Narr1 As String = "", Narr2 As String = "", Narr3 As String = ""
        Dim vattax As Single = 0
        Dim addtax As Single = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))

            'MessageBox.Show("CurY1 = " & CurY)

            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))


            'If is_LastPage = True Then
            '    Erase BnkDetAr
            '    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            '        BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

            '        BInc = -1
            '        Yax = CurY

            '        Yax = Yax + TxtHgt - 10
            '        'If Val(prn_PageNo) = 1 Then
            '        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
            '        Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
            '        'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
            '        'End If

            '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '        BInc = BInc + 1
            '        If UBound(BnkDetAr) >= BInc Then
            '            Yax = Yax + TxtHgt
            '            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
            '        End If

            '        BInc = BInc + 1
            '        If UBound(BnkDetAr) >= BInc Then
            '            Yax = Yax + TxtHgt - 3
            '            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
            '        End If

            '        BInc = BInc + 1
            '        If UBound(BnkDetAr) >= BInc Then
            '            Yax = Yax + TxtHgt - 3
            '            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
            '        End If

            '        BInc = BInc + 1
            '        If UBound(BnkDetAr) >= BInc Then
            '            Yax = Yax + TxtHgt - 3
            '            Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
            '        End If

            '    End If

            'End If


            Narr1 = "" : Narr2 = "" : Narr3 = ""

            If Trim(prn_HdDt.Rows(0).Item("Narration").ToString) <> "" Then
                Narr1 = "NOTE : " & Trim(prn_HdDt.Rows(0).Item("Narration").ToString)
            End If

            If Len(Narr1) > 38 Then
                For I = 38 To 1 Step -1
                    If Mid$(Trim(Narr1), I, 1) = " " Or Mid$(Trim(Narr1), I, 1) = "," Or Mid$(Trim(Narr1), I, 1) = "." Or Mid$(Trim(Narr1), I, 1) = "-" Or Mid$(Trim(Narr1), I, 1) = "/" Or Mid$(Trim(Narr1), I, 1) = "_" Or Mid$(Trim(Narr1), I, 1) = "(" Or Mid$(Trim(Narr1), I, 1) = ")" Or Mid$(Trim(Narr1), I, 1) = "\" Or Mid$(Trim(Narr1), I, 1) = "[" Or Mid$(Trim(Narr1), I, 1) = "]" Or Mid$(Trim(Narr1), I, 1) = "{" Or Mid$(Trim(Narr1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 38
                Narr2 = Microsoft.VisualBasic.Right(Trim(Narr1), Len(Narr1) - I)
                Narr1 = Microsoft.VisualBasic.Left(Trim(Narr1), I - 1)
            End If

            If Len(Narr2) > 38 Then
                For I = 38 To 1 Step -1
                    If Mid$(Trim(Narr2), I, 1) = " " Or Mid$(Trim(Narr2), I, 1) = "," Or Mid$(Trim(Narr2), I, 1) = "." Or Mid$(Trim(Narr2), I, 1) = "-" Or Mid$(Trim(Narr2), I, 1) = "/" Or Mid$(Trim(Narr2), I, 1) = "_" Or Mid$(Trim(Narr2), I, 1) = "(" Or Mid$(Trim(Narr2), I, 1) = ")" Or Mid$(Trim(Narr2), I, 1) = "\" Or Mid$(Trim(Narr2), I, 1) = "[" Or Mid$(Trim(Narr2), I, 1) = "]" Or Mid$(Trim(Narr2), I, 1) = "{" Or Mid$(Trim(Narr2), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 38
                Narr3 = Microsoft.VisualBasic.Right(Trim(Narr2), Len(Narr2) - I)
                Narr2 = Microsoft.VisualBasic.Left(Trim(Narr2), I - 1)
            End If

            CurY = CurY - 10
            If Trim(Narr1) <> "" Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Narr1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If Trim(Narr2) <> "" Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Narr2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            If Trim(Narr3) <> "" Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Narr3), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If lbl_Company.Text = "SAARA CREATIONS AHMEDABAD" Then
                        vattax = 0
                        addtax = 0

                        vattax = Val(lbl_Assessable.Text) * 4 / 100
                        addtax = Val(lbl_Assessable.Text) * 1 / 100
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString) & "@ " & "4" & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vattax), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        CurY = CurY + TxtHgt + 5
                        Common_Procedures.Print_To_PrintDocument(e, "ADD.TAX" & "@ " & "1" & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(addtax), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)


                    Else

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString) & "@ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                    End If
                End If
            End If
            If Chk_BranchTransfer.Checked = True Or Trim(UCase(cbo_TaxType.Text)) = "CST" And Val(txt_TaxPerc.Text) = 2 Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "NOTE:", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 5
            If Trim(UCase(cbo_TaxType.Text)) = "CST" And Val(txt_TaxPerc.Text) = 2 Then
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "PLEASE PROVIDE FORM 'C'", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Branch_Transfer_Status").ToString) = 1 Then
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "GOODS ARE BRANCH", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt + 5
            If Trim(UCase(cbo_TaxType.Text)) = "CST" And Val(txt_TaxPerc.Text) = 2 Then
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "IMMEDIATELY", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Branch_Transfer_Status").ToString) = 1 Then
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "TRANSFFERED AGAINST FORM 'F'", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
                End If
            End If
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

            CurY = CurY + TxtHgt + 5
            If Trim(UCase(prn_DetDt.Rows(0).Item("Unit_Name").ToString)) <> "ROLL" Then
                If Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString) <> 0 Then
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "Total Rolls", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString) & " Rolls", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                    End If
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            If Trim(UCase(prn_DetDt.Rows(0).Item("Unit_Name").ToString)) = "KG" Then
                If Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString) <> 0 Then
                    If is_LastPage = True Then
                        If prn_HdDt.Rows(0).Item("NoOf_Bundle").ToString <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, "No.Of Bundle : " & prn_HdDt.Rows(0).Item("NoOf_Bundle").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "Total Quantity", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "########0.000") & " " & prn_DetDt.Rows(0).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                    End If
                End If
            ElseIf Trim(UCase(prn_DetDt.Rows(0).Item("Unit_Name").ToString)) = "METER" Then
                If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    If is_LastPage = True Then
                        If prn_HdDt.Rows(0).Item("NoOf_Bundle").ToString <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, "No.Of Bundle : " & prn_HdDt.Rows(0).Item("NoOf_Bundle").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                        End If
                        Common_Procedures.Print_To_PrintDocument(e, "Total Meters", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "########0.00") & " " & prn_DetDt.Rows(0).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                    End If
                End If
            ElseIf Trim(UCase(prn_DetDt.Rows(0).Item("Unit_Name").ToString)) = "ROLL" Then
                If Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString) <> 0 Then
                    If is_LastPage = True Then
                        If prn_HdDt.Rows(0).Item("NoOf_Bundle").ToString <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, "No.Of Bundle : " & prn_HdDt.Rows(0).Item("NoOf_Bundle").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                        End If

                        Common_Procedures.Print_To_PrintDocument(e, "Total Rolls", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Rolls").ToString), "########0") & " " & prn_DetDt.Rows(0).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                    End If
                End If
            End If

            '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("NoOf_Bundle").ToString, LMargin + Cen1 + w1 + 30, CurY + 20, 0, 0, p1Font)

            'If Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "Total Quantity", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), "########0.000") & " " & prn_DetDt.Rows(0).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
            '    End If
            'End If

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                ' p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Rs. " & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(6), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(6), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))

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
            Common_Procedures.Print_To_PrintDocument(e, "Rupees(INR) : " & Rup1, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            CurY = CurY + TxtHgt - 10


            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)


            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We declare that this invoice shows the actual price of", LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "the goods described and that all particulars are true and correct.", LMargin + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(7), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            Jurs = Common_Procedures.settings.Jurisdiction
            If Trim(Jurs) = "" Then Jurs = "Tirupur"

            Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Jurs & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)

            'MessageBox.Show("CurY_LAST = " & CurY)

            If Print_PDF_Status = True Then
                CurY = CurY + TxtHgt - 15
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '  p1Font = New Font("Calibri", 9, FontStyle.Regular)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_OrderDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_OrderDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_OrderDate.Text = Date.Today
            txt_OrderDate.SelectAll()
        End If
    End Sub

    Private Sub txt_DcDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DcDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_DcDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DcDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_DcDate.Text = Date.Today
            txt_DcDate.SelectAll()
        End If
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

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_CashDiscPerc, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_TaxPerc, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            If Val(txt_TaxPerc.Text) = 0 And Trim(UCase(cbo_TaxAc.Text)) = "VAT" Then
                txt_TaxPerc.Text = 5
            End If
        End If


    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        If Trim(UCase(cbo_TaxType.Text)) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then txt_TaxPerc.Text = ""
    End Sub

    Private Sub cbo_OnAcc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OnAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_OnAcc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OnAccount, txt_DcDate, chk_AgainstCForm, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_OnAcc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OnAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OnAccount, chk_AgainstCForm, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_OnAcc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            'Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_OnAccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_Rolls_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rolls.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rolls_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rolls.TextChanged
        Weight_Calculation()
    End Sub

    Private Sub txt_WgtRolls_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WgtRolls.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WgtRolls_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WgtRolls.TextChanged
        Weight_Calculation()
    End Sub

    Private Sub chk_AgainstCForm_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_AgainstCForm.CheckedChanged
        If chk_AgainstCForm.Checked = True Then
            cbo_TaxType.Text = "CST"
            txt_TaxPerc.Text = 2
        Else
            cbo_TaxType.Text = "VAT"
            txt_TaxPerc.Text = 5
        End If
    End Sub

    Private Sub cbo_Remarks_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Remarks.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sales_Details", "Serial_No", "", "")

    End Sub

    Private Sub cbo_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Remarks.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Remarks, txt_Rate, btn_Add, "Sales_Details", "Serial_No", "", "")

    End Sub

    Private Sub cbo_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Remarks.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Remarks, btn_Add, "Sales_Details", "Serial_No", "", "", False)

    End Sub

    Private Sub cbo_colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub

    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_colour.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_colour, cbo_ItemName, txt_Rolls, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_colour, txt_Rolls, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

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

    Private Sub cbo_UnitName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UnitName.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0
        Dim LedCondt As String = ""

        If New_Entry = False Then
            If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then
                MessageBox.Show("Invalid Entry Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
                Exit Sub
            End If
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        LedCondt = ""
        If LedIdNo <> 0 Then
            LedCondt = "(a.Ledger_Idno = " & Str(Val(LedIdNo)) & ")"
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name from Delivery_Head a INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_Idno where " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " a.Invoice_Code = '" & Trim(NewCode) & "'  order by a.Delivery_Date, a.for_orderby, a.Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Total_Rolls").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Qty").ToString
                    .Rows(n).Cells(7).Value = "1"
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Delivery_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name from Delivery_Head a INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_Idno where " & LedCondt & IIf(Trim(LedCondt) <> "", " and ", "") & " a.Invoice_Code = ''  order by a.Delivery_Date, a.for_orderby, a.Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Total_Rolls").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Total_Meters").ToString
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Total_Qty").ToString
                    .Rows(n).Cells(7).Value = ""
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Delivery_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()
        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_PavuReceipt(e.RowIndex)
    End Sub

    Private Sub Select_PavuReceipt(ByVal RwIndx As Integer)

        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(7).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_PavuReceipt(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Delivery_Selection()
    End Sub

    Private Sub Close_Delivery_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Set_No As String = ""

        If New_Entry = False Then
            If Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
                MessageBox.Show("Invalid Entry Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
                Exit Sub
            End If
        End If

        ' dgv_PavuDetails.Rows.Clear()

        pnl_Back.Enabled = True

        ' MovSTS = True
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(7).Value) = 1 Then

                Da1 = New SqlClient.SqlDataAdapter("select a.* from Delivery_Details a where a.Delivery_Code  = '" & Trim(dgv_Selection.Rows(i).Cells(8).Value) & "' order by a.sl_no", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)

                '  cbo_Type.Text = "RECEIPT"
                txt_DcNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                txt_DcDate.Text = dgv_Selection.Rows(i).Cells(2).Value
                cbo_Ledger.Text = dgv_Selection.Rows(i).Cells(3).Value
                lbl_DeliveryCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                dgv_Details.Rows.Clear()

                If Dt1.Rows.Count > 0 Then

                    For j = 0 To Dt1.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        sno = sno + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                        dgv_Details.Rows(n).Cells(1).Value = Common_Procedures.Item_IdNoToName(con, (Dt1.Rows(j).Item("Item_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(2).Value = Common_Procedures.Colour_IdNoToName(con, (Dt1.Rows(j).Item("Colour_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(j).Item("Rolls").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(j).Item("Weight_Rolls").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Common_Procedures.Unit_IdNoToName(con, (Dt1.Rows(j).Item("Unit_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(j).Item("Quantity").ToString), "########0.000")
                        'dgv_Details.Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(j).Item("Rate").ToString), "########0.00")
                        'dgv_Details.Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(j).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = Dt1.Rows(j).Item("Remarks").ToString

                    Next

                End If
                Dt1.Clear()

            End If
            Dt1.Clear()

        Next

        TotalAmount_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        '  If txt_SetNo.Enabled Then
        'txt_SetNo.Focus()
        '  Else
        '  txt_TotalBeams.Focus()
        '  End If
        'If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
        '    If dgv_PavuDetails.Rows.Count > 0 Then
        '        btn_save.Focus()
        '    Else
        '        dtp_Date.Focus()
        '    End If
        'End If

    End Sub

    Private Sub txt_NoofItems_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofItems.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim sTATE As Integer = 0
        Dim LDG_Id As Integer = 0
        'Dim SNo As Integer

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.* FROM Ledger_Head A LEFT OUTER JOIN sTATE_Head B ON a.STATE_IdNo = B.STATE_IdNo where a.lEDGER_NAME = '" & Trim(cbo_Ledger.Text) & "'", con)
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            sTATE = dt1.Rows(0).Item("State_Idno").ToString
            LDG_ID = dt1.Rows(0).Item("lEDGER_iDNO").ToString
        End If
        dt1.Dispose()
        da1.Dispose()

        'If LDG_Id = 104 Then
        '    Chk_BranchTransfer.Checked = True
        '    cbo_TaxType.Enabled = False
        '    txt_TaxPerc.Enabled = False
        'Else
        '    Chk_BranchTransfer.Checked = False
        '    cbo_TaxType.Enabled = True
        '    txt_TaxPerc.Enabled = True
        'End If

        If sTATE = 1 Then

            cbo_TaxType.Text = "VAT"
            txt_TaxPerc.Text = 5
            cbo_TaxType.Enabled = False
            txt_TaxPerc.Enabled = False

        ElseIf sTATE = 8 And LDG_Id = 104 Then

            Chk_BranchTransfer.Checked = True
            cbo_TaxType.Text = ""
            cbo_TaxType.Enabled = False
            txt_TaxPerc.Enabled = False
            txt_TaxPerc.Enabled = False

        ElseIf sTATE = 0 Then

            Chk_BranchTransfer.Checked = False
            cbo_TaxType.Text = ""
            txt_TaxPerc.Text = 0
            cbo_TaxType.Enabled = True
            txt_TaxPerc.Enabled = True
            txt_TaxPerc.Enabled = True

        Else

            Chk_BranchTransfer.Checked = False
            cbo_TaxType.Text = ""
            txt_TaxPerc.Text = 0
            cbo_TaxType.Text = "CST"
            cbo_TaxType.Enabled = True
            txt_TaxPerc.Enabled = True

        End If
    End Sub
End Class