Public Class PayRoll_Employee_Salary_Advance_Payment

    Implements Interface_MDIActions

    Public Advance_Opening_Entry_Status As Boolean = False

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition1 As String = "ESLPY-"
    Private Pk_Condition2 As String = "EADPY-"
    Private Pk_Condition3 As String = "ESAPY-"

    Private Pk_OldCondition1 As String = "EPYMT-"
    Private Pk_OldCondition2 As String = "EAPMT-"
    Private Pk_OldCondition3 As String = "ESAPY-"

    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_PageNo As Integer
    Private FnYrCode As String = ""

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Public previlege As String

    Private Sub clear()

        Dim OpDate As Date

        New_Entry = False
        Insert_Entry = False
        pnl_filter.Visible = False
        pnl_back.Enabled = True
        lbl_VouNo.Text = ""
        lbl_VouNo.ForeColor = Color.Black
        lbl_VoucherNo.Text = ""
        lbl_VoucherNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        cbo_EmployeeName.Text = ""
        'cbo_AdvanceSalary.Text = ""
        txt_remarks.Text = ""
        cbo_CashCheque.Text = "CASH"
        cbo_DebitAccount.Text = Common_Procedures.Ledger_IdNoToName(con, 1)
        txt_Amount.Text = ""
        txt_ExistingEMI.Text = "0.00"
        txt_ExistingLoan.Text = "0.00"
        txt_CurrentLoan.Text = "0.00"
        txt_NewEMI.Text = "0.00"

        If Advance_Opening_Entry_Status = True Then
            OpDate = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), Microsoft.VisualBasic.DateAndTime.Month(Common_Procedures.Company_FromDate), Microsoft.VisualBasic.DateAndTime.Day(Common_Procedures.Company_FromDate))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)
            dtp_Date.Text = OpDate
            dtp_Date.Enabled = False
            cbo_AdvanceSalary.Text = "ADVANCE"
            cbo_AdvanceSalary.Enabled = False
            cbo_CashCheque.Enabled = False
            cbo_DebitAccount.Enabled = False
        End If

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

        'If Me.ActiveControl.Name <> cbo_ItemName.Name Then
        '    cbo_ItemName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_PackingType.Name Then
        '    cbo_PackingType.Visible = False
        'End If


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

    Private Sub move_record(ByVal no As String)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim EmpIdNo As String

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(FnYrCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Employee_Name  from PayRoll_Employee_Payment_Head a LEFT OUTER JOIN PayRoll_Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  where a.Employee_Payment_Code = '" & Trim(NewCode) & "' and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_VouNo.Text = dt1.Rows(0).Item("Employee_Payment_No").ToString
                lbl_VoucherNo.Text = dt1.Rows(0).Item("Voucher_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Employee_Payment_Date").ToString
                cbo_EmployeeName.Text = dt1.Rows(0).Item("Employee_Name").ToString
                cbo_CashCheque.Text = dt1.Rows(0).Item("Cash_Cheque").ToString
                cbo_DebitAccount.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DebitAc_IdNo").ToString))
                cbo_AdvanceSalary.Text = dt1.Rows(0).Item("Advance_Salary").ToString
                txt_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "#########0.00")
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                EmpIdNo = dt1.Rows(0).Item("Employee_IdNo")
                dt1.Rows.Clear()

                'da1 = New SqlClient.SqlDataAdapter("Select * from Loan_EMI_Settings where Employee_IdNo = " & EmpIdNo.ToString.ToString, con)
                'dt1 = New DataTable
                'da1.Fill(dt1)

                'If dt1.Rows.Count > 0 Then
                '    If Not IsDBNull(dt1.Rows(0).Item(0)) Then
                '        txt_ExistingEMI.Text = FormatNumber(dt1.Rows(0).Item(1), 2, TriState.False, TriState.False, TriState.False)
                '    End If
                'End If

                'dt1.Rows.Clear()

                Display_Loan_Status()

            Else

                new_record()

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("D") Then
            MsgBox("THIS USER DOES Not HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Advance_Opening_Entry_Status = True Then
        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Advance_Opening, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Advance_Opening, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES Not DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        'ElseIf Trim(Common_Procedures.AdvanceType) = "SALARY" Then
        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Salary_Payment_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Salary_Payment_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES Not DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        'ElseIf Trim(Common_Procedures.AdvanceType) = "ADVANCE" Then
        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Advance_Payment_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Advance_Payment_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES Not DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        'ElseIf Trim(Common_Procedures.AdvanceType) = "SALARYADVANCE" Then
        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Salary_Advance_Payment_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Salary_Advance_Payment_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES Not DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        'End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This Is New Entry", "DOES Not DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(FnYrCode)

            cmd.Connection = con
            cmd.Transaction = tr

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition3) & Trim(NewCode), tr)

            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_OldCondition1) & Trim(NewCode), tr)
            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_OldCondition2) & Trim(NewCode), tr)
            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_OldCondition3) & Trim(NewCode), tr)

            cmd.CommandText = "delete from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " And Employee_Payment_Code = '" & Trim(NewCode) & "' and  Voucher_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or  AccountsGroup_IdNo  = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_EmployeeFilter.DataSource = dt1
            cbo_EmployeeFilter.DisplayMember = "Ledger_DisplayName"

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""
            cbo_EmployeeFilter.SelectedIndex = -1
            dgv_filter.Rows.Clear()

            da.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

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

            inpno = InputBox("Enter New Voucher No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(FnYrCode)

            cmd.Connection = con
            cmd.CommandText = "select Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Voucher_No = '" & Trim(inpno) & "' and Employee_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'"
            'cmd.CommandText = "select Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Voucher_No = '" & Trim(inpno) & "' and Employee_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'"
            ''cmd.CommandText = "select Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Employee_Payment_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Voucher No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_VoucherNo.Text = Trim(UCase(inpno))

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

            cmd.CommandText = "select top 1 Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' Order by For_OrderByVoucher, Employee_Payment_No, for_Orderby"
            'cmd.CommandText = "select top 1 Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' Order by for_Orderby, Employee_Payment_No"
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

            If Val(movno) <> 0 Then
                ' movno = Common_Procedures.OrderBy_ValueToCode(Val(movno))
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

            cmd.CommandText = "select top 1 Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' Order by For_OrderByVoucher desc, Employee_Payment_No desc, For_OrderBy desc"
            'cmd.CommandText = "select top 1 Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' Order by for_Orderby desc, Employee_Payment_No desc"
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

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VoucherNo.Text))

            cmd.Connection = con

            cmd.CommandText = "select top 1 Employee_Payment_No from PayRoll_Employee_Payment_Head where For_OrderByVoucher > " & Str(OrdByNo) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' Order by For_OrderByVoucher, Employee_Payment_No, For_OrderBy"
            'cmd.CommandText = "select top 1 Employee_Payment_No from PayRoll_Employee_Payment_Head where for_orderby > " & Str(OrdByNo) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' Order by for_Orderby,Employee_Payment_No"
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

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VoucherNo.Text))
            'OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_VouNo.Text))

            cmd.Connection = con

            cmd.CommandText = "select top 1 Employee_Payment_No from PayRoll_Employee_Payment_Head where For_OrderByVoucher < " & Str(OrdByNo) & "  and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' Order by For_OrderByVoucher desc, Employee_Payment_No desc, for_Orderby desc"
            'cmd.CommandText = "select top 1 Employee_Payment_No from PayRoll_Employee_Payment_Head where for_orderby < " & Str(OrdByNo) & "  and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' Order by for_Orderby desc,Employee_Payment_No desc"
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

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("A") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Dim dt2 As New DataTable
        Dim NewCode As Integer = 0
        Dim NewNo As Integer = 0

        Try
            clear()

            New_Entry = True


            da = New SqlClient.SqlDataAdapter("select max(for_orderbyVoucher) from PayRoll_Employee_Payment_Head where Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code like '%/" & Trim(FnYrCode) & "' ", con)
            dt2 = New DataTable
            da.Fill(dt2)

            NewNo = 0
            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                    NewNo = Val(dt2.Rows(0)(0).ToString)
                End If
            End If

            NewNo = NewNo + 1

            lbl_VoucherNo.Text = NewNo
            lbl_VoucherNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Employee_Payment_Code like '%/" & Trim(FnYrCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            NewID = NewID + 1

            lbl_VouNo.Text = NewID
            lbl_VouNo.ForeColor = Color.Red

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

            inpno = InputBox("Enter Voucher No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(FnYrCode)

            cmd.Connection = con

            cmd.CommandText = "select Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Voucher_No = '" & Trim(inpno) & "' and Employee_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'"
            'cmd.CommandText = "select Employee_Payment_No from PayRoll_Employee_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and Employee_Payment_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Voucher No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim entcode As String


        entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(FnYrCode)
        Try

            ' If Trim(LCase(lbl_VouType.Text)) = "purc" Or Trim(LCase(lbl_VouType.Text)) = "rcpt" Or Trim(LCase(lbl_VouType.Text)) = "csrp" Or Trim(LCase(lbl_VouType.Text)) = "crnt" Then
            Da1 = New SqlClient.SqlDataAdapter("Select a.*, c.ledger_name as Employee_name, d.ledger_name as Debitor_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4 from PayRoll_Employee_Payment_Head a, ledger_head c, ledger_head d where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and a.Employee_Payment_Code = '" & Trim(entcode) & "' and a.Employee_idno = c.ledger_idno and a.DebitAc_idno = d.ledger_idno", con)
            Da1.Fill(Dt1)
            ' Else
            'Da1 = New SqlClient.SqlDataAdapter("Select a.voucher_no, a.voucher_date, a.Total_VoucherAmount, a.Narration, c.ledger_name as to_name, d.ledger_name as by_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4 from voucher_head a, ledger_head c, ledger_head d where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Voucher_Code = '" & Trim(entcode) & "' and a.debtor_idno = c.ledger_idno and a.creditor_idno = d.ledger_idno", con)
            'Da1.Fill(Dt1)
            ' End If

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


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint

        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim entcode As String

        entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(FnYrCode)

        prn_HdDt = New DataTable
        prn_PageNo = 0


        Da1 = New SqlClient.SqlDataAdapter("Select a.*, c.ledger_name as Employee_name, d.ledger_name as Debitor_name, c.ledger_address1, c.ledger_address2, c.ledger_address3, c.ledger_address4,b.*  from PayRoll_Employee_Payment_Head a, ledger_head c, ledger_head d,Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and a.Employee_Payment_Code = '" & Trim(entcode) & "' and a.Employee_IdNo = c.ledger_idno and a.DebitAc_idno = d.ledger_idno and a.Company_IdNo = b.Company_IdNo", con)
        Da1.Fill(prn_HdDt)


        If prn_HdDt.Rows.Count <= 0 Then

            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format_1(e)

    End Sub

    Private Sub Printing_Format_Advance_1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single, W2 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PrnHeading As String = ""

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
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

        ClArr(1) = Val(550) : ClArr(2) = 100
        ClArr(3) = PageWidth - (LMargin + ClArr(1))

        'CurY = TMargin
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Employee_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Voucher No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Voucher_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Voucher Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Employee_Payment_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

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
        W2 = e.Graphics.MeasureString("Advance/Salary  : ", pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "By " & Trim(prn_HdDt.Rows(0).Item("Debitor_Name").ToString), LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Cash/Cheque", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Cheque").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Advance/Salary", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Advance_Salary").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Remarks ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 30
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(3))


        CurY = CurY + TxtHgt - 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")
        Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY



        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "checked", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Signature ", PageWidth - 20, CurY, 1, 0, pFont)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(7), LMargin, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(7), PageWidth, LnAr(2))

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        'Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single, W2 As Single
        Dim C1 As Single, C2 As Single, C3 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim PrnHeading As String = ""

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
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

        ClArr(1) = Val(450) : ClArr(2) = (PageWidth - (LMargin + ClArr(1))) / 2 : ClArr(3) = (PageWidth - (LMargin + ClArr(1))) / 2
        ClArr(4) = PageWidth - (LMargin + ClArr(1))

        'CurY = TMargin
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
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

        Common_Procedures.Print_To_PrintDocument(e, PrnHeading, LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)
        C3 = C2 + 100

        W1 = e.Graphics.MeasureString("Voucher No  : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Employee_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Voucher No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Voucher_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Voucher Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Employee_Payment_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


        CurY = CurY + 8

        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin, CurY, 2, ClArr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, " AMOUNT  ", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        CurY = CurY + 13
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(3))
        W2 = e.Graphics.MeasureString("Advance/Salary  : ", pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "By " & Trim(prn_HdDt.Rows(0).Item("Debitor_Name").ToString), LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Cash/Cheque", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Cheque").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Advance/Salary", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Advance_Salary").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Remarks ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + W2 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 30
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(3))

        CurY = CurY + TxtHgt - 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")
        Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY



        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "checked", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Signature ", PageWidth - 20, CurY, 1, 0, pFont)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(7), LMargin, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(7), PageWidth, LnAr(2))

        e.HasMorePages = False

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("A") And Not UCase(previlege).Contains("E") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        If Not New_Entry Then
            If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("E") Then
                MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
                Exit Sub
            End If
        End If

        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewCode1 As String = ""
        Dim NewNo As Long = 0
        Dim Emp_id As Integer = 0
        Dim DbtAc_id As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim OnAc_id As Integer = 0
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""


        'If Advance_Opening_Entry_Status = True Then
        '    If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Advance_Opening, New_Entry) = False Then Exit Sub
        'ElseIf Trim(Common_Procedures.AdvanceType) = "SALARY" Then
        '    If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Salary_Payment_Entry, New_Entry) = False Then Exit Sub
        'ElseIf Trim(Common_Procedures.AdvanceType) = "ADVANCE" Then
        '    If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Advance_Payment_Entry, New_Entry) = False Then Exit Sub
        'ElseIf Trim(Common_Procedures.AdvanceType) = "SALARYADVANCE" Then
        '    If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Salary_Advance_Payment_Entry, New_Entry) = False Then Exit Sub
        'End If


        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Advance_Opening_Entry_Status = False Then
            If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dtp_Date.Enabled Then dtp_Date.Focus()
                Exit Sub
            End If
        End If

        Emp_id = Common_Procedures.Employee_NameToIdNo(con, cbo_EmployeeName.Text)
        If Emp_id = 0 Then
            MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EmployeeName.Enabled Then cbo_EmployeeName.Focus()
            Exit Sub
        End If

        DbtAc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DebitAccount.Text)
        If Advance_Opening_Entry_Status = False Then
            If DbtAc_id = 0 Then
                MessageBox.Show("Invalid Debit Account", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_DebitAccount.Enabled Then cbo_DebitAccount.Focus()
                Exit Sub
            End If
        End If

        If Advance_Opening_Entry_Status = False Then
            If Trim(cbo_CashCheque.Text) <> "CASH" And Trim(cbo_CashCheque.Text) <> "CHEQUE" Then
                MessageBox.Show("Invalid Cash/Cheque", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_CashCheque.Enabled Then cbo_CashCheque.Focus()
                Exit Sub
            End If
        End If




        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(FnYrCode)

            Else

                lbl_VouNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Payment_Head", "Employee_Payment_Code", "For_OrderBy", "", Val(lbl_Company.Tag), FnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(FnYrCode)

                lbl_VoucherNo.Text = Common_Procedures.get_MaxCode(con, "PayRoll_Employee_Payment_Head", "Voucher_Code", "For_OrderByVoucher", "(Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "')", Val(lbl_Company.Tag), FnYrCode, tr)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into PayRoll_Employee_Payment_Head ( Voucher_Code    ,        Voucher_No              ,                 for_OrderByVoucher                                         ,     Employee_Payment_Code,                 Company_IdNo     ,          Employee_Payment_No  ,                               for_OrderBy                              , Employee_Payment_Date,      Employee_IdNo      ,               Cash_Cheque          ,      DebitAc_IdNo   ,               Advance_Salary          ,                 Amount           ,               Remarks            ) " &
                                  "            Values                ( '" & Trim(NewCode) & "',  '" & Trim(lbl_VoucherNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VoucherNo.Text))) & ",'" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_VouNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text))) & ",     @EntryDate       , " & Str(Val(Emp_id)) & ", '" & Trim(cbo_CashCheque.Text) & "'," & Val(DbtAc_id) & ", '" & Trim(cbo_AdvanceSalary.Text) & "', " & Str(Val(txt_Amount.Text)) & ", '" & Trim(txt_remarks.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update PayRoll_Employee_Payment_Head set Employee_Payment_Date = @EntryDate, Employee_IdNo = " & Str(Val(Emp_id)) & ", Cash_Cheque = '" & Trim(cbo_CashCheque.Text) & "', DebitAc_IdNo = " & Str(Val(DbtAc_id)) & ", Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "', Amount = " & Str(Val(txt_Amount.Text)) & ", Remarks = '" & Trim(txt_remarks.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_Payment_Code = '" & Trim(NewCode) & "'  "
                cmd.ExecuteNonQuery()

            End If

            If grp_PreviousLoanBalance.Visible Then

                cmd.CommandText = "Delete from Loan_EMI_Settings where Employee_IdNo = " & Emp_id.ToString
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into Loan_EMI_Settings (Employee_IdNo,Current_EMI) values (" & Emp_id.ToString & "," & Val(txt_NewEMI.Text).ToString & ")"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Entry_Identification = '" & Trim(Pk_Condition1) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Entry_Identification = '" & Trim(Pk_Condition1) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Entry_Identification = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Entry_Identification = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Entry_Identification = '" & Trim(Pk_Condition3) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Entry_Identification = '" & Trim(Pk_Condition3) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Advance_Opening_Entry_Status = True Then

                cmd.CommandText = "Insert into Voucher_Details (                 Voucher_Code                ,                               For_OrderByCode                          ,               Company_IdNo       ,             Voucher_No            ,                                 For_OrderBy                               , Voucher_Type, Voucher_Date, Sl_No,           Ledger_IdNo   ,                  Voucher_Amount       , Narration,                                                      Year_For_Report               ,                 Entry_Identification          ) " &
                                    "           Values         ('" & Trim(Pk_Condition1) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_VoucherNo.Text) & "', '" & Trim(Val(Common_Procedures.OrderBy_CodeToValue(lbl_VouNo.Text))) & "',   'Opng'    , @EntryDate  ,   1  , " & Str(Val(Emp_id)) & ", " & Str(-1 * Val(txt_Amount.Text)) & ", 'Opening', " & Str(Val(Microsoft.VisualBasic.Left(Common_Procedures.CompGroupFnRange, 4))) & ", '" & Trim(Pk_Condition1) & Trim(NewCode) & "' ) "
                cmd.ExecuteNonQuery()

            Else

                If Trim(UCase(cbo_AdvanceSalary.Text)) = "ADVANCE" Or Trim(UCase(cbo_AdvanceSalary.Text)) = "LOAN" Then

                    vLed_IdNos = DbtAc_id & "|" & Emp_id
                    vVou_Amts = Val(txt_Amount.Text) & "|" & -1 * (Val(txt_Amount.Text))
                    If Common_Procedures.Voucher_Updation(con, "Emp.Loan.Adv", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_VoucherNo.Text), dtp_Date.Value.Date, "Vou.No : " & Trim(lbl_VoucherNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                        Exit Sub
                    End If

                ElseIf Trim(UCase(cbo_AdvanceSalary.Text)) = "SALARY ADVANCE" Or Trim(UCase(cbo_AdvanceSalary.Text)) = "SALARYADVANCE" Then

                    vLed_IdNos = DbtAc_id & "|" & Emp_id
                    vVou_Amts = Val(txt_Amount.Text) & "|" & -1 * (Val(txt_Amount.Text))
                    If Common_Procedures.Voucher_Updation(con, "Emp.Sal.Adv", Val(lbl_Company.Tag), Trim(Pk_Condition3) & Trim(NewCode), Trim(lbl_VoucherNo.Text), dtp_Date.Value.Date, "Vou.No : " & Trim(lbl_VoucherNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                        Exit Sub
                    End If

                Else

                    vLed_IdNos = DbtAc_id & "|" & Emp_id
                    vVou_Amts = Val(txt_Amount.Text) & "|" & -1 * (Val(txt_Amount.Text))
                    If Common_Procedures.Voucher_Updation(con, "Emp.Sal.Pymt", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_VoucherNo.Text), dtp_Date.Value.Date, "Vou.No : " & Trim(lbl_VoucherNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                        Exit Sub
                    End If

                End If

            End If

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_VouNo.Text)
                End If
            Else
                move_record(lbl_VouNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub


    Private Sub cbo_EmployeeName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EmployeeName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_EmployeeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EmployeeName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EmployeeName, dtp_Date, cbo_CashCheque, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_EmployeeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EmployeeName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EmployeeName, cbo_CashCheque, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_EmployeeName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EmployeeName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EmployeeName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_closefilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Emp_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Emp_IdNo = 0
            ' Itm_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Employee_Payment_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Employee_Payment_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Employee_Payment _Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_EmployeeFilter.Text) <> "" Then
                Emp_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_EmployeeFilter.Text)
            End If

            If Val(Emp_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Employee_Idno = " & Str(Val(Emp_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as Employee_Name from PayRoll_Employee_Payment_Head a INNER JOIN Ledger_Head b ON a.Employee_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Advance_Salary = '" & Trim(cbo_AdvanceSalary.Text) & "' and a.Employee_Payment_Code LIKE '%/" & Trim(FnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Employee_Payment_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()
                    dgv_filter.Rows(n).Cells(0).Value = " " & dt2.Rows(i).Item("Employee_Payment_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = " " & dt2.Rows(i).Item("Voucher_No").ToString
                    dgv_filter.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Employee_Payment_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Employee_Name").ToString
                    dgv_filter.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub


    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String = ""

        Try

            If dgv_filter.Rows.Count > 0 Then

                movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

                If Val(movno) <> 0 Then

                    Filter_Status = True
                    move_record(movno)
                    pnl_back.Enabled = True
                    pnl_filter.Visible = False

                End If

            End If

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN OPEN FILTER.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_AdvanceSalary_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AdvanceSalary.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AdvanceSalary, cbo_DebitAccount, txt_Amount, "", "", "", "")
    End Sub

    Private Sub cbo_AdvanceSalary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AdvanceSalary.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AdvanceSalary, txt_Amount, "", "", "", "")
    End Sub

    Private Sub cbo_DebitAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DebitAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DebitAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DebitAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DebitAccount, txt_Amount, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DebitAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DebitAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DebitAccount, cbo_CashCheque, txt_Amount, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_CashCheque_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CashCheque.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CashCheque, cbo_DebitAccount, "", "", "", "")
    End Sub

    Private Sub cbo_CashCheque_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CashCheque.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CashCheque, cbo_EmployeeName, cbo_DebitAccount, "", "", "", "")
    End Sub

    Private Sub txt_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub


    Private Sub Employee_Payment_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EmployeeName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EmployeeName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DebitAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DebitAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Employee_Payment_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub YarnDelivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
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

    Private Sub Employee_Payment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        cbo_AdvanceSalary.Text = Trim(Common_Procedures.AdvanceType)

        If Advance_Opening_Entry_Status = True Then
            lbl_Heading.Text = "EMPLOYEE ADVANCE OPENING"

            FnYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            FnYrCode = Trim(Mid(Val(FnYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(FnYrCode, 2))

            Pk_Condition1 = "ADVOP-"

        Else
            Select Case Trim(LCase(cbo_AdvanceSalary.Text))
                Case "advance"
                    lbl_Heading.Text = "EMPLOYEE LOAN PAYMENT"
                    grp_PreviousLoanBalance.Visible = True

                Case "salary"
                    lbl_Heading.Text = "SALARY EMPLOYEE PAYMENT"
                    'Pk_Condition1 = "ESLPY-"

                Case "salaryadvance"
                    lbl_Heading.Text = "SALARY ADVANCE EMPLOYEE PAYMENT"
                    'Pk_Condition1 = "ESAPY-"
            End Select


            FnYrCode = Common_Procedures.FnYearCode

        End If



        con.Open()

        cbo_CashCheque.Items.Clear()
        cbo_CashCheque.Items.Add("")
        cbo_CashCheque.Items.Add("CASH")
        cbo_CashCheque.Items.Add("CHEQUE")


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EmployeeName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CashCheque.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DebitAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AdvanceSalary.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterFrom_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterTo_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EmployeeFilter.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EmployeeName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CashCheque.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DebitAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AdvanceSalary.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_FilterFrom_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterTo_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EmployeeFilter.LostFocus, AddressOf ControlLostFocus

        'AddHandler cbo_MillFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        ' AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub cbo_EmployeeFilter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EmployeeFilter.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'EMPLOYEE')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_EmployeeFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EmployeeFilter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EmployeeFilter, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'EMPLOYEE')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyNameFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EmployeeFilter.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EmployeeFilter, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'EMPLOYEE')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_filtershow_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub txt_Amount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Amount.LostFocus
        txt_Amount.Text = Format(Val(txt_Amount.Text), "#########0.00")
    End Sub

    Private Sub cbo_DebitAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DebitAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Payroll_Employee_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DebitAccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_VouNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_VouNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub



    Private Sub cbo_EmployeeName_Validated(sender As Object, e As EventArgs) Handles cbo_EmployeeName.Validated

        Display_Loan_Status()
        cbo_EmployeeName.Tag = cbo_EmployeeName.Text

    End Sub

    Private Sub cbo_EmployeeName_Enter(sender As Object, e As EventArgs) Handles cbo_EmployeeName.Enter
        cbo_EmployeeName.Tag = cbo_EmployeeName.Text
    End Sub

    Private Sub txt_Amount_TextChanged(sender As Object, e As EventArgs) Handles txt_Amount.TextChanged
        txt_CurrentLoan.Text = FormatNumber(Val(txt_ExistingLoan.Text) + Val(txt_Amount.Text), 2, TriState.False, TriState.False, TriState.False)
    End Sub

    Private Sub txt_ExistingLoan_TextChanged(sender As Object, e As EventArgs) Handles txt_ExistingLoan.TextChanged
        txt_CurrentLoan.Text = FormatNumber(Val(txt_ExistingLoan.Text) + Val(txt_Amount.Text), 2, TriState.False, TriState.False, TriState.False)
    End Sub

    Private Sub cbo_EmployeeName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_EmployeeName.SelectedIndexChanged

    End Sub

    Private Sub dtp_Date_ValueChanged(sender As Object, e As EventArgs) Handles dtp_Date.ValueChanged

    End Sub

    Private Sub dtp_Date_Validated(sender As Object, e As EventArgs) Handles dtp_Date.Validated


    End Sub

    Private Sub dtp_Date_LostFocus(sender As Object, e As EventArgs) Handles dtp_Date.LostFocus

        Display_Loan_Status()

    End Sub

    Private Sub Display_Loan_Status()

        txt_ExistingLoan.Text = "0.00"
        txt_ExistingEMI.Text = "0.00"

        If grp_PreviousLoanBalance.Visible And Len(cbo_EmployeeName.Text) > 0 Then

            If cbo_EmployeeName.Tag <> cbo_EmployeeName.Text Then

                Dim vEmp_IdNo As Integer = Common_Procedures.Employee_NameToIdNo(con, cbo_EmployeeName.Text)

                If vEmp_IdNo > 0 Then
                    Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_VouNo.Text) & "/" & Trim(FnYrCode)

                    Dim CMD As New SqlClient.SqlCommand
                    CMD.Connection = con

                    CMD.CommandText = " Select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ Where a.Ledger_IdNo = " & Str(Val(vEmp_IdNo)) & " and " &
                                  " a.ledger_idno = b.ledger_idno and a.company_idno = tZ.company_idno " &
                                  " and not a.Entry_Identification = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'" &
                                  " and (a.Voucher_Code LIKE 'ADVOP-%' or a.Voucher_Code LIKE 'EADPY-%' or a.Voucher_Code LIKE 'ADVLS-%' or a.Voucher_Code LIKE 'AVLDD-%')"
                    Dim da As New SqlClient.SqlDataAdapter
                    da.SelectCommand = CMD

                    Dim dt As New DataTable
                    da.Fill(dt)

                    If dt.Rows.Count > 0 Then
                        If Not IsDBNull(dt.Rows(0).Item(0)) Then
                            txt_ExistingLoan.Text = FormatNumber(-1 * dt.Rows(0).Item(0), 2, True, TriState.True, False)
                        End If

                    End If

                    dt.Rows.Clear()


                    '-------------------

                    Dim dt1 As New DataTable

                    CMD.CommandText = "Select * from Loan_EMI_Settings where Employee_IdNo = " & vEmp_IdNo.ToString
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If Not IsDBNull(dt1.Rows(0).Item(1)) Then
                            txt_ExistingEMI.Text = dt1.Rows(0).Item(1)
                        End If
                    End If

                    dt1.Rows.Clear()

                    '-------------------

                    CMD.CommandText = "Select count(*) from Voucher_Details where Ledger_IdNo = " & vEmp_IdNo.ToString & " and  Voucher_Code like '" & Pk_Condition2 & "%' " &
                                  "and Voucher_date > '" & Format(dtp_Date.Value, "dd-MMM-yyyy") & "' and not Voucher_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
                    da.Fill(dt)

                    If dt.Rows(0).Item(0) > 0 Then
                        txt_NewEMI.Text = txt_ExistingEMI.Text
                        txt_NewEMI.Visible = False
                    Else
                        txt_NewEMI.Text = txt_ExistingEMI.Text
                        txt_NewEMI.Visible = True
                    End If

                End If

            End If
        End If




    End Sub

    Private Sub cbo_EmployeeName_Leave(sender As Object, e As EventArgs) Handles cbo_EmployeeName.Leave


    End Sub
End Class