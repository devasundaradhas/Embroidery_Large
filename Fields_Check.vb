Imports System.IO

Public Class FieldsCheck

    Public Shared vFldsChk_All_Status As Boolean = False
    Public Shared vFldsChk_From_CompGroupCreation_Status As Boolean = False

    Public Shared Sub FieldsCheck_All(ByVal cn1 As SqlClient.SqlConnection, ByVal FrmNm As Form)

        On Error Resume Next

        FrmNm.Cursor = Cursors.WaitCursor
        If Trim(LCase(FrmNm.Name)) = "mdiparent1" Then
            MDIParent1.Cursor = Cursors.WaitCursor
        End If

        vFldsChk_All_Status = True

        '======= Put New Script Here ==========

        FieldsCheck_1(cn1, FrmNm)

        Common_Procedures.Default_Unit_Creation(cn1)

        '=============================

        Field_Check_PayRoll(cn1, FrmNm)

        '=============================

        If vFldsChk_From_CompGroupCreation_Status = True Then

            Common_Procedures.Default_GroupHead_Updation(cn1)

            Common_Procedures.Default_LedgerHead_Updation(cn1)

            Common_Procedures.Default_MonthHead_Updation(cn1)

            Common_Procedures.Default_Shift_Updation(cn1)

            Common_Procedures.Default_StateHead_Updation(cn1)

            Common_Procedures.Default_Master_Updation(cn1)


        End If

        vFldsChk_All_Status = False

        FrmNm.Cursor = Cursors.Default
        If Trim(LCase(FrmNm.Name)) = "mdiparent1" Then
            MDIParent1.Cursor = Cursors.Default
        End If


        If vFldsChk_From_CompGroupCreation_Status = False Then
            MessageBox.Show("Fields Verified", "FOR FIELDS CREATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        End If

    End Sub

    Public Shared Sub FieldsCheck_1(ByVal cn1 As SqlClient.SqlConnection, ByVal FrmNm As Form)

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Nr As Integer = 0

        On Error Resume Next

        FrmNm.Cursor = Cursors.WaitCursor
        If Trim(LCase(FrmNm.Name)) = "mdiparent1" Then
            MDIParent1.Cursor = Cursors.WaitCursor
        End If

        cmd.Connection = cn1

        'GoTo A

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()

A:
        cmd.CommandText = "Alter table Simple_Receipt_Details Add Unit_IdNo smallint"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head Add Unit_IdNo smallint"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Company_Head Add GSTP_CA_Mail_Id varchar(300)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Ledger_Legal_Name varchar(300)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Update Ledger_Head Set Ledger_Legal_Name = '' where Ledger_Legal_Name is null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Voucher_Head add Salesman_IdNo int Default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "update Voucher_Head set Salesman_IdNo = 0 Where Sales_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head Add Price_List_IdNo_Cash int"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE LEDGER_HEAD ADD DISTANCE NUMERIC(18,3) DEFAULT (0)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE LEDGER_HEAD ADD GSTIN_VERIFIED BIT DEFAULT(0)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE ItemGroup_Head Add IsService Bit"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE Item_Head Add IsService Bit"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details Add Component_IdNo smallint"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Invoice_DC_Details Add Component_IdNo smallint"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Invoice_DC_Details Add Colour_IdNo smallint"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "  CREATE TABLE " & Common_Procedures.CompanyDetailsDataBaseName & ".[dbo].[AWS_KEY_SETTINGS](	[AWS_ACCESS_KEY] [varchar](1000) NULL,	[AWS_SECRET_KEY] [varchar](1000) NULL,	[AWS_BUCKET_FOR_DB] [varchar](100) NULL," &
                          "[AWS_BUCKET_FOR_SW] [varchar](100) NULL,	[AWS_BUCKET_FOR_DOWNLOADER] [varchar](100) NULL,	[AWS_FOLDER_FOR_SW_PROGRAMS] [varchar](250) NULL," &
                          "[AWS_FOLDER_FOR_SW_REPORTS] [varchar](250) NULL) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "DELETE FROM " & Common_Procedures.CompanyDetailsDataBaseName & ".[dbo].AWS_KEY_SETTINGS"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "INSERT " & Common_Procedures.CompanyDetailsDataBaseName & ".[dbo].[AWS_KEY_SETTINGS] ([AWS_ACCESS_KEY]       , [AWS_SECRET_KEY]                           , [AWS_BUCKET_FOR_DB] , [AWS_BUCKET_FOR_SW]    , [AWS_BUCKET_FOR_DOWNLOADER]   , [AWS_FOLDER_FOR_SW_PROGRAMS]         , [AWS_FOLDER_FOR_SW_REPORTS]) VALUES " &
                                                                                     "(N'AKIAQDQ3Z6TGQHMBRQHE', N'fPlLN+G33py9c59k35eQRYBtZ4cTdDX4w8oFHw9n', N'ndbfiles'         , N'nsoftwarefordownload', N'novasoftwaredownloader'     , N'embroidery_large/programfiles'     , N'embroidery_large/reports')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE OrderJobNo_Head ADD CONSTRAINT uq_OrderJobNo_Head UNIQUE(OrderNo_Name , OrderJobNo_Name)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head Add Style_No varchar(250)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Loan_EMI_Settings](	[Employee_IdNo] [int] NOT NULL,	[Current_EMI] [numeric](9, 3) NULL, CONSTRAINT [PK_Loan_EMI_Settings] PRIMARY KEY CLUSTERED " &
                          "([Employee_IdNo] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Production_Details Add Job_No Varchar(100) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head Add Contact_Person_Phone varchar(15) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Invoice_Head Add Bill_No Varchar(50) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Production_Head Add Incharge_IdNo int default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Production_Cost](	[UID] [varchar](50) NOT NULL,	[Production_Cost] [numeric](18, 3) NULL,	[Remarks] [varchar](150) NULL," &
                          " CONSTRAINT [PK_Production_Cost] PRIMARY KEY CLUSTERED (	[UID] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head Add Rejection_Allowance tinyint default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head Add User_Name VARCHAR(50) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head Add User_Name VARCHAR(50) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Receipt_Details Add Receipt_Type Varchar(30) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head Add Prepared_By Varchar(50) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head Add Style_Ref_No Varchar(100) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head Add Emb_Part Varchar (100) default '',Emb_Position Varchar(100) default '' , Emb_Type Varchar(100) default '' , " &
                          " Foam_Removal_rate numeric(18,3) default 0,Material_rate numeric(18,3) default 0,Sizes Varchar(100) default 0, Thread_Colour_Count tinyint default 0," &
                          " No_Of_Appliques tinyint default 0,No_Of_Sequins tinyint default 0,Is_Material_Provided bit default 0, Material_Provided Varchar (250) default ''," &
                          " Confirmed_By varchar(150) default '', Contact_Person varchar(150) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "alter table Invoice_DC_Details Add UID Varchar(200) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE Order_Program_Head Add Billing_Name_IdNo Int Default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE TSoft_Billing_CompanyGroup_Details..User_Head Add User_Real_Name Varchar(50)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_DETAILS ADD DCCODES VARCHAR(500)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_HEAD ADD Party_Ref_No Varchar (150)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter Table Sales_Details Add Job_No Varchar(100)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Combo_Pop_Temp]([LOV] [varchar](500) NULL) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE INVOICE_DC_DETAILS ADD Job_No varchar(100), DC_Date smalldatetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_DELIVERY_HEAD ADD Total_Bundles TinyInt"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_DELIVERY_DETAILS ADD Job_NO VARCHAR(100),Bundles TINYINT, Delivery_Purpose Varchar(50),Component_IdNo Int,Party_DC_No varchar(50)"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [dbo].[OrderJobNo_Head](	[OrderJobNo_IdNo] [int] Not NULL,	[OrderNo_Name] [varchar](50) Not NULL,	[OrderJobNo_Name] [varchar](50) Not NULL," &
                          "[Sur_Name] [varchar](50) Not NULL, CONSTRAINT [PK_OrderJobNo_Head] PRIMARY KEY CLUSTERED ([OrderJobNo_IdNo] ASC), CONSTRAINT [IX_OrderJobNo_Head] UNIQUE NONCLUSTERED " &
                          "( [Sur_Name] ASC)) On [PRIMARY]"
        cmd.ExecuteNonQuery()





        cmd.CommandText = "ALTER TABLE SALES_DELIVERY_HEAD ADD Total_Bundles TINYINT"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_DELIVERY_DETAILS ADD Total_Bundles TINYINT"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SIMPLE_RECEIPT_DETAILS ADD Job_NO VARCHAR(100),Bundles TINYINT, Receipt_Purpose Varchar(50),Component_IdNo Int"
        cmd.ExecuteNonQuery()

        cmd.CommandText = " CREATE TABLE [dbo].[Component_Head]([Component_IdNo] [int] Not NULL,  	[Component_Name] [varchar](50) Not NULL,	[Sur_Name] [varchar](50) Not NULL," & _
                          " CONSTRAINT [PK_Component_Head] PRIMARY KEY CLUSTERED ([Component_IdNo] Asc ))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_QUOTATION_HEAD ADD UID VARCHAR(500)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_QUOTATION_HEAD ADD UNIQUE (UID)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head Add DC_Cutoff_Date smalldatetime "
        cmd.ExecuteNonQuery()

        cmd.CommandText = " CREATE TABLE [dbo].[Invoice_DC_Details]([Sales_Code] [varchar](50) Not NULL,[Sales_DC_Code] [varchar](50) Not NULL)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Head Add Salary_Year Varchar(4) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head Add Non_Billable_Reason varchar(250) Default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Simple_Receipt_Head Add Return_Reason varchar(250) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Delivery_Head Add Return_Reason varchar(250) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Receipt_Head Add Non_Billable_Reason varchar(250) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head Add IsBillable bit default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Simple_Receipt_Head Add IsReturn bit default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Delivery_Head Add IsReturn bit default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Receipt_Head Add IsBillable bit default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[OrderNo_Head]([OrderNo_IdNo] [int] NOT NULL,[OrderNo_Name] [varchar](50) NOT NULL,[Sur_Name] [varchar](50) NOT NULL,CONSTRAINT [PK_OrderNo_Head] PRIMARY KEY CLUSTERED " & _
                          "([OrderNo_IdNo] Asc ) ON [PRIMARY], CONSTRAINT [IX_OrderNo_Head] UNIQUE NONCLUSTERED ([Sur_Name]Asc) ON [PRIMARY]) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head Add Finalised_Rate numeric(18,3) default 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head Add Remarks Varchar(500) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = File.OpenText(My.Application.Info.DirectoryPath & "\CREATE GSTR DB.SQL").ReadToEnd()
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[ReportTemp] ADD [Name16] [Varchar](1000) NULL, [Name17] [Varchar](1000) NULL, [Name18] [Varchar](1000) NULL, [Name19] [Varchar](1000) NULL, [Name20] [Varchar](1000) NULL," & _
                          "  [Name21] [Varchar](1000) NULL, [Name22] [Varchar](1000) NULL, [Name23] [Varchar](1000) NULL, [Name24] [Varchar](1000) NULL, [Name25] [Varchar](1000) NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[ReportTemp] ADD [Meters13] [numeric](18, 2) NULL, [Meters14] [numeric](18, 2) NULL, [Meters15] [numeric](18, 2) NULL, [Meters16] [numeric](18, 2) NULL, [Meters17] [numeric](18, 2) NULL," & _
                          "  [Meters18] [numeric](18, 2) NULL, [Meter19] [numeric](18, 2) NULL, [Meters20] [numeric](18, 2) NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Payroll_Bonus_Head]([Bonus_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Bonus_No] [int] NOT NULL," & _
                       "[for_OrderBy] [numeric](18, 2) NOT NULL,	[Bonus_Date] [datetime] NOT NULL,	[From_Date] [varchar](50) NULL,	[To_Date] [varchar](50) NULL,	[Max_Shifts] [numeric](18, 3) NULL," & _
                       "[Min_Shifts] [numeric](18, 3) NULL,	[Min_Att_Reqd] [numeric](18, 3) NULL,	[Exclude_WO] [bit] NULL,	[Exclude_PH_LH] [bit] NULL,	[Bonus_Rate] [numeric](18, 3) NULL," & _
                       "[M1] [varchar](30) NULL,	[M2] [varchar](30) NULL,	[M3] [varchar](30) NULL,	[M4] [varchar](30) NULL,	[M5] [varchar](30) NULL,	[M6] [varchar](30) NULL, " & _
                       "[M7] [varchar](30) NULL,	[M8] [varchar](30) NULL,	[M9] [varchar](30) NULL,	[M10] [varchar](30) NULL,	[M11] [varchar](30) NULL,	[M12] [varchar](30) NULL, " & _
                       "[M13] [varchar](30) NULL,	[M14] [varchar](30) NULL,	[Salary_Payment_Type_IdNo] [int] NULL,	[Category_IdNo] [int] NULL," & _
                       "CONSTRAINT [PK_Payroll_Bonus_Head] PRIMARY KEY CLUSTERED ( [Bonus_Code]Asc)) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = " CREATE TABLE [dbo].[Payroll_Bonus_Details]([Bonus_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Bonus_No] [varchar](50) NOT NULL ,	[Sl_No] [varchar](50) NOT NULL," & _
                        "[Employee_IdNo] [int] NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,	[M1] [numeric](18, 3) NULL,	[M2] [numeric](18, 3) NULL,	[M3] [numeric](18, 3) NULL," & _
                        "[M4] [numeric](18, 3) NULL,	[M5] [numeric](18, 3) NULL,	[M6] [numeric](18, 3) NULL,	[M7] [numeric](18, 3) NULL,	[M8] [numeric](18, 3) NULL,	[M9] [numeric](18, 3) NULL," & _
                        "[M10] [numeric](18, 3) NULL,[M11] [numeric](18, 3) NULL,	[M12] [numeric](18, 3) NULL,	[M13] [numeric](18, 3) NULL,	[M14] [numeric](18, 3) NULL,	[Tot_Shifts] [numeric](18, 3) NULL, " & _
                        "[Tot_Att] [numeric](18, 3) NULL,	[Wage_Per_Day] [numeric](18, 3) NULL,	[Total_Earnings] [numeric](18, 3) NULL,	[Bonus_Earned] [numeric](18, 3) NULL,	[Bonus_Finalised] [numeric](18, 3) NULL," & _
                        "CONSTRAINT [PK_Payroll_Bonus_Details] PRIMARY KEY CLUSTERED ([Bonus_Code] ASC,[Employee_IdNo] ASC)) ON [PRIMARY]"

        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter Table Other_GST_Entry_Head Add Payment_Method Varchar(50)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Salary_Details]ADD 	[PF_Credit_Amount] [numeric](18, 2) NULL,	[E_P_S_AUDIT] [numeric](18, 2) NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Employee_Head] ADD [Esi_For_OTSalary_Status] [tinyint] NULL,	[PF_Credit_Status] [tinyint] NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[PayRoll_Employee_Deduction_Details](	[Employee_Deduction_Code] [varchar](30) NOT NULL," & _
                          "[Company_IdNo] [smallint] NOT NULL,[Employee_Deduction_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Employee_Deduction_Date] [smalldatetime] NOT NULL," & _
                          "[Employee_IdNo] [int] NULL,[Sl_No] [int] NOT NULL,[Advance_Deduction_Amount] [numeric](18, 3) NULL,[Mess] [numeric](18, 3) NULL,[Medical] [numeric](18, 3) NULL," & _
                          "[Store] [numeric](18, 3) NULL,[Other_Addition] [numeric](18, 3) NULL,[Quality_Fine] [numeric](18, 2) NULL,[Other_Deduction_Amount] [numeric](18, 2) NULL," & _
                          "CONSTRAINT [PK_PayRoll_Employee_Deduction_Details] PRIMARY KEY CLUSTERED ([Employee_Deduction_Code] ASC,[Sl_No] ASC)) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Details] ADD  CONSTRAINT [DF_PayRoll_Employee_Deduction_Details_Mess]  DEFAULT ((0)) FOR [Mess]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Details] ADD  CONSTRAINT [DF_PayRoll_Employee_Deduction_Details_Medical]  DEFAULT ((0)) FOR [Medical]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Details] ADD  CONSTRAINT [DF_PayRoll_Employee_Deduction_Details_Store]  DEFAULT ((0)) FOR [Store]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Details] ADD  CONSTRAINT [DF_PayRoll_Employee_Deduction_Details_Other_Addition]  DEFAULT ((0)) FOR [Other_Addition]"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "CREATE TABLE [dbo].[Payroll_Employee_OverTime_Details](	[Timing_OverTime_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NULL,	[Timing_OverTime_No] [varchar](30) NOT NULL," & _
                         "[for_OrderBy] [numeric](18, 2) NOT NULL,[Timing_OverTime_Date] [smalldatetime] NOT NULL,[Sl_No] [smallint] NOT NULL,[Employee_IdNo] [smallint] NULL," & _
                         "[OT_Minutes] [numeric](18, 2) NULL,[OT_Hours] [numeric](18, 2) NULL,CONSTRAINT [PK_Payroll_Employee_OverTime_Details] PRIMARY KEY CLUSTERED " & _
                         "(	[Timing_OverTime_Code] ASC,[Sl_No] ASC))"

        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [dbo].[Payroll_Employee_OverTime_Details] ADD  CONSTRAINT [DF_Payroll_Employee_OverTime_Details_Employee_IdNo]  DEFAULT ((0)) FOR [Employee_IdNo]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [dbo].[Payroll_Employee_OverTime_Details] ADD  CONSTRAINT [DF_Table_1_OT_Minutes1]  DEFAULT ((0)) FOR [OT_Minutes]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[Payroll_Employee_OverTime_Details] ADD  CONSTRAINT [DF_Table_1_OT_Hours1]  DEFAULT ((0)) FOR [OT_Hours]"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "CREATE TABLE [dbo].[ESI_PF_Head]( [ESI_PF_Group_IdNo] [smallint] NOT NULL,[ESI_PF_Group_Name] [varchar](100) NOT NULL,[ESI_PF_SurName] [varchar](50) NOT NULL," & _
                       "[ESI_AUDIT_STATUS] [int] NULL,[PF_AUDIT_STATUS] [int] NULL,[ESI_SALARY_STATUS] [int] NULL,[PF_SALARY_STATUS] [int] NULL," & _
                        "CONSTRAINT [PK_ESI_PF_Head] PRIMARY KEY ([ESI_PF_Group_IdNo]))"

        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [dbo].[ESI_PF_Head] ADD  DEFAULT ((0)) FOR [ESI_AUDIT_STATUS]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[ESI_PF_Head] ADD  DEFAULT ((0)) FOR [PF_AUDIT_STATUS]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[ESI_PF_Head] ADD  DEFAULT ((0)) FOR [ESI_SALARY_STATUS]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[ESI_PF_Head] ADD  DEFAULT ((0)) FOR [PF_SALARY_STATUS]"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "CREATE TABLE [dbo].[Payroll_Employee_OverTime_Head](	[Timing_OverTime_Code] [varchar](30) NOT NULL,	[Company_IdNo] [smallint] NOT NULL," & _
                          "[Timing_OverTime_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Timing_OverTime_Date] [smalldatetime] NOT NULL," & _
                          "[Day_Name] [varchar](50) NULL,CONSTRAINT [PK_Payroll_Employee_OverTime_Head] PRIMARY KEY CLUSTERED ([Timing_OverTime_Code]))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[Payroll_Employee_OverTime_Head] ADD  CONSTRAINT [DF_Payroll_Employee_OverTime_Head_Day_Name]  DEFAULT ('') FOR [Day_Name]"
        cmd.ExecuteNonQuery()




        cmd.CommandText = "CREATE TABLE [dbo].[Ledger_ItemName_Details](	[Ledger_Idno] [int] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Item_Idno] [int] NOT NULL," & _
                       "[Party_ItemName] [varchar](50) NOT NULL,CONSTRAINT [PK_Ledger_ItemName_Details] PRIMARY KEY NONCLUSTERED " & _
                        "([Ledger_Idno] ASC,[Sl_No] ASC) ," & _
                        "CONSTRAINT [IX_Ledger_ItemName_Details] UNIQUE NONCLUSTERED ([Ledger_Idno] ASC,[Item_Idno] ASC)) "
        cmd.ExecuteNonQuery()



        cmd.CommandText = "CREATE TABLE [dbo].[ReportTemp_Simple](	[Name1] [varchar](100) NULL,	[Name2] [varchar](100) NULL,	[Name3] [varchar](100) NULL,	[Name4] [varchar](100) NULL," & _
                        "[Name5] [varchar](100) NULL,	[Name6] [varchar](100) NULL,	[name7] [varchar](100) NULL,	[Name8] [varchar](100) NULL,	[Name9] [varchar](100) NULL," & _
                        "[Name10] [varchar](100) NULL,	[Name11] [varchar](100) NULL,	[Name12] [varchar](100) NULL,	[Date1] [smalldatetime] NULL,	[Date2] [smalldatetime] NULL,	[Date3] [smalldatetime] NULL,	[Date4] [smalldatetime] NULL," & _
                        "[Int1] [int] NULL,	[Int2] [int] NULL,	[Int3] [int] NULL,	[Int4] [int] NULL,	[Int5] [int] NULL,	[Int6] [int] NULL," & _
                        "[Int7] [int] NULL,	[Int8] [int] NULL,	[Int9] [int] NULL,	[Int10] [int] NULL,	[Meters1] [numeric](18, 2) NULL,	[Meters2] [numeric](18, 2) NULL," & _
                        "[Meters3] [numeric](18, 2) NULL,	[Meters4] [numeric](18, 2) NULL,	[Meters5] [numeric](18, 2) NULL,	[Meters6] [numeric](18, 2) NULL,	[Meters7] [numeric](18, 2) NULL," & _
                        "[Meters8] [numeric](18, 2) NULL,	[Meters9] [numeric](18, 2) NULL,	[Meters10] [numeric](18, 2) NULL,	[Meters11] [numeric](18, 2) NULL,	" & _
                        "[Weight1] [numeric](18, 3) NULL,	[Weight2] [numeric](18, 3) NULL,	[Weight3] [numeric](18, 3) NULL,	[Weight4] [numeric](18, 3) NULL,	[Weight5] [numeric](18, 3) NULL," & _
                        "[Weight6] [numeric](18, 3) NULL,	[Weight7] [numeric](18, 3) NULL,	[Weight8] [numeric](18, 3) NULL,	[Weight9] [numeric](18, 3) NULL,	[Weight10] [numeric](18, 3) NULL, 	[Weight11] [numeric](18, 3) NULL," & _
                        "[Currency1] [numeric](18, 2) NULL,	[Currency2] [numeric](18, 2) NULL,	[Currency3] [numeric](18, 2) NULL,	[Currency4] [numeric](18, 2) NULL,	[Currency5] [numeric](18, 2) NULL," & _
                        "[Currency6] [numeric](18, 2) NULL,	[Currency7] [numeric](18, 2) NULL,	[Currency8] [numeric](18, 2) NULL,	[Currency9] [numeric](18, 7) NULL,	[Currency10] [numeric](18, 7) NULL," & _
                        "[Currency11] [numeric](18, 7) NULL,	[Currency12] [numeric](18, 7) NULL)"

        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter TABLE [dbo].[Company_Head] add [SMS_Provider_SenderID] [varchar](50) NULL,	[SMS_Provider_Key] [varchar](50) NULL,	[SMS_Provider_RouteID] [varchar](50) NULL," & _
                       "[SMS_Provider_Type] [varchar](50) NULL,[Area_IdNo] [int] NULL"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter Table Department_Head Add Department_Code Varchar(50)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter Table Employee_Head Add Card_No Varchar(50)"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter Table Payroll_AttendanceLog_FromMachine_Details Add AttendanceLog_IP_Address Varchar(50)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter Table Payroll_AttendanceLog_FromMachine_Head Add AttendanceLog_IP_Address Varchar(50)"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Category_Head] add [Min_Minutes_One_Shift_1] [smallint] NULL,[Min_Minutes_Half_Shift_1] [smallint] NULL," & _
                       "[Min_Minutes_One_Shift_2] [smallint] NULL,[Min_Minutes_Half_Shift_2] [smallint] NULL,[Min_Minutes_One_Shift_3] [smallint] NULL," & _
                       "[Min_Minutes_Half_Shift_3] [smallint] NULL,[Shift_Rotation_Status] [int] NULL"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Employee_Head] ADD [Employee_MainName] [varchar](200) NULL,	[Bank_IdNo] [int] NULL,	[ESI_PF_Group_IdNo] [int] NULL," & _
                          "[Mother_Tongue] [varchar](50) NULL,[bank_code] [varchar](50) NULL,	[PAN_No] [varchar](20) NULL,	[UAN_No] [varchar](20) NULL"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Employee_Salary_Details] ADD [Other_Deduction2] [numeric](18, 2) NULL,[Other_Deduction1] [numeric](18, 2) NULL"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Salary_Details] ADD [Card_No] [varchar](50) NULL,	[Opening_Advance] [numeric](18, 2) NULL,	[Signature_Status] [int] NULL," & _
                            "[ESI_AUDIT] [numeric](18, 3) NULL,	[PF_AUDIT] [numeric](18, 3) NULL,	[E_P_F_AUDIT] [numeric](18, 3) NULL,	[OT_HOURS_HALF] [numeric](18, 3) NULL,	[OT_ESI] [numeric](18, 2) NULL," & _
                            "[SALARY_OT_ESI] [numeric](18, 2) NULL"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [dbo].[PayRoll_Salary_Head] ADD [Category_IdNo] [int] NULL,[ESI_PF_Group_IdNo] [int] NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[ReportTemp] ADD	[AutoSlNo] [int] IDENTITY(1,1) NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[ReportTempSub] ADD [AutoSlNo] [int] IDENTITY(1,1) NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [dbo].[Ledger_AlaisHead] ADD [Area_IdNo] [int] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = " ALTER TABLE [dbo].[Settings_Head] ADD [S_Name] [varchar](50) NULL,[EmpDate_indx] [int] NULL,	[EmpCode_indx] [int] NULL,	[EmpInOutMode_indx] [int] NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Voucher_Head Alter Column Voucher_Code Varchar(50) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Voucher_Bill_Head Alter Column Voucher_Bill_Code Varchar(50) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Voucher_Details Alter Column Voucher_Code Varchar(50) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Voucher_Bill_Details Alter Column Voucher_Bill_Code Varchar(50) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Receipt_Head add Party_DC_No Varchar(50) default '' "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Close_Order Bit "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Invoice_Head add Party_DC_No Varchar(50) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Production_Details add Ledger_IdNo Int "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add OrderCode_forSelection Varchar(200) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Other_GST_Entry_Head add Unregister_Type Varchar(100) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Update Other_GST_Entry_Head set Unregister_Type = '' Where Unregister_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Other_GST_Entry_Head add Reason_For_Issuing_Note Varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Other_GST_Entry_Head set Reason_For_Issuing_Note = '' Where Reason_For_Issuing_Note is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Other_GST_Entry_Head add Tds_Percentage numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Other_GST_Entry_Head set Tds_Percentage = 0 Where Tds_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Other_GST_Entry_Head add Tds_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Other_GST_Entry_Head set Tds_Amount = 0 Where Tds_Amount is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Other_GST_Entry_Head add Bill_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Other_GST_Entry_Head set Bill_Amount = 0 Where Bill_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Other_GST_Entry_Head] ( [Other_GST_Entry_Reference_Code] [varchar](50) NOT NULL, 	[Other_GST_Entry_Reference_No] [varchar](50) NOT NULL, 	[ForOrderBy_ReferenceCode] [numeric](18, 2) NOT NULL, 	[Other_GST_Entry_Type] [varchar](50) NOT NULL, 	[Company_IdNo] [smallint] NOT NULL, 	[Other_GST_Entry_PrefixNo] [varchar](50) NOT NULL, 	[Other_GST_Entry_No] [varchar](50) NOT NULL, 	[Other_GST_Entry_RefNo] [varchar](50) NOT NULL, 	[for_OrderBy] [numeric](18, 2) NOT NULL, 	[Other_GST_Entry_Date] [smalldatetime] NOT NULL, 	[Ledger_IdNo] [int] NULL DEFAULT (0) , 	[Bill_No] [varchar](100) NULL DEFAULT ('') , 	[Bill_Date] [smalldatetime] NULL, 	[Other_GST_Entry_Ac_IdNo] [int] NULL DEFAULT (0) , " & _
                         "	[Gross_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[CashDiscount_Perc] [numeric](18, 2) NULL DEFAULT (0) , 	[CashDiscount_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Taxable_Value] [numeric](18, 2) NULL DEFAULT (0) , 	[CGST_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[SGST_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[IGST_AMount] [numeric](18, 2) NULL DEFAULT (0) , 	[Chess_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Round_Off_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Net_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[TaxAmount_RoundOff_Status] [tinyint] NULL DEFAULT (0) , 	[Total_Quantity] [numeric](18, 2) NULL DEFAULT (0) , 	[Total_Sub_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Total_DiscountAmount] [numeric](18, 2) NULL DEFAULT (0) , 	[Total_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Total_Footer_Cash_Discount_Amount] [numeric](18, 2) NULL DEFAULT (0) , " & _
                         "   [Total_Taxable_Value] [numeric](18, 2) NULL DEFAULT (0) , 	[Remarks] [varchar](1000) NULL DEFAULT ('') , 	[User_Idno] [smallint] NULL DEFAULT (0) ,   CONSTRAINT [PK_Other_GST_Entry_Head] PRIMARY KEY CLUSTERED ( [Other_GST_Entry_Reference_Code] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Other_GST_Entry_Details] ( [Other_GST_Entry_Reference_Code] [varchar](50) NOT NULL, 	[Other_GST_Entry_Reference_No] [varchar](50) NOT NULL, 	[ForOrderBy_ReferenceCode] [numeric](18, 2) NOT NULL, 	[Other_GST_Entry_Type] [varchar](50) NOT NULL, 	[Company_IdNo] [smallint] NOT NULL, 	[Other_GST_Entry_PrefixNo] [varchar](50) NOT NULL, 	[Other_GST_Entry_No] [varchar](50) NOT NULL, 	[Other_GST_Entry_RefNo] [varchar](50) NOT NULL, 	[for_OrderBy] [numeric](18, 2) NOT NULL, 	[Other_GST_Entry_Date] [smalldatetime] NOT NULL, 	[Ledger_IdNo] [int] NULL DEFAULT (0) , " & _
                            "	[Sl_No] [int] NOT NULL,  	[Item_Particulars] [varchar](200) NULL DEFAULT ('') , 	[Unit_IdNo] [smallint] NULL DEFAULT (0) , 	[Hsn_Sac_Code] [varchar](50) NULL DEFAULT ('') , 	[Gst_Perc] [numeric](18, 3) NULL DEFAULT (0) , 	[Quantity] [numeric](18, 3) NULL DEFAULT (0) , 	[Rate] [numeric](18, 2) NULL DEFAULT (0) ,	[Amount] [numeric](18, 2) NULL DEFAULT (0) ,	[Discount_Perc] [numeric](18, 2) NULL DEFAULT (0) , 	[Discount_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Total_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Footer_Cash_Discount_Perc] [numeric](18, 2) NULL DEFAULT (0) , 	[Footer_Cash_Discount_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Taxable_Value] [numeric](18, 2) NULL DEFAULT (0) ,  CONSTRAINT [PK_Other_GST_Entry_Details] PRIMARY KEY CLUSTERED  ( [Other_GST_Entry_Reference_Code] , 	[Sl_No] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Other_GST_Entry_Tax_Details] ( [Other_GST_Entry_Reference_Code] [varchar](50) NOT NULL, 	[Other_GST_Entry_Reference_No] [varchar](50) NOT NULL, 	[ForOrderBy_ReferenceCode] [numeric](18, 2) NOT NULL, 	[Other_GST_Entry_Type] [varchar](50) NOT NULL, 	[Company_IdNo] [smallint] NOT NULL, 	[Other_GST_Entry_PrefixNo] [varchar](50) NOT NULL, 	[Other_GST_Entry_No] [varchar](50) NOT NULL, 	[Other_GST_Entry_RefNo] [varchar](50) NOT NULL, 	[for_OrderBy] [numeric](18, 2) NOT NULL, 	[Other_GST_Entry_Date] [smalldatetime] NOT NULL, 	[Ledger_IdNo] [int] NULL DEFAULT (0) , " & _
                            "	[Sl_No] [int] NOT NULL, 	[HSN_SAC_Code] [varchar](100) NULL DEFAULT ('') ,	[GST_Percentage] [numeric](18, 2) NULL DEFAULT (0) , 	[Taxable_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[CGST_Percentage] [numeric](18, 2) NULL DEFAULT (0) , 	[CGST_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[SGST_Percentage] [numeric](18, 2) NULL DEFAULT (0) , 	[SGST_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[IGST_Percentage] [numeric](18, 2) NULL DEFAULT (0) , 	[IGST_Amount] [numeric](18, 2) NULL DEFAULT (0) , 	[Chess_Perc] [numeric](18, 2) NULL DEFAULT (0) , 	[Chess_Amount] [numeric](18, 2) NULL DEFAULT (0) ,   CONSTRAINT [PK_Other_GST_Entry_GST_Tax_Details_1] PRIMARY KEY CLUSTERED  ( [Other_GST_Entry_Reference_Code],  [Sl_No] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_DELIVERY_DETAILS ADD GRN_No VARCHAR(50),ISBILLED BIT"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE ITEM_HEAD ADD ISDEFAULT_ITEM_FOR_AUTO_BILL BIT"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE SALES_HEAD ADD ISDIRECT BIT"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE COMPANY_HEAD ADD Company_ESINo Varchar(50)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE Purchase_DETAILS ADD Description Varchar(100)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE PRICE_LIST_DETAILS ADD Minimum_Stitches SmallInt"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE Sales_Delivery_Head ADD Manual_DC_No Varchar(20)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE PRICE_LIST_DETAILS ADD Rate_Per_1000_Stitches NUMERIC(18,3),Minimum_Amount NUMERIC(18,3)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME1 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME2 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME3 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME4 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME5 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME6 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME7 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME8 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME9 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME10 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME11 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME12 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME13 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME14 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMP ALTER COLUMN NAME15 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME1 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME2 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME3 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME4 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME5 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME6 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME7 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME8 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME9 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME10 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME11 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME12 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME13 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME14 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE REPORTTEMPSUB ALTER COLUMN NAME15 VARCHAR(1000)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = " CREATE TABLE [Embroidery_Jobwork_Invoice_Details]([Embroidery_Jobwork_Invoice_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Embroidery_Jobwork_Invoice_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Embroidery_Jobwork_Invoice_Date] [smalldatetime] NOT NULL,[Ledger_IdNo] [int] NOT NULL,[SL_No] [smallint] NOT NULL,[Item_IdNo] [int] NULL,[ItemGroup_IdNo] [smallint] NULL,[Unit_IdNo] [smallint] NULL,[Noof_Items] [numeric](18, 3) NULL,[Bags] [int] NULL,[Rate] [numeric](18, 2) NULL,[Tax_Rate] [numeric](18, 2) NULL,[Amount] [numeric](18, 2) NULL,[Discount_Perc] [numeric](18, 2) NULL,[Discount_Amount] [numeric](18, 2) NULL,[Tax_Perc] [numeric](18, 2) NULL,[Tax_Amount] [numeric](18, 2) NULL,[Total_Amount] [numeric](18, 2) NULL,[Size_IdNo] [int] NULL,[Meters] [numeric](18, 2) NULL,[Colour_IdNo] [int] NULL,[Item_code] [varchar](100) NULL,[Entry_Type] [varchar](50) NULL,[Order_Code] [varchar](50) NULL,[Order_Detail_SlNo] [int] NULL,[Noof_Items_Return] [numeric](18, 2) NULL,[Embroidery_Jobwork_Invoice_Details_SlNo] [int] IDENTITY(1,1) NOT NULL,[Design_Picture] [image] NULL,[Rate_For] [varchar](50) NULL,[Order_No] [varchar](100) NULL,[Order_Date] [varchar](100) NULL,[Quantity] [numeric](18, 2) NULL,[Rate_1000Stitches] [numeric](18, 2) NULL,[Design_No] [varchar](50) NULL,[Details_Design] [varchar](500) NULL,[Return_Qty] [numeric](18, 2) NULL,[Style_Idno] [int] NULL,[Style_Name] [varchar](50) NULL,[Trade_Discount_Amount_For_All_Item] [numeric](18, 2) NULL,[Trade_Discount_Perc_For_All_Item] [numeric](18, 2) NULL,[Assessable_Value] [numeric](18, 2) NULL,[HSN_Code] [varchar](50) NULL,[GST_Percentage] [numeric](18, 2) NULL,[Actual_Amount] [numeric](18, 2) NULL,[Actual_Rate] [numeric](18, 2) NULL,[Advance_Amount] [numeric](18, 2) NULL,[Balance_Amount] [numeric](18, 2) NULL,[Dc_No] [varchar](50) NULL,[Sales_Price] [numeric](18, 2) NULL,[Discount_Amount_item] [numeric](18, 2) NULL,[Rate_Tax] [numeric](18, 2) NULL,[Discount_Perc_Item] [numeric](18, 2) NULL,[Cash_Discount_Perc_For_All_Item] [numeric](18, 2) NULL,[Cash_Discount_Amount_For_All_Item] [numeric](18, 2) NULL,[RateWithTax] [numeric](18, 2) NULL,[Item_Description] [varchar](500) NULL,[Embroidery_Jobwork_Receipt_Code] [varchar](50) NULL,[Embroidery_Jobwork_Receipt_Detail_SlNo] [int] NULL,[Area_IdNo] [int] NULL,[Agent_IdNo] [int] NULL,[Cgst_Percentage] [numeric](18, 2) NULL,[Cgst_Amount] [numeric](18, 2) NULL,[Sgst_Percentage] [numeric](18, 2) NULL,[Sgst_Amount] [numeric](18, 2) NULL,[Igst_Percentage] [numeric](18, 2) NULL,[Igst_Amount] [numeric](18, 2) NULL,[Net_Amount] [numeric](18, 2) NULL,[Total_Rate] [numeric](18, 2) NULL,[Discount_Total] [numeric](18, 2) NULL,[Ordercode_forSelection] [varchar](100) NULL," & _
 "CONSTRAINT [PK_Embroidery_Jobwork_Invoice_Details] PRIMARY KEY CLUSTERED ([Embroidery_Jobwork_Invoice_Code] ASC,[SL_No] ASC) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "CREATE TABLE [Embroidery_Jobwork_Invoice_Head]([Embroidery_Jobwork_Invoice_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Embroidery_Jobwork_Invoice_No] [varchar](50) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Embroidery_Jobwork_Invoice_Date] [datetime] NOT NULL,[Payment_Method] [varchar](20) NULL,[Ledger_IdNo] [int] NULL,[Cash_PartyName] [varchar](50) NULL,[Party_PhoneNo] [varchar](50) NULL,[SalesAc_IdNo] [int] NULL,[Tax_Type] [varchar](20) NULL,[Vehicle_No] [varchar](50) NULL,[Narration] [varchar](500) NULL,[Total_Qty] [numeric](18, 3) NULL,[Total_Bags] [int] NULL,[Total_Weight] [numeric](18, 3) NULL,[SubTotal_Amount] [numeric](18, 2) NULL,[Total_DiscountAmount] [numeric](18, 2) NULL,[Total_TaxAmount] [numeric](18, 2) NULL,[Gross_Amount] [numeric](18, 2) NULL,[CashDiscount_Perc] [numeric](18, 2) NULL,[CashDiscount_Amount] [numeric](18, 2) NULL,[Assessable_Value] [numeric](18, 2) NULL,[Tax_Perc] [numeric](18, 2) NULL,[Tax_Amount] [numeric](18, 2) NULL,[Freight_Amount] [numeric](18, 2) NULL,[AddLess_Amount] [numeric](18, 2) NULL,[Round_Off] [numeric](18, 2) NULL,[Net_Amount] [numeric](18, 2) NULL,[Dc_No] [varchar](35) NULL,[Dc_Date] [varchar](20) NULL,[Booked_By] [varchar](35) NULL,[Transport_IdNo] [int] NULL,[Freight_ToPay_Amount] [numeric](18, 2) NULL,[Ro_Division_Status] [tinyint] NULL,[Order_No] [varchar](100) NULL,[Order_Date] [varchar](50) NULL,[Against_CForm_Status] [tinyint] NULL,[Weight] [numeric](18, 3) NULL,[Entry_Type] [varchar](20) NULL,[Payment_Terms] [varchar](100) NULL,[Total_Rolls] [numeric](18, 2) NULL,[OnAc_IdNo] [int] NULL,[Delivery_Code] [varchar](50) NULL,[Selection_Type] [varchar](50) NULL,[Party_Name] [varchar](50) NULL,[Entry_Status] [varchar](50) NULL,[Party_Dc_No] [varchar](200) NULL,[charge] [numeric](18, 2) NULL,[DeliveryTo_idNo] [int] NULL,[Place_Of_Supply] [varchar](100) NULL,[CGst_Percentage] [numeric](18, 2) NULL,[SGst_Percentage] [numeric](18, 2) NULL,[Entry_VAT_GST_Type] [varchar](100) NULL,[Electronic_Reference_No] [varchar](100) NULL,[Transportation_Mode] [varchar](100) NULL,[Date_Time_Of_Supply] [varchar](100) NULL,[Entry_GST_Tax_Type] [varchar](50) NULL,[CGst_Amount] [numeric](18, 2) NULL,[SGst_Amount] [numeric](18, 2) NULL,[IGst_Amount] [numeric](18, 2) NULL,[Actual_Net_Amount] [numeric](18, 2) NULL,[Actual_Gross_Amount] [numeric](18, 2) NULL,[Actual_Tax_Amount] [numeric](18, 2) NULL,[Freight_Charge] [numeric](18, 2) NULL,[Freight_Charge_Name] [varchar](50) NULL,[Receipt_Amount] [numeric](18, 2) NULL,[Delivery_Date] [varchar](50) NULL,[Received_Date] [varchar](50) NULL,[Sales_Order_Selection_Code] [varchar](50) NULL,[Delivery_Status] [int] NULL,[Advance_Amount] [numeric](18, 2) NULL,[Balance_Amount] [numeric](18, 2) NULL,[Form_H_Status] [numeric](18, 2) NULL,[ItemWise_DiscAmount] [numeric](18, 2) NULL,[Total_DiscountAmount_item] [numeric](18, 2) NULL,[Aessable_Amount] [numeric](18, 2) NULL,[AddLess_Name] [varchar](50) NULL,[Freight_Name] [varchar](50) NULL,[Received_Amount] [numeric](18, 2) NULL,[TaxAc_IdNo] [int] NULL," & _
" CONSTRAINT [PK_Embroidery_Jobwork_Invoice_Head] PRIMARY KEY CLUSTERED ( [Embroidery_Jobwork_Invoice_Code] Asc) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Jobwork_Invoice_GST_Tax_Details](	[Jobwork_Invoice_Code] [varchar](50) NOT NULL,	[Company_Idno] [int] NOT NULL,	[Jobwork_Invoice_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Jobwork_Invoice_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL , [Sl_No] [int] NOT NULL,	[HSN_Code] [varchar](100) NULL,	[Taxable_Amount] [numeric](18, 2) NULL,	[CGST_Percentage] [numeric](18, 2) NULL,	[CGST_Amount] [numeric](18, 2) NULL,	[SGST_Percentage] [numeric](18, 2) NULL,	[SGST_Amount] [numeric](18, 2) NULL,	[IGST_Percentage] [numeric](18, 2) NULL,	[IGST_Amount] [numeric](18, 2) NULL, " & _
                         " CONSTRAINT [PK_Jobwork_Invoice_GST_Tax_Details] PRIMARY KEY CLUSTERED (	[Jobwork_Invoice_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Receipt_Details add Order_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Embroidery_Jobwork_Receipt_Details set Order_Code = '' Where Order_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Embroidery_Jobwork_Receipt_Details add Order_Date varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Embroidery_Jobwork_Receipt_Details set Order_Date = '' Where Order_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Embroidery_Jobwork_Receipt_Details]([Embroidery_Jobwork_Receipt_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Embroidery_Jobwork_Receipt_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Embroidery_Jobwork_Receipt_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL,	[Unit_IdNo] [smallint] NULL,	[Quantity] [numeric](18, 3) NULL,	[Rate] [numeric](18, 2) NULL,	[Amount] [numeric](18, 2) NULL,	[Item_Description] [varchar](500) NULL,	[Embroidery_Jobwork_Receipt_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,	[Invoice_Quantity] [numeric](18, 2) NULL,	[No_Of_Rolls] [numeric](18, 2) NULL,	[Entry_Type] [varchar](30) NULL,	[Order_Detail_SlNo] [int] NULL,	[Noof_Items] [numeric](18, 2) NULL,	[HSN_Code] [varchar](50) NULL,	[Tax_Perc] [numeric](18, 2) NULL,	[Assessable_Value] [numeric](18, 2) NULL,[Order_No] [varchar](100) NULL,[Ordercode_forSelection] [varchar](100) NULL,	[Size_Idno] [int] NULL,[Colour_IdNo] [smallint] NULL,	[Style_Idno] [int] NULL, CONSTRAINT [PK_Embroidery_Jobwork_Receipt_Details] PRIMARY KEY CLUSTERED (	[Embroidery_Jobwork_Receipt_Code] ASC,	[SL_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Embroidery_Jobwork_Receipt_Head](	[Embroidery_Jobwork_Receipt_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Embroidery_Jobwork_Receipt_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Embroidery_Jobwork_Receipt_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NULL,	[Order_No] [varchar](50) NULL,	[Order_Date] [varchar](50) NULL,	[Total_Qty] [numeric](18, 3) NULL,	[Gross_Amount] [numeric](18, 2) NULL,	[Vehicle_No] [varchar](50) NULL,	[Transport_IdNo] [int] NULL,	[Remarks] [varchar](500) NULL,	[Entry_VAT_GST_Type] [varchar](50) NULL,	[Assessable_Value] [numeric](18, 2) NULL,	[CGst_Amount] [numeric](18, 2) NULL,	[SGst_Amount] [numeric](18, 2) NULL,	[IGst_Amount] [numeric](18, 2) NULL,	[SubTotal_Amount] [numeric](18, 2) NULL,	[Net_Amount] [numeric](18, 2) NULL,	[Round_Off] [numeric](18, 2) NULL,	[Entry_GST_Tax_Type] [varchar](20) NULL,	[Total_Bags] [int] NULL,	[Electronic_Reference_No] [varchar](100) NULL,	[Transportation_Mode] [varchar](100) NULL,	[Date_Time_Of_Supply] [varchar](100) NULL,	[Weight] [numeric](18, 2) NULL,	[Freight_ToPay_Amount] [numeric](18, 2) NULL,	[charge] [numeric](18, 2) NULL,	[Lr_Date] [varchar](50) NULL,	[Lr_No] [varchar](50) NULL,	[Booked_By] [varchar](50) NULL,	[Entry_Type] [varchar](50) NULL, CONSTRAINT [PK_Embroidery_Jobwork_Receipt_Head] PRIMARY KEY CLUSTERED (	[Embroidery_Jobwork_Receipt_Code] ASC,	[Embroidery_Jobwork_Receipt_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Embroidery_Jobwork_Delivery_Details]([Embroidery_Jobwork_Delivery_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Embroidery_Jobwork_Delivery_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Embroidery_Jobwork_Delivery_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL,	[Unit_IdNo] [smallint] NULL,	[Quantity] [numeric](18, 3) NULL,	[Rate] [numeric](18, 2) NULL,	[Amount] [numeric](18, 2) NULL,	[Item_Description] [varchar](500) NULL,	[Embroidery_Jobwork_Delivery_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,	[Receipt_Quantity] [numeric](18, 2) NULL,	[No_Of_Rolls] [numeric](18, 2) NULL,	[Entry_Type] [varchar](30) NULL,	[Order_Detail_SlNo] [int] NULL,	[Noof_Items] [numeric](18, 2) NULL,	[HSN_Code] [varchar](50) NULL,	[Tax_Perc] [numeric](18, 2) NULL,	[Assessable_Value] [numeric](18, 2) NULL,[Order_No] [varchar](100) NULL,[Ordercode_forSelection] [varchar](100) NULL,	[Size_Idno] [int] NULL,[Colour_IdNo] [smallint] NULL,	[Style_Idno] [int] NULL, CONSTRAINT [PK_Embroidery_Jobwork_Delivery_Details] PRIMARY KEY CLUSTERED (	[Embroidery_Jobwork_Delivery_Code] ASC,	[SL_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Embroidery_Jobwork_Delivery_Head](	[Embroidery_Jobwork_Delivery_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Embroidery_Jobwork_Delivery_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Embroidery_Jobwork_Delivery_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NULL,	[Order_No] [varchar](50) NULL,	[Order_Date] [varchar](50) NULL,	[Total_Qty] [numeric](18, 3) NULL,	[Gross_Amount] [numeric](18, 2) NULL,	[Vehicle_No] [varchar](50) NULL,	[Transport_IdNo] [int] NULL,	[Remarks] [varchar](500) NULL,	[Entry_VAT_GST_Type] [varchar](50) NULL,	[Assessable_Value] [numeric](18, 2) NULL,	[CGst_Amount] [numeric](18, 2) NULL,	[SGst_Amount] [numeric](18, 2) NULL,	[IGst_Amount] [numeric](18, 2) NULL,	[SubTotal_Amount] [numeric](18, 2) NULL,	[Net_Amount] [numeric](18, 2) NULL,	[Round_Off] [numeric](18, 2) NULL,	[Entry_GST_Tax_Type] [varchar](20) NULL,	[Total_Bags] [int] NULL,	[Electronic_Reference_No] [varchar](100) NULL,	[Transportation_Mode] [varchar](100) NULL,	[Date_Time_Of_Supply] [varchar](100) NULL,	[Weight] [numeric](18, 2) NULL,	[Freight_ToPay_Amount] [numeric](18, 2) NULL,	[charge] [numeric](18, 2) NULL,	[Lr_Date] [varchar](50) NULL,	[Lr_No] [varchar](50) NULL,	[Booked_By] [varchar](50) NULL,	[Entry_Type] [varchar](50) NULL, CONSTRAINT [PK_Embroidery_Jobwork_Delivery_Head] PRIMARY KEY CLUSTERED (	[Embroidery_Jobwork_Delivery_Code] ASC,	[Embroidery_Jobwork_Delivery_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Ordercode_forSelection varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Ordercode_forSelection = '' Where Ordercode_forSelection is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Embroidery_Expense_Head]([Expense_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Expense_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Expense_Date] [smalldatetime] NOT NULL," & _
 "CONSTRAINT [PK_Embroidery_Expense_Head] PRIMARY KEY CLUSTERED ([Expense_Code] Asc) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Embroidery_Expense_Details]([Expense_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Expense_No] [varchar](50) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Expense_Date] [smalldatetime] NOT NULL,[Sl_No] [int] NOT NULL,[Expense_IdNo] [int] NULL,[First_Shift] [numeric](18, 2) NULL,[Second_Shift] [numeric](18, 2) NULL,[Third_Shift] [numeric](18, 2) NULL,[Total_Shift] [numeric](18, 2) NULL,[Rate] [numeric](18, 2) NULL,[Amount] [numeric](18, 2) NULL,[Cost_Type] [varchar](50) NULL," & _
 "CONSTRAINT [PK_Embroidery_Expense_Details] PRIMARY KEY CLUSTERED ([Expense_Code] ASC,[Sl_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Expense_Head](	[Expense_IdNo] [int] NOT NULL,	[Expense_Name] [varchar](200) NULL,	[Sur_Name] [varchar](200) NULL," & _
                        " CONSTRAINT [PK_Expense_Head] PRIMARY KEY CLUSTERED (        [Expense_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Machine_Head add Machine_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Machine_Head set Machine_No = '' Where Machine_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Machine_Head add Machine_Make varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Machine_Head set Machine_Make = '' Where Machine_Make is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Machine_Head add Noof_Heads int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Machine_Head set Noof_Heads = 0 Where Noof_Heads is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Order_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Order_Code = '' Where Order_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Colour_IdNo = 0 Where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Production_Details add Size_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Production_Details set Size_IdNo = 0 Where Size_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Production_Details add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Production_Details set Colour_IdNo = 0 Where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Simple_Receipt_Details add Size_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Simple_Receipt_Details set Size_IdNo = 0 Where Size_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Simple_Receipt_Details add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Simple_Receipt_Details set Colour_IdNo = 0 Where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Size_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Size_IdNo = 0 Where Size_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Colour_IdNo = 0 Where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table Item_Head add Item_Description varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Head set Item_Description = '' Where Item_Description is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Scheme_Disc_Percentage Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Scheme_Disc_Percentage = 0 Where Scheme_Disc_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Scheme_Discount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Scheme_Discount = 0 Where Scheme_Discount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Trade_Disc_Percentage Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Trade_Disc_Percentage = 0 Where Trade_Disc_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Trade_Discount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Trade_Discount = 0 Where Trade_Discount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Cgst_Percentage Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Cgst_Percentage = 0 Where Cgst_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Cgst_Amount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Cgst_Amount = 0 Where Cgst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Sgst_Percentage Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Sgst_Percentage = 0 Where Sgst_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Sgst_Amount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Sgst_Amount = 0 Where Sgst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Igst_Percentage Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Igst_Percentage = 0 Where Igst_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Igst_Amount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Igst_Amount = 0 Where Igst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Net_Amount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Net_Amount = 0 Where Net_Amount is Null"
        cmd.ExecuteNonQuery()

        '=========

        cmd.CommandText = "Alter table Sales_Details add Total_Rate Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Total_Rate = 0 Where Total_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Scheme_UCP Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Scheme_UCP = 0 Where Scheme_UCP is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Discount_Total Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Discount_Total = 0 Where Discount_Total is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Ledger_DiscountDetails](	[Ledger_IdNo] [int] NOT NULL,	[Sl_No] [int] NOT NULL,	[ItemGroup_IdNo] [int] NULL,	[Discount_Percentage] [numeric](18, 2) NULL," & _
                          " CONSTRAINT [PK_Ledger_DiscountDetails] PRIMARY KEY CLUSTERED (	[Ledger_IdNo] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Scheme_Details](	[Scheme_IdNo] [int] NOT NULL,	[Sl_No] [int] NOT NULL,	[Item_IdNo] [int] NULL,	[Discount_Percentage] [numeric](18, 2) NULL,	[Primary_StartDate] [smalldatetime] NULL,	[Primary_EndDate] [smalldatetime] NULL,	[Secondary_StartDate] [smalldatetime] NULL,	[Secondary_EndDate] [smalldatetime] NULL,	[Primary_StartDate_Text] [varchar](50) NULL,	[Primary_EndDate_Text] [varchar](50) NULL,	[Secondary_StartDate_Text] [varchar](50) NULL,	[Secondary_EndDate_Text] [varchar](50) NULL," & _
                          " CONSTRAINT [PK_Scheme_Details] PRIMARY KEY CLUSTERED (	[Scheme_IdNo], 	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Scheme_Head](	[Scheme_IdNo] [int] NOT NULL,	[Scheme_Name] [varchar](200) NULL,	[Sur_Name] [varchar](200) NULL,	[Cetegory_IdNo] [int] NULL," & _
                          " CONSTRAINT [PK_Scheme_Head] PRIMARY KEY CLUSTERED (        [Scheme_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Order_Image image"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Production_Details add Ordercode_forSelection varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Production_Details set Ordercode_forSelection = '' Where Ordercode_forSelection is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Ordercode_forSelection varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Ordercode_forSelection = '' Where Ordercode_forSelection is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Simple_Receipt_Details add Order_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Simple_Receipt_Details set Order_No = '' Where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Simple_Receipt_Details add Ordercode_forSelection varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Simple_Receipt_Details set Ordercode_forSelection = '' Where Ordercode_forSelection is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Close_Status int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Close_Status = 0 Where Close_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Ordercode_forSelection varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Ordercode_forSelection = '' Where Ordercode_forSelection is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add StchsPr_Pcs NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set StchsPr_Pcs = 0 Where StchsPr_Pcs is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Stiches NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Stiches = 0 Where Stiches is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Pieces = 0 Where Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Receipt_Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Receipt_Pieces = 0 Where Receipt_Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Delivery_Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Delivery_Pieces = 0 Where Delivery_Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Production_Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Production_Pieces = 0 Where Production_Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Pieces = 0 Where Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Rate NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Rate = 0 Where Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Amount = 0 Where Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Design varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Design = '' Where Design is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Cheque_Print_Positioning_Head](	[Cheque_Print_Positioning_No] [varchar](50) NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[Paper_Orientation] [varchar](50) NULL,	[Left_Margin] [numeric](18, 3) NULL,	[Top_Margin] [numeric](18, 3) NULL,	[Account_No] [varchar](50) NULL,	[Ac_Payee_Left] [numeric](18, 3) NULL,	[Ac_Payee_Top] [numeric](18, 3) NULL,	[Ac_Payee_Width] [numeric](18, 3) NULL,	[Date_Left] [numeric](18, 3) NULL,	[Date_Top] [numeric](18, 3) NULL,	[Date_Width] [numeric](18, 3) NULL,	[PartyName_Left] [numeric](18, 3) NULL,	[PartyName_Top] [numeric](18, 3) NULL,	[PartyName_Width] [numeric](18, 3) NULL,	[Second_PartyName_Left] [numeric](18, 3) NULL,	[Second_PartyName_Top] [numeric](18, 3) NULL,	[Second_PartyName_Width] [numeric](18, 3) NULL,	[AmountWords_Left] [numeric](18, 3) NULL,	[AmountWords_Top] [numeric](18, 3) NULL,	[AmountWords_Width] [numeric](18, 3) NULL,	[Second_AmountWords_Left] [numeric](18, 3) NULL,	[Second_AmountWords_Top] [numeric](18, 3) NULL,	[Second_AmountWords_Width] [numeric](18, 3) NULL,	[Rupees_Left] [numeric](18, 3) NULL,	[Rupees_Top] [numeric](18, 3) NULL,	[Rupees_Width] [numeric](18, 3) NULL,	[CompanyName_Left] [numeric](18, 3) NULL,	[CompanyName_Top] [numeric](18, 3) NULL,	[CompanyName_Width] [numeric](18, 3) NULL,	[Partner_Left] [numeric](18, 3) NULL,	[Partner_Top] [numeric](18, 3) NULL,	[Partner_Width] [numeric](18, 3) NULL,	[AccountNo_Left] [numeric](18, 3) NULL,	[AccountNo_Top] [numeric](18, 3) NULL,	[AccountNo_Width] [numeric](18, 3) NULL,	[Partner] [varchar](50) NULL," & _
                              " CONSTRAINT [PK_Cheque_Print_Positining_Head] PRIMARY KEY CLUSTERED ([Ledger_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Sales_Quotation_Image image"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Sales_Quotation_Image2 image"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Order_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Order_No = '' Where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Design1 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Design1 = '' Where Design1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Design2 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Design2 = '' Where Design2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Stitches1 int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Stitches1 = 0 Where Stitches1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Stitches2 int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Stitches2 = 0 Where Stitches2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Rate_Applique NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Rate_Applique = 0 Where Rate_Applique is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Rate_Embroidery NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Rate_Embroidery = 0 Where Rate_Embroidery is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Rate_Stitches NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Rate_Stitches = 0 Where Rate_Stitches is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Quotation_Head add Rate_Pasting NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Rate_Pasting = 0 Where Rate_Pasting is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Entry_Status varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Entry_Status = '' Where Entry_Status is Null"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "CREATE TABLE [Simple_Receipt_Details]([Simple_Receipt_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Simple_Receipt_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Simple_Receipt_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL,	[Unit_IdNo] [smallint] NULL,	[Quantity] [numeric](18, 3) NULL,	[Rate] [numeric](18, 2) NULL,	[Amount] [numeric](18, 2) NULL,	[Item_Description] [varchar](500) NULL,	[Simple_Receipt_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,	[Receipt_Quantity] [numeric](18, 2) NULL,	[No_Of_Rolls] [numeric](18, 2) NULL,	[Entry_Type] [varchar](30) NULL,	[Order_Detail_SlNo] [int] NULL,	[Noof_Items] [numeric](18, 2) NULL,	[HSN_Code] [varchar](50) NULL,	[Tax_Perc] [numeric](18, 2) NULL,	[Assessable_Value] [numeric](18, 2) NULL,	[Size_Idno] [int] NULL,	[Style_Idno] [int] NULL, CONSTRAINT [PK_Simple_Receipt_Details] PRIMARY KEY CLUSTERED (	[Simple_Receipt_Code] ASC,	[SL_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Simple_Receipt_Head](	[Simple_Receipt_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Simple_Receipt_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Simple_Receipt_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NULL,	[Order_No] [varchar](50) NULL,	[Order_Date] [varchar](50) NULL,	[Total_Qty] [numeric](18, 3) NULL,	[Gross_Amount] [numeric](18, 2) NULL,	[Vehicle_No] [varchar](50) NULL,	[Transport_IdNo] [int] NULL,	[Remarks] [varchar](500) NULL,	[Entry_VAT_GST_Type] [varchar](50) NULL,	[Assessable_Value] [numeric](18, 2) NULL,	[CGst_Amount] [numeric](18, 2) NULL,	[SGst_Amount] [numeric](18, 2) NULL,	[IGst_Amount] [numeric](18, 2) NULL,	[SubTotal_Amount] [numeric](18, 2) NULL,	[Net_Amount] [numeric](18, 2) NULL,	[Round_Off] [numeric](18, 2) NULL,	[Entry_GST_Tax_Type] [varchar](20) NULL,	[Total_Bags] [int] NULL,	[Electronic_Reference_No] [varchar](100) NULL,	[Transportation_Mode] [varchar](100) NULL,	[Date_Time_Of_Supply] [varchar](100) NULL,	[Weight] [numeric](18, 2) NULL,	[Freight_ToPay_Amount] [numeric](18, 2) NULL,	[charge] [numeric](18, 2) NULL,	[Lr_Date] [varchar](50) NULL,	[Lr_No] [varchar](50) NULL,	[Booked_By] [varchar](50) NULL,	[Entry_Type] [varchar](50) NULL, CONSTRAINT [PK_Simple_Receipt_Head] PRIMARY KEY CLUSTERED (	[Simple_Receipt_Code] ASC,	[Simple_Receipt_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Production_Head]( " & _
                            "[Production_Code] [varchar](50) NOT NULL," & _
                            "[Company_IdNo] [smallint] NOT NULL," & _
                            "[Production_No] [varchar](50) NOT NULL," & _
                            "[for_OrderBy] [numeric](18, 2) NOT NULL," & _
                            "[Production_Date] [datetime] NOT NULL," & _
                            "[Ledger_IdNo] [int] NULL, " & _
                            "[Remarks] [varchar](500) NULL," & _
                            "[Shift][varchar](500) NULL," & _
                            "[Machine_IdNo] [int] NULL," & _
                            "[Operator_IdNo] [int] NULL," & _
                            "[Framer_IdNo] [int] NULL," & _
                            "[Total_Heads] [numeric](18, 3) NULL," & _
                            "[Total_Stchs] [numeric](18, 3) NULL," & _
                            "[Total_Pcs] [numeric](18, 3) NULL," & _
                            "[Total_Amt] [numeric](18, 3) NULL," & _
                            "CONSTRAINT [PK_Production_Head] PRIMARY KEY CLUSTERED ([Production_Code] ASC)ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Production_Details](" & _
                                "[Production_Code] [varchar](50) NOT NULL, " & _
                             "[Company_IdNo] [smallint] NOT NULL," & _
                             "[Production_No] [varchar](20) NOT NULL, " & _
                             "[for_OrderBy] [numeric](18, 2) NOT NULL, " & _
                             "[Production_Date] [smalldatetime] NOT NULL, " & _
                             "[Ledger_IdNo] [int] NULL, " & _
                             "[SL_No] [smallint] NOT NULL, " & _
                             "[Order_No] [varchar](20) NULL, " & _
                             "[Colour_IdNo] [int] NULL, " & _
                             "[Design] [varchar](500) NULL, " & _
                             "[StchsPr_Pcs] [numeric](18, 3) NULL, " & _
                             "[Head] [numeric](18, 3) NULL, " & _
                             "[Stiches] [numeric](18, 3) NULL, " & _
                             "[Pieces] [numeric](18, 3) NULL, " & _
                             "[Rate] [numeric](18, 3) NULL, " & _
                             "[Amount] [numeric](18, 3) NULL, " & _
                                " CONSTRAINT [PK_Production_Details] PRIMARY KEY CLUSTERED ([Production_Code] ASC, [SL_No] ASC )ON [PRIMARY]) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Employee_Head](	[Employee_IdNo] [int] NOT NULL,	[Employee_Name] [varchar](100) NOT NULL,	[Sur_name] [varchar](100) NOT NULL,	[Salary_Bobin] [numeric](18, 2) NULL, CONSTRAINT [PK_Employee_Head] PRIMARY KEY CLUSTERED (	[Employee_IdNo] ASC) ON [PRIMARY], CONSTRAINT [IX_Employee_Head] UNIQUE NONCLUSTERED (	[Sur_name] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Design_Picture image"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Update Sales_Details set Design_Picture = '' Where Design_Picture is Null"
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Rate_For varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Rate_For = '' Where Rate_For is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Challan_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Challan_No = '' Where Challan_No is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Challan_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Challan_Date = '' Where Challan_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Order_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Order_No = '' Where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Order_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Order_Date = '' Where Order_Date is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Details add Challan_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Challan_No = '' Where Challan_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Challan_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Challan_Date = '' Where Challan_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Order_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Order_No = '' Where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Order_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Order_Date = '' Where Order_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Head add Challan_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set Challan_No = '' Where Challan_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Head add Challan_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set Challan_Date = '' Where Challan_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Details add Delivery_Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Details set Delivery_Weight = 0 Where Delivery_Weight is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Head add Total_Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Total_Weight = 0 Where Total_Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Head add Total_Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set Total_Weight = 0 Where Total_Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Details add Rate_For varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Details set Rate_For = '' Where Rate_For is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Receipt_Details add ItemGroup_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Details set ItemGroup_IdNo = 0 Where ItemGroup_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Details add Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Details set Weight = 0 Where Weight is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Job_Card_Head add Total_WasteQuantity NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Job_Card_Head set Total_WasteQuantity = 0 Where Total_WasteQuantity is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Job_Card_Waste_Details](	[Job_Card_Code] [varchar](100) NOT NULL,	[Company_IdNo] [int] NULL,	[Job_Card_No] [varchar](50) NULL,	[for_OrderBy] [numeric](18, 2) NULL,	[Job_Card_Date] [smalldatetime] NULL,	[Ledger_IdNo] [int] NULL,	[SL_No] [int] NOT NULL,	[Item_IdNo] [int] NULL,	[Quantity] [numeric](18, 2) NULL," & _
                         " CONSTRAINT [PK_Job_Card_Waste_Details] PRIMARY KEY CLUSTERED (	[Job_Card_Code],	[SL_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Site_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Site_IdNo = 0 Where Site_IdNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into Site_Head(Site_IdNo, Site_Name, Sur_Name,Pk_Condition) Values (0, '', '', '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Site_Head](	[Site_IdNo] [smallint] NOT NULL,	[Site_Name] [varchar](100) NOT NULL,	[Sur_Name] [varchar](100) NOT NULL,	[Pk_Condition] [varchar](50) NULL," & _
                                             "  CONSTRAINT [PK_Site_Head] PRIMARY KEY CLUSTERED ( [Site_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Inv_No_Prefix1 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Inv_No_Prefix1 = '' Where Inv_No_Prefix1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Inv_No_Prefix2 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Inv_No_Prefix2 = '' Where Inv_No_Prefix2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add StchsPr_Pcs NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set StchsPr_Pcs = 0 Where StchsPr_Pcs is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Stiches NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Stiches = 0 Where Stiches is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Pieces = 0 Where Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Receipt_Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Receipt_Pieces = 0 Where Receipt_Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Delivery_Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Delivery_Pieces = 0 Where Delivery_Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Production_Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Production_Pieces = 0 Where Production_Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Pieces NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Pieces = 0 Where Pieces is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Rate NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Rate = 0 Where Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Amount = 0 Where Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Order_Program_Head add Design varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Order_Program_Head set Design = '' Where Design is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Cheque_Print_Positioning_Head](	[Cheque_Print_Positioning_No] [varchar](50) NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[Paper_Orientation] [varchar](50) NULL,	[Left_Margin] [numeric](18, 3) NULL,	[Top_Margin] [numeric](18, 3) NULL,	[Account_No] [varchar](50) NULL,	[Ac_Payee_Left] [numeric](18, 3) NULL,	[Ac_Payee_Top] [numeric](18, 3) NULL,	[Ac_Payee_Width] [numeric](18, 3) NULL,	[Date_Left] [numeric](18, 3) NULL,	[Date_Top] [numeric](18, 3) NULL,	[Date_Width] [numeric](18, 3) NULL,	[PartyName_Left] [numeric](18, 3) NULL,	[PartyName_Top] [numeric](18, 3) NULL,	[PartyName_Width] [numeric](18, 3) NULL,	[Second_PartyName_Left] [numeric](18, 3) NULL,	[Second_PartyName_Top] [numeric](18, 3) NULL,	[Second_PartyName_Width] [numeric](18, 3) NULL,	[AmountWords_Left] [numeric](18, 3) NULL,	[AmountWords_Top] [numeric](18, 3) NULL,	[AmountWords_Width] [numeric](18, 3) NULL,	[Second_AmountWords_Left] [numeric](18, 3) NULL,	[Second_AmountWords_Top] [numeric](18, 3) NULL,	[Second_AmountWords_Width] [numeric](18, 3) NULL,	[Rupees_Left] [numeric](18, 3) NULL,	[Rupees_Top] [numeric](18, 3) NULL,	[Rupees_Width] [numeric](18, 3) NULL,	[CompanyName_Left] [numeric](18, 3) NULL,	[CompanyName_Top] [numeric](18, 3) NULL,	[CompanyName_Width] [numeric](18, 3) NULL,	[Partner_Left] [numeric](18, 3) NULL,	[Partner_Top] [numeric](18, 3) NULL,	[Partner_Width] [numeric](18, 3) NULL,	[AccountNo_Left] [numeric](18, 3) NULL,	[AccountNo_Top] [numeric](18, 3) NULL,	[AccountNo_Width] [numeric](18, 3) NULL,	[Partner] [varchar](50) NULL," & _
                              " CONSTRAINT [PK_Cheque_Print_Positining_Head] PRIMARY KEY CLUSTERED ([Ledger_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Sales_Quotation_Image image"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Sales_Quotation_Image2 image"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Order_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Order_No = '' Where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Design1 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Design1 = '' Where Design1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Design2 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Design2 = '' Where Design2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Stitches1 int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Stitches1 = 0 Where Stitches1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Stitches2 int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Stitches2 = 0 Where Stitches2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Rate_Applique NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Rate_Applique = 0 Where Rate_Applique is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Rate_Embroidery NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Rate_Embroidery = 0 Where Rate_Embroidery is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Rate_Stitches NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Rate_Stitches = 0 Where Rate_Stitches is Null"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter table Sales_Head add Entry_Status varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Entry_Status = '' Where Entry_Status is Null"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "CREATE TABLE [Simple_Receipt_Details]([Simple_Receipt_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Simple_Receipt_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Simple_Receipt_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL,	[Unit_IdNo] [smallint] NULL,	[Quantity] [numeric](18, 3) NULL,	[Rate] [numeric](18, 2) NULL,	[Amount] [numeric](18, 2) NULL,	[Item_Description] [varchar](500) NULL,	[Simple_Receipt_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,	[Receipt_Quantity] [numeric](18, 2) NULL,	[No_Of_Rolls] [numeric](18, 2) NULL,	[Entry_Type] [varchar](30) NULL,	[Order_Detail_SlNo] [int] NULL,	[Noof_Items] [numeric](18, 2) NULL,	[HSN_Code] [varchar](50) NULL,	[Tax_Perc] [numeric](18, 2) NULL,	[Assessable_Value] [numeric](18, 2) NULL,	[Size_Idno] [int] NULL,	[Style_Idno] [int] NULL, CONSTRAINT [PK_Simple_Receipt_Details] PRIMARY KEY CLUSTERED (	[Simple_Receipt_Code] ASC,	[SL_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Simple_Receipt_Head](	[Simple_Receipt_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Simple_Receipt_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Simple_Receipt_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NULL,	[Order_No] [varchar](50) NULL,	[Order_Date] [varchar](50) NULL,	[Total_Qty] [numeric](18, 3) NULL,	[Gross_Amount] [numeric](18, 2) NULL,	[Vehicle_No] [varchar](50) NULL,	[Transport_IdNo] [int] NULL,	[Remarks] [varchar](500) NULL,	[Entry_VAT_GST_Type] [varchar](50) NULL,	[Assessable_Value] [numeric](18, 2) NULL,	[CGst_Amount] [numeric](18, 2) NULL,	[SGst_Amount] [numeric](18, 2) NULL,	[IGst_Amount] [numeric](18, 2) NULL,	[SubTotal_Amount] [numeric](18, 2) NULL,	[Net_Amount] [numeric](18, 2) NULL,	[Round_Off] [numeric](18, 2) NULL,	[Entry_GST_Tax_Type] [varchar](20) NULL,	[Total_Bags] [int] NULL,	[Electronic_Reference_No] [varchar](100) NULL,	[Transportation_Mode] [varchar](100) NULL,	[Date_Time_Of_Supply] [varchar](100) NULL,	[Weight] [numeric](18, 2) NULL,	[Freight_ToPay_Amount] [numeric](18, 2) NULL,	[charge] [numeric](18, 2) NULL,	[Lr_Date] [varchar](50) NULL,	[Lr_No] [varchar](50) NULL,	[Booked_By] [varchar](50) NULL,	[Entry_Type] [varchar](50) NULL, CONSTRAINT [PK_Simple_Receipt_Head] PRIMARY KEY CLUSTERED (	[Simple_Receipt_Code] ASC,	[Simple_Receipt_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Production_Head]( " & _
                            "[Production_Code] [varchar](50) NOT NULL," & _
                            "[Company_IdNo] [smallint] NOT NULL," & _
                            "[Production_No] [varchar](50) NOT NULL," & _
                            "[for_OrderBy] [numeric](18, 2) NOT NULL," & _
                            "[Production_Date] [datetime] NOT NULL," & _
                            "[Ledger_IdNo] [int] NULL, " & _
                            "[Remarks] [varchar](500) NULL," & _
                            "[Shift][varchar](500) NULL," & _
                            "[Machine_IdNo] [int] NULL," & _
                            "[Operator_IdNo] [int] NULL," & _
                            "[Framer_IdNo] [int] NULL," & _
                            "[Total_Heads] [numeric](18, 3) NULL," & _
                            "[Total_Stchs] [numeric](18, 3) NULL," & _
                            "[Total_Pcs] [numeric](18, 3) NULL," & _
                            "[Total_Amt] [numeric](18, 3) NULL," & _
                            "CONSTRAINT [PK_Production_Head] PRIMARY KEY CLUSTERED ([Production_Code] ASC)ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Production_Details](" & _
                                "[Production_Code] [varchar](50) NOT NULL, " & _
                             "[Company_IdNo] [smallint] NOT NULL," & _
                             "[Production_No] [varchar](20) NOT NULL, " & _
                             "[for_OrderBy] [numeric](18, 2) NOT NULL, " & _
                             "[Production_Date] [smalldatetime] NOT NULL, " & _
                             "[Ledger_IdNo] [int] NULL, " & _
                             "[SL_No] [smallint] NOT NULL, " & _
                             "[Order_No] [varchar](20) NULL, " & _
                             "[Colour_IdNo] [int] NULL, " & _
                             "[Design] [varchar](500) NULL, " & _
                             "[StchsPr_Pcs] [numeric](18, 3) NULL, " & _
                             "[Head] [numeric](18, 3) NULL, " & _
                             "[Stiches] [numeric](18, 3) NULL, " & _
                             "[Pieces] [numeric](18, 3) NULL, " & _
                             "[Rate] [numeric](18, 3) NULL, " & _
                             "[Amount] [numeric](18, 3) NULL, " & _
                                " CONSTRAINT [PK_Production_Details] PRIMARY KEY CLUSTERED ([Production_Code] ASC, [SL_No] ASC )ON [PRIMARY]) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Employee_Head](	[Employee_IdNo] [int] NOT NULL,	[Employee_Name] [varchar](100) NOT NULL,	[Sur_name] [varchar](100) NOT NULL,	[Salary_Bobin] [numeric](18, 2) NULL, CONSTRAINT [PK_Employee_Head] PRIMARY KEY CLUSTERED (	[Employee_IdNo] ASC) ON [PRIMARY], CONSTRAINT [IX_Employee_Head] UNIQUE NONCLUSTERED (	[Sur_name] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Design_Picture image"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Update Sales_Details set Design_Picture = '' Where Design_Picture is Null"
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Rate_For varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Rate_For = '' Where Rate_For is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Challan_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Challan_No = '' Where Challan_No is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Challan_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Challan_Date = '' Where Challan_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Order_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Order_No = '' Where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Order_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Order_Date = '' Where Order_Date is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Details add Challan_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Challan_No = '' Where Challan_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Challan_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Challan_Date = '' Where Challan_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Order_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Order_No = '' Where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Order_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Order_Date = '' Where Order_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Head add Challan_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set Challan_No = '' Where Challan_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Head add Challan_Date varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set Challan_Date = '' Where Challan_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Details add Delivery_Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Details set Delivery_Weight = 0 Where Delivery_Weight is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Head add Total_Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Total_Weight = 0 Where Total_Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Head add Total_Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set Total_Weight = 0 Where Total_Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Details add Rate_For varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Details set Rate_For = '' Where Rate_For is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Details add Rate_For varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Rate_For = '' Where Rate_For is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add ItemGroup_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set ItemGroup_IdNo = 0 Where ItemGroup_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Weight = 0 Where Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Processing_Details add Weight NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Processing_Details set Weight = 0 Where Weight is Null"
        cmd.ExecuteNonQuery()

        '__________________________________________________________________________________________

        cmd.CommandText = "Alter table Sales_Details add Rate_For varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Rate_For = '' Where Rate_For is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Token_Monthly_Head](	[Token_Monthly_Code] [varchar](50) NOT NULL,	[Company_IdNo] [int] NULL,	[Token_Monthly_No] [varchar](100) NULL,	[for_OrderBy] [numeric](18, 2) NULL,	[Token_Monthly_Date] [smalldatetime] NULL,	[Vehicle_No] [varchar](50) NULL,	[Ledger_Idno] [int] NULL,	[StartDate] [smalldatetime] NULL,	[EndDate] [smalldatetime] NULL,	[StartDateTime] [datetime] NULL,	[EndDateTime] [datetime] NULL,	[Total_Days] [numeric](18, 2) NULL,	[Party_Name] [varchar](100) NULL,	[Party_Address1] [varchar](100) NULL,	[Party_Address2] [varchar](100) NULL,	[Party_Address3] [varchar](100) NULL,	[Party_Address4] [varchar](100) NULL,	[Party_MobileNo] [varchar](100) NULL,	[Rate] [numeric](18, 2) NULL,	[Amount] [numeric](18, 2) NULL,	[Vehicle_Type] [varchar](100) NULL,	[Sur_Name] [varchar](100) NULL,	[Close_Status] [int] NULL," & _
                            "  CONSTRAINT [PK_Token_Monthly_Head] PRIMARY KEY CLUSTERED (  [Token_Monthly_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Tocken_Head add Vehicle_Type varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Tocken_Head set Vehicle_Type = '' Where Vehicle_Type is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Rate_Head](	[Rate_Per_Hour] [numeric](18, 2) NULL,	[Rate_Per_Day] [numeric](18, 2) NULL,	[Rate_Per_Month] [numeric](18, 2) NULL,	[Company_Idno] [int] NULL) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Tocken_Head](	[Tocken_Code] [varchar](50) NOT NULL,	[Company_IdNo] [int] NOT NULL,	[Tocken_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Tocken_Date] [smalldatetime] NULL,	[Vehicle_No] [varchar](50) NULL,	[Ledger_Idno] [int] NULL,	[InTime] [smalldatetime] NULL,	[OutTime] [smalldatetime] NULL,	[InDateTime] [datetime] NULL,	[OutDateTime] [datetime] NULL,	[Total_Hrs] [numeric](18, 2) NULL,	[Total_Days] [numeric](18, 2) NULL,	[Party_Name] [varchar](100) NULL,	[Address1] [varchar](200) NULL,	[Address2] [varchar](200) NULL,	[Address3] [varchar](200) NULL,	[Address4] [varchar](200) NULL,	[Mobile_No] [varchar](100) NULL,	[Rate] [numeric](18, 2) NULL,	[Amount] [numeric](18, 2) NULL,	[Tocken_Type] [varchar](50) NULL, " & _
                         " CONSTRAINT [PK_Tocken_Head] PRIMARY KEY CLUSTERED (        [Tocken_Code]) ON [PRIMARY]) ON [PRIMARY]"

        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_head add Rate_For_1000 NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ledger_head set Rate_For_1000 = 0 where Rate_For_1000 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_head add Minimum_Pcs NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ledger_head set Minimum_Pcs = 0 where Minimum_Pcs is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_head add Minimum_Bill_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ledger_head set Minimum_Bill_Amount = 0 where Minimum_Bill_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Entry_VAT_GST_Type varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Entry_VAT_GST_Type = '' Where Entry_VAT_GST_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add  Cash_Discount_Amount_For_All_Item   numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set  Cash_Discount_Amount_For_All_Item   = 0 Where  Cash_Discount_Amount_For_All_Item   is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add Cash_Discount_Perc_For_All_Item   numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set Cash_Discount_Perc_For_All_Item   = 0 Where Cash_Discount_Perc_For_All_Item   is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Entry_GST_Tax_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Entry_GST_Tax_Type = '' Where Entry_GST_Tax_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add CGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set CGst_Amount = 0 where CGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add SGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set SGst_Amount = 0 where SGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add IGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set IGst_Amount = 0 where IGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add Assessable_Value NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set Assessable_Value = 0 where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add HSN_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set HSN_Code = '' where HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add GST_Perc NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set GST_Perc = 0 where GST_Perc is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Electronic_Reference_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Electronic_Reference_No  = '' Where Electronic_Reference_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Transportation_Mode varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Transportation_Mode = '' Where Transportation_Mode is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Date_Time_Of_Supply varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Date_Time_Of_Supply = '' Where Date_Time_Of_Supply is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Price_List_Head add Ledger_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Price_List_Head set Ledger_IdNo = 0 where Ledger_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Price_List_Details add Size_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Price_List_Details set Size_IdNo = 0 where Size_IdNo is Null"
        cmd.ExecuteNonQuery()
        '__________________________________________________________________________________________

        'TALLY EXPORT
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            Da = New SqlClient.SqlDataAdapter("select Insurance_No ,Ledger_Idno from Ledger_Head Where Ledger_Type = 'WEAVER' and Insurance_No <> ''", cn1)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1
                    If IsDBNull(Dt1.Rows(I).Item("Ledger_Idno").ToString) = False Then
                        If Trim(Dt1.Rows(I).Item("Insurance_No").ToString) <> "" Then
                            cmd.CommandText = "Update Ledger_Head set Insurance_No = '' ,   Noof_Looms = '" & Trim(Dt1.Rows(I).Item("Insurance_No").ToString) & "' Where Ledger_Idno =" & Val(Dt1.Rows(I).Item("Ledger_Idno").ToString)
                            cmd.ExecuteNonQuery()
                        End If
                    End If
                Next I
            End If
            Dt1.Clear()
            Dt1.Dispose()
            Da.Dispose()
        End If

        cmd.CommandText = " ALTER TABLE Purchase_Details ALTER COLUMN  Rate NUMERIC(18,3) "
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Quantity NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Quantity = 0 where Quantity is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Rate_1000Stitches NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Rate_1000Stitches = 0 where Rate_1000Stitches is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Design_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Design_No = '' where Design_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Details_Design varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Details_Design = '' where Details_Design is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_return_Head add CGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_return_Head set CGst_Amount = 0 where CGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_return_Head add SGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_return_Head set SGst_Amount = 0 where SGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_return_Head add IGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_return_Head set IGst_Amount = 0 where IGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Return_Details add Footer_Cash_Discount_Perc_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Return_Details set Footer_Cash_Discount_Perc_For_All_Item = 0 where Footer_Cash_Discount_Perc_For_All_Item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Return_Details add Footer_Cash_Discount_Amount_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Return_Details set Footer_Cash_Discount_Amount_For_All_Item = 0 where Footer_Cash_Discount_Amount_For_All_Item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Return_Details add Assessable_Value NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Return_Details set Assessable_Value = 0 where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Return_Details add HSN_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Return_Details set HSN_Code = '' where HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Return_Details add Gst_Perc  NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Return_Details set Gst_Perc = 0 where Gst_Perc is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Purchase_Return_GST_Tax_Details](	[Purchase_Return_Code] [varchar](50) NOT NULL,	[Company_Idno] [int] NOT NULL,	[Purchase_Return_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Purchase_Return_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL , [Sl_No] [int] NOT NULL,	[HSN_Code] [varchar](100) NULL,	[Taxable_Amount] [numeric](18, 2) NULL,	[CGST_Percentage] [numeric](18, 2) NULL,	[CGST_Amount] [numeric](18, 2) NULL,	[SGST_Percentage] [numeric](18, 2) NULL,	[SGST_Amount] [numeric](18, 2) NULL,	[IGST_Percentage] [numeric](18, 2) NULL,	[IGST_Amount] [numeric](18, 2) NULL, " & _
                          " CONSTRAINT [PK_Purchase_Return_GST_Tax_Details] PRIMARY KEY CLUSTERED (	[Purchase_Return_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_return_Head add Entry_VAT_GST_Type varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_return_Head set Entry_VAT_GST_Type = '' Where Entry_VAT_GST_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Return_GST_Tax_Details](	[Sales_Return_Code] [varchar](50) NOT NULL,	[Company_Idno] [int] NOT NULL,	[Sales_Return_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Sales_Return_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL , [Sl_No] [int] NOT NULL,	[HSN_Code] [varchar](100) NULL,	[Taxable_Amount] [numeric](18, 2) NULL,	[CGST_Percentage] [numeric](18, 2) NULL,	[CGST_Amount] [numeric](18, 2) NULL,	[SGST_Percentage] [numeric](18, 2) NULL,	[SGST_Amount] [numeric](18, 2) NULL,	[IGST_Percentage] [numeric](18, 2) NULL,	[IGST_Amount] [numeric](18, 2) NULL, " & _
                           " CONSTRAINT [PK_Sales_Return_GST_Tax_Details] PRIMARY KEY CLUSTERED (	[Sales_Return_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Entry_VAT_GST_Type varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Entry_VAT_GST_Type = '' Where Entry_VAT_GST_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Electronic_Reference_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Electronic_Reference_No  = '' Where Electronic_Reference_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Transportation_Mode varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Transportation_Mode = '' Where Transportation_Mode is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Date_Time_Of_Supply varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Date_Time_Of_Supply = '' Where Date_Time_Of_Supply is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Entry_GST_Tax_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Entry_GST_Tax_Type = '' Where Entry_GST_Tax_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add CGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set CGst_Amount = 0 where CGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add SGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set SGst_Amount = 0 where SGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add IGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set IGst_Amount = 0 where IGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Cash_Discount_Perc_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Cash_Discount_Perc_For_All_Item = 0 where Cash_Discount_Perc_For_All_Item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Cash_Discount_Amount_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Cash_Discount_Amount_For_All_Item = 0 where Cash_Discount_Amount_For_All_Item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Assessable_Value NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Assessable_Value = 0 where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add HSN_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set HSN_Code = '' where HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Tax_Perc  NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Tax_Perc = 0 where Tax_Perc is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Quotation_Head add Entry_VAT_GST_Type varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Entry_VAT_GST_Type = '' Where Entry_VAT_GST_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add  Cash_Discount_Amount_For_All_Item   numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set  Cash_Discount_Amount_For_All_Item   = 0 Where  Cash_Discount_Amount_For_All_Item   is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Quotation_Details add Cash_Discount_Perc_For_All_Item   numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set Cash_Discount_Perc_For_All_Item   = 0 Where Cash_Discount_Perc_For_All_Item   is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Quotation_Head add Entry_GST_Tax_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Entry_GST_Tax_Type = '' Where Entry_GST_Tax_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add CGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set CGst_Amount = 0 where CGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add SGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set SGst_Amount = 0 where SGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add IGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set IGst_Amount = 0 where IGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add Assessable_Value NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set Assessable_Value = 0 where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add HSN_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set HSN_Code = '' where HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add GST_Perc NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details set GST_Perc = 0 where GST_Perc is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Electronic_Reference_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Electronic_Reference_No  = '' Where Electronic_Reference_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Transportation_Mode varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Transportation_Mode = '' Where Transportation_Mode is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Date_Time_Of_Supply varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Date_Time_Of_Supply = '' Where Date_Time_Of_Supply is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] CHECK CONSTRAINT [CK_Sales_Receipt_Details_2]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Receipt_Details_2] CHECK  (([Quantity]>=[Delivery_Quantity]))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] CHECK CONSTRAINT [CK_Sales_Receipt_Details_1]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Receipt_Details_1] CHECK  (([Delivery_Quantity]>=(0)))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Entry_Type VARCHAR(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Entry_Type = '' where Entry_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Receipt_No VARCHAR(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Receipt_No = '' where Receipt_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Sales_Receipt_Code VARCHAR(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Sales_Receipt_Code = '' where Sales_Receipt_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Sales_Receipt_Detail_Slno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Sales_Receipt_Detail_Slno = 0 where Sales_Receipt_Detail_Slno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Head add SubTotal_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set SubTotal_Amount = 0 where SubTotal_Amount is Null"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table Sales_Receipt_Head add Entry_GST_Tax_Type VARCHAR(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set Entry_GST_Tax_Type = '' where Entry_GST_Tax_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Head add Booked_By VARCHAR(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Receipt_Head set Booked_By = '' where Booked_By is Null"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "CREATE TABLE [Sales_Receipt_Head]([Sales_Receipt_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Sales_Receipt_No] [varchar](50) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Sales_Receipt_Date] [smalldatetime] NOT NULL,[Ledger_IdNo] [int] NULL,[Order_No] [varchar](50) NULL,[Order_Date] [varchar](50) NULL,[Total_Qty] [numeric](18, 3) NULL,[Gross_Amount] [numeric](18, 2) NULL,[Vehicle_No] [varchar](50) NULL,[Transport_IdNo] [int] NULL,[Remarks] [varchar](500) NULL,[Assessable_Value] [numeric](18, 2) NULL,[Freight_ToPay_Amount] [numeric](18, 2) NULL,[Weight] [numeric](18, 3) NULL,[Charge] [numeric](18, 2) NULL,[Lr_No] [varchar](50) NULL,[Lr_Date] [varchar](50) NULL,[Total_Bags] [numeric](18, 2) NULL,[Round_Off] [numeric](18, 2) NULL,[Net_Amount] [numeric](18, 2) NULL,[Electronic_Reference_No] [varchar](100) NULL,[Transportation_Mode] [varchar](100) NULL,[Date_Time_Of_Supply] [varchar](100) NULL,[CGst_Amount] [numeric](18, 2) NULL,[SGst_Amount] [numeric](18, 2) NULL,[IGst_Amount] [numeric](18, 2) NULL," & _
 "CONSTRAINT [PK_Sales_Receipt_Head] PRIMARY KEY CLUSTERED ([Sales_Receipt_Code] Asc) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Order_No]  DEFAULT ('') FOR [Order_No]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Order_Date]  DEFAULT ('') FOR [Order_Date]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Total_Qty]  DEFAULT ((0)) FOR [Total_Qty]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Gross_Amount]  DEFAULT ((0)) FOR [Gross_Amount]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Vehicle_No]  DEFAULT ('') FOR [Vehicle_No]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Transport_IdNo]  DEFAULT ((0)) FOR [Transport_IdNo]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Remarks]  DEFAULT ('') FOR [Remarks]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Assessable_Value]  DEFAULT ((0)) FOR [Assessable_Value]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Freight_ToPay_Amount]  DEFAULT ((0)) FOR [Freight_ToPay_Amount]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Weight]  DEFAULT ((0)) FOR [Weight]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Charge]  DEFAULT ((0)) FOR [Charge]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Lr_No]  DEFAULT ('') FOR [Lr_No]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Lr_Date]  DEFAULT ('') FOR [Lr_Date]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Total_Bags]  DEFAULT ((0)) FOR [Total_Bags]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Electronic_Reference_No]  DEFAULT ('') FOR [Electronic_Reference_No]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Transportation_Mode]  DEFAULT ('') FOR [Transportation_Mode]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Date_Time_Of_Supply]  DEFAULT ('') FOR [Date_Time_Of_Supply]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_CGst_Amount]  DEFAULT ((0)) FOR [CGst_Amount]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_SGst_Amount]  DEFAULT ((0)) FOR [SGst_Amount]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_IGst_Amount]  DEFAULT ((0)) FOR [IGst_Amount]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Receipt_Details]([Sales_Receipt_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Sales_Receipt_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Sales_Receipt_Date] [smalldatetime] NOT NULL,[Ledger_IdNo] [int] NOT NULL,[SL_No] [smallint] NOT NULL,[Item_IdNo] [int] NULL,[Style_IdNo] [int] NULL,[Size_IdNo] [int] NULL,[Unit_IdNo] [smallint] NULL,[Quantity] [numeric](18, 3) NULL,[Rate] [numeric](18, 2) NULL,[Amount] [numeric](18, 2) NULL,[Assessable_Value] [numeric](18, 2) NULL,[HSN_Code] [varchar](100) NULL,[Tax_Perc] [numeric](18, 2) NULL,[Item_Description] [varchar](500) NULL,[No_Of_Rolls] [int] NULL,[Delivery_Quantity] [numeric](18, 2) NULL,[Delivery_No_Of_Rolls] [int] NULL," & _
 "CONSTRAINT [PK_Sales_Receipt_Details] PRIMARY KEY CLUSTERED ([Sales_Receipt_Code] ASC,[SL_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Receipt_Details add Sales_Receipt_Detail_SlNo [int] IDENTITY (1, 1) NOT NULL"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Style_IdNo]  DEFAULT ((0)) FOR [Style_IdNo]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Size_IdNo]  DEFAULT ((0)) FOR [Size_IdNo]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Rate]  DEFAULT ((0)) FOR [Rate]"""
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Amount]  DEFAULT ((0)) FOR [Amount]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Assessable_Value]  DEFAULT ((0)) FOR [Assessable_Value]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Item_Description]  DEFAULT ('') FOR [Item_Description]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_HSN_Code]  DEFAULT ('') FOR [HSN_Code]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Delivery_Quantity]  DEFAULT ((0)) FOR [Delivery_Quantity]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_No_Of_Rolls]  DEFAULT ((0)) FOR [No_Of_Rolls]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE [Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Delivery_No_Of_Rolls]  DEFAULT ((0)) FOR [Delivery_No_Of_Rolls]"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "CREATE TABLE [Sales_Receipt_GST_Tax_Details](	[Sales_Receipt_Code] [varchar](50) NOT NULL,	[Company_Idno] [int] NOT NULL,	[Sales_Receipt_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Sales_Receipt_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[Sl_No] [int] NOT NULL,	[HSN_Code] [varchar](100) NULL,	[Taxable_Amount] [numeric](18, 2) NULL,	[CGST_Percentage] [numeric](18, 2) NULL,	[CGST_Amount] [numeric](18, 2) NULL,	[SGST_Percentage] [numeric](18, 2) NULL,	[SGST_Amount] [numeric](18, 2) NULL,	[IGST_Percentage] [numeric](18, 2) NULL,	[IGST_Amount] [numeric](18, 2) NULL," & _
                         " CONSTRAINT [PK_Sales_Receipt_GST_Tax_Details] PRIMARY KEY CLUSTERED (	[Sales_Receipt_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter table Sales_Head add Party_Dc_No varchar(200) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Party_Dc_No = '' where Party_Dc_No is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table SalesReturn_Details add Return_Qty NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Return_Qty = 0 where Return_Qty is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Sales_Code varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Sales_Code = '' where Sales_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Return_Qty NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Return_Qty = 0 where Return_Qty is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add CGST_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set CGST_Amount = 0 where CGST_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add SGST_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set SGST_Amount = 0 where SGST_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add IGST_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set IGST_Amount = 0 where IGST_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Return_GST_Tax_Details](	[Sales_Code] [varchar](50) NOT NULL,	[Company_Idno] [int] NOT NULL,	[Sales_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Sales_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[Sl_No] [int] NOT NULL,	[HSN_Code] [varchar](100) NULL,	[Taxable_Amount] [numeric](18, 2) NULL,	[CGST_Percentage] [numeric](18, 2) NULL,	[CGST_Amount] [numeric](18, 2) NULL,	[SGST_Percentage] [numeric](18, 2) NULL,	[SGST_Amount] [numeric](18, 2) NULL,	[IGST_Percentage] [numeric](18, 2) NULL,	[IGST_Amount] [numeric](18, 2) NULL," & _
                            " CONSTRAINT [PK_Sales_Return_GST_Tax_Details] PRIMARY KEY CLUSTERED (	[Sales_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table SalesReturn_Details add Cash_Discount_Perc_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Cash_Discount_Amount_For_All_Item = 0 where Cash_Discount_Amount_For_All_Item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Cash_Discount_Amount_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Cash_Discount_Amount_For_All_Item = 0 where Cash_Discount_Amount_For_All_Item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Assessable_Value NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Assessable_Value = 0 where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add HSN_Code VARCHAR(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set HSN_Code = '' where HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add GST_Percentage NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set GST_Percentage = 0 where GST_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Tax_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Tax_Amount = 0 where Tax_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add net_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set net_Amount = 0 where net_Amount is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add LessFor NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set LessFor = 0 where LessFor is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Entry_VAT_GST_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Entry_VAT_GST_Type ='' Where Entry_VAT_GST_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Tds_Percentage NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Tds_Percentage = 0 where Tds_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Tds_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Tds_Amount = 0 where Tds_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Net_Amount_Tds NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Net_Amount_Tds = 0 where Net_Amount_Tds is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add No_Of_Rolls NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set No_Of_Rolls = 0 where No_Of_Rolls is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Multi_Dc_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Multi_Dc_No = '' Where Multi_Dc_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Cash_Discount_Perc_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Cash_Discount_Perc_For_All_Item = 0 where Cash_Discount_Perc_For_All_Item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Cash_Discount_Amount_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Cash_Discount_Amount_For_All_Item = 0 where Cash_Discount_Amount_For_All_Item is Null"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter table Purchase_Head add Place_Of_Supply varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Place_Of_Supply = '' Where Place_Of_Supply is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Purchase_GST_Tax_Details](	[Purchase_Code] [varchar](50) NOT NULL,	[Company_Idno] [int] NOT NULL,	[Purchase_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Purchase_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL , [Sl_No] [int] NOT NULL,	[HSN_Code] [varchar](100) NULL,	[Taxable_Amount] [numeric](18, 2) NULL,	[CGST_Percentage] [numeric](18, 2) NULL,	[CGST_Amount] [numeric](18, 2) NULL,	[SGST_Percentage] [numeric](18, 2) NULL,	[SGST_Amount] [numeric](18, 2) NULL,	[IGST_Percentage] [numeric](18, 2) NULL,	[IGST_Amount] [numeric](18, 2) NULL, " & _
                            " CONSTRAINT [PK_Purchase_GST_Tax_Details] PRIMARY KEY CLUSTERED (	[Purchase_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Entry_VAT_GST_Type varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Entry_VAT_GST_Type = '' Where Entry_VAT_GST_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Electronic_Reference_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Electronic_Reference_No  = '' Where Electronic_Reference_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Transportation_Mode varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Transportation_Mode = '' Where Transportation_Mode is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Date_Time_Of_Supply varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Date_Time_Of_Supply = '' Where Date_Time_Of_Supply is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Entry_GST_Tax_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Entry_GST_Tax_Type = '' Where Entry_GST_Tax_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add CGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set CGst_Amount = 0 where CGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add SGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set SGst_Amount = 0 where SGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add IGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set IGst_Amount = 0 where IGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Assessable_Value NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Assessable_Value = 0 where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add HSN_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set HSN_Code = '' where HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add GST_Percentage  NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set GST_Percentage = 0 where GST_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Entry_Type varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Entry_Type = '' Where Assessable_Value is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Details add Sales_Order_Code varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Sales_Order_Code = '' Where Sales_Order_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Sales_Order_Detail_SlNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Sales_Order_Detail_SlNo = 0 Where Sales_Order_Detail_SlNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Details add Noof_Items numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Noof_Items = 0 Where Noof_Items is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Head add Assessable_Value numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Assessable_Value = 0 Where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add CGst_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set CGst_Amount = 0 Where CGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add SGst_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set SGst_Amount = 0 Where SGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add IGst_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set IGst_Amount = 0 Where IGst_Amount is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Head add SubTotal_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set SubTotal_Amount = 0 Where SubTotal_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Net_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Net_Amount = 0 Where Net_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Round_Off numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Round_Off = 0 Where Round_Off is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Entry_GST_Tax_Type varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Entry_GST_Tax_Type = '' Where SubTotal_Amount is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Details add HSN_Code varchar(50) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set HSN_Code = '' Where HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Tax_Perc numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Tax_Perc = 0 Where Tax_Perc is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Assessable_Value numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Assessable_Value = 0 Where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Size_Idno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Size_Idno = 0 Where Size_Idno is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Head add Total_Bags int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Total_Bags = 0 Where Total_Bags is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Electronic_Reference_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Electronic_Reference_No  = '' Where Electronic_Reference_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Transportation_Mode varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Transportation_Mode = '' Where Transportation_Mode is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Date_Time_Of_Supply varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Date_Time_Of_Supply = '' Where Date_Time_Of_Supply is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table  Sales_Delivery_Head add Weight numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Weight = 0 Where Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table  Sales_Delivery_Head add Freight_ToPay_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Freight_ToPay_Amount = 0 Where Freight_ToPay_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table  Sales_Delivery_Head add charge numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set charge = 0 Where charge is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Lr_Date varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Lr_Date = '' Where Lr_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Head add Lr_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Lr_No = '' Where Lr_No is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Delivery_Head add Booked_By varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Head set Booked_By = '' Where Booked_By is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Delivery_Details add Style_Idno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Delivery_Details set Style_Idno = 0 Where Style_Idno is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Style_Idno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Style_Idno = 0 Where Style_Idno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Style_Head]([Style_IdNo] [int] NOT NULL,	[Style_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL, " & _
          " CONSTRAINT [PK_Style_Head] PRIMARY KEY CLUSTERED ( [Style_IdNo]) ON [PRIMARY], CONSTRAINT [IX_Style_Head] UNIQUE NONCLUSTERED ( [Sur_Name]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table  Sales_Head add Pcs_or_Box varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Pcs_or_Box = '' Where Pcs_or_Box is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table  Sales_Head add charge numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set charge = 0 Where charge is Null"
        cmd.ExecuteNonQuery()


        Common_Procedures.Drop_Column_Default_Constraint(cn1, "Sales_Head", "Dc_No")
        cmd.CommandText = "Alter table Sales_Head alter column Dc_No varchar(100)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE Sales_Head Add CONSTRAINT DF_SalesHead_DcNo DEFAULT ('') for Dc_No"
        cmd.ExecuteNonQuery()


        Common_Procedures.Drop_Column_Default_Constraint(cn1, "Sales_Head", "Order_No")
        cmd.CommandText = "Alter table Sales_Head alter column Order_No varchar(100)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "ALTER TABLE Sales_Head Add CONSTRAINT DF_SalesHead_OrderNo DEFAULT ('') for Order_No"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add DeliveryTo_idNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set DeliveryTo_idNo = 0 Where DeliveryTo_idNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add Place_Of_Supply varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Place_Of_Supply = '' Where Place_Of_Supply is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add  Style_Name  varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set  Style_Name  = '' Where  Style_Name   is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add  Trade_Discount_Amount_For_All_Item   numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set  Trade_Discount_Amount_For_All_Item   = 0 Where  Trade_Discount_Amount_For_All_Item   is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Trade_Discount_Perc_For_All_Item   numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Trade_Discount_Perc_For_All_Item   = 0 Where Trade_Discount_Perc_For_All_Item   is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Head add Gst_Percentage numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Head set Gst_Percentage = 0 Where Gst_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Head add Gst_Rate numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Head set Gst_Rate = 0 Where Gst_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add CGst_Percentage numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set CGst_Percentage = 0 Where CGst_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add SGst_Percentage numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set SGst_Percentage = 0 Where SGst_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_GST_Tax_Details](	[Sales_Code] [varchar](50) NOT NULL,	[Company_Idno] [int] NOT NULL,	[Sales_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Sales_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL , [Sl_No] [int] NOT NULL,	[HSN_Code] [varchar](100) NULL,	[Taxable_Amount] [numeric](18, 2) NULL,	[CGST_Percentage] [numeric](18, 2) NULL,	[CGST_Amount] [numeric](18, 2) NULL,	[SGST_Percentage] [numeric](18, 2) NULL,	[SGST_Amount] [numeric](18, 2) NULL,	[IGST_Percentage] [numeric](18, 2) NULL,	[IGST_Amount] [numeric](18, 2) NULL, " & _
                            " CONSTRAINT [PK_Sales_GST_Tax_Details] PRIMARY KEY CLUSTERED (	[Sales_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Entry_VAT_GST_Type varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Entry_VAT_GST_Type = '' Where Entry_VAT_GST_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Electronic_Reference_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Electronic_Reference_No  = '' Where Electronic_Reference_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Transportation_Mode varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Transportation_Mode = '' Where Transportation_Mode is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Date_Time_Of_Supply varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Date_Time_Of_Supply = '' Where Date_Time_Of_Supply is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Entry_GST_Tax_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Entry_GST_Tax_Type = '' Where Entry_GST_Tax_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add CGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set CGst_Amount = 0 where CGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add SGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set SGst_Amount = 0 where SGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add IGst_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set IGst_Amount = 0 where IGst_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Assessable_Value NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Assessable_Value = 0 where Assessable_Value is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add HSN_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set HSN_Code = '' where HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add GST_Percentage  NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set GST_Percentage = 0 where GST_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Tax_Head] ( [Tax_IdNo] [int] NOT NULL, [Tax_Name] [varchar](50) NOT NULL, 	[Sur_Name] [varchar](50) NOT NULL, 	[Tax_Ledger_Ac_IdNo] [int] NULL CONSTRAINT [DF_Tax_Head_Tax_Ledger_Ac_IdNo]  DEFAULT ((0)),  CONSTRAINT [PK_Tax_Head] PRIMARY KEY CLUSTERED ( [Tax_IdNo] ) ON [PRIMARY],  CONSTRAINT [IX_Tax_Head] UNIQUE NONCLUSTERED ( [Sur_Name] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ItemGroup_Head add Item_HSN_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ItemGroup_Head set Item_HSN_Code = '' Where Item_HSN_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ItemGroup_Head add Item_GST_Percentage NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ItemGroup_Head set Item_GST_Percentage= 0 Where Item_GST_Percentage  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [State_Head](	[State_IdNo] [smallint] NOT NULL,	[State_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL,	[Cst_Value] [int] NOT NULL," & _
                            "CONSTRAINT [PK_State_Head] PRIMARY KEY CLUSTERED (  [State_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table State_Head add State_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update State_Head set State_Code  = '' Where State_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Company_Head add Company_Owner_Designation varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Company_Head set Company_Owner_Designation  = '' Where Company_Owner_Designation  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Company_Head add Company_Website varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Company_Head set Company_Website  = '' Where Company_Website  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Company_Head add Company_GSTinNo varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Company_Head set Company_GSTinNo  = '' Where Company_GSTinNo  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Company_Head add Company_State_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Company_Head set Company_State_IdNo  =0 Where Company_State_IdNo  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_Head add Ledger_GSTinNo varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ledger_Head set Ledger_GSTinNo  = '' Where Ledger_GSTinNo  is Null"
        cmd.ExecuteNonQuery()

        '========================================================================================

        cmd.CommandText = "Alter table SalesReturn_Details add Actual_Amount NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details  set Actual_Amount = 0 where Actual_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Actual_Rate NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details  set Actual_Rate = 0 where Actual_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Actual_Net_Amount NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head  set Actual_Net_Amount = 0 where Actual_Net_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Actual_Gross_Amount NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head  set Actual_Gross_Amount = 0 where Actual_Gross_Amount is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Actual_Amount NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Actual_Amount = 0 where Actual_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Actual_Rate NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Actual_Rate = 0 where Actual_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Actual_Net_Amount NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Actual_Net_Amount = 0 where Actual_Net_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Actual_Gross_Amount NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Actual_Gross_Amount = 0 where Actual_Gross_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Actual_Tax_Amount NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Actual_Tax_Amount = 0 where Actual_Tax_Amount is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Job_Card_Details add Item_IdNo Int"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Job_Card_Details  set Item_IdNo = 0 where Item_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Job_Card_Details add Quantity NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Job_Card_Details  set Quantity = 0 where Quantity is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Job_Card_Head add Item_IdNo Int"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Job_Card_Head  set Item_IdNo = 0 where Item_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Job_Card_Head add Quantity NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Job_Card_Head  set Quantity = 0 where Quantity is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Job_Card_Head add Total_Quantity NUMERIC(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Job_Card_Head  set Total_Quantity = 0 where Total_Quantity is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Order_Head add Order_Close Int"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Order_Head  set Order_Close = 0 where Order_Close is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Enquiry_No VARCHAR(100)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head  set Enquiry_No = '' where Enquiry_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Head add Enquiry_Date VARCHAR(100)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head  set Enquiry_Date = '' where Enquiry_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Freight_Charge NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Freight_Charge = 0 where Freight_Charge is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Freight_Charge_Name VARCHAR(50)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Freight_Charge_Name = 'Freight And Forwarding' where Freight_Charge_Name is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Enquiry_Details](	[Sales_Enquiry_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Sales_Enquiry_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Enquiry_Details_for_OrderBy]  DEFAULT ((0)),	[Sales_Enquiry_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Sales_Enquiry_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL CONSTRAINT [DF_Sales_Enquiry_Details_Item_IdNo]  DEFAULT ((0)),	[ItemGroup_IdNo] [int] NULL CONSTRAINT [DF_Sales_Enquiry_Details_ItemGroup_IdNo]  DEFAULT ((0)),	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Enquiry_Details_Unit_IdNo]  DEFAULT ((0)),	[Quantity] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Enquiry_Details_Quantity]  DEFAULT ((0)),	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Details_Rate]  DEFAULT ((0)),	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Details_Amount]  DEFAULT ((0)),	[Item_Description] [varchar](500) NULL CONSTRAINT [DF_Sales_Enquiry_Details_Item_Description]  DEFAULT (''),	[Sales_Enquiry_Detail_SlNo] [int]  NULL,	[Order_Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Details_Order_Quantity]  DEFAULT ((0))," & _
                          "CONSTRAINT [PK_Sales_Enquiry_Details] PRIMARY KEY CLUSTERED (	[Sales_Enquiry_No] ,	[SL_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Enquiry_Head](	[Sales_Enquiry_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Sales_Enquiry_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Enquiry_Head_for_OrderBy]  DEFAULT ((0)),	[Sales_Enquiry_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Sales_Enquiry_Head_Ledger_IdNo]  DEFAULT ((0)),	[Delivery_Terms] [varchar](50) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Delivery_Terms]  DEFAULT (''),	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Total_Qty]  DEFAULT ((0)),	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Gross_Amount]  DEFAULT ((0)),	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_CashDiscount_Perc]  DEFAULT ((0)),	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_CashDiscount_Amount]  DEFAULT ((0)),	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Assessable_Value]  DEFAULT ((0)),	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Tax_Perc]  DEFAULT ((0)),	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Tax_Amount]  DEFAULT ((0)),	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Freight_Amount]  DEFAULT ((0)),	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_AddLess_Amount]  DEFAULT ((0)),	[Labour_Charge] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Labour_Charge]  DEFAULT ((0)),	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Round_Off]  DEFAULT ((0)),	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Net_Amount]  DEFAULT ((0)),	[Payment_Terms] [varchar](100) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Payment_Terms]  DEFAULT (''),	[Tax_Type] [varchar](30) NULL CONSTRAINT [DF_Sales_Enquiry_Head_Tax_Type]  DEFAULT ('')," & _
                          "CONSTRAINT [PK_Sales_Enquiry_Head] PRIMARY KEY CLUSTERED ( [Sales_Enquiry_Code]) ON [PRIMARY]) ON [PRIMARY]"

        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Order_Selection_Code_Head](	[Reference_Code] [varchar](50) NOT NULL,	[Order_Selection_Code] [varchar](100) NULL, CONSTRAINT [PK_Order_Selection_Code_Head] PRIMARY KEY CLUSTERED ([Reference_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Head add Job_Work_Status Int"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Head  set Job_Work_Status = 0 where Job_Work_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [JobWork_Project_Head](	[JobWork_IdNo] [int] NOT NULL,	[JobWork_Name] [varchar](300) NULL,	[Sur_Name] [varchar](300) NULL,	[Description] [varchar](500) NULL," & _
                          " CONSTRAINT [PK_JobWork_Project_Head] PRIMARY KEY CLUSTERED ( [JobWork_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Advance_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Advance_Amount = 0 where Advance_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Receipt_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Receipt_Amount = 0 where Receipt_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Balance_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Balance_Amount = 0 where Balance_Amount is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add Delivery_Date VARCHAR(50)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Delivery_Date = '' where Delivery_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Received_Date VARCHAR(50)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Received_Date = '' where Received_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Job_Card_Details]([Job_Card_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Job_Card_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Job_Card_Date] [smalldatetime] NOT NULL,[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Job_Card_Details_Ledger_IdNo]  DEFAULT ((0)),[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Job_Card_Details_SL_No]  DEFAULT ((0)),[Item_IdNo] [int] NULL,[Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Job_Card_Details_Quantity]  DEFAULT ((0))," & _
" CONSTRAINT [PK_Job_Card_Details] PRIMARY KEY CLUSTERED ([Job_Card_Code] ASC,[SL_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Job_Card_Head]([Job_Card_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Job_Card_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Job_Card_Date] [smalldatetime] NOT NULL,[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Job_Card_Head_Ledger_IdNo]  DEFAULT ((0)),[Item_IdNo] [int] NULL CONSTRAINT [DF_Job_Card_Head_Item_IdNo]  DEFAULT ((0)),[Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Job_Card_Head_Quantity]  DEFAULT ((0)),[Total_Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Job_Card_Head_Total_Quantity]  DEFAULT ((0))," & _
 "CONSTRAINT [PK_Job_Card_Head] PRIMARY KEY CLUSTERED ([Job_Card_Code] Asc ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Voucher_Order_Details](	[Voucher_Code] [varchar](50) NOT NULL,	[For_OrderByCode] [numeric](18, 2) NOT NULL,	[Company_Idno] [int] NOT NULL,	[Voucher_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Voucher_Type] [varchar](20) NOT NULL,	[Voucher_Date] [smalldatetime] NOT NULL,	[Sl_No] [int] NOT NULL,	[Sales_Order_Selection_Code] [varchar](20) NOT NULL,	[Amount] [numeric](18, 2) NULL," & _
                           "  CONSTRAINT [PK_Voucher_Order_Details] PRIMARY KEY CLUSTERED (	[Voucher_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table SalesReturn_Head add Sales_Order_Selection_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Sales_Order_Selection_Code  = '' where Sales_Order_Selection_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Sales_Order_Selection_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Sales_Order_Selection_Code  = '' where Sales_Order_Selection_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Sales_Order_Selection_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Sales_Order_Selection_Code  = '' where Sales_Order_Selection_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Return_Head add Sales_Order_Selection_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Return_Head set Sales_Order_Selection_Code  = '' where Sales_Order_Selection_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Order_Head add Sales_Order_Selection_Code varchar(100) default ''"
        cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("Select * from Sales_Order_Head Where Sales_Order_Selection_Code is Null", cn1)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                cmd.CommandText = "update Sales_Order_Head set Sales_Order_Selection_Code ='" & Trim(Dt1.Rows(i).Item("Sales_Order_No").ToString) & "/" & Microsoft.VisualBasic.Right(Trim(Dt1.Rows(i).Item("Sales_Order_Code").ToString), 5) & "/" & Trim(Dt1.Rows(i).Item("Company_IdNo").ToString) & "' where  Sales_Order_Code = '" & Trim(Dt1.Rows(i).Item("Sales_Order_Code").ToString) & "'"
                Nr = cmd.ExecuteNonQuery()
            Next
        End If
        Dt1.Clear()

        cmd.CommandText = "Alter table Sales_Head add Delivery_Status int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Delivery_Status = 0 where Delivery_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Advance_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Advance_Amount = 0 where Advance_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Balance_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Balance_Amount = 0 where Balance_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Form_H_Status Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Form_H_Status = 0 where Form_H_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add ItemWise_DiscAmount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set ItemWise_DiscAmount = 0 where ItemWise_DiscAmount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_head add Ledger_PanNo varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ledger_head  set Ledger_PanNo = '' where Ledger_PanNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Company_Head add Company_PanNo varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Company_Head  set Company_PanNo = '' where Company_PanNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Dc_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Dc_No = '' where Dc_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Item_Stock_Selection_Processing_Details](	[Item_IdNo] [int] NOT NULL,	[Batch_No] [varchar](500) NOT NULL,	[Manufactured_Day] [int] NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Manufactured_Day]  DEFAULT ((0)),	[Manufactured_Month_IdNo] [int] NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Manufactured_Month_IdNo]  DEFAULT ((0)),	[Manufactured_Year] [int] NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Manufactured_Year]  DEFAULT ((0)),	[Manufactured_Date] [smalldatetime] NOT NULL,	[Expiry_Period_Days] [int] NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Expiry_Period_Days]  DEFAULT ((0)),	[Expiry_Day] [int] NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Expiray_Day]  DEFAULT ((0)),	[Expiry_Month_IdNo] [int] NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Expiray_Month_IdNo]  DEFAULT ((0)),	[Expiry_Year] [int] NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Expiray_Year]  DEFAULT ((0)),	[Expiry_Date] [smalldatetime] NOT NULL,	[Purchase_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Purchase_Rate]  DEFAULT ((0)),	[Mrp_Rate] [numeric](18, 2) NOT NULL,	[Sales_Rate] [numeric](18, 2) NULL,	[Inward_Quantity] [numeric](18, 2) NULL,	[OutWard_Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_OutWard_Quantity]  DEFAULT ((0))," & _
                          " CONSTRAINT [PK_Item_Stock_Selection_Processing_Details] PRIMARY KEY CLUSTERED (	[Item_IdNo] ,	[Batch_No] ,	[Manufactured_Date] ,	[Expiry_Date] ,	[Mrp_Rate] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Item_Stock_Selection_Processing_Details]  WITH CHECK ADD  CONSTRAINT [CK_Item_Stock_Selection_Processing_Details] CHECK  (([Inward_Quantity]>=(0)))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Item_Stock_Selection_Processing_Details] CHECK CONSTRAINT [CK_Item_Stock_Selection_Processing_Details]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Item_Stock_Selection_Processing_Details]  WITH CHECK ADD  CONSTRAINT [CK_Item_Stock_Selection_Processing_Details_1] CHECK  (([outward_Quantity]>=(0)))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Item_Stock_Selection_Processing_Details] CHECK CONSTRAINT [CK_Item_Stock_Selection_Processing_Details_1]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "ALTER TABLE [Item_Stock_Selection_Processing_Details]  WITH CHECK ADD  CONSTRAINT [CK_Item_Stock_Selection_Processing_Details_3] CHECK  (([Inward_Quantity]>=[Outward_Quantity]))"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Sales_Price NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Sales_Price = 0 where Sales_Price is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Discount_Amount_item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Discount_Amount_item = 0 where Discount_Amount_item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Rate_Tax NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Rate_Tax = 0 where Rate_Tax is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Total_DiscountAmount_item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Total_DiscountAmount_item = 0 where Total_DiscountAmount_item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Aessable_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Aessable_Amount = 0 where Aessable_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add AddLess_Name varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set AddLess_Name  = '' where AddLess_Name  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Freight_Name varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Freight_Name  = '' where Freight_Name  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Discount_Perc_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Discount_Perc_Item = 0 where Discount_Perc_Item is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Expiry_Month_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Expiry_Month_IdNo = 0 where Expiry_Month_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Manufacture_Month_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Manufacture_Month_IdNo = 0 where Manufacture_Month_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Manufacture_Day NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Manufacture_Day = 0 where Manufacture_Day is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Manufacture_Year NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Manufacture_Year = 0 where Manufacture_Year is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Expiry_Period_Days NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Expiry_Period_Days = 0 where Expiry_Period_Days is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Expiry_Day NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Expiry_Day = 0 where Expiry_Day is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Manufacture_Date smalldatetime"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table Sales_Details add Expiry_Date smalldatetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Expiry_Year NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Expiry_Year = 0 where Expiry_Year is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Batch_Serial_No varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Batch_Serial_No  = '' where Batch_Serial_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Received_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Received_Amount = 0 where Received_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Tax_Perc2 NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Tax_Perc2 = 0 where Tax_Perc2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Tax_Amount2 NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Tax_Amount2 = 0 where Tax_Amount2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Cash_Discount_Perc_For_All_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Cash_Discount_Perc_For_All_Item  = 0 where Cash_Discount_Perc_For_All_Item  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Cash_Discount_Amount_For_All_Item  NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Cash_Discount_Amount_For_All_Item   = 0 where Cash_Discount_Amount_For_All_Item   is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Balance_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Balance_Amount = 0 where Balance_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_BatchNo_Details add Detail_SlNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_BatchNo_Details  set Detail_SlNo = 0 where Detail_SlNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Detail_SlNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Detail_SlNo = 0 where Detail_SlNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Tax_Details add Tax_Perc NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Tax_Details  set Tax_Perc = 0 where Tax_Perc is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Tax_Details add Tax_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Tax_Details  set Tax_Amount = 0 where Tax_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Owner_Name varchar(200) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Owner_Name = '' Where Owner_Name is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Printing_Order_Details add Order_No_New varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Printing_Order_Details set Order_No_New = '' Where Order_No_New is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Purchase_BatchNo_Details]([Purchase_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Purchase_No] [varchar](20) NOT NULL CONSTRAINT [DF_Purchase_BatchNo_Details_Purchase_No]  DEFAULT (''),	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Purchase_BatchNo_Details_for_OrderBy]  DEFAULT ((0)),	[Purchase_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Purchase_BatchNo_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL,	[Batch_No] [varchar](100) NULL CONSTRAINT [DF_Purchase_BatchNo_Details_Batch_Serial_No]  DEFAULT (''),	[Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_BatchNo_Details_Quantity]  DEFAULT ((0)),	[Item_idNo] [int] NULL CONSTRAINT [DF_Purchase_BatchNo_Details_Item_idNo]  DEFAULT ((0)),	[Detail_SlNo] [int] NULL DEFAULT ((0))," & _
                           "  CONSTRAINT [PK_Purchase_BatchNo_Details] PRIMARY KEY CLUSTERED (	[Purchase_Code] ,	[SL_No] ) ON [PRIMARY]," & _
                           " CONSTRAINT [IX_Purchase_BatchNo_Details_1] UNIQUE NONCLUSTERED (	[Purchase_Code] ,	[Detail_SlNo],[Batch_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = " CREATE TABLE [Purchase_Tax_Details]([Purchase_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Purchase_No] [varchar](20) NOT NULL CONSTRAINT [DF_Purchase_Tax_Details_Purchase_No]  DEFAULT (''),[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Purchase_Tax_Details_for_OrderBy]  DEFAULT ((0)),[Purchase_Date] [smalldatetime] NOT NULL,[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Purchase_Tax_Details_Ledger_IdNo]  DEFAULT ((0)),[SL_No] [smallint] NOT NULL,[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Batch_No]  DEFAULT ((0)),[Discount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Quantity]  DEFAULT ((0)),[Aessable_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Tax_Details_Aessable_Amount]  DEFAULT ((0)),[Tax_Pec] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Tax_Details_Tax_Pec]  DEFAULT ((0)),[Item_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Tax_Details_Item_IdNo]  DEFAULT ((0))," & _
                          " CONSTRAINT [PK_Purchase_Tax_Details] PRIMARY KEY CLUSTERED ([Purchase_Code] ASC,[SL_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Total_DiscountAmount_item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head  set Total_DiscountAmount_item = 0 where Total_DiscountAmount_item is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Aessable_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head  set Aessable_Amount = 0 where Aessable_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add AddLess_Name varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set AddLess_Name  = '' where AddLess_Name  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Freight_Name varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Freight_Name  = '' where Freight_Name  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Discount_Perc_Item NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Discount_Perc_Item = 0 where Discount_Perc_Item is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Purchase_Details add Item_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Item_Code  = '' where Item_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Batch_Serial_No varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Batch_Serial_No  = '' where Batch_Serial_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Manufacture_Day NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Manufacture_Day = 0 where Manufacture_Day is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Manufacture_Year NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Manufacture_Year = 0 where Manufacture_Year is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Expiry_Period_Days NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Expiry_Period_Days = 0 where Expiry_Period_Days is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Expiry_Day NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Expiry_Day = 0 where Expiry_Day is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Manufacture_Date smalldatetime"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table Purchase_Details add Expiry_Date smalldatetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Expiry_Year NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Expiry_Year = 0 where Expiry_Year is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Mrp NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Mrp = 0 where Mrp is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Sales_Price NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Sales_Price = 0 where Sales_Price is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Expiry_Month_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Expiry_Month_IdNo = 0 where Expiry_Month_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Manufacture_Month_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details  set Manufacture_Month_IdNo = 0 where Manufacture_Month_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Quotation_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Quotation_Details_2] CHECK  (([Quantity]>=[Order_Quantity]))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Quotation_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Quotation_Details_1] CHECK  (([Order_Quantity]>=(0)))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add Order_Quantity NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Details  set Order_Quantity = 0 where Order_Quantity is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Quotation_Details add Sales_Quotation_Detail_SlNo [int] IDENTITY (1, 1) NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Order_Details add Sales_Quotation_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Order_Details set Sales_Quotation_Code  = '' where Sales_Quotation_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Order_Details add Sales_Quotation_Detail_SlNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Order_Details set Sales_Quotation_Detail_SlNo = 0 where Sales_Quotation_Detail_SlNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Order_Details add item_Description varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Order_Details set item_Description = '' Where item_Description is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Order_Details add Entry_Type varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Order_Details set Entry_Type = '' Where Entry_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Order_Head add Quotation_No varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Order_Head set Quotation_No = '' Where Quotation_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Order_Head add Quotation_Date varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Order_Head set Quotation_Date = '' Where Quotation_Date is Null"
        cmd.ExecuteNonQuery()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1003" Then
            cmd.CommandText = "Update Sales_Head set Tax_Type = 'VAT' Where Tax_Type is Null"
            cmd.ExecuteNonQuery()
        End If

        cmd.CommandText = "Alter table Sales_Quotation_Head add Tax_Type varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Quotation_Head set Tax_Type = '' Where Tax_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Printing_Order_Head add Advance_Date varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Printing_Order_Head set Advance_Date = '' Where Advance_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Printing_Invoice_Details alter column Printing_Invoice_slno int"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add MRP_Rate NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set MRP_Rate = 0 where MRP_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add MRP_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set MRP_Amount = 0 where MRP_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Printing_Order_Details add Cancel_Status tinyint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Printing_Order_Details  set Cancel_Status = 0 where Cancel_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Order_Paper_Details](	[Printing_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Order_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Order_Date] [smalldatetime] NOT NULL,	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Printing_Order_Paper_Details_SL_No]  DEFAULT ((0)),	[Paper_IdNo] [smallint] NULL CONSTRAINT [DF_Printing_Order_Paper_Details_Paper_IdNo]  DEFAULT ((0)),	[Detail_SlNo] [int] NOT NULL CONSTRAINT [DF_Printing_Order_Paper_Details_Detail_SlNo]  DEFAULT ((0)), CONSTRAINT [PK_Printing_Order_Paper_Details] PRIMARY KEY CLUSTERED (	[Printing_Order_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Order_Head](	[Printing_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Order_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Order_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Printing_Order_Head_Ledger_IdNo]  DEFAULT ((0)),	[advance] [numeric](18, 2) NULL CONSTRAINT [DF_Printing_Order_Head_advance]  DEFAULT ((0)),	[remarks] [varchar](500) NULL CONSTRAINT [DF_Printing_Order_Head_remarks]  DEFAULT (''), CONSTRAINT [PK_Printing_Order_Head] PRIMARY KEY CLUSTERED (        [Printing_Order_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Order_Details](	[Printing_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Order_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Order_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Printing_Order_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Printing_Order_Details_SL_No]  DEFAULT ((0)),	[Variety_IdNo] [int] NULL,	[Colour_Details] [varchar](250) NULL CONSTRAINT [DF_Printing_Order_Details_Colour_IdNo]  DEFAULT (''),	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Printing_Order_Details_Unit_IdNo]  DEFAULT ((0)),	[Size_IdNo] [smallint] NULL CONSTRAINT [DF_Printing_Order_Details_Size_IdNo]  DEFAULT ((0)),	[Paper_Details] [varchar](250) NULL CONSTRAINT [DF_Printing_Order_Details_Paper_IdNo]  DEFAULT (''),	[Order_no] [varchar](50) NULL CONSTRAINT [DF_Printing_Order_Details_Order_no]  DEFAULT (''),	[Binding_No] [varchar](50) NULL CONSTRAINT [DF_Printing_Order_Details_Binding_No]  DEFAULT (''),	[Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Printing_Order_Details_Quantity]  DEFAULT ((0)),	[NO_of_SET] [varchar](50) NOT NULL CONSTRAINT [DF_Printing_Order_Details_NO_of_SET]  DEFAULT (''),	[No_Of_Copies] [varchar](50) NOT NULL CONSTRAINT [DF_Printing_Order_Details_No_Of_Copies]  DEFAULT (''),	[Printing_Order_Details_SlNo] [int] IDENTITY(1,1) NOT NULL,	[Order_Program_Code] [varchar](50) NULL CONSTRAINT [DF_Printing_Order_Details_Order_Program_Code]  DEFAULT (''),	[Order_Program_Increment] [int] NULL,	[Details_SlNo] [int] NULL CONSTRAINT [DF_Printing_Order_Details_Details_SlNo]  DEFAULT ((0)), CONSTRAINT [PK_Printing_Order_Details] PRIMARY KEY CLUSTERED (	[Printing_Order_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Order_colour_Details](	[Printing_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Order_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Order_Date] [smalldatetime] NOT NULL,	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Printing_Order_colour_Details_SL_No]  DEFAULT ((0)),	[Colour_IdNo] [smallint] NULL CONSTRAINT [DF_Printing_Order_colour_Details_Colour_IdNo]  DEFAULT ((0)),	[Detail_SlNo] [int] NOT NULL CONSTRAINT [DF_Printing_Order_colour_Details_Detail_SlNo]  DEFAULT ((0)), CONSTRAINT [PK_Printing_Order_colour_Details] PRIMARY KEY CLUSTERED (	[Printing_Order_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Invoice_Head](	[Printing_Invoice_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Invoice_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Invoice_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Printing_Invoice_Head_Ledger_IdNo]  DEFAULT ((0)),	[Assesable_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Order_No]  DEFAULT ((0)),	[Other_Charges] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Assesable_Amount2]  DEFAULT ((0)),	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Assesable_Amount1]  DEFAULT ((0)),	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Net_Amount1]  DEFAULT ((0)), CONSTRAINT [PK_Printing_Invoice_Head] PRIMARY KEY CLUSTERED (        [Printing_Invoice_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Invoice_Details](	[Printing_Invoice_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Invoice_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Invoice_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_SL_No]  DEFAULT ((0)),	[Variety_IdNo] [int] NOT NULL,	[Unit_IdNo] [smallint] NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_Unit_IdNo]  DEFAULT ((0)),	[Quantity] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_Quantity]  DEFAULT ((0)),	[Amount] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Table_1_Net_Amount]  DEFAULT ((0)),	[Printing_Invoice_slno] [int] IDENTITY(1,1) NOT NULL,	[Order_Program_Code] [varchar](50) NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_Order_Program_Code]  DEFAULT (''),	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Printing_Invoice_Details_Order_No]  DEFAULT (''), CONSTRAINT [PK_Printing_Invoice_Details] PRIMARY KEY CLUSTERED (	[Printing_Invoice_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Order_Program_Head](	[Order_Program_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Order_Program_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Order_Program_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Order_Program_Head_Ledger_IdNo]  DEFAULT ((0)),	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Table_2_advance]  DEFAULT (''),	[remarks] [varchar](500) NULL CONSTRAINT [DF_Order_Program_Head_remarks]  DEFAULT (''),	[Printing_Order_Code] [varchar](50) NOT NULL,	[Printing_Order_Details_SlNo] [int] NOT NULL,	[Printing_Invoice_Code] [varchar](50) NOT NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_Code]  DEFAULT (''),	[Printing_Invoice_slno] [tinyint] NOT NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_slno]  DEFAULT ((0)),	[Printing_Invoice_Increment] [int] NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_Increment]  DEFAULT ((0)), CONSTRAINT [PK_Order_Program_Head] PRIMARY KEY CLUSTERED (        [Order_Program_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Order_Program_Details](	[Order_Program_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Order_Program_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Order_Program_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Order_Program_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Order_Program_Details_SL_No]  DEFAULT ((0)),	[Variety_IdNo] [int] NULL,	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Order_Program_Details_Unit_IdNo]  DEFAULT ((0)),	[Size_IdNo] [smallint] NULL CONSTRAINT [DF_Order_Program_Details_Size_IdNo]  DEFAULT ((0)),	[Binding_No] [varchar](50) NULL CONSTRAINT [DF_Order_Program_Details_Binding_No]  DEFAULT (''),	[Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Order_Program_Details_Quantity]  DEFAULT ((0)),	[NO_of_SET] [varchar](50) NOT NULL CONSTRAINT [DF_Order_Program_Details_NO_of_SET]  DEFAULT (''),	[No_Of_Copies] [varchar](50) NOT NULL CONSTRAINT [DF_Order_Program_Details_No_Of_Copies]  DEFAULT (''),	[Colour_Details] [varchar](250) NULL CONSTRAINT [DF_Order_Program_Details_Colour_Details]  DEFAULT (''),	[Paper_Details] [varchar](250) NULL CONSTRAINT [DF_Order_Program_Details_Paper_Details]  DEFAULT (''), CONSTRAINT [PK_Order_Program_Details] PRIMARY KEY CLUSTERED (	[Order_Program_Code] ,        [SL_No])ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Head add MRP_Rate NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Head  set MRP_Rate = 0 where MRP_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ReportTemp add Name11 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ReportTemp set Name11 = '' Where Name11 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Printing_Order_Details add Order_No_New varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Printing_Order_Details set Order_No_New = '' Where Order_No_New is Null"
        cmd.ExecuteNonQuery()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1003" Then
            cmd.CommandText = "Update Sales_Head set Tax_Type = 'VAT' Where Tax_Type is Null"
            cmd.ExecuteNonQuery()
        End If

        cmd.CommandText = "Alter table Printing_Order_Head add Advance_Date varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Printing_Order_Head set Advance_Date = '' Where Advance_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Printing_Invoice_Details alter column Printing_Invoice_slno int"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add MRP_Rate NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set MRP_Rate = 0 where MRP_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add MRP_Amount NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set MRP_Amount = 0 where MRP_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Printing_Order_Details add Cancel_Status tinyint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Printing_Order_Details  set Cancel_Status = 0 where Cancel_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Order_Paper_Details](	[Printing_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Order_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Order_Date] [smalldatetime] NOT NULL,	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Printing_Order_Paper_Details_SL_No]  DEFAULT ((0)),	[Paper_IdNo] [smallint] NULL CONSTRAINT [DF_Printing_Order_Paper_Details_Paper_IdNo]  DEFAULT ((0)),	[Detail_SlNo] [int] NOT NULL CONSTRAINT [DF_Printing_Order_Paper_Details_Detail_SlNo]  DEFAULT ((0)), CONSTRAINT [PK_Printing_Order_Paper_Details] PRIMARY KEY CLUSTERED (	[Printing_Order_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Order_Head](	[Printing_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Order_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Order_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Printing_Order_Head_Ledger_IdNo]  DEFAULT ((0)),	[advance] [numeric](18, 2) NULL CONSTRAINT [DF_Printing_Order_Head_advance]  DEFAULT ((0)),	[remarks] [varchar](500) NULL CONSTRAINT [DF_Printing_Order_Head_remarks]  DEFAULT (''), CONSTRAINT [PK_Printing_Order_Head] PRIMARY KEY CLUSTERED (        [Printing_Order_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Order_Details](	[Printing_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Order_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Order_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Printing_Order_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Printing_Order_Details_SL_No]  DEFAULT ((0)),	[Variety_IdNo] [int] NULL,	[Colour_Details] [varchar](250) NULL CONSTRAINT [DF_Printing_Order_Details_Colour_IdNo]  DEFAULT (''),	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Printing_Order_Details_Unit_IdNo]  DEFAULT ((0)),	[Size_IdNo] [smallint] NULL CONSTRAINT [DF_Printing_Order_Details_Size_IdNo]  DEFAULT ((0)),	[Paper_Details] [varchar](250) NULL CONSTRAINT [DF_Printing_Order_Details_Paper_IdNo]  DEFAULT (''),	[Order_no] [varchar](50) NULL CONSTRAINT [DF_Printing_Order_Details_Order_no]  DEFAULT (''),	[Binding_No] [varchar](50) NULL CONSTRAINT [DF_Printing_Order_Details_Binding_No]  DEFAULT (''),	[Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Printing_Order_Details_Quantity]  DEFAULT ((0)),	[NO_of_SET] [varchar](50) NOT NULL CONSTRAINT [DF_Printing_Order_Details_NO_of_SET]  DEFAULT (''),	[No_Of_Copies] [varchar](50) NOT NULL CONSTRAINT [DF_Printing_Order_Details_No_Of_Copies]  DEFAULT (''),	[Printing_Order_Details_SlNo] [int] IDENTITY(1,1) NOT NULL,	[Order_Program_Code] [varchar](50) NULL CONSTRAINT [DF_Printing_Order_Details_Order_Program_Code]  DEFAULT (''),	[Order_Program_Increment] [int] NULL,	[Details_SlNo] [int] NULL CONSTRAINT [DF_Printing_Order_Details_Details_SlNo]  DEFAULT ((0)), CONSTRAINT [PK_Printing_Order_Details] PRIMARY KEY CLUSTERED (	[Printing_Order_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Order_colour_Details](	[Printing_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Order_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Order_Date] [smalldatetime] NOT NULL,	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Printing_Order_colour_Details_SL_No]  DEFAULT ((0)),	[Colour_IdNo] [smallint] NULL CONSTRAINT [DF_Printing_Order_colour_Details_Colour_IdNo]  DEFAULT ((0)),	[Detail_SlNo] [int] NOT NULL CONSTRAINT [DF_Printing_Order_colour_Details_Detail_SlNo]  DEFAULT ((0)), CONSTRAINT [PK_Printing_Order_colour_Details] PRIMARY KEY CLUSTERED (	[Printing_Order_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Invoice_Head](	[Printing_Invoice_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Invoice_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Invoice_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Printing_Invoice_Head_Ledger_IdNo]  DEFAULT ((0)),	[Assesable_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Order_No]  DEFAULT ((0)),	[Other_Charges] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Assesable_Amount2]  DEFAULT ((0)),	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Assesable_Amount1]  DEFAULT ((0)),	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Net_Amount1]  DEFAULT ((0)), CONSTRAINT [PK_Printing_Invoice_Head] PRIMARY KEY CLUSTERED (        [Printing_Invoice_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Printing_Invoice_Details](	[Printing_Invoice_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Printing_Invoice_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Printing_Invoice_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_SL_No]  DEFAULT ((0)),	[Variety_IdNo] [int] NOT NULL,	[Unit_IdNo] [smallint] NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_Unit_IdNo]  DEFAULT ((0)),	[Quantity] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_Quantity]  DEFAULT ((0)),	[Amount] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Table_1_Net_Amount]  DEFAULT ((0)),	[Printing_Invoice_slno] [int] IDENTITY(1,1) NOT NULL,	[Order_Program_Code] [varchar](50) NOT NULL CONSTRAINT [DF_Printing_Invoice_Details_Order_Program_Code]  DEFAULT (''),	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Printing_Invoice_Details_Order_No]  DEFAULT (''), CONSTRAINT [PK_Printing_Invoice_Details] PRIMARY KEY CLUSTERED (	[Printing_Invoice_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Order_Program_Head](	[Order_Program_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Order_Program_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Order_Program_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Order_Program_Head_Ledger_IdNo]  DEFAULT ((0)),	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Table_2_advance]  DEFAULT (''),	[remarks] [varchar](500) NULL CONSTRAINT [DF_Order_Program_Head_remarks]  DEFAULT (''),	[Printing_Order_Code] [varchar](50) NOT NULL,	[Printing_Order_Details_SlNo] [int] NOT NULL,	[Printing_Invoice_Code] [varchar](50) NOT NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_Code]  DEFAULT (''),	[Printing_Invoice_slno] [tinyint] NOT NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_slno]  DEFAULT ((0)),	[Printing_Invoice_Increment] [int] NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_Increment]  DEFAULT ((0)), CONSTRAINT [PK_Order_Program_Head] PRIMARY KEY CLUSTERED (        [Order_Program_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Order_Program_Details](	[Order_Program_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Order_Program_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Order_Program_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Order_Program_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL CONSTRAINT [DF_Order_Program_Details_SL_No]  DEFAULT ((0)),	[Variety_IdNo] [int] NULL,	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Order_Program_Details_Unit_IdNo]  DEFAULT ((0)),	[Size_IdNo] [smallint] NULL CONSTRAINT [DF_Order_Program_Details_Size_IdNo]  DEFAULT ((0)),	[Binding_No] [varchar](50) NULL CONSTRAINT [DF_Order_Program_Details_Binding_No]  DEFAULT (''),	[Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Order_Program_Details_Quantity]  DEFAULT ((0)),	[NO_of_SET] [varchar](50) NOT NULL CONSTRAINT [DF_Order_Program_Details_NO_of_SET]  DEFAULT (''),	[No_Of_Copies] [varchar](50) NOT NULL CONSTRAINT [DF_Order_Program_Details_No_Of_Copies]  DEFAULT (''),	[Colour_Details] [varchar](250) NULL CONSTRAINT [DF_Order_Program_Details_Colour_Details]  DEFAULT (''),	[Paper_Details] [varchar](250) NULL CONSTRAINT [DF_Order_Program_Details_Paper_Details]  DEFAULT (''), CONSTRAINT [PK_Order_Program_Details] PRIMARY KEY CLUSTERED (	[Order_Program_Code] ,        [SL_No])ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Head add MRP_Rate NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Head  set MRP_Rate = 0 where MRP_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ReportTemp add Name11 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ReportTemp set Name11 = '' Where Name11 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ReportTemp add Name12 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ReportTemp set Name12 = '' Where Name12 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ReportTemp add Name13 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ReportTemp set Name13 = '' Where Name13 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ReportTemp add Name14 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ReportTemp set Name14 = '' Where Name14 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ReportTemp add Name15 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ReportTemp set Name15 = '' Where Name15 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_head add Pan_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ledger_head set Pan_No = '' Where Pan_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_head add Birth_Date smalldatetime"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ledger_head add Wedding_Date smalldatetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add RateWithTax NUMERIC(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set RateWithTax = 0 where RateWithTax is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Item_Description varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Item_Description  = '' where Item_Description  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Sales_Delivery_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Sales_Delivery_Code  = '' where Sales_Delivery_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Sales_Delivery_Detail_SlNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Sales_Delivery_Detail_SlNo = 0 where Sales_Delivery_Detail_SlNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Sales_Delivery_Head] ( [Sales_Delivery_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Sales_Delivery_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Delivery_Head_for_OrderBy]  DEFAULT ((0)),	[Sales_Delivery_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Sales_Delivery_Head_Ledger_IdNo]  DEFAULT ((0)),	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Delivery_Head_Delivery_Terms]  DEFAULT (''),	[Order_Date] [varchar](50) NULL CONSTRAINT [DF_Sales_Delivery_Head_Order_Date]  DEFAULT (''),[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Delivery_Head_Total_Qty]  DEFAULT ((0)),[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Delivery_Head_Gross_Amount]  DEFAULT ((0)),	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Delivery_Head_Payment_Terms]  DEFAULT (''),[Transport_IdNo] [int] NULL CONSTRAINT [DF_Sales_Delivery_Head_Transport_IdNo]  DEFAULT ((0)),[Remarks] [varchar](500) NULL CONSTRAINT [DF_Sales_Delivery_Head_Remarks]  DEFAULT ('')," & _
                            "CONSTRAINT [PK_Sales_Delivery_Head] PRIMARY KEY CLUSTERED ([Sales_Delivery_Code] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Delivery_Details] ( [Sales_Delivery_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Sales_Delivery_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Delivery_Details_for_OrderBy]  DEFAULT ((0)),	[Sales_Delivery_Date] [smalldatetime] NOT NULL,[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Sales_Delivery_Details_Ledger_IdNo]  DEFAULT ((0)),[SL_No] [smallint] NOT NULL,[Item_IdNo] [int] NULL CONSTRAINT [DF_Sales_Delivery_Details_Item_IdNo]  DEFAULT ((0)),[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Delivery_Details_Unit_IdNo]  DEFAULT ((0)),[Quantity] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Delivery_Details_Quantity]  DEFAULT ((0)),[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Delivery_Details_Rate]  DEFAULT ((0)),[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Delivery_Details_Amount]  DEFAULT ((0)),[Item_Description] [varchar](500) NULL CONSTRAINT [DF_Sales_Delivery_Details_Item_Description]  DEFAULT (''),[Sales_delivery_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,[Receipt_Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Delivery_Details_Receipt_Quantity]  DEFAULT ((0))," & _
                            "CONSTRAINT [PK_Sales_Delivery_Details] PRIMARY KEY CLUSTERED ([Sales_Delivery_Code] , [SL_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Delivery_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Delivery_Details_2] CHECK  (([Quantity]>=[Receipt_Quantity]))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [Sales_Delivery_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Delivery_Details_1] CHECK  (([Receipt_Quantity]>=(0)))"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Quotation_Head] ( [Sales_Quotation_Code] [varchar](50) NOT NULL, 	[Company_IdNo] [smallint] NOT NULL, [Sales_Quotation_No] [varchar](50) NOT NULL, [for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Quotation_Head_for_OrderBy]  DEFAULT ((0)), 	[Sales_Quotation_Date] [smalldatetime] NOT NULL, 	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Sales_Quotation_Head_Ledger_IdNo]  DEFAULT ((0)), 	[Delivery_Terms] [varchar](50) NULL CONSTRAINT [DF_Sales_Quotation_Head_Delivery_Terms]  DEFAULT (''), 	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Quotation_Head_Total_Qty]  DEFAULT ((0)), 	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Gross_Amount]  DEFAULT ((0)), " & _
                         " [CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_CashDiscount_Perc]  DEFAULT ((0)), 	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_CashDiscount_Amount]  DEFAULT ((0)), 	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Assessable_Value]  DEFAULT ((0)), 	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Tax_Perc]  DEFAULT ((0)), 	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Tax_Amount]  DEFAULT ((0)), 	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Freight_Amount]  DEFAULT ((0)), 	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_AddLess_Amount]  DEFAULT ((0)), 	[Labour_Charge] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Labour_Charge]  DEFAULT ((0)), " & _
                         " [Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Round_Off]  DEFAULT ((0)), 	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Net_Amount]  DEFAULT ((0)), 	[Payment_Terms] [varchar](100) NULL CONSTRAINT [DF_Sales_Quotation_Head_PaymentTerms]  DEFAULT (''),  CONSTRAINT [PK_Sales_Quotation_Head] PRIMARY KEY CLUSTERED  ( [Sales_Quotation_Code] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Sales_Quotation_Details] ( [Sales_Quotation_Code] [varchar](50) NOT NULL, 	[Company_IdNo] [smallint] NOT NULL, 	[Sales_Quotation_No] [varchar](20) NOT NULL, 	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Quotation_Details_for_OrderBy]  DEFAULT ((0)), 	[Sales_Quotation_Date] [smalldatetime] NOT NULL, 	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Sales_Quotation_Details_Ledger_IdNo]  DEFAULT ((0)), 	[SL_No] [smallint] NOT NULL, 	[Item_IdNo] [int] NULL CONSTRAINT [DF_Sales_Quotation_Details_Item_IdNo]  DEFAULT ((0)), 	[ItemGroup_IdNo] [int] NULL CONSTRAINT [DF_Sales_Quotation_Details_ItemGroup_IdNo]  DEFAULT ((0)), 	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Quotation_Details_Unit_IdNo]  DEFAULT ((0)), " & _
                         " [Quantity] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Quotation_Details_Noof_Items]  DEFAULT ((0)), 	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Details_Rate]  DEFAULT ((0)), 	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Details_Total_Amount1]  DEFAULT ((0)), 	[Item_Description] [varchar](500) NULL CONSTRAINT [DF_Sales_Quotation_Details_Item_Description]  DEFAULT (''), 	[Sales_Quotation_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,  CONSTRAINT [PK_Sales_Quotation_Details] PRIMARY KEY CLUSTERED  ( 	[Sales_Quotation_Code] ,  [SL_No] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add Total_FreeQty NUMERIC(18, 3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Total_FreeQty = 0 where Total_FreeQty is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Ledger_item_Details] ( [Ledger_Item_Code] [varchar](50) NOT NULL, [Ledger_Item_No] [varchar](50) NOT NULL, [Company_IdNo] [int] NOT NULL, [For_OrderBy] [numeric](18, 2) NOT NULL, 	[Sl_No] [smallint] NOT NULL, [Ledger_IdNo] [smallint] NULL, [Item_IdNo] [int] NULL, [Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Ledger_item_Details_Quantity]  DEFAULT ((0)),  CONSTRAINT [PK_Ledger_item_Details] PRIMARY KEY CLUSTERED  ( [Ledger_Item_Code] , [Sl_No] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Ledger_Item_Head] ( [Ledger_Item_Code] [varchar](50) NOT NULL, [Company_IdNo] [smallint] NOT NULL, [Ledger_Item_No] [varchar](20) NOT NULL, 	[For_OrderBy] [numeric](9, 2) NULL, [Ledger_IdNo] [int] NULL, [Total_Quantity] [numeric](18, 3) NULL,  CONSTRAINT [PK_Ledger_Item_Head] PRIMARY KEY CLUSTERED  ( [Ledger_Item_Code] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Area_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Area_IdNo = 0 where Area_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Agent_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Agent_IdNo = 0 where Agent_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Agent_idno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Agent_idno = 0 where Agent_idno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Total_Extra_Quantity int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Total_Extra_Quantity = 0 where Total_Extra_Quantity is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Details add Extra_Quantity int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Extra_Quantity = 0 where Extra_Quantity is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_AlaisHead add Agent_idNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_AlaisHead set Agent_idNo = 0 where Agent_idNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_head add Agent_idNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ledger_head set Agent_idNo = 0 where Agent_idNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Free_Item_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Free_Item_IdNo = 0 where Free_Item_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Free_Qty int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Free_Qty = 0 where Free_Qty is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Free_Qty int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Free_Qty = 0 where Free_Qty is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Discount_Head]([Sales_Discount_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Sales_Discount_No] [varchar](50) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Discount_Head_for_OrderBy]  DEFAULT ((0)),[Sales_Discount_Date] [datetime] NOT NULL,[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Sales_Discount_Head_Ledger_IdNo]  DEFAULT ((0)),[SalesAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Discount_Head_SalesAc_IdNo]  DEFAULT ((0)),[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_Sales_Discount_Head_Tax_Type]  DEFAULT (''),[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Discount_Head_TaxAc_IdNo]  DEFAULT ((0)),[Narration] [varchar](500) NULL CONSTRAINT [DF_Sales_Discount_Head_Narration]  DEFAULT (''),[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Discount_Head_Total_Qty]  DEFAULT ((0)),[SubTotal_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Head_SubTotal_Amount]  DEFAULT ((0)),[Total_DiscountAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Head_Total_DiscountAmount]  DEFAULT ((0)),[Total_TaxAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Head_Total_TaxAmount]  DEFAULT ((0)),[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Head_Tax_Perc]  DEFAULT ((0)),[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Head_AddLess_Amount]  DEFAULT ((0)),[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Head_Round_Off]  DEFAULT ((0)),[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Head_Net_Amount]  DEFAULT ((0)),[Selection_Type] [varchar](50) NULL CONSTRAINT [DF_Sales_Discount_Head_Selection_Type]  DEFAULT (''),[Agent_idno] [int] NULL CONSTRAINT [DF_Sales_Discount_Head_Agent_idno]  DEFAULT ((0))," & _
                        "CONSTRAINT [PK_Sales_Discount_Head] PRIMARY KEY CLUSTERED ([Sales_Discount_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Sales_Discount_Details](	[Sales_Discount_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Sales_Discount_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Discount_Details_for_OrderBy]  DEFAULT ((0)),	[Sales_Discount_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Sales_Discount_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL CONSTRAINT [DF_Sales_Discount_Details_Item_IdNo]  DEFAULT ((0)),	[ItemGroup_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Discount_Details_ItemGroup_IdNo]  DEFAULT ((0)),	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Discount_Details_Unit_IdNo]  DEFAULT ((0)),	[Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Discount_Details_Noof_Items]  DEFAULT ((0)),	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Details_Rate]  DEFAULT ((0)),	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Details_Amount]  DEFAULT ((0)),	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Details_Tax_Perc]  DEFAULT ((0)),	[Tax_Amount] [numeric](18, 2) NULL  CONSTRAINT [DF_Sales_Discount_Details_Tax_Amount]  DEFAULT ((0)),	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Details_Total_Amount]  DEFAULT ((0)),	[Serial_No] [varchar](500) NULL CONSTRAINT [DF_Sales_Discount_Details_Serial_No]  DEFAULT (''),	[Sales_Detail_SlNo] [int] NULL CONSTRAINT [DF_Sales_Discount_Details_Sales_Detail_SlNo]  DEFAULT ((0)),	[Sales_Code] [varchar](50) NULL CONSTRAINT [DF_Sales_Discount_Details_Sales_Code]  DEFAULT (''),	[Discount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Details_Discount_Perc]  DEFAULT ((0)),	[Discount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Discount_Details_Discount_Amount]  DEFAULT ((0))," & _
                          " CONSTRAINT [PK_Sales_Discount_Details] PRIMARY KEY CLUSTERED (	[Sales_Discount_Code],	[SL_No] ) ON [PRIMARY]) ON [PRIMARY]"

        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Sales_Discount_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Sales_Discount_Code  = '' where Sales_Discount_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Discount_Amount_Item NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Discount_Amount_Item = 0 where Discount_Amount_Item is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add Salesman_Idno smallint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Salesman_Idno = 0 where Salesman_Idno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Salesman_Head](	[Salesman_Idno] [int] NOT NULL,	[Salesman_Name] [varchar](100) NULL,	[Sur_Name] [varchar](100) NOT NULL, CONSTRAINT [PK_Salesman_Head] PRIMARY KEY CLUSTERED ( [Salesman_Idno]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into Salesman_Head(Salesman_Idno, Salesman_Name, Sur_Name ) Values (0,'','')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table AccountsGroup_Head add LedgerOrder_Position Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update AccountsGroup_Head set LedgerOrder_Position = Order_Position Where LedgerOrder_Position is Null"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update AccountsGroup_Head set LedgerOrder_Position = Order_Position Where LedgerOrder_Position = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update AccountsGroup_Head set LedgerOrder_Position = 1.5 Where AccountsGroup_IdNo = 27"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update AccountsGroup_Head set LedgerOrder_Position = 1.6 Where AccountsGroup_IdNo = 28"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Closing_Stock_Value_Head] ( [Closing_Stock_Value_Code] [varchar](30) NOT NULL, [Company_IdNo] [smallint] NULL, [for_OrderBy] [numeric](18, 2) NULL, [Closing_Stock_Value_Idno] [int] NULL, [Closing_Stock_Value_Date] [smalldatetime] NULL, [Closing_Stock_Value] [numeric](18, 2) NULL, CONSTRAINT [PK_Closing_Stock_Value_Head_1] PRIMARY KEY CLUSTERED ( [Closing_Stock_Value_Code] ) ON [PRIMARY],  CONSTRAINT [IX_Closing_Stock_Value_Head_1] UNIQUE NONCLUSTERED ( [Company_IdNo] ,  [Closing_Stock_Value_Date] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table ReportTemp add Name11 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ReportTemp add Name12 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ReportTemp add Name13 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ReportTemp add Name14 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ReportTemp add Name15 varchar(100) default ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ReportTempSub  add Name11 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ReportTempSub  add Name12 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ReportTempSub  add Name13 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ReportTempSub  add Name14 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table ReportTempSub  add Name15 varchar(100) default ''"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter table Voucher_Bill_Head add Voucher_Bill_DetailsSlNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Voucher_Bill_Head set Voucher_Bill_DetailsSlNo = 0 where Voucher_Bill_DetailsSlNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table itemgroup_head add Cetegory_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update itemgroup_head set Cetegory_IdNo = 0 where Cetegory_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Cetegory_Head](	[Cetegory_IdNo] [int] NOT NULL,	[Cetegory_Name] [varchar](50) NULL CONSTRAINT [DF_Cetegory_Head_Cetegory_Name]  DEFAULT (''),	[Sur_Name] [varchar](50) NULL CONSTRAINT [DF_Cetegory_Head_Sur_Name]  DEFAULT (''), CONSTRAINT [PK_Cetegory_Head] PRIMARY KEY CLUSTERED (        [Cetegory_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Item_Details]([Item_Idno] [int] NOT NULL,[Sl_No] [int] NOT NULL,[Size_IdNo] [int] NULL CONSTRAINT [DF_Item_Details_Size_IdNo]  DEFAULT ((0)),[Purchase_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Item_Details_Purchase_Rate]  DEFAULT ((0)),[Sales_rate] [numeric](18, 2) NULL CONSTRAINT [DF_Item_Details_Sales_rate]  DEFAULT ((0))," & _
                      "CONSTRAINT [PK_Item_Details] PRIMARY KEY CLUSTERED ([Item_Idno] ASC,[Sl_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Details add Piece_Box NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Details set Piece_Box = 0 where Piece_Box is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Purchase_Order_Head](	[Purchase_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Purchase_Order_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Purchase_Order_Head_for_OrderBy]  DEFAULT ((0)),	[Purchase_Order_Date] [datetime] NOT NULL,	[Payment_Method] [varchar](20) NULL CONSTRAINT [DF_Purchase_Order_Head_Payment_Method]  DEFAULT (''),	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_Ledger_IdNo]  DEFAULT ((0)),	[Cash_PartyName] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Cash_PartyName]  DEFAULT (''),	[Party_PhoneNo] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Party_PhoneNo]  DEFAULT (''),	[PurchaseAc_IdNo] [int] NULL CONSTRAINT [DF_Table_1_SalesAc_IdNo]  DEFAULT ((0)),	[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_Purchase_Order_Head_Tax_Type]  DEFAULT (''),	[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_TaxAc_IdNo]  DEFAULT ((0)),	[Delivery_Address1] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address1]  DEFAULT (''),	[Delivery_Address2] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address2]  DEFAULT (''),	[Delivery_Address3] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address3]  DEFAULT (''),	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Vehicle_No]  DEFAULT (''),	[Narration] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Head_Narration]  DEFAULT (''),	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Head_Total_Qty]  DEFAULT ((0)),	[Total_Bags] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_Total_Bags]  DEFAULT ((0)),	[SubTotal_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_SubTotal_Amount]  DEFAULT ((0)),	[Total_DiscountAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Total_DiscountAmount]  DEFAULT ((0)),	[Total_TaxAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Total_TaxAmount]  DEFAULT ((0)),	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Gross_Amount]  DEFAULT ((0)),	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_CashDiscount_Perc]  DEFAULT ((0)),	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_CashDiscount_Amount]  DEFAULT ((0)),	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Assessable_Value]  DEFAULT ((0)),	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Tax_Perc]  DEFAULT ((0)),	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Tax_Amount]  DEFAULT ((0)),	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Freight_Amount]  DEFAULT ((0)),	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_AddLess_Amount]  DEFAULT ((0)),	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Round_Off]  DEFAULT ((0)),	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Net_Amount]  DEFAULT ((0)),	[Document_Through] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Document_Through]  DEFAULT (''),	[Despatch_To] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Despatch_To]  DEFAULT (''),	[Lr_No] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Head_Lr_No]  DEFAULT (''),	[Lr_Date] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Head_Lr_Date]  DEFAULT (''),	[Booked_By] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Booked_By]  DEFAULT (''),	[Transport_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_Transport_IdNo]  DEFAULT ((0)),	[Freight_ToPay_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Freight_ToPay_Amount]  DEFAULT ((0)),	[Dc_No] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Dc_No]  DEFAULT (''),	[Dc_Date] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Dc_Date]  DEFAULT (''),	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Order_No]  DEFAULT (''),	[Order_Date] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Order_Date]  DEFAULT (''),	[Against_CForm_Status] [tinyint] NULL CONSTRAINT [DF_Purchase_Order_Head_Against_CForm_Status]  DEFAULT ((0)),	[Entry_Type] [varchar](20) NULL CONSTRAINT [DF_Purchase_Order_Head_Entry_Type]  DEFAULT (''),	[Payment_Terms] [varchar](100) NULL CONSTRAINT [DF_Purchase_Order_Head_Payment_Terms]  DEFAULT (''),	[OnAc_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_OnAc_IdNo]  DEFAULT ((0)),	[Extra_Charges] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Extra_Charges]  DEFAULT ((0)),	[Total_Extra_Copies] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Total_Extra_Copies]  DEFAULT ((0)),	[Sub_Total_Copies] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Sub_Total_Copies]  DEFAULT ((0)),	[Party_Name] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Party_Name]  DEFAULT (''),	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Head_Weight]  DEFAULT ((0)),	[Purchase_OrderAc_IdNo] [int] NULL CONSTRAINT [DF_Table_1_Sales_OrderAc_IdNo]  DEFAULT ((0)), CONSTRAINT [PK_Purchase_Order_Head] PRIMARY KEY CLUSTERED (        [Purchase_Order_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Purchase_Order_Details](	[Purchase_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Purchase_Order_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Purchase_Order_Details_for_OrderBy]  DEFAULT ((0)),	[Purchase_Order_Date] [datetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Purchase_Order_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Details_Item_IdNo]  DEFAULT ((0)),	[ItemGroup_IdNo] [smallint] NULL CONSTRAINT [DF_Purchase_Order_Details_ItemGroup_IdNo]  DEFAULT ((0)),	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Purchase_Order_Details_Unit_IdNo]  DEFAULT ((0)),	[Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Details_Noof_Items]  DEFAULT ((0)),	[Bags] [int] NULL CONSTRAINT [DF_Purchase_Order_Details_Bags]  DEFAULT ((0)),	[Weight_Bag] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Details_Weight_Bag]  DEFAULT ((0)),	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Details_Weight]  DEFAULT ((0)),	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Rate]  DEFAULT ((0)),	[Tax_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Tax_Rate]  DEFAULT ((0)),	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Amount]  DEFAULT ((0)),	[Discount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Discount_Perc]  DEFAULT ((0)),	[Discount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Discount_Amount]  DEFAULT ((0)),	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Tax_Perc]  DEFAULT ((0)),	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Tax_Amount]  DEFAULT ((0)),	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Total_Amount]  DEFAULT ((0)),	[Bag_Nos] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Details_Bag_Nos]  DEFAULT (''),	[Serial_No] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Details_Serial_No]  DEFAULT (''),	[Size_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Details_Size_IdNo]  DEFAULT ((0)),	[Meters] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Meters]  DEFAULT ((0)),	[Colour_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Details_Colour_IdNo]  DEFAULT ((0)),	[Noof_Items_Return] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Noof_Items_Return]  DEFAULT ((0)),	[Purchase_Order_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,	[Purchase_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Table_1_Sales_Items]  DEFAULT ((0))," & _
                          "CONSTRAINT [PK_Purchase_Order_Details] PRIMARY KEY CLUSTERED (	[Purchase_Order_Code] ,	[SL_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Entry_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Entry_Type  = '' where Entry_Type  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Freight_ToPay_Amount  NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_head set Freight_ToPay_Amount = 0 where Freight_ToPay_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add TradeDiscount_Perc  NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_head set TradeDiscount_Perc = 0 where TradeDiscount_Perc is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add TradeDiscount_Amount  NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_head set TradeDiscount_Amount = 0 where TradeDiscount_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Transport_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_head set Transport_IdNo = 0 where Transport_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Bags int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Bags = 0 where Bags is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Purchase_Detail_SlNo [int] IDENTITY (1, 1) NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Entry_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Entry_Type  = '' where Entry_Type  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Purchase_Order_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Purchase_Order_Code  = '' where Purchase_Order_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Purchase_Order_Detail_SlNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Purchase_Order_Detail_SlNo = 0 where Purchase_Order_Detail_SlNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Order_Head](	[Sales_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Sales_Order_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Order_Head_for_OrderBy]  DEFAULT ((0)),	[Sales_Order_Date] [datetime] NOT NULL,	[Payment_Method] [varchar](20) NULL CONSTRAINT [DF_Sales_Order_Head_Payment_Method]  DEFAULT (''),	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_Ledger_IdNo]  DEFAULT ((0)),	[Cash_PartyName] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Cash_PartyName]  DEFAULT (''),	[Party_PhoneNo] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Party_PhoneNo]  DEFAULT (''),	[SalesAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_SalesAc_IdNo]  DEFAULT ((0)),	[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_Sales_Order_Head_Tax_Type]  DEFAULT (''),	[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_TaxAc_IdNo]  DEFAULT ((0)),	[Delivery_Address1] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Delivery_Address1]  DEFAULT (''),	[Delivery_Address2] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Delivery_Address2]  DEFAULT (''),	[Delivery_Address3] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Delivery_Address3]  DEFAULT (''),	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Vehicle_No]  DEFAULT (''),	[Narration] [varchar](500) NULL CONSTRAINT [DF_Sales_Order_Head_Narration]  DEFAULT (''),	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Head_Total_Qty]  DEFAULT ((0)),	[Total_Bags] [int] NULL CONSTRAINT [DF_Sales_Order_Head_Total_Bags]  DEFAULT ((0)),	[SubTotal_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_SubTotal_Amount]  DEFAULT ((0)),	[Total_DiscountAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Total_DiscountAmount]  DEFAULT ((0)),	[Total_TaxAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Total_TaxAmount]  DEFAULT ((0)),	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Gross_Amount]  DEFAULT ((0)),	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_CashDiscount_Perc]  DEFAULT ((0)),	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_CashDiscount_Amount]  DEFAULT ((0)),	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Assessable_Value]  DEFAULT ((0)),	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Tax_Perc]  DEFAULT ((0)),	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Tax_Amount]  DEFAULT ((0)),	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Freight_Amount]  DEFAULT ((0)),	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_AddLess_Amount]  DEFAULT ((0)),	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Round_Off]  DEFAULT ((0)),	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Net_Amount]  DEFAULT ((0)),	[Document_Through] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Document_Through]  DEFAULT (''),	[Despatch_To] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Despatch_To]  DEFAULT (''),	[Lr_No] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Lr_No]  DEFAULT (''),	[Lr_Date] [varchar](20) NULL CONSTRAINT [DF_Sales_Order_Head_Lr_Date]  DEFAULT (''),	[Booked_By] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Booked_By]  DEFAULT (''),	[Transport_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_Transport_IdNo]  DEFAULT ((0)),	[Freight_ToPay_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Freight_ToPay_Amount]  DEFAULT ((0)),	[Dc_No] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Dc_No]  DEFAULT (''),	[Dc_Date] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Dc_Date]  DEFAULT (''),	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Order_No]  DEFAULT (''),	[Order_Date] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Order_Date]  DEFAULT (''),	[Against_CForm_Status] [tinyint] NULL CONSTRAINT [DF_Sales_Order_Head_Against_CForm_Status]  DEFAULT ((0)),	[Entry_Type] [varchar](20) NULL CONSTRAINT [DF_Sales_Order_Head_Entry_Type]  DEFAULT (''),	[Payment_Terms] [varchar](100) NULL CONSTRAINT [DF_Sales_Order_Head_Payment_Terms]  DEFAULT (''),	[OnAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_OnAc_IdNo]  DEFAULT ((0)),	[Extra_Charges] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Extra_Charges]  DEFAULT ((0)),	[Total_Extra_Copies] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Total_Extra_Copies]  DEFAULT ((0)),	[Sub_Total_Copies] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Sub_Total_Copies]  DEFAULT ((0)),	[Party_Name] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Party_Name]  DEFAULT (''),	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Head_Weight]  DEFAULT ((0)),	[Sales_OrderAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_Sales_OrderAc_IdNo]  DEFAULT ((0)), CONSTRAINT [PK_Sales_Order_Head] PRIMARY KEY CLUSTERED (        [Sales_Order_Code]) ON [PRIMARY]) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sales_Order_Details](	[Sales_Order_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Sales_Order_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Order_Details_for_OrderBy]  DEFAULT ((0)),	[Sales_Order_Date] [datetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Sales_Order_Details_Ledger_IdNo]  DEFAULT ((0)),	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Details_Item_IdNo]  DEFAULT ((0)),	[ItemGroup_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Order_Details_ItemGroup_IdNo]  DEFAULT ((0)),	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Order_Details_Unit_IdNo]  DEFAULT ((0)),	[Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Details_Noof_Items]  DEFAULT ((0)),	[Bags] [int] NULL CONSTRAINT [DF_Sales_Order_Details_Bags]  DEFAULT ((0)),	[Weight_Bag] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Details_Weight_Bag]  DEFAULT ((0)),	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Details_Weight]  DEFAULT ((0)),	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Rate]  DEFAULT ((0)),	[Tax_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Tax_Rate]  DEFAULT ((0)),	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Amount]  DEFAULT ((0)),	[Discount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Discount_Perc]  DEFAULT ((0)),	[Discount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Discount_Amount]  DEFAULT ((0)),	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Tax_Perc]  DEFAULT ((0)),	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Tax_Amount]  DEFAULT ((0)),	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Total_Amount]  DEFAULT ((0)),	[Bag_Nos] [varchar](500) NULL CONSTRAINT [DF_Sales_Order_Details_Bag_Nos]  DEFAULT (''),	[Serial_No] [varchar](500) NULL CONSTRAINT [DF_Sales_Order_Details_Serial_No]  DEFAULT (''),	[Size_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Details_Size_IdNo]  DEFAULT ((0)),	[Meters] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Meters]  DEFAULT ((0)),	[Colour_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Details_Colour_IdNo]  DEFAULT ((0)),	[Noof_Items_Return] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Noof_Items_Return]  DEFAULT ((0)),	[Sales_Order_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,	[Sales_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Details_Sales_Items]  DEFAULT ((0)), CONSTRAINT [PK_Sales_Order_Details] PRIMARY KEY CLUSTERED (	[Sales_Order_Code] ,        [SL_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Entry_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Entry_Type  = '' where Entry_Type  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Sales_Order_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Sales_Order_Code  = '' where Sales_Order_Code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Sales_Order_Detail_SlNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Sales_Order_Detail_SlNo = 0 where Sales_Order_Detail_SlNo is Null"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter table Item_Head add Item_Name_Tamil varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Head set Item_Name_Tamil = '' where Item_Name_Tamil is Null"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Update " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..Settings_Head set Cc_No = '1053'"
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Purchase_Return_Head]([Purchase_Return_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Purchase_Return_No] [varchar](50) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Purchase_Return_Date] [smalldatetime] NOT NULL,[Payment_Method] [varchar](20) NULL default (''),[Ledger_IdNo] [int] NULL default (0),[Bill_No] [varchar](20) NULL default (''),[PurchaseAc_IdNo] [int] NULL default (0),[Tax_Type] [varchar](20) NULL default (''),[TaxAc_IdNo] [int] NULL default (0),[Narration] [varchar](1000) NULL default (''),[Vehicle_No] [varchar](50) NULL default (''),[Total_Qty] [numeric](18, 3) NULL default (0),[Total_Bags] [int] NULL default (0),[Total_Weight] [numeric](18, 3) NULL default (0),[SubTotal_Amount] [numeric](18, 2) NULL default (0),[Total_DiscountAmount] [numeric](18, 2) NULL,[Total_TaxAmount] [numeric](18, 2) NULL default (0),[Gross_Amount] [numeric](18, 2) NULL default (0),[CashDiscount_Perc] [numeric](18, 2) NULL default (0),[CashDiscount_Amount] [numeric](18, 2) NULL default (0),[AddLess_BeforeTax_Amount] [numeric](18, 2) NULL default (0),[Tax_Perc] [numeric](18, 2) NULL default (0),[Tax_Amount] [numeric](18, 2) NULL default (0),[Assessable_Value] [numeric](18, 2) NULL default (0),[Freight_Amount] [numeric](18, 2) NULL default (0),[AddLess_Amount] [numeric](18, 2) NULL default (0),[Round_Off] [numeric](18, 2) NULL default (0),[Net_Amount] [numeric](18, 2) NULL default (0),[Bale_Nos] [varchar](500) NULL default ('')," & _
                   "CONSTRAINT [PK_Purhase_Return_Head] PRIMARY KEY CLUSTERED ([Purchase_Return_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Purchase_Return_Details]([Purchase_Return_Code] [varchar](50) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Purchase_Return_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Purchase_Return_Date] [smalldatetime] NOT NULL,[Ledger_IdNo] [int] NOT NULL,[SL_No] [smallint] NOT NULL,[Item_IdNo] [int] NULL default (0),[Unit_IdNo] [smallint] NULL default (0),[Noof_Items] [numeric](18, 3) NULL default (0),[Bales] [int] NULL default (0),[Weight] [numeric](18, 3) NULL default (0),[Rate] [numeric](18, 2) NULL default (0),[Tax_Rate] [numeric](18, 2) NULL default (0),	[Amount] [numeric](18, 2) NULL default (0),[Discount_Perc] [numeric](18, 2) NULL default (0),[Discount_Amount] [numeric](18, 2) NULL default (0),[Tax_Perc] [numeric](18, 2) NULL default (0),[Tax_Amount] [numeric](18, 2) NULL default (0),[Total_Amount] [numeric](18, 2) NULL default (0),[Bale_Nos] [varchar](500) NULL default (''),[TaxAmount_Difference] [numeric](18, 2) NULL default (0),[Size_IdNo] [int] NULL default (0)," & _
                          "CONSTRAINT [PK_Purhase_Return_Details] PRIMARY KEY CLUSTERED ([Purchase_Return_Code]  , [SL_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Bill_No varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Bill_No = '' where Bill_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Bill_Date varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Bill_Date = '' where Bill_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Noof_Items_Return NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Noof_Items_return = 0 where Noof_Items_Return is Null"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Alter table Sales_details add Sales_Slno int default 0"
        'cmd.ExecuteNonQuery()
        'cmd.CommandText = "Update Sales_Details set Sales_SlNo = 0 where Sales_Slno is Null"
        'cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table Sales_Details add Sales_Detail_SlNo [int] IDENTITY (1, 1) NOT NULL"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table SalesReturn_details add Sales_Detail_Slno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Sales_Detail_SlNo = 0 where Sales_Detail_Slno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Details add Sales_Code varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Sales_Code = '' where Sales_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Total_DiscountAmount NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_head set Total_DiscountAmount = 0 where Total_DiscountAmount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Total_TaxAmount NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_head set Total_TaxAmount = 0 where Total_TaxAmount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Freight_Amount NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_head set Freight_Amount = 0 where Freight_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add SubTotal_Amount NUMERIC(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_head set SubTotal_Amount = 0 where SubTotal_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Total_Bags int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_head set Total_Bags = 0 where Total_Bags is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Weight NUMERIC(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_head set Weight = 0 where Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Against_CForm_Status int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_head set Against_CForm_Status = 0 where Against_CForm_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Order_No varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Order_No = '' where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Order_Date varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Order_Date = '' where Order_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Lr_No varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Lr_No = '' where Lr_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Lr_Date varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Lr_Date = '' where Lr_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Document_Through varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Document_Through = '' where Document_Through is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add Booked_By varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Head set Booked_By = '' where Booked_By is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table SalesReturn_Head add Despatch_To varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Despatch_To = '' where Despatch_To is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_Head add SalesAc_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_head set SalesAc_IdNo = 0 where SalesAc_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_details add Tax_Rate numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Tax_Rate = 0 where Tax_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_details add Tax_Perc numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Tax_Perc = 0 where Tax_Perc is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_details add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Colour_IdNo = 0 where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_details add Design_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Design_IdNo = 0 where Design_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_details add Gender_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Gender_IdNo = 0 where Gender_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_details add Sleeve_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Sleeve_IdNo = 0 where Sleeve_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table SalesReturn_details add Size_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update SalesReturn_Details set Size_IdNo = 0 where Size_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Weight NUMERIC(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_head set Weight = 0 where Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Against_CForm_Status int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_head set Against_CForm_Status = 0 where Against_CForm_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Order_No varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Order_No = '' where Order_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Order_Date varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Order_Date = '' where Order_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Lr_No varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Lr_No = '' where Lr_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Lr_Date varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Lr_Date = '' where Lr_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Document_Through varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Document_Through = '' where Document_Through is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Booked_By varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Booked_By = '' where Booked_By is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add Despatch_To varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Head set Despatch_To = '' where Despatch_To is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Head add SalesAc_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_head set SalesAc_IdNo = 0 where SalesAc_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_details add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Colour_IdNo = 0 where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_details add Design_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Design_IdNo = 0 where Design_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_details add Gender_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Gender_IdNo = 0 where Gender_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_details add Sleeve_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Sleeve_IdNo = 0 where Sleeve_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Design_Head]([Design_IdNo] [int] NOT NULL,	[Design_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL, " & _
            " CONSTRAINT [PK_Design_Head] PRIMARY KEY CLUSTERED ( [Design_IdNo]) ON [PRIMARY], CONSTRAINT [IX_Design_Head] UNIQUE NONCLUSTERED ( [Sur_Name]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Sleeve_Head]([Sleeve_IdNo] [int] NOT NULL,	[Sleeve_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL, " & _
            " CONSTRAINT [PK_Sleeve_Head] PRIMARY KEY CLUSTERED ( [Sleeve_IdNo]) ON [PRIMARY], CONSTRAINT [IX_Sleeve_Head] UNIQUE NONCLUSTERED ( [Sur_Name]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Gender_Head]([Gender_IdNo] [int] NOT NULL,	[Gender_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL, " & _
            " CONSTRAINT [PK_Gender_Head] PRIMARY KEY CLUSTERED ( [Gender_IdNo]) ON [PRIMARY], CONSTRAINT [IX_Gender_Head] UNIQUE NONCLUSTERED ( [Sur_Name]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Item_Processing_Details add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Processing_Details set Colour_IdNo = 0 where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Processing_Details add Design_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Processing_Details set Design_IdNo = 0 where Design_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Processing_Details add Gender_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Processing_Details set Gender_IdNo = 0 where Gender_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Processing_Details add Sleeve_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Processing_Details set Sleeve_IdNo = 0 where Sleeve_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_details add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Colour_IdNo = 0 where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_details add Design_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Design_IdNo = 0 where Design_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_details add Gender_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Gender_IdNo = 0 where Gender_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_details add Sleeve_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Sleeve_IdNo = 0 where Sleeve_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Item_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Item_Code = '' where Item_code is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add Party_Name varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Party_Name = '' where Party_Name is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Labour_Charge int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Labour_Charge = 0 where Labour_Charge is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Machine_Head add Description varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Machine_Head set Description = '' where Description is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [TempTable_For_NegativeStock]( [Reference_Code] [varchar](50) NULL DEFAULT ('') , [Reference_Date] [smalldatetime] NULL, [Company_Idno] [smallint] NULL DEFAULT (0), [Item_IdNo] [int] NULL DEFAULT (0), [Quantity] [numeric](18, 3) NULL  DEFAULT (0)  ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Delivery_Head add NoOf_Bundle varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Delivery_Head set NoOf_Bundle = '' where NoOf_Bundle is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add NoOf_Bundle varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set NoOf_Bundle = '' where NoOf_Bundle is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Sales_Reading_Details]([Sales_Code] [varchar](50) NOT NULL,	[Sales_No] [varchar](50) NOT NULL,	[Company_IdNo] [int] NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Sales_Date] [smalldatetime] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Machine_IdNo] [smallint] NULL,	[Opening_Reading] [int] NULL,	[Closing_Reading] [int] NULL,	[Sub_Total_Copies] [int] NULL,	[Extra_Copies] [int] NULL, CONSTRAINT [PK_Sales_Reading_Details] PRIMARY KEY NONCLUSTERED (	[Sales_Code] ,  [Sl_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Machine_Head](	[Machine_IdNo] [int] NOT NULL,	[Machine_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL, CONSTRAINT [PK_Machine_Head] PRIMARY KEY CLUSTERED ( [Machine_IdNo]) ON [PRIMARY], CONSTRAINT [IX_Machine_Head] UNIQUE NONCLUSTERED ( [Sur_Name]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Ledger_Reading_Details]([Ledger_IdNo] [int] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Machine_IdNo] [int] NOT NULL,	[Opening_Reading] [int] NULL, CONSTRAINT [PK_Ledger_Reading_Details] PRIMARY KEY NONCLUSTERED (	[Ledger_IdNo] ,        [Sl_No]) ON [PRIMARY], CONSTRAINT [IX_Ledger_Reading_Details] UNIQUE NONCLUSTERED (	[Ledger_IdNo] ,        [Machine_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Rate_Extra_Copy numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Rate_Extra_Copy = 0 where Rate_Extra_Copy is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Rent_Machine numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Rent_Machine = 0 where Rent_Machine is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Free_Copies_Machine int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Free_Copies_Machine = 0 where Free_Copies_Machine is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Total_Copies int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Total_Copies = 0 where Total_Copies is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Total_Free_Copies int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Total_Free_Copies = 0 where Total_Free_Copies is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Additional_Copies int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Additional_Copies = 0 where Additional_Copies is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Rent numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Rent = 0 where Rent is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Extra_Charges numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Extra_Charges = 0 where Extra_Charges is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add Total_Extra_Copies numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Total_Extra_Copies = 0 where Total_Extra_Copies is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add Sub_Total_Copies numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Sub_Total_Copies = 0 where Sub_Total_Copies is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Opening_Date smalldatetime default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Opening_Date = 0 where Opening_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Closing_Date smalldatetime default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Closing_Date = 0 where Closing_Date is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Total_Machine int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Total_Machine = 0 where Total_Machine is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Rent_Machine numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Rent_Machine = 0 where Rent_Machine is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Free_Copies_Machine int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Free_Copies_Machine = 0 where Free_Copies_Machine is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Rate_Extra_Copy numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Rate_Extra_Copy = 0 where Rent_Machine is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Machine_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Machine_IdNo = 0 where Machine_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Opening_Reading int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Opening_Reading = 0 where Opening_Reading is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Total_Machine int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Total_Machine = 0 where Total_Machine is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Payment_Terms varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Payment_Terms = '' where Payment_Terms is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [State_Head](	[State_IdNo] [smallint] NOT NULL,	[State_Name] [varchar](50) NOT NULL, [Sur_Name] [varchar](50) NOT NULL,	[State_Code] [varchar](50) NOT NULL, " & _
                          "CONSTRAINT [PK_State_Head] PRIMARY KEY CLUSTERED (  [State_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head  add State_Idno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set State_Idno = 0 where State_Idno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Delivery_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Delivery_Code = '' where Delivery_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Delivery_Head add Invoice_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Delivery_Head set Invoice_Code = '' where Invoice_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Selection_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Selection_Type = '' where Selection_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Delivery_Head](	[Delivery_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Delivery_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Delivery_Date] [datetime] NOT NULL,	[Payment_Method] [varchar](20) NULL,	[Ledger_IdNo] [int] NULL,	[Cash_PartyName] [varchar](50) NULL,	[Party_PhoneNo] [varchar](50) NULL,	[SalesAc_IdNo] [int] NULL,	[Tax_Type] [varchar](20) NULL,	[TaxAc_IdNo] [int] NULL,	[Vehicle_No] [varchar](50) NULL,	[Removal_Date] [varchar](20) NULL,	[Removal_Time] [varchar](20) NULL,	[Bag_Nos] [varchar](500) NULL,	[Narration] [varchar](500) NULL,	[Total_Qty] [numeric](18, 3) NULL,	[SubTotal_Amount] [numeric](18, 2) NULL, [Net_Amount] [numeric](18, 2) NULL,	[Transport_IdNo] [int] NULL,	[Order_No] [varchar](50) NULL,	[Order_Date] [varchar](50) NULL,	[Against_CForm_Status] [tinyint] NULL,	[Weight] [numeric](18, 3) NULL,	[Branch_Transfer_Status] [tinyint] NULL,	[OnAc_IdNo] [int] NULL,	[Total_Rolls] [numeric](18, 2) NULL,	[Total_Meters] [numeric](18, 2) NULL,	[Remarks] [varchar](100) NULL," & _
                          "CONSTRAINT [PK_Delivery_Head] PRIMARY KEY CLUSTERED (  [Delivery_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Delivery_Details](	[Delivery_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Delivery_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Delivery_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NOT NULL,	[SL_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL,	[Colour_IdNo] [smallint] NULL,	[Unit_IdNo] [smallint] NULL,	[Rate] [numeric](18, 2) NULL,	[Amount] [numeric](18, 2) NULL,	[Total_Amount] [numeric](18, 2) NULL,	[Gms] [numeric](18, 2) NULL,	[Rolls] [numeric](18, 2) NULL,	[Weight_Rolls] [numeric](18, 3) NULL,	[Meters] [numeric](18, 2) NULL,	[Quantity] [numeric](18, 3) NULL,	[Remarks] [varchar](100) NULL, " & _
                          "CONSTRAINT [PK_Delivery_Details] PRIMARY KEY CLUSTERED (	[Delivery_Code] ,	[SL_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Delivery_Head add Total_Actual_Weight numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Delivery_Head set Total_Actual_Weight = 0 where Total_Actual_Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Delivery_Details add Actual_Weight numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Delivery_Details set Actual_Weight = 0 where Actual_Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head  add LedgerGroup_Idno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set LedgerGroup_Idno = 0 where LedgerGroup_Idno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Colour_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Colour_IdNo = 0 where Colour_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Colour_Head]([Colour_IdNo] [int] NOT NULL,	[Colour_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL," & _
            "CONSTRAINT [PK_Colour_Head] PRIMARY KEY CLUSTERED ( [Colour_IdNo]) ON [PRIMARY], CONSTRAINT [IX_Colour_Head] UNIQUE NONCLUSTERED (        [Sur_Name]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Item_code varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details  set Item_code  = '' where Item_code  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Total_Rolls numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Total_Rolls = 0 where Total_Rolls is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Total_Meters numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Total_Meters = 0 where Total_Meters is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Branch_Transfer_Status tinyint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Branch_Transfer_Status = 0 where Branch_Transfer_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add OnAc_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set OnAc_IdNO = 0 where OnAc_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add GSM Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set GSM = 0 where GSM is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Rolls Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Rolls = 0 where Rolls is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Weight_Roll Numeric(18, 3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Weight_Roll = 0 where Weight_Roll is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Meters Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Meters = 0 where Meters is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Payment_Terms varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head  set Payment_Terms  = '' where Payment_Terms  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Pan_No varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Pan_No = '' where Pan_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Cloth_Sales_Head add Discount_Percentage Numeric(18, 3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Cloth_Sales_Head set Discount_Percentage = 0 where Discount_Percentage is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Cloth_Sales_Head add Discount_Amount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Cloth_Sales_Head set Discount_Amount = 0 where Discount_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Cloth_Sales_Head add Add_Less Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Cloth_Sales_Head set Add_Less = 0 where Add_Less is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Cloth_Sales_Head add net_Amount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Cloth_Sales_Head set net_Amount = 0 where net_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Cloth_Sales_Head add Bale_Nos varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Cloth_Sales_Head set Bale_Nos = '' where Bale_Nos is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Cloth_Sales_Head add Agent_IdNo Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Cloth_Sales_Head set Agent_IdNo = 0 where Agent_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Cloth_Sales_Head](	[Cloth_Sales_Code] [varchar](50) NOT NULL,	[Cloth_Sales_No] [varchar](50) NOT NULL,	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Company_IdNo] [int] NOT NULL,	[Cloth_Sales_Date] [smalldatetime] NOT NULL,	[Invoice_No] [varchar](50) NULL,	[Ledger_IdNo] [smallint] NULL,	[Ledger_IdNo1] [smallint] NULL,	[Transport_IdNo] [smallint] NULL,	[Lr_No] [varchar](50) NULL,	[No_Of_Sales] [int] NULL,	[Meter] [numeric](18, 2) NULL,	[Rate] [numeric](18, 3) NULL,	[Amount] [numeric](18, 3) NULL,	[Com_Type] [varchar](25) NULL,	[Com_Rate] [numeric](18, 3) NULL,	[Com_Amount] [numeric](18, 3) NULL," & _
                          "CONSTRAINT [PK_Cloth_Sales_Head] PRIMARY KEY NONCLUSTERED ( [Cloth_Sales_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Price_List_Head add Price_List_IdNo Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Price_List_Head set Price_List_IdNo = 0 where Price_List_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Price_List_Head]( [Price_List_IdNo] [int] NOT NULL,	[Price_List_Name] [varchar](50) NOT NULL,	[sur_name] [varchar](50) NOT NULL," & _
                          "CONSTRAINT [PK_Price_List_Head] PRIMARY KEY NONCLUSTERED (  [Price_List_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Price_List_Details]([Price_List_IdNo] [int] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Item_IdNo] [int] NULL,	[Rate] [numeric](18, 3) NULL," & _
                          "CONSTRAINT [PK_Price_List_Details] PRIMARY KEY NONCLUSTERED (	[Price_List_IdNo] ,	[Sl_No]  )  ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Temp_Ends_Head] ( [Ends_Name] [varchar](50) NOT NULL ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ledger_head add Price_List_IdNo Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ledger_head set Price_List_IdNo = 0 where Price_List_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_head add Price_List_IdNo Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_head set Price_List_IdNo = 0 where Price_List_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_head add Ledger_IdNo Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_head set Ledger_IdNo = 0 where Ledger_IdNo is Null"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "Alter table Knotting_Head add Knotting_IdNo Int default 0"
        'cmd.ExecuteNonQuery()
        'cmd.CommandText = "Update Knotting_Head set Knotting_IdNo = 0 where Knotting_IdNo is Null"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "CREATE TABLE [Knotting_Head] ( [Knotting_Code] [varchar](50) NOT NULL, [Company_IdNo] [smallint] NOT NULL, [Knotting_No] [varchar](20) NOT NULL, [for_OrderBy] [numeric](18, 2) NOT NULL, [Knotting_Date] [smalldatetime] NOT NULL, [Ledger_IdNo] [int] NULL CONSTRAINT [DF_Knotting_Head_Ledger_IdNo]  DEFAULT ((0)), [Shift] [varchar](20) NULL CONSTRAINT [DF_Knotting_Head_Shift]  DEFAULT (''), [Loom] [varchar](100) NULL CONSTRAINT [DF_Knotting_Head_Loom]  DEFAULT (''), [Ends] [int] NULL CONSTRAINT [DF_Knotting_Head_Ends]  DEFAULT ((0)), [No_Pavu] [int] NULL CONSTRAINT [DF_Knotting_Head_No_Pavu]  DEFAULT ((0)), [Knotting_Bill_Code] [varchar](50) NULL CONSTRAINT [DF_Knotting_Head_Knotting_Bill_Code]  DEFAULT (''), CONSTRAINT [PK_Knotting_Head] PRIMARY KEY CLUSTERED ( [Knotting_Code] ) ON [PRIMARY] ) ON [PRIMARY]"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "CREATE TABLE [Knotting_Bill_Head] ( [Knotting_Bill_Code] [varchar](50) NOT NULL, [Company_IdNo] [smallint] NOT NULL, [Knotting_Bill_No] [varchar](20) NOT NULL, [for_OrderBy] [numeric](18, 2) NOT NULL, [Knotting_Bill_Date] [smalldatetime] NOT NULL, [Ledger_IdNo] [int] NOT NULL, [Entry_Type] [varchar](30) NULL CONSTRAINT [DF_Knotting_Bill_Head_Entry_Type]  DEFAULT (''), [Total_Pavu] [int] NULL CONSTRAINT [DF_Knotting_Bill_Head_Total_Pavu]  DEFAULT ((0)), [Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_Rate]  DEFAULT ((0)), [Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_Gross_Amount]  DEFAULT ((0)), [AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_AddLess_Amount]  DEFAULT ((0)), 	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_Round_Off]  DEFAULT ((0)), " & _
        '                     " [Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_Net_Amount] DEFAULT ((0)),  CONSTRAINT [PK_Knotting_Bill_Head] PRIMARY KEY CLUSTERED  ( [Knotting_Bill_Code] ) ON [PRIMARY] ) ON [PRIMARY] "
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "CREATE TABLE [Knotting_Bill_Details] ( [Knotting_Bill_Code] [varchar](50) NOT NULL, [Company_IdNo] [smallint] NOT NULL, 	[Knotting_Bill_No] [varchar](30) NOT NULL, 	[for_OrderBy] [numeric](18, 2) NOT NULL, [Knotting_Bill_Date] [smalldatetime] NOT NULL, [Ledger_IdNo] [int] NOT NULL, [Sl_No] [smallint] NOT NULL, [Knotting_Date] [smalldatetime] NULL, [Knotting_No] [varchar](20) NULL CONSTRAINT [DF_Knotting_Bill_Details_Knotting_No]  DEFAULT (''), 	[Shift] [varchar](20) NULL CONSTRAINT [DF_Knotting_Bill_Details_Shift]  DEFAULT (''), 	[Ends] [int] NULL CONSTRAINT [DF_Knotting_Bill_Details_Ends]  DEFAULT ((0)), [Loom] [varchar](200) NULL CONSTRAINT [DF_Knotting_Bill_Details_Loom]  DEFAULT (''), 	[No_Pavu] [int] NULL CONSTRAINT [DF_Knotting_Bill_Details_No_Pavu]  DEFAULT ((0)), " & _
        '                     " [Knotting_Code] [varchar](50) NULL CONSTRAINT [DF_Knotting_Bill_Details_Knotting_Code]  DEFAULT (''),  CONSTRAINT [PK_Knotting_Bill_Details] PRIMARY KEY CLUSTERED  ( [Knotting_Bill_Code] , [Sl_No] ) ON [PRIMARY] ) ON [PRIMARY]"
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Shift_Head] ( [Shift_IdNo] [Int] NOT NULL, [Shift_Name] [varchar](50) NOT NULL,  CONSTRAINT [PK_Shift_Head] PRIMARY KEY CLUSTERED  ( [Shift_IdNo] )   WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table ItemGroup_Head add Commodity_Code varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update ItemGroup_Head set Commodity_Code = '' where Commodity_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Size_Head add Total_Sqft Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Size_Head set Total_Sqft = 0 where Total_Sqft is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Rate_Sqft Numeric(18, 4) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Rate_Sqft = 0 where Rate_Sqft is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Entry_Type varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Entry_Type   = '' where Entry_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add JobWork_Code varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set JobWork_Code   = '' where JobWork_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add JobWork_No varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set JobWork_No    = '' where JobWork_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add JobWork_Date varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set JobWork_Date    = '' where JobWork_Date  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table JobWork_Head add Sales_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update JobWork_Head set Sales_Code   = '' where Sales_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details  add JobWork_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set JobWork_No    = '' where JobWork_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details  add JobWork_Code varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set JobWork_Code  = '' where JobWork_Code is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add Size_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set Size_IdNo = 0 where Size_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Processing_Details add Size_Idno int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Processing_Details set Size_Idno = 0 where Size_Idno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Order_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Order_No   = '' where Order_No  is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Sales_Head add Order_Date varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Order_Date   = '' where Order_Date  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Against_CForm_Status tinyint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Against_CForm_Status = 0 where Against_CForm_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Weight Numeric(18, 3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Weight = 0 where Weight is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Purchase_Details add TaxAmount_Difference Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Purchase_Details set TaxAmount_Difference = 0 where TaxAmount_Difference is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [SalesReturn_Details] ( [SalesReturn_Code] [varchar](50) NOT NULL, 	[Company_IdNo] [smallint] NOT NULL,  [SalesReturn_No] [varchar](20) NOT NULL, [for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_CashSalesReturn_Details_for_OrderBy]  DEFAULT ((0)), [SalesReturn_Date] [smalldatetime] NOT NULL, [Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_CashSalesReturn_Details_Ledger_IdNo]  DEFAULT ((0)), [SL_No] [smallint] NOT NULL, [Item_IdNo] [int] NULL CONSTRAINT [DF_CashSalesReturn_Details_Item_IdNo]  DEFAULT ((0)), [Unit_IdNo] [smallint] NULL CONSTRAINT [DF_CashSalesReturn_Details_Unit_IdNo]  DEFAULT ((0)), [Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_CashSalesReturn_Details_Noof_Items]  DEFAULT ((0)), 	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Details_Rate]  DEFAULT ((0)), 	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Details_Total_Amount1]  DEFAULT ((0)), " & _
                         " [Serial_No] [varchar](500) NULL CONSTRAINT [DF_SalesReturn_Details_Serial_No]  DEFAULT (''),  CONSTRAINT [PK_SalesReturn_Details] PRIMARY KEY CLUSTERED  ( [SalesReturn_Code] , [SL_No] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [SalesReturn_Head] ( [SalesReturn_Code] [varchar](50) NOT NULL, [Company_IdNo] [smallint] NOT NULL, [SalesReturn_No] [varchar](50) NOT NULL, [for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_CashSalesReturn_Head_for_OrderBy]  DEFAULT ((0)), [SalesReturn_Date] [datetime] NOT NULL, [Payment_Method] [varchar](20) NULL CONSTRAINT [DF_SalesReturn_Head_Payment_Method]  DEFAULT (''), [Ledger_IdNo] [int] NULL CONSTRAINT [DF_CashSalesReturn_Head_Ledger_IdNo]  DEFAULT ((0)), [Cash_PartyName] [varchar](50) NULL CONSTRAINT [DF_SalesReturn_Head_Delivery_Address11]  DEFAULT (''), 	[Bill_No] [varchar](35) NULL CONSTRAINT [DF__SalesRetu__Dc_No__592635D8]  DEFAULT (''), " & _
                         " [SalesReturnAc_IdNo] [int] NULL CONSTRAINT [DF_CashSalesReturn_Head_SalesAc_IdNo]  DEFAULT ((0)), 	[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_SalesReturn_Head_Tax_Type]  DEFAULT (''), 	[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_SalesReturn_Head_SalesAc_IdNo1]  DEFAULT ((0)), 	[Narration] [varchar](500) NULL CONSTRAINT [DF_SalesReturn_Head_Narration]  DEFAULT (''), 	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_CashSalesReturn_Head_Total_Qty]  DEFAULT ((0)), 	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Head_CashDiscount_Perc1]  DEFAULT ((0)), 	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Perc]  DEFAULT ((0)), 	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Amount]  DEFAULT ((0)), " & _
                         " [Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_AddLess_Amount1]  DEFAULT ((0)), 	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Perc1]  DEFAULT ((0)), 	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Amount1]  DEFAULT ((0)), 	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Head_AddLess_Amount]  DEFAULT ((0)), 	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Head_Round_Off]  DEFAULT ((0)), 	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Head_Net_Amount]  DEFAULT ((0)),  CONSTRAINT [PK_SalesReturn_Head] PRIMARY KEY CLUSTERED ( [SalesReturn_Code] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_Head add Ledger_EmailID varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_Head set Ledger_EmailID  = '' where Ledger_EmailID  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Voucher_Bill_Details] ( [Voucher_Bill_Code] [varchar](30) NOT NULL, [Company_Idno] [smallint] NOT NULL, [Voucher_Bill_Date] [smalldatetime] NOT NULL, [Ledger_IdNo] [smallint] NOT NULL, [Entry_Identification] [varchar](35) NOT NULL, [Amount] [numeric](18, 2) NOT NULL, [CrDr_Type] [varchar](10) NOT NULL ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Voucher_Bill_Head] ( [Voucher_Bill_Code] [varchar](30) NOT NULL, [Company_Idno] [smallint] NOT NULL, [Voucher_Bill_No] [varchar](20) NOT NULL, [For_OrderBy] [numeric](18, 2) NOT NULL, [Voucher_Bill_Date] [smalldatetime] NOT NULL, [Ledger_IdNo] [smallint] NOT NULL CONSTRAINT [DF_Voucher_Bill_Head_Ledger_IdNo]  DEFAULT ((0)), [Party_Bill_No] [varchar](20) NULL CONSTRAINT [DF_Voucher_Bill_Head_Party_Bill_No]  DEFAULT (''), [Agent_IdNo] [smallint] NULL CONSTRAINT [DF_VoucherBillHead_AgentIdNo]  DEFAULT ((0)), [Bill_Amount] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Voucher_Bill_Head_Bill_Amount]  DEFAULT ((0)), [Credit_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_VoucherBillHead_CreditAmount]  DEFAULT ((0)), 	[Debit_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_VoucherBillHead_DebitAmount]  DEFAULT ((0)), [CrDr_Type] [varchar](10) NOT NULL, 	[Entry_Identification] [varchar](35) NOT NULL CONSTRAINT [DF_Voucher_Bill_Head_Entry_Identification]  DEFAULT (''), 	[Commission_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF_Voucher_Bill_Head_Commission_Percentage]  DEFAULT ((0)), " & _
                             " [Agent_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Voucher_Bill_Head_Agent_Amount]  DEFAULT ((0)), CONSTRAINT [PK_Voucher_Bills_Head] PRIMARY KEY NONCLUSTERED  ( [Voucher_Bill_Code] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table voucher_head add Entry_ID varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update voucher_head set Entry_ID =  left(entry_identification, len(entry_identification)-6) end ) Where Entry_ID is Null or Entry_ID = ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table voucher_details add Entry_ID varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update voucher_details set Entry_ID =  left(entry_identification, len(entry_identification)-6) end ) where Entry_ID is Null or Entry_ID = ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Month_Head] ( [Month_IdNo] [tinyint] NOT NULL, [Month_Name] [varchar](30) NOT NULL, [Month_ShortName] [varchar](20) NOT NULL, [Idno] [tinyint] NOT NULL, CONSTRAINT [PK_Month_Head] PRIMARY KEY NONCLUSTERED  (  [Month_IdNo] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [EntryTemp] ( [Name1] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name1]  DEFAULT (''), 	[Name2] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name2]  DEFAULT (''), 	[Name3] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name3]  DEFAULT (''), 	[Name4] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name4]  DEFAULT (''), 	[Name5] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name5]  DEFAULT (''), 	[Name6] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name6]  DEFAULT (''), 	[name7] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_name7]  DEFAULT (''), 	[Name8] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name8]  DEFAULT (''), 	[Name9] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name9]  DEFAULT (''), 	[Name10] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name10]  DEFAULT (''), " & _
                                " [Date1] [smalldatetime] NULL, 	[Date2] [smalldatetime] NULL, 	[Date3] [smalldatetime] NULL, 	[Date4] [smalldatetime] NULL, 	[Date5] [smalldatetime] NULL, " & _
                                " [Int1] [int] NULL CONSTRAINT [DF_EntryTemp_Int1]  DEFAULT ((0)),  	[Int2] [int] NULL CONSTRAINT [DF_EntryTemp_Int2]  DEFAULT ((0)), 	[Int3] [int] NULL CONSTRAINT [DF_EntryTemp_Int3]  DEFAULT ((0)), 	[Int4] [int] NULL CONSTRAINT [DF_EntryTemp_Int4]  DEFAULT ((0)), 	[Int5] [int] NULL CONSTRAINT [DF_EntryTemp_Int5]  DEFAULT ((0)), 	[Int6] [int] NULL CONSTRAINT [DF_EntryTemp_Int6]  DEFAULT ((0)), 	[Int7] [int] NULL CONSTRAINT [DF_EntryTemp_Int7]  DEFAULT ((0)), 	[Int8] [int] NULL CONSTRAINT [DF_EntryTemp_Int8]  DEFAULT ((0)), 	[Int9] [int] NULL CONSTRAINT [DF_EntryTemp_Int9]  DEFAULT ((0)), 	[Int10] [int] NULL CONSTRAINT [DF_EntryTemp_Int10]  DEFAULT ((0)), " & _
                                " [Meters1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters1]  DEFAULT ((0)), 	[Meters2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters2]  DEFAULT ((0)),	[Meters3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters3]  DEFAULT ((0)), 	[Meters4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters4]  DEFAULT ((0)), 	[Meters5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters5]  DEFAULT ((0)), 	[Meters6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters6]  DEFAULT ((0)), 	[Meters7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters7]  DEFAULT ((0)), 	[Meters8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters8]  DEFAULT ((0)), 	[Meters9] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters9]  DEFAULT ((0)), 	[Meters10] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters10]  DEFAULT ((0)), " & _
                                " [Weight1] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight1]  DEFAULT ((0)), 	[Weight2] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight2]  DEFAULT ((0)), 	[Weight3] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight3]  DEFAULT ((0)), 	[Weight4] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight4]  DEFAULT ((0)), 	[Weight5] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight5]  DEFAULT ((0)), 	[Weight6] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight6]  DEFAULT ((0)), 	[Weight7] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight7]  DEFAULT ((0)), 	[Weight8] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight8]  DEFAULT ((0)), 	[Weight9] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight9]  DEFAULT ((0)), 	[Weight10] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight10]  DEFAULT ((0)), " & _
                                " [Currency1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency1]  DEFAULT ((0)), 	[Currency2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency2]  DEFAULT ((0)), 	[Currency3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency3]  DEFAULT ((0)), 	[Currency4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency4]  DEFAULT ((0)), 	[Currency5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency5]  DEFAULT ((0)), 	[Currency6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency6]  DEFAULT ((0)), 	[Currency7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency7]  DEFAULT ((0)), 	[Currency8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency8]  DEFAULT ((0)), 	[Currency9] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTemp_Currency9]  DEFAULT ((0)), 	[Currency10] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTemp_Currency10]  DEFAULT ((0)) ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [EntryTempSub] ( [Name1] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name1]  DEFAULT (''), 	[Name2] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name2]  DEFAULT (''), 	[Name3] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name3]  DEFAULT (''), 	[Name4] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name4]  DEFAULT (''), 	[Name5] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name5]  DEFAULT (''), 	[Name6] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name6]  DEFAULT (''), 	[name7] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_name7]  DEFAULT (''), 	[Name8] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name8]  DEFAULT (''), 	[Name9] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name9]  DEFAULT (''), 	[Name10] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name10]  DEFAULT (''), " & _
                                " [Date1] [smalldatetime] NULL, 	[Date2] [smalldatetime] NULL, 	[Date3] [smalldatetime] NULL, 	[Date4] [smalldatetime] NULL, 	[Date5] [smalldatetime] NULL, " & _
                                " [Int1] [int] NULL CONSTRAINT [DF_EntryTempSub_Int1]  DEFAULT ((0)),  	[Int2] [int] NULL CONSTRAINT [DF_EntryTempSub_Int2]  DEFAULT ((0)), 	[Int3] [int] NULL CONSTRAINT [DF_EntryTempSub_Int3]  DEFAULT ((0)), 	[Int4] [int] NULL CONSTRAINT [DF_EntryTempSub_Int4]  DEFAULT ((0)), 	[Int5] [int] NULL CONSTRAINT [DF_EntryTempSub_Int5]  DEFAULT ((0)), 	[Int6] [int] NULL CONSTRAINT [DF_EntryTempSub_Int6]  DEFAULT ((0)), 	[Int7] [int] NULL CONSTRAINT [DF_EntryTempSub_Int7]  DEFAULT ((0)), 	[Int8] [int] NULL CONSTRAINT [DF_EntryTempSub_Int8]  DEFAULT ((0)), 	[Int9] [int] NULL CONSTRAINT [DF_EntryTempSub_Int9]  DEFAULT ((0)), 	[Int10] [int] NULL CONSTRAINT [DF_EntryTempSub_Int10]  DEFAULT ((0)), " & _
                                " [Meters1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters1]  DEFAULT ((0)), 	[Meters2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters2]  DEFAULT ((0)),	[Meters3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters3]  DEFAULT ((0)), 	[Meters4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters4]  DEFAULT ((0)), 	[Meters5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters5]  DEFAULT ((0)), 	[Meters6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters6]  DEFAULT ((0)), 	[Meters7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters7]  DEFAULT ((0)), 	[Meters8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters8]  DEFAULT ((0)), 	[Meters9] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters9]  DEFAULT ((0)), 	[Meters10] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters10]  DEFAULT ((0)), " & _
                                " [Weight1] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight1]  DEFAULT ((0)), 	[Weight2] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight2]  DEFAULT ((0)), 	[Weight3] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight3]  DEFAULT ((0)), 	[Weight4] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight4]  DEFAULT ((0)), 	[Weight5] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight5]  DEFAULT ((0)), 	[Weight6] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight6]  DEFAULT ((0)), 	[Weight7] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight7]  DEFAULT ((0)), 	[Weight8] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight8]  DEFAULT ((0)), 	[Weight9] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight9]  DEFAULT ((0)), 	[Weight10] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight10]  DEFAULT ((0)), " & _
                                " [Currency1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency1]  DEFAULT ((0)), 	[Currency2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency2]  DEFAULT ((0)), 	[Currency3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency3]  DEFAULT ((0)), 	[Currency4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency4]  DEFAULT ((0)), 	[Currency5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency5]  DEFAULT ((0)), 	[Currency6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency6]  DEFAULT ((0)), 	[Currency7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency7]  DEFAULT ((0)), 	[Currency8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency8]  DEFAULT ((0)), 	[Currency9] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTempSub_Currency9]  DEFAULT ((0)), 	[Currency10] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTempSub_Currency10]  DEFAULT ((0)) ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Dc_No varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Dc_No  = '' where Dc_No  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Dc_Date varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Dc_Date   = '' where Dc_Date  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Ro_Division_Status tinyint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Ro_Division_Status = 0 where Ro_Division_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Charging_Quantity Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Charging_Quantity = 0 where Charging_Quantity is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Charging_Rate Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Charging_Rate  = 0 where Charging_Rate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Company_Head add Company_Bank_Ac_Details varchar(200) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Company_Head set Company_Bank_Ac_Details = '' where Company_Bank_Ac_Details is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_AlaisHead add Ledger_Type varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_AlaisHead set Ledger_Type = '' where Ledger_Type is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Ledger_AlaisHead add AccountsGroup_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Ledger_AlaisHead set AccountsGroup_IdNo = 0 where AccountsGroup_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Document_Through varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Document_Through = '' where Document_Through is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Document_Through varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Document_Through = '' where Document_Through is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Despatch_To varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Despatch_To  = '' where Despatch_To  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Lr_No varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Lr_No = '' where Lr_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Lr_Date varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Lr_Date  = '' where Lr_Date  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Booked_By varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Booked_By  = '' where Booked_By  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Transport_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Transport_IdNo = 0 where Transport_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Freight_ToPay_Amount Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Freight_ToPay_Amount = 0 where Freight_ToPay_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Details add Size_IdNo Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Details set Size_IdNo = 0 Where Size_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Sales_Head add Tax_Type varchar(35) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Sales_Head set Tax_Type = '' where Tax_Type is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE Waste_Head(Waste_IdNo smallint NOT NULL, Waste_Name varchar(50) NOT NULL, Sur_Name varchar(50) NOT NULL, [Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Waste_Head_Unit_IdNo]  DEFAULT ((0)), CONSTRAINT [PK_Waste_Head] PRIMARY KEY CLUSTERED ( Waste_IdNo ) ON [PRIMARY], CONSTRAINT [IX_Waste_Head] UNIQUE NONCLUSTERED  ([Sur_Name] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Spinning_WasteSales_Details]([Spinning_WasteSales_Code] [varchar](50) NOT NULL, [Company_IdNo] [smallint] NOT NULL, [Spinning_WasteSales_No] [varchar](20) NOT NULL, [for_OrderBy] [numeric](18, 2) NOT NULL, [Spinning_WasteSales_Date] [smalldatetime] NOT NULL, [Ledger_IdNo] [int] NOT NULL, [SL_No] [smallint] NOT NULL, [Waste_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Item_IdNo]  DEFAULT ((0)), [Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Unit_IdNo]  DEFAULT ((0)), [Packs] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Packs]  DEFAULT ((0)), 	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Weight]  DEFAULT ((0)), " & _
                         " [Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Rate]  DEFAULT ((0)), [Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Amount]  DEFAULT ((0)), [Pack_Nos] [varchar](500) NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Bag_Nos]  DEFAULT (''), CONSTRAINT [PK_Spinning_WasteSales_Details] PRIMARY KEY CLUSTERED  ( [Spinning_WasteSales_Code], [SL_No] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Spinning_WasteSales_Head]([Spinning_WasteSales_Code] [varchar](50) NOT NULL, [Company_IdNo] [smallint] NOT NULL, [Spinning_WasteSales_No] [varchar](50) NOT NULL, [for_OrderBy] [numeric](18, 2) NOT NULL, [Spinning_WasteSales_Date] [datetime] NOT NULL, [Ledger_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Ledger_IdNo]  DEFAULT ((0)), [SalesAc_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_SalesAc_IdNo]  DEFAULT ((0)), [TaxAc_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_TaxAc_IdNo]  DEFAULT ((0)), [CessAc_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_CessAc_IdNo]  DEFAULT ((0)), 	[Delivery_Address1] [varchar](50) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address1]  DEFAULT (''), " & _
                         " [Delivery_Address2] [varchar](50) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address2]  DEFAULT (''), 	[Delivery_Address3] [varchar](50) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address3]  DEFAULT (''), 	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Vehicle_No]  DEFAULT (''), 	[Removal_Date] [varchar](20) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Removal_Date]  DEFAULT (''), 	[Pack_Nos] [varchar](500) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Bag_Nos]  DEFAULT (''), 	[Total_Packs] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Total_Packs]  DEFAULT ((0)), 	[Total_Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Total_Weight]  DEFAULT ((0)), " & _
                         " [Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_GrossAmount]  DEFAULT ((0)), 	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_CashDiscount_Perc]  DEFAULT ((0)), [CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_CashDiscount_Amount]  DEFAULT ((0)), [Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Assessable_Value]  DEFAULT ((0)), [Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Tax_Perc]  DEFAULT ((0)), [Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Tax_Amount]  DEFAULT ((0)), [Cess_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Cess_Perc]  DEFAULT ((0)), [Cess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Cess_Amount]  DEFAULT ((0)), [AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_AddLess_Amount]  DEFAULT ((0)), " & _
                         " [Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Round_Off]  DEFAULT ((0)), [Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Net_Amount]  DEFAULT ((0)), CONSTRAINT [PK_Spinning_WasteSales_Head] PRIMARY KEY CLUSTERED  ( [Spinning_WasteSales_Code] ) ON [PRIMARY] ) ON [PRIMARY]"

        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Item_Head add Minimum_Stock Numeric(18, 3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Item_Head set Minimum_Stock = 0 where Minimum_Stock is Null"
        cmd.ExecuteNonQuery()

        cmd.Dispose()

        If vFldsChk_From_CompGroupCreation_Status = False Then

            Common_Procedures.Default_GroupHead_Updation(cn1)

            Common_Procedures.Default_LedgerHead_Updation(cn1)

            Common_Procedures.Default_MonthHead_Updation(cn1)

            Common_Procedures.Default_Shift_Updation(cn1)

            Common_Procedures.Default_StateHead_Updation(cn1)

            Common_Procedures.Default_Master_Updation(cn1)
            Common_Procedures.UpdateDefaultValuesForNewFields(cn1)

        End If

        cmd.Dispose()

        vFldsChk_All_Status = True
        Field_Check_PayRoll(cn1, FrmNm)

        FrmNm.Cursor = Cursors.Default
        If Trim(LCase(FrmNm.Name)) = "mdiparent1" Then
            MDIParent1.Cursor = Cursors.Default
        End If

        If vFldsChk_All_Status = False Then
            MessageBox.Show("Fields Verified", "FOR FIELDS CREATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        End If

    End Sub


    Public Shared Sub Field_Check_PayRoll(ByVal cn1 As SqlClient.SqlConnection, ByVal FrmNm As Form)
        Dim cn As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Nr As Long = 0

        On Error Resume Next

        FrmNm.Cursor = Cursors.WaitCursor
        If Trim(LCase(FrmNm.Name)) = "mdiparent1" Then
            MDIParent1.Cursor = Cursors.WaitCursor
        End If


        cmd.Connection = cn1

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Payroll_Employee_Incentive_Head](	[Incentive_Code] [varchar](30) NOT NULL,	[Company_IdNo] [smallint] NOT NULL," &
                          "[Incentive_No] [varchar](20) Not NULL,    [for_OrderBy] [numeric](18, 2) NOT NULL,	[Incentive_Date] [smalldatetime] NOT NULL," &
                          "[Day_Name] [varchar](50) NULL CONSTRAINT [DF_Payroll_Employee_Incentive_Head_Day_Name]  DEFAULT (''), CONSTRAINT [PK_Payroll_Employee_Incentive_Head] PRIMARY KEY CLUSTERED " &
                          "([Incentive_Code] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Payroll_Employee_Incentive_Details](	[Incentive_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NULL," &
                          "[Incentive_No] [varchar](30) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Incentive_Date] [smalldatetime] NOT NULL," &
                          "[Sl_No] [smallint] NOT NULL,	[Employee_IdNo] [smallint] NULL CONSTRAINT [DF_Payroll_Incentive_Details_Employee_IdNo]  DEFAULT ((0))," &
                          "[Incentive_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Incentive]  DEFAULT ((0))," &
                          "CONSTRAINT [PK_Payroll_Employee_Incentive_Details] PRIMARY KEY CLUSTERED (	[Incentive_Code] ASC,	[Sl_No] ASC) ON [PRIMARY])" &
                          "ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Payroll_Employee_PermissionLeaveTime_Head](	[Incentive_Code] [varchar](30) NOT NULL,	[Company_IdNo] [smallint] NOT NULL," &
                          "[Incentive_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Incentive_Date] [smalldatetime] NOT NULL," &
                          "[Day_Name] [varchar](50) NULL CONSTRAINT [DF_Payroll_Employee_PermissionLeaveTime_Head_Day_Name]  DEFAULT (''), CONSTRAINT [PK_Payroll_Employee_PermissionLeaveTime_Head] PRIMARY KEY CLUSTERED " &
                          "(	[Incentive_Code] ASC)ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [dbo].[Payroll_Employee_PermissionLeaveTime_Details](	[Incentive_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NULL," &
                          "[Incentive_No] [varchar](30) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Incentive_Date] [smalldatetime] NOT NULL," &
                          "[Sl_No] [smallint] NOT NULL,	[Employee_IdNo] [smallint] NULL CONSTRAINT [DF_Payroll_Employee_PermissionLeaveTime_Details_Employee_IdNo]  DEFAULT ((0))," &
                          "[PermissionLeaveTime_Minutes] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_PermissionLeaveTime_Minutes1]  DEFAULT ((0)),	[PermissionLeaveTime_Hours] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_PermissionLeaveTime_Hours1]  DEFAULT ((0))," &
                          "CONSTRAINT [PK_Payroll_Employee_PermissionLeaveTime_Details] PRIMARY KEY CLUSTERED (	[Incentive_Code] ASC,	[Sl_No] ASC) ON [PRIMARY])" &
                          "ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.commandtext = "Alter table PayRoll_Employee_Attendance_Details Add Permission_Absence_Duration numeric(6,3)"
        cmd.ExecuteNonQuery()

        cmd.commandtext = "Alter table Payoll_Employee_Head Add Current_Loan_EMI numeric(9,3) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Daily_Working_Head]([Reference_Code] [varchar](50) Not NULL,	[Reference_No] [varchar](50) Not NULL,	[Reference_Date] [smalldatetime] Not NULL,	[Employee_IdNo] [int] Not NULL,	[Start_Time] [datetime] NULL,	[Start_Time_Text] [varchar](20) NULL CONSTRAINT [DF_PayRoll_Employee_Daily_Working_Start_Time_Text]  DEFAULT (''), [End_Time] [datetime] NULL, 	[End_Time_Text] [varchar](20) NULL CONSTRAINT [DF_PayRoll_Employee_Daily_Working_End_Time_Text]  DEFAULT (''), 	[Work_Description] [varchar](1000) NULL CONSTRAINT [DF_PayRoll_Employee_Daily_Working_Work_Description]  DEFAULT (''),	[For_OrderBy] [numeric](18, 2) NOT NULL,	[Company_IdNo] [smallint] NOT NULL, " &
                                          "CONSTRAINT [PK_PayRoll_Employee_Daily_Working_Head] PRIMARY KEY CLUSTERED (	[Reference_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Deduction_Head add Mess_Amount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Deduction_Head set Mess_Amount = 0 Where Mess_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Late_Hours Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Late_Hours = 0 Where Late_Hours is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Late_Minutes Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Late_Minutes = 0 Where Late_Minutes is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add EarlyOut_Hours Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set EarlyOut_Hours = 0 Where EarlyOut_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add EarlyOut_Minutes Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set EarlyOut_Minutes = 0 Where EarlyOut_Minutes is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Category_Head add Office_TotalInHours_As_WorkedHours tinyint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Office_TotalInHours_As_WorkedHours = 0 Where Office_TotalInHours_As_WorkedHours is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Category_Head add Office_TotalInHours_As_WorkedHours_Status tinyint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Office_TotalInHours_As_WorkedHours_Status = 0 Where Office_TotalInHours_As_WorkedHours_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Category_Head](	[Category_IdNo] [smallint] NOT NULL,	[Category_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL,	[In_Time_Shift1] [numeric](18, 2) NULL,	[In_Time_Shift2] [numeric](18, 2) NULL,	[In_Time_Shift3] [numeric](18, 2) NULL,	[Lunch_minutes] [int] NULL,	[Fixed_Rotation] [varchar](20) NULL,	[OT_Allowed] [int] NULL,	[Time_Delay] [int] NULL,	[Attendance_Leave] [varchar](20) NULL,	[Week_Attendance_OT] [int] NULL,	[Attendance_Incentive] [int] NULL,	[Out_Time_Shift1] [numeric](18, 2) NULL,	[Out_Time_Shift2] [numeric](18, 2) NULL,	[Out_Time_Shift3] [numeric](18, 2) NULL,	[Monthly_Shift] [varchar](20) NULL,	[OT_Allowed_After_Minutes] [int] NULL,	[Minimum_Delay] [int] NULL,	[Festival_Holidays] [int] NULL,	[Incentive_Amount] [numeric](18, 2) NULL,	[Working_Hours1] [numeric](18, 2) NULL,	[Working_Hours2] [numeric](18, 2) NULL,	[Working_Hours3] [numeric](18, 2) NULL,	[No_Days_Month_Wages] [int] NULL,	[Week_Off_Credit] [int] NULL,	[Less_minute_Delay] [int] NULL,	[Production_Incentive] [int] NULL,	[Festival_Holidays_ot_Salary] [int] NULL,	[Incentive_Amount_Days] [numeric](18, 2) NULL,	[Shift1_In_Time] [varchar](30) NULL DEFAULT (''),	[Shift1_Out_Time] [varchar](30) NULL DEFAULT (''),	[Shift2_In_Time] [varchar](30) NULL DEFAULT (''),	[Shift2_Out_Time] [varchar](30) NULL DEFAULT (''),	[Shift3_In_Time] [varchar](30) NULL DEFAULT (''),	[Shift3_Out_Time] [varchar](30) NULL DEFAULT (''),	[Shift1_Working_Hours] [varchar](30) NULL DEFAULT (''),	[Shift2_Working_Hours] [varchar](30) NULL DEFAULT (''),	[Shift3_Working_Hours] [varchar](30) NULL DEFAULT (''),	[Leave_Salary_Less] [smallint] NULL DEFAULT ((0)),	[Att_Incentive_FromDays_Range1] [smallint] NULL DEFAULT ((0)),	[Att_Incentive_ToDays_Range1] [smallint] NULL DEFAULT ((0)),	[Att_Incentive_FromDays_Range2] [smallint] NULL DEFAULT ((0)),	[Att_Incentive_ToDays_Range2] [smallint] NULL DEFAULT ((0)),	[CL_Leave] [smallint] NULL DEFAULT ((0)),	[SL_Leave] [smallint] NULL DEFAULT ((0)),	[CL_Arrear_Type] [varchar](50) NULL DEFAULT (''),	[SL_Arrear_Type] [varchar](50) NULL DEFAULT (''),	[Shift1_Working_Minutes] [int] NULL DEFAULT ((0)),	[Shift2_Working_Minutes] [int] NULL DEFAULT ((0)),	[Shift3_Working_Minutes] [int] NULL DEFAULT ((0)),	[Shift1_In_DateTime] [datetime] NULL,	[Shift2_In_DateTime] [datetime] NULL,	[Shift3_In_DateTime] [datetime] NULL,	[Shift1_Out_DateTime] [datetime] NULL,	[Shift2_Out_DateTime] [datetime] NULL,	[Shift3_Out_DateTime] [datetime] NULL,	[Office_TotalInHours_As_WorkedHours] [tinyint] NULL DEFAULT ((0)),	[Office_TotalInHours_As_WorkedHours_Status] [tinyint] NULL DEFAULT ((0)),	[CL_Arrear_Type_Year] [varchar](50) NULL DEFAULT ((0)),	[SL_Arrear_Type_Year] [varchar](50) NULL DEFAULT ((0)),	[Week_Off_Allowance] [int] NULL DEFAULT ((0))," &
                         " CONSTRAINT [PK_PayRoll_Category_Head] PRIMARY KEY NONCLUSTERED ( [Category_IdNo]) ON [PRIMARY]," &
                         " CONSTRAINT [IX_PayRoll_Category_Head] UNIQUE NONCLUSTERED ( [Sur_Name]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift1_In_DateTime datetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift2_In_DateTime datetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift3_In_DateTime datetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift1_Out_DateTime datetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift2_Out_DateTime datetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift3_Out_DateTime datetime"
        cmd.ExecuteNonQuery()




        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift1_Working_Minutes int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift1_Working_Minutes = 0 Where Shift1_Working_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift2_Working_Minutes int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift2_Working_Minutes = 0 Where Shift2_Working_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift3_Working_Minutes int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift3_Working_Minutes = 0 Where Shift3_Working_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Attendance_Timing_Details](	[Employee_Attendance_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NULL,	[Employee_Attendance_No] [varchar](30) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Employee_Attendance_Date] [smalldatetime] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Employee_IdNo] [smallint] NULL DEFAULT ((0)),	[InOut_Type] [varchar](50) NULL DEFAULT (''),	[InOut_Time_Text] [varchar](50) NULL DEFAULT (''),	[InOut_DateTime] [datetime] NULL," &
                          " CONSTRAINT [PK_PayRoll_Attendance_Timing_Details] PRIMARY KEY NONCLUSTERED (	[Employee_Attendance_Code] ,	[Sl_No] ) ON [PRIMARY]," &
                           " CONSTRAINT [IX_PayRoll_Attendance_Timing_Details_1] UNIQUE NONCLUSTERED (	[Company_IdNo] ,	[Employee_Attendance_Date],	[Employee_IdNo],	[InOut_DateTime] ) ON [PRIMARY]," &
                            " CONSTRAINT [IX_PayRoll_Attendance_Timing_Details_2] UNIQUE NONCLUSTERED (	[Employee_Attendance_Code] ,	[Employee_IdNo] ,	[InOut_DateTime] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Payroll_Timing_Addition_Details](	[Timing_Addition_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NULL,	[Timing_Addition_No] [varchar](30) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Timing_Addition_Date] [smalldatetime] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Employee_IdNo] [smallint] NULL DEFAULT ((0)),	[InOut_Type] [varchar](50) NULL DEFAULT (''),	[InOut_Time_Text] [varchar](50) NULL DEFAULT (''),	[InOut_DateTime] [datetime] NULL," &
                           " CONSTRAINT [PK_Payroll_Timing_Addition_Details] PRIMARY KEY NONCLUSTERED (	[Timing_Addition_Code] ,	[Sl_No]) ON [PRIMARY]," &
                           " CONSTRAINT [IX_Payroll_Timing_Addition_Details_1] UNIQUE NONCLUSTERED (	[Company_IdNo] ,	[Employee_IdNo],	[InOut_DateTime]) ON [PRIMARY]," &
                           " CONSTRAINT [IX_Payroll_Timing_Addition_Details_2] UNIQUE NONCLUSTERED (	[Timing_Addition_Code] 	,[Employee_IdNo],	[InOut_DateTime]) ON [PRIMARY]) ON [PRIMARY]"

        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Payroll_AttendanceLog_FromMachine_Details](	[AttendanceLog_FromMachine_Code] [varchar](30) NOT NULL,	[AttendanceLog_FromMachine_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[AttendanceLog_FromMachine_Date] [smalldatetime] NOT NULL,	[Sl_No] [int] NOT NULL,	[Employee_CardNo] [varchar](50) NULL CONSTRAINT [DF_Payroll_AttendanceLog_FromMachine_Details_Employee_CardNo]  DEFAULT (''),	[IN_Out] [varchar](30) NULL CONSTRAINT [DF_Payroll_AttendanceLog_FromMachine_Details_IN_Out]  DEFAULT (''),	[INOut_DateTime_Text] [varchar](50) NULL CONSTRAINT [DF_Payroll_AttendanceLog_FromMachine_Details_INOut_DateTime_Text]  DEFAULT (''),	[INOut_DateTime] [datetime] NULL," &
                      " CONSTRAINT [PK_Payroll_AttendanceLog_FromMachine_Details] PRIMARY KEY CLUSTERED (	[AttendanceLog_FromMachine_Code] ,	[Sl_No]) ON [PRIMARY]," &
                        " CONSTRAINT [IX_Payroll_AttendanceLog_FromMachine_Details] UNIQUE NONCLUSTERED (	[Employee_CardNo],	[INOut_DateTime]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Payroll_AttendanceLog_FromMachine_Head]([AttendanceLog_FromMachine_Code] [varchar](30) NOT NULL,	[AttendanceLog_FromMachine_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[AttendanceLog_FromMachine_Date] [smalldatetime] NOT NULL, CONSTRAINT [PK_Payroll_AttendanceLog_FromMachine_Head] PRIMARY KEY CLUSTERED (        [AttendanceLog_FromMachine_Code]) ON [PRIMARY], CONSTRAINT [IX_Payroll_AttendanceLog_FromMachine_Head] UNIQUE NONCLUSTERED (  [AttendanceLog_FromMachine_Date]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Other_Deduction1 int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Other_Deduction1 = 0 Where Other_Deduction1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Week_Off_Allowance int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Week_Off_Allowance = 0 Where Week_Off_Allowance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Week_Off_Allowance int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Week_Off_Allowance = 0 Where Week_Off_Allowance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Actual_Salary Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Actual_Salary = 0 Where Actual_Salary is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Week_Off_Allowance Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Week_Off_Allowance = 0 Where Week_Off_Allowance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Other_Addition1 Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Other_Addition1 = 0 Where Other_Addition1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Other_Addition2 Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Other_Addition2 = 0 Where Other_Addition2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Other_Addition1 Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Other_Addition1 = 0 Where Other_Addition1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Other_Addition2 Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Other_Addition2 = 0 Where Other_Addition2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Week_Off_Allowance Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Week_Off_Allowance = 0 Where Week_Off_Allowance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Ded_Caption1 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Ded_Caption1  = '' where Ded_Caption1   is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Ded_Caption2  varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Ded_Caption2  = '' where Ded_Caption2   is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Ded_Caption3  varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Ded_Caption3  = '' where Ded_Caption3   is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Add_Caption1 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Add_Caption1 = '' where Add_Caption1  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Add_Caption2 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Add_Caption2 = '' where Add_Caption2  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Add_Caption3 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Add_Caption3 = '' where Add_Caption3  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Add_Caption4 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Add_Caption4 = '' where Add_Caption4  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Minus_MainAdvance Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Minus_MainAdvance = 0 where Minus_MainAdvance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Salary_Pending Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Salary_Pending = 0 where Salary_Pending is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Releave_Details](	[Employee_IdNo] [int] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Join_Date] [varchar](50) NULL,	[Releave_Date] [varchar](50) NULL,	[Join_DateTime] [smalldatetime] NULL,	[Releave_DateTime] [smalldatetime] NULL,	[Reason] [varchar](200) NULL, CONSTRAINT [PK_PayRoll_Employee_Releave_Details] PRIMARY KEY CLUSTERED (	[Employee_IdNo] ,       [Sl_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Working_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Working_Hours = 0 where Working_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_Minutes int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_Minutes = 0 Where Shift_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_Hours = 0 where Shift_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add CL Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set CL = 0 Where CL is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add SL Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set SL = 0 Where SL is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add From_DateTime smalldatetime"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add To_DateTime smalldatetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Head add Salary_Payment_Type_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Head set Salary_Payment_Type_IdNo = 0 Where Salary_Payment_Type_IdNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Salary_Shift Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Salary_Shift = 0 Where Salary_Shift is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Incentive_Amount Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Incentive_Amount = 0 Where Incentive_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Total_Salary Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Total_Salary = 0 Where Total_Salary is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add OT_Minutes Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set OT_Minutes = 0 Where OT_Minutes is Null"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Salary_Head", "Salary_Type")
        cmd.CommandText = "Alter table PayRoll_Salary_Head Drop Column Salary_Type"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Head", "Scheme_Total_salary")
        cmd.CommandText = "Alter table PayRoll_Employee_Head Drop Column Scheme_Total_salary"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Head", "Scheme_Total_EsiPf")
        cmd.CommandText = "Alter table PayRoll_Employee_Head Drop Column Scheme_Total_EsiPf"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Head", "Scheme_Total_Ot")
        cmd.CommandText = "Alter table PayRoll_Employee_Head Drop Column Scheme_Total_Ot"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Voucher_Head DROP CONSTRAINT IX_Voucher_Head"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table Voucher_Details DROP CONSTRAINT IX_Voucher_Details"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "drop table Working_Type_Head"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Category_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Category_IdNo = 0 where Category_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Time Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Time = 0 where Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_IdNo = 0 where Shift_IdNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Add_Less_Minutes Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Add_Less_Minutes = 0 where Add_Less_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_Minutes Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_Minutes = 0 where Shift_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_Hours = 0 where Shift_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add No_Of_Minutes Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set No_Of_Minutes = 0 where No_Of_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add No_Of_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set No_Of_Hours = 0 where No_Of_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add In_Out_Timings varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set In_Out_Timings = '' where In_Out_Timings  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Payroll_Timing_Addition_Head]([Timing_Addition_Code] [varchar](30) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Timing_Addition_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Timing_Addition_Date] [smalldatetime] NOT NULL,[Day_Name] [varchar](50) NULL default ('')," &
                            "CONSTRAINT [PK_Payroll_Timing_Addition_Head] PRIMARY KEY CLUSTERED ([Timing_Addition_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Category_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Category_IdNo = 0 Where Category_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Join_DateTime smalldatetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Releave_DateTime smalldatetime"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Head", "Working_Type_IdNo")
        cmd.CommandText = "Alter table PayRoll_Employee_Head Drop Column Working_Type_IdNo"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Category_Details] ( [Category_IdNo] [int] NOT NULL, [Sl_No] [smallint] NOT NULL, [From_Attendance] [int] NULL  DEFAULT (0) , [To_Attendance] [int] NULL  DEFAULT (0) , [Amount] [numeric](18, 2) NULL  DEFAULT (0) ,  CONSTRAINT [PK_PayRoll_Category_Details] PRIMARY KEY CLUSTERED  ( [Category_IdNo] ,  [Sl_No] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "CREATE TABLE [PayRoll_Category_Head] ( [Category_IdNo] [smallint] NOT NULL, [Category_Name] [varchar](50) NOT NULL, [Sur_Name] [varchar](50) NOT NULL, [In_Time_Shift1] [numeric](18, 2) NULL, [In_Time_Shift2] [numeric](18, 2) NULL, [In_Time_Shift3] [numeric](18, 2) NULL, [Lunch_minutes] [int] NULL, [Fixed_Rotation] [varchar](20) NULL, [OT_Allowed] [int] NULL, [Time_Delay] [int] NULL, [Attendance_Leave] [varchar](20) NULL, [Week_Attendance_OT] [int] NULL, [Attendance_Incentive] [int] NULL, [Out_Time_Shift1] [numeric](18, 2) NULL, [Out_Time_Shift2] [numeric](18, 2) NULL, [Out_Time_Shift3] [numeric](18, 2) NULL, [Monthly_Shift] [varchar](20) NULL, [OT_Allowed_After_Minutes] [int] NULL, " &
                         " [Minimum_Delay] [int] NULL, 	[Festival_Holidays] [int] NULL, 	[Incentive_Amount] [numeric](18, 2) NULL, 	[Working_Hours1] [numeric](18, 2) NULL,  [Working_Hours2] [numeric](18, 2) NULL, [Working_Hours3] [numeric](18, 2) NULL, [No_Days_Month_Wages] [int] NULL, [Week_Off_Credit] [int] NULL, [Less_minute_Delay] [int] NULL, [Production_Incentive] [int] NULL, [Festival_Holidays_ot_Salary] [int] NULL, [Incentive_Amount_Days] [numeric](18, 2) NULL, " &
                         " CONSTRAINT [PK_PayRoll_Category_Head] PRIMARY KEY NONCLUSTERED ( [Category_IdNo]  ) ON [PRIMARY] ,   CONSTRAINT [IX_PayRoll_Category_Head] UNIQUE NONCLUSTERED ( [Sur_Name] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift1_In_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift1_In_Time = '' where Shift1_In_Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift1_Out_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift1_Out_Time = '' where Shift1_Out_Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift2_In_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift2_In_Time = '' where Shift2_In_Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Add_Caption5 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Add_Caption5 = '' where Add_Caption5  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Add_Caption6 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Add_Caption6 = '' where Add_Caption6  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Add_Caption7 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Add_Caption7 = '' where Add_Caption7  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Add_Caption8 varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Add_Caption8 = '' where Add_Caption8  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Other_Addition2 INT default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Other_Addition2 = 0 where Other_Addition2  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Other_Addition3 INT default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Other_Addition2 = 0 where Other_Addition2  is Null"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter table Shift_Head add Total_Hours Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Shift_Head set Total_Hours = 0 Where Total_Hours is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Shift_Head add Total_Minutes Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Shift_Head set Total_Minutes = 0 Where Total_Minutes is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [EntryTemp_Simple](	[Name1] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name1]  DEFAULT (''),	[Name2] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name2]  DEFAULT (''),	[Name3] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name3]  DEFAULT (''),	[Name4] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name4]  DEFAULT (''),	[Name5] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name5]  DEFAULT (''),	[Name6] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name6]  DEFAULT (''),	[name7] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_name7]  DEFAULT (''),	[Name8] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name8]  DEFAULT (''),	[Name9] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name9]  DEFAULT (''),	[Name10] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Simple_Name10]  DEFAULT (''),	[Date1] [smalldatetime] NULL,	[Date2] [smalldatetime] NULL,	[Date3] [smalldatetime] NULL,	[Date4] [smalldatetime] NULL,	[Date5] [smalldatetime] NULL,	[Int1] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int1]  DEFAULT ((0)),	[Int2] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int2]  DEFAULT ((0)),	[Int3] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int3]  DEFAULT ((0)),	[Int4] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int4]  DEFAULT ((0)),	[Int5] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int5]  DEFAULT ((0)),	[Int6] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int6]  DEFAULT ((0)),	[Int7] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int7]  DEFAULT ((0)),	[Int8] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int8]  DEFAULT ((0)),	[Int9] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int9]  DEFAULT ((0)),	[Int10] [int] NULL CONSTRAINT [DF_EntryTemp_Simple_Int10]  DEFAULT ((0)),	[Meters1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters1]  DEFAULT ((0)),	[Meters2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters2]  DEFAULT ((0)),	[Meters3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters3]  DEFAULT ((0)),	[Meters4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters4]  DEFAULT ((0)),	[Meters5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters5]  DEFAULT ((0)),	[Meters6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters6]  DEFAULT ((0)),	[Meters7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters7]  DEFAULT ((0)),	[Meters8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters8]  DEFAULT ((0)),	[Meters9] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters9]  DEFAULT ((0)),	[Meters10] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Meters10]  DEFAULT ((0)),	[Weight1] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight1]  DEFAULT ((0)),	[Weight2] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight2]  DEFAULT ((0)),	[Weight3] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight3]  DEFAULT ((0)),	[Weight4] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight4]  DEFAULT ((0)),	[Weight5] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight5]  DEFAULT ((0)),	[Weight6] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight6]  DEFAULT ((0)),	[Weight7] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight7]  DEFAULT ((0)),	[Weight8] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight8]  DEFAULT ((0)),	[Weight9] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight9]  DEFAULT ((0)),	[Weight10] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Simple_Weight10]  DEFAULT ((0)),	[Currency1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency1]  DEFAULT ((0)),	[Currency2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency2]  DEFAULT ((0)),	[Currency3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency3]  DEFAULT ((0)),	[Currency4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency4]  DEFAULT ((0)),	[Currency5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency5]  DEFAULT ((0)),	[Currency6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency6]  DEFAULT ((0)),	[Currency7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency7]  DEFAULT ((0)),	[Currency8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency8]  DEFAULT ((0)),	[Currency9] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency9]  DEFAULT ((0)),	[Currency10] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTemp_Simple_Currency10]  DEFAULT ((0))) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Provision Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Provision = 0 Where Provision is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Provision int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Provision = 0 Where Provision is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add late_Mins int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set late_Mins = 0 Where late_Mins is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Settings add Late_Hours_Salary int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Settings set Late_Hours_Salary = 0 Where Late_Hours_Salary is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Leave_Salary_Less Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Leave_Salary_Less = 0 Where Leave_Salary_Less is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Provision Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Provision = 0 Where Provision is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Late_Mins Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Late_Hours = 0 Where Late_Hours is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Late_Hours_Salary Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Late_Hours_Salary = 0 Where Late_Hours_Salary is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [PayRoll_Settings]([Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,[Employee_IdNo] [int] NULL,[Basic_Salary] [int] NULL,[Total_Days] [int] NULL,[Net_Pay] [int] NULL,[No_Of_Attendance_Days] [int] NULL,[From_W_off_CR] [int] NULL,[Festival_Holidays] [int] NULL,[No_Of_Leave] [int] NULL,[Attendance_On_W_Off_FH] [int] NULL,[Op_W_Off_CR] [int] NULL,[Add_W_Off_CR] [int] NULL,[Less_W_Off_CR] [int] NULL,[Total_W_Off_CR] [int] NULL,[Salary_Days] [int] NULL,[Basic_Pay] [int] NULL,[D_A] [int] NULL,[Earning] [int] NULL,[H_R_A] [int] NULL,[Conveyance] [int] NULL,[Washing] [int] NULL,[Entertainment] [int] NULL,[Maintenance] [int] NULL,[Other_Addition] [int] NULL,[Total_Addition] [int] NULL,[Mess] [int] NULL,[Medical] [int] NULL,[Store] [int] NULL,[ESI] [int] NULL,[P_F] [int] NULL,[E_P_F] [int] NULL,[Pension_Scheme] [int] NULL,[Other_Deduction] [int] NULL,[Total_Deduction] [int] NULL,[Attendance_Incentive] [int] NULL,[Net_Salary] [int] NULL,[Advance] [int] NULL,[Day_For_Bonus] [int] NULL,[Earning_For_Bonus] [int] NULL,[Working_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Working_Hours]  DEFAULT ((0)),[Salary_Shift] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Shift]  DEFAULT ((0)),[Incentive_Amount] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Incentive_Amount]  DEFAULT ((0)),[Total_Salary] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Salary]  DEFAULT ((0)),[OT_Minutes] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OT_Minutes]  DEFAULT ((0)),[OT_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OT_Hours]  DEFAULT ((0)),[Ot_Pay_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Ot_Pay_Hours]  DEFAULT ((0)),[Ot_Salary] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Ot_Salary]  DEFAULT ((0)),[Minus_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Minus_Advance]  DEFAULT ((0)),[Balance_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Balance_Advance]  DEFAULT ((0)),[Net_Pay_Amount] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Net_Pay_Amount]  DEFAULT ((0)),[Minus_MainAdvance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Minus_MainAdvance]  DEFAULT ((0)),[Salary_Pending] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Pending]  DEFAULT ((0)),[Total_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_SL_CR_Days]  DEFAULT ((0)),[Less_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Less_SL_CR_Days]  DEFAULT ((0)),[OP_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OP_SL_CR_Days]  DEFAULT ((0)),[Total_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_CL_CR_Days]  DEFAULT ((0)),[Less_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Less_CL_CR_Days]  DEFAULT ((0)),[OP_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OP_CL_CR_Days]  DEFAULT ((0)),[From_Cl_For_Leave] [int] NULL CONSTRAINT [DF_PayRoll_Settings_From_Cl_For_Leave]  DEFAULT ((0)),[From_SL_For_Leave] [int] NULL CONSTRAINT [DF_PayRoll_Settings_From_SL_For_Leave]  DEFAULT ((0)),[Total_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Advance]  DEFAULT ((0)),[Salary_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Advance]  DEFAULT ((0)),[Total_Leave_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Leave_Days]  DEFAULT ((0))," &
                          "CONSTRAINT [PK_PayRoll_Settings] PRIMARY KEY CLUSTERED ([Auto_SlNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = ""
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table PayRoll_Salary_Details add Net_Pay_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        Nr = 0
        cmd.CommandText = "Update PayRoll_Salary_Details set Net_Pay_Amount = Net_Salary Where Net_Pay_Amount is Null"
        Nr = cmd.ExecuteNonQuery()
        If Nr > 0 Then
            cmd.CommandText = "Update PayRoll_Salary_Details set Net_Salary = round((Total_Salary-Mess),0)"
            cmd.ExecuteNonQuery()
        End If

        cmd.CommandText = "Alter table PayRoll_Salary_Head add Advance_UptoDate SmalldateTime"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Head set Advance_UptoDate = To_Date Where Advance_UptoDate is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Mess_Attendance numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Mess_Attendance = 1 Where Mess_Attendance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Wages_Amount Numeric(18, 2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Wages_Amount = 0 Where Wages_Amount is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [PayRoll_Settings]([Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,[Employee_IdNo] [int] NULL,[Basic_Salary] [int] NULL,[Total_Days] [int] NULL,[Net_Pay] [int] NULL,[No_Of_Attendance_Days] [int](18, 3) NULL,[From_W_off_CR] [int] NULL,[Festival_Holidays] [int] NULL,[No_Of_Leave] [int] NULL,[Attendance_On_W_Off_FH] [int] NULL,[Op_W_Off_CR] [int] NULL,[Add_W_Off_CR] [int] NULL,[Less_W_Off_CR] [int] NULL,[Total_W_Off_CR] [int] NULL,[Salary_Days] [int] NULL,[Basic_Pay] [int] NULL,[D_A] [int] NULL,[Earning] [int] NULL,[H_R_A] [int] NULL,[Conveyance] [int] NULL,[Washing] [int] NULL,[Entertainment] [int] NULL,[Maintenance] [int] NULL,[Other_Addition] [int] NULL,[Total_Addition] [int] NULL,[Mess] [int] NULL,[Medical] [int] NULL,[Store] [int] NULL,[ESI] [int] NULL,[P_F] [int] NULL,[E_P_F] [int] NULL,[Pension_Scheme] [int] NULL,[Other_Deduction] [int] NULL,[Total_Deduction] [int] NULL,[Attendance_Incentive] [int] NULL,[Net_Salary] [int] NULL,[Advance] [int] NULL,[Day_For_Bonus] [int] NULL,[Earning_For_Bonus] [int] NULL,[Working_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Working_Hours]  DEFAULT ((0)),[Salary_Shift] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Shift]  DEFAULT ((0)),[Incentive_Amount] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Incentive_Amount]  DEFAULT ((0)),[Total_Salary] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Salary]  DEFAULT ((0)),[OT_Minutes] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OT_Minutes]  DEFAULT ((0)),[OT_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OT_Hours]  DEFAULT ((0)),[Ot_Pay_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Ot_Pay_Hours]  DEFAULT ((0)),[Ot_Salary] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Ot_Salary]  DEFAULT ((0)),[Minus_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Minus_Advance]  DEFAULT ((0)),[Balance_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Balance_Advance]  DEFAULT ((0)),[Net_Pay_Amount] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Net_Pay_Amount]  DEFAULT ((0)),[Minus_MainAdvance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Minus_MainAdvance]  DEFAULT ((0)),[Salary_Pending] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Pending]  DEFAULT ((0)),[Total_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_SL_CR_Days]  DEFAULT ((0)),[Less_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Less_SL_CR_Days]  DEFAULT ((0)),[OP_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OP_SL_CR_Days]  DEFAULT ((0)),[Total_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_CL_CR_Days]  DEFAULT ((0)),[Less_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Less_CL_CR_Days]  DEFAULT ((0)),[OP_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OP_CL_CR_Days]  DEFAULT ((0)),[From_Cl_For_Leave] [int] NULL CONSTRAINT [DF_PayRoll_Settings_From_Cl_For_Leave]  DEFAULT ((0)),[From_SL_For_Leave] [int] NULL CONSTRAINT [DF_PayRoll_Settings_From_SL_For_Leave]  DEFAULT ((0)),[Total_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Advance]  DEFAULT ((0)),[Salary_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Advance]  DEFAULT ((0)),[Total_Leave_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Leave_Days]  DEFAULT ((0))," &
                              "CONSTRAINT [PK_PayRoll_Settings] PRIMARY KEY CLUSTERED ([Auto_SlNo](Asc)) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [Holiday_Details](	[Year_Code] [varchar](50) NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Holiday_Date] [varchar](50) NULL CONSTRAINT [DF_Table_1_Count_IdNo_2]  DEFAULT (''),	[HolidayDateTime] [smalldatetime] NOT NULL,	[Reason] [varchar](100) NULL," &
                           " CONSTRAINT [PK_Holiday_Details] PRIMARY KEY CLUSTERED ([Year_Code] ,[Sl_No]) ON [PRIMARY], " &
                           " CONSTRAINT [IX_Holiday_Details] UNIQUE NONCLUSTERED ( [HolidayDateTime]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "CREATE TABLE [Holiday_Details](	[Sl_No] [smallint] NOT NULL,	[Holiday_Date] [varchar](50) NULL CONSTRAINT [DF_Table_1_Count_IdNo_2]  DEFAULT (''),	[Reason] [varchar](100) NULL," & _
        '                   " CONSTRAINT [PK_Holiday_Details] PRIMARY KEY CLUSTERED ([Sl_No]) ON [PRIMARY]) ON [PRIMARY]"
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Holiday_Details add HolidayDateTime smalldatetime"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Employee_Payment_Head add Voucher_No varchar(30) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Payment_Head add Voucher_Code varchar(30) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Payment_Head add for_orderbyVoucher Numeric(18,2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table PayRoll_Category_Head add CL_Arrear_Type_Year varchar(50) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set CL_Arrear_Type_Year = '' where CL_Arrear_Type_Year is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add SL_Arrear_Type_Year varchar(50) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set SL_Arrear_Type_Year  = 0 where SL_Arrear_Type_Year  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Add_CL_Leaves Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Add_CL_Leaves = 0 where Add_CL_Leaves is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Add_SL_Leaves Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Add_SL_Leaves  = 0 where Add_SL_Leaves  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Opening_SalaryFor_Bonus Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Opening_SalaryFor_Bonus = 0 where Opening_SalaryFor_Bonus is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Opening_CL_Leaves Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Opening_CL_Leaves = 0 where Opening_CL_Leaves is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Opening_ML_Leaves Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Opening_ML_Leaves = 0 where Opening_ML_Leaves is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Opening_WeekOff_Credits Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Opening_WeekOff_Credits = 0 where Opening_WeekOff_Credits is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [PayRoll_Category_Details]([Category_IdNo] [int] NOT NULL,[Sl_No] [smallint] NOT NULL,[From_Attendance] [int] NULL CONSTRAINT [DF_PayRoll_Category_Details_From_Attendance]  DEFAULT ((0)),[To_Attendance] [int] NULL CONSTRAINT [DF_PayRoll_Category_Details_To_Attendance]  DEFAULT ((0)),[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_PayRoll_Category_Details_Amount]  DEFAULT ((0))," &
         "CONSTRAINT [PK_PayRoll_Category_Details] PRIMARY KEY CLUSTERED ([Category_IdNo] ASC,[Sl_No] ASC) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Employee_Deduction_Head add Mess Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Deduction_Head set Mess = 0 where Mess is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Deduction_Head add Medical Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Deduction_Head set Medical = 0 where Medical is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Employee_Deduction_Head add Store Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Deduction_Head set Store = 0 where Store is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Deduction_Head add Other_Addition Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Deduction_Head set Other_Addition = 0 where Other_Addition is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Deduction_Head add Other_Deduction Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Deduction_Head set Other_Deduction = 0 where Other_Deduction is Null"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Alter table PayRoll_Salary_Details add Total_Leave_Days Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Total_Leave_Days = 0 where Total_Leave_Days is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Salary_Advance Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Salary_Advance = 0 where Salary_Advance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Total_Advance Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Total_Advance = 0 where Total_Advance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add From_SL_For_Leave Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set From_SL_For_Leave = 0 where From_SL_For_Leave is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add From_Cl_For_Leave Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set From_Cl_For_Leave = 0 where From_Cl_For_Leave is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Total_Addition Numeric(18,3) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Total_Addition = 0 where Total_Addition is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Total_SL_CR_Days Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Total_SL_CR_Days = 0 where Total_SL_CR_Days is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Less_SL_CR_Days Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Less_SL_CR_Days = 0 where Less_SL_CR_Days is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add OP_SL_CR_Days Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set OP_SL_CR_Days = 0 where OP_SL_CR_Days is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Total_CL_CR_Days Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Total_CL_CR_Days = 0 where Total_CL_CR_Days is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Less_CL_CR_Days Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Less_CL_CR_Days = 0 where Less_CL_CR_Days is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add OP_CL_CR_Days Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set OP_CL_CR_Days = 0 where OP_CL_CR_Days is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Minus_MainAdvance Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Minus_MainAdvance = 0 where Minus_MainAdvance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Salary_Pending Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Salary_Pending = 0 where Salary_Pending is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Releave_Details](	[Employee_IdNo] [int] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[Join_Date] [varchar](50) NULL,	[Releave_Date] [varchar](50) NULL,	[Join_DateTime] [smalldatetime] NULL,	[Releave_DateTime] [smalldatetime] NULL,	[Reason] [varchar](200) NULL, CONSTRAINT [PK_PayRoll_Employee_Releave_Details] PRIMARY KEY CLUSTERED (	[Employee_IdNo] ,       [Sl_No]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Working_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Working_Hours = 0 where Working_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_Minutes int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_Minutes = 0 Where Shift_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_Hours = 0 where Shift_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add CL Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set CL = 0 Where CL is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add SL Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set SL = 0 Where SL is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add From_DateTime smalldatetime"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add To_DateTime smalldatetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Head add Salary_Payment_Type_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Head set Salary_Payment_Type_IdNo = 0 Where Salary_Payment_Type_IdNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Salary_Details add Salary_Shift Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Salary_Shift = 0 Where Salary_Shift is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Incentive_Amount Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Incentive_Amount = 0 Where Incentive_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Total_Salary Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Total_Salary = 0 Where Total_Salary is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add OT_Minutes Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set OT_Minutes = 0 Where OT_Minutes is Null"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Salary_Head", "Salary_Type")
        cmd.CommandText = "Alter table PayRoll_Salary_Head Drop Column Salary_Type"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Head", "Scheme_Total_salary")
        cmd.CommandText = "Alter table PayRoll_Employee_Head Drop Column Scheme_Total_salary"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Head", "Scheme_Total_EsiPf")
        cmd.CommandText = "Alter table PayRoll_Employee_Head Drop Column Scheme_Total_EsiPf"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Head", "Scheme_Total_Ot")
        cmd.CommandText = "Alter table PayRoll_Employee_Head Drop Column Scheme_Total_Ot"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Voucher_Head DROP CONSTRAINT IX_Voucher_Head"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table Voucher_Details DROP CONSTRAINT IX_Voucher_Details"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "drop table Working_Type_Head"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Category_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Category_IdNo = 0 where Category_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Time Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Time = 0 where Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_IdNo = 0 where Shift_IdNo is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Add_Less_Minutes Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Add_Less_Minutes = 0 where Add_Less_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_Minutes Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_Minutes = 0 where Shift_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Shift_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Shift_Hours = 0 where Shift_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add No_Of_Minutes Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set No_Of_Minutes = 0 where No_Of_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add No_Of_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set No_Of_Hours = 0 where No_Of_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add In_Out_Timings varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set In_Out_Timings = '' where In_Out_Timings  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Payroll_Timing_Addition_Head]([Timing_Addition_Code] [varchar](30) NOT NULL,[Company_IdNo] [smallint] NOT NULL,[Timing_Addition_No] [varchar](20) NOT NULL,[for_OrderBy] [numeric](18, 2) NOT NULL,[Timing_Addition_Date] [smalldatetime] NOT NULL,[Day_Name] [varchar](50) NULL default ('')," &
                            "CONSTRAINT [PK_Payroll_Timing_Addition_Head] PRIMARY KEY CLUSTERED ([Timing_Addition_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Category_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Category_IdNo = 0 Where Category_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Join_DateTime smalldatetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Releave_DateTime smalldatetime"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Head", "Working_Type_IdNo")
        cmd.CommandText = "Alter table PayRoll_Employee_Head Drop Column Working_Type_IdNo"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift1_In_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift1_In_Time = '' where Shift1_In_Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift1_Out_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift1_Out_Time = '' where Shift1_Out_Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift2_In_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift2_In_Time = '' where Shift2_In_Time is Null"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift2_Out_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift2_Out_Time = '' where Shift2_Out_Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift3_In_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift3_In_Time = '' where Shift3_In_Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift3_Out_Time varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift3_Out_Time = '' where Shift3_Out_Time is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift1_Working_Hours varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift1_Working_Hours = '' where Shift1_Working_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift2_Working_Hours varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift2_Working_Hours = '' where Shift2_Working_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Shift3_Working_Hours varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Shift3_Working_Hours = '' where Shift3_Working_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Leave_Salary_Less smallint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Leave_Salary_Less = 0 where Leave_Salary_Less is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Att_Incentive_FromDays_Range1 smallint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Att_Incentive_FromDays_Range1 = 0 where Att_Incentive_FromDays_Range1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Att_Incentive_ToDays_Range1 smallint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Att_Incentive_ToDays_Range1 = 0 where Att_Incentive_ToDays_Range1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Att_Incentive_FromDays_Range2 smallint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Att_Incentive_FromDays_Range2 = 0 where Att_Incentive_FromDays_Range2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add Att_Incentive_ToDays_Range2 smallint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set Att_Incentive_ToDays_Range2 = 0 where Att_Incentive_ToDays_Range2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add CL_Leave smallint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set CL_Leave = 0 where CL_Leave is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add SL_Leave smallint default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set SL_Leave = 0 where SL_Leave is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add CL_Arrear_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set CL_Arrear_Type = '' where CL_Arrear_Type  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Category_Head add SL_Arrear_Type varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Category_Head set SL_Arrear_Type = '' where SL_Arrear_Type  is Null"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Payment_Head", "Cash_Check")
        cmd.CommandText = "Alter table PayRoll_Employee_Payment_Head Drop Column Cash_Check"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Payment_Head add Cash_Cheque varchar(20) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Payment_Head set Cash_Cheque = '' where Cash_Cheque is Null"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Attendance_Details", "Working_Type_Name")
        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details Drop Column Working_Type_Name"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add No_Of_Shift numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set No_Of_Shift = 0 where No_Of_Shift is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Warp_Count_Coolie_Details] ( [Sl_No] [smallint] NOT NULL, [Count_IdNo] [int] NOT NULL, [Value] [numeric](18, 2) NULL DEFAULT (0) , CONSTRAINT [PK_PayRoll_Warp_Count_Coolie_Details] PRIMARY KEY CLUSTERED  ( [Count_IdNo] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Wages_Head] ( [Employee_Wages_Code] [varchar](50) NOT NULL, [Company_IdNo] [smallint] NOT NULL, [Employee_Wages_No] [varchar](50) NOT NULL, [for_OrderBy] [numeric](18, 2) NOT NULL, [Employee_IdNo] [int] NOT NULL, [Front_Warper] [numeric](18, 2) NULL DEFAULT (0) , [Back_Warper] [numeric](18, 2) NULL DEFAULT (0) , [Helper] [numeric](18, 2) NULL DEFAULT (0) , [Front_Sizer] [numeric](18, 2) NULL DEFAULT (0) , [Back_Sizer] [numeric](18, 2) NULL DEFAULT (0) , [Boiler] [numeric](18, 2) NULL DEFAULT (0) , [Cooker] [numeric](18, 2) NULL DEFAULT (0) ,  CONSTRAINT [PK_PayRoll_Employee_Wages_Head] PRIMARY KEY CLUSTERED  ( [Employee_Wages_Code] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Wages_Details] ( [Employee_Wages_Code] [varchar](50) NOT NULL, 	[Company_IdNo] [smallint] NOT NULL, 	[Employee_Wages_No] [varchar](50) NOT NULL, 	[for_OrderBy] [numeric](18, 2) NOT NULL, 	[Sl_No] [smallint] NOT NULL, 	[Shift_IdNo] [int] NULL DEFAULT (0) ,  	[Weight_From] [numeric](18, 3) NULL DEFAULT (0) , 	[Weight_To] [numeric](18, 3) NULL DEFAULT (0) , 	[Front_Sizing_Wages] [numeric](18, 2) NULL DEFAULT (0) , 	[Back_Sizing_Wages] [numeric](18, 2) NULL DEFAULT (0) , 	[Boiler_Wages] [numeric](18, 2) NULL DEFAULT (0) , 	[Cooker_Wages] [numeric](18, 2) NULL DEFAULT (0) ,  CONSTRAINT [PK_PayRoll_Employee_Wages_Details] PRIMARY KEY CLUSTERED  ( 	[Employee_Wages_Code] , [Sl_No]  ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Shift_Head] ( [Shift_IdNo] [int] NOT NULL, [Shift_Name] [varchar](50) NOT NULL, CONSTRAINT [PK_Shift_Head] PRIMARY KEY CLUSTERED  ( [Shift_IdNo] ) ON [PRIMARY], CONSTRAINT [IX_Shift_Head] UNIQUE NONCLUSTERED  ( [Shift_Name] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Job_Card_Details](	[Job_Card_Code] [varchar](30) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Job_Card_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Job_Card_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NULL,[Sl_No] [smallint] NOT NULL,	[Count_IdNo] [smallint] NOT NULL,	[Yarn_Type] [varchar](50) NULL,[SetCode_ForSelection] [varchar](30) NOT NULL,	[Mill_IdNo] [smallint] NULL,	[Bags] [numeric](18, 2) NULL,	[Cones] [int] NULL,	[Weight] [numeric](18, 3) NOT NULL,	[Set_Code] [varchar](30) NULL,	[Job_Card_SlNo] [int] NULL ," &
                        "CONSTRAINT [PK_Job_Card_Details] PRIMARY KEY CLUSTERED (	[Job_Card_Code]  ,	[Sl_No]  ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Job_Card_Head](	[Job_Card_Code] [varchar](30) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Job_Card_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Job_Card_Date] [smalldatetime] NOT NULL,	[Ledger_IdNo] [int] NULL,	[setcode_forSelection] [varchar](50) NULL,	[count_idno] [smallint] NOT NULL,	[mill_idno] [smallint] NULL,	[Beam_Width_Idno] [varchar](20) NULL,	[ends_name] [varchar](50) NOT NULL,	[pcs_length] [varchar](50) NULL,	[tape_length] [varchar](20) NULL,	[meters_yards_type] [varchar](20) NULL,	[warp_meters] [varchar](35) NULL,	[Empty_Bags] [numeric](18, 0) NULL,	[Empty_Cones] [numeric](18, 0) NULL,	[Empty_Beam] [numeric](18, 0) NULL,	[Transport_IdNo] [int] NULL,	[Vehicle_No] [varchar](50) NULL,	[Delivery_At] [varchar](50) NULL,	[Remarks] [varchar](500) NULL,	[total_bags] [numeric](18, 2) NULL,	[total_cones] [int] NULL,	[total_weight] [numeric](18, 3) NULL, " &
                          "CONSTRAINT [PK_Job_Card_Head] PRIMARY KEY CLUSTERED ( [Job_Card_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Salary_Payment_Type_Head](	[Salary_Payment_Type_IdNo] [smallint] NOT NULL,	[Salary_Payment_Type_Name] [varchar](50) NOT NULL,	[sur_name] [varchar](50) NOT NULL,	[Monthly_Weekly] [varchar](50) NULL," &
                          "CONSTRAINT [PK_PayRoll_Salary_Payment_Type_Head] PRIMARY KEY CLUSTERED (        [Salary_Payment_Type_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Working_Type_Head]([Working_Type_IdNo] [smallint] NOT NULL,	[Working_Type_Name] [varchar](50) NOT NULL,	[sur_name] [varchar](50) NOT NULL," &
                          "CONSTRAINT [PK_Working_Type_Head] PRIMARY KEY CLUSTERED ( [Working_Type_IdNo]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Department_Head](	[Department_Idno] [smallint] NOT NULL,	[Department_Name] [varchar](50) NOT NULL,	[sur_name] [varchar](50) NOT NULL, " &
                          "CONSTRAINT [PK_Department_Head] PRIMARY KEY CLUSTERED (  [Department_Idno]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Relation_Name1 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Relation_Name1 = '' where Relation_Name1  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Relation_Name2 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Relation_Name2 = '' where Relation_Name2  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Relation_Name3 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Relation_Name3 = '' where Relation_Name3  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Relation_Name4 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Relation_Name4 = '' where Relation_Name4  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Relation_Ship1 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Relation_Ship1 = '' where Relation_Ship1  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Relation_Ship2 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Relation_Ship2 = '' where Relation_Ship2  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Relation_Ship3 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Relation_Ship3 = '' where Relation_Ship3  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Relation_Ship4 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Relation_Ship4 = '' where Relation_Ship4  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add RelationName_Image1 Image default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set RelationName_Image1 = '' where RelationName_Image1  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add RelationName_Image2 Image default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set RelationName_Image2 = '' where RelationName_Image2  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add RelationName_Image3 Image default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set RelationName_Image3= '' where RelationName_Image3  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add RelationName_Image4 Image default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set RelationName_Image4 = '' where RelationName_Image4  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Document_Name1 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Document_Name1 = '' where Document_Name1  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Document_Name2 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Document_Name2 = '' where Document_Name2  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Document_Name3 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Document_Name3 = '' where Document_Name3  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Document_Name4 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Document_Name4 = '' where Document_Name4  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Certificate1 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Certificate1 = '' where Certificate1  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Certificate2 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Certificate2 = '' where Certificate2  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Certificate3 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Certificate3 = '' where Certificate3  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Certificate4 varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Certificate4 = '' where Certificate4  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Document_Image1 Image default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Document_Image1 = '' where Document_Image1  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Document_Image2 Image default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Document_Image2 = '' where Document_Image2  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Document_Image3 Image default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Document_Image3= '' where Document_Image3  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Document_Image4 Image default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Document_Image4 = '' where Document_Image4  is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table PayRoll_Employee_Head add Department_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Department_IdNo = 0 where Department_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Company_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Company_IdNo = 0 where Company_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Salary_Payment_Type_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Salary_Payment_Type_IdNo = 0 where Salary_Payment_Type_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add D_A numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set D_A = 0 where D_A is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add H_R_A numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set H_R_A = 0 where H_R_A is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Conveyance_Esi_Pf numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Conveyance_Esi_pf = 0 where Conveyance_Esi_pf is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Conveyance_Salary numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Conveyance_Salary = 0 where Conveyance_Salary is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Washing numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Washing = 0 where Washing is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Entertainment numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Entertainment = 0 where Entertainment is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add Maintenance numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set Maintenance = 0 where Maintenance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Salary_Details add MessDeduction numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Salary_Details set MessDeduction = 0 where MessDeduction is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Wages_Head add Front_Sizer numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Wages_Head set Front_Sizer = 0 where Front_Sizer is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Wages_Head add Back_Sizer numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Wages_Head set Back_Sizer = 0 where Back_Sizer is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Wages_Head add Cooker numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Wages_Head set Cooker = 0 where Cooker is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Wages_Head add Boiler numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Wages_Head set Boiler = 0 where Boiler is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Head add Area_IdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Head set Area_IdNo = 0 Where Area_IdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Department_Head](	[Department_Idno] [smallint] NOT NULL,	[Department_Name] [varchar](50) NOT NULL,	[sur_name] [varchar](50) NOT NULL, " &
                          "CONSTRAINT [PK_Department_Head] PRIMARY KEY CLUSTERED (  [Department_Idno])ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Payment_Head](	[Employee_Payment_Code] [varchar](30) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Employee_Payment_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Employee_Payment_Date] [smalldatetime] NOT NULL,	[Employee_IdNo] [int] NULL,	[Cash_Cheque] [varchar](20) NULL,	[Advance_Salary] [varchar](20) NULL,	[DebitAc_IdNo] [int] NULL,	[Amount] [numeric](18, 2) NULL,	[Remarks] [varchar](200) NULL, " &
                          "CONSTRAINT [PK_PayRoll_Employee_Payment_Head] PRIMARY KEY CLUSTERED ( [Employee_Payment_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Deduction_Head](	[Employee_Deduction_Code] [varchar](30) NOT NULL,[Company_IdNo] [smallint] NOT NULL,	[Employee_Deduction_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Employee_Deduction_Date] [smalldatetime] NOT NULL,	[Employee_IdNo] [int] NULL,	[Advance_Deduction_Amount] [numeric](18, 3) NULL,	[Remarks] [varchar](200) NULL," &
                         "CONSTRAINT [PK_PayRoll_Employee_Deduction_Head] PRIMARY KEY CLUSTERED ([Employee_Deduction_Code])  ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add Incentive_Amount numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set Incentive_Amount = 0 where Incentive_Amount is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add OT_Minutes numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set OT_Minutes = 0 where OT_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add OT_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set OT_Hours = 0 where No_Of_Shift is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Ot_Pay_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Ot_Pay_Hours = 0 where Ot_Pay_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Ot_Salary Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Ot_Salary = 0 where Ot_Salary is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Minus_Advance Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Minus_Advance = 0 where Minus_Advance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Salary_Details add Balance_Advance Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Salary_Details set Balance_Advance = 0 where Balance_Advance is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Salary_Details](	[Salary_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Salary_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Salary_Date] [smalldatetime] NOT NULL,	[SL_No] [smallint] NOT NULL,	[Employee_IdNo] [int] NULL,	[Basic_Salary] [numeric](18, 3) NULL,	[Total_Days] [numeric](18, 3) NULL,	[Net_Pay] [numeric](18, 2) NULL,	[No_Of_Attendance_Days] [numeric](18, 3) NULL,	[From_W_off_CR] [numeric](18, 3) NULL,	[Festival_Holidays] [numeric](18, 3) NULL,	[No_Of_Leave] [numeric](18, 3) NULL,	[Attendance_On_W_Off_FH] [numeric](18, 3) NULL,	[Op_W_Off_CR] [numeric](18, 3) NULL,	[Add_W_Off_CR] [numeric](18, 3) NULL,	[Less_W_Off_CR] [numeric](18, 3) NULL,	[Total_W_Off_CR] [numeric](18, 3) NULL,	[Salary_Days] [numeric](18, 3) NULL,	[Basic_Pay] [numeric](18, 3) NULL,	[D_A] [numeric](18, 3) NULL,	[Earning] [numeric](18, 3) NULL,	[H_R_A] [numeric](18, 3) NULL,	[Conveyance] [numeric](18, 3) NULL,	[Washing] [numeric](18, 3) NULL,	[Entertainment] [numeric](18, 3) NULL,	[Maintenance] [numeric](18, 3) NULL,	[Other_Addition] [numeric](18, 3) NULL,	[Total_Addition] [numeric](18, 3) NULL,	[Mess] [numeric](18, 3) NULL,	[Medical] [numeric](18, 3) NULL,	[Store] [numeric](18, 3) NULL,	[ESI] [numeric](18, 3) NULL,	[P_F] [numeric](18, 3) NULL,	[E_P_F] [numeric](18, 3) NULL,	[Pension_Scheme] [numeric](18, 3) NULL,	[Other_Deduction] [numeric](18, 3) NULL,	[Total_Deduction] [numeric](18, 3) NULL,	[Attendance_Incentive] [numeric](18, 3) NULL,	[Net_Salary] [numeric](18, 3) NULL,	[Advance] [numeric](18, 3) NULL,	[Day_For_Bonus] [numeric](18, 3) NULL,	[Earning_For_Bonus] [numeric](18, 3) NULL, " &
                          "CONSTRAINT [PK_PayRoll_Salary_Details] PRIMARY KEY CLUSTERED (	[Salary_Code] ,	[SL_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE [PayRoll_Salary_Head] ADD  CONSTRAINT [DF_PayRoll_Salary_Head_Month_IdNo]  DEFAULT ((0)) FOR [Month_IdNo]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Salary_Head](	[Salary_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Salary_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Salary_Date] [datetime] NOT NULL,	[Salary_Type] [varchar](20) NULL,	[From_Date] [varchar](50) NULL,	[To_Date] [varchar](50) NULL,	[Total_Days] [numeric](18, 3) NULL,	[Festival_Days] [numeric](18, 3) NULL,	[Month_IdNo] [int] NULL," &
                           "CONSTRAINT [PK_PayRoll_Salary_Head] PRIMARY KEY CLUSTERED (      [Salary_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add OT_Minutes int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set OT_Minutes = 0 Where OT_Minutes is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add OT_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set OT_Hours = 0 where No_Of_Shift is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add OT_Hours Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Details set OT_Hours = 0 where OT_Hours is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Attendance_Details](	[Employee_Attendance_Code] [varchar](50) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Employee_Attendance_No] [varchar](50) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Employee_Attendance_Date] [smalldatetime] NOT NULL,	[Sl_No] [int] NOT NULL,	[Employee_IdNo] [int] NULL,	[Working_Type_Name] [varchar](50) NULL,	[day_Shift] [int] NULL,	[Night_Shift] [int] NULL,	[Bonus_Shift] [numeric](18, 2) NULL,	[Wages_Shift] [numeric](18, 2) NULL,	[Tiffen] [numeric](18, 3) NULL,	[Extra_Wages] [numeric](18, 2) NULL," &
                          "[Total_Wages] [numeric](18, 3) NULL, CONSTRAINT [PK_PayRoll_Employee_Attendance_Details] PRIMARY KEY CLUSTERED (	[Employee_Attendance_Code] ,	[Sl_No] ) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Details add CONSTRAINT [IX_PayRoll_Employee_Attendance_Details] UNIQUE NONCLUSTERED ( [Employee_Attendance_Date], [Employee_IdNo] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Attendance_Head](	[Employee_Attendance_Code] [varchar](30) NOT NULL,	[Company_IdNo] [smallint] NOT NULL,	[Employee_Attendance_No] [varchar](20) NOT NULL,	[for_OrderBy] [numeric](18, 2) NOT NULL,	[Employee_Attendance_Date] [smalldatetime] NOT NULL,	[Day_Name] [varchar](50) NULL, " &
                          "CONSTRAINT [PK_PayRoll_Employee_Attendance_Head] PRIMARY KEY CLUSTERED (        [Employee_Attendance_Code]) ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Head add CONSTRAINT [IX_PayRoll_Employee_Attendance_Head] UNIQUE NONCLUSTERED ( [Employee_Attendance_Date] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Head add Day_Name varchar(30) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update PayRoll_Employee_Attendance_Head set Day_Name = '' Where Day_Name is Null"
        cmd.ExecuteNonQuery()

        Common_Procedures.Drop_Column_Default_Constraint(cn1, "PayRoll_Employee_Attendance_Head", "Day")
        cmd.CommandText = "Alter table PayRoll_Employee_Attendance_Head Drop Column Day"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Head](	[Employee_IdNo] [smallint] NOT NULL,	[Employee_Name] [varchar](50) NOT NULL,	[Sur_Name] [varchar](50) NOT NULL,	[Card_No] [varchar](50) NULL,	[Working_Type_IdNo] [int] NULL,	[Employee_Image] [image] NULL,	[Join_date] [varchar](50) NULL,	[Scheme_Starts] [varchar](50) NULL,	[Payment_Type] [varchar](50) NULL,	[shift_Day_Month] [varchar](50) NULL,	[Week_Off] [varchar](50) NULL,	[Trainee] [varchar](50) NULL,	[Designation] [varchar](50) NULL,	[Department] [varchar](50) NULL,	[Dispensary] [varchar](50) NULL," &
                            "[Esi_Status] [int] NULL,	[Pf_Status] [int] NULL,	[Esi_Salary] [int] NULL,	[Pf_Salary] [int] NULL,	[Esi_No] [int] NULL,	[Pf_No] [int] NULL,	[Esi_Join_Date] [varchar](50) NULL,	[Esi_Leave_Date] [varchar](50) NULL,	[Pf_Join_Date] [varchar](50) NULL,	[Pf_Leave_Date] [varchar](50) NULL,	[D_A] [numeric](18, 2) NULL,	[H_R_A] [numeric](18, 2) NULL,	[Esi_Conveyance] [numeric](18, 2) NULL,	[Salary_Conveyance] [numeric](18, 2) NULL,	[Washing] [numeric](18, 2) NULL,	[Entertainment] [numeric](18, 2) NULL,	[Maintenance] [numeric](18, 2) NULL," &
                            "[Mess_Deduction] [numeric](18, 2) NULL,	[wekk_Credit] [int] NULL,	[OP_Balance] [numeric](18, 2) NULL,	[Op_Att] [int] NULL,	[Op_Amount] [int] NULL,	[O_T_Salary] [numeric](18, 2) NULL,	[Bank_Ac_No] [varchar](50) NULL,[Date_Birth] [varchar](50) NULL,	[Age] [int] NULL,	[Sex] [varchar](50) NULL,	[Height] [int] NULL,	[weight] [int] NULL,	[Father_Husband] [varchar](50) NULL,	[Marital_Status] [varchar](50) NULL,	[No_Children] [int] NULL,	[Qualification] [varchar](50) NULL,	[Community] [varchar](50) NULL,	[Blood_Group] [varchar](50) NULL," &
                            "[address1] [varchar](50) NULL,	[Address2] [varchar](50) NULL,	[Address3] [varchar](50) NULL,	[Village] [varchar](50) NULL,	[Taulk] [varchar](50) NULL,	[District] [varchar](50) NULL,	[Phone_No] [varchar](50) NULL,	[Mobile_No] [varchar](50) NULL,	[Date_Status] [int] NULL,	[Releave_Date] [varchar](50) NULL,	[Reason] [varchar](100) NULL,	[Scheme_Total_Salary] [int] NULL,	[Scheme_Total_Esipf] [int] NULL,	[Scheme_Total_Ot] [int] NULL,	[nonScheme_Total_Salary] [int] NULL,	[NonScheme_Total_Esipf] [int] NULL,	[NonScheme_Total_Ot] [int] NULL," &
                            " CONSTRAINT [PK_PayRoll_Employee_Head] PRIMARY KEY CLUSTERED (  [Employee_IdNo]) ON [PRIMARY], CONSTRAINT [Duplicate_PayRoll_Employee_Head] UNIQUE NONCLUSTERED (       [Sur_Name]) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [PayRoll_Employee_Salary_Details](	[Employee_IdNo] [int] NOT NULL,	[Sl_No] [smallint] NOT NULL,	[From_Date] [varchar](50) NULL,	[To_Date] [varchar](50) NULL,	[For_Salary] [numeric](18, 3) NULL,	[Esi_Pf] [numeric](18, 3) NULL,	[O_T] [numeric](18, 3) NULL , " &
                            "CONSTRAINT [PK_Employee_Scheme_PayRoll_Salary_Details] PRIMARY KEY CLUSTERED (	[Employee_IdNo] ,        [Sl_No])  ON [PRIMARY]) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.Dispose()

        FrmNm.Cursor = Cursors.Default
        If Trim(LCase(FrmNm.Name)) = "mdiparent1" Then
            MDIParent1.Cursor = Cursors.Default
        End If

        If vFldsChk_All_Status = False Then
            MessageBox.Show("Fields Verified", "FOR FIELDS CREATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        End If

    End Sub

    Public Shared Sub Populate_Invoice_DC_Codes(Cn1 As SqlClient.SqlConnection)

        Dim tr As SqlClient.SqlTransaction
        tr = Cn1.BeginTransaction
        Try

            Dim da1 As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim cmd As New SqlClient.SqlCommand

            cmd.Transaction = tr
            cmd.Connection = Cn1
            cmd.CommandText = "Select distinct dccodes,job_no,ordercode_forselection,sales_code,Sales_Date from sales_details where len(DCcodes)>0"

            da1.SelectCommand = cmd
            da1.Fill(dt1)


            cmd.CommandText = "delete from Invoice_DC_Details "
            cmd.ExecuteNonQuery()

            If dt1.Rows.Count > 0 Then
                For I As Integer = 0 To dt1.Rows.Count - 1
                    For J As Integer = 0 To Split(dt1.Rows(I).Item(0), "$$$").GetUpperBound(0)
                        cmd.CommandText = "Insert into Invoice_DC_Details values ('" & dt1.Rows(I).Item(3) & "','" & Split(dt1.Rows(I).Item(0), "$$$")(J) & "','" & dt1.Rows(I).Item(1) & "','" & Format(dt1.Rows(I).Item(4), "dd-MMM-yyyy") & "','" & dt1.Rows(I).Item(2) & "')"
                        cmd.ExecuteNonQuery()
                    Next
                Next
            End If

            tr.Commit()
            MsgBox("Invoice-DC Nos Match Table Updated")

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + ". Could Not Update Invoice-DC Nos Match Table")


        End Try

    End Sub

End Class
