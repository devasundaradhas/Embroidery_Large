<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Agent_Creation
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
        Me.Label8 = New System.Windows.Forms.Label()
        Me.grp_open = New System.Windows.Forms.GroupBox()
        Me.btn_find_close = New System.Windows.Forms.Button()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_open = New System.Windows.Forms.ComboBox()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.btn_OpenFilter = New System.Windows.Forms.Button()
        Me.Panel_back = New System.Windows.Forms.Panel()
        Me.cbo_area = New System.Windows.Forms.ComboBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.txt_Alaisname = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.lbl_idno = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.txt_pan = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.cbo_group = New System.Windows.Forms.ComboBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txt_emailid = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txt_phoneno = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txt_address4 = New System.Windows.Forms.TextBox()
        Me.txt_address3 = New System.Windows.Forms.TextBox()
        Me.txt_address2 = New System.Windows.Forms.TextBox()
        Me.txt_Address1 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grp_open.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_back.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Gray
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(659, 35)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "AGENT CREATION"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grp_open
        '
        Me.grp_open.Controls.Add(Me.btn_find_close)
        Me.grp_open.Controls.Add(Me.btn_Find)
        Me.grp_open.Controls.Add(Me.cbo_open)
        Me.grp_open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_open.Location = New System.Drawing.Point(666, 412)
        Me.grp_open.Name = "grp_open"
        Me.grp_open.Size = New System.Drawing.Size(532, 301)
        Me.grp_open.TabIndex = 0
        Me.grp_open.TabStop = False
        Me.grp_open.Text = "Finding"
        '
        'btn_find_close
        '
        Me.btn_find_close.BackColor = System.Drawing.Color.Gray
        Me.btn_find_close.ForeColor = System.Drawing.Color.White
        Me.btn_find_close.Location = New System.Drawing.Point(421, 199)
        Me.btn_find_close.Name = "btn_find_close"
        Me.btn_find_close.Size = New System.Drawing.Size(98, 40)
        Me.btn_find_close.TabIndex = 2
        Me.btn_find_close.Text = "&CLOSE"
        Me.btn_find_close.UseVisualStyleBackColor = False
        '
        'btn_Find
        '
        Me.btn_Find.BackColor = System.Drawing.Color.Gray
        Me.btn_Find.ForeColor = System.Drawing.Color.White
        Me.btn_Find.Location = New System.Drawing.Point(297, 199)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(109, 40)
        Me.btn_Find.TabIndex = 1
        Me.btn_Find.Text = "&FIND"
        Me.btn_Find.UseVisualStyleBackColor = False
        '
        'cbo_open
        '
        Me.cbo_open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_open.FormattingEnabled = True
        Me.cbo_open.IntegralHeight = False
        Me.cbo_open.Location = New System.Drawing.Point(39, 63)
        Me.cbo_open.Name = "cbo_open"
        Me.cbo_open.Size = New System.Drawing.Size(480, 23)
        Me.cbo_open.Sorted = True
        Me.cbo_open.TabIndex = 0
        Me.cbo_open.Text = "cbo_open"
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.btn_OpenFilter)
        Me.grp_Filter.Location = New System.Drawing.Point(666, 50)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(516, 356)
        Me.grp_Filter.TabIndex = 38
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.AllowUserToResizeColumns = False
        Me.dgv_Filter.AllowUserToResizeRows = False
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.Location = New System.Drawing.Point(9, 16)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(494, 277)
        Me.dgv_Filter.TabIndex = 0
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.Gray
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.ForeColor = System.Drawing.Color.White
        Me.btn_CloseFilter.Location = New System.Drawing.Point(409, 304)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(94, 40)
        Me.btn_CloseFilter.TabIndex = 2
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'btn_OpenFilter
        '
        Me.btn_OpenFilter.BackColor = System.Drawing.Color.Gray
        Me.btn_OpenFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_OpenFilter.ForeColor = System.Drawing.Color.White
        Me.btn_OpenFilter.Location = New System.Drawing.Point(269, 306)
        Me.btn_OpenFilter.Name = "btn_OpenFilter"
        Me.btn_OpenFilter.Size = New System.Drawing.Size(96, 39)
        Me.btn_OpenFilter.TabIndex = 1
        Me.btn_OpenFilter.Text = "&OPEN"
        Me.btn_OpenFilter.UseVisualStyleBackColor = False
        '
        'Panel_back
        '
        Me.Panel_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel_back.Controls.Add(Me.cbo_area)
        Me.Panel_back.Controls.Add(Me.Label30)
        Me.Panel_back.Controls.Add(Me.txt_Alaisname)
        Me.Panel_back.Controls.Add(Me.Label29)
        Me.Panel_back.Controls.Add(Me.lbl_idno)
        Me.Panel_back.Controls.Add(Me.Label27)
        Me.Panel_back.Controls.Add(Me.btn_close)
        Me.Panel_back.Controls.Add(Me.btn_Save)
        Me.Panel_back.Controls.Add(Me.txt_pan)
        Me.Panel_back.Controls.Add(Me.Label21)
        Me.Panel_back.Controls.Add(Me.cbo_group)
        Me.Panel_back.Controls.Add(Me.Label20)
        Me.Panel_back.Controls.Add(Me.txt_emailid)
        Me.Panel_back.Controls.Add(Me.Label19)
        Me.Panel_back.Controls.Add(Me.txt_phoneno)
        Me.Panel_back.Controls.Add(Me.Label18)
        Me.Panel_back.Controls.Add(Me.txt_address4)
        Me.Panel_back.Controls.Add(Me.txt_address3)
        Me.Panel_back.Controls.Add(Me.txt_address2)
        Me.Panel_back.Controls.Add(Me.txt_Address1)
        Me.Panel_back.Controls.Add(Me.Label17)
        Me.Panel_back.Controls.Add(Me.txt_Name)
        Me.Panel_back.Controls.Add(Me.Label3)
        Me.Panel_back.Location = New System.Drawing.Point(6, 45)
        Me.Panel_back.Name = "Panel_back"
        Me.Panel_back.Size = New System.Drawing.Size(644, 419)
        Me.Panel_back.TabIndex = 39
        '
        'cbo_area
        '
        Me.cbo_area.FormattingEnabled = True
        Me.cbo_area.Location = New System.Drawing.Point(125, 92)
        Me.cbo_area.MaxLength = 35
        Me.cbo_area.Name = "cbo_area"
        Me.cbo_area.Size = New System.Drawing.Size(500, 23)
        Me.cbo_area.TabIndex = 2
        Me.cbo_area.Text = "cbo_Area"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(12, 96)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(69, 15)
        Me.Label30.TabIndex = 62
        Me.Label30.Text = "Area Name"
        '
        'txt_Alaisname
        '
        Me.txt_Alaisname.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Alaisname.Location = New System.Drawing.Point(125, 64)
        Me.txt_Alaisname.MaxLength = 35
        Me.txt_Alaisname.Name = "txt_Alaisname"
        Me.txt_Alaisname.Size = New System.Drawing.Size(500, 23)
        Me.txt_Alaisname.TabIndex = 1
        Me.txt_Alaisname.Text = "TXT_ALAISNAME"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(12, 68)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(68, 15)
        Me.Label29.TabIndex = 61
        Me.Label29.Text = "Alais Name"
        '
        'lbl_idno
        '
        Me.lbl_idno.BackColor = System.Drawing.Color.White
        Me.lbl_idno.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_idno.Location = New System.Drawing.Point(125, 8)
        Me.lbl_idno.Name = "lbl_idno"
        Me.lbl_idno.Size = New System.Drawing.Size(500, 23)
        Me.lbl_idno.TabIndex = 60
        Me.lbl_idno.Text = "Lbl_idno"
        Me.lbl_idno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(12, 12)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(36, 15)
        Me.Label27.TabIndex = 59
        Me.Label27.Text = "Id No"
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.Gray
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(542, 361)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(83, 35)
        Me.btn_close.TabIndex = 17
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.Gray
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(435, 361)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(83, 35)
        Me.btn_Save.TabIndex = 16
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'txt_pan
        '
        Me.txt_pan.Location = New System.Drawing.Point(125, 316)
        Me.txt_pan.MaxLength = 35
        Me.txt_pan.Name = "txt_pan"
        Me.txt_pan.Size = New System.Drawing.Size(500, 23)
        Me.txt_pan.TabIndex = 10
        Me.txt_pan.Text = "txt_Pan"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(12, 320)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(46, 15)
        Me.Label21.TabIndex = 50
        Me.Label21.Text = "Pan No"
        '
        'cbo_group
        '
        Me.cbo_group.DropDownHeight = 75
        Me.cbo_group.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_group.FormattingEnabled = True
        Me.cbo_group.IntegralHeight = False
        Me.cbo_group.Location = New System.Drawing.Point(125, 120)
        Me.cbo_group.Name = "cbo_group"
        Me.cbo_group.Size = New System.Drawing.Size(500, 23)
        Me.cbo_group.TabIndex = 3
        Me.cbo_group.Text = "cbo_group"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.ForeColor = System.Drawing.Color.Black
        Me.Label20.Location = New System.Drawing.Point(12, 124)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(65, 15)
        Me.Label20.TabIndex = 49
        Me.Label20.Text = "A/C Group"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_emailid
        '
        Me.txt_emailid.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_emailid.Location = New System.Drawing.Point(125, 288)
        Me.txt_emailid.MaxLength = 35
        Me.txt_emailid.Name = "txt_emailid"
        Me.txt_emailid.Size = New System.Drawing.Size(500, 23)
        Me.txt_emailid.TabIndex = 9
        Me.txt_emailid.Text = "txt_Emailid"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Black
        Me.Label19.Location = New System.Drawing.Point(12, 292)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(49, 15)
        Me.Label19.TabIndex = 32
        Me.Label19.Text = "Email.Id"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_phoneno
        '
        Me.txt_phoneno.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_phoneno.Location = New System.Drawing.Point(125, 260)
        Me.txt_phoneno.MaxLength = 35
        Me.txt_phoneno.Name = "txt_phoneno"
        Me.txt_phoneno.Size = New System.Drawing.Size(500, 23)
        Me.txt_phoneno.TabIndex = 8
        Me.txt_phoneno.Text = "txt_PhoneNo"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.Color.Black
        Me.Label18.Location = New System.Drawing.Point(12, 264)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(61, 15)
        Me.Label18.TabIndex = 31
        Me.Label18.Text = "Phone No"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_address4
        '
        Me.txt_address4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_address4.Location = New System.Drawing.Point(125, 232)
        Me.txt_address4.MaxLength = 35
        Me.txt_address4.Name = "txt_address4"
        Me.txt_address4.Size = New System.Drawing.Size(500, 23)
        Me.txt_address4.TabIndex = 7
        Me.txt_address4.Text = "txt_Address4"
        '
        'txt_address3
        '
        Me.txt_address3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_address3.Location = New System.Drawing.Point(125, 204)
        Me.txt_address3.MaxLength = 35
        Me.txt_address3.Name = "txt_address3"
        Me.txt_address3.Size = New System.Drawing.Size(500, 23)
        Me.txt_address3.TabIndex = 6
        Me.txt_address3.Text = "txt_Address3"
        '
        'txt_address2
        '
        Me.txt_address2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_address2.Location = New System.Drawing.Point(125, 176)
        Me.txt_address2.MaxLength = 35
        Me.txt_address2.Name = "txt_address2"
        Me.txt_address2.Size = New System.Drawing.Size(500, 23)
        Me.txt_address2.TabIndex = 5
        Me.txt_address2.Text = "txt_Address2"
        '
        'txt_Address1
        '
        Me.txt_Address1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address1.Location = New System.Drawing.Point(125, 148)
        Me.txt_Address1.MaxLength = 35
        Me.txt_Address1.Name = "txt_Address1"
        Me.txt_Address1.Size = New System.Drawing.Size(500, 23)
        Me.txt_Address1.TabIndex = 4
        Me.txt_Address1.Text = "txt_address1"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.Black
        Me.Label17.Location = New System.Drawing.Point(12, 152)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(51, 15)
        Me.Label17.TabIndex = 30
        Me.Label17.Text = "Address"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(125, 36)
        Me.txt_Name.MaxLength = 35
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(500, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(12, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 15)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Agent_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(659, 478)
        Me.Controls.Add(Me.Panel_back)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_open)
        Me.Controls.Add(Me.Label8)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(5, 0)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Agent_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AGENTCREATION"
        Me.grp_open.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_back.ResumeLayout(False)
        Me.Panel_back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents grp_open As System.Windows.Forms.GroupBox
    Friend WithEvents cbo_open As System.Windows.Forms.ComboBox
    Friend WithEvents btn_find_close As System.Windows.Forms.Button
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents btn_OpenFilter As System.Windows.Forms.Button
    Friend WithEvents Panel_back As System.Windows.Forms.Panel
    Friend WithEvents cbo_area As System.Windows.Forms.ComboBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents txt_Alaisname As System.Windows.Forms.TextBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents lbl_idno As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents txt_pan As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cbo_group As System.Windows.Forms.ComboBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txt_emailid As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txt_phoneno As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txt_address4 As System.Windows.Forms.TextBox
    Friend WithEvents txt_address3 As System.Windows.Forms.TextBox
    Friend WithEvents txt_address2 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address1 As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
