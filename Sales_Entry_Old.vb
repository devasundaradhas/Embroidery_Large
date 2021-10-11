Public Class Sales_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PURCH-"
    Private cmbItmNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer

    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False

        lbl_PurchaseNo.Text = ""
        lbl_PurchaseNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lb_AvailableStock.Text = ""
        lbl_AmountInWords.Text = "Amount In Words : "

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

        dgv_Details.Rows.Clear()

        cbo_PaymentMethod.Text = "CREDIT"
        cbo_TaxType.Text = "NO TAX"
        txt_SlNo.Text = "1"
        lb_AvailableStock.Text = ""

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer

        If Trim(no) = "" Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Ledger_Name as PurchaseAcName from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.PurchaseAc_IdNo = c.Ledger_IdNo where a.Purchase_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_PurchaseNo.Text = dt1.Rows(0).Item("Purhase_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Purhase_Date").ToString
                cbo_PaymentMethod.Text = dt1.Rows(0).Item("Payment_Method").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                cbo_PurchaseAc.Text = dt1.Rows(0).Item("PurchaseAcName").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_TotalQty.Text = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                txt_SubTotal.Text = Format(Val(dt1.Rows(0).Item("Sub_Total").ToString), "########0.00")
                txt_DiscountAmount.Text = Format(Val(dt1.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00")
                txt_TotalTaxAmount.Text = Format(Val(dt1.Rows(0).Item("Total_TaxAmount").ToString), "########0.00")
                txt_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_CashDiscPerc.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Perc").ToString), "########0.00")
                txt_CashDiscAmount.Text = Format(Val(dt1.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00")
                txt_AddLessAmount.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                txt_RoundOff.Text = Format(Val(dt1.Rows(0).Item("Round_Off").ToString), "########0.00")
                txt_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")

                da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        dgv_Details.Rows(n).Cells(0).Value = i + 1
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Unit_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Noof_Items").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Tax_Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Discount_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Discount_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Tax_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Tax_Amount").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")

                    Next i

                End If

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Purchase_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    Dim da As SqlClient.SqlDataAdapter
                    Dim dt1 As New DataTable

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    'MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Exit Sub

                End If

                FrmLdSTS = False
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Purchase_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select ledger_name from ledger_head where (AccountsGroup_IdNo = 3 or AccountsGroup_IdNo = 4)  order by ledger_name", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "ledger_name"

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"

        da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        da.Fill(dt3)
        cbo_Unit.DataSource = dt3
        cbo_Unit.DisplayMember = "unit_name"

        da = New SqlClient.SqlDataAdapter("select ledger_name from ledger_head where AccountsGroup_IdNo = 5 order by ledger_name", con)
        da.Fill(dt4)
        cbo_PurchaseAc.DataSource = dt4
        cbo_PurchaseAc.DisplayMember = "ledger_name"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Purchase_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub Purchase_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If pnl_Filter.Visible = True Then
                btn_Filter_Close_Click(sender, e)
                Exit Sub
            End If

            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            Dim f As New Company_Selection
            f.ShowDialog()

            If Val(Common_Procedures.CompIdNo) <> 0 Then

                Dim da As SqlClient.SqlDataAdapter
                Dim dt1 As New DataTable

                da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                        lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                        Me.Text = Trim(dt1.Rows(0)(1).ToString)
                    End If
                End If
                dt1.Clear()
                dt1.Dispose()
                da.Dispose()

                new_record()

            Else

                Me.Close()
            End If

        End If
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Purchase_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purhase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purhase_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select ledger_name from ledger_head where (AccountsGroup_IdNo = 3 or AccountsGroup_IdNo = 4)  order by ledger_name", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "ledger_name"

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
            cmd.CommandText = "select Purhase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Purhase_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_PurchaseNo.Text))

            da = New SqlClient.SqlDataAdapter("select Purhase_No from Purchase_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Purhase_No", con)
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
            cmd.CommandText = "select Purhase_No from Purchase_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Purhase_No desc"

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
        Dim da As New SqlClient.SqlDataAdapter("select Purhase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Purhase_No desc", con)
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

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_PurchaseNo.Text = NewID
            lbl_PurchaseNo.ForeColor = Color.Red


            da = New SqlClient.SqlDataAdapter("select a.Payment_Method, b.ledger_name as PurchaseAcName, a.Tax_Type from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Purhase_No desc", con)
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Purhase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
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
            cmd.CommandText = "select Purhase_No from Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
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
        Dim purcac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sno As Integer = 0

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

        da = New SqlClient.SqlDataAdapter("select ledger_idno from ledger_head where ledger_name = '" & Trim(cbo_Ledger.Text) & "'", con)
        da.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                led_id = Val(dt1.Rows(0)(0).ToString)
            End If
        End If

        dt1.Clear()

        If led_id = 0 And Trim(UCase(cbo_PaymentMethod.Text)) = "CREDIT" Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select ledger_idno from ledger_head where ledger_name = '" & Trim(cbo_PurchaseAc.Text) & "'", con)
        da.Fill(dt2)

        If dt2.Rows.Count > 0 Then
            If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                purcac_id = Val(dt2.Rows(0)(0).ToString)
            End If
        End If

        If purcac_id = 0 And Val(txt_NetAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

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

            If New_Entry = True Then

                cmd.CommandText = "Insert into Purchase_Head(Purchase_Code, Company_IdNo, Purhase_No, for_OrderBy, Purhase_Date, Payment_Method, Ledger_IdNo, PurchaseAc_IdNo, Tax_Type, Narration, Total_Qty, Sub_Total, Total_DiscountAmount, Total_TaxAmount, Gross_Amount, CashDiscount_Perc, CashDiscount_Amount, AddLess_Amount, Round_Off, Net_Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(purcac_id)) & ", '" & Trim(cbo_TaxType.Text) & "', '" & Trim(txt_Narration.Text) & "', " & Str(Val(txt_TotalQty.Text)) & ", " & Str(Val(txt_SubTotal.Text)) & ", " & Str(Val(txt_TotalDiscAmount.Text)) & ", " & Str(Val(txt_TotalTaxAmount.Text)) & ", " & Str(Val(txt_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_AddLessAmount.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(txt_NetAmount.Text)) & ")"
                'cmd.CommandText = "Insert into Purchase_Head(Purchase_Code, Company_IdNo, Purhase_No, for_OrderBy, Purhase_Date, Payment_Method, Ledger_IdNo, PurchaseAc_IdNo, Tax_Type, Narration, Total_Qty, Sub_Total, Total_DiscountAmount, Total_TaxAmount, Gross_Amount, CashDiscount_Perc, CashDiscount_Amount, AddLess_Amount, Round_Off, Net_Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(lbl_PurchaseNo.Text)) & ", '" & Trim(Format(PurcDate, "mm/dd/yyyy")) & "', '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(purcac_id)) & ", '" & Trim(cbo_TaxType.Text) & "', '" & Trim(txt_Narration.Text) & "', " & Str(Val(txt_TotalQty.Text)) & ", " & Str(Val(txt_SubTotal.Text)) & ", " & Str(Val(txt_TotalDiscAmount.Text)) & ", " & Str(Val(txt_TotalTaxAmount.Text)) & ", " & Str(Val(txt_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_AddLessAmount.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(txt_NetAmount.Text)) & ")"
                'cmd.CommandText = "Insert into Purchase_Head(Purchase_Code, Company_IdNo, Purhase_No, for_OrderBy, Purhase_Date, Payment_Method, Ledger_IdNo, PurchaseAc_IdNo, Tax_Type, Narration, Total_Qty, Sub_Total, Total_DiscountAmount, Total_TaxAmount, Gross_Amount, CashDiscount_Perc, CashDiscount_Amount, AddLess_Amount, Round_Off, Net_Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(lbl_PurchaseNo.Text)) & ", '" & dtp_Date.Value.Date & "', '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(purcac_id)) & ", '" & Trim(cbo_TaxType.Text) & "', '" & Trim(txt_Narration.Text) & "', " & Str(Val(txt_TotalQty.Text)) & ", " & Str(Val(txt_SubTotal.Text)) & ", " & Str(Val(txt_TotalDiscAmount.Text)) & ", " & Str(Val(txt_TotalTaxAmount.Text)) & ", " & Str(Val(txt_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_AddLessAmount.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(txt_NetAmount.Text)) & ")"
                'cmd.CommandText = "Insert into Purchase_Head(Purchase_Code, Company_IdNo, Purhase_No, for_OrderBy, Purhase_Date, Payment_Method, Ledger_IdNo, PurchaseAc_IdNo, Tax_Type, Narration, Total_Qty, Sub_Total, Total_DiscountAmount, Total_TaxAmount, Gross_Amount, CashDiscount_Perc, CashDiscount_Amount, AddLess_Amount, Round_Off, Net_Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(lbl_PurchaseNo.Text)) & ", '" & Trim(Format(Convert.ToDateTime(dtp_Date.Text), "mm/dd/yyyy")) & "', '" & Trim(cbo_PaymentMethod.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(purcac_id)) & ", '" & Trim(cbo_TaxType.Text) & "', '" & Trim(txt_Narration.Text) & "', " & Str(Val(txt_TotalQty.Text)) & ", " & Str(Val(txt_SubTotal.Text)) & ", " & Str(Val(txt_TotalDiscAmount.Text)) & ", " & Str(Val(txt_TotalTaxAmount.Text)) & ", " & Str(Val(txt_GrossAmount.Text)) & ", " & Str(Val(txt_CashDiscPerc.Text)) & ", " & Str(Val(txt_CashDiscAmount.Text)) & ", " & Str(Val(txt_AddLessAmount.Text)) & ", " & Str(Val(txt_RoundOff.Text)) & ", " & Str(Val(txt_NetAmount.Text)) & ")"

                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Purchase_Head set Purhase_Date = @PurchaseDate, Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", PurchaseAc_IdNo = " & Str(Val(purcac_id)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Narration = '" & Trim(txt_Narration.Text) & "', Total_Qty = " & Str(Val(txt_TotalQty.Text)) & ", Sub_Total = " & Str(Val(txt_SubTotal.Text)) & ", Total_DiscountAmount = " & Str(Val(txt_DiscountAmount.Text)) & ", Total_TaxAmount = " & Str(Val(txt_TotalTaxAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_DiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_DiscountAmount.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLessAmount.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(txt_NetAmount.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
                'cmd.CommandText = "Update Purchase_Head set Purhase_Date = '" & Trim(Format(dtp_Date.Value.Date, "mm/dd/yyyy")) & "', Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", PurchaseAc_IdNo = " & Str(Val(purcac_id)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Narration = '" & Trim(txt_Narration.Text) & "', Total_Qty = " & Str(Val(txt_TotalQty.Text)) & ", Sub_Total = " & Str(Val(txt_SubTotal.Text)) & ", Total_DiscountAmount = " & Str(Val(txt_DiscountAmount.Text)) & ", Total_TaxAmount = " & Str(Val(txt_TotalTaxAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_DiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_DiscountAmount.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLessAmount.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(txt_NetAmount.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
                'cmd.CommandText = "Update Purchase_Head set Purhase_Date = '" & Trim(Format(Convert.ToDateTime(dtp_Date.Text), "mm/dd/yyyy")) & "', Payment_Method = '" & Trim(cbo_PaymentMethod.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", PurchaseAc_IdNo = " & Str(Val(purcac_id)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Narration = '" & Trim(txt_Narration.Text) & "', Total_Qty = " & Str(Val(txt_TotalQty.Text)) & ", Sub_Total = " & Str(Val(txt_SubTotal.Text)) & ", Total_DiscountAmount = " & Str(Val(txt_DiscountAmount.Text)) & ", Total_TaxAmount = " & Str(Val(txt_TotalTaxAmount.Text)) & ", Gross_Amount = " & Str(Val(txt_GrossAmount.Text)) & ", CashDiscount_Perc = " & Str(Val(txt_DiscPerc.Text)) & ", CashDiscount_Amount = " & Str(Val(txt_DiscountAmount.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLessAmount.Text)) & ", Round_Off = " & Str(Val(txt_RoundOff.Text)) & ", Net_Amount = " & Str(Val(txt_NetAmount.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            For i = 0 To dgv_Details.RowCount - 1

                itm_id = 0
                unt_id = 0

                da = New SqlClient.SqlDataAdapter("select item_idno from item_head where item_name = '" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "'", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt3)

                If dt3.Rows.Count > 0 Then
                    If IsDBNull(dt3.Rows(0)(0).ToString) = False Then
                        itm_id = Val(dt3.Rows(0)(0).ToString)
                    End If
                End If

                dt3.Clear()

                If itm_id <> 0 And Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 And Val(dgv_Details.Rows(i).Cells(11).Value) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select unit_idno from unit_head where unit_name = '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "'", con)
                    da.SelectCommand.Transaction = tr
                    da.Fill(dt5)

                    If dt5.Rows.Count > 0 Then
                        If IsDBNull(dt5.Rows(0)(0).ToString) = False Then
                            unt_id = Val(dt5.Rows(0)(0).ToString)
                        End If
                    End If

                    dt5.Clear()

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Purchase_Details(Purchase_Code, Company_IdNo, Purhase_No, for_OrderBy, Purhase_Date, Ledger_IdNo, SL_No, Item_IdNo, Unit_IdNo, Noof_Items, Rate, Tax_Rate, Amount, Discount_Perc, Discount_Amount, Tax_Perc, Tax_Amount, Total_Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(3).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(8).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(9).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(10).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(11).Value)) & " )"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Party_Bill_No, SL_No, Item_IdNo, Unit_IdNo, Quantity) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_PurchaseNo.Text))) & ", @PurchaseDate, " & Str(Val(led_id)) & ", '" & Trim(lbl_PurchaseNo.Text) & "', " & Str(Val(Sno)) & ", " & Str(Val(itm_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(3).Value)) & " )"
                    cmd.ExecuteNonQuery()

                End If

            Next

            tr.Commit()

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus

        With cbo_ItemName
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_ItemName.Text.Length

            cmbItmNm = cbo_ItemName.Text
        End With
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        If Trim(cbo_ItemName.Text) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT ADD...", MessageBoxButtons.OK)
            Exit Sub
        End If

        If Val(txt_NoofItems.Text) = 0 Then
            MessageBox.Show("Invalid No.of Items", "DOES NOT ADD...", MessageBoxButtons.OK)
            Exit Sub
        End If

        If Val(txt_Rate.Text) = 0 And Val(txt_TaxRate.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT ADD...", MessageBoxButtons.OK)
            Exit Sub
        End If

        If Val(txt_Amount.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT ADD...", MessageBoxButtons.OK)
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_ItemName.Text
                    .Rows(i).Cells(2).Value = cbo_Unit.Text
                    .Rows(i).Cells(3).Value = Val(txt_NoofItems.Text)
                    .Rows(i).Cells(4).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(5).Value = Format(Val(txt_TaxRate.Text), "########0.00")
                    .Rows(i).Cells(6).Value = Format(Val(txt_SubAmount.Text), "########0.00")
                    .Rows(i).Cells(7).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                    .Rows(i).Cells(8).Value = Format(Val(txt_DiscountAmount.Text), "########0.00")
                    .Rows(i).Cells(9).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                    .Rows(i).Cells(10).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                    .Rows(i).Cells(11).Value = Format(Val(txt_Amount.Text), "########0.00")

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 10 Then .FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_ItemName.Text
                .Rows(n).Cells(2).Value = cbo_Unit.Text
                .Rows(n).Cells(3).Value = Val(txt_NoofItems.Text)
                .Rows(n).Cells(4).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(5).Value = Format(Val(txt_TaxRate.Text), "########0.00")
                .Rows(n).Cells(6).Value = Format(Val(txt_SubAmount.Text), "########0.00")
                .Rows(n).Cells(7).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                .Rows(n).Cells(8).Value = Format(Val(txt_DiscountAmount.Text), "########0.00")
                .Rows(n).Cells(9).Value = Format(Val(txt_TaxPerc.Text), "########0.00")
                .Rows(n).Cells(10).Value = Format(Val(txt_TaxAmount.Text), "########0.00")
                .Rows(n).Cells(11).Value = Format(Val(txt_Amount.Text), "########0.00")

                .Rows(n).Selected = True

                If n >= 10 Then .FirstDisplayedScrollingRowIndex = n - 9

            End If

        End With

        GrossAmount_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        txt_SubAmount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscountAmount.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_Amount.Text = ""

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub txt_NoofItems_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NoofItems.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_NoofItems_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofItems.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_NoofItems_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoofItems.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
        Amount_Calculation()
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_DiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        Call Amount_Calculation()
    End Sub

    Private Sub txt_TaxPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")

    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress

        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If

    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
        Amount_Calculation()
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        Try
            With cbo_ItemName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_SlNo.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Unit.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_ItemName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_ItemName
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
                        End With

                        If Trim(cbo_ItemName.Text) <> "" Then
                            SendKeys.Send("{TAB}")
                        Else
                            txt_CashDiscPerc.Focus()
                        End If

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

                        If Trim(FindStr) <> "" Then
                            Condt = " Where item_name like '" & Trim(FindStr) & "%' or item_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select item_name from item_head " & Condt & " order by item_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "item_name"

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

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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
                cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
                txt_TaxRate.Text = dt.Rows(0)("Sale_TaxRate").ToString
                txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
            End If
            dt.Dispose()
            da.Dispose()
        End If

        cbo_ItemName.BackColor = Color.White
        cbo_ItemName.ForeColor = Color.Black

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        'If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_PaymentMethod.Focus()
        End If
    End Sub

    Private Sub cbo_PaymentMethod_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.GotFocus
        With cbo_PaymentMethod
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_PaymentMethod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentMethod.KeyDown
        Try
            With cbo_PaymentMethod
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    dtp_Date.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Ledger.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_PaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentMethod.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_PaymentMethod

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .GetItemText(.SelectedItem)
                                    '.Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        cbo_Ledger.Focus()
                        'SendKeys.Send("{TAB}")

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

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx

                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        With cbo_Ledger
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        With cbo_Ledger
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub


    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            With cbo_Ledger
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_PaymentMethod.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_PurchaseAc.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Ledger

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

                        cbo_PurchaseAc.Focus()

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
                            Condt = " Where ledger_name like '" & FindStr & "%' or ledger_name like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select ledger_name from ledger_head " & Condt & " order by ledger_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "ledger_name"

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

    Private Sub cbo_PurchaseAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.GotFocus
        With cbo_PurchaseAc
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        Try
            With cbo_PurchaseAc
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_Ledger.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_TaxType.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_PurchaseAc

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

                        cbo_TaxType.Focus()

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

                        Condt = " Where AccountsGroup_IdNo = 5 "
                        If Trim(FindStr) <> "" Then
                            Condt = " where AccountsGroup_IdNo = 5 and (ledger_name like '" & FindStr & "%' or ledger_name like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select ledger_name from ledger_head " & Condt & " order by ledger_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "ledger_name"

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

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        With cbo_TaxType
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Try
            With cbo_TaxType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_PurchaseAc.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_SlNo.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        With cbo_TaxType

            If Asc(e.KeyChar) <> 27 Then

                If Asc(e.KeyChar) = 13 Then

                    If Trim(.Text) <> "" Then
                        If .DroppedDown = True Then

                            FindStr = LTrim(.Text)

                            indx = .FindString(FindStr)

                            If indx <> -1 Then
                                .SelectedText = ""
                                .SelectedIndex = indx
                                .SelectionStart = FindStr.Length
                                .SelectionLength = .Text.Length
                            End If

                        End If

                    End If

                    txt_SlNo.Focus()

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

                    indx = .FindString(FindStr)

                    If indx <> -1 Then
                        .SelectedText = ""
                        .SelectedIndex = indx
                        .SelectionStart = FindStr.Length
                        .SelectionLength = .Text.Length
                        e.Handled = True

                    Else
                        e.Handled = True

                    End If

                End If

            End If

        End With

    End Sub

    Private Sub txt_AddLessAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLessAmount.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_AddLessAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLessAmount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_AddLessAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLessAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CashDiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CashDiscPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_ItemName.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CashDiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDiscPerc.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_CashDiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CashDiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_GrossAmount.TextChanged
        NetAmount_Calculation()
    End Sub


    Private Sub txt_SlNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SlNo.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        cbo_ItemName.Text = Trim(.Rows(i).Cells(1).Value)
                        cbo_Unit.Text = Trim(.Rows(i).Cells(2).Value)
                        txt_NoofItems.Text = Val(.Rows(i).Cells(3).Value)
                        txt_Rate.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.00")
                        txt_TaxRate.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.00")
                        txt_SubAmount.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.00")
                        txt_DiscPerc.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.00")
                        txt_DiscountAmount.Text = Format(Val(.Rows(i).Cells(8).Value), "########0.00")
                        txt_TaxPerc.Text = Format(Val(.Rows(i).Cells(9).Value), "########0.00")
                        txt_TaxAmount.Text = Format(Val(.Rows(i).Cells(10).Value), "########0.00")
                        txt_Amount.Text = Format(Val(.Rows(i).Cells(11).Value), "########0.00")

                        Exit For

                    End If

                Next

            End With

            SendKeys.Send("{TAB}")

        End If
    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        With cbo_Unit
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        Try
            With cbo_Unit
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_ItemName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_NoofItems.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Unit

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

                        txt_NoofItems.Focus()

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

                        If Trim(FindStr) <> "" Then
                            Condt = " Where ledger_name like '" & FindStr & "%' or ledger_name like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select ledger_name from ledger_head " & Condt & " order by ledger_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "ledger_name"

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

    Private Sub txt_TaxRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")

    End Sub

    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyUp
        txt_Rate.Text = Format(Val(txt_TaxRate.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
        Amount_Calculation()
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
    End Sub


    Private Sub Amount_Calculation()

        txt_SubAmount.Text = Val(txt_NoofItems.Text) * Val(txt_Rate.Text)
        txt_DiscountAmount.Text = Format(Val(txt_SubAmount.Text) * Val(txt_DiscPerc.Text) / 100, "#########0.00")

        txt_TaxAmount.Text = "0.00"
        If Trim(cbo_TaxType.Text) <> "" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
            txt_TaxAmount.Text = Format((Val(txt_SubAmount.Text) - Val(txt_DiscountAmount.Text)) * Val(txt_TaxPerc.Text) / 100, "#########0.00")
        End If

        txt_Amount.Text = Format(Val(txt_SubAmount.Text) - Val(txt_DiscountAmount.Text) + Val(txt_TaxAmount.Text), "########0.00")


    End Sub

    Private Sub GrossAmount_Calculation()
        Dim Sno As Integer
        Dim TotQty As Decimal, TotSubAmt As Decimal, TotDiscAmt As Decimal, TotTxAmt As Decimal, TotAmt As Decimal

        Sno = 0
        TotQty = 0
        TotSubAmt = 0
        TotDiscAmt = 0
        TotTxAmt = 0
        TotAmt = 0
        For i = 0 To dgv_Details.RowCount - 1
            Sno = Sno + 1
            dgv_Details.Rows(i).Cells(0).Value = Sno

            TotQty = TotQty + Val(dgv_Details.Rows(i).Cells(3).Value)
            TotSubAmt = TotSubAmt + Val(dgv_Details.Rows(i).Cells(6).Value)
            TotDiscAmt = TotDiscAmt + Val(dgv_Details.Rows(i).Cells(8).Value)
            TotTxAmt = TotTxAmt + Val(dgv_Details.Rows(i).Cells(10).Value)
            TotAmt = TotAmt + Val(dgv_Details.Rows(i).Cells(11).Value)

        Next

        txt_TotalQty.Text = Val(TotQty)
        txt_SubTotal.Text = Format(TotSubAmt, "########0.00")
        txt_TotalDiscAmount.Text = Format(TotDiscAmt, "########0.00")
        txt_TotalTaxAmount.Text = Format(TotTxAmt, "########0.00")
        txt_GrossAmount.Text = Format(TotAmt, "########0.00")

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        txt_CashDiscAmount.Text = Format(Val(txt_GrossAmount.Text) * Val(txt_CashDiscPerc.Text) / 100, "#########0.00")

        NtAmt = Val(txt_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLessAmount.Text)

        txt_RoundOff.Text = Format(Format(Val(NtAmt), "#########0") - Val(NtAmt), "#########0.00")

        txt_NetAmount.Text = Format(Val(txt_GrossAmount.Text) - Val(txt_CashDiscAmount.Text) + Val(txt_AddLessAmount.Text) + Val(txt_RoundOff.Text), "########0.00")

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Purchase_Code = '" & Trim(NewCode) & "'", con)
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
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
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

        da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Purchase_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Purchase_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
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
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 10, PageWidth, CurY + 10)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO.    :  " & prn_HdDt.Rows(0).Item("Purhase_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE    :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Purhase_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY + 15, 0, 0, pFont)

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
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Sub_Total").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_DiscountAmount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
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
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
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
                Condt = "a.Purhase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Purhase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Purhase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                da = New SqlClient.SqlDataAdapter("select ledger_idno from ledger_head where ledger_name = '" & Trim(cbo_Filter_PartyName.Text) & "'", con)
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        Led_IdNo = Val(dt1.Rows(0)(0).ToString)
                    End If
                End If

                dt1.Clear()
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

            da = New SqlClient.SqlDataAdapter("select a.Purhase_No, a.Purhase_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Purchase_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Purhase_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Purhase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Purhase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
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

                If Trim(FindStr) <> "" Then
                    Condt = " Where ledger_name like '" & Trim(FindStr) & "%' or ledger_name like '% " & Trim(FindStr) & "%' "
                End If

                da = New SqlClient.SqlDataAdapter("select ledger_name from ledger_head " & Condt & " order by ledger_name", con)
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "ledger_name"

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

    Private Sub cbo_PaymentMethod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentMethod.LostFocus
        With cbo_PaymentMethod
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_PurchaseAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_PurchaseAc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.LostFocus
        With cbo_PurchaseAc
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.LostFocus
        With cbo_TaxType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Unit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.LostFocus
        With cbo_Unit
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            cbo_ItemName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
            cbo_Unit.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            txt_NoofItems.Text = Val(dgv_Details.CurrentRow.Cells(3).Value)
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(4).Value), "########0.00")
            txt_TaxRate.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.00")
            txt_SubAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")
            txt_DiscPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.00")
            txt_DiscountAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(8).Value), "########0.00")
            txt_TaxPerc.Text = Format(Val(dgv_Details.CurrentRow.Cells(9).Value), "########0.00")
            txt_TaxAmount.Text = Format(Val(dgv_Details.CurrentRow.Cells(10).Value), "########0.00")
            txt_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(11).Value), "########0.00")

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
        cbo_Unit.Text = ""
        txt_NoofItems.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        txt_SubAmount.Text = ""
        txt_DiscPerc.Text = ""
        txt_DiscountAmount.Text = ""
        txt_TaxPerc.Text = ""
        txt_TaxAmount.Text = ""
        txt_Amount.Text = ""

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
            txt_NoofItems.Text = ""
            txt_Rate.Text = ""
            txt_TaxRate.Text = ""
            txt_SubAmount.Text = ""
            txt_DiscPerc.Text = ""
            txt_DiscountAmount.Text = ""
            txt_TaxPerc.Text = ""
            txt_TaxAmount.Text = ""
            txt_Amount.Text = ""

            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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
        Dim TotAmt As Single

        For i = 0 To dgv_Details.RowCount - 1
            SubAmt = Val(dgv_Details.Rows(i).Cells(6).Value)
            DiscAmt = Val(dgv_Details.Rows(i).Cells(8).Value)
            TxPerc = Val(dgv_Details.Rows(i).Cells(9).Value)

            TxAmt = 0
            If Trim(cbo_TaxType.Text) <> "" And Trim(UCase(cbo_TaxType.Text)) <> "NO TAX" Then
                TxAmt = Format((Val(SubAmt) - Val(DiscAmt)) * Val(TxPerc) / 100, "#########0.00")
            End If

            TotAmt = Val(SubAmt) - Val(DiscAmt) + Val(TxAmt)

            dgv_Details.Rows(i).Cells(10).Value = Trim(Format(Val(TxAmt), "#########0.00"))
            dgv_Details.Rows(i).Cells(11).Value = Trim(Format(Val(TotAmt), "#########0.00"))

        Next

        GrossAmount_Calculation()

    End Sub

End Class