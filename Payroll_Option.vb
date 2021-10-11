Public Class Payroll_Option

    Private con As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
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



        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(44, 61, 90)
                Prec_ActCtrl.ForeColor = Color.White
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


    Private Sub Payroll_Option_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()

    End Sub

    Private Sub Payroll_Option_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        con.Open()

        txt_Esi.Text = ""
        txt_Epf.Text = ""


        AddHandler txt_Epf.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Esi.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Epf.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Esi.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Esi.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Esi.KeyPress, AddressOf TextBoxControlKeyPress
        move_record()

    End Sub

    Public Sub move_record()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try
            da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then


                txt_Esi.Text = dt1.Rows(0).Item("Basic_Wages_For_Esi").ToString()
                txt_Epf.Text = dt1.Rows(0).Item("Basic_Pay_For_Epf").ToString()

            End If
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Esi.Visible And txt_Esi.Enabled Then txt_Esi.Focus()
        End Try

    End Sub

    Public Sub save_record()

        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim CC_Leng As Integer = 0

        If Trim(txt_Esi.Text) = "" Then
            MessageBox.Show("Invalid Basic Wages for Esi", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Esi.Visible And txt_Esi.Enabled Then txt_Esi.Focus()
            Exit Sub
        End If

        If Trim(txt_Epf.Text) = "" Then
            MessageBox.Show("Invalid Basic Pay for Epf", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Epf.Visible And txt_Epf.Enabled Then txt_Epf.Focus()
            Exit Sub
        End If

     

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "update Settings_Head set Basic_Wages_For_Esi = " & Trim(txt_Esi.Text) & " ,Basic_Pay_For_Epf =" & Trim(txt_Epf.Text) & ""
            cmd.ExecuteNonQuery()

            trans.Commit()

            MessageBox.Show("Upadated SuccessFully", "FOR UPDATE", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES NOT Update", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Me.Close()
            ' Common_Procedures.vShowEntrance_Status_ForCC = True
            '  MDIParent1.Close()
            ' Entrance.Show()
        End Try

    End Sub

    Private Sub btn_UPDATE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_UPDATE.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        On Error Resume Next
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub txt_Epf_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Epf.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            btn_UPDATE.Focus()
        End If
    End Sub

    Private Sub txt_Esi_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Esi.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
End Class