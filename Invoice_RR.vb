Public Class Invoice_RR
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "INVRR-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private cmbLedNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_NoofMachines As Integer
    Private prn_DetMxIndx As Integer
    Private prn_DetAr(200, 10) As String
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_PG2 As Integer = 0
    Private prn_Count As Integer
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private BrnTfer_STS As Integer = 0
    Private Agst_STS As Integer = 0
    Private prn_DetIndx As Integer

    Private Property txt_Weight As Object

    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_AmountInWords.Text = "Rupees :                                                                               "

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

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

        lbl_Total.Text = ""
        lbl_AdditionalCopies.Text = ""
        lbl_ExtraCharges.Text = ""

        lbl_FreeCopies.Text = ""

        lbl_RateExtraCopy.Text = ""
        lbl_Rent.Text = ""
        lbl_RentMachine.Text = ""

        lbl_TotalFreeCopies.Text = ""
        lbl_NetAmount.Text = ""

        dgv_Details.Rows.Clear()



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

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_Cell_DeSelect()
        End If

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
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName  from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_DcNo.Text = dt1.Rows(0).Item("Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sales_Date").ToString
                'If IsDBNull(dt1.Rows(0).Item("LedgerName").ToString) = False Then

                '    If Trim(dt1.Rows(0).Item("LedgerName").ToString) <> "" Then

                '        If Val(dt1.Rows(0).Item("Ledger_IdNo").ToString) <> 1 Then
                '            cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString

                '        Else
                '            cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString

                '        End If

                '    Else
                '        cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString

                '    End If

                'Else

                '    cbo_Ledger.Text = dt1.Rows(0).Item("Cash_PartyName").ToString


                'End If
                cbo_Ledger.Text = dt1.Rows(0).Item("LedgerName").ToString
                dtp_OpeningDate.Text = dt1.Rows(0).Item("Opening_Date").ToString
                dtp_ClosingDate.Text = dt1.Rows(0).Item("Closing_Date").ToString
                lbl_RentMachine.Text = dt1.Rows(0).Item("Rent_Machine").ToString
                lbl_FreeCopies.Text = dt1.Rows(0).Item("Free_Copies_Machine").ToString
                lbl_RateExtraCopy.Text = dt1.Rows(0).Item("Rate_Extra_Copy").ToString
                lbl_Total.Text = dt1.Rows(0).Item("Total_Copies").ToString
                lbl_TotalFreeCopies.Text = dt1.Rows(0).Item("Total_Free_Copies").ToString
                lbl_AdditionalCopies.Text = dt1.Rows(0).Item("Additional_Copies").ToString
                lbl_Rent.Text = Format(Val(dt1.Rows(0).Item("Rent").ToString), "########0.00")
                lbl_ExtraCharges.Text = Format(Val(dt1.Rows(0).Item("Extra_Charges").ToString), "########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                'lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Machine_Name from Sales_Reading_Details a INNER JOIN Machine_Head b on a.Machine_IdNo = b.Machine_IdNo  where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()

                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_Details.Rows.Add()
                            SNo = SNo + 1
                            dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                            dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Machine_Name").ToString
                            dgv_Details.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Opening_Reading").ToString)
                            dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Closing_Reading").ToString)
                            dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Sub_Total_Copies").ToString)
                            dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Extra_Copies").ToString)

                        Next i

                    End If

                    For i = 0 To .Rows.Count - 1
                        dgv_Details.Rows(n).Cells(0).Value = i + 1
                    Next

                End With

                TotalAmount_Calculation()

                'SNo = SNo + 1
                'txt_SlNo.Text = Val(SNo)

                dt2.Clear()

            End If

            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            dt2.Dispose()

            da1.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub Delivery_Saara_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

    Private Sub Delivery_Saara_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_ClosingDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_OpeningDate.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_ClosingDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_OpeningDate.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus

        ' AddHandler dtp_ClosingDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_OpeningDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_OpeningDate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler dtp_ClosingDate.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub Delivery_Saara_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Delivery_Saara_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim tr As SqlClient.SqlTransaction

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Reading_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.ledger_idno = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"



            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""

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

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_No desc", con)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_DcNo.ForeColor = Color.Red



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            dt2.Dispose()
            da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Inv No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Inv No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Inv No.", "FOR NEW INVOICE INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Inv No.", "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim led_id As Integer = 0
        Dim saleac_id As Integer = 0
        Dim txac_id As Integer = 0
        Dim Mac_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim vTotCopi As Single = 0, vTotExtra As Single = 0, vTotWgt As Single = 0, vTotRls As Single = 0
        Dim vforOrdby As Single = 0
        Dim Amt As Single = 0

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

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Then

                    Mac_id = Common_Procedures.Machine_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Mac_id = 0 Then
                        MessageBox.Show("Invalid Machine Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    'If Val(.Rows(i).Cells(3).Value) = 0 Then
                    '    MessageBox.Show("Invalid Close reading", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(3)
                    '    End If
                    '    Exit Sub
                    'End If

                    If Val(.Rows(i).Cells(3).Value) < Val(.Rows(i).Cells(2).Value) Then
                        MessageBox.Show("Invalid VALUE", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        NetAmount_Calculation()
        'TotalAmount_Calculation()

        vTotCopi = 0 : vTotExtra = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotCopi = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotExtra = Format(Val(dgv_Details_Total.Rows(0).Cells(4).Value()), "########0.00")
            ' vTotWgt = Format(Val(dgv_Details_Total.Rows(0).Cells(8).Value()), "########0.00")

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Sales_Head", "Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvDate", dtp_Date.Value.Date)
            cmd.Parameters.AddWithValue("@OpnDate", dtp_OpeningDate.Value.Date)
            cmd.Parameters.AddWithValue("@ClsDate", dtp_ClosingDate.Value.Date)

            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sales_Head (  Sales_Code  ,              Company_IdNo        ,                Sales_No           ,             for_OrderBy    , Sales_Date,                                         Ledger_IdNo    ,           Opening_Date            ,   Closing_Date           ,                     Rent_Machine      ,           Free_Copies_Machine        ,                 Rate_Extra_Copy   ,                       Total_Copies      ,     Total_Free_Copies              ,                     Additional_Copies         ,                 Rent      ,              Extra_Charges                 ,             Net_Amount  ,            Sub_Total_Copies,        Total_Extra_Copies   ,Total_Machine  ) " & _
                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vforOrdby)) & ",              @InvDate,  " & Str(Val(led_id)) & ",          @OpnDate             ,    @ClsDate               ,  " & Str(Val(lbl_RentMachine.Text)) & ",    " & Val(lbl_FreeCopies.Text) & ", " & Str(Val(lbl_RateExtraCopy.Text)) & ", " & Val(lbl_Total.Text) & ", " & Str(Val(lbl_TotalFreeCopies.Text)) & ", " & Str(Val(lbl_AdditionalCopies.Text)) & ", " & Str(Val(lbl_Rent.Text)) & "," & Str(Val(lbl_ExtraCharges.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & "," & Str(Val(vTotCopi)) & "," & Str(Val(vTotExtra)) & ", " & Str(Val(lbl_TotalMachine.Text)) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Sales_Head set Sales_Date = @InvDate,  Ledger_IdNo = " & Str(Val(led_id)) & ", Opening_Date = @opnDate, Closing_Date = @ClsDate, Rent_Machine = " & Val(lbl_RentMachine.Text) & ",  Free_Copies_Machine = " & Str(Val(lbl_FreeCopies.Text)) & ", Rate_Extra_Copy = " & Val(lbl_RateExtraCopy.Text) & ", Total_Copies = " & Str(Val(lbl_Total.Text)) & ", Total_Free_Copies = " & Val(lbl_TotalFreeCopies.Text) & ",Additional_Copies = " & Str(Val(lbl_AdditionalCopies.Text)) & ", Rent = " & Str(Val(lbl_Rent.Text)) & ", Extra_Charges = " & Str(Val(lbl_ExtraCharges.Text)) & ",  Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Sub_Total_Copies = " & Str(Val(vTotCopi)) & ", Total_Extra_Copies = " & Str(Val(vTotExtra)) & " ,Total_Machine = " & Str(Val(lbl_TotalMachine.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Sales_Reading_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                For i = 0 To .Rows.Count - 1

                    Mac_id = Common_Procedures.Machine_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(Mac_id) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Sales_Reading_Details ( Sales_Code                 ,             Company_IdNo         ,               Sales_No            ,           for_OrderBy      , Sales_Date,            Sl_No     ,      Machine_IdNo  ,                          Opening_Reading       ,                   Closing_Reading                 ,                 Sub_Total_Copies      ,            Extra_Copies                     ) " & _
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vforOrdby)) & ", @InvDate,              " & Str(Val(Sno)) & ",  " & Str(Val(Mac_id)) & ",   " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & "," & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With


            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Ac_id = led_id
            saleac_id = 22

            cmd.CommandText = "Insert into Voucher_Head (     Voucher_Code            ,          For_OrderByCode   ,             Company_IdNo         ,           Voucher_No         ,             For_OrderBy    , Voucher_Type, Voucher_Date,     Debtor_Idno    ,          Creditor_Idno     ,                Total_VoucherAmount        ,         Narration                           , Indicate,       Year_For_Report                                     ,       Entry_Identification                  , Voucher_Receipt_Code ) " & _
                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vforOrdby)) & ", 'Sales' ,   @InvDate, " & Str(Val(Ac_id)) & ", " & Str(Val(saleac_id)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ",    'Bill No . : " & Trim(lbl_DcNo.Text) & "',    1    , " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "',          ''          ) "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Voucher_Details (       Voucher_Code                   ,          For_OrderByCode   ,              Company_IdNo        ,           Voucher_No         ,           For_OrderBy      , Voucher_Type, Voucher_Date, SL_No,        Ledger_IdNo     ,                       Voucher_Amount           ,              Narration                   ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              "   Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vforOrdby)) & ",  'Sales'    ,  @InvDate ,   1  , " & Str(Val(Ac_id)) & ", " & Str(-1 * Val(CSng(lbl_NetAmount.Text))) & ", 'Bill No . : " & Trim(lbl_DcNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            cmd.ExecuteNonQuery()

            Amt = CSng(lbl_NetAmount.Text)

            cmd.CommandText = "Insert into Voucher_Details (      Voucher_Code                  ,          For_OrderByCode   ,             Company_IdNo         ,           Voucher_No         ,           For_OrderBy      , Voucher_Type, Voucher_Date, SL_No,          Ledger_IdNo       ,     Voucher_Amount   ,     Narration                            ,             Year_For_Report                               ,           Entry_Identification               ) " & _
                              " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vforOrdby)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vforOrdby)) & ",  'Sales'    ,  @InvDate ,   2  , " & Str(Val(saleac_id)) & ", " & Str(Val(Amt)) & ", 'Bill No . : " & Trim(lbl_DcNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
            cmd.ExecuteNonQuery()


            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            move_record(lbl_DcNo.Text)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()
            tr.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub




    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Try
            cbo_Ledger.Tag = cbo_Ledger.Text
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, dtp_OpeningDate, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, dtp_OpeningDate, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        Dim Led_IdNo As Integer, Mac_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Mac_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If



            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Copies").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Extra_Charges").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            dt2.Dispose()
            da.Dispose()

        End Try

        If dgv_Filter_Details.Rows.Count > 0 Then
            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
        Else
            dtp_Filter_Fromdate.Focus()
        End If

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")
            If Asc(e.KeyChar) = 13 Then
                btn_Filter_Show_Click(sender, e)
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub Open_FilterEntry()
        Dim movno As String

        If dgv_Filter_Details.Rows.Count > 0 Then
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

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

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim i As Integer = 0

        Try
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                        Amount_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)

                    End If
                End If
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer = 0

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    n = .CurrentRow.Index
                    .Rows.RemoveAt(n)

                    For i = 0 To .Rows.Count - 1
                        .Rows(n).Cells(0).Value = i + 1
                    Next

                End With

                TotalAmount_Calculation()

            End If

        Catch ex As Exception
            '----
        End Try



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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub TotalAmount_Calculation()
        Dim Sno As Integer
        Dim TotExtra As Decimal
        Dim TotCopi As Decimal

        Sno = 0
        TotExtra = 0
        TotCopi = 0
        For i = 0 To dgv_Details.RowCount - 1
            Sno = Sno + 1
            dgv_Details.Rows(i).Cells(0).Value = Sno

            If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then
                TotExtra = TotExtra + Val(dgv_Details.Rows(i).Cells(5).Value)
                TotCopi = TotCopi + Val(dgv_Details.Rows(i).Cells(4).Value)

            End If

        Next

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotExtra)
            .Rows(0).Cells(4).Value = Format(Val(TotCopi), "########0.00")

        End With

        lbl_Total.Text = Format(TotCopi, "########0.00")

        Addition_Extra_Charges_Calculation()

    End Sub

    Private Sub Addition_Extra_Charges_Calculation()
        ' Dim Tot_Mac As Integer
        'lbl_TotalFreeCopies.Text = Format(Val(to) * Val(lbshDiscPerc.Text) / 100, "#########0.00")

        If Val(lbl_Total.Text) - Val(lbl_TotalFreeCopies.Text) > 0 Then
            lbl_AdditionalCopies.Text = Val(lbl_Total.Text) - Val(lbl_TotalFreeCopies.Text)
        Else
            lbl_AdditionalCopies.Text = 0
        End If

        If Val(lbl_AdditionalCopies.Text) > 0 Then
            lbl_ExtraCharges.Text = Format(Val(lbl_RateExtraCopy.Text) * Val(lbl_AdditionalCopies.Text), "#########0.00")
        Else
            lbl_ExtraCharges.Text = 0
        End If

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Decimal

        NtAmt = Val(lbl_Rent.Text) + Val(lbl_ExtraCharges.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        ' lbl_ExtraCharges.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")

        lbl_AmountInWords.Text = "Rupees :                                                                               "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees : : " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        'Dim bytes As Byte() = PrintDocument1.Print("pdf")
        ''Dim bytes As Byte() = RptViewer.LocalReport.Render("Excel")
        'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.xls")

        ''Dim bytes As Byte() = RptViewer.LocalReport.Render("Pdf")
        ''Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.pdf")
        'fs.Write(bytes, 0, bytes.Length)
        'fs.Close()
        'MessageBox.Show("ok")

        prn_InpOpts = ""
        If Trim(Common_Procedures.settings.CustomerCode) = "1039" Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        Else

            'prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. ExtraCopy" & Chr(13) & "                  4. All                         ", "FOR INVOICE PRINTING...", "12")

            'prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "123")

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Print_PDF_Status = True Then
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDFv7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else

                    PrintDocument1.Print()

                    'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

                    '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                    '    'PrintDocument1.DocumentName = "c:\test1.pdf"
                    '    'PrintDocument1.Print()

                    '    'Dim bytes As Byte() = PrintDocument1.Print("pdf")
                    '    ''Dim bytes As Byte() = RptViewer.LocalReport.Render("Excel")
                    '    'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.xls")

                    '    'Dim bytes As Byte() = System.IO.File.ReadAllBytes("C:\test1.pdf")
                    '    'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.pdf")
                    '    'fs.Write(bytes, 0, bytes.Length)
                    '    'fs.Close()
                    '    'MessageBox.Show("ok")

                    '    'PrintDocument1.Print()
                    'End If

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

        Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim I As Integer
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0 '1
        DetSNo = 0
        prn_NoofMachines = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_Count = 0
        prn_PG2 = 0

        Erase prn_DetAr

        prn_DetAr = New String(200, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sales_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Machine_Name ,B.Description from Sales_Reading_Details a INNER JOIN Machine_Head b on a.Machine_IdNo = b.Machine_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For I = 0 To prn_DetDt.Rows.Count - 1
                        prn_NoofMachines = prn_NoofMachines + 1
                    Next
                End If

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            da2.Dispose()

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If prn_PG2 = 1 Then
            Printing_Format2(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        'Dim p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        'Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim SNo As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50
            .Top = 175 ' 125
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        TxtHgt = 21 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        NoofItems_PerPage = 16 ' 19
        If prn_NoofMachines <= 1 Then
            NoofItems_PerPage = 15
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 60
        ClArr(2) = 450
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    CurY = CurY + TxtHgt
                    SNo = SNo + 1
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 15, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Rent", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Rent").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt + 10
                    If Val(prn_HdDt.Rows(0).Item("Extra_Charges").ToString) <> 0 Then
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Extra Charges", LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Extra_Charges").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                    End If

                    NoofDets = NoofDets + 1

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    If prn_NoofMachines > 1 Then

                        prn_PG2 = 1
                        e.HasMorePages = True
                        Return
                    End If

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
        Dim Cmp_Name As String = "", Cmp_Add1 As String = "", Cmp_Add2 As String = ""
        Dim Cmp_PhNo As String = "", Cmp_TinNo As String = "", Cmp_CstNo As String = ""
        Dim strHeight As Single = 0
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim PnAr() As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String = "", Cmp_Email As String = ""
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim W3 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String = ""
        Dim strWidth As String = 0
        Dim CurX As Single = 0
        Dim OpReadng As Double = 0
        Dim CloReadng As Double = 0


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.* from Sales_Reading_Details a INNER JOIN Machine_Head b on a.Machine_IdNo = b.Machine_IdNo  where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        dt2 = New DataTable
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False


        'p1Font = New Font("Calibri", 12, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ''CurY = CurY + TxtHgt '+ 10
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(1) = CurY

        'Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        'Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        'Cmp_Desc = "" : Cmp_Email = ""

        'Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)
        ''If Val(prn_HdDt.Rows(0).Item("Ro_Division_Status").ToString) = 1 Then
        ''    Cmp_Name = Trim(Cmp_Name) & " (RO Division)"
        ''End If

        'Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) '& IIf(Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        'If Trim(Cmp_Add1) <> "" Then
        '    If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
        '        Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        '    Else
        '        Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        '    End If
        'Else
        '    Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        'End If

        'Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) '& IIf(Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        'If Trim(Cmp_Add2) <> "" Then
        '    If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
        '        Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        '    Else
        '        Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        '    End If
        'Else
        '    Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        'End If

        ''If Val(prn_HdDt.Rows(0).Item("Ro_Division_Status").ToString) = 1 Then
        ''    Cmp_PhNo = "PHONE : 99426 17009"
        ''Else
        'If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
        '    Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        'End If
        '' End If

        'If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
        '    Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
        '    Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
        '    Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
        '    Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        'End If

        'CurY = CurY + TxtHgt - 10
        'p1Font = New Font("Calibri", 18, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        'strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1011" Then '---- Chellam Batteries (Thekkalur)
        ' If Trim(UCase(prn_OriDupTri)) = "ORIGINAL" Then
        ' e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.company_logo2, Drawing.Image), LMargin + 20, CurY, 75, 75)


        'CurY = CurY + strHeight
        'End If

        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt

        'strWidth = e.Graphics.MeasureString(Cmp_PhNo & "      " & Cmp_Email, pFont).Width

        'If PrintWidth > strWidth Then
        '    CurX = LMargin + (PrintWidth - strWidth) / 2
        'Else
        '    CurX = LMargin
        'End If

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, CurX, CurY, 0, PrintWidth, pFont)
        'strWidth = e.Graphics.MeasureString(Cmp_PhNo, pFont).Width
        'CurX = CurX + strWidth
        'Common_Procedures.Print_To_PrintDocument(e, "      " & Cmp_Email, CurX, CurY, 0, PrintWidth, pFont)

        ''Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "      " & Cmp_Email, LMargin, CurY, 2, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = ""

            If Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString) <> "" Then
                PnAr = Split(Trim(prn_HdDt.Rows(0).Item("Cash_PartyName").ToString), ",")

                If UBound(PnAr) >= 0 Then Led_Name = IIf(Trim(LCase(PnAr(0))) <> "cash", "M/s. ", "") & Trim(PnAr(0))
                If UBound(PnAr) >= 1 Then Led_Add1 = Trim(PnAr(1))
                If UBound(PnAr) >= 2 Then Led_Add2 = Trim(PnAr(2))
                If UBound(PnAr) >= 3 Then Led_Add3 = Trim(PnAr(3))
                If UBound(PnAr) >= 4 Then Led_Add4 = Trim(PnAr(4))
                If UBound(PnAr) >= 5 Then Led_TinNo = Trim(PnAr(5))

            Else

                Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

                Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
                Led_PhNo = Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)


                'Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
                'Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
                'Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)
                'Led_Add4 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
                'Led_TinNo = Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)

            End If

            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            If Trim(Led_Add4) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add4
            End If

            'If Trim(Led_PhNo) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = "Phone No : " & Led_PhNo
            'End If

            If Trim(Led_TinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = "Tin No : " & Led_TinNo
            End If


            Cen1 = ClAr(1) + 300
            W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
            W2 = e.Graphics.MeasureString("TO:", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            If prn_NoofMachines <= 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "Model No", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Machine_Name").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Description").ToString, LMargin + Cen1 + W1 + 10, CurY + TxtHgt, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, CurY + 25, PageWidth, CurY + 25)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2, CurY - TxtHgt, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Opening Date.", LMargin + Cen1 + 10, CurY + 7, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 7, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Opening_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, CurY + 7, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2, CurY - TxtHgt, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Closing Date", LMargin + Cen1 + 10, CurY + 7, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY + 7, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Closing_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, CurY + 7, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2, CurY - TxtHgt, 0, 0, pFont)

            CurY = CurY + TxtHgt - 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + TxtHgt - 5
            W3 = e.Graphics.MeasureString("ADDITIONAL COPIES :", pFont).Width

            If prn_NoofMachines <= 1 Then

                OpReadng = 0
                CloReadng = 0
                If prn_DetDt.Rows.Count > 0 Then
                    OpReadng = Val(prn_DetDt.Rows(0).Item("Opening_Reading").ToString)
                    CloReadng = Val(prn_DetDt.Rows(0).Item("Closing_Reading").ToString)
                End If

                Common_Procedures.Print_To_PrintDocument(e, "Opening Reading", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, OpReadng, LMargin + W3 + 15, CurY, 0, 0, p1Font)


                Common_Procedures.Print_To_PrintDocument(e, "Rent(" & prn_HdDt.Rows(0).Item("Total_Machine").ToString & "Machine)", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rent").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt

            End If

            Common_Procedures.Print_To_PrintDocument(e, "Closing Reading", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, CloReadng, LMargin + W3 + 15, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Free Copies", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Free_Copies").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Total Copies", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Copies").ToString, LMargin + W3 + 15, CurY, 0, 0, p1Font)

            '  CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Rate/Copy", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "[" & Trim(prn_DetDt.Rows(DetIndx).Item("Gms").ToString) & "Gsm,", LMargin + ClArr(1) + 15, CurY, 0, 0, pFont)
           
          
            If Val(prn_HdDt.Rows(0).Item("Rate_Extra_Copy").ToString) >= 1 Then

                Common_Procedures.Print_To_PrintDocument(e, "Rate/Copy", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Rs. " & prn_HdDt.Rows(0).Item("Rate_Extra_Copy").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)

            Else

                Common_Procedures.Print_To_PrintDocument(e, "Rate/Copy", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_Extra_Copy").ToString & " Ps", LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)

            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Additional Copies", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Additional_Copies").ToString, LMargin + W3 + 15, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 300, LnAr(4), LMargin + ClAr(1) + 300, LnAr(1))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer = 0
        Dim Yax As Single = 0
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(6), LMargin, LnAr(4))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(6), LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(6), LMargin + ClAr(1) + ClAr(2), LnAr(4))

            p1Font = New Font("Calibri", 12, FontStyle.Bold)


            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL ", LMargin + 400, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(7), LMargin + ClAr(1), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(7), LMargin + ClAr(1) + ClAr(2), LnAr(6))

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

            CurY = CurY
            Common_Procedures.Print_To_PrintDocument(e, "Rupees : " & Rup1, LMargin + 10, CurY, 0, 0, pFont)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "         " & Rup2, LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "Authorized Signature", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            If prn_NoofMachines > 1 Then
                CurY = CurY + TxtHgt - 5
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Page 1 of 2", PageWidth - 15, CurY, 1, 0, p1Font)
            End If

            'Jurs = Common_Procedures.settings.Jurisdiction
            'If Trim(Jurs) = "" Then Jurs = "Tirupur"
            'Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Jurs & " Jurisdiction", LMargin, CurY, 2, PrintWidth, p1Font)
            'If Print_PDF_Status = True Then
            '    CurY = CurY + TxtHgt - 15
            '    p1Font = New Font("Calibri", 9, FontStyle.Regular)
            '    Common_Procedures.Print_To_PrintDocument(e, "This computer generated invoice, so need sign", LMargin + 10, CurY, 0, 0, p1Font)
            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0
        Dim W1 As Single = 0, N1 As Single = 0, M1 As Single = 0
        Dim Arr(300, 5) As String
        Dim I As Integer, K As Integer, NoofDets As Integer = 0
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String = ""
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim ps As Printing.PaperSize
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim SNo As Integer = 0
        Dim pFont As Font
        Dim TotCopies As Long, TotExCopies As Long

        pFont = New Font("Calibri", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "Page : 2", PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50
            .Top = 175 ' 125
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        TxtHgt = 22 ' 21 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 260
        ClArr(2) = 110 : ClArr(3) = 110 : ClArr(4) = 110
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

        'ClArr(0) = 0
        'ClArr(1) = 120
        'ClArr(2) = 150 : ClArr(3) = 150 : ClArr(4) = 150
        'ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

        CurY = TMargin

        'If prn_DetMxIndx > (2 * NoofItems_PerPage) Then

        ' End If
        prn_DetIndx = 0

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "MACHINE NAME", LMargin, CurY, 2, ClArr(1), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "OPENING ", LMargin + ClArr(1), CurY, 2, ClArr(2), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "CLOSING ", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL ", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "EXTRA ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin, CurY, 2, ClArr(1), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "READING", LMargin + ClArr(1), CurY, 2, ClArr(2), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "READING", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "COPIES", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "COPIES", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), p1Font)

        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            TotCopies = 0 : TotExCopies = 0

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                    CurY = CurY + TxtHgt - 10

                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Machine_Name").ToString)
                    ItmNm2 = ""
                    If Len(ItmNm1) > 32 Then
                        For K = 32 To 1 Step -1
                            If Mid$(Trim(ItmNm1), K, 1) = " " Or Mid$(Trim(ItmNm1), K, 1) = "," Or Mid$(Trim(ItmNm1), K, 1) = "." Or Mid$(Trim(ItmNm1), K, 1) = "-" Or Mid$(Trim(ItmNm1), K, 1) = "/" Or Mid$(Trim(ItmNm1), K, 1) = "_" Or Mid$(Trim(ItmNm1), K, 1) = "(" Or Mid$(Trim(ItmNm1), K, 1) = ")" Or Mid$(Trim(ItmNm1), K, 1) = "\" Or Mid$(Trim(ItmNm1), K, 1) = "[" Or Mid$(Trim(ItmNm1), K, 1) = "]" Or Mid$(Trim(ItmNm1), K, 1) = "{" Or Mid$(Trim(ItmNm1), K, 1) = "}" Then Exit For
                        Next K
                        If K = 0 Then K = 32
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - K)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), K - 1)
                    End If

                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Machine_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Opening_Reading").ToString, LMargin + ClArr(1) + ClArr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Closing_Reading").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sub_Total_Copies").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Extra_Copies").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                    TotCopies = TotCopies + Val(prn_DetDt.Rows(prn_DetIndx).Item("Sub_Total_Copies").ToString)
                    TotExCopies = TotExCopies + Val(prn_DetDt.Rows(prn_DetIndx).Item("Extra_Copies").ToString)


                    If Trim(ItmNm2) <> "" Then
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, ItmNm2, LMargin + 10, CurY, 0, 0, pFont)
                    End If

                    CurY = CurY + TxtHgt + 15
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                    prn_DetIndx = prn_DetIndx + 1
                Loop
            End If

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(3) = CurY
            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Val(TotCopies), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Val(TotExCopies), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(1))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(1))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(1))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(1))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(1))


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            If prn_NoofMachines > 1 Then
                CurY = CurY + TxtHgt + TxtHgt - 15
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Page 2 of 2", PageWidth - 15, CurY, 1, 0, p1Font)
            End If

            e.HasMorePages = False

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Ledger_Reading_Details()
        Dim q As Single = 0
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim LedID As Integer
        Dim n As Integer
        Dim New_Code As String = ""

        Try

            If IsDate(dtp_OpeningDate.Text) = False Then
                MessageBox.Show("Invalid Opening Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dtp_OpeningDate.Enabled Then dtp_OpeningDate.Focus()
                Exit Sub
            End If

            If IsDate(dtp_ClosingDate.Text) = False Then
                MessageBox.Show("Invalid Closing Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dtp_ClosingDate.Enabled Then dtp_ClosingDate.Focus()
                Exit Sub
            End If

            If Not (dtp_ClosingDate.Value.Date >= dtp_OpeningDate.Value.Date) Then
                MessageBox.Show("Invalid Closing Date, Should greater than or equal to opening date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dtp_ClosingDate.Enabled Then dtp_ClosingDate.Focus()
                Exit Sub
            End If

            LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            If LedID <> 0 Then

                dgv_Details.Rows.Clear()

                Cmd.Connection = con

                Cmd.Parameters.Clear()
                Cmd.Parameters.AddWithValue("@FromDate", dtp_OpeningDate.Value.Date)
                Cmd.Parameters.AddWithValue("@ToDate", dtp_ClosingDate.Value.Date)

                New_Code = ""
                If New_Entry = True Then
                    Cmd.CommandText = "select top 1 a.*, b.*, b.Total_Machine as Ledger_TotalMachine from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_Idno <> 0 and a.Ledger_Idno = b.Ledger_IdNo where a.Ledger_idno = " & Str(Val(LedID)) & " and Opening_Date < @FromDate and Opening_Date < @ToDate and Closing_Date < @FromDate and Closing_Date < @ToDate Order by a.Sales_Date desc, a.for_orderby desc, a.Sales_No desc"
                    Da = New SqlClient.SqlDataAdapter(Cmd)
                    'Da = New SqlClient.SqlDataAdapter("select top 1 a.*, b.*, b.Total_Machine as Ledger_TotalMachine from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_Idno <> 0 and a.Ledger_Idno = b.Ledger_IdNo where a.Ledger_idno = " & Str(Val(LedID)) & " Order by a.Sales_Date desc, a.for_orderby desc, a.Sales_No desc", con)
                    Dt = New DataTable
                    Da.Fill(Dt)
                    If Dt.Rows.Count > 0 Then
                        New_Code = Dt.Rows(0).Item("Sales_Code").ToString
                    End If
                    Dt.Clear()

                Else

                    New_Code = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                End If


                Cmd.CommandText = "select top 1 a.*, b.*, b.Total_Machine as Ledger_TotalMachine from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_Idno <> 0 and a.Ledger_Idno = b.Ledger_IdNo where a.Ledger_idno = " & Str(Val(LedID)) & " and a.Sales_Code = '" & Trim(New_Code) & "' Order by a.Sales_Date desc, a.for_orderby desc, a.Sales_No desc"
                'Cmd.CommandText = "select top 1 a.*, b.*, b.Total_Machine as Ledger_TotalMachine from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_Idno <> 0 and a.Ledger_Idno = b.Ledger_IdNo where a.Ledger_idno = " & Str(Val(LedID)) & " and Opening_Date < @FromDate and Opening_Date < @ToDate and Closing_Date < @FromDate and Closing_Date < @ToDate Order by a.Sales_Date desc, a.for_orderby desc, a.Sales_No desc"
                Da = New SqlClient.SqlDataAdapter(Cmd)
                'Da = New SqlClient.SqlDataAdapter("select top 1 a.*, b.*, b.Total_Machine as Ledger_TotalMachine from Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_Idno <> 0 and a.Ledger_Idno = b.Ledger_IdNo where a.Ledger_idno = " & Str(Val(LedID)) & " Order by a.Sales_Date desc, a.for_orderby desc, a.Sales_No desc", con)
                Dt = New DataTable
                Da.Fill(Dt)

                If Dt.Rows.Count > 0 Then

                    lbl_RentMachine.Text = Dt.Rows(0).Item("Rent_Machine").ToString
                    lbl_FreeCopies.Text = Dt.Rows(0).Item("Free_Copies_Machine").ToString
                    lbl_RateExtraCopy.Text = Dt.Rows(0).Item("Rate_Extra_Copy").ToString
                    lbl_TotalMachine.Text = Dt.Rows(0).Item("Ledger_TotalMachine").ToString

                    'For i = 0 To Dt.Rows.Count - 1
                    '    lbl_RentMachine.Text = Dt.Rows(i).Item("Rent_Machine").ToString
                    '    lbl_FreeCopies.Text = Dt.Rows(i).Item("Free_Copies_Machine").ToString
                    '    lbl_RateExtraCopy.Text = Dt.Rows(i).Item("Rate_Extra_Copy").ToString
                    '    lbl_TotalMachine.Text = Dt.Rows(i).Item("Ledger_TotalMachine").ToString
                    '    sls_Cd = Dt.Rows(i).Item("Sales_Code").ToString
                    'Next i

                    Da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Machine_Name from Ledger_Reading_Details a LEFT OUTER JOIN Sales_Reading_Details b ON a.Machine_Idno <> 0 and b.Sales_Code = '" & Trim(Dt.Rows(0).Item("Sales_Code").ToString) & "' and a.Machine_Idno = b.Machine_IdNo INNER JOIN Machine_Head c ON a.Machine_Idno <> 0 and a.Machine_Idno = c.Machine_IdNo where a.Ledger_idno = " & Str(Val(LedID)) & " Order by a.sl_no", con)
                    'Da1 = New SqlClient.SqlDataAdapter("select  a.*, c.Machine_Name from Sales_Reading_Details a LEFT OUTER JOIN Machine_Head C ON A.Machine_Idno = C.Machine_IdNo where a.Sales_Code = '" & Trim(sls_Cd) & "'Order by c.Machine_Name ", con)
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)
                    With dgv_Details
                        If Dt1.Rows.Count > 0 Then
                            For i = 0 To Dt1.Rows.Count - 1
                                n = dgv_Details.Rows.Add()
                                dgv_Details.Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Machine_Name").ToString
                                dgv_Details.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Closing_Reading").ToString
                            Next i
                        End If

                    End With
                    Dt1.Clear()

                Else

                    Da2 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Machine_Name from Ledger_Reading_Details a INNER JOIN Ledger_Head b ON a.Ledger_Idno <> 0 and a.Ledger_Idno = b.Ledger_IdNo INNER JOIN Machine_Head c ON a.Machine_Idno <> 0 and a.Machine_Idno = C.Machine_IdNo where a.Ledger_idno = " & Str(Val(LedID)) & " Order by a.sl_no", con)
                    Dt2 = New DataTable
                    Da2.Fill(Dt2)
                    With dgv_Details

                        If Dt2.Rows.Count > 0 Then

                            lbl_RentMachine.Text = Dt2.Rows(0).Item("Rent_Machine").ToString
                            lbl_FreeCopies.Text = Dt2.Rows(0).Item("Free_Copies_Machine").ToString
                            lbl_RateExtraCopy.Text = Dt2.Rows(0).Item("Rate_Extra_Copy").ToString
                            lbl_TotalMachine.Text = Dt2.Rows(0).Item("Total_Machine").ToString

                            For i = 0 To Dt2.Rows.Count - 1
                                n = dgv_Details.Rows.Add()


                                dgv_Details.Rows(n).Cells(1).Value = Dt2.Rows(i).Item("Machine_Name").ToString
                                dgv_Details.Rows(n).Cells(2).Value = Dt2.Rows(i).Item("Opening_Reading").ToString

                            Next i
                        End If

                        Dt2.Clear()
                        Dt2.Dispose()
                        Da2.Dispose()

                    End With

                End If
            End If

            lbl_TotalFreeCopies.Text = Val(lbl_TotalMachine.Text) * Val(lbl_FreeCopies.Text)
            lbl_Rent.Text = Val(lbl_TotalMachine.Text) * Val(lbl_RentMachine.Text)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            Dt1.Dispose()
            Da1.Dispose()

            Dt2.Dispose()
            Da2.Dispose()


        End Try

    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        Try
            If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then

                If IsDate(dtp_OpeningDate.Text) = False Then
                    'MessageBox.Show("Invalid Opening Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'If dtp_OpeningDate.Enabled Then dtp_OpeningDate.Focus()
                    Exit Sub
                End If

                If IsDate(dtp_ClosingDate.Text) = False Then
                    'MessageBox.Show("Invalid Closing Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'If dtp_ClosingDate.Enabled Then dtp_ClosingDate.Focus()
                    Exit Sub
                End If

                If Not (dtp_ClosingDate.Value.Date >= dtp_OpeningDate.Value.Date) Then
                    'MessageBox.Show("Invalid Closing Date, Should greater than or equal to opening date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'If dtp_ClosingDate.Enabled Then dtp_ClosingDate.Focus()
                    Exit Sub
                End If

                Ledger_Reading_Details()

            End If

        Catch ex As Exception
            '------

        End Try
    End Sub

    Private Sub dgv_details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Try
            With dgv_Details
                If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                    .Rows(.RowCount - 1).Cells(0).Value = .RowCount
                End If
            End With

        Catch ex As Exception
            '------
        End Try
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try

            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If

        Catch ex As Exception
            '-------

        End Try

    End Sub

    Private Sub dgv_countdetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Try
            dgv_details_CellLeave(sender, e)
            TotalAmount_Calculation()
        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            With dgv_Details

                If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                    .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
                End If

            End With

        Catch ex As Exception
            '----

        End Try



    End Sub

    Private Sub dgv_details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        'Try
        '    With dgv_Details
        '        If .Visible Then
        '            If .Rows.Count > 0 Then
        '                If .CurrentCell.ColumnIndex = 3 Then
        '                    If Val(.CurrentRow.Cells(3).Value) < Val(.CurrentRow.Cells(2).Value) Then
        '                        MessageBox.Show("Invalid VALUE", "DOES NOT VALID...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '                        .CurrentRow.Cells(3).Value = Val(.CurrentRow.Cells(2).Value)
        '                    End If
        '                End If
        '            End If
        '        End If

        '    End With

        'Catch ex As Exception
        '    '-----

        'End Try
    End Sub

    Private Sub dgv_details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

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

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(3)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= 3 Then
                        If .CurrentCell.RowIndex = 0 Then
                            dtp_ClosingDate.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(3)

                        End If

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)

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

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Try
            With dgv_Details

                If .Visible Then

                    If .Rows.Count > 0 Then

                        If CurCol = 2 Or CurCol = 3 Or CurCol = 4 Or CurCol = 5 Then

                            If CurCol = 2 Or CurCol = 3 Then

                                If Val(.Rows(CurRow).Cells(3).Value) <> 0 Then
                                    .Rows(CurRow).Cells(4).Value = Val(.Rows(CurRow).Cells(3).Value) - Val(.Rows(CurRow).Cells(2).Value)
                                Else
                                    .Rows(CurRow).Cells(4).Value = ""
                                End If

                                If Val(.Rows(CurRow).Cells(4).Value) - Val(lbl_FreeCopies.Text) > 0 Then
                                    .Rows(CurRow).Cells(5).Value = Val(.Rows(CurRow).Cells(4).Value) - Val(lbl_FreeCopies.Text)
                                Else
                                    .Rows(CurRow).Cells(5).Value = ""
                                End If

                            End If

                        End If

                        TotalAmount_Calculation()

                    End If

                End If

            End With

        Catch ex As Exception
            '-----
        End Try


    End Sub

    Private Sub dtp_ClosingDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_ClosingDate.KeyDown
        Try
            If e.KeyValue = 40 Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If


            End If
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dtp_ClosingDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ClosingDate.KeyPress
        Try
            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(dtp_OpeningDate.Tag)) <> Trim(UCase(dtp_OpeningDate.Text)) Or Trim(UCase(dtp_ClosingDate.Tag)) <> Trim(UCase(dtp_ClosingDate.Text)) Then
                    Ledger_Reading_Details()
                End If

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .CurrentCell.ColumnIndex = 3 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Ledger_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.TextChanged
        'dgv_Details.Rows.Clear()
    End Sub

    
    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Ledger_Reading_Details()

        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
            dgv_Details.CurrentCell.Selected = True
        Else
            dtp_ClosingDate.Focus()
        End If


    End Sub

    Private Sub dtp_ClosingDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ClosingDate.GotFocus
        Try
            dtp_OpeningDate.Tag = dtp_OpeningDate.Text
            dtp_ClosingDate.Tag = dtp_ClosingDate.Text

        Catch ex As Exception
            '------

        End Try
    End Sub

    Private Sub dtp_ClosingDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_ClosingDate.LostFocus
        'Try
        '    If Trim(UCase(dtp_OpeningDate.Tag)) <> Trim(UCase(dtp_OpeningDate.Text)) Or Trim(UCase(dtp_ClosingDate.Tag)) <> Trim(UCase(dtp_ClosingDate.Text)) Then
        '        Ledger_Reading_Details()
        '    End If

        'Catch ex As Exception
        '    '------

        'End Try
    End Sub

    Private Sub dtp_OpeningDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_OpeningDate.GotFocus
        Try
            dtp_OpeningDate.Tag = dtp_OpeningDate.Text
            dtp_ClosingDate.Tag = dtp_ClosingDate.Text

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub dtp_OpeningDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_OpeningDate.LostFocus
        Try
            If Trim(UCase(dtp_OpeningDate.Tag)) <> Trim(UCase(dtp_OpeningDate.Text)) Or Trim(UCase(dtp_ClosingDate.Tag)) <> Trim(UCase(dtp_ClosingDate.Text)) Then

                If IsDate(dtp_OpeningDate.Text) = False Then
                    'MessageBox.Show("Invalid Opening Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'If dtp_OpeningDate.Enabled Then dtp_OpeningDate.Focus()
                    Exit Sub
                End If

                If IsDate(dtp_ClosingDate.Text) = False Then
                    'MessageBox.Show("Invalid Closing Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'If dtp_ClosingDate.Enabled Then dtp_ClosingDate.Focus()
                    Exit Sub
                End If

                If Not (dtp_ClosingDate.Value.Date >= dtp_OpeningDate.Value.Date) Then
                    'MessageBox.Show("Invalid Closing Date, Should greater than or equal to opening date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'If dtp_ClosingDate.Enabled Then dtp_ClosingDate.Focus()
                    Exit Sub
                End If

                Ledger_Reading_Details()

            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub dgv_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_Pdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pdf.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        Print_PDF_Status = False
    End Sub
End Class