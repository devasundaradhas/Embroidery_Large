Imports System.IO

Public Class Transfer_Master_Ledgers_From_CompanyGroup

    Private CnTo As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private CnFrm As New SqlClient.SqlConnection

    Private Sub Transfer_Master_Ledgers_From_CompanyGroup_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If txt_DbIdNo_From.Enabled And txt_DbIdNo_From.Visible Then txt_DbIdNo_From.Focus()
    End Sub

    Private Sub Transfer_Master_Ledgers_From_CompanyGroup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txt_DbIdNo_From.Text = ""
        Me.Text = "MASTERS TRANSFER"
    End Sub

    Private Sub btn_Transfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Transfer.Click
        Dim tr As SqlClient.SqlTransaction
        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As DataTable
        Dim DbFrmName As String = ""
        Dim DbFrm_ConnStr As String = ""
        Dim Nr As Long = 0

        If Val(txt_DbIdNo_From.Text) = 0 Then
            MessageBox.Show("Invalid CompanyGroup From", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_DbIdNo_From.Visible And txt_DbIdNo_From.Enabled Then txt_DbIdNo_From.Focus()
            Exit Sub
        End If

        If MessageBox.Show("All the master datas will lost " & Chr(13) & "Are you sure you want to Transfer?", "FOR MASTER TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        MDIParent1.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor

        btn_Transfer.Enabled = False
        Me.Text = ""

        CnTo.Open()

        DbFrmName = Common_Procedures.get_Company_DataBaseName(Trim(Val(txt_DbIdNo_From.Text)))

        da2 = New SqlClient.SqlDataAdapter("Select name from master..sysdatabases where name = '" & Trim(DbFrmName) & "'", CnTo)
        dt2 = New DataTable
        da2.Fill(dt2)
        Nr = 0
        If dt2.Rows.Count > 0 Then
            If IsDBNull(dt2.Rows(0).Item("name").ToString) = False Then
                Nr = 1
            End If
        End If
        dt2.Dispose()
        da2.Dispose()

        If Nr = 0 Then
            MessageBox.Show("Invalid CompanyGroup From - Does not Exists", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_DbIdNo_From.Visible And txt_DbIdNo_From.Enabled Then txt_DbIdNo_From.Focus()
            btn_Transfer.Enabled = True
            Exit Sub
        End If


        DbFrm_ConnStr = Common_Procedures.Create_Sql_ConnectionString(DbFrmName)
        CnFrm = New SqlClient.SqlConnection(DbFrm_ConnStr)

        CnFrm.Open()

        MDIParent1.vFldsChk_All_Status = True

        Me.Text = "Fields Check is Running.   Please Wait.."

        MDIParent1.mnu_Tools_FieldsCheck_Click(sender, e)

        MDIParent1.vFldsChk_All_Status = False


        tr = CnTo.BeginTransaction

        'Try


        Transfer_Table(tr, DbFrmName, "Company_Head")
        Transfer_Table(tr, DbFrmName, "AccountsGroup_Head")
        Transfer_Table(tr, DbFrmName, "Area_Head")
        Transfer_Table(tr, DbFrmName, "Ledger_Head")
        Transfer_Table(tr, DbFrmName, "Ledger_AlaisHead")
        ' Transfer_Table(tr, DbFrmName, "Ledger_Item_Head")
        ' Transfer_Table(tr, DbFrmName, "Ledger_item_Details")
        Transfer_Table(tr, DbFrmName, "Ledger_PhoneNo_Head")
        Transfer_Table(tr, DbFrmName, "Ledger_Reading_Details")

        Transfer_Table(tr, DbFrmName, "Cetegory_Head")

        Transfer_Table(tr, DbFrmName, "Colour_Head")
        Transfer_Table(tr, DbFrmName, "Design_Head")
        Transfer_Table(tr, DbFrmName, "Gender_Head")
        Transfer_Table(tr, DbFrmName, "Item_Details")
        Transfer_Table(tr, DbFrmName, "Item_ExcessShort_Head")

        Transfer_Table(tr, DbFrmName, "Item_Head")
        Transfer_Table(tr, DbFrmName, "ItemGroup_Head")
        Transfer_Table(tr, DbFrmName, "Machine_Head")
        Transfer_Table(tr, DbFrmName, "Month_Head")
        Transfer_Table(tr, DbFrmName, "Unit_Head")

        Transfer_Table(tr, DbFrmName, "Size_Head")

        Transfer_Table(tr, DbFrmName, "Sleeve_Head")
        Transfer_Table(tr, DbFrmName, "Variety_Head")

        Transfer_Table(tr, DbFrmName, "Waste_Head")
        Transfer_Table(tr, DbFrmName, "Salesman_Head")


        tr.Commit()

        Me.Text = "MASTERS TRANSFER"


        MDIParent1.Cursor = Cursors.Default
        Me.Cursor = Cursors.Default

        MessageBox.Show("All Masters Transfered Sucessfully", "FOR MASTERS TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        btn_Transfer.Enabled = True

        'Catch ex As Exception

        '    tr.Rollback()
        '    Me.Text = "MASTERS TRANSFER"
        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default
        '    btn_Transfer.Enabled = True
        '    MessageBox.Show(ex.Message, "INVALID TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally

        '    CnFrm.Close()
        '    CnTo.Close()
        '    tr.Dispose()

        '    btn_Transfer.Enabled = True
        '    Me.Text = "MASTERS TRANSFER"

        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default

        'End Try

    End Sub

    Private Sub Transfer_Table(ByVal sqltr As SqlClient.SqlTransaction, ByVal DbFrmName As String, ByVal TblName As String)
        Dim CmdTo As New SqlClient.SqlCommand

        Me.Text = TblName

        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Drop table " & TblName
        CmdTo.ExecuteNonQuery()

        CmdTo.CommandText = "Select * into " & Trim(Common_Procedures.DataBaseName) & ".." & TblName & " from " & Trim(DbFrmName) & ".." & TblName
        CmdTo.ExecuteNonQuery()

    End Sub

    Private Sub Ledger_Opening_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Cmp_FromDt As Date, OpDt As Date
        Dim OpDateCondt As String
        Dim OpBal As Single = 0
        Dim I As Integer, J As Integer
        Dim Sno As Integer = 0
        Dim CompIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim L_Id As Integer = 0
        Dim NewCode As String = ""
        Dim OpYrCode As String = ""
        Dim Pk_Condition As String = ""
        Dim vou_bil_no As String
        Dim vou_bil_code As String
        Dim vAgt_ID As Integer
        Dim Bl_Amt As Single, Cr_Amt As Single, Dr_Amt As Single

        Me.Text = "Ledger Opening"
        Pk_Condition = "OPENI-"

        CmdFrm.Connection = CnFrm

        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "delete from voucher_details where Entry_Identification LIKE '" & Trim(Pk_Condition) & "%'"
        CmdTo.ExecuteNonQuery()

        CmdTo.CommandText = "delete from voucher_bill_head where Entry_Identification LIKE '" & Trim(Pk_Condition) & "%'"
        CmdTo.ExecuteNonQuery()

        Cmp_FromDt = #4/1/2015#
        OpDt = #3/31/2015#

        OpYrCode = "14-15"

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Ledger_Head a, Company_Head b where Ledger_Idno <> 0 and Company_IdNo <> 0 Order by Ledger_Idno, Company_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                CmdFrm.Parameters.Clear()
                CmdFrm.Parameters.AddWithValue("@CompFromDate", Cmp_FromDt)
                CmdFrm.Parameters.AddWithValue("@OpeningDate", OpDt)

                CmdTo.Parameters.Clear()
                CmdTo.Parameters.AddWithValue("@CompFromDate", Cmp_FromDt)
                CmdTo.Parameters.AddWithValue("@OpeningDate", OpDt)


                CompIdNo = Val(Dt1.Rows(I).Item("Company_IdNo").ToString)
                LedIdNo = Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString)

                If Val(LedIdNo) <= 20 Then
                    L_Id = LedIdNo

                Else
                    L_Id = LedIdNo + 80

                End If

                Me.Text = "Ledger Opening  -  " & LedIdNo

                NewCode = Trim(Val(CompIdNo)) & "-" & Trim(Val(L_Id)) & "/" & Trim(OpYrCode)

                OpDateCondt = ""
                If Trim(Dt1.Rows(I).Item("Parent_Code").ToString) Like "*~18~" Then
                    OpDateCondt = " a.Voucher_Date >= @CompFromDate"
                End If

                CmdFrm.CommandText = "Select sum(a.voucher_amount) from voucher_details a, company_head tz where " & OpDateCondt & IIf(OpDateCondt <> "", " and ", "") & " a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " and a.company_idno = tz.company_idno"
                Da1 = New SqlClient.SqlDataAdapter(CmdFrm)
                Dt2 = New DataTable
                Da1.Fill(Dt2)

                OpBal = 0
                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                        OpBal = Val(Dt2.Rows(0)(0).ToString)
                    End If
                End If
                Dt2.Clear()

                'CmdTo.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(CompIdNo)) & " and Ledger_IdNo = " & Str(Val(LedIdNo)) & " and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%'"
                'CmdTo.ExecuteNonQuery()

                Sno = 0

                If Val(OpBal) <> 0 Then

                    Sno = Sno + 1


                    CmdTo.CommandText = "Insert into Voucher_Details (        Voucher_Code    ,       For_OrderByCode ,         Company_IdNo      ,           Voucher_No     ,         For_OrderBy   , Voucher_Type, Voucher_Date,         Sl_No        ,           Ledger_IdNo ,    Voucher_Amount      , Narration,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                        "            Values          ( '" & Trim(NewCode) & "', " & Str(Val(L_Id)) & ", " & Str(Val(CompIdNo)) & ", '" & Trim(Val(L_Id)) & "', " & Str(Val(L_Id)) & ",    'Opening', @OpeningDate, " & Str(Val(Sno)) & ", " & Str(Val(L_Id)) & ", " & Str(Val(OpBal)) & ", 'Opening', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                    CmdTo.ExecuteNonQuery()


                    If Trim(UCase(Dt1.Rows(I).Item("Bill_Maintenance").ToString)) = "BILL TO BILL CLEAR" Or Trim(UCase(Dt1.Rows(I).Item("Bill_Maintenance").ToString)) = "BILL TO BILL" Then

                        'CmdFrm.CommandText = "truncate table reporttempsub"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "insert into reporttempsub ( int1, int2, name1, currency1 ) Select tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_No, sum(a.Amount) from voucher_bill_details a INNER JOIN company_head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " group by tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_No"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "insert into reporttempsub ( int1, int2, name1, currency1 ) Select tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_No, 0 from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " group by tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_No"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "truncate table Entry_Temp"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "insert into Entry_Temp ( SmallInt_1, SmallInt_2, Text_1, Amount_1 ) Select int1, int2, name1, sum(currency1) from reporttempsub group by int1, int2, name1"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "truncate table ReportTemp"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "insert into ReportTemp ( Int1,   Int2       ,   Int3      ,   name3        ,   Date1            ,                     Amount_1                                                                                                              ,                                          currency2                                                                                         ,       currency3                                                                            ,   name4      ) " & _
                        '                        " Select     a.Company_Idno, a.Ledger_Idno, a.Agent_Idno, a.party_bill_no, a.voucher_bill_date, (case when lower(a.crdr_type) = 'cr' then a.bill_amount else (case when b.Amount_1 is null then 0 else b.Amount_1 end ) end) as cr_amount, (case when lower(a.crdr_type) = 'dr' then a.bill_amount else (case when b.Amount_1 is null then 0 else b.Amount_1 end ) end) as db_amount, abs(a.bill_amount - (case when b.Amount_1 is null then 0 else b.Amount_1 end)) as balance, a.crdr_type from voucher_bill_head a, Entry_Temp b, company_head tz, ledger_head tp Where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " and (a.bill_amount- (case when b.Amount_1 is null then 0 else b.Amount_1 end)) <> 0 and a.Voucher_Bill_No = b.text_1 and a.company_idno = b.SmallInt_1 and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno order by a.voucher_bill_date, a.For_OrderBy, a.Voucher_Bill_No"
                        'CmdFrm.ExecuteNonQuery()

                        Da1 = New SqlClient.SqlDataAdapter("Select a.* from voucher_bill_head a, company_head tz, ledger_head tp Where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " and a.bill_amount <>  0 AND a.Credit_Amount <> a.Debit_Amount and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno order by a.voucher_bill_date, a.For_OrderBy, a.Voucher_Bill_No", CnFrm)
                        'Da1 = New SqlClient.SqlDataAdapter("Select a.Company_Idno, a.Ledger_Idno, a.Agent_Idno, a.party_bill_no, a.voucher_bill_date, (case when lower(a.crdr_type) = 'cr' then a.bill_amount else (case when b.Amount_1 is null then 0 else b.Amount_1 end ) end) as cr_amount, (case when lower(a.crdr_type) = 'dr' then a.bill_amount else (case when b.Amount_1 is null then 0 else b.Amount_1 end ) end) as db_amount, abs(a.bill_amount - (case when b.Amount_1 is null then 0 else b.Amount_1 end)) as balance, a.crdr_type from voucher_bill_head a, Entry_Temp b, company_head tz, ledger_head tp Where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " and (a.bill_amount- (case when b.Amount_1 is null then 0 else b.Amount_1 end)) <> 0 and a.Voucher_Bill_No = b.text_1 and a.company_idno = b.SmallInt_1 and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno order by a.voucher_bill_date, a.For_OrderBy, a.Voucher_Bill_No", CnFrm)
                        'Da1 = New SqlClient.SqlDataAdapter("select name1, name2, name3, Date1, currency1, currency2, currency3, name4, int6, int7 from reporttemp Order by name2, date1, name3, name1", CnFrm)
                        Dt2 = New DataTable
                        Da1.Fill(Dt2)

                        If Dt2.Rows.Count > 0 Then

                            For J = 0 To Dt2.Rows.Count - 1

                                CmdTo.Parameters.Clear()
                                CmdTo.Parameters.AddWithValue("@VouBillDate", CDate(Dt2.Rows(J).Item("Voucher_Bill_Date").ToString))

                                vAgt_ID = Val(Dt2.Rows(J).Item("Agent_Idno").ToString)
                                If Val(vAgt_ID) > 20 Then
                                    vAgt_ID = vAgt_ID + 80
                                End If

                                Bl_Amt = Math.Abs(Val(Dt2.Rows(J).Item("Credit_Amount").ToString) - Val(Dt2.Rows(J).Item("Debit_Amount").ToString))
                                Cr_Amt = 0
                                Dr_Amt = 0
                                If Trim(UCase(Dt2.Rows(J).Item("CrDr_Type").ToString)) = "CR" Then
                                    Cr_Amt = Bl_Amt
                                Else
                                    Dr_Amt = Bl_Amt
                                End If

                                vou_bil_no = Common_Procedures.get_MaxCode(CnTo, "Voucher_Bill_Head", "Voucher_Bill_Code", "For_OrderBy", "", Val(CompIdNo), OpYrCode, sqltr)
                                vou_bil_code = Trim(Val(CompIdNo)) & "-" & Trim(vou_bil_no) & "/" & Trim(OpYrCode)

                                CmdTo.CommandText = "Insert into voucher_bill_head ( voucher_bill_code ,         company_idno      ,        voucher_bill_no    ,            for_orderby      , voucher_bill_date,         ledger_idno   ,                             party_bill_no                 ,            agent_idno    ,         bill_amount     ,         Credit_Amount   ,         Debit_Amount    ,                                   crdr_type                  ,        entry_identification                  ) " _
                                                        & "      Values  ( '" & Trim(vou_bil_code) & "', " & Str(Val(CompIdNo)) & ", '" & Trim(vou_bil_no) & "', " & Str(Val(vou_bil_no)) & ",     @VouBillDate , " & Str(Val(L_Id)) & ", '" & Trim(Dt2.Rows(J).Item("Party_Bill_No").ToString) & "', " & Str(Val(vAgt_ID)) & ", " & Str(Val(Bl_Amt)) & ", " & Str(Val(Cr_Amt)) & ", " & Str(Val(Dr_Amt)) & ", '" & Trim(UCase(Dt2.Rows(J).Item("CrDr_Type").ToString)) & "', '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                                CmdTo.ExecuteNonQuery()

                            Next

                        End If

                    End If

                End If

            Next I

        End If

        Me.Text = ""

    End Sub

    Private Sub txt_DbIdNo_From_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DbIdNo_From.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub Fields_Check()
        On Error Resume Next



    End Sub

End Class