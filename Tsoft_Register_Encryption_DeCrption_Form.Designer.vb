<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Tsoft_Register_Encryption_DeCrption_Form
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.pnl_Register = New System.Windows.Forms.Panel()
        Me.btn_SendSms = New System.Windows.Forms.Button()
        Me.btn_UnRegister = New System.Windows.Forms.Button()
        Me.txt_SystemSerialNo = New System.Windows.Forms.TextBox()
        Me.btn_Generate_LicenseCode = New System.Windows.Forms.Button()
        Me.btn_Register = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_LicenseCode = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btn_Login = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_SqlPassword = New System.Windows.Forms.TextBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_UserPwd_EncryptionCode = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txt_UserPassword = New System.Windows.Forms.TextBox()
        Me.pnl_SMS = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txt_Message = New System.Windows.Forms.TextBox()
        Me.btnSendSMS = New System.Windows.Forms.Button()
        Me.btn_CloseSMS = New System.Windows.Forms.Button()
        Me.txt_MobileNo = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.txt_CustomerName = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.pnl_Register.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.pnl_SMS.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BackgroundImage = Global.Billing.My.Resources.Resources.Mdi_Background
        Me.pnl_Back.Controls.Add(Me.pnl_SMS)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.TabControl1)
        Me.pnl_Back.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Back.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(753, 378)
        Me.pnl_Back.TabIndex = 0
        '
        'btn_Close
        '
        Me.btn_Close.FlatAppearance.BorderSize = 2
        Me.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.Red
        Me.btn_Close.Location = New System.Drawing.Point(598, 322)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(91, 38)
        Me.btn_Close.TabIndex = 3
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 25)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(717, 279)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl1.TabIndex = 20
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.pnl_Register)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(709, 251)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "REGISTER"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'pnl_Register
        '
        Me.pnl_Register.Controls.Add(Me.btn_SendSms)
        Me.pnl_Register.Controls.Add(Me.btn_UnRegister)
        Me.pnl_Register.Controls.Add(Me.txt_SystemSerialNo)
        Me.pnl_Register.Controls.Add(Me.btn_Generate_LicenseCode)
        Me.pnl_Register.Controls.Add(Me.btn_Register)
        Me.pnl_Register.Controls.Add(Me.Label5)
        Me.pnl_Register.Controls.Add(Me.txt_LicenseCode)
        Me.pnl_Register.Controls.Add(Me.Label6)
        Me.pnl_Register.Location = New System.Drawing.Point(28, 35)
        Me.pnl_Register.Name = "pnl_Register"
        Me.pnl_Register.Size = New System.Drawing.Size(645, 177)
        Me.pnl_Register.TabIndex = 19
        '
        'btn_SendSms
        '
        Me.btn_SendSms.FlatAppearance.BorderSize = 2
        Me.btn_SendSms.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_SendSms.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_SendSms.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_SendSms.ForeColor = System.Drawing.Color.Black
        Me.btn_SendSms.Location = New System.Drawing.Point(343, 118)
        Me.btn_SendSms.Name = "btn_SendSms"
        Me.btn_SendSms.Size = New System.Drawing.Size(87, 32)
        Me.btn_SendSms.TabIndex = 28
        Me.btn_SendSms.TabStop = False
        Me.btn_SendSms.Text = "&SEND SMS"
        Me.btn_SendSms.UseVisualStyleBackColor = False
        '
        'btn_UnRegister
        '
        Me.btn_UnRegister.FlatAppearance.BorderSize = 2
        Me.btn_UnRegister.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_UnRegister.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_UnRegister.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_UnRegister.ForeColor = System.Drawing.Color.Red
        Me.btn_UnRegister.Location = New System.Drawing.Point(225, 117)
        Me.btn_UnRegister.Name = "btn_UnRegister"
        Me.btn_UnRegister.Size = New System.Drawing.Size(87, 32)
        Me.btn_UnRegister.TabIndex = 27
        Me.btn_UnRegister.TabStop = False
        Me.btn_UnRegister.Text = "&UN-REGISTER"
        Me.btn_UnRegister.UseVisualStyleBackColor = False
        '
        'txt_SystemSerialNo
        '
        Me.txt_SystemSerialNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_SystemSerialNo.Enabled = False
        Me.txt_SystemSerialNo.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SystemSerialNo.Location = New System.Drawing.Point(107, 27)
        Me.txt_SystemSerialNo.MaxLength = 40
        Me.txt_SystemSerialNo.Name = "txt_SystemSerialNo"
        Me.txt_SystemSerialNo.Size = New System.Drawing.Size(520, 30)
        Me.txt_SystemSerialNo.TabIndex = 0
        '
        'btn_Generate_LicenseCode
        '
        Me.btn_Generate_LicenseCode.FlatAppearance.BorderSize = 2
        Me.btn_Generate_LicenseCode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Generate_LicenseCode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Generate_LicenseCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Generate_LicenseCode.ForeColor = System.Drawing.Color.Navy
        Me.btn_Generate_LicenseCode.Location = New System.Drawing.Point(461, 118)
        Me.btn_Generate_LicenseCode.Name = "btn_Generate_LicenseCode"
        Me.btn_Generate_LicenseCode.Size = New System.Drawing.Size(164, 32)
        Me.btn_Generate_LicenseCode.TabIndex = 3
        Me.btn_Generate_LicenseCode.TabStop = False
        Me.btn_Generate_LicenseCode.Text = "&GENERATE LICENSE CODE"
        Me.btn_Generate_LicenseCode.UseVisualStyleBackColor = False
        '
        'btn_Register
        '
        Me.btn_Register.FlatAppearance.BorderSize = 2
        Me.btn_Register.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Register.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Register.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Register.ForeColor = System.Drawing.Color.Navy
        Me.btn_Register.Location = New System.Drawing.Point(107, 118)
        Me.btn_Register.Name = "btn_Register"
        Me.btn_Register.Size = New System.Drawing.Size(87, 32)
        Me.btn_Register.TabIndex = 2
        Me.btn_Register.TabStop = False
        Me.btn_Register.Text = "&REGISTER"
        Me.btn_Register.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(13, 76)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 15)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "License Code"
        '
        'txt_LicenseCode
        '
        Me.txt_LicenseCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_LicenseCode.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_LicenseCode.Location = New System.Drawing.Point(107, 72)
        Me.txt_LicenseCode.MaxLength = 40
        Me.txt_LicenseCode.Name = "txt_LicenseCode"
        Me.txt_LicenseCode.Size = New System.Drawing.Size(520, 30)
        Me.txt_LicenseCode.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(13, 31)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 15)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "System No"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel4)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(709, 251)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "SQL CODE"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.btn_Login)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.TextBox1)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.txt_SqlPassword)
        Me.Panel4.Location = New System.Drawing.Point(50, 51)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(540, 183)
        Me.Panel4.TabIndex = 21
        '
        'btn_Login
        '
        Me.btn_Login.FlatAppearance.BorderSize = 2
        Me.btn_Login.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Login.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Login.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Login.ForeColor = System.Drawing.Color.Navy
        Me.btn_Login.Location = New System.Drawing.Point(208, 109)
        Me.btn_Login.Name = "btn_Login"
        Me.btn_Login.Size = New System.Drawing.Size(87, 38)
        Me.btn_Login.TabIndex = 22
        Me.btn_Login.TabStop = False
        Me.btn_Login.Text = "&SHOW CODE"
        Me.btn_Login.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(39, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Code"
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(100, 80)
        Me.TextBox1.MaxLength = 40
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(381, 23)
        Me.TextBox1.TabIndex = 20
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(39, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 13)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Enter Sql Password"
        '
        'txt_SqlPassword
        '
        Me.txt_SqlPassword.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SqlPassword.Location = New System.Drawing.Point(182, 47)
        Me.txt_SqlPassword.MaxLength = 40
        Me.txt_SqlPassword.Name = "txt_SqlPassword"
        Me.txt_SqlPassword.Size = New System.Drawing.Size(299, 23)
        Me.txt_SqlPassword.TabIndex = 18
        '
        'TabPage3
        '
        Me.TabPage3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TabPage3.Controls.Add(Me.Panel3)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(709, 251)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "USER CODE"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.txt_UserPwd_EncryptionCode)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.txt_UserPassword)
        Me.Panel3.Location = New System.Drawing.Point(59, 72)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(522, 140)
        Me.Panel3.TabIndex = 20
        '
        'Button1
        '
        Me.Button1.FlatAppearance.BorderSize = 2
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.Button1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.Navy
        Me.Button1.Location = New System.Drawing.Point(209, 82)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(87, 38)
        Me.Button1.TabIndex = 27
        Me.Button1.TabStop = False
        Me.Button1.Text = "&SHOW CODE"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(40, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(36, 13)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "Code"
        '
        'txt_UserPwd_EncryptionCode
        '
        Me.txt_UserPwd_EncryptionCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_UserPwd_EncryptionCode.Location = New System.Drawing.Point(101, 53)
        Me.txt_UserPwd_EncryptionCode.MaxLength = 40
        Me.txt_UserPwd_EncryptionCode.Name = "txt_UserPwd_EncryptionCode"
        Me.txt_UserPwd_EncryptionCode.Size = New System.Drawing.Size(381, 23)
        Me.txt_UserPwd_EncryptionCode.TabIndex = 25
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(40, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(125, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Enter User Password"
        '
        'txt_UserPassword
        '
        Me.txt_UserPassword.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_UserPassword.Location = New System.Drawing.Point(209, 20)
        Me.txt_UserPassword.MaxLength = 40
        Me.txt_UserPassword.Name = "txt_UserPassword"
        Me.txt_UserPassword.Size = New System.Drawing.Size(273, 23)
        Me.txt_UserPassword.TabIndex = 23
        '
        'pnl_SMS
        '
        Me.pnl_SMS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_SMS.Controls.Add(Me.txt_CustomerName)
        Me.pnl_SMS.Controls.Add(Me.Label8)
        Me.pnl_SMS.Controls.Add(Me.lbl_Heading)
        Me.pnl_SMS.Controls.Add(Me.Label7)
        Me.pnl_SMS.Controls.Add(Me.Label19)
        Me.pnl_SMS.Controls.Add(Me.txt_Message)
        Me.pnl_SMS.Controls.Add(Me.btnSendSMS)
        Me.pnl_SMS.Controls.Add(Me.btn_CloseSMS)
        Me.pnl_SMS.Controls.Add(Me.txt_MobileNo)
        Me.pnl_SMS.Controls.Add(Me.Label11)
        Me.pnl_SMS.Location = New System.Drawing.Point(864, 71)
        Me.pnl_SMS.Name = "pnl_SMS"
        Me.pnl_SMS.Size = New System.Drawing.Size(480, 244)
        Me.pnl_SMS.TabIndex = 36
        Me.pnl_SMS.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Navy
        Me.Label7.Location = New System.Drawing.Point(12, 117)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 15)
        Me.Label7.TabIndex = 60
        Me.Label7.Text = "MESSAGE"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Navy
        Me.Label19.Location = New System.Drawing.Point(16, 55)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(0, 15)
        Me.Label19.TabIndex = 59
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Message
        '
        Me.txt_Message.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Message.Location = New System.Drawing.Point(122, 113)
        Me.txt_Message.MaxLength = 720
        Me.txt_Message.Multiline = True
        Me.txt_Message.Name = "txt_Message"
        Me.txt_Message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_Message.Size = New System.Drawing.Size(337, 66)
        Me.txt_Message.TabIndex = 2
        '
        'btnSendSMS
        '
        Me.btnSendSMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSendSMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSendSMS.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSendSMS.ForeColor = System.Drawing.Color.Navy
        Me.btnSendSMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSendSMS.Location = New System.Drawing.Point(225, 191)
        Me.btnSendSMS.Name = "btnSendSMS"
        Me.btnSendSMS.Size = New System.Drawing.Size(106, 31)
        Me.btnSendSMS.TabIndex = 3
        Me.btnSendSMS.TabStop = False
        Me.btnSendSMS.Text = "SEND SMS"
        Me.btnSendSMS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSendSMS.UseVisualStyleBackColor = True
        '
        'btn_CloseSMS
        '
        Me.btn_CloseSMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_CloseSMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_CloseSMS.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseSMS.ForeColor = System.Drawing.Color.Navy
        Me.btn_CloseSMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseSMS.Location = New System.Drawing.Point(353, 191)
        Me.btn_CloseSMS.Name = "btn_CloseSMS"
        Me.btn_CloseSMS.Size = New System.Drawing.Size(106, 31)
        Me.btn_CloseSMS.TabIndex = 4
        Me.btn_CloseSMS.TabStop = False
        Me.btn_CloseSMS.Text = "CLOSE"
        Me.btn_CloseSMS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseSMS.UseVisualStyleBackColor = True
        '
        'txt_MobileNo
        '
        Me.txt_MobileNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_MobileNo.Location = New System.Drawing.Point(122, 43)
        Me.txt_MobileNo.MaxLength = 50
        Me.txt_MobileNo.Name = "txt_MobileNo"
        Me.txt_MobileNo.Size = New System.Drawing.Size(337, 23)
        Me.txt_MobileNo.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(12, 47)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(88, 15)
        Me.Label11.TabIndex = 19
        Me.Label11.Text = "TO MOBILE NO"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(478, 30)
        Me.lbl_Heading.TabIndex = 61
        Me.lbl_Heading.Text = "MESSAGE"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txt_CustomerName
        '
        Me.txt_CustomerName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_CustomerName.Location = New System.Drawing.Point(122, 78)
        Me.txt_CustomerName.MaxLength = 50
        Me.txt_CustomerName.Name = "txt_CustomerName"
        Me.txt_CustomerName.Size = New System.Drawing.Size(337, 23)
        Me.txt_CustomerName.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Navy
        Me.Label8.Location = New System.Drawing.Point(12, 82)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(104, 15)
        Me.Label8.TabIndex = 63
        Me.Label8.Text = "CUSTOMER NAME"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Tsoft_Register_Encryption_DeCrption_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImage = Global.Billing.My.Resources.Resources.Mdi_Background
        Me.ClientSize = New System.Drawing.Size(753, 378)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Tsoft_Register_Encryption_DeCrption_Form"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TSOFT REGISTER"
        Me.pnl_Back.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.pnl_Register.ResumeLayout(False)
        Me.pnl_Register.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.pnl_SMS.ResumeLayout(False)
        Me.pnl_SMS.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents pnl_Register As System.Windows.Forms.Panel
    Friend WithEvents btn_Generate_LicenseCode As System.Windows.Forms.Button
    Friend WithEvents btn_Register As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txt_LicenseCode As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btn_Login As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_SqlPassword As System.Windows.Forms.TextBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_UserPwd_EncryptionCode As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txt_UserPassword As System.Windows.Forms.TextBox
    Friend WithEvents txt_SystemSerialNo As System.Windows.Forms.TextBox
    Friend WithEvents btn_SendSms As System.Windows.Forms.Button
    Friend WithEvents btn_UnRegister As System.Windows.Forms.Button
    Friend WithEvents pnl_SMS As System.Windows.Forms.Panel
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txt_Message As System.Windows.Forms.TextBox
    Friend WithEvents btnSendSMS As System.Windows.Forms.Button
    Friend WithEvents btn_CloseSMS As System.Windows.Forms.Button
    Friend WithEvents txt_MobileNo As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_CustomerName As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
End Class
