<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Style_Creation
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
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.grp_filter = New System.Windows.Forms.GroupBox()
        Me.btn_FilterClose = New System.Windows.Forms.Button()
        Me.btn_FilterOpen = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grp_Find = New System.Windows.Forms.GroupBox()
        Me.btn_FindClose = New System.Windows.Forms.Button()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grp_filter.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Find.SuspendLayout()
        Me.pnl_back.SuspendLayout()
        Me.SuspendLayout()
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.Khaki
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.Location = New System.Drawing.Point(330, 137)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(87, 27)
        Me.btn_Close.TabIndex = 2
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.Khaki
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.Location = New System.Drawing.Point(212, 137)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(87, 27)
        Me.btn_Save.TabIndex = 1
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'grp_filter
        '
        Me.grp_filter.BackColor = System.Drawing.Color.LightCyan
        Me.grp_filter.Controls.Add(Me.btn_FilterClose)
        Me.grp_filter.Controls.Add(Me.btn_FilterOpen)
        Me.grp_filter.Controls.Add(Me.dgv_filter)
        Me.grp_filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_filter.Location = New System.Drawing.Point(510, 161)
        Me.grp_filter.Name = "grp_filter"
        Me.grp_filter.Size = New System.Drawing.Size(453, 188)
        Me.grp_filter.TabIndex = 17
        Me.grp_filter.TabStop = False
        Me.grp_filter.Text = "FILTER"
        '
        'btn_FilterClose
        '
        Me.btn_FilterClose.BackColor = System.Drawing.Color.Khaki
        Me.btn_FilterClose.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_FilterClose.Location = New System.Drawing.Point(369, 155)
        Me.btn_FilterClose.Name = "btn_FilterClose"
        Me.btn_FilterClose.Size = New System.Drawing.Size(77, 27)
        Me.btn_FilterClose.TabIndex = 2
        Me.btn_FilterClose.Text = "&CLOSE"
        Me.btn_FilterClose.UseVisualStyleBackColor = False
        '
        'btn_FilterOpen
        '
        Me.btn_FilterOpen.BackColor = System.Drawing.Color.Khaki
        Me.btn_FilterOpen.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_FilterOpen.Location = New System.Drawing.Point(253, 155)
        Me.btn_FilterOpen.Name = "btn_FilterOpen"
        Me.btn_FilterOpen.Size = New System.Drawing.Size(87, 27)
        Me.btn_FilterOpen.TabIndex = 1
        Me.btn_FilterOpen.Text = "&OPEN"
        Me.btn_FilterOpen.UseVisualStyleBackColor = False
        '
        'dgv_filter
        '
        Me.dgv_filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Location = New System.Drawing.Point(22, 21)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(422, 122)
        Me.dgv_filter.TabIndex = 0
        '
        'txt_Name
        '
        Me.txt_Name.BackColor = System.Drawing.Color.White
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(113, 97)
        Me.txt_Name.MaxLength = 35
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(304, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_IdNo.Location = New System.Drawing.Point(113, 35)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(304, 27)
        Me.lbl_IdNo.TabIndex = 2
        Me.lbl_IdNo.Text = "lbl_idno"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(23, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Name"
        '
        'grp_Find
        '
        Me.grp_Find.BackColor = System.Drawing.Color.LightCyan
        Me.grp_Find.Controls.Add(Me.btn_FindClose)
        Me.grp_Find.Controls.Add(Me.btn_Open)
        Me.grp_Find.Controls.Add(Me.cbo_Find)
        Me.grp_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Find.Location = New System.Drawing.Point(14, 243)
        Me.grp_Find.Name = "grp_Find"
        Me.grp_Find.Size = New System.Drawing.Size(483, 188)
        Me.grp_Find.TabIndex = 19
        Me.grp_Find.TabStop = False
        Me.grp_Find.Text = "FINDING"
        '
        'btn_FindClose
        '
        Me.btn_FindClose.BackColor = System.Drawing.Color.Khaki
        Me.btn_FindClose.Location = New System.Drawing.Point(369, 104)
        Me.btn_FindClose.Name = "btn_FindClose"
        Me.btn_FindClose.Size = New System.Drawing.Size(87, 27)
        Me.btn_FindClose.TabIndex = 2
        Me.btn_FindClose.Text = "CLOSE"
        Me.btn_FindClose.UseVisualStyleBackColor = False
        '
        'btn_Open
        '
        Me.btn_Open.BackColor = System.Drawing.Color.Khaki
        Me.btn_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Open.Location = New System.Drawing.Point(253, 104)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(87, 27)
        Me.btn_Open.TabIndex = 1
        Me.btn_Open.Text = "&OPEN"
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(7, 36)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(448, 23)
        Me.cbo_Find.TabIndex = 0
        Me.cbo_Find.Text = "cbo_find"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Khaki
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(510, 35)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "STYLE CREATION"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_back
        '
        Me.pnl_back.BackColor = System.Drawing.Color.LightCyan
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.btn_Close)
        Me.pnl_back.Controls.Add(Me.btn_Save)
        Me.pnl_back.Controls.Add(Me.txt_Name)
        Me.pnl_back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_back.Location = New System.Drawing.Point(12, 50)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(483, 182)
        Me.pnl_back.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(23, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        '
        'Style_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightCyan
        Me.ClientSize = New System.Drawing.Size(510, 443)
        Me.Controls.Add(Me.grp_filter)
        Me.Controls.Add(Me.grp_Find)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.pnl_back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Style_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "STYLE CREATION"
        Me.grp_filter.ResumeLayout(False)
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Find.ResumeLayout(False)
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents grp_filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FilterClose As System.Windows.Forms.Button
    Friend WithEvents btn_FilterOpen As System.Windows.Forms.Button
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grp_Find As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FindClose As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
