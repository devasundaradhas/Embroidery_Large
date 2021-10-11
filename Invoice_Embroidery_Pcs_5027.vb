Imports System.IO

Public Class Invoice_Embroidery_Pcs_5027

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GINVE-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private vcmb_ItmNm As String
    Private vcmb_SizNm As String

    Private prn_HdDt_VAT As New DataTable
    Private prn_DetDt_VAT As New DataTable

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetAr(200, 15) As String

    Private prn_PageNo As Integer
    Private prn_DetMxIndx As Integer
    'Private NoCalc_Status As Boolean = False
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_DetDt_VAT1 As New DataTable
    Private prn_DetIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_OriDupTri As String = ""
    Private Ord_No As String = ""
    Private Ord_Date As String = ""

    Dim ImageInAutoBill As Boolean
    Dim CloseOrderAfterAutoBill As Boolean
    Dim AutoBillOrdNo As String
    Dim AutoBillRow As Integer
    Dim CurrPrintingRow As Integer

    Dim DCCODES As String = ""
    Dim DCCODES1 As String = ""

    Private Order_Disp_Cond As String = ""

    Dim cbo_Buff As String

    Public previlege As String

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

        NoCalc_Status = True

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
        txt_CashDiscAmount.Text = ""
        'txt_VechileNo.Text = ""
        lbl_NetAmount.Text = ""
        lbl_GrossAmount.Text = ""
        cbo_EntType.Text = "DIRECT"
        Picture_Box.BackgroundImage = Nothing

        pnl_InputDetails.Enabled = True
        cbo_EntType.Enabled = True
        lbl_AmountInWords.Text = "Rupees  :  "
        cbo_TaxType.Text = "GST"
        chk_AgainstFormH.Checked = False

        Dim Buff As String = lbl_Grid_GstPerc.Text
        Dim Buff1 As String = lbl_Grid_HsnCode.Text

        txt_Quantity.BackColor = Color.White
        txt_Quantity.ForeColor = Color.Black

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            'cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            'cbo_Filter_ItemName.SelectedIndex = -1
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
        '***** GST START *****
        lbl_Assessable.Text = ""
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        'lbl_OrderNo.Text = ""
        lbl_Design.Text = ""
        'lbl_Grid_GstPerc.Text = ""
        lbl_IGstAmount.Text = ""
        lbl_CGstAmount.Text = ""
        lbl_SGstAmount.Text = ""

        '***** GST END *****
        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dtp_CutOffDate.Value = Now
        DCCODES = ""

        dtp_CutOffDate.Value = Now
        cbo_ItemName.Text = "EMBROIDERY"
        lbl_Grid_GstPerc.Text = Buff
        lbl_Grid_HsnCode.Text = Buff1


        txt_SlNo.Text = "1"
        NoCalc_Status = False
        ImageInAutoBill = False
        CloseOrderAfterAutoBill = False
        AutoBillOrdNo = ""
        AutoBillRow = -1

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
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Amount_Calculation(ByVal GridAll_Row_STS As Boolean)
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim i As Integer = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If GridAll_Row_STS = True Then

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        ItmIdNo = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                        If ItmIdNo <> 0 Then

                            .Rows(i).Cells(15).Value = ""
                            .Rows(i).Cells(16).Value = 0

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

                            .Rows(i).Cells(8).Value = Format(Val(.Rows(i).Cells(6).Value) * Val(.Rows(i).Cells(7).Value), "#########0.00")
                            .Rows(i).Cells(12).Value = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
                            .Rows(i).Cells(13).Value = Format(Val(.Rows(i).Cells(8).Value) * Val(.Rows(i).Cells(12).Value) / 100, "#########0.00")
                            .Rows(i).Cells(14).Value = Format(Val(.Rows(i).Cells(8).Value) - Val(.Rows(i).Cells(13).Value), "#########0.00")

                        End If

                    End If

                Next

            End With

            TotalAmount_Calculation()

        Else

            lbl_Amount.Text = Format(Val(txt_Quantity.Text) * Val(txt_Rate.Text), "#########0.00")
            lbl_Grid_DiscPerc.Text = Format(Val(txt_CashDiscPerc.Text), "#########0.00")
            lbl_Grid_DiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(lbl_Grid_DiscPerc.Text) / 100, "#########0.00")
            lbl_Grid_AssessableValue.Text = Format(Val(lbl_Amount.Text) - Val(lbl_Grid_DiscAmount.Text), "#########0.00")

        End If

    End Sub

    Private Sub TotalAmount_Calculation()

        Dim Sno As Integer = 0
        Dim TotQty As Decimal = 0
        Dim TotNoSts As Decimal = 0
        Dim TotGrsAmt As Decimal = 0
        Dim TotDiscAmt As Decimal = 0
        Dim TotAssval As Decimal = 0
        Dim TotCGstAmt As Decimal = 0
        Dim TotSGstAmt As Decimal = 0
        Dim TotIGstAmt As Decimal = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotQty = 0
        TotGrsAmt = 0
        TotDiscAmt = 0
        TotAssval = 0

        For i = 0 To dgv_Details.RowCount - 1

            Sno = Sno + 1

            dgv_Details.Rows(i).Cells(0).Value = Sno

            If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                TotNoSts = TotNoSts + Val(dgv_Details.Rows(i).Cells(4).Value)
                TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(7).Value)
                TotGrsAmt = TotGrsAmt + Val(dgv_Details.Rows(i).Cells(8).Value)

                If Val(txt_CashDiscPerc.Text) > 0 Then
                    TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(13).Value)
                Else
                    TotDiscAmt = Val(txt_CashDiscAmount.Text)
                End If

                TotAssval = TotAssval + Val(dgv_Details.Rows(i).Cells(14).Value)

            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotNoSts)
            .Rows(0).Cells(7).Value = Val(TotQty)
            .Rows(0).Cells(8).Value = Format(Val(TotGrsAmt), "########0.00")
            .Rows(0).Cells(13).Value = Format(Val(TotDiscAmt), "########0.00")
            .Rows(0).Cells(14).Value = Format(Val(TotAssval), "########0.00")
        End With

        lbl_GrossAmount.Text = Format(TotGrsAmt, "########0.00")

        If Val(txt_CashDiscPerc.Text) > 0 Then
            txt_CashDiscAmount.Text = Format(TotDiscAmt, "########0.00")
        End If

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

    Private Sub Amount_Calculation()

        lbl_Amount.Text = Format(Val(txt_Quantity.Text) * Val(txt_Rate.Text), "#########0.00")

    End Sub

    'Private Sub GrossAmount_Calculation()
    '    Dim I As Integer
    '    Dim Sno As Integer
    '    Dim TotQty As Decimal, TotAmt As Decimal

    '    Sno = 0
    '    TotQty = 0
    '    TotAmt = 0

    '    With dgv_Details

    '        For I = 0 To .RowCount - 1
    '            Sno = Sno + 1
    '            dgv_Details.Rows(I).Cells(0).Value = Sno

    '            If Trim(.Rows(I).Cells(1).Value) <> "" Or Val(.Rows(I).Cells(5).Value) <> 0 Then

    '                TotQty = TotQty + Val(dgv_Details.Rows(I).Cells(5).Value)
    '                TotAmt = TotAmt + Val(dgv_Details.Rows(I).Cells(7).Value)

    '            End If

    '        Next

    '    End With

    '    With dgv_Details_Total
    '        If .Rows.Count = 0 Then .Rows.Add()
    '        .Rows(0).Cells(5).Value = Val(TotQty)
    '        .Rows(0).Cells(7).Value = Format(Val(TotAmt), "########0.00")
    '    End With

    '    lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")

    '    NetAmount_Calculation()

    'End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal



        'txt_CashDiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

        ''lbl_Assessable.Text = Format(Val(lbl_GrossAmount.Text) - Val(txt_CashDiscAmount.Text), "#########0.00")

        ''lbl_VatAmount.Text = Format(Val(lbl_Assessable.Text) * Val(txt_VatPerc.Text) / 100, "#########0.00")

        'NtAmt = Val(lbl_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)
        NtAmt = Val(lbl_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)


        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

        txt_RoundOff.Text = Format(Val(lbl_NetAmount.Text) - Val(NtAmt), "#########0.00")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Private Sub get_Item_Unit_Rate_TaxPerc()

        'Dim da As SqlClient.SqlDataAdapter
        'Dim dt As New DataTable

        'If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
        '    cbo_ItemName.Tag = cbo_ItemName.Text
        '    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
        '    dt = New DataTable
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then

        '        If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
        '            txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
        '        End If
        '        get_Item_Tax(False)
        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If

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

            'If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)

            ItmIdNo = Common_Procedures.Item_NameToIdNo(con, "EMBROIDERY")
            'ItmIdNo = "EMBROIDERY"

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

            'End If

            Amount_Calculation(False)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT GET ITEM TAX DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Get_HSN_CodeWise_GSTTax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt4 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim AssVal_Frgt_Othr_Charges As Double = 0
        Dim LedIdNo As Integer = 0
        Dim ItmIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

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

            If Trim(UCase(cbo_TaxType.Text)) = "GST" Then
                With dgv_Details

                    If .Rows.Count > 0 Then
                        For i = 0 To .Rows.Count - 1

                            If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(16).Value) <> 0 And Val(.Rows(i).Cells(14).Value) <> 0 Then

                                cmd.CommandText = "Insert into EntryTemp (                    Name1                ,                   Currency1            ,                       Currency2                                      ) " &
                                                  "            Values    ( '" & Trim(.Rows(i).Cells(15).Value) & "', " & (Val(.Rows(i).Cells(16).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value) + AssVal_Frgt_Othr_Charges) & " ) "
                                cmd.ExecuteNonQuery()

                                AssVal_Frgt_Othr_Charges = 0

                            End If

                        Next
                    End If
                End With
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



        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
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

        NoCalc_Status = True
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString
                'cbo_Transport.Text = Common_Procedures.Transport_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                txt_OrderNo.Tag = dt1.Rows(0).Item("Order_No").ToString
                txt_OrderNo.Text = Join(Split(txt_OrderNo.Tag, "$$$"), ",")

                txt_PartyRefNo.Text = dt1.Rows(0).Item("Party_Ref_No").ToString
                txt_Dcno.Text = dt1.Rows(0).Item("Dc_No").ToString
                txt_DcDate.Text = dt1.Rows(0).Item("Dc_Date").ToString
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                lbl_Assessable.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString

                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                cbo_PaymentMethod.Text = dt1.Rows(0).Item("Payment_Method").ToString


                cbo_EntType.Text = dt1.Rows(0).Item("Entry_Type").ToString
                chk_AgainstFormH.Checked = IIf(Val(dt1.Rows(0).Item("Form_H_Status").ToString) <> 0, True, False)

                If Not IsDBNull(dt1.Rows(0).Item("DC_Cutoff_Date")) Then
                    dtp_CutOffDate.Value = dt1.Rows(0).Item("DC_Cutoff_Date")
                End If

                '***** GST START *****
                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGst_Amount").ToString), "########0.00")
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGst_Amount").ToString), "########0.00")
                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGst_Amount").ToString), "########0.00")
                '***** GST END ********

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, c.Unit_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo " &
                                                   " LEFT OUTER JOIN Unit_Head c on a.Unit_idno = c.Unit_idno where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        dgv_Details.Rows(n).Cells(21).Value = dt2.Rows(i).Item("Ordercode_forSelection").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Dc_No").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)

                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Rate_1000Stitches").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Quantity").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Sales_Detail_SlNo").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Sales_Delivery_Code").ToString
                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Sales_Delivery_Detail_SlNo").ToString

                        '***** GST START *****
                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Perc_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Amount_For_All_Item").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Assessable_Value").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("HSN_Code").ToString
                        dgv_Details.Rows(n).Cells(16).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                        '***** GST END *****
                        dgv_Details.Rows(n).Cells(17).Value = dt2.Rows(i).Item("Details_Design").ToString
                        dgv_Details.Rows(n).Cells(19).Value = Common_Procedures.Colour_IdNoToName(con, Val(dt2.Rows(i).Item("Colour_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(20).Value = Common_Procedures.Size_IdNoToName(con, Val(dt2.Rows(i).Item("Size_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Order_No").ToString

                        If IsDBNull(dt2.Rows(n).Item("Design_Picture")) = False Then
                            Dim imageData As Byte() = DirectCast(dt2.Rows(n).Item("Design_Picture"), Byte())
                            If Not imageData Is Nothing Then
                                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                                    ms.Write(imageData, 0, imageData.Length)
                                    If imageData.Length > 0 Then

                                        dgv_Details.Rows(n).Cells(18).Value = Image.FromStream(ms)

                                    End If
                                End Using
                            End If
                        End If

                        dgv_Details.Rows(n).Cells(22).Value = False

                        If Not IsDBNull(dt2.Rows(i).Item("Close_Order")) Then
                            If dt2.Rows(i).Item("Close_Order") = True Then
                                dgv_Details.Rows(n).Cells(22).Value = True
                            End If
                        End If

                        dgv_Details.Rows(n).Cells(23).Value = dt2.Rows(i).Item("DCCODES").ToString
                        dgv_Details.Rows(n).Cells(24).Value = dt2.Rows(i).Item("Job_No").ToString

                        If Not IsDBNull(dt2.Rows(i).Item("Component_IdNo")) Then
                            dgv_Details.Rows(n).Cells(25).Value = Common_Procedures.Component_IdNoToName(con, dt2.Rows(i).Item("Component_IdNo"))
                        End If

                        dgv_Details.Rows(n).Cells(26).Value = "PCS-PIECES"

                        If Not IsDBNull(dt2.Rows(i).Item("Unit_IdNo")) Then
                            dgv_Details.Rows(n).Cells(26).Value = Common_Procedures.Unit_IdNoToName(con, dt2.Rows(i).Item("Unit_IdNo"))
                        End If

                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                '***** GST START *****

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

                '***** GST END *****

                'With dgv_Details_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                '    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("SubTotal_Amount").ToString), "########0.00")
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

        NoCalc_Status = False

        TotalAmount_Calculation()

    End Sub

    Private Sub Invoice_Embroidery_Pcs_5027_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated



        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            'cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Invoice_Embroidery_Pcs_5027_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'dtp_Date.MaxDate = Common_Procedures.settings.Validation_End_Date

        Me.Text = ""

        con.Open()
        cbo_EntType.Enabled = False

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

        cbo_PaymentMethod.Items.Clear()
        cbo_PaymentMethod.Items.Add("CASH")
        cbo_PaymentMethod.Items.Add("CREDIT")

        'cbo_TransportMode.Items.Clear()

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("NO TAX")
        cbo_TaxType.Items.Add("GST")

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1201" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1117" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "5002" Then  '--- SWASTHICK KNITT (Tirupur ) Embroidery
        '    Label41.Text = "Party Dc.No"
        'End If


        '***** GST START *****

        pnl_GSTTax_Details.Visible = False
        pnl_GSTTax_Details.Left = (Me.Width - pnl_GSTTax_Details.Width) \ 2
        pnl_GSTTax_Details.Top = ((Me.Height - pnl_GSTTax_Details.Height) \ 2) - 100
        pnl_GSTTax_Details.BringToFront()

        '***** GST END *****

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Dcno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OrderCode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RAte_1000Stitches.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Quantity.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDiscAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Component.GotFocus, AddressOf ControlGotFocus

        '***** GST START *****

        AddHandler cbo_PaymentMethod.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_JobNumber.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DCNo.GotFocus, AddressOf ControlGotFocus

        '***** GST END *****

        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_AgainstFormH.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noof_Stitches.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EntType.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Dcno.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OrderCode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OrderCode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RAte_1000Stitches.LostFocus, AddressOf ControlLostFocus
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
        AddHandler txt_PartyRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noof_Stitches.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EntType.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_AgainstFormH.LostFocus, AddressOf ControlLostFocus

        '***** GST START *****

        AddHandler cbo_PaymentMethod.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_JobNumber.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DCNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Component.LostFocus, AddressOf ControlLostFocus

        '***** GST END *****

        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyRefNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Quantity.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Noof_Stitches.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Dcno.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RAte_1000Stitches.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDiscAmount.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Noof_Stitches.KeyDown, AddressOf TextBoxControlKeyDown

        '***** GST START *****
        'AddHandler txt_Electronic_RefNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_DateTime_Of_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        '***** GST END *****

        'AddHandler txt_VechileNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Noof_Stitches.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Dcno.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDiscAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RAte_1000Stitches.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyRefNo.KeyPress, AddressOf TextBoxControlKeyPress

        '***** GST START *****

        'AddHandler txt_Electronic_RefNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_DateTime_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress

        '***** GST END *****


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True

        With dgv_Details
            .Columns(24).DisplayIndex = 3
            .Columns(19).DisplayIndex = 4
            .Columns(25).DisplayIndex = 5
            .Columns(21).DisplayIndex = 1
            .Columns(17).DisplayIndex = 2
            .Columns(15).DisplayIndex = 6
            .Columns(26).DisplayIndex = 14
        End With

        For I As Integer = 0 To dgv_Details_Total.ColumnCount - 1
            If I <> 18 Then
                dgv_Details_Total.Columns(I).DefaultCellStyle = dgv_Details.Columns(I).DefaultCellStyle
                dgv_Details_Total.Columns(I).Width = dgv_Details.Columns(I).Width
                dgv_Details_Total.Columns(I).Visible = dgv_Details.Columns(I).Visible
            End If
        Next

        With dgv_Details_Total
            .Columns(24).DisplayIndex = 3
            .Columns(19).DisplayIndex = 4
            .Columns(25).DisplayIndex = 5
            .Columns(21).DisplayIndex = 1
            .Columns(17).DisplayIndex = 2
            .Columns(15).DisplayIndex = 6
            .Columns(26).DisplayIndex = 14
        End With

        If Common_Procedures.settings.CustomerCode = "5008" Then
            Label18.Text = "Material Value Debit :"
        End If

        new_record()

    End Sub

    Private Sub Invoice_Embroidery_Pcs_5027_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        con.Close()
        con.Dispose()

    End Sub

    Private Sub Invoice_Embroidery_Pcs_5027_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

                'Me.Close

            Else

                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("D") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim trans As SqlClient.SqlTransaction

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Pk_Condition = "LBINV-" Then
        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Labour_Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        'Else
        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Tax_Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        'End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If


        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = a.Receipt_Quantity - b.Quantity from Sales_dELIVERY_Details a, Sales_Details b where b.Sales_Code = '" & Trim(NewCode) & "' and b.Entry_Type = 'DELIVERY' and a.Sales_Delivery_Code = b.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = b.Sales_Delivery_Detail_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "Delete from Voucher_Bill_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Bill_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Bill_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Bill_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Order_Program_Head Set Close_Status = 0 Where OrderCode_forSelection In " &
                               "  (Select OrderCode_forSelection From Sales_Details Where Sales_Code = '" & Trim(NewCode) & "') and not " &
                               "  OrderCode_forSelection In (Select OrderCode_forSelection From Sales_Details Where Not Sales_Code = '" & Trim(NewCode) & "' And Close_Order = 1)"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "DELETE FROM Invoice_DC_Details Where Sales_Code = '" & NewCode & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()


            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            'cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            'cbo_Filter_ItemName.SelectedIndex = -1
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
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' AND  ISNULL(ISDIRECT,0) <> 1 Order by for_Orderby, Sales_No"
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

            da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "'  AND  ISNULL(ISDIRECT,0) <> 1 Order by for_Orderby, Sales_No", con)
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

            '

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Sales_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' AND  ISDIRECT <> 1 Order by for_Orderby desc, Sales_No desc"


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

        Dim da As New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "'  AND  ISNULL(ISDIRECT,0) <> 1 Order by for_Orderby desc, Sales_No desc", con)
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

        'Exit Sub

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True
            CloseOrderAfterAutoBill = False

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "(Sales_Code like '" & Trim(Pk_Condition) & "%' or Sales_Code like  'GEIVD-%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select  a.* from Sales_Head a  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '" & Trim(Pk_Condition) & "%' and a.Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Sales_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then

                If dt2.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_EntType.Text = dt2.Rows(0).Item("Entry_Type").ToString
                If dt2.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Tax_Type").ToString

            End If

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

            'If Common_Procedures.settings.CustomerCode = "1117" Then
            'chk_AutoPopulate.Checked = False
            'rdo_OrderNoInHeader.Checked = True
            'End If

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

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("I") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

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

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("A") And Not UCase(previlege).Contains("E") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

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
        Dim Clr_id As Integer = 0
        Dim Sz_id As Integer = 0
        Dim comp_idno As Int16 = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim vTot_Qty As Single = 0
        Dim itm_GrpId As Integer = 0
        Dim VouType As String = ""
        Dim VouBil As String = ""
        Dim FormH_sts As Integer = 0
        Dim Close_Order As String

        TotalAmount_Calculation()

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
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

        'trans_id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)
        'If trans_id = 0 And Trim(cbo_Transport.Text) <> "" Then
        '    MessageBox.Show("Invalid Transport Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Transport.Enabled Then cbo_Transport.Focus()
        '    Exit Sub
        'End If

        saleac_id = 0
        If saleac_id = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            saleac_id = 22
        End If

        txac_id = 0


        With dgv_Details

            For i = 0 To dgv_Details.RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then


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

                    If Val(.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(6)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

        NoCalc_Status = False
        Amount_Calculation(False)

        vTot_Qty = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTot_Qty = Val(dgv_Details_Total.Rows(0).Cells(7).Value)
            If vTot_Qty = 0 Then

                For I As Integer = 0 To dgv_Details.RowCount - 1
                    vTot_Qty = vTot_Qty + Val(dgv_Details.Rows(I).Cells(7).Value)
                Next

                If vTot_Qty = 0 Then
                    MsgBox("Total Quantity Cannot be Zero")
                    Exit Sub
                End If

            End If


        End If


        FormH_sts = 0
        If chk_AgainstFormH.Checked = True Then
            FormH_sts = 1
        End If

        If Not New_Entry Then
            If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("E") Then
                MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
                Exit Sub
            End If
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "(Sales_Code like '" & Trim(Pk_Condition) & "%' or Sales_Code like  'GEIVD-%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sales_Head(Sales_Code ,             Company_IdNo         ,              Sales_No             ,                               for_OrderBy                                  , Sales_Date,           Ledger_IdNo   ,          SalesAc_IdNo      ,            TaxAc_IdNo    ,         Transport_IdNo    ,              Dc_No           ,               Dc_Date          ,             Total_Qty     ,               SubTotal_Amount           , Total_DiscountAmount, Total_TaxAmount,                Gross_Amount           ,               CashDiscount_Perc        ,               CashDiscount_Amount        ,             Assessable_Value         ,               Tax_Type          ,               Tax_Perc            ,                Tax_Amount           ,              Freight_Amount       ,              AddLess_Amount       ,               Round_Off            ,                Net_Amount                  ,              Order_No             ,                Party_Ref_No         ,       Entry_Type                 ,Form_H_Status          ,              Entry_GST_Tax_Type ,                 CGst_Amount          ,                 SGst_Amount          ,               IGst_Amount            ,Payment_Method                 ,  ISDIRECT , DC_Cutoff_Date                                      ,User_Name) " &
                                    " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(txac_id)) & ", " & Str(Val(trans_id)) & ",  '" & Trim(txt_Dcno.Text) & "', '" & Trim(txt_DcDate.Text) & "',  " & Str(Val(vTot_Qty)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ",          0          ,        0       , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(lbl_Assessable.Text)) & ", '" & Trim(cbo_TaxType.Text) & "',                   0                 ,                    0               , " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & " ,   '" & Trim(txt_OrderNo.Tag) & "', '" & Trim(txt_PartyRefNo.Text) & "',  '" & Trim(cbo_EntType.Text) & "'," & Val(FormH_sts) & " , '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(lbl_CGstAmount.Text)) & ", " & Str(Val(lbl_SGstAmount.Text)) & ", " & Str(Val(lbl_IGstAmount.Text)) & " ,'" & Trim(cbo_PaymentMethod.Text) & "', 0   ,'" & Format(dtp_CutOffDate.Value, "dd-MMM-yyyy") & "' ,'" & Common_Procedures.User.RealName.ToString & "')"
                cmd.ExecuteNonQuery()
            Else
                cmd.CommandText = "Update Sales_Head set Sales_Date = @SalesDate, Ledger_IdNo = " & Str(Val(led_id)) & ", SalesAc_IdNo = " & Str(Val(saleac_id)) & ", TaxAc_IdNo = " & Str(Val(txac_id)) & ",  Transport_IdNo = " & Str(Val(trans_id)) & ", Dc_No = '" & Trim(txt_Dcno.Text) & "', Dc_Date = '" & Trim(txt_DcDate.Text) & "', Total_Qty = " & Str(Val(vTot_Qty)) & ", SubTotal_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Gross_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_CashDiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_CashDiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_Assessable.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Perc = 0, Tax_Amount = 0, Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Entry_Type = '" & Trim(cbo_EntType.Text) & "' , Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & " ,  Order_No =  '" & Trim(txt_OrderNo.Tag) & "' , Party_Ref_No = '" & Trim(txt_PartyRefNo.Text) & "',Form_H_Status = " & Val(FormH_sts) & " , Entry_GST_Tax_Type = '" & Trim(cbo_TaxType.Text) & "',  CGst_Amount = " & Str(Val(lbl_CGstAmount.Text)) & " , SGst_Amount = " & Str(Val(lbl_SGstAmount.Text)) & " , IGst_Amount = " & Str(Val(lbl_IGstAmount.Text)) & " ,Payment_Method ='" & Trim(cbo_PaymentMethod.Text) & "', ISDIRECT = 0 , DC_Cutoff_Date = '" & Format(dtp_CutOffDate.Value, "dd-MMM-yyyy") & "',User_Name = '" & Common_Procedures.User.RealName.ToString & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = a.Receipt_Quantity - b.Quantity from Sales_dELIVERY_Details a, Sales_Details b where b.Sales_Code = '" & Trim(NewCode) & "' and b.Entry_Type = 'DELIVERY' and a.Sales_Delivery_Code = b.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = b.Sales_Delivery_Detail_SlNo"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Update Order_Program_Head Set Close_Status = 0 Where OrderCode_forSelection In " &
                                 "  (Select OrderCode_forSelection From Sales_Details Where Sales_Code = '" & Trim(NewCode) & "') and not " &
                                 "  OrderCode_forSelection In (Select OrderCode_forSelection From Sales_Details Where Not Sales_Code = '" & Trim(NewCode) & "' And Close_Order = 1)"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Sales_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "DELETE FROM Invoice_DC_Details Where Sales_Code = '" & NewCode & "'"
            cmd.ExecuteNonQuery()

            Sno = 0
            Dim nr As Integer

            With dgv_Details

                For i = 0 To dgv_Details.RowCount - 1

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" Or Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                        itm_id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        itm_GrpId = Common_Procedures.Item_NameToItemGroupIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Clr_id = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(19).Value, tr)
                        Sz_id = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(20).Value, tr)
                        comp_idno = Common_Procedures.Component_NameToIdNo(con, .Rows(i).Cells(25).Value, tr)

                        Sno = Sno + 1

                        '   If Trim(dgv_Details.Rows(i).Cells(17).Value) <> "" Then
                        Dim ms As New MemoryStream()
                        If IsNothing(dgv_Details.Rows(i).Cells(18).Value) = False Then
                            Dim PIC As Image
                            PIC = dgv_Details.Rows(i).Cells(18).Value
                            Dim bitmp As New Bitmap(PIC)
                            bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                            'PictureBox1.BackgroundImage.Save(ms, PictureBox1.BackgroundImage.RawFormat)
                        End If

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@SalesDate", dtp_Date.Value.Date)
                        Dim data As Byte() = ms.GetBuffer()
                        Dim p As New SqlClient.SqlParameter("@photo", SqlDbType.Image)
                        p.Value = data
                        cmd.Parameters.Add(p)
                        ms.Dispose()
                        'End If

                        'cmd.CommandText = "Update Sales_Details set Sales_Date= @SalesDate, Entry_Type = '" & Trim(cbo_EntType.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ",  Sl_No = " & Str(Val(Sno)) & ", Item_Idno = " & Str(Val(itm_id)) & ", ItemGroup_IdNo = " & Str(Val(itm_GrpId)) & ", Size_IdNo = " & Str(Val(Sz_id)) & " ,  Bags = " & Str(Val(.Rows(i).Cells(4).Value)) & ", Noof_Items = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Unit_Idno = " & Val(unt_id) & ", Rate = " & Str(Val(.Rows(i).Cells(6).Value)) & ", Amount = " & Str(Val(.Rows(i).Cells(6).Value)) & ",Total_Amount = " & Str(Val(.Rows(i).Cells(7).Value)) & " ,   Sales_Order_Code = '" & Trim(.Rows(i).Cells(8).Value) & "', Sales_Order_Detail_SlNo = " & Str(Val(.Rows(i).Cells(9).Value)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "' and Sales_Detail_SlNo = " & Str(Val(.Rows(i).Cells(10).Value))
                        'nr = cmd.ExecuteNonQuery()
                        'If nr = 0 Then

                        If .Rows(i).Cells(22).Value = True Then
                            Close_Order = "1"
                        Else
                            Close_Order = "0"
                        End If

                        cmd.CommandText = "Insert into Sales_Details ( Sales_Code            ,            Company_IdNo          ,              Sales_No             ,                                              for_OrderBy                   , Sales_Date,          Ledger_IdNo    ,            SL_No     ,          Item_IdNo      , ItemGroup_IdNo            ,      Dc_No                            ,                            Noof_Items    ,                 Rate_1000Stitches        ,    Rate                                 ,             Quantity                     ,      Amount                               ,   Total_Amount                          ,                    Entry_Type    ,                  Sales_Delivery_Code    ,            Sales_dELIVERY_Detail_SlNo      ,       Cash_Discount_Perc_For_All_Item     ,       Cash_Discount_Amount_For_All_Item   ,              Assessable_Value             ,                      HSN_Code           ,                      Tax_Perc              , GST_Percentage                              ,Details_Design                             , Design_Picture   ,    Colour_IdNo        ,     Size_IdNo      , OrderCode_Forselection                ,Close_Order            ,    Unit_IdNo                                                                        , Order_No                               ,Job_No                                  ,DCCODES                                 ,Component_IdNo)" &
                                                " Values             ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @SalesDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & "," & Str(Val(itm_GrpId)) & ",'" & Trim(.Rows(i).Cells(3).Value) & "', " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & " , '" & Trim(cbo_EntType.Text) & "' ,'" & Trim(.Rows(i).Cells(10).Value) & "' ,  " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & ", '" & Trim(.Rows(i).Cells(15).Value) & "', " & Str(Val(.Rows(i).Cells(16).Value)) & " ,  " & Str(Val(.Rows(i).Cells(16).Value)) & " , '" & Trim(.Rows(i).Cells(17).Value) & "' , @photo            ,  " & Val(Clr_id) & "  , " & Val(Sz_id) & " ,'" & Trim(.Rows(i).Cells(21).Value) & "'," & Close_Order & "   ,'" & Common_Procedures.Unit_NameToIdNo(con, Trim(.Rows(i).Cells(26).Value), tr) & "' ,'" & Trim(.Rows(i).Cells(2).Value) & "','" & Trim(.Rows(i).Cells(24).Value) & "','" & Trim(.Rows(i).Cells(23).Value) & "'," & comp_idno.ToString & ")"
                        cmd.ExecuteNonQuery()

                        Dim clr_idno As Int16

                        clr_idno = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(19).Value, tr)

                        If UCase(Trim(Trim(.Rows(i).Cells(3).Value))) <> "ALL" Then

                            cmd.CommandText = "Insert into Invoice_DC_Details ( Sales_Code,            Sales_DC_Code         ,Job_No  ,UID , Colour_IdNo  ,Component_IdNo) Values ('" & NewCode & "', '" & Trim(.Rows(i).Cells(3).Value) & "' , '" & Trim(.Rows(i).Cells(24).Value) & "' , '" & Trim(.Rows(i).Cells(2).Value) & "'," & clr_idno.ToString & "," & comp_idno.ToString & ")"
                            cmd.ExecuteNonQuery()

                        Else

                            For j As Integer = 0 To Split(.Rows(i).Cells(23).Value, "$$$").GetUpperBound(0)
                                cmd.CommandText = "Insert into Invoice_DC_Details ( Sales_Code,            Sales_DC_Code         ,Job_No     ,UID, Colour_IdNo , Component_IdNo) Values ('" & NewCode & "', '" & Trim(Split(.Rows(i).Cells(23).Value, "$$$")(j)) & "' , '" & Trim(.Rows(i).Cells(24).Value) & "' , '" & Trim(.Rows(i).Cells(2).Value) & "'," & clr_idno.ToString & "," & comp_idno.ToString & ")"
                                cmd.ExecuteNonQuery()
                            Next

                        End If

                        If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then

                            cmd.CommandText = "Update Sales_Delivery_Details Set Receipt_Quantity = Receipt_Quantity + " & Str(Val(.Rows(i).Cells(7).Value)) & " where Sales_Delivery_Code = '" & Trim(.Rows(i).Cells(10).Value) & "' and Sales_Delivery_Detail_SlNo = " & Str(Val(.Rows(i).Cells(11).Value)) & " and Ledger_IdNo = " & Str(Val(led_id))
                            nr = cmd.ExecuteNonQuery()

                            If nr = 0 Then
                                tr.Rollback()
                                MessageBox.Show("Mismatch of Delivery and Party details", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                                Exit Sub
                            End If

                        End If

                    End If

                Next

                cmd.CommandText = "Update Order_Program_Head Set Close_Status = 1 Where OrderCode_forSelection In " &
                                  "(Select OrderCode_forSelection From Sales_Details Where Sales_Code = '" & Trim(NewCode) & "' and Close_Order = 1)"
                cmd.ExecuteNonQuery()


            End With

            '---Tax Details

            cmd.CommandText = "Delete from Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_GSTTax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Sales_GST_Tax_Details   (        Sales_Code      ,               Company_IdNo       ,                Sales_No           ,                               for_OrderBy                                  , Sales_Date ,         Ledger_IdNo     ,            Sl_No     ,                    HSN_Code            ,                      Taxable_Amount      ,                      CGST_Percentage     ,                      CGST_Amount         ,                      SGST_Percentage      ,                      SGST_Amount         ,                      IGST_Percentage     ,                      IGST_Amount          ) " &
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
            vVouPos_IdNos = AcPos_ID & "|" & saleac_id & "|" & "25|26|27|9|17|24"

            ' vVouPos_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(lbl_RoundOff.Text)) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(txt_Freight.Text) & "|" & Val(txt_AddLess.Text) & "|" & Val(lbl_RoundOff.Text)
            vVouPos_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(txt_RoundOff.Text)) & "|" & Val(lbl_CGstAmount.Text) & "|" & Val(lbl_SGstAmount.Text) & "|" & Val(lbl_IGstAmount.Text) & "|" & Val(txt_Freight.Text) & "|" & Val(txt_AddLess.Text) & "|" & Val(txt_RoundOff.Text)

            Dim Vou_Type As String

            If Common_Procedures.settings.CustomerCode = "5002" Or Common_Procedures.settings.CustomerCode = "1201" Then
                Vou_Type = "Rcpt"
            Else
                Vou_Type = "GST-Sales"
            End If

            If Common_Procedures.Voucher_Updation(con, Vou_Type, Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Bill No . : " & Trim(lbl_InvoiceNo.Text), vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If

            '---Bill Posting

            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, led_id, Trim(lbl_InvoiceNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(NewCode), tr)

            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            move_record(lbl_InvoiceNo.Text)

            New_Entry = False

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

            ImageInAutoBill = False
            CloseOrderAfterAutoBill = False
            AutoBillOrdNo = ""
            AutoBillRow = -1

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click

        If btn_Add.Enabled = False Then
            Exit Sub
        End If

        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim itm_id As Integer = 0
        Dim Sz_id As Integer = 0
        Dim PIC As Image

        itm_id = Common_Procedures.Item_NameToIdNo(con, "EMBROIDERY")

        If Val(itm_id) = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
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

        If Val(txt_SlNo.Text) = 1 Then
            txt_PartyRefNo.Text = Ord_Date
        End If

        get_Item_Tax(False)

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(1).Value = "EMBROIDERY"

                    .Rows(i).Cells(21).Value = cbo_OrderCode.Text
                    .Rows(i).Cells(3).Value = cbo_DCNo.Text

                    .Rows(i).Cells(4).Value = Val(txt_Noof_Stitches.Text)

                    .Rows(i).Cells(5).Value = Format(Val(txt_RAte_1000Stitches.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(txt_Quantity.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(lbl_Amount.Text), "########0.00")

                    .Rows(i).Cells(12).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(13).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")

                    .Rows(i).Cells(14).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")

                    If cbo_TaxType.Text = "GST" Then
                        .Rows(i).Cells(15).Value = lbl_Grid_HsnCode.Text
                        .Rows(i).Cells(16).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")
                    Else
                        .Rows(i).Cells(15).Value = ""
                        .Rows(i).Cells(16).Value = "0.00"
                    End If

                    .Rows(i).Cells(24).Value = cbo_JobNumber.Text

                    .Rows(i).Cells(19).Value = cbo_Colour.Text

                    .Rows(i).Cells(25).Value = cbo_Component.Text

                    '.Rows(i).Cells(2).Value = lbl_OrderNo.Text
                    .Rows(i).Cells(17).Value = lbl_Design.Text
                    .Rows(i).Cells("DC_CODES").Value = DCCODES

                    PIC = Nothing
                    PIC = Picture_Box.BackgroundImage

                    .Rows(i).Cells(18).Value = PIC

                    .Rows(i).Cells(26).Value = txt_UoM.Text

                    MtchSTS = True

                    If i >= 7 Then .FirstDisplayedScrollingRowIndex = i - 6

                    Exit For

                End If



            Next

            If MtchSTS = False Then

                n = .Rows.Add()

                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(1).Value = "EMBROIDERY"
                .Rows(n).Cells(21).Value = cbo_OrderCode.Text
                .Rows(n).Cells(3).Value = cbo_DCNo.Text
                .Rows(n).Cells(4).Value = Val(txt_Noof_Stitches.Text)
                .Rows(n).Cells(5).Value = Format(Val(txt_RAte_1000Stitches.Text), "########0.00")
                .Rows(n).Cells(6).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(txt_Quantity.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(lbl_Amount.Text), "########0.00")
                .Rows(n).Cells(12).Value = Format(Val(lbl_Grid_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(13).Value = Format(Val(lbl_Grid_DiscAmount.Text), "########0.00")
                .Rows(n).Cells(14).Value = Format(Val(lbl_Grid_AssessableValue.Text), "########0.00")
                .Rows(n).Cells(15).Value = lbl_Grid_HsnCode.Text
                .Rows(n).Cells(16).Value = Format(Val(lbl_Grid_GstPerc.Text), "########0.00")
                '.Rows(n).Cells(2).Value = lbl_OrderNo.Text
                .Rows(n).Cells(17).Value = lbl_Design.Text

                If cbo_DCNo.Text = "ALL" Then
                    .Rows(n).Cells("DC_CODES").Value = DCCODES
                Else
                    .Rows(n).Cells("DC_CODES").Value = cbo_DCNo.Text
                End If

                .Rows(n).Cells(24).Value = cbo_JobNumber.Text
                .Rows(n).Cells(19).Value = cbo_Colour.Text
                .Rows(n).Cells(25).Value = cbo_Component.Text

                .Rows(n).Cells(26).Value = txt_UoM.Text

                PIC = Nothing
                PIC = Picture_Box.BackgroundImage

                .Rows(n).Cells(18).Value = PIC

                If n >= 7 Then .FirstDisplayedScrollingRowIndex = n - 6

            End If



            If Common_Procedures.settings.CustomerCode = "1201" Or Common_Procedures.settings.CustomerCode = "5002" Then

                txt_OrderNo.Tag = txt_OrderNo.Text

            Else

                txt_OrderNo.Text = ""
                txt_OrderNo.Tag = ""

                For I As Integer = 0 To dgv_Details.RowCount - 1
                    If I > 0 Then
                        For j = 0 To I - 1
                            If Trim(.Rows(I).Cells(24).Value) = Trim(.Rows(j).Cells(24).Value) Then
                                GoTo a
                            End If
                        Next
                    End If
                    If Len(Trim(.Rows(I).Cells(24).Value)) Then
                        If Len(txt_OrderNo.Tag) > 0 Then
                            txt_OrderNo.Tag = txt_OrderNo.Tag + "$$$"
                        End If
                        txt_OrderNo.Tag = txt_OrderNo.Tag + Trim(.Rows(I).Cells(24).Value)
                        txt_OrderNo.Text = Join(Split(txt_OrderNo.Tag, "$$$"), ",")
                    End If
a:
                Next

            End If

        End With

        TotalAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_OrderCode.Text = ""
        txt_Noof_Stitches.Text = ""
        txt_RAte_1000Stitches.Text = ""
        txt_Quantity.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        lbl_Grid_HsnCode.Text = ""
        cbo_DCNo.Text = ""
        cbo_JobNumber.Text = ""
        lbl_Design.Text = ""
        cbo_Colour.Text = ""
        cbo_Component.Text = ""
        txt_UoM.Text = ""

        DCCODES = ""

        txt_Quantity.BackColor = Color.White
        txt_Quantity.ForeColor = Color.Black

        Picture_Box.BackgroundImage = Nothing

        Grid_Cell_DeSelect()

        If cbo_OrderCode.Enabled And cbo_OrderCode.Visible Then cbo_OrderCode.Focus()

    End Sub

    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call Amount_Calculation(False)
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation(False)
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus

        'With cbo_ItemName
        'vcmb_ItmNm = Trim(.Text)
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
        'End With

    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        'vcbo_KeyDwnVal = e.KeyValue

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, txt_VechileNo, cbo_OrderCode, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        'If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        'If Trim(cbo_ItemName.Text) <> "" Then
        'cbo_OrderCode.Focus()
        'Else
        'txt_CashDiscPerc.Focus()
        'End If
        'End If

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress

        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, cbo_OrderCode, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        'If Asc(e.KeyChar) = 13 Then
        'If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
        'get_Item_Unit_Rate_TaxPerc()
        'End If
        'If Trim(cbo_ItemName.Text) <> "" Then
        'cbo_OrderCode.Focus()
        'Else
        'txt_CashDiscPerc.Focus()
        'End If
        'End If

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

        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            'get_Item_Unit_Rate_TaxPerc()
        End If
        If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
            da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then


                'If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
                '    txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
                'End If
                get_Item_Tax(False)
            End If
            dt.Dispose()
            da.Dispose()
        End If

        'If Trim(UCase(vcmb_ItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
        '    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
        '    dt = New DataTable
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then

        '        If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
        '            txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
        '        End If

        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_AddLess.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_EntType, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If txt_OrderNo.Enabled Then
                txt_OrderNo.Focus()

            Else

                cbo_PaymentMethod.Focus()

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
                        cbo_PaymentMethod.Focus()

                    End If

                End If

            Else
                txt_OrderNo.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    'Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_DcDate, txt_VechileNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_VechileNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    'End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    Dim f As New Transport_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
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
        Amount_Calculation(True)
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        TotalAmount_Calculation()
    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown

        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            txt_CashDiscAmount.Focus()
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
        If e.KeyCode = 38 Then e.Handled = True : cbo_TaxType.Focus()

    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress

        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then
            cbo_ItemName.Focus()
            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                        'cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
                        'txt_DcNo_ItemWise.Text = (dgv_Details.CurrentRow.Cells(3).Value)
                        txt_Noof_Stitches.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                        txt_RAte_1000Stitches.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                        txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
                        txt_Quantity.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
                        lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")


                        lbl_Grid_DiscPerc.Text = Format(Val(.Rows(i).Cells(12).Value), "########0.00")
                        lbl_Grid_DiscAmount.Text = Format(Val(.Rows(i).Cells(13).Value), "########0.00")

                        lbl_Grid_AssessableValue.Text = Format(Val(.Rows(i).Cells(14).Value), "########0.00")

                        lbl_Grid_GstPerc.Text = Format(Val(.Rows(i).Cells(16).Value), "########0.00")

                        'txt_DetailsDesign.Text = Trim(.Rows(i).Cells(17).Value)
                        'cbo_colour.Text = Trim(.Rows(i).Cells(19).Value)
                        'cbo_Size.Text = Trim(.Rows(i).Cells(20).Value)
                        Exit For

                    End If

                Next

            End With

        End If
    End Sub

    'Private Sub txt_SerialNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DetailsDesign.KeyDown
    '    If e.KeyCode = 40 Then btn_Add.Focus() ' SendKeys.Send("{TAB}")
    '    If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub

    'Private Sub txt_SerialNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DetailsDesign.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        btn_Add_Click(sender, e)
    '        'SendKeys.Send("{TAB}")
    '    End If
    'End Sub

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

            If Len(Trim(Condt)) > 0 Then
                Condt = Condt + " AND "
            End If

            Condt = Condt + " ISNULL(ISDIRECT,0) <> 1 "


            'If Trim(cbo_Filter_ItemName.Text) <> "" Then
            'Itm_IdNo = Common_Procedures.Item_NameToIdNo(con, cbo_Filter_ItemName.Text)
            'End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.Sales_No, a.Sales_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Sales_Head a " &
                                              " INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) &
                                              " and a.Sales_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' " &
                                              " and a.Sales_Code LIKE '" & Pk_Condition & "%' " &
                                              IIf(Trim(Condt) <> "", " and ", "") &
                                              Condt & " Order by a.for_orderby, a.Sales_No", con)
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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")
    End Sub

    'Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

    '    If Asc(e.KeyChar) = 13 Then
    '        btn_Filter_Show_Click(sender, e)
    '    End If

    'End Sub

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
            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                        If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                            .Rows(.CurrentCell.RowIndex).Cells(8).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(6).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value), "#########0.00")
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


        If pnl_InputDetails.Enabled = True And txt_SlNo.Enabled = True Then

            If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

                txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
                cbo_ItemName.Text = "EMBROIDERY"
                cbo_OrderCode.Text = Trim(dgv_Details.CurrentRow.Cells(21).Value)
                lbl_Design.Text = Trim(dgv_Details.CurrentRow.Cells(17).Value)
                cbo_JobNumber.Text = dgv_Details.CurrentRow.Cells(24).Value
                cbo_Colour.Text = dgv_Details.CurrentRow.Cells(19).Value
                cbo_Component.Text = dgv_Details.CurrentRow.Cells(25).Value

                QuantityDetails()

                txt_Noof_Stitches.Text = Val(dgv_Details.CurrentRow.Cells(4).Value)
                txt_RAte_1000Stitches.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
                txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
                txt_Quantity.Text = Val(dgv_Details.CurrentRow.Cells(7).Value)
                txt_UoM.Text = dgv_Details.CurrentRow.Cells(26).Value
                lbl_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
                lbl_Grid_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
                lbl_Grid_DiscAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")
                lbl_Grid_AssessableValue.Text = Format(Val(dgv_Details.CurrentRow.Cells(14).Value), "########0.00")
                lbl_Grid_HsnCode.Text = dgv_Details.CurrentRow.Cells(15).Value
                lbl_Grid_GstPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(16).Value), "########0.00")
                cbo_DCNo.Text = dgv_Details.CurrentRow.Cells(3).Value
                DCCODES = dgv_Details.CurrentRow.Cells("DC_CODES").Value
                Dim DCODES() As String = Split(DCCODES, "$$$")
                For I = 0 To DCODES.GetUpperBound(0)
                    DCODES(I) = "'" & DCODES(I) + "'"
                Next
                DCCODES1 = Join(DCODES, ",")

                QuantityDetails()

                Picture_Box.BackgroundImage = dgv_Details.CurrentRow.Cells(18).Value

                If cbo_OrderCode.Enabled And cbo_OrderCode.Visible Then cbo_OrderCode.Focus()

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

            txt_OrderNo.Text = ""
            txt_OrderNo.Tag = ""

            For I As Integer = 0 To dgv_Details.RowCount - 1
                If Len(Trim(.Rows(I).Cells(2).Value)) Then
                    If Len(txt_OrderNo.Tag) > 0 Then
                        txt_OrderNo.Tag = txt_OrderNo.Tag + "$$$"
                    End If
                    txt_OrderNo.Tag = txt_OrderNo.Tag + Trim(.Rows(I).Cells(2).Value)
                    txt_OrderNo.Text = Join(Split(txt_OrderNo.Tag, "$$$"), ",")
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
        'cbo_ItemName.Text = ""
        txt_RAte_1000Stitches.Text = ""
        txt_Noof_Stitches.Text = ""
        txt_Quantity.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        'txt_DcNo_ItemWise.Text = ""
        lbl_Grid_DiscPerc.Text = ""
        lbl_Grid_DiscAmount.Text = ""
        lbl_Grid_AssessableValue.Text = ""
        'lbl_Grid_GstPerc.Text = ""
        'txt_DetailsDesign.Text = ""
        Picture_Box.BackgroundImage = Nothing
        'cbo_colour.Text = ""
        'cbo_Size.Text = ""
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
            'cbo_ItemName.Text = ""
            txt_Noof_Stitches.Text = ""
            txt_RAte_1000Stitches.Text = ""
            txt_Quantity.Text = ""
            txt_Rate.Text = ""
            lbl_Amount.Text = ""
            'txt_DcNo_ItemWise.Text = ""
            lbl_Grid_DiscPerc.Text = ""
            lbl_Grid_DiscAmount.Text = ""
            lbl_Grid_AssessableValue.Text = ""
            'lbl_Grid_GstPerc.Text = ""
            Picture_Box.BackgroundImage = Nothing
            'cbo_colour.Text = ""
            'cbo_Size.Text = ""
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

    Private Sub txt_OrderDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PartyRefNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_OrderDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PartyRefNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            txt_PartyRefNo.Text = Date.Today
            txt_PartyRefNo.SelectionStart = txt_PartyRefNo.Text.Length
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
            cbo_EntType.Focus()
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





    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

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


        CurrPrintingRow = 0

        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "12")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    If Common_Procedures.settings.CustomerCode = "5002" Then
                        PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    Else
                        PrintDocument1.PrinterSettings.PrinterName = "doPDF 9"
                    End If

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

        Dim I As Integer, K As Integer
        Dim ItmNm As String
        Dim ItmNm1 As String, ItmNm2 As String
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim OrdNo As String = ""
        Dim MxDetLen As Integer

        If Common_Procedures.settings.CustomerCode = "1086" Then
            MxDetLen = 45
        ElseIf Common_Procedures.settings.CustomerCode = "5027" Then
            MxDetLen = 60
        Else
            MxDetLen = 35
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt_VAT.Clear()
        prn_DetDt_VAT.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetDt_VAT1.Clear()
        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0 '1
        DetSNo = 0
        prn_DetMxIndx = 0
        prn_Count = 0

        ' Try

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sales_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(NewCode) & "' ", con)
            prn_HdDt_VAT = New DataTable
            da1.Fill(prn_HdDt_VAT)

            If prn_HdDt_VAT.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Sales_Details a  where a.Sales_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Sales_No", con)
                prn_DetDt_VAT = New DataTable
                da2.Fill(prn_DetDt_VAT)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        '----------------------GST GST GST-----------------------------------------------

        'Try

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code," &
                                                   "Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Sales_Head a LEFT OUTER JOIN " &
                                                   " Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo " &
                                                   " INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh " &
                                                   " ON c.Company_State_IdNo = csh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) &
                                                   " and a.Sales_Code = '" & Trim(NewCode) & "'", con)
                prn_HdDt = New DataTable
                da1.Fill(prn_HdDt)

                If prn_HdDt.Rows.Count > 0 Then


            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name, b.Item_Name_tamil, c.Unit_Name,OP.Order_No as OrdNo,col.Colour_Name,com.Component_Name,u.Unit_Name  from Sales_Details a " &
                                                       " INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on a.unit_idno = c.unit_idno " &
                                                       "  LEFT OUTER JOIN Colour_Head col on a.colour_idno = col.Colour_IdNo LEFT OUTER JOIN Component_Head com on a.Component_IdNo = com.Component_IdNo " &
                                                       " LEFT OUTER JOIN Order_Program_Head OP on " &
                                                       " a.OrderCode_forSelection = OP.OrderCode_forSelection left outer join unit_head u on a.Unit_IdNo = u.Unit_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And a.Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    If prn_DetDt.Rows.Count > 0 Then

                        prn_DetMxIndx = 0

                        For I = 0 To prn_DetDt.Rows.Count - 1

                    'ItmNm = Trim(prn_DetDt.Rows(I).Item("OrderCode_forSelection").ToString) + "(" + Trim(prn_DetDt.Rows(I).Item("Details_Design").ToString) + ")"
                    'ItmNm1 = Trim(prn_DetDt.Rows(I).Item("OrderCode_forSelection").ToString) + "(" + Trim(prn_DetDt.Rows(I).Item("Details_Design").ToString) + ")"

                    ItmNm = Trim(prn_DetDt.Rows(I).Item("Details_Design").ToString) + " " + Trim(prn_DetDt.Rows(I).Item("Colour_Name").ToString) + " " + Trim(prn_DetDt.Rows(I).Item("Component_Name").ToString)
                    ItmNm1 = Trim(prn_DetDt.Rows(I).Item("Details_Design").ToString) + " " + Trim(prn_DetDt.Rows(I).Item("Colour_Name").ToString) + " " + Trim(prn_DetDt.Rows(I).Item("Component_Name").ToString)

                    If rdo_OrderNoOnDetails.Checked Then
                                If Not IsDBNull(prn_DetDt.Rows(I).Item("OrdNo")) Then
                                    If Len(Trim(prn_DetDt.Rows(I).Item("OrdNo"))) > 0 Then
                                        OrdNo = "Order No : " & Trim(prn_DetDt.Rows(I).Item("OrdNo"))
                                    End If
                                End If
                            Else
                                ItmNm2 = ""
                                If Len(ItmNm1) > MxDetLen Then
                                    For K = MxDetLen To 1 Step -1
                                        If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                                    Next K
                                    If K = 0 Then K = MxDetLen
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K - 1)
                                End If
                            End If



                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(I) + 1)
                            If Len(Trim(OrdNo)) > 0 Then
                                prn_DetAr(prn_DetMxIndx, 2) = Trim(OrdNo)
                            Else
                                prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm1)
                            End If

                    prn_DetAr(prn_DetMxIndx, 3) = prn_DetDt.Rows(I).Item("HSN_Code").ToString
                    prn_DetAr(prn_DetMxIndx, 4) = (prn_DetDt.Rows(I).Item("Dc_No").ToString)
                    prn_DetAr(prn_DetMxIndx, 5) = Val(prn_DetDt.Rows(I).Item("Noof_Items").ToString)
                    prn_DetAr(prn_DetMxIndx, 6) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Rate_1000Stitches").ToString), "########0.00"))
                    prn_DetAr(prn_DetMxIndx, 7) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Rate").ToString), "########0.00"))
                    prn_DetAr(prn_DetMxIndx, 8) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Quantity").ToString), "########0"))
                    prn_DetAr(prn_DetMxIndx, 9) = Trim(Format(Val(prn_DetDt.Rows(I).Item("Amount").ToString), "########0.00"))
                    prn_DetAr(prn_DetMxIndx, 10) = ""
                    prn_DetAr(prn_DetMxIndx, 11) = prn_DetAr(prn_DetMxIndx, 2)

                    'prn_DetAr(prn_DetMxIndx, 12) = "PCS"
                    'If Not IsDBNull(prn_DetDt.Rows(I).Item("Unit_IdNo")) Then
                    '    If prn_DetDt.Rows(I).Item("Unit_IdNo") > 0 Then
                    '        prn_DetAr(prn_DetMxIndx, 12) = Microsoft.VisualBasic.Left(Common_Procedures.Unit_IdNoToName(con, prn_DetDt.Rows(I).Item("Unit_IdNo")), 3)
                    '    End If
                    'End If

                    Dim UNIT As String = "PCS"

                    If Not IsDBNull(prn_DetDt.Rows(I).Item("Unit_IdNo")) Then
                        If prn_DetDt.Rows(I).Item("Unit_IdNo") > 0 Then
                            UNIT = Microsoft.VisualBasic.Left(Common_Procedures.Unit_IdNoToName(con, prn_DetDt.Rows(I).Item("Unit_IdNo")), 3)
                        End If
                    End If

                    prn_DetAr(prn_DetMxIndx, 8) = prn_DetAr(prn_DetMxIndx, 8) + " " + UNIT

                    If Len(Trim(OrdNo)) > 0 Then
                                ItmNm2 = ItmNm
                                GoTo a
                            Else
                                If Len(ItmNm1) > MxDetLen Then
                                    For K = MxDetLen To 1 Step -1
                                        If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                                    Next K
                                    If K = 0 Then K = MxDetLen
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K - 1)

                                    If Trim(ItmNm1) <> "" Then
                                        prn_DetMxIndx = prn_DetMxIndx + 1
                                        prn_DetAr(prn_DetMxIndx, 1) = ""
                                        prn_DetAr(prn_DetMxIndx, 2) = Trim(ItmNm1)
                                        prn_DetAr(prn_DetMxIndx, 3) = ""
                                        prn_DetAr(prn_DetMxIndx, 4) = ""
                                        prn_DetAr(prn_DetMxIndx, 5) = ""
                                        prn_DetAr(prn_DetMxIndx, 6) = ""
                                        prn_DetAr(prn_DetMxIndx, 7) = ""
                                        prn_DetAr(prn_DetMxIndx, 8) = ""
                                        prn_DetAr(prn_DetMxIndx, 9) = ""
                                        prn_DetAr(prn_DetMxIndx, 10) = "ITEM_2ND_LINE"
                                        prn_DetAr(prn_DetMxIndx, 11) = Trim(ItmNm1)
                                    End If


                                End If
                            End If



a:

                            If Trim(ItmNm2) <> "" Then

                                Erase BlNoAr
                                BlNoAr = New String(20) {}

                                m1 = 0
                                bln = Trim(ItmNm2)

LOOP1:
                                If Len(bln) > MxDetLen Then
                                    For K = MxDetLen To 1 Step -1
                                        If Mid$(bln, K, 1) = " " Or Mid$(bln, K, 1) = "," Or Mid$(bln, K, 1) = "/" Or Mid$(bln, K, 1) = "\" Or Mid$(bln, K, 1) = "-" Or Mid$(bln, K, 1) = "." Or Mid$(bln, K, 1) = "&" Or Mid$(bln, K, 1) = "_" Then Exit For
                                    Next K
                                    If K = 0 Then K = MxDetLen
                                    m1 = m1 + 1
                                    BlNoAr(m1) = Microsoft.VisualBasic.Left(Trim(bln), K)
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
                                    prn_DetAr(prn_DetMxIndx, 7) = ""
                                    prn_DetAr(prn_DetMxIndx, 8) = ""
                                    prn_DetAr(prn_DetMxIndx, 9) = ""
                                    prn_DetAr(prn_DetMxIndx, 10) = "SERIALNO"
                                    prn_DetAr(prn_DetMxIndx, 11) = Trim(BlNoAr(K))
                                Next K

                            End If

                        Next I

                    End If

                Else

                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally

        '    da1.Dispose()
        '    da2.Dispose()

        'End Try

        '--------------------------------------------------------------------------------------------------

        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt_VAT.Rows.Count <= 0 Then Exit Sub

        Dim Cmp_Nam As String = ""

        Cmp_Nam = Trim(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_Name", "Company_IdNo =" & Val(Common_Procedures.CompIdNo)))

        Printing_Format2_GST(e)

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
            .Left = 23
            .Right = 62
            .Top = 206
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

        NoofItems_PerPage = 17

        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 70 : ClArr(3) = 300 : ClArr(4) = 110 : ClArr(5) = 80
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt_VAT.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt_VAT.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt_VAT.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNmDesc = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_IdNO").ToString))
                        If (prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "" Then
                            ItmNmDesc = Trim(ItmNmDesc) & "  -  " & prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_Description").ToString
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
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Dc_No").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(0)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 35, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

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


        LnAr(1) = CurY

        '---TOP 250
        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("INVOICE DATE   : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
        S2 = e.Graphics.MeasureString("ORDER.NO & DATE :    ", pFont).Width

        CurY = CurY - 20
        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PAN NO : " & Trim(prn_HdDt_VAT.Rows(0).Item("Company_PanNo").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + 25
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try


            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt_VAT.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_VAT.Rows(0).Item("Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt_VAT.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            OrdNoDt = prn_HdDt_VAT.Rows(0).Item("Order_No").ToString
            'If Trim(prn_HdDt_VAT.Rows(0).Item("Party_Ref_No").ToString) <> "" Then
            '    OrdNoDt = Trim(OrdNoDt) & "  Dt : " & Trim(prn_HdDt_VAT.Rows(0).Item("Party_Ref_No").ToString)
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(OrdNoDt) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(OrdNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If


            DcNoDt = prn_HdDt_VAT.Rows(0).Item("Dc_No").ToString
            'If Trim(prn_HdDt_VAT.Rows(0).Item("Dc_date").ToString) <> "" Then
            '    DcNoDt = Trim(DcNoDt) & "  Dt : " & Trim(prn_HdDt_VAT.Rows(0).Item("Dc_date").ToString)
            'End If

            CurY = CurY + TxtHgt
            If prn_HdDt_VAT.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "TIN NO: " & prn_HdDt_VAT.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            'If Trim(DcNoDt) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(DcNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            If prn_HdDt_VAT.Rows(0).Item("Ledger_PanNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PAN NO: " & prn_HdDt_VAT.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Oty", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rate", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
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
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt_VAT.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, " " & (prn_HdDt_VAT.Rows(0).Item("gROSS_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt_VAT.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt_VAT.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

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


            CurY = CurY + TxtHgt + 1
            If Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt_VAT.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt_VAT.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3
            If Val(prn_HdDt_VAT.Rows(0).Item("Freight_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3
            If Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then

                If is_LastPage = True Then

                    If Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                End If
            End If

            If Val(prn_HdDt_VAT.Rows(0).Item("Form_H_Status").ToString) <> 0 Then
                CurY = CurY + TxtHgt + 3
                Common_Procedures.Print_To_PrintDocument(e, "Against Form-H", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(0.0), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt_VAT.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Round_Off").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt_VAT.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt_VAT.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            StrConv(BmsInWrds, vbProperCase)

            Common_Procedures.Print_To_PrintDocument(e, "Rupees    : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt_VAT.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(2), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(2), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Itm_Nm As String = ""
        Dim Itm_Id As Integer = 0
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
        Itm_Nm = Common_Procedures.Item_IdNoToName(con, 1)
        Itm_Id = "1"
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*,b.*, c.*,e.*, f.Noof_Items as Ent_Sales_Quantity, f.Rate as Ent_Rate, f.Sales_Detail_SlNo as Ent_Sales_SlNo,g.*  from Sales_Delivery_Details a INNER JOIN Sales_Delivery_Head b ON a.Sales_Delivery_Code = b.Sales_Delivery_Code LEFT OUTER JOIN Item_Head c ON " & Val(Itm_Id) & "  = c.Item_IdNo INNER JOIN Order_Program_Head e ON a.Order_Code = e.Order_Program_Code LEFT OUTER JOIN ItemGroup_Head g ON c.ItemGroup_IdNo = G.ItemGroup_IdNo  LEFT OUTER JOIN Sales_Details F ON f.Sales_Code = '" & Trim(NewCode) & "' and f.Entry_Type = '" & Trim(cbo_EntType.Text) & "' and a.Sales_Delivery_Code = f.Sales_Delivery_Code and a.Sales_Delivery_Detail_SlNo = f.Sales_Delivery_Detail_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.Quantity  - a.Receipt_Quantity ) > 0 or f.Noof_Items > 0 ) Order by a.For_OrderBy, a.Sales_Delivery_No, a.Sales_Delivery_Detail_SlNo", con)
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
                    .Rows(n).Cells(3).Value = Itm_Nm
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Item_Description").ToString
                    .Rows(n).Cells(5).Value = Common_Procedures.Colour_IdNoToName(con, Val(Dt1.Rows(i).Item("Colour_IdNo").ToString))
                    .Rows(n).Cells(6).Value = Common_Procedures.Size_IdNoToName(con, Val(Dt1.Rows(i).Item("Size_IdNo").ToString))
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("StchsPr_Pcs")) * Val(Dt1.Rows(i).Item("Pieces").ToString))
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Stiches").ToString), "###########0.00")
                    .Rows(n).Cells(9).Value = (Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Ent_Qty)

                    .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "########0.00")
                    .Rows(n).Cells(11).Value = Format((Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Ent_Qty) * Val(Dt1.Rows(i).Item("Amount").ToString), "########0.00")
                    If Val(Ent_Qty) > 0 Then
                        .Rows(n).Cells(12).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(12).Value = ""
                    End If
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Sales_Delivery_Code").ToString
                    .Rows(n).Cells(14).Value = Val(Dt1.Rows(i).Item("Sales_Delivery_Detail_SlNo").ToString)
                    .Rows(n).Cells(15).Value = Val(Ent_Qty)
                    .Rows(n).Cells(16).Value = Val(Ent_Rate)
                    '
                    .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Item_HSN_Code").ToString
                    .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Item_GST_Percentage").ToString
                    '.Rows(n).Cells(17).Value = Common_Procedures.Transport_IdNoToName(con, Val(Dt1.Rows(i).Item("Transport_IdNo").ToString))
                    .Rows(n).Cells(19).Value = (Dt1.Rows(i).Item("Order_No").ToString)
                    .Rows(n).Cells(20).Value = (Dt1.Rows(i).Item("Party_Ref_No").ToString)
                    .Rows(n).Cells(21).Value = (Dt1.Rows(i).Item("Ordercode_forSelection").ToString)


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

                .Rows(RwIndx).Cells(12).Value = (Val(.Rows(RwIndx).Cells(12).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(12).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(12).Value = ""

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

            If Val(dgv_Selection.Rows(i).Cells(12).Value) = 1 Then

                If Val(dgv_Selection.Rows(i).Cells(15).Value) <> 0 Then
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(15).Value)

                Else
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(9).Value)

                End If

                If Val(dgv_Selection.Rows(i).Cells(16).Value) <> 0 Then
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(16).Value)

                Else
                    Ent_Rate = Val(dgv_Selection.Rows(i).Cells(10).Value)

                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(6).Value = Val(Ent_Rate)
                dgv_Details.Rows(n).Cells(7).Value = Val(Ent_Qty)
                dgv_Details.Rows(n).Cells(8).Value = Format(Val(Ent_Qty) * Val(Ent_Rate), "##########0.00")
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(13).Value
                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(14).Value

                dgv_Details.Rows(n).Cells(15).Value = dgv_Selection.Rows(i).Cells(17).Value
                dgv_Details.Rows(n).Cells(16).Value = dgv_Selection.Rows(i).Cells(17).Value
                dgv_Details.Rows(n).Cells(17).Value = dgv_Selection.Rows(i).Cells(4).Value
                ' 
                dgv_Details.Rows(n).Cells(19).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(20).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(21).Value = dgv_Selection.Rows(i).Cells(21).Value
                txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(19).Value
                txt_PartyRefNo.Text = dgv_Selection.Rows(i).Cells(20).Value
            End If

        Next i

        NoCalc_Status = False



        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        'txt_BillNo.Focus()
        'cbo_EntType.Enabled = False
        If cbo_PaymentMethod.Enabled And cbo_PaymentMethod.Visible Then cbo_PaymentMethod.Focus()

    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_EntType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EntType.TextChanged
        If Trim(UCase(cbo_EntType.Text)) = "DIRECT" Then
            pnl_InputDetails.Enabled = True
            dgv_Details.EditMode = DataGridViewEditMode.EditProgrammatically
            dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            txt_PartyRefNo.Enabled = True
            txt_OrderNo.Enabled = True
            txt_DcDate.Enabled = True
            txt_Dcno.Enabled = True
            'cbo_Transport.Enabled = True
            'txt_VechileNo.Enabled = True

        Else

            pnl_InputDetails.Enabled = False
            dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
            dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
            txt_PartyRefNo.Enabled = False
            txt_OrderNo.Enabled = False
            txt_DcDate.Enabled = False
            txt_Dcno.Enabled = False
            'cbo_Transport.Enabled = False
            'txt_VechileNo.Enabled = False

        End If
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = Nothing
        If dgv_Details.CurrentCell.ColumnIndex = 6 Or dgv_Details.CurrentCell.ColumnIndex = 7 Then
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
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(7)


                        End If


                    ElseIf .CurrentCell.ColumnIndex < 6 Then
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= 6 Then
                        If .CurrentCell.RowIndex = 0 Then
                            If pnl_InputDetails.Enabled = True And cbo_ItemName.Enabled = True Then
                                cbo_ItemName.Focus()

                            Else
                                cbo_Ledger.Focus()

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(8)

                        End If

                    ElseIf .CurrentCell.ColumnIndex > 7 Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(8)

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

    'Private Sub txt_VechileNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.KeyCode = 38 Then
    '        cbo_Transport.Focus()
    '    End If
    '    If e.KeyCode = 40 Then
    '        cbo_TransportMode.Focus()
    '    End If
    'End Sub


    'Private Sub txt_VechileNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If Asc(e.KeyChar) = 13 Then
    '        cbo_TransportMode.Focus()
    '    End If
    'End Sub


    Private Sub txt_Description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
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
    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        cbo_TaxType.Tag = cbo_TaxType.Text
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_ItemName, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_TaxType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_CashDiscPerc.Focus()
                End If

            Else
                If pnl_InputDetails.Enabled = True And cbo_ItemName.Enabled And cbo_ItemName.Visible Then
                    cbo_ItemName.Focus()
                Else
                    txt_CashDiscPerc.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, Nothing, "", "", "", "", True)
            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                    If dgv_Details.RowCount > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                        dgv_Details.CurrentCell.Selected = True
                    Else
                        txt_CashDiscPerc.Focus()
                    End If

                Else
                    If pnl_InputDetails.Enabled = True And cbo_ItemName.Enabled And cbo_ItemName.Visible Then
                        cbo_ItemName.Focus()
                    Else
                        txt_CashDiscPerc.Focus()
                    End If

                End If
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
                get_Item_Tax(True)
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
    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentMethod, Nothing, cbo_OrderCode, "", "", "", "")
            If (e.KeyValue = 38 And cbo_PaymentMethod.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If txt_PartyRefNo.Enabled Then
                    txt_PartyRefNo.Focus()

                Else
                    cbo_Ledger.Focus()

                End If

            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentMethod, cbo_OrderCode, "", "", "", "")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Try
    '        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportMode, txt_VechileNo, txt_DateTime_Of_Supply, "", "", "", "")
    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Try
    '        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, txt_DateTime_Of_Supply, "", "", "", "")
    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
    '    End Try
    'End Sub
    Private Sub cbo_PaymentMethod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.LostFocus
        If Trim(UCase(cbo_PaymentMethod.Text)) = "CASH" And Trim(cbo_Ledger.Text) = "" Then
            cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
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

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font, pFont1 As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNmDesc As String = ""
        Dim ItmDescAr(20) As String
        Dim CurX As Single = 0
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
            .Left = 0
            .Right = 0
            .Top = 0
            .Bottom = 0
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

        NoofItems_PerPage = 16

        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 70 : ClArr(3) = 300 : ClArr(4) = 110 : ClArr(5) = 80
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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


        Try

            If prn_HdDt_VAT.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = TMargin + 355 ' 365

                If prn_DetDt_VAT.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt_VAT.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + 340, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNmDesc = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_IdNO").ToString))
                        If (prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "" Then
                            ItmNmDesc = Trim(ItmNmDesc) & "  -  " & prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_Description").ToString
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
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 30, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, IIf(Trim(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Dc_No").ToString) = "0", "", Trim(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Dc_No").ToString)), LMargin + 65, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(0)), LMargin + 145, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Noof_Items").ToString), LMargin + 540, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Unit_IdNO").ToString)), LMargin + 580, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + 650, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), LMargin + 755, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1


                        For k = 1 To m1
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(k)), LMargin + 145, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        Next k

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim i As Integer = 0
        Dim strHeight As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim C1 As Single, W1 As Single, S1 As Single, S2 As Single
        Dim CurX As Single = 0
        Dim OrdNoDt As String = ""
        Dim DcNoDt As String = ""
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String, Led_PanNo As String
        Dim LedAr(10) As String
        Dim Indx As Integer = 0


        Try

            PageNo = PageNo + 1

            CurY = TMargin

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sales_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(EntryCode) & "'  Order by a.For_OrderBy", con)
            da2.Fill(dt2)
            If dt2.Rows.Count > NoofItems_PerPage Then
                Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
            dt2.Clear()

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = ""

            Led_Name = Trim(prn_HdDt_VAT.Rows(0).Item("Ledger_Name").ToString)
            Led_Add1 = Trim(prn_HdDt_VAT.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt_VAT.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt_VAT.Rows(0).Item("Ledger_Address3").ToString)
            Led_Add4 = Trim(prn_HdDt_VAT.Rows(0).Item("Ledger_Address4").ToString)
            Led_TinNo = Trim(prn_HdDt_VAT.Rows(0).Item("Ledger_TinNo").ToString)
            Led_PanNo = Trim(prn_HdDt_VAT.Rows(0).Item("Ledger_PanNo").ToString)

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
                LedAr(Indx) = "TIN NO : " & Trim(Led_TinNo)
            End If
            If Trim(Led_PanNo) <> "" Then
                Indx = Indx + 1
                LedAr(Indx) = "PAN NO: " & Trim(Led_TinNo)
            End If



            CurY = 180
            If Trim(prn_HdDt_VAT.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PAN NO : " & Trim(prn_HdDt_VAT.Rows(0).Item("Company_PanNo").ToString), LMargin + 590, CurY, 0, 0, pFont)
            End If

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            CurY = TMargin + 210
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_VAT.Rows(0).Item("Sales_No").ToString, LMargin + 560, CurY, 0, 0, p1Font)

            CurY = TMargin + 240
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt_VAT.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + 560, CurY, 0, 0, pFont)

            If Trim(OrdNoDt) <> "" Then
                CurY = TMargin + 265
                Common_Procedures.Print_To_PrintDocument(e, Trim(OrdNoDt), LMargin + 560, CurY, 0, 0, pFont)
            End If


            CurY = TMargin + 220
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, LedAr(1), LMargin + 90, CurY, 0, 0, p1Font)
            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(2), LMargin + 90, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(3), LMargin + 90, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(4), LMargin + 90, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(5), LMargin + 90, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, LedAr(6), LMargin + 90, CurY, 0, 0, pFont)



        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
        Dim Rps As String = ""
        Dim Rps1 As String = ""
        W1 = e.Graphics.MeasureString("Payment Terms : ", pFont).Width



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try


            'For I = NoofDets + 1 To NoofItems_PerPage

            '    CurY = CurY + TxtHgt

            '    prn_DetIndx = prn_DetIndx + 1

            'Next

            If is_LastPage = True Then

                p1Font = New Font("Calibri", 12, FontStyle.Bold)

                CurY = TMargin + 915 '920
                Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt_VAT.Rows(0).Item("Total_Qty").ToString), LMargin + 530, CurY, 1, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, " " & (prn_HdDt_VAT.Rows(0).Item("Gross_Amount").ToString), LMargin + 755, CurY, 1, 0, p1Font)

                If Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                    CurY = TMargin + 940 '945
                    ' Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt_VAT.Rows(0).Item("Tax_Perc").ToString), LMargin + 550, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString), LMargin + 755, CurY, 1, 0, p1Font)

                End If

                If Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Perc").ToString) <> 0 Then

                    CurY = TMargin + 965 '970
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Dis @ " & Trim(Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + 470, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + 755, CurY, 1, 0, p1Font)

                End If

                If Val(prn_HdDt_VAT.Rows(0).Item("Form_H_Status").ToString) <> 0 Then
                    CurY = TMargin + 970
                    Common_Procedures.Print_To_PrintDocument(e, "Against Form-H", 470, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "0.00", LMargin + 755, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt_VAT.Rows(0).Item("Round_Off").ToString) <> 0 Then
                    CurY = TMargin + 990 '995
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + 755, CurY, 1, 0, p1Font)

                End If

                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt_VAT.Rows(0).Item("Net_Amount").ToString))
                BmsInWrds = Replace(Trim(BmsInWrds), "", "")

                Dim k As Integer, m1 As Integer

                If Len(BmsInWrds) > 80 Then
                    For k = 80 To 1 Step -1
                        If Mid$(BmsInWrds, k, 1) = " " Or Mid$(BmsInWrds, k, 1) = "," Or Mid$(BmsInWrds, k, 1) = "/" Or Mid$(BmsInWrds, k, 1) = "\" Or Mid$(BmsInWrds, k, 1) = "-" Or Mid$(BmsInWrds, k, 1) = "." Or Mid$(BmsInWrds, k, 1) = "&" Or Mid$(BmsInWrds, k, 1) = "_" Then Exit For
                    Next k
                    If k = 0 Then k = 80
                    m1 = m1 + 1
                    Rps = Microsoft.VisualBasic.Left(Trim(BmsInWrds), k)
                    BmsInWrds = Microsoft.VisualBasic.Right(BmsInWrds, Len(BmsInWrds) - k)
                End If

                StrConv(BmsInWrds, vbProperCase)
                CurY = TMargin + 970
                Common_Procedures.Print_To_PrintDocument(e, Rps & " ", LMargin + 80, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BmsInWrds & " ", LMargin + 80, CurY, 0, 0, p1Font)

                CurY = TMargin + 1020

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt_VAT.Rows(0).Item("Net_Amount").ToString)), LMargin + 750, CurY, 1, 0, p1Font)

            End If


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

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

        NoofItems_PerPage = 17 '19
        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
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

            If prn_HdDt_VAT.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt_VAT.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt_VAT.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNmDesc = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_IdNO").ToString))

                        If (prn_DetDt_VAT.Rows(prn_DetIndx).Item("Dc_No").ToString) <> "" Then
                            ItmNmDesc = Trim(ItmNmDesc) & "  -  " & prn_DetDt_VAT.Rows(prn_DetIndx).Item("Dc_No").ToString
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
                        NoofDets = NoofDets + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_Description").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1

                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(0)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1


                        For k = 1 To m1
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(k)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        Next k

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt
                    Loop

                End If

                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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
        Cmp_Name = prn_HdDt_VAT.Rows(0).Item("Company_Name").ToString
        Cmp_ShrtName = prn_HdDt_VAT.Rows(0).Item("Company_ShortName").ToString

        Cmp_Add1 = prn_HdDt_VAT.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt_VAT.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt_VAT.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt_VAT.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt_VAT.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt_VAT.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt_VAT.Rows(0).Item("Company_CstNo").ToString
        End If

        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt_VAT.Rows(0).Item("Company_EMail").ToString)
        End If

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

        ItmNm1 = Trim(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString)
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

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("INVOICE DATE   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            S2 = e.Graphics.MeasureString("ORDER.NO & DATE :    ", pFont).Width

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt_VAT.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_VAT.Rows(0).Item("Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt_VAT.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            OrdNoDt = prn_HdDt_VAT.Rows(0).Item("Order_No").ToString
            'If Trim(prn_HdDt_VAT.Rows(0).Item("Party_Ref_No").ToString) <> "" Then
            '    OrdNoDt = Trim(OrdNoDt) & "  Dt : " & Trim(prn_HdDt_VAT.Rows(0).Item("Party_Ref_No").ToString)
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(OrdNoDt) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(OrdNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If


            DcNoDt = prn_HdDt_VAT.Rows(0).Item("Dc_No").ToString
            'If Trim(prn_HdDt_VAT.Rows(0).Item("Dc_date").ToString) <> "" Then
            '    DcNoDt = Trim(DcNoDt) & "  Dt : " & Trim(prn_HdDt_VAT.Rows(0).Item("Dc_date").ToString)
            'End If

            CurY = CurY + TxtHgt
            If prn_HdDt_VAT.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt_VAT.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
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

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

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

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)

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
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt_VAT.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, " " & (prn_HdDt_VAT.Rows(0).Item("gROSS_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
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

            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt_VAT.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt_VAT.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

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


            CurY = CurY + TxtHgt + 1
            If Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "VAT @ " & Trim(Val(prn_HdDt_VAT.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3
            If Val(prn_HdDt_VAT.Rows(0).Item("Freight_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3
            If Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then

                If is_LastPage = True Then

                    If Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                End If
            End If

            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt_VAT.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Round_Off").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            If prn_HdDt_VAT.Rows(0).Item("Vehicle_No").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Through : " & prn_HdDt_VAT.Rows(0).Item("Vehicle_No").ToString, LMargin + 10, CurY, 0, 0, pFont)
            End If

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt_VAT.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt_VAT.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            StrConv(BmsInWrds, vbProperCase)

            Common_Procedures.Print_To_PrintDocument(e, "Rupees    : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 5
            Cmp_Name = prn_HdDt_VAT.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    'Private Sub txt_Electronic_RefNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.KeyValue = 38 Then
    '        cbo_PaymentMethod.Focus()
    '    ElseIf e.KeyValue = 40 Then
    '        cbo_Transport.Focus()
    '    End If
    'End Sub

    'Private Sub txt_Electronic_RefNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If Asc(e.KeyChar) = 13 Then
    '        cbo_Transport.Focus()
    '    End If
    'End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus



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

            Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
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


    Private Sub Printing_Format_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        TxtHgt = 18.5 ' 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 20 ' 17 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 30 : ClArr(2) = 200 : ClArr(3) = 50 : ClArr(4) = 60 : ClArr(5) = 55 : ClArr(6) = 70 : ClArr(7) = 70 : ClArr(8) = 70
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                'If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1


                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 7
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                Printing_Format_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try
                    NoofDets = 0

                    If Val(DetIndx) > 18 Then
                        CurY = CurY + TxtHgt
                    End If

                    If Trim(Common_Procedures.settings.CustomerCode) = "1201" Then
                        ' e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SLT_GreyLogo, Drawing.Image), LMargin + 220, CurY + 70, 290, 290)

                    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
                        e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SLT_GreyLogo, Drawing.Image), LMargin + 220, CurY + 70, 290, 290)
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
                                Printing_Format_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt

                            'If DetIndx <> 1 And Val(prn_DetAr(DetIndx, 1)) <> 0 Then
                            '    CurY = CurY + 2
                            'End If

                            If Trim(prn_DetAr(DetIndx, 11)) <> "" And Trim(prn_DetAr(DetIndx, 10)) = "SERIALNO" Then
                                CurY = CurY - 3
                                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 25, CurY, 0, 0, p1Font)

                            ElseIf Trim(prn_DetAr(DetIndx, 11)) <> "" And Trim(prn_DetAr(DetIndx, 10)) = "ITEM_2ND_LINE" Then
                                CurY = CurY - 3
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 25, CurY, 0, 0, pFont)

                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 4), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 9), PageWidth - 10, CurY, 1, 0, pFont)
                            End If

                            NoofDets = NoofDets + 1

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)
                    prn_Count = prn_Count + 1
                    'If Trim(prn_InpOpts) <> "" Then
                    '    If prn_Count < Len(Trim(prn_InpOpts)) Then

                    '        DetIndx = 1
                    '        prn_PageNo = 0

                    '        e.HasMorePages = True
                    '        Return
                    '    End If
                    'End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        ' prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        'prn_OriDupTri = ""
        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count <= Len(Trim(prn_InpOpts)) Then

        '        S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

        '        If Val(S) = 1 Then
        '            prn_OriDupTri = "ORIGINAL"
        '            PrintDocument1.DefaultPageSettings.Color = True
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
        '            e.PageSettings.Color = True
        '        ElseIf Val(S) = 2 Then
        '            prn_OriDupTri = "DUPLICATE"
        '        ElseIf Val(S) = 3 Then
        '            prn_OriDupTri = "TRIPLICATE"
        '        End If

        '    End If
        'End If

        'If Trim(prn_OriDupTri) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If

        p1Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "JOBWORK TAX INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
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

        If Trim(Common_Procedures.settings.CustomerCode) = "1201--" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SWASTHICK_LOGO, Drawing.Image), LMargin + 24, CurY + 10, 100, 100)

        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SLT_Logo, Drawing.Image), LMargin + 24, CurY + 10, 100, 100)

        End If

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("President", 25, FontStyle.Bold)
        pFont = New Font("Calibri", 10, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Green)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Dim br = New SolidBrush(Color.FromArgb(191, 43, 133))

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, br)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, br)

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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, br)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, br)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, br)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, br)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, br)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        pFont = New Font("Calibri", 10, FontStyle.Regular)

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
                If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
                If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))

            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

                Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)

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


            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

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


            '------------------- Invoice No Block

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt


            BlockInvNoY = BlockInvNoY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            BlockInvNoY = BlockInvNoY + TxtHgt
            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Order No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            End If
            'If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Electronic Ref.No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            'End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY + 5, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY + 5, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClAr(1) + ClAr(2), CurY + 5, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 5, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "No.of", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rate/100", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 5, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + 5, 2, ClAr(9), pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Stitches", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Stitches", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim vTaxPerc As Single = 0
        Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
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


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    If vTaxPerc <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

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

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 4, CurY, 1, 0, p1Font)
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

            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            ''=============GST SUMMARY============
            'vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'If vNoofHsnCodes <> 0 Then
            '    Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            'End If
            ''==========================


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font, Brushes.Green)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Payment must be produce 10 days from our bill date.", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Normal embroidery mistake allowance 1 % and applique", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "embroidery mistake 3 % would be allowed", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, Brushes.Green)

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

    Private Sub Printing_Format2_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single, CurY1 As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim Dc_No(10) As String
        Dim Rw As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins

            .Left = 30
            .Right = 50
            .Top = 25 ' 65
            .Bottom = 40

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
        TxtHgt = 18.5 ' 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        If prn_HdDt.Rows(0).Item("Tax_Type") = "GST" Then
            NoofItems_PerPage = 12 ' 17 
        Else
            NoofItems_PerPage = 22
        End If

        Erase LnAr

        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0

        ClArr(1) = 25 : ClArr(2) = 350 : ClArr(3) = 110 : ClArr(4) = 60 : ClArr(5) = 110
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If prn_HdDt.Rows.Count > 0 Then

            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

            If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)

            Printing_Format2_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            If Trim(Common_Procedures.settings.CustomerCode) = "1201" Then

                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.swasthick_Greylogo, Drawing.Image), LMargin + 240, CurY + 70, 290, 290)

            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1117" Then

                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SLT_GreyLogo, Drawing.Image), LMargin + 220, CurY + 70, 290, 290)

            End If

            If prn_PageNo <= 1 And prn_Count <= 1 Then
                CurY = CurY - TxtHgt - 10
            Else
                CurY = CurY - 16
            End If

            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            If prn_DetMxIndx > 0 Then

                Do While DetIndx <= prn_DetMxIndx

                    If NoofDets > NoofItems_PerPage Then

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        NoofDets = NoofDets + 1
                        Printing_Format2_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                        e.HasMorePages = True

                        Return

                    End If

                    If DetIndx > 0 Then
                        If IsDBNull(prn_DetDt_VAT.Rows(CurrPrintingRow).Item("Design_Picture")) = False Then

                            If prn_DetAr(DetIndx, 2) <> prn_DetAr(DetIndx - 1, 2) Then
                                If (NoofDets + 4) > NoofItems_PerPage Then
                                    For k = NoofDets To NoofItems_PerPage
                                        CurY = CurY + TxtHgt
                                    Next
                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                    NoofDets = NoofItems_PerPage
                                    Printing_Format2_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                        Else

                            If prn_DetAr(DetIndx, 2) <> prn_DetAr(DetIndx - 1, 2) Then
                                If (NoofDets + 2) > NoofItems_PerPage Then
                                    For k = NoofDets To NoofItems_PerPage
                                        CurY = CurY + TxtHgt
                                    Next
                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                    NoofDets = NoofItems_PerPage
                                    Printing_Format2_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                    e.HasMorePages = True

                                    Return

                                End If
                            End If
                        End If
                    End If

                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + ClArr(1) - 5, CurY, 1, ClArr(1), p1Font)


                    If Trim(prn_DetAr(DetIndx, 11)) <> "" And Trim(prn_DetAr(DetIndx, 10)) = "SERIALNO" Then

                        CurY = CurY - 3
                        p1Font = New Font("Calibri", 8, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 5, CurY, 0, 0, p1Font)

                    ElseIf Trim(prn_DetAr(DetIndx, 11)) <> "" And Trim(prn_DetAr(DetIndx, 10)) = "ITEM_2ND_LINE" Then

                        CurY = CurY - 3
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 5, CurY, 0, 0, p1Font)

                    Else

                        ' Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + +ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                        If Len(Trim(prn_DetAr(DetIndx, 11))) > 10 Then

                            If prn_DetAr(DetIndx, 11).Contains("Order No : ") Then
                                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 5, CurY, 0, 0, p1Font)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 5, CurY, 0, 0, p1Font)
                            End If

                        Else

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 5, CurY, 0, 0, p1Font)

                        End If

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 4), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, p1Font)

                        CurY1 = CurY - TxtHgt

                        For Rw = 0 To 10
                            If Trim(prn_DetAr(DetIndx, 4)) <> "" Then
                                If Trim(Dc_No(Rw)) <> "" Then
                                    CurY1 = CurY1 + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Dc_No(Rw)), LMargin + ClArr(1) + ClArr(2) + 5, CurY1, 0, 0, p1Font)
                                End If
                            End If
                        Next

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 9), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, p1Font)

                    End If

                    If DetIndx = 0 Then
                        GoTo SKIPIMAGE1
                    End If

                    If Not DetIndx = prn_DetMxIndx Then
                        If DetIndx < prn_DetMxIndx Then
                            If Len(Trim(prn_DetAr(DetIndx + 1, 1))) = 0 Then
                                GoTo SKIPIMAGE1
                            End If
                        End If
                    End If

                    If prn_DetAr(DetIndx, 2) = prn_DetAr(DetIndx - 1, 2) Then
                        GoTo SKIPIMAGE1
                    End If

                    Dim PIC As Image = Nothing
                    If IsDBNull(prn_DetDt_VAT.Rows(CurrPrintingRow).Item("Design_Picture")) = False Then
                        ' Dim imageData As Byte() = DirectCast(prn_DetDt_VAT.Rows(CurrPrintingRow).Item("Design_Picture"), Byte())
                        Dim imageData As Byte() = DirectCast(prn_DetDt_VAT.Rows(CurrPrintingRow).Item("Design_Picture"), Byte())
                        If Not imageData Is Nothing Then
                            Using ms As New MemoryStream(imageData, 0, imageData.Length)
                                ms.Write(imageData, 0, imageData.Length)
                                If imageData.Length > 0 Then
                                    CurY = CurY + TxtHgt
                                    PIC = Image.FromStream(ms)
                                    e.Graphics.DrawImage(DirectCast(PIC, Drawing.Image), LMargin + ClArr(1) + 5, CurY, 130, 65)
                                    CurY = CurY + TxtHgt + TxtHgt + TxtHgt
                                    NoofDets = NoofDets + 3
                                End If
                            End Using
                        End If
                    End If



SKIPIMAGE1:

                    If CurrPrintingRow < prn_DetDt_VAT.Rows.Count - 1 And DetIndx > 0 Then
                        CurrPrintingRow = CurrPrintingRow + 1
                    End If

                    NoofDets = NoofDets + 1

                    DetIndx = DetIndx + 1

                Loop

            End If

            Printing_Format2_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

            If Trim(prn_InpOpts) <> "" Then
                If prn_Count < Len(Trim(prn_InpOpts)) Then

                    CurrPrintingRow = 0
                    DetIndx = 1
                    prn_PageNo = 0

                    e.HasMorePages = True
                    Return
                End If
            End If

        End If

        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False


    End Sub

    Private Sub Printing_Format2_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        'Try

        Dim da2 As New SqlClient.SqlDataAdapter
            Dim dt2 As New DataTable
            Dim p1Font As Font
            Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
            Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
            Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
            Dim strHeight As Single
            Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
            Dim Led_GSTTinNo As String, Led_State As String, Cmp_PAN As String, Cmp_ESINo As String
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
            Dim ItmNm1 As String, ItmNm2 As String

            PageNo = PageNo + 1

            CurY = TMargin

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)
            If dt2.Rows.Count > NoofItems_PerPage Then
                Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
            dt2.Clear()

            '-------------

            If PageNo = 1 Then

                prn_Count = prn_Count + 1

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
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), LMargin + 3, CurY - TxtHgt, 0, 0, pFont)
                End If

            End If

            '----------

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
            'CurY = CurY + TxtHgt '+ 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
            Cmp_Desc = "" : Cmp_Email = ""
            Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_ESINo = "" : Cmp_PAN = ""

            Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
            If Trim(Cmp_Add1) <> "" Then
                If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) <> "," Then
                    Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
                Else
                    Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
                End If
            Else
                Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If

            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
            If Trim(Cmp_Add2) <> "" Then
                If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) <> "," Then
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

            Cmp_Add2 = Cmp_Add2 + "  " + Cmp_PhNo

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

        Cmp_Add2 = Cmp_Add2 + "  " + Cmp_Email

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
                Cmp_StateCap = "STATE : "
                Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            End If

            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
            End If

            If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
                Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
            End If

            If Trim(prn_HdDt.Rows(0).Item("Company_PANNo").ToString) <> "" Then
                Cmp_PAN = "PAN :" & prn_HdDt.Rows(0).Item("Company_PANNo").ToString
            End If

            If Trim(prn_HdDt.Rows(0).Item("Company_ESINo").ToString) <> "" Then
                Cmp_ESINo = "ESI No :" & prn_HdDt.Rows(0).Item("Company_ESINo").ToString
            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "5010" Then

                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.spclogo, Drawing.Image), LMargin + 24, CurY + 10, 100, 100)

                p1Font = New Font("Arial Narrow", 9, FontStyle.Bold)

                strWidth = e.Graphics.MeasureString("""The Thread Art Studio""", p1Font).Width
                Common_Procedures.Print_To_PrintDocument(e, """The Thread Art Studio""", LMargin + 74 - (strWidth / 2), CurY + 115, 2, strWidth, p1Font, Brushes.Black)

                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SPCISO, Drawing.Image), LMargin + 610, CurY + 10, 130, 80)

                e.Graphics.DrawLine(Pens.Black, LMargin + 605, CurY + 5, LMargin + 605 + 140, CurY + 5)
                e.Graphics.DrawLine(Pens.Black, LMargin + 605, CurY + 5, LMargin + 605, CurY + 5 + 85)
                e.Graphics.DrawLine(Pens.Black, LMargin + 605, CurY + 5 + 85, LMargin + 605 + 140, CurY + 5 + 85)
                e.Graphics.DrawLine(Pens.Black, LMargin + 605 + 140, CurY + 5, LMargin + 605 + 140, CurY + 5 + 85)

            End If

            If Common_Procedures.settings.CustomerCode = "5010" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PAN, PageWidth - 145, CurY + 95, 0, strWidth, p1Font, Brushes.Black)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_ESINo, PageWidth - 145, CurY + 110, 0, strWidth, p1Font, Brushes.Black)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, PageWidth - 145, CurY + 125, 0, strWidth, p1Font, Brushes.Black)
            End If

            p1Font = New Font("Cooper Black", 36, FontStyle.Bold)

            CurY = CurY + TxtHgt - 10

            pFont = New Font("Calibri", 12, FontStyle.Bold)

            Dim cM_br = New SolidBrush(Color.FromArgb(235, 39, 5))
            Dim br = New SolidBrush(Color.FromArgb(0, 0, 111))

            If Common_Procedures.settings.CustomerCode = "5010" Then
                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SP_CREATION_HEADER, Drawing.Image), LMargin + 180, CurY, PageWidth - 400, 50)
            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "5027" Then
                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.FWC_LOGO_1, Drawing.Image), LMargin + 180, CurY, PageWidth - 400, 75)
                CurY = CurY + 60
                Common_Procedures.Print_To_PrintDocument(e, "(EMBROIDERY DIVISION)", LMargin, CurY + 10, 2, PrintWidth - 20, pFont, Brushes.Green)
                CurY = CurY + 25
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, cM_br)
            End If

        If Len(Trim(Cmp_Add1)) > 0 Then
            CurY = CurY + 12
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY - 10, 2, PrintWidth - 20, pFont, Brushes.Green)
        End If

        If Len(Trim(Cmp_Add2)) > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY - 10, 2, PrintWidth - 20, pFont, Brushes.Green)
        End If

        'If Len(Trim(Cmp_Email)) > 0 Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Email), LMargin, CurY - 10, 2, PrintWidth - 20, pFont, Brushes.Green)
        'End If

        If (Common_Procedures.settings.CustomerCode = "5027" Or Common_Procedures.settings.CustomerCode = "5022") And prn_HdDt.Rows(0).Item("Tax_Type") = "GST" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_GSTIN_No) + " , " + Cmp_PAN, LMargin, CurY - 10, 2, PrintWidth - 20, pFont, Brushes.Black)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC : " & prn_DetDt.Rows(0).Item("HSN_CODE").ToString, LMargin, CurY - 10, 2, PrintWidth - 20, pFont, Brushes.Black)
        End If

        CurY = CurY + TxtHgt

            If CurY < LnAr(1) + 140 Then CurY = LnAr(1) + 140

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            LnAr(2) = CurY

            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            pFont = New Font("Calibri", 10, FontStyle.Regular)

            'Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then

                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
                If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
                If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))

            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)
                Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) + " (" + Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString) + ")"
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
            Led_GSTTinNo = Led_GSTTinNo + " STATE : " & Led_State
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

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If


        Cen1 = ClAr(1) + ClAr(2) + ClAr(3) '+ ClAr(4) '+ ClAr(5)
        W1 = e.Graphics.MeasureString("INVOICE No :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width


            BlockInvNoY = CurY + 10
            CurY = CurY + TxtHgt

            Dim AddBlankRows As Integer = 5

            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY - 3, 0, 0, p1Font)

        If Len(Trim(LedNmAr(2))) > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            AddBlankRows = AddBlankRows - 1
        End If

        If Len(Trim(LedNmAr(3))) > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            AddBlankRows = AddBlankRows - 1
        End If

        If Len(Trim(LedNmAr(4))) > 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)
                AddBlankRows = AddBlankRows - 1
            End If

            If Len(Trim(LedNmAr(5))) > 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)
                AddBlankRows = AddBlankRows - 1
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)


        'If Len(Trim(Led_State)) > 0 Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(Led_State), LMargin + W2 + 10, CurY, 0, 0, p1Font)
        '    AddBlankRows = AddBlankRows - 1
        'End If

        If Len(Trim(Led_GSTTinNo)) > 0 And prn_HdDt.Rows(0).Item("Tax_Type") = "GST" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Led_GSTTinNo), LMargin + W2 + 10, CurY, 0, 0, p1Font)
            AddBlankRows = AddBlankRows - 1
        End If

        If AddBlankRows > 0 Then
            CurY = CurY + (AddBlankRows * TxtHgt)
        End If

        '------------------- Invoice No Block

        If Common_Procedures.settings.CustomerCode = "5010" Then
                p1Font = New Font("Times New Roman", 20, FontStyle.Bold)
                strWidth = e.Graphics.MeasureString("LABOUR BILL", p1Font).Width
                Common_Procedures.Print_To_PrintDocument(e, "LABOUR BILL", (LMargin + Cen1 + ((PageWidth - (LMargin + Cen1)) - strWidth) / 2), BlockInvNoY, 2, 0, p1Font)
            ElseIf Common_Procedures.settings.CustomerCode = "5027" Then
                p1Font = New Font("Times New Roman", 20, FontStyle.Bold)
                strWidth = e.Graphics.MeasureString("LABOUR BILL", p1Font).Width
            'Common_Procedures.Print_To_PrintDocument(e, "EMBROIDERY", (LMargin + Cen1 + ((PageWidth - (LMargin + Cen1)) - strWidth) / 2), BlockInvNoY, 2, 0, p1Font)
            'BlockInvNoY = BlockInvNoY + 30
            Common_Procedures.Print_To_PrintDocument(e, "LABOUR BILL", (LMargin + Cen1 + ((PageWidth - (LMargin + Cen1)) - strWidth) / 2), BlockInvNoY, 2, 0, p1Font)
            End If

            BlockInvNoY = BlockInvNoY + 30

            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, BlockInvNoY, PageWidth, BlockInvNoY)

            BlockInvNoY = BlockInvNoY + 5

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 5, BlockInvNoY, 0, 0, pFont)

        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode = "5027" Then
            Dim INVNO As String = prn_HdDt.Rows(0).Item("Sales_No").ToString
            INVNO = INVNO.PadLeft(3, "0")
            Common_Procedures.Print_To_PrintDocument(e, "FWC/EMB/" & INVNO, LMargin + Cen1 + W1 + 10, BlockInvNoY - 3, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, p1Font)
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)

            BlockInvNoY = BlockInvNoY + TxtHgt + 5
            'BlockInvNoY = BlockInvNoY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 5, BlockInvNoY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)

        BlockInvNoY = BlockInvNoY + TxtHgt

            If Not IsDBNull(prn_HdDt.Rows(0).Item("Party_Ref_No")) Then
                If Len(Trim(prn_HdDt.Rows(0).Item("Party_Ref_No"))) > 0 Then
                    BlockInvNoY = BlockInvNoY + TxtHgt - 10
                    Common_Procedures.Print_To_PrintDocument(e, "Ref", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 5, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_Ref_No")), LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                BlockInvNoY = BlockInvNoY + TxtHgt + 10
                End If

            End If

        'BlockInvNoY = BlockInvNoY + TxtHgt - 10
        Dim JobNoLnCnt As Integer = 0

            If rdo_OrderNoInHeader.Checked Then

                Dim MxJobWdt As Integer = PageWidth - (LMargin + Cen1 + 10)
                Dim PrtStr As String = ""

                If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then

                    Common_Procedures.Print_To_PrintDocument(e, "Job No(s) ", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 5, BlockInvNoY, 0, 0, pFont)

                For I = 0 To Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$").GetUpperBound(0)

                        p1Font = New Font("Calibri", 10, FontStyle.Bold)

                        If I > 0 Then
                            For J = 0 To I - 1
                                If Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(I) = Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(J) Then
                                    GoTo A
                                End If
                            Next
                        End If

                        'Common_Procedures.Print_To_PrintDocument(e, Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(I), LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                        'BlockInvNoY = BlockInvNoY + TxtHgt

                        If I = 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(I), LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                        BlockInvNoY = BlockInvNoY + TxtHgt
                            JobNoLnCnt = JobNoLnCnt + 1
                        Else

                            If I < Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$").GetUpperBound(0) Then

                                If e.Graphics.MeasureString(PrtStr + "," + Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(I), pFont).Width > (MxJobWdt - 5) Then
                                    Common_Procedures.Print_To_PrintDocument(e, PrtStr, LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                                    BlockInvNoY = BlockInvNoY + TxtHgt
                                    JobNoLnCnt = JobNoLnCnt + 1
                                    PrtStr = ""
                                Else
                                    PrtStr = PrtStr + "," + Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(I)
                                End If

                            Else

                                If e.Graphics.MeasureString(PrtStr + "," + Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(I), pFont).Width > (MxJobWdt - 5) Then
                                    Common_Procedures.Print_To_PrintDocument(e, PrtStr, LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                                    BlockInvNoY = BlockInvNoY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(I), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                                    JobNoLnCnt = JobNoLnCnt + 1
                                    PrtStr = ""
                                    'BlockInvNoY = BlockInvNoY + TxtHgt
                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, PrtStr + "," + Split(Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), "$$$")(I), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                                    JobNoLnCnt = JobNoLnCnt + 1
                                    'BlockInvNoY = BlockInvNoY + TxtHgt
                                End If

                            End If

                        End If
A:
                    Next
                End If

            End If


            If JobNoLnCnt > 2 And prn_PageNo = 1 Then
                NoofItems_PerPage = NoofItems_PerPage - (JobNoLnCnt - 2)
            End If

            BlockInvNoY = BlockInvNoY + TxtHgt
            BlockInvNoY = BlockInvNoY + TxtHgt - 10

            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Party Dc No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)

                Dim I As Integer
                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString)
                ItmNm2 = ""
                If Len(ItmNm1) > 25 Then
                    For I = 25 To 1 Step -1
                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 25
                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                End If

                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)

                If Trim(ItmNm2) <> "" Then
                    BlockInvNoY = BlockInvNoY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                End If

            End If

            CurY = CurY + TxtHgt + 5

            If JobNoLnCnt > 2 Then
                CurY = CurY + ((JobNoLnCnt - 2) * TxtHgt)
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "No", LMargin, CurY + 5, 2, ClAr(1), p1Font, Brushes.Black)
        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY + 5, 2, ClAr(2), p1Font, Brushes.Black)
        Common_Procedures.Print_To_PrintDocument(e, "DC No.", LMargin + ClAr(1) + ClAr(2), CurY + 5, 2, ClAr(3), p1Font, Brushes.Black)

        'If Common_Procedures.settings.CustomerCode = "5001" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 5, 2, ClAr(4), p1Font, Brushes.Black)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, "SAC", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY - 2, 2, ClAr(4), p1Font, Brushes.Black)
        '    Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 12, 2, ClAr(4), p1Font, Brushes.Black)
        'End If

        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font, Brushes.Black)
        'Common_Procedures.Print_To_PrintDocument(e, "RATE /", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY - 2, 2, ClAr(4), p1Font, Brushes.Black')
        'Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 13, 2, ClAr(4), p1Font, Brushes.Black)

        Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p1Font, Brushes.Black)
        'Common_Procedures.Print_To_PrintDocument(e, "No. OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY - 2, 2, ClAr(5), p1Font, Brushes.Black)
        'Common_Procedures.Print_To_PrintDocument(e, "PIECES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 13, 2, ClAr(5), p1Font, Brushes.Black)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 5, 2, ClAr(6), p1Font, Brushes.Black)

        CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try



    End Sub

    Private Sub Printing_Format2_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)

        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim vTaxPerc As Single = 0
        Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim Unit As String = "PCS"
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select distinct Unit_IdNo from Sales_Details where Sales_Code = '" & NewCode & "' and Not Unit_IdNo is null ", con)
        Dim dt As New DataTable

        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            Unit = "PCS"
        ElseIf dt.Rows.Count = 1 Then
            If dt.Rows(0).Item(0) = "0" Then
                Unit = "PCS"
            Else
                Unit = Microsoft.VisualBasic.Left(Common_Procedures.Unit_IdNoToName(con, dt.Rows(0).Item(0)), 3)
            End If
        Else
            Unit = ""
        End If

        'Try

        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        If CurY < 700 Then
            CurY = 700
        End If

        CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 5, CurY, 1, 0, p1Font)

        'If is_LastPage = True Then
        If Len(Trim(Unit)) > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString) & " " & Unit, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
        End If

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
        'End If

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
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))


        'If is_LastPage = True Then
        Erase BnkDetAr
            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then

                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                BInc = -1
                Yax = CurY

            'Yax = Yax + TxtHgt - 10
            'If Val(prn_PageNo) = 1 Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BANK ACCOUNT INFORMATION ", LMargin + 20, Yax, 0, 0, p1Font)
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

        'End If


        'CurY = CurY

        If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                'If is_LastPage = True Then
                If Common_Procedures.settings.CustomerCode = "5008" Then
                    If Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc")) > 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Material Value Debit @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Material Value Debit : ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
                Else
                Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        End If
            'End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'End If
        End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                'If is_LastPage = True Then

                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

            'End If
        End If

        If prn_HdDt.Rows(0).Item("Tax_Type") = "GST" Then
            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then

                If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
                Else
                    CurY = CurY + 10
                End If

                CurY = CurY + TxtHgt - 10
                'If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
                'End If
            End If



            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                'If is_LastPage = True Then
                If vTaxPerc <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
                'End If
            End If



            If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                'If is_LastPage = True Then
                If vTaxPerc <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
                'End If
            End If

            If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                'If is_LastPage = True Then
                If vTaxPerc <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
                'End If
            End If

        End If

        CurY = CurY + TxtHgt

        If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
            'CurY = CurY + TxtHgt
            'If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            'End If
        End If

        If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Or prn_HdDt.Rows(0).Item("Tax_Type") = "GST" Then

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        End If

        CurY = CurY + TxtHgt - 15

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Total Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
        'If is_LastPage = True Then
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 4, CurY, 1, 0, p1Font)
        'End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))

        Rup1 = ""
            Rup2 = ""
            'If is_LastPage = True Then
            Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            If Len(Rup1) > 80 Then
                For I = 80 To 1 Step -1
                    If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                Next I
                If I = 0 Then I = 80
                Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
            End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "RUPEES : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(Rup2) <> "" Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        'CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Terms :", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "* Payment within 15 days from bill date.    * Overdue bils will carry interest @ 24 % per annum", LMargin + 60, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "* Before using the products in bulk production should make your own test", LMargin + 60, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "* We cannot assume any responsibilty for their use by the customer", LMargin + 60, CurY, 0, 0, pFont)

        ''==========================

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Common_Procedures.Print_To_PrintDocument(e, "Arbitration Clause : Any dispute arising out of this transaction / contarct will be referred to Institutional Arbitration Council", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "of Tirupur as per the Rules and Regulations of Arbitration Council of Tirupur and the award passed will be binding on us.", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Dim cM_br = New SolidBrush(Color.FromArgb(235, 39, 5))

            If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
                cM_br = New SolidBrush(Color.Green)
            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "5002" Then
                cM_br = New SolidBrush(Color.Navy)
            Else
                cM_br = New SolidBrush(Color.Black)
            End If

        Dim TxtWdt As Single

        CurY = CurY + 3



        If Common_Procedures.settings.CustomerCode = "5027" Then
            Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10 - 120 - 5, CurY, 1, 0, p1Font, cM_br)
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.FWC_NAME_1, Drawing.Image), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10 - 120, CurY, 120, 15)
            TxtWdt = 120
        Else
            p1Font = New Font("Cooper Black", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font, Brushes.Red)
            TxtWdt = e.Graphics.MeasureString(Cmp_Name, p1Font).Width
        End If

        If Not IsDBNull(prn_HdDt.Rows(0).Item("User_Name")) Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("User_Name"), LMargin + (PageWidth / 4), CurY, 2, PageWidth / 4, pFont, cM_br)
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature & Seal ", LMargin + 10, CurY, 0, 0, p1Font, cM_br)

        CurY = CurY + (3 * TxtHgt)

        Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + (PageWidth / 4), CurY, 2, PageWidth / 4, pFont, cM_br)
        Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + (PageWidth / 2), CurY, 2, PageWidth / 4, pFont, cM_br)

        If Common_Procedures.settings.CustomerCode = "5027" Then
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, cM_br)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "Director", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, cM_br)
        End If

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

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub txt_Noof_Stitches_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Noof_Stitches.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Noof_Stitches_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Noof_Stitches.TextChanged
        RatePerPcs_Calculation()
    End Sub

    Private Sub txt_RAte_1000Stitches_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RAte_1000Stitches.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_RAte_1000Stitches_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_RAte_1000Stitches.TextChanged
        RatePerPcs_Calculation()
    End Sub

    Private Sub RatePerPcs_Calculation()
        Dim Rate_Pc As Single = 0
        txt_Rate.Text = Format((Val(txt_Noof_Stitches.Text) / 1000) * Val(txt_RAte_1000Stitches.Text), "#########0.00")
    End Sub
    Private Sub btn_BrowsePhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BrowsePhoto.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Picture_Box.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub Printing_Format3_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

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
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ItmNm3 As String, ItmNm4 As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            If Common_Procedures.settings.CustomerCode = "1117" Then
                .Left = 40
            Else
                .Left = 65
            End If
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
        TxtHgt = 18.5 ' 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 20 ' 17 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 30 : ClArr(2) = 300 : ClArr(3) = 70 : ClArr(4) = 0 : ClArr(5) = 0 : ClArr(6) = 70 : ClArr(7) = 80 : ClArr(8) = 60
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                'If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 And Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1


                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 7
                Else
                    If vNoofHsnCodes = 1 Then NoofItems_PerPage = NoofItems_PerPage + vNoofHsnCodes Else NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                Printing_Format3_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try
                    NoofDets = 0

                    If Val(DetIndx) > 18 Then
                        CurY = CurY + TxtHgt
                    End If
                    If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
                        e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SLT_GreyLogo, Drawing.Image), LMargin + 220, CurY + 70, 290, 290)
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
                                Printing_Format3_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                                e.HasMorePages = True

                                Return

                            End If

                            CurY = CurY + TxtHgt


                            Dim k As Integer

                            ItmNm2 = ""
                            ItmNm1 = prn_DetAr(DetIndx, 11)
                            If Len(ItmNm1) > 40 Then
                                For k = 40 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), k, 1) = " " Or Mid$(Trim(ItmNm1), k, 1) = "," Or Mid$(Trim(ItmNm1), k, 1) = "." Or Mid$(Trim(ItmNm1), k, 1) = "-" Or Mid$(Trim(ItmNm1), k, 1) = "/" Or Mid$(Trim(ItmNm1), k, 1) = "_" Or Mid$(Trim(ItmNm1), k, 1) = "(" Or Mid$(Trim(ItmNm1), k, 1) = ")" Or Mid$(Trim(ItmNm1), k, 1) = "\" Or Mid$(Trim(ItmNm1), k, 1) = "[" Or Mid$(Trim(ItmNm1), k, 1) = "]" Or Mid$(Trim(ItmNm1), k, 1) = "{" Or Mid$(Trim(ItmNm1), k, 1) = "}" Then Exit For
                                Next k
                                If k = 0 Then k = 40
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - k)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), k - 1)
                            End If

                            ItmNm4 = ""
                            ItmNm3 = ItmNm2
                            If Len(ItmNm3) > 40 Then
                                For k = 40 To 1 Step -1
                                    If Mid$(Trim(ItmNm3), k, 1) = " " Or Mid$(Trim(ItmNm3), k, 1) = "," Or Mid$(Trim(ItmNm3), k, 1) = "." Or Mid$(Trim(ItmNm3), k, 1) = "-" Or Mid$(Trim(ItmNm3), k, 1) = "/" Or Mid$(Trim(ItmNm3), k, 1) = "_" Or Mid$(Trim(ItmNm3), k, 1) = "(" Or Mid$(Trim(ItmNm3), k, 1) = ")" Or Mid$(Trim(ItmNm3), k, 1) = "\" Or Mid$(Trim(ItmNm3), k, 1) = "[" Or Mid$(Trim(ItmNm3), k, 1) = "]" Or Mid$(Trim(ItmNm3), k, 1) = "{" Or Mid$(Trim(ItmNm3), k, 1) = "}" Then Exit For
                                Next k
                                If k = 0 Then k = 40
                                ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - k)
                                ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), k - 1)
                            End If



                            If Trim(prn_DetAr(DetIndx, 11)) <> "" And Trim(prn_DetAr(DetIndx, 10)) = "SERIALNO" Then
                                CurY = CurY - 3
                                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 25, CurY, 0, 0, p1Font)
                            ElseIf Trim(prn_DetAr(DetIndx, 11)) <> "" And Trim(prn_DetAr(DetIndx, 10)) = "ITEM_2ND_LINE" Then
                                CurY = CurY - 3
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 11), LMargin + ClArr(1) + 25, CurY, 0, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 1), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 4), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 2, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 2, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(DetIndx, 9), PageWidth - 5, CurY, 1, 0, pFont)
                            End If

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm3), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmNm3) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm4), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(ItmNm4) <> "" Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            NoofDets = NoofDets + 1

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format3_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)
                    prn_Count = prn_Count + 1
                    'If Trim(prn_InpOpts) <> "" Then
                    '    If prn_Count < Len(Trim(prn_InpOpts)) Then

                    '        DetIndx = 1
                    '        prn_PageNo = 0

                    '        e.HasMorePages = True
                    '        Return
                    '    End If
                    'End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        ' prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        'prn_OriDupTri = ""
        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count <= Len(Trim(prn_InpOpts)) Then

        '        S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

        '        If Val(S) = 1 Then
        '            prn_OriDupTri = "ORIGINAL"
        '            PrintDocument1.DefaultPageSettings.Color = True
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
        '            e.PageSettings.Color = True
        '        ElseIf Val(S) = 2 Then
        '            prn_OriDupTri = "DUPLICATE"
        '        ElseIf Val(S) = 3 Then
        '            prn_OriDupTri = "TRIPLICATE"
        '        End If

        '    End If
        'End If

        'If Trim(prn_OriDupTri) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If

        p1Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "JOBWORK TAX INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
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

        If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SLT_Logo, Drawing.Image), LMargin + 24, CurY + 10, 100, 100)
        End If


        CurY = CurY + TxtHgt - 10

        p1Font = New Font("President", 25, FontStyle.Bold)
        pFont = New Font("Calibri", 10, FontStyle.Bold)

        If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Green)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            Dim br = New SolidBrush(Color.FromArgb(191, 43, 133))

            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, br)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, br)

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
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, br)
            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, br)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, br)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, br)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, br)


        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            Dim br = New SolidBrush(Color.FromArgb(191, 43, 133))

            CurY = CurY + strHeight
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

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

        End If



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        pFont = New Font("Calibri", 10, FontStyle.Regular)

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                If UBound(PnAr) >= 5 Then Led_State = Trim(PnAr(5))
                If UBound(PnAr) >= 6 Then Led_PhNo = Trim(PnAr(6))
                If UBound(PnAr) >= 7 Then Led_GSTTinNo = Trim(PnAr(7))

            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

                Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)

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


            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

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


            '------------------- Invoice No Block

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt


            BlockInvNoY = BlockInvNoY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            BlockInvNoY = BlockInvNoY + TxtHgt
            BlockInvNoY = BlockInvNoY + TxtHgt

            If rdo_OrderNoInHeader.Checked Then
                If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Order No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
                End If
            End If

            'If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Electronic Ref.No", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 25, BlockInvNoY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + Cen1 + W1 + 40, BlockInvNoY, 0, 0, pFont)
            'End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY + 5, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY + 5, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClAr(1) + ClAr(2), CurY + 5, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 5, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "No.of", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Rate/100", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 5, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + 5, 2, ClAr(9), pFont)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Stitches", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "Stitches", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)

        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim vTaxPerc As Single = 0
        Dim Yax As Single
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
            'If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SubTotal_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            'End If

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

            If prn_HdDt.Rows(0).Item("Tax_Type").ToString = "GST" Then

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

                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                    If is_LastPage = True Then
                        If vTaxPerc <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If
                End If

                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString) <> 0 Then
                    If is_LastPage = True Then
                        If vTaxPerc <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If
                End If

                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString) <> 0 Then
                    If is_LastPage = True Then
                        If vTaxPerc <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(vTaxPerc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGst_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If
                End If

            End If
            'CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 4, CurY, 1, 0, p1Font)
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

            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            ''=============GST SUMMARY============
            'vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'If vNoofHsnCodes <> 0 Then
            '    Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            'End If
            ''==========================

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
            End If


            If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Remarks :", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Payment must be produce 10 days from our bill date.", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Normal embroidery mistake allowance 1 % and applique", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "embroidery mistake 3 % would be allowed", LMargin + 10, CurY, 0, 0, pFont)

            Else
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Remarks :", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Payment must be produce 7 days from our bill date.", LMargin + 10, CurY, 0, 0, pFont)

            End If
            If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, Brushes.Green)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If


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

    Private Sub Btn_Clear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Clear1.Click
        Picture_Box.BackgroundImage = Nothing
    End Sub
    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "size_head", "size_name", "", "(Size_IdNo = 0)")
    End Sub


    Private Sub cbo_Size_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        QuantityDetails()

    End Sub

    Private Sub cbo_Size_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        QuantityDetails()

    End Sub

    Private Sub cbo_OrderCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OrderCode.GotFocus

    End Sub

    Private Sub cbo_OrderCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OrderCode.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OrderCode, cbo_ItemName, cbo_JobNumber, "Order_Program_head", "Ordercode_forSelection",
                                                         "(Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, "") &
                                                         " OR Billing_Name_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, "") & ")", "")



    End Sub

    Private Sub cbo_OrderCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OrderCode.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OrderCode, cbo_JobNumber, "Order_Program_head", "Ordercode_forSelection",
                                                          "(Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, "") &
                                                          " OR Billing_Name_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, "") & ")", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_OrderCode.Tag)) <> Trim(UCase(cbo_OrderCode.Text)) Then
                'get_Item_Unit_Rate_TaxPerc()
            End If
            If Trim(cbo_OrderCode.Text) <> "" Then
                cbo_JobNumber.Focus()
            Else
                txt_CashDiscPerc.Focus()
            End If
        End If

    End Sub


    Private Sub btn_DelPending_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_DelPending.Click
        QuantityDetails()
        grp_Quantity_Details.Visible = True
        dgv_Details.Enabled = False
    End Sub

    Private Sub btn_Close_Quantity_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Quantity_Details.Click
        grp_Quantity_Details.Visible = False
        dgv_Details.Enabled = True
    End Sub

    Private Sub t1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles t1.TextChanged
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(o1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(o2.Text)
        'b3.Text = Val(t3.Text) - Val(s3.Text)
    End Sub

    Private Sub s1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles s1.TextChanged
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(o1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(o2.Text)
        'b3.Text = Val(t3.Text) - Val(s3.Text)
    End Sub

    Private Sub t2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles t2.TextChanged
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(o1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(o2.Text)
        'b3.Text = Val(t3.Text) - Val(s3.Text)
    End Sub

    Private Sub s2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles s2.TextChanged
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(o1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(o2.Text)
        'b3.Text = Val(t3.Text) - Val(s3.Text)
    End Sub

    Private Sub b1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles b1.TextChanged
        If Val(txt_Quantity.Text) > Val(b1.Text) Or Val(txt_Quantity.Text) > Val(b2.Text) Then
            txt_Quantity.BackColor = Color.Red
            txt_Quantity.ForeColor = Color.Yellow
            Beep()
        Else
            txt_Quantity.BackColor = Color.White
            txt_Quantity.ForeColor = Color.Black
        End If
    End Sub

    Private Sub lbl_Amount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_Amount.Click

        If Val(txt_Quantity.Text) > Val(b1.Text) Or Val(txt_Quantity.Text) > Val(b2.Text) Then
            txt_Quantity.BackColor = Color.Red
            txt_Quantity.ForeColor = Color.Yellow
            Beep()
        Else
            txt_Quantity.BackColor = Color.White
            txt_Quantity.ForeColor = Color.Black
        End If

    End Sub

    Private Sub QuantityDetails()

        If Len(Trim(cbo_OrderCode.Text)) = 0 Or Len(Trim(cbo_JobNumber.Text)) = 0 Or Len(Trim(cbo_DCNo.Text)) = 0 Then


            t1.Text = "0"
            s1.Text = "0"

        Else

            t1.Text = Common_Procedures.get_FieldValue(con, "Sales_Delivery_Details", "sum(Quantity)", "OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Sales_Delivery_Code Like 'EMDEL%'  and  Job_No = '" & cbo_JobNumber.Text & "' and " &
                                                            " CONVERT(varchar,Sales_Delivery_No)+'('+ convert(varchar,Sales_delivery_Date,5) + ')' IN (" & DCCODES1 & ") and Delivery_Purpose = 'Good'" _
                                                            , 0)


            s1.Text = Common_Procedures.get_FieldValue(con, "Sales_Details", "sum(Quantity)", "OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Sales_Code Like 'GINVE%'  and " &
                                                               "  Job_No = '" & cbo_JobNumber.Text & "' and  DC_No IN (" & DCCODES1 & ") " &
                                                               " AND NOT SALES_CODE = '" & Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "'" _
                                                               , 0)

            o1.Text = "0"

            For I As Int16 = 0 To dgv_Details.Rows.Count - 1
                If Val(txt_SlNo.Text) <> Val(dgv_Details.Rows(I).Cells(0).Value) And cbo_OrderCode.Text = dgv_Details.Rows(I).Cells(21).Value And cbo_JobNumber.Text = dgv_Details.Rows(I).Cells(24).Value And cbo_DCNo.Text = dgv_Details.Rows(I).Cells(3).Value Then
                    o1.Text = Val(o1.Text) + Val(dgv_Details.Rows(I).Cells(7).Value)
                End If
            Next

        End If

        '------------------

        If Len(Trim(cbo_OrderCode.Text)) = 0 Or Len(Trim(cbo_JobNumber.Text)) = 0 Or Len(Trim(cbo_DCNo.Text)) = 0 Or Len(Trim(cbo_Colour.Text)) = 0 Or Len(Trim(cbo_Component.Text)) = 0 Then


            t2.Text = "0"
            s2.Text = "0"

        Else

            t2.Text = Common_Procedures.get_FieldValue(con, "Sales_Delivery_Details", "sum(Quantity)", "OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Sales_Delivery_Code Like 'EMDEL%'  and  Job_No = '" & cbo_JobNumber.Text & "' and " &
                                                            " CONVERT(varchar,Sales_Delivery_No)+'('+ convert(varchar,Sales_delivery_Date,5) + ')' IN (" & DCCODES1 & ") and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) &
                                                            " and Component_IdNo = " & Common_Procedures.Component_NameToIdNo(con, cbo_Component.Text) &
                                                            " And Delivery_Purpose = 'Good'" _
                                                            , 0)


            s2.Text = Common_Procedures.get_FieldValue(con, "Sales_Details", "sum(Quantity)", "OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Sales_Code Like 'GINVE%'  and " &
                                                               "  Job_No = '" & cbo_JobNumber.Text & "' and  DC_No IN (" & DCCODES1 & ") and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) &
                                                               " and Component_IdNo = " & Common_Procedures.Component_NameToIdNo(con, cbo_Component.Text) &
                                                               " AND NOT SALES_CODE = '" & Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "'" _
                                                               , 0)

            o2.Text = "0"

            For I As Int16 = 0 To dgv_Details.Rows.Count - 1
                If Val(txt_SlNo.Text) <> Val(dgv_Details.Rows(I).Cells(0).Value) And cbo_OrderCode.Text = dgv_Details.Rows(I).Cells(21).Value And
                    cbo_JobNumber.Text = dgv_Details.Rows(I).Cells(24).Value And cbo_DCNo.Text = dgv_Details.Rows(I).Cells(3).Value And
                    cbo_Colour.Text = dgv_Details.Rows(I).Cells(19).Value And cbo_Component.Text = dgv_Details.Rows(I).Cells(25).Value Then
                    o2.Text = Val(o2.Text + Val(dgv_Details.Rows(I).Cells(7).Value))
                End If
            Next


        End If

        '------------------

        txt_Quantity.Text = b2.Text

        For i As Int16 = 0 To dgv_Details.Rows.Count - 1
            If dgv_Details.Rows(i).Cells(21).Value = cbo_OrderCode.Text And dgv_Details.Rows(i).Cells(24).Value = cbo_JobNumber.Text _
                And dgv_Details.Rows(i).Cells(19).Value = cbo_Colour.Text And dgv_Details.Rows(i).Cells(25).Value = cbo_Component.Text _
                And UCase(dgv_Details.Rows(i).Cells(3).Value) = "ALL" And Val(dgv_Details.Rows(i).Cells(0).Value) <> Val(txt_SlNo.Text) Then
                'MsgBox("ALL D.C NUMBERS PERTAINING TO THIS JOB NUMBER / ORDER CODE HAVE BEEN INCLUDED IN THIS INVOICE. INVALID ENTRY", vbOK, "INVALID ENTRY")
                txt_Quantity.Text = "0"
                Exit Sub
            End If
        Next

        If cbo_DCNo.Text = "ALL" Then

            For i As Int16 = 0 To dgv_Details.Rows.Count - 1
                If dgv_Details.Rows(i).Cells(21).Value = cbo_OrderCode.Text And dgv_Details.Rows(i).Cells(24).Value = cbo_JobNumber.Text _
                    And dgv_Details.Rows(i).Cells(19).Value = cbo_Colour.Text And dgv_Details.Rows(i).Cells(25).Value = cbo_Component.Text _
                    And Val(dgv_Details.Rows(i).Cells(0).Value) <> Val(txt_SlNo.Text) Then
                    'MsgBox("'ALL' OR FEW D.C NUMBERS PERTAINING TO THIS JOB NUMBER / ORDER CODE HAVE BEEN INCLUDED IN THIS INVOICE ALREADY . INVALID ENTRY", vbOK, "INVALID ENTRY")
                    txt_Quantity.Text = "0"
                    Exit Sub
                End If
            Next

        End If

        If (Val(txt_Quantity.Text) > Val(b2.Text)) Or Val(txt_Quantity.Text) = 0 Then

            If Val(txt_Quantity.Text) <> 0 Then
                txt_Quantity.BackColor = Color.Red
                txt_Quantity.ForeColor = Color.Yellow
            End If
            btn_Add.Enabled = False
            Beep()

        Else

            txt_Quantity.BackColor = Color.White
            txt_Quantity.ForeColor = Color.Black

            btn_Add.Enabled = True

            If Val(txt_Quantity.Text) = 0 Then
                txt_Quantity.BackColor = Color.White
                txt_Quantity.ForeColor = Color.Black
            End If

        End If

    End Sub

    Private Sub b2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles b2.TextChanged

        If Val(txt_Quantity.Text) > Val(b1.Text) Or Val(txt_Quantity.Text) > Val(b2.Text) Then
            txt_Quantity.BackColor = Color.Red
            txt_Quantity.ForeColor = Color.Yellow
            Beep()
        Else
            txt_Quantity.BackColor = Color.White
            txt_Quantity.ForeColor = Color.Black
        End If

    End Sub

    Private Sub btn_AutoBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AutoBill.Click
        pnl_InputDetails.Enabled = False
        dgv_Details.Enabled = False
        ListOrdersInvoicePending()
        'grp_OrderList.Visible = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pnl_InputDetails.Enabled = True
        dgv_Details.Enabled = True
        'grp_OrderList.Visible = False
    End Sub

    Private Sub ListOrdersInvoicePending()

        'dgv_OrderList.Rows.Clear()

        'Dim da1 As New SqlClient.SqlDataAdapter
        'Dim da2 As New SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim dt2 As New DataTable
        'Dim dt4 As New DataTable

        'da1 = New SqlClient.SqlDataAdapter("select B.Ledger_Name,A.OrderCode_forSelection,A.Order_Program_Date,A.Design,A.Pieces from Order_Program_Head A Inner Join Ledger_Head B on A.Ledger_IdNo = B.Ledger_IdNo where Not A.Close_Status = 1", con)
        'da1.Fill(dt1)

        'If dt1.Rows.Count > 0 Then

        '    For I As Integer = 0 To dt1.Rows.Count - 1

        '        dgv_OrderList.Rows.Add()
        '        dgv_OrderList.Item(0, I).Value = dt1.Rows(I).Item(0)
        '        dgv_OrderList.Item(1, I).Value = dt1.Rows(I).Item(1)
        '        dgv_OrderList.Item(2, I).Value = Format(dt1.Rows(I).Item(2), "dd-MM-yyyy")
        '        dgv_OrderList.Item(3, I).Value = dt1.Rows(I).Item(3)
        '        dgv_OrderList.Item(4, I).Value = dt1.Rows(I).Item(4)

        '        dgv_OrderList.Item(5, I).Value = Common_Procedures.get_FieldValue(con, "Simple_Receipt_Details", "sum(Quantity)", "OrderCode_forSelection = '" & dt1.Rows(I).Item(1) & "' and Simple_Receipt_Code Like 'EMREC%'", Common_Procedures.CompIdNo)
        '        dgv_OrderList.Item(6, I).Value = Common_Procedures.get_FieldValue(con, "Sales_Delivery_Details", "sum(Quantity)", "OrderCode_forSelection = '" & dt1.Rows(I).Item(1) & "' and Sales_Delivery_Code Like 'EMDEL%' ", Common_Procedures.CompIdNo)
        '        dgv_OrderList.Item(7, I).Value = Common_Procedures.get_FieldValue(con, "Sales_Details", "sum(Quantity)", "OrderCode_forSelection = '" & dt1.Rows(I).Item(1) & "' and Sales_Code Like 'GINVE%' ", Common_Procedures.CompIdNo)

        '        dgv_OrderList.Item(8, I).Value = Val(dgv_OrderList.Item(6, I).Value) - Val(dgv_OrderList.Item(7, I).Value)

        '    Next

        'End If

    End Sub

    Private Sub dgv_OrderList_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        'If e.ColumnIndex = 9 Then
        '    If CBool(dgv_OrderList.Item(9, e.RowIndex).Value) = True Then
        '        For i = 0 To dgv_OrderList.Rows.Count - 1
        '            If Not IsDBNull(dgv_OrderList.Item(9, i).Value) And Not i = e.RowIndex Then
        '                dgv_OrderList.Item(9, i).Value = False
        '                dgv_OrderList.Item(10, i).Value = False
        '                dgv_OrderList.Item(11, i).Value = False
        '            End If
        '        Next
        '    Else
        '        dgv_OrderList.Item(10, e.RowIndex).Value = False
        '        dgv_OrderList.Item(11, e.RowIndex).Value = False
        '    End If
        'End If

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '        ImageInAutoBill = False
        '        CloseOrderAfterAutoBill = False
        '        AutoBillOrdNo = ""
        '        AutoBillRow = -1

        '        For i = 0 To dgv_OrderList.Rows.Count - 1

        '            If CBool(dgv_OrderList.Item(9, i).Value) = True Then
        '                AutoBillOrdNo = dgv_OrderList.Item(1, i).Value

        '                If CBool(dgv_OrderList.Item(10, i).Value) = True Then
        '                    ImageInAutoBill = True
        '                End If

        '                If CBool(dgv_OrderList.Item(11, i).Value) = True Then
        '                    CloseOrderAfterAutoBill = True
        '                End If

        '                AutoBillRow = i

        '                GoTo Cont

        '            End If
        '        Next

        '        MsgBox("No Order Selected To Generate Invoice")

        '        Exit Sub

        'Cont:

        '        If Val(dgv_OrderList.Item(8, AutoBillRow).Value) <= 0 Then
        '            MsgBox("Invoice Quantity Is Zero Or Negative. Cannot Continue")
        '            Exit Sub
        '        End If

        '        Dim MsgStr As String

        '        MsgStr = "You Are About to Generate Auto Bill For " & AutoBillOrdNo & "."

        '        If ImageInAutoBill Then
        '            MsgStr = MsgStr + "Image Will Be Displayed On Bill. "
        '        Else
        '            MsgStr = MsgStr + "Image Will NOT Be Displayed On Bill. "
        '        End If


        '        If CloseOrderAfterAutoBill Then
        '            MsgStr = MsgStr + "Order Will Be CLOSED. CONTINE ? "
        '        Else
        '            MsgStr = MsgStr + "Order Will Be OPEN . CONTINUE ?"
        '        End If

        '        If MsgBox(MsgStr, MsgBoxStyle.YesNo) = vbNo Then
        '            Exit Sub
        '        End If

        '        GenerateAutoInvoice()

        '        pnl_InputDetails.Enabled = True
        '        dgv_Details.Enabled = True
        '        grp_OrderList.Visible = False

    End Sub

    Private Sub GenerateAutoInvoice()

        'Dim ClOrd As Boolean = CloseOrderAfterAutoBill
        'Dim ImAM As Boolean = ImageInAutoBill
        'Dim ABONo = AutoBillOrdNo
        'Dim ABRow = AutoBillRow

        'new_record()

        'CloseOrderAfterAutoBill = ClOrd
        'ImageInAutoBill = ImAM
        'AutoBillOrdNo = ABONo
        'AutoBillRow = ABRow

        'cbo_EntType.Text = "DIRECT"
        ''cbo_Ledger.Text = dgv_OrderList.Item(0, AutoBillRow).Value
        'cbo_PaymentMethod.Text = "CREDIT"
        'cbo_TaxType.Text = "GST"


        'With cbo_ItemName
        '    vcmb_ItmNm = Trim(.Text)
        '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_ItemName, con, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
        'End With

        'If cbo_ItemName.Items.Count >= 2 Then
        '    cbo_ItemName.Text = cbo_ItemName.GetItemText(cbo_ItemName.Items(1))
        'End If


        'Dim da As SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim gstrate As Double
        'Dim lED_iD As Integer = 0
        'Ord_No = ""
        'Ord_Date = ""

        'da = New SqlClient.SqlDataAdapter("SELECT ITEM_NAME FROM ITEM_HEAD WHERE ISDEFAULT_ITEM_FOR_AUTO_BILL = 1", con)
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    cbo_ItemName.Text = dt.Rows(0).Item(0)
        'End If
        'dt.Dispose()
        'da.Dispose()

        'If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
        '    'get_Item_Unit_Rate_TaxPerc()
        'End If
        'If Trim(UCase(cbo_ItemName.Tag)) <> Trim(UCase(cbo_ItemName.Text)) Then
        '    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
        '    dt = New DataTable
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then


        '        'If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
        '        '    txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
        '        'End If
        '        get_Item_Tax(False)
        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If

        'If Trim(UCase(vcmb_ItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then
        '    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_ItemName.Text) & "'", con)
        '    dt = New DataTable
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then

        '        If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
        '            txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
        '        End If

        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If

        ''-------------------------------

        ''cbo_OrderCode.Text = dgv_OrderList.Item(1, AutoBillRow).Value



        'If Trim(UCase(cbo_OrderCode.Text)) <> "" Then

        '    da = New SqlClient.SqlDataAdapter("select a.* from Order_Program_Head a where a.Ordercode_forSelection = '" & Trim(cbo_OrderCode.Text) & "'", con)
        '    dt = New DataTable
        '    da.Fill(dt)

        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)("Design").ToString) = False Then
        '            txt_Detail = dt.Rows(0)("Design").ToString
        '        End If
        '        If IsDBNull(dt.Rows(0)("StchsPr_Pcs").ToString) = False Then
        '            txt_Noof_Stitches.Text = dt.Rows(0)("StchsPr_Pcs").ToString
        '        End If
        '        If IsDBNull(dt.Rows(0)("Colour_Idno").ToString) = False Then
        '            cbo_colour.Text = Common_Procedures.Colour_IdNoToName(con, Val(dt.Rows(0).Item("COlour_IdNo").ToString))
        '        End If
        '        If IsDBNull(dt.Rows(0)("Size_Idno").ToString) = False Then
        '            cbo_Size.Text = Common_Procedures.Size_IdNoToName(con, Val(dt.Rows(0).Item("Size_IdNo").ToString))
        '        End If
        '        If IsDBNull(dt.Rows(0)("Order_No").ToString) = False Then
        '            Ord_No = dt.Rows(0).Item("Order_No").ToString
        '        End If
        '        If IsDBNull(dt.Rows(0)("Order_Program_Date")) = False Then
        '            Ord_Date = Format(dt.Rows(0).Item("Order_Program_Date"), "dd-MM-yyyy")
        '        End If
        '        If IsDBNull(dt.Rows(0)("Pieces").ToString) = False Then
        '            txt_Quantity.Text = dt.Rows(0)("Pieces").ToString
        '        End If
        '        If IsDBNull(dt.Rows(0)("Stiches").ToString) = False Then
        '            txt_RAte_1000Stitches.Text = dt.Rows(0)("Stiches").ToString
        '        End If

        '        If IsDBNull(dt.Rows(0)("Rate").ToString) = False Then
        '            txt_Rate.Text = dt.Rows(0)("Rate").ToString
        '        End If

        '        If ImageInAutoBill Then
        '            If IsDBNull(dt.Rows(0).Item("Order_Image")) = False Then
        '                Dim imageData As Byte() = DirectCast(dt.Rows(0).Item("Order_Image"), Byte())
        '                If Not imageData Is Nothing Then
        '                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
        '                        ms.Write(imageData, 0, imageData.Length)
        '                        If imageData.Length > 0 Then

        '                            Picture_Box.BackgroundImage = Image.FromStream(ms)

        '                        End If
        '                    End Using
        '                End If
        '            End If
        '        End If

        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If


        'QuantityDetails()

        'txt_Quantity.Text = Val(dgv_OrderList.Item(8, AutoBillRow).Value)

    End Sub


    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
    End Sub

    Private Sub txt_Quantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Quantity.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Quantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Quantity.TextChanged

        If (Val(txt_Quantity.Text) > Val(b2.Text)) Or Val(txt_Quantity.Text) = 0 Then
            txt_Quantity.BackColor = Color.Red
            txt_Quantity.ForeColor = Color.Yellow
            btn_Add.Enabled = False
            Beep()
        Else
            txt_Quantity.BackColor = Color.White
            txt_Quantity.ForeColor = Color.Black
            btn_Add.Enabled = True
        End If

        Call Amount_Calculation(False)


    End Sub

    Private Sub chk_Close_Order_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub chk_Close_Order_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        'If UCase(e.KeyChar) = "Y" Then
        '    chk_Close_Order.Checked = True
        'ElseIf UCase(e.KeyChar) = "N" Then
        '    chk_Close_Order.Checked = False
        'ElseIf Asc(e.KeyChar) = 13 Then
        '    btn_Add_Click(sender, e)
        'End If

    End Sub



    Private Sub chk_ShowOnlyActiveOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_ShowOnlyActiveOrders.CheckedChanged

        If chk_ShowOnlyActiveOrders.Checked Then
            Order_Disp_Cond = "Close_Status = 0"
        Else
            Order_Disp_Cond = ""
        End If
    End Sub



    Private Sub btn_Close_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        MDIParent1.Close_Form()
    End Sub

    Private Sub txt_OrderNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_OrderNo.TextChanged
        If Common_Procedures.settings.CustomerCode = "1201" Or Common_Procedures.settings.CustomerCode = "5002" Then
            txt_OrderNo.Tag = txt_OrderNo.Text
        End If
    End Sub

    Private Sub chk_AutoPopulate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'If chk_AutoPopulate.Checked Then

        '    rdo_TotalQty.Enabled = True
        '    rdo_Colour.Enabled = True
        '    rdo_Colour_Size.Enabled = True
        '    dtp_CutOffDate.Enabled = True
        '    dtp_CutOffDate.Value = Now
        'Else

        '    rdo_TotalQty.Enabled = False
        '    rdo_Colour.Enabled = False
        '    rdo_Colour_Size.Enabled = False

        '    rdo_TotalQty.Checked = False
        '    rdo_Colour.Checked = False
        '    rdo_Colour_Size.Checked = False
        '    dtp_CutOffDate.Enabled = False
        '    dtp_CutOffDate.Value = CDate("1-1-2018")

        'End If

    End Sub

    Private Sub t3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(o1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(o2.Text)
        'b3.Text = Val(t3.Text) - Val(s3.Text)
    End Sub

    Private Sub s3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(o1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(o2.Text)
        'b3.Text = Val(t3.Text) - Val(s3.Text)
    End Sub


    Private Sub txt_CashDiscAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscAmount.KeyDown

        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            If Trim(UCase(cbo_EntType.Text)) = "DELIVERY" Then
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    cbo_Ledger.Focus()
                End If

            Else
                cbo_ItemName.Focus()

            End If
        End If

    End Sub

    Private Sub txt_CashDiscAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscAmount.KeyPress

        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_CashDiscAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscAmount.TextChanged
        If Val(txt_CashDiscAmount.Text) > Val(lbl_GrossAmount.Text) Then

            MsgBox("Discount Amount Cannot Exceed Gross Invoice Amount")
            txt_CashDiscPerc.Text = "0.00"
            txt_CashDiscAmount.Text = "0.00"

        Else
            If Val(txt_CashDiscPerc.Text) = 0 Then
                Dim DedAmt As Single = Val(txt_CashDiscAmount.Text)

                For I As Integer = 0 To dgv_Details.Rows.Count - 1
                    If DedAmt >= Val(dgv_Details.Rows(I).Cells(8).Value) Then
                        dgv_Details.Rows(I).Cells(14).Value = "0.00"
                        DedAmt = DedAmt - Val(dgv_Details.Rows(I).Cells(8).Value)
                    Else
                        dgv_Details.Rows(I).Cells(14).Value = FormatNumber((Val(dgv_Details.Rows(I).Cells(8).Value) - DedAmt), 2, TriState.False, TriState.False, TriState.False)
                        DedAmt = 0
                    End If

                    If DedAmt <= 0 Then
                        Exit For
                    End If
                Next
            End If

            TotalAmount_Calculation()

        End If

    End Sub

    Private Sub lbl_GrossAmount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_GrossAmount.Click

    End Sub

    Private Sub lbl_GrossAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_GrossAmount.TextChanged
        If Val(txt_CashDiscAmount.Text) > Val(lbl_GrossAmount.Text) Then
            MsgBox("Discount Amount Cannot Exceed Gross Invoice Amount")
            txt_CashDiscPerc.Text = "0.00"
            txt_CashDiscAmount.Text = "0.00"
        End If
    End Sub

    Private Sub cbo_JobNumber_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_JobNumber.GotFocus



    End Sub

    Private Sub cbo_JobNumber_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_JobNumber.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_JobNumber, txt_SlNo, cbo_Colour, "OrderJobNo_Head", "OrderJobNo_Name", " OrderNo_Name = '" & cbo_OrderCode.Text & "'", "")

        If (e.KeyValue = 40 And cbo_OrderCode.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_JobNumber.Text) <> "" Then
                cbo_Colour.Focus()
            Else
                txt_CashDiscPerc.Focus()
            End If
        End If

        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "SIMPLE_RECEIPT_DETAILS", "JOB_NO", " Ordercode_forSelection = '" & Cbo_OrderCode.Text & "' " & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, ""), "")

    End Sub

    Private Sub cbo_JobNumber_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_JobNumber.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_JobNumber, Nothing, "OrderJobNo_Head", "OrderJobNo_Name", " OrderNo_Name = '" & cbo_OrderCode.Text & "'", "")

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_JobNumber.Text) <> "" Then
                cbo_DCNo.Focus()
            Else
                txt_CashDiscPerc.Focus()
            End If
        End If

        ' Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "SIMPLE_RECEIPT_DETAILS", "JOB_NO", " Ordercode_forSelection = '" & Cbo_OrderCode.Text & "' " & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, ""), "")

    End Sub


    Private Sub cbo_DCNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_DCNo.KeyDown

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        CMD.CommandText = "truncate table Combo_Pop_Temp"
        CMD.ExecuteNonQuery()

        CMD.CommandText = " INSERT INTO Combo_Pop_Temp SELECT convert(varchar,Sales_Delivery_No)+'('+CONVERT(varchar,sales_delivery_date,5)+')' FROM Sales_Delivery_Details  " &
                              " WHERE  not Sales_Delivery_No + '('+ convert(varchar,sales_delivery_date,5)+')' + Job_No + OrderCode_forSelection + convert(varchar,Colour_IdNo) + convert(varchar,Component_IdNo) in " &
                              " (Select Sales_DC_Code + Job_NO + uid + convert(varchar,ISNULL(Colour_IdNo,0)) + convert(varchar,ISNULL(Component_IdNo,0))  " &
                              " from Invoice_DC_Details) " &
                              " And OrderCode_forSelection in (Select OrderCode_forSelection from Order_Program_Head Where Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) &
                              " Or Billing_Name_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & ")" &
                              " And Job_No = '" & cbo_JobNumber.Text & "' AND OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) &
                              " and Component_IdNo = " & Common_Procedures.Component_NameToIdNo(con, cbo_Component.Text) & " and Sales_Delivery_Date <= '" & Format(dtp_CutOffDate.Value, "dd/MMM/yyyy") & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "INSERT INTO Combo_Pop_Temp SELECT DC_NO FROM SALES_DETAILS WHERE SALES_CODE = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "INSERT INTO Combo_Pop_Temp VALUES('ALL')"
        CMD.ExecuteNonQuery()

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DCNo, cbo_Colour, txt_Noof_Stitches, "Combo_Pop_Temp", "LOV", "", "")


    End Sub

    Private Sub cbo_DCNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_DCNo.KeyPress

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        CMD.CommandText = "truncate table Combo_Pop_Temp"
        CMD.ExecuteNonQuery()

        'CMD.CommandText = "INSERT INTO Combo_Pop_Temp SELECT convert(varchar,Sales_Delivery_No)+'('+CONVERT(varchar,sales_delivery_date,5)+')' FROM Sales_Delivery_Details  " &
        '                      " WHERE  not Sales_Delivery_No + '('+ convert(varchar,sales_delivery_date,5)+')' + Job_No in (Select Sales_DC_Code + Job_NO from Invoice_DC_Details) AND Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) &
        '                      " and Job_No = '" & cbo_JobNumber.Text & "' AND Sales_Delivery_Date <= '" & Format(dtp_CutOffDate.Value, "dd/MMM/yyyy") & "'"

        CMD.CommandText = " INSERT INTO Combo_Pop_Temp SELECT convert(varchar,Sales_Delivery_No)+'('+CONVERT(varchar,sales_delivery_date,5)+')' FROM Sales_Delivery_Details  " &
                              " WHERE  not Sales_Delivery_No + '('+ convert(varchar,sales_delivery_date,5)+')' + Job_No + OrderCode_forSelection + convert(varchar,Colour_IdNo) + convert(varchar,Component_IdNo) in " &
                              " (Select Sales_DC_Code + Job_NO + uid + convert(varchar,ISNULL(Colour_IdNo,0)) + convert(varchar,ISNULL(Component_IdNo,0))  " &
                              " from Invoice_DC_Details) " &
                              " And OrderCode_forSelection in (Select OrderCode_forSelection from Order_Program_Head Where Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) &
                              " Or Billing_Name_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & ")" &
                              " And Job_No = '" & cbo_JobNumber.Text & "' AND OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) &
                              " and Component_IdNo = " & Common_Procedures.Component_NameToIdNo(con, cbo_Component.Text) & " and Sales_Delivery_Date <= '" & Format(dtp_CutOffDate.Value, "dd/MMM/yyyy") & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "INSERT INTO Combo_Pop_Temp SELECT DC_NO FROM SALES_DETAILS WHERE SALES_CODE = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "INSERT INTO Combo_Pop_Temp VALUES('ALL')"
        CMD.ExecuteNonQuery()

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DCNo, Nothing, "Combo_Pop_Temp", "LOV", "", "")

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_JobNumber.Text) <> "" Then
                txt_Noof_Stitches.Focus()
            Else
                txt_CashDiscPerc.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_DCNo_LostFocus(sender As Object, e As EventArgs) Handles cbo_DCNo.LostFocus




    End Sub

    Private Sub btn_Quantity_Click_2(sender As Object, e As EventArgs) Handles btn_Quantity.Click

        txt_Quantity.Text = "0"

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        'Dim gstrate As Double
        Dim lED_iD As Integer = 0
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Ord_No = ""
        Ord_Date = ""

        If Trim(UCase(cbo_OrderCode.Text)) <> "" Then


            If Len(Trim(cbo_Colour.Text)) > 0 Then
                da = New SqlClient.SqlDataAdapter("select sum(Quantity) from Sales_Delivery_Details where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and convert(varchar,Sales_Delivery_No) + '('+ convert(varchar,Sales_Delivery_Date,5) +')' = '" &
                                                cbo_DCNo.Text & "' and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) & " and Component_IdNo = " & Common_Procedures.Component_NameToIdNo(con, cbo_Component.Text) & " And Delivery_Purpose = 'Good'", con)
            Else
                da = New SqlClient.SqlDataAdapter("select sum(Quantity) from Sales_Delivery_Details where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and convert(varchar,Sales_Delivery_No) + '('+ convert(varchar,Sales_Delivery_Date,5) +')' = '" &
                                                    cbo_DCNo.Text & "' And Delivery_Purpose = 'Good'", con)
            End If

            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If Not IsDBNull(dt.Rows(0).Item(0)) Then
                    txt_Quantity.Text = dt.Rows(0).Item(0).ToString
                End If

            End If

            '--------

            If Len(Trim(cbo_Colour.Text)) > 0 Then
                da = New SqlClient.SqlDataAdapter("select sum(Quantity) from Sales_Details where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and  Job_No = '" & cbo_JobNumber.Text & "' and DC_No = '" &
                                                cbo_DCNo.Text & "' and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) &
                                                "  and Component_IdNo = " & Common_Procedures.Component_NameToIdNo(con, cbo_Component.Text) & " AND not Sales_Code = '" & NewCode & "'", con)
            Else
                da = New SqlClient.SqlDataAdapter("select sum(Quantity) from Sales_Details where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and DC_No = '" &
                                                cbo_DCNo.Text & "' and not Sales_Code = '" & NewCode & "'", con)
            End If

            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If Not IsDBNull(dt.Rows(0).Item(0)) Then
                    txt_Quantity.Text = Val(txt_Quantity.Text) - Val(dt.Rows(0).Item(0).ToString)
                End If

            End If

        End If

        For I As Int16 = 0 To dgv_Details.Rows.Count - 1
            If Val(txt_SlNo.Text) <> Val(dgv_Details.Rows(I).Cells(0).Value) And cbo_OrderCode.Text = dgv_Details.Rows(I).Cells(21).Value And
                cbo_JobNumber.Text = dgv_Details.Rows(I).Cells(24).Value And cbo_Colour.Text = dgv_Details.Rows(I).Cells(19).Value And
                cbo_Component.Text = dgv_Details.Rows(I).Cells(25).Value And cbo_DCNo.Text = dgv_Details.Rows(I).Cells(3).Value Then
                txt_Quantity.Text = Val(txt_Quantity.Text) - Val(dgv_Details.Rows(I).Cells(7).Value)
            End If
        Next

    End Sub

    Private Sub cbo_TaxType_TextChanged(sender As Object, e As EventArgs) Handles cbo_TaxType.TextChanged

        Try
            If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
                get_Item_Tax(True)
                cbo_TaxType.Tag = cbo_TaxType.Text
                Amount_Calculation(True)
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub pnl_Back_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Back.Paint

    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub lbl_NetAmount_Click(sender As Object, e As EventArgs) Handles lbl_NetAmount.Click

    End Sub

    Private Sub lbl_NetAmount_TextChanged(sender As Object, e As EventArgs) Handles lbl_NetAmount.TextChanged
        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(lbl_NetAmount.Text) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If
    End Sub

    Private Sub cbo_OrderCode_TextChanged(sender As Object, e As EventArgs) Handles cbo_OrderCode.TextChanged

    End Sub

    Private Sub cbo_JobNumber_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_JobNumber.SelectedIndexChanged

    End Sub

    Private Sub cbo_JobNumber_LostFocus(sender As Object, e As EventArgs) Handles cbo_JobNumber.LostFocus


        QuantityDetails()

        'For i As Int16 = 0 To dgv_Details.Rows.Count - 1
        '    If dgv_Details.Rows(i).Cells(21).Value = cbo_OrderCode.Text And dgv_Details.Rows(i).Cells(24).Value = cbo_JobNumber.Text And UCase(dgv_Details.Rows(i).Cells(3).Value) = "ALL" Then
        '        cbo_JobNumber.Text = ""
        '        MsgBox("ALL D.C NUMBERS PERTAINING TO THIS JOB NUMBER / ORDER CODE HAVE BEEN INCLUDED IN THIS INVOICE. INVALID ENTRY", vbOK, "INVALID ENTRY")
        '        txt_Quantity.Text = "0"
        '        cbo_JobNumber.Focus()
        '        Exit Sub
        '    End If
        'Next

    End Sub

    Private Sub txt_Quantity_GotFocus(sender As Object, e As EventArgs) Handles txt_Quantity.GotFocus

        QuantityDetails()

    End Sub

    Private Sub cbo_Ledger_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Ledger.SelectedIndexChanged

    End Sub

    Private Sub cbo_PaymentMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_PaymentMethod.SelectedIndexChanged

    End Sub

    Private Sub o1_TextChanged(sender As Object, e As EventArgs) Handles o1.TextChanged
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(o1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(o2.Text)
    End Sub

    Private Sub o2_TextChanged(sender As Object, e As EventArgs) Handles o2.TextChanged
        b1.Text = Val(t1.Text) - Val(s1.Text) - Val(o1.Text)
        b2.Text = Val(t2.Text) - Val(s2.Text) - Val(o2.Text)
    End Sub

    Private Sub cbo_JobNumber_Validated(sender As Object, e As EventArgs) Handles cbo_JobNumber.Validated

    End Sub

    Private Sub cbo_OrderCode_GiveFeedback(sender As Object, e As GiveFeedbackEventArgs) Handles cbo_OrderCode.GiveFeedback

    End Sub

    Private Sub cbo_OrderCode_Enter(sender As Object, e As EventArgs) Handles cbo_OrderCode.Enter
        cbo_Buff = cbo_OrderCode.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Order_Program_head", "Ordercode_forSelection",
                                "(Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, "") &
                                " OR Billing_Name_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & IIf(Len(Order_Disp_Cond) > 0, " and " & Order_Disp_Cond, "") & ")", "")
    End Sub

    Private Sub cbo_OrderCode_Leave(sender As Object, e As EventArgs) Handles cbo_OrderCode.Leave

        If cbo_OrderCode.Text <> cbo_Buff Then


            lbl_Design.Text = ""
            txt_Noof_Stitches.Text = ""
            txt_RAte_1000Stitches.Text = ""
            txt_Rate.Text = ""
            cbo_JobNumber.Text = ""
            cbo_DCNo.Text = ""
            cbo_Colour.Text = ""
            Picture_Box.BackgroundImage = Nothing
            cbo_Component.Text = ""
            txt_UoM.Text = "PCS-PIECES"

        End If

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        'Dim gstrate As Double
        Dim lED_iD As Integer = 0
        Ord_No = ""
        Ord_Date = ""

        If Trim(UCase(cbo_OrderCode.Text)) <> "" Then

            da = New SqlClient.SqlDataAdapter(" Select a.*, B.Rate_Stitches As Tot_Stitches, b.Finalised_Rate as Rate_Per_Piece from Order_Program_Head " &
                                              " a left outer Join Sales_Quotation_Head b on a.OrderCode_forSelection = b.UID  where " &
                                              " a.Ordercode_forSelection = '" & Trim(cbo_OrderCode.Text) & "'", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0)("Design").ToString) = False Then
                    lbl_Design.Text = dt.Rows(0)("Design").ToString
                End If

                If IsDBNull(dt.Rows(0)("StchsPr_Pcs").ToString) = False Then
                    txt_Noof_Stitches.Text = dt.Rows(0)("StchsPr_Pcs").ToString
                End If

                If IsDBNull(dt.Rows(0)("Tot_Stitches").ToString) = False Then
                    txt_RAte_1000Stitches.Text = dt.Rows(0)("Tot_Stitches").ToString
                End If

                If IsDBNull(dt.Rows(0)("Rate_Per_Piece").ToString) = False Then
                    txt_Rate.Text = dt.Rows(0)("Rate_Per_Piece").ToString
                End If

                If IsDBNull(dt.Rows(0)("Unit_IdNo")) = False Then
                    If dt.Rows(0)("Unit_IdNo") > 0 Then
                        txt_UoM.Text = Common_Procedures.Unit_IdNoToName(con, dt.Rows(0)("Unit_IdNo").ToString)
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("Order_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(dt.Rows(0).Item("Order_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                Picture_Box.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If

            End If

            dt.Dispose()
            da.Dispose()

        End If


        QuantityDetails()
    End Sub

    Private Sub cbo_JobNumber_Enter(sender As Object, e As EventArgs) Handles cbo_JobNumber.Enter

        cbo_Buff = cbo_OrderCode.Text

        Try

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "OrderJobNo_Head", "OrderJobNo_Name", " OrderNo_Name = '" & cbo_OrderCode.Text & "'", "")


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_JobNumber_Leave(sender As Object, e As EventArgs) Handles cbo_JobNumber.Leave

        If cbo_JobNumber.Text <> cbo_Buff Then

            cbo_DCNo.Text = ""
            cbo_Colour.Text = ""
            cbo_Component.Text = ""

        End If

    End Sub

    Private Sub cbo_DCNo_Enter(sender As Object, e As EventArgs) Handles cbo_DCNo.Enter

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cbo_Buff = cbo_OrderCode.Text

        Try

            Dim CMD As New SqlClient.SqlCommand
            CMD.Connection = con

            CMD.CommandText = "truncate table Combo_Pop_Temp"
            CMD.ExecuteNonQuery()

            CMD.CommandText = " INSERT INTO Combo_Pop_Temp SELECT convert(varchar,Sales_Delivery_No)+'('+CONVERT(varchar,sales_delivery_date,5)+')' FROM Sales_Delivery_Details  " &
                              " WHERE  not Sales_Delivery_No + '('+ convert(varchar,sales_delivery_date,5)+')' + Job_No + OrderCode_forSelection + convert(varchar,Colour_IdNo) + convert(varchar,Component_IdNo) in " &
                              " (Select Sales_DC_Code + Job_NO + uid + convert(varchar,ISNULL(Colour_IdNo,0)) + convert(varchar,ISNULL(Component_IdNo,0))  " &
                              " from Invoice_DC_Details) " &
                              " And OrderCode_forSelection in (Select OrderCode_forSelection from Order_Program_Head Where Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) &
                              " Or Billing_Name_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text) & ")" &
                              " And Job_No = '" & cbo_JobNumber.Text & "' AND OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) &
                              " and Component_IdNo = " & Common_Procedures.Component_NameToIdNo(con, cbo_Component.Text) & " and Sales_Delivery_Date <= '" & Format(dtp_CutOffDate.Value, "dd/MMM/yyyy") & "'"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "INSERT INTO Combo_Pop_Temp SELECT DC_NO FROM SALES_DETAILS WHERE SALES_CODE = '" & NewCode & "'"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "INSERT INTO Combo_Pop_Temp VALUES('ALL')"
            CMD.ExecuteNonQuery()

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Combo_Pop_Temp", "LOV", "", "")

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_DCNo_Leave(sender As Object, e As EventArgs) Handles cbo_DCNo.Leave

        'If cbo_DCNo.Text = "ALL" Then

        '    For i As Int16 = 0 To dgv_Details.Rows.Count - 1
        '        If dgv_Details.Rows(i).Cells(21).Value = cbo_OrderCode.Text And dgv_Details.Rows(i).Cells(24).Value = cbo_JobNumber.Text Then
        '            MsgBox("'ALL' OR FEW D.C NUMBERS PERTAINING TO THIS JOB NUMBER / ORDER CODE HAVE BEEN INCLUDED IN THIS INVOICE ALREADY . INVALID ENTRY", vbOK, "INVALID ENTRY")
        '            cbo_DCNo.Text = ""
        '            cbo_DCNo.Focus()
        '            Exit Sub
        '        End If
        '    Next

        'End If

        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable


        If cbo_DCNo.Text = "ALL" Then

            DCCODES = ""
            DCCODES1 = ""
            da = New SqlClient.SqlDataAdapter("Select distinct LOV from Combo_Pop_Temp WHERE NOT LOV = 'ALL'", con)

            da.Fill(dt1)

            For i As Integer = 0 To dt1.Rows.Count - 1

                If Len(DCCODES) > 0 Then
                    DCCODES = DCCODES + "$$$"
                    DCCODES1 = DCCODES1 + ","
                End If
                DCCODES = DCCODES + dt1.Rows(i).Item(0)
                DCCODES1 = DCCODES1 + "'" + dt1.Rows(i).Item(0) + "'"

            Next

        Else

            DCCODES = cbo_DCNo.Text
            DCCODES1 = "'" + cbo_DCNo.Text + "'"

        End If

        If Len(Trim(DCCODES1)) = 0 Then
            DCCODES1 = "''"
        End If

        QuantityDetails()

    End Sub

    Private Sub cbo_Ledger_Enter(sender As Object, e As EventArgs) Handles cbo_Ledger.Enter

        cbo_Buff = cbo_Ledger.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_Leave(sender As Object, e As EventArgs) Handles cbo_Ledger.Leave

        If Len(Trim(cbo_Buff)) <> 0 And cbo_Ledger.Text <> cbo_Buff Then

            Dim c As Integer = MsgBox("Changing the party Name will CLEAR all 'DETAILS' entered. Continue to Change Party ?", vbYesNo, "Change Party ?")

            If c = vbNo Then
                cbo_Ledger.Text = cbo_Buff
            End If

            If c = vbYes Then

                dgv_Details.Rows.Clear()
                dgv_Details_Total.Rows.Clear()
                cbo_OrderCode.Text = ""
                txt_Noof_Stitches.Text = ""
                txt_RAte_1000Stitches.Text = ""
                txt_Quantity.Text = ""
                txt_Rate.Text = ""
                lbl_Amount.Text = ""
                lbl_Grid_DiscPerc.Text = ""
                lbl_Grid_DiscAmount.Text = ""
                lbl_Grid_AssessableValue.Text = ""
                lbl_Grid_HsnCode.Text = ""
                cbo_DCNo.Text = ""
                cbo_JobNumber.Text = ""
                lbl_Design.Text = ""

                DCCODES = ""
                DCCODES1 = ""
                txt_OrderNo.Text = ""

            End If

        End If

        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            Amount_Calculation(True)
        End If
    End Sub



    Private Sub cbo_Colour_Enter(sender As Object, e As EventArgs) Handles cbo_Colour.Enter

        cbo_Buff = cbo_Colour.Text

        Try

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", " Colour_IdNo in (Select Colour_IdNo from Sales_Delivery_Details Where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and Delivery_Purpose = 'Good')", "")


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Colour_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Colour.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_JobNumber, cbo_Component, "Colour_Head", "Colour_Name", "  Colour_IdNo in (Select Colour_IdNo from Sales_Delivery_Details Where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and Delivery_Purpose = 'Good')", "")

    End Sub

    Private Sub cbo_Colour_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Colour.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, cbo_Component, "Colour_Head", "Colour_Name", "  Colour_IdNo in (Select Colour_IdNo from Sales_Delivery_Details Where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and Delivery_Purpose = 'Good')", "")

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_JobNumber.Text) <> "" Then
                cbo_Component.Focus()
            Else
                txt_CashDiscPerc.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Colour_Leave(sender As Object, e As EventArgs) Handles cbo_Colour.Leave

        If cbo_Colour.Text <> cbo_Buff Then

            cbo_Component.Text = ""
            cbo_DCNo.Text = ""

        End If

    End Sub



    Private Sub ComboBox1_Enter(sender As Object, e As EventArgs) Handles cbo_Component.Enter

        cbo_Buff = cbo_Component.Text

        Try

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Component_Head", "Component_Name", " Component_IdNo in (Select Component_IdNo from Sales_Delivery_Details Where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) & " and Delivery_Purpose = 'Good')", "")


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Component_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Component.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Component, cbo_Colour, cbo_DCNo, "Component_Head", "Component_Name", " Component_IdNo in (Select Component_IdNo from Sales_Delivery_Details Where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) & " and Delivery_Purpose = 'Good')", "")

    End Sub


    Private Sub cbo_Component_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Component.KeyPress


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Component, cbo_DCNo, "Component_Head", "Component_Name", " Component_IdNo in (Select Component_IdNo from Sales_Delivery_Details Where OrderCode_forSelection = '" & cbo_OrderCode.Text & "' and Job_No = '" & cbo_JobNumber.Text & "' and Colour_IdNo = " & Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text) & " and Delivery_Purpose = 'Good')", "")

    End Sub

    Private Sub cbo_Colour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Colour.SelectedIndexChanged

    End Sub

    Private Sub cbo_Component_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Component.SelectedIndexChanged

    End Sub

    Private Sub cbo_Component_Leave(sender As Object, e As EventArgs) Handles cbo_Component.Leave

        If cbo_Component.Text <> cbo_Buff Then

            cbo_DCNo.Text = ""

        End If
    End Sub

    Private Sub cbo_OrderCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_OrderCode.SelectedIndexChanged

    End Sub

End Class
