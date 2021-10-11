Public Class Company_Creation

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private CompType_Condt As String

    Private Sub clear()

        Dim obj As Object

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Or TypeOf obj Is ComboBox Then
                obj.text = ""
            End If
        Next

        lbl_CompID.ForeColor = Color.Black
        cbo_CompanyType.Text = "ACCOUNT"

    End Sub

    Public Sub move_record(ByVal IdNo As Integer)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(IdNo) = 0 Then Exit Sub

        Call clear()

        Try

            da = New SqlClient.SqlDataAdapter("select * from Company_Head Where " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " Company_IdNo = " & Str(Val(IdNo)), con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                lbl_CompID.Text = dt.Rows(0)("Company_IdNo").ToString
                txt_CompanyName.Text = dt.Rows(0)("Company_Name").ToString
                txt_ShortName.Text = dt.Rows(0)("Company_ShortName").ToString
                cbo_CompanyType.Text = dt.Rows(0)("Company_Type").ToString
                txt_ContactName.Text = dt.Rows(0)("Company_ContactPerson").ToString
                txt_Address1.Text = dt.Rows(0)("Company_Address1").ToString
                txt_Address2.Text = dt.Rows(0)("Company_Address2").ToString
                txt_Address3.Text = dt.Rows(0)("Company_Address3").ToString
                txt_Address4.Text = dt.Rows(0)("Company_Address4").ToString
                txt_City.Text = dt.Rows(0)("Company_City").ToString
                txt_PinCode.Text = dt.Rows(0)("Company_PinCode").ToString
                txt_PhoneNo.Text = dt.Rows(0)("Company_PhoneNo").ToString
                txt_FaxNo.Text = dt.Rows(0)("Company_FaxNo").ToString
                txt_ESINo.Text = dt.Rows(0)("Company_ESINo").ToString
                txt_EMail.Text = dt.Rows(0)("Company_EMail").ToString
                txt_Bank_Ac_Details.Text = dt.Rows(0)("Company_Bank_Ac_Details").ToString
                txt_Description.Text = dt.Rows(0)("Company_Description").ToString
                txt_PanNo.Text = dt.Rows(0)("Company_PanNo").ToString

                '-----------------GST ALTER------------------------------------

                txt_GSTIN_No.Text = dt.Rows(0)("Company_GSTinNo").ToString
                txt_Website.Text = dt.Rows(0)("Company_Website").ToString
                cbo_Company_Designation.Text = dt.Rows(0)("Company_Owner_Designation").ToString
                cbo_State.Text = Common_Procedures.State_IdNoToName(con, Val(dt.Rows(0)("Company_State_IdNo").ToString))

                '---------------------------------------------------------------

                If Not IsDBNull(dt.Rows(0)("GSTP_CA_Mail_Id")) Then
                    txt_GSTP_Email_ID.Text = dt.Rows(0)("GSTP_CA_Mail_Id")
                End If

                If txt_CompanyName.Enabled And txt_CompanyName.Visible Then txt_CompanyName.Focus()

                End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORD", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

        dt.Clear()

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record


        Dim cmd As New SqlClient.SqlCommand
        Dim new_idno As Integer

        clear()

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(company_idno) from company_head"


            new_idno = Val(cmd.ExecuteScalar())

        Catch ex As Exception
            new_idno = 0

        End Try

        cmd.Dispose()

        lbl_CompID.Text = new_idno + 1

        lbl_CompID.ForeColor = Color.Red

        If txt_CompanyName.Enabled And txt_CompanyName.Visible Then txt_CompanyName.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim pwd As String = ""

        pwd = InputBox("Enter Password :", "FOR COMPANY DELETION...")
        If Trim(UCase(pwd)) <> "TSDCOM" Then
            MessageBox.Show("Invalid Password", "DOES NOT DELETE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to delete", "FOR DELETION..", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where Company_Idno = " & Str(Val(lbl_CompID.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this company", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Item_Processing_Details where Company_Idno = " & Str(Val(lbl_CompID.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this company", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con
            cmd.CommandText = "delete from company_head where company_idno = " & Str(Val(lbl_CompID.Text))
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("Select Company_IdNo, Company_Name from Company_Head where " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            '.Rows.Clear()

            .DataSource = dt
            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            '.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "COMPANY NAME"

            .Columns(0).FillWeight = 35
            .Columns(1).FillWeight = 165

            grp_Filter.Visible = True

            .Focus()

        End With

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try

            cmd.Connection = con
            cmd.CommandText = "select min(company_idno) from company_head where  " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " Company_IdNo <> 0"
            cmd.ExecuteNonQuery()

            movid = 0
            If IsDBNull(cmd.ExecuteScalar) = False Then
                movid = cmd.ExecuteScalar
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(company_idno ) from company_head Where " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " company_idno <> 0"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar) = False Then
                movid = cmd.ExecuteScalar
            End If

            If movid <> 0 Then
                move_record(movid)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(company_idno) from company_head where  " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " company_idno > " & Str(Val(lbl_CompID.Text))

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            If movid > 0 Then
                move_record(movid)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(company_idno) from company_head where  " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " Company_IdNo <> 0 and company_idno < " & Str(Val(lbl_CompID.Text))

            movid = 0
            If IsDBNull(cmd.ExecuteScalar) = False Then
                movid = cmd.ExecuteScalar
            End If

            If movid <> 0 Then
                move_record(movid)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select company_name from company_head  " & IIf(Trim(CompType_Condt) <> "", " Where ", "") & CompType_Condt & " order by company_name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "company_name"

        grp_Open.Visible = True
        cbo_Open.Focus()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'MessageBox.Show("Company creation -  print")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim nr As Long
        Dim new_entry As Boolean = False
        Dim SurNm As String = ""
        Dim sTATE_iD As Integer = 0

        If Trim(txt_CompanyName.Text) = "" Then
            MessageBox.Show("Invalid company name", "DOES NOT SAVE", MessageBoxButtons.OK)
            Exit Sub
        End If

        If Trim(txt_ShortName.Text) = "" Then
            MessageBox.Show("Invalid Short name", "DOES NOT SAVE", MessageBoxButtons.OK)
            Exit Sub
        End If

        SurNm = Common_Procedures.Remove_NonCharacters(Trim(txt_CompanyName.Text))
        sTATE_iD = Common_Procedures.State_NameToIdNo(con, cbo_State.Text)

        trans = con.BeginTransaction

        Try
            cmd.CommandType = CommandType.Text
            cmd.CommandText = " Update Company_Head set Company_Name = '" & Trim(txt_CompanyName.Text) & "', Company_SurName = '" & Trim(SurNm) & "'," &
                              " Company_Address1 = '" & Trim(txt_Address1.Text) & "', Company_Address2 = '" & Trim(txt_Address2.Text) & "', " &
                              " Company_Address3 = '" & Trim(txt_Address3.Text) & "', Company_Address4 = '" & Trim(txt_Address4.Text) & "', " &
                              " Company_City = '" & Trim(txt_City.Text) & "', Company_PinCode = '" & Trim(txt_PinCode.Text) & "', " &
                              " company_PhoneNo = '" & Trim(txt_PhoneNo.Text) & "', Company_FaxNo = '" & Trim(txt_FaxNo.Text) & "', " &
                              " Company_ESINo = '" & Trim(txt_ESINo.Text) & "', Company_ShortName = '" & Trim(txt_ShortName.Text) & "', " &
                              " Company_Type = '" & Trim(cbo_CompanyType.Text) & "', Company_EMail = '" & Trim(txt_EMail.Text) & "', " &
                              " Company_ContactPerson = '" & Trim(txt_ContactName.Text) & "', Company_Bank_Ac_Details = '" & Trim(txt_Bank_Ac_Details.Text) & "'," &
                              " Company_Description = '" & Trim(txt_Description.Text) & "',Company_PanNo = '" & Trim(txt_PanNo.Text) & "', " &
                              " Company_GSTinNo='" & Trim(txt_GSTIN_No.Text) & "',Company_Owner_Designation='" & Trim(cbo_Company_Designation.Text) & "'," &
                              " Company_Website='" & Trim(txt_Website.Text) & "',Company_State_IdNo= " & Str(sTATE_iD) & "," &
                              " GSTP_CA_Mail_Id = '" & txt_GSTP_Email_ID.Text & "' where Company_IdNo = " & Str(Val(lbl_CompID.Text))
            cmd.Connection = con
            cmd.Transaction = trans

            nr = cmd.ExecuteNonQuery

            If nr = 0 Then

                cmd.CommandText = "Insert into Company_Head ( Company_IdNo                  ,       Company_Name,                   Company_SurName     ,       Company_Address1            ,   Company_Address2            ,   Company_Address3                ,       Company_Address4            ,   Company_City                ,       Company_PinCode         ,       Company_PhoneNo             ,       Company_FaxNo           ,       Company_ESINo           ,       Company_ShortName           ,       Company_Type                ,       Company_EMail           ,       Company_ContactPerson           ,       Company_Bank_Ac_Details         ,       Company_Description             ,           Company_PanNo       ,       Company_GSTinNo             ,       Company_Owner_Designation           ,       Company_Website         ,   Company_State_IdNo    ,GSTP_CA_Mail_Id) " &
                                 "  values               ( " & Str(Val(lbl_CompID.Text)) & ", '" & Trim(txt_CompanyName.Text) & "', '" & Trim(SurNm) & "', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', '" & Trim(txt_Address4.Text) & "', '" & Trim(txt_City.Text) & "', '" & Trim(txt_PinCode.Text) & "', '" & Trim(txt_PhoneNo.Text) & "', '" & Trim(txt_FaxNo.Text) & "', '" & Trim(txt_ESINo.Text) & "', '" & Trim(txt_ShortName.Text) & "', '" & Trim(cbo_CompanyType.Text) & "', '" & Trim(txt_EMail.Text) & "', '" & Trim(txt_ContactName.Text) & "', '" & Trim(txt_Bank_Ac_Details.Text) & "', '" & Trim(txt_Description.Text) & "' ,'" & Trim(txt_PanNo.Text) & "','" & Trim(txt_GSTIN_No.Text) & "','" & Trim(cbo_Company_Designation.Text) & "','" & Trim(txt_Website.Text) & "', " & Str(sTATE_iD) & "   ,'" & txt_GSTP_Email_ID.Text & "')"
                cmd.ExecuteNonQuery()

                new_entry = True

            End If

            trans.Commit()

            MessageBox.Show("Saved", "For SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If new_entry = True Then
                new_record()
            End If

        Catch ex As Exception

            trans.Rollback()

            If InStr(1, LCase(ex.Message), "duplicate_companyhead_name") > 0 Or InStr(1, LCase(ex.Message), "duplicate_companyhead_surname") > 0 Then
                MessageBox.Show("Duplicate Company Name", "DOES Not SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, LCase(ex.Message), "duplicate_companyhead_shortname") > 0 Then
                MessageBox.Show("Duplicate Company Short Name", "DOES Not SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES Not SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            Exit Sub

        End Try

        If txt_CompanyName.Enabled And txt_CompanyName.Visible Then txt_CompanyName.Focus()

    End Sub

    Private Sub Company_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        da = New SqlClient.SqlDataAdapter("Select State_Name from State_Head Order by State_Name", con)
        da.Fill(dt1)
        cbo_State.Items.Clear()
        cbo_State.DataSource = dt1
        cbo_State.DisplayMember = "State_Name"

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" Then
            lbl_CompIDCaption.Visible = True
            lbl_CompID.Visible = True

            lbl_CompanyType.Visible = True
            cbo_CompanyType.Visible = True

            txt_ShortName.Width = 125

            CompType_Condt = ""

        Else

            lbl_CompIDCaption.Visible = False
            lbl_CompID.Visible = False

            lbl_CompanyType.Visible = False
            cbo_CompanyType.Visible = False

            txt_ShortName.Width = txt_CompanyName.Width

            CompType_Condt = "(Company_Type <> 'UNACCOUNT')"

        End If

        grp_Open.Visible = False
        grp_Open.BackColor = Me.BackColor
        grp_Open.Left = (Me.Width - grp_Open.Width) - 50
        grp_Open.Top = (Me.Height - grp_Open.Height) - 50

        grp_Filter.Visible = False
        grp_Filter.BackColor = Me.BackColor
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 50
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 50

        con.Open()

        new_record()

    End Sub

    Private Sub Company_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub Company_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible = True Then
                grp_Open.Visible = False
                Exit Sub
            End If
            If dgv_Filter.Visible = True Then
                grp_Filter.Visible = False
                Exit Sub
            End If
            Me.Close()
        End If
    End Sub

    Private Sub txt_CompanyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CompanyName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_ContactName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ContactName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address3.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address4.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_City_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_City.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PinCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PinCode.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PhoneNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PhoneNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_TinNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FaxNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_ShortName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ShortName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_EMail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EMail.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Description.KeyPress

        If Asc(e.KeyChar) = 13 Then

            txt_GSTP_Email_ID.Focus()

        End If

    End Sub

    Private Sub txt_PanNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PanNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txt_PanNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PanNo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_CompanyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CompanyName.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_ContactName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ContactName.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address1.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address2.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address3.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address4_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address4.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_City_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_City.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PinCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PinCode.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PhoneNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PhoneNo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_TinNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FaxNo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_ShortName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ShortName.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_EMail_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EMail.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Description_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Description.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else

                txt_CompanyName.Focus()
            End If
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim idno As Integer

        Try

            cmd.CommandText = "select company_idno from company_head where company_idno <> 0 and company_name = '" & Trim(cbo_Open.Text) & "'"
            cmd.Connection = con

            dr = cmd.ExecuteReader

            If dr.HasRows() Then
                If dr.Read() Then
                    idno = Val(dr("company_idno"))
                    If Val(idno) <> 0 Then
                        dr.Close()
                        move_record(Val(idno))
                        grp_Open.Visible = False
                    End If
                Else
                    dr.Close()
                End If
            Else
                dr.Close()
            End If

            cmd.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus

        With cbo_Open
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Open.Text.Length
        End With

    End Sub

    Private Sub cbo_Open_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.LostFocus
        cbo_Open.BackColor = Color.White
        cbo_Open.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Try
            With cbo_Open
                If e.KeyValue = 38 And .DroppedDown = False Then
                    'e.Handled = True
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    'e.Handled = True
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        'Try

        With cbo_Open

            If Asc(e.KeyChar) <> 27 Then

                If Asc(e.KeyChar) = 13 Then

                    With cbo_Open
                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If
                    End With

                    Call btn_Find_Click(sender, e)

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

                    FindStr = LTrim(FindStr)

                    Condt = IIf(Trim(CompType_Condt) <> "", " Where ", "") & Trim(CompType_Condt)
                    If Trim(FindStr) <> "" Then
                        Condt = " Where  " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " (Company_Name like '" & Trim(FindStr) & "%' or Company_Name like '% " & Trim(FindStr) & "%') "
                    End If

                    da = New SqlClient.SqlDataAdapter("select Company_Name from Company_Head " & Condt & " order by Company_Name", con)
                    da.Fill(dt)

                    .DataSource = dt
                    .DisplayMember = "Company_Name"

                    .Text = FindStr

                    .SelectionStart = FindStr.Length

                    e.Handled = True

                    da.Dispose()

                End If

            End If

        End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim IdNo As Integer

        IdNo = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        If Val(IdNo) <> 0 Then
            Call move_record(IdNo)
            grp_Filter.Visible = False
        End If

    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            btn_Open_Click(sender, e)
        End If
    End Sub


    Private Sub cbo_CompanyType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CompanyType.GotFocus
        With cbo_CompanyType
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            '.SelectionStart = -1
            '.SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_CompanyType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CompanyType.KeyPress
        If Asc(e.KeyChar) = 13 And cbo_CompanyType.DroppedDown = False Then
            txt_ContactName.Focus()
        End If
        If Asc(e.KeyChar) = 32 Then
            cbo_CompanyType.DroppedDown = True
        End If
    End Sub

    Private Sub cbo_CompanyType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CompanyType.KeyUp
        Try
            With cbo_CompanyType
                If e.KeyValue = 38 And .DroppedDown = False Then

                    txt_ShortName.Focus()
                    e.Handled = True
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_ContactName.Focus()
                    'SendKeys.Send("{TAB}")
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_CompanyType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CompanyType.LostFocus
        With cbo_CompanyType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub txt_CstNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ESINo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If

    End Sub

    Private Sub txt_CstNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ESINo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Bank_Ac_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bank_Ac_Details.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Bank_Ac_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Bank_Ac_Details.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cbo_Company_Designation_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company_Designation.GotFocus
        With cbo_Company_Designation
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            '.SelectionStart = 0
            '.SelectionLength = .Text.Length
        End With
    End Sub


    Private Sub cbo_Company_Designation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company_Designation.KeyPress
        If Asc(e.KeyChar) = 13 And cbo_Company_Designation.DroppedDown = False Then
            txt_Address1.Focus()
        End If
        If Asc(e.KeyChar) = 32 Then
            cbo_Company_Designation.DroppedDown = True
        End If
    End Sub

    Private Sub cbo_Company_Designation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Company_Designation.KeyUp
        Try
            With cbo_Company_Designation
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_ContactName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_Address1.Focus()
                    'SendKeys.Send("{TAB}")

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Company_Designation_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company_Designation.LostFocus
        With cbo_Company_Designation
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_State_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.GotFocus
        With cbo_State
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            '.SelectionStart = 0
            '.SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_State_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_State.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_State, "", "", "State_Head", "State_Name", "", "(State_Idno = 0)")
    End Sub


    Private Sub cbo_State_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_State.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_State, "", "State_Head", "State_Name", "", "(State_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
        If Asc(e.KeyChar) = 32 Then
            cbo_State.DroppedDown = True
        End If
    End Sub

    Private Sub cbo_State_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_State.KeyUp
        Try
            With cbo_State
                If e.KeyValue = 38 And .DroppedDown = False Then
                    'e.Handled = True
                    'txt_Address4.Focus()
                    SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    'e.Handled = True
                    'txt_PhoneNo.Focus()
                    SendKeys.Send("{TAB}")
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_State_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.LostFocus
        With cbo_State
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub



    Private Sub txt_GSTIN_No_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GSTIN_No.GotFocus
        With txt_GSTIN_No
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub txt_GSTIN_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GSTIN_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_GSTIN_No_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTIN_No.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_GSTIN_No_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GSTIN_No.LostFocus
        With txt_GSTIN_No
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub



    Private Sub txt_Website_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Website.GotFocus
        With txt_Website
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub txt_Website_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Website.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Website_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Website.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Website_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Website.LostFocus
        With txt_Website
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub


    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_GSTIN_No_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_GSTIN_No.TextChanged

    End Sub

    Private Sub txt_Description_TextChanged(sender As Object, e As EventArgs) Handles txt_Description.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txt_GSTP_Email_ID.TextChanged

    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_GSTP_Email_ID.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else

                txt_CompanyName.Focus()
            End If
        End If

    End Sub

End Class