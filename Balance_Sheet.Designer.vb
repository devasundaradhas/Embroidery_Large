<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Balance_Sheet
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lbl_Title = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txt_Selection = New System.Windows.Forms.TextBox()
        Me.lbl_MiscExpenses = New System.Windows.Forms.Label()
        Me.lbl_FixedAssets = New System.Windows.Forms.Label()
        Me.lbl_Investments = New System.Windows.Forms.Label()
        Me.lbl_SuspenseAcc = New System.Windows.Forms.Label()
        Me.lbl_CurrentAssets = New System.Windows.Forms.Label()
        Me.lbl_CapitalAcc = New System.Windows.Forms.Label()
        Me.lbl_LoansLiabilities = New System.Windows.Forms.Label()
        Me.lbl_BranchDivisions = New System.Windows.Forms.Label()
        Me.lbl_CurrentLiabilities = New System.Windows.Forms.Label()
        Me.lbl_MiscExpensesName = New System.Windows.Forms.Label()
        Me.lbl_SuspenseAccName = New System.Windows.Forms.Label()
        Me.lbl_CurrentAssetsName = New System.Windows.Forms.Label()
        Me.lbl_InvestmentsName = New System.Windows.Forms.Label()
        Me.lbl_FixedAssetsName = New System.Windows.Forms.Label()
        Me.lbl_BranchDivisionsName = New System.Windows.Forms.Label()
        Me.lbl_CurrentLiabilitiesName = New System.Windows.Forms.Label()
        Me.lbl_LoansLiabilitiesName = New System.Windows.Forms.Label()
        Me.lbl_CapitalAccName = New System.Windows.Forms.Label()
        Me.lbl_Selection = New System.Windows.Forms.Label()
        Me.lbl_Netloss = New System.Windows.Forms.Label()
        Me.lbl_OpeningDiffCR = New System.Windows.Forms.Label()
        Me.lbl_OpeningDiffDB = New System.Windows.Forms.Label()
        Me.lbl_NetProfit = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.lbl_TotalLiabilities = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.lbl_TotalAssets = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.lbl_OpeningDiffNameCR = New System.Windows.Forms.Label()
        Me.lbl_OpeningDiffNameDB = New System.Windows.Forms.Label()
        Me.lbl_NetLossName = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.lbl_NetProfitName = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.pnl_ReportInputs = New System.Windows.Forms.Panel()
        Me.opt_Details = New System.Windows.Forms.RadioButton()
        Me.opt_Simple = New System.Windows.Forms.RadioButton()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.btn_Show = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.cbo_Inputs1 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs1 = New System.Windows.Forms.Label()
        Me.dtp_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtp_FromDate = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.PrintDocument2 = New System.Drawing.Printing.PrintDocument()
        Me.pnl_GridView = New System.Windows.Forms.Panel()
        Me.dgv_PrfitAndLossDetails = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnl_Back.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.pnl_ReportInputs.SuspendLayout()
        Me.pnl_GridView.SuspendLayout()
        CType(Me.dgv_PrfitAndLossDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbl_Title
        '
        Me.lbl_Title.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Title.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Title.ForeColor = System.Drawing.Color.White
        Me.lbl_Title.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Title.Name = "lbl_Title"
        Me.lbl_Title.Size = New System.Drawing.Size(867, 24)
        Me.lbl_Title.TabIndex = 0
        Me.lbl_Title.Text = "BALANCE SHEET"
        Me.lbl_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_Back
        '
        Me.pnl_Back.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_Back.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Panel2)
        Me.pnl_Back.Location = New System.Drawing.Point(-1, 95)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(866, 485)
        Me.pnl_Back.TabIndex = 37
        '
        'Panel2
        '
        Me.Panel2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.txt_Selection)
        Me.Panel2.Controls.Add(Me.lbl_MiscExpenses)
        Me.Panel2.Controls.Add(Me.lbl_FixedAssets)
        Me.Panel2.Controls.Add(Me.lbl_Investments)
        Me.Panel2.Controls.Add(Me.lbl_SuspenseAcc)
        Me.Panel2.Controls.Add(Me.lbl_CurrentAssets)
        Me.Panel2.Controls.Add(Me.lbl_CapitalAcc)
        Me.Panel2.Controls.Add(Me.lbl_LoansLiabilities)
        Me.Panel2.Controls.Add(Me.lbl_BranchDivisions)
        Me.Panel2.Controls.Add(Me.lbl_CurrentLiabilities)
        Me.Panel2.Controls.Add(Me.lbl_MiscExpensesName)
        Me.Panel2.Controls.Add(Me.lbl_SuspenseAccName)
        Me.Panel2.Controls.Add(Me.lbl_CurrentAssetsName)
        Me.Panel2.Controls.Add(Me.lbl_InvestmentsName)
        Me.Panel2.Controls.Add(Me.lbl_FixedAssetsName)
        Me.Panel2.Controls.Add(Me.lbl_BranchDivisionsName)
        Me.Panel2.Controls.Add(Me.lbl_CurrentLiabilitiesName)
        Me.Panel2.Controls.Add(Me.lbl_LoansLiabilitiesName)
        Me.Panel2.Controls.Add(Me.lbl_CapitalAccName)
        Me.Panel2.Controls.Add(Me.lbl_Selection)
        Me.Panel2.Controls.Add(Me.lbl_Netloss)
        Me.Panel2.Controls.Add(Me.lbl_OpeningDiffCR)
        Me.Panel2.Controls.Add(Me.lbl_OpeningDiffDB)
        Me.Panel2.Controls.Add(Me.lbl_NetProfit)
        Me.Panel2.Controls.Add(Me.Panel6)
        Me.Panel2.Controls.Add(Me.lbl_OpeningDiffNameCR)
        Me.Panel2.Controls.Add(Me.lbl_OpeningDiffNameDB)
        Me.Panel2.Controls.Add(Me.lbl_NetLossName)
        Me.Panel2.Controls.Add(Me.Panel5)
        Me.Panel2.Controls.Add(Me.lbl_NetProfitName)
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(863, 482)
        Me.Panel2.TabIndex = 0
        '
        'txt_Selection
        '
        Me.txt_Selection.Location = New System.Drawing.Point(-100, 77)
        Me.txt_Selection.Name = "txt_Selection"
        Me.txt_Selection.Size = New System.Drawing.Size(45, 23)
        Me.txt_Selection.TabIndex = 70
        Me.txt_Selection.Text = "1"
        '
        'lbl_MiscExpenses
        '
        Me.lbl_MiscExpenses.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_MiscExpenses.BackColor = System.Drawing.Color.Transparent
        Me.lbl_MiscExpenses.Location = New System.Drawing.Point(733, 278)
        Me.lbl_MiscExpenses.Name = "lbl_MiscExpenses"
        Me.lbl_MiscExpenses.Size = New System.Drawing.Size(116, 15)
        Me.lbl_MiscExpenses.TabIndex = 77
        Me.lbl_MiscExpenses.Text = "0.00"
        Me.lbl_MiscExpenses.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_FixedAssets
        '
        Me.lbl_FixedAssets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_FixedAssets.BackColor = System.Drawing.Color.Transparent
        Me.lbl_FixedAssets.Location = New System.Drawing.Point(733, 79)
        Me.lbl_FixedAssets.Name = "lbl_FixedAssets"
        Me.lbl_FixedAssets.Size = New System.Drawing.Size(116, 15)
        Me.lbl_FixedAssets.TabIndex = 76
        Me.lbl_FixedAssets.Text = "0.00"
        Me.lbl_FixedAssets.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Investments
        '
        Me.lbl_Investments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_Investments.BackColor = System.Drawing.Color.Transparent
        Me.lbl_Investments.Location = New System.Drawing.Point(733, 130)
        Me.lbl_Investments.Name = "lbl_Investments"
        Me.lbl_Investments.Size = New System.Drawing.Size(116, 13)
        Me.lbl_Investments.TabIndex = 75
        Me.lbl_Investments.Text = "0.00"
        Me.lbl_Investments.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_SuspenseAcc
        '
        Me.lbl_SuspenseAcc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_SuspenseAcc.BackColor = System.Drawing.Color.Transparent
        Me.lbl_SuspenseAcc.Location = New System.Drawing.Point(733, 229)
        Me.lbl_SuspenseAcc.Name = "lbl_SuspenseAcc"
        Me.lbl_SuspenseAcc.Size = New System.Drawing.Size(116, 14)
        Me.lbl_SuspenseAcc.TabIndex = 74
        Me.lbl_SuspenseAcc.Text = "0.00"
        Me.lbl_SuspenseAcc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_CurrentAssets
        '
        Me.lbl_CurrentAssets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_CurrentAssets.BackColor = System.Drawing.Color.Transparent
        Me.lbl_CurrentAssets.Location = New System.Drawing.Point(733, 180)
        Me.lbl_CurrentAssets.Name = "lbl_CurrentAssets"
        Me.lbl_CurrentAssets.Size = New System.Drawing.Size(116, 13)
        Me.lbl_CurrentAssets.TabIndex = 73
        Me.lbl_CurrentAssets.Text = "0.00"
        Me.lbl_CurrentAssets.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_CapitalAcc
        '
        Me.lbl_CapitalAcc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_CapitalAcc.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lbl_CapitalAcc.ForeColor = System.Drawing.Color.Red
        Me.lbl_CapitalAcc.Location = New System.Drawing.Point(303, 78)
        Me.lbl_CapitalAcc.Name = "lbl_CapitalAcc"
        Me.lbl_CapitalAcc.Size = New System.Drawing.Size(117, 17)
        Me.lbl_CapitalAcc.TabIndex = 72
        Me.lbl_CapitalAcc.Text = "0.00"
        Me.lbl_CapitalAcc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_LoansLiabilities
        '
        Me.lbl_LoansLiabilities.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_LoansLiabilities.BackColor = System.Drawing.Color.Transparent
        Me.lbl_LoansLiabilities.Location = New System.Drawing.Point(304, 129)
        Me.lbl_LoansLiabilities.Name = "lbl_LoansLiabilities"
        Me.lbl_LoansLiabilities.Size = New System.Drawing.Size(117, 14)
        Me.lbl_LoansLiabilities.TabIndex = 71
        Me.lbl_LoansLiabilities.Text = "0.00"
        Me.lbl_LoansLiabilities.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_BranchDivisions
        '
        Me.lbl_BranchDivisions.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_BranchDivisions.BackColor = System.Drawing.Color.Transparent
        Me.lbl_BranchDivisions.Location = New System.Drawing.Point(303, 229)
        Me.lbl_BranchDivisions.Name = "lbl_BranchDivisions"
        Me.lbl_BranchDivisions.Size = New System.Drawing.Size(117, 14)
        Me.lbl_BranchDivisions.TabIndex = 70
        Me.lbl_BranchDivisions.Text = "0.00"
        Me.lbl_BranchDivisions.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_CurrentLiabilities
        '
        Me.lbl_CurrentLiabilities.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_CurrentLiabilities.BackColor = System.Drawing.Color.Transparent
        Me.lbl_CurrentLiabilities.Location = New System.Drawing.Point(303, 179)
        Me.lbl_CurrentLiabilities.Name = "lbl_CurrentLiabilities"
        Me.lbl_CurrentLiabilities.Size = New System.Drawing.Size(117, 15)
        Me.lbl_CurrentLiabilities.TabIndex = 69
        Me.lbl_CurrentLiabilities.Text = "0.00"
        Me.lbl_CurrentLiabilities.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_MiscExpensesName
        '
        Me.lbl_MiscExpensesName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_MiscExpensesName.AutoSize = True
        Me.lbl_MiscExpensesName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_MiscExpensesName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_MiscExpensesName.Location = New System.Drawing.Point(455, 277)
        Me.lbl_MiscExpensesName.Name = "lbl_MiscExpensesName"
        Me.lbl_MiscExpensesName.Size = New System.Drawing.Size(102, 18)
        Me.lbl_MiscExpensesName.TabIndex = 68
        Me.lbl_MiscExpensesName.Text = "Misc. Expenses"
        '
        'lbl_SuspenseAccName
        '
        Me.lbl_SuspenseAccName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_SuspenseAccName.AutoSize = True
        Me.lbl_SuspenseAccName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_SuspenseAccName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SuspenseAccName.Location = New System.Drawing.Point(455, 227)
        Me.lbl_SuspenseAccName.Name = "lbl_SuspenseAccName"
        Me.lbl_SuspenseAccName.Size = New System.Drawing.Size(91, 18)
        Me.lbl_SuspenseAccName.TabIndex = 67
        Me.lbl_SuspenseAccName.Text = "Suspense A/c"
        '
        'lbl_CurrentAssetsName
        '
        Me.lbl_CurrentAssetsName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_CurrentAssetsName.AutoSize = True
        Me.lbl_CurrentAssetsName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_CurrentAssetsName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CurrentAssetsName.Location = New System.Drawing.Point(455, 177)
        Me.lbl_CurrentAssetsName.Name = "lbl_CurrentAssetsName"
        Me.lbl_CurrentAssetsName.Size = New System.Drawing.Size(98, 18)
        Me.lbl_CurrentAssetsName.TabIndex = 66
        Me.lbl_CurrentAssetsName.Text = "Current Assets"
        '
        'lbl_InvestmentsName
        '
        Me.lbl_InvestmentsName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_InvestmentsName.AutoSize = True
        Me.lbl_InvestmentsName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_InvestmentsName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_InvestmentsName.Location = New System.Drawing.Point(455, 127)
        Me.lbl_InvestmentsName.Name = "lbl_InvestmentsName"
        Me.lbl_InvestmentsName.Size = New System.Drawing.Size(85, 18)
        Me.lbl_InvestmentsName.TabIndex = 65
        Me.lbl_InvestmentsName.Text = "Investments"
        '
        'lbl_FixedAssetsName
        '
        Me.lbl_FixedAssetsName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_FixedAssetsName.AutoSize = True
        Me.lbl_FixedAssetsName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_FixedAssetsName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FixedAssetsName.Location = New System.Drawing.Point(455, 77)
        Me.lbl_FixedAssetsName.Name = "lbl_FixedAssetsName"
        Me.lbl_FixedAssetsName.Size = New System.Drawing.Size(85, 18)
        Me.lbl_FixedAssetsName.TabIndex = 64
        Me.lbl_FixedAssetsName.Text = "Fixed Assets"
        '
        'lbl_BranchDivisionsName
        '
        Me.lbl_BranchDivisionsName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl_BranchDivisionsName.AutoSize = True
        Me.lbl_BranchDivisionsName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_BranchDivisionsName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_BranchDivisionsName.Location = New System.Drawing.Point(15, 227)
        Me.lbl_BranchDivisionsName.Name = "lbl_BranchDivisionsName"
        Me.lbl_BranchDivisionsName.Size = New System.Drawing.Size(118, 18)
        Me.lbl_BranchDivisionsName.TabIndex = 63
        Me.lbl_BranchDivisionsName.Text = "Branch / Divisions"
        '
        'lbl_CurrentLiabilitiesName
        '
        Me.lbl_CurrentLiabilitiesName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl_CurrentLiabilitiesName.AutoSize = True
        Me.lbl_CurrentLiabilitiesName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_CurrentLiabilitiesName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CurrentLiabilitiesName.Location = New System.Drawing.Point(15, 177)
        Me.lbl_CurrentLiabilitiesName.Name = "lbl_CurrentLiabilitiesName"
        Me.lbl_CurrentLiabilitiesName.Size = New System.Drawing.Size(118, 18)
        Me.lbl_CurrentLiabilitiesName.TabIndex = 62
        Me.lbl_CurrentLiabilitiesName.Text = "Current Liabilities"
        '
        'lbl_LoansLiabilitiesName
        '
        Me.lbl_LoansLiabilitiesName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl_LoansLiabilitiesName.AutoSize = True
        Me.lbl_LoansLiabilitiesName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_LoansLiabilitiesName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_LoansLiabilitiesName.Location = New System.Drawing.Point(15, 127)
        Me.lbl_LoansLiabilitiesName.Name = "lbl_LoansLiabilitiesName"
        Me.lbl_LoansLiabilitiesName.Size = New System.Drawing.Size(95, 18)
        Me.lbl_LoansLiabilitiesName.TabIndex = 61
        Me.lbl_LoansLiabilitiesName.Text = "Loans Liability"
        '
        'lbl_CapitalAccName
        '
        Me.lbl_CapitalAccName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl_CapitalAccName.AutoSize = True
        Me.lbl_CapitalAccName.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lbl_CapitalAccName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CapitalAccName.ForeColor = System.Drawing.Color.Red
        Me.lbl_CapitalAccName.Location = New System.Drawing.Point(15, 77)
        Me.lbl_CapitalAccName.Name = "lbl_CapitalAccName"
        Me.lbl_CapitalAccName.Size = New System.Drawing.Size(75, 18)
        Me.lbl_CapitalAccName.TabIndex = 60
        Me.lbl_CapitalAccName.Text = "Capital A/c"
        '
        'lbl_Selection
        '
        Me.lbl_Selection.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lbl_Selection.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lbl_Selection.Location = New System.Drawing.Point(0, 77)
        Me.lbl_Selection.Name = "lbl_Selection"
        Me.lbl_Selection.Size = New System.Drawing.Size(431, 24)
        Me.lbl_Selection.TabIndex = 59
        '
        'lbl_Netloss
        '
        Me.lbl_Netloss.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_Netloss.BackColor = System.Drawing.Color.Transparent
        Me.lbl_Netloss.Location = New System.Drawing.Point(733, 329)
        Me.lbl_Netloss.Name = "lbl_Netloss"
        Me.lbl_Netloss.Size = New System.Drawing.Size(116, 15)
        Me.lbl_Netloss.TabIndex = 55
        Me.lbl_Netloss.Text = "0.00"
        Me.lbl_Netloss.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_OpeningDiffCR
        '
        Me.lbl_OpeningDiffCR.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_OpeningDiffCR.BackColor = System.Drawing.Color.Transparent
        Me.lbl_OpeningDiffCR.ForeColor = System.Drawing.Color.Red
        Me.lbl_OpeningDiffCR.Location = New System.Drawing.Point(733, 377)
        Me.lbl_OpeningDiffCR.Name = "lbl_OpeningDiffCR"
        Me.lbl_OpeningDiffCR.Size = New System.Drawing.Size(116, 22)
        Me.lbl_OpeningDiffCR.TabIndex = 54
        Me.lbl_OpeningDiffCR.Text = "0.00"
        Me.lbl_OpeningDiffCR.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_OpeningDiffDB
        '
        Me.lbl_OpeningDiffDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_OpeningDiffDB.BackColor = System.Drawing.Color.Transparent
        Me.lbl_OpeningDiffDB.ForeColor = System.Drawing.Color.Red
        Me.lbl_OpeningDiffDB.Location = New System.Drawing.Point(307, 377)
        Me.lbl_OpeningDiffDB.Name = "lbl_OpeningDiffDB"
        Me.lbl_OpeningDiffDB.Size = New System.Drawing.Size(117, 22)
        Me.lbl_OpeningDiffDB.TabIndex = 48
        Me.lbl_OpeningDiffDB.Text = "0.00"
        Me.lbl_OpeningDiffDB.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_NetProfit
        '
        Me.lbl_NetProfit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_NetProfit.BackColor = System.Drawing.Color.Transparent
        Me.lbl_NetProfit.Location = New System.Drawing.Point(302, 279)
        Me.lbl_NetProfit.Name = "lbl_NetProfit"
        Me.lbl_NetProfit.Size = New System.Drawing.Size(117, 14)
        Me.lbl_NetProfit.TabIndex = 47
        Me.lbl_NetProfit.Text = "0.00"
        Me.lbl_NetProfit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel6
        '
        Me.Panel6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.lbl_TotalLiabilities)
        Me.Panel6.Controls.Add(Me.Label34)
        Me.Panel6.Controls.Add(Me.lbl_TotalAssets)
        Me.Panel6.Controls.Add(Me.Label33)
        Me.Panel6.Location = New System.Drawing.Point(0, 452)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(862, 27)
        Me.Panel6.TabIndex = 29
        '
        'lbl_TotalLiabilities
        '
        Me.lbl_TotalLiabilities.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_TotalLiabilities.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_TotalLiabilities.ForeColor = System.Drawing.Color.White
        Me.lbl_TotalLiabilities.Location = New System.Drawing.Point(295, 3)
        Me.lbl_TotalLiabilities.Name = "lbl_TotalLiabilities"
        Me.lbl_TotalLiabilities.Size = New System.Drawing.Size(130, 18)
        Me.lbl_TotalLiabilities.TabIndex = 3
        Me.lbl_TotalLiabilities.Text = "0.00"
        Me.lbl_TotalLiabilities.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label34
        '
        Me.Label34.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.Color.White
        Me.Label34.Location = New System.Drawing.Point(455, 3)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(91, 18)
        Me.Label34.TabIndex = 2
        Me.Label34.Text = "Total Amount"
        '
        'lbl_TotalAssets
        '
        Me.lbl_TotalAssets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_TotalAssets.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_TotalAssets.ForeColor = System.Drawing.Color.White
        Me.lbl_TotalAssets.Location = New System.Drawing.Point(732, 3)
        Me.lbl_TotalAssets.Name = "lbl_TotalAssets"
        Me.lbl_TotalAssets.Size = New System.Drawing.Size(120, 18)
        Me.lbl_TotalAssets.TabIndex = 1
        Me.lbl_TotalAssets.Text = "0.00"
        Me.lbl_TotalAssets.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label33
        '
        Me.Label33.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.Color.White
        Me.Label33.Location = New System.Drawing.Point(14, 0)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(91, 18)
        Me.Label33.TabIndex = 0
        Me.Label33.Text = "Total Amount"
        '
        'lbl_OpeningDiffNameCR
        '
        Me.lbl_OpeningDiffNameCR.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_OpeningDiffNameCR.AutoSize = True
        Me.lbl_OpeningDiffNameCR.BackColor = System.Drawing.Color.Transparent
        Me.lbl_OpeningDiffNameCR.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_OpeningDiffNameCR.ForeColor = System.Drawing.Color.Red
        Me.lbl_OpeningDiffNameCR.Location = New System.Drawing.Point(455, 377)
        Me.lbl_OpeningDiffNameCR.Name = "lbl_OpeningDiffNameCR"
        Me.lbl_OpeningDiffNameCR.Size = New System.Drawing.Size(99, 18)
        Me.lbl_OpeningDiffNameCR.TabIndex = 15
        Me.lbl_OpeningDiffNameCR.Text = "[Opening Diff.]"
        '
        'lbl_OpeningDiffNameDB
        '
        Me.lbl_OpeningDiffNameDB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl_OpeningDiffNameDB.AutoSize = True
        Me.lbl_OpeningDiffNameDB.BackColor = System.Drawing.Color.Transparent
        Me.lbl_OpeningDiffNameDB.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_OpeningDiffNameDB.ForeColor = System.Drawing.Color.Red
        Me.lbl_OpeningDiffNameDB.Location = New System.Drawing.Point(15, 377)
        Me.lbl_OpeningDiffNameDB.Name = "lbl_OpeningDiffNameDB"
        Me.lbl_OpeningDiffNameDB.Size = New System.Drawing.Size(99, 18)
        Me.lbl_OpeningDiffNameDB.TabIndex = 14
        Me.lbl_OpeningDiffNameDB.Text = "[Opening Diff.]"
        '
        'lbl_NetLossName
        '
        Me.lbl_NetLossName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_NetLossName.AutoSize = True
        Me.lbl_NetLossName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_NetLossName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_NetLossName.Location = New System.Drawing.Point(455, 327)
        Me.lbl_NetLossName.Name = "lbl_NetLossName"
        Me.lbl_NetLossName.Size = New System.Drawing.Size(60, 18)
        Me.lbl_NetLossName.TabIndex = 13
        Me.lbl_NetLossName.Text = "Net Loss"
        '
        'Panel5
        '
        Me.Panel5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Location = New System.Drawing.Point(431, 5)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1, 474)
        Me.Panel5.TabIndex = 6
        '
        'lbl_NetProfitName
        '
        Me.lbl_NetProfitName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl_NetProfitName.AutoSize = True
        Me.lbl_NetProfitName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_NetProfitName.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_NetProfitName.Location = New System.Drawing.Point(15, 277)
        Me.lbl_NetProfitName.Name = "lbl_NetProfitName"
        Me.lbl_NetProfitName.Size = New System.Drawing.Size(68, 18)
        Me.lbl_NetProfitName.TabIndex = 5
        Me.lbl_NetProfitName.Text = "Net Profit"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(861, 27)
        Me.Panel4.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(637, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 19)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Assets"
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(192, 2)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 19)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Liabilities"
        '
        'pnl_ReportInputs
        '
        Me.pnl_ReportInputs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_ReportInputs.BackColor = System.Drawing.Color.Cyan
        Me.pnl_ReportInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_ReportInputs.Controls.Add(Me.opt_Details)
        Me.pnl_ReportInputs.Controls.Add(Me.opt_Simple)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Print)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Show)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Close)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs1)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs1)
        Me.pnl_ReportInputs.Controls.Add(Me.dtp_ToDate)
        Me.pnl_ReportInputs.Controls.Add(Me.Label2)
        Me.pnl_ReportInputs.Controls.Add(Me.dtp_FromDate)
        Me.pnl_ReportInputs.Controls.Add(Me.Label3)
        Me.pnl_ReportInputs.Location = New System.Drawing.Point(0, 24)
        Me.pnl_ReportInputs.Name = "pnl_ReportInputs"
        Me.pnl_ReportInputs.Size = New System.Drawing.Size(868, 73)
        Me.pnl_ReportInputs.TabIndex = 39
        '
        'opt_Details
        '
        Me.opt_Details.AutoSize = True
        Me.opt_Details.Location = New System.Drawing.Point(559, 44)
        Me.opt_Details.Name = "opt_Details"
        Me.opt_Details.Size = New System.Drawing.Size(57, 19)
        Me.opt_Details.TabIndex = 50
        Me.opt_Details.Text = "Detail"
        Me.opt_Details.UseVisualStyleBackColor = True
        '
        'opt_Simple
        '
        Me.opt_Simple.AutoSize = True
        Me.opt_Simple.Location = New System.Drawing.Point(559, 3)
        Me.opt_Simple.Name = "opt_Simple"
        Me.opt_Simple.Size = New System.Drawing.Size(62, 19)
        Me.opt_Simple.TabIndex = 49
        Me.opt_Simple.Text = "Simple"
        Me.opt_Simple.UseVisualStyleBackColor = True
        '
        'btn_Print
        '
        Me.btn_Print.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Print.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Print.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(642, 22)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(69, 30)
        Me.btn_Print.TabIndex = 48
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&Print"
        Me.btn_Print.UseVisualStyleBackColor = False
        '
        'btn_Show
        '
        Me.btn_Show.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn_Show.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Show.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Show.ForeColor = System.Drawing.Color.White
        Me.btn_Show.Location = New System.Drawing.Point(484, 20)
        Me.btn_Show.Name = "btn_Show"
        Me.btn_Show.Size = New System.Drawing.Size(69, 30)
        Me.btn_Show.TabIndex = 46
        Me.btn_Show.TabStop = False
        Me.btn_Show.Text = "&Show"
        Me.btn_Show.UseVisualStyleBackColor = False
        '
        'btn_Close
        '
        Me.btn_Close.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(736, 22)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(69, 30)
        Me.btn_Close.TabIndex = 47
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&Close"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'cbo_Inputs1
        '
        Me.cbo_Inputs1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbo_Inputs1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs1.FormattingEnabled = True
        Me.cbo_Inputs1.Location = New System.Drawing.Point(363, 24)
        Me.cbo_Inputs1.MaxDropDownItems = 15
        Me.cbo_Inputs1.Name = "cbo_Inputs1"
        Me.cbo_Inputs1.Size = New System.Drawing.Size(115, 23)
        Me.cbo_Inputs1.Sorted = True
        Me.cbo_Inputs1.TabIndex = 44
        '
        'lbl_Inputs1
        '
        Me.lbl_Inputs1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl_Inputs1.AutoSize = True
        Me.lbl_Inputs1.BackColor = System.Drawing.Color.Cyan
        Me.lbl_Inputs1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs1.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs1.Location = New System.Drawing.Point(299, 28)
        Me.lbl_Inputs1.Name = "lbl_Inputs1"
        Me.lbl_Inputs1.Size = New System.Drawing.Size(58, 15)
        Me.lbl_Inputs1.TabIndex = 45
        Me.lbl_Inputs1.Text = "Company"
        '
        'dtp_ToDate
        '
        Me.dtp_ToDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dtp_ToDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_ToDate.Location = New System.Drawing.Point(207, 24)
        Me.dtp_ToDate.Name = "dtp_ToDate"
        Me.dtp_ToDate.Size = New System.Drawing.Size(86, 23)
        Me.dtp_ToDate.TabIndex = 41
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Cyan
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(175, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 15)
        Me.Label2.TabIndex = 43
        Me.Label2.Text = "To :"
        '
        'dtp_FromDate
        '
        Me.dtp_FromDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dtp_FromDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_FromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_FromDate.Location = New System.Drawing.Point(82, 24)
        Me.dtp_FromDate.Name = "dtp_FromDate"
        Me.dtp_FromDate.Size = New System.Drawing.Size(87, 23)
        Me.dtp_FromDate.TabIndex = 40
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Cyan
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(7, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 15)
        Me.Label3.TabIndex = 42
        Me.Label3.Text = "Date From:"
        '
        'Button3
        '
        Me.Button3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button3.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.ForeColor = System.Drawing.Color.White
        Me.Button3.Location = New System.Drawing.Point(709, 20)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(69, 30)
        Me.Button3.TabIndex = 49
        Me.Button3.TabStop = False
        Me.Button3.UseVisualStyleBackColor = False
        '
        'PrintDocument1
        '
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Lime
        Me.lbl_Company.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Company.Location = New System.Drawing.Point(153, -350)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 63
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'PrintDocument2
        '
        '
        'pnl_GridView
        '
        Me.pnl_GridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_GridView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_GridView.Controls.Add(Me.dgv_PrfitAndLossDetails)
        Me.pnl_GridView.Location = New System.Drawing.Point(1500, 48)
        Me.pnl_GridView.Name = "pnl_GridView"
        Me.pnl_GridView.Size = New System.Drawing.Size(865, 482)
        Me.pnl_GridView.TabIndex = 64
        Me.pnl_GridView.Visible = False
        '
        'dgv_PrfitAndLossDetails
        '
        Me.dgv_PrfitAndLossDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgv_PrfitAndLossDetails.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Gray
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_PrfitAndLossDetails.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_PrfitAndLossDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_PrfitAndLossDetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4})
        Me.dgv_PrfitAndLossDetails.EnableHeadersVisualStyles = False
        Me.dgv_PrfitAndLossDetails.Location = New System.Drawing.Point(1, -2)
        Me.dgv_PrfitAndLossDetails.Name = "dgv_PrfitAndLossDetails"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_PrfitAndLossDetails.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgv_PrfitAndLossDetails.RowHeadersVisible = False
        Me.dgv_PrfitAndLossDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_PrfitAndLossDetails.Size = New System.Drawing.Size(861, 510)
        Me.dgv_PrfitAndLossDetails.TabIndex = 3
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "DESCRIPTION"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 550
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "LIABILITIES"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 140
        '
        'Column3
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column3.HeaderText = "ASSETS"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 140
        '
        'Column4
        '
        Me.Column4.HeaderText = ""
        Me.Column4.Name = "Column4"
        '
        'Balance_Sheet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(867, 579)
        Me.Controls.Add(Me.pnl_GridView)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.pnl_ReportInputs)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.lbl_Title)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "Balance_Sheet"
        Me.Text = "Balance_Sheet"
        Me.pnl_Back.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.pnl_ReportInputs.ResumeLayout(False)
        Me.pnl_ReportInputs.PerformLayout()
        Me.pnl_GridView.ResumeLayout(False)
        CType(Me.dgv_PrfitAndLossDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl_Title As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents pnl_ReportInputs As System.Windows.Forms.Panel
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents btn_Show As System.Windows.Forms.Button
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents cbo_Inputs1 As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Inputs1 As System.Windows.Forms.Label
    Friend WithEvents dtp_ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtp_FromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lbl_NetProfitName As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents lbl_NetLossName As System.Windows.Forms.Label
    Friend WithEvents lbl_OpeningDiffNameCR As System.Windows.Forms.Label
    Friend WithEvents lbl_OpeningDiffNameDB As System.Windows.Forms.Label
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents lbl_TotalLiabilities As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents lbl_TotalAssets As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents lbl_OpeningDiffDB As System.Windows.Forms.Label
    Friend WithEvents lbl_NetProfit As System.Windows.Forms.Label
    Friend WithEvents lbl_Netloss As System.Windows.Forms.Label
    Friend WithEvents lbl_OpeningDiffCR As System.Windows.Forms.Label
    Friend WithEvents txt_Selection As System.Windows.Forms.TextBox
    Friend WithEvents lbl_MiscExpenses As System.Windows.Forms.Label
    Friend WithEvents lbl_FixedAssets As System.Windows.Forms.Label
    Friend WithEvents lbl_Investments As System.Windows.Forms.Label
    Friend WithEvents lbl_SuspenseAcc As System.Windows.Forms.Label
    Friend WithEvents lbl_CurrentAssets As System.Windows.Forms.Label
    Friend WithEvents lbl_CapitalAcc As System.Windows.Forms.Label
    Friend WithEvents lbl_LoansLiabilities As System.Windows.Forms.Label
    Friend WithEvents lbl_BranchDivisions As System.Windows.Forms.Label
    Friend WithEvents lbl_CurrentLiabilities As System.Windows.Forms.Label
    Friend WithEvents lbl_MiscExpensesName As System.Windows.Forms.Label
    Friend WithEvents lbl_SuspenseAccName As System.Windows.Forms.Label
    Friend WithEvents lbl_CurrentAssetsName As System.Windows.Forms.Label
    Friend WithEvents lbl_InvestmentsName As System.Windows.Forms.Label
    Friend WithEvents lbl_FixedAssetsName As System.Windows.Forms.Label
    Friend WithEvents lbl_BranchDivisionsName As System.Windows.Forms.Label
    Friend WithEvents lbl_CurrentLiabilitiesName As System.Windows.Forms.Label
    Friend WithEvents lbl_LoansLiabilitiesName As System.Windows.Forms.Label
    Friend WithEvents lbl_CapitalAccName As System.Windows.Forms.Label
    Friend WithEvents lbl_Selection As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents opt_Simple As System.Windows.Forms.RadioButton
    Friend WithEvents opt_Details As System.Windows.Forms.RadioButton
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents PrintDocument2 As System.Drawing.Printing.PrintDocument
    Friend WithEvents pnl_GridView As System.Windows.Forms.Panel
    Friend WithEvents dgv_PrfitAndLossDetails As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
