Public Class Opening_Balance_Stock
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private OpYrCode As String = ""
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private ClrSTS As Boolean = False
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BillDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ClothDetails As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        ClrSTS = True

        New_Entry = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        pnl_Back.Enabled = True

        lbl_IdNo.Text = ""
        cbo_Ledger.Text = ""
        txt_OpAmount.Text = "0.00"
        cbo_CrDrType.Text = "Cr"

      
        cbo_BillGrid_AgentName.Visible = False
        cbo_BillGrid_CrDr.Visible = False

    
        cbo_BillGrid_AgentName.Text = ""
        cbo_BillGrid_CrDr.Text = ""

        dgv_BillDetails.Rows.Clear()

      

        dgv_BillDetails_Total.Rows.Clear()
        dgv_BillDetails_Total.Rows.Add()

        tab_Main.SelectTab(0)
    

      
        cbo_BillGrid_AgentName.Visible = False
        cbo_BillGrid_CrDr.Visible = False

        txt_OpAmount.Enabled = True
        cbo_CrDrType.Enabled = True
        dgv_BillDetails.Enabled = False

        Grid_Cell_DeSelect()

        ClrSTS = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim dgvtxtedtctrl As DataGridViewTextBoxEditingControl

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

      
        If Me.ActiveControl.Name <> cbo_BillGrid_AgentName.Name Then
            cbo_BillGrid_AgentName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BillGrid_CrDr.Name Then
            cbo_BillGrid_CrDr.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        dgv_BillDetails.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Sno As Integer = 0, n As Integer = 0
        Dim NewCode As String = ""
        Dim BilType As String = ""
        Dim LedType As String = ""
        Dim LockSTS As Boolean = False
        Dim J As Integer = 0
          Dim Nr As Integer = 0
        Dim CrDr_Amt_ColNm As String = ""


        If Val(idno) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(idno)) & "/" & Trim(OpYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.Ledger_IdNo, a.Ledger_Name from Ledger_Head a Where a.Ledger_IdNo = " & Str(Val(idno)) & "", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_IdNo.Text = dt1.Rows(0).Item("Ledger_IdNo").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString

                BilType = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Bill_Type", "(Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & ")")

                LedType = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & ")")

                If Trim(UCase(BilType)) = "BILL TO BILL" Then
                    txt_OpAmount.Enabled = False
                    cbo_CrDrType.Enabled = False
                    dgv_BillDetails.Enabled = True

                Else
                    txt_OpAmount.Enabled = True
                    cbo_CrDrType.Enabled = True
                    dgv_BillDetails.Enabled = False

                End If

              


                Cmd.Connection = con



                da2 = New SqlClient.SqlDataAdapter("Select a.party_bill_no, a.voucher_bill_date, b.ledger_name as agent_name, a.bill_amount, a.crdr_type, abs(a.bill_amount - a.credit_amount - a.debit_amount) as Paid_rcvd_Amount, a.Voucher_Bill_Code from voucher_bill_head a left outer join ledger_head b on a.agent_idno = b.ledger_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(idno)) & " and a.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' order by a.voucher_bill_date, a.Voucher_Bill_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_BillDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_BillDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_BillDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_BillDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("party_bill_no").ToString
                        dgv_BillDetails.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("voucher_bill_date").ToString), "dd-MM-yyyy")
                        dgv_BillDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("agent_name").ToString
                        dgv_BillDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("bill_amount").ToString), "########0.00")
                        dgv_BillDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("crdr_type").ToString
                        dgv_BillDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Paid_rcvd_Amount").ToString), "########0.00")
                        If Val(dgv_BillDetails.Rows(n).Cells(6).Value) = 0 Then
                            dgv_BillDetails.Rows(n).Cells(6).Value = ""
                        End If
                        dgv_BillDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Voucher_Bill_Code").ToString

                    Next i

                End If

                dt2.Clear()

                Total_BillAmount_Calculation()
      
                'da2 = New SqlClient.SqlDataAdapter("Select a.party_bill_no, a.voucher_bill_date, b.ledger_name as agent_name, a.bill_amount, a.crdr_type, abs(a.bill_amount - a.credit_amount - a.debit_amount) as Vou_Paid_Rcvd_Amount, a.Voucher_Bill_Code, a.Voucher_Bill_DetailsSlNo, c.Currency1 as YPYS_Paid_Rcvd_Amount from voucher_bill_head a LEFT OUTER JOIN ledger_head b on a.agent_idno = b.ledger_idno LEFT OUTER JOIN entrytemp c ON a.company_idno = c.int1  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(idno)) & " and a.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' order by a.voucher_bill_date, a.Voucher_Bill_No", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'dgv_BillDetails.Rows.Clear()
                'Sno = 0

                'If dt2.Rows.Count > 0 Then

                '    For i = 0 To dt2.Rows.Count - 1

                '        n = dgv_BillDetails.Rows.Add()

                '        Sno = Sno + 1
                '        dgv_BillDetails.Rows(n).Cells(0).Value = Val(Sno)
                '        dgv_BillDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("party_bill_no").ToString
                '        dgv_BillDetails.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("voucher_bill_date").ToString), "dd-MM-yyyy")
                '        dgv_BillDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("agent_name").ToString
                '        dgv_BillDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("bill_amount").ToString), "########0.00")
                '        dgv_BillDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("crdr_type").ToString
                '        dgv_BillDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("YPYS_Paid_Rcvd_Amount").ToString), "########0.00")
                '        If Val(dgv_BillDetails.Rows(n).Cells(6).Value) <> 0 Then
                '            LockSTS = True

                '            For J = 0 To dgv_BillDetails.ColumnCount - 1
                '                dgv_BillDetails.Rows(n).Cells(J).Style.ForeColor = Color.Red
                '                dgv_BillDetails.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                '            Next

                '        Else
                '            dgv_BillDetails.Rows(n).Cells(6).Value = ""
                '        End If
                '        dgv_BillDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Voucher_Bill_Code").ToString
                '        dgv_BillDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Voucher_Bill_DetailsSlNo").ToString

                '    Next i

                'End If

                'dt2.Clear()


                da2 = New SqlClient.SqlDataAdapter("Select sum(voucher_amount) from voucher_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(idno)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item(0).ToString) = False Then
                        If Val(dt2.Rows(0).Item(0).ToString) <> 0 Then
                            txt_OpAmount.Text = Trim(Format(Math.Abs(Val(dt2.Rows(0).Item(0).ToString)), "#########0.00"))
                        End If
                        If Val(dt2.Rows(0).Item(0).ToString) >= 0 Then
                            cbo_CrDrType.Text = "Cr"
                        Else
                            cbo_CrDrType.Text = "Dr"
                        End If
                    End If
                End If
                dt2.Clear()

            End If

            dt1.Clear()


            If LockSTS = True Then

            End If


            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            'If cbo_Ledger.Visible And cbo_Ledger.Enabled Then cbo_Ledger.Focus()

        End Try



    End Sub

    Private Sub Opening_Balance_Stock_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Opening_Balance_Stock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim Dt7 As New DataTable
        Dim Dt8 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
        da.Fill(Dt1)
        cbo_Ledger.DataSource = Dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(Dt2)
        cbo_BillGrid_AgentName.DataSource = Dt2
        cbo_BillGrid_AgentName.DisplayMember = "Ledger_DisplayName"

        cbo_BillGrid_CrDr.Items.Clear()
        cbo_BillGrid_CrDr.Items.Add("CR")
        cbo_BillGrid_CrDr.Items.Add("DR")



        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CrDrType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillGrid_AgentName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillGrid_CrDr.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CrDrType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillGrid_AgentName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillGrid_CrDr.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_OpAmount.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_OpAmount.KeyPress, AddressOf TextBoxControlKeyPress

        'cbo_Ledger.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Opening_Balance_Stock_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Opening_Balance_Stock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_BillDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

           If ActiveControl.Name = dgv_BillDetails.Name Then
                dgv1 = dgv_BillDetails

            ElseIf dgv_BillDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BillDetails

            ElseIf tab_Main.SelectedIndex = 0 Then
                dgv1 = dgv_BillDetails

            End If

            With dgv1

                If dgv1.Name = dgv_BillDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                             
                                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()

                                    Else
                                        tab_Main.SelectTab(0)
                                        cbo_Ledger.Focus()

                                    End If


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                               
                                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()

                                    Else
                                        tab_Main.SelectTab(0)
                                        cbo_Ledger.Focus()

                                    End If



                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If cbo_CrDrType.Enabled = True Then
                                    cbo_CrDrType.Focus()
                                Else
                                    cbo_Ledger.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim tr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim NewCode As String = ""
        Dim New_PurSalCode As String = ""
        Dim LedName As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Opening_Balance_Stock, "~L~") = 0 And InStr(Common_Procedures.UR.Opening_Balance_Stock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        LedName = Common_Procedures.Ledger_IdNoToName(con, Val(lbl_IdNo.Text))

        If Trim(LedName) = "" Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

        New_PurSalCode = Trim(Pk_Condition) & Trim(Val(lbl_IdNo.Text)) & "-"

        da = New SqlClient.SqlDataAdapter("select count(*) from voucher_bill_head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and bill_amount <> (credit_amount + debit_amount) ", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Alrady Amount Received/Paid for some bills", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        dt.Clear()

        'da = New SqlClient.SqlDataAdapter("Select sum(Received_BillAmount+Received_CommAmount ) from Commission_Yarn_Purchase_Head Where Yarn_Purchase_Code LIKE '" & Trim(New_PurSalCode) & "%' and Company_Idno = " & Str(Val(lbl_Company.Tag)) & " and Purchase_LedgerIdNo = " & Str(Val(lbl_IdNo.Text)), con)
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '        If Val(dt.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already BillAmount Paid", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'dt.Clear()

        'da = New SqlClient.SqlDataAdapter("Select sum(Received_BillAmount+Received_CommAmount) from Commission_Yarn_Sales_Head Where Yarn_Sales_Code LIKE '" & Trim(New_PurSalCode) & "%' and Company_Idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_LedgerIdNo = " & Str(Val(lbl_IdNo.Text)), con)
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '        If Val(dt.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already BillAmount Paid", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'dt.Clear()

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
                Exit Sub
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            tr.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Enabled = True And cbo_Ledger.Visible = True Then cbo_Ledger.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Ledger_IdNo from Ledger_Head where Ledger_IdNo <> 0 Order by Ledger_IdNo"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            da = New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Ledger_Head where Ledger_IdNo > " & Str(OrdByNo) & " Order by Ledger_IdNo", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 Ledger_IdNo from Ledger_Head where ledger_idno < " & Str(Val(OrdByNo)) & " Order by Ledger_IdNo desc"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If
            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Ledger_Head where ledger_idno <> 0 Order by Ledger_IdNo desc", con)
        Dim dt As New DataTable
        Dim movno As Integer

        Try
            da.Fill(dt)

            movno = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

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

            da = New SqlClient.SqlDataAdapter("select max(ledger_idno) from Ledger_Head where ledger_idno <> 0", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(NewID) <= 100 Then NewID = 100

            lbl_IdNo.Text = Val(NewID) + 1

            lbl_IdNo.ForeColor = Color.Red

            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim LedName As String
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim OpDate As Date
        Dim VouAmt As Double
       
        Dim Dup_SetCd As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim Mtr_Pc As Double = 0
        Dim vAgt_ID As Integer = 0
        Dim vTot_BlAmt As Double, vTot_Bl_PydRcvd_Amt As Double
        Dim bl_amt As Single = 0
        Dim CrDr_Amt_ColNm As String = ""
        Dim vou_bil_no As String = ""
        Dim vou_bil_code As String = ""
        Dim New_PurSalCode As String = "", Dup_PBillNo As String = ""
        Dim Yps_SlNo As Integer = 0, Yps_BillAmt As Double = 0, Yps_CommAmt As Double = 0, Yps_PBillNo As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Opening_Balance_Stock, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_Ledger.Text) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        LedName = Common_Procedures.Ledger_IdnoToAlaisName(con, Val(lbl_IdNo.Text))
        If Trim(LedName) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Val(txt_OpAmount.Text) <> 0 And Trim(cbo_CrDrType.Text) = "" Then
            MessageBox.Show("Invalid Cr/Dr", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CrDrType.Enabled Then cbo_CrDrType.Focus()
            Exit Sub
        End If

        With dgv_BillDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Bill Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If IsDate(.Rows(i).Cells(2).Value) = False Then
                        MessageBox.Show("Invalid Bill Date format", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(3).Value) <> "" Then
                        vAgt_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(3).Value))
                        If Val(vAgt_ID) = 0 Then
                            MessageBox.Show("Invalid Agent Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                tab_Main.SelectTab(0)
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(3)
                            End If
                            Exit Sub
                        End If
                    End If

                    If Trim(.Rows(i).Cells(5).Value) = "" Then
                        MessageBox.Show("Invalid Cr/Dr", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next i

        End With


      
        vTot_BlAmt = 0 : vTot_Bl_PydRcvd_Amt = 0
        If dgv_BillDetails_Total.RowCount > 0 Then
            vTot_BlAmt = Val(dgv_BillDetails_Total.Rows(0).Cells(4).Value())
            vTot_Bl_PydRcvd_Amt = Val(dgv_BillDetails_Total.Rows(0).Cells(6).Value())
        End If

        tr = con.BeginTransaction

        Try

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            If Val(txt_OpAmount.Text) <> 0 Then

                VouAmt = Math.Abs(Val(txt_OpAmount.Text))
                If Trim(UCase(cbo_CrDrType.Text)) = "DR" Then VouAmt = -1 * VouAmt

                Sno = Sno + 1

                cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Sl_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(lbl_IdNo.Text)) & ", 'Opng', @OpeningDate, " & Str(Val(Sno)) & ", " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(VouAmt)) & ", 'Opening', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "')"
                cmd.ExecuteNonQuery()

            End If


         
     
            cmd.CommandText = "delete from voucher_bill_head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(lbl_IdNo.Text)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and bill_amount = (credit_amount + debit_amount)"
            cmd.ExecuteNonQuery()

            Sno = 0

            With dgv_BillDetails

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@VouBillDate", CDate(.Rows(i).Cells(2).Value))

                        If Trim(UCase(.Rows(i).Cells(5).Value)) = "CR" Then CrDr_Amt_ColNm = "credit_amount" Else CrDr_Amt_ColNm = "debit_amount"

                        vAgt_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(3).Value), tr)

                        Sno = Sno + 1

                        If Trim(.Rows(i).Cells(7).Value) <> "" And Val(.Rows(i).Cells(6).Value) <> 0 Then
                            Nr = 0
                            cmd.CommandText = "update voucher_bill_head set " _
                                                        & " voucher_bill_date = @VouBillDate, " _
                                                        & " party_bill_no = '" & Trim(.Rows(i).Cells(1).Value) & "', " _
                                                        & " agent_idno = " & Str(Val(vAgt_ID)) & ", " _
                                                        & " bill_amount = " & Str(Val(.Rows(i).Cells(4).Value)) & ", " _
                                                        & " crdr_type = '" & Trim(.Rows(i).Cells(5).Value) & "', " _
                                                        & " " & CrDr_Amt_ColNm & " = " & Str(Val(.Rows(i).Cells(4).Value)) & " " _
                                                        & " where " _
                                                        & " Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and " _
                                                        & " voucher_bill_code = '" & Trim(.Rows(i).Cells(7).Value) & "'"

                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                Throw New ApplicationException("Error On Bill Details")
                            End If


                        Else

                            vou_bil_no = Common_Procedures.get_MaxCode(con, "Voucher_Bill_Head", "Voucher_Bill_Code", "For_OrderBy", "", Val(lbl_Company.Tag), OpYrCode, tr)
                            vou_bil_code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vou_bil_no) & "/" & Trim(OpYrCode)

                            cmd.CommandText = "Insert into voucher_bill_head ( voucher_bill_code,             company_idno         ,        voucher_bill_no    ,            for_orderby      , voucher_bill_date,              ledger_idno        ,        party_bill_no                   ,            agent_idno    ,              bill_amount                 , " & Trim(CrDr_Amt_ColNm) & "             ,            crdr_type                   ,        entry_identification                  ) " _
                                                    & "  Values ( '" & Trim(vou_bil_code) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vou_bil_no) & "', " & Str(Val(vou_bil_no)) & ",     @VouBillDate , " & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(vAgt_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", '" & Trim(.Rows(i).Cells(5).Value) & "', '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next i

            End With

            tr.Commit()

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

    End Sub

    
    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub cbo_CrDrType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CrDrType.KeyDown
        Try
            With cbo_CrDrType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_OpAmount.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    If dgv_BillDetails.Enabled = True Then
                        tab_Main.SelectTab(0)
                        dgv_BillDetails.Focus()
                        dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                        dgv_BillDetails.CurrentCell.Selected = True

                    Else
                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                            save_record()

                        Else
                            tab_Main.SelectTab(0)
                            cbo_Ledger.Focus()

                        End If

                    End If

                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_CrDrType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CrDrType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_CrDrType

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


                        If dgv_BillDetails.Enabled = True Then
                            tab_Main.SelectTab(0)
                            dgv_BillDetails.Focus()
                            dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                            dgv_BillDetails.CurrentCell.Selected = True

                        Else
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                save_record()

                            Else
                                tab_Main.SelectTab(0)
                                cbo_Ledger.Focus()

                            End If

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

        With cbo_Ledger
            If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If txt_OpAmount.Enabled And txt_OpAmount.Visible Then
                    txt_OpAmount.Focus()
                Else

                    If dgv_BillDetails.Enabled = True Then
                        tab_Main.SelectTab(0)
                        dgv_BillDetails.Focus()
                        dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                        dgv_BillDetails.CurrentCell.Selected = True

                  

                    Else
                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                            save_record()

                        Else
                            tab_Main.SelectTab(0)
                            cbo_Ledger.Focus()

                        End If

                    End If

                End If

            End If
        End With

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim LedIdNo As Integer
        Dim BilType As String

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            If Val(LedIdNo) <> 0 Then
                move_record(LedIdNo)
            End If

            BilType = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Bill_Type", "(Ledger_IdNo = " & Str(Val(LedIdNo)) & ")")

            If Trim(UCase(BilType)) = "BILL TO BILL" Then
                txt_OpAmount.Enabled = False
                cbo_CrDrType.Enabled = False
                dgv_BillDetails.Enabled = True

            Else
                txt_OpAmount.Enabled = True
                cbo_CrDrType.Enabled = True
                dgv_BillDetails.Enabled = False

            End If

            If txt_OpAmount.Enabled And txt_OpAmount.Visible Then
                txt_OpAmount.Focus()

            Else

                If dgv_BillDetails.Enabled = True Then
                    tab_Main.SelectTab(0)
                    dgv_BillDetails.Focus()
                    dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                    dgv_BillDetails.CurrentCell.Selected = True


                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()

                    Else
                        tab_Main.SelectTab(0)
                        cbo_Ledger.Focus()

                    End If

                End If

            End If

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

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

   

    Private Sub cbo_BillGrid_AgentName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillGrid_AgentName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_BillGrid_AgentName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillGrid_AgentName.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillGrid_AgentName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

            With cbo_BillGrid_AgentName
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_BillDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True

                    End With

                    .Visible = False

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    With dgv_BillDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_BillGrid_AgentName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillGrid_AgentName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillGrid_AgentName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

        Try

            With cbo_BillGrid_AgentName

                If Asc(e.KeyChar) = 13 Then

                    With dgv_BillDetails
                        .Focus()
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BillGrid_AgentName.Text)
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_BillGrid_AgentName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillGrid_AgentName.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    Dim f As New Agent_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_BillGrid_AgentName.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
    End Sub

    Private Sub cbo_BillGrid_AgentName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillGrid_AgentName.TextChanged
        Try
            If cbo_BillGrid_AgentName.Visible Then
                With dgv_BillDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_BillGrid_AgentName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BillGrid_AgentName.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_BillGrid_CrDr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillGrid_CrDr.KeyDown

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillGrid_CrDr, Nothing, Nothing, "", "", "", "")

            With cbo_BillGrid_CrDr
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_BillDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                    End With

                    .Visible = False

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    With dgv_BillDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_BillGrid_CrDr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillGrid_CrDr.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillGrid_CrDr, Nothing, "", "", "", "")

        Try

            With cbo_BillGrid_CrDr

                If Asc(e.KeyChar) = 13 Then

                    With dgv_BillDetails
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BillGrid_CrDr.Text)
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False


                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_BillGrid_CrDr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillGrid_CrDr.TextChanged
        Try
            If cbo_BillGrid_CrDr.Visible Then
                With dgv_BillDetails
                    If .Visible = True Then
                        If Val(cbo_BillGrid_CrDr.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BillGrid_CrDr.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_BillDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellEndEdit
        dgv_BillDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_BillDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        If ClrSTS = True = True Then Exit Sub

        With dgv_BillDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(8).Value = 3
                Else
                    .Rows(e.RowIndex).Cells(8).Value = Val(.Rows(e.RowIndex - 1).Cells(8).Value) + 1
                End If
            End If

            If .CurrentCell.ColumnIndex = 3 And Val(.CurrentRow.Cells(6).Value) = 0 Then

                If cbo_BillGrid_AgentName.Visible = False Or Val(cbo_BillGrid_AgentName.Tag) <> e.RowIndex Then

                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_BillGrid_AgentName.DataSource = Dt1
                    cbo_BillGrid_AgentName.DisplayMember = "Ledger_DisplayName"

                    cbo_BillGrid_AgentName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BillGrid_AgentName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BillGrid_AgentName.Width = .CurrentCell.Size.Width
                    cbo_BillGrid_AgentName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BillGrid_AgentName.Tag = Val(.CurrentCell.RowIndex)
                    cbo_BillGrid_AgentName.Visible = True

                    cbo_BillGrid_AgentName.BringToFront()
                    cbo_BillGrid_AgentName.Focus()

                End If

            Else

                cbo_BillGrid_AgentName.Visible = False

            End If

            If .CurrentCell.ColumnIndex = 5 And Val(.CurrentRow.Cells(6).Value) = 0 Then

                If cbo_BillGrid_CrDr.Visible = False Or Val(cbo_BillGrid_CrDr.Tag) <> e.RowIndex Then

                    cbo_BillGrid_CrDr.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BillGrid_CrDr.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BillGrid_CrDr.Width = .CurrentCell.Size.Width
                    cbo_BillGrid_CrDr.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BillGrid_CrDr.Tag = Val(.CurrentCell.RowIndex)
                    cbo_BillGrid_CrDr.Visible = True

                    cbo_BillGrid_CrDr.BringToFront()
                    cbo_BillGrid_CrDr.Focus()

                End If

            Else

                cbo_BillGrid_CrDr.Visible = False


            End If

        End With

    End Sub

    Private Sub dgv_BillDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellLeave
        Try
            With dgv_BillDetails
                If .Rows.Count > 0 Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                        Else
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub dgv_BillDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellValueChanged
        Try
            With dgv_BillDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then
                            Total_BillAmount_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_BillDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BillDetails.EditingControlShowing
        dgtxt_BillDetails = Nothing

        With dgv_BillDetails

            If .Rows.Count > 0 Then


                dgtxt_BillDetails = CType(dgv_BillDetails.EditingControl, DataGridViewTextBoxEditingControl)

            End If

        End With

    End Sub

    Private Sub dgv_BillDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BillDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_BillDetails

                If .Rows.Count > 0 Then

                    If Val(.CurrentRow.Cells(6).Value) = 0 Then

                        n = .CurrentRow.Index

                        If n = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                        Total_BillAmount_Calculation()

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub dgv_BillDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BillDetails.LostFocus
        On Error Resume Next
        dgv_BillDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_BillDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_BillDetails.RowsAdded
        Dim n As Integer

        With dgv_BillDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub Total_BillAmount_Calculation()
        Dim Sno As Integer
        Dim TotBlCrAmt As Single
        Dim TotBlDrAmt As Single
        Dim TotPydRcvdAmt As Single

        Sno = 0
        TotBlCrAmt = 0
        TotBlDrAmt = 0
        TotPydRcvdAmt = 0

        With dgv_BillDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    If Trim(UCase(.Rows(i).Cells(5).Value)) = "CR" Then
                        TotBlCrAmt = TotBlCrAmt + Math.Abs(Val(.Rows(i).Cells(4).Value))
                    Else
                        TotBlDrAmt = TotBlDrAmt + Math.Abs(Val(.Rows(i).Cells(4).Value))
                    End If
                    TotPydRcvdAmt = TotPydRcvdAmt + Val(.Rows(i).Cells(6).Value)
                End If
            Next
        End With

        With dgv_BillDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Format(Math.Abs(Val(TotBlCrAmt) - Val(TotBlDrAmt)), "########0.00")
            txt_OpAmount.Text = Trim(Format(Val(.Rows(0).Cells(4).Value), "#########0.00"))
            If Val(TotBlCrAmt) > Val(TotBlDrAmt) Then
                .Rows(0).Cells(5).Value = "Cr"
                cbo_CrDrType.Text = "Cr"
            Else
                .Rows(0).Cells(5).Value = "Dr"
                cbo_CrDrType.Text = "Dr"
            End If
        End With

    End Sub

    
    Private Sub dgtxt_BillDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BillDetails.Enter
        dgv_BillDetails.EditingControl.BackColor = Color.Lime
        dgv_BillDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_BillDetails.SelectAll()
    End Sub



    Private Sub dgtxt_BillDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BillDetails.KeyDown
        With dgv_BillDetails

            If .Rows.Count > 0 Then

                If Val(.CurrentRow.Cells(6).Value) <> 0 Then

                    'e.Handled = True
                    'e.SuppressKeyPress = True

                End If

            End If

        End With
    End Sub

    Private Sub dgtxt_BillDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BillDetails.KeyPress
        With dgv_BillDetails
            If Val(.CurrentRow.Cells(6).Value) <> 0 Then
                'e.Handled = True

            Else
                If .CurrentCell.ColumnIndex = 4 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End If


        End With
    End Sub

    Private Sub dgtxt_BillDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BillDetails.KeyUp
        dgv_BillDetails_KeyUp(sender, e)
    End Sub


   


    Private Sub tab_Main_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_Main.SelectedIndexChanged
        If tab_Main.SelectedIndex = 0 Then
            If dgv_BillDetails.Enabled Then
                dgv_BillDetails.Focus()
                dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                dgv_BillDetails.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus

        Dim LedIdNo As Integer
        Dim BilType As String

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Val(LedIdNo) <> 0 Then
            move_record(LedIdNo)
        End If

        BilType = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Bill_Type", "(Ledger_IdNo = " & Str(Val(LedIdNo)) & ")")

        If Trim(UCase(BilType)) = "BILL TO BILL" Then
            txt_OpAmount.Enabled = False
            cbo_CrDrType.Enabled = False
            dgv_BillDetails.Enabled = True

        Else
            txt_OpAmount.Enabled = True
            cbo_CrDrType.Enabled = True
            dgv_BillDetails.Enabled = False

        End If

        If txt_OpAmount.Enabled And txt_OpAmount.Visible Then
            txt_OpAmount.Focus()

        Else

            If dgv_BillDetails.Enabled = True Then
                tab_Main.SelectTab(0)
                dgv_BillDetails.Focus()
                dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                dgv_BillDetails.CurrentCell.Selected = True


            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()

                Else
                    tab_Main.SelectTab(0)
                    cbo_Ledger.Focus()

                End If

            End If

        End If



    End Sub

    Private Sub cbo_Ledger_MarginChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.MarginChanged

    End Sub

    
    Private Sub cbo_Ledger_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Ledger.SelectedIndexChanged

    End Sub
End Class