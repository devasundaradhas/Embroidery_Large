Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Management

Public Class Login

    Private cn1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

    Public ValidationText As String
    Dim ServerName As String
    Dim Password As String
    Dim TransactionDataBase As String

    Private Sub clear()
        cbo_UserName.Text = ""
        txt_Password.Text = ""
    End Sub

    Private Sub Login_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

    End Sub

    Private Sub Login_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        '------VALIDATE SOFTWARE

        If File.Exists(System.Windows.Forms.Application.StartupPath & "\LOGINPARAMETERS.TXT") = False Then
            Dim C As Integer = MsgBox("REGISTRATION FILE IS ABSENT !. Do You Want to Register This Software ? ", vbYesNo)

            If C = vbYes Then
                Me.Visible = False
                btn_Login.Enabled = False
                RegisterSW.Show()
                Exit Sub
            Else
                Application.Exit()
            End If
        End If



        Try

            Using SR As StreamReader = New StreamReader(System.Windows.Forms.Application.StartupPath & "\LOGINPARAMETERS.TXT")

                Dim LINE As String
                Dim LNNO As Integer = 1

                LINE = SR.ReadLine

                If LINE <> Nothing Then
                    ServerName = Authenticate.RevertAuthenticationCode(LINE)
                End If


                While LINE <> Nothing

                    If LNNO = 2 Then
                        Password = Authenticate.RevertAuthenticationCode(LINE)
                    End If


                    If LNNO = 3 Then
                        TransactionDataBase = Authenticate.RevertAuthenticationCode(LINE)
                    End If


                    If LNNO = 4 Then

                        ValidationText = Authenticate.RevertAuthenticationCode(LINE)

                        Dim PARAMS() As String = Split(ValidationText, "$$$")

                        If PARAMS.GetUpperBound(0) < 1 Then

                            Dim C As Integer = MsgBox("INVALID REGISTRATION KEY !. Do You Want to Register This Software ? ", vbYesNo)

                            If C = vbYes Then
                                Me.Visible = False
                                btn_Login.Enabled = False
                                RegisterSW.Show()
                            Else
                                Application.Exit()
                            End If

                        End If

                        If Not IsDate(PARAMS(1)) Then

                            If Not IsDate(PARAMS(1)) Then

                                Dim C As Integer = MsgBox("INVALID/CORRUPTED REGISTRATION KEY(Date). Do You Want to Register This Software ?", vbYesNo)

                                If C = vbYes Then
                                    Me.Visible = False
                                    btn_Login.Enabled = False
                                    RegisterSW.Show()
                                Else
                                    Application.Exit()
                                End If

                            End If

                        End If

                        If DateAdd(DateInterval.Day, 1, Now) > CDate(PARAMS(1)) Then

                            Dim C As Integer = MsgBox("TRIAL PERIOD HAS EXPIRED. DO YOU WANT TO REGISTER PURCHAESD LICENSE. ", vbYesNo)

                            If C = vbYes Then
                                Me.Visible = False
                                btn_Login.Enabled = False
                                RegisterSW.Show()
                            Else
                                Application.Exit()
                            End If

                        End If


                        Dim serial As String = ""
                        Dim fso As Object = CreateObject("Scripting.FileSystemObject")
                        Dim Drv As Object = fso.GetDrive(fso.GetDriveName(Application.StartupPath))

                        With Drv

                            If .IsReady Then
                                serial = .SerialNumber.ToString
                            Else    '"Drive Not Ready!"
                                serial = ""
                            End If

                        End With



A:


                        If serial <> PARAMS(0) Then

                            Dim C As Integer = MsgBox("Invalid Registration Key ! Do you want to Register the Software ", vbYesNo)

                            If C = vbYes Then
                                Me.Visible = False
                                btn_Login.Enabled = False
                                RegisterSW.Show()
                            Else
                                Application.Exit()
                            End If

                        End If

                        Common_Procedures.settings.Validation_End_Date = CDate(PARAMS(1))

                        If IsDate(ValidationText) Then
                            Common_Procedures.settings.Validation_End_Date = CDate(ValidationText)
                        End If

                    End If

                    LINE = SR.ReadLine
                    LNNO = LNNO + 1

                End While

                If LNNO < 5 Then
                    Dim C As Integer = MsgBox("Invalid Registration Key ! Do you want to Register the Software ", vbYesNo)

                    If C = vbYes Then
                        Me.Visible = False
                        btn_Login.Enabled = False
                        RegisterSW.Show()
                    Else
                        Application.Exit()
                    End If
                End If

            End Using


        Catch ex As Exception

            MsgBox(ex.Message)

        End Try

        'If IsDate(ValidationText) Then

        '    If DateDiff(DateInterval.Day, Now, CDate(ValidationText)) < 0 Then
        '        MsgBox("Trial Period Has Expired")
        '        Me.Close()
        '        Exit Sub
        '    End If

        'ElseIf Len(Trim(ValidationText)) = 0 Then

        '    MsgBox("Invalid Software Registration")
        '    Me.Close()
        '    Exit Sub

        'Else

        '    Dim cn As New SqlConnection

        '    cn.ConnectionString = "INITIAL CATALOG=" & TransactionDataBase & ";Data Source=" & ServerName & _
        '                        ";User Id=SA;Password=" & Password

        '    cn.Open()


        '    Dim DAdapt As New SqlDataAdapter
        '    Dim Dset As New DataSet
        '    Dim Cmd As New SqlCommand

        '    Cmd.Connection = cn
        '    Cmd.CommandText = "select count(company_name) from company_head where upper(Company_Name) = '" & UCase(ValidationText) & "'"
        '    DAdapt.SelectCommand = Cmd
        '    DAdapt.Fill(Dset, "LegderCheck")

        '    If Dset.Tables("LedgerCheck").Rows(0).Item(0) = 0 Then
        '        MsgBox("Invalid Software Registration")
        '        Me.Close()
        '        Exit Sub
        '    End If

        'End If

        '-------------------

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        cn1.Open()

        da = New SqlClient.SqlDataAdapter("select user_name from User_Head order by User_Name", cn1)
        da.Fill(dt1)
        cbo_UserName.DataSource = dt1
        cbo_UserName.DisplayMember = "User_Name"

        clear()

    End Sub

    Private Sub Login_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        cn1.Close()
        cn1.Dispose()

    End Sub

    Private Sub Login_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then
                Me.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_UserName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UserName.GotFocus
        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, cn1, "User_Head", "User_Name", "", "(User_IdNo = 0)")

            With cbo_UserName
                .BackColor = Color.Lime
                .ForeColor = Color.Blue
                .SelectAll()
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_UserName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_UserName.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, cn1, cbo_UserName, Nothing, txt_Password, "User_Head", "User_Name", "", "(User_IdNo = 0)")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_UserName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_UserName.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, cn1, cbo_UserName, txt_Password, "User_Head", "User_Name", "", "(User_IdNo = 0)")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Password_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Password.GotFocus
        With txt_Password
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_Password_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Password.KeyDown
        If e.KeyCode = 40 Then btn_Login.Focus()
        If e.KeyCode = 38 Then cbo_UserName.Focus()
    End Sub

    Private Sub txt_Password_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Password.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Check_Login_Password()
            'SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_Login_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Login.Click
        Check_Login_Password()
    End Sub

    Private Sub Check_Login_Password()
        Static Inc As Integer = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim AcPwd As String
        Dim UnAcPwd As String

        Da = New SqlClient.SqlDataAdapter("select * from user_head where user_name = '" & Trim(cbo_UserName.Text) & "'", cn1)
        Da.Fill(Dt1)

        AcPwd = ""
        UnAcPwd = ""
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Account_Password").ToString) = False Then
                AcPwd = Dt1.Rows(0).Item("Account_Password").ToString
            End If
            If IsDBNull(Dt1.Rows(0).Item("UnAccount_Password").ToString) = False Then
                UnAcPwd = Dt1.Rows(0).Item("UnAccount_Password").ToString
            End If

            If Trim(AcPwd) = Trim(txt_Password.Text) Then

                Common_Procedures.User.IdNo = Val(Dt1.Rows(0).Item("User_IdNo").ToString)
                Common_Procedures.User.Name = Trim(Dt1.Rows(0).Item("User_Name").ToString)
                Common_Procedures.User.Type = "ACCOUNT"
                Common_Procedures.User.RealName = Trim(Dt1.Rows(0).Item("User_Real_Name").ToString)

                determine_User_Access_Rights(cn1)
                GET_AWS_S3_ACCESS_KEYS(cn1)
                Me.Hide()

            ElseIf Trim(UnAcPwd) <> "" And Trim(UnAcPwd) = Trim(txt_Password.Text) Then

                Common_Procedures.User.IdNo = Val(Dt1.Rows(0).Item("User_IdNo").ToString)
                Common_Procedures.User.Name = Trim(Dt1.Rows(0).Item("User_Name").ToString)
                Common_Procedures.User.Type = "UNACCOUNT"
                Common_Procedures.User.RealName = Trim(Dt1.Rows(0).Item("User_Real_Name").ToString)

                determine_User_Access_Rights(cn1)
                GET_AWS_S3_ACCESS_KEYS(cn1)
                Me.Hide()

            Else

                Inc = Inc + 1
                MessageBox.Show("Invalid Password", "LOGIN FAILED...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                If Inc > 2 Then
                    Me.Close()
                    Application.Exit()
                End If

            End If

        Else
            MessageBox.Show("Invalid User Name", "LOGIN FAILED...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Application.Exit()

        End If

        Dt1.Dispose()
        Da.Dispose()

    End Sub

    Private Sub txt_Password_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Password.LostFocus
        With txt_Password
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_UserName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_UserName.LostFocus
        With cbo_UserName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Public Sub determine_User_Access_Rights(ByVal Cn1 As SqlClient.SqlConnection)

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        With Common_Procedures.UR1
            If Common_Procedures.User.IdNo = 1 Then
                ReDim .UserInfo(0, 0)
                .UserInfo(0, 0) = "ALL"
                Exit Sub
            Else

                Da = New SqlClient.SqlDataAdapter("select * from User_Access_Rights where user_idno = " & Str(Common_Procedures.User.IdNo), Cn1)
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    ReDim .UserInfo(Dt1.Rows.Count - 1, 2)

                    For i As Integer = 0 To Dt1.Rows.Count - 1

                        If Not IsDBNull(Dt1.Rows(i).Item(1)) And Not IsDBNull(Dt1.Rows(i).Item(2)) Then
                            'MsgBox(Dt1.Rows(i).Item(1))
                            .UserInfo(i, 0) = Dt1.Rows(i).Item(1)
                            .UserInfo(i, 1) = Dt1.Rows(i).Item(2)
                        End If

                    Next

                End If


            End If

        End With

    End Sub

    Public Sub GET_AWS_S3_ACCESS_KEYS(ByVal Cn1 As SqlClient.SqlConnection)

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Da = New SqlClient.SqlDataAdapter("SELECT * FROM AWS_KEY_SETTINGS", Cn1)

        Da.Fill(Dt1)



        If Dt1.Rows.Count > 0 Then

            If Not IsDBNull(Dt1.Rows(0).Item("AWS_ACCESS_KEY")) Then

                Common_Procedures.AWS_ACCESS_KEY = Dt1.Rows(0).Item("AWS_ACCESS_KEY")

            End If

            If Not IsDBNull(Dt1.Rows(0).Item("AWS_SECRET_KEY")) Then

                Common_Procedures.AWS_SECRET_KEY = Dt1.Rows(0).Item("AWS_SECRET_KEY")

            End If

            If Not IsDBNull(Dt1.Rows(0).Item("AWS_BUCKET_FOR_SW")) Then

                Common_Procedures.AWS_SW_BUCKET = Dt1.Rows(0).Item("AWS_BUCKET_FOR_SW")

            End If

            If Not IsDBNull(Dt1.Rows(0).Item("AWS_BUCKET_FOR_DB")) Then

                Common_Procedures.AWS_DB_BUCKET = Dt1.Rows(0).Item("AWS_BUCKET_FOR_DB")

            End If

            If Not IsDBNull(Dt1.Rows(0).Item("AWS_BUCKET_FOR_DOWNLOADER")) Then

                Common_Procedures.AWS_BUCKET_FOR_DOWNLOADER = Dt1.Rows(0).Item("AWS_BUCKET_FOR_DOWNLOADER")

            End If

        End If

        'End With

    End Sub

End Class