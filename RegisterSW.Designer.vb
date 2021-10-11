<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RegisterSW
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_OTP = New System.Windows.Forms.TextBox()
        Me.btn_Register = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtp_ValidUpto = New System.Windows.Forms.DateTimePicker()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Registration OTP"
        '
        'txt_OTP
        '
        Me.txt_OTP.Location = New System.Drawing.Point(15, 35)
        Me.txt_OTP.Multiline = True
        Me.txt_OTP.Name = "txt_OTP"
        Me.txt_OTP.Size = New System.Drawing.Size(391, 35)
        Me.txt_OTP.TabIndex = 1
        '
        'btn_Register
        '
        Me.btn_Register.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Register.Location = New System.Drawing.Point(15, 112)
        Me.btn_Register.Name = "btn_Register"
        Me.btn_Register.Size = New System.Drawing.Size(391, 30)
        Me.btn_Register.TabIndex = 2
        Me.btn_Register.Text = "Register"
        Me.btn_Register.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(15, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(116, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Registration OTP"
        '
        'dtp_ValidUpto
        '
        Me.dtp_ValidUpto.CustomFormat = "dd-MM-yyyy"
        Me.dtp_ValidUpto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_ValidUpto.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_ValidUpto.Location = New System.Drawing.Point(137, 79)
        Me.dtp_ValidUpto.Name = "dtp_ValidUpto"
        Me.dtp_ValidUpto.Size = New System.Drawing.Size(268, 20)
        Me.dtp_ValidUpto.TabIndex = 4
        '
        'RegisterSW
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(418, 153)
        Me.Controls.Add(Me.dtp_ValidUpto)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btn_Register)
        Me.Controls.Add(Me.txt_OTP)
        Me.Controls.Add(Me.Label1)
        Me.Name = "RegisterSW"
        Me.Text = "Register SW"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_OTP As System.Windows.Forms.TextBox
    Friend WithEvents btn_Register As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtp_ValidUpto As System.Windows.Forms.DateTimePicker
End Class
