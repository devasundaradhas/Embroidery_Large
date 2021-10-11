Imports System.IO
Public Class Tally_Export

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False

    Private Sub clear()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0

        cbo_ExportFormat.Text = "TALLY7.2 Or BELOW"

        dtp_FromDate.Text = Common_Procedures.Company_FromDate
        dtp_ToDate.Text = Common_Procedures.Company_ToDate

        With dgv_Statistics_Details

            .Rows.Clear()

            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "PURCHASE"
            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "SALES"
            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "RECEIPT"
            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "PAYMENT"
            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "CONTRA"
            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "JOURNAL"
            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "CREDIT NOTE"
            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "DEBIT NOTE"

        End With

        With dgv_Statistics_Total
            .Rows.Clear()
            n = .Rows.Add()
            .Rows(n).Cells(0).Value = "TOTAL"
        End With

        chklst_Ledgers.Items.Clear()
        Da1 = New SqlClient.SqlDataAdapter("select * from ledger_head where ledger_name <> '' order by ledger_name", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                chklst_Ledgers.Items.Add(Dt1.Rows(i).Item("ledger_name").ToString, CheckState.Checked)
            Next
        End If
        Dt1.Clear()

    End Sub

    Private Sub Tally_Export_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

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

            End If

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Tally_Export_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MainPath As String = ""

        Me.Text = ""

        con.Open()

        cbo_ExportFormat.Items.Clear()
        cbo_ExportFormat.Items.Add("")
        cbo_ExportFormat.Items.Add("TALLY7.2 Or BELOW")
        cbo_ExportFormat.Items.Add("TALLY9 Or ABOVE")

        MainPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
        txt_Path.Text = Microsoft.VisualBasic.Left(MainPath, 2)

        clear()
        FrmLdSTS = True

    End Sub

    Private Sub Tally_Export_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Tally_Export_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                Close_Form()

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

                clear()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '-----
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '-----
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '-----
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '-----
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '-----
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '-----
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '-----
    End Sub

    Private Sub cbo_ExportFormat_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ExportFormat.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ExportFormat, Nothing, msk_ToDate, "", "", "", "")
    End Sub

    Private Sub cbo_ExportFormat_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ExportFormat.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ExportFormat, msk_ToDate, "", "", "", "")
    End Sub

    Private Sub dtp_FromDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_FromDate.ValueChanged
        msk_FromDate.Text = dtp_FromDate.Text
    End Sub

    Private Sub dtp_FromDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_FromDate.Enter
        msk_FromDate.Focus()
        msk_FromDate.SelectionStart = 0
    End Sub

    Private Sub dtp_ToDate_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ToDate.ValueChanged
        msk_ToDate.Text = dtp_ToDate.Text
    End Sub

    Private Sub dtp_ToDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ToDate.Enter
        msk_ToDate.Focus()
        msk_ToDate.SelectionStart = 0
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_ExportTally_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ExportTally.Click
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_ExportFormat.Text) = "" Then
            MessageBox.Show("Invalid Tally Export Format", "DOES NOT EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(msk_FromDate.Text) = "" Then
            MessageBox.Show("Invalid From Date", "DOES NOT EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_FromDate.Enabled And msk_FromDate.Visible Then msk_FromDate.Focus()
            Exit Sub
        End If

        If IsDate(msk_FromDate.Text) = False Then
            MessageBox.Show("Invalid From Date", "DOES NOT EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_FromDate.Enabled And msk_FromDate.Visible Then msk_FromDate.Focus()
            Exit Sub
        End If

        If Trim(msk_ToDate.Text) = "" Then
            MessageBox.Show("Invalid To Date", "DOES NOT EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_ToDate.Enabled And msk_ToDate.Visible Then msk_ToDate.Focus()
            Exit Sub
        End If

        If IsDate(msk_ToDate.Text) = False Then
            MessageBox.Show("Invalid To Date", "DOES NOT EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_ToDate.Enabled And msk_ToDate.Visible Then msk_ToDate.Focus()
            Exit Sub
        End If

        If Trim(txt_Path.Text) = "" Then
            MessageBox.Show("Invalid Export Path", "DOES NOT EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Path.Enabled And txt_Path.Visible Then txt_Path.Focus()
            Exit Sub
        End If

        If Directory.Exists(Trim(txt_Path.Text)) = False Then
            MessageBox.Show("Invalid Path - Does not exists", "DOES NOT EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Path.Enabled And txt_Path.Visible Then txt_Path.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_ExportFormat.Text)) = "TALLY7.2 OR BELOW" Then
            Call TallyExport_Ver7_Below()
        Else
            Call TallyExport_Ver9_Above()
        End If

    End Sub

    Private Sub TallyExport_Ver9_Above()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim Fs As FileStream
        Dim Wr As StreamWriter
        Dim MainPath As String = ""
        Dim MasFileNm As String = "", VouFileNm As String = ""
        Dim Indx As Integer = 0
        Dim LedID As Integer = 0
        Dim vTypAr(20, 4) As String
        Dim Grp_Name As String, Pnt_Name As String, Led_Name As String, Narr As String
        Dim inc_Single As Long, Inc_All As Long
        Dim I As Integer, J As Integer, K As Integer
        Dim Rf_Code As String = "", P_Idno As String = "", Led_Cond As String = "", TinNo As String = "", Reg_Type As String = "", GSTinNo As String = ""
        Dim Opn_Bal As Double = 0

        Try

            MDIParent1.Cursor = Cursors.WaitCursor
            Me.Cursor = Cursors.WaitCursor

            Cmd.Connection = con

            Cmd.Parameters.Clear()
            Cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(msk_FromDate.Text))
            Cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(msk_ToDate.Text))

            For J = 0 To dgv_Statistics_Details.Rows.Count - 1
                dgv_Statistics_Details.Rows(J).Cells(1).Value = ""
            Next J
            dgv_Statistics_Total.Rows(0).Cells(1).Value = ""

            MainPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows)

            MainPath = Microsoft.VisualBasic.Left(MainPath, 2)

            '-------------------------------------------------------------
            '-----------------------      Masters Posting
            '-------------------------------------------------------------

            '  If chk_AllLedger.Value = 1 Then

            MasFileNm = Trim(txt_Path.Text) & "\master.xml"
            'MasFileNm = Trim(MainPath) & "\master.xml"

            Fs = New FileStream(MasFileNm, FileMode.Create)
            Wr = New StreamWriter(Fs)

            Wr.WriteLine("<ENVELOPE>")
            Wr.WriteLine("<HEADER>")
            Wr.WriteLine("<TALLYREQUEST>Import Data</TALLYREQUEST>")
            Wr.WriteLine("</HEADER>")
            Wr.WriteLine("<BODY>")
            Wr.WriteLine("<IMPORTDATA>")
            Wr.WriteLine("<REQUESTDATA>")

            Da1 = New SqlClient.SqlDataAdapter("select * from AccountsGroup_Head where AccountsGroup_IdNo > 30 order by AccountsGroup_Name", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1

                    Grp_Name = StrConv(Dt1.Rows(I).Item("AccountsGroup_Name").ToString, vbProperCase)
                    Grp_Name = Replace(Grp_Name, "&", "&amp;")

                    Pnt_Name = StrConv(Dt1.Rows(I).Item("Parent_Name").ToString, vbProperCase)
                    Pnt_Name = Replace(Pnt_Name, "&", "&amp;")

                    If Trim(LCase(Pnt_Name)) = "branch / division" Then Pnt_Name = "Branch / Divisions"
                    If Trim(LCase(Pnt_Name)) = "purchase account" Then Pnt_Name = "Purchase Accounts"
                    If Trim(LCase(Pnt_Name)) = "sales account" Then Pnt_Name = "Sales Accounts"
                    If Trim(LCase(Pnt_Name)) = "suspense account" Then Pnt_Name = "Suspense A/c"
                    If Trim(LCase(Pnt_Name)) = "income (revenue)" Then Pnt_Name = "Direct Incomes"
                    If Trim(LCase(Pnt_Name)) = "revenue accounts" Then Pnt_Name = "Direct Incomes"

                    Wr.WriteLine("<TALLYMESSAGE xmlns:UDF=" & Chr(34) & "TallyUDF" & Chr(34) & ">")
                    Wr.WriteLine("<GROUP NAME=" & Chr(34) & Grp_Name & Chr(34) & " RESERVEDNAME=" & Chr(34) & Chr(34) & ">")
                    Wr.WriteLine("<NAME.LIST>")
                    Wr.WriteLine("<NAME>" & Grp_Name & "</NAME>")
                    Wr.WriteLine("</NAME.LIST>")
                    Wr.WriteLine("<PARENT>" & Pnt_Name & "</PARENT>")
                    Wr.WriteLine("</GROUP>")
                    Wr.WriteLine("</TALLYMESSAGE>")

                Next
            End If
            Dt1.Clear()

            Cmd.CommandText = "truncate table EntryTempSub"
            Cmd.ExecuteNonQuery()

            If opt_WithOpeningBalance.Checked = True Then
                Cmd.CommandText = "Insert into EntryTempSub ( Int1, Currency1 ) select b.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b where a.company_idno = " & Str(Val(Val(lbl_Company.Tag))) & " and b.parent_code NOT LIKE '%~18~%' and a.voucher_date < @FromDate and a.ledger_idno = b.ledger_idno group by b.ledger_idno"
                Cmd.ExecuteNonQuery()
            End If

            Da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Currency1 from ledger_head a INNER JOIN AccountsGroup_Head b ON a.AccountsGroup_IdNo = b.AccountsGroup_IdNo LEFT OUTER JOIN EntryTempSub c ON a.ledger_idno = c.int1 Where a.Ledger_IdNo <> 0 and a.Ledger_IdNo <> 13 order by a.ledger_name", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1

                    Opn_Bal = 0
                    If Dt1.Rows(I).Item("Currency1").ToString <> "" Then Opn_Bal = Val(Dt1.Rows(I).Item("Currency1").ToString)

                    Led_Name = StrConv(Dt1.Rows(I).Item("ledger_name").ToString, vbProperCase)
                    Led_Name = Replace(Led_Name, "&", "&amp;")

                    Grp_Name = StrConv(Dt1.Rows(I).Item("AccountsGroup_Name").ToString, vbProperCase)
                    Grp_Name = Replace(Grp_Name, "&", "&amp;")

                    ' TinNo = StrConv(Dt1.Rows(I).Item("Ledger_TinNo").ToString, vbProperCase)
                    ' TinNo = Replace(TinNo, "&", "&amp;")

                    'If Trim(Dt1.Rows(I).Item("Ledger_GSTinNo").ToString) <> "" Then
                    '    Reg_Type = StrConv("REGULAR", vbProperCase)
                    '    Reg_Type = Replace(Reg_Type, "&", "&amp;")
                    'Else
                    '    Reg_Type = StrConv("UNREGISTERED", vbProperCase)
                    '    Reg_Type = Replace(Reg_Type, "&", "&amp;")
                    'End If

                    'GSTinNo = StrConv(Dt1.Rows(I).Item("Ledger_GSTinNo").ToString, vbProperCase)
                    'GSTinNo = Replace(GSTinNo, "&", "&amp;")

                    If Trim(LCase(Grp_Name)) = "branch / division" Then Grp_Name = "Branch / Divisions"
                    If Trim(LCase(Grp_Name)) = "purchase account" Then Grp_Name = "Purchase Accounts"
                    If Trim(LCase(Grp_Name)) = "sales account" Then Grp_Name = "Sales Accounts"
                    If Trim(LCase(Grp_Name)) = "suspense account" Then Grp_Name = "Suspense A/c"
                    If Trim(LCase(Grp_Name)) = "income (revenue)" Then Grp_Name = "Direct Incomes"
                    If Trim(LCase(Grp_Name)) = "revenue accounts" Then Grp_Name = "Direct Incomes"
                    If Trim(LCase(Grp_Name)) = "revenue accounts" Then Grp_Name = "Direct Incomes"

                    Wr.WriteLine("<TALLYMESSAGE xmlns:UDF=" & Chr(34) & "TallyUDF" & Chr(34) & ">")
                    Wr.WriteLine("<LEDGER NAME=" & Chr(34) & Led_Name & Chr(34) & " RESERVEDNAME=" & Chr(34) & Chr(34) & ">")

                    'Wr.WriteLine("<INCOMETAXNUMBER >" & TinNo & "</INCOMETAXNUMBER>")

                    '' Wr.WriteLine("<INCOMETAXNUMBER >" & TinNo & "</INCOMETAXNUMBER>")

                    'Wr.WriteLine("<GSTREGISTRATIONTYPE >" & Reg_Type & "</GSTREGISTRATIONTYPE>")

                    '    Wr.WriteLine("<PARTYGSTIN >" & GSTinNo & "</PARTYGSTIN>")

                    ' GSTREGISTRATIONTYPE = REGULAR  , UNREGISTERED
                    ' PARTYGSTIN = 

                    Wr.WriteLine("<NAME.LIST>")
                    Wr.WriteLine("<NAME>" & Led_Name & "</NAME>")
                    Wr.WriteLine("</NAME.LIST>")
                    Wr.WriteLine("<PARENT>" & Grp_Name & "</PARENT>")
                    If opt_WithOpeningBalance.Checked = True Then
                        Wr.WriteLine("<OPENINGBALANCE>" & Opn_Bal & "</OPENINGBALANCE>")
                    End If
                    Wr.WriteLine("</LEDGER>")
                    Wr.WriteLine("</TALLYMESSAGE>")

                Next
            End If
            Dt1.Clear()

            Wr.Close()
            Fs.Close()
            Wr.Dispose()
            Fs.Dispose()

            'End If

            '-------------------------------------------------------------
            '-----------------------      Voucher Posting
            '-------------------------------------------------------------

            Indx = 0

            If chk_Purchase.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Purc"
                vTypAr(Indx, 1) = "(b.voucher_type='Purc' or b.voucher_type='Yarn.Purc' or b.voucher_type='Clo.Purc' or b.voucher_type='Purch'  or b.voucher_type='SPPurchase' or b.voucher_type='Gst.Purc' or b.voucher_type ='Gen.Gst.Purc')"
                vTypAr(Indx, 2) = 0
                vTypAr(Indx, 3) = "Purchase"
            End If
            If chk_Sales.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Sale"
                vTypAr(Indx, 1) = "(b.voucher_type='Sale' or b.voucher_type='Clo.Sale' or b.voucher_type='Yarn.Sales' or b.voucher_type='Sales' or b.voucher_type='SalesDsn' or b.voucher_type='SaleSV' or b.voucher_type='SPInvoice' or b.voucher_type='SPWstInv' or b.voucher_type = 'Sales.Ent'  or b.voucher_type = 'V.Sales' or b.voucher_type = 'Gst.Sales' or b.voucher_type = 'GST.Sales' or b.voucher_type = 'GST-Sales' or b.voucher_type = 'Gst.Invoice' or b.voucher_type = 'L.Invoice' or b.voucher_type = 'Gst.InvService' or b.voucher_type LIKE 'InvGar%' or b.voucher_type = 'Gen.Gst.Sale')"
                vTypAr(Indx, 2) = 1
                vTypAr(Indx, 3) = "Sales"
            End If
            If chk_Receipt.Checked = True Or chk_CashReceipt.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Rcpt"
                If chk_Receipt.Checked = True Then vTypAr(Indx, 1) = "b.voucher_type='Rcpt'"
                If chk_CashReceipt.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='CsRp'"
                If chk_Receipt.Checked = True Or chk_CashReceipt.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='Amt.Rcpt'"
                vTypAr(Indx, 2) = 2
                vTypAr(Indx, 3) = "Receipt"
            End If
            If chk_Payment.Checked = True Or chk_CashPayment.Checked = True Or chk_PettiCash.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Pymt"
                If chk_Payment.Checked = True Then vTypAr(Indx, 1) = "b.voucher_type='Pymt'"
                If chk_CashPayment.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='CsPy' "
                If chk_Payment.Checked = True Or chk_CashPayment.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='Wea.Pymt'"
                If chk_PettiCash.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='PtCs'"
                vTypAr(Indx, 2) = 3
                vTypAr(Indx, 3) = "Payment"
            End If
            If chk_Contra.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Ctra"
                vTypAr(Indx, 1) = "(b.voucher_type='Cntr')"
                vTypAr(Indx, 2) = 4
                vTypAr(Indx, 3) = "Contra"
            End If
            If chk_Journal.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Jrnl"
                vTypAr(Indx, 1) = "(b.voucher_type='Jrnl' or b.voucher_type='Wea.Wages')"
                vTypAr(Indx, 2) = 5
                vTypAr(Indx, 3) = "Journal"
            End If
            If chk_CreditNote.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "C/N "
                vTypAr(Indx, 1) = "(b.voucher_type='CrNt' or b.voucher_type='Sales.Ret' or b.voucher_type='GstSales.Ret' or b.voucher_type='Gst.CrNt')"
                vTypAr(Indx, 2) = 6
                vTypAr(Indx, 3) = "Credit Note"
            End If
            If chk_DebitNote.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "D/N "
                vTypAr(Indx, 1) = "(b.voucher_type='DbNt' or b.voucher_type='PurRt' or b.voucher_type='Gst.Purc.Ret'  or b.voucher_type = 'Gst.DbNt')"
                vTypAr(Indx, 2) = 7
                vTypAr(Indx, 3) = "Debit Note"
            End If
            Led_Cond = ""

            If opt_SelectedLedgers.Checked = True Then

                P_Idno = ""
                For Each itemChecked In chklst_Ledgers.CheckedItems

                    LedID = Common_Procedures.Ledger_NameToIdNo(con, itemChecked)

                    P_Idno = P_Idno & IIf(Trim(P_Idno) <> "", ",", "") & Trim(Val(LedID))

                Next

                Led_Cond = " and ( b.voucher_code in ( select distinct(z.voucher_code) from voucher_details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.ledger_idno IN ( " & P_Idno & " ) ) )"

            End If

            VouFileNm = Trim(txt_Path.Text) & "\voucher.xml"
            'VouFileNm = Trim(MainPath) & "\voucher.xml"
            Fs = New FileStream(VouFileNm, FileMode.Create)
            Wr = New StreamWriter(Fs)

            Wr.WriteLine("<ENVELOPE>")
            Wr.WriteLine("<HEADER>")
            Wr.WriteLine("<TALLYREQUEST>Import Data</TALLYREQUEST>")
            Wr.WriteLine("</HEADER>")
            Wr.WriteLine("<BODY>")
            Wr.WriteLine("<IMPORTDATA>")
            Wr.WriteLine("<REQUESTDATA>")

            Inc_All = 0
            If Month(Convert.ToDateTime(msk_FromDate.Text)) = 5 Then
                Inc_All = 25000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 6 Then
                Inc_All = 50000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 7 Then
                Inc_All = 75000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 8 Then
                Inc_All = 100000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 9 Then
                Inc_All = 125000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 10 Then
                Inc_All = 150000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 11 Then
                Inc_All = 175000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 12 Then
                Inc_All = 200000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 1 Then
                Inc_All = 225000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 2 Then
                Inc_All = 250000
            ElseIf Month(Convert.ToDateTime(msk_FromDate.Text)) = 3 Then
                Inc_All = 275000
            End If

            Rf_Code = ""

            For J = 1 To Indx

                inc_Single = 0

                Cmd.CommandText = "select b.Voucher_Code, b.voucher_no, b.voucher_date, b.voucher_type, a.voucher_amount, a.narration, (case when c.ledger_name='Cash A/c' then 'Cash' else c.ledger_name end ) as party_name from voucher_details a, voucher_head b, ledger_head c where a.voucher_amount <> 0 and (" & vTypAr(J, 1) & ") " & Led_Cond & " and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.voucher_date between @FromDate and @ToDate and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno and a.ledger_idno = c.ledger_idno order by b.voucher_date, b.for_orderby, b.Voucher_Code, a.sl_no"
                Da1 = New SqlClient.SqlDataAdapter(Cmd)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then

                    For K = 0 To Dt1.Rows.Count - 1

                        Led_Name = StrConv(Dt1.Rows(K).Item("party_name").ToString, vbProperCase)
                        Led_Name = Replace(Led_Name, "&", "&amp;")

                        If Trim(UCase(Rf_Code)) <> Trim(UCase(Dt1.Rows(K).Item("Voucher_Code").ToString)) Then

                            If Trim(Rf_Code) <> "" Then
                                Wr.WriteLine("</VOUCHER>")
                                Wr.WriteLine("</TALLYMESSAGE>")
                            End If

                            inc_Single = inc_Single + 1

                            Wr.WriteLine("<TALLYMESSAGE xmlns:UDF=" & Chr(34) & "TallyUDF" & Chr(34) & ">")
                            Wr.WriteLine("<VOUCHER REMOTEID=" & Chr(34) & "Bill-cgidno-compidno-dc505166-9494-4e1d-940d-935a67429320-" & Trim(Format(Inc_All + inc_Single, "00000000")) & Chr(34) & " VCHTYPE=" & Chr(34) & vTypAr(J, 3) & Chr(34) & " ACTION=" & Chr(34) & "Create" & Chr(34) & ">")
                            Wr.WriteLine("<VOUCHERTYPENAME>" & vTypAr(J, 3) & "</VOUCHERTYPENAME>")
                            Wr.WriteLine("<DATE>" & Format(Dt1.Rows(K).Item("Voucher_Date"), "yyyyMMdd") & "</DATE>")
                            Wr.WriteLine("<EFFECTIVEDATE>" & Format(Dt1.Rows(K).Item("Voucher_Date"), "yyyyMMdd") & "</EFFECTIVEDATE>")
                            Wr.WriteLine("<PARTYNAME>" & Led_Name & "</PARTYNAME>")
                            Wr.WriteLine("<PARTYLEDGERNAME>" & Led_Name & "</PARTYLEDGERNAME>")

                            Narr = StrConv(Dt1.Rows(K).Item("Narration").ToString, vbProperCase)
                            Narr = Replace(Narr, "&", "&amp;")

                            Wr.WriteLine("<NARRATION>" & Narr & "</NARRATION>")
                            Wr.WriteLine("<GUID>dc505166-9494-4e1d-940d-935a67429320-" & Trim(Format(Inc_All + inc_Single, "00000000")) & "</GUID>")

                        End If

                        Wr.WriteLine("<ALLLEDGERENTRIES.LIST>")
                        Wr.WriteLine("<LEDGERNAME>" & Led_Name & "</LEDGERNAME>")
                        Wr.WriteLine("<GSTCLASS />")
                        If Val(Dt1.Rows(K).Item("voucher_amount").ToString) < 0 Then
                            Wr.WriteLine("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>")

                        Else
                            Wr.WriteLine("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>")

                        End If

                        Wr.WriteLine("<ISPARTYLEDGER>Yes</ISPARTYLEDGER>")
                        Wr.WriteLine("<AMOUNT>" & Trim(Format(Val(Dt1.Rows(K).Item("voucher_amount").ToString), "###########0.00")) & "</AMOUNT>")
                        Wr.WriteLine("</ALLLEDGERENTRIES.LIST>")

                        Rf_Code = Dt1.Rows(K).Item("Voucher_Code").ToString

                    Next K

                End If
                Dt1.Clear()

                Inc_All = Inc_All + inc_Single
                dgv_Statistics_Details.Rows(Val(vTypAr(J, 2))).Cells(1).Value = inc_Single

            Next J

            Wr.WriteLine("</VOUCHER>")
            Wr.WriteLine("</TALLYMESSAGE>")
            Wr.WriteLine("</REQUESTDATA>")
            Wr.WriteLine("</IMPORTDATA>")
            Wr.WriteLine("</BODY>")
            Wr.WriteLine("</ENVELOPE>")

            Wr.Close()
            Fs.Close()
            Wr.Dispose()
            Fs.Dispose()

            dgv_Statistics_Total.Rows(0).Cells(1).Value = Val(dgv_Statistics_Details.Rows(0).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(1).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(2).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(3).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(4).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(5).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(6).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(7).Cells(1).Value)

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show("Exported Sucessfully", "FOR TALLY EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show(ex.Message, "INVALID TALLY EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub TallyExport_Ver9_Above_111()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim Fs As FileStream
        Dim Wr As StreamWriter
        Dim MainPath As String = ""
        Dim MasFileNm As String = "", VouFileNm As String = ""
        Dim Indx As Integer = 0
        Dim LedID As Integer = 0
        Dim vTypAr(20, 4) As String
        Dim Grp_Name As String, Pnt_Name As String, Led_Name As String, Narr As String
        Dim inc_Single As Long, Inc_All As Long
        Dim I As Integer, J As Integer, K As Integer
        Dim Rf_Code As String = "", P_Idno As String = "", Led_Cond As String = ""
        Dim Opn_Bal As Double = 0

        Try

            MDIParent1.Cursor = Cursors.WaitCursor
            Me.Cursor = Cursors.WaitCursor

            Cmd.Connection = con

            Cmd.Parameters.Clear()
            Cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(msk_FromDate.Text))
            Cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(msk_ToDate.Text))

            For J = 0 To dgv_Statistics_Details.Rows.Count - 1
                dgv_Statistics_Details.Rows(J).Cells(1).Value = ""
            Next J
            dgv_Statistics_Total.Rows(0).Cells(1).Value = ""

            MainPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows)

            MainPath = Microsoft.VisualBasic.Left(MainPath, 2)

            '-------------------------------------------------------------
            '-----------------------      Masters Posting
            '-------------------------------------------------------------

            'If chk_AllLedger.Value = 1 Then

            MasFileNm = Trim(txt_Path.Text) & "\master.xml"
            'MasFileNm = Trim(MainPath) & "\master.xml"

            Fs = New FileStream(MasFileNm, FileMode.Create)
            Wr = New StreamWriter(Fs)

            Wr.WriteLine("<ENVELOPE>")
            Wr.WriteLine("<HEADER>")
            Wr.WriteLine("<TALLYREQUEST>Import Data</TALLYREQUEST>")
            Wr.WriteLine("</HEADER>")
            Wr.WriteLine("<BODY>")
            Wr.WriteLine("<IMPORTDATA>")
            Wr.WriteLine("<REQUESTDATA>")

            Da1 = New SqlClient.SqlDataAdapter("select * from AccountsGroup_Head where AccountsGroup_IdNo > 30 order by AccountsGroup_Name", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1

                    Grp_Name = StrConv(Dt1.Rows(I).Item("AccountsGroup_Name").ToString, vbProperCase)
                    Grp_Name = Replace(Grp_Name, "&", "&amp;")

                    Pnt_Name = StrConv(Dt1.Rows(I).Item("Parent_Name").ToString, vbProperCase)
                    Pnt_Name = Replace(Pnt_Name, "&", "&amp;")

                    If Trim(LCase(Pnt_Name)) = "branch / division" Then Pnt_Name = "Branch / Divisions"
                    If Trim(LCase(Pnt_Name)) = "purchase account" Then Pnt_Name = "Purchase Accounts"
                    If Trim(LCase(Pnt_Name)) = "sales account" Then Pnt_Name = "Sales Accounts"
                    If Trim(LCase(Pnt_Name)) = "suspense account" Then Pnt_Name = "Suspense A/c"
                    If Trim(LCase(Pnt_Name)) = "income (revenue)" Then Pnt_Name = "Direct Incomes"
                    If Trim(LCase(Pnt_Name)) = "revenue accounts" Then Pnt_Name = "Direct Incomes"


                    Wr.WriteLine("<TALLYMESSAGE xmlns:UDF=" & Chr(34) & "TallyUDF" & Chr(34) & ">")
                    Wr.WriteLine("<GROUP NAME=" & Chr(34) & Grp_Name & Chr(34) & " RESERVEDNAME=" & Chr(34) & Chr(34) & ">")
                    Wr.WriteLine("<NAME.LIST>")
                    Wr.WriteLine("<NAME>" & Grp_Name & "</NAME>")
                    Wr.WriteLine("</NAME.LIST>")
                    Wr.WriteLine("<PARENT>" & Pnt_Name & "</PARENT>")
                    Wr.WriteLine("</GROUP>")
                    Wr.WriteLine("</TALLYMESSAGE>")

                Next
            End If
            Dt1.Clear()

            Cmd.CommandText = "truncate table EntryTempSub"
            Cmd.ExecuteNonQuery()

            'If opt_OpeningBalance(0).Value = True Then
            Cmd.CommandText = "Insert into EntryTempSub ( Int1, Currency1 ) select b.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b where a.company_idno = " & Str(Val(Val(lbl_Company.Tag))) & " and b.parent_code NOT LIKE '%~18~%' and a.voucher_date < @FromDate and a.ledger_idno = b.ledger_idno group by b.ledger_idno"
            Cmd.ExecuteNonQuery()
            'End If

            Da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Currency1 from ledger_head a INNER JOIN AccountsGroup_Head b ON a.parent_code = b.parent_idno LEFT OUTER JOIN EntryTempSub c ON a.ledger_idno = c.int1 Where a.Ledger_IdNo <> 0 and a.Ledger_IdNo <> 13 order by a.ledger_name", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1

                    Opn_Bal = 0
                    If Dt1.Rows(I).Item("Currency1").ToString <> "" Then Opn_Bal = Val(Dt1.Rows(I).Item("Currency1").ToString)

                    Led_Name = StrConv(Dt1.Rows(I).Item("ledger_name").ToString, vbProperCase)
                    Led_Name = Replace(Led_Name, "&", "&amp;")

                    Grp_Name = StrConv(Dt1.Rows(I).Item("AccountsGroup_Name").ToString, vbProperCase)
                    Grp_Name = Replace(Grp_Name, "&", "&amp;")

                    If Trim(LCase(Grp_Name)) = "branch / division" Then Grp_Name = "Branch / Divisions"
                    If Trim(LCase(Grp_Name)) = "purchase account" Then Grp_Name = "Purchase Accounts"
                    If Trim(LCase(Grp_Name)) = "sales account" Then Grp_Name = "Sales Accounts"
                    If Trim(LCase(Grp_Name)) = "suspense account" Then Grp_Name = "Suspense A/c"
                    If Trim(LCase(Grp_Name)) = "income (revenue)" Then Grp_Name = "Direct Incomes"
                    If Trim(LCase(Grp_Name)) = "revenue accounts" Then Grp_Name = "Direct Incomes"
                    If Trim(LCase(Grp_Name)) = "revenue accounts" Then Grp_Name = "Direct Incomes"

                    Wr.WriteLine("<TALLYMESSAGE xmlns:UDF=" & Chr(34) & "TallyUDF" & Chr(34) & ">")
                    Wr.WriteLine("<LEDGER NAME=" & Chr(34) & Led_Name & Chr(34) & " RESERVEDNAME=" & Chr(34) & Chr(34) & ">")
                    Wr.WriteLine("<NAME.LIST>")
                    Wr.WriteLine("<NAME>" & Led_Name & "</NAME>")
                    Wr.WriteLine("</NAME.LIST>")
                    Wr.WriteLine("<PARENT>" & Grp_Name & "</PARENT>")
                    Wr.WriteLine("<OPENINGBALANCE>" & Opn_Bal & "</OPENINGBALANCE>")
                    Wr.WriteLine("</LEDGER>")
                    Wr.WriteLine("</TALLYMESSAGE>")

                Next
            End If
            Dt1.Clear()

            Wr.Close()
            Fs.Close()
            Wr.Dispose()
            Fs.Dispose()

            'End If

            '-------------------------------------------------------------
            '-----------------------      Voucher Posting
            '-------------------------------------------------------------

            Indx = 0

            If chk_Purchase.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Purc"
                vTypAr(Indx, 1) = "(b.voucher_type='Purc' or b.voucher_type='Yarn.Purc' or b.voucher_type='Clo.Purc' or b.voucher_type='Purch'  or b.voucher_type='SPPurchase' or b.voucher_type='Gst.Purc')"
                vTypAr(Indx, 2) = 0
                vTypAr(Indx, 3) = "Purchase"
            End If
            If chk_Sales.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Sale"
                vTypAr(Indx, 1) = "(b.voucher_type='Sale' or b.voucher_type='Clo.Sale' or b.voucher_type='Yarn.Sales' or b.voucher_type='Sales' or b.voucher_type='SalesDsn' or b.voucher_type='SaleSV' or b.voucher_type='SPInvoice' or b.voucher_type='SPWstInv' or b.voucher_type = 'Sales.Ent' or b.voucher_type = 'Gst.Sales' or b.voucher_type = 'V.Sales' )"
                vTypAr(Indx, 2) = 1
                vTypAr(Indx, 3) = "Sales"
            End If
            If chk_Receipt.Checked = True Or chk_CashReceipt.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Rcpt"
                If chk_Receipt.Checked = True Then vTypAr(Indx, 1) = "b.voucher_type='Rcpt'"
                If chk_CashReceipt.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='CsRp'"
                If chk_Receipt.Checked = True Or chk_CashReceipt.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='Amt.Rcpt'"
                vTypAr(Indx, 2) = 2
                vTypAr(Indx, 3) = "Receipt"
            End If
            If chk_Payment.Checked = True Or chk_CashPayment.Checked = True Or chk_PettiCash.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Pymt"
                If chk_Payment.Checked = True Then vTypAr(Indx, 1) = "b.voucher_type='Pymt'"
                If chk_CashPayment.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='CsPy' "
                If chk_Payment.Checked = True Or chk_CashPayment.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='Wea.Pymt'"
                If chk_PettiCash.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='PtCs'"
                vTypAr(Indx, 2) = 3
                vTypAr(Indx, 3) = "Payment"
            End If
            If chk_Contra.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Ctra"
                vTypAr(Indx, 1) = "(b.voucher_type='Cntr')"
                vTypAr(Indx, 2) = 4
                vTypAr(Indx, 3) = "Contra"
            End If
            If chk_Journal.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Jrnl"
                vTypAr(Indx, 1) = "(b.voucher_type='Jrnl' or b.voucher_type='Wea.Wages')"
                vTypAr(Indx, 2) = 5
                vTypAr(Indx, 3) = "Journal"
            End If
            If chk_CreditNote.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "C/N "
                vTypAr(Indx, 1) = "(b.voucher_type='CrNt' or b.voucher_type='Sales.Ret')"
                vTypAr(Indx, 2) = 6
                vTypAr(Indx, 3) = "Credit Note"
            End If
            If chk_DebitNote.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "D/N "
                vTypAr(Indx, 1) = "b.voucher_type='DbNt'"
                vTypAr(Indx, 2) = 7
                vTypAr(Indx, 3) = "Debit Note"
            End If

            Led_Cond = ""
            If opt_SelectedLedgers.Checked = True Then

                P_Idno = ""
                For Each itemChecked In chklst_Ledgers.CheckedItems

                    LedID = Common_Procedures.Ledger_NameToIdNo(con, itemChecked)

                    P_Idno = P_Idno & IIf(Trim(P_Idno) <> "", ",", "") & Trim(Val(LedID))

                    'MessageBox.Show("Item with title: " + quote + itemChecked.ToString() + quote + ", is checked. Checked state is: " + CheckedListBox1.GetItemCheckState(CheckedListBox1.Items.IndexOf(itemChecked)).ToString() + ".")

                Next

                Led_Cond = " and ( b.voucher_code in ( select distinct(z.voucher_code) from voucher_details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.ledger_idno IN ( " & P_Idno & " ) ) )"

            End If

            VouFileNm = Trim(txt_Path.Text) & "\voucher.xml"
            'VouFileNm = Trim(MainPath) & "\voucher.xml"

            Fs = New FileStream(VouFileNm, FileMode.Create)
            Wr = New StreamWriter(Fs)

            Wr.WriteLine("<ENVELOPE>")
            Wr.WriteLine("<HEADER>")
            Wr.WriteLine("<TALLYREQUEST>Import Data</TALLYREQUEST>")
            Wr.WriteLine("</HEADER>")
            Wr.WriteLine("<BODY>")
            Wr.WriteLine("<IMPORTDATA>")
            Wr.WriteLine("<REQUESTDATA>")

            Inc_All = 0
            Rf_Code = ""
            For J = 1 To Indx

                inc_Single = 0

                Cmd.CommandText = "select b.Voucher_Code, b.voucher_no, b.voucher_date, b.voucher_type, a.voucher_amount, a.narration, (case when c.ledger_name='Cash A/c' then 'Cash' else c.ledger_name end ) as party_name from voucher_details a, voucher_head b, ledger_head c where a.voucher_amount <> 0 and (" & vTypAr(J, 1) & ") " & Led_Cond & " and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.voucher_date between @FromDate and @ToDate and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno and a.ledger_idno = c.ledger_idno order by b.voucher_date, b.for_orderby, b.Voucher_Code, a.sl_no"
                Da1 = New SqlClient.SqlDataAdapter(Cmd)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then

                    For K = 0 To Dt1.Rows.Count - 1

                        Led_Name = StrConv(Dt1.Rows(K).Item("party_name").ToString, vbProperCase)
                        Led_Name = Replace(Led_Name, "&", "&amp;")

                        If Trim(UCase(Rf_Code)) <> Trim(UCase(Dt1.Rows(K).Item("Voucher_Code").ToString)) Then
                            If Trim(Rf_Code) <> "" Then
                                Wr.WriteLine("</VOUCHER>")
                                Wr.WriteLine("</TALLYMESSAGE>")
                            End If

                            inc_Single = inc_Single + 1

                            Wr.WriteLine("<TALLYMESSAGE xmlns:UDF=" & Chr(34) & "TallyUDF" & Chr(34) & ">")
                            Wr.WriteLine("<VOUCHER REMOTEID=" & Chr(34) & "dc505166-9494-4e1d-940d-935a67429320-" & Trim(Format(Inc_All + inc_Single, "00000000")) & Chr(34) & " VCHTYPE=" & Chr(34) & vTypAr(J, 3) & Chr(34) & " ACTION=" & Chr(34) & "Create" & Chr(34) & ">")
                            Wr.WriteLine("<VOUCHERTYPENAME>" & vTypAr(J, 3) & "</VOUCHERTYPENAME>")
                            Wr.WriteLine("<DATE>" & Format(Dt1.Rows(K).Item("Voucher_Date"), "yyyyMMdd") & "</DATE>")
                            Wr.WriteLine("<EFFECTIVEDATE>" & Format(Dt1.Rows(K).Item("Voucher_Date"), "yyyyMMdd") & "</EFFECTIVEDATE>")
                            Wr.WriteLine("<PARTYNAME>" & Led_Name & "</PARTYNAME>")
                            Wr.WriteLine("<PARTYLEDGERNAME>" & Led_Name & "</PARTYLEDGERNAME>")

                            Narr = StrConv(Dt1.Rows(K).Item("Narration").ToString, vbProperCase)
                            Narr = Replace(Narr, "&", "&amp;")

                            Wr.WriteLine("<NARRATION>" & Narr & "</NARRATION>")
                            Wr.WriteLine("<GUID>dc505166-9494-4e1d-940d-935a67429320-" & Trim(Format(Inc_All + inc_Single, "00000000")) & "</GUID>")

                        End If


                        Wr.WriteLine("<ALLLEDGERENTRIES.LIST>")
                        Wr.WriteLine("<LEDGERNAME>" & Led_Name & "</LEDGERNAME>")
                        Wr.WriteLine("<AMOUNT>" & Trim(Format(Val(Dt1.Rows(K).Item("voucher_amount").ToString), "###########0.00")) & "</AMOUNT>")
                        Wr.WriteLine("</ALLLEDGERENTRIES.LIST>")

                        Rf_Code = Dt1.Rows(K).Item("Voucher_Code").ToString

                    Next K

                End If
                Dt1.Clear()

                Inc_All = Inc_All + inc_Single
                dgv_Statistics_Details.Rows(Val(vTypAr(J, 2))).Cells(1).Value = inc_Single

            Next J

            Wr.WriteLine("</VOUCHER>")
            Wr.WriteLine("</TALLYMESSAGE>")
            Wr.WriteLine("</REQUESTDATA>")
            Wr.WriteLine("</IMPORTDATA>")
            Wr.WriteLine("</BODY>")
            Wr.WriteLine("</ENVELOPE>")

            Wr.Close()
            Fs.Close()
            Wr.Dispose()
            Fs.Dispose()

            dgv_Statistics_Total.Rows(0).Cells(1).Value = Val(dgv_Statistics_Details.Rows(0).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(1).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(2).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(3).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(4).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(5).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(6).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(7).Cells(1).Value)

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show("Exported Sucessfully", "FOR TALLY EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show(ex.Message, "INVALID TALLY EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub TallyExport_Ver7_Below()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim Fs As FileStream
        Dim Wr As StreamWriter
        Dim MainPath As String = ""
        Dim Indx As Integer = 0
        Dim LedFileNm As String = "", GrpFileNm As String = "", VouFileNm As String = ""
        Dim LedErrFileNm As String = "", GrpErrFileNm As String, VouErrFileNm As String = ""
        Dim vStr As String = "", grpnm As String = ""
        Dim LedID As Integer = 0
        Dim vTypAr(20, 3) As String
        Dim Inc_Single As Integer = 0, Inc_All As Integer = 0, I As Integer = 0, J As Integer = 0
        Dim Rf_No As String = "", P_IdNo As String = "", Led_Cond As String = ""
        Dim Opn_Bal As Double = 0

        Try

            MDIParent1.Cursor = Cursors.WaitCursor
            Me.Cursor = Cursors.WaitCursor

            Cmd.Connection = con

            Cmd.Parameters.Clear()
            Cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(msk_FromDate.Text))
            Cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(msk_ToDate.Text))

            MainPath = Trim(txt_Path.Text)
            'MainPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
            'MainPath = Microsoft.VisualBasic.Left(MainPath, 2)

            '-------------------------------------------------------------
            '-----------------------      Masters Posting
            '-------------------------------------------------------------


            'ledger posting - position
            '1. starting                =   "L"
            '2. name                    =   2
            '3. group name              =   62
            '5. sub group               =   92
            '6. mail to                 =   122
            '7. opening balance         =   306 (right allignment, cr=+ dr=-)
            '8. closing balance         =   330       "                "
            '9. sub group               =   331
            '10.allocate space upto     =   381
            '   last cursor position    =   382

            For J = 0 To dgv_Statistics_Details.Rows.Count - 1
                dgv_Statistics_Details.Rows(J).Cells(1).Value = ""
            Next J
            dgv_Statistics_Total.Rows(0).Cells(1).Value = ""

            GrpFileNm = Trim(MainPath) & "\grp.txt"
            LedFileNm = Trim(MainPath) & "\led.txt"
            VouFileNm = Trim(MainPath) & "\vou.txt"

            GrpErrFileNm = Trim(MainPath) & "\grp.log"
            LedErrFileNm = Trim(MainPath) & "\led.log"
            VouErrFileNm = Trim(MainPath) & "\vou.log"

            If File.Exists(GrpErrFileNm) = False Then File.Delete(GrpErrFileNm)
            If File.Exists(LedErrFileNm) = False Then File.Delete(LedErrFileNm)
            If File.Exists(VouErrFileNm) = False Then File.Delete(VouErrFileNm)

            If chk_AllLedgers.Checked = True Then

                Fs = New FileStream(GrpFileNm, FileMode.Create)
                Wr = New StreamWriter(Fs)

                Da1 = New SqlClient.SqlDataAdapter("select * from AccountsGroup_Head where AccountsGroup_IdNo > 30 and AccountsGroup_Name <> '' order by AccountsGroup_Name", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    For I = 0 To Dt1.Rows.Count - 1

                        grpnm = Dt1.Rows(I).Item("AccountsGroup_Name").ToString
                        If Trim(UCase(grpnm)) = "EXPENSES (INDIRECT)" Then grpnm = "INDIRECT EXPENSES"

                        '                                            1                                2                                                                                           62                                                                                            92                                                    306                                                                                                                                                                                                                         331                                                                                 361                      382
                        vStr = "G" & Trim(StrConv(Microsoft.VisualBasic.Left(grpnm, 30), vbProperCase)) & Space(60 - Len(Trim(Microsoft.VisualBasic.Left(grpnm, 30)))) & Trim(StrConv(Dt1.Rows(I).Item("parent_name").ToString, vbProperCase)) & Space(30 - Len(Trim(Dt1.Rows(I).Item("parent_name").ToString))) & Trim(Dt1.Rows(I).Item("tallysubname").ToString) & Space(215 - Len(Trim(Dt1.Rows(I).Item("tallysubname").ToString))) & Space(24) & Trim(StrConv(Dt1.Rows(I).Item("parent_name").ToString, vbProperCase)) & Space(30 - Len(Trim(Dt1.Rows(I).Item("parent_name").ToString))) & Space(22)
                        'vStr = "G" & Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("AccountsGroup_Name").ToString, 30), vbProperCase)) & Space(60 - Len(Trim(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("AccountsGroup_Name").ToString, 30)))) & Trim(StrConv(Dt1.Rows(I).Item("parent_name").ToString, vbProperCase)) & Space(30 - Len(Trim(Dt1.Rows(I).Item("parent_name").ToString))) & Trim(Dt1.Rows(I).Item("tallysubname").ToString) & Space(215 - Len(Trim(Dt1.Rows(I).Item("tallysubname").ToString))) & Space(24) & Trim(StrConv(Dt1.Rows(I).Item("parent_name").ToString, vbProperCase)) & Space(30 - Len(Trim(Dt1.Rows(I).Item("parent_name").ToString))) & Space(22)
                        Wr.WriteLine(vStr)

                    Next
                End If
                Dt1.Clear()

                Wr.Close()
                Fs.Close()


                Fs = New FileStream(LedFileNm, FileMode.Create)
                Wr = New StreamWriter(Fs)

                Cmd.CommandText = "truncate table EntryTempSub"
                Cmd.ExecuteNonQuery()

                If opt_WithOpeningBalance.Checked = True Then
                    Cmd.CommandText = "insert into EntryTempSub ( Int1, Currency1 ) select b.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.parent_code NOT LIKE '%~18~%' and a.voucher_date < @FromDate and a.ledger_idno = b.ledger_idno group by b.ledger_idno"
                    Cmd.ExecuteNonQuery()
                End If

                Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.Currency1 from ledger_head a INNER JOIN AccountsGroup_Head b ON a.parent_code = b.parent_idno LEFT OUTER JOIN EntryTempSub c ON a.ledger_idno = c.Int1 Where a.ledger_name <> '' order by a.ledger_name", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    For I = 0 To Dt1.Rows.Count - 1

                        grpnm = Dt1.Rows(I).Item("AccountsGroup_Name").ToString
                        If Trim(UCase(grpnm)) = "EXPENSES (INDIRECT)" Then grpnm = "INDIRECT EXPENSES"

                        Opn_Bal = 0
                        If Dt1.Rows(I).Item("Currency1").ToString <> "" Then Opn_Bal = Val(Dt1.Rows(I).Item("Currency1").ToString)

                        '                                1                                2                                                                                           62                                                                                            92                                                    306                                                                                                                                                                                                                         331                                                                                 361                      382
                        vStr = "L" & Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("ledger_name").ToString, 30), vbProperCase)) & Space(60 - Len(Trim(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("ledger_name").ToString, 30)))) & Trim(StrConv(grpnm, vbProperCase)) & Space(30 - Len(Trim(grpnm))) & Trim(Dt1.Rows(I).Item("tallysubname").ToString) & Space(215 - Len(Trim(Dt1.Rows(I).Item("tallysubname").ToString)) - Len(Trim(Format(Opn_Bal, "#########0.00")))) & Trim(Format(Opn_Bal, "#########0.00")) & Space(24 - Len(Trim(Format(Opn_Bal, "#########0.00")))) & Trim(Format(Opn_Bal, "#########0.00")) & Trim(StrConv(grpnm, vbProperCase)) & Space(30 - Len(Trim(grpnm))) & Trim(Format(Common_Procedures.Company_FromDate, "yyyyMMdd")) & Space(14)

                        Wr.WriteLine(vStr)

                    Next

                End If
                Dt1.Clear()

                Wr.Close()
                Fs.Close()

            End If

            'voucher posting - position
            '--------------------------
            '1. auto increament number, starting from 0, format ('000000') - 6 digits
            '2. voucher date            =   7   format (yyyymmdd)
            '3. voucher type            =   4   chars
            '4  voucher no              =   19
            '5. ledger idno             =   59
            '6. amount                  =   103 (right allignment)
            '7. '0' (zero)              =   182
            '8. narration               =   198
            '9. last column             =   383

            'Voucher Posting

            Erase vTypAr
            vTypAr = New String(20, 3) {}

            Indx = 0

            If chk_Purchase.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Purc"
                vTypAr(Indx, 1) = "(b.voucher_type='Purc' or b.voucher_type='Yarn.Purc' or b.voucher_type='Clo.Purc' or b.voucher_type='Purch'  or b.voucher_type='SPPurchase' or b.voucher_type='Gst.Purc')"
                vTypAr(Indx, 2) = 0
            End If
            If chk_Sales.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Sale"
                vTypAr(Indx, 1) = "(b.voucher_type='Sale' or b.voucher_type='Clo.Sale' or b.voucher_type='Yarn.Sales' or b.voucher_type='Sales' or b.voucher_type='SalesDsn' or b.voucher_type='SaleSV' or b.voucher_type='SPInvoice' or b.voucher_type='SPWstInv' or b.voucher_type = 'Sales.Ent'  or b.voucher_type = 'V.Sales' or b.voucher_type = 'Gst.Sales' or b.voucher_type = 'GST.Sales' or b.voucher_type = 'GST-Sales' or b.voucher_type = 'Gst.Invoice' or b.voucher_type = 'L.Invoice' or b.voucher_type = 'Gst.InvService' or b.voucher_type LIKE 'InvGar%')"
                'vTypAr(Indx, 1) = "(b.voucher_type='Sale' or b.voucher_type='Clo.Sale' or b.voucher_type='Yarn.Sales' or b.voucher_type='Sales'   or b.voucher_type='SalesDsn'  or b.voucher_type='SaleSV'  or b.voucher_type='SPInvoice'   or b.voucher_type='SPWstInv' or b.voucher_type = 'Gst.Sales' or b.voucher_type = 'V.Sales')"
                vTypAr(Indx, 2) = 1
            End If
            If chk_Receipt.Checked = True Or chk_CashReceipt.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Rcpt"
                If chk_Receipt.Checked = True Then vTypAr(Indx, 1) = "b.voucher_type='Rcpt'"
                If chk_CashReceipt.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='CsRp'"
                If chk_Receipt.Checked = True Or chk_CashReceipt.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='Amt.Rcpt'"
                vTypAr(Indx, 2) = 2
            End If
            If chk_Payment.Checked = True Or chk_CashPayment.Checked = True Or chk_PettiCash.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Pymt"
                If chk_Payment.Checked = True Then vTypAr(Indx, 1) = "b.voucher_type='Pymt'"
                If chk_CashPayment.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='CsPy' "
                If chk_Payment.Checked = True Or chk_CashPayment.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='Wea.Pymt'"
                If chk_PettiCash.Checked = True Then vTypAr(Indx, 1) = vTypAr(Indx, 1) & IIf(vTypAr(Indx, 1) <> "", " or ", "") & " b.voucher_type='PtCs'"
                vTypAr(Indx, 2) = 3
            End If
            If chk_Contra.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Ctra"
                vTypAr(Indx, 1) = "(b.voucher_type='Cntr')"
                vTypAr(Indx, 2) = 4
            End If
            If chk_Journal.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "Jrnl"
                vTypAr(Indx, 1) = "(b.voucher_type='Jrnl' or b.voucher_type='Wea.Wages')"
                vTypAr(Indx, 2) = 5
            End If
            If chk_CreditNote.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "C/N "
                vTypAr(Indx, 1) = "(b.voucher_type='CrNt' or b.voucher_type='Sales.Ret' or b.voucher_type='GstSales.Ret')"
                'vTypAr(Indx, 1) = "(b.voucher_type='CrNt' or b.voucher_type='Sales.Ret')"
                vTypAr(Indx, 2) = 6
            End If
            If chk_DebitNote.Checked = True Then
                Indx = Indx + 1
                vTypAr(Indx, 0) = "D/N "
                vTypAr(Indx, 1) = "(b.voucher_type='DbNt' or b.voucher_type='PurRt' or b.voucher_type='Gst.Purc.Ret')"
                'vTypAr(Indx, 1) = "b.voucher_type='DbNt'"
                vTypAr(Indx, 2) = 7
            End If

            Led_Cond = ""
            If opt_SelectedLedgers.Checked = True Then

                P_IdNo = ""
                For Each itemChecked In chklst_Ledgers.CheckedItems

                    LedID = Common_Procedures.Ledger_NameToIdNo(con, itemChecked)

                    P_IdNo = P_IdNo & IIf(Trim(P_IdNo) <> "", ",", "") & Trim(Val(LedID))

                    'MessageBox.Show("Item with title: " + quote + itemChecked.ToString() + quote + ", is checked. Checked state is: " + CheckedListBox1.GetItemCheckState(CheckedListBox1.Items.IndexOf(itemChecked)).ToString() + ".")

                Next
                'For J = 0 To chklst_Ledgers.Items.Count - 1
                '    If chklst_Ledgers.Items.IndexOf(J) = True Then P_IdNo = P_IdNo & IIf(Trim(P_IdNo) <> "", ",", "") & lst_Ledger.ItemData(J)
                'Next J
                Led_Cond = " and ( b.voucher_code in ( select distinct(z.voucher_code) from voucher_details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.ledger_idno IN ( " & P_IdNo & " ) ) )"

            End If


            Fs = New FileStream(VouFileNm, FileMode.Create)
            Wr = New StreamWriter(Fs)

            For J = 1 To Indx

                Cmd.CommandText = "select b.voucher_code, b.voucher_no, b.voucher_date, b.voucher_type, a.voucher_amount, a.narration, (case when c.ledger_name='Cash A/c' then 'Cash' else c.ledger_name end ) as party_name from voucher_details a, voucher_head b, ledger_head c where a.voucher_amount <> 0 and (" & vTypAr(J, 1) & ") " & Led_Cond & " and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and b.voucher_date between @FromDate and @ToDate and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno and a.ledger_idno = c.ledger_idno order by b.voucher_date, b.for_orderby, b.voucher_code, a.sl_no"
                Da1 = New SqlClient.SqlDataAdapter(Cmd)
                Dt1 = New DataTable
                Da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    Inc_Single = 0 : Rf_No = ""

                    For I = 0 To Dt1.Rows.Count - 1

                        If Rf_No <> Dt1.Rows(I).Item("voucher_code").ToString Then
                            Inc_Single = Inc_Single + 1
                            vStr = Trim(Format(Inc_All + Inc_Single - 1, "000000")) & Trim(Format(Dt1.Rows(I).Item("Voucher_Date"), "yyyyMMdd")) & vTypAr(J, 0) & Trim(Str(Inc_Single)) & Space(40 - Len(Trim(Str(Inc_Single)))) & Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("party_name").ToString, 30), vbProperCase)) & Space(45 - Len(Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("party_name").ToString, 30), vbProperCase))) - Len(Trim(Format(Val(Dt1.Rows(I).Item("voucher_amount").ToString), "###########0.00")))) & Trim(Format(Val(Dt1.Rows(I).Item("voucher_amount").ToString), "###########0.00")) & Space(78) & "0" & Space(15) & Trim(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("Narration").ToString, 120)) & Space(120 - Len(Trim(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("Narration").ToString, 120)))) & Space(30) & " " & Space(34)
                        Else
                            vStr = Trim(Format(Inc_All + Inc_Single - 1, "000000")) & "            " & Space(40) & Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("party_name").ToString, 30), vbProperCase)) & Space(45 - Len(Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("party_name").ToString, 30), vbProperCase))) - Len(Trim(Format(Val(Dt1.Rows(I).Item("voucher_amount").ToString), "###########0.00")))) & Trim(Format(Val(Dt1.Rows(I).Item("voucher_amount").ToString), "###########0.00")) & Space(78) & "0" & Space(15) & Space(150) & " " & Space(34)
                        End If


                        'If Rf_No <> Dt1.Rows(I).Item("voucher_code").ToString Then
                        '    Inc_Single = Inc_Single + 1
                        '    vStr = Trim(Format(Inc_All + Inc_Single - 1, "000000")) & Trim(Format(Dt1.Rows(I).Item("Voucher_Date"), "yyyyMMdd")) & vTypAr(J, 0) & Trim(Str(Inc_Single)) & Space(40 - Len(Trim(Str(Inc_Single)))) & Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("party_name").ToString, 30), vbProperCase)) & Space(45 - Len(Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("party_name").ToString, 30), vbProperCase))) - Len(Trim(Format(Val(Dt1.Rows(I).Item("voucher_amount").ToString), "###########0.00")))) & Trim(Format(Val(Dt1.Rows(I).Item("voucher_amount").ToString), "###########0.00")) & Space(78) & "0" & Space(15) & Trim(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("Narration").ToString, 120)) & Space(120 - Len(Trim(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("Narration").ToString, 120)))) & Space(30) & " " & Space(34)

                        'Else
                        '    vStr = Trim(Format(Inc_All + Inc_Single - 1, "000000")) & "            " & Space(40) & Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("party_name").ToString, 30), vbProperCase)) & Space(45 - Len(Trim(StrConv(Microsoft.VisualBasic.Left(Dt1.Rows(I).Item("party_name").ToString, 30), vbProperCase))) - Len(Trim(Format(Val(Dt1.Rows(I).Item("voucher_amount").ToString), "###########0.00")))) & Trim(Format(Val(Dt1.Rows(I).Item("voucher_amount").ToString), "###########0.00")) & Space(78) & "0" & Space(15) & Space(150) & " " & Space(34)

                        'End If

                        Wr.WriteLine(vStr)

                        Rf_No = Dt1.Rows(I).Item("voucher_code").ToString

                    Next

                    Inc_All = Inc_All + Inc_Single
                    dgv_Statistics_Details.Rows(Val(vTypAr(J, 2))).Cells(1).Value = Inc_Single

                End If
                Dt1.Clear()

            Next

            dgv_Statistics_Total.Rows(0).Cells(1).Value = Val(dgv_Statistics_Details.Rows(0).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(1).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(2).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(3).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(4).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(5).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(6).Cells(1).Value) + Val(dgv_Statistics_Details.Rows(7).Cells(1).Value)

            Wr.Close()
            Fs.Close()

            Wr.Dispose()
            Fs.Dispose()

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show("Exported Sucessfully", "FOR TALLY EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show(ex.Message, "INVALID TALLY EXPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

        End Try

    End Sub

End Class