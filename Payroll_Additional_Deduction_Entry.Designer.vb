<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Payroll_Additional_Deduction_Entry
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cbo_EmployeeFilter = New System.Windows.Forms.ComboBox()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.txt_OtherDeduction = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_OtherAddition = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_Store = New System.Windows.Forms.TextBox()
        Me.lbl_Store_Caption = New System.Windows.Forms.Label()
        Me.txt_Medical = New System.Windows.Forms.TextBox()
        Me.lbl_Medical_Caption = New System.Windows.Forms.Label()
        Me.txt_Mess = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txt_Amount = New System.Windows.Forms.TextBox()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbl_Refno = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_remarks = New System.Windows.Forms.TextBox()
        Me.cbo_EmployeeName = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.pnl_back.SuspendLayout()
        Me.pnl_filter.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cbo_EmployeeFilter
        '
        Me.cbo_EmployeeFilter.FormattingEnabled = True
        Me.cbo_EmployeeFilter.Location = New System.Drawing.Point(115, 71)
        Me.cbo_EmployeeFilter.MaxLength = 35
        Me.cbo_EmployeeFilter.Name = "cbo_EmployeeFilter"
        Me.cbo_EmployeeFilter.Size = New System.Drawing.Size(279, 23)
        Me.cbo_EmployeeFilter.TabIndex = 2
        Me.cbo_EmployeeFilter.Text = "cbo_EmployeeFilter"
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.txt_OtherDeduction)
        Me.pnl_back.Controls.Add(Me.Label13)
        Me.pnl_back.Controls.Add(Me.txt_OtherAddition)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.txt_Store)
        Me.pnl_back.Controls.Add(Me.lbl_Store_Caption)
        Me.pnl_back.Controls.Add(Me.txt_Medical)
        Me.pnl_back.Controls.Add(Me.lbl_Medical_Caption)
        Me.pnl_back.Controls.Add(Me.txt_Mess)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.txt_Amount)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.Label12)
        Me.pnl_back.Controls.Add(Me.lbl_Refno)
        Me.pnl_back.Controls.Add(Me.lbl_Company)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.dtp_Date)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.txt_remarks)
        Me.pnl_back.Controls.Add(Me.cbo_EmployeeName)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(691, 308)
        Me.pnl_back.TabIndex = 34
        '
        'txt_OtherDeduction
        '
        Me.txt_OtherDeduction.Location = New System.Drawing.Point(132, 178)
        Me.txt_OtherDeduction.MaxLength = 20
        Me.txt_OtherDeduction.Name = "txt_OtherDeduction"
        Me.txt_OtherDeduction.ReadOnly = True
        Me.txt_OtherDeduction.Size = New System.Drawing.Size(204, 23)
        Me.txt_OtherDeduction.TabIndex = 6
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Blue
        Me.Label13.Location = New System.Drawing.Point(16, 181)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(99, 15)
        Me.Label13.TabIndex = 31
        Me.Label13.Text = "Other Deduction"
        '
        'txt_OtherAddition
        '
        Me.txt_OtherAddition.Location = New System.Drawing.Point(463, 178)
        Me.txt_OtherAddition.MaxLength = 20
        Me.txt_OtherAddition.Name = "txt_OtherAddition"
        Me.txt_OtherAddition.ReadOnly = True
        Me.txt_OtherAddition.Size = New System.Drawing.Size(207, 23)
        Me.txt_OtherAddition.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(360, 181)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(90, 15)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "Other Addition"
        '
        'txt_Store
        '
        Me.txt_Store.Location = New System.Drawing.Point(463, 141)
        Me.txt_Store.MaxLength = 20
        Me.txt_Store.Name = "txt_Store"
        Me.txt_Store.ReadOnly = True
        Me.txt_Store.Size = New System.Drawing.Size(207, 23)
        Me.txt_Store.TabIndex = 5
        '
        'lbl_Store_Caption
        '
        Me.lbl_Store_Caption.AutoSize = True
        Me.lbl_Store_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Store_Caption.Location = New System.Drawing.Point(360, 145)
        Me.lbl_Store_Caption.Name = "lbl_Store_Caption"
        Me.lbl_Store_Caption.Size = New System.Drawing.Size(37, 15)
        Me.lbl_Store_Caption.TabIndex = 27
        Me.lbl_Store_Caption.Text = "Store"
        '
        'txt_Medical
        '
        Me.txt_Medical.Location = New System.Drawing.Point(132, 141)
        Me.txt_Medical.MaxLength = 20
        Me.txt_Medical.Name = "txt_Medical"
        Me.txt_Medical.ReadOnly = True
        Me.txt_Medical.Size = New System.Drawing.Size(204, 23)
        Me.txt_Medical.TabIndex = 4
        '
        'lbl_Medical_Caption
        '
        Me.lbl_Medical_Caption.AutoSize = True
        Me.lbl_Medical_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Medical_Caption.Location = New System.Drawing.Point(16, 145)
        Me.lbl_Medical_Caption.Name = "lbl_Medical_Caption"
        Me.lbl_Medical_Caption.Size = New System.Drawing.Size(49, 15)
        Me.lbl_Medical_Caption.TabIndex = 25
        Me.lbl_Medical_Caption.Text = "Medical"
        '
        'txt_Mess
        '
        Me.txt_Mess.Location = New System.Drawing.Point(463, 102)
        Me.txt_Mess.MaxLength = 20
        Me.txt_Mess.Name = "txt_Mess"
        Me.txt_Mess.Size = New System.Drawing.Size(207, 23)
        Me.txt_Mess.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(360, 106)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 15)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Mess"
        '
        'txt_Amount
        '
        Me.txt_Amount.Location = New System.Drawing.Point(132, 102)
        Me.txt_Amount.MaxLength = 20
        Me.txt_Amount.Name = "txt_Amount"
        Me.txt_Amount.ReadOnly = True
        Me.txt_Amount.Size = New System.Drawing.Size(204, 23)
        Me.txt_Amount.TabIndex = 2
        Me.txt_Amount.Text = "txt_Amount"
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(587, 258)
        Me.btn_close.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(83, 30)
        Me.btn_close.TabIndex = 10
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Blue
        Me.Label12.Location = New System.Drawing.Point(16, 106)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(112, 15)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Advance Deduction" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lbl_Refno
        '
        Me.lbl_Refno.BackColor = System.Drawing.Color.White
        Me.lbl_Refno.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_Refno.Location = New System.Drawing.Point(132, 24)
        Me.lbl_Refno.Name = "lbl_Refno"
        Me.lbl_Refno.Size = New System.Drawing.Size(204, 23)
        Me.lbl_Refno.TabIndex = 21
        Me.lbl_Refno.Text = "lbl_RefNo"
        Me.lbl_Refno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lbl_Company.Location = New System.Drawing.Point(129, 271)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 19
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(481, 258)
        Me.btn_save.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(83, 30)
        Me.btn_save.TabIndex = 9
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'dtp_Date
        '
        Me.dtp_Date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_Date.Location = New System.Drawing.Point(463, 24)
        Me.dtp_Date.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(207, 23)
        Me.dtp_Date.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(16, 67)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(97, 15)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Employee Name"
        '
        'txt_remarks
        '
        Me.txt_remarks.Location = New System.Drawing.Point(134, 219)
        Me.txt_remarks.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txt_remarks.MaxLength = 100
        Me.txt_remarks.Name = "txt_remarks"
        Me.txt_remarks.Size = New System.Drawing.Size(536, 23)
        Me.txt_remarks.TabIndex = 8
        Me.txt_remarks.Text = "txt_Remarks"
        '
        'cbo_EmployeeName
        '
        Me.cbo_EmployeeName.FormattingEnabled = True
        Me.cbo_EmployeeName.Location = New System.Drawing.Point(132, 63)
        Me.cbo_EmployeeName.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.cbo_EmployeeName.MaxLength = 35
        Me.cbo_EmployeeName.Name = "cbo_EmployeeName"
        Me.cbo_EmployeeName.Size = New System.Drawing.Size(538, 23)
        Me.cbo_EmployeeName.TabIndex = 1
        Me.cbo_EmployeeName.Text = "cbo_EmployeeName"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(16, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ref.No"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 300)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 15)
        Me.Label7.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(360, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(16, 226)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 15)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Remarks"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 133)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.ForeColor = System.Drawing.Color.Blue
        Me.Label20.Location = New System.Drawing.Point(12, 76)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(97, 15)
        Me.Label20.TabIndex = 9
        Me.Label20.Text = "Employee Name"
        '
        'pnl_filter
        '
        Me.pnl_filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_filter.Controls.Add(Me.cbo_EmployeeFilter)
        Me.pnl_filter.Controls.Add(Me.Label20)
        Me.pnl_filter.Controls.Add(Me.btn_closefilter)
        Me.pnl_filter.Controls.Add(Me.Label17)
        Me.pnl_filter.Controls.Add(Me.btn_filtershow)
        Me.pnl_filter.Controls.Add(Me.dgv_filter)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterTo_date)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterFrom_date)
        Me.pnl_filter.Controls.Add(Me.Label18)
        Me.pnl_filter.Controls.Add(Me.Label19)
        Me.pnl_filter.Location = New System.Drawing.Point(25, 395)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(599, 284)
        Me.pnl_filter.TabIndex = 36
        '
        'btn_closefilter
        '
        Me.btn_closefilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_closefilter.ForeColor = System.Drawing.Color.White
        Me.btn_closefilter.Location = New System.Drawing.Point(491, 40)
        Me.btn_closefilter.Name = "btn_closefilter"
        Me.btn_closefilter.Size = New System.Drawing.Size(75, 52)
        Me.btn_closefilter.TabIndex = 4
        Me.btn_closefilter.Text = "&CLOSE"
        Me.btn_closefilter.UseVisualStyleBackColor = False
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label17.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(-1, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(599, 30)
        Me.Label17.TabIndex = 8
        Me.Label17.Text = "FILTER"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_filtershow
        '
        Me.btn_filtershow.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_filtershow.ForeColor = System.Drawing.Color.White
        Me.btn_filtershow.Location = New System.Drawing.Point(410, 40)
        Me.btn_filtershow.Name = "btn_filtershow"
        Me.btn_filtershow.Size = New System.Drawing.Size(75, 52)
        Me.btn_filtershow.TabIndex = 3
        Me.btn_filtershow.Text = "SHOW"
        Me.btn_filtershow.UseVisualStyleBackColor = False
        '
        'dgv_filter
        '
        Me.dgv_filter.AllowUserToAddRows = False
        Me.dgv_filter.AllowUserToDeleteRows = False
        Me.dgv_filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dc, Me.Column1, Me.Column2, Me.Column5})
        Me.dgv_filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_filter.Location = New System.Drawing.Point(15, 102)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(573, 161)
        Me.dgv_filter.TabIndex = 5
        '
        'dc
        '
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dc.DefaultCellStyle = DataGridViewCellStyle1
        Me.dc.HeaderText = "REF.NO"
        Me.dc.MaxInputLength = 8
        Me.dc.Name = "dc"
        Me.dc.ReadOnly = True
        Me.dc.Width = 70
        '
        'Column1
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "DATE"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 80
        '
        'Column2
        '
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "EMPLOYEE NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 250
        '
        'Column5
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column5.HeaderText = "A.DTION AMOUNT"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 150
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(292, 40)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(115, 40)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(97, 23)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.Color.Blue
        Me.Label18.Location = New System.Drawing.Point(232, 44)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(19, 15)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "To"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Blue
        Me.Label19.Location = New System.Drawing.Point(12, 44)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(33, 15)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Date"
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(712, 35)
        Me.Label11.TabIndex = 35
        Me.Label11.Text = "ADDITION AND DEDUCTION TO SALARY"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Payroll_Additional_Deduction_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(712, 364)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.Label11)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Payroll_Additional_Deduction_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PAYROLL ADDITIONAL DEDUCTION"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbo_EmployeeFilter As System.Windows.Forms.ComboBox
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents txt_Amount As System.Windows.Forms.TextBox
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lbl_Refno As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_remarks As System.Windows.Forms.TextBox
    Friend WithEvents cbo_EmployeeName As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents txt_OtherDeduction As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txt_OtherAddition As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txt_Store As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Store_Caption As System.Windows.Forms.Label
    Friend WithEvents txt_Medical As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Medical_Caption As System.Windows.Forms.Label
    Friend WithEvents txt_Mess As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
