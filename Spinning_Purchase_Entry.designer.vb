<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Spinning_Purchase_Entry
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.txt_BillNo = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txt_Freight = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_VatPerc = New System.Windows.Forms.TextBox()
        Me.txt_VatAmount = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.lbl_InvoiceNo = New System.Windows.Forms.Label()
        Me.txt_Assessable = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.cbo_VatAc = New System.Windows.Forms.ComboBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txt_Rate_Kg = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_BaleNos = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txt_Amount = New System.Windows.Forms.TextBox()
        Me.txt_Bales = New System.Windows.Forms.TextBox()
        Me.cbo_ItemName = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txt_Weight = New System.Windows.Forms.TextBox()
        Me.cbo_SalesAc = New System.Windows.Forms.ComboBox()
        Me.cbo_Ledger = New System.Windows.Forms.ComboBox()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txt_NetAmount = New System.Windows.Forms.TextBox()
        Me.txt_RoundOff = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txt_AddLess_AfterTax = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txt_AddLess_BeforeTax = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txt_VehicleNo = New System.Windows.Forms.TextBox()
        Me.lbl_AmountInWords = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.pnl_Filter = New System.Windows.Forms.Panel()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.btn_Filter_Close = New System.Windows.Forms.Button()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.cbo_Filter_ItemName = New System.Windows.Forms.ComboBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.btn_Filter_Show = New System.Windows.Forms.Button()
        Me.dgv_Filter_Details = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cbo_Filter_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.dtp_Filter_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.dtp_Filter_Fromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.pnl_Back.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnl_Filter.SuspendLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.AutoScroll = True
        Me.pnl_Back.AutoSize = True
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.txt_BillNo)
        Me.pnl_Back.Controls.Add(Me.Label24)
        Me.pnl_Back.Controls.Add(Me.txt_Freight)
        Me.pnl_Back.Controls.Add(Me.Label13)
        Me.pnl_Back.Controls.Add(Me.txt_VatPerc)
        Me.pnl_Back.Controls.Add(Me.txt_VatAmount)
        Me.pnl_Back.Controls.Add(Me.Label10)
        Me.pnl_Back.Controls.Add(Me.Label12)
        Me.pnl_Back.Controls.Add(Me.lbl_Company)
        Me.pnl_Back.Controls.Add(Me.lbl_InvoiceNo)
        Me.pnl_Back.Controls.Add(Me.txt_Assessable)
        Me.pnl_Back.Controls.Add(Me.Label28)
        Me.pnl_Back.Controls.Add(Me.cbo_VatAc)
        Me.pnl_Back.Controls.Add(Me.Label25)
        Me.pnl_Back.Controls.Add(Me.Panel2)
        Me.pnl_Back.Controls.Add(Me.cbo_SalesAc)
        Me.pnl_Back.Controls.Add(Me.cbo_Ledger)
        Me.pnl_Back.Controls.Add(Me.dtp_Date)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.Label7)
        Me.pnl_Back.Controls.Add(Me.txt_NetAmount)
        Me.pnl_Back.Controls.Add(Me.txt_RoundOff)
        Me.pnl_Back.Controls.Add(Me.Label22)
        Me.pnl_Back.Controls.Add(Me.txt_AddLess_AfterTax)
        Me.pnl_Back.Controls.Add(Me.Label20)
        Me.pnl_Back.Controls.Add(Me.txt_AddLess_BeforeTax)
        Me.pnl_Back.Controls.Add(Me.Label19)
        Me.pnl_Back.Controls.Add(Me.Label18)
        Me.pnl_Back.Controls.Add(Me.txt_VehicleNo)
        Me.pnl_Back.Controls.Add(Me.lbl_AmountInWords)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Enabled = False
        Me.pnl_Back.Location = New System.Drawing.Point(6, 10)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(806, 549)
        Me.pnl_Back.TabIndex = 0
        '
        'txt_BillNo
        '
        Me.txt_BillNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_BillNo.Location = New System.Drawing.Point(449, 78)
        Me.txt_BillNo.MaxLength = 35
        Me.txt_BillNo.Name = "txt_BillNo"
        Me.txt_BillNo.Size = New System.Drawing.Size(328, 20)
        Me.txt_BillNo.TabIndex = 2
        Me.txt_BillNo.Text = "txt_BillNo"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label24.Location = New System.Drawing.Point(344, 81)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(52, 13)
        Me.Label24.TabIndex = 40
        Me.Label24.Text = "Bill No. "
        '
        'txt_Freight
        '
        Me.txt_Freight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Freight.Location = New System.Drawing.Point(651, 372)
        Me.txt_Freight.MaxLength = 12
        Me.txt_Freight.Name = "txt_Freight"
        Me.txt_Freight.Size = New System.Drawing.Size(139, 20)
        Me.txt_Freight.TabIndex = 15
        Me.txt_Freight.Text = "txt_Freight"
        Me.txt_Freight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label13.Location = New System.Drawing.Point(541, 376)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(46, 13)
        Me.Label13.TabIndex = 38
        Me.Label13.Text = "Freight"
        '
        'txt_VatPerc
        '
        Me.txt_VatPerc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_VatPerc.Location = New System.Drawing.Point(651, 340)
        Me.txt_VatPerc.MaxLength = 5
        Me.txt_VatPerc.Name = "txt_VatPerc"
        Me.txt_VatPerc.Size = New System.Drawing.Size(40, 20)
        Me.txt_VatPerc.TabIndex = 14
        Me.txt_VatPerc.Text = "txt_VatPerc"
        Me.txt_VatPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_VatAmount
        '
        Me.txt_VatAmount.Enabled = False
        Me.txt_VatAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_VatAmount.Location = New System.Drawing.Point(708, 340)
        Me.txt_VatAmount.MaxLength = 12
        Me.txt_VatAmount.Name = "txt_VatAmount"
        Me.txt_VatAmount.Size = New System.Drawing.Size(82, 20)
        Me.txt_VatAmount.TabIndex = 23
        Me.txt_VatAmount.Text = "txt_VatAmount"
        Me.txt_VatAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label10.Location = New System.Drawing.Point(693, 344)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(16, 13)
        Me.Label10.TabIndex = 34
        Me.Label10.Text = "%"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label12.Location = New System.Drawing.Point(541, 344)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(26, 13)
        Me.Label12.TabIndex = 35
        Me.Label12.Text = "Vat"
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.Location = New System.Drawing.Point(134, 66)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(67, 13)
        Me.lbl_Company.TabIndex = 33
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'lbl_InvoiceNo
        '
        Me.lbl_InvoiceNo.BackColor = System.Drawing.Color.White
        Me.lbl_InvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_InvoiceNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_InvoiceNo.Location = New System.Drawing.Point(450, 14)
        Me.lbl_InvoiceNo.Name = "lbl_InvoiceNo"
        Me.lbl_InvoiceNo.Size = New System.Drawing.Size(99, 20)
        Me.lbl_InvoiceNo.TabIndex = 0
        Me.lbl_InvoiceNo.Text = "lbl_InvoiceNo"
        '
        'txt_Assessable
        '
        Me.txt_Assessable.Enabled = False
        Me.txt_Assessable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Assessable.Location = New System.Drawing.Point(651, 308)
        Me.txt_Assessable.MaxLength = 12
        Me.txt_Assessable.Name = "txt_Assessable"
        Me.txt_Assessable.Size = New System.Drawing.Size(137, 20)
        Me.txt_Assessable.TabIndex = 13
        Me.txt_Assessable.Text = "txt_Assessable"
        Me.txt_Assessable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label28.Location = New System.Drawing.Point(541, 312)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(106, 13)
        Me.Label28.TabIndex = 30
        Me.Label28.Text = "Assessable Value"
        '
        'cbo_VatAc
        '
        Me.cbo_VatAc.DropDownHeight = 125
        Me.cbo_VatAc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_VatAc.FormattingEnabled = True
        Me.cbo_VatAc.IntegralHeight = False
        Me.cbo_VatAc.Items.AddRange(New Object() {"NO TAX", "VAT", "CST"})
        Me.cbo_VatAc.Location = New System.Drawing.Point(137, 312)
        Me.cbo_VatAc.Name = "cbo_VatAc"
        Me.cbo_VatAc.Size = New System.Drawing.Size(328, 21)
        Me.cbo_VatAc.TabIndex = 10
        Me.cbo_VatAc.Text = "cbo_VatAc"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label25.Location = New System.Drawing.Point(13, 316)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(55, 13)
        Me.Label25.TabIndex = 27
        Me.Label25.Text = "Vat A/c "
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.txt_Rate_Kg)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.txt_BaleNos)
        Me.Panel2.Controls.Add(Me.Label16)
        Me.Panel2.Controls.Add(Me.txt_Amount)
        Me.Panel2.Controls.Add(Me.txt_Bales)
        Me.Panel2.Controls.Add(Me.cbo_ItemName)
        Me.Panel2.Controls.Add(Me.Label15)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label21)
        Me.Panel2.Controls.Add(Me.txt_Weight)
        Me.Panel2.Location = New System.Drawing.Point(13, 117)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(775, 139)
        Me.Panel2.TabIndex = 8
        '
        'txt_Rate_Kg
        '
        Me.txt_Rate_Kg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Rate_Kg.Location = New System.Drawing.Point(544, 40)
        Me.txt_Rate_Kg.MaxLength = 10
        Me.txt_Rate_Kg.Name = "txt_Rate_Kg"
        Me.txt_Rate_Kg.Size = New System.Drawing.Size(76, 20)
        Me.txt_Rate_Kg.TabIndex = 6
        Me.txt_Rate_Kg.Text = "txt_Rate_Kg"
        Me.txt_Rate_Kg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label3.Location = New System.Drawing.Point(565, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "Rate/Kg"
        '
        'txt_BaleNos
        '
        Me.txt_BaleNos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_BaleNos.Location = New System.Drawing.Point(20, 96)
        Me.txt_BaleNos.MaxLength = 200
        Me.txt_BaleNos.Name = "txt_BaleNos"
        Me.txt_BaleNos.Size = New System.Drawing.Size(723, 20)
        Me.txt_BaleNos.TabIndex = 8
        Me.txt_BaleNos.Text = "txt_BaleNos"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label16.Location = New System.Drawing.Point(20, 72)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(66, 13)
        Me.Label16.TabIndex = 19
        Me.Label16.Text = "Bale No.s "
        '
        'txt_Amount
        '
        Me.txt_Amount.Enabled = False
        Me.txt_Amount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Amount.Location = New System.Drawing.Point(640, 39)
        Me.txt_Amount.MaxLength = 12
        Me.txt_Amount.Name = "txt_Amount"
        Me.txt_Amount.Size = New System.Drawing.Size(102, 20)
        Me.txt_Amount.TabIndex = 7
        Me.txt_Amount.Text = "txt_Amount"
        Me.txt_Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_Bales
        '
        Me.txt_Bales.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Bales.Location = New System.Drawing.Point(329, 40)
        Me.txt_Bales.MaxLength = 6
        Me.txt_Bales.Name = "txt_Bales"
        Me.txt_Bales.Size = New System.Drawing.Size(76, 20)
        Me.txt_Bales.TabIndex = 4
        Me.txt_Bales.Text = "txt_Bags"
        Me.txt_Bales.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cbo_ItemName
        '
        Me.cbo_ItemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_ItemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ItemName.FormattingEnabled = True
        Me.cbo_ItemName.Location = New System.Drawing.Point(20, 39)
        Me.cbo_ItemName.Name = "cbo_ItemName"
        Me.cbo_ItemName.Size = New System.Drawing.Size(289, 21)
        Me.cbo_ItemName.Sorted = True
        Me.cbo_ItemName.TabIndex = 3
        Me.cbo_ItemName.Text = "cbo_ItemName"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label15.Location = New System.Drawing.Point(657, 16)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(85, 13)
        Me.Label15.TabIndex = 6
        Me.Label15.Text = "Gross Amount"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label9.Location = New System.Drawing.Point(367, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(38, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Bales"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label8.Location = New System.Drawing.Point(17, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Variety Name"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label21.Location = New System.Drawing.Point(477, 16)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(47, 13)
        Me.Label21.TabIndex = 1
        Me.Label21.Text = "Weight"
        '
        'txt_Weight
        '
        Me.txt_Weight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Weight.Location = New System.Drawing.Point(425, 39)
        Me.txt_Weight.MaxLength = 12
        Me.txt_Weight.Name = "txt_Weight"
        Me.txt_Weight.Size = New System.Drawing.Size(99, 20)
        Me.txt_Weight.TabIndex = 5
        Me.txt_Weight.Text = "txt_Weight"
        Me.txt_Weight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cbo_SalesAc
        '
        Me.cbo_SalesAc.DropDownHeight = 150
        Me.cbo_SalesAc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_SalesAc.FormattingEnabled = True
        Me.cbo_SalesAc.IntegralHeight = False
        Me.cbo_SalesAc.Location = New System.Drawing.Point(137, 280)
        Me.cbo_SalesAc.MaxDropDownItems = 13
        Me.cbo_SalesAc.Name = "cbo_SalesAc"
        Me.cbo_SalesAc.Size = New System.Drawing.Size(327, 21)
        Me.cbo_SalesAc.Sorted = True
        Me.cbo_SalesAc.TabIndex = 9
        Me.cbo_SalesAc.Text = "cbo_SalesAc"
        '
        'cbo_Ledger
        '
        Me.cbo_Ledger.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Ledger.FormattingEnabled = True
        Me.cbo_Ledger.Location = New System.Drawing.Point(450, 45)
        Me.cbo_Ledger.MaxDropDownItems = 15
        Me.cbo_Ledger.Name = "cbo_Ledger"
        Me.cbo_Ledger.Size = New System.Drawing.Size(327, 21)
        Me.cbo_Ledger.Sorted = True
        Me.cbo_Ledger.TabIndex = 1
        Me.cbo_Ledger.Text = "cbo_Ledger"
        '
        'dtp_Date
        '
        Me.dtp_Date.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(678, 14)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(99, 20)
        Me.dtp_Date.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label4.Location = New System.Drawing.Point(578, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Invoice Date"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label7.Location = New System.Drawing.Point(13, 280)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(85, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Purchase A/c"
        '
        'txt_NetAmount
        '
        Me.txt_NetAmount.Enabled = False
        Me.txt_NetAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_NetAmount.Location = New System.Drawing.Point(651, 469)
        Me.txt_NetAmount.MaxLength = 12
        Me.txt_NetAmount.Name = "txt_NetAmount"
        Me.txt_NetAmount.Size = New System.Drawing.Size(139, 20)
        Me.txt_NetAmount.TabIndex = 18
        Me.txt_NetAmount.Text = "txt_NetAmount"
        Me.txt_NetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_RoundOff
        '
        Me.txt_RoundOff.Enabled = False
        Me.txt_RoundOff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_RoundOff.Location = New System.Drawing.Point(651, 437)
        Me.txt_RoundOff.MaxLength = 12
        Me.txt_RoundOff.Name = "txt_RoundOff"
        Me.txt_RoundOff.Size = New System.Drawing.Size(139, 20)
        Me.txt_RoundOff.TabIndex = 17
        Me.txt_RoundOff.Text = "txt_RoundOff"
        Me.txt_RoundOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label22.Location = New System.Drawing.Point(541, 471)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(77, 13)
        Me.Label22.TabIndex = 1
        Me.Label22.Text = "Net Amount "
        '
        'txt_AddLess_AfterTax
        '
        Me.txt_AddLess_AfterTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_AddLess_AfterTax.Location = New System.Drawing.Point(651, 404)
        Me.txt_AddLess_AfterTax.MaxLength = 12
        Me.txt_AddLess_AfterTax.Name = "txt_AddLess_AfterTax"
        Me.txt_AddLess_AfterTax.Size = New System.Drawing.Size(139, 20)
        Me.txt_AddLess_AfterTax.TabIndex = 16
        Me.txt_AddLess_AfterTax.Text = "txt_AddLess_AfterTax"
        Me.txt_AddLess_AfterTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label20.Location = New System.Drawing.Point(541, 441)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(69, 13)
        Me.Label20.TabIndex = 1
        Me.Label20.Text = "Round Off "
        '
        'txt_AddLess_BeforeTax
        '
        Me.txt_AddLess_BeforeTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_AddLess_BeforeTax.Location = New System.Drawing.Point(651, 277)
        Me.txt_AddLess_BeforeTax.Name = "txt_AddLess_BeforeTax"
        Me.txt_AddLess_BeforeTax.Size = New System.Drawing.Size(139, 20)
        Me.txt_AddLess_BeforeTax.TabIndex = 12
        Me.txt_AddLess_BeforeTax.Text = "txt_AddLess_BeforeTax"
        Me.txt_AddLess_BeforeTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label19.Location = New System.Drawing.Point(541, 408)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(73, 13)
        Me.Label19.TabIndex = 1
        Me.Label19.Text = "Add / Less "
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label18.Location = New System.Drawing.Point(541, 280)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(69, 13)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "Add / Less"
        '
        'txt_VehicleNo
        '
        Me.txt_VehicleNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_VehicleNo.Location = New System.Drawing.Point(137, 344)
        Me.txt_VehicleNo.MaxLength = 20
        Me.txt_VehicleNo.Name = "txt_VehicleNo"
        Me.txt_VehicleNo.Size = New System.Drawing.Size(328, 20)
        Me.txt_VehicleNo.TabIndex = 11
        Me.txt_VehicleNo.Text = "txt_VehicleNo"
        '
        'lbl_AmountInWords
        '
        Me.lbl_AmountInWords.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_AmountInWords.ForeColor = System.Drawing.Color.MediumBlue
        Me.lbl_AmountInWords.Location = New System.Drawing.Point(27, 501)
        Me.lbl_AmountInWords.Name = "lbl_AmountInWords"
        Me.lbl_AmountInWords.Size = New System.Drawing.Size(763, 18)
        Me.lbl_AmountInWords.TabIndex = 1
        Me.lbl_AmountInWords.Text = "Amount In Words :                                "
        Me.lbl_AmountInWords.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label6.Location = New System.Drawing.Point(13, 348)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Vehicle No."
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label5.Location = New System.Drawing.Point(344, 49)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Party Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label2.Location = New System.Drawing.Point(344, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Invoice No"
        '
        'Label1
        '
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(13, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(315, 89)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "PURCHASE ENTRY"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'pnl_Filter
        '
        Me.pnl_Filter.BackColor = System.Drawing.Color.White
        Me.pnl_Filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Filter.Controls.Add(Me.Label34)
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Close)
        Me.pnl_Filter.Controls.Add(Me.Label29)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_ItemName)
        Me.pnl_Filter.Controls.Add(Me.Label33)
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Show)
        Me.pnl_Filter.Controls.Add(Me.dgv_Filter_Details)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_PartyName)
        Me.pnl_Filter.Controls.Add(Me.Label32)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_ToDate)
        Me.pnl_Filter.Controls.Add(Me.Label31)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_Fromdate)
        Me.pnl_Filter.Controls.Add(Me.Label30)
        Me.pnl_Filter.Controls.Add(Me.ShapeContainer1)
        Me.pnl_Filter.Location = New System.Drawing.Point(20, 600)
        Me.pnl_Filter.Margin = New System.Windows.Forms.Padding(0)
        Me.pnl_Filter.Name = "pnl_Filter"
        Me.pnl_Filter.Size = New System.Drawing.Size(802, 384)
        Me.pnl_Filter.TabIndex = 4
        Me.pnl_Filter.Visible = False
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.BackColor = System.Drawing.Color.Purple
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.Color.White
        Me.Label34.Location = New System.Drawing.Point(363, 3)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(71, 20)
        Me.Label34.TabIndex = 41
        Me.Label34.Text = "FILTER"
        '
        'btn_Filter_Close
        '
        Me.btn_Filter_Close.BackColor = System.Drawing.Color.White
        Me.btn_Filter_Close.BackgroundImage = Global.Billing.My.Resources.Resources.Close1
        Me.btn_Filter_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Filter_Close.FlatAppearance.BorderSize = 0
        Me.btn_Filter_Close.Location = New System.Drawing.Point(772, 2)
        Me.btn_Filter_Close.Name = "btn_Filter_Close"
        Me.btn_Filter_Close.Size = New System.Drawing.Size(25, 25)
        Me.btn_Filter_Close.TabIndex = 40
        Me.btn_Filter_Close.UseVisualStyleBackColor = True
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.Purple
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.White
        Me.Label29.Location = New System.Drawing.Point(402, -36)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(71, 20)
        Me.Label29.TabIndex = 37
        Me.Label29.Text = "FILTER"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbo_Filter_ItemName
        '
        Me.cbo_Filter_ItemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Filter_ItemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Filter_ItemName.FormattingEnabled = True
        Me.cbo_Filter_ItemName.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.cbo_Filter_ItemName.Location = New System.Drawing.Point(114, 71)
        Me.cbo_Filter_ItemName.MaxDropDownItems = 15
        Me.cbo_Filter_ItemName.Name = "cbo_Filter_ItemName"
        Me.cbo_Filter_ItemName.Size = New System.Drawing.Size(257, 21)
        Me.cbo_Filter_ItemName.Sorted = True
        Me.cbo_Filter_ItemName.TabIndex = 29
        Me.cbo_Filter_ItemName.Text = "cbo_Filter_ItemName"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.Color.Blue
        Me.Label33.Location = New System.Drawing.Point(13, 72)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(75, 13)
        Me.Label33.TabIndex = 34
        Me.Label33.Text = "Item Name :"
        '
        'btn_Filter_Show
        '
        Me.btn_Filter_Show.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter_Show.ForeColor = System.Drawing.Color.Blue
        Me.btn_Filter_Show.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter_Show.Location = New System.Drawing.Point(725, 38)
        Me.btn_Filter_Show.Name = "btn_Filter_Show"
        Me.btn_Filter_Show.Size = New System.Drawing.Size(59, 68)
        Me.btn_Filter_Show.TabIndex = 30
        Me.btn_Filter_Show.Text = "&SHOW"
        Me.btn_Filter_Show.UseVisualStyleBackColor = False
        '
        'dgv_Filter_Details
        '
        Me.dgv_Filter_Details.AllowUserToAddRows = False
        Me.dgv_Filter_Details.AllowUserToOrderColumns = True
        Me.dgv_Filter_Details.AllowUserToResizeColumns = False
        Me.dgv_Filter_Details.AllowUserToResizeRows = False
        Me.dgv_Filter_Details.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgv_Filter_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Filter_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn7})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Filter_Details.DefaultCellStyle = DataGridViewCellStyle10
        Me.dgv_Filter_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter_Details.Location = New System.Drawing.Point(0, 120)
        Me.dgv_Filter_Details.MultiSelect = False
        Me.dgv_Filter_Details.Name = "dgv_Filter_Details"
        Me.dgv_Filter_Details.RowHeadersWidth = 15
        Me.dgv_Filter_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Filter_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter_Details.Size = New System.Drawing.Size(799, 262)
        Me.dgv_Filter_Details.TabIndex = 31
        Me.dgv_Filter_Details.TabStop = False
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "INV.NO"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 80
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "DATE"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn3.HeaderText = "PARTY NAME"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 200
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "COUNT NAME"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 125
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "BAGS"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Width = 60
        '
        'DataGridViewTextBoxColumn6
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn6.HeaderText = "WEIGHT"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.Width = 80
        '
        'DataGridViewTextBoxColumn7
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.NullValue = Nothing
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewTextBoxColumn7.HeaderText = "NET AMOUNT"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.Width = 110
        '
        'cbo_Filter_PartyName
        '
        Me.cbo_Filter_PartyName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Filter_PartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Filter_PartyName.FormattingEnabled = True
        Me.cbo_Filter_PartyName.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.cbo_Filter_PartyName.Location = New System.Drawing.Point(475, 38)
        Me.cbo_Filter_PartyName.MaxDropDownItems = 15
        Me.cbo_Filter_PartyName.Name = "cbo_Filter_PartyName"
        Me.cbo_Filter_PartyName.Size = New System.Drawing.Size(238, 21)
        Me.cbo_Filter_PartyName.Sorted = True
        Me.cbo_Filter_PartyName.TabIndex = 28
        Me.cbo_Filter_PartyName.Text = "cbo_Filter_PartyName"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ForeColor = System.Drawing.Color.Blue
        Me.Label32.Location = New System.Drawing.Point(385, 42)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(80, 13)
        Me.Label32.TabIndex = 30
        Me.Label32.Text = "Party Name :"
        '
        'dtp_Filter_ToDate
        '
        Me.dtp_Filter_ToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Filter_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_ToDate.Location = New System.Drawing.Point(273, 38)
        Me.dtp_Filter_ToDate.Name = "dtp_Filter_ToDate"
        Me.dtp_Filter_ToDate.Size = New System.Drawing.Size(99, 20)
        Me.dtp_Filter_ToDate.TabIndex = 27
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.ForeColor = System.Drawing.Color.Blue
        Me.Label31.Location = New System.Drawing.Point(238, 42)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(22, 13)
        Me.Label31.TabIndex = 29
        Me.Label31.Text = "To"
        '
        'dtp_Filter_Fromdate
        '
        Me.dtp_Filter_Fromdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Filter_Fromdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_Fromdate.Location = New System.Drawing.Point(127, 38)
        Me.dtp_Filter_Fromdate.Name = "dtp_Filter_Fromdate"
        Me.dtp_Filter_Fromdate.Size = New System.Drawing.Size(99, 20)
        Me.dtp_Filter_Fromdate.TabIndex = 26
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.ForeColor = System.Drawing.Color.Blue
        Me.Label30.Location = New System.Drawing.Point(13, 42)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(88, 13)
        Me.Label30.TabIndex = 27
        Me.Label30.Text = "Invoice Date :"
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(800, 382)
        Me.ShapeContainer1.TabIndex = 39
        Me.ShapeContainer1.TabStop = False
        '
        'RectangleShape1
        '
        Me.RectangleShape1.FillColor = System.Drawing.Color.Purple
        Me.RectangleShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid
        Me.RectangleShape1.Location = New System.Drawing.Point(0, 0)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(799, 25)
        '
        'Spinning_Purchase_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(827, 576)
        Me.Controls.Add(Me.pnl_Filter)
        Me.Controls.Add(Me.pnl_Back)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Spinning_Purchase_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PURCHASE"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnl_Filter.ResumeLayout(False)
        Me.pnl_Filter.PerformLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents cbo_Ledger As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbo_SalesAc As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents cbo_ItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_Bales As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txt_Amount As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txt_AddLess_AfterTax As System.Windows.Forms.TextBox
    Friend WithEvents txt_AddLess_BeforeTax As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txt_VehicleNo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txt_RoundOff As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txt_NetAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txt_Weight As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents lbl_AmountInWords As System.Windows.Forms.Label
    Friend WithEvents cbo_VatAc As System.Windows.Forms.ComboBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents txt_Assessable As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents lbl_InvoiceNo As System.Windows.Forms.Label
    Friend WithEvents pnl_Filter As System.Windows.Forms.Panel
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents btn_Filter_Close As System.Windows.Forms.Button
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents cbo_Filter_ItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents btn_Filter_Show As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter_Details As System.Windows.Forms.DataGridView
    Friend WithEvents cbo_Filter_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents dtp_Filter_ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents dtp_Filter_Fromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents txt_Freight As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txt_VatPerc As System.Windows.Forms.TextBox
    Friend WithEvents txt_VatAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txt_BaleNos As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txt_BillNo As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txt_Rate_Kg As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
