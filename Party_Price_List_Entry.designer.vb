<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Party_Price_List_Entry
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Party_Price_List_Entry))
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.cbo_Size = New System.Windows.Forms.ComboBox()
        Me.cbo_PartyName = New System.Windows.Forms.ComboBox()
        Me.cbo_ItemName = New System.Windows.Forms.ComboBox()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.dgv_PriceListdetails = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnl_back.SuspendLayout()
        CType(Me.dgv_PriceListdetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Open.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.cbo_Size)
        Me.pnl_back.Controls.Add(Me.cbo_PartyName)
        Me.pnl_back.Controls.Add(Me.cbo_ItemName)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_back.Controls.Add(Me.dgv_PriceListdetails)
        Me.pnl_back.Location = New System.Drawing.Point(11, 47)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(457, 332)
        Me.pnl_back.TabIndex = 35
        '
        'cbo_Size
        '
        Me.cbo_Size.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Size.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Size.FormattingEnabled = True
        Me.cbo_Size.IntegralHeight = False
        Me.cbo_Size.Location = New System.Drawing.Point(263, 154)
        Me.cbo_Size.Name = "cbo_Size"
        Me.cbo_Size.Size = New System.Drawing.Size(60, 23)
        Me.cbo_Size.Sorted = True
        Me.cbo_Size.TabIndex = 22
        Me.cbo_Size.Text = "cbo_Size"
        '
        'cbo_PartyName
        '
        Me.cbo_PartyName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_PartyName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_PartyName.FormattingEnabled = True
        Me.cbo_PartyName.Location = New System.Drawing.Point(118, 63)
        Me.cbo_PartyName.Name = "cbo_PartyName"
        Me.cbo_PartyName.Size = New System.Drawing.Size(324, 23)
        Me.cbo_PartyName.Sorted = True
        Me.cbo_PartyName.TabIndex = 1
        Me.cbo_PartyName.Text = "cbo_PartyName"
        '
        'cbo_ItemName
        '
        Me.cbo_ItemName.FormattingEnabled = True
        Me.cbo_ItemName.Location = New System.Drawing.Point(62, 150)
        Me.cbo_ItemName.Name = "cbo_ItemName"
        Me.cbo_ItemName.Size = New System.Drawing.Size(175, 23)
        Me.cbo_ItemName.TabIndex = 11
        Me.cbo_ItemName.Text = "cbo_Itemname"
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.Gray
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(350, 292)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(92, 35)
        Me.btn_close.TabIndex = 10
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.Gray
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(252, 292)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(92, 35)
        Me.btn_save.TabIndex = 9
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(12, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "IdNo"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(12, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 15)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Party Name"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(119, 17)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(323, 23)
        Me.lbl_IdNo.TabIndex = 5
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dgv_PriceListdetails
        '
        Me.dgv_PriceListdetails.AllowUserToResizeColumns = False
        Me.dgv_PriceListdetails.AllowUserToResizeRows = False
        Me.dgv_PriceListdetails.BackgroundColor = System.Drawing.Color.White
        Me.dgv_PriceListdetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_PriceListdetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column4, Me.Column3})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_PriceListdetails.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgv_PriceListdetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_PriceListdetails.Location = New System.Drawing.Point(12, 100)
        Me.dgv_PriceListdetails.Name = "dgv_PriceListdetails"
        Me.dgv_PriceListdetails.RowHeadersVisible = False
        Me.dgv_PriceListdetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_PriceListdetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_PriceListdetails.Size = New System.Drawing.Size(432, 186)
        Me.dgv_PriceListdetails.TabIndex = 3
        Me.dgv_PriceListdetails.TabStop = False
        '
        'Column1
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle1
        Me.Column1.HeaderText = "S.NO"
        Me.Column1.MaxInputLength = 35
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 50
        '
        'Column2
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column2.HeaderText = "ITEM NAME"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 200
        '
        'Column4
        '
        Me.Column4.HeaderText = "SIZE"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 60
        '
        'Column3
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column3.HeaderText = "RATE"
        Me.Column3.MaxInputLength = 35
        Me.Column3.Name = "Column3"
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(180, 184)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(92, 35)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'cbo_Open
        '
        Me.cbo_Open.DropDownHeight = 125
        Me.cbo_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(22, 37)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(363, 23)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(4, 428)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(412, 269)
        Me.grp_Open.TabIndex = 37
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "Finding"
        Me.grp_Open.Visible = False
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseOpen.Image = CType(resources.GetObject("btn_CloseOpen.Image"), System.Drawing.Image)
        Me.btn_CloseOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseOpen.Location = New System.Drawing.Point(293, 184)
        Me.btn_CloseOpen.Name = "btn_CloseOpen"
        Me.btn_CloseOpen.Size = New System.Drawing.Size(92, 35)
        Me.btn_CloseOpen.TabIndex = 30
        Me.btn_CloseOpen.Text = "&Close"
        Me.btn_CloseOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseOpen.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label6.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(0, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(482, 35)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "PARTY PRICE LIST"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Party_Price_List_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(482, 391)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.Label6)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Party_Price_List_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PART PRICE LIST ENTRY"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        CType(Me.dgv_PriceListdetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Open.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents cbo_ItemName As System.Windows.Forms.ComboBox
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents dgv_PriceListdetails As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbo_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Size As System.Windows.Forms.ComboBox
End Class
