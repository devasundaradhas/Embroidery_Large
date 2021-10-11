Module Report_Details_1_PDF

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private RptHeading1 As String = ""
    Private RptHeading2 As String = ""
    Private RptHeading3 As String = ""
    Private CompName As String = ""
    Private CompAdd1 As String = ""
    Private CompAdd2 As String = ""

    Private Sub Single_Ledger(LedgerName As String, Optional FromDate As Date = CDate(#1-1-2001#), Optional ToDate As Date = CDate(#12-31-2099#), Optional CompanyIdNo As Int16 = 0)

        Dim Comp_IdNo As Integer, Led_IdNo As Integer

        RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "" And Trim(RptHeading3) <> "", vbCrLf, "") & RptHeading3
        RptHeading3 = ""

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Dtbl1 As New DataTable
        Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource

        Dim CompCondt As String = ""
        Dim RptCondt As String = ""
        Dim IpColNm1 As String = ""
        Dim OpAmt As Double = 0

        'If IsDate(dtp_FromDate.Text) = False Then
        '    MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dtp_FromDate.Visible = True And dtp_FromDate.Enabled = True Then dtp_FromDate.Focus()
        '    Exit Sub
        'End If

        'If IsDate(dtp_ToDate.Text) = False Then
        '    MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dtp_ToDate.Visible = True And dtp_ToDate.Enabled = True Then dtp_ToDate.Focus()
        '    Exit Sub
        'End If

        'Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Inputs1.Text)
        Comp_IdNo = CompanyIdNo
        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, LedgerName)

        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@fromdate", FromDate)
        cmd.Parameters.AddWithValue("@todate", ToDate)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        RptCondt = CompCondt
        IpColNm1 = ""
        'If cbo_Inputs1.Visible = True Then
        '    If Val(Comp_IdNo) <> 0 Then
        '        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Comp_IdNo))
        '        IpColNm1 = "[HIDDEN]"
        '    End If
        'Else
        '    IpColNm1 = "[HIDDEN]"
        'End If
        If Val(Comp_IdNo) <> 0 Then
            RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))
        End If
        If  Val(Led_IdNo) <> 0 Then
            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
        End If

        Amt = 0

        cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date < @fromdate and a.ledger_idno = b.ledger_idno and b.parent_code NOT LIKE '%~18~' and a.company_idno = tZ.company_idno"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                Amt = Val(Dt.Rows(0)(0).ToString)
            End If
        End If
        Dt.Clear()

        BillPend = 0


        cmd.CommandText = "Truncate table ReportTemp"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTemp(Int5, Meters1, Name1, Name2, Name3, Name4, Name5, Currency1, Currency2, Meters6 ) values (0, 0, 'OPENING', '', 'OPENING', '', '', " & IIf(Amt < 0, Math.Abs(Amt), 0) & ", " & IIf(Amt > 0, Amt, 0) & ", " & Str(Val(BillPend)) & " ) "
        cmd.ExecuteNonQuery()

        RptCondt = CompCondt
        If Val(Comp_IdNo) <> 0 Then
            RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))
        End If

        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))



        cmd.CommandText = "Insert into ReportTemp(Meters5, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Name7, Name8, Name9) select 0, 1, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'To ' + c.ledger_name, Abs(a.voucher_amount), 0, a.narration, a.Voucher_Type, a.Entry_Identification, tZ.Company_ShortName, c.Parent_Code from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN voucher_head b ON a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno LEFT OUTER JOIN ledger_head c ON b.creditor_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.voucher_amount < 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into ReportTemp(Meters5, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Name7, Name8, Name9) select 0, 2, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'By ' + c.ledger_name, 0, a.Voucher_Amount, a.narration, a.Voucher_Type, a.Entry_Identification, tZ.Company_ShortName, c.Parent_Code from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN voucher_head b ON a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno LEFT OUTER JOIN ledger_head c ON b.debtor_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.voucher_amount > 0"
            cmd.ExecuteNonQuery()



        If Trim(LCase(RptIpDet_ReportName)) = "weaver amount balance details" Then
            cmd.CommandText = "Insert into ReportTemp(Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Meters6, Name9) select 10, a.Weaver_ClothReceipt_Date, a.For_OrderBy, 'WCLRC-' + a.Weaver_ClothReceipt_Code, a.Weaver_ClothReceipt_No, 'By ' + c.ledger_name, 0, 0, 'Party Dc.No. : ' + a.Party_DcNo, 'Clo.Rcpt', a.Receipt_Meters, C.Parent_Code from Weaver_Cloth_Receipt_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN ledger_head c ON a.Ledger_IdNo = c.Ledger_IdNo where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Weaver_ClothReceipt_Date between @fromdate and @todate and a.Receipt_Meters <> 0 and a.Weaver_Wages_Code = ''"
            cmd.ExecuteNonQuery()
        End If

        cmd.CommandText = "Update ReportTemp SET Meters5 = b.LedgerOrder_Position from ReportTemp a, AccountsGroup_Head b Where a.Name9 = b.Parent_Idno"
        cmd.ExecuteNonQuery()

        Tot_CR = 0 : Tot_DB = 0
        If RptIpDet_IsGridReport = True Then

            Da = New SqlClient.SqlDataAdapter("select Date1 as VouDate, Name5 as VouType, Name8 as Company_ShortName, Name2 as VouNo, Name3 as Particulars, Currency1 as Debit, Currency2 as Credit, Name6 as Balance, Name4 as Narration, Name7 as VoucherCode from reporttemp Order by Date1, Meters5, Int5, meters1, name2, name1", con)
            Dtbl1 = New DataTable
            Da.Fill(Dtbl1)

            Bal = 0
            If Dtbl1.Rows.Count > 0 Then
                Tot_DB = Tot_DB + Val(Dtbl1.Rows(0).Item("Debit").ToString)
                Tot_CR = Tot_CR + Val(Dtbl1.Rows(0).Item("Credit").ToString)
                Bal = Val(Dtbl1.Rows(0).Item("Debit").ToString) - Val(Dtbl1.Rows(0).Item("Credit").ToString)
                Dtbl1.Rows(0).Item("Balance") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                For i = 1 To Dtbl1.Rows.Count - 1
                    Tot_DB = Tot_DB + Val(Dtbl1.Rows(i).Item("Debit").ToString)
                    Tot_CR = Tot_CR + Val(Dtbl1.Rows(i).Item("Credit").ToString)
                    Bal = Val(Bal) + Val(Dtbl1.Rows(i).Item("Debit").ToString) - Val(Dtbl1.Rows(i).Item("Credit").ToString)
                    Dtbl1.Rows(i).Item("Balance") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                Next i

            End If

            If Dtbl1.Rows.Count = 0 Then

                cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("select Date1 as VouDate, Name5 as VouType, Name2 as VouNo, Name3 as Particulars, Currency1 as Debit, Currency2 as Credit, Name6 as Balance, Name4 as Narration from reporttemp Order by Date1, Int5, meters1, name2, name1", con)
                Dtbl1 = New DataTable
                Da.Fill(Dtbl1)

            End If

            Dim MyNewRow As DataRow
            MyNewRow = Dtbl1.NewRow
            With MyNewRow
                .Item(4) = "TOTAL"
                .Item(5) = Format(Tot_DB, "0000000.00")
                .Item(6) = Format(Tot_CR, "0000000.00")
                .Item(7) = Common_Procedures.Currency_Format(Math.Abs(Bal)) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                .Item(8) = ""
                .Item(9) = ""
            End With
            Dtbl1.Rows.Add(MyNewRow)
            Dtbl1.AcceptChanges()

        Else

            Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5, Meters6, Name7 from reporttemp Order by Date1, Meters5, Int5, meters1, name2, name1", con)
            Dtbl1 = New DataTable
            Da.Fill(Dtbl1)

            Bal = 0
            If Dtbl1.Rows.Count > 0 Then
                Bal = Val(Dtbl1.Rows(0).Item("Currency1").ToString) - Val(Dtbl1.Rows(0).Item("Currency2").ToString)
                Dtbl1.Rows(0).Item("Name6") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                For i = 1 To Dtbl1.Rows.Count - 1
                    Bal = Val(Bal) + Val(Dtbl1.Rows(i).Item("Currency1").ToString) - Val(Dtbl1.Rows(i).Item("Currency2").ToString)
                    Dtbl1.Rows(i).Item("Name6") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                Next i
            End If

            If Dtbl1.Rows.Count = 0 Then

                cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5, Meters6 from reporttemp Order by Int5, Date1, meters1, name2, name1", con)
                Dtbl1 = New DataTable
                Da.Fill(Dtbl1)

            End If

        End If


        If RptIpDet_IsGridReport = True Then

            With dgv_Report
                .SuspendLayout()
                Application.DoEvents()

                .BackgroundColor = Color.White
                .BorderStyle = BorderStyle.FixedSingle

                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToOrderColumns = False
                .ReadOnly = True
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
                .AllowUserToResizeColumns = False
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                .AllowUserToResizeRows = False

                .DefaultCellStyle.SelectionBackColor = Color.Lime
                .DefaultCellStyle.SelectionForeColor = Color.Blue

                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                .Columns.Clear()
                .DataSource = Dtbl1
                .RowHeadersVisible = False
                .AllowUserToOrderColumns = False

                .Columns(0).HeaderText = "DATE"
                .Columns(1).HeaderText = "VOU.TYPE"
                .Columns(2).HeaderText = "COMPANY"
                .Columns(3).HeaderText = "VOU.NO"
                .Columns(4).HeaderText = "PARTICULARS"
                .Columns(5).HeaderText = "DEBIT"
                .Columns(6).HeaderText = "CREDIT"
                .Columns(7).HeaderText = "BALANCE"
                .Columns(8).HeaderText = "NARRATION"
                .Columns(9).HeaderText = "voucher_code [HIDDEN]"

                .Columns(2).Visible = True
                If Trim(IpColNm1) <> "" Then
                    .Columns(2).Visible = False
                End If
                .Columns(9).Visible = False

                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

                .RowsDefaultCellStyle.BackColor = Color.White
                .AlternatingRowsDefaultCellStyle.BackColor = Color.Beige

                '.RowsDefaultCellStyle.BackColor = Color.Bisque
                '.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige

                '.RowsDefaultCellStyle.BackColor = Color.LightGray
                '.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray

                .Columns(0).FillWeight = 65
                .Columns(1).FillWeight = 55
                .Columns(2).FillWeight = 50
                .Columns(3).FillWeight = 45
                .Columns(4).FillWeight = 180
                .Columns(5).FillWeight = 75
                .Columns(6).FillWeight = 75
                .Columns(7).FillWeight = 85
                .Columns(8).FillWeight = 175
                .Columns(9).FillWeight = 100

                .Columns(5).DefaultCellStyle.Alignment = 4
                .Columns(6).DefaultCellStyle.Alignment = 4
                .Columns(7).DefaultCellStyle.Alignment = 4

                .Columns(0).ReadOnly = True
                .Columns(1).ReadOnly = True
                .Columns(2).ReadOnly = True
                .Columns(3).ReadOnly = True
                .Columns(4).ReadOnly = True
                .Columns(5).ReadOnly = True
                .Columns(6).ReadOnly = True
                .Columns(7).ReadOnly = True
                .Columns(8).ReadOnly = True
                .Columns(9).ReadOnly = True

                .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable

                N = .Rows.Count - 1
                .Rows(N).Height = 40
                For j = 0 To .ColumnCount - 1
                    .Rows(N).Cells(j).Style.BackColor = Color.LightGray
                    .Rows(N).Cells(j).Style.ForeColor = Color.Red
                Next

                .Visible = True
                .ResumeLayout()

                .BringToFront()
                .Focus()

                If .Rows.Count > 0 Then
                    .CurrentCell = .Rows(0).Cells(0)
                    .CurrentCell.Selected = True
                End If

            End With


        Else

            RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
            RpDs1.Name = "DataSet1"
            RpDs1.Value = Dtbl1

            If Trim(LCase(RptIpDet_ReportName)) = "weaver amount balance details" Then
                RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Weaver_Amount_Balance_Details.rdlc"
            ElseIf Common_Procedures.settings.CustomerCode = "5007" Then
                RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SingleLedger_5007.rdlc"
            Else
                RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SingleLedger.rdlc"
            End If

            RptViewer.LocalReport.DataSources.Clear()

            RptViewer.LocalReport.DataSources.Add(RpDs1)

            RptViewer.LocalReport.Refresh()
            RptViewer.RefreshReport()

            RptViewer.Visible = True
            RptViewer.Focus()
            SendKeys.Send("{TAB}")

        End If

    End Sub

End Module
