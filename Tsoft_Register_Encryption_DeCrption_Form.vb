Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail
Imports System.IO

Public Class Tsoft_Register_Encryption_DeCrption_Form
    Private FrmLdSTS As Boolean = False
    Private Prec_ActCtrl As New Control

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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

        If FrmLdSTS = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub

    Private Sub Tsoft_Register_Encryption_DeCrption_Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim pathnameRoot As String = ""

        Common_Procedures.SoftWareRegister.passPhrase = "Solutions-IXC0307249"
        Common_Procedures.SoftWareRegister.saltValue = "GOLD-15101979"

        Common_Procedures.Entrance_SQL_PassWord.passPhrase = "T.ThanGesWaran"
        Common_Procedures.Entrance_SQL_PassWord.saltValue = "N.VaRaLakshmi"

        Common_Procedures.UserCreation_AcPassWord.passPhrase = "Tsoft_Ac_User_Name"
        Common_Procedures.UserCreation_AcPassWord.saltValue = "Tsoft_Ac_PassWord"

        Common_Procedures.UserCreation_UnAcPassWord.passPhrase = "Tsoft_UnAc_User_Name"
        Common_Procedures.UserCreation_UnAcPassWord.saltValue = "Tsoft_UnAc_PassWord"

        AddHandler txt_SystemSerialNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LicenseCode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MobileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CustomerName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Message.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_SystemSerialNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LicenseCode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MobileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CustomerName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Message.LostFocus, AddressOf ControlLostFocus

        Common_Procedures.DriveVolumeSerialName = ""
        Try
            Common_Procedures.DriveVolumeSerialName = Common_Procedures.GetDriveSerialNumber("D:")
        Catch ex As Exception
            '---
        End Try

        pnl_SMS.Visible = False
        pnl_SMS.Left = (Me.Width - pnl_SMS.Width) \ 2
        pnl_SMS.Top = (Me.Height - pnl_SMS.Height) \ 2
        pnl_SMS.BringToFront()

        txt_SystemSerialNo.Text = ""
        txt_LicenseCode.Text = ""
        txt_MobileNo.Text = ""
        txt_CustomerName.Text = ""
        txt_Message.Text = ""


        txt_SystemSerialNo.Enabled = False
        btn_Generate_LicenseCode.Visible = False
        If Common_Procedures.is_OfficeSystem = True Then
            txt_SystemSerialNo.Enabled = True
            btn_Generate_LicenseCode.Visible = True
        End If

        If InStr(1, Trim(LCase(Application.StartupPath)), "\bin\debug") > 0 Then
            Common_Procedures.AppPath = Replace(Trim(LCase(Application.StartupPath)), "\bin\debug", "")
        Else
            Common_Procedures.AppPath = Application.StartupPath
        End If

        pathnameRoot = Path.GetPathRoot(Common_Procedures.AppPath)

        txt_SystemSerialNo.Text = Common_Procedures.GetDriveSerialNumber(Microsoft.VisualBasic.Left(pathnameRoot, 2))

        For Each tabpg As TabPage In TabControl1.TabPages
            If tabpg.Name = "TabPage2" Or tabpg.Name = "TabPage3" Then
                TabControl1.TabPages.Remove(tabpg)
            End If
        Next

        'TabControl1.TabPages(1).Visible = False
        'TabControl1.TabPages(2).Visible = False
        'TabControl1.TabPages(1).Hide()
        'TabControl1.TabPages(2).Hide()

    End Sub

    Private Sub Tsoft_Register_Encryption_DeCrption_Form_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try
            If txt_SystemSerialNo.Enabled And txt_SystemSerialNo.Visible Then
                txt_SystemSerialNo.Focus()
            Else
                txt_LicenseCode.Focus()
            End If
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub Tsoft_Register_Encryption_DeCrption_Form_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then
                If pnl_SMS.Visible = True Then
                    btn_CloseSMS_Click(sender, e)

                Else

                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING SOFTWARE...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        Me.Close()
                        Me.Dispose()
                        Application.Exit()
                    End If

                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
        Application.Exit()
    End Sub

    Private Sub btn_Show_LicenseCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Generate_LicenseCode.Click
        Dim LicCode As String = ""
        LicCode = get_LicenseCode()
        txt_LicenseCode.Text = Trim(UCase(LicCode))
    End Sub

    Private Sub btn_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Register.Click
        Dim FontsPath As String = ""
        Dim RegFile As String = ""
        Dim LicCode As String = ""
        Dim pth As String = ""
        Dim fs As FileStream
        Dim r As StreamReader
        Dim w As StreamWriter

        Try


            LicCode = get_LicenseCode()

            If Trim(LicCode) = "" Then Exit Sub

            If Trim(UCase(txt_LicenseCode.Text)) = Trim(UCase(LicCode)) Then
                FontsPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts)

                RegFile = Trim(FontsPath) & "\amutha.ttf"

                File.WriteAllBytes(RegFile, My.Resources.amutha)

                pth = Trim(Common_Procedures.AppPath) & "\license.ini"
                If File.Exists(pth) = True Then
                    File.Delete(pth)
                End If
                If File.Exists(pth) = False Then
                    fs = New FileStream(pth, FileMode.Create)
                    w = New StreamWriter(fs)
                    w.WriteLine(Trim(UCase(LicCode)))
                    w.Close()
                    fs.Close()
                    w.Dispose()
                    fs.Dispose()
                End If

                MessageBox.Show("Sucessfully Registered!!!", "FOR SOFTWARE REGISTERATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Else

                MessageBox.Show("Invalid License code", "DOES NOT REGISTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_LicenseCode.Enabled Then txt_LicenseCode.Focus()
                Exit Sub

            End If

        Catch ex As Exception

            If InStr(1, Trim(LCase(Err.Description)), "amutha") > 0 Then
                MessageBox.Show("Invalid Access to windows path : Run as Administrator", "DOES NOT REGISTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(Err.Description, "DOES NOT REGISTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If
            If txt_LicenseCode.Enabled Then txt_LicenseCode.Focus()
            Exit Sub

        End Try

    End Sub

    Private Sub txt_LicenseCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_LicenseCode.GotFocus
        txt_LicenseCode.BackColor = Color.Lime
        txt_LicenseCode.ForeColor = Color.Blue
    End Sub

    Private Sub txt_LicenseCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_LicenseCode.LostFocus
        txt_LicenseCode.BackColor = Color.White
        txt_LicenseCode.ForeColor = Color.Black
    End Sub

    Private Sub txt_SystemSerialNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SystemSerialNo.GotFocus
        txt_SystemSerialNo.BackColor = Color.Lime
        txt_SystemSerialNo.ForeColor = Color.Blue
    End Sub

    Private Sub txt_SystemSerialNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SystemSerialNo.LostFocus
        txt_SystemSerialNo.BackColor = Color.White
        txt_SystemSerialNo.ForeColor = Color.Black
    End Sub

    Private Function get_LicenseCode() As String
        Dim LicCode As String = ""

        LicCode = ""
        If Trim(txt_SystemSerialNo.Text) = "" Then
            MessageBox.Show("Invalid System No", "DOES NOT REGISTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_SystemSerialNo.Enabled Then txt_SystemSerialNo.Focus() Else txt_LicenseCode.Focus()
            get_LicenseCode = Trim(UCase(LicCode))
            Exit Function
        End If

        LicCode = Common_Procedures.Encrypt(Trim(UCase(txt_SystemSerialNo.Text)), Trim(Common_Procedures.SoftWareRegister.passPhrase), Trim(Common_Procedures.SoftWareRegister.passPhrase))
        LicCode = Trim(UCase(LicCode))
        If Microsoft.VisualBasic.Right(Trim(LicCode), 2) = "==" Then
            LicCode = Microsoft.VisualBasic.Left(Trim(LicCode), (Microsoft.VisualBasic.Len(Trim(LicCode)) - 2))
        End If

        get_LicenseCode = Trim(UCase(LicCode))

    End Function

    Private Sub btn_UnRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UnRegister.Click
        Dim FontsPath As String = ""
        Dim RegFile As String = ""

        FontsPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts)

        RegFile = Trim(FontsPath) & "\amutha.ttf"

        If File.Exists(RegFile) = True Then
            File.Delete(RegFile)

            MessageBox.Show("Software Un-Registered Sucessfully!!!", "FOR SOFTWARE UN-REGISTERATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            MessageBox.Show("Already Software Not Registered!!!", "FOR SOFTWARE UN-REGISTERATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

        End If

    End Sub

    Private Sub btn_SendSms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SendSms.Click
        Dim smstxt As String = ""

        txt_MobileNo.Text = "8508403229"
        txt_CustomerName.Text = ""

        smstxt = ""
        If Trim(txt_SystemSerialNo.Text) <> "" Then
            smstxt = "SYSTEM NO : " & Trim(txt_SystemSerialNo.Text)
        End If
        If Trim(txt_LicenseCode.Text) <> "" Then
            smstxt = Trim(smstxt) & IIf(Trim(smstxt) <> "", "," & vbCrLf & vbCrLf, "") & "LICENSE CODE : " & Trim(txt_LicenseCode.Text)
        End If

        txt_Message.Text = smstxt
        pnl_SMS.Visible = True
        txt_MobileNo.Focus()
    End Sub

    Private Sub btn_CloseSMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseSMS.Click
        pnl_SMS.Visible = False
        txt_LicenseCode.Focus()
    End Sub

    Private Sub txt_MobileNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MobileNo.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_MobileNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MobileNo.KeyPress
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_CustomerName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CustomerName.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_CustomerName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CustomerName.KeyPress
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Message_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Message.KeyPress
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnSendSMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendSMS.Click
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse = Nothing
        Dim url As String
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim timeout As Integer = 50000

        Dim SMSProvider_LoginID As String = ""
        Dim SMSProvider_LoginPWD As String = ""
        Dim SMSProvider_SenderID As String = ""


        Try

            PhNo = Trim(txt_MobileNo.Text)

            smstxt = ""
            If Trim(txt_Message.Text) <> "" Then
                smstxt = "CUSTOMER NAME : " & Trim(txt_CustomerName.Text)
            End If
            smstxt = Trim(smstxt) & IIf(Trim(smstxt) <> "", "," & vbCrLf & vbCrLf, "") & Trim(txt_Message.Text)

            SMSProvider_LoginID = "tsoft"
            SMSProvider_LoginPWD = "amutha"
            SMSProvider_SenderID = "TSOFTS"

            url = "http://198.24.149.4/API/pushsms.aspx?loginID=" & Trim(SMSProvider_LoginID) & "&password=" & Trim(SMSProvider_LoginPWD) & "&mobile=" & Trim(PhNo) & " &text=" & Trim(smstxt) & "&senderid=" & Trim(SMSProvider_SenderID) & "&route_id=2&Unicode=0"

            request = DirectCast(WebRequest.Create(url), HttpWebRequest)
            request.KeepAlive = True

            request.Timeout = timeout

            response = DirectCast(request.GetResponse(), HttpWebResponse)

            If Trim(UCase(response.StatusDescription)) = "OK" Then
                MessageBox.Show("Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Try
                response.Close()
                response = Nothing
                request = Nothing

            Catch ex As Exception
                '-----
            End Try

        End Try

    End Sub

    Private Sub txt_MobileNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MobileNo.GotFocus
        txt_MobileNo.BackColor = Color.Lime
        txt_MobileNo.ForeColor = Color.Blue
    End Sub

    Private Sub txt_MobileNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MobileNo.LostFocus
        txt_MobileNo.BackColor = Color.White
        txt_MobileNo.ForeColor = Color.Black
    End Sub

    Private Sub txt_CustomerName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CustomerName.GotFocus
        txt_CustomerName.BackColor = Color.Lime
        txt_CustomerName.ForeColor = Color.Blue
    End Sub

    Private Sub txt_CustomerName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CustomerName.LostFocus
        txt_CustomerName.BackColor = Color.White
        txt_CustomerName.ForeColor = Color.Black
    End Sub

    Private Sub txt_Message_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Message.GotFocus
        txt_Message.BackColor = Color.Lime
        txt_Message.ForeColor = Color.Blue
    End Sub

    Private Sub txt_Message_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Message.LostFocus
        txt_Message.BackColor = Color.White
        txt_Message.ForeColor = Color.Black
    End Sub

End Class