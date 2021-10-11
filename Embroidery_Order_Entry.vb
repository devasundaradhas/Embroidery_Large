Imports System.IO

Public Class Embroidery_Order_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False

    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private cmbItmNm As String
    Private cmbszNm As String
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private vcmb_ItmNm As String

    Public previlege As String

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_JobNo.Text = ""
        lbl_JobNo.ForeColor = Color.Black

        cbo_PartyName.Text = ""

        txt_Design.Text = ""
        txt_NoOfPcs.Text = ""
        txt_NoOfStiches.Text = ""

        cbo_colour.Text = ""
        cbo_Size.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
        txt_Stch_Pcs.Text = ""
        txt_StyleNo.Text = ""

        cbo_Billing_PartyName.Text = ""

        chk_AgainstCForm.Checked = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            'cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            'cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Unit.Text = "PCS-PIECES"

        Picture_Box.BackgroundImage = Nothing

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

    Private Sub move_record(ByVal no As String)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Order_Program_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.Order_Program_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_JobNo.Text = dt1.Rows(0).Item("Order_Program_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Order_Program_Date").ToString
                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString

                txt_Design.Text = dt1.Rows(0).Item("Design").ToString
                txt_Stch_Pcs.Text = Val(dt1.Rows(0).Item("StchsPr_Pcs").ToString)

                txt_NoOfStiches.Text = Val(dt1.Rows(0).Item("Stiches").ToString)
                txt_NoOfPcs.Text = Val(dt1.Rows(0).Item("Pieces").ToString)

                txt_Rate.Text = Val(dt1.Rows(0).Item("Rate").ToString)
                chk_UpdateNewRate.Checked = False

                lbl_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "########0.00")
                cbo_colour.Text = Common_Procedures.Colour_IdNoToName(con, Val(dt1.Rows(0).Item("Colour_IdNo").ToString))
                cbo_Size.Text = Common_Procedures.Size_IdNoToName(con, Val(dt1.Rows(0).Item("Size_IdNo").ToString))
                If Val(dt1.Rows(0).Item("Close_Status").ToString) = 1 Then chk_AgainstCForm.Checked = True

                If Not IsDBNull(dt1.Rows(0).Item("Billing_Name_IdNo")) Then
                    cbo_Billing_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("Billing_Name_IdNo").ToString)
                End If

                If IsDBNull(dt1.Rows(0).Item("Order_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(dt1.Rows(0).Item("Order_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                Picture_Box.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If

            End If

            If Not IsDBNull(dt1.Rows(0).Item("Style_Ref_No")) Then
                txt_StyleNo.Text = dt1.Rows(0).Item("Style_Ref_No")
            End If

            If Not IsDBNull(dt1.Rows(0).Item("Unit_IdNo")) Then
                cbo_Unit.Text = Common_Procedures.Unit_IdNoToName(con, dt1.Rows(0).Item("Unit_IdNo"))
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub Order_Program_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BRAND" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Order_Program_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'dtp_Date.MaxDate = Common_Procedures.settings.Validation_End_Date

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        con.Open()


        'da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (b.ledger_idno = 0 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        'da.Fill(dt2)

        'cbo_PartyName.DataSource = dt2
        'cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        'cbo_Billing_Name.DataSource = dt2
        'cbo_Billing_Name.DisplayMember = "Ledger_DisplayName"

        pnl_Filter.Visible = False

        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Billing_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Design.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Stch_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfStiches.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Design.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_StyleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btnClose.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Billing_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Design.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Stch_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfStiches.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Design.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_StyleNo.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_Save.LostFocus, AddressOf ControlLostFocus
        AddHandler btnClose.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Design.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Stch_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfPcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfStiches.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_StyleNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Design.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Stch_Pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfPcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfStiches.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_StyleNo.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        If Common_Procedures.settings.CustomerCode = "1244" Then

            Label39.Visible = False
            Label6.Visible = False
            txt_Stch_Pcs.Visible = False
            txt_NoOfStiches.Visible = False

        End If

        If Val(Common_Procedures.settings.CustomerCode) = 5022 Then
            Label12.Text = Replace(Label12.Text, "SPC", "RVM")
        ElseIf Val(Common_Procedures.settings.CustomerCode) = 5027 Then
            Label12.Text = Replace(Label12.Text, "SPC", "FWC")
        End If

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Purchase_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Purchase_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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

        Dim NewCode As String = ""

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Order_Program_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code = '" & Trim(NewCode) & "' and ( Receipt_Pieces <> 0 or Delivery_Pieces <> 0 or Production_Pieces <> 0 ) ", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                If Val(Dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Invoice Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt.Clear()

        Try

            cmd.Connection = con

            cmd.CommandText = "Delete from Order_Program_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
            da.Fill(dt2)
            'cbo_Filter_ItemName.DataSource = dt2
            'cbo_Filter_ItemName.DisplayMember = "item_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate

            cbo_Filter_PartyName.Text = ""
            'cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            'cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

        If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("I") Then
            MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
            Exit Sub
        End If

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Job No.", "FOR NEW REC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Order_Program_No from Order_Program_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code = '" & Trim(RecCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Lot No", "DOES NOT INSERT NEW REC...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_JobNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Order_Program_No from Order_Program_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Order_Program_No", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Order_Program_No from Order_Program_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Order_Program_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Order_Program_No from Order_Program_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Order_Program_No", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Order_Program_No from Order_Program_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Order_Program_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try

            clear()

            dtp_Date.Text = Date.Today.ToShortDateString

            New_Entry = True

            lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Order_Program_Head", "Order_Program_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_JobNo.ForeColor = Color.Red

            chk_UpdateNewRate.Checked = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt.Dispose()
            da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try



    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Job.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Order_Program_No from Order_Program_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Job No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()


        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
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
        Dim NewCode As String = ""
        Dim Nr As Long = 0
        Dim Itm_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim Cls_Sts As Integer = 0
        Dim Sno As Integer = 0
        Dim Selc_OrderCode As String = ""
        Dim Unt_Id As Int16

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If
        Siz_ID = Common_Procedures.Size_NameToIdNo(con, cbo_Size.Text)
        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, cbo_colour.Text)
        Cls_Sts = 0

        If Len(Trim(cbo_Billing_PartyName.Text)) = 0 Then
            cbo_Billing_PartyName.Text = cbo_PartyName.Text
        End If

        If chk_AgainstCForm.Checked = True Then Cls_Sts = 1

        If Not New_Entry Then
            If Not UCase(previlege).Contains("L") And Not UCase(previlege).Contains("E") Then
                MsgBox("THIS USER DOES NOT HAVE THE PREVILEGE TO EXECUTE THIS ACTION")
                Exit Sub
            End If
        End If

        If Len(Trim(cbo_Unit.Text)) = 0 Then
            cbo_Unit.Text = "PCS-PIECES"
        End If

        Unt_Id = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Order_Program_Head ", "Order_Program_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            Selc_OrderCode = ""
            Selc_OrderCode = txt_UID.Text

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@Order_ProgramDate", dtp_Date.Value.Date)
            Dim ms As New MemoryStream()
            If IsNothing(Picture_Box.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(Picture_Box.BackgroundImage)
                bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                'PictureBox1.BackgroundImage.Save(ms, PictureBox1.BackgroundImage.RawFormat)
            End If

            Dim data As Byte() = ms.GetBuffer()
            Dim p As New SqlClient.SqlParameter("@photo", SqlDbType.Image)
            p.Value = data
            cmd.Parameters.Add(p)
            ms.Dispose()

            If New_Entry = True Then
                cmd.CommandText = "Insert into Order_Program_Head(Order_Program_Code       , Company_IdNo                     , Order_Program_No              , for_OrderBy                                                            , Order_Program_Date,  Ledger_IdNo            ,   Design                        , StchsPr_Pcs                        , Stiches                               ,  Pieces                           ,  Rate                            ,  Amount                          , Printing_Order_Code , Printing_Order_Details_SlNo , Printing_Invoice_Code , Printing_Invoice_slno , Ordercode_forSelection         , Close_Status              ,Order_Image,Colour_IdNo           ,  Size_Idno         ,  Billing_Name_IdNo                                                                             ,Style_Ref_No             ,Unit_IdNo) " &
                                                          " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @Order_ProgramDate, " & Str(Val(Led_ID)) & ",  '" & Trim(txt_Design.Text) & "', " & Str(Val(txt_Stch_Pcs.Text)) & ", " & Str(Val(txt_NoOfStiches.Text)) & ", " & Str(Val(txt_NoOfPcs.Text)) & ",  " & Str(Val(txt_Rate.Text)) & " ," & Str(Val(lbl_Amount.Text)) & " , ''                  , ''                          , ''                    , ''                    , '" & Trim(Selc_OrderCode) & "' , " & Str(Val(Cls_Sts)) & " , @photo    , " & Val(Clr_ID) & "  , " & Val(Siz_ID) & ", " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Billing_PartyName.Text, tr).ToString & ",'" & txt_StyleNo.Text & "'," & Unt_Id.ToString & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Order_Program_Head Set Order_Program_Date = @Order_ProgramDate, Ledger_IdNo = " & Str(Val(Led_ID)) & " ,  Design ='" & Trim(txt_Design.Text) & "'    , StchsPr_Pcs = " & Str(Val(txt_Stch_Pcs.Text)) & "   , Stiches = " & Str(Val(txt_NoOfStiches.Text)) & "  ,  Pieces = " & Str(Val(txt_NoOfPcs.Text)) & " ,  Rate = " & Str(Val(txt_Rate.Text)) & "  ,  Amount= " & Str(Val(lbl_Amount.Text)) & " , Printing_Order_Code = '' ,Ordercode_forSelection = '" & Trim(Selc_OrderCode) & "' , Close_Status =  " & Str(Val(Cls_Sts)) & ", Order_Image =  @photo, Colour_IDno = " & Val(Clr_ID) & "   , Size_IdNo = " & Val(Siz_ID) & ", Style_Ref_No = '" & txt_StyleNo.Text & "', Billing_Name_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Billing_PartyName.Text, tr) & ",Unit_IdNo = " & Unt_Id.ToString & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Order_Program_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If

            tr.Commit()

            If New_Entry = True Then
                move_record(lbl_JobNo.Text)
            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbo_partyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, dtp_Date, txt_StyleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_partyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, txt_StyleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
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

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Order_Program_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Order_Program_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Order_Program_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            'If Val(Itm_IdNo) <> 0 Then
            'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Itm_IdNo))
            'End If

            'LEFT OUTER JOIN Item_Head c ON a.Item_IdNo = c.Item_IdNo 
            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, '',d.Size_Name from Order_Program_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Size_Head d ON a.Size_IdNo = d.Size_IdNo where  a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Order_Program_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Order_Program_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Order_Program_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Order_Program_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Design").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("size_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Pieces").ToString)

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            dt1.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True 'SendKeys.Send("+{TAB}")
    End Sub

    'Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    'End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    'Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, dtp_Filter_ToDate, cbo_Filter_PartyName, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
    'End Sub

    'Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
    'End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        Try
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            '---

        End Try


    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            Open_FilterEntry()
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

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_Billing_PartyName.Focus()
            e.Handled = True
        End If

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_NoOfPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfPcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoOfPcs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoOfPcs.TextChanged
        lbl_Amount.Text = Format(Val(txt_NoOfPcs.Text) * Val(txt_Rate.Text), "#########0.00")
    End Sub

    Private Sub txt_NoOfStiches_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfStiches.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        lbl_Amount.Text = Format(Val(txt_NoOfPcs.Text) * Val(txt_Rate.Text), "#########0.00")
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        With txt_Design
            vcmb_ItmNm = Trim(.Text)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Order_Program_Head", "Design", "", "")
        End With

    End Sub


    Private Sub btn_BrowsePhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BrowsePhoto.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Picture_Box.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_EnLargeImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnLargeImage.Click
        If IsNothing(Picture_Box.BackgroundImage) = False Then

            EnlargePicture.Text = "IMAGE   -   Design 1. : " & lbl_JobNo.Text
            EnlargePicture.PictureBox2.ClientSize = Picture_Box.BackgroundImage.Size
            EnlargePicture.PictureBox2.Image = CType(Picture_Box.BackgroundImage.Clone, Image)
            EnlargePicture.ShowDialog()

        End If

    End Sub

    Private Sub Btn_Clear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Clear1.Click
        Picture_Box.BackgroundImage = Nothing
    End Sub
    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "size_head", "size_name", "", "(Size_IdNo = 0)")
    End Sub

    Private Sub cbo_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, cbo_colour, txt_Stch_Pcs, "size_head", "size_name", "", "(size_idno = 0)")
    End Sub

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, Nothing, "size_head", "size_name", "", "(size_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub cbo_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_colour.GotFocus

    End Sub

    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_colour, txt_Design, cbo_Size, "Colour_head", "Colour_name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_colour, cbo_Size, "Colour_head", "Colour_name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_colour.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub Calculate_Rate()
        txt_Rate.Text = Format(Val(txt_Stch_Pcs.Text) * Val(txt_NoOfStiches.Text) / 1000, "########.##")
    End Sub

    Private Sub txt_Stch_Pcs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Stch_Pcs.TextChanged
        Calculate_Rate()
    End Sub

    Private Sub txt_NoOfStiches_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_NoOfStiches.TextChanged
        Calculate_Rate()
    End Sub


    'Private Sub cbo_OrderNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim BUFF As String = cbo_OrderNo.Text

    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable

    '    Me.Text = ""

    '    da = New SqlClient.SqlDataAdapter("select Order_No As OrderNumber From Sales_Quotation_Head Where Ledger_IdNo = " & Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text).ToString &
    '                                      "  Union Select OrderNo_Name as OrderNumber from OrderNo_Head Order By 1 ", con)
    '    da.Fill(dt1)
    '    cbo_OrderNo.DataSource = dt1
    '    cbo_OrderNo.DisplayMember = "OrderNumber"

    '    cbo_OrderNo.Text = BUFF

    'End Sub

    'Private Sub cbo_OrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_OrderNo, cbo_PartyName, txt_Design, "", "", "", "")
    'End Sub

    'Private Sub cbo_OrderNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_OrderNo, txt_Design, "", "", "", "", True)
    'End Sub

    'Private Sub cbo_OrderNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

    '        Dim f As New OrderNo_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_OrderNo.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()
    '    End If

    '    'OrderJobNo_Creation

    'End Sub

    'Private Sub cbo_OrderNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

    '    If chk_UpdateNewRate.Checked Then

    '        txt_Rate.Text = "0.00"

    '        Dim da As New SqlClient.SqlDataAdapter
    '        Dim dt1 As New DataTable

    '        Me.Text = ""

    '        da = New SqlClient.SqlDataAdapter("select isnull(Finalised_Rate,0),Isnull(Stitches1,0)+Isnull(Stitches2,0),isnull(Rate_Stitches,0),Isnull(Design1,''),Isnull(Design2,''),Sales_Quotation_Image " &
    '                                          "From Sales_Quotation_Head Where Order_No = '" & cbo_OrderNo.Text & "' ", con)
    '        da.Fill(dt1)
    '        If dt1.Rows.Count > 0 Then


    '            txt_Stch_Pcs.Text = FormatNumber(dt1.Rows(0).Item(1), 0, TriState.False, TriState.False, TriState.False)
    '            txt_NoOfStiches.Text = FormatNumber(dt1.Rows(0).Item(2), 2, TriState.False, TriState.False, TriState.False)
    '            txt_Design.Text = dt1.Rows(0).Item(3)
    '            If Len(Trim(dt1.Rows(0).Item(4))) > 0 Then
    '                If Len(Trim(txt_Design.Text)) > 0 Then
    '                    txt_Design.Text = txt_Design.Text + "/"
    '                End If
    '                txt_Design.Text = txt_Design.Text + dt1.Rows(0).Item(4)
    '            End If

    '            txt_Rate.Text = FormatNumber(dt1.Rows(0).Item(0), 2, TriState.False, TriState.False, TriState.False)

    '            If IsDBNull(dt1.Rows(0).Item("Sales_Quotation_Image")) = False Then
    '                Dim imageData As Byte() = DirectCast(dt1.Rows(0).Item("Sales_Quotation_Image"), Byte())
    '                If Not imageData Is Nothing Then
    '                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
    '                        ms.Write(imageData, 0, imageData.Length)
    '                        If imageData.Length > 0 Then

    '                            Picture_Box.BackgroundImage = Image.FromStream(ms)

    '                        End If
    '                    End Using
    '                End If
    '            End If

    '        End If
    '    End If

    'End Sub




    Private Sub lbl_JobNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_JobNo.TextChanged

        If Val(Common_Procedures.settings.CustomerCode) = 5010 Then
            txt_UID.Text = "SPC-" & lbl_JobNo.Text & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))
        ElseIf Val(Common_Procedures.settings.CustomerCode) = 5022 Then
            txt_UID.Text = "RVM-" & lbl_JobNo.Text & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))
        ElseIf Val(Common_Procedures.settings.CustomerCode) = 5027 Then
            txt_UID.Text = "FWC-" & lbl_JobNo.Text & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))
        End If

    End Sub



    Private Sub cbo_BillingName_KeyPress(sender As Object, e As KeyPressEventArgs)

        ' Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Billing_Name, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_BillingName_KeyDown(sender As Object, e As KeyEventArgs)
        vcbo_KeyDwnVal = e.KeyValue
       ' Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Billing_Name, txt_Rate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_PartyName.SelectedIndexChanged

    End Sub

    Private Sub cbo_BillingName_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txt_Design_TextChanged(sender As Object, e As EventArgs) Handles txt_Design.TextChanged

    End Sub

    Private Sub txt_StyleNo_TextChanged(sender As Object, e As EventArgs) Handles txt_StyleNo.TextChanged

    End Sub

    Private Sub pnl_Back_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Back.Paint

    End Sub

    Private Sub cbo_BillingName_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbo_colour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_colour.SelectedIndexChanged

    End Sub

    Private Sub cbo_PartyName_GotFocus(sender As Object, e As EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Billing_PartyName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Billing_PartyName.SelectedIndexChanged

    End Sub

    Private Sub cbo_Billing_PartyName_GotFocus(sender As Object, e As EventArgs) Handles cbo_Billing_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Billing_PartyName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Billing_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Billing_PartyName, txt_Rate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Billing_PartyName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Billing_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Billing_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MsgBox("Do you want to Save ?", vbYesNo, "Save ?") = vbYes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub



    Private Sub cbo_Size_Enter(sender As Object, e As EventArgs) Handles cbo_Size.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_head", "Colour_name", "", "(Colour_IdNo = 0)")
    End Sub


    Private Sub cbo_Unit_Enter(sender As Object, e As EventArgs) Handles cbo_Unit.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_head", "Unit_name", "", "(Unit_IdNo = 0)")
    End Sub

    Private Sub cbo_Unit_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Unit.SelectedIndexChanged

    End Sub

    Private Sub cbo_Unit_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Unit.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, txt_NoOfPcs, txt_Rate, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
    End Sub

    Private Sub cbo_Unit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_Rate, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
    End Sub

    Private Sub cbo_Unit_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Unit.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

End Class
