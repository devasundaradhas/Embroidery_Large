Imports System.IO
Imports Excel = Microsoft.Office.Interop.Excel

Public Class Report_Details

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private RptHeading1 As String
    Private RptHeading2 As String
    Private RptHeading3 As String
    Private CompName As String
    Private CompAdd1 As String
    Private CompAdd2 As String

    Private RptIpDet_ReportGroupName As String = ""
    Private RptIpDet_ReportName As String = ""
    Private RptIpDet_ReportHeading As String = ""
    Private RptIpDet_IsGridReport As Boolean = False
    Private RptIpDet_ReportInputs As String = ""

    Dim ShowCompCol_STS As Boolean = False
    Dim Mth_IdNo As Integer = 0, Mid As Integer = 0
    Dim GrpCd As String = ""
    Dim H1 As String = "", H2 As String = ""
    Dim Ttc As Single = 0, Ttd As Single = 0
    Dim Fnt As Single = 0
    Dim Tot_CR As Decimal = 0, Tot_DB As Decimal = 0
    Dim NtTt_CR As Decimal = 0, NtTt_DB As Decimal = 0
    Dim Opds As String = "", Clds As String = "", Opn As String = ""
    Dim m1 As Integer = 0, a1 As Integer = 0
    Dim PrevGrpNm As String = ""
    Dim GrpHdRwNo As Long = 0

    Dim IpColNm1 As String = "", IpColVal1 As String = ""
    Dim IpColNm2 As String = ""
    Dim IpColNm3 As String = ""
    Dim IpColNm4 As String = ""
    Dim IpColNm5 As String = ""

    Public Structure SubReport_InputDetails
        Dim PKey As String
        Dim TableName As String
        Dim Selection_FieldName As String
        Dim Return_FieldName As String
        Dim Condition As String
        Dim Display_Name As String
        Dim BlankFieldCondition As String
        Dim CtrlType_Cbo_OR_Txt As String
    End Structure

    Public Structure SubReport_Details
        Dim ReportName As String
        Dim ReportGroupName As String
        Dim ReportHeading As String
        Dim ReportInputs As String
        Dim IsGridReport As Boolean

        Dim CurrentRowVal As Integer
        Dim TopRowVal As Integer

        Dim DateInp_Value1 As Date
        Dim DateInp_Value2 As Date
        Dim CboInp_Text1 As String
        Dim CboInp_Text2 As String
        Dim CboInp_Text3 As String
        Dim CboInp_Text4 As String
        Dim CboInp_Text5 As String

    End Structure
    Public RptSubReportDet(10) As SubReport_Details
    Public RptSubReport_Index As Integer = 0
    Public RptSubReportInpDet(10, 10) As SubReport_InputDetails

    Private Sub clear()

        pnl_Back.Enabled = True

        'lbl_ReportHeading.Text = ""
        If dtp_ToDate.Visible = False Then
            dtp_FromDate.Text = Date.Today  ' Common_Procedures.Company_ToDate
        Else
            dtp_FromDate.Text = Common_Procedures.Company_FromDate
        End If
        dtp_ToDate.Text = Date.Today  ' Common_Procedures.Company_ToDate

        cbo_Company.Text = ""
        cbo_ItemName.Text = ""
        cbo_Ledger.Text = ""
        cbo_GroupName.Text = ""
        cbo_SerialNo.Text = ""
        cbo_PhoneNo.Text = ""
        cbo_SizeName.Text = ""
        cbo_ItemGroupName.Text = ""
        cbo_Agent.Text = ""
        cbo_Transport.Text = ""
        Cbo_SalesMan.Text = ""
        txt_Inputs1.Text = ""

        If txt_Inputs1.Visible = True Then
            If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "party balance - daywise" Then
                txt_Inputs1.Text = "30,60,90"
            End If
        End If

    End Sub

    Private Sub Report_Details_Layout(ByVal sender As Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles Me.Layout

    End Sub

    Private Sub Report_Details_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim dt9 As New DataTable
        Dim dt10 As New DataTable
        Dim dt11 As New DataTable
        Dim CompCondt As String = ""


        RptIpDet_ReportGroupName = Common_Procedures.RptInputDet.ReportGroupName
        RptIpDet_ReportName = Common_Procedures.RptInputDet.ReportName
        RptIpDet_ReportHeading = Common_Procedures.RptInputDet.ReportHeading
        RptIpDet_IsGridReport = Common_Procedures.RptInputDet.IsGridReport
        RptIpDet_ReportInputs = Common_Procedures.RptInputDet.ReportInputs

        'Common_Procedures.RptInputDet.ReportGroupName = ""
        'Common_Procedures.RptInputDet.ReportName = ""
        'Common_Procedures.RptInputDet.ReportHeading = ""
        'Common_Procedures.RptInputDet.IsGridReport = False
        'Common_Procedures.RptInputDet.ReportInputs = ""

        Me.Text = ""
        Me.BackColor = Color.LightSkyBlue   ' Color.FromArgb(203, 213, 228)   'Color.Blue 
        pnl_Back.BackColor = Me.BackColor     'Color.Red  
        pnl_ReportInputs.BackColor = Me.BackColor     'Color.Green  
        pnl_ReportDetails.BackColor = Me.BackColor     'Color.Yellow 

        Me.Left = 0
        Me.Top = 0
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width - 12  ' Screen.PrimaryScreen.Bounds.Width
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height - 90 ' Screen.PrimaryScreen.Bounds.Height

        pnl_Back.Location = New Point(0, 0)
        pnl_ReportInputs.Location = New Point(0, 0)
        pnl_ReportDetails.Location = New Point(0, 0)

        pnl_ReportDetails.Height = Screen.PrimaryScreen.WorkingArea.Height - 90 - pnl_ReportInputs.Height

        pnl_Back.Dock = DockStyle.Fill
        pnl_ReportInputs.Dock = DockStyle.Top
        pnl_ReportDetails.Dock = DockStyle.Bottom
        RptViewer.Dock = DockStyle.Fill

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select item_name from item_head order by item_name", con)
        da.Fill(dt2)
        cbo_ItemName.DataSource = dt2
        cbo_ItemName.DisplayMember = "item_name"

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        da = New SqlClient.SqlDataAdapter("select Company_ShortName from company_head " & IIf(Trim(CompCondt) <> "", " Where ", "") & Trim(CompCondt) & " order by Company_ShortName", con)
        da.Fill(dt3)
        cbo_Company.DataSource = dt3
        cbo_Company.DisplayMember = "Company_ShortName"

        da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head order by AccountsGroup_Name", con)
        da.Fill(dt4)
        cbo_GroupName.DataSource = dt4
        cbo_GroupName.DisplayMember = "AccountsGroup_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Serial_No) from Sales_Details order by Serial_No", con)
        da.Fill(dt5)
        cbo_SerialNo.DataSource = dt5
        cbo_SerialNo.DisplayMember = "Serial_No"

        da = New SqlClient.SqlDataAdapter("select distinct(Party_PhoneNo) from Sales_Head order by Party_PhoneNo", con)
        da.Fill(dt6)
        cbo_PhoneNo.DataSource = dt6
        cbo_PhoneNo.DisplayMember = "Party_PhoneNo"

        da = New SqlClient.SqlDataAdapter("select Size_Name from Size_Head order by Size_Name", con)
        da.Fill(dt7)
        cbo_SizeName.DataSource = dt7
        cbo_SizeName.DisplayMember = "Size_Name"

        da = New SqlClient.SqlDataAdapter("select itemgroup_name from itemgroup_head order by itemgroup_name", con)
        da.Fill(dt8)
        cbo_ItemGroupName.DataSource = dt8
        cbo_ItemGroupName.DisplayMember = "itemgroup_name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt9)
        cbo_Agent.DataSource = dt9
        cbo_Agent.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Transport_Name from Transport_Head order by Transport_Name", con)
        da.Fill(dt10)
        cbo_Transport.DataSource = dt10
        cbo_Transport.DisplayMember = "Transport_Name"

        da = New SqlClient.SqlDataAdapter("select Salesman_Name from Salesman_Head order by Salesman_Name", con)
        da.Fill(dt11)
        Cbo_SalesMan.DataSource = dt11
        Cbo_SalesMan.DisplayMember = "Salesman_Name"

        lbl_ReportHeading.Text = Trim(UCase(Common_Procedures.RptInputDet.ReportHeading))

        Design_ReportInputs()

        clear()

    End Sub

    Private Sub Purchase_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub Report_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub

    Private Sub Report_Details_Load111(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        'Dim cmd As New SqlClient.SqlCommand
        'Dim Da As New SqlClient.SqlDataAdapter
        'Dim MyDtbl1 As New DataTable
        'Dim MyDtbl2 As New DataTable

        'Me.WindowState = FormWindowState.Maximized
        'Me.ReportViewer1.Dock = DockStyle.Fill
        ''ReportViewer1.LocalReport.DataSources.Clear()
        ''ReportViewer1.LocalReport.Refresh()
        ''ReportViewer1.RefreshReport()

        ''con.Open()

        ''cmd.Connection = con

        ''cmd.CommandText = "Truncate table ReportTemp"
        ''cmd.ExecuteNonQuery()


        ''cmd.CommandText = "Insert into ReportTemp(Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Meters1, Date1, Name3, Int2, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Currency9, Currency10 ) select f.Company_Name, (f.Company_Address1 + ', ' + f.Company_Address2), (f.Company_Address3 + ', ' + f.Company_Address4), 'PURCHASE DETAILS', 'DATE RANGE : 01-04-2014 TO 31-03-2015', '',  a.Purchase_Code, a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, a.Sub_Total, a.Total_DiscountAmount, a.Total_TaxAmount, a.Gross_Amount, a.CashDiscount_Amount, a.AddLess_Amount, a.Net_Amount, b.SL_No, b.Noof_Items, b.Rate, b.Amount, c.Ledger_Name, d.Item_Name, e.Unit_Name from Purchase_Head a LEFT OUTER JOIN Purchase_Details b ON A.Purchase_Code = B.Purchase_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = D.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo Order by a.Purchase_Date, a.for_OrderBy, a.Purchase_No, a.Company_IdNo"
        ''cmd.ExecuteNonQuery()

        ''Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Int1, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Currency9, Currency10 from ReportTemp Order by Date1, Meters1, Name1, Name2", con)
        ''Da.Fill(MyDtbl1)

        ' ''Da = New SqlClient.SqlDataAdapter("select a.Purchase_Code, a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, a.Sub_Total, a.Total_DiscountAmount, a.Total_TaxAmount, a.Gross_Amount,   a.CashDiscount_Amount, a.AddLess_Amount, a.Net_Amount, b.SL_No, b.Noof_Items, b.Rate, b.Amount, c.Ledger_Name, d.Item_Name, e.Unit_Name, f.Company_Name, f.Company_Address1, f.Company_Address2, f.Company_Address3, f.Company_Address4 from Purchase_Head a LEFT OUTER JOIN Purchase_Details b ON A.Purchase_Code = B.Purchase_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = D.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo Order by a.Purchase_Date, a.for_OrderBy, a.Purchase_No, a.Company_IdNo", con)
        ' ''Da.Fill(MyDtbl2)

        ''con.Close()

        ''Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource
        ' ''Dim RpDs2 As New Microsoft.Reporting.WinForms.ReportDataSource

        ' ''Here We have to mention the actual Report's Dataset Name

        ''RpDs1.Name = "DataSet1"
        ''RpDs1.Value = MyDtbl1

        ' ''RpDs2.Name = "DataSet2"
        ' ''RpDs2.Value = MyDtbl2


        ''ReportViewer1.LocalReport.ReportPath = "D:\TSOFT\Inventory_VB\Inventory\Inventory\Report2.rdlc"

        ''ReportViewer1.LocalReport.DataSources.Clear()

        ''ReportViewer1.LocalReport.DataSources.Add(RpDs1)
        ' ''ReportViewer1.LocalReport.DataSources.Add(RpDs2)

        'Me.ReportTempTableAdapter.Fill(Me.InventoryDataSet.ReportTemp)

        'ReportViewer1.LocalReport.Refresh()
        'ReportViewer1.RefreshReport()

        'ReportViewer1.Visible = True


        ''---------------------------------------
        ''---------------------------------------

        ''con.Open()

        ''cmd.Connection = con

        ''cmd.CommandText = "Truncate table ReportTemp"
        ''cmd.ExecuteNonQuery()

        ''cmd.CommandText = "Insert into ReportTemp(Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Currency1) Select 'TSOFT SOLUTIONS', '160, P.N ROAD', 'TIRUPUR - 2', 'PURCHASE REGISTER', 'DATE RANGE : 01-04-2014 TO 31-03-2015', '', a.Purchase_Code, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, b.Ledger_Name, a.Net_Amount from Purchase_Head a, Ledger_Head b where a.ledger_idno = b.ledger_idno"
        ''cmd.ExecuteNonQuery()

        ''Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Currency1 from ReportTemp Order by Date1, Meters1, Name1, Name2", con)
        ''Da.Fill(MyDtbl1)

        ''con.Close()

        ' ''ReportViewer1.RefreshReport()

        ''Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource

        ' ''Here We have to mention the actual Report's Dataset Name

        ''RpDs1.Name = "DataSet1"

        ''RpDs1.Value = MyDtbl1



        ' ''ReportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "Report1.rdlc  ")

        ' ''Select Case CType(ComboBox1.SelectedItem, String).ToLower

        ' ''    Case "rpttest"

        ' ''        rptView.LocalReport.ReportPath = Path.Combine(Application.StartupPath, "rptTest.rdlc")

        ' ''    Case "rptallrepairs"

        ' ''        rptView.LocalReport.ReportPath = Path.Combine(Application.StartupPath, "rptAllRepairs.rdlc")

        ' ''End Select

        ''ReportViewer1.LocalReport.ReportPath = "D:\TSOFT\Inventory_VB\Inventory\Inventory\Report1.rdlc"

        ''ReportViewer1.LocalReport.DataSources.Clear()

        ''ReportViewer1.LocalReport.DataSources.Add(RpDs1)
        ''ReportViewer1.LocalReport.Refresh()
        ''ReportViewer1.RefreshReport()

        ''ReportViewer1.Visible = True


        ''---------------------------------------
        ''---------------------------------------

        ' ''TODO: This line of code loads data into the 'InventoryDataSet.ReportTemp' table. You can move, or remove it, as needed.
        ''Me.ReportTempTableAdapter.Fill(Me.InventoryDataSet.ReportTemp)

        ' ''Me.ReportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout)
        ''Me.ReportViewer1.RefreshReport()

    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btn_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        Show_Report()
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_ItemName.Text.Length
        End With
    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown

        Try
            With cbo_ItemName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If cbo_ItemGroupName.Visible And cbo_ItemGroupName.Enabled Then
                        cbo_ItemGroupName.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                        cbo_GroupName.Focus()
                    ElseIf cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_SizeName.Visible And cbo_SizeName.Enabled Then
                        cbo_SizeName.Focus()
                    ElseIf cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                        cbo_SerialNo.Focus()
                    ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                        txt_Inputs1.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                    End If

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_ItemName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_ItemName
                            If Trim(.Text) <> "" Then
                                If .DroppedDown = True Then
                                    If Trim(.SelectedText) <> "" Then
                                        .Text = .SelectedText
                                    Else
                                        If .Items.Count > 0 Then
                                            .SelectedIndex = 0
                                            .SelectedItem = .Items(0)
                                            .Text = .GetItemText(.SelectedItem)
                                        End If
                                    End If
                                End If
                            End If
                        End With

                        If cbo_SizeName.Visible And cbo_SizeName.Enabled Then
                            cbo_SizeName.Focus()
                        ElseIf cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                            cbo_SerialNo.Focus()
                        ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                            txt_Inputs1.Focus()
                        Else
                            btn_Show.Focus()
                            Show_Report()
                        End If

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where item_name like '" & Trim(FindStr) & "%' or item_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select item_name from item_head " & Condt & " order by item_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "item_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        cbo_ItemName.BackColor = Color.White
        cbo_ItemName.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        With cbo_Ledger
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            With cbo_Ledger
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                        cbo_ItemName.Focus()
                    ElseIf cbo_Agent.Visible And cbo_Agent.Enabled Then
                        cbo_Agent.Focus()
                    ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                        txt_Inputs1.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                    End If
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Ledger

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        If cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                            cbo_ItemName.Focus()
                        ElseIf cbo_Agent.Visible And cbo_Agent.Enabled Then
                            cbo_Agent.Focus()
                        ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                            txt_Inputs1.Focus()
                        Else
                            btn_Show.Focus()
                            Show_Report()
                        End If
                        'SendKeys.Send("+{TAB}")

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead " & Condt & " order by Ledger_DisplayName", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Ledger_DisplayName"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        With cbo_Ledger
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub


    Private Sub cbo_ItemGroupName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroupName.GotFocus
        With cbo_ItemGroupName
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_ItemGroupName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroupName.KeyDown
        Try
            With cbo_ItemGroupName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                        cbo_ItemName.Focus()
                    ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                        txt_Inputs1.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                    End If
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ItemGroupName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroupName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_ItemGroupName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        If cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                            cbo_ItemName.Focus()
                        ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                            txt_Inputs1.Focus()
                        Else
                            btn_Show.Focus()
                            Show_Report()
                        End If
                        'SendKeys.Send("+{TAB}")

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where itemgroup_name like '" & FindStr & "%' or itemgroup_name like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select itemgroup_name from itemgroup_head " & Condt & " order by itemgroup_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "itemgroup_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_ItemGroupName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroupName.LostFocus
        With cbo_ItemGroupName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        With cbo_Agent

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_type = 'AGENT')", "(Ledger_IdNo = 0)")

            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_type = 'AGENT')", "(Ledger_IdNo = 0)")

            With cbo_Agent
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If cbo_ItemGroupName.Visible And cbo_ItemGroupName.Enabled Then
                        cbo_ItemGroupName.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                        cbo_GroupName.Focus()
                    ElseIf cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_SizeName.Visible And cbo_SizeName.Enabled Then
                        cbo_SizeName.Focus()
                    ElseIf cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                        cbo_SerialNo.Focus()
                    ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                        txt_Inputs1.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

            With cbo_Agent

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If cbo_SizeName.Visible And cbo_SizeName.Enabled Then
                            cbo_SizeName.Focus()
                        ElseIf cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                            cbo_SerialNo.Focus()
                        ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                            txt_Inputs1.Focus()
                        Else
                            btn_Show.Focus()
                            Show_Report()
                        End If


                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Agent_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.LostFocus
        cbo_Agent.BackColor = Color.White
        cbo_Agent.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        With cbo_Transport

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")

            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, Nothing, Nothing, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")

            With cbo_Transport
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If cbo_ItemGroupName.Visible And cbo_ItemGroupName.Enabled Then
                        cbo_ItemGroupName.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                        cbo_GroupName.Focus()
                    ElseIf cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_SizeName.Visible And cbo_SizeName.Enabled Then
                        cbo_SizeName.Focus()
                    ElseIf cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                        cbo_SerialNo.Focus()
                    ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                        txt_Inputs1.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")

            With cbo_Transport

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If cbo_SizeName.Visible And cbo_SizeName.Enabled Then
                            cbo_SizeName.Focus()
                        ElseIf cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                            cbo_SerialNo.Focus()
                        ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                            txt_Inputs1.Focus()
                        Else
                            btn_Show.Focus()
                            Show_Report()
                        End If


                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Transport_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.LostFocus
        cbo_Transport.BackColor = Color.White
        cbo_Transport.ForeColor = Color.Black
    End Sub

    Private Sub Cbo_SalesMan_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_SalesMan.GotFocus
        With Cbo_SalesMan

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Salesman_Head", "Salesman_Name", "", "(Salesman_IdNo = 0)")

            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub Cbo_SalesMan_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_SalesMan.KeyDown

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_SalesMan, Nothing, Nothing, "Salesman_Head", "Salesman_Name", "", "(Salesman_IdNo = 0)")

            With Cbo_SalesMan
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If cbo_ItemGroupName.Visible And cbo_ItemGroupName.Enabled Then
                        cbo_ItemGroupName.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                        cbo_GroupName.Focus()
                    ElseIf cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_SizeName.Visible And cbo_SizeName.Enabled Then
                        cbo_SizeName.Focus()
                    ElseIf cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                        cbo_SerialNo.Focus()
                    ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                        txt_Inputs1.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cbo_SalesMan_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_SalesMan.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_SalesMan, Nothing, "Salesman_Head", "Salesman_Name", "", "(Salesman_IdNo = 0)")

            With Cbo_SalesMan

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If cbo_SizeName.Visible And cbo_SizeName.Enabled Then
                            cbo_SizeName.Focus()
                        ElseIf cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                            cbo_SerialNo.Focus()
                        ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                            txt_Inputs1.Focus()
                        Else
                            btn_Show.Focus()
                            Show_Report()
                        End If


                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cbo_SalesMan_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_SalesMan.LostFocus
        Cbo_SalesMan.BackColor = Color.White
        Cbo_SalesMan.ForeColor = Color.Black
    End Sub


    Private Sub dtp_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_ToDate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub dtp_FromDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FromDate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        'If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_FromDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FromDate.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_Company_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.GotFocus
        With cbo_Company
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Company_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Company.KeyDown
        Try
            With cbo_Company
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                        cbo_GroupName.Focus()
                    ElseIf cbo_ItemGroupName.Visible And cbo_ItemGroupName.Enabled Then
                        cbo_ItemGroupName.Focus()
                    ElseIf cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                        cbo_ItemName.Focus()
                    ElseIf cbo_Agent.Visible And cbo_Agent.Enabled Then
                        cbo_Agent.Focus()
                    ElseIf cbo_Transport.Visible And cbo_Transport.Enabled Then
                        cbo_Transport.Focus()
                    End If
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Company_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim CompCondt As String

        Try

            With cbo_Company

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        If cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                            cbo_Ledger.Focus()
                        ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                            cbo_GroupName.Focus()
                        ElseIf cbo_ItemGroupName.Visible And cbo_ItemGroupName.Enabled Then
                            cbo_ItemGroupName.Focus()
                        ElseIf cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                            cbo_ItemName.Focus()
                        ElseIf cbo_Agent.Visible And cbo_Agent.Enabled Then
                            cbo_Agent.Focus()
                        ElseIf cbo_Transport.Visible And cbo_Transport.Enabled Then
                            cbo_Transport.Focus()
                        Else
                            btn_Show.Focus()
                            Show_Report()
                        End If

                        'SendKeys.Send("{TAB}")
                        'cbo_PurchaseAc.Focus()

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        CompCondt = ""
                        If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                            CompCondt = "(Company_Type <> 'UNACCOUNT')"
                        End If

                        Condt = IIf(Trim(CompCondt) <> "", " Where ", "") & CompCondt
                        If Trim(FindStr) <> "" Then
                            Condt = " Where " & CompCondt & IIf(CompCondt <> "", " and ", "") & " (Company_ShortName like '" & FindStr & "%' or Company_ShortName like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Company_ShortName from Company_Head " & Condt & " order by Company_ShortName", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Company_ShortName"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Company_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.LostFocus
        With cbo_Company
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub Design_ReportInputs()
        Dim RptInpts As String

        RptInpts = "," & Trim(Common_Procedures.RptInputDet.ReportInputs) & ","

        lbl_FromDate.Visible = True
        dtp_FromDate.Visible = True

        lbl_ToDate.Visible = True
        dtp_ToDate.Visible = True

        lbl_Company.Visible = True
        cbo_Company.Visible = True

        lbl_Ledger.Visible = True
        cbo_Ledger.Visible = True
        cbo_Ledger.Tag = ""

        lbl_ItemGroupName.Visible = True
        cbo_ItemGroupName.Visible = True

        lbl_ItemName.Visible = True
        cbo_ItemName.Visible = True

        lbl_GroupName.Visible = True
        cbo_GroupName.Visible = True

        lbl_SizeName.Visible = True
        cbo_SizeName.Visible = True

        lbl_Agent.Visible = True
        cbo_Agent.Visible = True

        lbl_Transport.Visible = True
        cbo_Transport.Visible = True

        lbl_SerialNo.Visible = True
        cbo_SerialNo.Visible = True

        lbl_PhoneNo.Visible = True
        cbo_PhoneNo.Visible = True

        lbl_TextInputs1.Visible = True
        txt_Inputs1.Visible = True

        lbl_Salesman.Visible = True
        Cbo_SalesMan.Visible = True

        'lbl_GroupName.Left = lbl_Ledger.Left
        'lbl_GroupName.Top = lbl_Ledger.Top

        'cbo_GroupName.Left = cbo_GroupName.Left
        'cbo_GroupName.Top = lbl_Ledger.Top

        If InStr(1, UCase(RptInpts), ",2DT,") = 0 And InStr(1, UCase(RptInpts), ",1DT,") = 0 Then
            lbl_FromDate.Visible = False
            dtp_FromDate.Visible = False
        Else
            If InStr(1, UCase(RptInpts), ",2DT,") = 0 Then
                lbl_FromDate.Text = "Up To :"
            End If
        End If

        If InStr(1, UCase(RptInpts), ",2DT,") = 0 Then
            lbl_ToDate.Visible = False
            dtp_ToDate.Visible = False
        End If

        If InStr(1, UCase(RptInpts), ",Z,") = 0 Then
            lbl_Company.Visible = False
            cbo_Company.Visible = False
        Else
            If InStr(1, UCase(RptInpts), ",2DT,") = 0 And InStr(1, UCase(RptInpts), ",1DT,") = 0 Then
                lbl_Company.Left = lbl_FromDate.Left
                cbo_Company.Left = dtp_FromDate.Left
            End If
        End If

        If InStr(1, UCase(RptInpts), ",L,") = 0 And InStr(1, UCase(RptInpts), ",P,") = 0 And InStr(1, UCase(RptInpts), ",PARFRM,") = 0 And InStr(1, UCase(RptInpts), ",PARTO,") = 0 Then
            lbl_Ledger.Visible = False
            cbo_Ledger.Visible = False
        End If

        lbl_Ledger.Text = "Party Name"
        If InStr(1, UCase(RptInpts), ",PARFRM,") > 0 Then
            lbl_Ledger.Text = "Party From"
            cbo_Ledger.Tag = "PARFRM"
        End If
        If InStr(1, UCase(RptInpts), ",PARTO,") > 0 Then
            lbl_Ledger.Text = "Party To"
            cbo_Ledger.Tag = "PARTO"
        End If


        If InStr(1, UCase(RptInpts), ",IG,") = 0 Then
            lbl_ItemGroupName.Visible = False
            cbo_ItemGroupName.Visible = False

        Else
            If InStr(1, UCase(RptInpts), ",L,") = 0 And InStr(1, UCase(RptInpts), ",P,") = 0 Then
                lbl_ItemGroupName.Left = lbl_Ledger.Left
                lbl_ItemGroupName.Top = lbl_Ledger.Top

                cbo_ItemGroupName.Left = cbo_Ledger.Left
                cbo_ItemGroupName.Top = cbo_Ledger.Top

            Else
                lbl_ItemGroupName.Left = lbl_GroupName.Left
                lbl_ItemGroupName.Top = lbl_GroupName.Top

                cbo_ItemGroupName.Left = cbo_GroupName.Left
                cbo_ItemGroupName.Top = cbo_GroupName.Top
            End If
        End If

        If InStr(1, UCase(RptInpts), ",I,") = 0 Then
            lbl_ItemName.Visible = False
            cbo_ItemName.Visible = False
        Else
            If InStr(1, UCase(RptInpts), ",L,") = 0 And InStr(1, UCase(RptInpts), ",P,") = 0 And InStr(1, UCase(RptInpts), ",IG,") = 0 Then
                lbl_ItemName.Left = lbl_Ledger.Left
                cbo_ItemName.Left = cbo_Ledger.Left
            End If
        End If

        If InStr(1, UCase(RptInpts), ",G,") = 0 Then
            lbl_GroupName.Visible = False
            cbo_GroupName.Visible = False
        Else
            If InStr(1, UCase(RptInpts), ",L,") = 0 And InStr(1, UCase(RptInpts), ",P,") = 0 Then
                lbl_GroupName.Left = lbl_Ledger.Left
                lbl_GroupName.Top = lbl_Ledger.Top

                cbo_GroupName.Left = cbo_GroupName.Left
                cbo_GroupName.Top = lbl_Ledger.Top

            Else
                lbl_GroupName.Left = lbl_ItemName.Left
                lbl_GroupName.Top = lbl_ItemName.Top

                cbo_GroupName.Left = cbo_ItemName.Left
                cbo_GroupName.Top = cbo_ItemName.Top

            End If
        End If


        If InStr(1, UCase(RptInpts), ",SL,") = 0 Then
            lbl_SerialNo.Visible = False
            cbo_SerialNo.Visible = False
        Else
            lbl_SerialNo.Left = lbl_GroupName.Left
            lbl_SerialNo.Top = lbl_GroupName.Top

            cbo_SerialNo.Left = cbo_GroupName.Left
            cbo_SerialNo.Top = cbo_GroupName.Top

        End If

        If InStr(1, UCase(RptInpts), ",PH,") = 0 Then
            lbl_PhoneNo.Visible = False
            cbo_PhoneNo.Visible = False
        End If

        If InStr(1, UCase(RptInpts), ",DY,") = 0 Then
            lbl_TextInputs1.Visible = False
            txt_Inputs1.Visible = False

        Else
            If cbo_ItemName.Visible = False Then
                lbl_TextInputs1.Left = lbl_ItemName.Left
                lbl_TextInputs1.Top = lbl_ItemName.Top

                txt_Inputs1.Left = cbo_ItemName.Left
                txt_Inputs1.Top = cbo_ItemName.Top

            Else
                lbl_TextInputs1.Left = lbl_GroupName.Left
                lbl_TextInputs1.Top = lbl_GroupName.Top

                txt_Inputs1.Left = cbo_GroupName.Left
                txt_Inputs1.Top = cbo_GroupName.Top

            End If

        End If

        If InStr(1, UCase(RptInpts), ",SZ,") = 0 Then
            lbl_SizeName.Visible = False
            cbo_SizeName.Visible = False
        Else
            lbl_SizeName.Left = lbl_GroupName.Left
            lbl_SizeName.Top = lbl_GroupName.Top

            cbo_SizeName.Left = cbo_GroupName.Left
            cbo_SizeName.Top = cbo_GroupName.Top

        End If

        If InStr(1, UCase(RptInpts), ",TR,") = 0 Then
            lbl_Transport.Visible = False
            cbo_Transport.Visible = False

        Else

            If InStr(1, UCase(RptInpts), ",L,") = 0 And InStr(1, UCase(RptInpts), ",P,") = 0 Then
                lbl_Transport.Left = lbl_Ledger.Left
                lbl_Transport.Top = lbl_Ledger.Top

                cbo_Transport.Left = cbo_Ledger.Left
                cbo_Transport.Top = cbo_Ledger.Top

            Else
                lbl_Transport.Left = lbl_ItemName.Left
                lbl_Transport.Top = lbl_ItemName.Top

                cbo_Transport.Left = cbo_ItemName.Left
                cbo_Transport.Top = cbo_ItemName.Top

            End If

        End If

        If InStr(1, UCase(RptInpts), ",AG,") = 0 Then
            lbl_Agent.Visible = False
            cbo_Agent.Visible = False

        Else

            If InStr(1, UCase(RptInpts), ",L,") = 0 And InStr(1, UCase(RptInpts), ",P,") = 0 Then
                lbl_Agent.Left = lbl_Ledger.Left
                lbl_Agent.Top = lbl_Ledger.Top

                cbo_Agent.Left = cbo_Ledger.Left
                cbo_Agent.Top = cbo_Ledger.Top

            Else
                lbl_Agent.Left = lbl_ItemName.Left
                lbl_Agent.Top = lbl_ItemName.Top

                cbo_Agent.Left = cbo_ItemName.Left
                cbo_Agent.Top = cbo_ItemName.Top

            End If

        End If

        If InStr(1, UCase(RptInpts), ",SM,") = 0 Then
            lbl_Salesman.Visible = False
            Cbo_SalesMan.Visible = False

        Else

            If InStr(1, UCase(RptInpts), ",L,") = 0 And InStr(1, UCase(RptInpts), ",P,") = 0 Then
                lbl_Salesman.Left = lbl_Ledger.Left
                lbl_Salesman.Top = lbl_Ledger.Top

                Cbo_SalesMan.Left = cbo_Ledger.Left
                Cbo_SalesMan.Top = cbo_Ledger.Top

            Else
                lbl_Salesman.Left = lbl_ItemName.Left
                lbl_Salesman.Top = lbl_ItemName.Top

                Cbo_SalesMan.Left = cbo_ItemName.Left
                Cbo_SalesMan.Top = cbo_ItemName.Top

            End If

        End If

    End Sub

    Private Sub cbo_GroupName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GroupName.GotFocus
        With cbo_GroupName
            .BackColor = Color.Lime  ' Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_GroupName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GroupName.KeyDown
        Try
            With cbo_GroupName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                        cbo_ItemName.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                    End If
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_GroupName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GroupName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_GroupName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        If cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                            cbo_Ledger.Focus()
                        ElseIf cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                            cbo_ItemName.Focus()
                        Else
                            btn_Show.Focus()
                            Show_Report()
                        End If
                        'SendKeys.Send("+{TAB}")

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        If Trim(FindStr) <> "" Then
                            Condt = " Where AccountsGroup_Name like '" & FindStr & "%' or AccountsGroup_Name like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head " & Condt & " order by AccountsGroup_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "AccountsGroup_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_GroupName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GroupName.LostFocus
        With cbo_GroupName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_SizeName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SizeName.GotFocus
        With cbo_SizeName
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_SizeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SizeName.KeyDown
        Try
            With cbo_SizeName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                        cbo_ItemName.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    'If cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                    '    cbo_Ledger.Focus()
                    'ElseIf cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                    '    cbo_ItemName.Focus()
                    'Else
                    btn_Show.Focus()
                    Show_Report()
                    'End If

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_SizeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SizeName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_SizeName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        'If cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        '    cbo_Ledger.Focus()
                        'ElseIf cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                        '    cbo_ItemName.Focus()
                        'Else
                        btn_Show.Focus()
                        Show_Report()
                        'End If

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Size_Name like '" & FindStr & "%' or Size_Name like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Size_Name from Size_Head " & Condt & " order by Size_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Size_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_SizeName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SizeName.LostFocus
        With cbo_SizeName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '-----
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '-----
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '-----
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '-----
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '-----
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '-----
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '-----
    End Sub

    Private Sub Show_Report()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CompCondt As String = ""
        Dim ParNmCap As String = ""

        Try

            RptHeading1 = "" : RptHeading2 = "" : RptHeading3 = ""

            RptHeading1 = Trim(UCase(Common_Procedures.RptInputDet.ReportHeading))

            If cbo_GroupName.Visible = True And Trim(cbo_GroupName.Text) <> "" Then
                If Trim(RptHeading2) = "" Then
                    RptHeading2 = "GROUP NAME : " & Trim(cbo_GroupName.Text)
                Else
                    RptHeading2 = Trim(RptHeading2) & "    -    " & "GROUP NAME : " & Trim(cbo_GroupName.Text)
                End If
            End If

            If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then


                ParNmCap = "PARTY NAME : "
                If Trim(UCase(cbo_Ledger.Tag)) = "PARFRM" Then
                    ParNmCap = "PARTY FROM : "
                ElseIf Trim(UCase(cbo_Ledger.Tag)) = "PARTO" Then
                    ParNmCap = "PARTY TO : "
                End If

                If Trim(RptHeading2) = "" Then
                    RptHeading2 = ParNmCap & Trim(cbo_Ledger.Text)
                Else
                    RptHeading2 = Trim(RptHeading2) & "    -    " & ParNmCap & Trim(cbo_Ledger.Text)
                End If
            End If

            If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                If Trim(RptHeading2) = "" Then
                    RptHeading2 = "ITEMGROUP NAME : " & Trim(cbo_ItemGroupName.Text)
                Else
                    RptHeading2 = Trim(RptHeading2) & "    -    " & "ITEMGROUP NAME : " & Trim(cbo_ItemGroupName.Text)
                End If
            End If

            If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                If Trim(RptHeading2) = "" Then
                    RptHeading2 = "ITEM NAME : " & Trim(cbo_ItemName.Text)
                Else
                    RptHeading2 = Trim(RptHeading2) & "    -    " & "ITEM NAME : " & Trim(cbo_ItemName.Text)
                End If
            End If

            If cbo_SizeName.Visible = True And Trim(cbo_SizeName.Text) <> "" Then
                If Trim(RptHeading2) = "" Then
                    RptHeading2 = "SIZE : " & Trim(cbo_SizeName.Text)
                Else
                    RptHeading2 = Trim(RptHeading2) & "    -    " & "SIZE : " & Trim(cbo_SizeName.Text)
                End If
            End If

            If cbo_Agent.Visible = True And Trim(cbo_Agent.Text) <> "" Then
                If Trim(RptHeading2) = "" Then
                    RptHeading2 = "AGENT : " & Trim(cbo_Agent.Text)
                Else
                    RptHeading2 = Trim(RptHeading2) & "    -    " & "AGENT : " & Trim(cbo_Agent.Text)
                End If
            End If

            If cbo_Transport.Visible = True And Trim(cbo_Transport.Text) <> "" Then
                If Trim(RptHeading2) = "" Then
                    RptHeading2 = "TRANSPORT : " & Trim(cbo_Transport.Text)
                Else
                    RptHeading2 = Trim(RptHeading2) & "    -    " & "TRANSPORT : " & Trim(cbo_Transport.Text)
                End If
            End If


            If Cbo_SalesMan.Visible = True And Trim(Cbo_SalesMan.Text) <> "" Then
                If Trim(RptHeading2) = "" Then
                    RptHeading2 = "SALESMAN : " & Trim(Cbo_SalesMan.Text)
                Else
                    RptHeading2 = Trim(RptHeading2) & "    -    " & "SALESMAN : " & Trim(Cbo_SalesMan.Text)
                End If
            End If

            If Trim(RptHeading2) = "" Then
                If dtp_FromDate.Visible = True And dtp_ToDate.Visible = True Then
                    RptHeading2 = "DATE RANGE : " & Trim(dtp_FromDate.Text) & " TO " & Trim(dtp_ToDate.Text)
                Else
                    If dtp_FromDate.Visible = True Then RptHeading2 = "UP TO : " & Trim(dtp_FromDate.Text)
                End If
            Else
                If dtp_FromDate.Visible = True And dtp_ToDate.Visible = True Then
                    RptHeading3 = "DATE RANGE : " & Trim(dtp_FromDate.Text) & " TO " & Trim(dtp_ToDate.Text)
                Else
                    If dtp_FromDate.Visible = True Then RptHeading3 = "UP TO : " & Trim(dtp_FromDate.Text)
                End If
            End If

            CompName = ""
            CompAdd1 = ""
            CompAdd2 = ""

            If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then

                Da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name, Company_Address1, Company_Address2, Company_Address3, Company_Address4 from Company_Head Where Company_ShortName = '" & Trim(cbo_Company.Text) & "' Order by Company_IdNo ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        CompName = Dt.Rows(0).Item("Company_Name").ToString
                        CompAdd1 = Dt.Rows(0).Item("Company_Address1").ToString & " " & Dt.Rows(0).Item("Company_Address2").ToString
                        CompAdd2 = Dt.Rows(0).Item("Company_Address3").ToString & " " & Dt.Rows(0).Item("Company_Address4").ToString
                    End If
                End If
                Dt.Clear()

            Else

                CompCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompCondt = "(Company_Type <> 'UNACCOUNT')"
                End If

                Da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name, Company_Address1, Company_Address2, Company_Address3, Company_Address4 from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo ", con)
                Dt = New DataTable
                Da.Fill(Dt)

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        CompName = Dt.Rows(0).Item("Company_Name").ToString
                        CompAdd1 = Dt.Rows(0).Item("Company_Address1").ToString & " " & Dt.Rows(0).Item("Company_Address2").ToString
                        CompAdd2 = Dt.Rows(0).Item("Company_Address3").ToString & " " & Dt.Rows(0).Item("Company_Address4").ToString
                    End If
                End If
                Dt.Clear()

            End If

            Select Case Trim(LCase(Common_Procedures.RptInputDet.ReportGroupName))
                Case "accounts"
                    Accounts_Report()
                Case "register"
                    Register_Report()
                Case "stock"
                    Stock_Report()
            End Select

            RptViewer.Focus()

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Accounts_Report()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dtbl1 As New DataTable
        Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource
        Dim RptCondt As String
        Dim Bal As Decimal = 0
        Dim Amt As Single = 0
        Dim Comp_IdNo As Integer, Led_IdNo As Integer, Grp_IdNo As Integer
        Dim CompCondt As String
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Mnth_ID As Integer = 0
        Dim Yr As Integer = 0
        Dim n As Long = 0
        Dim Mnth_Yr_Nm As String = ""
        Dim b() As String
        Dim S As String, oldvl As String
        Dim BlAmt As Single = 0
        Dim RecAmt As Single = 0
        Dim Nr As Long = 0

        Try

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "(Company_Type <> 'UNACCOUNT')"
            End If

            Select Case Trim(LCase(Common_Procedures.RptInputDet.ReportName))

                Case "single ledger a/c"

                    If IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Visible = True And dtp_FromDate.Enabled = True Then dtp_FromDate.Focus()
                        Exit Sub
                    End If

                    If IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Visible = True And dtp_ToDate.Enabled = True Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                    If Led_IdNo = 0 Then
                        MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
                    cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)


                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Comp_IdNo))
                    End If
                    If cbo_Ledger.Visible = True And Val(Led_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
                    End If

                    Amt = 0

                    cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo, ledger_head b where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date < @fromdate and a.ledger_idno = b.ledger_idno and b.parent_code not like '%~18~'"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            Amt = Val(Dt.Rows(0)(0).ToString)
                        End If
                    End If

                    cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo, ledger_head b Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date >= @companyfromdate and a.voucher_date < @fromdate and a.ledger_idno = b.ledger_idno and b.parent_code like '%~18~'"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            Amt = Amt + Val(Dt.Rows(0)(0).ToString)
                        End If
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp(Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Int5, Meters1, Name1, Name2, Name3, Name4, Name5, Currency1, Currency2 ) values ('" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', 0, 0, 'OPENING', '', 'OPENING', '', '', " & IIf(Amt < 0, Math.Abs(Amt), 0) & ", " & IIf(Amt > 0, Amt, 0) & ") "
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp(Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5) select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', 1, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'To ' + c.ledger_name, Abs(a.voucher_amount), 0, a.narration, a.Voucher_Type from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo , voucher_head b, ledger_head c where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.voucher_amount < 0 and a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno and b.creditor_idno = c.ledger_idno"
                    'cmd.CommandText = "Insert into ReportTemp(Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5) select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', 1, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'To ' + c.ledger_name, Abs(a.voucher_amount), 0, a.narration, ( case when b.entry_identification like 'VOUCH-%' then ('V'+upper(b.voucher_type)+'-'+cast(b.voucher_no as varchar(20))) else left(b.entry_identification, len(b.entry_identification)-6) end ) from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo , voucher_head b, ledger_head c where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.voucher_amount < 0 and a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno and b.creditor_idno = c.ledger_idno"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp(Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5) select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', 1, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'By ' + c.ledger_name, 0, a.Voucher_Amount, a.narration, a.Voucher_Type from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo, voucher_head b, ledger_head c where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.voucher_amount > 0 and a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno and b.debtor_idno = c.ledger_idno"
                    'cmd.CommandText = "Insert into ReportTemp(Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5) select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', 1, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'By ' + c.ledger_name, 0, a.Voucher_Amount, a.narration, ( case when b.entry_identification like 'VOUCH-%' then 'V'+upper(b.voucher_type)+'-'+cast(b.voucher_no as varchar(20)) else left(b.entry_identification, len(b.entry_identification)-6) end ) from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo, voucher_head b, ledger_head c where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.voucher_amount > 0 and a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno and b.debtor_idno = c.ledger_idno"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5 from reporttemp Order by Int5, Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    Bal = 0
                    If Dtbl1.Rows.Count > 0 Then
                        Bal = Val(Dtbl1.Rows(0).Item("Currency1").ToString) - Val(Dtbl1.Rows(0).Item("Currency2").ToString)
                        Dtbl1.Rows(0).Item("Name6") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                        'Dtbl1.Rows(0).Item("Currency3") = Val(Bal)
                        For i = 1 To Dtbl1.Rows.Count - 1
                            Bal = Val(Bal) + Val(Dtbl1.Rows(i).Item("Currency1").ToString) - Val(Dtbl1.Rows(i).Item("Currency2").ToString)
                            Dtbl1.Rows(i).Item("Name6") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                            'Dtbl1.Rows(i).Item("Currency3") = Val(Bal)
                        Next i
                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SingleLedger.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "group ledger"

                    If IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Visible = True And dtp_FromDate.Enabled = True Then dtp_FromDate.Focus()
                        Exit Sub
                    End If

                    If IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Visible = True And dtp_ToDate.Enabled = True Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

                    Grp_IdNo = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_GroupName.Text)
                    If Grp_IdNo = 0 Then
                        MessageBox.Show("Invalid Group Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_GroupName.Enabled And cbo_GroupName.Visible Then cbo_GroupName.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
                    cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Comp_IdNo))
                    End If
                    If cbo_GroupName.Visible = True And Val(Grp_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tL.AccountsGroup_IdNo = " & Str(Val(Grp_IdNo))
                    End If


                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTempSub ( int1, currency1 ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head tL, company_head tZ Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " tL.parent_Code like '%~18~' and a.voucher_date >= @companyfromdate and voucher_date < @fromdate and a.company_idno = tz.company_idno and a.ledger_idno = tL.ledger_idno group by a.ledger_idno having sum(a.voucher_amount) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into ReportTempSub ( int1, currency1 ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head tL, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " tL.parent_code not like '%~18~' and voucher_date < @fromdate and a.company_idno = tz.company_idno and a.ledger_idno = tL.ledger_idno group by a.ledger_idno having sum(a.voucher_amount) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into ReportTempSub ( int1, currency2 ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head tL, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.company_idno = tz.company_idno and a.voucher_amount > 0 and a.ledger_idno = tL.ledger_idno group by a.ledger_idno having sum(a.voucher_amount) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into ReportTempSub ( int1, currency3 ) select a.ledger_idno, sum(a.voucher_amount) from voucher_details a, ledger_head tL, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.company_idno = tz.company_idno and a.voucher_amount < 0 and a.ledger_idno = tL.ledger_idno group by a.ledger_idno having sum(a.voucher_amount) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into reporttemp ( Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, name1, currency1, currency2, currency3, currency4 ) select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "' +  '   -   ' + '" & Trim(RptHeading3) & "', '', b.ledger_name, -1*sum(a.currency1) as opening, -1*sum(currency3) as debit, sum(a.currency2) as credit, -1*sum(a.currency1+a.currency2+a.currency3) as balance from reporttempsub a, ledger_head b where a.int1 = b.ledger_idno group by b.ledger_name order by b.ledger_name"
                    cmd.ExecuteNonQuery()


                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, name1, currency1, currency2, currency3, currency4, name2, name3 from reporttemp Order by name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    Bal = 0
                    If Dtbl1.Rows.Count > 0 Then
                        For i = 0 To Dtbl1.Rows.Count - 1
                            Amt = Val(Dtbl1.Rows(i).Item("Currency1").ToString)
                            Dtbl1.Rows(i).Item("Name2") = Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Dr", " Cr")

                            Amt = Val(Dtbl1.Rows(i).Item("Currency4").ToString)
                            Dtbl1.Rows(i).Item("Name3") = Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Dr", " Cr")
                        Next i
                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_GroupLedger.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "opening tb", "general tb"

                    Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tZ.Company_IdNo = " & Str(Val(Comp_IdNo))
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "opening tb" Then

                        RptHeading2 = "AS ON : " & Trim(Common_Procedures.Company_FromDate)

                        cmd.CommandText = "Insert into reporttemp ( Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, currency1, currency2 ) Select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', b.ledger_Name, (case when sum(a.voucher_amount) < 0 then abs(sum(a.voucher_amount)) else 0 end), (case when sum(a.voucher_amount) > 0 then abs(sum(a.voucher_amount)) else 0 end) from voucher_details a, ledger_head b, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date < @companyfromdate and b.parent_code not like '%~18~' and a.ledger_idno = b.ledger_idno and a.company_idno = tz.company_idno group by b.ledger_name having sum(a.voucher_amount) <> 0"
                        cmd.ExecuteNonQuery()

                    Else
                        cmd.CommandText = "Insert into reporttemp ( Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, currency1, currency2 ) Select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', b.ledger_name, (case when sum(a.voucher_amount) < 0 then abs(sum(a.voucher_amount)) else 0 end), (case when sum(a.voucher_amount) > 0 then abs(sum(a.voucher_amount)) else 0 end) from voucher_details a, ledger_head b, company_head tZ where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @companyfromdate and @fromdate and b.parent_code like '%~18~' and a.ledger_idno = b.ledger_idno and a.company_idno = tz.company_idno group by b.ledger_name having sum(a.voucher_amount) <> 0"
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "Insert into reporttemp ( Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, currency1, currency2 ) Select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', b.ledger_name, (case when sum(a.voucher_amount) < 0 then abs(sum(a.voucher_amount)) else 0 end), (case when sum(a.voucher_amount) > 0 then abs(sum(a.voucher_amount)) else 0 end) from voucher_details a, ledger_head b, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @fromdate and a.year_for_report < " & Str(Year(Common_Procedures.Company_ToDate)) & " and b.parent_code not like '%~18~' and a.ledger_idno = b.ledger_idno and a.company_idno = tz.company_idno group by b.ledger_name having sum(a.voucher_amount) <> 0"
                        cmd.ExecuteNonQuery()

                    End If

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, currency1, currency2 from reporttemp Order by name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_GeneralTB.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "group tb", "final tb"

                    RptHeading2 = RptHeading2 & IIf(Trim(RptHeading3) <> "", "   -   ", "") & RptHeading3

                    Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

                    Grp_IdNo = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_GroupName.Text)


                    'If Grp_IdNo = 0 Then
                    '    MessageBox.Show("Invalid Group Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If cbo_GroupName.Enabled And cbo_GroupName.Visible Then cbo_GroupName.Focus()
                    '    Exit Sub
                    'End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tZ.Company_IdNo = " & Str(Val(Comp_IdNo))
                    End If
                    If cbo_GroupName.Visible = True And Val(Grp_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tG.AccountsGroup_IdNo = " & Str(Val(Grp_IdNo))
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "group tb" Then

                        cmd.CommandText = "Insert into reporttemp ( Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, name1, meters1, name2, currency1, currency2 ) Select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', tG.AccountsGroup_Name, tG.Order_Position, b.ledger_name, (case when sum(a.voucher_amount) < 0 then abs(sum(a.voucher_amount)) else 0 end), (case when sum(a.voucher_amount) > 0 then abs(sum(a.voucher_amount)) else 0 end) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @companyfromdate and @fromdate and a.year_for_report < " & Str(Year(Common_Procedures.Company_ToDate)) & " and b.parent_code like '%~18~' and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno = tZ.company_idno group by tG.AccountsGroup_Name, tG.Order_Position, b.ledger_name having sum(a.voucher_amount) <> 0"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into reporttemp ( Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, name1, meters1, name2, currency1, currency2 ) Select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', tG.AccountsGroup_Name, tG.Order_Position, b.ledger_name, (case when sum(a.voucher_amount) < 0 then abs(sum(a.voucher_amount)) else 0 end), (case when sum(a.voucher_amount) > 0 then abs(sum(a.voucher_amount)) else 0 end) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @fromdate and b.parent_code not like '%~18~' and a.year_for_report < " & Str(Year(Common_Procedures.Company_ToDate)) & " and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno = tz.company_idno group by tG.AccountsGroup_Name, tG.Order_Position, b.ledger_name having sum(a.voucher_amount) <> 0"
                        cmd.ExecuteNonQuery()

                    Else
                        cmd.CommandText = "Insert into reporttemp ( Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, name1, meters1, name2, currency1, currency2 ) Select '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', tG.AccountsGroup_Name, tG.Order_Position, b.ledger_name, (case when sum(a.voucher_amount) < 0 then abs(sum(a.voucher_amount)) else 0 end), (case when sum(a.voucher_amount) > 0 then abs(sum(a.voucher_amount)) else 0 end) from voucher_details a, ledger_head b, AccountsGroup_Head tG, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @fromdate and tG.parent_idno not like '%~18~' and a.ledger_idno = b.ledger_idno and b.AccountsGroup_IdNo = tG.AccountsGroup_IdNo and a.company_idno = tz.company_idno group by tG.AccountsGroup_Name, tG.Order_Position, b.ledger_name having sum(a.voucher_amount) <> 0"
                        cmd.ExecuteNonQuery()

                    End If

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, meters1, name2, currency1, currency2 from reporttemp Order by meters1, name1, name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_GroupTB.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "party balance - monthwise"

                    If dtp_FromDate.Visible = True Then
                        If IsDate(dtp_FromDate.Text) = False Then
                            MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                            Exit Sub
                        End If
                    End If

                    If dtp_ToDate.Visible = True Then
                        If IsDate(dtp_ToDate.Text) = False Then
                            MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                            Exit Sub
                        End If
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
                    If dtp_ToDate.Visible = True Then
                        cmd.Parameters.AddWithValue("@uptodate", dtp_ToDate.Value.Date)
                    Else
                        cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)
                    End If
                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table EntryTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into EntryTemp (int1, name1, currency1 ) Select b.ledger_idno, b.ledger_name, sum(a.voucher_amount) from voucher_details a, ledger_head b, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and b.AccountsGroup_IdNo = 10 and a.ledger_idno = b.ledger_idno and a.company_idno = tz.company_idno group by b.ledger_idno, b.ledger_name having sum(a.voucher_amount) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select int1 as Ledger_IdNo, name1 as Ledger_Name, abs(sum(currency1)) as BalanceAmount from EntryTemp group by int1, name1 having sum(currency1) < 0 Order by int1, name1", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)

                    Bal = 0
                    If Dt1.Rows.Count > 0 Then
                        For i = 0 To Dt1.Rows.Count - 1

                            If Math.Abs(Val(Dt1.Rows(i).Item("BalanceAmount").ToString)) <> 0 Then

                                cmd.Parameters.Clear()
                                cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                                cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
                                If dtp_ToDate.Visible = True Then
                                    cmd.Parameters.AddWithValue("@uptodate", dtp_ToDate.Value.Date)
                                Else
                                    cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)
                                End If

                                cmd.CommandText = "Select a.voucher_date, abs(sum(a.voucher_amount)) as VouAmt from voucher_details a INNER JOIN company_head tz ON a.company_idno = tz.company_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and a.ledger_idno = " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & " and a.voucher_amount < 0 group by a.voucher_date having sum(a.voucher_amount) <> 0 Order by a.voucher_date desc"
                                Da = New SqlClient.SqlDataAdapter(cmd)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)

                                Mnth_ID = 0
                                Yr = 0
                                Mnth_Yr_Nm = ""
                                Bal = Math.Abs(Val(Dt1.Rows(i).Item("BalanceAmount").ToString))

                                If Dt2.Rows.Count > 0 Then

                                    For j = 0 To Dt2.Rows.Count - 1

                                        n = DateDiff(DateInterval.Day, dtp_FromDate.Value.Date, CDate(Dt2.Rows(j).Item("voucher_date")))

                                        Mnth_Yr_Nm = ""
                                        Mnth_ID = 0
                                        Yr = 0

                                        If n < 0 Then
                                            Mnth_Yr_Nm = "Opening"

                                        Else
                                            Mnth_Yr_Nm = Format(Convert.ToDateTime(Dt2.Rows(j).Item("voucher_date").ToString), "MMM/yyyy").ToString
                                            Mnth_ID = Month(Dt2.Rows(j).Item("voucher_date").ToString)
                                            Yr = Year(Dt2.Rows(j).Item("voucher_date").ToString)

                                        End If

                                        Amt = 0
                                        If Bal >= Val(Dt2.Rows(j).Item("VouAmt").ToString) Then
                                            Amt = Val(Dt2.Rows(j).Item("VouAmt").ToString)
                                        Else
                                            Amt = Bal
                                        End If
                                        Bal = Bal - Amt

                                        cmd.Parameters.Clear()
                                        cmd.Parameters.AddWithValue("@VouDate", Convert.ToDateTime(Dt2.Rows(j).Item("voucher_date").ToString))

                                        cmd.CommandText = "Insert into ReportTempSub ( Date1, int1, int2, Int3, Name1, currency1 ) values ( @VouDate, " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", " & Str(Val(Mnth_ID)) & ", " & Str(Val(Yr)) & ", '" & Trim(Mnth_Yr_Nm) & "', " & Str(Val(Amt)) & " ) "
                                        cmd.ExecuteNonQuery()

                                        If Bal <= 0 Then
                                            Exit For
                                        End If

                                    Next j
                                End If
                                Dt2.Clear()

                                If Bal <> 0 Then
                                    Mnth_Yr_Nm = "Opening"
                                    Mnth_ID = 0
                                    Yr = 0
                                    Amt = Bal
                                    cmd.CommandText = "Insert into ReportTempSub ( int1, int2, Int3, Name1, currency1 ) values ( " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", " & Str(Val(Mnth_ID)) & ", " & Str(Val(Yr)) & ", '" & Trim(Mnth_Yr_Nm) & "', " & Str(Val(Amt)) & " ) "
                                    cmd.ExecuteNonQuery()
                                End If

                            End If

                        Next i

                    End If
                    Dt1.Clear()

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1      ,   Int2,   Int3,   Name2,     currency1   ) " &
                                            " Select        b.Ledger_IdNo, b.Ledger_Name, c.Idno, a.Int3, a.Name1,   sum(a.currency1)   from ReportTempSub a INNER JOIN Ledger_Head b ON a.Int1 = b.Ledger_IdNo LEFT OUTER JOIN Month_Head c ON a.Int2 = c.Month_IdNo group by b.Ledger_IdNo, b.Ledger_Name, c.Idno, a.Int3, a.Name1"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1, Name1, Int2, Name2, currency1  from reporttemp where currency1 <> 0 Order by Name1, Int2, Int3, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1, Int2, Name2, currency1 ) VALUES ( 0 , '' ,  0 , '' , 0 ) "
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1, Name1, Int2, Int3, Name2, currency1  from reporttemp Order by Name1, Int2, Int3, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Party_OutStanding_MonthWise.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "party balance - daywise"

                    If dtp_FromDate.Visible = True Then
                        If IsDate(dtp_FromDate.Text) = False Then
                            MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                            Exit Sub
                        End If
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table EntryTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into EntryTemp (int1, name1, currency1 ) Select b.ledger_idno, b.ledger_name, sum(a.voucher_amount) from voucher_details a, ledger_head b, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and b.AccountsGroup_IdNo = 10 and a.ledger_idno = b.ledger_idno and a.company_idno = tz.company_idno group by b.ledger_idno, b.ledger_name having sum(a.voucher_amount) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select int1 as Ledger_IdNo, name1 as Ledger_Name, abs(sum(currency1)) as BalanceAmount from EntryTemp group by int1, name1 having sum(currency1) < 0 Order by int1, name1", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)

                    Bal = 0
                    If Dt1.Rows.Count > 0 Then
                        For i = 0 To Dt1.Rows.Count - 1

                            If Math.Abs(Val(Dt1.Rows(i).Item("BalanceAmount").ToString)) <> 0 Then

                                cmd.Parameters.Clear()
                                cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                                cmd.CommandText = "Select a.voucher_date, abs(sum(a.voucher_amount)) as VouAmt from voucher_details a INNER JOIN company_head tz ON a.company_idno = tz.company_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and a.ledger_idno = " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & " and a.voucher_amount < 0 group by a.voucher_date having sum(a.voucher_amount) <> 0 Order by a.voucher_date desc"
                                Da = New SqlClient.SqlDataAdapter(cmd)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)

                                Bal = Math.Abs(Val(Dt1.Rows(i).Item("BalanceAmount").ToString))

                                If Dt2.Rows.Count > 0 Then

                                    For j = 0 To Dt2.Rows.Count - 1

                                        n = DateDiff(DateInterval.Day, CDate(Dt2.Rows(j).Item("voucher_date")), dtp_FromDate.Value.Date)

                                        Amt = 0
                                        If Bal >= Val(Dt2.Rows(j).Item("VouAmt").ToString) Then
                                            Amt = Val(Dt2.Rows(j).Item("VouAmt").ToString)
                                        Else
                                            Amt = Bal
                                        End If
                                        Bal = Bal - Amt

                                        cmd.CommandText = "Insert into ReportTempSub ( int1, int2, currency1 ) values ( " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", " & Str(Val(n)) & ", " & Str(Val(Amt)) & " ) "
                                        cmd.ExecuteNonQuery()

                                        If Bal <= 0 Then
                                            Exit For
                                        End If

                                    Next j
                                End If
                                Dt2.Clear()

                                If Bal <> 0 Then
                                    n = 99999
                                    Amt = Bal
                                    cmd.CommandText = "Insert into ReportTempSub ( int1, int2, currency1 ) values ( " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", " & Str(Val(n)) & ", " & Str(Val(Amt)) & " ) "
                                    cmd.ExecuteNonQuery()
                                End If

                            End If

                        Next i

                    End If
                    Dt1.Clear()

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    If Trim(txt_Inputs1.Text) = "" Then txt_Inputs1.Text = "30,60,90"


                    oldvl = "0"
                    b = Split(txt_Inputs1.Text, ",")

                    For i = 0 To UBound(b)
                        If Val(b(i)) <> 0 Then
                            S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

                            cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1      ,             Int2       ,         Name2     , currency1    ) " &
                                                " Select        b.Ledger_IdNo, b.Ledger_Name, " & Str(Val(i) + 1) & ", '" & Trim(S) & "',      0    from ReportTempSub a INNER JOIN Ledger_Head b ON a.Int1 = b.Ledger_IdNo group by b.Ledger_IdNo, b.Ledger_Name having sum(a.currency1) <> 0"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1      ,             Int2       ,         Name2     ,        currency1    ) " &
                                                " Select        b.Ledger_IdNo, b.Ledger_Name, " & Str(Val(i) + 1) & ", '" & Trim(S) & "',   sum(a.currency1)   from ReportTempSub a INNER JOIN Ledger_Head b ON a.Int1 = b.Ledger_IdNo where Int2 Between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " group by b.Ledger_IdNo, b.Ledger_Name"
                            cmd.ExecuteNonQuery()

                            oldvl = Val(b(i)) + 1

                        End If
                    Next i

                    If Val(oldvl) <> 0 Then

                        S = "ABV " & Trim(Val(oldvl) - 1)

                        cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1      ,             Int2       ,         Name2     , currency1    ) " &
                                            " Select        b.Ledger_IdNo, b.Ledger_Name, " & Str(Val(i) + 1) & ", '" & Trim(S) & "',      0    from ReportTempSub a INNER JOIN Ledger_Head b ON a.Int1 = b.Ledger_IdNo group by b.Ledger_IdNo, b.Ledger_Name having sum(a.currency1) <> 0"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1      ,             Int2       ,         Name2     ,        currency1    ) " &
                                            " Select        b.Ledger_IdNo, b.Ledger_Name, " & Str(Val(i) + 1) & ", '" & Trim(S) & "',   sum(a.currency1)   from ReportTempSub a INNER JOIN Ledger_Head b ON a.Int1 = b.Ledger_IdNo where Int2 >= " & Str(Val(oldvl)) & " group by b.Ledger_IdNo, b.Ledger_Name"
                        cmd.ExecuteNonQuery()

                    End If

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1, Name1, Int2, Name2, currency1  from reporttemp Order by Name1, Int2, Int3, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1, Int2, Name2, currency1 ) VALUES ( 0 , '' ,  0 , '' , 0 ) "
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1, Name1, Int2, Int3, Name2, currency1  from reporttemp Order by Name1, Int2, Int3, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Party_OutStanding_DayWise.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "party balance - billwise", "party balance - billwise simple"

                    If dtp_FromDate.Visible = True Then
                        If IsDate(dtp_FromDate.Text) = False Then
                            MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                            Exit Sub
                        End If
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table EntryTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into EntryTemp (int1, name1, currency1 ) Select b.ledger_idno, b.ledger_name, sum(a.voucher_amount) from voucher_details a, ledger_head b, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and b.AccountsGroup_IdNo = 10 and a.ledger_idno = b.ledger_idno and a.company_idno = tz.company_idno group by b.ledger_idno, b.ledger_name having sum(a.voucher_amount) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select int1 as Ledger_IdNo, name1 as Ledger_Name, abs(sum(currency1)) as BalanceAmount from EntryTemp group by int1, name1 having sum(currency1) < 0 Order by int1, name1", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)

                    Bal = 0
                    If Dt1.Rows.Count > 0 Then
                        For i = 0 To Dt1.Rows.Count - 1

                            If Math.Abs(Val(Dt1.Rows(i).Item("BalanceAmount").ToString)) <> 0 Then

                                cmd.Parameters.Clear()
                                cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                                cmd.CommandText = "Truncate table EntryTempSub"
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "Insert into EntryTempSub (name1, meters1, date1, currency1 ) Select 'Opening', 0, a.voucher_date, abs(sum(a.voucher_amount)) as VouAmt from voucher_details a INNER JOIN company_head tz ON a.company_idno = tz.company_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and a.ledger_idno = " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & " and a.voucher_amount < 0 and a.Entry_Identification LIKE 'OPENI-%' group by a.voucher_date having sum(a.voucher_amount) <> 0"
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "Insert into EntryTempSub (name1, meters1, date1, currency1 ) Select c.sales_no, c.for_orderby, a.voucher_date, abs(sum(a.voucher_amount)) as VouAmt from voucher_details a INNER JOIN company_head tz ON a.company_idno = tz.company_idno LEFT OUTER JOIN sales_head c ON a.Entry_Identification = 'SALES-' + c.sales_code Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and a.ledger_idno = " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & " and a.voucher_amount < 0 and a.Entry_Identification NOT LIKE 'OPENI-%' and a.Entry_Identification NOT LIKE 'GSALE-%' group by c.sales_no, c.for_orderby, a.voucher_date having sum(a.voucher_amount) <> 0"
                                cmd.ExecuteNonQuery()

                                cmd.CommandText = "Insert into EntryTempSub (name1, meters1, date1, currency1 ) Select c.sales_no, c.for_orderby, a.voucher_date, abs(sum(a.voucher_amount)) as VouAmt from voucher_details a INNER JOIN company_head tz ON a.company_idno = tz.company_idno LEFT OUTER JOIN sales_head c ON a.Entry_Identification = 'GSALE-' + c.sales_code Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and a.ledger_idno = " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & " and a.voucher_amount < 0 and a.Entry_Identification NOT LIKE 'OPENI-%'  and a.Entry_Identification NOT LIKE 'SALES-%' group by c.sales_no, c.for_orderby, a.voucher_date having sum(a.voucher_amount) <> 0"
                                Nr = cmd.ExecuteNonQuery()

                                cmd.CommandText = "Select a.name1 as VouNo, a.meters1 as VouOrderBy, a.date1 as VouDate, a.currency1 as VouAmt from EntryTempSub a where a.currency1 <> 0 Order by a.date1 desc, a.meters1 desc, a.name1 desc"
                                'cmd.CommandText = "Select a.voucher_date, abs(sum(a.voucher_amount)) as VouAmt from voucher_details a INNER JOIN company_head tz ON a.company_idno = tz.company_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and a.ledger_idno = " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & " and a.voucher_amount < 0 group by a.voucher_date having sum(a.voucher_amount) <> 0 Order by a.voucher_date desc"
                                Da = New SqlClient.SqlDataAdapter(cmd)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)

                                Bal = Math.Abs(Val(Dt1.Rows(i).Item("BalanceAmount").ToString))

                                If Dt2.Rows.Count > 0 Then

                                    For j = 0 To Dt2.Rows.Count - 1

                                        n = DateDiff(DateInterval.Day, CDate(Dt2.Rows(j).Item("VouDate")), dtp_FromDate.Value.Date)

                                        BlAmt = Val(Dt2.Rows(j).Item("VouAmt").ToString)
                                        RecAmt = 0
                                        Amt = 0
                                        If Bal >= Val(Dt2.Rows(j).Item("VouAmt").ToString) Then
                                            Amt = Val(Dt2.Rows(j).Item("VouAmt").ToString)
                                        Else
                                            Amt = Bal
                                        End If
                                        Bal = Bal - Amt

                                        RecAmt = BlAmt - Amt

                                        cmd.Parameters.Clear()
                                        cmd.Parameters.AddWithValue("@VouDate", Convert.ToDateTime(Dt2.Rows(j).Item("VouDate").ToString))

                                        cmd.CommandText = "Insert into ReportTempSub (                    Name1                 ,                              Meters1                     ,  Date1  ,                               int1                        ,         currency1      ,            currency2    ,          currency3   ,            int2     ) " &
                                                                "    Values ( '" & Trim(Dt2.Rows(j).Item("VouNo").ToString) & "', " & Str(Val(Dt2.Rows(j).Item("VouOrderBy").ToString)) & ", @VouDate, " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", " & Str(Val(BlAmt)) & ", " & Str(Val(RecAmt)) & ", " & Str(Val(Amt)) & ", " & Str(Val(n)) & " ) "
                                        cmd.ExecuteNonQuery()

                                        If Bal <= 0 Then
                                            Exit For
                                        End If

                                    Next j
                                End If
                                Dt2.Clear()

                                If Bal <> 0 Then
                                    n = 99999
                                    Amt = Bal
                                    cmd.CommandText = "Insert into ReportTempSub (  Name1   , Meters1,                                int1                       ,         currency1    , currency2,       currency3      ,            int2     ) " &
                                                            "          Values    ( 'Opening',     0  , " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", " & Str(Val(Amt)) & ",      0   , " & Str(Val(Amt)) & ", " & Str(Val(n)) & " ) "
                                End If

                            End If

                        Next i

                    End If
                    Dt1.Clear()

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp (  Name1,   Meters1,   Date1,   Int1       ,   Name2      ,   currency1,   currency2,   currency3,   Int2  ) " &
                                            "    Select        a.Name1, a.Meters1, a.Date1, b.Ledger_IdNo, b.Ledger_Name, a.currency1, a.currency2, a.currency3, a.Int2  from ReportTempSub a INNER JOIN Ledger_Head b ON a.Int1 = b.Ledger_IdNo"
                    cmd.ExecuteNonQuery()


                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Meters1, Date1, Int1, Name2, currency1, currency2, currency3, Int2  from reporttemp Order by Int2 desc, Date1, Meters1, Name1, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1, Int2, Name2, currency1 ) VALUES ( 0 , '' ,  0 , '' , 0 ) "
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Meters1, Date1, Int1, Name2, currency1, currency2, currency3, Int2  from reporttemp Order by Int2 desc, Date1, Meters1, Name1, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1
                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "party balance - billwise simple" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Party_OutStanding_BillWise_Simple.rdlc"
                    Else
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Party_OutStanding_BillWise.rdlc"
                    End If




                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "party balance - monthwise-----", "party balance - daywise----"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table EntryTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into EntryTemp ( int1, name1, currency1 ) Select b.ledger_idno, b.ledger_name, sum(a.voucher_amount) from voucher_details a, ledger_head b, company_head tz where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and b.AccountsGroup_IdNo = 10 and a.ledger_idno = b.ledger_idno and a.company_idno = tz.company_idno group by b.ledger_idno, b.ledger_name having sum(a.voucher_amount) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()

                    'Debug.Print(Now)

                    Da = New SqlClient.SqlDataAdapter("Select int1 as Ledger_IdNo, name1 as Ledger_Name, abs(sum(currency1)) as BalanceAmount from EntryTemp group by int1, name1 having sum(currency1) < 0 Order by int1, name1", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)

                    Bal = 0
                    If Dt1.Rows.Count > 0 Then
                        For i = 0 To Dt1.Rows.Count - 1

                            cmd.CommandText = "Select month(a.voucher_date) as Month_IdNo, abs(sum(a.voucher_amount)) as VouAmt from voucher_details a INNER JOIN company_head tz ON a.company_idno = tz.company_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date <= @uptodate and a.ledger_idno = " & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & " and a.voucher_amount < 0 group by month(a.voucher_date) having sum(a.voucher_amount) <> 0 Order by month(a.voucher_date) desc"
                            Da = New SqlClient.SqlDataAdapter(cmd)
                            Dt2 = New DataTable
                            Da.Fill(Dt2)

                            Mnth_ID = 0
                            Bal = Math.Abs(Val(Dt1.Rows(i).Item("BalanceAmount").ToString))

                            If Dt2.Rows.Count > 0 Then
                                For j = 0 To Dt2.Rows.Count - 1

                                    Mnth_ID = Val(Dt2.Rows(j).Item("Month_IdNo").ToString)
                                    Amt = 0
                                    If Bal >= Val(Dt2.Rows(j).Item("VouAmt").ToString) Then
                                        Amt = Val(Dt2.Rows(j).Item("VouAmt").ToString)
                                    Else
                                        Amt = Bal
                                    End If
                                    Bal = Bal - Amt

                                    cmd.CommandText = "Insert into ReportTempSub (int1, int2, currency1 ) values (" & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", " & Str(Val(Mnth_ID)) & ", " & Str(Val(Amt)) & ") "
                                    cmd.ExecuteNonQuery()

                                    If Bal <= 0 Then
                                        Exit For
                                    End If

                                Next j
                            End If
                            Dt2.Clear()

                            'If Bal <> 0 Then
                            '    Amt = Bal
                            '    cmd.CommandText = "Insert into ReportTempSub (int1, int2, currency1 ) values (" & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ", " & Str(Val(Mnth_ID)) & ", " & Str(Val(Amt)) & ") "
                            '    cmd.ExecuteNonQuery()
                            'End If

                        Next i
                    End If
                    Dt1.Clear()
                    'Debug.Print(Now)

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1      ,   Int2      ,   Name2          ,     currency1   ) " &
                                        " Select        b.Ledger_IdNo, b.Ledger_Name, c.Month_IdNo, c.Month_ShortName,   sum(a.currency1)   from ReportTempSub a INNER JOIN Ledger_Head b ON a.Int1 = b.Ledger_IdNo INNER JOIN Month_Head c ON a.Int2 = c.Month_IdNo group by b.Ledger_IdNo, b.Ledger_Name, c.Month_IdNo, c.Month_ShortName"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1, Name1, Int2, Name2, currency1  from reporttemp where currency1 <> 0 Order by Name1, Int2, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into ReportTemp ( Int1 ,   Name1,   Int2,   Name2          , currency1 ) " &
                                            " Select                  0,       '' , a.Month_IdNo, a.Month_ShortName,      0   from Month_Head a where a.month_idno = " & Str(Val(Month(Date.Today)))
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1, Name1, Int2, Name2, currency1  from reporttemp Order by Name1, Int2, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Party_Balance_MonthWise.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "month ledger a/c"

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "" And Trim(RptHeading3) <> "", vbCrLf, "") & RptHeading3
                    RptHeading3 = ""

                    Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                    If Led_IdNo = 0 Then
                        MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                        Exit Sub
                    End If

                    Mth_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_ItemName.Text)
                    If Mth_IdNo = 0 Then
                        Mth_IdNo = 3
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
                        RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))
                    End If
                    If cbo_Ledger.Visible = True And Val(Led_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
                    End If

                    Amt = 0

                    GrpCd = Common_Procedures.get_FieldValue(con, "ledger_head", "parent_code", "ledger_idno = " & Str(Val(Led_IdNo)))

                    Ttc = 0 : Ttd = 0 : Fnt = 0 : Tot_CR = 0 : Tot_DB = 0
                    Opds = "0.00 Cr"

                    If Not (GrpCd Like "*~18~") Then

                        cmd.CommandText = "Select sum(a.voucher_amount) as OPAmount from voucher_details a INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " voucher_date < @companyfromdate"
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)
                        If Dt.Rows.Count > 0 Then
                            Opds = Common_Procedures.Currency_Format(Math.Abs(Val(Dt.Rows(0).Item("OPAmount").ToString))) & IIf(Val(Dt.Rows(0).Item("OPAmount").ToString) >= 0, " Cr", " Dr")
                            Fnt = Val(Dt.Rows(0).Item("OPAmount").ToString)
                        End If
                        Dt.Clear()

                    End If

                    cmd.CommandText = "truncate table reporttemp"
                    cmd.ExecuteNonQuery()

                    Opn = IIf(Val(Opds) < 0, Common_Procedures.Currency_Format(Math.Abs(Val(Opds))) + " Dr", Common_Procedures.Currency_Format(Val(Opds)) + " Cr")

                    a1 = IIf(Val(Mth_IdNo) < 4, 12, Val(Mth_IdNo))

                    For m1 As Integer = 4 To a1

                        Ttc = 0
                        cmd.CommandText = "select sum(a.voucher_amount) as Cr_Amount from voucher_details a INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.voucher_date) = " & Str(Val(m1)) & " and year(voucher_date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and voucher_amount > 0"
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)
                        If Dt.Rows.Count > 0 Then
                            Ttc = Val(Dt.Rows(0).Item("Cr_Amount").ToString)
                        End If
                        Dt.Clear()

                        Ttd = 0
                        cmd.CommandText = "select sum(a.voucher_amount) as Dr_Amount from voucher_details a INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.voucher_date) = " & Str(Val(m1)) & " and year(voucher_date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and voucher_amount < 0"
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)
                        If Dt.Rows.Count > 0 Then
                            Ttd = Math.Abs(Val(Dt.Rows(0).Item("Dr_Amount").ToString))
                        End If
                        Dt.Clear()

                        Fnt = Fnt + Ttc - Ttd
                        Tot_CR = Tot_CR + Ttc
                        Tot_DB = Tot_DB + Ttd
                        Clds = IIf(Fnt < 0, Common_Procedures.Currency_Format(Math.Abs(Fnt)) + " Dr", Common_Procedures.Currency_Format(Fnt) + " Cr")

                        cmd.CommandText = "Insert into reporttemp ( Int1, name1, Currency1, Name2, Currency2, Currency3, Currency4, Name3 ) values ( " & Str(Val(m1)) & ", '" & Trim(UCase(MonthName(m1))) & "', " & Str(Val(Opds)) & ", '" & Trim(Opds) & "', " & Str(Val(Ttc)) & ", " & Str(Val(Ttd)) & ", " & Str(Val(Clds)) & ", '" & Trim(Clds) & "')"
                        cmd.ExecuteNonQuery()

                        Opds = Clds

                    Next m1


                    If Val(Mth_IdNo) <= 3 Then

                        For m1 As Integer = 1 To Val(Mth_IdNo)

                            Ttc = 0
                            cmd.CommandText = "select sum(a.voucher_amount) as Cr_Amount from voucher_details a INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.voucher_date) = " & Str(Val(m1)) & " and year(voucher_date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and voucher_amount > 0"
                            Da = New SqlClient.SqlDataAdapter(cmd)
                            Dt = New DataTable
                            Da.Fill(Dt)
                            If Dt.Rows.Count > 0 Then
                                Ttc = Val(Dt.Rows(0).Item("Cr_Amount").ToString)
                            End If
                            Dt.Clear()

                            Ttd = 0
                            cmd.CommandText = "select sum(a.voucher_amount) as Dr_Amount from voucher_details a INNER JOIN Company_Head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.voucher_date) = " & Str(Val(m1)) & " and year(voucher_date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " and voucher_amount < 0"
                            Da = New SqlClient.SqlDataAdapter(cmd)
                            Dt = New DataTable
                            Da.Fill(Dt)
                            If Dt.Rows.Count > 0 Then
                                Ttd = Math.Abs(Val(Dt.Rows(0).Item("Dr_Amount").ToString))
                            End If
                            Dt.Clear()

                            Fnt = Fnt + Ttc - Ttd
                            Tot_CR = Tot_CR + Ttc
                            Tot_DB = Tot_DB + Ttd
                            Clds = IIf(Fnt < 0, Common_Procedures.Currency_Format(Math.Abs(Fnt)) + " Dr", Common_Procedures.Currency_Format(Fnt) + " Cr")

                            Mid = Val(Common_Procedures.get_FieldValue(con, "Month_Head", "Idno", "(Month_IdNo = " & Str(Val(m1)) & ")"))

                            cmd.CommandText = "Insert into reporttemp ( Int1, Int2, name1, Currency1, Name2, Currency2, Currency3, Currency4, Name3 ) Values ( " & Str(Val(m1)) & ", " & Str(Val(Mid)) & ", '" & Trim(UCase(MonthName(m1))) & "', " & Str(Val(Opds)) & ", '" & Trim(Opds) & "', " & Str(Val(Ttc)) & ", " & Str(Val(Ttd)) & ", " & Str(Val(Clds)) & ", '" & Trim(Clds) & "')"
                            cmd.ExecuteNonQuery()

                            Opds = Clds

                        Next m1

                    End If

                    cmd.CommandText = "update reporttemp set Name4 = '" & Trim(Opn) & "',  Name5 = '" & Trim(Clds) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1, Int2, name1, Currency1, Name2, Currency2, Currency3, Currency4, Name3, Name4, Name5 from reporttemp Order by Int2, Int1, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1, Int2, name1, Currency1, Name2, Currency2, Currency3, Currency4, Name3, Name4, Name5  from reporttemp Order by Int2, Int1, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_MonthLedger.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                    RptViewer.Focus()
                    SendKeys.Send("{TAB}")

                Case "day book"
                    DayBook_Report()

                Case "single ledger - grid - datewise"

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "" And Trim(RptHeading3) <> "", vbCrLf, "") & RptHeading3
                    RptHeading3 = ""

                    If IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Visible = True And dtp_FromDate.Enabled = True Then dtp_FromDate.Focus()
                        Exit Sub
                    End If

                    If IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Visible = True And dtp_ToDate.Enabled = True Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                    If Led_IdNo = 0 Then
                        MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
                    cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

                    RptCondt = CompCondt
                    IpColNm1 = ""
                    If cbo_Company.Visible = True Then
                        If Val(Comp_IdNo) <> 0 Then
                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Comp_IdNo))
                            IpColNm1 = "[HIDDEN]"
                        End If
                    Else
                        IpColNm1 = "[HIDDEN]"
                    End If
                    If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
                        RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))
                    End If
                    If cbo_Ledger.Visible = True And Val(Led_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
                    End If

                    Amt = 0
                    cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date < @fromdate and a.ledger_idno = b.ledger_idno and b.parent_code NOT LIKE '%~18~' and a.company_idno = tZ.company_idno"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            Amt = Val(Dt.Rows(0)(0).ToString)
                        End If
                    End If
                    Dt.Clear()



                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()


                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
                        RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))
                    End If
                    If cbo_Ledger.Visible = True And Val(Led_IdNo) <> 0 Then
                        If Trim(LCase(RptIpDet_ReportName)) = "single ledger a/c - ledgerwise" Then
                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo <> " & Str(Val(Led_IdNo))
                        Else
                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
                        End If
                    End If

                    If Trim(LCase(RptIpDet_ReportName)) = "single ledger a/c - ledgerwise" Then

                        cmd.CommandText = "Insert into ReportTemp(Meters5, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Name7, Name8, Name9) select 0, 1, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'To ' + c.ledger_name, Abs(a.voucher_amount), 0, a.narration, a.Voucher_Type, a.Entry_Identification, tZ.Company_ShortName, c.Parent_Code from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN voucher_head b ON a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno LEFT OUTER JOIN ledger_head c ON a.ledger_idno = c.ledger_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Voucher_Code IN ( select z1.Voucher_Code from voucher_details z1 where z1.ledger_idno = " & Str(Val(Led_IdNo)) & " ) and a.voucher_date between @fromdate and @todate and a.voucher_amount > 0"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into ReportTemp(Meters5, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Name7, Name8, Name9) select 0, 2, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'By ' + c.ledger_name, 0, abs(a.Voucher_Amount), a.narration, a.Voucher_Type, a.Entry_Identification, tZ.Company_ShortName, c.Parent_Code from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN voucher_head b ON  a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno LEFT OUTER JOIN ledger_head c ON a.ledger_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Voucher_Code IN ( select z1.Voucher_Code from voucher_details z1 where z1.ledger_idno = " & Str(Val(Led_IdNo)) & " ) and a.voucher_date between @fromdate and @todate and a.voucher_amount < 0"
                        cmd.ExecuteNonQuery()

                    Else

                        cmd.CommandText = "Insert into ReportTemp(Meters5, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Name7, Name8, Name9) select 0, 1, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'To ' + c.ledger_name, Abs(a.voucher_amount), 0, a.narration, a.Voucher_Type, a.Entry_Identification, tZ.Company_ShortName, c.Parent_Code from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN voucher_head b ON a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno LEFT OUTER JOIN ledger_head c ON b.creditor_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.voucher_amount < 0"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into ReportTemp(Meters5, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Name7, Name8, Name9) select 0, 2, a.Voucher_Date, b.For_OrderBy, b.Voucher_Code, b.Voucher_No, 'By ' + c.ledger_name, 0, a.Voucher_Amount, a.narration, a.Voucher_Type, a.Entry_Identification, tZ.Company_ShortName, c.Parent_Code from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN voucher_head b ON a.Voucher_Code = b.Voucher_Code and a.Company_Idno = b.Company_Idno LEFT OUTER JOIN ledger_head c ON b.debtor_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and a.voucher_amount > 0"
                        cmd.ExecuteNonQuery()

                    End If


                    'cmd.CommandText = "Update ReportTemp SET Meters5 = b.LedgerOrder_Position from ReportTemp a, AccountsGroup_Head b Where a.Name9 = b.Parent_Idno"
                    'cmd.ExecuteNonQuery()

                    Tot_CR = 0 : Tot_DB = 0
                    If RptIpDet_IsGridReport = True Then

                        Da = New SqlClient.SqlDataAdapter("select Date1 as VouDate, Name5 as VouType, Name8 as Company_ShortName, Name2 as VouNo, Name3 as Particulars, Currency1 as Debit, Currency2 as Credit, Name6 as Balance, Name4 as Narration, Name7 as VoucherCode from reporttemp Order by Date1, Meters5, Int5, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                        Bal = 0
                        If Dtbl1.Rows.Count > 0 Then
                            Tot_DB = Tot_DB + Val(Dtbl1.Rows(0).Item("Debit").ToString)
                            Tot_CR = Tot_CR + Val(Dtbl1.Rows(0).Item("Credit").ToString)
                            Bal = Val(Dtbl1.Rows(0).Item("Debit").ToString) - Val(Dtbl1.Rows(0).Item("Credit").ToString)
                            Dtbl1.Rows(0).Item("Balance") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                            For i = 1 To Dtbl1.Rows.Count - 1
                                Tot_DB = Tot_DB + Val(Dtbl1.Rows(i).Item("Debit").ToString)
                                Tot_CR = Tot_CR + Val(Dtbl1.Rows(i).Item("Credit").ToString)
                                Bal = Val(Bal) + Val(Dtbl1.Rows(i).Item("Debit").ToString) - Val(Dtbl1.Rows(i).Item("Credit").ToString)
                                Dtbl1.Rows(i).Item("Balance") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                            Next i

                        End If

                        If Dtbl1.Rows.Count = 0 Then

                            cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                            cmd.ExecuteNonQuery()

                            Da = New SqlClient.SqlDataAdapter("select Date1 as VouDate, Name5 as VouType, Name2 as VouNo, Name3 as Particulars, Currency1 as Debit, Currency2 as Credit, Name6 as Balance, Name4 as Narration from reporttemp Order by Date1, Int5, meters1, name2, name1", con)
                            Dtbl1 = New DataTable
                            Da.Fill(Dtbl1)

                        End If

                        Dim MyNewRow As DataRow
                        MyNewRow = Dtbl1.NewRow
                        With MyNewRow
                            '.Item(4) = "TOTAL"
                            .Item(5) = Format(Tot_DB, "0000000.00")
                            .Item(6) = Format(Tot_CR, "0000000.00")
                            .Item(7) = Common_Procedures.Currency_Format(Math.Abs(Bal)) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                            .Item(8) = ""
                            .Item(9) = ""
                        End With
                        Dtbl1.Rows.Add(MyNewRow)
                        Dtbl1.AcceptChanges()

                    Else

                        Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5, Meters6, Name7 from reporttemp Order by Date1, Meters5, Int5, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                        Bal = 0
                        If Dtbl1.Rows.Count > 0 Then
                            Bal = Val(Dtbl1.Rows(0).Item("Currency1").ToString) - Val(Dtbl1.Rows(0).Item("Currency2").ToString)
                            Dtbl1.Rows(0).Item("Name6") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                            For i = 1 To Dtbl1.Rows.Count - 1
                                Bal = Val(Bal) + Val(Dtbl1.Rows(i).Item("Currency1").ToString) - Val(Dtbl1.Rows(i).Item("Currency2").ToString)
                                Dtbl1.Rows(i).Item("Name6") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
                            Next i
                        End If

                        If Dtbl1.Rows.Count = 0 Then

                            cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                            cmd.ExecuteNonQuery()

                            Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5, Meters6 from reporttemp Order by Int5, Date1, meters1, name2, name1", con)
                            Dtbl1 = New DataTable
                            Da.Fill(Dtbl1)

                        End If

                    End If


                'If RptIpDet_IsGridReport = True Then

                '    With dgv_Report
                '        .SuspendLayout()
                '        Application.DoEvents()

                '        .BackgroundColor = Color.White
                '        .BorderStyle = BorderStyle.FixedSingle

                '        .AllowUserToAddRows = False
                '        .AllowUserToDeleteRows = False
                '        .AllowUserToOrderColumns = False
                '        .ReadOnly = True
                '        .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                '        .MultiSelect = False
                '        .AllowUserToResizeColumns = False
                '        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                '        .AllowUserToResizeRows = False

                '        .DefaultCellStyle.SelectionBackColor = Color.Lime
                '        .DefaultCellStyle.SelectionForeColor = Color.Blue

                '        .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                '        .Columns.Clear()
                '        .DataSource = Dtbl1
                '        .RowHeadersVisible = False
                '        .AllowUserToOrderColumns = False

                '        .Columns(0).HeaderText = "DATE"
                '        .Columns(1).HeaderText = "VOU.TYPE"
                '        .Columns(2).HeaderText = "COMPANY"
                '        .Columns(3).HeaderText = "VOU.NO"
                '        .Columns(4).HeaderText = "PARTICULARS"
                '        .Columns(5).HeaderText = "DEBIT"
                '        .Columns(6).HeaderText = "CREDIT"
                '        .Columns(7).HeaderText = "BALANCE"
                '        .Columns(8).HeaderText = "NARRATION"
                '        .Columns(9).HeaderText = "voucher_code [HIDDEN]"

                '        .Columns(2).Visible = True
                '        If Trim(IpColNm1) <> "" Then
                '            .Columns(2).Visible = False
                '        End If
                '        .Columns(9).Visible = False

                '        .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                '        .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

                '        .RowsDefaultCellStyle.BackColor = Color.White
                '        .AlternatingRowsDefaultCellStyle.BackColor = Color.Beige

                '        '.RowsDefaultCellStyle.BackColor = Color.Bisque
                '        '.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige

                '        '.RowsDefaultCellStyle.BackColor = Color.LightGray
                '        '.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray

                '        .Columns(0).FillWeight = 65
                '        .Columns(1).FillWeight = 55
                '        .Columns(2).FillWeight = 50
                '        .Columns(3).FillWeight = 45
                '        .Columns(4).FillWeight = 180
                '        .Columns(5).FillWeight = 75
                '        .Columns(6).FillWeight = 75
                '        .Columns(7).FillWeight = 85
                '        .Columns(8).FillWeight = 175
                '        .Columns(9).FillWeight = 100

                '        .Columns(5).DefaultCellStyle.Alignment = 4
                '        .Columns(6).DefaultCellStyle.Alignment = 4
                '        .Columns(7).DefaultCellStyle.Alignment = 4

                '        .Columns(0).ReadOnly = True
                '        .Columns(1).ReadOnly = True
                '        .Columns(2).ReadOnly = True
                '        .Columns(3).ReadOnly = True
                '        .Columns(4).ReadOnly = True
                '        .Columns(5).ReadOnly = True
                '        .Columns(6).ReadOnly = True
                '        .Columns(7).ReadOnly = True
                '        .Columns(8).ReadOnly = True
                '        .Columns(9).ReadOnly = True

                '        .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable
                '        .Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable

                '        n = .Rows.Count - 1
                '        .Rows(n).Height = 40
                '        For j = 0 To .ColumnCount - 1
                '            .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                '            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                '        Next

                '        .Visible = True
                '        .ResumeLayout()

                '        .BringToFront()
                '        .Focus()

                '        If .Rows.Count > 0 Then
                '            .CurrentCell = .Rows(0).Cells(0)
                '            .CurrentCell.Selected = True
                '        End If

                '    End With


                'Else

                '    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                '    RpDs1.Name = "DataSet1"
                '    RpDs1.Value = Dtbl1


                '        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SingleLedger.rdlc"


                '    RptViewer.LocalReport.DataSources.Clear()

                '    RptViewer.LocalReport.DataSources.Add(RpDs1)

                '    RptViewer.LocalReport.Refresh()
                '    RptViewer.RefreshReport()

                '    RptViewer.Visible = True
                '    RptViewer.Focus()
                '    SendKeys.Send("{TAB}")

                'End If

                Case "customer bill pending - single", "customer bill pending - all", "customer bill pending - purchased", "customer bill pending - invoiced", "customer bill pending - single - with postdated amount", "customer bill pending - invoiced - notification"

                    If IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Visible = True And dtp_FromDate.Enabled = True Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    Comp_IdNo = 0
                    If cbo_Company.Visible Then Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

                    Led_IdNo = 0
                    If cbo_Ledger.Visible Then
                        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                        If Led_IdNo = 0 Then
                            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                            Exit Sub
                        End If
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "" And Trim(RptHeading3) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
                        RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))
                    End If
                    If cbo_Ledger.Visible = True And Val(Led_IdNo) <> 0 Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Led_IdNo))
                    End If

                    cmd.CommandText = "truncate table reporttempsub"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into reporttempsub ( int1, int2, name1, currency1 ) Select tZ.company_idno, tP.ledger_idno, a.voucher_bill_code, 0 from voucher_bill_head a INNER JOIN company_head tz  ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate group by tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_Code"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into reporttempsub ( int1, int2, name1, currency1 ) Select tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_Code, sum(a.Amount) from voucher_bill_details a INNER JOIN company_head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate group by tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_Code"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into reporttempsub ( int1, int2, name1, currency2 ) Select tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_Code, sum(a.Amount) from voucher_bill_details a INNER JOIN company_head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date > @uptodate group by tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_Code"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "truncate table EntryTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into EntryTemp ( int1, int2, name1, currency1, currency2 ) Select int1, int2, name1, sum(currency1), sum(currency2) from reporttempsub group by int1, int2, name1"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    IpColNm1 = "tZ.Company_ShortName"
                    If Comp_IdNo <> 0 Or ShowCompCol_STS = False Then
                        IpColNm1 = "'[HIDDEN]'"
                    End If

                    If Trim(LCase(RptIpDet_ReportName)) = "customer bill pending - purchased" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.crdr_type = 'CR' and tP.ledger_type = '' "
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "customer bill pending - invoiced" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.crdr_type = 'DR' and tP.ledger_type = '' "
                    End If

                    If Trim(LCase(RptIpDet_ReportName)) = "customer bill pending - invoiced - notification" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " datediff(day, a.voucher_bill_date, @uptodate) > 30 "
                    End If

                    cmd.CommandText = "insert into ReportTemp ( name1, name2, name3, Date1, currency1, currency2, currency3, name4, currency4, currency5, currency6, currency7, int6, int7, Name10 ) Select " & IpColNm1 & ", tP.Ledger_Name, a.party_bill_no, a.voucher_bill_date, (case when lower(a.crdr_type) = 'cr' then a.bill_amount else (case when b.currency1 is null then 0 else b.currency1 end ) end) as cr_amount, (case when lower(a.crdr_type) = 'dr' then a.bill_amount else (case when b.currency1 is null then 0 else b.currency1 end ) end) as db_amount, abs(a.bill_amount - (case when b.currency1 is null then 0 else b.currency1 end)) as as_on_balance, a.crdr_type, b.currency2, abs(a.bill_amount - (case when b.currency1 is null then 0 else b.currency1 end) - (case when b.currency2 is null then 0 else b.currency2 end)) as net_balance, (CASE WHEN a.crdr_type = 'DR' THEN b.currency2 ELSE 0 END) as posted_amt_cr, (CASE WHEN a.crdr_type = 'CR' THEN b.currency2 ELSE 0 END) as posted_amt_Dr, datediff(day, a.voucher_bill_date, @uptodate) as noof_days, datediff (day, a.voucher_bill_date, getdate()) as noof_days_s, a.Entry_Identification from voucher_bill_head a, entrytemp b, company_head tz, ledger_head tp Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and (a.bill_amount- (case when b.currency1 is null then 0 else b.currency1 end)) <> 0 and a.voucher_bill_code = b.name1 and a.company_idno = b.int1 and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno order by a.voucher_bill_date, a.voucher_bill_code"
                    cmd.ExecuteNonQuery()

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then '---- Kumaravel Textiles
                        Da = New SqlClient.SqlDataAdapter("select * from reporttemp Order by Name6, name4, name2, date1, name3, name1", con)
                        Dt = New DataTable
                        Da.Fill(Dt)

                        If Dt.Rows.Count > 0 Then

                            For i = 0 To Dt.Rows.Count - 1

                                If Trim(UCase(Dt.Rows(i).Item("Name10").ToString)) Like "CSINV-*" Then

                                    Da = New SqlClient.SqlDataAdapter("Select z1.Remarks , Z1.Cash_Discount from ClothSales_Invoice_Head z1 where 'CSINV-' + z1.ClothSales_Invoice_Code = '" & Trim(Dt.Rows(i).Item("Name10").ToString) & "'", con)
                                    Dt1 = New DataTable
                                    Da.Fill(Dt1)
                                    If Dt1.Rows.Count > 0 Then

                                        cmd.CommandText = "Update ReportTemp set Name11 = '" & Trim(Dt1.Rows(0).Item("Remarks").ToString) & "', meters1 = " & Str(Val(Dt1.Rows(0).Item("Cash_Discount").ToString)) & "  Where Name10 = '" & Trim(Dt.Rows(i).Item("Name10").ToString) & "'"
                                        cmd.ExecuteNonQuery()

                                    End If
                                    Dt1.Clear()

                                End If
                            Next
                        End If
                    End If



                    H1 = "DAYS (I)"
                    H2 = "DAYS (S)"

                    cmd.CommandText = "Update ReportTemp set name8 = '" & Trim(H1) & "', name9 = '" & Trim(H2) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, name1, name2, name3, Date1, Date2, currency1, currency2, currency3, name4, currency4, currency5, currency6, currency7, int6, int7, name8, name9, name10 ,Name11,meters1  from reporttemp Order by name2, date1, name3, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        IpColNm1 = "'[HIDDEN]'"
                        cmd.CommandText = "Insert into reporttemp(name1, Currency12) values (" & IpColNm1 & ", -9999)"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Update ReportTemp set name8 = '" & Trim(H1) & "', name9 = '" & Trim(H2) & "'"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, name1, name2, name3, Date1, Date2, currency1, currency2, currency3, name4, currency4, currency5, currency6, currency7, int6, int7, name8, name9, name10 ,Name11,meters1 from reporttemp Order by name2, date1, name3, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Trim(LCase(RptIpDet_ReportName)) = "customer bill pending - single" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Customer_Bill_Pending_Single.rdlc"

                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "customer bill pending - single - with postdated amount" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Customer_Bill_Pending_Single_With_PostDated_Amount.rdlc"

                    Else
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Customer_Bill_Pending_All.rdlc"

                    End If



                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                    RptViewer.Focus()
                    SendKeys.Send("{TAB}")

                Case "customer bill details - single", "customer bill details - agentwise - single"
                    Customer_Bill_Details_Single()
                Case "customer bill pending aging analysis"
                    Customer_Bills_Pending_AgingAnalysis()

            End Select

        Catch ex As Exception
            Dt.Dispose()
            Dt1.Dispose()
            Dt2.Dispose()
            Da.Dispose()
            cmd.Dispose()

            MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Customer_Bill_Details_Single()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dtbl1 As New DataTable
        Dim Dt1 As New DataTable
        Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        Dim Comp_IdNo As Integer
        Dim i As Integer
        Dim Led_IdNo As Integer
        Dim Nr As Long = 0
        Dim IpColVal1 As String = ""
        Dim rf As String = ""
        Dim amt As Single
        Dim v_amt As Single
        Dim Prev_SlNo As Long = 0
        Dim Ag_JoinType As String = ""
        Dim Tot_Bill_Amt As Single = 0

        If IsDate(dtp_FromDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_FromDate.Visible = True And dtp_FromDate.Enabled = True Then dtp_FromDate.Focus()
            Exit Sub
        End If

        If IsDate(dtp_ToDate.Text) = False Then
            MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_ToDate.Visible = True And dtp_ToDate.Enabled = True Then dtp_ToDate.Focus()
            Exit Sub
        End If

        Comp_IdNo = 0
        If cbo_Company.Visible Then Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

        Led_IdNo = 0
        If cbo_Ledger.Visible Then
            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            If Led_IdNo = 0 Then
                MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                Exit Sub
            End If
        End If

        RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "" And Trim(RptHeading3) <> "", "   -   ", "") & RptHeading3
        RptHeading3 = ""

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        Cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt

        IpColVal1 = ""
        If cbo_Company.Visible = True Then
            If Trim(cbo_Company.Text) <> "" Then
                RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))
                IpColVal1 = "[HIDDEN]"
            End If

        Else
            IpColVal1 = "[HIDDEN]"

        End If


        Ag_JoinType = "LEFT OUTER JOIN"
        If cbo_Ledger.Visible = True And Val(Led_IdNo) <> 0 Then

            If Trim(LCase(RptIpDet_ReportName)) = "customer bill details - agentwise - single" Then
                RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tA.Ledger_IdNo = " & Str(Val(Led_IdNo))
                Ag_JoinType = "INNER JOIN"
            Else
                RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

        End If

        Cmd.Connection = con

        'Case "customer bill details - single", "customer bill details - agentwise - single"
        'Cmd.CommandText = "Insert into reporttempsub ( Name9, Name10, Name1, Name2, Name3, Name4, Name5, currency" & Trim(i + 1) & " ) Select tA.Ledger_Name, tP.Ledger_Name, '" & Trim(S1) & "', '" & Trim(S2) & "', '" & Trim(S3) & "', '" & Trim(S4) & "', '" & Trim(S5) & "', sum(a.debit_amount - a.credit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo INNER JOIN Ledger_Head tA ON a.Agent_IdNo <> 0 and a.Agent_idno = tA.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and a.debit_amount > a.credit_amount group by tA.Ledger_Name, tP.Ledger_Name"

        Cmd.CommandText = "truncate table reporttempsub"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into reporttempsub (  int1,   date3            ,   currency1,   name1            ,   date1            ,   meters1    ,   name9          ,   currency2  ,                currency3                                                                                                                                                                                                    ,   name2        ,   name3    ,   name4       ,   name10       ,   date2       ,   name5    ,   name6                   ,   name7       ,   name8            ,   name11  ,   Int7       ,    Name12     ,    Name13     ) " _
                                & " Select   tz.company_idno, a.voucher_bill_date, a.amount   , b.Voucher_Bill_Code, b.voucher_bill_date, b.For_OrderBy, b.voucher_bill_no, b.bill_amount, ( Select sum(z.amount) from voucher_bill_details z where z.voucher_bill_date < @fromdate and z.voucher_bill_code = b.voucher_bill_code and z.company_idno = b.company_idno and z.ledger_idno = b.ledger_idno ) as prv_amount, b.party_bill_no, b.crdr_type, d.voucher_code, d.voucher_no, c.voucher_date, c.narration, e.ledger_name as bank_name, d.voucher_type, d.entry_identification, d.Entry_ID, b.Ledger_IdNo, tP.Ledger_name, tAr.Area_Name   " _
                                & " from voucher_bill_details a, voucher_bill_head b INNER JOIN Ledger_Head tP ON b.Ledger_IdNo <> 0 and b.Ledger_idno = tP.Ledger_IdNo LEFT OUTER JOIN Area_Head tAR ON tP.Area_idNo = tAR.Area_IdNo " & Ag_JoinType & " Ledger_Head tA ON b.Agent_IdNo <> 0 and b.Agent_idno = tA.Ledger_IdNo, voucher_details c, voucher_head d, ledger_head e, company_head tz " _
                                & " where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date between @fromdate and @todate and tZ.company_idno <> 0 and a.company_idno = tZ.company_idno and a.voucher_bill_code = b.voucher_bill_code and a.company_idno = b.company_idno and b.ledger_idno = c.ledger_idno and ( a.entry_identification = 'VOUCH-'+d.Voucher_Code or a.entry_identification = d.entry_identification ) and a.company_idno = d.company_idno and c.Voucher_Code = d.Voucher_Code and c.company_idno = d.company_idno and (case when a.ledger_idno = d.creditor_idno then d.debtor_idno else d.creditor_idno end) = e.ledger_idno order by b.voucher_bill_date, b.For_OrderBy, b.voucher_bill_no, a.voucher_bill_date"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into reporttempsub ( int1,         currency1                                                                                                                                                          ,   name1            ,   name9          ,   date1            ,   meters1    ,   currency2  ,   name2        ,   name3    ,   Int7       ,    Name12     ,     name13  ) " _
                                & "Select   tz.company_idno, (select sum(z.amount) from voucher_bill_details z where z.voucher_bill_code = a.voucher_bill_code and z.company_idno = a.company_idno and z.voucher_bill_date < @fromdate ), a.voucher_bill_code, a.voucher_bill_no, a.voucher_bill_date, a.For_OrderBy, a.bill_amount, a.party_bill_no, a.crdr_type, a.Ledger_IdNo, tP.Ledger_name, tAr.Area_Name  " _
                                & " from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tZ.company_idno  INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo LEFT OUTER JOIN Area_Head tAR ON tP.Area_idNo = tAR.Area_IdNo " & Ag_JoinType & " Ledger_Head tA ON a.Agent_IdNo <> 0 and a.Agent_idno = tA.Ledger_IdNo " _
                                & " where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @todate and (case when a.crdr_type = 'CR' then a.credit_amount else a.debit_amount end ) <> ( select ( case when sum(vbd.amount) is null then 0 else sum(vbd.amount) end ) from voucher_bill_details vbd where vbd.voucher_bill_code = a.voucher_bill_code and vbd.company_idno = a.company_idno and vbd.voucher_bill_date <= @todate ) and a.voucher_bill_code NOT IN ( select z2.name1 from reporttempsub z2) "
        Cmd.ExecuteNonQuery()


        Cmd.CommandText = "truncate table reporttemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into reporttemp (   date1,   meters1,   name1,    name2            ,   name3,   Currency1,   currency3,   name4,                                                                          name5                                                                                    ,   date2,   name6,   name7,   date3,   currency2,                                                       name8                                      , currency4,   name9, currency5, name10,                  Int6           , Int7, Name12, name13  ) " _
                               & "    Select        a.date1, a.meters1, a.name1, tz.company_shortname, a.name2, a.currency2, a.currency3, a.name3, (case when a.currency1 <> 0 then  (case when a.name8 NOT LIKE 'VOUCH-%' and len(a.name8) > 12 then a.name11 else 'V' + a.name7 + '-' + a.name10 end)  else '' end), a.date2, a.name6, a.name5, a.date3, a.currency1, (case when a.currency1 <> 0 then (case when a.name3 = 'CR' then 'Dr' else 'Cr' end ) else '' end),      0   , a.name3,       0  ,   ''  ,  datediff(day, a.date1, a.date2), Int7, Name12, name13  From ReportTempSub a, company_head tZ where a.int1 <> 0 and a.int1 = tz.company_idno"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Update reporttemp set Weight1 = (select sum(z1.bill_amount) from voucher_bill_head z1 where z1.voucher_bill_Code IN (select z2.name1 from reportTemp z2) )"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Update reporttemp set Weight2 = (select sum(z1.bill_amount) from voucher_bill_head z1 where z1.Ledger_Idno = a.Int7 and z1.voucher_bill_Code IN (select z2.name1 from reportTemp z2 where z2.Int7 = z1.Ledger_Idno ) ) from reporttemp a, voucher_bill_head b where a.Int7 = b.Ledger_Idno "
        Cmd.ExecuteNonQuery()

        If Trim(IpColVal1) <> "" Then
            Cmd.CommandText = "Update reporttemp set name2 = '" & Trim(IpColVal1) & "'"
            Nr = Cmd.ExecuteNonQuery()
        End If

        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, date1, meters1, name1, name2, name3, Currency1, currency3, name4, name5, date2, name6, name7, date3, currency2, name8, currency4, name9, currency5, name10, Int6, Weight1, Weight2, Int7, Name12, name13 from reporttemp order by name13, name12, date1, meters1, name1, date2, date3", con)
        Dtbl1 = New DataTable
        Da.Fill(Dtbl1)

        rf = ""
        If Dtbl1.Rows.Count > 0 Then

            For i = 0 To Dtbl1.Rows.Count - 1

                If rf <> Dtbl1.Rows(i).Item("name1").ToString() Then
                    If amt <> 0 Then
                        Dtbl1.Rows(Prev_SlNo).Item("currency5") = Val(amt)
                        Dtbl1.Rows(Prev_SlNo).Item("name10") = Dtbl1.Rows(Prev_SlNo).Item("name4").ToString()
                    End If

                    amt = Val(Dtbl1.Rows(i).Item("Currency1").ToString())
                    If Dtbl1.Rows(i).Item("currency3").ToString() <> "" Then amt = amt - Val(Dtbl1.Rows(i).Item("currency3").ToString())
                End If

                v_amt = 0
                If Dtbl1.Rows(i).Item("currency2").ToString() <> "" Then v_amt = Val(Dtbl1.Rows(i).Item("currency2").ToString())

                amt = amt - v_amt

                Dtbl1.Rows(i).Item("currency4") = Val(amt)
                Dtbl1.Rows(i).Item("name9") = Dtbl1.Rows(i).Item("name4").ToString()

                rf = Dtbl1.Rows(i).Item("name1").ToString()

                Prev_SlNo = i

            Next i

            If amt <> 0 Then
                Dtbl1.Rows(Prev_SlNo).Item("currency5") = Val(amt)
                Dtbl1.Rows(Prev_SlNo).Item("name10") = Dtbl1.Rows(Prev_SlNo).Item("name4").ToString()
            End If

        Else
            Cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
            Cmd.ExecuteNonQuery()

            If Trim(IpColVal1) <> "" Then
                Cmd.CommandText = "Update reporttemp set name2 = '" & Trim(IpColVal1) & "'"
                Nr = Cmd.ExecuteNonQuery()
            End If

            Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, date1, meters1, name1, name2, name3, Currency1, name4, name5, date2, name6, name7, date3, currency2, name8, currency3, name9, currency4, name10, Int6, Weight1, Weight2, Int7, Name12, name13 from reporttemp order by name13, name12, date1, meters1, name1, date2, date3", con)
            Dtbl1 = New DataTable
            Da.Fill(Dtbl1)

        End If

        RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
        RpDs1.Name = "DataSet1"
        RpDs1.Value = Dtbl1

        If Trim(LCase(RptIpDet_ReportName)) = "customer bill details - agentwise - single" Then
            RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Customer_Bill_Details_AgentWise_Single.rdlc"
        Else
            RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Customer_Bill_Details_Single.rdlc"
        End If


        RptViewer.LocalReport.DataSources.Clear()

        RptViewer.LocalReport.DataSources.Add(RpDs1)

        RptViewer.LocalReport.Refresh()
        RptViewer.RefreshReport()

        RptViewer.Visible = True

        RptViewer.Focus()
        SendKeys.Send("{TAB}")

    End Sub

    Private Sub Customer_Bills_Pending_AgingAnalysis()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dtbl1 As New DataTable
        Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource
        Dim RptCondt As String = ""
        Dim CompCondt As String = ""
        Dim Comp_IdNo As Integer
        Dim b() As String
        Dim i As Integer
        Dim S As String
        Dim oldvl As String
        Dim RepPeriods As String = ""

        If IsDate(dtp_FromDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_FromDate.Visible = True And dtp_FromDate.Enabled = True Then dtp_FromDate.Focus()
            Exit Sub
        End If

        Comp_IdNo = 0
        If cbo_Company.Visible Then Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

        RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "" And Trim(RptHeading3) <> "", "   -   ", "") & RptHeading3
        RptHeading3 = ""

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        RptCondt = CompCondt
        If cbo_Company.Visible = True And Val(Comp_IdNo) <> 0 Then
            RptCondt = " a.Company_IdNo = " & Str(Val(Comp_IdNo))
        End If

        If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
            RepPeriods = cbo_Ledger.Text
        End If
        If Trim(RepPeriods) = "" Then
            RepPeriods = "30,60,90,120"
        End If

        b = Split(RepPeriods, ",")

        Cmd.Connection = con

        Cmd.CommandText = "truncate table reporttempsub"
        Cmd.ExecuteNonQuery()

        oldvl = "0"

        For i = 0 To UBound(b)

            S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

            Cmd.CommandText = "Insert into reporttempsub ( Int1,    Name1      , Int2, Name2, currency1) " &
                                        " Select tP.Ledger_idNo, tP.Ledger_Name, " & Str(Val(i) + 1) & ", '" & Trim(S) & "',      0    from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and a.debit_amount > a.credit_amount group by tP.Ledger_idNo, tP.Ledger_Name"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into reporttempsub ( Int1,    Name1      , Int2, Name2, currency1) " &
                                        " Select tP.Ledger_idNo, tP.Ledger_Name, " & Str(Val(i) + 1) & ", '" & Trim(S) & "', sum(a.debit_amount - a.credit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) between " & Str(Val(oldvl)) & " and " & Str(Val(b(i))) & " and a.debit_amount > a.credit_amount group by tP.Ledger_idNo, tP.Ledger_Name"
            Cmd.ExecuteNonQuery()

            oldvl = Val(b(i)) + 1

        Next i

        If Val(oldvl) <> 0 Then

            S = "ABV " & Trim(Val(oldvl) - 1)

            Cmd.CommandText = "Insert into reporttempsub ( Int1,    Name1      , Int2, Name2, currency1) " &
                                        " Select tP.Ledger_idNo, tP.Ledger_Name, " & Str(Val(i) + 1) & ", '" & Trim(S) & "',      0    from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate) >= " & Str(Val(oldvl)) & " and a.debit_amount > a.credit_amount group by tP.Ledger_idNo, tP.Ledger_Name"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into reporttempsub ( Int1,    Name1      , Int2, Name2, currency1) " &
                                        " Select tP.Ledger_idNo, tP.Ledger_Name, " & Str(Val(i) + 1) & ", '" & Trim(S) & "', sum(a.debit_amount - a.credit_amount) from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tz.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_idno = tP.Ledger_IdNo Where " & Trim(RptCondt) & IIf(RptCondt <> "", " and ", "") & " a.voucher_bill_date <= @uptodate and datediff(dd, a.voucher_bill_date, @uptodate)  >= " & Str(Val(oldvl)) & " and a.debit_amount > a.credit_amount group by tP.Ledger_idNo, tP.Ledger_Name"
            Cmd.ExecuteNonQuery()

        End If

        Cmd.CommandText = "truncate table reporttemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "insert into ReportTemp ( Int1,    Name1      , Int2, Name2, currency1 ) Select Int1, Name1, Int2, Name2, sum(currency1) from reporttempsub group by Int1, Name1, Int2, Name2 having sum(currency1) <> 0 "
        Cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1,    Name1      , Int2, Name2, currency1  from reporttemp Order by name1, Int2, name2", con)
        Dtbl1 = New DataTable
        Da.Fill(Dtbl1)

        If Dtbl1.Rows.Count = 0 Then

            oldvl = "0"

            For i = 0 To UBound(b)

                S = Trim(Val(oldvl)) & " TO " & Trim(Val(b(i)))

                Cmd.CommandText = "Insert into reporttempsub ( Int2, Name2, Currency12) values (" & Str(Val(i) + 1) & ", '" & Trim(S) & "', -99999 )"

                oldvl = Val(b(i)) + 1

            Next i

            If Val(oldvl) <> 0 Then

                S = "ABV " & Trim(Val(oldvl) - 1)

                Cmd.CommandText = "Insert into reporttempsub ( Int2, Name2, Currency12) values (" & Str(Val(i) + 1) & ", '" & Trim(S) & "', -99999 )"
                Cmd.ExecuteNonQuery()

            End If

            Cmd.CommandText = "truncate table reporttemp"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "insert into ReportTemp ( Int1,    Name1      , Int2, Name2, currency1 ) Select Int1, Name1, Int2, Name2, sum(currency1) from reporttempsub group by Int1, Name1, Int2, Name2 "
            Cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int1,    Name1      , Int2, Name2, currency1  from reporttemp Order by name1, Int2, name2", con)
            Dtbl1 = New DataTable
            Da.Fill(Dtbl1)

        End If

        RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
        RpDs1.Name = "DataSet1"
        RpDs1.Value = Dtbl1

        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Party_OutStanding_DayWise.rdlc"

        RptViewer.LocalReport.DataSources.Clear()

        RptViewer.LocalReport.DataSources.Add(RpDs1)

        RptViewer.LocalReport.Refresh()
        RptViewer.RefreshReport()

        RptViewer.Visible = True

    End Sub

    Private Sub Register_Report()

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dtbl1 As New DataTable
        Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource
        Dim RptCondt As String
        Dim CompCondt As String
        Dim InpNm1 As String
        Dim FlName1 As String = "", FlName2 As String = ""
        Dim CompTinNo As String = ""
        Dim Cnt_GrpIdNos As String
        Dim Cnt_IdNo As Integer, Cnt_UndIdNo As Integer
        Dim Cnt_Cond As String
        Dim nr As Integer = 0
        Dim VouCond As String = ""
        Dim ReportHd As String = ""

        Try

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "(Company_Type <> 'UNACCOUNT')"
            End If

            Select Case Trim(LCase(Common_Procedures.RptInputDet.ReportName))

                Case "purchase register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptCondt = CompCondt

                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    cmd.CommandText = "Insert into ReportTemp(Name1, Name2, Meters1, Date1, Name3, Name4, Currency1) Select a.Purchase_Code, convert(varchar,a.Purchase_No)+'R', a.for_OrderBy, a.Purchase_Date, b.Ledger_Name, a.Bill_No, a.Net_Amount from Purchase_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo, Ledger_Head b where " & Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Purchase_Date between @fromdate and @todate and a.ledger_idno = b.ledger_idno Order by a.Purchase_Date, a.for_OrderBy, a.Purchase_Code, a.Purchase_No"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp(Name1, Name2, Meters1, Date1, Name3, Name4, Currency1) Select a.Other_GST_Entry_Reference_Code,convert(varchar, a.Other_GST_Entry_Reference_No)+'(D)', a.ForOrderBy_ReferenceCode, a.Other_GST_Entry_Date, b.Ledger_Name, a.Bill_No, a.Net_Amount from Other_GST_Entry_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo, Ledger_Head b where " & Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") &
                                      " a.Other_GST_Entry_Date between @fromdate and @todate and a.ledger_idno = b.ledger_idno and Other_GST_Entry_Type = 'PURC' Order by a.Other_GST_Entry_Date, a.ForOrderBy_ReferenceCode, a.Other_GST_Entry_Reference_Code, a.Other_GST_Entry_Reference_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Name4, Currency1 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Name4, Currency1 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_PurchaseRegister.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "sales register", "garments invoice register", "invoice register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt

                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1080" Then
                        If Cbo_SalesMan.Visible = True And Trim(Cbo_SalesMan.Text) <> "" Then
                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Salesman_IdNo = " & Str(Val(Common_Procedures.Salesman_NameToIdNo(con, Cbo_SalesMan.Text)))
                        End If
                    End If
                    cmd.CommandText = "Insert into ReportTemp ( Name1,   Name2   ,   Meters1    ,   Date1     ,    Name3       , Name4             ,    Weight1  ,   Currency1  , Currency2   , Currency3         , Name10 ) " &
                                            "     Select a.Sales_Code, a.Sales_No, a.for_OrderBy, a.Sales_Date, tP.Ledger_Name , tP.Ledger_EmailID , a.Total_Qty, a.Net_Amount , a.Tax_Amount, a.Assessable_Value ,a.Narration  from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ledger_idno = tP.ledger_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_Code, a.Sales_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 ,Currency2  , Name4 , Name10 Currency3 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 , Name4 ,Name5 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "garments invoice register" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Garments_Invoice_Register.rdlc"
                    ElseIf Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "invoice register" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Invoice_Register.rdlc"
                    Else
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1068" Then
                            RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SaleRegister_WithMail.rdlc"
                        Else
                            RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SaleRegister_Simple.rdlc"
                        End If
                    End If

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "milk sales register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_Agent.Visible = True And Trim(cbo_Agent.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Agent_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)))
                    End If

                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp ( Name1,   Name2   ,   Meters1    ,   Date1     ,    Name3       , Name4                    ,  Name5     ,     Name6   ,  INT2       ,   int3          , Meters2 , Currency1   ) " &
                                            "     Select a.Sales_Code, a.Sales_No, a.for_OrderBy, a.Sales_Date, tP.Ledger_Name ,d.Ledger_Name AS AgentName ,e.Item_Name  ,f.Area_Name , a.Noof_Items,a.Extra_Quantity ,a.Rate   , a.Amount from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ledger_idno = tP.ledger_idno INNER JOIN Ledger_Head d ON a.Agent_idno = d.Ledger_idno INNER JOIN Item_Head e ON a.Item_idno = e.Item_idno  INNER JOIN Area_Head f ON a.Area_idno = f.Area_idno   Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_Code, a.Sales_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 ,Weight2 ,Meters2 , Name4 , Name5  ,  Name6  ,int2 ,int3,  Currency3 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 , Name4 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1


                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Milk_Sales_Register.rdlc"


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "sales delivery register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp ( Name1          ,   Name2         ,   Meters1    ,                   Date1     ,    Name3       ,  Name4         ,  Name5 , INT2  ,   name6   ,   Name7       ,   Name8           ,  name9     ,    Meters2  ,  Meters3 ) " &
                                            "     Select a.Sales_Delivery_Code, a.Sales_Delivery_No, a.for_OrderBy, a.Sales_Delivery_Date, tP.Ledger_Name , d.Item_Name  ,e.Unit_Name ,a.Quantity,a.Order_No,a.Item_Description,f.Colour_Name   ,  g.Size_NAme,   a.Rate , a.Amount   from Sales_Delivery_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ledger_idno = tP.ledger_idno  INNER JOIN Item_Head d ON a.Item_idno = d.Item_idno  INNER JOIN Unit_Head e ON a.Unit_idno = e.Unit_idno  LEFT OUTER JOIN Colour_Head f ON f.Colour_idno = a.Colour_idno LEFT OUTER JOIN Size_Head g ON a.Size_idno = g.Size_idno  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Delivery_Date between @fromdate and @todate Order by a.Sales_Delivery_Date, a.for_OrderBy, a.Sales_Delivery_Code, a.Sales_Delivery_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3,  Name4 , Name5   ,int2, Name8,  Name6, Name7,Name9,Meters2,meters3 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 , Name4 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Trim(Common_Procedures.settings.CustomerCode) = "1201" Or Trim(Common_Procedures.settings.CustomerCode) = "1117" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Delivery_Register.rdlc"
                    Else

                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Delivery_Register.rdlc"
                    End If

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "sales delivery summary"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If



                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If



                    cmd.CommandText = "Insert into ReportTemp (  Name1   , Name2  ,       int2    ) " &
                                        " Select        c.Ledger_Name, d.Item_Name, SUM(a.Quantity)  from Sales_Delivery_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo   INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Delivery_Date between @fromdate and @todate group by c.Ledger_Name ,  d.Item_Name having sum(a.Amount) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Name3 ,int2  from reporttemp where int2 <> 0 Order by Name1, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Weight1 from reporttemp Order by Name1, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Delivery_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "sales delivery pending"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp ( Name1          ,   Name2         ,   Meters1    ,                   Date1     ,    Name3       ,  Name4         ,  Name5 , INT2    ) " &
                                            "     Select a.Sales_Delivery_Code, a.Sales_Delivery_No, a.for_OrderBy, a.Sales_Delivery_Date, tP.Ledger_Name , d.Item_Name  ,e.Unit_Name ,a.Quantity from Sales_Delivery_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ledger_idno = tP.ledger_idno  INNER JOIN Item_Head d ON a.Item_idno = d.Item_idno  INNER JOIN Unit_Head e ON a.Unit_idno = e.Unit_idno   Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Receipt_Quantity = 0 and a.Sales_Delivery_Date between @fromdate and @todate Order by a.Sales_Delivery_Date, a.for_OrderBy, a.Sales_Delivery_Code, a.Sales_Delivery_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3,  Name4 , Name5   ,int2 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 , Name4 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1


                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Delivery_Pending.rdlc"


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "sales quotation register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    'If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    'End If

                    cmd.CommandText = "Insert into ReportTemp ( Name1          ,   Name2         ,   Meters1    ,                   Date1     ,    Name3     ,   Currency1   ) " &
                                            "     Select a.Sales_Quotation_Code, a.Sales_Quotation_No, a.for_OrderBy, a.Sales_Quotation_Date, tP.Ledger_Name ,a.Net_Amount  from Sales_Quotation_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ledger_idno = tP.ledger_idno     Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Quotation_Date between @fromdate and @todate Order by a.Sales_Quotation_Date, a.for_OrderBy, a.Sales_Quotation_Code, a.Sales_Quotation_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3,  Currency1  from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 , Name4 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1


                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Quotation_Register.rdlc"


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "sales quotation details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If




                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3   ,   Name1                   ,   Int1        ,   Name2   ,       Weight10   ,   Date1                   ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   , Currency1  , Currency2  ,    Currency5      ,   Currency6        ,   Currency7     ,   Currency8 , Meters1, Meters2, Meters3, Meters4) " &
                                   " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Quotation_Code, a.Company_IdNo, a.Sales_Quotation_No, a.for_OrderBy, a.Sales_Quotation_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, e.Unit_Name, b.Quantity     , b.Rate     , b.Amount   , a.Tax_Amount, a.CashDiscount_Amount, a.Labour_Charge, a.Net_Amount,      0      , 0      , 0      , 0          from Sales_Quotation_Head a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Quotation_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Quotation_Code = b.Sales_Quotation_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Quotation_Date between @fromdate and @todate Order by a.Sales_Quotation_Date, a.for_OrderBy, a.Sales_Quotation_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Select sum(z.Tax_Amount), sum(z.CashDiscount_Amount), sum(z.Labour_Charge), sum(z.Net_Amount) from Sales_Quotation_Head z where z.Sales_Quotation_Code IN (Select a.Sales_Quotation_Code from Sales_Quotation_Head a LEFT OUTER JOIN Sales_Quotation_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Quotation_Code = b.Sales_Quotation_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Quotation_Date between @fromdate and @todate) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)




                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters1 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters2 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters3 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters4 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Date1, Name2, Name3, Name4, Int3, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Quotation_Details.rdlc"


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "purchase details", "purchase details - 1"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con
                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt

                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Name1,   Int1        ,   Name2      ,   Weight10   ,   Date1        ,   Name6  ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   ,   Currency1,   Currency2,   Currency3      ,   Currency4           ,   Currency5                                                              ,   Currency6          ,   Currency7     ,   Currency8 ,  Currency9                      , Meters1, Meters2, Meters3, Meters4, Meters5, Meters6) " &
                                        " Select    a.Purchase_Code, a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, a.Bill_No, c.Ledger_Name, b.SL_No, d.Item_Name, e.Unit_Name, b.Noof_Items, b.Rate     , b.Amount   , a.SubTotal_Amount, a.Total_DiscountAmount, (ISNULL(a.CGST_Amount,0)+ISNULL(a.SGST_Amount,0)+ISNULL(a.IGST_Amount,0)), a.CashDiscount_Amount, a.Round_off     , a.Net_Amount,  b.GST_Percentage               ,0       , 0      , 0      , 0      , 0      , 0             from Purchase_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Purchase_Details b ON a.Company_IdNo = b.Company_IdNo and a.Purchase_Code = b.Purchase_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo  LEFT OUTER JOIN Month_Head f ON b.Manufacture_Month_IdNo = f.Month_IdNo LEFT OUTER JOIN Month_Head g ON b.Manufacture_Month_IdNo = g.Month_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Purchase_Date between @fromdate and @todate Order by a.Purchase_Date, a.for_OrderBy, a.Purchase_No, a.Company_IdNo"

                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Select sum(z.SubTotal_Amount), sum(z.Total_DiscountAmount), sum(z.SGST_Amount+z.CGST_Amount), sum(z.CashDiscount_Amount), sum(z.Round_Off), sum(z.Net_Amount) from Purchase_Head z where z.Purchase_Code IN (Select a.Purchase_Code from Purchase_Head a LEFT OUTER JOIN Purchase_Details b ON a.Company_IdNo = b.Company_IdNo and a.Purchase_Code = b.Purchase_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Purchase_Date between @fromdate and @todate) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters1 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters2 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters3 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters4 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(4).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters5 = " & Str(Val((Dt.Rows(0)(4).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(5).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters6 = " & Str(Val((Dt.Rows(0)(5).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                    End If

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "purchase details" Then
                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Name6, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8,Currency9, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6,Name7,Name8,Name9,int3,int4,int5,int6,int7,date2,date3 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Else
                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Name6, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8,Currency9, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6,Name7,Name8,Name9,int3,int4,int5,int6,int7,date2,date3 from reporttemp Order by  convert(int,name2), Weight10, name1, Int2", con)
                    End If

                    Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)


                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Name6, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8,Currency9, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_PurchaseDetails.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "sales details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1048" Then


                        cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3   ,   Name1     ,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,Int3,   Name5    ,   Weight1   , Currency1  , Currency2  , Currency3        , Currency4             ,   Currency5      ,   Currency6          ,   Currency7     ,   Currency8 , Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 ) " &
                                       " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name,b.Rolls  ,e.Unit_Name, b.Weight     , b.Rate     , b.Amount   , a.SubTotal_Amount, a.Total_DiscountAmount, a.Total_TaxAmount, a.CashDiscount_Amount, a.AddLess_Amount, a.Net_Amount, 0      , 0      , 0      , 0      , 0      , 0        from Sales_Head a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Select sum(z.SubTotal_Amount), sum(z.Total_DiscountAmount), sum(z.Total_TaxAmount), sum(z.CashDiscount_Amount), sum(z.AddLess_Amount), sum(z.Net_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Head a LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate) "
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)

                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1119" Then

                        cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3   ,   Name1     ,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   , Currency1  , Currency2  , Currency3        , Currency4             ,   Currency5      ,   Currency6          ,   Currency7     ,   Currency8 , Currency9       ,  Currency10   ,  Meters1, Meters2, Meters3, Meters4, Meters5, Meters6,Meters7 ,  Meters8 ,    Name6           ,     int3          ,     Name7                 ,      int4          ,      Date2         ,      int5             ,   int6        ,        Name8                  ,    int7      ,  Date3 ) " &
                                       " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, e.Unit_Name, b.Noof_Items, b.Rate     , b.Amount   , a.SubTotal_Amount, a.Total_DiscountAmount, a.Total_TaxAmount, a.CashDiscount_Amount, a.AddLess_Amount, a.Net_Amount,a.Advance_Amount , a.Balance_Amount, 0      , 0      , 0      , 0      , 0      , 0     ,  0      ,    0      , b.Batch_Serial_No , b.Manufacture_Day  , f.Month_ShortName as MName , b.Manufacture_Year , b.Manufacture_Date , b.Expiry_Period_Days  ,b.Expiry_Day   ,  g.Month_ShortName as SName  , b.Expiry_Year,b.Expiry_Date      from Sales_Head a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo LEFT OUTER JOIN Month_Head f ON b.Manufacture_Month_IdNo = f.Month_IdNo LEFT OUTER JOIN Month_Head g ON b.Manufacture_Month_IdNo = g.Month_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Delivery_Status = 0 and a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Select sum(z.SubTotal_Amount), sum(z.Total_DiscountAmount), sum(z.Total_TaxAmount), sum(z.CashDiscount_Amount), sum(z.AddLess_Amount),sum(z.Net_Amount), sum(z.Advance_Amount),sum(z.Balance_Amount)  from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Head a LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Delivery_Status = 0 and a.Sales_Date between @fromdate and @todate) "
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)
                    Else

                        cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3   ,   Name1     ,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   , Currency1  , Currency2  , Currency3        , Currency4             ,   Currency5      ,   Currency6          ,   Currency7     ,   Currency8 , Meters1, Meters2, Meters3, Meters4, Meters5, Meters6,    Name6           ,     int3          ,     Name7                 ,      int4          ,      Date2         ,      int5             ,   int6        ,        Name8                  ,    int7      ,  Date3        ,              Currency9                   ) " &
                                       " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, e.Unit_Name, b.Noof_Items, b.Rate     , b.Amount   , a.SubTotal_Amount, a.Total_DiscountAmount, a.Total_TaxAmount, a.CashDiscount_Amount, a.AddLess_Amount, a.Net_Amount, 0      , 0      , 0      , 0      , 0      , 0     ,  b.Batch_Serial_No , b.Manufacture_Day  , f.Month_ShortName as MName , b.Manufacture_Year , b.Manufacture_Date , b.Expiry_Period_Days  ,b.Expiry_Day   ,  g.Month_ShortName as SName  , b.Expiry_Year,b.Expiry_Date , (a.CGst_Amount+a.IGst_Amount+a.SGst_Amount)     from Sales_Head a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo LEFT OUTER JOIN Month_Head f ON b.Manufacture_Month_IdNo = f.Month_IdNo LEFT OUTER JOIN Month_Head g ON b.Manufacture_Month_IdNo = g.Month_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Select sum(z.SubTotal_Amount), sum(z.Total_DiscountAmount), sum(z.Total_TaxAmount), sum(z.CashDiscount_Amount), sum(z.AddLess_Amount), sum(z.Net_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Head a LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate) "
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)

                    End If

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters1 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters2 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters3 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters4 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(4).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters5 = " & Str(Val((Dt.Rows(0)(4).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(5).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters6 = " & Str(Val((Dt.Rows(0)(5).ToString)))
                            cmd.ExecuteNonQuery()
                        End If

                        '    If IsDBNull(Dt.Rows(0)(6).ToString) = False Then
                        '        cmd.CommandText = "Update ReportTemp set Meters7 = " & Str(Val((Dt.Rows(0)(6).ToString)))
                        '        cmd.ExecuteNonQuery()
                        '    End If
                        '    If IsDBNull(Dt.Rows(0)(7).ToString) = False Then
                        '        cmd.CommandText = "Update ReportTemp set Meters8 = " & Str(Val((Dt.Rows(0)(7).ToString)))
                        '        cmd.ExecuteNonQuery()
                        '    End If
                    End If

                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Date1, Name2, Name3, Name4, Int3, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6,Meters7 ,  Meters8  ,  Currency9 , Currency10 ,Name6,Name7,Name8, Int3,int4,int5,int6,int7,Date2,Date3 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1048" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_Saara.rdlc"
                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1108" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_BatchNoDetails.rdlc"
                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1119" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_DetailsDelivery.rdlc"
                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1171" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_GST.rdlc"
                    Else
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails.rdlc"
                    End If


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "spinning invoice details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3        ,   Name1     ,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,   Weight1,   Weight2,   Currency1,   Currency2,   Currency3          ,   Currency4      ,   Currency5     ,   Currency6     ,   Currency7 ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, b.Bags   , b.Weight , b.Rate     , b.Amount   , a.CashDiscount_Amount, a.Total_TaxAmount, a.Freight_Amount, a.AddLess_Amount, a.Net_Amount from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Int2, Name4, Weight1, Weight2, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7  from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Spinning_Invoice_Details.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "garments invoice details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3        ,   Name1     ,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   ,   Currency1,   Currency2,   Currency3       ,   Currency4 ,   Currency5     ,   Currency6, Meters1, Meters2, Meters3, Meters4) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, e.Size_Name, b.Noof_Items, b.Rate     , b.Amount   , a.Assessable_Value, a.Tax_Amount, a.Freight_Amount, a.Net_Amount, 0      , 0      , 0      , 0            from Sales_Head a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Size_Head e ON b.Size_IdNo = e.Size_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo "
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Select sum(z.Assessable_Value), sum(z.Tax_Amount), sum(z.Freight_Amount), sum(z.Net_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Head a LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Size_Head e ON b.size_IdNo = e.size_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters1 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters2 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters3 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters4 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                    End If

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Garments_Invoice_Details.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "invoice details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Name1,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,   Weight1   ,   Currency1,   Currency2,   Currency3   ,   Currency7          ,   Currency4 ,   Currency5     ,   Currency6, Meters1, Meters2, Meters3, Meters4, Meters5 ) " &
                                        " Select       a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, b.Noof_Items, b.Rate     , b.Amount   , a.Gross_Amount, a.CashDiscount_Amount, a.Tax_Amount, a.Freight_Amount, a.Net_Amount, 0      , 0      , 0      , 0     ,    0       from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo "
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Select sum(z.Gross_Amount), sum(z.Tax_Amount), sum(z.Freight_Amount), sum(z.Net_Amount), sum(z.CashDiscount_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters1 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters2 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters3 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters4 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(4).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters5 = " & Str(Val((Dt.Rows(0)(4).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                    End If

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If


                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Invoice_Details.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "milk sales summary"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If



                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If



                    cmd.CommandText = "Insert into ReportTemp (             Name1   , Name2  ,  int2    , Currency1 ) " &
                                        " Select        c.Ledger_Name, d.Item_Name, SUM(a.Noof_Items+a.Extra_Quantity) , sum(a.Amount) from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo   INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate group by c.Ledger_Name ,  d.Item_Name having sum(a.Amount) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Name3 ,int2 , currency1 from reporttemp where int2 <> 0 Order by Name1, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Weight1 from reporttemp Order by Name1, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Milk_Sales_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "spinning purchase details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    'If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Variety_NameToIdNo(con, cbo_ItemName.Text)))
                    'End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3        ,   Name1        ,   Int1        ,   Name2      ,   Weight10   ,   Date1        ,   Name3      ,   Int2 ,   Name4       ,   Weight1,   Weight2,   Currency1,   Currency2,   Currency3               ,   Currency4      ,   Currency5 ,   Currency6     ,   Currency7  ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Purchase_Code, a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, c.Ledger_Name, b.Sl_No, d.Variety_Name, b.Bales  , b.Weight , b.Rate     , b.Amount   , a.AddLess_BeforeTax_Amount, a.Total_TaxAmount, a.Tax_Amount, a.AddLess_Amount, a.Net_Amount from Purchase_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Purchase_Details b ON a.Company_IdNo = b.Company_IdNo and a.Purchase_Code = b.Purchase_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Variety_Head d ON b.Item_IdNo = d.Variety_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Purchase_Date between @fromdate and @todate Order by a.Purchase_Date, a.for_OrderBy, a.Purchase_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Int2, Name4, Weight1, Weight2, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7  from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Spinning_Purchase_Details.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "sales warranty report"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    If cbo_PhoneNo.Visible = True And Trim(cbo_PhoneNo.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Party_PhoneNo = '" & Trim(cbo_PhoneNo.Text) & "'"
                    End If
                    If cbo_SerialNo.Visible = True And Trim(cbo_SerialNo.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Serial_No = '" & Trim(cbo_SerialNo.Text) & "'"
                    End If

                    cmd.CommandText = "Insert into ReportTemp ( Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3        ,   Name1        ,   Int1        ,   Name2      ,   Weight10   ,   Date1        ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Name6    ,   Weight1   ,   Currency1,   Currency2,   Currency3,   Currency4           ,   Currency5      ,   Currency6          ,   Currency7     ,   Currency8 , Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 ) " &
                                        " Select  '" & Trim(CompName) & "'  , '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, e.Unit_Name, b.Serial_No, b.Noof_Items, b.Rate     , b.Amount   , a.SubTotal_Amount, a.Total_DiscountAmount, a.Total_TaxAmount, a.CashDiscount_Amount, a.AddLess_Amount, a.Net_Amount, 0      , 0      , 0      , 0      , 0      , 0        from Sales_Head a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Name6, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Warrenty_Register.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "annexure-i", "annexurei-excel"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexurei-excel" Then
                        If cbo_Company.Visible = True Then
                            If Trim(cbo_Company.Text) = "" Then
                                MessageBox.Show("Invalid Company", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                If cbo_Company.Enabled Then cbo_Company.Focus()
                                Exit Sub
                            End If
                        End If
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    InpNm1 = "tZ.Company_ShortName"

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()




                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Then

                        cmd.CommandText = "Insert into ReportTemp ( Name1,   Int1        ,   Name2      ,   meters1    ,        Date1   ,                 Name3                                                                                                                                                        ,   Name4  ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                 Name7                                              ,          Meters2                    ,   Meters3 ,       Meters4    ,  name9 , Name8 ) " &
                                          " Select      a.Purchase_Code  , a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, (cast(DATEPART(dd, a.Purchase_Date) as varchar) + '/' + cast(DATEPART(mm, a.Purchase_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Purchase_Date) as varchar) ) as PurcDate, a.Bill_No, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when c.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end), sum(a.Assessable_Value) as Ass_Value, a.Tax_Perc, sum(a.Tax_Amount),    'R' , (select top 1 z3.Commodity_Code from Purchase_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.Purchase_Code = a.Purchase_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) from Purchase_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.area_idno = d.area_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Purchase_Date between @fromdate and @todate and a.Tax_Amount <> 0 group by a.Purchase_Code, a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, a.Bill_No, c.Ledger_MainName, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, c.Ledger_TinNo, d.Area_Name, a.Tax_Perc having sum(a.Tax_Amount) <> 0 "
                        cmd.ExecuteNonQuery()

                    Else

                        cmd.CommandText = "Insert into ReportTemp ( Name1,   Int1        ,   Name2      ,   meters1    ,        Date1   ,                 Name3                                                                                                                                                        ,   Name4  ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                 Name7                                              ,           Meters2                            ,   Meters3 ,       Meters4    ,  name9 ,Name8 ) " &
                                          " Select      a.Purchase_Code  , a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, (cast(DATEPART(dd, a.Purchase_Date) as varchar) + '/' + cast(DATEPART(mm, a.Purchase_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Purchase_Date) as varchar) ) as PurcDate, a.Bill_No, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when c.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end),  sum(b.Amount-b.Discount_Amount) as Ass_Value, b.Tax_Perc, sum(b.Tax_Amount+b.TaxAmount_Difference),    'R' ,(select top 1 z3.Commodity_Code from Purchase_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.Purchase_Code = a.Purchase_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) from Purchase_Head a INNER JOIN Purchase_Details b ON a.Company_IdNo = b.Company_IdNo and a.Purchase_Code = b.Purchase_Code INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.area_idno = d.area_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Purchase_Date between @fromdate and @todate and b.Tax_Amount <> 0 group by a.Purchase_Code, a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, a.Bill_No, c.Ledger_MainName, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, c.Ledger_TinNo, d.Area_Name, b.Tax_Perc having sum(b.Tax_Amount) <> 0 "
                        cmd.ExecuteNonQuery()


                        'cmd.CommandText = "Insert into ReportTemp ( Name1 ,   Int1        ,   Name2         ,   meters1    ,        Date1      ,                 Name3                                                                                                                                                                 ,   Name4  ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                 Name7                                              , Name8 ,          Meters2                ,   Meters3 ,       Meters4    ,  name9  ) " & _
                        '                  " Select      a.SalesReturn_Code, a.Company_IdNo, a.SalesReturn_No, a.for_OrderBy, a.SalesReturn_Date, (cast(DATEPART(dd, a.SalesReturn_Date) as varchar) + '/' + cast(DATEPART(mm, a.SalesReturn_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.SalesReturn_Date) as varchar) ) as SRetDate, a.Bill_No, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when c.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end), '780' , sum(a.Gross_Amount) as Ass_Value, a.Tax_Perc, sum(a.Tax_Amount),   'R'   from SalesReturn_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.area_idno = d.area_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.SalesReturn_Date between @fromdate and @todate and a.Tax_Amount <> 0 group by a.SalesReturn_Code, a.Company_IdNo, a.SalesReturn_No, a.for_OrderBy, a.SalesReturn_Date, a.Bill_No, c.Ledger_MainName, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, c.Ledger_TinNo, d.Area_Name, a.Tax_Perc having sum(a.Tax_Amount) <> 0 "
                        'cmd.ExecuteNonQuery()

                    End If

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4,  name9  from reporttemp Order by Date1, meters1, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Truncate table ReportTemp"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4, name9  from reporttemp Order by Date1, meters1, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If


                'If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexurei-excel" Then

                '    Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()

                '    If xlApp Is Nothing Then
                '        MessageBox.Show("Excel is not properly installed!!", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)
                '        Return
                '        Exit Sub
                '    End If

                '    FlName1 = Trim(Common_Procedures.AppPath) & "\Images\a1.xls"
                '    If File.Exists(FlName1) = False Then
                '        MessageBox.Show("Invalid Master Annexure file " & Chr(13) & FlName1, "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)
                '        Return
                '        Exit Sub
                '    End If

                '    CompTinNo = Common_Procedures.get_FieldValue(con, "Company_Head", "Company_TinNo", "(Company_ShortName = '" & Trim(cbo_Company.Text) & "')")
                '    FlName2 = "c:\" & Trim(CompTinNo) & Format(Convert.ToDateTime(dtp_FromDate.Text), "MMyyyy").ToString & "A1.xls"
                '    If File.Exists(FlName2) = True Then
                '        File.Delete(FlName2)
                '    End If

                '    File.Copy(FlName1, FlName2, True)

                '    Dim xlWorkBook As Excel.Workbook
                '    Dim xlWorkSheet As Excel.Worksheet
                '    Dim misValue As Object = System.Reflection.Missing.Value

                '    xlWorkBook = xlApp.Workbooks.Open(FlName2)
                '    'xlWorkBook = xlApp.Workbooks.Add(misValue)
                '    xlWorkSheet = xlWorkBook.Sheets(1)

                '    Try

                '        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4,  name9  from reporttemp Order by Date1, meters1, Name2", con)
                '        Dtbl1 = New DataTable
                '        Da.Fill(Dtbl1)

                '        If Dtbl1.Rows.Count > 0 Then

                '            For i = 0 To Dtbl1.Rows.Count - 1
                '                xlWorkSheet.Cells(i + 2, 1) = Val(Val(i) + 1)
                '                xlWorkSheet.Cells(i + 2, 2) = Trim(Dtbl1.Rows(i).Item("Name5").ToString)
                '                xlWorkSheet.Cells(i + 2, 3) = Trim(Dtbl1.Rows(i).Item("Name7").ToString)
                '                xlWorkSheet.Cells(i + 2, 4) = Trim(Dtbl1.Rows(i).Item("Name8").ToString)
                '                xlWorkSheet.Cells(i + 2, 5) = Trim(Dtbl1.Rows(i).Item("Name4").ToString)
                '                xlWorkSheet.Cells(i + 2, 6) = Trim(Dtbl1.Rows(i).Item("Name3").ToString)
                '                xlWorkSheet.Cells(i + 2, 7) = Val(Dtbl1.Rows(i).Item("Meters2").ToString)
                '                xlWorkSheet.Cells(i + 2, 8) = Val(Dtbl1.Rows(i).Item("Meters3").ToString)
                '                xlWorkSheet.Cells(i + 2, 9) = Val(Dtbl1.Rows(i).Item("Meters4").ToString)
                '                xlWorkSheet.Cells(i + 2, 10) = Trim(Dtbl1.Rows(i).Item("Name9").ToString)
                '            Next

                '        End If

                '        xlWorkBook.Save()
                '        'xlWorkBook.SaveAs(FlName2, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue)
                '        xlWorkBook.Close(True, misValue, misValue)
                '        xlApp.Quit()

                '        releaseObject(xlWorkSheet)
                '        releaseObject(xlWorkBook)
                '        releaseObject(xlApp)

                '        MessageBox.Show("Excel file created, you can find the file " & FlName2, "ANNEXURE-I PREPARATION....", MessageBoxButtons.OKCancel)

                '    Catch ex As Exception
                '        MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)

                '    Finally
                '        releaseObject(xlWorkSheet)
                '        releaseObject(xlWorkBook)
                '        releaseObject(xlApp)

                '    End Try

                '    ''Dim xlApp As Excel.Application
                '    ''Dim xlWorkBook As Excel.Workbook
                '    ''Dim xlWorkSheet As Excel.Worksheet

                '    ''FlName1 = Trim(Common_Procedures.AppPath) & "\a1.xls"
                '    ''FlName2 = Trim(Common_Procedures.AppPath) & "\a2.xls"

                '    ''xlApp = New Excel.Application
                '    ''xlWorkBook = xlApp.Workbooks.Open("c:\a1.xls")
                '    ''xlWorkSheet = xlWorkBook.Worksheets("sheet1")
                '    '' ''display the cells value B2
                '    ' ''MsgBox(xlWorkSheet.Cells(2, 2).value)
                '    ' ''edit the cell with new value
                '    ''xlWorkSheet.Cells(2, 2) = "http://vb.net-informations.com"

                '    ''FlName = "c:\" & Trim(CompTinNo) & Format(Convert.ToDateTime(dtp_FromDate.Text), "MMyyyy").ToString & "A1.xls"

                '    ''xlWorkBook.SaveAs(FlName)

                '    ''xlWorkBook.Close()
                '    ''xlApp.Quit()

                '    ''releaseObject(xlApp)
                '    ''releaseObject(xlWorkBook)
                '    ''releaseObject(xlWorkSheet)

                '    ' '' ''Dim strFile As String = "c:\test.xls"
                '    ' '' ''Dim objProcess As New System.Diagnostics.ProcessStartInfo

                '    ' '' ''With objProcess
                '    ' '' ''    .FileName = strFile
                '    ' '' ''    .WindowStyle = ProcessWindowStyle.Hidden
                '    ' '' ''    .Verb = "print"

                '    ' '' ''    .CreateNoWindow = True
                '    ' '' ''    .UseShellExecute = True
                '    ' '' ''End With
                '    ' '' ''Try
                '    ' '' ''    System.Diagnostics.Process.Start(objProcess)
                '    ' '' ''Catch ex As Exception
                '    ' '' ''    MessageBox.Show(ex.Message)
                '    ' '' ''End Try

                'Else

                '    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                '    RpDs1.Name = "DataSet1"
                '    RpDs1.Value = Dtbl1

                '    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Annexure1.rdlc"

                '    RptViewer.LocalReport.DataSources.Clear()

                '    RptViewer.LocalReport.DataSources.Add(RpDs1)

                '    RptViewer.LocalReport.Refresh()
                '    RptViewer.RefreshReport()

                '    RptViewer.Visible = True

                'End If

                Case "gstr-2"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexurei-excel" Then
                        If cbo_Company.Visible = True Then
                            If Trim(cbo_Company.Text) = "" Then
                                MessageBox.Show("Invalid Company", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                If cbo_Company.Enabled Then cbo_Company.Focus()
                                Exit Sub
                            End If
                        End If
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    InpNm1 = "tZ.Company_ShortName"

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()




                    cmd.CommandText = "Insert into ReportTemp ( Name1,   Int1        ,   Name2      ,   meters1    ,        Date1   ,                 Name3                                                                                                                                                        ,   Name4  ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                 Name7                                              ,           Meters2                                ,   Meters3       ,       Meters4      ,Meters5            ,Meters6           ,  name9 ,Name8 ) " &
                                      " Select      a.Purchase_Code  , a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, (cast(DATEPART(dd, a.Purchase_Date) as varchar) + '/' + cast(DATEPART(mm, a.Purchase_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Purchase_Date) as varchar) ) as PurcDate, a.Bill_No, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when c.Ledger_GSTinNo = '' then 'Cash' else c.Ledger_GSTinNo end),  sum(b.Amount-b.Discount_Amount) as Ass_Value, b.GST_Percentage, sum(A.CGst_Amount) , SUM(A.SGst_Amount),SUM(A.IGst_Amount),    'R' ,(select top 1 z3.Commodity_Code from Purchase_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.Purchase_Code = a.Purchase_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) from Purchase_Head a INNER JOIN Purchase_Details b ON a.Company_IdNo = b.Company_IdNo and a.Purchase_Code = b.Purchase_Code INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.area_idno = d.area_idno Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Entry_GST_Tax_Type = 'GST' and  a.Purchase_Date between @fromdate and @todate  group by a.Purchase_Code, a.Company_IdNo, a.Purchase_No, a.for_OrderBy, a.Purchase_Date, a.Bill_No, c.Ledger_MainName, c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, c.Ledger_GSTinNo, d.Area_Name, b.GST_Percentage " 'having sum(b.Tax_Amount) <> 0 "
                    cmd.ExecuteNonQuery()





                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4,Meters5,Meters6,  name9  from reporttemp Order by Date1, meters1, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Truncate table ReportTemp"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4, name9  from reporttemp Order by Date1, meters1, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If


                'If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexurei-excel" Then

                '    Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()

                '    If xlApp Is Nothing Then
                '        MessageBox.Show("Excel is not properly installed!!", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)
                '        Return
                '        Exit Sub
                '    End If

                '    FlName1 = Trim(Common_Procedures.AppPath) & "\Images\a1.xls"
                '    If File.Exists(FlName1) = False Then
                '        MessageBox.Show("Invalid Master Annexure file " & Chr(13) & FlName1, "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)
                '        Return
                '        Exit Sub
                '    End If

                '    CompTinNo = Common_Procedures.get_FieldValue(con, "Company_Head", "Company_TinNo", "(Company_ShortName = '" & Trim(cbo_Company.Text) & "')")
                '    FlName2 = "c:\" & Trim(CompTinNo) & Format(Convert.ToDateTime(dtp_FromDate.Text), "MMyyyy").ToString & "A1.xls"
                '    If File.Exists(FlName2) = True Then
                '        File.Delete(FlName2)
                '    End If

                '    File.Copy(FlName1, FlName2, True)

                '    Dim xlWorkBook As Excel.Workbook
                '    Dim xlWorkSheet As Excel.Worksheet
                '    Dim misValue As Object = System.Reflection.Missing.Value

                '    xlWorkBook = xlApp.Workbooks.Open(FlName2)
                '    'xlWorkBook = xlApp.Workbooks.Add(misValue)
                '    xlWorkSheet = xlWorkBook.Sheets(1)

                '    Try

                '        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4,  name9  from reporttemp Order by Date1, meters1, Name2", con)
                '        Dtbl1 = New DataTable
                '        Da.Fill(Dtbl1)

                '        If Dtbl1.Rows.Count > 0 Then

                '            For i = 0 To Dtbl1.Rows.Count - 1
                '                xlWorkSheet.Cells(i + 2, 1) = Val(Val(i) + 1)
                '                xlWorkSheet.Cells(i + 2, 2) = Trim(Dtbl1.Rows(i).Item("Name5").ToString)
                '                xlWorkSheet.Cells(i + 2, 3) = Trim(Dtbl1.Rows(i).Item("Name7").ToString)
                '                xlWorkSheet.Cells(i + 2, 4) = Trim(Dtbl1.Rows(i).Item("Name8").ToString)
                '                xlWorkSheet.Cells(i + 2, 5) = Trim(Dtbl1.Rows(i).Item("Name4").ToString)
                '                xlWorkSheet.Cells(i + 2, 6) = Trim(Dtbl1.Rows(i).Item("Name3").ToString)
                '                xlWorkSheet.Cells(i + 2, 7) = Val(Dtbl1.Rows(i).Item("Meters2").ToString)
                '                xlWorkSheet.Cells(i + 2, 8) = Val(Dtbl1.Rows(i).Item("Meters3").ToString)
                '                xlWorkSheet.Cells(i + 2, 9) = Val(Dtbl1.Rows(i).Item("Meters4").ToString)
                '                xlWorkSheet.Cells(i + 2, 10) = Trim(Dtbl1.Rows(i).Item("Name9").ToString)
                '            Next

                '        End If

                '        xlWorkBook.Save()
                '        'xlWorkBook.SaveAs(FlName2, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue)
                '        xlWorkBook.Close(True, misValue, misValue)
                '        xlApp.Quit()

                '        releaseObject(xlWorkSheet)
                '        releaseObject(xlWorkBook)
                '        releaseObject(xlApp)

                '        MessageBox.Show("Excel file created, you can find the file " & FlName2, "ANNEXURE-I PREPARATION....", MessageBoxButtons.OKCancel)

                '    Catch ex As Exception
                '        MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)

                '    Finally
                '        releaseObject(xlWorkSheet)
                '        releaseObject(xlWorkBook)
                '        releaseObject(xlApp)

                '    End Try

                '    ''Dim xlApp As Excel.Application
                '    ''Dim xlWorkBook As Excel.Workbook
                '    ''Dim xlWorkSheet As Excel.Worksheet

                '    ''FlName1 = Trim(Common_Procedures.AppPath) & "\a1.xls"
                '    ''FlName2 = Trim(Common_Procedures.AppPath) & "\a2.xls"

                '    ''xlApp = New Excel.Application
                '    ''xlWorkBook = xlApp.Workbooks.Open("c:\a1.xls")
                '    ''xlWorkSheet = xlWorkBook.Worksheets("sheet1")
                '    '' ''display the cells value B2
                '    ' ''MsgBox(xlWorkSheet.Cells(2, 2).value)
                '    ' ''edit the cell with new value
                '    ''xlWorkSheet.Cells(2, 2) = "http://vb.net-informations.com"

                '    ''FlName = "c:\" & Trim(CompTinNo) & Format(Convert.ToDateTime(dtp_FromDate.Text), "MMyyyy").ToString & "A1.xls"

                '    ''xlWorkBook.SaveAs(FlName)

                '    ''xlWorkBook.Close()
                '    ''xlApp.Quit()

                '    ''releaseObject(xlApp)
                '    ''releaseObject(xlWorkBook)
                '    ''releaseObject(xlWorkSheet)

                '    ' '' ''Dim strFile As String = "c:\test.xls"
                '    ' '' ''Dim objProcess As New System.Diagnostics.ProcessStartInfo

                '    ' '' ''With objProcess
                '    ' '' ''    .FileName = strFile
                '    ' '' ''    .WindowStyle = ProcessWindowStyle.Hidden
                '    ' '' ''    .Verb = "print"

                '    ' '' ''    .CreateNoWindow = True
                '    ' '' ''    .UseShellExecute = True
                '    ' '' ''End With
                '    ' '' ''Try
                '    ' '' ''    System.Diagnostics.Process.Start(objProcess)
                '    ' '' ''Catch ex As Exception
                '    ' '' ''    MessageBox.Show(ex.Message)
                '    ' '' ''End Try

                'Else

                '    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                '    RpDs1.Name = "DataSet1"
                '    RpDs1.Value = Dtbl1

                '    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_GSTR_2.rdlc"

                '    RptViewer.LocalReport.DataSources.Clear()

                '    RptViewer.LocalReport.DataSources.Add(RpDs1)

                '    RptViewer.LocalReport.Refresh()
                '    RptViewer.RefreshReport()

                '    RptViewer.Visible = True

                'End If



                Case "annexure-ii", "annexureii-excel", "annexure-ii audit"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexureii-excel" Then
                        If cbo_Company.Visible = True Then
                            If Trim(cbo_Company.Text) = "" Then
                                MessageBox.Show("Invalid Company", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                If cbo_Company.Enabled Then cbo_Company.Focus()
                                Exit Sub
                            End If
                        End If
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    InpNm1 = "tZ.Company_ShortName"

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()



                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexure-ii audit" Then  'Alphonsa

                        cmd.CommandText = "Insert into ReportTemp ( Name1 ,   Int1        ,   Name2   ,   meters1    ,       Date1 ,                        Name3                                                                                                                                      ,   Name5                                                                                     ,               Name6                                                                                                                                                                                       ,                                 Name7                              ,    Meters2            ,   Meters3 ,   Meters4                                                                           ,  name9,  name8  ) " &
                                               " Select  a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date,   (cast(DATEPART(dd, a.Sales_Date) as varchar) + '/' + cast(DATEPART(mm, a.Sales_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Sales_Date) as varchar) ) as SaleDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end), a.Actual_Gross_Amount , a.Tax_Perc, (CASE WHEN a.Tax_Perc <> 0 THEN a.Actual_Gross_Amount * a.Tax_Perc/100  ELSE 0 END ),     'F', (select top 1 z3.Commodity_Code from Sales_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.Sales_Code = a.Sales_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) as Comm_Code from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate AND a.Tax_Perc <> 0"
                        cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Insert into ReportTemp ( Name1       ,   Int1        ,   Name2         ,   meters1    ,   Date1           ,                      Name3                                                                                                                                                               ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                                 Name7                              ,        Meters2               ,   Meters3 ,      Meters4                                                                             ,  name9  , Name8  ) " & _
                        '                            " Select  a.SalesReturn_Code, a.Company_IdNo, a.SalesReturn_No, a.for_OrderBy, a.SalesReturn_Date, (cast(DATEPART(dd, a.SalesReturn_Date) as varchar) + '/' + cast(DATEPART(mm, a.SalesReturn_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.SalesReturn_Date) as varchar) ) as SaleRetDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end),     -1*a.Actual_Gross_Amount, a.Tax_Perc, (CASE WHEN a.Tax_Perc <> 0 THEN  -1*(a.Actual_Gross_Amount * a.Tax_Perc/100)  ELSE 0 END ),    'R'  ,(select top 1 z3.Commodity_Code from SalesReturn_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.SalesReturn_Code = a.SalesReturn_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) from SalesReturn_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.SalesReturn_Date between @fromdate and @todate AND a.Tax_Perc <> 0 "
                        'cmd.ExecuteNonQuery()

                    Else
                        cmd.CommandText = "Insert into ReportTemp ( Name1 ,   Int1        ,   Name2   ,   meters1    ,       Date1 ,                        Name3                                                                                                                                                   ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                                 Name7                     ,    Meters2         ,   Meters3 ,   Meters4   ,  name9,  name8  ) " &
                                               " Select  a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date,   (cast(DATEPART(dd, a.Sales_Date) as varchar) + '/' + cast(DATEPART(mm, a.Sales_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Sales_Date) as varchar) ) as SaleDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end), a.Assessable_Value , a.Tax_Perc, a.Tax_Amount,     'F', (select top 1 z3.Commodity_Code from Sales_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.Sales_Code = a.Sales_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) as Comm_Code from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate and a.Tax_Amount <> 0 "
                        cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Insert into ReportTemp ( Name1 ,   Int1        ,   Name2   ,   meters1    ,       Date1 ,                        Name3                                                                                                                                                   ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                                 Name7                              ,  Name8  ,   Meters2         ,   Meters3 ,   Meters4   ,  name9  ) " & _
                        '                            " Select  a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date,   (cast(DATEPART(dd, a.Sales_Date) as varchar) + '/' + cast(DATEPART(mm, a.Sales_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Sales_Date) as varchar) ) as SaleDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end),    '780', a.Assessable_Value, a.Tax_Perc, a.Tax_Amount,     'F'     from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate and a.Tax_Amount <> 0 "
                        'cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into ReportTemp ( Name1       ,   Int1        ,   Name2         ,   meters1    ,   Date1           ,                      Name3                                                                                                                                                               ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                                 Name7                              ,        Meters2         ,   Meters3 ,      Meters4   ,  name9  , Name8  ) " &
                                                    " Select  a.SalesReturn_Code, a.Company_IdNo, a.SalesReturn_No, a.for_OrderBy, a.SalesReturn_Date, (cast(DATEPART(dd, a.SalesReturn_Date) as varchar) + '/' + cast(DATEPART(mm, a.SalesReturn_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.SalesReturn_Date) as varchar) ) as SaleRetDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end),     -1*a.Assessable_Value, a.Tax_Perc, -1*a.Tax_Amount,    'R' ,(select top 1 z3.Commodity_Code from SalesReturn_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.SalesReturn_Code = a.SalesReturn_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) from SalesReturn_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.SalesReturn_Date between @fromdate and @todate and a.Tax_Amount <> 0 "
                        cmd.ExecuteNonQuery()
                        'cmd.CommandText = "Insert into ReportTemp ( Name1 ,   Int1        ,   Name2   ,   meters1    ,       Date1 ,  Name3,   Name5      ,                Name6                                                                                                                                                                                       ,                                 Name7                                 ,  Name8,   Meters2         ,   Meters3 ,   Meters4   ,  name9  ) " & _
                        '                            " Select  a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date,    '' , c.Ledger_MainName,   (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_TinNo = '' then 'Cash' else c.Ledger_TinNo end),    '780' , a.Assessable_Value, a.Tax_Perc, a.Tax_Amount,     'F'     from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate and a.Tax_Amount <> 0 "
                        'cmd.ExecuteNonQuery()


                    End If
                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4, name9  from reporttemp Order by Date1, Meters1, Name2 ", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Truncate table ReportTemp"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4, name9  from reporttemp Order by Date1, Meters1, Name2 ", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If


                'If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexureii-excel" Then

                '    Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()

                '    If xlApp Is Nothing Then
                '        MessageBox.Show("Excel is not properly installed!!", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)
                '        Return
                '        Exit Sub
                '    End If

                '    FlName1 = Trim(Common_Procedures.AppPath) & "\Images\A2.xls"
                '    If File.Exists(FlName1) = False Then
                '        MessageBox.Show("Invalid Master Annexure file " & Chr(13) & FlName1, "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)
                '        Return
                '        Exit Sub
                '    End If

                '    CompTinNo = Common_Procedures.get_FieldValue(con, "Company_Head", "Company_TinNo", "(Company_ShortName = '" & Trim(cbo_Company.Text) & "')")
                '    FlName2 = "c:\" & Trim(CompTinNo) & Format(Convert.ToDateTime(dtp_FromDate.Text), "MMyyyy").ToString & "A2.xls"
                '    If File.Exists(FlName2) = True Then
                '        File.Delete(FlName2)
                '    End If

                '    File.Copy(FlName1, FlName2, True)

                '    Dim xlWorkBook As Excel.Workbook
                '    Dim xlWorkSheet As Excel.Worksheet
                '    Dim misValue As Object = System.Reflection.Missing.Value

                '    xlWorkBook = xlApp.Workbooks.Open(FlName2)
                '    'xlWorkBook = xlApp.Workbooks.Add(misValue)
                '    xlWorkSheet = xlWorkBook.Sheets(1)

                '    Try

                '        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4,  name9  from reporttemp Order by Date1, meters1, Name2", con)
                '        Dtbl1 = New DataTable
                '        Da.Fill(Dtbl1)

                '        If Dtbl1.Rows.Count > 0 Then

                '            For i = 0 To Dtbl1.Rows.Count - 1
                '                xlWorkSheet.Cells(i + 2, 1) = Val(Val(i) + 1)
                '                xlWorkSheet.Cells(i + 2, 2) = Trim(Dtbl1.Rows(i).Item("Name5").ToString)
                '                xlWorkSheet.Cells(i + 2, 3) = Trim(Dtbl1.Rows(i).Item("Name7").ToString)
                '                xlWorkSheet.Cells(i + 2, 4) = Trim(Dtbl1.Rows(i).Item("Name8").ToString)
                '                xlWorkSheet.Cells(i + 2, 5) = Trim(Dtbl1.Rows(i).Item("Name2").ToString)
                '                xlWorkSheet.Cells(i + 2, 6) = Trim(Dtbl1.Rows(i).Item("Name3").ToString)
                '                xlWorkSheet.Cells(i + 2, 7) = Val(Dtbl1.Rows(i).Item("Meters2").ToString)
                '                xlWorkSheet.Cells(i + 2, 8) = Val(Dtbl1.Rows(i).Item("Meters3").ToString)
                '                xlWorkSheet.Cells(i + 2, 9) = Val(Dtbl1.Rows(i).Item("Meters4").ToString)
                '                xlWorkSheet.Cells(i + 2, 10) = Trim(Dtbl1.Rows(i).Item("Name9").ToString)
                '            Next

                '        End If

                '        xlWorkBook.Save()
                '        'xlWorkBook.SaveAs(FlName2, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue)
                '        xlWorkBook.Close(True, misValue, misValue)
                '        xlApp.Quit()

                '        releaseObject(xlWorkSheet)
                '        releaseObject(xlWorkBook)
                '        releaseObject(xlApp)

                '        MessageBox.Show("Excel file created, you can find the file " & FlName2, "ANNEXURE-II PREPARATION....", MessageBoxButtons.OKCancel)

                '    Catch ex As Exception
                '        MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)

                '    Finally
                '        releaseObject(xlWorkSheet)
                '        releaseObject(xlWorkBook)
                '        releaseObject(xlApp)

                '    End Try


                'Else

                '    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                '    RpDs1.Name = "DataSet1"
                '    RpDs1.Value = Dtbl1

                '    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Annexure2.rdlc"

                '    RptViewer.LocalReport.DataSources.Clear()

                '    RptViewer.LocalReport.DataSources.Add(RpDs1)

                '    RptViewer.LocalReport.Refresh()
                '    RptViewer.RefreshReport()

                '    RptViewer.Visible = True

                'End If

                Case "gstr-1"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexureii-excel" Then
                        If cbo_Company.Visible = True Then
                            If Trim(cbo_Company.Text) = "" Then
                                MessageBox.Show("Invalid Company", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                If cbo_Company.Enabled Then cbo_Company.Focus()
                                Exit Sub
                            End If
                        End If
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    InpNm1 = "tZ.Company_ShortName"

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()



                    cmd.CommandText = "Insert into ReportTemp ( Name1 ,   Int1        ,   Name2   ,   meters1    ,       Date1 ,                        Name3                                                                                                                                     ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                                 Name7                     ,    Meters2                      ,   Meters3                                                                                             ,   Meters4    ,Meters5       ,Meters6         , name9,  name8  ) " &
                                           " Select  a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date,   (cast(DATEPART(dd, a.Sales_Date) as varchar) + '/' + cast(DATEPART(mm, a.Sales_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Sales_Date) as varchar) ) as SaleDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_GSTinNo = '' then 'Cash' else c.Ledger_GSTinNo end), a.Assessable_Value ,  (select top 1 z2.Tax_Perc from Sales_Details z2 where z2.Sales_Code = a.Sales_Code ) as TAX_PERCNTAGE, a.CGst_Amount,a.SGst_Amount ,a.IGst_Amount   ,  'F', (select top 1 z3.Commodity_Code from Sales_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.Sales_Code = a.Sales_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) as Comm_Code from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate " 'and (a.CGst_Amount <> 0 and a.SGst_Amount <> 0 and a.IGst_Amount <> 0)"
                    cmd.ExecuteNonQuery()

                    'cmd.CommandText = "Insert into ReportTemp (              Name1 ,   Int1        ,   Name2      ,   meters1    ,       Date1  ,                  Name3                                                                                                                                                ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                                 Name7                                  ,    Meters2          ,   Meters3  ,   Meters4                                                                                                                             ,Meters5                                                                                                                                ,Meters6                                                                                                                               , name9,  name8  ) " & _
                    '                                        " Select  a.Sales_Code , a.Company_IdNo, a.Sales_No   , a.for_OrderBy, a.Sales_Date ,   (cast(DATEPART(dd, a.Sales_Date) as varchar) + '/' + cast(DATEPART(mm, a.Sales_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Sales_Date) as varchar) ) as SaleDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_GSTinNo = '' then 'Cash' else c.Ledger_GSTinNo end), sd.Assessable_Value , sd.Tax_Perc, (case when a.CGst_Amount <> 0 then (case when sd.Tax_Perc <> 0 then (sd.Assessable_Value*sd.Tax_Perc /100)/2 else 0 end ) else 0 end ),(case when a.SGst_Amount <> 0 then (case when sd.Tax_Perc <> 0 then (sd.Assessable_Value*sd.Tax_Perc /100)/2 else 0 end ) else 0 end ) ,(case when a.IGst_Amount <> 0 then (case when sd.Tax_Perc <> 0 then (sd.Assessable_Value*sd.Tax_Perc /100) else 0 end ) else 0 end ) ,  'F' , (select top 1 z3.Commodity_Code from Sales_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.Sales_Code = a.Sales_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) as Comm_Code from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo LEFT OUTER JOIN Sales_Details SD ON a.Sales_Code =SD.Sales_Code Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate and a.Entry_Vat_Gst_Type = 'GST' " 'and (a.CGst_Amount <> 0 and a.SGst_Amount <> 0 and a.IGst_Amount <> 0)"
                    'cmd.ExecuteNonQuery()


                    'cmd.CommandText = "Insert into ReportTemp (              Name1 ,   Int1        ,   Name2      ,   meters1    ,       Date1  ,                  Name3                                                                                                                                                ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                                 Name7                                  ,    Meters2               ,   Meters3  ,   Meters4                                                                                                                             ,Meters5                                                                                                                                ,Meters6                                                                                                                               , name9,  name8  ) " & _
                    '                                        " Select  a.Sales_Code , a.Company_IdNo, a.Sales_No   , a.for_OrderBy, a.Sales_Date ,   (cast(DATEPART(dd, a.Sales_Date) as varchar) + '/' + cast(DATEPART(mm, a.Sales_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.Sales_Date) as varchar) ) as SaleDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_GSTinNo = '' then 'Cash' else c.Ledger_GSTinNo end), sum(sd.Assessable_Value) , sd.Tax_Perc, (case when sum(a.CGst_Amount) <> 0 then (case when sd.Tax_Perc <> 0 then (sum(sd.Assessable_Value)*sd.Tax_Perc /100)/2 else 0 end ) else 0 end ),(case when sum(a.SGst_Amount) <> 0 then (case when sd.Tax_Perc <> 0 then (sum(sd.Assessable_Value)*sd.Tax_Perc /100)/2 else 0 end ) else 0 end ) ,(case when sum(a.IGst_Amount) <> 0 then (case when sd.Tax_Perc <> 0 then (sum(sd.Assessable_Value)*sd.Tax_Perc /100) else 0 end ) else 0 end ) ,  'F' , (select top 1 z3.Commodity_Code from Sales_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.Sales_Code = a.Sales_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) as Comm_Code from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo LEFT OUTER JOIN Sales_Details SD ON a.Sales_Code =SD.Sales_Code Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate and a.Entry_Vat_Gst_Type = 'GST'  GROUP BY a.Sales_Code , a.Company_IdNo, a.Sales_No   , a.for_OrderBy,a.Sales_Date, c.Ledger_MainName,d.AREA_NAME,c.Ledger_Address4,c.Ledger_Address3,c.Ledger_Address2,c.Ledger_Address1, C.Ledger_GSTinNo,sd.Tax_Perc "
                    'cmd.ExecuteNonQuery()

                    '-------Sales Return
                    cmd.CommandText = "Insert into ReportTemp ( Name1       ,   Int1        ,   Name2         ,   meters1    ,   Date1           ,                      Name3                                                                                                                                                               ,   Name5                                                                                      ,               Name6                                                                                                                                                                                       ,                                 Name7                              ,        Meters2         ,   Meters3 ,      Meters4   ,  name9  , Name8  ) " &
                                                " Select  a.SalesReturn_Code, a.Company_IdNo, a.SalesReturn_No, a.for_OrderBy, a.SalesReturn_Date, (cast(DATEPART(dd, a.SalesReturn_Date) as varchar) + '/' + cast(DATEPART(mm, a.SalesReturn_Date) as varchar) + '/' + cast(DATEPART(yyyy, a.SalesReturn_Date) as varchar) ) as SaleRetDate, c.Ledger_MainName + ( CASE WHEN d.area_name IS NOT NULL THEN ', ' + d.AREA_NAME ELSE '' END ),  (case when c.Ledger_Address4 <> '' then c.Ledger_Address4 when c.Ledger_Address3 <> '' then c.Ledger_Address3 when c.Ledger_Address2 <> '' then c.Ledger_Address2 else c.Ledger_Address1 end) as addresss, (case when C.Ledger_GSTinNo = '' then 'Cash' else c.Ledger_GSTinNo end),     -1*a.Assessable_Value, a.Tax_Perc, -1*a.Tax_Amount,    'R' ,(select top 1 z3.Commodity_Code from SalesReturn_Details z1, Item_Head z2, ItemGroup_Head z3 where z1.SalesReturn_Code = a.SalesReturn_Code and z1.Item_IdNo = z2.Item_IdNo and z2.Itemgroup_idno = z3.itemgroup_idno order by z1.sl_no ) from SalesReturn_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Area_Head d ON c.Area_IdNo = d.Area_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.SalesReturn_Date between @fromdate and @todate and a.Tax_Amount <> 0 and a.Tax_Type <> 'VAT' and a.Tax_Type <> '' "
                    cmd.ExecuteNonQuery()




                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4,Meters5       ,Meters6  ,  name9  from reporttemp Order by Date1, Meters1, Name2 ", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Truncate table ReportTemp"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4, name9  from reporttemp Order by Date1, Meters1, Name2 ", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If


                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "annexureii-excel" Then

                        'Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()

                        'If xlApp Is Nothing Then
                        '    MessageBox.Show("Excel is not properly installed!!", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)
                        '    Return
                        '    Exit Sub
                        'End If

                        'FlName1 = Trim(Common_Procedures.AppPath) & "\Images\A2.xls"
                        'If File.Exists(FlName1) = False Then
                        '    MessageBox.Show("Invalid Master Annexure file " & Chr(13) & FlName1, "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)
                        '    Return
                        '    Exit Sub
                        'End If

                        'CompTinNo = Common_Procedures.get_FieldValue(con, "Company_Head", "Company_TinNo", "(Company_ShortName = '" & Trim(cbo_Company.Text) & "')")
                        'FlName2 = "c:\" & Trim(CompTinNo) & Format(Convert.ToDateTime(dtp_FromDate.Text), "MMyyyy").ToString & "A2.xls"
                        'If File.Exists(FlName2) = True Then
                        '    File.Delete(FlName2)
                        'End If

                        'File.Copy(FlName1, FlName2, True)

                        'Dim xlWorkBook As Excel.Workbook
                        'Dim xlWorkSheet As Excel.Worksheet
                        'Dim misValue As Object = System.Reflection.Missing.Value

                        'xlWorkBook = xlApp.Workbooks.Open(FlName2)
                        ''xlWorkBook = xlApp.Workbooks.Add(misValue)
                        'xlWorkSheet = xlWorkBook.Sheets(1)

                        'Try

                        '    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Name4, Name5, Name6, Name7, Name8, Meters2, Meters3, Meters4,  name9  from reporttemp Order by Date1, meters1, Name2", con)
                        '    Dtbl1 = New DataTable
                        '    Da.Fill(Dtbl1)

                        '    If Dtbl1.Rows.Count > 0 Then

                        '        For i = 0 To Dtbl1.Rows.Count - 1
                        '            xlWorkSheet.Cells(i + 2, 1) = Val(Val(i) + 1)
                        '            xlWorkSheet.Cells(i + 2, 2) = Trim(Dtbl1.Rows(i).Item("Name5").ToString)
                        '            xlWorkSheet.Cells(i + 2, 3) = Trim(Dtbl1.Rows(i).Item("Name7").ToString)
                        '            xlWorkSheet.Cells(i + 2, 4) = Trim(Dtbl1.Rows(i).Item("Name8").ToString)
                        '            xlWorkSheet.Cells(i + 2, 5) = Trim(Dtbl1.Rows(i).Item("Name2").ToString)
                        '            xlWorkSheet.Cells(i + 2, 6) = Trim(Dtbl1.Rows(i).Item("Name3").ToString)
                        '            xlWorkSheet.Cells(i + 2, 7) = Val(Dtbl1.Rows(i).Item("Meters2").ToString)
                        '            xlWorkSheet.Cells(i + 2, 8) = Val(Dtbl1.Rows(i).Item("Meters3").ToString)
                        '            xlWorkSheet.Cells(i + 2, 9) = Val(Dtbl1.Rows(i).Item("Meters4").ToString)
                        '            xlWorkSheet.Cells(i + 2, 10) = Trim(Dtbl1.Rows(i).Item("Name9").ToString)
                        '        Next

                        '    End If

                        '    xlWorkBook.Save()
                        '    'xlWorkBook.SaveAs(FlName2, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue)
                        '    xlWorkBook.Close(True, misValue, misValue)
                        '    xlApp.Quit()

                        '    releaseObject(xlWorkSheet)
                        '    releaseObject(xlWorkBook)
                        '    releaseObject(xlApp)

                        '    MessageBox.Show("Excel file created, you can find the file " & FlName2, "ANNEXURE-II PREPARATION....", MessageBoxButtons.OKCancel)

                        'Catch ex As Exception
                        '    MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT...", MessageBoxButtons.OK)

                        'Finally
                        '    releaseObject(xlWorkSheet)
                        '    releaseObject(xlWorkBook)
                        '    releaseObject(xlApp)

                        'End Try


                    Else

                        RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                        RpDs1.Name = "DataSet1"
                        RpDs1.Value = Dtbl1

                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_GSTR_1.rdlc"

                        RptViewer.LocalReport.DataSources.Clear()

                        RptViewer.LocalReport.DataSources.Add(RpDs1)

                        RptViewer.LocalReport.Refresh()
                        RptViewer.RefreshReport()

                        RptViewer.Visible = True

                    End If


                Case "item inward and outward register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con


                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTempSub (    Int1,     Weight1   )   " &
                                        "   Select             a.Item_IdNo, sum(a.Noof_Items) from Purchase_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Purchase_Date between @fromdate and @todate group by a.Item_IdNo"
                    cmd.ExecuteNonQuery()


                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1085" Then



                        cmd.CommandText = "Insert into ReportTempSub (    Int1,     Weight2   )   " &
                                            "   Select             a.Item_IdNo, sum(a.Noof_Items+a.Extra_Quantity) from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate group by a.Item_IdNo"
                        cmd.ExecuteNonQuery()
                    Else

                        cmd.CommandText = "Insert into ReportTempSub (    Int1,     Weight2   )   " &
                                            "   Select             a.Item_IdNo, sum(a.Noof_Items) from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate group by a.Item_IdNo"
                        cmd.ExecuteNonQuery()
                    End If


                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp (    Name1,       Weight1 ,   Meters1  ,        Currency1                           ,       Weight2 ,   Meters2   ,        Currency2                             ) " &
                                        " Select             b.Item_Name, sum(a.Weight1), b.Cost_Rate, (sum(a.Weight1) * b.Cost_Rate) as PurcValue, sum(a.Weight2), b.Sales_Rate, (sum(a.Weight2) * b.Sales_Rate) as SaleValue from ReportTempSub a INNER JOIN Item_Head b ON a.Int1 = b.Item_IdNo group by b.Item_Name, b.Cost_Rate, b.Sales_Rate "
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Weight1, Meters1,  Currency1, Weight2, Meters2, Currency2 from reporttemp where Weight1 <> 0 or Weight2 <> 0 Order by Name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Weight1, Meters1,  Currency1, Weight2, Meters2, Currency2 from reporttemp Order by Name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Item_Inward_OutWard_Register.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "jobwork entry details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Name1,   Int1        ,   Name2      ,   Weight10   ,   Date1        ,   Name3      ,   Name4       , Name5  ,Weight1    ) " &
                                        " Select     a.JobWork_Code, a.Company_IdNo, a.Jobwork_No, a.for_OrderBy, a.Jobwork_Date, b.Ledger_Name, c.Item_Name, d.Size_Name ,a. Quantity     from JobWork_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Item_Head c ON a.Item_IdNo = c.Item_IdNo  LEFT OUTER JOIN  Size_Head d ON d.Size_IdNo = a.Size_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.JobWork_Date between @fromdate and @todate Order by a.jobWork_Date, a.for_OrderBy, a.JobWork_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, Weight1  from reporttemp Order by Date1, Weight10   ,   name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, Weight1  from reporttemp Order by Date1, Weight10   ,   name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_JobWork_Entry_Details.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "knotting entry details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " (a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))) & " or a.Knotting_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))) & ")"
                    End If
                    'If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Shift = '" & Trim(cbo_temName.Text)))
                    'End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3        ,   Name1        ,   Int1        ,   Name2      ,   Weight10   ,   date1      ,   Name3  ,   Name4 , Name5 ,int2, int3  ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Knotting_Code, a.Company_IdNo, a.Knotting_No, a.for_OrderBy, a.Knotting_Date, b.Ledger_Name, a.Shift, a.Loom ,a.Ends  , No_Pavu     from Knotting_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Date between @fromdate and @todate Order by a.Knotting_Date, a.for_OrderBy, a.Knotting_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3 from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Knotting_Entry_Details.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True



                Case "knotting summary"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " (a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))) & " or a.Knotting_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))) & ")"
                    End If
                    'If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Shift = '" & Trim(cbo_temName.Text)))
                    'End If

                    cmd.CommandText = "Insert into ReportTemp( Name3  ,   Name4 , int3  ) " &
                                        " Select         b.Ledger_Name, a.Shift, sum(a.No_Pavu )    from Knotting_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Date between @fromdate and @todate group by b.Ledger_Name, a.Shift having sum(a.No_Pavu) <> 0"
                    cmd.ExecuteNonQuery()

                    'cmd.CommandText = "Insert into ReportTemp( Name3  ,   Name4 , Name5 ,int2, int3  ) " & _
                    '                    " Select         b.Ledger_Name, a.Shift, a.Loom ,sum(a.Ends)  , sum(a.No_Pavu )    from Knotting_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Date between @fromdate and @todate group by b.Ledger_Name, a.Shift, a.Loom having sum(a.Ends) <> 0  and sum(a.No_Pavu) <> 0"
                    'cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name3,Name4,Name5, int2, int3 from reporttemp where int3 <> 0 Order by Name3, Name4", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name3,Name4,Name5, int2, int3 from reporttemp Order by Name3, Name4", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Knotting_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True




                Case "knotting bill summary"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    'RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    'RptHeading3 = ""

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    'If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    'End If

                    cmd.CommandText = "Insert into ReportTemp (    Name2 ,        int2  ,        Weight1 ) " &
                                        " Select             b.Ledger_Name, sum(a.Total_Pavu),sum(a.Net_Amount) from Knotting_Bill_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Bill_Date between @fromdate and @todate group by b.Ledger_Name having sum(a.Total_pavu) <> 0 and sum(a.Net_Amount) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name2,int2, Weight1 from reporttemp where Weight1 <> 0 Order by Name1, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name2,int2, Weight1 from reporttemp Order by Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Knotting_Bill_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True



                Case "knotting bill details", "knotting bill register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    'If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Shift = '" & Trim(cbo_temName.Text)))
                    'End If


                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "knotting bill register" Then
                        cmd.CommandText = "Insert into ReportTemp(Name1   ,       Int1    ,   Name2           ,    Weight10   ,    date1        ,           Name3  ,    int3      ,   Weight1,     weight3, weight2 ) " &
                                            " Select  a.Knotting_Bill_Code, a.Company_IdNo, a.Knotting_Bill_No, a.for_OrderBy, a.Knotting_Bill_Date, c.Ledger_Name,   a.Total_Pavu, a.Rate , a.AddLess_Amount, a.Net_Amount  from Knotting_Bill_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Bill_Date between @fromdate and @todate Order by a.Knotting_Bill_Date, a.for_OrderBy, a.Knotting_Bill_No, a.Company_IdNo"
                        cmd.ExecuteNonQuery()

                    Else

                        cmd.CommandText = "Insert into ReportTemp(Name1        ,       Int1        ,   Name2      ,    Weight10   ,    date1        ,           Name3  ,  Name4  ,          date2        ,   Name5,   Name6,   int2   , int3      , Weight1,     weight3, weight2, int4      , Currency1, Currency2) " &
                                            " Select  a.Knotting_Bill_Code, a.Company_IdNo, a.Knotting_Bill_No, a.for_OrderBy, a.Knotting_Bill_Date, c.Ledger_Name, b.Knotting_No , b.Knotting_Date, b.Shift, b.Loom , b.No_Pavu, a.Total_Pavu, a.Rate ,  a.AddLess_Amount, a.Net_Amount, 0, 0, 0  from Knotting_Bill_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Knotting_Bill_Details b ON a.Knotting_Bill_Code = b.Knotting_Bill_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Bill_Date between @fromdate and @todate Order by a.Knotting_Bill_Date, a.for_OrderBy, a.Knotting_Bill_No, a.Company_IdNo"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Select sum(z.Total_Pavu), sum(z.Net_Amount), sum(z.AddLess_Amount) from Knotting_Bill_Head z where z.Knotting_Bill_Code IN ( Select a.Knotting_Bill_Code from Knotting_Bill_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Knotting_Bill_Details b ON a.Knotting_Bill_Code = b.Knotting_Bill_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Bill_Date between @fromdate and @todate ) "
                        Da = New SqlClient.SqlDataAdapter(cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)

                        If Dt.Rows.Count > 0 Then
                            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                                cmd.CommandText = "Update ReportTemp set Int4 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                                cmd.ExecuteNonQuery()
                            End If
                            If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                                cmd.CommandText = "Update ReportTemp set Currency1 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                                cmd.ExecuteNonQuery()
                            End If
                            If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                                cmd.CommandText = "Update ReportTemp set Currency2 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                                cmd.ExecuteNonQuery()
                            End If
                        End If
                        Dt.Clear()

                    End If

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name1, Int1, Name2, Weight10, Date1, Name3, Name4,date2        ,   Name5,Name6 , int2, int3, Weight1,Weight2, Weight3, int4, Currency1, Currency2 from reporttemp Order by Date1, Weight10   ,name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()
                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,date2        ,   Name5,Name6 , int2, int3, Weight1,Weight2, Weight3, int4, Currency1, Currency2 from reporttemp Order by Date1, Weight10   ,name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)


                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "knotting bill register" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Knotting_Bill_Register.rdlc"
                    Else
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Knotting_Bill_Details.rdlc"
                    End If


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "knotting bill pending register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " (a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))) & " or a.Knotting_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))) & ")"
                    End If
                    'If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Shift = '" & Trim(cbo_temName.Text)))
                    'End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3        ,       Name1        ,       Int1        ,   Name2      ,    Weight10   ,    date2             ,           Name3  ,          Name5    ,   Name6  ,   int2  ,   int3   ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Knotting_Code    , a.Company_IdNo    , a.Knotting_No, a.for_OrderBy , a.Knotting_Date       , c.Ledger_Name    ,     A.Shift      , A.Loom   , A.Ends  , A.No_Pavu   from Knotting_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Date between @fromdate and @todate AND Knotting_Bill_Code = '' Order by a.Knotting_Date, a.for_OrderBy, a.Knotting_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name1        ,       Int1        ,   Name2      ,    Weight10   ,    date2    ,    Name3  ,          Name5    ,   Name6  ,   int2  ,   int3  from reporttemp Order by Date1,    Weight10   , name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()
                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name1, Int1, Name2, Weight10, Date1,Date3, Name3, Name4,Name5,Name6 , int2,int3 from reporttemp Order by Date1,    Weight10   , name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)


                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Knotting_Bill_Pending_Register.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "knotting bill pending summary"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " (a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))) & " or a.Knotting_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))) & ")"
                    End If

                    'If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    'End If

                    cmd.CommandText = "Insert into ReportTemp (    Name2 ,        int2   ) " &
                                        " Select             b.Ledger_Name, sum(a.No_Pavu) from Knotting_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Knotting_Date between @fromdate and @todate and Knotting_Bill_Code = '' group by b.Ledger_Name having sum(a.No_Pavu) <> 0 "
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name2,int2 from reporttemp where int2 <> 0 Order by Name1, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name2,int2, Weight1 from reporttemp Order by Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Knotting_Bill_Pending_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "cloth sales register", "cloth sales register - agentwise", "cloth sales register - transportwise", "cloth sales register - partyfromwise", "cloth sales register - partytowise"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then

                        Cnt_IdNo = Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text))

                        Cnt_UndIdNo = Val(Cnt_IdNo)

                        Da = New SqlClient.SqlDataAdapter("select * from Ledger_Head where Ledger_idno = " & Str(Val(Cnt_UndIdNo)), con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            If IsDBNull(Dt1.Rows(0).Item("LedgerGroup_Idno").ToString) = False Then
                                If Val(Dt1.Rows(0).Item("LedgerGroup_Idno").ToString) <> 0 Then Cnt_UndIdNo = Val(Dt1.Rows(0).Item("LedgerGroup_Idno").ToString)
                            End If
                        End If
                        Dt1.Clear()

                        Da = New SqlClient.SqlDataAdapter("select * from Ledger_Head where LedgerGroup_Idno = " & Str(Val(Cnt_UndIdNo)), con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)

                        Cnt_GrpIdNos = ""
                        If Dt1.Rows.Count > 0 Then
                            For i = 0 To Dt1.Rows.Count - 1
                                Cnt_GrpIdNos = Trim(Cnt_GrpIdNos) & IIf(Trim(Cnt_GrpIdNos) <> "", ", ", "") & Trim(Val(Dt1.Rows(i).Item("Ledger_idno")))
                            Next
                        End If
                        If Trim(Cnt_GrpIdNos) <> "" Then
                            Cnt_GrpIdNos = "(" & Cnt_GrpIdNos & ")"
                        Else
                            Cnt_GrpIdNos = "(" & Trim(Val(Cnt_IdNo)) & ")"
                        End If

                        If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "cloth sales register - partyfromwise" Then
                            Cnt_Cond = "(a.Ledger_idno = " & Str(Cnt_IdNo) & " or a.Ledger_idno IN " & Trim(Cnt_GrpIdNos) & ")"

                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & Cnt_Cond
                        Else
                            Cnt_Cond = "(a.Ledger_IdNo1 = " & Str(Cnt_IdNo) & " or a.Ledger_IdNo1 IN " & Trim(Cnt_GrpIdNos) & ")"

                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & Cnt_Cond

                            'RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo1 = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                        End If
                    End If

                    If cbo_Agent.Visible = True And Trim(cbo_Agent.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Agent_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)))
                    End If
                    If cbo_Transport.Visible = True And Trim(cbo_Transport.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Transport_IdNo = " & Str(Val(Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)))
                    End If


                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp(Name1 ,   Int1        ,   Name2         ,    Weight10   ,          date1             ,     Name3  ,             Name4                  ,          Name5                ,   weight1       ,  Weight2   ,   Weight3     ,   weight4    , Meters2       ,  Name6            , Name7   , Name8 ) " &
                                        " Select  a.Cloth_Sales_Code, a.Company_IdNo, a.Cloth_Sales_No, a.for_OrderBy , a.Cloth_Sales_Date       , a.Invoice_No   ,b.Ledger_Name as PartyName_From   , C.Ledger_Name as PartyName_To ,    a.Meter      ,  a.Rate    , a.Net_Amount  , A.Com_Amount , a.No_Of_Sales ,  d.Transport_Name  , a.Lr_No , e.Ledger_Name as Agent_Name  from Cloth_Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo1 = c.Ledger_IdNo LEFT OUTER JOIN Transport_Head d ON a.Transport_IdNo = d.Transport_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Cloth_Sales_Date between @fromdate and @todate Order by a.Cloth_Sales_Date, a.for_OrderBy, a.Cloth_Sales_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name1        ,       Int1        ,   Name2      ,    Weight10   ,    date1    ,     Name3  ,             Name4                  ,          Name5                ,   weight1       ,  Weight2   ,   Weight3     ,   weight4    , Meters2        ,  Name6            , Name7   , Name8 from reporttemp Order by Date1,    Weight10   , name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name1, Int1, Name2, Weight10, Date1,Date3,   Name3  ,             Name4                  ,          Name5                ,   weight1       ,  Weight2   ,   Weight3     ,   weight4    , Meters2        ,  Name6            , Name7   , Name8  from reporttemp Order by Date1,    Weight10   , name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "cloth sales register - partyfromwise" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Cloth_Sales_Register_PartyFrom.rdlc"
                    ElseIf Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "cloth sales register - partytowise" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Cloth_Sales_Register_PartyTo.rdlc"
                    ElseIf Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "cloth sales register - agentwise" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Cloth_Sales_Register_Agent.rdlc"
                    ElseIf Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "cloth sales register - transportwise" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Cloth_Sales_Register_Transport.rdlc"
                    Else
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Cloth_Sales_Register_PartyFrom.rdlc"
                    End If

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "cloth sales summary"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    'If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                    '    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    'End If

                    cmd.CommandText = "Insert into ReportTemp (    Name4 ,        Weight1  , Weight3   , Weight4  ) " &
                                        " Select             b.Ledger_Name, sum(a.Meter),sum(a.Amount),sum(a.Com_Amount) from Cloth_Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Cloth_Sales_Date between @fromdate and @todate  group by b.Ledger_Name having sum(a.Meter) <> 0 and sum(a.Amount) <> 0 and sum(a.Com_Amount) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name4,Weight1,Weight3,Weight4 from reporttemp where Weight3 <> 0 Order by Name1, Name4", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3,  Name4,Weight1,Weight3,Weight4 from reporttemp Order by Name4", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Cloth_Sales_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True



                Case "delivery register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3        , Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3            ,   Name4                             ,Name5          , Int2    ,Meters1   ,Name6        , Weight1        ,Weight2  ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Delivery_Code, a.Company_IdNo , a.Delivery_No, a.for_OrderBy, a.Delivery_Date, tP.Ledger_Name, d.Item_Name + '(' + a.Remarks + ')' , f.Colour_Name ,a.Rolls  , a.Meters , u.Unit_Name , a.Weight_Rolls ,a.Actual_Weight          from Delivery_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Delivery_Date between @fromdate and @todate Order by a.Delivery_Code ,a.Delivery_Date, a.for_OrderBy, a.Delivery_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3         ,   Name4     ,Name5          , Int2    ,Meters1   ,Name6        , Weight1        ,Weight2     from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Delivery_Register.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "delivery summary"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If


                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3        , Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3            ,   Name4                             ,Name5          , Int2    ,Meters1   ,Name6        , Weight1        ,Weight2  ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Delivery_Code, a.Company_IdNo , a.Delivery_No, a.for_OrderBy, a.Delivery_Date, tP.Ledger_Name, d.Item_Name + '(' + a.Remarks + ')' , f.Colour_Name ,a.Rolls  , a.Meters , u.Unit_Name , a.Weight_Rolls ,a.Actual_Weight          from Delivery_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Delivery_Date between @fromdate and @todate Order by a.Delivery_Code ,a.Delivery_Date, a.for_OrderBy, a.Delivery_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3         ,   Name4     ,Name5          , Int2    ,Meters1   ,Name6        , Weight1        ,Weight2     from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Delivery_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "invoice register - saara"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3         , Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3         ,   Name4                               ,Name5          , Int2    ,Meters1   ,Name6        , Weight1   ,Currency1 , Currency2 , Name7         , Name8                                                              ,    Currency3       ,  Currency4   ,Currency5  , Currency6  , name10    , int4) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Sales_Code , a.Company_IdNo , a.Sales_No   , a.for_OrderBy, a.Sales_Date   , tP.Ledger_Name, d.Item_Name + '(' + a.Serial_No + ')' , f.Colour_Name ,a.Rolls  , a.Meters , u.Unit_Name , a.Weight  ,a.Rate    ,a.Amount   , i.Ledger_Name , h.Tax_Type + '            (' + cast(h.Tax_Perc  as varchar) + '%) ',  h.Tax_Amount      , h.Net_Amount , 0         ,     0      ,  h.Dc_No  , a.SL_No  from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Sales_Head H ON a.Sales_Code = h.Sales_Code INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head i ON h.OnAc_IdNo = i.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Code ,a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()



                    cmd.CommandText = "Select sum(z.Tax_Amount), sum(z.Net_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Sales_Head H ON a.Sales_Code = h.Sales_Code INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head i ON h.OnAc_IdNo = i.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate ) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency5 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency6 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                    End If


                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3         ,   Name4     ,Name5          , Int2    ,Meters1   ,Name6        , Weight1   ,Currency1 , Currency2  ,Name7  , Name8  ,    Currency3  ,  Currency4 ,Currency5,Currency6,Name10  from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3,Currency5 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Invoice_Register_Saara.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "invoice register - saara gst"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3         , Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3         ,Name9             ,   Name4                               , Name5          , Int2    ,Meters1   ,Name6        , Weight1   ,Currency1 , Currency2 , Name7         ,    Currency4  , Currency5  , Meters2            , Meters3          , Meters4            , Meters5        ,Meters6        ,Meters7        ,  Meters8 ,  Currency6  , Currency7 , Currency8 , Currency9 , Currency10 , name10    , int4) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Sales_Code , a.Company_IdNo , a.Sales_No   , a.for_OrderBy, a.Sales_Date   , tP.Ledger_Name,tP.Ledger_GSTinNo , d.Item_Name + '(' + a.Serial_No + ')' , f.Colour_Name ,a.Rolls  , a.Meters , u.Unit_Name , a.Weight  ,a.Rate    ,a.Amount   , i.Ledger_Name  ,  h.Net_Amount , 0          , st.CGST_Percentage ,st.SGST_Percentage, st.IGST_Percentage , st.CGST_Amount ,st.SGST_Amount ,st.IGST_Amount ,  0       , 0           ,  0        , 0         ,  0        ,  0         , h.Dc_No  , a.SL_No  from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Sales_Head H ON a.Sales_Code = h.Sales_Code INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head i ON h.OnAc_IdNo = i.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo LEFT OUTER JOIN Sales_GST_Tax_Details ST ON a.Sales_Code = ST.Sales_Code Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Code ,a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Select sum(z.CGST_Amount),sum(z.SGST_Amount),sum(z.IGST_Amount), sum(z.Net_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Sales_Head H ON a.Sales_Code = h.Sales_Code INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head i ON h.OnAc_IdNo = i.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate ) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency5 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency6 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency7 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency8 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                    End If


                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3         ,   Name4   ,Name9  ,Name5          , Int2    ,Meters1   ,Name6        , Weight1   ,Currency1 , Currency2  ,Name7  , Name8  ,    Currency3  ,  Currency4 ,Currency5,Currency6,Name10, Meters2            , Meters3          , Meters4            , Meters5        ,Meters6        ,Meters7        ,  Meters8 ,  Currency6  , Currency7 , Currency8 , Currency9 , Currency10   from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3,Currency5 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Invoice_Register_Saara_gst.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "invoice summary - saara"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If



                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3         , Name1       ,   Int1          ,  weight10          , Name3         ,   Name4        , Int2         ,Meters1         , Weight1           , Currency4     ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Sales_Code , a.Company_IdNo  , a.for_OrderBy      , tP.Ledger_Name, d.Item_Name + '(' + a.Serial_No + ')'   ,SUM(a.Rolls)  ,SUM( a.Meters ) , SUM(a.Weight )    ,sum(a.Amount)       from Sales_Details a INNER JOIN Sales_Head H ON a.Sales_Code = h.Sales_Code INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate group by a.Sales_Code , a.Company_IdNo  , a.for_OrderBy      , tP.Ledger_Name, d.Item_Name + '(' + a.Serial_No + ')'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3 , Name1       ,   Int1          ,  weight10          , Name3         ,   Name4                                 , Int2         ,Meters1         , Weight1           , Currency4      from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Invoice_Summary_Saara.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True



                Case "invoice register - rr"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If


                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3         , Name1      ,   Int1         ,   Name2      ,  weight10    ,date1        ,Name3           , Date2            ,   Date3               ,Int2          , Int3                ,Currency1 , Currency2       ,Currency3 ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Sales_Code, a.Company_IdNo , a.Sales_No   , a.for_OrderBy, a.Sales_Date, tP.Ledger_Name ,a.Opening_Date    ,   a.Closing_Date      ,a.Total_Copies,a.Additional_Copies  ,a.Rent    , a.Extra_Charges , a.Net_Amount from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Code ,a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3  , Name1      ,   Int1         ,   Name2      ,  weight10    ,date1        ,Name3           , Date2            ,   Date3               ,Int2          , Int3                ,Currency1 , Currency2       ,Currency3  from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Invoice_Register_RR.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "invoice details - rr"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If


                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3         , Name1      ,   Int1         ,   Name2      ,  weight10     ,  date1      ,    Name3       ,   Name4          , Date2            ,   Date3               ,   Name5          ,  Name6            ,  Int2             , Int3            ,Currency1 , Currency2       ,Currency3 , Meters1, Meters2, Meters3 ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Sales_Code, a.Company_IdNo , a.Sales_No   , a.for_OrderBy , a.Sales_Date, tP.Ledger_Name ,m.Machine_Name    ,a.Opening_Date    ,   a.Closing_Date      ,b.Opening_Reading ,b.Closing_Reading  ,b.Sub_Total_Copies ,b.Extra_Copies   ,a.Rent    , a.Extra_Charges , a.Net_Amount ,0  ,  0  ,  0  from Sales_Head a INNER JOIN Sales_Reading_Details b ON a.Sales_No = b.Sales_No INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Machine_Head m On b.Machine_IdNo = m.Machine_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate Order by a.Sales_Code ,a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Select sum(z.Rent), sum(z.Extra_Charges), sum(z.Net_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Reading_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Machine_Head m On b.Machine_IdNo = m.Machine_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters1 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters2 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters3 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                    End If

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3  ,Name1 ,Int1 ,Name2 ,weight10 ,date1 ,Name3 ,Name4 ,Date2 ,Date3 ,Name5 ,Name6 ,Int2 ,Int3 ,Currency1 ,Currency2 ,Currency3 , Meters1, Meters2, Meters3  from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Invoice_Details_RR.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "purchase return summary"
                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    cmd.CommandText = "Insert into ReportTemp(Name1, Name2, Meters1, Date1, Name3, Name4, Currency1) Select a.Purchase_return_Code, a.Purchase_Return_No, a.for_OrderBy, a.Purchase_Return_Date, b.Ledger_Name, a.Bill_No, a.Net_Amount from Purchase_Return_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo, Ledger_Head b where " & Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Purchase_Return_Date between @fromdate and @todate and a.ledger_idno = b.ledger_idno Order by a.Purchase_Return_Date, a.for_OrderBy, a.Purchase_Return_Code, a.Purchase_Return_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Name4, Currency1 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Name4, Currency1 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Purchase_Return_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "purchase return register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Name1,   Int1        ,   Name2      ,   Weight10   ,   Date1        ,   Name6  ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   ,   Currency1,   Currency2,   Currency3      ,   Currency4           ,   Currency5      ,   Currency6          ,   Currency7     ,   Currency8 , Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 ) " &
                                        " Select    a.Purchase_Return_Code, a.Company_IdNo, a.Purchase_Return_No, a.for_OrderBy, a.Purchase_Return_Date, a.Bill_No, c.Ledger_Name, b.SL_No, d.Item_Name, e.Unit_Name, b.Noof_Items, b.Rate     , b.Amount   , a.Gross_Amount, a.Total_DiscountAmount, a.Total_TaxAmount, a.CashDiscount_Amount, a.AddLess_Amount, a.Net_Amount, 0      , 0      , 0      , 0      , 0      , 0        from Purchase_Return_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Purchase_return_Details b ON a.Company_IdNo = b.Company_IdNo and a.Purchase_Return_Code = b.Purchase_Return_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Purchase_Return_Date between @fromdate and @todate Order by a.Purchase_Return_Date, a.for_OrderBy, a.Purchase_Return_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Select sum(z.Gross_Amount), sum(z.Total_DiscountAmount), sum(z.Total_TaxAmount), sum(z.CashDiscount_Amount), sum(z.AddLess_Amount), sum(z.Net_Amount) from Purchase_Return_Head z where z.Purchase_Return_Code IN (Select a.Purchase_Return_Code from Purchase_Return_Head a LEFT OUTER JOIN Purchase_Return_Details b ON a.Company_IdNo = b.Company_IdNo and a.Purchase_Return_Code = b.Purchase_Return_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Purchase_Return_Date between @fromdate and @todate) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters1 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters2 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters3 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters4 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(4).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters5 = " & Str(Val((Dt.Rows(0)(4).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(5).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Meters6 = " & Str(Val((Dt.Rows(0)(5).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                    End If

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Name6, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)


                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Name6, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Purchase_Return_Register.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "sales details - withtax", "sales details - withouttax"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "sales details - withtax" Then

                        cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3   ,   Name1     ,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   , Currency1      , Currency2      , Currency3         , Currency4  , Currency5    ) " &
                                     " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, e.Unit_Name, b.Noof_Items, b.Rate     , b.Amount   , b.Discount_Perc , b.Discount_Amount, b.tax_Perc   from Sales_Head a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate and b.tax_Perc <> 0 Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                        cmd.ExecuteNonQuery()

                    Else

                        cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3   ,   Name1     ,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   , Currency1      , Currency2      , Currency3         , Currency4  , Currency5    ) " &
                                          " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "', a.Sales_Code, a.Company_IdNo, a.Sales_No, a.for_OrderBy, a.Sales_Date, c.Ledger_Name, b.Sl_No, d.Item_Name, e.Unit_Name, b.Noof_Items, b.Rate     , b.Amount   , b.Discount_Perc , b.Discount_Amount, b.tax_Perc   from Sales_Head a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON b.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate and b.tax_Perc = 0 Order by a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                        cmd.ExecuteNonQuery()

                    End If

                    'cmd.CommandText = "Select sum(z.SubTotal_Amount), sum(z.Total_DiscountAmount), sum(z.Total_TaxAmount), sum(z.CashDiscount_Amount), sum(z.AddLess_Amount), sum(z.Net_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Head a LEFT OUTER JOIN Sales_Details b ON a.Company_IdNo = b.Company_IdNo and a.Sales_Code = b.Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON B.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Unit_Head e ON b.Unit_IdNo = e.Unit_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate) "
                    'Da = New SqlClient.SqlDataAdapter(cmd)
                    'Dt = New DataTable
                    'Da.Fill(Dt)


                    'If Dt.Rows.Count > 0 Then
                    '    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    '        cmd.CommandText = "Update ReportTemp set Meters1 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                    '        cmd.ExecuteNonQuery()
                    '    End If
                    '    If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                    '        cmd.CommandText = "Update ReportTemp set Meters2 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                    '        cmd.ExecuteNonQuery()
                    '    End If
                    '    If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                    '        cmd.CommandText = "Update ReportTemp set Meters3 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                    '        cmd.ExecuteNonQuery()
                    '    End If
                    '    If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                    '        cmd.CommandText = "Update ReportTemp set Meters4 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                    '        cmd.ExecuteNonQuery()
                    '    End If
                    '    If IsDBNull(Dt.Rows(0)(4).ToString) = False Then
                    '        cmd.CommandText = "Update ReportTemp set Meters5 = " & Str(Val((Dt.Rows(0)(4).ToString)))
                    '        cmd.ExecuteNonQuery()
                    '    End If
                    '    If IsDBNull(Dt.Rows(0)(5).ToString) = False Then
                    '        cmd.CommandText = "Update ReportTemp set Meters6 = " & Str(Val((Dt.Rows(0)(5).ToString)))
                    '        cmd.ExecuteNonQuery()
                    '    End If
                    'End If

                    'cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    'cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3   ,   Name1     ,   Int1        ,   Name2   ,   Weight10   ,   Date1     ,   Name3      ,   Int2 ,   Name4    ,   Name5    ,   Weight1   , Currency1      , Currency2      , Currency3         , Currency4  , Currency5    from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "sales details - withtax" Then
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_withtax.rdlc"

                    Else
                        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_withouttax.rdlc"

                    End If


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "monthly sales details - quarterly"

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()

                    cmd.Parameters.AddWithValue("CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("CompToDate", Common_Procedures.Company_ToDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If


                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()
                    '-------Empty Records 
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 4 and 6 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 7 and 9 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name4          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 10 and 12 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name5          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 1 and 3 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    '---------------------------------

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Weight1    ,       Weight2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 4 and 6 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Weight3    ,       Weight4 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 7 and 9 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name4          ,       Weight5    ,       Weight6 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 10 and 12 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name5          ,       Weight7    ,       Weight8 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 1 and 3 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_ToDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set  Name2 = 'APR' Where Name2 = ''"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update ReportTemp set  Name3 = 'JUL' Where Name3 = ''"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update ReportTemp set Name4 = 'OCT' Where Name4 = ''"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update ReportTemp set  Name5 = 'JAN' Where Name5 = ''"
                    cmd.ExecuteNonQuery()



                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1 ,Name2    ,Name3    ,Name4    ,Name5,     Weight1 ,Weight2 , Weight3 , Weight4 , Weight5 , Weight6 , Weight7 , Weight8 from reporttemp Order by int1 ,name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_Quarterly.rdlc"
                    ' RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Summary.rdlc"


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True



                Case "monthly sales details - halfyearly"

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("CompToDate", Common_Procedures.Company_ToDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If


                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()
                    '-------Empty Records 
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 4 and 9 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 10 and 12 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 1 and 3 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    '---------------------------------

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Weight1    ,       Weight2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 4 and 9 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Weight3    ,       Weight4 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 10 and 12 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Weight3    ,       Weight4 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 1 and 3 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_ToDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set Name2 = 'APR' Where Name2 = ''"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update ReportTemp set  Name3 = 'OCT' Where Name3 = ''"
                    cmd.ExecuteNonQuery()



                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1 ,Name2    ,Name3   ,    Weight1 ,Weight2 , Weight3 , Weight4 from reporttemp Order by int1 ,name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_HalfYearly.rdlc"



                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "monthly sales details - yearly"

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("CompToDate", Common_Procedures.Company_ToDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If


                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()
                    '-------Empty Records 
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 4 and 12 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Currency1    ,       Currency2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 1 and 3 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    '---------------------------------

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Weight1    ,       Weight2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 4 and 12 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Weight1    ,       Weight2 ) " &
                                      " Select            c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 1 and 3 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_ToDate))) & " GROUP BY c.Ledger_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set Name2 = 'APR' Where Name2 = ''"
                    cmd.ExecuteNonQuery()




                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1 ,Name2    ,  Weight1 ,Weight2  from reporttemp Order by int1 ,name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_Yearly.rdlc"



                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "monthly sales details -itemwise quarterly"


                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("CompToDate", Common_Procedures.Company_ToDate)


                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If


                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()
                    '-------Empty Records 
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 4 and 6 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 7 and 9 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name4          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 10 and 12 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name5          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 1 and 3 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    '---------------------------------

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Weight1    ,       Weight2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 4 and 6 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Weight3    ,       Weight4 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 7 and 9 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name4          ,       Weight5    ,       Weight6 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 10 and 12 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name5          ,       Weight7    ,       Weight8 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 1 and 3 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_ToDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set  Name2 = 'APR' Where Name2 = ''"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update ReportTemp set  Name3 = 'JUL' Where Name3 = ''"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update ReportTemp set Name4 = 'OCT' Where Name4 = ''"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update ReportTemp set  Name5 = 'JAN' Where Name5 = ''"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1 ,Name2    ,Name3    ,Name4    ,Name5,     Weight1 ,Weight2 , Weight3 , Weight4 , Weight5 , Weight6 , Weight7 , Weight8 from reporttemp Order by int1 ,name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_ItemWIseQuarterly.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "monthly sales details -itemwise halfyearly"

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("CompToDate", Common_Procedures.Company_ToDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If


                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()
                    '-------Empty Records 
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 4 and 9 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 10 and 12 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 1 and 3 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    '---------------------------------

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Weight1    ,       Weight2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 4 and 9 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Weight3    ,       Weight4 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 10 and 12 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name3          ,       Weight3    ,       Weight4 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 1 and 3 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_ToDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set  Name2 = 'APR' Where Name2 = ''"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update ReportTemp set  Name3 = 'OCT' Where Name3 = ''"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1 ,Name2    ,Name3 ,      Weight1 ,Weight2 , Weight3 , Weight4  from reporttemp Order by int1 ,name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_ItemWise_HalfYearly.rdlc"



                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "monthly sales details -itemwise yearly"


                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("CompToDate", Common_Procedures.Company_ToDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If


                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()
                    '-------Empty Records 
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 4 and 12 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Currency1    ,       Currency2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 1 and 3 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    '---------------------------------

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Weight1    ,       Weight2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 4 and 12 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1,    Int1,    Int2      ,    Name2          ,       Weight1    ,       Weight2 ) " &
                                      " Select            d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 1 and 3 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_ToDate))) & " GROUP BY d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set  Name2 = 'APR' Where Name2 = ''"
                    cmd.ExecuteNonQuery()



                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1 ,Name2  , Weight1 ,Weight2  from reporttemp Order by int1 ,name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_ItemWise_Yearly.rdlc"



                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "voucher register - bank receipt", "voucher register - bank payment", "voucher register - cash payment", "voucher register - cash receipt", "voucher register - credit note", "voucher register - debit note", "voucher register - petticash"

                    VouCond = ""
                    ReportHd = ""
                    If Trim(LCase(RptIpDet_ReportName)) = "voucher register - purchase" Then
                        VouCond = "(Voucher_Amount > 0 And a.Voucher_Type = 'Purc')"
                        ReportHd = "PURCHASE VOUCHER REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - sales" Then
                        VouCond = "(Voucher_Amount < 0 And a.Voucher_Type = 'Sale')"
                        ReportHd = "SALES VOUCHER REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - bank receipt" Then
                        VouCond = "(Voucher_Amount > 0 And a.Voucher_Type = 'Rcpt')"
                        ReportHd = "BANK RECEIPT REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - bank payment" Then
                        VouCond = "(Voucher_Amount < 0 And a.Voucher_Type = 'Pymt')"
                        ReportHd = "BANK PAYMENT REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - cash receipt" Then
                        VouCond = "(Voucher_Amount > 0 And a.Voucher_Type = 'Csrp')"
                        ReportHd = "CASH RECEIPT REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - cash payment" Then
                        VouCond = "(Voucher_Amount < 0 And a.Voucher_Type = 'Cspy')"
                        ReportHd = "CASH PAYMENT REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - credit note" Then
                        VouCond = "(Voucher_Amount > 0 And a.Voucher_Type = 'Crnt')"
                        ReportHd = "CREDIT NOTE REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - debit note" Then
                        VouCond = "(Voucher_Amount < 0 And a.Voucher_Type = 'Dbnt')"
                        ReportHd = "DEBIT NOTE REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - petticash" Then
                        VouCond = "(Voucher_Amount < 0 And a.Voucher_Type = 'PtCs')"
                        ReportHd = "PETTI CASH REGISTER"
                    End If

                    RptHeading1 = Trim(ReportHd)
                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & Trim(RptHeading3)
                    RptHeading3 = ""

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If


                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt

                    If Trim(VouCond) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & Trim(VouCond)
                    End If

                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Ledger_Idno = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1 ,     Int1        ,    Name2     ,   meters1    ,   Date1       ,   Name3      ,       Currency1                    ,   Name4          ) " &
                                       " Select         a.Voucher_Code,   a.Company_IdNo,  a.Voucher_No, a.for_OrderBy, a.Voucher_Date, c.Ledger_Name, abs(b.Voucher_Amount) as Vou_Amount, a.Narration from Voucher_Head a INNER JOIN Voucher_Details b ON a.Company_IdNo = b.Company_IdNo and a.Voucher_Code = b.Voucher_Code INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON b.Ledger_Idno = c.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Currency1,int2, Name4 from reporttemp Order by Date1, Meters1, name2, name1, Int1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Currency1,int2, Name4 from reporttemp Order by Date1, Meters1, name2, name1, Int1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Voucher_Payment_Receipt_Register.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()

                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "voucher register - contra", "voucher register - journal"

                    VouCond = ""
                    If Trim(LCase(RptIpDet_ReportName)) = "voucher register - contra" Then
                        VouCond = "(a.Voucher_Type = 'Cntr')"
                        ReportHd = "CONTRA REGISTER"
                    ElseIf Trim(LCase(RptIpDet_ReportName)) = "voucher register - journal" Then
                        VouCond = "(a.Voucher_Type = 'Jrnl')"
                        ReportHd = "JOURNAL REGISTER"
                    End If

                    RptHeading1 = Trim(ReportHd)
                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & Trim(RptHeading3)
                    RptHeading3 = ""

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt

                    If Trim(VouCond) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & Trim(VouCond)
                    End If

                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.Ledger_Idno = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1 ,     Int1        ,    Name2     ,   meters1    ,   Date1       ,   Name3      ,                                            Currency1                             ,                                            Currency2                             ,   Name4          ) " &
                                       " Select         a.Voucher_Code,   a.Company_IdNo,  a.Voucher_No, a.for_OrderBy, a.Voucher_Date, c.Ledger_Name, (case When b.Voucher_Amount < 0 then abs(b.Voucher_Amount) else 0 end ) as Db_Amt, (case When b.Voucher_Amount > 0 then abs(b.Voucher_Amount) else 0 end ) as Cr_Amt, a.Narration from Voucher_Head a INNER JOIN Voucher_Details b ON a.Company_IdNo = b.Company_IdNo and a.Voucher_Code = b.Voucher_Code INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head c ON b.Ledger_Idno = c.Ledger_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate"
                    cmd.ExecuteNonQuery()

                    '    Rpt.Report_Show Rpt_Main, cn1.ConnectionString, "Select Voucher_No, a.Voucher_Date, Ledger_Name, (case When Voucher_Amount > 0 then Voucher_Amount else 0 end) as Cr_Amt, (case When Voucher_Amount < 0 then abs(Voucher_Amount) else 0 end ) as Db_Amt, Narration from Voucher_Details a, Ledger_Head b, Voucher_Head c where " & Trim(Cond) & " and a.voucher_ref_no = c.voucher_ref_no and a.voucher_date between '" & Trim(Format(RptDet_Date1, "mm/dd/yyyy")) & "' and '" & Trim(Format(RptDet_Date2, "mm/dd/yyyy")) & "' and a.ledger_idno = b.ledger_idno order by a.voucher_date, c.for_orderby", "" & _
                    '            "<=[LEN7]VOU.NO |<[LEN10]VOU.DATE |<[LEN35]PARTY NAME        |>@[LEN12][ZS]CREDIT |>@[LEN12][ZS]DEBIT |<[LEN35]NARRATION", ReportHd & "|RANGE : " & Trim(RptDet_Date1) & " To " & Trim(RptDet_Date2), FrmNm, "|dd-mm-yy||2|2", "TOTAL|0", [Ledger 136Cols], Portrait, [Vertical Line], [Draft 12cpi]

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Currency1, Currency2, int2, Name4 from reporttemp Order by Date1, Meters1, Int1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Int1, Name2, meters1, Date1, Name3, Currency1, Currency2, int2, Name4 from reporttemp Order by Date1, Meters1, Int1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Voucher_Journal_Contra_Register.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()

                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "sales details item group wise"

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("CompToDate", Common_Procedures.Company_ToDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()
                    '-------Empty Records 
                    cmd.CommandText = "Insert into ReportTemp (Name1                , Name2      ,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select                      e.ItemGroup_Name,d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 4 and 12 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY  e.ItemGroup_Name    ,d.Item_Name, tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp (Name1                , Name2      ,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select                 e.ItemGroup_Name    ,d.Item_Name  , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 1 and 3 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY  e.ItemGroup_Name    ,d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    '---------------------------------

                    cmd.CommandText = "Insert into ReportTemp ( Name1         ,   Name2    , Int1     ,    Int2      ,    Name3          ,       Weight1    ,       Weight2 ) " &
                                      " Select            e.ItemGroup_Name    ,d.Item_Name , tM.IdNo  , tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 4 and 12 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY  e.ItemGroup_Name    ,d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1         ,   Name2    , Int1  ,    Int2      ,    Name3          ,        Weight1    ,       Weight2 ) " &
                                      " Select            e.ItemGroup_Name    ,d.Item_Name ,tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items) , SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 1 and 3 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_ToDate))) & " GROUP BY  e.ItemGroup_Name    ,d.Item_Name, tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set Name2 = 'APR' Where Name3 = ''"
                    cmd.ExecuteNonQuery()




                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1 ,Name2 ,Name3   ,  Weight1 ,Weight2  from reporttemp Order by int1 ,name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_ItemGroupWise.rdlc"



                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "sales details party wise"

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("CompFromDate", Common_Procedures.Company_FromDate)
                    cmd.Parameters.AddWithValue("CompToDate", Common_Procedures.Company_ToDate)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    cmd.CommandText = "Truncate table ReportTempSub"
                    cmd.ExecuteNonQuery()
                    '-------Empty Records 
                    cmd.CommandText = "Insert into ReportTemp (Name1                , Name2      ,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select                      c.Ledger_Name,d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 4 and 12 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY c.Ledger_Name   ,d.Item_Name, tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into ReportTemp (Name1                , Name2      ,    Int1,    Int2      ,    Name3          ,       Currency1    ,       Currency2 ) " &
                                      " Select                c.Ledger_Name    ,d.Item_Name  , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Month_Head tM LEFT OUTER JOIN Sales_Details a ON tM.Month_IdNo between 1 and 3 and month(a.Sales_Date) <> tM.Month_IdNo INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Date between @CompFromDate and @CompToDate GROUP BY  c.Ledger_Name    ,d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName"
                    cmd.ExecuteNonQuery()
                    '---------------------------------

                    cmd.CommandText = "Insert into ReportTemp ( Name1         ,   Name2    , Int1     ,    Int2      ,    Name3          ,       Weight1    ,       Weight2 ) " &
                                      " Select            c.Ledger_Name    ,d.Item_Name , tM.IdNo  , tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items), SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 4 and 12 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_FromDate))) & " GROUP BY  c.Ledger_Name   ,d.Item_Name , tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into ReportTemp ( Name1         ,   Name2    , Int1  ,    Int2      ,    Name3          ,        Weight1    ,       Weight2 ) " &
                                      " Select            c.Ledger_Name    ,d.Item_Name ,tM.IdNo, tM.Month_IdNo, tM.Month_ShortName, SUM(a.Noof_Items) , SUM(a.Amount)   from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo <> 0 and a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN ItemGroup_Head e ON a.ItemGroup_IdNo <> 0 and a.ItemGroup_IdNo = e.ItemGroup_IdNo INNER JOIN Month_Head tM ON tM.Month_IdNo <> 0 and month(a.Sales_Date) = tM.Month_IdNo  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " month(a.Sales_Date) between 1 and 3 and Year(a.Sales_Date) = " & Str(Val(Year(Common_Procedures.Company_ToDate))) & " GROUP BY  c.Ledger_Name   ,d.Item_Name, tM.IdNo, tM.Month_IdNo, tM.Month_ShortName Having SUM(a.Noof_Items) <> 0"
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update ReportTemp set Name2 = 'APR' Where Name3 = ''"
                    cmd.ExecuteNonQuery()




                    cmd.CommandText = "Update ReportTemp set Company_Name = '" & Trim(CompName) & "', Company_Address1 = '" & Trim(CompAdd1) & "', Company_Address2 = '" & Trim(CompAdd2) & "', Report_Heading1 = '" & Trim(RptHeading1) & "', Report_Heading2 = '" & Trim(RptHeading2) & "', Report_Heading3 = '" & Trim(RptHeading3) & "'"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,Name1 ,Name2 ,Name3   ,  Weight1 ,Weight2  from reporttemp Order by int1 ,name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Date1, Name2, Name3, Name4, Name5, Weight1, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8, Meters1, Meters2, Meters3, Meters4, Meters5, Meters6 from reporttemp Order by Date1, Weight10, name2, name1, Int2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SalesDetails_ItemGroupWise.rdlc"



                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True



                Case "sales receipt summary"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If



                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If



                    cmd.CommandText = "Insert into ReportTemp (  Name1   , Name2  ,       Meters2   ,int2 ,           meters3    ,   Meters4 ) " &
                                        " Select        c.Ledger_Name, d.Item_Name, SUM(a.Quantity) ,sum(a.No_Of_Rolls),sum(a.Rate),sum(a.Amount) from Sales_Receipt_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo   INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo INNER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Receipt_Date between @fromdate and @todate group by c.Ledger_Name ,  d.Item_Name having sum(a.Amount) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Name3 ,int2 , Meters2,Meters3,Meters4 from reporttemp where int2 <> 0 Order by Name1, Name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("Select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Weight1 from reporttemp Order by Name1, Name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Receipt_Summary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True


                Case "sales delivery pending register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp ( Name1          ,   Name2         ,   Meters1    ,        Date1     ,    Name3          ,  Name4         ,  Name5 ,      Name6    ,    Name7   , Meters2                      , Meters3   ,Currency1,  NAME8 , Meters6 ,int2) " &
                                            "     Select a.Sales_Receipt_Code, a.Sales_Receipt_No, a.for_OrderBy, a.Sales_Receipt_Date, tP.Ledger_Name , d.Item_Name  ,e.Unit_Name ,f.Style_Name ,g.Size_Name ,(a.Quantity-a.Delivery_Quantity) ,a.Rate,a.Amount,a.Hsn_Code,a.Tax_Perc,(a.No_Of_Rolls-  a.Delivery_No_Of_Rolls)from Sales_Receipt_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ledger_idno = tP.ledger_idno  INNER JOIN Item_Head d ON a.Item_idno = d.Item_idno  INNER JOIN Unit_Head e ON a.Unit_idno = e.Unit_idno   LEFT OUTER JOIN Style_Head f ON a.Style_idno = f.Style_idno  LEFT OUTER JOIN Size_Head g ON a.Size_idno = g.Size_idno  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " (a.Quantity - a.Delivery_Quantity) > 0 and a.Sales_Receipt_Date between @fromdate and @todate Order by a.Sales_Receipt_Date, a.for_OrderBy, a.Sales_Receipt_Code, a.Sales_Receipt_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3,  Name4 , Name5   ,Name6,name7,Meters2,Meters3, Currency1 , Name8 , Meters6,int2 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 , Name4 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1


                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Delivery_Pending_Register.rdlc"


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "sales receipt register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp ( Name1          ,   Name2         ,   Meters1    ,        Date1     ,    Name3          ,  Name4         ,  Name5 ,      Name6    ,    Name7   , Meters2 , Meters3   ,Currency1,  name8 , Meters6 ,int2) " &
                                            "     Select a.Sales_Receipt_Code, a.Sales_Receipt_No, a.for_OrderBy, a.Sales_Receipt_Date, tP.Ledger_Name , d.Item_Name  ,e.Unit_Name ,f.Style_Name ,g.Size_Name ,a.Quantity ,a.Rate,a.Amount,a.Hsn_Code,a.Tax_Perc,a.No_of_Rolls from Sales_Receipt_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ledger_idno = tP.ledger_idno  INNER JOIN Item_Head d ON a.Item_idno = d.Item_idno  LEFT OUTER JOIN Unit_Head e ON a.Unit_idno = e.Unit_idno   LEFT OUTER JOIN Style_Head f ON a.Style_idno = f.Style_idno  LEFT OUTER JOIN Size_Head g ON a.Size_idno = g.Size_idno  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & "  a.Sales_Receipt_Date between @fromdate and @todate Order by a.Sales_Receipt_Date, a.for_OrderBy, a.Sales_Receipt_Code, a.Sales_Receipt_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3,  Name4 , Name5   ,Name6,Name7 , int2,Meters2,Meters3,name8,currency1,Meters6 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 , Name4 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1


                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Receipt_Register.rdlc"


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "production register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3         ,     Name1       ,       Int1         ,   Name2        ,  weight1    ,       date1           ,   Name3     ,   Name4  ,        Name5  ,                 Name6       ,       Name7                 ,  Meters1    ,   Meters2   ,   Meters3        , Currency1  ) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Production_Code , a.Company_IdNo , a.Production_No   , a.for_OrderBy, a.Production_Date   , tP.Ledger_Name, a.Shift,  mh.Machine_Name, FEM.Employee_Name as Framer, FEM.Employee_Name as Operator, a.Total_Heads, a.Total_Stchs , a.Total_Pcs , a.Total_Amt  from Production_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo INNER JOIN Machine_Head MH ON a.Machine_IdNo = mh.Machine_IdNo INNER JOIN Employee_Head FEM ON a.Framer_IdNo = FEM.Employee_IdNo INNER JOIN Employee_Head OEM ON a.Operator_IdNo = OEM.Employee_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Production_Date between @fromdate and @todate Order by a.Production_Code ,a.Production_Date, a.for_OrderBy, a.Production_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()


                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3,   Name1 ,Name2,Name3,Name4,Name5,Name6,Name7,  weight1,weight2,weight3,weight4,weight5,   date1,date2,date3,  Currency1, Meters1,Meters2,Meters3,Meters4 from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3,Currency5 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Production_Register.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True



                Case "bill register"


                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)))
                    End If

                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp(Company_Name,    Company_Address1     ,   Company_Address2      ,    Report_Heading1         ,     Report_Heading2        ,     Report_Heading3         , Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3         ,Name9             ,   Name4                                                                                           , Name5          , Int2    ,Meters1   ,Name6        , Weight1   ,Currency1 , Currency2 , Name7         ,   Currency4  , Currency5  , Meters2            , Meters3          , Meters4            , Meters5        ,Meters6        ,Meters7        ,  Meters8 ,  Currency6  , Currency7 , Currency8 , Currency9 , Currency10 , name10    , int4) " &
                                        " Select  '" & Trim(CompName) & "', '" & Trim(CompAdd1) & "', '" & Trim(CompAdd2) & "', '" & Trim(RptHeading1) & "', '" & Trim(RptHeading2) & "', '" & Trim(RptHeading3) & "' ,a.Sales_Code , a.Company_IdNo , a.Sales_No   , a.for_OrderBy, a.Sales_Date   , tP.Ledger_Name,tP.Ledger_GSTinNo , (case when a.Serial_No <> '' then  d.Item_Name + '(' + a.Serial_No + ')' else d.Item_Name end   ) , f.Colour_Name ,a.Rolls  , a.Meters , u.Unit_Name , a.Weight  ,a.Rate    ,a.Amount   , i.Ledger_Name  , h.Net_Amount , 0          , st.CGST_Percentage ,st.SGST_Percentage, st.IGST_Percentage , st.CGST_Amount ,st.SGST_Amount ,st.IGST_Amount ,  0       , 0           ,  0        , 0         ,  0        ,  0         , h.Dc_No  , a.SL_No  from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Sales_Head H ON a.Sales_Code = h.Sales_Code INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head i ON h.OnAc_IdNo = i.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo LEFT OUTER JOIN Sales_GST_Tax_Details ST ON a.Sales_Code = ST.Sales_Code Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate and Entry_Status = 'BILL' Order by a.Sales_Code ,a.Sales_Date, a.for_OrderBy, a.Sales_No, a.Company_IdNo"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Select sum(z.CGST_Amount),sum(z.SGST_Amount),sum(z.IGST_Amount), sum(z.Net_Amount) from Sales_Head z where z.Sales_Code IN (Select a.Sales_Code from Sales_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  INNER JOIN Sales_Head H ON a.Sales_Code = h.Sales_Code INNER JOIN Ledger_Head tP ON a.Ledger_IdNo = tP.Ledger_IdNo LEFT OUTER JOIN Ledger_Head i ON h.OnAc_IdNo = i.Ledger_IdNo LEFT OUTER JOIN Item_Head d ON a.Item_IdNo = d.Item_IdNo LEFT OUTER JOIN Colour_Head f ON a.Colour_IdNo = f.Colour_IdNo LEFT OUTER JOIN Unit_Head u ON a.Unit_IdNo = u.Unit_IdNo Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Date between @fromdate and @todate ) "
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency5 = " & Str(Val((Dt.Rows(0)(0).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency6 = " & Str(Val((Dt.Rows(0)(1).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(2).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency7 = " & Str(Val((Dt.Rows(0)(2).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                        If IsDBNull(Dt.Rows(0)(3).ToString) = False Then
                            cmd.CommandText = "Update ReportTemp set Currency8 = " & Str(Val((Dt.Rows(0)(3).ToString)))
                            cmd.ExecuteNonQuery()
                        End If
                    End If


                    Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1       ,   Int1         ,   Name2      ,  weight10    ,date1           , Name3         ,   Name4   ,Name9  ,Name5          , Int2    ,Meters1   ,Name6        , Weight1   ,Currency1 , Currency2  ,Name7  , Name8  ,    Currency3  ,  Currency4 ,Currency5,Currency6,Name10, Meters2            , Meters3          , Meters4            , Meters5        ,Meters6        ,Meters7        ,  Meters8 ,  Currency6  , Currency7 , Currency8 , Currency9 , Currency10   from reporttemp Order by Date1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select Company_Name, Company_Address1, Company_Address2, Report_Heading1, Report_Heading2, Report_Heading3, Name1, Int1, Name2, Weight10, Date1, Name3, Name4,Name5, int2,int3,Currency5 from reporttemp Order by Date1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1
                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_BillRegister_Saara_gst.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True

                Case "general delivery register"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt

                    RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Sales_Delivery_Code Like 'OTDEL%'"

                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_Ledger.Visible = True And Trim(cbo_Ledger.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into ReportTemp ( Name1          ,   Name2         ,   Meters1    ,                   Date1     ,    Name3       ,  Name4         ,  Name5   , INT2     ,     Name7         ) " &
                                            "     Select a.Sales_Delivery_Code, a.Sales_Delivery_No, a.for_OrderBy, a.Sales_Delivery_Date, tP.Ledger_Name      , d.Item_Name  ,e.Unit_Name ,a.Quantity, a.Item_Description  from Sales_Delivery_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ledger_idno = tP.ledger_idno  INNER JOIN Item_Head d ON a.Item_idno = d.Item_idno  INNER JOIN Unit_Head e ON a.Unit_idno = e.Unit_idno  Where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Sales_Delivery_Date between @fromdate and @todate Order by a.Sales_Delivery_Date, a.for_OrderBy, a.Sales_Delivery_Code, a.Sales_Delivery_No"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3,  Name4 , Name5   ,int2, Name8,  Name6, Name7,Name9,Meters2,meters3 from reporttemp Order by Date1, meters1, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Name1, Name2, Meters1, Date1, Name3, Weight1, Currency1 , Name4 from reporttemp Order by Date1, meters1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1


                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Sales_Delivery_Register.rdlc"


                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True



            End Select


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Stock_Report()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dtbl1 As New DataTable
        Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource
        Dim RptCondt As String, Condt1 As String
        Dim CompCondt As String
        Dim IpColVal1 As String

        Try

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "(Company_Type <> 'UNACCOUNT')"
            End If

            Select Case Trim(LCase(Common_Procedures.RptInputDet.ReportName))

                Case "stock details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) = "" Then
                        MessageBox.Show("Invalid ItemName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    cmd.CommandText = "Insert into reporttemp(int5, name3, weight1, weight2) Select 0, 'Opening', (case when sum(a.Quantity) > 0 then sum(a.Quantity) else 0 end ), (case when sum(a.Quantity) < 0 then abs(sum(a.Quantity)) else 0 end ) from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into reporttemp(int5, Date1, name1, name2, meters1, name3, weight1, weight2) Select 1, a.Reference_Date, a.Reference_Code, a.Party_Bill_No, a.For_OrderBy, b.Ledger_Name, a.Quantity, 0 from Item_Processing_Details a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Quantity > 0 "
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into reporttemp(int5, Date1, name1, name2, meters1, name3, weight1, weight2) Select 2, a.Reference_Date, a.Reference_Code, a.Party_Bill_No, a.For_OrderBy, b.Ledger_Name, 0, abs(a.Quantity) from Item_Processing_Details a  INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Quantity < 0 "
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int5, Date1, name1, name2, meters1, name3, weight1, weight2, Weight3 from reporttemp where weight1 <> 0 or weight2 <> 0 Order by Date1, Int5, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    ''Debug.Print(Now)
                    'TotStk = 0
                    'If Dtbl1.Rows.Count > 0 Then
                    '    TotStk = Val(Dtbl1.Rows(0).Item("Weight1").ToString) - Val(Dtbl1.Rows(0).Item("Weight2").ToString)
                    '    Dtbl1.Rows(0).Item("Weight3") = Val(TotStk)
                    '    For i = 1 To Dtbl1.Rows.Count - 1
                    '        TotStk = Val(TotStk) + Val(Dtbl1.Rows(i).Item("Weight1").ToString) - Val(Dtbl1.Rows(i).Item("Weight2").ToString)
                    '        Dtbl1.Rows(i).Item("Weight3") = Val(TotStk)
                    '    Next i
                    'End If
                    ''Debug.Print(Now)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int5, Date1, name1, name2, meters1, name3, weight1, weight2, Weight3 from reporttemp Order by Date1, Int1, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_StockDetails.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    'Debug.Print(Now)
                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                'Debug.Print(Now)

                Case "garments stock details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) = "" Then
                        MessageBox.Show("Invalid ItemName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    IpColVal1 = ""
                    If cbo_SizeName.Visible = True Then

                        If Trim(cbo_SizeName.Text) <> "" Then
                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Size_IdNo = " & Str(Val(Common_Procedures.Size_NameToIdNo(con, cbo_SizeName.Text)))
                            IpColVal1 = "[HIDDEN]"
                        End If

                    Else
                        IpColVal1 = "[HIDDEN]"

                    End If

                    cmd.CommandText = "Insert into reporttemp(int5, name3, weight1, weight2) Select 0, 'Opening', (case when sum(a.Quantity) > 0 then sum(a.Quantity) else 0 end ), (case when sum(a.Quantity) < 0 then abs(sum(a.Quantity)) else 0 end ) from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into reporttemp(int5, Date1, name1, name2, meters1, name3, Name4, weight1, weight2) Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, b.Ledger_Name, c.Size_Name, abs(a.Quantity), 0 from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Size_Head c ON a.Size_IdNo = c.Size_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Quantity > 0 "
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into reporttemp(int5, Date1, name1, name2, meters1, name3, Name4, weight1, weight2) Select 2, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, b.Ledger_Name, c.Size_Name, 0, abs(a.Quantity) from Item_Processing_Details a  INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Size_Head c ON a.Size_IdNo = c.Size_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Quantity < 0 "
                    cmd.ExecuteNonQuery()


                    If Trim(IpColVal1) <> "" Then
                        cmd.CommandText = "update reporttemp set name4 = '" & Trim(IpColVal1) & "'"
                        cmd.ExecuteNonQuery()
                    End If

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int5, Date1, name1, name2, meters1, name3, name4, weight1, weight2, Weight3 from reporttemp where weight1 <> 0 or weight2 <> 0 Order by Date1, Int5, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int5, Date1, name1, name2, meters1, name3, name4, weight1, weight2, Weight3 from reporttemp Order by Date1, Int5, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_GarmentsStockDetails.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    'Debug.Print(Now)
                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                'Debug.Print(Now)


                Case "stock summary", "minimum stock level"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid Up Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con


                    cmd.Parameters.Clear()

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemGroupName.Visible = True And Trim(cbo_ItemGroupName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " b.ItemGroup_IdNo = " & Str(Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroupName.Text)))
                    End If

                    Condt1 = ""
                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "minimum stock level" Then
                        cmd.Parameters.AddWithValue("@uptodate", Date.Today)

                        Condt1 = "(sum(a.Quantity) < b.Minimum_Stock)"

                    Else
                        cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into reporttemp ( name1, name2, weight1, meters1, currency1, Weight2) Select b.item_name, c.itemgroup_name, sum(a.Quantity), b.Cost_Rate, (sum(a.Quantity) * b.Cost_Rate) as stockvalue, b.Minimum_Stock from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Item_Head b ON a.item_idno = b.item_idno LEFT OUTER JOIN ItemGroup_Head c ON b.itemgroup_idno = c.itemgroup_idno Where " & Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date <= @uptodate group by b.item_name, c.itemgroup_name, b.Cost_Rate, b.Minimum_Stock " & IIf(Trim(Condt1) <> "", " Having ", "") & Condt1 & " order by b.item_name"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int1, name1, name2, weight1, meters1, currency1 from reporttemp Order by name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int1, name1, weight1, meters1, currency1 from reporttemp Order by name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_StockSummary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    Debug.Print(Now)
                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                    Debug.Print(Now)

                Case "stock summary details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid Up Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con


                    cmd.Parameters.Clear()

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If


                    Condt1 = ""
                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "minimum stock level" Then
                        cmd.Parameters.AddWithValue("@uptodate", Date.Today)

                        Condt1 = "(sum(a.Quantity) < b.Minimum_Stock)"

                    Else
                        cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                    End If

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into reporttemp ( name1, name2,  weight1, meters1, currency1, Weight2  ) Select b.item_name , c.Unit_Name ,  sum(a.Quantity), b.Cost_Rate, (sum(a.Quantity) * b.Cost_Rate) as stockvalue, b.Minimum_Stock from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Item_Head b ON a.item_idno = b.item_idno LEFT OUTER JOIN Unit_Head c ON C.Unit_idno = B.Unit_idno Where " & Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date <= @uptodate group by b.item_name , c.Unit_Name  , b.Cost_Rate, b.Minimum_Stock " & IIf(Trim(Condt1) <> "", " Having ", "") & Condt1 & " order by b.item_name"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int1, name1, name2, weight1, meters1, currency1 from reporttemp Order by name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int1, name1, weight1, meters1, currency1 from reporttemp Order by name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_StockSummaryDetails.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    Debug.Print(Now)
                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                    Debug.Print(Now)

                Case "garments stock summary"

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid Up Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If
                    If cbo_SizeName.Visible = True Then
                        If Trim(cbo_SizeName.Text) <> "" Then
                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Size_IdNo = " & Str(Val(Common_Procedures.Size_NameToIdNo(con, cbo_SizeName.Text)))
                        End If
                    End If


                    Condt1 = ""
                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "minimum stock level" Then
                        cmd.Parameters.AddWithValue("@uptodate", Date.Today)

                        Condt1 = "(sum(a.Quantity) < b.Minimum_Stock)"

                    Else
                        cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                    End If

                    cmd.CommandText = "Insert into reporttemp ( name1, name2, weight1, meters1, currency1) Select b.item_name, c.Size_name, sum(a.Quantity), b.Cost_Rate, (sum(a.Quantity) * b.Cost_Rate) as stockvalue from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Item_Head b ON a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN size_head c ON a.size_idno = c.size_idno Where " & Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date <= @uptodate group by b.item_name, c.Size_name, b.Cost_Rate Having sum(a.Quantity) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, name1, name2, weight1, meters1, currency1 from reporttemp where weight1 <> 0 Order by name1, name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, name1, name2, weight1, meters1, currency1 from reporttemp Order by name1, name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_GarmentsStockSummary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    Debug.Print(Now)
                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                    Debug.Print(Now)


                Case "garments2 stock details"

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If
                    If dtp_ToDate.Visible = True And IsDate(dtp_ToDate.Text) = False Then
                        MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_ToDate.Enabled Then dtp_ToDate.Focus()
                        Exit Sub
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) = "" Then
                        MessageBox.Show("Invalid ItemName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
                    cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If
                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If

                    IpColVal1 = ""
                    If cbo_SizeName.Visible = True Then
                        If Trim(cbo_SizeName.Text) <> "" Then
                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Size_IdNo = " & Str(Val(Common_Procedures.Size_NameToIdNo(con, cbo_SizeName.Text)))
                            IpColVal1 = "[HIDDEN]"
                        End If

                    Else
                        IpColVal1 = "[HIDDEN]"

                    End If

                    cmd.CommandText = "Insert into reporttemp(int5, name3, weight1, weight2) Select 0, 'Opening', (case when sum(a.Quantity) > 0 then sum(a.Quantity) else 0 end ), (case when sum(a.Quantity) < 0 then abs(sum(a.Quantity)) else 0 end ) from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into reporttemp(int5, Date1, name1, name2, meters1, name3, Name4,Name5  ,  Name6  ,  Name7    , Name8 ,  weight1, weight2) Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, b.Ledger_Name, c.Size_Name,d.Colour_Name ,e.Design_Name  ,  f.Gender_Name  , g.Sleeve_Name,    abs(a.Quantity), 0 from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Size_Head c ON a.Size_IdNo = c.Size_IdNo LEFT OUTER JOIN Colour_Head d ON a.Colour_IdNo = d.Colour_IdNo LEFT OUTER JOIN Design_Head e ON a.Design_IdNo = e.design_IdNo LEFT OUTER JOIN Gender_Head f ON a.Gender_IdNo = f.Gender_IdNo LEFT OUTER JOIN Sleeve_Head g ON a.Sleeve_IdNo = g.Sleeve_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Quantity > 0 "
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into reporttemp(int5, Date1, name1, name2, meters1, name3, Name4,Name5  ,  Name6  ,  Name7    , Name8  ,  weight1, weight2) Select 2, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, b.Ledger_Name, c.Size_Name,  d.Colour_Name ,e.Design_Name  ,  f.Gender_Name  , g.Sleeve_Name,  0, abs(a.Quantity) from Item_Processing_Details a  INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Size_Head c ON a.Size_IdNo = c.Size_IdNo LEFT OUTER JOIN Colour_Head d ON a.Colour_IdNo = d.Colour_IdNo LEFT OUTER JOIN Design_Head e ON a.Design_IdNo = e.design_IdNo LEFT OUTER JOIN Gender_Head f ON a.Gender_IdNo = f.Gender_IdNo LEFT OUTER JOIN Sleeve_Head g ON a.Sleeve_IdNo = g.Sleeve_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Quantity < 0 "
                    cmd.ExecuteNonQuery()


                    If Trim(IpColVal1) <> "" Then
                        cmd.CommandText = "update reporttemp set name4 = '" & Trim(IpColVal1) & "'"
                        cmd.ExecuteNonQuery()
                    End If

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int5, Date1, name1, name2, meters1, name3, name4,Name5  ,Name6  ,  Name7  ,  Name8, weight1, weight2 from reporttemp where weight1 <> 0 or weight2 <> 0 Order by Date1, Int5, name2, name1", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, int5, Date1, name1, name2, meters1, name3, name4, Name5  ,Name6  ,  Name7  , Name8  ,  weight1, weight2 from reporttemp Order by Date1, Int5, name2, name1", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Garments2_StockDetails.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    'Debug.Print(Now)
                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                'Debug.Print(Now)



                Case "garments2 stock summary"

                    RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "", "   -   ", "") & RptHeading3
                    RptHeading3 = ""

                    If dtp_FromDate.Visible = True And IsDate(dtp_FromDate.Text) = False Then
                        MessageBox.Show("Invalid Up Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
                        Exit Sub
                    End If

                    cmd.Connection = con

                    cmd.CommandText = "Truncate table ReportTemp"
                    cmd.ExecuteNonQuery()

                    cmd.Parameters.Clear()

                    RptCondt = CompCondt
                    If cbo_Company.Visible = True And Trim(cbo_Company.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Company_IdNo = " & Str(Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)))
                    End If

                    If cbo_ItemName.Visible = True And Trim(cbo_ItemName.Text) <> "" Then
                        RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Item_IdNo = " & Str(Val(Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)))
                    End If
                    If cbo_SizeName.Visible = True Then
                        If Trim(cbo_SizeName.Text) <> "" Then
                            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Size_IdNo = " & Str(Val(Common_Procedures.Size_NameToIdNo(con, cbo_SizeName.Text)))
                        End If
                    End If


                    Condt1 = ""
                    If Trim(LCase(Common_Procedures.RptInputDet.ReportName)) = "minimum stock level" Then
                        cmd.Parameters.AddWithValue("@uptodate", Date.Today)

                        Condt1 = "(sum(a.Quantity) < b.Minimum_Stock)"

                    Else
                        cmd.Parameters.AddWithValue("@uptodate", dtp_FromDate.Value.Date)

                    End If

                    cmd.CommandText = "Insert into reporttemp ( name1, name2,name3   ,   name4    , name5   ,  name6  , weight1, meters1, currency1) Select b.item_name, c.Size_name,d.Colour_Name   , e.Design_Name  , f.Gender_Name  ,  g.Sleeve_Name  , sum(a.Quantity), b.Cost_Rate, (sum(a.Quantity) * b.Cost_Rate) as stockvalue from Item_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Item_Head b ON a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN size_head c ON a.size_idno = c.size_idno LEFT OUTER JOIN Colour_Head d ON a.Colour_IdNo = d.Colour_IdNo LEFT OUTER JOIN Design_Head e ON a.Design_IdNo = e.design_IdNo LEFT OUTER JOIN Gender_Head f ON a.Gender_IdNo = f.Gender_IdNo LEFT OUTER JOIN Sleeve_Head g ON a.Sleeve_IdNo = g.Sleeve_IdNo Where " & Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date <= @uptodate group by b.item_name, c.Size_name,d.Colour_Name   , e.Design_Name  , f.Gender_Name  ,  g.Sleeve_Name, b.Cost_Rate Having sum(a.Quantity) <> 0"
                    cmd.ExecuteNonQuery()

                    Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, name1, name2,name3   ,   name4    , name5   ,  name6  , weight1, meters1, currency1 from reporttemp where weight1 <> 0 Order by name1, name2", con)
                    Dtbl1 = New DataTable
                    Da.Fill(Dtbl1)

                    RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
                    RpDs1.Name = "DataSet1"
                    RpDs1.Value = Dtbl1

                    If Dtbl1.Rows.Count = 0 Then

                        cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
                        cmd.ExecuteNonQuery()

                        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, name1, name2,name3   ,   name4    , name5   ,  name6  , weight1, meters1, currency1 from reporttemp Order by name1, name2", con)
                        Dtbl1 = New DataTable
                        Da.Fill(Dtbl1)

                    End If

                    RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_Garments2_StockSummary.rdlc"

                    RptViewer.LocalReport.DataSources.Clear()

                    RptViewer.LocalReport.DataSources.Add(RpDs1)

                    Debug.Print(Now)
                    RptViewer.LocalReport.Refresh()
                    RptViewer.RefreshReport()

                    RptViewer.Visible = True
                    Debug.Print(Now)
            End Select


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SHOW REPORT....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub cbo_PhoneNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PhoneNo.GotFocus
        With cbo_PhoneNo
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_PhoneNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PhoneNo.KeyDown

        Try
            With cbo_PhoneNo
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                        cbo_SerialNo.Focus()
                    ElseIf txt_Inputs1.Visible And txt_Inputs1.Enabled Then
                        txt_Inputs1.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                        cbo_GroupName.Focus()
                    ElseIf cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    btn_Show.Focus()
                    Show_Report()
                    'SendKeys.Send("{TAB}")

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_PhoneNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PhoneNo.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_PhoneNo

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_PhoneNo
                            If Trim(.Text) <> "" Then
                                If .DroppedDown = True Then
                                    If Trim(.SelectedText) <> "" Then
                                        .Text = .SelectedText
                                    Else
                                        If .Items.Count > 0 Then
                                            .SelectedIndex = 0
                                            .SelectedItem = .Items(0)
                                            .Text = .GetItemText(.SelectedItem)
                                        End If
                                    End If
                                End If
                            End If
                        End With

                        btn_Show.Focus()
                        Show_Report()
                        'SendKeys.Send("{TAB}")

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Party_PhoneNo like '" & Trim(FindStr) & "%' or Party_PhoneNo like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select DISTINCT(Party_PhoneNo) from Sales_Head " & Condt & " order by Party_PhoneNo", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Party_PhoneNo"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_PhoneNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PhoneNo.LostFocus
        cbo_PhoneNo.BackColor = Color.White
        cbo_PhoneNo.ForeColor = Color.Black
    End Sub


    Private Sub txt_Inputs1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Inputs1.GotFocus
        With txt_Inputs1
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_Inputs1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Inputs1.KeyDown

        Try
            With txt_Inputs1
                If e.KeyValue = 38 Then
                    e.Handled = True
                    If cbo_SerialNo.Visible And cbo_SerialNo.Enabled Then
                        cbo_SerialNo.Focus()
                    ElseIf cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                        cbo_ItemName.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                        cbo_GroupName.Focus()
                    ElseIf cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 Then
                    e.Handled = True

                    If cbo_PhoneNo.Visible And cbo_PhoneNo.Enabled Then
                        cbo_PhoneNo.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                        'SendKeys.Send("{TAB}")

                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Inputs1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Inputs1.KeyPress

        Try

            With txt_Inputs1

                If Asc(e.KeyChar) = 13 Then

                    If cbo_PhoneNo.Visible And cbo_PhoneNo.Enabled Then
                        cbo_PhoneNo.Focus()

                    Else
                        btn_Show.Focus()
                        Show_Report()
                        'SendKeys.Send("{TAB}")

                    End If


                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Inputs1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Inputs1.LostFocus
        txt_Inputs1.BackColor = Color.White
        txt_Inputs1.ForeColor = Color.Black
    End Sub



    Private Sub cbo_SerialNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SerialNo.GotFocus
        With cbo_SerialNo
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_SerialNo.Text.Length
        End With
    End Sub

    Private Sub cbo_SerialNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SerialNo.KeyDown

        Try
            With cbo_SerialNo
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_ItemName.Visible And cbo_ItemName.Enabled Then
                        cbo_ItemName.Focus()
                    ElseIf cbo_Ledger.Visible And cbo_Ledger.Enabled Then
                        cbo_Ledger.Focus()
                    ElseIf cbo_GroupName.Visible And cbo_GroupName.Enabled Then
                        cbo_GroupName.Focus()
                    ElseIf cbo_Company.Visible And cbo_Company.Enabled Then
                        cbo_Company.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_ToDate.Focus()
                    ElseIf dtp_ToDate.Visible And dtp_ToDate.Enabled Then
                        dtp_FromDate.Focus()
                    End If
                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If cbo_PhoneNo.Visible And cbo_PhoneNo.Enabled Then
                        cbo_PhoneNo.Focus()
                    Else
                        btn_Show.Focus()
                        Show_Report()
                        'SendKeys.Send("{TAB}")

                    End If

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_SerialNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SerialNo.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_SerialNo

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_SerialNo
                            If Trim(.Text) <> "" Then
                                If .DroppedDown = True Then
                                    If Trim(.SelectedText) <> "" Then
                                        .Text = .SelectedText
                                    Else
                                        If .Items.Count > 0 Then
                                            .SelectedIndex = 0
                                            .SelectedItem = .Items(0)
                                            .Text = .GetItemText(.SelectedItem)
                                        End If
                                    End If
                                End If
                            End If
                        End With

                        If cbo_PhoneNo.Visible And cbo_PhoneNo.Enabled Then
                            cbo_PhoneNo.Focus()

                        Else
                            btn_Show.Focus()
                            Show_Report()
                            'SendKeys.Send("{TAB}")

                        End If

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Serial_No like '" & Trim(FindStr) & "%' or Serial_No like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select DISTINCT(Serial_No) from Sales_Details " & Condt & " order by Serial_No", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Serial_No"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_SerialNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SerialNo.LostFocus
        cbo_SerialNo.BackColor = Color.White
        cbo_SerialNo.ForeColor = Color.Black
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim bytes As Byte() = RptViewer.LocalReport.Render("Excel")
        Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.xls")

        'Dim bytes As Byte() = RptViewer.LocalReport.Render("Pdf")
        'Dim fs As System.IO.FileStream = System.IO.File.Create("C:\test.pdf")
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()
        MessageBox.Show("ok")
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub DayBook_Report()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dtbl1 As New DataTable
        Dim RpDs1 As New Microsoft.Reporting.WinForms.ReportDataSource
        Dim Nr As Long = 0
        Dim Comp_IdNo As Integer = 0
        Dim CompCondt As String = ""
        Dim RptCondt As String = ""
        Dim IpColNm1 As String = ""
        Dim OpAmt As Double = 0

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            CompCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
        End If

        RptHeading2 = Trim(RptHeading2) & IIf(Trim(RptHeading2) <> "" And Trim(RptHeading3) <> "", vbCrLf, "") & RptHeading3
        RptHeading3 = ""

        If IsDate(dtp_FromDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_FromDate.Visible = True And dtp_FromDate.Enabled = True Then dtp_FromDate.Focus()
            Exit Sub
        End If

        If IsDate(dtp_ToDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_ToDate.Visible = True And dtp_ToDate.Enabled = True Then dtp_ToDate.Focus()
            Exit Sub
        End If

        Comp_IdNo = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)

        Cmd.Connection = con

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        Cmd.Parameters.AddWithValue("@todate", dtp_ToDate.Value.Date)
        Cmd.Parameters.AddWithValue("@companyfromdate", Common_Procedures.Company_FromDate)

        RptCondt = CompCondt

        IpColNm1 = ""
        If cbo_Company.Visible = True Then
            If Val(Comp_IdNo) <> 0 Then
                RptCondt = " tZ.Company_IdNo = " & Str(Val(Comp_IdNo))
                IpColNm1 = "[HIDDEN]"
            End If
        Else
            IpColNm1 = "[HIDDEN]"
        End If


        OpAmt = 0
        Cmd.CommandText = "select sum(a.voucher_amount) from voucher_details a, ledger_head b, Company_Head tZ where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date < @fromdate and a.ledger_idno = b.ledger_idno and b.parent_code LIKE '%~6~4~%' and a.company_idno = tZ.company_idno"
        Da = New SqlClient.SqlDataAdapter(Cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                OpAmt = Val(Dt.Rows(0)(0).ToString)
            End If
        End If
        Dt.Clear()


        Cmd.CommandText = "Truncate table ReportTemp"
        Cmd.ExecuteNonQuery()

        '---- Opening
        Cmd.CommandText = "Insert into ReportTemp(Int5, Date1, Meters1, Int6, Name1, Name2, Name3, Name4, Name5, Currency1, Currency2) values (0, @fromdate, 0, 0, 'OPENING', '', 'OPENING', '', '', " & IIf(OpAmt < 0, Math.Abs(OpAmt), 0) & ", " & IIf(OpAmt > 0, OpAmt, 0) & " ) "
        Cmd.ExecuteNonQuery()

        '---- Details
        Cmd.CommandText = "Insert into ReportTemp(Int5, Date1, Meters1, Int6, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Name6) select 1, a.Voucher_Date, a.For_OrderBy, a.Sl_No, a.Voucher_Code, a.Voucher_No, 'By ' + c.ledger_name, Abs(a.voucher_amount), 0, a.narration, a.Voucher_Type, tZ.Company_ShortName from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN ledger_head c ON c.ledger_idno <> 0 and c.parent_code NOT LIKE '%~6~4~%' and a.ledger_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and c.parent_code NOT LIKE '%~6~4~%' and a.ledger_idno <> 0 and a.voucher_amount < 0"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into ReportTemp(Int5, Date1, Meters1, Int6, Name1, Name2, Name3, Currency1, Currency2, Name4, Name5, Name6) select 2, a.Voucher_Date, a.For_OrderBy, a.Sl_No, a.Voucher_Code, a.Voucher_No, 'To ' + c.ledger_name, 0, a.Voucher_Amount, a.narration, a.Voucher_Type, tZ.Company_ShortName from voucher_details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN ledger_head c ON c.ledger_idno <> 0 and c.parent_code NOT LIKE '%~6~4~%' and a.ledger_idno = c.ledger_idno where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.voucher_date between @fromdate and @todate and c.parent_code NOT LIKE '%~6~4~%' and a.ledger_idno <> 0 and a.voucher_amount > 0"
        Cmd.ExecuteNonQuery()


        If Trim(IpColNm1) <> "" Then
            Cmd.CommandText = "Update ReportTemp set Name6 = '" & Trim(IpColNm1) & "'"
            Cmd.ExecuteNonQuery()

        End If

        Da = New SqlClient.SqlDataAdapter("select '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5, Name6, Meters6 from reporttemp Order by Date1, meters1, name5, Int6, name2, name1", con)
        Dtbl1 = New DataTable
        Da.Fill(Dtbl1)

        If Dtbl1.Rows.Count = 0 Then

            Cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
            Cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("select  '" & Trim(CompName) & "' as Company_Name, '" & Trim(CompAdd1) & "' as Company_Address1, '" & Trim(CompAdd2) & "' as Company_Address2, '" & Trim(RptHeading1) & "' as Report_Heading1, '" & Trim(RptHeading2) & "' as Report_Heading2, '" & Trim(RptHeading3) & "' as Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5, Name6, Meters6 from reporttemp Order by Int5, Date1, meters1, name2, name1", con)
            Dtbl1 = New DataTable
            Da.Fill(Dtbl1)

        End If

        RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
        RpDs1.Name = "DataSet1"
        RpDs1.Value = Dtbl1

        RptViewer.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_DayBook.rdlc"

        RptViewer.LocalReport.DataSources.Clear()

        RptViewer.LocalReport.DataSources.Add(RpDs1)

        RptViewer.LocalReport.Refresh()
        RptViewer.RefreshReport()

        RptViewer.Visible = True
        RptViewer.Focus()
        SendKeys.Send("{TAB}")

    End Sub




End Class