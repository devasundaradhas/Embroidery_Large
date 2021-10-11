<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Ledger_Creation_SMS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Ledger_Creation_SMS))
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.grp_Back = New System.Windows.Forms.GroupBox()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.btn_BirthDay_Sms = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.msk_WeddingDate = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_WeddingDate = New System.Windows.Forms.DateTimePicker()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.msk_BirthDate = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_BirthDate = New System.Windows.Forms.DateTimePicker()
        Me.txt_PanNo = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btn_Send_All_SMS = New System.Windows.Forms.Button()
        Me.btn_Send_Single_SMS = New System.Windows.Forms.Button()
        Me.Cbo_State = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btn_WeddingDay_Sms = New System.Windows.Forms.Button()
        Me.txt_EmailID = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.cbo_Area = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txt_AlaisName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txt_PhoneNo = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txt_Address4 = New System.Windows.Forms.TextBox()
        Me.txt_Address3 = New System.Windows.Forms.TextBox()
        Me.txt_Address2 = New System.Windows.Forms.TextBox()
        Me.txt_Address1 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbo_PriceListName = New System.Windows.Forms.ComboBox()
        Me.lbl_Price_Agent = New System.Windows.Forms.Label()
        Me.cbo_Agent = New System.Windows.Forms.ComboBox()
        Me.cbo_LedgerGroup = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cbo_BillType = New System.Windows.Forms.ComboBox()
        Me.cbo_AcGroup = New System.Windows.Forms.ComboBox()
        Me.txt_CstNo = New System.Windows.Forms.TextBox()
        Me.lbl_CstNo = New System.Windows.Forms.Label()
        Me.txt_TinNo = New System.Windows.Forms.TextBox()
        Me.lbl_TinNo = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Filter = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.Ledger_IdNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Ledger_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.pnl_Reading = New System.Windows.Forms.Panel()
        Me.txt_TotalMachine = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.cbo_Machine = New System.Windows.Forms.ComboBox()
        Me.txt_RateExtraCopy = New System.Windows.Forms.TextBox()
        Me.txt_FreeCopiesMachine = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_RentMachine = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btn_Close_Reading = New System.Windows.Forms.Button()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.dgv_Details = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.grp_Back.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Open.SuspendLayout()
        Me.pnl_Reading.SuspendLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grp_Back
        '
        Me.grp_Back.BackColor = System.Drawing.Color.Transparent
        Me.grp_Back.Controls.Add(Me.btn_close)
        Me.grp_Back.Controls.Add(Me.btn_save)
        Me.grp_Back.Controls.Add(Me.btn_BirthDay_Sms)
        Me.grp_Back.Controls.Add(Me.Label19)
        Me.grp_Back.Controls.Add(Me.msk_WeddingDate)
        Me.grp_Back.Controls.Add(Me.dtp_WeddingDate)
        Me.grp_Back.Controls.Add(Me.Label18)
        Me.grp_Back.Controls.Add(Me.msk_BirthDate)
        Me.grp_Back.Controls.Add(Me.dtp_BirthDate)
        Me.grp_Back.Controls.Add(Me.txt_PanNo)
        Me.grp_Back.Controls.Add(Me.Label10)
        Me.grp_Back.Controls.Add(Me.btn_Send_All_SMS)
        Me.grp_Back.Controls.Add(Me.btn_Send_Single_SMS)
        Me.grp_Back.Controls.Add(Me.Cbo_State)
        Me.grp_Back.Controls.Add(Me.Label17)
        Me.grp_Back.Controls.Add(Me.btn_WeddingDay_Sms)
        Me.grp_Back.Controls.Add(Me.txt_EmailID)
        Me.grp_Back.Controls.Add(Me.Label12)
        Me.grp_Back.Controls.Add(Me.lbl_IdNo)
        Me.grp_Back.Controls.Add(Me.cbo_Area)
        Me.grp_Back.Controls.Add(Me.Label7)
        Me.grp_Back.Controls.Add(Me.txt_AlaisName)
        Me.grp_Back.Controls.Add(Me.Label6)
        Me.grp_Back.Controls.Add(Me.txt_PhoneNo)
        Me.grp_Back.Controls.Add(Me.Label9)
        Me.grp_Back.Controls.Add(Me.txt_Address4)
        Me.grp_Back.Controls.Add(Me.txt_Address3)
        Me.grp_Back.Controls.Add(Me.txt_Address2)
        Me.grp_Back.Controls.Add(Me.txt_Address1)
        Me.grp_Back.Controls.Add(Me.Label5)
        Me.grp_Back.Controls.Add(Me.txt_Name)
        Me.grp_Back.Controls.Add(Me.Label2)
        Me.grp_Back.Controls.Add(Me.Label1)
        Me.grp_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Back.Location = New System.Drawing.Point(0, 39)
        Me.grp_Back.Name = "grp_Back"
        Me.grp_Back.Size = New System.Drawing.Size(677, 513)
        Me.grp_Back.TabIndex = 1
        Me.grp_Back.TabStop = False
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(571, 463)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(80, 32)
        Me.btn_close.TabIndex = 57
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(571, 429)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(80, 32)
        Me.btn_save.TabIndex = 56
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'btn_BirthDay_Sms
        '
        Me.btn_BirthDay_Sms.BackColor = System.Drawing.Color.PaleTurquoise
        Me.btn_BirthDay_Sms.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_BirthDay_Sms.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_BirthDay_Sms.ForeColor = System.Drawing.Color.Navy
        Me.btn_BirthDay_Sms.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_BirthDay_Sms.Location = New System.Drawing.Point(22, 430)
        Me.btn_BirthDay_Sms.Name = "btn_BirthDay_Sms"
        Me.btn_BirthDay_Sms.Size = New System.Drawing.Size(139, 29)
        Me.btn_BirthDay_Sms.TabIndex = 55
        Me.btn_BirthDay_Sms.Text = "&BIRTH DAY SMS"
        Me.btn_BirthDay_Sms.UseVisualStyleBackColor = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Navy
        Me.Label19.Location = New System.Drawing.Point(387, 394)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(85, 15)
        Me.Label19.TabIndex = 54
        Me.Label19.Text = "Wedding Date"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'msk_WeddingDate
        '
        Me.msk_WeddingDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_WeddingDate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_WeddingDate.Location = New System.Drawing.Point(515, 390)
        Me.msk_WeddingDate.Mask = "00-00-0000"
        Me.msk_WeddingDate.Name = "msk_WeddingDate"
        Me.msk_WeddingDate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_WeddingDate.Size = New System.Drawing.Size(116, 22)
        Me.msk_WeddingDate.TabIndex = 53
        '
        'dtp_WeddingDate
        '
        Me.dtp_WeddingDate.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_WeddingDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_WeddingDate.Location = New System.Drawing.Point(630, 390)
        Me.dtp_WeddingDate.Name = "dtp_WeddingDate"
        Me.dtp_WeddingDate.Size = New System.Drawing.Size(19, 22)
        Me.dtp_WeddingDate.TabIndex = 52
        Me.dtp_WeddingDate.TabStop = False
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.Color.Navy
        Me.Label18.Location = New System.Drawing.Point(22, 394)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(63, 15)
        Me.Label18.TabIndex = 51
        Me.Label18.Text = "Birth Date"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'msk_BirthDate
        '
        Me.msk_BirthDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_BirthDate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_BirthDate.Location = New System.Drawing.Point(140, 390)
        Me.msk_BirthDate.Mask = "00-00-0000"
        Me.msk_BirthDate.Name = "msk_BirthDate"
        Me.msk_BirthDate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_BirthDate.Size = New System.Drawing.Size(116, 22)
        Me.msk_BirthDate.TabIndex = 50
        '
        'dtp_BirthDate
        '
        Me.dtp_BirthDate.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_BirthDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_BirthDate.Location = New System.Drawing.Point(253, 390)
        Me.dtp_BirthDate.Name = "dtp_BirthDate"
        Me.dtp_BirthDate.Size = New System.Drawing.Size(19, 22)
        Me.dtp_BirthDate.TabIndex = 48
        Me.dtp_BirthDate.TabStop = False
        '
        'txt_PanNo
        '
        Me.txt_PanNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PanNo.Location = New System.Drawing.Point(141, 359)
        Me.txt_PanNo.Name = "txt_PanNo"
        Me.txt_PanNo.Size = New System.Drawing.Size(510, 23)
        Me.txt_PanNo.TabIndex = 42
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Navy
        Me.Label10.Location = New System.Drawing.Point(22, 363)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 15)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Pan No"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btn_Send_All_SMS
        '
        Me.btn_Send_All_SMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Send_All_SMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Send_All_SMS.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Send_All_SMS.ForeColor = System.Drawing.Color.Navy
        Me.btn_Send_All_SMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Send_All_SMS.Location = New System.Drawing.Point(177, 461)
        Me.btn_Send_All_SMS.Name = "btn_Send_All_SMS"
        Me.btn_Send_All_SMS.Size = New System.Drawing.Size(138, 30)
        Me.btn_Send_All_SMS.TabIndex = 40
        Me.btn_Send_All_SMS.TabStop = False
        Me.btn_Send_All_SMS.Text = "Send SMS ( ALL )"
        Me.btn_Send_All_SMS.UseVisualStyleBackColor = True
        '
        'btn_Send_Single_SMS
        '
        Me.btn_Send_Single_SMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Send_Single_SMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Send_Single_SMS.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Send_Single_SMS.ForeColor = System.Drawing.Color.Navy
        Me.btn_Send_Single_SMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Send_Single_SMS.Location = New System.Drawing.Point(177, 429)
        Me.btn_Send_Single_SMS.Name = "btn_Send_Single_SMS"
        Me.btn_Send_Single_SMS.Size = New System.Drawing.Size(138, 31)
        Me.btn_Send_Single_SMS.TabIndex = 39
        Me.btn_Send_Single_SMS.TabStop = False
        Me.btn_Send_Single_SMS.Text = "Send SMS (Single)"
        Me.btn_Send_Single_SMS.UseVisualStyleBackColor = True
        '
        'Cbo_State
        '
        Me.Cbo_State.DropDownHeight = 70
        Me.Cbo_State.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cbo_State.FormattingEnabled = True
        Me.Cbo_State.IntegralHeight = False
        Me.Cbo_State.Location = New System.Drawing.Point(141, 266)
        Me.Cbo_State.Name = "Cbo_State"
        Me.Cbo_State.Size = New System.Drawing.Size(510, 23)
        Me.Cbo_State.TabIndex = 9
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.Navy
        Me.Label17.Location = New System.Drawing.Point(22, 270)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(36, 15)
        Me.Label17.TabIndex = 38
        Me.Label17.Text = "State"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btn_WeddingDay_Sms
        '
        Me.btn_WeddingDay_Sms.BackColor = System.Drawing.Color.PaleTurquoise
        Me.btn_WeddingDay_Sms.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_WeddingDay_Sms.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_WeddingDay_Sms.ForeColor = System.Drawing.Color.Navy
        Me.btn_WeddingDay_Sms.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_WeddingDay_Sms.Location = New System.Drawing.Point(22, 461)
        Me.btn_WeddingDay_Sms.Name = "btn_WeddingDay_Sms"
        Me.btn_WeddingDay_Sms.Size = New System.Drawing.Size(139, 29)
        Me.btn_WeddingDay_Sms.TabIndex = 34
        Me.btn_WeddingDay_Sms.Text = "&WEDDING DAY SMS"
        Me.btn_WeddingDay_Sms.UseVisualStyleBackColor = False
        '
        'txt_EmailID
        '
        Me.txt_EmailID.Location = New System.Drawing.Point(141, 297)
        Me.txt_EmailID.Name = "txt_EmailID"
        Me.txt_EmailID.Size = New System.Drawing.Size(510, 23)
        Me.txt_EmailID.TabIndex = 10
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Navy
        Me.Label12.Location = New System.Drawing.Point(22, 301)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(54, 15)
        Me.Label12.TabIndex = 17
        Me.Label12.Text = "E-Mail ID"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(141, 18)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(510, 23)
        Me.lbl_IdNo.TabIndex = 16
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbo_Area
        '
        Me.cbo_Area.DropDownHeight = 75
        Me.cbo_Area.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Area.FormattingEnabled = True
        Me.cbo_Area.IntegralHeight = False
        Me.cbo_Area.Location = New System.Drawing.Point(141, 111)
        Me.cbo_Area.Name = "cbo_Area"
        Me.cbo_Area.Size = New System.Drawing.Size(510, 23)
        Me.cbo_Area.TabIndex = 2
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Navy
        Me.Label7.Location = New System.Drawing.Point(22, 115)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(69, 15)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Area Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_AlaisName
        '
        Me.txt_AlaisName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_AlaisName.Location = New System.Drawing.Point(141, 80)
        Me.txt_AlaisName.Name = "txt_AlaisName"
        Me.txt_AlaisName.Size = New System.Drawing.Size(510, 23)
        Me.txt_AlaisName.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(22, 84)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(109, 15)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Ledger Alais Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_PhoneNo
        '
        Me.txt_PhoneNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PhoneNo.Location = New System.Drawing.Point(141, 328)
        Me.txt_PhoneNo.Name = "txt_PhoneNo"
        Me.txt_PhoneNo.Size = New System.Drawing.Size(510, 23)
        Me.txt_PhoneNo.TabIndex = 11
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Navy
        Me.Label9.Location = New System.Drawing.Point(22, 332)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(61, 15)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Phone No"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Address4
        '
        Me.txt_Address4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address4.Location = New System.Drawing.Point(141, 235)
        Me.txt_Address4.Name = "txt_Address4"
        Me.txt_Address4.Size = New System.Drawing.Size(510, 23)
        Me.txt_Address4.TabIndex = 8
        '
        'txt_Address3
        '
        Me.txt_Address3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address3.Location = New System.Drawing.Point(141, 204)
        Me.txt_Address3.Name = "txt_Address3"
        Me.txt_Address3.Size = New System.Drawing.Size(510, 23)
        Me.txt_Address3.TabIndex = 7
        '
        'txt_Address2
        '
        Me.txt_Address2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address2.Location = New System.Drawing.Point(141, 173)
        Me.txt_Address2.Name = "txt_Address2"
        Me.txt_Address2.Size = New System.Drawing.Size(510, 23)
        Me.txt_Address2.TabIndex = 6
        '
        'txt_Address1
        '
        Me.txt_Address1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address1.Location = New System.Drawing.Point(141, 142)
        Me.txt_Address1.Name = "txt_Address1"
        Me.txt_Address1.Size = New System.Drawing.Size(510, 23)
        Me.txt_Address1.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(22, 146)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Address"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Name
        '
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(141, 49)
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(510, 23)
        Me.txt_Name.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(22, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Ledger Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(22, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbo_PriceListName
        '
        Me.cbo_PriceListName.DropDownHeight = 70
        Me.cbo_PriceListName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_PriceListName.FormattingEnabled = True
        Me.cbo_PriceListName.IntegralHeight = False
        Me.cbo_PriceListName.Location = New System.Drawing.Point(879, 249)
        Me.cbo_PriceListName.Name = "cbo_PriceListName"
        Me.cbo_PriceListName.Size = New System.Drawing.Size(128, 23)
        Me.cbo_PriceListName.TabIndex = 14
        Me.cbo_PriceListName.Text = "cbo_PriceListName"
        Me.cbo_PriceListName.Visible = False
        '
        'lbl_Price_Agent
        '
        Me.lbl_Price_Agent.AutoSize = True
        Me.lbl_Price_Agent.ForeColor = System.Drawing.Color.Navy
        Me.lbl_Price_Agent.Location = New System.Drawing.Point(781, 252)
        Me.lbl_Price_Agent.Name = "lbl_Price_Agent"
        Me.lbl_Price_Agent.Size = New System.Drawing.Size(92, 15)
        Me.lbl_Price_Agent.TabIndex = 18
        Me.lbl_Price_Agent.Text = "Price List Name"
        Me.lbl_Price_Agent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_Price_Agent.Visible = False
        '
        'cbo_Agent
        '
        Me.cbo_Agent.DropDownHeight = 70
        Me.cbo_Agent.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Agent.FormattingEnabled = True
        Me.cbo_Agent.IntegralHeight = False
        Me.cbo_Agent.Location = New System.Drawing.Point(879, 278)
        Me.cbo_Agent.Name = "cbo_Agent"
        Me.cbo_Agent.Size = New System.Drawing.Size(110, 23)
        Me.cbo_Agent.TabIndex = 14
        Me.cbo_Agent.Text = "cbo_Agent"
        Me.cbo_Agent.Visible = False
        '
        'cbo_LedgerGroup
        '
        Me.cbo_LedgerGroup.DropDownHeight = 70
        Me.cbo_LedgerGroup.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_LedgerGroup.FormattingEnabled = True
        Me.cbo_LedgerGroup.IntegralHeight = False
        Me.cbo_LedgerGroup.Location = New System.Drawing.Point(879, 100)
        Me.cbo_LedgerGroup.Name = "cbo_LedgerGroup"
        Me.cbo_LedgerGroup.Size = New System.Drawing.Size(128, 23)
        Me.cbo_LedgerGroup.TabIndex = 4
        Me.cbo_LedgerGroup.Text = "cbo_LedgerGroup"
        Me.cbo_LedgerGroup.Visible = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Navy
        Me.Label16.Location = New System.Drawing.Point(781, 105)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(82, 15)
        Me.Label16.TabIndex = 35
        Me.Label16.Text = "Ledger Group"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label16.Visible = False
        '
        'cbo_BillType
        '
        Me.cbo_BillType.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_BillType.FormattingEnabled = True
        Me.cbo_BillType.Location = New System.Drawing.Point(879, 132)
        Me.cbo_BillType.Name = "cbo_BillType"
        Me.cbo_BillType.Size = New System.Drawing.Size(128, 23)
        Me.cbo_BillType.TabIndex = 3
        Me.cbo_BillType.Text = "cbo_BillType"
        Me.cbo_BillType.Visible = False
        '
        'cbo_AcGroup
        '
        Me.cbo_AcGroup.DropDownHeight = 70
        Me.cbo_AcGroup.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_AcGroup.FormattingEnabled = True
        Me.cbo_AcGroup.IntegralHeight = False
        Me.cbo_AcGroup.Location = New System.Drawing.Point(879, 71)
        Me.cbo_AcGroup.Name = "cbo_AcGroup"
        Me.cbo_AcGroup.Size = New System.Drawing.Size(128, 23)
        Me.cbo_AcGroup.TabIndex = 3
        Me.cbo_AcGroup.Text = "cbo_AcGroup"
        Me.cbo_AcGroup.Visible = False
        '
        'txt_CstNo
        '
        Me.txt_CstNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_CstNo.Location = New System.Drawing.Point(879, 210)
        Me.txt_CstNo.Name = "txt_CstNo"
        Me.txt_CstNo.Size = New System.Drawing.Size(128, 23)
        Me.txt_CstNo.TabIndex = 13
        Me.txt_CstNo.Text = "txt_CstNo"
        Me.txt_CstNo.Visible = False
        '
        'lbl_CstNo
        '
        Me.lbl_CstNo.AutoSize = True
        Me.lbl_CstNo.ForeColor = System.Drawing.Color.Navy
        Me.lbl_CstNo.Location = New System.Drawing.Point(781, 210)
        Me.lbl_CstNo.Name = "lbl_CstNo"
        Me.lbl_CstNo.Size = New System.Drawing.Size(43, 15)
        Me.lbl_CstNo.TabIndex = 0
        Me.lbl_CstNo.Text = "Cst No"
        Me.lbl_CstNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_CstNo.Visible = False
        '
        'txt_TinNo
        '
        Me.txt_TinNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_TinNo.Location = New System.Drawing.Point(879, 176)
        Me.txt_TinNo.Name = "txt_TinNo"
        Me.txt_TinNo.Size = New System.Drawing.Size(128, 23)
        Me.txt_TinNo.TabIndex = 12
        Me.txt_TinNo.Text = "txt_TinNo"
        Me.txt_TinNo.Visible = False
        '
        'lbl_TinNo
        '
        Me.lbl_TinNo.AutoSize = True
        Me.lbl_TinNo.ForeColor = System.Drawing.Color.Navy
        Me.lbl_TinNo.Location = New System.Drawing.Point(781, 179)
        Me.lbl_TinNo.Name = "lbl_TinNo"
        Me.lbl_TinNo.Size = New System.Drawing.Size(42, 15)
        Me.lbl_TinNo.TabIndex = 0
        Me.lbl_TinNo.Text = "Tin No"
        Me.lbl_TinNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_TinNo.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(781, 135)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 15)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Bill Type"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(781, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 15)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "A/c Group"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label3.Visible = False
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(1061, 75)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(597, 345)
        Me.grp_Filter.TabIndex = 32
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "Filter"
        Me.grp_Filter.Visible = False
        '
        'btn_Filter
        '
        Me.btn_Filter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter.Image = CType(resources.GetObject("btn_Filter.Image"), System.Drawing.Image)
        Me.btn_Filter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter.Location = New System.Drawing.Point(386, 308)
        Me.btn_Filter.Name = "btn_Filter"
        Me.btn_Filter.Size = New System.Drawing.Size(83, 29)
        Me.btn_Filter.TabIndex = 33
        Me.btn_Filter.Text = "&Open"
        Me.btn_Filter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Filter.UseVisualStyleBackColor = True
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.Image = CType(resources.GetObject("btn_CloseFilter.Image"), System.Drawing.Image)
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(482, 308)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(83, 29)
        Me.btn_CloseFilter.TabIndex = 32
        Me.btn_CloseFilter.Text = "&Close"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = True
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.AllowUserToResizeColumns = False
        Me.dgv_Filter.AllowUserToResizeRows = False
        DataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle21
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Ledger_IdNo, Me.Ledger_Name})
        Me.dgv_Filter.Location = New System.Drawing.Point(14, 30)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(551, 271)
        Me.dgv_Filter.TabIndex = 0
        '
        'Ledger_IdNo
        '
        Me.Ledger_IdNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.Ledger_IdNo.FillWeight = 40.0!
        Me.Ledger_IdNo.HeaderText = "LEDGER IDNO"
        Me.Ledger_IdNo.Name = "Ledger_IdNo"
        Me.Ledger_IdNo.ReadOnly = True
        Me.Ledger_IdNo.Width = 105
        '
        'Ledger_Name
        '
        Me.Ledger_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.Ledger_Name.FillWeight = 160.0!
        Me.Ledger_Name.HeaderText = "LEDGER NAME"
        Me.Ledger_Name.Name = "Ledger_Name"
        Me.Ledger_Name.ReadOnly = True
        Me.Ledger_Name.Width = 110
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(856, 491)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(534, 247)
        Me.grp_Open.TabIndex = 31
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "Finding"
        Me.grp_Open.Visible = False
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(332, 197)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(83, 29)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'cbo_Open
        '
        Me.cbo_Open.DropDownHeight = 120
        Me.cbo_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(19, 32)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(493, 23)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseOpen.Image = CType(resources.GetObject("btn_CloseOpen.Image"), System.Drawing.Image)
        Me.btn_CloseOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseOpen.Location = New System.Drawing.Point(428, 197)
        Me.btn_CloseOpen.Name = "btn_CloseOpen"
        Me.btn_CloseOpen.Size = New System.Drawing.Size(83, 29)
        Me.btn_CloseOpen.TabIndex = 30
        Me.btn_CloseOpen.Text = "&Close"
        Me.btn_CloseOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseOpen.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(675, 40)
        Me.Label8.TabIndex = 33
        Me.Label8.Text = "LEDGER CREATION"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_Reading
        '
        Me.pnl_Reading.BackColor = System.Drawing.Color.LightSkyBlue
        Me.pnl_Reading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Reading.Controls.Add(Me.txt_TotalMachine)
        Me.pnl_Reading.Controls.Add(Me.Label15)
        Me.pnl_Reading.Controls.Add(Me.cbo_Machine)
        Me.pnl_Reading.Controls.Add(Me.txt_RateExtraCopy)
        Me.pnl_Reading.Controls.Add(Me.txt_FreeCopiesMachine)
        Me.pnl_Reading.Controls.Add(Me.Label14)
        Me.pnl_Reading.Controls.Add(Me.Label13)
        Me.pnl_Reading.Controls.Add(Me.txt_RentMachine)
        Me.pnl_Reading.Controls.Add(Me.Label11)
        Me.pnl_Reading.Controls.Add(Me.btn_Close_Reading)
        Me.pnl_Reading.Controls.Add(Me.Label39)
        Me.pnl_Reading.Controls.Add(Me.dgv_Details)
        Me.pnl_Reading.Controls.Add(Me.Label40)
        Me.pnl_Reading.Location = New System.Drawing.Point(45, 677)
        Me.pnl_Reading.Margin = New System.Windows.Forms.Padding(0)
        Me.pnl_Reading.Name = "pnl_Reading"
        Me.pnl_Reading.Size = New System.Drawing.Size(578, 377)
        Me.pnl_Reading.TabIndex = 267
        Me.pnl_Reading.Visible = False
        '
        'txt_TotalMachine
        '
        Me.txt_TotalMachine.Location = New System.Drawing.Point(423, 93)
        Me.txt_TotalMachine.MaxLength = 5
        Me.txt_TotalMachine.Name = "txt_TotalMachine"
        Me.txt_TotalMachine.Size = New System.Drawing.Size(138, 23)
        Me.txt_TotalMachine.TabIndex = 47
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.Color.Navy
        Me.Label15.Location = New System.Drawing.Point(294, 97)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(82, 15)
        Me.Label15.TabIndex = 49
        Me.Label15.Text = "Total Machine"
        '
        'cbo_Machine
        '
        Me.cbo_Machine.FormattingEnabled = True
        Me.cbo_Machine.Location = New System.Drawing.Point(76, 240)
        Me.cbo_Machine.Name = "cbo_Machine"
        Me.cbo_Machine.Size = New System.Drawing.Size(121, 23)
        Me.cbo_Machine.TabIndex = 48
        Me.cbo_Machine.Text = "cbo_Machine"
        '
        'txt_RateExtraCopy
        '
        Me.txt_RateExtraCopy.Location = New System.Drawing.Point(107, 93)
        Me.txt_RateExtraCopy.MaxLength = 5
        Me.txt_RateExtraCopy.Name = "txt_RateExtraCopy"
        Me.txt_RateExtraCopy.Size = New System.Drawing.Size(181, 23)
        Me.txt_RateExtraCopy.TabIndex = 46
        '
        'txt_FreeCopiesMachine
        '
        Me.txt_FreeCopiesMachine.Location = New System.Drawing.Point(423, 46)
        Me.txt_FreeCopiesMachine.MaxLength = 12
        Me.txt_FreeCopiesMachine.Name = "txt_FreeCopiesMachine"
        Me.txt_FreeCopiesMachine.Size = New System.Drawing.Size(138, 23)
        Me.txt_FreeCopiesMachine.TabIndex = 45
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.Color.Navy
        Me.Label14.Location = New System.Drawing.Point(294, 50)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(123, 15)
        Me.Label14.TabIndex = 46
        Me.Label14.Text = "Free Copies/Machine"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Navy
        Me.Label13.Location = New System.Drawing.Point(5, 97)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(96, 15)
        Me.Label13.TabIndex = 45
        Me.Label13.Text = "Rate/Extra Copy"
        '
        'txt_RentMachine
        '
        Me.txt_RentMachine.Location = New System.Drawing.Point(107, 46)
        Me.txt_RentMachine.MaxLength = 12
        Me.txt_RentMachine.Name = "txt_RentMachine"
        Me.txt_RentMachine.Size = New System.Drawing.Size(181, 23)
        Me.txt_RentMachine.TabIndex = 44
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(5, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 15)
        Me.Label11.TabIndex = 43
        Me.Label11.Text = "Rent/Machine"
        '
        'btn_Close_Reading
        '
        Me.btn_Close_Reading.BackColor = System.Drawing.Color.White
        Me.btn_Close_Reading.BackgroundImage = Global.Billing.My.Resources.Resources.Close1
        Me.btn_Close_Reading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Close_Reading.FlatAppearance.BorderSize = 0
        Me.btn_Close_Reading.Location = New System.Drawing.Point(548, -1)
        Me.btn_Close_Reading.Name = "btn_Close_Reading"
        Me.btn_Close_Reading.Size = New System.Drawing.Size(29, 30)
        Me.btn_Close_Reading.TabIndex = 49
        Me.btn_Close_Reading.UseVisualStyleBackColor = True
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.BackColor = System.Drawing.Color.Purple
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.ForeColor = System.Drawing.Color.White
        Me.Label39.Location = New System.Drawing.Point(402, -36)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(71, 20)
        Me.Label39.TabIndex = 37
        Me.Label39.Text = "FILTER"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgv_Details
        '
        Me.dgv_Details.AllowUserToResizeColumns = False
        Me.dgv_Details.AllowUserToResizeRows = False
        Me.dgv_Details.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Details.CausesValidation = False
        DataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle22.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle22.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle22.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle22
        Me.dgv_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn14, Me.DataGridViewTextBoxColumn15, Me.Column11})
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle25.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle25.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle25.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle25.SelectionForeColor = System.Drawing.Color.MidnightBlue
        DataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Details.DefaultCellStyle = DataGridViewCellStyle25
        Me.dgv_Details.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgv_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_Details.Location = New System.Drawing.Point(0, 134)
        Me.dgv_Details.Name = "dgv_Details"
        Me.dgv_Details.RowHeadersVisible = False
        Me.dgv_Details.RowHeadersWidth = 15
        Me.dgv_Details.RowTemplate.Height = 25
        Me.dgv_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_Details.Size = New System.Drawing.Size(576, 241)
        Me.dgv_Details.TabIndex = 47
        Me.dgv_Details.TabStop = False
        '
        'DataGridViewTextBoxColumn14
        '
        DataGridViewCellStyle23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn14.DefaultCellStyle = DataGridViewCellStyle23
        Me.DataGridViewTextBoxColumn14.Frozen = True
        Me.DataGridViewTextBoxColumn14.HeaderText = "S.NO"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        Me.DataGridViewTextBoxColumn14.ReadOnly = True
        Me.DataGridViewTextBoxColumn14.Width = 50
        '
        'DataGridViewTextBoxColumn15
        '
        Me.DataGridViewTextBoxColumn15.HeaderText = "MACHINE NAME"
        Me.DataGridViewTextBoxColumn15.Name = "DataGridViewTextBoxColumn15"
        Me.DataGridViewTextBoxColumn15.ReadOnly = True
        Me.DataGridViewTextBoxColumn15.Width = 300
        '
        'Column11
        '
        DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column11.DefaultCellStyle = DataGridViewCellStyle24
        Me.Column11.HeaderText = "OPENING READING"
        Me.Column11.Name = "Column11"
        Me.Column11.Width = 200
        '
        'Label40
        '
        Me.Label40.BackColor = System.Drawing.Color.DeepPink
        Me.Label40.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label40.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label40.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.ForeColor = System.Drawing.Color.White
        Me.Label40.Location = New System.Drawing.Point(0, 0)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(576, 35)
        Me.Label40.TabIndex = 41
        Me.Label40.Text = "OPENING READING"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Ledger_Creation_SMS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PaleTurquoise
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(675, 542)
        Me.Controls.Add(Me.pnl_Reading)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cbo_Agent)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.grp_Back)
        Me.Controls.Add(Me.cbo_PriceListName)
        Me.Controls.Add(Me.lbl_Price_Agent)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.cbo_LedgerGroup)
        Me.Controls.Add(Me.cbo_AcGroup)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cbo_BillType)
        Me.Controls.Add(Me.txt_TinNo)
        Me.Controls.Add(Me.lbl_TinNo)
        Me.Controls.Add(Me.txt_CstNo)
        Me.Controls.Add(Me.lbl_CstNo)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Ledger_Creation_SMS"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LEDGER CREATION"
        Me.grp_Back.ResumeLayout(False)
        Me.grp_Back.PerformLayout()
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Open.ResumeLayout(False)
        Me.pnl_Reading.ResumeLayout(False)
        Me.pnl_Reading.PerformLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grp_Back As System.Windows.Forms.GroupBox
    Friend WithEvents txt_CstNo As System.Windows.Forms.TextBox
    Friend WithEvents lbl_CstNo As System.Windows.Forms.Label
    Friend WithEvents txt_TinNo As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TinNo As System.Windows.Forms.Label
    Friend WithEvents txt_PhoneNo As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txt_Address4 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address3 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address2 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address1 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbo_BillType As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_AcGroup As System.Windows.Forms.ComboBox
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Filter As System.Windows.Forms.Button
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents Ledger_IdNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Ledger_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_AlaisName As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbo_Area As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents txt_EmailID As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cbo_PriceListName As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Price_Agent As System.Windows.Forms.Label
    Friend WithEvents pnl_Reading As System.Windows.Forms.Panel
    Friend WithEvents txt_RateExtraCopy As System.Windows.Forms.TextBox
    Friend WithEvents txt_FreeCopiesMachine As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txt_RentMachine As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btn_Close_Reading As System.Windows.Forms.Button
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents dgv_Details As System.Windows.Forms.DataGridView
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents cbo_Machine As System.Windows.Forms.ComboBox
    Friend WithEvents btn_WeddingDay_Sms As System.Windows.Forms.Button
    Friend WithEvents txt_TotalMachine As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Cbo_State As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents cbo_LedgerGroup As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTextBoxColumn14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn15 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbo_Agent As System.Windows.Forms.ComboBox
    Friend WithEvents btn_Send_All_SMS As System.Windows.Forms.Button
    Friend WithEvents btn_Send_Single_SMS As System.Windows.Forms.Button
    Friend WithEvents txt_PanNo As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents msk_WeddingDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_WeddingDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents msk_BirthDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_BirthDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btn_BirthDay_Sms As System.Windows.Forms.Button
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
End Class
