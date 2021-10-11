Public Class Ledger_Creation_SMS
    Implements Interface_MDIActions
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1


    Private Sub clear()
        Dim obj As Object
        Dim ctrl As Object
        Dim grpbx As GroupBox
        Dim pnlbx As Panel

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""
            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""
            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl In grpbx.Controls
                    If TypeOf ctrl Is TextBox Then
                        ctrl.text = ""
                    ElseIf TypeOf ctrl Is ComboBox Then
                        ctrl.text = ""
                    End If

                Next

            ElseIf TypeOf obj Is Panel Then
                pnlbx = obj
                For Each ctrl In pnlbx.Controls
                    If TypeOf ctrl Is TextBox Then
                        ctrl.text = ""
                    ElseIf TypeOf ctrl Is ComboBox Then
                        ctrl.text = ""
                    End If

                Next

            End If

        Next

        New_Entry = False

        cbo_BillType.Text = "BALANCE ONLY"

        lbl_IdNo.ForeColor = Color.Black

        dgv_Details.Rows.Clear()
        dtp_BirthDate.Text = ""
        msk_BirthDate.Text = ""
        dtp_WeddingDate.Text = ""
        msk_WeddingDate.Text = ""


        grp_Back.Enabled = True
        grp_Open.Visible = False
        grp_Filter.Visible = False
        pnl_Reading.Visible = False

        cbo_Machine.Visible = False
        'cbo_Machine.Text = ""
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

        If Me.ActiveControl.Name <> cbo_Machine.Name Then
            cbo_Machine.Visible = False
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim SNo As Integer = 0
        Dim n As Integer = 0
        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.* , b.AccountsGroup_Name, c.Area_Name,  d.Price_List_Name  from ledger_head a LEFT OUTER JOIN Area_Head c ON a.Area_IdNo = c.Area_IdNo LEFT OUTER JOIN Price_List_Head d ON a.Price_List_IdNo = d.Price_List_IdNo , AccountsGroup_Head b where a.ledger_idno = " & Str(Val(idno)) & " and a.AccountsGroup_IdNo = b.AccountsGroup_IdNo", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            lbl_IdNo.Text = dt.Rows(0).Item("Ledger_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Ledger_MainName").ToString
            txt_AlaisName.Text = dt.Rows(0).Item("Ledger_AlaisName").ToString
            cbo_Area.Text = dt.Rows(0)("Area_Name").ToString
            cbo_AcGroup.Text = dt.Rows(0)("AccountsGroup_Name").ToString
            cbo_LedgerGroup.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0)("LedgerGroup_Idno").ToString)
            cbo_BillType.Text = dt.Rows(0)("Bill_Type").ToString
            txt_Address1.Text = dt.Rows(0)("Ledger_Address1").ToString
            txt_Address2.Text = dt.Rows(0)("Ledger_Address2").ToString
            txt_Address3.Text = dt.Rows(0)("Ledger_Address3").ToString
            txt_Address4.Text = dt.Rows(0)("Ledger_Address4").ToString
            txt_EmailID.Text = dt.Rows(0)("Ledger_EmailID").ToString
            txt_PhoneNo.Text = dt.Rows(0)("Ledger_PhoneNo").ToString
            txt_TinNo.Text = dt.Rows(0)("Ledger_TinNo").ToString
            txt_CstNo.Text = dt.Rows(0)("Ledger_CstNo").ToString
            txt_PanNo.Text = dt.Rows(0)("Pan_No").ToString
            cbo_PriceListName.Text = dt.Rows(0)("Price_List_Name").ToString
            Cbo_State.Text = Common_Procedures.State_IdNoToName(con, dt.Rows(0)("State_Idno").ToString)
            cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0)("Agent_idNo").ToString)

            If IsDBNull(dt.Rows(0).Item("Birth_Date")) = False Then
                If IsDate(dt.Rows(0).Item("Birth_Date")) = True Then
                    dtp_BirthDate.Text = dt.Rows(0).Item("Birth_Date").ToString
                    msk_BirthDate.Text = dtp_BirthDate.Text
                End If
            End If
            If IsDBNull(dt.Rows(0).Item("Wedding_Date")) = False Then
                If IsDate(dt.Rows(0).Item("Wedding_Date")) = True Then
                    dtp_WeddingDate.Text = dt.Rows(0).Item("Wedding_Date").ToString
                    msk_WeddingDate.Text = dtp_WeddingDate.Text
                End If
            End If

        End If

        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Call clear()

        lbl_IdNo.ForeColor = Color.Red
        New_Entry = True

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "")

        If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Item_Processing_Details where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Purchase_Head where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Sales_Head where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con

            cmd.CommandText = "delete from Ledger_Reading_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_PhoneNo_Head where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ledger_head where ledger_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()
            dt.Dispose()
            da.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select ledger_idno, ledger_name from ledger_head where ledger_idno <> 0 order by ledger_idno", con)
        Dim dt As New DataTable

        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt
        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "LEDGER NAME"

        dgv_Filter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 35
        dgv_Filter.Columns(1).FillWeight = 165

        grp_Filter.Visible = True
        grp_Filter.BringToFront()
        dgv_Filter.Focus()

        grp_Back.Enabled = False

        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(ledger_idno) from ledger_head Where  Ledger_Type = '' and ledger_idno <> 0"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno) from ledger_head where  Ledger_Type = '' and ledger_idno <> 0"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand("select min(ledger_idno) from ledger_head where  Ledger_Type = '' and ledger_idno <> 0 and ledger_idno > " & Str(Val(lbl_IdNo.Text)), con)
        Dim movid As Integer = 0

        Try
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno ) from ledger_head where  Ledger_Type = '' and  ledger_idno <> 0 and ledger_idno < " & Str((lbl_IdNo.Text))

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        grp_Back.Enabled = False

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'MessageBox.Show("Ledger creation  -  print")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim nr As Long = 0
        Dim acgrp_idno As Integer = 0
        Dim ar_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        Dim PhAr() As String
        Dim Sno As Integer = 0
        Dim PrcLst_idno As Integer = 0
        Dim Mac_id As Integer = 0
        Dim Grp_idno As Integer = 0
        Dim State_idno As Integer = 0
        Dim Agnt_idno As Integer = 0
        Dim dttm As DateTime
        Dim WedDtTmSTS As Boolean
        Dim BirthDtTmSTS As Boolean



        If grp_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        acgrp_idno = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_AcGroup.Text)
        If acgrp_idno = 0 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1096" Then
                acgrp_idno = 10
            Else
                MessageBox.Show("Invalid Accounts Group", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_AcGroup.Enabled Then cbo_AcGroup.Focus()
                Exit Sub
            End If
        End If

        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)

        'cbo_BillType.Text = "BALANCE ONLY"
        If Trim(cbo_BillType.Text) = "" Then
            MessageBox.Show("Invalid Bill Type", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_BillType.Enabled And cbo_BillType.Visible Then cbo_BillType.Focus()
            Exit Sub
        End If

        ar_idno = Common_Procedures.Area_NameToIdNo(con, cbo_Area.Text)
        PrcLst_idno = Common_Procedures.Price_List_NameToIdNo(con, cbo_PriceListName.Text)

        LedName = Trim(txt_Name.Text)
        If Val(ar_idno) <> 0 Then
            LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
        End If

        State_idno = Common_Procedures.State_NameToIdNo(con, Cbo_State.Text)

        SurName = Common_Procedures.Remove_NonCharacters(LedName)

        Grp_idno = Common_Procedures.Ledger_NameToIdNo(con, cbo_LedgerGroup.Text)
        If Val(Grp_idno) = 0 Then
            Grp_idno = Val(lbl_IdNo.Text)
        End If

        Agnt_idno = Common_Procedures.Ledger_NameToIdNo(con, cbo_Agent.Text)

        SurName = Common_Procedures.Remove_NonCharacters(LedName)

        trans = con.BeginTransaction

        Try

            cmd.Transaction = trans

            cmd.Connection = con

            cmd.Parameters.Clear()

            'cmd.Parameters.AddWithValue("@WeddingDate", Convert.ToDateTime(msk_WeddingDate.Text))

            BirthDtTmSTS = False
            If Trim(msk_BirthDate.Text) <> "" Then
                If IsDate(msk_BirthDate.Text) = True Then
                    dttm = Convert.ToDateTime(msk_BirthDate.Text)
                    cmd.Parameters.AddWithValue("@BirthDate", dttm)
                    BirthDtTmSTS = True
                End If
            End If

            WedDtTmSTS = False
            If Trim(msk_WeddingDate.Text) <> "" Then
                If IsDate(msk_WeddingDate.Text) = True Then
                    dttm = Convert.ToDateTime(msk_WeddingDate.Text)
                    cmd.Parameters.AddWithValue("@WeddingDate", dttm)
                    WedDtTmSTS = True
                End If
            End If

            If New_Entry = True Then
                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", trans)
                If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into ledger_head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4,Ledger_EmailID , Ledger_PhoneNo, Ledger_TinNo, Ledger_CstNo, Ledger_Type, Price_List_IdNo,Rent_Machine,Free_Copies_Machine,Rate_Extra_Copy,Total_Machine , State_Idno ,LedgerGroup_Idno ,Agent_idNo, Pan_No, Birth_Date, Wedding_Date) Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_AlaisName.Text) & "', " & Str(Val(ar_idno)) & ", " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', '" & Trim(cbo_BillType.Text) & "', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', '" & Trim(txt_Address4.Text) & "','" & Trim(txt_EmailID.Text) & "' , '" & Trim(txt_PhoneNo.Text) & "', '" & Trim(txt_TinNo.Text) & "', '" & Trim(txt_CstNo.Text) & "', '', " & Str(Val(PrcLst_idno)) & "," & Str(Val(txt_RentMachine.Text)) & "  ," & Str(Val(txt_FreeCopiesMachine.Text)) & " , " & Str(Val(txt_RateExtraCopy.Text)) & "," & Str(Val(txt_TotalMachine.Text)) & " ,  " & Str(Val(State_idno)) & "  , " & Str(Val(Grp_idno)) & "  , " & Str(Val(Agnt_idno)) & ", '" & Trim(txt_PanNo.Text) & "', " & IIf(BirthDtTmSTS = True, "@BirthDate", "NUll") & ", " & IIf(WedDtTmSTS = True, "@WeddingDate", "NUll") & ")"

            Else
                cmd.CommandText = "Update ledger_head set Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Ledger_MainName = '" & Trim(txt_Name.Text) & "', Ledger_AlaisName = '" & Trim(txt_AlaisName.Text) & "', State_Idno = " & Str(Val(State_idno)) & " , LedgerGroup_Idno = " & Str(Val(Grp_idno)) & " , Area_IdNo = " & Str(Val(ar_idno)) & ", AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & ", Parent_Code = '" & Trim(Parnt_CD) & "', Bill_Type = '" & Trim(cbo_BillType.Text) & "', Ledger_Address1 = '" & Trim(txt_Address1.Text) & "', Ledger_Address2 = '" & Trim(txt_Address2.Text) & "', Ledger_Address3 = '" & Trim(txt_Address3.Text) & "', Ledger_Address4 = '" & Trim(txt_Address4.Text) & "',Ledger_EmailID = '" & Trim(txt_EmailID.Text) & "' , Ledger_PhoneNo = '" & Trim(txt_PhoneNo.Text) & "', Ledger_TinNo = '" & Trim(txt_TinNo.Text) & "', Ledger_CstNo = '" & Trim(txt_CstNo.Text) & "',Price_List_IdNo = " & Str(Val(PrcLst_idno)) & ",Rent_Machine = " & Str(Val(txt_RentMachine.Text)) & "  , Free_Copies_Machine = " & Str(Val(txt_FreeCopiesMachine.Text)) & " , Rate_Extra_Copy = " & Str(Val(txt_RateExtraCopy.Text)) & " , Total_Machine = " & Str(Val(txt_TotalMachine.Text)) & " , Agent_idNo = " & Str(Val(Agnt_idno)) & ", Pan_No = '" & Trim(txt_PanNo.Text) & "', Birth_Date = " & IIf(BirthDtTmSTS = True, "@BirthDate", "NUll") & ", Wedding_Date = " & IIf(WedDtTmSTS = True, "@WeddingDate", "NUll") & " Where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))

            End If

            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            LedArName = Trim(txt_Name.Text)
            If Val(ar_idno) <> 0 Then
                LedArName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
            End If

            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type ,Agent_idNo ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", '' ," & Str(Val(Agnt_idno)) & ")"
            cmd.ExecuteNonQuery()

            If Trim(txt_AlaisName.Text) <> "" Then
                LedArName = Trim(txt_AlaisName.Text)
                If Val(ar_idno) <> 0 Then
                    LedArName = Trim(txt_AlaisName.Text) & " (" & Trim(cbo_Area.Text) & ")"
                End If

                cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type ,Agent_IdNo ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 2, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", ''," & Str(Val(Agnt_idno)) & ")"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Ledger_PhoneNo_Head where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            PhAr = Split(txt_PhoneNo.Text, ",")
            Sno = 0
            For i = 0 To UBound(PhAr)
                If Trim(PhAr(i)) <> "" Then

                    LedPhNo = Trim(PhAr(i))
                    LedPhNo = Replace(LedPhNo, " ", "")
                    LedPhNo = Replace(LedPhNo, "-", "")
                    LedPhNo = Replace(LedPhNo, "_", "")
                    LedPhNo = Replace(LedPhNo, "+", "")
                    LedPhNo = Replace(LedPhNo, "/", "")
                    LedPhNo = Replace(LedPhNo, "\", "")
                    LedPhNo = Replace(LedPhNo, "*", "")

                    If Trim(LedPhNo) <> "" Then
                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Ledger_PhoneNo_Head(Ledger_IdNo, Sl_No, Ledger_PhoneNo) Values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(LedPhNo) & "')"
                        cmd.ExecuteNonQuery()
                    End If

                End If
            Next


            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            Common_Procedures.Master_Return.Return_Value = Trim(LedName)
            Common_Procedures.Master_Return.Master_Type = "LEDGER"

            If New_Entry = True Then new_record()

            MessageBox.Show("Sucessfully Saved", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_ledger_head") > 0 Then
                MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_alaishead") > 0 Then
                MessageBox.Show("Duplicate Ledger Alais Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_phoneno_head") > 0 Then
                MessageBox.Show("Duplicate PhoneNo to this ledger", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Exit Sub

        End Try

    End Sub

    Private Sub Ledger_Creation_SMS_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PriceListName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PRICELIST" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_PriceListName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

    End Sub

    Private Sub Ledger_Creation_SMS_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        con.Open()

        lbl_Price_Agent.Visible = False
        cbo_PriceListName.Visible = False
        cbo_Agent.Visible = False

        lbl_TinNo.Visible = False
        txt_TinNo.Visible = False
        lbl_CstNo.Visible = False
        txt_CstNo.Visible = False
        '********************************************************

        da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head Order by AccountsGroup_Name", con)
        da.Fill(dt1)
        cbo_AcGroup.Items.Clear()
        cbo_AcGroup.DataSource = dt1
        cbo_AcGroup.DisplayMember = "AccountsGroup_Name"

        cbo_BillType.Items.Clear()
        cbo_BillType.Items.Add("BALANCE ONLY")
        cbo_BillType.Items.Add("BILL TO BILL")

        da = New SqlClient.SqlDataAdapter("select Area_Name from Area_Head Order by Area_Name", con)
        da.Fill(dt2)
        cbo_Area.Items.Clear()
        cbo_Area.DataSource = dt2
        cbo_Area.DisplayMember = "Area_Name"

        da = New SqlClient.SqlDataAdapter("select Price_List_Name from Price_List_Head Order by Price_List_Name", con)
        da.Fill(dt3)
        cbo_PriceListName.Items.Clear()
        cbo_PriceListName.DataSource = dt3
        cbo_PriceListName.DisplayMember = "Price_List_Name"

        da = New SqlClient.SqlDataAdapter("select State_Name from State_Head Order by State_Name", con)
        da.Fill(dt4)
        Cbo_State.Items.Clear()
        Cbo_State.DataSource = dt4
        Cbo_State.DisplayMember = "State_Name"

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 or b.AccountsGroup_IdNo = 14 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt5)
        cbo_LedgerGroup.Items.Clear()
        cbo_LedgerGroup.DataSource = dt5
        cbo_LedgerGroup.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 100
        grp_Open.Top = (Me.Height - grp_Open.Height) - 100

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 100
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 100

        pnl_Reading.Visible = False
        pnl_Reading.Left = (Me.Width - grp_Open.Width) / 2
        pnl_Reading.Top = (Me.Height - grp_Open.Height) / 2
        pnl_Reading.BringToFront()

        AddHandler cbo_Machine.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RateExtraCopy.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RentMachine.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalMachine.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FreeCopiesMachine.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PanNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_BirthDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_WeddingDate.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Machine.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FreeCopiesMachine.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RentMachine.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RateExtraCopy.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalMachine.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PanNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_BirthDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_WeddingDate.LostFocus, AddressOf ControlLostFocus

        new_record()

    End Sub

    Private Sub Ledger_Creation_SMS_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Ledger_Creation_SMS_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            ElseIf grp_Open.Visible Then
                btn_CloseFilter_Click(sender, e)
            ElseIf pnl_Reading.Visible Then
                btn_Close_Reading_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        grp_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        'Dim cmd As New SqlClient.SqlCommand
        'Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer


        movid = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Open.Text)

        'cmd.CommandText = "select ledger_idno from ledger_head where ledger_name = '" & Trim(cbo_Open.Text) & "'"
        'cmd.Connection = con

        'movid = 0

        'dr = cmd.ExecuteReader()
        'If dr.HasRows Then
        '    If dr.Read Then
        '        If IsDBNull(dr(0).ToString) = False Then
        '            movid = Val((dr(0).ToString))
        '        End If
        '    End If
        'End If
        'dr.Close()
        'cmd.Dispose()

        If movid <> 0 Then move_record(movid)

        grp_Back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Try
            With cbo_Open
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Open

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

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

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Ledger_DisplayName like '" & Trim(FindStr) & "%' or Ledger_DisplayName like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead " & Condt & " order by Ledger_DisplayName", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Ledger_DisplayName"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        da.Dispose()


        'If Asc(e.KeyChar) = 13 Then
        '    Call btn_Find_Click(sender, e)
        'End If

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Filter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter.Click
        Dim idno As Integer

        idno = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        If Val(idno) <> 0 Then
            move_record(idno)
            grp_Back.Enabled = True
            grp_Filter.Visible = False
        End If


    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        'Dim K As Integer

        'If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
        '    K = Asc(e.KeyChar)
        '    K = K - 32
        '    e.KeyChar = Chr(K)
        'End If

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_AcGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AcGroup.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Try
            With cbo_AcGroup
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_Area.Visible And cbo_Area.Enabled Then
                        cbo_Area.Focus()
                    End If

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_LedgerGroup.Visible And cbo_LedgerGroup.Enabled Then
                        cbo_LedgerGroup.Focus()
                    ElseIf cbo_BillType.Enabled And cbo_BillType.Visible Then
                        cbo_BillType.Focus()
                    End If

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then
                    If .DroppedDown = False Then
                        .DroppedDown = True
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_AcGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AcGroup.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_AcGroup

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

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

                        cbo_LedgerGroup.Focus()

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where (AccountsGroup_Name like '" & FindStr & "%' or AccountsGroup_Name like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head " & Condt & " order by AccountsGroup_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "AccountsGroup_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        da.Dispose()

    End Sub

    Private Sub cbo_BillType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillType, cbo_AcGroup, txt_Address1, "", "", "", "")

    End Sub

    Private Sub cbo_BillType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillType.KeyPress
        ' If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillType, txt_Address1, "", "", "", "")
    End Sub

    Private Sub txt_Address1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address1.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Address1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address1.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Address2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address2.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Address2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address2.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Address3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address3.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Address3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address3.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Address4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address4.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Address4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address4.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_PhoneNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PhoneNo.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_PhoneNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PhoneNo.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_TinNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TinNo.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_TinNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TinNo.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_AlaisName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AlaisName.KeyPress
        'Dim K As Integer

        'If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
        '    K = Asc(e.KeyChar)
        '    K = K - 32
        '    e.KeyChar = Chr(K)
        'End If

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_AlaisName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AlaisName.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Try
            With cbo_Area
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_AlaisName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_AcGroup.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then
                    If .DroppedDown = False Then
                        .DroppedDown = True
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Area.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Area

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

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

                        cbo_AcGroup.Focus()

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where (area_name like '" & FindStr & "%' or area_name like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select area_name from area_head " & Condt & " order by area_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "area_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        da.Dispose()

    End Sub

    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Area.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_pricelistName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PriceListName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Try

            With cbo_PriceListName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_CstNo.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    btn_save.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then
                    If .DroppedDown = False Then
                        .DroppedDown = True
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_PriceListName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PriceListName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_PriceListName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

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
                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
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

                        FindStr = LTrim(FindStr)

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = " Where (Price_List_name like '" & FindStr & "%' or Price_List_name like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Price_List_name from Price_List_head " & Condt & " order by Price_List_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Price_List_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        da.Dispose()

    End Sub
    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus


        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT') ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_CstNo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT') ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Agent.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT') ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    Dim f As New Agent_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
    End Sub

    Private Sub cbo_PriceListName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PriceListName.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

        '    Dim f As New Price_List_Entry

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_PriceListName.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If

    End Sub


    Private Sub txt_CstNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CstNo.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If cbo_PriceListName.Visible = True Then
            If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        ElseIf cbo_Agent.Visible = True Then
            If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        Else
            If e.KeyValue = 40 Then
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                End If
            End If
        End If
    End Sub

    Private Sub txt_CstNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CstNo.KeyPress

        If cbo_PriceListName.Visible = True Then
            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
        ElseIf cbo_Agent.Visible = True Then
            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
        Else
            If Asc(e.KeyChar) = 13 Then
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                End If
            End If
        End If

    End Sub

    Private Sub txt_Address4_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address4.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "T" Then
            txt_Address4.Text = "Tamil Nadu"
            txt_Address3.SelectionStart = txt_Address3.Text.Length
        End If
    End Sub

    Private Sub txt_Address3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address3.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "T" Then
            txt_Address3.Text = "Tamil Nadu"
            txt_Address3.SelectionStart = txt_Address3.Text.Length
        End If
    End Sub

    Private Sub txt_EmailID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EmailID.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_EmailID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmailID.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub


    Private Sub cbo_Machine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Machine.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Machine, Nothing, Nothing, "Machine_Head", "Machine_Name", "", "(Machine_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Machine.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_TotalMachine.Focus()
                    ' btn_Close_Reading_Click(sender, e)

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Machine.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    btn_Close_Reading_Click(sender, e)

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Machine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Machine.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Machine, Nothing, "Machine_Head", "Machine_Name", "", "(Machine_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Machine.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '    save_record()
                    'Else
                    txt_Name.Focus()
                    'End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If

    End Sub

    Private Sub cbo_Machine_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Machine.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Machine_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Machine.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub dgv_details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded

        With dgv_Details
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub cbo_Machine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Machine.TextChanged
        Try
            If Val(cbo_Machine.Tag) = Val(dgv_Details.CurrentCell.ColumnIndex) Then
                dgv_Details.Rows(Me.dgv_Details.CurrentCell.RowIndex).Cells.Item(dgv_Details.CurrentCell.ColumnIndex).Value = Trim(cbo_Machine.Text)
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub
    'Private Sub dgv_countdetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PriceListdetails.CellEndEdit
    '    dgv_details_CellLeave(sender, e)
    'End Sub

    Private Sub dgv_details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        With dgv_Details

            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Machine.Visible = False Or Val(cbo_Machine.Tag) <> e.RowIndex Then

                    cbo_Machine.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Machine_Name from Machine_Head order by Machine_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Machine.DataSource = Dt1
                    cbo_Machine.DisplayMember = "Machine_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Machine.Left = .Left + rect.Left
                    cbo_Machine.Top = .Top + rect.Top

                    cbo_Machine.Width = rect.Width
                    cbo_Machine.Height = rect.Height
                    cbo_Machine.Text = .CurrentCell.Value

                    cbo_Machine.Tag = Val(e.RowIndex)
                    cbo_Machine.Visible = True

                    cbo_Machine.BringToFront()
                    cbo_Machine.Focus()

                End If

            Else
                cbo_Machine.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        'With dgv_Details
        '    If .CurrentCell.ColumnIndex = 2 Then
        '        .CurrentRow.Cells(2).Value = Format(Val(.CurrentRow.Cells(2).Value), "#########0.000")
        '    End If
        'End With
    End Sub



    Private Sub dgv_details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
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
                            btn_Close_Reading.Focus()
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
                            btn_Close_Reading.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            txt_Name.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

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

    Private Sub txt_RentMachine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RentMachine.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_FreeCopiesMachine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FreeCopiesMachine.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_RateExtraCopy_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RateExtraCopy.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_RentMachine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RentMachine.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_FreeCopiesMachine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FreeCopiesMachine.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_RateExtraCopy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RateExtraCopy.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_MachineDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_WeddingDay_Sms.Click
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""

        Try

            PhNo = "WEDDINGDAY CUSTOMERS"

            'smstxt = Trim(txt_Name.Text)
            'If Trim(txt_Address1.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address1.Text)
            'If Trim(txt_Address2.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address2.Text)
            'If Trim(txt_Address3.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address3.Text)
            'If Trim(txt_Address4.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address4.Text)
            'If Trim(txt_TinNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "TIN NO : " & Trim(txt_TinNo.Text)
            'If Trim(txt_PhoneNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "PHONE NO : " & Trim(txt_PhoneNo.Text)

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, 0, SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = "WISH YOU HAPPY WEDDING DAY !!!"

            Sms_Entry.vSmsSendStatus = "ALL"
            Sms_Entry.vSmsSendFor = "WEDDING"

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Close_Reading_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Reading.Click
        grp_Back.Enabled = True
        pnl_Reading.Visible = False
        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()
    End Sub


    Private Sub txt_TotalMachine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TotalMachine.KeyDown
        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True

        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_TotalMachine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TotalMachine.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub
    Private Sub cbo_LedgerGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LedgerGroup.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LedgerGroup, cbo_AcGroup, cbo_BillType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_LedgerGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LedgerGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LedgerGroup, cbo_BillType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub Cbo_State_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_State.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_State, txt_Address4, txt_EmailID, "State_Head", "State_Name", "", "(State_Idno = 0)")

    End Sub

    Private Sub Cbo_State_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_State.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_State, txt_EmailID, "State_Head", "State_Name", "", "(State_Idno = 0)")

    End Sub

    Private Sub btn_SendSMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Send_Single_SMS.Click
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""

        Try

            PhNo = Trim(txt_PhoneNo.Text)

            'smstxt = Trim(txt_Name.Text)
            'If Trim(txt_Address1.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address1.Text)
            'If Trim(txt_Address2.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address2.Text)
            'If Trim(txt_Address3.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address3.Text)
            'If Trim(txt_Address4.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address4.Text)
            'If Trim(txt_TinNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "TIN NO : " & Trim(txt_TinNo.Text)
            'If Trim(txt_PhoneNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "PHONE NO : " & Trim(txt_PhoneNo.Text)

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, 0, SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.vSmsSendStatus = "SINGLE"
            Sms_Entry.vSmsSendFor = "NORMAL"

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_SendSMSAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Send_All_SMS.Click
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""

        Try

            PhNo = "TO ALL"

            'smstxt = Trim(txt_Name.Text)
            'If Trim(txt_Address1.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address1.Text)
            'If Trim(txt_Address2.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address2.Text)
            'If Trim(txt_Address3.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address3.Text)
            'If Trim(txt_Address4.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address4.Text)
            'If Trim(txt_TinNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "TIN NO : " & Trim(txt_TinNo.Text)
            'If Trim(txt_PhoneNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "PHONE NO : " & Trim(txt_PhoneNo.Text)

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, 0, SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.vSmsSendStatus = "ALL"
            Sms_Entry.vSmsSendFor = "NORMAL"

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub msk_BirthDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BirthDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_WeddingDate.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_PanNo.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_BirthDate.Text
            vmskSelStrt = msk_BirthDate.SelectionStart
        End If

    End Sub

    Private Sub msk_BirthDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_BirthDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_WeddingDate.Focus()
        End If
    End Sub

    Private Sub msk_BirthDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BirthDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_BirthDate.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_BirthDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_BirthDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_BirthDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_BirthDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub dtp_BirthDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_BirthDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_WeddingDate.Focus()
        End If
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_PanNo.Focus()
        End If
    End Sub

    Private Sub dtp_BirthDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_BirthDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_WeddingDate.Focus()
        End If
    End Sub

    Private Sub dtp_BirthDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_BirthDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_BirthDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_BirthDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_BirthDate.TextChanged
        If IsDate(dtp_BirthDate.Text) = True Then
            msk_BirthDate.Text = dtp_BirthDate.Text
            msk_BirthDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_WeddingDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_WeddingDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            btn_save.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_BirthDate.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_WeddingDate.Text
            vmskSelStrt = msk_WeddingDate.SelectionStart
        End If

    End Sub

    Private Sub msk_WeddingDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_WeddingDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub msk_WeddingDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_WeddingDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_WeddingDate.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_WeddingDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_WeddingDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_WeddingDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_WeddingDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub dtp_WeddingDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_WeddingDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_WeddingDate.Focus()
        End If
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_BirthDate.Focus()
        End If
    End Sub

    Private Sub dtp_WeddingDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_WeddingDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub dtp_WeddingDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_WeddingDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_WeddingDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_WeddingDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_WeddingDate.TextChanged
        If IsDate(dtp_WeddingDate.Text) = True Then
            msk_WeddingDate.Text = dtp_WeddingDate.Text
            msk_WeddingDate.SelectionStart = 0
        End If
    End Sub


    Private Sub btn_BirthDay_Sms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BirthDay_Sms.Click
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""

        Try

            PhNo = "BIRTHDAY CUSTOMERS"

            'smstxt = Trim(txt_Name.Text)
            'If Trim(txt_Address1.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address1.Text)
            'If Trim(txt_Address2.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address2.Text)
            'If Trim(txt_Address3.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address3.Text)
            'If Trim(txt_Address4.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address4.Text)
            'If Trim(txt_TinNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "TIN NO : " & Trim(txt_TinNo.Text)
            'If Trim(txt_PhoneNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "PHONE NO : " & Trim(txt_PhoneNo.Text)

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, 0, SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = "WISH YOU HAPPY BIRTHDAY !!!"

            Sms_Entry.vSmsSendStatus = "ALL"
            Sms_Entry.vSmsSendFor = "BIRTHDAY"

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
End Class
