<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGSTR3B
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
        Me.cboGSTIN = New System.Windows.Forms.ComboBox()
        Me.cboACName = New System.Windows.Forms.ComboBox()
        Me.lblACName = New System.Windows.Forms.Label()
        Me.cboMonth = New System.Windows.Forms.ComboBox()
        Me.cboYear = New System.Windows.Forms.ComboBox()
        Me.lblYear = New System.Windows.Forms.Label()
        Me.lblMonth = New System.Windows.Forms.Label()
        Me.lblGSTIN = New System.Windows.Forms.Label()
        Me.lblTaxableValue = New System.Windows.Forms.Label()
        Me.txtTaxableValue = New System.Windows.Forms.TextBox()
        Me.txtIGST = New System.Windows.Forms.TextBox()
        Me.lblIGST = New System.Windows.Forms.Label()
        Me.txtCGST = New System.Windows.Forms.TextBox()
        Me.lblCGST = New System.Windows.Forms.Label()
        Me.txtSGST = New System.Windows.Forms.TextBox()
        Me.lblSGST = New System.Windows.Forms.Label()
        Me.txtITC_SGST = New System.Windows.Forms.TextBox()
        Me.lblITC_SGST = New System.Windows.Forms.Label()
        Me.txtITC_CGST = New System.Windows.Forms.TextBox()
        Me.lblITC_CGST = New System.Windows.Forms.Label()
        Me.txtITC_IGST = New System.Windows.Forms.TextBox()
        Me.lblITC_IGST = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.txtCESS = New System.Windows.Forms.TextBox()
        Me.lblCESS = New System.Windows.Forms.Label()
        Me.txtITC_CESS = New System.Windows.Forms.TextBox()
        Me.lblITC_CESS = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cboGSTIN
        '
        Me.cboGSTIN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboGSTIN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboGSTIN.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGSTIN.FormattingEnabled = True
        Me.cboGSTIN.Items.AddRange(New Object() {"January", "February", "March", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboGSTIN.Location = New System.Drawing.Point(498, 14)
        Me.cboGSTIN.Name = "cboGSTIN"
        Me.cboGSTIN.Size = New System.Drawing.Size(211, 24)
        Me.cboGSTIN.TabIndex = 11
        '
        'cboACName
        '
        Me.cboACName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboACName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboACName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboACName.FormattingEnabled = True
        Me.cboACName.Location = New System.Drawing.Point(98, 14)
        Me.cboACName.Name = "cboACName"
        Me.cboACName.Size = New System.Drawing.Size(326, 24)
        Me.cboACName.TabIndex = 9
        '
        'lblACName
        '
        Me.lblACName.AutoSize = True
        Me.lblACName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblACName.Location = New System.Drawing.Point(12, 18)
        Me.lblACName.Name = "lblACName"
        Me.lblACName.Size = New System.Drawing.Size(86, 16)
        Me.lblACName.TabIndex = 8
        Me.lblACName.Text = "A/C Name :"
        '
        'cboMonth
        '
        Me.cboMonth.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboMonth.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMonth.FormattingEnabled = True
        Me.cboMonth.Items.AddRange(New Object() {"January - March", "April - June", "July - September", "October - December", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboMonth.Location = New System.Drawing.Point(367, 44)
        Me.cboMonth.Name = "cboMonth"
        Me.cboMonth.Size = New System.Drawing.Size(342, 24)
        Me.cboMonth.TabIndex = 15
        '
        'cboYear
        '
        Me.cboYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboYear.FormattingEnabled = True
        Me.cboYear.Location = New System.Drawing.Point(98, 44)
        Me.cboYear.Name = "cboYear"
        Me.cboYear.Size = New System.Drawing.Size(189, 24)
        Me.cboYear.TabIndex = 13
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYear.Location = New System.Drawing.Point(14, 48)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(49, 16)
        Me.lblYear.TabIndex = 12
        Me.lblYear.Text = "Year :"
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonth.Location = New System.Drawing.Point(304, 47)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(57, 16)
        Me.lblMonth.TabIndex = 14
        Me.lblMonth.Text = "Month :"
        '
        'lblGSTIN
        '
        Me.lblGSTIN.AutoSize = True
        Me.lblGSTIN.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGSTIN.Location = New System.Drawing.Point(430, 18)
        Me.lblGSTIN.Name = "lblGSTIN"
        Me.lblGSTIN.Size = New System.Drawing.Size(62, 16)
        Me.lblGSTIN.TabIndex = 10
        Me.lblGSTIN.Text = "GSTIN :"
        '
        'lblTaxableValue
        '
        Me.lblTaxableValue.AutoSize = True
        Me.lblTaxableValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxableValue.Location = New System.Drawing.Point(13, 88)
        Me.lblTaxableValue.Name = "lblTaxableValue"
        Me.lblTaxableValue.Size = New System.Drawing.Size(157, 16)
        Me.lblTaxableValue.TabIndex = 16
        Me.lblTaxableValue.Text = "Total Taxable Value :"
        '
        'txtTaxableValue
        '
        Me.txtTaxableValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxableValue.Location = New System.Drawing.Point(176, 85)
        Me.txtTaxableValue.Name = "txtTaxableValue"
        Me.txtTaxableValue.Size = New System.Drawing.Size(148, 22)
        Me.txtTaxableValue.TabIndex = 17
        Me.txtTaxableValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtIGST
        '
        Me.txtIGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIGST.Location = New System.Drawing.Point(176, 113)
        Me.txtIGST.Name = "txtIGST"
        Me.txtIGST.Size = New System.Drawing.Size(148, 22)
        Me.txtIGST.TabIndex = 19
        Me.txtIGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblIGST
        '
        Me.lblIGST.AutoSize = True
        Me.lblIGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIGST.Location = New System.Drawing.Point(13, 116)
        Me.lblIGST.Name = "lblIGST"
        Me.lblIGST.Size = New System.Drawing.Size(51, 16)
        Me.lblIGST.TabIndex = 18
        Me.lblIGST.Text = "IGST :"
        '
        'txtCGST
        '
        Me.txtCGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCGST.Location = New System.Drawing.Point(176, 141)
        Me.txtCGST.Name = "txtCGST"
        Me.txtCGST.Size = New System.Drawing.Size(148, 22)
        Me.txtCGST.TabIndex = 21
        Me.txtCGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCGST
        '
        Me.lblCGST.AutoSize = True
        Me.lblCGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCGST.Location = New System.Drawing.Point(13, 144)
        Me.lblCGST.Name = "lblCGST"
        Me.lblCGST.Size = New System.Drawing.Size(57, 16)
        Me.lblCGST.TabIndex = 20
        Me.lblCGST.Text = "CGST :"
        '
        'txtSGST
        '
        Me.txtSGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSGST.Location = New System.Drawing.Point(176, 169)
        Me.txtSGST.Name = "txtSGST"
        Me.txtSGST.Size = New System.Drawing.Size(148, 22)
        Me.txtSGST.TabIndex = 23
        Me.txtSGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblSGST
        '
        Me.lblSGST.AutoSize = True
        Me.lblSGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSGST.Location = New System.Drawing.Point(13, 172)
        Me.lblSGST.Name = "lblSGST"
        Me.lblSGST.Size = New System.Drawing.Size(57, 16)
        Me.lblSGST.TabIndex = 22
        Me.lblSGST.Text = "SGST :"
        '
        'txtITC_SGST
        '
        Me.txtITC_SGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtITC_SGST.Location = New System.Drawing.Point(552, 166)
        Me.txtITC_SGST.Name = "txtITC_SGST"
        Me.txtITC_SGST.Size = New System.Drawing.Size(157, 22)
        Me.txtITC_SGST.TabIndex = 31
        Me.txtITC_SGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblITC_SGST
        '
        Me.lblITC_SGST.AutoSize = True
        Me.lblITC_SGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblITC_SGST.Location = New System.Drawing.Point(388, 169)
        Me.lblITC_SGST.Name = "lblITC_SGST"
        Me.lblITC_SGST.Size = New System.Drawing.Size(95, 16)
        Me.lblITC_SGST.TabIndex = 30
        Me.lblITC_SGST.Text = "ITC (SGST) :"
        '
        'txtITC_CGST
        '
        Me.txtITC_CGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtITC_CGST.Location = New System.Drawing.Point(552, 138)
        Me.txtITC_CGST.Name = "txtITC_CGST"
        Me.txtITC_CGST.Size = New System.Drawing.Size(157, 22)
        Me.txtITC_CGST.TabIndex = 29
        Me.txtITC_CGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblITC_CGST
        '
        Me.lblITC_CGST.AutoSize = True
        Me.lblITC_CGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblITC_CGST.Location = New System.Drawing.Point(389, 141)
        Me.lblITC_CGST.Name = "lblITC_CGST"
        Me.lblITC_CGST.Size = New System.Drawing.Size(95, 16)
        Me.lblITC_CGST.TabIndex = 28
        Me.lblITC_CGST.Text = "ITC (CGST) :"
        '
        'txtITC_IGST
        '
        Me.txtITC_IGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtITC_IGST.Location = New System.Drawing.Point(552, 110)
        Me.txtITC_IGST.Name = "txtITC_IGST"
        Me.txtITC_IGST.Size = New System.Drawing.Size(157, 22)
        Me.txtITC_IGST.TabIndex = 27
        Me.txtITC_IGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblITC_IGST
        '
        Me.lblITC_IGST.AutoSize = True
        Me.lblITC_IGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblITC_IGST.Location = New System.Drawing.Point(389, 113)
        Me.lblITC_IGST.Name = "lblITC_IGST"
        Me.lblITC_IGST.Size = New System.Drawing.Size(89, 16)
        Me.lblITC_IGST.TabIndex = 26
        Me.lblITC_IGST.Text = "ITC (IGST) :"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Button1.Font = New System.Drawing.Font("Modern No. 20", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(588, 233)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(122, 43)
        Me.Button1.TabIndex = 33
        Me.Button1.Text = "&X Close"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnRefresh.Font = New System.Drawing.Font("Modern No. 20", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(123, 233)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(122, 43)
        Me.btnRefresh.TabIndex = 32
        Me.btnRefresh.Text = "&Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'txtCESS
        '
        Me.txtCESS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCESS.Location = New System.Drawing.Point(176, 197)
        Me.txtCESS.Name = "txtCESS"
        Me.txtCESS.Size = New System.Drawing.Size(148, 22)
        Me.txtCESS.TabIndex = 35
        Me.txtCESS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCESS
        '
        Me.lblCESS.AutoSize = True
        Me.lblCESS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCESS.Location = New System.Drawing.Point(13, 200)
        Me.lblCESS.Name = "lblCESS"
        Me.lblCESS.Size = New System.Drawing.Size(56, 16)
        Me.lblCESS.TabIndex = 34
        Me.lblCESS.Text = "CESS :"
        '
        'txtITC_CESS
        '
        Me.txtITC_CESS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtITC_CESS.Location = New System.Drawing.Point(552, 194)
        Me.txtITC_CESS.Name = "txtITC_CESS"
        Me.txtITC_CESS.Size = New System.Drawing.Size(157, 22)
        Me.txtITC_CESS.TabIndex = 37
        Me.txtITC_CESS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblITC_CESS
        '
        Me.lblITC_CESS.AutoSize = True
        Me.lblITC_CESS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblITC_CESS.Location = New System.Drawing.Point(389, 197)
        Me.lblITC_CESS.Name = "lblITC_CESS"
        Me.lblITC_CESS.Size = New System.Drawing.Size(94, 16)
        Me.lblITC_CESS.TabIndex = 36
        Me.lblITC_CESS.Text = "ITC (CESS) :"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Button2.Font = New System.Drawing.Font("Modern No. 20", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(251, 233)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(331, 43)
        Me.Button2.TabIndex = 38
        Me.Button2.Text = "&Save and Fill GSTR 3B offline Template"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'frmGSTR3B
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(722, 283)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.txtITC_CESS)
        Me.Controls.Add(Me.lblITC_CESS)
        Me.Controls.Add(Me.txtCESS)
        Me.Controls.Add(Me.lblCESS)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.txtITC_SGST)
        Me.Controls.Add(Me.lblITC_SGST)
        Me.Controls.Add(Me.txtITC_CGST)
        Me.Controls.Add(Me.lblITC_CGST)
        Me.Controls.Add(Me.txtITC_IGST)
        Me.Controls.Add(Me.lblITC_IGST)
        Me.Controls.Add(Me.txtSGST)
        Me.Controls.Add(Me.lblSGST)
        Me.Controls.Add(Me.txtCGST)
        Me.Controls.Add(Me.lblCGST)
        Me.Controls.Add(Me.txtIGST)
        Me.Controls.Add(Me.lblIGST)
        Me.Controls.Add(Me.txtTaxableValue)
        Me.Controls.Add(Me.lblTaxableValue)
        Me.Controls.Add(Me.cboGSTIN)
        Me.Controls.Add(Me.cboACName)
        Me.Controls.Add(Me.lblACName)
        Me.Controls.Add(Me.cboMonth)
        Me.Controls.Add(Me.cboYear)
        Me.Controls.Add(Me.lblYear)
        Me.Controls.Add(Me.lblMonth)
        Me.Controls.Add(Me.lblGSTIN)
        Me.Name = "frmGSTR3B"
        Me.Text = "GSTR3B"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboGSTIN As System.Windows.Forms.ComboBox
    Friend WithEvents cboACName As System.Windows.Forms.ComboBox
    Friend WithEvents lblACName As System.Windows.Forms.Label
    Friend WithEvents cboMonth As System.Windows.Forms.ComboBox
    Friend WithEvents cboYear As System.Windows.Forms.ComboBox
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents lblMonth As System.Windows.Forms.Label
    Friend WithEvents lblGSTIN As System.Windows.Forms.Label
    Friend WithEvents lblTaxableValue As System.Windows.Forms.Label
    Friend WithEvents txtTaxableValue As System.Windows.Forms.TextBox
    Friend WithEvents txtIGST As System.Windows.Forms.TextBox
    Friend WithEvents lblIGST As System.Windows.Forms.Label
    Friend WithEvents txtCGST As System.Windows.Forms.TextBox
    Friend WithEvents lblCGST As System.Windows.Forms.Label
    Friend WithEvents txtSGST As System.Windows.Forms.TextBox
    Friend WithEvents lblSGST As System.Windows.Forms.Label
    Friend WithEvents txtITC_SGST As System.Windows.Forms.TextBox
    Friend WithEvents lblITC_SGST As System.Windows.Forms.Label
    Friend WithEvents txtITC_CGST As System.Windows.Forms.TextBox
    Friend WithEvents lblITC_CGST As System.Windows.Forms.Label
    Friend WithEvents txtITC_IGST As System.Windows.Forms.TextBox
    Friend WithEvents lblITC_IGST As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents txtCESS As System.Windows.Forms.TextBox
    Friend WithEvents lblCESS As System.Windows.Forms.Label
    Friend WithEvents txtITC_CESS As System.Windows.Forms.TextBox
    Friend WithEvents lblITC_CESS As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
