Public Class Voucher_Entry

    Implements Interface_MDIActions

    Private Structure VoucherEntry_AmountDetails
        Dim LedgerIdNo As Integer
        Dim VoucherAmount As Double
    End Structure
    Private VouAmtAr(10) As VoucherEntry_AmountDetails

    Private Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal vKey As Int32) As UShort

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Mov_Status As Boolean = False
    Private Pk_Condition As String = "VOUCH-"
    Private vEnt_AutoPosting_Status As Boolean = False
    Private vEnt_AutoPosting_Code As String = ""
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private prn_HdDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_Status As Integer
    Private prn_DetSNo As Integer
    Private prn_HeadIndx As Integer

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_OrderList As New DataGridViewTextBoxEditingControl

    Public RptSubReport_Index As Integer = 0
    Public RptSubReport_CompanyShortName As String = ""
    Public RptSubReport_VouNo As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1


    Public Structure SubReport_Details

        Dim ReportName As String
        Dim ReportGroupName As String
        Dim ReportHeading As String
        Dim ReportInputs As String
        Dim IsGridReport As Boolean

        Dim CurrentRowVal As Integer
        Dim TopRowVal As Integer

        Dim DateInp_Value1 As Date
        Dim DateInp_Value2 As Date
        Dim CboInp_Text1 As String
        Dim CboInp_Text2 As String
        Dim CboInp_Text3 As String
        Dim CboInp_Text4 As String
        Dim CboInp_Text5 As String

    End Structure
    Public RptSubReportDet(10) As SubReport_Details

    Public Structure SubReport_InputDetails
        Dim PKey As String
        Dim TableName As String
        Dim Selection_FieldName As String
        Dim Return_FieldName As String
        Dim Condition As String
        Dim Display_Name As String
        Dim BlankFieldCondition As String
        Dim CtrlType_Cbo_OR_Txt As String
    End Structure
    Public RptSubReportInpDet(10, 10) As SubReport_InputDetails

    Private Sub clear()
        Dim I As Integer = 0

        New_Entry = False
        Insert_Entry = False
        Mov_Status = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_VouCode.Text = ""

        lbl_VouNo.Text = ""
        lbl_VouNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_CurrentBalance.Visible = False
        Pnl_PrintRange.Visible = False
        pnl_Print_Voucher.Visible = False

        lbl_CurrentBalance.Tag = -100
        lbl_CurrentBalance.Text = "Current Balance :"

        lbl_Day.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
            dtp_Date.Text = ""
            msk_Date.Text = ""
        End If

        txt_Narration.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_OrderList.Rows.Clear()

        Select Case Trim(LCase(lbl_VouType.Text))
            Case "pymt", "rcpt"
                cbo_AdvanceType.Text = "ADVANCE"
            Case Else
                cbo_AdvanceType.Text = "BILL"
        End Select
        cbo_AdvanceType.Enabled = False


        txt_BillNo.Text = ""
        txt_AdvanceAmount.Text = ""
        lbl_Advance_AdjustAmount.Text = ""
        lbl_AdvanceReceiptNo.Text = ""
        lbl_Total_BillAmount.Text = ""

        dgv_Selection.Rows.Clear()

        dgv_SelectionDetails.Rows.Clear()

        cbo_Grid_CrDrType.Text = ""
        cbo_Grid_CrDrType.Visible = False

        cbo_Grid_Ledger.Visible = False
        cbo_Grid_Ledger.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        vEnt_AutoPosting_Status = False
        vEnt_AutoPosting_Code = ""

        Erase VouAmtAr
        VouAmtAr = New VoucherEntry_AmountDetails(10) {}
        For I = 0 To UBound(VouAmtAr)
            VouAmtAr(I).LedgerIdNo = 0
            VouAmtAr(I).VoucherAmount = 0
        Next

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Prec_ActCtrl Is Button Then
            Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
            Prec_ActCtrl.ForeColor = Color.White
        End If

        If Me.ActiveControl.Name <> cbo_Grid_CrDrType.Name Then
            cbo_Grid_CrDrType.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Ledger.Name Then
            cbo_Grid_Ledger.Visible = False
        End If

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
        dgv_Selection.CurrentCell.Selected = False
        dgv_OrderList.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            Mov_Status = True

            da1 = New SqlClient.SqlDataAdapter("select * from Voucher_Head where Voucher_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_VouCode.Text = Common_Procedures.OrderBy_ValueToCode(Val(dt1.Rows(0).Item("For_OrderByCode").ToString))
                lbl_VouNo.Text = dt1.Rows(0).Item("Voucher_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Voucher_Date").ToString
                msk_Date.Text = dtp_Date.Text
                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                vEnt_AutoPosting_Status = False
                vEnt_AutoPosting_Code = ""
                If Val(dt1.Rows(0).Item("indicate").ToString) = 1 Then
                    vEnt_AutoPosting_Status = True
                    vEnt_AutoPosting_Code = dt1.Rows(0).Item("Entry_Identification").ToString
                End If


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Voucher_Details a, Ledger_Head b where a.Voucher_Code = '" & Trim(NewCode) & "' and a.ledger_idno = b.ledger_idno Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Ledger_Name").ToString

                        If Val(dt2.Rows(i).Item("Voucher_Amount").ToString) <= 0 Then
                            dgv_Details.Rows(n).Cells(0).Value = "DR"
                            dgv_Details.Rows(n).Cells(2).Value = Trim(Format(Math.Abs(Val(dt2.Rows(i).Item("Voucher_Amount").ToString)), "#########0.00"))
                        Else
                            dgv_Details.Rows(n).Cells(0).Value = "CR"
                            dgv_Details.Rows(n).Cells(3).Value = Trim(Format(Math.Abs(Val(dt2.Rows(i).Item("Voucher_Amount").ToString)), "#########0.00"))
                        End If

                        VouAmtAr(n).LedgerIdNo = Val(dt2.Rows(i).Item("Ledger_IdNo").ToString)
                        VouAmtAr(n).VoucherAmount = Val(dt2.Rows(i).Item("Voucher_Amount").ToString)

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Trim(Format(Val(dt1.Rows(0).Item("Total_VoucherAmount").ToString), "#########0.00"))
                    .Rows(0).Cells(3).Value = Trim(Format(Val(dt1.Rows(0).Item("Total_VoucherAmount").ToString), "#########0.00"))
                End With

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select * from voucher_bill_details where Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_SelectionDetails.Rows.Clear()

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_SelectionDetails.Rows.Add()

                        dgv_SelectionDetails.Rows(n).Cells(0).Value = Val(dt2.Rows(i).Item("Ledger_IdNo").ToString)
                        dgv_SelectionDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Voucher_Bill_Code").ToString
                        dgv_SelectionDetails.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Amount").ToString)
                        dgv_SelectionDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("CrDr_Type").ToString
                        dgv_SelectionDetails.Rows(n).Cells(4).Value = "BILL"
                        dgv_SelectionDetails.Rows(n).Cells(5).Value = ""
                        dgv_SelectionDetails.Rows(n).Cells(6).Value = ""

                    Next i

                End If

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select *, bill_amount-(abs(credit_amount-debit_amount)) as paid_rcvd_amount from voucher_bill_head where Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_SelectionDetails.Rows.Add()

                        dgv_SelectionDetails.Rows(n).Cells(0).Value = Val(dt2.Rows(i).Item("Ledger_IdNo").ToString)
                        dgv_SelectionDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Voucher_Bill_Code").ToString
                        dgv_SelectionDetails.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("bill_amount").ToString)
                        dgv_SelectionDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("CrDr_Type").ToString
                        dgv_SelectionDetails.Rows(n).Cells(4).Value = "ADV"
                        dgv_SelectionDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("party_bill_no").ToString
                        dgv_SelectionDetails.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("paid_rcvd_amount").ToString)

                    Next i

                End If

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select * from Voucher_Order_Details where Voucher_Code = '" & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_OrderList.Rows.Clear()

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_OrderList.Rows.Add()
                        dgv_OrderList.Rows(n).Cells(0).Value = Trim(dt2.Rows(i).Item("Sl_No").ToString)
                        dgv_OrderList.Rows(n).Cells(1).Value = Trim(dt2.Rows(i).Item("Sales_Order_Selection_Code").ToString)
                        dgv_OrderList.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "##########0.00")

                    Next i

                End If

                dt2.Clear()
            End If

            dt1.Clear()

            cbo_Grid_CrDrType.Visible = False
            cbo_Grid_Ledger.Visible = False

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Mov_Status = False

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Voucher_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Voucher_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If pnl_Selection.Enabled = False Then
            MessageBox.Show("Close Bill Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If vEnt_AutoPosting_Status = True Then
            MessageBox.Show("Auto Posted from Other Entry (" & vEnt_AutoPosting_Code & ")", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        da = New SqlClient.SqlDataAdapter("Select * from voucher_bill_head where entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (bill_amount - (abs(credit_amount-debit_amount)) ) <> 0", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            MessageBox.Show("Amount Receipt (or) Paid to advance bill", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        dt.Clear()

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b Where b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code and a.ledger_idno = b.ledger_idno and b.crdr_type = 'CR'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update voucher_bill_head set debit_amount = a.debit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code and a.ledger_idno = b.ledger_idno and b.crdr_type = 'DR'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_details where entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_head where entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and bill_amount = (credit_amount+debit_amount)"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Voucher_Order_Details where Voucher_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            da.Dispose()
            dt.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()
        End Try



    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

            da.Dispose()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim VouCode As String
        Dim OrdByNo_Code As Double

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Voucher_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Voucher_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Voucher No.", "FOR INSERTION...")

            cmd.Connection = con
            cmd.CommandText = "select Voucher_No, For_OrderByCode from Voucher_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_No = '" & Trim(inpno) & "' and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            OrdByNo_Code = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                        OrdByNo_Code = Val(dr(1).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                VouCode = Common_Procedures.OrderBy_ValueToCode(OrdByNo_Code)
                move_record(VouCode)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Voucher No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_VouNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 For_OrderByCode from Voucher_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Voucher_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                movno = Common_Procedures.OrderBy_ValueToCode(Val(movno))
                move_record(movno)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 For_OrderByCode from Voucher_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Voucher_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                movno = Common_Procedures.OrderBy_ValueToCode(Val(movno))
                move_record(movno)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String
        Dim OrdByNo As Double

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VouNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 For_OrderByCode from Voucher_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Voucher_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                movno = Common_Procedures.OrderBy_ValueToCode(Val(movno))
                move_record(movno)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String
        Dim OrdByNo As Double

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VouNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 For_OrderByCode from Voucher_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Voucher_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                movno = Common_Procedures.OrderBy_ValueToCode(Val(movno))
                move_record(movno)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As Integer = 0
        Dim NewNo As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(For_OrderByCode) from Voucher_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NewCode = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NewCode = Val(dt1.Rows(0)(0).ToString)
                End If
            End If

            NewCode = NewCode + 1
            lbl_VouCode.Text = NewCode

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Voucher_Head where Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            dt2 = New DataTable
            da.Fill(dt2)

            NewNo = 0
            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                    NewNo = Val(dt2.Rows(0)(0).ToString)
                End If
            End If

            NewNo = NewNo + 1

            lbl_VouNo.Text = NewNo
            lbl_VouNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString

            'If UCase(Trim(lbl_VouType.Text)) = "RCPT" Or UCase(Trim(lbl_VouType.Text)) = "CSRP" Then
            '    dgv_Details.Rows.Add(0)
            '    dgv_Details.Rows(0).Cells(0).Value = "Cr"
            '    dgv_Details.Rows.Add(1)
            '    dgv_Details.Rows(1).Cells(0).Value = "Dr"
            '    dgv_Details.Columns(0).ReadOnly = True
            'Else
            '    dgv_Details.Columns(0).ReadOnly = False
            'End If

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim VouCode As String
        Dim OrdByNo_Code As Double

        Try

            inpno = InputBox("Enter Voucher No.", "FOR FINDING...")

            cmd.Connection = con
            cmd.CommandText = "select Voucher_No, For_OrderByCode from Voucher_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_No = '" & Trim(inpno) & "' and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            OrdByNo_Code = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                        OrdByNo_Code = Val(dr(1).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                VouCode = Common_Procedures.OrderBy_ValueToCode(OrdByNo_Code)
                move_record(VouCode)

            Else
                MessageBox.Show("Voucher No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim New_BillCode As String = ""
        Dim New_BillNo As String = ""
        Dim Nr As Long = 0
        Dim led_idno As Integer = 0
        Dim Sno As Integer = 0
        Dim Dup_LedIdNos As String
        Dim db_idno As Integer = 0
        Dim cr_idno As Integer = 0
        Dim VouAmt As Double = 0
        Dim vTotCrAmt As Double = 0
        Dim vTotDrAmt As Double = 0
        Dim Mx_DrAmt As Double = 0
        Dim Mx_CrAmt As Double = 0
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim BilTyp As String = ""
        Dim TtBlAmt As Double = 0
        Dim vVochOrderAmt As Double = 0

        If pnl_Selection.Enabled = False Then
            MessageBox.Show("Close Bill Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Voucher_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If vEnt_AutoPosting_Status = True Then
            MessageBox.Show("Auto Posted from Other Entry (" & vEnt_AutoPosting_Code & ")", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        Dup_LedIdNos = ""


        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then

                led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If led_idno = 0 Then
                    MessageBox.Show("Invalid Ledger A/c Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    Exit Sub
                End If

                If InStr(1, Dup_LedIdNos, "~" & Trim(Val(led_idno)) & "~") > 0 Then
                    MessageBox.Show("Duplicae Ledger A/c Name - Dont seelct same ledger.", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    Exit Sub
                End If

                Dup_LedIdNos = Dup_LedIdNos & "~" & Trim(Val(led_idno)) & "~"

            End If

        Next

        db_idno = 0
        cr_idno = 0

        Mx_DrAmt = 0
        Mx_CrAmt = 0

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then

                If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 And Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then
                    MessageBox.Show("Invalid Amount - Feed Either Debit or Credit", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 And (db_idno = 0 Or Val(dgv_Details.Rows(i).Cells(2).Value) > Mx_DrAmt) Then
                    db_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                    Mx_DrAmt = Val(dgv_Details.Rows(i).Cells(2).Value)
                End If

                If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 And (cr_idno = 0 Or Val(dgv_Details.Rows(i).Cells(3).Value) > Mx_CrAmt) Then
                    cr_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                    Mx_CrAmt = Val(dgv_Details.Rows(i).Cells(3).Value)
                End If

            End If

        Next

        vTotDrAmt = 0 : vTotCrAmt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotDrAmt = Format(CSng(dgv_Details_Total.Rows(0).Cells(2).Value()), "##########0.00")
            vTotCrAmt = Format(CSng(dgv_Details_Total.Rows(0).Cells(3).Value()), "##########0.00")
        End If

        If Val(vTotDrAmt) <> Val(vTotCrAmt) Then
            MessageBox.Show("Invalid Voucher Amount - Total Debit and Credit amount not equal", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
            dgv_Details.CurrentCell.Selected = True
            Exit Sub
        End If

        If Val(vTotDrAmt) = 0 And Val(vTotCrAmt) = 0 Then
            MessageBox.Show("Invalid Voucher Amount Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
            Exit Sub
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_VouCode.Text = Common_Procedures.get_MaxCode(con, "Voucher_Head", "Voucher_Code", "For_OrderByCode", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                lbl_VouNo.Text = Common_Procedures.get_MaxCode(con, "Voucher_Head", "Voucher_Code", "For_OrderBy", "(Voucher_Type = '" & Trim(lbl_VouType.Text) & "')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@VouchDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) Values ('" & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouCode.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_VouNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text))) & ", '" & Trim(lbl_VouType.Text) & "', @VouchDate, " & Str(Val(db_idno)) & ", " & Str(Val(cr_idno)) & ", " & Str(Val(vTotCrAmt)) & ", '" & Trim(txt_Narration.Text) & "', 0, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Voucher_Head set Voucher_Type = '" & Trim(lbl_VouType.Text) & "', Voucher_date = @VouchDate, Debtor_Idno = " & Str(Val(db_idno)) & ", Creditor_Idno = " & Str(Val(cr_idno)) & ", Total_VoucherAmount = " & Str(Val(vTotCrAmt)) & ", Narration = '" & Trim(txt_Narration.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b Where b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code and a.ledger_idno = b.ledger_idno and b.crdr_type = 'CR'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update voucher_bill_head set debit_amount = a.debit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.voucher_bill_code = b.voucher_bill_code and a.ledger_idno = b.ledger_idno and b.crdr_type = 'DR'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_details where entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from voucher_bill_head where entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and bill_amount = (credit_amount+debit_amount)"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Voucher_Order_Details where Voucher_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            Sno = 0
            For i = 0 To dgv_Details.RowCount - 1

                If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then

                    led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(1).Value), tr)

                    If led_idno <> 0 Then

                        Sno = Sno + 1

                        VouAmt = 0
                        If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Then VouAmt = -1 * Val(dgv_Details.Rows(i).Cells(2).Value)
                        If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then VouAmt = Val(dgv_Details.Rows(i).Cells(3).Value)

                        cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) Values ('" & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouCode.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_VouNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text))) & ", '" & Trim(lbl_VouType.Text) & "', @VouchDate, " & Str(Val(Sno)) & ", " & Str(Val(led_idno)) & ", " & Str(Val(VouAmt)) & ", '" & Trim(txt_Narration.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                        cmd.ExecuteNonQuery()

                        BilTyp = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Bill_Type", "(Ledger_Idno = " & Str(Val(led_idno)) & ")", , tr)

                        If Trim(UCase(BilTyp)) = "BILL TO BILL" Then

                            TtBlAmt = 0

                            For k = 0 To dgv_SelectionDetails.Rows.Count - 1

                                Nr = 0
                                If Val(dgv_SelectionDetails.Rows(k).Cells(0).Value) = Val(led_idno) And Trim(UCase(dgv_SelectionDetails.Rows(k).Cells(3).Value)) = Trim(UCase(dgv_Details.Rows(i).Cells(0).Value)) Then

                                    If Trim(UCase(dgv_SelectionDetails.Rows(k).Cells(4).Value)) = "BILL" Then

                                        cmd.CommandText = "Insert into Voucher_Bill_Details ( Voucher_Bill_Code, Company_Idno, Voucher_Bill_Date, Ledger_Idno, entry_identification, Amount, CrDr_Type ) values ( '" & Trim(dgv_SelectionDetails.Rows(k).Cells(1).Value) & "', " & Str(Val(lbl_Company.Tag)) & ", @VouchDate, " & Str(Val(led_idno)) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(dgv_SelectionDetails.Rows(k).Cells(2).Value)) & ", '" & Trim(dgv_SelectionDetails.Rows(k).Cells(3).Value) & "' )"
                                        cmd.ExecuteNonQuery()

                                        Nr = 0
                                        cmd.CommandText = "update voucher_bill_head set " & IIf(Trim(UCase(dgv_Details.Rows(i).Cells(0).Value)) = "CR", "Credit_Amount", "Debit_Amount") & " = " & IIf(Trim(UCase(dgv_Details.Rows(i).Cells(0).Value)) = "CR", "Credit_Amount", "Debit_Amount") & " + " & Str(Val(dgv_SelectionDetails.Rows(k).Cells(2).Value)) & " where ledger_idno = " & Str(Val(led_idno)) & " and voucher_bill_code = '" & Trim(dgv_SelectionDetails.Rows(k).Cells(1).Value) & "'"
                                        Nr = cmd.ExecuteNonQuery()


                                    ElseIf Trim(UCase(dgv_SelectionDetails.Rows(k).Cells(4).Value)) = "ADV" Then

                                        Nr = 0
                                        cmd.CommandText = "update voucher_bill_head set bill_amount = " & Str(Val(dgv_SelectionDetails.Rows(k).Cells(2).Value)) & ", " & IIf(Trim(UCase(dgv_Details.Rows(i).Cells(0).Value)) = "CR", "Credit_Amount", "Debit_Amount") & " = " & Str(Val(dgv_SelectionDetails.Rows(k).Cells(2).Value)) & " where ledger_idno = " & Str(Val(led_idno)) & " and voucher_bill_code = '" & Trim(dgv_SelectionDetails.Rows(k).Cells(1).Value) & "'"
                                        Nr = cmd.ExecuteNonQuery()

                                        If Nr = 0 Then

                                            New_BillNo = Common_Procedures.get_MaxCode(con, "Voucher_Bill_Head", "Voucher_Bill_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                                            New_BillCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(New_BillNo) & "/" & Trim(Common_Procedures.FnYearCode)

                                            Nr = 0
                                            cmd.CommandText = "Insert into voucher_bill_head ( Voucher_Bill_Code,             Company_Idno         ,           Voucher_Bill_No ,            For_OrderBy      , Voucher_Bill_Date,           Ledger_IdNo     ,                                  party_bill_no                    , " & IIf(Trim(UCase(dgv_Details.Rows(i).Cells(0).Value)) = "CR", "Credit_Amount", "Debit_Amount") & ",                  bill_amount                                 ,                 entry_identification        ,                                  crdr_type                         ) " _
                                                                    & " Values  (   '" & Trim(New_BillCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(New_BillNo) & "', " & Str(Val(New_BillNo)) & ",    @VouchDate    , " & Str(Val(led_idno)) & ", '" & Trim(UCase(dgv_SelectionDetails.Rows(k).Cells(5).Value)) & "',                                        " & Str(Val(dgv_SelectionDetails.Rows(k).Cells(2).Value)) & ", " & Str(Val(dgv_SelectionDetails.Rows(k).Cells(2).Value)) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '" & Trim(UCase(dgv_SelectionDetails.Rows(k).Cells(3).Value)) & "' )"
                                            Nr = cmd.ExecuteNonQuery()

                                        End If


                                    End If

                                    If Nr = 0 Then
                                        tr.Rollback()
                                        MessageBox.Show("Invalid Bill Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Exit Sub
                                    End If

                                    TtBlAmt = TtBlAmt + Val(dgv_SelectionDetails.Rows(k).Cells(2).Value)

                                End If

                            Next

                            If Format(Math.Abs(Val(TtBlAmt)), "#########0.00") <> Format(Math.Abs(Val(VouAmt)), "#########0.00") Then
                                Throw New ApplicationException("Invalid Details - Mismatch of Voucher and Bill Amount")
                                'tr.Rollback()
                                'MessageBox.Show("Invalid Details - Mismatch of Voucher and Bill Amount", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Exit Sub
                            End If

                        End If

                    End If

                End If

            Next

            Sno = 0
            For i = 0 To dgv_OrderList.RowCount - 1

                If Val(dgv_OrderList.Rows(i).Cells(2).Value) <> 0 Then

                    vVochOrderAmt = 0
                    If VouAmt < 0 Then vVochOrderAmt = Val(dgv_OrderList.Rows(i).Cells(2).Value)
                    If VouAmt > 0 Then vVochOrderAmt = -1 * Val(dgv_OrderList.Rows(i).Cells(2).Value)

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into Voucher_Order_Details(Voucher_Code           , For_OrderByCode                                                          , Company_IdNo                     , Voucher_No                    , For_OrderBy                                                            , Voucher_Type                    , Voucher_Date, SL_No                ,  Sales_Order_Selection_Code                              , Amount ) " &
                                                                "Values ('" & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouCode.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_VouNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text))) & ", '" & Trim(lbl_VouType.Text) & "', @VouchDate  , " & Str(Val(Sno)) & ", '" & Trim(dgv_OrderList.Rows(i).Cells(1).Value) & "', " & Str(Val(vVochOrderAmt)) & ")"
                    cmd.ExecuteNonQuery()


                End If


            Next



            tr.Commit()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(Trim(lbl_VouCode.Text))
                End If
            Else
                move_record(Trim(lbl_VouCode.Text))
            End If


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
        End Try



    End Sub

    Private Sub Voucher_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim OrdByNo_Code As String = ""
        Dim VouCode As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                If Val(RptSubReport_Index) > 0 And Val(RptSubReport_VouNo) > 0 Then

                    Common_Procedures.CompIdNo = Val(Common_Procedures.Company_ShortNameToIdNo(con, RptSubReport_CompanyShortName))

                    If Common_Procedures.CompIdNo <> 0 Then

                        lbl_Company.Text = Common_Procedures.Company_IdNoToName(con, Common_Procedures.CompIdNo) & "  -  " & RptSubReport_CompanyShortName
                        lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                        Me.Text = lbl_Company.Text

                        OrdByNo_Code = ""
                        Da1 = New SqlClient.SqlDataAdapter("Select For_OrderByCode from Voucher_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_No = '" & Trim(RptSubReport_VouNo) & "' and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                                OrdByNo_Code = Dt1.Rows(0)(0).ToString
                            End If
                        End If
                        Dt1.Clear()

                        If Val(OrdByNo_Code) <> 0 Then
                            VouCode = Common_Procedures.OrderBy_ValueToCode(Format(Val(OrdByNo_Code), "#########0.00"))
                            move_record(VouCode)
                        End If

                    End If

                Else
                    lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                    lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                    Me.Text = lbl_Company.Text

                    Call new_record()

                End If

            End If

        Catch ex As Exception
            '-----

        Finally
            Da1.Dispose()
            Dt1.Dispose()

        End Try

        FrmLdSTS = False

        If Trim(Common_Procedures.Voucher_CR_Name) <> "" Then
            Dim n As Integer = 0
            n = dgv_Details.Rows.Add()
            dgv_Details.Rows(n).Cells(0).Value = "CR"
            dgv_Details.Rows(n).Cells(1).Value = Trim(Common_Procedures.Voucher_CR_Name)
        End If


    End Sub

    Private Sub Voucher_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        lbl_VouType.Text = Trim(Common_Procedures.VoucherType)

        Select Case Trim(LCase(lbl_VouType.Text))
            Case "purc"
                lbl_EntHeading.Text = "PURCHASE VOUCHER ENTRY"
            Case "sale"
                lbl_EntHeading.Text = "SALES VOUCHER ENTRY"
            Case "pymt"
                lbl_EntHeading.Text = "BANK PAYMENT VOUCHER ENTRY"
            Case "rcpt"
                lbl_EntHeading.Text = "BANK RECEIPT VOUCHER ENTRY"
            Case "cspy"
                lbl_EntHeading.Text = "CASH PAYMENT VOUCHER ENTRY"
            Case "csrp"
                lbl_EntHeading.Text = "CASH RECEIPT VOUCHER ENTRY"
            Case "cntr"
                lbl_EntHeading.Text = "CONTRA VOUCHER ENTRY"
            Case "jrnl"
                lbl_EntHeading.Text = "JOURNAL VOUCHER ENTRY"
            Case "crnt"
                lbl_EntHeading.Text = "CREDIT NOTE VOUCHER ENTRY"
            Case "dbnt"
                lbl_EntHeading.Text = "DEBIT NOTE VOUCHER ENTRY"
            Case "ptcs"
                lbl_EntHeading.Text = "PETTI CASH VOUCHER ENTRY"
        End Select

        Me.Text = ""

        con.Open()

        VouAmtAr = New VoucherEntry_AmountDetails(10) {}

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a order by a.Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Grid_Ledger.DataSource = dt1
        cbo_Grid_Ledger.DisplayMember = "Ledger_DisplayName"
        cbo_Grid_Ledger.Visible = False

        cbo_Grid_CrDrType.Visible = False
        cbo_Grid_CrDrType.Items.Clear()
        cbo_Grid_CrDrType.Items.Add("DR")
        cbo_Grid_CrDrType.Items.Add("CR")

        cbo_AdvanceType.Items.Clear()
        cbo_AdvanceType.Items.Add("BILL")
        cbo_AdvanceType.Items.Add("ADVANCE")

        pnl_Filter.Visible = False
        pnl_Filter.BringToFront()
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.BringToFront()
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = ((Me.Height - pnl_Selection.Height) \ 2) + 75

        pnl_Print_Voucher.Visible = False
        pnl_Print_Voucher.BringToFront()
        pnl_Print_Voucher.Left = (Me.Width - pnl_Print_Voucher.Width) \ 2
        pnl_Print_Voucher.Top = (Me.Height - pnl_Print_Voucher.Height) \ 2

        Pnl_PrintRange.Visible = False
        Pnl_PrintRange.BringToFront()
        Pnl_PrintRange.Left = (Me.Width - Pnl_PrintRange.Width) \ 2
        Pnl_PrintRange.Top = (Me.Height - Pnl_PrintRange.Height) \ 2
        Pnl_PrintRange.BringToFront()


        pnl_Voucher_ChequePrint.Visible = False
        pnl_Voucher_ChequePrint.BringToFront()
        pnl_Voucher_ChequePrint.Left = (Me.Width - pnl_Voucher_ChequePrint.Width) \ 2
        pnl_Voucher_ChequePrint.Top = (Me.Height - pnl_Voucher_ChequePrint.Height) \ 2

        'btn_SMS.Visible = False
        'If Trim(LCase(lbl_VouType.Text)) = "pymt" Or Trim(LCase(lbl_VouType.Text)) = "rcpt" Or Trim(LCase(lbl_VouType.Text)) = "cspy" Or Trim(LCase(lbl_VouType.Text)) = "csrp" Then
        '    btn_SMS.Visible = True
        'End If

        pnl_CurrentBalance.Visible = False
        pnl_CurrentBalance.Left = dgv_Details_Total.Left
        pnl_CurrentBalance.Top = dgv_Details_Total.Top


        pnl_OrderList.Visible = False
        pnl_OrderList.BringToFront()
        pnl_OrderList.Left = (Me.Width - pnl_OrderList.Width) \ 2
        pnl_OrderList.Top = (Me.Height - pnl_OrderList.Height) \ 2

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CrDrType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AdvanceType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AdvanceAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint_Voucher.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Ordinary_Voucher.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_Print_Voucher.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_PrintCheque.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_PrintVoucher.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_Voucher_ChequePrint.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFromNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintToNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_PrintRange.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_PrintRange.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CrDrType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AdvanceType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AdvanceAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint_Voucher.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Ordinary_Voucher.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_Print_Voucher.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_PrintCheque.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_PrintVoucher.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_Voucher_ChequePrint.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFromNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintToNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_PrintRange.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_PrintRange.LostFocus, AddressOf ControlLostFocus

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2001" Then '---- Demo - Elpro Chem for Vasanth by Deva (Chennai)
            btn_Order_Selection.Visible = True
        End If
        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()


    End Sub

    Private Sub Voucher_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Dim I As Integer = 0
        Dim J As Integer = 0
        Dim RptIpDet_ReportGroupName As String = ""
        Dim RptIpDet_ReportName As String = ""
        Dim RptIpDet_ReportHeading As String = ""
        Dim RptIpDet_IsGridReport As Boolean = False
        Dim RptIpDet_ReportInputs As String = ""
        Dim vCurRow As Integer = -1
        Dim vTopRow As Integer = -1
        Dim vDateInp1 As Date
        Dim vDateInp2 As Date
        Dim vCboInpText1 As String = ""
        Dim vCboInpText2 As String = ""
        Dim vCboInpText3 As String = ""
        Dim vCboInpText4 As String = ""
        Dim vCboInpText5 As String = ""


        On Error Resume Next

        If Val(RptSubReport_Index) > 0 And Val(RptSubReport_VouNo) > 0 And Trim(RptSubReport_CompanyShortName) <> "" Then

            RptIpDet_ReportName = RptSubReportDet(RptSubReport_Index).ReportName
            RptIpDet_ReportGroupName = RptSubReportDet(RptSubReport_Index).ReportGroupName
            RptIpDet_ReportHeading = RptSubReportDet(RptSubReport_Index).ReportHeading
            RptIpDet_ReportInputs = RptSubReportDet(RptSubReport_Index).ReportInputs
            RptIpDet_IsGridReport = RptSubReportDet(RptSubReport_Index).IsGridReport
            vCurRow = Val(RptSubReportDet(RptSubReport_Index).CurrentRowVal)
            vTopRow = Val(RptSubReportDet(RptSubReport_Index).TopRowVal)
            vDateInp1 = RptSubReportDet(RptSubReport_Index).DateInp_Value1
            vDateInp2 = RptSubReportDet(RptSubReport_Index).DateInp_Value2
            vCboInpText1 = RptSubReportDet(RptSubReport_Index).CboInp_Text1
            vCboInpText2 = RptSubReportDet(RptSubReport_Index).CboInp_Text2
            vCboInpText3 = RptSubReportDet(RptSubReport_Index).CboInp_Text3
            vCboInpText4 = RptSubReportDet(RptSubReport_Index).CboInp_Text4
            vCboInpText5 = RptSubReportDet(RptSubReport_Index).CboInp_Text5

            RptSubReportDet(RptSubReport_Index).ReportName = ""
            RptSubReportDet(RptSubReport_Index).ReportGroupName = ""
            RptSubReportDet(RptSubReport_Index).ReportHeading = ""
            RptSubReportDet(RptSubReport_Index).ReportInputs = ""
            RptSubReportDet(RptSubReport_Index).IsGridReport = False
            RptSubReportDet(RptSubReport_Index).CurrentRowVal = -1
            RptSubReportDet(RptSubReport_Index).TopRowVal = -1
            RptSubReportDet(RptSubReport_Index).DateInp_Value1 = #1/1/1900#
            RptSubReportDet(RptSubReport_Index).DateInp_Value2 = #1/1/1900#
            RptSubReportDet(RptSubReport_Index).CboInp_Text1 = ""
            RptSubReportDet(RptSubReport_Index).CboInp_Text2 = ""
            RptSubReportDet(RptSubReport_Index).CboInp_Text3 = ""
            RptSubReportDet(RptSubReport_Index).CboInp_Text4 = ""
            RptSubReportDet(RptSubReport_Index).CboInp_Text5 = ""

            For I = 1 To 10

                RptSubReportInpDet(RptSubReport_Index, I).PKey = ""
                RptSubReportInpDet(RptSubReport_Index, I).TableName = ""
                RptSubReportInpDet(RptSubReport_Index, I).Selection_FieldName = ""
                RptSubReportInpDet(RptSubReport_Index, I).Return_FieldName = ""
                RptSubReportInpDet(RptSubReport_Index, I).Condition = ""
                RptSubReportInpDet(RptSubReport_Index, I).Display_Name = ""
                RptSubReportInpDet(RptSubReport_Index, I).BlankFieldCondition = ""
                RptSubReportInpDet(RptSubReport_Index, I).CtrlType_Cbo_OR_Txt = ""

            Next I

            RptSubReport_Index = RptSubReport_Index - 1


            Common_Procedures.RptInputDet.ReportGroupName = RptIpDet_ReportGroupName
            Common_Procedures.RptInputDet.ReportName = RptIpDet_ReportName
            Common_Procedures.RptInputDet.ReportHeading = RptIpDet_ReportHeading
            Common_Procedures.RptInputDet.IsGridReport = RptIpDet_IsGridReport
            Common_Procedures.RptInputDet.ReportInputs = RptIpDet_ReportInputs

            Dim f As New Report_Details_1

            f.RptSubReport_Index = RptSubReport_Index

            For I = 1 To 10

                f.RptSubReportDet(I).ReportName = RptSubReportDet(I).ReportName
                f.RptSubReportDet(I).ReportGroupName = RptSubReportDet(I).ReportGroupName
                f.RptSubReportDet(I).ReportHeading = RptSubReportDet(I).ReportHeading
                f.RptSubReportDet(I).ReportInputs = RptSubReportDet(I).ReportInputs
                f.RptSubReportDet(I).IsGridReport = RptSubReportDet(I).IsGridReport
                f.RptSubReportDet(I).CurrentRowVal = RptSubReportDet(I).CurrentRowVal
                f.RptSubReportDet(I).TopRowVal = RptSubReportDet(I).TopRowVal

                f.RptSubReportDet(I).DateInp_Value1 = RptSubReportDet(I).DateInp_Value1
                f.RptSubReportDet(I).DateInp_Value2 = RptSubReportDet(I).DateInp_Value2
                f.RptSubReportDet(I).CboInp_Text1 = RptSubReportDet(I).CboInp_Text1
                f.RptSubReportDet(I).CboInp_Text2 = RptSubReportDet(I).CboInp_Text2
                f.RptSubReportDet(I).CboInp_Text3 = RptSubReportDet(I).CboInp_Text3
                f.RptSubReportDet(I).CboInp_Text4 = RptSubReportDet(I).CboInp_Text4
                f.RptSubReportDet(I).CboInp_Text5 = RptSubReportDet(I).CboInp_Text5

                For J = 1 To 10

                    f.RptSubReportInpDet(I, J).PKey = RptSubReportInpDet(I, J).PKey
                    f.RptSubReportInpDet(I, J).TableName = RptSubReportInpDet(I, J).TableName
                    f.RptSubReportInpDet(I, J).Selection_FieldName = RptSubReportInpDet(I, J).Selection_FieldName
                    f.RptSubReportInpDet(I, J).Return_FieldName = RptSubReportInpDet(I, J).Return_FieldName
                    f.RptSubReportInpDet(I, J).Condition = RptSubReportInpDet(I, J).Condition
                    f.RptSubReportInpDet(I, J).Display_Name = RptSubReportInpDet(I, J).Display_Name
                    f.RptSubReportInpDet(I, J).BlankFieldCondition = RptSubReportInpDet(I, J).BlankFieldCondition
                    f.RptSubReportInpDet(I, J).CtrlType_Cbo_OR_Txt = RptSubReportInpDet(I, J).CtrlType_Cbo_OR_Txt

                Next J

            Next I

            f.MdiParent = MDIParent1
            f.Show()

            f.dtp_FromDate.Text = vDateInp1.ToShortDateString
            f.dtp_ToDate.Text = vDateInp2.ToShortDateString

            f.cbo_Inputs1.Text = vCboInpText1
            f.cbo_Inputs2.Text = vCboInpText2
            f.cbo_Inputs3.Text = vCboInpText3
            f.cbo_Inputs4.Text = vCboInpText4
            f.cbo_Inputs5.Text = vCboInpText5

            f.Show_Report()

            If vCurRow > 0 Then
                If f.dgv_Report.Rows.Count > 0 And f.dgv_Report.Rows.Count >= vCurRow Then
                    f.dgv_Report.CurrentCell = f.dgv_Report.Rows(vCurRow).Cells(0)
                    f.dgv_Report.CurrentCell.Selected = True
                End If
            End If
            If vTopRow > 0 Then
                If f.dgv_Report.Rows.Count > 0 And f.dgv_Report.Rows.Count >= vTopRow Then
                    f.dgv_Report.FirstDisplayedScrollingRowIndex = vTopRow
                End If
            End If

        End If

        con.Close()
        con.Dispose()
        'Common_Procedures.Last_Closed_FormName = Me.Name

    End Sub

    Private Sub Voucher_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then


            If pnl_Filter.Visible = True Then
                btn_Filter_Close_Click(sender, e)
                Exit Sub

            ElseIf pnl_Selection.Visible = True Then
                btn_Close_Selection_Click(sender, e)
                Exit Sub

            ElseIf pnl_Print_Voucher.Visible = True Then
                btn_Close_Print_Voucher_Click(sender, e)
                Exit Sub

            ElseIf pnl_Voucher_ChequePrint.Visible = True Then
                btn_Close_Voucher_ChequePrint_Click(sender, e)
                Exit Sub

            Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    Close_Form()
                End If

            End If

        End If
    End Sub

    Private Sub Close_Form()

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            If Val(RptSubReport_Index) > 0 And Val(RptSubReport_VouNo) > 0 And Trim(RptSubReport_CompanyShortName) <> "" Then
                Me.Close()

            Else
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim TcAm As Double, TdAm As Double
        Dim Led_IdNo As Integer = 0
        Dim BilTyp As String = ""

        'On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_Selection.Name Or ActiveControl.Name = dgv_OrderList.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing
            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf ActiveControl.Name = dgv_Selection.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Selection.IsCurrentRowDirty = True Then
                dgv1 = dgv_Selection

            ElseIf pnl_Selection.Visible = True Then
                dgv1 = dgv_Selection
            ElseIf ActiveControl.Name = dgv_OrderList.Name Then
                dgv1 = dgv_OrderList

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_OrderList

            ElseIf ActiveControl.Name = dgv_Selection.Name Then
                dgv1 = dgv_OrderList
            ElseIf pnl_OrderList.Visible = True Then
                dgv1 = dgv_OrderList
            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Selection.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then

                                    If txt_BillNo.Enabled = True Then txt_BillNo.Focus() Else txt_AdvanceAmount.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(6)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(6)

                            End If
                            Return True


                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= .ColumnCount - 2 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    txt_AdvanceAmount.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(6)

                            End If
                            Return True


                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)


                        End If
                    End If

                    If dgv1.Name = dgv_OrderList.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then

                                    If .CurrentCell.ColumnIndex = 2 Then
                                        pnl_Back.Enabled = True
                                        pnl_OrderList.Visible = False
                                    Else
                                        Return MyBase.ProcessCmdKey(msg, keyData)
                                    End If




                                ElseIf .CurrentCell.ColumnIndex = 2 Then

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                Else
                                    If cbo_Grid_OrderNo.DroppedDown = False Then
                                        .Focus()
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)
                                    Else
                                        Return MyBase.ProcessCmdKey(msg, keyData)
                                    End If



                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Trim(.CurrentRow.Cells(1).Value) = "") Or (.CurrentCell.ColumnIndex = 1 And Trim(dgv_OrderList.Text) = "")) Then
                                    For i = 0 To .Columns.Count - 1
                                        .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                    Next


                                    txt_BillNo.Focus()

                                Else
                                    .Focus()
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                End If


                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    'pnl_VendorDetails_EmptyBeam.Visible = False
                                    'pnl_Back.Enabled = True

                                    Return MyBase.ProcessCmdKey(msg, keyData)

                                Else
                                    If cbo_Grid_OrderNo.DroppedDown = False Then
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                                    Else
                                        Return MyBase.ProcessCmdKey(msg, keyData)
                                    End If


                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else

                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    End If


                    Led_IdNo = 0
                    BilTyp = ""

                    If dgv1.Name = dgv_Details.Name Then
                        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .CurrentRow.Cells(1).Value)
                        BilTyp = ""
                        If Led_IdNo <> 0 And Trim(.CurrentRow.Cells(0).Value) <> "" Then
                            BilTyp = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Bill_Type", "(Ledger_Idno = " & Str(Val(Led_IdNo)) & ")")
                        End If
                    End If

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If Trim(UCase(BilTyp)) = "BILL TO BILL" Then
                                    dgv_Details.EndEdit()
                                    Bill_Selection()

                                Else
                                    txt_Narration.Focus()

                                End If

                            Else

                                If Trim(.Rows(.CurrentCell.RowIndex + 1).Cells(1).Value) = "" And Val(.Rows(.CurrentCell.RowIndex + 1).Cells(2).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex + 1).Cells(3).Value) = 0 Then

                                    TcAm = 0 : TdAm = 0

                                    For i = 0 To .Rows.Count - 1

                                        If ActiveControl.Name = dgv_Details.Name Or i <> .CurrentCell.RowIndex Then
                                            TdAm = TdAm + Val(.Rows(i).Cells(2).Value)
                                            TcAm = TcAm + Val(.Rows(i).Cells(3).Value)

                                        ElseIf TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then
                                            If .CurrentCell.ColumnIndex = 2 Then
                                                TdAm = TdAm + Val(dgtxt_Details.Text)
                                                TcAm = TcAm + Val(.Rows(i).Cells(3).Value)

                                            Else
                                                TdAm = TdAm + Val(.Rows(i).Cells(2).Value)
                                                TcAm = TcAm + Val(dgtxt_Details.Text)

                                            End If

                                        End If

                                    Next i

                                    If Trim(UCase(BilTyp)) = "BILL TO BILL" Then
                                        dgv_Details.EndEdit()
                                        Bill_Selection()

                                    Else
                                        If TcAm = TdAm Then
                                            txt_Narration.Focus()

                                        Else
                                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                                        End If

                                    End If

                                Else
                                    If Trim(UCase(BilTyp)) = "BILL TO BILL" Then
                                        dgv_Details.EndEdit()
                                        Bill_Selection()

                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)
                                    End If


                                End If

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 2 Then
                            If Trim(UCase(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value)) = "DR" Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    If Trim(UCase(BilTyp)) = "BILL TO BILL" Then
                                        dgv_Details.EndEdit()
                                        Bill_Selection()
                                    Else
                                        txt_Narration.Focus()
                                    End If

                                Else
                                    If Trim(.Rows(.CurrentCell.RowIndex + 1).Cells(1).Value) = "" And Val(.Rows(.CurrentCell.RowIndex + 1).Cells(2).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex + 1).Cells(3).Value) = 0 Then

                                        TcAm = 0 : TdAm = 0

                                        For i = 0 To .Rows.Count - 1

                                            If ActiveControl.Name = dgv_Details.Name Or i <> .CurrentCell.RowIndex Then
                                                TdAm = TdAm + Val(.Rows(i).Cells(2).Value)
                                                TcAm = TcAm + Val(.Rows(i).Cells(3).Value)

                                            ElseIf TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then
                                                If .CurrentCell.ColumnIndex = 2 Then
                                                    TdAm = TdAm + Val(dgtxt_Details.Text)
                                                    TcAm = TcAm + Val(.Rows(i).Cells(3).Value)

                                                Else
                                                    TdAm = TdAm + Val(.Rows(i).Cells(2).Value)
                                                    TcAm = TcAm + Val(dgtxt_Details.Text)

                                                End If

                                            End If

                                        Next i

                                        If Trim(UCase(BilTyp)) = "BILL TO BILL" Then
                                            dgv_Details.EndEdit()
                                            Bill_Selection()

                                        Else
                                            If TcAm = TdAm Then
                                                txt_Narration.Focus()

                                            Else
                                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                                            End If

                                        End If

                                    Else

                                        If Trim(UCase(BilTyp)) = "BILL TO BILL" Then
                                            dgv_Details.EndEdit()
                                            Bill_Selection()

                                        Else
                                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                                        End If



                                    End If

                                End If


                            Else

                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        Else

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                msk_Date.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = .ColumnCount - 1 Then
                            If Trim(UCase(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value)) = "CR" Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

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

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim rect As Rectangle
        Dim i As Integer
        Dim TcAm As Double, TdAm As Double


        With dgv_Details

            If e.ColumnIndex = 0 And Mov_Status = False Then

                'If (UCase(Trim(lbl_VouType.Text)) = "RCPT" Or UCase(Trim(lbl_VouType.Text)) = "CSRP") And .CurrentRow.Index < 2 Then
                'Exit Sub
                'Else

                If cbo_Grid_CrDrType.Visible = False Or Val(cbo_Grid_CrDrType.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CrDrType.Left = .Left + rect.Left
                    cbo_Grid_CrDrType.Top = .Top + rect.Top

                    cbo_Grid_CrDrType.Width = rect.Width
                    cbo_Grid_CrDrType.Height = rect.Height
                    cbo_Grid_CrDrType.Text = .CurrentCell.Value

                    cbo_Grid_CrDrType.Tag = Val(e.RowIndex)
                    cbo_Grid_CrDrType.Visible = True

                    cbo_Grid_CrDrType.BringToFront()
                    cbo_Grid_CrDrType.Focus()

                End If

            Else

                cbo_Grid_CrDrType.Visible = False

            End If

            If e.ColumnIndex = 1 And Mov_Status = False Then

                If cbo_Grid_Ledger.Visible = False Or Val(cbo_Grid_Ledger.Tag) <> e.RowIndex Then

                    'cbo_Grid_Ledger.Tag = -1

                    Select Case UCase(Trim(lbl_VouType.Text))

                        Case "CSPY"
                            If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                                Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a WHERE AccountsGroup_IdNo = 6 OR AccountsGroup_IdNo = 12 order by a.Ledger_DisplayName", con)
                                'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  AccountsGroup_IdNo = 6 OR AccountsGroup_IdNo = 12 ", "(Ledger_IdNo = 0)")
                            ElseIf UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                                Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a WHERE NOT AccountsGroup_IdNo IN (5,6) order by a.Ledger_DisplayName", con)
                                'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  NOT AccountsGroup_IdNo IN (5,6) ", "(Ledger_IdNo = 0)")
                            End If
                        Case "CSRP"
                            If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                                Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a WHERE AccountsGroup_IdNo = 6 OR AccountsGroup_IdNo = 12 order by a.Ledger_DisplayName", con)
                                'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  AccountsGroup_IdNo = 6 OR AccountsGroup_IdNo = 12 ", "(Ledger_IdNo = 0)")
                            ElseIf UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                                Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a WHERE NOT AccountsGroup_IdNo IN (5,6) order by a.Ledger_DisplayName", con)
                                'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  NOT AccountsGroup_IdNo IN (5,6) ", "(Ledger_IdNo = 0)")
                            End If
                        Case "PYMT"
                            If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                                Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a WHERE AccountsGroup_IdNo = 5 OR AccountsGroup_IdNo = 12 order by a.Ledger_DisplayName", con)
                                'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  AccountsGroup_IdNo = 6 OR AccountsGroup_IdNo = 12", "(Ledger_IdNo = 0)")
                            ElseIf UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                                Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a WHERE NOT AccountsGroup_IdNo IN (5,6) order by a.Ledger_DisplayName", con)
                                'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  NOT AccountsGroup_IdNo IN (5,6) ", "(Ledger_IdNo = 0)")
                            End If
                        Case "RCPT"
                            If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                                Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a WHERE AccountsGroup_IdNo = 5 OR AccountsGroup_IdNo = 12 order by a.Ledger_DisplayName", con)
                                'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  AccountsGroup_IdNo = 6 OR AccountsGroup_IdNo = 12", "(Ledger_IdNo = 0)")
                            ElseIf UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                                Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a WHERE NOT AccountsGroup_IdNo IN (5,6) order by a.Ledger_DisplayName", con)
                                'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  NOT AccountsGroup_IdNo IN (5,6) ", "(Ledger_IdNo = 0)")
                            End If

                        Case Else

                            Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a  order by a.Ledger_DisplayName", con)

                    End Select



                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Ledger.DataSource = Dt1
                    cbo_Grid_Ledger.DisplayMember = "Ledger_DisplayName"

                    Select Case UCase(Trim(lbl_VouType.Text))
                        Case "CSPY"
                            If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                                dgv_Details.CurrentCell.Value = Common_Procedures.Ledger_IdnoToAlaisName(con, 1)
                                cbo_Grid_Ledger.Text = Common_Procedures.Ledger_IdnoToAlaisName(con, 1)
                            End If
                        Case "CSRP"
                            If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                                dgv_Details.CurrentCell.Value = Common_Procedures.Ledger_IdnoToAlaisName(con, 1)
                                cbo_Grid_Ledger.Text = Common_Procedures.Ledger_IdnoToAlaisName(con, 1)
                            End If
                    End Select

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)

                    cbo_Grid_Ledger.Left = .Left + rect.Left
                    cbo_Grid_Ledger.Top = .Top + rect.Top

                    cbo_Grid_Ledger.Width = rect.Width
                    cbo_Grid_Ledger.Height = rect.Height
                    cbo_Grid_Ledger.Text = .CurrentCell.Value

                    cbo_Grid_Ledger.Tag = Val(e.RowIndex)
                    cbo_Grid_Ledger.Visible = True

                    cbo_Grid_Ledger.BringToFront()
                    cbo_Grid_Ledger.Focus()

                End If

            Else
                cbo_Grid_Ledger.Visible = False

            End If

            If e.RowIndex = .Rows.Count - 1 And (e.ColumnIndex = 2 Or e.ColumnIndex = 3) Then

                'If Val(.CurrentRow.Cells(2).Value) = 0 And Val(.CurrentRow.Cells(3).Value) = 0 Then

                TcAm = 0 : TdAm = 0
                For i = 0 To .Rows.Count - 1
                    If i <> e.RowIndex Then
                        TdAm = TdAm + Val(.Rows(i).Cells(2).Value)
                        TcAm = TcAm + Val(.Rows(i).Cells(3).Value)
                    End If
                Next i

                If Trim(UCase(.CurrentRow.Cells(0).Value)) = "DR" And (TcAm - TdAm) > 0 Then .CurrentRow.Cells(2).Value = Val(TcAm - TdAm)
                If Trim(UCase(.CurrentRow.Cells(0).Value)) = "CR" And (TdAm - TcAm) > 0 Then .CurrentRow.Cells(3).Value = Val(TdAm - TcAm)

                'End If

            End If


            If e.ColumnIndex = 0 Or e.ColumnIndex = 1 Or e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then

                'If Val(lbl_CurrentBalance.Tag) = .CurrentRow.Index Then
                get_Ledger_CurrentBalance()
                'Else
                'pnl_CurrentBalance.Visible = False
                'End If

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        With dgv_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    GrossAmount_Calculation()
                End If

            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        On Error Resume Next

        With dgv_Details

            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    If Trim(UCase(.Rows(.CurrentCell.RowIndex).Cells(0).Value)) = "DR" Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    Else
                        e.Handled = True

                    End If

                End If

                If .CurrentCell.ColumnIndex = 3 Then
                    If Trim(UCase(.Rows(.CurrentCell.RowIndex).Cells(0).Value)) = "CR" Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    Else
                        e.Handled = True

                    End If

                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        With dgv_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then

                    GrossAmount_Calculation()

                End If
            End If
        End With
    End Sub


    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        msk_Date.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

            If e.KeyCode = Keys.Right Then
                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                    If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
                        txt_Narration.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        pnl_CurrentBalance.Visible = False
    End Sub

    Private Sub cbo_Grid_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Ledger.GotFocus

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim CrDr_Type As String
        Dim Prev_LedIdNo As Integer


        Select Case UCase(Trim(lbl_VouType.Text))

            Case "CSPY"
                If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  AccountsGroup_IdNo = 6 OR AccountsGroup_IdNo = 12", "(Ledger_IdNo = 0)")
                ElseIf UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  NOT AccountsGroup_IdNo IN (5,6) ", "(Ledger_IdNo = 0)")
                End If
            Case "CSRP"
                If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  AccountsGroup_IdNo = 6 OR AccountsGroup_IdNo = 12 ", "(Ledger_IdNo = 0)")
                ElseIf UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  NOT AccountsGroup_IdNo IN (5,6) ", "(Ledger_IdNo = 0)")
                End If
            Case "PYMT"
                If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  AccountsGroup_IdNo = 5 OR AccountsGroup_IdNo = 12 ", "(Ledger_IdNo = 0)")
                ElseIf UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  NOT AccountsGroup_IdNo IN (5,6) ", "(Ledger_IdNo = 0)")
                End If
            Case "RCPT"
                If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  AccountsGroup_IdNo = 5 OR AccountsGroup_IdNo = 12", "(Ledger_IdNo = 0)")
                ElseIf UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "  NOT AccountsGroup_IdNo IN (5,6) ", "(Ledger_IdNo = 0)")
                End If

            Case Else
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "")

        End Select

        Select Case UCase(Trim(lbl_VouType.Text))
            Case "CSPY"
                If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "CR" Then
                    dgv_Details.CurrentCell.Value = Common_Procedures.Ledger_IdnoToAlaisName(con, 1)
                    cbo_Grid_Ledger.Text = Common_Procedures.Ledger_IdnoToAlaisName(con, 1)
                End If
            Case "CSRP"
                If UCase(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(0).Value) = "DR" Then
                    dgv_Details.CurrentCell.Value = Common_Procedures.Ledger_IdnoToAlaisName(con, 1)
                    cbo_Grid_Ledger.Text = Common_Procedures.Ledger_IdnoToAlaisName(con, 1)
                End If
        End Select

        With dgv_Details

            If Trim(cbo_Grid_Ledger.Text) = "" Then

                If .CurrentCell.RowIndex = 1 Then

                    CrDr_Type = Trim(.Rows(.CurrentCell.RowIndex).Cells(0).Value)

                    Prev_LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(.CurrentCell.RowIndex - 1).Cells(1).Value))

                    Da = New SqlClient.SqlDataAdapter("Select " & IIf(Trim(UCase(CrDr_Type)) = "CR", "Creditor_Idno", "Debtor_Idno") & " from voucher_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " And voucher_type = '" & Trim(lbl_VouType.Text) & "' and " & IIf(Trim(UCase(CrDr_Type)) = "DR", "Creditor_Idno", "Debtor_Idno") & " = " & Str(Val(Prev_LedIdNo)) & " order by For_OrderBy, Voucher_No, For_OrderByCode", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                            cbo_Grid_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val((Dt1.Rows(0)(0).ToString)))
                        End If
                    End If
                    Dt1.Clear()

                    If Trim(cbo_Grid_Ledger.Text) = "" Then

                        Da = New SqlClient.SqlDataAdapter("Select " & IIf(Trim(UCase(CrDr_Type)) = "CR", "Creditor_Idno", "Debtor_Idno") & " from voucher_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and voucher_type = '" & Trim(lbl_VouType.Text) & "' order by For_OrderBy, Voucher_No, For_OrderByCode", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                                cbo_Grid_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val((Dt1.Rows(0)(0).ToString)))
                            End If
                        End If
                        Dt1.Clear()

                    End If

                    Dt1.Dispose()
                    Da.Dispose()

                End If

                End If

        End With

    End Sub

    Private Sub cbo_Grid_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Ledger.KeyDown

        vcbo_KeyDwnVal = e.KeyValue



        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Ledger, Nothing, Nothing, "", "", "", "")


        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    .CurrentCell = .Rows(0).Cells(0)
                    txt_Narration.Focus()

                Else
                    .Focus()

                    If Trim(UCase(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value)) = "CR" Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2)

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Ledger.KeyPress

        'If UCase(Trim(lbl_VouType.Text)) = "RCPT" And dgv_Details.CurrentRow.Index = 1 Then

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Ledger, Nothing, "", "", "", "")

        'ElseIf UCase(Trim(lbl_VouType.Text)) = "RCPT" And dgv_Details.CurrentRow.Index = 1
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "AccountsGroup_IdNo = 6", "(Ledger_IdNo = 0)")
        'Else
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        'End If

        With dgv_Details

            If Asc(e.KeyChar) = 13 Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    .CurrentCell = .Rows(0).Cells(0)
                    txt_Narration.Focus()

                Else
                    .Focus()
                    If Trim(UCase(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value)) = "CR" Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2)

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If

                get_Ledger_CurrentBalance()

            End If

        End With

    End Sub

    Private Sub cbo_Grid_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            'Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Ledger_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Ledger.TextChanged

        Try
            If cbo_Grid_Ledger.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Ledger.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Ledger.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_CrDrType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CrDrType.GotFocus

        Dim i As Integer
        Dim TcAm As Double, TdAm As Double

        With dgv_Details

            If Trim(cbo_Grid_CrDrType.Text) = "" Then

                'If .CurrentCell.RowIndex = 0 Then

                '    Select Case Trim(UCase(lbl_VouType.Text))
                '        Case "PURC", "RCPT", "CSRP", "CRNT", "CNTR"
                '            cbo_Grid_CrDrType.Text = "CR"
                '        Case Else
                '            cbo_Grid_CrDrType.Text = "DR"
                '    End Select

                'ElseIf .CurrentCell.RowIndex > 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(3).Value) = 0 Then

                '    If .CurrentCell.RowIndex = 1 Then
                '        Select Case Trim(UCase(lbl_VouType.Text))
                '            Case "PURC", "RCPT", "CSRP", "CRNT", "CNTR"
                '                cbo_Grid_CrDrType.Text = "DR"
                '            Case Else
                '                cbo_Grid_CrDrType.Text = "CR"
                '        End Select
                '    End If

                i = 0 : TcAm = 0 : TdAm = 0

                        For i = 0 To .Rows.Count - 1
                            TdAm = TdAm + Val(.Rows(i).Cells(2).Value)
                            TcAm = TcAm + Val(.Rows(i).Cells(3).Value)
                        Next i

                        cbo_Grid_CrDrType.Text = IIf(TcAm > TdAm, "DR", "CR")

                'End If

            End If

        End With

    End Sub


    Private Sub cbo_Grid_CrDrType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CrDrType.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CrDrType, Nothing, Nothing, "", "", "", "")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_CrDrType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If .CurrentCell.RowIndex = 0 Then
                    msk_Date.Focus()

                Else
                    .Focus()
                    If Trim(UCase(.Rows(.CurrentCell.RowIndex - 1).Cells.Item(0).Value)) = "DR" Then
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                    End If

                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_CrDrType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_Narration.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_CrDrType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CrDrType.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CrDrType, Nothing, "", "", "", "")

        With dgv_Details

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value) <> "" And (Val(.Rows(.CurrentCell.RowIndex).Cells.Item(2).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells.Item(3).Value) <> 0) Then

                    If Trim(UCase(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value)) = "DR" Then
                        If Val(.Rows(.CurrentCell.RowIndex).Cells.Item(2).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells.Item(3).Value) <> 0 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value
                        End If
                        .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = ""

                    ElseIf Trim(UCase(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value)) = "CR" Then
                        If Val(.Rows(.CurrentCell.RowIndex).Cells.Item(2).Value) <> 0 And Val(.Rows(.CurrentCell.RowIndex).Cells.Item(3).Value) = 0 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value
                        End If
                        .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = ""

                    End If
                    GrossAmount_Calculation()

                End If

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(0).Value) = "" Then
                    txt_Narration.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

                get_Ledger_CurrentBalance()

            End If

        End With

    End Sub

    Private Sub cbo_Grid_CrDrType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CrDrType.TextChanged

        Try
            If cbo_Grid_CrDrType.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_CrDrType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CrDrType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                Condt = "a.Voucher_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Voucher_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Voucher_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Debtor_Idno = " & Str(Val(Led_IdNo)) & " or a.Creditor_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.Voucher_No, a.Voucher_Date, a.Total_VoucherAmount, b.Ledger_Name as Debtor_Name, c.Ledger_Name as Creditor_Name from Voucher_Head a INNER JOIN Ledger_Head b on a.Debtor_Idno = b.Ledger_IdNo INNER JOIN Ledger_Head c on a.Creditor_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Voucher_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Voucher_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Voucher_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Debtor_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Creditor_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_VoucherAmount").ToString), "########0.00")

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

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If

    End Sub

    Private Sub Open_FilterEntry()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Dr As SqlClient.SqlDataReader
        Dim movno As String
        Dim VouCode As String
        Dim OrdByNo_Code As Double

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True

            Cmd.Connection = con
            Cmd.CommandText = "select Voucher_No, For_OrderByCode from Voucher_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_No = '" & Trim(movno) & "' and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'"
            Dr = Cmd.ExecuteReader

            movno = ""
            OrdByNo_Code = 0
            If Dr.HasRows Then
                If Dr.Read Then
                    If IsDBNull(Dr(0).ToString) = False Then
                        movno = Dr(0).ToString
                        OrdByNo_Code = Val(Dr(1).ToString)
                    End If
                End If
            End If

            Dr.Close()
            Cmd.Dispose()

            If Val(movno) <> 0 Then
                VouCode = Common_Procedures.OrderBy_ValueToCode(OrdByNo_Code)
                move_record(VouCode)
            End If

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

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.GotFocus
        pnl_CurrentBalance.Visible = False
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Narration.Focus()
        End If

    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Narration.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
        'If Asc(e.KeyChar) = 9 Then
        '    e.Handled = True
        '    'Windows.Forms.SendKeys.Send("{BACKSPACE}")
        '    'Windows.Forms.SendKeys.Send("{LEFT}")
        'End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Date.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

            'If e.KeyCode = 46 Then
            '    'If vmskSelStrt > 0 Then
            '    If vmskSelStrt <= 2 Then
            '        vmRetTxt = "  " & Microsoft.VisualBasic.Mid(vmskOldText, 3, Len(vmskOldText))
            '        vmRetSelStrt = 0
            '    ElseIf vmskSelStrt >= 3 And vmskSelStrt <= 5 Then
            '        vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, 3) & "  " & Microsoft.VisualBasic.Mid(vmskOldText, 6, Len(vmskOldText))
            '        vmRetSelStrt = 3
            '    Else
            '        vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, 6)
            '        vmRetSelStrt = 6
            '    End If

            '    'If Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 1, 1) = "-" Then
            '    '    vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, vmskSelStrt + 1) & "  " & Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 4, Len(vmskOldText))
            '    'Else
            '    '    vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, vmskSelStrt) & "  " & Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 3, Len(vmskOldText))
            '    'End If

            '    'Else

            '    'End If

            '    msk_Date.Text = vmRetTxt
            '    msk_Date.SelectionStart = vmRetSelStrt

            '    'If Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 1, 1) = "-" Then
            '    '    msk_Date.SelectionStart = vmskSelStrt + 1
            '    'Else
            '    '    msk_Date.SelectionStart = vmskSelStrt
            '    'End If

            'ElseIf e.KeyCode = 8 Then
            '    If vmskSelStrt > 0 Then
            '        vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, vmskSelStrt - 1) & " " & Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 1, Len(vmskOldText))
            '    Else
            '        'vmRetTxt = ""
            '        vmRetTxt = vmskOldText
            '    End If

            '    msk_Date.Text = vmRetTxt

            '    If vmskSelStrt > 0 Then
            '        msk_Date.SelectionStart = vmskSelStrt - 1
            '    End If

            'End If

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        lbl_Day.Text = ""
        If IsDate(dtp_Date.Text) = True Then
            lbl_Day.Text = Format(Convert.ToDateTime(dtp_Date.Text), "dddd").ToString
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub msk_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.TextChanged
        lbl_Day.Text = ""
        If IsDate(msk_Date.Text) = True Then
            lbl_Day.Text = Format(Convert.ToDateTime(msk_Date.Text), "dddd").ToString
        End If
    End Sub

    Private Sub txt_Narration_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Narration.GotFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        pnl_CurrentBalance.Visible = False
    End Sub

    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim OrdByNo As Double = 0

        If e.Control = True And UCase(Chr(e.KeyCode)) = "B" Then
            txt_Narration.Text = "BILL NO : "
            txt_Narration.SelectionStart = txt_Narration.Text.Length
        End If
        If e.Control = True And UCase(Chr(e.KeyCode)) = "C" Then
            txt_Narration.Text = "CHEQUE NO : "
            txt_Narration.SelectionStart = txt_Narration.Text.Length
        End If
        If e.Control = True And UCase(Chr(e.KeyCode)) = "R" Then
            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VouNo.Text))
            Da = New SqlClient.SqlDataAdapter("select top 1 * from Voucher_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and Voucher_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Voucher_No desc", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                txt_Narration.Text = Dt1.Rows(0).Item("Narration").ToString
            End If
            Dt1.Clear()
            txt_Narration.SelectionStart = txt_Narration.TextLength
        End If
        If e.KeyCode = 38 Then
            e.Handled = True
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        End If
        If e.KeyCode = 40 Then
            e.Handled = True
            btn_save.Focus()
        End If
    End Sub

    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub GrossAmount_Calculation()
        Dim TotDB As Double, TotCR As Double

        TotDB = 0
        TotCR = 0
        For i = 0 To dgv_Details.RowCount - 1
            TotDB = TotDB + Val(dgv_Details.Rows(i).Cells(2).Value)
            TotCR = TotCR + Val(dgv_Details.Rows(i).Cells(3).Value)
        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Common_Procedures.Currency_Format(Val(TotDB))
            .Rows(0).Cells(3).Value = Common_Procedures.Currency_Format(Val(TotCR))
        End With

    End Sub

    Private Sub Total_SelectionAmount_Calculation()
        Dim i As Integer
        Dim TotAmt As Double

        TotAmt = 0
        With dgv_Selection
            For i = 0 To .RowCount - 1
                TotAmt = TotAmt + Val(.Rows(i).Cells(6).Value)
            Next
        End With

        TotAmt = TotAmt + Val(txt_AdvanceAmount.Text)

        lbl_Total_BillAmount.Text = Common_Procedures.Currency_Format(Val(TotAmt))

    End Sub

    Private Sub Bill_Selection()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Led_IdNo As Integer
        Dim BilTyp As String
        Dim Cond As String = ""
        Dim NewCode As String = ""
        Dim n As Integer = 0
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim SNo As Integer = 0
        Dim Amt As Double = 0
        Dim AdvSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Details

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .CurrentRow.Cells(1).Value)
            If Led_IdNo <> 0 And Trim(.CurrentRow.Cells(0).Value) <> "" Then

                BilTyp = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Bill_Type", "(Ledger_Idno = " & Str(Val(Led_IdNo)) & ")")

                If Trim(UCase(BilTyp)) = "BILL TO BILL" Then

                    Cond = ""
                    If Trim(UCase(.CurrentRow.Cells(0).Value)) = "CR" Then Cond = "(Debit_Amount > Credit_Amount)" Else Cond = "(Credit_Amount > Debit_Amount)"

                    Cmd.Connection = con

                    Cmd.CommandText = "truncate table EntryTemp"
                    Cmd.ExecuteNonQuery()

                    Cmd.CommandText = "insert into EntryTemp ( Name1, Currency1, Currency2 ) Select voucher_bill_code, abs(credit_amount-debit_amount), 0 from voucher_bill_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(Led_IdNo)) & " and " & Trim(Cond)
                    Cmd.ExecuteNonQuery()

                    Cmd.CommandText = "insert into EntryTemp ( Name1, Currency1, Currency2 ) Select voucher_bill_code, amount, amount from voucher_bill_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and ledger_idno = " & Str(Val(Led_IdNo)) & " and crdr_type = '" & Trim(dgv_Details.CurrentRow.Cells(0).Value) & "'"
                    Cmd.ExecuteNonQuery()

                    Cmd.CommandText = "truncate table reporttempsub"
                    Cmd.ExecuteNonQuery()

                    Cmd.CommandText = "insert into reporttempsub ( name1, currency1, currency2 ) Select name1, sum(currency1), sum(currency2) from EntryTemp group by name1"
                    Cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select b.Party_Bill_No, b.Voucher_Bill_Date, c.ledger_name as AgentName, a.currency1 as BillAmount, (case when (b.credit_amount>b.debit_amount) then 'Cr' else 'Dr' end) as CrDrType, a.currency2 as Paid_Rcvd_Amount, a.name1 as Voucher_Bill_Code from reporttempsub a INNER JOIN voucher_bill_head b ON  a.name1 = b.voucher_bill_code LEFT OUTER JOIN ledger_head c ON b.agent_idno = c.ledger_idno where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " order by a.currency2 desc, b.voucher_bill_date", con)
                    'Da = New SqlClient.SqlDataAdapter("Select b.Party_Bill_No, b.Voucher_Bill_Date, c.ledger_name as AgentName, a.currency1 as BillAmount, (case when (b.credit_amount>b.debit_amount) then 'Cr' else 'Dr' end) as CrDrType, a.currency2 as Paid_Rcvd_Amount, a.name1 as Voucher_Bill_Code from reporttempsub a, voucher_bill_head b, ledger_head c where b.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.name1 = b.voucher_bill_code and b.agent_idno *= c.ledger_idno order by a.currency2 desc, b.voucher_bill_date", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)

                    dgv_Selection.Rows.Clear()
                    txt_BillNo.Text = ""
                    txt_AdvanceAmount.Text = ""
                    lbl_Advance_AdjustAmount.Text = ""
                    lbl_AdvanceReceiptNo.Text = ""
                    lbl_Total_BillAmount.Text = ""
                    If Trim(UCase(cbo_AdvanceType.Text)) <> "BILL" Then
                        txt_BillNo.Enabled = False
                        txt_BillNo.Text = "Advance"
                        lbl_AdvAmount_Caption.Text = "Advance Amount"
                    End If

                    SNo = 0

                    If Dt1.Rows.Count > 0 Then

                        For i = 0 To Dt1.Rows.Count - 1

                            n = dgv_Selection.Rows.Add()

                            SNo = SNo + 1

                            dgv_Selection.Rows(n).Cells(0).Value = Val(SNo)

                            dgv_Selection.Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Party_Bill_No").ToString
                            dgv_Selection.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Voucher_Bill_Date").ToString), "dd-MM-yyyy")
                            dgv_Selection.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("AgentName").ToString
                            dgv_Selection.Rows(n).Cells(4).Value = Trim(Format(Math.Abs(Val(Dt1.Rows(i).Item("BillAmount").ToString)), "#########0.00"))
                            dgv_Selection.Rows(n).Cells(5).Value = Trim(UCase(Dt1.Rows(i).Item("CrDrType").ToString))
                            dgv_Selection.Rows(n).Cells(6).Value = Trim(Format(Math.Abs(Val(Dt1.Rows(i).Item("Paid_Rcvd_Amount").ToString)), "#########0.00"))
                            dgv_Selection.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Voucher_Bill_Code").ToString

                        Next i

                    End If

                    If New_Entry = True Then

                        Amt = 0
                        If Trim(UCase(dgv_Details.CurrentRow.Cells(0).Value)) = "DR" Then Amt = Val(dgv_Details.CurrentRow.Cells(2).Value) Else Amt = Val(dgv_Details.CurrentRow.Cells(3).Value)

                        i = 0
                        Do While Amt > 0 And i <= dgv_Selection.Rows.Count - 1
                            If Val(dgv_Selection.Rows(i).Cells(4).Value) > Amt Then
                                dgv_Selection.Rows(i).Cells(6).Value = Format(Val(Amt), "#########0.00")
                                Amt = 0

                            Else
                                dgv_Selection.Rows(i).Cells(6).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                                Amt = Amt - Val(dgv_Selection.Rows(i).Cells(4).Value)

                            End If

                            i = i + 1

                        Loop

                    End If

                    AdvSTS = False
                    For k = 0 To dgv_SelectionDetails.Rows.Count - 1
                        If Val(dgv_SelectionDetails.Rows(k).Cells(0).Value) = Val(Led_IdNo) And Trim(UCase(dgv_SelectionDetails.Rows(k).Cells(3).Value)) = Trim(UCase(.CurrentRow.Cells(0).Value)) And Trim(UCase(dgv_SelectionDetails.Rows(k).Cells(4).Value)) = "ADV" Then
                            AdvSTS = True
                            lbl_AdvanceReceiptNo.Text = dgv_SelectionDetails.Rows(k).Cells(1).Value
                            txt_AdvanceAmount.Text = Val(dgv_SelectionDetails.Rows(k).Cells(2).Value)
                            txt_BillNo.Text = dgv_SelectionDetails.Rows(k).Cells(5).Value
                            lbl_Advance_AdjustAmount.Text = Val(dgv_SelectionDetails.Rows(k).Cells(6).Value)
                        End If
                    Next

                    If AdvSTS = False Then
                        If Amt <> 0 Then
                            txt_AdvanceAmount.Text = Format(Val(Amt), "#########0.00")
                        End If
                    End If

                    Call Total_SelectionAmount_Calculation()

                    pnl_Selection.Visible = True
                    pnl_Selection.BringToFront()
                    pnl_Back.Enabled = False

                    If dgv_Selection.Rows.Count > 0 Then
                        dgv_Selection.Focus()
                        dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(6)

                    Else
                        If txt_BillNo.Enabled = True Then txt_BillNo.Focus() Else txt_AdvanceAmount.Focus()

                    End If

                    Exit Sub

                End If

            End If

        End With

    End Sub

    Private Sub Close_BillSelection()
        Dim i As Integer
        Dim n As Integer
        Dim Led_IdNo As Integer = 0
        Dim vVouAmt As Double = 0

        With dgv_Details

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .CurrentRow.Cells(1).Value)
            If Led_IdNo <> 0 And Trim(.CurrentRow.Cells(0).Value) <> "" Then

                For i = 0 To dgv_Selection.Rows.Count - 1
                    If Val(dgv_Selection.Rows(i).Cells(6).Value) <> 0 Then
                        If Val(dgv_Selection.Rows(i).Cells(6).Value) > Val(dgv_Selection.Rows(i).Cells(4).Value) Then
                            MessageBox.Show("Invalid Receipt/Payment Amount", "INVALID BILL SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            dgv_Selection.Focus()
                            dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(6)
                            dgv_Selection.CurrentCell.Selected = True
                            Exit Sub
                        End If
                    End If
                Next i

                If Val(lbl_Advance_AdjustAmount.Text) > Val(txt_AdvanceAmount.Text) Then
                    MessageBox.Show("Invalid Advance/NewBill Amount, Lesser than Received/paid Amount", "INVALID BILL SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    txt_AdvanceAmount.Focus()
                    Exit Sub
                End If

                vVouAmt = 0
                If Trim(UCase(.CurrentRow.Cells(0).Value)) = "DR" Then vVouAmt = Format(Val(.CurrentRow.Cells(2).Value), "#########0.00")
                If Trim(UCase(.CurrentRow.Cells(0).Value)) = "CR" Then vVouAmt = Format(Val(.CurrentRow.Cells(3).Value), "#########0.00")

                'If Format(Val(CDbl(lbl_Total_BillAmount.Text)), "##########0.00") <> Format(Val(vVouAmt), "##########0.00") Then
                'MessageBox.Show("Invalid Bill Details, Mismatch of voucher and bill Amount", "INVALID BILL SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'If dgv_Selection.Rows.Count > 0 Then
                '    dgv_Selection.Focus()
                '    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(6)
                '    dgv_Selection.CurrentCell.Selected = True

                'Else
                '    txt_AdvanceAmount.Focus()

                'End If

                'Exit Sub

                'End If

LOOP1:
                For i = 0 To dgv_SelectionDetails.Rows.Count - 1
                    If Val(dgv_SelectionDetails.Rows(i).Cells(0).Value) = Val(Led_IdNo) And Trim(UCase(dgv_SelectionDetails.Rows(i).Cells(3).Value)) = Trim(UCase(.CurrentRow.Cells(0).Value)) Then
                        dgv_SelectionDetails.Rows.RemoveAt(i)
                        GoTo LOOP1
                    End If
                Next i

                For i = 0 To dgv_Selection.Rows.Count - 1
                    If Val(dgv_Selection.Rows(i).Cells(6).Value) <> 0 Then

                        n = dgv_SelectionDetails.Rows.Add
                        dgv_SelectionDetails.Rows(n).Cells(0).Value = Val(Led_IdNo)
                        dgv_SelectionDetails.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(7).Value
                        dgv_SelectionDetails.Rows(n).Cells(2).Value = Val(dgv_Selection.Rows(i).Cells(6).Value)
                        dgv_SelectionDetails.Rows(n).Cells(3).Value = dgv_Details.CurrentRow.Cells(0).Value
                        dgv_SelectionDetails.Rows(n).Cells(4).Value = "BILL"
                        dgv_SelectionDetails.Rows(n).Cells(5).Value = ""
                        dgv_SelectionDetails.Rows(n).Cells(6).Value = ""

                    End If

                Next i

                If Val(lbl_Advance_AdjustAmount.Text) > 0 Or Val(txt_AdvanceAmount.Text) > 0 Then
                    n = dgv_SelectionDetails.Rows.Add
                    dgv_SelectionDetails.Rows(n).Cells(0).Value = Val(Led_IdNo)
                    dgv_SelectionDetails.Rows(n).Cells(1).Value = lbl_AdvanceReceiptNo.Text
                    dgv_SelectionDetails.Rows(n).Cells(2).Value = Val(txt_AdvanceAmount.Text)
                    dgv_SelectionDetails.Rows(n).Cells(3).Value = .CurrentRow.Cells(0).Value
                    dgv_SelectionDetails.Rows(n).Cells(4).Value = "ADV"
                    dgv_SelectionDetails.Rows(n).Cells(5).Value = txt_BillNo.Text
                    dgv_SelectionDetails.Rows(n).Cells(6).Value = Val(lbl_Advance_AdjustAmount.Text)
                End If

                If Trim(UCase(.CurrentRow.Cells(0).Value)) = "DR" Then .CurrentRow.Cells(2).Value = Format(Val(CDbl(lbl_Total_BillAmount.Text)), "##########0.00")
                If Trim(UCase(.CurrentRow.Cells(0).Value)) = "CR" Then .CurrentRow.Cells(3).Value = Format(Val(CDbl(lbl_Total_BillAmount.Text)), "##########0.00")

                pnl_Back.Enabled = True
                pnl_Selection.Visible = False

                If (.CurrentRow.Index + 1) <= .Rows.Count - 1 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(0)

                Else
                    txt_Narration.Focus()

                End If

            End If

        End With

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Order_Selection.Click
        ' Bill_Selection()
        pnl_Back.Enabled = False
        pnl_OrderList.Visible = True

        dgv_OrderList.Focus()
        dgv_OrderList.CurrentCell = dgv_OrderList.Rows(0).Cells(1)


    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_BillSelection()
    End Sub

    Private Sub cbo_AdvanceType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AdvanceType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AdvanceType, Nothing, Nothing, "", "", "", "")
        If (e.KeyValue = 38 And cbo_AdvanceType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(6)
            End If
        End If
        If (e.KeyValue = 40 And cbo_AdvanceType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            If txt_BillNo.Enabled Then
                txt_BillNo.Focus()
            Else
                txt_AdvanceAmount.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_AdvanceType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AdvanceType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AdvanceType, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If txt_BillNo.Enabled Then
                txt_BillNo.Focus()
            Else
                txt_AdvanceAmount.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_AdvanceType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_AdvanceType.TextChanged
        If Trim(UCase(cbo_AdvanceType.Text)) = "BILL" Then
            txt_BillNo.Enabled = True
            lbl_AdvAmount_Caption.Text = "Bill Amount"

        Else
            txt_BillNo.Enabled = False
            txt_BillNo.Text = "Advance"
            lbl_AdvAmount_Caption.Text = "Advance Amount"

        End If
    End Sub

    Private Sub txt_AdvanceAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AdvanceAmount.TextChanged
        Total_SelectionAmount_Calculation()
    End Sub

    Private Sub dgv_Selection_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellValueChanged
        On Error Resume Next
        With dgv_Selection
            If .CurrentCell.ColumnIndex = 6 Then
                Total_SelectionAmount_Calculation()
            End If
        End With
    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(6)
            Else
                txt_AdvanceAmount.Focus()
            End If

        End If
        If e.KeyCode = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_BillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BillNo.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_AdvanceAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AdvanceAmount.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            SendKeys.Send("+{TAB}")
        End If
        'If e.KeyCode = 40 Then
        '    e.Handled = True
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txt_AdvanceAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AdvanceAmount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Call Close_BillSelection()
        End If
    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
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

    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then '---- Jeno Textiles (Somanur)

            pnl_Print_Voucher.Visible = True
            pnl_Print_Voucher.BringToFront()
            pnl_Back.Enabled = False
            If btn_Print_Ordinary_Voucher.Enabled And btn_Print_Ordinary_Voucher.Visible Then
                btn_Print_Ordinary_Voucher.Focus()
            End If

        Else

            If Trim(LCase(lbl_VouType.Text)) = "pymt" Or Trim(LCase(lbl_VouType.Text)) = "cntr" Then
                pnl_Voucher_ChequePrint.Visible = True
                pnl_Voucher_ChequePrint.BringToFront()
                pnl_Back.Enabled = False

                If btn_PrintVoucher.Enabled And btn_PrintVoucher.Visible Then
                    btn_PrintVoucher.Focus()
                End If

            Else

                prn_Status = 1
                Pnl_PrintRange.Visible = True
                Pnl_PrintRange.BringToFront()
                pnl_Back.Enabled = False
                txt_PrintFromNo.Text = lbl_VouNo.Text
                txt_PrintToNo.Text = lbl_VouNo.Text
                If txt_PrintFromNo.Enabled And txt_PrintFromNo.Visible Then
                    txt_PrintFromNo.Focus()
                    txt_PrintFromNo.SelectAll()
                End If

            End If

        End If

    End Sub

    Private Sub btn_Print_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_PrintRange.Click
        prn_Status = 1
        Printing_Voucher()
        btn_Close_PrintRange_Click(sender, e)
    End Sub

    Private Sub txt_PrintFromNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintFromNo.KeyDown
        If e.KeyCode = Keys.Down Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_PrintToNo.Focus()
        End If
    End Sub

    Private Sub txt_PrintFromNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintFromNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_PrintToNo.Focus()
        End If
    End Sub

    Private Sub txt_PrintToNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintToNo.KeyDown
        If e.KeyCode = Keys.Down Then
            e.Handled = True
            e.SuppressKeyPress = True
            btn_Print_PrintRange.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_PrintFromNo.Focus()
        End If
    End Sub

    Private Sub txt_PrintToNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintToNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            btn_Print_PrintRange_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Cancel_PrintRange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintRange.Click
        btn_Close_PrintRange_Click(sender, e)
    End Sub

    Private Sub btn_Close_PrintRange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_PrintRange.Click
        pnl_Back.Enabled = True
        Pnl_PrintRange.Visible = False
        pnl_Voucher_ChequePrint.Visible = False
        pnl_Print_Voucher.Visible = False
    End Sub

    Private Sub Printing_Voucher()
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim entcode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFromNo.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintToNo.Text))

        Condt = ""
        If Val(txt_PrintFromNo.Text) <> 0 And Val(txt_PrintToNo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Format(Val(prtFrm), "#########0.00")) & " and " & Str(Format(Val(prtTo), "#########0.00"))

        ElseIf Val(txt_PrintFromNo.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Format(Val(prtFrm), "#########0.00"))

        ElseIf Val(txt_PrintToNo.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Format(Val(prtTo), "#########0.00"))

        Else
            Exit Sub

        End If

        entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If Trim(LCase(lbl_VouType.Text)) = "purc" Or Trim(LCase(lbl_VouType.Text)) = "rcpt" Or Trim(LCase(lbl_VouType.Text)) = "csrp" Or Trim(LCase(lbl_VouType.Text)) = "crnt" Then
                Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4 from voucher_head a, ledger_head c, ledger_head d where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and a.Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.creditor_idno = c.ledger_idno and a.debtor_idno = d.ledger_idno", con)
                'Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4 from voucher_head a, ledger_head c, ledger_head d where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Code = '" & Trim(entcode) & "'  and a.creditor_idno = c.ledger_idno and a.debtor_idno = d.ledger_idno " & IIf(Trim(Condt) <> "", " and ", "") & Condt, con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
            Else
                Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4 from voucher_head a, ledger_head c, ledger_head d where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and a.Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.debtor_idno = c.ledger_idno and a.creditor_idno = d.ledger_idno", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
            End If

            If Dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            Dt1.Dispose()
            Da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            'Debug.Print(ps.PaperName)
                            If ps.Width = 800 And ps.Height = 600 Then
                                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                PrintDocument1.DefaultPageSettings.PaperSize = ps
                                PpSzSTS = True
                                Exit For
                            End If
                        Next

                        If PpSzSTS = False Then
                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PpSzSTS = True
                                    Exit For
                                End If
                            Next

                            If PpSzSTS = False Then
                                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                                        Exit For
                                    End If
                                Next
                            End If

                        End If

                        PrintDocument1.Print()

                    End If

                Else

                    PrintDocument1.Print()

                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(800, 800)
                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
        pnl_Back.Enabled = True
        Pnl_PrintRange.Visible = False
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim entcode As String
        Dim PrnHeading As String
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""

        Try

            If Val(txt_PrintFromNo.Text) = 0 Then Exit Sub
            If Val(txt_PrintToNo.Text) = 0 Then Exit Sub

            prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFromNo.Text))
            prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintToNo.Text))

            Condt = ""
            If Val(txt_PrintFromNo.Text) <> 0 And Val(txt_PrintToNo.Text) <> 0 Then
                Condt = " a.for_OrderBy between " & Str(Format(Val(prtFrm), "#########0.00")) & " and " & Str(Format(Val(prtTo), "#########0.00"))

            ElseIf Val(txt_PrintFromNo.Text) <> 0 Then
                Condt = " a.for_OrderBy = " & Str(Format(Val(prtFrm), "#########0.00"))

            ElseIf Val(txt_PrintToNo.Text) <> 0 Then
                Condt = " a.for_OrderBy = " & Str(Format(Val(prtTo), "#########0.00"))

            Else
                Exit Sub

            End If

            entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            prn_HdDt = New DataTable
            prn_PageNo = 0
            prn_HeadIndx = 0

            If Trim(LCase(lbl_VouType.Text)) = "purc" Or Trim(LCase(lbl_VouType.Text)) = "rcpt" Or Trim(LCase(lbl_VouType.Text)) = "csrp" Or Trim(LCase(lbl_VouType.Text)) = "crnt" Then
                Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4, b.* from voucher_head a, ledger_head c, ledger_head d, Company_Head b Where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and a.Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.creditor_idno = c.ledger_idno and a.debtor_idno = d.ledger_idno and a.Company_IdNo = b.Company_IdNo order by a.for_OrderBy, a.voucher_no, a.voucher_code", con)
                'Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4,b.*  from voucher_head a, ledger_head c, ledger_head d,Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Code = '" & Trim(entcode) & "' and a.creditor_idno = c.ledger_idno and a.debtor_idno = d.ledger_idno and a.Company_IdNo = b.Company_IdNo" & IIf(Trim(Condt) <> "", " and ", "") & Condt, con)
                prn_HdDt = New DataTable
                Da1.Fill(prn_HdDt)
            Else
                Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4, b.* from voucher_head a, ledger_head c, ledger_head d, Company_Head b where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Type = '" & Trim(lbl_VouType.Text) & "' and a.Year_For_Report = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.debtor_idno = c.ledger_idno and a.creditor_idno = d.ledger_idno and a.Company_IdNo = b.Company_IdNo order by a.for_OrderBy, a.voucher_no, a.voucher_code", con)
                'Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4,b.* from voucher_head a, ledger_head c, ledger_head d,Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Code = '" & Trim(entcode) & "' and a.debtor_idno = c.ledger_idno and a.creditor_idno = d.ledger_idno and a.Company_IdNo = b.Company_IdNo" & IIf(Trim(Condt) <> "", " and ", "") & Condt, con)
                prn_HdDt = New DataTable
                Da1.Fill(prn_HdDt)
            End If
            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            Select Case Trim(LCase(lbl_VouType.Text))
                Case "purc"
                    PrnHeading = "PURCHASE VOUCHER"
                Case "sale"
                    PrnHeading = "SALES VOUCHER"
                Case "pymt"
                    PrnHeading = "PAYMENT VOUCHER"
                Case "rcpt"
                    PrnHeading = "RECEIPT VOUCHER"
                Case "cntr"
                    PrnHeading = "CONTRA VOUCHER"
                Case "jrnl"
                    PrnHeading = "JOURNAL VOUCHER"
                Case "crnt"
                    PrnHeading = "CREDIT NOTE VOUCHER"
                Case "dbnt"
                    PrnHeading = "DEBIT NOTE VOUCHER"
                Case "ptcs"
                    PrnHeading = "PETTI CASH VOUCHER"
            End Select


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If prn_Status = 1 Then
            Printing_Format1(e)
        Else
            Printing_Format2_Jeno(e)
        End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PrnHeading As String = ""

        If prn_HeadIndx > prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = False
            Exit Sub
        End If

        If prn_HeadIndx <= 0 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                'Debug.Print(ps.PaperName)
                If ps.Width = 800 And ps.Height = 600 Then
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            PrintDocument1.DefaultPageSettings.PaperSize = ps
                            e.PageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

            End If

            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 800)
            'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.PrinterSettings.DefaultPageSettings.Landscape = False
            'PrintDocument1.DefaultPageSettings.Landscape = False
            'e.PageSettings.Landscape = False

            ' ''PrintDocument pd = new PrintDocument();
            ' ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
            ' ''pd.Print();

            ''For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ''    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            ''    Debug.Print(ps.PaperName)
            ''    If ps.Width = 800 And ps.Height = 600 Then
            ''        PrintDocument1.DefaultPageSettings.PaperSize = ps
            ''        e.PageSettings.PaperSize = ps
            ''        PpSzSTS = True
            ''        Exit For
            ''    End If
            ''Next

            ''If PpSzSTS = False Then
            ''    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ''        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            ''            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            ''            PrintDocument1.DefaultPageSettings.PaperSize = ps
            ''            e.PageSettings.PaperSize = ps
            ''            PpSzSTS = True
            ''            Exit For
            ''        End If
            ''    Next

            ''    If PpSzSTS = False Then
            ''        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ''            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            ''                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            ''                PrintDocument1.DefaultPageSettings.PaperSize = ps
            ''                e.PageSettings.PaperSize = ps
            ''                Exit For
            ''            End If
            ''        Next
            ''    End If

            ''End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50 ' 20
            .Right = 60 ' 50
            .Top = 30
            .Bottom = 30

            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize


            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 18.5 ' 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(525) : ClArr(2) = 100
        ClArr(3) = PageWidth - (LMargin + ClArr(1))

        'CurY = TMargin
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
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

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 13, FontStyle.Bold)


        Select Case Trim(LCase(lbl_VouType.Text))
            Case "purc"
                PrnHeading = "PURCHASE VOUCHER"
            Case "sale"
                PrnHeading = "SALES VOUCHER"
            Case "pymt", "cspy"
                PrnHeading = "PAYMENT VOUCHER"
            Case "rcpt", "csrp"
                PrnHeading = "RECEIPT VOUCHER"
            Case "cntr"
                PrnHeading = "CONTRA VOUCHER"
            Case "jrnl"
                PrnHeading = "JOURNAL VOUCHER"
            Case "crnt"
                PrnHeading = "CREDIT NOTE VOUCHER"
            Case "dbnt"
                PrnHeading = "DEBIT NOTE VOUCHER"
            Case "ptcs"
                PrnHeading = "PETTI CASH VOUCHER"
        End Select


        Common_Procedures.Print_To_PrintDocument(e, PrnHeading, LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("Voucher No  : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(prn_HeadIndx).Item("To_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Voucher No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Voucher Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + 8

        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin, CurY, 2, ClArr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, " AMOUNT  ", LMargin + ClArr(1) + 75, CurY, 2, ClArr(2), pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        CurY = CurY + 13
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(3))


        Common_Procedures.Print_To_PrintDocument(e, "By " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("By_Name").ToString), LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Narration : ", LMargin + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Narration").ToString), LMargin + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 30 ' 40
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(3))


        CurY = CurY + TxtHgt - 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString))
        'BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")
        Common_Procedures.Print_To_PrintDocument(e, "Rupees  :   " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "checked", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Signature ", PageWidth - 20, CurY, 1, 0, pFont)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(7), LMargin, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(7), PageWidth, LnAr(2))

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

    End Sub

    Private Sub Printing_Format2_Jeno(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim I As Integer, NoofItems_PerPage As Integer
        Dim AmtInWrds As String = ""
        Dim PrnHeading As String = ""

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

        pFont = New Font("Calibri", 11, FontStyle.Regular)
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

        TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        NoofItems_PerPage = 5

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


            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 125 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(prn_HeadIndx).Item("To_Name").ToString, CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, CurX, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, CurX, CurY, 0, 0, pFont)
            End If
            'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString, CurX, CurY, 0, 0, pFont)
            'End If

            CurX = LMargin + 500
            CurY = TMargin + 120

            PrnHeading = ""
            Select Case Trim(LCase(lbl_VouType.Text))
                Case "purc"
                    PrnHeading = "PURCHASE VOUCHER"
                Case "sale"
                    PrnHeading = "SALES VOUCHER"
                Case "pymt", "cspy"
                    PrnHeading = "PAYMENT VOUCHER"
                Case "rcpt", "csrp"
                    PrnHeading = "RECEIPT VOUCHER"
                Case "cntr"
                    PrnHeading = "CONTRA VOUCHER"
                Case "jrnl"
                    PrnHeading = "JOURNAL VOUCHER"
                Case "crnt"
                    PrnHeading = "CREDIT NOTE VOUCHER"
                Case "dbnt"
                    PrnHeading = "DEBIT NOTE VOUCHER"
                Case "ptcs"
                    PrnHeading = "PETTI CASH VOUCHER"
            End Select

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, PrnHeading, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 465
            CurY = TMargin + 150
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_No").ToString, CurX, CurY, 0, 0, p1Font)
            CurY = TMargin + 170
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Removal_Time").ToString, CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 150
            CurY = TMargin + 190

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 570

            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", CurX, CurY, 0, 0, p1Font)
            CurX = LMargin + 390


            CurX = LMargin + 75
            CurY = TMargin + 260


            Common_Procedures.Print_To_PrintDocument(e, "By " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("By_Name").ToString), CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 750


            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)
            CurX = LMargin + 75
            CurY = TMargin + 300

            Common_Procedures.Print_To_PrintDocument(e, "Narration : ", CurX, CurY, 0, 0, pFont)
            CurX = LMargin + 200

            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Narration").ToString), CurX, CurY, 0, 0, pFont)

            CurY = TMargin + 350
            e.Graphics.DrawLine(Pens.Black, LMargin + 30, CurY, LMargin + 770, CurY)

            CurX = LMargin + 750
            CurY = TMargin + 360
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)

            CurY = TMargin + 395
            e.Graphics.DrawLine(Pens.Black, LMargin + 30, CurY, LMargin + 770, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + 560, CurY, LMargin + 560, TMargin + CurY)

            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            CurX = LMargin + 75
            CurY = TMargin + 405
            AmtInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString))
            AmtInWrds = Replace(Trim(LCase(AmtInWrds)), "", "")
            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & AmtInWrds & " ", CurX, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 10

            CurY = TMargin + 455
            e.Graphics.DrawLine(Pens.Black, LMargin + 30, CurY, LMargin + 770, CurY)


            '        CurY = TMargin + 460
            '        e.Graphics.DrawLine(Pens.Black, LMargin + 30, CurY, LMargin + 770, CurY)


            '        CurY = TMargin + 500
            '        e.Graphics.DrawLine(Pens.Black, LMargin + 30, CurY, LMargin + 770, CurY)

            '        CurY = TMargin + 500
            '        e.Graphics.DrawLine(Pens.Black, LMargin + 100, CurY, LMargin + 100, TMargin + 200)
            '        e.Graphics.DrawLine(Pens.Black, LMargin + 360, CurY, LMargin + 360, TMargin + 200)
            '        e.Graphics.DrawLine(Pens.Black, LMargin + 480, CurY, LMargin + 480, TMargin + 200)
            '        e.Graphics.DrawLine(Pens.Black, LMargin + 580, CurY, LMargin + 580, TMargin + 200)
            '        e.Graphics.DrawLine(Pens.Black, LMargin + 680, CurY, LMargin + 680, TMargin + 200)

            '        CurY = TMargin + 500
            '        e.Graphics.DrawLine(Pens.Black, LMargin + 30, CurY, LMargin + 770, CurY)

            '        CurX = LMargin + 300
            '        CurY = TMargin + 475
            '        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", CurX, CurY, 0, 0, pFont)

            '        CurX = LMargin + 475
            '        CurY = TMargin + 475

            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Quantity").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)

            '        CurX = LMargin + 560

            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)


            '    End If


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub
    Private Sub Printing_Format3_ChequePrint(ByRef e As System.Drawing.Printing.PrintPageEventArgs)   '----------Cheque Printing--------------
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Double = 0
        Dim CurY As Double = 0
        Dim CurZ As Double = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PrnHeading As String = ""
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim prn_CheqDet As New DataTable
        Dim W As Single
        Dim dtWdth As Single
        Dim Amt As String
        Dim Rup1 As String, Rup2 As String
        Dim m As Integer
        Dim PrtyNm1 As String, PrtyNm2 As String

        For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument2.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next
        PrintDocument2.DefaultPageSettings.Landscape = False

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 30 ' 40
            .Right = 45
            .Top = 50 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        Da1 = New SqlClient.SqlDataAdapter("Select *  from Cheque_Print_Positioning_Head where Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Creditor_Idno").ToString)) & "order by Cheque_Print_Positioning_No", con)
        Da1.Fill(prn_CheqDet)

        LMargin = LMargin + Val(prn_CheqDet.Rows(0).Item("Left_Margin").ToString)
        TMargin = TMargin + Val(prn_CheqDet.Rows(0).Item("Top_Margin").ToString)

        If prn_CheqDet.Rows.Count > 0 Then
            If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) <> "" Then
                If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                    If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) = "LANDSCAPE" Then
                        PrintDocument2.DefaultPageSettings.Landscape = True
                        If PrintDocument2.DefaultPageSettings.Landscape = True Then
                            With PrintDocument2.DefaultPageSettings.PaperSize
                                PrintWidth = .Height - TMargin - BMargin
                                PrintHeight = .Width - RMargin - LMargin
                                PageWidth = .Height - TMargin
                                PageHeight = .Width - RMargin
                            End With
                        End If
                    Else
                        With PrintDocument2.DefaultPageSettings.PaperSize
                            PrintWidth = .Width - RMargin - LMargin
                            PrintHeight = .Height - TMargin - BMargin
                            PageWidth = .Width - RMargin
                            PageHeight = .Height - BMargin
                        End With
                    End If
                End If
            End If

            If PpSzSTS = False Then

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            e.PageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

            End If
        End If




        'With PrintDocument2.DefaultPageSettings.PaperSize
        '    PrintWidth = .Width - RMargin - LMargin
        '    PrintHeight = .Height - TMargin - BMargin
        '    PageWidth = .Width - RMargin
        '    PageHeight = .Height - BMargin
        'End With


        pFont = New Font("Calibri", 11, FontStyle.Regular)
        TxtHgt = 18.5


        If Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Top").ToString) <> 0 Then
            If Not Common_Procedures.VoucherType = "Cntr" Then
                CurX = Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Left").ToString) / 2.54 * 100
                CurY = Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Top").ToString) / 2.54 * 100
                Common_Procedures.Print_To_PrintDocument(e, "A/C PAYEE", LMargin + CurX, TMargin + CurY, 0, 0, pFont)
            End If
        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("Date_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("Date_Top").ToString) / 2.54 * 100
        dtWdth = Val(prn_CheqDet.Rows(0).Item("Date_Width").ToString)

        If dtWdth > 0 Then
            W = CurX
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 1, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 2, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            '  Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 3, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            '  W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 4, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 5, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            ' Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 6, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            ' W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 7, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 8, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 9, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 10, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
        Else

            If Val(prn_CheqDet.Rows(0).Item("Date_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Date_Top").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), LMargin + CurX, TMargin + CurY, 1, 0, pFont)
            End If

        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("PartyName_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("PartyName_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("PartyName_Width").ToString)
        If CurX <> 0 And CurY <> 0 Then
            If Common_Procedures.VoucherType = "Cntr" Then
                Common_Procedures.Print_To_PrintDocument(e, "Self", LMargin + CurX, TMargin + CurY, 0, 0, pFont)
            Else
                PrtyNm2 = ""
                PrtyNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("by_name").ToString)
                If Len(PrtyNm1) > CurZ Then
                    For m = CurZ To 1 Step -1
                        If Mid$(Trim(PrtyNm1), m, 1) = " " Then Exit For
                    Next m
                    If m <> 0 Then
                        PrtyNm2 = Microsoft.VisualBasic.Right(Trim(PrtyNm1), Len(PrtyNm1) - m)
                        PrtyNm1 = Microsoft.VisualBasic.Left(Trim(PrtyNm1), m - 1)
                        m = 0
                    End If
                End If
                Common_Procedures.Print_To_PrintDocument(e, PrtyNm1, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
                Common_Procedures.Print_To_PrintDocument(e, PrtyNm2, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
            End If
        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("AmountWords_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("AmountWords_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("AmountWords_Width").ToString)

        If CurX <> 0 And CurY <> 0 Then
            'If CurX <> 0 And CurY <> 0 And CurZ <> 0 Then
            Amt = Microsoft.VisualBasic.Left(Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString))), Len(Trim(Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString))))) - 3) & "/--"
            Rup2 = ""
            Rup1 = Common_Procedures.Rupees_Converstion(Math.Abs(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString)))
            If Len(Rup1) > CurZ Then
                For m = CurZ To 1 Step -1
                    If Mid$(Trim(Rup1), m, 1) = " " Then Exit For
                Next m
                If m <> 0 Then
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - m)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), m - 1)
                    m = 0
                End If
            End If
            Common_Procedures.Print_To_PrintDocument(e, Rup1, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

            CurX = Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Left").ToString) / 2.54 * 100
            CurY = Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Top").ToString) / 2.54 * 100
            CurZ = Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Width").ToString)
            Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

            CurX = Val(prn_CheqDet.Rows(0).Item("Rupees_Left").ToString) / 2.54 * 100
            CurY = Val(prn_CheqDet.Rows(0).Item("Rupees_Top").ToString) / 2.54 * 100
            CurZ = Val(prn_CheqDet.Rows(0).Item("Rupees_Width").ToString)
            Common_Procedures.Print_To_PrintDocument(e, "**" & Amt, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("CompanyName_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("CompanyName_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("CompanyName_Width").ToString)
        If CurX <> 0 And CurY <> 0 Then Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

        CurX = Val(prn_CheqDet.Rows(0).Item("Partner_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("Partner_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("Partner_Width").ToString)
        If CurX <> 0 And CurY <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Trim(prn_CheqDet.Rows(0).Item("Partner").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

        CurX = Val(prn_CheqDet.Rows(0).Item("AccountNo_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("AccountNo_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("AccountNo_Width").ToString)
        If CurX <> 0 And CurY <> 0 Then Common_Procedures.Print_To_PrintDocument(e, "ACC NO. " & Trim(prn_CheqDet.Rows(0).Item("Account_No").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)


        e.HasMorePages = False

    End Sub
    Private Sub printing_Cheque()
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da2 As SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim entcode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        'Dim ps As Printing.PaperSize

        entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try


            Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date,a.creditor_idno, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4 ,b.* from voucher_head a, ledger_head c, ledger_head d , Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Code = '" & Trim(entcode) & "' and a.creditor_idno = c.ledger_idno and a.debtor_idno = d.ledger_idno", con)
            Da1.Fill(Dt1)


            If Dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            Da2 = New SqlClient.SqlDataAdapter("Select *  from Cheque_Print_Positioning_Head where Ledger_IdNo = " & Str(Val(Dt1.Rows(0).Item("Creditor_Idno").ToString)) & "order by Cheque_Print_Positioning_No", con)
            Da2.Fill(Dt2)
            If Dt2.Rows.Count <= 0 Then

                MessageBox.Show("Cheque Printing Position not Found ", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dt1.Dispose()
            Da1.Dispose()
            Dt2.Dispose()
            Da2.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument2.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument2

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()


            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim prn_CheqDet As New DataTable
        Dim ps As Printing.PaperSize
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim PpSzSTS As Boolean = False

        Dim entcode As String

        entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt = New DataTable
        prn_PageNo = 0


        Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration,a.Creditor_Idno, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4,b.*  from voucher_head a, ledger_head c, ledger_head d,Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Code = '" & Trim(entcode) & "' and a.creditor_idno = c.ledger_idno and a.debtor_idno = d.ledger_idno and a.Company_IdNo = b.Company_IdNo", con)
        Da1.Fill(prn_HdDt)
        If prn_HdDt.Rows.Count <= 0 Then

            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

        Da1 = New SqlClient.SqlDataAdapter("Select *  from Cheque_Print_Positioning_Head where Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Creditor_Idno").ToString)) & "order by Cheque_Print_Positioning_No", con)
        Da1.Fill(prn_CheqDet)

        If prn_CheqDet.Rows.Count > 0 Then
            If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) <> "" Then
                If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                    If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) = "LANDSCAPE" Then
                        PrintDocument2.DefaultPageSettings.Landscape = True
                        If PrintDocument2.DefaultPageSettings.Landscape = True Then
                            With PrintDocument2.DefaultPageSettings.PaperSize
                                PrintWidth = .Height - TMargin - BMargin
                                PrintHeight = .Width - RMargin - LMargin
                                PageWidth = .Height - TMargin
                                PageHeight = .Width - RMargin
                            End With
                        End If
                    Else
                        With PrintDocument2.DefaultPageSettings.PaperSize
                            PrintWidth = .Width - RMargin - LMargin
                            PrintHeight = .Height - TMargin - BMargin
                            PageWidth = .Width - RMargin
                            PageHeight = .Height - BMargin
                        End With
                    End If
                End If
            End If

            If PpSzSTS = False Then

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

            End If
        End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If prn_Status = 1 Then
            Printing_Format1(e)
        Else
            ' Printing_Format3(e)
            Printing_Format3_ChequePrint(e)
        End If
    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        '----------Cheque Printing--------------

        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Double = 0
        Dim CurY As Double = 0
        Dim CurZ As Double = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PrnHeading As String = ""
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim prn_CheqDet As New DataTable
        Dim W As Single
        Dim dtWdth As Single
        Dim Amt As String
        Dim Rup1 As String, Rup2 As String
        Dim m As Integer
        Dim PrtyNm1 As String, PrtyNm2 As String

        PrintDocument1.DefaultPageSettings.Landscape = False

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 40
            .Right = 45
            .Top = 50 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        Da1 = New SqlClient.SqlDataAdapter("Select *  from Cheque_Print_Positioning_Head where Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Creditor_Idno").ToString)) & "order by Cheque_Print_Positioning_No", con)
        Da1.Fill(prn_CheqDet)

        LMargin = LMargin + Val(prn_CheqDet.Rows(0).Item("Left_Margin").ToString)
        TMargin = TMargin + Val(prn_CheqDet.Rows(0).Item("Top_Margin").ToString)

        If prn_CheqDet.Rows.Count > 0 Then
            If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) <> "" Then
                If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                    If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) = "LANDSCAPE" Then
                        PrintDocument2.DefaultPageSettings.Landscape = True
                        If PrintDocument2.DefaultPageSettings.Landscape = True Then
                            With PrintDocument2.DefaultPageSettings.PaperSize
                                PrintWidth = .Height - TMargin - BMargin
                                PrintHeight = .Width - RMargin - LMargin
                                PageWidth = .Height - TMargin
                                PageHeight = .Width - RMargin
                            End With
                        End If
                    Else
                        With PrintDocument2.DefaultPageSettings.PaperSize
                            PrintWidth = .Width - RMargin - LMargin
                            PrintHeight = .Height - TMargin - BMargin
                            PageWidth = .Width - RMargin
                            PageHeight = .Height - BMargin
                        End With
                    End If
                End If
            End If

            If PpSzSTS = False Then

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            e.PageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

            End If
        End If




        'With PrintDocument2.DefaultPageSettings.PaperSize
        '    PrintWidth = .Width - RMargin - LMargin
        '    PrintHeight = .Height - TMargin - BMargin
        '    PageWidth = .Width - RMargin
        '    PageHeight = .Height - BMargin
        'End With


        pFont = New Font("Calibri", 11, FontStyle.Regular)
        TxtHgt = 18.5


        If Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Top").ToString) <> 0 Then
            If Not Common_Procedures.VoucherType = "Cntr" Then
                CurX = Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Left").ToString) / 2.54 * 100
                CurY = Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Top").ToString) / 2.54 * 100
                Common_Procedures.Print_To_PrintDocument(e, "A/C PAYEE", LMargin + CurX, TMargin + CurY, 0, 0, pFont)
            End If
        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("Date_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("Date_Top").ToString) / 2.54 * 100
        dtWdth = Val(prn_CheqDet.Rows(0).Item("Date_Width").ToString)

        If dtWdth > 0 Then
            W = CurX
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 1, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 2, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            '  Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 3, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            '  W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 4, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 5, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            ' Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 6, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            ' W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 7, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 8, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 9, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), 10, 1)), LMargin + W, TMargin + CurY, 1, 0, pFont)
        Else

            If Val(prn_CheqDet.Rows(0).Item("Date_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Date_Top").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Format(prn_HdDt.Rows(prn_HeadIndx).Item("Voucher_Date"), "dd-MM-yyyy"), LMargin + CurX, TMargin + CurY, 1, 0, pFont)
            End If

        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("PartyName_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("PartyName_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("PartyName_Width").ToString)
        If CurX <> 0 And CurY <> 0 Then
            If Common_Procedures.VoucherType = "Cntr" Then
                Common_Procedures.Print_To_PrintDocument(e, "Self", LMargin + CurX, TMargin + CurY, 0, 0, pFont)
            Else
                PrtyNm2 = ""
                PrtyNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("by_name").ToString)
                If Len(PrtyNm1) > CurZ Then
                    For m = CurZ To 1 Step -1
                        If Mid$(Trim(PrtyNm1), m, 1) = " " Then Exit For
                    Next m
                    If m <> 0 Then
                        PrtyNm2 = Microsoft.VisualBasic.Right(Trim(PrtyNm1), Len(PrtyNm1) - m)
                        PrtyNm1 = Microsoft.VisualBasic.Left(Trim(PrtyNm1), m - 1)
                        m = 0
                    End If
                End If
                Common_Procedures.Print_To_PrintDocument(e, PrtyNm1, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
                Common_Procedures.Print_To_PrintDocument(e, PrtyNm2, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
            End If
        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("AmountWords_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("AmountWords_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("AmountWords_Width").ToString)

        If CurX <> 0 And CurY <> 0 And CurZ <> 0 Then
            Amt = Microsoft.VisualBasic.Left(Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString))), Len(Trim(Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString))))) - 3) & "/--"
            Rup2 = ""
            Rup1 = Common_Procedures.Rupees_Converstion(Math.Abs(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_VoucherAmount").ToString)))
            If Len(Rup1) > CurZ Then
                For m = CurZ To 1 Step -1
                    If Mid$(Trim(Rup1), m, 1) = " " Then Exit For
                Next m
                If m <> 0 Then
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - m)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), m - 1)
                    m = 0
                End If
            End If
            Common_Procedures.Print_To_PrintDocument(e, Rup1, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

            CurX = Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Left").ToString) / 2.54 * 100
            CurY = Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Top").ToString) / 2.54 * 100
            CurZ = Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Width").ToString)
            Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

            CurX = Val(prn_CheqDet.Rows(0).Item("Rupees_Left").ToString) / 2.54 * 100
            CurY = Val(prn_CheqDet.Rows(0).Item("Rupees_Top").ToString) / 2.54 * 100
            CurZ = Val(prn_CheqDet.Rows(0).Item("Rupees_Width").ToString)
            Common_Procedures.Print_To_PrintDocument(e, "**" & Amt, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("CompanyName_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("CompanyName_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("CompanyName_Width").ToString)
        If (CurX - LMargin) <> 0 And (CurY - TMargin) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

        CurX = Val(prn_CheqDet.Rows(0).Item("Partner_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("Partner_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("Partner_Width").ToString)
        If (CurX - LMargin) <> 0 And (CurY - TMargin) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Trim(prn_CheqDet.Rows(0).Item("Partner").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

        CurX = Val(prn_CheqDet.Rows(0).Item("AccountNo_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("AccountNo_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("AccountNo_Width").ToString)
        If (CurX - LMargin) <> 0 And (CurY - TMargin) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, "ACC NO. " & Trim(prn_CheqDet.Rows(0).Item("Account_No").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)


        e.HasMorePages = False
    End Sub

    Private Sub btn_PrintVoucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_PrintVoucher.Click
        prn_Status = 1
        Pnl_PrintRange.Visible = True
        Pnl_PrintRange.BringToFront()
        pnl_Back.Enabled = False
        txt_PrintFromNo.Text = lbl_VouNo.Text
        txt_PrintToNo.Text = lbl_VouNo.Text
        If txt_PrintFromNo.Enabled And txt_PrintFromNo.Visible Then
            txt_PrintFromNo.Focus()
            txt_PrintFromNo.SelectAll()
        End If

        btn_Close_Voucher_ChequePrint_Click(sender, e)

    End Sub

    Private Sub btn_PrintCheque_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_PrintCheque.Click
        prn_Status = 2
        printing_Cheque()
        btn_Close_Voucher_ChequePrint_Click(sender, e)
    End Sub

    Private Sub btn_Cancel_Voucher_ChequePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_Voucher_ChequePrint.Click
        btn_Close_Voucher_ChequePrint_Click(sender, e)
    End Sub

    Private Sub btn_Close_Voucher_ChequePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Voucher_ChequePrint.Click
        pnl_Back.Enabled = True
        pnl_Voucher_ChequePrint.Visible = False
    End Sub

    Private Sub btn_Print_Ordinary_Voucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Ordinary_Voucher.Click
        prn_Status = 1
        Printing_Voucher()
        btn_Close_Print_Voucher_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint_Voucher.Click
        prn_Status = 2
        Printing_Voucher()
        btn_Close_Print_Voucher_Click(sender, e)
    End Sub

    Private Sub btn_Cancel_Print_Voucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_Print_Voucher.Click
        btn_Close_Print_Voucher_Click(sender, e)
    End Sub

    Private Sub btn_Close_Print_Voucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print_Voucher.Click
        pnl_Back.Enabled = True
        pnl_Voucher_ChequePrint.Visible = False
        pnl_Print_Voucher.Visible = False
        Pnl_PrintRange.Visible = False
    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click

        'Dim Da1 As New SqlClient.SqlDataAdapter
        'Dim Dt1 As New DataTable
        'Dim i As Integer = 0
        'Dim smstxt As String = ""
        'Dim PhNo As String = "", AgPNo As String = ""
        'Dim Led_IdNo As Integer = 0, Agnt_IdNo As Integer = 0
        'Dim SMS_SenderID As String = ""
        'Dim SMS_Key As String = ""
        'Dim SMS_RouteID As String = ""
        'Dim SMS_Type As String = ""
        'Dim ParNm As String = ""
        'Dim Narr As String = ""
        'Dim vouamt As Double = 0
        'Dim PrnHeading As String = ""
        'Dim entcode As String = ""


        'Try

        '    entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouCode.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '    Led_IdNo = 0
        '    vouamt = 0
        '    Narr = ""
        '    ParNm = ""

        '    If Trim(LCase(lbl_VouType.Text)) = "purc" Or Trim(LCase(lbl_VouType.Text)) = "rcpt" Or Trim(LCase(lbl_VouType.Text)) = "csrp" Or Trim(LCase(lbl_VouType.Text)) = "crnt" Then
        '        Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_idno as to_idno, c.ledger_name as to_name, d.ledger_idno as by_idno, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4,b.*  from voucher_head a, ledger_head c, ledger_head d,Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Code = '" & Trim(entcode) & "' and a.creditor_idno = c.ledger_idno and a.debtor_idno = d.ledger_idno and a.Company_IdNo = b.Company_IdNo", con)
        '        Dt1 = New DataTable
        '        Da1.Fill(Dt1)

        '    Else

        '        Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_idno as to_idno, c.ledger_name as to_name, d.ledger_idno as by_idno, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4,b.* from voucher_head a, ledger_head c, ledger_head d,Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Code = '" & Trim(entcode) & "' and a.debtor_idno = c.ledger_idno and a.creditor_idno = d.ledger_idno and a.Company_IdNo = b.Company_IdNo", con)
        '        Dt1 = New DataTable
        '        Da1.Fill(Dt1)

        '    End If

        '    If Dt1.Rows.Count > 0 Then
        '        Led_IdNo = Val(Dt1.Rows(0).Item("to_idno").ToString)
        '        ParNm = Dt1.Rows(0).Item("to_name").ToString
        '        vouamt = Val(Dt1.Rows(0).Item("Total_VoucherAmount").ToString)
        '        Narr = Dt1.Rows(0).Item("Narration").ToString
        '    End If
        '    Dt1.Clear()

        '    PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")


        '    PrnHeading = ""
        '    Select Case Trim(LCase(lbl_VouType.Text))
        '        Case "purc"
        '            PrnHeading = "PURCHASE VOUCHER"
        '        Case "sale"
        '            PrnHeading = "SALES VOUCHER"
        '        Case "pymt", "cspy"
        '            PrnHeading = "Paid Amount to " '  "PAYMENT VOUCHER"
        '        Case "rcpt", "csrp"
        '            PrnHeading = "Received Amount from " ' "RECEIPT VOUCHER"
        '        Case "cntr"
        '            PrnHeading = "CONTRA VOUCHER"
        '        Case "jrnl"
        '            PrnHeading = "JOURNAL VOUCHER"
        '        Case "crnt"
        '            PrnHeading = "CREDIT NOTE VOUCHER"
        '        Case "dbnt"
        '            PrnHeading = "DEBIT NOTE VOUCHER"
        '        Case "ptcs"
        '            PrnHeading = "PETTI CASH VOUCHER"
        '    End Select

        '    smstxt = Trim(PrnHeading) & " " & Chr(13)

        '    smstxt = smstxt & Trim(ParNm) & " " & Chr(13)
        '    smstxt = smstxt & " Vou No : " & Trim(lbl_VouNo.Text) & " " & Chr(13)
        '    smstxt = smstxt & " Date : " & Trim(msk_Date.Text) & " " & Chr(13)
        '    smstxt = smstxt & " Voucher Amount : " & Trim(Common_Procedures.Currency_Format(Val(vouamt))) & " " & Chr(13)
        '    If Trim(Narr) <> "" Then
        '        smstxt = smstxt & " Narration : " & Trim(Narr) & " " & Chr(13)
        '    End If
        '    smstxt = smstxt & "  " & Chr(13)
        '    smstxt = smstxt & " Thanks! " & " " & Chr(13)
        '    smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

        '    SMS_SenderID = ""
        '    SMS_Key = ""
        '    SMS_RouteID = ""
        '    SMS_Type = ""

        '    Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

        '    Sms_Entry.vSmsPhoneNo = Trim(PhNo)
        '    Sms_Entry.vSmsMessage = Trim(smstxt)

        '    Sms_Entry.SMSProvider_SenderID = SMS_SenderID
        '    Sms_Entry.SMSProvider_Key = SMS_Key
        '    Sms_Entry.SMSProvider_RouteID = SMS_RouteID
        '    Sms_Entry.SMSProvider_Type = SMS_Type

        '    Dim f1 As New Sms_Entry
        '    f1.MdiParent = MDIParent1
        '    f1.Show()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Private Sub StatusBar_Purchase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Purchase.Click

        Common_Procedures.VoucherType = "Purc"
        Change_VoucherType()

    End Sub


    Private Sub StatusBar_Sales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Sales.Click
        Common_Procedures.VoucherType = "Sale"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_PaymentBank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_PaymentBank.Click
        Common_Procedures.VoucherType = "Pymt"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_ReceiptBank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_ReceiptBank.Click
        Common_Procedures.VoucherType = "Rcpt"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_PaymentCash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_PaymentCash.Click
        Common_Procedures.VoucherType = "CsPy"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_ReceiptCash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_ReceiptCash.Click
        Common_Procedures.VoucherType = "CsRp"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_Contra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Contra.Click
        Common_Procedures.VoucherType = "Cntr"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_Journal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Journal.Click
        Common_Procedures.VoucherType = "Jrnl"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_CreditNote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_CreditNote.Click
        Common_Procedures.VoucherType = "CrNt"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_DebitNote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_DebitNote.Click
        Common_Procedures.VoucherType = "DbNt"
        Change_VoucherType()
    End Sub

    Private Sub StatusBar_PettiCash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_PettiCash.Click
        Common_Procedures.VoucherType = "PtCs"
        Change_VoucherType()
    End Sub

    Private Sub Change_VoucherType()

        lbl_VouType.Text = Trim(Common_Procedures.VoucherType)

        Select Case Trim(LCase(lbl_VouType.Text))
            Case "purc"
                lbl_EntHeading.Text = "PURCHASE VOUCHER ENTRY"
            Case "sale"
                lbl_EntHeading.Text = "SALES VOUCHER ENTRY"
            Case "pymt"
                lbl_EntHeading.Text = "BANK PAYMENT VOUCHER ENTRY"
            Case "rcpt"
                lbl_EntHeading.Text = "BANK RECEIPT VOUCHER ENTRY"
            Case "cspy"
                lbl_EntHeading.Text = "CASH PAYMENT VOUCHER ENTRY"
            Case "csrp"
                lbl_EntHeading.Text = "CASH RECEIPT VOUCHER ENTRY"
            Case "cntr"
                lbl_EntHeading.Text = "CONTRA VOUCHER ENTRY"
            Case "jrnl"
                lbl_EntHeading.Text = "JOURNAL VOUCHER ENTRY"
            Case "crnt"
                lbl_EntHeading.Text = "CREDIT NOTE VOUCHER ENTRY"
            Case "dbnt"
                lbl_EntHeading.Text = "DEBIT NOTE VOUCHER ENTRY"
            Case "ptcs"
                lbl_EntHeading.Text = "PETTI CASH VOUCHER ENTRY"
        End Select

        Me.Text = ""

        btn_SMS.Visible = False
        If Trim(LCase(lbl_VouType.Text)) = "pymt" Or Trim(LCase(lbl_VouType.Text)) = "rcpt" Or Trim(LCase(lbl_VouType.Text)) = "cspy" Or Trim(LCase(lbl_VouType.Text)) = "csrp" Then
            btn_SMS.Visible = True
        End If

        new_record()

    End Sub

    Private Sub get_Ledger_CurrentBalance()
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim BalAmt As Double = 0
        Dim GpCd As String = ""
        Dim Datcondt As String = ""
        Dim n As Integer = 0
        Dim I As Integer = 0
        Dim Led_ID As Integer = 0

        Try

            lbl_CurrentBalance.Text = "Current Balance :"

            '-----------BALANCE

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

            With dgv_Details
                If .Rows.Count > 0 Then

                    n = .CurrentRow.Index

                    Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(n).Cells(1).Value)

                    If Led_ID <> 0 Then
                        GpCd = Common_Procedures.get_FieldValue(con, "ledger_head", "parent_code", "(ledger_idno = " & Str(Val(Led_ID)) & ")")
                        If GpCd Like "*~18~*" Then Datcondt = " and a.Voucher_date >= @companyfromdate " Else Datcondt = ""

                        cmd.CommandText = "select sum(a.Voucher_amount) as BalAmount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " " & Datcondt

                        da = New SqlClient.SqlDataAdapter(cmd)
                        'da = New SqlClient.SqlDataAdapter("select sum(a.Voucher_amount) as BalAmount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " " & Datcondt, con)
                        dt1 = New DataTable
                        da.Fill(dt1)

                        BalAmt = 0
                        If dt1.Rows.Count > 0 Then
                            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                                BalAmt = Val(dt1.Rows(0).Item("BalAmount").ToString)
                            End If
                        End If
                        dt1.Clear()

                        dt1.Dispose()
                        da.Dispose()
                        cmd.Dispose()

                        If Trim(UCase(.Rows(n).Cells(0).Value)) = "DR" Then BalAmt = BalAmt - Val(.Rows(n).Cells(2).Value)
                        If Trim(UCase(.Rows(n).Cells(0).Value)) = "CR" Then BalAmt = BalAmt + Val(.Rows(n).Cells(3).Value)

                        For I = 0 To UBound(VouAmtAr)
                            If Val(Led_ID) = Val(VouAmtAr(I).LedgerIdNo) Then BalAmt = BalAmt - Val(VouAmtAr(I).VoucherAmount)
                        Next I

                        lbl_CurrentBalance.Tag = n
                        lbl_CurrentBalance.Text = "Current Balance : " & Trim(Common_Procedures.Currency_Format(Math.Abs(Val(BalAmt)))) & IIf(Val(BalAmt) >= 0, " Cr", " Dr")
                        pnl_CurrentBalance.Visible = True

                    Else
                        lbl_CurrentBalance.Tag = -100
                        lbl_CurrentBalance.Text = "Current Balance : "
                        pnl_CurrentBalance.Visible = False

                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE GETTI CURRENT BALANCE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_OrderList_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OrderList.CellEndEdit
        Try
            With dgv_OrderList
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        dgv_OrderList_CellLeave(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL END EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_OrderList_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OrderList.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim rect As Rectangle

        With dgv_OrderList

            ActiveControl.Name = .Name


            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = 1 Then

                If cbo_Grid_OrderNo.Visible = False Or Val(cbo_Grid_OrderNo.Tag) <> e.RowIndex Then

                    cbo_Grid_OrderNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select a.Sales_Order_Selection_Code from Sales_Order_Head a order by a.Sales_Order_Selection_Code", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_OrderNo.DataSource = Dt1
                    cbo_Grid_OrderNo.DisplayMember = "Sales_Order_Selection_Code"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_OrderNo.Left = .Left + rect.Left
                    cbo_Grid_OrderNo.Top = .Top + rect.Top

                    cbo_Grid_OrderNo.Width = rect.Width
                    cbo_Grid_OrderNo.Height = rect.Height
                    cbo_Grid_OrderNo.Text = .CurrentCell.Value

                    cbo_Grid_OrderNo.Tag = Val(e.RowIndex)
                    cbo_Grid_OrderNo.Visible = True

                    cbo_Grid_OrderNo.BringToFront()
                    cbo_Grid_OrderNo.Focus()

                End If

            Else
                cbo_Grid_OrderNo.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_OrderList_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OrderList.CellLeave
        Try
            With dgv_OrderList

                If .CurrentCell.ColumnIndex = 1 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Trim(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub dgv_OrderList_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_OrderList.EditingControlShowing
        dgtxt_OrderList = CType(dgv_OrderList.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_OrderList_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_OrderList.GotFocus
        dgtxt_OrderList.ForeColor = Color.Black
        dgtxt_OrderList.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_OrderList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_OrderList.KeyPress

        On Error Resume Next

        'With dgv_OrderList

        '    If .Visible Then

        '        If Asc(e.KeyChar) = 13 Then
        '            If .CurrentCell.ColumnIndex = 1 Then

        '                If .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex).Value <> "" Then
        '                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
        '                Else

        '                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        '                    pnl_OrderList.Visible = False
        '                    pnl_Back.Enabled = True
        '                End If

        '            End If

        '        End If

        '    End If

        'End With

    End Sub

    Private Sub dgtxt_OrderList_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_OrderList.TextChanged
        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)

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

    Private Sub cbo_Grid_OrderNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_OrderNo.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Order_Selection_Code_Head", "Order_Selection_Code", "", "")

    End Sub

    Private Sub cbo_Grid_OrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_OrderNo.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_OrderNo, Nothing, Nothing, "Order_Selection_Code_Head", "Order_Selection_Code", "", "")

            With dgv_OrderList
                If (e.KeyValue = 38 And cbo_Grid_OrderNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)
                    .CurrentCell.Selected = True
                End If
                If (e.KeyValue = 40 And cbo_Grid_OrderNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    .CurrentCell.Selected = True
                End If

            End With


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Grid_OrderNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_OrderNo.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_OrderNo, Nothing, "Order_Selection_Code_Head", "Order_Selection_Code", "", "")

            If Asc(e.KeyChar) = 13 Then
                If cbo_Grid_OrderNo.Text <> "" Then

                    '     dgv_OrderList.Rows.Add()
                    dgv_OrderList.CurrentCell = dgv_OrderList.Rows(dgv_OrderList.CurrentRow.Index).Cells(2)

                Else

                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                    pnl_OrderList.Visible = False
                    pnl_Back.Enabled = True

                End If

            End If
        Catch ex As Exception

            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btn_CloseOrderList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOrderList.Click
        pnl_Back.Enabled = True
        pnl_OrderList.Visible = False
    End Sub

    Private Sub cbo_Grid_OrderNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_OrderNo.TextChanged
        Try
            With dgv_OrderList
                If Trim(cbo_Grid_OrderNo.Text) <> "" And Trim(cbo_Grid_OrderNo.Text) <> "System.Data.DataRowView" Then
                    .Rows(.CurrentCell.RowIndex).Cells(1).Value = Trim(cbo_Grid_OrderNo.Text)
                End If
            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgv_OrderList_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_OrderList.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_OrderList
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub cbo_Grid_Ledger_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Ledger.SelectedIndexChanged

    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub
End Class
