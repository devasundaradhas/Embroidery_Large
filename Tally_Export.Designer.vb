<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Tally_Export
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
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.dgv_Statistics_Total = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgv_Statistics_Details = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.txt_Path = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.opt_WithOutOpeningBalance = New System.Windows.Forms.RadioButton()
        Me.opt_WithOpeningBalance = New System.Windows.Forms.RadioButton()
        Me.chk_AllLedgers = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chklst_Ledgers = New System.Windows.Forms.CheckedListBox()
        Me.opt_SelectedLedgers = New System.Windows.Forms.RadioButton()
        Me.opt_AllLedgers = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chk_PettiCash = New System.Windows.Forms.CheckBox()
        Me.chk_DebitNote = New System.Windows.Forms.CheckBox()
        Me.chk_Purchase = New System.Windows.Forms.CheckBox()
        Me.chk_Sales = New System.Windows.Forms.CheckBox()
        Me.chk_Receipt = New System.Windows.Forms.CheckBox()
        Me.chk_Payment = New System.Windows.Forms.CheckBox()
        Me.chk_CashReceipt = New System.Windows.Forms.CheckBox()
        Me.chk_CashPayment = New System.Windows.Forms.CheckBox()
        Me.chk_Contra = New System.Windows.Forms.CheckBox()
        Me.chk_Journal = New System.Windows.Forms.CheckBox()
        Me.chk_CreditNote = New System.Windows.Forms.CheckBox()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.msk_ToDate = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.msk_FromDate = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_FromDate = New System.Windows.Forms.DateTimePicker()
        Me.cbo_ExportFormat = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btn_ExportTally = New System.Windows.Forms.Button()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.dgv_Statistics_Total, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv_Statistics_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Panel5)
        Me.pnl_Back.Controls.Add(Me.Panel4)
        Me.pnl_Back.Controls.Add(Me.Panel3)
        Me.pnl_Back.Controls.Add(Me.Panel2)
        Me.pnl_Back.Controls.Add(Me.Panel1)
        Me.pnl_Back.Controls.Add(Me.Panel6)
        Me.pnl_Back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(662, 472)
        Me.pnl_Back.TabIndex = 0
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.dgv_Statistics_Total)
        Me.Panel5.Controls.Add(Me.dgv_Statistics_Details)
        Me.Panel5.Controls.Add(Me.Label3)
        Me.Panel5.Location = New System.Drawing.Point(426, 154)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(221, 306)
        Me.Panel5.TabIndex = 69
        '
        'dgv_Statistics_Total
        '
        Me.dgv_Statistics_Total.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Statistics_Total.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Statistics_Total.ColumnHeadersVisible = False
        Me.dgv_Statistics_Total.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2})
        Me.dgv_Statistics_Total.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgv_Statistics_Total.Location = New System.Drawing.Point(0, 263)
        Me.dgv_Statistics_Total.Name = "dgv_Statistics_Total"
        Me.dgv_Statistics_Total.RowHeadersVisible = False
        Me.dgv_Statistics_Total.RowTemplate.Height = 25
        Me.dgv_Statistics_Total.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgv_Statistics_Total.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Statistics_Total.Size = New System.Drawing.Size(219, 41)
        Me.dgv_Statistics_Total.TabIndex = 2
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "VOUCHER TYPE"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 125
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "NO.OF ENTRIES"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 75
        '
        'dgv_Statistics_Details
        '
        Me.dgv_Statistics_Details.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Statistics_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Statistics_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.dgv_Statistics_Details.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgv_Statistics_Details.Location = New System.Drawing.Point(0, 27)
        Me.dgv_Statistics_Details.Name = "dgv_Statistics_Details"
        Me.dgv_Statistics_Details.RowHeadersVisible = False
        Me.dgv_Statistics_Details.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgv_Statistics_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Statistics_Details.Size = New System.Drawing.Size(219, 230)
        Me.dgv_Statistics_Details.TabIndex = 1
        '
        'Column1
        '
        Me.Column1.HeaderText = "VOUCHER TYPE"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 125
        '
        'Column2
        '
        Me.Column2.HeaderText = "NO.OF ENTRIES"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 75
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Khaki
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(219, 27)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "STATISTICS"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.txt_Path)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Location = New System.Drawing.Point(347, 112)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(300, 36)
        Me.Panel4.TabIndex = 64
        '
        'txt_Path
        '
        Me.txt_Path.Location = New System.Drawing.Point(61, 7)
        Me.txt_Path.Name = "txt_Path"
        Me.txt_Path.Size = New System.Drawing.Size(232, 23)
        Me.txt_Path.TabIndex = 67
        Me.txt_Path.Text = "C:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(41, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "PATH :"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.opt_WithOutOpeningBalance)
        Me.Panel3.Controls.Add(Me.opt_WithOpeningBalance)
        Me.Panel3.Controls.Add(Me.chk_AllLedgers)
        Me.Panel3.Location = New System.Drawing.Point(347, 12)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(300, 94)
        Me.Panel3.TabIndex = 68
        '
        'opt_WithOutOpeningBalance
        '
        Me.opt_WithOutOpeningBalance.AutoSize = True
        Me.opt_WithOutOpeningBalance.Location = New System.Drawing.Point(45, 58)
        Me.opt_WithOutOpeningBalance.Name = "opt_WithOutOpeningBalance"
        Me.opt_WithOutOpeningBalance.Size = New System.Drawing.Size(185, 19)
        Me.opt_WithOutOpeningBalance.TabIndex = 2
        Me.opt_WithOutOpeningBalance.Text = "WITHOUT OPENING BALANCE"
        Me.opt_WithOutOpeningBalance.UseVisualStyleBackColor = True
        '
        'opt_WithOpeningBalance
        '
        Me.opt_WithOpeningBalance.AutoSize = True
        Me.opt_WithOpeningBalance.Checked = True
        Me.opt_WithOpeningBalance.Location = New System.Drawing.Point(45, 33)
        Me.opt_WithOpeningBalance.Name = "opt_WithOpeningBalance"
        Me.opt_WithOpeningBalance.Size = New System.Drawing.Size(162, 19)
        Me.opt_WithOpeningBalance.TabIndex = 1
        Me.opt_WithOpeningBalance.TabStop = True
        Me.opt_WithOpeningBalance.Text = "WITH OPENING BALANCE"
        Me.opt_WithOpeningBalance.UseVisualStyleBackColor = True
        '
        'chk_AllLedgers
        '
        Me.chk_AllLedgers.AutoSize = True
        Me.chk_AllLedgers.Checked = True
        Me.chk_AllLedgers.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_AllLedgers.Location = New System.Drawing.Point(17, 8)
        Me.chk_AllLedgers.Name = "chk_AllLedgers"
        Me.chk_AllLedgers.Size = New System.Drawing.Size(96, 19)
        Me.chk_AllLedgers.TabIndex = 0
        Me.chk_AllLedgers.Text = "ALL LEDGERS"
        Me.chk_AllLedgers.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.chklst_Ledgers)
        Me.Panel2.Controls.Add(Me.opt_SelectedLedgers)
        Me.Panel2.Controls.Add(Me.opt_AllLedgers)
        Me.Panel2.Location = New System.Drawing.Point(212, 154)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(208, 306)
        Me.Panel2.TabIndex = 67
        '
        'chklst_Ledgers
        '
        Me.chklst_Ledgers.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.chklst_Ledgers.FormattingEnabled = True
        Me.chklst_Ledgers.Location = New System.Drawing.Point(0, 30)
        Me.chklst_Ledgers.Name = "chklst_Ledgers"
        Me.chklst_Ledgers.Size = New System.Drawing.Size(206, 274)
        Me.chklst_Ledgers.TabIndex = 65
        '
        'opt_SelectedLedgers
        '
        Me.opt_SelectedLedgers.AutoSize = True
        Me.opt_SelectedLedgers.Location = New System.Drawing.Point(99, 7)
        Me.opt_SelectedLedgers.Name = "opt_SelectedLedgers"
        Me.opt_SelectedLedgers.Size = New System.Drawing.Size(76, 19)
        Me.opt_SelectedLedgers.TabIndex = 64
        Me.opt_SelectedLedgers.Text = "SELECTED"
        Me.opt_SelectedLedgers.UseVisualStyleBackColor = True
        '
        'opt_AllLedgers
        '
        Me.opt_AllLedgers.AutoSize = True
        Me.opt_AllLedgers.Checked = True
        Me.opt_AllLedgers.Location = New System.Drawing.Point(7, 7)
        Me.opt_AllLedgers.Name = "opt_AllLedgers"
        Me.opt_AllLedgers.Size = New System.Drawing.Size(45, 19)
        Me.opt_AllLedgers.TabIndex = 63
        Me.opt_AllLedgers.TabStop = True
        Me.opt_AllLedgers.Text = "ALL"
        Me.opt_AllLedgers.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.chk_PettiCash)
        Me.Panel1.Controls.Add(Me.chk_DebitNote)
        Me.Panel1.Controls.Add(Me.chk_Purchase)
        Me.Panel1.Controls.Add(Me.chk_Sales)
        Me.Panel1.Controls.Add(Me.chk_Receipt)
        Me.Panel1.Controls.Add(Me.chk_Payment)
        Me.Panel1.Controls.Add(Me.chk_CashReceipt)
        Me.Panel1.Controls.Add(Me.chk_CashPayment)
        Me.Panel1.Controls.Add(Me.chk_Contra)
        Me.Panel1.Controls.Add(Me.chk_Journal)
        Me.Panel1.Controls.Add(Me.chk_CreditNote)
        Me.Panel1.Location = New System.Drawing.Point(6, 154)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 306)
        Me.Panel1.TabIndex = 66
        '
        'chk_PettiCash
        '
        Me.chk_PettiCash.AutoSize = True
        Me.chk_PettiCash.Checked = True
        Me.chk_PettiCash.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_PettiCash.Location = New System.Drawing.Point(17, 263)
        Me.chk_PettiCash.Name = "chk_PettiCash"
        Me.chk_PettiCash.Size = New System.Drawing.Size(86, 19)
        Me.chk_PettiCash.TabIndex = 60
        Me.chk_PettiCash.Text = "PETTI CASH"
        Me.chk_PettiCash.UseVisualStyleBackColor = True
        '
        'chk_DebitNote
        '
        Me.chk_DebitNote.AutoSize = True
        Me.chk_DebitNote.Checked = True
        Me.chk_DebitNote.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_DebitNote.Location = New System.Drawing.Point(17, 238)
        Me.chk_DebitNote.Name = "chk_DebitNote"
        Me.chk_DebitNote.Size = New System.Drawing.Size(89, 19)
        Me.chk_DebitNote.TabIndex = 59
        Me.chk_DebitNote.Text = "DEBIT NOTE"
        Me.chk_DebitNote.UseVisualStyleBackColor = True
        '
        'chk_Purchase
        '
        Me.chk_Purchase.AutoSize = True
        Me.chk_Purchase.Checked = True
        Me.chk_Purchase.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_Purchase.Location = New System.Drawing.Point(17, 13)
        Me.chk_Purchase.Name = "chk_Purchase"
        Me.chk_Purchase.Size = New System.Drawing.Size(83, 19)
        Me.chk_Purchase.TabIndex = 50
        Me.chk_Purchase.Text = "PURCHASE"
        Me.chk_Purchase.UseVisualStyleBackColor = True
        '
        'chk_Sales
        '
        Me.chk_Sales.AutoSize = True
        Me.chk_Sales.Checked = True
        Me.chk_Sales.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_Sales.Location = New System.Drawing.Point(17, 38)
        Me.chk_Sales.Name = "chk_Sales"
        Me.chk_Sales.Size = New System.Drawing.Size(58, 19)
        Me.chk_Sales.TabIndex = 51
        Me.chk_Sales.Text = "SALES"
        Me.chk_Sales.UseVisualStyleBackColor = True
        '
        'chk_Receipt
        '
        Me.chk_Receipt.AutoSize = True
        Me.chk_Receipt.Checked = True
        Me.chk_Receipt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_Receipt.Location = New System.Drawing.Point(17, 63)
        Me.chk_Receipt.Name = "chk_Receipt"
        Me.chk_Receipt.Size = New System.Drawing.Size(68, 19)
        Me.chk_Receipt.TabIndex = 52
        Me.chk_Receipt.Text = "RECEIPT"
        Me.chk_Receipt.UseVisualStyleBackColor = True
        '
        'chk_Payment
        '
        Me.chk_Payment.AutoSize = True
        Me.chk_Payment.Checked = True
        Me.chk_Payment.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_Payment.Location = New System.Drawing.Point(17, 88)
        Me.chk_Payment.Name = "chk_Payment"
        Me.chk_Payment.Size = New System.Drawing.Size(78, 19)
        Me.chk_Payment.TabIndex = 53
        Me.chk_Payment.Text = "PAYMENT"
        Me.chk_Payment.UseVisualStyleBackColor = True
        '
        'chk_CashReceipt
        '
        Me.chk_CashReceipt.AutoSize = True
        Me.chk_CashReceipt.Checked = True
        Me.chk_CashReceipt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_CashReceipt.Location = New System.Drawing.Point(17, 113)
        Me.chk_CashReceipt.Name = "chk_CashReceipt"
        Me.chk_CashReceipt.Size = New System.Drawing.Size(100, 19)
        Me.chk_CashReceipt.TabIndex = 54
        Me.chk_CashReceipt.Text = "CASH RECEIPT"
        Me.chk_CashReceipt.UseVisualStyleBackColor = True
        '
        'chk_CashPayment
        '
        Me.chk_CashPayment.AutoSize = True
        Me.chk_CashPayment.Checked = True
        Me.chk_CashPayment.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_CashPayment.Location = New System.Drawing.Point(17, 138)
        Me.chk_CashPayment.Name = "chk_CashPayment"
        Me.chk_CashPayment.Size = New System.Drawing.Size(110, 19)
        Me.chk_CashPayment.TabIndex = 55
        Me.chk_CashPayment.Text = "CASH PAYMENT"
        Me.chk_CashPayment.UseVisualStyleBackColor = True
        '
        'chk_Contra
        '
        Me.chk_Contra.AutoSize = True
        Me.chk_Contra.Checked = True
        Me.chk_Contra.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_Contra.Location = New System.Drawing.Point(17, 163)
        Me.chk_Contra.Name = "chk_Contra"
        Me.chk_Contra.Size = New System.Drawing.Size(72, 19)
        Me.chk_Contra.TabIndex = 56
        Me.chk_Contra.Text = "CONTRA"
        Me.chk_Contra.UseVisualStyleBackColor = True
        '
        'chk_Journal
        '
        Me.chk_Journal.AutoSize = True
        Me.chk_Journal.Checked = True
        Me.chk_Journal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_Journal.Location = New System.Drawing.Point(17, 188)
        Me.chk_Journal.Name = "chk_Journal"
        Me.chk_Journal.Size = New System.Drawing.Size(77, 19)
        Me.chk_Journal.TabIndex = 57
        Me.chk_Journal.Text = "JOURNAL"
        Me.chk_Journal.UseVisualStyleBackColor = True
        '
        'chk_CreditNote
        '
        Me.chk_CreditNote.AutoSize = True
        Me.chk_CreditNote.Checked = True
        Me.chk_CreditNote.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chk_CreditNote.Location = New System.Drawing.Point(17, 213)
        Me.chk_CreditNote.Name = "chk_CreditNote"
        Me.chk_CreditNote.Size = New System.Drawing.Size(96, 19)
        Me.chk_CreditNote.TabIndex = 58
        Me.chk_CreditNote.Text = "CREDIT NOTE"
        Me.chk_CreditNote.UseVisualStyleBackColor = True
        '
        'Panel6
        '
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.msk_ToDate)
        Me.Panel6.Controls.Add(Me.dtp_ToDate)
        Me.Panel6.Controls.Add(Me.msk_FromDate)
        Me.Panel6.Controls.Add(Me.dtp_FromDate)
        Me.Panel6.Controls.Add(Me.cbo_ExportFormat)
        Me.Panel6.Controls.Add(Me.Label14)
        Me.Panel6.Controls.Add(Me.Label1)
        Me.Panel6.Controls.Add(Me.Label4)
        Me.Panel6.Controls.Add(Me.btn_ExportTally)
        Me.Panel6.Controls.Add(Me.btn_close)
        Me.Panel6.Location = New System.Drawing.Point(6, 8)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(335, 140)
        Me.Panel6.TabIndex = 65
        '
        'msk_ToDate
        '
        Me.msk_ToDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_ToDate.Location = New System.Drawing.Point(212, 56)
        Me.msk_ToDate.Mask = "00-00-0000"
        Me.msk_ToDate.Name = "msk_ToDate"
        Me.msk_ToDate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_ToDate.Size = New System.Drawing.Size(94, 22)
        Me.msk_ToDate.TabIndex = 3
        '
        'dtp_ToDate
        '
        Me.dtp_ToDate.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_ToDate.Location = New System.Drawing.Point(304, 56)
        Me.dtp_ToDate.Name = "dtp_ToDate"
        Me.dtp_ToDate.Size = New System.Drawing.Size(20, 22)
        Me.dtp_ToDate.TabIndex = 4
        Me.dtp_ToDate.TabStop = False
        '
        'msk_FromDate
        '
        Me.msk_FromDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_FromDate.Location = New System.Drawing.Point(71, 56)
        Me.msk_FromDate.Mask = "00-00-0000"
        Me.msk_FromDate.Name = "msk_FromDate"
        Me.msk_FromDate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_FromDate.Size = New System.Drawing.Size(94, 22)
        Me.msk_FromDate.TabIndex = 1
        '
        'dtp_FromDate
        '
        Me.dtp_FromDate.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_FromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_FromDate.Location = New System.Drawing.Point(164, 56)
        Me.dtp_FromDate.Name = "dtp_FromDate"
        Me.dtp_FromDate.Size = New System.Drawing.Size(19, 22)
        Me.dtp_FromDate.TabIndex = 2
        Me.dtp_FromDate.TabStop = False
        '
        'cbo_ExportFormat
        '
        Me.cbo_ExportFormat.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ExportFormat.FormattingEnabled = True
        Me.cbo_ExportFormat.Items.AddRange(New Object() {"", "TALLY 7.2 OR BELOW", "TALLY 9 OR ABOVE"})
        Me.cbo_ExportFormat.Location = New System.Drawing.Point(71, 12)
        Me.cbo_ExportFormat.Name = "cbo_ExportFormat"
        Me.cbo_ExportFormat.Size = New System.Drawing.Size(253, 23)
        Me.cbo_ExportFormat.TabIndex = 0
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Blue
        Me.Label14.Location = New System.Drawing.Point(7, 16)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(58, 15)
        Me.Label14.TabIndex = 168
        Me.Label14.Text = "Export To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(189, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(19, 15)
        Me.Label1.TabIndex = 123
        Me.Label1.Text = "To"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(7, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 15)
        Me.Label4.TabIndex = 121
        Me.Label4.Text = " Date Range"
        '
        'btn_ExportTally
        '
        Me.btn_ExportTally.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.btn_ExportTally.ForeColor = System.Drawing.Color.White
        Me.btn_ExportTally.Location = New System.Drawing.Point(84, 96)
        Me.btn_ExportTally.Name = "btn_ExportTally"
        Me.btn_ExportTally.Size = New System.Drawing.Size(77, 30)
        Me.btn_ExportTally.TabIndex = 47
        Me.btn_ExportTally.TabStop = False
        Me.btn_ExportTally.Text = "&EXPORT"
        Me.btn_ExportTally.UseVisualStyleBackColor = False
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(212, 96)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(73, 32)
        Me.btn_close.TabIndex = 48
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(680, 35)
        Me.Label2.TabIndex = 37
        Me.Label2.Text = "TALLY EXPORT"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Lime
        Me.lbl_Company.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Company.Location = New System.Drawing.Point(136, 9)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 38
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'Tally_Export
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Ivory
        Me.ClientSize = New System.Drawing.Size(680, 526)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Tally_Export"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TALLY  EXPORT"
        Me.pnl_Back.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        CType(Me.dgv_Statistics_Total, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv_Statistics_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents dgv_Statistics_Details As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents txt_Path As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents opt_WithOutOpeningBalance As System.Windows.Forms.RadioButton
    Friend WithEvents opt_WithOpeningBalance As System.Windows.Forms.RadioButton
    Friend WithEvents chk_AllLedgers As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents chklst_Ledgers As System.Windows.Forms.CheckedListBox
    Friend WithEvents opt_SelectedLedgers As System.Windows.Forms.RadioButton
    Friend WithEvents opt_AllLedgers As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chk_PettiCash As System.Windows.Forms.CheckBox
    Friend WithEvents chk_DebitNote As System.Windows.Forms.CheckBox
    Friend WithEvents chk_Purchase As System.Windows.Forms.CheckBox
    Friend WithEvents chk_Sales As System.Windows.Forms.CheckBox
    Friend WithEvents chk_Receipt As System.Windows.Forms.CheckBox
    Friend WithEvents chk_Payment As System.Windows.Forms.CheckBox
    Friend WithEvents chk_CashReceipt As System.Windows.Forms.CheckBox
    Friend WithEvents chk_CashPayment As System.Windows.Forms.CheckBox
    Friend WithEvents chk_Contra As System.Windows.Forms.CheckBox
    Friend WithEvents chk_Journal As System.Windows.Forms.CheckBox
    Friend WithEvents chk_CreditNote As System.Windows.Forms.CheckBox
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents msk_ToDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents msk_FromDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_FromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents cbo_ExportFormat As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btn_ExportTally As System.Windows.Forms.Button
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents dgv_Statistics_Total As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
