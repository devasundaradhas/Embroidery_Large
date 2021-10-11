<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Company_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Company_Creation))
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lbl_CompIDCaption = New System.Windows.Forms.Label()
        Me.txt_CompanyName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txt_Address1 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_ContactName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txt_Address2 = New System.Windows.Forms.TextBox()
        Me.txt_Address3 = New System.Windows.Forms.TextBox()
        Me.txt_Address4 = New System.Windows.Forms.TextBox()
        Me.txt_City = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txt_ShortName = New System.Windows.Forms.TextBox()
        Me.txt_FaxNo = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_PhoneNo = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txt_Description = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_EMail = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txt_PinCode = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.LabelHeader = New System.Windows.Forms.Label()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.Company_IdNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Copany_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lbl_CompID = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_ESINo = New System.Windows.Forms.TextBox()
        Me.cbo_CompanyType = New System.Windows.Forms.ComboBox()
        Me.lbl_CompanyType = New System.Windows.Forms.Label()
        Me.txt_Bank_Ac_Details = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_PanNo = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbo_Company_Designation = New System.Windows.Forms.ComboBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txt_Website = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.cbo_State = New System.Windows.Forms.ComboBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txt_GSTIN_No = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.txt_GSTP_Email_ID = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.grp_Open.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbl_CompIDCaption
        '
        Me.lbl_CompIDCaption.AutoSize = True
        Me.lbl_CompIDCaption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CompIDCaption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_CompIDCaption.Location = New System.Drawing.Point(6, 69)
        Me.lbl_CompIDCaption.Name = "lbl_CompIDCaption"
        Me.lbl_CompIDCaption.Size = New System.Drawing.Size(72, 15)
        Me.lbl_CompIDCaption.TabIndex = 3
        Me.lbl_CompIDCaption.Text = "Company ID"
        '
        'txt_CompanyName
        '
        Me.txt_CompanyName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_CompanyName.Location = New System.Drawing.Point(143, 94)
        Me.txt_CompanyName.MaxLength = 50
        Me.txt_CompanyName.Name = "txt_CompanyName"
        Me.txt_CompanyName.Size = New System.Drawing.Size(384, 23)
        Me.txt_CompanyName.TabIndex = 0
        Me.txt_CompanyName.Text = "txt_CompanyName"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(6, 97)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 15)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Company Name"
        '
        'txt_Address1
        '
        Me.txt_Address1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address1.Location = New System.Drawing.Point(143, 181)
        Me.txt_Address1.MaxLength = 100
        Me.txt_Address1.Name = "txt_Address1"
        Me.txt_Address1.Size = New System.Drawing.Size(384, 23)
        Me.txt_Address1.TabIndex = 5
        Me.txt_Address1.Text = "txt_Address1"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(6, 184)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 15)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Address "
        '
        'txt_ContactName
        '
        Me.txt_ContactName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_ContactName.Location = New System.Drawing.Point(143, 152)
        Me.txt_ContactName.MaxLength = 50
        Me.txt_ContactName.Name = "txt_ContactName"
        Me.txt_ContactName.Size = New System.Drawing.Size(137, 23)
        Me.txt_ContactName.TabIndex = 3
        Me.txt_ContactName.Text = "txt_ContactName"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(6, 158)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 15)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Name"
        '
        'txt_Address2
        '
        Me.txt_Address2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address2.Location = New System.Drawing.Point(143, 210)
        Me.txt_Address2.MaxLength = 100
        Me.txt_Address2.Name = "txt_Address2"
        Me.txt_Address2.Size = New System.Drawing.Size(384, 23)
        Me.txt_Address2.TabIndex = 6
        Me.txt_Address2.Text = "txt_Address2"
        '
        'txt_Address3
        '
        Me.txt_Address3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address3.Location = New System.Drawing.Point(143, 239)
        Me.txt_Address3.MaxLength = 100
        Me.txt_Address3.Name = "txt_Address3"
        Me.txt_Address3.Size = New System.Drawing.Size(384, 23)
        Me.txt_Address3.TabIndex = 7
        Me.txt_Address3.Text = "txt_Address3"
        '
        'txt_Address4
        '
        Me.txt_Address4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address4.Location = New System.Drawing.Point(143, 268)
        Me.txt_Address4.MaxLength = 100
        Me.txt_Address4.Name = "txt_Address4"
        Me.txt_Address4.Size = New System.Drawing.Size(384, 23)
        Me.txt_Address4.TabIndex = 8
        Me.txt_Address4.Text = "txt_Address4"
        '
        'txt_City
        '
        Me.txt_City.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_City.Location = New System.Drawing.Point(588, 173)
        Me.txt_City.MaxLength = 50
        Me.txt_City.Name = "txt_City"
        Me.txt_City.Size = New System.Drawing.Size(81, 23)
        Me.txt_City.TabIndex = 7
        Me.txt_City.Text = "txt_City"
        Me.txt_City.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(588, 145)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 15)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "City :"
        Me.Label6.Visible = False
        '
        'txt_ShortName
        '
        Me.txt_ShortName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_ShortName.Location = New System.Drawing.Point(143, 123)
        Me.txt_ShortName.MaxLength = 50
        Me.txt_ShortName.Name = "txt_ShortName"
        Me.txt_ShortName.Size = New System.Drawing.Size(136, 23)
        Me.txt_ShortName.TabIndex = 1
        Me.txt_ShortName.Text = "txt_ShortName"
        '
        'txt_FaxNo
        '
        Me.txt_FaxNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_FaxNo.Location = New System.Drawing.Point(143, 384)
        Me.txt_FaxNo.MaxLength = 50
        Me.txt_FaxNo.Name = "txt_FaxNo"
        Me.txt_FaxNo.Size = New System.Drawing.Size(141, 23)
        Me.txt_FaxNo.TabIndex = 12
        Me.txt_FaxNo.Text = "txt_FaxNo"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Navy
        Me.Label8.Location = New System.Drawing.Point(6, 389)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 15)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Fax No "
        '
        'txt_PhoneNo
        '
        Me.txt_PhoneNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PhoneNo.Location = New System.Drawing.Point(143, 326)
        Me.txt_PhoneNo.MaxLength = 50
        Me.txt_PhoneNo.Name = "txt_PhoneNo"
        Me.txt_PhoneNo.Size = New System.Drawing.Size(384, 23)
        Me.txt_PhoneNo.TabIndex = 10
        Me.txt_PhoneNo.Text = "txt_PhoneNo"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Navy
        Me.Label9.Location = New System.Drawing.Point(6, 329)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 15)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Phone No "
        '
        'txt_Description
        '
        Me.txt_Description.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Description.Location = New System.Drawing.Point(143, 501)
        Me.txt_Description.MaxLength = 200
        Me.txt_Description.Name = "txt_Description"
        Me.txt_Description.Size = New System.Drawing.Size(384, 23)
        Me.txt_Description.TabIndex = 18
        Me.txt_Description.Text = "txt_Description"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(6, 504)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 15)
        Me.Label11.TabIndex = 25
        Me.Label11.Text = "Description "
        '
        'txt_EMail
        '
        Me.txt_EMail.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_EMail.Location = New System.Drawing.Point(143, 443)
        Me.txt_EMail.MaxLength = 50
        Me.txt_EMail.Name = "txt_EMail"
        Me.txt_EMail.Size = New System.Drawing.Size(141, 23)
        Me.txt_EMail.TabIndex = 15
        Me.txt_EMail.Text = "txt_EMail"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Navy
        Me.Label12.Location = New System.Drawing.Point(6, 446)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(43, 15)
        Me.Label12.TabIndex = 23
        Me.Label12.Text = "E-Mail "
        '
        'txt_PinCode
        '
        Me.txt_PinCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PinCode.Location = New System.Drawing.Point(588, 220)
        Me.txt_PinCode.MaxLength = 10
        Me.txt_PinCode.Name = "txt_PinCode"
        Me.txt_PinCode.Size = New System.Drawing.Size(83, 23)
        Me.txt_PinCode.TabIndex = 8
        Me.txt_PinCode.Text = "txt_PinCode"
        Me.txt_PinCode.Visible = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Navy
        Me.Label13.Location = New System.Drawing.Point(588, 204)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(62, 15)
        Me.Label13.TabIndex = 29
        Me.Label13.Text = "Pin Code :"
        Me.Label13.Visible = False
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btnClose)
        Me.grp_Open.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(646, 407)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(458, 214)
        Me.grp_Open.TabIndex = 30
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "Finding"
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(285, 171)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(71, 25)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'cbo_Open
        '
        Me.cbo_Open.DropDownHeight = 80
        Me.cbo_Open.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(16, 28)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(423, 26)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(367, 171)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(71, 25)
        Me.btnClose.TabIndex = 30
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'LabelHeader
        '
        Me.LabelHeader.AutoEllipsis = True
        Me.LabelHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LabelHeader.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelHeader.Font = New System.Drawing.Font("Cambria", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LabelHeader.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.LabelHeader.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.LabelHeader.Location = New System.Drawing.Point(0, 0)
        Me.LabelHeader.Name = "LabelHeader"
        Me.LabelHeader.Size = New System.Drawing.Size(533, 58)
        Me.LabelHeader.TabIndex = 1
        Me.LabelHeader.Text = "COMPANY CREATION"
        Me.LabelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.LabelHeader.UseMnemonic = False
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Open)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(646, 95)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(503, 299)
        Me.grp_Filter.TabIndex = 31
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "Filter"
        Me.grp_Filter.Visible = False
        '
        'btn_Open
        '
        Me.btn_Open.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Open.Image = CType(resources.GetObject("btn_Open.Image"), System.Drawing.Image)
        Me.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Open.Location = New System.Drawing.Point(331, 267)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(71, 25)
        Me.btn_Open.TabIndex = 33
        Me.btn_Open.Text = "&Open"
        Me.btn_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Open.UseVisualStyleBackColor = True
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.Image = CType(resources.GetObject("btn_CloseFilter.Image"), System.Drawing.Image)
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(413, 267)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(71, 25)
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
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle7
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Company_IdNo, Me.Copany_Name})
        Me.dgv_Filter.Location = New System.Drawing.Point(12, 26)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(472, 235)
        Me.dgv_Filter.TabIndex = 0
        '
        'Company_IdNo
        '
        Me.Company_IdNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Company_IdNo.DefaultCellStyle = DataGridViewCellStyle8
        Me.Company_IdNo.FillWeight = 40.0!
        Me.Company_IdNo.HeaderText = "COMPANY IDNO"
        Me.Company_IdNo.Name = "Company_IdNo"
        Me.Company_IdNo.ReadOnly = True
        Me.Company_IdNo.Width = 111
        '
        'Copany_Name
        '
        Me.Copany_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Copany_Name.DefaultCellStyle = DataGridViewCellStyle9
        Me.Copany_Name.FillWeight = 160.0!
        Me.Copany_Name.HeaderText = "COMPANY NAME"
        Me.Copany_Name.Name = "Copany_Name"
        Me.Copany_Name.ReadOnly = True
        Me.Copany_Name.Width = 115
        '
        'lbl_CompID
        '
        Me.lbl_CompID.BackColor = System.Drawing.Color.White
        Me.lbl_CompID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_CompID.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CompID.Location = New System.Drawing.Point(143, 65)
        Me.lbl_CompID.Name = "lbl_CompID"
        Me.lbl_CompID.Size = New System.Drawing.Size(384, 23)
        Me.lbl_CompID.TabIndex = 32
        Me.lbl_CompID.Text = "lbl_CompID"
        Me.lbl_CompID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(293, 389)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(44, 15)
        Me.Label5.TabIndex = 34
        Me.Label5.Text = "ESI No "
        '
        'txt_ESINo
        '
        Me.txt_ESINo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_ESINo.Location = New System.Drawing.Point(347, 384)
        Me.txt_ESINo.MaxLength = 50
        Me.txt_ESINo.Name = "txt_ESINo"
        Me.txt_ESINo.Size = New System.Drawing.Size(180, 23)
        Me.txt_ESINo.TabIndex = 13
        Me.txt_ESINo.Text = "txt_ESINo"
        '
        'cbo_CompanyType
        '
        Me.cbo_CompanyType.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_CompanyType.FormattingEnabled = True
        Me.cbo_CompanyType.Items.AddRange(New Object() {"", "ACCOUNT", "UNACCOUNT"})
        Me.cbo_CompanyType.Location = New System.Drawing.Point(384, 123)
        Me.cbo_CompanyType.Name = "cbo_CompanyType"
        Me.cbo_CompanyType.Size = New System.Drawing.Size(143, 23)
        Me.cbo_CompanyType.TabIndex = 2
        Me.cbo_CompanyType.Text = "cbo_CompanyType"
        '
        'lbl_CompanyType
        '
        Me.lbl_CompanyType.AutoSize = True
        Me.lbl_CompanyType.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CompanyType.ForeColor = System.Drawing.Color.Navy
        Me.lbl_CompanyType.Location = New System.Drawing.Point(284, 128)
        Me.lbl_CompanyType.Name = "lbl_CompanyType"
        Me.lbl_CompanyType.Size = New System.Drawing.Size(87, 15)
        Me.lbl_CompanyType.TabIndex = 36
        Me.lbl_CompanyType.Text = "Company Type"
        '
        'txt_Bank_Ac_Details
        '
        Me.txt_Bank_Ac_Details.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Bank_Ac_Details.Location = New System.Drawing.Point(143, 472)
        Me.txt_Bank_Ac_Details.MaxLength = 200
        Me.txt_Bank_Ac_Details.Name = "txt_Bank_Ac_Details"
        Me.txt_Bank_Ac_Details.Size = New System.Drawing.Size(384, 23)
        Me.txt_Bank_Ac_Details.TabIndex = 17
        Me.txt_Bank_Ac_Details.Text = "txt_Bank_Ac_Details"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(6, 475)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 15)
        Me.Label1.TabIndex = 38
        Me.Label1.Text = "Bank A/c Details"
        '
        'txt_PanNo
        '
        Me.txt_PanNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PanNo.Location = New System.Drawing.Point(143, 413)
        Me.txt_PanNo.MaxLength = 200
        Me.txt_PanNo.Name = "txt_PanNo"
        Me.txt_PanNo.Size = New System.Drawing.Size(384, 23)
        Me.txt_PanNo.TabIndex = 14
        Me.txt_PanNo.Text = "txt_PanNo"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Navy
        Me.Label10.Location = New System.Drawing.Point(6, 416)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 15)
        Me.Label10.TabIndex = 40
        Me.Label10.Text = "Pan No"
        '
        'cbo_Company_Designation
        '
        Me.cbo_Company_Designation.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Company_Designation.FormattingEnabled = True
        Me.cbo_Company_Designation.Items.AddRange(New Object() {"", "PARTNER", "PROPRIETOR"})
        Me.cbo_Company_Designation.Location = New System.Drawing.Point(384, 152)
        Me.cbo_Company_Designation.MaxLength = 50
        Me.cbo_Company_Designation.Name = "cbo_Company_Designation"
        Me.cbo_Company_Designation.Size = New System.Drawing.Size(143, 23)
        Me.cbo_Company_Designation.TabIndex = 4
        Me.cbo_Company_Designation.Text = "cbo_Company_Designation"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.Navy
        Me.Label19.Location = New System.Drawing.Point(284, 158)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(71, 15)
        Me.Label19.TabIndex = 285
        Me.Label19.Text = "Designation"
        '
        'txt_Website
        '
        Me.txt_Website.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Website.Location = New System.Drawing.Point(347, 443)
        Me.txt_Website.MaxLength = 50
        Me.txt_Website.Name = "txt_Website"
        Me.txt_Website.Size = New System.Drawing.Size(180, 23)
        Me.txt_Website.TabIndex = 16
        Me.txt_Website.Text = "txt_Website"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.Navy
        Me.Label20.Location = New System.Drawing.Point(290, 448)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(53, 15)
        Me.Label20.TabIndex = 287
        Me.Label20.Text = "Website"
        '
        'cbo_State
        '
        Me.cbo_State.DropDownHeight = 99
        Me.cbo_State.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_State.FormattingEnabled = True
        Me.cbo_State.IntegralHeight = False
        Me.cbo_State.Location = New System.Drawing.Point(143, 297)
        Me.cbo_State.MaxLength = 50
        Me.cbo_State.Name = "cbo_State"
        Me.cbo_State.Size = New System.Drawing.Size(384, 23)
        Me.cbo_State.TabIndex = 9
        Me.cbo_State.Text = "cbo_State"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.Navy
        Me.Label21.Location = New System.Drawing.Point(6, 300)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(36, 15)
        Me.Label21.TabIndex = 289
        Me.Label21.Text = "State"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Navy
        Me.Label14.Location = New System.Drawing.Point(6, 126)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(127, 15)
        Me.Label14.TabIndex = 290
        Me.Label14.Text = "Company ShortName "
        '
        'txt_GSTIN_No
        '
        Me.txt_GSTIN_No.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_GSTIN_No.Location = New System.Drawing.Point(143, 355)
        Me.txt_GSTIN_No.MaxLength = 50
        Me.txt_GSTIN_No.Name = "txt_GSTIN_No"
        Me.txt_GSTIN_No.Size = New System.Drawing.Size(384, 23)
        Me.txt_GSTIN_No.TabIndex = 11
        Me.txt_GSTIN_No.Text = "txt_GSTIN_No"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.Navy
        Me.Label22.Location = New System.Drawing.Point(6, 358)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(61, 15)
        Me.Label22.TabIndex = 292
        Me.Label22.Text = "GSTIN No "
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(465, 559)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(64, 27)
        Me.btn_Close.TabIndex = 20
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(402, 559)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(57, 27)
        Me.btn_Save.TabIndex = 19
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'txt_GSTP_Email_ID
        '
        Me.txt_GSTP_Email_ID.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_GSTP_Email_ID.Location = New System.Drawing.Point(143, 530)
        Me.txt_GSTP_Email_ID.MaxLength = 200
        Me.txt_GSTP_Email_ID.Name = "txt_GSTP_Email_ID"
        Me.txt_GSTP_Email_ID.Size = New System.Drawing.Size(384, 23)
        Me.txt_GSTP_Email_ID.TabIndex = 19
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Navy
        Me.Label7.Location = New System.Drawing.Point(6, 533)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(108, 15)
        Me.Label7.TabIndex = 294
        Me.Label7.Text = "GSTP / CA Email-ID"
        '
        'Company_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(533, 584)
        Me.Controls.Add(Me.txt_GSTP_Email_ID)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btn_Close)
        Me.Controls.Add(Me.btn_Save)
        Me.Controls.Add(Me.txt_GSTIN_No)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.cbo_State)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.txt_Website)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.cbo_Company_Designation)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.txt_PanNo)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txt_Bank_Ac_Details)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbl_CompanyType)
        Me.Controls.Add(Me.cbo_CompanyType)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.txt_ESINo)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lbl_CompID)
        Me.Controls.Add(Me.txt_PinCode)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txt_Description)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txt_EMail)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txt_ShortName)
        Me.Controls.Add(Me.txt_FaxNo)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txt_PhoneNo)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txt_City)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txt_Address4)
        Me.Controls.Add(Me.txt_Address3)
        Me.Controls.Add(Me.txt_Address2)
        Me.Controls.Add(Me.txt_Address1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txt_ContactName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txt_CompanyName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lbl_CompIDCaption)
        Me.Controls.Add(Me.LabelHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Company_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "COMPANY CREATION"
        Me.grp_Open.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelHeader As System.Windows.Forms.Label
    Friend WithEvents lbl_CompIDCaption As System.Windows.Forms.Label
    Friend WithEvents txt_CompanyName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_Address1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_ContactName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txt_Address2 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address3 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address4 As System.Windows.Forms.TextBox
    Friend WithEvents txt_City As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txt_ShortName As System.Windows.Forms.TextBox
    Friend WithEvents txt_FaxNo As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_PhoneNo As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txt_Description As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_EMail As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txt_PinCode As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents lbl_CompID As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txt_ESINo As System.Windows.Forms.TextBox
    Friend WithEvents Company_IdNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Copany_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbo_CompanyType As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_CompanyType As System.Windows.Forms.Label
    Friend WithEvents txt_Bank_Ac_Details As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_PanNo As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbo_Company_Designation As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txt_Website As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents cbo_State As System.Windows.Forms.ComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txt_GSTIN_No As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents txt_GSTP_Email_ID As TextBox
    Friend WithEvents Label7 As Label
End Class
