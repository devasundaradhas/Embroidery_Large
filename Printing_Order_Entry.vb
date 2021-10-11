Public Class Printing_Order_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PRORE-"
    Private Pk_Condition1 As String = "PRORA-"
    Private cbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private vdgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""
    Private vCloPic_STS As Boolean = False
    Private NoCalc_Status As Boolean = False
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer


    Private Sub clear()

        NoCalc_Status = True

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        New_Entry = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_date.Text = ""

        cbo_Ledger.Text = ""


        txt_ADVANCE.Text = ""
        txt_AdvDate.Text = ""

        txt_Remarks.Text = ""

        dgv_Details.Rows.Clear()

        dgv_Selection.Rows.Clear()

        cbo_Grid_Vareity.Text = False
        cbo_Grid_paper.Visible = False
        cbo_Grid_ink.Text = False
        cbo_Grid_Unit.Visible = False
        cbo_Grid_size.Visible = False

        dgv_INKColourDetails.Rows.Clear()
        dgv_INKColourDetails.Rows.Add()
        dgv_PaperColourDetails.Rows.Clear()
        dgv_PaperColourDetails.Rows.Add()

        dgv_InkAllDetails.Rows.Clear()
        dgv_InkAllDetails.Rows.Add()
        dgv_InkAllDetails.Enabled = True

        dgv_paperAllDetails.Rows.Clear()
        dgv_paperAllDetails.Rows.Add()
        dgv_paperAllDetails.Enabled = True

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Ledger.Text = ""
            cbo_Filter_Ledger.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        vdgv_DrawNo = ""
        vCbo_ItmNm = ""

        NoCalc_Status = False


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Vareity.Name Then
            cbo_Grid_Vareity.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_paper.Name Then
            cbo_Grid_paper.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_ink.Name Then
            cbo_Grid_ink.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Unit.Name Then
            cbo_Grid_Unit.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_size.Name Then
            cbo_Grid_size.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
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
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub Printing_Order_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Vareity.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VAREITY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Vareity.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_paper.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SLEEVE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_paper.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ink.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ink.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Printing_Order_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

        Me.Text = ""

        con.Open()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_INKColourDetails.Visible = False
        pnl_INKColourDetails.Left = (Me.Width - pnl_INKColourDetails.Width) \ 2
        pnl_INKColourDetails.Top = (Me.Height - pnl_INKColourDetails.Height) \ 2
        pnl_INKColourDetails.BringToFront()

        pnl_PaperColourDetails.Visible = False
        pnl_PaperColourDetails.Left = (Me.Width - pnl_PaperColourDetails.Width) \ 2
        pnl_PaperColourDetails.Top = (Me.Height - pnl_PaperColourDetails.Height) \ 2
        pnl_PaperColourDetails.BringToFront()

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Vareity.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_paper.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ink.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_size.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_ADVANCE.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AdvDate.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_AdvDate.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Vareity.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_paper.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ink.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_size.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ADVANCE.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AdvDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AdvDate.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub Printing_Order_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Printing_Order_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_PaperColourDetails.Visible = True Then
                    btn_Close_PaperColourDetails_Click(sender, e)
                    Exit Sub

                ElseIf pnl_INKColourDetails.Visible = True Then
                    btn_Close_INKColourDetails_Click(sender, e)
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

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_PaperColourDetails.Name Or ActiveControl.Name = dgv_INKColourDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            ElseIf ActiveControl.Name = dgv_PaperColourDetails.Name Then
                dgv1 = dgv_PaperColourDetails
            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_PaperColourDetails
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_PaperColourDetails

            ElseIf ActiveControl.Name = dgv_INKColourDetails.Name Then
                dgv1 = dgv_INKColourDetails
            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_INKColourDetails
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_INKColourDetails
         
            End If


            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 5 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_ADVANCE.Focus()
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(2)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    cbo_Ledger.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 5)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_paperAllDetails.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    btn_Close_PaperColourDetails.Focus()
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                    ' If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    'save_record()
                                    'Else
                                    ' txt_Name.Focus()
                                    ' End If
                                    btn_Close_PaperColourDetails.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                End If

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    btn_Close_PaperColourDetails.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True



                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_INKColourDetails.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    btn_Close_INKColourDetails.Focus()
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                    ' If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    'save_record()
                                    'Else
                                    ' txt_Name.Focus()
                                    ' End If
                                    btn_Close_INKColourDetails.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                End If

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then
                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    btn_Close_INKColourDetails.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True



                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    End If


                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim Cancel_Sts As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Printing_Order_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  Where a.Printing_Order_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Printing_Order_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Printing_Order_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString

                txt_ADVANCE.Text = Format(Val(dt1.Rows(0).Item("Advance").ToString), "#########0.00")
                txt_AdvDate.Text = dt1.Rows(0).Item("Advance_Date").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.* from Printing_Order_Details a  where a.Printing_Order_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        Cancel_Sts = 0
                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)


                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("oRDER_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Common_Procedures.Variety_IdNoToName(con, Val(dt2.Rows(i).Item("Variety_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Common_Procedures.Unit_IdNoToName(con, Val(dt2.Rows(i).Item("Unit_idNo").ToString))
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("NO_of_SET").ToString
                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("No_Of_Copies").ToString
                        dgv_Details.Rows(n).Cells(7).Value = Common_Procedures.Size_IdNoToName(con, Val(dt2.Rows(i).Item("Size_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("paper_Details").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("colour_Details").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Binding_No").ToString
                        dgv_Details.Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Printing_Order_Details_SlNo").ToString)

                        dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Order_Program_Code").ToString
                      
                        If Trim(dgv_Details.Rows(n).Cells(12).Value) <> "" Then
                            LockSTS = True
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(i).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        End If

                        dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Details_SlNo").ToString

                        If Val(dt2.Rows(i).Item("Cancel_Status").ToString) = 1 Then dgv_Details.Rows(n).Cells(14).Value = True
                        dgv_Details.Rows(n).Cells(15).Value = dt2.Rows(i).Item("Order_No_New").ToString

                    Next i

                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*  from Printing_Order_Paper_Details a  where a.Printing_Order_Code = '" & Trim(NewCode) & "'  Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt3)

                dgv_paperAllDetails.Rows.Clear()
                SNo = 0

                If dt3.Rows.Count > 0 Then

                    For j = 0 To dt3.Rows.Count - 1

                        n = dgv_paperAllDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_paperAllDetails.Rows(n).Cells(0).Value = Val(dt3.Rows(j).Item("Detail_SlNo").ToString)
                        dgv_paperAllDetails.Rows(n).Cells(1).Value = Common_Procedures.Sleeve_IdNoToName(con, Val(dt3.Rows(j).Item("Paper_IdNo").ToString))

                    Next j

                End If
                dt3.Clear()
                dt3.Dispose()

                da2 = New SqlClient.SqlDataAdapter("select a.*  from Printing_Order_colour_Details a  where a.Printing_Order_Code = '" & Trim(NewCode) & "'  Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt4)

                dgv_InkAllDetails.Rows.Clear()

                SNo = 0

                If dt4.Rows.Count > 0 Then

                    For k = 0 To dt4.Rows.Count - 1

                        n = dgv_InkAllDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_InkAllDetails.Rows(n).Cells(0).Value = Val(dt4.Rows(k).Item("Detail_SlNo").ToString)
                        dgv_InkAllDetails.Rows(n).Cells(1).Value = Common_Procedures.Colour_IdNoToName(con, Val(dt4.Rows(k).Item("Colour_IdNo").ToString))

                    Next k

                End If
                dt4.Clear()
                dt4.Dispose()

                If LockSTS = True Then
                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray

                End If

            End If

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

            If dtp_date.Visible And dtp_date.Enabled Then dtp_date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Printing_Order_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Printing_Order_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Printing_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "' and  Order_Program_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already ORDER Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Printing_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Printing_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Printing_Order_Paper_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Printing_Order_colour_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and AccountsGroup_IdNo = 14 ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_Ledger.DataSource = dt1
            cbo_Filter_Ledger.DisplayMember = "Ledger_DisplayName"

            cbo_Filter_Ledger.Text = ""
            cbo_Filter_Ledger.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If Filter_Status = False Then
            If dgv_Filter_Details.Rows.Count > 0 Then
                dgv_Filter_Details.Focus()
                dgv_Filter_Details.CurrentCell = dgv_Filter_Details.Rows(0).Cells(0)
                dgv_Filter_Details.CurrentCell.Selected = True

            Else
                If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

            End If

        Else
            If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

        End If


    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Printing_Order_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Printing_Order_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Ref No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Printing_Order_No from Printing_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(RefCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT PO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW NO ...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Printing_Order_No from Printing_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Printing_Order_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Printing_Order_No from Printing_Order_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Printing_Order_No", con)
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

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Printing_Order_No from Printing_Order_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Printing_Order_No desc", con)
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

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Printing_Order_No from Printing_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Printing_Order_No desc", con)
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

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Printing_Order_Head", "Printing_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            'da1 = New SqlClient.SqlDataAdapter("select Top 1  from Printing_Order_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Printing_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Printing_Order_No desc", con)
            'dt1 = New DataTable
            'Da1.Fill(Dt1)


            Dt1.Clear()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()

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

            Da = New SqlClient.SqlDataAdapter("select Printing_Order_No from Printing_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(RefCode) & "'", con)
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

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_IdNo As Integer = 0
        Dim Vrty_ID As Integer = 0
        Dim size_ID As Integer = 0
        Dim Unit_ID As Integer = 0
        Dim Paper_ID As Integer = 0
        Dim colr_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Spno As Integer = 0
        Dim Sino As Integer = 0
        Dim vTotQty As Single = 0
        Dim vTotAmt As Single = 0
        Dim PurcAc_ID As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim Cancel_Sts As Integer = 0
        Dim Nr As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        'Dim CsParNm As String

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Printing_Order_Entry, New_Entry) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Da = New SqlClient.SqlDataAdapter("select * from Printing_Order_Head where Ledger_IdNo = " & Str(Val(Led_IdNo)) & " and Bill_No = '" & Trim(lbl_RefNo.Text) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Printing_Order_Code <> '" & Trim(NewCode) & "'", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then


                'If dgv_Details.Rows(i).Cells(1).Value = "" Then
                '    MessageBox.Show("Invalid ORDER NO", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                '    End If
                '    Exit Sub
                'End If

                Vrty_ID = Common_Procedures.Variety_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value)
                If Vrty_ID = 0 Then
                    MessageBox.Show("Invalid VARIETY Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    End If
                    Exit Sub
                End If

                If dgv_Details.Rows(i).Cells(3).Value = 0 Then
                    MessageBox.Show("Invalid QUANTITY", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    End If
                    Exit Sub
                End If

                Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value)
                If Unit_ID = 0 Then
                    MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                    End If
                    Exit Sub
                End If

            End If

        Next

        NoCalc_Status = False


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Printing_Order_Head", "Printing_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OrdDate", dtp_date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Printing_Order_Head(Printing_Order_Code,             Company_IdNo         ,           Printing_Order_No   ,                               for_OrderBy                             , Printing_Order_Date,          Ledger_IdNo     ,      Advance  ,     Remarks    , Advance_Date    ) " & _
                                    "              Values   ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @OrdDate     ,  " & Str(Val(Led_IdNo)) & ", " & Str(Val(txt_ADVANCE.Text)) & ", '" & Trim(txt_Remarks.Text) & "' ,'" & Trim(txt_AdvDate.Text) & "'  )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Printing_Order_Head set Printing_Order_Date= @OrdDate,  Ledger_IdNo = " & Str(Val(Led_IdNo)) & " ,  Advance = " & Str(Val(txt_ADVANCE.Text)) & ",  Remarks = '" & Trim(txt_Remarks.Text) & "' , Advance_Date = '" & Trim(txt_AdvDate.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "PRINT Order : Ref.No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Printing_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "' and Order_Program_Code ='' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Printing_Order_Paper_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Printing_Order_Colour_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then
                        Cancel_Sts = 0
                        Sno = Sno + 1

                        Vrty_ID = Common_Procedures.Variety_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value, tr)
                        Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value, tr)

                        size_ID = Common_Procedures.Size_NameToIdNo(con, dgv_Details.Rows(i).Cells(7).Value, tr)

                        If dgv_Details.Rows(i).Cells(14).Value = True Then Cancel_Sts = 1

                        cmd.CommandText = "Update Printing_Order_Details set Printing_Order_Date= @OrdDate , Ledger_IdNo = " & Str(Val(Led_IdNo)) & ", oRDER_No = '" & Trim(.Rows(i).Cells(1).Value) & "', Sl_No = " & Str(Val(Sno)) & ", Variety_IdNo = " & Str(Val(Vrty_ID)) & ", Quantity = " & Str(Val(.Rows(i).Cells(3).Value)) & ", Unit_Idno = " & Val(Unit_ID) & ",NO_of_SET = '" & Trim(.Rows(i).Cells(5).Value) & "',No_Of_Copies = '" & Trim(.Rows(i).Cells(6).Value) & "', Size_IdNo = " & Str(Val(size_ID)) & ", Paper_Details =  '" & Trim(.Rows(i).Cells(8).Value) & "', Colour_Details = '" & Trim(.Rows(i).Cells(9).Value) & "', Binding_No = '" & Trim(.Rows(i).Cells(10).Value) & "' ,Order_No_New = '" & Trim(.Rows(i).Cells(15).Value) & "'  ,  Details_Slno = " & Str(Val(.Rows(i).Cells(13).Value)) & " , Cancel_Status = " & Str(Val(Cancel_Sts)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "' and Printing_Order_Details_SlNo = " & Str(Val(.Rows(i).Cells(11).Value))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Printing_Order_Details ( Printing_Order_Code, Company_IdNo                     , Printing_Order_No              , for_OrderBy                                                 , Printing_Order_Date, Ledger_IdNo                 , Sl_No                , oRDER_No                             ,  Variety_IdNo            , Quantity                                , Unit_idNo          , NO_of_SET                                      , No_Of_Copies                          , Size_IdNo                 , Paper_Details                 , Colour_Details             , Binding_No                       , Details_Slno              ,          Cancel_Status          , Order_No_New  ) " & _
                                                                      "Values  ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @OrdDate, " & Str(Val(Led_IdNo)) & ", " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(Vrty_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Val(Unit_ID) & ", '" & Trim(.Rows(i).Cells(5).Value) & "', '" & Trim(.Rows(i).Cells(6).Value) & "',  " & Str(Val(size_ID)) & ", '" & Trim(.Rows(i).Cells(8).Value) & "', '" & Trim(.Rows(i).Cells(9).Value) & "', '" & Trim(.Rows(i).Cells(10).Value) & "' , " & Str(Val(.Rows(i).Cells(13).Value)) & " , " & Str(Val(Cancel_Sts)) & " , '" & Trim(.Rows(i).Cells(15).Value) & "' )"
                            cmd.ExecuteNonQuery()
                        End If

                        With dgv_paperAllDetails

                            For j = 0 To .RowCount - 1

                                If Val(.Rows(j).Cells(0).Value) = Val(dgv_Details.Rows(i).Cells(13).Value) Then
                                    Paper_ID = Common_Procedures.Sleeve_NameToIdNo(con, .Rows(j).Cells(1).Value, tr)

                                    Spno = Spno + 1

                                    cmd.CommandText = "Insert into Printing_Order_Paper_Details( Printing_Order_Code, Company_IdNo  , Printing_Order_No  , for_OrderBy ,   Printing_Order_Date ,    Detail_SlNo , Sl_No   , Paper_IdNo ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @OrdDate ,  " & Val(.Rows(j).Cells(0).Value) & ", " & Val(Spno) & " , " & Val(Paper_ID) & " )"
                                    cmd.ExecuteNonQuery()

                                End If

                            Next j

                        End With

                        With dgv_InkAllDetails

                            For k = 0 To .RowCount - 1

                                If Val(.Rows(k).Cells(0).Value) = Val(dgv_Details.Rows(i).Cells(13).Value) Then

                                    colr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(k).Cells(1).Value, tr)

                                    Sino = Sino + 1

                                    cmd.CommandText = "Insert into Printing_Order_Colour_Details( Printing_Order_Code, Company_IdNo  , Printing_Order_No  , for_OrderBy ,  Printing_Order_Date ,    Detail_SlNo ,Sl_No   , Colour_IdNo ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @OrdDate,  " & Val(.Rows(k).Cells(0).Value) & ", " & Val(Sino) & " , " & Val(colr_ID) & " )"
                                    cmd.ExecuteNonQuery()

                                End If

                            Next k


                        End With

                    End If

                Next

            End With


            PurcAc_ID = 1

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            'If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            vLed_IdNos = Led_IdNo & "|" & PurcAc_ID
            vVou_Amts = Val(txt_ADVANCE.Text) & "|" & -1 * Val(txt_ADVANCE.Text)
            If Common_Procedures.Voucher_Updation(con, "ADVANCE", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(dtp_date.Text), "REF No : " & Trim(lbl_RefNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If
            'End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_date.Text, Led_IdNo, Trim(lbl_RefNo.Text), 0, Val((txt_ADVANCE.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


        Catch ex As Exception

            tr.Rollback()

            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()
            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        End Try

    End Sub

    Private Sub cbo_Grid_Vareity_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Vareity.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_HEAD", "Variety_name", "", "(Variety_idno = 0)")

    End Sub

    Private Sub cbo_Grid_Variety_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Vareity.KeyDown

        cbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Vareity, Nothing, Nothing, "Variety_HEAD", "Variety_name", "", "(Variety_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Vareity.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    cbo_Ledger.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(10)
                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_Vareity.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True

                .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Variety_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Vareity.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Vareity, Nothing, "Variety_Head", "Variety_name", "", "(Variety_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(2).Value) = "" Then
                    txt_ADVANCE.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
        End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim rect As Rectangle
        Dim dep_idno As Integer = 0
        '  Dim Condt As String

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Val(.Rows(e.RowIndex).Cells(13).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(13).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(13).Value = Val(.Rows(e.RowIndex - 1).Cells(13).Value) + 1
                End If
            End If

            If e.ColumnIndex = 2 And Trim(.Rows(e.RowIndex).Cells(12).Value) = "" Then

                If cbo_Grid_Vareity.Visible = False Or Val(cbo_Grid_Vareity.Tag) <> e.RowIndex Then

                    cbo_Grid_Vareity.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Variety_Name from Variety_Head order by Variety_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Vareity.DataSource = Dt1
                    cbo_Grid_Vareity.DisplayMember = "Variety_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Vareity.Left = .Left + rect.Left
                    cbo_Grid_Vareity.Top = .Top + rect.Top

                    cbo_Grid_Vareity.Width = rect.Width
                    cbo_Grid_Vareity.Height = rect.Height
                    cbo_Grid_Vareity.Text = .CurrentCell.Value

                    cbo_Grid_Vareity.Tag = Val(e.RowIndex)
                    cbo_Grid_Vareity.Visible = True

                    cbo_Grid_Vareity.BringToFront()
                    cbo_Grid_Vareity.Focus()

                End If

            Else
                cbo_Grid_Vareity.Visible = False

            End If

            If e.ColumnIndex = 4 And Trim(.Rows(e.RowIndex).Cells(12).Value) = "" Then

                If cbo_Grid_Unit.Visible = False Or Val(cbo_Grid_Unit.Tag) <> e.RowIndex Then

                    cbo_Grid_Unit.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt4)
                    cbo_Grid_Unit.DataSource = Dt4
                    cbo_Grid_Unit.DisplayMember = "Unit_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Unit.Left = .Left + rect.Left
                    cbo_Grid_Unit.Top = .Top + rect.Top

                    cbo_Grid_Unit.Width = rect.Width
                    cbo_Grid_Unit.Height = rect.Height
                    cbo_Grid_Unit.Text = .CurrentCell.Value

                    cbo_Grid_Unit.Tag = Val(e.RowIndex)
                    cbo_Grid_Unit.Visible = True

                    cbo_Grid_Unit.BringToFront()
                    cbo_Grid_Unit.Focus()

                End If

            Else
                cbo_Grid_Unit.Visible = False

            End If

            If e.ColumnIndex = 7 And Trim(.Rows(e.RowIndex).Cells(12).Value) = "" Then

                If cbo_Grid_size.Visible = False Or Val(cbo_Grid_size.Tag) <> e.RowIndex Then

                    cbo_Grid_size.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select sIZE_Name from sIZE_Head order by sIZE_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_size.DataSource = Dt3
                    cbo_Grid_size.DisplayMember = "sIZE_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_size.Left = .Left + rect.Left
                    cbo_Grid_size.Top = .Top + rect.Top

                    cbo_Grid_size.Width = rect.Width
                    cbo_Grid_size.Height = rect.Height
                    cbo_Grid_size.Text = .CurrentCell.Value

                    cbo_Grid_size.Tag = Val(e.RowIndex)
                    cbo_Grid_size.Visible = True

                    cbo_Grid_size.BringToFront()
                    cbo_Grid_size.Focus()


                End If


            Else
                cbo_Grid_size.Visible = False

            End If

            'If e.ColumnIndex = 8 And Trim(.Rows(e.RowIndex).Cells(12).Value) = "" Then

            '    If cbo_Grid_paper.Visible = False Or Val(cbo_Grid_paper.Tag) <> e.RowIndex Then

            '        cbo_Grid_paper.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Sleeve_Name from Sleeve_Head  order by Sleeve_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt2)
            '        cbo_Grid_paper.DataSource = Dt2
            '        cbo_Grid_paper.DisplayMember = "Sleeve_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_paper.Left = .Left + rect.Left
            '        cbo_Grid_paper.Top = .Top + rect.Top

            '        cbo_Grid_paper.Width = rect.Width
            '        cbo_Grid_paper.Height = rect.Height
            '        cbo_Grid_paper.Text = .CurrentCell.Value

            '        cbo_Grid_paper.Tag = Val(e.RowIndex)
            '        cbo_Grid_paper.Visible = True

            '        cbo_Grid_paper.BringToFront()
            '        cbo_Grid_paper.Focus()

            '    End If

            'Else
            '    cbo_Grid_paper.Visible = False

            'End If


            'If e.ColumnIndex = 9 And Trim(.Rows(e.RowIndex).Cells(12).Value) = "" Then

            '    If cbo_Grid_ink.Visible = False Or Val(cbo_Grid_ink.Tag) <> e.RowIndex Then

            '        cbo_Grid_ink.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt3)
            '        cbo_Grid_ink.DataSource = Dt3
            '        cbo_Grid_ink.DisplayMember = "Colour_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_ink.Left = .Left + rect.Left
            '        cbo_Grid_ink.Top = .Top + rect.Top

            '        cbo_Grid_ink.Width = rect.Width
            '        cbo_Grid_ink.Height = rect.Height
            '        cbo_Grid_ink.Text = .CurrentCell.Value

            '        cbo_Grid_ink.Tag = Val(e.RowIndex)
            '        cbo_Grid_ink.Visible = True

            '        cbo_Grid_ink.BringToFront()
            '        cbo_Grid_ink.Focus()


            '    End If


            'Else
            '    cbo_Grid_ink.Visible = False

            'End If

            If e.ColumnIndex = 8 Then

                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                pnl_PaperSelection_ToolTip.Left = .Left + rect.Left
                pnl_PaperSelection_ToolTip.Top = .Top + rect.Top + rect.Height + 3

                pnl_PaperSelection_ToolTip.Visible = True

            Else
                pnl_PaperSelection_ToolTip.Visible = False

            End If

            If e.ColumnIndex = 9 Then

                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                pnl_InkSelection_ToolTip.Left = .Left + rect.Left
                pnl_InkSelection_ToolTip.Top = .Top + rect.Top + rect.Height + 3

                pnl_InkSelection_ToolTip.Visible = True

            Else
                pnl_InkSelection_ToolTip.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Then

                    ' .CurrentRow.Cells(8).Value = Format(Val(.CurrentRow.Cells(5).Value) * Val(.CurrentRow.Cells(7).Value), "#########0.00")

                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 3 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                ElseIf .CurrentCell.ColumnIndex = 3 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Or Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value) <> "" Then
                        e.Handled = True
                    End If

                End If

            End If
        End With

    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        cbo_Ledger.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 5)
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

                If Trim(.Rows(n).Cells(12).Value) = "" Then

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

                End If

            End With
        End If

        If e.Control = False And e.KeyValue = 17 Then
            If dgv_Details.CurrentCell.ColumnIndex = 8 Then
                Paper_Selection()
            ElseIf dgv_Details.CurrentCell.ColumnIndex = 9 Then
                Ink_Selection()
            End If
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

    Private Sub cbo_Grid_paper_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_paper.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sleeve_Head", "Sleeve_Name", "", "(Sleeve_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_paper_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_paper.KeyDown

        cbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_paper, Nothing, Nothing, "Sleeve_Head", "Sleeve_Name", "", "(Sleeve_IdNo = 0)")

        With dgv_PaperColourDetails

            If (e.KeyValue = 38 And cbo_Grid_paper.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    btn_Close_PaperColourDetails.Focus()
                   
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_paper.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    btn_Close_PaperColourDetails_Click(sender, e)

                ElseIf .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) <> "" Then
                    .Rows.Add()
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_paper_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_paper.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_paper, Nothing, "Sleeve_Head", "Sleeve_Name", "", "(Sleeve_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_PaperColourDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_paper.Text)

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    btn_Close_PaperColourDetails_Click(sender, e)

                ElseIf .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) <> "" Then
                    .Rows.Add()
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                End If

            End With
        End If

    End Sub

    Private Sub cbo_Grid_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")

    End Sub

    Private Sub cbo_Grid_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Unit, Nothing, Nothing, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Item_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_paper.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Sleeve_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_paper.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Grid_Item_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_paper.TextChanged
        Try
            If cbo_Grid_paper.Visible Then
                With dgv_PaperColourDetails
                    If Val(cbo_Grid_paper.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_paper.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Unit, Nothing, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(4).Value = Trim(cbo_Grid_Unit.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

        End If
    End Sub

    Private Sub cbo_Grid_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyUp

        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Unit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.TextChanged
        Try
            If cbo_Grid_Unit.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Unit.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Unit.Text)
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
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Printing_Order_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Printing_Order_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Printing_Order_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Ledger.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_Ledger.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Printing_Order_Head a left outer join ledger_head b on a.Ledger_IdNo = b.Ledger_IdNo Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Printing_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Printing_Order_Date, a.for_orderby, a.Printing_Order_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Printing_Order_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Printing_Order_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("ADVANCE").ToString
                 
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

    Private Sub cbo_Filter_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Ledger, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub dtp_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown
        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    SendKeys.Send("{TAB}")
        'End If
        'If e.KeyValue = 38 Then
        '    e.Handled = True
        '    btn_Cancel.Focus()
        'End If
    End Sub

    Private Sub dtp_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

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

    Private Sub cbo_Variety_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Vareity.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Variety_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Vareity.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_ink_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ink.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_name", "", "(Colour_idno = 0)")

    End Sub

    Private Sub cbo_Grid_Brand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ink.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ink, Nothing, Nothing, "Colour_Head", "Colour_name", "", "(Colour_idno = 0)")

        With dgv_INKColourDetails

            If (e.KeyValue = 38 And cbo_Grid_ink.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    btn_Close_INKColourDetails.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_ink.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    btn_Close_INKColourDetails_Click(sender, e)

                ElseIf .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) <> "" Then
                    .Rows.Add()
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Brand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ink.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ink, Nothing, "Colour_Head", "Colour_name", "", "(Colour_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_INKColourDetails

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_ink.Text)

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    btn_Close_INKColourDetails_Click(sender, e)

                ElseIf .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) <> "" Then
                    .Rows.Add()
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                End If

            End With
        End If
    End Sub

    Private Sub cbo_Grid_Brand_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ink.KeyUp

        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ink.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Variety_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Vareity.TextChanged
        Try
            If cbo_Grid_Vareity.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Vareity.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Vareity.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "size_Head", "size_name", "", "(size_idno = 0)")

    End Sub

    Private Sub cbo_Grid_size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_size.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_size, Nothing, Nothing, "size_Head", "size_name", "", "(size_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_Grid_size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With
    End Sub

    Private Sub cbo_Grid_size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_size, Nothing, "size_Head", "size_name", "", "(size_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(7).Value = Trim(cbo_Grid_size.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(8)

        End If
    End Sub

    Private Sub cbo_Grid_size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_size.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_ink_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ink.TextChanged
        Try
            If cbo_Grid_ink.Visible Then
                With dgv_INKColourDetails
                    If Val(cbo_Grid_ink.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ink.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    SendKeys.Send("{TAB}")
        'End If
        'If e.KeyValue = 38 Then
        '    btn_Filter_Show.Focus()
        'End If
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress

        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    SendKeys.Send("{TAB}")
        'End If
        'If e.KeyValue = 38 Then
        '    e.Handled = True
        '    SendKeys.Send("+{TAB}")
        'End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            txt_AdvDate.Focus()
        End If
        If e.KeyValue = 40 Then
            e.Handled = True
            btn_Save.Focus()
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Printing_Order_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Printing_Order_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*,c.Ledger_Name from Printing_Order_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Printing_Order_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name,  f.Brand_Name from Printing_Order_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno  LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Printing_Order_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format1(e)
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            If ps.Width = 800 And ps.Height = 600 Then
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
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 30
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        NoofItems_PerPage = 8

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 325 : ClAr(3) = 150 : ClAr(4) = 80 : ClAr(5) = 75
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Machine_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, f.Brand_Name from Printing_Order_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno  LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Printing_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

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

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("Received From :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "Issued To", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Printing_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Received From", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "Received From :  " & "M/s." & prn_HdDt.Rows(0).Item("Received_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Printing_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BRAND", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, "MACHINE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For I = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_Grid_size_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_size.TextChanged
        Try
            If cbo_Grid_size.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_size.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 7 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_size.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_ADVANCE_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ADVANCE.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)

            Else
                cbo_Ledger.Focus()

            End If
        End If
        If e.KeyValue = 40 Then
            txt_AdvDate.Focus()
        End If
    End Sub

    Private Sub txt_ADVANCE_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ADVANCE.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_AdvDate.Focus()
        End If
    End Sub
    Private Sub Paper_Selection()

        Dim Det_SLNo As Integer
        Dim n As Integer, SNo As Integer
        Try

        

            Det_SLNo = Val(dgv_Details.CurrentRow.Cells(13).Value)

            With dgv_PaperColourDetails

                SNo = 0
                .Rows.Clear()

                For i = 0 To dgv_paperAllDetails.RowCount - 1
                    If Det_SLNo = Val(dgv_paperAllDetails.Rows(i).Cells(0).Value) Then

                        SNo = SNo + 1

                        n = .Rows.Add()
                        .Rows(n).Cells(0).Value = SNo
                        .Rows(n).Cells(1).Value = Trim(dgv_paperAllDetails.Rows(i).Cells(1).Value)
                       
                    End If
                Next i

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT PAPER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        pnl_PaperColourDetails.Visible = True
        pnl_Back.Enabled = False
        dgv_PaperColourDetails.Focus()

        If dgv_PaperColourDetails.RowCount = 0 Then
            dgv_PaperColourDetails.Rows.Add()
        End If

        If dgv_PaperColourDetails.Rows.Count > 0 Then
            dgv_PaperColourDetails.CurrentCell = dgv_PaperColourDetails.Rows(0).Cells(1)
        End If

    End Sub

    Private Sub Ink_Selection()

        Dim Det_SLNo As Integer
        Dim n As Integer, SNo As Integer
        Try

            Det_SLNo = Val(dgv_Details.CurrentRow.Cells(13).Value)

            With dgv_INKColourDetails

                SNo = 0
                .Rows.Clear()

                For i = 0 To dgv_InkAllDetails.RowCount - 1
                    If Det_SLNo = Val(dgv_InkAllDetails.Rows(i).Cells(0).Value) Then

                        SNo = SNo + 1
                        n = .Rows.Add()
                        .Rows(n).Cells(0).Value = SNo
                        .Rows(n).Cells(1).Value = Trim(dgv_InkAllDetails.Rows(i).Cells(1).Value)

                    End If
                Next i

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT INK...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        pnl_INKColourDetails.Visible = True
        pnl_Back.Enabled = False
        dgv_INKColourDetails.Focus()

        If dgv_INKColourDetails.RowCount = 0 Then
            dgv_INKColourDetails.Rows.Add()
        End If

        If dgv_INKColourDetails.Rows.Count > 0 Then
            dgv_INKColourDetails.CurrentCell = dgv_INKColourDetails.Rows(0).Cells(1)
            dgv_INKColourDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub dgv_paperColourDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PaperColourDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_PaperColourDetails

            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_paper.Visible = False Or Val(cbo_Grid_paper.Tag) <> e.RowIndex Then

                    cbo_Grid_paper.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_paper.DataSource = Dt3
                    cbo_Grid_paper.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_paper.Left = .Left + rect.Left
                    cbo_Grid_paper.Top = .Top + rect.Top

                    cbo_Grid_paper.Width = rect.Width
                    cbo_Grid_paper.Height = rect.Height
                    cbo_Grid_paper.Text = .CurrentCell.Value

                    cbo_Grid_paper.Tag = Val(e.RowIndex)
                    cbo_Grid_paper.Visible = True

                    cbo_Grid_paper.BringToFront()
                    cbo_Grid_paper.Focus()


                End If


            Else
                cbo_Grid_paper.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_PaperColourDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PaperColourDetails.KeyUp
        'Dim n As Integer
        'With dgv_PaperColourDetails

        '    n = .CurrentRow.Index

        '    If n = .Rows.Count - 1 Then
        '        For i = 0 To .Columns.Count - 1
        '            .Rows(n).Cells(i).Value = ""
        '        Next

        '    Else
        '        .Rows.RemoveAt(n)

        '    End If


        '    For i = 0 To .Rows.Count - 1

        '        .Rows(i).Cells(0).Value = Val(.Rows(i - 1).Cells(0).Value) + 1

        '    Next

        'End With
    End Sub

    Private Sub dgv_paperColourDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PaperColourDetails.RowsAdded
        With dgv_paperColourDetails
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub dgv_INKColourDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_INKColourDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_INKColourDetails

            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ink.Visible = False Or Val(cbo_Grid_ink.Tag) <> e.RowIndex Then

                    cbo_Grid_ink.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_ink.DataSource = Dt3
                    cbo_Grid_ink.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ink.Left = .Left + rect.Left
                    cbo_Grid_ink.Top = .Top + rect.Top

                    cbo_Grid_ink.Width = rect.Width
                    cbo_Grid_ink.Height = rect.Height
                    cbo_Grid_ink.Text = .CurrentCell.Value

                    cbo_Grid_ink.Tag = Val(e.RowIndex)
                    cbo_Grid_ink.Visible = True

                    cbo_Grid_ink.BringToFront()
                    cbo_Grid_ink.Focus()


                End If


            Else
                cbo_Grid_ink.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_INKColourDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_INKColourDetails.RowsAdded
        With dgv_INKColourDetails
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub btn_Close_INKColourDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_INKColourDetails.Click
        Close_inkDetails()
    End Sub

    Private Sub Close_PaperDetails()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Dt1 As New DataTable
        Dim dgvDet_CurRow As Integer = 0
        Dim Paps As String
        Dim Det_SlNo As Integer = 0
        Dim n As Integer = 0
        Try


            Det_SlNo = Val(dgv_Details.CurrentRow.Cells(13).Value)

            Cmd.Connection = con

            Paps = ""

            With dgv_paperAllDetails

LOOP1:
                For I = 0 To .RowCount - 1

                    If Val(.Rows(I).Cells(0).Value) = Val(Det_SlNo) Then

                        If I = .Rows.Count - 1 Then
                            For J = 0 To .ColumnCount - 1
                                .Rows(I).Cells(J).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(I)

                        End If

                        GoTo LOOP1

                    End If

                Next I

                For I = 0 To dgv_PaperColourDetails.RowCount - 1
                    If Trim(dgv_PaperColourDetails.Rows(I).Cells(1).Value) <> "" Then

                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = Val(Det_SlNo)
                        .Rows(n).Cells(1).Value = dgv_PaperColourDetails.Rows(I).Cells(1).Value

                        If I = 0 Then
                            Paps = Trim(dgv_PaperColourDetails.Rows(I).Cells(1).Value)
                        Else
                            Paps = Paps & ", " & Trim(dgv_PaperColourDetails.Rows(I).Cells(1).Value)
                        End If

                    End If
                Next I

            End With

            pnl_Back.Enabled = True
            pnl_PaperColourDetails.Visible = False
           

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If Trim(Paps) <> "" Then

                    dgv_Details.CurrentRow.Cells(8).Value = Paps

                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(9)
                    dgv_Details.CurrentCell.Selected = True

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID PAPER DETAILS ENTRY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Close_PaperColourDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PaperColourDetails.Click
        Close_PaperDetails()
    End Sub
    Private Sub Close_inkDetails()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Dt1 As New DataTable
        Dim dgvDet_CurRow As Integer = 0
        Dim Paps As String
        Dim Det_SlNo As Integer = 0
        Dim n As Integer = 0
        Try


            Det_SlNo = Val(dgv_Details.CurrentRow.Cells(13).Value)

            Cmd.Connection = con

            Paps = ""

            With dgv_InkAllDetails

LOOP1:
                For I = 0 To .RowCount - 1

                    If Val(.Rows(I).Cells(0).Value) = Val(Det_SlNo) Then

                        If I = .Rows.Count - 1 Then
                            For J = 0 To .ColumnCount - 1
                                .Rows(I).Cells(J).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(I)

                        End If

                        GoTo LOOP1

                    End If

                Next I

                For I = 0 To dgv_INKColourDetails.RowCount - 1
                    If Trim(dgv_INKColourDetails.Rows(I).Cells(1).Value) <> "" Then

                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = Val(Det_SlNo)
                        .Rows(n).Cells(1).Value = dgv_INKColourDetails.Rows(I).Cells(1).Value

                        If I = 0 Then
                            Paps = Trim(dgv_INKColourDetails.Rows(I).Cells(1).Value)
                        Else
                            Paps = Paps & ", " & Trim(dgv_INKColourDetails.Rows(I).Cells(1).Value)
                        End If

                    End If
                Next I

            End With



            pnl_Back.Enabled = True
            pnl_INKColourDetails.Visible = False


            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If Trim(Paps) <> "" Then

                    dgv_Details.CurrentRow.Cells(9).Value = Paps

                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(10)
                    dgv_Details.CurrentCell.Selected = True

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID ink DETAILS ENTRY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_AdvDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AdvDate.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            txt_AdvDate.Text = Format(Convert.ToDateTime(Date.Today), "dd-MM-yyyy").ToString
        End If
    End Sub
End Class