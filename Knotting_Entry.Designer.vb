<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Knotting_Entry
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btn_Filter_Close = New System.Windows.Forms.Button()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.cbo_Ends = New System.Windows.Forms.ComboBox()
        Me.cbo_KnottingName = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txt_LoomNo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.lbl_KnotNo = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_Size = New System.Windows.Forms.Label()
        Me.cbo_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.cbo_Shift = New System.Windows.Forms.ComboBox()
        Me.txt_NoOfPavu = New System.Windows.Forms.TextBox()
        Me.lbl_Quantity = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.cbo_Filter_Shift = New System.Windows.Forms.ComboBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.btn_Filter_Show = New System.Windows.Forms.Button()
        Me.dgv_Filter_Details = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cbo_Filter_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.dtp_Filter_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.pnl_Filter = New System.Windows.Forms.Panel()
        Me.dtp_Filter_Fromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Filter.SuspendLayout()
        Me.SuspendLayout()
        '
        'btn_Filter_Close
        '
        Me.btn_Filter_Close.BackColor = System.Drawing.Color.White
        Me.btn_Filter_Close.BackgroundImage = Global.Billing.My.Resources.Resources.Close1
        Me.btn_Filter_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Filter_Close.FlatAppearance.BorderSize = 0
        Me.btn_Filter_Close.Location = New System.Drawing.Point(516, -1)
        Me.btn_Filter_Close.Name = "btn_Filter_Close"
        Me.btn_Filter_Close.Size = New System.Drawing.Size(30, 29)
        Me.btn_Filter_Close.TabIndex = 40
        Me.btn_Filter_Close.TabStop = False
        Me.btn_Filter_Close.UseVisualStyleBackColor = True
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.cbo_Ends)
        Me.pnl_Back.Controls.Add(Me.cbo_KnottingName)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.txt_LoomNo)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.dtp_Date)
        Me.pnl_Back.Controls.Add(Me.lbl_KnotNo)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.lbl_Size)
        Me.pnl_Back.Controls.Add(Me.cbo_PartyName)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.lbl_Company)
        Me.pnl_Back.Controls.Add(Me.cbo_Shift)
        Me.pnl_Back.Controls.Add(Me.txt_NoOfPavu)
        Me.pnl_Back.Controls.Add(Me.lbl_Quantity)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.btnClose)
        Me.pnl_Back.Location = New System.Drawing.Point(6, 51)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(538, 442)
        Me.pnl_Back.TabIndex = 7
        '
        'cbo_Ends
        '
        Me.cbo_Ends.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Ends.DropDownHeight = 120
        Me.cbo_Ends.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Ends.FormattingEnabled = True
        Me.cbo_Ends.IntegralHeight = False
        Me.cbo_Ends.Location = New System.Drawing.Point(112, 299)
        Me.cbo_Ends.Name = "cbo_Ends"
        Me.cbo_Ends.Size = New System.Drawing.Size(403, 23)
        Me.cbo_Ends.Sorted = True
        Me.cbo_Ends.TabIndex = 5
        '
        'cbo_KnottingName
        '
        Me.cbo_KnottingName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_KnottingName.DropDownHeight = 150
        Me.cbo_KnottingName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_KnottingName.FormattingEnabled = True
        Me.cbo_KnottingName.IntegralHeight = False
        Me.cbo_KnottingName.Location = New System.Drawing.Point(113, 162)
        Me.cbo_KnottingName.Name = "cbo_KnottingName"
        Me.cbo_KnottingName.Size = New System.Drawing.Size(403, 23)
        Me.cbo_KnottingName.Sorted = True
        Me.cbo_KnottingName.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(13, 166)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(89, 15)
        Me.Label6.TabIndex = 43
        Me.Label6.Text = "Knotting Name"
        '
        'txt_LoomNo
        '
        Me.txt_LoomNo.Location = New System.Drawing.Point(112, 253)
        Me.txt_LoomNo.MaxLength = 100
        Me.txt_LoomNo.Name = "txt_LoomNo"
        Me.txt_LoomNo.Size = New System.Drawing.Size(402, 23)
        Me.txt_LoomNo.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(12, 257)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 15)
        Me.Label5.TabIndex = 41
        Me.Label5.Text = "Loom No"
        '
        'dtp_Date
        '
        Me.dtp_Date.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(112, 70)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(403, 23)
        Me.dtp_Date.TabIndex = 0
        '
        'lbl_KnotNo
        '
        Me.lbl_KnotNo.BackColor = System.Drawing.Color.White
        Me.lbl_KnotNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_KnotNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_KnotNo.Location = New System.Drawing.Point(112, 23)
        Me.lbl_KnotNo.Name = "lbl_KnotNo"
        Me.lbl_KnotNo.Size = New System.Drawing.Size(404, 23)
        Me.lbl_KnotNo.TabIndex = 40
        Me.lbl_KnotNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(12, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 15)
        Me.Label4.TabIndex = 39
        Me.Label4.Text = "Knot No"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(12, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 38
        Me.Label2.Text = "Date"
        '
        'lbl_Size
        '
        Me.lbl_Size.AutoSize = True
        Me.lbl_Size.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Size.ForeColor = System.Drawing.Color.Navy
        Me.lbl_Size.Location = New System.Drawing.Point(12, 305)
        Me.lbl_Size.Name = "lbl_Size"
        Me.lbl_Size.Size = New System.Drawing.Size(32, 15)
        Me.lbl_Size.TabIndex = 37
        Me.lbl_Size.Text = "Ends"
        '
        'cbo_PartyName
        '
        Me.cbo_PartyName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_PartyName.DropDownHeight = 150
        Me.cbo_PartyName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_PartyName.FormattingEnabled = True
        Me.cbo_PartyName.IntegralHeight = False
        Me.cbo_PartyName.Location = New System.Drawing.Point(112, 207)
        Me.cbo_PartyName.Name = "cbo_PartyName"
        Me.cbo_PartyName.Size = New System.Drawing.Size(403, 23)
        Me.cbo_PartyName.Sorted = True
        Me.cbo_PartyName.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(12, 120)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 15)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "Shift"
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.Location = New System.Drawing.Point(437, 0)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 34
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'cbo_Shift
        '
        Me.cbo_Shift.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Shift.DropDownHeight = 120
        Me.cbo_Shift.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Shift.FormattingEnabled = True
        Me.cbo_Shift.IntegralHeight = False
        Me.cbo_Shift.Location = New System.Drawing.Point(112, 116)
        Me.cbo_Shift.Name = "cbo_Shift"
        Me.cbo_Shift.Size = New System.Drawing.Size(403, 23)
        Me.cbo_Shift.Sorted = True
        Me.cbo_Shift.TabIndex = 1
        '
        'txt_NoOfPavu
        '
        Me.txt_NoOfPavu.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_NoOfPavu.Location = New System.Drawing.Point(112, 345)
        Me.txt_NoOfPavu.MaxLength = 10
        Me.txt_NoOfPavu.Name = "txt_NoOfPavu"
        Me.txt_NoOfPavu.Size = New System.Drawing.Size(403, 23)
        Me.txt_NoOfPavu.TabIndex = 6
        '
        'lbl_Quantity
        '
        Me.lbl_Quantity.AutoSize = True
        Me.lbl_Quantity.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Quantity.ForeColor = System.Drawing.Color.Navy
        Me.lbl_Quantity.Location = New System.Drawing.Point(12, 349)
        Me.lbl_Quantity.Name = "lbl_Quantity"
        Me.lbl_Quantity.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Quantity.TabIndex = 16
        Me.lbl_Quantity.Text = "No.Of.Pavu"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(12, 211)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 15)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Party Name"
        '
        'btn_Save
        '
        Me.btn_Save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.ForeColor = System.Drawing.Color.Navy
        Me.btn_Save.Image = Global.Billing.My.Resources.Resources.SAVE1
        Me.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Save.Location = New System.Drawing.Point(285, 390)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(96, 31)
        Me.btn_Save.TabIndex = 7
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "&Save"
        Me.btn_Save.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Save.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Navy
        Me.btnClose.Image = Global.Billing.My.Resources.Resources.Close1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(417, 390)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(96, 31)
        Me.btnClose.TabIndex = 8
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label34
        '
        Me.Label34.BackColor = System.Drawing.Color.Purple
        Me.Label34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label34.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.Color.White
        Me.Label34.Location = New System.Drawing.Point(0, 0)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(546, 29)
        Me.Label34.TabIndex = 41
        Me.Label34.Text = "FILTER"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.Purple
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.White
        Me.Label29.Location = New System.Drawing.Point(547, -48)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(71, 20)
        Me.Label29.TabIndex = 37
        Me.Label29.Text = "FILTER"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbo_Filter_Shift
        '
        Me.cbo_Filter_Shift.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Filter_Shift.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Filter_Shift.FormattingEnabled = True
        Me.cbo_Filter_Shift.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.cbo_Filter_Shift.Location = New System.Drawing.Point(377, 45)
        Me.cbo_Filter_Shift.MaxDropDownItems = 15
        Me.cbo_Filter_Shift.Name = "cbo_Filter_Shift"
        Me.cbo_Filter_Shift.Size = New System.Drawing.Size(160, 23)
        Me.cbo_Filter_Shift.Sorted = True
        Me.cbo_Filter_Shift.TabIndex = 2
        Me.cbo_Filter_Shift.Text = "cbo_Filter_Shift"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.Color.Blue
        Me.Label33.Location = New System.Drawing.Point(340, 49)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(31, 15)
        Me.Label33.TabIndex = 34
        Me.Label33.Text = "Shift"
        '
        'btn_Filter_Show
        '
        Me.btn_Filter_Show.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter_Show.ForeColor = System.Drawing.Color.Blue
        Me.btn_Filter_Show.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter_Show.Location = New System.Drawing.Point(450, 80)
        Me.btn_Filter_Show.Name = "btn_Filter_Show"
        Me.btn_Filter_Show.Size = New System.Drawing.Size(87, 53)
        Me.btn_Filter_Show.TabIndex = 4
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
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Filter_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Filter_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.Column1, Me.Column2})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Filter_Details.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgv_Filter_Details.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgv_Filter_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter_Details.Location = New System.Drawing.Point(0, 146)
        Me.dgv_Filter_Details.MultiSelect = False
        Me.dgv_Filter_Details.Name = "dgv_Filter_Details"
        Me.dgv_Filter_Details.ReadOnly = True
        Me.dgv_Filter_Details.RowHeadersVisible = False
        Me.dgv_Filter_Details.RowHeadersWidth = 15
        Me.dgv_Filter_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Filter_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter_Details.Size = New System.Drawing.Size(546, 235)
        Me.dgv_Filter_Details.TabIndex = 31
        Me.dgv_Filter_Details.TabStop = False
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn1.Frozen = True
        Me.DataGridViewTextBoxColumn1.HeaderText = "S.NO"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 35
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "KNOT NO"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 70
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "DATE"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 80
        '
        'DataGridViewTextBoxColumn4
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn4.HeaderText = "PARTY NAME"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 165
        '
        'Column1
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column1.HeaderText = "ENDS"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 75
        '
        'Column2
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column2.HeaderText = "NO OF PAVU"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'cbo_Filter_PartyName
        '
        Me.cbo_Filter_PartyName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Filter_PartyName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Filter_PartyName.FormattingEnabled = True
        Me.cbo_Filter_PartyName.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.cbo_Filter_PartyName.Location = New System.Drawing.Point(85, 95)
        Me.cbo_Filter_PartyName.MaxDropDownItems = 15
        Me.cbo_Filter_PartyName.Name = "cbo_Filter_PartyName"
        Me.cbo_Filter_PartyName.Size = New System.Drawing.Size(347, 23)
        Me.cbo_Filter_PartyName.Sorted = True
        Me.cbo_Filter_PartyName.TabIndex = 3
        Me.cbo_Filter_PartyName.Text = "cbo_Filter_PartyName"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ForeColor = System.Drawing.Color.Blue
        Me.Label32.Location = New System.Drawing.Point(7, 99)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(72, 15)
        Me.Label32.TabIndex = 30
        Me.Label32.Text = "Party Name"
        '
        'dtp_Filter_ToDate
        '
        Me.dtp_Filter_ToDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Filter_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_ToDate.Location = New System.Drawing.Point(229, 45)
        Me.dtp_Filter_ToDate.Name = "dtp_Filter_ToDate"
        Me.dtp_Filter_ToDate.Size = New System.Drawing.Size(96, 23)
        Me.dtp_Filter_ToDate.TabIndex = 1
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.ForeColor = System.Drawing.Color.Blue
        Me.Label31.Location = New System.Drawing.Point(204, 49)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(19, 15)
        Me.Label31.TabIndex = 29
        Me.Label31.Text = "To"
        '
        'pnl_Filter
        '
        Me.pnl_Filter.BackColor = System.Drawing.Color.White
        Me.pnl_Filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Close)
        Me.pnl_Filter.Controls.Add(Me.Label34)
        Me.pnl_Filter.Controls.Add(Me.Label29)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_Shift)
        Me.pnl_Filter.Controls.Add(Me.Label33)
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Show)
        Me.pnl_Filter.Controls.Add(Me.dgv_Filter_Details)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_PartyName)
        Me.pnl_Filter.Controls.Add(Me.Label32)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_ToDate)
        Me.pnl_Filter.Controls.Add(Me.Label31)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_Fromdate)
        Me.pnl_Filter.Controls.Add(Me.Label30)
        Me.pnl_Filter.Location = New System.Drawing.Point(22, 517)
        Me.pnl_Filter.Margin = New System.Windows.Forms.Padding(0)
        Me.pnl_Filter.Name = "pnl_Filter"
        Me.pnl_Filter.Size = New System.Drawing.Size(548, 383)
        Me.pnl_Filter.TabIndex = 9
        Me.pnl_Filter.Visible = False
        '
        'dtp_Filter_Fromdate
        '
        Me.dtp_Filter_Fromdate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Filter_Fromdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_Fromdate.Location = New System.Drawing.Point(85, 45)
        Me.dtp_Filter_Fromdate.Name = "dtp_Filter_Fromdate"
        Me.dtp_Filter_Fromdate.Size = New System.Drawing.Size(113, 23)
        Me.dtp_Filter_Fromdate.TabIndex = 0
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.ForeColor = System.Drawing.Color.Blue
        Me.Label30.Location = New System.Drawing.Point(7, 49)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(62, 15)
        Me.Label30.TabIndex = 27
        Me.Label30.Text = "Knot Date"
        '
        'Label7
        '
        Me.Label7.AutoEllipsis = True
        Me.Label7.BackColor = System.Drawing.Color.DimGray
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label7.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label7.Location = New System.Drawing.Point(0, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(559, 40)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "KNOTTING ENTRY"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Knotting_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SkyBlue
        Me.ClientSize = New System.Drawing.Size(559, 522)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.pnl_Filter)
        Me.Controls.Add(Me.Label7)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Knotting_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "KNOTTING ENTRY"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Filter.ResumeLayout(False)
        Me.pnl_Filter.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_Filter_Close As System.Windows.Forms.Button
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_KnotNo As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_Size As System.Windows.Forms.Label
    Friend WithEvents cbo_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents cbo_Shift As System.Windows.Forms.ComboBox
    Friend WithEvents txt_NoOfPavu As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Quantity As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents cbo_Filter_Shift As System.Windows.Forms.ComboBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents btn_Filter_Show As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter_Details As System.Windows.Forms.DataGridView
    Friend WithEvents cbo_Filter_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents dtp_Filter_ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents pnl_Filter As System.Windows.Forms.Panel
    Friend WithEvents dtp_Filter_Fromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txt_LoomNo As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbo_KnottingName As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbo_Ends As System.Windows.Forms.ComboBox
End Class
