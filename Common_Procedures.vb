Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Management

Public Class Common_Procedures

    Public Shared CompanyDetailsDataBaseName As String = "TSoft_Billing_CompanyGroup_Details"
    Public Shared Connection_String As String
    Public Shared ConnectionString_CompanyGroupdetails As String
    Public Shared ConnectionString_Master As String
    Public Shared ServerName As String
    Public Shared ServerPassword As String
    Public Shared ServerWindowsLogin As String
    Public Shared ServerDataBaseLocation_InExTernalUSB As String

    Public Shared CompGroupIdNo As Integer
    Public Shared CompGroupName As String
    Public Shared CompGroupFnRange As String

    Public Shared DataBaseName As String

    Public Shared FnRange As String
    Public Shared FnYearCode As String
    Public Shared CompIdNo As Integer
    Public Shared Company_FromDate As Date
    Public Shared Company_ToDate As Date
    Public Shared AppPath As String
    Public Shared MRP_saving As Single

    Public Shared Print_OR_Preview_Status As Integer

    Public Shared BillAdj_Amt As Single = 0

    Public Shared VoucherType As String = ""
    Public Shared Voucher_CR_Name As String = ""
    Public Shared Voucher_CR_or_DR As String = ""
    Public Shared Voucher_Code As String = ""
    Public Shared Voucher_DR_Name As String = ""

    Public Shared Password_Input As String = ""
    Public Shared Sales_Or_Service As String = ""
    Public Shared SalesEntryType As String = ""
    Public Shared vShowEntrance_Status_ForCC As Boolean = False
    Public Shared First_Opened_Today As Boolean = False
    Public Shared MDI_LedType As String
    Public Shared Last_Closed_FormName As String
    Public Shared GST_and_VAT_Entry_Status As Boolean = False
    Public Shared DriveVolumeSerialName As String = ""

    Public Shared AdvanceType As String
    Public Shared Att_Log_IN_OUT_STS As String
    Public Shared WeekOff_Allowance_Fixed_Status As Integer

    Public Structure Report_ComboDetails
        Dim PKey As String
        Dim TableName As String
        Dim Selection_FieldName As String
        Dim Return_FieldName As String
        Dim Condition As String
        Dim Display_Name As String
        Dim BlankFieldCondition As String
        Dim CtrlType_Cbo_OR_Txt As String
    End Structure
    Public Shared RptCboDet(10) As Report_ComboDetails

    Public Structure Encryption_DeEncryption_Pass_Salt_Phrase
        Dim passPhrase As String
        Dim saltValue As String
    End Structure
    Public Shared Entrance_SQL_PassWord As Encryption_DeEncryption_Pass_Salt_Phrase
    Public Shared UserCreation_AcPassWord As Encryption_DeEncryption_Pass_Salt_Phrase
    Public Shared UserCreation_UnAcPassWord As Encryption_DeEncryption_Pass_Salt_Phrase
    Public Shared SoftWareRegister As Encryption_DeEncryption_Pass_Salt_Phrase

    Public Structure SettingsDetails
        Dim CustomerCode As String
        Dim CustomerDBCode As String
        Dim CompanyName As String
        Dim SoftWare_UserType As String
        Dim Sdd As Date
        Dim AutoBackUp_Date As Date

        Dim SMS_Provider_SenderID As String
        Dim SMS_Provider_Key As String
        Dim SMS_Provider_RouteID As String
        Dim SMS_Provider_Type As String

        Dim Email_Address As String
        Dim Email_Password As String
        Dim Email_Host As String
        Dim Email_Port As Integer

        Dim EntrySelection_Combine_AllCompany As Integer
        Dim InvoicePrint_Format As String
        Dim Jurisdiction As String
        Dim Report_Show_CurrentDate_IN_ToDate As Integer

        Dim NegativeStock_Restriction As Integer
        Dim Printing_Show_PrintDialogue As Integer
        Dim OnSave_MoveTo_NewEntry_Status As Integer
        Dim PAYROLLENTRY_Attendance_In_Hours_Status As Integer
        Dim PreviousEntryDate_ByDefault As Integer
        Dim Payroll_Status As Integer

        Dim Validation_End_Date As Date

        Dim OT_Allowed_Only_After_ShiftOut_Time_Status As Integer
        Dim NoOfDays_For_Month_Wages_Take_TotalDays_In_Month As Integer

        Dim WeekOff_Allowance_Fixed_Status As Integer

    End Structure

    Public Shared settings As SettingsDetails

    Public Enum CommonLedger As Integer

        Cash_Ac = 1
        Weaving_Wages_Ac = 2
        Sizing_Charges_Ac = 3
        Godown_Ac = 4
        Transport_Charges_Ac = 7
        TDS_Charges_Ac = 8
        Freight_Charges_Ac = 9
        Salary_Ac = 10
        DD_COMMISSION_Ac = 11
        Stock_In_Hand_Ac = 12
        Profit_Loss_Ac = 13
        RATE_DIFFERENCE_Ac = 14
        CASH_DISCOUNT_Ac = 15
        Agent_Commission_Ac = 16
        Discount_Ac = 17
        Conversion_Bill_Charges_Ac = 18
        Processing_Charges_Ac = 19
        Vat_Ac = 20
        Purchase_Ac = 21
        Sales_Ac = 22
        ADVANCE_DEDUCTION_AC = 30

    End Enum

    Public Enum OperationType As Integer

        Open = 0
        All = 1
        AddNew = 2
        Edit = 3
        Delete = 4
        Insert = 5
        View = 6

    End Enum

    Public Structure MasterReturnDetails
        Dim Form_Name As String
        Dim Control_Name As String
        Dim Master_Type As String
        Dim Return_Value As String
    End Structure
    Public Shared Master_Return As MasterReturnDetails

    Public Structure Report_InputDetails
        Dim ReportName As String
        Dim ReportGroupName As String
        Dim ReportHeading As String
        Dim ReportInputs As String
        Dim IsGridReport As Boolean
        Dim Date1 As Date
        Dim Date2 As Date
        Dim IdNo1 As Integer
        Dim IdNo2 As Integer
        Dim Name1 As String
        Dim Name2 As String
        Dim Prev_IdNo_Column As String
        Dim Filter_Based_On_Prev_Col As Boolean
    End Structure

    Public Shared RptInputDet As Report_InputDetails

    Public Structure UserDetails
        Dim IdNo As Integer
        Dim Name As String
        Dim Type As String
        Dim AccountPassword As String
        Dim UnAccountPassword As Date
        Dim RealName As String
    End Structure

    Public Shared User As UserDetails


    Public Structure UserRightsDetails

        Dim Ledger_Creation As String
        Dim Area_Creation As String
        Dim Item_Creation As String
        Dim ItemGroup_Creation As String
        Dim Unit_Creation As String
        Dim Category_Creation As String
        Dim Variety_Creation As String
        Dim Waste_Creation As String
        Dim Size_Creation As String
        Dim Transport_Creation As String

        Dim Ledger_OpeningBalance As String
        Dim Opening_Stock As String

        Dim Bill_Entry As String

        Dim Purchase_Entry As String
        Dim Sales_Entry As String
        Dim Tax_Sales_Entry As String
        Dim Labour_Sales_Entry As String
        Dim Delivery_entry As String
        Dim sales_Quotation_Entry As String

        Dim WasteSales_Entry As String

        Dim Knotting_Entry As String
        Dim Knotting_Invoice_Entry As String

        Dim Invoice_Saara_Entry As String
        Dim Delivery_Saara_Entry As String
        Dim Bill_Entry_Saara As String

        Dim Printing_Invoice_Entry As String
        Dim Printing_Order_Entry As String
        Dim Printing_Order_Program_Entry As String

        Dim Voucher_Entry As String

        Dim Accounts_Ledger_Report As String
        Dim Accounts_GroupLedger_Report As String
        Dim Accounts_DayBook As String
        Dim Accounts_AllLedger As String
        Dim Accounts_TB As String
        Dim Accounts_Profit_Loss As String
        Dim Accounts_BalanceSheet As String
        Dim Accounts_CustomerBills As String

        Dim Report_Purchase_Register As String
        Dim Report_Sales_Register As String
        Dim Report_Stock_Register As String
        Dim Report_Minimum_Stock_Register As String
        Dim Report_Knotting_Reports As String

        Dim Shift_Creation As String
        Dim Salary_Payment_type_Creation As String
        Dim Holiday_Creation As String
        Dim Employee_creation As String
        Dim Employee_Advance_Opening As String
        Dim Employee_Salary_Payment_Entry As String
        Dim Employee_Advance_Payment_Entry As String
        Dim Employee_Salary_Advance_Payment_Entry As String
        Dim Employee_Salary_Advance_Opening As String
        Dim Employee_Salary_Entry As String
        Dim Employee_Attendance_Missing_Time_Addition As String
        Dim Employee_Attendance_Manual As String
        Dim Employee_Attendance_Machine As String
        Dim Employee_Addition_and_Deduction_Entry As String

        Dim Employee_Bonus_Entry As String

    End Structure

    Public Structure UserRightsDetails1

        Public UserInfo(,) As String

    End Structure

    Public Shared UR As UserRightsDetails
    Public Shared UR1 As UserRightsDetails1

    Public Shared AWS_ACCESS_KEY As String
    Public Shared AWS_SECRET_KEY As String

    Public Shared AWS_DB_BUCKET As String
    Public Shared AWS_SW_BUCKET As String

    Public Shared AWS_BUCKET_FOR_DOWNLOADER As String

    'Public Shared Form_UR1 As UserRightsDetails1

    'Public Enum DriveType As Integer
    '    Unknown = 0
    '    NoRoot = 1
    '    Removable = 2
    '    Localdisk = 3
    '    Network = 4
    '    CD = 5
    '    RAMDrive = 6
    'End Enum

    Public Shared Sub Print_To_PrintDocument(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal PrintText As String, ByVal Xaxis As Decimal, ByVal Yaxis As Decimal, ByVal AlignMent As Integer, ByVal DataWidth As Decimal, ByVal DataFont As Font, Optional ByVal BrushColor As Brush = Nothing)
        Dim X As Decimal, Y As Decimal
        Dim strWidth As Decimal, strHeight As Decimal = 0
        Dim vbrushcolor As Brush

        strWidth = e.Graphics.MeasureString(PrintText, DataFont).Width
        'strHeight = e.Graphics.MeasureString(PrintText, DataFont).Height

        If AlignMent = 1 Then
            X = Xaxis - strWidth

        ElseIf AlignMent = 2 Then
            If DataWidth > strWidth Then
                X = Xaxis + (DataWidth - strWidth) / 2
            Else
                X = Xaxis
            End If

        Else
            X = Xaxis

        End If
        Y = Yaxis

        If IsNothing(BrushColor) = False Then
            vbrushcolor = BrushColor
        Else
            vbrushcolor = Brushes.Black
        End If

        e.Graphics.DrawString(PrintText, DataFont, vbrushcolor, X, Y)

    End Sub

    Public Shared Sub Print_To_PrintDocument_Red(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal PrintText As String, ByVal Xaxis As Decimal, ByVal Yaxis As Decimal, ByVal AlignMent As Integer, ByVal DataWidth As Decimal, ByVal DataFont As Font)
        Dim X As Decimal, Y As Decimal
        Dim strWidth As Decimal, strHeight As Decimal = 0

        strWidth = e.Graphics.MeasureString(PrintText, DataFont).Width
        'strHeight = e.Graphics.MeasureString(PrintText, DataFont).Height

        If AlignMent = 1 Then
            X = Xaxis - strWidth

        ElseIf AlignMent = 2 Then
            If DataWidth > strWidth Then
                X = Xaxis + (DataWidth - strWidth) / 2
            Else
                X = Xaxis
            End If

        Else
            X = Xaxis

        End If
        Y = Yaxis

        e.Graphics.DrawString(PrintText, DataFont, Brushes.Red, X, Y)

    End Sub

    Public Shared Sub Print_To_PrintDocument_Green(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal PrintText As String, ByVal Xaxis As Decimal, ByVal Yaxis As Decimal, ByVal AlignMent As Integer, ByVal DataWidth As Decimal, ByVal DataFont As Font)
        Dim X As Decimal, Y As Decimal
        Dim strWidth As Decimal, strHeight As Decimal = 0

        strWidth = e.Graphics.MeasureString(PrintText, DataFont).Width
        'strHeight = e.Graphics.MeasureString(PrintText, DataFont).Height

        If AlignMent = 1 Then
            X = Xaxis - strWidth

        ElseIf AlignMent = 2 Then
            If DataWidth > strWidth Then
                X = Xaxis + (DataWidth - strWidth) / 2
            Else
                X = Xaxis
            End If

        Else
            X = Xaxis

        End If
        Y = Yaxis

        e.Graphics.DrawString(PrintText, DataFont, Brushes.Green, X, Y)

    End Sub

    Public Shared Function Rupees_Converstion(ByVal amt As Single) As String
        Dim A1 As String = ""
        Dim s2 As String = ""
        Dim s3 As String = ""
        Dim Ps1 As Single
        Dim i, j As Integer
        Dim d(100) As String
        Dim Wrd(6) As String
        Dim Sum As Integer

        d(1) = "One"
        d(2) = "Two"
        d(3) = "Three"
        d(4) = "Four"
        d(5) = "Five"
        d(6) = "Six"
        d(7) = "Seven"
        d(8) = "Eight"
        d(9) = "Nine"
        d(10) = "Ten"
        d(11) = "Eleven"
        d(12) = "Twelve"
        d(13) = "Thirteen"
        d(14) = "Fourteen"
        d(15) = "Fifteen"
        d(16) = "Sixteen"
        d(17) = "Seventeen"
        d(18) = "Eighteen"
        d(19) = "Ninteen"
        d(20) = "Twenty"
        d(30) = "Thirty"
        d(40) = "Forty"
        d(50) = "Fifty"
        d(60) = "Sixty"
        d(70) = "Seventy"
        d(80) = "Eighty"
        d(90) = "Ninety"
        Wrd(1) = ""
        Wrd(2) = " Hundred "
        Wrd(3) = " Thousand "
        Wrd(4) = " Lakhs "
        Wrd(5) = " Crores "
        s3 = ""
        Ps1 = Val(Right$(Trim(Format(amt, "###########0.00")), 2))
        If Ps1 <> 0 Then If (Ps1 Mod 10 = 0) Or Ps1 <= 20 Then s3 = d(Ps1) + " Paise" Else s3 = d(Int(Ps1 / 10) * 10) + " " + d(Ps1 Mod 10) + " Paise"
        If Ps1 > 0 Then amt = amt - (Ps1 / 100)
        Do While amt > 0
            i = i + 1
            Sum = amt Mod (IIf((i = 2), 10, 100))
            amt = Int(amt / (IIf((i = 2), 10, 100)))
            If Sum <> 0 Then j = j + 1
            A1 = IIf((j = 2), "And ", "")
            If Sum <> 0 Then If (Sum Mod 10 = 0) Or Sum <= 20 Then s2 = d(Sum) + Wrd(i) + A1 + s2 Else s2 = d(Int(Sum / 10) * 10) + " " + d(Sum Mod 10) + Wrd(i) + A1 + s2
        Loop
        Rupees_Converstion = Trim(s2) + IIf((Len(Trim(s2)) > 0) And (Len(Trim(s3)) > 0), " Rupees And ", "") + s3 + " Only"
    End Function

    Public Shared Function Currency_Format(ByVal Value As Double) As String
        Dim s1 As String = ""
        Dim s2 As String = ""
        Dim k As String = ""

        If Value >= 0 Then k = "" Else k = "-"

        s1 = Trim(Format(Math.Abs(Value), "############0.00"))

        Select Case Len(s1)
            Case Is < 9
                s2 = Format(Val(s1), "##,##0.00")
            Case 9, 10
                s2 = Left$(s1, Len(s1) - 8) & "," & Mid$(s1, Len(s1) - 7, 2) & "," & Right$(s1, 6)
            Case 11, 12
                s2 = Left$(s1, Len(s1) - 10) & "," & Mid$(s1, Len(s1) - 9, 2) & "," & Mid$(s1, Len(s1) - 7, 2) & "," & Right$(s1, 6)
            Case 13, 14
                s2 = Left$(s1, Len(s1) - 12) & "," & Mid$(s1, Len(s1) - 11, 2) & "," & Mid$(s1, Len(s1) - 9, 2) & "," & Mid$(s1, Len(s1) - 7, 2) & "," & Right$(s1, 6)
            Case Is > 14
                s2 = Left$(s1, Len(s1) - 14) & "," & Mid$(s1, Len(s1) - 13, 2) & "," & Mid$(s1, Len(s1) - 11, 2) & "," & Mid$(s1, Len(s1) - 9, 2) & "," & Mid$(s1, Len(s1) - 7, 2) & "," & Right$(s1, 6)
        End Select
        Currency_Format = k & Trim(s2)
    End Function

    Public Shared Function get_VoucherType(ByVal VouName As String) As String
        Select Case Trim(LCase(VouName))
            Case "purc"
                get_VoucherType = "Purchase"
            Case "sale"
                get_VoucherType = "Sales"
            Case "rcpt"
                get_VoucherType = "Receipt"
            Case "pymt"
                get_VoucherType = "Payment"
            Case "cntr"
                get_VoucherType = "Contra"
            Case "jrnl"
                get_VoucherType = "Journal"
            Case "crnt"
                get_VoucherType = "Credit Note"
            Case "dbnt"
                get_VoucherType = "Debit Note"
            Case "csrp"
                get_VoucherType = "Cash Receipt"
            Case "cspy"
                get_VoucherType = "Cash Payment"
            Case "ptcs"
                get_VoucherType = "Petti Cash"
            Case "chrt"
                get_VoucherType = "Cheque Return"
            Case Else
                get_VoucherType = ""
        End Select
    End Function

    Public Shared Function Remove_NonCharacters(ByVal Txt As String) As String
        Dim S As String
        Dim I As Integer
        Dim k As Integer

        S = ""
        For I = 1 To Len(Txt)
            k = Asc(Mid(Txt, I, 1))
            If k = 45 Or k = 47 Or (k >= 48 And k <= 57) Or (k >= 65 And k <= 90) Or (k >= 97 And k <= 122) Or k = 95 Then
                S = S & Chr(k)
            End If
        Next
        Remove_NonCharacters = S
    End Function

    Public Shared Sub Control_Focus(ByVal Ka As Integer, ByVal Ctrl As Object)
        If Ka = 13 Or Ka = 40 Then SendKeys.Send("{TAB}")
        If Ka = 38 Then SendKeys.Send("+{TAB}")
        If TypeOf Ctrl Is TextBox Or TypeOf Ctrl Is ComboBox Then SendKeys.Send("{HOME}+{END}")
    End Sub

    Public Shared Function OrderBy_CodeToValue(ByVal Code As String) As Single
        Dim c As String = ""
        Dim k As Single = 0

        If Val(Code) = 0 Then
            OrderBy_CodeToValue = 0
            Exit Function
        End If

        c = Replace(Code, Val(Code), "")
        k = 0
        If Trim(c) <> "" Then k = Format((Asc(UCase(c)) - 64) / 100, "######0.00")

        OrderBy_CodeToValue = Format(Val(Code) + k, "#####0.00")

    End Function

    Public Shared Function OrderBy_ValueToCode(ByVal value As Single) As String
        Dim c As String = ""
        Dim k As Single = 0

        If Val(value) = 0 Then
            OrderBy_ValueToCode = ""
            Exit Function
        End If

        k = Format(Val(value), "#####0.00") - Int(Val(value))

        c = ""
        If Val(k) > 0 Then c = UCase(Chr(64 + k))

        OrderBy_ValueToCode = Int(Val(value)) & c

    End Function

    Public Shared Function Accept_NumericOnly(ByVal KeyAscii_Value As Integer) As Integer
        Accept_NumericOnly = 0
        If (KeyAscii_Value >= 48 And KeyAscii_Value <= 57) Or KeyAscii_Value = 45 Or KeyAscii_Value = 46 Or KeyAscii_Value = 13 Or KeyAscii_Value = 8 Or KeyAscii_Value = 9 Then
            Accept_NumericOnly = KeyAscii_Value
        End If
    End Function

    Public Function Accept_AlphaNumericOnly(ByVal KeyAscii_Value As Integer) As Integer
        Accept_AlphaNumericOnly = 0
        If (KeyAscii_Value <> 39 And (KeyAscii_Value >= 32 And KeyAscii_Value <= 57)) Or (KeyAscii_Value >= 65 And KeyAscii_Value <= 90) Or (KeyAscii_Value >= 97 And KeyAscii_Value <= 122) Or KeyAscii_Value = 13 Or KeyAscii_Value = 8 Or KeyAscii_Value = 9 Or KeyAscii_Value = 92 Then
            Accept_AlphaNumericOnly = KeyAscii_Value
        End If
    End Function

    Public Shared Function get_Company_DataBaseName(ByVal CompIdNo As Integer) As String
        Dim DbNm As String = ""
        Dim S As String = ""

        DbNm = ""

        If Trim(CompanyDetailsDataBaseName) <> "" Then

            S = Replace(Trim(LCase(CompanyDetailsDataBaseName)), "_companygroup_details", "")

            DbNm = Trim(S) & "_" & Trim(Val(CompIdNo))

        End If

        get_Company_DataBaseName = Trim(DbNm)

    End Function


    Public Shared Function Company_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCompany_Nm As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCompany_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Company_IdNo from Company_Head where Company_Name = '" & Trim(vCompany_Nm) & "'", Cn1)
        Da.Fill(Dt)

        vCompany_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCompany_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Company_NameToIdNo = Val(vCompany_ID)

    End Function

    Public Shared Sub Default_Unit_Creation(ByVal Cn1 As SqlClient.SqlConnection)

        Dim ERRCNT As Integer = 0

        Dim Units() As String = Split("BAG-BAGS,BAL-BALE,BDL-BUNDLES,BKL-BUCKLES,BOU-BILLION OF UNITS,BOX-BOX,BTL-BOTTLES,BUN-BUNCHES,CAN-CANS" & _
                                    "CBM-CUBIC METERS,CCM-CUBIC CENTIMETERS,CMS-CENTIMETERS,CTN-CARTONS,DOZ-DOZENS,DRM-DRUMS,GGK-GREAT GROSS,GMS-GRAMMES," & _
                                      "GRS-GROSS,GYD-GROSS YARDS,KGS-KILOGRAMS,KLR-KILOLITRE,KME-KILOMETRE,MLT-MILILITRE,MTR-METERS,MTS-METRIC TON," & _
                                      "NOS-NUMBERS,PAC-PACKS,PCS-PIECES,PRS-PAIRS,QTL-QUINTAL,ROL-ROLLS,SET-SETS,SQF-SQUARE FEET,SQM-SQUARE METERS,SQY-SQUARE YARDS," & _
                                       "TBS-TABLETS,TGM-TEN GROSS,THD-THOUSANDS,TON-TONNES,TUB-TUBES,UGS-US GALLONS,UNT-UNITS,YDS-YARDS,OTH-OTHERS", ",")


        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = Cn1

        For I As Integer = 0 To UBound(Units)

            Try

                CMD.CommandText = "INSERT INTO Unit_Head  SELECT MAX(UNIT_IDNO) +1 ,'" & Units(I) & "','" & Units(I) & "' FROM UNIT_HEAD " & _
                                  " WHERE NOT EXISTS ( SELECT unit_name from unit_head where unit_name = '" & Units(I) & "')"
                CMD.ExecuteNonQuery()

            Catch ex As Exception

                ERRCNT = ERRCNT + 1

            End Try

        Next

    End Sub

    Public Shared Sub Default_Value_Updation(ByVal Cn1 As SqlClient.SqlConnection)

        Dim ERRCNT As Integer = 0


        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = Cn1


            Try

            CMD.CommandText = "UPDATE SALES_DETAILS SET UNIT_IDNO = " & Common_Procedures.Unit_NameToIdNo(Cn1, "NOS-NUMBERS") & " WHERE UNIT_IDNO IS NULL OR UNIT_IDNO = 0"
            CMD.ExecuteNonQuery()

            CMD.CommandText = "UPDATE SALES_DETAILS SET GST_PERCENTAGE = TAX_PERC WHERE GST_PERCENTAGE IS NULL OR GST_PERCENTAGE = 0"
            CMD.ExecuteNonQuery()

            Catch ex As Exception

                ERRCNT = ERRCNT + 1

            End Try



    End Sub

    Public Shared Function Company_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCompany_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCompany_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Company_Name from Company_Head where Company_IdNo = " & Str(Val(vCompany_ID)), Cn1)
        Da.Fill(Dt)

        vCompany_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCompany_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Company_IdNoToName = Trim(vCompany_Nm)

    End Function

    Public Shared Function Company_ShortNameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCompany_ShtNm As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCompany_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Company_IdNo from Company_Head where Company_ShortName = '" & Trim(vCompany_ShtNm) & "'", Cn1)
        Da.Fill(Dt)

        vCompany_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCompany_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Company_ShortNameToIdNo = Val(vCompany_ID)

    End Function

    
    Public Shared Function Company_IdNoToShortName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCompany_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCompany_ShtNm As String

        Da = New SqlClient.SqlDataAdapter("select Company_ShortName from Company_Head where Company_IdNo = " & Str(Val(vCompany_ID)), Cn1)
        Da.Fill(Dt)

        vCompany_ShtNm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCompany_ShtNm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Company_IdNoToShortName = Trim(vCompany_ShtNm)

    End Function

    Public Shared Function AccountsGroup_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vAccountsGroup_Nm As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vAccountsGroup_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select AccountsGroup_IdNo from AccountsGroup_Head where AccountsGroup_Name = '" & Trim(vAccountsGroup_Nm) & "'", Cn1)
        Da.Fill(Dt)

        vAccountsGroup_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vAccountsGroup_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        AccountsGroup_NameToIdNo = Val(vAccountsGroup_ID)

    End Function

    Public Shared Sub get_SMS_Provider_Details(ByVal Cn1 As SqlClient.SqlConnection, ByVal CompIDNo As Integer, ByRef SMS_SenderID As String, ByRef SMS_Key As String, ByRef SMS_RouteID As String, ByRef SMS_Type As String)
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim S_SenderID As String = ""
        Dim S_Key As String = ""
        Dim S_RouteID As String = ""
        Dim S_Type As String = ""

        Try
            If Val(CompIDNo) = 0 Then
                SMS_SenderID = Trim(Common_Procedures.settings.SMS_Provider_SenderID)
                SMS_Key = Trim(Common_Procedures.settings.SMS_Provider_Key)
                SMS_RouteID = Trim(Common_Procedures.settings.SMS_Provider_RouteID)
                SMS_Type = Trim(Common_Procedures.settings.SMS_Provider_Type)

            Else

                S_SenderID = ""
                S_Key = ""
                S_RouteID = ""
                S_Type = ""

                Da1 = New SqlClient.SqlDataAdapter("select * from company_head where company_idno = " & Str(Val(CompIDNo)), Cn1)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("SMS_Provider_SenderID").ToString) = False Then
                        S_SenderID = Trim(Dt1.Rows(0).Item("SMS_Provider_SenderID").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(0).Item("SMS_Provider_Key").ToString) = False Then
                        S_Key = Trim(Dt1.Rows(0).Item("SMS_Provider_Key").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(0).Item("SMS_Provider_RouteID").ToString) = False Then
                        S_RouteID = Trim(Dt1.Rows(0).Item("SMS_Provider_RouteID").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(0).Item("SMS_Provider_Type").ToString) = False Then
                        S_Type = Trim(Dt1.Rows(0).Item("SMS_Provider_Type").ToString)
                    End If
                End If
                Dt1.Clear()

                If Trim(S_SenderID) <> "" And Trim(S_Key) <> "" And Trim(S_RouteID) <> "" Then
                    SMS_SenderID = Trim(S_SenderID)
                    SMS_Key = Trim(S_Key)
                    SMS_RouteID = Trim(S_RouteID)
                    SMS_Type = Trim(S_Type)

                Else
                    SMS_SenderID = Trim(Common_Procedures.settings.SMS_Provider_SenderID)
                    SMS_Key = Trim(Common_Procedures.settings.SMS_Provider_Key)
                    SMS_RouteID = Trim(Common_Procedures.settings.SMS_Provider_RouteID)
                    SMS_Type = Trim(Common_Procedures.settings.SMS_Provider_Type)

                End If

            End If

            Dt1.Dispose()
            Da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR GETTING SMS PROVIDER DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Shared Function AccountsGroup_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vAccountsGroup_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vAccountsGroup_Nm As String

        Da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head where AccountsGroup_IdNo = " & Str(Val(vAccountsGroup_ID)), Cn1)
        Da.Fill(Dt)

        vAccountsGroup_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vAccountsGroup_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        AccountsGroup_IdNoToName = Trim(vAccountsGroup_Nm)

    End Function


    Public Shared Function Ledger_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vLed_IdNo As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vLed_Name As String

        Da = New SqlClient.SqlDataAdapter("select Ledger_Name from Ledger_Head where Ledger_IdNo = " & Str(Val(vLed_IdNo)), Cn1)
        Da.Fill(Dt)

        vLed_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vLed_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Ledger_IdNoToName = Trim(vLed_Name)

    End Function

    Public Shared Function Ledger_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vLed_Name As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vLed_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Ledger_IdNo from Ledger_Head where Ledger_Name = '" & Trim(vLed_Name) & "'", Cn1)
        Da.Fill(Dt)

        vLed_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vLed_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Ledger_NameToIdNo = Val(vLed_ID)

    End Function
    Public Shared Function State_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSte_IdNo As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vste_Name As String

        Da = New SqlClient.SqlDataAdapter("select State_Name from State_Head where State_IdNo = " & Str(Val(vSte_IdNo)), Cn1)
        Da.Fill(Dt)

        vste_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vste_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        State_IdNoToName = Trim(vste_Name)

    End Function

    Public Shared Function State_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSte_Name As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vSte_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select State_IdNo from State_Head where State_Name = '" & Trim(vSte_Name) & "'", Cn1)
        Da.Fill(Dt)

        vSte_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vSte_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        State_NameToIdNo = Val(vSte_ID)

    End Function
    Public Shared Function Salesman_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSte_IdNo As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vste_Name As String

        Da = New SqlClient.SqlDataAdapter("select Salesman_Name from Salesman_Head where Salesman_Idno = " & Str(Val(vSte_IdNo)), Cn1)
        Da.Fill(Dt)

        vste_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vste_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Salesman_IdNoToName = Trim(vste_Name)

    End Function

    Public Shared Function Salesman_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSte_Name As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vSte_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Salesman_Idno from Salesman_Head where Salesman_Name = '" & Trim(vSte_Name) & "'", Cn1)
        Da.Fill(Dt)

        vSte_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vSte_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Salesman_NameToIdNo = Val(vSte_ID)

    End Function
    Public Shared Function Ledger_AlaisNameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vLed_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vLed_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Ledger_IdNo from Ledger_AlaisHead where Ledger_DisplayName = '" & Trim(vLed_Name) & "' Order by Ledger_IdNo", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vLed_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vLed_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Ledger_AlaisNameToIdNo = Val(vLed_ID)

    End Function
    Public Shared Function Ledger_IdnoToAlaisName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vLed_idno As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vLed_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where Ledger_IdNo  = " & Val(vLed_idno) & " Order by Ledger_DisplayName", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vLed_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vLed_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Ledger_IdnoToAlaisName = Trim(vLed_Nm)

    End Function

    Public Shared Function AccountsGroup_NameToCode(ByVal Cn1 As SqlClient.SqlConnection, ByVal vAccountsGroup_Nm As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vAccountsGroup_Code As Integer

        Da = New SqlClient.SqlDataAdapter("select Parent_Idno from AccountsGroup_Head where AccountsGroup_Name = '" & Trim(vAccountsGroup_Nm) & "'", Cn1)
        Da.Fill(Dt)

        vAccountsGroup_Code = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vAccountsGroup_Code = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        AccountsGroup_NameToCode = Val(vAccountsGroup_Code)

    End Function

    Public Shared Function AccountsGroup_IdNoToCode(ByVal Cn1 As SqlClient.SqlConnection, ByVal vAccountsGroup_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vAccountsGroup_Code As String

        Da = New SqlClient.SqlDataAdapter("select Parent_Idno from AccountsGroup_Head where AccountsGroup_IdNo = " & Str(Val(vAccountsGroup_ID)), Cn1)
        Da.Fill(Dt)

        vAccountsGroup_Code = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vAccountsGroup_Code = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        AccountsGroup_IdNoToCode = Trim(vAccountsGroup_Code)

    End Function

    Public Shared Function Item_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vItem_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vItem_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Item_IdNo from Item_Head where Item_Name = '" & Trim(vItem_Name) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vItem_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vItem_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Item_NameToIdNo = Val(vItem_ID)

    End Function
    Public Shared Function Item_CodeToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vItem_Code As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vItem_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Item_IdNo from Item_Head where Item_Code = '" & Trim(vItem_Code) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vItem_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vItem_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Item_CodeToIdNo = Val(vItem_ID)

    End Function

    Public Shared Function Item_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vItem_ID As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vItem_Name As String

        Da = New SqlClient.SqlDataAdapter("select Item_Name from Item_Head where Item_IdNo = " & Str(Val(vItem_ID)), Cn1)
        Dt = New DataTable
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vItem_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vItem_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Item_IdNoToName = Trim(vItem_Name)

    End Function

    Public Shared Function Unit_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vUnit_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vUnit_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Unit_IdNo from Unit_Head where Unit_Name = '" & Trim(vUnit_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vUnit_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vUnit_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Unit_NameToIdNo = Val(vUnit_ID)

    End Function

    Public Shared Function Unit_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vUnit_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vUnit_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head where Unit_IdNo = " & Str(Val(vUnit_ID)), Cn1)
        Da.Fill(Dt)

        vUnit_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vUnit_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Unit_IdNoToName = Trim(vUnit_Nm)

    End Function

    Public Shared Function ItemGroup_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vItemGroup_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vItemGroup_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select ItemGroup_IdNo from ItemGroup_Head where ItemGroup_Name = '" & Trim(vItemGroup_Name) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)


        vItemGroup_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vItemGroup_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        ItemGroup_NameToIdNo = Val(vItemGroup_ID)

    End Function

    Public Shared Function Item_NameToItemGroupIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vItem_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vItemGroup_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select ItemGroup_IdNo from Item_Head where Item_Name = '" & Trim(vItem_Name) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)


        vItemGroup_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vItemGroup_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Item_NameToItemGroupIdNo = Val(vItemGroup_ID)

    End Function

    Public Shared Function Price_List_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vPrice_List_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vPrice_List_Name As String

        Da = New SqlClient.SqlDataAdapter("select Price_List_Name from Price_List_Head where Price_List_IdNo = " & Str(Val(vPrice_List_ID)), Cn1)
        Da.Fill(Dt)

        vPrice_List_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vPrice_List_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Price_List_IdNoToName = Trim(vPrice_List_Name)

    End Function
    Public Shared Function Price_List_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vPrice_List_Name As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vPrice_List_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Price_List_IdNo from Price_List_Head where Price_List_Name = '" & Trim(vPrice_List_Name) & "'", Cn1)
        Da.Fill(Dt)

        vPrice_List_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vPrice_List_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Price_List_NameToIdNo = Val(vPrice_List_ID)

    End Function

    Public Shared Function ItemGroup_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vItemGroup_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vItemGroup_Name As String

        Da = New SqlClient.SqlDataAdapter("select ItemGroup_Name from ItemGroup_Head where ItemGroup_IdNo = " & Str(Val(vItemGroup_ID)), Cn1)
        Da.Fill(Dt)

        vItemGroup_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vItemGroup_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        ItemGroup_IdNoToName = Trim(vItemGroup_Name)

    End Function

    Public Shared Function Variety_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vVariety_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vVariety_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Variety_IdNo from Variety_Head where Variety_Name = '" & Trim(vVariety_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vVariety_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vVariety_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Variety_NameToIdNo = Val(vVariety_ID)

    End Function

    Public Shared Function Variety_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vVariety_ID As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vVariety_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Variety_Name from Variety_Head where Variety_IdNo = " & Str(Val(vVariety_ID)), Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vVariety_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vVariety_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Variety_IdNoToName = Trim(vVariety_Nm)

    End Function

    Public Shared Function Area_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vArea_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vArea_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Area_IdNo from Area_Head where Area_Name = '" & Trim(vArea_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vArea_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vArea_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Area_NameToIdNo = Val(vArea_ID)

    End Function

    Public Shared Function Area_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vArea_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vArea_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Area_Name from Area_Head where Area_IdNo = " & Str(Val(vArea_ID)), Cn1)
        Da.Fill(Dt)

        vArea_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vArea_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Area_IdNoToName = Trim(vArea_Nm)

    End Function
    Public Shared Function Machine_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vMachine_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vMachine_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Machine_IdNo from Machine_Head where Machine_Name = '" & Trim(vMachine_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vMachine_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vMachine_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Machine_NameToIdNo = Val(vMachine_ID)

    End Function

    Public Shared Function Machine_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vMachine_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vMachine_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Machine_Name from Machine_Head where Machine_IdNo = " & Str(Val(vMachine_ID)), Cn1)
        Da.Fill(Dt)

        vMachine_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vMachine_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Machine_IdNoToName = Trim(vMachine_Nm)

    End Function
    Public Shared Function Cetegory_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCetegory_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCetegory_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Cetegory_IdNo from Cetegory_Head where Cetegory_Name = '" & Trim(vCetegory_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vCetegory_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCetegory_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Cetegory_NameToIdNo = Val(vCetegory_ID)

    End Function


    Public Shared Function Cetegory_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCetegory_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCetegory_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Cetegory_Name from Cetegory_Head where Cetegory_IdNo = " & Str(Val(vCetegory_ID)), Cn1)
        Da.Fill(Dt)

        vCetegory_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCetegory_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Cetegory_IdNoToName = Trim(vCetegory_Nm)

    End Function

    Public Shared Function Colour_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vColour_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vColour_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Colour_IdNo from Colour_Head where Colour_Name = '" & Trim(vColour_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vColour_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vColour_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Colour_NameToIdNo = Val(vColour_ID)

    End Function

    Public Shared Function Component_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vComponent_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vComponent_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Component_IdNo from Component_Head where Component_Name = '" & Trim(vComponent_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vComponent_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vComponent_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Component_NameToIdNo = Val(vComponent_ID)

    End Function

    Public Shared Function OrderNo_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vOrderNo_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vOrderNo_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select OrderNo_IdNo from OrderNo_Head where OrderNo_Name = '" & Trim(vOrderNo_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vOrderNo_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vOrderNo_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        OrderNo_NameToIdNo = Val(vOrderNo_ID)

    End Function

    Public Shared Function Colour_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vColour_ID As Integer) As String

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vColour_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head where Colour_IdNo = " & Str(Val(vColour_ID)), Cn1)
        Da.Fill(Dt)

        vColour_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vColour_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Colour_IdNoToName = Trim(vColour_Nm)

    End Function

    Public Shared Function Component_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vComponent_ID As Integer) As String

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vComponent_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Component_Name from Component_Head where Component_IdNo = " & Str(Val(vComponent_ID)), Cn1)
        Da.Fill(Dt)

        vComponent_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vComponent_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Component_IdNoToName = Trim(vComponent_Nm)

    End Function

    Public Shared Function OrderNo_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vOrderNo_ID As Integer) As String

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vOrderNo_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head where Colour_IdNo = " & Str(Val(vOrderNo_ID)), Cn1)
        Da.Fill(Dt)

        vOrderNo_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vOrderNo_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        OrderNo_IdNoToName = Trim(vOrderNo_Nm)

    End Function


    Public Shared Function Gender_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vGender_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vGender_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Gender_IdNo from Gender_Head where Gender_Name = '" & Trim(vGender_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vGender_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vGender_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Gender_NameToIdNo = Val(vGender_ID)

    End Function

    Public Shared Function Gender_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vGender_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vGender_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Gender_Name from Gender_Head where Gender_IdNo = " & Str(Val(vGender_ID)), Cn1)
        Da.Fill(Dt)

        vGender_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vGender_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Gender_IdNoToName = Trim(vGender_Nm)

    End Function
    Public Shared Function Style_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vStyle_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vStyle_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Style_IdNo from Style_Head where Style_Name = '" & Trim(vStyle_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vStyle_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vStyle_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Style_NameToIdNo = Val(vStyle_ID)

    End Function

    Public Shared Function Style_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vStyle_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vStyle_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Style_Name from Style_Head where Style_IdNo = " & Str(Val(vStyle_ID)), Cn1)
        Da.Fill(Dt)

        vStyle_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vStyle_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Style_IdNoToName = Trim(vStyle_Nm)

    End Function
    Public Shared Function Sleeve_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSleeve_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vSleeve_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Sleeve_IdNo from Sleeve_Head where Sleeve_Name = '" & Trim(vSleeve_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vSleeve_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vSleeve_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Sleeve_NameToIdNo = Val(vSleeve_ID)

    End Function

    Public Shared Function Sleeve_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSleeve_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vSleeve_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Sleeve_Name from Sleeve_Head where Sleeve_IdNo = " & Str(Val(vSleeve_ID)), Cn1)
        Da.Fill(Dt)

        vSleeve_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vSleeve_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Sleeve_IdNoToName = Trim(vSleeve_Nm)

    End Function
    Public Shared Function Design_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vDesign_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vDesign_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Design_IdNo from Design_Head where Design_Name = '" & Trim(vDesign_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vDesign_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vDesign_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Design_NameToIdNo = Val(vDesign_ID)

    End Function

    Public Shared Function Design_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vDesign_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vDesign_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Design_Name from Design_Head where Design_IdNo = " & Str(Val(vDesign_ID)), Cn1)
        Da.Fill(Dt)

        vDesign_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vDesign_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Design_IdNoToName = Trim(vDesign_Nm)

    End Function
    Public Shared Function Waste_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vWaste_Name As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vWaste_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Waste_IdNo from Waste_Head where Waste_Name = '" & Trim(vWaste_Name) & "'", Cn1)
        Da.Fill(Dt)

        vWaste_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vWaste_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Waste_NameToIdNo = Val(vWaste_ID)

    End Function

    Public Shared Function Waste_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vWaste_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vWaste_Name As String

        Da = New SqlClient.SqlDataAdapter("select Waste_Name from Waste_Head where Waste_IdNo = " & Str(Val(vWaste_ID)), Cn1)
        Da.Fill(Dt)

        vWaste_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vWaste_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Waste_IdNoToName = Trim(vWaste_Name)

    End Function

    Public Shared Function Transport_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vTransport_Nm As String) As Integer

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vTransport_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Transport_IdNo from Transport_Head where Transport_Name = '" & Trim(vTransport_Nm) & "'", Cn1)
        Da.Fill(Dt)

        vTransport_ID = 0

        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vTransport_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Transport_NameToIdNo = Val(vTransport_ID)

    End Function

    Public Shared Function Transport_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vTransport_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vTransport_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Transport_Name from Transport_Head where Transport_IdNo = " & Str(Val(vTransport_ID)), Cn1)
        Da.Fill(Dt)

        vTransport_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vTransport_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Transport_IdNoToName = Trim(vTransport_Nm)

    End Function

    Public Shared Function Size_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSize_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vSize_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Size_IdNo from Size_Head where Size_Name = '" & Trim(vSize_Name) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vSize_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vSize_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Size_NameToIdNo = Val(vSize_ID)

    End Function

    Public Shared Function Size_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSize_ID As Integer) As String

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vSize_Name As String

        Da = New SqlClient.SqlDataAdapter("select Size_Name from Size_Head where Size_IdNo = " & Str(Val(vSize_ID)), Cn1)
        Da.Fill(Dt)

        vSize_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vSize_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Size_IdNoToName = Trim(vSize_Name)

    End Function

    Public Shared Function get_FieldValue(ByVal Cn1 As SqlClient.SqlConnection, ByVal vTable_name As String, ByVal vField_Name As String, ByVal vCondition As String, Optional ByVal vCompany_ID As Integer = 0, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As String

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim RetVal As String
        Dim SqlCondt As String

        SqlCondt = ""

        Try

            If Trim(vCondition) <> "" Then
                SqlCondt = "(" & Trim(vCondition) & ")"
            End If

            If Val(vCompany_ID) <> 0 Then
                SqlCondt = Trim(SqlCondt) & IIf(Trim(SqlCondt) <> "", " and ", "") & " Company_IdNo = " & Str(Val(vCompany_ID))
            End If

            Da = New SqlClient.SqlDataAdapter("select " & vField_Name & " from " & vTable_name & IIf(Trim(SqlCondt) <> "", " Where ", "") & SqlCondt, Cn1)



            If IsNothing(sqltr) = False Then
                Da.SelectCommand.Transaction = sqltr
            End If
            Da.Fill(Dt)

            RetVal = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    RetVal = Dt.Rows(0)(0).ToString
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            get_FieldValue = RetVal


        Catch ex As Exception

            'MsgBox("select " & vField_Name & " from " & vTable_name & IIf(Trim(SqlCondt) <> "", " Where ", "") & SqlCondt)

        End Try

    End Function


    Public Shared Function get_MaxIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vTable_name As String, ByVal vField_name As String, ByVal vCondition As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim MxId As Integer

        Da = New SqlClient.SqlDataAdapter("select max(" & vField_name & ") from " & vTable_name & IIf(Trim(vCondition) <> "", " Where ", "") & vCondition, Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        MxId = 0

        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                MxId = Val(Dt.Rows(0)(0).ToString)
            End If
        End If
        MxId = MxId + 1

        Dt.Dispose()
        Da.Dispose()

        get_MaxIdNo = Val(MxId)

    End Function

    Public Shared Function get_MaxCode(ByVal Cn1 As SqlClient.SqlConnection, ByVal vTable_name As String, ByVal vPK_Fieldname As String, ByVal vOrderBy_Fieldname As String, ByVal vCondition As String, ByVal vCompany_ID As Integer, ByVal vFinYr As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As String

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim MxId As Integer
        Dim SqlCondt As String

        SqlCondt = ""
        If Trim(vCondition) <> "" Then
            SqlCondt = "(" & Trim(vCondition) & ")"
        End If
        If Val(vCompany_ID) <> 0 Then
            SqlCondt = Trim(SqlCondt) & IIf(Trim(SqlCondt) <> "", " and ", "") & " Company_IdNo = " & Str(Val(vCompany_ID))
        End If

        If Trim(vFinYr) <> "" Then
            SqlCondt = Trim(SqlCondt) & IIf(Trim(SqlCondt) <> "", " and ", "") & " " & vPK_Fieldname & " like '%" & Trim(vFinYr) & "'"
        End If

        Da = New SqlClient.SqlDataAdapter("select max(" & vOrderBy_Fieldname & ") from " & vTable_name & IIf(Trim(SqlCondt) <> "", " Where ", "") & SqlCondt, Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        MxId = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                MxId = Int(Val(Dt.Rows(0)(0).ToString))
            End If
        End If
        MxId = MxId + 1

        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        get_MaxCode = Trim(Val(MxId))

    End Function

    Public Shared Function get_Item_CurrentStock(ByVal Cn1 As SqlClient.SqlConnection, ByVal vComp_IdNo As Integer, ByVal vItem_IdNo As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Decimal
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CurStk As Decimal = 0

        Da = New SqlClient.SqlDataAdapter("select sum(Quantity) from Item_Processing_Details where Company_IdNo = " & Str(Val(vComp_IdNo)) & " and Item_IdNo = " & Str(Val(vItem_IdNo)), Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Dt = New DataTable
        Da.Fill(Dt)

        CurStk = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                CurStk = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        get_Item_CurrentStock = Val(CurStk)

    End Function

    Public Shared Sub Default_GroupHead_Updation(ByVal Cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand

        cmd.Connection = Cn1

        cmd.CommandText = "delete from AccountsGroup_Head where AccountsGroup_IdNo <= 30"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (0, '',          '',       '',    0, '',       0,  0,      '',                         ''      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (1, 'BRANCH / DIVISION',          'BRANCHDIVISION',       'BRANCH / DIVISION',    1, '~1~',       0,  7,      '',                         'SUBSIDIARY FIRMS'      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (2, 'CAPITAL ACCOUNT',            'CAPITALACCOUNT',       'CAPITAL ACCOUNT',      1,  '~2~',      0,  1,      '',                         'EQUITY'                )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (3, 'RESERVESE & SURPLUS',        'RESERVESESURPLUS',     'CAPITAL ACCOUNT',      1,  '~3~2~',    0,  1.1,    '',                         'RETAINED EARNINGS'     )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (4, 'CURRENT ASSETS',             'CURRENTASSETS',        'CURRENT ASSETS',       1,  '~4~',      0,  6,      'CURRENT ASSETS',           ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (5, 'BANK ACCOUNTS',              'BANKACCOUNTS',         'CURRENT ASSETS',       1,  '~5~4~',    0,  6.7,    'CURRENT ASSETS',           ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (6, 'CASH-IN-HAND',               'CASHINHAND',           'CURRENT ASSETS',       1,  '~6~4~',    0,  6.6,    'CURRENT ASSETS',           ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (7, 'DEPOSITS (ASSET)',           'DEPOSITSASSET',        'CURRENT ASSETS',       1,  '~7~4~',    0,  6.2,    'CURRENT ASSETS',           ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (8, 'LOANS & ADVANCES (ASSET)',   'LOANSADVANCESASSET',   'CURRENT ASSETS',       1,  '~8~4~',    0,  6.3,    'CURRENT ASSETS',           ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (9, 'STOCK-IN-HAND',              'STOCKINHAND',          'CURRENT ASSETS',       1,  '~9~4~',    0,  6.1,    'CURRENT ASSETS',           ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (10, 'SUNDRY DEBTORS',            'SUNDRYDEBTORS',        'CURRENT ASSETS',       1,  '~10~4~',   0,  6.5,    'CURRENT ASSETS',           'ACCOUNTS RECEIVABLE'   )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (11, 'CURRENT LIABILITIES',       'CURRENTLIABILITIES',   'CURRENT LIABILITIES',  1,  '~11~',     0,  3,      'CURRENT LIABILITIES',      ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (12, 'DUTIES & TAXES',            'DUTIESTAXES',          'CURRENT LIABILITIES',  1,  '~12~11~',  0,  3.2,    'CURRENT LIABILITIES',      ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (13, 'PROVISIONS',                'PROVISIONS',           'CURRENT LIABILITIES',  1,  '~13~11~',  0,  3.3,    'CURRENT LIABILITIES',      ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (14, 'SUNDRY CREDITORS',          'SUNDRYCREDITORS',      'CURRENT LIABILITIES',  1,  '~14~11~',  0,  3.4,    'CURRENT LIABILITIES',      'ACCOUNTS PAYABLE'      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (15, 'EXPENSES (DIRECT)',         'EXPENSESDIRECT',       'EXPENSES (DIRECT)',    1,  '~15~18~',  1,  13,     'EXPENDITURE ACCOUNT',      'MFG./TRDG. EXPENSES'   )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (16, 'EXPENSES (INDIRECT)',       'EXPENSESINDIRECT',     'EXPENSES (INDIRECT)',  1,  '~16~18~',  1,  15,     'EXPENDITURE ACCOUNT',      'ADMIN. EXPENSES'       )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (17, 'FIXED ASSETS',              'FIXEDASSETS',          'FIXED ASSETS',         1,  '~17~',     0,  4,      '',                         'IMMOVABLE PROPERTIES'  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (18, 'REVENUE ACCOUNTS',          'REVENUEACCOUNTS',      'REVENUE ACCOUNTS',     1,  '~18~',     0,  18,     '',                         ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (19, 'INCOME (REVENUE)',          'INCOMEREVENUE',        'INCOME (REVENUE)',     1,  '~19~18~',  1,  12,     'REVENUE ACCOUNTS',         ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (20, 'INVESTMENTS',               'INVESTMENTS',          'INVESTMENTS',          1,  '~20~',     0,  5,      '',                         ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (21, 'LOANS (LIABILITY)',         'LOANSLIABILITY',       'LOANS (LIABILITY)',    1,  '~21~',     0,  2,      '',                         ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (23, 'BANK OCC A/C',              'BANKOCCAC',            'LOANS (LIABILITY)',    1,  '~23~21~',  0,  2.1,    'LOANS (LIABILITY)',        ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (24, 'SECURED LOANS',             'SECUREDLOANS',         'LOANS (LIABILITY)',    1,  '~24~21~',  0,  2.2,    'LOANS (LIABILITY)',        ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (25, 'UNSECURED LOANS',           'UNSECUREDLOANS',       'LOANS (LIABILITY)',    1,  '~25~21~',  0,  2.3,    'LOANS (LIABILITY)',        ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (26, 'MISC.EXPENSES (ASSET)',     'MISCEXPENSESASSET',    'MISC.EXPENSES (ASSET)',1,  '~26~',     0,  8,      'Misc Expenses (ASSET)',    'Misc Expenses (ASSET)' )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (27, 'PURCHASE ACCOUNT',          'PURCHASEACCOUNT',      'PURCHASE ACCOUNT',     1,  '~27~18~',  1,  11,     'REVENUE ACCOUNTS',         ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (28, 'SALES ACCOUNT',             'SALESACCOUNT',         'SALES ACCOUNT',        1,  '~28~18~',  1,  10,     'REVENUE ACCOUNTS',         ''                      )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (29, 'SUSPENSE ACCOUNT',          'SUSPENSEACCOUNT',      'SUSPENSE ACCOUNT',     1,  '~29~',     0,  9,      '',                         'TEMPORARY A/CS'        )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (30, 'PROFIT & LOSS A/C',         'PROFITLOSSAC',         'PROFIT & LOSS A/C',    1,  '~30~',     0,  16,     'Profit & Loss A/c',        'Profit & Loss Account' )"
        cmd.ExecuteNonQuery()

    End Sub

    Public Shared Sub Default_LedgerHead_Updation(ByVal Cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0

        cmd.Connection = Cn1

        cmd.CommandText = "delete from Ledger_Head where Ledger_IdNo <= 100"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (0,     '',                       '',                 '',                     '',         0,      0,      '',         '',                 '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (1,     'CASH A/C',               'CASHAC',           'CASH A/C',             '',         0,      6,      '~6~4~',    'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (4,     'GODOWN',                 'GODOWN',           'GODOWN',               '',         0,      9,      '~9~4~',    'BALANCE ONLY',     'GODOWN', '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (8,     'TDS CHARGES',            'TDSCHARGES',       'TDS CHARGES',          '',         0,      12,     '~12~11~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()

        If Trim(Common_Procedures.settings.CustomerCode) = "1011" Then
            cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (9,    'BATTERY CHARGING A/C',    'BATTERYCHARGINGAC', 'BATTERY CHARGING A/C', '',         0,      19,     '~19~18~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
            cmd.ExecuteNonQuery()
        Else
            cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (9,     'FREIGHT CHARGES',        'FREIGHTCHARGES',   'FREIGHT CHARGES',       '',         0,      16,     '~16~18~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
            cmd.ExecuteNonQuery()
        End If

        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (10,    'SALARY A/C',                 'SALARYAC',             'SALARY A/C',               '',         0,      15,     '~15~18~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (12,    'STOCK-IN-HAND',          'STOCKINHAND',      'STOCK-IN-HAND',        '',         0,      9,      '~9~4~',    'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (13,    'PROFIT & LOSS A/C',      'PROFITLOSSAC',     'PROFIT & LOSS A/C',    '',         0,      30,     '~30~',     'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (17,    'DISCOUNT A/C',           'DISCOUNTAC',       'DISCOUNT A/C',         '',         0,      16,     '~16~18~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (20,    'VAT A/C',                'VATAC',            'VAT A/C',              '',         0,      12,     '~12~11~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (21,    'PURCHASE A/C',           'PURCHASEAC',       'PURCHASE A/C',         '',         0,      27,     '~27~18~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (22,    'SALES A/C',              'SALESAC',          'SALES A/C',            '',         0,      28,     '~28~18~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (23,    'CESS A/C',                'CESSAC',          'CESS A/C',             '',         0,      12,     '~12~11~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (24,    'ROUNDOFF A/C',           'ROUNDOFFAC',       'ROUNDOFF A/C',         '',         0,      16,     '~16~18~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (25,    'CGST A/C',                'CGSTAC',            'CGST A/C',              '',         0,      12,     '~12~11~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (26,    'SGST A/C',                'SGSTAC',            'SGST A/C',              '',         0,      12,     '~12~11~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (27,    'IGST A/C',                'IGSTAC',            'IGST A/C',              '',         0,      12,     '~12~11~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1193" Then  '--- BIKE STAND
            cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (28,    'SERVICE REVENUE A/C',                'SERVICEREVENUEAC',            'SERVICE REVENUE A/C',              '',         0,      19,     '~19~18~',  'BALANCE ONLY',     '',       '',     '',     '',     '',     '',     ''  )"
            cmd.ExecuteNonQuery()
        End If


        cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo <= 100"
        cmd.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head where Ledger_IdNo <= 100", Cn1)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For i = 0 To Dt1.Rows.Count - 1
                cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo ) Values (" & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ",    1,      '" & Trim(Dt1.Rows(i).Item("Ledger_Name").ToString) & "',   '" & Trim(Dt1.Rows(i).Item("Ledger_Type").ToString) & "',    " & Str(Val(Dt1.Rows(i).Item("AccountsGroup_IdNo").ToString)) & ")"
                cmd.ExecuteNonQuery()
            Next

        End If

        cmd.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

    End Sub

    Public Shared Sub Default_MonthHead_Updation(ByVal Cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand

        cmd.Connection = Cn1

        cmd.CommandText = "delete from Month_Head"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (0,     '',                '',         0)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (4,     'APRIL',           'APR',      1)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (5,     'MAY',             'MAY',      2)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (6,     'JUNE',            'JUN',      3)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (7,     'JULY',            'JUL',      4)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (8,     'AUGUST',          'AUG',      5)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (9,     'SEPTEMBER',       'SEP',      6)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (10,     'OCTOBER',        'OCT',      7)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (11,     'NOVEMBER',       'NOV',      8)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (12,     'DECEMBER',       'DEC',      9)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (1,     'JANUARY',         'JAN',      10)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (2,     'FEBRUARY',        'FEB',      11)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Month_Head(Month_IdNo, Month_Name, Month_ShortName, Idno ) Values (3,     'MARCH',           'MAR',      12)"
        cmd.ExecuteNonQuery()

    End Sub

    Public Shared Sub Default_StateHead_Updation(ByVal Cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand

        cmd.Connection = Cn1

        cmd.CommandText = "delete from State_Head"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (0,     '',                '',         0, '')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (1,     'TAMIL NADU'         ,    'TAMILNADU'      ,      0,  '33')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (2,     'ANDHRA PRADESH'     ,    'ANDHRAPRADESH'  ,      1,  28)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (3,     'ARUNACHAL PRADESH'  ,    'ARUNACHALPRADESH',      1,  12)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (4,     'ASSAM'              ,    'ASSAM'           ,      1,  18)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (5,     'BIHAR'              ,    'BIHAR'           ,      1,  10)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (6,     'CHHATTISGARH '      ,    'CHHATTISGARH'    ,      1,  04)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (7,     'GOA'                ,    'GOA'             ,      1,  30)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (8,     'GUJARAT'            ,    'GUJARAT'         ,      1,  24)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (9,     'HARYANA '           ,    'HARYANA'         ,      1,  06)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (10,     'HIMACHAL PRADESH'  ,    'HIMACHALPRADESH',      1,  02)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (11,     'JAMMU AND KASHMIR' ,    'JAMMUANDKASHMIR' ,      1, 01)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (12,     'JHARKHAND'         ,    'JHARKHAND'       ,      1, 20)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (13,     'KARNATAKA'         ,    'KARNATAKA'       ,     1, 29)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (14,     'KERALA'            ,    'KERALA'          ,     1, 32)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (15,     'MADHYA PRADESH'    ,    'MADHYAPRADESH'   ,      1, 23)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (16,     'MAHARASHTRA'       ,    'MAHARASHTRA'     ,      1, 27)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (17,     'MANIPUR'           ,    'MANIPUR'         ,      1, 14)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (18,     'MEGHALAYA'         ,    'MEGHALAYA'       ,      1, 17)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (19,     'MIZORAM'           ,    'MIZORAM'         ,     1, 15)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (20,     'NAGALAND'          ,    'NAGALAND'        ,      1, 13)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (21,     'ODISHA'            ,    'ODISHA'          ,      1, 21)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (22,     'PUNJAB'            ,    'PUNJAB'          ,      1, 03)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (23,     'RAJASTHAN'         ,    'RAJASTHAN'       ,      1, 08)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (24,     'SIKKIM'            ,    'SIKKIM'          ,      1, 11)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (25,     'TELANGANA'         ,    'TELANGANA'       ,      1, 36)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (26,     'TRIPURA'           ,    'TRIPURA'         ,      1, 16)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (27,     'UTTAR PRADESH'     ,    'UTTARPRADESH'   ,      1, 09)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (28,     'UTTARAKHAND'       ,    'UTTARAKHAND'     ,      1, 05)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (29,     'WEST BENGAL'       ,    'WESTBENGAL'     ,      1, 19)"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (30,     'PUDUCHERRY'         ,    'PUDUCHERRY'       ,      1, 34)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (31,     'NEW DELHI'        ,    'NEWDELHI'         ,      1,  07)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (32,     'LAKSHADWEEPH'     ,    'LAKSHADWEEPH'   ,      1, 31)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (33,     'ANDAMAN AND NICOBAR ISLANDS'       ,   'ANDAMANANDNICOBARISLANDS'    ,      1, 35)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code  ) Values (34,     'CHANDIGARH'                   ,   'CHANDIGARH'                  ,      1,  04)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code ) Values (35,     'DADRA AND NAGAR HAVELI'       ,    'DADRAANDNAGARHAVELI'        ,     1,  26)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into State_Head(State_IdNo, State_Name, Sur_Name, Cst_Value, State_Code ) Values (36,    'DAMAN AND DIU'                ,    'DAMANANDDIU'                ,  1,    25)"
        cmd.ExecuteNonQuery()


    End Sub

    Public Shared Sub Default_Shift_Updation(ByVal Cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand

        cmd.Connection = Cn1

        cmd.CommandText = "delete from Shift_Head"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into Shift_Head(Shift_IdNo, Shift_Name) Values (0, '')"
        cmd.ExecuteNonQuery()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then
            cmd.CommandText = "Insert into Shift_Head(Shift_IdNo, Shift_Name) Values (1, 'DAY')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Shift_Head(Shift_IdNo, Shift_Name) Values (2, 'NIGHT')"
            cmd.ExecuteNonQuery()

        Else

            cmd.CommandText = "Insert into Shift_Head(Shift_IdNo, Shift_Name) Values (1, '1ST SHIFT')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Shift_Head(Shift_IdNo, Shift_Name) Values (2, '2ND SHIFT')"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Shift_Head(Shift_IdNo, Shift_Name) Values (3, '3RD SHIFT')"
            cmd.ExecuteNonQuery()

        End If



    End Sub


    Public Shared Sub Default_Master_Updation(ByVal Cn1 As SqlClient.SqlConnection)
        Dim Cn2 As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand

        On Error Resume Next

        If Trim(Common_Procedures.ConnectionString_CompanyGroupdetails) <> "" Then

            Cn2 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

            Cn2.Open()

            cmd.Connection = Cn2

            cmd.CommandText = "Delete from User_Head where user_idno = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password) values (0, '', '', '', '') "
            cmd.ExecuteNonQuery()

            Cn2.Close()
            Cn2.Dispose()

        End If

        cmd.Connection = Cn1

        cmd.CommandText = "delete from Ledger_Head where Ledger_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_Head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_TinNo, Ledger_CstNo ) Values (0,   '',   '',   '',     '',     0,      0,      '',     '',     '',     '',     '',     '',     '',     '',     ''  )"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName) Values (0,      0,     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Ledger_PhoneNo_Head where Ledger_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Ledger_PhoneNo_Head(Ledger_IdNo, Sl_No, Ledger_PhoneNo) Values (0,      0,     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from AccountsGroup_Head where AccountsGroup_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into AccountsGroup_Head(AccountsGroup_IdNo, AccountsGroup_Name, Sur_Name, Parent_Name, Indicate, Parent_Idno, Carried_Balance, Order_Position, TallyName, TallySubName ) Values (0, '',          '',       '',    0, '',       0,  0,      '',                         ''      )"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Company_Head where Company_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Company_Head(Company_IdNo, Company_Name, Company_SurName, Company_ShortName, Company_Address1, Company_Address2, Company_Address3, Company_Address4, Company_City, Company_PinCode, Company_PhoneNo, Company_TinNo, Company_CstNo, Company_FaxNo, Company_EMail, Company_ContactPerson, Company_Description) Values (0,      '',     '',     '',     '',     '',     '',     '',     '',     '',     '',     '',     '',     '',     '',     '',     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Item_Head where Item_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Item_Head(Item_IdNo, Item_Name, Sur_Name, Item_Code, ItemGroup_IdNo, Unit_IdNo, Tax_Percentage, Sale_TaxRate, Sales_Rate, Cost_Rate, Minimum_Stock) Values (0,   '',     '',     '',     0,     0,     0,     0,     0,     0,     0)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from ItemGroup_Head where ItemGroup_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into ItemGroup_Head(ItemGroup_IdNo, ItemGroup_Name, Sur_Name) Values (0,   '',     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Unit_Head where Unit_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Unit_Head(Unit_IdNo, Unit_Name, Sur_Name) Values (0,   '',     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Cetegory_Head where Cetegory_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Cetegory_Head(Cetegory_IdNo, Cetegory_Name, Sur_Name) Values (0,   '',     '')"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "delete from Variety_Head where Variety_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Variety_Head(Variety_IdNo, Variety_Name, Sur_Name) Values (0,   '',     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Area_Head where Area_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Area_Head(Area_IdNo, Area_Name, Sur_Name) Values (0,   '',     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Waste_Head where Waste_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Waste_Head(Waste_IdNo, Waste_Name, Sur_Name) Values (0,   '',     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Size_Head where Size_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Size_Head(Size_IdNo, Size_Name, Sur_Name) Values (0,   '',     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Transport_Head where Transport_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Transport_Head(Transport_IdNo, Transport_Name, Sur_Name) Values (0,   '',     '')"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Shift_Head where Shift_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Shift_Head ( Shift_IdNo , Shift_Name ) Values (0,   '' )"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Price_List_Head where Price_List_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Price_List_Head ( Price_List_IdNo , Price_List_Name, sur_name) Values (0,   '', '' )"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Machine_Head where Machine_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Machine_Head ( Machine_IdNo , Machine_Name, sur_name) Values (0,   '', '' )"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Tax_Head where Tax_IdNo = 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into Tax_Head(Tax_IdNo, Tax_Name, Sur_Name, Tax_Ledger_Ac_IdNo) Values (0,   '',     '', 0)"
        cmd.ExecuteNonQuery()

    End Sub

    Public Shared Sub Sql_AutoBackUP(ByVal Db_Name As String)
        Dim cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim Fl_Name As String, Fl_Name2 As String
        Dim Fl_Name3 As String
        Dim ServrNm As String = ""
        Dim ServrPath As String = ""

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        cn1.Open()

        ServrNm = Common_Procedures.get_Server_SystemName()
        If ServrNm = Trim(UCase(SystemInformation.ComputerName)) Then

            Fl_Name = Common_Procedures.AppPath & "\Auto_BackUP"
            Fl_Name3 = Common_Procedures.AppPath & "\Auto_BackUP"
        Else
            ServrPath = Trim(Common_Procedures.get_FieldValue(cn1, "Settings_Head", "Autobackup_Path_Server", ""))

            If ServrPath = "" Then Exit Sub

            Fl_Name = Trim(ServrPath) & "\Auto_BackUP"
            Fl_Name3 = Trim(ServrPath) & "\Auto_BackUP"

        End If

        If System.IO.Directory.Exists(Fl_Name) = False Then
            System.IO.Directory.CreateDirectory(Fl_Name)
        End If

        Fl_Name3 = Trim(Fl_Name) & "\" & Trim(Db_Name) & "_" & Trim(Format(Date.Today, "ddMMMyy")) & ".tssl"
        Fl_Name = Trim(Fl_Name) & "\" & Trim(Db_Name) & "_" & Trim(Format(Date.Today, "ddMMMyy"))  ' & ".bak"

        cmd.Connection = cn1

        cmd.CommandText = "BACKUP DATABASE " & Trim(Db_Name) & " TO DISK = '" & Trim(Fl_Name) & "' WITH INIT"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "BACKUP DATABASE " & Trim(Db_Name) & " TO DISK = '" & Trim(Fl_Name3) & "' WITH INIT"
        cmd.ExecuteNonQuery()

        cmd.Dispose()

        cn1.Close()
        cn1.Dispose()

        Sql_AutoBackUP_File_To_Client_Sysytem(Fl_Name, Trim(Db_Name), ServrNm)

        Dim allDrives() As DriveInfo = DriveInfo.GetDrives()
        Dim d As DriveInfo

        For Each d In allDrives

            If d.IsReady = True Then

                If d.DriveType = DriveType.Removable Then

                    Fl_Name2 = Trim(d.Name) & "TSOFT\Auto_BackUP"

                    If System.IO.Directory.Exists(Fl_Name2) = True Then

                        Fl_Name2 = Trim(Fl_Name2) & "\" & Trim(Db_Name) & "_" & Trim(Format(Date.Today, "ddMMMyy")) & ".bak"

                        System.IO.File.Copy(Fl_Name, Fl_Name2, True)

                        Exit For

                    End If

                End If

            End If

        Next

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then
            For Each d In allDrives

                If d.IsReady = True Then

                    If d.DriveType = DriveType.Fixed Then

                        Fl_Name2 = "D:\TSOFT\Auto_BackUP"

                        If System.IO.Directory.Exists(Fl_Name2) = True Then

                            Fl_Name2 = Trim(Fl_Name2) & "\" & Trim(Db_Name) & "_" & Trim(Format(Date.Today, "ddMMMyy")) & ".bak"

                            System.IO.File.Copy(Fl_Name, Fl_Name2, True)

                            Exit For

                        End If

                    End If

                End If

            Next
        End If


    End Sub

    Public Shared Sub Sql_AutoBackUP_File_To_Client_Sysytem(ByVal File_Name As String, ByVal Db_Name As String, ByVal Servnam As String)
        Dim cn1 As SqlClient.SqlConnection
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Path1 As String = ""
        Dim Path2 As String = ""

        Try
            Common_Procedures.Sql_AutoBackUP_Client_Path()

            cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
            cn1.Open()

            Da = New SqlClient.SqlDataAdapter("select * from AutoBackup_Path_Head  order by Auto_SlNo asc ", cn1)
            Da.Fill(Dt)
            If Dt.Rows.Count > 0 Then
                For i = 0 To Dt.Rows.Count - 1
                    If IsDBNull(Dt.Rows(0).Item("App_Path").ToString) = False Then
                        If Trim(Dt.Rows(0).Item("App_Path").ToString) <> "" Then

                            If Directory.Exists(Trim(Dt.Rows(0).Item("App_Path").ToString)) Then
                                'System.IO.File.Copy(File_Name, Trim(Dt.Rows(0).Item("App_Path").ToString) & "\" & Trim(Db_Name) & "_" & Trim(Format(Date.Today, "ddMMMyy")))
                                System.IO.File.Copy(File_Name, Trim(Dt.Rows(0).Item("App_Path").ToString) & "\" & Trim(Db_Name) & "_" & Trim(Format(Date.Today, "ddMMMyy")) & ".tssql")

                                'Directory.CreateDirectory(Path1)
                            End If

                        End If

                    End If
                Next

            End If

            Dt.Dispose()
            Da.Dispose()

            cn1.Close()
            cn1.Dispose()


        Catch ex As Exception
            '----
        End Try


    End Sub
    Public Shared Sub Sql_AutoBackUP_Client_Path()
        Dim cmd As New SqlClient.SqlCommand
        Dim cn1 As SqlClient.SqlConnection
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim ServrNm As String = ""
        Dim Path_Sts As Boolean = False
        Dim Client_Count As Integer = 0
        Dim Nr As Integer = 0
        Dim Path1 As String = ""
        Try
            ServrNm = Common_Procedures.get_Server_SystemName()
            If ServrNm = Trim(UCase(SystemInformation.ComputerName)) Then
                Exit Sub
            End If

            cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
            cn1.Open()
            cmd.Connection = cn1

            Path1 = Replace(Trim(Common_Procedures.AppPath & "\Auto_BackUP"), ":", "")
            Path1 = "\\" & Trim(UCase(SystemInformation.ComputerName)) & "\" & Trim(Path1)

            If Not Directory.Exists(Path1) Then
                Directory.CreateDirectory(Path1)
            End If

            Da = New SqlClient.SqlDataAdapter("select top 1 * from AutoBackup_Path_Head where Computer_Name = '" & Trim(Trim(UCase(SystemInformation.ComputerName))) & "' order by Auto_SlNo asc ", cn1)
            Da.Fill(Dt)
            If Dt.Rows.Count > 0 Then

                Nr = 0
                cmd.CommandText = "update AutoBackup_Path_Head set App_Path = '" & Trim(Path1) & "' where Computer_Name = '" & Trim(Trim(UCase(SystemInformation.ComputerName))) & "'"
                Nr = cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Insert into AutoBackup_Path_Head(Computer_Name,App_Path) Values ('" & Trim(Trim(UCase(SystemInformation.ComputerName))) & "' ,'" & Trim(Path1) & "' )"
                cmd.ExecuteNonQuery()

            End If
            Dt.Dispose()
            Da.Dispose()


            cn1.Close()
            cn1.Dispose()

        Catch ex As Exception
            '----
        End Try


    End Sub
    Public Shared Function get_Company_From_CompanySelection(ByVal Cn1 As SqlClient.SqlConnection) As String

        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String
        Dim CompNm As String

        CompNm = ""
        Common_Procedures.CompIdNo = 0

        Try

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "(Company_Type <> 'UNACCOUNT')"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", Cn1)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) = 1 Then

                da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", Cn1)
                dt1 = New DataTable
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                        CompNm = Trim(dt1.Rows(0)(1).ToString)
                    End If
                End If
                dt1.Clear()

            Else

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), Cn1)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                            CompNm = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()

                Else
                    MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Common_Procedures.CompIdNo = 0
                    get_Company_From_CompanySelection = ""
                    Exit Function

                End If

            End If



        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        get_Company_From_CompanySelection = Trim(CompNm)

    End Function


    Public Shared Function Show_CompanySelection_On_FormClose(ByVal Cn1 As SqlClient.SqlConnection) As String
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String
        Dim CompNm As String

        CompNm = ""
        Common_Procedures.CompIdNo = 0

        Try

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "(Company_Type <> 'UNACCOUNT')"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", Cn1)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), Cn1)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                            CompNm = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()

                End If

            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        Show_CompanySelection_On_FormClose = Trim(CompNm)

    End Function

    Public Shared Function UserRight_Check(ByVal User_Access_Type As String, ByVal NewEntry_Status As Boolean) As Boolean

        UserRight_Check = True

        If Val(Common_Procedures.User.IdNo) <> 1 Then
            If InStr(Trim(UCase(User_Access_Type)), "~L~") = 0 Then
                If NewEntry_Status = True Then
                    If InStr(Trim(UCase(User_Access_Type)), "~A~") = 0 Then
                        MessageBox.Show("You have No Rights to Add", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        UserRight_Check = False
                    End If

                Else
                    If InStr(Trim(UCase(User_Access_Type)), "~E~") = 0 Then
                        MessageBox.Show("You have No Rights to Change", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        UserRight_Check = False
                    End If

                End If
            End If
        End If

    End Function

    Public Shared Function UserRight_Check_1(ByVal FormName As String, ByVal RequestedOperation As OperationType) As Boolean

        If User.IdNo = 1 Then
            UserRight_Check_1 = True
            Exit Function
        End If

        UserRight_Check_1 = False

        Dim Op_Code As String

        Select Case RequestedOperation

            Case OperationType.Open
                Op_Code = "O"
            Case OperationType.AddNew
                Op_Code = "A"
            Case OperationType.Edit
                Op_Code = "E"
            Case OperationType.Delete
                Op_Code = "D"
            Case OperationType.View
                Op_Code = "V"
            Case OperationType.Insert
                Op_Code = "I"

        End Select


        For I As Integer = 0 To UR1.UserInfo.GetUpperBound(0)

            If UCase(UR1.UserInfo(I, 2)) = UCase(FormName) Then

                If Op_Code = "O" Then

                    If Len(Trim(UR1.UserInfo(I, 1))) > 0 Then

                        UserRight_Check_1 = True
                        Exit Function

                    End If
                Else

                    If InStr(UCase(UR1.UserInfo(I, 1)), Op_Code) > 0 Or InStr(UCase(UR1.UserInfo(I, 1)), "L") > 0 Then

                        UserRight_Check_1 = True
                        Exit Function
                    End If

                End If

                Exit Function

            End If

        Next

    End Function

    Public Shared Sub ComboBox_ItemSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal Cn1 As SqlClient.SqlConnection, ByVal CboName As ComboBox, ByVal NextCtrlName As Object, ByVal vTableName As String, ByVal vSelectionFieldName As String, ByVal vSqlCondition As String, ByVal vBlankFieldCondition As String, Optional ByVal vBlock_Typing_Status As Boolean = True, Optional ByVal UPPERCASE As Boolean = True)
        Dim da As New SqlClient.SqlDataAdapter
        Dim Cmd As New SqlClient.SqlCommand
        Dim dt As New DataTable
        Dim SqlCondt As String, Condt2 As String
        Dim FindStr As String = ""
        Dim indx As Integer = -1
        Dim SelStrt As Integer = 0
        Dim Mtch_STS As Boolean = False


        Try

            With CboName

                If Asc(e.KeyChar) <> 27 Then

                    SelStrt = .SelectionStart

                    If Asc(e.KeyChar) = 13 Then

                        Try

                            If Trim(.Text) <> "" Then

                                If .DroppedDown = True Then

                                    If .Items.Count > 0 Then

                                        indx = .FindString(FindStr)

                                        If indx <> -1 Then

                                            If .SelectedIndex >= 0 Then
                                                .SelectedItem = .Items(.SelectedIndex)
                                                If UPPERCASE = True Then
                                                    .Text = UCase(.GetItemText(.SelectedItem))
                                                Else
                                                    .Text = .GetItemText(.SelectedItem)
                                                End If

                                            Else

                                                If Trim(vTableName) <> "" And Trim(vSelectionFieldName) <> "" Then
                                                    .SelectedIndex = 0
                                                    .SelectedItem = .Items(0)
                                                    If UPPERCASE = True Then
                                                        .Text = UCase(.GetItemText(.SelectedItem))
                                                    Else
                                                        .Text = .GetItemText(.SelectedItem)
                                                    End If

                                                End If

                                            End If

                                        End If

                                    End If

                                End If

                            End If

                        Catch ex As Exception
                            '---

                        End Try


                        If IsNothing(NextCtrlName) = False Then
                            If NextCtrlName.Enabled Then
                                NextCtrlName.Focus()

                            Else
                                SendKeys.Send("{TAB}")

                            End If
                        End If

                    Else

                        SqlCondt = ""
                        Condt2 = ""
                        FindStr = ""
                        indx = -1

                        If Asc(e.KeyChar) = 8 Then

                            If Trim(.Text) <> "" Then

                                If .SelectionLength = 0 Then
                                    If .SelectionStart > 1 Then
                                        FindStr = .Text.Substring(0, .SelectionStart - 1)
                                    End If
                                    FindStr = FindStr & Mid(CboName.Text, CboName.SelectionStart + 1, Len(CboName.Text))

                                Else

                                    If .SelectionStart <= 1 Then
                                        .Text = ""
                                    Else
                                        FindStr = .Text.Substring(0, .SelectionStart - 1)
                                    End If

                                End If

                            End If

                        Else

                            If .SelectionLength = 0 Then
                                If .SelectionStart > 0 Then FindStr = .Text.Substring(0, .SelectionStart)
                                FindStr = FindStr & e.KeyChar & Mid(CboName.Text, CboName.SelectionStart + 1, Len(CboName.Text))

                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar

                            End If

                        End If

                        FindStr = LTrim(FindStr)



                        If Trim(vTableName) <> "" Then

                            indx = .FindString(FindStr)

                            SqlCondt = ""
                            If Trim(FindStr) <> "" Then
                                SqlCondt = " Where " & vSqlCondition & IIf(Trim(vSqlCondition) <> "", " and ", "") & " (" & vSelectionFieldName & " like '" & FindStr & "%' or " & vSelectionFieldName & " like '% " & FindStr & "%' or " & vSelectionFieldName & " like '% (" & FindStr & "%' or " & vSelectionFieldName & " like '(" & FindStr & "%' or " & vSelectionFieldName & " like '% {" & FindStr & "%' or " & vSelectionFieldName & " like '{" & FindStr & "%'   or " & vSelectionFieldName & " like '% [" & FindStr & "%' or " & vSelectionFieldName & " like '[" & FindStr & "%')"

                            Else

                                Condt2 = ""
                                If Trim(vSqlCondition) <> "" Then
                                    Condt2 = Trim(vSqlCondition)
                                    If Trim(vBlankFieldCondition) <> "" Then Condt2 = Condt2 & IIf(Trim(Condt2) <> "", " or ", "") & vBlankFieldCondition
                                End If

                                If Trim(Condt2) <> "" Then
                                    SqlCondt = " Where " & Trim(Condt2)
                                End If

                            End If

                            Mtch_STS = False
                            da = New SqlClient.SqlDataAdapter("select distinct(" & vSelectionFieldName & ") from " & vTableName & " " & SqlCondt & " order by " & vSelectionFieldName, Cn1)
                            dt = New DataTable
                            da.Fill(dt)
                            If dt.Rows.Count > 0 Then
                                Mtch_STS = True
                            End If

                            If Mtch_STS = True Then

                                da = New SqlClient.SqlDataAdapter("Select distinct(" & vSelectionFieldName & ") from " & vTableName & " " & SqlCondt & " order by " & vSelectionFieldName, Cn1)
                                dt = New DataTable
                                da.Fill(dt)
                                .DataSource = dt
                                .DisplayMember = Trim(vSelectionFieldName)

                                If .Items.Count > 0 Then
                                    If Asc(e.KeyChar) = 32 And Len(FindStr) = 0 Then .DroppedDown = False
                                    .DroppedDown = True
                                End If

                                If UPPERCASE = True Then
                                    .Text = UCase(FindStr)
                                Else
                                    .Text = FindStr
                                End If

                                If Asc(e.KeyChar) = 8 Then
                                    If SelStrt > 0 Then .SelectionStart = SelStrt - 1
                                Else
                                    .SelectionStart = SelStrt + 1
                                End If

                            Else

                                If vBlock_Typing_Status = True Then
                                    If Trim(FindStr) <> "" Then
                                        If UPPERCASE = True Then
                                            .Text = UCase(Microsoft.VisualBasic.Left(FindStr, Len(FindStr) - 1))
                                        Else
                                            .Text = Microsoft.VisualBasic.Left(FindStr, Len(FindStr) - 1)
                                        End If

                                        .SelectionStart = .Text.Length
                                    End If
                                Else
                                    .DataSource = Nothing
                                    .DisplayMember = ""

                                    If UPPERCASE = True Then
                                        .Text = UCase(FindStr)
                                    Else
                                        .Text = FindStr
                                    End If
                                    If Asc(e.KeyChar) = 8 Then
                                        If SelStrt > 0 Then .SelectionStart = SelStrt - 1
                                    Else
                                        .SelectionStart = SelStrt + 1
                                    End If
                                End If

                            End If

                            e.Handled = True

                            If Mtch_STS = False And vBlock_Typing_Status = False Then
                                If .DroppedDown = True Then

                                    Cmd.Connection = Cn1

                                    Cmd.CommandText = "truncate table Combo_Temp"
                                    Cmd.ExecuteNonQuery()

                                    Cmd.CommandText = "insert into Combo_Temp(name1) values ('" & Trim(UCase(FindStr)) & "')"
                                    Cmd.ExecuteNonQuery()

                                    da = New SqlClient.SqlDataAdapter("Select distinct(Name1) from Combo_Temp", Cn1)
                                    dt = New DataTable
                                    da.Fill(dt)
                                    .DataSource = dt
                                    .DisplayMember = "Name1"

                                    indx = .FindString(FindStr)

                                    If indx <> -1 Then
                                        If .SelectedIndex >= 0 Then
                                            .SelectedIndex = 0
                                            .SelectedItem = .Items(.SelectedIndex)
                                            If UPPERCASE = True Then
                                                .Text = UCase(.GetItemText(.SelectedItem))
                                            Else
                                                .Text = .GetItemText(.SelectedItem)
                                            End If
                                        End If
                                    End If


                                    Try
                                        .DroppedDown = False
                                    Catch ex As Exception
                                        '----
                                    End Try

                                    '.DataSource = Nothing
                                    '.DisplayMember = ""

                                    Try
                                        If UPPERCASE = True Then
                                            .Text = UCase(FindStr)
                                        Else
                                            .Text = FindStr
                                        End If

                                    Catch ex As Exception
                                        ''---
                                        ''Try
                                        ''    .Text = FindStr
                                        ''Catch ex1 As Exception
                                        ''    ----
                                        ''End Try

                                    End Try

                                    If Asc(e.KeyChar) = 8 Then
                                        If SelStrt > 0 Then .SelectionStart = SelStrt - 1
                                    Else
                                        .SelectionStart = SelStrt + 1 'FindStr.Length
                                    End If

                                    .SelectionLength = .Text.Length

                                    '.SelectedIndex = -1

                                End If
                            End If

                        Else

                            indx = .FindString(FindStr)
                            If indx <> -1 Then

                                If .Items.Count > 0 Then
                                    If Asc(e.KeyChar) = 32 And Len(FindStr) = 0 Then .DroppedDown = False
                                    .DroppedDown = True
                                End If

                                '.SelectedText = ""
                                .SelectedIndex = indx
                                .SelectedItem = .Items(.SelectedIndex)
                                'If UPPERCASE = True Then
                                '    .Text = UCase(.GetItemText(.SelectedItem))
                                'Else
                                .Text = .GetItemText(.SelectedItem)
                                'End If

                                If Asc(e.KeyChar) = 8 Then
                                    If SelStrt > 0 Then .SelectionStart = SelStrt - 1
                                Else
                                    .SelectionStart = SelStrt + 1 'FindStr.Length
                                End If

                                .SelectionLength = .Text.Length
                                e.Handled = True

                            Else

                                If vBlock_Typing_Status = True Then
                                    If Trim(FindStr) <> "" Then
                                        'If UPPERCASE = True Then
                                        '    .Text = UCase(Microsoft.VisualBasic.Left(FindStr, Len(FindStr) - 1))
                                        'Else
                                        .Text = Microsoft.VisualBasic.Left(FindStr, Len(FindStr) - 1)
                                        'End If
                                        .SelectionStart = .Text.Length
                                    End If

                                Else
                                    .DataSource = Nothing
                                    .DisplayMember = ""
                                    .SelectedText = ""
                                    .SelectedIndex = -1

                                    'If UPPERCASE = True Then
                                    '    .Text = UCase(FindStr)
                                    'Else
                                    .Text = FindStr
                                    'End If

                                    If Asc(e.KeyChar) = 8 Then
                                        If SelStrt > 0 Then .SelectionStart = SelStrt - 1
                                    Else
                                        .SelectionStart = SelStrt + 1 'FindStr.Length
                                    End If
                                End If

                                e.Handled = True
                            End If

                        End If

                    End If

                End If

            End With


        Catch ex As NullReferenceException
            'MessageBox.Show(ex.Message, "ERROR WHILE KEYPRESS IN COMBOBOX " & sender.ToString & "....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            'MessageBox.Show(ex.Message, "ERROR WHILE KEYPRESS IN COMBOBOX " & sender.ToString & "....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ArgumentException
            'MessageBox.Show(ex.Message, "ERROR WHILE KEYPRESS IN COMBOBOX " & sender.ToString & "....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "ERROR IN WHILE KEYPRESS IN COMBOBOX " & sender.ToString & "....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            '---

        End Try

    End Sub



    Public Shared Sub ComboBox_ItemSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs, ByVal Cn1 As SqlClient.SqlConnection, ByVal CboName As ComboBox, ByVal PreviousCtrlName As Object, ByVal NextCtrlName As Object, ByVal vTableName As String, ByVal vSelectionFieldName As String, ByVal vSqlCondition As String, ByVal vBlankFieldCondition As String)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim SqlCondt As String, Condt2 As String
        Dim FindStr As String
        Dim indx As Integer
        Dim SelStrt As Integer

        Try

            With CboName

                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True
                    If IsNothing(PreviousCtrlName) = False Then
                        PreviousCtrlName.Focus()
                    End If

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    If IsNothing(NextCtrlName) = False Then
                        NextCtrlName.Focus()
                    End If

                ElseIf e.KeyValue = 46 Then

                    SqlCondt = ""
                    Condt2 = ""
                    FindStr = ""
                    indx = -1

                    SelStrt = .SelectionStart

                    If .SelectionStart <= 1 And .SelectionLength > 0 Then
                        .Text = ""
                    End If

                    If Trim(.Text) <> "" Then

                        If .SelectionLength = 0 Then

                            If .SelectionStart > 0 Then
                                FindStr = .Text.Substring(0, .SelectionStart)
                            End If
                            FindStr = FindStr & Mid(CboName.Text, CboName.SelectionStart + 2, Len(CboName.Text))

                        Else

                            FindStr = .Text.Substring(0, .SelectionStart - 1)

                        End If

                        'If .SelectionLength = 0 Then
                        '    FindStr = .Text.Substring(0, .Text.Length - 1)
                        'Else
                        '    FindStr = .Text.Substring(0, .SelectionStart - 1)
                        'End If
                    End If

                    FindStr = LTrim(FindStr)


                    If Trim(vTableName) <> "" Then

                        SqlCondt = ""

                        If Trim(FindStr) <> "" Then
                            SqlCondt = " Where " & vSqlCondition & IIf(Trim(vSqlCondition) <> "", " and ", "") & " (" & vSelectionFieldName & " like '" & FindStr & "%' or " & vSelectionFieldName & " like '% " & FindStr & "%') "

                        Else

                            Condt2 = ""
                            If Trim(vSqlCondition) <> "" Then
                                Condt2 = Trim(vSqlCondition)
                                If Trim(vBlankFieldCondition) <> "" Then Condt2 = Condt2 & IIf(Trim(Condt2) <> "", " or ", "") & vBlankFieldCondition
                            End If

                            If Trim(Condt2) <> "" Then
                                SqlCondt = " Where " & Trim(Condt2)
                            End If

                        End If

                        da = New SqlClient.SqlDataAdapter("select " & vSelectionFieldName & " from " & vTableName & " " & SqlCondt & " order by " & vSelectionFieldName, Cn1)
                        da.Fill(dt)
                        .DataSource = dt
                        .DisplayMember = Trim(vSelectionFieldName)

                        .Text = FindStr

                        .SelectionStart = SelStrt  ' FindStr.Length

                        e.Handled = True

                    Else

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx

                            .SelectionStart = SelStrt  ' FindStr.Length
                            '.SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            .Text = FindStr

                            .SelectionStart = SelStrt  ' FindStr.Length

                            e.Handled = True

                        End If

                    End If

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then
                    If .DroppedDown = False Then
                        .DroppedDown = True
                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Public Shared Sub ComboBox_ItemSelection_SetDataSource(ByVal sender As Object, ByVal Cn1 As SqlClient.SqlConnection, ByVal vTableName As String, ByVal vSelectionFieldName As String, ByVal vSqlCondition As String, ByVal vBlankFieldCondition As String)

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCboTxt As String = ""
        Dim SqlCondt As String = ""

        ' Try

        With sender

            If Trim(vTableName) <> "" And Trim(vSelectionFieldName) <> "" Then

                vCboTxt = .Text

                SqlCondt = ""

                If Trim(vSqlCondition) <> "" Then
                    SqlCondt = " Where " & Trim(vBlankFieldCondition) & IIf(Trim(vBlankFieldCondition) <> "", " or ", "") & Trim(vSqlCondition)
                End If

                'Da = New SqlClient.SqlDataAdapter("select distinct(" & Trim(vSelectionFieldName) & ") from " & vTableName & " " & SqlCondt & " order by " & vSelectionFieldName, Cn1)
                Da = New SqlClient.SqlDataAdapter("select distinct(" & Trim(vSelectionFieldName) & ") from " & vTableName & " " & SqlCondt & " union select '' from " & vTableName & " order by 1 ", Cn1)
                'MsgBox("select distinct(" & Trim(vSelectionFieldName) & ") from " & vTableName & " " & SqlCondt & " order by " & vSelectionFieldName)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                .DataSource = Dt1
                .DisplayMember = Trim(vSelectionFieldName)

                .Text = Trim(vCboTxt)

                .BackColor = Color.Lime
                .ForeColor = Color.Blue
                .SelectAll()

            End If

            '.Items.Add("")

        End With

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "ERROR IN SETTING DATASOURCE " & sender.ToString & "....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally
        'Da.Dispose()

        ' End Try

    End Sub

    Public Shared Sub ComboBox_ItemSelection_DirectValues(ByVal sender As ComboBox, ByRef ListItems() As String, Optional ByRef ClearExisitngValues As Boolean = False, Optional ByRef AddBlankValue As Boolean = True)

        Try

            If ClearExisitngValues Then
                sender.Items.Clear()
            End If

            If AddBlankValue Then
                sender.Items.Add("")
            End If

            If ListItems.GetUpperBound(0) >= 0 Then
                For I As Integer = 0 To ListItems.GetUpperBound(0)
                    sender.Items.Add(ListItems(I))
                Next
            End If


        Catch ex As Exception

        End Try

    End Sub

    Public Shared Function Check_Negative_Stock_Status(ByVal Cn1 As SqlClient.SqlConnection, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Boolean
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Led_Idno As Integer = 0
        Dim I As Integer
        Dim Stk As Single = 0
        Dim ForStk_Weight As String = ""
        Dim Descp As String = ""
        Dim CurStk As Decimal = 0

        Check_Negative_Stock_Status = False

        Da1 = New SqlClient.SqlDataAdapter("Select Company_Idno, Item_IdNo, sum(Quantity) from TempTable_For_NegativeStock group by Company_Idno, Item_IdNo Order by Company_Idno, Item_IdNo", Cn1)
        If IsNothing(sqltr) = False Then
            Da1.SelectCommand.Transaction = sqltr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Descp = "NEGATIVE STOCK : " & Chr(13)

                If Val(Dt1.Rows(I).Item("Item_IdNo").ToString) <> 0 Then

                    CurStk = get_Item_CurrentStock(Cn1, Val(Dt1.Rows(I).Item("Company_Idno").ToString), Val(Dt1.Rows(I).Item("Item_IdNo").ToString), sqltr)

                    If CurStk < 0 Then

                        Descp = Descp & "Item : " & Common_Procedures.Item_IdNoToName(Cn1, Val(Dt1.Rows(I).Item("Item_IdNo").ToString), sqltr)
                        Descp = Descp & Chr(13) & " Stock : " & Val(CurStk)

                        Check_Negative_Stock_Status = True
                        Throw New ApplicationException(Descp)
                        Exit Function

                    End If

                End If

            Next I

        End If

        Dt1.Clear()

        Dt1.Dispose()
        Da1.Dispose()

    End Function

    Public Shared Function VoucherBill_Deletion(ByVal Cn1 As SqlClient.SqlConnection, ByVal ent_idn As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Boolean
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim vou_bil_code As String = ""
        Dim Amt As Double = 0

        vou_bil_code = get_FieldValue(Cn1, "Voucher_Bill_Head", "VoUcher_Bill_Code", "(Entry_Identification = '" & Trim(ent_idn) & "')", , sqltr)

        Amt = 0
        Da1 = New SqlClient.SqlDataAdapter("Select sum(amount) from voucher_bill_details where voucher_bill_code = '" & Trim(vou_bil_code) & "' and entry_identification <> '" & Trim(ent_idn) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da1.SelectCommand.Transaction = sqltr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then Amt = Val(Dt1.Rows(0)(0).ToString)
        End If
        Dt1.Clear()

        If Val(Amt) <> 0 Then
            VoucherBill_Deletion = False
            Throw New ApplicationException("Already Received/Paid Amount is  Rs." & Trim(Format(Amt, "#########0.00")))
            'Err.Description = "Already Received/Paid Amount is  Rs." & Trim(Format(Amt, "#########0.00"))

        Else

            Cmd.Connection = Cn1

            If IsNothing(sqltr) = False Then
                Cmd.Transaction = sqltr
            End If

            Cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.entry_identification = '" & Trim(ent_idn) & "' and b.crdr_type = 'CR' and a.voucher_bill_code = b.voucher_bill_code"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "update voucher_bill_head set debit_amount = a.debit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.entry_identification = '" & Trim(ent_idn) & "' and b.crdr_type = 'DR' and a.voucher_bill_code = b.voucher_bill_code"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "delete from voucher_bill_details where entry_identification = '" & Trim(ent_idn) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "delete from voucher_bill_head where entry_identification = '" & Trim(ent_idn) & "'"
            Cmd.ExecuteNonQuery()

            VoucherBill_Deletion = True

        End If

        Cmd.Dispose()
        Da1.Dispose()
        Dt1.Dispose()

    End Function

    Public Shared Function Voucher_Deletion(ByVal Cn1 As SqlClient.SqlConnection, ByVal Comp_IdNo As Integer, ByVal Ent_IdnCode As String, Optional ByVal SqlTr As SqlClient.SqlTransaction = Nothing) As Boolean
        Dim cmd As New SqlClient.SqlCommand

        Voucher_Deletion = False

        cmd.Connection = Cn1
        If IsNothing(SqlTr) = False Then
            cmd.Transaction = SqlTr
        End If

        cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(Comp_IdNo)) & " and Voucher_Code = '" & Trim(Ent_IdnCode) & "' and Entry_Identification = '" & Trim(Ent_IdnCode) & "'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(Comp_IdNo)) & " and Voucher_Code = '" & Trim(Ent_IdnCode) & "' and Entry_Identification = '" & Trim(Ent_IdnCode) & "'"
        cmd.ExecuteNonQuery()

        Voucher_Deletion = True

    End Function


    Public Shared Function AccountsGroup_CodeToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vAccountsGroup_CD As String) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vAccountsGroup_Nm As String

        Da = New SqlClient.SqlDataAdapter("Select AccountsGroup_Name from AccountsGroup_Head where Parent_Idno = '" & Trim(vAccountsGroup_CD) & "'", Cn1)
        Dt = New DataTable
        Da.Fill(Dt)

        vAccountsGroup_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vAccountsGroup_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        AccountsGroup_CodeToName = Trim(vAccountsGroup_Nm)

    End Function
    Public Shared Function Show_CompanyCondition_for_Report(ByVal Cn1 As SqlClient.SqlConnection) As Boolean
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Show_CompanyCondition_for_Report = False

        Try

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
                CompCondt = "(Company_Type <> 'UNACCOUNT')"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", Cn1)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Show_CompanyCondition_for_Report = True

            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da.Dispose()

        End Try

    End Function

    Public Shared Function Voucher_Updation(ByVal Cn1 As SqlClient.SqlConnection, ByVal Vou_Type As String, ByVal Comp_IdNo As Integer, ByVal Ent_IdnCode As String, ByVal Ref_No As String, ByVal Vou_Date As Date, ByVal Par_BilNo As String, ByVal Led_IDNos As String, ByVal Vou_Amts As String, ByRef ErrMsg As String, Optional ByVal SqlTr As SqlClient.SqlTransaction = Nothing) As Boolean

        Dim cmd As New SqlClient.SqlCommand
        Dim vforOrdBy As Double = 0
        Dim LedAr() As String, AmtAr() As String
        Dim db_idno As Integer = 0
        Dim cr_idno As Integer = 0
        Dim vTotCrAmt As Double = 0
        Dim vTotDrAmt As Double = 0
        Dim Mx_DrAmt As Double = 0
        Dim Mx_CrAmt As Double = 0
        Dim i As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim Nr As Integer = 0

        Voucher_Updation = False
        ErrMsg = ""

        vforOrdBy = Val(Common_Procedures.OrderBy_CodeToValue(Ref_No))

        LedAr = Split(Led_IDNos, "|")
        AmtAr = Split(Vou_Amts, "|")

        If UBound(LedAr) <> UBound(AmtAr) Then
            ErrMsg = "Invalid Voucher Posting, mismatch of ledger and Amount details"
            Exit Function
        End If

        db_idno = 0 : cr_idno = 0
        Mx_DrAmt = 0 : Mx_CrAmt = 0
        vTotDrAmt = 0 : vTotCrAmt = 0

        For i = 0 To UBound(LedAr)

            If Val(LedAr(i)) <> 0 And Val(AmtAr(i)) <> 0 Then

                If Val(AmtAr(i)) < 0 Then
                    If (db_idno = 0 Or Math.Abs(Val(AmtAr(i))) > Mx_DrAmt) Then
                        db_idno = Val(LedAr(i))
                        Mx_DrAmt = Math.Abs(Val(AmtAr(i)))
                    End If
                    vTotDrAmt = vTotDrAmt + Format(Val(AmtAr(i)), "###########0.00")
                End If

                If Val(AmtAr(i)) > 0 Then
                    If (cr_idno = 0 Or Math.Abs(Val(AmtAr(i))) > Mx_CrAmt) Then
                        cr_idno = Val(LedAr(i))
                        Mx_CrAmt = Math.Abs(Val(AmtAr(i)))
                    End If
                    vTotCrAmt = vTotCrAmt + Format(Val(AmtAr(i)), "###########0.00")
                End If

            End If

        Next

        vTotDrAmt = Format(Val(vTotDrAmt), "#########0.00")
        vTotCrAmt = Format(Val(vTotCrAmt), "#########0.00")

        If Math.Abs(vTotDrAmt) <> Math.Abs(vTotCrAmt) Then
            ErrMsg = "Invalid Voucher Amount - Debit and Credit amount not equal"
            Exit Function
        End If

        EntID = Left(Trim(Ent_IdnCode), 6) & Trim(Ref_No)

        cmd.Connection = Cn1
        If IsNothing(SqlTr) = False Then
            cmd.Transaction = SqlTr
        End If

        cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(Comp_IdNo)) & " and Voucher_Code = '" & Trim(Ent_IdnCode) & "' and Entry_Identification = '" & Trim(Ent_IdnCode) & "'"
        Nr = cmd.ExecuteNonQuery()
        cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(Comp_IdNo)) & " and Voucher_Code = '" & Trim(Ent_IdnCode) & "' and Entry_Identification = '" & Trim(Ent_IdnCode) & "'"
        cmd.ExecuteNonQuery()

        If Val(vTotDrAmt) <> 0 And Val(vTotCrAmt) <> 0 Then

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@VouchDate", Vou_Date.Date)

            cmd.CommandText = "Insert into Voucher_Head ( Voucher_Code         ,                 For_OrderByCode                       ,         Company_IdNo       ,          Voucher_No   ,                   For_OrderBy                         ,         Voucher_Type    , Voucher_Date,          Creditor_Idno   ,          Debtor_Idno     ,              Total_VoucherAmount                       ,          Narration       , Indicate,             Year_For_Report                               ,       Entry_Identification ,           Entry_ID   , Voucher_Receipt_Code ) " & _
                                        "   Values ('" & Trim(Ent_IdnCode) & "', " & Str(Format(Val(vforOrdBy), "###########0.00")) & ", " & Str(Val(Comp_IdNo)) & ", '" & Trim(Ref_No) & "', " & Str(Format(Val(vforOrdBy), "###########0.00")) & ", '" & Trim(Vou_Type) & "',   @VouchDate, " & Str(Val(cr_idno)) & ", " & Str(Val(db_idno)) & ", " & Str(Format(Val(vTotDrAmt), "###########0.00")) & " , '" & Trim(Par_BilNo) & "',     1   , " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Ent_IdnCode) & "', '" & Trim(EntID) & "',             ''       ) "
            cmd.ExecuteNonQuery()

            Sno = 0

            For i = 0 To UBound(LedAr)

                If Val(LedAr(i)) <> 0 And Val(AmtAr(i)) <> 0 Then

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Voucher_Details (         Voucher_Code      ,                 For_OrderByCode                       ,          Company_IdNo      ,        Voucher_No     ,                For_OrderBy                            ,       Voucher_Type      , Voucher_Date,           SL_No      ,          Ledger_IdNo      ,              Voucher_Amount                         ,         Narration        ,             Year_For_Report                               ,   Entry_Identification     ,           Entry_ID   ) " & _
                                      "            Values          ('" & Trim(Ent_IdnCode) & "', " & Str(Format(Val(vforOrdBy), "###########0.00")) & ", " & Str(Val(Comp_IdNo)) & ", '" & Trim(Ref_No) & "', " & Str(Format(Val(vforOrdBy), "###########0.00")) & ", '" & Trim(Vou_Type) & "',  @VouchDate , " & Str(Val(Sno)) & ", " & Str(Val(LedAr(i))) & ", " & Str(Format(Val(AmtAr(i)), "##########0.00")) & ", '" & Trim(Par_BilNo) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Ent_IdnCode) & "', '" & Trim(EntID) & "')"
                    cmd.ExecuteNonQuery()

                End If

            Next i

        End If

        cmd.Dispose()

        Voucher_Updation = True

    End Function

    Public Shared Function VoucherBill_Posting(ByVal Cn1 As SqlClient.SqlConnection, ByVal Comp_IdNo As Integer, ByVal Vou_Bil_Date As Date, ByVal Led_IdNo As Integer, ByVal Par_Bil_No As String, ByVal Agt_Idno As Integer, ByVal Bil_Amt As Double, ByVal CrDr_Type As String, ByVal Ent_Idn As String, Optional ByVal SqlTr As SqlClient.SqlTransaction = Nothing) As String
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim Posting_Column As String = ""
        Dim Adjust_Column As String = ""
        Dim Nr As Long = 0
        Dim adj_amt As Double = 0
        Dim amt As Double = 0
        Dim RcptAmt As Double = 0
        Dim Tot_AdvBil_Amt As Single = 0
        Dim vou_amt As Double = 0
        Dim bill_main_sts As Boolean = False
        Dim vou_bil_no As String = ""
        Dim vou_bil_code As String = ""
        Dim NewEntry As Boolean = False
        Dim Show_AdvBillAdj_sts As Boolean = False

        Err.Clear()
        Err.Description = ""

        If Led_IdNo = 1 Then

            Cmd.Connection = Cn1
            Cmd.Transaction = SqlTr
            Cmd.CommandText = "delete from voucher_bill_head where entry_identification = '" & Trim(Ent_Idn) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "delete from voucher_bill_details where entry_identification = '" & Trim(UCase(Ent_Idn)) & "' "
            Cmd.ExecuteNonQuery()

            Exit Function

        End If


        vou_bil_code = get_FieldValue(Cn1, "Voucher_Bill_Head", "VoUcher_Bill_Code", "(Entry_Identification = '" & Trim(Ent_Idn) & "')", , SqlTr)
        vou_bil_no = get_FieldValue(Cn1, "Voucher_Bill_Head", "VoUcher_Bill_No", "(Entry_Identification = '" & Trim(Ent_Idn) & "')", , SqlTr)

       
        If Trim(UCase(get_FieldValue(Cn1, "Ledger_head", "Bill_Type", "(Ledger_idno = " & Str(Val(Led_IdNo)) & ")", , SqlTr))) = "BILL TO BILL" Then bill_main_sts = True Else bill_main_sts = False

        Cmd.Connection = Cn1
        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@VouchDate", Vou_Bil_Date)

        If IsNothing(SqlTr) = False Then
            Cmd.Transaction = SqlTr
        End If

        If bill_main_sts = False Then

            If Trim(vou_bil_code) = "" Then
                VoucherBill_Posting = ""
                Exit Function

            Else
                If VoucherBill_Checking(Cn1, vou_bil_code, Ent_Idn, CrDr_Type, Led_IdNo, 0, SqlTr) = False Then

                    If Led_IdNo <> 1 Then
                        VoucherBill_Posting = "Error"
                        Exit Function
                    Else
                        Cmd.CommandText = "delete from voucher_bill_head where entry_identification = '" & Trim(Ent_Idn) & "'"
                        Cmd.ExecuteNonQuery()

                        VoucherBill_Posting = ""
                        Exit Function
                    End If
                Else
                        Cmd.CommandText = "delete from voucher_bill_head where entry_identification = '" & Trim(Ent_Idn) & "'"
                    Cmd.ExecuteNonQuery()

                    VoucherBill_Posting = ""
                    Exit Function

                End If

            End If

        End If

        Posting_Column = IIf(Trim(UCase(CrDr_Type)) = "CR", "Credit", "Debit")
        Adjust_Column = IIf(Trim(UCase(CrDr_Type)) = "CR", "Debit", "Credit")

        Nr = 0

        If Trim(vou_bil_code) = "" Then

            vou_bil_no = get_MaxCode(Cn1, "Voucher_Bill_Head", "Voucher_Bill_Code", "For_OrderBy", "", Val(Comp_IdNo), Common_Procedures.FnYearCode, SqlTr)
            vou_bil_code = Trim(Val(Comp_IdNo)) & "-" & Trim(vou_bil_no) & "/" & Trim(Common_Procedures.FnYearCode)

            Err.Description = ""
            NewEntry = True
            GoTo AdvanceBills_Display
LOOP100:
            If Trim(UCase(Err.Description)) = "ERROR" Then
                VoucherBill_Posting = "Error"
                Exit Function
            End If


            Nr = 0
            Cmd.CommandText = "Insert into Voucher_bill_head ( voucher_bill_code,          company_idno      ,          voucher_bill_no  ,             for_orderby     , voucher_bill_date,            ledger_idno    ,         party_bill_no     ,          agent_idno       ,         bill_amount      , " & Trim(Posting_Column) & "_amount, " & Trim(Adjust_Column) & "_amount,              crdr_type          ,       entry_identification     ) " _
                                    & " Values (    '" & Trim(vou_bil_code) & "', " & Str(Val(Comp_IdNo)) & ", '" & Trim(vou_bil_no) & "', " & Str(Val(vou_bil_no)) & ",     @VouchDate   , " & Str(Val(Led_IdNo)) & ", '" & Trim(Par_Bil_No) & "', " & Str(Val(Agt_Idno)) & ", " & Str(Val(Bil_Amt)) & ", " & Str(Val(Bil_Amt)) & "          , " & Str(Val(adj_amt)) & "         , '" & Trim(UCase(CrDr_Type)) & "', '" & Trim(UCase(Ent_Idn)) & "' )"
            Nr = Cmd.ExecuteNonQuery

        Else

            If VoucherBill_Checking(Cn1, vou_bil_code, Ent_Idn, CrDr_Type, Led_IdNo, Bil_Amt, SqlTr) = False Then
                VoucherBill_Posting = "Error"
                Exit Function
            End If

            Err.Description = ""
            NewEntry = False
            GoTo AdvanceBills_Display

LOOP200:
            If Trim(UCase(Err.Description)) = "ERROR" Then
                VoucherBill_Posting = "Error"
                Exit Function
            End If

            Nr = 0
            Cmd.CommandText = "update voucher_bill_head set Voucher_bill_date = @VouchDate, party_bill_no = '" & Trim(Par_Bil_No) & "', agent_idno = " & Str(Val(Agt_Idno)) & ", bill_amount = " & Str(Val(Bil_Amt)) & ", " & Trim(Posting_Column) & "_amount = " & Str(Val(Bil_Amt)) & ", " & Trim(Adjust_Column) & "_amount = " & Str(Val(adj_amt + vou_amt)) & " " _
                                & "  Where voucher_bill_code = '" & Trim(vou_bil_code) & "' and Ledger_Idno = " & Str(Led_IdNo)
            Nr = Cmd.ExecuteNonQuery

            If Nr = 0 Then

                Cmd.CommandText = "update voucher_bill_head set credit_amount = a.credit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.entry_identification = '" & Trim(UCase(Ent_Idn)) & "' and b.Ledger_Idno <> " & Str(Led_IdNo) & " and b.crdr_type = 'CR' and a.voucher_bill_code = b.voucher_bill_code"
                Cmd.ExecuteNonQuery()

                Cmd.CommandText = "update voucher_bill_head set debit_amount = a.debit_amount - b.amount from voucher_bill_head a, voucher_bill_details b where b.entry_identification = '" & Trim(UCase(Ent_Idn)) & "' and b.Ledger_Idno <> " & Str(Led_IdNo) & " and b.crdr_type = 'DR' and a.voucher_bill_code = b.voucher_bill_code"
                Cmd.ExecuteNonQuery()

                Cmd.CommandText = "delete from voucher_bill_details where entry_identification = '" & Trim(UCase(Ent_Idn)) & "' and Ledger_Idno <> " & Str(Led_IdNo)
                Cmd.ExecuteNonQuery()

                Cmd.CommandText = "delete from voucher_bill_head where entry_identification = '" & Trim(UCase(Ent_Idn)) & "' and Ledger_Idno <> " & Str(Led_IdNo)
                Cmd.ExecuteNonQuery()

                Cmd.CommandText = "Insert into voucher_bill_head ( voucher_bill_code,           company_idno     ,        voucher_bill_no    ,            for_orderby      , voucher_bill_date,        ledger_idno   ,        party_bill_no      ,        agent_idno    ,      bill_amount    , " & Trim(Posting_Column) & "_amount, " & Trim(Adjust_Column) & "_amount,         crdr_type        ,        entry_identification    ) " _
                                        & "  Values ( '" & Trim(vou_bil_code) & "'  , " & Str(Val(Comp_IdNo)) & ", '" & Trim(vou_bil_no) & "', " & Str(Val(vou_bil_no)) & ",     @VouchDate   , " & Str(Led_IdNo) & ", '" & Trim(Par_Bil_No) & "', " & Str(Agt_Idno) & ", " & Str(Bil_Amt) & ", " & Str(Bil_Amt) & "               , " & Str(adj_amt) & "              , '" & Trim(CrDr_Type) & "', '" & Trim(UCase(Ent_Idn)) & "' )"
                Nr = Cmd.ExecuteNonQuery

            End If

        End If

        If Val(adj_amt) > 0 Then
            Cmd.CommandText = "Insert into voucher_bill_details ( Voucher_Bill_Code,           Company_Idno     , Voucher_Bill_Date,        Ledger_Idno        ,   entry_identification ,            Amount        ,              CrDr_Type                   ) " & _
                                            " Values ( '" & Trim(vou_bil_code) & "', " & Str(Val(Comp_IdNo)) & ",      @VouchDate  , " & Str(Val(Led_IdNo)) & ", '" & Trim(Ent_Idn) & "', " & Str(Val(adj_amt)) & ", '" & Trim(Left$(Adjust_Column, 1)) & "R' )"
            Cmd.ExecuteNonQuery()
        End If

        If Nr = 0 Then VoucherBill_Posting = "Error" Else VoucherBill_Posting = vou_bil_code

        Cmd.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

        Exit Function


AdvanceBills_Display:

        RcptAmt = 0
        Da1 = New SqlClient.SqlDataAdapter("Select sum(amount) from voucher_bill_details where ledger_idno = " & Str(Val(Led_IdNo)) & " and Voucher_Bill_Code = '" & Trim(vou_bil_code) & "' and Entry_Identification <> '" & Trim(Ent_Idn) & "'", Cn1)
        If IsNothing(SqlTr) = False Then
            Da1.SelectCommand.Transaction = SqlTr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then RcptAmt = Val(Dt1.Rows(0)(0).ToString)
        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da1.Dispose()

        Show_AdvBillAdj_sts = True
        If RcptAmt > 0 Then
            If RcptAmt >= Bil_Amt Then
                Show_AdvBillAdj_sts = False
            End If
        End If

        amt = 0
        adj_amt = 0 : vou_amt = 0
        Tot_AdvBil_Amt = 0

        If Show_AdvBillAdj_sts = True Then

            Da1 = New SqlClient.SqlDataAdapter("Select sum(" & Trim(Adjust_Column) & "_amount - " & Trim(Posting_Column) & "_amount) from voucher_bill_head where company_idno = " & Str(CompIdNo) & " and ledger_idno = " & Str(Val(Led_IdNo)) & " and " & Trim(Adjust_Column) & "_amount > " & Trim(Posting_Column) & "_amount", Cn1)
            If IsNothing(SqlTr) = False Then
                Da1.SelectCommand.Transaction = SqlTr
            End If
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then amt = Val(Dt1.Rows(0)(0).ToString)
            End If
            Dt1.Clear()

            Da1 = New SqlClient.SqlDataAdapter("Select sum(amount) from voucher_bill_details where company_idno = " & Str(CompIdNo) & " and ledger_idno = " & Str(Val(Led_IdNo)) & " and entry_identification = '" & Trim(Ent_Idn) & "'", Cn1)
            If IsNothing(SqlTr) = False Then
                Da1.SelectCommand.Transaction = SqlTr
            End If
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then amt = amt + Val(Dt1.Rows(0)(0).ToString)
            End If
            Dt1.Clear()

            adj_amt = 0 : vou_amt = 0
            Tot_AdvBil_Amt = 0
            If amt > 0 Then
                Dim f1 As New Advance_Bill_Adjustment

                f1.Bills_Display(Cn1, Comp_IdNo, vou_bil_code, Vou_Bil_Date, Led_IdNo, Par_Bil_No, Agt_Idno, CrDr_Type, Bil_Amt, Ent_Idn, Tot_AdvBil_Amt, SqlTr)

                If Tot_AdvBil_Amt > 0 Then
                    f1.ShowDialog()


                Else
                    f1.Close()
                    f1.Dispose()

                End If

            End If

        End If

        Da1 = New SqlClient.SqlDataAdapter("Select sum(amount) from voucher_bill_details where voucher_bill_code = '" & Trim(vou_bil_code) & "' and entry_identification <> '" & Trim(Ent_Idn) & "'", Cn1)
        If IsNothing(SqlTr) = False Then
            Da1.SelectCommand.Transaction = SqlTr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then vou_amt = Val(Dt1.Rows(0)(0).ToString)
        End If
        Dt1.Clear()

        Da1 = New SqlClient.SqlDataAdapter("Select sum(amount) from voucher_bill_details where voucher_bill_code <> '" & Trim(vou_bil_code) & "' and entry_identification = '" & Trim(Ent_Idn) & "' and ledger_idno = " & Str(Val(Led_IdNo)), Cn1)
        If IsNothing(SqlTr) = False Then
            Da1.SelectCommand.Transaction = SqlTr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then adj_amt = Val(Dt1.Rows(0)(0).ToString)
        End If
        Dt1.Clear()



        If NewEntry = True Then
            GoTo LOOP100
        Else
            GoTo LOOP200
        End If

    End Function

    Public Shared Function VoucherBill_Checking(ByVal Cn1 As SqlClient.SqlConnection, ByVal vou_bil_cd As String, ByVal ent_idn As String, ByVal crdr_type As String, ByVal c_ledidno As Integer, ByVal c_amt As Double, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Boolean
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim amt As Double = 0
        Dim Led_Id As Integer = 0

        VoucherBill_Checking = True

        Err.Clear()

        Da1 = New SqlClient.SqlDataAdapter("Select ledger_idno, sum(amount) from voucher_bill_details where Voucher_Bill_Code = '" & Trim(vou_bil_cd) & "' and Entry_Identification <> '" & Trim(ent_idn) & "' group by ledger_idno", Cn1)
        If IsNothing(sqltr) = False Then
            Da1.SelectCommand.Transaction = sqltr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then Led_Id = Val(Dt1.Rows(0)(0).ToString)
            If IsDBNull(Dt1.Rows(0)(1).ToString) = False Then amt = Val(Dt1.Rows(0)(1).ToString)
        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da1.Dispose()

        If amt > 0 Then
            If amt > c_amt Then
                Err.Description = "Already " & IIf(crdr_type = "Cr", "paid", "received") & " amount is Rs." & Trim(Format(amt, "#######0.00"))
                VoucherBill_Checking = False
            End If
            If c_ledidno <> Led_Id Then
                Err.Description = "Does not change the party name" & Chr(13) & "Already " & IIf(crdr_type = "Cr", "paid", "received") & " amount is Rs." & Trim(Format(amt, "#######0.00"))
                VoucherBill_Checking = False
            End If
        End If

    End Function

    Public Shared Sub maskEdit_Date_ON_DelBackSpace(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs, ByVal mskOldText As String, ByVal mskSelStrt As Integer)
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            If e.KeyCode = 46 Then
                If mskSelStrt <= 2 Then
                    vmRetTxt = "  " & Microsoft.VisualBasic.Mid(mskOldText, 3, Len(mskOldText))
                    vmRetSelStrt = 0
                ElseIf mskSelStrt >= 3 And mskSelStrt <= 5 Then
                    vmRetTxt = Microsoft.VisualBasic.Left(mskOldText, 3) & "  " & Microsoft.VisualBasic.Mid(mskOldText, 6, Len(mskOldText))
                    vmRetSelStrt = 3
                Else
                    vmRetTxt = Microsoft.VisualBasic.Left(mskOldText, 6)
                    vmRetSelStrt = 6
                End If

                sender.Text = vmRetTxt
                sender.SelectionStart = vmRetSelStrt

            ElseIf e.KeyCode = 8 Then
                If mskSelStrt > 0 Then
                    vmRetTxt = Microsoft.VisualBasic.Left(mskOldText, mskSelStrt - 1) & " " & Microsoft.VisualBasic.Mid(mskOldText, mskSelStrt + 1, Len(mskOldText))
                Else
                    vmRetTxt = mskOldText
                End If

                sender.Text = vmRetTxt

                If mskSelStrt > 0 Then
                    sender.SelectionStart = mskSelStrt - 1
                End If

            End If

        End If

    End Sub
    Public Shared Function Month_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vMnth_IdNo As Integer, Optional ByVal SqlTr As SqlClient.SqlTransaction = Nothing) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vMnth_Name As String

        Da = New SqlClient.SqlDataAdapter("select Month_Name from Month_Head where Month_IdNo = " & Str(Val(vMnth_IdNo)), Cn1)
        If IsNothing(SqlTr) = False Then
            Da.SelectCommand.Transaction = SqlTr
        End If
        Da.Fill(Dt)

        vMnth_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vMnth_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Month_IdNoToName = Trim(vMnth_Name)

    End Function

    Public Shared Function Month_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vMnth_Name As String, Optional ByVal SqlTr As SqlClient.SqlTransaction = Nothing) As Integer

        'Dim Da As New SqlClient.SqlDataAdapter
        'Dim Dt As New DataTable
        'Dim vMnth_IdNo As Integer

        'Da = New SqlClient.SqlDataAdapter("select Month_IdNo from Month_Head where Month_Name = '" & Trim(vMnth_Name) & "'", Cn1)
        'If IsNothing(SqlTr) = False Then
        '    Da.SelectCommand.Transaction = SqlTr
        'End If
        'Da.Fill(Dt)

        'vMnth_IdNo = 0
        'If Dt.Rows.Count > 0 Then
        '    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
        '        vMnth_IdNo = Val(Dt.Rows(0)(0).ToString)
        '    End If
        'End If

        'Dt.Dispose()
        'Da.Dispose()

        'Month_NameToIdNo = Val(vMnth_IdNo)

        If Len(vMnth_Name) < 3 Then
            Month_NameToIdNo = 0
        ElseIf IsDate("1-" & Left(vMnth_Name, 3) & "-2018") Then
            Month_NameToIdNo = Format(CDate("1-" & Left(vMnth_Name, 3) & "-2018"), "MM")
        Else
            Month_NameToIdNo = 0
        End If

    End Function
    Public Shared Function Month_IdNoToShortName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vMnth_IdNo As Integer, Optional ByVal SqlTr As SqlClient.SqlTransaction = Nothing) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vMnth_Name As String

        Da = New SqlClient.SqlDataAdapter("select Month_ShortName from Month_Head where Month_IdNo = " & Str(Val(vMnth_IdNo)), Cn1)
        If IsNothing(SqlTr) = False Then
            Da.SelectCommand.Transaction = SqlTr
        End If
        Da.Fill(Dt)

        vMnth_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vMnth_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Month_IdNoToShortName = Trim(vMnth_Name)

    End Function
    Public Shared Function Month_ShortNameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vMnth_Name As String, Optional ByVal SqlTr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vMnth_IdNo As Integer

        Da = New SqlClient.SqlDataAdapter("select Month_IdNo from Month_Head where Month_ShortName = '" & Trim(vMnth_Name) & "'", Cn1)
        If IsNothing(SqlTr) = False Then
            Da.SelectCommand.Transaction = SqlTr
        End If
        Da.Fill(Dt)

        vMnth_IdNo = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vMnth_IdNo = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Month_ShortNameToIdNo = Val(vMnth_IdNo)

    End Function
    Public Shared Function Accept_AlphaNumericOnlyWithSlash(ByVal KeyAscii_Value As Integer) As Integer
        Accept_AlphaNumericOnlyWithSlash = 0
        If (KeyAscii_Value = 47 Or (KeyAscii_Value >= 48 And KeyAscii_Value <= 57)) Or (KeyAscii_Value >= 65 And KeyAscii_Value <= 90) Or (KeyAscii_Value >= 97 And KeyAscii_Value <= 122) Or KeyAscii_Value = 13 Or KeyAscii_Value = 8 Or KeyAscii_Value = 9 Then
            Accept_AlphaNumericOnlyWithSlash = KeyAscii_Value
        End If
    End Function

    Public Shared Function Create_Sql_ConnectionString(ByVal DBName As String) As String
        Dim myConnection_string As String
        Dim pth As String
        Dim fs As FileStream
        Dim w As StreamWriter
        Dim sInpIP As String = ""

        If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "IP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "SIP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "DIP" Then
            If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "DIP" Then
                If Common_Procedures.First_Opened_Today = True Then
                    sInpIP = InputBox("Enter Server System IP address :", "FOR CORRECT SERVER SYSTEM IP ADDRESS..", Trim(Common_Procedures.ServerName))

                    If Trim(sInpIP) <> "" Then
                        pth = Trim(Common_Procedures.AppPath) & "\connection.ini"

                        If File.Exists(pth) = True Then
                            File.Delete(pth)
                        End If

                        Common_Procedures.ServerName = Trim(sInpIP)

                        fs = New FileStream(pth, FileMode.Create)
                        w = New StreamWriter(fs)
                        w.WriteLine(Trim(Common_Procedures.ServerName) & "," & Trim(Common_Procedures.ServerPassword) & ",DIP")
                        w.Close()
                        fs.Close()
                        w.Dispose()
                        fs.Dispose()

                        Common_Procedures.First_Opened_Today = False

                    End If
                End If
            End If

            myConnection_string = "Data Source=" & Trim(Common_Procedures.ServerName) & ",1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(DBName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False;Connect Timeout=60"

            'myConnection_string = "Data Source=" & Trim(Common_Procedures.ServerName) & ":3389;Network Library=DBMSSOCN;Initial Catalog=" & Trim(DBName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False"
            'myConnection_string = "Data Source=" & Trim(Common_Procedures.ServerName) & ",1033;Network Library=DBMSSOCN;Initial Catalog=" & Trim(DBName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False"
            'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(DBName) & ";Integrated Security=True"
            'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=169.254.147.41,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(DBName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False"
            'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(DBName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Trusted_Connection=True;MultipleActiveResultSets=true;"
            'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(DBName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";"
            'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(DBName) & ";Integrated Security=True"

        ElseIf Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "AMA" Then
            myConnection_string = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(DBName) & ";User ID=DEVA;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False;Connect Timeout=60"
        ElseIf Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "WIN" Then
            myConnection_string = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(DBName) & ";Integrated Security=True;Connect Timeout=60"
        Else
            myConnection_string = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(DBName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False;Connect Timeout=60"
        End If


        'If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "IP" Then

        '    Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";Integrated Security=True"
        '    Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False;Connect Timeout=120"

        '    'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=169.254.147.41,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False"

        '    'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Trusted_Connection=True;MultipleActiveResultSets=true;"
        '    'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";"
        '    'Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";Integrated Security=True"

        'ElseIf Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "WIN" Then
        '    Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";Integrated Security=True;Connect Timeout=120"

        'Else
        '    Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False;Connect Timeout=120"

        'End If

        ''If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "IP" Then
        ''    Common_Procedures.ConnectionString_CompanyGroupdetails = "Data Source=122.165.215.63,1433;Network Library=DBMSSOCN;Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False"
        ''ElseIf Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "WIN" Then
        ''    Common_Procedures.ConnectionString_CompanyGroupdetails = "Persist Security Info=False;Integrated Security=SSPI;database=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";server=" & Trim(Common_Procedures.ServerName) & ";Connect Timeout=120"
        ''Else
        ''    Common_Procedures.ConnectionString_CompanyGroupdetails = "Persist Security Info=False;Integrated Security=SSPI;database=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";server=" & Trim(Common_Procedures.ServerName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Connect Timeout=120"
        ''End If

        'Dim myConnection As New SqlClient.SqlConnection()
        'myConnection.ConnectionString = "Persist Security Info=False;Integrated Security=SSPI;database=northwind;server=mySQLServer;Connect Timeout=30"
        'myConnection.Open()

        Create_Sql_ConnectionString = Trim(myConnection_string)

    End Function
    Public Shared Sub Drop_Column_Default_Constraint(ByVal Cn1 As SqlClient.SqlConnection, ByVal TblName As String, ByVal FldName As String)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim DF_ConsName As String

        Try

            DF_ConsName = ""

            Da = New SqlClient.SqlDataAdapter("select d.name as Default_ConstraintName from sysobjects a inner join dbo.syscolumns c on a.id = c.id inner join dbo.sysobjects d on c.cdefault = d.id Where a.name = '" & Trim(TblName) & "' and c.name = '" & Trim(FldName) & "'", Cn1)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Default_ConstraintName").ToString) = False Then
                    If Trim(Dt1.Rows(0).Item("Default_ConstraintName").ToString) <> "" Then DF_ConsName = Dt1.Rows(0).Item("Default_ConstraintName").ToString
                End If
            End If
            Dt1.Clear()

            If Trim(DF_ConsName) <> "" Then

                Cmd.Connection = Cn1

                Cmd.CommandText = "ALTER TABLE [dbo].[" & Trim(TblName) & "] DROP CONSTRAINT " & Trim(DF_ConsName)
                Cmd.ExecuteNonQuery()

            End If

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "ERROR IN DROPPING CONSTRAINT....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Da.Dispose()
            Dt1.Dispose()
            Cmd.Dispose()

        End Try

    End Sub
    Public Shared Function Salary_PaymentType_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSaPyTy_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vSaPyTy_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Salary_Payment_Type_IdNo from PayRoll_Salary_Payment_Type_Head where Salary_Payment_Type_Name = '" & Trim(vSaPyTy_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vSaPyTy_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vSaPyTy_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Salary_PaymentType_NameToIdNo = Val(vSaPyTy_ID)

    End Function

    Public Shared Function Salary_PaymentType_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vSaPyTy_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vSaPyTy_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Salary_Payment_Type_Name from PayRoll_Salary_Payment_Type_Head where Salary_Payment_Type_IdNo = " & Str(Val(vSaPyTy_ID)), Cn1)
        Da.Fill(Dt)

        vSaPyTy_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vSaPyTy_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Salary_PaymentType_IdNoToName = Trim(vSaPyTy_Nm)

    End Function

    Public Shared Function Employee_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vEmployee_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vEmployee_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Employee_IdNo from PayRoll_Employee_Head where Employee_Name = '" & Trim(vEmployee_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vEmployee_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vEmployee_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Employee_NameToIdNo = Val(vEmployee_ID)

    End Function

    Public Shared Function Employee_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vEmployee_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vEmployee_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head where Employee_IdNo = " & Str(Val(vEmployee_ID)), Cn1)
        Da.Fill(Dt)

        vEmployee_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vEmployee_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Employee_IdNoToName = Trim(vEmployee_Nm)

    End Function
    Public Shared Function Expense_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vExpense_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vExpense_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Expense_IdNo from Expense_Head where Expense_Name = '" & Trim(vExpense_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vExpense_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vExpense_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Expense_NameToIdNo = Val(vExpense_ID)

    End Function

    Public Shared Function Expense_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vExpense_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vExpense_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Expense_Name from Expense_Head where Employee_IdNo = " & Str(Val(vExpense_ID)), Cn1)
        Da.Fill(Dt)

        vExpense_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vExpense_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Expense_IdNoToName = Trim(vExpense_Nm)

    End Function
    Public Shared Function Shift_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vShift_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vShift_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Shift_IdNo from Shift_Head where Shift_Name = '" & Trim(vShift_Name) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vShift_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vShift_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Shift_NameToIdNo = Val(vShift_ID)

    End Function

    Public Shared Function Shift_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vShift_IdNo As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vShift_Name As String = ""

        Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head where Shift_IdNo = " & Str(Val(vShift_IdNo)), Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vShift_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vShift_Name = Dt.Rows(0)(0).ToString
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Shift_IdNoToName = vShift_Name

    End Function

    Public Shared Function Category_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCategory_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCategory_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Category_IdNo from PayRoll_Category_Head where Category_Name = '" & Trim(vCategory_Name) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vCategory_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCategory_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Category_NameToIdNo = Val(vCategory_ID)

    End Function

    Public Shared Function Category_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCategory_IdNo As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCategory_Name As String = ""

        Da = New SqlClient.SqlDataAdapter("select Category_Name from PayRoll_Category_Head where Category_IdNo = " & Str(Val(vCategory_IdNo)), Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vCategory_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCategory_Name = Dt.Rows(0)(0).ToString
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Category_IdNoToName = vCategory_Name

    End Function
    Public Shared Function Department_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vDepartment_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vDepartment_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Department_IdNo from Department_Head where Department_Name = '" & Trim(vDepartment_Name) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vDepartment_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vDepartment_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Department_NameToIdNo = Val(vDepartment_ID)

    End Function

    Public Shared Function Department_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vDepartment_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vDepartment_Name As String

        Da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head where Department_IdNo = " & Str(Val(vDepartment_ID)), Cn1)
        Da.Fill(Dt)

        vDepartment_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vDepartment_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Department_IdNoToName = Trim(vDepartment_Name)

    End Function

    Public Shared Function get_Server_SystemName() As String
        Dim InstNm As String = ""
        Dim ServerNm As String = ""

        If InStr(1, Common_Procedures.ServerName, "\") > 0 Then
            InstNm = Right(Common_Procedures.ServerName, Len(Common_Procedures.ServerName) - InStr(1, Common_Procedures.ServerName, "\"))

            ServerNm = Replace(Trim(UCase(Common_Procedures.ServerName)), Trim(UCase("\" & InstNm)), "")
        Else
            ServerNm = Trim(UCase(Common_Procedures.ServerName))
        End If


        get_Server_SystemName = ServerNm

    End Function

    Public Shared Function is_ServerSystem() As Boolean

        Dim InstNm As String

        InstNm = Right(Common_Procedures.ServerName, Len(Common_Procedures.ServerName) - InStr(1, Common_Procedures.ServerName, "\"))

        is_ServerSystem = False
        If Trim(UCase(Common_Procedures.ServerName)) = Trim(UCase(Trim(SystemInformation.ComputerName))) Or Trim(UCase(Common_Procedures.ServerName)) = Trim(UCase(Trim(SystemInformation.ComputerName) & "\TSOFT")) Or Trim(UCase(Common_Procedures.ServerName)) = Trim(UCase(Trim(SystemInformation.ComputerName) & "\" & Trim(InstNm))) Then
            is_ServerSystem = True
        End If

    End Function

    Public Shared Function is_Database_File_Exists(ByVal DbName As String) As Boolean
        Dim cn1 As SqlClient.SqlConnection
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim mdf_filname As String = "", ldf_filname As String = "", FlNm As String = ""
        Dim SysNm As String

        is_Database_File_Exists = False
        Err.Description = ""

        Try

            cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
            cn1.Open()

            da1 = New SqlClient.SqlDataAdapter("Select * from sysdatabases where name = '" & Trim(DbName) & "'", cn1)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                Call get_DataBase_MdfLdf_FileNames(DbName, mdf_filname, ldf_filname)

                If Trim(mdf_filname) = "" Then
                    Err.Description = "database file does not exists"
                    Exit Function
                End If
                If Trim(ldf_filname) = "" Then
                    Err.Description = "database file does not exists"
                    Exit Function
                End If

                FlNm = Trim(mdf_filname)
                If Common_Procedures.is_ServerSystem = False Then

                    SysNm = Common_Procedures.get_Server_SystemName
                    FlNm = "\\" & Trim(SysNm) & "\" & Trim(Replace(mdf_filname, ":\", "\"))

                    'If InStr(1, "\mssql\data\") > 0 Then
                    '    FldrNm()

                    'End If

                    If File.Exists(FlNm) = False Then
                        Err.Description = "database file does not exists"
                        Exit Function
                    End If

                    'Dim sFile As New FileInfo(FlNm)

                    ''FileInfo sFile = new FileInfo(@"\\server\share\file.xml")
                    ''bool fileExist = sFile.Exists;

                    'If sFile.Exists = False Then
                    '    Err.Description = "database file does not exists"
                    '    Exit Function
                    'End If


                Else
                    If File.Exists(FlNm) = False Then
                        Err.Description = "database file does not exists"
                        Exit Function
                    End If

                End If



                FlNm = Trim(ldf_filname)
                If Common_Procedures.is_ServerSystem = False Then
                    SysNm = Common_Procedures.get_Server_SystemName

                    FlNm = "\\" & Trim(SysNm) & "\" & Trim(Replace(ldf_filname, ":\", "\"))
                End If
                If File.Exists(FlNm) = False Then
                    Err.Description = "database file does not exists"
                    Exit Function
                End If

            Else
                Err.Description = Trim(DbName) & " does not exists"
                Exit Function

            End If

            dt1.Dispose()
            da1.Dispose()

            cn1.Close()
            cn1 = Nothing

            is_Database_File_Exists = True

        Catch ex As Exception
            MessageBox.Show("Select Company Group Name", "INVALID COMPANY GROUP SELECTION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Function

    Public Shared Sub get_DataBase_MdfLdf_FileNames(ByVal DbName As String, ByRef MDF_FileName As String, ByRef LDF_FileName As String)
        Dim CnMas As SqlClient.SqlConnection
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim Da2 As SqlClient.SqlDataAdapter
        Dim Dt2 As DataTable

        Dim DefPath As String

        CnMas = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
        CnMas.Open()

        MDF_FileName = ""
        LDF_FileName = ""

        Da1 = New SqlClient.SqlDataAdapter("SELECT * FROM sysdatabases WHERE name = '" & Trim(DbName) & "'", CnMas)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            MDF_FileName = Dt1.Rows(0).Item("FileName").ToString
            If InStr(1, LCase(MDF_FileName), "_data.mdf") > 0 Then
                LDF_FileName = Replace(LCase(MDF_FileName), "_data.mdf", "_log.ldf")
            Else
                LDF_FileName = Replace(LCase(MDF_FileName), ".mdf", "_log.ldf")
            End If


            'If Common_Procedures.is_ServerSystem = True Then
            '    If File.Exists(LDF_FileName) = False Then
            '        LDF_FileName = Replace(LCase(MDF_FileName), "_data.mdf", "_log.ldf")
            '        If File.Exists(LDF_FileName) = False Then
            '            GoTo 100
            '        End If
            '    End If
            'End If


        Else

100:
            Da2 = New SqlClient.SqlDataAdapter("SELECT * FROM sysdatabases WHERE name = 'master'", CnMas)
            Dt2 = New DataTable
            Da2.Fill(Dt2)

            If Dt2.Rows.Count > 0 Then

                DefPath = Replace(LCase(Dt2.Rows(0).Item("FileName").ToString), "\master_data.mdf", "")
                DefPath = Replace(LCase(Dt2.Rows(0).Item("FileName").ToString), "\master.mdf", "")

                MDF_FileName = Trim(DefPath) & "\" & Trim(DbName) & ".mdf"
                LDF_FileName = Trim(DefPath) & "\" & Trim(DbName) & "_log.ldf"

            End If
            Dt2.Dispose()
        End If
        Dt1.Dispose()

        CnMas.Close()
        CnMas = Nothing

    End Sub

    Public Shared Function Is_InterState_Party(ByVal Cn1 As SqlClient.SqlConnection, ByVal CompIdNo As Integer, ByVal LedIdNo As Integer) As Boolean
        Dim CompStateIdNo As Integer = 0
        Dim LedStateIdNo As Integer = 0
        Dim sts As Boolean = False

        CompStateIdNo = Val(Common_Procedures.get_FieldValue(Cn1, "Company_Head", "Company_State_IdNo", "(Company_IdNo = " & Str(Val(CompIdNo)) & ")"))
        LedStateIdNo = Val(Common_Procedures.get_FieldValue(Cn1, "Ledger_Head", "State_Idno", "(Ledger_IdNo = " & Str(Val(LedIdNo)) & ")"))

        If Val(CompStateIdNo) = 0 Or Val(LedStateIdNo) = 0 Then
            sts = False
        ElseIf Val(CompStateIdNo) = Val(LedStateIdNo) Then
            sts = False
        Else
            sts = True
        End If

        Is_InterState_Party = sts

    End Function

    Public Shared Sub FillRegionRectangle(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal X1axis As Decimal, ByVal Y1axis As Decimal, ByVal X2axis As Decimal, ByVal Y2axis As Decimal)
        Dim Hght As Double = 0
        Dim Wdth As Double = 0

        ' Create solid brush.
        Dim blueBrush As New SolidBrush(Color.FromArgb(235, 235, 235))

        Wdth = X2axis - X1axis
        Hght = Y2axis - Y1axis

        ' Create rectangle for region.
        Dim fillRect As New Rectangle(X1axis, Y1axis, Wdth, Hght)

        ' Create region for fill.
        Dim fillRegion As New [Region](fillRect)

        ' Fill region to screen.
        e.Graphics.FillRegion(blueBrush, fillRegion)

    End Sub
    Public Shared Function SimpleEmployee_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vEmployee_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vEmployee_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Employee_IdNo from Employee_Head where Employee_Name = '" & Trim(vEmployee_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vEmployee_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vEmployee_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        SimpleEmployee_NameToIdNo = Val(vEmployee_ID)

    End Function

    Public Shared Function SimpleEmployee_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vEmployee_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vEmployee_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Employee_Name from Employee_Head where Employee_IdNo = " & Str(Val(vEmployee_ID)), Cn1)
        Da.Fill(Dt)

        vEmployee_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vEmployee_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        SimpleEmployee_IdNoToName = Trim(vEmployee_Nm)

    End Function
    Public Shared Function Site_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vGender_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vGender_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Site_IdNo from Site_Head where Site_Name = '" & Trim(vGender_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vGender_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vGender_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Site_NameToIdNo = Val(vGender_ID)

    End Function

    Public Shared Function Site_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vGender_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vGender_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Site_Name from Site_Head where Site_IdNo = " & Str(Val(vGender_ID)), Cn1)
        Da.Fill(Dt)

        vGender_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vGender_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Site_IdNoToName = Trim(vGender_Nm)

    End Function

    Public Shared Function Encrypt(ByVal plainText As String, ByVal passPhrase As String, ByVal saltValue As String) As String
        Dim hashAlgorithm As String = "SHA1"

        Dim passwordIterations As Integer = 2
        Dim initVector As String = "@1B2c3D4e5F6g7H8"
        Dim keySize As Integer = 256

        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)

        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)

        Dim mypassword As New Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations)

        Dim keyBytes As Byte() = mypassword.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()

        symmetricKey.Mode = CipherMode.CBC

        Dim encryptor As ICryptoTransform = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)

        Dim memoryStream As New MemoryStream()
        Dim cryptoStream As New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)

        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)
        cryptoStream.FlushFinalBlock()
        Dim cipherTextBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()
        cryptoStream.Close()
        Dim cipherText As String = Convert.ToBase64String(cipherTextBytes)
        Return cipherText
    End Function

    Public Shared Function Decrypt(ByVal cipherText As String, ByVal passPhrase As String, ByVal saltValue As String) As String
        Dim plainText As String = ""

        Try

            'Dim passPhrase As String = "T.ThanGesWaran"
            'Dim saltValue As String = "N.VaRaLakshmi"
            Dim hashAlgorithm As String = "SHA1"

            Dim passwordIterations As Integer = 2
            Dim initVector As String = "@1B2c3D4e5F6g7H8"
            Dim keySize As Integer = 256
            ' Convert strings defining encryption key characteristics into byte
            ' arrays. Let us assume that strings only contain ASCII codes.
            ' If strings include Unicode characters, use Unicode, UTF7, or UTF8
            ' encoding.
            Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
            Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)

            ' Convert our ciphertext into a byte array.
            Dim cipherTextBytes As Byte() = Convert.FromBase64String(cipherText)

            ' First, we must create a password, from which the key will be 
            ' derived. This password will be generated from the specified 
            ' passphrase and salt value. The password will be created using
            ' the specified hash algorithm. Password creation can be done in
            ' several iterations.
            Dim mypassword As New Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations)
            'Dim mypassword As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)

            ' Use the password to generate pseudo-random bytes for the encryption
            ' key. Specify the size of the key in bytes (instead of bits).
            Dim keyBytes As Byte() = mypassword.GetBytes(keySize \ 8)

            ' Create uninitialized Rijndael encryption object.
            Dim symmetricKey As New RijndaelManaged()

            ' It is reasonable to set encryption mode to Cipher Block Chaining
            ' (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC

            ' Generate decryptor from the existing key bytes and initialization 
            ' vector. Key size will be defined based on the number of the key 
            ' bytes.
            Dim decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)

            ' Define memory stream which will be used to hold encrypted data.
            Dim memoryStream As New MemoryStream(cipherTextBytes)

            ' Define cryptographic stream (always use Read mode for encryption).
            Dim cryptoStream As New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)

            ' Since at this point we don't know what the size of decrypted data
            ' will be, allocate the buffer long enough to hold ciphertext;
            ' plaintext is never longer than ciphertext.
            Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}

            ' Start decrypting.
            Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)

            ' Close both streams.
            memoryStream.Close()
            cryptoStream.Close()

            ' Convert decrypted data into a string. 
            ' Let us assume that the original plaintext string was UTF8-encoded.
            plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)

            ' Return decrypted string.   
            Return plainText

        Catch ex As Exception
            plainText = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
            Return plainText

        End Try

    End Function

    Public Shared Function GetDriveSerialNumber(ByVal DriveLetter As String) As String
        Try
            Dim disk As ManagementObject = New ManagementObject(String.Format("Win32_Logicaldisk='{0}'", DriveLetter))
            Dim VolumeName As String = disk.Properties("VolumeName").Value.ToString()
            Dim SerialNumber As String = disk.Properties("VolumeSerialnumber").Value.ToString()
            Return SerialNumber
            'Return SerialNumber.Insert(4, "-")

        Catch ex As Exception
            Return ""

        End Try
    End Function

    Public Shared Function is_OfficeSystem() As Boolean
        Dim STS As Boolean = False

        Try

            Common_Procedures.DriveVolumeSerialName = ""
            Try
                Common_Procedures.DriveVolumeSerialName = Common_Procedures.GetDriveSerialNumber("D:")
            Catch ex As Exception
                '---
            End Try

            '---                                                       Server                                                               Mukilan                                                              Gopal               
            If Trim(UCase(Common_Procedures.DriveVolumeSerialName)) = "AEAA0163" Or Trim(UCase(Common_Procedures.DriveVolumeSerialName)) = "F0203A37" Or Trim(UCase(Common_Procedures.DriveVolumeSerialName)) = "424638AA" Then
                STS = True
            End If

            Return STS

        Catch ex As Exception
            Return ""

        End Try
    End Function
    Public Shared Function Scheme_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vColour_Nm As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vColour_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select Scheme_IdNo from Scheme_Head where Scheme_Name = '" & Trim(vColour_Nm) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vColour_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vColour_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Scheme_NameToIdNo = Val(vColour_ID)

    End Function

    Public Shared Function Scheme_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vColour_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vColour_Nm As String

        Da = New SqlClient.SqlDataAdapter("select Scheme_Name from Scheme_Head where Scheme_IdNo = " & Str(Val(vColour_ID)), Cn1)
        Da.Fill(Dt)

        vColour_Nm = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vColour_Nm = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Scheme_IdNoToName = Trim(vColour_Nm)

    End Function

    Public Shared Sub UpdateDefaultValuesForNewFields(ByVal cn1 As SqlClient.SqlConnection)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = cn1

        Cmd.CommandText = "Update Production_Details Set Ledger_IdNo = (Select Ledger_IdNo From Production_Head Where Production_Head.Production_Code = Production_Details.Production_Code) Where (Ledger_IdNo Is Null Or Ledger_IdNo =0)"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Update Sales_Details Set Close_Order =1 Where Close_Order Is Null"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Update Sales_Quotation_Head Set Remarks = '' Where Remarks Is Null"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Update Sales_Quotation_Head Set Finalised_rate = 0 Where Finalised_Rate Is Null"
        Cmd.ExecuteNonQuery()

    End Sub

    Public Shared Function Esi_Pf_Group_IdNoToName(ByVal Cn1 As SqlClient.SqlConnection, ByVal vESIPFGROUP_ID As Integer) As String
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vEsiPfGroup_Name As String

        Da = New SqlClient.SqlDataAdapter("select ESI_PF_Group_Name from ESI_PF_Head where ESI_PF_Group_IdNo = " & Str(Val(vESIPFGROUP_ID)), Cn1)
        Da.Fill(Dt)

        vEsiPfGroup_Name = ""
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vEsiPfGroup_Name = Trim(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Esi_Pf_Group_IdNoToName = Trim(vEsiPfGroup_Name)

    End Function

    Public Shared Function Esi_Pf_Group_NameToIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vEsiPfGroup_Name As String, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vESIPFGROUP_ID As Integer

        Da = New SqlClient.SqlDataAdapter("select ESI_PF_Group_IdNo from ESI_PF_Head where ESI_PF_Group_Name = '" & Trim(vEsiPfGroup_Name) & "'", Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vESIPFGROUP_ID = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vESIPFGROUP_ID = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Esi_Pf_Group_NameToIdNo = Val(vESIPFGROUP_ID)

    End Function

   
    Public Shared Sub AccountsVoucher_Posting_For_ProfitAndLoss()
        Dim Cn1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim DT1 As New DataTable
        Dim DT2 As New DataTable
        Dim vAmt As String = ""
        Dim FnYr As String = ""
        Dim FromDaTe As Date
        Dim ToDaTe As Date
        Dim NewCode As String = ""
        Dim vCompID As Integer = 0
        Dim Nr As Long = 0

        cn1.Open()

        cmd.Connection = Cn1

        FromDaTe = DateAdd("yyyy", -1, Common_Procedures.Company_FromDate)
        ToDaTe = DateAdd("yyyy", -1, Common_Procedures.Company_ToDate)
        FnYr = Trim(Right((Year(Common_Procedures.Company_FromDate) - 1), 2)) & "-" & Trim(Right(Year(Common_Procedures.Company_FromDate), 2))

        cmd.CommandText = "delete from voucher_details where entry_identification LIKE 'PR&LS-%" & Trim(FnYr) & "'"
        cmd.ExecuteNonQuery()

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@FromDate", FromDaTe)
        cmd.Parameters.AddWithValue("@ToDate", ToDaTe)

        Da = New SqlClient.SqlDataAdapter("Select * from Company_Head Where Company_IdNo <> 0 Order by Company_IdNo", Cn1)
        DT1 = New DataTable
        Da.Fill(DT1)
        If DT1.Rows.Count > 0 Then
            For I = 0 To DT1.Rows.Count - 1
                vAmt = 0

                vCompID = Val(DT1.Rows(I).Item("Company_Idno").ToString)

                Da = New SqlClient.SqlDataAdapter("Select sum(b.voucher_amount) from ledger_head a, voucher_details b where b.company_idno = " & Str(Val(vCompID)) & " and b.voucher_date between '" & Trim(Format(DateAdd("d", 1, DateAdd("yyyy", -1, ToDaTe)), "MM/dd/yyyy")) & "' and '" & Trim(Format(ToDaTe, "MM/dd/yyyy")) & "' and ( a.parent_code like '%~18~' ) and b.year_for_report < " & Str(Year(ToDaTe)) & " and a.ledger_idno = b.ledger_idno", Cn1)
                DT2 = New DataTable
                Da.Fill(DT2)
                If DT2.Rows.Count > 0 Then
                    If IsDBNull(DT2.Rows(0)(0).ToString) = False Then
                        vAmt = Val(DT2.Rows(0)(0).ToString)
                    End If
                End If
                DT2.Clear()

                If Val(vAmt) <> 0 Then

                    cmd.CommandText = "truncate table reporttempsub"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "insert into reporttempsub ( date1, currency1 ) Select tZ.Closing_Stock_Value_Date, sum(tz.Closing_Stock_Value) from Closing_Stock_Value_Head tz where tZ.company_idno = " & Str(Val(vCompID)) & " and tz.Closing_Stock_Value_Date <= @FromDaTe group by tZ.Closing_Stock_Value_Date Having sum(tz.Closing_Stock_Value) <> 0"
                    Nr = cmd.ExecuteNonQuery()

                    '----OPENING STOCK
                    cmd.CommandText = "Select top 1 Currency1 as OpStockValue from ReportTempSub where date1 <= @FromDaTe Order by date1 desc"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    DT2 = New DataTable
                    Da.Fill(DT2)
                    If DT2.Rows.Count > 0 Then
                        If IsDBNull(DT2.Rows(0)(0).ToString) = False Then
                            vAmt = Val(vAmt) - Val(DT2.Rows(0)(0).ToString)
                        End If
                    End If
                    DT2.Clear()


                    cmd.CommandText = "truncate table reporttempsub"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "insert into reporttempsub ( date1, currency1 ) Select tZ.Closing_Stock_Value_Date, sum(tz.Closing_Stock_Value) from Closing_Stock_Value_Head tz where tZ.company_idno = " & Str(Val(vCompID)) & " and tz.Closing_Stock_Value_Date <= @todate group by tZ.Closing_Stock_Value_Date Having sum(tz.Closing_Stock_Value) <> 0"
                    Nr = cmd.ExecuteNonQuery()

                    '----CLOSING STOCK
                    cmd.CommandText = "Select top 1 Currency1 as OpStockValue from ReportTempSub where date1 <= @todate Order by date1 desc"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    DT2 = New DataTable
                    Da.Fill(DT2)
                    If DT2.Rows.Count > 0 Then
                        If IsDBNull(DT2.Rows(0)(0).ToString) = False Then
                            vAmt = Val(vAmt) + Val(DT2.Rows(0)(0).ToString)
                        End If
                    End If
                    DT2.Clear()

                    NewCode = "PR&LS-" & Trim(Val(vCompID)) & "-2000/" & Trim(FnYr)

                    cmd.CommandText = "delete from voucher_details where entry_identification = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into Voucher_Details (     Voucher_Code      , For_OrderByCode ,          Company_IdNo    , Voucher_No, For_OrderBy , Voucher_Type, Voucher_Date, SL_No , Ledger_IdNo ,        Voucher_Amount , Narration,         Year_For_Report     ,   Entry_Identification  ) " & _
                                        "          Values          ('" & Trim(NewCode) & "',     -2000       , " & Str(Val(vCompID)) & ",  '-2000'  ,    -2000    ,   'P&L.Jrnl'   , @ToDate     ,   13  ,     13      , " & Str(Val(vAmt)) & ",    ''    , " & Str(Val(Year(ToDaTe))) & ", '" & Trim(NewCode) & "' ) "
                    cmd.ExecuteNonQuery()


                End If


            Next I

        End If
        DT1.Clear()

        DT1.Dispose()
        DT2.Dispose()
        Da.Dispose()

        Cn1.Close()
        Cn1.Dispose()

    End Sub

    Public Shared Function LocateUserInfo(mnuName As String) As Integer

        LocateUserInfo = -1

        If UR1.UserInfo.GetUpperBound(0) > -1 Then
            For J As Integer = 0 To UR1.UserInfo.GetUpperBound(0)

                If UCase(mnuName) = UCase(UR1.UserInfo(J, 0)) Then
                    LocateUserInfo = J
                End If

            Next
        End If

    End Function

End Class
