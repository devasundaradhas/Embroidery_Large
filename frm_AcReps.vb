Public Class frm_AcReps
    '    Option Explicit
    '    Public RptHeading1 As String
    '    Public RptHeading2 As String
    '    Public RptHeading3 As String

    '    Private PROC As Object
    '    Private Mdi1 As Object
    '    Private FrmNm As Object
    '    Private Grid1 As Object

    '    Private cn1 As ADODB.Connection
    '    Private Comp_IdNo As Integer
    '    Private Rpt_Main As String
    '    Private Rpt_Sub As String
    '    Private Cmp_FnYear As String
    '    Private Cmp_FromDt As Date
    '    Private Cmp_ToDt As Date
    '    Private Cmp_Name As String
    '    Private Cmp_Address As String

    '    Private RptDet_IdNo1 As Integer
    '    Private RptDet_IdNo2 As Integer
    '    Private RptDet_IdNo3 As Integer
    '    Private RptDet_Name1 As String
    '    Private RptDet_Name2 As String
    '    Private RptDet_Name3 As String
    '    Private RptDet_Tex_Val1 As String
    '    Private RptDet_Tex_Val2 As String
    '    Private RptDet_Date1 As Date
    '    Private RptDet_Date2 As Date
    '    Private month_idno As Integer

    '    Private Heading_1 As String, Heading_2 As String, Condt As String
    '    Private Field_1 As String, Field_2 As String, Rpt_Hd As String
    '    Private Format_1 As String, Format_2 As String, Report_PKey As String
    '    Private CompType_Condt As String

    '    Public Sub Report_Details(ByVal CN As ADODB.Connection, ByVal CompIdNo As Integer, ByVal MdiFrm1 As Object, ByVal FormName As Object, ByVal Grd1 As Object, ByVal RMain As String, ByVal RSub As String, ByVal FnYr As String, ByVal FromDt As Date, ByVal ToDt As Date, ByVal RptDt_IdNo1 As Integer, ByVal RptDt_IdNo2 As Integer, ByVal RptDt_IdNo3 As Integer, ByVal RptDt_Name1 As String, ByVal RptDt_Name2 As String, ByVal RptDt_Name3 As String, ByVal RptDt_Tex_Val1 As String, ByVal RptDt_Tex_Val2 As String, ByVal RptDt_Date1 As Date, ByVal RptDt_Date2 As Date, ByVal CompType_Condition As String)
    '        Dim i As Integer
    '        Dim t1 As Single

    '        PROC = GetObject("", "Smart_Procedures_NT10.Basic_Procedures")

    '        cn1 = CN
    '        Mdi1 = MdiFrm1
    '        FrmNm = FormName
    '        Grid1 = Grd1
    '        Comp_IdNo = CompIdNo

    '        RptDet_IdNo1 = RptDt_IdNo1
    '        RptDet_IdNo2 = RptDt_IdNo2
    '        RptDet_IdNo3 = RptDt_IdNo3
    '        RptDet_Name1 = RptDt_Name1
    '        RptDet_Name2 = RptDt_Name2
    '        RptDet_Name3 = RptDt_Name3
    '        RptDet_Tex_Val1 = RptDt_Tex_Val1
    '        RptDet_Tex_Val2 = RptDt_Tex_Val2
    '        RptDet_Date1 = RptDt_Date1
    '        RptDet_Date2 = RptDt_Date2

    '        Rpt_Main = RMain
    '        Rpt_Sub = RSub
    '        Cmp_FnYear = FnYr
    '        Cmp_FromDt = FromDt
    '        Cmp_ToDt = ToDt
    '        CompType_Condt = CompType_Condition

    '        Call Report_Intialize(cn1)

    '        FormName.Height = MdiFrm1.Height - 1885
    '        Grid1.Height = FormName.Height
    '        FormName.Width = MdiFrm1.Width - 200
    '        FormName.Left = 0 : FormName.Top = 0
    '        FormName.BackColor = RGB(250, 250, 250)

    '        Grid1.BackColorBkg = FormName.BackColor
    '        Grid1.BackColor = RGB(255, 255, 255)
    '        Grid1.ForeColor = RGB(0, 0, 0)
    '        Grid1.ForeColorFixed = RGB(9, 125, 122)
    '        Grid1.BackColorFixed = RGB(230, 230, 230)
    '        Grid1.BackColorSel = RGB(230, 238, 215)
    '        Grid1.ForeColorSel = RGB(3, 10, 245)
    '        Grid1.Rows = 2
    '        Grid1.FixedRows = 1

    '        Select Case Rpt_Main
    '            Case "DAY BOOK"
    '                Call Accounts_DayBook()
    '            Case "Negative Cash"
    '                Call Accounts_NegativeCash()
    '            Case "LEDGER A/C", "BANK BOOK", "CASH BOOK", "PURCHASE BOOK", "SALES BOOK", "LEDGER A/C (LW)", "LEDGER A/C - Confirmation Details"
    '                Call Accounts_SingleLedger()
    '                MdiFrm1.StatusBar4.Panels.Clear()
    '                MdiFrm1.StatusBar4.Panels.Add, "Day Balance", "F2 - Day Balance"
    '                MdiFrm1.StatusBar4.Panels.Item(1).Width = 1630
    '                MdiFrm1.StatusBar4.Panels.Item(1).Alignment = 1
    '                MdiFrm1.StatusBar4.Panels.Add, "With Out Particulars", "F3 - With Out Particulars"
    '                MdiFrm1.StatusBar4.Panels.Item(2).Width = 2300
    '                MdiFrm1.StatusBar4.Panels.Item(2).Alignment = 1
    '                MdiFrm1.StatusBar4.Panels.Add, "Print", "F11 - Print"
    '                MdiFrm1.StatusBar4.Panels.Item(3).Width = 1200
    '                MdiFrm1.StatusBar4.Panels.Item(3).Alignment = 1
    '                MdiFrm1.StatusBar4.Panels.Add, "Close", "Esc - Close"
    '                MdiFrm1.StatusBar4.Panels.Item(4).Width = 1300
    '                MdiFrm1.StatusBar4.Panels.Item(4).Alignment = 1
    '                MdiFrm1.StatusBar4.Panels.Add, "Empty", ""
    '                MdiFrm1.StatusBar4.Panels.Item(5).Width = 5500
    '                MdiFrm1.StatusBar3.Visible = False
    '                MdiFrm1.StatusBar1.Visible = False
    '                MdiFrm1.StatusBar4.Visible = True
    '                MdiFrm1.StatusBar3.Visible = True
    '            Case "SUNDRY BOOK", "Single Ledger - Details"
    '                Call Accounts_SundryBook()
    '            Case "LEDGER A/C (MONTHLY)"
    '                Call Accounts_MonthLedger()
    '            Case "Opening TB", "OPENING TRIAL BALANCE"
    '                Call Accounts_OpeningTB()
    '            Case "General TB", "GENERAL TRIAL BALANCE"
    '                Call Accounts_GeneralTB()
    '            Case "GROUP TRIAL BALANCE"
    '                Call Accounts_GroupTB()
    '            Case "Final TB"
    '                Call Accounts_FinalTB()
    '            Case "Group Ledger - Details"
    '                Call Accounts_GroupLedger_Details()
    '            Case "ALL LEDGER"
    '                Call Accounts_AllLedger()
    '            Case "Bank/Cash -Inflow/Outflow"
    '                Call Accounts_BankCash_InflowOutflow()
    '            Case "Bank And Cash Transaction Details"
    '                Call Accounts_BankCash_Transaction_Details()
    '            Case "Bank And Cash Transaction Summary"
    '                Call Accounts_BankCash_Transaction_Summary()
    '            Case "Entry List (Month Wise)"
    '                Call EntryList_MonthWise()
    '            Case "Entry List (Detail)"
    '                Call EntryList_Detail()
    '            Case "All Ledger"
    '                Call Accounts_AllLedger()
    '            Case "Customer Bill Details Single"
    '                Call Bills_Customer_Details_Single()
    '            Case "Customer Bill Pending Single"
    '                Call Bills_Customer_Pending_Single()
    '            Case "Customer Bill Pending All", "Customer Bill Pending Purchased Bills", "Customer Bill Pending Invoiced Bills", "Customer Bill Pending Sizing Bills", "Customer Bill Pending Weaver Bills"
    '                Call Bills_Customer_Pending_All()
    '            Case "Agent Bill Details Single"
    '                Call Bills_Agent_Details_Single()
    '            Case "Agent Bill Pending Single"
    '                Call Bills_Agent_Bill_Pending_Single()
    '            Case "Agent Bill Pending All", "Agent Bill Pending Purchased", "Agent Bill Pending Invoiced"
    '                Call Bills_Agent_Pending_All()
    '            Case "Customer Aging Analysis"
    '                Call Bills_Customer_Pending_AgingAnalysis()
    '            Case "Agent Aging Analysis"
    '                Call Bills_Agent_Pending_AgingAnalysis()

    '        End Select

    '        Grid1.RowHeight(0) = 350
    '        Grid1.Row = 0
    '        For i = 0 To Grid1.Cols - 1
    '            Grid1.Col = i
    '            Grid1.CellFontSize = 8
    '            Grid1.CellFontName = "Ms Sans Serif"
    '        Next i
    '        MdiFrm1.StatusBar3.Panels(1).Text = ""
    '        MdiFrm1.StatusBar3.Panels(2).Text = ""
    '        FormName.Shape1.FillColor = RGB(162, 162, 162)
    '        Grid1.Row = 0
    '        For i = 0 To Grid1.Cols - 1
    '            t1 = t1 + Grid1.ColWidth(i)
    '            Grid1.Col = i
    '            Grid1.CellAlignment = 4
    '            Grid1.CellFontBold = True
    '        Next i
    '        If t1 < Screen.Width - 500 Then
    '            Grid1.Width = t1 + 250
    '            Grid1.Left = Int((FormName.Width - t1) / 2)
    '        Else
    '            Grid1.Left = 50
    '            Grid1.Width = Screen.Width - 50
    '            FormName.Label1(0).Width = 3850
    '        End If
    '        t1 = 0
    '        FormName.Height = MdiFrm1.Height - 1900
    '        Grid1.Height = FormName.Height - 350
    '        Grid1.Row = 1 : Grid1.Col = 0
    '        FormName.Label1(0).Width = FormName.Width
    '        FormName.Shape1.Width = FormName.Width
    '        MdiFrm1.MousePointer = 0
    '        FormName.MousePointer = 0
    '        Grid1.Visible = True
    '        FormName.Label1(0).Visible = True
    '        FormName.Shape1.Visible = True

    '    End Sub

    '    Private Sub Accounts_DayBook()
    '        Dim Rs1 As Recordset, Rt1 As Recordset
    '        Dim Tot_CR As Currency, Tot_Dr As Currency
    '        Dim Dt1 As Date, dt2 As Date

    '        Grid1.FormatString = "<DATE         |<COMP. |<VOU NO            |<PARTICULARS                                                     |<TYPE   |>DB.AMOUNT      |>CR.AMOUNT       |< NARRATION                                       |<ENT-ID"
    '        Grid1.ColWidth(8) = 0
    '        RptHeading1 = "DAY BOOK " & Set_Details("Company")
    '        RptHeading2 = "RANGE : " & Format(RptDet_Date1, "dd-mm-yyyy") & " TO " & Format(RptDet_Date2, "dd-mm-yyyy")
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2

    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select sum(voucher_amount) from voucher_details a, company_head tz where ledger_idno = 1 and " & Condt & IIf(Condt <> "", " and ", "") & " voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and a.company_idno = tz.company_idno", cn1, adOpenStatic, adLockReadOnly)
    '        If Rs1(0).Value <> "" Then If Val(Rs1(0).Value) > 0 Then Tot_Dr = Val(Rs1(0).Value) Else Tot_CR = Abs(Val(Rs1(0).Value))
    '        Rs1.Close()
    '        Rs1 = Nothing

    '        Rt1 = New ADODB.Recordset
    '        Rt1.Open("Select tz.company_shortname, a.*, b.ledger_idno, c.ledger_name, b.voucher_amount, b.narration, c.parent_code, voucher_type from voucher_head a, voucher_details b, ledger_head c, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and tz.company_idno = b.company_idno and a.company_idno = b.company_idno and b.ledger_idno = c.ledger_idno order by a.voucher_date, a.company_idno, a.for_orderby, b.sl_no", cn1, adOpenStatic, adLockReadOnly)
    '        With Rt1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Grid1.AddItem(Format(!Voucher_Date, "dd-mm-yy") & vbTab & vbTab & vbTab & "DAY OPENING" & Chr(9) & "" & Chr(9) & IIf(Tot_Dr > 0, PROC.Currency_Format(Tot_Dr), "") & Chr(9) & IIf(Tot_CR > 0, PROC.Currency_Format(Tot_CR), IIf(Tot_Dr = 0, "0.00", "")))
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '                Do While Not .EOF
    '                    If InStr(!Parent_Code, "~6~4~") = 0 Then
    '                        Grid1.AddItem("" & Chr(9) & !Company_ShortName & vbTab & Left(!entry_identification, Len(!entry_identification) - 6) & vbTab & IIf(!Voucher_Amount > 0, "To ", "By ") & !ledger_name & Chr(9) & !Voucher_Type & Chr(9) & IIf(!Voucher_Amount < 0, "", vbTab) & PROC.Currency_Format(Abs(!Voucher_Amount)) & IIf(!Voucher_Amount > 0, "", vbTab) & Chr(9) & !Narration & vbTab & !entry_identification)
    '                        If !Voucher_Amount > 0 Then Tot_CR = Tot_CR + !Voucher_Amount Else Tot_Dr = Tot_Dr + Abs(!Voucher_Amount)
    '                    End If
    '                    Dt1 = Trim(!Voucher_Date)
    '                    .MoveNext()
    '                    If Not .EOF Then dt2 = Trim(!Voucher_Date) Else dt2 = DateAdd("d", 1, Dt1)
    '                    If Dt1 <> dt2 Then
    '                        Mdi1.StatusBar3.Panels(2).Text = Trim(Format(dt2, "dd mmmm"))
    '                        Grid1.AddItem("" & Chr(9) & vbTab & vbTab & "TOTAL" & Chr(9) & "" & Chr(9) & PROC.Currency_Format(Tot_Dr) & Chr(9) & PROC.Currency_Format(Tot_CR))
    '                        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1, "248,248,248") ', 2, 5)
    '                        If Tot_CR > Tot_Dr Then
    '                            Tot_CR = Tot_CR - Tot_Dr : Tot_Dr = 0
    '                        Else
    '                            Tot_Dr = Tot_Dr - Tot_CR : Tot_CR = 0
    '                        End If
    '                        Grid1.AddItem("" & Chr(9) & vbTab & vbTab & "DAY CLOSING" & Chr(9) & "" & Chr(9) & IIf(Tot_Dr <> 0, PROC.Currency_Format(Tot_Dr), "") & Chr(9) & IIf(Tot_CR <> 0, PROC.Currency_Format(Tot_CR), ""))
    '                        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1, "247,247,247") ', 2, 5)
    '                        Grid1.AddItem("")
    '                        Grid1.AddItem(Format(dt2, "dd-mm-yy") & vbTab & vbTab & vbTab & "DAY OPENING" & Chr(9) & "" & Chr(9) & IIf(Tot_Dr <> 0, PROC.Currency_Format(Tot_Dr), "") & Chr(9) & IIf(Tot_CR <> 0, PROC.Currency_Format(Tot_CR), ""))
    '                        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '                    End If
    '                Loop
    '                Grid1.RemoveItem(Grid1.Rows - 1)
    '                Grid1.RemoveItem(Grid1.Rows - 1)
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing

    '    End Sub

    '    Private Sub Accounts_SingleLedger()
    '        Dim Rs1 As Recordset, Rt1 As Recordset
    '        Dim Ttc As Currency, Ttd As Currency
    '        Dim dt_cndt As String, GpCd As String, ent_idno As String

    '        Grid1.FormatString = "<DATE          |<ENT ID         |<COMP. |<PARTICULARS                                           |<PARTICULARS                                           |<TYPE   |>DB.AMOUNT       |>CR.AMOUNT       |>BALANCE             |<NARRATION                                  |<VOU.NO"
    '        Grid1.ColWidth(3) = 3000 : Grid1.ColWidth(4) = 2000 : Grid1.ColWidth(8) = 1800 : Grid1.ColWidth(9) = 2800 : Grid1.ColWidth(10) = 0

    '        If Rpt_Main = "LEDGER A/C - Confirmation Details" Then
    '            RptHeading1 = "CONFIRMATION OF ACCOUNTS OF  - " & Set_Details("Ledger")
    '            Grid1.ColWidth(2) = 0
    '        Else
    '            RptHeading1 = "LEDGER A/C of " & Set_Details("Ledger")
    '        End If

    '        RptHeading2 = Set_Details("Company")
    '        RptHeading3 = "RANGE : " & Format(RptDet_Date1, "dd-mm-yyyy") & " TO " & Format(RptDet_Date2, "dd-mm-yyyy")
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3

    '        Rs1 = New ADODB.Recordset
    '        'Rs1.Open "Select parent_code from ledger_head tl, company_head tz where company_idno = 1 " & IIf(Condt <> "", " and ", "") & Condt, Cn1, adOpenStatic, adLockReadOnly
    '        Rs1.Open("Select parent_code from ledger_head tl, company_head tz " & IIf(Condt <> "", " where ", "") & Condt, cn1, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            Rs1.MoveFirst()
    '            GpCd = Rs1!Parent_Code
    '        End If
    '        Rs1.Close()
    '        Condt = Replace(Condt, "tZ.", "a.")
    '        Condt = Replace(Condt, "tL.", "a.")

    '        If GpCd Like "*~18~" Then dt_cndt = "voucher_date >= '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "'" Else dt_cndt = "voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "'"

    '        Rs1.Open("Select sum(voucher_amount) from voucher_details a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & dt_cndt & " and a.company_idno = tz.company_idno", cn1, adOpenStatic, adLockReadOnly)
    '        If Rs1(0).Value <> "" Then Ttc = Val(Rs1(0).Value)
    '        Rs1.Close()
    '        Rs1 = Nothing

    '        If Ttc <> 0 Then Grid1.AddItem("" & vbTab & vbTab & vbTab & "   OPENING BALANCE" & vbTab & "   OPENING BALANCE" & vbTab & "" & vbTab & IIf(Ttc < 0, "", vbTab) & PROC.Currency_Format(Abs(Ttc)))
    '        If Ttc < 0 Then Ttd = Abs(Ttc) : Ttc = 0
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)


    '        Rs1 = New ADODB.Recordset
    '        If Rpt_Main = "LEDGER A/C (LW)" Or Rpt_Main = "LEDGER A/C - Confirmation Details" Then
    '            Rs1.Open("Select e.company_shortname, b.entry_identification, f.voucher_date, (-1*f.voucher_amount) as voucher_amount, b.voucher_no, b.voucher_type, b.entry_identification, c.ledger_name as party_name, a.narration from voucher_details a, voucher_head b, ledger_head c, company_head e, voucher_details f where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and a.company_idno = b.company_idno and f.voucher_ref_no = b.voucher_ref_no and f.company_idno = b.company_idno and f.ledger_idno = c.ledger_idno and f.ledger_idno <> a.ledger_idno and a.company_idno = e.company_idno order by a.voucher_date, b.for_orderby", cn1, adOpenStatic, adLockReadOnly)
    '        Else
    '            Rs1.Open("Select e.company_shortname, b.entry_identification, a.voucher_date, a.voucher_amount, b.voucher_no, b.voucher_type, b.entry_identification, (case when a.voucher_amount<0 then c.ledger_name else d.ledger_name end) as party_name, a.narration from voucher_details a, voucher_head b, ledger_head c, ledger_head d, company_head e where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and a.company_idno = b.company_idno and b.creditor_idno = c.ledger_idno and b.debtor_idno = d.ledger_idno and a.company_idno = e.company_idno order by a.voucher_date, b.for_orderby", cn1, adOpenStatic, adLockReadOnly)
    '        End If
    '        With Rs1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If !Voucher_Amount > 0 Then Ttc = Ttc + Val(!Voucher_Amount) Else Ttd = Ttd + Abs(Val(!Voucher_Amount))
    '                    If Left(!entry_identification, 6) = "VOUCH-" Then
    '                        ent_idno = UCase(!Voucher_Type) & "-" & !Voucher_No
    '                    Else
    '                        ent_idno = Replace(!entry_identification, "/" & Cmp_FnYear, "")
    '                    End If
    '                    '                        DATE                             ENT ID                  COMP.                                                 PARTICULARS                                                             PARTICULARS                                 TYPE                                                                        DB.AMOUNT                                                   CR.AMOUNT                                           BALANCE                                                     NARRATION                    VOU.NO
    '                    Grid1.AddItem(Format(!Voucher_Date, "dd-mm-yy") & vbTab & ent_idno & vbTab & !Company_ShortName & vbTab & IIf(!Voucher_Amount > 0, "By ", "To ") & Trim(!Party_name) & Chr(9) & Trim(StrConv(!Narration, vbProperCase)) & Chr(9) & Trim(!Voucher_Type) & Chr(9) & IIf(!Voucher_Amount < 0, PROC.Currency_Format(Abs(!Voucher_Amount)), "") & vbTab & IIf(!Voucher_Amount > 0, PROC.Currency_Format(Abs(!Voucher_Amount)), "") & vbTab & PROC.Currency_Format(Abs(Ttc - Ttd)) & IIf(Ttc > Ttd, " Cr", " Dr") & Chr(9) & Trim(!Narration) & Chr(9) & !entry_identification)
    '                    Mdi1.StatusBar3.Panels(2).Text = Format(!Voucher_Date, "dd mmm")
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing
    '        Grid1.AllowUserResizing = 0
    '        Grid1.ColWidth(4) = 0 : Grid1.ColWidth(8) = 0
    '        Grid1.AddItem("")
    '        Grid1.AddItem("" & vbTab & vbTab & vbTab & "TOTAL" & Chr(9) & "TOTAL" & Chr(9) & "" & Chr(9) & PROC.Currency_Format(Ttd) & Chr(9) & PROC.Currency_Format(Ttc))
    '        Grid1.AddItem("" & vbTab & vbTab & vbTab & "CLOSING BALANCE" & Chr(9) & "CLOSING BALANCE" & Chr(9) & "" & Chr(9) & IIf(Ttc - Ttd < 0, "", vbTab) & PROC.Currency_Format(Abs(Ttc - Ttd)))
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '    End Sub

    '    Private Sub Accounts_MonthLedger()
    '        Dim Ttc As Currency, Ttd As Currency
    '        Dim Rs As ADODB.Recordset
    '        Dim m1 As Integer, a1 As Integer
    '        Dim Fnt As Currency
    '        Dim Opds As String, Clds As String
    '        Dim dt_cndt As String, GpCd As String

    '        Grid1.Cols = 3
    '        Grid1.FormatString = "<MONTH                         |>OPENING                    |>CR.AMOUNT                  |>DB.AMOUNT               |>CLOSING                "

    '        RptHeading1 = "LEDGER A/C of " & Set_Details("Ledger")
    '        RptHeading2 = Set_Details("Company")
    '        RptHeading3 = "Month of " & Set_Details("Month")

    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3

    '        Ttc = 0 : Ttd = 0 : Fnt = 0
    '        Opds = "0.00 Cr"

    '        Rs = New ADODB.Recordset
    '        Rs.Open("Select parent_code from ledger_head tl, company_head tz where company_idno = 1 " & IIf(Condt <> "", " and ", "") & Condt, cn1, adOpenStatic, adLockReadOnly)
    '        If Not (Rs.BOF And Rs.EOF) Then
    '            Rs.MoveFirst()
    '            GpCd = Rs!Parent_Code
    '        End If
    '        Rs.Close()
    '        Condt = Replace(Condt, "tZ.", "a.")
    '        Condt = Replace(Condt, "tL.", "a.")

    '        'If GpCd Like "*~18~" Then dt_cndt = "voucher_date >= '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "'" Else dt_cndt = "voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "'"

    '        If Not (GpCd Like "*~18~") Then
    '            Rs.Open("Select sum(voucher_amount) from voucher_details a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date < '" & Trim(Format(CmpDet.FromDate, "mm/dd/yyyy")) & "' and a.company_idno = tz.company_idno", cn1, adOpenStatic, adLockReadOnly)
    '            If Rs(0).Value <> "" Then
    '                Opds = PROC.Currency_Format(Abs(Val(Rs(0).Value))) & IIf(Val(Rs(0).Value) >= 0, " Cr", " Dr")
    '                Fnt = Val(Rs(0).Value)
    '            End If
    '            Rs.Close()
    '        End If

    '        a1 = IIf(month_idno < 4, 12, month_idno)
    '        For m1 = 4 To a1
    '            Rs.Open("Select sum(voucher_amount) from voucher_details a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " month(voucher_date) = " & Str(m1) & " and year(voucher_date) = " & Str(Year(Cmp_FromDt)) & " and voucher_amount > 0 and a.company_idno = tz.company_idno", cn1, adOpenStatic, adLockReadOnly)
    '            If Rs(0).Value <> "" Then Ttc = Val(Rs(0).Value) Else Ttc = 0
    '            Rs.Close()
    '            Rs.Open("Select sum(voucher_amount) from voucher_details a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " month(voucher_date) = " & Str(m1) & " and year(voucher_date) = " & Str(Year(Cmp_FromDt)) & " and voucher_amount < 0 and a.company_idno = tz.company_idno", cn1, adOpenStatic, adLockReadOnly)
    '            If Rs(0).Value <> "" Then Ttd = Val(Abs(Rs(0).Value)) Else Ttd = 0
    '            Rs.Close()
    '            Grid1.AddItem(UCase(MonthName(m1)) & Chr(9) & Trim(Opds) & Chr(9) & PROC.Currency_Format(Ttc) & Chr(9) & PROC.Currency_Format(Ttd))
    '            Fnt = Fnt + Ttc - Ttd
    '            Clds = IIf(Fnt < 0, PROC.Currency_Format(Abs(Fnt)) + " Dr", PROC.Currency_Format(Fnt) + " Cr")
    '            Opds = Clds
    '            Grid1.TextMatrix(Grid1.Rows - 1, 4) = Clds
    '            Mdi1.StatusBar3.Panels(2) = MonthName(m1)
    '        Next m1

    '        If Val(month_idno) < 4 Then
    '            For m1 = 1 To Val(month_idno)

    '                Rs.Open("Select sum(voucher_amount) from voucher_details a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " month(voucher_date) = " & Str(m1) & " and year(voucher_date) = " & Str(Year(Cmp_ToDt)) & " and voucher_amount > 0 and a.company_idno = tz.company_idno", cn1, adOpenStatic, adLockReadOnly)
    '                If Rs(0).Value <> "" Then Ttc = Val(Rs(0).Value) Else Ttc = 0
    '                Rs.Close()
    '                Rs.Open("Select sum(voucher_amount) from voucher_details a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " month(voucher_date) = " & Str(m1) & " and year(voucher_date) = " & Str(Year(Cmp_ToDt)) & " and voucher_amount < 0 and a.company_idno = tz.company_idno", cn1, adOpenStatic, adLockReadOnly)
    '                If Rs(0).Value <> "" Then Ttd = Val(Abs(Rs(0).Value)) Else Ttd = 0
    '                Rs.Close()

    '                Grid1.AddItem(UCase(MonthName(m1)) & Chr(9) & Trim(Opds) & Chr(9) & PROC.Currency_Format(Ttc) & Chr(9) & PROC.Currency_Format(Ttd))
    '                Fnt = Fnt + Ttc - Ttd
    '                Clds = IIf(Fnt < 0, PROC.Currency_Format(Abs(Fnt)) + " Dr", PROC.Currency_Format(Fnt) + " Cr")
    '                Opds = Clds
    '                Grid1.TextMatrix(Grid1.Rows - 1, 4) = Clds
    '                Mdi1.StatusBar3.Panels(2) = UCase(MonthName(m1))
    '            Next m1
    '        End If
    '        Grid1.SelectionMode = 1
    '        Rs = Nothing
    '    End Sub

    '    Private Sub Accounts_OpeningTB()
    '        Dim tt1 As Currency, tt2 As Currency
    '        Dim Rt1 As Recordset

    '        If CmpDet.IdNo > 0 Then Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "tZ.company_idno in (" & Trim(CmpDet.IdNo) & ")"
    '        RptHeading1 = "OPENING TRIAL BALANCE " & Set_Details("Company")
    '        RptHeading2 = "AS ON DATE : " & Trim(Format(Cmp_FromDt, "dd-mm-yyyy"))
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2
    '        Grid1.Cols = 3
    '        Grid1.FormatString = "<PARTY NAME                                            |>DEBIT                        |>CREDIT                    "
    '        Grid1.SelectionMode = 1

    '        Condt = Replace(Condt, "tZ.", "a.")
    '        cn1.Execute("delete from reporttemp")
    '        cn1.Execute("insert into reporttemp ( int1, currency1 ) Select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head c, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date < '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and b.parent_code not like '%~18~' and a.ledger_idno = b.ledger_idno and c.parent_idno = b.parent_code and a.company_idno = tz.company_idno group by a.ledger_idno having sum(voucher_amount) <> 0")

    '        tt1 = 0 : tt2 = 0
    '        Rt1 = New ADODB.Recordset
    '        Rt1.Open("Select a.ledger_name, b.currency1 as opening_balance from ledger_head a, reporttemp b where a.ledger_idno = b.int1 order by a.ledger_name", cn1, adOpenStatic, adLockReadOnly)
    '        With Rt1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If !Opening_Balance < 0 Then
    '                        Grid1.AddItem(StrConv(!ledger_name, vbProperCase) & Chr(9) & PROC.Currency_Format(Abs(!Opening_Balance)))
    '                        tt1 = tt1 + Abs(!Opening_Balance)
    '                    Else
    '                        Grid1.AddItem(StrConv(!ledger_name, vbProperCase) & Chr(9) & "" & Chr(9) & PROC.Currency_Format(Abs(!Opening_Balance)))
    '                        tt2 = tt2 + !Opening_Balance
    '                    End If
    '                    .MoveNext()
    '                Loop
    '                If tt1 > 0 Or tt2 > 0 Then
    '                    Grid1.AddItem("")
    '                    Grid1.AddItem("TOTAL" & Chr(9) & PROC.Currency_Format(tt1) & Chr(9) & PROC.Currency_Format(tt2))
    '                        If tt1 <> tt2 Then If tt1 > tt2 Then Grid1.AddItem "CLOSING BALANCE" & Chr(9) & PROC.Currency_Format(tt1 - tt2) Else Grid1.AddItem "CLOSING BALANCE" & Chr(9) & "" & Chr(9) & PROC.Currency_Format(tt2 - tt1)
    '                End If
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            End If
    '        End With
    '        Rt1 = Nothing
    '    End Sub

    '    Private Sub Accounts_GeneralTB()
    '        Dim Rt1 As Recordset
    '        Dim tt2 As Currency, tt3 As Currency, Tot1 As Currency

    '        Grid1.Cols = 3
    '        Grid1.FormatString = "<PARTY NAME                                                   |>DEBIT                            |>CREDIT                     "
    '        Grid1.SelectionMode = 1
    '        RptHeading1 = "GENERAL TRIAL BALANCE " & Set_Details("Company")
    '        RptHeading2 = "AS ON : " & Format(RptDet_Date1, "dd/mm/yyyy")
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2

    '        tt2 = 0 : tt3 = 0
    '        Condt = Replace(Condt, "tZ.", "a.")
    '        cn1.Execute("delete from reporttemp")
    '        cn1.Execute("insert into reporttemp ( int1, currency1 ) Select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head c, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date between '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and b.parent_code like '%~18~' and a.ledger_idno = b.ledger_idno and b.parent_code = c.parent_idno and a.company_idno = tz.company_idno group by a.ledger_idno having sum(voucher_amount) <> 0")
    '        cn1.Execute("insert into reporttemp ( int1, currency1 ) Select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head c, company_head tz  where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and year_for_report < " & Str(Year(Cmp_ToDt)) & " and b.parent_code not like '%~18~' and a.ledger_idno = b.ledger_idno and b.parent_code = c.parent_idno and a.company_idno = tz.company_idno group by a.ledger_idno having sum(voucher_amount) <> 0")

    '        Rt1 = New ADODB.Recordset
    '        Rt1.Open("Select a.ledger_idno, a.ledger_name, b.currency1 as sumofamount from ledger_head a, reporttemp b where b.currency1 <> 0 and a.ledger_idno = b.int1 order by a.ledger_name", cn1, adOpenStatic, adLockReadOnly)
    '        With Rt1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If !sumofamount < 0 Then
    '                        Grid1.AddItem !ledger_name & Chr(9) & PROC.Currency_Format(Abs(!sumofamount))
    '                        tt2 = tt2 + Abs(!sumofamount)
    '                    Else
    '                        Grid1.AddItem !ledger_name & Chr(9) & "" & Chr(9) & PROC.Currency_Format(Abs(!sumofamount))
    '                        tt3 = tt3 + Abs(!sumofamount)
    '                    End If
    '                    Grid1.RowData(Grid1.Rows - 1) = !Ledger_IdNo
    '                    Mdi1.StatusBar3.Panels(2).Text = StrConv(!ledger_name, vbProperCase)
    '                    .MoveNext()
    '                Loop
    '                If tt2 <> 0 Or tt3 <> 0 Then
    '                    Grid1.AddItem("")
    '                    Grid1.AddItem("TOTAL" & Chr(9) & PROC.Currency_Format(tt2) & Chr(9) & PROC.Currency_Format(tt3))
    '                    If tt2 <> tt3 Then If tt2 > tt3 Then Grid1.AddItem "CLOSING BALANCE" & Chr(9) & PROC.Currency_Format(tt2 - tt3) Else Grid1.AddItem "Closing Balance" & Chr(9) & "" & Chr(9) & PROC.Currency_Format(tt3 - tt2)
    '                    Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                    Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1, , 1)
    '                    Grid1.Row = Grid1.Rows - 1
    '                    Grid1.Col = 0 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                    Grid1.Col = 1 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                    Grid1.Col = 2 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                Else
    '                    If Grid1.Rows > 4 Then Grid1.Rows = Grid1.Rows - 2
    '                End If
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing

    '    End Sub

    '    Private Sub Accounts_GroupTB()
    '        Dim tt1 As Currency, tt3 As Currency, tt4 As Currency
    '        Dim RT2 As Recordset, Rt1 As Recordset
    '        Dim Rw As Integer
    '        Dim amt As Currency
    '        Dim g_condt As String ', com_id As String
    '        Dim i As Integer, J As Integer, k As Integer
    '        Dim L As String, P As String
    '        Dim m() As String

    '        Grid1.Cols = 3
    '        Grid1.FormatString = "<PARTY NAME                                                   |>DEBIT                         |>CREDIT                    "
    '        RptHeading1 = "GROUP TRAIL BALANCE " & Set_Details("Group")
    '        RptHeading2 = Set_Details("Company")
    '        RptHeading3 = "AS ON : " & Format(RptDet_Date1, "dd/mm/yyyy")
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3
    '        Grid1.SelectionMode = 1

    '        Condt = Replace(Condt, "tZ.", "a.")
    '        If InStr(Condt, "tG.Group_Idno in") > 0 Then
    '            i = InStr(Condt, "tG.Group_Idno in ")
    '            J = InStr(i, Condt, "(")
    '            k = InStr(J, Condt, ")")
    '            L = Mid(Condt, J + 1, k - J - 1)
    '            m = Split(L, ",")
    '            For J = 0 To UBound(m)
    '                P = P & IIf(P <> "", " or ", "") & " tG.parent_idno like '%" & Cmpr.Get_FieldValue(Con, "group_head", "parent_idno", "group_idno = " & Str(Val(m(J)))) & "' "
    '            Next J
    '            Condt = Replace(Condt, Mid(Condt, i, k - i + 1), " ( " & P & " )")
    '        End If

    '        cn1.Execute("delete from reporttemp")
    '        cn1.Execute("insert into reporttemp ( name1, int1, currency1 ) Select b.parent_code, a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head tg, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date between '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and year_for_report < " & Str(Year(Cmp_ToDt)) & " and b.parent_code like '%~18~' and a.ledger_idno = b.ledger_idno and b.parent_code = tg.parent_idno and a.company_idno = tz.company_idno group by b.parent_code, a.ledger_idno having sum(voucher_amount) <> 0")
    '        cn1.Execute("insert into reporttemp ( name1, int1, currency1 ) Select b.parent_code, a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head tg, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and b.parent_code not like '%~18~' and year_for_report < " & Str(Year(Cmp_ToDt)) & " and a.ledger_idno = b.ledger_idno and b.parent_code = tg.parent_idno and a.company_idno = tz.company_idno group by b.parent_code, a.ledger_idno having sum(voucher_amount) <> 0")

    '        'If InStr(Condt, "tG.Group_Idno") > 0 Then g_condt = Right(Condt, Len(Condt) - InStr(Condt, "tG.Group_Idno") + 1) Else g_condt = ""
    '        'com_id = Replace(Condt, g_condt, "")
    '        'If InStr(g_condt, "and ") > 0 Then g_condt = Mid(g_condt, 1, InStr(g_condt, " and "))
    '        'com_id = Replace(com_id, "and", "")

    '        Rt1 = New ADODB.Recordset
    '        'Rt1.Open "Select group_name, group_idno, parent_idno from group_head tg " & IIf(g_condt <> "", " where ", "") & g_condt & " order by order_position", Cn1, adOpenStatic, adLockReadOnly
    '        Rt1.Open("Select group_name, group_idno, parent_idno from group_head tg " & IIf(P <> "", " where ", "") & P & " order by order_position", cn1, adOpenStatic, adLockReadOnly)
    '        With Rt1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    Grid1.AddItem(UCase(!Group_Name))
    '                    Rw = Grid1.Rows - 1 : tt1 = 0
    '                    Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellBackColor = RGB(238, 230, 230)
    '                    RT2 = New ADODB.Recordset
    '                    RT2.Open("Select a.int1, a.currency1 as amount, b.ledger_name from reporttemp a, ledger_head b where a.name1 = '" & Trim(!parent_idno) & "' and a.int1 = b.ledger_idno order by b.ledger_name", cn1, adOpenStatic, adLockReadOnly)
    '                    If Not (RT2.BOF And RT2.EOF) Then
    '                        RT2.MoveFirst()
    '                        Do While Not RT2.EOF
    '                            Grid1.AddItem(" - " & StrConv(RT2!ledger_name, vbProperCase) & Chr(9) & IIf(RT2!Amount < 0, "", vbTab) & PROC.Currency_Format(Abs(RT2!Amount)))
    '                            tt1 = tt1 + RT2!Amount
    '                            Grid1.RowData(Grid1.Rows - 1) = RT2!int1
    '                            RT2.MoveNext()
    '                        Loop
    '                        Grid1.TextMatrix(Rw, IIf(tt1 < 0, 1, 2)) = PROC.Currency_Format(Abs(tt1))
    '                        If tt1 < 0 Then tt3 = tt3 + Abs(tt1) Else tt4 = tt4 + Abs(tt1)
    '                        Grid1.Row = Rw : Grid1.Col = IIf(tt1 < 0, 1, 2) : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                    Else
    '                        Grid1.Rows = Grid1.Rows - 1
    '                    End If
    '                    RT2 = Nothing
    '                    Mdi1.StatusBar3.Panels(2).Text = StrConv(!Group_Name, vbProperCase)
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing

    '        Grid1.AddItem("")
    '        Grid1.AddItem("TOTAL" & Chr(9) & PROC.Currency_Format(tt3) & Chr(9) & PROC.Currency_Format(tt4))
    '        If tt3 <> tt4 Then
    '            If tt3 > tt4 Then tt3 = tt3 - tt4 : tt4 = 0 Else tt4 = tt4 - tt3 : tt3 = 0
    '            Grid1.AddItem("CLOSING BALANCE" & Chr(9) & PROC.Currency_Format(tt3) & Chr(9) & PROC.Currency_Format(tt4))
    '        End If
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellForeColor = RGB(0, 0, 0)
    '        Grid1.Col = 0 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '        Grid1.Col = 1 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '        Grid1.Col = 2 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '    End Sub

    '    Private Function Set_Details(ByVal S As String) As String
    '        Dim i As Integer
    '        Dim Rs As ADODB.Recordset
    '        Dim com_id As String
    '        If InStr(LCase(Condt), LCase(S) & "_idno in") > 0 Then
    '            i = InStr(LCase(Condt), LCase(S) & "_idno in")
    '            com_id = "where " & Mid(Condt, i, InStr(i, Condt, ")") - i + 1)
    '        End If
    '        If LCase(S) = "company" And Trim(CompType_Condt) <> "" Then com_id = com_id & IIf(com_id <> "", " and ", " where ") & Trim(CompType_Condt)
    '        If com_id <> "" Or LCase(S) = "company" Then
    '            Rs = New ADODB.Recordset
    '            Rs.Open("Select * from " & S & "_head " & com_id & " order by " & S & "_name", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                If LCase(S) = "month" Then
    '                    Condt = Replace(Condt, " and tJ." & Mid(Condt, i, InStr(i, Condt, ")") - i + 1), "")
    '                    If InStr(Condt, "month_idno in ") > 0 Then Condt = Replace(Condt, " tJ." & Mid(Condt, i, InStr(i, Condt, ")") - i + 1), "")
    '                    month_idno = Rs(S & "_Idno")
    '                End If
    '                If LCase(S) = "company" Then com_id = "( " Else com_id = " "
    '                Do While Not Rs.EOF
    '                    com_id = com_id & Rs(S & "_Name")
    '                    Rs.MoveNext()
    '                    com_id = com_id & IIf(Not Rs.EOF, ", ", IIf(LCase(S) = "company", " )", ""))
    '                Loop
    '                Set_Details = com_id
    '            End If
    '        End If
    '    End Function

    '    Private Sub Accounts_FinalTB()
    '        Dim RT2 As Recordset, Rt1 As Recordset
    '        Dim tt1 As Currency, tt3 As Currency, tt4 As Currency
    '        Dim Rw As Integer
    '        Dim pr_ls As Currency

    '        Grid1.Cols = 3
    '        RptDet_Date1 = Cmp_ToDt
    '        Grid1.FormatString = "<PARTY NAME                                                   |>DEBIT                         |>CREDIT                    "
    '        Grid1.SelectionMode = 1
    '        FrmNm.Label1(0).Caption = Rpt_Main & " - UPTO DATE : " & Format(RptDet_Date1, "dd/mm/yyyy")

    '        cn1.Execute("delete from reporttemp")
    '        cn1.Execute("insert into reporttemp ( int1, currency1 ) Select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head c where a.company_idno = " & Str(Comp_IdNo) & " and a.voucher_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and a.ledger_idno = b.ledger_idno and b.parent_code = c.parent_idno group by a.ledger_idno having sum(voucher_amount) <> 0")

    '        Rt1 = New ADODB.Recordset
    '        With Rt1
    '            .Open("Select group_name, group_idno, parent_idno from group_head where parent_idno not like '%~18~' order by order_position", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    Grid1.AddItem(UCase(!Group_Name))
    '                    Rw = Grid1.Rows - 1 : tt1 = 0
    '                    Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellBackColor = RGB(238, 230, 230)
    '                    RT2 = New ADODB.Recordset
    '                    RT2.Open("Select a.int1, a.currency1 as amount, b.ledger_name from reporttemp a, ledger_head b where b.parent_code = '" & Trim(Rt1!parent_idno) & "' and a.int1 = b.ledger_idno order by b.ledger_name", cn1)
    '                    If Not (RT2.BOF And RT2.EOF) Then
    '                        RT2.MoveFirst()
    '                        Do While Not RT2.EOF
    '                            Grid1.AddItem(" - " & StrConv(RT2!ledger_name, vbProperCase) & Chr(9) & IIf(RT2!Amount < 0, "", vbTab) & PROC.Currency_Format(Abs(RT2!Amount)))
    '                            tt1 = tt1 + RT2!Amount
    '                            Grid1.RowData(Grid1.Rows - 1) = RT2!int1
    '                            RT2.MoveNext()
    '                        Loop
    '                        Grid1.TextMatrix(Rw, IIf(tt1 < 0, 1, 2)) = PROC.Currency_Format(Abs(tt1))
    '                        If tt1 < 0 Then tt3 = tt3 + Abs(tt1) Else tt4 = tt4 + Abs(tt1)
    '                        Grid1.Row = Rw : Grid1.Col = IIf(tt1 < 0, 1, 2) : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                    Else
    '                        Grid1.Rows = Grid1.Rows - 1
    '                    End If
    '                    RT2 = Nothing
    '                    Mdi1.StatusBar3.Panels(2).Text = StrConv(!Group_Name, vbProperCase)
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing

    '        Grid1.AddItem("")
    '        Grid1.AddItem("TOTAL" & Chr(9) & PROC.Currency_Format(tt3) & Chr(9) & PROC.Currency_Format(tt4))
    '        If tt3 <> tt4 Then
    '            If tt3 > tt4 Then tt3 = tt3 - tt4 : tt4 = 0 Else tt4 = tt4 - tt3 : tt3 = 0
    '            Grid1.AddItem("CLOSING BALANCE" & Chr(9) & PROC.Currency_Format(tt3) & Chr(9) & PROC.Currency_Format(tt4))
    '        End If
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellForeColor = RGB(0, 0, 0)

    '    End Sub

    '    Private Sub Accounts_AllLedger()
    '        Dim Rs1 As ADODB.Recordset, Rs2 As ADODB.Recordset
    '        Dim Tot1 As Currency, Tot2 As Currency
    '        Dim Pp1 As Integer, Gg1 As Integer, Id1 As Integer
    '        Dim Dt1 As Date
    '        Dt1 = Time

    '        RptDet_Date1 = CmpDet.FromDate
    '        RptDet_Date2 = CmpDet.ToDate

    '        Grid1.Cols = 7
    '        Grid1.FormatString = "<DATE           |<VOU.NO |<PARTY NAME                                  |<TYPE  |>DR.AMOUNT          |>CR.AMOUNT         |<NARRATION                                     "
    '        FrmNm.Label1(0).Caption = "ALL LEDGER - NAME : " & RptDet.Name1 & " - RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")

    '        Rs2 = New ADODB.Recordset
    '        Rs1 = New ADODB.Recordset
    '        With Rs1

    '            .Open("select b.ledger_name, b.ledger_idno, b.Parent_Code from reporttempsub a, ledger_head b where a.int1 = b.ledger_idno order by b.ledger_name", cn1, adOpenDynamic, adLockOptimistic)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    Tot1 = 0 : Tot2 = 0
    '                    If Not (!Parent_Code Like "*~18~") Then
    '                        Rs2.Open("select sum(voucher_amount) from voucher_details where company_idno = " & Str(RptDet.Idno1) & " and ledger_idno = " & Str(!Ledger_IdNo) & " and voucher_date < '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "'", cn1, adOpenForwardOnly, adLockReadOnly)
    '                        If Rs2(0).Value <> "" Then If Val(Rs2(0).Value) > 0 Then Tot1 = Val(Rs2(0).Value) Else Tot2 = Abs(Val(Rs2(0).Value))
    '                        Rs2.Close()
    '                    End If
    '                    Grid1.AddItem("")
    '                    Grid1.AddItem("" & Chr(9) & "" & Chr(9) & !ledger_name)
    '                    Grid1.RowData(Grid1.Rows - 1) = -100
    '                    Mdi1.StatusBar3.Panels(2).Text = !ledger_name
    '                    Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 2 : Grid1.CellBackColor = RGB(220, 200, 200)
    '                    If Tot1 <> 0 Or Tot2 <> 0 Then Grid1.AddItem("" & Chr(9) & "" & Chr(9) & "OPENING BALANCE" & Chr(9) & "" & Chr(9) & IIf(Tot2 > 0, PROC.Currency_Format(Tot2), "") & Chr(9) & IIf(Tot1 > 0, PROC.Currency_Format(Tot1), ""))

    '                    With Rs2
    '                        .Open("select a.voucher_date, a.voucher_amount, b.voucher_no, b.voucher_type, c.ledger_name as creditor_name, d.ledger_name as debtor_name, a.narration from voucher_details a, voucher_head b, ledger_head c, ledger_head d where a.ledger_idno = " & Str(Rs1!Ledger_IdNo) & " and a.company_idno = " & Str(RptDet.Idno1) & " and a.voucher_date between '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and '" & Trim(Format(Cmp_ToDt, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and a.company_idno = b.company_idno and b.creditor_idno = c.ledger_idno and b.debtor_idno = d.ledger_idno order by a.voucher_date, b.for_orderby", cn1, adOpenForwardOnly, adLockReadOnly)
    '                        If Not (.BOF And .EOF) Then
    '                            .MoveFirst()
    '                            Do While Not .EOF
    '                                If !Voucher_Amount >= 0 Then
    '                                    Grid1.AddItem(Format(!Voucher_Date, "dd-mm-yy") & Chr(9) & !Voucher_No & Chr(9) & "By " & !Debtor_Name & Chr(9) & !Voucher_Type & Chr(9) & "" & Chr(9) & PROC.Currency_Format(!Voucher_Amount) & Chr(9) & Trim(!Narration))
    '                                    Tot1 = Tot1 + !Voucher_Amount
    '                                Else
    '                                    Grid1.AddItem(Format(!Voucher_Date, "dd-mm-yy") & Chr(9) & !Voucher_No & Chr(9) & "To " & !creditor_name & Chr(9) & !Voucher_Type & Chr(9) & PROC.Currency_Format(Abs(!Voucher_Amount)) & Chr(9) & "" & Chr(9) & Trim(!Narration))
    '                                    Tot2 = Tot2 + Abs(!Voucher_Amount)
    '                                End If
    '                                .MoveNext()
    '                            Loop
    '                        End If
    '                        .Close()
    '                    End With
    '                    Grid1.AddItem("")
    '                    Grid1.AddItem("" & Chr(9) & "" & Chr(9) & "TOTAL" & Chr(9) & "" & Chr(9) & PROC.Currency_Format(Tot2) & Chr(9) & PROC.Currency_Format(Tot1))
    '                    Grid1.AddItem("" & Chr(9) & "" & Chr(9) & "CLOSING BALANCE")
    '                    Grid1.RowData(Grid1.Rows - 1) = -200
    '                    Tot1 = Tot1 - Tot2
    '                    If Tot1 <> 0 Then If Tot1 < 0 Then Grid1.TextMatrix(Grid1.Rows - 1, 4) = PROC.Currency_Format(Abs(Tot1)) Else Grid1.TextMatrix(Grid1.Rows - 1, 5) = PROC.Currency_Format(Abs(Tot1))
    '                    If Tot1 = 0 And Tot2 = 0 Then Grid1.Rows = Grid1.Rows - 5
    '                    Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                    Grid1.Row = Grid1.Rows - 1
    '                    Grid1.Col = 4 : Grid1.CellForeColor = RGB(255, 0, 100)
    '                    Grid1.Col = 5 : Grid1.CellForeColor = RGB(255, 0, 100)
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rs1 = Nothing
    '        Rs2 = Nothing
    '    End Sub

    '    Private Sub Accounts_NegativeCash()
    '        Dim Rs1 As ADODB.Recordset
    '        Dim Opc As Currency, Old_Opc As Currency

    '        Grid1.Cols = 2
    '        Grid1.FormatString = "<DATE                |>AMOUNT                  "
    '        Grid1.ColData(0) = 15 : Grid1.ColData(1) = 18
    '        FrmNm.Label1(0).Caption = "NEGATIVE CASH BALANCE"

    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select a.voucher_date, (Select sum(z.voucher_amount) from voucher_details z where z.company_idno = " & Str(Comp_IdNo) & " and z.ledger_idno = 1 and z.voucher_date <= a.voucher_date ) as amount from voucher_details a where a.company_idno = " & Str(Comp_IdNo) & " and a.ledger_idno = 1 and a.voucher_date between '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and '" & Trim(Format(Cmp_ToDt, "mm/dd/yyyy")) & "' group by a.voucher_date order by a.voucher_date", cn1, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            Rs1.MoveFirst()
    '            Do While Not Rs1.EOF
    '                If Rs1!Amount > 0 Then
    '                    Grid1.AddItem(Format(Rs1!Voucher_Date, "dd-mm-yyyy") & Chr(9) & Format(Rs1!Amount, "########0.00"))
    '                End If
    '                Rs1.MoveNext()
    '            Loop
    '        End If
    '        Rs1.Close()
    '        Rs1 = Nothing
    '    End Sub

    '    Private Sub Bills_Agent_Details_Single()
    '        Dim Rs As ADODB.Recordset
    '        Dim tt1 As Currency, tt2 As Currency
    '        Dim rf As String, ent_idn As String, ld_nm As String
    '        Dim amt As Currency, Nr As Integer

    '        FrmNm.Label1(0).Caption = Rpt_Main & " - NAME : " & RptDet_Name1 & " - RANGE : " & RptDet_Date1 & " To " & RptDet_Date2
    '        Grid1.Cols = 13 : tt1 = 0 : tt2 = 0
    '        Grid1.FormatString = "<PARTY NAME                   |<COMP. |<BL.DATE      |<BL.NO     |>AMOUNT                |<         |<ENT.NO   |<VOU.DATE     |<BANK NAME                 |<NARRATION                 |>AMOUNT           |>BALANCE         |<        "
    '        Grid1.ColData(0) = 25 : Grid1.ColData(1) = 10 : Grid1.ColData(2) = 7 : Grid1.ColData(3) = 7 : Grid1.ColData(4) = 12 : Grid1.ColData(5) = 5 : Grid1.ColData(6) = 6 : Grid1.ColData(7) = 10 : Grid1.ColData(8) = 25 : Grid1.ColData(9) = 15 : Grid1.ColData(10) = 12 : Grid1.ColData(11) = 12 : Grid1.ColData(12) = 5

    '        cn1.Execute("truncate table reporttemp")

    '        cn1.Execute("insert into reporttemp ( name8, int1,          currency1, name1,             date1,               currency2,                  currency3,                                                                                                                                                           name2,            name3,      name7,                                                      date2,       name4,         name5,          name6 ) " _
    '                                    & "select tz.company_shortname, a.ledger_idno, a.amount,  b.voucher_bill_no, b.voucher_bill_date, b.bill_amount, ( select sum(z.amount) from voucher_bill_details z where z.voucher_bill_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and z.voucher_bill_no = b.voucher_bill_no ), b.party_bill_no, b.crdr_type, ('V'+d.voucher_type + '-' + cast(d.voucher_no as varchar(20)) ), c.voucher_date, c.narration, e.ledger_name, d.entry_identification " _
    '                                    & "from voucher_bill_details a, voucher_bill_head b, voucher_details c, voucher_head d, ledger_head e, company_head tz where tz.company_idno = a.company_idno and " & Replace(LCase(Condt), "ta.ledger_idno", "b.agent_idno") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and c.ledger_idno = b.ledger_idno and ( a.entry_identification = 'VOUCH-'+d.voucher_ref_no or a.entry_identification = d.entry_identification ) and a.company_idno = d.company_idno and c.voucher_ref_no = d.voucher_ref_no and c.company_idno = d.company_idno and (case when a.ledger_idno=d.creditor_idno then d.debtor_idno else d.creditor_idno end)=e.ledger_idno order by b.voucher_bill_date, b.voucher_bill_no, a.voucher_bill_date", Nr)

    '        Rs = New ADODB.Recordset
    '        With Rs

    '            .Open("select name8, b.ledger_name, currency1 as amount, name1 as voucher_bill_no, date1 as voucher_bill_date, currency2 as bill_amount, currency3 as prv_amount, name2 as party_bill_no, name3 as crdr_type, name7 as voucher_no, date2 as voucher_date, name4 as narration, name5 as bank_name, name6 as entry_identification" _
    '                & " from reporttemp a, ledger_head b where a.int1 = b.ledger_idno order by b.ledger_name, date1, name1, date2", cn1)

    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If Left(Trim(!entry_identification), 6) <> "VOUCH-" And Len(!entry_identification) > 12 Then
    '                        ent_idn = Left(!entry_identification, Len(!entry_identification) - 6)
    '                    Else
    '                        ent_idn = UCase(!Voucher_No)
    '                    End If

    '                    If rf <> !voucher_bill_no Then
    '                        amt = !bill_amount
    '                        If !prv_amount <> "" Then amt = amt - !prv_amount
    '                        Grid1.AddItem(IIf(ld_nm <> !ledger_name, !ledger_name, "") & vbTab & !Name8 & Chr(9) & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & PROC.Currency_Format(amt) & Chr(9) & !crdr_type & Chr(9) & ent_idn & Chr(9) & Format(!Voucher_Date, "dd/mm/yy") & Chr(9) & !Bank_Name & Chr(9) & !Narration & Chr(9) & PROC.Currency_Format(!Amount) & Chr(9) & PROC.Currency_Format(amt - !Amount) & Chr(9) & !crdr_type)
    '                    Else
    '                        Grid1.AddItem(Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & ent_idn & Chr(9) & Format(!Voucher_Date, "dd/mm/yy") & Chr(9) & !Bank_Name & Chr(9) & !Narration & Chr(9) & PROC.Currency_Format(!Amount) & Chr(9) & PROC.Currency_Format(amt - !Amount) & Chr(9) & !crdr_type)
    '                    End If
    '                    tt1 = tt1 + !Amount
    '                    rf = !voucher_bill_no
    '                    ld_nm = !ledger_name
    '                    amt = amt - !Amount

    '                    .MoveNext()
    '                If .EOF = False Then If ld_nm <> !ledger_name Then GoSub Customer_Total

    '                Loop
    '                Grid1.RowData(Grid1.Rows - 1) = 2
    '            End If
    '            .Close()
    '        End With
    '      GoSub Customer_Total
    '        Grid1.AddItem("GRAND TOTAL" & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & PROC.Currency_Format(tt2))
    '        Grid1.RowData(Grid1.Rows - 1) = "3"
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Rs = Nothing
    '        Exit Sub

    'Customer_Total:
    '        Grid1.AddItem(Chr(9) & "TOTAL" & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & PROC.Currency_Format(tt1))
    '        Grid1.RowData(Grid1.Rows - 1) = "3"
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        tt2 = tt2 + tt1
    '        tt1 = 0
    '        Grid1.AddItem("")
    '        Return

    '    End Sub

    '    Private Sub Bills_Customer_Details_Single()

    '        Dim Rs As ADODB.Recordset
    '        Dim tt1 As Currency, tt2 As Currency
    '        Dim rf As String, ent_idn As String
    '        Dim amt As Currency, Tot As Currency, Tot1 As Currency, Tot2 As Currency

    '        RptHeading1 = "BILL PENDING " & Set_Details("Company")
    '        RptHeading2 = Set_Details("Ledger")
    '        RptHeading3 = "RANGE : " & RptDet_Date1 & " To " & RptDet_Date2
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3
    '        Grid1.Cols = 13
    '        Grid1.FormatString = "<BL.DATE |<COMP. |<BL.NO        |>AMOUNT         |<         |<ENT.NO         |<VOU.DT   |<BANK NAME                 |<NARRATION                 |>AMOUNT         |>BALANCE        |<      |>BILL BALANCE|<      "
    '        Grid1.ColData(0) = 10 : Grid1.ColData(1) = 7 : Grid1.ColData(2) = 12 : Grid1.ColData(3) = 5 : Grid1.ColData(4) = 6 : Grid1.ColData(5) = 10 : Grid1.ColData(6) = 25 : Grid1.ColData(7) = 15 : Grid1.ColData(8) = 10 : Grid1.ColData(9) = 10 : Grid1.ColData(10) = 5 : Grid1.ColData(11) = 10 : Grid1.ColData(12) = 5
    '        tt1 = 0 : tt2 = 0
    '        Rs = New ADODB.Recordset
    '        With Rs

    '            Con.Execute("truncate table reporttemp")

    '            Con.Execute("insert into reporttemp (   int1,       currency1,      name1,              date1,              currency2,              currency3,                                                                                                                                                                                                                                                name2,         name3,        name4,        date2,          name5,          name6,                      name7,              name8 ) " _
    '                                    & " Select   tz.company_idno, a.amount, b.voucher_bill_no, b.voucher_bill_date, b.bill_amount, ( Select sum(z.amount) from voucher_bill_details z where z.voucher_bill_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and z.voucher_bill_no = b.voucher_bill_no and z.ledger_idno = b.ledger_idno and z.company_idno = b.company_idno ) as prv_amount, b.party_bill_no, b.crdr_type, d.voucher_no, c.voucher_date, c.narration, e.ledger_name as bank_name, d.voucher_type, d.entry_identification " _
    '                                    & "from voucher_bill_details a, voucher_bill_head b, voucher_details c, voucher_head d, ledger_head e, company_head tz where tz.company_idno = a.company_idno and " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and c.ledger_idno = b.ledger_idno and ( a.entry_identification = 'VOUCH-'+d.voucher_ref_no or a.entry_identification = d.entry_identification ) and a.company_idno = d.company_idno and c.voucher_ref_no = d.voucher_ref_no and c.company_idno = d.company_idno and (case when a.ledger_idno=d.creditor_idno then d.debtor_idno else d.creditor_idno end)=e.ledger_idno order by b.voucher_bill_date, b.voucher_bill_no, a.voucher_bill_date")

    '            Con.Execute("insert into reporttemp (   int1,         currency1,     name1,        date1,        currency2,        name2,          name3    ) " _
    '                                    & "Select   tz.company_idno, 0, b.voucher_bill_no, b.voucher_bill_date, b.bill_amount, b.party_bill_no, b.crdr_type from voucher_bill_head b, company_head tz " _
    '                                    & "where b.debit_amount <> b.credit_amount and (ltrim(cast(b.company_idno as varchar(4)))+'-'+b.voucher_bill_no) not in ( select (ltrim(cast(rtmp.int1 as varchar(4)))+'-'+rtmp.name1) from reporttemp rtmp) and tz.company_idno = b.company_idno and " & Replace(Condt, "tP.", "b.") & IIf(Condt <> "", " and ", "") & " b.voucher_bill_date <= '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "'")


    '            .Open("Select tz.company_shortname, currency1 as amount, name1 as voucher_bill_no, date1 as voucher_bill_date, currency2 as bill_amount, currency3 as prv_amount, name2 as party_bill_no, name3 as crdr_type, name4 as voucher_no, date2 as voucher_date, name5 as narration, name6 as bank_name, name7 as voucher_type, name8 as entry_identification" _
    '                & " from reporttemp a, company_head tz where tz.company_idno = a.int1 order by a.date1, a.name1, a.date2", cn1)


    '            '        .Open "Select tz.company_shortname, a.amount, b.voucher_bill_no, b.voucher_bill_date, b.bill_amount, ( Select sum(z.amount) from voucher_bill_details z where z.voucher_bill_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and z.voucher_bill_no = b.voucher_bill_no and z.ledger_idno = b.ledger_idno and z.company_idno = b.company_idno ) as prv_amount, b.party_bill_no, " _
    '            '            & "b.crdr_type, d.voucher_no, c.voucher_date, c.narration, e.ledger_name as bank_name, d.voucher_type, d.entry_identification from voucher_bill_details a, voucher_bill_head b, voucher_details c, voucher_head d, ledger_head e, company_head tz where tz.company_idno = a.company_idno and " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and c.ledger_idno = b.ledger_idno and ( a.entry_identification = 'VOUCH-'+d.voucher_ref_no or a.entry_identification = d.entry_identification ) and a.company_idno = d.company_idno and c.voucher_ref_no = d.voucher_ref_no and c.company_idno = d.company_idno and (case when a.ledger_idno=d.creditor_idno then d.debtor_idno else d.creditor_idno end)=e.ledger_idno order by b.voucher_bill_date, b.voucher_bill_no, a.voucher_bill_date", Cn1
    '            '
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    ent_idn = "V" & UCase(!Voucher_Type) & "-" & !Voucher_No
    '                    If Left(Trim(!entry_identification), 6) <> "VOUCH-" And Len(!entry_identification) > 12 Then
    '                        ent_idn = Left(!entry_identification, Len(!entry_identification) - 6)
    '                    End If

    '                    If rf <> !voucher_bill_no Then
    '                        If amt <> 0 Then
    '                            Grid1.TextMatrix(Grid1.Rows - 1, 12) = PROC.Currency_Format(amt)
    '                            Grid1.TextMatrix(Grid1.Rows - 1, 13) = Grid1.TextMatrix(Grid1.Rows - 1, 11)
    '                            Tot2 = Tot2 + (amt * IIf(Grid1.TextMatrix(Grid1.Rows - 1, 11) = "Cr", 1, -1))
    '                        End If
    '                        amt = !bill_amount
    '                        If !prv_amount <> "" Then amt = amt - !prv_amount
    '                        Grid1.AddItem(Format(!voucher_bill_date, "dd-mm-yy") & vbTab & !Company_ShortName & vbTab & !Party_Bill_No & vbTab & PROC.Currency_Format(amt) & Chr(9) & !crdr_type & Chr(9) & ent_idn & Chr(9) & Format(!Voucher_Date, "dd/mm/yy") & Chr(9) & !Bank_Name & Chr(9) & !Narration & Chr(9) & PROC.Currency_Format(!Amount) & Chr(9) & PROC.Currency_Format(amt - !Amount) & Chr(9) & !crdr_type)
    '                        Tot1 = Tot1 + !bill_amount
    '                    Else
    '                        Grid1.AddItem(Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & ent_idn & Chr(9) & Format(!Voucher_Date, "dd/mm/yy") & Chr(9) & !Bank_Name & Chr(9) & !Narration & Chr(9) & PROC.Currency_Format(!Amount) & Chr(9) & PROC.Currency_Format(amt - !Amount) & Chr(9) & !crdr_type)
    '                    End If
    '                    rf = !voucher_bill_no
    '                    amt = amt - !Amount
    '                    Tot = Tot + !Amount
    '                    .MoveNext()
    '                Loop
    '                Grid1.RowData(Grid1.Rows - 1) = 2
    '                Grid1.AddItem("")
    '                Grid1.AddItem(Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & "TOTAL" & Chr(9) & Chr(9) & Chr(9) & PROC.Currency_Format(Tot) & Chr(9) & Chr(9) & Chr(9) & PROC.Currency_Format(Abs(Tot2)) & Chr(9) & IIf(Tot2 > 0, "Cr", "Dr"))
    '                Grid1.RowData(Grid1.Rows - 1) = "3"
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            End If
    '            .Close()
    '        End With
    '        Rs = Nothing
    '    End Sub

    '    Private Sub Bills_Customer_Pending_Single()
    '        Dim Rs As ADODB.Recordset
    '        Dim tt_cr As Currency, tt_db As Currency
    '        Dim cr_amt As Currency, db_amt As Currency
    '        Dim tt1 As Currency, tt2 As Currency

    '        RptHeading1 = "BILL PENDING " & Set_Details("Company")
    '        RptHeading2 = Set_Details("Ledger")
    '        RptHeading3 = "AS ON : " & RptDet_Date1
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3
    '        Grid1.Cols = 7 : tt1 = 0 : tt2 = 0
    '        Grid1.FormatString = "<BILL DATE   |<COMP. |<BILL NO      |>CR.AMOUNT           |>DR.AMOUNT          |>BALANCE              |<      |>DAYS    "
    '        Grid1.ColData(0) = 10 : Grid1.ColData(1) = 7 : Grid1.ColData(2) = 13 : Grid1.ColData(3) = 13 : Grid1.ColData(4) = 13 : Grid1.ColData(5) = 5 : Grid1.ColData(6) = 6

    '        cn1.Execute("truncate table reporttemp_simple")
    '        cn1.Execute("insert into reporttemp_simple ( smallint_1, smallint_2, text_1, amount_1 ) Select tp.company_idno, ledger_idno, voucher_bill_no, sum(amount) from voucher_bill_details tp,Company_Head tZ where " & Replace(Condt, "tZ.", "tP.") & IIf(Condt <> "", " and ", "") & " voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and tz.Company_Idno = tp.Company_Idno group by tp.company_idno, ledger_idno, voucher_bill_no")

    '        Rs = New ADODB.Recordset
    '        With Rs
    '            .Open("Select tz.company_shortname, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount from voucher_bill_head a LEFT OUTER JOIN reporttemp_simple b ON a.voucher_bill_no = b.text_1 and a.company_idno = b.smallint_1 INNER JOIN company_head tz ON a.company_idno = tz.company_idno where " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' order by a.voucher_bill_date, a.voucher_bill_no", cn1, adOpenStatic, adLockReadOnly)
    '            '.Open "Select tz.company_shortname, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount from voucher_bill_head a, reporttemp_simple b, company_head tz where " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and a.voucher_bill_no *= b.text_1 and a.company_idno *= b.smallint_1 and a.company_idno = tz.company_idno order by a.voucher_bill_date, a.voucher_bill_no", cn1, adOpenStatic, adLockReadOnly
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If !crdr_type = "Cr" Then
    '                        cr_amt = !bill_amount
    '                        If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                    Else
    '                        db_amt = !bill_amount
    '                        If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                    End If
    '                    If cr_amt <> db_amt Then
    '                        Grid1.AddItem Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Company_ShortName & Chr(9) & !Party_Bill_No & Chr(9) & PROC.Currency_Format(cr_amt) & Chr(9) & PROC.Currency_Format(db_amt) & Chr(9) & PROC.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Db", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                        tt_cr = tt_cr + cr_amt
    '                        tt_db = tt_db + db_amt
    '                    End If
    '                    .MoveNext()
    '                Loop
    '                Grid1.AddItem("")
    '                Grid1.AddItem("" & Chr(9) & "" & Chr(9) & "Total" & Chr(9) & PROC.Currency_Format(tt_cr) & Chr(9) & PROC.Currency_Format(tt_db) & Chr(9) & PROC.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr"))
    '                Grid1.RowData(Grid1.Rows - 1) = "3"
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            End If
    '            .Close()
    '        End With
    '        Rs = Nothing
    '    End Sub

    '    Private Sub Bills_Customer_Pending_All()
    '        Dim Rs As ADODB.Recordset
    '        Dim tt_cr As Currency, tt_db As Currency
    '        Dim cr_amt As Currency, db_amt As Currency
    '        Dim P_Name As String, type_condt As String
    '        Dim tt1 As Currency, tt2 As Currency
    '        Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency

    '        If Rpt_Main = "Customer Bill Pending Purchased Bills" Then
    '            P_Name = "PURCHASED "
    '            type_condt = " and a.crdr_type = 'Cr' and tp.ledger_type = '' "
    '        ElseIf Rpt_Main = "Customer Bill Pending Invoiced Bills" Then
    '            P_Name = "INVOICED "
    '            type_condt = " and a.crdr_type = 'Dr' and tp.ledger_type = '' "
    '        ElseIf Rpt_Main = "Customer Bill Pending Sizing Bills" Then
    '            P_Name = "BILLED "
    '            type_condt = " and a.crdr_type = 'Cr' and tp.ledger_type = 'SIZING' "
    '        ElseIf Rpt_Main = "Customer Bill Pending Weaver Bills" Then
    '            P_Name = "BILLED "
    '            type_condt = " and a.crdr_type = 'Cr' and tp.ledger_type = 'WEAVER' "
    '        End If
    '        RptHeading1 = P_Name & "BILL PENDING " & Set_Details("Company")
    '        RptHeading2 = Set_Details("Ledger")
    '        RptHeading3 = "AS ON : " & RptDet_Date1
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3

    '        Grid1.Cols = 10 : tt1 = 0 : tt2 = 0
    '        Grid1.FormatString = "<PARTY NAME                     |<COMP. |<BL.DATE  |<BL.NO |>CR.AMOUNT       |>DR.AMOUNT        |>BALANCE          |<     |>DAYS(I) |>DAYS(S)"
    '        Grid1.ColData(0) = 33 : Grid1.ColData(1) = 8 : Grid1.ColData(2) = 10 : Grid1.ColData(3) = 10 : Grid1.ColData(4) = 15 : Grid1.ColData(5) = 15 : Grid1.ColData(6) = 15 : Grid1.ColData(7) = 5 : Grid1.ColData(8) = 6 : Grid1.ColData(9) = 6

    '        cn1.Execute("truncate table reporttemp_simple")
    '        cn1.Execute("insert into reporttemp_simple ( smallint_1, smallint_2, text_1, amount_1 ) Select tz.company_idno, ledger_idno, voucher_bill_no, sum(amount) from voucher_bill_details tp, company_head tz  where " & Replace(Condt, "tZ.", "tP.") & IIf(Condt <> "", " and ", "") & " voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and tp.company_idno = tz.company_idno group by tz.company_idno, ledger_idno, voucher_bill_no")

    '        Rs = New ADODB.Recordset
    '        With Rs
    '            .Open("Select tz.company_shortname, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount, tp.ledger_name from voucher_bill_head a LEFT OUTER JOIN reporttemp_simple b ON a.voucher_bill_no = b.text_1 and a.company_idno = b.smallint_1 , ledger_head tp, company_head tz where " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' " & type_condt & " and a.company_idno = tz.company_idno and a.ledger_idno = tp.ledger_idno order by tp.ledger_name, a.voucher_bill_date, a.voucher_bill_no", cn1, adOpenStatic, adLockReadOnly)
    '            '.Open "Select tz.company_shortname, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount, tp.ledger_name from voucher_bill_head a, reporttemp_simple b, ledger_head tp, company_head tz where " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' " & type_condt & " and a.voucher_bill_no *= b.text_1 and a.company_idno *= b.smallint_1 and a.company_idno = tz.company_idno and a.ledger_idno = tp.ledger_idno order by tp.ledger_name, a.voucher_bill_date, a.voucher_bill_no", cn1, adOpenStatic, adLockReadOnly
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If !crdr_type = "Cr" Then
    '                        cr_amt = !bill_amount
    '                        If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                    Else
    '                        db_amt = !bill_amount
    '                        If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                    End If
    '                    If cr_amt <> db_amt Then
    '                        Grid1.AddItem IIf(P_Name <> !ledger_name, !ledger_name, "") & vbTab & !Company_ShortName & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & PROC.Currency_Format(cr_amt) & Chr(9) & PROC.Currency_Format(db_amt) & Chr(9) & PROC.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, RptDet_Date1) & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                        P_Name = !ledger_name
    '                        tt_cr = tt_cr + cr_amt
    '                        tt_db = tt_db + db_amt
    '                    End If
    '                    .MoveNext()
    '                    If .EOF Then
    '                        If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                    ElseIf !ledger_name <> P_Name And (tt_cr > 0 Or tt_db > 0) Then
    '                        GoSub Party_Total
    '                    End If
    '                Loop
    '                Grid1.AddItem("TOTAL" & vbTab & vbTab & vbTab & vbTab & PROC.Currency_Format(Al_Tt_Cr) & Chr(9) & PROC.Currency_Format(Al_Tt_Db) & Chr(9) & PROC.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr"))
    '                Grid1.RowData(Grid1.Rows - 1) = "3"
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            End If
    '            .Close()
    '        End With
    '        Rs = Nothing
    '        Exit Sub

    'Party_Total:
    '        Grid1.AddItem(vbTab & "TOTAL" & vbTab & vbTab & vbTab & PROC.Currency_Format(tt_cr) & Chr(9) & PROC.Currency_Format(tt_db) & Chr(9) & PROC.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr"))
    '        Grid1.RowData(Grid1.Rows - 1) = "3"
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Grid1.AddItem("")
    '        Al_Tt_Cr = Al_Tt_Cr + tt_cr : Al_Tt_Db = Al_Tt_Db + tt_db
    '        tt_cr = 0 : tt_db = 0
    '        Return

    '    End Sub

    '    Private Sub Bills_Customer_Pending_AgingAnalysis()
    '        Dim Rs As ADODB.Recordset
    '        Dim b() As String
    '        Dim i As Integer
    '        Dim S As String, oldvl As String, T As String
    '        Dim tt As Currency, tt_c(20) As Currency

    '        RptHeading1 = "BILL PENDING LIST " & Set_Details("Company")
    '        RptHeading2 = "AS ON : " & Date
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3

    '        S = "<PARTY NAME                     "
    '        cn1.Execute("truncate table reporttemp")
    '        oldvl = "0"
    '        b = Split(RptDet_Tex_Val1, ",")
    '        Grid1.Cols = UBound(b) + 2
    '        Grid1.ColData(0) = 35
    '        For i = 0 To UBound(b)
    '            S = S & "|>" & oldvl & " TO " & Val(b(i)) & "  "
    '            cn1.Execute("insert into reporttemp ( int1, currency" & Trim(i + 1) & " ) Select ledger_idno, sum(debit_amount-credit_amount) from voucher_bill_head a, company_head tz where " & Trim(Condt) & IIf(Condt <> "", " and ", "") & " datediff(dd, voucher_bill_date, getdate()) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and debit_amount > credit_amount and a.company_idno = tz.company_idno group by ledger_idno")
    '            oldvl = Val(b(i)) + 1
    '            T = T & "sum(currency" & Trim(i + 1) & "),"
    '        Next i
    '        Grid1.FormatString = S & "|>ABV " & Val(b(i - 1)) & "    |>TOTAL         "
    '        cn1.Execute("insert into reporttemp ( int1, currency" & Trim(i + 1) & " ) Select ledger_idno, sum(debit_amount-credit_amount) from voucher_bill_head a, company_head tz where " & Trim(Condt) & IIf(Condt <> "", " and ", "") & " datediff(dd, voucher_bill_date, getdate()) > " & Str(Val(b(i - 1))) & " and debit_amount > credit_amount and a.company_idno = tz.company_idno group by ledger_idno ")
    '        T = T & "sum(currency" & Trim(i + 1) & ")"
    '        For i = 0 To UBound(b) + 2
    '            Grid1.ColData(i + 1) = 14
    '            Grid1.ColWidth(i + 1) = 1500
    '        Next i

    '        Rs = New ADODB.Recordset
    '        With Rs
    '            .Open("Select b.ledger_name, " & T & " from reporttemp a, ledger_head b where a.int1 = b.ledger_idno group by b.ledger_name order by b.ledger_name", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    tt = 0
    '                    Grid1.AddItem!ledger_name()
    '                    For i = 1 To .Fields.Count - 1
    '                        If Val(Rs(i)) > 0 Then
    '                            Grid1.TextMatrix(Grid1.Rows - 1, i) = PROC.Currency_Format(Rs(i))
    '                            tt = tt + Val(Rs(i))
    '                            tt_c(i) = tt_c(i) + Val(Rs(i))
    '                        End If
    '                    Next i
    '                    Grid1.TextMatrix(Grid1.Rows - 1, i) = PROC.Currency_Format(tt)
    '                    tt_c(i) = tt_c(i) + tt
    '                    .MoveNext()
    '                Loop
    '                Grid1.AddItem("TOTAL")
    '                For i = 0 To UBound(b) + 2
    '                    Grid1.TextMatrix(Grid1.Rows - 1, i + 1) = PROC.Currency_Format(tt_c(i + 1))
    '                Next i
    '                Grid1.RowData(Grid1.Rows - 1) = "3"
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            End If
    '            .Close()
    '        End With
    '        Rs = Nothing

    '    End Sub

    '    Private Sub Bills_Agent_Bill_Pending_Single()
    '        Dim Rs As ADODB.Recordset
    '        Dim tt_cr As Currency, tt_db As Currency, tt1 As Currency, tt2 As Currency
    '        Dim cr_amt As Currency, db_amt As Currency, Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '        Dim P_Name As String

    '        RptHeading1 = "BILL PENDING " & Set_Details("Company")
    '        RptHeading2 = Set_Details("Ledger")
    '        RptHeading3 = "AS ON : " & RptDet_Date1
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3

    '        Grid1.Cols = 10 : tt1 = 0 : tt2 = 0
    '        Grid1.FormatString = "<PARTY NAME                      |<COMP.|<BILL DATE  |<BILL NO     |>CR.AMOUNT          |>DR.AMOUNT        |>BALANCE             |<      |>DAYS(I)|>DAYS(S)"
    '        Grid1.ColData(0) = 30 : Grid1.ColData(1) = 15 : Grid1.ColData(2) = 13 : Grid1.ColData(3) = 10 : Grid1.ColData(4) = 11 : Grid1.ColData(5) = 12 : Grid1.ColData(6) = 12 : Grid1.ColData(7) = 12 : Grid1.ColData(8) = 5 : Grid1.ColData(9) = 5

    '        Condt = Replace(Replace(Condt, "tA.Ledger_Idno", "a.Agent_Idno"), "tZ.", "a.")
    '        cn1.Execute("truncate table reporttemp_simple")
    '        cn1.Execute("insert into reporttemp_simple ( smallint_1, text_1, amount_1 ) Select b.company_idno, b.voucher_bill_no, sum(b.amount) from voucher_bill_details b, voucher_bill_head a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " b.voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and tz.company_idno = b.company_idno and a.company_idno = b.company_idno group by b.company_idno, b.voucher_bill_no")

    '        Rs = New ADODB.Recordset
    '        With Rs
    '            .Open("Select c.ledger_name, tz.company_shortname, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount from voucher_bill_head a, reporttemp_simple b, ledger_head c, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and a.voucher_bill_no *= b.text_1 and a.company_idno *= b.smallint_1 and a.ledger_idno = c.ledger_idno and a.company_idno = tz.company_idno order by c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If !crdr_type = "Cr" Then
    '                        cr_amt = !bill_amount
    '                        If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                    Else
    '                        db_amt = !bill_amount
    '                        If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                    End If
    '                    If cr_amt <> db_amt Then
    '                        Grid1.AddItem IIf(P_Name <> !ledger_name, !ledger_name, "") & vbTab & !Company_ShortName & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & PROC.Currency_Format(cr_amt) & Chr(9) & PROC.Currency_Format(db_amt) & Chr(9) & PROC.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Db", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, RptDet_Date1) & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                        P_Name = !ledger_name
    '                        tt_cr = tt_cr + cr_amt
    '                        tt_db = tt_db + db_amt
    '                    End If
    '                    .MoveNext()
    '                    If .EOF Then
    '                        If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                    ElseIf P_Name <> !ledger_name And (tt_cr > 0 Or tt_db > 0) Then
    '                        GoSub Party_Total
    '                    End If
    '                Loop
    '                Grid1.AddItem("" & Chr(9) & "Total" & Chr(9) & Chr(9) & Chr(9) & PROC.Currency_Format(Al_Tt_Cr) & Chr(9) & PROC.Currency_Format(Al_Tt_Db) & Chr(9) & PROC.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr"))
    '                Grid1.RowData(Grid1.Rows - 1) = "1"
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            End If
    '            .Close()
    '        End With
    '        Rs = Nothing
    '        Exit Sub

    'Party_Total:
    '        Grid1.AddItem(Chr(9) & "Total" & Chr(9) & Chr(9) & Chr(9) & PROC.Currency_Format(tt_cr) & Chr(9) & PROC.Currency_Format(tt_db) & Chr(9) & PROC.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr"))
    '        Grid1.RowData(Grid1.Rows - 1) = "1"
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Grid1.AddItem("")
    '        Al_Tt_Cr = Al_Tt_Cr + tt_cr
    '        Al_Tt_Db = Al_Tt_Db + tt_db
    '        tt_cr = 0 : tt_db = 0
    '        Return

    '    End Sub

    '    Private Sub Bills_Agent_Pending_All()
    '        Dim Rs As ADODB.Recordset
    '        Dim tt_cr As Currency, tt_db As Currency
    '        Dim cr_amt As Currency, db_amt As Currency
    '        Dim P_Name As String, a_name As String, type_condt As String
    '        Dim tt1 As Currency, tt2 As Currency
    '        Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '        Dim Ag_TotCr As Currency, Ag_TotDb As Currency
    '        Dim Nr_Par As Integer

    '        If Rpt_Main = "Agent Bill Pending Purchased" Then
    '            P_Name = "PURCHASED "
    '            type_condt = " and a.crdr_type = 'Cr' "
    '        ElseIf Rpt_Main = "Agent Bill Pending Invoiced" Then
    '            P_Name = "INVOICED "
    '            type_condt = " and a.crdr_type = 'Dr' "
    '        End If
    '        RptHeading1 = P_Name & "BILL PENDING " & Set_Details("Company")
    '        RptHeading2 = "AS ON : " & RptDet_Date1
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3
    '        Grid1.Cols = 11 : tt1 = 0 : tt2 = 0
    '        Grid1.FormatString = "<AGENT NAME          |<PARTY NAME              |<COMP.|<BL.DATE   |<BL.NO |>CR.AMOUNT       |>DR.AMOUNT       |>BALANCE           |<      |>DAYS(I)|>DAYS(S)"
    '        Grid1.ColData(0) = 25 : Grid1.ColData(1) = 25 : Grid1.ColData(2) = 8 : Grid1.ColData(3) = 10 : Grid1.ColData(4) = 7 : Grid1.ColData(5) = 13 : Grid1.ColData(6) = 13 : Grid1.ColData(7) = 13 : Grid1.ColData(8) = 4 : Grid1.ColData(9) = 4 : Grid1.ColData(10) = 4

    '        Condt = Replace(Condt, "tZ.", "a.")
    '        cn1.Execute("truncate table reporttemp_simple")
    '        cn1.Execute("insert into reporttemp_simple ( smallint_1, text_1, amount_1 ) Select tz.company_idno, voucher_bill_no, sum(amount) from voucher_bill_details a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and a.company_idno = tz.company_idno group by tz.company_idno, voucher_bill_no")
    '        cn1.Execute("insert into reporttemp_simple ( smallint_1, text_1, amount_1 ) Select tz.company_idno, voucher_bill_no, 0 from voucher_bill_head a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " voucher_bill_date <= '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and a.company_idno = tz.company_idno")
    '        cn1.Execute("truncate table reporttempsub")
    '        cn1.Execute("insert into reporttempsub ( int1, name1, currency1 ) Select smallint_1, text_1, sum(amount_1) from reporttemp_simple group by smallint_1, text_1")

    '        Rs = New ADODB.Recordset
    '        With Rs
    '            .Open("Select tz.company_shortname, d.ledger_name as agent_name, c.ledger_name, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.currency1 as amount from voucher_bill_head a, reporttempsub b, ledger_head c, ledger_head d, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " a.company_idno = tz.company_idno " & type_condt & " and a.voucher_bill_no = b.name1 and a.company_idno = b.int1 and a.bill_amount <> b.currency1 and a.ledger_idno = c.ledger_idno and a.agent_idno = d.ledger_idno order by d.ledger_name, c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", cn1)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If !crdr_type = "Cr" Then
    '                        cr_amt = !bill_amount
    '                        If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                    Else
    '                        db_amt = !bill_amount
    '                        If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                    End If
    '                    Grid1.AddItem IIf(a_name <> !agent_name, !agent_name, "") & vbTab & IIf(P_Name <> !ledger_name Or a_name <> !agent_name, !ledger_name, "") & vbTab & !Company_ShortName & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & PROC.Currency_Format(cr_amt) & Chr(9) & PROC.Currency_Format(db_amt) & Chr(9) & PROC.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, RptDet_Date1) & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                    P_Name = !ledger_name
    '                    a_name = !agent_name
    '                    tt_cr = tt_cr + cr_amt
    '                    tt_db = tt_db + db_amt
    '                    Ag_TotCr = Ag_TotCr + cr_amt
    '                    Ag_TotDb = Ag_TotDb + db_amt
    '                    .MoveNext()
    '                    If .EOF Then
    '                        If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                        GoSub Agent_Total
    '                    ElseIf (!ledger_name <> P_Name Or a_name <> !agent_name) And (tt_cr > 0 Or tt_db > 0) Then
    '                        GoSub Party_Total
    '                        Nr_Par = Nr_Par + 1
    '                        If a_name <> !agent_name Then GoSub Agent_Total
    '                        If a_name <> !agent_name Then Nr_Par = 0
    '                    End If
    '                Loop
    '                Grid1.AddItem("GRAND TOTAL" & vbTab & vbTab & vbTab & vbTab & vbTab & PROC.Currency_Format(Al_Tt_Cr) & Chr(9) & PROC.Currency_Format(Al_Tt_Db) & Chr(9) & PROC.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr"))
    '                Grid1.RowData(Grid1.Rows - 1) = 2
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            End If
    '            .Close()
    '        End With
    '        Rs = Nothing

    '        Exit Sub

    'Party_Total:
    '        Grid1.AddItem(vbTab & "TOTAL (PARTY)" & vbTab & vbTab & vbTab & vbTab & PROC.Currency_Format(tt_cr) & Chr(9) & PROC.Currency_Format(tt_db) & Chr(9) & PROC.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr"))
    '        Grid1.RowData(Grid1.Rows - 1) = "3"
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Al_Tt_Cr = Al_Tt_Cr + tt_cr : Al_Tt_Db = Al_Tt_Db + tt_db
    '        tt_cr = 0 : tt_db = 0
    '        Return

    'Agent_Total:
    '        Grid1.AddItem("TOTAL (AGENT)" & vbTab & vbTab & vbTab & vbTab & vbTab & PROC.Currency_Format(Ag_TotCr) & Chr(9) & PROC.Currency_Format(Ag_TotDb) & Chr(9) & PROC.Currency_Format(Abs(Ag_TotCr - Ag_TotDb)) & Chr(9) & IIf(Ag_TotCr >= Ag_TotDb, "Cr", "Dr"))
    '        Grid1.RowData(Grid1.Rows - 1) = "2"
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Ag_TotCr = 0 : Ag_TotDb = 0
    '        Nr_Par = 0
    '        Return
    '    End Sub

    '    Private Sub Bills_Agent_Pending_AgingAnalysis()
    '        Dim Rs As ADODB.Recordset
    '        Dim b() As String
    '        Dim i As Integer, sl As Integer
    '        Dim S As String, oldvl As String, T As String, AgNm As String
    '        Dim tt As Currency, tt_c(20) As Currency, tt_a(20) As Currency

    '        RptHeading1 = "BILL PENDING LIST " & Set_Details("Company")
    '        RptHeading2 = "AS ON : " & Date
    '        FrmNm.Label1(0).Caption = RptHeading1 & " " & RptHeading2 & " " & RptHeading3

    '        S = "<AGENT NAME        |<PARTY NAME                     "
    '        cn1.Execute("truncate table reporttemp")
    '        oldvl = "0"
    '        b = Split(RptDet_Tex_Val1, ",")
    '        Grid1.Cols = UBound(b) + 3
    '        Grid1.ColData(0) = 30 : Grid1.ColData(1) = 30
    '        For i = 0 To UBound(b)
    '            S = S & "|>" & oldvl & " TO " & Val(b(i)) & "  "
    '            cn1.Execute("insert into reporttemp ( int1, int2, currency" & Trim(i + 1) & " ) Select agent_idno, ledger_idno, sum(debit_amount-credit_amount) from voucher_bill_head a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " datediff(dd, voucher_bill_date, getdate()) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and debit_amount>credit_amount and a.company_idno = tz.company_idno group by agent_idno, ledger_idno")
    '            oldvl = Val(b(i)) + 1
    '            T = T & "sum(currency" & Trim(i + 1) & "),"
    '        Next i
    '        Grid1.FormatString = S & "|>ABV " & Val(b(i - 1)) & "    |>TOTAL         "
    '        cn1.Execute("insert into reporttemp ( int1, int2, currency" & Trim(i + 1) & " ) Select agent_idno, ledger_idno, sum(debit_amount-credit_amount) from voucher_bill_head a, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " datediff(dd, voucher_bill_date, getdate()) > " & Str(Val(b(i - 1))) & " and debit_amount>credit_amount and a.company_idno = tz.company_idno group by agent_idno, ledger_idno")
    '        T = T & "sum(currency" & Trim(i + 1) & ")"
    '        For i = 0 To UBound(b) + 2
    '            Grid1.ColData(i + 2) = 14
    '            Grid1.ColWidth(i + 2) = 1500
    '        Next i

    '        Rs = New ADODB.Recordset
    '        With Rs
    '            .Open("Select b.ledger_name as agent_name, c.ledger_name, " & T & " from reporttemp a, ledger_head b, ledger_head c where int1 = b.ledger_idno and int2 = c.ledger_idno group by b.ledger_name, c.ledger_name order by b.ledger_name, c.ledger_name", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    tt = 0
    '                    Grid1.AddItem(IIf(AgNm <> !agent_name, !agent_name, "") & vbTab & !ledger_name)
    '                    For i = 2 To .Fields.Count - 1
    '                        If Val(Rs(i)) > 0 Then
    '                            Grid1.TextMatrix(Grid1.Rows - 1, i) = PROC.Currency_Format(Rs(i))
    '                            tt = tt + Val(Rs(i))
    '                            tt_c(i) = tt_c(i) + Val(Rs(i))
    '                        End If
    '                    Next i
    '                    AgNm = !agent_name
    '                    sl = sl + 1
    '                    Grid1.TextMatrix(Grid1.Rows - 1, i) = PROC.Currency_Format(tt)
    '                    tt_c(i) = tt_c(i) + tt
    '                    .MoveNext()
    '                    If Not .EOF Then If !agent_name <> AgNm Then GoSub Total_Customer
    '                Loop
    '                GoSub Total_Customer
    '                Grid1.AddItem("GRAND TOTAL")
    '                For i = 2 To UBound(b) + 4
    '                    Grid1.TextMatrix(Grid1.Rows - 1, i) = PROC.Currency_Format(tt_a(i))
    '                Next i
    '                Grid1.RowData(Grid1.Rows - 1) = "3"
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            End If
    '            .Close()
    '        End With
    '        Rs = Nothing

    '        Exit Sub

    'Total_Customer:
    '        If sl > 1 Then
    '            Grid1.AddItem(vbTab & "TOTAL (AGENT)")
    '            For i = 2 To UBound(b) + 4
    '                Grid1.TextMatrix(Grid1.Rows - 1, i) = PROC.Currency_Format(tt_c(i))
    '                tt_a(i) = tt_a(i) + tt_c(i)
    '            Next i
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Else
    '            Grid1.AddItem("")
    '            For i = 2 To UBound(b) + 4
    '                tt_a(i) = tt_a(i) + tt_c(i)
    '            Next i
    '        End If
    '        sl = 0
    '        Erase tt_c
    '        Return

    '    End Sub

    '    Private Sub Accounts_SundryBook()
    '        Dim Rs1 As Recordset, Rt1 As Recordset
    '        Dim Ttc As Currency, Ttd As Currency
    '        Dim dt_cndt As String, ent_idno As String

    '        Grid1.FormatString = "<DATE          |<ENT ID              |<COMP |<PARTICULARS                                           |<PARTICULARS                                           |<TYPE   |>DB.AMOUNT       |>CR.AMOUNT       |>BALANCE             |<NARRATION                                  |<VOU.NO"
    '        Grid1.ColWidth(4) = 0 : Grid1.ColWidth(8) = 0 : Grid1.ColWidth(10) = 0
    '        'Grid1.ColWidth(2) = 3000: Grid1.ColWidth(3) = 2000: Grid1.ColWidth(7) = 1800: Grid1.ColWidth(8) = 2800: Grid1.ColWidth(9) = 0
    '        FrmNm.Label1(0).Caption = "LEDGER : " & Trim(RptDet_Name1) & " - RANGE : " & Format(RptDet_Date1, "dd-mm-yyyy") & " TO " & Format(RptDet_Date2, "dd-mm-yyyy")
    '        RptHeading1 = "LEDGER : " & Trim(RptDet_Name1)
    '        RptHeading2 = "RANGE : " & Format(RptDet_Date1, "dd-mm-yyyy") & " TO " & Format(RptDet_Date2, "dd-mm-yyyy")

    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select sum(voucher_amount) from voucher_details where ledger_idno = " & Str(RptDet_IdNo1) & " and voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "'", cn1)
    '        If Rs1(0).Value <> "" Then Ttc = Val(Rs1(0).Value)
    '        Rs1.Close()
    '        Rs1 = Nothing

    '        If Ttc <> 0 Then Grid1.AddItem(vbTab & vbTab & vbTab & "   OPENING BALANCE" & vbTab & "   OPENING BALANCE" & vbTab & "" & vbTab & IIf(Ttc < 0, "", vbTab) & PROC.Currency_Format(Abs(Ttc)))
    '        If Ttc < 0 Then Ttd = Abs(Ttc) : Ttc = 0
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)

    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select tz.company_shortname, a.entry_identification, a.voucher_date, a.voucher_amount, b.voucher_no, b.voucher_type, tl.ledger_name, a.narration from voucher_details a, voucher_head b, ledger_head tl, company_head tz where a.voucher_ref_no in ( Select z.voucher_ref_no from voucher_details z where z.ledger_idno = tl.ledger_idno and a.company_idno = z.company_idno ) and " & Condt & IIf(Condt <> "", " and ", "") & " a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.company_idno = tz.company_idno and a.voucher_ref_no = b.voucher_ref_no and a.company_idno = b.company_idno and a.ledger_idno = tl.ledger_idno order by a.voucher_date, b.for_orderby", cn1, adOpenStatic, adLockReadOnly)
    '        With Rs1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If Trim(Format(!Voucher_Date, "dd-mm-yy")) = Trim(Grid1.TextMatrix(Grid1.Rows - 1, 0)) Then Grid1.TextMatrix(Grid1.Rows - 1, 7) = ""
    '                    If !Voucher_Amount < 0 Then Ttc = Ttc + Abs(Val(!Voucher_Amount)) Else Ttd = Ttd + Abs(Val(!Voucher_Amount))
    '                    If Left(!entry_identification, 6) = "VOUCH-" Then
    '                        ent_idno = UCase(!Voucher_Type) & "-" & !Voucher_No
    '                    Else
    '                        ent_idno = Replace(!entry_identification, "/" & Cmp_FnYear, "")
    '                    End If
    '                    Grid1.AddItem(Format(!Voucher_Date, "dd-mm-yy") & Chr(9) & ent_idno & Chr(9) & !Company_ShortName & Chr(9) & IIf(!Voucher_Amount > 0, "By " & Trim(!ledger_name), "To " & Trim(!ledger_name)) & Chr(9) & Trim(StrConv(!Narration, vbProperCase)) & Chr(9) & Trim(!Voucher_Type) & Chr(9) & IIf(!Voucher_Amount > 0, PROC.Currency_Format(Abs(!Voucher_Amount)) & vbTab, vbTab & PROC.Currency_Format(Abs(!Voucher_Amount))) & Chr(9) & PROC.Currency_Format(Abs(Ttc - Ttd)) & IIf(Ttc > Ttd, " Cr", " Dr") & Chr(9) & Trim(!Narration) & Chr(9) & !entry_identification)
    '                    Mdi1.StatusBar3.Panels(2).Text = Format(!Voucher_Date, "dd mmm")
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing
    '        Grid1.AllowUserResizing = 0
    '        Grid1.AddItem("")
    '        Grid1.AddItem(Chr(9) & Chr(9) & Chr(9) & "TOTAL" & Chr(9) & "TOTAL" & Chr(9) & "" & Chr(9) & PROC.Currency_Format(Ttd) & Chr(9) & PROC.Currency_Format(Ttc))
    '        Grid1.AddItem(Chr(9) & Chr(9) & Chr(9) & "CLOSING BALANCE" & Chr(9) & "CLOSING BALANCE" & Chr(9) & "" & Chr(9) & IIf(Ttc - Ttd < 0, "", vbTab) & PROC.Currency_Format(Abs(Ttc - Ttd)))
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '    End Sub

    '    Public Function Printing(ByVal CN As ADODB.Connection, ByVal MdiFrm1 As Object, ByVal Grd As Object, ByVal RMain As String, ByVal RSub As String, ByVal FnYr As String, ByVal FromDt As Date, ByVal ToDt As Date, ByVal RptDt_IdNo1 As Integer, ByVal RptDt_IdNo2 As Integer, ByVal RptDt_IdNo3 As Integer, ByVal RptDt_Name1 As String, ByVal RptDt_Name2 As String, ByVal RptDt_Name3 As String, ByVal RptDt_Tex_Val1 As String, ByVal RptDt_Tex_Val2 As String, ByVal RptDt_Date1 As Date, ByVal RptDt_Date2 As Date, ByVal Company_Name As String, ByVal Company_Address As String) As Boolean
    '        Dim i As Integer
    '        Dim v As String

    '        PROC = GetObject("", "Smart_Procedures_NT10.Basic_Procedures")

    '        cn1 = CN
    '        Mdi1 = MdiFrm1
    '        Grid1 = Grd

    '        Rpt_Main = RMain
    '        Rpt_Sub = RSub
    '        Cmp_Name = Company_Name
    '        Cmp_Address = Company_Address
    '        Cmp_FnYear = FnYr
    '        Cmp_FromDt = FromDt
    '        Cmp_ToDt = ToDt

    '        RptDet_IdNo1 = RptDt_IdNo1
    '        RptDet_IdNo2 = RptDt_IdNo2
    '        RptDet_IdNo3 = RptDt_IdNo3
    '        RptDet_Name1 = RptDt_Name1
    '        RptDet_Name2 = RptDt_Name2
    '        RptDet_Name3 = RptDt_Name3
    '        RptDet_Tex_Val1 = RptDt_Tex_Val1
    '        RptDet_Tex_Val2 = RptDt_Tex_Val2
    '        RptDet_Date1 = RptDt_Date1
    '        RptDet_Date2 = RptDt_Date2

    '        Rpt_Main = RMain
    '        Rpt_Sub = RSub
    '        Cmp_FnYear = FnYr
    '        Cmp_FromDt = FromDt
    '        Cmp_ToDt = ToDt

    '        Call Report_Intialize(cn1)

    '        If InStr(LCase(Condt), "company_idno") > 0 Then
    '            i = InStr(LCase(Condt), "company_idno")
    '            v = Right(Condt, Len(Condt) - i + 1)
    '            i = InStr(v, ")")
    '            v = " where " & Left(v, i)

    '            Dim Rs As ADODB.Recordset

    '            Cmp_Address = ""
    '            Cmp_Name = ""
    '            Rs = New ADODB.Recordset
    '            Rs.Open("Select * from company_head " & v & " order by company_name", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Cmp_Address = Trim(Rs!Company_Address1 & " " & Rs!Company_Address2 & " " & Rs!Company_Address3 & " " & Rs!Company_Address4)
    '                Cmp_Name = Rs!Company_Name
    '            End If
    '            Rs.Close()
    '            Rs = Nothing
    '        End If

    '        Open "c:\samp1.txt" For Output As #1
    '        Printing = True
    '        Select Case RMain
    '            Case "LEDGER A/C", "BANK BOOK", "CASH BOOK", "PURCHASE BOOK", "SALES BOOK", "LEDGER A/C (LW)"
    '                If Grid1.ColWidth(8) > 0 Then
    '                    Call Print_SingleLedger_WithDayBalance()
    '                Else
    '                    Call Print_SingleLedger()
    '                End If
    '            Case "LEDGER A/C - Confirmation Details"
    '                Call Confirmation_Of_Accounts_Details()

    '            Case "Single Ledger - Month Wise", "LEDGER A/C (MONTH WISE)", "LEDGER A/C (MONTHLY)"
    '                Call Print_MonthLedger()
    '            Case "Day Book", "DAY BOOK"
    '                Call Print_DayBook()
    '            Case "Group Ledger", "GROUP LEDGER"
    '                Call Print_GroupLedger()
    '            Case "Opening TB", "OPENING TRIAL BALANCE"
    '                Call Print_OpeningTB()
    '            Case "General TB", "GENERAL TRIAL BALANCE"
    '                Call Print_GeneralTB()
    '            Case "Group TB", "GROUP TRIAL BALANCE"
    '                Call Print_GroupTB()
    '            Case "Final TB"
    '                Call Print_FinalTB()
    '            Case "ALL LEDGER"
    '                Call Print_AllLedger()
    '            Case Else
    '                Call Common_Printing()
    '        End Select
    '        Close #1

    '    End Function

    '    Private Sub Print_SingleLedger_WithDayBalance()
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '        Dim tt_cr As Currency, Tt_Dr As Currency
    '        Dim nar As String

    '        If Trim(Cmp_Name) <> "" Then
    '            RptHeading2 = RptHeading3
    '            RptHeading3 = ""
    '        End If

    '    GoSub Page_Header

    '        For i = 2 To Grid1.Rows - 4
    '        If Ln_No > 61 Then GoSub Page_Footer
    '        Print #1, Chr(27); "M"; Trim(Grid1.TextMatrix(i, 0)); Spc(10 - Len(Trim(Grid1.TextMatrix(i, 0)))); Chr(27); "P";
    '        Print #1, Chr(27); "M"; Trim(Grid1.TextMatrix(i, 2)); Spc(6 - Len(Trim(Grid1.TextMatrix(i, 2)))); Chr(27); "P";
    '        If Grid1.ColWidth(3) > 0 Then Print #1, Chr(27); "g"; Trim(Grid1.TextMatrix(i, 3)); Spc(44 - Len(Trim(Grid1.TextMatrix(i, 3)))); Chr(27); "P";
    '        If Grid1.ColWidth(4) > 0 Then Print #1, Chr(27); "g"; Trim(Grid1.TextMatrix(i, 4)); Spc(44 - Len(Trim(Grid1.TextMatrix(i, 4)))); Chr(27); "P";
    '        Print #1, Chr(27); "g"; Trim(Grid1.TextMatrix(i, 5)); Spc(6 - Len(Trim(Grid1.TextMatrix(i, 5)))); Chr(27); "P";
    '        Print #1, Chr(27); "M"; Spc(14 - Len(Trim(Grid1.TextMatrix(i, 6)))); Trim(Grid1.TextMatrix(i, 6)); Chr(27); "P";
    '        Print #1, Chr(27); "M"; Spc(14 - Len(Trim(Grid1.TextMatrix(i, 7)))); Trim(Grid1.TextMatrix(i, 7)); Chr(27); "P";
    '        If Grid1.ColWidth(8) > 0 Then Print #1, Chr(27); "M"; Spc(17 - Len(Trim(Grid1.TextMatrix(i, 8)))); Trim(Grid1.TextMatrix(i, 8)); Chr(27); "P" Else Print #1,
    '            If Val(Grid1.TextMatrix(i, 6)) <> 0 Then Tt_Dr = Tt_Dr + CCur(Grid1.TextMatrix(i, 6))
    '            If Val(Grid1.TextMatrix(i, 7)) <> 0 Then tt_cr = tt_cr + CCur(Grid1.TextMatrix(i, 7))
    '            Ln_No = Ln_No + 1

    '            If Trim(Grid1.TextMatrix(i, 1)) <> Trim(Grid1.TextMatrix(i + 1, 1)) Then
    '                If Grid1.ColWidth(9) > 0 And Trim(Grid1.TextMatrix(i, 9)) <> "" Then
    '                    nar = Trim(Grid1.TextMatrix(i, 9))
    '                    Do While Len(nar) > 41
    '                        For k = 40 To 1 Step -1
    '                            If Mid$(nar, k, 1) = " " Or Mid$(nar, k, 1) = "," Or Mid$(nar, k, 1) = ")" Then Exit For
    '                        Next k
    '                        If k = 0 Then k = 40
    '                    Print #1, Chr(27); "M"; Spc(10); Chr(27); "P";
    '                    Print #1, Chr(27); "M"; Spc(6); Chr(27); "P";
    '                    Print #1, Chr(27); "g"; "   "; Trim(Left$(nar, k)); Chr(27); "P"
    '                        Ln_No = Ln_No + 1
    '                        nar = Right(nar, Len(nar) - k)
    '                    Loop
    '                Print #1, Chr(27); "M"; Spc(10); Chr(27); "P";
    '                Print #1, Chr(27); "M"; Spc(6); Chr(27); "P";
    '                Print #1, Chr(27); "g"; "   "; Trim(nar); Chr(27); "P"
    '                    Ln_No = Ln_No + 1
    '                End If
    '            Print #1,
    '            Ln_No = Ln_No + 1
    '            End If
    '        Next i

    '        'Print #1, Chr(27); "M"; String(78, 45); String(IIf(Grid1.ColWidth(7) > 0, 17, 0), 45); Chr(27); "P"
    '    Print #1, Chr(27); "M"; String(91, 45); String(IIf(Grid1.ColWidth(7) > 0, 12, 0), 45); Chr(27); "P"

    '    Print #1, Chr(27); "M"; Spc(10); Chr(27); "P";
    '    Print #1, Chr(27); "M"; Spc(6); Chr(27); "P";
    '    Print #1, Chr(27); "g"; Spc(3); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 4)); Spc(41 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 4)))); Chr(27); "P";
    '    Print #1, Chr(27); "g"; Spc(6); Chr(27); "P";
    '    Print #1, Chr(27); "M"; Spc(14 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 6)))); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 6)); Chr(27); "P";
    '    Print #1, Chr(27); "M"; Spc(14 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 7)))); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 7)); Chr(27); "P"

    '        'Print #1, Chr(27); "M"; String(78, 45); String(IIf(Grid1.ColWidth(7) > 0, 17, 0), 45); Chr(27); "P"
    '    Print #1, Chr(27); "M"; String(91, 45); String(IIf(Grid1.ColWidth(7) > 0, 12, 0), 45); Chr(27); "P"

    '    Print #1, Chr(27); "M"; Spc(10); Chr(27); "P";
    '    Print #1, Chr(27); "M"; Spc(6); Chr(27); "P";
    '    Print #1, Chr(27); "g"; Spc(3); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 4)); Spc(41 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 4)))); Chr(27); "P";
    '    Print #1, Chr(27); "g"; Spc(6); Chr(27); "P";
    '    Print #1, Chr(27); "M"; Spc(14 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 6)))); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 6)); Chr(27); "P";
    '    Print #1, Chr(27); "M"; Spc(14 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 7)))); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 7)); Chr(27); "P"

    '        'Print #1, Chr(27); "M"; String(78, 45); String(IIf(Grid1.ColWidth(7) > 0, 17, 0), 45); Chr(27); "P"
    '    Print #1, Chr(27); "M"; String(91, 45); String(IIf(Grid1.ColWidth(7) > 0, 12, 0), 45); Chr(27); "P"

    '        Exit Sub

    'Page_Header:
    '        If RptDet.RptCode_Main <> "LEDGER A/C - Confirmation Details" Then
    '            Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '            Print #1, Chr(27); "g"; Spc(60 - (Len(Cmp_Address) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '            Print #1,
    '            Pg_No = Pg_No + 1
    '            'Print #1, Chr(18); Chr(27); "P"; Spc(40 - (Len(Trim("Ledger : " & RptInp(1).Caption)) / 2)); Chr(27); "E"; Trim("LEDGER : " & RptInp(1).Caption); Chr(27); "F"
    '            'Print #1, Chr(18); Chr(27); "P"; Spc(40 - (Len(Trim(Format(RptDet_Date1, "dd/mm/yyyy") & " To " & Format(RptDet_Date2, "dd/mm/yyyy"))) / 2)); Chr(27); "E"; Trim(Format(RptDet_Date1, "dd/mm/yyyy") & " To " & Format(RptDet_Date2, "dd/mm/yyyy")); Chr(27); "F"

    '            Print #1, Spc(40 - (Len(RptHeading1) / 2)); Chr(27); "E"; RptHeading1; Chr(27); "F"
    '            Print #1, Spc(40 - (Len(RptHeading2) / 2)); Chr(27); "E"; RptHeading2; Chr(27); "F"
    '            Print #1, Spc(40 - (Len(RptHeading3) / 2)); Chr(27); "E"; RptHeading3; Chr(27); "F"

    '            Print #1, Chr(18); Chr(27); "P"; Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        End If

    '        'Print #1, Chr(27); "M"; String(91, 45); String(IIf(Grid1.ColWidth(7) > 0, 17, 0), 45); Chr(27); "P"
    '        'Print #1, Chr(27); "M"; "  DATE    COMP      PARTICULARS                                           DEBIT        CREDIT"; IIf(Grid1.ColWidth(7) > 0, "       BALANCE", "")
    '        'Print #1, Chr(27); "M"; String(91, 45); String(IIf(Grid1.ColWidth(7) > 0, 17, 0), 45); Chr(27); "P"
    '        Print #1, Chr(27); "M"; String(91, 45); String(IIf(Grid1.ColWidth(7) > 0, 12, 0), 45); Chr(27); "P"
    '         Print #1, Chr(27); "M"; "  DATE    COMP  PARTICULARS                                      DEBIT        CREDIT"; IIf(Grid1.ColWidth(7) > 0, "       BALANCE", "")
    '        Print #1, Chr(27); "M"; String(91, 45); String(IIf(Grid1.ColWidth(7) > 0, 12, 0), 45); Chr(27); "P"
    '        Ln_No = 10
    '        Return

    'Page_Footer:
    '        Print #1, Chr(27); "M"; String(79, 45); String(IIf(Grid1.ColWidth(7) > 0, 17, 0), 45); Chr(27); "P"
    '        Print #1, Chr(27); "M"; Spc(10); Chr(27); "P";
    '        Print #1, Chr(27); "g"; "   C/O"; Spc(37); Chr(27); "P";
    '        Print #1, Chr(27); "g"; Spc(7); Chr(27); "P";
    '        Print #1, Chr(27); "M"; Spc(14 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr)); Chr(27); "P";
    '        Print #1, Chr(27); "M"; Spc(14 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr)); Chr(27); "P"
    '        Print #1, Chr(27); "M"; String(79, 45); String(IIf(Grid1.ColWidth(7) > 0, 17, 0), 45); Chr(27); "P"
    '        Print #1, Chr(18); Chr(27); "P"; Spc(72); "Contd..."
    '        Ln_No = Ln_No + 4
    '        If RptDet.RptCode_Main <> "LEDGER A/C - Confirmation Details" Then
    '            For k = Ln_No + 1 To 72
    '                Print #1, ""
    '            Next k
    '        End If
    '        GoSub Page_Header
    '        Print #1, Chr(27); "M"; Spc(10); Chr(27); "P";
    '        Print #1, Chr(27); "g"; "   B/F"; Spc(37); Chr(27); "P";
    '        Print #1, Chr(27); "g"; Spc(7); Chr(27); "P";
    '        Print #1, Chr(27); "M"; Spc(14 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr)); Chr(27); "P";
    '        Print #1, Chr(27); "M"; Spc(14 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr)); Chr(27); "P"
    '        Print #1,
    '        Ln_No = Ln_No + 2
    '        Return
    '    End Sub

    '    Private Sub Print_SingleLedger()
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '        Dim tt_cr As Currency, Tt_Dr As Currency
    '        Dim nar As String

    '        If Trim(Cmp_Name) <> "" Then
    '            RptHeading2 = RptHeading3
    '            RptHeading3 = ""
    '        End If

    '    GoSub Page_Header

    '        For i = 2 To Grid1.Rows - 4
    '        If Ln_No > 60 Then GoSub Page_Footer
    '            If Trim(Grid1.TextMatrix(i, 1)) <> Trim(Grid1.TextMatrix(i - 1, 1)) Then
    '            Print #1, Trim(Grid1.TextMatrix(i, 0)); Spc(11 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '            If Grid1.ColWidth(2) > 0 Then Print #1, Grid1.TextMatrix(i, 2); Spc(7 - Len(Grid1.TextMatrix(i, 2)));
    '            Else
    '            Print #1, Spc(11);
    '            If Grid1.ColWidth(2) > 0 Then Print #1, Spc(7);
    '            End If
    '        If Grid1.ColWidth(3) > 0 Then Print #1, Left$(Trim(Grid1.TextMatrix(i, 3)), 30); Spc(32 - Len(Left$(Trim(Grid1.TextMatrix(i, 3)), 30)));
    '        If Grid1.ColWidth(4) > 0 Then Print #1, Left$(Trim(Grid1.TextMatrix(i, 4)), 30); Spc(32 - Len(Left$(Trim(Grid1.TextMatrix(i, 4)), 30)));
    '        Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(i, 6)))); Trim(Grid1.TextMatrix(i, 6));
    '        Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(i, 7)))); Trim(Grid1.TextMatrix(i, 7))
    '            If Val(Grid1.TextMatrix(i, 6)) <> 0 Then Tt_Dr = Tt_Dr + CCur(Grid1.TextMatrix(i, 6))
    '            If Val(Grid1.TextMatrix(i, 7)) <> 0 Then tt_cr = tt_cr + CCur(Grid1.TextMatrix(i, 7))
    '            Ln_No = Ln_No + 1
    '            If Trim(Grid1.TextMatrix(i, 1)) <> Trim(Grid1.TextMatrix(i + 1, 1)) Then
    '                If Grid1.ColWidth(9) > 0 And Trim(Grid1.TextMatrix(i, 9)) <> "" Then
    '                    nar = Trim(Grid1.TextMatrix(i, 9))
    '                    Do While Len(nar) > 35
    '                        For k = 35 To 1 Step -1
    '                            If Mid$(nar, k, 1) = " " Or Mid$(nar, k, 1) = "," Or Mid$(nar, k, 1) = ")" Then Exit For
    '                        Next k
    '                        If k = 0 Then k = 35
    '                    Print #1, Spc(18);
    '                    Print #1, "   "; Trim(Left$(nar, k))
    '                        Ln_No = Ln_No + 1
    '                        nar = Right(nar, Len(nar) - k)
    '                    Loop
    '                Print #1, Spc(18);
    '                Print #1, "   "; Trim(nar)
    '                    Ln_No = Ln_No + 1
    '                End If
    '            Print #1,
    '            Ln_No = Ln_No + 1
    '            End If
    '        Next i
    '    Print #1, String(80, 45)
    '    Print #1, Spc(18);
    '    Print #1, Spc(3); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 4)); Spc(29 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 4))));
    '    Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 6)))); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 6));
    '    Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 7)))); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 7))
    '    Print #1, String(80, 45)
    '    Print #1, Spc(18);
    '    Print #1, Spc(3); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 4)); Spc(29 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 4))));
    '    Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 6)))); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 6));
    '    Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 7)))); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 7))
    '    Print #1, String(80, 45)
    '        Ln_No = Ln_No + 5
    '        For k = Ln_No + 1 To 72
    '        Print #1, ""
    '        Next k

    '        Exit Sub

    'Page_Header:
    '        Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '        Print #1, Chr(27); "M"; Spc(48 - (Len(Trim(Cmp_Address)) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '        Print #1,
    '        Pg_No = Pg_No + 1
    '        Print #1, Spc(40 - (Len(RptHeading1) / 2)); Chr(27); "E"; RptHeading1; Chr(27); "F"
    '        Print #1, Spc(40 - (Len(RptHeading2) / 2)); Chr(27); "E"; RptHeading2; Chr(27); "F"
    '        Print #1, Spc(40 - (Len(RptHeading3) / 2)); Chr(27); "E"; RptHeading3; Chr(27); "F"
    '        Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        Print #1, String(80, 45)
    '        Print #1, "  DATE               PARTICULARS                           DEBIT         CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = 10
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(18);
    '        Print #1, "   C/O"; Spc(26);
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr));
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr))
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 4
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '        Print #1, Spc(18);
    '        Print #1, "   B/F"; Spc(26);
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr));
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr))
    '        Print #1,
    '        Ln_No = Ln_No + 2
    '        Return
    '    End Sub

    '    Public Sub Print_MonthLedger()
    '        Dim i As Integer
    '        Dim rng As String

    '        Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '        Print #1, Chr(27); "M"; Spc(48 - (Len(Trim(Cmp_Address)) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '        Print #1,
    '        Print #1, Spc(40 - (Len(Trim("Ledger : " & RptDet_Name1)) / 2)); Chr(27); "E"; Trim("LEDGER : " & RptDet_Name1); Chr(27); "F"
    '        rng = "FROM THE MONTH OF APRIL - " & Trim(Year(Cmp_FromDt)) & " TO " & RptDet_Name2 & " - " & Trim(Year(IIf(RptDet_IdNo2 > 3, Cmp_FromDt, Cmp_ToDt)))
    '        Print #1, Spc(40 - (Len(rng) / 2)); Chr(27); "E"; rng; Chr(27); "F"
    '        Print #1,
    '        Print #1, String(80, 196)
    '        Print #1, Spc(2); Chr(27); "E"; "MONTH"; Spc(6); Spc(6); "OPENING"; Spc(2); Spc(9); "CREDIT"; Spc(10); "DEBIT"; Spc(12); "CLOSING"; Chr(27); "F"
    '        Print #1, String(80, 196)
    '        Print #1,
    '        For i = 2 To Grid1.Rows - 1
    '            Print #1, Spc(1); Trim(Grid1.TextMatrix(i, 0)); Spc(10 - Len(Trim(Grid1.TextMatrix(i, 0)))); Spc(18 - Len(Trim(Grid1.TextMatrix(i, 1)))); Trim(Grid1.TextMatrix(i, 1)); Spc(15 - Len(Trim(Grid1.TextMatrix(i, 2)))); Trim(Grid1.TextMatrix(i, 2)); Spc(15 - Len(Trim(Grid1.TextMatrix(i, 3)))); Trim(Grid1.TextMatrix(i, 3)); Spc(20 - Len(Trim(Grid1.TextMatrix(i, 4)))); Trim(Grid1.TextMatrix(i, 4))
    '        Next i
    '        Print #1,
    '        Print #1, String(80, 196)
    '        Print #1, Chr(12)
    '    End Sub

    '    Private Sub Print_DayBook()
    '        Dim Rt1 As Recordset
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '        Dim tt_cr As Currency, Tt_Dr As Currency
    '        Dim Dt1 As Date, dt2 As Date
    '        Dim vno As String, nar As String, dt3 As String
    '        Dim cmpid As Integer

    '        Rt1 = New ADODB.Recordset
    '        With Rt1
    '            .Open("Select sum(voucher_amount) from voucher_details tz where " & Condt & IIf(Condt <> "", " and ", "") & " ledger_idno in ( Select ledger_idno from ledger_head where parent_code like '%~6~4~%' ) and voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "'", cn1, adOpenStatic, adLockReadOnly)
    '            If Rt1(0).Value <> "" Then If Val(Rt1(0).Value) >= 0 Then Tt_Dr = Val(Rt1(0).Value) Else tt_cr = Abs(Val(Rt1(0).Value))
    '            .Close()

    '            .Open("Select tz.*, b.ledger_idno, c.ledger_name, b.voucher_amount, b.narration, c.parent_code from voucher_head tz, voucher_details b, ledger_head c where " & Condt & IIf(Condt <> "", " and ", "") & " tz.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and tz.voucher_ref_no = b.voucher_ref_no and tz.company_idno = b.company_idno and b.ledger_idno = c.ledger_idno order by tz.voucher_date, tz.company_idno, tz.for_orderby, b.sl_no", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()

    '                    GoSub Page_Header
    '                dt2 = !Voucher_Date
    '                    GoSub Day_Opening

    '                Do While Not .EOF
    '                    If InStr(!Parent_Code, "~6~4~") = 0 Then
    '                            Print #1, dt3; Spc(10 - Len(dt3));
    '                            Print #1, IIf(!Voucher_Amount > 0, "To ", "By ") & !ledger_name; Spc(35 - Len(!ledger_name));
    '                            If !Voucher_Amount < 0 Then Print #1, Spc(16 - Len(PROC.Currency_Format(Abs(!Voucher_Amount)))); PROC.Currency_Format(Abs(!Voucher_Amount)); Else Print #1, Spc(16);
    '                            If !Voucher_Amount > 0 Then Print #1, Spc(16 - Len(PROC.Currency_Format(!Voucher_Amount))); PROC.Currency_Format(!Voucher_Amount) Else Print #1, Spc(16)
    '                        Ln_No = Ln_No + 1
    '                        If !Voucher_Amount > 0 Then tt_cr = tt_cr + !Voucher_Amount Else Tt_Dr = Tt_Dr + Abs(!Voucher_Amount)
    '                    End If
    '                    Dt1 = Trim(!Voucher_Date)
    '                    vno = Trim(!voucher_ref_no)
    '                    cmpid = !Company_Idno
    '                    nar = Trim(!Narration)
    '                    dt3 = ""
    '                    .MoveNext()
    '                    If Not .EOF Then
    '                        dt2 = Trim(!Voucher_Date)
    '                            If vno <> Trim(!voucher_ref_no) Or cmpid <> !Company_Idno Then GoSub Narration_Print
    '                    Else
    '                        dt2 = DateAdd("d", 1, Dt1)
    '                            GoSub Narration_Print
    '                    End If

    '                    If Dt1 <> dt2 Then
    '                        Mdi1.StatusBar3.Panels(2).Text = Trim(Format(dt2, "dd mmmm"))
    '                            Print #1, Spc(48); String(32, 45)
    '                            Print #1, Spc(10);
    '                            Print #1, "TOTAL"; Spc(33);
    '                            Print #1, Spc(16 - Len(PROC.Currency_Format(Tt_Dr))); PROC.Currency_Format(Tt_Dr);
    '                            Print #1, Spc(16 - Len(PROC.Currency_Format(tt_cr))); PROC.Currency_Format(tt_cr)
    '                        If tt_cr > Tt_Dr Then
    '                            tt_cr = tt_cr - Tt_Dr : Tt_Dr = 0
    '                        Else
    '                            Tt_Dr = Tt_Dr - tt_cr : tt_cr = 0
    '                        End If
    '                            Print #1, Spc(10); Chr(27); "E";
    '                            Print #1, "DAY CLOSING"; Spc(27);
    '                            If Tt_Dr > 0 Then Print #1, Spc(16 - Len(PROC.Currency_Format(Tt_Dr))); PROC.Currency_Format(Tt_Dr); Else Print #1, Spc(16);
    '                            If tt_cr > 0 Then Print #1, Spc(16 - Len(PROC.Currency_Format(tt_cr))); PROC.Currency_Format(tt_cr); Else Print #1, Spc(16);
    '                            Print #1, Chr(27); "F"
    '                            Print #1, Spc(48); String(32, 45)
    '                            Print #1,
    '                            Ln_No = Ln_No + 5
    '                        If Not .EOF Then
    '                                If Ln_No > 58 Then GoSub Page_Footer
    '                                GoSub Day_Opening
    '                        Else
    '                                Print #1, String(80, 45)
    '                                Print #1,
    '                                Print #1, Spc(35); "---  *  ---"
    '                            Ln_No = Ln_No + 3
    '                            For k = Ln_No + 1 To 72
    '                                    Print #1, ""
    '                            Next k
    '                        End If
    '                    Else
    '                        If Ln_No > 58 Then
    '                                GoSub Page_Footer
    '                            dt3 = Trim(Format(dt2, "dd-mm-yy"))
    '                        End If
    '                    End If
    '                Loop
    '            End If
    '        End With
    '        Rt1 = Nothing

    '        Exit Sub

    'Page_Header:
    '        Print #1, Chr(18); Chr(14); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"; Chr(20)
    '        Print #1, Chr(27); "M"; Trim(Cmp_Address); Chr(27); "P"
    '        Print #1,
    '        Pg_No = Pg_No + 1
    '        Print #1, Spc(36); Chr(27); "E"; "DAY BOOK"; Chr(27); "F"
    '        Print #1, Spc(40 - (Len(Trim(Format(RptDet_Date1, "dd/mm/yyyy") & " To " & Format(RptDet_Date2, "dd/mm/yyyy"))) / 2)); Chr(27); "E"; Trim(Format(RptDet_Date1, "dd/mm/yyyy") & " To " & Format(RptDet_Date2, "dd/mm/yyyy")); Chr(27); "F"
    '        Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        Print #1, String(80, 45)
    '        Print #1, "  DATE          PARTICULARS                              DEBIT          CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = 9
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(10);
    '        Print #1, "   C/O"; Spc(32);
    '        Print #1, Spc(16 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr));
    '        Print #1, Spc(16 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr))
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 4
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '        Print #1, Spc(10);
    '        Print #1, "   B/F"; Spc(32);
    '        Print #1, Spc(16 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr));
    '        Print #1, Spc(16 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr))
    '        Print #1,
    '        Ln_No = Ln_No + 2
    '        Return

    'Day_Opening:
    '        Print #1, Trim(Format(dt2, "dd-mm-yy")); Spc(10 - Len(Trim(Format(dt2, "dd-mm-yy"))));
    '        Print #1, "DAY OPENING"; Spc(27);
    '        If Tt_Dr > 0 Then Print #1, Spc(16 - Len(PROC.Currency_Format(Tt_Dr))); PROC.Currency_Format(Tt_Dr); Else Print #1, Spc(16);
    '        If tt_cr > 0 Then Print #1, Spc(16 - Len(PROC.Currency_Format(tt_cr))); PROC.Currency_Format(tt_cr) Else Print #1, Spc(16)
    '        Print #1,
    '        Ln_No = Ln_No + 2
    '        Return

    'Narration_Print:
    '        If Trim(nar) <> "" Then
    '            Do While Len(nar) > 35
    '                For k = 35 To 1 Step -1
    '                    If Mid$(nar, k, 1) = " " Or Mid$(nar, k, 1) = "," Or Mid$(nar, k, 1) = ")" Then Exit For
    '                Next k
    '                If k = 0 Then k = 35
    '                Print #1, Spc(10);
    '                Print #1, "   "; Trim(Left$(nar, k))
    '                Ln_No = Ln_No + 1
    '                nar = Right(nar, Len(nar) - k)
    '            Loop
    '            Print #1, Spc(10);
    '            Print #1, "   "; Trim(nar)
    '            Ln_No = Ln_No + 1
    '        End If
    '        Print #1,
    '        Ln_No = Ln_No + 1
    '        Return

    '    End Sub

    '    Private Sub Print_GroupLedger()
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '        Dim tt_cr As Currency, Tt_Dr As Currency
    '        Dim nar As String

    '    GoSub Page_Header
    '        For i = 2 To Grid1.Rows - 4
    '        If Ln_No > 61 Then GoSub Page_Footer
    '            If Left$(Trim(Grid1.TextMatrix(i, 0)), 1) = "-" Then
    '            Print #1, Spc(4); Trim(Left(Grid1.TextMatrix(i, 0), 39)); Spc(39 - Len(Trim(Left(Grid1.TextMatrix(i, 0), 39))));
    '            Else
    '            Print #1,
    '            Ln_No = Ln_No + 1
    '            Print #1, Spc(1); Chr(27); "E"; Trim(Grid1.TextMatrix(i, 0)); Spc(42 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '            End If
    '        Print #1, Spc(18 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '        Print #1, Spc(18 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2);
    '        If Left$(Trim(Grid1.TextMatrix(i, 0)), 1) <> "-" Then Print #1, Chr(27); "F" Else Print #1,
    '            Ln_No = Ln_No + 1
    '        Next i
    '        i = i + 1
    '    Print #1, String(80, 45)
    '    Print #1, Spc(4); Trim(Grid1.TextMatrix(i, 0)); Spc(39 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '    Print #1, Spc(18 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '    Print #1, Spc(18 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '        i = i + 1
    '    Print #1, String(80, 45)
    '    Print #1, Spc(4); Trim(Grid1.TextMatrix(i, 0)); Spc(39 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '    Print #1, Spc(18 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '    Print #1, Spc(18 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '    Print #1, String(80, 45)
    '        Ln_No = Ln_No + 5
    '        For k = Ln_No + 1 To 72
    '        Print #1, ""
    '        Next k

    '        Exit Sub

    'Page_Header:
    '        Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '        Print #1, Chr(27); "M"; Spc(48 - (Len(Trim(Cmp_Address)) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '        Print #1,
    '        Pg_No = Pg_No + 1
    '        Print #1, Spc(40 - (Len(RptHeading1) / 2)); Chr(27); "E"; RptHeading1; Chr(27); "F"
    '        Print #1, Spc(40 - (Len(RptHeading2) / 2)); Chr(27); "E"; RptHeading2; Chr(27); "F"
    '        Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        Print #1, String(80, 45)
    '        Print #1, "  PARTICULARS                                           DEBIT            CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = 9
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 2
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '        Return
    '    End Sub

    '    Private Sub Print_OpeningTB()
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '        Dim rng As String

    '    GoSub Page_Header
    '        For i = 2 To Grid1.Rows - 3
    '        If Ln_No > 61 Then GoSub Page_Footer
    '        Print #1, Spc(1); Trim(Grid1.TextMatrix(i, 0)); Spc(39 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '            Ln_No = Ln_No + 1
    '        Next i
    '    Print #1, String(80, 45)
    '        If Trim(Grid1.TextMatrix(i, 0)) <> "" Then
    '        Print #1, Spc(1); Trim(Grid1.TextMatrix(i, 0)); Spc(39 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '        Print #1, String(80, 45)
    '            Ln_No = Ln_No + 2
    '        End If
    '        i = i + 1
    '    Print #1, Spc(1); Trim(Grid1.TextMatrix(i, 0)); Spc(39 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '    Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '    Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '    Print #1, String(80, 45)
    '        Ln_No = Ln_No + 3
    '        For k = Ln_No + 1 To 72
    '        Print #1, ""
    '        Next k

    '        Exit Sub

    'Page_Header:
    '        Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '        Print #1, Chr(27); "M"; Spc(48 - (Len(Trim(Cmp_Address)) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '        Print #1,
    '        Pg_No = Pg_No + 1
    '        Print #1, Spc(29); Chr(27); "E"; Trim("OPENING TRAIL BALANCE"); Chr(27); "F"
    '        rng = Trim(MonthName(Month(Cmp_FromDt), True) & "'" & Trim(Year(Cmp_FromDt)) & " - " & MonthName(Month(Cmp_ToDt), True) & "'" & Trim(Year(Cmp_ToDt)))
    '        Print #1, Spc(40 - (Len(rng) / 2)); Chr(27); "E"; rng; Chr(27); "F"
    '        Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        Print #1, String(80, 45)
    '        Print #1, "  PARTICULARS                                          DEBIT              CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = 9
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 2
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '        Return
    '    End Sub

    '    Private Sub Print_GeneralTB()
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer

    '    GoSub Page_Header
    '        For i = 2 To Grid1.Rows - 3
    '        If Ln_No > 61 Then GoSub Page_Footer
    '        Print #1, Spc(1); Trim(Grid1.TextMatrix(i, 0)); Spc(39 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '            Ln_No = Ln_No + 1
    '        Next i
    '    Print #1, String(80, 45)
    '        If Trim(Grid1.TextMatrix(i, 0)) <> "" Then
    '        Print #1, Spc(1); Trim(Grid1.TextMatrix(i, 0)); Spc(39 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '        Print #1, String(80, 45)
    '            Ln_No = Ln_No + 2
    '        End If
    '        i = i + 1
    '    Print #1, Spc(1); Trim(Grid1.TextMatrix(i, 0)); Spc(39 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '    Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '    Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '    Print #1, String(80, 45)
    '        Ln_No = Ln_No + 3

    '        For k = Ln_No + 1 To 72
    '        Print #1, ""
    '        Next k

    '        Exit Sub

    'Page_Header:
    '        Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '        Print #1, Chr(27); "M"; Spc(48 - (Len(Trim(Cmp_Address)) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '        Print #1,
    '        Pg_No = Pg_No + 1
    '        Print #1, Spc(40 - (Len(Trim("GENERAL TRAIL BALANCE")) / 2)); Chr(27); "E"; Trim("GENERAL TRAIL BALANCE"); Chr(27); "F"
    '        Print #1, Spc(40 - (Len("AS ON " & Trim(Format(RptDet_Date1, "dd/mm/yyyy"))) / 2)); Chr(27); "E"; "AS ON " & Trim(Format(RptDet_Date1, "dd/mm/yyyy")); Chr(27); "F"
    '        Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        Print #1, String(80, 45)
    '        Print #1, "  PARTICULARS                                          DEBIT              CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = 9
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 2
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '        Return

    '    End Sub

    '    Private Sub Print_GroupTB()
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer

    '    GoSub Page_Header
    '        For i = 2 To Grid1.Rows - 3
    '        If Ln_No > 61 Then GoSub Page_Footer
    '            If Left$(Trim(Grid1.TextMatrix(i, 0)), 1) = "-" Then
    '            Print #1, Spc(4); Trim(Left(Grid1.TextMatrix(i, 0), 35)); Spc(35 - Len(Trim(Left(Grid1.TextMatrix(i, 0), 35))));
    '            Else
    '            Print #1,
    '            Ln_No = Ln_No + 1
    '            Print #1, Spc(1); Chr(27); "E"; Trim(Grid1.TextMatrix(i, 0)); Spc(38 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '            End If
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2);
    '        If Left$(Trim(Grid1.TextMatrix(i, 0)), 1) <> "-" Then Print #1, Chr(27); "F" Else Print #1,
    '            Ln_No = Ln_No + 1
    '        Next i
    '        i = i + 1
    '    Print #1, String(80, 45)
    '    Print #1, Spc(4); Trim(Grid1.TextMatrix(i, 0)); Spc(35 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '    Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '    Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '    Print #1, String(80, 45)
    '        Ln_No = Ln_No + 3
    '        For k = Ln_No + 1 To 72
    '        Print #1, ""
    '        Next k

    '        Exit Sub

    'Page_Header:
    '        Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '        Print #1, Chr(27); "M"; Spc(48 - (Len(Trim(Cmp_Address)) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '        Print #1,
    '        Pg_No = Pg_No + 1
    '        Print #1, Spc(31); Chr(27); "E"; "GROUP TRIAL BALANCE"; Chr(27); "F"; Spc(30) 'Spc(40 - (Len(Trim("Group Ledger : " & RptDet_Name1)) / 2));  Trim("GROUP LEDGER : " & RptDet_Name1);
    '        Print #1, Spc(40 - (Len("AS ON " & Trim(Format(RptDet_Date1, "dd/mm/yyyy"))) / 2)); Chr(27); "E"; "AS ON " & Trim(Format(RptDet_Date1, "dd/mm/yyyy")); Chr(27); "F"
    '        Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        Print #1, String(80, 45)
    '        Print #1, "  PARTICULARS                                         DEBIT              CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = 9
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 2
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '        Return
    '    End Sub

    '    Private Sub Print_FinalTB()
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '        Dim rng As String

    '    GoSub Page_Header
    '        For i = 2 To Grid1.Rows - 3
    '        If Ln_No > 61 Then GoSub Page_Footer
    '            If Left$(Trim(Grid1.TextMatrix(i, 0)), 1) = "-" Then
    '            Print #1, Spc(4); Trim(Grid1.TextMatrix(i, 0)); Spc(35 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '            Else
    '            Print #1,
    '            Ln_No = Ln_No + 1
    '            Print #1, Spc(1); Chr(27); "E"; Trim(Grid1.TextMatrix(i, 0)); Spc(38 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '            End If
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '        Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2);
    '        If Left$(Trim(Grid1.TextMatrix(i, 0)), 1) <> "-" Then Print #1, Chr(27); "F" Else Print #1,
    '            Ln_No = Ln_No + 1
    '        Next i
    '        i = i + 1
    '    Print #1, String(80, 45)
    '    Print #1, Spc(4); Trim(Grid1.TextMatrix(i, 0)); Spc(35 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '    Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 1))); Grid1.TextMatrix(i, 1);
    '    Print #1, Spc(20 - Len(Grid1.TextMatrix(i, 2))); Grid1.TextMatrix(i, 2)
    '    Print #1, String(80, 45)
    '        Ln_No = Ln_No + 3
    '        For k = Ln_No + 1 To 72
    '        Print #1, ""
    '        Next k

    '        Exit Sub

    'Page_Header:
    '        Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '        Print #1, Chr(27); "M"; Spc(48 - (Len(Trim(Cmp_Address)) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '        Print #1,
    '        Pg_No = Pg_No + 1
    '        Print #1, Spc(31); Chr(27); "E"; "FINAL TRIAL BALANCE"; Chr(27); "F"; Spc(30) 'Spc(40 - (Len(Trim("Group Ledger : " & RptDet_Name1)) / 2));  Trim("GROUP LEDGER : " & RptDet_Name1);
    '        rng = Trim(MonthName(Month(Cmp_FromDt), True) & "'" & Trim(Year(Cmp_FromDt)) & " - " & MonthName(Month(Cmp_ToDt), True) & "'" & Trim(Year(Cmp_ToDt)))
    '        Print #1, Spc(40 - (Len(rng) / 2)); Chr(27); "E"; rng; Chr(27); "F"
    '        Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        Print #1, String(80, 45)
    '        Print #1, "  PARTICULARS                                         DEBIT              CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = 9
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 2
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '        Return
    '    End Sub

    '    Public Sub Print_AllLedger()
    '        Dim Rs1 As ADODB.Recordset, Rs2 As ADODB.Recordset
    '        Dim Tot1 As Currency, Tot2 As Currency
    '        Dim i As Integer, Pg1 As Integer, Ln As Integer, k As Integer, prv_pg As Integer
    '        Dim v1 As String, v2 As String, V3 As String, v4 As String, nar As String
    '        Dim PageSep As Boolean

    '        If MsgBox("Do you Print Each Ledger in Seperate Page", vbYesNo + vbQuestion, "Accept [Y]es / [N]o") = vbYes Then PageSep = True Else PageSep = False

    '        Pg1 = 1 : Ln = 0

    '        Rs2 = New ADODB.Recordset
    '        Rs1 = New ADODB.Recordset
    '        With Rs1

    '            .Open("Select * from company_head where company_idno = " & Str(RptDet.Idno1), Con, adOpenStatic, adLockReadOnly)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Cmp_Name = !Company_Name
    '                Cmp_Address = !Company_Address1 & " " & !Company_Address2 & " " & !Company_Address3 & " " & !Company_Address4
    '            End If
    '            .Close()

    '            .Open("Select a.int2 as page_no, b.ledger_name, b.ledger_idno, b.Parent_Code, ( case when b.ledger_address4 <> '' then b.ledger_address4 when b.ledger_address3 <> '' then b.ledger_address3 when b.ledger_address2 <> '' then b.ledger_address2 else b.ledger_address1 end ) as ledger_address from ledger_head b, reporttempsub a where b.ledger_idno = a.int1 order by b.ledger_name", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF

    '                    Mdi1.StatusBar3.Panels(2).Text = !ledger_name
    '                    If !Page_No > 0 Then Pg1 = !Page_No
    '                    Tot1 = 0 : Tot2 = 0

    '                    With Rs2

    '                        If Not (Rs1!Parent_Code Like "*~18~") Then
    '                            .Open("Select sum(voucher_amount) from voucher_details where ledger_idno = " & Str(Rs1!Ledger_IdNo) & " and company_idno = " & Str(RptDet.Idno1) & " and voucher_date < '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "'", cn1, adOpenForwardOnly, adLockReadOnly)
    '                            If Rs2(0).Value <> "" Then If Val(Rs2(0).Value) > 0 Then Tot1 = Val(Rs2(0).Value) Else Tot2 = Abs(Val(Rs2(0).Value))
    '                            .Close()
    '                        End If
    '                        .Open("Select a.voucher_date, a.voucher_amount, b.voucher_no, b.voucher_type, c.ledger_name as creditor_name, d.ledger_name as debtor_name, a.narration from voucher_details a, voucher_head b, ledger_head c, ledger_head d where a.ledger_idno = " & Str(Rs1!Ledger_IdNo) & " and a.company_idno = " & Str(RptDet.Idno1) & " and a.voucher_date between '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and '" & Trim(Format(Cmp_ToDt, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and a.company_idno = b.company_idno and b.creditor_idno = c.ledger_idno and b.debtor_idno = d.ledger_idno order by a.voucher_date, b.for_orderby", cn1, adOpenForwardOnly, adLockReadOnly)
    '                        If Tot1 <> 0 Or Tot2 <> 0 Or Not (.BOF And .EOF) Then

    '                            Print #1, Chr(14); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"; Chr(20)
    '                            Print #1, Cmp_Address
    '                            Print #1,
    '                            Print #1, Chr(27); "E"; "Ledger Of "; Trim(Rs1!ledger_name); Chr(27); "F"
    '                            If Rs1!ledger_address <> "" Then
    '                                Print #1, Spc(10); Rs1!ledger_address
    '                                Ln = Ln + 1
    '                            End If
    '                            Print #1,

    '                            cn1.Execute("update reporttempsub set int3 = " & Str(Pg1) & " where int1 = " & Str(Rs1!Ledger_IdNo))

    '                            Print #1, "For The Period From "; Trim(Cmp_FromDt); " To "; Trim(Cmp_ToDt); Spc(26 - Len(Trim(Pg1))); IIf(prv_pg <> Pg1, "PAGE NO : " & Trim(Pg1), "")
    '                            Print #1, String(80, "-")
    '                            Print #1, Spc(1); "DATE"; Spc(2); Spc(13); "PARTICULARS"; Spc(14); Spc(13); "DEBIT"; Spc(10); "CREDIT"
    '                            Print #1, String(80, "-")
    '                            If Tot1 = 0 Then v1 = "" Else v1 = PROC.Currency_Format(Tot1)
    '                            If Tot2 = 0 Then v2 = "" Else v2 = PROC.Currency_Format(Tot2)
    '                            Ln = Ln + 9
    '                            If Tot1 > 0 Or Tot2 > 0 Then
    '                                Print #1, Spc(14); "OPENING BALANCE"; Spc(19); Spc(16 - Len(v2)); v2; Spc(16 - Len(v1)); v1
    '                                Print #1,
    '                                Ln = Ln + 2
    '                            End If
    '                            If Not (.BOF And .EOF) Then
    '                                .MoveFirst()
    '                                Do While Not .EOF
    '                                    If Ln > 61 Then
    '                                        If Tot1 = 0 Then v1 = "" Else v1 = PROC.Currency_Format(Tot1)
    '                                        If Tot2 = 0 Then v2 = "" Else v2 = PROC.Currency_Format(Tot2)
    '                                        Print #1, String(80, "-")
    '                                        Print #1, Spc(14); "C/O"; Spc(31); Chr(27); "E"; Spc(16 - Len(v2)); v2; Spc(16 - Len(v1)); v1; Chr(27); "F"
    '                                        Print #1, String(80, "-")
    '                                        Print #1, Spc(72); "Contd..."
    '                                        Ln = Ln + 4
    '                                        For i = Ln + 1 To 72
    '                                            Print #1,
    '                                        Next i
    '                                        Print #1, Chr(14); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"; Chr(20)
    '                                        Print #1, Cmp_Address
    '                                        Print #1,
    '                                        Print #1, Chr(27); "E"; "Ledger Of "; Trim(Rs1!ledger_name); Chr(27); "F"
    '                                        Print #1,
    '                                        Pg1 = Pg1 + 1
    '                                        v1 = PROC.Currency_Format(Tot1)
    '                                        v2 = PROC.Currency_Format(Tot2)
    '                                        Print #1, "For The Period From "; Trim(Cmp_FromDt); " To "; Trim(Cmp_ToDt); Spc(26 - Len(Trim(Pg1))); IIf(prv_pg <> Pg1, "PAGE NO : " & Trim(Pg1), "")
    '                                        Print #1, String(80, "-")
    '                                        Print #1, Spc(1); "DATE"; Spc(2); Spc(13); "PARTICULARS"; Spc(14); Spc(13); "DEBIT"; Spc(10); "CREDIT"
    '                                        Print #1, String(80, "-")
    '                                        Print #1, Spc(14); "B/F"; Spc(31); Spc(16 - Len(v2)); Chr(27); "E"; v2; Spc(16 - Len(v1)); v1; Chr(27); "F"
    '                                        Print #1,
    '                                        Ln = 11
    '                                    End If
    '                                    V3 = "" : v4 = ""
    '                                    If !Voucher_Amount < 0 Then
    '                                        v2 = "To " & Left$(!creditor_name, 35)
    '                                        v4 = PROC.Currency_Format(Abs(!Voucher_Amount))
    '                                        Tot2 = Tot2 + Abs(!Voucher_Amount)
    '                                    Else
    '                                        v2 = "By " & Left$(!Debtor_Name, 35)
    '                                        V3 = PROC.Currency_Format(!Voucher_Amount)
    '                                        Tot1 = Tot1 + !Voucher_Amount
    '                                    End If
    '                                    v1 = Format(!Voucher_Date, "dd-mm-yy")
    '                                    Print #1, Trim(v1); Spc(11 - Len(Trim(v1))); Trim(v2); Spc(37 - Len(Trim(v2))); Spc(16 - Len(Trim(v4))); Trim(v4); Spc(16 - Len(Trim(V3))); Trim(V3)
    '                                    If !Narration <> "" Then
    '                                        nar = Trim(!Narration)
    '                                        Do While Len(nar) > 40
    '                                            For k = 40 To 1 Step -1
    '                                                If Mid$(nar, k, 1) = " " Or Mid$(nar, k, 1) = "," Or Mid$(nar, k, 1) = ")" Then Exit For
    '                                            Next k
    '                                            If k = 0 Then k = 40
    '                                            Print #1, Spc(11);
    '                                            Print #1, "   "; Chr(27); "M"; Trim(Left$(nar, k)); Chr(27); "P"
    '                                            Ln = Ln + 1
    '                                            nar = Right(nar, Len(nar) - k)
    '                                        Loop
    '                                        Print #1, Spc(11);
    '                                        Print #1, "   "; Chr(27); "M"; Trim(nar); Chr(27); "P"
    '                                        Ln = Ln + 1
    '                                    End If
    '                                    Print #1,
    '                                    Ln = Ln + 2
    '                                    .MoveNext()
    '                                Loop
    '                            End If
    '                            Print #1, String(80, "-")
    '                            If Tot1 = 0 Then v1 = "" Else v1 = PROC.Currency_Format(Tot1)
    '                            If Tot2 = 0 Then v2 = "" Else v2 = PROC.Currency_Format(Tot2)
    '                            Print #1, Spc(14); "TOTAL"; Spc(29); Spc(16 - Len(v2)); v2; Spc(16 - Len(v1)); v1
    '                            Print #1, String(80, "-")
    '                            Ln = Ln + 3
    '                            Tot1 = Tot1 - Tot2
    '                            v1 = "" : v2 = ""
    '                            If Tot1 >= 0 Then v1 = PROC.Currency_Format(Tot1) Else v2 = PROC.Currency_Format(Abs(Tot1))
    '                            Print #1, Spc(14); "CLOSING BALANCE"; Spc(19); Spc(16 - Len(v2)); Chr(27); "E"; v2; Spc(16 - Len(v1)); v1; Chr(27); "F"
    '                            Print #1, String(80, "-")
    '                            Ln = Ln + 2
    '                            prv_pg = Pg1
    '                            If PageSep = True Or (Ln > 50 And PageSep = False) Then
    '                                For i = Ln + 1 To 72
    '                                    Print #1,
    '                                Next i
    '                                Ln = 0
    '                                Pg1 = Pg1 + 1
    '                            Else
    '                                Print #1,: Print #1,: Print #1,
    '                                Ln = Ln + 3
    '                            End If
    '                        End If
    '                        .Close()
    '                    End With
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rs1 = Nothing
    '        Rs2 = Nothing

    '        If MsgBox("Do you print the ledger with page index", vbYesNo + vbQuestion, "Accept [Y]es / [N]o") = vbYes Then
    '            If PageSep = True Then nar = Pg1 - 1 Else nar = Pg1
    '            Pg1 = 1 : i = 0
    '        Print #1, Spc(20); Chr(14); Chr(27); "E"; "PAGE INDEX"; Chr(27); "F"; Chr(20)
    '        Print #1, Spc(49 - Len(Trim(Pg1))); "PAGE NO : " & Trim(Pg1)
    '        Print #1, String(59, "-")
    '        Print #1, Spc(2); "S.NO"; Spc(15); "PARTY NAME"; Spc(20); "PAGE NO"
    '        Print #1, String(59, "-")
    '            Ln = 5
    '            Rs1 = New ADODB.Recordset
    '            Rs1.Open("Select a.int3 as page_no, b.ledger_name from ledger_head b, reporttempsub a where a.int3 > 0 and b.ledger_idno = a.int1 order by b.ledger_name", cn1, adOpenDynamic, adLockOptimistic)
    '            If Not (Rs1.BOF And Rs1.EOF) Then
    '                Rs1.MoveFirst()
    '                Do While Not Rs1.EOF
    '                    If Ln > 63 Then
    '                        Print #1, String(59, "-")
    '                        Print #1, Spc(51); "Contd..."
    '                        Ln = Ln + 2
    '                        For k = Ln + 1 To 72
    '                            Print #1,
    '                        Next k
    '                        Pg1 = Pg1 + 1
    '                        Print #1, Spc(20); Chr(14); Chr(27); "E"; "PAGE INDEX"; Chr(27); "F"; Chr(20)
    '                        Print #1, Spc(49 - Len(Trim(Pg1))); "PAGE NO : " & Trim(Pg1)
    '                        Print #1, String(59, "-")
    '                        Print #1, Spc(2); "S.NO"; Spc(15); "PARTY NAME"; Spc(20); "PAGE NO"
    '                        Print #1, String(59, "-")
    '                        Ln = 5
    '                    End If
    '                    i = i + 1
    '                    Print #1, Spc(1); Str(i); Spc(8 - Len(Str(i))); Rs1!ledger_name; Spc(44 - Len(Rs1!ledger_name)); Spc(5 - Len(Trim(Rs1!Page_No))); Trim(Rs1!Page_No)
    '                    Ln = Ln + 1
    '                    Rs1.MoveNext()
    '                Loop
    '            End If
    '            Rs1 = Nothing
    '        Print #1, String(59, "-")
    '        Print #1, Spc(21); "LAST PAGE : "; Trim(nar)
    '        Print #1, String(59, "-")
    '            Ln = Ln + 3
    '            For k = Ln + 1 To 72
    '            Print #1,
    '            Next k
    '        End If
    '    End Sub

    '    Public Sub Common_Printing(Optional StyleNo As Integer)
    '        Dim Cd1 As String
    '        Dim i As Integer, J As Integer, Left_Margin As Integer, k As Integer
    '        Dim LineNo As Integer, PageNo As Integer, TotSpc As Integer

    '    Print #1, Chr(15); Chr(18);
    '    Print #1, Spc(44 - (Len(Cmp_Name) / 2)); Chr(27); "E"; Cmp_Name; Chr(27); "F"
    '        LineNo = 1
    '    If Cmp_Address <> "" Then Print #1, Spc(44 - (Len(Cmp_Address) / 2)); Cmp_Address: LineNo = LineNo + 1
    '    Print #1,
    '    LineNo = LineNo + 1
    '    If RptHeading1 <> "" Then Print #1, Spc(44 - (Len(RptHeading1) / 2)); Chr(27); "E"; UCase(RptHeading1); Chr(27); "F": LineNo = LineNo + 1
    '    If RptHeading2 <> "" Then Print #1, Spc(44 - (Len(RptHeading2) / 2)); UCase(RptHeading2): LineNo = LineNo + 1
    '    If RptHeading3 <> "" Then Print #1, Spc(44 - (Len(RptHeading3) / 2)); UCase(RptHeading3): LineNo = LineNo + 1
    '    Print #1,
    '    LineNo = LineNo + 1
    '        PageNo = 1

    '        Select Case StyleNo

    '            Case 1

    '                For i = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(i) > 300 Then TotSpc = TotSpc + Grid1.ColData(i) + 1
    '                Next i
    '                TotSpc = TotSpc + 1
    '                If TotSpc < 80 Then Left_Margin = Int((84 - TotSpc) / 2) Else Left_Margin = 0
    '        If TotSpc > 84 Then Print #1, Chr(15); Else Print #1, Chr(15); Chr(18);
    '        Print #1, Spc(Left_Margin); Spc(TotSpc - 10 - Len(Trim(Str(PageNo)))); "Page No : "; Trim(Str(PageNo))

    '        Print #1, Spc(Left_Margin); "";
    '                For J = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(218); Else Print #1, Chr(194);
    '            If Grid1.ColWidth(J) > 300 Then Print #1, String(Grid1.ColData(J), 196);
    '                Next J
    '        Print #1, Chr(191)

    '        Print #1, Spc(Left_Margin); Chr(179);
    '                For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then
    '                If Grid1.ColAlignment(J) < 6 Then Print #1, Grid1.TextMatrix(0, J); Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Chr(179); Else Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Grid1.TextMatrix(0, J); Chr(179);
    '                    End If
    '                Next J
    '        Print #1,

    '        Print #1, Spc(Left_Margin); "";
    '                For J = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(195); Else Print #1, Chr(197);
    '            If Grid1.ColWidth(J) > 300 Then Print #1, String(Grid1.ColData(J), 196);
    '                Next J
    '        Print #1, Chr(180)

    '                For i = 2 To Grid1.Rows - 1
    '                    If i Mod 54 = 0 Then

    '                Print #1, Spc(Left_Margin); "";
    '                        For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(192); Else Print #1, Chr(193);
    '                    If Grid1.ColWidth(J) > 300 Then Print #1, String(Grid1.ColData(J), 196);
    '                        Next J
    '                Print #1, Chr(217)
    '                Print #1, Spc(Left_Margin); Spc(TotSpc - 8); "Contd..."
    '                Print #1, Chr(12)

    '                Print #1, Chr(15); Chr(18);
    '                Print #1, Spc(44 - (Len(Cmp_Name) / 2)); Chr(27); "E"; Cmp_Name; Chr(27); "F"
    '                        LineNo = 3
    '                If Cmp_Address <> "" Then Print #1, Spc(44 - (Len(Cmp_Address) / 2)); Cmp_Address: LineNo = LineNo + 1
    '                Print #1,
    '                If RptHeading1 <> "" Then Print #1, Spc(44 - (Len(RptHeading1) / 2)); Chr(27); "E"; UCase(RptHeading1); Chr(27); "F": LineNo = LineNo + 1
    '                If RptHeading2 <> "" Then Print #1, Spc(44 - (Len(RptHeading2) / 2)); UCase(RptHeading2): LineNo = LineNo + 1
    '                If RptHeading3 <> "" Then Print #1, Spc(44 - (Len(RptHeading3) / 2)); UCase(RptHeading3): LineNo = LineNo + 1
    '                Print #1,

    '                If TotSpc > 84 Then Print #1, Chr(15); Else Print #1, Chr(15); Chr(18);
    '                        PageNo = PageNo + 1
    '                Print #1, Spc(Left_Margin); Spc(TotSpc - 10 - Len(Trim(Str(PageNo)))); "Page No : "; Trim(Str(PageNo))

    '                Print #1, Spc(Left_Margin); "";
    '                        For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(218); Else Print #1, Chr(194);
    '                    If Grid1.ColWidth(J) > 300 Then Print #1, String(Grid1.ColData(J), 196);
    '                        Next J
    '                Print #1, Chr(191)

    '                Print #1, Spc(Left_Margin); Chr(179);
    '                        For J = 0 To Grid1.Cols - 1
    '                            If Grid1.ColWidth(J) > 300 Then
    '                        If Grid1.ColAlignment(J) < 6 Then Print #1, Grid1.TextMatrix(0, J); Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Chr(179); Else Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Grid1.TextMatrix(0, J); Chr(179);
    '                            End If
    '                        Next J
    '                Print #1,

    '                Print #1, Spc(Left_Margin); "";
    '                        For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(195); Else Print #1, Chr(197);
    '                    If Grid1.ColWidth(J) > 300 Then Print #1, String(Grid1.ColData(J), 196);
    '                        Next J
    '                Print #1, Chr(180)

    '                    End If

    '                    If Grid1.RowData(i) = "1" Then
    '                Print #1, Spc(Left_Margin); "";
    '                        For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(195); Else Print #1, Chr(197);
    '                    If Grid1.ColWidth(J) > 300 Then Print #1, String(Grid1.ColData(J), 196);
    '                        Next J
    '                Print #1, Chr(180)
    '                    End If
    '            Print #1, Spc(Left_Margin); Chr(179);
    '                    For J = 0 To Grid1.Cols - 1
    '                        If Grid1.ColWidth(J) > 300 Then
    '                            If Grid1.ColAlignment(J) < 6 Then
    '                                Cd1 = Trim(Left$(Grid1.TextMatrix(i, J), (Val(Grid1.ColData(J)))))
    '                        Print #1, Cd1; Spc(Val(Grid1.ColData(J)) - Len(Cd1)); Chr(179);
    '                            Else
    '                        Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(i, J))); Left$(Grid1.TextMatrix(i, J), Val(Grid1.ColData(J))); Chr(179);
    '                            End If
    '                        End If
    '                    Next J
    '            Print #1,
    '                Next i

    '        Print #1, Spc(Left_Margin); "";
    '                For J = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(192); Else Print #1, Chr(193);
    '            If Grid1.ColWidth(J) > 300 Then Print #1, String(Grid1.ColData(J), 196);
    '                Next J
    '        Print #1, Chr(217)

    '            Case Else

    '                For i = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(i) > 300 Then
    '                        TotSpc = TotSpc + Grid1.ColData(i)
    '                        If i <> Grid1.Cols - 1 And Grid1.ColAlignment(i) > 5 Then TotSpc = TotSpc + 2
    '                    End If
    '                Next i

    '        If TotSpc > 84 Then Print #1, Chr(15); Else Print #1, Chr(15); Chr(18);
    '        Print #1, Spc(TotSpc - 10 - Len(Trim(Str(PageNo)))); "Page No : "; Trim(Str(PageNo))
    '                LineNo = LineNo + 1
    '        Print #1, String(TotSpc, "-")
    '        Print #1, "";
    '                For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then
    '                If Grid1.ColAlignment(J) < 6 Then Print #1, Grid1.TextMatrix(0, J); Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Else Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Grid1.TextMatrix(0, J); Spc(IIf(J <> Grid1.Cols - 1, 2, 0));
    '                    End If
    '                Next J
    '        Print #1,
    '        Print #1, String(TotSpc, "-")
    '                LineNo = LineNo + 3
    '                For i = 2 To Grid1.Rows - 1

    '                    If LineNo >= 60 Then

    '                Print #1, String(TotSpc, "-")
    '                Print #1, Spc(TotSpc - 8); "Contd..."
    '                        For k = LineNo + 3 To 72
    '                    Print #1, 'Chr(12)
    '                        Next k

    '                Print #1, Chr(15); Chr(18);
    '                Print #1, Spc(44 - (Len(Cmp_Name) / 2)); Chr(27); "E"; Cmp_Name; Chr(27); "F"
    '                        LineNo = 1
    '                If Cmp_Address <> "" Then Print #1, Spc(44 - (Len(Cmp_Address) / 2)); Cmp_Address: LineNo = LineNo + 1
    '                Print #1,
    '                LineNo = LineNo + 1
    '                If RptHeading1 <> "" Then Print #1, Spc(44 - (Len(RptHeading1) / 2)); Chr(27); "E"; UCase(RptHeading1); Chr(27); "F": LineNo = LineNo + 1
    '                If RptHeading2 <> "" Then Print #1, Spc(44 - (Len(RptHeading2) / 2)); UCase(RptHeading2): LineNo = LineNo + 1
    '                If RptHeading3 <> "" Then Print #1, Spc(44 - (Len(RptHeading3) / 2)); UCase(RptHeading3): LineNo = LineNo + 1
    '                Print #1,
    '                LineNo = LineNo + 1
    '                If TotSpc > 84 Then Print #1, Chr(15); Else Print #1, Chr(15); Chr(18);
    '                        PageNo = PageNo + 1
    '                Print #1, Spc(TotSpc - 10 - Len(Trim(Str(PageNo)))); "Page No : "; Trim(Str(PageNo))
    '                        LineNo = LineNo + 1
    '                Print #1, String(TotSpc, "-")
    '                Print #1, "";
    '                        For J = 0 To Grid1.Cols - 1
    '                            If Grid1.ColWidth(J) > 300 Then
    '                        If Grid1.ColAlignment(J) < 6 Then Print #1, Grid1.TextMatrix(0, J); Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Else Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Grid1.TextMatrix(0, J); Spc(IIf(J <> Grid1.Cols - 1, 2, 0));
    '                            End If
    '                        Next J
    '                Print #1,
    '                Print #1, String(TotSpc, "-")
    '                        LineNo = LineNo + 3
    '                    End If

    '                    If Grid1.RowData(i) = 1 Or Grid1.RowData(i) = 3 Then
    '                Print #1, String(TotSpc, "-")
    '                        LineNo = LineNo + 1
    '                    End If
    '            Print #1, "";
    '                    For J = 0 To Grid1.Cols - 1
    '                        If Grid1.ColWidth(J) > 300 Then
    '                            If Grid1.ColAlignment(J) < 6 Then
    '                                Cd1 = Trim(Left$(Grid1.TextMatrix(i, J), (Val(Grid1.ColData(J)) - 2)))
    '                        Print #1, Cd1; Spc(Val(Grid1.ColData(J)) - Len(Cd1));
    '                            Else
    '                        Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(i, J))); Left$(Grid1.TextMatrix(i, J), Val(Grid1.ColData(J))); Spc(IIf(J <> Grid1.Cols - 1, 2, 0));
    '                            End If
    '                        End If
    '                    Next J
    '            Print #1,
    '            LineNo = LineNo + 1
    '                    If Grid1.RowData(i) = 2 Or Grid1.RowData(i) = 3 Then
    '                Print #1, String(TotSpc, "-")
    '                        LineNo = LineNo + 1
    '                    End If
    '                Next i

    '        End Select

    '    Print #1, Chr(18)

    '    End Sub

    '    Private Sub EntryList_MonthWise()
    '        Dim Rs1 As Recordset
    '        Dim Tot As Integer
    '        Dim vtp As String

    '        Grid1.Cols = 2
    '        Grid1.FormatString = "<MONTH NAME           |>NO.OF VOUCHERS "
    '        Grid1.ColWidth(0) = 2000 : Grid1.ColWidth(1) = 2000

    '        vtp = PROC.Get_FieldValue(cn1, "voucher_type_head", "voucher_type_short_name", "voucher_type_idno = " & Str(RptDet_IdNo1))
    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select month(voucher_date) as month_name, count(*) as noof_vouchers from voucher_head where voucher_date between '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and '" & Trim(Format(Cmp_ToDt, "mm/dd/yyyy")) & "' and voucher_type = '" & Trim(vtp) & "' group by month(voucher_date), year(voucher_date) order by year(voucher_date),month(voucher_date)", cn1, adOpenStatic, adLockReadOnly)
    '        With Rs1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    Grid1.AddItem(MonthName(Rs1!Month_Name) & vbTab & Rs1!noof_vouchers)
    '                    Tot = Tot + Rs1!noof_vouchers
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rs1 = Nothing
    '        Grid1.AddItem("")
    '        Grid1.AddItem("TOTAL" & vbTab & Tot)
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '    End Sub

    '    Private Sub EntryList_Detail()
    '        Dim Rs1 As Recordset
    '        Dim vtp As String

    '        Grid1.Cols = 6
    '        Grid1.FormatString = "<S.NO  |<DATE         |<VOU.NO    |<CREDITOR NAME                 |<DEBTOR NAME                   |>AMOUNT      "

    '        vtp = PROC.Get_FieldValue(cn1, "voucher_type_head", "voucher_type_short_name", "voucher_type_idno = " & Str(RptDet_IdNo1))
    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select a.*, b.ledger_name as cre_name, c.ledger_name as deb_name, d.voucher_amount from voucher_head a, ledger_head b, ledger_head c, voucher_details d where month(a.voucher_date) = " & Str(RptDet_IdNo2) & " And Year(a.voucher_date) = " & Str(IIf(RptDet_IdNo2 > 3, Year(Cmp_FromDt), Year(Cmp_ToDt))) & " and voucher_type = '" & Trim(vtp) & "' and a.creditor_idno = b.ledger_idno and a.debtor_idno = c.ledger_idno and a.voucher_ref_no=d.voucher_ref_no and d.sl_no = 1 order by a.voucher_date", cn1, adOpenStatic, adLockReadOnly)
    '        With Rs1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    Grid1.AddItem(Grid1.Rows - 1 & vbTab & Format(Rs1!Voucher_Date, "dd-mm-yy") & vbTab & Rs1!Voucher_No & vbTab & Rs1!cre_name & vbTab & Rs1!deb_name & vbTab & Format(Abs(Rs1!Voucher_Amount), "########0.00"))
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rs1 = Nothing
    '    End Sub

    '    Private Sub Report_Intialize(ByVal Cn2 As ADODB.Connection)
    '        Dim i As Integer, J As Integer
    '        Dim Rs As ADODB.Recordset

    '        Heading_1 = "" : Heading_2 = "" : Field_1 = "" : Field_2 = "" : Format_1 = "" : Format_2 = ""
    '        If Trim(Report_PKey) <> "" Then
    '            Rs = New ADODB.Recordset
    '            Rs.Open("Select * from Report_Inputs_Head where pkey in ( " & Trim(Report_PKey) & " ) order by for_orderby", Cn2, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    Field_2 = Field_2 & IIf(Rs!Input_Type = "C", "t" & Trim(Rs!PKey) & ".", "") & Trim(Rs!Selection_field_name) & ","
    '                    Heading_2 = Heading_2 & "<[LEN" & Trim(Rs!Field_Length) & "]" & Rs!Display & "|"
    '                    Format_2 = Format_2 & "|"
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs = Nothing
    '        End If

    '        Condt = Trim(CompType_Condt)
    '        Rpt_Hd = UCase(RptDet.RptCode_Main) & " - "
    '        For i = 0 To 4
    '            If RptInp(i).PKey = "I" Then
    '                RptDet_Tex_Val1 = Replace(RptInp(i).Value, "('", "")
    '                RptDet_Tex_Val1 = Replace(RptDet_Tex_Val1, "')", "")
    '            ElseIf (RptInp(i).Value <> "" Or RptInp(i).Total > 0) And RptInp(i).PKey <> "I" Then
    '                If RptInp(i).Value <> "" Then
    '                    Condt = Condt & IIf(Condt <> "", " and ", "") & IIf(RptInp(i).Input_Type = "C", "t" & Trim(RptInp(i).PKey) & ".", "") & IIf(Trim(RptInp(i).Return_Field) <> "", Trim(RptInp(i).Return_Field), Trim(RptInp(i).Selection_Field)) & " in " & IIf(RptInp(i).Input_Type = "C", RptInp(i).Value, Replace(RptInp(i).Value, ",", "','"))
    '                    If InStr(RptInp(i).Value, ",") = 0 Then Rpt_Hd = Rpt_Hd & RptInp(i).Report_Display & " : " & RptInp(i).Caption & " - "
    '                    If i = 0 Then Rpt_Hd = Rpt_Hd & "|"
    '                End If
    '                If RptInp(i).Total > 0 Then
    '                    Field_1 = Field_1 & IIf(RptInp(i).Input_Type = "C", "t" & Trim(RptInp(i).PKey) & ".", "") & Trim(RptInp(i).Selection_Field) & ","
    '                    Heading_1 = Heading_1 & "<" & IIf(Trim(Heading_1) = "", "[GP=]", "=") & "[LEN" & Trim(RptInp(i).Field_Length) & "]" & IIf(InStr(RptInp(i).Value, ",") = 0 And Trim(RptInp(i).Value) <> "", "[HIDDEN]", "") & IIf(RptInp(i).Total > 0, "[SUB_GP]", "") & RptInp(i).Report_Display & "|"
    '                    Format_1 = Format_1 & "|"
    '                    Field_2 = Replace(Field_2, IIf(RptInp(i).Input_Type = "C", "t" & Trim(RptInp(i).PKey) & ".", "") & Trim(RptInp(i).Selection_Field) & ",", "")
    '                    Heading_2 = Replace(Heading_2, "<[LEN" & Trim(RptInp(i).Field_Length) & "]" & RptInp(i).Report_Display & "|", "")
    '                    Format_2 = Replace(Format_2, "|", "", , 1)
    '                Else
    '                    If (InStr(RptInp(i).Value, ",")) = 0 Then
    '                        J = InStr(Heading_2, RptInp(i).Report_Display)
    '                        If J > 0 Then Heading_2 = Left(Heading_2, J - 1) & "[HIDDEN]" & Right(Heading_2, Len(Heading_2) - J + 1)
    '                    End If
    '                End If
    '                'If RptDet.RptCode_Sub = "Accounts" Then RptInp(i).Value = Replace(RptInp(i).Value, "(", ""): RptInp(i).Value = Replace(RptInp(i).Value, ")", "")
    '            End If
    '        Next i
    '        If InStr(RptDet.Inputs, "2") > 0 Then
    '            Rpt_Hd = Rpt_Hd & "RANGE : " & Trim(Format(RptDet.Date1, "dd/mm/yyyy")) & " TO " & Trim(Format(RptDet.Date2, "dd/mm/yyyy"))
    '        ElseIf InStr(RptDet.Inputs, "1") > 0 Then
    '            Rpt_Hd = Rpt_Hd & "AS ON : " & Trim(Format(RptDet.Date1, "dd/mm/yyyy"))
    '        End If

    '    End Sub

    '    Private Sub Accounts_GroupLedger_Details()
    '        Dim tt1 As Currency, op_tt As Currency, cr_tt As Currency, db_tt As Currency, cr_tt1 As Currency, db_tt1 As Currency
    '        Dim RT2 As Recordset, Rt1 As Recordset
    '        Dim Rw As Integer
    '        Dim GpCd As String
    '        Dim dt_cndt As String

    '        Grid1.Cols = 5
    '        Grid1.FormatString = "<PARTY NAME                                          |>OPENING                    |>DEBIT                        |>CREDIT                      |>CLOSING                        "
    '        FrmNm.Label1(0).Caption = "GROUP LEDGER - NAME : " & Trim(RptDet_Name1) & " - RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '        RptHeading1 = "GROUP LEDGER - NAME : " & Trim(RptDet_Name1)
    '        RptHeading2 = "RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '        Grid1.SelectionMode = 1

    '        Rt1 = New ADODB.Recordset
    '        With Rt1
    '            '    .Open "select parent_idno from group_head tG " & IIf(Condt <> "", " where ", "") & Condt, Cn1, adOpenStatic, adLockReadOnly
    '            '    If Not (.BOF And .EOF) Then
    '            '        .MoveFirst
    '            '        GpCd = !Parent_Idno
    '            '    End If
    '            '    .Close
    '            '    If GpCd Like "*~18~" Then dt_cndt = "1 = 2" Else dt_cndt = "voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "'"

    '            cn1.Execute("delete from reporttemp")
    '            'Cn1.Execute "insert into reporttemp ( int1, currency1 ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head c where " & dt_cndt & " and a.ledger_idno = b.ledger_idno and b.parent_code = c.parent_idno group by a.ledger_idno having sum(voucher_amount) <> 0"

    '            cn1.Execute("insert into reporttemp ( int1, currency1 ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head c where ( c.parent_idno like '%~18~' and voucher_date between '" & Trim(Format(Cmp_FromDt, "mm/dd/yyyy")) & "' and '" & Trim(Format(DateAdd("d", -1, RptDet_Date1), "mm/dd/yyyy")) & "' ) or ( c.parent_idno not like '%~18~' and voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' ) and a.ledger_idno = b.ledger_idno and b.parent_code = c.parent_idno group by a.ledger_idno having sum(voucher_amount) <> 0")
    '            cn1.Execute("insert into reporttemp ( int1, currency2 ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head c where voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and voucher_amount > 0 and a.ledger_idno = b.ledger_idno and b.parent_code = c.parent_idno group by a.ledger_idno having sum(voucher_amount) <> 0")
    '            cn1.Execute("insert into reporttemp ( int1, currency3 ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b, group_head c where voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and voucher_amount < 0 and a.ledger_idno = b.ledger_idno and b.parent_code = c.parent_idno group by a.ledger_idno having sum(voucher_amount) <> 0")

    '            .Open("select group_name, group_idno, parent_idno from group_head tG " & IIf(Condt <> "", " where ", "") & Condt & " order by order_position", cn1)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    Grid1.AddItem(UCase(!Group_Name))
    '                    Rw = Grid1.Rows - 1 : tt1 = 0
    '                    Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellBackColor = RGB(238, 230, 230)
    '                    RT2 = New ADODB.Recordset
    '                    RT2.Open("select a.int1, b.ledger_name, sum(a.currency1) as op_amt, sum(a.currency2) as cr_amt, sum(a.currency3) as db_amt, sum(a.currency1+a.currency2+a.currency3) as cl_amt from reporttemp a, ledger_head b where b.parent_code = '" & Trim(Rt1!parent_idno) & "' and a.int1 = b.ledger_idno group by a.int1, b.ledger_name having sum(a.currency1)<>0 or sum(a.currency2)<>0 or sum(a.currency3)<>0 order by b.ledger_name ", cn1)
    '                    If Not (RT2.BOF And RT2.EOF) Then
    '                        RT2.MoveFirst()
    '                        Do While Not RT2.EOF
    '                            Grid1.AddItem(" - " & StrConv(RT2!ledger_name, vbProperCase) & Chr(9) & PROC.Currency_Format(Abs(RT2!Op_Amt)) & IIf(RT2!Op_Amt > 0, " Cr", " Dr") & Chr(9) & PROC.Currency_Format(Abs(RT2!db_amt)) & Chr(9) & PROC.Currency_Format(RT2!cr_amt) & Chr(9) & PROC.Currency_Format(Abs(RT2!cl_amt)) & IIf(RT2!cl_amt > 0, " Cr", " Dr"))
    '                            tt1 = tt1 + RT2!cl_amt
    '                            op_tt = op_tt + RT2!Op_Amt : cr_tt1 = cr_tt1 + RT2!cr_amt : db_tt1 = db_tt1 + RT2!db_amt
    '                            Grid1.RowData(Grid1.Rows - 1) = RT2!int1
    '                            RT2.MoveNext()
    '                        Loop
    '                        Grid1.TextMatrix(Rw, 2) = PROC.Currency_Format(Abs(cr_tt1))
    '                        Grid1.TextMatrix(Rw, 3) = PROC.Currency_Format(Abs(db_tt1))
    '                        Grid1.TextMatrix(Rw, 4) = PROC.Currency_Format(Abs(tt1)) & IIf(tt1 > 0, " Cr", " Dr")
    '                        cr_tt = cr_tt + cr_tt1 : db_tt = db_tt + db_tt1
    '                        cr_tt1 = 0 : db_tt1 = 0
    '                        'tt3 = tt3 + tt1 'Else tt4 = tt4 + Abs(tt1)
    '                        Grid1.Row = Rw : Grid1.Col = 2 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                        Grid1.Row = Rw : Grid1.Col = 3 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                        Grid1.Row = Rw : Grid1.Col = 4 : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                    Else
    '                        Grid1.Rows = Grid1.Rows - 1
    '                    End If
    '                    RT2 = Nothing
    '                    Mdi1.StatusBar3.Panels(2).Text = StrConv(!Group_Name, vbProperCase)
    '                    .MoveNext()
    '                Loop
    '                Grid1.AddItem("")
    '                tt1 = op_tt + cr_tt + db_tt
    '                Grid1.AddItem("Total" & vbTab & PROC.Currency_Format(Abs(op_tt)) & IIf(op_tt > 0, " Cr", " Dr") & vbTab & PROC.Currency_Format(Abs(db_tt)) & vbTab & PROC.Currency_Format(cr_tt) & vbTab & PROC.Currency_Format(Abs(tt1)) & IIf(tt1 > 0, " Cr", " Dr"))
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '                Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellForeColor = RGB(0, 0, 0)
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing
    '    End Sub

    '    Private Sub Accounts_BankCash_InflowOutflow()
    '        Dim ope As Currency, tt1 As Currency, tt2 As Currency, tt3 As Currency
    '        Dim Rt1 As Recordset
    '        Dim gp_nm As String
    '        Dim Rw As Integer

    '        Grid1.Cols = 3
    '        Grid1.FormatString = "<PARTY NAME                                                   |>DEBIT                         |>CREDIT                    "
    '        Grid1.ColData(0) = 37 : Grid1.ColData(1) = 18 : Grid1.ColData(2) = 18
    '        FrmNm.Label1(0).Caption = "INFLOW / OUTFLOW - NAME : " & Trim(RptDet_Name1) & " - RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '        RptHeading1 = "INFLOW / OUTFLOW - NAME : " & Trim(RptDet_Name1)
    '        RptHeading2 = "RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '        Grid1.SelectionMode = 1

    '        cn1.Execute("delete from reporttemp")
    '        'Cn1.Execute "insert into reporttemp ( int1, currency1, currency2 ) select a.ledger_idno, sum(case when a.voucher_amount > 0 then abs(a.voucher_amount) else 0 end ), sum(case when a.voucher_amount < 0 then abs(a.voucher_amount) else 0 end ) from voucher_details a, voucher_head b, ledger_head c, voucher_details d where a.ledger_idno <> " & Str(RptDet_IdNo1) & " and ( b.creditor_idno = " & Str(RptDet_IdNo1) & " or b.debtor_idno = " & Str(RptDet_IdNo1) & " ) and ( a.sl_no = 1 or a.sl_no = 2 ) and a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and a.ledger_idno = c.ledger_idno and a.voucher_ref_no = d.voucher_ref_no and d.ledger_idno = " & Str(RptDet_IdNo1) & " group by a.ledger_idno"
    '        cn1.Execute("insert into reporttemp ( int1, currency1, currency2 ) select a.ledger_idno, sum(case when a.voucher_amount > 0 then abs(a.voucher_amount) else 0 end ), sum(case when a.voucher_amount < 0 then abs(a.voucher_amount) else 0 end ) from voucher_details a, ledger_head b where (rtrim(cast(a.company_idno as varchar(3)))+'-'+a.voucher_ref_no) in ( select distinct (rtrim(cast(z.company_idno as varchar(3)))+'-'+z.voucher_ref_no) from voucher_details z where z.ledger_idno = " & Str(RptDet_IdNo1) & " and z.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' ) and a.ledger_idno <> " & Str(RptDet_IdNo1) & " and a.ledger_idno = b.ledger_idno group by a.ledger_idno")

    '        Rt1 = New ADODB.Recordset
    '        With Rt1
    '            .Open("select group_name, b.ledger_idno, b.ledger_name, sum(currency1) as inflow, sum(currency2) as outflow from reporttemp, ledger_head b, group_head where int1 = b.ledger_idno and parent_idno = parent_code group by group_name, b.ledger_idno, b.ledger_name order by group_name, b.ledger_name", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If gp_nm <> !Group_Name Then
    '                        If Rw > 0 Then
    '                            Grid1.TextMatrix(Rw, IIf(tt3 < 0, 2, 1)) = PROC.Currency_Format(Abs(tt3))
    '                            Grid1.Row = Rw : Grid1.Col = IIf(tt3 < 0, 2, 1) : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                            Grid1.AddItem("")
    '                        End If
    '                        Grid1.AddItem!Group_Name()
    '                        Grid1.RowData(Grid1.Rows - 1) = 5
    '                        Rw = Grid1.Rows - 1 : tt3 = 0
    '                        Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellBackColor = RGB(238, 230, 230)
    '                    End If
    '                    Grid1.AddItem("     - " & !ledger_name & vbTab & PROC.Currency_Format(!inflow, True) & vbTab & PROC.Currency_Format(!outflow, True))
    '                    Grid1.RowData(Grid1.Rows - 1) = !Ledger_IdNo
    '                    tt3 = tt3 + !inflow - !outflow
    '                    tt1 = tt1 + !inflow
    '                    tt2 = tt2 + !outflow
    '                    gp_nm = !Group_Name
    '                    .MoveNext()
    '                Loop
    '                If Rw > 0 Then
    '                    Grid1.TextMatrix(Rw, IIf(tt3 < 0, 2, 1)) = PROC.Currency_Format(Abs(tt3))
    '                    Grid1.Row = Rw : Grid1.Col = IIf(tt3 < 0, 2, 1) : Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '                End If
    '                Grid1.AddItem("")
    '                Grid1.AddItem("TOTAL" & Chr(9) & PROC.Currency_Format(tt1, True) & Chr(9) & PROC.Currency_Format(tt2, True))
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '                Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellForeColor = RGB(0, 0, 0)
    '                Grid1.RowData(Grid1.Rows - 1) = 1
    '            End If
    '            .Close()

    '            .Open("select sum(voucher_amount) from voucher_details where ledger_idno = " & Str(RptDet_IdNo1) & " and voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "'", cn1, adOpenStatic, adLockReadOnly)
    '            If Rt1(0).Value <> "" Then ope = Val(Rt1(0).Value)
    '            .Close()
    '            Grid1.AddItem("OPENING" & vbTab & IIf(ope <= 0, PROC.Currency_Format(Abs(ope)), "") & vbTab & IIf(ope > 0, PROC.Currency_Format(ope), ""))
    '            ope = ope - tt1 + tt2
    '            Grid1.AddItem("CLOSING" & vbTab & IIf(ope <= 0, PROC.Currency_Format(Abs(ope)), "") & vbTab & IIf(ope > 0, PROC.Currency_Format(ope), ""))
    '            Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellForeColor = RGB(0, 0, 0)
    '            Grid1.RowData(Grid1.Rows - 1) = 2

    '        End With
    '        Rt1 = Nothing

    '    End Sub

    '    Private Sub Accounts_BankCash_InflowOutflow_PartyWise()
    '        Dim Rs1 As Recordset, Rt1 As Recordset
    '        Dim Ttc As Currency, Ttd As Currency
    '        Dim dt_cndt As String, GpCd As String, ent_idno As String

    '        Grid1.FormatString = "<DATE          |<ENT ID              |<PARTICULARS                                           |<PARTICULARS                                           |<TYPE   |>DB.AMOUNT       |>CR.AMOUNT       |>BALANCE             |<NARRATION                                  |<VOU.NO"
    '        Grid1.ColWidth(2) = 3000 : Grid1.ColWidth(3) = 2000 : Grid1.ColWidth(7) = 1800 : Grid1.ColWidth(8) = 2800 : Grid1.ColWidth(9) = 0
    '        FrmNm.Label1(0).Caption = "LEDGER : " & Trim(RptDet_Name2) & " - RANGE : " & Format(RptDet_Date1, "dd-mm-yyyy") & " TO " & Format(RptDet_Date2, "dd-mm-yyyy")
    '        RptHeading1 = "BANK/CASH INFLOW/OUTFLOW DETAILS - NAME : " & Trim(RptDet_Name2)
    '        RptHeading2 = "RANGE : " & Format(RptDet_Date1, "dd-mm-yyyy") & " TO " & Format(RptDet_Date2, "dd-mm-yyyy")

    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("select b.entry_identification, a.voucher_date, a.voucher_amount, b.voucher_no, b.voucher_type, b.entry_identification, c.ledger_name as crdr_name, a.narration from voucher_details a, voucher_head b, ledger_head c, voucher_details d where a.ledger_idno = " & Str(RptDet_IdNo2) & " and a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and ( case when a.voucher_amount < 0 then b.creditor_idno else b.debtor_idno end ) = c.ledger_idno and ( b.creditor_idno = " & Str(RptDet_IdNo1) & " or b.debtor_idno = " & Str(RptDet_IdNo1) & " ) and a.voucher_ref_no = d.voucher_ref_no and d.ledger_idno = " & Str(RptDet_IdNo1) & " order by a.voucher_date, b.for_orderby", cn1, adOpenStatic, adLockReadOnly)
    '        'Rs1.Open "select b.entry_identification, a.voucher_date, a.voucher_amount, b.voucher_no, b.voucher_type, b.entry_identification, c.ledger_name as crdr_name, a.narration from voucher_details a, voucher_head b, ledger_head c, voucher_details d where a.ledger_idno = " & Str(RptDet_IdNo2) & " and a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and ( case when a.voucher_amount < 0 then b.creditor_idno else b.debtor_idno end ) = c.ledger_idno and ( b.creditor_idno = " & Str(RptDet_IdNo1) & " or b.debtor_idno = " & Str(RptDet_IdNo1) & " ) and a.voucher_ref_no = d.voucher_ref_no and d.ledger_idno = " & Str(RptDet_IdNo1) & " order by a.voucher_date, b.for_orderby", Cn1, adOpenStatic, adLockReadOnly
    '        With Rs1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If Trim(Format(!Voucher_Date, "dd-mm-yy")) = Trim(Grid1.TextMatrix(Grid1.Rows - 1, 0)) Then Grid1.TextMatrix(Grid1.Rows - 1, 7) = ""
    '                    If !Voucher_Amount > 0 Then Ttc = Ttc + Val(!Voucher_Amount) Else Ttd = Ttd + Abs(Val(!Voucher_Amount))
    '                    If Left(!entry_identification, 6) = "VOUCH-" Then
    '                        ent_idno = UCase(!Voucher_Type) & "-" & !Voucher_No
    '                    Else
    '                        ent_idno = Replace(!entry_identification, "/" & Cmp_FnYear, "")
    '                    End If
    '                    Grid1.AddItem(Format(!Voucher_Date, "dd-mm-yy") & Chr(9) & ent_idno & Chr(9) & IIf(!Voucher_Amount > 0, "By " & Trim(!crdr_name), "To " & Trim(!crdr_name)) & Chr(9) & Trim(StrConv(!Narration, vbProperCase)) & Chr(9) & Trim(!Voucher_Type) & Chr(9) & IIf(!Voucher_Amount < 0, PROC.Currency_Format(Abs(!Voucher_Amount)) & vbTab, vbTab & PROC.Currency_Format(Abs(!Voucher_Amount))) & Chr(9) & PROC.Currency_Format(Abs(Ttc - Ttd)) & IIf(Ttc > Ttd, " Cr", " Dr") & Chr(9) & Trim(!Narration) & Chr(9) & !entry_identification)
    '                    Mdi1.StatusBar3.Panels(2).Text = Format(!Voucher_Date, "dd mmm")
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing
    '        Grid1.AllowUserResizing = 0
    '        Grid1.ColWidth(3) = 0 : Grid1.ColWidth(7) = 0
    '        Grid1.AddItem("")
    '        Grid1.AddItem("" & Chr(9) & "" & Chr(9) & "TOTAL" & Chr(9) & "TOTAL" & Chr(9) & "" & Chr(9) & PROC.Currency_Format(Ttd) & Chr(9) & PROC.Currency_Format(Ttc))
    '        Grid1.AddItem("" & Chr(9) & "" & Chr(9) & "CLOSING BALANCE" & Chr(9) & "CLOSING BALANCE" & Chr(9) & "" & Chr(9) & IIf(Ttc - Ttd < 0, "", vbTab) & PROC.Currency_Format(Abs(Ttc - Ttd)))
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '    End Sub

    '    Private Sub Accounts_BankCash_Transaction_Details()
    '        Dim Rs As ADODB.Recordset, Rs1 As ADODB.Recordset
    '        Dim Tot1 As Currency, Tot2 As Currency, Tot3 As Currency, Exp_Dir As Currency, Exp_InDir As Currency, Inc_Rnv As Currency
    '        Dim Rw As Integer, C1 As Integer, i As Integer
    '        Dim gp_nm As String, fl_nm As String
    '        Dim sub_tot(10) As Currency, Net_Tot(10) As Currency, Op_Tot As Currency, Cl_Tot As Currency

    '        With Grid1
    '            FrmNm.Label1(0).Caption = "CASH AND BANK TRANSACTION STATEMENT - RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '            RptHeading1 = "CASH AND BANK TRANSACTION STATEMENT"
    '            RptHeading2 = "RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '            Grid1.SelectionMode = 1
    '            .Rows = 2 : .Cols = 1
    '            .FormatString = "<A/C NAME                                         "
    '            cn1.Execute("delete from reporttemp")
    '            .TextMatrix(1, 0) = "OPENING BALANCE"
    '            Tot2 = 0
    '            Rs = New ADODB.Recordset
    '            Rs1 = New ADODB.Recordset
    '            Rs.Open("select * from ledger_head where parent_code like '%~5~4~' or parent_code like '%~6~4~' order by ledger_name", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    C1 = C1 + 1
    '                    .Cols = .Cols + 1
    '                    .TextMatrix(0, .Cols - 1) = Rs!ledger_name
    '                    .ColWidth(.Cols - 1) = 1500
    '                    Rs1.Open("select -1*sum(voucher_amount) from voucher_details where ledger_idno = " & Str(Rs!Ledger_IdNo) & " and voucher_date < '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "'", Con, adOpenStatic, adLockReadOnly)
    '                    If Rs1(0).Value <> "" Then
    '                        .TextMatrix(1, .Cols - 1) = Cmpr.Currency_Format(Rs1(0).Value)
    '                        Net_Tot(.Cols - 1) = Val(Rs1(0).Value)
    '                        Tot2 = Tot2 + Val(Rs1(0).Value)
    '                    End If
    '                    Rs1.Close()
    '                    fl_nm = fl_nm & ", sum(currency" & Trim(C1) & ")"
    '                    cn1.Execute("insert into reporttemp ( int1, currency" & Trim(C1) & " ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b where (rtrim(cast(a.company_idno as varchar(3)))+'-'+a.voucher_ref_no) in ( select distinct (rtrim(cast(z.company_idno as varchar(3)))+'-'+z.voucher_ref_no) from voucher_details z where z.ledger_idno = " & Str(Rs!Ledger_IdNo) & " and z.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' ) and a.ledger_idno <> " & Str(Rs!Ledger_IdNo) & " and a.ledger_idno = b.ledger_idno group by a.ledger_idno")
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .Cols = .Cols + 1
    '            .TextMatrix(0, .Cols - 1) = "TOTAL"
    '            .TextMatrix(.Rows - 1, .Cols - 1) = PROC.Currency_Format(Abs(Tot2)) & IIf(Tot2 < 0, " Cr", " Dr")
    '            .ColWidth(.Cols - 1) = 1500
    '            .AddItem("")
    '            Rs.Open("select group_name, b.ledger_idno, b.ledger_name " & Trim(fl_nm) & " from reporttemp, ledger_head b, group_head where int1 = b.ledger_idno and parent_idno = parent_code group by group_name, b.ledger_idno, b.ledger_name order by group_name, b.ledger_name", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    If gp_nm <> Rs!Group_Name Then
    '                        If Rw > 0 Then
    '                            Tot2 = 0
    '                            For i = 1 To .Cols - 1
    '                                .TextMatrix(Rw, i) = PROC.Currency_Format(sub_tot(i)) '& IIf(Sub_Tot(i) > 0, " Cr", " Dr")
    '                                Net_Tot(i) = Net_Tot(i) + sub_tot(i)
    '                                Tot2 = Tot2 + sub_tot(i)
    '                            Next i
    '                            If Rs!Group_Name = "EXPENSES (DIRECT)" Or Rs!Group_Name = "STAFF SALARY & ADVANCE" Then
    '                                Exp_Dir = Exp_Dir + (-1 * Tot2)
    '                            ElseIf Rs!Group_Name = "EXPENSES (INDIRECT)" Then
    '                                Exp_InDir = Exp_InDir + (-1 * Tot2)
    '                            ElseIf Rs!Group_Name = "INCOME (REVENUE)" Then
    '                                Inc_Rnv = Inc_Rnv + (Tot2)
    '                            End If
    '                            .TextMatrix(Rw, .Cols - 1) = PROC.Currency_Format(Abs(Tot2)) & IIf(Tot2 > 0, " Cr", " Dr")
    '                            Erase sub_tot
    '                            .AddItem("")
    '                        End If
    '                        .AddItem(Rs!Group_Name)
    '                        .RowData(Grid1.Rows - 1) = 5
    '                        Rw = Grid1.Rows - 1
    '                        .Row = .Rows - 1 : .Col = 0 : .CellBackColor = RGB(238, 230, 230)
    '                    End If
    '                    .AddItem("     - " & Rs!ledger_name)
    '                    Tot1 = 0
    '                    For i = 3 To Rs.Fields.Count - 1
    '                        .TextMatrix(.Rows - 1, i - 2) = PROC.Currency_Format(Val(Rs(i).Value), True)
    '                        Tot1 = Tot1 + Val(Rs(i).Value)
    '                        sub_tot(i - 2) = sub_tot(i - 2) + Val(Rs(i).Value)
    '                    Next i
    '                    .TextMatrix(Rw, .Cols - 1) = Tot1
    '                    .RowData(Grid1.Rows - 1) = Rs!Ledger_IdNo
    '                    gp_nm = Rs!Group_Name
    '                    Rs.MoveNext()
    '                Loop
    '                Tot2 = 0
    '                If Rw > 0 Then
    '                    For i = 1 To .Cols - 1
    '                        .TextMatrix(Rw, i) = PROC.Currency_Format(sub_tot(i)) '& IIf(Sub_Tot(i) > 0, " Cr", " Dr")
    '                        Net_Tot(i) = Net_Tot(i) + sub_tot(i)
    '                        Tot2 = Tot2 + sub_tot(i)
    '                    Next i
    '                    .TextMatrix(Rw, .Cols - 1) = PROC.Currency_Format(Abs(Tot2)) & IIf(Tot2 > 0, " Cr", " Dr")
    '                End If
    '                Tot2 = 0
    '                Grid1.AddItem("")
    '                Grid1.AddItem("CLOSING BALANCE")
    '                For i = 1 To .Cols - 1
    '                    .TextMatrix(.Rows - 1, i) = PROC.Currency_Format(Net_Tot(i)) '& IIf(Sub_Tot(i) > 0, " Cr", " Dr")
    '                    Tot2 = Tot2 + Net_Tot(i)
    '                Next i
    '                .TextMatrix(.Rows - 1, .Cols - 1) = PROC.Currency_Format(Abs(Tot2)) & IIf(Tot2 < 0, " Cr", " Dr")
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '                Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellForeColor = RGB(0, 0, 0)
    '                Grid1.RowData(Grid1.Rows - 1) = 1
    '            End If
    '            Rs.Close()

    '            .AddItem(vbTab & "INCOME" & vbTab & "EXPENSE")
    '            Tot2 = 0
    '            'Rs.Open "select c.ledger_name, a.amount, b.entry_identification from voucher_bill_details a, voucher_bill_head b, ledger_head c where a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and a.crdr_type = 'Cr' and ( b.entry_identification like 'SFINV-%' or b.entry_identification like 'SFAMC-%' or b.entry_identification like 'OPNBL-%' or b.entry_identification like 'HRDIN-%' ) and a.ledger_idno = c.ledger_idno order by ledger_name", Con, adOpenStatic, adLockReadOnly
    '            Rs.Open("select left(b.entry_identification,5), sum(a.amount) from voucher_bill_details a, voucher_bill_head b, ledger_head c where a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and a.crdr_type = 'Cr' and ( b.entry_identification like 'SFINV-%' or b.entry_identification like 'SFAMC-%' or ( b.entry_identification like 'OPNBL-%' and a.crdr_type = 'Cr' ) ) and a.ledger_idno = c.ledger_idno group by left(b.entry_identification,5) order by left(b.entry_identification,5)", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs(0).Value & vbTab & Cmpr.Currency_Format(Rs(1).Value)) '& vbTab & Rs(2).Value
    '                    Tot2 = Tot2 + Val(Rs(1).Value)
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            Tot3 = 0
    '            'Rs.Open "select sum(e.amount-f.amount) from voucher_bill_details a, voucher_bill_head b, ledger_head c, HardWare_Invoice_Head d, HardWare_Invoice_Details e, Hardware_Purchase_Details f where b.credit_amount=b.debit_amount and a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no=b.voucher_bill_no and a.company_idno=b.company_idno and a.crdr_type='Cr' and b.entry_identification like 'HRDIN-%' and a.ledger_idno=c.ledger_idno and b.entry_identification = 'HRDIN-'+d.HardWare_Invoice_Code and b.company_idno=d.company_idno and d.HardWare_Invoice_Code=e.HardWare_Invoice_Code and d.company_idno=e.company_idno and e.HardWare_Purchase_Code=f.HardWare_Purchase_Code and f.Details_SlNo=e.Hardware_Purchase_Slno and e.company_idno=b.company_idno", Con, adOpenStatic, adLockReadOnly
    '            Rs.Open("select c.ledger_name, e.amount, f.amount, b.entry_identification, cast(d.Company_Idno as varchar(5))+'-'+cast(d.Invoice_No as varchar(20)), a.amount from voucher_bill_details a, voucher_bill_head b, ledger_head c, HardWare_Invoice_Head d, HardWare_Invoice_Details e, Hardware_Purchase_Details f where b.credit_amount=b.debit_amount and a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no=b.voucher_bill_no and a.company_idno=b.company_idno and a.crdr_type='Cr' and b.entry_identification like 'HRDIN-%' and a.ledger_idno=c.ledger_idno and b.entry_identification = 'HRDIN-'+d.HardWare_Invoice_Code and b.company_idno=d.company_idno and d.HardWare_Invoice_Code=e.HardWare_Invoice_Code and d.company_idno=e.company_idno and e.HardWare_Purchase_Code=f.HardWare_Purchase_Code and f.Details_SlNo=e.Hardware_Purchase_Slno and e.company_idno=b.company_idno order by ledger_name", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!ledger_name & vbTab & Cmpr.Currency_Format(Rs(1).Value - Rs(2).Value) & vbTab & vbTab & Cmpr.Currency_Format(Rs(1).Value) & vbTab & Cmpr.Currency_Format(Rs(2).Value))
    '                    Tot3 = Tot3 + (Val(Rs(1).Value) - Val(Rs(2).Value))
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .AddItem("HRDIN-" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Tot3))
    '            Tot2 = Tot2 + Tot3
    '            .AddItem("EXPENSE (OFFICE)" & vbTab & vbTab & Cmpr.Currency_Format(Exp_Dir))
    '            .AddItem(vbTab & Cmpr.Currency_Format(Tot2) & vbTab & Cmpr.Currency_Format(Exp_Dir))
    '            .AddItem("PROFIT" & vbTab & Cmpr.Currency_Format(Tot2 - Exp_Dir))
    '            .AddItem("OTHER INCOME" & vbTab & Cmpr.Currency_Format(Inc_Rnv))
    '            .AddItem("ACTAUL SAVING" & vbTab & Cmpr.Currency_Format(Cl_Tot - Op_Tot))
    '            .AddItem("EXPENSE" & vbTab & Cmpr.Currency_Format((Tot2 + Inc_Rnv - Exp_Dir) - (Cl_Tot - Op_Tot)))

    '            .AddItem("")
    '            Tot2 = 0 : Tot3 = 0
    '            Rs.Open("select c.ledger_name, a.amount, a.crdr_type from voucher_bill_details a, voucher_bill_head b, ledger_head c where a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and ( b.entry_identification like 'HRDIN-%' or b.entry_identification like 'HRDPR-%' or ( b.entry_identification like 'OPNBL-%' and a.crdr_type = 'Dr' ) ) and a.ledger_idno = c.ledger_idno order by c.ledger_name", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!ledger_name & vbTab & IIf(Rs!crdr_type = "Cr", Cmpr.Currency_Format(Rs(1).Value), vbTab & Cmpr.Currency_Format(Rs(1).Value)))
    '                    If Rs!crdr_type = "Cr" Then Tot2 = Tot2 + Val(Rs(1).Value) Else Tot3 = Tot3 + Val(Rs(1).Value)
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .AddItem("TOTAL" & vbTab & Cmpr.Currency_Format(Tot2) & vbTab & Cmpr.Currency_Format(Tot3))
    '            Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            .AddItem("")

    '            .AddItem("SUNDRY CREDITORS ( HARDWARE BILLS ) PENDING")
    '            Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellBackColor = RGB(238, 230, 230)
    '            Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '            Tot2 = 0 : Tot3 = 0
    '            Rs.Open("select a.party_bill_no, a.voucher_bill_date, c.ledger_name, (a.credit_amount-a.debit_amount) as balance from voucher_bill_head a, ledger_head c where a.voucher_bill_date <= '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.entry_identification like 'HRDPR-%' and a.ledger_idno = c.ledger_idno and (a.credit_amount-a.debit_amount) <> 0 order by c.ledger_name, a.voucher_bill_date", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!ledger_name & vbTab & Cmpr.Currency_Format(Rs!Balance) & vbTab & Rs!Party_Bill_No & vbTab & Format(Rs!voucher_bill_date, "dd/mm/yy"))
    '                    Tot2 = Tot2 + Val(Rs!Balance)
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .AddItem("TOTAL" & vbTab & Cmpr.Currency_Format(Tot2))
    '            Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)

    '            .AddItem("SUNDRY DEBTORS ( HARDWARE BILLS ) PENDING")
    '            Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellBackColor = RGB(238, 230, 230)
    '            Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '            Tot2 = 0 : Tot3 = 0
    '            Rs.Open("select a.party_bill_no, a.voucher_bill_date, c.ledger_name, (a.debit_amount-a.credit_amount) as balance from voucher_bill_head a, ledger_head c where a.voucher_bill_date <= '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.entry_identification like 'HRDIN-%' and a.ledger_idno = c.ledger_idno and (a.debit_amount-a.credit_amount) <> 0 order by c.ledger_name, a.voucher_bill_date", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!ledger_name & vbTab & Cmpr.Currency_Format(Rs!Balance) & vbTab & Rs!Party_Bill_No & vbTab & Format(Rs!voucher_bill_date, "dd/mm/yy"))
    '                    Tot2 = Tot2 + Val(Rs!Balance)
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .AddItem("TOTAL" & vbTab & Cmpr.Currency_Format(Tot2))
    '            Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)

    '        End With
    '        Rs = Nothing

    '    End Sub

    '    Private Sub Accounts_BankCash_Transaction_Summary()
    '        Dim Rs As ADODB.Recordset, Rs1 As ADODB.Recordset
    '        Dim Tot1 As Currency, Tot2 As Currency, Tot3 As Currency
    '        Dim Rw As Integer, C1 As Integer, i As Integer
    '        Dim gp_nm As String, fl_nm As String
    '        Dim sub_tot(10) As Currency, Net_Tot(10) As Currency, Op_Tot As Currency, Cl_Tot As Currency
    '        Dim Exp_Dir As Currency, Exp_InDir As Currency, Inc_Rnv As Currency

    '        With Grid1
    '            FrmNm.Label1(0).Caption = "CASH AND BANK TRANSACTION SUMMARY - RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '            RptHeading1 = "CASH AND BANK TRANSACTION SUMMARY"
    '            RptHeading2 = "RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '            Grid1.SelectionMode = 1
    '            .Rows = 2 : .Cols = 1
    '            .FormatString = "<A/C NAME                                         "
    '            cn1.Execute("delete from reporttemp")
    '            .TextMatrix(1, 0) = "OPENING BALANCE"
    '            Tot2 = 0
    '            Rs = New ADODB.Recordset
    '            Rs1 = New ADODB.Recordset
    '            Rs.Open("select * from ledger_head where parent_code like '%~5~4~' or parent_code like '%~6~4~' order by ledger_name", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    C1 = C1 + 1
    '                    .Cols = .Cols + 1
    '                    .TextMatrix(0, .Cols - 1) = Rs!Ledger_Address1
    '                    .ColWidth(.Cols - 1) = 1500
    '                    Rs1.Open("select -1*sum(voucher_amount) from voucher_details where ledger_idno = " & Str(Rs!Ledger_IdNo) & " and voucher_date < '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "'", Con, adOpenStatic, adLockReadOnly)
    '                    If Rs1(0).Value <> "" Then
    '                        .TextMatrix(1, .Cols - 1) = Cmpr.Currency_Format(Rs1(0).Value)
    '                        Net_Tot(.Cols - 1) = Val(Rs1(0).Value)
    '                        Tot2 = Tot2 + Val(Rs1(0).Value)
    '                    End If
    '                    Rs1.Close()
    '                    fl_nm = fl_nm & ", sum(currency" & Trim(C1) & ")"
    '                    cn1.Execute("insert into reporttemp ( int1, currency" & Trim(C1) & " ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b where (rtrim(cast(a.company_idno as varchar(3)))+'-'+a.voucher_ref_no) in ( select distinct (rtrim(cast(z.company_idno as varchar(3)))+'-'+z.voucher_ref_no) from voucher_details z where z.ledger_idno = " & Str(Rs!Ledger_IdNo) & " and z.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' ) and a.ledger_idno <> " & Str(Rs!Ledger_IdNo) & " and a.ledger_idno = b.ledger_idno group by a.ledger_idno")
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .Cols = .Cols + 1
    '            .TextMatrix(0, .Cols - 1) = "TOTAL"
    '            Op_Tot = Tot2
    '            .TextMatrix(.Rows - 1, .Cols - 1) = PROC.Currency_Format(Abs(Tot2)) & IIf(Tot2 < 0, " Cr", " Dr")
    '            .ColWidth(.Cols - 1) = 1500
    '            .AddItem("")
    '            Rs.Open("select group_name " & Trim(fl_nm) & " from reporttemp, ledger_head b, group_head where int1 = b.ledger_idno and parent_idno = parent_code group by group_name order by group_name", cn1, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!Group_Name)
    '                    Tot1 = 0
    '                    For i = 1 To Rs.Fields.Count - 1
    '                        .TextMatrix(.Rows - 1, i) = PROC.Currency_Format(Val(Rs(i).Value), True)
    '                        Tot1 = Tot1 + Val(Rs(i).Value)
    '                        Net_Tot(i) = Net_Tot(i) + Val(Rs(i).Value)
    '                    Next i
    '                    If Rs!Group_Name = "EXPENSES (DIRECT)" Or Rs!Group_Name = "STAFF SALARY & ADVANCE" Then
    '                        Exp_Dir = Exp_Dir + (-1 * Tot1)
    '                    ElseIf Rs!Group_Name = "EXPENSES (INDIRECT)" Then
    '                        Exp_InDir = Exp_InDir + (-1 * Tot1)
    '                    ElseIf Rs!Group_Name = "INCOME (REVENUE)" Then
    '                        Inc_Rnv = Inc_Rnv + (Tot1)
    '                    End If
    '                    .TextMatrix(.Rows - 1, .Cols - 1) = PROC.Currency_Format(Abs(Tot1), True) & IIf(Tot1 > 0, " Cr", " Dr")
    '                    Rs.MoveNext()
    '                Loop
    '                Tot2 = 0
    '                Grid1.AddItem("")
    '                Grid1.AddItem("CLOSING BALANCE")
    '                For i = 1 To .Cols - 1
    '                    .TextMatrix(.Rows - 1, i) = PROC.Currency_Format(Net_Tot(i)) '& IIf(Sub_Tot(i) > 0, " Cr", " Dr")
    '                    Tot2 = Tot2 + Net_Tot(i)
    '                Next i
    '                Cl_Tot = Tot2
    '                .TextMatrix(.Rows - 1, .Cols - 1) = PROC.Currency_Format(Abs(Tot2)) & IIf(Tot2 < 0, " Cr", " Dr")
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '                Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellForeColor = RGB(0, 0, 0)
    '                Grid1.RowData(Grid1.Rows - 1) = 1
    '            End If
    '            Rs.Close()

    '            .AddItem(vbTab & "INCOME" & vbTab & "EXPENSE")
    '            Tot2 = 0
    '            'Rs.Open "select c.ledger_name, a.amount, b.entry_identification from voucher_bill_details a, voucher_bill_head b, ledger_head c where a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and a.crdr_type = 'Cr' and ( b.entry_identification like 'SFINV-%' or b.entry_identification like 'SFAMC-%' or b.entry_identification like 'OPNBL-%' or b.entry_identification like 'HRDIN-%' ) and a.ledger_idno = c.ledger_idno order by ledger_name", Con, adOpenStatic, adLockReadOnly
    '            Rs.Open("select left(b.entry_identification,5), sum(a.amount) from voucher_bill_details a, voucher_bill_head b, ledger_head c where a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and a.crdr_type = 'Cr' and ( b.entry_identification like 'SFINV-%' or b.entry_identification like 'SFAMC-%' or ( b.entry_identification like 'OPNBL-%' and a.crdr_type = 'Cr' ) ) and a.ledger_idno = c.ledger_idno group by left(b.entry_identification,5) order by left(b.entry_identification,5)", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs(0).Value & vbTab & Cmpr.Currency_Format(Rs(1).Value)) '& vbTab & Rs(2).Value
    '                    Tot2 = Tot2 + Val(Rs(1).Value)
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            Tot3 = 0
    '            'Rs.Open "select sum(e.amount-f.amount) from voucher_bill_details a, voucher_bill_head b, ledger_head c, HardWare_Invoice_Head d, HardWare_Invoice_Details e, Hardware_Purchase_Details f where b.credit_amount=b.debit_amount and a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no=b.voucher_bill_no and a.company_idno=b.company_idno and a.crdr_type='Cr' and b.entry_identification like 'HRDIN-%' and a.ledger_idno=c.ledger_idno and b.entry_identification = 'HRDIN-'+d.HardWare_Invoice_Code and b.company_idno=d.company_idno and d.HardWare_Invoice_Code=e.HardWare_Invoice_Code and d.company_idno=e.company_idno and e.HardWare_Purchase_Code=f.HardWare_Purchase_Code and f.Details_SlNo=e.Hardware_Purchase_Slno and e.company_idno=b.company_idno", Con, adOpenStatic, adLockReadOnly
    '            Rs.Open("select c.ledger_name, e.amount, f.amount, b.entry_identification, cast(d.Company_Idno as varchar(5))+'-'+cast(d.Invoice_No as varchar(20)), a.amount from voucher_bill_details a, voucher_bill_head b, ledger_head c, HardWare_Invoice_Head d, HardWare_Invoice_Details e, Hardware_Purchase_Details f where b.credit_amount=b.debit_amount and a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no=b.voucher_bill_no and a.company_idno=b.company_idno and a.crdr_type='Cr' and b.entry_identification like 'HRDIN-%' and a.ledger_idno=c.ledger_idno and b.entry_identification = 'HRDIN-'+d.HardWare_Invoice_Code and b.company_idno=d.company_idno and d.HardWare_Invoice_Code=e.HardWare_Invoice_Code and d.company_idno=e.company_idno and e.HardWare_Purchase_Code=f.HardWare_Purchase_Code and f.Details_SlNo=e.Hardware_Purchase_Slno and e.company_idno=b.company_idno order by ledger_name", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!ledger_name & vbTab & Cmpr.Currency_Format(Rs(1).Value - Rs(2).Value) & vbTab & vbTab & Cmpr.Currency_Format(Rs(1).Value) & vbTab & Cmpr.Currency_Format(Rs(2).Value))
    '                    Tot3 = Tot3 + (Val(Rs(1).Value) - Val(Rs(2).Value))
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .AddItem("HRDIN-" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Tot3))
    '            Tot2 = Tot2 + Tot3
    '            .AddItem("EXPENSE (OFFICE)" & vbTab & vbTab & Cmpr.Currency_Format(Exp_Dir))
    '            .AddItem(vbTab & Cmpr.Currency_Format(Tot2) & vbTab & Cmpr.Currency_Format(Exp_Dir))
    '            .AddItem("PROFIT" & vbTab & Cmpr.Currency_Format(Tot2 - Exp_Dir))
    '            .AddItem("OTHER INCOME" & vbTab & Cmpr.Currency_Format(Inc_Rnv))
    '            .AddItem("ACTAUL SAVING" & vbTab & Cmpr.Currency_Format(Cl_Tot - Op_Tot))
    '            .AddItem("EXPENSE" & vbTab & Cmpr.Currency_Format((Tot2 + Inc_Rnv - Exp_Dir) - (Cl_Tot - Op_Tot)))

    '            .AddItem("")
    '            Tot2 = 0 : Tot3 = 0
    '            Rs.Open("select c.ledger_name, a.amount, a.crdr_type from voucher_bill_details a, voucher_bill_head b, ledger_head c where a.voucher_bill_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and a.company_idno = b.company_idno and ( b.entry_identification like 'HRDIN-%' or b.entry_identification like 'HRDPR-%' or ( b.entry_identification like 'OPNBL-%' and a.crdr_type = 'Dr' ) ) and a.ledger_idno = c.ledger_idno order by c.ledger_name", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!ledger_name & vbTab & IIf(Rs!crdr_type = "Cr", Cmpr.Currency_Format(Rs(1).Value), vbTab & Cmpr.Currency_Format(Rs(1).Value)))
    '                    If Rs!crdr_type = "Cr" Then Tot2 = Tot2 + Val(Rs(1).Value) Else Tot3 = Tot3 + Val(Rs(1).Value)
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .AddItem("TOTAL" & vbTab & Cmpr.Currency_Format(Tot2) & vbTab & Cmpr.Currency_Format(Tot3))
    '            Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            .AddItem("")

    '            .AddItem("SUNDRY CREDITORS ( HARDWARE BILLS ) PENDING")
    '            Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellBackColor = RGB(238, 230, 230)
    '            Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '            Tot2 = 0 : Tot3 = 0
    '            Rs.Open("select c.ledger_name, (a.credit_amount-a.debit_amount) as balance, a.party_bill_no, a.Particulars from voucher_bill_head a, ledger_head c where a.voucher_bill_date <= '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.entry_identification like 'HRDPR-%' and a.ledger_idno = c.ledger_idno and (a.credit_amount-a.debit_amount) <> 0 order by c.ledger_name", Con, adOpenStatic, adLockReadOnly)
    '            'Rs.Open "select c.ledger_name, sum(a.credit_amount-a.debit_amount) from voucher_bill_head a, ledger_head c where a.voucher_bill_date <= '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.entry_identification like 'HRDPR-%' and a.ledger_idno = c.ledger_idno group by c.ledger_name having sum(a.credit_amount-a.debit_amount) <> 0 order by c.ledger_name", Con, adOpenStatic, adLockReadOnly
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!ledger_name & vbTab & Cmpr.Currency_Format(Rs(1).Value) & vbTab & Rs!Party_Bill_No & vbTab & Rs!Particulars)
    '                    Tot2 = Tot2 + Val(Rs(1).Value)
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .AddItem("TOTAL" & vbTab & Cmpr.Currency_Format(Tot2))
    '            Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)

    '            .AddItem("SUNDRY DEBTORS ( HARDWARE BILLS ) PENDING")
    '            Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellBackColor = RGB(238, 230, 230)
    '            Grid1.CellFontSize = 10 : Grid1.CellFontName = "Ms Sans Serif" : Grid1.CellFontBold = True
    '            Tot2 = 0 : Tot3 = 0
    '            Rs.Open("select c.ledger_name, (a.debit_amount-a.credit_amount) as balance, party_bill_no from voucher_bill_head a, ledger_head c where a.voucher_bill_date <= '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.entry_identification like 'HRDIN-%' and a.ledger_idno = c.ledger_idno and (a.debit_amount-a.credit_amount) <> 0 order by c.ledger_name", Con, adOpenStatic, adLockReadOnly)
    '            If Not (Rs.BOF And Rs.EOF) Then
    '                Rs.MoveFirst()
    '                Do While Not Rs.EOF
    '                    .AddItem(Rs!ledger_name & vbTab & Cmpr.Currency_Format(Rs(1).Value) & vbTab & Rs!Party_Bill_No)
    '                    Tot2 = Tot2 + Val(Rs(1).Value)
    '                    Rs.MoveNext()
    '                Loop
    '            End If
    '            Rs.Close()
    '            .AddItem("TOTAL" & vbTab & Cmpr.Currency_Format(Tot2))
    '            Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)

    '        End With


    '        Rs = Nothing

    '    End Sub

    '    Private Sub Accounts_Bank_InflowOutflow_Summary_PartyWise()
    '        Dim Rs1 As Recordset, Rt1 As Recordset
    '        Dim Ttc As Currency, Ttd As Currency
    '        Dim dt_cndt As String, GpCd As String, ent_idno As String

    '        Grid1.FormatString = "<DATE          |<ENT ID              |<PARTICULARS                                           |<PARTICULARS                                           |<TYPE   |>DB.AMOUNT       |>CR.AMOUNT       |>BALANCE             |<NARRATION                                  |<VOU.NO"
    '        Grid1.ColWidth(2) = 3000 : Grid1.ColWidth(3) = 2000 : Grid1.ColWidth(7) = 1800 : Grid1.ColWidth(8) = 2800 : Grid1.ColWidth(9) = 0
    '        FrmNm.Label1(0).Caption = "LEDGER : " & Trim(RptDet_Name2) & " - RANGE : " & Format(RptDet_Date1, "dd-mm-yyyy") & " TO " & Format(RptDet_Date2, "dd-mm-yyyy")
    '        RptHeading1 = "LEDGER : " & Trim(RptDet_Name1)
    '        RptHeading2 = "RANGE : " & Format(RptDet_Date1, "dd-mm-yyyy") & " TO " & Format(RptDet_Date2, "dd-mm-yyyy")

    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("select b.entry_identification, a.voucher_date, a.voucher_amount, b.voucher_no, b.voucher_type, b.entry_identification, c.ledger_name as crdr_name, a.narration from voucher_details a, voucher_head b, ledger_head c where a.ledger_idno = " & Str(RptDet_IdNo1) & " and a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and ( case when a.ledger_idno = b.creditor_idno then b.debtor_idno else b.creditor_idno end ) = c.ledger_idno and c.parent_code like '%~5~4~' order by a.voucher_date, b.for_orderby", cn1, adOpenStatic, adLockReadOnly)
    '        With Rs1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                    If Trim(Format(!Voucher_Date, "dd-mm-yy")) = Trim(Grid1.TextMatrix(Grid1.Rows - 1, 0)) Then Grid1.TextMatrix(Grid1.Rows - 1, 7) = ""
    '                    If !Voucher_Amount > 0 Then Ttc = Ttc + Val(!Voucher_Amount) Else Ttd = Ttd + Abs(Val(!Voucher_Amount))
    '                    If Left(!entry_identification, 6) = "VOUCH-" Then
    '                        ent_idno = UCase(!Voucher_Type) & "-" & !Voucher_No
    '                    Else
    '                        ent_idno = Replace(!entry_identification, "/" & Cmp_FnYear, "")
    '                    End If
    '                    Grid1.AddItem(Format(!Voucher_Date, "dd-mm-yy") & Chr(9) & ent_idno & Chr(9) & IIf(!Voucher_Amount > 0, "By " & Trim(!crdr_name), "To " & Trim(!crdr_name)) & Chr(9) & Trim(StrConv(!Narration, vbProperCase)) & Chr(9) & Trim(!Voucher_Type) & Chr(9) & IIf(!Voucher_Amount < 0, PROC.Currency_Format(Abs(!Voucher_Amount)) & vbTab, vbTab & PROC.Currency_Format(Abs(!Voucher_Amount))) & Chr(9) & PROC.Currency_Format(Abs(Ttc - Ttd)) & IIf(Ttc > Ttd, " Cr", " Dr") & Chr(9) & Trim(!Narration) & Chr(9) & !entry_identification)
    '                    Mdi1.StatusBar3.Panels(2).Text = Format(!Voucher_Date, "dd mmm")
    '                    .MoveNext()
    '                Loop
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing
    '        Grid1.AllowUserResizing = 0
    '        Grid1.ColWidth(3) = 0 : Grid1.ColWidth(7) = 0
    '        Grid1.AddItem("")
    '        Grid1.AddItem("" & Chr(9) & "" & Chr(9) & "TOTAL" & Chr(9) & "TOTAL" & Chr(9) & "" & Chr(9) & PROC.Currency_Format(Ttd) & Chr(9) & PROC.Currency_Format(Ttc))
    '        Grid1.AddItem("" & Chr(9) & "" & Chr(9) & "CLOSING BALANCE" & Chr(9) & "CLOSING BALANCE" & Chr(9) & "" & Chr(9) & IIf(Ttc - Ttd < 0, "", vbTab) & PROC.Currency_Format(Abs(Ttc - Ttd)))
    '        Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '    End Sub

    '    Private Sub Accounts_MoneyTurnOver()
    '        Dim ope As Currency, tt1 As Currency, tt2 As Currency
    '        Dim Rt1 As Recordset

    '        Grid1.Cols = 5
    '        Grid1.FormatString = "<BANK / CASH                                    |>OPENING                       |>DEBIT                            |>CREDIT                       |>CLOSING                    "
    '        Grid1.ColData(0) = 37 : Grid1.ColData(1) = 20 : Grid1.ColData(2) = 18 : Grid1.ColData(3) = 18 : Grid1.ColData(4) = 20
    '        FrmNm.Label1(0).Caption = "MONEY TURN OVER - RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '        RptHeading1 = "MONEY TURN OVER - RANGE : " & Format(RptDet_Date1, "dd/mm/yyyy") & " TO " & Format(RptDet_Date2, "dd/mm/yyyy")
    '        Grid1.SelectionMode = 1

    '        cn1.Execute("delete from reporttemp")

    '        cn1.Execute("insert into reporttemp ( int1, currency1 ) select b.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b where ( b.parent_code like '%~5~4~' or b.parent_code like '%~6~4~' or b.parent_code like '%~23~21~' ) and a.voucher_date < '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and a.ledger_idno = b.ledger_idno group by b.ledger_idno")
    '        cn1.Execute("insert into reporttemp ( int1, currency2 ) select b.ledger_idno, -1*sum(a.voucher_amount) from voucher_details a, ledger_head b where ( b.parent_code like '%~5~4~' or b.parent_code like '%~6~4~' or b.parent_code like '%~23~21~' ) and a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_amount < 0 and a.ledger_idno = b.ledger_idno group by b.ledger_idno")
    '        cn1.Execute("insert into reporttemp ( int1, currency3 ) select b.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head b where ( b.parent_code like '%~5~4~' or b.parent_code like '%~6~4~' or b.parent_code like '%~23~21~' ) and a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.voucher_amount > 0 and a.ledger_idno = b.ledger_idno group by b.ledger_idno")

    '        Rt1 = New ADODB.Recordset
    '        With Rt1
    '            .Open("select ledger_name, sum(currency1) as opening, sum(currency2) as debit, sum(currency3) as credit from reporttemp, ledger_head where int1 = ledger_idno group by ledger_name", cn1)
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst()
    '                Do While Not .EOF
    '                Grid1.AddItem !ledger_name & vbTab & PROC.Currency_Format(Abs(!opening)) & IIf(!opening <= 0, "  Dr", "  Cr") & vbTab & PROC.Currency_Format(!Debit, True) & vbTab & PROC.Currency_Format(!Credit, True) & vbTab & PROC.Currency_Format(Abs(!opening + !Credit - !Debit)) & IIf(!opening + !Credit - !Debit <= 0, "  Dr", "  Cr")
    '                    tt1 = tt1 + !Debit
    '                    tt2 = tt2 + !Credit
    '                    ope = ope + !opening
    '                    .MoveNext()
    '                Loop
    '                Grid1.AddItem("")
    '                Grid1.AddItem("TOTAL" & Chr(9) & PROC.Currency_Format(Abs(ope)) & IIf(ope > 0, "  Cr", "  Dr") & Chr(9) & PROC.Currency_Format(tt1) & Chr(9) & PROC.Currency_Format(tt2) & vbTab & PROC.Currency_Format(Abs(ope + tt2 - tt1)) & IIf(ope + tt2 - tt1 <= 0, "  Dr", "  Cr"))
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '                Grid1.RowData(Grid1.Rows - 1) = 1
    '                Grid1.AddItem("BALANCE" & Chr(9) & Chr(9) & PROC.Currency_Format(Abs(ope - tt1)) & IIf(ope - tt1 > 0, " Cr", " Dr") & Chr(9) & vbTab & PROC.Currency_Format(Abs((ope + tt2 - tt1) - tt2)) & IIf(((ope + tt2 - tt1) - tt2) > 0, " Cr", " Dr"))
    '                Call PROC.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '                Call PROC.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '                Grid1.Row = Grid1.Rows - 1 : Grid1.Col = 0 : Grid1.CellForeColor = RGB(0, 0, 0)
    '                Grid1.RowData(Grid1.Rows - 1) = 1
    '            End If
    '            .Close()
    '        End With
    '        Rt1 = Nothing

    '    End Sub

    '    Public Sub Confirmation_Of_Accounts_Details()
    '        Dim Rs1 As ADODB.Recordset
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '        Dim tt_cr As Currency, Tt_Dr As Currency
    '        Dim nar As String

    '        RptDet.Idno1 = Company_ShortNameToIdno(RptInp(0).Caption)
    '        If Val(RptDet.Idno1) = 0 Then MgB.Message Error, "Invalid Companyname"

    '        RptDet.Idno2 = Cmpr.Ledger_NameToIdno(Con, RptInp(1).Caption)
    '        If Val(RptDet.Idno2) = 0 Then MgB.Message Error, "Invalid PartyName"

    '        'Open "C:\Samp1.txt" For Output As #1
    '        Print #1, Chr(15); Chr(18)

    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select * from Company_Head a, Ledger_Head b Where Company_Idno in " & RptInp(0).Value & " and Ledger_Idno in " & RptInp(1).Value, Con, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            Rs1.MoveFirst()
    '                    Print #1, " From "; Chr(27); "E"; Trim(Rs1!Company_Name); Chr(27); "F"; Spc(36 - Len(Trim(Rs1!Company_Name))); "To   : " & Chr(27); "E"; Trim(Rs1!ledger_name); Chr(27); "F"
    '                    Print #1, "      "; Trim(Rs1!Company_Address1); Spc(43 - Len(Trim(Rs1!Company_Address1))); Trim(Rs1!Ledger_Address1)
    '                    Print #1, "      "; Trim(Rs1!Company_Address2); Spc(43 - Len(Trim(Rs1!Company_Address2))); Trim(Rs1!Ledger_Address2)
    '                    Print #1, "      "; Trim(Rs1!Company_Address3); Spc(43 - Len(Trim(Rs1!Company_Address3))); Trim(Rs1!Ledger_Address3)
    '                    Print #1, "      "; Trim(Rs1!Company_Address4); Spc(43 - Len(Trim(Rs1!Company_Address4))); Trim(Rs1!Ledger_Address4)
    '                    Print #1,
    '                    Print #1, " Dear Sir/Madam,"; Spc(44); "Dated  : " & Trim(Format(Date, "dd-mmm-yyyy"))
    '                    Print #1,
    '                    Print #1, Spc(24); "Sub : Confirmation Of Accounts"
    '                    Print #1, Spc(26); Trim(Format(CmpDet.FromDate, "dd-mmm-yyyy")); " to "; Trim(Format(CmpDet.ToDate, "dd-mmm-yyyy"))
    '                    Print #1,
    '                    Print #1,
    '                    Print #1, " Given  below  the  details of  your  accounts  as  standing in  my/our Books of"
    '                    Print #1, " Accounts for the above mentioned period."
    '                    Print #1,
    '                    Print #1, " Kindly  return 3 copies  stating your I.T Permananent A/c No, duly  signed  and"
    '                    Print #1, " sealed, in  confirmation  of the same. Please note that if no reply is received"
    '                    Print #1, " from  you  within  a  fortnight. it will be assumed that you have accepted that"
    '                    Print #1, " balance shown below."

    '                    Print #1,
    '                    Print #1,
    '                    Ln_No = 22
    '                    GoSub Page_Header
    '                    GoSub Print_Details

    '                    Print #1,
    '                    Print #1,
    '                    Print #1, " I/We hereby confirm the above"; Spc(30); "Yours faithfully,"
    '                    Print #1,
    '                    Print #1,
    '                    Print #1,
    '                    Print #1, " I.T PAN No."; Spc(35); "Our I.T. PAN No .: " & Trim(Rs1!Company_PanNo)
    '            Ln_No = Ln_No + 7

    '            For i = Ln_No + 1 To 72
    '                        Print #1,
    '            Next
    '        End If
    '        Rs1.Close()
    '        Rs1 = Nothing

    '        Close #1

    '        Exit Sub

    '        '================================================================================================================
    '        '================================================================================================================


    'Page_Header:
    '        Print #1, Chr(15); Chr(18); String(80, 45)
    '        Print #1, "  DATE               PARTICULARS                           DEBIT         CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = Ln_No + 3
    '        Return

    'Print_Details:
    '        For i = 2 To Grid1.Rows - 4
    '            If Ln_No > 60 Then GoSub Page_Footer
    '            If Trim(Grid1.TextMatrix(i, 1)) <> Trim(Grid1.TextMatrix(i - 1, 1)) Then
    '                Print #1, Trim(Grid1.TextMatrix(i, 0)); Spc(11 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '                If Grid1.ColWidth(2) > 0 Then Print #1, Grid1.TextMatrix(i, 2); Spc(7 - Len(Grid1.TextMatrix(i, 2)));
    '            Else
    '                Print #1, Spc(11);
    '                If Grid1.ColWidth(2) > 0 Then Print #1, Spc(7);
    '            End If
    '            'If Grid1.ColWidth(3) > 0 Then Print #1, Left$(Trim(Grid1.TextMatrix(i, 3)), 30); Spc(32 - Len(Left$(Trim(Grid1.TextMatrix(i, 3)), 30)));
    '            'If Grid1.ColWidth(4) > 0 Then Print #1, Left$(Trim(Grid1.TextMatrix(i, 4)), 30); Spc(32 - Len(Left$(Trim(Grid1.TextMatrix(i, 4)), 30)));
    '            If Grid1.ColWidth(3) > 0 Then Print #1, Left$(Trim(Grid1.TextMatrix(i, 3)), 37); Spc(39 - Len(Left$(Trim(Grid1.TextMatrix(i, 3)), 37)));
    '            If Grid1.ColWidth(4) > 0 Then Print #1, Left$(Trim(Grid1.TextMatrix(i, 4)), 37); Spc(39 - Len(Left$(Trim(Grid1.TextMatrix(i, 4)), 37)));
    '            Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(i, 6)))); Trim(Grid1.TextMatrix(i, 6));
    '            Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(i, 7)))); Trim(Grid1.TextMatrix(i, 7))
    '            If Val(Grid1.TextMatrix(i, 6)) <> 0 Then Tt_Dr = Tt_Dr + CCur(Grid1.TextMatrix(i, 6))
    '            If Val(Grid1.TextMatrix(i, 7)) <> 0 Then tt_cr = tt_cr + CCur(Grid1.TextMatrix(i, 7))
    '            Ln_No = Ln_No + 1
    '            If Trim(Grid1.TextMatrix(i, 1)) <> Trim(Grid1.TextMatrix(i + 1, 1)) Then
    '                If Grid1.ColWidth(9) > 0 And Trim(Grid1.TextMatrix(i, 9)) <> "" Then
    '                    nar = Trim(Grid1.TextMatrix(i, 9))
    '                    Do While Len(nar) > 35
    '                        For k = 35 To 1 Step -1
    '                            If Mid$(nar, k, 1) = " " Or Mid$(nar, k, 1) = "," Or Mid$(nar, k, 1) = ")" Then Exit For
    '                        Next k
    '                        If k = 0 Then k = 35
    '                        Print #1, Spc(18);
    '                        Print #1, "   "; Trim(Left$(nar, k))
    '                        Ln_No = Ln_No + 1
    '                        nar = Right(nar, Len(nar) - k)
    '                    Loop
    '                    Print #1, Spc(18);
    '                    Print #1, "   "; Trim(nar)
    '                    Ln_No = Ln_No + 1
    '                End If
    '                Print #1,
    '                Ln_No = Ln_No + 1
    '            End If
    '        Next i
    '        Print #1, String(80, 45)
    '        Print #1, Spc(18);
    '        Print #1, Spc(3); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 4)); Spc(29 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 4))));
    '        Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 6)))); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 6));
    '        Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 7)))); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 7))
    '        Print #1, String(80, 45)
    '        Print #1, Spc(18);
    '        Print #1, Spc(3); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 4)); Spc(29 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 4))));
    '        Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 6)))); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 6));
    '        Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 7)))); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 7))
    '        Print #1, String(80, 45)
    '        Ln_No = Ln_No + 5
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(18);
    '        Print #1, "   C/O"; Spc(26);
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr));
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr))
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 4
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        Ln_No = 0
    '        GoSub Page_Header
    '        Print #1, Spc(18);
    '        Print #1, "   B/F"; Spc(26);
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr));
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr))
    '        Print #1,
    '        Ln_No = Ln_No + 2
    '        Return

    '    End Sub

    '    Private Sub Print_SingleLedger_ConfirmationLetter()
    '        Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '        Dim tt_cr As Currency, Tt_Dr As Currency
    '        Dim nar As String

    '    GoSub Page_Header

    '        For i = 2 To Grid1.Rows - 4
    '        If Ln_No > 60 Then GoSub Page_Footer
    '            If Trim(Grid1.TextMatrix(i, 1)) <> Trim(Grid1.TextMatrix(i - 1, 1)) Then
    '            Print #1, Trim(Grid1.TextMatrix(i, 0)); Spc(11 - Len(Trim(Grid1.TextMatrix(i, 0))));
    '            If Grid1.ColWidth(2) > 0 Then Print #1, Grid1.TextMatrix(i, 2); Spc(7 - Len(Grid1.TextMatrix(i, 2)));
    '            Else
    '            Print #1, Spc(11);
    '            If Grid1.ColWidth(2) > 0 Then Print #1, Spc(7);
    '            End If
    '        If Grid1.ColWidth(3) > 0 Then Print #1, Left$(Trim(Grid1.TextMatrix(i, 3)), 30); Spc(32 - Len(Left$(Trim(Grid1.TextMatrix(i, 3)), 30)));
    '        If Grid1.ColWidth(4) > 0 Then Print #1, Left$(Trim(Grid1.TextMatrix(i, 4)), 30); Spc(32 - Len(Left$(Trim(Grid1.TextMatrix(i, 4)), 30)));
    '        Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(i, 6)))); Trim(Grid1.TextMatrix(i, 6));
    '        Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(i, 7)))); Trim(Grid1.TextMatrix(i, 7))
    '            If Val(Grid1.TextMatrix(i, 6)) <> 0 Then Tt_Dr = Tt_Dr + CCur(Grid1.TextMatrix(i, 6))
    '            If Val(Grid1.TextMatrix(i, 7)) <> 0 Then tt_cr = tt_cr + CCur(Grid1.TextMatrix(i, 7))
    '            Ln_No = Ln_No + 1
    '            If Trim(Grid1.TextMatrix(i, 1)) <> Trim(Grid1.TextMatrix(i + 1, 1)) Then
    '                If Grid1.ColWidth(9) > 0 And Trim(Grid1.TextMatrix(i, 9)) <> "" Then
    '                    nar = Trim(Grid1.TextMatrix(i, 9))
    '                    Do While Len(nar) > 35
    '                        For k = 35 To 1 Step -1
    '                            If Mid$(nar, k, 1) = " " Or Mid$(nar, k, 1) = "," Or Mid$(nar, k, 1) = ")" Then Exit For
    '                        Next k
    '                        If k = 0 Then k = 35
    '                    Print #1, Spc(18);
    '                    Print #1, "   "; Trim(Left$(nar, k))
    '                        Ln_No = Ln_No + 1
    '                        nar = Right(nar, Len(nar) - k)
    '                    Loop
    '                Print #1, Spc(18);
    '                Print #1, "   "; Trim(nar)
    '                    Ln_No = Ln_No + 1
    '                End If
    '            Print #1,
    '            Ln_No = Ln_No + 1
    '            End If
    '        Next i
    '    Print #1, String(80, 45)
    '    Print #1, Spc(18);
    '    Print #1, Spc(3); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 4)); Spc(29 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 4))));
    '    Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 6)))); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 6));
    '    Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 2, 7)))); Trim(Grid1.TextMatrix(Grid1.Rows - 2, 7))
    '    Print #1, String(80, 45)
    '    Print #1, Spc(18);
    '    Print #1, Spc(3); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 4)); Spc(29 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 4))));
    '    Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 6)))); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 6));
    '    Print #1, Spc(15 - Len(Trim(Grid1.TextMatrix(Grid1.Rows - 1, 7)))); Trim(Grid1.TextMatrix(Grid1.Rows - 1, 7))
    '    Print #1, String(80, 45)
    '        Ln_No = Ln_No + 5
    '        'For k = Ln_No + 1 To 72
    '        '    Print #1, ""
    '        'Next k

    '        Exit Sub

    'Page_Header:
    '        If RptDet.RptCode_Main <> "LEDGER A/C - Confirmation Details" Then
    '            Print #1, Chr(18); Chr(27); "P"; Spc(40 - Len(Trim(Cmp_Name)) / 2); Chr(27); "E"; Trim(Cmp_Name); Chr(27); "F"
    '            Print #1, Chr(27); "M"; Spc(48 - (Len(Trim(Cmp_Address)) / 2)); Trim(Cmp_Address); Chr(27); "P"
    '            Print #1,
    '            Pg_No = Pg_No + 1
    '            Print #1, Spc(40 - (Len(RptHeading1) / 2)); Chr(27); "E"; RptHeading1; Chr(27); "F"
    '            Print #1, Spc(40 - (Len(RptHeading2) / 2)); Chr(27); "E"; RptHeading2; Chr(27); "F"
    '            Print #1, Spc(40 - (Len(RptHeading3) / 2)); Chr(27); "E"; RptHeading3; Chr(27); "F"
    '            Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        End If
    '        Print #1, String(80, 45)
    '        Print #1, "  DATE               PARTICULARS                           DEBIT         CREDIT"
    '        Print #1, String(80, 45)
    '        Ln_No = 10
    '        Return

    'Page_Footer:
    '        Print #1, String(80, 45)
    '        Print #1, Spc(18);
    '        Print #1, "   C/O"; Spc(26);
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr));
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr))
    '        Print #1, String(80, 45)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 4
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '        Print #1, Spc(18);
    '        Print #1, "   B/F"; Spc(26);
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(Tt_Dr)))); Trim(PROC.Currency_Format(Tt_Dr));
    '        Print #1, Spc(15 - Len(Trim(PROC.Currency_Format(tt_cr)))); Trim(PROC.Currency_Format(tt_cr))
    '        Print #1,
    '        Ln_No = Ln_No + 2
    '        Return
    '    End Sub

End Class