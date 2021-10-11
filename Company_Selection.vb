Public Class Company_Selection

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private Sub Company_Selection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim CompCondt As String

        Me.BackColor = Color.White  ' Color.FromArgb(41, 57, 85)
        pnl_Back.BackColor = Me.BackColor ' Color.FromArgb(41, 57, 85)

        con.Open()

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        da = New SqlClient.SqlDataAdapter("select Company_Name from Company_Head " & IIf(Trim(CompCondt) <> "", " Where ", "") & CompCondt & " order by Company_Name", con)
        da.Fill(dt1)
        cbo_Company.DataSource = dt1
        cbo_Company.DisplayMember = "Company_Name"

    End Sub

    Private Sub Company_Selection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Common_Procedures.CompIdNo = 0
            Me.Close()
        End If
    End Sub

    Private Sub Company_Selection_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Common_Procedures.CompIdNo = 0
        Me.Close()
    End Sub

    Private Sub btn_OK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_OK.Click
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim CompID As Integer

        da = New SqlClient.SqlDataAdapter("select Company_IdNo from Company_Head where Company_Name = '" & Trim(cbo_Company.Text) & "'", con)
        da.Fill(dt1)

        CompID = 0
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                CompID = Val(dt1.Rows(0)(0).ToString)
            End If
        End If

        If CompID = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Company.Enabled Then cbo_Company.Focus()
            Exit Sub
        End If

        Common_Procedures.CompIdNo = Val(CompID)

        Me.Close()

    End Sub

    Private Sub cbo_Company_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_Company

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then

                                FindStr = LTrim(.Text)

                                indx = .FindString(FindStr)

                                If indx <> -1 Then
                                    .SelectedText = ""
                                    .SelectedIndex = indx
                                    .SelectionStart = FindStr.Length
                                    .SelectionLength = .Text.Length
                                End If

                            End If

                        End If

                        btn_OK_Click(sender, e)

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

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx
                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Company_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Company.KeyUp
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_Company

                If e.KeyCode <> 27 Then

                    If e.KeyCode = 46 Then

                        Condt = ""
                        FindStr = LTrim(.Text)

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx
                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
End Class