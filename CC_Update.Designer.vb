<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CC_Update
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
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.lbl_CurrentCC = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_UPDATE = New System.Windows.Forms.Button()
        Me.txt_NewCC = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Gray
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(304, 35)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Customer Code"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.lbl_CurrentCC)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_UPDATE)
        Me.pnl_Back.Controls.Add(Me.txt_NewCC)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Location = New System.Drawing.Point(5, 49)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(283, 145)
        Me.pnl_Back.TabIndex = 7
        '
        'lbl_CurrentCC
        '
        Me.lbl_CurrentCC.BackColor = System.Drawing.Color.LightGray
        Me.lbl_CurrentCC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_CurrentCC.ForeColor = System.Drawing.Color.OrangeRed
        Me.lbl_CurrentCC.Location = New System.Drawing.Point(89, 17)
        Me.lbl_CurrentCC.Name = "lbl_CurrentCC"
        Me.lbl_CurrentCC.Size = New System.Drawing.Size(163, 23)
        Me.lbl_CurrentCC.TabIndex = 7
        Me.lbl_CurrentCC.Text = "1011"
        Me.lbl_CurrentCC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 15)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Current CC :"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.Gray
        Me.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Close.FlatAppearance.BorderColor = System.Drawing.Color.Blue
        Me.btn_Close.FlatAppearance.BorderSize = 2
        Me.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow
        Me.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(144, 98)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(108, 32)
        Me.btn_Close.TabIndex = 2
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_UPDATE
        '
        Me.btn_UPDATE.BackColor = System.Drawing.Color.Gray
        Me.btn_UPDATE.FlatAppearance.BorderSize = 2
        Me.btn_UPDATE.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_UPDATE.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_UPDATE.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_UPDATE.ForeColor = System.Drawing.Color.White
        Me.btn_UPDATE.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_UPDATE.Location = New System.Drawing.Point(21, 98)
        Me.btn_UPDATE.Name = "btn_UPDATE"
        Me.btn_UPDATE.Size = New System.Drawing.Size(108, 32)
        Me.btn_UPDATE.TabIndex = 1
        Me.btn_UPDATE.TabStop = False
        Me.btn_UPDATE.Text = "&UPDATE"
        Me.btn_UPDATE.UseVisualStyleBackColor = False
        '
        'txt_NewCC
        '
        Me.txt_NewCC.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_NewCC.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_NewCC.Location = New System.Drawing.Point(89, 56)
        Me.txt_NewCC.MaxLength = 40
        Me.txt_NewCC.Name = "txt_NewCC"
        Me.txt_NewCC.Size = New System.Drawing.Size(163, 23)
        Me.txt_NewCC.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "New CC :"
        '
        'CC_Update
        '
        Me.AcceptButton = Me.btn_UPDATE
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.CancelButton = Me.btn_Close
        Me.ClientSize = New System.Drawing.Size(304, 210)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "CC_Update"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CC_Update"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents lbl_CurrentCC As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_UPDATE As System.Windows.Forms.Button
    Friend WithEvents txt_NewCC As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
