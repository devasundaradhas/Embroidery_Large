Public Class Closing_Stock_value
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private FrmLdSTS As Boolean = False

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True

        Me.Height = 200
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        txt_ClosingValue.Text = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        '  Dim n As Integer
        Dim NewCode As String


        If Val(idno) = 0 Then Exit Sub




        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(idno) & "/" & Trim(Common_Procedures.FnYearCode)




        clear()

        Try
            cmd.Connection = con
            cmd.CommandText = "select * from Closing_Stock_Value_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Closing_Stock_Value_Code = '" & Trim(NewCode) & "'"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read() Then
                    lbl_RefNo.Text = dr("Closing_Stock_Value_Idno").ToString()
                    dtp_Date.Text = dr("Closing_Stock_Value_Date").ToString()
                    msk_date.Text = dtp_Date.Text
                    txt_ClosingValue.Text = dr("Closing_Stock_Value").ToString()
                End If
            End If

            dr.Close()

            cmd.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Closing_Stock_value_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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
    End Sub

    Private Sub Area_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Height = 284 ' 197

        con.Open()

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ClosingValue.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ClosingValue.GotFocus, AddressOf ControlGotFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        FrmLdSTS = True
        new_record()


    End Sub

    Private Sub Closing_Stock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then

            If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub

            Else
                Close_Form()
            End If
        End If



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
    Private Sub Closing_Stock_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        'Dim da As SqlClient.SqlDataAdapter
        ' Dim Dt As DataTable
        Dim NewCode As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Master_Area_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Master_Area_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            'da = New SqlClient.SqlDataAdapter("select count(*) from Ledger_Head where Area_IdNo = " & Str(Val(lbl_RefNo.Text)), con)
            'Dt = New DataTable
            'da.Fill(Dt)
            'If Dt.Rows.Count > 0 Then
            '    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '        If Val(Dt.Rows(0)(0).ToString) > 0 Then
            '            MessageBox.Show("Already used this ItemGroup", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            Exit Sub
            '        End If
            '    End If
            'End If

            cmd.Connection = con
            cmd.CommandText = "delete from Closing_Stock_Value_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Closing_Stock_Value_Code = '" & Trim(NewCode) & "'"

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Closing_Stock_Value_Idno from Closing_Stock_Value_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Closing_Stock_Value_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_OrderBy, Closing_Stock_Value_Idno", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Closing_Stock_Value_Idno from Closing_Stock_Value_Head WHERE  Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Closing_Stock_Value_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_OrderBy desc, Closing_Stock_Value_Idno desc"

            dr = cmd.ExecuteReader

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer
        Dim OrdByNo As Single = 0
        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Closing_Stock_Value_Idno from Closing_Stock_Value_Head where for_orderby > " & Str(Val(OrdByNo)) & " and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Closing_Stock_Value_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_OrderBy, Closing_Stock_Value_Idno"

            dr = cmd.ExecuteReader()

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))
            da = New SqlClient.SqlDataAdapter("select top 1 Closing_Stock_Value_Idno from Closing_Stock_Value_Head Where for_orderby < " & Str(Val(OrdByNo)) & " and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Closing_Stock_Value_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_OrderBy desc, Closing_Stock_Value_Idno desc ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim cmd As New SqlClient.SqlCommand
        clear()

        New_Entry = True
        lbl_RefNo.ForeColor = Color.Red
        lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Closing_Stock_Value_Head", "Closing_Stock_Value_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() : msk_date.Text = dtp_Date.Text

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---------
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NewCode As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Master_Area_Creation, New_Entry) = False Then Exit Sub

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            If New_Entry = True Then
                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Closing_Stock_Value_Head", "Closing_Stock_Value_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, trans)


                cmd.CommandText = "Insert into Closing_Stock_Value_Head(Closing_Stock_Value_Code, Company_IdNo, Closing_Stock_Value_Idno, for_OrderBy, Closing_Stock_Value_Date, Closing_Stock_Value ) values ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", " & Str(Val(lbl_RefNo.Text)) & ", " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(txt_ClosingValue.Text)) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Closing_Stock_Value_Head set Closing_Stock_Value_Date = @EntryDate, Closing_Stock_Value = " & Str(Val(txt_ClosingValue.Text)) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Closing_Stock_Value_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If


        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(ex.Message), "IX_Closing_Stock_Value_Head_1") > 0 Then
                MessageBox.Show("Duplicate Date", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub txt_ClosingValue_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ClosingValue.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            btn_save.Focus()
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ClosingValue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

   
    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_ClosingValue.Focus()
        End If
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub
    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Date.Text = Date.Today
        End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
    End Sub
    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

End Class