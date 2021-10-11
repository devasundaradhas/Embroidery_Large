<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Change_Period
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
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_ChangePeriod = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lbl_ToYear = New System.Windows.Forms.Label()
        Me.txt_FromYear = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.btn_close)
        Me.pnl_Back.Controls.Add(Me.btn_ChangePeriod)
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.lbl_ToYear)
        Me.pnl_Back.Controls.Add(Me.txt_FromYear)
        Me.pnl_Back.Controls.Add(Me.Label7)
        Me.pnl_Back.Location = New System.Drawing.Point(7, 8)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(316, 207)
        Me.pnl_Back.TabIndex = 225
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(212, 144)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(77, 35)
        Me.btn_close.TabIndex = 2
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_ChangePeriod
        '
        Me.btn_ChangePeriod.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_ChangePeriod.ForeColor = System.Drawing.Color.White
        Me.btn_ChangePeriod.Location = New System.Drawing.Point(105, 144)
        Me.btn_ChangePeriod.Name = "btn_ChangePeriod"
        Me.btn_ChangePeriod.Size = New System.Drawing.Size(101, 35)
        Me.btn_ChangePeriod.TabIndex = 1
        Me.btn_ChangePeriod.TabStop = False
        Me.btn_ChangePeriod.Text = "CHANGE &YEAR"
        Me.btn_ChangePeriod.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(17, 92)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 15)
        Me.Label8.TabIndex = 228
        Me.Label8.Text = "To Year"
        '
        'lbl_ToYear
        '
        Me.lbl_ToYear.BackColor = System.Drawing.Color.White
        Me.lbl_ToYear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_ToYear.Location = New System.Drawing.Point(105, 88)
        Me.lbl_ToYear.Name = "lbl_ToYear"
        Me.lbl_ToYear.Size = New System.Drawing.Size(184, 23)
        Me.lbl_ToYear.TabIndex = 227
        Me.lbl_ToYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txt_FromYear
        '
        Me.txt_FromYear.Location = New System.Drawing.Point(105, 32)
        Me.txt_FromYear.MaxLength = 4
        Me.txt_FromYear.Name = "txt_FromYear"
        Me.txt_FromYear.Size = New System.Drawing.Size(184, 23)
        Me.txt_FromYear.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Blue
        Me.Label7.Location = New System.Drawing.Point(17, 36)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(63, 15)
        Me.Label7.TabIndex = 226
        Me.Label7.Text = "From Year"
        '
        'Change_Period
        '
        Me.AcceptButton = Me.btn_ChangePeriod
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.CancelButton = Me.btn_close
        Me.ClientSize = New System.Drawing.Size(330, 226)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Change_Period"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CHANGE PERIOD"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_ToYear As System.Windows.Forms.Label
    Friend WithEvents txt_FromYear As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_ChangePeriod As System.Windows.Forms.Button
End Class
