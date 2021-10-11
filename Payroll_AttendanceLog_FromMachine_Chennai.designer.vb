<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Payroll_AttendanceLog_FromMachine_Chennai
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Payroll_AttendanceLog_FromMachine_Chennai))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Pnl_Back = New System.Windows.Forms.Panel()
        Me.btnClearGLog = New System.Windows.Forms.Button()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.lvLogs = New System.Windows.Forms.ListView()
        Me.lvLogsch1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvLogsch2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvLogsch3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvLogsch4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvLogsch5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvLogsch6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnGetDeviceStatus = New System.Windows.Forms.Button()
        Me.groupBox2 = New System.Windows.Forms.GroupBox()
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.tabPage1 = New System.Windows.Forms.TabPage()
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tabPage2 = New System.Windows.Forms.TabPage()
        Me.groupBox5 = New System.Windows.Forms.GroupBox()
        Me.cbBaudRate = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtMachineSN = New System.Windows.Forms.TextBox()
        Me.cbPort = New System.Windows.Forms.ComboBox()
        Me.label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnRsConnect = New System.Windows.Forms.Button()
        Me.tabPage3 = New System.Windows.Forms.TabPage()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtMachineSN2 = New System.Windows.Forms.TextBox()
        Me.label18 = New System.Windows.Forms.Label()
        Me.btnUSBConnect = New System.Windows.Forms.Button()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.btn_Settings_FileFrom = New System.Windows.Forms.Button()
        Me.btn_GenerateLogFromFile = New System.Windows.Forms.Button()
        Me.btn_SelectFile = New System.Windows.Forms.Button()
        Me.txt_FileName = New System.Windows.Forms.TextBox()
        Me.lblState = New System.Windows.Forms.Label()
        Me.btnGetGeneralLogData = New System.Windows.Forms.Button()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.pnl_Settings_FileFrom = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txt_LineStartFrom_FileFrom = New System.Windows.Forms.TextBox()
        Me.lbl_btn_CloseFileFromSettings = New System.Windows.Forms.Label()
        Me.lbl_btn_SaveFileFromSettings = New System.Windows.Forms.Label()
        Me.lbl_btn_CloseSettings_FileFrom = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_EmpAttDate_FileFrom = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txt_EmpInOut_FileFrom = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_EmpCardNo_FileFrom = New System.Windows.Forms.TextBox()
        Me.lbl_btn_ResetDefault_Filefrom = New System.Windows.Forms.Label()
        Me.Pnl_Back.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.tabPage1.SuspendLayout()
        Me.tabPage2.SuspendLayout()
        Me.groupBox5.SuspendLayout()
        Me.tabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.pnl_Settings_FileFrom.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(672, 35)
        Me.Label1.TabIndex = 131
        Me.Label1.Text = "ATTENDANCE LOG FROM MACHINE"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Pnl_Back
        '
        Me.Pnl_Back.BackColor = System.Drawing.Color.LightSkyBlue
        Me.Pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Pnl_Back.Controls.Add(Me.btnClearGLog)
        Me.Pnl_Back.Controls.Add(Me.groupBox1)
        Me.Pnl_Back.Controls.Add(Me.btnGetDeviceStatus)
        Me.Pnl_Back.Controls.Add(Me.groupBox2)
        Me.Pnl_Back.Controls.Add(Me.btnGetGeneralLogData)
        Me.Pnl_Back.Controls.Add(Me.btn_close)
        Me.Pnl_Back.Controls.Add(Me.btn_save)
        Me.Pnl_Back.Controls.Add(Me.lbl_RefNo)
        Me.Pnl_Back.Controls.Add(Me.dtp_Date)
        Me.Pnl_Back.Controls.Add(Me.Label4)
        Me.Pnl_Back.Controls.Add(Me.Label2)
        Me.Pnl_Back.Location = New System.Drawing.Point(6, 40)
        Me.Pnl_Back.Name = "Pnl_Back"
        Me.Pnl_Back.Size = New System.Drawing.Size(651, 506)
        Me.Pnl_Back.TabIndex = 129
        '
        'btnClearGLog
        '
        Me.btnClearGLog.Location = New System.Drawing.Point(500, 274)
        Me.btnClearGLog.Name = "btnClearGLog"
        Me.btnClearGLog.Size = New System.Drawing.Size(136, 30)
        Me.btnClearGLog.TabIndex = 6
        Me.btnClearGLog.Text = "Clear All Attendance Log"
        Me.btnClearGLog.UseVisualStyleBackColor = True
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.lvLogs)
        Me.groupBox1.ForeColor = System.Drawing.Color.DarkBlue
        Me.groupBox1.Location = New System.Drawing.Point(17, 182)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(472, 311)
        Me.groupBox1.TabIndex = 15
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Attendance Records"
        '
        'lvLogs
        '
        Me.lvLogs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvLogsch1, Me.lvLogsch2, Me.lvLogsch3, Me.lvLogsch4, Me.lvLogsch5, Me.lvLogsch6})
        Me.lvLogs.GridLines = True
        Me.lvLogs.Location = New System.Drawing.Point(10, 16)
        Me.lvLogs.Name = "lvLogs"
        Me.lvLogs.Size = New System.Drawing.Size(449, 290)
        Me.lvLogs.TabIndex = 0
        Me.lvLogs.UseCompatibleStateImageBehavior = False
        Me.lvLogs.View = System.Windows.Forms.View.Details
        '
        'lvLogsch1
        '
        Me.lvLogsch1.Text = "S.NO"
        Me.lvLogsch1.Width = 61
        '
        'lvLogsch2
        '
        Me.lvLogsch2.Text = "Emp Card No"
        Me.lvLogsch2.Width = 97
        '
        'lvLogsch3
        '
        Me.lvLogsch3.Text = "VerifyMode"
        Me.lvLogsch3.Width = 1
        '
        'lvLogsch4
        '
        Me.lvLogsch4.Text = "InOut Mode"
        Me.lvLogsch4.Width = 97
        '
        'lvLogsch5
        '
        Me.lvLogsch5.Text = "Punch Date & Time"
        Me.lvLogsch5.Width = 188
        '
        'lvLogsch6
        '
        Me.lvLogsch6.Text = "WorkCode"
        Me.lvLogsch6.Width = 0
        '
        'btnGetDeviceStatus
        '
        Me.btnGetDeviceStatus.Location = New System.Drawing.Point(500, 240)
        Me.btnGetDeviceStatus.Name = "btnGetDeviceStatus"
        Me.btnGetDeviceStatus.Size = New System.Drawing.Size(136, 30)
        Me.btnGetDeviceStatus.TabIndex = 3
        Me.btnGetDeviceStatus.Text = "Get Record Count"
        Me.btnGetDeviceStatus.UseVisualStyleBackColor = True
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.tabControl1)
        Me.groupBox2.Controls.Add(Me.lblState)
        Me.groupBox2.Location = New System.Drawing.Point(17, 39)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(473, 143)
        Me.groupBox2.TabIndex = 14
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Communication with Device"
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tabPage1)
        Me.tabControl1.Controls.Add(Me.tabPage2)
        Me.tabControl1.Controls.Add(Me.tabPage3)
        Me.tabControl1.Controls.Add(Me.TabPage4)
        Me.tabControl1.Location = New System.Drawing.Point(6, 20)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(458, 102)
        Me.tabControl1.TabIndex = 7
        '
        'tabPage1
        '
        Me.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tabPage1.Controls.Add(Me.txtIP)
        Me.tabPage1.Controls.Add(Me.Label3)
        Me.tabPage1.Controls.Add(Me.btnConnect)
        Me.tabPage1.Controls.Add(Me.txtPort)
        Me.tabPage1.Controls.Add(Me.Label5)
        Me.tabPage1.Cursor = System.Windows.Forms.Cursors.Default
        Me.tabPage1.ForeColor = System.Drawing.Color.DarkBlue
        Me.tabPage1.Location = New System.Drawing.Point(4, 24)
        Me.tabPage1.Name = "tabPage1"
        Me.tabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPage1.Size = New System.Drawing.Size(450, 74)
        Me.tabPage1.TabIndex = 0
        Me.tabPage1.Text = "TCP/IP"
        Me.tabPage1.UseVisualStyleBackColor = True
        '
        'txtIP
        '
        Me.txtIP.Location = New System.Drawing.Point(118, 14)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(95, 23)
        Me.txtIP.TabIndex = 6
        Me.txtIP.Text = "192.168.1.201"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(257, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 15)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Port"
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(183, 47)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnConnect.TabIndex = 4
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(300, 14)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(53, 23)
        Me.txtPort.TabIndex = 7
        Me.txtPort.Text = "4370"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(87, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(17, 15)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "IP"
        '
        'tabPage2
        '
        Me.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tabPage2.Controls.Add(Me.groupBox5)
        Me.tabPage2.Controls.Add(Me.btnRsConnect)
        Me.tabPage2.ForeColor = System.Drawing.Color.DarkBlue
        Me.tabPage2.Location = New System.Drawing.Point(4, 24)
        Me.tabPage2.Name = "tabPage2"
        Me.tabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPage2.Size = New System.Drawing.Size(450, 74)
        Me.tabPage2.TabIndex = 1
        Me.tabPage2.Text = "RS232/485"
        Me.tabPage2.UseVisualStyleBackColor = True
        '
        'groupBox5
        '
        Me.groupBox5.Controls.Add(Me.cbBaudRate)
        Me.groupBox5.Controls.Add(Me.Label6)
        Me.groupBox5.Controls.Add(Me.txtMachineSN)
        Me.groupBox5.Controls.Add(Me.cbPort)
        Me.groupBox5.Controls.Add(Me.label7)
        Me.groupBox5.Controls.Add(Me.Label8)
        Me.groupBox5.Location = New System.Drawing.Point(17, -1)
        Me.groupBox5.Name = "groupBox5"
        Me.groupBox5.Size = New System.Drawing.Size(406, 40)
        Me.groupBox5.TabIndex = 12
        Me.groupBox5.TabStop = False
        '
        'cbBaudRate
        '
        Me.cbBaudRate.FormattingEnabled = True
        Me.cbBaudRate.Items.AddRange(New Object() {"1200", "2400", "4800", "9600", "19200", "38400", "115200"})
        Me.cbBaudRate.Location = New System.Drawing.Point(187, 14)
        Me.cbBaudRate.Name = "cbBaudRate"
        Me.cbBaudRate.Size = New System.Drawing.Size(65, 23)
        Me.cbBaudRate.TabIndex = 6
        Me.cbBaudRate.Text = "115200"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(31, 15)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Port"
        '
        'txtMachineSN
        '
        Me.txtMachineSN.Location = New System.Drawing.Point(337, 14)
        Me.txtMachineSN.Name = "txtMachineSN"
        Me.txtMachineSN.Size = New System.Drawing.Size(56, 23)
        Me.txtMachineSN.TabIndex = 10
        Me.txtMachineSN.Text = "1"
        '
        'cbPort
        '
        Me.cbPort.FormattingEnabled = True
        Me.cbPort.Items.AddRange(New Object() {"COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9"})
        Me.cbPort.Location = New System.Drawing.Point(52, 14)
        Me.cbPort.Name = "cbPort"
        Me.cbPort.Size = New System.Drawing.Size(56, 23)
        Me.cbPort.TabIndex = 5
        Me.cbPort.Text = "COM1"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(265, 18)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(68, 15)
        Me.label7.TabIndex = 9
        Me.label7.Text = "MachineSN"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(121, 18)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(59, 15)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "BaudRate"
        '
        'btnRsConnect
        '
        Me.btnRsConnect.Location = New System.Drawing.Point(183, 47)
        Me.btnRsConnect.Name = "btnRsConnect"
        Me.btnRsConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnRsConnect.TabIndex = 11
        Me.btnRsConnect.Text = "Connect"
        Me.btnRsConnect.UseVisualStyleBackColor = True
        '
        'tabPage3
        '
        Me.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tabPage3.Controls.Add(Me.Label9)
        Me.tabPage3.Controls.Add(Me.txtMachineSN2)
        Me.tabPage3.Controls.Add(Me.label18)
        Me.tabPage3.Controls.Add(Me.btnUSBConnect)
        Me.tabPage3.ForeColor = System.Drawing.Color.DarkBlue
        Me.tabPage3.Location = New System.Drawing.Point(4, 24)
        Me.tabPage3.Name = "tabPage3"
        Me.tabPage3.Size = New System.Drawing.Size(450, 74)
        Me.tabPage3.TabIndex = 2
        Me.tabPage3.Text = "USBClient"
        Me.tabPage3.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(233, 17)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(68, 15)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "MachineSN"
        '
        'txtMachineSN2
        '
        Me.txtMachineSN2.BackColor = System.Drawing.Color.AliceBlue
        Me.txtMachineSN2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMachineSN2.Location = New System.Drawing.Point(294, 12)
        Me.txtMachineSN2.Name = "txtMachineSN2"
        Me.txtMachineSN2.Size = New System.Drawing.Size(27, 23)
        Me.txtMachineSN2.TabIndex = 13
        Me.txtMachineSN2.Text = "1"
        '
        'label18
        '
        Me.label18.AutoSize = True
        Me.label18.ForeColor = System.Drawing.Color.Crimson
        Me.label18.Location = New System.Drawing.Point(120, 17)
        Me.label18.Name = "label18"
        Me.label18.Size = New System.Drawing.Size(100, 15)
        Me.label18.TabIndex = 12
        Me.label18.Text = "Virtual USBClient"
        '
        'btnUSBConnect
        '
        Me.btnUSBConnect.Location = New System.Drawing.Point(183, 41)
        Me.btnUSBConnect.Name = "btnUSBConnect"
        Me.btnUSBConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnUSBConnect.TabIndex = 11
        Me.btnUSBConnect.Text = "Connect"
        Me.btnUSBConnect.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.btn_Settings_FileFrom)
        Me.TabPage4.Controls.Add(Me.btn_GenerateLogFromFile)
        Me.TabPage4.Controls.Add(Me.btn_SelectFile)
        Me.TabPage4.Controls.Add(Me.txt_FileName)
        Me.TabPage4.Location = New System.Drawing.Point(4, 24)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(450, 74)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "From File"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'btn_Settings_FileFrom
        '
        Me.btn_Settings_FileFrom.BackColor = System.Drawing.Color.Transparent
        Me.btn_Settings_FileFrom.BackgroundImage = CType(resources.GetObject("btn_Settings_FileFrom.BackgroundImage"), System.Drawing.Image)
        Me.btn_Settings_FileFrom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn_Settings_FileFrom.Location = New System.Drawing.Point(0, 51)
        Me.btn_Settings_FileFrom.Name = "btn_Settings_FileFrom"
        Me.btn_Settings_FileFrom.Size = New System.Drawing.Size(26, 23)
        Me.btn_Settings_FileFrom.TabIndex = 20
        Me.btn_Settings_FileFrom.UseVisualStyleBackColor = False
        '
        'btn_GenerateLogFromFile
        '
        Me.btn_GenerateLogFromFile.Location = New System.Drawing.Point(394, 21)
        Me.btn_GenerateLogFromFile.Name = "btn_GenerateLogFromFile"
        Me.btn_GenerateLogFromFile.Size = New System.Drawing.Size(53, 24)
        Me.btn_GenerateLogFromFile.TabIndex = 19
        Me.btn_GenerateLogFromFile.Text = "Open"
        Me.btn_GenerateLogFromFile.UseVisualStyleBackColor = True
        '
        'btn_SelectFile
        '
        Me.btn_SelectFile.Location = New System.Drawing.Point(340, 21)
        Me.btn_SelectFile.Name = "btn_SelectFile"
        Me.btn_SelectFile.Size = New System.Drawing.Size(53, 24)
        Me.btn_SelectFile.TabIndex = 16
        Me.btn_SelectFile.Text = "Select"
        Me.btn_SelectFile.UseVisualStyleBackColor = True
        '
        'txt_FileName
        '
        Me.txt_FileName.Location = New System.Drawing.Point(6, 21)
        Me.txt_FileName.Name = "txt_FileName"
        Me.txt_FileName.Size = New System.Drawing.Size(328, 23)
        Me.txt_FileName.TabIndex = 18
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.ForeColor = System.Drawing.Color.Crimson
        Me.lblState.Location = New System.Drawing.Point(150, 125)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(159, 15)
        Me.lblState.TabIndex = 2
        Me.lblState.Text = "Current State:Disconnected"
        '
        'btnGetGeneralLogData
        '
        Me.btnGetGeneralLogData.Location = New System.Drawing.Point(500, 206)
        Me.btnGetGeneralLogData.Name = "btnGetGeneralLogData"
        Me.btnGetGeneralLogData.Size = New System.Drawing.Size(136, 30)
        Me.btnGetGeneralLogData.TabIndex = 1
        Me.btnGetGeneralLogData.Text = "Get Attendance Log Data"
        Me.btnGetGeneralLogData.UseVisualStyleBackColor = True
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(500, 460)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(136, 30)
        Me.btn_close.TabIndex = 13
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(500, 423)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(136, 30)
        Me.btn_save.TabIndex = 11
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_RefNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RefNo.Location = New System.Drawing.Point(93, 5)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(165, 23)
        Me.lbl_RefNo.TabIndex = 9
        Me.lbl_RefNo.Text = "lbl_RefNo"
        Me.lbl_RefNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtp_Date
        '
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(315, 5)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(175, 23)
        Me.dtp_Date.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(265, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 15)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Date"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(14, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 15)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Ref No"
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'pnl_Settings_FileFrom
        '
        Me.pnl_Settings_FileFrom.BackColor = System.Drawing.Color.Silver
        Me.pnl_Settings_FileFrom.Controls.Add(Me.lbl_btn_ResetDefault_Filefrom)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.Label14)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.txt_LineStartFrom_FileFrom)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.lbl_btn_CloseFileFromSettings)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.lbl_btn_SaveFileFromSettings)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.lbl_btn_CloseSettings_FileFrom)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.Label13)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.txt_EmpAttDate_FileFrom)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.Label12)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.txt_EmpInOut_FileFrom)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.Label11)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.Label10)
        Me.pnl_Settings_FileFrom.Controls.Add(Me.txt_EmpCardNo_FileFrom)
        Me.pnl_Settings_FileFrom.Location = New System.Drawing.Point(785, 100)
        Me.pnl_Settings_FileFrom.Name = "pnl_Settings_FileFrom"
        Me.pnl_Settings_FileFrom.Size = New System.Drawing.Size(290, 177)
        Me.pnl_Settings_FileFrom.TabIndex = 132
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Black
        Me.Label14.Location = New System.Drawing.Point(19, 123)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(92, 15)
        Me.Label14.TabIndex = 30
        Me.Label14.Text = "Line Start From"
        '
        'txt_LineStartFrom_FileFrom
        '
        Me.txt_LineStartFrom_FileFrom.Location = New System.Drawing.Point(152, 120)
        Me.txt_LineStartFrom_FileFrom.Name = "txt_LineStartFrom_FileFrom"
        Me.txt_LineStartFrom_FileFrom.Size = New System.Drawing.Size(115, 23)
        Me.txt_LineStartFrom_FileFrom.TabIndex = 31
        '
        'lbl_btn_CloseFileFromSettings
        '
        Me.lbl_btn_CloseFileFromSettings.AutoSize = True
        Me.lbl_btn_CloseFileFromSettings.Font = New System.Drawing.Font("Calibri", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_btn_CloseFileFromSettings.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_btn_CloseFileFromSettings.Location = New System.Drawing.Point(222, 147)
        Me.lbl_btn_CloseFileFromSettings.Name = "lbl_btn_CloseFileFromSettings"
        Me.lbl_btn_CloseFileFromSettings.Size = New System.Drawing.Size(36, 15)
        Me.lbl_btn_CloseFileFromSettings.TabIndex = 29
        Me.lbl_btn_CloseFileFromSettings.Text = "Close"
        Me.lbl_btn_CloseFileFromSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_btn_SaveFileFromSettings
        '
        Me.lbl_btn_SaveFileFromSettings.AutoSize = True
        Me.lbl_btn_SaveFileFromSettings.Font = New System.Drawing.Font("Calibri", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_btn_SaveFileFromSettings.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_btn_SaveFileFromSettings.Location = New System.Drawing.Point(167, 147)
        Me.lbl_btn_SaveFileFromSettings.Name = "lbl_btn_SaveFileFromSettings"
        Me.lbl_btn_SaveFileFromSettings.Size = New System.Drawing.Size(32, 15)
        Me.lbl_btn_SaveFileFromSettings.TabIndex = 28
        Me.lbl_btn_SaveFileFromSettings.Text = "Save"
        '
        'lbl_btn_CloseSettings_FileFrom
        '
        Me.lbl_btn_CloseSettings_FileFrom.AutoSize = True
        Me.lbl_btn_CloseSettings_FileFrom.BackColor = System.Drawing.Color.Black
        Me.lbl_btn_CloseSettings_FileFrom.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_btn_CloseSettings_FileFrom.ForeColor = System.Drawing.Color.White
        Me.lbl_btn_CloseSettings_FileFrom.Location = New System.Drawing.Point(273, 3)
        Me.lbl_btn_CloseSettings_FileFrom.Name = "lbl_btn_CloseSettings_FileFrom"
        Me.lbl_btn_CloseSettings_FileFrom.Size = New System.Drawing.Size(14, 15)
        Me.lbl_btn_CloseSettings_FileFrom.TabIndex = 27
        Me.lbl_btn_CloseSettings_FileFrom.Text = "X"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Black
        Me.Label13.Location = New System.Drawing.Point(19, 91)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(98, 15)
        Me.Label13.TabIndex = 25
        Me.Label13.Text = "Attandance Date"
        '
        'txt_EmpAttDate_FileFrom
        '
        Me.txt_EmpAttDate_FileFrom.Location = New System.Drawing.Point(152, 88)
        Me.txt_EmpAttDate_FileFrom.Name = "txt_EmpAttDate_FileFrom"
        Me.txt_EmpAttDate_FileFrom.Size = New System.Drawing.Size(115, 23)
        Me.txt_EmpAttDate_FileFrom.TabIndex = 26
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Black
        Me.Label12.Location = New System.Drawing.Point(19, 62)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(82, 15)
        Me.Label12.TabIndex = 23
        Me.Label12.Text = "In /Out Mode"
        '
        'txt_EmpInOut_FileFrom
        '
        Me.txt_EmpInOut_FileFrom.Location = New System.Drawing.Point(152, 59)
        Me.txt_EmpInOut_FileFrom.Name = "txt_EmpInOut_FileFrom"
        Me.txt_EmpInOut_FileFrom.Size = New System.Drawing.Size(115, 23)
        Me.txt_EmpInOut_FileFrom.TabIndex = 24
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.Black
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(290, 20)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "Set Attandance Log File Column Index"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(19, 34)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(108, 15)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "Employee Card No"
        '
        'txt_EmpCardNo_FileFrom
        '
        Me.txt_EmpCardNo_FileFrom.Location = New System.Drawing.Point(152, 31)
        Me.txt_EmpCardNo_FileFrom.Name = "txt_EmpCardNo_FileFrom"
        Me.txt_EmpCardNo_FileFrom.Size = New System.Drawing.Size(115, 23)
        Me.txt_EmpCardNo_FileFrom.TabIndex = 21
        '
        'lbl_btn_ResetDefault_Filefrom
        '
        Me.lbl_btn_ResetDefault_Filefrom.AutoSize = True
        Me.lbl_btn_ResetDefault_Filefrom.Font = New System.Drawing.Font("Calibri", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_btn_ResetDefault_Filefrom.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_btn_ResetDefault_Filefrom.Location = New System.Drawing.Point(3, 147)
        Me.lbl_btn_ResetDefault_Filefrom.Name = "lbl_btn_ResetDefault_Filefrom"
        Me.lbl_btn_ResetDefault_Filefrom.Size = New System.Drawing.Size(81, 15)
        Me.lbl_btn_ResetDefault_Filefrom.TabIndex = 32
        Me.lbl_btn_ResetDefault_Filefrom.Text = "Reset Default"
        '
        'Payroll_AttendanceLog_FromMachine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(672, 564)
        Me.Controls.Add(Me.pnl_Settings_FileFrom)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Payroll_AttendanceLog_FromMachine"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ATTENDANCE LOG FROM MACHINE"
        Me.Pnl_Back.ResumeLayout(False)
        Me.Pnl_Back.PerformLayout()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.tabControl1.ResumeLayout(False)
        Me.tabPage1.ResumeLayout(False)
        Me.tabPage1.PerformLayout()
        Me.tabPage2.ResumeLayout(False)
        Me.groupBox5.ResumeLayout(False)
        Me.groupBox5.PerformLayout()
        Me.tabPage3.ResumeLayout(False)
        Me.tabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.pnl_Settings_FileFrom.ResumeLayout(False)
        Me.pnl_Settings_FileFrom.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents lbl_RefNo As System.Windows.Forms.Label
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents tabControl1 As System.Windows.Forms.TabControl
    Private WithEvents tabPage1 As System.Windows.Forms.TabPage
    Private WithEvents txtIP As System.Windows.Forms.TextBox
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents btnConnect As System.Windows.Forms.Button
    Private WithEvents txtPort As System.Windows.Forms.TextBox
    Private WithEvents Label5 As System.Windows.Forms.Label
    Private WithEvents tabPage2 As System.Windows.Forms.TabPage
    Private WithEvents groupBox5 As System.Windows.Forms.GroupBox
    Private WithEvents cbBaudRate As System.Windows.Forms.ComboBox
    Private WithEvents Label6 As System.Windows.Forms.Label
    Private WithEvents txtMachineSN As System.Windows.Forms.TextBox
    Private WithEvents cbPort As System.Windows.Forms.ComboBox
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents Label8 As System.Windows.Forms.Label
    Private WithEvents btnRsConnect As System.Windows.Forms.Button
    Private WithEvents tabPage3 As System.Windows.Forms.TabPage
    Private WithEvents Label9 As System.Windows.Forms.Label
    Private WithEvents txtMachineSN2 As System.Windows.Forms.TextBox
    Private WithEvents label18 As System.Windows.Forms.Label
    Private WithEvents btnUSBConnect As System.Windows.Forms.Button
    Private WithEvents lblState As System.Windows.Forms.Label
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents btnClearGLog As System.Windows.Forms.Button
    Private WithEvents btnGetDeviceStatus As System.Windows.Forms.Button
    Private WithEvents btnGetGeneralLogData As System.Windows.Forms.Button
    Private WithEvents lvLogs As System.Windows.Forms.ListView
    Private WithEvents lvLogsch1 As System.Windows.Forms.ColumnHeader
    Private WithEvents lvLogsch2 As System.Windows.Forms.ColumnHeader
    Private WithEvents lvLogsch3 As System.Windows.Forms.ColumnHeader
    Private WithEvents lvLogsch4 As System.Windows.Forms.ColumnHeader
    Private WithEvents lvLogsch5 As System.Windows.Forms.ColumnHeader
    Private WithEvents lvLogsch6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txt_FileName As System.Windows.Forms.TextBox
    Private WithEvents btn_GenerateLogFromFile As System.Windows.Forms.Button
    Private WithEvents btn_SelectFile As System.Windows.Forms.Button
    Friend WithEvents pnl_Settings_FileFrom As System.Windows.Forms.Panel
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txt_EmpAttDate_FileFrom As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txt_EmpInOut_FileFrom As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txt_EmpCardNo_FileFrom As System.Windows.Forms.TextBox
    Friend WithEvents btn_Settings_FileFrom As System.Windows.Forms.Button
    Friend WithEvents lbl_btn_CloseSettings_FileFrom As System.Windows.Forms.Label
    Friend WithEvents lbl_btn_CloseFileFromSettings As System.Windows.Forms.Label
    Friend WithEvents lbl_btn_SaveFileFromSettings As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txt_LineStartFrom_FileFrom As System.Windows.Forms.TextBox
    Friend WithEvents lbl_btn_ResetDefault_Filefrom As System.Windows.Forms.Label
End Class
