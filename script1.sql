SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AccountsGroup_Head](
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
 CONSTRAINT [PK_AccountsGroup_Head] PRIMARY KEY CLUSTERED 
(
	[AccountsGroup_IdNo] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Area_Head](
	[Area_IdNo] [smallint] NOT NULL,
	[Area_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Area_Head] PRIMARY KEY CLUSTERED 
(
	[Area_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Area_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cloth_Sales_Head](
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
	[Discount_Percentage] [numeric](18, 3) NULL DEFAULT ((0)),
	[Discount_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[net_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Bale_Nos] [varchar](100) NULL DEFAULT (''),
	[Agent_IdNo] [int] NULL DEFAULT ((0)),
	[Add_Less] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_Cloth_Sales_Head] PRIMARY KEY NONCLUSTERED 
(
	[Cloth_Sales_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Colour_Head](
	[Colour_IdNo] [int] NOT NULL,
	[Colour_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Colour_Head] PRIMARY KEY CLUSTERED 
(
	[Colour_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Colour_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Company_Head](
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
 CONSTRAINT [PK_Company_Head] PRIMARY KEY CLUSTERED 
(
	[Company_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [Duplicate_CompanyHead_Name] UNIQUE NONCLUSTERED 
(
	[Company_Name] ASC
) ON [PRIMARY],
 CONSTRAINT [Duplicate_CompanyHead_ShortName] UNIQUE NONCLUSTERED 
(
	[Company_ShortName] ASC
) ON [PRIMARY],
 CONSTRAINT [Duplicate_CompanyHead_SurName] UNIQUE NONCLUSTERED 
(
	[Company_SurName] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Delivery_Details](
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
	[Actual_Weight] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_Delivery_Details] PRIMARY KEY CLUSTERED 
(
	[Delivery_Code] ASC,
	[SL_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Delivery_Head](
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
	[Total_Actual_Weight] [numeric](18, 2) NULL DEFAULT ((0)),
	[Invoice_Code] [varchar](50) NULL DEFAULT (''),
	[NoOf_Bundle] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_Delivery_Head] PRIMARY KEY CLUSTERED 
(
	[Delivery_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Design_Head](
	[Design_IdNo] [int] NOT NULL,
	[Design_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Design_Head] PRIMARY KEY CLUSTERED 
(
	[Design_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Design_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EntryTemp](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EntryTempSub](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FinancialRange_Head](
	[Financial_Range] [varchar](20) NOT NULL,
 CONSTRAINT [PK_FinancialRange_Head] PRIMARY KEY CLUSTERED 
(
	[Financial_Range] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Gender_Head](
	[Gender_IdNo] [int] NOT NULL,
	[Gender_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Gender_Head] PRIMARY KEY CLUSTERED 
(
	[Gender_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Gender_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Item_ExcessShort_Head](
	[Item_ExcessShort_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Item_ExcessShort_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Item_ExcessShort_Date] [smalldatetime] NOT NULL,
	[Item_IdNo] [smallint] NOT NULL,
	[Unit_IdNo] [smallint] NULL,
	[ExcessShort_Type] [varchar](50) NULL,
	[Quantity] [numeric](18, 3) NULL CONSTRAINT [DF_Item_ExcessShort_Head_Quantity]  DEFAULT ((0)),
 CONSTRAINT [PK_Item_ExcessShort_Head] PRIMARY KEY CLUSTERED 
(
	[Item_ExcessShort_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_Item_ExcessShort_Head] ON [Item_ExcessShort_Head] 
(
	[Company_IdNo] ASC,
	[Item_ExcessShort_No] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Item_Head](
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
 CONSTRAINT [PK_Item_Head] PRIMARY KEY CLUSTERED 
(
	[Item_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Item_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Item_Processing_Details](
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
 CONSTRAINT [PK_Item_Processing_Details] PRIMARY KEY CLUSTERED 
(
	[Reference_Code] ASC,
	[Sl_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_Item_Processing_Details] ON [Item_Processing_Details] 
(
	[Company_IdNo] ASC,
	[Reference_No] ASC,
	[Sl_No] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ItemGroup_Head](
	[ItemGroup_IdNo] [smallint] NOT NULL,
	[ItemGroup_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Commodity_Code] [varchar](20) NULL DEFAULT (''),
 CONSTRAINT [PK_ItemGroup_Head] PRIMARY KEY CLUSTERED 
(
	[ItemGroup_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_ItemGroup_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [JobWork_Head](
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
	[Sales_Code] [varchar](50) NULL DEFAULT (''),
	[JobWork_Image] [image] NULL,
 CONSTRAINT [PK_JobWork_Head] PRIMARY KEY CLUSTERED 
(
	[JobWork_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Knotting_Bill_Details](
	[Knotting_Bill_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Knotting_Bill_No] [varchar](30) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Knotting_Bill_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Knotting_Date] [smalldatetime] NULL,
	[Knotting_No] [varchar](20) NULL CONSTRAINT [DF_Knotting_Bill_Details_Knotting_No]  DEFAULT (''),
	[Shift] [varchar](20) NULL CONSTRAINT [DF_Knotting_Bill_Details_Shift]  DEFAULT (''),
	[Ends] [int] NULL CONSTRAINT [DF_Knotting_Bill_Details_Ends]  DEFAULT ((0)),
	[Loom] [varchar](200) NULL CONSTRAINT [DF_Knotting_Bill_Details_Loom]  DEFAULT (''),
	[No_Pavu] [int] NULL CONSTRAINT [DF_Knotting_Bill_Details_No_Pavu]  DEFAULT ((0)),
	[Knotting_Code] [varchar](50) NULL CONSTRAINT [DF_Knotting_Bill_Details_Knotting_Code]  DEFAULT (''),
 CONSTRAINT [PK_Knotting_Bill_Details] PRIMARY KEY CLUSTERED 
(
	[Knotting_Bill_Code] ASC,
	[Sl_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Knotting_Bill_Head](
	[Auto_BillNo] [int] IDENTITY(1,1) NOT NULL,
	[Knotting_Bill_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Knotting_Bill_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Knotting_Bill_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[Entry_Type] [varchar](30) NULL CONSTRAINT [DF_Knotting_Bill_Head_Entry_Type]  DEFAULT (''),
	[Total_Pavu] [int] NULL CONSTRAINT [DF_Knotting_Bill_Head_Total_Pavu]  DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_Rate]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_Gross_Amount]  DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_AddLess_Amount]  DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_Round_Off]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Knotting_Bill_Head_Net_Amount]  DEFAULT ((0)),
 CONSTRAINT [PK_Knotting_Bill_Head] PRIMARY KEY CLUSTERED 
(
	[Knotting_Bill_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Knotting_Head](
	[Knotting_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Knotting_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Knotting_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Knotting_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Shift] [varchar](20) NULL CONSTRAINT [DF_Knotting_Head_Shift]  DEFAULT (''),
	[Loom] [varchar](100) NULL CONSTRAINT [DF_Knotting_Head_Loom]  DEFAULT (''),
	[Ends] [int] NULL CONSTRAINT [DF_Knotting_Head_Ends]  DEFAULT ((0)),
	[No_Pavu] [int] NULL CONSTRAINT [DF_Knotting_Head_No_Pavu]  DEFAULT ((0)),
	[Knotting_Bill_Code] [varchar](50) NULL CONSTRAINT [DF_Knotting_Head_Knotting_Bill_Code]  DEFAULT (''),
	[Knotting_IdNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Knotting_Head] PRIMARY KEY CLUSTERED 
(
	[Knotting_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Ledger_AlaisHead](
	[Ledger_IdNo] [smallint] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Ledger_DisplayName] [varchar](200) NOT NULL,
	[Ledger_Type] [varchar](35) NULL DEFAULT (''),
	[AccountsGroup_IdNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Ledger_AlaisHead] PRIMARY KEY CLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Sl_No] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_AlaisHead] UNIQUE NONCLUSTERED 
(
	[Ledger_DisplayName] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Ledger_Head](
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
 CONSTRAINT [PK_Ledger_Head] PRIMARY KEY CLUSTERED 
(
	[Ledger_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Ledger_PhoneNo_Head](
	[Ledger_IdNo] [smallint] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Ledger_PhoneNo] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Ledger_PhoneNo_Head] PRIMARY KEY CLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Sl_No] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_PhoneNo_Head] UNIQUE NONCLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Ledger_PhoneNo] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Ledger_Reading_Details](
	[Ledger_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Machine_IdNo] [int] NOT NULL,
	[Opening_Reading] [int] NULL,
 CONSTRAINT [PK_Ledger_Reading_Details] PRIMARY KEY NONCLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Sl_No] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Ledger_Reading_Details] UNIQUE NONCLUSTERED 
(
	[Ledger_IdNo] ASC,
	[Machine_IdNo] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Machine_Head](
	[Machine_IdNo] [int] NOT NULL,
	[Machine_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Description] [varchar](100) NULL DEFAULT (''),
 CONSTRAINT [PK_Machine_Head] PRIMARY KEY CLUSTERED 
(
	[Machine_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Machine_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Month_Head](
	[Month_IdNo] [tinyint] NOT NULL,
	[Month_Name] [varchar](30) NOT NULL,
	[Month_ShortName] [varchar](20) NOT NULL,
	[Idno] [tinyint] NOT NULL,
 CONSTRAINT [PK_Month_Head] PRIMARY KEY NONCLUSTERED 
(
	[Month_IdNo] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Price_List_Details](
	[Price_List_IdNo] [int] NOT NULL,
	[Sl_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL,
	[Rate] [numeric](18, 3) NULL,
 CONSTRAINT [PK_Price_List_Details] PRIMARY KEY NONCLUSTERED 
(
	[Price_List_IdNo] ASC,
	[Sl_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Price_List_Head](
	[Price_List_IdNo] [int] NOT NULL,
	[Price_List_Name] [varchar](50) NOT NULL,
	[sur_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Price_List_Head] PRIMARY KEY NONCLUSTERED 
(
	[Price_List_IdNo] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Purchase_Details](
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
	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Purhase_Details_Rate]  DEFAULT ((0)),
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
 CONSTRAINT [PK_Purhase_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Code] ASC,
	[SL_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Purchase_Head](
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
 CONSTRAINT [PK_Purhase_Head] PRIMARY KEY CLUSTERED 
(
	[Purchase_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_Purhase_Head] ON [Purchase_Head] 
(
	[Purchase_Date] ASC,
	[for_OrderBy] ASC,
	[Purchase_No] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Purchase_Order_Details](
	[Purchase_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_Order_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Purchase_Order_Details_for_OrderBy]  DEFAULT ((0)),
	[Purchase_Order_Date] [datetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Purchase_Order_Details_Ledger_IdNo]  DEFAULT ((0)),
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Details_Item_IdNo]  DEFAULT ((0)),
	[ItemGroup_IdNo] [smallint] NULL CONSTRAINT [DF_Purchase_Order_Details_ItemGroup_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Purchase_Order_Details_Unit_IdNo]  DEFAULT ((0)),
	[Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Details_Noof_Items]  DEFAULT ((0)),
	[Bags] [int] NULL CONSTRAINT [DF_Purchase_Order_Details_Bags]  DEFAULT ((0)),
	[Weight_Bag] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Details_Weight_Bag]  DEFAULT ((0)),
	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Details_Weight]  DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Rate]  DEFAULT ((0)),
	[Tax_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Tax_Rate]  DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Amount]  DEFAULT ((0)),
	[Discount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Discount_Perc]  DEFAULT ((0)),
	[Discount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Discount_Amount]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Tax_Amount]  DEFAULT ((0)),
	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Total_Amount]  DEFAULT ((0)),
	[Bag_Nos] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Details_Bag_Nos]  DEFAULT (''),
	[Serial_No] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Details_Serial_No]  DEFAULT (''),
	[Size_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Details_Size_IdNo]  DEFAULT ((0)),
	[Meters] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Meters]  DEFAULT ((0)),
	[Colour_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Details_Colour_IdNo]  DEFAULT ((0)),
	[Noof_Items_Return] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Details_Noof_Items_Return]  DEFAULT ((0)),
	[Purchase_Order_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Purchase_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Table_1_Sales_Items]  DEFAULT ((0)),
 CONSTRAINT [PK_Purchase_Order_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Order_Code] ASC,
	[SL_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Purchase_Order_Head](
	[Purchase_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_Order_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Purchase_Order_Head_for_OrderBy]  DEFAULT ((0)),
	[Purchase_Order_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL CONSTRAINT [DF_Purchase_Order_Head_Payment_Method]  DEFAULT (''),
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Cash_PartyName] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Cash_PartyName]  DEFAULT (''),
	[Party_PhoneNo] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Party_PhoneNo]  DEFAULT (''),
	[PurchaseAc_IdNo] [int] NULL CONSTRAINT [DF_Table_1_SalesAc_IdNo]  DEFAULT ((0)),
	[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_Purchase_Order_Head_Tax_Type]  DEFAULT (''),
	[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_TaxAc_IdNo]  DEFAULT ((0)),
	[Delivery_Address1] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address1]  DEFAULT (''),
	[Delivery_Address2] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address2]  DEFAULT (''),
	[Delivery_Address3] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Delivery_Address3]  DEFAULT (''),
	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Vehicle_No]  DEFAULT (''),
	[Narration] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Head_Narration]  DEFAULT (''),
	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Head_Total_Qty]  DEFAULT ((0)),
	[Total_Bags] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_Total_Bags]  DEFAULT ((0)),
	[SubTotal_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_SubTotal_Amount]  DEFAULT ((0)),
	[Total_DiscountAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Total_DiscountAmount]  DEFAULT ((0)),
	[Total_TaxAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Total_TaxAmount]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Gross_Amount]  DEFAULT ((0)),
	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_CashDiscount_Perc]  DEFAULT ((0)),
	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_CashDiscount_Amount]  DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Assessable_Value]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Tax_Amount]  DEFAULT ((0)),
	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Freight_Amount]  DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_AddLess_Amount]  DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Round_Off]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Net_Amount]  DEFAULT ((0)),
	[Document_Through] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Document_Through]  DEFAULT (''),
	[Despatch_To] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Despatch_To]  DEFAULT (''),
	[Lr_No] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Head_Lr_No]  DEFAULT (''),
	[Lr_Date] [varchar](500) NULL CONSTRAINT [DF_Purchase_Order_Head_Lr_Date]  DEFAULT (''),
	[Booked_By] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Booked_By]  DEFAULT (''),
	[Transport_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_Transport_IdNo]  DEFAULT ((0)),
	[Freight_ToPay_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Freight_ToPay_Amount]  DEFAULT ((0)),
	[Dc_No] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Dc_No]  DEFAULT (''),
	[Dc_Date] [varchar](35) NULL CONSTRAINT [DF_Purchase_Order_Head_Dc_Date]  DEFAULT (''),
	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Order_No]  DEFAULT (''),
	[Order_Date] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Order_Date]  DEFAULT (''),
	[Against_CForm_Status] [tinyint] NULL CONSTRAINT [DF_Purchase_Order_Head_Against_CForm_Status]  DEFAULT ((0)),
	[Entry_Type] [varchar](20) NULL CONSTRAINT [DF_Purchase_Order_Head_Entry_Type]  DEFAULT (''),
	[Payment_Terms] [varchar](100) NULL CONSTRAINT [DF_Purchase_Order_Head_Payment_Terms]  DEFAULT (''),
	[OnAc_IdNo] [int] NULL CONSTRAINT [DF_Purchase_Order_Head_OnAc_IdNo]  DEFAULT ((0)),
	[Extra_Charges] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Extra_Charges]  DEFAULT ((0)),
	[Total_Extra_Copies] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Total_Extra_Copies]  DEFAULT ((0)),
	[Sub_Total_Copies] [numeric](18, 2) NULL CONSTRAINT [DF_Purchase_Order_Head_Sub_Total_Copies]  DEFAULT ((0)),
	[Party_Name] [varchar](50) NULL CONSTRAINT [DF_Purchase_Order_Head_Party_Name]  DEFAULT (''),
	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Purchase_Order_Head_Weight]  DEFAULT ((0)),
	[Purchase_OrderAc_IdNo] [int] NULL CONSTRAINT [DF_Table_1_Sales_OrderAc_IdNo]  DEFAULT ((0)),
 CONSTRAINT [PK_Purchase_Order_Head] PRIMARY KEY CLUSTERED 
(
	[Purchase_Order_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Purchase_Return_Details](
	[Purchase_Return_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_Return_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Return_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL DEFAULT ((0)),
	[Noof_Items] [numeric](18, 3) NULL DEFAULT ((0)),
	[Bales] [int] NULL DEFAULT ((0)),
	[Weight] [numeric](18, 3) NULL DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL DEFAULT ((0)),
	[Tax_Rate] [numeric](18, 2) NULL DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Discount_Perc] [numeric](18, 2) NULL DEFAULT ((0)),
	[Discount_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Bale_Nos] [varchar](500) NULL DEFAULT (''),
	[TaxAmount_Difference] [numeric](18, 2) NULL DEFAULT ((0)),
	[Size_IdNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Purhase_Return_Details] PRIMARY KEY CLUSTERED 
(
	[Purchase_Return_Code] ASC,
	[SL_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Purchase_Return_Head](
	[Purchase_Return_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Purchase_Return_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Purchase_Return_Date] [smalldatetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL DEFAULT (''),
	[Ledger_IdNo] [int] NULL DEFAULT ((0)),
	[Bill_No] [varchar](20) NULL DEFAULT (''),
	[PurchaseAc_IdNo] [int] NULL DEFAULT ((0)),
	[Tax_Type] [varchar](20) NULL DEFAULT (''),
	[TaxAc_IdNo] [int] NULL DEFAULT ((0)),
	[Narration] [varchar](1000) NULL DEFAULT (''),
	[Vehicle_No] [varchar](50) NULL DEFAULT (''),
	[Total_Qty] [numeric](18, 3) NULL DEFAULT ((0)),
	[Total_Bags] [int] NULL DEFAULT ((0)),
	[Total_Weight] [numeric](18, 3) NULL DEFAULT ((0)),
	[SubTotal_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_DiscountAmount] [numeric](18, 2) NULL,
	[Total_TaxAmount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[CashDiscount_Perc] [numeric](18, 2) NULL DEFAULT ((0)),
	[CashDiscount_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[AddLess_BeforeTax_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL DEFAULT ((0)),
	[Freight_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Bale_Nos] [varchar](500) NULL DEFAULT (''),
 CONSTRAINT [PK_Purhase_Return_Head] PRIMARY KEY CLUSTERED 
(
	[Purchase_Return_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [report_settings](
	[report_code] [varchar](100) NOT NULL,
	[font_size] [tinyint] NULL CONSTRAINT [DF_report_settings_font_size]  DEFAULT ((10)),
	[paper_size] [tinyint] NULL CONSTRAINT [DF_report_settings_paper_size]  DEFAULT ((0)),
	[paper_orientation] [tinyint] NULL CONSTRAINT [DF_report_settings_paper_orientation]  DEFAULT ((0)),
	[print_mode] [tinyint] NULL CONSTRAINT [DF_report_settings_print_mode]  DEFAULT ((0)),
	[horizontal_line] [tinyint] NULL CONSTRAINT [DF_report_settings_horizontal_line]  DEFAULT ((0))
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [report_settings_column_size](
	[report_code] [varchar](100) NOT NULL,
	[field_name] [varchar](100) NOT NULL,
	[noof_characters] [smallint] NOT NULL CONSTRAINT [DF_report_settings_column_size_noof_characters]  DEFAULT ((0))
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ReportTemp](
	[Name1] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name1]  DEFAULT (''),
	[Name2] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name2]  DEFAULT (''),
	[Name3] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name3]  DEFAULT (''),
	[Name4] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name4]  DEFAULT (''),
	[Name5] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name5]  DEFAULT (''),
	[Name6] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name6]  DEFAULT (''),
	[name7] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_name7]  DEFAULT (''),
	[Name8] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name8]  DEFAULT (''),
	[Name9] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name9]  DEFAULT (''),
	[Name10] [varchar](100) NULL CONSTRAINT [DF_ReportTemp_Name10]  DEFAULT (''),
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
	[Company_Address2] [varchar](300) NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ReportTempSub](
	[Name1] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name1]  DEFAULT (''),
	[Name2] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name2]  DEFAULT (''),
	[Name3] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name3]  DEFAULT (''),
	[Name4] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name4]  DEFAULT (''),
	[Name5] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name5]  DEFAULT (''),
	[Name6] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name6]  DEFAULT (''),
	[name7] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_name7]  DEFAULT (''),
	[Name8] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name8]  DEFAULT (''),
	[Name9] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name9]  DEFAULT (''),
	[Name10] [varchar](100) NULL CONSTRAINT [DF_ReportTempSub_Name10]  DEFAULT (''),
	[Date1] [smalldatetime] NULL,
	[Date2] [smalldatetime] NULL,
	[Date3] [smalldatetime] NULL,
	[Date4] [smalldatetime] NULL,
	[Date5] [smalldatetime] NULL,
	[Int1] [int] NULL CONSTRAINT [DF_ReportTempSub_Int1]  DEFAULT ((0)),
	[Int2] [int] NULL CONSTRAINT [DF_ReportTempSub_Int2]  DEFAULT ((0)),
	[Int3] [int] NULL CONSTRAINT [DF_ReportTempSub_Int3]  DEFAULT ((0)),
	[Int4] [int] NULL CONSTRAINT [DF_ReportTempSub_Int4]  DEFAULT ((0)),
	[Int5] [int] NULL CONSTRAINT [DF_ReportTempSub_Int5]  DEFAULT ((0)),
	[Int6] [int] NULL CONSTRAINT [DF_ReportTempSub_Int6]  DEFAULT ((0)),
	[Int7] [int] NULL CONSTRAINT [DF_ReportTempSub_Int7]  DEFAULT ((0)),
	[Int8] [int] NULL CONSTRAINT [DF_ReportTempSub_Int8]  DEFAULT ((0)),
	[Int9] [int] NULL CONSTRAINT [DF_ReportTempSub_Int9]  DEFAULT ((0)),
	[Int10] [int] NULL CONSTRAINT [DF_ReportTempSub_Int10]  DEFAULT ((0)),
	[Meters1] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters1]  DEFAULT ((0)),
	[Meters2] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters2]  DEFAULT ((0)),
	[Meters3] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters3]  DEFAULT ((0)),
	[Meters4] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters4]  DEFAULT ((0)),
	[Meters5] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters5]  DEFAULT ((0)),
	[Meters6] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters6]  DEFAULT ((0)),
	[Meters7] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters7]  DEFAULT ((0)),
	[Meters8] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters8]  DEFAULT ((0)),
	[Meters9] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters9]  DEFAULT ((0)),
	[Meters10] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters102]  DEFAULT ((0)),
	[Meters11] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters101]  DEFAULT ((0)),
	[Meters12] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Meters10]  DEFAULT ((0)),
	[Weight1] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight1]  DEFAULT ((0)),
	[Weight2] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight2]  DEFAULT ((0)),
	[Weight3] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight3]  DEFAULT ((0)),
	[Weight4] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight4]  DEFAULT ((0)),
	[Weight5] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight5]  DEFAULT ((0)),
	[Weight6] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight6]  DEFAULT ((0)),
	[Weight7] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight7]  DEFAULT ((0)),
	[Weight8] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight8]  DEFAULT ((0)),
	[Weight9] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight9]  DEFAULT ((0)),
	[Weight10] [numeric](18, 3) NULL CONSTRAINT [DF_ReportTempSub_Weight10]  DEFAULT ((0)),
	[Currency1] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Currency1]  DEFAULT ((0)),
	[Currency2] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Currency2]  DEFAULT ((0)),
	[Currency3] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Currency3]  DEFAULT ((0)),
	[Currency4] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Currency4]  DEFAULT ((0)),
	[Currency5] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Currency5]  DEFAULT ((0)),
	[Currency6] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Currency6]  DEFAULT ((0)),
	[Currency7] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Currency7]  DEFAULT ((0)),
	[Currency8] [numeric](18, 2) NULL CONSTRAINT [DF_ReportTempSub_Currency8]  DEFAULT ((0)),
	[Currency9] [numeric](18, 7) NULL CONSTRAINT [DF_ReportTempSub_Currency9]  DEFAULT ((0)),
	[Currency10] [numeric](18, 7) NULL CONSTRAINT [DF_ReportTempSub_Currency10]  DEFAULT ((0)),
	[Currency11] [numeric](18, 7) NULL CONSTRAINT [DF_ReportTempSub_Currency11]  DEFAULT ((0)),
	[Currency12] [numeric](18, 7) NULL CONSTRAINT [DF_ReportTempSub_Currency12]  DEFAULT ((0)),
	[Report_Heading1] [varchar](250) NULL,
	[Report_Heading2] [varchar](250) NULL,
	[Report_Heading3] [varchar](250) NULL,
	[Company_Name] [varchar](100) NULL,
	[Company_Address1] [varchar](300) NULL,
	[Company_Address2] [varchar](300) NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales_Details](
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
	[JobWork_No] [varchar](50) NULL DEFAULT (''),
	[JobWork_Code] [varchar](50) NULL DEFAULT (''),
	[JobWork_Date] [varchar](20) NULL DEFAULT (''),
	[Rate_Sqft] [numeric](18, 2) NULL DEFAULT ((0)),
	[GSM] [numeric](18, 2) NULL DEFAULT ((0)),
	[Rolls] [numeric](18, 2) NULL DEFAULT ((0)),
	[Weight_Roll] [numeric](18, 3) NULL DEFAULT ((0)),
	[Meters] [numeric](18, 2) NULL DEFAULT ((0)),
	[Colour_IdNo] [int] NULL DEFAULT ((0)),
	[Item_code] [varchar](100) NULL DEFAULT (''),
	[Entry_Type] [varchar](50) NULL DEFAULT (''),
	[Sales_Order_Code] [varchar](50) NULL DEFAULT (''),
	[Sales_Order_Detail_SlNo] [int] NULL DEFAULT ((0)),
	[Noof_Items_Return] [numeric](18, 2) NULL DEFAULT ((0)),
	[Sales_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Design_IdNo] [int] NULL DEFAULT ((0)),
	[Gender_IdNo] [int] NULL DEFAULT ((0)),
	[Sleeve_IdNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Sales_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Code] ASC,
	[SL_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales_Head](
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
	[Document_Through] [varchar](35) NULL DEFAULT (''),
	[Despatch_To] [varchar](35) NULL DEFAULT (''),
	[Lr_No] [varchar](35) NULL DEFAULT (''),
	[Lr_Date] [varchar](20) NULL DEFAULT (''),
	[Booked_By] [varchar](35) NULL DEFAULT (''),
	[Transport_IdNo] [int] NULL DEFAULT ((0)),
	[Freight_ToPay_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Dc_No] [varchar](35) NULL DEFAULT (''),
	[Dc_Date] [varchar](35) NULL DEFAULT (''),
	[Ro_Division_Status] [tinyint] NULL DEFAULT ((0)),
	[Charging_Quantity] [numeric](18, 2) NULL DEFAULT ((0)),
	[Charging_Rate] [numeric](18, 2) NULL DEFAULT ((0)),
	[Order_No] [varchar](50) NULL DEFAULT (''),
	[Order_Date] [varchar](50) NULL DEFAULT (''),
	[Against_CForm_Status] [tinyint] NULL DEFAULT ((0)),
	[Weight] [numeric](18, 3) NULL DEFAULT ((0)),
	[Entry_Type] [varchar](20) NULL DEFAULT (''),
	[Payment_Terms] [varchar](100) NULL DEFAULT (''),
	[Total_Rolls] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_Meters] [numeric](18, 2) NULL DEFAULT ((0)),
	[Branch_Transfer_Status] [tinyint] NULL DEFAULT ((0)),
	[OnAc_IdNo] [int] NULL DEFAULT ((0)),
	[Rate_Extra_Copy] [numeric](18, 2) NULL DEFAULT ((0)),
	[Rent_Machine] [numeric](18, 2) NULL DEFAULT ((0)),
	[Free_Copies_Machine] [int] NULL DEFAULT ((0)),
	[Total_Copies] [int] NULL DEFAULT ((0)),
	[Total_Free_Copies] [int] NULL DEFAULT ((0)),
	[Additional_Copies] [int] NULL DEFAULT ((0)),
	[Rent] [numeric](18, 2) NULL DEFAULT ((0)),
	[Extra_Charges] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_Extra_Copies] [numeric](18, 2) NULL DEFAULT ((0)),
	[Sub_Total_Copies] [numeric](18, 2) NULL DEFAULT ((0)),
	[Opening_Date] [smalldatetime] NULL DEFAULT ((0)),
	[Closing_Date] [smalldatetime] NULL DEFAULT ((0)),
	[Total_Machine] [int] NULL DEFAULT ((0)),
	[Delivery_Code] [varchar](50) NULL DEFAULT (''),
	[Selection_Type] [varchar](50) NULL DEFAULT (''),
	[Party_Name] [varchar](50) NULL DEFAULT (''),
	[Labour_Charge] [int] NULL DEFAULT ((0)),
	[NoOf_Bundle] [varchar](50) NULL DEFAULT (''),
 CONSTRAINT [PK_Sales_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales_Order_Details](
	[Sales_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Order_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Order_Details_for_OrderBy]  DEFAULT ((0)),
	[Sales_Order_Date] [datetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_Sales_Order_Details_Ledger_IdNo]  DEFAULT ((0)),
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Details_Item_IdNo]  DEFAULT ((0)),
	[ItemGroup_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Order_Details_ItemGroup_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Sales_Order_Details_Unit_IdNo]  DEFAULT ((0)),
	[Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Details_Noof_Items]  DEFAULT ((0)),
	[Bags] [int] NULL CONSTRAINT [DF_Sales_Order_Details_Bags]  DEFAULT ((0)),
	[Weight_Bag] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Details_Weight_Bag]  DEFAULT ((0)),
	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Details_Weight]  DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Rate]  DEFAULT ((0)),
	[Tax_Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Tax_Rate]  DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Amount]  DEFAULT ((0)),
	[Discount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Discount_Perc]  DEFAULT ((0)),
	[Discount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Discount_Amount]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Tax_Amount]  DEFAULT ((0)),
	[Total_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Total_Amount]  DEFAULT ((0)),
	[Bag_Nos] [varchar](500) NULL CONSTRAINT [DF_Sales_Order_Details_Bag_Nos]  DEFAULT (''),
	[Serial_No] [varchar](500) NULL CONSTRAINT [DF_Sales_Order_Details_Serial_No]  DEFAULT (''),
	[Size_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Details_Size_IdNo]  DEFAULT ((0)),
	[Meters] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Meters]  DEFAULT ((0)),
	[Colour_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Details_Colour_IdNo]  DEFAULT ((0)),
	[Noof_Items_Return] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Details_Noof_Items_Return]  DEFAULT ((0)),
	[Sales_Order_Detail_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[Sales_Items] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Details_Sales_Items]  DEFAULT ((0)),
 CONSTRAINT [PK_Sales_Order_Details] PRIMARY KEY CLUSTERED 
(
	[Sales_Order_Code] ASC,
	[SL_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales_Order_Head](
	[Sales_Order_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Sales_Order_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_Sales_Order_Head_for_OrderBy]  DEFAULT ((0)),
	[Sales_Order_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL CONSTRAINT [DF_Sales_Order_Head_Payment_Method]  DEFAULT (''),
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Cash_PartyName] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Cash_PartyName]  DEFAULT (''),
	[Party_PhoneNo] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Party_PhoneNo]  DEFAULT (''),
	[SalesAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_SalesAc_IdNo]  DEFAULT ((0)),
	[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_Sales_Order_Head_Tax_Type]  DEFAULT (''),
	[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_TaxAc_IdNo]  DEFAULT ((0)),
	[Delivery_Address1] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Delivery_Address1]  DEFAULT (''),
	[Delivery_Address2] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Delivery_Address2]  DEFAULT (''),
	[Delivery_Address3] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Delivery_Address3]  DEFAULT (''),
	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Vehicle_No]  DEFAULT (''),
	[Narration] [varchar](500) NULL CONSTRAINT [DF_Sales_Order_Head_Narration]  DEFAULT (''),
	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Head_Total_Qty]  DEFAULT ((0)),
	[Total_Bags] [int] NULL CONSTRAINT [DF_Sales_Order_Head_Total_Bags]  DEFAULT ((0)),
	[SubTotal_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_SubTotal_Amount]  DEFAULT ((0)),
	[Total_DiscountAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Total_DiscountAmount]  DEFAULT ((0)),
	[Total_TaxAmount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Total_TaxAmount]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Gross_Amount]  DEFAULT ((0)),
	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_CashDiscount_Perc]  DEFAULT ((0)),
	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_CashDiscount_Amount]  DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Assessable_Value]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Tax_Amount]  DEFAULT ((0)),
	[Freight_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Freight_Amount]  DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_AddLess_Amount]  DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Round_Off]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Net_Amount]  DEFAULT ((0)),
	[Document_Through] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Document_Through]  DEFAULT (''),
	[Despatch_To] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Despatch_To]  DEFAULT (''),
	[Lr_No] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Lr_No]  DEFAULT (''),
	[Lr_Date] [varchar](20) NULL CONSTRAINT [DF_Sales_Order_Head_Lr_Date]  DEFAULT (''),
	[Booked_By] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Booked_By]  DEFAULT (''),
	[Transport_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_Transport_IdNo]  DEFAULT ((0)),
	[Freight_ToPay_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Freight_ToPay_Amount]  DEFAULT ((0)),
	[Dc_No] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Dc_No]  DEFAULT (''),
	[Dc_Date] [varchar](35) NULL CONSTRAINT [DF_Sales_Order_Head_Dc_Date]  DEFAULT (''),
	[Order_No] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Order_No]  DEFAULT (''),
	[Order_Date] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Order_Date]  DEFAULT (''),
	[Against_CForm_Status] [tinyint] NULL CONSTRAINT [DF_Sales_Order_Head_Against_CForm_Status]  DEFAULT ((0)),
	[Entry_Type] [varchar](20) NULL CONSTRAINT [DF_Sales_Order_Head_Entry_Type]  DEFAULT (''),
	[Payment_Terms] [varchar](100) NULL CONSTRAINT [DF_Sales_Order_Head_Payment_Terms]  DEFAULT (''),
	[OnAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_OnAc_IdNo]  DEFAULT ((0)),
	[Extra_Charges] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Extra_Charges]  DEFAULT ((0)),
	[Total_Extra_Copies] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Total_Extra_Copies]  DEFAULT ((0)),
	[Sub_Total_Copies] [numeric](18, 2) NULL CONSTRAINT [DF_Sales_Order_Head_Sub_Total_Copies]  DEFAULT ((0)),
	[Party_Name] [varchar](50) NULL CONSTRAINT [DF_Sales_Order_Head_Party_Name]  DEFAULT (''),
	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Sales_Order_Head_Weight]  DEFAULT ((0)),
	[Sales_OrderAc_IdNo] [int] NULL CONSTRAINT [DF_Sales_Order_Head_Sales_OrderAc_IdNo]  DEFAULT ((0)),
 CONSTRAINT [PK_Sales_Order_Head] PRIMARY KEY CLUSTERED 
(
	[Sales_Order_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales_Reading_Details](
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
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [SalesReturn_Details](
	[SalesReturn_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[SalesReturn_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_CashSalesReturn_Details_for_OrderBy]  DEFAULT ((0)),
	[SalesReturn_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL CONSTRAINT [DF_CashSalesReturn_Details_Ledger_IdNo]  DEFAULT ((0)),
	[SL_No] [smallint] NOT NULL,
	[Item_IdNo] [int] NULL CONSTRAINT [DF_CashSalesReturn_Details_Item_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_CashSalesReturn_Details_Unit_IdNo]  DEFAULT ((0)),
	[Noof_Items] [numeric](18, 3) NULL CONSTRAINT [DF_CashSalesReturn_Details_Noof_Items]  DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Details_Rate]  DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Details_Total_Amount1]  DEFAULT ((0)),
	[Serial_No] [varchar](500) NULL CONSTRAINT [DF_SalesReturn_Details_Serial_No]  DEFAULT (''),
	[Sales_Detail_Slno] [int] NULL DEFAULT ((0)),
	[Sales_Code] [varchar](30) NULL DEFAULT (''),
	[Tax_Rate] [numeric](18, 2) NULL DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL DEFAULT ((0)),
	[Colour_IdNo] [int] NULL DEFAULT ((0)),
	[Design_IdNo] [int] NULL DEFAULT ((0)),
	[Gender_IdNo] [int] NULL DEFAULT ((0)),
	[Sleeve_IdNo] [int] NULL DEFAULT ((0)),
	[Size_IdNo] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_SalesReturn_Details] PRIMARY KEY CLUSTERED 
(
	[SalesReturn_Code] ASC,
	[SL_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [SalesReturn_Head](
	[SalesReturn_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[SalesReturn_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL CONSTRAINT [DF_CashSalesReturn_Head_for_OrderBy]  DEFAULT ((0)),
	[SalesReturn_Date] [datetime] NOT NULL,
	[Payment_Method] [varchar](20) NULL CONSTRAINT [DF_SalesReturn_Head_Payment_Method]  DEFAULT (''),
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_CashSalesReturn_Head_Ledger_IdNo]  DEFAULT ((0)),
	[Cash_PartyName] [varchar](50) NULL CONSTRAINT [DF_SalesReturn_Head_Delivery_Address11]  DEFAULT (''),
	[Bill_No] [varchar](35) NULL CONSTRAINT [DF__SalesRetu__Dc_No__592635D8]  DEFAULT (''),
	[SalesReturnAc_IdNo] [int] NULL CONSTRAINT [DF_CashSalesReturn_Head_SalesAc_IdNo]  DEFAULT ((0)),
	[Tax_Type] [varchar](20) NULL CONSTRAINT [DF_SalesReturn_Head_Tax_Type]  DEFAULT (''),
	[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_SalesReturn_Head_SalesAc_IdNo1]  DEFAULT ((0)),
	[Narration] [varchar](500) NULL CONSTRAINT [DF_SalesReturn_Head_Narration]  DEFAULT (''),
	[Total_Qty] [numeric](18, 3) NULL CONSTRAINT [DF_CashSalesReturn_Head_Total_Qty]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Head_CashDiscount_Perc1]  DEFAULT ((0)),
	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Perc]  DEFAULT ((0)),
	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Amount]  DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_AddLess_Amount1]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Perc1]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_SalesReturn_Head_CashDiscount_Amount1]  DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Head_AddLess_Amount]  DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Head_Round_Off]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_CashSalesReturn_Head_Net_Amount]  DEFAULT ((0)),
	[Bill_Date] [varchar](35) NULL DEFAULT (''),
	[Total_DiscountAmount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_TaxAmount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Freight_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[SubTotal_Amount] [numeric](18, 2) NULL DEFAULT ((0)),
	[Total_Bags] [int] NULL DEFAULT ((0)),
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
 CONSTRAINT [PK_SalesReturn_Head] PRIMARY KEY CLUSTERED 
(
	[SalesReturn_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Settings_Head](
	[Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,
	[C_Name] [varchar](50) NULL CONSTRAINT [DF__Settings___C_Nam__795DFB40]  DEFAULT (''),
	[AutoBackUp_Date] [smalldatetime] NULL,
 CONSTRAINT [PK_Settings_Head] PRIMARY KEY CLUSTERED 
(
	[Auto_SlNo] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shift_Head](
	[Shift_IdNo] [int] NOT NULL,
	[Shift_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Shift_Head] PRIMARY KEY CLUSTERED 
(
	[Shift_IdNo] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Size_Head](
	[Size_IdNo] [int] NOT NULL,
	[Size_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Total_Sqft] [numeric](18, 2) NULL DEFAULT ((0)),
 CONSTRAINT [PK_Size_Head] PRIMARY KEY CLUSTERED 
(
	[Size_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Size_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sleeve_Head](
	[Sleeve_IdNo] [int] NOT NULL,
	[Sleeve_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Sleeve_Head] PRIMARY KEY CLUSTERED 
(
	[Sleeve_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Sleeve_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Spinning_WasteSales_Details](
	[Spinning_WasteSales_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Spinning_WasteSales_No] [varchar](20) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Spinning_WasteSales_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [int] NOT NULL,
	[SL_No] [smallint] NOT NULL,
	[Waste_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Item_IdNo]  DEFAULT ((0)),
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Unit_IdNo]  DEFAULT ((0)),
	[Packs] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Packs]  DEFAULT ((0)),
	[Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Weight]  DEFAULT ((0)),
	[Rate] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Rate]  DEFAULT ((0)),
	[Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Amount]  DEFAULT ((0)),
	[Pack_Nos] [varchar](500) NULL CONSTRAINT [DF_Spinning_WasteSales_Details_Bag_Nos]  DEFAULT (''),
 CONSTRAINT [PK_Spinning_WasteSales_Details] PRIMARY KEY CLUSTERED 
(
	[Spinning_WasteSales_Code] ASC,
	[SL_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Spinning_WasteSales_Head](
	[Spinning_WasteSales_Code] [varchar](50) NOT NULL,
	[Company_IdNo] [smallint] NOT NULL,
	[Spinning_WasteSales_No] [varchar](50) NOT NULL,
	[for_OrderBy] [numeric](18, 2) NOT NULL,
	[Spinning_WasteSales_Date] [datetime] NOT NULL,
	[Ledger_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Ledger_IdNo]  DEFAULT ((0)),
	[SalesAc_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_SalesAc_IdNo]  DEFAULT ((0)),
	[TaxAc_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_TaxAc_IdNo]  DEFAULT ((0)),
	[CessAc_IdNo] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_CessAc_IdNo]  DEFAULT ((0)),
	[Delivery_Address1] [varchar](50) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address1]  DEFAULT (''),
	[Delivery_Address2] [varchar](50) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address2]  DEFAULT (''),
	[Delivery_Address3] [varchar](50) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Delivery_Address3]  DEFAULT (''),
	[Vehicle_No] [varchar](50) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Vehicle_No]  DEFAULT (''),
	[Removal_Date] [varchar](20) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Removal_Date]  DEFAULT (''),
	[Pack_Nos] [varchar](500) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Bag_Nos]  DEFAULT (''),
	[Total_Packs] [int] NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Total_Packs]  DEFAULT ((0)),
	[Total_Weight] [numeric](18, 3) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Total_Weight]  DEFAULT ((0)),
	[Gross_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_GrossAmount]  DEFAULT ((0)),
	[CashDiscount_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_CashDiscount_Perc]  DEFAULT ((0)),
	[CashDiscount_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_CashDiscount_Amount]  DEFAULT ((0)),
	[Assessable_Value] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Assessable_Value]  DEFAULT ((0)),
	[Tax_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Tax_Perc]  DEFAULT ((0)),
	[Tax_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Tax_Amount]  DEFAULT ((0)),
	[Cess_Perc] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Cess_Perc]  DEFAULT ((0)),
	[Cess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Cess_Amount]  DEFAULT ((0)),
	[AddLess_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_AddLess_Amount]  DEFAULT ((0)),
	[Round_Off] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Round_Off]  DEFAULT ((0)),
	[Net_Amount] [numeric](18, 2) NULL CONSTRAINT [DF_Spinning_WasteSales_Head_Net_Amount]  DEFAULT ((0)),
 CONSTRAINT [PK_Spinning_WasteSales_Head] PRIMARY KEY CLUSTERED 
(
	[Spinning_WasteSales_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [State_Head](
	[State_IdNo] [smallint] NOT NULL,
	[State_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Cst_Value] [int] NOT NULL,
 CONSTRAINT [PK_State_Head] PRIMARY KEY CLUSTERED 
(
	[State_IdNo] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Temp_Ends_Head](
	[Ends_Name] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [temp_report](
	[ledger_name] [varchar](100) NULL DEFAULT (''),
	[ledger_address1] [varchar](50) NULL DEFAULT (''),
	[ledger_address2] [varchar](50) NULL DEFAULT (''),
	[ledger_address3] [varchar](50) NULL DEFAULT (''),
	[ledger_address4] [varchar](50) NULL DEFAULT (''),
	[ledger_phoneno] [varchar](50) NULL DEFAULT (''),
	[ledger_tinno] [varchar](50) NULL DEFAULT (''),
	[ledger_idno] [int] NULL DEFAULT ((0)),
	[status_for_row] [varchar](10) NULL DEFAULT ('DATA'),
	[auto_slno_for_orderby] [int] IDENTITY(1,1) NOT NULL,
	[auto_slno_for_orderby_group] [numeric](9, 1) NULL DEFAULT ((0)),
	[row_data] [varchar](3000) NULL DEFAULT ('')
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TempTable_For_NegativeStock](
	[Reference_Code] [varchar](50) NULL DEFAULT (''),
	[Reference_Date] [smalldatetime] NULL,
	[Company_Idno] [smallint] NULL DEFAULT ((0)),
	[Item_IdNo] [int] NULL DEFAULT ((0)),
	[Quantity] [numeric](18, 3) NULL DEFAULT ((0))
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Transport_Head](
	[Transport_IdNo] [smallint] NOT NULL,
	[Transport_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Transport_Head] PRIMARY KEY CLUSTERED 
(
	[Transport_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Transport_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Unit_Head](
	[Unit_IdNo] [smallint] NOT NULL,
	[Unit_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Unit_Head] PRIMARY KEY CLUSTERED 
(
	[Unit_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Unit_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Variety_Head](
	[Variety_IdNo] [smallint] NOT NULL,
	[Variety_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Variety_Head] PRIMARY KEY CLUSTERED 
(
	[Variety_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Variety_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher_Bill_Details](
	[Voucher_Bill_Code] [varchar](100) NOT NULL,
	[Company_Idno] [smallint] NOT NULL,
	[Voucher_Bill_Date] [smalldatetime] NOT NULL,
	[Ledger_IdNo] [smallint] NOT NULL,
	[Entry_Identification] [varchar](100) NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[CrDr_Type] [varchar](10) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher_Bill_Head](
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
 CONSTRAINT [PK_Voucher_Bills_Head] PRIMARY KEY NONCLUSTERED 
(
	[Voucher_Bill_Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher_Details](
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
) ON [PRIMARY],
 CONSTRAINT [Dup_VoucherDetails_EntryIdentication_SlNo] UNIQUE NONCLUSTERED 
(
	[Entry_Identification] ASC,
	[Sl_No] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Voucher_Details] UNIQUE NONCLUSTERED 
(
	[Company_Idno] ASC,
	[Year_For_Report] ASC,
	[Voucher_Type] ASC,
	[Voucher_No] ASC,
	[Sl_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher_Head](
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
) ON [PRIMARY],
 CONSTRAINT [Dup_VoucherHead_EntryIndentification] UNIQUE NONCLUSTERED 
(
	[Entry_Identification] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Voucher_Head] UNIQUE NONCLUSTERED 
(
	[Company_Idno] ASC,
	[Year_For_Report] ASC,
	[Voucher_Type] ASC,
	[Voucher_No] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Waste_Head](
	[Waste_IdNo] [smallint] NOT NULL,
	[Waste_Name] [varchar](50) NOT NULL,
	[Sur_Name] [varchar](50) NOT NULL,
	[Unit_IdNo] [smallint] NULL CONSTRAINT [DF_Waste_Head_Unit_IdNo]  DEFAULT ((0)),
 CONSTRAINT [PK_Waste_Head] PRIMARY KEY CLUSTERED 
(
	[Waste_IdNo] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Waste_Head] UNIQUE NONCLUSTERED 
(
	[Sur_Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]

