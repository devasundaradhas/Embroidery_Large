Public Class Spinning_Invoice_Entry_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GSPIN-"
    Private cmbItmNm As String
    Private prn_HdDt_Vat As New DataTable
    Private prn_DetDt_Vat As New DataTable
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private CFrm_STS As Integer
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_DetAr(200, 10) As String

    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_GSTTax_Details.Visible = False

        chk_No_CForm.Checked = False

        lbl_InvoiceNo.Text = ""
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        cbo_SalesAc.Text = ""
        cbo_VatAc.Text = ""

        txt_BagNos.Text = ""

        txt_VehicleNo.Text = ""
        txt_DelvAdd1.Text = ""
        txt_DelvAdd2.Text = ""
        txt_DelvAdd3.Text = ""

        dtp_RemovalDate.Text = ""
        dtp_RemovalTime.Text = ""

        txt_CashDiscPerc.Text = ""
        txt_CashDiscAmount.Text = ""

        txt_Assessable.Text = ""

        txt_VatPerc.Text = ""
        txt_VatAmount.Text = ""

        txt_Freight.Text = ""

        txt_AddLess.Text = ""
        txt_RoundOff.Text = ""
        txt_NetAmount.Text = ""

        cbo_ItemName.Text = ""
        cbo_ItemDescription.Text = ""
        txt_Bags.Text = ""
        txt_Weight_Bag.Text = ""
        txt_Weight.Text = ""
        txt_Rate_5Kg.Text = ""
        txt_Rate_Kg.Text = ""
        txt_Amount.Text = ""

        txt_Electronic_RefNo.Text = ""
        txt_DateTime_Of_Supply.Text = ""
        cbo_TransportMode.Text = ""
        txt_Hsn_Code.Text = ""
        txt_Gst_Perc.Text = ""
        lbl_CGstAmount.Text = ""
        lbl_IGstAmount.Text = ""
        lbl_SGstAmount.Text = ""

        lbl_AmountInWords.Text = "Amount In Words :                                "
        txt_LrNo.Text = ""
        dtp_LrDate.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, 22)
        cbo_VatAc.Text = Common_Procedures.Ledger_IdNoToName(con, 20)

        dtp_RemovalDate.Text = Date.Today.ToShortDateString
        dtp_RemovalTime.Text = TimeOfDay.ToShortTimeString

        dgv_GSTTax_Details.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Clear()
        dgv_GSTTax_Details_Total.Rows.Add()
        cbo_TaxType.Text = "GST"
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
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


        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If
    End Sub
    'Private Sub Grid_Cell_DeSelect()
    '    On Error Resume Next
    '    If FrmLdSTS = True Then Exit Sub
    '    dgv_GSTTax_Details.CurrentCell.Selected = False
    '    dgv_GSTTax_Details_Total.CurrentCell.Selected = False
    'End Sub
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Ledger_Name as SalesAcName, d.Ledger_Name as TaxAcName from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.SalesAc_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.TaxAc_IdNo = d.Ledger_IdNo where a.Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                cbo_SalesAc.Text = dt1.Rows(0).Item("SalesAcName").ToString
                cbo_VatAc.Text = dt1.Rows(0).Item("TaxAcName").ToString

                txt_BagNos.Text = dt1.Rows(0).Item("Bag_Nos").ToString

                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_DelvAdd1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_DelvAdd2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString
                txt_DelvAdd3.Text = dt1.Rows(0).Item("Delivery_Address3").ToString

                dtp_RemovalDate.Text = dt1.Rows(0).Item("Removal_Date").ToString
                dtp_RemovalTime.Text = dt1.Rows(0).Item("Removal_Time").ToString

                If Val(dt1.Rows(0).Item("Against_CForm_Status").ToString) = 1 Then chk_No_CForm.Checked = True


                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")

                txt_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")

                txt_VatPerc.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc").ToString), "########0.00")
                txt_VatAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")

                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")

                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                txt_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                'cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_Electronic_RefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                cbo_TransportMode.Text = dt1.Rows(0).Item("Transportation_Mode").ToString
                txt_DateTime_Of_Supply.Text = dt1.Rows(0).Item("Date_Time_Of_Supply").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Entry_GST_Tax_Type").ToString
                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")
                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGst_Amount").ToString), "########0.00")

                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                dtp_LrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.ItemGroup_Name  as DesCription , D.Colour_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN ItemGroup_Head c on a.ItemGroup_IdNo = c.ItemGroup_IdNo LEFT OUTER JOIN Colour_Head d on a.Colour_idno = d.Colour_idno Where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then

                    cbo_ItemName.Text = dt2.Rows(0).Item("Item_Name").ToString
                    cbo_ItemDescription.Text = dt2.Rows(0).Item("DesCription").ToString
                    cbo_Colour.Text = dt2.Rows(0).Item("Colour_Name").ToString

                    txt_Bags.Text = Val(dt2.Rows(0).Item("Bags").ToString)
                    txt_Weight_Bag.Text = Format(Val(dt2.Rows(0).Item("Weight_Bag").ToString), "########0.000")
                    txt_Weight.Text = Format(Val(dt2.Rows(0).Item("Weight").ToString), "########0.000")
                    txt_Rate_5Kg.Text = Format(Val(dt2.Rows(0).Item("Rate_5Kg").ToString), "########0.00")
                    txt_Rate_Kg.Text = Format(Val(dt2.Rows(0).Item("Rate").ToString), "########0.00")
                    txt_Amount.Text = Format(Val(dt2.Rows(0).Item("Amount").ToString), "########0.00")
                    txt_Hsn_Code.Text = (dt2.Rows(0).Item("HSN_Code").ToString)
                    txt_Gst_Perc.Text = Format(Val(dt2.Rows(0).Item("Tax_Perc").ToString), "########0.00")

                End If

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()
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
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN MOVING RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Spinning_Invoice_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VatAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VatAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemDescription.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemDescription.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
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

    Private Sub Spinning_Invoice_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 12 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_VatAc.DataSource = dt3
        cbo_VatAc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 28 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_SalesAc.DataSource = dt4
        cbo_SalesAc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select ItemGroup_Name from ItemGroup_Head order by ItemGroup_Name", con)
        da.Fill(dt5)
        cbo_ItemDescription.DataSource = dt5
        cbo_ItemDescription.DisplayMember = "ItemGroup_Name"

        da.Dispose()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("NO TAX")
        cbo_TaxType.Items.Add("GST")

        cbo_TransportMode.Items.Clear()

        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Bag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate_5Kg.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate_Kg.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemDescription.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BagNos.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VatAc.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_RemovalDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_RemovalTime.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Assessable.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Electronic_RefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateTime_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Hsn_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Gst_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_LrDate.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Bag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate_5Kg.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate_Kg.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemDescription.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BagNos.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VatAc.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_RemovalDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_RemovalTime.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Assessable.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Electronic_RefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateTime_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Hsn_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Gst_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_LrDate.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Electronic_RefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DateTime_Of_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Hsn_Code.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_LrDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Electronic_RefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateTime_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Hsn_Code.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Gst_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_LrDate.KeyPress, AddressOf TextBoxControlKeyPress
        clear()

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Spinning_Invoice_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub Spinning_Invoice_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_GSTTax_Details.Visible = True Then
                    btn_Close_GSTTax_Details_Click(sender, e)
                    Exit Sub
                End If

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

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
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
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and   Sales_Code  like '" & Trim(Pk_Condition) & "%' and  Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No"
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

            da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and   Sales_Code  like '" & Trim(Pk_Condition) & "%' and  Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No", con)
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
            cmd.CommandText = "select Sales_No from Sales_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and   Sales_Code  like '" & Trim(Pk_Condition) & "%' and  Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc"

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
        Dim da As New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Sales_Code  like '" & Trim(Pk_Condition) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc", con)
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
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code  like '" & Trim(Pk_Condition) & "%' and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_InvoiceNo.Text = NewID
            lbl_InvoiceNo.ForeColor = Color.Red


            da = New SqlClient.SqlDataAdapter("select top 1 a.Sales_Code, a.Tax_Perc, a.Entry_GST_Tax_Type,a.Against_CForm_Status, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Sales_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Entry_GST_Tax_Type").ToString
                txt_VatPerc.Text = Val(dt2.Rows(0).Item("Tax_Perc").ToString)
                cbo_SalesAc.Text = dt2.Rows(0).Item("SalesAcName").ToString
                If dt2.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_VatAc.Text = dt2.Rows(0).Item("TaxAcName").ToString
                '    If dt2.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Tax_Type").ToString
                If dt2.Rows(0).Item("Against_CForm_Status").ToString <> "" Then
                    If Val(dt2.Rows(0).Item("Against_CForm_Status").ToString) = 1 Then chk_No_CForm.Checked = True
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.Weight_Bag, c.ItemGroup_Name from Sales_Details a INNER JOIN ItemGroup_Head c on a.ItemGroup_IdNo = c.ItemGroup_IdNo Where a.Sales_Code = '" & Trim(dt2.Rows(0).Item("Sales_Code").ToString) & "' Order by a.sl_no", con)
                da2.Fill(dt3)

                If dt3.Rows.Count > 0 Then

                    txt_Weight_Bag.Text = Format(Val(dt3.Rows(0).Item("Weight_Bag").ToString), "########0.000")
                    cbo_ItemDescription.Text = dt3.Rows(0).Item("ItemGroup_Name").ToString

                End If

                dt3.Clear()
                dt3.Dispose()
                da2.Dispose()

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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
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
        Dim saleac_id As Integer = 0, txac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim itmdesc_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim Amt As Single = 0
        Dim Col_id As Integer = 0

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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


        saleac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        If saleac_id = 0 And Val(txt_NetAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled Then cbo_SalesAc.Focus()
            Exit Sub
        End If

        txac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_VatAc.Text)
        If txac_id = 0 And Val(txt_VatAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Vat A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_VatAc.Enabled Then cbo_VatAc.Focus()
            Exit Sub
        End If

        itm_id = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
        If itm_id = 0 And (Val(txt_Bags.Text) <> 0 Or Val(txt_Weight.Text) <> 0) Then
            MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        If itm_id <> 0 And Val(txt_Weight.Text) = 0 Then
            MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Weight.Enabled Then txt_Weight.Focus()
            Exit Sub
        End If

        itmdesc_id = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemDescription.Text)
        If itmdesc_id = 0 And Val(txt_Weight.Text) <> 0 Then
            MessageBox.Show("Invalid Item description", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemDescription.Enabled Then cbo_ItemDescription.Focus()
            Exit Sub
        End If


        CFrm_STS = 0
        If chk_No_CForm.Checked = True Then CFrm_STS = 1


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Sales_Code like '" & Trim(Pk_Condition) & "%' and  Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)

            If New_Entry = True Then



                cmd.CommandText = "Insert into Sales_Head(Entry_VAT_GST_Type ,Sales_Code ,              Company_IdNo        ,           Sales_No                ,                               for_OrderBy                                  , Sales_Date, Payment_Method,           Ledger_IdNo   ,          SalesAc_IdNo      ,              TaxAc_IdNo    , Narration,          Delivery_Address1       ,           Delivery_Address2      ,           Delivery_Address3      ,           Vehicle_No              ,           Removal_Date              ,            Removal_Time             ,           Bag_Nos              ,            Total_Qty            ,              Total_Bags         ,             Total_Weight         ,             SubTotal_Amount      , Total_DiscountAmount,             Total_TaxAmount         ,             Gross_Amount         ,             CashDiscount_Perc          ,             CashDiscount_Amount          ,             Assessable_Value         ,             Tax_Perc              ,             Tax_Amount              ,             Freight_Amount        ,             AddLess_Amount                            ,             Round_Off              ,             Net_Amount            ,Tax_Type     ,Against_CForm_Status   ,           Electronic_Reference_No   ,               Transportation_Mode     ,               Date_Time_Of_Supply                       ,              Entry_GST_Tax_Type ,                 CGst_Amount          ,                 SGst_Amount          ,               IGst_Amount             ,Lr_No                         ,Lr_Date) " & _
                                    " Values (                'GST'           ,        '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, 'CREDIT'      , " & Str(Val(led_id)) & ", " & Str(Val(saleac_id)) & ",  " & Str(Val(txac_id)) & ", ''       , '" & Trim(txt_DelvAdd1.Text) & "', '" & Trim(txt_DelvAdd2.Text) & "', '" & Trim(txt_DelvAdd3.Text) & "', '" & Trim(txt_VehicleNo.Text) & "', '" & Trim(dtp_RemovalDate.Text) & "', '" & Trim(dtp_RemovalTime.Text) & "', '" & Trim(txt_BagNos.Text) & "', " & Str(Val(txt_Weight.Text)) & ", " & Str(Val(txt_Bags.Text)) & ", " & Str(Val(txt_Weight.Text)) & ", " & Str(Val(txt_Amount.Text)) & ",       0             , " & Str(Val(txt_VatAmount.Text)) & ", " & Str(Val(txt_Amount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_Assessable.Text)) & ", " & Str(Val(txt_VatPerc.Text)) & ", " & Str(Val(txt_VatAmount.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(CSng(txt_NetAmount.Text))) & " , 'VAT',   " & Str(Val(CFrm_STS)) & ", '" & Trim(txt_Electronic_RefNo.Text) & "', '" & Trim(cbo_TransportMode.Text) & "', '" & Trim(txt_DateTime_Of_Supply.Text) & "', '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & " ,'" & Trim(txt_LrNo.Text) & "' ,'" & Trim(dtp_LrDate.Text) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Sales_Head set Entry_VAT_GST_Type = 'GST' , Sales_Date = @SalesDate, Payment_Method = 'CREDIT', Ledger_IdNo = " & Str(Val(led_id)) & ", SalesAc_IdNo = " & Str(Val(saleac_id)) & ",  TaxAc_IdNo = " & Str(Val(txac_id)) & ", Delivery_Address1 = '" & Trim(txt_DelvAdd1.Text) & "', Delivery_Address2 = '" & Trim(txt_DelvAdd2.Text) & "', Delivery_Address3 = '" & Trim(txt_DelvAdd3.Text) & "', Vehicle_No = '" & Trim(txt_VehicleNo.Text) & "', Removal_Date = '" & Trim(dtp_RemovalDate.Text) & "', Removal_Time = '" & Trim(dtp_RemovalTime.Text) & "', Bag_Nos = '" & Trim(txt_BagNos.Text) & "', Narration = '', " & _
                                    " Total_Qty = " & Str(Val(txt_Weight.Text)) & ",  Tax_Type = 'VAT', Against_CForm_Status =   " & Str(Val(CFrm_STS)) & "  , Total_Bags = " & Str(Val(txt_Bags.Text)) & ", Total_Weight = " & Str(Val(txt_Weight.Text)) & ", SubTotal_Amount = " & Str(Val(txt_Amount.Text)) & ", Total_DiscountAmount = 0, Total_TaxAmount = " & Str(Val(txt_VatAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_Amount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(txt_Assessable.Text)) & ", Tax_Perc = " & Str(Val(txt_VatPerc.Text)) & ", Tax_Amount = " & Str(Val(txt_VatAmount.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(txt_NetAmount.Text))) & " ,  Electronic_Reference_No = '" & Trim(txt_Electronic_RefNo.Text) & "' ,  Transportation_Mode = '" & Trim(cbo_TransportMode.Text) & "'  ,  Date_Time_Of_Supply = '" & Trim(txt_DateTime_Of_Supply.Text) & "'  , Entry_GST_Tax_Type = '" & Trim(cbo_TaxType.Text) & "',  CGst_Amount = " & Str(Val(lbl_CGstAmount.Text)) & " , SGst_Amount = " & Str(Val(lbl_SGstAmount.Text)) & " , IGst_Amount = " & Str(Val(lbl_IGstAmount.Text)) & " ,Lr_No = '" & Trim(txt_LrNo.Text) & "' ,Lr_Date = '" & Trim(dtp_LrDate.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            unt_id = 0

            If itm_id <> 0 And Val(txt_Weight.Text) <> 0 Then

                da = New SqlClient.SqlDataAdapter("select unit_idno from item_head where item_idno = " & Str(Val(itm_id)), con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt5)


                unt_id = 0
                If dt5.Rows.Count > 0 Then
                    If IsDBNull(dt5.Rows(0)(0).ToString) = False Then
                        unt_id = Val(dt5.Rows(0)(0).ToString)
                    End If
                End If

                dt5.Clear()

                Col_id = Common_Procedures.Colour_NameToIdNo(con, Trim(cbo_Colour.Text), tr)

                Sno = Sno + 1

                cmd.CommandText = "Insert into Sales_Details(Sales_Code,              Company_IdNo        ,           Sales_No                ,                               for_OrderBy                                  , Sales_Date,           Ledger_IdNo   ,             SL_No    ,            Item_IdNo    ,           ItemGroup_IdNo    ,           Unit_IdNo     ,             Noof_Items           ,             Bags               ,             Weight_Bag               ,             Weight               ,              Rate_5Kg              ,             Rate                  ,  Tax_Rate,             Amount               , Discount_Perc, Discount_Amount,  Tax_Amount,             Total_Amount         ,          Bag_Nos            ,Colour_Idno               ,                     HSN_Code  ,                      Tax_Perc             ) " & _
                                    " Values (  '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(itmdesc_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(txt_Weight.Text)) & ", " & Str(Val(txt_Bags.Text)) & ", " & Str(Val(txt_Weight_Bag.Text)) & ", " & Str(Val(txt_Weight.Text)) & ", " & Str(Val(txt_Rate_5Kg.Text)) & ", " & Str(Val(txt_Rate_Kg.Text)) & ",    0     , " & Str(Val(txt_Amount.Text)) & ",     0        ,          0     ,     0    , " & Str(Val(txt_Amount.Text)) & ", '" & Trim(txt_BagNos.Text) & "'," & Val(Col_id) & " , '" & Trim(txt_Hsn_Code.Text) & "'," & Val(txt_Gst_Perc.Text) & " )"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into Item_Processing_Details (  Reference_Code  ,             Company_IdNo         ,          Reference_No             ,                               for_OrderBy                                  , Reference_Date,          Ledger_IdNo    ,           Party_Bill_No           ,            SL_No     ,           Item_IdNo    ,            Unit_IdNo     ,         Quantity                       ) " & _
                                    " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",    @SalesDate , " & Str(Val(led_id)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(-1 * Val(txt_Weight.Text)) & " )"
                cmd.ExecuteNonQuery()

            End If


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


            AcPos_ID = led_id


            Dim vNetAmt As String = Format(Val(CSng(txt_NetAmount.Text)), "#############0.00")

            '---GST
            vVouPos_IdNos = AcPos_ID & "|" & saleac_id & "|" & txac_id & "|" & "25|26|27|9|17|24"

            vVouPos_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(txt_VatAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(txt_RoundOff.Text)) & "|" & Val(txt_VatAmount.Text) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(txt_Freight.Text) & "|" & Val(txt_AddLess.Text) & "|" & Val(txt_RoundOff.Text)

            If Common_Procedures.Voucher_Updation(con, "Gst.Sales", Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Bill No . : " & Trim(lbl_InvoiceNo.Text), vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'Ac_id = 0
            ''If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" Then
            ''    Ac_id = 1
            ''Else
            'Ac_id = led_id
            ''End If

            'cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " & _
            '                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SPInvoice', @SalesDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(txt_NetAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SPInvoice', @SalesDate, 1, " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(txt_NetAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'Amt = Val(txt_NetAmount.Text) - Val(txt_VatAmount.Text)

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SPInvoice', @SalesDate, 2, " & Str(Val(saleac_id)) & ", " & Str(Val(Amt)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'If Val(txt_VatAmount.Text) <> 0 Then
            '    cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'SPInvoice', @SalesDate, 3, " & Str(Val(txac_id)) & ", " & Str(Val(txt_VatAmount.Text)) & ", 'Bill No . : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            '    cmd.ExecuteNonQuery()
            'End If

            tr.Commit()

            If New_Entry = True Then
                move_record(lbl_InvoiceNo.Text)
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
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "item_head", "item_Name", "", "(item_idno = 0)")
            cbo_ItemName.Tag = cbo_ItemName.Text
        End With

    End Sub

    Private Sub txt_Bags_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Bags.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Bags_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Bags.TextChanged
        txt_Weight.Text = Format(Val(txt_Bags.Text) * Val(txt_Weight_Bag.Text), "#########0.000")
    End Sub

    Private Sub txt_Rate_5Kg_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate_5Kg.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_5Kg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate_5Kg.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Rate_5Kg_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate_5Kg.LostFocus
        txt_Rate_5Kg.Text = Format(Val(txt_Rate_5Kg.Text), "########0.00")
    End Sub

    Private Sub txt_Rate_5Kg_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate_5Kg.TextChanged
        txt_Rate_Kg.Text = Format(Val(txt_Rate_5Kg.Text) / 5, "#########0.00")
        Amount_Calculation()
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, cbo_TaxType, txt_Bags, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, txt_Bags, "item_head", "item_Name", "", "(item_idno = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
                get_Item_Unit_Rate_TaxPerc()
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
        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            get_Item_Unit_Rate_TaxPerc()
        End If
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Ledger.Focus()
        End If
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")


        cbo_Ledger.Tag = cbo_Ledger.Text
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Amount_Calculation()
        End If
    End Sub


    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                    cbo_Ledger.Tag = cbo_Ledger.Text
                    Amount_Calculation()
                End If
            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub



    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_Ledger, txt_Electronic_RefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
            'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_PaymentMethod, IIf(cbo_OrderNo.Visible = True, cbo_OrderNo, txt_OrderNo), "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, txt_Electronic_RefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub


    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub txt_Assessable_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Assessable.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Assessable_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Assessable.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Assessable.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Weight_Bag_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight_Bag.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Weight_Bag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Bag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Weight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight.TextChanged
        NetAmount_Calculation()
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
                Condt = "a.Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.Item_IdNo = " & Str(Val(Itm_IdNo))
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Code IN (select z.Sales_Code from Sales_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.Sales_No, a.Sales_Date, c.Bags, c.Weight, a.Net_Amount, b.Ledger_Name, d.item_name from Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Sales_Details c on a.Sales_code = c.Sales_code and a.company_IdNo = c.company_IdNo Left Outer Join Item_Head d on c.item_idno = d.item_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
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


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        With cbo_Filter_PartyName
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            With cbo_Filter_PartyName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    dtp_Filter_ToDate.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_ItemName.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        With cbo_Filter_PartyName

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Text) <> "" Then
                    If .DroppedDown = True Then
                        If Trim(.SelectedText) <> "" Then
                            .Text = .SelectedText
                        Else
                            If .Items.Count > 0 Then
                                .SelectedIndex = 0
                                .SelectedItem = .Items(0)
                                .Text = .GetItemText(.SelectedItem)
                            End If
                        End If
                    End If
                End If

                cbo_Filter_ItemName.Focus()

            Else

                Condt = ""
                FindStr = ""

                If Asc(e.KeyChar) = 8 Then
                    If .SelectionStart <= 1 Then
                        .Text = ""
                    End If

                    If Trim(.Text) <> "" Then
                        If .SelectionLength = 0 Then
                            FindStr = .Text.Substring(0, .Text.Length - 1)
                        Else
                            FindStr = .Text.Substring(0, .SelectionStart - 1)
                        End If
                    End If

                Else
                    If .SelectionLength = 0 Then
                        FindStr = .Text & e.KeyChar
                    Else
                        FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                    End If

                End If

                Condt = " (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) "
                If Trim(FindStr) <> "" Then
                    Condt = "(b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and (a.Ledger_DisplayName like '" & Trim(FindStr) & "%' or a.Ledger_DisplayName like '% " & Trim(FindStr) & "%')"
                End If

                da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "Ledger_DisplayName"

                .Text = Trim(FindStr)

                .SelectionStart = FindStr.Length

                e.Handled = True

            End If

        End With

    End Sub

    Private Sub cbo_Filter_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemName.GotFocus
        With cbo_Filter_ItemName
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Try
            With cbo_Filter_ItemName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Filter_PartyName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    btn_Filter_Show.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        With cbo_Filter_ItemName

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Text) <> "" Then
                    If .DroppedDown = True Then
                        If Trim(.SelectedText) <> "" Then
                            .Text = .SelectedText
                        Else
                            If .Items.Count > 0 Then
                                .SelectedIndex = 0
                                .SelectedItem = .Items(0)
                                .Text = .GetItemText(.SelectedItem)
                            End If
                        End If
                    End If
                End If

                btn_Filter_Show.Focus()

            Else

                Condt = ""
                FindStr = ""

                If Asc(e.KeyChar) = 8 Then
                    If .SelectionStart <= 1 Then
                        .Text = ""
                    End If

                    If Trim(.Text) <> "" Then
                        If .SelectionLength = 0 Then
                            FindStr = .Text.Substring(0, .Text.Length - 1)
                        Else
                            FindStr = .Text.Substring(0, .SelectionStart - 1)
                        End If
                    End If

                Else
                    If .SelectionLength = 0 Then
                        FindStr = .Text & e.KeyChar
                    Else
                        FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                    End If

                End If

                If Trim(FindStr) <> "" Then
                    Condt = " Where item_name like '" & Trim(FindStr) & "%' or item_name like '% " & Trim(FindStr) & "%' "
                End If

                da = New SqlClient.SqlDataAdapter("select item_name from item_head " & Condt & " order by item_name", con)
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "item_name"

                .Text = Trim(FindStr)

                .SelectionStart = FindStr.Length

                e.Handled = True

            End If

        End With

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

    Private Sub cbo_Filter_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemName.LostFocus
        With cbo_Filter_ItemName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_Filter_PartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.LostFocus
        With cbo_Filter_PartyName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_SalesAc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.LostFocus
        With cbo_SalesAc
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_VatAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VatAc.GotFocus
        With cbo_VatAc
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With

    End Sub

    Private Sub cbo_VatAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyDown
        Try
            With cbo_VatAc
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_VehicleNo.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    dtp_RemovalDate.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_VatAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VatAc.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_VatAc

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        dtp_RemovalDate.Focus()

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        Condt = " (a.Ledger_idno = 0 or b.AccountsGroup_IdNo = 12) "
                        If Trim(FindStr) <> "" Then
                            Condt = " b.AccountsGroup_IdNo = 12 and (a.Ledger_DisplayName like '" & FindStr & "%' or a.Ledger_DisplayName like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Ledger_DisplayName"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
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


    Private Sub cbo_ItemDescription_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemDescription.GotFocus
        With cbo_ItemDescription
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_ItemDescription_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemDescription.KeyDown
        Try
            With cbo_ItemDescription
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_Rate_5Kg.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Colour.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ItemDescription_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemDescription.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_ItemDescription

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        cbo_Colour.Focus()

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where ItemGroup_Name like '" & FindStr & "%' or ItemGroup_Name like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select ItemGroup_Name from ItemGroup_Head " & Condt & " order by ItemGroup_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "ItemGroup_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_ItemDescription_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemDescription.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New ItemGroup_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemDescription.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_ItemDescription_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemDescription.LostFocus
        With cbo_ItemDescription
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub txt_Weight_Bag_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight_Bag.LostFocus
        txt_Weight_Bag.Text = Format(Val(txt_Weight_Bag.Text), "########0.000")
    End Sub

    Private Sub txt_Weight_Bag_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Weight_Bag.TextChanged
        txt_Weight.Text = Format(Val(txt_Bags.Text) * Val(txt_Weight_Bag.Text), "#########0.000")
    End Sub

    Private Sub txt_Weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Rate_Kg_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate_Kg.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_Kg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate_Kg.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Amount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Amount.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Amount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_BagNos_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BagNos.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_Colour.Focus()
    End Sub

    Private Sub txt_BagNos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BagNos.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_DelvAdd1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd1.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_DelvAdd1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_DelvAdd2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd2.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_DelvAdd2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_DelvAdd3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd3.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_DelvAdd3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd3.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VehicleNo.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VehicleNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub dtp_RemovalDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_RemovalDate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_RemovalDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_RemovalDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub dtp_RemovalTime_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_RemovalTime.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_RemovalTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_RemovalTime.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_CashDiscAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscAmount.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CashDiscAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscAmount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_VatPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VatPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_VatAc.Focus()
    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatPerc.KeyPress
        '  If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Trim(cbo_TaxType.Text) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then
            e.Handled = True

        Else
            If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True

        End If
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_VatAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VatAmount.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")

    End Sub

    Private Sub txt_VatAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatAmount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_VatPerc.TextChanged
        Gross_Discount_Tax_Amount_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub txt_AddLessAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess.KeyDown
        'If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_AddLessAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = MsgBoxResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_Rate_Kg_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate_Kg.LostFocus
        txt_Rate_Kg.Text = Format(Val(txt_Rate_Kg.Text), "########0.00")
    End Sub

    Private Sub txt_Weight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight.LostFocus
        txt_Weight.Text = Format(Val(txt_Weight.Text), "########0.000")
    End Sub



    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
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
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "12")

        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

    
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt_Vat.Clear()
        prn_DetDt_Vat.Clear()
        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0
        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt_Vat = New DataTable
            da1.Fill(prn_HdDt_Vat)


            If prn_HdDt_Vat.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name, d.ItemGroup_Name , g.Colour_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno LEFT OUTER JOIN ItemGroup_Head d ON a.ItemGroup_IdNo = d.ItemGroup_IdNo  LEFT OUTER JOIN Colour_Head g on a.Colour_idno = g.Colour_idno where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(prn_DetDt_Vat)

                da2.Dispose()

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        Dim I As Integer, K As Integer
        Dim ItmNm1 As String, ItmNm2 As String
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String

        Erase prn_DetAr

        prn_DetAr = New String(200, 10) {}

        Try

            '***** GST START *****
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)
            '***** GST END *****
            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, b.Item_Name_tamil, c.Unit_Name ,IG.ItemGroup_nAME ,CH.Colour_nAME from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN ItemGroup_Head IG ON a.ItemGroup_IdNo = IG.ItemGroup_IdNo  LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno LEFT OUTER JOIN Colour_Head CH ON A.Colour_iDNO = CH.Colour_iDNO where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then

                    prn_DetMxIndx = 0
                    For I = 0 To prn_DetDt.Rows.Count - 1

                        '***** GST START *****
                        ItmNm1 = Trim(prn_DetDt.Rows(I).Item("Item_Name").ToString) & "  " & Trim(prn_DetDt.Rows(I).Item("ItemGroup_Name").ToString)
                        '***** GST END *****
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For K = 30 To 1 Step -1
                                If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                            Next K
                            If K = 0 Then K = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K - 1)
                        End If

                        '***** GST START *****
                        prn_DetMxIndx = prn_DetMxIndx + 1
                        prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(I) + 1)
                        prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm1)
                        prn_DetAr(prn_DetMxIndx, 3) = prn_DetDt.Rows(I).Item("HSN_Code").ToString
                        prn_DetAr(prn_DetMxIndx, 4) = Val(prn_DetDt.Rows(I).Item("Tax_Perc").ToString) & " %"
                        prn_DetAr(prn_DetMxIndx, 5) = Val(prn_DetDt.Rows(I).Item("Noof_Items").ToString)
                        prn_DetAr(prn_DetMxIndx, 6) = prn_DetDt.Rows(I).Item("Unit_Name").ToString
                        prn_DetAr(prn_DetMxIndx, 7) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Rate").ToString), "########0.00"))
                        prn_DetAr(prn_DetMxIndx, 8) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Amount").ToString), "########0.00"))
                        prn_DetAr(prn_DetMxIndx, 9) = ""
                        prn_DetAr(prn_DetMxIndx, 10) = Val(prn_DetDt.Rows(I).Item("BAGS").ToString)
                        '***** GST END *****

                        If Trim(ItmNm2) <> "" Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = ""
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm2)
                            prn_DetAr(prn_DetMxIndx, 3) = ""
                            prn_DetAr(prn_DetMxIndx, 4) = ""
                            prn_DetAr(prn_DetMxIndx, 5) = ""
                            prn_DetAr(prn_DetMxIndx, 6) = ""
                            '***** GST START *****
                            prn_DetAr(prn_DetMxIndx, 7) = ""
                            prn_DetAr(prn_DetMxIndx, 8) = ""
                            prn_DetAr(prn_DetMxIndx, 9) = "ITEM_2ND_LINE"
                            '***** GST END *****
                            prn_DetAr(prn_DetMxIndx, 10) = ""
                        End If

                        If Trim(prn_DetDt.Rows(I).Item("Colour_Name").ToString) <> "" Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = ""
                            prn_DetAr(prn_DetMxIndx, 2) = "Color : " & Trim(prn_DetDt.Rows(I).Item("Colour_Name").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = ""
                            prn_DetAr(prn_DetMxIndx, 4) = ""
                            prn_DetAr(prn_DetMxIndx, 5) = ""
                            prn_DetAr(prn_DetMxIndx, 6) = ""
                            '***** GST START *****
                            prn_DetAr(prn_DetMxIndx, 7) = ""
                            prn_DetAr(prn_DetMxIndx, 8) = ""
                            prn_DetAr(prn_DetMxIndx, 9) = "ITEM_2ND_LINE"
                            '***** GST END *****
                            prn_DetAr(prn_DetMxIndx, 10) = ""
                        End If

                        If Trim(prn_DetDt.Rows(I).Item("Bag_Nos").ToString) <> "" Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = ""
                            prn_DetAr(prn_DetMxIndx, 2) = "Bag No : " & Trim(prn_DetDt.Rows(I).Item("Bag_Nos").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = ""
                            prn_DetAr(prn_DetMxIndx, 4) = ""
                            prn_DetAr(prn_DetMxIndx, 5) = ""
                            prn_DetAr(prn_DetMxIndx, 6) = ""
                            '***** GST START *****
                            prn_DetAr(prn_DetMxIndx, 7) = ""
                            prn_DetAr(prn_DetMxIndx, 8) = ""
                            prn_DetAr(prn_DetMxIndx, 9) = "ITEM_2ND_LINE"
                            prn_DetAr(prn_DetMxIndx, 10) = ""
                            '***** GST END *****
                        End If

                    Next I


                End If

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            da2.Dispose()

        End Try
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt_Vat.Rows.Count <= 0 Then Exit Sub
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1003" Then
            '  Printing_Format3Gst(e)
            Printing_GST_Format1(e)
        Else
            Printing_Format1(e)
        End If
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

        NoofItems_PerPage = 10

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

        Cmp_Name = "TSOFT SOLUTIONS"
        Cmp_Add1 = "4, IIIrd floor, R.A Tower"
        Cmp_Add2 = "P.N Road, Tirupur - 2."
        Cmp_PhNo = "PHONE : 96293 37417"
        Cmp_TinNo = "TIN NO. : 33554488556"
        Cmp_CstNo = "CST NO. : 998875 Dt. 01-04-2015"

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt_Vat.Rows.Count > 0 Then

                If Val(prn_HdDt_Vat.Rows(0).Item("Total_DiscountAmount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt_Vat.Rows(0).Item("Total_TaxAmount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt_Vat.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt_Vat.Rows(0).Item("Round_Off").ToString) <> 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try


                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt_Vat.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt_Vat.Rows.Count - 1

                            If NoofDets > NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            DetSNo = DetSNo + 1


                            ItmNm1 = Trim(prn_DetDt_Vat.Rows(DetIndx).Item("Item_Name").ToString)
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
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt_Vat.Rows(DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt_Vat.Rows(DetIndx).Item("Unit_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_Vat.Rows(DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_Vat.Rows(DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

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

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = "TSOFT SOLUTIONS"
        Cmp_Add1 = "4, IIIrd floor, R.A Tower"
        Cmp_Add2 = "P.N Road, Tirupur - 2."
        Cmp_PhNo = "PHONE : 96293 37417"
        Cmp_TinNo = "TIN NO. : 33554488556"
        Cmp_CstNo = "CST NO. : 998875 Dt. 01-04-2015"

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
            Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt_Vat.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 10, PageWidth, CurY + 10)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt_Vat.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO.    :  " & prn_HdDt_Vat.Rows(0).Item("Sales_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt_Vat.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt_Vat.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE    :  " & Format(Convert.ToDateTime(prn_HdDt_Vat.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt_Vat.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "     Tin No : " & prn_HdDt_Vat.Rows(0).Item("Ledger_TinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
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
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt_Vat.Rows(0).Item("Total_DiscountAmount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt_Vat.Rows(0).Item("Total_TaxAmount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, StrConv(Trim(prn_HdDt_Vat.Rows(0).Item("Tax_Type").ToString), vbProperCase), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Total_TaxAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt_Vat.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt_Vat.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt_Vat.Rows(0).Item("Net_Amount").ToString))
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
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)

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

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim I As Integer, NoofDets As Integer
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ItmDesc1 As String, ItmDesc2 As String
        Dim ps As Printing.PaperSize
        Dim Rup1 As String, Rup2 As String
        Dim BgNo1 As String, BgNo2 As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0 ' 65
            .Right = 0 ' 50
            .Top = 0 ' 65
            .Bottom = 0 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 12, FontStyle.Regular)
        pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        Try

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

            If prn_HdDt_Vat.Rows.Count > 0 Then

                CurX = 65 ' 40  '150
                CurY = 142 ' 122 ' 100
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt_Vat.Rows(0).Item("Ledger_MainName").ToString, CurX, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Ledger_Address1").ToString, CurX, CurY, 0, 0, pFont)

                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Ledger_Address3").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Ledger_Address4").ToString, CurX, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt_Vat.Rows(0).Item("Ledger_TinNo").ToString, CurX, CurY, 0, 0, pFont)
                End If

                CurX = 565
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Sales_No").ToString, CurX, 190, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt_Vat.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), CurX, 225, 0, 0, pFont)

                CurX = 155 ' 255
                CurY = 295  ' 265
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Delivery_Address1").ToString, CurX, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Delivery_Address2").ToString, CurX, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Delivery_Address3").ToString, CurX, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Vehicle_No").ToString, 565, 290, 0, 0, pFont)

                CurX = 635 ' 680
                CurY = 320 ' 270
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Removal_Date").ToString, CurX, CurY, 0, 0, pFont)

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Removal_Time").ToString, CurX, CurY, 0, 0, pFont)

                Try

                    NoofDets = 0

                    CurY = 420 ' 370

                    If prn_DetDt_Vat.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt_Vat.Rows.Count - 1

                            DetSNo = DetSNo + 1

                            ItmNm1 = Trim(prn_DetDt_Vat.Rows(DetIndx).Item("Item_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 6 Then
                                For I = 6 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 6
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            ItmDesc1 = Trim(prn_DetDt_Vat.Rows(DetIndx).Item("ItemGroup_Name").ToString)
                            ItmDesc2 = ""
                            If Len(ItmDesc1) > 20 Then
                                For I = 20 To 1 Step -1
                                    If Mid$(Trim(ItmDesc1), I, 1) = " " Or Mid$(Trim(ItmDesc1), I, 1) = "," Or Mid$(Trim(ItmDesc1), I, 1) = "." Or Mid$(Trim(ItmDesc1), I, 1) = "-" Or Mid$(Trim(ItmDesc1), I, 1) = "/" Or Mid$(Trim(ItmDesc1), I, 1) = "_" Or Mid$(Trim(ItmDesc1), I, 1) = "(" Or Mid$(Trim(ItmDesc1), I, 1) = ")" Or Mid$(Trim(ItmDesc1), I, 1) = "\" Or Mid$(Trim(ItmDesc1), I, 1) = "[" Or Mid$(Trim(ItmDesc1), I, 1) = "]" Or Mid$(Trim(ItmDesc1), I, 1) = "{" Or Mid$(Trim(ItmDesc1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 20
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmDesc1), Len(ItmDesc1) - I)
                                ItmDesc1 = Microsoft.VisualBasic.Left(Trim(ItmDesc1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), 30, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), 70, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc1), 140, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt_Vat.Rows(DetIndx).Item("Colour_Name").ToString), 140, CurY + TxtHgt, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt_Vat.Rows(DetIndx).Item("Bags").ToString), 420, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_Vat.Rows(DetIndx).Item("Weight").ToString), "########0.00"), 520, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_Vat.Rows(DetIndx).Item("Rate_5Kg").ToString), "########0.00"), 630, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_Vat.Rows(DetIndx).Item("Amount").ToString), "########0.00"), 770, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Or Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), 70, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc2), 140, CurY, 0, 0, pFont)

                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try


                BgNo1 = Trim(prn_HdDt_Vat.Rows(0).Item("Bag_Nos").ToString)
                BgNo2 = ""
                If Len(BgNo1) > 20 Then
                    For I = 20 To 1 Step -1
                        If Mid$(Trim(BgNo1), I, 1) = " " Or Mid$(Trim(BgNo1), I, 1) = "," Or Mid$(Trim(BgNo1), I, 1) = "." Or Mid$(Trim(BgNo1), I, 1) = "-" Or Mid$(Trim(BgNo1), I, 1) = "/" Or Mid$(Trim(BgNo1), I, 1) = "_" Or Mid$(Trim(BgNo1), I, 1) = "(" Or Mid$(Trim(BgNo1), I, 1) = ")" Or Mid$(Trim(BgNo1), I, 1) = "\" Or Mid$(Trim(BgNo1), I, 1) = "[" Or Mid$(Trim(BgNo1), I, 1) = "]" Or Mid$(Trim(BgNo1), I, 1) = "{" Or Mid$(Trim(BgNo1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 20
                    BgNo2 = Microsoft.VisualBasic.Right(Trim(BgNo1), Len(BgNo1) - I)
                    BgNo1 = Microsoft.VisualBasic.Left(Trim(BgNo1), I - 1)
                End If

                CurX = 770  '830
                CurY = 795  ' 770
                Common_Procedures.Print_To_PrintDocument(e, BgNo1, 100, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "SUB TOTAL", 440, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BgNo2, 100, CurY, 0, 0, pFont)
                If Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", 440, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)
                End If


                CurY = CurY + TxtHgt
                If Val(prn_HdDt_Vat.Rows(0).Item("Against_CForm_Status").ToString) <> 0 Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Against C Form", 100, CurY, 0, 0, pFont)
                End If
                If Val(prn_HdDt_Vat.Rows(0).Item("Total_TaxAmount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt_Vat.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt_Vat.Rows(0).Item("Tax_Perc").ToString)) & "%", 440, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Total_TaxAmount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                If Val(prn_HdDt_Vat.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add/Less", 440, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                If Val(prn_HdDt_Vat.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", 440, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Round_Off").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)
                End If

                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Net_Amount").ToString), "########0.00"), CurX, 935, 1, 0, p1Font)

            End If

            Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt_Vat.Rows(0).Item("Net_Amount").ToString))
            Rup2 = ""
            If Len(Rup1) > 70 Then
                For I = 70 To 1 Step -1
                    If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                Next I
                If I = 0 Then I = 70
                Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
            End If

            CurX = 155
            CurY = 975 ' 950
            Common_Procedures.Print_To_PrintDocument(e, Rup1, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Rup2, CurX, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub




    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_head", "Colour_name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        '  vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_ItemDescription, txt_BagNos, "Colour_head", "Colour_name", "", "(Colour_idno = 0)")
    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, txt_BagNos, "Colour_head", "Colour_name", "", "(Colour_idno = 0)")
    End Sub

    Private Sub cbo_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub Amount_Calculation()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0

        '***** GST START *****

        If FrmLdSTS = True Then Exit Sub




        If Trim(cbo_ItemName.Text) <> "" Then

            ItmIdNo = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)
            If ItmIdNo <> 0 Then

                txt_Hsn_Code.Text = ""
                txt_Gst_Perc.Text = ""

                If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                    da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then

                        If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                            txt_Hsn_Code.Text = dt.Rows(0)("Item_HSN_Code").ToString
                        End If
                        If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                            txt_Gst_Perc.Text = Format(Val(dt.Rows(0)("Item_GST_Percentage").ToString), "#########0.00")
                        End If

                    End If
                    dt.Clear()

                End If

                txt_Amount.Text = Format(Val(txt_Weight.Text) * Val(txt_Rate_Kg.Text), "#########0.00")
                txt_CashDiscAmount.Text = Format(Val(txt_Amount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")
                txt_Assessable.Text = Format(Val(txt_Amount.Text) - Val(txt_CashDiscAmount.Text), "#########0.00")

            End If

        End If


        TotalAmount_Calculation()


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

        txt_Assessable.Text = Format(TotAssval, "########0")
        txt_Assessable.Text = Format(Val(txt_Assessable.Text), "########0.00")

        lbl_CGstAmount.Text = Format(TotCGstAmt, "########0")
        lbl_CGstAmount.Text = Format(Val(lbl_CGstAmount.Text), "########0.00")

        lbl_SGstAmount.Text = Format(TotSGstAmt, "########0")
        lbl_SGstAmount.Text = Format(Val(lbl_SGstAmount.Text), "########0.00")

        lbl_IGstAmount.Text = Format(TotIGstAmt, "########0")
        lbl_IGstAmount.Text = Format(Val(lbl_IGstAmount.Text), "########0.00")

        Gross_Discount_Tax_Amount_Calculation()



    End Sub
    Private Sub Gross_Discount_Tax_Amount_Calculation()
        '***** GST START *****
        If FrmLdSTS = True Then Exit Sub

        txt_VatAmount.Text = ""
        If Trim(UCase(cbo_TaxType.Text)) = "VAT" Then
            txt_VatAmount.Text = Format(Val(txt_Assessable.Text) * Val(txt_VatPerc.Text) / 100, "#########0.00")
        End If
        NetAmount_Calculation()
        '***** GST END *****
    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        If FrmLdSTS = True Then Exit Sub

        NtAmt = Val(txt_Amount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_VatAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)

        txt_NetAmount.Text = Format(Val(NtAmt), "#########0")
        txt_NetAmount.Text = Common_Procedures.Currency_Format(Val(txt_NetAmount.Text))

        txt_RoundOff.Text = Format(Val(CSng(txt_NetAmount.Text)) - Val(NtAmt), "#########0.00")

        lbl_AmountInWords.Text = "Rupees :                                                                               "
        If Val(CSng(txt_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees : : " & Common_Procedures.Rupees_Converstion(Val(CSng(txt_NetAmount.Text)))
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

            txt_Hsn_Code.Text = ""
            txt_Gst_Perc.Text = ""

            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

                ItmIdNo = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

                '  lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")

                da = New SqlClient.SqlDataAdapter("Select b.* from item_head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.item_idno = " & Str(Val(ItmIdNo)), con)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)("itemgroup_name").ToString) = False Then
                        cbo_ItemDescription.Text = dt.Rows(0)("itemgroup_name").ToString
                    End If
                    If IsDBNull(dt.Rows(0)("Item_HSN_Code").ToString) = False Then
                        txt_Hsn_Code.Text = dt.Rows(0)("Item_HSN_Code").ToString
                    End If
                    If IsDBNull(dt.Rows(0)("Item_GST_Percentage").ToString) = False Then
                        txt_Gst_Perc.Text = dt.Rows(0)("Item_GST_Percentage").ToString
                    End If
                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            End If

            Amount_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT GET ITEM TAX DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_DateTime_Of_Supply, cbo_ItemName, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_ItemName, "", "", "", "", True)
            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
                    cbo_TaxType.Tag = cbo_TaxType.Text
                    Amount_Calculation()
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
                Amount_Calculation()
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        Amount_Calculation()
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub
    Private Sub txt_VatAmount_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_VatAmount.DoubleClick
        Dim VtAmt As String = ""

        VtAmt = InputBox("Enter vat Amount :", "FOR VAT AMOUNT ALTERATION....", Val(txt_VatAmount.Text))

        If Trim(VtAmt) <> "" Then
            If Val(VtAmt) <> 0 Then
                txt_VatAmount.Text = Format(Val(VtAmt), "#########0.00")
                NetAmount_Calculation()
            End If
        End If

        If txt_VatPerc.Visible And txt_VatPerc.Enabled Then txt_VatPerc.Focus()

    End Sub
    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, txt_DateTime_Of_Supply, cbo_TaxType, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, cbo_TaxType, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub get_Item_Unit_Rate_TaxPerc()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            cbo_ItemName.Tag = cbo_ItemName.Text
            da = New SqlClient.SqlDataAdapter("select a.* from item_head a  where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then


                If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                    txt_Rate_5Kg.Text = dt.Rows(0)("Sales_Rate").ToString
                End If
                get_Item_Tax(False)
            End If
            dt.Dispose()
            da.Dispose()
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

            AssVal_Frgt_Othr_Charges = Val(txt_Freight.Text) + Val(txt_AddLess.Text)

            cmd.Connection = con

            cmd.CommandText = "Truncate table EntryTemp"
            cmd.ExecuteNonQuery()



            If Trim(cbo_ItemName.Text) <> "" And Trim(txt_Hsn_Code.Text) <> "" And Val(txt_Gst_Perc.Text) <> 0 Then

                cmd.CommandText = "Insert into EntryTemp (                    Name1         ,                   Currency1            ,                       Currency2                                      ) " & _
                                  "            Values    ( '" & Trim(txt_Hsn_Code.Text) & "', " & Val(txt_Gst_Perc.Text) & ", " & Str(Val(txt_Assessable.Text) + AssVal_Frgt_Othr_Charges) & " ) "
                cmd.ExecuteNonQuery()

                AssVal_Frgt_Othr_Charges = 0

            End If



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
    Private Sub Printing_Format3Gst(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim I As Integer, NoofDets As Integer
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ItmDesc1 As String, ItmDesc2 As String
        Dim ps As Printing.PaperSize
        Dim Rup1 As String, Rup2 As String
        Dim BgNo1 As String, BgNo2 As String
        Dim vTaxPerc As Single = 0
        Dim EntryCode As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim vLine_Pen As Pen
        vLine_Pen = New Pen(Color.Black, 2)
        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0 ' 65
            .Right = 0 ' 50
            .Top = 0 ' 65
            .Bottom = 0 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 12, FontStyle.Regular)
        pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        Try

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

            If prn_HdDt_Vat.Rows.Count > 0 Then

                CurX = 65 ' 40  '150
                CurY = 142 ' 122 ' 100
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt_Vat.Rows(0).Item("Ledger_MainName").ToString, CurX, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Ledger_Address1").ToString, CurX, CurY, 0, 0, pFont)

                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Ledger_Address3").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Ledger_Address4").ToString, CurX, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt_Vat.Rows(0).Item("Ledger_TinNo").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Phone No : " & prn_HdDt_Vat.Rows(0).Item("Ledger_PhoneNo").ToString, CurX, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "State : " & prn_HdDt_Vat.Rows(0).Item("Ledger_State_Name").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt_Vat.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt_Vat.Rows(0).Item("Ledger_GSTinNo").ToString, CurX, CurY, 0, 0, pFont)
                End If


                CurX = 565
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Sales_No").ToString, CurX, 190, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt_Vat.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), CurX, 225, 0, 0, pFont)

                CurX = 155 ' 255
                CurY = 340 ' 265
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Delivery_Address1").ToString, CurX, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Delivery_Address2").ToString, CurX, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Delivery_Address3").ToString, CurX, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Vehicle_No").ToString, 565, 290, 0, 0, pFont)

                CurX = 635 ' 680
                CurY = 360 ' 270
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Removal_Date").ToString, CurX, CurY, 0, 0, pFont)

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_Vat.Rows(0).Item("Removal_Time").ToString, CurX, CurY, 0, 0, pFont)

                Try

                    NoofDets = 0

                    CurY = 450 ' 370

                    If prn_DetDt_Vat.Rows.Count > 0 Then

                        Do While DetIndx <= prn_DetDt_Vat.Rows.Count - 1

                            DetSNo = DetSNo + 1

                            ItmNm1 = Trim(prn_DetDt_Vat.Rows(DetIndx).Item("Item_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 6 Then
                                For I = 6 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 6
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            ItmDesc1 = Trim(prn_DetDt_Vat.Rows(DetIndx).Item("ItemGroup_Name").ToString)
                            ItmDesc2 = ""
                            If Len(ItmDesc1) > 20 Then
                                For I = 20 To 1 Step -1
                                    If Mid$(Trim(ItmDesc1), I, 1) = " " Or Mid$(Trim(ItmDesc1), I, 1) = "," Or Mid$(Trim(ItmDesc1), I, 1) = "." Or Mid$(Trim(ItmDesc1), I, 1) = "-" Or Mid$(Trim(ItmDesc1), I, 1) = "/" Or Mid$(Trim(ItmDesc1), I, 1) = "_" Or Mid$(Trim(ItmDesc1), I, 1) = "(" Or Mid$(Trim(ItmDesc1), I, 1) = ")" Or Mid$(Trim(ItmDesc1), I, 1) = "\" Or Mid$(Trim(ItmDesc1), I, 1) = "[" Or Mid$(Trim(ItmDesc1), I, 1) = "]" Or Mid$(Trim(ItmDesc1), I, 1) = "{" Or Mid$(Trim(ItmDesc1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 20
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmDesc1), Len(ItmDesc1) - I)
                                ItmDesc1 = Microsoft.VisualBasic.Left(Trim(ItmDesc1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(DetSNo)), 30, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), 70, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc1), 140, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt_Vat.Rows(DetIndx).Item("Colour_Name").ToString), 140, CurY + TxtHgt, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt_Vat.Rows(DetIndx).Item("HSN_Code").ToString), 320, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt_Vat.Rows(DetIndx).Item("Tax_Perc").ToString) & "%", 400, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt_Vat.Rows(DetIndx).Item("Bags").ToString), 500, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_Vat.Rows(DetIndx).Item("Weight").ToString), "########0.00"), 580, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_Vat.Rows(DetIndx).Item("Rate_5Kg").ToString), "########0.00"), 680, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_Vat.Rows(DetIndx).Item("Amount").ToString), "########0.00"), 770, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Or Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), 70, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc2), 140, CurY, 0, 0, pFont)

                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try


                BgNo1 = Trim(prn_HdDt_Vat.Rows(0).Item("Bag_Nos").ToString)
                BgNo2 = ""
                If Len(BgNo1) > 20 Then
                    For I = 20 To 1 Step -1
                        If Mid$(Trim(BgNo1), I, 1) = " " Or Mid$(Trim(BgNo1), I, 1) = "," Or Mid$(Trim(BgNo1), I, 1) = "." Or Mid$(Trim(BgNo1), I, 1) = "-" Or Mid$(Trim(BgNo1), I, 1) = "/" Or Mid$(Trim(BgNo1), I, 1) = "_" Or Mid$(Trim(BgNo1), I, 1) = "(" Or Mid$(Trim(BgNo1), I, 1) = ")" Or Mid$(Trim(BgNo1), I, 1) = "\" Or Mid$(Trim(BgNo1), I, 1) = "[" Or Mid$(Trim(BgNo1), I, 1) = "]" Or Mid$(Trim(BgNo1), I, 1) = "{" Or Mid$(Trim(BgNo1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 20
                    BgNo2 = Microsoft.VisualBasic.Right(Trim(BgNo1), Len(BgNo1) - I)
                    BgNo1 = Microsoft.VisualBasic.Left(Trim(BgNo1), I - 1)
                End If

                CurX = 770  '830
                CurY = 580 ' 770
                Common_Procedures.Print_To_PrintDocument(e, BgNo1, 100, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "SUB TOTAL", 440, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BgNo2, 100, CurY, 0, 0, pFont)
                If Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", 440, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)

                End If
                CurY = CurY + TxtHgt
                If Val(prn_HdDt_Vat.Rows(0).Item("Against_CForm_Status").ToString) <> 0 Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Against C Form", 100, CurY, 0, 0, pFont)
                End If

                If Val(prn_HdDt_Vat.Rows(0).Item("Freight_Amount").ToString) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Freight_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)
                End If
                CurY = CurY + TxtHgt

                If Val(prn_HdDt_Vat.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt_Vat.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), CurX, CurY, 1, 0, pFont)


                End If

                vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

                If Val(prn_HdDt_Vat.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt_Vat.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt_Vat.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                    If Val(prn_HdDt_Vat.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Or Val(prn_HdDt_Vat.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt_Vat.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                        CurY = CurY + TxtHgt
                        ' e.Graphics.DrawLine(Pens.Black, CurX, CurY, PageWidth, CurY)
                    Else
                        CurY = CurY + 10
                    End If

                    CurY = CurY + TxtHgt - 10

                    p1Font = New Font("Calibri", 10, FontStyle.Bold)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Assessable_Value").ToString), "########0.00"), CurX, CurY, 1, 0, p1Font)

                End If
                CurY = CurY + TxtHgt
                If Val(prn_HdDt_Vat.Rows(0).Item("CGst_Amount").ToString) <> 0 Then

                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt_Vat.Rows(0).Item("Entry_GST_Tax_Type").ToString) & " @ " & Trim(Val(vTaxPerc)) & " %", 440, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "", 440, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("CGst_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt_Vat.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt

                If vTaxPerc <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt_Vat.Rows(0).Item("Entry_GST_Tax_Type").ToString) & " @ " & Trim(Val(vTaxPerc)) & " %", 0, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, " ", 440, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("SGst_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)

            End If

            If Val(prn_HdDt_Vat.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt

                If vTaxPerc <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt_Vat.Rows(0).Item("Entry_GST_Tax_Type").ToString) & " @ " & Trim(Val(vTaxPerc)) & " %", 440 - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, " ", 440, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("IGst_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)

            End If
            '***** GST END *****

            CurY = CurY + TxtHgt
            If Val(prn_HdDt_Vat.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Round Off", 440, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_Vat.Rows(0).Item("Round_Off").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)

            End If

            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            ' Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt_Vat.Rows(0).Item("Net_Amount").ToString)), CurX, CurY, 1, 0, p1Font)





            Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt_Vat.Rows(0).Item("Net_Amount").ToString))
            Rup2 = ""
            If Len(Rup1) > 70 Then
                For I = 70 To 1 Step -1
                    If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                Next I
                If I = 0 Then I = 70
                Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
            End If

            CurX = 155
            CurY = 850 ' 950
            Common_Procedures.Print_To_PrintDocument(e, Rup1, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Rup2, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            If vNoofHsnCodes <> 0 Then
                'Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), vLine_Pen)
            End If
        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub
    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByRef CurY As Single, ByRef TopLnYAxis As Single, ByVal vLine_Pen As Pen)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer = 0
        Dim p1Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SNo As Integer = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String = ""

        Try

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 100 : SubClAr(2) = 100 : SubClAr(3) = 45 : SubClAr(4) = 90 : SubClAr(5) = 45 : SubClAr(6) = 90 : SubClAr(7) = 45 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin, CurY + 15, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + SubClAr(1), CurY, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY)
            LnAr2 = CurY
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1), CurY - 10, 2, SubClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY - 10, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)


            Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)


            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0

                CurY = CurY - 15

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Format(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString), "############0")), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString), "###########0.00")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString), "###########0")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString), "###########0.00")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString), "############0")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString), "############0.00")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString), "###############0")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString), "###########0")), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)

                    prn_DetIndx = prn_DetIndx + 1

                Loop

            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Format(Val(Ttl_TaxAmt), "############0")), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Format(Val(Ttl_CGst), "############0")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Format(Val(Ttl_Sgst), "############0")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Format(Val(Ttl_CGst), "############0")), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Format(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst), "###############0")), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), TopLnYAxis)

            CurY = CurY + 5
            BmsInWrds = ""
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    'Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt As New DataTable
    '    Dim I As Integer, NoofDets As Integer
    '    Dim p1Font As Font
    '    Dim p2Font As Font
    '    Dim SubClAr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim SNo As Integer = 0
    '    Dim NoofItems_Increment As Integer
    '    Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
    '    Dim LnAr2 As Single
    '    Dim BmsInWrds As String

    '    Try

    '        TxtHgt = TxtHgt - 1

    '        p2Font = New Font("Calibri", 9, FontStyle.Regular)

    '        Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

    '        Erase SubClAr

    '        SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '        SubClAr(1) = 140 : SubClAr(2) = 130 : SubClAr(3) = 60 : SubClAr(4) = 95 : SubClAr(5) = 60 : SubClAr(6) = 90 : SubClAr(7) = 60
    '        SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

    '        Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
    '        LnAr2 = CurY
    '        CurY = CurY + 5

    '        Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(Pk_Condition) & Trim(EntryCode) & "'", con)
    '        Dt = New DataTable
    '        Da.Fill(Dt)

    '        If Dt.Rows.Count > 0 Then

    '            prn_DetIndx = 0
    '            NoofDets = 0
    '            NoofItems_Increment = 0

    '            CurY = CurY - 20

    '            Do While prn_DetIndx <= Dt.Rows.Count - 1

    '                ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

    '                ItmNm2 = ""
    '                If Len(ItmNm1) > 40 Then
    '                    For I = 35 To 1 Step -1
    '                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                    Next I
    '                    If I = 0 Then I = 40
    '                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                End If



    '                CurY = CurY + TxtHgt + 3

    '                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
    '                Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
    '                Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
    '                Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
    '                Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
    '                Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
    '                Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
    '                Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

    '                NoofItems_Increment = NoofItems_Increment + 1

    '                NoofDets = NoofDets + 1

    '                Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)
    '                Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)
    '                Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)
    '                Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)
    '                prn_DetIndx = prn_DetIndx + 1
    '            Loop

    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        CurY = CurY + TxtHgt - 15
    '        Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
    '        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
    '        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
    '        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)
    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

    '        e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


    '        CurY = CurY + 5
    '        p1Font = New Font("Calibri", 12, FontStyle.Regular)
    '        BmsInWrds = ""
    '        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
    '        BmsInWrds = Replace(Trim(BmsInWrds), "", "")


    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "Tax Amount (In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try



    'End Sub


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

    Private Sub txt_Gst_Perc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Gst_Perc.KeyDown
        If e.KeyCode = 40 Then
            txt_DelvAdd1.Focus()
        End If
        If e.KeyCode = 38 Then
            txt_Hsn_Code.Focus()
        End If
    End Sub

    Private Sub txt_Gst_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Gst_Perc.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_DelvAdd1.Focus()

        End If
    End Sub

    Private Sub Printing_GST_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim vNoofHsnCodes As Integer = 0

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
            .Top = 40 ' 65
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)

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
        TxtHgt = 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 11 ' 14  ' 19  

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 45
        ClArr(2) = 205
        ClArr(3) = 70
        ClArr(4) = 50
        ClArr(5) = 50
        ClArr(6) = 50
        ClArr(7) = 50
        ClArr(8) = 75
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                '***** GST START *****
                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 7
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                '***** GST END *****

                Printing_GST_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0
                    If prn_Count > 1 Then
                        CurY = CurY + TxtHgt
                    End If

                    CurY = CurY - TxtHgt - 10

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "100 % HOSIERY GOODS", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    End If

                    If prn_DetMxIndx > 0 Then

                        Do While DetIndx <= prn_DetMxIndx

                            If NoofDets > NoofItems_PerPage Then

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                NoofDets = NoofDets + 1
                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                e.HasMorePages = True

                                Return

                            End If

                   

                            CurY = CurY + TxtHgt
                            '***** GST START *****
                            'If DetIndx <> 1 And Val(prn_DetAr(DetIndx, 1)) <> 0 Then
                            '    CurY = CurY + 2
                            'End If
                            '***** GST END *****

                            If Trim(prn_DetAr(DetIndx, 2)) <> "" And Trim(prn_DetAr(DetIndx, 9)) = "SERIALNO" Then
                                CurY = CurY - 3
                                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 25, CurY, 0, 0, p1Font)

                            ElseIf Trim(prn_DetAr(DetIndx, 2)) <> "" And Trim(prn_DetAr(DetIndx, 9)) = "ITEM_2ND_LINE" Then
                                CurY = CurY - 3
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 25, CurY, 0, 0, pFont)


                            Else
                                '***** GST START *****
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 3), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 10), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                '***** GST END *****
                            End If

                            NoofDets = NoofDets + 1

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If Trim(prn_InpOpts) <> "" Then
                        If prn_Count < Len(Trim(prn_InpOpts)) Then

                            DetIndx = 1
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
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
                    prn_OriDupTri = "BUYER COPY"
                    PrintDocument1.DefaultPageSettings.Color = True
                    PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
                    e.PageSettings.Color = True
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "TRANSPORT COPY"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "ASSESSE COPY"
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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then
            p1Font = New Font("Calibri", 20, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
        End If
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
            W1 = e.Graphics.MeasureString("Date & Time of Supply  :", pFont).Width
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY - 3, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)


            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Electronic Ref.No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If

            BlockInvNoY = BlockInvNoY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse charge (Y/N)", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "N", LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)

            BlockInvNoY = BlockInvNoY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY + 10, PageWidth, BlockInvNoY + 10)

            BlockInvNoY = BlockInvNoY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Led_State, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)

            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Removal_Date").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Supply", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Removal_Date").ToString & " " & prn_HdDt.Rows(0).Item("Removal_Time").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)
            End If


            If Trim(prn_HdDt.Rows(0).Item("Transportation_Mode").ToString) <> "" Then
                BlockInvNoY = BlockInvNoY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                BlockInvNoY = BlockInvNoY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
                BlockInvNoY = BlockInvNoY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Lr No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + Cen1 + W1 + 75, BlockInvNoY, 0, 0, p1Font)

            End If

            '***** GST END *****

            '----------------------------


            If BlockInvNoY > CurY Then CurY = BlockInvNoY

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))

            '***** GST START *****
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            '***** GST END *****
            CurY = CurY + TxtHgt + 10
            'CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim BInc As Integer = 0
        Dim BnkDetAr() As String
        Dim vTaxPerc As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim CurY1 As Single = 0
        Dim vLine_Pen As Pen
        vLine_Pen = New Pen(Color.Black, 2)
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_BAGS").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))



         

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


            CurY1 = CurY

            p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
            '***** GST START *****
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Delivery Address", LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + 10, CurY1, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + 10, CurY1, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Address3").ToString, LMargin + 10, CurY1, 0, 0, pFont)
            End If

            CurY = CurY - 10

            '***** GST START *****
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                End If
            End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                If is_LastPage = True Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " % - 00010001", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " % - 00030001", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " % - 00020001", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5))

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

            '***** GST START *****
            '=============GST SUMMARY============
            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            If vNoofHsnCodes <> 0 Then
                'Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
                Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), vLine_Pen)
            End If
            '==========================
            '***** GST END *****


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We Declare that this Invoice Shows the actual price of the goods", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "described and that all particulars are true and correct.", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

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
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Pdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pdf.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        Print_PDF_Status = False
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub
End Class