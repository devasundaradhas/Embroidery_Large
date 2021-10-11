<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PayRoll_Employee_Salary_Advance_Payment
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.grp_PreviousLoanBalance = New System.Windows.Forms.GroupBox()
        Me.txt_ExistingEMI = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_CurrentLoan = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_ExistingLoan = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_NewEMI = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.lbl_VoucherNo = New System.Windows.Forms.Label()
        Me.btn_SaveAll = New System.Windows.Forms.Button()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.txt_Amount = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cbo_CashCheque = New System.Windows.Forms.ComboBox()
        Me.cbo_DebitAccount = New System.Windows.Forms.ComboBox()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbl_VouNo = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_remarks = New System.Windows.Forms.TextBox()
        Me.cbo_EmployeeName = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.cbo_EmployeeFilter = New System.Windows.Forms.ComboBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.cbo_AdvanceSalary = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnl_back.SuspendLayout()
        Me.grp_PreviousLoanBalance.SuspendLayout()
        Me.pnl_filter.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.grp_PreviousLoanBalance)
        Me.pnl_back.Controls.Add(Me.txt_NewEMI)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.dtp_Date)
        Me.pnl_back.Controls.Add(Me.lbl_VoucherNo)
        Me.pnl_back.Controls.Add(Me.btn_SaveAll)
        Me.pnl_back.Controls.Add(Me.btn_Print)
        Me.pnl_back.Controls.Add(Me.txt_Amount)
        Me.pnl_back.Controls.Add(Me.Label16)
        Me.pnl_back.Controls.Add(Me.cbo_CashCheque)
        Me.pnl_back.Controls.Add(Me.cbo_DebitAccount)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.Label12)
        Me.pnl_back.Controls.Add(Me.lbl_VouNo)
        Me.pnl_back.Controls.Add(Me.lbl_Company)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.txt_remarks)
        Me.pnl_back.Controls.Add(Me.cbo_EmployeeName)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Location = New System.Drawing.Point(12, 45)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(692, 317)
        Me.pnl_back.TabIndex = 28
        '
        'grp_PreviousLoanBalance
        '
        Me.grp_PreviousLoanBalance.Controls.Add(Me.txt_ExistingEMI)
        Me.grp_PreviousLoanBalance.Controls.Add(Me.Label13)
        Me.grp_PreviousLoanBalance.Controls.Add(Me.txt_CurrentLoan)
        Me.grp_PreviousLoanBalance.Controls.Add(Me.Label11)
        Me.grp_PreviousLoanBalance.Controls.Add(Me.txt_ExistingLoan)
        Me.grp_PreviousLoanBalance.Controls.Add(Me.Label10)
        Me.grp_PreviousLoanBalance.Location = New System.Drawing.Point(11, 152)
        Me.grp_PreviousLoanBalance.Name = "grp_PreviousLoanBalance"
        Me.grp_PreviousLoanBalance.Size = New System.Drawing.Size(676, 56)
        Me.grp_PreviousLoanBalance.TabIndex = 276
        Me.grp_PreviousLoanBalance.TabStop = False
        Me.grp_PreviousLoanBalance.Text = "Total Loan Info"
        Me.grp_PreviousLoanBalance.Visible = False
        '
        'txt_ExistingEMI
        '
        Me.txt_ExistingEMI.Location = New System.Drawing.Point(565, 22)
        Me.txt_ExistingEMI.MaxLength = 20
        Me.txt_ExistingEMI.Name = "txt_ExistingEMI"
        Me.txt_ExistingEMI.ReadOnly = True
        Me.txt_ExistingEMI.Size = New System.Drawing.Size(92, 23)
        Me.txt_ExistingEMI.TabIndex = 281
        Me.txt_ExistingEMI.TabStop = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(484, 26)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(78, 15)
        Me.Label13.TabIndex = 280
        Me.Label13.Text = "Existing EMI :"
        '
        'txt_CurrentLoan
        '
        Me.txt_CurrentLoan.Location = New System.Drawing.Point(398, 22)
        Me.txt_CurrentLoan.MaxLength = 20
        Me.txt_CurrentLoan.Name = "txt_CurrentLoan"
        Me.txt_CurrentLoan.ReadOnly = True
        Me.txt_CurrentLoan.Size = New System.Drawing.Size(81, 23)
        Me.txt_CurrentLoan.TabIndex = 279
        Me.txt_CurrentLoan.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(279, 26)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(106, 15)
        Me.Label11.TabIndex = 278
        Me.Label11.Text = "Total Loan Balance"
        '
        'txt_ExistingLoan
        '
        Me.txt_ExistingLoan.Location = New System.Drawing.Point(199, 22)
        Me.txt_ExistingLoan.MaxLength = 20
        Me.txt_ExistingLoan.Name = "txt_ExistingLoan"
        Me.txt_ExistingLoan.ReadOnly = True
        Me.txt_ExistingLoan.Size = New System.Drawing.Size(74, 23)
        Me.txt_ExistingLoan.TabIndex = 277
        Me.txt_ExistingLoan.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(7, 26)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(190, 15)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Loan Balance (Excluding this Loan)"
        '
        'txt_NewEMI
        '
        Me.txt_NewEMI.Location = New System.Drawing.Point(479, 122)
        Me.txt_NewEMI.MaxLength = 20
        Me.txt_NewEMI.Name = "txt_NewEMI"
        Me.txt_NewEMI.Size = New System.Drawing.Size(191, 23)
        Me.txt_NewEMI.TabIndex = 274
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(364, 126)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 15)
        Me.Label4.TabIndex = 275
        Me.Label4.Text = "New EMI (Total) :"
        '
        'dtp_Date
        '
        Me.dtp_Date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_Date.Location = New System.Drawing.Point(479, 35)
        Me.dtp_Date.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(191, 23)
        Me.dtp_Date.TabIndex = 31
        '
        'lbl_VoucherNo
        '
        Me.lbl_VoucherNo.BackColor = System.Drawing.Color.Gainsboro
        Me.lbl_VoucherNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_VoucherNo.Location = New System.Drawing.Point(118, 35)
        Me.lbl_VoucherNo.Name = "lbl_VoucherNo"
        Me.lbl_VoucherNo.Size = New System.Drawing.Size(204, 23)
        Me.lbl_VoucherNo.TabIndex = 273
        Me.lbl_VoucherNo.Text = "lbl_VoucherNo"
        Me.lbl_VoucherNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btn_SaveAll
        '
        Me.btn_SaveAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_SaveAll.ForeColor = System.Drawing.Color.White
        Me.btn_SaveAll.Location = New System.Drawing.Point(19, 252)
        Me.btn_SaveAll.Name = "btn_SaveAll"
        Me.btn_SaveAll.Size = New System.Drawing.Size(72, 28)
        Me.btn_SaveAll.TabIndex = 272
        Me.btn_SaveAll.TabStop = False
        Me.btn_SaveAll.Text = "&SAVE ALL"
        Me.btn_SaveAll.UseVisualStyleBackColor = False
        Me.btn_SaveAll.Visible = False
        '
        'btn_Print
        '
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(484, 250)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(83, 30)
        Me.btn_Print.TabIndex = 32
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&PRINT"
        Me.btn_Print.UseVisualStyleBackColor = False
        '
        'txt_Amount
        '
        Me.txt_Amount.Location = New System.Drawing.Point(118, 121)
        Me.txt_Amount.MaxLength = 20
        Me.txt_Amount.Name = "txt_Amount"
        Me.txt_Amount.Size = New System.Drawing.Size(204, 23)
        Me.txt_Amount.TabIndex = 4
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Blue
        Me.Label16.Location = New System.Drawing.Point(16, 125)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(52, 15)
        Me.Label16.TabIndex = 31
        Me.Label16.Text = "Amount"
        '
        'cbo_CashCheque
        '
        Me.cbo_CashCheque.FormattingEnabled = True
        Me.cbo_CashCheque.Location = New System.Drawing.Point(118, 92)
        Me.cbo_CashCheque.MaxLength = 35
        Me.cbo_CashCheque.Name = "cbo_CashCheque"
        Me.cbo_CashCheque.Size = New System.Drawing.Size(204, 23)
        Me.cbo_CashCheque.TabIndex = 2
        '
        'cbo_DebitAccount
        '
        Me.cbo_DebitAccount.DropDownHeight = 150
        Me.cbo_DebitAccount.FormattingEnabled = True
        Me.cbo_DebitAccount.IntegralHeight = False
        Me.cbo_DebitAccount.Location = New System.Drawing.Point(479, 92)
        Me.cbo_DebitAccount.MaxLength = 35
        Me.cbo_DebitAccount.Name = "cbo_DebitAccount"
        Me.cbo_DebitAccount.Size = New System.Drawing.Size(191, 23)
        Me.cbo_DebitAccount.TabIndex = 3
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(587, 250)
        Me.btn_close.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(83, 30)
        Me.btn_close.TabIndex = 8
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Blue
        Me.Label12.Location = New System.Drawing.Point(16, 96)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(80, 15)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Cash/Cheque"
        '
        'lbl_VouNo
        '
        Me.lbl_VouNo.BackColor = System.Drawing.Color.Gainsboro
        Me.lbl_VouNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_VouNo.Location = New System.Drawing.Point(175, 246)
        Me.lbl_VouNo.Name = "lbl_VouNo"
        Me.lbl_VouNo.Size = New System.Drawing.Size(102, 23)
        Me.lbl_VouNo.TabIndex = 21
        Me.lbl_VouNo.Text = "lbl_VouNo"
        Me.lbl_VouNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbl_VouNo.Visible = False
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.Location = New System.Drawing.Point(115, 0)
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
        Me.btn_save.Location = New System.Drawing.Point(385, 250)
        Me.btn_save.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(83, 30)
        Me.btn_save.TabIndex = 7
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
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
        Me.txt_remarks.Location = New System.Drawing.Point(118, 216)
        Me.txt_remarks.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txt_remarks.MaxLength = 100
        Me.txt_remarks.Name = "txt_remarks"
        Me.txt_remarks.Size = New System.Drawing.Size(552, 23)
        Me.txt_remarks.TabIndex = 5
        '
        'cbo_EmployeeName
        '
        Me.cbo_EmployeeName.DropDownHeight = 160
        Me.cbo_EmployeeName.FormattingEnabled = True
        Me.cbo_EmployeeName.IntegralHeight = False
        Me.cbo_EmployeeName.Location = New System.Drawing.Point(118, 63)
        Me.cbo_EmployeeName.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.cbo_EmployeeName.MaxLength = 35
        Me.cbo_EmployeeName.Name = "cbo_EmployeeName"
        Me.cbo_EmployeeName.Size = New System.Drawing.Size(552, 23)
        Me.cbo_EmployeeName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(16, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Voucher No"
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
        Me.Label2.Location = New System.Drawing.Point(364, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(17, 220)
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
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(362, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 15)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Credit Account"
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
        Me.pnl_filter.Location = New System.Drawing.Point(751, 252)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(599, 304)
        Me.pnl_filter.TabIndex = 30
        '
        'cbo_EmployeeFilter
        '
        Me.cbo_EmployeeFilter.DropDownHeight = 90
        Me.cbo_EmployeeFilter.DropDownWidth = 250
        Me.cbo_EmployeeFilter.FormattingEnabled = True
        Me.cbo_EmployeeFilter.IntegralHeight = False
        Me.cbo_EmployeeFilter.Location = New System.Drawing.Point(100, 86)
        Me.cbo_EmployeeFilter.MaxLength = 35
        Me.cbo_EmployeeFilter.Name = "cbo_EmployeeFilter"
        Me.cbo_EmployeeFilter.Size = New System.Drawing.Size(294, 23)
        Me.cbo_EmployeeFilter.TabIndex = 2
        Me.cbo_EmployeeFilter.Text = "cbo_EmployeeFilter"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(12, 90)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(72, 15)
        Me.Label20.TabIndex = 9
        Me.Label20.Text = "Party Name"
        '
        'btn_closefilter
        '
        Me.btn_closefilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_closefilter.ForeColor = System.Drawing.Color.White
        Me.btn_closefilter.Location = New System.Drawing.Point(509, 44)
        Me.btn_closefilter.Name = "btn_closefilter"
        Me.btn_closefilter.Size = New System.Drawing.Size(75, 65)
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
        Me.btn_filtershow.Location = New System.Drawing.Point(422, 44)
        Me.btn_filtershow.Name = "btn_filtershow"
        Me.btn_filtershow.Size = New System.Drawing.Size(75, 65)
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
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column3, Me.dc, Me.Column1, Me.Column2, Me.Column5})
        Me.dgv_filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_filter.Location = New System.Drawing.Point(11, 123)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(573, 161)
        Me.dgv_filter.TabIndex = 5
        '
        'Column3
        '
        Me.Column3.HeaderText = "REF.NO"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 80
        '
        'dc
        '
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dc.DefaultCellStyle = DataGridViewCellStyle5
        Me.dc.HeaderText = "VOU.NO"
        Me.dc.MaxInputLength = 8
        Me.dc.Name = "dc"
        Me.dc.ReadOnly = True
        Me.dc.Width = 80
        '
        'Column1
        '
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column1.HeaderText = "DATE"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 80
        '
        'Column2
        '
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle7
        Me.Column2.HeaderText = "PARTY NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 200
        '
        'Column5
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle8
        Me.Column5.HeaderText = "AMOUNT"
        Me.Column5.Name = "Column5"
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(292, 44)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(100, 44)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(232, 48)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(19, 15)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "To"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(12, 48)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(33, 15)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Date"
        '
        'cbo_AdvanceSalary
        '
        Me.cbo_AdvanceSalary.BackColor = System.Drawing.Color.Red
        Me.cbo_AdvanceSalary.FormattingEnabled = True
        Me.cbo_AdvanceSalary.Location = New System.Drawing.Point(863, 201)
        Me.cbo_AdvanceSalary.MaxLength = 35
        Me.cbo_AdvanceSalary.Name = "cbo_AdvanceSalary"
        Me.cbo_AdvanceSalary.Size = New System.Drawing.Size(204, 23)
        Me.cbo_AdvanceSalary.TabIndex = 4
        Me.cbo_AdvanceSalary.Visible = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Red
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(761, 205)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(91, 15)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Advance/Salary"
        Me.Label9.Visible = False
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(712, 35)
        Me.lbl_Heading.TabIndex = 29
        Me.lbl_Heading.Text = "EMPLOYEE PAYMENT"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'PrintDocument1
        '
        '
        'Timer1
        '
        '
        'PayRoll_Employee_Salary_Advance_Payment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(712, 374)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.cbo_AdvanceSalary)
        Me.Controls.Add(Me.Label9)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PayRoll_Employee_Salary_Advance_Payment"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EMPLOYEE PAYMENT"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.grp_PreviousLoanBalance.ResumeLayout(False)
        Me.grp_PreviousLoanBalance.PerformLayout()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents txt_Amount As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cbo_CashCheque As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_DebitAccount As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_AdvanceSalary As System.Windows.Forms.ComboBox
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lbl_VouNo As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_remarks As System.Windows.Forms.TextBox
    Friend WithEvents cbo_EmployeeName As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents cbo_EmployeeFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents btn_SaveAll As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lbl_VoucherNo As System.Windows.Forms.Label
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents grp_PreviousLoanBalance As GroupBox
    Friend WithEvents txt_CurrentLoan As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txt_ExistingLoan As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txt_NewEMI As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txt_ExistingEMI As TextBox
    Friend WithEvents Label13 As Label
End Class
