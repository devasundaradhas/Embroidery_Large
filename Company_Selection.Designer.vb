<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Company_Selection
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
        Me.btn_Cancel = New System.Windows.Forms.Button()
        Me.btn_OK = New System.Windows.Forms.Button()
        Me.cbo_Company = New System.Windows.Forms.ComboBox()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.White
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnl_Back.Controls.Add(Me.btn_Cancel)
        Me.pnl_Back.Controls.Add(Me.btn_OK)
        Me.pnl_Back.Controls.Add(Me.cbo_Company)
        Me.pnl_Back.Location = New System.Drawing.Point(2, 5)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(253, 291)
        Me.pnl_Back.TabIndex = 0
        '
        'btn_Cancel
        '
        Me.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Cancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Cancel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btn_Cancel.Location = New System.Drawing.Point(129, 251)
        Me.btn_Cancel.Name = "btn_Cancel"
        Me.btn_Cancel.Size = New System.Drawing.Size(110, 30)
        Me.btn_Cancel.TabIndex = 2
        Me.btn_Cancel.Text = "&CANCEL"
        Me.btn_Cancel.UseVisualStyleBackColor = True
        '
        'btn_OK
        '
        Me.btn_OK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_OK.ForeColor = System.Drawing.Color.Green
        Me.btn_OK.Location = New System.Drawing.Point(10, 251)
        Me.btn_OK.Name = "btn_OK"
        Me.btn_OK.Size = New System.Drawing.Size(110, 30)
        Me.btn_OK.TabIndex = 1
        Me.btn_OK.Text = "&OK"
        Me.btn_OK.UseVisualStyleBackColor = True
        '
        'cbo_Company
        '
        Me.cbo_Company.BackColor = System.Drawing.Color.White
        Me.cbo_Company.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cbo_Company.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Company.FormattingEnabled = True
        Me.cbo_Company.Location = New System.Drawing.Point(10, 10)
        Me.cbo_Company.Name = "cbo_Company"
        Me.cbo_Company.Size = New System.Drawing.Size(229, 236)
        Me.cbo_Company.TabIndex = 0
        Me.cbo_Company.Text = "cbo_Company"
        '
        'Company_Selection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btn_Cancel
        Me.ClientSize = New System.Drawing.Size(258, 300)
        Me.Controls.Add(Me.pnl_Back)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "Company_Selection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "COMPANY SELECTION"
        Me.pnl_Back.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents cbo_Company As System.Windows.Forms.ComboBox
    Friend WithEvents btn_Cancel As System.Windows.Forms.Button
    Friend WithEvents btn_OK As System.Windows.Forms.Button
End Class
