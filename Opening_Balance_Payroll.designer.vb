<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Opening_Balance_Payroll
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
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_OpnWeekOff_Credits = New System.Windows.Forms.TextBox()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txt_OpnML_Leaves = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txt_OpnCL_Leaves = New System.Windows.Forms.TextBox()
        Me.txt_OpnSalaryForBonus = New System.Windows.Forms.TextBox()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.cbo_Name = New System.Windows.Forms.ComboBox()
        Me.cbo_CrDrType = New System.Windows.Forms.ComboBox()
        Me.txt_OpnAmountSalAdvance = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.lbl_UserName = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.AutoScroll = True
        Me.pnl_Back.AutoSize = True
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.txt_OpnWeekOff_Credits)
        Me.pnl_Back.Controls.Add(Me.lbl_Company)
        Me.pnl_Back.Controls.Add(Me.btnSave)
        Me.pnl_Back.Controls.Add(Me.btnClose)
        Me.pnl_Back.Controls.Add(Me.txt_OpnML_Leaves)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.txt_OpnCL_Leaves)
        Me.pnl_Back.Controls.Add(Me.txt_OpnSalaryForBonus)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.cbo_Name)
        Me.pnl_Back.Controls.Add(Me.cbo_CrDrType)
        Me.pnl_Back.Controls.Add(Me.txt_OpnAmountSalAdvance)
        Me.pnl_Back.Controls.Add(Me.Label19)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label9)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.Label56)
        Me.pnl_Back.Enabled = False
        Me.pnl_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_Back.Location = New System.Drawing.Point(6, 40)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(412, 351)
        Me.pnl_Back.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(19, 173)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(109, 15)
        Me.Label8.TabIndex = 45
        Me.Label8.Text = "Opening CL Leaves"
        '
        'txt_OpnWeekOff_Credits
        '
        Me.txt_OpnWeekOff_Credits.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_OpnWeekOff_Credits.Location = New System.Drawing.Point(194, 241)
        Me.txt_OpnWeekOff_Credits.Name = "txt_OpnWeekOff_Credits"
        Me.txt_OpnWeekOff_Credits.Size = New System.Drawing.Size(195, 23)
        Me.txt_OpnWeekOff_Credits.TabIndex = 6
        Me.txt_OpnWeekOff_Credits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.ForeColor = System.Drawing.Color.White
        Me.lbl_Company.Location = New System.Drawing.Point(297, 17)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 33
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'btnSave
        '
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.Navy
        Me.btnSave.Image = Global.Billing.My.Resources.Resources.SAVE1
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(194, 292)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(76, 31)
        Me.btnSave.TabIndex = 7
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Navy
        Me.btnClose.Image = Global.Billing.My.Resources.Resources.cancel1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(300, 292)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(76, 31)
        Me.btnClose.TabIndex = 8
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txt_OpnML_Leaves
        '
        Me.txt_OpnML_Leaves.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_OpnML_Leaves.Location = New System.Drawing.Point(194, 203)
        Me.txt_OpnML_Leaves.Name = "txt_OpnML_Leaves"
        Me.txt_OpnML_Leaves.Size = New System.Drawing.Size(195, 23)
        Me.txt_OpnML_Leaves.TabIndex = 5
        Me.txt_OpnML_Leaves.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(279, 92)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 15)
        Me.Label6.TabIndex = 39
        Me.Label6.Text = "Cr/Dr"
        '
        'txt_OpnCL_Leaves
        '
        Me.txt_OpnCL_Leaves.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_OpnCL_Leaves.Location = New System.Drawing.Point(194, 165)
        Me.txt_OpnCL_Leaves.Name = "txt_OpnCL_Leaves"
        Me.txt_OpnCL_Leaves.Size = New System.Drawing.Size(195, 23)
        Me.txt_OpnCL_Leaves.TabIndex = 4
        Me.txt_OpnCL_Leaves.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_OpnSalaryForBonus
        '
        Me.txt_OpnSalaryForBonus.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_OpnSalaryForBonus.Location = New System.Drawing.Point(194, 127)
        Me.txt_OpnSalaryForBonus.Name = "txt_OpnSalaryForBonus"
        Me.txt_OpnSalaryForBonus.Size = New System.Drawing.Size(195, 23)
        Me.txt_OpnSalaryForBonus.TabIndex = 3
        Me.txt_OpnSalaryForBonus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_IdNo.Location = New System.Drawing.Point(139, 13)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(73, 23)
        Me.lbl_IdNo.TabIndex = 0
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbo_Name
        '
        Me.cbo_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Name.FormattingEnabled = True
        Me.cbo_Name.Location = New System.Drawing.Point(139, 51)
        Me.cbo_Name.MaxDropDownItems = 15
        Me.cbo_Name.Name = "cbo_Name"
        Me.cbo_Name.Size = New System.Drawing.Size(250, 23)
        Me.cbo_Name.Sorted = True
        Me.cbo_Name.TabIndex = 0
        '
        'cbo_CrDrType
        '
        Me.cbo_CrDrType.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_CrDrType.FormattingEnabled = True
        Me.cbo_CrDrType.Items.AddRange(New Object() {"Cr", "Dr"})
        Me.cbo_CrDrType.Location = New System.Drawing.Point(323, 89)
        Me.cbo_CrDrType.Name = "cbo_CrDrType"
        Me.cbo_CrDrType.Size = New System.Drawing.Size(66, 23)
        Me.cbo_CrDrType.TabIndex = 2
        '
        'txt_OpnAmountSalAdvance
        '
        Me.txt_OpnAmountSalAdvance.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_OpnAmountSalAdvance.Location = New System.Drawing.Point(139, 89)
        Me.txt_OpnAmountSalAdvance.Name = "txt_OpnAmountSalAdvance"
        Me.txt_OpnAmountSalAdvance.Size = New System.Drawing.Size(134, 23)
        Me.txt_OpnAmountSalAdvance.TabIndex = 1
        Me.txt_OpnAmountSalAdvance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.Blue
        Me.Label19.Location = New System.Drawing.Point(19, 92)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(100, 32)
        Me.Label19.TabIndex = 1
        Me.Label19.Text = "Opening Balance (Sal. Advance)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(19, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(97, 15)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Employee Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(19, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "IdNo "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(19, 208)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(113, 15)
        Me.Label9.TabIndex = 47
        Me.Label9.Text = "Opening ML Leaves"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(19, 130)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(169, 23)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "Opening Salary (For Bonus)"
        '
        'Label56
        '
        Me.Label56.ForeColor = System.Drawing.Color.Blue
        Me.Label56.Location = New System.Drawing.Point(19, 243)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(152, 30)
        Me.Label56.TabIndex = 77
        Me.Label56.Text = "Opening  Week off Credit" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lbl_Heading
        '
        Me.lbl_Heading.AutoEllipsis = True
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(435, 30)
        Me.lbl_Heading.TabIndex = 35
        Me.lbl_Heading.Text = "OPENING BALANCE"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(91, Byte), Integer))
        Me.lbl_UserName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.Color.White
        Me.lbl_UserName.Location = New System.Drawing.Point(784, 7)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(105, 19)
        Me.lbl_UserName.TabIndex = 268
        Me.lbl_UserName.Text = "USER : ADMIN"
        '
        'Opening_Balance_Payroll
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(435, 408)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.lbl_Heading)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Opening_Balance_Payroll"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OPENING BALANCE"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbo_Name As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_CrDrType As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txt_OpnAmountSalAdvance As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents txt_OpnML_Leaves As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txt_OpnCL_Leaves As System.Windows.Forms.TextBox
    Friend WithEvents txt_OpnSalaryForBonus As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents txt_OpnWeekOff_Credits As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
End Class
