Public Class frm_Generate_Script

    Private Sub btn_GenerateScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_GenerateScript.Click
        Dim ar() As String
        Dim s As String
        Dim I As Integer
        Dim cmd As New SqlClient.SqlCommand


        ar = Split(RichTextBox1.Text, "GO")

        s = ""
        RichTextBox2.Text = ""

        For I = 0 To UBound(ar)
            If Trim(ar(I)) <> "" Then

                If Trim(UCase(ar(I))).Substring(0, 3) = "SET" Or Trim(UCase(ar(I))).Substring(1, 3) = "SET" Or Trim(UCase(ar(I))).Substring(0, 6) = "CREATE" Or Trim(UCase(ar(I))).Substring(1, 6) = "CREATE" Or Trim(UCase(ar(I))).Substring(0, 5) = "ALTER" Or Trim(UCase(ar(I))).Substring(1, 6) = "ALTER" Then
                    If Trim(s) <> "" Then
                        RichTextBox2.Text = RichTextBox2.Text & Chr(13) & "cmd.CommandText = """ & Trim(s) & """"
                        RichTextBox2.Text = RichTextBox2.Text & Chr(13) & "cmd.EndExecuteNonQuery()"
                        RichTextBox2.Text = RichTextBox2.Text & Chr(13) & ""
                    End If

                    s = ""

                End If

                s = s & Trim(ar(I))

            Else

                If Trim(s) <> "" Then
                    RichTextBox2.Text = RichTextBox2.Text & Chr(13) & Trim(s)
                End If

                s = ""

            End If
        Next

        If Trim(s) <> "" Then
            RichTextBox2.Text = RichTextBox2.Text & Chr(13) & Trim(s)
        End If

    End Sub

End Class