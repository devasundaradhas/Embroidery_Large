Imports System.IO

Public Class Embroidery_Production_Cost

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False

    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GEQUO-"

    Private cmbItmNm As String
    Private cmbszNm As String
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt_VAT As New DataTable
    Private prn_DetDt_VAT As New DataTable

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetAr(200, 11) As String

    Private prn_PageNo As Integer
    Private prn_DetMxIndx As Integer
    'Private NoCalc_Status As Boolean = False
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_DetDt_VAT1 As New DataTable
    Private prn_DetIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer

    Private prn_OriDupTri As String = ""

    Public previlege As String

    Dim PrevHorLinePos As Integer = 0

    Dim QUOTE_ID As String

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        cbo_PartyName.Text = ""
        txt_Design1.Text = ""
        txt_Remarks.Text = ""

        If Filter_Status = False Then

            'dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            'dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White
        Quote_Id = ""

    End Sub

    Private Sub ControlKeyDown(ByVal sender As Object, ByVal e As System.EventArgs)

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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
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

        ' Grid_Cell_DeSelect()
        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        '    Grid_Cell_DeSelect()
        'End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then

            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If

        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
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

    Private Sub move_record()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        'DIM QUOTE_ID As String

        'If Val(no) = 0 Then Exit Sub

        clear()

        Try

            ' QUOTE_ID = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*,B.Ledger_Name from Order_Program_Head a  inner join Ledger_Head B on a.Ledger_IdNo = b.Ledger_IdNo Where OrderCode_forSelection = '" & cbo_UID.Text & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then


                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_UID.Text = dt1.Rows(0).Item("OrderCode_forSelection").ToString

                txt_Design1.Text = (dt1.Rows(0).Item("Design").ToString)

                If Not IsDBNull(dt1.Rows(0).Item("Style_Ref_No")) Then
                    txt_StyleNo.Text = dt1.Rows(0).Item("Style_Ref_No").ToString
                End If



            End If

            dt1.Rows.Clear()

            '---------------------------------------

            da1 = New SqlClient.SqlDataAdapter("select a.sales_quotation_code from sales_quotation_head a  Where UID = '" & cbo_UID.Text & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then


                Quote_Id = dt1.Rows(0).Item("Sales_Quotation_Code").ToString


            End If

            dt1.Rows.Clear()

            '-----------------------------------------

            da1 = New SqlClient.SqlDataAdapter("select a.* from Production_Cost a  Where UID = '" & cbo_UID.Text & "'", con)
            da1.Fill(dt1)


            If dt1.Rows.Count > 0 Then

                If Not IsDBNull(dt1.Rows(0).Item("Production_Cost")) Then
                    txt_ProductionCost.Text = FormatNumber(dt1.Rows(0).Item("Production_Cost"), 2, TriState.False, TriState.False, TriState.False)
                End If

                If Not IsDBNull(dt1.Rows(0).Item("Remarks")) Then
                    txt_Remarks.Text = dt1.Rows(0).Item("Remarks")
                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()



        End Try

    End Sub

    Private Sub Sales_Quotation_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""

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

    Private Sub Sales_Quotation_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'dtp_Date.MaxDate = Common_Procedures.settings.Validation_End_Date

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        con.Open()


        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_PartyName.DataSource = dt2
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"


        pnl_Filter.Visible = False

        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler btn_Save.KeyDown, AddressOf ControlKeyDown
        AddHandler btnClose.KeyDown, AddressOf ControlKeyDown


        AddHandler txt_Design1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_UID.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ProductionCost.GotFocus, AddressOf ControlGotFocus



        AddHandler txt_Design1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_UID.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ProductionCost.LostFocus, AddressOf ControlLostFocus

        'AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Design1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ProductionCost.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Design1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ProductionCost.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0

        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Purchase_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)

        con.Close()
        con.Dispose()

    End Sub

    Private Sub Purchase_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("D") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable

        'DIM QUOTE_ID As String = ""

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        Try


            cmd.Connection = con

            cmd.CommandText = "Delete from Production_Cost Where UID = '" & cbo_UID.Text & "'"
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select order_no from sales_quotation_head order by order_no", con)
            da.Fill(dt2)
            'cbo_Filter_ItemName.DataSource = dt2
            'cbo_Filter_ItemName.DisplayMember = "order_no"

            'dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            'dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate

            cbo_Filter_PartyName.Text = ""
            'cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            'cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False

        'If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

        'If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("I") Then
        '    MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
        '    Exit Sub
        'End If

        'Dim Da As New SqlClient.SqlDataAdapter
        'Dim Dt As New DataTable
        'Dim movno As String, inpno As String
        'Dim RecCode As String

        ''If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        'Try

        '    inpno = InputBox("Enter New Job No.", "FOR NEW REC INSERTION...")

        '    RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

        '    Da = New SqlClient.SqlDataAdapter("select Sales_Quotation_No from Sales_Quotation_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Quotation_Code = '" & Trim(RecCode) & "'", con)
        '    Da.Fill(Dt)

        '    movno = ""
        '    If Dt.Rows.Count > 0 Then
        '        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
        '            movno = Trim(Dt.Rows(0)(0).ToString)
        '        End If
        '    End If

        '    Dt.Clear()
        '    Dt.Dispose()
        '    Da.Dispose()

        '    If Val(movno) <> 0 Then
        '        move_record(movno)

        '    Else
        '        If Val(inpno) = 0 Then
        '            MessageBox.Show("Invalid Lot No", "DOES NOT INSERT NEW REC...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '        Else
        '            new_record()
        '            Insert_Entry = True
        '            'lbl_JobNo.Text = Trim(UCase(inpno))

        '        End If

        '    End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim movno As String = ""

        'Try

        '    da = New SqlClient.SqlDataAdapter("select top 1 Sales_Quotation_No from Sales_Quotation_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Quotation_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_Quotation_No", con)
        '    dt = New DataTable
        '    da.Fill(dt)

        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            movno = dt.Rows(0)(0).ToString
        '        End If
        '    End If

        '    If Trim(movno) <> "" Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim movno As String = ""

        'Try

        '    da = New SqlClient.SqlDataAdapter("select top 1 Sales_Quotation_No from Sales_Quotation_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Quotation_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Quotation_No desc", con)
        '    dt = New DataTable
        '    da.Fill(dt)

        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            movno = dt.Rows(0)(0).ToString
        '        End If
        '    End If

        '    If Trim(movno) <> "" Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim movno As String = ""
        'Dim OrdByNo As Single

        'Try

        '    'OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

        '    da = New SqlClient.SqlDataAdapter("select top 1 Sales_Quotation_No from Sales_Quotation_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Quotation_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_Quotation_No", con)
        '    dt = New DataTable
        '    da.Fill(dt)

        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            movno = dt.Rows(0)(0).ToString
        '        End If
        '    End If

        '    If Trim(movno) <> "" Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim movno As String = ""
        'Dim OrdByNo As Single

        'Try

        '    'OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

        '    da = New SqlClient.SqlDataAdapter("select top 1 Sales_Quotation_No from Sales_Quotation_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Quotation_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Quotation_No desc", con)
        '    dt = New DataTable
        '    da.Fill(dt)

        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '            movno = dt.Rows(0)(0).ToString
        '        End If
        '    End If

        '    If Trim(movno) <> "" Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try

            clear()



            New_Entry = True



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt.Dispose()
            da.Dispose()

            'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try



    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        'Dim Da As New SqlClient.SqlDataAdapter
        'Dim Dt As New DataTable
        'Dim movno As String, inpno As String
        'Dim RecCode As String

        'Try

        '    inpno = InputBox("Enter Job.No.", "FOR FINDING...")

        '    RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

        '    Da = New SqlClient.SqlDataAdapter("select Sales_Quotation_No from Sales_Quotation_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Quotation_Code = '" & Trim(RecCode) & "'", con)
        '    Da.Fill(Dt)

        '    movno = ""
        '    If Dt.Rows.Count > 0 Then
        '        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
        '            movno = Trim(Dt.Rows(0)(0).ToString)
        '        End If
        '    End If

        '    Dt.Clear()

        '    If Val(movno) <> 0 Then
        '        move_record(movno)

        '    Else
        '        MessageBox.Show("Job No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '    End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally
        '    Dt.Dispose()
        '    Da.Dispose()


        'End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record


        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("A") And Not UCase(previlege).Contains("E") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If



        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable

        'DIM QUOTE_ID As String = ""
        Dim Nr As Long = 0
        Dim Itm_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Sno As Integer = 0

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)


        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try



            cmd.Connection = con
            cmd.Transaction = tr

            ' cmd.Parameters.Clear()
            'cmd.Parameters.AddWithValue("@Sales_QuotationDate", dtp_Date.Value.Date)



            If Not New_Entry Then
                If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("E") Then
                    MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
                    Exit Sub
                End If
            End If

            cmd.CommandText = "Delete from Production_Cost where UID = '" & cbo_UID.Text & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Production_Cost values ('" & cbo_UID.Text & "'," & Val(txt_ProductionCost.Text).ToString & ",'" & txt_Remarks.Text & "')"
            cmd.ExecuteNonQuery()



            tr.Commit()

            move_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub




    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub



    Private Sub cbo_partyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_UID, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
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
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            If Trim(cbo_Filter_Status.Text) = "" Or Trim(cbo_Filter_Status.Text) = "ALL" Then

            ElseIf Trim(cbo_Filter_Status.Text) = "PRODN. COST ENTERED"
                Condt = " a.OrderCode_forSelection in (Select UID from Production_Cost Where Production_Cost > 0)"
            Else
                Condt = " not a.OrderCode_forSelection in (Select UID from Production_Cost Where Production_Cost > 0)"
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
                If Len(Trim(Condt)) > 0 Then
                    Condt = Condt + " and "
                End If
                Condt = Condt + " a.Ledger_IdNo = " & Led_IdNo.ToString
            End If

            da = New SqlClient.SqlDataAdapter("Select a.OrderCode_forSelection,b.Ledger_Name from Order_Program_Head a inner join Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo " & IIf(Len(Trim(Condt)) > 0, " where " & Condt, ""), con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("OrderCode_forSelection").ToString

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES Not FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            dt1.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True 'SendKeys.Send("+{TAB}")
    End Sub

    'Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    '    If Asc(e.KeyChar) = 13 Then
    '        btn_Filter_Show_Click(sender, e)
    '    End If
    'End Sub

    'Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "Sales_Quotation_Head", "Order_No", "", "(Sales_Quotation_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, btn_Filter_Show, "Sales_Quotation__Head", "Order_No", "", "(Sales_Quotation_IdNo = 0)")
    'End Sub

    Private Sub Open_FilterEntry()

        'Dim movno As String

        'Try
        '    movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        '    If Val(movno) <> 0 Then
        '        Filter_Status = True
        '        move_record(movno)
        '        pnl_Back.Enabled = True
        '        pnl_Filter.Visible = False
        '    End If

        'Catch ex As Exception
        '    '---

        'End Try


    End Sub


    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        cbo_PartyName.Text = dgv_Filter_Details.Rows(dgv_Filter_Details.CurrentCell.RowIndex).Cells(1).Value
        cbo_UID.Text = dgv_Filter_Details.Rows(dgv_Filter_Details.CurrentCell.RowIndex).Cells(2).Value
        move_record()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            cbo_PartyName.Text = dgv_Filter_Details.Rows(dgv_Filter_Details.CurrentCell.RowIndex).Cells(1).Value
            cbo_UID.Text = dgv_Filter_Details.Rows(dgv_Filter_Details.CurrentCell.RowIndex).Cells(2).Value
            move_record()
        End If
    End Sub


    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_stitches1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_stitches2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_Per_Embroidery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_Per_Stiches_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub





    Public Sub print_record() Implements Interface_MDIActions.print_record

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        'DIM QUOTE_ID As String
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        'QUOTE_ID = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Sales_Quotation_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Quotation_Code = '" & Trim(QUOTE_ID) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If


            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "12")

        'prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    If Common_Procedures.settings.CustomerCode = "5002" Then
                        PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    Else
                        PrintDocument1.PrinterSettings.PrinterName = "doPDF 9"
                    End If
                    'PrintDocument1.PrinterSettings.PrinterName = "OneNote"
                    PrintDocument1.PrinterSettings.PrintToFile = True
                    PrintDocument1.PrinterSettings.PrintFileName = "d:\Invoice.pdf"
                    PrintDocument1.Print()

                Else
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If
                End If


            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Print_PDF_Status = False
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()


    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        'DIM QUOTE_ID As String
        Dim W1 As Single = 0
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String

        'QUOTE_ID = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt_VAT.Clear()
        prn_DetDt_VAT.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0 '1
        DetSNo = 0
        prn_DetMxIndx = 0
        prn_Count = 0

        Try


            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, " &
                                               "Lsh.State_Code as Ledger_State_Code,D.* from Sales_Quotation_Head a inner join Order_Program_Head d on a.UID = d.OrderCode_forSelection " &
                                               "LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo " &
                                               "INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo " &
                                               " where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And a.Sales_Quotation_Code = '" & Trim(QUOTE_ID) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If


            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Dim Cmp_Nam As String = ""

        Cmp_Nam = Trim(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_Name", "Company_IdNo =" & Val(Common_Procedures.CompIdNo)))

        Printing_Format2_GST(e)

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNmDesc As String = ""
        Dim ItmDescAr(20) As String

        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim m1 As Integer = 0
        Dim k As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 23
            .Right = 62
            .Top = 206
            .Bottom = 35
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

        NoofItems_PerPage = 17

        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt_VAT.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 70 : ClArr(3) = 300 : ClArr(4) = 110 : ClArr(5) = 80
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        ' EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt_VAT.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt_VAT.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt_VAT.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNmDesc = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_IdNO").ToString))
                        If (prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "" Then
                            ItmNmDesc = Trim(ItmNmDesc) & "  -  " & prn_DetDt_VAT.Rows(prn_DetIndx).Item("Item_Description").ToString
                        End If

                        Erase ItmDescAr
                        ItmDescAr = New String(20) {}

                        m1 = -1

LOOP1:
                        If Len(ItmNmDesc) > 40 Then
                            For k = 40 To 1 Step -1
                                If Mid$(ItmNmDesc, k, 1) = " " Or Mid$(ItmNmDesc, k, 1) = "," Or Mid$(ItmNmDesc, k, 1) = "/" Or Mid$(ItmNmDesc, k, 1) = "\" Or Mid$(ItmNmDesc, k, 1) = "-" Or Mid$(ItmNmDesc, k, 1) = "." Or Mid$(ItmNmDesc, k, 1) = "&" Or Mid$(ItmNmDesc, k, 1) = "_" Then Exit For
                            Next k
                            If k = 0 Then k = 40
                            m1 = m1 + 1
                            ItmDescAr(m1) = Microsoft.VisualBasic.Left(Trim(ItmNmDesc), k)
                            'ItmDescAr(m1) = Microsoft.VisualBasic.Left(Trim(ItmNmDesc), K - 1)
                            ItmNmDesc = Microsoft.VisualBasic.Right(ItmNmDesc, Len(ItmNmDesc) - k)
                            GoTo LOOP1

                        Else

                            m1 = m1 + 1
                            ItmDescAr(m1) = ItmNmDesc

                        End If


                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Dc_No").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(0)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 35, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_VAT.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1


                        For k = 1 To m1
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(k)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        Next k

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim i As Integer = 0
        Dim strHeight As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim C1 As Single, W1 As Single, S1 As Single, S2 As Single
        Dim CurX As Single = 0
        Dim OrdNoDt As String = ""
        Dim DcNoDt As String = ""


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sales_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(EntryCode) & "'  Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        LnAr(1) = CurY

        '---TOP 250
        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("INVOICE DATE   : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
        S2 = e.Graphics.MeasureString("ORDER.NO & DATE :    ", pFont).Width

        CurY = CurY - 20
        If Trim(prn_HdDt_VAT.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "PAN NO : " & Trim(prn_HdDt_VAT.Rows(0).Item("Company_PanNo").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + 25
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try


            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt_VAT.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt_VAT.Rows(0).Item("Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt_VAT.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            OrdNoDt = prn_HdDt_VAT.Rows(0).Item("Order_No").ToString
            'If Trim(prn_HdDt_VAT.Rows(0).Item("Order_Date").ToString) <> "" Then
            '    OrdNoDt = Trim(OrdNoDt) & "  Dt : " & Trim(prn_HdDt_VAT.Rows(0).Item("Order_Date").ToString)
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt_VAT.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(OrdNoDt) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(OrdNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If


            DcNoDt = prn_HdDt_VAT.Rows(0).Item("Dc_No").ToString
            'If Trim(prn_HdDt_VAT.Rows(0).Item("Dc_date").ToString) <> "" Then
            '    DcNoDt = Trim(DcNoDt) & "  Dt : " & Trim(prn_HdDt_VAT.Rows(0).Item("Dc_date").ToString)
            'End If

            CurY = CurY + TxtHgt
            If prn_HdDt_VAT.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "TIN NO: " & prn_HdDt_VAT.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            'If Trim(DcNoDt) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(DcNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            If prn_HdDt_VAT.Rows(0).Item("Ledger_PanNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PAN NO: " & prn_HdDt_VAT.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Oty", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rate", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        'DIM QUOTE_ID As String
        Dim p1Font As Font
        Dim I As Integer
        Dim BmsInWrds As String
        Dim W1 As Single = 0
        Dim CurY1 As Single = 0
        Dim Cmp_Name As String
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim Yax As Single
        Dim vprn_PckNos As String = ""
        Dim Tot_Wgt As Single = 0, Tot_Amt As Single = 0, Tot_Bgs As Single = 0, Tot_Wgt_Bag As Single = 0
        W1 = e.Graphics.MeasureString("Payment Terms : ", pFont).Width

        'QUOTE_ID = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt_VAT.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, " " & (prn_HdDt_VAT.Rows(0).Item("gROSS_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt_VAT.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt_VAT.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                End If

            End If

            CurY = CurY - 10


            CurY = CurY + TxtHgt + 1
            If Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt_VAT.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt_VAT.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3
            If Val(prn_HdDt_VAT.Rows(0).Item("Freight_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3
            If Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then

                If is_LastPage = True Then

                    If Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                End If
            End If

            If Val(prn_HdDt_VAT.Rows(0).Item("Form_H_Status").ToString) <> 0 Then
                CurY = CurY + TxtHgt + 3
                Common_Procedures.Print_To_PrintDocument(e, "Against Form-H", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(0.0), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt_VAT.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt_VAT.Rows(0).Item("Round_Off").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt_VAT.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt_VAT.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            StrConv(BmsInWrds, vbProperCase)

            Common_Procedures.Print_To_PrintDocument(e, "Rupees    : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt_VAT.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(2), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(2), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim vNoofHsnCodes As Integer = 0
        Dim jurs As String = ""
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 45
            .Right = 50
            .Top = 30 ' 65
            .Bottom = 40
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

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 19 ' 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 20 ' 17 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 30 : ClArr(2) = 260 : ClArr(3) = 100 : ClArr(4) = 70 : ClArr(5) = 0 : ClArr(6) = 0 : ClArr(7) = 85 : ClArr(8) = 70
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        CurY = TMargin


        'EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try

        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format2_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            'Try
            NoofDets = 0

            If Val(DetIndx) > 18 Then
                CurY = CurY + TxtHgt
            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SLT_GreyLogo, Drawing.Image), LMargin + 220, CurY + 70, 250, 250)
            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1201--" Then
                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.swasthick_Greylogo, Drawing.Image), LMargin + 240, CurY + 70, 250, 250)

            End If

            Cen1 = (PageWidth / 2)
            W1 = e.Graphics.MeasureString("INVOICE DATE             :", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Contact Person", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Name & Mobile #", LMargin + 10, CurY + TxtHgt, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Contact_Person").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (2 * TxtHgt), PageWidth, CurY + (2 * TxtHgt))
            PrevHorLinePos = CurY + (2 * TxtHgt)
            'e.Graphics.DrawLine(Pens.Black, LMargin + Cen1 , PrevHorLinePos, LMargin + Cen1 , CurY + (2 * TxtHgt))

            CurY = CurY + (2 * TxtHgt) + 10

            Common_Procedures.Print_To_PrintDocument(e, "Style / Ref #", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Style_Ref_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Order Quantity", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Style_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (TxtHgt))
            PrevHorLinePos = CurY + (TxtHgt)
            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, "Embroidering Part", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Emb_Part").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Position", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Emb_Position").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (TxtHgt))
            PrevHorLinePos = CurY + (TxtHgt)
            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, "Grade & Sizes", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Under the Grade", LMargin + 10, CurY + TxtHgt, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizes").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (2 * TxtHgt), PageWidth, CurY + (2 * TxtHgt))
            'e.Graphics.DrawLine(Pens.Black, LMargin + Cen1 + 50, PrevHorLinePos, PageWidth, CurY + (2 * TxtHgt))
            PrevHorLinePos = CurY + (2 * TxtHgt)
            CurY = CurY + (2 * TxtHgt) + 10

            Common_Procedures.Print_To_PrintDocument(e, "Design Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Design").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (TxtHgt))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))
            PrevHorLinePos = CurY + (TxtHgt)
            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, "No. Of Thread Colours", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Thread_Colour_Count").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "No. Of Appliques", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("No_Of_Appliques").ToString, LMargin + W1 + Cen1 + 30, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))
            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (TxtHgt))
            PrevHorLinePos = CurY + (TxtHgt)
            CurY = CurY + TxtHgt + 10

            If Val(Common_Procedures.settings.CustomerCode) = 5010 Then
                Common_Procedures.Print_To_PrintDocument(e, "SPC #", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Val(Common_Procedures.settings.CustomerCode) = 5022 Then
                Common_Procedures.Print_To_PrintDocument(e, "RVM #", LMargin + 10, CurY, 0, 0, pFont)
            ElseIf Val(Common_Procedures.settings.CustomerCode) = 5027 Then
                Common_Procedures.Print_To_PrintDocument(e, "FWC #", LMargin + 10, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("UID").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "No. Of Sequins", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("No_Of_Sequins").ToString, LMargin + W1 + Cen1 + 30, CurY, 0, 0, pFont)

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (TxtHgt))

                PrevHorLinePos = CurY + (TxtHgt)
                CurY = CurY + TxtHgt + 10

                Common_Procedures.Print_To_PrintDocument(e, "No. Of Stitches", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Total_Qty")).ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), LMargin + Cen1, CurY + (TxtHgt))
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (TxtHgt))

                PrevHorLinePos = CurY + (TxtHgt)
                'CurY = CurY + TxtHgt + 10

                'Right side

                ' p1Font = New Font("Calibri", 12, FontStyle.Bold)
                'Common_Procedures.Print_To_PrintDocument(e, "IMAGE", LMargin + Cen1 + 10, CurY + 10, 2, 300, p1Font)

                Dim PIC As Image = Nothing
                If IsDBNull(prn_HdDt.Rows(0).Item("Sales_Quotation_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Sales_Quotation_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then
                                PIC = Image.FromStream(ms)
                                e.Graphics.DrawImage(DirectCast(PIC, Drawing.Image), LMargin + Cen1 + 5, CurY + 5, PageWidth - (LMargin + Cen1) - 10, 270)
                            End If
                        End Using
                    End If
                End If

                'Dim R As New Rectangle(New Point(LMargin + Cen1 + 10, CurY + 30), New Size(300, 240))
                'e.Graphics.DrawRectangle(New Pen(Brushes.Black), R)

                '---------

                CurY = CurY + TxtHgt + 10

                Common_Procedures.Print_To_PrintDocument(e, "Rate/1000 Stitches", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Rate_Stitches")).ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), LMargin + Cen1, CurY + (TxtHgt))
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (TxtHgt))

                PrevHorLinePos = CurY + (TxtHgt)
                CurY = CurY + TxtHgt + 10

                Common_Procedures.Print_To_PrintDocument(e, "Embroidering Rate", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "/Piece", LMargin + 10, CurY + TxtHgt, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, FormatNumber(prn_HdDt.Rows(0).Item("Rate_Embroidery"), 2, TriState.False, TriState.False, TriState.False).ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (2 * TxtHgt), LMargin + Cen1, CurY + (2 * TxtHgt))
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (2 * TxtHgt))
                PrevHorLinePos = CurY + (2 * TxtHgt)

                CurY = CurY + (2 * TxtHgt) + 10

                Common_Procedures.Print_To_PrintDocument(e, "Applique Cutting", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Rate/Piece", LMargin + 10, CurY + TxtHgt, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, FormatNumber(prn_HdDt.Rows(0).Item("Rate_Applique"), 2, TriState.False, TriState.False, TriState.False).ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (2 * TxtHgt), LMargin + Cen1, CurY + (2 * TxtHgt))
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (2 * TxtHgt))
                PrevHorLinePos = CurY + (2 * TxtHgt)

                CurY = CurY + (2 * TxtHgt) + 10

                Common_Procedures.Print_To_PrintDocument(e, "Material Rate/Piece", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)

                If Not IsDBNull(prn_HdDt.Rows(0).Item("Material_rate")) Then
                    Common_Procedures.Print_To_PrintDocument(e, FormatNumber(prn_HdDt.Rows(0).Item("Material_rate"), 2, TriState.False, TriState.False, TriState.False).ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                End If

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), LMargin + Cen1, CurY + (TxtHgt))
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (TxtHgt))
                PrevHorLinePos = CurY + (TxtHgt)

                CurY = CurY + TxtHgt + 10

                Common_Procedures.Print_To_PrintDocument(e, "Back Paper (Foam) ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "removing & Trimming", LMargin + 10, CurY + TxtHgt, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                If Not IsDBNull(prn_HdDt.Rows(0).Item("Foam_Removal_rate")) Then
                    Common_Procedures.Print_To_PrintDocument(e, FormatNumber((prn_HdDt.Rows(0).Item("Foam_Removal_rate")), 2, TriState.False, TriState.False, TriState.False).ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
                End If

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (2 * TxtHgt), LMargin + Cen1, CurY + (2 * TxtHgt))
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (2 * TxtHgt))
                PrevHorLinePos = CurY + (2 * TxtHgt)

                CurY = CurY + (2 * TxtHgt) + 10

                Common_Procedures.Print_To_PrintDocument(e, "Total Rate per Piece", LMargin + 10, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY + 10, 0, 0, pFont)
                p1Font = New Font("Calibri", 22, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Rs." & FormatNumber(prn_HdDt.Rows(0).Item("Net_Amount"), 2, TriState.False, TriState.False, TriState.False).ToString, LMargin + W1 + 30, CurY, 0, 0, p1Font)

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (2 * TxtHgt), PageWidth, CurY + (2 * TxtHgt))
                e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (2 * TxtHgt))
                PrevHorLinePos = CurY + (2 * TxtHgt)

                CurY = CurY + (2 * TxtHgt) + 10

                Common_Procedures.Print_To_PrintDocument(e, "Approved Rate", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Approved by/Signature", LMargin + 10, CurY + TxtHgt, 0, 0, pFont)

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (2 * TxtHgt), PageWidth, CurY + (2 * TxtHgt))
                'e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, PrevHorLinePos, LMargin + Cen1, CurY + (2 * TxtHgt))
                PrevHorLinePos = CurY + (2 * TxtHgt)

                CurY = CurY + (2 * TxtHgt) + 10


                Common_Procedures.Print_To_PrintDocument(e, "Payment Terms", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                If Not IsDBNull(prn_HdDt.Rows(0).Item("Payment_Terms")) Then Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Payment_Terms").ToString + IIf(Len(Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString)) > 0, " from the date of Invoice.", ""), LMargin + W1 + 30, CurY, 0, 0, pFont)
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Rejection Allowance", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)

                If Not IsDBNull(prn_HdDt.Rows(0).Item("Rejection_Allowance")) Then Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rejection_Allowance").ToString + " %", LMargin + W1 + 30, CurY, 0, 0, pFont)
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Are resources provided by Customer ? ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 150, CurY, 0, 0, pFont)
                If Not IsDBNull(prn_HdDt.Rows(0).Item("Is_Material_Provided")) Then Common_Procedures.Print_To_PrintDocument(e, (IIf(prn_HdDt.Rows(0).Item("Is_Material_Provided") = True, "Yes", "No")), LMargin + W1 + 175, CurY, 0, 0, pFont)
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "If yes What are they ? ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 150, CurY, 0, 0, pFont)
                If Not IsDBNull(prn_HdDt.Rows(0).Item("Material_Provided")) Then Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Material_Provided"), LMargin + W1 + 175, CurY, 0, 0, pFont)
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY + (TxtHgt), PageWidth, CurY + (TxtHgt))

                CurY = CurY + TxtHgt

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                PrevHorLinePos = CurY

                Common_Procedures.Print_To_PrintDocument(e, "Remarks :", LMargin, CurY + 3, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Remarks"), LMargin + 100, CurY + 3, 0, 0, pFont)

                If Not IsDBNull(prn_HdDt.Rows(0).Item("Prepared_by")) Then Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Prepared_by"), LMargin + 350, CurY + 3, 2, 200, pFont)

                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt

                'Common_Procedures.Print_To_PrintDocument(e, "(Name & Signature)", LMargin + 10, CurY, 2, 150, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(Digitizer)", LMargin + 350, CurY, 2, 200, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(Managing Director)", LMargin + 550, CurY, 2, PageWidth - 550, pFont)



                CurY = CurY + TxtHgt


                e.Graphics.DrawLine(Pens.Black, LMargin + 350, PrevHorLinePos, LMargin + 350, CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin + 550, PrevHorLinePos, LMargin + 550, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Sincerest thanks for your VALUED BUSINESS... Looking forward to work with you again soon !!!", LMargin - 25, CurY, 2, PageWidth, p1Font)

                CurY = CurY + TxtHgt



                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

                '        CurY = CurY + TxtHgt - 15
                '        p1Font = New Font("Calibri", 9, FontStyle.Regular)



            End If

            ' Catch ex As Exception

            'MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            'End Try

            e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String
        Dim PnAr() As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_Name from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        ' prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        'prn_OriDupTri = ""
        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count <= Len(Trim(prn_InpOpts)) Then

        '        S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

        '        If Val(S) = 1 Then
        '            prn_OriDupTri = "ORIGINAL"
        '            PrintDocument1.DefaultPageSettings.Color = True
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
        '            e.PageSettings.Color = True
        '        ElseIf Val(S) = 2 Then
        '            prn_OriDupTri = "DUPLICATE"
        '        ElseIf Val(S) = 3 Then
        '            prn_OriDupTri = "TRIPLICATE"
        '        End If

        '    End If
        'End If

        'If Trim(prn_OriDupTri) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If

        e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.spclogo, Drawing.Image), LMargin + 24, CurY + 10, 100, 100)

        p1Font = New Font("Arial Narrow", 9, FontStyle.Bold)

        strWidth = e.Graphics.MeasureString("""The Thread Art Studio""", p1Font).Width
        Common_Procedures.Print_To_PrintDocument(e, """The Thread Art Studio""", LMargin + 74 - (strWidth / 2), CurY + 115, 2, strWidth, p1Font, Brushes.Black)

        '---------------------------

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "EMBROIDERING RATE CONFIRMATION", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        'CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
        If Trim(Cmp_Add1) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
                Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            Else
                Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
            End If
        Else
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        End If

        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
        If Trim(Cmp_Add2) <> "" Then
            If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
                Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            Else
                Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
            End If
        Else
            Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SLT_Logo, Drawing.Image), LMargin + 24, CurY + 10, 100, 100)
            p1Font = New Font("President", 28, FontStyle.Bold)

        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1201--" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SWASTHICK_LOGO, Drawing.Image), LMargin + 24, CurY + 10, 100, 100)
            p1Font = New Font("Calibri", 22, FontStyle.Bold)

        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "5002" Then
            e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.ABILOGO, Drawing.Image), LMargin + 15, CurY + 5, 198, 132)
            p1Font = New Font("Calibri", 22, FontStyle.Bold)

        End If

        CurY = CurY + TxtHgt - 10

        ' p1Font = New Font("President", 20, FontStyle.Bold)
        pFont = New Font("Calibri", 10, FontStyle.Bold)

        '  Dim cM_br = New SolidBrush(Color.FromArgb(234, 240, 64))

        Dim cM_br = New SolidBrush(Color.FromArgb(235, 39, 5))
        Dim br = New SolidBrush(Color.FromArgb(0, 0, 111))

        If Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
            cM_br = New SolidBrush(Color.Green)
            br = New SolidBrush(Color.FromArgb(191, 43, 133))
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "5002" Then
            cM_br = New SolidBrush(Color.Navy)
            br = New SolidBrush(Color.Black)
        Else
            cM_br = New SolidBrush(Color.FromArgb(235, 39, 5))
            br = New SolidBrush(Color.FromArgb(0, 0, 111))
        End If

        LMargin = LMargin + 25

        'p1Font = New Font("Calibri", 20, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, cM_br)

        e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.SP_CREATION_HEADER, Drawing.Image), LMargin + 180, CurY, PageWidth - 400, 50)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight - 15

        CurY = CurY + 45

        'If Trim(Common_Procedures.settings.CustomerCode) = "5002" Then
        '    cM_br = New SolidBrush(Color.Black)
        'End If

        'strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'CurY = CurY + strHeight

        'If Len(Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)) > 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString), LMargin, CurY - 4, 2, PrintWidth, pFont, br)
        '    CurY = CurY + TxtHgt
        'End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, br)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, br)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width

        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, br)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, br)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font, br)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, br)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont, br)

        CurY = CurY + TxtHgt + 10

        LMargin = LMargin - 25



        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + 25

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "EMBROIDERING RATE CONFIRMATION", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)

        CurY = CurY + 15

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        pFont = New Font("Calibri", 10, FontStyle.Regular)

        ' Try

        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

        Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
        Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
        Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
        Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
        'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
        Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
        If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

        Led_State = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString)
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)

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

        'If Trim(Led_Add4) <> "" Then
        '    LInc = LInc + 1
        '    LedNmAr(LInc) = Led_Add4
        'End If

        If Trim(Led_State) <> "" Then
            LInc = LInc + 1
            LedNmAr(LInc) = Led_State
        End If

        If Trim(Led_PhNo) <> "" Then
            LInc = LInc + 1
            LedNmAr(LInc) = Led_PhNo
        End If

        If Trim(Led_GSTTinNo) <> "" Then
            LInc = LInc + 1
            LedNmAr(LInc) = Led_GSTTinNo
        End If

        'If Trim(Led_TinNo) <> "" Then
        '    LInc = LInc + 1
        '    LedNmAr(LInc) = Led_TinNo
        'End If

        Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("INVOICE DATE  :", pFont).Width
        W2 = e.Graphics.MeasureString("TO :", pFont).Width

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "No.", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Quotation_No").ToString, LMargin + Cen1 + W1 + 30, CurY, 0, 0, p1Font)

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + Cen1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Quotation_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, CurY, 0, 0, pFont)

        If Len(Trim(LedNmAr(4))) > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)
        End If

        If Len(Trim(LedNmAr(5))) > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)
        End If

        If Len(Trim(LedNmAr(6))) > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)
        End If

        If Len(Trim(LedNmAr(7))) > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        LnAr(3) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        PrevHorLinePos = CurY

        'CurY = CurY + TxtHgt - 10
        'If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Order No", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 45, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 60, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt + 5
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(4) = CurY

        'Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub









    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()

    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")

        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                'txt_Net_Amount.Focus()
            End If
        End If


    End Sub

    Private Sub txt_FinalRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyCode = 40 Then
        '    txt_Remarks.Focus()
        'End If
    End Sub

    Private Sub txt_FinalRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    txt_Remarks.Focus()
        'End If
    End Sub


    Private Sub lbl_JobNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ' txt_UID.Text = "SPC-" + lbl_JobNo.Text + "(" + Common_Procedures.FnYearCode + ")"

    End Sub

    Private Sub cbo_PartyName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_PartyName.SelectedIndexChanged

    End Sub

    Private Sub cbo_UID_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_UID.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Embroidery_Order_Entry

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_UID.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_UID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UID.LostFocus

        move_record()

    End Sub


    '

    Private Sub pnl_Filter_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Filter.Paint

    End Sub

    Private Sub cbo_Filter_UID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Filter_Status.SelectedIndexChanged

    End Sub

    Private Sub cbo_Filter_UID_KeyDown(sender As Object, e As EventArgs) Handles cbo_Filter_Status.KeyDown
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Order_Program_head", "Ordercode_forSelection", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text), "")
    End Sub

    Private Sub cbo_Filter_UID_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_Status.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Status, cbo_Filter_PartyName, btn_Filter_Show, "Order_Program_Head", "OrderCode_forSelection", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text), "")

    End Sub

    Private Sub cbo_Filter_UID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_Status.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Status, btn_Filter_Show, "Order_Program_Head", "OrderCode_forSelection", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text), "")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Status, cbo_Filter_Status, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Status, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_ThColCnt_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub



    Private Sub txt_RejectionAllowance_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub pnl_Back_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Back.Paint

    End Sub

    Private Sub txt_ProductionCost_TextChanged(sender As Object, e As EventArgs) Handles txt_ProductionCost.TextChanged

    End Sub

    Private Sub txt_ProductionCost_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_ProductionCost.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub cbo_UID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_UID.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_UID, cbo_PartyName, txt_ProductionCost, "Order_Program_Head", "OrderCode_forSelection", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text), "")
    End Sub

    Private Sub cbo_UID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_UID.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_UID, txt_ProductionCost, "Order_Program_Head", "OrderCode_forSelection", "Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text), "")
    End Sub

    Private Sub cbo_UID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_UID.SelectedIndexChanged

    End Sub

    Private Sub btn_ViewQuotation_Click(sender As Object, e As EventArgs) Handles btn_ViewQuotation.Click


        Common_Procedures.Print_OR_Preview_Status = 2
        Print_PDF_Status = False
        print_record()

    End Sub

    Private Sub dgv_Filter_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellContentClick

    End Sub

    Private Sub txt_Remarks_TextChanged(sender As Object, e As EventArgs) Handles txt_Remarks.TextChanged

    End Sub
End Class
