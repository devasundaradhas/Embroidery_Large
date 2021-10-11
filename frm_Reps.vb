Public Class frm_Reps
    'Option Explicit
    '    Private AccFormat As String
    '    Private RptDt As Object
    '    Private Rep_Heading1 As String
    '    Private Rep_Heading2 As String
    '    Private Rep_Heading3 As String
    '    Private Heading_1 As String, Heading_2 As String, Condt  As String
    '    Private Field_1 As String, Field_2 As String, Rpt_Hd As String
    '    Private Format_1 As String, Format_2 As String
    '    Private CompType_Condt As String
    '
    'Private Sub Form_Activate()
    '    FrmSts.Status = "New,Save,Print"
    '    If Rpt.Visible Then Rpt.SetFocus
    'End Sub
    '
    'Private Sub Form_Deactivate()
    '    FrmSts.Status = ""
    'End Sub
    '
    'Private Sub Form_KeyUp(KeyCode As Integer, Shift As Integer)
    '    Dim mail_add As String
    '    MDIForm1.MousePointer = 99: Me.MousePointer = 99
    '    If Dir("c:\smartbmp\wait.ico") <> "" Then Me.MouseIcon = LoadPicture("c:\smartbmp\wait.ico")
    '    If Dir("c:\smartbmp\wait.ico") <> "" Then MDIForm1.MouseIcon = LoadPicture("c:\smartbmp\wait.ico")
    '    If Shift = 2 And UCase(Chr(KeyCode)) = "E" Then Rpt.Export_To_Excel
    '    If Shift = 2 And UCase(Chr(KeyCode)) = "M" Then
    '        mail_add = Cmpr.Get_FieldValue(CON, "ledger_head", "Email_Id", "Ledger_Idno = " & Str(RptDet.Idno1))
    '        If Trim(mail_add) = "" Then MgB.Message Others, "Invalid Mail Id", "Outlook Folder", , , Information_ico: Exit Sub
    '        Rpt.Export_To_Mail mail_add
    '    End If
    '    MDIForm1.MousePointer = 0: Me.MousePointer = 0
    'End Sub
    '
    'Private Sub Form_Load()
    '    Dim Rep As String
    '    Dim i As Integer
    '    Dim t1 As Single
    '
    '    License_Information Me
    '
    '    FrmSts.Status = "Print"
    '    FrmSts.Count = FrmSts.Count + 1
    '
    '    Call Change_DosWindows_DefaultPrinter_For_Software(2)
    '    Sleep (100)
    '
    '    Rpt.Host_Name = Cmpr.Remove_NonCharacters(SrvDet.Host) & "_U" & Trim(Val(User.IdNo))
    '    Rpt.Connection_String = Open_DataBaseConnection(CON, CON.DefaultDatabase)
    '
    '    Call Report_Intialize(CON)
    '
    '    If RptDet.RptCode_Sub = "Accounts" Then
    '
    '        Dim acc_rep As New Accounts
    '        acc_rep.Report_Details CON, 0, MDIForm1, Me, Grid1, RptDet.RptCode_Main, RptDet.RptCode_Sub, CmpDet.FnYear, CmpDet.FromDate, CmpDet.ToDate, RptDet.Idno1, RptDet.Idno2, RptDet.Idno3, RptDet.Name1, RptDet.Name2, RptDet.Name3, RptDet.tex_Val1, RptDet.tex_Val2, RptDet.Date1, RptDet.Date2, CompType_Condt
    '        'Set RptDt = GetObject("", "Smart_Report_TexNT10_DLL_8.Accounts")
    '        'RptDt.Report_Details Con, MDIForm1, Me, Grid1, RptDet.RptCode_Main, RptDet.RptCode_Sub, CmpDet.FnYear, CmpDet.FromDate, CmpDet.ToDate, RptDet.Idno1, RptDet.Idno2, RptDet.Idno3, RptDet.Name1, RptDet.Name2, RptDet.Name3, RptDet.tex_Val1, RptDet.tex_Val2, RptDet.Date1, RptDet.Date2, CompType_Condt
    '        Rep_Heading1 = acc_rep.RptHeading1
    '        Rep_Heading2 = acc_rep.RptHeading2
    '        Rep_Heading3 = acc_rep.RptHeading3
    '        FrmSts.Status = "Print"
    '        If RptChg(RptPoint + 1).RowVal > 0 And Grid1.Rows > RptChg(RptPoint + 1).RowVal Then Grid1.Row = RptChg(RptPoint + 1).RowVal
    '        RptChg(RptPoint + 1).RowVal = 0
    '        If RptChg(RptPoint + 1).RowTop > 0 And Grid1.Rows > RptChg(RptPoint + 1).RowTop Then Grid1.TopRow = RptChg(RptPoint + 1).RowTop
    '        RptChg(RptPoint + 1).RowTop = 0
    '        Rpt.Visible = False
    '        Call Smart_Save
    '        Exit Sub
    '
    '    End If
    '
    '    If RptDet.RptCode_Sub = "Bill Details" Then
    '
    '        Me.Height = MDIForm1.Height - 1885
    '        Grid1.Height = Me.Height
    '        Me.Width = MDIForm1.Width - 200
    '        Me.Left = 0: Me.Top = 0
    '        Me.BackColor = RGB(250, 250, 250)
    '
    '        CmPrnt.Heading1 = "": CmPrnt.Heading2 = "": CmPrnt.Heading3 = ""
    '
    '        Grid1.BackColorBkg = Me.BackColor
    '        Grid1.BackColor = RGB(255, 255, 255)
    '        Grid1.ForeColor = RGB(0, 0, 0)
    '        Grid1.ForeColorFixed = RGB(9, 125, 122)
    '        Grid1.BackColorFixed = RGB(230, 230, 230)
    '        Grid1.BackColorSel = RGB(230, 238, 215)
    '        Grid1.ForeColorSel = RGB(3, 10, 245)
    '        Grid1.Rows = 2
    '        Grid1.FixedRows = 1
    '
    '        Select Case RptDet.RptCode_Main
    '            Case "Day Book"
    '                'Call Accounts_DayBook
    '            Case "Single Ledger - Date Wise", "Bank Book", "Cash Book", "Purchase Book", "Sales Book"
    '                'Call Accounts_SingleLedger
    '                MDIForm1.StatusBar4.Panels.Clear
    '                MDIForm1.StatusBar4.Panels.Add , "Day Balance", "F2 - Day Balance"
    '                MDIForm1.StatusBar4.Panels.Item(1).Width = 1630
    '                MDIForm1.StatusBar4.Panels.Item(1).Alignment = sbrCenter
    '                MDIForm1.StatusBar4.Panels.Add , "With Out Particulars", "F3 - With Out Particulars"
    '                MDIForm1.StatusBar4.Panels.Item(2).Width = 2300
    '                MDIForm1.StatusBar4.Panels.Item(2).Alignment = sbrCenter
    '                MDIForm1.StatusBar4.Panels.Add , "Print", "F11 - Print"
    '                MDIForm1.StatusBar4.Panels.Item(3).Width = 1200
    '                MDIForm1.StatusBar4.Panels.Item(3).Alignment = sbrCenter
    '                MDIForm1.StatusBar4.Panels.Add , "Close", "Esc - Close"
    '                MDIForm1.StatusBar4.Panels.Item(4).Width = 1300
    '                MDIForm1.StatusBar4.Panels.Item(4).Alignment = sbrCenter
    '                MDIForm1.StatusBar4.Panels.Add , "Empty", ""
    '                MDIForm1.StatusBar4.Panels.Item(5).Width = 5500
    '                MDIForm1.StatusBar3.Visible = False
    '                MDIForm1.StatusBar1.Visible = False
    '                MDIForm1.StatusBar4.Visible = True
    '                MDIForm1.StatusBar3.Visible = True
    '            Case "Sundry Book"
    '                'Call Accounts_SundryBook
    '            Case "Single Ledger - Month Wise"
    '                'Call Accounts_MonthLedger
    '            Case "Group Ledger"
    '                'Call Accounts_GroupLedger
    '            Case "Opening TB"
    '                'Call Accounts_OpeningTB
    '            Case "General TB"
    '                'Call Accounts_GeneralTB
    '            Case "Group TB"
    '                'Call Accounts_GroupTB
    '            Case "Final TB"
    '                'Call Accounts_FinalTB
    '            Case "Buyer-Seller Ledger"
    '                Call Accounts_BuyerSellerLedger
    '            Case "All Ledger"
    '              'Call Accounts_AllLedger
    '            Case "Bill Balance (Single Party)", "Commission Balance (Single)", "Freight Balance (Single Party)"
    '                Call Bills_Customer_Pending_Single
    '            Case "All Balance (Single Party)"
    '                Call BillsAll_Customer_Pending_Single
    '            Case "Agent BillPending Single"
    '                Call Bills_Agent_Pending_Single
    '            Case "Bill Balance (All Party)", "Commission Balance (All)", "Commission Balance (Buyer)", "Commission Balance (Seller)", "Freight Balance (All Party)", "All Balance (All Party)"
    '                Call Bills_Customer_Pending_All
    '            Case "Bill Balance (Buyer)", "Bill Balance (Seller)"
    '                Call Bills_Customer_Pending_Buyer_Seller
    '            Case "Customer Bill Pending Purchased Bills"
    '                Call Bills_Customer_Pending_Purchased
    '            Case "Customer Bill Pending Invoiced Bills"
    '                Call Bills_Customer_Pending_Invoiced
    '            Case "Customer Bill Pending Invoiced (Cur.Yr)"
    '                Call Bills_Customer_Pending_Invoiced_CurYr
    '            Case "Agent BillPending All"
    '                Call Bills_Agent_Pending_All
    '            Case "Agent BillPending Purchased"
    '                Call Bills_Agent_Pending_Purchased
    '            Case "Agent BillPending Invoiced"
    '                Call Bills_Agent_Pending_Invoiced
    '        End Select
    '
    '            Grid1.RowHeight(0) = 350
    '        Grid1.Row = 0
    '        For i = 0 To Grid1.Cols - 1
    '            Grid1.Col = i
    '            Grid1.CellFontSize = 8
    '            Grid1.CellFontName = "Ms Sans Serif"
    '        Next i
    '        FrmSts.Status = "Print"
    '        MDIForm1.StatusBar3.Panels(1).Text = ""
    '        MDIForm1.StatusBar3.Panels(2).Text = ""
    '        Label1(0).Caption = CmPrnt.Heading1 & " " & IIf(Trim(CmPrnt.Heading2) <> "", "-", "") & " " & CmPrnt.Heading2 & " " & IIf(Trim(CmPrnt.Heading3) <> "", "-", "") & " " & CmPrnt.Heading3
    '        Label1(0).Caption = StrConv(Label1(0).Caption, vbUpperCase)
    '        Shape1.FillColor = RGB(162, 162, 162)
    '        Grid1.Row = 0
    '        For i = 0 To Grid1.Cols - 1
    '            t1 = t1 + Grid1.ColWidth(i)
    '            Grid1.Col = i
    '            Grid1.CellAlignment = 4
    '                Grid1.CellFontBold = True
    '        Next i
    '        If t1 < 11750 Then
    '            Grid1.Width = t1 + 250
    '            Grid1.Left = Int((Me.Width - t1) / 2)
    '        Else
    '            Grid1.Left = 50
    '            Label1(0).Width = 3850
    '        End If
    '        t1 = 0
    '        Me.Height = MDIForm1.Height - 1900
    '        Grid1.Height = Me.Height - 350
    '        Grid1.Row = 1: Grid1.Col = 0
    '        Label1(0).Width = Me.Width
    '        Shape1.Width = Me.Width
    '        MDIForm1.MousePointer = 0
    '        Me.MousePointer = 0
    '        Grid1.Visible = True
    '        Label1(0).Visible = True
    '        Shape1.Visible = True
    '        Rpt.Visible = False
    '        If RptChg(RptPoint + 1).RowVal > 0 Then Grid1.Row = RptChg(RptPoint + 1).RowVal
    '        RptChg(RptPoint + 1).RowVal = 0
    '        If RptChg(RptPoint + 1).RowTop > 0 Then Grid1.TopRow = RptChg(RptPoint + 1).RowTop
    '        RptChg(RptPoint + 1).RowTop = 0
    '
    '        Exit Sub
    '
    '    End If
    '
    '    Rpt.Visible = True
    '    If Trim(Settings.Printer_PaperSize_Nos) <> "" Then
    '        Rpt.PaperSize_Nos = Settings.Printer_PaperSize_Nos
    '    Else
    '        Rpt.PaperSize_Nos = "0,0,0,0"
    '    End If
    '    Rpt.Company_Name = CmpDet.Name
    '    Rpt.Company_Address = CmpDet.Add1 & " " & CmpDet.Add2 & " " & CmpDet.Add3 & " " & CmpDet.Add4
    '    If Settings.Report_Print_Format = "WINDOWS" Then
    '        Rpt.ModeofPrint = Win
    '    Else
    '        Rpt.ModeofPrint = Dos
    '    End If
    '    If Settings.Report_Print_Format = "WINDOWS" Then Rpt.ModeofPrint = Win
    '    Settings.Report_Print_Format = "CR/DR"
    '    If Settings.Amount_Display_Format = "CR/DR" Then AccFormat = "ACC" Else AccFormat = "2"
    '
    '    Rpt.Left = 0
    '    Rpt.Top = 0
    '    Me.Top = 0
    '    Me.Left = 0
    '    Me.Height = MDIForm1.ScaleHeight
    '    Me.Width = MDIForm1.ScaleWidth
    '    Me.BackColor = RGB(245, 255, 251)
    '
    '    Dim v As String
    '    If InStr(LCase(Condt), "company_idno") > 0 Then
    '        i = InStr(LCase(Condt), "company_idno")
    '        v = Right(Condt, Len(Condt) - i + 1)
    '        i = InStr(v, ")")
    '        v = " where " & Left(v, i)
    '    End If
    '    Dim RS As ADODB.Recordset
    '    Set RS = New ADODB.Recordset
    '        RS.Open "Select * from company_head " & v & " order by company_name", CON, adOpenStatic, adLockReadOnly
    '        If Not (RS.BOF And RS.EOF) Then
    '            RS.MoveFirst
    '            Rpt.Company_Name = RS!Company_Name
    '            Rpt.Company_Address = Trim(RS!Company_Address1 & " " & RS!Company_Address2 & " " & RS!Company_Address3 & " " & RS!Company_Address4)
    '        End If
    '        RS.Close
    '    Set RS = Nothing
    '
    '    Select Case RptDet.RptCode_Sub
    '
    '    '===========================================================================================
    '    '                           REGISTERS
    '    '===========================================================================================
    '
    '        Case "Register"
    '
    '            Call Register_Reports
    '
    '    End Select
    '
    'End Sub
    '
    'Private Sub Form_Unload(Cancel As Integer)
    '    FrmSts.Status = ""
    '    FrmSts.Count = FrmSts.Count - 1
    '    MDIForm1.StatusBar4.Visible = False
    '    MDIForm1.StatusBar3.ZOrder 0
    '    MDIForm1.StatusBar1.ZOrder 0
    '    MDIForm1.StatusBar1.Visible = True
    '    MDIForm1.StatusBar3.Visible = False
    '    MDIForm1.StatusBar3.Visible = True
    'End Sub
    '
    'Private Sub Form_KeyPress(KeyAscii As Integer)
    '    If KeyAscii = 27 Then
    '        Unload Me
    '        If RptPoint > 0 Then
    '            RptDet.RptCode_Main = RptChg(RptPoint).RptCode_Main
    '            RptDet.RptCode_Sub = RptChg(RptPoint).RptCode_Sub
    '            RptDet.Name1 = RptChg(RptPoint).Name1
    '            RptDet.Name2 = RptChg(RptPoint).Name2
    '            RptDet.Name3 = RptChg(RptPoint).Name3
    '            RptDet.Idno1 = RptChg(RptPoint).Idno1
    '            RptDet.Idno2 = RptChg(RptPoint).Idno2
    '            RptDet.Date1 = RptChg(RptPoint).Date1
    '            RptDet.Date2 = RptChg(RptPoint).Date2
    '            RptDet.tex_Val1 = RptChg(RptPoint).tex_Val1
    '            RptDet.tex_Val2 = RptChg(RptPoint).tex_Val2
    '            RptPoint = RptPoint - 1
    '            If RptDet.RptCode_Main = "Profit && Loss A/c" Then
    '                Profit_Loss.Show
    '            ElseIf RptDet.RptCode_Main = "Balance Sheet" Then
    '                Balance_Sheet.Show
    '            Else
    '                Me.Show
    '            End If
    '        Else
    '            If RptDet.RptCode_Main <> "Opening TB" And RptDet.RptCode_Main <> "Party Address" And RptDet.RptCode_Main <> "Final TB" And RptDet.Inputs <> "NIL" Then Report_Main.Show
    '        End If
    '    End If
    'End Sub
    '
    '
    'Public Sub Smart_New()
    '    Dim i As Integer, t1 As Integer
    '    Select Case RptDet.RptCode_Sub
    '      Case "Accounts"
    '        Select Case RptDet.RptCode_Main
    '          Case "Single Ledger - Date Wise", "Bank Book", "Cash Book", "Purchase Book", "Sales Book"
    '            If Grid1.ColWidth(7) > 0 Then
    '                Grid1.ColWidth(7) = 0
    '                If Grid1.ColWidth(8) > 0 Then Grid1.ColWidth(8) = 2800
    '                If Grid1.ColWidth(2) > 0 Then Grid1.ColWidth(2) = 3200
    '                If Grid1.ColWidth(3) > 0 Then Grid1.ColWidth(3) = 3200
    '            Else
    '                Grid1.ColWidth(7) = 1500
    '                If Grid1.ColWidth(8) > 0 Then Grid1.ColWidth(8) = 2300
    '                If Grid1.ColWidth(2) > 0 Then Grid1.ColWidth(2) = 2500
    '                If Grid1.ColWidth(3) > 0 Then Grid1.ColWidth(3) = 2500
    '            End If
    '        End Select
    '    End Select
    '    For i = 0 To Grid1.Cols - 1
    '        t1 = t1 + Grid1.ColWidth(i)
    '    Next i
    '    If t1 < MDIForm1.Width Then
    '        Grid1.Width = t1 + 250
    '        Grid1.Left = Int((Me.Width - t1) / 2)
    '    Else
    '        Grid1.Width = Me.Width - 150
    '        Grid1.Left = 50
    '        Label1(0).Width = 3850
    '    End If
    'End Sub
    '
    'Public Sub Smart_Save()
    '    Dim i As Integer, t1 As Integer
    '    Select Case RptDet.RptCode_Sub
    '      Case "Accounts"
    '        Select Case RptDet.RptCode_Main
    '          Case "Single Ledger - Date Wise", "Bank Book", "Cash Book", "Purchase Book", "Sales Book"
    '            If Grid1.ColWidth(2) > 0 Then
    '                Grid1.ColWidth(2) = 0
    '                If Grid1.ColWidth(7) = 0 Then Grid1.ColWidth(3) = 3500 Else Grid1.ColWidth(3) = 2500
    '                Grid1.ColWidth(8) = 0
    '            Else
    '                Grid1.ColWidth(3) = 0
    '                If Grid1.ColWidth(7) = 0 Then Grid1.ColWidth(2) = 3500 Else Grid1.ColWidth(2) = 2500
    '                Grid1.ColWidth(8) = 2300
    '            End If
    '        End Select
    '    End Select
    '    For i = 0 To Grid1.Cols - 1
    '        t1 = t1 + Grid1.ColWidth(i)
    '    Next i
    '    If t1 < MDIForm1.Width - 100 Then
    '        Grid1.Width = t1 + 250
    '        Grid1.Left = Int((Me.Width - t1) / 2)
    '    Else
    '        Grid1.Width = Me.Width - 150
    '        Grid1.Left = 50
    '        Label1(0).Width = 3850
    '    End If
    'End Sub
    '
    '
    'Public Function Smart_Print() As Boolean
    '
    '    If Grid1.Visible Then
    '        Dim acc_rep As New Accounts
    '        acc_rep.RptHeading1 = Rep_Heading1
    '        acc_rep.RptHeading2 = Rep_Heading2
    '        acc_rep.RptHeading3 = Rep_Heading3
    '        Smart_Print = acc_rep.Printing(CON, MDIForm1, Grid1, RptDet.RptCode_Main, RptDet.RptCode_Sub, CmpDet.FnYear, CmpDet.FromDate, CmpDet.ToDate, RptDet.Idno1, RptDet.Idno2, RptDet.Idno3, RptDet.Name1, RptDet.Name2, RptDet.Name3, RptDet.tex_Val1, RptDet.tex_Val2, RptDet.Date1, RptDet.Date2, CmpDet.Name, CmpDet.Add1 & " " & CmpDet.Add2 & " " & CmpDet.Add3 & " " & CmpDet.Add4)
    '        'Set RptDt = GetObject("", "Smart_Report_TexNT9_5_DLL_8.Accounts")
    '        'Smart_Print = RptDt.Printing(Con, MDIForm1, Grid1, RptDet.RptCode_Main, RptDet.RptCode_Sub, CmpDet.FnYear, CmpDet.FromDate, CmpDet.ToDate, RptDet.Idno1, RptDet.Idno2, RptDet.Idno3, RptDet.Name1, RptDet.Name2, RptDet.Name3, RptDet.tex_Val1, RptDet.tex_Val2, RptDet.Date1, RptDet.Date2, CmpDet.Name, CmpDet.Add1 & " " & CmpDet.Add2 & " " & CmpDet.Add3 & " " & CmpDet.Add4)
    '        Set RptDt = Nothing
    '
    '    Else
    '        If Preview_Print = 1 Then
    '            Call Rpt.Export_To_DosFile
    '        Else
    '            Call Rpt.Smart_Print
    '        End If
    '
    '    End If
    'End Function
    '
    'Public Sub Common_Printing(Optional StyleNo As Integer)
    '    Dim PartyAdd As String, Cd1 As String
    '    Dim i As Integer, J As Integer, Left_Margin As Integer, k As Integer
    '    Dim LineNo As Integer, PageNo As Integer, TotSpc As Integer
    '
    '    CmPrnt.CharNo = 45
    '    Print #1, Chr(15); Chr(18);
    '    Print #1, Spc(44 - (Len(CmpDet.Name) / 2)); Chr(27); "E"; CmpDet.Name; Chr(27); "F"
    '    LineNo = 1
    '    PartyAdd = Trim(StrConv((CmpDet.Add1 + " " + CmpDet.Add2 + " " + CmpDet.Add3 + " " + CmpDet.Add4), vbProperCase))
    '    If PartyAdd <> "" Then Print #1, Spc(44 - (Len(PartyAdd) / 2)); PartyAdd: LineNo = LineNo + 1
    '    Print #1,
    '    LineNo = LineNo + 1
    '    If CmPrnt.Heading1 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading1) / 2)); Chr(27); "E"; UCase(CmPrnt.Heading1); Chr(27); "F": LineNo = LineNo + 1
    '    If CmPrnt.Heading2 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading2) / 2)); UCase(CmPrnt.Heading2): LineNo = LineNo + 1
    '    If CmPrnt.Heading3 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading3) / 2)); UCase(CmPrnt.Heading3): LineNo = LineNo + 1
    '    Print #1,
    '    LineNo = LineNo + 1
    '    PageNo = 1
    '
    '    Select Case StyleNo
    '
    '      Case 1
    '
    '        For i = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(i) > 300 Then TotSpc = TotSpc + Grid1.ColData(i) + 1
    '        Next i
    '        TotSpc = TotSpc + 1
    '        If TotSpc < 80 Then Left_Margin = Int((84 - TotSpc) / 2) Else Left_Margin = 0
    '        If TotSpc > 84 Then Print #1, Chr(15); Else Print #1, Chr(15); Chr(18);
    '        Print #1, Spc(Left_Margin); Spc(TotSpc - 10 - Len(Trim(Str(PageNo)))); "Page No : "; Trim(Str(PageNo))
    '
    '        Print #1, Spc(Left_Margin); "";
    '        For J = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(218); Else Print #1, Chr(194);
    '            If Grid1.ColWidth(J) > 300 Then Print #1, Cmpr.CharPrnt(196, Grid1.ColData(J));
    '        Next J
    '        Print #1, Chr(191)
    '
    '        Print #1, Spc(Left_Margin); Chr(179);
    '        For J = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(J) > 300 Then
    '                If Grid1.ColAlignment(J) < 6 Then Print #1, Grid1.TextMatrix(0, J); Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Chr(179); Else Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Grid1.TextMatrix(0, J); Chr(179);
    '            End If
    '        Next J
    '        Print #1,
    '
    '        Print #1, Spc(Left_Margin); "";
    '        For J = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(195); Else Print #1, Chr(197);
    '            If Grid1.ColWidth(J) > 300 Then Print #1, Cmpr.CharPrnt(196, Grid1.ColData(J));
    '        Next J
    '        Print #1, Chr(180)
    '
    '        For i = 2 To Grid1.Rows - 1
    '            If i Mod 54 = 0 Then
    '
    '                Print #1, Spc(Left_Margin); "";
    '                For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(192); Else Print #1, Chr(193);
    '                    If Grid1.ColWidth(J) > 300 Then Print #1, Cmpr.CharPrnt(196, Grid1.ColData(J));
    '                Next J
    '                Print #1, Chr(217)
    '                Print #1, Spc(Left_Margin); Spc(TotSpc - 8); "Contd..."
    '                Print #1, Chr(12)
    '
    '                Print #1, Chr(15); Chr(18);
    '                Print #1, Spc(44 - (Len(CmpDet.Name) / 2)); Chr(27); "E"; CmpDet.Name; Chr(27); "F"
    '                LineNo = 3
    '                PartyAdd = Trim(StrConv((CmpDet.Add1 + " " + CmpDet.Add2 + " " + CmpDet.Add3 + " " + CmpDet.Add4), vbProperCase))
    '                If PartyAdd <> "" Then Print #1, Spc(44 - (Len(PartyAdd) / 2)); PartyAdd: LineNo = LineNo + 1
    '                Print #1,
    '                If CmPrnt.Heading1 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading1) / 2)); Chr(27); "E"; UCase(CmPrnt.Heading1); Chr(27); "F": LineNo = LineNo + 1
    '                If CmPrnt.Heading2 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading2) / 2)); UCase(CmPrnt.Heading2): LineNo = LineNo + 1
    '                If CmPrnt.Heading3 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading3) / 2)); UCase(CmPrnt.Heading3): LineNo = LineNo + 1
    '                Print #1,
    '
    '                If TotSpc > 84 Then Print #1, Chr(15); Else Print #1, Chr(15); Chr(18);
    '                PageNo = PageNo + 1
    '                Print #1, Spc(Left_Margin); Spc(TotSpc - 10 - Len(Trim(Str(PageNo)))); "Page No : "; Trim(Str(PageNo))
    '
    '                Print #1, Spc(Left_Margin); "";
    '                For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(218); Else Print #1, Chr(194);
    '                    If Grid1.ColWidth(J) > 300 Then Print #1, Cmpr.CharPrnt(196, Grid1.ColData(J));
    '                Next J
    '                Print #1, Chr(191)
    '
    '                Print #1, Spc(Left_Margin); Chr(179);
    '                For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then
    '                        If Grid1.ColAlignment(J) < 6 Then Print #1, Grid1.TextMatrix(0, J); Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Chr(179); Else Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Grid1.TextMatrix(0, J); Chr(179);
    '                    End If
    '                Next J
    '                Print #1,
    '
    '                Print #1, Spc(Left_Margin); "";
    '                For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(195); Else Print #1, Chr(197);
    '                    If Grid1.ColWidth(J) > 300 Then Print #1, Cmpr.CharPrnt(196, Grid1.ColData(J));
    '                Next J
    '                Print #1, Chr(180)
    '
    '            End If
    '
    '            If Grid1.RowData(i) = "1" Then
    '                Print #1, Spc(Left_Margin); "";
    '                For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(195); Else Print #1, Chr(197);
    '                    If Grid1.ColWidth(J) > 300 Then Print #1, Cmpr.CharPrnt(196, Grid1.ColData(J));
    '                Next J
    '                Print #1, Chr(180)
    '            End If
    '            Print #1, Spc(Left_Margin); Chr(179);
    '            For J = 0 To Grid1.Cols - 1
    '                If Grid1.ColWidth(J) > 300 Then
    '                    If Grid1.ColAlignment(J) < 6 Then
    '                        Cd1 = Trim(Left$(Grid1.TextMatrix(i, J), (Val(Grid1.ColData(J)))))
    '                        Print #1, Cd1; Spc(Val(Grid1.ColData(J)) - Len(Cd1)); Chr(179);
    '                    Else
    '                        Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(i, J))); Left$(Grid1.TextMatrix(i, J), Val(Grid1.ColData(J))); Chr(179);
    '                    End If
    '                End If
    '            Next J
    '            Print #1,
    '        Next i
    '
    '        Print #1, Spc(Left_Margin); "";
    '        For J = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(J) > 300 Then If J = 0 Then Print #1, Chr(192); Else Print #1, Chr(193);
    '            If Grid1.ColWidth(J) > 300 Then Print #1, Cmpr.CharPrnt(196, Grid1.ColData(J));
    '        Next J
    '        Print #1, Chr(217)
    '
    '      Case Else
    '
    '        For i = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(i) > 300 Then
    '                TotSpc = TotSpc + Grid1.ColData(i)
    '                If i <> Grid1.Cols - 1 And Grid1.ColAlignment(i) > 5 Then TotSpc = TotSpc + 2
    '            End If
    '        Next i
    '
    '        If TotSpc > 84 Then Print #1, Chr(15); Else Print #1, Chr(15); Chr(18);
    '        Print #1, Spc(TotSpc - 10 - Len(Trim(Str(PageNo)))); "Page No : "; Trim(Str(PageNo))
    '        LineNo = LineNo + 1
    '        Print #1, Cmpr.CharPrnt(CmPrnt.CharNo, TotSpc)
    '        Print #1, "";
    '        For J = 0 To Grid1.Cols - 1
    '            If Grid1.ColWidth(J) > 300 Then
    '                If Grid1.ColAlignment(J) < 6 Then Print #1, Grid1.TextMatrix(0, J); Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Else Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Grid1.TextMatrix(0, J); Spc(IIf(J <> Grid1.Cols - 1, 2, 0));
    '            End If
    '        Next J
    '        Print #1,
    '        Print #1, Cmpr.CharPrnt(CmPrnt.CharNo, TotSpc)
    '        LineNo = LineNo + 3
    '        For i = 2 To Grid1.Rows - 1
    '
    '            If LineNo >= 60 Then
    '
    '                Print #1, Cmpr.CharPrnt(CmPrnt.CharNo, TotSpc)
    '                Print #1, Spc(TotSpc - 8); "Contd..."
    '                For k = LineNo + 3 To 72
    '                    Print #1, 'Chr(12)
    '                Next k
    '
    '                Print #1, Chr(15); Chr(18);
    '                Print #1, Spc(44 - (Len(CmpDet.Name) / 2)); Chr(27); "E"; CmpDet.Name; Chr(27); "F"
    '                LineNo = 1
    '                PartyAdd = Trim(StrConv((CmpDet.Add1 + " " + CmpDet.Add2 + " " + CmpDet.Add3 + " " + CmpDet.Add4), vbProperCase))
    '                If PartyAdd <> "" Then Print #1, Spc(44 - (Len(PartyAdd) / 2)); PartyAdd: LineNo = LineNo + 1
    '                Print #1,
    '                LineNo = LineNo + 1
    '                If CmPrnt.Heading1 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading1) / 2)); Chr(27); "E"; UCase(CmPrnt.Heading1); Chr(27); "F": LineNo = LineNo + 1
    '                If CmPrnt.Heading2 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading2) / 2)); UCase(CmPrnt.Heading2): LineNo = LineNo + 1
    '                If CmPrnt.Heading3 <> "" Then Print #1, Spc(44 - (Len(CmPrnt.Heading3) / 2)); UCase(CmPrnt.Heading3): LineNo = LineNo + 1
    '                Print #1,
    '                LineNo = LineNo + 1
    '                If TotSpc > 84 Then Print #1, Chr(15); Else Print #1, Chr(15); Chr(18);
    '                PageNo = PageNo + 1
    '                Print #1, Spc(TotSpc - 10 - Len(Trim(Str(PageNo)))); "Page No : "; Trim(Str(PageNo))
    '                LineNo = LineNo + 1
    '                Print #1, Cmpr.CharPrnt(CmPrnt.CharNo, TotSpc)
    '                Print #1, "";
    '                For J = 0 To Grid1.Cols - 1
    '                    If Grid1.ColWidth(J) > 300 Then
    '                        If Grid1.ColAlignment(J) < 6 Then Print #1, Grid1.TextMatrix(0, J); Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Else Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(0, J))); Grid1.TextMatrix(0, J); Spc(IIf(J <> Grid1.Cols - 1, 2, 0));
    '                    End If
    '                Next J
    '                Print #1,
    '                Print #1, Cmpr.CharPrnt(CmPrnt.CharNo, TotSpc)
    '                LineNo = LineNo + 3
    '            End If
    '
    '            If Grid1.RowData(i) = 1 Or Grid1.RowData(i) = 3 Then
    '                Print #1, Cmpr.CharPrnt(CmPrnt.CharNo, TotSpc)
    '                LineNo = LineNo + 1
    '            End If
    '            Print #1, "";
    '            For J = 0 To Grid1.Cols - 1
    '                If Grid1.ColWidth(J) > 300 Then
    '                    If Grid1.ColAlignment(J) < 6 Then
    '                        Cd1 = Trim(Left$(Grid1.TextMatrix(i, J), (Val(Grid1.ColData(J)) - 2)))
    '                        Print #1, Cd1; Spc(Val(Grid1.ColData(J)) - Len(Cd1));
    '                    Else
    '                        Print #1, Spc(Val(Grid1.ColData(J)) - Len(Grid1.TextMatrix(i, J))); Left$(Grid1.TextMatrix(i, J), Val(Grid1.ColData(J))); Spc(IIf(J <> Grid1.Cols - 1, 2, 0));
    '                    End If
    '                End If
    '            Next J
    '            Print #1,
    '            LineNo = LineNo + 1
    '            If Grid1.RowData(i) = 2 Or Grid1.RowData(i) = 3 Then
    '                Print #1, Cmpr.CharPrnt(CmPrnt.CharNo, TotSpc)
    '                LineNo = LineNo + 1
    '            End If
    '        Next i
    '
    '    End Select
    '
    '    Print #1, Chr(18)
    '
    'End Sub
    '
    '
    'Private Sub Grid1_Click()
    '    Dim mo1 As Integer, yr1 As Integer
    '
    '    If Not (RptDet.RptCode_Main = "Single Ledger - Date Wise" Or RptDet.RptCode_Main = "Bank Book" Or RptDet.RptCode_Main = "Cash Book" Or RptDet.RptCode_Main = "Purchase Book" Or RptDet.RptCode_Main = "Sales Book" Or RptDet.RptCode_Main = "Day Book" Or RptDet.RptCode_Main = "Sundry Book" Or RptDet.RptCode_Main = "Single Ledger - Month Wise" Or RptDet.RptCode_Main = "General TB" Or RptDet.RptCode_Main = "Group TB" Or RptDet.RptCode_Main = "Group Ledger") Then Exit Sub
    '    If (RptDet.RptCode_Main = "Single Ledger - Date Wise" Or RptDet.RptCode_Main = "Sundry Book" Or RptDet.RptCode_Main = "Day Book") And Val(Grid1.TextMatrix(Grid1.Row, 1)) = 0 Then Exit Sub
    '    If RptDet.RptCode_Main = "Single Ledger - Month Wise" And Grid1.Row < 2 Then Exit Sub
    '    If (RptDet.RptCode_Main = "General TB" Or RptDet.RptCode_Main = "Group TB" Or RptDet.RptCode_Main = "Group Ledger") And Grid1.RowData(Grid1.Row) = 0 Then Exit Sub
    '
    '    RptPoint = RptPoint + 1
    '    RptChg(RptPoint).RptCode_Main = RptDet.RptCode_Main
    '    RptChg(RptPoint).RptCode_Sub = RptDet.RptCode_Sub
    '    RptChg(RptPoint).Name1 = RptDet.Name1
    '    RptChg(RptPoint).Name2 = RptDet.Name2
    '    RptChg(RptPoint).Name3 = RptDet.Name3
    '    RptChg(RptPoint).Idno1 = RptDet.Idno1
    '    RptChg(RptPoint).Idno2 = RptDet.Idno2
    '    RptChg(RptPoint).Date1 = RptDet.Date1
    '    RptChg(RptPoint).Date2 = RptDet.Date2
    '    RptChg(RptPoint).tex_Val1 = RptDet.tex_Val1
    '    RptChg(RptPoint).tex_Val2 = RptDet.tex_Val2
    '    RptChg(RptPoint).RowVal = Grid1.Row
    '    RptChg(RptPoint).RowTop = Grid1.TopRow
    '
    '    Select Case RptDet.RptCode_Main
    '        Case "Day Book"
    '            RptChg(RptPoint).VouNo = Val(Grid1.TextMatrix(Grid1.Row, 1))
    '            Voucher_Type = Trim(Grid1.TextMatrix(Grid1.Row, 3))
    '            Unload Me
    '            Voucher_Entry.Show
    '        Case "Single Ledger - Date Wise", "Bank Book", "Cash Book", "Purchase Book", "Sales Book", "Sundry Book"
    '            RptChg(RptPoint).VouNo = Val(Grid1.TextMatrix(Grid1.Row, 1))
    '            Voucher_Type = Trim(Grid1.TextMatrix(Grid1.Row, 4))
    '            Unload Me
    '            Voucher_Entry.Show
    '        Case "Single Ledger - Month Wise"
    '            RptDet.RptCode_Main = "Single Ledger - Date Wise"
    '            mo1 = IIf(Grid1.Row <= 10, Val(Grid1.Row + 2), Grid1.Row - 10)
    '            yr1 = IIf(Grid1.Row <= 10, Year(CmpDet.FromDate), Year(CmpDet.ToDate))
    '            RptDet.Date1 = Format("01/" & Trim(Str(mo1)) & "/" & Str(yr1), "dd/mm/yyyy")
    '            RptDet.Date2 = Format("01-" & Trim(Str(mo1 + 1)) & "/" & Str(yr1), "dd/mm/yyyy")
    '            RptDet.Date2 = RptDet.Date2 - 1
    '            Unload Me
    '            Me.Show
    '        Case "General TB", "Group TB", "Group Ledger"
    '            RptDet.RptCode_Main = "Single Ledger - Date Wise"
    '            RptDet.Name1 = Trim(Grid1.TextMatrix(Grid1.Row, 0))
    '            RptDet.Idno1 = Val(Grid1.RowData(Grid1.Row))
    '            RptDet.Date2 = RptDet.Date1
    '            RptDet.Date1 = CmpDet.FromDate
    '            Unload Me
    '            Me.Show
    '        Case Else
    '            RptPoint = RptPoint - 1
    '    End Select
    'End Sub
    '
    'Private Sub Grid1_RowColChange()
    '    If Grid1.Visible Then MDIForm1.StatusBar3.Panels(2).Text = Grid1.Text
    'End Sub
    '
    'Private Sub GRID1_KeyDown(KeyCode As Integer, Shift As Integer)
    '    If KeyCode = 13 Then Call Grid1_Click
    'End Sub
    '
    '
    '' START  ***********************      ACCOUNTS     ***********************
    '
    'Private Sub Accounts_BuyerSellerLedger()
    '    Dim Rs1 As Recordset, Rt1 As Recordset
    '    Dim Ttc As Currency, Ttd As Currency
    '    Dim dt_cndt As String
    '
    '    Grid1.FormatString = "<DATE          |<VOU NO |<PARTICULARS                                           |<PARTICULARS                                           |<TYPE   |>DB.AMOUNT       |>CR.AMOUNT       |>BALANCE             |<NARRATION                                  "
    '    Grid1.ColWidth(2) = 3000: Grid1.ColWidth(3) = 2000: Grid1.ColWidth(7) = 1800: Grid1.ColWidth(8) = 2800
    '    CmPrnt.Heading1 = "LEDGER : " & Trim(RptDet.Name1)
    '    CmPrnt.Heading2 = "RANGE : " & Format(RptDet.Date1, "dd-mm-yyyy") & " TO " & Format(RptDet.Date2, "dd-mm-yyyy")
    '
    '    Set Rs1 = New ADODB.Recordset
    '        Rs1.Open "select sum(case when CrDr_Type='Cr' then bill_amount else -1*bill_amount end) from voucher_bill_head where Ledger_Idno = " & Str(RptDet.Idno1) & " and Agent_Idno = " & Str(RptDet.Idno2) & " and voucher_bill_date < '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "'", CON, adOpenStatic, adLockReadOnly
    '        If Rs1(0).Value <> "" Then Ttc = Val(Rs1(0).Value)
    '        Rs1.Close
    '    Set Rs1 = Nothing
    '
    '    If Ttc <> 0 Then Grid1.AddItem "" & vbTab & vbTab & "   OPENING BALANCE" & vbTab & "   OPENING BALANCE" & vbTab & "" & vbTab & IIf(Ttc < 0, "", vbTab) & Cmpr.Currency_Format(Abs(Ttc))
    '    If Ttc < 0 Then Ttd = Abs(Ttc): Ttc = 0
    '    Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '    Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '
    '    Set Rs1 = New ADODB.Recordset
    '        Rs1.Open "select a.voucher_date, a.voucher_amount, b.voucher_no, b.voucher_type, c.ledger_name as cre_name, d.ledger_name as deb_name, a.narration from voucher_details a, voucher_head b, ledger_head c, ledger_head d where a.Company_idno in " & Trim(RptInp(0).Value) & " and a.Company_idno = b.Company_idno and a.ledger_idno in " & Trim(RptInp(1).Value) & " and ( b.creditor_idno in " & Trim(RptInp(1).Value) & " or b.debtor_idno in " & Trim(RptInp(1).Value) & " ) and a.voucher_date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and a.voucher_ref_no = b.voucher_ref_no and b.creditor_idno = c.ledger_idno and b.debtor_idno = d.ledger_idno order by a.voucher_date, b.for_orderby", CON, adOpenStatic, adLockReadOnly
    '        With Rs1
    '            If Not (.BOF And .EOF) Then
    '                .MoveFirst
    '                Do While Not .EOF
    '                    If Trim(Format(!Voucher_Date, "dd-mm-yy")) = Trim(Grid1.TextMatrix(Grid1.Rows - 1, 0)) Then Grid1.TextMatrix(Grid1.Rows - 1, 7) = ""
    '                    If !Voucher_Amount > 0 Then Ttc = Ttc + Val(!Voucher_Amount) Else Ttd = Ttd + Abs(Val(!Voucher_Amount))
    '                    Grid1.AddItem Format(!Voucher_Date, "dd-mm-yy") & Chr(9) & Val(!Voucher_No) & Chr(9) & IIf(!Voucher_Amount > 0, "By " & Trim(!deb_name), "To " & Trim(!cre_name)) & Chr(9) & Trim(StrConv(!Narration, vbProperCase)) & Chr(9) & Trim(!Voucher_Type) & Chr(9) & IIf(!Voucher_Amount < 0, Cmpr.Currency_Format(Abs(!Voucher_Amount)) & vbTab, vbTab & Cmpr.Currency_Format(Abs(!Voucher_Amount))) & Chr(9) & Cmpr.Currency_Format(Abs(Ttc - Ttd)) & IIf(Ttc > Ttd, " Cr", " Dr") & Chr(9) & Trim(!Narration)
    '                    MDIForm1.StatusBar3.Panels(2).Text = Format(!Voucher_Date, "dd mmm")
    '                    .MoveNext
    '                Loop
    '            End If
    '            .Close
    '        End With
    '    Set Rt1 = Nothing
    '    Grid1.AllowUserResizing = flexResizeNone
    '    Grid1.ColWidth(3) = 0: Grid1.ColWidth(7) = 0
    '    Grid1.AddItem ""
    '    Grid1.AddItem "" & Chr(9) & "" & Chr(9) & "TOTAL" & Chr(9) & "TOTAL" & Chr(9) & "" & Chr(9) & Cmpr.Currency_Format(Ttd) & Chr(9) & Cmpr.Currency_Format(Ttc)
    '    Grid1.AddItem "" & Chr(9) & "" & Chr(9) & "CLOSING BALANCE" & Chr(9) & "CLOSING BALANCE" & Chr(9) & "" & Chr(9) & IIf(Ttc - Ttd < 0, "", vbTab) & Cmpr.Currency_Format(Abs(Ttc - Ttd))
    '    Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '    Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    'End Sub
    '
    'Private Sub Bills_Customer_Details_Single()
    '    Dim RS As ADODB.Recordset
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim rf As String
    '    Dim amt As Currency
    '    CmPrnt.Heading1 = RptDet.RptCode_Main
    '    CmPrnt.Heading2 = "Name : " & RptDet.Name1
    '    CmPrnt.Heading3 = "Range : " & RptDet.Date1 & " To " & RptDet.Date2
    '    Grid1.Cols = 10: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<BL.DATE      |<BL.NO        |>AMOUNT                |<         |<VOU.NO   |<VOU.DATE     |<NARRATION                 |>AMOUNT           |>BALANCE         |<        "
    '    Grid1.ColData(0) = 10: Grid1.ColData(1) = 7: Grid1.ColData(2) = 12: Grid1.ColData(3) = 5: Grid1.ColData(4) = 6: Grid1.ColData(5) = 10: Grid1.ColData(6) = 15: Grid1.ColData(7) = 10: Grid1.ColData(8) = 10: Grid1.ColData(9) = 5
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select a.amount, b.voucher_bill_no, b.voucher_bill_date, b.bill_amount, b.party_bill_no, b.crdr_type, d.voucher_no, c.voucher_date, c.narration from voucher_bill_details a, voucher_bill_head b, voucher_details c, voucher_head d where a.ledger_idno = " & Str(RptDet.Idno1) & " and a.voucher_bill_date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and c.ledger_idno = b.ledger_idno and a.entry_identification = 'VOUCH-'+d.voucher_ref_no and c.voucher_ref_no = d.voucher_ref_no order by b.voucher_bill_date, b.voucher_bill_no, a.voucher_bill_date", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '
    '                If rf <> !voucher_bill_no Then
    '                    amt = !bill_amount
    '                    Grid1.AddItem Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(!bill_amount) & Chr(9) & !crdr_type & Chr(9) & !Voucher_No & Chr(9) & Format(!Voucher_Date, "dd/mm/yy") & Chr(9) & !Narration & Chr(9) & Cmpr.Currency_Format(!Amount) & Chr(9) & Cmpr.Currency_Format(amt - !Amount) & Chr(9) & !crdr_type
    '                Else
    '                    Grid1.AddItem Chr(9) & Chr(9) & Chr(9) & Chr(9) & !Voucher_No & Chr(9) & Format(!Voucher_Date, "dd/mm/yy") & Chr(9) & !Narration & Chr(9) & Cmpr.Currency_Format(!Amount) & Chr(9) & Cmpr.Currency_Format(amt - !Amount) & Chr(9) & !crdr_type
    '                End If
    '                rf = !voucher_bill_no
    '                amt = amt - !Amount
    '                .MoveNext
    '            Loop
    '            Grid1.RowData(Grid1.Rows - 1) = 2
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    'End Sub
    '
    'Private Sub Bills_Agent_Details_Single()
    '    Dim RS As ADODB.Recordset
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim rf As String
    '    Dim amt As Currency
    '    CmPrnt.Heading1 = RptDet.RptCode_Main
    '    CmPrnt.Heading2 = "Name : " & RptDet.Name1
    '    CmPrnt.Heading3 = "Range : " & RptDet.Date1 & " To " & RptDet.Date2
    '    Grid1.Cols = 11: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<PARTY NAME                       |<BL.DATE      |<BL.NO        |>AMOUNT                |<         |<VOU.NO   |<VOU.DATE     |<NARRATION                 |>AMOUNT           |>BALANCE         |<        "
    '    Grid1.ColData(0) = 35: Grid1.ColData(1) = 10: Grid1.ColData(2) = 7: Grid1.ColData(3) = 12: Grid1.ColData(4) = 5: Grid1.ColData(5) = 6: Grid1.ColData(6) = 10: Grid1.ColData(7) = 15: Grid1.ColData(8) = 10: Grid1.ColData(9) = 10: Grid1.ColData(10) = 5
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select a.amount, b.voucher_bill_no, b.voucher_bill_date, b.bill_amount, b.party_bill_no, b.crdr_type, d.voucher_no, c.voucher_date, c.narration, e.ledger_name from voucher_bill_details a, voucher_bill_head b, voucher_details c, voucher_head d, ledger_head e where a.agent_idno = " & Str(RptDet.Idno1) & " and a.voucher_bill_date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no and c.ledger_idno = b.ledger_idno and a.entry_identification = 'VOUCH-'+d.voucher_ref_no and c.voucher_ref_no = d.voucher_ref_no order by b.voucher_bill_date, b.voucher_bill_no, a.voucher_bill_date", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '
    '                If rf <> !voucher_bill_no Then
    '                    amt = !bill_amount
    '                    Grid1.AddItem Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(!bill_amount) & Chr(9) & !crdr_type & Chr(9) & !Voucher_No & Chr(9) & Format(!Voucher_Date, "dd/mm/yy") & Chr(9) & !Narration & Chr(9) & Cmpr.Currency_Format(!Amount) & Chr(9) & Cmpr.Currency_Format(amt - !Amount) & Chr(9) & !crdr_type
    '                Else
    '                    Grid1.AddItem Chr(9) & Chr(9) & Chr(9) & Chr(9) & !Voucher_No & Chr(9) & Format(!Voucher_Date, "dd/mm/yy") & Chr(9) & !Narration & Chr(9) & Cmpr.Currency_Format(!Amount) & Chr(9) & Cmpr.Currency_Format(amt - !Amount) & Chr(9) & !crdr_type
    '                End If
    '                rf = !voucher_bill_no
    '                amt = amt - !Amount
    '                .MoveNext
    '            Loop
    '            Grid1.RowData(Grid1.Rows - 1) = 2
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    'End Sub
    '
    'Private Sub Bills_Customer_Pending_Single()
    '
    '    CON.Execute "truncate table reporttemp"
    '    CON.Execute "insert into reporttemp( int1, int2, name1, currency1 ) Select tp.company_idno, ledger_idno, voucher_bill_no, sum(amount) from voucher_bill_details tp, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and tp.company_idno = tz.company_idno group by tp.company_idno, ledger_idno, voucher_bill_no"
    '    CON.Execute "insert into reporttemp ( int1, int2, name1, currency1 ) Select tp.company_idno, ledger_idno, voucher_bill_no, 0 from voucher_bill_head tp, company_head tz where " & Condt & IIf(Condt <> "", " and ", "") & " voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and tp.company_idno = tz.company_idno group by tp.company_idno, ledger_idno, voucher_bill_no"
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( smallint_1, smallint_2, text_1, amount_1 ) Select int1, int2, name1, sum(currency1) from reporttemp group by int1, int2, name1"
    '
    '    Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select " & Field_1 & " d.ledger_name as mill_name, a.party_bill_no, a.voucher_bill_date, " & Field_2 & " count_name, noof_bags, rate, abs(credit_amount-debit_amount) as balance, a.crdr_type, datediff(day,a.voucher_bill_date,getdate()) as days from voucher_bill_head a, reporttemp_simple b, company_head tz, ledger_head tp, ledger_head d where " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and (a.bill_amount- (case when b.amount_1 is null then 0 else b.amount_1 end)) <> 0" _
    '                    & " and a.voucher_bill_no = b.text_1 and a.company_idno = b.smallint_1 and a.ledger_idno = tp.ledger_idno and a.company_idno = tz.company_idno and a.mill_idno *= d.ledger_idno order by a.voucher_bill_date, a.voucher_bill_no ", Heading_1 & "<[LEN25]MILL/PARTY      |<[LEN8]BILL NO |<=[LEN9]DATE |" & Heading_2 & "<COUNT     |>BAGS   |>RATE      |>[LEN14]BALANCE |<[LEN3]C/D|>DAYS", Rpt_Hd, Me, Format_1 & "|DD-MM-YY|" & Format_2 & "CUR2|CUR2||CUR2", "TOTAL|1~BALANCE", , , , [Draft 12cpi]
    '
    '    Exit Sub
    '
    '    Dim RS As ADODB.Recordset
    '    Dim p_bal As Currency, t_p_bal As Currency, cr_amt As Currency, db_amt As Currency
    '    Dim condt_1 As String, ldnm As String
    '    Dim p_sno As Integer
    '
    '    CmPrnt.Heading1 = RptDet.RptCode_Main
    '    CmPrnt.Heading2 = "party name : " & RptDet.Name1
    '    CmPrnt.Heading3 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 9
    '    Grid1.FormatString = "<MILL/PARTY                               |<BL.DATE    |<BL.NO  |<COUNT     |>BAGS   |>RATE      |>BALANCE             |>TOTAL                     |>DAYS    "
    '    Grid1.ColData(0) = 30: Grid1.ColData(1) = 10: Grid1.ColData(2) = 7: Grid1.ColData(3) = 8: Grid1.ColData(4) = 5: Grid1.ColData(5) = 7: Grid1.ColData(6) = 13: Grid1.ColData(7) = 13: Grid1.ColData(8) = 5
    '
    '    If RptDet.RptCode_Main = "Bill Balance (Single Party)" Then
    '        condt_1 = " and a.bill_type = '' "
    '    ElseIf RptDet.RptCode_Main = "Commission Balance (Single)" Then
    '        condt_1 = " and a.bill_type = 'C' "
    '    ElseIf RptDet.RptCode_Main = "Freight Balance (Single Party)" Then
    '        condt_1 = " and a.bill_type = 'F' "
    '    Else
    '        condt_1 = ""
    '    End If
    '
    '    CON.Execute "truncate table reporttemp"
    '    CON.Execute "insert into reporttemp ( name1, currency1 ) select voucher_bill_no, sum(amount) from voucher_bill_details where ledger_idno = " & Str(RptDet.Idno1) & " and voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' group by voucher_bill_no"
    '    CON.Execute "insert into reporttemp ( name1, currency1 ) select voucher_bill_no, 0 from voucher_bill_head where ledger_idno = " & Str(RptDet.Idno1) & " and voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "'"
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select name1, sum(currency1) from reporttemp group by name1"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, c.ledger_name, a.count_name, noof_bags, a.bill_amount, a.crdr_type, b.amount_1 as amount, rate from voucher_bill_head a, reporttemp_simple b, ledger_head c where a.ledger_idno = " & Str(RptDet.Idno1) & " and a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' " & Condt & " and a.bill_amount <> b.amount_1 and a.voucher_bill_no = b.text_1 and a.mill_idno *= c.ledger_idno order by c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            p_sno = 1
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                If cr_amt <> db_amt Then
    '                    Grid1.AddItem IIf(ldnm <> !ledger_name, p_sno & ". " & !ledger_name, "") & Chr(9) & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & !Count_Name & Chr(9) & IIf(!Noof_Bags > 0, !Noof_Bags, "") & Chr(9) & !Rate & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & IIf(db_amt > cr_amt, " Db", " Cr") & Chr(9) & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                    p_bal = p_bal + cr_amt - db_amt
    '                    If !ledger_name <> "" Then ldnm = !ledger_name
    '                End If
    '                .MoveNext
    '                If .EOF Then
    '                    Grid1.TextMatrix(Grid1.Rows - 1, 7) = Cmpr.Currency_Format(Abs(p_bal)) & " " & IIf(p_bal > 0, "Cr", "Dr")
    '                    t_p_bal = t_p_bal + p_bal
    '                    p_bal = 0
    '                ElseIf !ledger_name <> ldnm Then
    '                    Grid1.TextMatrix(Grid1.Rows - 1, 7) = Cmpr.Currency_Format(Abs(p_bal)) & " " & IIf(p_bal > 0, "Cr", "Dr")
    '                    Grid1.AddItem ""
    '                    t_p_bal = t_p_bal + p_bal
    '                    p_bal = 0: p_sno = p_sno + 1
    '                End If
    '            Loop
    '            Grid1.AddItem ""
    '            Grid1.AddItem "" & Chr(9) & "Total" & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Chr(9) & Cmpr.Currency_Format(Abs(t_p_bal)) & " " & IIf(t_p_bal > 0, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    'End Sub
    '
    'Private Sub BillsAll_Customer_Pending_Single()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim condt_1 As String
    '
    '    CmPrnt.Heading1 = RptDet.RptCode_Main
    '    CmPrnt.Heading2 = "party name : " & RptDet.Name1
    '    CmPrnt.Heading3 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 8: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<BL.DATE    |<BL.NO  |<MILL/PARTY          |>CR.AMOUNT       |>DR.AMOUNT       |>BALANCE             |<      |>DAYS    "
    '    Grid1.ColData(0) = 10: Grid1.ColData(1) = 7: Grid1.ColData(2) = 20: Grid1.ColData(3) = 13: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 5: Grid1.ColData(7) = 5
    '
    '    If RptDet.RptCode_Main = "Bill Balance (Single Party)" Then
    '        condt_1 = " and a.bill_type = '' "
    '    ElseIf RptDet.RptCode_Main = "Commission Balance (Single)" Then
    '        condt_1 = " and a.bill_type = 'C' "
    '    ElseIf RptDet.RptCode_Main = "Freight Balance (Single Party)" Then
    '        condt_1 = " and a.bill_type = 'F' "
    '    Else
    '        condt_1 = ""
    '    End If
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( smallint_1, smallint_2, text_1, amount_1 ) Select tz.company_idno, ledger_idno, voucher_bill_no, sum(amount) from voucher_bill_details tp, company_head tz  where " & Replace(Condt, "tZ.", "tP.") & IIf(Condt <> "", " and ", "") & " voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and tp.company_idno = tz.company_idno group by tz.company_idno, ledger_idno, voucher_bill_no"
    '    'Con.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, sum(amount) from voucher_bill_details where ledger_idno = " & Str(RptDet.Idno1) & " and voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' group by voucher_bill_no"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        '.Open "select a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, c.ledger_name, count_name, noof_bags, a.bill_amount, a.crdr_type, b.amount_1 as amount from voucher_bill_head a, reporttemp_simple b, ledger_head c where a.ledger_idno = " & Str(RptDet.Idno1) & " and a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' " & Condt & " and a.voucher_bill_no *= b.text_1 and a.mill_idno *= c.ledger_idno order by a.voucher_bill_date, a.voucher_bill_no", Con
    '        .Open "Select tz.company_shortname, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, 0 as noof_bags, a.bill_amount, a.crdr_type, b.amount_1 as amount, d.ledger_name from voucher_bill_head a, reporttemp_simple b, ledger_head tp, company_head tz, ledger_head d where " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' " & condt_1 & " and a.voucher_bill_no *= b.text_1 and a.company_idno *= b.smallint_1 and a.company_idno = tz.company_idno and a.ledger_idno = tp.ledger_idno and a.agent_idno *= d.ledger_idno order by tp.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON, adOpenStatic, adLockReadOnly
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                If cr_amt <> db_amt Then
    '                    Grid1.AddItem Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & !ledger_name & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                    tt_cr = tt_cr + cr_amt
    '                    tt_db = tt_db + db_amt
    '                End If
    '                .MoveNext
    '            Loop
    '            Grid1.AddItem ""
    '            Grid1.AddItem "" & Chr(9) & "Total" & Chr(9) & Chr(9) & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    'End Sub
    '
    'Private Sub Bills_Agent_Pending_Single()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim P_Name As String
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '
    '
    '    CmPrnt.Heading1 = RptDet.RptCode_Main
    '    CmPrnt.Heading2 = "party name : " & RptDet.Name1
    '    CmPrnt.Heading3 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 8: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<PARTY NAME                       |<BILL DATE        |<BILL NO             |>CR.AMOUNT                |>DR.AMOUNT                |>BALANCE                    |<      |>DAYS    "
    '    Grid1.ColData(0) = 37: Grid1.ColData(1) = 12: Grid1.ColData(2) = 7: Grid1.ColData(3) = 13: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 5: Grid1.ColData(7) = 5
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select a.voucher_bill_no, sum(a.amount) from voucher_bill_details a, voucher_bill_head b where b.agent_idno = " & Str(RptDet.Idno1) & " and a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.voucher_bill_no group by a.voucher_bill_no"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select c.ledger_name, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount from voucher_bill_head a, reporttemp_simple b, ledger_head c where a.agent_idno = " & Str(RptDet.Idno1) & " and a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and a.voucher_bill_no *= b.text_1 and a.ledger_idno = c.ledger_idno order by c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                If cr_amt <> db_amt Then
    '                    Grid1.AddItem IIf(P_Name <> !ledger_name, !ledger_name, P_Name) & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                    P_Name = !ledger_name
    '                    tt_cr = tt_cr + cr_amt
    '                    tt_db = tt_db + db_amt
    '                End If
    '                .MoveNext
    '                If .EOF Then
    '                    If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                ElseIf P_Name <> !ledger_name And (tt_cr > 0 Or tt_db > 0) Then
    '                    GoSub Party_Total
    '                End If
    '            Loop
    '            Grid1.AddItem "" & Chr(9) & "Total" & Chr(9) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Cr) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Db) & Chr(9) & Cmpr.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "1"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '    Exit Sub
    '
    'Party_Total:
    '        Grid1.AddItem "" & Chr(9) & "Total" & Chr(9) & Chr(9) & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '        Grid1.RowData(Grid1.Rows - 1) = "1"
    '        Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Grid1.AddItem ""
    '        Al_Tt_Cr = Al_Tt_Cr + tt_cr
    '        Al_Tt_Db = Al_Tt_Db + tt_db
    '        tt_cr = 0: tt_db = 0
    '    Return
    '
    'End Sub
    '
    'Private Sub Bills_Customer_Pending_All()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim P_Name As String, condt_1 As String
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '
    '    CmPrnt.Heading1 = RptDet.RptCode_Main
    '    CmPrnt.Heading2 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 10: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<PARTY NAME                     |<COMP.  |<BL.DATE |<BL.NO  |<MILL/PARTY   |>CR.AMOUNT    |>DR.AMOUNT    |>BALANCE         |<     |>DAYS "
    '    Grid1.ColData(0) = 30: Grid1.ColData(1) = 7: Grid1.ColData(2) = 10: Grid1.ColData(3) = 7: Grid1.ColData(4) = 20: Grid1.ColData(5) = 15: Grid1.ColData(6) = 15: Grid1.ColData(7) = 15: Grid1.ColData(8) = 4: Grid1.ColData(9) = 4
    '
    '    If RptDet.RptCode_Main = "Bill Balance (All Party)" Then
    '        condt_1 = " and a.bill_type = '' "
    '    ElseIf RptDet.RptCode_Main = "Commission Balance (All)" Then
    '        condt_1 = " and a.bill_type = 'C' "
    '        Grid1.ColWidth(4) = 0
    '    ElseIf RptDet.RptCode_Main = "Commission Balance (Buyer)" Then
    '        condt_1 = " and a.bill_type = 'C' and tp.Parent_Code like '%~10~4~' "
    '        Grid1.ColWidth(4) = 0
    '    ElseIf RptDet.RptCode_Main = "Commission Balance (Seller)" Then
    '        condt_1 = " and a.bill_type = 'C' and tp.Parent_Code like '%~14~11~' "
    '        Grid1.ColWidth(4) = 0
    '    ElseIf RptDet.RptCode_Main = "Freight Balance (All Party)" Then
    '        condt_1 = " and a.bill_type = 'F' "
    '    Else
    '        condt_1 = ""
    '    End If
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '
    '    CON.Execute "insert into reporttemp_simple ( smallint_1, smallint_2, text_1, amount_1 ) Select tz.company_idno, ledger_idno, voucher_bill_no, sum(amount) from voucher_bill_details tp, company_head tz  where " & Replace(Condt, "tZ.", "tP.") & IIf(Condt <> "", " and ", "") & " voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and tp.company_idno = tz.company_idno group by tz.company_idno, ledger_idno, voucher_bill_no"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select tz.company_shortname, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, d.ledger_name as mill_party, a.bill_amount, a.crdr_type, b.amount_1 as amount, tp.ledger_name from voucher_bill_head a, reporttemp_simple b, ledger_head d, ledger_head tp, company_head tz where " & Replace(Condt, "tP.", "a.") & IIf(Condt <> "", " and ", "") & " a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' " & condt_1 & " and a.voucher_bill_no *= b.text_1 and a.company_idno *= b.smallint_1 and a.agent_idno *= d.ledger_idno and a.company_idno = tz.company_idno and a.ledger_idno = tp.ledger_idno order by tp.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON, adOpenStatic, adLockReadOnly
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                If cr_amt <> db_amt Then
    '                    Grid1.AddItem IIf(P_Name <> !ledger_name, !ledger_name, "") & vbTab & !Company_ShortName & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & !Mill_Party & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                    P_Name = !ledger_name
    '                    tt_cr = tt_cr + cr_amt
    '                    tt_db = tt_db + db_amt
    '                End If
    '                .MoveNext
    '                If .EOF Then
    '                    If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                ElseIf !ledger_name <> P_Name And (tt_cr > 0 Or tt_db > 0) Then
    '                    GoSub Party_Total
    '                End If
    '            Loop
    '            Grid1.AddItem "TOTAL" & vbTab & vbTab & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Al_Tt_Cr) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Db) & Chr(9) & Cmpr.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '    Exit Sub
    '
    'Party_Total:
    '            Grid1.AddItem vbTab & "TOTAL" & vbTab & vbTab & vbTab & vbTab & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Grid1.AddItem ""
    '            Al_Tt_Cr = Al_Tt_Cr + tt_cr: Al_Tt_Db = Al_Tt_Db + tt_db
    '            tt_cr = 0: tt_db = 0
    '    Return
    '
    'End Sub
    '
    'Private Sub Bills_Customer_Pending_Buyer_Seller()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim P_Name As String, Condt As String
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '
    '    CmPrnt.Heading1 = RptDet.RptCode_Main
    '    CmPrnt.Heading2 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 9: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<PARTY NAME                     |<BL.DATE |<BL.NO  |<PARTICLUARS   |>CR.AMOUNT    |>DR.AMOUNT    |>BALANCE         |<     |>DAYS "
    '    Grid1.ColData(0) = 30: Grid1.ColData(1) = 10: Grid1.ColData(2) = 7: Grid1.ColData(3) = 20: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 13: Grid1.ColData(7) = 4: Grid1.ColData(8) = 6
    '
    '    Condt = Condt & " and a.bill_type = '' and " & IIf(RptDet.RptCode_Main = "Bill Balance (Buyer)", "a.crdr_type = 'Dr'", "a.crdr_type = 'Cr'") & " and a.ledger_idno = c.ledger_idno and a.agent_idno *= d.ledger_idno"
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, sum(amount) from voucher_bill_details group by voucher_bill_no"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, d.ledger_name as mill_party, a.bill_amount, a.crdr_type, b.amount_1 as amount, c.ledger_name from voucher_bill_head a, reporttemp_simple b, ledger_head c, ledger_head d where a.voucher_bill_date < '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' " & Condt & " and a.voucher_bill_no *= b.text_1 order by c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                If cr_amt <> db_amt Then
    '                    Grid1.AddItem IIf(P_Name <> !ledger_name, !ledger_name, "") & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & !Mill_Party & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                    P_Name = !ledger_name
    '                    tt_cr = tt_cr + cr_amt
    '                    tt_db = tt_db + db_amt
    '                End If
    '                .MoveNext
    '                If .EOF Then
    '                    If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                ElseIf !ledger_name <> P_Name And (tt_cr > 0 Or tt_db > 0) Then
    '                    GoSub Party_Total
    '                End If
    '            Loop
    '            Grid1.AddItem "TOTAL" & vbTab & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Al_Tt_Cr) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Db) & Chr(9) & Cmpr.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '    Exit Sub
    '
    'Party_Total:
    '        Grid1.AddItem vbTab & "TOTAL" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '        Grid1.RowData(Grid1.Rows - 1) = "3"
    '        Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '        Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        Grid1.AddItem ""
    '        Al_Tt_Cr = Al_Tt_Cr + tt_cr: Al_Tt_Db = Al_Tt_Db + tt_db
    '        tt_cr = 0: tt_db = 0
    '    Return
    '
    'End Sub
    '
    'Private Sub Bills_Customer_Pending_Purchased()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim P_Name As String
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '
    '    CmPrnt.Heading1 = "PURCHASED BILL PENDING"
    '    CmPrnt.Heading2 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 8: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<PARTY NAME                       |<BILL DATE     |<BILL NO     |>CR.AMOUNT           |>DR.AMOUNT           |>BALANCE              |<      |>DAYS    "
    '    Grid1.ColData(0) = 35: Grid1.ColData(1) = 10: Grid1.ColData(2) = 7: Grid1.ColData(3) = 13: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 5: Grid1.ColData(7) = 6
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, sum(amount) from voucher_bill_details where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' group by voucher_bill_no"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount, c.ledger_name from voucher_bill_head a, reporttemp_simple b, ledger_head c where a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and a.crdr_type = 'Cr' and a.voucher_bill_no *= b.text_1 and a.ledger_idno = c.ledger_idno order by c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                If cr_amt <> db_amt Then
    '                    Grid1.AddItem IIf(P_Name <> !ledger_name, !ledger_name, "") & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                    P_Name = !ledger_name
    '                    tt_cr = tt_cr + cr_amt
    '                    tt_db = tt_db + db_amt
    '                End If
    '                .MoveNext
    '                If .EOF Then
    '                    If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                ElseIf !ledger_name <> P_Name And (tt_cr > 0 Or tt_db > 0) Then
    '                    GoSub Party_Total
    '                End If
    '            Loop
    '            Grid1.AddItem "TOTAL" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Al_Tt_Cr) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Db) & Chr(9) & Cmpr.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '    Exit Sub
    '
    'Party_Total:
    '            Grid1.AddItem vbTab & "TOTAL" & vbTab & vbTab & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Grid1.AddItem ""
    '            Al_Tt_Cr = Al_Tt_Cr + tt_cr: Al_Tt_Db = Al_Tt_Db + tt_db
    '            tt_cr = 0: tt_db = 0
    '    Return
    '
    'End Sub
    '
    'Private Sub Bills_Customer_Pending_Invoiced()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim P_Name As String
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '
    '    CmPrnt.Heading1 = "INVOICED BILL PENDING"
    '    CmPrnt.Heading2 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 8: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<PARTY NAME                       |<BILL DATE     |<BILL NO     |>CR.AMOUNT           |>DR.AMOUNT           |>BALANCE              |<      |>DAYS    "
    '    Grid1.ColData(0) = 35: Grid1.ColData(1) = 10: Grid1.ColData(2) = 7: Grid1.ColData(3) = 13: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 5: Grid1.ColData(7) = 6
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, sum(amount) from voucher_bill_details where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' group by voucher_bill_no"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount, c.ledger_name from voucher_bill_head a, reporttemp_simple b, ledger_head c where a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and a.crdr_type = 'Dr' and a.voucher_bill_no *= b.text_1 and a.ledger_idno = c.ledger_idno order by c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                If cr_amt <> db_amt Then
    '                    Grid1.AddItem IIf(P_Name <> !ledger_name, !ledger_name, "") & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                    P_Name = !ledger_name
    '                    tt_cr = tt_cr + cr_amt
    '                    tt_db = tt_db + db_amt
    '                End If
    '                .MoveNext
    '                If .EOF Then
    '                    If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                ElseIf !ledger_name <> P_Name And (tt_cr > 0 Or tt_db > 0) Then
    '                    GoSub Party_Total
    '                End If
    '            Loop
    '            Grid1.AddItem "TOTAL" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Al_Tt_Cr) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Db) & Chr(9) & Cmpr.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '    Exit Sub
    '
    'Party_Total:
    '            Grid1.AddItem vbTab & "TOTAL" & vbTab & vbTab & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Grid1.AddItem ""
    '            Al_Tt_Cr = Al_Tt_Cr + tt_cr: Al_Tt_Db = Al_Tt_Db + tt_db
    '            tt_cr = 0: tt_db = 0
    '    Return
    '
    'End Sub
    '
    'Private Sub Bills_Customer_Pending_Invoiced_CurYr()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim P_Name As String
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '
    '    CmPrnt.Heading1 = "INVOICED BILL PENDING"
    '    CmPrnt.Heading2 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 8: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<PARTY NAME                       |<DLV.DATE     |<BILL NO     |>CR.AMOUNT           |>DR.AMOUNT           |>BALANCE              |<      |>DAYS    "
    '    Grid1.ColData(0) = 35: Grid1.ColData(1) = 10: Grid1.ColData(2) = 7: Grid1.ColData(3) = 13: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 5: Grid1.ColData(7) = 6
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, sum(amount) from voucher_bill_details where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' group by voucher_bill_no"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.amount_1 as amount, c.ledger_name, d.order_date from voucher_bill_head a, reporttemp_simple b, ledger_head c, cloth_invoice_head d where a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and a.crdr_type = 'Dr' and a.voucher_bill_no *= b.text_1 and a.ledger_idno = c.ledger_idno and a.entry_identification = 'CLINV-'+d.Invoice_Code and a.voucher_bill_no like '%/" & Trim(CmpDet.FnYear) & "' order by c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                If cr_amt <> db_amt Then
    '                    Grid1.AddItem IIf(P_Name <> !ledger_name, !ledger_name, "") & vbTab & Format(IIf(IsDate(!Order_Date) = True, !Order_Date, !voucher_bill_date), "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", IIf(IsDate(!Order_Date) = True, !Order_Date, !voucher_bill_date), Date)
    '                    P_Name = !ledger_name
    '                    tt_cr = tt_cr + cr_amt
    '                    tt_db = tt_db + db_amt
    '                End If
    '                .MoveNext
    '            Loop
    '            Grid1.AddItem "TOTAL" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '
    'End Sub
    '
    'Private Sub Bills_Agent_Pending_All()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim P_Name As String, a_name As String
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '    Dim Ag_TotCr As Currency, Ag_TotDb As Currency
    '    Dim Nr_Par As Integer
    '
    '    CmPrnt.Heading1 = RptDet.RptCode_Main
    '    CmPrnt.Heading2 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 9: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<AGENT NAME               |<PARTY NAME              |<BL.DATE     |<BL.NO  |>CR.AMOUNT        |>DR.AMOUNT        |>BALANCE           |<      |>DAYS    "
    '    Grid1.ColData(0) = 30: Grid1.ColData(1) = 30: Grid1.ColData(2) = 10: Grid1.ColData(3) = 7: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 13: Grid1.ColData(7) = 5: Grid1.ColData(8) = 6
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, sum(amount) from voucher_bill_details where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' group by voucher_bill_no"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, 0 from voucher_bill_head where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "'"
    '    CON.Execute "truncate table reporttempsub"
    '    CON.Execute "insert into reporttempsub ( name1, currency1 ) select text_1, sum(amount_1) from reporttemp_simple group by text_1"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select d.ledger_name as agent_name, c.ledger_name, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.currency1 as amount from voucher_bill_head a, reporttempsub b, ledger_head c, ledger_head d where a.voucher_bill_no = b.name1 and a.bill_amount <> b.currency1 and a.ledger_idno = c.ledger_idno and a.agent_idno = d.ledger_idno order by d.ledger_name, c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                Grid1.AddItem IIf(a_name <> !agent_name, !agent_name, "") & vbTab & IIf(P_Name <> !ledger_name Or a_name <> !agent_name, !ledger_name, "") & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                P_Name = !ledger_name
    '                a_name = !agent_name
    '                tt_cr = tt_cr + cr_amt
    '                tt_db = tt_db + db_amt
    '                Ag_TotCr = Ag_TotCr + cr_amt
    '                Ag_TotDb = Ag_TotDb + db_amt
    '                .MoveNext
    '                If .EOF Then
    '                    If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                    GoSub Agent_Total
    '                ElseIf (!ledger_name <> P_Name Or a_name <> !agent_name) And (tt_cr > 0 Or tt_db > 0) Then
    '                    GoSub Party_Total
    '                    Nr_Par = Nr_Par + 1
    '                    If a_name <> !agent_name Then GoSub Agent_Total
    '                    If a_name <> !agent_name Then Nr_Par = 0
    '                End If
    '            Loop
    '            Grid1.AddItem "GRAND TOTAL" & vbTab & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Al_Tt_Cr) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Db) & Chr(9) & Cmpr.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = 2
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '
    '    Exit Sub
    '
    'Party_Total:
    '            Grid1.AddItem vbTab & "TOTAL (PARTY)" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Al_Tt_Cr = Al_Tt_Cr + tt_cr: Al_Tt_Db = Al_Tt_Db + tt_db
    '            tt_cr = 0: tt_db = 0
    '    Return
    '
    'Agent_Total:
    '            Grid1.AddItem "TOTAL (AGENT)" & vbTab & "" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Ag_TotCr) & Chr(9) & Cmpr.Currency_Format(Ag_TotDb) & Chr(9) & Cmpr.Currency_Format(Abs(Ag_TotCr - Ag_TotDb)) & Chr(9) & IIf(Ag_TotCr >= Ag_TotDb, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "2"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Ag_TotCr = 0: Ag_TotDb = 0
    '            Nr_Par = 0
    '    Return
    'End Sub
    '
    'Private Sub Bills_Agent_Pending_Purchased()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim P_Name As String, a_name As String
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '    Dim Ag_TotCr As Currency, Ag_TotDb As Currency
    '    Dim Nr_Par As Integer
    '
    '
    '    CmPrnt.Heading1 = "PURCHASED BILL PENDING"
    '    CmPrnt.Heading2 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 9: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<AGENT NAME               |<PARTY NAME              |<BL.DATE     |<BL.NO  |>CR.AMOUNT        |>DR.AMOUNT        |>BALANCE           |<      |>DAYS    "
    '    Grid1.ColData(0) = 30: Grid1.ColData(1) = 30: Grid1.ColData(2) = 10: Grid1.ColData(3) = 7: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 13: Grid1.ColData(7) = 5: Grid1.ColData(8) = 6
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, sum(amount) from voucher_bill_details where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' group by voucher_bill_no"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, 0 from voucher_bill_head where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "'"
    '    CON.Execute "truncate table reporttempsub"
    '    CON.Execute "insert into reporttempsub ( name1, currency1 ) select text_1, sum(amount_1) from reporttemp_simple group by text_1"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select d.ledger_name as agent_name, c.ledger_name, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.currency1 as amount from voucher_bill_head a, reporttempsub b, ledger_head c, ledger_head d where a.crdr_type = 'Cr' and a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.name1 and a.bill_amount <> b.currency1 and a.ledger_idno = c.ledger_idno and a.agent_idno = d.ledger_idno order by d.ledger_name, c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                Grid1.AddItem IIf(a_name <> !agent_name, !agent_name, "") & vbTab & IIf(P_Name <> !ledger_name Or a_name <> !agent_name, !ledger_name, "") & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                P_Name = !ledger_name
    '                a_name = !agent_name
    '                tt_cr = tt_cr + cr_amt
    '                tt_db = tt_db + db_amt
    '                Ag_TotCr = Ag_TotCr + cr_amt
    '                Ag_TotDb = Ag_TotDb + db_amt
    '                .MoveNext
    '                If .EOF Then
    '                    If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                    GoSub Agent_Total
    '                ElseIf (!ledger_name <> P_Name Or !ledger_name <> P_Name) And (tt_cr > 0 Or tt_db > 0) Then
    '                    GoSub Party_Total
    '                    Nr_Par = Nr_Par + 1
    '                    If a_name <> !agent_name Then GoSub Agent_Total
    '                    If a_name <> !agent_name Then Nr_Par = 0
    '                End If
    '            Loop
    '            Grid1.AddItem "GRAND TOTAL" & vbTab & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Al_Tt_Cr) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Db) & Chr(9) & Cmpr.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = 2
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '    Exit Sub
    '
    'Party_Total:
    '            Grid1.AddItem vbTab & "TOTAL (PARTY)" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "3"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Al_Tt_Cr = Al_Tt_Cr + tt_cr: Al_Tt_Db = Al_Tt_Db + tt_db
    '            tt_cr = 0: tt_db = 0
    '    Return
    '
    'Agent_Total:
    '            Grid1.AddItem "TOTAL (AGENT)" & vbTab & "" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Ag_TotCr) & Chr(9) & Cmpr.Currency_Format(Ag_TotDb) & Chr(9) & Cmpr.Currency_Format(Abs(Ag_TotCr - Ag_TotDb)) & Chr(9) & IIf(Ag_TotCr >= Ag_TotDb, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "2"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Ag_TotCr = 0: Ag_TotDb = 0
    '            Nr_Par = 0
    '    Return
    '
    'End Sub
    '
    'Private Sub Bills_Agent_Pending_Invoiced()
    '    Dim RS As ADODB.Recordset
    '    Dim tt_cr As Currency, tt_db As Currency
    '    Dim cr_amt As Currency, db_amt As Currency
    '    Dim P_Name As String, a_name As String
    '    Dim tt1 As Currency, tt2 As Currency
    '    Dim Al_Tt_Cr As Currency, Al_Tt_Db As Currency
    '    Dim Ag_TotCr As Currency, Ag_TotDb As Currency
    '    Dim Nr_Par As Integer
    '
    '    CmPrnt.Heading1 = "INVOICED BILL PENDING"
    '    CmPrnt.Heading2 = " as on : " & RptDet.Date1
    '    Grid1.Cols = 9: tt1 = 0: tt2 = 0
    '    Grid1.FormatString = "<AGENT NAME               |<PARTY NAME              |<BL.DATE     |<BL.NO  |>CR.AMOUNT        |>DR.AMOUNT        |>BALANCE           |<      |>DAYS    "
    '    Grid1.ColData(0) = 30: Grid1.ColData(1) = 30: Grid1.ColData(2) = 10: Grid1.ColData(3) = 7: Grid1.ColData(4) = 13: Grid1.ColData(5) = 13: Grid1.ColData(6) = 13: Grid1.ColData(7) = 5: Grid1.ColData(8) = 6
    '
    '    CON.Execute "truncate table reporttemp_simple"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, sum(amount) from voucher_bill_details where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' group by voucher_bill_no"
    '    CON.Execute "insert into reporttemp_simple ( text_1, amount_1 ) select voucher_bill_no, 0 from voucher_bill_head where voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "'"
    '    CON.Execute "truncate table reporttempsub"
    '    CON.Execute "insert into reporttempsub ( name1, currency1 ) select text_1, sum(amount_1) from reporttemp_simple group by text_1"
    '
    '    Set RS = New ADODB.Recordset
    '      With RS
    '        .Open "select d.ledger_name as agent_name, c.ledger_name, a.voucher_bill_no, a.voucher_bill_date, a.party_bill_no, a.bill_amount, a.crdr_type, b.currency1 as amount from voucher_bill_head a, reporttempsub b, ledger_head c, ledger_head d where a.crdr_type = 'Dr' and a.voucher_bill_date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and a.voucher_bill_no = b.name1 and a.bill_amount <> b.currency1 and a.ledger_idno = c.ledger_idno and a.agent_idno = d.ledger_idno order by d.ledger_name, c.ledger_name, a.voucher_bill_date, a.voucher_bill_no", CON
    '        If Not (.BOF And .EOF) Then
    '            .MoveFirst
    '            Do While Not .EOF
    '                If !crdr_type = "Cr" Then
    '                    cr_amt = !bill_amount
    '                    If !Amount <> "" Then db_amt = !Amount Else db_amt = 0
    '                Else
    '                    db_amt = !bill_amount
    '                    If !Amount <> "" Then cr_amt = !Amount Else cr_amt = 0
    '                End If
    '                Grid1.AddItem IIf(a_name <> !agent_name, !agent_name, "") & vbTab & IIf(P_Name <> !ledger_name Or a_name <> !agent_name, !ledger_name, "") & vbTab & Format(!voucher_bill_date, "dd-mm-yy") & Chr(9) & !Party_Bill_No & Chr(9) & Cmpr.Currency_Format(cr_amt) & Chr(9) & Cmpr.Currency_Format(db_amt) & Chr(9) & Cmpr.Currency_Format(Abs(cr_amt - db_amt)) & Chr(9) & IIf(db_amt > cr_amt, "Dr", "Cr") & Chr(9) & DateDiff("d", !voucher_bill_date, Date)
    '                P_Name = !ledger_name
    '                a_name = !agent_name
    '                tt_cr = tt_cr + cr_amt
    '                tt_db = tt_db + db_amt
    '                Ag_TotCr = Ag_TotCr + cr_amt
    '                Ag_TotDb = Ag_TotDb + db_amt
    '                .MoveNext
    '                If .EOF Then
    '                    If tt_cr > 0 Or tt_db > 0 Then GoSub Party_Total
    '                    GoSub Agent_Total
    '                ElseIf (!ledger_name <> P_Name Or a_name <> !agent_name) And (tt_cr > 0 Or tt_db > 0 Or Ag_TotCr > 0 Or Ag_TotDb > 0) Then
    '                    GoSub Party_Total
    '                    Nr_Par = Nr_Par + 1
    '                    If a_name <> !agent_name Then GoSub Agent_Total
    '                    If a_name <> !agent_name Then Nr_Par = 0
    '                End If
    '            Loop
    '            Grid1.AddItem "GRAND TOTAL" & vbTab & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Al_Tt_Cr) & Chr(9) & Cmpr.Currency_Format(Al_Tt_Db) & Chr(9) & Cmpr.Currency_Format(Abs(Al_Tt_Cr - Al_Tt_Db)) & Chr(9) & IIf(Al_Tt_Cr >= Al_Tt_Db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = 2
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '        End If
    '        .Close
    '      End With
    '    Set RS = Nothing
    '    Exit Sub
    '
    'Party_Total:
    '            Grid1.AddItem vbTab & "TOTAL (PARTY)" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(tt_cr) & Chr(9) & Cmpr.Currency_Format(tt_db) & Chr(9) & Cmpr.Currency_Format(Abs(tt_cr - tt_db)) & Chr(9) & IIf(tt_cr >= tt_db, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = 3
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Al_Tt_Cr = Al_Tt_Cr + tt_cr: Al_Tt_Db = Al_Tt_Db + tt_db
    '            tt_cr = 0: tt_db = 0
    '    Return
    '
    'Agent_Total:
    '            Grid1.AddItem "TOTAL (AGENT)" & vbTab & "" & vbTab & vbTab & vbTab & Cmpr.Currency_Format(Ag_TotCr) & Chr(9) & Cmpr.Currency_Format(Ag_TotDb) & Chr(9) & Cmpr.Currency_Format(Abs(Ag_TotCr - Ag_TotDb)) & Chr(9) & IIf(Ag_TotCr >= Ag_TotDb, "Cr", "Dr")
    '            Grid1.RowData(Grid1.Rows - 1) = "2"
    '            Call Cmpr.Grids_CellBackColor(Grid1, Grid1.Rows - 1)
    '            Call Cmpr.Grids_CellForeColor(Grid1, Grid1.Rows - 1, 1)
    '            Ag_TotCr = 0: Ag_TotDb = 0
    '            Nr_Par = 0
    '    Return
    '
    'End Sub
    '
    'Private Function Replace_FieldName(ByVal S As String)
    '    S = Replace(S, "tZ.", "")
    '    S = Replace(S, "tP.", "")
    '    S = Replace(S, "tL.", "")
    '    S = Replace(S, "tY.", "")
    '    S = Replace(S, "tM.", "")
    '    S = Replace(S, "tC.", "")
    '    S = Replace(S, "tS.", "")
    '    S = Replace(S, "tE.", "")
    '    S = Replace(S, "tI.", "")
    '    S = Replace(S, "tQ.", "")
    '    Replace_FieldName = S
    'End Function
    '
    'Private Function Eliminate_Comma(ByVal S As String)
    '    Eliminate_Comma = S
    '    If Trim(S) <> "" Then Eliminate_Comma = Left(S, Len(S) - 1)
    'End Function
    '
    'Private Sub Print_BillBalanceSingle()
    '    Dim Ln_No As Integer, Pg_No As Integer, i As Integer, k As Integer
    '    Dim tt_cr As Currency, Tt_Dr As Currency
    '    Dim nar As String
    '
    '    GoSub Page_Header
    '    For i = 2 To Grid1.Rows - 3
    '        If Ln_No > 61 Then GoSub Page_Footer
    '        If Trim(Grid1.TextMatrix(i, 0)) <> "" Then
    '            Print #1, Chr(27); "E"; Trim(Grid1.TextMatrix(i, 0)); Chr(27); "F"
    '            Ln_No = Ln_No + 1
    '        End If
    '        Print #1, Grid1.TextMatrix(i, 1); Spc(10 - Len(Grid1.TextMatrix(i, 1)));
    '        Print #1, Grid1.TextMatrix(i, 2); Spc(7 - Len(Grid1.TextMatrix(i, 2)));
    '        Print #1, Grid1.TextMatrix(i, 3); Spc(7 - Len(Grid1.TextMatrix(i, 3)));
    '        Print #1, Grid1.TextMatrix(i, 4); Spc(6 - Len(Grid1.TextMatrix(i, 4)));
    '        Print #1, Grid1.TextMatrix(i, 5); Spc(7 - Len(Grid1.TextMatrix(i, 5)));
    '        Print #1, Spc(18 - Len(Grid1.TextMatrix(i, 6))); Grid1.TextMatrix(i, 6);
    '        Print #1, Spc(19 - Len(Grid1.TextMatrix(i, 7))); Grid1.TextMatrix(i, 7);
    '        Print #1, Spc(6 - Len(Grid1.TextMatrix(i, 8))); Grid1.TextMatrix(i, 8)
    '        Ln_No = Ln_No + 1
    '    Next i
    '    Print #1, Cmpr.CharPrnt(45, 80)
    '    Print #1, Spc(74 - Len(Grid1.TextMatrix(i + 1, 7))); Grid1.TextMatrix(i + 1, 7)
    '    Print #1, Cmpr.CharPrnt(45, 80)
    '    Ln_No = Ln_No + 3
    '    For i = Ln_No + 1 To 72
    '        Print #1,
    '    Next i
    '
    '    Exit Sub
    '
    'Page_Header:
    '        Pg_No = Pg_No + 1
    '        Print #1, Spc(40 - (Len(Trim(RptDet.Name1)) / 2)); Chr(27); "E"; Trim(RptDet.Name1); Chr(27); "F"
    '        Print #1, Spc(40 - (Len("OUTSTANDING BILLS - AS ON " & Trim(Format(RptDet.Date1, "dd/mm/yyyy"))) / 2)); "OUTSTANDING BILLS - AS ON " & Trim(Format(RptDet.Date1, "dd/mm/yyyy"))
    '        Print #1, Spc(73 - Len(Trim(Str(Pg_No)))); "PAGE : "; Trim(Str(Pg_No))
    '        Print #1, Cmpr.CharPrnt(45, 80)
    '        Print #1, "  DATE    BL.NO  COUNT  BAGS  RATE           BALANCE             TOTAL      DAYS"
    '        Print #1, Cmpr.CharPrnt(45, 80)
    '        Ln_No = 6
    '    Return
    '
    'Page_Footer:
    '        Print #1, Cmpr.CharPrnt(45, 80)
    '        Print #1, Spc(72); "Contd..."
    '        Ln_No = Ln_No + 2
    '        For k = Ln_No + 1 To 72
    '            Print #1, ""
    '        Next k
    '        GoSub Page_Header
    '    Return
    'End Sub
    '
    'Private Sub Register_Reports()
    '    Dim Rt1 As ADODB.Recordset
    '    Dim RS As ADODB.Recordset
    '    Dim i As Integer
    '    Dim condt_1 As String
    '    Dim Fld1, Fld2 As String
    '    Dim Type_Db_1(4) As Currency
    '    Dim Type_Cr_1(5) As Currency
    '    Dim CmpNm As String, CmpAdd1 As String, CmpAdd2 As String, CmpAdd3 As String, CmpAdd4 As String, CmpTin As String
    '    Dim SG_STS As String
    '    Dim Format_Dec As String
    '
    '    Select Case RptDet.RptCode_Main
    '
    ''============================================================================================
    ''                           FABRIC PURCHASE REGISTER
    ''============================================================================================
    '
    '        Case "Purchase Register - Details"
    '
    '        If Trim(UCase(Settings.Company_Name)) = "RASILAKSHMI" Then Format_Dec = "|4" Else Format_Dec = "|3"
    '
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select " & Field_1 & " a.Purchase_Code as code, a.Purchase_Date, left(a.Purchase_No,(len(a.Purchase_No)-6)) as PurNo, a.Bill_No, " & Field_2 & " d.Unit_Name, CAST(b.Quantity AS NUMERIC(18,6)) as Qty, b.Rate, b.Amount, a.Gross_Amount, a.Tax_Amount, a.AddLess_Amount, a.Net_Amount from Purchase_Head a, Purchase_Details b, Ledger_Head tP, Item_Head tI, Unit_Head d, Company_Head tZ where " & Condt & IIf(Condt <> "", " and ", "") & " a.Purchase_Date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and  '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and a.company_idno = b.company_idno and a.Purchase_Code = b.Purchase_Code and a.company_idno = tZ.company_idno and a.Ledger_Idno =  tp.Ledger_Idno and b.item_idno = ti.item_idno and b.Unit_Idno = d.Unit_Idno order by " & Field_1 & " a.Purchase_Date, a.For_OrderBy, a.Purchase_Code", _
    '                    Heading_1 & "<[HIDDEN][GP=]a.Purchase_Code |<=[LEN10]DATE |<=[LEN6]REF.NO |<=[LEN8]BILL NO  |" & Heading_2 & "<=[LEN20]UNIT NAME| >@[LEN10]QUANTITY  |>[LEN11]RATE   |>@[LEN12]AMOUNT |>=@[LEN12]GROSS AMOUNT     |>=[LEN12][ZS]TAX AMOUNT    |>=[LEN12][ZS]ADD/LESS     |>=@[LEN12]NET AMOUNT", Rpt_Hd, Me, Format_1 & "|DD-MM-YY|||" & Format_2 & Format_Dec & " |2|2|2|2|2|2", "TOTAL|1", [Ledger 136Cols], Portrait, None, [Draft 12cpi]
    '
    '        Case "Purchase Register - Summary"
    '
    '            If Trim(UCase(Settings.Company_Name)) = "RASILAKSHMI" Then Format_Dec = "||4|2" Else Format_Dec = "||3|2"
    '
    '            SG_STS = ""
    '            If Heading_1 = "" Then
    '                Heading_2 = Replace(Heading_2, "COMPANY", "[SUB_GP]COMPANY")
    '            End If
    '
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select " & Field_1 & Field_2 & " b.Ledger_Name, c.Item_Name, sum(a.Quantity) as Qty, sum(a.Amount) as Amt from Purchase_Details a, Ledger_Head b, Item_Head c, Company_Head tZ where " & Condt & IIf(Condt <> "", " and ", "") & " a.Purchase_Date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and  '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and a.company_idno = tz.company_idno and a.Ledger_Idno = b.Ledger_Idno and a.item_idno = c.item_idno group by " & Field_1 & Field_2 & " b.ledger_name, c.item_name order by " & Field_1 & " b.ledger_name, c.item_name", _
    '                    Heading_1 & Heading_2 & "<=[LEN30]PARTY NAME |<[LEN20]ITEM NAME |>@[LEN10]QUANTITY  |>@[LEN12]AMOUNT ", Rpt_Hd, Me, Format_1 & Format_2 & Format_Dec & "", "TOTAL|0", [Ledger 136Cols], Portrait, None, [Draft 12cpi]
    '
    '        Case "Invoice Register - Details"
    '
    '            If Trim(UCase(Settings.Company_Name)) = "RASILAKSHMI" Then Format_Dec = "|4" Else Format_Dec = "|3"
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select " & Field_1 & " a.Invoice_RefCode, a.Invoice_Date, left(a.Invoice_No,(len(a.Invoice_No)-6)) as InvNo, " & Field_2 & " a.Invoice_Type, a.Dc_No, a.Dc_Date, a.Order_No, a.Order_Date, d.Unit_Name, c.Quantity, c.Rate, c.Amount, c.Discount_Amount, a.Total_Amount, (a.Tax_Amount1+a.Tax_Amount2) as TaxAmt, a.Other_Charges_Value, a.AddLess_Amount, a.Net_Amount from Invoice_Head a, Ledger_Head tp, Invoice_Details c , Item_Head ti, Unit_Head d, Company_Head tZ where " & Condt & IIf(Condt <> "", " and ", "") & " a.Invoice_Date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and  '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and a.Ledger_Idno = tp.Ledger_Idno and c.item_idno = ti.item_idno and a.Invoice_RefCode = c.Invoice_RefCode and a.company_idno = tZ.company_idno and tI.Unit_Idno = d.Unit_Idno order by " & Field_1 & " a.Invoice_Date, a.For_OrderBy, a.Invoice_RefCode, a.Invoice_Code, c.sl_No ", _
    '                    Heading_1 & "<[HIDDEN][GP=]Invoice_Code |<=[LEN10]DATE |<=[LEN6]REF.NO |" & Heading_2 & "<=[LEN15]TYPE|<=[LEN8]DC.CODE|<=[LEN10][ZS]DC.DATE|<=[LEN8]ORD.NO|<=[LEN10][ZS]ORD.DATE|<[LEN10]UNIT NAME|>@[LEN10]QUANTITY  |>[LEN11][ZS]RATE |>[LEN12][ZS]AMOUNT |>@[LEN11][ZS]DISCOUNT |>=@[LEN12][ZS]GROSS AMOUNT  |>=@[LEN12][ZS]VAT AMOUNT  |>=[LEN11][ZS]OTHER.CHRGS  |>=[LEN10][ZS]ADD/LESS     |>=@[LEN12][ZS]NET AMOUNT", Rpt_Hd, Me, Format_1 & "|DD-MM-YY|| " & Format_2 & "|||DD-MM-YY||DD-MM-YY" & Format_Dec & " |2|2|2|2|2|2|2|2|2", "TOTAL|1", [Ledger 136Cols], Portrait, None, [Draft 12cpi]
    '
    '        Case "Invoice Register - Summary"
    '
    '            If Trim(UCase(Settings.Company_Name)) = "RASILAKSHMI" Then Format_Dec = "|4" Else Format_Dec = "|3"
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select " & Field_1 & Field_2 & " b.Ledger_Name, c.Item_Name, sum(a.Quantity) as Qty, sum(a.Amount) as Amt from Invoice_Head d, Invoice_Details a, Ledger_Head b, Item_Head c, Company_Head tZ where " & Condt & IIf(Condt <> "", " and ", "") & " a.Invoice_Date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and a.Invoice_Code = d.Invoice_Code and d.Ledger_Idno = b.Ledger_Idno and a.item_idno = c.item_idno and a.company_idno = tz.company_idno group by " & Field_1 & Field_2 & " b.Ledger_Name, c.Item_Name order by " & Field_1 & Field_2 & " b.Ledger_Name, c.Item_Name ", _
    '                    Heading_1 & Heading_2 & "<=[LEN30]PARTY NAME |<[LEN20]ITEM NAME |>@[LEN10][ZS]QUANTITY  |>@[ZS][LEN12]AMOUNT", Rpt_Hd, Me, "||" & Format_Dec & "|2", "TOTAL|0", [Ledger 136Cols], Portrait, None, [Draft 12cpi]
    '
    '        Case "Item Stock - Details"
    '
    '            CON.Execute "Truncate Table ReportTemp"
    '            If Trim(UCase(Settings.Company_Name)) = "RASILAKSHMI" Then Format_Dec = "|4|4|4" Else Format_Dec = "|3|3|3"
    '
    '            CON.Execute "insert into reporttemp(int5, name1, weight2, weight3) Select 0, 'Opening123456', (case when sum((Quantity_Debit-Quantity_Credit)) > 0 then sum((Quantity_Debit-Quantity_Credit)) else 0 end ), (case when sum((Quantity_Debit-Quantity_Credit)) < 0 then abs(sum((Quantity_Debit-Quantity_Credit))) else 0 end ) from Item_Processing_Details where Company_Idno in " & RptInp(0).Value & " and item_idno in " & Trim(RptInp(1).Value) & "  and Reference_Date < '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "'"
    '            CON.Execute "insert into Reporttemp(int5, Date1, name1, Name2, int2, weight2) Select 1, Reference_Date, Reference_Code, Ledger_Name, Item_Idno, Quantity_Debit from Item_Processing_Details a, Ledger_Head b where Company_Idno in " & RptInp(0).Value & " and a.Item_IdNo IN " & Trim(RptInp(1).Value) & " and a.Ledger_Idno *= b.Ledger_Idno and Reference_Date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and Quantity_Debit > 0 "
    '            CON.Execute "insert into Reporttemp(int5, Date1, name1, Name2, int2, weight3) Select 2, Reference_Date, Reference_Code, Ledger_Name, Item_Idno, Quantity_Credit from Item_Processing_Details a, Ledger_head b where Company_Idno in " & RptInp(0).Value & " and a.Item_IdNo IN " & Trim(RptInp(1).Value) & " and a.Ledger_Idno *= b.Ledger_idno and Reference_Date between '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet.Date2, "mm/dd/yyyy")) & "' and Quantity_Credit <> 0 "
    '
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select Date1, Left(Name1,(len(Name1)-6)) as Entry_Id, Name2 as Particluars, sum(weight2) as Debit, sum(weight3) as Credit, sum(weight4) as Stock from ReportTemp a Group by date1, name2, int5, name1 order by date1, int5, name1 ", _
    '                                    "<[LEN10]DATE  |<[LEN11]ENT ID |<[LEN30]PARTICULARS   |>@[LEN12][ZS]DEBIT |>@[LEN12][ZS]CREDIT |>[LEN12][CAL3-4]STOCK ", Rpt_Hd, Me, "dd-mm-yy||" & Format_Dec & "", "TOTAL|1~STOCK", [Ledger 80Cols], Portrait, [Vertical Line], [Draft 12cpi]
    '
    '        Case "Item Stock - Summary"
    '            If Trim(UCase(Settings.Company_Name)) = "RASILAKSHMI" Then Format_Dec = "|4" Else Format_Dec = "|3"
    '
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select Item_Name, sum(a.Quantity_Debit-a.Quantity_Credit) as Stock, b.Unit_Name, tI.Rate, (sum(a.Quantity_Debit-a.Quantity_Credit)*tI.Rate) as StockValue from Item_Processing_Details a, Item_Head tI, Unit_Head b where a.Company_Idno in " & RptInp(0).Value & " and a.Reference_Date <= '" & Trim(Format(RptDet.Date1, "mm/dd/yyyy")) & "' and a.Item_IdNo = tI.Item_IdNo and tI.Unit_IdNo *= b.Unit_IdNo Group by tI.Item_Name, b.Unit_Name, tI.Rate order by tI.Item_Name, b.Unit_Name", _
    '                                    "<[LEN45]ITEM NAME  |>[LEN13]STOCK  |>[LEN6]UNIT  |>[LEN10][ZS]RATE |>@[LEN15][ZS]STOCK VALUE", Rpt_Hd, Me, "" & Format_Dec & "||2|2", "TOTAL|0", [Ledger 80Cols], Portrait, [Vertical Line], [Draft 12cpi]
    '
    '
    '        Case "AnnexureI - BillWise"
    '
    '            CmpDet.Name = ""
    '            CmpDet.Add1 = "": CmpDet.Add2 = "": CmpDet.Add3 = "": CmpDet.Add4 = ""
    '
    '            Rpt.Company_Name = ""
    '            Rpt.Company_Address = ""
    '
    '            Set Rt1 = New ADODB.Recordset
    '                Rt1.Open "Select * from Company_Head where Company_Idno in " & Trim(RptInp(0).Value), CON, adOpenStatic, adLockReadOnly
    '                If Not (Rt1.BOF And Rt1.EOF) Then
    '                    Rt1.MoveFirst
    '                    CmpNm = Rt1!Company_Name
    '                    CmpAdd1 = Rt1!Company_Address1
    '                    CmpAdd2 = Rt1!Company_Address2
    '                    CmpAdd3 = Rt1!Company_Address3
    '                    CmpAdd4 = Rt1!Company_Address4
    '                    CmpTin = Rt1!Company_TinNo
    '                End If
    '                Rt1.Close
    '            Set Rt1 = Nothing
    '
    '            CON.Execute "Truncate Table ReportTemp"
    '            CON.Execute "Insert into ReportTemp(Meters10, Date1, Name1, Name2, Name3, Int1, Name4, Currency1, Meters1, currency2, name5) select a.for_Orderby, a.Purchase_Date, a.Purchase_Code, left(a.Purchase_No,len(a.Purchase_No)-6) as RefNo, a.Bill_No, a.Ledger_Idno, c.Ledger_TinNo as Commodity_Code, a.Assessable_Value as Purchase_Value, a.Tax_Perc, a.Tax_Amount, c.Ledger_CstNo as Category from Purchase_Head a, Ledger_head c where a.Company_IdNo in " & Trim(RptInp(0).Value) & " and a.Purchase_Code LIKE '%/" & Trim(CmpDet.FnYear) & "' and month(a.Purchase_Date) IN " & Trim(RptInp(1).Value) & " and a.PurchaseAc_IdNo = c.Ledger_Idno"
    '
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select 0 as AUTO_SLNO, Date1 as Party_Bill_Date, Name2 as refNo, name3 as Party_Bill_No, b.ledger_name, (case when Ledger_Address4 <> '' then Ledger_Address4 when Ledger_Address3 <> '' then Ledger_Address3 when Ledger_Address2 <> '' then Ledger_Address2 else Ledger_Address1 end) as Place, Ledger_TinNo, name4 as Commodity_Code, currency1 as Purchase_Value, Meters1 as Rate_of_Tax, currency2 as Vat_Amount, name5 as Category from ReportTemp a, Ledger_Head b where a.Int1 = b.Ledger_IdNo order by a.Date1, a.Meters10, a.Name1", _
    '                        "<[LEN6]SL.NO |<[LEN11]BILL DATE |<[LEN9]REF.NO |<[LEN10]BILL NO |<[LEN30]NAME OF THE SELLER |<[LEN15]PLACE |<[LEN16]SELLERS TIN |<[LEN15]COMMODITY CODE  |>@[LEN15][ZS]PURCHASE VALUE |>[LEN12][ZS]RATE OF TAX  |>@[LEN12][ZS]VAT PAID  |<[LEN9]CATEGORY ", "ANNEXURE - I | DETAILS OF PURCHASES DURING THE MONTH " & RptInp(1).Caption & "  " & IIf(Month_NameToIdno((RptInp(1).Caption)) > 3, Format(CmpDet.FromDate, "yyyy"), Format(CmpDet.ToDate, "yyyy")) & "| COMPANY NAME : " & CmpNm & "   -  TIN NO : " & CmpTin & " | " & CmpAdd1 & " " & CmpAdd2 & " " & CmpAdd3 & " " & CmpAdd4, Me, "|dd-mm-yyyy|||||||2|2|2|", "TOTAL|1", [Ledger 136Cols], Portrait, [Vertical Line], [Draft 12cpi]
    '
    '        Case "AnnexureI"
    '
    '            CmpDet.Name = ""
    '            CmpDet.Add1 = "": CmpDet.Add2 = "": CmpDet.Add3 = "": CmpDet.Add4 = ""
    '
    '            Rpt.Company_Name = ""
    '            Rpt.Company_Address = ""
    '
    '            Set Rt1 = New ADODB.Recordset
    '                Rt1.Open "Select * from Company_Head where Company_Idno in " & Trim(RptInp(0).Value), CON, adOpenStatic, adLockReadOnly
    '                If Not (Rt1.BOF And Rt1.EOF) Then
    '                    Rt1.MoveFirst
    '                    CmpNm = Rt1!Company_Name
    '                    CmpAdd1 = Rt1!Company_Address1
    '                    CmpAdd2 = Rt1!Company_Address2
    '                    CmpAdd3 = Rt1!Company_Address3
    '                    CmpAdd4 = Rt1!Company_Address4
    '                    CmpTin = Rt1!Company_TinNo
    '                End If
    '                Rt1.Close
    '            Set Rt1 = Nothing
    '
    '            CON.Execute "Truncate Table ReportTemp"
    '            CON.Execute "Insert into ReportTemp(Int1, Name4, Currency1, Meters1, currency2, name5) select a.Ledger_Idno, c.Ledger_TinNo as Commodity_Code, sum(a.Gross_Amount) as Purchase_Value, a.Tax_Perc, sum(a.Tax_Amount), c.Ledger_CstNo as Category from Purchase_Head a, Ledger_head c where a.Company_IdNo in " & Trim(RptInp(0).Value) & " and a.Purchase_Code LIKE '%/" & Trim(CmpDet.FnYear) & "' and month(a.Purchase_Date) IN " & Trim(RptInp(1).Value) & " and a.PurchaseAc_IdNo = c.Ledger_Idno group by a.Ledger_Idno, c.Ledger_TinNo, a.Tax_Perc, c.Ledger_CstNo"
    '
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select 0 as AUTO_SLNO, b.ledger_name, (case when Ledger_Address4 <> '' then Ledger_Address4 when Ledger_Address3 <>  '' then Ledger_Address3 when Ledger_Address2 <> '' then Ledger_Address2 else Ledger_Address1 end) as Place, Ledger_TinNo, name4 as Commodity_Code, currency1 as Purchase_Value, Meters1 as Rate_of_Tax, currency2 as Vat_Amount, name5 as Category from ReportTemp a, Ledger_Head b where a.Int1 = b.Ledger_IdNo order by a.Meters1, b.ledger_name", _
    '                        "<[LEN6]SL.NO |<[LEN30]NAME OF THE SELLER |<[LEN15]PLACE |<[LEN16]SELLERS TIN |<[LEN15]COMMODITY CODE  |>@[LEN15][ZS]PURCHASE VALUE |>[LEN12][ZS]RATE OF TAX  |>@[LEN12][ZS]VAT PAID  |<[LEN9]CATEGORY ", "ANNEXURE - I | DETAILS OF PURCHASES DURING THE MONTH " & RptInp(1).Caption & "  " & IIf(Month_NameToIdno((RptInp(1).Caption)) > 3, Format(CmpDet.FromDate, "yyyy"), Format(CmpDet.ToDate, "yyyy")) & "| COMPANY NAME : " & CmpNm & "   -  TIN NO : " & CmpTin & " | " & CmpAdd1 & " " & CmpAdd2 & " " & CmpAdd3 & " " & CmpAdd4, Me, "|||||2|2|2|", "TOTAL|1", [Ledger 136Cols], Portrait, [Vertical Line], [Draft 12cpi]
    '
    '
    '        Case "AnnexureII - BillWise"
    '
    '            CmpDet.Name = ""
    '            CmpDet.Add1 = "": CmpDet.Add2 = "": CmpDet.Add3 = "": CmpDet.Add4 = ""
    '
    '            Rpt.Company_Name = ""
    '            Rpt.Company_Address = ""
    '
    '            Set Rt1 = New ADODB.Recordset
    '                Rt1.Open "Select * from Company_Head where Company_Idno in " & Trim(RptInp(0).Value), CON, adOpenStatic, adLockReadOnly
    '                If Not (Rt1.BOF And Rt1.EOF) Then
    '                    Rt1.MoveFirst
    '                    CmpNm = Rt1!Company_Name
    '                    CmpAdd1 = Rt1!Company_Address1
    '                    CmpAdd2 = Rt1!Company_Address2
    '                    CmpAdd3 = Rt1!Company_Address3
    '                    CmpAdd4 = Rt1!Company_Address4
    '                    CmpTin = Rt1!Company_TinNo
    '                End If
    '                Rt1.Close
    '            Set Rt1 = Nothing
    '
    '            CON.Execute "Truncate Table ReportTemp"
    '            CON.Execute "Insert into ReportTemp(Meters10, Date1, Name1, Name2, Name3, Int1, Name4, Currency1, Meters1, currency2, name5) select a.for_Orderby, a.Invoice_Date, a.Invoice_Code, left(a.Invoice_No,len(a.Invoice_No)-6) as RefNo, left(a.Invoice_No,len(a.Invoice_No)-6) as Bill_No, a.Ledger_Idno, c.Ledger_TinNo as Commodity_Code, a.Tax_Assesablevalue1 as Purchase_Value, b.Tax_Percentage, a.Tax_Amount1, c.Ledger_CstNo as Category from Invoice_Head a, Tax_Head b, Ledger_head c where a.Company_IdNo in " & Trim(RptInp(0).Value) & " and a.Invoice_Code LIKE '%/" & Trim(CmpDet.FnYear) & "' and month(a.Invoice_Date) IN " & Trim(RptInp(1).Value) & " and a.Tax_IdNo1 = b.Tax_Idno and a.SalesAc_IdNo = c.Ledger_Idno"
    '            CON.Execute "Insert into ReportTemp(Meters10, Date1, Name1, Name2, Name3, Int1, Name4, Currency1, Meters1, currency2, name5) select a.for_Orderby, a.Invoice_Date, a.Invoice_Code, left(a.Invoice_No,len(a.Invoice_No)-6) as RefNo, left(a.Invoice_No,len(a.Invoice_No)-6) as Bill_No, a.Ledger_Idno, c.Ledger_TinNo as Commodity_Code, a.Tax_Assesablevalue2 as Purchase_Value, b.Tax_Percentage, a.Tax_Amount2, c.Ledger_CstNo as Category from Invoice_Head a, Tax_Head b, Ledger_head c where a.Company_IdNo in " & Trim(RptInp(0).Value) & " and a.Invoice_Code LIKE '%/" & Trim(CmpDet.FnYear) & "' and month(a.Invoice_Date) IN " & Trim(RptInp(1).Value) & " and a.Tax_IdNo2 = b.Tax_Idno and a.SalesAc_IdNo = c.Ledger_Idno"
    '
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select 0 as AUTO_SLNO, Date1 as Party_Bill_Date, name3 as Party_Bill_No, b.ledger_name, (case when Ledger_Address4 <> '' then Ledger_Address4 when Ledger_Address3 <>  '' then Ledger_Address3 when Ledger_Address2 <> '' then Ledger_Address2 else Ledger_Address1 end) as Place, Ledger_TinNo, name4 as Commodity_Code, currency1 as Purchase_Value, Meters1 as Rate_of_Tax, currency2 as Vat_Amount, name5 as Category from ReportTemp a, Ledger_Head b where a.Int1 = b.Ledger_IdNo order by a.Date1, a.Meters10, a.Name1", _
    '                        "<[LEN6]SL.NO |<[LEN11]BILL DATE |<[LEN10]BILL NO |<[LEN30]NAME OF THE BUYER |<[LEN15]PLACE |<[LEN16]BUYERS TIN |<[LEN15]COMMODITY CODE  |>@[LEN15][ZS]SALES VALUE |>[LEN12][ZS]RATE OF TAX  |>@[LEN12][ZS]VAT PAID  |<[LEN9]CATEGORY ", "ANNEXURE - II | DETAILS OF SALES DURING THE MONTH " & RptInp(1).Caption & "  " & IIf(Month_NameToIdno((RptInp(1).Caption)) > 3, Format(CmpDet.FromDate, "yyyy"), Format(CmpDet.ToDate, "yyyy")) & "| COMPANY NAME : " & CmpNm & "   -  TIN NO : " & CmpTin & " | " & CmpAdd1 & " " & CmpAdd2 & " " & CmpAdd3 & " " & CmpAdd4, Me, "|dd-mm-yyyy||||||2|2|2|", "TOTAL|1", [Ledger 136Cols], Portrait, [Vertical Line], [Draft 12cpi]
    '
    '        Case "AnnexureII"
    '
    '            CmpDet.Name = ""
    '            CmpDet.Add1 = "": CmpDet.Add2 = "": CmpDet.Add3 = "": CmpDet.Add4 = ""
    '
    '            Rpt.Company_Name = ""
    '            Rpt.Company_Address = ""
    '
    '            Set Rt1 = New ADODB.Recordset
    '                Rt1.Open "Select * from Company_Head where Company_Idno in " & Trim(RptInp(0).Value), CON, adOpenStatic, adLockReadOnly
    '                If Not (Rt1.BOF And Rt1.EOF) Then
    '                    Rt1.MoveFirst
    '                    CmpNm = Rt1!Company_Name
    '                    CmpAdd1 = Rt1!Company_Address1
    '                    CmpAdd2 = Rt1!Company_Address2
    '                    CmpAdd3 = Rt1!Company_Address3
    '                    CmpAdd4 = Rt1!Company_Address4
    '                    CmpTin = Rt1!Company_TinNo
    '                End If
    '                Rt1.Close
    '            Set Rt1 = Nothing
    '
    '            CON.Execute "Truncate Table ReportTemp"
    '            CON.Execute "Insert into ReportTemp(Int1, Name4, Currency1, Meters1, currency2, name5) select a.Ledger_Idno, c.Ledger_TinNo as Commodity_Code, sum(a.Tax_Assesablevalue1) as Sales_Value, b.Tax_Percentage, sum(a.Tax_Amount1), c.Ledger_CstNo as Category from Invoice_Head a, Tax_Head b, Ledger_head c where a.Company_IdNo in " & Trim(RptInp(0).Value) & " and a.Invoice_Code LIKE '%/" & Trim(CmpDet.FnYear) & "' and month(a.Invoice_Date) IN " & Trim(RptInp(1).Value) & " and a.Tax_IdNo1 = b.Tax_Idno and a.SalesAc_IdNo = c.Ledger_Idno Group by a.Ledger_Idno, c.Ledger_TinNo, b.Tax_Percentage, c.Ledger_CstNo"
    '            CON.Execute "Insert into ReportTemp(Int1, Name4, Currency1, Meters1, currency2, name5) select a.Ledger_Idno, c.Ledger_TinNo as Commodity_Code, sum(a.Tax_Assesablevalue2) as Sales_Value, b.Tax_Percentage, sum(a.Tax_Amount2), c.Ledger_CstNo as Category from Invoice_Head a, Tax_Head b, Ledger_head c where a.Company_IdNo in " & Trim(RptInp(0).Value) & " and a.Invoice_Code LIKE '%/" & Trim(CmpDet.FnYear) & "' and month(a.Invoice_Date) IN " & Trim(RptInp(1).Value) & " and a.Tax_IdNo2 = b.Tax_Idno and a.SalesAc_IdNo = c.Ledger_Idno Group by a.Ledger_Idno, c.Ledger_TinNo, b.Tax_Percentage, c.Ledger_CstNo"
    '
    '            CON.Execute "Truncate Table ReportTempSub"
    '            CON.Execute "Insert into ReportTempSub(Int1, Name4, Currency1, Meters1, currency2, name5) select Int1, Name4, sum(Currency1), Meters1, sum(currency2), name5 from ReportTemp group by Int1, Name4, Meters1, name5"
    '
    '            Rpt.Report_Show RptDet.RptCode_Main, CON.ConnectionString, "Select 0 as AUTO_SLNO, b.ledger_name, (case when Ledger_Address4 <> '' then Ledger_Address4 when Ledger_Address3 <>  '' then Ledger_Address3 when Ledger_Address2 <> '' then Ledger_Address2 else Ledger_Address1 end) as Place, Ledger_TinNo, name4 as Commodity_Code, currency1 as Purchase_Value, Meters1 as Rate_of_Tax, currency2 as Vat_Amount, name5 as Category from ReportTempSub a, Ledger_Head b where a.Int1 = b.Ledger_IdNo order by Meters1, b.ledger_name", _
    '                        "<[LEN6]SL.NO |<[LEN30]NAME OF THE BUYER |<[LEN15]PLACE |<[LEN16]BUYERS TIN |<[LEN15]COMMODITY CODE  |>@[LEN15][ZS]SALES VALUE |>[LEN12][ZS]RATE OF TAX  |>@[LEN12][ZS]VAT PAID  |<[LEN9]CATEGORY ", "ANNEXURE - II | DETAILS OF SALES DURING THE MONTH " & RptInp(1).Caption & "  " & IIf(Month_NameToIdno((RptInp(1).Caption)) > 3, Format(CmpDet.FromDate, "yyyy"), Format(CmpDet.ToDate, "yyyy")) & "| COMPANY NAME : " & CmpNm & "   -  TIN NO : " & CmpTin & " | " & CmpAdd1 & " " & CmpAdd2 & " " & CmpAdd3 & " " & CmpAdd4, Me, "|||||2|2|2|", "TOTAL|1", [Ledger 136Cols], Portrait, [Vertical Line], [Draft 12cpi]
    '
    '    End Select
    '
    'End Sub
    '
    'Private Sub Report_Intialize(Cn2 As ADODB.Connection)
    '    Dim i As Integer, J As Integer
    '    Dim RS As ADODB.Recordset
    '
    '    Heading_1 = "": Heading_2 = "": Field_1 = "": Field_2 = "": Format_1 = "": Format_2 = ""
    '    If Trim(Report_PKey) <> "" Then
    '        Set RS = New ADODB.Recordset
    '            RS.Open "Select * from Report_Inputs_Head where pkey in ( " & Trim(Report_PKey) & " ) order by for_orderby", Cn2, adOpenStatic, adLockReadOnly
    '            If Not (RS.BOF And RS.EOF) Then
    '                RS.MoveFirst
    '                Do While Not RS.EOF
    '                    Field_2 = Field_2 & IIf(RS!Input_Type = "C", "t" & Trim(RS!PKey) & ".", "") & Trim(RS!Selection_field_name) & ","
    '                    Heading_2 = Heading_2 & "<[LEN" & Trim(RS!Field_Length) & "]" & RS!Display & "|"
    '                    Format_2 = Format_2 & "|"
    '                    RS.MoveNext
    '                Loop
    '            End If
    '        Set RS = Nothing
    '    End If
    '
    '    If Trim(UCase(User.Type)) = "UNACCOUNT" Then
    '        CompType_Condt = ""
    '    Else
    '        CompType_Condt = "(Company_Type = '" & Trim(User.Type) & "')"
    '    End If
    '
    '    Condt = Trim(CompType_Condt)
    '    Rpt_Hd = UCase(RptDet.RptCode_Main) & " - "
    '    For i = 0 To 4
    '        If (RptInp(i).Value <> "" Or RptInp(i).Total > 0) Then
    '            If RptInp(i).Value <> "" Then
    '                Condt = Condt & IIf(Condt <> "", " and ", "") & IIf(RptInp(i).Input_Type = "C", "t" & Trim(RptInp(i).PKey) & ".", "") & IIf(Trim(RptInp(i).Return_Field) <> "", Trim(RptInp(i).Return_Field), Trim(RptInp(i).Selection_Field)) & " in " & IIf(RptInp(i).Input_Type = "C", RptInp(i).Value, Replace(RptInp(i).Value, ",", "','"))
    '                If InStr(RptInp(i).Value, ",") = 0 Then Rpt_Hd = Rpt_Hd & RptInp(i).Report_Display & " : " & RptInp(i).Caption & " - "
    '                If i = 0 Then Rpt_Hd = Rpt_Hd & "|"
    '            End If
    '            If RptInp(i).Total > 0 Then
    '                Field_1 = Field_1 & IIf(RptInp(i).Input_Type = "C", "t" & Trim(RptInp(i).PKey) & ".", "") & Trim(RptInp(i).Selection_Field) & ","
    '                Heading_1 = Heading_1 & "<" & IIf(Trim(Heading_1) = "", "[GP=]", "=") & "[LEN" & Trim(RptInp(i).Field_Length) & "]" & IIf(InStr(RptInp(i).Value, ",") = 0 And Trim(RptInp(i).Value) <> "", "[HIDDEN]", "") & IIf(RptInp(i).Total > 0, "[SUB_GP]", "") & RptInp(i).Report_Display & "|"
    '                Format_1 = Format_1 & "|"
    '                Field_2 = Replace(Field_2, IIf(RptInp(i).Input_Type = "C", "t" & Trim(RptInp(i).PKey) & ".", "") & Trim(RptInp(i).Selection_Field) & ",", "")
    '                Heading_2 = Replace(Heading_2, "<[LEN" & Trim(RptInp(i).Field_Length) & "]" & RptInp(i).Report_Display & "|", "")
    '                Format_2 = Replace(Format_2, "|", "", , 1)
    '            Else
    '                If (InStr(RptInp(i).Value, ",")) = 0 Then
    '                    J = InStr(Heading_2, RptInp(i).Report_Display)
    '                    If J > 0 Then Heading_2 = Left(Heading_2, J - 1) & "[HIDDEN]" & Right(Heading_2, Len(Heading_2) - J + 1)
    '                End If
    '            End If
    '            'If RptDet.RptCode_Sub = "Accounts" Then RptInp(i).Value = Replace(RptInp(i).Value, "(", ""): RptInp(i).Value = Replace(RptInp(i).Value, ")", "")
    '        End If
    '    Next i
    '    If InStr(RptDet.Inputs, "2") > 0 Then
    '        Rpt_Hd = Rpt_Hd & "RANGE : " & Trim(Format(RptDet.Date1, "dd/mm/yyyy")) & " TO " & Trim(Format(RptDet.Date2, "dd/mm/yyyy"))
    '    ElseIf InStr(RptDet.Inputs, "1") > 0 Then
    '        Rpt_Hd = Rpt_Hd & "AS ON : " & Trim(Format(RptDet.Date1, "dd/mm/yyyy"))
    '    End If
    '
    'End Sub

End Class