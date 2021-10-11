Public Class Password

    Private Sub Password_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Password_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Common_Procedures.Password_Input = ""
        txt_Password.Text = ""
        txt_Password.Focus()
    End Sub

    Private Sub txt_Password_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Password.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Ok.Focus()
        End If
    End Sub

    Private Sub btn_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Ok.Click
        Common_Procedures.Password_Input = Trim(txt_Password.Text)
        Me.Close()
    End Sub
End Class