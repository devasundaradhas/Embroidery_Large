<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Cheque_Print_Positioning
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
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.label = New System.Windows.Forms.Label()
        Me.pnl_Filter = New System.Windows.Forms.Panel()
        Me.txt_filter_billNo = New System.Windows.Forms.TextBox()
        Me.btn_Filter_Close = New System.Windows.Forms.Button()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.cbo_Filter_DelvAt = New System.Windows.Forms.ComboBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.btn_Filter_Show = New System.Windows.Forms.Button()
        Me.dgv_Filter_Details = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column17 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column19 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cbo_Filter_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.dtp_Filter_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.dtp_Filter_Fromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbl_ChqNo = New System.Windows.Forms.Label()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.cbo_BankName = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txt_TopMargin = New System.Windows.Forms.TextBox()
        Me.txt_AccountNo = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Cbo_PaperOrientation = New System.Windows.Forms.ComboBox()
        Me.cbo_Partner = New System.Windows.Forms.ComboBox()
        Me.txt_LeftMargin = New System.Windows.Forms.TextBox()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.dgv_BackDetails = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.txt_second_PartyName_Left = New System.Windows.Forms.TextBox()
        Me.txt_PartyName_Left = New System.Windows.Forms.TextBox()
        Me.txt_Account_No = New System.Windows.Forms.TextBox()
        Me.txt_AcPayee_Top = New System.Windows.Forms.TextBox()
        Me.txt_Date = New System.Windows.Forms.TextBox()
        Me.txt_Partner = New System.Windows.Forms.TextBox()
        Me.txt_PartyName = New System.Windows.Forms.TextBox()
        Me.txt_Company_Name = New System.Windows.Forms.TextBox()
        Me.txt_Amount_Words = New System.Windows.Forms.TextBox()
        Me.txt_Second_PartyName = New System.Windows.Forms.TextBox()
        Me.txt_Second_Amount_Words = New System.Windows.Forms.TextBox()
        Me.txt_ACPayee = New System.Windows.Forms.TextBox()
        Me.lbl_SizingQty1 = New System.Windows.Forms.Label()
        Me.lbl_SizingQty2 = New System.Windows.Forms.Label()
        Me.lbl_SizingQty3 = New System.Windows.Forms.Label()
        Me.lbl_VatGross1 = New System.Windows.Forms.Label()
        Me.lbl_VatGross2 = New System.Windows.Forms.Label()
        Me.txt_AcPayee_Left = New System.Windows.Forms.TextBox()
        Me.txt_Date_Left = New System.Windows.Forms.TextBox()
        Me.txt_Date_width = New System.Windows.Forms.TextBox()
        Me.txt_PartyName_Top = New System.Windows.Forms.TextBox()
        Me.txt_AmountWords_Left = New System.Windows.Forms.TextBox()
        Me.txt_Second_AmountWords_Left = New System.Windows.Forms.TextBox()
        Me.txt_Rs_Left = New System.Windows.Forms.TextBox()
        Me.txt_CompanyName_Top = New System.Windows.Forms.TextBox()
        Me.txt_CompanyName_Left = New System.Windows.Forms.TextBox()
        Me.lbl_SizingAmount1 = New System.Windows.Forms.Label()
        Me.lbl_SizingAmount2 = New System.Windows.Forms.Label()
        Me.txt_Rs = New System.Windows.Forms.TextBox()
        Me.lbl_SizingAmount3 = New System.Windows.Forms.Label()
        Me.lbl_WeldingAmount = New System.Windows.Forms.Label()
        Me.lbl_RewindingAmount = New System.Windows.Forms.Label()
        Me.txt_Second_PartyName_Top = New System.Windows.Forms.TextBox()
        Me.txt_Second_PartyName_Width = New System.Windows.Forms.TextBox()
        Me.txt_AmountWords_Top = New System.Windows.Forms.TextBox()
        Me.txt_AmountWords_Width = New System.Windows.Forms.TextBox()
        Me.txt_Second_AmountWords_Top = New System.Windows.Forms.TextBox()
        Me.txt_Second_AmountWords_Width = New System.Windows.Forms.TextBox()
        Me.txt_Rs_Top = New System.Windows.Forms.TextBox()
        Me.txt_Rs_Width = New System.Windows.Forms.TextBox()
        Me.txt_Date_Top = New System.Windows.Forms.TextBox()
        Me.txt_CompanyName_Width = New System.Windows.Forms.TextBox()
        Me.txt_Partner_Width = New System.Windows.Forms.TextBox()
        Me.txt_AccountNo_Width = New System.Windows.Forms.TextBox()
        Me.txt_AccountNo_Left = New System.Windows.Forms.TextBox()
        Me.txt_Partner_Top = New System.Windows.Forms.TextBox()
        Me.txt_AccountNo_Top = New System.Windows.Forms.TextBox()
        Me.txt_Partner_Left = New System.Windows.Forms.TextBox()
        Me.txt_AcPayeeWidth = New System.Windows.Forms.TextBox()
        Me.txt_PartyName_width = New System.Windows.Forms.TextBox()
        Me.grp_find = New System.Windows.Forms.GroupBox()
        Me.btn_FindOpen = New System.Windows.Forms.Button()
        Me.btn_FindClose = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.lbl_UserName = New System.Windows.Forms.Label()
        Me.pnl_Filter.SuspendLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        CType(Me.dgv_BackDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.grp_find.SuspendLayout()
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
        Me.Label1.Size = New System.Drawing.Size(697, 35)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "CHEQUE PRINT POSITINING"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'label
        '
        Me.label.AutoSize = True
        Me.label.ForeColor = System.Drawing.Color.Blue
        Me.label.Location = New System.Drawing.Point(532, 73)
        Me.label.Name = "label"
        Me.label.Size = New System.Drawing.Size(42, 15)
        Me.label.TabIndex = 42
        Me.label.Text = "Bill.No"
        '
        'pnl_Filter
        '
        Me.pnl_Filter.BackColor = System.Drawing.Color.White
        Me.pnl_Filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Filter.Controls.Add(Me.txt_filter_billNo)
        Me.pnl_Filter.Controls.Add(Me.label)
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Close)
        Me.pnl_Filter.Controls.Add(Me.Label29)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_DelvAt)
        Me.pnl_Filter.Controls.Add(Me.Label33)
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Show)
        Me.pnl_Filter.Controls.Add(Me.dgv_Filter_Details)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_PartyName)
        Me.pnl_Filter.Controls.Add(Me.Label32)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_ToDate)
        Me.pnl_Filter.Controls.Add(Me.Label31)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_Fromdate)
        Me.pnl_Filter.Controls.Add(Me.Label30)
        Me.pnl_Filter.Controls.Add(Me.Label34)
        Me.pnl_Filter.Location = New System.Drawing.Point(745, 136)
        Me.pnl_Filter.Margin = New System.Windows.Forms.Padding(0)
        Me.pnl_Filter.Name = "pnl_Filter"
        Me.pnl_Filter.Size = New System.Drawing.Size(933, 416)
        Me.pnl_Filter.TabIndex = 25
        Me.pnl_Filter.Visible = False
        '
        'txt_filter_billNo
        '
        Me.txt_filter_billNo.Location = New System.Drawing.Point(596, 68)
        Me.txt_filter_billNo.Name = "txt_filter_billNo"
        Me.txt_filter_billNo.Size = New System.Drawing.Size(212, 23)
        Me.txt_filter_billNo.TabIndex = 2
        Me.txt_filter_billNo.Text = "txt_BillNo"
        '
        'btn_Filter_Close
        '
        Me.btn_Filter_Close.BackColor = System.Drawing.Color.White
        Me.btn_Filter_Close.BackgroundImage = Global.Billing.My.Resources.Resources.Close1
        Me.btn_Filter_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Filter_Close.FlatAppearance.BorderSize = 0
        Me.btn_Filter_Close.Location = New System.Drawing.Point(891, -1)
        Me.btn_Filter_Close.Name = "btn_Filter_Close"
        Me.btn_Filter_Close.Size = New System.Drawing.Size(41, 40)
        Me.btn_Filter_Close.TabIndex = 6
        Me.btn_Filter_Close.UseVisualStyleBackColor = True
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.Purple
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.White
        Me.Label29.Location = New System.Drawing.Point(868, -73)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(71, 20)
        Me.Label29.TabIndex = 37
        Me.Label29.Text = "FILTER"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbo_Filter_DelvAt
        '
        Me.cbo_Filter_DelvAt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Filter_DelvAt.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Filter_DelvAt.FormattingEnabled = True
        Me.cbo_Filter_DelvAt.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.cbo_Filter_DelvAt.Location = New System.Drawing.Point(596, 118)
        Me.cbo_Filter_DelvAt.MaxDropDownItems = 15
        Me.cbo_Filter_DelvAt.Name = "cbo_Filter_DelvAt"
        Me.cbo_Filter_DelvAt.Size = New System.Drawing.Size(212, 23)
        Me.cbo_Filter_DelvAt.Sorted = True
        Me.cbo_Filter_DelvAt.TabIndex = 4
        Me.cbo_Filter_DelvAt.Text = "cbo_Filter_DelvAt"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.Color.Blue
        Me.Label33.Location = New System.Drawing.Point(532, 122)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(46, 15)
        Me.Label33.TabIndex = 34
        Me.Label33.Text = "Delv.At"
        '
        'btn_Filter_Show
        '
        Me.btn_Filter_Show.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter_Show.ForeColor = System.Drawing.Color.Blue
        Me.btn_Filter_Show.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter_Show.Location = New System.Drawing.Point(826, 68)
        Me.btn_Filter_Show.Name = "btn_Filter_Show"
        Me.btn_Filter_Show.Size = New System.Drawing.Size(96, 80)
        Me.btn_Filter_Show.TabIndex = 5
        Me.btn_Filter_Show.Text = "&SHOW"
        Me.btn_Filter_Show.UseVisualStyleBackColor = False
        '
        'dgv_Filter_Details
        '
        Me.dgv_Filter_Details.AllowUserToAddRows = False
        Me.dgv_Filter_Details.AllowUserToDeleteRows = False
        Me.dgv_Filter_Details.AllowUserToResizeColumns = False
        Me.dgv_Filter_Details.AllowUserToResizeRows = False
        Me.dgv_Filter_Details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle8
        Me.dgv_Filter_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Filter_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.Column17, Me.Column19, Me.Column5})
        Me.dgv_Filter_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter_Details.Location = New System.Drawing.Point(-1, 166)
        Me.dgv_Filter_Details.MultiSelect = False
        Me.dgv_Filter_Details.Name = "dgv_Filter_Details"
        Me.dgv_Filter_Details.ReadOnly = True
        Me.dgv_Filter_Details.RowHeadersVisible = False
        Me.dgv_Filter_Details.RowHeadersWidth = 15
        Me.dgv_Filter_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Filter_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter_Details.Size = New System.Drawing.Size(923, 254)
        Me.dgv_Filter_Details.TabIndex = 32
        Me.dgv_Filter_Details.TabStop = False
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn1.Frozen = True
        Me.DataGridViewTextBoxColumn1.HeaderText = "REF.NO"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 70
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle10
        Me.DataGridViewTextBoxColumn2.HeaderText = "DATE"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewTextBoxColumn3.HeaderText = "PARTYNAME"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 250
        '
        'Column17
        '
        Me.Column17.HeaderText = "BILL NO"
        Me.Column17.Name = "Column17"
        Me.Column17.ReadOnly = True
        '
        'Column19
        '
        Me.Column19.HeaderText = "DELV.AT"
        Me.Column19.Name = "Column19"
        Me.Column19.ReadOnly = True
        Me.Column19.Width = 120
        '
        'Column5
        '
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle12
        Me.Column5.HeaderText = "BILL AMOUNT"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 120
        '
        'cbo_Filter_PartyName
        '
        Me.cbo_Filter_PartyName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Filter_PartyName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Filter_PartyName.FormattingEnabled = True
        Me.cbo_Filter_PartyName.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.cbo_Filter_PartyName.Location = New System.Drawing.Point(127, 118)
        Me.cbo_Filter_PartyName.MaxDropDownItems = 15
        Me.cbo_Filter_PartyName.Name = "cbo_Filter_PartyName"
        Me.cbo_Filter_PartyName.Size = New System.Drawing.Size(381, 23)
        Me.cbo_Filter_PartyName.Sorted = True
        Me.cbo_Filter_PartyName.TabIndex = 3
        Me.cbo_Filter_PartyName.Text = "cbo_Filter_PartyName"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ForeColor = System.Drawing.Color.Blue
        Me.Label32.Location = New System.Drawing.Point(27, 122)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(72, 15)
        Me.Label32.TabIndex = 30
        Me.Label32.Text = "Party Name"
        '
        'dtp_Filter_ToDate
        '
        Me.dtp_Filter_ToDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Filter_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_ToDate.Location = New System.Drawing.Point(363, 68)
        Me.dtp_Filter_ToDate.Name = "dtp_Filter_ToDate"
        Me.dtp_Filter_ToDate.Size = New System.Drawing.Size(145, 23)
        Me.dtp_Filter_ToDate.TabIndex = 1
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.ForeColor = System.Drawing.Color.Blue
        Me.Label31.Location = New System.Drawing.Point(317, 73)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(19, 15)
        Me.Label31.TabIndex = 29
        Me.Label31.Text = "To"
        '
        'dtp_Filter_Fromdate
        '
        Me.dtp_Filter_Fromdate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Filter_Fromdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_Fromdate.Location = New System.Drawing.Point(127, 68)
        Me.dtp_Filter_Fromdate.Name = "dtp_Filter_Fromdate"
        Me.dtp_Filter_Fromdate.Size = New System.Drawing.Size(170, 23)
        Me.dtp_Filter_Fromdate.TabIndex = 0
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.ForeColor = System.Drawing.Color.Blue
        Me.Label30.Location = New System.Drawing.Point(27, 73)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(54, 15)
        Me.Label30.TabIndex = 27
        Me.Label30.Text = "Ref Date"
        '
        'Label34
        '
        Me.Label34.BackColor = System.Drawing.Color.Indigo
        Me.Label34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label34.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label34.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.Color.White
        Me.Label34.Location = New System.Drawing.Point(0, 0)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(931, 39)
        Me.Label34.TabIndex = 41
        Me.Label34.Text = "FILTER"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(16, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Chq.No"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(16, 46)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 15)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Bank Name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(1122, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 3
        '
        'lbl_ChqNo
        '
        Me.lbl_ChqNo.BackColor = System.Drawing.Color.White
        Me.lbl_ChqNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_ChqNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ChqNo.ForeColor = System.Drawing.Color.Black
        Me.lbl_ChqNo.Location = New System.Drawing.Point(97, 6)
        Me.lbl_ChqNo.Name = "lbl_ChqNo"
        Me.lbl_ChqNo.Size = New System.Drawing.Size(548, 23)
        Me.lbl_ChqNo.TabIndex = 0
        Me.lbl_ChqNo.Text = "lbl_ChqNo"
        Me.lbl_ChqNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(474, 478)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(82, 35)
        Me.btn_save.TabIndex = 17
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(562, 478)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(82, 35)
        Me.btn_close.TabIndex = 20
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'cbo_BankName
        '
        Me.cbo_BankName.FormattingEnabled = True
        Me.cbo_BankName.Location = New System.Drawing.Point(97, 42)
        Me.cbo_BankName.Name = "cbo_BankName"
        Me.cbo_BankName.Size = New System.Drawing.Size(548, 23)
        Me.cbo_BankName.TabIndex = 0
        Me.cbo_BankName.Text = "cbo_BankName"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.Color.Blue
        Me.Label14.Location = New System.Drawing.Point(16, 82)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(49, 15)
        Me.Label14.TabIndex = 68
        Me.Label14.Text = "Partner"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Blue
        Me.Label16.Location = New System.Drawing.Point(340, 82)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(106, 15)
        Me.Label16.TabIndex = 70
        Me.Label16.Text = "Paper Orientation"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.Blue
        Me.Label17.Location = New System.Drawing.Point(16, 118)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(69, 15)
        Me.Label17.TabIndex = 71
        Me.Label17.Text = "Left Margin"
        '
        'txt_TopMargin
        '
        Me.txt_TopMargin.Location = New System.Drawing.Point(443, 114)
        Me.txt_TopMargin.MaxLength = 20
        Me.txt_TopMargin.Name = "txt_TopMargin"
        Me.txt_TopMargin.Size = New System.Drawing.Size(202, 23)
        Me.txt_TopMargin.TabIndex = 4
        Me.txt_TopMargin.Text = "txt_TopMargin"
        '
        'txt_AccountNo
        '
        Me.txt_AccountNo.Location = New System.Drawing.Point(97, 150)
        Me.txt_AccountNo.MaxLength = 20
        Me.txt_AccountNo.Name = "txt_AccountNo"
        Me.txt_AccountNo.Size = New System.Drawing.Size(548, 23)
        Me.txt_AccountNo.TabIndex = 5
        Me.txt_AccountNo.Text = "txt_AccountNo"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(16, 154)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 15)
        Me.Label9.TabIndex = 82
        Me.Label9.Text = "Account No"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(340, 118)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(67, 15)
        Me.Label11.TabIndex = 83
        Me.Label11.Text = "Top Margin"
        '
        'Cbo_PaperOrientation
        '
        Me.Cbo_PaperOrientation.FormattingEnabled = True
        Me.Cbo_PaperOrientation.Location = New System.Drawing.Point(443, 78)
        Me.Cbo_PaperOrientation.Name = "Cbo_PaperOrientation"
        Me.Cbo_PaperOrientation.Size = New System.Drawing.Size(202, 23)
        Me.Cbo_PaperOrientation.TabIndex = 2
        Me.Cbo_PaperOrientation.Text = "cbo_PaperOrientation"
        '
        'cbo_Partner
        '
        Me.cbo_Partner.FormattingEnabled = True
        Me.cbo_Partner.Location = New System.Drawing.Point(97, 78)
        Me.cbo_Partner.Name = "cbo_Partner"
        Me.cbo_Partner.Size = New System.Drawing.Size(216, 23)
        Me.cbo_Partner.TabIndex = 1
        Me.cbo_Partner.Text = "cbo_Partner"
        '
        'txt_LeftMargin
        '
        Me.txt_LeftMargin.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_LeftMargin.Location = New System.Drawing.Point(97, 114)
        Me.txt_LeftMargin.MaxLength = 35
        Me.txt_LeftMargin.Name = "txt_LeftMargin"
        Me.txt_LeftMargin.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_LeftMargin.Size = New System.Drawing.Size(216, 23)
        Me.txt_LeftMargin.TabIndex = 3
        Me.txt_LeftMargin.Text = "txt_LeftMargin"
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.LightSkyBlue
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.dgv_BackDetails)
        Me.pnl_Back.Controls.Add(Me.TableLayoutPanel1)
        Me.pnl_Back.Controls.Add(Me.txt_LeftMargin)
        Me.pnl_Back.Controls.Add(Me.cbo_Partner)
        Me.pnl_Back.Controls.Add(Me.Cbo_PaperOrientation)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Controls.Add(Me.Label9)
        Me.pnl_Back.Controls.Add(Me.txt_AccountNo)
        Me.pnl_Back.Controls.Add(Me.txt_TopMargin)
        Me.pnl_Back.Controls.Add(Me.Label17)
        Me.pnl_Back.Controls.Add(Me.Label16)
        Me.pnl_Back.Controls.Add(Me.Label14)
        Me.pnl_Back.Controls.Add(Me.cbo_BankName)
        Me.pnl_Back.Controls.Add(Me.btn_close)
        Me.pnl_Back.Controls.Add(Me.btn_save)
        Me.pnl_Back.Controls.Add(Me.lbl_ChqNo)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Enabled = False
        Me.pnl_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_Back.ForeColor = System.Drawing.Color.Blue
        Me.pnl_Back.Location = New System.Drawing.Point(14, 38)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(667, 535)
        Me.pnl_Back.TabIndex = 24
        '
        'dgv_BackDetails
        '
        Me.dgv_BackDetails.AllowUserToAddRows = False
        Me.dgv_BackDetails.AllowUserToDeleteRows = False
        Me.dgv_BackDetails.AllowUserToResizeColumns = False
        Me.dgv_BackDetails.AllowUserToResizeRows = False
        Me.dgv_BackDetails.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.MediumVioletRed
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_BackDetails.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle13
        Me.dgv_BackDetails.ColumnHeadersHeight = 30
        Me.dgv_BackDetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.DataGridViewTextBoxColumn4})
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle14.BackColor = System.Drawing.Color.DeepPink
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_BackDetails.DefaultCellStyle = DataGridViewCellStyle14
        Me.dgv_BackDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_BackDetails.Enabled = False
        Me.dgv_BackDetails.Location = New System.Drawing.Point(19, 190)
        Me.dgv_BackDetails.Name = "dgv_BackDetails"
        Me.dgv_BackDetails.RowHeadersVisible = False
        Me.dgv_BackDetails.RowTemplate.Height = 25
        Me.dgv_BackDetails.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgv_BackDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_BackDetails.Size = New System.Drawing.Size(626, 30)
        Me.dgv_BackDetails.TabIndex = 84
        '
        'Column1
        '
        Me.Column1.HeaderText = "S.NO"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 50
        '
        'Column2
        '
        Me.Column2.HeaderText = "DESCRIPTION"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 230
        '
        'Column3
        '
        Me.Column3.HeaderText = "LEFT (cm)"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 120
        '
        'Column4
        '
        Me.Column4.HeaderText = "TOP (cm)"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 120
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "WIDTH (chr)"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 130
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.BackColor = System.Drawing.Color.White
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 354.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.txt_second_PartyName_Left, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_PartyName_Left, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Account_No, 1, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AcPayee_Top, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Date, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Partner, 1, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_PartyName, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Company_Name, 1, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Amount_Words, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Second_PartyName, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Second_Amount_Words, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_ACPayee, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_SizingQty1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_SizingQty2, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_SizingQty3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_VatGross1, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_VatGross2, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AcPayee_Left, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Date_Left, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Date_width, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_PartyName_Top, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AmountWords_Left, 2, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Second_AmountWords_Left, 2, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Rs_Left, 2, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_CompanyName_Top, 3, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_CompanyName_Left, 2, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_SizingAmount1, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_SizingAmount2, 0, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Rs, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_SizingAmount3, 0, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_WeldingAmount, 0, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_RewindingAmount, 0, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Second_PartyName_Top, 3, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Second_PartyName_Width, 4, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AmountWords_Top, 3, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AmountWords_Width, 4, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Second_AmountWords_Top, 3, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Second_AmountWords_Width, 4, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Rs_Top, 3, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Rs_Width, 4, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Date_Top, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_CompanyName_Width, 4, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Partner_Width, 4, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AccountNo_Width, 4, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AccountNo_Left, 2, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Partner_Top, 3, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AccountNo_Top, 3, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_Partner_Left, 2, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_AcPayeeWidth, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.txt_PartyName_width, 4, 2)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(19, 220)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 12
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(626, 243)
        Me.TableLayoutPanel1.TabIndex = 85
        '
        'txt_second_PartyName_Left
        '
        Me.txt_second_PartyName_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_second_PartyName_Left.Location = New System.Drawing.Point(285, 76)
        Me.txt_second_PartyName_Left.MaxLength = 20
        Me.txt_second_PartyName_Left.Name = "txt_second_PartyName_Left"
        Me.txt_second_PartyName_Left.Size = New System.Drawing.Size(112, 16)
        Me.txt_second_PartyName_Left.TabIndex = 19
        Me.txt_second_PartyName_Left.Text = "txt_partyName_Left"
        Me.txt_second_PartyName_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_PartyName_Left
        '
        Me.txt_PartyName_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_PartyName_Left.Location = New System.Drawing.Point(285, 52)
        Me.txt_PartyName_Left.MaxLength = 20
        Me.txt_PartyName_Left.Name = "txt_PartyName_Left"
        Me.txt_PartyName_Left.Size = New System.Drawing.Size(112, 16)
        Me.txt_PartyName_Left.TabIndex = 15
        Me.txt_PartyName_Left.Text = "txt_PartyName_Left"
        Me.txt_PartyName_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Account_No
        '
        Me.txt_Account_No.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Account_No.Location = New System.Drawing.Point(54, 220)
        Me.txt_Account_No.MaxLength = 35
        Me.txt_Account_No.Name = "txt_Account_No"
        Me.txt_Account_No.Size = New System.Drawing.Size(224, 16)
        Me.txt_Account_No.TabIndex = 42
        Me.txt_Account_No.TabStop = False
        Me.txt_Account_No.Text = "ACCOUNT NO"
        '
        'txt_AcPayee_Top
        '
        Me.txt_AcPayee_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AcPayee_Top.Location = New System.Drawing.Point(405, 4)
        Me.txt_AcPayee_Top.MaxLength = 20
        Me.txt_AcPayee_Top.Name = "txt_AcPayee_Top"
        Me.txt_AcPayee_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_AcPayee_Top.TabIndex = 8
        Me.txt_AcPayee_Top.Text = "txt_AcPayee_Top"
        Me.txt_AcPayee_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Date
        '
        Me.txt_Date.BackColor = System.Drawing.Color.White
        Me.txt_Date.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Date.Location = New System.Drawing.Point(54, 28)
        Me.txt_Date.MaxLength = 50
        Me.txt_Date.Name = "txt_Date"
        Me.txt_Date.Size = New System.Drawing.Size(224, 16)
        Me.txt_Date.TabIndex = 10
        Me.txt_Date.TabStop = False
        Me.txt_Date.Text = "DATE"
        '
        'txt_Partner
        '
        Me.txt_Partner.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Partner.Location = New System.Drawing.Point(54, 196)
        Me.txt_Partner.MaxLength = 35
        Me.txt_Partner.Name = "txt_Partner"
        Me.txt_Partner.Size = New System.Drawing.Size(224, 16)
        Me.txt_Partner.TabIndex = 38
        Me.txt_Partner.TabStop = False
        Me.txt_Partner.Text = "PARTNER"
        '
        'txt_PartyName
        '
        Me.txt_PartyName.BackColor = System.Drawing.Color.White
        Me.txt_PartyName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_PartyName.Location = New System.Drawing.Point(54, 52)
        Me.txt_PartyName.MaxLength = 50
        Me.txt_PartyName.Name = "txt_PartyName"
        Me.txt_PartyName.Size = New System.Drawing.Size(224, 16)
        Me.txt_PartyName.TabIndex = 14
        Me.txt_PartyName.TabStop = False
        Me.txt_PartyName.Text = "PARTY NAME"
        '
        'txt_Company_Name
        '
        Me.txt_Company_Name.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Company_Name.Location = New System.Drawing.Point(54, 172)
        Me.txt_Company_Name.MaxLength = 35
        Me.txt_Company_Name.Name = "txt_Company_Name"
        Me.txt_Company_Name.Size = New System.Drawing.Size(224, 16)
        Me.txt_Company_Name.TabIndex = 34
        Me.txt_Company_Name.TabStop = False
        Me.txt_Company_Name.Text = "FOR COMPANY NAME"
        '
        'txt_Amount_Words
        '
        Me.txt_Amount_Words.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Amount_Words.Location = New System.Drawing.Point(54, 100)
        Me.txt_Amount_Words.MaxLength = 35
        Me.txt_Amount_Words.Name = "txt_Amount_Words"
        Me.txt_Amount_Words.Size = New System.Drawing.Size(224, 16)
        Me.txt_Amount_Words.TabIndex = 22
        Me.txt_Amount_Words.TabStop = False
        Me.txt_Amount_Words.Text = "AMOUNT IN WORDS"
        '
        'txt_Second_PartyName
        '
        Me.txt_Second_PartyName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Second_PartyName.Location = New System.Drawing.Point(54, 76)
        Me.txt_Second_PartyName.Name = "txt_Second_PartyName"
        Me.txt_Second_PartyName.Size = New System.Drawing.Size(224, 16)
        Me.txt_Second_PartyName.TabIndex = 18
        Me.txt_Second_PartyName.TabStop = False
        Me.txt_Second_PartyName.Text = "2nd LINE"
        '
        'txt_Second_Amount_Words
        '
        Me.txt_Second_Amount_Words.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Second_Amount_Words.Location = New System.Drawing.Point(54, 124)
        Me.txt_Second_Amount_Words.MaxLength = 35
        Me.txt_Second_Amount_Words.Name = "txt_Second_Amount_Words"
        Me.txt_Second_Amount_Words.Size = New System.Drawing.Size(224, 16)
        Me.txt_Second_Amount_Words.TabIndex = 26
        Me.txt_Second_Amount_Words.TabStop = False
        Me.txt_Second_Amount_Words.Text = "2nd LINE"
        '
        'txt_ACPayee
        '
        Me.txt_ACPayee.BackColor = System.Drawing.Color.White
        Me.txt_ACPayee.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_ACPayee.Location = New System.Drawing.Point(54, 4)
        Me.txt_ACPayee.MaxLength = 50
        Me.txt_ACPayee.Name = "txt_ACPayee"
        Me.txt_ACPayee.Size = New System.Drawing.Size(224, 16)
        Me.txt_ACPayee.TabIndex = 6
        Me.txt_ACPayee.TabStop = False
        Me.txt_ACPayee.Text = "A/C PAYEE"
        '
        'lbl_SizingQty1
        '
        Me.lbl_SizingQty1.BackColor = System.Drawing.Color.White
        Me.lbl_SizingQty1.Location = New System.Drawing.Point(4, 25)
        Me.lbl_SizingQty1.Name = "lbl_SizingQty1"
        Me.lbl_SizingQty1.Size = New System.Drawing.Size(43, 16)
        Me.lbl_SizingQty1.TabIndex = 5
        Me.lbl_SizingQty1.Text = "2"
        Me.lbl_SizingQty1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_SizingQty2
        '
        Me.lbl_SizingQty2.BackColor = System.Drawing.Color.White
        Me.lbl_SizingQty2.Location = New System.Drawing.Point(4, 1)
        Me.lbl_SizingQty2.Name = "lbl_SizingQty2"
        Me.lbl_SizingQty2.Size = New System.Drawing.Size(43, 16)
        Me.lbl_SizingQty2.TabIndex = 8
        Me.lbl_SizingQty2.Text = "1"
        Me.lbl_SizingQty2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_SizingQty3
        '
        Me.lbl_SizingQty3.BackColor = System.Drawing.Color.White
        Me.lbl_SizingQty3.Location = New System.Drawing.Point(4, 49)
        Me.lbl_SizingQty3.Name = "lbl_SizingQty3"
        Me.lbl_SizingQty3.Size = New System.Drawing.Size(43, 16)
        Me.lbl_SizingQty3.TabIndex = 11
        Me.lbl_SizingQty3.Text = "3"
        Me.lbl_SizingQty3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_VatGross1
        '
        Me.lbl_VatGross1.BackColor = System.Drawing.Color.White
        Me.lbl_VatGross1.Location = New System.Drawing.Point(4, 73)
        Me.lbl_VatGross1.Name = "lbl_VatGross1"
        Me.lbl_VatGross1.Size = New System.Drawing.Size(40, 16)
        Me.lbl_VatGross1.TabIndex = 46
        Me.lbl_VatGross1.Text = "4"
        Me.lbl_VatGross1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_VatGross2
        '
        Me.lbl_VatGross2.BackColor = System.Drawing.Color.White
        Me.lbl_VatGross2.Location = New System.Drawing.Point(4, 97)
        Me.lbl_VatGross2.Name = "lbl_VatGross2"
        Me.lbl_VatGross2.Size = New System.Drawing.Size(43, 16)
        Me.lbl_VatGross2.TabIndex = 47
        Me.lbl_VatGross2.Text = "5"
        Me.lbl_VatGross2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txt_AcPayee_Left
        '
        Me.txt_AcPayee_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AcPayee_Left.Location = New System.Drawing.Point(285, 4)
        Me.txt_AcPayee_Left.MaxLength = 20
        Me.txt_AcPayee_Left.Name = "txt_AcPayee_Left"
        Me.txt_AcPayee_Left.Size = New System.Drawing.Size(112, 16)
        Me.txt_AcPayee_Left.TabIndex = 7
        Me.txt_AcPayee_Left.Text = "txt_AcPayeeLeft"
        Me.txt_AcPayee_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Date_Left
        '
        Me.txt_Date_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Date_Left.Location = New System.Drawing.Point(285, 28)
        Me.txt_Date_Left.MaxLength = 20
        Me.txt_Date_Left.Name = "txt_Date_Left"
        Me.txt_Date_Left.Size = New System.Drawing.Size(112, 16)
        Me.txt_Date_Left.TabIndex = 11
        Me.txt_Date_Left.Text = "txt_DateLeft"
        Me.txt_Date_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Date_width
        '
        Me.txt_Date_width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Date_width.Location = New System.Drawing.Point(523, 28)
        Me.txt_Date_width.MaxLength = 20
        Me.txt_Date_width.Name = "txt_Date_width"
        Me.txt_Date_width.Size = New System.Drawing.Size(100, 16)
        Me.txt_Date_width.TabIndex = 13
        Me.txt_Date_width.Text = "txt_Date_Width"
        Me.txt_Date_width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_PartyName_Top
        '
        Me.txt_PartyName_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_PartyName_Top.Location = New System.Drawing.Point(405, 52)
        Me.txt_PartyName_Top.MaxLength = 20
        Me.txt_PartyName_Top.Name = "txt_PartyName_Top"
        Me.txt_PartyName_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_PartyName_Top.TabIndex = 16
        Me.txt_PartyName_Top.Text = "txt_partyName_Top"
        Me.txt_PartyName_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_AmountWords_Left
        '
        Me.txt_AmountWords_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AmountWords_Left.Location = New System.Drawing.Point(285, 100)
        Me.txt_AmountWords_Left.MaxLength = 20
        Me.txt_AmountWords_Left.Name = "txt_AmountWords_Left"
        Me.txt_AmountWords_Left.Size = New System.Drawing.Size(112, 16)
        Me.txt_AmountWords_Left.TabIndex = 23
        Me.txt_AmountWords_Left.Text = "txt_AmountInWords"
        Me.txt_AmountWords_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Second_AmountWords_Left
        '
        Me.txt_Second_AmountWords_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Second_AmountWords_Left.Location = New System.Drawing.Point(285, 124)
        Me.txt_Second_AmountWords_Left.MaxLength = 20
        Me.txt_Second_AmountWords_Left.Name = "txt_Second_AmountWords_Left"
        Me.txt_Second_AmountWords_Left.Size = New System.Drawing.Size(112, 16)
        Me.txt_Second_AmountWords_Left.TabIndex = 27
        Me.txt_Second_AmountWords_Left.Text = "txt_AmountInWords"
        Me.txt_Second_AmountWords_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Rs_Left
        '
        Me.txt_Rs_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Rs_Left.Location = New System.Drawing.Point(285, 148)
        Me.txt_Rs_Left.MaxLength = 20
        Me.txt_Rs_Left.Name = "txt_Rs_Left"
        Me.txt_Rs_Left.Size = New System.Drawing.Size(113, 16)
        Me.txt_Rs_Left.TabIndex = 31
        Me.txt_Rs_Left.Text = "txt_Rs"
        Me.txt_Rs_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_CompanyName_Top
        '
        Me.txt_CompanyName_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_CompanyName_Top.Location = New System.Drawing.Point(405, 172)
        Me.txt_CompanyName_Top.MaxLength = 20
        Me.txt_CompanyName_Top.Name = "txt_CompanyName_Top"
        Me.txt_CompanyName_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_CompanyName_Top.TabIndex = 36
        Me.txt_CompanyName_Top.Text = "txt_CompanyName_Top"
        Me.txt_CompanyName_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_CompanyName_Left
        '
        Me.txt_CompanyName_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_CompanyName_Left.Location = New System.Drawing.Point(285, 172)
        Me.txt_CompanyName_Left.MaxLength = 20
        Me.txt_CompanyName_Left.Name = "txt_CompanyName_Left"
        Me.txt_CompanyName_Left.Size = New System.Drawing.Size(112, 16)
        Me.txt_CompanyName_Left.TabIndex = 35
        Me.txt_CompanyName_Left.Text = "txt_ForCompanyName"
        Me.txt_CompanyName_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_SizingAmount1
        '
        Me.lbl_SizingAmount1.BackColor = System.Drawing.Color.White
        Me.lbl_SizingAmount1.Location = New System.Drawing.Point(4, 121)
        Me.lbl_SizingAmount1.Name = "lbl_SizingAmount1"
        Me.lbl_SizingAmount1.Size = New System.Drawing.Size(43, 16)
        Me.lbl_SizingAmount1.TabIndex = 69
        Me.lbl_SizingAmount1.Text = "6"
        Me.lbl_SizingAmount1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_SizingAmount2
        '
        Me.lbl_SizingAmount2.BackColor = System.Drawing.Color.White
        Me.lbl_SizingAmount2.Location = New System.Drawing.Point(4, 145)
        Me.lbl_SizingAmount2.Name = "lbl_SizingAmount2"
        Me.lbl_SizingAmount2.Size = New System.Drawing.Size(43, 16)
        Me.lbl_SizingAmount2.TabIndex = 70
        Me.lbl_SizingAmount2.Text = "7"
        Me.lbl_SizingAmount2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txt_Rs
        '
        Me.txt_Rs.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Rs.Location = New System.Drawing.Point(54, 148)
        Me.txt_Rs.MaxLength = 35
        Me.txt_Rs.Name = "txt_Rs"
        Me.txt_Rs.Size = New System.Drawing.Size(224, 16)
        Me.txt_Rs.TabIndex = 30
        Me.txt_Rs.TabStop = False
        Me.txt_Rs.Text = "RS"
        '
        'lbl_SizingAmount3
        '
        Me.lbl_SizingAmount3.BackColor = System.Drawing.Color.White
        Me.lbl_SizingAmount3.Location = New System.Drawing.Point(4, 169)
        Me.lbl_SizingAmount3.Name = "lbl_SizingAmount3"
        Me.lbl_SizingAmount3.Size = New System.Drawing.Size(42, 16)
        Me.lbl_SizingAmount3.TabIndex = 48
        Me.lbl_SizingAmount3.Text = "8"
        Me.lbl_SizingAmount3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_WeldingAmount
        '
        Me.lbl_WeldingAmount.BackColor = System.Drawing.Color.White
        Me.lbl_WeldingAmount.Location = New System.Drawing.Point(4, 193)
        Me.lbl_WeldingAmount.Name = "lbl_WeldingAmount"
        Me.lbl_WeldingAmount.Size = New System.Drawing.Size(43, 16)
        Me.lbl_WeldingAmount.TabIndex = 49
        Me.lbl_WeldingAmount.Text = "9"
        Me.lbl_WeldingAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_RewindingAmount
        '
        Me.lbl_RewindingAmount.BackColor = System.Drawing.Color.White
        Me.lbl_RewindingAmount.Location = New System.Drawing.Point(4, 217)
        Me.lbl_RewindingAmount.Name = "lbl_RewindingAmount"
        Me.lbl_RewindingAmount.Size = New System.Drawing.Size(43, 16)
        Me.lbl_RewindingAmount.TabIndex = 50
        Me.lbl_RewindingAmount.Text = "10"
        Me.lbl_RewindingAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txt_Second_PartyName_Top
        '
        Me.txt_Second_PartyName_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Second_PartyName_Top.Location = New System.Drawing.Point(405, 76)
        Me.txt_Second_PartyName_Top.MaxLength = 20
        Me.txt_Second_PartyName_Top.Name = "txt_Second_PartyName_Top"
        Me.txt_Second_PartyName_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_Second_PartyName_Top.TabIndex = 20
        Me.txt_Second_PartyName_Top.Text = "txt_PartyName_Top"
        Me.txt_Second_PartyName_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Second_PartyName_Width
        '
        Me.txt_Second_PartyName_Width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Second_PartyName_Width.Location = New System.Drawing.Point(523, 76)
        Me.txt_Second_PartyName_Width.MaxLength = 20
        Me.txt_Second_PartyName_Width.Name = "txt_Second_PartyName_Width"
        Me.txt_Second_PartyName_Width.Size = New System.Drawing.Size(100, 16)
        Me.txt_Second_PartyName_Width.TabIndex = 21
        Me.txt_Second_PartyName_Width.Text = "txt_PartyName_Width"
        Me.txt_Second_PartyName_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_AmountWords_Top
        '
        Me.txt_AmountWords_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AmountWords_Top.Location = New System.Drawing.Point(405, 100)
        Me.txt_AmountWords_Top.MaxLength = 20
        Me.txt_AmountWords_Top.Name = "txt_AmountWords_Top"
        Me.txt_AmountWords_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_AmountWords_Top.TabIndex = 24
        Me.txt_AmountWords_Top.Text = "txt_AmountINWord_Top"
        Me.txt_AmountWords_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_AmountWords_Width
        '
        Me.txt_AmountWords_Width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AmountWords_Width.Location = New System.Drawing.Point(523, 100)
        Me.txt_AmountWords_Width.MaxLength = 20
        Me.txt_AmountWords_Width.Name = "txt_AmountWords_Width"
        Me.txt_AmountWords_Width.Size = New System.Drawing.Size(100, 16)
        Me.txt_AmountWords_Width.TabIndex = 25
        Me.txt_AmountWords_Width.Text = "txt_AmountInWords"
        Me.txt_AmountWords_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Second_AmountWords_Top
        '
        Me.txt_Second_AmountWords_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Second_AmountWords_Top.Location = New System.Drawing.Point(405, 124)
        Me.txt_Second_AmountWords_Top.MaxLength = 20
        Me.txt_Second_AmountWords_Top.Name = "txt_Second_AmountWords_Top"
        Me.txt_Second_AmountWords_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_Second_AmountWords_Top.TabIndex = 28
        Me.txt_Second_AmountWords_Top.Text = "txt_Amont_InWords"
        Me.txt_Second_AmountWords_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Second_AmountWords_Width
        '
        Me.txt_Second_AmountWords_Width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Second_AmountWords_Width.Location = New System.Drawing.Point(523, 124)
        Me.txt_Second_AmountWords_Width.MaxLength = 20
        Me.txt_Second_AmountWords_Width.Name = "txt_Second_AmountWords_Width"
        Me.txt_Second_AmountWords_Width.Size = New System.Drawing.Size(100, 16)
        Me.txt_Second_AmountWords_Width.TabIndex = 29
        Me.txt_Second_AmountWords_Width.Text = "txt_Amount_Width"
        Me.txt_Second_AmountWords_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Rs_Top
        '
        Me.txt_Rs_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Rs_Top.Location = New System.Drawing.Point(405, 148)
        Me.txt_Rs_Top.MaxLength = 20
        Me.txt_Rs_Top.Name = "txt_Rs_Top"
        Me.txt_Rs_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_Rs_Top.TabIndex = 32
        Me.txt_Rs_Top.Text = "txt_Rs_Top"
        Me.txt_Rs_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Rs_Width
        '
        Me.txt_Rs_Width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Rs_Width.Location = New System.Drawing.Point(523, 148)
        Me.txt_Rs_Width.MaxLength = 20
        Me.txt_Rs_Width.Name = "txt_Rs_Width"
        Me.txt_Rs_Width.Size = New System.Drawing.Size(100, 16)
        Me.txt_Rs_Width.TabIndex = 33
        Me.txt_Rs_Width.Text = "txt_Rs_Width"
        Me.txt_Rs_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txt_Rs_Width.Visible = False
        '
        'txt_Date_Top
        '
        Me.txt_Date_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Date_Top.Location = New System.Drawing.Point(405, 28)
        Me.txt_Date_Top.MaxLength = 20
        Me.txt_Date_Top.Name = "txt_Date_Top"
        Me.txt_Date_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_Date_Top.TabIndex = 12
        Me.txt_Date_Top.Text = "txt_Date_Top"
        Me.txt_Date_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_CompanyName_Width
        '
        Me.txt_CompanyName_Width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_CompanyName_Width.Location = New System.Drawing.Point(523, 172)
        Me.txt_CompanyName_Width.MaxLength = 20
        Me.txt_CompanyName_Width.Name = "txt_CompanyName_Width"
        Me.txt_CompanyName_Width.Size = New System.Drawing.Size(100, 16)
        Me.txt_CompanyName_Width.TabIndex = 37
        Me.txt_CompanyName_Width.Text = "txt_Company_Name_Width"
        Me.txt_CompanyName_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Partner_Width
        '
        Me.txt_Partner_Width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Partner_Width.Location = New System.Drawing.Point(523, 196)
        Me.txt_Partner_Width.MaxLength = 20
        Me.txt_Partner_Width.Name = "txt_Partner_Width"
        Me.txt_Partner_Width.Size = New System.Drawing.Size(100, 16)
        Me.txt_Partner_Width.TabIndex = 41
        Me.txt_Partner_Width.Text = "txt_Partner_Width"
        Me.txt_Partner_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_AccountNo_Width
        '
        Me.txt_AccountNo_Width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AccountNo_Width.Location = New System.Drawing.Point(523, 220)
        Me.txt_AccountNo_Width.MaxLength = 20
        Me.txt_AccountNo_Width.Name = "txt_AccountNo_Width"
        Me.txt_AccountNo_Width.Size = New System.Drawing.Size(100, 16)
        Me.txt_AccountNo_Width.TabIndex = 45
        Me.txt_AccountNo_Width.Text = "txt_Account_Width"
        Me.txt_AccountNo_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txt_AccountNo_Width.Visible = False
        '
        'txt_AccountNo_Left
        '
        Me.txt_AccountNo_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AccountNo_Left.Location = New System.Drawing.Point(285, 220)
        Me.txt_AccountNo_Left.MaxLength = 20
        Me.txt_AccountNo_Left.Name = "txt_AccountNo_Left"
        Me.txt_AccountNo_Left.Size = New System.Drawing.Size(113, 16)
        Me.txt_AccountNo_Left.TabIndex = 43
        Me.txt_AccountNo_Left.Text = "txt_AccountNo_Lefft"
        Me.txt_AccountNo_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Partner_Top
        '
        Me.txt_Partner_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Partner_Top.Location = New System.Drawing.Point(405, 196)
        Me.txt_Partner_Top.MaxLength = 20
        Me.txt_Partner_Top.Name = "txt_Partner_Top"
        Me.txt_Partner_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_Partner_Top.TabIndex = 40
        Me.txt_Partner_Top.Text = "txt_PartNer_Top"
        Me.txt_Partner_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_AccountNo_Top
        '
        Me.txt_AccountNo_Top.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AccountNo_Top.Location = New System.Drawing.Point(405, 220)
        Me.txt_AccountNo_Top.MaxLength = 20
        Me.txt_AccountNo_Top.Name = "txt_AccountNo_Top"
        Me.txt_AccountNo_Top.Size = New System.Drawing.Size(111, 16)
        Me.txt_AccountNo_Top.TabIndex = 44
        Me.txt_AccountNo_Top.Text = "txt_Account_Top"
        Me.txt_AccountNo_Top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Partner_Left
        '
        Me.txt_Partner_Left.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_Partner_Left.Location = New System.Drawing.Point(285, 196)
        Me.txt_Partner_Left.MaxLength = 20
        Me.txt_Partner_Left.Name = "txt_Partner_Left"
        Me.txt_Partner_Left.Size = New System.Drawing.Size(113, 16)
        Me.txt_Partner_Left.TabIndex = 39
        Me.txt_Partner_Left.Text = "txt_Partner_Left"
        Me.txt_Partner_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_AcPayeeWidth
        '
        Me.txt_AcPayeeWidth.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_AcPayeeWidth.Location = New System.Drawing.Point(523, 4)
        Me.txt_AcPayeeWidth.MaxLength = 20
        Me.txt_AcPayeeWidth.Name = "txt_AcPayeeWidth"
        Me.txt_AcPayeeWidth.Size = New System.Drawing.Size(100, 16)
        Me.txt_AcPayeeWidth.TabIndex = 9
        Me.txt_AcPayeeWidth.Text = "txt_AcPayee_Width"
        Me.txt_AcPayeeWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txt_AcPayeeWidth.Visible = False
        '
        'txt_PartyName_width
        '
        Me.txt_PartyName_width.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txt_PartyName_width.Location = New System.Drawing.Point(523, 52)
        Me.txt_PartyName_width.MaxLength = 20
        Me.txt_PartyName_width.Name = "txt_PartyName_width"
        Me.txt_PartyName_width.Size = New System.Drawing.Size(100, 16)
        Me.txt_PartyName_width.TabIndex = 17
        Me.txt_PartyName_width.Text = "txt_PartyName_Width"
        Me.txt_PartyName_width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'grp_find
        '
        Me.grp_find.Controls.Add(Me.btn_FindOpen)
        Me.grp_find.Controls.Add(Me.btn_FindClose)
        Me.grp_find.Controls.Add(Me.cbo_Find)
        Me.grp_find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_find.Location = New System.Drawing.Point(34, 620)
        Me.grp_find.Name = "grp_find"
        Me.grp_find.Size = New System.Drawing.Size(516, 174)
        Me.grp_find.TabIndex = 27
        Me.grp_find.TabStop = False
        Me.grp_find.Text = "FINDING"
        '
        'btn_FindOpen
        '
        Me.btn_FindOpen.BackColor = System.Drawing.Color.Maroon
        Me.btn_FindOpen.ForeColor = System.Drawing.Color.White
        Me.btn_FindOpen.Location = New System.Drawing.Point(315, 131)
        Me.btn_FindOpen.Name = "btn_FindOpen"
        Me.btn_FindOpen.Size = New System.Drawing.Size(77, 28)
        Me.btn_FindOpen.TabIndex = 1
        Me.btn_FindOpen.Text = "&OPEN"
        Me.btn_FindOpen.UseVisualStyleBackColor = False
        '
        'btn_FindClose
        '
        Me.btn_FindClose.BackColor = System.Drawing.Color.Maroon
        Me.btn_FindClose.ForeColor = System.Drawing.Color.White
        Me.btn_FindClose.Location = New System.Drawing.Point(416, 131)
        Me.btn_FindClose.Name = "btn_FindClose"
        Me.btn_FindClose.Size = New System.Drawing.Size(77, 28)
        Me.btn_FindClose.TabIndex = 2
        Me.btn_FindClose.Text = "&CLOSE"
        Me.btn_FindClose.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(18, 25)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(475, 23)
        Me.cbo_Find.TabIndex = 5
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(91, Byte), Integer))
        Me.lbl_UserName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.Color.White
        Me.lbl_UserName.Location = New System.Drawing.Point(576, 9)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(105, 19)
        Me.lbl_UserName.TabIndex = 267
        Me.lbl_UserName.Text = "USER : ADMIN"
        '
        'Cheque_Print_Positioning
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(697, 584)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.grp_find)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnl_Filter)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Cheque_Print_Positioning"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CHEQUE PRINT POSITINING"
        Me.pnl_Filter.ResumeLayout(False)
        Me.pnl_Filter.PerformLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        CType(Me.dgv_BackDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.grp_find.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents label As System.Windows.Forms.Label
    Friend WithEvents pnl_Filter As System.Windows.Forms.Panel
    Friend WithEvents txt_filter_billNo As System.Windows.Forms.TextBox
    Friend WithEvents btn_Filter_Close As System.Windows.Forms.Button
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents cbo_Filter_DelvAt As System.Windows.Forms.ComboBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents btn_Filter_Show As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter_Details As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column17 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column19 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbo_Filter_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents dtp_Filter_ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents dtp_Filter_Fromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbl_ChqNo As System.Windows.Forms.Label
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents cbo_BankName As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txt_TopMargin As System.Windows.Forms.TextBox
    Friend WithEvents txt_AccountNo As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Cbo_PaperOrientation As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Partner As System.Windows.Forms.ComboBox
    Friend WithEvents txt_LeftMargin As System.Windows.Forms.TextBox
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents grp_find As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FindOpen As System.Windows.Forms.Button
    Friend WithEvents btn_FindClose As System.Windows.Forms.Button
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents dgv_BackDetails As System.Windows.Forms.DataGridView
    Friend WithEvents txt_PartyName As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents txt_second_PartyName_Left As System.Windows.Forms.TextBox
    Friend WithEvents txt_PartyName_Left As System.Windows.Forms.TextBox
    Friend WithEvents lbl_VatGross2 As System.Windows.Forms.Label
    Friend WithEvents lbl_VatGross1 As System.Windows.Forms.Label
    Friend WithEvents lbl_SizingQty3 As System.Windows.Forms.Label
    Friend WithEvents lbl_SizingQty2 As System.Windows.Forms.Label
    Friend WithEvents lbl_SizingQty1 As System.Windows.Forms.Label
    Friend WithEvents lbl_SizingAmount3 As System.Windows.Forms.Label
    Friend WithEvents lbl_SizingAmount2 As System.Windows.Forms.Label
    Friend WithEvents lbl_SizingAmount1 As System.Windows.Forms.Label
    Friend WithEvents txt_CompanyName_Left As System.Windows.Forms.TextBox
    Friend WithEvents txt_Second_AmountWords_Left As System.Windows.Forms.TextBox
    Friend WithEvents lbl_RewindingAmount As System.Windows.Forms.Label
    Friend WithEvents txt_ACPayee As System.Windows.Forms.TextBox
    Friend WithEvents lbl_WeldingAmount As System.Windows.Forms.Label
    Friend WithEvents txt_CompanyName_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_Rs_Left As System.Windows.Forms.TextBox
    Friend WithEvents txt_AcPayeeWidth As System.Windows.Forms.TextBox
    Friend WithEvents txt_Account_No As System.Windows.Forms.TextBox
    Friend WithEvents txt_Date_Left As System.Windows.Forms.TextBox
    Friend WithEvents txt_PartyName_width As System.Windows.Forms.TextBox
    Friend WithEvents txt_AcPayee_Left As System.Windows.Forms.TextBox
    Friend WithEvents txt_PartyName_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_AcPayee_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_Date As System.Windows.Forms.TextBox
    Friend WithEvents txt_Date_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_Partner As System.Windows.Forms.TextBox
    Friend WithEvents txt_Date_width As System.Windows.Forms.TextBox
    Friend WithEvents txt_Company_Name As System.Windows.Forms.TextBox
    Friend WithEvents txt_Rs As System.Windows.Forms.TextBox
    Friend WithEvents txt_Amount_Words As System.Windows.Forms.TextBox
    Friend WithEvents txt_Second_PartyName As System.Windows.Forms.TextBox
    Friend WithEvents txt_Second_Amount_Words As System.Windows.Forms.TextBox
    Friend WithEvents txt_AmountWords_Left As System.Windows.Forms.TextBox
    Friend WithEvents txt_Second_PartyName_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_Second_PartyName_Width As System.Windows.Forms.TextBox
    Friend WithEvents txt_AmountWords_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_AmountWords_Width As System.Windows.Forms.TextBox
    Friend WithEvents txt_Second_AmountWords_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_Second_AmountWords_Width As System.Windows.Forms.TextBox
    Friend WithEvents txt_Rs_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_Rs_Width As System.Windows.Forms.TextBox
    Friend WithEvents txt_CompanyName_Width As System.Windows.Forms.TextBox
    Friend WithEvents txt_Partner_Width As System.Windows.Forms.TextBox
    Friend WithEvents txt_AccountNo_Width As System.Windows.Forms.TextBox
    Friend WithEvents txt_AccountNo_Left As System.Windows.Forms.TextBox
    Friend WithEvents txt_Partner_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_AccountNo_Top As System.Windows.Forms.TextBox
    Friend WithEvents txt_Partner_Left As System.Windows.Forms.TextBox
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
End Class
