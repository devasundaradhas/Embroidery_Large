CREATE TABLE [dbo].[ACCOUNT_MASTER](
	[AC_Name] [varchar](100) NULL,
	[GSTIN] [varchar](15) NULL,
	[SERVERNAME] [varchar](100) NULL,
	[SERVERPASSWORD] [varchar](400) NULL,
	[TRANSDATABASENAME] [varchar](100) NULL,
	[GSTINTABLE] [varchar](100) NULL,
	[GSTINFIELD] [varchar](100) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AccountsGroup_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AccountsGroup_Head](
	[AccountsGroup_IdNo] [smallint] NOT NULL,
	[AccountsGroup_Name] [varchar](100) NOT NULL,
	[Sur_Name] [varchar](100) NOT NULL,
	[Parent_Name] [varchar](100) NULL,
	[Parent_Idno] [varchar](50) NULL,
	[Carried_Balance] [tinyint] NULL,
	[Order_Position] [numeric](18, 2) NULL,
	[TallyName] [varchar](100) NULL,
	[TallySubName] [varchar](100) NULL,
	[Indicate] [tinyint] NULL,
	[LedgerOrder_Position] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_AccountsGroup_Head] PRIMARY KEY CLUSTERED 
(
	[AccountsGroup_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Area_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Area_Head](
	[Area_IdNo] [smallint] NOT NULL,
	[Area_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Area_Head] PRIMARY KEY CLUSTERED 
(
	[Area_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Area_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cetegory_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cetegory_Head](
	[Cetegory_IdNo] [int] NOT NULL,
	[Cetegory_Name] [varchar](50) NULL CONSTRAINT [DF_Cetegory_Head_Cetegory_Name]  DEFAULT (''),
	[Sur_Name] [varchar](50) NULL CONSTRAINT [DF_Cetegory_Head_Sur_Name]  DEFAULT (''),
 CONSTRAINT [PK_Cetegory_Head] PRIMARY KEY CLUSTERED 
(
	[Cetegory_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cheque_Print_Positioning_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cheque_Print_Positioning_Head](
	[Cheque_Print_Positioning_No] [varchar](50) NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Paper_Orientation] [varchar](50) NULL,
	[Left_Margin] [numeric](18, 3) NULL,
	[Top_Margin] [numeric](18, 3) NULL,
	[Account_No] [varchar](50) NULL,
	[Ac_Payee_Left] [numeric](18, 3) NULL,
	[Ac_Payee_Top] [numeric](18, 3) NULL,
	[Ac_Payee_Width] [numeric](18, 3) NULL,
	[Date_Left] [numeric](18, 3) NULL,
	[Date_Top] [numeric](18, 3) NULL,
	[Date_Width] [numeric](18, 3) NULL,
	[PartyName_Left] [numeric](18, 3) NULL,
	[PartyName_Top] [numeric](18, 3) NULL,
	[PartyName_Width] [numeric](18, 3) NULL,
	[Second_PartyName_Left] [numeric](18, 3) NULL,
	[Second_PartyName_Top] [numeric](18, 3) NULL,
	[Second_PartyName_Width] [numeric](18, 3) NULL,
	[AmountWords_Left] [numeric](18, 3) NULL,
	[AmountWords_Top] [numeric](18, 3) NULL,
	[AmountWords_Width] [numeric](18, 3) NULL,
	[Second_AmountWords_Left] [numeric](18, 3) NULL,
	[Second_AmountWords_Top] [numeric](18, 3) NULL,
	[Second_AmountWords_Width] [numeric](18, 3) NULL,
	[Rupees_Left] [numeric](18, 3) NULL,
	[Rupees_Top] [numeric](18, 3) NULL,
	[Rupees_Width] [numeric](18, 3) NULL,
	[CompanyName_Left] [numeric](18, 3) NULL,
	[CompanyName_Top] [numeric](18, 3) NULL,
	[CompanyName_Width] [numeric](18, 3) NULL,
	[Partner_Left] [numeric](18, 3) NULL,
	[Partner_Top] [numeric](18, 3) NULL,
	[Partner_Width] [numeric](18, 3) NULL,
	[AccountNo_Left] [numeric](18, 3) NULL,
	[AccountNo_Top] [numeric](18, 3) NULL,
	[AccountNo_Width] [numeric](18, 3) NULL,
	[Partner] [varchar](50) NULL,
 CONSTRAINT [PK_Cheque_Print_Positining_Head] PRIMARY KEY CLUSTERED 
(
	[Ledger_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Closing_Stock_Value_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Closing_Stock_Value_Head](
	[Closing_Stock_Value_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NULL,
	[for_OrderBy] [numeric](18, 2) NULL,
	[Closing_Stock_Value_Idno] [int] NULL,
	[Closing_Stock_Value_Date] [smalldatetime] NULL,
	[Closing_Stock_Value] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Closing_Stock_Value_Head_1] PRIMARY KEY CLUSTERED 
(
	[Closing_Stock_Value_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Closing_Stock_Value_Head_1] UNIQUE NONCLUSTERED 
(
	[Company_IdNo] ASC,
	[Closing_Stock_Value_Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cloth_Sales_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cloth_Sales_Head](
	[Cloth_Sales_Code] [varchar](50) NOT NULL,
	[Cloth_Sales_No] [varchar](50) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Company_IdNo] [int] NOT NULL,
	[Cloth_Sales_Date] [smalldatetime] NOT NULL,
	[Invoice_No] [varchar](50) NULL,
	[Ledger_IdNo] [smallint] NULL,
	[Ledger_IdNo1] [smallint] NULL,
	[Transport_IdNo] [smallint] NULL,
	[Lr_No] [varchar](50) NULL,
	[No_Of_Sales] [int] NULL,
	[Meter] [numeric](18, 2) NULL,
	[Rate] [numeric](18, 3) NULL,
	[Amount] [numeric](18, 3) NULL,
	[Com_Type] [varchar](25) NULL,
	[Com_Rate] [numeric](18, 3) NULL,
	[Com_Amount] [numeric](18, 3) NULL,
	[Discount_Percentage] [numeric](18, 3) NULL,
	[Discount_Amount] [numeric](18, 2) NULL,
	[net_Amount] [numeric](18, 2) NULL,
	[Bale_Nos] [varchar](100) NULL,
	[Agent_IdNo] [int] NULL,
	[Add_Less] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Cloth_Sales_Head] PRIMARY KEY NONCLUSTERED 
(
	[Cloth_Sales_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Colour_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Colour_Head](
	[Colour_IdNo] [int] NOT NULL,
	[Colour_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Colour_Head] PRIMARY KEY CLUSTERED 
(
	[Colour_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Colour_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Combo_Pop_Temp]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Combo_Pop_Temp](
	[LOV] [varchar](500) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Company_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Company_Head](
	[Company_IdNo] [smallint] NOT NULL,
	[Company_Name] [varchar](100) NOT NULL,
	[Company_SurName] [varchar](100) NOT NULL,
	[Company_ShortName] [varchar](20) NOT NULL,
	[Company_Address1] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_Address1]  DEFAULT (''),
	[Company_Address2] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_Address2]  DEFAULT (''),
	[Company_Address3] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_Address3]  DEFAULT (''),
	[Company_Address4] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_Address4]  DEFAULT (''),
	[Company_City] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_City]  DEFAULT (''),
	[Company_PinCode] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_PinCode]  DEFAULT (''),
	[Company_PhoneNo] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_PhoneNo]  DEFAULT (''),
	[Company_FaxNo] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_FaxNo]  DEFAULT (''''),
	[Company_TinNo] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_MobileNo]  DEFAULT (''),
	[Company_CstNo] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_TinNo1]  DEFAULT (''),
	[Company_EMail] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_EMail]  DEFAULT (''),
	[Company_ContactPerson] [varchar](100) NULL CONSTRAINT [DF_Company_Head_Company_ContactPerson]  DEFAULT (''),
	[Company_Description] [varchar](200) NULL CONSTRAINT [DF_Company_Head_Company_Description]  DEFAULT (''),
	[Company_Type] [varchar](50) NULL DEFAULT ('ACCOUNT'),
	[Company_Bank_Ac_Details] [varchar](200) NULL DEFAULT (''),
	[SMS_Provider_SenderID] [varchar](50) NULL,
	[SMS_Provider_Key] [varchar](50) NULL,
	[SMS_Provider_RouteID] [varchar](50) NULL,
	[SMS_Provider_Type] [varchar](50) NULL,
	[Area_IdNo] [int] NULL,
	[Company_ESINo] [varchar](50) NULL,
	[Company_Owner_Designation] [varchar](50) NULL DEFAULT (''),
	[Company_Website] [varchar](50) NULL DEFAULT (''),
	[Company_GSTinNo] [varchar](50) NULL DEFAULT (''),
	[Company_State_IdNo] [int] NULL DEFAULT ((0)),
	[Company_PanNo] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_Company_Head] PRIMARY KEY CLUSTERED 
(
	[Company_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Duplicate_CompanyHead_Name] UNIQUE NONCLUSTERED 
(
	[Company_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Duplicate_CompanyHead_ShortName] UNIQUE NONCLUSTERED 
(
	[Company_ShortName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Duplicate_CompanyHead_SurName] UNIQUE NONCLUSTERED 
(
	[Company_SurName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Component_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Component_Head](
	[Component_IdNo] [int] NOT NULL,
	[Component_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Component_Head] PRIMARY KEY CLUSTERED 
(
	[Component_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Delivery_Details](
	[Delivery_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Delivery_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Delivery_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Colour_IdNo] [smallint] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
	[Gms] [numeric](18, 2) NULL,
	[Rolls] [numeric](18, 2) NULL,
	[Weight_Rolls] [numeric](18, 3) NULL,
	[Meters] [numeric](18, 2) NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Remarks] [varchar](100) NULL,
	[Actual_Weight] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Delivery_Details] PRIMARY KEY CLUSTERED 
(
	[Delivery_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Delivery_Head](
	[Delivery_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Delivery_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Delivery_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL,
	[Ledger_IdNo] [int] NULL,
	[Cash_PartyName] [varchar](50) NULL,
	[Party_PhoneNo] [varchar](50) NULL,
	[SalesAc_IdNo] [int] NULL,
	[Tax_Type] [varchar](20) NULL,
	[TaxAc_IdNo] [int] NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Removal_Date] [varchar](20) NULL,
	[Removal_Time] [varchar](20) NULL,
	[Bag_Nos] [varchar](500) NULL,
	[Narration] [varchar](500) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Transport_IdNo] [int] NULL,
	[Order_No] [varchar](50) NULL,
	[Order_Date] [varchar](50) NULL,
	[Against_CForm_Status] [tinyint] NULL,
	[Weight] [numeric](18, 3) NULL,
	[Branch_Transfer_Status] [tinyint] NULL,
	[OnAc_IdNo] [int] NULL,
	[Total_Rolls] [numeric](18, 2) NULL,
	[Total_Meters] [numeric](18, 2) NULL,
	[Remarks] [varchar](100) NULL,
	[Total_Actual_Weight] [numeric](18, 2) NULL,
	[Invoice_Code] [varchar](50) NULL,
	[NoOf_Bundle] [varchar](50) NULL,
 CONSTRAINT [PK_Delivery_Head] PRIMARY KEY CLUSTERED 
(
	[Delivery_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Department_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Department_Head](
	[Department_Idno] [smallint] NOT NULL,
	[Department_Name] [varchar](50) NOT NULL,
	[sur_name] [varchar](50) NOT NULL,
	[Department_Code] [varchar](50) NULL,
 CONSTRAINT [PK_Department_Head] PRIMARY KEY CLUSTERED 
(
	[Department_Idno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Design_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Design_Head](
	[Design_IdNo] [int] NOT NULL,
	[Design_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Design_Head] PRIMARY KEY CLUSTERED 
(
	[Design_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Design_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Embroidery_Expense_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Embroidery_Expense_Details](
	[Expense_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Expense_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Expense_Date] [smalldatetime] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[Expense_IdNo] [int] NULL,
	[First_Shift] [numeric](18, 2) NULL,
	[Second_Shift] [numeric](18, 2) NULL,
	[Third_Shift] [numeric](18, 2) NULL,
	[Total_Shift] [numeric](18, 2) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Cost_Type] [varchar](50) NULL,
 CONSTRAINT [PK_Embroidery_Expense_Details] PRIMARY KEY CLUSTERED 
(
	[Expense_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Embroidery_Expense_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Embroidery_Expense_Head](
	[Expense_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Expense_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Expense_Date] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Embroidery_Expense_Head] PRIMARY KEY CLUSTERED 
(
	[Expense_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Embroidery_Jobwork_Delivery_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Embroidery_Jobwork_Delivery_Details](
	[Embroidery_Jobwork_Delivery_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Embroidery_Jobwork_Delivery_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Embroidery_Jobwork_Delivery_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Item_Description] [varchar](500) NULL,
	[Embroidery_Jobwork_Delivery_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,
	[Receipt_Quantity] [numeric](18, 2) NULL,
	[No_Of_Rolls] [numeric](18, 2) NULL,
	[Entry_Type] [varchar](30) NULL,
	[Order_Detail_SlNo] [int] NULL,
	[Noof_Items] [numeric](18, 2) NULL,
	[HSN_Code] [varchar](50) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Order_No] [varchar](100) NULL,
	[Ordercode_forSelection] [varchar](100) NULL,
	[Size_Idno] [int] NULL,
	[Colour_IdNo] [smallint] NULL,
	[Style_Idno] [int] NULL,
 CONSTRAINT [PK_Embroidery_Jobwork_Delivery_Details] PRIMARY KEY CLUSTERED 
(
	[Embroidery_Jobwork_Delivery_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Embroidery_Jobwork_Delivery_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Embroidery_Jobwork_Delivery_Head](
	[Embroidery_Jobwork_Delivery_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Embroidery_Jobwork_Delivery_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Embroidery_Jobwork_Delivery_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Order_No] [varchar](50) NULL,
	[Order_Date] [varchar](50) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Transport_IdNo] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[Entry_VAT_GST_Type] [varchar](50) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[CGst_Amount] [numeric](18, 2) NULL,
	[SGst_Amount] [numeric](18, 2) NULL,
	[IGst_Amount] [numeric](18, 2) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Entry_GST_Tax_Type] [varchar](20) NULL,
	[Total_Bags] [int] NULL,
	[Electronic_Reference_No] [varchar](100) NULL,
	[Transportation_Mode] [varchar](100) NULL,
	[Date_Time_Of_Supply] [varchar](100) NULL,
	[Weight] [numeric](18, 2) NULL,
	[Freight_ToPay_Amount] [numeric](18, 2) NULL,
	[charge] [numeric](18, 2) NULL,
	[Lr_Date] [varchar](50) NULL,
	[Lr_No] [varchar](50) NULL,
	[Booked_By] [varchar](50) NULL,
	[Entry_Type] [varchar](50) NULL,
	[Return_Reason] [varchar](250) NULL DEFAULT (''),
	[IsReturn] [bit] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Embroidery_Jobwork_Delivery_Head] PRIMARY KEY CLUSTERED 
(
	[Embroidery_Jobwork_Delivery_Code] ASC,
	[Embroidery_Jobwork_Delivery_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Embroidery_Jobwork_Invoice_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Embroidery_Jobwork_Invoice_Details](
	[Embroidery_Jobwork_Invoice_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Embroidery_Jobwork_Invoice_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Embroidery_Jobwork_Invoice_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[ItemGroup_IdNo] [smallint] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Noof_Items] [numeric](18, 3) NULL,
	[Bags] [int] NULL,
	[Rate] [numeric](18, 2) NULL,
	[Tax_Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Discount_Perc] [numeric](18, 2) NULL,
	[Discount_Amount] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
	[Size_IdNo] [int] NULL,
	[Meters] [numeric](18, 2) NULL,
	[Colour_IdNo] [int] NULL,
	[Item_code] [varchar](100) NULL,
	[Entry_Type] [varchar](50) NULL,
	[Order_Code] [varchar](50) NULL,
	[Order_Detail_SlNo] [int] NULL,
	[Noof_Items_Return] [numeric](18, 2) NULL,
	[Embroidery_Jobwork_Invoice_Details_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Design_Picture] [image] NULL,
	[Rate_For] [varchar](50) NULL,
	[Order_No] [varchar](100) NULL,
	[Order_Date] [varchar](100) NULL,
	[Quantity] [numeric](18, 2) NULL,
	[Rate_1000Stitches] [numeric](18, 2) NULL,
	[Design_No] [varchar](50) NULL,
	[Details_Design] [varchar](500) NULL,
	[Return_Qty] [numeric](18, 2) NULL,
	[Style_Idno] [int] NULL,
	[Style_Name] [varchar](50) NULL,
	[Trade_Discount_Amount_For_All_Item] [numeric](18, 2) NULL,
	[Trade_Discount_Perc_For_All_Item] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[HSN_Code] [varchar](50) NULL,
	[GST_Percentage] [numeric](18, 2) NULL,
	[Actual_Amount] [numeric](18, 2) NULL,
	[Actual_Rate] [numeric](18, 2) NULL,
	[Advance_Amount] [numeric](18, 2) NULL,
	[Balance_Amount] [numeric](18, 2) NULL,
	[Dc_No] [varchar](50) NULL,
	[Sales_Price] [numeric](18, 2) NULL,
	[Discount_Amount_item] [numeric](18, 2) NULL,
	[Rate_Tax] [numeric](18, 2) NULL,
	[Discount_Perc_Item] [numeric](18, 2) NULL,
	[Cash_Discount_Perc_For_All_Item] [numeric](18, 2) NULL,
	[Cash_Discount_Amount_For_All_Item] [numeric](18, 2) NULL,
	[RateWithTax] [numeric](18, 2) NULL,
	[Item_Description] [varchar](500) NULL,
	[Embroidery_Jobwork_Receipt_Code] [varchar](50) NULL,
	[Embroidery_Jobwork_Receipt_Detail_SlNo] [int] NULL,
	[Area_IdNo] [int] NULL,
	[Agent_IdNo] [int] NULL,
	[Cgst_Percentage] [numeric](18, 2) NULL,
	[Cgst_Amount] [numeric](18, 2) NULL,
	[Sgst_Percentage] [numeric](18, 2) NULL,
	[Sgst_Amount] [numeric](18, 2) NULL,
	[Igst_Percentage] [numeric](18, 2) NULL,
	[Igst_Amount] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Total_Rate] [numeric](18, 2) NULL,
	[Discount_Total] [numeric](18, 2) NULL,
	[Ordercode_forSelection] [varchar](100) NULL,
 CONSTRAINT [PK_Embroidery_Jobwork_Invoice_Details] PRIMARY KEY CLUSTERED 
(
	[Embroidery_Jobwork_Invoice_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Embroidery_Jobwork_Invoice_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Embroidery_Jobwork_Invoice_Head](
	[Embroidery_Jobwork_Invoice_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Embroidery_Jobwork_Invoice_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Embroidery_Jobwork_Invoice_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL,
	[Ledger_IdNo] [int] NULL,
	[Cash_PartyName] [varchar](50) NULL,
	[Party_PhoneNo] [varchar](50) NULL,
	[SalesAc_IdNo] [int] NULL,
	[Tax_Type] [varchar](20) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Narration] [varchar](500) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Total_Bags] [int] NULL,
	[Total_Weight] [numeric](18, 3) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Total_DiscountAmount] [numeric](18, 2) NULL,
	[Total_TaxAmount] [numeric](18, 2) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[CashDiscount_Perc] [numeric](18, 2) NULL,
	[CashDiscount_Amount] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Freight_Amount] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Dc_No] [varchar](35) NULL,
	[Dc_Date] [varchar](20) NULL,
	[Booked_By] [varchar](35) NULL,
	[Transport_IdNo] [int] NULL,
	[Freight_ToPay_Amount] [numeric](18, 2) NULL,
	[Ro_Division_Status] [tinyint] NULL,
	[Order_No] [varchar](100) NULL,
	[Order_Date] [varchar](50) NULL,
	[Against_CForm_Status] [tinyint] NULL,
	[Weight] [numeric](18, 3) NULL,
	[Entry_Type] [varchar](20) NULL,
	[Payment_Terms] [varchar](100) NULL,
	[Total_Rolls] [numeric](18, 2) NULL,
	[OnAc_IdNo] [int] NULL,
	[Delivery_Code] [varchar](50) NULL,
	[Selection_Type] [varchar](50) NULL,
	[Party_Name] [varchar](50) NULL,
	[Entry_Status] [varchar](50) NULL,
	[Party_Dc_No] [varchar](200) NULL,
	[charge] [numeric](18, 2) NULL,
	[DeliveryTo_idNo] [int] NULL,
	[Place_Of_Supply] [varchar](100) NULL,
	[CGst_Percentage] [numeric](18, 2) NULL,
	[SGst_Percentage] [numeric](18, 2) NULL,
	[Entry_VAT_GST_Type] [varchar](100) NULL,
	[Electronic_Reference_No] [varchar](100) NULL,
	[Transportation_Mode] [varchar](100) NULL,
	[Date_Time_Of_Supply] [varchar](100) NULL,
	[Entry_GST_Tax_Type] [varchar](50) NULL,
	[CGst_Amount] [numeric](18, 2) NULL,
	[SGst_Amount] [numeric](18, 2) NULL,
	[IGst_Amount] [numeric](18, 2) NULL,
	[Actual_Net_Amount] [numeric](18, 2) NULL,
	[Actual_Gross_Amount] [numeric](18, 2) NULL,
	[Actual_Tax_Amount] [numeric](18, 2) NULL,
	[Freight_Charge] [numeric](18, 2) NULL,
	[Freight_Charge_Name] [varchar](50) NULL,
	[Receipt_Amount] [numeric](18, 2) NULL,
	[Delivery_Date] [varchar](50) NULL,
	[Received_Date] [varchar](50) NULL,
	[Sales_Order_Selection_Code] [varchar](50) NULL,
	[Delivery_Status] [int] NULL,
	[Advance_Amount] [numeric](18, 2) NULL,
	[Balance_Amount] [numeric](18, 2) NULL,
	[Form_H_Status] [numeric](18, 2) NULL,
	[ItemWise_DiscAmount] [numeric](18, 2) NULL,
	[Total_DiscountAmount_item] [numeric](18, 2) NULL,
	[Aessable_Amount] [numeric](18, 2) NULL,
	[AddLess_Name] [varchar](50) NULL,
	[Freight_Name] [varchar](50) NULL,
	[Received_Amount] [numeric](18, 2) NULL,
	[TaxAc_IdNo] [int] NULL,
	[Bill_No] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_Embroidery_Jobwork_Invoice_Head] PRIMARY KEY CLUSTERED 
(
	[Embroidery_Jobwork_Invoice_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Embroidery_Jobwork_Receipt_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Embroidery_Jobwork_Receipt_Details](
	[Embroidery_Jobwork_Receipt_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Embroidery_Jobwork_Receipt_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Embroidery_Jobwork_Receipt_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Item_Description] [varchar](500) NULL,
	[Embroidery_Jobwork_Receipt_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,
	[Invoice_Quantity] [numeric](18, 2) NULL,
	[No_Of_Rolls] [numeric](18, 2) NULL,
	[Entry_Type] [varchar](30) NULL,
	[Order_Detail_SlNo] [int] NULL,
	[Noof_Items] [numeric](18, 2) NULL,
	[HSN_Code] [varchar](50) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Order_No] [varchar](100) NULL,
	[Ordercode_forSelection] [varchar](100) NULL,
	[Size_Idno] [int] NULL,
	[Colour_IdNo] [smallint] NULL,
	[Style_Idno] [int] NULL,
	[Order_Code] [varchar](50) NULL DEFAULT (''),
	[Order_Date] [varchar](50) NULL DEFAULT (''),
	[Receipt_Type] [varchar](30) NULL DEFAULT (''),
 CONSTRAINT [PK_Embroidery_Jobwork_Receipt_Details] PRIMARY KEY CLUSTERED 
(
	[Embroidery_Jobwork_Receipt_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Embroidery_Jobwork_Receipt_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Embroidery_Jobwork_Receipt_Head](
	[Embroidery_Jobwork_Receipt_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Embroidery_Jobwork_Receipt_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Embroidery_Jobwork_Receipt_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Order_No] [varchar](50) NULL,
	[Order_Date] [varchar](50) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Transport_IdNo] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[Entry_VAT_GST_Type] [varchar](50) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[CGst_Amount] [numeric](18, 2) NULL,
	[SGst_Amount] [numeric](18, 2) NULL,
	[IGst_Amount] [numeric](18, 2) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Entry_GST_Tax_Type] [varchar](20) NULL,
	[Total_Bags] [int] NULL,
	[Electronic_Reference_No] [varchar](100) NULL,
	[Transportation_Mode] [varchar](100) NULL,
	[Date_Time_Of_Supply] [varchar](100) NULL,
	[Weight] [numeric](18, 2) NULL,
	[Freight_ToPay_Amount] [numeric](18, 2) NULL,
	[charge] [numeric](18, 2) NULL,
	[Lr_Date] [varchar](50) NULL,
	[Lr_No] [varchar](50) NULL,
	[Booked_By] [varchar](50) NULL,
	[Entry_Type] [varchar](50) NULL,
	[Non_Billable_Reason] [varchar](250) NULL DEFAULT (''),
	[IsBillable] [bit] NULL DEFAULT ((0)),
	[Party_DC_No] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_Embroidery_Jobwork_Receipt_Head] PRIMARY KEY CLUSTERED 
(
	[Embroidery_Jobwork_Receipt_Code] ASC,
	[Embroidery_Jobwork_Receipt_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Employee_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employee_Head](
	[Employee_IdNo] [int] NOT NULL,
	[Employee_Name] [varchar](100) NOT NULL,
	[Sur_name] [varchar](100) NOT NULL,
	[Salary_Bobin] [numeric](18, 2) NULL,
	[Card_No] [varchar](50) NULL,
 CONSTRAINT [PK_Employee_Head] PRIMARY KEY CLUSTERED 
(
	[Employee_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Employee_Head] UNIQUE NONCLUSTERED 
(
	[Sur_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EntryTemp]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntryTemp](
	[Name1] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name1]  DEFAULT (''),
	[Name2] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name2]  DEFAULT (''),
	[Name3] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name3]  DEFAULT (''),
	[Name4] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name4]  DEFAULT (''),
	[Name5] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name5]  DEFAULT (''),
	[Name6] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name6]  DEFAULT (''),
	[name7] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_name7]  DEFAULT (''),
	[Name8] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name8]  DEFAULT (''),
	[Name9] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name9]  DEFAULT (''),
	[Name10] [varchar](100) NULL CONSTRAINT [DF_EntryTemp_Name10]  DEFAULT (''),
	[Date1] [smalldatetime] NULL,
	[Date2] [smalldatetime] NULL,
	[Date3] [smalldatetime] NULL,
	[Date4] [smalldatetime] NULL,
	[Date5] [smalldatetime] NULL,
	[Int1] [int] NULL CONSTRAINT [DF_EntryTemp_Int1]  DEFAULT ((0)),
	[Int2] [int] NULL CONSTRAINT [DF_EntryTemp_Int2]  DEFAULT ((0)),
	[Int3] [int] NULL CONSTRAINT [DF_EntryTemp_Int3]  DEFAULT ((0)),
	[Int4] [int] NULL CONSTRAINT [DF_EntryTemp_Int4]  DEFAULT ((0)),
	[Int5] [int] NULL CONSTRAINT [DF_EntryTemp_Int5]  DEFAULT ((0)),
	[Int6] [int] NULL CONSTRAINT [DF_EntryTemp_Int6]  DEFAULT ((0)),
	[Int7] [int] NULL CONSTRAINT [DF_EntryTemp_Int7]  DEFAULT ((0)),
	[Int8] [int] NULL CONSTRAINT [DF_EntryTemp_Int8]  DEFAULT ((0)),
	[Int9] [int] NULL CONSTRAINT [DF_EntryTemp_Int9]  DEFAULT ((0)),
	[Int10] [int] NULL CONSTRAINT [DF_EntryTemp_Int10]  DEFAULT ((0)),
	[Meters1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters1]  DEFAULT ((0)),
	[Meters2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters2]  DEFAULT ((0)),
	[Meters3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters3]  DEFAULT ((0)),
	[Meters4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters4]  DEFAULT ((0)),
	[Meters5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters5]  DEFAULT ((0)),
	[Meters6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters6]  DEFAULT ((0)),
	[Meters7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters7]  DEFAULT ((0)),
	[Meters8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters8]  DEFAULT ((0)),
	[Meters9] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters9]  DEFAULT ((0)),
	[Meters10] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Meters10]  DEFAULT ((0)),
	[Weight1] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight1]  DEFAULT ((0)),
	[Weight2] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight2]  DEFAULT ((0)),
	[Weight3] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight3]  DEFAULT ((0)),
	[Weight4] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight4]  DEFAULT ((0)),
	[Weight5] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight5]  DEFAULT ((0)),
	[Weight6] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight6]  DEFAULT ((0)),
	[Weight7] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight7]  DEFAULT ((0)),
	[Weight8] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight8]  DEFAULT ((0)),
	[Weight9] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight9]  DEFAULT ((0)),
	[Weight10] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTemp_Weight10]  DEFAULT ((0)),
	[Currency1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency1]  DEFAULT ((0)),
	[Currency2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency2]  DEFAULT ((0)),
	[Currency3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency3]  DEFAULT ((0)),
	[Currency4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency4]  DEFAULT ((0)),
	[Currency5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency5]  DEFAULT ((0)),
	[Currency6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency6]  DEFAULT ((0)),
	[Currency7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency7]  DEFAULT ((0)),
	[Currency8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTemp_Currency8]  DEFAULT ((0)),
	[Currency9] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTemp_Currency9]  DEFAULT ((0)),
	[Currency10] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTemp_Currency10]  DEFAULT ((0))
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EntryTemp_Simple]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntryTemp_Simple](
	[Name1] [varchar](100) NULL,
	[Name2] [varchar](100) NULL,
	[Name3] [varchar](100) NULL,
	[Name4] [varchar](100) NULL,
	[Name5] [varchar](100) NULL,
	[Name6] [varchar](100) NULL,
	[name7] [varchar](100) NULL,
	[Name8] [varchar](100) NULL,
	[Name9] [varchar](100) NULL,
	[Name10] [varchar](100) NULL,
	[Date1] [smalldatetime] NULL,
	[Date2] [smalldatetime] NULL,
	[Date3] [smalldatetime] NULL,
	[Date4] [smalldatetime] NULL,
	[Date5] [smalldatetime] NULL,
	[Int1] [int] NULL,
	[Int2] [int] NULL,
	[Int3] [int] NULL,
	[Int4] [int] NULL,
	[Int5] [int] NULL,
	[Int6] [int] NULL,
	[Int7] [int] NULL,
	[Int8] [int] NULL,
	[Int9] [int] NULL,
	[Int10] [int] NULL,
	[Meters1] [numeric](18, 2) NULL,
	[Meters2] [numeric](18, 2) NULL,
	[Meters3] [numeric](18, 2) NULL,
	[Meters4] [numeric](18, 2) NULL,
	[Meters5] [numeric](18, 2) NULL,
	[Meters6] [numeric](18, 2) NULL,
	[Meters7] [numeric](18, 2) NULL,
	[Meters8] [numeric](18, 2) NULL,
	[Meters9] [numeric](18, 2) NULL,
	[Meters10] [numeric](18, 2) NULL,
	[Weight1] [numeric](18, 3) NULL,
	[Weight2] [numeric](18, 3) NULL,
	[Weight3] [numeric](18, 3) NULL,
	[Weight4] [numeric](18, 3) NULL,
	[Weight5] [numeric](18, 3) NULL,
	[Weight6] [numeric](18, 3) NULL,
	[Weight7] [numeric](18, 3) NULL,
	[Weight8] [numeric](18, 3) NULL,
	[Weight9] [numeric](18, 3) NULL,
	[Weight10] [numeric](18, 3) NULL,
	[Currency1] [numeric](18, 2) NULL,
	[Currency2] [numeric](18, 2) NULL,
	[Currency3] [numeric](18, 2) NULL,
	[Currency4] [numeric](18, 2) NULL,
	[Currency5] [numeric](18, 2) NULL,
	[Currency6] [numeric](18, 2) NULL,
	[Currency7] [numeric](18, 2) NULL,
	[Currency8] [numeric](18, 2) NULL,
	[Currency9] [numeric](18, 7) NULL,
	[Currency10] [numeric](18, 7) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EntryTempSub]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntryTempSub](
	[Name1] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name1]  DEFAULT (''),
	[Name2] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name2]  DEFAULT (''),
	[Name3] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name3]  DEFAULT (''),
	[Name4] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name4]  DEFAULT (''),
	[Name5] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name5]  DEFAULT (''),
	[Name6] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name6]  DEFAULT (''),
	[name7] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_name7]  DEFAULT (''),
	[Name8] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name8]  DEFAULT (''),
	[Name9] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name9]  DEFAULT (''),
	[Name10] [varchar](100) NULL CONSTRAINT [DF_EntryTempSub_Name10]  DEFAULT (''),
	[Date1] [smalldatetime] NULL,
	[Date2] [smalldatetime] NULL,
	[Date3] [smalldatetime] NULL,
	[Date4] [smalldatetime] NULL,
	[Date5] [smalldatetime] NULL,
	[Int1] [int] NULL CONSTRAINT [DF_EntryTempSub_Int1]  DEFAULT ((0)),
	[Int2] [int] NULL CONSTRAINT [DF_EntryTempSub_Int2]  DEFAULT ((0)),
	[Int3] [int] NULL CONSTRAINT [DF_EntryTempSub_Int3]  DEFAULT ((0)),
	[Int4] [int] NULL CONSTRAINT [DF_EntryTempSub_Int4]  DEFAULT ((0)),
	[Int5] [int] NULL CONSTRAINT [DF_EntryTempSub_Int5]  DEFAULT ((0)),
	[Int6] [int] NULL CONSTRAINT [DF_EntryTempSub_Int6]  DEFAULT ((0)),
	[Int7] [int] NULL CONSTRAINT [DF_EntryTempSub_Int7]  DEFAULT ((0)),
	[Int8] [int] NULL CONSTRAINT [DF_EntryTempSub_Int8]  DEFAULT ((0)),
	[Int9] [int] NULL CONSTRAINT [DF_EntryTempSub_Int9]  DEFAULT ((0)),
	[Int10] [int] NULL CONSTRAINT [DF_EntryTempSub_Int10]  DEFAULT ((0)),
	[Meters1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters1]  DEFAULT ((0)),
	[Meters2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters2]  DEFAULT ((0)),
	[Meters3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters3]  DEFAULT ((0)),
	[Meters4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters4]  DEFAULT ((0)),
	[Meters5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters5]  DEFAULT ((0)),
	[Meters6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters6]  DEFAULT ((0)),
	[Meters7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters7]  DEFAULT ((0)),
	[Meters8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters8]  DEFAULT ((0)),
	[Meters9] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters9]  DEFAULT ((0)),
	[Meters10] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Meters10]  DEFAULT ((0)),
	[Weight1] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight1]  DEFAULT ((0)),
	[Weight2] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight2]  DEFAULT ((0)),
	[Weight3] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight3]  DEFAULT ((0)),
	[Weight4] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight4]  DEFAULT ((0)),
	[Weight5] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight5]  DEFAULT ((0)),
	[Weight6] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight6]  DEFAULT ((0)),
	[Weight7] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight7]  DEFAULT ((0)),
	[Weight8] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight8]  DEFAULT ((0)),
	[Weight9] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight9]  DEFAULT ((0)),
	[Weight10] [numeric](18, 3) NULL CONSTRAINT [DF_EntryTempSub_Weight10]  DEFAULT ((0)),
	[Currency1] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency1]  DEFAULT ((0)),
	[Currency2] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency2]  DEFAULT ((0)),
	[Currency3] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency3]  DEFAULT ((0)),
	[Currency4] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency4]  DEFAULT ((0)),
	[Currency5] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency5]  DEFAULT ((0)),
	[Currency6] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency6]  DEFAULT ((0)),
	[Currency7] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency7]  DEFAULT ((0)),
	[Currency8] [numeric](18, 2) NULL CONSTRAINT [DF_EntryTempSub_Currency8]  DEFAULT ((0)),
	[Currency9] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTempSub_Currency9]  DEFAULT ((0)),
	[Currency10] [numeric](18, 7) NULL CONSTRAINT [DF_EntryTempSub_Currency10]  DEFAULT ((0))
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ESI_PF_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ESI_PF_Head](
	[ESI_PF_Group_IdNo] [smallint] NOT NULL,
	[ESI_PF_Group_Name] [varchar](100) NOT NULL,
	[ESI_PF_SurName] [varchar](50) NOT NULL,
	[ESI_AUDIT_STATUS] [int] NULL,
	[PF_AUDIT_STATUS] [int] NULL,
	[ESI_SALARY_STATUS] [int] NULL,
	[PF_SALARY_STATUS] [int] NULL,
 CONSTRAINT [PK_ESI_PF_Head] PRIMARY KEY CLUSTERED 
(
	[ESI_PF_Group_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Expense_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Expense_Head](
	[Expense_IdNo] [int] NOT NULL,
	[Expense_Name] [varchar](200) NULL,
	[Sur_Name] [varchar](200) NULL,
 CONSTRAINT [PK_Expense_Head] PRIMARY KEY CLUSTERED 
(
	[Expense_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FinancialRange_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinancialRange_Head](
	[Financial_Range] [varchar](20) NOT NULL,
 CONSTRAINT [PK_FinancialRange_Head] PRIMARY KEY CLUSTERED 
(
	[Financial_Range] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Gender_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Gender_Head](
	[Gender_IdNo] [int] NOT NULL,
	[Gender_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Gender_Head] PRIMARY KEY CLUSTERED 
(
	[Gender_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Gender_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_ADV]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_ADV](
	[PLACE_OF_SUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[GROSS_ADVANCE_RECEIVED] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_ADV_ADJ]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_ADV_ADJ](
	[PLACE_OF_SUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[GROSS_ADVANCE_RECEIVED_ADJUSTED] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_ADV_ADJ_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_ADV_ADJ_FILING_HISTORY](
	[PLACE_OF_SUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[GROSS_ADVANCE_RECEIVED_ADJUSTED] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_ADV_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_ADV_FILING_HISTORY](
	[PLACE_OF_SUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[GROSS_ADVANCE_RECEIVED] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_B2B]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_B2B](
	[GSTIN] [varchar](15) NULL,
	[RECEIVER_NAME] [varchar](200) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[REVERSE_CHARGE] [varchar](1) NULL,
	[APP_TAX_RATE] [numeric](18, 1) NULL,
	[INV_TYPE] [varchar](100) NULL,
	[ECOMGSTIN] [varchar](20) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](15) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_B2B_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_B2B_FILING_HISTORY](
	[GSTIN] [varchar](15) NOT NULL,
	[RECEIVER_NAME] [varchar](200) NOT NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[REVERSE_CHARGE] [varchar](1) NULL,
	[APP_TAX_RATE] [numeric](18, 1) NULL,
	[INV_TYPE] [varchar](100) NULL,
	[ECOMGSTIN] [varchar](20) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_B2CL]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_B2CL](
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[APP_TAX_RATE] [numeric](18, 1) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[ECOMGSTIN] [varchar](20) NULL,
	[CMP_GSTIN] [varchar](15) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_B2CL_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_B2CL_FILING_HISTORY](
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[APP_TAX_RATE] [nchar](10) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[ECOMGSTIN] [varchar](20) NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_B2CS]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_B2CS](
	[INV_TYPE] [nchar](10) NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[APP_TAX_RATE] [numeric](18, 1) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[ECOMGSTIN] [varchar](20) NULL,
	[CMP_GSTIN] [varchar](15) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_B2CS_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_B2CS_FILING_HISTORY](
	[INV_TYPE] [nchar](10) NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[APP_TAX_RATE] [numeric](18, 1) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[ECOMGSTIN] [varchar](20) NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_CDNR]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_CDNR](
	[GSTIN] [varchar](20) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[REFUND_VOUCHER_NO] [varchar](25) NULL,
	[REFUND_VOUCHER_DATE] [smalldatetime] NULL,
	[DOCUMENT_TYPE] [varchar](2) NULL,
	[REASON_FOR_ISSUEING_DOCUMENT] [nchar](100) NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[REFUND_VOUCHER_VALUE] [real] NOT NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[PRE_GST] [varchar](1) NULL,
	[CMP_GSTIN] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_CDNR_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_CDNR_FILING_HISTORY](
	[GSTIN] [varchar](20) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[REFUND_VOUCHER_NO] [varchar](25) NULL,
	[REFUND_VOUCHER_DATE] [smalldatetime] NULL,
	[DOCUMENT_TYPE] [varchar](2) NULL,
	[REASON_FOR_ISSUEING_DOCUMENT] [nchar](100) NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[REFUND_VOUCHER_VALUE] [real] NOT NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[PRE_GST] [varchar](1) NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_CDNUR]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_CDNUR](
	[URTYPE] [varchar](50) NULL,
	[REFUND_VOUCHER_NO] [varchar](25) NULL,
	[REFUND_VOUCHER_DATE] [smalldatetime] NULL,
	[DOCUMENT_TYPE] [varchar](2) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[REASON_FOR_ISSUEING_DOCUMENT] [nchar](100) NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[REFUND_VOUCHER_VALUE] [real] NOT NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[PRE_GST] [varchar](1) NULL,
	[CMP_GSTIN] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_CDNUR_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_CDNUR_FILING_HISTORY](
	[URTYPE] [varchar](50) NULL,
	[REFUND_VOUCHER_NO] [varchar](25) NULL,
	[REFUND_VOUCHER_DATE] [smalldatetime] NULL,
	[DOCUMENT_TYPE] [varchar](2) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[REASON_FOR_ISSUEING_DOCUMENT] [nchar](100) NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[REFUND_VOUCHER_VALUE] [real] NOT NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[PRE_GST] [varchar](1) NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_DOC]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_DOC](
	[Sl_No] [smallint] NULL,
	[TYPE_OF_DOCUMENT] [varchar](250) NULL,
	[SL_No_FROM] [varchar](50) NULL,
	[SL_No_TO] [varchar](50) NULL,
	[DOC_COUNT] [int] NULL,
	[CAN_DOC_COUNT] [int] NULL,
	[CMP_GSTIN] [varchar](15) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_DOC_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_DOC_FILING_HISTORY](
	[SL_No] [smallint] NULL,
	[TYPE_OF_DOCUMENT] [varchar](250) NULL,
	[SL_No_FROM] [varchar](50) NULL,
	[SL_No_TO] [varchar](50) NULL,
	[DOC_COUNT] [int] NULL,
	[CAN_DOC_COUNT] [int] NULL,
	[CMP_GSTIN] [varchar](15) NULL,
	[MONTH] [varchar](20) NULL,
	[YEAR] [varchar](10) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_EXEMP]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GSTR1_EXEMP](
	[NIL_RATED_INTERSTATE_REG] [real] NULL,
	[NIL_RATED_INTERSTATE_UNR] [real] NULL,
	[NIL_RATED_INTRASTATE_REG] [real] NULL,
	[NIL_RATED_INTRASTATE_UNR] [real] NULL,
	[EXEMP_RATED_INTERSTATE_REG] [real] NULL,
	[EXEMP_RATED_INTERSTATE_UNR] [real] NULL,
	[EXEMP_RATED_INTRASTATE_REG] [real] NULL,
	[EXEMP_RATED_INTRASTATE_UNR] [real] NULL,
	[NONGST_RATED_INTERSTATE_REG] [real] NULL,
	[NONGST_RATED_INTERSTATE_UNR] [real] NULL,
	[NONGST_RATED_INTRASTATE_REG] [real] NULL,
	[NONGST_RATED_INTRASTATE_UNR] [real] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GSTR1_EXEMP_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_EXEMP_FILING_HISTORY](
	[NIL_RATED_INTERSTATE_REG] [real] NULL,
	[NIL_RATED_INTERSTATE_UNR] [real] NULL,
	[NIL_RATED_INTRASTATE_REG] [real] NULL,
	[NIL_RATED_INTRASTATE_UNR] [real] NULL,
	[EXEMP_RATED_INTERSTATE_REG] [real] NULL,
	[EXEMP_RATED_INTERSTATE_UNR] [real] NULL,
	[EXEMP_RATED_INTRASTATE_REG] [real] NULL,
	[EXEMP_RATED_INTRASTATE_UNR] [real] NULL,
	[NONGST_RATED_INTERSTATE_REG] [real] NULL,
	[NONGST_RATED_INTERSTATE_UNR] [real] NULL,
	[NONGST_RATED_INTRASTATE_REG] [real] NULL,
	[NONGST_RATED_INTRASTATE_UNR] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_EXP]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_EXP](
	[EXPORT_TYPE] [varchar](20) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PORTCODE] [varchar](70) NULL,
	[SHIPPING_BILL_NO] [varchar](40) NULL,
	[SHIPPING_BILL_DATE] [smalldatetime] NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CMP_GSTIN] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_EXP_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_EXP_FILING_HISTORY](
	[EXPORT_TYPE] [varchar](20) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PORTCODE] [varchar](70) NULL,
	[SHIPPING_BILL_NO] [varchar](40) NULL,
	[SHIPPING_BILL_DATE] [smalldatetime] NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_HSN]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_HSN](
	[HSN_CODE] [varchar](10) NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[UQC] [varchar](50) NULL,
	[TOT_QTY] [real] NULL,
	[TOT_VALUE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST] [real] NULL,
	[CGST] [real] NULL,
	[SGST] [real] NULL,
	[CESS] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL,
	[GST_RATE] [real] NULL,
	[BILLYR] [varchar](10) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR1_HSN_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR1_HSN_FILING_HISTORY](
	[HSN] [varchar](10) NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[UQC] [varchar](50) NULL,
	[TOT_QTY] [real] NULL,
	[TOT_VALUE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST] [real] NULL,
	[CGST] [real] NULL,
	[SGST] [real] NULL,
	[CESS] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_ADV]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_ADV](
	[PLACE_OF_SUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[GROSS_ADVANCE_PAID] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_ADV_ADJ]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_ADV_ADJ](
	[PLACE_OF_SUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[GROSS_ADVANCE_PAID_ADJUSTED] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_ADV_ADJ_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_ADV_ADJ_FILING_HISTORY](
	[PLACE_OF_SUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[GROSS_ADVANCE_PAID_ADJUSTED] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_ADV_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_ADV_FILING_HISTORY](
	[PLACE_OF_SUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[GROSS_ADVANCE_PAID] [real] NULL,
	[CESS_AMOUNT] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_B2B]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_B2B](
	[GSTIN] [varchar](20) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[REVERSE_CHARGE] [varchar](1) NULL,
	[INV_TYPE] [varchar](100) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CGST_PAID] [real] NULL,
	[SGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILTY] [varchar](30) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CGST] [real] NULL,
	[AVAILED_ITC_SGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_B2B_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_B2B_FILING_HISTORY](
	[GSTIN] [varchar](20) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[REVERSE_CHARGE] [varchar](30) NULL,
	[INV_TYPE] [varchar](100) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CGST_PAID] [real] NULL,
	[SGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILTY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CGST] [real] NULL,
	[AVAILED_ITC_SGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_B2BUR]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_B2BUR](
	[SUPPLIER_NAME] [varchar](250) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[SUPPLY_TYPE] [varchar](50) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CGST_PAID] [real] NULL,
	[SGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILTY] [varchar](30) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CGST] [real] NULL,
	[AVAILED_ITC_SGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [varchar](15) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_B2BUR_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_B2BUR_FILING_HISTORY](
	[SUPPLIER_NAME] [varchar](250) NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[SUPPLY_TYPE] [varchar](50) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CGST_PAID] [real] NULL,
	[SGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILTY] [varchar](30) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CGST] [real] NULL,
	[AVAILED_ITC_SGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [varbinary](50) NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_CDNR]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_CDNR](
	[GSTIN] [varchar](20) NULL,
	[REFUND_VOUCHER_NO] [varchar](25) NULL,
	[REFUND_VOUCHER_DATE] [smalldatetime] NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[PRE_GST] [varchar](1) NULL,
	[DOCUMENT_TYPE] [varchar](2) NULL,
	[REASON_FOR_ISSUEING_DOCUMENT] [nchar](100) NULL,
	[SUPPLY_TYPE] [varchar](70) NULL,
	[REFUND_VOUCHER_VALUE] [real] NOT NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CGST_PAID] [real] NULL,
	[SGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILITY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CGST] [real] NULL,
	[AVAILED_ITC_SGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_CDNR_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_CDNR_FILING_HISTORY](
	[GSTIN] [varchar](20) NULL,
	[REFUND_VOUCHER_NO] [varchar](25) NULL,
	[REFUND_VOUCHER_DATE] [smalldatetime] NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[PRE_GST] [varchar](1) NULL,
	[DOCUMENT_TYPE] [varchar](2) NULL,
	[REASON_FOR_ISSUEING_DOCUMENT] [nchar](100) NULL,
	[SUPPLY_TYPE] [varchar](70) NULL,
	[REFUND_VOUCHER_VALUE] [real] NOT NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CGST_PAID] [real] NULL,
	[SGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILITY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CGST] [real] NULL,
	[AVAILED_ITC_SGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_CDNUR]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_CDNUR](
	[REFUND_VOUCHER_NO] [varchar](25) NULL,
	[REFUND_VOUCHER_DATE] [smalldatetime] NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[PRE_GST] [varchar](1) NULL,
	[DOCUMENT_TYPE] [varchar](2) NULL,
	[REASON_FOR_ISSUEING_DOCUMENT] [nchar](100) NULL,
	[SUPPLY_TYPE] [varchar](70) NULL,
	[REFUND_VOUCHER_VALUE] [real] NOT NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CGST_PAID] [real] NULL,
	[SGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILITY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CGST] [real] NULL,
	[AVAILED_ITC_SGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_CDNUR_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_CDNUR_FILING_HISTORY](
	[REFUND_VOUCHER_NO] [varchar](25) NULL,
	[REFUND_VOUCHER_DATE] [smalldatetime] NULL,
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[PRE_GST] [varchar](1) NULL,
	[DOCUMENT_TYPE] [varchar](2) NULL,
	[REASON_FOR_ISSUEING_DOCUMENT] [nchar](100) NULL,
	[SUPPLY_TYPE] [varchar](70) NULL,
	[REFUND_VOUCHER_VALUE] [real] NOT NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CGST_PAID] [real] NULL,
	[SGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILITY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CGST] [real] NULL,
	[AVAILED_ITC_SGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_EXEMP]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GSTR2_EXEMP](
	[COMPOSITE_INTERSTATE] [real] NULL,
	[COMPOSITE_INTRASTATE] [real] NULL,
	[NIL_RATED_INTERSTATE] [real] NULL,
	[NIL_RATED_INTRASTATE] [real] NULL,
	[EXEMP_RATED_INTERSTATE] [real] NULL,
	[EXEMP_RATED_INTRASTATE] [real] NULL,
	[NONGST_RATED_INTERSTATE] [real] NULL,
	[NONGST_RATED_INTRASTATE] [real] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GSTR2_EXEMP_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_EXEMP_FILING_HISTORY](
	[COMPOSITE_INTERSTATE] [real] NULL,
	[COMPOSITE_INTRASTATE] [real] NULL,
	[NIL_RATED_INTERSTATE] [real] NULL,
	[NIL_RATED_INTRASTATE] [real] NULL,
	[EXEMP_RATED_INTERSTATE] [real] NULL,
	[EXEMP_RATED_INTRASTATE] [real] NULL,
	[NONGST_RATED_INTERSTATE] [real] NULL,
	[NONGST_RATED_INTRASTATE] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_HSN]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_HSN](
	[HSN_CODE] [varchar](10) NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[UQC] [varchar](50) NULL,
	[TOT_QTY] [real] NULL,
	[TOT_VALUE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST] [real] NULL,
	[CGST] [real] NULL,
	[SGST] [real] NULL,
	[CESS] [real] NULL,
	[CMP_GSTIN] [varchar](15) NULL,
	[GST_RATE] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_HSN_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_HSN_FILING_HISTORY](
	[HSN] [varchar](10) NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[UQC] [varchar](50) NULL,
	[TOT_QTY] [real] NULL,
	[TOT_VALUE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST] [real] NULL,
	[CGST] [real] NULL,
	[SGST] [real] NULL,
	[CESS] [real] NULL,
	[CMP_GSTIN] [varchar](20) NULL,
	[GST_RATE] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_IMPG]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_IMPG](
	[PORT_CODE] [varchar](30) NULL,
	[BILL_OF_ENTRY_NO] [varchar](25) NULL,
	[BILL_OF_ENTRY_DATE] [smalldatetime] NULL,
	[BILL_OF_ENTRY_VALUE] [real] NULL,
	[DOCUMENT_TYPE] [varchar](50) NULL,
	[GSTIN_OF_SEZ_SUPPLIER] [varchar](20) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILTY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_IMPG_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_IMPG_FILING_HISTORY](
	[PORT_CODE] [varchar](30) NULL,
	[BILL_OF_ENTRY_NO] [varchar](25) NULL,
	[BILL_OF_ENTRY_DATE] [smalldatetime] NULL,
	[BILL_OF_ENTRY_VALUE] [real] NULL,
	[DOCUMENT_TYPE] [varchar](50) NULL,
	[GSTIN_OF_SEZ_SUPPLIER] [varchar](20) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILTY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_IMPS]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_IMPS](
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILTY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_IMPS_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_IMPS_FILING_HISTORY](
	[INVNO] [varchar](25) NULL,
	[INVDATE] [smalldatetime] NULL,
	[INVVALUE] [real] NULL,
	[PLACEOFSUPPLY] [varchar](70) NULL,
	[RATE] [real] NULL,
	[TAXABLE_VALUE] [real] NULL,
	[IGST_PAID] [real] NULL,
	[CESS_PAID] [real] NULL,
	[ITC_ELIGIBILTY] [varchar](1) NULL,
	[AVAILED_ITC_IGST] [real] NULL,
	[AVAILED_ITC_CESS] [real] NULL,
	[CMP_GSTIN] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_ITCR]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_ITCR](
	[ADD_REDUCE] [varchar](50) NULL,
	[ITC_IGST] [real] NULL,
	[ITC_CGST] [real] NULL,
	[ITC_SGST] [real] NULL,
	[ITC_CESS] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR2_ITCR_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR2_ITCR_FILING_HISTORY](
	[ADD_REDUCE] [varchar](50) NULL,
	[ITC_IGST] [real] NULL,
	[ITC_CGST] [real] NULL,
	[ITC_SGST] [real] NULL,
	[ITC_CESS] [real] NULL,
	[MONTH] [varchar](2) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR3B]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR3B](
	[TAXABLE_VALUE] [real] NULL,
	[IGST] [real] NULL,
	[CGST] [real] NULL,
	[SGST] [real] NULL,
	[CESS] [real] NULL,
	[ITC_IGST] [real] NULL,
	[ITC_CGST] [real] NULL,
	[ITC_SGST] [real] NULL,
	[ITC_CESS] [real] NULL,
	[CMP_GSTIN] [varchar](15) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GSTR3B_FILING_HISTORY]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GSTR3B_FILING_HISTORY](
	[TAXABLE_VALUE] [real] NULL,
	[IGST] [real] NULL,
	[CGST] [real] NULL,
	[SGST] [real] NULL,
	[CESS] [real] NULL,
	[ITC_IGST] [real] NULL,
	[ITC_CGST] [real] NULL,
	[ITC_SGST] [real] NULL,
	[ITC_CESS] [real] NULL,
	[CMP_GSTIN] [varchar](15) NULL,
	[MONTH] [varchar](20) NULL,
	[YEAR] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Holiday_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Holiday_Details](
	[Year_Code] [varchar](50) NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Holiday_Date] [varchar](50) NULL,
	[HolidayDateTime] [smalldatetime] NOT NULL,
	[Reason] [varchar](100) NULL,
 CONSTRAINT [PK_Holiday_Details] PRIMARY KEY CLUSTERED 
(
	[Year_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Holiday_Details] UNIQUE NONCLUSTERED 
(
	[HolidayDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Invoice_DC_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Invoice_DC_Details](
	[Sales_Code] [varchar](50) NOT NULL,
	[Sales_DC_Code] [varchar](50) NOT NULL,
	[Job_No] [varchar](100) NULL,
	[DC_Date] [smalldatetime] NULL,
	[UID] [varchar](200) NULL DEFAULT ('')
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Item_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Details](
	[Item_Idno] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[Size_IdNo] [int] NULL,
	[Purchase_Rate] [numeric](18, 2) NULL,
	[Sales_rate] [numeric](18, 2) NULL,
	[Piece_Box] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Item_Details] PRIMARY KEY CLUSTERED 
(
	[Item_Idno] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Item_ExcessShort_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Item_ExcessShort_Head](
	[Item_ExcessShort_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Item_ExcessShort_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Item_ExcessShort_Date] [smalldatetime] NOT NULL,
	[Item_IdNo] [smallint] NOT NULL,
	[Unit_IdNo] [smallint] NULL,
	[ExcessShort_Type] [varchar](50) NULL,
	[Quantity] [numeric](18, 3) NULL,
 CONSTRAINT [PK_Item_ExcessShort_Head] PRIMARY KEY CLUSTERED 
(
	[Item_ExcessShort_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Item_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Item_Head](
	[Item_IdNo] [int] NOT NULL,
	[Item_Name] [varchar](100) NOT NULL,
	[Sur_Name] [varchar](100) NOT NULL,
	[Item_Code] [varchar](50) NULL CONSTRAINT [DF_Item_Head_Item_Code]  DEFAULT (''),
	[ItemGroup_IdNo] [smallint] NULL CONSTRAINT [DF_Item_Head_ItemGroup_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Item_Head_Unit_IdNo]  DEFAULT ((0)),
	[Tax_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF_Item_Head_Tax_Perc]  DEFAULT ((0)),
	[Sale_TaxRate] [numeric](18, 2) NULL CONSTRAINT [DF_Item_Head_Tax_Rate]  DEFAULT ((0)),
	[Sales_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Item_Head_Rate]  DEFAULT ((0)),
	[Cost_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Item_Head_Cost_Rate]  DEFAULT ((0)),
	[Minimum_Stock] [numeric](18, 3) NULL DEFAULT ((0)),
	[Price_List_IdNo] [int] NULL DEFAULT ((0)),
	[Ledger_IdNo] [int] NULL DEFAULT ((0)),
	[Item_Name_Tamil] [varchar](35) NULL DEFAULT (''),
	[ISDEFAULT_ITEM_FOR_AUTO_BILL] [bit] NULL,
	[Item_Description] [varchar](500) NULL DEFAULT (''),
	[Gst_Percentage] [numeric](18, 2) NULL DEFAULT ((0)),
	[Gst_Rate] [numeric](18, 2) NULL DEFAULT ((0)),
	[Job_Work_Status] [int] NULL,
	[MRP_Rate] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_Item_Head] PRIMARY KEY CLUSTERED 
(
	[Item_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Item_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Item_Processing_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Item_Processing_Details](
	[Reference_Code] [varchar](35) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Reference_No] [varchar](20) NOT NULL,
	[For_OrderBy] [numeric](9, 2) NULL CONSTRAINT [DF_Item_Processing_Details_For_OrderBy]  DEFAULT ((0)),
	[Reference_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Item_Processing_Details_Ledger_IdNo]  DEFAULT ((0)),
	[Party_Bill_No] [varchar](20) NULL CONSTRAINT [DF_Item_Processing_Details_Party_Bill_No]  DEFAULT (''),
	[Sl_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL CONSTRAINT [DF_Item_Processing_Details_Item_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [int] NULL CONSTRAINT [DF_Item_Processing_Details_Unit_IdNo]  DEFAULT ((0)),
	[Quantity] [numeric](18, 3) NULL CONSTRAINT [DF_Item_Processing_Details_Quantity_Debit]  DEFAULT ((0)),
	[Size_Idno] [int] NULL DEFAULT ((0)),
	[Colour_IdNo] [int] NULL DEFAULT ((0)),
	[Design_IdNo] [int] NULL DEFAULT ((0)),
	[Gender_IdNo] [int] NULL DEFAULT ((0)),
	[Sleeve_IdNo] [int] NULL DEFAULT ((0)),
	[Weight] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_Item_Processing_Details] PRIMARY KEY CLUSTERED 
(
	[Reference_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Item_Stock_Selection_Processing_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Item_Stock_Selection_Processing_Details](
	[Item_IdNo] [int] NOT NULL,
	[Batch_No] [varchar](500) NOT NULL,
	[Manufactured_Day] [int] NULL,
	[Manufactured_Month_IdNo] [int] NULL,
	[Manufactured_Year] [int] NULL,
	[Manufactured_Date] [smalldatetime] NOT NULL,
	[Expiry_Period_Days] [int] NULL,
	[Expiry_Day] [int] NULL,
	[Expiry_Month_IdNo] [int] NULL,
	[Expiry_Year] [int] NULL,
	[Expiry_Date] [smalldatetime] NOT NULL,
	[Purchase_Rate] [numeric](18, 2) NULL,
	[Mrp_Rate] [numeric](18, 2) NOT NULL,
	[Sales_Rate] [numeric](18, 2) NULL,
	[Inward_Quantity] [numeric](18, 2) NULL,
	[OutWard_Quantity] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Item_Stock_Selection_Processing_Details] PRIMARY KEY CLUSTERED 
(
	[Item_IdNo] ASC,
	[Batch_No] ASC,
	[Manufactured_Date] ASC,
	[Expiry_Date] ASC,
	[Mrp_Rate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemGroup_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemGroup_Head](
	[ItemGroup_IdNo] [smallint] NOT NULL,
	[ItemGroup_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Commodity_Code] [varchar](20) NULL DEFAULT (''),
	[Item_HSN_Code] [varchar](50) NULL DEFAULT (''),
	[Item_GST_Percentage] [numeric](18, 2) NULL DEFAULT ((0)),
	[Cetegory_IdNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_ItemGroup_Head] PRIMARY KEY CLUSTERED 
(
	[ItemGroup_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ItemGroup_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Job_Card_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Job_Card_Details](
	[Job_Card_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Job_Card_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Job_Card_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Quantity] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Job_Card_Details] PRIMARY KEY CLUSTERED 
(
	[Job_Card_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Job_Card_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Job_Card_Head](
	[Job_Card_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Job_Card_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Job_Card_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Quantity] [numeric](18, 2) NULL,
	[Total_Quantity] [numeric](18, 2) NULL,
	[Total_WasteQuantity] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Job_Card_Head] PRIMARY KEY CLUSTERED 
(
	[Job_Card_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Job_Card_Waste_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Job_Card_Waste_Details](
	[Job_Card_Code] [varchar](100) NOT NULL,
	[Company_IdNo] [int] NULL,
	[Job_Card_No] [varchar](50) NULL,
	[for_OrderBy] [numeric](18, 2) NULL,
	[Job_Card_Date] [smalldatetime] NULL,
	[Ledger_IdNo] [int] NULL,
	[SL_No] [int] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Quantity] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Job_Card_Waste_Details] PRIMARY KEY CLUSTERED 
(
	[Job_Card_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[JobWork_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[JobWork_Head](
	[JobWork_Code] [varchar](35) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[JobWork_No] [varchar](20) NOT NULL,
	[For_OrderBy] [numeric](9, 2) NULL,
	[JobWork_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Item_IdNo] [int] NULL,
	[Unit_IdNo] [int] NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Size_Idno] [int] NULL,
	[Sales_Code] [varchar](50) NULL,
	[JobWork_Image] [image] NULL,
 CONSTRAINT [PK_JobWork_Head] PRIMARY KEY CLUSTERED 
(
	[JobWork_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Jobwork_Invoice_GST_Tax_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Jobwork_Invoice_GST_Tax_Details](
	[Jobwork_Invoice_Code] [varchar](50) NOT NULL,
	[Company_Idno] [int] NOT NULL,
	[Jobwork_Invoice_No] [varchar](50) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Jobwork_Invoice_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[HSN_Code] [varchar](100) NULL,
	[Taxable_Amount] [numeric](18, 2) NULL,
	[CGST_Percentage] [numeric](18, 2) NULL,
	[CGST_Amount] [numeric](18, 2) NULL,
	[SGST_Percentage] [numeric](18, 2) NULL,
	[SGST_Amount] [numeric](18, 2) NULL,
	[IGST_Percentage] [numeric](18, 2) NULL,
	[IGST_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Jobwork_Invoice_GST_Tax_Details] PRIMARY KEY CLUSTERED 
(
	[Jobwork_Invoice_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[JobWork_Project_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[JobWork_Project_Head](
	[JobWork_IdNo] [int] NOT NULL,
	[JobWork_Name] [varchar](300) NULL,
	[Sur_Name] [varchar](300) NULL,
	[Description] [varchar](500) NULL,
 CONSTRAINT [PK_JobWork_Project_Head] PRIMARY KEY CLUSTERED 
(
	[JobWork_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Knotting_Bill_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Knotting_Bill_Details](
	[Knotting_Bill_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Knotting_Bill_No] [varchar](30) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Knotting_Bill_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Knotting_Date] [smalldatetime] NULL,
	[Knotting_No] [varchar](20) NULL,
	[Shift] [varchar](20) NULL,
	[Ends] [int] NULL,
	[Loom] [varchar](200) NULL,
	[No_Pavu] [int] NULL,
	[Knotting_Code] [varchar](50) NULL,
 CONSTRAINT [PK_Knotting_Bill_Details] PRIMARY KEY CLUSTERED 
(
	[Knotting_Bill_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Knotting_Bill_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Knotting_Bill_Head](
	[Auto_BillNo] [int] IDENTITY(1,1) NOT NULL,
	[Knotting_Bill_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Knotting_Bill_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Knotting_Bill_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Entry_Type] [varchar](30) NULL,
	[Total_Pavu] [int] NULL,
	[Rate] [numeric](18, 2) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Knotting_Bill_Head] PRIMARY KEY CLUSTERED 
(
	[Knotting_Bill_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Knotting_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Knotting_Head](
	[Knotting_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Knotting_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Knotting_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Shift] [varchar](20) NULL,
	[Loom] [varchar](100) NULL,
	[Ends] [int] NULL,
	[No_Pavu] [int] NULL,
	[Knotting_Bill_Code] [varchar](50) NULL,
	[Knotting_IdNo] [int] NULL,
 CONSTRAINT [PK_Knotting_Head] PRIMARY KEY CLUSTERED 
(
	[Knotting_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ledger_AlaisHead]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ledger_AlaisHead](
	[Ledger_IdNo] [smallint] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Ledger_DisplayName] [varchar](200) NOT NULL,
	[Ledger_Type] [varchar](35) NULL DEFAULT (''),
	[AccountsGroup_IdNo] [int] NULL DEFAULT ((0)),
	[Area_IdNo] [int] NULL,
	[Agent_idNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Ledger_AlaisHead] PRIMARY KEY CLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_AlaisHead] UNIQUE NONCLUSTERED 
(
	[Ledger_DisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ledger_DiscountDetails]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ledger_DiscountDetails](
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[ItemGroup_IdNo] [int] NULL,
	[Discount_Percentage] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Ledger_DiscountDetails] PRIMARY KEY CLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ledger_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ledger_Head](
	[Ledger_IdNo] [int] NOT NULL,
	[Ledger_Name] [varchar](100) NOT NULL,
	[Sur_Name] [varchar](100) NOT NULL,
	[Ledger_MainName] [varchar](100) NOT NULL CONSTRAINT [DF_Ledger_Head_Ledger_MainName]  DEFAULT (''),
	[Ledger_AlaisName] [varchar](100) NULL CONSTRAINT [DF_Ledger_Head_Ledger_AlaisName]  DEFAULT (''),
	[Area_IdNo] [smallint] NULL CONSTRAINT [DF_Ledger_Head_Area_IdNo]  DEFAULT ((0)),
	[AccountsGroup_IdNo] [smallint] NULL CONSTRAINT [DF_Ledger_Head_AccountsGroup_IdNo]  DEFAULT ((0)),
	[Parent_Code] [varchar](50) NULL CONSTRAINT [DF_Ledger_Head_Parent_Code]  DEFAULT (''),
	[Bill_Type] [varchar](50) NULL CONSTRAINT [DF_Ledger_Head_Bill_Type]  DEFAULT (''),
	[Ledger_Address1] [varchar](100) NULL CONSTRAINT [DF_Ledger_Head_Ledger_Address1]  DEFAULT (''),
	[Ledger_Address2] [varchar](100) NULL CONSTRAINT [DF_Ledger_Head_Ledger_Address2]  DEFAULT (''),
	[Ledger_Address3] [varchar](100) NULL CONSTRAINT [DF_Ledger_Head_Ledger_Address3]  DEFAULT (''),
	[Ledger_Address4] [varchar](100) NULL CONSTRAINT [DF_Ledger_Head_Ledger_Address4]  DEFAULT (''),
	[Ledger_PhoneNo] [varchar](200) NULL CONSTRAINT [DF_Ledger_Head_Ledger_PhoneNo]  DEFAULT (''),
	[Ledger_TinNo] [varchar](50) NULL CONSTRAINT [DF_Ledger_Head_Ledger_TinNo]  DEFAULT (''),
	[Ledger_CstNo] [varchar](50) NULL CONSTRAINT [DF_Ledger_Head_Ledger_CstNo]  DEFAULT (''),
	[Ledger_Type] [varchar](50) NULL CONSTRAINT [DF_Ledger_Head_Ledger_Type]  DEFAULT (''),
	[Ledger_EmailID] [varchar](35) NULL DEFAULT (''),
	[Price_List_IdNo] [int] NULL DEFAULT ((0)),
	[Pan_No] [varchar](100) NULL DEFAULT (''),
	[Rent_Machine] [numeric](18, 2) NULL DEFAULT ((0)),
	[Free_Copies_Machine] [int] NULL DEFAULT ((0)),
	[Rate_Extra_Copy] [numeric](18, 2) NULL DEFAULT ((0)),
	[Machine_IdNo] [int] NULL DEFAULT ((0)),
	[Opening_Reading] [int] NULL DEFAULT ((0)),
	[Total_Machine] [int] NULL DEFAULT ((0)),
	[State_Idno] [int] NULL DEFAULT ((0)),
	[LedgerGroup_Idno] [int] NULL DEFAULT ((0)),
	[Rate_For_1000] [numeric](18, 2) NULL DEFAULT ((0)),
	[Minimum_Pcs] [numeric](18, 2) NULL DEFAULT ((0)),
	[Minimum_Bill_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Ledger_GSTinNo] [varchar](50) NULL DEFAULT (''),
	[Ledger_PanNo] [varchar](50) NULL DEFAULT (''),
	[Owner_Name] [varchar](200) NULL DEFAULT (''),
	[Birth_Date] [smalldatetime] NULL,
	[Wedding_Date] [smalldatetime] NULL,
	[Agent_idNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Ledger_Head] PRIMARY KEY CLUSTERED 
(
	[Ledger_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ledger_item_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ledger_item_Details](
	[Ledger_Item_Code] [varchar](50) NOT NULL,
	[Ledger_Item_No] [varchar](50) NOT NULL,
	[Company_IdNo] [int] NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Ledger_IdNo] [smallint] NULL,
	[Item_IdNo] [int] NULL,
	[Quantity] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Ledger_item_Details] PRIMARY KEY CLUSTERED 
(
	[Ledger_Item_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ledger_Item_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ledger_Item_Head](
	[Ledger_Item_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Ledger_Item_No] [varchar](20) NOT NULL,
	[For_OrderBy] [numeric](9, 2) NULL,
	[Ledger_IdNo] [int] NULL,
	[Total_Quantity] [numeric](18, 3) NULL,
 CONSTRAINT [PK_Ledger_Item_Head] PRIMARY KEY CLUSTERED 
(
	[Ledger_Item_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ledger_ItemName_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ledger_ItemName_Details](
	[Ledger_Idno] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Item_Idno] [int] NOT NULL,
	[Party_ItemName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Ledger_ItemName_Details] PRIMARY KEY NONCLUSTERED 
(
	[Ledger_Idno] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_ItemName_Details] UNIQUE NONCLUSTERED 
(
	[Ledger_Idno] ASC,
	[Item_Idno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ledger_PhoneNo_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ledger_PhoneNo_Head](
	[Ledger_IdNo] [smallint] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Ledger_PhoneNo] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Ledger_PhoneNo_Head] PRIMARY KEY CLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_PhoneNo_Head] UNIQUE NONCLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Ledger_PhoneNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ledger_Reading_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ledger_Reading_Details](
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Machine_IdNo] [int] NOT NULL,
	[Opening_Reading] [int] NULL,
 CONSTRAINT [PK_Ledger_Reading_Details] PRIMARY KEY NONCLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_Reading_Details] UNIQUE NONCLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Machine_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Loan_EMI_Settings]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loan_EMI_Settings](
	[Employee_IdNo] [int] NOT NULL,
	[Current_EMI] [numeric](9, 3) NULL,
 CONSTRAINT [PK_Loan_EMI_Settings] PRIMARY KEY CLUSTERED 
(
	[Employee_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Machine_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Machine_Head](
	[Machine_IdNo] [int] NOT NULL,
	[Machine_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Description] [varchar](100) NULL DEFAULT (''),
	[Machine_No] [varchar](100) NULL DEFAULT (''),
	[Machine_Make] [varchar](100) NULL DEFAULT (''),
	[Noof_Heads] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Machine_Head] PRIMARY KEY CLUSTERED 
(
	[Machine_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Machine_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Month_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Month_Head](
	[Month_IdNo] [tinyint] NOT NULL,
	[Month_Name] [varchar](30) NOT NULL,
	[Month_ShortName] [varchar](20) NOT NULL,
	[Idno] [tinyint] NOT NULL,
 CONSTRAINT [PK_Month_Head] PRIMARY KEY NONCLUSTERED 
(
	[Month_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order_Program_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order_Program_Details](
	[Order_Program_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Order_Program_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Order_Program_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Variety_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Size_IdNo] [smallint] NULL,
	[Binding_No] [varchar](50) NULL,
	[Quantity] [numeric](18, 2) NULL,
	[NO_of_SET] [varchar](50) NOT NULL,
	[No_Of_Copies] [varchar](50) NOT NULL,
	[Colour_Details] [varchar](250) NULL,
	[Paper_Details] [varchar](250) NULL,
 CONSTRAINT [PK_Order_Program_Details] PRIMARY KEY CLUSTERED 
(
	[Order_Program_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order_Program_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order_Program_Head](
	[Order_Program_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Order_Program_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Order_Program_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Order_Program_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Table_2_advance]  DEFAULT (''),
	[remarks] [varchar](500) NULL CONSTRAINT [DF_Order_Program_Head_remarks]  DEFAULT (''),
	[Printing_Order_Code] [varchar](50) NOT NULL,
	[Printing_Order_Details_SlNo] [int] NOT NULL,
	[Printing_Invoice_Code] [varchar](50) NOT NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_Code]  DEFAULT (''),
	[Printing_Invoice_slno] [tinyint] NOT NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_slno]  DEFAULT ((0)),
	[Printing_Invoice_Increment] [int] NULL CONSTRAINT [DF_Order_Program_Head_Printing_Invoice_Increment]  DEFAULT ((0)),
	[Size_IdNo] [int] NULL DEFAULT ((0)),
	[Colour_IdNo] [int] NULL DEFAULT ((0)),
	[Order_Image] [image] NULL,
	[Close_Status] [int] NULL DEFAULT ((0)),
	[Ordercode_forSelection] [varchar](100) NULL DEFAULT (''),
	[StchsPr_Pcs] [numeric](18, 2) NULL DEFAULT ((0)),
	[Stiches] [numeric](18, 2) NULL DEFAULT ((0)),
	[Pieces] [numeric](18, 2) NULL DEFAULT ((0)),
	[Receipt_Pieces] [numeric](18, 2) NULL DEFAULT ((0)),
	[Delivery_Pieces] [numeric](18, 2) NULL DEFAULT ((0)),
	[Production_Pieces] [numeric](18, 2) NULL DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Design] [varchar](100) NULL DEFAULT (''),
	[Billing_Name_IdNo] [int] NULL DEFAULT ((0)),
	[Style_Ref_No] [varchar](100) NULL DEFAULT (''),
 CONSTRAINT [PK_Order_Program_Head] PRIMARY KEY CLUSTERED 
(
	[Order_Program_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order_Selection_Code_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order_Selection_Code_Head](
	[Reference_Code] [varchar](50) NOT NULL,
	[Order_Selection_Code] [varchar](100) NULL,
 CONSTRAINT [PK_Order_Selection_Code_Head] PRIMARY KEY CLUSTERED 
(
	[Reference_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderJobNo_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderJobNo_Head](
	[OrderJobNo_IdNo] [int] NOT NULL,
	[OrderNo_Name] [varchar](50) NOT NULL,
	[OrderJobNo_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_OrderJobNo_Head] PRIMARY KEY CLUSTERED 
(
	[OrderJobNo_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderNo_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderNo_Head](
	[OrderNo_IdNo] [int] NOT NULL,
	[OrderNo_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_OrderNo_Head] PRIMARY KEY CLUSTERED 
(
	[OrderNo_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_OrderNo_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Other_GST_Entry_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Other_GST_Entry_Details](
	[Other_GST_Entry_Reference_Code] [varchar](50) NOT NULL,
	[Other_GST_Entry_Reference_No] [varchar](50) NOT NULL,
	[ForOrderBy_ReferenceCode] [numeric](18, 2) NOT NULL,
	[Other_GST_Entry_Type] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Other_GST_Entry_PrefixNo] [varchar](50) NOT NULL,
	[Other_GST_Entry_No] [varchar](50) NOT NULL,
	[Other_GST_Entry_RefNo] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Other_GST_Entry_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Sl_No] [int] NOT NULL,
	[Item_Particulars] [varchar](200) NULL,
	[Unit_IdNo] [smallint] NULL,
	[Hsn_Sac_Code] [varchar](50) NULL,
	[Gst_Perc] [numeric](18, 3) NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Discount_Perc] [numeric](18, 2) NULL,
	[Discount_Amount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
	[Footer_Cash_Discount_Perc] [numeric](18, 2) NULL,
	[Footer_Cash_Discount_Amount] [numeric](18, 2) NULL,
	[Taxable_Value] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Other_GST_Entry_Details] PRIMARY KEY CLUSTERED 
(
	[Other_GST_Entry_Reference_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Other_GST_Entry_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Other_GST_Entry_Head](
	[Other_GST_Entry_Reference_Code] [varchar](50) NOT NULL,
	[Other_GST_Entry_Reference_No] [varchar](50) NOT NULL,
	[ForOrderBy_ReferenceCode] [numeric](18, 2) NOT NULL,
	[Other_GST_Entry_Type] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Other_GST_Entry_PrefixNo] [varchar](50) NOT NULL,
	[Other_GST_Entry_No] [varchar](50) NOT NULL,
	[Other_GST_Entry_RefNo] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Other_GST_Entry_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Bill_No] [varchar](100) NULL,
	[Bill_Date] [smalldatetime] NULL,
	[Other_GST_Entry_Ac_IdNo] [int] NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[CashDiscount_Perc] [numeric](18, 2) NULL,
	[CashDiscount_Amount] [numeric](18, 2) NULL,
	[Taxable_Value] [numeric](18, 2) NULL,
	[CGST_Amount] [numeric](18, 2) NULL,
	[SGST_Amount] [numeric](18, 2) NULL,
	[IGST_AMount] [numeric](18, 2) NULL,
	[Chess_Amount] [numeric](18, 2) NULL,
	[Round_Off_Amount] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[TaxAmount_RoundOff_Status] [tinyint] NULL,
	[Total_Quantity] [numeric](18, 2) NULL,
	[Total_Sub_Amount] [numeric](18, 2) NULL,
	[Total_DiscountAmount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
	[Total_Footer_Cash_Discount_Amount] [numeric](18, 2) NULL,
	[Total_Taxable_Value] [numeric](18, 2) NULL,
	[Remarks] [varchar](1000) NULL,
	[User_Idno] [smallint] NULL,
	[Payment_Method] [varchar](50) NULL,
	[Unregister_Type] [varchar](100) NULL,
	[Reason_For_Issuing_Note] [varchar](100) NULL,
	[Tds_Percentage] [numeric](18, 2) NULL,
	[Tds_Amount] [numeric](18, 2) NULL,
	[Bill_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Other_GST_Entry_Head] PRIMARY KEY CLUSTERED 
(
	[Other_GST_Entry_Reference_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Other_GST_Entry_Tax_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Other_GST_Entry_Tax_Details](
	[Other_GST_Entry_Reference_Code] [varchar](50) NOT NULL,
	[Other_GST_Entry_Reference_No] [varchar](50) NOT NULL,
	[ForOrderBy_ReferenceCode] [numeric](18, 2) NOT NULL,
	[Other_GST_Entry_Type] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Other_GST_Entry_PrefixNo] [varchar](50) NOT NULL,
	[Other_GST_Entry_No] [varchar](50) NOT NULL,
	[Other_GST_Entry_RefNo] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Other_GST_Entry_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Sl_No] [int] NOT NULL,
	[HSN_SAC_Code] [varchar](100) NULL,
	[GST_Percentage] [numeric](18, 2) NULL,
	[Taxable_Amount] [numeric](18, 2) NULL,
	[CGST_Percentage] [numeric](18, 2) NULL,
	[CGST_Amount] [numeric](18, 2) NULL,
	[SGST_Percentage] [numeric](18, 2) NULL,
	[SGST_Amount] [numeric](18, 2) NULL,
	[IGST_Percentage] [numeric](18, 2) NULL,
	[IGST_Amount] [numeric](18, 2) NULL,
	[Chess_Perc] [numeric](18, 2) NULL,
	[Chess_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Other_GST_Entry_GST_Tax_Details_1] PRIMARY KEY CLUSTERED 
(
	[Other_GST_Entry_Reference_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Attendance_Timing_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Attendance_Timing_Details](
	[Employee_Attendance_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NULL,
	[Employee_Attendance_No] [varchar](30) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Employee_Attendance_Date] [smalldatetime] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Employee_IdNo] [smallint] NULL,
	[InOut_Type] [varchar](50) NULL,
	[InOut_Time_Text] [varchar](50) NULL,
	[InOut_DateTime] [datetime] NULL,
 CONSTRAINT [PK_PayRoll_Attendance_Timing_Details] PRIMARY KEY NONCLUSTERED 
(
	[Employee_Attendance_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PayRoll_Attendance_Timing_Details_1] UNIQUE NONCLUSTERED 
(
	[Company_IdNo] ASC,
	[Employee_Attendance_Date] ASC,
	[Employee_IdNo] ASC,
	[InOut_DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PayRoll_Attendance_Timing_Details_2] UNIQUE NONCLUSTERED 
(
	[Employee_Attendance_Code] ASC,
	[Employee_IdNo] ASC,
	[InOut_DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_AttendanceLog_FromMachine_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_AttendanceLog_FromMachine_Details](
	[AttendanceLog_FromMachine_Code] [varchar](30) NOT NULL,
	[AttendanceLog_FromMachine_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[AttendanceLog_FromMachine_Date] [smalldatetime] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[Employee_CardNo] [varchar](50) NULL,
	[IN_Out] [varchar](30) NULL,
	[INOut_DateTime_Text] [varchar](50) NULL,
	[INOut_DateTime] [datetime] NULL,
	[AttendanceLog_IP_Address] [varchar](50) NULL,
 CONSTRAINT [PK_Payroll_AttendanceLog_FromMachine_Details] PRIMARY KEY CLUSTERED 
(
	[AttendanceLog_FromMachine_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Payroll_AttendanceLog_FromMachine_Details] UNIQUE NONCLUSTERED 
(
	[Employee_CardNo] ASC,
	[INOut_DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_AttendanceLog_FromMachine_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_AttendanceLog_FromMachine_Head](
	[AttendanceLog_FromMachine_Code] [varchar](30) NOT NULL,
	[AttendanceLog_FromMachine_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[AttendanceLog_FromMachine_Date] [smalldatetime] NOT NULL,
	[AttendanceLog_IP_Address] [varchar](50) NULL,
 CONSTRAINT [PK_Payroll_AttendanceLog_FromMachine_Head] PRIMARY KEY CLUSTERED 
(
	[AttendanceLog_FromMachine_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Payroll_AttendanceLog_FromMachine_Head] UNIQUE NONCLUSTERED 
(
	[AttendanceLog_FromMachine_Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Bonus_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Bonus_Details](
	[Bonus_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Bonus_No] [varchar](50) NOT NULL,
	[Sl_No] [varchar](50) NOT NULL,
	[Employee_IdNo] [int] NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[M1] [numeric](18, 3) NULL,
	[M2] [numeric](18, 3) NULL,
	[M3] [numeric](18, 3) NULL,
	[M4] [numeric](18, 3) NULL,
	[M5] [numeric](18, 3) NULL,
	[M6] [numeric](18, 3) NULL,
	[M7] [numeric](18, 3) NULL,
	[M8] [numeric](18, 3) NULL,
	[M9] [numeric](18, 3) NULL,
	[M10] [numeric](18, 3) NULL,
	[M11] [numeric](18, 3) NULL,
	[M12] [numeric](18, 3) NULL,
	[M13] [numeric](18, 3) NULL,
	[M14] [numeric](18, 3) NULL,
	[Tot_Shifts] [numeric](18, 3) NULL,
	[Tot_Att] [numeric](18, 3) NULL,
	[Wage_Per_Day] [numeric](18, 3) NULL,
	[Total_Earnings] [numeric](18, 3) NULL,
	[Bonus_Earned] [numeric](18, 3) NULL,
	[Bonus_Finalised] [numeric](18, 3) NULL,
 CONSTRAINT [PK_Payroll_Bonus_Details] PRIMARY KEY CLUSTERED 
(
	[Bonus_Code] ASC,
	[Employee_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Bonus_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Bonus_Head](
	[Bonus_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Bonus_No] [int] NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Bonus_Date] [datetime] NOT NULL,
	[From_Date] [varchar](50) NULL,
	[To_Date] [varchar](50) NULL,
	[Max_Shifts] [numeric](18, 3) NULL,
	[Min_Shifts] [numeric](18, 3) NULL,
	[Min_Att_Reqd] [numeric](18, 3) NULL,
	[Exclude_WO] [bit] NULL,
	[Exclude_PH_LH] [bit] NULL,
	[Bonus_Rate] [numeric](18, 3) NULL,
	[M1] [varchar](30) NULL,
	[M2] [varchar](30) NULL,
	[M3] [varchar](30) NULL,
	[M4] [varchar](30) NULL,
	[M5] [varchar](30) NULL,
	[M6] [varchar](30) NULL,
	[M7] [varchar](30) NULL,
	[M8] [varchar](30) NULL,
	[M9] [varchar](30) NULL,
	[M10] [varchar](30) NULL,
	[M11] [varchar](30) NULL,
	[M12] [varchar](30) NULL,
	[M13] [varchar](30) NULL,
	[M14] [varchar](30) NULL,
	[Salary_Payment_Type_IdNo] [int] NULL,
	[Category_IdNo] [int] NULL,
 CONSTRAINT [PK_Payroll_Bonus_Head] PRIMARY KEY CLUSTERED 
(
	[Bonus_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Category_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayRoll_Category_Details](
	[Category_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[From_Attendance] [int] NULL,
	[To_Attendance] [int] NULL,
	[Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PayRoll_Category_Details] PRIMARY KEY CLUSTERED 
(
	[Category_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PayRoll_Category_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Category_Head](
	[Category_IdNo] [smallint] NOT NULL,
	[Category_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[In_Time_Shift1] [numeric](18, 2) NULL,
	[In_Time_Shift2] [numeric](18, 2) NULL,
	[In_Time_Shift3] [numeric](18, 2) NULL,
	[Lunch_minutes] [int] NULL,
	[Fixed_Rotation] [varchar](20) NULL,
	[OT_Allowed] [int] NULL,
	[Time_Delay] [int] NULL,
	[Attendance_Leave] [varchar](20) NULL,
	[Week_Attendance_OT] [int] NULL,
	[Attendance_Incentive] [int] NULL,
	[Out_Time_Shift1] [numeric](18, 2) NULL,
	[Out_Time_Shift2] [numeric](18, 2) NULL,
	[Out_Time_Shift3] [numeric](18, 2) NULL,
	[Monthly_Shift] [varchar](20) NULL,
	[OT_Allowed_After_Minutes] [int] NULL,
	[Minimum_Delay] [int] NULL,
	[Festival_Holidays] [int] NULL,
	[Incentive_Amount] [numeric](18, 2) NULL,
	[Working_Hours1] [numeric](18, 2) NULL,
	[Working_Hours2] [numeric](18, 2) NULL,
	[Working_Hours3] [numeric](18, 2) NULL,
	[No_Days_Month_Wages] [int] NULL,
	[Week_Off_Credit] [int] NULL,
	[Less_minute_Delay] [int] NULL,
	[Production_Incentive] [int] NULL,
	[Festival_Holidays_ot_Salary] [int] NULL,
	[Incentive_Amount_Days] [numeric](18, 2) NULL,
	[Shift1_In_Time] [varchar](30) NULL DEFAULT (''),
	[Shift1_Out_Time] [varchar](30) NULL DEFAULT (''),
	[Shift2_In_Time] [varchar](30) NULL DEFAULT (''),
	[Shift2_Out_Time] [varchar](30) NULL DEFAULT (''),
	[Shift3_In_Time] [varchar](30) NULL DEFAULT (''),
	[Shift3_Out_Time] [varchar](30) NULL DEFAULT (''),
	[Shift1_Working_Hours] [varchar](30) NULL DEFAULT (''),
	[Shift2_Working_Hours] [varchar](30) NULL DEFAULT (''),
	[Shift3_Working_Hours] [varchar](30) NULL DEFAULT (''),
	[Leave_Salary_Less] [smallint] NULL DEFAULT ((0)),
	[Att_Incentive_FromDays_Range1] [smallint] NULL DEFAULT ((0)),
	[Att_Incentive_ToDays_Range1] [smallint] NULL DEFAULT ((0)),
	[Att_Incentive_FromDays_Range2] [smallint] NULL DEFAULT ((0)),
	[Att_Incentive_ToDays_Range2] [smallint] NULL DEFAULT ((0)),
	[CL_Leave] [smallint] NULL DEFAULT ((0)),
	[SL_Leave] [smallint] NULL DEFAULT ((0)),
	[CL_Arrear_Type] [varchar](50) NULL DEFAULT (''),
	[SL_Arrear_Type] [varchar](50) NULL DEFAULT (''),
	[Shift1_Working_Minutes] [int] NULL DEFAULT ((0)),
	[Shift2_Working_Minutes] [int] NULL DEFAULT ((0)),
	[Shift3_Working_Minutes] [int] NULL DEFAULT ((0)),
	[Shift1_In_DateTime] [datetime] NULL,
	[Shift2_In_DateTime] [datetime] NULL,
	[Shift3_In_DateTime] [datetime] NULL,
	[Shift1_Out_DateTime] [datetime] NULL,
	[Shift2_Out_DateTime] [datetime] NULL,
	[Shift3_Out_DateTime] [datetime] NULL,
	[Office_TotalInHours_As_WorkedHours] [tinyint] NULL DEFAULT ((0)),
	[Office_TotalInHours_As_WorkedHours_Status] [tinyint] NULL DEFAULT ((0)),
	[CL_Arrear_Type_Year] [varchar](50) NULL DEFAULT ((0)),
	[SL_Arrear_Type_Year] [varchar](50) NULL DEFAULT ((0)),
	[Week_Off_Allowance] [int] NULL DEFAULT ((0)),
	[Min_Minutes_One_Shift_1] [smallint] NULL,
	[Min_Minutes_Half_Shift_1] [smallint] NULL,
	[Min_Minutes_One_Shift_2] [smallint] NULL,
	[Min_Minutes_Half_Shift_2] [smallint] NULL,
	[Min_Minutes_One_Shift_3] [smallint] NULL,
	[Min_Minutes_Half_Shift_3] [smallint] NULL,
	[Shift_Rotation_Status] [int] NULL,
 CONSTRAINT [PK_PayRoll_Category_Head] PRIMARY KEY NONCLUSTERED 
(
	[Category_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PayRoll_Category_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Attendance_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Attendance_Details](
	[Employee_Attendance_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Employee_Attendance_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Employee_Attendance_Date] [smalldatetime] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[Employee_IdNo] [int] NULL,
	[day_Shift] [int] NULL,
	[Night_Shift] [int] NULL,
	[Bonus_Shift] [numeric](18, 2) NULL,
	[Wages_Shift] [numeric](18, 2) NULL,
	[Tiffen] [numeric](18, 3) NULL,
	[Extra_Wages] [numeric](18, 2) NULL,
	[Total_Wages] [numeric](18, 3) NULL,
	[Late_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[Late_Minutes] [numeric](18, 2) NULL DEFAULT ((0)),
	[EarlyOut_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[EarlyOut_Minutes] [numeric](18, 2) NULL DEFAULT ((0)),
	[Shift_Minutes] [int] NULL DEFAULT ((0)),
	[Shift_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[Category_IdNo] [int] NULL DEFAULT ((0)),
	[Time] [numeric](18, 2) NULL DEFAULT ((0)),
	[Shift_IdNo] [int] NULL DEFAULT ((0)),
	[Add_Less_Minutes] [numeric](18, 2) NULL DEFAULT ((0)),
	[No_Of_Minutes] [numeric](18, 2) NULL DEFAULT ((0)),
	[No_Of_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[In_Out_Timings] [varchar](50) NULL DEFAULT (''),
	[Mess_Attendance] [numeric](18, 2) NULL DEFAULT ((0)),
	[No_Of_Shift] [numeric](18, 2) NULL DEFAULT ((0)),
	[Incentive_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[OT_Minutes] [numeric](18, 2) NULL DEFAULT ((0)),
	[OT_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[Permission_Absence_Duration] [numeric](6, 3) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Attendance_Details] PRIMARY KEY CLUSTERED 
(
	[Employee_Attendance_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PayRoll_Employee_Attendance_Details] UNIQUE NONCLUSTERED 
(
	[Employee_Attendance_Date] ASC,
	[Employee_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Attendance_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Attendance_Head](
	[Employee_Attendance_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Employee_Attendance_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Employee_Attendance_Date] [smalldatetime] NOT NULL,
	[Day_Name] [varchar](50) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Attendance_Head] PRIMARY KEY CLUSTERED 
(
	[Employee_Attendance_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PayRoll_Employee_Attendance_Head] UNIQUE NONCLUSTERED 
(
	[Employee_Attendance_Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Daily_Working_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Daily_Working_Head](
	[Reference_Code] [varchar](50) NOT NULL,
	[Reference_No] [varchar](50) NOT NULL,
	[Reference_Date] [smalldatetime] NOT NULL,
	[Employee_IdNo] [int] NOT NULL,
	[Start_Time] [datetime] NULL,
	[Start_Time_Text] [varchar](20) NULL,
	[End_Time] [datetime] NULL,
	[End_Time_Text] [varchar](20) NULL,
	[Work_Description] [varchar](1000) NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
 CONSTRAINT [PK_PayRoll_Employee_Daily_Working_Head] PRIMARY KEY CLUSTERED 
(
	[Reference_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Deduction_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Deduction_Details](
	[Employee_Deduction_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Employee_Deduction_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Employee_Deduction_Date] [smalldatetime] NOT NULL,
	[Employee_IdNo] [int] NULL,
	[Sl_No] [int] NOT NULL,
	[Advance_Deduction_Amount] [numeric](18, 3) NULL,
	[Mess] [numeric](18, 3) NULL,
	[Medical] [numeric](18, 3) NULL,
	[Store] [numeric](18, 3) NULL,
	[Other_Addition] [numeric](18, 3) NULL,
	[Quality_Fine] [numeric](18, 2) NULL,
	[Other_Deduction_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Deduction_Details] PRIMARY KEY CLUSTERED 
(
	[Employee_Deduction_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Deduction_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Deduction_Head](
	[Employee_Deduction_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Employee_Deduction_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Employee_Deduction_Date] [smalldatetime] NOT NULL,
	[Employee_IdNo] [int] NULL,
	[Advance_Deduction_Amount] [numeric](18, 3) NULL,
	[Remarks] [varchar](200) NULL,
	[Mess_Amount] [numeric](18, 2) NULL,
	[Mess] [numeric](18, 3) NULL,
	[Medical] [numeric](18, 3) NULL,
	[Store] [numeric](18, 3) NULL,
	[Other_Addition] [numeric](18, 3) NULL,
	[Other_Deduction] [numeric](18, 3) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Deduction_Head] PRIMARY KEY CLUSTERED 
(
	[Employee_Deduction_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Head](
	[Employee_IdNo] [smallint] NOT NULL,
	[Employee_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Card_No] [varchar](50) NULL,
	[Employee_Image] [image] NULL,
	[Join_date] [varchar](50) NULL,
	[Scheme_Starts] [varchar](50) NULL,
	[Payment_Type] [varchar](50) NULL,
	[shift_Day_Month] [varchar](50) NULL,
	[Week_Off] [varchar](50) NULL,
	[Trainee] [varchar](50) NULL,
	[Designation] [varchar](50) NULL,
	[Department] [varchar](50) NULL,
	[Dispensary] [varchar](50) NULL,
	[Esi_Status] [int] NULL,
	[Pf_Status] [int] NULL,
	[Esi_Salary] [int] NULL,
	[Pf_Salary] [int] NULL,
	[Esi_No] [int] NULL,
	[Pf_No] [int] NULL,
	[Esi_Join_Date] [varchar](50) NULL,
	[Esi_Leave_Date] [varchar](50) NULL,
	[Pf_Join_Date] [varchar](50) NULL,
	[Pf_Leave_Date] [varchar](50) NULL,
	[D_A] [numeric](18, 2) NULL,
	[H_R_A] [numeric](18, 2) NULL,
	[Esi_Conveyance] [numeric](18, 2) NULL,
	[Salary_Conveyance] [numeric](18, 2) NULL,
	[Washing] [numeric](18, 2) NULL,
	[Entertainment] [numeric](18, 2) NULL,
	[Maintenance] [numeric](18, 2) NULL,
	[Mess_Deduction] [numeric](18, 2) NULL,
	[wekk_Credit] [int] NULL,
	[OP_Balance] [numeric](18, 2) NULL,
	[Op_Att] [int] NULL,
	[Op_Amount] [int] NULL,
	[O_T_Salary] [numeric](18, 2) NULL,
	[Bank_Ac_No] [varchar](50) NULL,
	[Date_Birth] [varchar](50) NULL,
	[Age] [int] NULL,
	[Sex] [varchar](50) NULL,
	[Height] [int] NULL,
	[weight] [int] NULL,
	[Father_Husband] [varchar](50) NULL,
	[Marital_Status] [varchar](50) NULL,
	[No_Children] [int] NULL,
	[Qualification] [varchar](50) NULL,
	[Community] [varchar](50) NULL,
	[Blood_Group] [varchar](50) NULL,
	[address1] [varchar](50) NULL,
	[Address2] [varchar](50) NULL,
	[Address3] [varchar](50) NULL,
	[Village] [varchar](50) NULL,
	[Taulk] [varchar](50) NULL,
	[District] [varchar](50) NULL,
	[Phone_No] [varchar](50) NULL,
	[Mobile_No] [varchar](50) NULL,
	[Date_Status] [int] NULL,
	[Releave_Date] [varchar](50) NULL,
	[Reason] [varchar](100) NULL,
	[nonScheme_Total_Salary] [int] NULL,
	[NonScheme_Total_Esipf] [int] NULL,
	[NonScheme_Total_Ot] [int] NULL,
	[Category_IdNo] [int] NULL DEFAULT ((0)),
	[Join_DateTime] [smalldatetime] NULL,
	[Releave_DateTime] [smalldatetime] NULL,
	[Wages_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Opening_SalaryFor_Bonus] [numeric](18, 3) NULL DEFAULT ((0)),
	[Opening_CL_Leaves] [int] NULL DEFAULT ((0)),
	[Opening_ML_Leaves] [int] NULL DEFAULT ((0)),
	[Opening_WeekOff_Credits] [int] NULL DEFAULT ((0)),
	[Relation_Name1] [varchar](50) NULL DEFAULT (''),
	[Relation_Name2] [varchar](50) NULL DEFAULT (''),
	[Relation_Name3] [varchar](50) NULL DEFAULT (''),
	[Relation_Name4] [varchar](50) NULL DEFAULT (''),
	[Relation_Ship1] [varchar](50) NULL DEFAULT (''),
	[Relation_Ship2] [varchar](50) NULL DEFAULT (''),
	[Relation_Ship3] [varchar](50) NULL DEFAULT (''),
	[Relation_Ship4] [varchar](50) NULL DEFAULT (''),
	[RelationName_Image1] [image] NULL DEFAULT (''),
	[RelationName_Image2] [image] NULL DEFAULT (''),
	[RelationName_Image3] [image] NULL DEFAULT (''),
	[RelationName_Image4] [image] NULL DEFAULT (''),
	[Document_Name1] [varchar](50) NULL DEFAULT (''),
	[Document_Name2] [varchar](50) NULL DEFAULT (''),
	[Document_Name3] [varchar](50) NULL DEFAULT (''),
	[Document_Name4] [varchar](50) NULL DEFAULT (''),
	[Certificate1] [varchar](50) NULL DEFAULT (''),
	[Certificate2] [varchar](50) NULL DEFAULT (''),
	[Certificate3] [varchar](50) NULL DEFAULT (''),
	[Certificate4] [varchar](50) NULL DEFAULT (''),
	[Document_Image1] [image] NULL DEFAULT (''),
	[Document_Image2] [image] NULL DEFAULT (''),
	[Document_Image3] [image] NULL DEFAULT (''),
	[Document_Image4] [image] NULL DEFAULT (''),
	[Department_IdNo] [int] NULL DEFAULT ((0)),
	[Company_IdNo] [int] NULL DEFAULT ((0)),
	[Salary_Payment_Type_IdNo] [int] NULL DEFAULT ((0)),
	[Area_IdNo] [int] NULL DEFAULT ((0)),
	[Esi_For_OTSalary_Status] [tinyint] NULL,
	[PF_Credit_Status] [tinyint] NULL,
	[Employee_MainName] [varchar](200) NULL,
	[Bank_IdNo] [int] NULL,
	[ESI_PF_Group_IdNo] [int] NULL,
	[Mother_Tongue] [varchar](50) NULL,
	[bank_code] [varchar](50) NULL,
	[PAN_No] [varchar](20) NULL,
	[UAN_No] [varchar](20) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Head] PRIMARY KEY CLUSTERED 
(
	[Employee_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Duplicate_PayRoll_Employee_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Employee_Incentive_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Employee_Incentive_Details](
	[Incentive_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NULL,
	[Incentive_No] [varchar](30) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Incentive_Date] [smalldatetime] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Employee_IdNo] [smallint] NULL CONSTRAINT [DF_Payroll_Incentive_Details_Employee_IdNo]  DEFAULT ((0)),
	[Incentive_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_Incentive]  DEFAULT ((0)),
 CONSTRAINT [PK_Payroll_Employee_Incentive_Details] PRIMARY KEY CLUSTERED 
(
	[Incentive_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Employee_Incentive_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Employee_Incentive_Head](
	[Incentive_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Incentive_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Incentive_Date] [smalldatetime] NOT NULL,
	[Day_Name] [varchar](50) NULL CONSTRAINT [DF_Payroll_Employee_Incentive_Head_Day_Name]  DEFAULT (''),
 CONSTRAINT [PK_Payroll_Employee_Incentive_Head] PRIMARY KEY CLUSTERED 
(
	[Incentive_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Employee_OverTime_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Employee_OverTime_Details](
	[Timing_OverTime_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NULL,
	[Timing_OverTime_No] [varchar](30) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Timing_OverTime_Date] [smalldatetime] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Employee_IdNo] [smallint] NULL CONSTRAINT [DF_Payroll_Employee_OverTime_Details_Employee_IdNo]  DEFAULT ((0)),
	[OT_Minutes] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_OT_Minutes1]  DEFAULT ((0)),
	[OT_Hours] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_OT_Hours1]  DEFAULT ((0)),
 CONSTRAINT [PK_Payroll_Employee_OverTime_Details] PRIMARY KEY CLUSTERED 
(
	[Timing_OverTime_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Employee_OverTime_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Employee_OverTime_Head](
	[Timing_OverTime_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Timing_OverTime_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Timing_OverTime_Date] [smalldatetime] NOT NULL,
	[Day_Name] [varchar](50) NULL CONSTRAINT [DF_Payroll_Employee_OverTime_Head_Day_Name]  DEFAULT (''),
 CONSTRAINT [PK_Payroll_Employee_OverTime_Head] PRIMARY KEY CLUSTERED 
(
	[Timing_OverTime_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Payment_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Payment_Head](
	[Employee_Payment_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Employee_Payment_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Employee_Payment_Date] [smalldatetime] NOT NULL,
	[Employee_IdNo] [int] NULL,
	[Cash_Cheque] [varchar](20) NULL,
	[Advance_Salary] [varchar](20) NULL,
	[DebitAc_IdNo] [int] NULL,
	[Amount] [numeric](18, 2) NULL,
	[Remarks] [varchar](200) NULL,
	[Voucher_No] [varchar](30) NULL,
	[Voucher_Code] [varchar](30) NULL,
	[for_orderbyVoucher] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Payment_Head] PRIMARY KEY CLUSTERED 
(
	[Employee_Payment_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Employee_PermissionLeaveTime_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Employee_PermissionLeaveTime_Details](
	[Timing_PermissionLeaveTime_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NULL,
	[Timing_PermissionLeaveTime_No] [varchar](30) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Timing_PermissionLeaveTime_Date] [smalldatetime] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Employee_IdNo] [smallint] NULL CONSTRAINT [DF_Payroll_Employee_PermissionLeaveTime_Details_Employee_IdNo]  DEFAULT ((0)),
	[PermissionLeaveTime_Minutes] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_PermissionLeaveTimeMinutes1]  DEFAULT ((0)),
	[PermissionLeaveTime_Hours] [numeric](18, 2) NULL CONSTRAINT [DF_Table_1_PermissionLeaveTimeHours1]  DEFAULT ((0)),
 CONSTRAINT [PK_Payroll_Employee_PermissionLeaveTime_Details] PRIMARY KEY CLUSTERED 
(
	[Timing_PermissionLeaveTime_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Employee_PermissionLeaveTime_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Employee_PermissionLeaveTime_Head](
	[Timing_PermissionLeaveTime_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Timing_PermissionLeaveTime_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Timing_PermissionLeaveTime_Date] [smalldatetime] NOT NULL,
	[Day_Name] [varchar](50) NULL CONSTRAINT [DF_Payroll_Employee_PermissionLeaveTime_Head_Day_Name]  DEFAULT (''),
 CONSTRAINT [PK_Payroll_Employee_PermissionLeaveTime_Head] PRIMARY KEY CLUSTERED 
(
	[Timing_PermissionLeaveTime_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Releave_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Releave_Details](
	[Employee_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Join_Date] [varchar](50) NULL,
	[Releave_Date] [varchar](50) NULL,
	[Join_DateTime] [smalldatetime] NULL,
	[Releave_DateTime] [smalldatetime] NULL,
	[Reason] [varchar](200) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Releave_Details] PRIMARY KEY CLUSTERED 
(
	[Employee_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Salary_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Salary_Details](
	[Employee_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[From_Date] [varchar](50) NULL,
	[To_Date] [varchar](50) NULL,
	[For_Salary] [numeric](18, 3) NULL,
	[Esi_Pf] [numeric](18, 3) NULL,
	[O_T] [numeric](18, 3) NULL,
	[Other_Addition1] [numeric](18, 2) NULL DEFAULT ((0)),
	[Other_Addition2] [numeric](18, 2) NULL DEFAULT ((0)),
	[Week_Off_Allowance] [numeric](18, 2) NULL DEFAULT ((0)),
	[CL] [numeric](18, 2) NULL DEFAULT ((0)),
	[SL] [numeric](18, 2) NULL DEFAULT ((0)),
	[From_DateTime] [smalldatetime] NULL,
	[To_DateTime] [smalldatetime] NULL,
	[Provision] [numeric](18, 2) NULL DEFAULT ((0)),
	[D_A] [numeric](18, 2) NULL DEFAULT ((0)),
	[H_R_A] [numeric](18, 2) NULL DEFAULT ((0)),
	[Conveyance_Esi_Pf] [numeric](18, 2) NULL DEFAULT ((0)),
	[Conveyance_Salary] [numeric](18, 2) NULL DEFAULT ((0)),
	[Washing] [numeric](18, 2) NULL DEFAULT ((0)),
	[Entertainment] [numeric](18, 2) NULL DEFAULT ((0)),
	[Maintenance] [numeric](18, 2) NULL DEFAULT ((0)),
	[MessDeduction] [numeric](18, 2) NULL DEFAULT ((0)),
	[Other_Deduction2] [numeric](18, 2) NULL,
	[Other_Deduction1] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Employee_Scheme_PayRoll_Salary_Details] PRIMARY KEY CLUSTERED 
(
	[Employee_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Wages_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Wages_Details](
	[Employee_Wages_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Employee_Wages_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Shift_IdNo] [int] NULL,
	[Weight_From] [numeric](18, 3) NULL,
	[Weight_To] [numeric](18, 3) NULL,
	[Front_Sizing_Wages] [numeric](18, 2) NULL,
	[Back_Sizing_Wages] [numeric](18, 2) NULL,
	[Boiler_Wages] [numeric](18, 2) NULL,
	[Cooker_Wages] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Wages_Details] PRIMARY KEY CLUSTERED 
(
	[Employee_Wages_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Employee_Wages_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Employee_Wages_Head](
	[Employee_Wages_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Employee_Wages_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Employee_IdNo] [int] NOT NULL,
	[Front_Warper] [numeric](18, 2) NULL,
	[Back_Warper] [numeric](18, 2) NULL,
	[Helper] [numeric](18, 2) NULL,
	[Front_Sizer] [numeric](18, 2) NULL,
	[Back_Sizer] [numeric](18, 2) NULL,
	[Boiler] [numeric](18, 2) NULL,
	[Cooker] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PayRoll_Employee_Wages_Head] PRIMARY KEY CLUSTERED 
(
	[Employee_Wages_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Salary_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Salary_Details](
	[Salary_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Salary_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Salary_Date] [smalldatetime] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Employee_IdNo] [int] NULL,
	[Basic_Salary] [numeric](18, 3) NULL,
	[Total_Days] [numeric](18, 3) NULL,
	[Net_Pay] [numeric](18, 2) NULL,
	[No_Of_Attendance_Days] [numeric](18, 3) NULL,
	[From_W_off_CR] [numeric](18, 3) NULL,
	[Festival_Holidays] [numeric](18, 3) NULL,
	[No_Of_Leave] [numeric](18, 3) NULL,
	[Attendance_On_W_Off_FH] [numeric](18, 3) NULL,
	[Op_W_Off_CR] [numeric](18, 3) NULL,
	[Add_W_Off_CR] [numeric](18, 3) NULL,
	[Less_W_Off_CR] [numeric](18, 3) NULL,
	[Total_W_Off_CR] [numeric](18, 3) NULL,
	[Salary_Days] [numeric](18, 3) NULL,
	[Basic_Pay] [numeric](18, 3) NULL,
	[D_A] [numeric](18, 3) NULL,
	[Earning] [numeric](18, 3) NULL,
	[H_R_A] [numeric](18, 3) NULL,
	[Conveyance] [numeric](18, 3) NULL,
	[Washing] [numeric](18, 3) NULL,
	[Entertainment] [numeric](18, 3) NULL,
	[Maintenance] [numeric](18, 3) NULL,
	[Other_Addition] [numeric](18, 3) NULL,
	[Total_Addition] [numeric](18, 3) NULL,
	[Mess] [numeric](18, 3) NULL,
	[Medical] [numeric](18, 3) NULL,
	[Store] [numeric](18, 3) NULL,
	[ESI] [numeric](18, 3) NULL,
	[P_F] [numeric](18, 3) NULL,
	[E_P_F] [numeric](18, 3) NULL,
	[Pension_Scheme] [numeric](18, 3) NULL,
	[Other_Deduction] [numeric](18, 3) NULL,
	[Total_Deduction] [numeric](18, 3) NULL,
	[Attendance_Incentive] [numeric](18, 3) NULL,
	[Net_Salary] [numeric](18, 3) NULL,
	[Advance] [numeric](18, 3) NULL,
	[Day_For_Bonus] [numeric](18, 3) NULL,
	[Earning_For_Bonus] [numeric](18, 3) NULL,
	[Actual_Salary] [numeric](18, 2) NULL DEFAULT ((0)),
	[Week_Off_Allowance] [numeric](18, 2) NULL DEFAULT ((0)),
	[Other_Addition1] [numeric](18, 2) NULL DEFAULT ((0)),
	[Other_Addition2] [numeric](18, 2) NULL DEFAULT ((0)),
	[Minus_MainAdvance] [numeric](18, 2) NULL DEFAULT ((0)),
	[Salary_Pending] [numeric](18, 2) NULL DEFAULT ((0)),
	[Working_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[Salary_Shift] [numeric](18, 2) NULL DEFAULT ((0)),
	[Incentive_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_Salary] [numeric](18, 2) NULL DEFAULT ((0)),
	[OT_Minutes] [int] NULL DEFAULT ((0)),
	[Leave_Salary_Less] [numeric](18, 2) NULL DEFAULT ((0)),
	[Provision] [numeric](18, 2) NULL DEFAULT ((0)),
	[Late_Mins] [numeric](18, 2) NULL DEFAULT ((0)),
	[Late_Hours_Salary] [numeric](18, 2) NULL DEFAULT ((0)),
	[Net_Pay_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Add_CL_Leaves] [int] NULL DEFAULT ((0)),
	[Add_SL_Leaves] [int] NULL DEFAULT ((0)),
	[Total_Leave_Days] [numeric](18, 3) NULL DEFAULT ((0)),
	[Salary_Advance] [numeric](18, 3) NULL DEFAULT ((0)),
	[Total_Advance] [numeric](18, 3) NULL DEFAULT ((0)),
	[From_SL_For_Leave] [numeric](18, 3) NULL DEFAULT ((0)),
	[From_Cl_For_Leave] [numeric](18, 3) NULL DEFAULT ((0)),
	[Total_SL_CR_Days] [numeric](18, 2) NULL DEFAULT ((0)),
	[Less_SL_CR_Days] [numeric](18, 2) NULL DEFAULT ((0)),
	[OP_SL_CR_Days] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_CL_CR_Days] [numeric](18, 2) NULL DEFAULT ((0)),
	[Less_CL_CR_Days] [numeric](18, 2) NULL DEFAULT ((0)),
	[OP_CL_CR_Days] [numeric](18, 2) NULL DEFAULT ((0)),
	[OT_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[Ot_Pay_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[Ot_Salary] [numeric](18, 2) NULL DEFAULT ((0)),
	[Minus_Advance] [numeric](18, 2) NULL DEFAULT ((0)),
	[Balance_Advance] [numeric](18, 2) NULL DEFAULT ((0)),
	[PF_Credit_Amount] [numeric](18, 2) NULL,
	[E_P_S_AUDIT] [numeric](18, 2) NULL,
	[Card_No] [varchar](50) NULL,
	[Opening_Advance] [numeric](18, 2) NULL,
	[Signature_Status] [int] NULL,
	[ESI_AUDIT] [numeric](18, 3) NULL,
	[PF_AUDIT] [numeric](18, 3) NULL,
	[E_P_F_AUDIT] [numeric](18, 3) NULL,
	[OT_HOURS_HALF] [numeric](18, 3) NULL,
	[OT_ESI] [numeric](18, 2) NULL,
	[SALARY_OT_ESI] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PayRoll_Salary_Details] PRIMARY KEY CLUSTERED 
(
	[Salary_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Salary_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Salary_Head](
	[Salary_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Salary_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Salary_Date] [datetime] NOT NULL,
	[From_Date] [varchar](50) NULL,
	[To_Date] [varchar](50) NULL,
	[Total_Days] [numeric](18, 3) NULL,
	[Festival_Days] [numeric](18, 3) NULL,
	[Month_IdNo] [int] NULL CONSTRAINT [DF_PayRoll_Salary_Head_Month_IdNo]  DEFAULT ((0)),
	[Salary_Payment_Type_IdNo] [int] NULL DEFAULT ((0)),
	[Advance_UptoDate] [smalldatetime] NULL,
	[Salary_Year] [varchar](4) NULL,
	[Category_IdNo] [int] NULL,
	[ESI_PF_Group_IdNo] [int] NULL,
 CONSTRAINT [PK_PayRoll_Salary_Head] PRIMARY KEY CLUSTERED 
(
	[Salary_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Salary_Payment_Type_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Salary_Payment_Type_Head](
	[Salary_Payment_Type_IdNo] [smallint] NOT NULL,
	[Salary_Payment_Type_Name] [varchar](50) NOT NULL,
	[sur_name] [varchar](50) NOT NULL,
	[Monthly_Weekly] [varchar](50) NULL,
 CONSTRAINT [PK_PayRoll_Salary_Payment_Type_Head] PRIMARY KEY CLUSTERED 
(
	[Salary_Payment_Type_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Settings]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayRoll_Settings](
	[Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Employee_IdNo] [int] NULL,
	[Basic_Salary] [int] NULL,
	[Total_Days] [int] NULL,
	[Net_Pay] [int] NULL,
	[No_Of_Attendance_Days] [int] NULL,
	[From_W_off_CR] [int] NULL,
	[Festival_Holidays] [int] NULL,
	[No_Of_Leave] [int] NULL,
	[Attendance_On_W_Off_FH] [int] NULL,
	[Op_W_Off_CR] [int] NULL,
	[Add_W_Off_CR] [int] NULL,
	[Less_W_Off_CR] [int] NULL,
	[Total_W_Off_CR] [int] NULL,
	[Salary_Days] [int] NULL,
	[Basic_Pay] [int] NULL,
	[D_A] [int] NULL,
	[Earning] [int] NULL,
	[H_R_A] [int] NULL,
	[Conveyance] [int] NULL,
	[Washing] [int] NULL,
	[Entertainment] [int] NULL,
	[Maintenance] [int] NULL,
	[Other_Addition] [int] NULL,
	[Total_Addition] [int] NULL,
	[Mess] [int] NULL,
	[Medical] [int] NULL,
	[Store] [int] NULL,
	[ESI] [int] NULL,
	[P_F] [int] NULL,
	[E_P_F] [int] NULL,
	[Pension_Scheme] [int] NULL,
	[Other_Deduction] [int] NULL,
	[Total_Deduction] [int] NULL,
	[Attendance_Incentive] [int] NULL,
	[Net_Salary] [int] NULL,
	[Advance] [int] NULL,
	[Day_For_Bonus] [int] NULL,
	[Earning_For_Bonus] [int] NULL,
	[Working_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Working_Hours]  DEFAULT ((0)),
	[Salary_Shift] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Shift]  DEFAULT ((0)),
	[Incentive_Amount] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Incentive_Amount]  DEFAULT ((0)),
	[Total_Salary] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Salary]  DEFAULT ((0)),
	[OT_Minutes] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OT_Minutes]  DEFAULT ((0)),
	[OT_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OT_Hours]  DEFAULT ((0)),
	[Ot_Pay_Hours] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Ot_Pay_Hours]  DEFAULT ((0)),
	[Ot_Salary] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Ot_Salary]  DEFAULT ((0)),
	[Minus_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Minus_Advance]  DEFAULT ((0)),
	[Balance_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Balance_Advance]  DEFAULT ((0)),
	[Net_Pay_Amount] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Net_Pay_Amount]  DEFAULT ((0)),
	[Minus_MainAdvance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Minus_MainAdvance]  DEFAULT ((0)),
	[Salary_Pending] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Pending]  DEFAULT ((0)),
	[Total_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_SL_CR_Days]  DEFAULT ((0)),
	[Less_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Less_SL_CR_Days]  DEFAULT ((0)),
	[OP_SL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OP_SL_CR_Days]  DEFAULT ((0)),
	[Total_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_CL_CR_Days]  DEFAULT ((0)),
	[Less_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Less_CL_CR_Days]  DEFAULT ((0)),
	[OP_CL_CR_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_OP_CL_CR_Days]  DEFAULT ((0)),
	[From_Cl_For_Leave] [int] NULL CONSTRAINT [DF_PayRoll_Settings_From_Cl_For_Leave]  DEFAULT ((0)),
	[From_SL_For_Leave] [int] NULL CONSTRAINT [DF_PayRoll_Settings_From_SL_For_Leave]  DEFAULT ((0)),
	[Total_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Advance]  DEFAULT ((0)),
	[Salary_Advance] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Salary_Advance]  DEFAULT ((0)),
	[Total_Leave_Days] [int] NULL CONSTRAINT [DF_PayRoll_Settings_Total_Leave_Days]  DEFAULT ((0)),
	[Other_Deduction1] [int] NULL DEFAULT ((0)),
	[Week_Off_Allowance] [int] NULL DEFAULT ((0)),
	[Ded_Caption1] [varchar](100) NULL DEFAULT (''),
	[Ded_Caption2] [varchar](100) NULL DEFAULT (''),
	[Ded_Caption3] [varchar](100) NULL DEFAULT (''),
	[Add_Caption1] [varchar](100) NULL DEFAULT (''),
	[Add_Caption2] [varchar](100) NULL DEFAULT (''),
	[Add_Caption3] [varchar](100) NULL DEFAULT (''),
	[Add_Caption4] [varchar](100) NULL DEFAULT (''),
	[Add_Caption5] [varchar](100) NULL DEFAULT (''),
	[Add_Caption6] [varchar](100) NULL DEFAULT (''),
	[Add_Caption7] [varchar](100) NULL DEFAULT (''),
	[Add_Caption8] [varchar](100) NULL DEFAULT (''),
	[Other_Addition2] [int] NULL DEFAULT (''),
	[Other_Addition3] [int] NULL DEFAULT (''),
	[Provision] [int] NULL DEFAULT ((0)),
	[late_Mins] [int] NULL DEFAULT ((0)),
	[Late_Hours_Salary] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_PayRoll_Settings] PRIMARY KEY CLUSTERED 
(
	[Auto_SlNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Timing_Addition_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Timing_Addition_Details](
	[Timing_Addition_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NULL,
	[Timing_Addition_No] [varchar](30) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Timing_Addition_Date] [smalldatetime] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Employee_IdNo] [smallint] NULL,
	[InOut_Type] [varchar](50) NULL,
	[InOut_Time_Text] [varchar](50) NULL,
	[InOut_DateTime] [datetime] NULL,
 CONSTRAINT [PK_Payroll_Timing_Addition_Details] PRIMARY KEY NONCLUSTERED 
(
	[Timing_Addition_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Payroll_Timing_Addition_Details_1] UNIQUE NONCLUSTERED 
(
	[Company_IdNo] ASC,
	[Employee_IdNo] ASC,
	[InOut_DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Payroll_Timing_Addition_Details_2] UNIQUE NONCLUSTERED 
(
	[Timing_Addition_Code] ASC,
	[Employee_IdNo] ASC,
	[InOut_DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll_Timing_Addition_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll_Timing_Addition_Head](
	[Timing_Addition_Code] [varchar](30) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Timing_Addition_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Timing_Addition_Date] [smalldatetime] NOT NULL,
	[Day_Name] [varchar](50) NULL,
 CONSTRAINT [PK_Payroll_Timing_Addition_Head] PRIMARY KEY CLUSTERED 
(
	[Timing_Addition_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayRoll_Warp_Count_Coolie_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayRoll_Warp_Count_Coolie_Details](
	[Sl_No] [smallint] NOT NULL,
	[Count_IdNo] [int] NOT NULL,
	[Value] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PayRoll_Warp_Count_Coolie_Details] PRIMARY KEY CLUSTERED 
(
	[Count_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Price_List_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Price_List_Details](
	[Price_List_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Rate] [numeric](18, 3) NULL,
	[Minimum_Stitches] [smallint] NULL,
	[Rate_Per_1000_Stitches] [numeric](18, 3) NULL,
	[Minimum_Amount] [numeric](18, 3) NULL,
	[Size_IdNo] [int] NULL,
 CONSTRAINT [PK_Price_List_Details] PRIMARY KEY NONCLUSTERED 
(
	[Price_List_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Price_List_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Price_List_Head](
	[Price_List_IdNo] [int] NOT NULL,
	[Price_List_Name] [varchar](50) NOT NULL,
	[sur_name] [varchar](50) NOT NULL,
	[Ledger_IdNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Price_List_Head] PRIMARY KEY NONCLUSTERED 
(
	[Price_List_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Printing_Invoice_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Printing_Invoice_Details](
	[Printing_Invoice_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Printing_Invoice_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Printing_Invoice_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Variety_IdNo] [int] NOT NULL,
	[Unit_IdNo] [smallint] NOT NULL,
	[Quantity] [numeric](18, 2) NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[Printing_Invoice_slno] [int] IDENTITY(1,1) NOT NULL,
	[Order_Program_Code] [varchar](50) NOT NULL,
	[Order_No] [varchar](50) NULL,
 CONSTRAINT [PK_Printing_Invoice_Details] PRIMARY KEY CLUSTERED 
(
	[Printing_Invoice_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Printing_Invoice_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Printing_Invoice_Head](
	[Printing_Invoice_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Printing_Invoice_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Printing_Invoice_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Assesable_Amount] [numeric](18, 2) NULL,
	[Other_Charges] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Printing_Invoice_Head] PRIMARY KEY CLUSTERED 
(
	[Printing_Invoice_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Printing_Order_colour_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Printing_Order_colour_Details](
	[Printing_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Printing_Order_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Printing_Order_Date] [smalldatetime] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Colour_IdNo] [smallint] NULL,
	[Detail_SlNo] [int] NOT NULL,
 CONSTRAINT [PK_Printing_Order_colour_Details] PRIMARY KEY CLUSTERED 
(
	[Printing_Order_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Printing_Order_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Printing_Order_Details](
	[Printing_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Printing_Order_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Printing_Order_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Variety_IdNo] [int] NULL,
	[Colour_Details] [varchar](250) NULL,
	[Unit_IdNo] [smallint] NULL,
	[Size_IdNo] [smallint] NULL,
	[Paper_Details] [varchar](250) NULL,
	[Order_no] [varchar](50) NULL,
	[Binding_No] [varchar](50) NULL,
	[Quantity] [numeric](18, 2) NULL,
	[NO_of_SET] [varchar](50) NOT NULL,
	[No_Of_Copies] [varchar](50) NOT NULL,
	[Printing_Order_Details_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Order_Program_Code] [varchar](50) NULL,
	[Order_Program_Increment] [int] NULL,
	[Details_SlNo] [int] NULL,
	[Order_No_New] [varchar](30) NULL,
	[Cancel_Status] [tinyint] NULL,
 CONSTRAINT [PK_Printing_Order_Details] PRIMARY KEY CLUSTERED 
(
	[Printing_Order_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Printing_Order_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Printing_Order_Head](
	[Printing_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Printing_Order_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Printing_Order_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[advance] [numeric](18, 2) NULL,
	[remarks] [varchar](500) NULL,
	[Advance_Date] [varchar](30) NULL,
 CONSTRAINT [PK_Printing_Order_Head] PRIMARY KEY CLUSTERED 
(
	[Printing_Order_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Printing_Order_Paper_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Printing_Order_Paper_Details](
	[Printing_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Printing_Order_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Printing_Order_Date] [smalldatetime] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Paper_IdNo] [smallint] NULL,
	[Detail_SlNo] [int] NOT NULL,
 CONSTRAINT [PK_Printing_Order_Paper_Details] PRIMARY KEY CLUSTERED 
(
	[Printing_Order_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Production_Cost]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Production_Cost](
	[UID] [varchar](50) NOT NULL,
	[Production_Cost] [numeric](18, 3) NULL,
	[Remarks] [varchar](150) NULL,
 CONSTRAINT [PK_Production_Cost] PRIMARY KEY CLUSTERED 
(
	[UID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Production_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Production_Details](
	[Production_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Production_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Production_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[SL_No] [smallint] NOT NULL,
	[Order_No] [varchar](20) NULL,
	[Colour_IdNo] [int] NULL,
	[Design] [varchar](500) NULL,
	[StchsPr_Pcs] [numeric](18, 3) NULL,
	[Head] [numeric](18, 3) NULL,
	[Stiches] [numeric](18, 3) NULL,
	[Pieces] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 3) NULL,
	[Amount] [numeric](18, 3) NULL,
	[Size_IdNo] [int] NULL DEFAULT ((0)),
	[Ordercode_forSelection] [varchar](100) NULL DEFAULT (''),
	[Job_No] [varchar](100) NULL DEFAULT (''),
 CONSTRAINT [PK_Production_Details] PRIMARY KEY CLUSTERED 
(
	[Production_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Production_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Production_Head](
	[Production_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Production_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Production_Date] [datetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[Shift] [varchar](500) NULL,
	[Machine_IdNo] [int] NULL,
	[Operator_IdNo] [int] NULL,
	[Framer_IdNo] [int] NULL,
	[Total_Heads] [numeric](18, 3) NULL,
	[Total_Stchs] [numeric](18, 3) NULL,
	[Total_Pcs] [numeric](18, 3) NULL,
	[Total_Amt] [numeric](18, 3) NULL,
	[Incharge_IdNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Production_Head] PRIMARY KEY CLUSTERED 
(
	[Production_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_BatchNo_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_BatchNo_Details](
	[Purchase_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Batch_No] [varchar](100) NULL,
	[Quantity] [numeric](18, 2) NULL,
	[Item_idNo] [int] NULL,
	[Detail_SlNo] [int] NULL,
 CONSTRAINT [PK_Purchase_BatchNo_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Purchase_BatchNo_Details_1] UNIQUE NONCLUSTERED 
(
	[Purchase_Code] ASC,
	[Detail_SlNo] ASC,
	[Batch_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_Details](
	[Purchase_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_No] [varchar](20) NOT NULL CONSTRAINT [DF_Purhase_Details_Purhase_No]  DEFAULT (''),
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Purhase_Details_for_OrderBy]  DEFAULT ((0)),
	[Purchase_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Purhase_Details_Ledger_IdNo]  DEFAULT ((0)),
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL CONSTRAINT [DF_Purhase_Details_Item_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Purhase_Details_Unit_IdNo]  DEFAULT ((0)),
	[Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Purhase_Details_Noof_Items]  DEFAULT ((0)),
	[Bales] [int] NULL,
	[Weight] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 3) NULL CONSTRAINT [DF_Purhase_Details_Rate]  DEFAULT ((0)),
	[Tax_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Details_Tax_Rate]  DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Details_Total_Amount1]  DEFAULT ((0)),
	[Discount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Details_Discount_Perc]  DEFAULT ((0)),
	[Discount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Details_Discount_Amount]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Details_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Details_Tax_Amount]  DEFAULT ((0)),
	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Details_Amount]  DEFAULT ((0)),
	[Bale_Nos] [varchar](500) NULL,
	[TaxAmount_Difference] [numeric](18, 2) NULL DEFAULT ((0)),
	[Size_IdNo] [int] NULL DEFAULT ((0)),
	[Bags] [int] NULL DEFAULT ((0)),
	[Purchase_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Entry_Type] [varchar](50) NULL DEFAULT (''),
	[Purchase_Order_Code] [varchar](50) NULL DEFAULT (''),
	[Purchase_Order_Detail_SlNo] [int] NULL DEFAULT ((0)),
	[Colour_IdNo] [int] NULL DEFAULT ((0)),
	[Design_IdNo] [int] NULL DEFAULT ((0)),
	[Gender_IdNo] [int] NULL DEFAULT ((0)),
	[Sleeve_IdNo] [int] NULL DEFAULT ((0)),
	[OrderCode_forSelection] [varchar](200) NULL DEFAULT (''),
	[Description] [varchar](100) NULL,
	[Cash_Discount_Perc_For_All_Item] [numeric](18, 2) NULL DEFAULT ((0)),
	[Cash_Discount_Amount_For_All_Item] [numeric](18, 2) NULL DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL DEFAULT ((0)),
	[HSN_Code] [varchar](50) NULL DEFAULT (''),
	[GST_Percentage] [numeric](18, 2) NULL DEFAULT ((0)),
	[Detail_SlNo] [int] NULL DEFAULT ((0)),
	[Discount_Perc_Item] [numeric](18, 2) NULL DEFAULT ((0)),
	[Item_Code] [varchar](50) NULL DEFAULT (''),
	[Batch_Serial_No] [varchar](500) NULL DEFAULT (''),
	[Manufacture_Day] [numeric](18, 2) NULL DEFAULT ((0)),
	[Manufacture_Year] [numeric](18, 2) NULL DEFAULT ((0)),
	[Expiry_Period_Days] [numeric](18, 2) NULL DEFAULT ((0)),
	[Expiry_Day] [numeric](18, 2) NULL DEFAULT ((0)),
	[Manufacture_Date] [smalldatetime] NULL,
	[Expiry_Date] [smalldatetime] NULL,
	[Expiry_Year] [numeric](18, 2) NULL DEFAULT ((0)),
	[Mrp] [numeric](18, 2) NULL DEFAULT ((0)),
	[Sales_Price] [numeric](18, 2) NULL DEFAULT ((0)),
	[Expiry_Month_IdNo] [int] NULL DEFAULT ((0)),
	[Manufacture_Month_IdNo] [int] NULL DEFAULT ((0)),
	[Free_Qty] [int] NULL DEFAULT ((0)),
	[Discount_Amount_Item] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_Purhase_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_GST_Tax_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_GST_Tax_Details](
	[Purchase_Code] [varchar](50) NOT NULL,
	[Company_Idno] [int] NOT NULL,
	[Purchase_No] [varchar](50) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[HSN_Code] [varchar](100) NULL,
	[Taxable_Amount] [numeric](18, 2) NULL,
	[CGST_Percentage] [numeric](18, 2) NULL,
	[CGST_Amount] [numeric](18, 2) NULL,
	[SGST_Percentage] [numeric](18, 2) NULL,
	[SGST_Amount] [numeric](18, 2) NULL,
	[IGST_Percentage] [numeric](18, 2) NULL,
	[IGST_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Purchase_GST_Tax_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_Head](
	[Purchase_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Purhase_Head_for_OrderBy]  DEFAULT ((0)),
	[Purchase_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL CONSTRAINT [DF_Purhase_Head_Payment_Method]  DEFAULT (''),
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Purhase_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Bill_No] [varchar](20) NULL CONSTRAINT [DF_Purchase_Head_Bill_No]  DEFAULT (''),
	[PurchaseAc_IdNo] [int] NULL CONSTRAINT [DF_Purhase_Head_PurchaseAc_IdNo]  DEFAULT ((0)),
	[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_Purhase_Head_Payment_Method1]  DEFAULT (''),
	[TaxAc_IdNo] [int] NULL,
	[Narration] [varchar](1000) NULL CONSTRAINT [DF_Purhase_Head_Narration]  DEFAULT (''),
	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Purchase_Head_Vehicle_No]  DEFAULT (''),
	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Purhase_Head_Total_Qty]  DEFAULT ((0)),
	[Total_Bags] [int] NULL,
	[Total_Weight] [numeric](18, 3) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_Sub_Total]  DEFAULT ((0)),
	[Total_DiscountAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_Total_TaxAmount1]  DEFAULT ((0)),
	[Total_TaxAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_Total_TaxAmount]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_CashDiscount_Perc1]  DEFAULT ((0)),
	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_CashDiscount_Perc]  DEFAULT ((0)),
	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_CashDiscount_Amount]  DEFAULT ((0)),
	[AddLess_BeforeTax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Head_AddLess_Amount1]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Head_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Head_Tax_Amount]  DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL,
	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Head_Freight_Amount]  DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_AddLess_Amount]  DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_Round_Off]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Head_Net_Amount]  DEFAULT ((0)),
	[Bale_Nos] [varchar](500) NULL,
	[Entry_Type] [varchar](50) NULL DEFAULT (''),
	[Freight_ToPay_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[TradeDiscount_Perc] [numeric](18, 2) NULL DEFAULT ((0)),
	[TradeDiscount_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Transport_IdNo] [int] NULL DEFAULT ((0)),
	[Weight] [numeric](18, 3) NULL DEFAULT ((0)),
	[Against_CForm_Status] [int] NULL DEFAULT ((0)),
	[Order_No] [varchar](35) NULL DEFAULT (''),
	[Order_Date] [varchar](20) NULL DEFAULT (''),
	[Lr_No] [varchar](35) NULL DEFAULT (''),
	[Lr_Date] [varchar](20) NULL DEFAULT (''),
	[Document_Through] [varchar](50) NULL DEFAULT (''),
	[Booked_By] [varchar](50) NULL DEFAULT (''),
	[Despatch_To] [varchar](50) NULL DEFAULT (''),
	[SalesAc_IdNo] [int] NULL DEFAULT ((0)),
	[Place_Of_Supply] [varchar](100) NULL DEFAULT (''),
	[Entry_VAT_GST_Type] [varchar](100) NULL DEFAULT (''),
	[Electronic_Reference_No] [varchar](100) NULL DEFAULT (''),
	[Transportation_Mode] [varchar](100) NULL DEFAULT (''),
	[Date_Time_Of_Supply] [varchar](100) NULL DEFAULT (''),
	[Entry_GST_Tax_Type] [varchar](50) NULL DEFAULT (''),
	[CGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[SGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[IGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Sales_Order_Selection_Code] [varchar](50) NULL DEFAULT (''),
	[Total_DiscountAmount_item] [numeric](18, 2) NULL DEFAULT ((0)),
	[Aessable_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[AddLess_Name] [varchar](50) NULL DEFAULT (''),
	[Freight_Name] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_Purhase_Head] PRIMARY KEY CLUSTERED 
(
	[Purchase_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_Order_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_Order_Details](
	[Purchase_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_Order_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Order_Date] [datetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[ItemGroup_IdNo] [smallint] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Noof_Items] [numeric](18, 3) NULL,
	[Bags] [int] NULL,
	[Weight_Bag] [numeric](18, 3) NULL,
	[Weight] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Tax_Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Discount_Perc] [numeric](18, 2) NULL,
	[Discount_Amount] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
	[Bag_Nos] [varchar](500) NULL,
	[Serial_No] [varchar](500) NULL,
	[Size_IdNo] [int] NULL,
	[Meters] [numeric](18, 2) NULL,
	[Colour_IdNo] [int] NULL,
	[Noof_Items_Return] [numeric](18, 2) NULL,
	[Purchase_Order_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Purchase_Items] [numeric](18, 3) NULL,
 CONSTRAINT [PK_Purchase_Order_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Order_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_Order_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_Order_Head](
	[Purchase_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_Order_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Order_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL,
	[Ledger_IdNo] [int] NULL,
	[Cash_PartyName] [varchar](50) NULL,
	[Party_PhoneNo] [varchar](50) NULL,
	[PurchaseAc_IdNo] [int] NULL,
	[Tax_Type] [varchar](20) NULL,
	[TaxAc_IdNo] [int] NULL,
	[Delivery_Address1] [varchar](50) NULL,
	[Delivery_Address2] [varchar](50) NULL,
	[Delivery_Address3] [varchar](50) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Narration] [varchar](500) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Total_Bags] [int] NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Total_DiscountAmount] [numeric](18, 2) NULL,
	[Total_TaxAmount] [numeric](18, 2) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[CashDiscount_Perc] [numeric](18, 2) NULL,
	[CashDiscount_Amount] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Freight_Amount] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Document_Through] [varchar](35) NULL,
	[Despatch_To] [varchar](35) NULL,
	[Lr_No] [varchar](500) NULL,
	[Lr_Date] [varchar](500) NULL,
	[Booked_By] [varchar](35) NULL,
	[Transport_IdNo] [int] NULL,
	[Freight_ToPay_Amount] [numeric](18, 2) NULL,
	[Dc_No] [varchar](35) NULL,
	[Dc_Date] [varchar](35) NULL,
	[Order_No] [varchar](50) NULL,
	[Order_Date] [varchar](50) NULL,
	[Against_CForm_Status] [tinyint] NULL,
	[Entry_Type] [varchar](20) NULL,
	[Payment_Terms] [varchar](100) NULL,
	[OnAc_IdNo] [int] NULL,
	[Extra_Charges] [numeric](18, 2) NULL,
	[Total_Extra_Copies] [numeric](18, 2) NULL,
	[Sub_Total_Copies] [numeric](18, 2) NULL,
	[Party_Name] [varchar](50) NULL,
	[Weight] [numeric](18, 3) NULL,
	[Purchase_OrderAc_IdNo] [int] NULL,
 CONSTRAINT [PK_Purchase_Order_Head] PRIMARY KEY CLUSTERED 
(
	[Purchase_Order_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PURCHASE_REG_HSN]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PURCHASE_REG_HSN](
	[Purchase_Code] [varchar](50) NULL,
	[HSN_Code] [varchar](10) NULL,
	[UQC] [varchar](50) NULL,
	[QUANTITY] [real] NULL,
	[TAXABLE_AMOUNT] [real] NULL,
	[IGST_RATE] [real] NULL,
	[CGST_RATE] [real] NULL,
	[SGST_RATE] [real] NULL,
	[IGST_Amount] [real] NULL,
	[CGST_Amount] [real] NULL,
	[SGST_Amount] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_Return_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_Return_Details](
	[Purchase_Return_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_Return_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Return_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Noof_Items] [numeric](18, 3) NULL,
	[Bales] [int] NULL,
	[Weight] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Tax_Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Discount_Perc] [numeric](18, 2) NULL,
	[Discount_Amount] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
	[Bale_Nos] [varchar](500) NULL,
	[TaxAmount_Difference] [numeric](18, 2) NULL,
	[Size_IdNo] [int] NULL,
	[Footer_Cash_Discount_Perc_For_All_Item] [numeric](18, 2) NULL,
	[Footer_Cash_Discount_Amount_For_All_Item] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[HSN_Code] [varchar](50) NULL,
	[Gst_Perc] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Purhase_Return_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Return_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_Return_GST_Tax_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_Return_GST_Tax_Details](
	[Purchase_Return_Code] [varchar](50) NOT NULL,
	[Company_Idno] [int] NOT NULL,
	[Purchase_Return_No] [varchar](50) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Return_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[HSN_Code] [varchar](100) NULL,
	[Taxable_Amount] [numeric](18, 2) NULL,
	[CGST_Percentage] [numeric](18, 2) NULL,
	[CGST_Amount] [numeric](18, 2) NULL,
	[SGST_Percentage] [numeric](18, 2) NULL,
	[SGST_Amount] [numeric](18, 2) NULL,
	[IGST_Percentage] [numeric](18, 2) NULL,
	[IGST_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Purchase_Return_GST_Tax_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Return_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_Return_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_Return_Head](
	[Purchase_Return_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_Return_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Return_Date] [smalldatetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL,
	[Ledger_IdNo] [int] NULL,
	[Bill_No] [varchar](20) NULL,
	[PurchaseAc_IdNo] [int] NULL,
	[Tax_Type] [varchar](20) NULL,
	[TaxAc_IdNo] [int] NULL,
	[Narration] [varchar](1000) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Total_Bags] [int] NULL,
	[Total_Weight] [numeric](18, 3) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Total_DiscountAmount] [numeric](18, 2) NULL,
	[Total_TaxAmount] [numeric](18, 2) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[CashDiscount_Perc] [numeric](18, 2) NULL,
	[CashDiscount_Amount] [numeric](18, 2) NULL,
	[AddLess_BeforeTax_Amount] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Freight_Amount] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Bale_Nos] [varchar](500) NULL,
	[CGst_Amount] [numeric](18, 2) NULL,
	[SGst_Amount] [numeric](18, 2) NULL,
	[IGst_Amount] [numeric](18, 2) NULL,
	[Entry_VAT_GST_Type] [varchar](100) NULL,
	[Sales_Order_Selection_Code] [varchar](50) NULL,
 CONSTRAINT [PK_Purhase_Return_Head] PRIMARY KEY CLUSTERED 
(
	[Purchase_Return_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Purchase_Tax_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Purchase_Tax_Details](
	[Purchase_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[Discount_Amount] [numeric](18, 2) NULL,
	[Aessable_Amount] [numeric](18, 2) NULL,
	[Tax_Pec] [numeric](18, 2) NULL,
	[Item_IdNo] [int] NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Purchase_Tax_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PWD]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PWD](
	[PWD] [varchar](1000) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rate_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rate_Head](
	[Rate_Per_Hour] [numeric](18, 2) NULL,
	[Rate_Per_Day] [numeric](18, 2) NULL,
	[Rate_Per_Month] [numeric](18, 2) NULL,
	[Company_Idno] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[report_settings]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[report_settings](
	[report_code] [varchar](100) NOT NULL,
	[font_size] [tinyint] NULL,
	[paper_size] [tinyint] NULL,
	[paper_orientation] [tinyint] NULL,
	[print_mode] [tinyint] NULL,
	[horizontal_line] [tinyint] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[report_settings_column_size]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[report_settings_column_size](
	[report_code] [varchar](100) NOT NULL,
	[field_name] [varchar](100) NOT NULL,
	[noof_characters] [smallint] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReportTemp]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReportTemp](
	[Name1] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name1]  DEFAULT (''),
	[Name2] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name2]  DEFAULT (''),
	[Name3] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name3]  DEFAULT (''),
	[Name4] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name4]  DEFAULT (''),
	[Name5] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name5]  DEFAULT (''),
	[Name6] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name6]  DEFAULT (''),
	[name7] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_name7]  DEFAULT (''),
	[Name8] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name8]  DEFAULT (''),
	[Name9] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name9]  DEFAULT (''),
	[Name10] [varchar](1000) NULL CONSTRAINT [DF_ReportTemp_Name10]  DEFAULT (''),
	[Date1] [smalldatetime] NULL,
	[Date2] [smalldatetime] NULL,
	[Date3] [smalldatetime] NULL,
	[Date4] [smalldatetime] NULL,
	[Date5] [smalldatetime] NULL,
	[Int1] [int] NULL CONSTRAINT [DF_ReportTemp_Int1]  DEFAULT ((0)),
	[Int2] [int] NULL CONSTRAINT [DF_ReportTemp_Int2]  DEFAULT ((0)),
	[Int3] [int] NULL CONSTRAINT [DF_ReportTemp_Int3]  DEFAULT ((0)),
	[Int4] [int] NULL CONSTRAINT [DF_ReportTemp_Int4]  DEFAULT ((0)),
	[Int5] [int] NULL CONSTRAINT [DF_ReportTemp_Int5]  DEFAULT ((0)),
	[Int6] [int] NULL CONSTRAINT [DF_ReportTemp_Int6]  DEFAULT ((0)),
	[Int7] [int] NULL CONSTRAINT [DF_ReportTemp_Int7]  DEFAULT ((0)),
	[Int8] [int] NULL CONSTRAINT [DF_ReportTemp_Int8]  DEFAULT ((0)),
	[Int9] [int] NULL CONSTRAINT [DF_ReportTemp_Int9]  DEFAULT ((0)),
	[Int10] [int] NULL CONSTRAINT [DF_ReportTemp_Int10]  DEFAULT ((0)),
	[Meters1] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters1]  DEFAULT ((0)),
	[Meters2] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters2]  DEFAULT ((0)),
	[Meters3] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters3]  DEFAULT ((0)),
	[Meters4] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters4]  DEFAULT ((0)),
	[Meters5] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters5]  DEFAULT ((0)),
	[Meters6] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters6]  DEFAULT ((0)),
	[Meters7] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters7]  DEFAULT ((0)),
	[Meters8] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters8]  DEFAULT ((0)),
	[Meters9] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters9]  DEFAULT ((0)),
	[Meters10] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters102]  DEFAULT ((0)),
	[Meters11] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters101]  DEFAULT ((0)),
	[Meters12] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Meters10]  DEFAULT ((0)),
	[Weight1] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight1]  DEFAULT ((0)),
	[Weight2] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight2]  DEFAULT ((0)),
	[Weight3] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight3]  DEFAULT ((0)),
	[Weight4] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight4]  DEFAULT ((0)),
	[Weight5] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight5]  DEFAULT ((0)),
	[Weight6] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight6]  DEFAULT ((0)),
	[Weight7] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight7]  DEFAULT ((0)),
	[Weight8] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight8]  DEFAULT ((0)),
	[Weight9] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight9]  DEFAULT ((0)),
	[Weight10] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTemp_Weight10]  DEFAULT ((0)),
	[Currency1] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Currency1]  DEFAULT ((0)),
	[Currency2] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Currency2]  DEFAULT ((0)),
	[Currency3] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Currency3]  DEFAULT ((0)),
	[Currency4] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Currency4]  DEFAULT ((0)),
	[Currency5] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Currency5]  DEFAULT ((0)),
	[Currency6] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Currency6]  DEFAULT ((0)),
	[Currency7] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Currency7]  DEFAULT ((0)),
	[Currency8] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTemp_Currency8]  DEFAULT ((0)),
	[Currency9] [numeric](18, 7) NULL CONSTRAINT [DF_ReportTemp_Currency9]  DEFAULT ((0)),
	[Currency10] [numeric](18, 7) NULL CONSTRAINT [DF_ReportTemp_Currency10]  DEFAULT ((0)),
	[Currency11] [numeric](18, 7) NULL CONSTRAINT [DF__ReportTem__Curre__51C50B32]  DEFAULT ((0)),
	[Currency12] [numeric](18, 7) NULL CONSTRAINT [DF__ReportTem__Curre__52B92F6B]  DEFAULT ((0)),
	[Report_Heading1] [varchar](250) NULL,
	[Report_Heading2] [varchar](250) NULL,
	[Report_Heading3] [varchar](250) NULL,
	[Company_Name] [varchar](100) NULL,
	[Company_Address1] [varchar](300) NULL,
	[Company_Address2] [varchar](300) NULL,
	[Name16] [varchar](1000) NULL,
	[Name17] [varchar](1000) NULL,
	[Name18] [varchar](1000) NULL,
	[Name19] [varchar](1000) NULL,
	[Name20] [varchar](1000) NULL,
	[Name21] [varchar](1000) NULL,
	[Name22] [varchar](1000) NULL,
	[Name23] [varchar](1000) NULL,
	[Name24] [varchar](1000) NULL,
	[Name25] [varchar](1000) NULL,
	[Meters13] [numeric](18, 2) NULL,
	[Meters14] [numeric](18, 2) NULL,
	[Meters15] [numeric](18, 2) NULL,
	[Meters16] [numeric](18, 2) NULL,
	[Meters17] [numeric](18, 2) NULL,
	[Meters18] [numeric](18, 2) NULL,
	[Meter19] [numeric](18, 2) NULL,
	[Meters20] [numeric](18, 2) NULL,
	[AutoSlNo] [int] IDENTITY(1,1) NOT NULL,
	[Name11] [varchar](1000) NULL DEFAULT (''),
	[Name12] [varchar](1000) NULL DEFAULT (''),
	[Name13] [varchar](1000) NULL DEFAULT (''),
	[Name14] [varchar](1000) NULL DEFAULT (''),
	[Name15] [varchar](1000) NULL DEFAULT ('')
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReportTemp_Simple]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReportTemp_Simple](
	[Name1] [varchar](100) NULL,
	[Name2] [varchar](100) NULL,
	[Name3] [varchar](100) NULL,
	[Name4] [varchar](100) NULL,
	[Name5] [varchar](100) NULL,
	[Name6] [varchar](100) NULL,
	[name7] [varchar](100) NULL,
	[Name8] [varchar](100) NULL,
	[Name9] [varchar](100) NULL,
	[Name10] [varchar](100) NULL,
	[Name11] [varchar](100) NULL,
	[Name12] [varchar](100) NULL,
	[Date1] [smalldatetime] NULL,
	[Date2] [smalldatetime] NULL,
	[Date3] [smalldatetime] NULL,
	[Date4] [smalldatetime] NULL,
	[Int1] [int] NULL,
	[Int2] [int] NULL,
	[Int3] [int] NULL,
	[Int4] [int] NULL,
	[Int5] [int] NULL,
	[Int6] [int] NULL,
	[Int7] [int] NULL,
	[Int8] [int] NULL,
	[Int9] [int] NULL,
	[Int10] [int] NULL,
	[Meters1] [numeric](18, 2) NULL,
	[Meters2] [numeric](18, 2) NULL,
	[Meters3] [numeric](18, 2) NULL,
	[Meters4] [numeric](18, 2) NULL,
	[Meters5] [numeric](18, 2) NULL,
	[Meters6] [numeric](18, 2) NULL,
	[Meters7] [numeric](18, 2) NULL,
	[Meters8] [numeric](18, 2) NULL,
	[Meters9] [numeric](18, 2) NULL,
	[Meters10] [numeric](18, 2) NULL,
	[Meters11] [numeric](18, 2) NULL,
	[Weight1] [numeric](18, 3) NULL,
	[Weight2] [numeric](18, 3) NULL,
	[Weight3] [numeric](18, 3) NULL,
	[Weight4] [numeric](18, 3) NULL,
	[Weight5] [numeric](18, 3) NULL,
	[Weight6] [numeric](18, 3) NULL,
	[Weight7] [numeric](18, 3) NULL,
	[Weight8] [numeric](18, 3) NULL,
	[Weight9] [numeric](18, 3) NULL,
	[Weight10] [numeric](18, 3) NULL,
	[Weight11] [numeric](18, 3) NULL,
	[Currency1] [numeric](18, 2) NULL,
	[Currency2] [numeric](18, 2) NULL,
	[Currency3] [numeric](18, 2) NULL,
	[Currency4] [numeric](18, 2) NULL,
	[Currency5] [numeric](18, 2) NULL,
	[Currency6] [numeric](18, 2) NULL,
	[Currency7] [numeric](18, 2) NULL,
	[Currency8] [numeric](18, 2) NULL,
	[Currency9] [numeric](18, 7) NULL,
	[Currency10] [numeric](18, 7) NULL,
	[Currency11] [numeric](18, 7) NULL,
	[Currency12] [numeric](18, 7) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReportTempSub]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReportTempSub](
	[Name1] [varchar](1000) NULL,
	[Name2] [varchar](1000) NULL,
	[Name3] [varchar](1000) NULL,
	[Name4] [varchar](1000) NULL,
	[Name5] [varchar](1000) NULL,
	[Name6] [varchar](1000) NULL,
	[name7] [varchar](1000) NULL,
	[Name8] [varchar](1000) NULL,
	[Name9] [varchar](1000) NULL,
	[Name10] [varchar](1000) NULL,
	[Date1] [smalldatetime] NULL,
	[Date2] [smalldatetime] NULL,
	[Date3] [smalldatetime] NULL,
	[Date4] [smalldatetime] NULL,
	[Date5] [smalldatetime] NULL,
	[Int1] [int] NULL,
	[Int2] [int] NULL,
	[Int3] [int] NULL,
	[Int4] [int] NULL,
	[Int5] [int] NULL,
	[Int6] [int] NULL,
	[Int7] [int] NULL,
	[Int8] [int] NULL,
	[Int9] [int] NULL,
	[Int10] [int] NULL,
	[Meters1] [numeric](18, 2) NULL,
	[Meters2] [numeric](18, 2) NULL,
	[Meters3] [numeric](18, 2) NULL,
	[Meters4] [numeric](18, 2) NULL,
	[Meters5] [numeric](18, 2) NULL,
	[Meters6] [numeric](18, 2) NULL,
	[Meters7] [numeric](18, 2) NULL,
	[Meters8] [numeric](18, 2) NULL,
	[Meters9] [numeric](18, 2) NULL,
	[Meters10] [numeric](18, 2) NULL,
	[Meters11] [numeric](18, 2) NULL,
	[Meters12] [numeric](18, 2) NULL,
	[Weight1] [numeric](18, 3) NULL,
	[Weight2] [numeric](18, 3) NULL,
	[Weight3] [numeric](18, 3) NULL,
	[Weight4] [numeric](18, 3) NULL,
	[Weight5] [numeric](18, 3) NULL,
	[Weight6] [numeric](18, 3) NULL,
	[Weight7] [numeric](18, 3) NULL,
	[Weight8] [numeric](18, 3) NULL,
	[Weight9] [numeric](18, 3) NULL,
	[Weight10] [numeric](18, 3) NULL,
	[Currency1] [numeric](18, 2) NULL,
	[Currency2] [numeric](18, 2) NULL,
	[Currency3] [numeric](18, 2) NULL,
	[Currency4] [numeric](18, 2) NULL,
	[Currency5] [numeric](18, 2) NULL,
	[Currency6] [numeric](18, 2) NULL,
	[Currency7] [numeric](18, 2) NULL,
	[Currency8] [numeric](18, 2) NULL,
	[Currency9] [numeric](18, 7) NULL,
	[Currency10] [numeric](18, 7) NULL,
	[Currency11] [numeric](18, 7) NULL,
	[Currency12] [numeric](18, 7) NULL,
	[Report_Heading1] [varchar](250) NULL,
	[Report_Heading2] [varchar](250) NULL,
	[Report_Heading3] [varchar](250) NULL,
	[Company_Name] [varchar](100) NULL,
	[Company_Address1] [varchar](300) NULL,
	[Company_Address2] [varchar](300) NULL,
	[AutoSlNo] [int] IDENTITY(1,1) NOT NULL,
	[Name11] [varchar](1000) NULL,
	[Name12] [varchar](1000) NULL,
	[Name13] [varchar](1000) NULL,
	[Name14] [varchar](1000) NULL,
	[Name15] [varchar](1000) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Delivery_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Delivery_Details](
	[Sales_Delivery_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Delivery_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Delivery_Details_for_OrderBy]  DEFAULT ((0)),
	[Sales_Delivery_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Sales_Delivery_Details_Ledger_IdNo]  DEFAULT ((0)),
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL CONSTRAINT [DF_Sales_Delivery_Details_Item_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Delivery_Details_Unit_IdNo]  DEFAULT ((0)),
	[Quantity] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Delivery_Details_Quantity]  DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Delivery_Details_Rate]  DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Delivery_Details_Amount]  DEFAULT ((0)),
	[Item_Description] [varchar](500) NULL CONSTRAINT [DF_Sales_Delivery_Details_Item_Description]  DEFAULT (''),
	[Sales_delivery_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,
	[Receipt_Quantity] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Delivery_Details_Receipt_Quantity]  DEFAULT ((0)),
	[GRN_No] [varchar](50) NULL,
	[ISBILLED] [bit] NULL,
	[Order_Code] [varchar](50) NULL DEFAULT (''),
	[Colour_IdNo] [int] NULL DEFAULT ((0)),
	[Ordercode_forSelection] [varchar](100) NULL DEFAULT (''),
	[Challan_No] [varchar](100) NULL DEFAULT (''),
	[Challan_Date] [varchar](100) NULL DEFAULT (''),
	[Order_No] [varchar](100) NULL DEFAULT (''),
	[Order_Date] [varchar](100) NULL DEFAULT (''),
	[Rate_For] [varchar](50) NULL DEFAULT (''),
	[ItemGroup_IdNo] [int] NULL DEFAULT ((0)),
	[Weight] [numeric](18, 2) NULL DEFAULT ((0)),
	[Receipt_No] [varchar](50) NULL DEFAULT (''),
	[Sales_Receipt_Code] [varchar](50) NULL DEFAULT (''),
	[Sales_Receipt_Detail_Slno] [int] NULL DEFAULT ((0)),
	[No_Of_Rolls] [numeric](18, 2) NULL DEFAULT ((0)),
	[Entry_Type] [varchar](30) NULL DEFAULT (''),
	[Sales_Order_Code] [varchar](30) NULL DEFAULT (''),
	[Sales_Order_Detail_SlNo] [int] NULL DEFAULT ((0)),
	[Noof_Items] [numeric](18, 2) NULL DEFAULT ((0)),
	[HSN_Code] [varchar](50) NULL DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL DEFAULT ((0)),
	[Size_Idno] [int] NULL DEFAULT ((0)),
	[Style_Idno] [int] NULL DEFAULT ((0)),
	[Job_NO] [varchar](100) NULL,
	[Bundles] [tinyint] NULL,
	[Delivery_Purpose] [varchar](50) NULL,
	[Component_IdNo] [int] NULL,
	[Party_DC_No] [varchar](50) NULL,
	[Total_Bundles] [tinyint] NULL,
 CONSTRAINT [PK_Sales_Delivery_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Delivery_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Delivery_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Delivery_Head](
	[Sales_Delivery_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Delivery_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Delivery_Head_for_OrderBy]  DEFAULT ((0)),
	[Sales_Delivery_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Sales_Delivery_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Delivery_Head_Delivery_Terms]  DEFAULT (''),
	[Order_Date] [varchar](50) NULL CONSTRAINT [DF_Sales_Delivery_Head_Order_Date]  DEFAULT (''),
	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Delivery_Head_Total_Qty]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Delivery_Head_Gross_Amount]  DEFAULT ((0)),
	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Delivery_Head_Payment_Terms]  DEFAULT (''),
	[Transport_IdNo] [int] NULL CONSTRAINT [DF_Sales_Delivery_Head_Transport_IdNo]  DEFAULT ((0)),
	[Remarks] [varchar](500) NULL CONSTRAINT [DF_Sales_Delivery_Head_Remarks]  DEFAULT (''),
	[Non_Billable_Reason] [varchar](250) NULL DEFAULT (''),
	[IsBillable] [bit] NULL DEFAULT ((0)),
	[Manual_DC_No] [varchar](20) NULL,
	[Total_Weight] [numeric](18, 2) NULL DEFAULT ((0)),
	[Entry_Type] [varchar](50) NULL DEFAULT (''),
	[Entry_VAT_GST_Type] [varchar](50) NULL DEFAULT (''),
	[Assessable_Value] [numeric](18, 2) NULL DEFAULT ((0)),
	[CGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[SGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[IGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[SubTotal_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL DEFAULT ((0)),
	[Entry_GST_Tax_Type] [varchar](20) NULL DEFAULT (''),
	[Total_Bags] [int] NULL DEFAULT ((0)),
	[Electronic_Reference_No] [varchar](100) NULL DEFAULT (''),
	[Transportation_Mode] [varchar](100) NULL DEFAULT (''),
	[Date_Time_Of_Supply] [varchar](100) NULL DEFAULT (''),
	[Weight] [numeric](18, 2) NULL DEFAULT ((0)),
	[Freight_ToPay_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[charge] [numeric](18, 2) NULL DEFAULT ((0)),
	[Lr_Date] [varchar](50) NULL DEFAULT (''),
	[Lr_No] [varchar](50) NULL DEFAULT (''),
	[Booked_By] [varchar](50) NULL DEFAULT (''),
	[Total_Bundles] [tinyint] NULL,
	[User_Name] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_Sales_Delivery_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Delivery_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Details](
	[Sales_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_CashSales_Details_for_OrderBy]  DEFAULT ((0)),
	[Sales_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_CashSales_Details_Ledger_IdNo]  DEFAULT ((0)),
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL CONSTRAINT [DF_CashSales_Details_Item_IdNo]  DEFAULT ((0)),
	[ItemGroup_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Details_ItemGroup_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_CashSales_Details_Unit_IdNo]  DEFAULT ((0)),
	[Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_CashSales_Details_Noof_Items]  DEFAULT ((0)),
	[Bags] [int] NULL CONSTRAINT [DF_Sales_Details_Unit_IdNo1]  DEFAULT ((0)),
	[Weight_Bag] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Details_Weight1]  DEFAULT ((0)),
	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Details_Noof_Items1]  DEFAULT ((0)),
	[Rate_5Kg] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Details_Rate1]  DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_CashSales_Details_Rate]  DEFAULT ((0)),
	[Tax_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Details_Tax_Rate]  DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSales_Details_Total_Amount1]  DEFAULT ((0)),
	[Discount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Details_Discount_Perc]  DEFAULT ((0)),
	[Discount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Details_Discount_Amount]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Details_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Details_Tax_Amount]  DEFAULT ((0)),
	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Details_Total_Amount]  DEFAULT ((0)),
	[Bag_Nos] [varchar](500) NULL CONSTRAINT [DF_Sales_Details_Bag_Nos]  DEFAULT (''),
	[Serial_No] [varchar](500) NULL CONSTRAINT [DF_Sales_Details_Serial_No]  DEFAULT (''),
	[Size_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Size___52593CB8]  DEFAULT ((0)),
	[JobWork_No] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__JobWo__33F4B129]  DEFAULT (''),
	[JobWork_Code] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__JobWo__34E8D562]  DEFAULT (''),
	[JobWork_Date] [varchar](20) NULL CONSTRAINT [DF__Sales_Det__JobWo__35DCF99B]  DEFAULT (''),
	[Rate_Sqft] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Rate___36D11DD4]  DEFAULT ((0)),
	[GSM] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Detai__GSM__37C5420D]  DEFAULT ((0)),
	[Rolls] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Rolls__38B96646]  DEFAULT ((0)),
	[Weight_Roll] [numeric](18, 3) NULL CONSTRAINT [DF__Sales_Det__Weigh__39AD8A7F]  DEFAULT ((0)),
	[Meters] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Meter__3AA1AEB8]  DEFAULT ((0)),
	[Colour_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Colou__3B95D2F1]  DEFAULT ((0)),
	[Item_code] [varchar](100) NULL CONSTRAINT [DF__Sales_Det__Item___3C89F72A]  DEFAULT (''),
	[Entry_Type] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__Entry__3D7E1B63]  DEFAULT (''),
	[Sales_Order_Code] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__Sales__3E723F9C]  DEFAULT (''),
	[Sales_Order_Detail_SlNo] [int] NULL CONSTRAINT [DF__Sales_Det__Sales__3F6663D5]  DEFAULT ((0)),
	[Noof_Items_Return] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Noof___405A880E]  DEFAULT ((0)),
	[Sales_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Design_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Desig__414EAC47]  DEFAULT ((0)),
	[Gender_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Gende__4242D080]  DEFAULT ((0)),
	[Sleeve_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Sleev__4336F4B9]  DEFAULT ((0)),
	[Close_Order] [bit] NULL,
	[Ordercode_forSelection] [varchar](100) NULL CONSTRAINT [DF__Sales_Det__Order__04659998]  DEFAULT (''),
	[Scheme_Disc_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Schem__0EE3280B]  DEFAULT ((0)),
	[Scheme_Discount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Schem__0FD74C44]  DEFAULT ((0)),
	[Trade_Disc_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Trade__10CB707D]  DEFAULT ((0)),
	[Trade_Discount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Trade__11BF94B6]  DEFAULT ((0)),
	[Cgst_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Cgst___12B3B8EF]  DEFAULT ((0)),
	[Cgst_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Cgst___13A7DD28]  DEFAULT ((0)),
	[Sgst_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Sgst___149C0161]  DEFAULT ((0)),
	[Sgst_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Sgst___1590259A]  DEFAULT ((0)),
	[Igst_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Igst___168449D3]  DEFAULT ((0)),
	[Igst_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Igst___17786E0C]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Net_A__186C9245]  DEFAULT ((0)),
	[Total_Rate] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Total__1960B67E]  DEFAULT ((0)),
	[Scheme_UCP] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Schem__1A54DAB7]  DEFAULT ((0)),
	[Discount_Total] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Disco__1B48FEF0]  DEFAULT ((0)),
	[Design_Picture] [image] NULL,
	[Rate_For] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__Rate___2F4FF79D]  DEFAULT (''),
	[Challan_No] [varchar](100) NULL CONSTRAINT [DF__Sales_Det__Chall__30441BD6]  DEFAULT (''),
	[Challan_Date] [varchar](100) NULL CONSTRAINT [DF__Sales_Det__Chall__3138400F]  DEFAULT (''),
	[Order_No] [varchar](100) NULL CONSTRAINT [DF__Sales_Det__Order__322C6448]  DEFAULT (''),
	[Order_Date] [varchar](100) NULL CONSTRAINT [DF__Sales_Det__Order__33208881]  DEFAULT (''),
	[Quantity] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Quant__4AF81212]  DEFAULT ((0)),
	[Rate_1000Stitches] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Rate___4BEC364B]  DEFAULT ((0)),
	[Design_No] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__Desig__4CE05A84]  DEFAULT (''),
	[Details_Design] [varchar](500) NULL CONSTRAINT [DF__Sales_Det__Detai__4DD47EBD]  DEFAULT (''),
	[Return_Qty] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Retur__1372D2FE]  DEFAULT ((0)),
	[Style_Idno] [int] NULL CONSTRAINT [DF__Sales_Det__Style__2C3E80C8]  DEFAULT ((0)),
	[Style_Name] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__Style__35C7EB02]  DEFAULT (''),
	[Trade_Discount_Amount_For_All_Item] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Trade__36BC0F3B]  DEFAULT ((0)),
	[Trade_Discount_Perc_For_All_Item] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Trade__37B03374]  DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Asses__45FE52CB]  DEFAULT ((0)),
	[HSN_Code] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__HSN_C__46F27704]  DEFAULT (''),
	[GST_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__GST_P__47E69B3D]  DEFAULT ((0)),
	[Actual_Amount] [numeric](18, 2) NULL,
	[Actual_Rate] [numeric](18, 2) NULL,
	[Advance_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Advan__7795AE5F]  DEFAULT ((0)),
	[Balance_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Balan__797DF6D1]  DEFAULT ((0)),
	[Dc_No] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__Dc_No__1249A49B]  DEFAULT (''),
	[Sales_Price] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Sales__2097C3F2]  DEFAULT ((0)),
	[Discount_Amount_item] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Disco__218BE82B]  DEFAULT ((0)),
	[Rate_Tax] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Rate___22800C64]  DEFAULT ((0)),
	[Discount_Perc_Item] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Disco__2744C181]  DEFAULT ((0)),
	[Expiry_Month_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Expir__2838E5BA]  DEFAULT ((0)),
	[Manufacture_Month_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Manuf__292D09F3]  DEFAULT ((0)),
	[Manufacture_Day] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Manuf__2A212E2C]  DEFAULT ((0)),
	[Manufacture_Year] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Manuf__2B155265]  DEFAULT ((0)),
	[Expiry_Period_Days] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Expir__2C09769E]  DEFAULT ((0)),
	[Expiry_Day] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Expir__2CFD9AD7]  DEFAULT ((0)),
	[Manufacture_Date] [smalldatetime] NULL,
	[Expiry_Date] [smalldatetime] NULL,
	[Expiry_Year] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Expir__2DF1BF10]  DEFAULT ((0)),
	[Batch_Serial_No] [varchar](500) NULL CONSTRAINT [DF__Sales_Det__Batch__2EE5E349]  DEFAULT (''),
	[Cash_Discount_Perc_For_All_Item] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Cash___32B6742D]  DEFAULT ((0)),
	[Cash_Discount_Amount_For_All_Item] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__Cash___33AA9866]  DEFAULT ((0)),
	[MRP_Rate] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__MRP_R__5E94F66B]  DEFAULT ((0)),
	[MRP_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__MRP_A__5F891AA4]  DEFAULT ((0)),
	[RateWithTax] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Det__RateW__2FA4FD58]  DEFAULT ((0)),
	[Item_Description] [varchar](500) NULL CONSTRAINT [DF__Sales_Det__Item___30992191]  DEFAULT (''),
	[Sales_Delivery_Code] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__Sales__318D45CA]  DEFAULT (''),
	[Sales_Delivery_Detail_SlNo] [int] NULL CONSTRAINT [DF__Sales_Det__Sales__32816A03]  DEFAULT ((0)),
	[Area_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Area___6BB9E75F]  DEFAULT ((0)),
	[Agent_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Agent__6CAE0B98]  DEFAULT ((0)),
	[Extra_Quantity] [int] NULL CONSTRAINT [DF__Sales_Det__Extra__6F8A7843]  DEFAULT ((0)),
	[Free_Item_IdNo] [int] NULL CONSTRAINT [DF__Sales_Det__Free___7266E4EE]  DEFAULT ((0)),
	[Free_Qty] [int] NULL CONSTRAINT [DF__Sales_Det__Free___735B0927]  DEFAULT ((0)),
	[Sales_Discount_Code] [varchar](50) NULL CONSTRAINT [DF__Sales_Det__Sales__1798699D]  DEFAULT (''),
	[Job_No] [varchar](100) NULL,
	[DCCODES] [varchar](5000) NULL,
 CONSTRAINT [PK_Sales_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Discount_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Discount_Details](
	[Sales_Discount_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Discount_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Discount_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[ItemGroup_IdNo] [smallint] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Noof_Items] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
	[Serial_No] [varchar](500) NULL,
	[Sales_Detail_SlNo] [int] NULL,
	[Sales_Code] [varchar](50) NULL,
	[Discount_Perc] [numeric](18, 2) NULL,
	[Discount_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Sales_Discount_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Discount_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Discount_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Discount_Head](
	[Sales_Discount_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Discount_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Discount_Date] [datetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[SalesAc_IdNo] [int] NULL,
	[Tax_Type] [varchar](20) NULL,
	[TaxAc_IdNo] [int] NULL,
	[Narration] [varchar](500) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Total_DiscountAmount] [numeric](18, 2) NULL,
	[Total_TaxAmount] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Selection_Type] [varchar](50) NULL,
	[Agent_idno] [int] NULL,
 CONSTRAINT [PK_Sales_Discount_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Discount_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Enquiry_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Enquiry_Details](
	[Sales_Enquiry_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Enquiry_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Enquiry_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[ItemGroup_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Item_Description] [varchar](500) NULL,
	[Sales_Enquiry_Detail_SlNo] [int] NULL,
	[Order_Quantity] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Sales_Enquiry_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Enquiry_No] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Enquiry_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Enquiry_Head](
	[Sales_Enquiry_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Enquiry_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Enquiry_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Delivery_Terms] [varchar](50) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[CashDiscount_Perc] [numeric](18, 2) NULL,
	[CashDiscount_Amount] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Freight_Amount] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Labour_Charge] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Payment_Terms] [varchar](100) NULL,
	[Tax_Type] [varchar](30) NULL,
 CONSTRAINT [PK_Sales_Enquiry_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Enquiry_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_GST_Tax_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_GST_Tax_Details](
	[Sales_Code] [varchar](50) NOT NULL,
	[Company_Idno] [int] NOT NULL,
	[Sales_No] [varchar](50) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[HSN_Code] [varchar](100) NULL,
	[Taxable_Amount] [numeric](18, 2) NULL,
	[CGST_Percentage] [numeric](18, 2) NULL,
	[CGST_Amount] [numeric](18, 2) NULL,
	[SGST_Percentage] [numeric](18, 2) NULL,
	[SGST_Amount] [numeric](18, 2) NULL,
	[IGST_Percentage] [numeric](18, 2) NULL,
	[IGST_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Sales_GST_Tax_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Head](
	[Sales_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_CashSales_Head_for_OrderBy]  DEFAULT ((0)),
	[Sales_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL CONSTRAINT [DF_Sales_Head_Payment_Method]  DEFAULT (''),
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_CashSales_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Cash_PartyName] [varchar](50) NULL CONSTRAINT [DF_Sales_Head_Delivery_Address11]  DEFAULT (''),
	[Party_PhoneNo] [varchar](50) NULL CONSTRAINT [DF_Sales_Head_Party_PhoneNo]  DEFAULT (''),
	[SalesAc_IdNo] [int] NULL CONSTRAINT [DF_CashSales_Head_SalesAc_IdNo]  DEFAULT ((0)),
	[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_Sales_Head_Tax_Type]  DEFAULT (''),
	[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Head_SalesAc_IdNo1]  DEFAULT ((0)),
	[Delivery_Address1] [varchar](50) NULL CONSTRAINT [DF_Sales_Head_Delivery_Address1]  DEFAULT (''),
	[Delivery_Address2] [varchar](50) NULL CONSTRAINT [DF_Sales_Head_Delivery_Address2]  DEFAULT (''),
	[Delivery_Address3] [varchar](50) NULL CONSTRAINT [DF_Sales_Head_Delivery_Address3]  DEFAULT (''),
	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Head_Vehicle_No]  DEFAULT (''),
	[Removal_Date] [varchar](20) NULL CONSTRAINT [DF_Sales_Head_Removal_Date]  DEFAULT (''),
	[Removal_Time] [varchar](20) NULL CONSTRAINT [DF_Sales_Head_Removal_Time]  DEFAULT (''),
	[Bag_Nos] [varchar](500) NULL CONSTRAINT [DF_Sales_Head_Bag_Nos]  DEFAULT (''),
	[Narration] [varchar](500) NULL CONSTRAINT [DF_Sales_Head_Narration]  DEFAULT (''),
	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_CashSales_Head_Total_Qty]  DEFAULT ((0)),
	[Total_Bags] [int] NULL CONSTRAINT [DF_Sales_Head_Total_Weight1]  DEFAULT ((0)),
	[Total_Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Head_Total_Qty1]  DEFAULT ((0)),
	[SubTotal_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_Sub_Total]  DEFAULT ((0)),
	[Total_DiscountAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_Total_DiscountAmount]  DEFAULT ((0)),
	[Total_TaxAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_Total_TaxAmount]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSales_Head_CashDiscount_Perc1]  DEFAULT ((0)),
	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_CashDiscount_Perc]  DEFAULT ((0)),
	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_CashDiscount_Amount]  DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_AddLess_Amount1]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_CashDiscount_Perc1]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_CashDiscount_Amount1]  DEFAULT ((0)),
	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Head_AddLess_Amount1_1]  DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSales_Head_AddLess_Amount]  DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_CashSales_Head_Round_Off]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSales_Head_Net_Amount]  DEFAULT ((0)),
	[Document_Through] [varchar](35) NULL CONSTRAINT [DF__Sales_Hea__Docum__6497E884]  DEFAULT (''),
	[Despatch_To] [varchar](35) NULL CONSTRAINT [DF__Sales_Hea__Despa__658C0CBD]  DEFAULT (''),
	[Lr_No] [varchar](35) NULL CONSTRAINT [DF__Sales_Hea__Lr_No__668030F6]  DEFAULT (''),
	[Lr_Date] [varchar](20) NULL CONSTRAINT [DF__Sales_Hea__Lr_Da__6774552F]  DEFAULT (''),
	[Booked_By] [varchar](35) NULL CONSTRAINT [DF__Sales_Hea__Booke__68687968]  DEFAULT (''),
	[Transport_IdNo] [int] NULL CONSTRAINT [DF__Sales_Hea__Trans__695C9DA1]  DEFAULT ((0)),
	[Freight_ToPay_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Freig__6A50C1DA]  DEFAULT ((0)),
	[Dc_No] [varchar](100) NULL CONSTRAINT [DF_SalesHead_DcNo]  DEFAULT (''),
	[Dc_Date] [varchar](35) NULL CONSTRAINT [DF__Sales_Hea__Dc_Da__6C390A4C]  DEFAULT (''),
	[Ro_Division_Status] [tinyint] NULL CONSTRAINT [DF__Sales_Hea__Ro_Di__6D2D2E85]  DEFAULT ((0)),
	[Charging_Quantity] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Charg__6E2152BE]  DEFAULT ((0)),
	[Charging_Rate] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Charg__6F1576F7]  DEFAULT ((0)),
	[Order_No] [varchar](1000) NULL CONSTRAINT [DF_SalesHead_OrderNo]  DEFAULT (''),
	[Order_Date] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Order__70FDBF69]  DEFAULT (''),
	[Against_CForm_Status] [tinyint] NULL CONSTRAINT [DF__Sales_Hea__Again__71F1E3A2]  DEFAULT ((0)),
	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF__Sales_Hea__Weigh__72E607DB]  DEFAULT ((0)),
	[Entry_Type] [varchar](20) NULL CONSTRAINT [DF__Sales_Hea__Entry__73DA2C14]  DEFAULT (''),
	[Payment_Terms] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Payme__74CE504D]  DEFAULT (''),
	[Total_Rolls] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Total__75C27486]  DEFAULT ((0)),
	[Total_Meters] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Total__76B698BF]  DEFAULT ((0)),
	[Branch_Transfer_Status] [tinyint] NULL CONSTRAINT [DF__Sales_Hea__Branc__77AABCF8]  DEFAULT ((0)),
	[OnAc_IdNo] [int] NULL CONSTRAINT [DF__Sales_Hea__OnAc___789EE131]  DEFAULT ((0)),
	[Rate_Extra_Copy] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Rate___7993056A]  DEFAULT ((0)),
	[Rent_Machine] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Rent___7A8729A3]  DEFAULT ((0)),
	[Free_Copies_Machine] [int] NULL CONSTRAINT [DF__Sales_Hea__Free___7B7B4DDC]  DEFAULT ((0)),
	[Total_Copies] [int] NULL CONSTRAINT [DF__Sales_Hea__Total__7C6F7215]  DEFAULT ((0)),
	[Total_Free_Copies] [int] NULL CONSTRAINT [DF__Sales_Hea__Total__7D63964E]  DEFAULT ((0)),
	[Additional_Copies] [int] NULL CONSTRAINT [DF__Sales_Hea__Addit__7E57BA87]  DEFAULT ((0)),
	[Rent] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Head__Rent__7F4BDEC0]  DEFAULT ((0)),
	[Extra_Charges] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Extra__004002F9]  DEFAULT ((0)),
	[Total_Extra_Copies] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Total__01342732]  DEFAULT ((0)),
	[Sub_Total_Copies] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Sub_T__02284B6B]  DEFAULT ((0)),
	[Opening_Date] [smalldatetime] NULL CONSTRAINT [DF__Sales_Hea__Openi__031C6FA4]  DEFAULT ((0)),
	[Closing_Date] [smalldatetime] NULL CONSTRAINT [DF__Sales_Hea__Closi__041093DD]  DEFAULT ((0)),
	[Total_Machine] [int] NULL CONSTRAINT [DF__Sales_Hea__Total__0504B816]  DEFAULT ((0)),
	[Delivery_Code] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Deliv__05F8DC4F]  DEFAULT (''),
	[Selection_Type] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Selec__06ED0088]  DEFAULT (''),
	[Party_Name] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Party__07E124C1]  DEFAULT (''),
	[Labour_Charge] [int] NULL CONSTRAINT [DF__Sales_Hea__Labou__08D548FA]  DEFAULT ((0)),
	[NoOf_Bundle] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__NoOf___09C96D33]  DEFAULT (''),
	[DC_Cutoff_Date] [smalldatetime] NULL,
	[ISDIRECT] [bit] NULL,
	[Entry_Status] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Entry__23DE44F1]  DEFAULT (''),
	[Site_IdNo] [int] NULL CONSTRAINT [DF__Sales_Hea__Site___35FCF52C]  DEFAULT ((0)),
	[Inv_No_Prefix1] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Inv_N__38D961D7]  DEFAULT (''),
	[Inv_No_Prefix2] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Inv_N__39CD8610]  DEFAULT (''),
	[Party_Dc_No] [varchar](200) NULL CONSTRAINT [DF__Sales_Hea__Party__118A8A8C]  DEFAULT (''),
	[LessFor] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__LessF__1837881B]  DEFAULT ((0)),
	[Tds_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Tds_P__192BAC54]  DEFAULT ((0)),
	[Tds_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Tds_A__1A1FD08D]  DEFAULT ((0)),
	[Net_Amount_Tds] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Net_A__1B13F4C6]  DEFAULT ((0)),
	[Multi_Dc_No] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Multi__1C0818FF]  DEFAULT (''),
	[Pcs_or_Box] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Pcs_o__300F11AC]  DEFAULT (''),
	[charge] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__charg__310335E5]  DEFAULT ((0)),
	[DeliveryTo_idNo] [int] NULL CONSTRAINT [DF__Sales_Hea__Deliv__33DFA290]  DEFAULT ((0)),
	[Place_Of_Supply] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Place__34D3C6C9]  DEFAULT (''),
	[CGst_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__CGst___3A8CA01F]  DEFAULT ((0)),
	[SGst_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__SGst___3B80C458]  DEFAULT ((0)),
	[Entry_VAT_GST_Type] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Entry__3E5D3103]  DEFAULT (''),
	[Electronic_Reference_No] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Elect__3F51553C]  DEFAULT (''),
	[Transportation_Mode] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Trans__40457975]  DEFAULT (''),
	[Date_Time_Of_Supply] [varchar](100) NULL CONSTRAINT [DF__Sales_Hea__Date___41399DAE]  DEFAULT (''),
	[Entry_GST_Tax_Type] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Entry__422DC1E7]  DEFAULT (''),
	[CGst_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__CGst___4321E620]  DEFAULT ((0)),
	[SGst_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__SGst___44160A59]  DEFAULT ((0)),
	[IGst_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__IGst___450A2E92]  DEFAULT ((0)),
	[Actual_Net_Amount] [numeric](18, 2) NULL,
	[Actual_Gross_Amount] [numeric](18, 2) NULL,
	[Actual_Tax_Amount] [numeric](18, 2) NULL,
	[Freight_Charge] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Freig__5540965B]  DEFAULT ((0)),
	[Freight_Charge_Name] [varchar](50) NULL,
	[Receipt_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Recei__7889D298]  DEFAULT ((0)),
	[Delivery_Date] [varchar](50) NULL,
	[Received_Date] [varchar](50) NULL,
	[Sales_Order_Selection_Code] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Sales__07CC1628]  DEFAULT (''),
	[Delivery_Status] [int] NULL CONSTRAINT [DF__Sales_Hea__Deliv__0B9CA70C]  DEFAULT ((0)),
	[Advance_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Advan__0C90CB45]  DEFAULT ((0)),
	[Balance_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Balan__0D84EF7E]  DEFAULT ((0)),
	[Form_H_Status] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Form___0E7913B7]  DEFAULT ((0)),
	[ItemWise_DiscAmount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__ItemW__0F6D37F0]  DEFAULT ((0)),
	[Total_DiscountAmount_item] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Total__2374309D]  DEFAULT ((0)),
	[Aessable_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Aessa__246854D6]  DEFAULT ((0)),
	[AddLess_Name] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__AddLe__255C790F]  DEFAULT (''),
	[Freight_Name] [varchar](50) NULL CONSTRAINT [DF__Sales_Hea__Freig__26509D48]  DEFAULT (''),
	[Received_Amount] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Recei__2FDA0782]  DEFAULT ((0)),
	[Tax_Perc2] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Tax_P__30CE2BBB]  DEFAULT ((0)),
	[Tax_Amount2] [numeric](18, 2) NULL CONSTRAINT [DF__Sales_Hea__Tax_A__31C24FF4]  DEFAULT ((0)),
	[Total_FreeQty] [numeric](18, 3) NULL CONSTRAINT [DF__Sales_Hea__Total__66010E09]  DEFAULT ((0)),
	[Agent_idno] [int] NULL CONSTRAINT [DF__Sales_Hea__Agent__6DA22FD1]  DEFAULT ((0)),
	[Total_Extra_Quantity] [int] NULL CONSTRAINT [DF__Sales_Hea__Total__6E96540A]  DEFAULT ((0)),
	[Salesman_Idno] [smallint] NULL CONSTRAINT [DF__Sales_Hea__Sales__1980B20F]  DEFAULT ((0)),
	[Party_Ref_No] [varchar](150) NULL,
	[User_Name] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_Sales_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Order_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Order_Details](
	[Sales_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Order_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Order_Date] [datetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[ItemGroup_IdNo] [smallint] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Noof_Items] [numeric](18, 3) NULL,
	[Bags] [int] NULL,
	[Weight_Bag] [numeric](18, 3) NULL,
	[Weight] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Tax_Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Discount_Perc] [numeric](18, 2) NULL,
	[Discount_Amount] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Total_Amount] [numeric](18, 2) NULL,
	[Bag_Nos] [varchar](500) NULL,
	[Serial_No] [varchar](500) NULL,
	[Size_IdNo] [int] NULL,
	[Meters] [numeric](18, 2) NULL,
	[Colour_IdNo] [int] NULL,
	[Noof_Items_Return] [numeric](18, 2) NULL,
	[Sales_Order_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Sales_Items] [numeric](18, 3) NULL,
	[Sales_Quotation_Code] [varchar](50) NULL,
	[Sales_Quotation_Detail_SlNo] [int] NULL,
	[item_Description] [varchar](500) NULL,
	[Entry_Type] [varchar](20) NULL,
 CONSTRAINT [PK_Sales_Order_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Order_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Order_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Order_Head](
	[Sales_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Order_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Order_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL,
	[Ledger_IdNo] [int] NULL,
	[Cash_PartyName] [varchar](50) NULL,
	[Party_PhoneNo] [varchar](50) NULL,
	[SalesAc_IdNo] [int] NULL,
	[Tax_Type] [varchar](20) NULL,
	[TaxAc_IdNo] [int] NULL,
	[Delivery_Address1] [varchar](50) NULL,
	[Delivery_Address2] [varchar](50) NULL,
	[Delivery_Address3] [varchar](50) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Narration] [varchar](500) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Total_Bags] [int] NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Total_DiscountAmount] [numeric](18, 2) NULL,
	[Total_TaxAmount] [numeric](18, 2) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[CashDiscount_Perc] [numeric](18, 2) NULL,
	[CashDiscount_Amount] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Freight_Amount] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Document_Through] [varchar](35) NULL,
	[Despatch_To] [varchar](35) NULL,
	[Lr_No] [varchar](35) NULL,
	[Lr_Date] [varchar](20) NULL,
	[Booked_By] [varchar](35) NULL,
	[Transport_IdNo] [int] NULL,
	[Freight_ToPay_Amount] [numeric](18, 2) NULL,
	[Dc_No] [varchar](35) NULL,
	[Dc_Date] [varchar](35) NULL,
	[Order_No] [varchar](50) NULL,
	[Order_Date] [varchar](50) NULL,
	[Against_CForm_Status] [tinyint] NULL,
	[Entry_Type] [varchar](20) NULL,
	[Payment_Terms] [varchar](100) NULL,
	[OnAc_IdNo] [int] NULL,
	[Extra_Charges] [numeric](18, 2) NULL,
	[Total_Extra_Copies] [numeric](18, 2) NULL,
	[Sub_Total_Copies] [numeric](18, 2) NULL,
	[Party_Name] [varchar](50) NULL,
	[Weight] [numeric](18, 3) NULL,
	[Sales_OrderAc_IdNo] [int] NULL,
	[Order_Close] [int] NULL,
	[Sales_Order_Selection_Code] [varchar](100) NULL,
	[Quotation_No] [varchar](30) NULL,
	[Quotation_Date] [varchar](30) NULL,
 CONSTRAINT [PK_Sales_Order_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Order_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Quotation_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Quotation_Details](
	[Sales_Quotation_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Quotation_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Quotation_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[ItemGroup_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Item_Description] [varchar](500) NULL,
	[Sales_Quotation_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Cash_Discount_Amount_For_All_Item] [numeric](18, 2) NULL,
	[Cash_Discount_Perc_For_All_Item] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[HSN_Code] [varchar](50) NULL,
	[GST_Perc] [numeric](18, 2) NULL,
	[Order_Quantity] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Sales_Quotation_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Quotation_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Quotation_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Quotation_Head](
	[Sales_Quotation_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Quotation_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Quotation_Head_for_OrderBy]  DEFAULT ((0)),
	[Sales_Quotation_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Sales_Quotation_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Delivery_Terms] [varchar](50) NULL CONSTRAINT [DF_Sales_Quotation_Head_Delivery_Terms]  DEFAULT (''),
	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Quotation_Head_Total_Qty]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Gross_Amount]  DEFAULT ((0)),
	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_CashDiscount_Perc]  DEFAULT ((0)),
	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_CashDiscount_Amount]  DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Assessable_Value]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Tax_Amount]  DEFAULT ((0)),
	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Freight_Amount]  DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_AddLess_Amount]  DEFAULT ((0)),
	[Labour_Charge] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Labour_Charge]  DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Round_Off]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Quotation_Head_Net_Amount]  DEFAULT ((0)),
	[Payment_Terms] [varchar](100) NULL CONSTRAINT [DF_Sales_Quotation_Head_PaymentTerms]  DEFAULT (''),
	[Finalised_Rate] [numeric](18, 3) NULL DEFAULT ((0)),
	[Remarks] [varchar](500) NULL DEFAULT (''),
	[Sales_Quotation_Image] [image] NULL,
	[Sales_Quotation_Image2] [image] NULL,
	[Order_No] [varchar](100) NULL DEFAULT (''),
	[Design1] [varchar](100) NULL DEFAULT (''),
	[Design2] [varchar](100) NULL DEFAULT (''),
	[Stitches1] [int] NULL DEFAULT ((0)),
	[Stitches2] [int] NULL DEFAULT ((0)),
	[Rate_Applique] [numeric](18, 2) NULL DEFAULT ((0)),
	[Rate_Embroidery] [numeric](18, 2) NULL DEFAULT ((0)),
	[Rate_Stitches] [numeric](18, 2) NULL DEFAULT ((0)),
	[Rate_Pasting] [numeric](18, 2) NULL DEFAULT ((0)),
	[Entry_VAT_GST_Type] [varchar](100) NULL DEFAULT (''),
	[Entry_GST_Tax_Type] [varchar](50) NULL DEFAULT (''),
	[CGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[SGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[IGst_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Electronic_Reference_No] [varchar](100) NULL DEFAULT (''),
	[Transportation_Mode] [varchar](100) NULL DEFAULT (''),
	[Date_Time_Of_Supply] [varchar](100) NULL DEFAULT (''),
	[Enquiry_No] [varchar](100) NULL,
	[Enquiry_Date] [varchar](100) NULL,
	[Tax_Type] [varchar](30) NULL DEFAULT (''),
	[UID] [varchar](500) NULL,
	[Prepared_By] [varchar](50) NULL DEFAULT (''),
	[Emb_Part] [varchar](100) NULL DEFAULT (''),
	[Emb_Position] [varchar](100) NULL DEFAULT (''),
	[Emb_Type] [varchar](100) NULL DEFAULT (''),
	[Foam_Removal_rate] [numeric](18, 3) NULL DEFAULT ((0)),
	[Material_rate] [numeric](18, 3) NULL DEFAULT ((0)),
	[Sizes] [varchar](100) NULL DEFAULT ((0)),
	[Thread_Colour_Count] [tinyint] NULL DEFAULT ((0)),
	[No_Of_Appliques] [tinyint] NULL DEFAULT ((0)),
	[No_Of_Sequins] [tinyint] NULL DEFAULT ((0)),
	[Is_Material_Provided] [bit] NULL DEFAULT ((0)),
	[Material_Provided] [varchar](250) NULL DEFAULT (''),
	[Confirmed_By] [varchar](150) NULL DEFAULT (''),
	[Contact_Person] [varchar](150) NULL DEFAULT (''),
	[Rejection_Allowance] [tinyint] NULL DEFAULT ((0)),
	[Contact_Person_Phone] [varchar](15) NULL DEFAULT (''),
 CONSTRAINT [PK_Sales_Quotation_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Quotation_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Reading_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Reading_Details](
	[Sales_Code] [varchar](50) NOT NULL,
	[Sales_No] [varchar](50) NOT NULL,
	[Company_IdNo] [int] NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Date] [smalldatetime] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Machine_IdNo] [smallint] NULL,
	[Opening_Reading] [int] NULL,
	[Closing_Reading] [int] NULL,
	[Sub_Total_Copies] [int] NULL,
	[Extra_Copies] [int] NULL,
 CONSTRAINT [PK_Sales_Reading_Details] PRIMARY KEY NONCLUSTERED 
(
	[Sales_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Receipt_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Receipt_Details](
	[Sales_Receipt_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Receipt_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Receipt_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Style_IdNo] [int] NULL,
	[Size_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[HSN_Code] [varchar](100) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Item_Description] [varchar](500) NULL,
	[No_Of_Rolls] [int] NULL,
	[Delivery_Quantity] [numeric](18, 2) NULL,
	[Delivery_No_Of_Rolls] [int] NULL,
	[Sales_Receipt_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Delivery_Weight] [numeric](18, 2) NULL,
	[Rate_For] [varchar](50) NULL,
	[ItemGroup_IdNo] [int] NULL,
	[Weight] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Sales_Receipt_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Receipt_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Receipt_GST_Tax_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Receipt_GST_Tax_Details](
	[Sales_Receipt_Code] [varchar](50) NOT NULL,
	[Company_Idno] [int] NOT NULL,
	[Sales_Receipt_No] [varchar](50) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Receipt_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[HSN_Code] [varchar](100) NULL,
	[Taxable_Amount] [numeric](18, 2) NULL,
	[CGST_Percentage] [numeric](18, 2) NULL,
	[CGST_Amount] [numeric](18, 2) NULL,
	[SGST_Percentage] [numeric](18, 2) NULL,
	[SGST_Amount] [numeric](18, 2) NULL,
	[IGST_Percentage] [numeric](18, 2) NULL,
	[IGST_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Sales_Receipt_GST_Tax_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Receipt_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Receipt_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Receipt_Head](
	[Sales_Receipt_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Receipt_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Receipt_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Order_No] [varchar](50) NULL,
	[Order_Date] [varchar](50) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Transport_IdNo] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Freight_ToPay_Amount] [numeric](18, 2) NULL,
	[Weight] [numeric](18, 3) NULL,
	[Charge] [numeric](18, 2) NULL,
	[Lr_No] [varchar](50) NULL,
	[Lr_Date] [varchar](50) NULL,
	[Total_Bags] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Electronic_Reference_No] [varchar](100) NULL,
	[Transportation_Mode] [varchar](100) NULL,
	[Date_Time_Of_Supply] [varchar](100) NULL,
	[CGst_Amount] [numeric](18, 2) NULL,
	[SGst_Amount] [numeric](18, 2) NULL,
	[IGst_Amount] [numeric](18, 2) NULL,
	[Challan_No] [varchar](100) NULL,
	[Challan_Date] [varchar](100) NULL,
	[Total_Weight] [numeric](18, 2) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Entry_GST_Tax_Type] [varchar](50) NULL,
	[Booked_By] [varchar](100) NULL,
 CONSTRAINT [PK_Sales_Receipt_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Receipt_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SALES_REG_HSN]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SALES_REG_HSN](
	[Sales_Code] [varchar](50) NULL,
	[HSN_Code] [varchar](10) NULL,
	[UQC] [varchar](50) NULL,
	[QUANTITY] [real] NULL,
	[TAXABLE_AMOUNT] [real] NULL,
	[IGST_RATE] [real] NULL,
	[CGST_RATE] [real] NULL,
	[SGST_RATE] [real] NULL,
	[IGST_Amount] [real] NULL,
	[CGST_Amount] [real] NULL,
	[SGST_Amount] [real] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Return_GST_Tax_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Return_GST_Tax_Details](
	[Sales_Return_Code] [varchar](50) NOT NULL,
	[Company_Idno] [int] NOT NULL,
	[Sales_Return_No] [varchar](50) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Sales_Return_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[HSN_Code] [varchar](100) NULL,
	[Taxable_Amount] [numeric](18, 2) NULL,
	[CGST_Percentage] [numeric](18, 2) NULL,
	[CGST_Amount] [numeric](18, 2) NULL,
	[SGST_Percentage] [numeric](18, 2) NULL,
	[SGST_Amount] [numeric](18, 2) NULL,
	[IGST_Percentage] [numeric](18, 2) NULL,
	[IGST_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Sales_Return_GST_Tax_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Return_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Salesman_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Salesman_Head](
	[Salesman_Idno] [int] NOT NULL,
	[Salesman_Name] [varchar](100) NULL,
	[Sur_Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Salesman_Head] PRIMARY KEY CLUSTERED 
(
	[Salesman_Idno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesReturn_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SalesReturn_Details](
	[SalesReturn_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[SalesReturn_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[SalesReturn_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Noof_Items] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Serial_No] [varchar](500) NULL,
	[Sales_Detail_Slno] [int] NULL,
	[Sales_Code] [varchar](30) NULL,
	[Tax_Rate] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Colour_IdNo] [int] NULL,
	[Design_IdNo] [int] NULL,
	[Gender_IdNo] [int] NULL,
	[Sleeve_IdNo] [int] NULL,
	[Size_IdNo] [int] NULL,
	[Cash_Discount_Perc_For_All_Item] [numeric](18, 2) NULL,
	[Cash_Discount_Amount_For_All_Item] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[HSN_Code] [varchar](50) NULL,
	[Return_Qty] [numeric](18, 2) NULL,
	[GST_Percentage] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[net_Amount] [numeric](18, 2) NULL,
	[Actual_Amount] [numeric](18, 2) NULL,
	[Actual_Rate] [numeric](18, 2) NULL,
 CONSTRAINT [PK_SalesReturn_Details] PRIMARY KEY CLUSTERED 
(
	[SalesReturn_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesReturn_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SalesReturn_Head](
	[SalesReturn_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[SalesReturn_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[SalesReturn_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL,
	[Ledger_IdNo] [int] NULL,
	[Cash_PartyName] [varchar](50) NULL,
	[Bill_No] [varchar](35) NULL,
	[SalesReturnAc_IdNo] [int] NULL,
	[Tax_Type] [varchar](20) NULL,
	[TaxAc_IdNo] [int] NULL,
	[Narration] [varchar](500) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[CashDiscount_Perc] [numeric](18, 2) NULL,
	[CashDiscount_Amount] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Bill_Date] [varchar](35) NULL,
	[Total_DiscountAmount] [numeric](18, 2) NULL,
	[Total_TaxAmount] [numeric](18, 2) NULL,
	[Freight_Amount] [numeric](18, 2) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Total_Bags] [int] NULL,
	[Weight] [numeric](18, 3) NULL,
	[Against_CForm_Status] [int] NULL,
	[Order_No] [varchar](35) NULL,
	[Order_Date] [varchar](20) NULL,
	[Lr_No] [varchar](35) NULL,
	[Lr_Date] [varchar](20) NULL,
	[Document_Through] [varchar](50) NULL,
	[Booked_By] [varchar](50) NULL,
	[Despatch_To] [varchar](50) NULL,
	[SalesAc_IdNo] [int] NULL,
	[Entry_VAT_GST_Type] [varchar](100) NULL,
	[Electronic_Reference_No] [varchar](100) NULL,
	[Transportation_Mode] [varchar](100) NULL,
	[Date_Time_Of_Supply] [varchar](100) NULL,
	[Entry_GST_Tax_Type] [varchar](50) NULL,
	[CGst_Amount] [numeric](18, 2) NULL,
	[SGst_Amount] [numeric](18, 2) NULL,
	[IGst_Amount] [numeric](18, 2) NULL,
	[Actual_Net_Amount] [numeric](18, 2) NULL,
	[Actual_Gross_Amount] [numeric](18, 2) NULL,
	[Sales_Order_Selection_Code] [varchar](50) NULL,
 CONSTRAINT [PK_SalesReturn_Head] PRIMARY KEY CLUSTERED 
(
	[SalesReturn_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Scheme_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Scheme_Details](
	[Scheme_IdNo] [int] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Discount_Percentage] [numeric](18, 2) NULL,
	[Primary_StartDate] [smalldatetime] NULL,
	[Primary_EndDate] [smalldatetime] NULL,
	[Secondary_StartDate] [smalldatetime] NULL,
	[Secondary_EndDate] [smalldatetime] NULL,
	[Primary_StartDate_Text] [varchar](50) NULL,
	[Primary_EndDate_Text] [varchar](50) NULL,
	[Secondary_StartDate_Text] [varchar](50) NULL,
	[Secondary_EndDate_Text] [varchar](50) NULL,
 CONSTRAINT [PK_Scheme_Details] PRIMARY KEY CLUSTERED 
(
	[Scheme_IdNo] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Scheme_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Scheme_Head](
	[Scheme_IdNo] [int] NOT NULL,
	[Scheme_Name] [varchar](200) NULL,
	[Sur_Name] [varchar](200) NULL,
	[Cetegory_IdNo] [int] NULL,
 CONSTRAINT [PK_Scheme_Head] PRIMARY KEY CLUSTERED 
(
	[Scheme_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Settings_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Settings_Head](
	[Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[C_Name] [varchar](50) NULL CONSTRAINT [DF__Settings___C_Nam__795DFB40]  DEFAULT (''),
	[AutoBackUp_Date] [smalldatetime] NULL,
	[S_Name] [varchar](50) NULL,
	[EmpDate_indx] [int] NULL,
	[EmpCode_indx] [int] NULL,
	[EmpInOutMode_indx] [int] NULL,
 CONSTRAINT [PK_Settings_Head] PRIMARY KEY CLUSTERED 
(
	[Auto_SlNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Shift_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Shift_Head](
	[Shift_IdNo] [int] NOT NULL,
	[Shift_Name] [varchar](50) NOT NULL,
	[Total_Hours] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_Minutes] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_Shift_Head] PRIMARY KEY CLUSTERED 
(
	[Shift_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Simple_Receipt_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Simple_Receipt_Details](
	[Simple_Receipt_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Simple_Receipt_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Simple_Receipt_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Quantity] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Item_Description] [varchar](500) NULL,
	[Simple_Receipt_Detail_Slno] [int] IDENTITY(1,1) NOT NULL,
	[Receipt_Quantity] [numeric](18, 2) NULL,
	[No_Of_Rolls] [numeric](18, 2) NULL,
	[Entry_Type] [varchar](30) NULL,
	[Order_Detail_SlNo] [int] NULL,
	[Noof_Items] [numeric](18, 2) NULL,
	[HSN_Code] [varchar](50) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Size_Idno] [int] NULL,
	[Style_Idno] [int] NULL,
	[Colour_IdNo] [int] NULL DEFAULT ((0)),
	[Order_No] [varchar](100) NULL DEFAULT (''),
	[Ordercode_forSelection] [varchar](100) NULL DEFAULT (''),
	[Job_NO] [varchar](100) NULL,
	[Bundles] [tinyint] NULL,
	[Receipt_Purpose] [varchar](50) NULL,
	[Component_IdNo] [int] NULL,
 CONSTRAINT [PK_Simple_Receipt_Details] PRIMARY KEY CLUSTERED 
(
	[Simple_Receipt_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Simple_Receipt_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Simple_Receipt_Head](
	[Simple_Receipt_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Simple_Receipt_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Simple_Receipt_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[Order_No] [varchar](50) NULL,
	[Order_Date] [varchar](50) NULL,
	[Total_Qty] [numeric](18, 3) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Transport_IdNo] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[Entry_VAT_GST_Type] [varchar](50) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[CGst_Amount] [numeric](18, 2) NULL,
	[SGst_Amount] [numeric](18, 2) NULL,
	[IGst_Amount] [numeric](18, 2) NULL,
	[SubTotal_Amount] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Entry_GST_Tax_Type] [varchar](20) NULL,
	[Total_Bags] [int] NULL,
	[Electronic_Reference_No] [varchar](100) NULL,
	[Transportation_Mode] [varchar](100) NULL,
	[Date_Time_Of_Supply] [varchar](100) NULL,
	[Weight] [numeric](18, 2) NULL,
	[Freight_ToPay_Amount] [numeric](18, 2) NULL,
	[charge] [numeric](18, 2) NULL,
	[Lr_Date] [varchar](50) NULL,
	[Lr_No] [varchar](50) NULL,
	[Booked_By] [varchar](50) NULL,
	[Entry_Type] [varchar](50) NULL,
	[Return_Reason] [varchar](250) NULL DEFAULT (''),
	[IsReturn] [bit] NULL DEFAULT ((0)),
	[Total_Bundles] [tinyint] NULL,
 CONSTRAINT [PK_Simple_Receipt_Head] PRIMARY KEY CLUSTERED 
(
	[Simple_Receipt_Code] ASC,
	[Simple_Receipt_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Site_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Site_Head](
	[Site_IdNo] [smallint] NOT NULL,
	[Site_Name] [varchar](100) NOT NULL,
	[Sur_Name] [varchar](100) NOT NULL,
	[Pk_Condition] [varchar](50) NULL,
 CONSTRAINT [PK_Site_Head] PRIMARY KEY CLUSTERED 
(
	[Site_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Size_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Size_Head](
	[Size_IdNo] [int] NOT NULL,
	[Size_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Total_Sqft] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_Size_Head] PRIMARY KEY CLUSTERED 
(
	[Size_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Size_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sleeve_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sleeve_Head](
	[Sleeve_IdNo] [int] NOT NULL,
	[Sleeve_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Sleeve_Head] PRIMARY KEY CLUSTERED 
(
	[Sleeve_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Sleeve_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Spinning_WasteSales_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Spinning_WasteSales_Details](
	[Spinning_WasteSales_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Spinning_WasteSales_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Spinning_WasteSales_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Waste_IdNo] [int] NULL,
	[Unit_IdNo] [smallint] NULL,
	[Packs] [int] NULL,
	[Weight] [numeric](18, 3) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Pack_Nos] [varchar](500) NULL,
 CONSTRAINT [PK_Spinning_WasteSales_Details] PRIMARY KEY CLUSTERED 
(
	[Spinning_WasteSales_Code] ASC,
	[SL_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Spinning_WasteSales_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Spinning_WasteSales_Head](
	[Spinning_WasteSales_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Spinning_WasteSales_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Spinning_WasteSales_Date] [datetime] NOT NULL,
	[Ledger_IdNo] [int] NULL,
	[SalesAc_IdNo] [int] NULL,
	[TaxAc_IdNo] [int] NULL,
	[CessAc_IdNo] [int] NULL,
	[Delivery_Address1] [varchar](50) NULL,
	[Delivery_Address2] [varchar](50) NULL,
	[Delivery_Address3] [varchar](50) NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Removal_Date] [varchar](20) NULL,
	[Pack_Nos] [varchar](500) NULL,
	[Total_Packs] [int] NULL,
	[Total_Weight] [numeric](18, 3) NULL,
	[Gross_Amount] [numeric](18, 2) NULL,
	[CashDiscount_Perc] [numeric](18, 2) NULL,
	[CashDiscount_Amount] [numeric](18, 2) NULL,
	[Assessable_Value] [numeric](18, 2) NULL,
	[Tax_Perc] [numeric](18, 2) NULL,
	[Tax_Amount] [numeric](18, 2) NULL,
	[Cess_Perc] [numeric](18, 2) NULL,
	[Cess_Amount] [numeric](18, 2) NULL,
	[AddLess_Amount] [numeric](18, 2) NULL,
	[Round_Off] [numeric](18, 2) NULL,
	[Net_Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Spinning_WasteSales_Head] PRIMARY KEY CLUSTERED 
(
	[Spinning_WasteSales_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[State_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[State_Head](
	[State_IdNo] [smallint] NOT NULL,
	[State_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Cst_Value] [int] NOT NULL,
	[State_Code] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_State_Head] PRIMARY KEY CLUSTERED 
(
	[State_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Style_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Style_Head](
	[Style_IdNo] [int] NOT NULL,
	[Style_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Style_Head] PRIMARY KEY CLUSTERED 
(
	[Style_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Style_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tax_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tax_Head](
	[Tax_IdNo] [int] NOT NULL,
	[Tax_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Tax_Ledger_Ac_IdNo] [int] NULL CONSTRAINT [DF_Tax_Head_Tax_Ledger_Ac_IdNo]  DEFAULT ((0)),
 CONSTRAINT [PK_Tax_Head] PRIMARY KEY CLUSTERED 
(
	[Tax_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Tax_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Temp_Ends_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Temp_Ends_Head](
	[Ends_Name] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[temp_report]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[temp_report](
	[ledger_name] [varchar](100) NULL,
	[ledger_address1] [varchar](50) NULL,
	[ledger_address2] [varchar](50) NULL,
	[ledger_address3] [varchar](50) NULL,
	[ledger_address4] [varchar](50) NULL,
	[ledger_phoneno] [varchar](50) NULL,
	[ledger_tinno] [varchar](50) NULL,
	[ledger_idno] [int] NULL,
	[status_for_row] [varchar](10) NULL,
	[auto_slno_for_orderby] [int] IDENTITY(1,1) NOT NULL,
	[auto_slno_for_orderby_group] [numeric](9, 1) NULL,
	[row_data] [varchar](3000) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TempTable_For_NegativeStock]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TempTable_For_NegativeStock](
	[Reference_Code] [varchar](50) NULL,
	[Reference_Date] [smalldatetime] NULL,
	[Company_Idno] [smallint] NULL,
	[Item_IdNo] [int] NULL,
	[Quantity] [numeric](18, 3) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tocken_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tocken_Head](
	[Tocken_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [int] NOT NULL,
	[Tocken_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Tocken_Date] [smalldatetime] NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Ledger_Idno] [int] NULL,
	[InTime] [smalldatetime] NULL,
	[OutTime] [smalldatetime] NULL,
	[InDateTime] [datetime] NULL,
	[OutDateTime] [datetime] NULL,
	[Total_Hrs] [numeric](18, 2) NULL,
	[Total_Days] [numeric](18, 2) NULL,
	[Party_Name] [varchar](100) NULL,
	[Address1] [varchar](200) NULL,
	[Address2] [varchar](200) NULL,
	[Address3] [varchar](200) NULL,
	[Address4] [varchar](200) NULL,
	[Mobile_No] [varchar](100) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Tocken_Type] [varchar](50) NULL,
	[Vehicle_Type] [varchar](100) NULL,
 CONSTRAINT [PK_Tocken_Head] PRIMARY KEY CLUSTERED 
(
	[Tocken_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Token_Monthly_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Token_Monthly_Head](
	[Token_Monthly_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [int] NULL,
	[Token_Monthly_No] [varchar](100) NULL,
	[for_OrderBy] [numeric](18, 2) NULL,
	[Token_Monthly_Date] [smalldatetime] NULL,
	[Vehicle_No] [varchar](50) NULL,
	[Ledger_Idno] [int] NULL,
	[StartDate] [smalldatetime] NULL,
	[EndDate] [smalldatetime] NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[Total_Days] [numeric](18, 2) NULL,
	[Party_Name] [varchar](100) NULL,
	[Party_Address1] [varchar](100) NULL,
	[Party_Address2] [varchar](100) NULL,
	[Party_Address3] [varchar](100) NULL,
	[Party_Address4] [varchar](100) NULL,
	[Party_MobileNo] [varchar](100) NULL,
	[Rate] [numeric](18, 2) NULL,
	[Amount] [numeric](18, 2) NULL,
	[Vehicle_Type] [varchar](100) NULL,
	[Sur_Name] [varchar](100) NULL,
	[Close_Status] [int] NULL,
 CONSTRAINT [PK_Token_Monthly_Head] PRIMARY KEY CLUSTERED 
(
	[Token_Monthly_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Transport_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Transport_Head](
	[Transport_IdNo] [smallint] NOT NULL,
	[Transport_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Transport_Head] PRIMARY KEY CLUSTERED 
(
	[Transport_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Transport_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Unit_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Unit_Head](
	[Unit_IdNo] [smallint] NOT NULL,
	[Unit_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Unit_Head] PRIMARY KEY CLUSTERED 
(
	[Unit_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Unit_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Variety_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Variety_Head](
	[Variety_IdNo] [smallint] NOT NULL,
	[Variety_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Variety_Head] PRIMARY KEY CLUSTERED 
(
	[Variety_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Variety_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Voucher_Bill_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Voucher_Bill_Details](
	[Voucher_Bill_Code] [varchar](50) NULL,
	[Company_Idno] [smallint] NOT NULL,
	[Voucher_Bill_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [smallint] NOT NULL,
	[Entry_Identification] [varchar](100) NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[CrDr_Type] [varchar](10) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Voucher_Bill_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Voucher_Bill_Head](
	[Voucher_Bill_Code] [varchar](100) NOT NULL,
	[Company_Idno] [smallint] NOT NULL,
	[Voucher_Bill_No] [varchar](100) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Voucher_Bill_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [smallint] NOT NULL CONSTRAINT [DF_Voucher_Bill_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Party_Bill_No] [varchar](20) NULL CONSTRAINT [DF_Voucher_Bill_Head_Party_Bill_No]  DEFAULT (''),
	[Agent_IdNo] [smallint] NULL CONSTRAINT [DF_VoucherBillHead_AgentIdNo]  DEFAULT ((0)),
	[Bill_Amount] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Voucher_Bill_Head_Bill_Amount]  DEFAULT ((0)),
	[Credit_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_VoucherBillHead_CreditAmount]  DEFAULT ((0)),
	[Debit_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_VoucherBillHead_DebitAmount]  DEFAULT ((0)),
	[CrDr_Type] [varchar](10) NOT NULL,
	[Entry_Identification] [varchar](100) NOT NULL CONSTRAINT [DF_Voucher_Bill_Head_Entry_Identification]  DEFAULT (''),
	[Commission_Percentage] [numeric](18, 2) NULL CONSTRAINT [DF_Voucher_Bill_Head_Commission_Percentage]  DEFAULT ((0)),
	[Agent_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Voucher_Bill_Head_Agent_Amount]  DEFAULT ((0)),
	[Voucher_Bill_DetailsSlNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Voucher_Bills_Head] PRIMARY KEY NONCLUSTERED 
(
	[Voucher_Bill_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Voucher_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Voucher_Details](
	[Voucher_Code] [varchar](100) NOT NULL,
	[For_OrderByCode] [numeric](18, 2) NOT NULL,
	[Company_Idno] [smallint] NOT NULL,
	[Voucher_No] [varchar](100) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Voucher_Type] [varchar](20) NOT NULL,
	[Voucher_Date] [smalldatetime] NOT NULL,
	[Sl_No] [tinyint] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Voucher_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_VoucherDetails_VoucherAmount]  DEFAULT ((0)),
	[Narration] [varchar](1000) NULL CONSTRAINT [DF_Voucher_Details_Narration]  DEFAULT (''),
	[Year_For_Report] [smallint] NOT NULL CONSTRAINT [DF_Voucher_Details_Year_For_Report]  DEFAULT ((0)),
	[Entry_Identification] [varchar](100) NOT NULL CONSTRAINT [DF_Voucher_Details_Entry_Identification]  DEFAULT (''),
	[Entry_ID] [varchar](100) NULL DEFAULT (''),
 CONSTRAINT [PK_Voucher_Details] PRIMARY KEY CLUSTERED 
(
	[Voucher_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Dup_VoucherDetails_EntryIdentication_SlNo] UNIQUE NONCLUSTERED 
(
	[Entry_Identification] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Voucher_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Voucher_Head](
	[Voucher_Code] [varchar](100) NOT NULL,
	[For_OrderByCode] [numeric](18, 2) NOT NULL,
	[Company_Idno] [smallint] NOT NULL,
	[Voucher_No] [varchar](100) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Voucher_Type] [varchar](20) NOT NULL,
	[Voucher_Date] [smalldatetime] NOT NULL,
	[Debtor_Idno] [int] NOT NULL,
	[Creditor_Idno] [int] NOT NULL,
	[Narration] [varchar](1000) NULL CONSTRAINT [DF_Voucher_Head_Narration]  DEFAULT (''),
	[Total_VoucherAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Voucher_Head_Total_VoucherAmount]  DEFAULT ((0)),
	[Indicate] [tinyint] NULL CONSTRAINT [DF_VoucherHead_Indicate]  DEFAULT ((0)),
	[Year_For_Report] [smallint] NOT NULL CONSTRAINT [DF_Voucher_Head_Year_For_Report]  DEFAULT ((0)),
	[Entry_Identification] [varchar](100) NOT NULL,
	[Voucher_Receipt_Code] [varchar](100) NULL CONSTRAINT [DF_VoucherHead_VoucherReceiptNo]  DEFAULT (''),
	[Entry_ID] [varchar](30) NULL DEFAULT (''),
 CONSTRAINT [PK_Voucher_Head] PRIMARY KEY CLUSTERED 
(
	[Voucher_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Dup_VoucherHead_EntryIndentification] UNIQUE NONCLUSTERED 
(
	[Entry_Identification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Voucher_Order_Details]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Voucher_Order_Details](
	[Voucher_Code] [varchar](50) NOT NULL,
	[For_OrderByCode] [numeric](18, 2) NOT NULL,
	[Company_Idno] [int] NOT NULL,
	[Voucher_No] [varchar](50) NOT NULL,
	[For_OrderBy] [numeric](18, 2) NOT NULL,
	[Voucher_Type] [varchar](20) NOT NULL,
	[Voucher_Date] [smalldatetime] NOT NULL,
	[Sl_No] [int] NOT NULL,
	[Sales_Order_Selection_Code] [varchar](20) NOT NULL,
	[Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Voucher_Order_Details] PRIMARY KEY CLUSTERED 
(
	[Voucher_Code] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Waste_Head]    Script Date: 15/02/2021 16:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Waste_Head](
	[Waste_IdNo] [smallint] NOT NULL,
	[Waste_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Waste_Head_Unit_IdNo]  DEFAULT ((0)),
 CONSTRAINT [PK_Waste_Head] PRIMARY KEY CLUSTERED 
(
	[Waste_IdNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Waste_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Item_ExcessShort_Head]    Script Date: 15/02/2021 16:12:55 ******/
CREATE NONCLUSTERED INDEX [IX_Item_ExcessShort_Head] ON [dbo].[Item_ExcessShort_Head]
(
	[Company_IdNo] ASC,
	[Item_ExcessShort_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Item_Processing_Details]    Script Date: 15/02/2021 16:12:55 ******/
CREATE NONCLUSTERED INDEX [IX_Item_Processing_Details] ON [dbo].[Item_Processing_Details]
(
	[Company_IdNo] ASC,
	[Reference_No] ASC,
	[Sl_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Purhase_Head]    Script Date: 15/02/2021 16:12:55 ******/
CREATE NONCLUSTERED INDEX [IX_Purhase_Head] ON [dbo].[Purchase_Head]
(
	[Purchase_Date] ASC,
	[for_OrderBy] ASC,
	[Purchase_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cloth_Sales_Head] ADD  DEFAULT ((0)) FOR [Discount_Percentage]
GO
ALTER TABLE [dbo].[Cloth_Sales_Head] ADD  DEFAULT ((0)) FOR [Discount_Amount]
GO
ALTER TABLE [dbo].[Cloth_Sales_Head] ADD  DEFAULT ((0)) FOR [net_Amount]
GO
ALTER TABLE [dbo].[Cloth_Sales_Head] ADD  DEFAULT ('') FOR [Bale_Nos]
GO
ALTER TABLE [dbo].[Cloth_Sales_Head] ADD  DEFAULT ((0)) FOR [Agent_IdNo]
GO
ALTER TABLE [dbo].[Cloth_Sales_Head] ADD  DEFAULT ((0)) FOR [Add_Less]
GO
ALTER TABLE [dbo].[Delivery_Details] ADD  DEFAULT ((0)) FOR [Actual_Weight]
GO
ALTER TABLE [dbo].[Delivery_Head] ADD  DEFAULT ((0)) FOR [Total_Actual_Weight]
GO
ALTER TABLE [dbo].[Delivery_Head] ADD  DEFAULT ('') FOR [Invoice_Code]
GO
ALTER TABLE [dbo].[Delivery_Head] ADD  DEFAULT ('') FOR [NoOf_Bundle]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name1]  DEFAULT ('') FOR [Name1]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name2]  DEFAULT ('') FOR [Name2]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name3]  DEFAULT ('') FOR [Name3]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name4]  DEFAULT ('') FOR [Name4]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name5]  DEFAULT ('') FOR [Name5]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name6]  DEFAULT ('') FOR [Name6]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_name7]  DEFAULT ('') FOR [name7]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name8]  DEFAULT ('') FOR [Name8]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name9]  DEFAULT ('') FOR [Name9]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Name10]  DEFAULT ('') FOR [Name10]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int1]  DEFAULT ((0)) FOR [Int1]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int2]  DEFAULT ((0)) FOR [Int2]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int3]  DEFAULT ((0)) FOR [Int3]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int4]  DEFAULT ((0)) FOR [Int4]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int5]  DEFAULT ((0)) FOR [Int5]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int6]  DEFAULT ((0)) FOR [Int6]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int7]  DEFAULT ((0)) FOR [Int7]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int8]  DEFAULT ((0)) FOR [Int8]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int9]  DEFAULT ((0)) FOR [Int9]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Int10]  DEFAULT ((0)) FOR [Int10]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters1]  DEFAULT ((0)) FOR [Meters1]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters2]  DEFAULT ((0)) FOR [Meters2]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters3]  DEFAULT ((0)) FOR [Meters3]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters4]  DEFAULT ((0)) FOR [Meters4]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters5]  DEFAULT ((0)) FOR [Meters5]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters6]  DEFAULT ((0)) FOR [Meters6]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters7]  DEFAULT ((0)) FOR [Meters7]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters8]  DEFAULT ((0)) FOR [Meters8]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters9]  DEFAULT ((0)) FOR [Meters9]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Meters10]  DEFAULT ((0)) FOR [Meters10]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight1]  DEFAULT ((0)) FOR [Weight1]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight2]  DEFAULT ((0)) FOR [Weight2]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight3]  DEFAULT ((0)) FOR [Weight3]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight4]  DEFAULT ((0)) FOR [Weight4]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight5]  DEFAULT ((0)) FOR [Weight5]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight6]  DEFAULT ((0)) FOR [Weight6]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight7]  DEFAULT ((0)) FOR [Weight7]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight8]  DEFAULT ((0)) FOR [Weight8]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight9]  DEFAULT ((0)) FOR [Weight9]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Weight10]  DEFAULT ((0)) FOR [Weight10]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency1]  DEFAULT ((0)) FOR [Currency1]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency2]  DEFAULT ((0)) FOR [Currency2]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency3]  DEFAULT ((0)) FOR [Currency3]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency4]  DEFAULT ((0)) FOR [Currency4]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency5]  DEFAULT ((0)) FOR [Currency5]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency6]  DEFAULT ((0)) FOR [Currency6]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency7]  DEFAULT ((0)) FOR [Currency7]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency8]  DEFAULT ((0)) FOR [Currency8]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency9]  DEFAULT ((0)) FOR [Currency9]
GO
ALTER TABLE [dbo].[EntryTemp_Simple] ADD  CONSTRAINT [DF_EntryTemp_Simple_Currency10]  DEFAULT ((0)) FOR [Currency10]
GO
ALTER TABLE [dbo].[ESI_PF_Head] ADD  DEFAULT ((0)) FOR [ESI_AUDIT_STATUS]
GO
ALTER TABLE [dbo].[ESI_PF_Head] ADD  DEFAULT ((0)) FOR [PF_AUDIT_STATUS]
GO
ALTER TABLE [dbo].[ESI_PF_Head] ADD  DEFAULT ((0)) FOR [ESI_SALARY_STATUS]
GO
ALTER TABLE [dbo].[ESI_PF_Head] ADD  DEFAULT ((0)) FOR [PF_SALARY_STATUS]
GO
ALTER TABLE [dbo].[Holiday_Details] ADD  CONSTRAINT [DF_Table_1_Count_IdNo_2]  DEFAULT ('') FOR [Holiday_Date]
GO
ALTER TABLE [dbo].[Item_Details] ADD  CONSTRAINT [DF_Item_Details_Size_IdNo]  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[Item_Details] ADD  CONSTRAINT [DF_Item_Details_Purchase_Rate]  DEFAULT ((0)) FOR [Purchase_Rate]
GO
ALTER TABLE [dbo].[Item_Details] ADD  CONSTRAINT [DF_Item_Details_Sales_rate]  DEFAULT ((0)) FOR [Sales_rate]
GO
ALTER TABLE [dbo].[Item_Details] ADD  DEFAULT ((0)) FOR [Piece_Box]
GO
ALTER TABLE [dbo].[Item_ExcessShort_Head] ADD  CONSTRAINT [DF_Item_ExcessShort_Head_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Manufactured_Day]  DEFAULT ((0)) FOR [Manufactured_Day]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Manufactured_Month_IdNo]  DEFAULT ((0)) FOR [Manufactured_Month_IdNo]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Manufactured_Year]  DEFAULT ((0)) FOR [Manufactured_Year]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Expiry_Period_Days]  DEFAULT ((0)) FOR [Expiry_Period_Days]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Expiray_Day]  DEFAULT ((0)) FOR [Expiry_Day]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Expiray_Month_IdNo]  DEFAULT ((0)) FOR [Expiry_Month_IdNo]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Expiray_Year]  DEFAULT ((0)) FOR [Expiry_Year]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_Purchase_Rate]  DEFAULT ((0)) FOR [Purchase_Rate]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] ADD  CONSTRAINT [DF_Item_Stock_Selection_Processing_Details_OutWard_Quantity]  DEFAULT ((0)) FOR [OutWard_Quantity]
GO
ALTER TABLE [dbo].[Job_Card_Details] ADD  CONSTRAINT [DF_Job_Card_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Job_Card_Details] ADD  CONSTRAINT [DF_Job_Card_Details_SL_No]  DEFAULT ((0)) FOR [SL_No]
GO
ALTER TABLE [dbo].[Job_Card_Details] ADD  CONSTRAINT [DF_Job_Card_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Job_Card_Head] ADD  CONSTRAINT [DF_Job_Card_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Job_Card_Head] ADD  CONSTRAINT [DF_Job_Card_Head_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Job_Card_Head] ADD  CONSTRAINT [DF_Job_Card_Head_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Job_Card_Head] ADD  CONSTRAINT [DF_Job_Card_Head_Total_Quantity]  DEFAULT ((0)) FOR [Total_Quantity]
GO
ALTER TABLE [dbo].[Job_Card_Head] ADD  DEFAULT ((0)) FOR [Total_WasteQuantity]
GO
ALTER TABLE [dbo].[JobWork_Head] ADD  DEFAULT ('') FOR [Sales_Code]
GO
ALTER TABLE [dbo].[Knotting_Bill_Details] ADD  CONSTRAINT [DF_Knotting_Bill_Details_Knotting_No]  DEFAULT ('') FOR [Knotting_No]
GO
ALTER TABLE [dbo].[Knotting_Bill_Details] ADD  CONSTRAINT [DF_Knotting_Bill_Details_Shift]  DEFAULT ('') FOR [Shift]
GO
ALTER TABLE [dbo].[Knotting_Bill_Details] ADD  CONSTRAINT [DF_Knotting_Bill_Details_Ends]  DEFAULT ((0)) FOR [Ends]
GO
ALTER TABLE [dbo].[Knotting_Bill_Details] ADD  CONSTRAINT [DF_Knotting_Bill_Details_Loom]  DEFAULT ('') FOR [Loom]
GO
ALTER TABLE [dbo].[Knotting_Bill_Details] ADD  CONSTRAINT [DF_Knotting_Bill_Details_No_Pavu]  DEFAULT ((0)) FOR [No_Pavu]
GO
ALTER TABLE [dbo].[Knotting_Bill_Details] ADD  CONSTRAINT [DF_Knotting_Bill_Details_Knotting_Code]  DEFAULT ('') FOR [Knotting_Code]
GO
ALTER TABLE [dbo].[Knotting_Bill_Head] ADD  CONSTRAINT [DF_Knotting_Bill_Head_Entry_Type]  DEFAULT ('') FOR [Entry_Type]
GO
ALTER TABLE [dbo].[Knotting_Bill_Head] ADD  CONSTRAINT [DF_Knotting_Bill_Head_Total_Pavu]  DEFAULT ((0)) FOR [Total_Pavu]
GO
ALTER TABLE [dbo].[Knotting_Bill_Head] ADD  CONSTRAINT [DF_Knotting_Bill_Head_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Knotting_Bill_Head] ADD  CONSTRAINT [DF_Knotting_Bill_Head_Gross_Amount]  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Knotting_Bill_Head] ADD  CONSTRAINT [DF_Knotting_Bill_Head_AddLess_Amount]  DEFAULT ((0)) FOR [AddLess_Amount]
GO
ALTER TABLE [dbo].[Knotting_Bill_Head] ADD  CONSTRAINT [DF_Knotting_Bill_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[Knotting_Bill_Head] ADD  CONSTRAINT [DF_Knotting_Bill_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Knotting_Head] ADD  CONSTRAINT [DF_Knotting_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Knotting_Head] ADD  CONSTRAINT [DF_Knotting_Head_Shift]  DEFAULT ('') FOR [Shift]
GO
ALTER TABLE [dbo].[Knotting_Head] ADD  CONSTRAINT [DF_Knotting_Head_Loom]  DEFAULT ('') FOR [Loom]
GO
ALTER TABLE [dbo].[Knotting_Head] ADD  CONSTRAINT [DF_Knotting_Head_Ends]  DEFAULT ((0)) FOR [Ends]
GO
ALTER TABLE [dbo].[Knotting_Head] ADD  CONSTRAINT [DF_Knotting_Head_No_Pavu]  DEFAULT ((0)) FOR [No_Pavu]
GO
ALTER TABLE [dbo].[Knotting_Head] ADD  CONSTRAINT [DF_Knotting_Head_Knotting_Bill_Code]  DEFAULT ('') FOR [Knotting_Bill_Code]
GO
ALTER TABLE [dbo].[Knotting_Head] ADD  DEFAULT ((0)) FOR [Knotting_IdNo]
GO
ALTER TABLE [dbo].[Ledger_item_Details] ADD  CONSTRAINT [DF_Ledger_item_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_SL_No]  DEFAULT ((0)) FOR [SL_No]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_Size_IdNo]  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_Binding_No]  DEFAULT ('') FOR [Binding_No]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_NO_of_SET]  DEFAULT ('') FOR [NO_of_SET]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_No_Of_Copies]  DEFAULT ('') FOR [No_Of_Copies]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_Colour_Details]  DEFAULT ('') FOR [Colour_Details]
GO
ALTER TABLE [dbo].[Order_Program_Details] ADD  CONSTRAINT [DF_Order_Program_Details_Paper_Details]  DEFAULT ('') FOR [Paper_Details]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ('') FOR [Item_Particulars]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ('') FOR [Hsn_Sac_Code]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Gst_Perc]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Discount_Perc]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Discount_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Total_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Footer_Cash_Discount_Perc]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Footer_Cash_Discount_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Details] ADD  DEFAULT ((0)) FOR [Taxable_Value]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ('') FOR [Bill_No]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Other_GST_Entry_Ac_IdNo]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [CashDiscount_Perc]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [CashDiscount_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Taxable_Value]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [CGST_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [SGST_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [IGST_AMount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Chess_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Round_Off_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [TaxAmount_RoundOff_Status]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Total_Quantity]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Total_Sub_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Total_DiscountAmount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Total_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Total_Footer_Cash_Discount_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Total_Taxable_Value]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ('') FOR [Remarks]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [User_Idno]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ('') FOR [Unregister_Type]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ('') FOR [Reason_For_Issuing_Note]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Tds_Percentage]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Tds_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Head] ADD  DEFAULT ((0)) FOR [Bill_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ('') FOR [HSN_SAC_Code]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [GST_Percentage]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [Taxable_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [CGST_Percentage]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [CGST_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [SGST_Percentage]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [SGST_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [IGST_Percentage]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [IGST_Amount]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [Chess_Perc]
GO
ALTER TABLE [dbo].[Other_GST_Entry_Tax_Details] ADD  DEFAULT ((0)) FOR [Chess_Amount]
GO
ALTER TABLE [dbo].[PayRoll_Attendance_Timing_Details] ADD  DEFAULT ((0)) FOR [Employee_IdNo]
GO
ALTER TABLE [dbo].[PayRoll_Attendance_Timing_Details] ADD  DEFAULT ('') FOR [InOut_Type]
GO
ALTER TABLE [dbo].[PayRoll_Attendance_Timing_Details] ADD  DEFAULT ('') FOR [InOut_Time_Text]
GO
ALTER TABLE [dbo].[Payroll_AttendanceLog_FromMachine_Details] ADD  CONSTRAINT [DF_Payroll_AttendanceLog_FromMachine_Details_Employee_CardNo]  DEFAULT ('') FOR [Employee_CardNo]
GO
ALTER TABLE [dbo].[Payroll_AttendanceLog_FromMachine_Details] ADD  CONSTRAINT [DF_Payroll_AttendanceLog_FromMachine_Details_IN_Out]  DEFAULT ('') FOR [IN_Out]
GO
ALTER TABLE [dbo].[Payroll_AttendanceLog_FromMachine_Details] ADD  CONSTRAINT [DF_Payroll_AttendanceLog_FromMachine_Details_INOut_DateTime_Text]  DEFAULT ('') FOR [INOut_DateTime_Text]
GO
ALTER TABLE [dbo].[PayRoll_Category_Details] ADD  DEFAULT ((0)) FOR [From_Attendance]
GO
ALTER TABLE [dbo].[PayRoll_Category_Details] ADD  DEFAULT ((0)) FOR [To_Attendance]
GO
ALTER TABLE [dbo].[PayRoll_Category_Details] ADD  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Daily_Working_Head] ADD  CONSTRAINT [DF_PayRoll_Employee_Daily_Working_Start_Time_Text]  DEFAULT ('') FOR [Start_Time_Text]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Daily_Working_Head] ADD  CONSTRAINT [DF_PayRoll_Employee_Daily_Working_End_Time_Text]  DEFAULT ('') FOR [End_Time_Text]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Daily_Working_Head] ADD  CONSTRAINT [DF_PayRoll_Employee_Daily_Working_Work_Description]  DEFAULT ('') FOR [Work_Description]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Details] ADD  CONSTRAINT [DF_PayRoll_Employee_Deduction_Details_Mess]  DEFAULT ((0)) FOR [Mess]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Details] ADD  CONSTRAINT [DF_PayRoll_Employee_Deduction_Details_Medical]  DEFAULT ((0)) FOR [Medical]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Details] ADD  CONSTRAINT [DF_PayRoll_Employee_Deduction_Details_Store]  DEFAULT ((0)) FOR [Store]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Details] ADD  CONSTRAINT [DF_PayRoll_Employee_Deduction_Details_Other_Addition]  DEFAULT ((0)) FOR [Other_Addition]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Head] ADD  DEFAULT ((0)) FOR [Mess_Amount]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Head] ADD  DEFAULT ((0)) FOR [Mess]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Head] ADD  DEFAULT ((0)) FOR [Medical]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Head] ADD  DEFAULT ((0)) FOR [Store]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Head] ADD  DEFAULT ((0)) FOR [Other_Addition]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Deduction_Head] ADD  DEFAULT ((0)) FOR [Other_Deduction]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Details] ADD  DEFAULT ((0)) FOR [Shift_IdNo]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Details] ADD  DEFAULT ((0)) FOR [Weight_From]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Details] ADD  DEFAULT ((0)) FOR [Weight_To]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Details] ADD  DEFAULT ((0)) FOR [Front_Sizing_Wages]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Details] ADD  DEFAULT ((0)) FOR [Back_Sizing_Wages]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Details] ADD  DEFAULT ((0)) FOR [Boiler_Wages]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Details] ADD  DEFAULT ((0)) FOR [Cooker_Wages]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Head] ADD  DEFAULT ((0)) FOR [Front_Warper]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Head] ADD  DEFAULT ((0)) FOR [Back_Warper]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Head] ADD  DEFAULT ((0)) FOR [Helper]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Head] ADD  DEFAULT ((0)) FOR [Front_Sizer]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Head] ADD  DEFAULT ((0)) FOR [Back_Sizer]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Head] ADD  DEFAULT ((0)) FOR [Boiler]
GO
ALTER TABLE [dbo].[PayRoll_Employee_Wages_Head] ADD  DEFAULT ((0)) FOR [Cooker]
GO
ALTER TABLE [dbo].[Payroll_Timing_Addition_Details] ADD  DEFAULT ((0)) FOR [Employee_IdNo]
GO
ALTER TABLE [dbo].[Payroll_Timing_Addition_Details] ADD  DEFAULT ('') FOR [InOut_Type]
GO
ALTER TABLE [dbo].[Payroll_Timing_Addition_Details] ADD  DEFAULT ('') FOR [InOut_Time_Text]
GO
ALTER TABLE [dbo].[Payroll_Timing_Addition_Head] ADD  DEFAULT ('') FOR [Day_Name]
GO
ALTER TABLE [dbo].[PayRoll_Warp_Count_Coolie_Details] ADD  DEFAULT ((0)) FOR [Value]
GO
ALTER TABLE [dbo].[Price_List_Details] ADD  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[Printing_Invoice_Details] ADD  CONSTRAINT [DF_Printing_Invoice_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Printing_Invoice_Details] ADD  CONSTRAINT [DF_Printing_Invoice_Details_SL_No]  DEFAULT ((0)) FOR [SL_No]
GO
ALTER TABLE [dbo].[Printing_Invoice_Details] ADD  CONSTRAINT [DF_Printing_Invoice_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Printing_Invoice_Details] ADD  CONSTRAINT [DF_Printing_Invoice_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Printing_Invoice_Details] ADD  CONSTRAINT [DF_Table_1_Net_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Printing_Invoice_Details] ADD  CONSTRAINT [DF_Printing_Invoice_Details_Order_Program_Code]  DEFAULT ('') FOR [Order_Program_Code]
GO
ALTER TABLE [dbo].[Printing_Invoice_Details] ADD  CONSTRAINT [DF_Printing_Invoice_Details_Order_No]  DEFAULT ('') FOR [Order_No]
GO
ALTER TABLE [dbo].[Printing_Invoice_Head] ADD  CONSTRAINT [DF_Printing_Invoice_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Printing_Invoice_Head] ADD  CONSTRAINT [DF_Table_1_Order_No]  DEFAULT ((0)) FOR [Assesable_Amount]
GO
ALTER TABLE [dbo].[Printing_Invoice_Head] ADD  CONSTRAINT [DF_Table_1_Assesable_Amount2]  DEFAULT ((0)) FOR [Other_Charges]
GO
ALTER TABLE [dbo].[Printing_Invoice_Head] ADD  CONSTRAINT [DF_Table_1_Assesable_Amount1]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Printing_Invoice_Head] ADD  CONSTRAINT [DF_Table_1_Net_Amount1]  DEFAULT ((0)) FOR [Total_Amount]
GO
ALTER TABLE [dbo].[Printing_Order_colour_Details] ADD  CONSTRAINT [DF_Printing_Order_colour_Details_SL_No]  DEFAULT ((0)) FOR [SL_No]
GO
ALTER TABLE [dbo].[Printing_Order_colour_Details] ADD  CONSTRAINT [DF_Printing_Order_colour_Details_Colour_IdNo]  DEFAULT ((0)) FOR [Colour_IdNo]
GO
ALTER TABLE [dbo].[Printing_Order_colour_Details] ADD  CONSTRAINT [DF_Printing_Order_colour_Details_Detail_SlNo]  DEFAULT ((0)) FOR [Detail_SlNo]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_SL_No]  DEFAULT ((0)) FOR [SL_No]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Colour_IdNo]  DEFAULT ('') FOR [Colour_Details]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Size_IdNo]  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Paper_IdNo]  DEFAULT ('') FOR [Paper_Details]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Order_no]  DEFAULT ('') FOR [Order_no]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Binding_No]  DEFAULT ('') FOR [Binding_No]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_NO_of_SET]  DEFAULT ('') FOR [NO_of_SET]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_No_Of_Copies]  DEFAULT ('') FOR [No_Of_Copies]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Order_Program_Code]  DEFAULT ('') FOR [Order_Program_Code]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  CONSTRAINT [DF_Printing_Order_Details_Details_SlNo]  DEFAULT ((0)) FOR [Details_SlNo]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  DEFAULT ('') FOR [Order_No_New]
GO
ALTER TABLE [dbo].[Printing_Order_Details] ADD  DEFAULT ((0)) FOR [Cancel_Status]
GO
ALTER TABLE [dbo].[Printing_Order_Head] ADD  CONSTRAINT [DF_Printing_Order_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Printing_Order_Head] ADD  CONSTRAINT [DF_Printing_Order_Head_advance]  DEFAULT ((0)) FOR [advance]
GO
ALTER TABLE [dbo].[Printing_Order_Head] ADD  CONSTRAINT [DF_Printing_Order_Head_remarks]  DEFAULT ('') FOR [remarks]
GO
ALTER TABLE [dbo].[Printing_Order_Head] ADD  DEFAULT ('') FOR [Advance_Date]
GO
ALTER TABLE [dbo].[Printing_Order_Paper_Details] ADD  CONSTRAINT [DF_Printing_Order_Paper_Details_SL_No]  DEFAULT ((0)) FOR [SL_No]
GO
ALTER TABLE [dbo].[Printing_Order_Paper_Details] ADD  CONSTRAINT [DF_Printing_Order_Paper_Details_Paper_IdNo]  DEFAULT ((0)) FOR [Paper_IdNo]
GO
ALTER TABLE [dbo].[Printing_Order_Paper_Details] ADD  CONSTRAINT [DF_Printing_Order_Paper_Details_Detail_SlNo]  DEFAULT ((0)) FOR [Detail_SlNo]
GO
ALTER TABLE [dbo].[Purchase_BatchNo_Details] ADD  CONSTRAINT [DF_Purchase_BatchNo_Details_Purchase_No]  DEFAULT ('') FOR [Purchase_No]
GO
ALTER TABLE [dbo].[Purchase_BatchNo_Details] ADD  CONSTRAINT [DF_Purchase_BatchNo_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Purchase_BatchNo_Details] ADD  CONSTRAINT [DF_Purchase_BatchNo_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Purchase_BatchNo_Details] ADD  CONSTRAINT [DF_Purchase_BatchNo_Details_Batch_Serial_No]  DEFAULT ('') FOR [Batch_No]
GO
ALTER TABLE [dbo].[Purchase_BatchNo_Details] ADD  CONSTRAINT [DF_Purchase_BatchNo_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Purchase_BatchNo_Details] ADD  CONSTRAINT [DF_Purchase_BatchNo_Details_Item_idNo]  DEFAULT ((0)) FOR [Item_idNo]
GO
ALTER TABLE [dbo].[Purchase_BatchNo_Details] ADD  DEFAULT ((0)) FOR [Detail_SlNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_ItemGroup_IdNo]  DEFAULT ((0)) FOR [ItemGroup_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Noof_Items]  DEFAULT ((0)) FOR [Noof_Items]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Bags]  DEFAULT ((0)) FOR [Bags]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Weight_Bag]  DEFAULT ((0)) FOR [Weight_Bag]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Tax_Rate]  DEFAULT ((0)) FOR [Tax_Rate]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Discount_Perc]  DEFAULT ((0)) FOR [Discount_Perc]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Discount_Amount]  DEFAULT ((0)) FOR [Discount_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Tax_Amount]  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Total_Amount]  DEFAULT ((0)) FOR [Total_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Bag_Nos]  DEFAULT ('') FOR [Bag_Nos]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Serial_No]  DEFAULT ('') FOR [Serial_No]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Size_IdNo]  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Meters]  DEFAULT ((0)) FOR [Meters]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Colour_IdNo]  DEFAULT ((0)) FOR [Colour_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Purchase_Order_Details_Noof_Items_Return]  DEFAULT ((0)) FOR [Noof_Items_Return]
GO
ALTER TABLE [dbo].[Purchase_Order_Details] ADD  CONSTRAINT [DF_Table_1_Sales_Items]  DEFAULT ((0)) FOR [Purchase_Items]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Payment_Method]  DEFAULT ('') FOR [Payment_Method]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Cash_PartyName]  DEFAULT ('') FOR [Cash_PartyName]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Party_PhoneNo]  DEFAULT ('') FOR [Party_PhoneNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Table_1_SalesAc_IdNo]  DEFAULT ((0)) FOR [PurchaseAc_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Tax_Type]  DEFAULT ('') FOR [Tax_Type]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_TaxAc_IdNo]  DEFAULT ((0)) FOR [TaxAc_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address1]  DEFAULT ('') FOR [Delivery_Address1]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address2]  DEFAULT ('') FOR [Delivery_Address2]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address3]  DEFAULT ('') FOR [Delivery_Address3]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Vehicle_No]  DEFAULT ('') FOR [Vehicle_No]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Narration]  DEFAULT ('') FOR [Narration]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Total_Qty]  DEFAULT ((0)) FOR [Total_Qty]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Total_Bags]  DEFAULT ((0)) FOR [Total_Bags]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_SubTotal_Amount]  DEFAULT ((0)) FOR [SubTotal_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Total_DiscountAmount]  DEFAULT ((0)) FOR [Total_DiscountAmount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Total_TaxAmount]  DEFAULT ((0)) FOR [Total_TaxAmount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Gross_Amount]  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_CashDiscount_Perc]  DEFAULT ((0)) FOR [CashDiscount_Perc]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_CashDiscount_Amount]  DEFAULT ((0)) FOR [CashDiscount_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Assessable_Value]  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Tax_Amount]  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Freight_Amount]  DEFAULT ((0)) FOR [Freight_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_AddLess_Amount]  DEFAULT ((0)) FOR [AddLess_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Document_Through]  DEFAULT ('') FOR [Document_Through]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Despatch_To]  DEFAULT ('') FOR [Despatch_To]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Lr_No]  DEFAULT ('') FOR [Lr_No]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Lr_Date]  DEFAULT ('') FOR [Lr_Date]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Booked_By]  DEFAULT ('') FOR [Booked_By]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Transport_IdNo]  DEFAULT ((0)) FOR [Transport_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Freight_ToPay_Amount]  DEFAULT ((0)) FOR [Freight_ToPay_Amount]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Dc_No]  DEFAULT ('') FOR [Dc_No]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Dc_Date]  DEFAULT ('') FOR [Dc_Date]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Order_No]  DEFAULT ('') FOR [Order_No]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Order_Date]  DEFAULT ('') FOR [Order_Date]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Against_CForm_Status]  DEFAULT ((0)) FOR [Against_CForm_Status]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Entry_Type]  DEFAULT ('') FOR [Entry_Type]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Payment_Terms]  DEFAULT ('') FOR [Payment_Terms]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_OnAc_IdNo]  DEFAULT ((0)) FOR [OnAc_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Extra_Charges]  DEFAULT ((0)) FOR [Extra_Charges]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Total_Extra_Copies]  DEFAULT ((0)) FOR [Total_Extra_Copies]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Sub_Total_Copies]  DEFAULT ((0)) FOR [Sub_Total_Copies]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Party_Name]  DEFAULT ('') FOR [Party_Name]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Purchase_Order_Head_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[Purchase_Order_Head] ADD  CONSTRAINT [DF_Table_1_Sales_OrderAc_IdNo]  DEFAULT ((0)) FOR [Purchase_OrderAc_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Noof_Items]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Bales]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Tax_Rate]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Discount_Perc]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Discount_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Total_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ('') FOR [Bale_Nos]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [TaxAmount_Difference]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Footer_Cash_Discount_Perc_For_All_Item]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Footer_Cash_Discount_Amount_For_All_Item]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ('') FOR [HSN_Code]
GO
ALTER TABLE [dbo].[Purchase_Return_Details] ADD  DEFAULT ((0)) FOR [Gst_Perc]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ('') FOR [Payment_Method]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ('') FOR [Bill_No]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [PurchaseAc_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ('') FOR [Tax_Type]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [TaxAc_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ('') FOR [Narration]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ('') FOR [Vehicle_No]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Total_Qty]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Total_Bags]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Total_Weight]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [SubTotal_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Total_TaxAmount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [CashDiscount_Perc]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [CashDiscount_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [AddLess_BeforeTax_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Freight_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [AddLess_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ('') FOR [Bale_Nos]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [CGst_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [SGst_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ((0)) FOR [IGst_Amount]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ('') FOR [Entry_VAT_GST_Type]
GO
ALTER TABLE [dbo].[Purchase_Return_Head] ADD  DEFAULT ('') FOR [Sales_Order_Selection_Code]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  CONSTRAINT [DF_Purchase_Tax_Details_Purchase_No]  DEFAULT ('') FOR [Purchase_No]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  CONSTRAINT [DF_Purchase_Tax_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  CONSTRAINT [DF_Purchase_Tax_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  CONSTRAINT [DF_Table_1_Batch_No]  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  CONSTRAINT [DF_Table_1_Quantity]  DEFAULT ((0)) FOR [Discount_Amount]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  CONSTRAINT [DF_Purchase_Tax_Details_Aessable_Amount]  DEFAULT ((0)) FOR [Aessable_Amount]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  CONSTRAINT [DF_Purchase_Tax_Details_Tax_Pec]  DEFAULT ((0)) FOR [Tax_Pec]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  CONSTRAINT [DF_Purchase_Tax_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Purchase_Tax_Details] ADD  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[report_settings] ADD  CONSTRAINT [DF_report_settings_font_size]  DEFAULT ((10)) FOR [font_size]
GO
ALTER TABLE [dbo].[report_settings] ADD  CONSTRAINT [DF_report_settings_paper_size]  DEFAULT ((0)) FOR [paper_size]
GO
ALTER TABLE [dbo].[report_settings] ADD  CONSTRAINT [DF_report_settings_paper_orientation]  DEFAULT ((0)) FOR [paper_orientation]
GO
ALTER TABLE [dbo].[report_settings] ADD  CONSTRAINT [DF_report_settings_print_mode]  DEFAULT ((0)) FOR [print_mode]
GO
ALTER TABLE [dbo].[report_settings] ADD  CONSTRAINT [DF_report_settings_horizontal_line]  DEFAULT ((0)) FOR [horizontal_line]
GO
ALTER TABLE [dbo].[report_settings_column_size] ADD  CONSTRAINT [DF_report_settings_column_size_noof_characters]  DEFAULT ((0)) FOR [noof_characters]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name1]  DEFAULT ('') FOR [Name1]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name2]  DEFAULT ('') FOR [Name2]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name3]  DEFAULT ('') FOR [Name3]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name4]  DEFAULT ('') FOR [Name4]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name5]  DEFAULT ('') FOR [Name5]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name6]  DEFAULT ('') FOR [Name6]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_name7]  DEFAULT ('') FOR [name7]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name8]  DEFAULT ('') FOR [Name8]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name9]  DEFAULT ('') FOR [Name9]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Name10]  DEFAULT ('') FOR [Name10]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int1]  DEFAULT ((0)) FOR [Int1]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int2]  DEFAULT ((0)) FOR [Int2]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int3]  DEFAULT ((0)) FOR [Int3]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int4]  DEFAULT ((0)) FOR [Int4]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int5]  DEFAULT ((0)) FOR [Int5]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int6]  DEFAULT ((0)) FOR [Int6]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int7]  DEFAULT ((0)) FOR [Int7]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int8]  DEFAULT ((0)) FOR [Int8]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int9]  DEFAULT ((0)) FOR [Int9]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Int10]  DEFAULT ((0)) FOR [Int10]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters1]  DEFAULT ((0)) FOR [Meters1]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters2]  DEFAULT ((0)) FOR [Meters2]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters3]  DEFAULT ((0)) FOR [Meters3]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters4]  DEFAULT ((0)) FOR [Meters4]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters5]  DEFAULT ((0)) FOR [Meters5]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters6]  DEFAULT ((0)) FOR [Meters6]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters7]  DEFAULT ((0)) FOR [Meters7]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters8]  DEFAULT ((0)) FOR [Meters8]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters9]  DEFAULT ((0)) FOR [Meters9]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters102]  DEFAULT ((0)) FOR [Meters10]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters101]  DEFAULT ((0)) FOR [Meters11]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Meters10]  DEFAULT ((0)) FOR [Meters12]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight1]  DEFAULT ((0)) FOR [Weight1]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight2]  DEFAULT ((0)) FOR [Weight2]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight3]  DEFAULT ((0)) FOR [Weight3]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight4]  DEFAULT ((0)) FOR [Weight4]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight5]  DEFAULT ((0)) FOR [Weight5]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight6]  DEFAULT ((0)) FOR [Weight6]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight7]  DEFAULT ((0)) FOR [Weight7]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight8]  DEFAULT ((0)) FOR [Weight8]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight9]  DEFAULT ((0)) FOR [Weight9]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Weight10]  DEFAULT ((0)) FOR [Weight10]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency1]  DEFAULT ((0)) FOR [Currency1]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency2]  DEFAULT ((0)) FOR [Currency2]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency3]  DEFAULT ((0)) FOR [Currency3]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency4]  DEFAULT ((0)) FOR [Currency4]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency5]  DEFAULT ((0)) FOR [Currency5]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency6]  DEFAULT ((0)) FOR [Currency6]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency7]  DEFAULT ((0)) FOR [Currency7]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency8]  DEFAULT ((0)) FOR [Currency8]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency9]  DEFAULT ((0)) FOR [Currency9]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency10]  DEFAULT ((0)) FOR [Currency10]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency11]  DEFAULT ((0)) FOR [Currency11]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  CONSTRAINT [DF_ReportTempSub_Currency12]  DEFAULT ((0)) FOR [Currency12]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  DEFAULT ('') FOR [Name11]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  DEFAULT ('') FOR [Name12]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  DEFAULT ('') FOR [Name13]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  DEFAULT ('') FOR [Name14]
GO
ALTER TABLE [dbo].[ReportTempSub] ADD  DEFAULT ('') FOR [Name15]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_ItemGroup_IdNo]  DEFAULT ((0)) FOR [ItemGroup_IdNo]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Noof_Items]  DEFAULT ((0)) FOR [Noof_Items]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Tax_Amount]  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Total_Amount]  DEFAULT ((0)) FOR [Total_Amount]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Serial_No]  DEFAULT ('') FOR [Serial_No]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Sales_Detail_SlNo]  DEFAULT ((0)) FOR [Sales_Detail_SlNo]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Sales_Code]  DEFAULT ('') FOR [Sales_Code]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Discount_Perc]  DEFAULT ((0)) FOR [Discount_Perc]
GO
ALTER TABLE [dbo].[Sales_Discount_Details] ADD  CONSTRAINT [DF_Sales_Discount_Details_Discount_Amount]  DEFAULT ((0)) FOR [Discount_Amount]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_SalesAc_IdNo]  DEFAULT ((0)) FOR [SalesAc_IdNo]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Tax_Type]  DEFAULT ('') FOR [Tax_Type]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_TaxAc_IdNo]  DEFAULT ((0)) FOR [TaxAc_IdNo]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Narration]  DEFAULT ('') FOR [Narration]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Total_Qty]  DEFAULT ((0)) FOR [Total_Qty]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_SubTotal_Amount]  DEFAULT ((0)) FOR [SubTotal_Amount]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Total_DiscountAmount]  DEFAULT ((0)) FOR [Total_DiscountAmount]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Total_TaxAmount]  DEFAULT ((0)) FOR [Total_TaxAmount]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_AddLess_Amount]  DEFAULT ((0)) FOR [AddLess_Amount]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Selection_Type]  DEFAULT ('') FOR [Selection_Type]
GO
ALTER TABLE [dbo].[Sales_Discount_Head] ADD  CONSTRAINT [DF_Sales_Discount_Head_Agent_idno]  DEFAULT ((0)) FOR [Agent_idno]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_ItemGroup_IdNo]  DEFAULT ((0)) FOR [ItemGroup_IdNo]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_Item_Description]  DEFAULT ('') FOR [Item_Description]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Details] ADD  CONSTRAINT [DF_Sales_Enquiry_Details_Order_Quantity]  DEFAULT ((0)) FOR [Order_Quantity]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Delivery_Terms]  DEFAULT ('') FOR [Delivery_Terms]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Total_Qty]  DEFAULT ((0)) FOR [Total_Qty]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Gross_Amount]  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_CashDiscount_Perc]  DEFAULT ((0)) FOR [CashDiscount_Perc]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_CashDiscount_Amount]  DEFAULT ((0)) FOR [CashDiscount_Amount]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Assessable_Value]  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Tax_Amount]  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Freight_Amount]  DEFAULT ((0)) FOR [Freight_Amount]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_AddLess_Amount]  DEFAULT ((0)) FOR [AddLess_Amount]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Labour_Charge]  DEFAULT ((0)) FOR [Labour_Charge]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Payment_Terms]  DEFAULT ('') FOR [Payment_Terms]
GO
ALTER TABLE [dbo].[Sales_Enquiry_Head] ADD  CONSTRAINT [DF_Sales_Enquiry_Head_Tax_Type]  DEFAULT ('') FOR [Tax_Type]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_ItemGroup_IdNo]  DEFAULT ((0)) FOR [ItemGroup_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Noof_Items]  DEFAULT ((0)) FOR [Noof_Items]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Bags]  DEFAULT ((0)) FOR [Bags]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Weight_Bag]  DEFAULT ((0)) FOR [Weight_Bag]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Tax_Rate]  DEFAULT ((0)) FOR [Tax_Rate]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Discount_Perc]  DEFAULT ((0)) FOR [Discount_Perc]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Discount_Amount]  DEFAULT ((0)) FOR [Discount_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Tax_Amount]  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Total_Amount]  DEFAULT ((0)) FOR [Total_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Bag_Nos]  DEFAULT ('') FOR [Bag_Nos]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Serial_No]  DEFAULT ('') FOR [Serial_No]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Size_IdNo]  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Meters]  DEFAULT ((0)) FOR [Meters]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Colour_IdNo]  DEFAULT ((0)) FOR [Colour_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Noof_Items_Return]  DEFAULT ((0)) FOR [Noof_Items_Return]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  CONSTRAINT [DF_Sales_Order_Details_Sales_Items]  DEFAULT ((0)) FOR [Sales_Items]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  DEFAULT ('') FOR [Sales_Quotation_Code]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  DEFAULT ((0)) FOR [Sales_Quotation_Detail_SlNo]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  DEFAULT ('') FOR [item_Description]
GO
ALTER TABLE [dbo].[Sales_Order_Details] ADD  DEFAULT ('') FOR [Entry_Type]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Payment_Method]  DEFAULT ('') FOR [Payment_Method]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Cash_PartyName]  DEFAULT ('') FOR [Cash_PartyName]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Party_PhoneNo]  DEFAULT ('') FOR [Party_PhoneNo]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_SalesAc_IdNo]  DEFAULT ((0)) FOR [SalesAc_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Tax_Type]  DEFAULT ('') FOR [Tax_Type]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_TaxAc_IdNo]  DEFAULT ((0)) FOR [TaxAc_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Delivery_Address1]  DEFAULT ('') FOR [Delivery_Address1]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Delivery_Address2]  DEFAULT ('') FOR [Delivery_Address2]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Delivery_Address3]  DEFAULT ('') FOR [Delivery_Address3]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Vehicle_No]  DEFAULT ('') FOR [Vehicle_No]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Narration]  DEFAULT ('') FOR [Narration]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Total_Qty]  DEFAULT ((0)) FOR [Total_Qty]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Total_Bags]  DEFAULT ((0)) FOR [Total_Bags]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_SubTotal_Amount]  DEFAULT ((0)) FOR [SubTotal_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Total_DiscountAmount]  DEFAULT ((0)) FOR [Total_DiscountAmount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Total_TaxAmount]  DEFAULT ((0)) FOR [Total_TaxAmount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Gross_Amount]  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_CashDiscount_Perc]  DEFAULT ((0)) FOR [CashDiscount_Perc]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_CashDiscount_Amount]  DEFAULT ((0)) FOR [CashDiscount_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Assessable_Value]  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Tax_Amount]  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Freight_Amount]  DEFAULT ((0)) FOR [Freight_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_AddLess_Amount]  DEFAULT ((0)) FOR [AddLess_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Document_Through]  DEFAULT ('') FOR [Document_Through]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Despatch_To]  DEFAULT ('') FOR [Despatch_To]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Lr_No]  DEFAULT ('') FOR [Lr_No]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Lr_Date]  DEFAULT ('') FOR [Lr_Date]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Booked_By]  DEFAULT ('') FOR [Booked_By]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Transport_IdNo]  DEFAULT ((0)) FOR [Transport_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Freight_ToPay_Amount]  DEFAULT ((0)) FOR [Freight_ToPay_Amount]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Dc_No]  DEFAULT ('') FOR [Dc_No]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Dc_Date]  DEFAULT ('') FOR [Dc_Date]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Order_No]  DEFAULT ('') FOR [Order_No]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Order_Date]  DEFAULT ('') FOR [Order_Date]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Against_CForm_Status]  DEFAULT ((0)) FOR [Against_CForm_Status]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Entry_Type]  DEFAULT ('') FOR [Entry_Type]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Payment_Terms]  DEFAULT ('') FOR [Payment_Terms]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_OnAc_IdNo]  DEFAULT ((0)) FOR [OnAc_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Extra_Charges]  DEFAULT ((0)) FOR [Extra_Charges]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Total_Extra_Copies]  DEFAULT ((0)) FOR [Total_Extra_Copies]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Sub_Total_Copies]  DEFAULT ((0)) FOR [Sub_Total_Copies]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Party_Name]  DEFAULT ('') FOR [Party_Name]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  CONSTRAINT [DF_Sales_Order_Head_Sales_OrderAc_IdNo]  DEFAULT ((0)) FOR [Sales_OrderAc_IdNo]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  DEFAULT ('') FOR [Sales_Order_Selection_Code]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  DEFAULT ('') FOR [Quotation_No]
GO
ALTER TABLE [dbo].[Sales_Order_Head] ADD  DEFAULT ('') FOR [Quotation_Date]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_ItemGroup_IdNo]  DEFAULT ((0)) FOR [ItemGroup_IdNo]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_Noof_Items]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_Total_Amount1]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  CONSTRAINT [DF_Sales_Quotation_Details_Item_Description]  DEFAULT ('') FOR [Item_Description]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  DEFAULT ((0)) FOR [Cash_Discount_Amount_For_All_Item]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  DEFAULT ((0)) FOR [Cash_Discount_Perc_For_All_Item]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  DEFAULT ('') FOR [HSN_Code]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  DEFAULT ((0)) FOR [GST_Perc]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] ADD  DEFAULT ((0)) FOR [Order_Quantity]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Style_IdNo]  DEFAULT ((0)) FOR [Style_IdNo]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Size_IdNo]  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Assessable_Value]  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_HSN_Code]  DEFAULT ('') FOR [HSN_Code]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Item_Description]  DEFAULT ('') FOR [Item_Description]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_No_Of_Rolls]  DEFAULT ((0)) FOR [No_Of_Rolls]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Delivery_Quantity]  DEFAULT ((0)) FOR [Delivery_Quantity]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  CONSTRAINT [DF_Sales_Receipt_Details_Delivery_No_Of_Rolls]  DEFAULT ((0)) FOR [Delivery_No_Of_Rolls]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  DEFAULT ((0)) FOR [Delivery_Weight]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  DEFAULT ('') FOR [Rate_For]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  DEFAULT ((0)) FOR [ItemGroup_IdNo]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] ADD  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Order_No]  DEFAULT ('') FOR [Order_No]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Order_Date]  DEFAULT ('') FOR [Order_Date]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Total_Qty]  DEFAULT ((0)) FOR [Total_Qty]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Gross_Amount]  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Vehicle_No]  DEFAULT ('') FOR [Vehicle_No]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Transport_IdNo]  DEFAULT ((0)) FOR [Transport_IdNo]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Remarks]  DEFAULT ('') FOR [Remarks]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Assessable_Value]  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Freight_ToPay_Amount]  DEFAULT ((0)) FOR [Freight_ToPay_Amount]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Charge]  DEFAULT ((0)) FOR [Charge]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Lr_No]  DEFAULT ('') FOR [Lr_No]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Lr_Date]  DEFAULT ('') FOR [Lr_Date]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Total_Bags]  DEFAULT ((0)) FOR [Total_Bags]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Electronic_Reference_No]  DEFAULT ('') FOR [Electronic_Reference_No]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Transportation_Mode]  DEFAULT ('') FOR [Transportation_Mode]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_Date_Time_Of_Supply]  DEFAULT ('') FOR [Date_Time_Of_Supply]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_CGst_Amount]  DEFAULT ((0)) FOR [CGst_Amount]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_SGst_Amount]  DEFAULT ((0)) FOR [SGst_Amount]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  CONSTRAINT [DF_Sales_Receipt_Head_IGst_Amount]  DEFAULT ((0)) FOR [IGst_Amount]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  DEFAULT ('') FOR [Challan_No]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  DEFAULT ('') FOR [Challan_Date]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  DEFAULT ((0)) FOR [Total_Weight]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  DEFAULT ((0)) FOR [SubTotal_Amount]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  DEFAULT ('') FOR [Entry_GST_Tax_Type]
GO
ALTER TABLE [dbo].[Sales_Receipt_Head] ADD  DEFAULT ('') FOR [Booked_By]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  CONSTRAINT [DF_CashSalesReturn_Details_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  CONSTRAINT [DF_CashSalesReturn_Details_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  CONSTRAINT [DF_CashSalesReturn_Details_Item_IdNo]  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  CONSTRAINT [DF_CashSalesReturn_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  CONSTRAINT [DF_CashSalesReturn_Details_Noof_Items]  DEFAULT ((0)) FOR [Noof_Items]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  CONSTRAINT [DF_CashSalesReturn_Details_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  CONSTRAINT [DF_CashSalesReturn_Details_Total_Amount1]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  CONSTRAINT [DF_SalesReturn_Details_Serial_No]  DEFAULT ('') FOR [Serial_No]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Sales_Detail_Slno]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ('') FOR [Sales_Code]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Tax_Rate]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Colour_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Design_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Gender_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Sleeve_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Size_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Cash_Discount_Perc_For_All_Item]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Cash_Discount_Amount_For_All_Item]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ('') FOR [HSN_Code]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Return_Qty]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [GST_Percentage]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Details] ADD  DEFAULT ((0)) FOR [net_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_CashSalesReturn_Head_for_OrderBy]  DEFAULT ((0)) FOR [for_OrderBy]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_Payment_Method]  DEFAULT ('') FOR [Payment_Method]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_CashSalesReturn_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_Delivery_Address11]  DEFAULT ('') FOR [Cash_PartyName]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF__SalesRetu__Dc_No__592635D8]  DEFAULT ('') FOR [Bill_No]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_CashSalesReturn_Head_SalesAc_IdNo]  DEFAULT ((0)) FOR [SalesReturnAc_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_Tax_Type]  DEFAULT ('') FOR [Tax_Type]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_SalesAc_IdNo1]  DEFAULT ((0)) FOR [TaxAc_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_Narration]  DEFAULT ('') FOR [Narration]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_CashSalesReturn_Head_Total_Qty]  DEFAULT ((0)) FOR [Total_Qty]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_CashSalesReturn_Head_CashDiscount_Perc1]  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Perc]  DEFAULT ((0)) FOR [CashDiscount_Perc]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Amount]  DEFAULT ((0)) FOR [CashDiscount_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_AddLess_Amount1]  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Perc1]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Amount1]  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_CashSalesReturn_Head_AddLess_Amount]  DEFAULT ((0)) FOR [AddLess_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_CashSalesReturn_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  CONSTRAINT [DF_CashSalesReturn_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Bill_Date]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [Total_DiscountAmount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [Total_TaxAmount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [Freight_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [SubTotal_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [Total_Bags]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [Against_CForm_Status]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Order_No]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Order_Date]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Lr_No]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Lr_Date]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Document_Through]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Booked_By]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Despatch_To]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [SalesAc_IdNo]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Entry_VAT_GST_Type]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Electronic_Reference_No]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Transportation_Mode]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Date_Time_Of_Supply]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Entry_GST_Tax_Type]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [CGst_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [SGst_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ((0)) FOR [IGst_Amount]
GO
ALTER TABLE [dbo].[SalesReturn_Head] ADD  DEFAULT ('') FOR [Sales_Order_Selection_Code]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Details] ADD  CONSTRAINT [DF_Spinning_WasteSales_Details_Item_IdNo]  DEFAULT ((0)) FOR [Waste_IdNo]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Details] ADD  CONSTRAINT [DF_Spinning_WasteSales_Details_Unit_IdNo]  DEFAULT ((0)) FOR [Unit_IdNo]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Details] ADD  CONSTRAINT [DF_Spinning_WasteSales_Details_Packs]  DEFAULT ((0)) FOR [Packs]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Details] ADD  CONSTRAINT [DF_Spinning_WasteSales_Details_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Details] ADD  CONSTRAINT [DF_Spinning_WasteSales_Details_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Details] ADD  CONSTRAINT [DF_Spinning_WasteSales_Details_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Details] ADD  CONSTRAINT [DF_Spinning_WasteSales_Details_Bag_Nos]  DEFAULT ('') FOR [Pack_Nos]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Ledger_IdNo]  DEFAULT ((0)) FOR [Ledger_IdNo]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_SalesAc_IdNo]  DEFAULT ((0)) FOR [SalesAc_IdNo]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_TaxAc_IdNo]  DEFAULT ((0)) FOR [TaxAc_IdNo]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_CessAc_IdNo]  DEFAULT ((0)) FOR [CessAc_IdNo]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address1]  DEFAULT ('') FOR [Delivery_Address1]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address2]  DEFAULT ('') FOR [Delivery_Address2]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address3]  DEFAULT ('') FOR [Delivery_Address3]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Vehicle_No]  DEFAULT ('') FOR [Vehicle_No]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Removal_Date]  DEFAULT ('') FOR [Removal_Date]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Bag_Nos]  DEFAULT ('') FOR [Pack_Nos]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Total_Packs]  DEFAULT ((0)) FOR [Total_Packs]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Total_Weight]  DEFAULT ((0)) FOR [Total_Weight]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_GrossAmount]  DEFAULT ((0)) FOR [Gross_Amount]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_CashDiscount_Perc]  DEFAULT ((0)) FOR [CashDiscount_Perc]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_CashDiscount_Amount]  DEFAULT ((0)) FOR [CashDiscount_Amount]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Assessable_Value]  DEFAULT ((0)) FOR [Assessable_Value]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Tax_Perc]  DEFAULT ((0)) FOR [Tax_Perc]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Tax_Amount]  DEFAULT ((0)) FOR [Tax_Amount]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Cess_Perc]  DEFAULT ((0)) FOR [Cess_Perc]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Cess_Amount]  DEFAULT ((0)) FOR [Cess_Amount]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_AddLess_Amount]  DEFAULT ((0)) FOR [AddLess_Amount]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Round_Off]  DEFAULT ((0)) FOR [Round_Off]
GO
ALTER TABLE [dbo].[Spinning_WasteSales_Head] ADD  CONSTRAINT [DF_Spinning_WasteSales_Head_Net_Amount]  DEFAULT ((0)) FOR [Net_Amount]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('') FOR [ledger_name]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('') FOR [ledger_address1]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('') FOR [ledger_address2]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('') FOR [ledger_address3]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('') FOR [ledger_address4]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('') FOR [ledger_phoneno]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('') FOR [ledger_tinno]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ((0)) FOR [ledger_idno]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('DATA') FOR [status_for_row]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ((0)) FOR [auto_slno_for_orderby_group]
GO
ALTER TABLE [dbo].[temp_report] ADD  DEFAULT ('') FOR [row_data]
GO
ALTER TABLE [dbo].[TempTable_For_NegativeStock] ADD  DEFAULT ('') FOR [Reference_Code]
GO
ALTER TABLE [dbo].[TempTable_For_NegativeStock] ADD  DEFAULT ((0)) FOR [Company_Idno]
GO
ALTER TABLE [dbo].[TempTable_For_NegativeStock] ADD  DEFAULT ((0)) FOR [Item_IdNo]
GO
ALTER TABLE [dbo].[TempTable_For_NegativeStock] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Tocken_Head] ADD  DEFAULT ('') FOR [Vehicle_Type]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details]  WITH CHECK ADD  CONSTRAINT [CK_Item_Stock_Selection_Processing_Details] CHECK  (([Inward_Quantity]>=(0)))
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] CHECK CONSTRAINT [CK_Item_Stock_Selection_Processing_Details]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details]  WITH CHECK ADD  CONSTRAINT [CK_Item_Stock_Selection_Processing_Details_1] CHECK  (([outward_Quantity]>=(0)))
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] CHECK CONSTRAINT [CK_Item_Stock_Selection_Processing_Details_1]
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details]  WITH CHECK ADD  CONSTRAINT [CK_Item_Stock_Selection_Processing_Details_3] CHECK  (([Inward_Quantity]>=[Outward_Quantity]))
GO
ALTER TABLE [dbo].[Item_Stock_Selection_Processing_Details] CHECK CONSTRAINT [CK_Item_Stock_Selection_Processing_Details_3]
GO
ALTER TABLE [dbo].[Sales_Delivery_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Delivery_Details_1] CHECK  (([Receipt_Quantity]>=(0)))
GO
ALTER TABLE [dbo].[Sales_Delivery_Details] CHECK CONSTRAINT [CK_Sales_Delivery_Details_1]
GO
ALTER TABLE [dbo].[Sales_Delivery_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Delivery_Details_2] CHECK  (([Quantity]>=[Receipt_Quantity]))
GO
ALTER TABLE [dbo].[Sales_Delivery_Details] CHECK CONSTRAINT [CK_Sales_Delivery_Details_2]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Quotation_Details_1] CHECK  (([Order_Quantity]>=(0)))
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] CHECK CONSTRAINT [CK_Sales_Quotation_Details_1]
GO
ALTER TABLE [dbo].[Sales_Quotation_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Quotation_Details_2] CHECK  (([Quantity]>=[Order_Quantity]))
GO
ALTER TABLE [dbo].[Sales_Quotation_Details] CHECK CONSTRAINT [CK_Sales_Quotation_Details_2]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Receipt_Details_1] CHECK  (([Delivery_Quantity]>=(0)))
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] CHECK CONSTRAINT [CK_Sales_Receipt_Details_1]
GO
ALTER TABLE [dbo].[Sales_Receipt_Details]  WITH CHECK ADD  CONSTRAINT [CK_Sales_Receipt_Details_2] CHECK  (([Quantity]>=[Delivery_Quantity]))
GO
ALTER TABLE [dbo].[Sales_Receipt_Details] CHECK CONSTRAINT [CK_Sales_Receipt_Details_2]
GO
USE [master]
GO
ALTER DATABASE [tsoft_billing_1] SET  READ_WRITE 
GO
