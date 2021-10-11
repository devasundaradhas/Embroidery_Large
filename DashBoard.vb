Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail
Imports System.IO.Ports
Imports System.Windows.Forms.DataVisualization.Charting

Public Class DashBoard

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private Current_Date As Date = Now
    Private CurX As Integer = 0


    Private Sub lbl_Close_Left_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub DashBoard_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub



    Private Sub DashBoard_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.Open()


        Me.Top = 0
        Me.Left = 0
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width - 9 ' 15
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height - 90 ' 100


        pnl_Back.Top = 0
        pnl_Back.Left = 0
        pnl_Back.Width = Me.Width
        pnl_Back.Height = Me.Height


        btn_Close.Left = Me.Width - 40
        btn_Close.Top = Me.Top + 10
        CurX = 10


        dgv_OverDueInvoices.DefaultCellStyle.ForeColor = Color.Blue
        dgv_OverDueBills.DefaultCellStyle.ForeColor = Color.Blue

        '    pnl_OverDue.Top = Panel1.Top

        Display()


    End Sub

    Private Sub Display()


        OverDue_Purchase()
        OverDue_Sales()
        Aged_PurchaseBills()
        Aged_SalesBills()
        Net_Income()
        Chart_IncomeAndExpense()
        Pie_Chart()
        Active_Orders()

    End Sub

    Private Sub OverDue_Purchase()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0



        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)



        cmd.CommandText = "truncate table ReportTemp"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTemp ( int1, name2, currency1) Select b.ledger_idno, b.ledger_name, sum(a.voucher_amount) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where tG.parent_idno NOT LIKE '%~18~' and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = 14 and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno = tz.company_idno group by b.ledger_idno, b.ledger_name having sum(a.voucher_amount) <> 0"
        cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("Select name2 as LedgerName , Int1 as LedgerIdno, sum(currency1) as Amount from ReportTemp group by name1 ,name2,int1 having sum(currency1) > 0 Order by name2", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then

            dgv_OverDueInvoices.Rows.Clear()

            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Amount").ToString) <> 0 Then

                    With dgv_OverDueInvoices

                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = "*"
                        .Rows(n).Cells(1).Value = Trim(Dt.Rows(i).Item("LedgerName").ToString)
                        .Rows(n).Cells(2).Value = Common_Procedures.Currency_Format(Val(Dt.Rows(i).Item("Amount").ToString))
                        .Rows(n).Cells(3).Value = "Remind"
                        .Rows(n).Cells(4).Value = Val(Dt.Rows(i).Item("LedgerIdno").ToString)

                        If .Height < 200 Then

                            CurX = CurX + 25

                            .Height = .Height + 25
                            ' pnl_OverDue.Height = pnl_OverDue.Height + 25

                        End If
                        '    Me.Height = Me.Height + 25
                    End With


                End If
            Next
        End If

        Dt.Clear()
        Da.Dispose()

        On Error Resume Next
        dgv_OverDueInvoices.CurrentCell.Selected = False

    End Sub

    Private Sub OverDue_Sales()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0


        'lbl_OverDue_Bills.Top = CurX + 30

        'dgv_OverDueBills.Top = CurX + lbl_OverDue_Bills.Height + 30

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)


        cmd.CommandText = "truncate table ReportTemp"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTemp ( int1, name2, name3, currency1) Select b.ledger_idno, b.ledger_name,b.ledger_phoneno, sum(a.voucher_amount) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where tG.parent_idno NOT LIKE '%~18~' and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = 10 and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno = tz.company_idno group by b.ledger_idno, b.ledger_name, b.ledger_phoneno having sum(a.voucher_amount) <> 0"
        cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("select name2 as LedgerName , name3 as PhoneNo, Int1 as LedgerIdno, abs(sum(currency1)) as Amount from ReportTemp group by name2 ,name3,int1 having sum(currency1) < 0 Order by name2", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then

            dgv_OverDueBills.Rows.Clear()
            'dgv_OverDueBills.Height = 0

            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Amount").ToString) <> 0 Then
                    With dgv_OverDueBills
                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = "*"
                        .Rows(n).Cells(1).Value = Trim(Dt.Rows(i).Item("LedgerName").ToString)
                        .Rows(n).Cells(2).Value = Common_Procedures.Currency_Format(Val(Dt.Rows(i).Item("Amount").ToString))
                        .Rows(n).Cells(3).Value = "Remind"
                        .Rows(n).Cells(4).Value = Val(Dt.Rows(i).Item("LedgerIdno").ToString)
                        .Rows(n).Cells(5).Value = Dt.Rows(i).Item("PhoneNo")

                        If .Height < 200 Then
                            .Height = .Height + 25
                            'pnl_OverDue.Height = pnl_OverDue.Height + 25

                        End If
                        'Me.Height = Me.Height + 25
                    End With


                End If
            Next
        End If

        Dt.Clear()
        Da.Dispose()


        On Error Resume Next
        dgv_OverDueBills.CurrentCell.Selected = False

    End Sub

    Private Sub dgv_OverDueBills_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Dim vPhnNo As String = ""
        Dim Smstxt As String = ""
        Dim vLedId As Integer = 0

        With dgv_OverDueBills
            If e.ColumnIndex = 3 Then
                If Val(.Rows(e.RowIndex).Cells(2).Value) <> 0 Then
                    If MessageBox.Show("Do you want to send Reminder Sms to  " & Trim(.Rows(e.RowIndex).Cells(1).Value) & " ?", "FOR REMINDER SMS...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
                        vPhnNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "Ledger_IdNo = " & Val(.Rows(e.RowIndex).Cells(4).Value))
                        If vPhnNo <> "" Then
                            Smstxt = "Mr. " & Trim(.Rows(e.RowIndex).Cells(1).Value) & " , Your Balance Bill Amount Rs." & Val(.Rows(e.RowIndex).Cells(2).Value) & "/- , Pay Immediatly.."

                            If REMINDER_SMS(vPhnNo, Smstxt, 1) = True Then
                                MessageBox.Show("Sms Send Successfully...", "SENDED..", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        Else
                            If MessageBox.Show("Phone No. not found.." & vbCrLf & "Do you want to add Phone No.", "NOT SEND..", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = vbYes Then
                                Dim F2 As New Ledger_Creation
                                F2.MdiParent = MDIParent1
                                F2.Show()
                                F2.move_record(Val(.Rows(e.RowIndex).Cells(4).Value))

                            End If
                        End If


                    End If
                End If

            End If


        End With

    End Sub
    Private Function REMINDER_SMS(ByVal vPhNo As String, ByVal smstext As String, Optional ByVal Gateway As Integer = 1) As Boolean
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse = Nothing
        Dim url As String
        Dim timeout As Integer = 50000

        REMINDER_SMS = False

        Try
            url = ""
            If Gateway = 1 Then

                url = "http://sms1.shamsoft.in/api/mt/SendSMS?APIKey=" & Trim(Common_Procedures.settings.SMS_Provider_Key) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&channel=2&DCS=0&flashsms=0&number=" & Trim(vPhNo) & "&text=" & Trim(smstext) & "&route=" & Trim(Common_Procedures.settings.SMS_Provider_RouteID)

            ElseIf Gateway = 2 Then

                'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(Common_Procedures.settings.SMS_Provider_Key_1) & "&routeid=" & Trim(Common_Procedures.settings.SMS_Provider_RouteID_1) & "&type=" & Trim(Common_Procedures.settings.SMS_Provider_Type_1) & "&contacts=" & Trim(vPhNo) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID_1) & "&msg=" & Trim(smstext)
            End If

            request = DirectCast(WebRequest.Create(url), HttpWebRequest)
            request.KeepAlive = True

            request.Timeout = timeout

            response = DirectCast(request.GetResponse(), HttpWebResponse)

            'If Trim(UCase(response.StatusDescription)) = "OK" Then
            '    MessageBox.Show("Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Else
            '    MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'End If

            REMINDER_SMS = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND OTP...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            response.Close()

            response = Nothing
            request = Nothing

        End Try

    End Function


    Private Sub Aged_PurchaseBills()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        ' Dim Comp_IdNo As Integer
        Dim b() As String
        Dim i As Integer
        Dim S As String
        Dim oldvl As String
        Dim RepPeriods As String = ""
        Dim Nr As Integer = 0
        Dim ParentCode As String = "~14~11~"   '-Sundry Creditors

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@uptodate", Current_Date)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        ' RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))

        If Trim(RepPeriods) = "" Then
            RepPeriods = "30,60,90,120"
        End If


        b = Split(RepPeriods, ",")

        Cmd.Connection = con

        Cmd.CommandText = "truncate table reporttempsub"
        Cmd.ExecuteNonQuery()

        oldvl = "0"

        For i = 0 To UBound(b)

            S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

            Cmd.CommandText = "Insert into reporttempsub (  Int1                          ,  currency1) " & _
                                        " Select  datediff(dd, a.voucher_date, @uptodate) ,      0    from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and tP.Bill_Type = 'BILL TO BILL'  and tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.voucher_amount > 0   group by a.voucher_date"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into reporttempsub ( Int1                                  ,  currency1) " & _
                                        " Select    datediff(dd, a.voucher_date, @uptodate) , sum(a.voucher_Amount) from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo  and tP.Bill_Type = 'BILL TO BILL' and  tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.voucher_amount > 0  group by a.voucher_date"
            Cmd.ExecuteNonQuery()

            oldvl = Val(b(i)) + 1

        Next i



        If Val(oldvl) <> 0 Then

            S = "ABV " & Trim(Val(oldvl) - 1)

            Cmd.CommandText = "Insert into reporttempsub ( Int1                          , currency1) " & _
                                        " Select datediff(dd, a.voucher_date, @uptodate) ,      0    from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and tP.Bill_Type = 'BILL TO BILL'   and tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) >= " & Str(Val(oldvl)) & " and  a.voucher_amount > 0  group by a.voucher_date"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into reporttempsub ( Int1                                , currency1) " & _
                                        " Select      datediff(dd, a.voucher_date, @uptodate)  ,  sum(a.voucher_Amount) from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and tP.Bill_Type = 'BILL TO BILL'  and tP.Parent_Code = '" & Trim(ParentCode) & "'  Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate)  >= " & Str(Val(oldvl)) & " and  a.voucher_amount > 0  group by a.voucher_date"
            Cmd.ExecuteNonQuery()

        End If


        Cmd.CommandText = "truncate table reporttemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into ReportTemp ( Int1 , currency1 ) Select Int1 , sum(currency1) from reporttempsub group by Int1 having sum(currency1) <> 0 "
        Cmd.ExecuteNonQuery()


        Da = New SqlClient.SqlDataAdapter("select Int1, currency1 from reporttemp", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Int1").ToString) <= 30 Then

                    lbl_Inv_1to30.Text = Format(Val(lbl_Inv_1to30.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 60 Then

                    lbl_Inv_31to60.Text = Format(Val(lbl_Inv_31to60.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 90 Then

                    lbl_Inv_61to90.Text = Format(Val(lbl_Inv_61to90.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 120 Then

                    lbl_Inv_91to120.Text = Format(Val(lbl_Inv_91to120.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) > 120 Then

                    lbl_Inv_Above120.Text = Format(Val(lbl_Inv_Above120.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")
                End If

            Next
        End If

        Dt.Clear()
        Da.Dispose()

        If Val(lbl_Inv_1to30.Text) <> 0 Then
            lbl_Inv_1to30.Text = "₹ " & Trim(lbl_Inv_1to30.Text)
        End If

        If Val(lbl_Inv_31to60.Text) <> 0 Then
            lbl_Inv_31to60.Text = "₹ " & Trim(lbl_Inv_31to60.Text)
        End If

        If Val(lbl_Inv_61to90.Text) <> 0 Then
            lbl_Inv_61to90.Text = "₹ " & Trim(lbl_Inv_61to90.Text)
        End If

        If Val(lbl_Inv_91to120.Text) <> 0 Then
            lbl_Inv_91to120.Text = "₹ " & Trim(lbl_Inv_91to120.Text)
        End If

        If Val(lbl_Inv_Above120.Text) <> 0 Then
            lbl_Inv_Above120.Text = "₹ " & Trim(lbl_Inv_Above120.Text)
        End If

    End Sub
    Private Sub Aged_SalesBills()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        '  Dim Comp_IdNo As Integer
        Dim b() As String
        Dim i As Integer
        Dim S As String
        Dim oldvl As String
        Dim RepPeriods As String = ""
        Dim ParentCode As String = "~10~4~"   '-Sundry Debtors

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@uptodate", Current_Date)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        ' RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))

        If Trim(RepPeriods) = "" Then
            RepPeriods = "30,60,90,120"
        End If


        b = Split(RepPeriods, ",")

        Cmd.Connection = con

        Cmd.CommandText = "truncate table reporttempsub"
        Cmd.ExecuteNonQuery()

        oldvl = "0"

        'For i = 0 To UBound(b)

        '    S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

        '    Cmd.CommandText = "Insert into reporttempsub (  Int1                               ,  currency1) " & _
        '                                " Select  datediff(dd, a.voucher_bill_date, @uptodate) ,      0    from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.debit_amount > a.credit_amount  group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        '    Cmd.CommandText = "Insert into reporttempsub ( Int1                                  ,  currency1) " & _
        '                                " Select    datediff(dd, a.voucher_bill_date, @uptodate) ,  sum(a.debit_amount - a.credit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.debit_amount > a.credit_amount group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        '    oldvl = Val(b(i)) + 1

        'Next i

        'If Val(oldvl) <> 0 Then

        '    S = "ABV " & Trim(Val(oldvl) - 1)

        '    Cmd.CommandText = "Insert into reporttempsub ( Int1                               , currency1) " & _
        '                                " Select datediff(dd, a.voucher_bill_date, @uptodate) ,      0    from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) >= " & Str(Val(oldvl)) & " and  a.debit_amount > a.credit_amount group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        '    Cmd.CommandText = "Insert into reporttempsub ( Int1                                     , currency1) " & _
        '                                " Select      datediff(dd, a.voucher_bill_date, @uptodate)  , sum(a.debit_amount - a.credit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate)  >= " & Str(Val(oldvl)) & " and  a.debit_amount > a.credit_amount group by a.voucher_bill_date"
        '    Cmd.ExecuteNonQuery()

        'End If



        For i = 0 To UBound(b)

            S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

            Cmd.CommandText = "Insert into reporttempsub (  Int1                          ,  currency1) " & _
                                        " Select  datediff(dd, a.voucher_date, @uptodate) ,      0    from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and  tP.Bill_Type = 'BILL TO BILL'  and tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.voucher_amount < 0   group by A.ledger_idno , a.voucher_date"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into reporttempsub ( Int1                                  ,  currency1) " & _
                                        " Select    datediff(dd, a.voucher_date, @uptodate) , -1* sum(a.voucher_Amount) from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and  tP.Bill_Type = 'BILL TO BILL'  and  tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and  a.voucher_amount < 0  group by A.ledger_idno , a.voucher_date"
            Cmd.ExecuteNonQuery()

            oldvl = Val(b(i)) + 1

        Next i



        If Val(oldvl) <> 0 Then

            S = "ABV " & Trim(Val(oldvl) - 1)

            Cmd.CommandText = "Insert into reporttempsub ( Int1                          , currency1) " & _
                                        " Select datediff(dd, a.voucher_date, @uptodate) ,      0    from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and  tP.Bill_Type = 'BILL TO BILL' and  tP.Parent_Code = '" & Trim(ParentCode) & "'  Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate) >= " & Str(Val(oldvl)) & " and  a.voucher_amount < 0  group by A.ledger_idno , a.voucher_date"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into reporttempsub ( Int1                                , currency1) " & _
                                        " Select      datediff(dd, a.voucher_date, @uptodate)  , -1 * sum(a.voucher_Amount) from voucher_details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo and  tP.Bill_Type = 'BILL TO BILL'  and  tP.Parent_Code = '" & Trim(ParentCode) & "' Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and datediff(dd, a.voucher_date, @uptodate)  >= " & Str(Val(oldvl)) & " and  a.voucher_amount < 0  group by A.ledger_idno , a.voucher_date"
            Cmd.ExecuteNonQuery()

        End If

        Cmd.CommandText = "truncate table reporttemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into ReportTemp ( Int1 , currency1 ) Select Int1 , sum(currency1) from reporttempsub group by Int1 having sum(currency1) <> 0 "
        Cmd.ExecuteNonQuery()


        Da = New SqlClient.SqlDataAdapter("select Int1, currency1 from reporttemp", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1

                If Val(Dt.Rows(i).Item("Int1").ToString) <= 30 Then

                    lbl_Bill_1to30.Text = Format(Val(lbl_Bill_1to30.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 60 Then

                    lbl_Bill_31to60.Text = Format(Val(lbl_Bill_31to60.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 90 Then

                    lbl_Bill_61to90.Text = Format(Val(lbl_Bill_61to90.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) <= 120 Then

                    lbl_Bill_91to120.Text = Format(Val(lbl_Bill_91to120.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")

                ElseIf Val(Dt.Rows(i).Item("Int1").ToString) > 120 Then

                    lbl_Bill_Above120.Text = Format(Val(lbl_Bill_Above120.Text) + Val(Dt.Rows(i).Item("currency1").ToString), "##########0.00")
                End If

            Next
        End If

        Dt.Clear()
        Da.Dispose()

        If Val(lbl_Bill_1to30.Text) <> 0 Then
            lbl_Bill_1to30.Text = "₹ " & Trim(lbl_Bill_1to30.Text)
        End If

        If Val(lbl_Bill_31to60.Text) <> 0 Then
            lbl_Bill_31to60.Text = "₹ " & Trim(lbl_Bill_31to60.Text)
        End If

        If Val(lbl_Bill_61to90.Text) <> 0 Then
            lbl_Bill_61to90.Text = "₹ " & Trim(lbl_Bill_61to90.Text)
        End If

        If Val(lbl_Bill_91to120.Text) <> 0 Then
            lbl_Bill_91to120.Text = "₹ " & Trim(lbl_Bill_91to120.Text)
        End If

        If Val(lbl_Bill_Above120.Text) <> 0 Then
            lbl_Bill_Above120.Text = "₹ " & Trim(lbl_Bill_Above120.Text)
        End If

    End Sub



    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        MDIParent1.mnu_Tools_Dashboard.Text = "Show DashBoard"
        Me.Close()
    End Sub
    Private Sub Net_Income()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim Curyear As String = ""
        Dim Prevyear As String = ""
        ' Dim ParentCode As String = "~10~4~"   '-Sundry Debtors


        Curyear = Current_Date.Year
        Prevyear = Current_Date.Year - 1

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)




        cmd.CommandText = "truncate table reporttempsub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "insert into reporttempsub ( int1,Name1                     ,  currency1       ) " & _
                                   "Select tZ.company_idno ,year(a.Voucher_Date) ,     sum(a.Voucher_Amount)  from voucher_Details a INNER JOIN company_head tz  ON a.company_idno <> 0 and a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head Lh ON a.Ledger_Idno = lh.ledger_Idno  where (LH.Parent_Code = '~19~18~' ) and a.Voucher_Amount <> 0  and  a.voucher_date <= @uptodate  group by tZ.company_idno, a.Voucher_Date"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "insert into reporttempsub ( int1 , Name1                    ,currency2 ) " & _
                                 "Select   tZ.company_idno , year(a.Voucher_Date) , -1 *  sum(a.Voucher_Amount)  from voucher_Details a INNER JOIN company_head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head Lh ON a.Ledger_Idno = lh.ledger_Idno  Where (LH.Parent_Code = '~15~18~' or LH.Parent_Code = '~16~18~') and a.Voucher_Amount <> 0  and  a.voucher_date <= @uptodate  group by tZ.company_idno, a.Voucher_Date"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "truncate table ReportTemp"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into ReportTemp ( int1    ,Name1  , currency1      ,Currency2) " & _
                                             " Select int1  ,Name1  , sum(currency1) , sum(currency2) from reporttempsub  group by  int1,name1"
        cmd.ExecuteNonQuery()


        Da = New SqlClient.SqlDataAdapter("select Name1, sum(currency1) as income, sum(currency2) as expense from ReportTemp group by Name1  Order by name1", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then



            For i = 0 To Dt.Rows.Count - 1

                If Trim(Dt.Rows(i).Item("Name1").ToString) = Curyear Then

                    lbl_CurYear.Text = Trim(Dt.Rows(i).Item("Name1").ToString)
                    lbl_CurIncome.Text = Format(IIf(Val(Dt.Rows(i).Item("income").ToString) < 0, -1 * Val(Dt.Rows(i).Item("income").ToString), Val(Dt.Rows(i).Item("income").ToString)), "#########0.00")
                    lbl_CurExpenses.Text = Format(Val(Dt.Rows(i).Item("expense").ToString), "#########0.00")
                    lbl_CurNetIncome.Text = Format(Val(lbl_CurIncome.Text) - Val(lbl_CurExpenses.Text), "############0.00")

                ElseIf Trim(Dt.Rows(i).Item("Name1").ToString) = Prevyear Then


                    lbl_PrevYear.Text = Trim(Dt.Rows(i).Item("Name1").ToString)
                    lbl_PrevIncome.Text = Format(Val(Dt.Rows(i).Item("income").ToString), "##########0.00")
                    lbl_PrevExpenses.Text = Format(Val(Dt.Rows(i).Item("expense").ToString), "##########0.00")
                    lbl_PrevNetIncome.Text = Format(Val(lbl_PrevIncome.Text) - Val(lbl_PrevExpenses.Text), "##########0.00")
                End If
            Next
        End If

        Dt.Clear()
        Da.Dispose()


    End Sub
    Private Sub Chart_IncomeAndExpense()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        Dim Nr As Integer = 0


        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        cmd.Connection = con

        cmd.CommandText = "truncate table reporttempsub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "truncate table ReportTemp"
        cmd.ExecuteNonQuery()


        For i = 1 To 12

            cmd.CommandText = "Insert into ReportTemp ( Int1   ,  NAME1          ,  currency1 , Currency2) " & _
                                " Select     MH.Idno   ,  MH.Month_ShortName  ,    0       ,   0     from  Month_Head MH where MH.Month_IdNo =" & i & " "
            cmd.ExecuteNonQuery()
        Next


        cmd.CommandText = "Insert into reporttempsub ( Int1   ,  NAME1               ,  currency1) " & _
                                    " Select     MH.Idno,  MH.Month_ShortName  ,     sum( a.Voucher_Amount)   from voucher_Details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo LEFT OUTER JOIN Month_Head MH ON MH.Month_IdNo = MONTH(a.voucher_date)   Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " (tP.Parent_Code = '~19~18~') AND a.voucher_date BETWEEN @companyfromdate AND  @uptodate group by MH.Month_ShortName ,MH.Idno "
        Nr = cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into reporttempsub ( Int1 ,  NAME1                ,  currency2) " & _
                                    " Select  MH.Idno , MH.Month_ShortName    ,   -1 *  sum(a.Voucher_Amount)  from voucher_Details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo LEFT OUTER JOIN Month_Head MH ON MH.Month_IdNo = MONTH(a.voucher_date)  Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " (tP.Parent_Code = '~15~18~' or tP.Parent_Code = '~16~18~') and a.voucher_date BETWEEN @companyfromdate AND @uptodate   group by MH.Month_ShortName ,MH.Idno"
        Nr = cmd.ExecuteNonQuery()



        cmd.CommandText = "Insert into ReportTemp ( int1 , NAME1   ,  currency1       ,  currency2   ) " & _
                                   " Select         int1 , NAME1   , sum(currency1)   , sum(currency2) from reporttempsub GROUP BY NAME1,int1 order by int1 asc"
        cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("select NAME1, sum(currency1) AS Income ,SUM(currency2) as Expense from ReportTemp group by NAME1, int1  order by int1 asc", con)
        Dt = New DataTable
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1

                Chart1.Series("Expenses").Points.AddXY((Trim(Dt.Rows(i).Item("NAME1").ToString)), Dt.Rows(i).Item("Expense").ToString)
                Chart1.Series("Income").Points.AddXY((Trim(Dt.Rows(i).Item("NAME1").ToString)), Dt.Rows(i).Item("Income").ToString)
            Next
        End If


        If Nr = 0 Then Chart1.Visible = False


    End Sub
    Private Sub Pie_Chart()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        Dim Nr As Integer = 0
        Dim Total_Value As Double = 0
        Dim Percen As Double = 0

        With Me.Chart2
            .Legends.Clear()
            .Series.Clear()
            .ChartAreas.Clear()
        End With

        Dim areas1 As ChartArea = Me.Chart2.ChartAreas.Add("Areas1")

        With areas1
        End With

        Dim series1 As Series = Me.Chart2.Series.Add("Series1")
        series1.ChartArea = areas1.Name
        series1.ChartType = SeriesChartType.Pie
        series1("PieLabelStyle") = "Disabled"


        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        cmd.Connection = con

        cmd.CommandText = "truncate table reporttempsub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "truncate table ReportTemp"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into reporttempsub (   NAME1               ,  currency1) " & _
                                    " Select            tP.Ledger_Name       ,   sum( a.Voucher_Amount)     from voucher_Details a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo    Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " ( tP.Parent_Code = '~15~18~' or tP.Parent_Code = '~16~18~') and a.voucher_date BETWEEN @companyfromdate AND  @uptodate  group by  tP.Ledger_Name"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into ReportTemp (  NAME1   ,  currency1        ) " & _
                                   " Select          NAME1   , sum(currency1)    from reporttempsub GROUP BY NAME1 order by NAME1 asc"
        cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("select NAME1, sum(currency1) AS Expenses  from ReportTemp group by NAME1 ", con)
        Dt = New DataTable
        Da.Fill(Dt)


        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1
                With series1
                   
                    Total_Value = Val(Dt.Rows(i).Item("Expenses").ToString)
                    
                End With
            Next

        Else

            Chart2.Visible = False
        End If

        If Total_Value <> 0 Then

            If Dt.Rows.Count > 0 Then
                For i = 0 To Dt.Rows.Count - 1
                    With series1

                        If Val(Dt.Rows(i).Item("Expenses").ToString) <> 0 Then
                            Percen = (Val(Dt.Rows(i).Item("Expenses").ToString) / Total_Value) * 100
                        End If
                        .Points.AddXY(Trim(Dt.Rows(i).Item("NAME1").ToString), Percen)

                    End With
                Next
            End If

        End If

        Dim legends1 As Legend = Me.Chart2.Legends.Add("Legends1")

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

    End Sub
    Private Sub Active_Orders()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim Curyear As String = ""
        Dim Prevyear As String = ""
        Dim Sno As Integer = 0

        Curyear = Current_Date.Year
        Prevyear = Current_Date.Year - 1

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", Current_Date)
        cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)



        Da = New SqlClient.SqlDataAdapter("select a.Order_Selection_Code ,b.Sales_Order_Date from Order_Selection_Code_Head a LEFT OUTER JOIN Sales_Order_Head b ON a.Reference_Code = b.Sales_Order_Code where b.Order_Close = 0", con)
        Dt = New DataTable
        Da.Fill(Dt)

        Dgv_ActiveOrders.Rows.Clear()

        SNo = 0
        If Dt.Rows.Count > 0 Then
            For i = 0 To Dt.Rows.Count - 1
                n = Dgv_ActiveOrders.Rows.Add()
                Sno = Sno + 1
                If IsDBNull(Trim(Dt.Rows(i).Item("Sales_Order_Date").ToString)) = False Then
                    Dgv_ActiveOrders.Rows(n).Cells(0).Value = Format(Convert.ToDateTime(Dt.Rows(i).Item("Sales_Order_Date")), "dd/MM/yyyy")
                End If
                If IsDBNull(Trim(Dt.Rows(i).Item("Order_Selection_Code").ToString)) = False Then
                    Dgv_ActiveOrders.Rows(n).Cells(1).Value = Trim(Dt.Rows(i).Item("Order_Selection_Code").ToString)
                End If
             
            Next
        End If



        Dt.Clear()
        Da.Dispose()


    End Sub
    Private Sub OrderWise_ProftAndLoss(ByVal OrderNo As String)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim Curyear As String = ""
        Dim Prevyear As String = ""
        Dim Sales As Double = 0

        lbl_OrderCost.Text = ""
        lbl_TotalPurchase.Text = ""
        lbl_TotalExpenses.Text = ""
        lbl_TotalReceipt.Text = ""
        lbl_TotalBalance.Text = ""
        lbl_ProftAndLoss.Text = ""


        '---Order Cost
        Da = New SqlClient.SqlDataAdapter("Select  b.Net_Amount  from Order_Selection_Code_Head a LEFT OUTER JOIN Sales_Order_Head B ON A.Reference_Code = B.Sales_Order_Code  where a.Order_Selection_Code = '" & Trim(OrderNo) & "' ", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            lbl_OrderCost.Text = Format(Val(Dt.Rows(0).Item("Net_Amount").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        '---Purchase
        Da = New SqlClient.SqlDataAdapter("Select  a.Net_Amount  from Purchase_Head a  where a.Sales_Order_Selection_Code = '" & Trim(OrderNo) & "'", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            lbl_TotalPurchase.Text = Format(Val(Dt.Rows(0).Item("Net_Amount").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        '---Purchase Return
        Da = New SqlClient.SqlDataAdapter("Select  a.Net_Amount  from Purchase_Return_Head a  where a.Sales_Order_Selection_Code = '" & Trim(OrderNo) & "'", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            lbl_TotalPurchase.Text = Format(Val(lbl_TotalPurchase.Text) - Val(Dt.Rows(0).Item("Net_Amount").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        '---Expense for Order
        Da = New SqlClient.SqlDataAdapter("Select  abs(a.Amount) as amt  from Voucher_Order_Details a  where a.Amount <> 0 and  a.Amount < 0 and  a.Sales_Order_Selection_Code = '" & Trim(OrderNo) & "'", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            lbl_TotalExpenses.Text = Format(Val(Dt.Rows(0).Item("amt").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        '---Expenses FOR ENQUIRY
        Da = New SqlClient.SqlDataAdapter(" Select sum(abs(a.Amount))  as amt   from Voucher_Order_Details a    LEFT OUTER JOIN Sales_Quotation_Head sQ ON a.Sales_Order_Selection_Code = sQ.Enquiry_No LEFT OUTER JOIN Sales_Order_Head SO ON SO.Quotation_No = SQ.Sales_Quotation_No LEFT OUTER JOIN  Order_Selection_Code_Head OSH ON so.Sales_Order_Code = OSH.REFERENCE_CODE where   a.Amount < 0  and  OSH.Order_Selection_Code ='" & Trim(OrderNo) & "'  having sum(a.Amount) <> 0  ", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            lbl_TotalExpenses.Text = Format(Val(lbl_TotalExpenses.Text) + Val(Dt.Rows(0).Item("amt").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()



        '---Receipt for Order
        Da = New SqlClient.SqlDataAdapter("Select  a.Amount  from Voucher_Order_Details a  where a.Amount <> 0 and a.Amount > 0 and a.Sales_Order_Selection_Code = '" & Trim(OrderNo) & "' ", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            lbl_TotalReceipt.Text = Format(Val(Dt.Rows(0).Item("Amount").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()


        '---Receipt FOR ENQUIRY
        Da = New SqlClient.SqlDataAdapter(" Select sum(abs(a.Amount))  as amt   from Voucher_Order_Details a    LEFT OUTER JOIN Sales_Quotation_Head sQ ON a.Sales_Order_Selection_Code = sQ.Enquiry_No LEFT OUTER JOIN Sales_Order_Head SO ON SO.Quotation_No = SQ.Sales_Quotation_No LEFT OUTER JOIN  Order_Selection_Code_Head OSH ON so.Sales_Order_Code = OSH.REFERENCE_CODE where   a.Amount > 0  and  OSH.Order_Selection_Code ='" & Trim(OrderNo) & "'  having sum(a.Amount) <> 0  ", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            lbl_TotalExpenses.Text = Format(Val(lbl_TotalReceipt.Text) + Val(Dt.Rows(0).Item("amt").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        '----Sales
        Da = New SqlClient.SqlDataAdapter("Select  a.Net_Amount  from Sales_Head a  where a.Sales_Order_Selection_Code = '" & Trim(OrderNo) & "'", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            Sales = Format(Val(Dt.Rows(0).Item("Net_Amount").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        '----Sales
        Da = New SqlClient.SqlDataAdapter("Select  a.Net_Amount  from SalesReturn_Head a  where a.Sales_Order_Selection_Code = '" & Trim(OrderNo) & "'", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            Sales = Format(Val(Sales) - Val(Dt.Rows(0).Item("Net_Amount").ToString), "############0.00")
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()


        lbl_TotalPayment.Text = Format(Val(lbl_TotalPurchase.Text) + Val(lbl_TotalExpenses.Text), "###########0.00")

        lbl_ProftAndLoss.Text = Format(Val(lbl_OrderCost.Text) - Val(lbl_TotalPayment.Text), "############0.00")

        lbl_TotalBalance.Text = Format(Val(lbl_OrderCost.Text) - Val(lbl_TotalReceipt.Text), "############0.00")


        If Val(lbl_OrderCost.Text) = 0 Then lbl_OrderCost.Text = ""
        If Val(lbl_TotalPurchase.Text) = 0 Then lbl_TotalPurchase.Text = ""
        If Val(lbl_TotalExpenses.Text) = 0 Then lbl_TotalExpenses.Text = ""
        If Val(lbl_TotalReceipt.Text) = 0 Then lbl_TotalReceipt.Text = ""
        If Val(lbl_TotalBalance.Text) = 0 Then lbl_TotalBalance.Text = ""
        If Val(lbl_ProftAndLoss.Text) = 0 Then lbl_ProftAndLoss.Text = ""


        If Val(lbl_ProftAndLoss.Text) > 0 Then
            lbl_ProftAndLoss.BackColor = Color.SpringGreen
            lbl_ProftAndLoss_Caption.BackColor = Color.LightGreen
            lbl_ProftAndLoss_Caption.Text = "Profit"
        ElseIf Val(lbl_ProftAndLoss.Text) < 0 Then
            lbl_ProftAndLoss.BackColor = Color.Salmon
            lbl_ProftAndLoss_Caption.BackColor = Color.LightSalmon
            lbl_ProftAndLoss_Caption.Text = "Loss"
        Else
            lbl_ProftAndLoss.BackColor = Color.White
            lbl_ProftAndLoss_Caption.BackColor = Color.FromArgb(199, 252, 232)
            lbl_ProftAndLoss_Caption.Text = "Profit/Loss"
        End If

        Chart_OrderWise.Series.Clear()


        Chart_OrderWise.Series.Add("Purchase")
        Chart_OrderWise.Series.Add("Sales")
        Chart_OrderWise.Series.Add("Expense")
        Chart_OrderWise.Series.Add("Income")
        Chart_OrderWise.Series.Add("Balance")
        Chart_OrderWise.Series.Add("Profit")
        Chart_OrderWise.Series.Add("Loss")

        Chart_OrderWise.Series("Purchase").Points.AddXY("", lbl_TotalPurchase.Text)
        Chart_OrderWise.Series("Sales").Points.AddXY("", Sales)
        Chart_OrderWise.Series("Expense").Points.AddXY("", lbl_TotalPayment.Text)
        Chart_OrderWise.Series("Income").Points.AddXY("", lbl_TotalReceipt.Text)
        Chart_OrderWise.Series("Balance").Points.AddXY("", lbl_TotalBalance.Text)
        If Val(lbl_ProftAndLoss.Text) > 0 Then
            Chart_OrderWise.Series("Profit").Points.AddXY("", lbl_ProftAndLoss.Text)
        Else
            Chart_OrderWise.Series("Loss").Points.AddXY("", Math.Abs(Val(lbl_ProftAndLoss.Text)))
        End If

        Chart_OrderWise.Titles(0).Text = "Oreder No. : " & Trim(OrderNo)
    End Sub

    Private Sub Dgv_ActiveOrders_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv_ActiveOrders.CellDoubleClick

        If Trim(Dgv_ActiveOrders.Rows(Dgv_ActiveOrders.CurrentCell.RowIndex).Cells(0).Value) <> "" Then

            OrderWise_ProftAndLoss(Trim(Dgv_ActiveOrders.Rows(Dgv_ActiveOrders.CurrentCell.RowIndex).Cells(1).Value))

            lbl_OrderNo.Text = "Order No. : " & Trim(Dgv_ActiveOrders.Rows(Dgv_ActiveOrders.CurrentCell.RowIndex).Cells(1).Value) & "    Date : " & Trim(Dgv_ActiveOrders.Rows(Dgv_ActiveOrders.CurrentCell.RowIndex).Cells(0).Value)

            pnl_OrderWiseChart.Visible = True
        End If

    End Sub

    Private Sub Dgv_ActiveOrders_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Dgv_ActiveOrders.Click

        If Trim(Dgv_ActiveOrders.Rows(Dgv_ActiveOrders.CurrentCell.RowIndex).Cells(0).Value) <> "" Then

            OrderWise_ProftAndLoss(Trim(Dgv_ActiveOrders.Rows(Dgv_ActiveOrders.CurrentCell.RowIndex).Cells(1).Value))

            lbl_OrderNo.Text = "Order No. : " & Trim(Dgv_ActiveOrders.Rows(Dgv_ActiveOrders.CurrentCell.RowIndex).Cells(1).Value) & "    Date : " & Trim(Dgv_ActiveOrders.Rows(Dgv_ActiveOrders.CurrentCell.RowIndex).Cells(0).Value)
            pnl_OrderWiseChart.Visible = True
        End If

    End Sub

    Private Function SENDOUTSTANDINGALERT(ByVal PARTYNAME As String, ByVal RECEPIENT As String, ByVal AMOUNT As String) As Long

        Dim SENDERID As String = ""
        Dim SMSLOGIN As String = ""
        Dim SMSPWD As String = ""

        If Common_Procedures.settings.CustomerCode = "1117" Then
            SENDERID = "SLDEMB"
            SMSLOGIN = "lakshmi_emb"
            SMSPWD = "Kamaraj31"

        Else

            MsgBox("PLEASE REGISTER FOR SMS ALERT SERIVCE TO USE THIS FACILITY")
            SENDOUTSTANDINGALERT = 0
            Exit Function

        End If

        SENDOUTSTANDINGALERT = 0

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim n As Integer = 0


        Da = New SqlClient.SqlDataAdapter("select name2 as LedgerName , Int1 as LedgerIdno, abs(sum(currency1)) as Amount from ReportTemp group by name1 ,name2,int1 having sum(currency1) < 0 Order by name2", con)
        Dt = New DataTable
        Da.Fill(Dt)

        Try

            Dim request As WebRequest = _
                WebRequest.Create("http://198.24.149.4/API/pushsms.aspx?loginID=" & SMSLOGIN & "&password=" & SMSPWD & _
                               "&mobile=" & RECEPIENT & "&text=DEAR M/s ." & PARTYNAME & " PLEASE CLEAR THE OUTSTANDING OF Rs." & _
                               FormatNumber(AMOUNT, 2, TriState.False, TriState.False, TriState.False) & " AT THE EARLIEST. By " & Common_Procedures.CompGroupName & "&senderid=" & SENDERID & "&route_id=2&Unicode=0")
            ' By " & Common_Procedures.CompGroupName & "
            ' If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials
            ' Get the response.
            Dim response As WebResponse = request.GetResponse()
            ' Display the status.
            'MsgBox(CType(response, HttpWebResponse).StatusDescription)
            ' Get the stream containing content returned by the server.
            Dim dataStream As Stream = response.GetResponseStream()
            ' Open the stream using a StreamReader for easy access.
            Dim reader As New StreamReader(dataStream)
            ' Read the content.
            Dim responseFromServer As String = reader.ReadToEnd()
            ' Display the content.
            'MsgBox(responseFromServer)
            ' Clean up the streams and the response.
            reader.Close()
            response.Close()

            SENDOUTSTANDINGALERT = 0
            Exit Function

        Catch ex As Exception

            SENDOUTSTANDINGALERT = Err.Number
            MsgBox(ex.Message & ". SMS SENDING FAILS .")

        End Try

    End Function

    Private Sub dgv_OverDueBills_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OverDueBills.CellContentClick
        If e.ColumnIndex = 3 Then
            If Val(dgv_OverDueBills.Rows(e.RowIndex).Cells(2).Value) <= 0 Then
                MsgBox("No Outstanding Amount")
                Exit Sub
            End If
            If Len(dgv_OverDueBills.Rows(e.RowIndex).Cells(5).Value) < 10 Then
                MsgBox("Invalid Phone Number")
                Exit Sub
            End If
            If Len(Split(dgv_OverDueBills.Rows(e.RowIndex).Cells(5).Value, ",")(0)) <> 10 Then
                MsgBox("Invalid Phone Number")
                Exit Sub
            End If
            If SENDOUTSTANDINGALERT(dgv_OverDueBills.Rows(e.RowIndex).Cells(1).Value, Split(dgv_OverDueBills.Rows(e.RowIndex).Cells(5).Value, ",")(0), _
                                 dgv_OverDueBills.Rows(e.RowIndex).Cells(2).Value) = 0 Then
                MsgBox("ALERT SMS HAS BEEN PUSHED")
            End If
        End If
    End Sub
End Class