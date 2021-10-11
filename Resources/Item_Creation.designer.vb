<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Item_Creation
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Item_Creation))
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.grp_Back = New System.Windows.Forms.GroupBox()
        Me.cbo_Size = New System.Windows.Forms.ComboBox()
        Me.dgv_details = New System.Windows.Forms.DataGridView()
        Me.txt_TamilName = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txt_MinimumStock = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_CostRate = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_Code = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cbo_Unit = New System.Windows.Forms.ComboBox()
        Me.cbo_ItemGroup = New System.Windows.Forms.ComboBox()
        Me.txt_TaxRate = New System.Windows.Forms.TextBox()
        Me.txt_Rate = New System.Windows.Forms.TextBox()
        Me.txt_TaxPerc = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.btn_OpenFilter = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.dgv_PriceListdetails = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grp_Back.SuspendLayout()
        CType(Me.dgv_details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Open.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv_PriceListdetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grp_Back
        '
        Me.grp_Back.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.grp_Back.Controls.Add(Me.cbo_Size)
        Me.grp_Back.Controls.Add(Me.dgv_details)
        Me.grp_Back.Controls.Add(Me.txt_TamilName)
        Me.grp_Back.Controls.Add(Me.Label12)
        Me.grp_Back.Controls.Add(Me.txt_MinimumStock)
        Me.grp_Back.Controls.Add(Me.Label11)
        Me.grp_Back.Controls.Add(Me.txt_CostRate)
        Me.grp_Back.Controls.Add(Me.Label10)
        Me.grp_Back.Controls.Add(Me.txt_Code)
        Me.grp_Back.Controls.Add(Me.Label9)
        Me.grp_Back.Controls.Add(Me.cbo_Unit)
        Me.grp_Back.Controls.Add(Me.cbo_ItemGroup)
        Me.grp_Back.Controls.Add(Me.txt_TaxRate)
        Me.grp_Back.Controls.Add(Me.txt_Rate)
        Me.grp_Back.Controls.Add(Me.txt_TaxPerc)
        Me.grp_Back.Controls.Add(Me.Label8)
        Me.grp_Back.Controls.Add(Me.txt_Name)
        Me.grp_Back.Controls.Add(Me.Label6)
        Me.grp_Back.Controls.Add(Me.Label4)
        Me.grp_Back.Controls.Add(Me.Label5)
        Me.grp_Back.Controls.Add(Me.Label3)
        Me.grp_Back.Controls.Add(Me.Label2)
        Me.grp_Back.Controls.Add(Me.lbl_IdNo)
        Me.grp_Back.Controls.Add(Me.Label1)
        Me.grp_Back.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Back.Location = New System.Drawing.Point(6, 53)
        Me.grp_Back.Name = "grp_Back"
        Me.grp_Back.Size = New System.Drawing.Size(713, 443)
        Me.grp_Back.TabIndex = 0
        Me.grp_Back.TabStop = False
        '
        'cbo_Size
        '
        Me.cbo_Size.FormattingEnabled = True
        Me.cbo_Size.Location = New System.Drawing.Point(127, 335)
        Me.cbo_Size.Name = "cbo_Size"
        Me.cbo_Size.Size = New System.Drawing.Size(175, 21)
        Me.cbo_Size.TabIndex = 14
        Me.cbo_Size.Visible = False
        '
        'dgv_details
        '
        Me.dgv_details.AllowUserToResizeColumns = False
        Me.dgv_details.AllowUserToResizeRows = False
        Me.dgv_details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Fuchsia
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column5, Me.Column3, Me.Column4})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_details.DefaultCellStyle = DataGridViewCellStyle7
        Me.dgv_details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_details.Location = New System.Drawing.Point(9, 233)
        Me.dgv_details.Name = "dgv_details"
        Me.dgv_details.RowHeadersVisible = False
        Me.dgv_details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_details.Size = New System.Drawing.Size(681, 186)
        Me.dgv_details.TabIndex = 13
        Me.dgv_details.TabStop = False
        '
        'txt_TamilName
        '
        Me.txt_TamilName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_TamilName.Location = New System.Drawing.Point(106, 193)
        Me.txt_TamilName.Name = "txt_TamilName"
        Me.txt_TamilName.Size = New System.Drawing.Size(584, 22)
        Me.txt_TamilName.TabIndex = 12
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 197)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(85, 13)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "Name  Tamil :"
        '
        'txt_MinimumStock
        '
        Me.txt_MinimumStock.Location = New System.Drawing.Point(402, 116)
        Me.txt_MinimumStock.MaxLength = 12
        Me.txt_MinimumStock.Name = "txt_MinimumStock"
        Me.txt_MinimumStock.Size = New System.Drawing.Size(111, 20)
        Me.txt_MinimumStock.TabIndex = 5
        Me.txt_MinimumStock.Text = "txt_MinimumStock"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(296, 120)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(100, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Minimum Stock :"
        '
        'txt_CostRate
        '
        Me.txt_CostRate.Location = New System.Drawing.Point(66, 155)
        Me.txt_CostRate.MaxLength = 12
        Me.txt_CostRate.Name = "txt_CostRate"
        Me.txt_CostRate.Size = New System.Drawing.Size(214, 20)
        Me.txt_CostRate.TabIndex = 6
        Me.txt_CostRate.Text = "txt_CostRate"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 159)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(71, 13)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Cost Rate :"
        '
        'txt_Code
        '
        Me.txt_Code.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Code.Location = New System.Drawing.Point(66, 78)
        Me.txt_Code.MaxLength = 20
        Me.txt_Code.Name = "txt_Code"
        Me.txt_Code.Size = New System.Drawing.Size(214, 20)
        Me.txt_Code.TabIndex = 2
        Me.txt_Code.Text = "TXT_CODE"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 82)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(44, 13)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Code :"
        '
        'cbo_Unit
        '
        Me.cbo_Unit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Unit.FormattingEnabled = True
        Me.cbo_Unit.Location = New System.Drawing.Point(66, 116)
        Me.cbo_Unit.Name = "cbo_Unit"
        Me.cbo_Unit.Size = New System.Drawing.Size(214, 21)
        Me.cbo_Unit.TabIndex = 4
        Me.cbo_Unit.Text = "cbo_Unit"
        '
        'cbo_ItemGroup
        '
        Me.cbo_ItemGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_ItemGroup.FormattingEnabled = True
        Me.cbo_ItemGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_ItemGroup.Location = New System.Drawing.Point(402, 78)
        Me.cbo_ItemGroup.Name = "cbo_ItemGroup"
        Me.cbo_ItemGroup.Size = New System.Drawing.Size(288, 21)
        Me.cbo_ItemGroup.TabIndex = 3
        Me.cbo_ItemGroup.Text = "cbo_ItemGroup"
        '
        'txt_TaxRate
        '
        Me.txt_TaxRate.Location = New System.Drawing.Point(610, 155)
        Me.txt_TaxRate.MaxLength = 12
        Me.txt_TaxRate.Name = "txt_TaxRate"
        Me.txt_TaxRate.Size = New System.Drawing.Size(80, 20)
        Me.txt_TaxRate.TabIndex = 8
        Me.txt_TaxRate.Text = "txt_TaxRate"
        '
        'txt_Rate
        '
        Me.txt_Rate.Location = New System.Drawing.Point(402, 155)
        Me.txt_Rate.MaxLength = 12
        Me.txt_Rate.Name = "txt_Rate"
        Me.txt_Rate.Size = New System.Drawing.Size(111, 20)
        Me.txt_Rate.TabIndex = 7
        Me.txt_Rate.Text = "txt_Rate"
        '
        'txt_TaxPerc
        '
        Me.txt_TaxPerc.Location = New System.Drawing.Point(592, 116)
        Me.txt_TaxPerc.MaxLength = 6
        Me.txt_TaxPerc.Name = "txt_TaxPerc"
        Me.txt_TaxPerc.Size = New System.Drawing.Size(98, 20)
        Me.txt_TaxPerc.TabIndex = 5
        Me.txt_TaxPerc.Text = "txt_TaxPerc"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(529, 154)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(84, 27)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Sales Rate : (Incl.Tax)"
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Location = New System.Drawing.Point(402, 40)
        Me.txt_Name.MaxLength = 50
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(288, 20)
        Me.txt_Name.TabIndex = 1
        Me.txt_Name.Text = "TXT_NAME"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(296, 154)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 27)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Sales Rate : (Excl.Tax)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 120)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Unit :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(529, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(41, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Tax %"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(292, 82)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Item Group :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(292, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Name :"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.Enabled = False
        Me.lbl_IdNo.Location = New System.Drawing.Point(66, 40)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(214, 20)
        Me.lbl_IdNo.TabIndex = 0
        Me.lbl_IdNo.Text = "txt_IdNo"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo :"
        '
        'grp_Open
        '
        Me.grp_Open.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btnClose)
        Me.grp_Open.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(877, 457)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(458, 214)
        Me.grp_Open.TabIndex = 31
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
        Me.cbo_Open.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Open.DropDownHeight = 90
        Me.cbo_Open.Font = New System.Drawing.Font("Cambria", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(16, 28)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(423, 25)
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
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.btn_OpenFilter)
        Me.grp_Filter.Location = New System.Drawing.Point(877, 119)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(506, 300)
        Me.grp_Filter.TabIndex = 32
        Me.grp_Filter.TabStop = False
        '
        'dgv_Filter
        '
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Location = New System.Drawing.Point(8, 14)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.Size = New System.Drawing.Size(482, 240)
        Me.dgv_Filter.TabIndex = 0
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.ForeColor = System.Drawing.Color.Blue
        Me.btn_CloseFilter.Location = New System.Drawing.Point(409, 265)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(81, 28)
        Me.btn_CloseFilter.TabIndex = 2
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'btn_OpenFilter
        '
        Me.btn_OpenFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btn_OpenFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_OpenFilter.ForeColor = System.Drawing.Color.Blue
        Me.btn_OpenFilter.Location = New System.Drawing.Point(311, 265)
        Me.btn_OpenFilter.Name = "btn_OpenFilter"
        Me.btn_OpenFilter.Size = New System.Drawing.Size(82, 27)
        Me.btn_OpenFilter.TabIndex = 1
        Me.btn_OpenFilter.Text = "&OPEN"
        Me.btn_OpenFilter.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoEllipsis = True
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label7.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label7.Location = New System.Drawing.Point(0, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(728, 50)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "ITEM CREATION"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgv_PriceListdetails
        '
        Me.dgv_PriceListdetails.AllowUserToResizeColumns = False
        Me.dgv_PriceListdetails.AllowUserToResizeRows = False
        Me.dgv_PriceListdetails.BackgroundColor = System.Drawing.Color.White
        Me.dgv_PriceListdetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_PriceListdetails.DefaultCellStyle = DataGridViewCellStyle8
        Me.dgv_PriceListdetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_PriceListdetails.Location = New System.Drawing.Point(9, 233)
        Me.dgv_PriceListdetails.Name = "dgv_PriceListdetails"
        Me.dgv_PriceListdetails.RowHeadersVisible = False
        Me.dgv_PriceListdetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_PriceListdetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_PriceListdetails.Size = New System.Drawing.Size(681, 186)
        Me.dgv_PriceListdetails.TabIndex = 13
        Me.dgv_PriceListdetails.TabStop = False
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "S.NO"
        Me.Column1.MaxInputLength = 35
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 60
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "SIZE"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 150
        '
        'Column5
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column5.HeaderText = "PIECE/RATE"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 150
        '
        'Column3
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column3.HeaderText = "PURCHASE RATE"
        Me.Column3.MaxInputLength = 35
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 150
        '
        'Column4
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column4.HeaderText = "SALES RATE"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 150
        '
        'Item_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(728, 511)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.grp_Back)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Item_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ITEM CREATION"
        Me.grp_Back.ResumeLayout(False)
        Me.grp_Back.PerformLayout()
        CType(Me.dgv_details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Open.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv_PriceListdetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grp_Back As System.Windows.Forms.GroupBox
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbo_Unit As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_ItemGroup As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_Rate As System.Windows.Forms.TextBox
    Friend WithEvents txt_TaxPerc As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents btn_OpenFilter As System.Windows.Forms.Button
    Friend WithEvents txt_TaxRate As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_Code As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txt_CostRate As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txt_MinimumStock As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txt_TamilName As System.Windows.Forms.TextBox
    Friend WithEvents dgv_details As System.Windows.Forms.DataGridView
    Friend WithEvents cbo_Size As System.Windows.Forms.ComboBox
    Friend WithEvents dgv_PriceListdetails As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
