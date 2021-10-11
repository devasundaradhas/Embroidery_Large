Public Class Advance_Bill_Adjustment
    Private Cn2 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private tr2 As SqlClient.SqlTransaction = Nothing
    Private v_b_no As String
    Private p_b_no As String
    Private cd_type As String
    Private ent_id As String
    Private v_b_date As Date
    Private Led_ID As Integer
    Private agt_id As Integer
    Private CompIdno As Integer
    Private bl_amt As Single = 0
    Private Posting_Column As String
    Private Adjust_Column As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private Sub Advance_Bill_Adjustment_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        With dgv_BillDetails
            If .Visible = True And .Enabled = True Then
                If .Rows.Count = 0 Then .Rows.Add()
                .Focus()
                .CurrentCell = .Rows(0).Cells(5)
            End If
            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(5)
            End If
        End With
    End Sub

    Private Sub Advance_Bill_Adjustment_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                btn_close_Click(sender, e)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_BillDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_BillDetails.Name Then
                dgv1 = dgv_BillDetails

            ElseIf dgv_BillDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BillDetails

            Else
                dgv1 = dgv_BillDetails

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= 5 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            Close_AdvanceBill_Adjustment_Details()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            Close_AdvanceBill_Adjustment_Details()

                        ElseIf .CurrentCell.ColumnIndex <= 4 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 5 Then
                        If .CurrentCell.RowIndex = 0 Then
                            'Close_AdvanceBill_Adjustment_Details()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)

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

    End Function

    Public Sub Bills_Display(ByVal Cn1 As SqlClient.SqlConnection, ByVal Comp_IdNo As Integer, ByVal vou_bill_code As String, ByVal vou_bill_date As Date, ByVal c_ledidno As Integer, ByVal par_bill_no As String, ByVal Agt_Idno As Integer, ByVal crdr_type As String, ByVal c_amt As Single, ByVal ent_idn As String, ByRef Tot_AdvBil_Amt As Single, Optional ByRef SqlTr As SqlClient.SqlTransaction = Nothing)
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim amt As Single = 0
        Dim I As Integer = 0
        Dim n As Integer = 0
        Dim SNo As Integer = 0

        Cmd.Connection = Cn1
        If IsNothing(SqlTr) = False Then
            Cmd.Transaction = SqlTr
        End If

        dgv_BillDetails.Rows.Clear()
        Posting_Column = IIf(Trim(UCase(crdr_type)) = "CR", "CREDIT", "DEBIT")
        Adjust_Column = IIf(Trim(UCase(crdr_type)) = "CR", "DEBIT", "CREDIT")
        CompIdno = Comp_IdNo

        dgv_BillDetails.Columns(4).HeaderText = Trim(UCase(Adjust_Column)) & "  AMOUNT"
        dgv_BillDetails.Columns(5).HeaderText = Trim(UCase(Posting_Column)) & "  AMOUNT"

        Cmd.CommandText = "truncate table ReportTempSub"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into ReportTempSub ( Name1, Date1, Name2, Currency1, Currency2, Name3 ) Select a.party_bill_no, a.voucher_bill_date, b.ledger_name, abs(a.credit_amount-a.debit_amount) as amount, 0, a.voucher_bill_code from voucher_bill_head a LEFT OUTER JOIN ledger_head b ON a.agent_idno = b.ledger_idno WHERE a.company_idno = " & Str(CompIdno) & " and a.ledger_idno = " & Str(c_ledidno) & " and " & Trim(Adjust_Column) & "_amount > " & Trim(Posting_Column) & "_amount and a.Bill_Amount <> 0"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into ReportTempSub ( Name1, Date1, Name2, Currency1, Currency2, Name3 ) Select b.party_bill_no, b.voucher_bill_date, c.ledger_name, a.amount, a.amount, b.voucher_bill_code from voucher_bill_details a INNER JOIN voucher_bill_head b ON a.voucher_bill_code = b.voucher_bill_code and a.company_idno = b.company_idno LEFT OUTER JOIN ledger_head c ON b.agent_idno = c.ledger_idno where a.company_idno = " & Str(CompIdno) & " and a.ledger_idno = " & Str(c_ledidno) & " and a.entry_identification = '" & Trim(ent_idn) & "' and a.voucher_bill_code <> '" & Trim(vou_bill_code) & "'"
        Cmd.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("Select Name1, Date1, Name2, sum(Currency1) as amount_1, sum(Currency2) as amount_2, Name3 from ReportTempSub group by Name1, date1, Name2, Name3 order by date1, name1, name2", Cn1)
        'Da1 = New SqlClient.SqlDataAdapter("Select Name1, Date1, Name2, sum(Currency1) as amount_1, sum(Currency2) as amount_2, Name3 from ReportTempSub group by Name1, date1, Name2, Name3 order by sum(Currency1) desc, date1", Cn1)
        If IsNothing(SqlTr) = False Then
            Da1.SelectCommand.Transaction = SqlTr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        With dgv_BillDetails

            SNo = 0
            .Rows.Clear()
            Tot_AdvBil_Amt = 0

            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1
                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Name1").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(I).Item("Date1").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Name2").ToString
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(I).Item("Amount_1").ToString), "########0.00")
                    If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(I).Item("Amount_2").ToString), "########0.00")
                    If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Dt1.Rows(I).Item("Name3").ToString

                    Tot_AdvBil_Amt = Tot_AdvBil_Amt + Val(Dt1.Rows(I).Item("Amount_1").ToString)

                Next

            End If

            If .Rows.Count = 0 Then .Rows.Add()

            .Focus()
            .CurrentCell = .Rows(0).Cells(5)
            .CurrentCell.Selected = True

        End With
        Dt1.Clear()

        tr2 = SqlTr
        Cn2 = Cn1
        v_b_no = vou_bill_code
        v_b_date = vou_bill_date
        Led_ID = c_ledidno
        agt_id = Agt_Idno
        p_b_no = par_bill_no
        cd_type = crdr_type
        ent_id = ent_idn
        bl_amt = c_amt

        Da1 = New SqlClient.SqlDataAdapter("Select sum(amount) from voucher_bill_details where company_idno = " & Str(CompIdno) & " and voucher_bill_code = '" & Trim(vou_bill_code) & "' and entry_identification <> '" & Trim(ent_idn) & "'", Cn1)
        If IsNothing(SqlTr) = False Then
            Da1.SelectCommand.Transaction = SqlTr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then bl_amt = bl_amt - Val(Dt1.Rows(0)(0).ToString)
        End If
        Dt1.Clear()

        Cmd.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_AdvanceBill_Adjustment_Details()
    End Sub

    Private Sub Close_AdvanceBill_Adjustment_Details()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim Nr As Long = 0
        Dim i As Integer = 0
        Dim amt As Single = 0
        Dim TtAdj_Amt As Single = 0

        With dgv_BillDetails

            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(5).Value) > Val(.Rows(i).Cells(4).Value) And Val(.Rows(i).Cells(4).Value) <> 0 Then
                    MessageBox.Show("Amount Exceed", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If .Enabled And .Visible Then
                        .Focus()
                        .CurrentCell = .Rows(i).Cells(5)
                    End If
                    Exit Sub
                End If
            Next i

            Cmd.Connection = Cn2
            If IsNothing(tr2) = False Then
                Cmd.Transaction = tr2
            End If

            Cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(CompIdno) & " and b.entry_identification = '" & Trim(ent_id) & "' and b.crdr_type = 'CR' and a.voucher_bill_code = b.voucher_bill_code and a.company_idno = b.company_idno"
            Nr = Cmd.ExecuteNonQuery()

            Cmd.CommandText = "update voucher_bill_head set debit_amount = a.debit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.company_idno = " & Str(CompIdno) & " and b.entry_identification = '" & Trim(ent_id) & "' and b.crdr_type = 'DR' and a.voucher_bill_code = b.voucher_bill_code and a.company_idno = b.company_idno"
            Nr = Cmd.ExecuteNonQuery()

            Cmd.CommandText = "delete from voucher_bill_details where company_idno = " & Str(CompIdno) & " and entry_identification = '" & Trim(ent_id) & "'"
            Nr = Cmd.ExecuteNonQuery()

            Common_Procedures.BillAdj_Amt = 0
            Err.Clear()
            TtAdj_Amt = 0

            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(5).Value) > 0 And Val(.Rows(i).Cells(4).Value) > 0 Then
                    TtAdj_Amt = TtAdj_Amt + Val(.Rows(i).Cells(5).Value)
                End If
            Next i

            If TtAdj_Amt > bl_amt Then
                MessageBox.Show("Total Adjustment Amount Exceeds the Bill Amount", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(5).Value) > 0 And Val(.Rows(i).Cells(4).Value) > 0 Then
                    amt = Val(.Rows(i).Cells(5).Value)
                    If amt > 0 Then

                        Cmd.Parameters.Clear()
                        Cmd.Parameters.AddWithValue("@VouchBillDate", v_b_date)

                        Cmd.CommandText = "Insert into voucher_bill_details (   Voucher_Bill_Code         ,           Company_Idno    , Voucher_Bill_Date,        Ledger_Idno      ,  entry_identification ,            Amount    ,                                      CrDr_Type                ) " & _
                                                        " Values ( '" & Trim(.Rows(i).Cells(6).Value) & "', " & Str(Val(CompIdno)) & ",  @VouchBillDate  , " & Str(Val(Led_ID)) & ", '" & Trim(ent_id) & "', " & Str(Val(amt)) & ", '" & Trim(UCase(cd_type)) & "' ) "
                        Cmd.ExecuteNonQuery()


                        Nr = 0
                        Cmd.CommandText = "update voucher_bill_head set " & Trim(Posting_Column) & "_amount = " & Trim(Posting_Column) & "_amount + " & Str(amt) & " where company_idno = " & Str(CompIdno) & " and ledger_idno = " & Str(Led_ID) & " and voucher_bill_code = '" & Trim(.Rows(i).Cells(6).Value) & "' and crdr_type = '" & Trim(Microsoft.VisualBasic.Left(Adjust_Column, 1)) & "R'"
                        Nr = Cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            Err.Description = "Error"
                            Exit Sub
                        End If

                    End If
                End If
            Next i

            Common_Procedures.BillAdj_Amt = Val(TtAdj_Amt)

        End With

        Cmd.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub dgv_BillDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellValueChanged
        On Error Resume Next
        With dgv_BillDetails
            If e.ColumnIndex = 5 Or e.ColumnIndex = 4 Then
                Total_Calculation()
            End If

        End With
    End Sub
    Private Sub Total_Calculation()
        Dim TtQty As Single
        Dim TtMtrs As Single
        Dim i As Integer

        TtQty = 0
        TtMtrs = 0

        For i = 0 To dgv_BillDetails.Rows.Count - 1
            If Val(dgv_BillDetails.Rows(i).Cells(4).Value) <> 0 Or Val(dgv_BillDetails.Rows(i).Cells(5).Value) <> 0 Then
                TtQty = TtQty + Val(dgv_BillDetails.Rows(i).Cells(4).Value)
                TtMtrs = TtMtrs + Val(dgv_BillDetails.Rows(i).Cells(5).Value)
            End If
        Next

        If dgv_Billdetails_total.Rows.Count <= 0 Then dgv_Billdetails_total.Rows.Add()
        dgv_Billdetails_total.Rows(0).Cells(4).Value = Format(Val(TtQty), "#########0.00")
        dgv_Billdetails_total.Rows(0).Cells(5).Value = Format(Val(TtMtrs), "#########0.00")

    End Sub
    Private Sub dgv_BillDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BillDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_BillDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_BillDetails.EditingControl.BackColor = Color.Lime
        dgv_BillDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_BillDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then

                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        End If
                    End If

                End If
            End With

        Catch ex As Exception
            '------
        End Try

    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_BillDetails
                If .Rows.Count > 0 Then
                    If .CurrentCell.RowIndex >= 0 And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                    End If
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Advance_Bill_Adjustment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class