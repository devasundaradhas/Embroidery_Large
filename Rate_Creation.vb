Public Class Rate_Creation

    Implements Interface_MDIActions


    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private dgv_ActiveCtrl_Name As String

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl


    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_DetAr(100, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private Print_PDF_Status As Boolean = False

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False


        txt_RatePerHr.Text = ""
        txt_RatePerDay.Text = ""
        txt_RatePerMonth.Text = ""



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

        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        Grid_DeSelect()

        'End If

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
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next


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


        'dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Job_Card_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

    Private Sub Job_Card_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub



    Private Sub Job_Card_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub



                Else
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    Else
                        Me.Close()
                    End If

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Job_Card_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()





        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler txt_RatePerHr.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RatePerDay.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RatePerMonth.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_RatePerHr.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RatePerDay.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RatePerMonth.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_RatePerHr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RatePerDay.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RatePerMonth.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_RatePerHr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RatePerDay.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RatePerMonth.KeyPress, AddressOf TextBoxControlKeyPress





        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

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


    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        '    If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Rate_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)), con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_RatePerHr.Text = dt1.Rows(0).Item("Rate_Per_Hour").ToString
                txt_RatePerDay.Text = dt1.Rows(0).Item("Rate_Per_Day").ToString
                txt_RatePerMonth.Text = dt1.Rows(0).Item("Rate_Per_Month").ToString


            End If





        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If txt_RatePerHr.Visible And txt_RatePerHr.Enabled Then txt_RatePerHr.Focus()

        End Try

        NoCalc_Status = False

    End Sub

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


            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from rate_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " "
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

            If txt_RatePerHr.Enabled = True And txt_RatePerHr.Visible = True Then txt_RatePerHr.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record


    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            'da = New SqlClient.SqlDataAdapter("select top 1 Job_Card_No from rate_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Job_Card_No", con)
            'da.Fill(dt)

            'movno = ""
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        movno = Trim(dt.Rows(0)(0).ToString)
            '    End If
            'End If

            'dt.Clear()

            ' If Val(movno) <> 0 Then move_record(movno)

            move_record(movno)

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

            'OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            'da = New SqlClient.SqlDataAdapter("select top 1 Job_Card_No from Job_Card_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Job_Card_No", con)
            'da.Fill(dt)

            'movno = ""
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        movno = dt.Rows(0)(0).ToString
            '    End If
            'End If

            'If Val(movno) <> 0 Then move_record(movno)

            move_record(movno)

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

            'OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            'da = New SqlClient.SqlDataAdapter("select top 1 Job_Card_No from Job_Card_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Job_Card_No desc", con)
            'da.Fill(dt)

            'movno = ""
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        movno = dt.Rows(0)(0).ToString
            '    End If
            'End If

            'If Val(movno) <> 0 Then move_record(movno)

            move_record(movno)

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
            'da = New SqlClient.SqlDataAdapter("select top 1 Job_Card_No from Job_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Job_Card_No desc", con)
            'da.Fill(dt)

            'movno = ""
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        movno = dt.Rows(0)(0).ToString
            '    End If
            'End If

            'If Val(movno) <> 0 Then move_record(movno)

            move_record(movno)

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

        Try
            clear()

            New_Entry = True





        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally



            If txt_RatePerHr.Enabled And txt_RatePerHr.Visible Then txt_RatePerHr.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record


    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim DelAt_ID As Integer = 0
        Dim Itm_Id As Integer = 0
        Dim Led_Id As Integer = 0
        Dim Prc_ID As Integer = 0
        Dim GdGsm_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotQty As Single

        Dim GItm_Id As Integer = 0

        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim PurCd As String = ""
        Dim PurSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim PcsChkCode As String = ""





        NoCalc_Status = False

        tr = con.BeginTransaction

        Try




            cmd.Connection = con
            cmd.Transaction = tr


            cmd.CommandText = "Delete from Rate_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Rate_Head (   Rate_Per_Hour       ,Rate_Per_Day                         ,Rate_Per_Month                   ,Company_Idno) " & _
                                "     Values     ( " & Val(txt_RatePerHr.Text) & "," & Val(txt_RatePerDay.Text) & "," & Val(txt_RatePerMonth.Text) & ", " & Str(Val(lbl_Company.Tag)) & ") "
            cmd.ExecuteNonQuery()



            tr.Commit()

            move_record(0)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If txt_RatePerHr.Enabled And txt_RatePerHr.Visible Then txt_RatePerHr.Focus()

        End Try

    End Sub









    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click

    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

    End Sub







    Private Sub txt_RatePerMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RatePerMonth.KeyPress
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                txt_RatePerHr.Focus()
            End If
        End If
    End Sub







    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Private Sub txt_RatePerDay_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_RatePerDay.TextChanged

    End Sub
End Class