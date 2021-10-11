Public Class PayRoll_Employee_Salary_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EMPSL-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_SalaryType.Text = ""
        Dtp_FromDate.Text = ""
        cbo_Month.Text = ""
        Dtp_ToDate.Text = ""
        txt_FestivalDays.Text = ""
        txt_TotalDays.Text = ""



        dgv_Details.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            ' txt_Month.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()


        NoCalc_Status = False

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
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Salary_Payment_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Payment_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Salary_Payment_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Salary_Payment_Date").ToString
                cbo_SalaryType.Text = dt1.Rows(0).Item("Salary_Type").ToString
                cbo_Month.Text = Common_Procedures.Month_IdNoToName(con, Val(dt1.Rows(0).Item("Month_IdNo").ToString))
                Dtp_FromDate.Text = dt1.Rows(0).Item("From_Date").ToString
                Dtp_ToDate.Text = dt1.Rows(0).Item("To_Date").ToString
                txt_TotalDays.Text = Format(Val(dt1.Rows(0).Item("Total_Days").ToString), "#########0.00")
                txt_FestivalDays.Text = Format(Val(dt1.Rows(0).Item("Festival_Days").ToString), "#########0.00")

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name from Salary_Payment_Details a INNER JOIN Employee_Head b ON a.Employee_IdNo = b.Employee_IdNo  Where a.Salary_Payment_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Employee_Name").ToString
                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Basic_Salary").ToString), "########0.00")
                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Total_Days").ToString), "########0.00")
                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Net_Pay").ToString), "########0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("No_Of_Attendance_Days").ToString), "########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("From_W_Off_CR").ToString), "########0.000")
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Festival_Holidays").ToString), "########0.000")
                            ' .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Days").ToString), "########0.000")
                            .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("No_Of_Leave").ToString), "########0.000")
                            .Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Attendance_On_W_Off_Fh").ToString), "########0.000")
                            .Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Op_W_Off_CR").ToString), "########0.000")
                            .Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Add_W_Off_CR").ToString), "########0.000")
                            .Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Less_W_Off_CR").ToString), "########0.000")
                            .Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Total_W_Off_CR").ToString), "########0.000")
                            .Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Salary_Days").ToString), "########0.000")
                            .Rows(n).Cells(16).Value = Format(Val(dt2.Rows(i).Item("Basic_Pay").ToString), "########0.000")
                            .Rows(n).Cells(17).Value = Format(Val(dt2.Rows(i).Item("D_A").ToString), "########0.000")
                            .Rows(n).Cells(18).Value = Format(Val(dt2.Rows(i).Item("Earning").ToString), "########0.00")
                            .Rows(n).Cells(19).Value = Format(Val(dt2.Rows(i).Item("H_R_A").ToString), "########0.00")
                            .Rows(n).Cells(20).Value = Format(Val(dt2.Rows(i).Item("Conveyance").ToString), "########0.00")
                            .Rows(n).Cells(21).Value = Format(Val(dt2.Rows(i).Item("Washing").ToString), "########0.00")
                            .Rows(n).Cells(22).Value = Format(Val(dt2.Rows(i).Item("Entertainment").ToString), "########0.000")
                            .Rows(n).Cells(23).Value = Format(Val(dt2.Rows(i).Item("Maintenance").ToString), "########0.000")
                            .Rows(n).Cells(24).Value = Format(Val(dt2.Rows(i).Item("Other_Addition").ToString), "########0.000")
                            .Rows(n).Cells(25).Value = Format(Val(dt2.Rows(i).Item("Total_Addition").ToString), "########0.000")
                            .Rows(n).Cells(26).Value = Format(Val(dt2.Rows(i).Item("Mess").ToString), "########0.000")
                            .Rows(n).Cells(27).Value = Format(Val(dt2.Rows(i).Item("Medical").ToString), "########0.000")
                            .Rows(n).Cells(28).Value = Format(Val(dt2.Rows(i).Item("Store").ToString), "########0.000")
                            .Rows(n).Cells(29).Value = Format(Val(dt2.Rows(i).Item("ESI").ToString), "########0.000")
                            .Rows(n).Cells(30).Value = Format(Val(dt2.Rows(i).Item("P_F").ToString), "########0.000")
                            .Rows(n).Cells(31).Value = Format(Val(dt2.Rows(i).Item("E_P_F").ToString), "########0.000")
                            .Rows(n).Cells(32).Value = Format(Val(dt2.Rows(i).Item("Pension_Scheme").ToString), "########0.000")
                            .Rows(n).Cells(33).Value = Format(Val(dt2.Rows(i).Item("Other_Deduction").ToString), "########0.000")
                            .Rows(n).Cells(34).Value = Format(Val(dt2.Rows(i).Item("Total_Deduction").ToString), "########0.000")
                            .Rows(n).Cells(35).Value = Format(Val(dt2.Rows(i).Item("Attendance_Incentive").ToString), "########0.000")
                            .Rows(n).Cells(36).Value = Format(Val(dt2.Rows(i).Item("Net_Salary").ToString), "########0.000")
                            .Rows(n).Cells(37).Value = Format(Val(dt2.Rows(i).Item("Advance").ToString), "########0.000")
                            '.Rows(n).Cells(38).Value = Format(Val(dt2.Rows(i).Item("Net_pay").ToString), "########0.000")
                            .Rows(n).Cells(39).Value = Format(Val(dt2.Rows(i).Item("Day_For_Bonus").ToString), "########0.000")
                            .Rows(n).Cells(40).Value = Format(Val(dt2.Rows(i).Item("Earning_For_Bonus").ToString), "########0.000")

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

        NoCalc_Status = False
    End Sub

    Private Sub Employee_List()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim SNo As Integer

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

        da1 = New SqlClient.SqlDataAdapter("select a.Employee_Name from Employee_Head a where a.Date_Status <> 1", con)
        da1.Fill(dt1)

        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    da2 = New SqlClient.SqlDataAdapter("select a.Employee_Name from Employee_Head a where a.Date_Status <> 1", con)
                    da2.Fill(dt2)

                    .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Employee_Name").ToString

                Next i

            End If
        End With
    End Sub

    Private Sub Salary_Payment_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Salary_Payment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable


        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Month_Name from Month_Head order by Month_Name", con)
        da.Fill(dt2)
        cbo_Month.DataSource = dt2
        cbo_Month.DisplayMember = "Month_Name"


        cbo_SalaryType.Items.Clear()
        cbo_SalaryType.Items.Add("")
        cbo_SalaryType.Items.Add("MONTHLY")
        cbo_SalaryType.Items.Add("WEEKLY")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalaryType.GotFocus, AddressOf ControlGotFocus
        AddHandler Dtp_FromDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Month.GotFocus, AddressOf ControlGotFocus
        AddHandler Dtp_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FestivalDays.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FilterBillNo.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus



        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalaryType.LostFocus, AddressOf ControlLostFocus
        AddHandler Dtp_FromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Month.LostFocus, AddressOf ControlLostFocus
        AddHandler Dtp_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FestivalDays.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_FilterBillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Dtp_FromDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Dtp_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FilterBillNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Month.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalDays.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_FestivalDays.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Dtp_FromDate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Month.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FilterBillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Dtp_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_FestivalDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Salary_Payment_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Salary_Payment_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next


        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            Dtp_ToDate.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(40)

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            'If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
            '    Throw New ApplicationException("Error on Voucher Bill Deletion")
            'End If

            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            'cmd.CommandText = "Delete from Stock_Chemical_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Salary_Payment_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Salary_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"




            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            txt_FilterBillNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_Payment_No from Salary_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_Payment_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_Payment_No from Salary_Payment_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Salary_Payment_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Salary_Payment_No from Salary_Payment_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_Payment_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Salary_Payment_No from Salary_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Salary_Payment_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Salary_Payment_Head", "Salary_Payment_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red






        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Salary_Payment_No from Salary_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Salary_Payment_No from Salary_Payment_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Emp_ID As Integer = 0
        Dim Mth_IDNo As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If


        If Trim(cbo_SalaryType.Text) = "" Then
            MessageBox.Show("Invalid Salary Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalaryType.Enabled And cbo_SalaryType.Visible Then cbo_SalaryType.Focus()
            Exit Sub
        End If


        Mth_IDNo = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)
        If Val(Mth_IdNo) = 0 Then
            MessageBox.Show("Invalid Month", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Month.Enabled And cbo_Month.Visible Then cbo_Month.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Emp_ID = 0 Then
                        MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    ''If Val(.Rows(i).Cells(5).Value) = 0 Then
                    ''    MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ''    If .Enabled And .Visible Then
                    ''        .Focus()
                    ''        .CurrentCell = .Rows(i).Cells(5)
                    ''    End If
                    ''    Exit Sub
                    ''End If

                End If

            Next

        End With



        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Salary_Payment_Head", "Salary_Payment_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SalaryDate", dtp_Date.Value.Date)

            cmd.Parameters.AddWithValue("@FromDate", dtp_FromDate.Value.Date)

            cmd.Parameters.AddWithValue("@ToDate", dtp_ToDate.Value.Date)


            If New_Entry = True Then
                cmd.CommandText = "Insert into Salary_Payment_Head (       Salary_Payment_Code ,               Company_IdNo       ,           Salary_Payment_No    ,                               for_OrderBy           ,                 Salary_Payment_Date   ,               Salary_Type          ,               Month_IdNo  ,    From_Date ,  To_Date    ,           Total_Days              ,    Festival_Days ) " & _
                                    "     Values                  (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @SalaryDate    ,  '" & Trim(cbo_SalaryType.Text) & "',  " & Val(Mth_IDNo) & "     , @FromDate    , @ToDate     , " & Str(Val(txt_TotalDays.Text)) & ",  " & Str(Val(txt_FestivalDays.Text)) & " ) "
                cmd.ExecuteNonQuery()
            Else

                cmd.CommandText = "Update Salary_Payment_Head set Salary_Payment_Date = @SalaryDate, Salary_Type = '" & Trim(cbo_SalaryType.Text) & "',  Month_IdNo = " & Val(Mth_IDNo) & ", From_Date = @FromDate  ,  To_Date =  @ToDate   , Total_Days = " & Str(Val(txt_TotalDays.Text)) & ", Festival_Days = " & Str(Val(txt_FestivalDays.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            'EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            'PBlNo = Trim(txt_Month.Text)
            'Partcls = "Salary : Ref No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Salary_Payment_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Salary_Payment_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            ''cmd.CommandText = "Delete from Stock_Chemical_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            ''cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Emp_ID = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)


                        cmd.CommandText = "Insert into Salary_Payment_Details ( Salary_Payment_Code ,               Company_IdNo       ,   Salary_Payment_No    ,                     for_OrderBy                                            ,   Salary_Payment_Date  ,             Sl_No     ,              Employee_IdNo    ,                 Basic_Salary                      ,       Total_Days          ,                      Net_Pay                        ,                No_Of_Attendance_Days       ,                     From_W_off_CR        ,                  Festival_Holidays       ,       No_Of_Leave                           ,  Attendance_On_W_Off_FH                  ,                   Op_W_Off_CR             ,           Add_W_Off_CR                    ,             Less_W_Off_CR               ,                  Total_W_Off_CR             ,                  Salary_Days         ,                        Basic_Pay         ,                             D_A               ,              Earning                     ,         H_R_A                          ,                    Conveyance                     ,        Washing                             ,      Entertainment                          ,                     Maintenance           ,               Other_Addition         ,                      Total_Addition         ,                          Mess             ,                           Medical          ,                  Store                      ,                   ESI             ,                           P_F               ,                      E_P_F                   ,                  Pension_Scheme         ,                      Other_Deduction     ,                Total_Deduction         ,                   Attendance_Incentive      ,                     Net_Salary     ,                         Advance                  ,        Day_For_Bonus              ,                     Earning_For_Bonus ) " & _
                                            "     Values                 (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @SalaryDate            ,  " & Str(Val(Sno)) & ", " & Str(Val(Emp_ID)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ",  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ",  " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & ", " & Str(Val(.Rows(i).Cells(15).Value)) & ", " & Str(Val(.Rows(i).Cells(16).Value)) & "," & Str(Val(.Rows(i).Cells(17).Value)) & ", " & Str(Val(.Rows(i).Cells(18).Value)) & "," & Str(Val(.Rows(i).Cells(19).Value)) & ",  " & Str(Val(.Rows(i).Cells(20).Value)) & ",   " & Str(Val(.Rows(i).Cells(21).Value)) & ",  " & Str(Val(.Rows(i).Cells(22).Value)) & ",  " & Str(Val(.Rows(i).Cells(23).Value)) & ", " & Str(Val(.Rows(i).Cells(24).Value)) & ", " & Str(Val(.Rows(i).Cells(25).Value)) & ", " & Str(Val(.Rows(i).Cells(26).Value)) & "," & Str(Val(.Rows(i).Cells(27).Value)) & "," & Str(Val(.Rows(i).Cells(28).Value)) & ", " & Str(Val(.Rows(i).Cells(29).Value)) & "," & Str(Val(.Rows(i).Cells(30).Value)) & ", " & Str(Val(.Rows(i).Cells(31).Value)) & ", " & Str(Val(.Rows(i).Cells(32).Value)) & ", " & Str(Val(.Rows(i).Cells(33).Value)) & ", " & Str(Val(.Rows(i).Cells(34).Value)) & "," & Str(Val(.Rows(i).Cells(35).Value)) & ", " & Str(Val(.Rows(i).Cells(36).Value)) & "," & Str(Val(.Rows(i).Cells(37).Value)) & "," & Str(Val(.Rows(i).Cells(39).Value)) & "," & Str(Val(.Rows(i).Cells(40).Value)) & ") "
                        cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Insert into Stock_Chemical_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                              , Reference_Date,        Ledger_IdNo      ,      Party_Bill_No   ,           Sl_No      ,          Item_IdNo      ,                Quantity                  ,                      Rate                ,                      Amount               ) " & _
                        '                        "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @SalaryDate   , " & Str(Val(Led_ID)) & ", '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(Itm_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " )"
                        'cmd.ExecuteNonQuery()


                    End If

                Next

            End With


            ''Bill Posting
            'VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(txt_BillNo.Text), Agt_Idno, Val(CSng(lbl_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            'If Trim(UCase(VouBil)) = "ERROR" Then
            '    Throw New ApplicationException("Error on Voucher Bill Posting")
            'End If

            tr.Commit()
            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


        End Try

    End Sub

    Private Sub cbo_SalaryType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalaryType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalaryType, dtp_Date, cbo_Month, "", "", "", "")
    End Sub

    Private Sub cbo_SalaryType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalaryType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalaryType, cbo_Month, "", "", "", "")
    End Sub

    Private Sub cbo_Month_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Month.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Month, cbo_SalaryType, Dtp_FromDate, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
    End Sub

    Private Sub cbo_Month_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Month.KeyPress
        Dim Dt As Date
        Dim Mth_ID As Integer

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Month, dtp_FromDate, "Month_Head", "Month_Name", "", "(Month_IdNo = 0)")
        If Asc(e.KeyChar) = 13 And Trim(cbo_Month.Text) <> "" Then
            Mth_ID = Common_Procedures.Month_NameToIdNo(con, cbo_Month.Text)

            Dt = "01/" & Trim(Mth_ID) & "/" & IIf(Mth_ID >= 4, Year(Common_Procedures.Company_FromDate), Year(Common_Procedures.Company_ToDate))

            dtp_FromDate.Text = Dt

            Dt = DateAdd("m", 1, Dt)
            Dt = DateAdd("d", -1, Dt)

            dtp_ToDate.Text = Dt

        End If

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
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Salary_Payment_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Salary_Payment_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Salary_Payment_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If


            If Trim(txt_FilterBillNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bill_No = '" & Trim(txt_FilterBillNo.Text) & "' "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*,  c.Ledger_Name as PartyName from Salary_Payment_Head a  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Salary_Payment_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Salary_Payment_Head a INNER JOIN Salary_Payment_Details b ON a.Salary_Payment_Code = b.Salary_Payment_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Salary_Payment_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Salary_Payment_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Salary_Payment_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Salary_Payment_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        ' Common_Procedures.get_CashPartyName_From_All_Entries(con)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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
        'If dgv_Details.CurrentCell.ColumnIndex = 3 Or dgv_Details.CurrentCell.ColumnIndex = 4 Then
        '    get_MillCount_Details()
        'End If
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        'With dgv_Details
        '    'If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
        '    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
        '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
        '    Else
        '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
        '    End If
        '    'End If


        'End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        'On Error Resume Next

        'With dgv_Details
        '    If .Visible Then
        '        If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then

        '            Amount_Calculation(e.RowIndex, e.ColumnIndex)
        '            Quantity_Calculation(e.RowIndex, e.ColumnIndex)
        '        End If
        '    End If
        'End With

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub


    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime

    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

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

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_FestivalDays_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FestivalDays.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True

            Else
                btn_save.Focus()

            End If
        End If

        If e.KeyValue = 38 Then txt_TotalDays.Focus()

    End Sub

    Private Sub txt_TotalDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TotalDays.LostFocus
        txt_TotalDays.Text = Format(Val(txt_TotalDays.Text), "#########0.00")
    End Sub
    Private Sub txt_FestivalDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FestivalDays.LostFocus
        txt_FestivalDays.Text = Format(Val(txt_FestivalDays.Text), "#########0.00")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Private Sub txt_FestivalDays_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FestivalDays.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_VatAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TotalDays.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub cbo_SalaryType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalaryType.TextChanged
        Dim Dt As Date

        If Trim(UCase(cbo_SalaryType.Text)) = "WEEKLY" Then
            cbo_Month.Enabled = False
            Dt = dtp_Date.Text
            dtp_FromDate.Text = Dt

            Dt = DateAdd("d", 6, Dt)

            dtp_ToDate.Text = Dt

        Else
            cbo_Month.Enabled = True

        End If


    End Sub
End Class