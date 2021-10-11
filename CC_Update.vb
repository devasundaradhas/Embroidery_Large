Public Class CC_Update
    Private con As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

    Private Sub CC_Update_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()

    End Sub

    Private Sub CC_Update_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        con.Open()

        lbl_CurrentCC.Text = ""
        txt_NewCC.Text = ""

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

                If IsDBNull(dt1.Rows(0).Item("Cc_No").ToString) = False Then
                    lbl_CurrentCC.Text = dt1.Rows(0).Item("Cc_No").ToString()
                End If

            End If
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

        End Try

    End Sub

    Public Sub save_record()

        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim CC_Leng As Integer = 0

        If Trim(txt_NewCC.Text) = "" Then
            MessageBox.Show("Invalid CC_No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        CC_Leng = Len(Trim(txt_NewCC.Text))

        If Val(CC_Leng) < 4 Then
            MessageBox.Show("Invalid CC No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "update Settings_Head set Cc_No = '" & Trim(txt_NewCC.Text) & "' where Cc_No ='" & Trim(lbl_CurrentCC.Text) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

        Catch ex As Exception
            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES NOT Update", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Me.Close()
            Me.Dispose()

            Common_Procedures.vShowEntrance_Status_ForCC = True
            MDIParent1.Close()
            MDIParent1.Dispose()
            Entrance.Show()

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

    Private Sub txt_NewCC_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NewCC.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            btn_UPDATE.Focus()
        End If
    End Sub
End Class