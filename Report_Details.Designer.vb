<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Report_Details
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
        Me.ReportTempBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.InventoryDataSet = New Billing.Report_DataSet()
        Me.ReportTempTableAdapter = New Billing.Report_DataSetTableAdapters.ReportTempTableAdapter()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.pnl_ReportDetails = New System.Windows.Forms.Panel()
        Me.RptViewer = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.pnl_ReportInputs = New System.Windows.Forms.Panel()
        Me.Cbo_SalesMan = New System.Windows.Forms.ComboBox()
        Me.lbl_Salesman = New System.Windows.Forms.Label()
        Me.cbo_Transport = New System.Windows.Forms.ComboBox()
        Me.lbl_Transport = New System.Windows.Forms.Label()
        Me.cbo_Agent = New System.Windows.Forms.ComboBox()
        Me.lbl_Agent = New System.Windows.Forms.Label()
        Me.cbo_ItemGroupName = New System.Windows.Forms.ComboBox()
        Me.lbl_ItemGroupName = New System.Windows.Forms.Label()
        Me.cbo_SizeName = New System.Windows.Forms.ComboBox()
        Me.lbl_SizeName = New System.Windows.Forms.Label()
        Me.txt_Inputs1 = New System.Windows.Forms.TextBox()
        Me.lbl_TextInputs1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cbo_SerialNo = New System.Windows.Forms.ComboBox()
        Me.lbl_SerialNo = New System.Windows.Forms.Label()
        Me.cbo_PhoneNo = New System.Windows.Forms.ComboBox()
        Me.lbl_PhoneNo = New System.Windows.Forms.Label()
        Me.cbo_GroupName = New System.Windows.Forms.ComboBox()
        Me.lbl_GroupName = New System.Windows.Forms.Label()
        Me.cbo_Company = New System.Windows.Forms.ComboBox()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.lbl_ReportHeading = New System.Windows.Forms.Label()
        Me.cbo_ItemName = New System.Windows.Forms.ComboBox()
        Me.lbl_ItemName = New System.Windows.Forms.Label()
        Me.cbo_Ledger = New System.Windows.Forms.ComboBox()
        Me.lbl_Ledger = New System.Windows.Forms.Label()
        Me.dtp_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.lbl_ToDate = New System.Windows.Forms.Label()
        Me.btn_Show = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.dtp_FromDate = New System.Windows.Forms.DateTimePicker()
        Me.lbl_FromDate = New System.Windows.Forms.Label()
        CType(Me.ReportTempBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        Me.pnl_ReportDetails.SuspendLayout()
        Me.pnl_ReportInputs.SuspendLayout()
        Me.SuspendLayout()
        '
        'ReportTempBindingSource
        '
        Me.ReportTempBindingSource.DataMember = "ReportTemp"
        Me.ReportTempBindingSource.DataSource = Me.InventoryDataSet
        '
        'InventoryDataSet
        '
        Me.InventoryDataSet.DataSetName = "InventoryDataSet"
        Me.InventoryDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ReportTempTableAdapter
        '
        Me.ReportTempTableAdapter.ClearBeforeFill = True
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.pnl_ReportDetails)
        Me.pnl_Back.Controls.Add(Me.pnl_ReportInputs)
        Me.pnl_Back.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Back.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(960, 537)
        Me.pnl_Back.TabIndex = 3
        '
        'pnl_ReportDetails
        '
        Me.pnl_ReportDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_ReportDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_ReportDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_ReportDetails.Controls.Add(Me.RptViewer)
        Me.pnl_ReportDetails.Location = New System.Drawing.Point(1, 122)
        Me.pnl_ReportDetails.Name = "pnl_ReportDetails"
        Me.pnl_ReportDetails.Size = New System.Drawing.Size(958, 415)
        Me.pnl_ReportDetails.TabIndex = 3
        '
        'RptViewer
        '
        Me.RptViewer.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ReportDataSource1.Name = "DataSet1"
        ReportDataSource1.Value = Me.ReportTempBindingSource
        Me.RptViewer.LocalReport.DataSources.Add(ReportDataSource1)
        Me.RptViewer.LocalReport.ReportEmbeddedResource = "Inventory.Report1.rdlc"
        Me.RptViewer.Location = New System.Drawing.Point(88, 13)
        Me.RptViewer.Name = "RptViewer"
        Me.RptViewer.Size = New System.Drawing.Size(774, 374)
        Me.RptViewer.TabIndex = 12
        '
        'pnl_ReportInputs
        '
        Me.pnl_ReportInputs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_ReportInputs.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_ReportInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_ReportInputs.Controls.Add(Me.Cbo_SalesMan)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Salesman)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Transport)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Transport)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Agent)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Agent)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_ItemGroupName)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_ItemGroupName)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_SizeName)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_SizeName)
        Me.pnl_ReportInputs.Controls.Add(Me.txt_Inputs1)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_TextInputs1)
        Me.pnl_ReportInputs.Controls.Add(Me.Button1)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_SerialNo)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_SerialNo)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_PhoneNo)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_PhoneNo)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_GroupName)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_GroupName)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Company)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Company)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_ReportHeading)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_ItemName)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_ItemName)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Ledger)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Ledger)
        Me.pnl_ReportInputs.Controls.Add(Me.dtp_ToDate)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_ToDate)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Show)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Close)
        Me.pnl_ReportInputs.Controls.Add(Me.dtp_FromDate)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_FromDate)
        Me.pnl_ReportInputs.Location = New System.Drawing.Point(0, 0)
        Me.pnl_ReportInputs.Name = "pnl_ReportInputs"
        Me.pnl_ReportInputs.Size = New System.Drawing.Size(958, 124)
        Me.pnl_ReportInputs.TabIndex = 2
        '
        'Cbo_SalesMan
        '
        Me.Cbo_SalesMan.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cbo_SalesMan.FormattingEnabled = True
        Me.Cbo_SalesMan.Location = New System.Drawing.Point(107, 212)
        Me.Cbo_SalesMan.MaxDropDownItems = 15
        Me.Cbo_SalesMan.Name = "Cbo_SalesMan"
        Me.Cbo_SalesMan.Size = New System.Drawing.Size(297, 23)
        Me.Cbo_SalesMan.Sorted = True
        Me.Cbo_SalesMan.TabIndex = 35
        Me.Cbo_SalesMan.Text = "cbo_Sales"
        Me.Cbo_SalesMan.UseWaitCursor = True
        '
        'lbl_Salesman
        '
        Me.lbl_Salesman.AutoSize = True
        Me.lbl_Salesman.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Salesman.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Salesman.Location = New System.Drawing.Point(12, 216)
        Me.lbl_Salesman.Name = "lbl_Salesman"
        Me.lbl_Salesman.Size = New System.Drawing.Size(65, 15)
        Me.lbl_Salesman.TabIndex = 36
        Me.lbl_Salesman.Text = "SalesMan :"
        '
        'cbo_Transport
        '
        Me.cbo_Transport.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Transport.FormattingEnabled = True
        Me.cbo_Transport.Location = New System.Drawing.Point(521, 183)
        Me.cbo_Transport.MaxDropDownItems = 15
        Me.cbo_Transport.Name = "cbo_Transport"
        Me.cbo_Transport.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Transport.Sorted = True
        Me.cbo_Transport.TabIndex = 32
        Me.cbo_Transport.Text = "cbo_Transport"
        '
        'lbl_Transport
        '
        Me.lbl_Transport.AutoSize = True
        Me.lbl_Transport.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Transport.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Transport.Location = New System.Drawing.Point(426, 187)
        Me.lbl_Transport.Name = "lbl_Transport"
        Me.lbl_Transport.Size = New System.Drawing.Size(66, 15)
        Me.lbl_Transport.TabIndex = 34
        Me.lbl_Transport.Text = "Transport :"
        '
        'cbo_Agent
        '
        Me.cbo_Agent.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Agent.FormattingEnabled = True
        Me.cbo_Agent.Location = New System.Drawing.Point(107, 183)
        Me.cbo_Agent.MaxDropDownItems = 15
        Me.cbo_Agent.Name = "cbo_Agent"
        Me.cbo_Agent.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Agent.Sorted = True
        Me.cbo_Agent.TabIndex = 31
        Me.cbo_Agent.Text = "cbo_Agent"
        '
        'lbl_Agent
        '
        Me.lbl_Agent.AutoSize = True
        Me.lbl_Agent.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Agent.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Agent.Location = New System.Drawing.Point(12, 187)
        Me.lbl_Agent.Name = "lbl_Agent"
        Me.lbl_Agent.Size = New System.Drawing.Size(83, 15)
        Me.lbl_Agent.TabIndex = 33
        Me.lbl_Agent.Text = "Agent Name :"
        '
        'cbo_ItemGroupName
        '
        Me.cbo_ItemGroupName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ItemGroupName.FormattingEnabled = True
        Me.cbo_ItemGroupName.Location = New System.Drawing.Point(521, 154)
        Me.cbo_ItemGroupName.MaxDropDownItems = 15
        Me.cbo_ItemGroupName.Name = "cbo_ItemGroupName"
        Me.cbo_ItemGroupName.Size = New System.Drawing.Size(297, 23)
        Me.cbo_ItemGroupName.Sorted = True
        Me.cbo_ItemGroupName.TabIndex = 9
        Me.cbo_ItemGroupName.Text = "cbo_ItemGroupName"
        '
        'lbl_ItemGroupName
        '
        Me.lbl_ItemGroupName.AutoSize = True
        Me.lbl_ItemGroupName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ItemGroupName.ForeColor = System.Drawing.Color.Blue
        Me.lbl_ItemGroupName.Location = New System.Drawing.Point(426, 158)
        Me.lbl_ItemGroupName.Name = "lbl_ItemGroupName"
        Me.lbl_ItemGroupName.Size = New System.Drawing.Size(77, 15)
        Me.lbl_ItemGroupName.TabIndex = 30
        Me.lbl_ItemGroupName.Text = "Item Group :"
        '
        'cbo_SizeName
        '
        Me.cbo_SizeName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_SizeName.FormattingEnabled = True
        Me.cbo_SizeName.Location = New System.Drawing.Point(107, 154)
        Me.cbo_SizeName.MaxDropDownItems = 15
        Me.cbo_SizeName.Name = "cbo_SizeName"
        Me.cbo_SizeName.Size = New System.Drawing.Size(297, 23)
        Me.cbo_SizeName.Sorted = True
        Me.cbo_SizeName.TabIndex = 8
        Me.cbo_SizeName.Text = "cbo_SizeName"
        '
        'lbl_SizeName
        '
        Me.lbl_SizeName.AutoSize = True
        Me.lbl_SizeName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SizeName.ForeColor = System.Drawing.Color.Blue
        Me.lbl_SizeName.Location = New System.Drawing.Point(12, 158)
        Me.lbl_SizeName.Name = "lbl_SizeName"
        Me.lbl_SizeName.Size = New System.Drawing.Size(71, 15)
        Me.lbl_SizeName.TabIndex = 28
        Me.lbl_SizeName.Text = "Size Name :"
        '
        'txt_Inputs1
        '
        Me.txt_Inputs1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Inputs1.Location = New System.Drawing.Point(521, 125)
        Me.txt_Inputs1.Name = "txt_Inputs1"
        Me.txt_Inputs1.Size = New System.Drawing.Size(297, 23)
        Me.txt_Inputs1.TabIndex = 7
        Me.txt_Inputs1.Text = "txt_Inputs1"
        '
        'lbl_TextInputs1
        '
        Me.lbl_TextInputs1.AutoSize = True
        Me.lbl_TextInputs1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_TextInputs1.ForeColor = System.Drawing.Color.Blue
        Me.lbl_TextInputs1.Location = New System.Drawing.Point(426, 129)
        Me.lbl_TextInputs1.Name = "lbl_TextInputs1"
        Me.lbl_TextInputs1.Size = New System.Drawing.Size(70, 15)
        Me.lbl_TextInputs1.TabIndex = 26
        Me.lbl_TextInputs1.Text = "Day Range :"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(856, 114)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 30)
        Me.Button1.TabIndex = 24
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'cbo_SerialNo
        '
        Me.cbo_SerialNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_SerialNo.FormattingEnabled = True
        Me.cbo_SerialNo.Location = New System.Drawing.Point(107, 125)
        Me.cbo_SerialNo.MaxDropDownItems = 15
        Me.cbo_SerialNo.Name = "cbo_SerialNo"
        Me.cbo_SerialNo.Size = New System.Drawing.Size(297, 23)
        Me.cbo_SerialNo.Sorted = True
        Me.cbo_SerialNo.TabIndex = 6
        Me.cbo_SerialNo.Text = "cbo_SerialNo"
        '
        'lbl_SerialNo
        '
        Me.lbl_SerialNo.AutoSize = True
        Me.lbl_SerialNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SerialNo.ForeColor = System.Drawing.Color.Blue
        Me.lbl_SerialNo.Location = New System.Drawing.Point(12, 129)
        Me.lbl_SerialNo.Name = "lbl_SerialNo"
        Me.lbl_SerialNo.Size = New System.Drawing.Size(66, 15)
        Me.lbl_SerialNo.TabIndex = 23
        Me.lbl_SerialNo.Text = "Serial No. :"
        '
        'cbo_PhoneNo
        '
        Me.cbo_PhoneNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_PhoneNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_PhoneNo.FormattingEnabled = True
        Me.cbo_PhoneNo.Location = New System.Drawing.Point(521, 99)
        Me.cbo_PhoneNo.Name = "cbo_PhoneNo"
        Me.cbo_PhoneNo.Size = New System.Drawing.Size(297, 23)
        Me.cbo_PhoneNo.Sorted = True
        Me.cbo_PhoneNo.TabIndex = 7
        Me.cbo_PhoneNo.Text = "cbo_PhoneNo"
        '
        'lbl_PhoneNo
        '
        Me.lbl_PhoneNo.AutoSize = True
        Me.lbl_PhoneNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_PhoneNo.ForeColor = System.Drawing.Color.Blue
        Me.lbl_PhoneNo.Location = New System.Drawing.Point(426, 103)
        Me.lbl_PhoneNo.Name = "lbl_PhoneNo"
        Me.lbl_PhoneNo.Size = New System.Drawing.Size(71, 15)
        Me.lbl_PhoneNo.TabIndex = 19
        Me.lbl_PhoneNo.Text = "Phone No. :"
        '
        'cbo_GroupName
        '
        Me.cbo_GroupName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_GroupName.FormattingEnabled = True
        Me.cbo_GroupName.Location = New System.Drawing.Point(107, 99)
        Me.cbo_GroupName.MaxDropDownItems = 15
        Me.cbo_GroupName.Name = "cbo_GroupName"
        Me.cbo_GroupName.Size = New System.Drawing.Size(297, 23)
        Me.cbo_GroupName.Sorted = True
        Me.cbo_GroupName.TabIndex = 5
        Me.cbo_GroupName.Text = "cbo_GroupName"
        '
        'lbl_GroupName
        '
        Me.lbl_GroupName.AutoSize = True
        Me.lbl_GroupName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_GroupName.ForeColor = System.Drawing.Color.Blue
        Me.lbl_GroupName.Location = New System.Drawing.Point(12, 103)
        Me.lbl_GroupName.Name = "lbl_GroupName"
        Me.lbl_GroupName.Size = New System.Drawing.Size(84, 15)
        Me.lbl_GroupName.TabIndex = 17
        Me.lbl_GroupName.Text = "Group Name :"
        '
        'cbo_Company
        '
        Me.cbo_Company.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Company.FormattingEnabled = True
        Me.cbo_Company.Location = New System.Drawing.Point(521, 43)
        Me.cbo_Company.MaxDropDownItems = 15
        Me.cbo_Company.Name = "cbo_Company"
        Me.cbo_Company.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Company.Sorted = True
        Me.cbo_Company.TabIndex = 2
        Me.cbo_Company.Text = "cbo_Company"
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Company.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Company.Location = New System.Drawing.Point(426, 47)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(65, 15)
        Me.lbl_Company.TabIndex = 15
        Me.lbl_Company.Text = "Company :"
        '
        'lbl_ReportHeading
        '
        Me.lbl_ReportHeading.AutoEllipsis = True
        Me.lbl_ReportHeading.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.lbl_ReportHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_ReportHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_ReportHeading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ReportHeading.ForeColor = System.Drawing.Color.White
        Me.lbl_ReportHeading.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lbl_ReportHeading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_ReportHeading.Name = "lbl_ReportHeading"
        Me.lbl_ReportHeading.Size = New System.Drawing.Size(956, 34)
        Me.lbl_ReportHeading.TabIndex = 13
        Me.lbl_ReportHeading.Text = "lbl_ReportHeading"
        Me.lbl_ReportHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbo_ItemName
        '
        Me.cbo_ItemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_ItemName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ItemName.FormattingEnabled = True
        Me.cbo_ItemName.Location = New System.Drawing.Point(521, 71)
        Me.cbo_ItemName.Name = "cbo_ItemName"
        Me.cbo_ItemName.Size = New System.Drawing.Size(297, 23)
        Me.cbo_ItemName.Sorted = True
        Me.cbo_ItemName.TabIndex = 4
        Me.cbo_ItemName.Text = "cbo_ItemName"
        '
        'lbl_ItemName
        '
        Me.lbl_ItemName.AutoSize = True
        Me.lbl_ItemName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ItemName.ForeColor = System.Drawing.Color.Blue
        Me.lbl_ItemName.Location = New System.Drawing.Point(426, 75)
        Me.lbl_ItemName.Name = "lbl_ItemName"
        Me.lbl_ItemName.Size = New System.Drawing.Size(76, 15)
        Me.lbl_ItemName.TabIndex = 12
        Me.lbl_ItemName.Text = "Item Name :"
        '
        'cbo_Ledger
        '
        Me.cbo_Ledger.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Ledger.FormattingEnabled = True
        Me.cbo_Ledger.Location = New System.Drawing.Point(107, 71)
        Me.cbo_Ledger.MaxDropDownItems = 15
        Me.cbo_Ledger.Name = "cbo_Ledger"
        Me.cbo_Ledger.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Ledger.Sorted = True
        Me.cbo_Ledger.TabIndex = 3
        Me.cbo_Ledger.Text = "cbo_Ledger"
        '
        'lbl_Ledger
        '
        Me.lbl_Ledger.AutoSize = True
        Me.lbl_Ledger.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Ledger.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Ledger.Location = New System.Drawing.Point(12, 75)
        Me.lbl_Ledger.Name = "lbl_Ledger"
        Me.lbl_Ledger.Size = New System.Drawing.Size(79, 15)
        Me.lbl_Ledger.TabIndex = 10
        Me.lbl_Ledger.Text = "Party Name :"
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
        Me.btn_Show.Image = Global.Billing.My.Resources.Resources.New1
        Me.btn_Show.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Show.Location = New System.Drawing.Point(855, 41)
        Me.btn_Show.Name = "btn_Show"
        Me.btn_Show.Size = New System.Drawing.Size(82, 27)
        Me.btn_Show.TabIndex = 10
        Me.btn_Show.TabStop = False
        Me.btn_Show.Text = "&Show"
        Me.btn_Show.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Show.UseVisualStyleBackColor = True
        '
        'btn_Close
        '
        Me.btn_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.Navy
        Me.btn_Close.Image = Global.Billing.My.Resources.Resources.Close1
        Me.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Close.Location = New System.Drawing.Point(855, 81)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(82, 27)
        Me.btn_Close.TabIndex = 11
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "Close"
        Me.btn_Close.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        'Report_Details
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 537)
        Me.Controls.Add(Me.pnl_Back)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "Report_Details"
        Me.Text = "Report_Details"
        CType(Me.ReportTempBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_ReportDetails.ResumeLayout(False)
        Me.pnl_ReportInputs.ResumeLayout(False)
        Me.pnl_ReportInputs.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ReportTempBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents InventoryDataSet As Billing.Report_DataSet
    Friend WithEvents ReportTempTableAdapter As Billing.Report_DataSetTableAdapters.ReportTempTableAdapter
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents pnl_ReportDetails As System.Windows.Forms.Panel
    Friend WithEvents RptViewer As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents pnl_ReportInputs As System.Windows.Forms.Panel
    Friend WithEvents dtp_FromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_FromDate As System.Windows.Forms.Label
    Friend WithEvents btn_Show As System.Windows.Forms.Button
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents dtp_ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_ToDate As System.Windows.Forms.Label
    Friend WithEvents cbo_Ledger As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Ledger As System.Windows.Forms.Label
    Friend WithEvents lbl_ItemName As System.Windows.Forms.Label
    Friend WithEvents cbo_ItemName As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Company As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents cbo_GroupName As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_GroupName As System.Windows.Forms.Label
    Friend WithEvents cbo_PhoneNo As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_PhoneNo As System.Windows.Forms.Label
    Friend WithEvents lbl_ReportHeading As System.Windows.Forms.Label
    Friend WithEvents cbo_SerialNo As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_SerialNo As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents txt_Inputs1 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TextInputs1 As System.Windows.Forms.Label
    Friend WithEvents cbo_SizeName As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_SizeName As System.Windows.Forms.Label
    Friend WithEvents cbo_ItemGroupName As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_ItemGroupName As System.Windows.Forms.Label
    Friend WithEvents cbo_Transport As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Transport As System.Windows.Forms.Label
    Friend WithEvents cbo_Agent As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Agent As System.Windows.Forms.Label
    Friend WithEvents Cbo_SalesMan As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Salesman As System.Windows.Forms.Label
End Class
