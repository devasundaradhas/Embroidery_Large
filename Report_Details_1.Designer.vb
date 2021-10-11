<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Report_Details_1
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
        Me.components = New System.ComponentModel.Container()
        Dim ReportDataSource1 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ReportTempBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Report_DataSet = New Billing.Report_DataSet()
        Me.ReportTempTableAdapter = New Billing.Report_DataSetTableAdapters.ReportTempTableAdapter()
        Me.RptViewer = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.pnl_ReportDetails = New System.Windows.Forms.Panel()
        Me.dgv_Report = New System.Windows.Forms.DataGridView()
        Me.cbo_Inputs9 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs9 = New System.Windows.Forms.Label()
        Me.cbo_Inputs8 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs8 = New System.Windows.Forms.Label()
        Me.cbo_Inputs7 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs7 = New System.Windows.Forms.Label()
        Me.cbo_Inputs6 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs6 = New System.Windows.Forms.Label()
        Me.cbo_Inputs5 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs5 = New System.Windows.Forms.Label()
        Me.cbo_Inputs4 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs4 = New System.Windows.Forms.Label()
        Me.cbo_Inputs1 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs1 = New System.Windows.Forms.Label()
        Me.lbl_ReportHeading = New System.Windows.Forms.Label()
        Me.cbo_Inputs3 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs3 = New System.Windows.Forms.Label()
        Me.cbo_Inputs2 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs2 = New System.Windows.Forms.Label()
        Me.dtp_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.lbl_ToDate = New System.Windows.Forms.Label()
        Me.btn_Show = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.dtp_FromDate = New System.Windows.Forms.DateTimePicker()
        Me.lbl_FromDate = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.pnl_ReportInputs = New System.Windows.Forms.Panel()
        CType(Me.ReportTempBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Report_DataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_ReportDetails.SuspendLayout()
        CType(Me.dgv_Report, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        Me.pnl_ReportInputs.SuspendLayout()
        Me.SuspendLayout()
        '
        'ReportTempBindingSource
        '
        Me.ReportTempBindingSource.DataMember = "ReportTemp"
        Me.ReportTempBindingSource.DataSource = Me.Report_DataSet
        '
        'Report_DataSet
        '
        Me.Report_DataSet.DataSetName = "Report_DataSet"
        Me.Report_DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ReportTempTableAdapter
        '
        Me.ReportTempTableAdapter.ClearBeforeFill = True
        '
        'RptViewer
        '
        ReportDataSource1.Name = "DataSet1"
        ReportDataSource1.Value = Nothing
        Me.RptViewer.LocalReport.DataSources.Add(ReportDataSource1)
        Me.RptViewer.LocalReport.ReportEmbeddedResource = "Sizing.Report_EmptyBeam_Receipt_Register.rdlc"
        Me.RptViewer.Location = New System.Drawing.Point(34, 19)
        Me.RptViewer.Name = "RptViewer"
        Me.RptViewer.Size = New System.Drawing.Size(352, 267)
        Me.RptViewer.TabIndex = 1
        '
        'pnl_ReportDetails
        '
        Me.pnl_ReportDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_ReportDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_ReportDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_ReportDetails.Controls.Add(Me.dgv_Report)
        Me.pnl_ReportDetails.Controls.Add(Me.RptViewer)
        Me.pnl_ReportDetails.Location = New System.Drawing.Point(0, 192)
        Me.pnl_ReportDetails.Name = "pnl_ReportDetails"
        Me.pnl_ReportDetails.Size = New System.Drawing.Size(993, 318)
        Me.pnl_ReportDetails.TabIndex = 3
        '
        'dgv_Report
        '
        Me.dgv_Report.AllowUserToAddRows = False
        Me.dgv_Report.AllowUserToDeleteRows = False
        Me.dgv_Report.AllowUserToResizeColumns = False
        Me.dgv_Report.AllowUserToResizeRows = False
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Report.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Report.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Report.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgv_Report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Report.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgv_Report.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Report.Location = New System.Drawing.Point(428, 13)
        Me.dgv_Report.Name = "dgv_Report"
        Me.dgv_Report.ReadOnly = True
        Me.dgv_Report.RowHeadersWidth = 20
        Me.dgv_Report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Report.Size = New System.Drawing.Size(427, 278)
        Me.dgv_Report.StandardTab = True
        Me.dgv_Report.TabIndex = 13
        Me.dgv_Report.TabStop = False
        Me.dgv_Report.Visible = False
        '
        'cbo_Inputs9
        '
        Me.cbo_Inputs9.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs9.FormattingEnabled = True
        Me.cbo_Inputs9.Location = New System.Drawing.Point(521, 155)
        Me.cbo_Inputs9.Name = "cbo_Inputs9"
        Me.cbo_Inputs9.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs9.Sorted = True
        Me.cbo_Inputs9.TabIndex = 23
        Me.cbo_Inputs9.Text = "cbo_Inputs9"
        '
        'lbl_Inputs9
        '
        Me.lbl_Inputs9.AutoSize = True
        Me.lbl_Inputs9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs9.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs9.Location = New System.Drawing.Point(426, 159)
        Me.lbl_Inputs9.Name = "lbl_Inputs9"
        Me.lbl_Inputs9.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs9.TabIndex = 27
        Me.lbl_Inputs9.Text = "lbl_Inputs9"
        '
        'cbo_Inputs8
        '
        Me.cbo_Inputs8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs8.FormattingEnabled = True
        Me.cbo_Inputs8.Location = New System.Drawing.Point(107, 155)
        Me.cbo_Inputs8.MaxDropDownItems = 15
        Me.cbo_Inputs8.Name = "cbo_Inputs8"
        Me.cbo_Inputs8.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs8.Sorted = True
        Me.cbo_Inputs8.TabIndex = 22
        Me.cbo_Inputs8.Text = "cbo_Inputs8"
        '
        'lbl_Inputs8
        '
        Me.lbl_Inputs8.AutoSize = True
        Me.lbl_Inputs8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs8.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs8.Location = New System.Drawing.Point(12, 159)
        Me.lbl_Inputs8.Name = "lbl_Inputs8"
        Me.lbl_Inputs8.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs8.TabIndex = 26
        Me.lbl_Inputs8.Text = "lbl_Inputs8"
        '
        'cbo_Inputs7
        '
        Me.cbo_Inputs7.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs7.FormattingEnabled = True
        Me.cbo_Inputs7.Location = New System.Drawing.Point(521, 127)
        Me.cbo_Inputs7.Name = "cbo_Inputs7"
        Me.cbo_Inputs7.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs7.Sorted = True
        Me.cbo_Inputs7.TabIndex = 21
        Me.cbo_Inputs7.Text = "cbo_Inputs7"
        '
        'lbl_Inputs7
        '
        Me.lbl_Inputs7.AutoSize = True
        Me.lbl_Inputs7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs7.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs7.Location = New System.Drawing.Point(426, 131)
        Me.lbl_Inputs7.Name = "lbl_Inputs7"
        Me.lbl_Inputs7.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs7.TabIndex = 25
        Me.lbl_Inputs7.Text = "lbl_Inputs7"
        '
        'cbo_Inputs6
        '
        Me.cbo_Inputs6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs6.FormattingEnabled = True
        Me.cbo_Inputs6.Location = New System.Drawing.Point(107, 127)
        Me.cbo_Inputs6.MaxDropDownItems = 15
        Me.cbo_Inputs6.Name = "cbo_Inputs6"
        Me.cbo_Inputs6.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs6.Sorted = True
        Me.cbo_Inputs6.TabIndex = 20
        Me.cbo_Inputs6.Text = "cbo_Inputs6"
        '
        'lbl_Inputs6
        '
        Me.lbl_Inputs6.AutoSize = True
        Me.lbl_Inputs6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs6.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs6.Location = New System.Drawing.Point(12, 131)
        Me.lbl_Inputs6.Name = "lbl_Inputs6"
        Me.lbl_Inputs6.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs6.TabIndex = 24
        Me.lbl_Inputs6.Text = "lbl_Inputs6"
        '
        'cbo_Inputs5
        '
        Me.cbo_Inputs5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs5.FormattingEnabled = True
        Me.cbo_Inputs5.Location = New System.Drawing.Point(521, 99)
        Me.cbo_Inputs5.Name = "cbo_Inputs5"
        Me.cbo_Inputs5.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs5.Sorted = True
        Me.cbo_Inputs5.TabIndex = 7
        Me.cbo_Inputs5.Text = "cbo_Inputs5"
        '
        'lbl_Inputs5
        '
        Me.lbl_Inputs5.AutoSize = True
        Me.lbl_Inputs5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs5.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs5.Location = New System.Drawing.Point(426, 103)
        Me.lbl_Inputs5.Name = "lbl_Inputs5"
        Me.lbl_Inputs5.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs5.TabIndex = 19
        Me.lbl_Inputs5.Text = "lbl_Inputs5"
        '
        'cbo_Inputs4
        '
        Me.cbo_Inputs4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs4.FormattingEnabled = True
        Me.cbo_Inputs4.Location = New System.Drawing.Point(107, 99)
        Me.cbo_Inputs4.MaxDropDownItems = 15
        Me.cbo_Inputs4.Name = "cbo_Inputs4"
        Me.cbo_Inputs4.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs4.Sorted = True
        Me.cbo_Inputs4.TabIndex = 5
        Me.cbo_Inputs4.Text = "cbo_Inputs4"
        '
        'lbl_Inputs4
        '
        Me.lbl_Inputs4.AutoSize = True
        Me.lbl_Inputs4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs4.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs4.Location = New System.Drawing.Point(12, 103)
        Me.lbl_Inputs4.Name = "lbl_Inputs4"
        Me.lbl_Inputs4.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs4.TabIndex = 17
        Me.lbl_Inputs4.Text = "lbl_Inputs4"
        '
        'cbo_Inputs1
        '
        Me.cbo_Inputs1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs1.FormattingEnabled = True
        Me.cbo_Inputs1.Location = New System.Drawing.Point(521, 43)
        Me.cbo_Inputs1.MaxDropDownItems = 15
        Me.cbo_Inputs1.Name = "cbo_Inputs1"
        Me.cbo_Inputs1.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs1.Sorted = True
        Me.cbo_Inputs1.TabIndex = 2
        Me.cbo_Inputs1.Text = "cbo_Inputs1"
        '
        'lbl_Inputs1
        '
        Me.lbl_Inputs1.AutoSize = True
        Me.lbl_Inputs1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs1.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs1.Location = New System.Drawing.Point(426, 47)
        Me.lbl_Inputs1.Name = "lbl_Inputs1"
        Me.lbl_Inputs1.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs1.TabIndex = 15
        Me.lbl_Inputs1.Text = "lbl_Inputs1"
        '
        'lbl_ReportHeading
        '
        Me.lbl_ReportHeading.AutoEllipsis = True
        Me.lbl_ReportHeading.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_ReportHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_ReportHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_ReportHeading.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ReportHeading.ForeColor = System.Drawing.Color.White
        Me.lbl_ReportHeading.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lbl_ReportHeading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_ReportHeading.Name = "lbl_ReportHeading"
        Me.lbl_ReportHeading.Size = New System.Drawing.Size(993, 30)
        Me.lbl_ReportHeading.TabIndex = 13
        Me.lbl_ReportHeading.Text = "lbl_ReportHeading"
        Me.lbl_ReportHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbo_Inputs3
        '
        Me.cbo_Inputs3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs3.FormattingEnabled = True
        Me.cbo_Inputs3.Location = New System.Drawing.Point(521, 71)
        Me.cbo_Inputs3.Name = "cbo_Inputs3"
        Me.cbo_Inputs3.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs3.Sorted = True
        Me.cbo_Inputs3.TabIndex = 4
        Me.cbo_Inputs3.Text = "cbo_Inputs3"
        '
        'lbl_Inputs3
        '
        Me.lbl_Inputs3.AutoSize = True
        Me.lbl_Inputs3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs3.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs3.Location = New System.Drawing.Point(426, 75)
        Me.lbl_Inputs3.Name = "lbl_Inputs3"
        Me.lbl_Inputs3.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs3.TabIndex = 12
        Me.lbl_Inputs3.Text = "lbl_Inputs3"
        '
        'cbo_Inputs2
        '
        Me.cbo_Inputs2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs2.FormattingEnabled = True
        Me.cbo_Inputs2.Location = New System.Drawing.Point(107, 71)
        Me.cbo_Inputs2.MaxDropDownItems = 15
        Me.cbo_Inputs2.Name = "cbo_Inputs2"
        Me.cbo_Inputs2.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs2.Sorted = True
        Me.cbo_Inputs2.TabIndex = 3
        Me.cbo_Inputs2.Text = "cbo_Inputs2"
        '
        'lbl_Inputs2
        '
        Me.lbl_Inputs2.AutoSize = True
        Me.lbl_Inputs2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs2.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs2.Location = New System.Drawing.Point(12, 75)
        Me.lbl_Inputs2.Name = "lbl_Inputs2"
        Me.lbl_Inputs2.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs2.TabIndex = 10
        Me.lbl_Inputs2.Text = "lbl_Inputs2"
        '
        'dtp_ToDate
        '
        Me.dtp_ToDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_ToDate.Location = New System.Drawing.Point(305, 43)
        Me.dtp_ToDate.Name = "dtp_ToDate"
        Me.dtp_ToDate.Size = New System.Drawing.Size(99, 23)
        Me.dtp_ToDate.TabIndex = 1
        '
        'lbl_ToDate
        '
        Me.lbl_ToDate.AutoSize = True
        Me.lbl_ToDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ToDate.ForeColor = System.Drawing.Color.Blue
        Me.lbl_ToDate.Location = New System.Drawing.Point(244, 47)
        Me.lbl_ToDate.Name = "lbl_ToDate"
        Me.lbl_ToDate.Size = New System.Drawing.Size(26, 15)
        Me.lbl_ToDate.TabIndex = 9
        Me.lbl_ToDate.Text = "To :"
        '
        'btn_Show
        '
        Me.btn_Show.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Show.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Show.ForeColor = System.Drawing.Color.Navy
        Me.btn_Show.Location = New System.Drawing.Point(845, 41)
        Me.btn_Show.Name = "btn_Show"
        Me.btn_Show.Size = New System.Drawing.Size(52, 27)
        Me.btn_Show.TabIndex = 8
        Me.btn_Show.TabStop = False
        Me.btn_Show.Text = "&Show"
        Me.btn_Show.UseVisualStyleBackColor = True
        '
        'btn_Close
        '
        Me.btn_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.Navy
        Me.btn_Close.Location = New System.Drawing.Point(908, 41)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(52, 27)
        Me.btn_Close.TabIndex = 9
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&Close"
        Me.btn_Close.UseVisualStyleBackColor = True
        '
        'dtp_FromDate
        '
        Me.dtp_FromDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_FromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_FromDate.Location = New System.Drawing.Point(107, 43)
        Me.dtp_FromDate.Name = "dtp_FromDate"
        Me.dtp_FromDate.Size = New System.Drawing.Size(99, 23)
        Me.dtp_FromDate.TabIndex = 0
        '
        'lbl_FromDate
        '
        Me.lbl_FromDate.AutoSize = True
        Me.lbl_FromDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FromDate.ForeColor = System.Drawing.Color.Blue
        Me.lbl_FromDate.Location = New System.Drawing.Point(12, 47)
        Me.lbl_FromDate.Name = "lbl_FromDate"
        Me.lbl_FromDate.Size = New System.Drawing.Size(69, 15)
        Me.lbl_FromDate.TabIndex = 5
        Me.lbl_FromDate.Text = "Date From:"
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.pnl_ReportInputs)
        Me.pnl_Back.Controls.Add(Me.pnl_ReportDetails)
        Me.pnl_Back.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Back.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(998, 515)
        Me.pnl_Back.TabIndex = 4
        '
        'pnl_ReportInputs
        '
        Me.pnl_ReportInputs.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_ReportInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs9)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs9)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs8)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs8)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs7)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs7)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs6)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs6)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs5)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs5)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs4)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs4)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs1)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs1)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_ReportHeading)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs3)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs3)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs2)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs2)
        Me.pnl_ReportInputs.Controls.Add(Me.dtp_ToDate)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_ToDate)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Show)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Close)
        Me.pnl_ReportInputs.Controls.Add(Me.dtp_FromDate)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_FromDate)
        Me.pnl_ReportInputs.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_ReportInputs.Location = New System.Drawing.Point(0, 0)
        Me.pnl_ReportInputs.Name = "pnl_ReportInputs"
        Me.pnl_ReportInputs.Size = New System.Drawing.Size(995, 190)
        Me.pnl_ReportInputs.TabIndex = 2
        '
        'Report_Details_1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(998, 515)
        Me.Controls.Add(Me.pnl_Back)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "Report_Details_1"
        Me.Text = "Report_Details"
        CType(Me.ReportTempBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Report_DataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_ReportDetails.ResumeLayout(False)
        CType(Me.dgv_Report, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_ReportInputs.ResumeLayout(False)
        Me.pnl_ReportInputs.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ReportTempBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents Report_DataSet As Billing.Report_DataSet
    Friend WithEvents ReportTempTableAdapter As Billing.Report_DataSetTableAdapters.ReportTempTableAdapter
    Friend WithEvents RptViewer As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents pnl_ReportDetails As Panel
    Friend WithEvents dgv_Report As DataGridView
    Friend WithEvents cbo_Inputs9 As ComboBox
    Friend WithEvents lbl_Inputs9 As Label
    Friend WithEvents cbo_Inputs8 As ComboBox
    Friend WithEvents lbl_Inputs8 As Label
    Friend WithEvents cbo_Inputs7 As ComboBox
    Friend WithEvents lbl_Inputs7 As Label
    Public WithEvents cbo_Inputs6 As ComboBox
    Friend WithEvents lbl_Inputs6 As Label
    Friend WithEvents cbo_Inputs5 As ComboBox
    Friend WithEvents lbl_Inputs5 As Label
    Friend WithEvents cbo_Inputs4 As ComboBox
    Friend WithEvents lbl_Inputs4 As Label
    Friend WithEvents cbo_Inputs1 As ComboBox
    Friend WithEvents lbl_Inputs1 As Label
    Friend WithEvents lbl_ReportHeading As Label
    Friend WithEvents cbo_Inputs3 As ComboBox
    Friend WithEvents lbl_Inputs3 As Label
    Public WithEvents cbo_Inputs2 As ComboBox
    Friend WithEvents lbl_Inputs2 As Label
    Friend WithEvents dtp_ToDate As DateTimePicker
    Friend WithEvents lbl_ToDate As Label
    Friend WithEvents btn_Show As Button
    Friend WithEvents btn_Close As Button
    Friend WithEvents dtp_FromDate As DateTimePicker
    Friend WithEvents lbl_FromDate As Label
    Friend WithEvents pnl_Back As Panel
    Friend WithEvents pnl_ReportInputs As Panel
End Class
