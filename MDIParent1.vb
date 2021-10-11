Imports System.Windows.Forms
Imports System.IO
Imports Amazon.S3
Imports Amazon.S3.Model
Imports Amazon.Runtime
Imports Amazon
Imports Amazon.S3.Util
Imports System.Collections.ObjectModel
Imports System.Reflection

Public Class MDIParent1

    Private m_ChildFormNumber As Integer
    Private vShowEntrance_Status As Boolean = False
    Public vFldsChk_All_Status As Boolean = False
    Private MenuVisiblitySetting As New Dictionary(Of String, String)
    Private UserRights As New Dictionary(Of String, String)

    Const AWS_ACCESS_KEY As String = "AKIA3HI2CJWF3PFTLBSW"
    Const AWS_SECRET_KEY As String = "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"

    Private Property s3Client As IAmazonS3

    Dim UpdatingSW As Boolean

    Public Sub New()

        Try

            's3Client = New AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, Region.)
            'Dim clientConfig As New AmazonS3Config
            'ClientConfig.RegionEndpoint = RegionEndpoint.APSouth1

            s3Client = New AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, RegionEndpoint.APSouth1)

        Catch ex As Exception

        End Try


        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnu_Company_Exit.Click
        Close_Form()
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private Sub mnu_Action_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_New.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.new_record()
        End If
    End Sub

    Private Sub mnu_Company_CompanyCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Company_CompanyCreation.Click

        Match_UserRights_WithForm("mnu_Company_CompanyCreation", "Company_Creation")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1171" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1183" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1190" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1201" Then '---- 
            Dim pwd As String = ""

            Dim g As New Password
            g.ShowDialog()

            pwd = Trim(Common_Procedures.Password_Input)

            If Trim(UCase(pwd)) <> "TS123" Then
                MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        Dim f As New Company_Creation
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_LedgerCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_LedgerCreation.Click
        
            Match_UserRights_WithForm("mnu_Master_LedgerCreation", "Ledger_Creation")
            Dim f As New Ledger_Creation
            f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub ExitMenu_Main_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_ExitMenu_Main.Click
        Close_Form()
    End Sub

    Private Sub mnu_Action_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_Save.Click
        If Not Me.ActiveMdiChild Is Nothing Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.save_record()
        End If
    End Sub

   
    Private Sub MDIParent1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim cn1 As SqlClient.SqlConnection
        Dim da1 As SqlClient.SqlDataAdapter
        Dim dt1 As DataTable
        Dim YrCode As String = ""
        Dim ChIndx As Integer = 0

        Me.IsMdiContainer = True

        Try

            lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User.Name))

            Common_Procedures.CompIdNo = 0

            Common_Procedures.FnRange = ""
            Common_Procedures.FnYearCode = ""
            Common_Procedures.Company_FromDate = #4/1/1900#
            Common_Procedures.Company_ToDate = #3/31/1901#

            cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)

            cn1.Open()

            da1 = New SqlClient.SqlDataAdapter("select Financial_Range from FinancialRange_Head", cn1)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0).Item("Financial_Range").ToString) = False Then
                    Common_Procedures.FnRange = dt1.Rows(0).Item("Financial_Range").ToString()
                End If
            End If

            If Trim(Common_Procedures.FnRange) <> "" Then
                YrCode = Microsoft.VisualBasic.Right(Trim(Common_Procedures.FnRange), 4)
                Common_Procedures.FnYearCode = Trim(Mid(Val(YrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(YrCode, 2))
                Common_Procedures.Company_FromDate = CDate("01/04/" & Val(Microsoft.VisualBasic.Right(Common_Procedures.FnRange, 4)) - 1)
                Common_Procedures.Company_ToDate = CDate("31/03/" & Val(Microsoft.VisualBasic.Right(Common_Procedures.FnRange, 4)))

            Else
                MessageBox.Show("Invalid Financial year", "DOES NOT OPEN THIS COMPANY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()
                End

            End If

            Me.Text = Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")         -          " & Common_Procedures.Company_FromDate & "  TO  " & Common_Procedures.Company_ToDate

            Get_Company_SettingsValue(cn1)

            get_Customer_Settings()

            get_User_Rights()

            Common_Procedures.AccountsVoucher_Posting_For_ProfitAndLoss()

            cn1.Close()
            cn1.Dispose()

            vShowEntrance_Status = False




            Common_Procedures.GST_and_VAT_Entry_Status = False

            'If Val(Common_Procedures.settings.CustomerCode) <= "1127" And Trim(Common_Procedures.settings.CustomerCode) <> "" And Trim(Common_Procedures.settings.CustomerCode) <> "0000" Then
            '    Common_Procedures.GST_and_VAT_Entry_Status = True
            'End If

            If Val(Common_Procedures.settings.CustomerCode) = 1201 Then

                mnu_Entry_Other_Delivery.Visible = False
                mnu_Entry_Invoice_Embroidery_Direct.Visible = False
                mnu_Entry_Invoice_Designing.Visible = False
                mnu_Entry_Embroidery_Expense_Entry.Visible = False
                mnu_Entry_Embroidery_Jobwork_Delivery.Visible = False
                mnu_Entry_Embroidery_Jobwork_Receipt.Visible = False
                mnu_Entry_Embroidery_Jobwork_Invoice.Visible = False
                mnu_Entry_GST_Miscellaneous.Visible = False

                mnu_Report_Embroidery_Expenses.Visible = False
                mnu_Report_Earning_Expense_Register.Visible = False
                mnu_Report_Earning_Expense_Register1.Visible = False
                mnu_Report_Embroidery_JobWork_Delivery.Visible = False
                mnu_Report_Embroidery_JobWork_Receipt.Visible = False
                mnu_Report_Embroidery_Jobwork_Invoices.Visible = False
                mnu_Report_Embroidery_General_Delivery.Visible = False
                mnu_Report_Embroidery_Designing_Invoices.Visible = False
                MnuReportRegisterGeneralOtherPurchaseSalesGSTToolStripMenuItem_Main.Visible = False
                Mnu_Report_Embroidery_Jobwork_Receipt_Pending_Register.Visible = False
                Mnu_Report_Embroidery_Jobwork_Receipt_Pending_Register1.Visible = False
                mnu_Tools_GST_Offline_Untitlity.Visible = False

            End If

            If Val(Common_Procedures.settings.CustomerCode) <= "1127" And Trim(Common_Procedures.settings.CustomerCode) <> "" And Trim(Common_Procedures.settings.CustomerCode) <> "0000" Then
                Common_Procedures.GST_and_VAT_Entry_Status = True
            End If

            If Common_Procedures.User.IdNo <> 1 Then

                For Each objMenuItem As ToolStripMenuItem In Me.MenuStrip.Items

                    If objMenuItem.Name <> "mnu_Action_Main" And objMenuItem.Name <> "mnu_WindowsMenu_Main" And
                        objMenuItem.Name <> "mnu_HelpMenu_Main" And objMenuItem.Name <> "mnu_ExitMenu_Main" Then

                        For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1

                            objMenuItem.DropDownItems(IntX).Enabled = False

                            For I As Integer = 0 To Common_Procedures.UR1.UserInfo.GetUpperBound(0)

                                If UCase(Common_Procedures.UR1.UserInfo(I, 0)) = UCase(objMenuItem.DropDownItems(IntX).Name) And Len(Trim(Common_Procedures.UR1.UserInfo(I, 1))) > 1 Then

                                    objMenuItem.DropDownItems(IntX).Enabled = True
                                    GoTo a

                                End If

                            Next

a:

                        Next

                    End If

                Next

            End If

            Match_UserRights_WithForm("mnu_Master_LedgerCreation", "Ledger_Creation")
            Match_UserRights_WithForm("mnu_Master_AreaCreation", "Area_Creation")
            Match_UserRights_WithForm("mnu_Master_ItemCreation", "Item_Creation")
            Match_UserRights_WithForm("mnu_Master_ItemGroupCreation", "ItemGroup_Creation")
            Match_UserRights_WithForm("mnu_Master_CategoryCreation", "Cetegory_Creation")
            Match_UserRights_WithForm("mnu_Master_UnitCreation", "Unit_Creation")
            Match_UserRights_WithForm("mnu_Master_PriceListNAme", "Price_List_Entry_Emb")
            Match_UserRights_WithForm("mnu_Master_MachineCreation", "Machine_Creation")
            Match_UserRights_WithForm("mnu_Master_ColourCreation", "Color_Creation")
            Match_UserRights_WithForm("mnu_Master_ComponentCreation", "Component_Creation")
            Match_UserRights_WithForm("mnu_Master_UserCreation", "User_Creation")
            Match_UserRights_WithForm("mnu_Master_ChequePrintingPosotion", "Cheque_Entry_Print_Positioning")
            Match_UserRights_WithForm("mnu_Master_Expense_Creation", "Expense_Creation")
            Match_UserRights_WithForm("mnu_Master_EmployeeCreation", "Employee_Creation")
            Match_UserRights_WithForm("mnu_Company_CompanyCreation", "Company_Creation")
            Match_UserRights_WithForm("mnu_Company_CompanyCreation", "Company_Creation")
            Match_UserRights_WithForm("mnu_Entry_GST_Miscellaneous_Click", "Other_GST_Entry")

            mnu_Company_SelectCompany.Enabled = True
            mnu_Company_ChangePeriod.Enabled = True
            mnu_Company_Exit.Enabled = True

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1222" Or Val(Trim(UCase(Common_Procedures.settings.CustomerCode))) > 5000 Or Val(Trim(UCase(Common_Procedures.settings.CustomerCode))) < 1000 Then '---- MANNARAI COMMON EFFLUENT TRATMENT PLANT PVT. LTD (MANNARAI)
        Apply_Form_Menu_Visiblity_Settings()
        'End If

    End Sub


    


    Private Sub MDIParent1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Dim cn1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Dim cmd As New SqlClient.SqlCommand
        cn1.Open()
        cmd.Connection = cn1

        If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "AMA" Then

            Try

                'cmd.CommandText = "exec msdb.dbo.rds_backup_database " & _
                '                  " @source_db_name='" & Common_Procedures.DataBaseName & "'," & _
                '                  " @s3_arn_to_backup_to=':::novabizdbfiles/" & Common_Procedures.DataBaseName & "_1'," & _
                '                  " @overwrite_S3_backup_file=1, " & _
                '                  " @type='differential';"

                cmd.CommandText = "exec msdb.dbo.rds_backup_database " & _
                                  "@source_db_name='" & Common_Procedures.DataBaseName & "'," & _
                                  "@s3_arn_to_backup_to='arn:aws:s3:::novabizdbfiles/" & Common_Procedures.DataBaseName & "'," & _
                                   "@overwrite_S3_backup_file=1," & _
                                  "@type='FULL';"

                '"@kms_master_key_arn='arn:aws:kms:ap-south-1:771538177419:key/086ab9d9-a6c0-4a43-af79-6ecbf91db666'," & _


                cmd.ExecuteNonQuery()

                'cmd.CommandText = "exec msdb.dbo.rds_backup_database " & _
                '                 " @source_db_name='" & Common_Procedures.CompanyDetailsDataBaseName & "'," & _
                '                 " @s3_arn_to_backup_to=':::novabizdbfiles/" & Common_Procedures.CompanyDetailsDataBaseName & "'," & _
                '                 " @overwrite_S3_backup_file=1, " & _
                '                 " @type='differential';"

                cmd.CommandText = "exec msdb.dbo.rds_backup_database " & _
                                  "@source_db_name='" & Common_Procedures.CompanyDetailsDataBaseName & "'," & _
                                  "@s3_arn_to_backup_to='arn:aws:s3:::novabizdbfiles/" & Common_Procedures.CompanyDetailsDataBaseName & "'," & _
                                  "@overwrite_S3_backup_file=1," & _
                                  "@type='FULL';"

                '"@kms_master_key_arn='arn:aws:kms:ap-south-1:771538177419:key/086ab9d9-a6c0-4a43-af79-6ecbf91db666'," & _


                cmd.ExecuteNonQuery()

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@BackupDate", Date.Today)

                cmd.CommandText = "update settings_head set AutoBackUp_Date = @BackupDate"
                cmd.ExecuteNonQuery()

                cmd.Dispose()

                cn1.Close()
                cn1.Dispose()

            Catch ex As Exception

                MsgBox(ex.Message & ". Backup Fails")
                Application.Exit()

            End Try

        Else


            If DateDiff("d", Common_Procedures.settings.AutoBackUp_Date, Date.Today) > 0 Then

                Common_Procedures.Sql_AutoBackUP(Common_Procedures.DataBaseName)

                Common_Procedures.Sql_AutoBackUP(Common_Procedures.CompanyDetailsDataBaseName)

                'Dim cn1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
                'Dim cmd As New SqlClient.SqlCommand

                'cn1.Open()

                'cmd.Connection = cn1

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@BackupDate", Date.Today)

                cmd.CommandText = "update settings_head set AutoBackUp_Date = @BackupDate"
                cmd.ExecuteNonQuery()

                cmd.Dispose()

                cn1.Close()
                cn1.Dispose()

            End If

        End If

        If vShowEntrance_Status = False And Common_Procedures.vShowEntrance_Status_ForCC = False Then
            Application.Exit()
        End If

    End Sub

    Private Sub Get_Company_SettingsValue(ByVal Cn1 As SqlClient.SqlConnection)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            Call FieldCheck_Company_Db(Cn1)

            Common_Procedures.settings.CompanyName = ""
            Common_Procedures.settings.AutoBackUp_Date = #1/1/1900#

            da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", Cn1)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0).Item("C_Name").ToString) = False Then
                    Common_Procedures.settings.CompanyName = dt1.Rows(0).Item("C_Name").ToString()
                End If
                If IsDBNull(dt1.Rows(0).Item("AutoBackUp_Date").ToString) = False Then
                    If IsDate(dt1.Rows(0).Item("AutoBackUp_Date").ToString) = True Then
                        Common_Procedures.settings.AutoBackUp_Date = dt1.Rows(0).Item("AutoBackUp_Date").ToString()
                    End If
                End If
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN GETTING SETTINGS VALUES...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub FieldCheck_Company_Db(ByVal cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        On Error Resume Next

        cmd.Connection = cn1

        cmd.CommandText = "Alter table Settings_Head add Auto_SlNo [int] IDENTITY (1, 1) NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add C_Name varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set C_Name = '' Where C_Name is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Company_Head add Company_Type varchar(50) default 'ACCOUNT'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Company_Head set Company_Type = 'ACCOUNT' Where Company_Type is Null"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Company_Head set Company_Type = 'ACCOUNT' Where Company_Type = ''"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add AutoBackUp_Date smalldatetime"
        cmd.ExecuteNonQuery()

        da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", cn1)
        dt1 = New DataTable
        da1.Fill(dt1)

        If dt1.Rows.Count = 0 Then
            cmd.CommandText = "truncate table Settings_Head"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into Settings_Head(C_Name) values ('') "
            cmd.ExecuteNonQuery()
        End If

        dt1.Dispose()
        da1.Dispose()

        cmd.Dispose()

    End Sub

    Private Sub get_Customer_Settings()

        Common_Procedures.settings.PreviousEntryDate_ByDefault = 0

        Common_Procedures.settings.Payroll_Status = 0

        Common_Procedures.settings.InvoicePrint_Format = ""

        Common_Procedures.settings.EntrySelection_Combine_AllCompany = 0

        Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        Common_Procedures.settings.Report_Show_CurrentDate_IN_ToDate = 1

        Common_Procedures.settings.NegativeStock_Restriction = 0

        Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status = 0

        Common_Procedures.settings.Printing_Show_PrintDialogue = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1002" Then '---- Peacock Traders(Tirupur)
            Common_Procedures.settings.Jurisdiction = "Tirupur"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1003" Then '---- SenthilNathan Spinners (Karumanthapatti)
            Common_Procedures.settings.Jurisdiction = "Coimbatore"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1004" Then '---- G.B Tape (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Then '---- BNC Garments (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1011" Then '---- Chellam Batteries (Thekkalur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.NegativeStock_Restriction = 1

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Then '---- Rathinam Fabrics (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1014" Then '---- SriRam Designer (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then '---- Rajendra Textiles (Somanur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1039" Then '---- Senthil Kumar Industries (Coimbatore)
            Common_Procedures.settings.Jurisdiction = "Coimbatore"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1048" Then '---- Saara Creations (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1051" Then '---- A.r Knotting (Somanur)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1053" Then '---- RR Business Solutions (Chennai)
            Common_Procedures.settings.Jurisdiction = "Chennai"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1054" Then '---- Amman Traders(Tirupur)
            Common_Procedures.settings.Jurisdiction = "Tirupur"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1068" Then '---- Gee Fashion (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1062" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2002" Then '---- Sobika Fashion (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1070" Then '----Billan Cotton (vijayamangalam)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1071" Then '---- Kovai Tirupur District Weaver Sangam  (Somanur)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1073" Then '---- Rainbow Silks(Somanur)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1077" Then '---- Sri vinayaka Silks(Somanur)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1079" Then '---- JSR Garments (Tantex) (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TRICHY"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1080" Then '---- Adhiswara Traders (vguard) (Tirupur) 
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1085-----" Then '---- Raju Milks (Chennai) 
            Common_Procedures.settings.Jurisdiction = "Chennai"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1086-----" Then '---- Raju Milks (Chennai) 
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1091" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1196" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1154" Then '---- Sri Arul Engineering Works (Avinashi)

            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1092" Then '---- Maruthi Stores (Thekkalur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1095" Then '---- Sri karpaka vinayakar offset printers
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1096" Then '---- Gowri Jwellary (Avinashi)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

            Common_Procedures.settings.SMS_Provider_SenderID = "SRIGTM"
            Common_Procedures.settings.SMS_Provider_Key = "3586603D24142C"
            Common_Procedures.settings.SMS_Provider_RouteID = "134"
            Common_Procedures.settings.SMS_Provider_Type = "text"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1103" Then '---- SRI BALAJI GARMENTS (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then '---- GAJAKHARNAA TRADERS (Somanur)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1108" Then  '--- Sri Selvanayaki Venture (Santhosh hardware)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1109" Then  '--- Auro Satya (Tirupur ) Hanger
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1117" Then  '--- Lakshmi Design (Tirupur ) Embroidery
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1119" Then  '--- Alphonsa Cards (Tirupur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.Payroll_Status = 1
            'Tsoft Sms
            Common_Procedures.settings.SMS_Provider_SenderID = "ALCARD"


            ' ''---sms.shamsoft.in
            'Common_Procedures.settings.SMS_Provider_SenderID = "SRRSIZ"
            'Common_Procedures.settings.SMS_Provider_Key = "458510BDFE6C6C"
            'Common_Procedures.settings.SMS_Provider_RouteID = "134"
            'Common_Procedures.settings.SMS_Provider_Type = "text"


            'Common_Procedures.settings.SMS_Provider_SenderID = "TSOFTS"
            'Common_Procedures.settings.SMS_Provider_Key = "355C7A0B5595B2"
            'Common_Procedures.settings.SMS_Provider_RouteID = "134"
            'Common_Procedures.settings.SMS_Provider_Type = "text"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1123" Then  '--- Shanthi Sizing (Somanur)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.Payroll_Status = 0

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1128" Then  '---Sri laxmi plastics (Tirupur ) Hanger
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.Payroll_Status = 1

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then  '---RADO  (Tirupur ) Garments
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.Payroll_Status = 0

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2001" Then '---- Demo - Elpro Chem for Vasanth by Deva (Chennai)
            Common_Procedures.settings.Jurisdiction = "CHENNAI"

            Common_Procedures.settings.SMS_Provider_SenderID = "TSOFTS"
            Common_Procedures.settings.SMS_Provider_Key = "355C7A0B5595B2"
            Common_Procedures.settings.SMS_Provider_RouteID = "134"
            Common_Procedures.settings.SMS_Provider_Type = "text"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1137" Then  '---  NATRAJ KNIT WEAR (SHANTHI THEATRE) 
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.Payroll_Status = 0

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1141" Then  '---  SRI SABARI TRADERS (TIRUPUR) 
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1142" Then  '---  SLP
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.Printing_Show_PrintDialogue = 1

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1149" Then  '---  NATRAJ KNIT WEAR (SHANTHI THEATRE) 
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
            Common_Procedures.settings.Payroll_Status = 0

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1150" Then  '---  SRIKA DESIGNS (TIRUPUR) 
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1154" Then  '---  Deksha Engineering (GANAPATHY - COVAI) 
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1156" Then '---- S.K Computers (Somanur)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1501" Then  '---  subi exports
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1502" Then  '---  CREAT PRINTS
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1503" Then  '---  LEO GEM KNITS (TIRUPUR)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Then  '---  GOLDEN TEXTILES PRINTERS (TIRUPUR)
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1506" Then  '--- ITech
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1167" Then  '---  F FASHIONS (COIMBATORE)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1171" Then '---- krishna general Stores ()
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1505" Then  '---   PRIN TECH
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1174" Then  '---  wintech garments
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Then '---- SREE GANESH AGENCIES (Somanur)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1183" Then '---- SREE GANESH AGENCIES (Somanur)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1190" Then '---- CITY BOOK CENTER
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1193" Then '----  USR TWO WHEELER STAND
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1194" Then  '---  VINAYAGA ENGINEERING (COIMBATORE)
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1196" Then '---- Maha Automation
            Common_Procedures.settings.Jurisdiction = "COIMBATORE"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1200" Then  '---Sri annai Design (Tirupur ) 
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1201" Then  '--- SWASTHICK KNITT (Tirupur ) Embroidery
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1217" Then  '--- alpha
            Common_Procedures.settings.Jurisdiction = "TIRUPUR"

        End If


    End Sub

    Public Sub Close_Form()
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As System.Windows.Forms.Form = Me.ActiveMdiChild
            f.Close()
        Else
            Me.Close()
            Application.Exit()
        End If
    End Sub

    Private Sub ToolBar_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolBar_Exit.Click
        Close_Form()
    End Sub

    Private Sub mnu_Action_MoveFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_MoveFirst.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim F As Interface_MDIActions = Me.ActiveMdiChild
            F.movefirst_record()
        End If
    End Sub

    Private Sub mnu_Action_MoveNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_MoveNext.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.movenext_record()
        End If
    End Sub

    Private Sub mnu_Action_MovePrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_MovePrevious.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.moveprevious_record()
        End If
    End Sub

    Private Sub mnu_Action_MoveLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_MoveLast.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.movelast_record()
        End If
    End Sub

    Private Sub mnu_Action_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_Delete.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.delete_record()
        End If
    End Sub

    Private Sub mnu_Action_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_Open.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.open_record()
        End If
    End Sub

    Private Sub mnu_Action_Filter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_Filter.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.filter_record()
        End If
    End Sub

    Private Sub mnu_Master_ItemGroupCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_ItemGroupCreation.Click
        Match_UserRights_WithForm("mnu_Master_ItemGroupCreation", "ItemGroup_Creation")
        Dim f As New ItemGroup_Creation
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_UnitCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_UnitCreation.Click
        Match_UserRights_WithForm("mnu_Master_UnitCreation", "Unit_Creation")
        Dim F As New Unit_Creation
        F.MdiParent = Me
        F.Show()
    End Sub

    Private Sub mnu_Master_ItemCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_ItemCreation.Click

        Match_UserRights_WithForm("mnu_Master_ItemCreation", "Item_Creation")

        Dim F As New Item_Creation
        F.MdiParent = Me
        F.Show()

    End Sub

    Private Sub mnu_Entry_PurchaseEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Common_Procedures.CompIdNo = 0

        'If Trim(Common_Procedures.settings.CustomerCode) = "1003" Then '---- SenthilNathan Spinners (Karumanthapatti)
        '    Dim F3 As New Spinning_Purchase_Entry
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1013" Then
        '    Dim F3 As New Purchase_Garments
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1048" Then

        '    Dim F3 As New Saara_Delivery
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1068" Then ' ---------------GEE FASHIONS

        '    Dim F3 As New Purchase_Garments2
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1079" Then

        '    Dim F5 As New Purchase_Garments1
        '    F5.MdiParent = Me
        '    F5.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F5.Close()
        '        F5.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1080" Then
        '    Dim F5 As New Purchase_Entry_Simple1
        '    F5.MdiParent = Me
        '    F5.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F5.Close()
        '        F5.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1092" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1171" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1183" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1190" Then
        '    Dim F5 As New Purchase_Entry_Simple1
        '    F5.MdiParent = Me
        '    F5.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F5.Close()
        '        F5.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1108" Then
        '    Dim F3 As New Purchase_Entry_BatchNo
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If

        'Else
        '    Dim F1 As New Purchase_Entry_Simple
        '    'Dim F1 As New Purchase_Entry
        '    F1.MdiParent = Me
        '    F1.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F1.Close()
        '        F1.Dispose()
        '    End If

        'End If

    End Sub

    Private Sub mnu_Action_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_Print.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim F As Interface_MDIActions = Me.ActiveMdiChild
            Common_Procedures.Print_OR_Preview_Status = 1
            F.print_record()
        End If
    End Sub

    Private Sub mnu_Action_Preview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_Preview.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim F As Interface_MDIActions = Me.ActiveMdiChild
            Common_Procedures.Print_OR_Preview_Status = 2
            F.print_record()
        End If
    End Sub

    Private Sub mnu_Action_Insert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Action_Insert.Click
        If Not (Me.ActiveMdiChild Is Nothing) Then
            Dim f As Interface_MDIActions = Me.ActiveMdiChild
            f.insert_record()
        End If
    End Sub

    Private Sub mnu_Company_SelectCompany_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Company_SelectCompany.Click
        vShowEntrance_Status = True
        Me.Close()
        Entrance.Show()
    End Sub

    Private Sub mnu_Reports_SalesRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        If Trim(Common_Procedures.settings.CustomerCode) = "1008" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then
            Common_Procedures.RptInputDet.ReportName = "Garments Invoice Register"
            Common_Procedures.RptInputDet.ReportHeading = "Invoice Register"
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1011" Or Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            Common_Procedures.RptInputDet.ReportName = "Invoice Register"
            Common_Procedures.RptInputDet.ReportHeading = "Invoice Register"
        Else
            Common_Procedures.RptInputDet.ReportName = "sales register"
            Common_Procedures.RptInputDet.ReportHeading = "Sales Register"
        End If
        If Trim(Common_Procedures.settings.CustomerCode) = "1080" Then
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,SM"
        Else

            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        End If
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_StockDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Trim(Common_Procedures.settings.CustomerCode) = "1013" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1079" Then
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Stock"
            Common_Procedures.RptInputDet.ReportName = "Garments Stock Details"
            Common_Procedures.RptInputDet.ReportHeading = "STOCK DETAILS"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,I,SZ"
            f.MdiParent = Me
            f.Show()
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1068" Then
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Stock"
            Common_Procedures.RptInputDet.ReportName = "Garments2 Stock Details"
            Common_Procedures.RptInputDet.ReportHeading = "STOCK DETAILS"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,I,SZ"
            f.MdiParent = Me
            f.Show()
        Else
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Stock"
            Common_Procedures.RptInputDet.ReportName = "Stock Details"
            Common_Procedures.RptInputDet.ReportHeading = "STOCK DETAILS"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,I"
            f.MdiParent = Me
            f.Show()
        End If



    End Sub

    Private Sub mnu_Reports_StockSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1013" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1079" Then
            Common_Procedures.RptInputDet.ReportName = "Garments Stock Summary"
            Common_Procedures.RptInputDet.ReportHeading = "STOCK SUMMARY"
            Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,I,SZ"

        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1068" Then

            Common_Procedures.RptInputDet.ReportName = "Garments2 Stock Summary"
            Common_Procedures.RptInputDet.ReportHeading = "STOCK SUMMARY"
            Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,IG"

        Else
            Common_Procedures.RptInputDet.ReportName = "Stock Summary"
            Common_Procedures.RptInputDet.ReportHeading = "STOCK SUMMARY"
            Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,IG"

        End If

        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_Purchase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_Purchase.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "Purc"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_Sales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_Sales.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "Sale"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_Payment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_Payment.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "Pymt"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_Receipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_Receipt.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "Rcpt"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_Contra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_Contra.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "Cntr"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_Journal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_Journal.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "Jrnl"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_CreditNote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_CreditNote.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "CrNt"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_DebitNote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_DebitNote.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "DbNt"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Voucher_PettiCash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Voucher_PettiCash.Click
        Dim f As New Voucher_Entry
        Common_Procedures.VoucherType = "PtCs"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_PurchaseRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_PurchaseRegister.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Register"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_SalesEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try

            Common_Procedures.CompIdNo = 0
            Common_Procedures.SalesEntryType = ""

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT LOAD FORM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub mnu_Tools_FieldsCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_FieldsCheck.Click

        Dim CN1 As New SqlClient.SqlConnection

        CN1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        CN1.Open()

        FieldsCheck.vFldsChk_All_Status = False
        FieldsCheck.vFldsChk_From_CompGroupCreation_Status = False
        FieldsCheck.FieldsCheck_1(CN1, Me)
        Common_Procedures.Default_Unit_Creation(CN1)
        Common_Procedures.Default_Value_Updation(CN1)
        FieldsCheck.vFldsChk_All_Status = False
        FieldsCheck.vFldsChk_From_CompGroupCreation_Status = False

        CN1.Close()
        CN1.Dispose()

        MsgBox("Field Check Complete")

    End Sub

    Private Sub mnu_Opening_ItemOpeningStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Opening_ItemOpeningStock.Click

        Match_UserRights_WithForm("mnu_Opening_ItemOpeningStock", "Item_OpeningStock")

        Dim f As New Item_OpeningStock
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Accounts_SingleLedger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_SingleLedger.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Single Ledger A/c"
        Common_Procedures.RptInputDet.ReportHeading = "Ledger Statement"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,*L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_PurchaseDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_PurchaseDetails.Click
       
       
    End Sub

    Private Sub mnu_Accounts_BalanceSheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_BalanceSheet.Click
        Common_Procedures.CompIdNo = 0
        Dim f As New Balance_Sheet
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Accounts_GroupLedger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_GroupLedger.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Group Ledger"
        Common_Procedures.RptInputDet.ReportHeading = "Group Ledger"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,G"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Accounts_OpeningTB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_OpeningTB.Click

        Dim f As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Opening TB"
        Common_Procedures.RptInputDet.ReportHeading = "Opening Trial Balance"
        Common_Procedures.RptInputDet.ReportInputs = "Z"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Accounts_GeneralTB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_GeneralTB.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "General TB"
        Common_Procedures.RptInputDet.ReportHeading = "General Trial Balance"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Accounts_GroupTB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_GroupTB.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Group TB"
        Common_Procedures.RptInputDet.ReportHeading = "Group Trial Balance"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,G"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_FinalTB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_FinalTB.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Final TB"
        Common_Procedures.RptInputDet.ReportHeading = "Final Trial Balance"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Opening_LedgerAmountBalance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Opening_LedgerAmountBalance.Click

        Match_UserRights_WithForm("mnu_Opening_LedgerAmountBalance", "Opening_Balance_Stock")

        If Val(Common_Procedures.User.IdNo) <> 1 And Trim(Common_Procedures.UR.Ledger_OpeningBalance) = "" Then MessageBox.Show("You have No Rights", "DOES NOT SHOW ENTRY...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Common_Procedures.CompIdNo = 0

        'Dim f As New Opening_Balance
        Dim f As New Opening_Balance_Stock
        f.MdiParent = Me
        f.Show()

        If Val(Common_Procedures.CompIdNo) = 0 Then
            f.Close()
            f.Dispose()
        End If

    End Sub

    Private Sub mnu_Reports_SalesDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Trim(Common_Procedures.settings.CustomerCode) = "1003" Then '---- SenthilNathan Spinners (Karumanthapatti)
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "Spinning Invoice Details"
            Common_Procedures.RptInputDet.ReportHeading = "Invoice Details"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
            f.MdiParent = Me
            f.Show()
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1008" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "Garments Invoice Details"
            Common_Procedures.RptInputDet.ReportHeading = "Invoice Details"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
            f.MdiParent = Me
            f.Show()
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1011" Or Trim(Common_Procedures.settings.CustomerCode) = "1039" Then
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "Invoice Details"
            Common_Procedures.RptInputDet.ReportHeading = "Invoice Details"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
            f.MdiParent = Me
            f.Show()
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1108" Then
            Dim f1 As New Report_Details_1
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "Sales BatchNo Details"
            Common_Procedures.RptInputDet.ReportHeading = "Sales BatchNo Details"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I,BTCHSN"
            f1.MdiParent = Me
            f1.Show()
        Else
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "Sales Details"
            Common_Procedures.RptInputDet.ReportHeading = "Sales Details"

            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
            f.MdiParent = Me
            f.Show()
        End If



    End Sub

    Private Sub mnu_Reports_MinimumStockLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Minimum Stock Level"
        Common_Procedures.RptInputDet.ReportHeading = "MINIMUM STOCK LEVEL"
        Common_Procedures.RptInputDet.ReportInputs = ""
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_ItemExcessShort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub mnu_Master_VarietyCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Variety_Creation
        'f.MdiParent = Me
        'f.Show()
    End Sub

    Private Sub mnu_Company_BackUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Company_BackUp.Click

        
        Dim cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim db_name As String
        Dim Fl_Name As String
        Dim Fl_Name1 As String

        SaveFileDialog1.ShowDialog()
        Fl_Name = SaveFileDialog1.FileName

        If Trim(Fl_Name) = "" Then
            MessageBox.Show("Invalid Backup FileName", "DOES NOT PREPARE BACKUP...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If InStr(1, Fl_Name, ".") = 0 Then
            Fl_Name = Trim(Fl_Name)   '& ".bak"
            Fl_Name1 = Trim(Fl_Name) & ".tssl"
        Else
            Fl_Name = Microsoft.VisualBasic.Left(Fl_Name, InStr(Fl_Name, ".") - 1)
            Fl_Name1 = Trim(Fl_Name) & ".tssl"
        End If

        cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        cn1.Open()
        db_name = cn1.Database
        cn1.Close()

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
        cn1.Open()

        cmd.Connection = cn1

        cmd.CommandText = "BACKUP DATABASE " & Trim(db_name) & " TO DISK = '" & Trim(Fl_Name) & "' WITH INIT"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "BACKUP DATABASE " & Trim(db_name) & " TO DISK = '" & Trim(Fl_Name1) & "' WITH INIT"
        cmd.ExecuteNonQuery()


        cmd.Dispose()

        cn1.Close()
        cn1.Dispose()

        MessageBox.Show("Backup Prepared!" & vbCrLf & Trim(Fl_Name), "FOR BACKUP...", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub mnu_Company_Restore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Company_Restore.Click

        Dim cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim BackUp_FlName As String
        Dim Resdb_name As String
        Dim ResDB_MDF_Name As String, ResDB_LDF_Name As String
        Dim ResDB_MDF_FilePath As String, ResDB_LDF_FilePath As String
        Dim BackUp_File_MDF_Name As String, BackUp_File_LDFName As String


        OpenFileDialog1.ShowDialog()
        BackUp_FlName = OpenFileDialog1.FileName

        If Trim(BackUp_FlName) = "" Then
            MessageBox.Show("Invalid FileName", "DOES NOT RESTORE DATABASE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        cn1.Open()
        Resdb_name = cn1.Database
        cn1.Close()

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
        cn1.Open()

        ResDB_MDF_Name = "" : ResDB_LDF_Name = ""
        ResDB_MDF_FilePath = "" : ResDB_LDF_FilePath = ""

        Da1 = New SqlClient.SqlDataAdapter("select * from sysdatabases where name = '" & Trim(Resdb_name) & "'", cn1)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("FileName").ToString) = False Then

                ResDB_MDF_Name = Dt1.Rows(0).Item("Name").ToString

                If InStr(1, LCase(ResDB_MDF_Name), "_data") > 0 Then
                    ResDB_LDF_Name = Replace(LCase(ResDB_MDF_Name), "_data", "_log")
                Else
                    ResDB_LDF_Name = Trim(LCase(ResDB_MDF_Name)) & "_log"
                End If


                ResDB_MDF_FilePath = Dt1.Rows(0).Item("FileName").ToString

                If InStr(1, LCase(ResDB_MDF_FilePath), "_data.mdf") > 0 Then
                    ResDB_LDF_FilePath = Replace(LCase(ResDB_MDF_FilePath), "_data.mdf", "_log.ldf")
                Else
                    ResDB_LDF_FilePath = Replace(LCase(ResDB_MDF_FilePath), ".mdf", "_log.ldf")
                End If

            End If

        End If

        'Call Delete_DataBase(Resdb_name)
        '
        'Call Delete_Mdf_Ldf_File(DbName)
        '
        'Rs2.Open("select * from master..sysdatabases where name = '" & Trim(Resdb_name) & "'", Cn2, adOpenStatic, adLockReadOnly)
        'If Rs2.BOF And Rs2.EOF Then
        '    If Trim(ResDB_MDF_Name) <> "" Then
        '        Cn2.Execute("Create Database " & Trim(Resdb_name) & " ON (Name = '" & Trim(ResDB_MDF_Name) & "', FileName = '" & Trim(ResDB_MDF_FilePath) & "') LOG ON (Name = '" & Trim(ResDB_LDF_Name) & "', FileName = '" & Trim(ResDB_LDF_FilePath) & "')")
        '    Else
        '        Cn2.Execute("create database " & Trim(Resdb_name))
        '    End If
        '    If Show_ProgressBar = True Then Call Change_ProgressBarValue(FrmProgressBar)
        'End If
        'Rs2.Close()

        BackUp_File_MDF_Name = ""
        BackUp_File_LDFName = ""

        Da1 = New SqlClient.SqlDataAdapter("exec(N'RESTORE FILELISTONLY FROM DISK=N''" & Trim(BackUp_FlName) & "'' WITH FILE=1')", cn1)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("LogicalName").ToString) = False Then
                If Trim(UCase(Dt1.Rows(0).Item("Type").ToString)) = "D" Then
                    BackUp_File_MDF_Name = (Dt1.Rows(0).Item("LogicalName").ToString)
                Else
                    BackUp_File_LDFName = (Dt1.Rows(0).Item("LogicalName").ToString)
                End If
            End If
            If IsDBNull(Dt1.Rows(1).Item("LogicalName").ToString) = False Then
                If Trim(UCase(Dt1.Rows(1).Item("Type").ToString)) = "D" Then
                    BackUp_File_MDF_Name = (Dt1.Rows(1).Item("LogicalName").ToString)
                Else
                    BackUp_File_LDFName = (Dt1.Rows(1).Item("LogicalName").ToString)
                End If
            End If
        End If

        cmd.Connection = cn1

        'cmd.CommandText = "exec(N'RESTORE FILELISTONLY FROM DISK=N''f:\abc_backup.bak'' WITH FILE=1')"

        'cmd.CommandText = "ALTER DATABASE " & Trim(Resdb_name) & " SET SINGLE_USER WITH ROLLBACK(IMMEDIATE)"
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER DATABASE " & Trim(Resdb_name) & " SET OFFLINE WITH ROLLBACK IMMEDIATE"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "RESTORE DATABASE " & Trim(Resdb_name) & " FROM DISK = '" & BackUp_FlName & "' WITH MOVE '" & BackUp_File_MDF_Name & "' TO '" & ResDB_MDF_FilePath & "', MOVE '" & BackUp_File_LDFName & "' TO '" & ResDB_LDF_FilePath & "', REPLACE"
        cmd.ExecuteNonQuery()

        'RESTORE DATABASE tsoft_trading_nt10_2 FROM DISK = 'I:\TSOFT\Trading\Trading_NT10\tsoft_trading_nt10_1_Backup_20Apr15' WITH MOVE 'tsoft_trading_nt10_1' TO 'D:\MSSQL2005\DATA\MSSQL.1\MSSQL\DATA\tsoft_trading_nt10_2.mdf', MOVE 'tsoft_trading_nt10_1_log' TO 'd:\mssql2005\data\mssql.1\mssql\data\tsoft_trading_nt10_2_log.ldf', REPLACE 

        Dt1.Dispose()
        Da1.Dispose()

        cmd.Dispose()

        cn1.Close()
        cn1.Dispose()

        MessageBox.Show("Restores Sucessfully!!", "FOR DATABASE RESTORE...", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub mnu_Master_AreaCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_AreaCreation.Click
        Match_UserRights_WithForm("mnu_Master_AreaCreation", "Area_Creation")
        Dim f As New Area_Creation
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_WarrantyCheckingReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportName = "Sales Warranty Report"
        Common_Procedures.RptInputDet.ReportHeading = "WARRANTY REPORT"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,P,I,SL"
        'Common_Procedures.RptInputDet.ReportInputs = "2DT,P,I,SL,PH"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_WasteCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Waste_Creation
        'f.MdiParent = Me
        'f.Show()
    End Sub

    Private Sub mnu_Entry_WasteSalesEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F3 As New Spinning_WasteSales_Entry
        'F3.MdiParent = Me
        'F3.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F3.Close()
        '    F3.Dispose()
        'End If
    End Sub

    Private Sub mnu_Master_UserCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_UserCreation.Click

        Match_UserRights_WithForm("mnu_Master_UserCreation", "User_Creation")

        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f As New User_Creation
            f.MdiParent = Me
            f.Show()

        Else
            MessageBox.Show("You have no rights for user creation" & Chr(13) & "Only admin can change user rights", "INVALID AUTHORISATION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

    End Sub

    Private Sub StatusBar_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_New.Click
        mnu_Action_New_Click(sender, e)
    End Sub

    Private Sub StatusBar_Insert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Insert.Click
        mnu_Action_Insert_Click(sender, e)
    End Sub

    Private Sub StatusBar_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Save.Click
        mnu_Action_Save_Click(sender, e)
    End Sub

    Private Sub StatusBar_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Open.Click
        mnu_Action_Open_Click(sender, e)
    End Sub

    Private Sub StatusBar_Filter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Filter.Click
        mnu_Action_Filter_Click(sender, e)
    End Sub

    Private Sub StatusBar_First_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_First.Click
        mnu_Action_MoveFirst_Click(sender, e)
    End Sub

    Private Sub StatusBar_Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Next.Click
        mnu_Action_MoveNext_Click(sender, e)
    End Sub

    Private Sub StatusBar_Previous_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Previous.Click
        mnu_Action_MovePrevious_Click(sender, e)
    End Sub

    Private Sub StatusBar_Last_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Last.Click
        mnu_Action_MoveLast_Click(sender, e)
    End Sub

    Private Sub StatusBar_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Delete.Click
        mnu_Action_Delete_Click(sender, e)
    End Sub

    Private Sub StatusBar_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Print.Click
        mnu_Action_Print_Click(sender, e)
    End Sub

    Private Sub StatusBar_Preview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Preview.Click
        mnu_Action_Preview_Click(sender, e)
    End Sub

    Private Sub StatusBar_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBar_Close.Click
        Close_Form()
    End Sub

    Private Sub mnu_Master_SizeCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Size_Creation
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_TransportCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Transport_Creation
        'f.MdiParent = Me
        'f.Show()
    End Sub

    Private Sub mnu_Accounts_PartyBalanceMonthwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_PartyBalanceMonthwise.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Party Balance - MonthWise"
        Common_Procedures.RptInputDet.ReportHeading = "Party OutStanding List"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_PartyBalanceDayWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_PartyBalanceDayWise.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Party Balance - DayWise"
        Common_Procedures.RptInputDet.ReportHeading = "Party OutStanding List"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,L,DY"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_PartyBalanceBillwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_PartyBalanceBillwise.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Party Balance - BillWise"
        Common_Procedures.RptInputDet.ReportHeading = "Party OutStanding List"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,L,AR"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Annexure1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Annexure-I"
        Common_Procedures.RptInputDet.ReportHeading = "Annexure-I"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Annexure2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Annexure-II"
        Common_Procedures.RptInputDet.ReportHeading = "Annexure-II"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Sales_Summary_Party_and_Item_Wise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Summary"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Sales_Summary_PartyWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Summary PartyWise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Summary PartyWise"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Sales_Summary_ItemWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Summary ItemWise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Summary ItemWise"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Item_InWard_and_OutWard_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Item InWard and OutWard Register"
        Common_Procedures.RptInputDet.ReportHeading = "Item InWard && OutWard Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_SalesReturnEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub CAnnexureIExcelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "AnnexureI-Excel"
        Common_Procedures.RptInputDet.ReportHeading = "Annexure-I"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub DAnnexureIIExcelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "AnnexureII-Excel"
        Common_Procedures.RptInputDet.ReportHeading = "Annexure-II"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_JobworkEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Common_Procedures.CompIdNo = 0

        'Dim F2 As New Jobwork_Entry
        'F2.MdiParent = Me
        'F2.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F2.Close()
        '    F2.Dispose()
        'End If

    End Sub

    Private Sub mnu_Reports_JobWorkRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "jobwork entry details"
        Common_Procedures.RptInputDet.ReportHeading = "JobWork Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Knotting_KnottingEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Common_Procedures.CompIdNo = 0

        'Dim F2 As New Knotting_Entry
        'F2.MdiParent = Me
        'F2.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F2.Close()
        '    F2.Dispose()
        'End If
    End Sub

    Private Sub mnu_Knotting_InvoiceEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Common_Procedures.CompIdNo = 0

        'Dim F2 As New Knotting_Bill_Entry
        'F2.MdiParent = Me
        'F2.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F2.Close()
        '    F2.Dispose()
        'End If
    End Sub

    Private Sub mnu_KnottingReports_KnottingRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Knotting Entry Details"
        Common_Procedures.RptInputDet.ReportHeading = "Knotting Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_KnottingReports_KnottingSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Knotting Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Knotting Summary"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_KnottingReports_KnottingBillDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "knotting bill details"
        Common_Procedures.RptInputDet.ReportHeading = "Invoice Details"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_KnottingReports_KnottingBillSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "knotting bill Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Invocie Summary"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_KnottingReports_KnottingBillPending_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "knotting bill pending register"
        Common_Procedures.RptInputDet.ReportHeading = "Invoice Pending Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub FKnottingBillPendingSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "knotting bill pending Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Invoice Pending Summary"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_KnottingReports_KnottingBill_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "knotting bill register"
        Common_Procedures.RptInputDet.ReportHeading = "Invoice Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_ClothSales_ClothSalesEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F5 As New Purchase_Entry_Simple1
        'F5.MdiParent = Me
        'F5.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F5.Close()
        '    F5.Dispose()
        'End If

    End Sub

    Private Sub mnu_PrintingReports_PrintingOrder_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Printing Order register"
        Common_Procedures.RptInputDet.ReportHeading = "Printing Order Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_AgentCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Agent_Creation
        'f.MdiParent = Me
        'f.Show()
    End Sub

    Private Sub mnu_Master_PriceListNAme_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_PriceListNAme.Click

        Match_UserRights_WithForm("mnu_Master_PriceListNAme", "Price_List_Entry_Emb")

        Dim f As New Form

        If Common_Procedures.settings.CustomerCode = "1117" Then
            f = Price_List_Entry_Emb
        End If

        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Company_ChangePeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Company_ChangePeriod.Click
        Dim f As New Change_Period
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_ColourCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_ColourCreation.Click
        Match_UserRights_WithForm("mnu_Master_ColourCreation", "Color_Creation")
        Dim f As New Color_Creation
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_Tally_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Export_Tally.Click
        Dim f As New Tally_Export
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_MachineCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_MachineCreation.Click

        Match_UserRights_WithForm("mnu_Master_MachineCreation", "Machine_Creation")
        Dim f As New Machine_Creation
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_DeliveryReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        If Trim(Common_Procedures.settings.CustomerCode) = "1201" Then
            Common_Procedures.RptInputDet.ReportName = "sales delivery register"
            Common_Procedures.RptInputDet.ReportHeading = "delivery register"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        Else
            Common_Procedures.RptInputDet.ReportName = "delivery register"
            Common_Procedures.RptInputDet.ReportHeading = "delivery register"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        End If

        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_DeliverySummaryReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "delivery summary"
        Common_Procedures.RptInputDet.ReportHeading = "delivery summary"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_InvoiceRegisterReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1053" Then
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "invoice register - rr"
            Common_Procedures.RptInputDet.ReportHeading = "invoice register"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
            f.MdiParent = Me
            f.Show()
        Else
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "invoice register - saara"
            Common_Procedures.RptInputDet.ReportHeading = "invoice register"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
            f.MdiParent = Me
            f.Show()
        End If
    End Sub

    Private Sub mnu_Reports_InvoiceSummaryReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1053" Then
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "invoice details - rr"
            Common_Procedures.RptInputDet.ReportHeading = "invoice Details"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
            f.MdiParent = Me
            f.Show()
        Else
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "invoice summary - saara"
            Common_Procedures.RptInputDet.ReportHeading = "invoice summary"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
            f.MdiParent = Me
            f.Show()
        End If
    End Sub

    Private Sub mnu_Accounts_DayBook_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_DayBook.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Day Book"
        Common_Procedures.RptInputDet.ReportHeading = "Day Book"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_DesignCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim F3 As New Design_Creation
        F3.MdiParent = Me
        F3.Show()
    End Sub

    Private Sub mnu_Master_GenderCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F3 As New Gender_Creation
        'F3.MdiParent = Me
        'F3.Show()
    End Sub

    Private Sub mnu_Master_SleeveCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F3 As New Sleeve_Creation
        'F3.MdiParent = Me
        'F3.Show()
    End Sub

    Private Sub mnu_Report_purchase_Return_register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Return Register"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Return Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Purchase_Return_Summary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Return Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Return Summary"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_entry_purchase_Return_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub mnu_Reports_SalesDetailsTax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportName = "Sales Details - withTax"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_SalesDetailsWithouttax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportName = "Sales Details - withoutTax"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Entry_OrderRegistration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1079" Then  'TANTEX (tirupur)
        '    Dim F2 As New Sales_Order_entry
        '    F2.MdiParent = Me
        '    F2.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F2.Close()
        '        F2.Dispose()
        '    End If
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1119" Then  'Alphonsa
        '    Dim F2 As New Sales_Order_3
        '    F2.MdiParent = Me
        '    F2.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F2.Close()
        '        F2.Dispose()
        '    End If
        'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2001" Then  'Deva
        '    Dim F2 As New Sales_Order_Project
        '    F2.MdiParent = Me
        '    F2.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F2.Close()
        '        F2.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1117" Or Trim(Common_Procedures.settings.CustomerCode) = "1201" Then
        '    Dim f1 As New Embroidery_Order_Entry
        '    f1.MdiParent = Me
        '    f1.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        f1.Close()
        '        f1.Dispose()
        '    End If
        'Else
        '    Dim F3 As New Sales_Order_2
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If

        'End If

    End Sub

    Private Sub mnu_Entry_PurchaseOrderEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F2 As New Purchase_Order_entry
        'F2.MdiParent = Me
        'F2.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F2.Close()
        '    F2.Dispose()
        'End If

    End Sub

    Private Sub mnu_MonthlySalesDetails_Quarterly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Monthly Sales Details - Quarterly"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details -Quarterly "
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_SalesDetails_HalfYearly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Monthly Sales Details - Halfyearly"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details -halfyearly"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_SalesDetails_Yearly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Monthly Sales Details - yearly"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details -yearly"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_MonthlySalesDetails_Quarterly_Itemwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Monthly Sales Details -ItemWise Quarterly"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details -ItemWise Quarterly "
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_MonthlySalesDetails_Halfyearly_Itemwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Monthly Sales Details -ItemWise Halfyearly"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details -ItemWise Halfyearly "
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_MonthlySalesDetails_Yearly_Itemwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Monthly Sales Details -ItemWise Yearly"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details -ItemWise Yearly "
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_CategoryCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_CategoryCreation.Click
        Match_UserRights_WithForm("mnu_Master_CategoryCreation", "Cetegory_Creation")
        Dim F2 As New Cetegory_Creation
        F2.MdiParent = Me
        F2.Show()
    End Sub

    Private Sub mnu_Accounts_GroupLedger_Grid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_GroupLedger_Grid.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Group Ledger - Grid"
        Common_Procedures.RptInputDet.ReportHeading = "Group Ledger"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,G*"
        Common_Procedures.RptInputDet.IsGridReport = True
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_SingleLedgerDateWise_Grid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_SingleLedgerDateWise_Grid.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "single ledger - Grid - datewise"
        Common_Procedures.RptInputDet.ReportHeading = "Ledger Statement"
        Common_Procedures.RptInputDet.IsGridReport = True
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,*L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_ProfitLossAc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_ProfitLossAc.Click
        Common_Procedures.CompIdNo = 0
        Dim f As New Profit_And_Loss
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_CustomerBills_BillPending_Single_Main_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_CustomerBills_BillPending_Single_Main.Click

    End Sub

    Private Sub mnu_Accounts_SingleLedger_DateWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_SingleLedger_DateWise.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Single Ledger A/c"
        Common_Procedures.RptInputDet.ReportHeading = "Ledger Statement"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,*L"
        f.MdiParent = Me
        f.Show()

    End Sub


    Private Sub mnu_Accounts_SingleLedger_MonthWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_SingleLedger_MonthWise.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Month Ledger A/c"
        Common_Procedures.RptInputDet.ReportHeading = "Ledger Statement - MonthWise"
        Common_Procedures.RptInputDet.ReportInputs = "Z,*L,MON"
        f.MdiParent = Me
        f.Show()
    End Sub


    Private Sub mnu_Accounts_CustomerBills_BillPending_All_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_CustomerBills_BillPending_All.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Customer Bill Pending - All"
        Common_Procedures.RptInputDet.ReportHeading = "CUSTOMER BILL PENDING"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub


    Private Sub mnu_Accounts_CustomerBills_BillPending_Purchased_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_CustomerBills_BillPending_Purchased.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Customer Bill Pending - Purchased"
        Common_Procedures.RptInputDet.ReportHeading = "CUSTOMER BILL PENDING - PURCHASED"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub


    Private Sub mnu_Accounts_CustomerBills_BillPending_Invoiced_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_CustomerBills_BillPending_Invoiced.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Customer Bill Pending - Invoiced"
        Common_Procedures.RptInputDet.ReportHeading = "CUSTOMER BILL PENDING - INVOICED"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub


    Private Sub mnu_Accounts_CustomerBills_BillDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_CustomerBills_BillDetails.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Customer Bill Details - Single"
        Common_Procedures.RptInputDet.ReportHeading = "CUSTOMER BILL DETAILS"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_CustomerBills_AgingAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_CustomerBills_AgingAnalysis.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Customer Bill Pending Aging Analysis"
        Common_Procedures.RptInputDet.ReportHeading = "CUSTOMER OUTSTANDING BILL LIST"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,DYSRNG"
        'Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,DYSRNG"
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Accounts_CustomerBills_InvoicePending_Agewise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Customer Bill Pending - All"
        Common_Procedures.RptInputDet.ReportHeading = "CUSTOMER BILL PENDING"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,*DYSFRM,*DYSTO"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_VoucherRegisters_BankReceiptRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_BankReceiptRegisters.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Bank Receipt"
        Common_Procedures.RptInputDet.ReportHeading = "Bank Voucher Receipt Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Accounts_VoucherRegisters_BankPaymentRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_BankPaymentRegisters.Click
        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportGroupName = "Register"


        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Bank Payment"
        Common_Procedures.RptInputDet.ReportHeading = "Bank Voucher Payment Register"

        'Common_Procedures.RptInputDet.ReportName = "Bank Payment Register"
        'Common_Procedures.RptInputDet.ReportHeading = "Bank Payment Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_VoucherRegisters_CashReceiptRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_CashReceiptRegisters.Click
        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportGroupName = "Register"

        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Cash Receipt"
        Common_Procedures.RptInputDet.ReportHeading = "Cash Voucher Receipt Register"
        'Common_Procedures.RptInputDet.ReportName = "Cash Receipt Register"
        'Common_Procedures.RptInputDet.ReportHeading = "Cash Receipt Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_VoucherRegisters_CashPaymentRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_CashPaymentRegisters.Click
        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportGroupName = "Register"

        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Cash Payment"
        Common_Procedures.RptInputDet.ReportHeading = "Cash Voucher Payment Register"

        'Common_Procedures.RptInputDet.ReportName = "Cash Payment Register"
        'Common_Procedures.RptInputDet.ReportHeading = "Cash Payment Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_VoucherRegisters_CreditNoteRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_CreditNoteRegisters.Click
        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportGroupName = "Register"

        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Credit Note"
        Common_Procedures.RptInputDet.ReportHeading = "Credit Note Voucher Register"
        'Common_Procedures.RptInputDet.ReportName = "Credit Note Register"
        'Common_Procedures.RptInputDet.ReportHeading = "Credit Note Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_VoucherRegisters_DebitNoteRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_DebitNoteRegisters.Click
        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportGroupName = "Register"

        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Debit Note"
        Common_Procedures.RptInputDet.ReportHeading = "Debit Note Voucher Register"
        'Common_Procedures.RptInputDet.ReportName = "Debit Note Register"
        'Common_Procedures.RptInputDet.ReportHeading = "Debit Note Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_VoucherRegisters_PettiCashRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_PettiCashRegisters.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"

        Common_Procedures.RptInputDet.ReportName = "Voucher Register - PettiCash"
        Common_Procedures.RptInputDet.ReportHeading = "PettiCash Voucher Register"
        'Common_Procedures.RptInputDet.ReportName = "Debit Note Register"
        'Common_Procedures.RptInputDet.ReportHeading = "Debit Note Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_VoucherRegisters_ContraRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_ContraRegisters.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Contra"
        Common_Procedures.RptInputDet.ReportHeading = "Contra Voucher Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_VoucherRegisters_JournalRegisters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_JournalRegisters.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Journal"
        Common_Procedures.RptInputDet.ReportHeading = "Journal Voucher Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_CustomerBills_BillPending_Single_AsOnDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_CustomerBills_BillPending_Single_AsOnDate.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Customer Bill Pending - Single"
        Common_Procedures.RptInputDet.ReportHeading = "CUSTOMER BILL PENDING"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Accounts_CustomerBills_BillPending_Single_AsOnDate_Including_Postdated_Amount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_CustomerBills_BillPending_Single_AsOnDate_Including_Postdated_Amount.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Customer Bill Pending - Single - With PostDated Amount"
        Common_Procedures.RptInputDet.ReportHeading = "CUSTOMER BILL PENDING"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_ClosingStockValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_ClosingStockValue.Click

        Match_UserRights_WithForm("mnu_ClosingStockValue", "Closing_Stock_value")

        Dim f As New Closing_Stock_value
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Tools_Settings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_Settings.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "SET@123" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim f As New CC_Update
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_master_Salesman_Creation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F3 As New Salesman_Creation
        'F3.MdiParent = Me
        'F3.Show()
    End Sub

    Private Sub mnu_Entry_Sales_Discount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F3 As New Sales_Discount_Entry
        'F3.MdiParent = Me
        'F3.Show()
    End Sub

    Private Sub mnu_Report_SalesDetails_ItemGroupIWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Details Item Group Wise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,I,P"
        f.MdiParent = Me
        f.Show()
    End Sub



    Private Sub mnu_Report_SalesDetails_PartyMonthwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Details Monthly - Partywise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details - Monthly"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,L,I,*MON"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_SalesDetails_Monthly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Details Monthly- ItemWise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details - Monthly"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,L,I,*MON"
        f.MdiParent = Me
        f.Show()
    End Sub

    'Private Sub mnu_Entry_Ledger_Item_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Ledger_Item_Details.Click
    '    Dim f As New Ledger_Item_Details
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub

    'Private Sub mnu_Report_Milk_Sales_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Milk_Sales_Register.Click
    '    Dim f As New Report_Details

    '    Common_Procedures.RptInputDet.ReportGroupName = "Register"
    '    Common_Procedures.RptInputDet.ReportName = "Milk Sales Register"
    '    Common_Procedures.RptInputDet.ReportHeading = "Sales Register"
    '    Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,AG,P"
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub


    'Private Sub mnu_Report_Milk_Sales_Summary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Milk_Sales_Summary.Click
    '    Dim f As New Report_Details
    '    Common_Procedures.RptInputDet.ReportGroupName = "Register"
    '    Common_Procedures.RptInputDet.ReportName = "Milk Sales Summary"
    '    Common_Procedures.RptInputDet.ReportHeading = "Sales Summary"
    '    Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub

    Private Sub mnu_Reports_Masters_Party_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Masters_Party.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Master Party Register"
        Common_Procedures.RptInputDet.ReportHeading = "Master Party Register"
        Common_Procedures.RptInputDet.ReportInputs = "P, AR"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Masters_Ledger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Masters_Ledger.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Master Ledger Register"
        Common_Procedures.RptInputDet.ReportHeading = "Master Ledger Register"
        Common_Procedures.RptInputDet.ReportInputs = "G,L"
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Reports_Masters_Item_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Masters_Item.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Master Item Register"
        Common_Procedures.RptInputDet.ReportHeading = "Master Item Register"
        Common_Procedures.RptInputDet.ReportInputs = "I,IG"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_SalesDetails_PartyWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Details Party Wise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub


    Private Sub mnu_Register_PartySalesRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Party Details Register"
        Common_Procedures.RptInputDet.ReportHeading = "Party Details Register"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_QuotationEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub mnu_Entry_DeliveryEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_DeliveryEntry.Click

        Match_UserRights_WithForm("mnu_Entry_DeliveryEntry", "Embroidery_Delivery_Entry")

        Dim f1 As New Embroidery_Delivery_Entry



        If Common_Procedures.User.IdNo = 1 Then

            f1.Previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_DeliveryEntry")
            If I > -1 Then
                f1.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If

        End If

        f1.MdiParent = Me
            f1.Show()

            If Val(Common_Procedures.CompIdNo) = 0 Then
                f1.Close()
                f1.Dispose()
            End If
        

    End Sub

    Private Sub mnu_Entry_LabourInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Common_Procedures.SalesEntryType = "LABOUR INVOICE"
        'Dim F1 As New SalesEntry_Simple1
        'F1.MdiParent = Me
        'F1.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F1.Close()
        '    F1.Dispose()
        'End If
    End Sub

    Private Sub mnu_Printing_OrderEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F1 As New Printing_Order_Entry
        'F1.MdiParent = Me
        'F1.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F1.Close()
        '    F1.Dispose()
        'End If
    End Sub

    Private Sub mnu_Printing_OrderProgramEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F1 As New Order_Program_Entry
        'F1.MdiParent = Me
        'F1.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F1.Close()
        '    F1.Dispose()
        'End If
    End Sub

    Private Sub mnu_Reports_StockSummary_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Stock Summary Details"
        Common_Procedures.RptInputDet.ReportHeading = "STOCK SUMMARY"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z ,CATI"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Printing_InvoiceEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F1 As New Printing_Invoice
        'F1.MdiParent = Me
        'F1.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F1.Close()
        '    F1.Dispose()
        'End If
    End Sub

    Private Sub mnu_PrintingReports_PrintingOrder_Program_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Order Program Register"
        Common_Procedures.RptInputDet.ReportHeading = "Order Program Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_PrintingReports_PrintingInvoice_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Printing Invoice register"
        Common_Procedures.RptInputDet.ReportHeading = "Printing Invoice Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_PrintingReports_PrintingOrder_Program_Pending_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Printing Order Program Pending"
        Common_Procedures.RptInputDet.ReportHeading = "Printing Order Program Pending"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_PrintingReports_Printing_Invoice_Pending_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Printing Invoice Pending"
        Common_Procedures.RptInputDet.ReportHeading = "Printing Invoice Pending"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub ToolStripMenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Register"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub ToolStripMenuItem9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Details"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Details"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Account_Party_Outstanding_Simple_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Account_Party_Outstanding_Simple.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Party Balance - BillWise Simple"
        Common_Procedures.RptInputDet.ReportHeading = "Party OutStanding List"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,L,AR"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_PrintingReports_PrintingOrdercancel_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Printing Order Cancel register"
        Common_Procedures.RptInputDet.ReportHeading = "Printing Order Cancel Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_PrintingReports_PrintingOrder_Details_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Printing Order Details List"
        Common_Procedures.RptInputDet.ReportHeading = "Printing Order Details"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub


    Private Sub mnu_Report_Purchase_Summary_CategoryWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Purchase_Summary_CategoryWise.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Purchase Summary CategoryWise"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Summary CategoryWise"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,CATI,IG"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Purchase_Summary_ItemWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Purchase_Summary_ItemWise.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Purchase Summary ItemWise"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Summary ItemWise"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,IG,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Purchase_Summary_Monthwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Purchase_Summary_Monthwise.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Purchase Summary Monthly- ItemWise"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Summary - Monthly"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,L,I,*MON"
        f.MdiParent = Me
        f.Show()

    End Sub


    Private Sub mnu_Company_Masters_Transfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Company_Masters_Transfer.Click

        Match_UserRights_WithForm("mnu_Company_Masters_Transfer", "Transfer_Master_Ledgers_From_CompanyGroup")

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSTRA7417" Then
            MessageBox.Show("Invalid Password", "MASTERS TRANSFER FAILD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim f As New Transfer_Master_Ledgers_From_CompanyGroup
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Entry_FreeItemDeliveryEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F4 As New Sales_Delivery_Free
        'F4.MdiParent = Me
        'F4.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F4.Close()
        '    F4.Dispose()
        'End If

    End Sub




    Private Sub mnu_Report_Purchase_Sales_BatchNo_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles mnu_Report_Purchase_Sales_BatchNo_Register.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Sales Details"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Sales  Details"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I,BTCHSN"
        f1.MdiParent = Me
        f1.Show()
    End Sub
    Private Sub btn_DashBoard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_DashBoard.Click
        '  btn_DashBoard.Visible = False
        Dim F2 As New DashBoard
        F2.MdiParent = Me
        F2.Show()
    End Sub


    Private Sub mnu_Report_OrderWise_Profit_And_Loss_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "OrderWise Profit And Loss Report"
        Common_Procedures.RptInputDet.ReportHeading = "OrderWise Profit And Loss Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Bank_And_Cash_Receipt_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Bank_And_Cash_Receipt_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Cash and Bank Receipt"
        Common_Procedures.RptInputDet.ReportHeading = "Bank and Cash Voucher Receipt Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Bank_And_Cash_Payment_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Bank_And_Cash_Payment_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Voucher Register - Cash and Bank Payment"
        Common_Procedures.RptInputDet.ReportHeading = "Bank and Cash Voucher Payment Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    
   


    
    
    
    
    Private Sub mnu_Tools_Field_Check_PayRoll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim CN1 As New SqlClient.SqlConnection

        CN1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        CN1.Open()

        FieldsCheck.vFldsChk_All_Status = False
        FieldsCheck.vFldsChk_From_CompGroupCreation_Status = False
        FieldsCheck.Field_Check_PayRoll(CN1, Me)
        FieldsCheck.vFldsChk_All_Status = False
        FieldsCheck.vFldsChk_From_CompGroupCreation_Status = False

        CN1.Open()
        CN1.Dispose()

    End Sub

    'Private Sub mnu_Entry_Report_PayRoll_Employee_Payment_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim f As New Report_Details_1
    '    Common_Procedures.RptInputDet.ReportGroupName = "Register"
    '    Common_Procedures.RptInputDet.ReportName = "Employee Payment Register"
    '    Common_Procedures.RptInputDet.ReportHeading = "Employee Payment Register"
    '    Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,DB"
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub

    'Private Sub mnu_Report_payRoll_Employee_deduction_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim f As New Report_Details_1
    '    Common_Procedures.RptInputDet.ReportGroupName = "Register"
    '    Common_Procedures.RptInputDet.ReportName = "Employee Deduction Register"
    '    Common_Procedures.RptInputDet.ReportHeading = "Employee Deduction Register"
    '    Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,EMP"
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub

    'Private Sub mnu_Reports_Payroll_Employee_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim f As New Report_Details_1
    '    Common_Procedures.RptInputDet.ReportGroupName = "Register"
    '    Common_Procedures.RptInputDet.ReportName = "Payroll Employee Register"
    '    Common_Procedures.RptInputDet.ReportHeading = "Employee Register"
    '    Common_Procedures.RptInputDet.ReportInputs = "EMP"
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub

    'Private Sub mnu_Reports_Payroll_Attendance_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim f As New Report_Details_1
    '    Common_Procedures.RptInputDet.ReportGroupName = "Register"

    '    If Common_Procedures.settings.PAYROLLENTRY_Attendance_In_Hours_Status = 1 Then
    '        Common_Procedures.RptInputDet.ReportName = "Payroll Attendance Register Hours"
    '    Else
    '        Common_Procedures.RptInputDet.ReportName = "Payroll Attendance Register"
    '    End If

    '    Common_Procedures.RptInputDet.ReportHeading = "Attendance Register"
    '    Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub

    Private Sub mnu_Reports_Payroll_Salary_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Salary Register"
        Common_Procedures.RptInputDet.ReportHeading = "Salary Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,CAT,MON"
        f.MdiParent = Me
        f.Show()
    End Sub

    'Private Sub mnu_Reports_Payroll_NetPay_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim f As New Report_Details_1
    '    Common_Procedures.RptInputDet.ReportGroupName = "Register"
    '    Common_Procedures.RptInputDet.ReportName = "Payroll NetPay Register"
    '    Common_Procedures.RptInputDet.ReportHeading = "NetPay Register"
    '    Common_Procedures.RptInputDet.ReportInputs = "2DT"
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub

    'Private Sub mnu_Reports_Payroll_Attendance_MothWise_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim f As New Report_Details_1
    '    Common_Procedures.RptInputDet.ReportGroupName = "Register"
    '    If Common_Procedures.settings.PAYROLLENTRY_Attendance_In_Hours_Status = 1 Then
    '        Common_Procedures.RptInputDet.ReportName = "Payroll Attendance MonthWise Register Hours"

    '    Else
    '        Common_Procedures.RptInputDet.ReportName = "Payroll Attendance MonthWise Register"

    '    End If

    '    Common_Procedures.RptInputDet.ReportHeading = "Attendance MonthWise Register"
    '    Common_Procedures.RptInputDet.ReportInputs = "*MON"
    '    f.MdiParent = Me
    '    f.Show()
    'End Sub

    Private Sub mnu_Reports_Payroll_Accounts_Details_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Accounts"
        Common_Procedures.RptInputDet.ReportName = "Employee Ledger A/c"
        Common_Procedures.RptInputDet.ReportHeading = "Employee Statement"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,*EMP"
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Entry_Billing_Job_Card_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Job_Card_Entry
        'f.MdiParent = Me
        'f.Show()
    End Sub

    Private Sub mnu_Master_JobWorkCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F3 As New JobWork_Creation
        'F3.MdiParent = Me
        'F3.Show()
    End Sub

    Private Sub mnu_Entry_Employee_DailyWorking_Entry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F As New Payroll_Employee_Daily_Working_Entry
        'F.MdiParent = Me
        'F.Show()
    End Sub

    Private Sub mnu_Entry_Enquiry_Entry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Common_Procedures.CompIdNo = 0
        'Dim F As New Sales_Enquiry
        'F.MdiParent = Me
        'F.Show()
        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F.Close()
        '    F.Dispose()
        'End If

    End Sub

    Private Sub mnu_Payroll_Daily_Working_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Employee Daily Working Register"
        Common_Procedures.RptInputDet.ReportHeading = "Employee Daily Working Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,EMP"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_ItemWise_SalesInvoice_Report_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_ItemWise_SalesInvoice_Report.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Stock Details simple"
        Common_Procedures.RptInputDet.ReportHeading = "ITEM STOCK DETAILS"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,I,P,PUP"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Annexture_2_Audit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Annexure-II Audit"
        Common_Procedures.RptInputDet.ReportHeading = "Annexure-II"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_AmountBalamnce_Report_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Amount Balance Report"
        Common_Procedures.RptInputDet.ReportHeading = "Amount Balance Report"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_TaxCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_TaxCreation.Click
        'Dim F As New Tax_Creation
        'F.MdiParent = Me
        'F.Show()
    End Sub

    Private Sub mnu_Entry_Invoice_Designing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Try

        '    Common_Procedures.CompIdNo = 0
        '    Common_Procedures.SalesEntryType = ""

        '    If Trim(Common_Procedures.settings.CustomerCode) = "1001--" Then
        '        Dim F2 As New CashSales_Entry   '---- No tax
        '        F2.MdiParent = Me
        '        F2.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F2.Close()
        '            F2.Dispose()
        '        End If

        '    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1002" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1004" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1054" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1141" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1150" Then '---- Peacock Traders(Tirupur)
        '        Dim F3 As New SalesEntry_YarnBill_GST
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1003" Then '---- SenthilNathan Spinners (Karumanthapatti)
        '        Dim F3 As New Spinning_Invoice_Entry_GST
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1008" Or Trim(Common_Procedures.settings.CustomerCode) = "1013" Then
        '        Dim F3 As New Invoice_Garments_GST
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1130" Then
        '        Dim F3 As New Invoice_Garments6_GST
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If
        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1108" Then

        '        Dim F5 As New Sales_Details_BatchNo_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1011" Then '---- Chellam Batteries (Thekkalur)

        '        Dim F6 As New SalesEntry_SimpleGst
        '        F6.MdiParent = Me
        '        F6.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F6.Close()
        '            F6.Dispose()
        '        End If


        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1014" Then       '---- SriRam Designer (Tirupur)
        '        Dim F5 As New Invoice_Design
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If


        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "BABU-UPS" Then
        '        Dim F3 As New SalesEntry_Single_Tax
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1048" Then

        '        Dim F3 As New Invoice_Saara_GST
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If


        '    ElseIf (Trim(Common_Procedures.settings.CustomerCode)) = "1051" Or (Trim(Common_Procedures.settings.CustomerCode)) = "1156" Or (Trim(Common_Procedures.settings.CustomerCode)) = "1167" Then

        '        Dim F3 As New SalesEntry_Simple3_GST
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()

        '        End If

        '    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1062" Or Trim(Common_Procedures.settings.CustomerCode) = "2002" Then

        '        Dim F3 As New SalesEntry_Simple2
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1053" Then

        '        Dim F3 As New Invoice_RR
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If

        '    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1068" Then '---- Gee Fashion (Tirupur)
        '        Dim F3 As New Invoice_Garments_Format2_GST
        '        F3.MdiParent = Me
        '        F3.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F3.Close()
        '            F3.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1070" Or Trim(Common_Procedures.settings.CustomerCode) = "1217" Then

        '        Dim F5 As New Invoice_Garments3_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1073" Then

        '        Dim F5 As New SalesEntry_Barcode_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1077" Then

        '        Dim F5 As New SalesEntry_Barcode_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1079" Then

        '        Dim F5 As New Invoice_Garments4
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1085" Then

        '        Dim F5 As New SalesEntry_Milk
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1091" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1196" Or Trim(Common_Procedures.settings.CustomerCode) = "1154" Or Trim(Common_Procedures.settings.CustomerCode) = "1196" Then

        '        Common_Procedures.SalesEntryType = "TAX INVOICE"
        '        Dim F6 As New SalesEntry_Simple1_GST
        '        F6.MdiParent = Me
        '        F6.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F6.Close()
        '            F6.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1080" Then

        '        'Common_Procedures.SalesEntryType = "TAX INVOICE"
        '        Dim F7 As New SalesEntry_Simple4_GST
        '        F7.MdiParent = Me
        '        F7.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F7.Close()
        '            F7.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1092" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1171" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1183" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1190" Then

        '        Dim F5 As New SalesEntry_Barcode_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1137" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1149" Then  '- NATRAJ KNIT WEAR

        '        Dim F5 As New Invoice_Garments5_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1103" Then  '--- BALAJI GARMENTS (Tirupur) 

        '        Dim F5 As New Invoice_Garments5_GST_Balaji
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1107" Then '---- GAJAKHARNAA TRADERS (Somanur)
        '        Dim F8 As New SalesEntry_Simple2_GST 'SalesEntry_MultiTax
        '        F8.MdiParent = Me
        '        F8.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F8.Close()
        '            F8.Dispose()
        '        End If
        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1108" Then

        '        Dim F5 As New Sales_Details_BatchNo
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1109" Or Trim(Common_Procedures.settings.CustomerCode) = "1123" Or Trim(Common_Procedures.settings.CustomerCode) = "1128" Then  ' - auro satya  AND SANTHI SIZING AND SRI LAXMI PLASTICS

        '        Dim F5 As New SalesEntry_Simple5_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1119" Then '-- Alphonsa Card
        '        Dim F5 As New SalesEntry_Simple6_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1142" Or Trim(Common_Procedures.settings.CustomerCode) = "1501" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1503" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1174" Then 'SLP (tirupur)  , subi exports  , leo gem
        '        Dim F5 As New SalesEntry_Hanger_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1194" Then '--- VINAYAGA ENGINEERING
        '        Dim F5 As New Jobwork_Bill_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1502" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1506" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1504" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1505" Then 'CREAT PRINT , GOLDEN TEXTILES  PRINTERS
        '        Dim F5 As New SalesEntry_Hanger_GST ' SalesEntry_Simple5_GST
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "2001" Then '-Deva
        '        Dim F5 As New SalesEntry_Project
        '        F5.MdiParent = Me
        '        F5.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F5.Close()
        '            F5.Dispose()
        '        End If

        '    ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1117" Or Trim(Common_Procedures.settings.CustomerCode) = "1200" Then
        '        Dim f2 As New Invoice_Embroidery_Design
        '        f2.MdiParent = Me
        '        f2.Show()
        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            f2.Close()
        '            f2.Dispose()
        '        End If

        '    Else

        '        Dim F4 As New SalesEntry_Simple2_GST
        '        F4.MdiParent = Me
        '        F4.Show()

        '        If Val(Common_Procedures.CompIdNo) = 0 Then
        '            F4.Close()
        '            F4.Dispose()
        '        End If

        '    End If

        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT LOAD FORM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try


    End Sub

    Private Sub mnu_Master_Style_Creation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles mnu_Master_Style_Creation.Click
        'Dim F3 As New Style_Creation
        'F3.MdiParent = Me
        'F3.Show()
    End Sub

    Private Sub mnu_Entry_PurchaseEntry_GST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_PurchaseEntry_GST.Click

        Match_UserRights_WithForm("mnu_Entry_PurchaseEntry_GST", "Purchase_Entry_Simple_Gst")

        Common_Procedures.CompIdNo = 0

        Dim F1 As New Purchase_Entry_Simple_Gst

        If Common_Procedures.User.IdNo = 1 Then

            F1.previlege = "L"

        Else
            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_PurchaseEntry_GST")
            If I > -1 Then
            F1.previlege = Common_Procedures.UR1.UserInfo(I, 1)
        End If
        End If

        F1.MdiParent = Me
        F1.Show()

        If Val(Common_Procedures.CompIdNo) = 0 Then

            F1.Close()
            F1.Dispose()

        End If


    End Sub

    Private Sub mnu_Master_Style_Creation_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Style_Creation
        'f.MdiParent = Me
        'f.Show()
    End Sub

    Private Sub mnu_Entry_LabourInvoice_GST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Common_Procedures.SalesEntryType = "LABOUR INVOICE"
        'Dim F1 As New SalesEntry_Simple1_GST
        'F1.MdiParent = Me
        'F1.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F1.Close()
        '    F1.Dispose()
        'End If
    End Sub

    Private Sub mnu_Reports_Annexure1_GST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "GSTR-1"
        Common_Procedures.RptInputDet.ReportHeading = "GSTR-I"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Annexure2_GST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "GSTR-2"
        Common_Procedures.RptInputDet.ReportHeading = "GSTR-II"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_Labour_Invoice_Sales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Common_Procedures.SalesEntryType = "LABOUR INVOICE"

        'Common_Procedures.Sales_Or_Service = "SALES"
        'Dim F1 As New SalesEntry_Simple1_GST
        'F1.MdiParent = Me
        'F1.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F1.Close()
        '    F1.Dispose()
        'End If
    End Sub

    Private Sub mnu_Entry_Labour_Invoice_Service_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Common_Procedures.SalesEntryType = "LABOUR INVOICE"
        'Common_Procedures.Sales_Or_Service = "SERVICE"
        'Dim F1 As New SalesEntry_Simple1_GST
        'F1.MdiParent = Me
        'F1.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F1.Close()
        '    F1.Dispose()
        'End If
    End Sub

    Private Sub mnu_Reports_Outward_Unregistered_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Outward Supply - UnRegistered"
        Common_Procedures.RptInputDet.ReportHeading = "Outward Supply - UnRegistered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Outward_Registered_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Outward Supply - Registered"
        Common_Procedures.RptInputDet.ReportHeading = "Outward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Inward_Unregistered_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Inward Supply - UnRegistered"
        Common_Procedures.RptInputDet.ReportHeading = "Inward Supply - UnRegistered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Inward_Registered_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Inward Supply - Registered"
        Common_Procedures.RptInputDet.ReportHeading = "Inward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_Sales_Receipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' Handles mnu_Entry_Sales_Receipt.Click
        'Dim f As New Sales_Receipt
        'f.MdiParent = Me
        'f.Show()
    End Sub
    Private Sub mnu_Report_Sales_Receipt_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles mnu_Report_Sales_Receipt_Register.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Sales Receipt Register"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Receipt Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Report_Sales_Receipt_Summary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles mnu_Report_Sales_Receipt_Summary.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Sales Receipt Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Receipt Summary"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Report_Sales_Delivery_Pending_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles mnu_Report_Sales_Delivery_Pending.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Sales Delivery Pending Register"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Delivery Pending"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_ItemStock_Aging_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Item Stock - Age Wise"
        Common_Procedures.RptInputDet.ReportHeading = "Item Stock - Age Wise"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Sales_Delivery_Pending_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Sales Delivery Pending Register"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Delivery Pending Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Sales_Receipt_Register_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Sales Receipt Register"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Receipt Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Sales_Receipt_Summary_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Sales Receipt Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Receipt Summary"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_Receipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_ReceiptEntry.Click

        Match_UserRights_WithForm("mnu_Entry_ReceiptEntry", "Embroidery_Receipt_Entry")

        Dim f1 As New Embroidery_Receipt_Entry

        If Common_Procedures.User.IdNo = 1 Then

            f1.previlege = "L"

        Else
            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_ReceiptEntry")
            If I > -1 Then
            f1.previlege = Common_Procedures.UR1.UserInfo(I, 1)
        End If
        End If

        f1.MdiParent = Me
        f1.Show()

        If Val(Common_Procedures.CompIdNo) = 0 Then
            f1.Close()
            f1.Dispose()
        End If

    End Sub





    Private Sub mnu_Master_PartyPriceListName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Party_Price_List_Entry
        'f.MdiParent = Me
        'f.Show()
    End Sub

    Private Sub mnu_Report_PartyWise_PriceListReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "PartyWise Price List"
        Common_Procedures.RptInputDet.ReportHeading = "Price List"
        Common_Procedures.RptInputDet.ReportInputs = "P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_PurchaseReturn_VAT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F4 As New Purchase_Return_Entry
        'F4.MdiParent = Me
        'F4.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F4.Close()
        '    F4.Dispose()
        'End If

    End Sub

    Private Sub mnu_Entry_PurchaseReturn_GST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F4 As New Purchase_Return_Gst
        'F4.MdiParent = Me
        'F4.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F4.Close()
        '    F4.Dispose()
        'End If

    End Sub

    Private Sub mnu_Entry_SalesReturn_VAT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'If Trim(Common_Procedures.settings.CustomerCode) = "1068" Then ' ---------------GEE FASHIONS

        '    Dim F3 As New Sales_Return_Garments2
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If
        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1119" Then ' ----Alphonsa

        '    Dim F3 As New SalesReturn_Entry_Simple1
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If

        'ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1167" Then ' ----f fashion

        '    Dim F3 As New SalesReturn_Entry_GST
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()
        '    End If

        'Else

        '    Common_Procedures.CompIdNo = 0

        '    Dim F2 As New SalesReturn_Entry
        '    F2.MdiParent = Me
        '    F2.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F2.Close()
        '        F2.Dispose()
        '    End If

        'End If



    End Sub

    Private Sub mnu_Entry_SalesReturn_GST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Common_Procedures.CompIdNo = 0

        'Dim F2 As New SalesReturn_Entry_GST
        'F2.MdiParent = Me
        'F2.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F2.Close()
        '    F2.Dispose()
        'End If

    End Sub

    Private Sub BVATToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'If Trim(Common_Procedures.settings.CustomerCode) = "2001" Then '-Deva

        '    Common_Procedures.CompIdNo = 0
        '    Dim F As New Sales_Quotation_Project
        '    F.MdiParent = Me
        '    F.Show()
        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F.Close()
        '        F.Dispose()
        '    End If
        'Else

        '    Common_Procedures.CompIdNo = 0
        '    Dim F As New Sales_Quotation
        '    F.MdiParent = Me
        '    F.Show()
        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F.Close()
        '        F.Dispose()
        '    End If
        'End If


    End Sub

    Private Sub mnu_entry_SalesQuotation_GST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If Trim(Common_Procedures.settings.CustomerCode) = "1201" Or Trim(Common_Procedures.settings.CustomerCode) = "1117" Then '-Deva
        Common_Procedures.CompIdNo = 0
        Dim F As New Embroidery_Quotation_Entry
        F.MdiParent = Me
        F.Show()
        If Val(Common_Procedures.CompIdNo) = 0 Then
            F.Close()
            F.Dispose()
        End If
        'End If

    End Sub

    Private Sub mnu_Report_Invoice_Details_GST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "invoice register - saara gst"
        Common_Procedures.RptInputDet.ReportHeading = "invoice register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_BillEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F4 As New SalesEntry_SimpleBill_GST
        'F4.MdiParent = Me
        'F4.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F4.Close()
        '    F4.Dispose()
        'End If
    End Sub

    Private Sub mnu_TockenEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F4 As New TockenEntryGST
        'F4.MdiParent = Me
        'F4.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F4.Close()
        '    F4.Dispose()
        'End If

    End Sub

    Private Sub mnu_Master_RateSeetings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim F As New Rate_Creation
        'F.MdiParent = Me
        'F.Show()
    End Sub

    Private Sub mnu_Report_TockenRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Token Register"
        Common_Procedures.RptInputDet.ReportHeading = "Token Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,VN,TKN,TKTP"

        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_MonthlyPlan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F4 As New Token_MonthlyPlan_Entry
        'F4.MdiParent = Me
        'F4.Show()

        'If Val(Common_Procedures.CompIdNo) = 0 Then
        '    F4.Close()
        '    F4.Dispose()
        'End If

    End Sub

    Private Sub mnu_Report_MonthlyToken_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Monthly Token Register"
        Common_Procedures.RptInputDet.ReportHeading = "Monthly Token Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MVN,MTKN,CSTS,PYSTS"

        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_StockSummary_HsnCodeWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Stock Summary Details HSNCode Wise"
        Common_Procedures.RptInputDet.ReportHeading = "STOCK SUMMARY"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z ,HSN"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_ProductionEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_ProductionEntry.Click

        Match_UserRights_WithForm("mnu_Entry_ProductionEntry", "Embroidery_Production_Entry")

        Common_Procedures.CompIdNo = 0
        Dim F As New Embroidery_Production_Entry
        F.MdiParent = Me
        F.Show()
        If Val(Common_Procedures.CompIdNo) = 0 Then
            F.Close()
            F.Dispose()
        End If

    End Sub

    Private Sub mnu_Report_Production_Register_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_Report_Production_Register.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "production register"
        Common_Procedures.RptInputDet.ReportHeading = "production register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,MC,SHIFT-EMB,EMB-JOB,EMB-ORD,EMP-IN,EMP-OP,EMP-FR"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_BillRegister_Report_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Bill Register"
        Common_Procedures.RptInputDet.ReportHeading = "Bill register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub
    
    Private Sub mnu_Master_ChequePrintingPosotion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_ChequePrintingPosotion.Click

        Match_UserRights_WithForm("mnu_Master_ChequePrintingPosotion", "Cheque_Entry_Print_Positioning")
        Dim f As New Cheque_Entry_Print_Positioning
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Master_Site_Creation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Site_Creation
        'f.MdiParent = Me
        'f.Show()
    End Sub



    Private Sub mnu_Tools_Dashboard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_Tools_Dashboard.Click

        If InStr(UCase((mnu_Tools_Dashboard.Text)), "SHOW") > 0 Then
            DashBoard.MdiParent = Me
            DashBoard.Show()
            mnu_Tools_Dashboard.Text = "B. Hide DashBoard"
        Else
            DashBoard.Close()
            mnu_Tools_Dashboard.Text = "B. Show DashBoard"
        End If

    End Sub



    'Private Sub MDIParent1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseClick
    '    ContextMenuStrip1.Show()

    'End Sub

    Private Sub MDIParent1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

    End Sub

    Private Sub MDIParent1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

    End Sub

    Private Sub MDIParent1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        'If mnu_Tools_Dashboard.Visible = False Then
        '    Dim F2 As New DashBoard
        '    F2.MdiParent = Me
        '    F2.Close()
        'End If
    End Sub

    Private Sub mnu_Tools_LicenseCodeGeneration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_LicenseCodeGeneration.Click
        Dim F2 As New Tsoft_Register_Encryption_DeCrption_Form
        F2.MdiParent = Me
        F2.Show()
    End Sub

    Private Sub mnu_DemoEntry_PurchaseEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mnu_Entry_PurchaseEntry_GST_Click(sender, e)
    End Sub

    Private Sub mnu_DemoEntry_PurchaseReturnEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mnu_Entry_PurchaseReturn_GST_Click(sender, e)
    End Sub

    Private Sub mnu_DemoEntry_SalesEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mnu_Entry_Invoice_Designing_Click(sender, e)
    End Sub

    Private Sub mnu_DemoEntry_SalesReturnEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mnu_Entry_SalesReturn_GST_Click(sender, e)
    End Sub

    Private Sub mnu_Entry_Estimate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub mnu_Master_Distribution_LedgerCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim f As New Ledger_Creation
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Master_Distrubution_ItemCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim F As New Item_Creation
        F.MdiParent = Me
        F.Show()

    End Sub

    Private Sub mnu_Master_Distrbution_SchemeMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim F As New Scheme_Master
        'F.MdiParent = Me
        'F.Show()

    End Sub

    Private Sub mnu_Master_Distribution_Sales_entry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If (Trim(Common_Procedures.settings.CustomerCode)) = "1219" Then  ' vels enterprises
        '    Dim F3 As New SalesEntry_Simple2_GST_Discount
        '    F3.MdiParent = Me
        '    F3.Show()

        '    If Val(Common_Procedures.CompIdNo) = 0 Then
        '        F3.Close()
        '        F3.Dispose()

        '    End If
        'End If

    End Sub

    Private Sub mnu_DistributionReport_PurchaseRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Register"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_distributionreports_sales_register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "sales register"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"

        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_salessummary_partywise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Summary PartyWise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Summary PartyWise"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_salessummary_itemwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Summary ItemWise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Summary ItemWise"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_salessummary_partywisand_itemwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Summary"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub


    Private Sub mnu_dr_salesdetails_itemGroup_wise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Details Item Group Wise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,I,P"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub ToolStripMenuItem43mnu_dr_salesdetails_partywise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Sales Details Party Wise"
        Common_Procedures.RptInputDet.ReportHeading = "Sales Details"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,CATI,IG,P,I"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_stock_details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Stock Details"
        Common_Procedures.RptInputDet.ReportHeading = "STOCK DETAILS"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,I"
        f.MdiParent = Me
        f.Show()


    End Sub

    Private Sub mnu_dr_Stock_summary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Stock Summary Details"
        Common_Procedures.RptInputDet.ReportHeading = "STOCK SUMMARY"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z ,CATI"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_stocksummary_withItemGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Stock Summary"
        Common_Procedures.RptInputDet.ReportHeading = "STOCK SUMMARY"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z,IG"

        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_stocksummary_hsncode_wise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Stock"
        Common_Procedures.RptInputDet.ReportName = "Stock Summary Details HSNCode Wise"
        Common_Procedures.RptInputDet.ReportHeading = "STOCK SUMMARY"
        Common_Procedures.RptInputDet.ReportInputs = "1DT,Z ,HSN"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Dr_GSTR_Reports_GSTR1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Outward Supply - Registered"
        Common_Procedures.RptInputDet.ReportHeading = "Outward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Dr_GSTR_Reports_GSTR2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Inward Supply - Registered"
        Common_Procedures.RptInputDet.ReportHeading = "Inward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Dr_GSTR_Reports_GSTR1_with_partyname_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Outward Supply - Registered with PartyName"
        Common_Procedures.RptInputDet.ReportHeading = "Outward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Dr_GSTR_Reports_GSTR2_with_partyname_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Inward Supply - Registered With PartyName"
        Common_Procedures.RptInputDet.ReportHeading = "Inward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_partyreports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Master Party Register"
        Common_Procedures.RptInputDet.ReportHeading = "Master Party Register"
        Common_Procedures.RptInputDet.ReportInputs = "P, AR"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_itemreports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Master Ledger Register"
        Common_Procedures.RptInputDet.ReportHeading = "Master Ledger Register"
        Common_Procedures.RptInputDet.ReportInputs = "G,L"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_itemreports_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Master Item Register"
        Common_Procedures.RptInputDet.ReportHeading = "Master Item Register"
        Common_Procedures.RptInputDet.ReportInputs = "I,IG"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_dr_master_user_creation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f As New User_Creation
            f.MdiParent = Me
            f.Show()

        Else
            MessageBox.Show("You have no rights for user creation" & Chr(13) & "Only admin can change user rights", "INVALID AUTHORISATION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

    End Sub

    Private Sub mnu_dr_master_unit_creation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim F As New Unit_Creation
        F.MdiParent = Me
        F.Show()
    End Sub

    Private Sub mnu_master_dr_category_creatiion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim F2 As New Cetegory_Creation
        F2.MdiParent = Me
        F2.Show()
    End Sub

    Private Sub mnu_dr_master_transport_creation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New Transport_Creation
        'f.MdiParent = Me
        'f.Show()
    End Sub

    Private Sub mnu_Master_Distribution_ItemGroup_Creation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New ItemGroup_Creation
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Master_Expense_Creation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_Expense_Creation.Click
        Match_UserRights_WithForm("mnu_Master_Expense_Creation", "Expense_Creation")
        Dim f As New Expense_Creation
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Entry_Cost_Shift_Entry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Embroidery_Expense_Entry.Click

        Match_UserRights_WithForm("mnu_Entry_Embroidery_Expense_Entry", "Embroidery_Expense_Entry")

        Dim f As New Embroidery_Expense_Entry
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Embroidery_Report_Inward_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_Inward_Register.Click


    End Sub

    Private Sub mnu_Embroidery_Report_Delivery_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_Delivery_Register.Click

    End Sub

    Private Sub mnu_Report_Earning_Expense_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Earning_Expense_Register.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery earning expense comparision register"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Earning (Production) Vs Expense Comparision Register (Drill Down)"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Earning_Expense_Register1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Earning_Expense_Register1.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery earning expense comparision register1"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Earning (Production) Vs Expense Comparision Register (Table)"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub Mnu_Report_Embroidery_Delivery_Pending_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mnu_Report_Embroidery_Delivery_Pending_Register.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery delivery pending register"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Receipt Vs Delivery Comparision (Drill Down)"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,CL,SZ"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub Mnu_Report_Embroidery_Delivery_Pending_Register1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mnu_Report_Embroidery_Delivery_Pending_Register1.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery delivery pending register1"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Receipt Vs Delivery Comparision (Table)"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,CL,SZ"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Entry_Embroidery_Jobwork_Delivery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Embroidery_Jobwork_Delivery.Click

        Match_UserRights_WithForm("mnu_Entry_Embroidery_Jobwork_Delivery", "Embroidery_Jobwork_Delivery_Entry")

        Dim f As New Embroidery_Jobwork_Delivery_Entry
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Entry_Embroidery_Jobwork_Receipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Embroidery_Jobwork_Receipt.Click

        Match_UserRights_WithForm("mnu_Entry_Embroidery_Jobwork_Receipt", "Embroidery_Jobwork_Receipt_Entry")

        Dim f As New Embroidery_Jobwork_Receipt_Entry
        f.MdiParent = Me
        f.Show()
    End Sub
    Private Sub mnu_Entry_Embroidery_Jobwork_Invoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Embroidery_Jobwork_Invoice.Click

        Match_UserRights_WithForm("mnu_Entry_Embroidery_Jobwork_Invoice", "Embroidery_Jobwork_Invoice")
        Match_UserRights_WithForm("mnu_Entry_Embroidery_Jobwork_Invoice", "Invoice_Embroidery_Pcs_JobWork")

        'Dim f As New Embroidery_Jobwork_Invoice_1
        Dim f As New Invoice_Embroidery_Pcs_JobWork

        f.MdiParent = Me
        f.Show()

    End Sub


    Private Sub mnu_Report_Embroidery_Expenses_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_Expenses.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery expense register"
        Common_Procedures.RptInputDet.ReportHeading = "Register of Expenses"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,EXH"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_Invoices_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_Invoices.Click

    End Sub

    Private Sub mnu_Master_EmployeeCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_EmployeeCreation.Click
        Match_UserRights_WithForm("mnu_Master_EmployeeCreation", "Employee_Creation")
        Dim f As New Employee_Creation
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_QuotationEntry_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_QuotationEntry.Click

        If Common_Procedures.settings.CustomerCode = 5024 Then

            Match_UserRights_WithForm("mnu_Entry_QuotationEntry", "Embroidery_Quotation_Entry_1")

            Dim f As New Embroidery_Quotation_Entry_1

            If Common_Procedures.User.IdNo = 1 Then

                f.previlege = "L"

            Else

                Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_QuotationEntry")

                If I > -1 Then
                    f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
                End If

            End If

            f.MdiParent = Me
            f.Show()

        Else

            Match_UserRights_WithForm("mnu_Entry_QuotationEntry", "Embroidery_Quotation_Entry")
            Dim f As New Embroidery_Quotation_Entry
            If Common_Procedures.User.IdNo = 1 Then

                f.previlege = "L"

            Else
                Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_QuotationEntry")
                If I > -1 Then
                    f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
                End If
            End If

            f.MdiParent = Me
            f.Show()

        End If

    End Sub

    Private Sub mnu_Entry_OrderRegistration_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_OrderRegistration.Click

        Match_UserRights_WithForm("mnu_Entry_OrderRegistration", "Embroidery_Order_Entry")

        Dim f As New Embroidery_Order_Entry

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_OrderRegistration")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Entry_Invoice_Embroidery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Invoice_Embroidery.Click

        If Common_Procedures.settings.CustomerCode = "5027" Then

            Match_UserRights_WithForm("mnu_Entry_Invoice_Embroidery", "Invoice_Embroidery_Pcs_5027")

            Dim f As New Invoice_Embroidery_Pcs_5027

            If Common_Procedures.User.IdNo = 1 Then

                f.previlege = "L"

            Else

                Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_Invoice_Embroidery")
                If I > -1 Then
                    f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
                End If

            End If

            f.MdiParent = Me
            f.Show()

        Else

            Match_UserRights_WithForm("mnu_Entry_Invoice_Embroidery", "Invoice_Embroidery_Pcs")

            Dim f As New Invoice_Embroidery_Pcs

            If Common_Procedures.User.IdNo = 1 Then

                f.previlege = "L"

            Else

                Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_Invoice_Embroidery")
                If I > -1 Then
                    f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
                End If

            End If

            f.MdiParent = Me
            f.Show()

        End If


    End Sub

    Private Sub mnu_Entry_Invoice_Designing_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Invoice_Designing.Click
        Match_UserRights_WithForm("mnu_Entry_Invoice_Designing", "Invoice_Embroidery_Design")

        Dim f As New Invoice_Embroidery_Design
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_Other_Delivery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Other_Delivery.Click

        Match_UserRights_WithForm("mnu_Entry_Other_Delivery", "Sales_Delivery_Free")

        Dim f As New Sales_Delivery_Free
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub CreateTagsForMenus()

        Dim objToolStripItem As ToolStripItem
        Dim Tmp_Tag As String

        For Each objToolStripItem In MenuStrip.Items

            For Each objMenuItem As ToolStripMenuItem In MenuStrip.Items


                Dim i As Integer


                For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1

                    Tmp_Tag = Replace(UCase(objMenuItem.DropDownItems(IntX).Text), "&", "")

                    If Split(Tmp_Tag, ".").GetUpperBound(0) > 0 Then
                        Tmp_Tag = Split(Tmp_Tag, ".")(1)
                    End If

                    objMenuItem.DropDownItems(IntX).Tag = Trim(Tmp_Tag)

                Next


            Next

        Next

    End Sub

    Public Sub Match_UserRights_WithForm(ByVal MenuName As String, ByVal FormName As String)

        If Not Common_Procedures.User.IdNo = 1 Then

            For I As Integer = 0 To UBound(Common_Procedures.UR1.UserInfo, 1)

                If UCase(Common_Procedures.UR1.UserInfo(I, 0)) = UCase(MenuName) Then
                    Common_Procedures.UR1.UserInfo(I, 2) = FormName
                End If

            Next

        End If

    End Sub



    Private Sub mnu_Report_Embroidery_JobWork_Delivery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_JobWork_Delivery.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery jobwork delivery register"
        Common_Procedures.RptInputDet.ReportHeading = "Jobwork Delivery Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_JobWork_Receipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_JobWork_Receipt.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery jobwork inward register"
        Common_Procedures.RptInputDet.ReportHeading = "Jobwork Inward Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub Mnu_Report_Embroidery_Jobwork_Receipt_Pending_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mnu_Report_Embroidery_Jobwork_Receipt_Pending_Register.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery jobwork receipt pending register"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Jobwork Delivery Vs Receipt Comparision (Drill Down)"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub Mnu_Report_Embroidery_Jobwork_Receipt_Pending_Register1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mnu_Report_Embroidery_Jobwork_Receipt_Pending_Register1.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery jobwork receipt pending register1"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Jobwork Delivery Vs Receipt Comparision (Table)"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_General_Delivery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_General_Delivery.Click

        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportGroupName = "Register"

        Common_Procedures.RptInputDet.ReportName = "general delivery register"
        Common_Procedures.RptInputDet.ReportHeading = "delivery register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"


        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_Jobwork_Invoices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_Jobwork_Invoices.Click

        Dim f1 As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery jobwork invoice register"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Jobwork Invoice (Received) Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f1.MdiParent = Me
        f1.Show()

    End Sub

    Private Sub mnu_Report_Embroidery_Quotation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_Quotation.Click



    End Sub

    Private Sub mnu_Report_Embroidery_Designing_Invoices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Embroidery_Designing_Invoices.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery design invoice register"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Designing Work Invoice Register "
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Tools_GST_Offline_GSTR1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_GST_Offline_GSTR1.Click
        Dim F As New frmGSTR1
        F.MdiParent = Me
        frmGSTR1.Show()
    End Sub

    Private Sub mnu_Tools_GST_Offline_GSTR2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_GST_Offline_GSTR2.Click
        'frmGSTR2.Show()
    End Sub

    Private Sub mnu_Tools_GST_Offline_GSTR3B_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_GST_Offline_GSTR3B.Click
        Dim F As New frmGSTR3B
        F.MdiParent = Me
        frmGSTR1.Show()
    End Sub

    Private Sub mnu_Entry_Invoice_Embroidery_Direct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Invoice_Embroidery_Direct.Click

        Match_UserRights_WithForm("mnu_Entry_Invoice_Embroidery_Direct", "Invoice_Embroidery_Pcs_Direct")

        Dim f As New Invoice_Embroidery_Pcs_Direct

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else
            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_Invoice_Embroidery_Direct")
            If I > -1 Then
            f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
        End If
        End If

        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Entry_Misc_GST_Purchase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Misc_GST_Purchase.Click
        Common_Procedures.CompIdNo = 0
        Dim f As New Other_GST_Entry("PURC")
        f.MdiParent = Me
        f.Show()
        If Val(Common_Procedures.CompIdNo) = 0 Then
            f.Close()
            f.Dispose()
        End If
    End Sub

    Private Sub mnu_Entry_Misc_GST_Sales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Misc_GST_Sales.Click
        Common_Procedures.CompIdNo = 0
        Dim f As New Other_GST_Entry("SALE")
        f.MdiParent = Me
        f.Show()
        If Val(Common_Procedures.CompIdNo) = 0 Then
            f.Close()
            f.Dispose()
        End If
    End Sub

    Private Sub mnu_Entry_Misc_GST_Credit_Note_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Misc_GST_Credit_Note.Click
        Common_Procedures.CompIdNo = 0
        Dim f As New Other_GST_Entry("CRNT")
        f.MdiParent = Me
        f.Show()
        If Val(Common_Procedures.CompIdNo) = 0 Then
            f.Close()
            f.Dispose()
        End If
    End Sub

    Private Sub mnu_Entry_Misc_GST_Debit_Note_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Misc_GST_Debit_Note.Click
        Common_Procedures.CompIdNo = 0
        Dim f As New Other_GST_Entry("DRNT")
        f.MdiParent = Me
        f.Show()
        If Val(Common_Procedures.CompIdNo) = 0 Then
            f.Close()
            f.Dispose()
        End If
    End Sub

    Private Sub mnu_Reports_PurchaseRegister_General_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub mnu_Reports_PurchaseRegister_Orderwise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

       
    End Sub

    Private Sub mnu_Reports_PurchaseDetails_General_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_PurchaseDetails_General.Click

    End Sub

    Private Sub mnu_Reports_PurchaseDetails_OrderWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_PurchaseDetails_OrderWise.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportName = "Purchase Details OrderWise"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Details OrderWise"
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_Register_General_Other_GST_Purchase_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Register_General_Other_GST_Purchase_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "General Other Purchase GST Register"
        Common_Procedures.RptInputDet.ReportHeading = "General Other Purchase GST Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,PG,UNT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Register_General_Other_GST_Sales_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Register_General_Other_GST_Sales_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "General Other Sales GST Register"
        Common_Procedures.RptInputDet.ReportHeading = "General Other Sales GST Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,PG,UNT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Register_General_Other_GST_Credit_Note_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Register_General_Other_GST_Credit_Note_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "General Other Credit Note GST Register"
        Common_Procedures.RptInputDet.ReportHeading = "Credit Note GST Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,PG,UNT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Register_General_Other_GST_Debit_Note_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Register_General_Other_GST_Debit_Note_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "General Other Debit Note GST Register"
        Common_Procedures.RptInputDet.ReportHeading = "Debit Note GST Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,PG,UNT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Tools_GST_Offline_Untitlity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_GST_Offline_Untitlity.Click

    End Sub

    Private Sub mnu_Entry_GST_Miscellaneous_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_GST_Miscellaneous.Click

    End Sub

    Private Sub mnu_Master_PayrollEmployeeCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_PayrollEmployeeCreation.Click


        Match_UserRights_WithForm("mnu_Master_PayrollEmployeeCreation", "Payroll_Employee_Creation")

        Dim f As New Payroll_Employee_Creation

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Master_PayrollEmployeeCreation")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Master_PayrollCategoryCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_PayrollCategoryCreation.Click

        Match_UserRights_WithForm("mnu_Master_PayrollCategoryCreation", "Payroll_Category_Creation")

        Dim f As New Payroll_Category_Creation

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Master_PayrollCategoryCreation")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Master_PayrollSalaryTypeCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_PayrollSalaryTypeCreation.Click

        'Dim f As New PayRoll_Salary_PaymentType_Creation
        'f.MdiParent = Me
        'f.Show()

        Match_UserRights_WithForm("mnu_Master_PayrollSalaryTypeCreation", "PayRoll_Salary_PaymentType_Creation")

        Dim f As New PayRoll_Salary_PaymentType_Creation

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Master_PayrollSalaryTypeCreation")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Master_PayrollHolidayCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_PayrollHolidayCreation.Click

        Match_UserRights_WithForm("mnu_Master_PayrollHolidayCreation", "Holidays_Creation")

        Dim f As New Holidays_Creation

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Master_PayrollHolidayCreation")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Master_PayrollShiftCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_PayrollShiftCreation.Click

        'Dim f As New Shift_Creation
        'f.MdiParent = Me
        'f.Show()

        Match_UserRights_WithForm("mnu_Master_PayrollShiftCreation", "Shift_Creation")

        Dim f As New Shift_Creation

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Master_PayrollShiftCreation")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Master_PayrollDepartmentCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_PayrollDepartmentCreation.Click

        Match_UserRights_WithForm("mnu_Master_PayrollDepartmentCreation", "Department_Creation")

        Dim f As New Department_Creation

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Master_PayrollDepartmentCreation")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Payroll_Loan_Opening_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_Loan_Opening.Click
        Common_Procedures.CompIdNo = 0
        Common_Procedures.AdvanceType = "ADVANCE"
        Dim f As New PayRoll_Employee_Salary_Advance_Payment
        f.Advance_Opening_Entry_Status = True
        f.MdiParent = Me
        f.Show()
        If Common_Procedures.CompIdNo = 0 Then
            f.Close()
            f.Dispose()
        End If
    End Sub

    Private Sub mnu_Payroll_Advance_Opening_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_Advance_Opening.Click

        Dim f As New Opening_Balance_Payroll
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Payroll_EmployeeAttendance_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_EmployeeAttendance.Click

        Common_Procedures.CompIdNo = 0

        If Val(Common_Procedures.settings.PAYROLLENTRY_Attendance_In_Hours_Status) = 1 Then
            Dim f As New PayRoll_Employee_Attendance_Hours
            f.MdiParent = Me
            f.Show()
            If Common_Procedures.CompIdNo = 0 Then
                f.Close()
                f.Dispose()
            End If

        Else

            Dim f As New PayRoll_Employee_Attendance_Simple

            Match_UserRights_WithForm("mnu_Payroll_EmployeeAttendance", "PayRoll_Employee_Attendance_Simple")


            If Common_Procedures.User.IdNo = 1 Then

                f.previlege = "L"

            Else

                Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_EmployeeAttendance")
                If I > -1 Then
                    f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
                End If
            End If


            f.MdiParent = Me
            f.Show()

            If Common_Procedures.CompIdNo = 0 Then

                f.Close()
                f.Dispose()
            End If

        End If

    End Sub

    Private Sub mnu_Entry_PayRoll_Employee_Attendance_Log_From_Machine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_PayRoll_Employee_Attendance_Log_From_Machine.Click

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL
        'Dim f As New Payroll_AttendanceLog_FromMachine
        'Dim f As New Payroll_AttendanceLog_FromMachine_Chennai
        'f.MdiParent = Me
        'f.Show()
        'End If


    End Sub


    Private Sub mnu_Entry_Payroll_Employee_Timing_Addition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Payroll_Employee_Timing_Addition.Click

        Dim f As New PayRoll_Employee_Timing_Addition

        Match_UserRights_WithForm("mnu_Entry_Payroll_Employee_Timing_Addition", "PayRoll_Employee_Timing_Addition")


        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_Payroll_Employee_Timing_Addition")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

        If Common_Procedures.CompIdNo = 0 Then
            f.Close()
            f.Dispose()
        End If

    End Sub

    Private Sub mnu_Entry_PayRoll_Employee_Attendance_From_Machine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_PayRoll_Employee_Attendance_From_Machine.Click

        'Dim f As New PayRoll_Employee_Attendance_From_Machine
        'f.MdiParent = Me
        'f.Show()

        Dim f As New PayRoll_Employee_Attendance_From_Machine

        Match_UserRights_WithForm("mnu_Entry_PayRoll_Employee_Attendance_From_Machine", "PayRoll_Employee_Attendance_From_Machine")


        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_PayRoll_Employee_Attendance_From_Machine")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Payroll_Employee_OT_Attendance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_Employee_OT_Attendance.Click

        'Dim f As New PayRoll_Employee_OverTime_Entry
        'f.MdiParent = Me
        'f.Show()

        Dim f As New PayRoll_Employee_OverTime_Entry

        Match_UserRights_WithForm("mnu_Payroll_Employee_OT_Attendance", "PayRoll_Employee_OverTime_Entry")


        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_Employee_OT_Attendance")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Payroll_EmployeeSalaryEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_EmployeeSalaryEntry.Click

        Common_Procedures.CompIdNo = 0
        If Val(Common_Procedures.settings.PAYROLLENTRY_Attendance_In_Hours_Status) = 1 Then

            Dim f As New PayRoll_Employee_Salary_Hours
            f.MdiParent = Me
            f.Show()
            If Common_Procedures.CompIdNo = 0 Then
                f.Close()
                f.Dispose()
            End If

        Else

            'Dim f As New Payroll_Salary_Entry_Details
            'f.MdiParent = Me
            'f.Show()

            Dim f As New Payroll_Salary_Entry_Details

            Match_UserRights_WithForm("mnu_Payroll_EmployeeSalaryEntry", "Payroll_Salary_Entry_Details")

            If Common_Procedures.User.IdNo = 1 Then

                f.previlege = "L"

            Else

                Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_EmployeeSalaryEntry")
                If I > -1 Then
                    f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
                End If
            End If


            f.MdiParent = Me
            f.Show()

            If Common_Procedures.CompIdNo = 0 Then
                f.Close()
                f.Dispose()
            End If

        End If
        'Common_Procedures.CompIdNo = 0
        'Dim f As New PayRoll_Employee_Salary_Entry_Simple
        'f.MdiParent = Me
        'f.Show()
        'If Common_Procedures.CompIdNo = 0 Then
        '    f.Close()
        '    f.Dispose()
        'End If
    End Sub

    Private Sub mnu_Payroll_EmployeeSalaryPaymentEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_EmployeeSalaryPaymentEntry.Click

        Common_Procedures.CompIdNo = 0

        Common_Procedures.AdvanceType = "SALARY"

        Dim f As New PayRoll_Employee_Salary_Advance_Payment

        f.Advance_Opening_Entry_Status = False

        Match_UserRights_WithForm("mnu_Payroll_EmployeeSalaryPaymentEntry", "PayRoll_Employee_Salary_Advance_Payment")

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_EmployeeSalaryPaymentEntry")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

        If Common_Procedures.CompIdNo = 0 Then
            f.Close()
            f.Dispose()
        End If


    End Sub

    Private Sub mnu_Payroll_EmployeeAdvancePaymentEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_EmployeeAdvancePaymentEntry.Click

        Common_Procedures.CompIdNo = 0
        Common_Procedures.AdvanceType = "ADVANCE"

        Dim f As New PayRoll_Employee_Salary_Advance_Payment

        f.Advance_Opening_Entry_Status = False

        Match_UserRights_WithForm("mnu_Payroll_EmployeeAdvancePaymentEntry", "PayRoll_Employee_Salary_Advance_Payment")

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_EmployeeAdvancePaymentEntry")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If

        End If

        f.MdiParent = Me
        f.Show()

        If Common_Procedures.CompIdNo = 0 Then
            f.Close()
            f.Dispose()
        End If

    End Sub

    Private Sub mnu_Payroll_EmployeeSalaryAdvancePaymentEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_EmployeeSalaryAdvancePaymentEntry.Click

        Common_Procedures.CompIdNo = 0
        Common_Procedures.AdvanceType = "SALARYADVANCE"
        Dim f As New PayRoll_Employee_Salary_Advance_Payment

        f.Advance_Opening_Entry_Status = False

        Match_UserRights_WithForm("mnu_Payroll_EmployeeSalaryAdvancePaymentEntry", "PayRoll_Employee_Salary_Advance_Payment")

        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_EmployeeSalaryAdvancePaymentEntry")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If

        End If

        f.MdiParent = Me
        f.Show()

        If Common_Procedures.CompIdNo = 0 Then
            f.Close()
            f.Dispose()
        End If

    End Sub

    Private Sub mnu_Payroll_EmployeeDeductionEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_EmployeeDeductionEntry.Click

        Common_Procedures.CompIdNo = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL

            Dim f2 As New Payroll_Deduction_On_Salary
            f2.MdiParent = Me
            f2.Show()
            If Common_Procedures.CompIdNo = 0 Then
                f2.Close()
                f2.Dispose()
            End If

        Else


            Dim f As New Payroll_Additional_Deduction_Entry

            Match_UserRights_WithForm("mnu_Payroll_EmployeeDeductionEntry", "Payroll_Additional_Deduction_Entry")

            If Common_Procedures.User.IdNo = 1 Then

                f.previlege = "L"

            Else

                Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_EmployeeDeductionEntry")
                If I > -1 Then
                    f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
                End If

            End If

            f.MdiParent = Me
            f.Show()

            If Common_Procedures.CompIdNo = 0 Then
                f.Close()
                f.Dispose()
            End If

        End If

    End Sub

    Private Sub Mnu_Report_Salary_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mnu_Report_Salary_Register.Click

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then
            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "Payroll Salary Register Format1"
            Common_Procedures.RptInputDet.ReportHeading = "Salary Register"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON,PT,CTNAME,EPGNME"
            f.MdiParent = Me
            f.Show()
        Else
            Dim f As New Report_Details_1
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "Payroll Salary Register Simple2"
            Common_Procedures.RptInputDet.ReportHeading = "Salary Register"
            Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON,PT"
            f.MdiParent = Me
            f.Show()
        End If


    End Sub

    Private Sub mnu_Reports_Payroll_NetPay_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_NetPay_Register.Click
        Dim cn1 As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        cn1.Open()

        cmd.Connection = cn1

        cmd.CommandText = "Truncate table EntryTemp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('YES')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('NO')"
        cmd.ExecuteNonQuery()



        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll NetPay Register"
        Common_Procedures.RptInputDet.ReportHeading = "NetPay Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON, BNKNAME,NTPYREG,CTNAME"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Payroll_NetPay_Register_ESIPFGroupWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_NetPay_Register_ESIPFGroupWise.Click
        Dim cn1 As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        cn1.Open()

        cmd.Connection = cn1

        cmd.CommandText = "Truncate table EntryTemp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('YES')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('NO')"
        cmd.ExecuteNonQuery()



        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll NetPay Register - ESI/PF GroupWIse"
        Common_Procedures.RptInputDet.ReportHeading = "NetPay Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON, BNKNAME,NTPYREG,EPGNME"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Payroll_Attendance_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Attendance_Register.Click

    End Sub

    Private Sub mnu_Reports_Payroll_Attendance_MothWise_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Attendance_MothWise_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Attendance MonthWise Register"
        Common_Procedures.RptInputDet.ReportHeading = "Attendance MonthWise Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,*MON"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Payroll_Report_ESI_PF_Register_Format1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_Report_ESI_PF_Register_Format1.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll ESI PF Register"
        Common_Procedures.RptInputDet.ReportHeading = ""
        'Common_Procedures.RptInputDet.ReportHeading = "Payroll ESI PF Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,MON,PT,CTNAME,EPGNME"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Payroll_Report_ESI_PF_Register_Format2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_Report_ESI_PF_Register_Format2.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll ESI PF Register - Format2"
        Common_Procedures.RptInputDet.ReportHeading = ""
        'Common_Procedures.RptInputDet.ReportHeading = "Payroll ESI PF Register"
        Common_Procedures.RptInputDet.ReportInputs = "Z,MON,PT,CTNAME,EPGNME"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_report_OT_Register_Datewise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_report_OT_Register_Datewise.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "ot register datewise"
        Common_Procedures.RptInputDet.ReportHeading = "OT Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Payroll_Reports_OT_Register_EmployeeWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_Reports_OT_Register_EmployeeWise.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "ot register employeewise"
        Common_Procedures.RptInputDet.ReportHeading = "OT Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP*"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Entry_Report_PayRoll_Employee_Payment_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Entry_Report_PayRoll_Employee_Payment_Register.Click

        Dim cn1 As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        cn1.Open()

        cmd.Connection = cn1

        cmd.CommandText = "Truncate table EntryTemp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('ADVANCE')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('LOAN')"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into EntryTemp(Name6) Values ('SALARY')"
        cmd.ExecuteNonQuery()

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Employee Payment Register"
        Common_Procedures.RptInputDet.ReportHeading = "Employee Payment Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,DB,ADVTYPE"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_payRoll_Employee_deduction_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_payRoll_Employee_deduction_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Employee Addition and Deduction Register"
        Common_Procedures.RptInputDet.ReportHeading = "Employee Addition and Deduction Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,EMP"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Payroll_Employee_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Employee_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Employee Register"
        Common_Procedures.RptInputDet.ReportHeading = "Employee Register"
        Common_Procedures.RptInputDet.ReportInputs = "EMP"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Employee_PF_List.Click
        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll PF List Register"
        Common_Procedures.RptInputDet.ReportHeading = "PF list Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON,CTNAME,D,EPGNME"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Payroll_Employee_AbsentList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Employee_AbsentList.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Absent List Register"
        Common_Procedures.RptInputDet.ReportHeading = "Absent List Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Payroll_Employee_LatecomersList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Employee_LatecomersList.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll LateComers List Register"
        Common_Procedures.RptInputDet.ReportHeading = "LateComers List Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Reports_Payroll_Employee_EarlyoutList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Employee_EarlyoutList.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll EarlyOut List Register"
        Common_Procedures.RptInputDet.ReportHeading = "EarlyOut List Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_TimeMissingReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_TimeMissingReport.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Time Missing Register"
        Common_Procedures.RptInputDet.ReportHeading = "Time Missing Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Advance_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Advance_Register.Click
        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Advance Register - Monthwise"
        Common_Procedures.RptInputDet.ReportHeading = "Advance Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP,*MON, CTNAME"
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Tools_Payroll_Columns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_Payroll_Columns.Click
        Dim f As New Payroll_Settings
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Tools_Payroll_Fixed_Values_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Tools_Payroll_Fixed_Values.Click
        Dim f As New Payroll_Option
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Report_Payroll_OT_Hours_Register_DateWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Payroll_OT_Hours_Register_DateWise.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll OT Hours Register - DateWise"
        Common_Procedures.RptInputDet.ReportHeading = "OT LIST"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON,CTNAME,EMP,EPGNME"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_Payroll_OT_Salary_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_Payroll_OT_Salary_Register.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "ot salary register"
        Common_Procedures.RptInputDet.ReportHeading = "OT Salary Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,MON,PT"
        f.MdiParent = Me
        f.Show()

    End Sub


    Private Sub Apply_Form_Menu_Visiblity_Settings()

        'Try

        If System.IO.File.Exists(System.Windows.Forms.Application.StartupPath & "\MENU_VISIBLITY_SETTINGS_" & Common_Procedures.settings.CustomerCode & ".TXT") Then

            Using SR As StreamReader = New StreamReader(System.Windows.Forms.Application.StartupPath & "\MENU_VISIBLITY_SETTINGS_" & Common_Procedures.settings.CustomerCode & ".TXT")

                Dim Setting As String
                Dim LNNO As Integer = 1

                Setting = SR.ReadLine

                '-----------

                While Setting <> Nothing

                    MenuVisiblitySetting.Add(Split(Setting, "$$$")(0), Split(Setting, "$$$")(1))
                    Setting = SR.ReadLine
                    LNNO = LNNO + 1

                End While

                SR.Close()

            End Using


            '------------------------

            For Each objMenuItem As ToolStripMenuItem In Me.MenuStrip.Items

                MenuVisiblitySettings(objMenuItem)

                ReNumberSubMenus(objMenuItem)

            Next

        Else

            MsgBox(" MENU VISIBLITY SETTINGS IS UNAVAIABLE")

            Application.Exit()

        End If

    End Sub

    Private Sub MenuVisiblitySettings(ByVal objMenuItem As ToolStripDropDownItem)

        Dim VisiblitySetting As String
        Dim VisiblitySetting1 As String
        VisiblitySetting = ""

        Try
            VisiblitySetting = MenuVisiblitySetting.Item(objMenuItem.Name)
        Catch
            GoTo a
        End Try

        If VisiblitySetting = "00" Then

            objMenuItem.Visible = False
            objMenuItem.Tag = "INVISIBLE"
            For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1
                If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) > 2 Then
                    Dim ObjTabstripDropItem As ToolStripDropDownItem = objMenuItem.DropDownItems(IntX)
                    MarkInvisible(ObjTabstripDropItem)
                End If
            Next

            GoTo a

        ElseIf VisiblitySetting = "11" Then

            objMenuItem.Visible = True
            objMenuItem.Tag = "VISIBLE"

            For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1
                objMenuItem.DropDownItems(IntX).Visible = True
                objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"
                If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) > 2 Then
                    Dim ObjTabstripDropItem As ToolStripDropDownItem = objMenuItem.DropDownItems(IntX)
                    If ObjTabstripDropItem.HasDropDownItems Then
                        For IntX1 As Integer = 0 To ObjTabstripDropItem.DropDownItems.Count - 1
                            MenuVisiblitySettings(ObjTabstripDropItem.DropDownItems(IntX1))
                        Next
                    End If
                End If
            Next

            GoTo a

        ElseIf VisiblitySetting = "10" Then

            objMenuItem.Visible = True

            For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1

                objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"
                VisiblitySetting1 = ""

                Try
                    VisiblitySetting1 = MenuVisiblitySetting.Item(objMenuItem.DropDownItems(IntX).Name)
                Catch
                    If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) > 1 Then  'if not separator then visible
                        objMenuItem.DropDownItems(IntX).Visible = False
                        objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                    Else
                        objMenuItem.DropDownItems(IntX).Visible = True
                        objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"
                    End If
                    'GoTo B
                    VisiblitySetting1 = "1"
                End Try


                If VisiblitySetting1 = "10" Then

                    'If objMenuItem.DropDownItems(IntX).Name = "mnu_Report_Accesssories_Purchase" Then
                    '    MsgBox("B")
                    'End If
                    objMenuItem.DropDownItems(IntX).Visible = True
                    objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"

                    Dim ObjTabstripDropItem As ToolStripDropDownItem = objMenuItem.DropDownItems(IntX)
                    If ObjTabstripDropItem.HasDropDownItems Then
                        For IntX1 As Integer = 0 To ObjTabstripDropItem.DropDownItems.Count - 1
                            ObjTabstripDropItem.DropDownItems(IntX1).Visible = False
                            ObjTabstripDropItem.DropDownItems(IntX1).Tag = "INVISIBLE"
                            MenuVisiblitySettings(ObjTabstripDropItem.DropDownItems(IntX1))
                        Next
                    End If

                ElseIf VisiblitySetting1 = "11" Then


                    objMenuItem.DropDownItems(IntX).Visible = True
                    objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"

                    Dim ObjTabstripDropItem As ToolStripDropDownItem = objMenuItem.DropDownItems(IntX)
                    If ObjTabstripDropItem.HasDropDownItems Then
                        For IntX1 As Integer = 0 To ObjTabstripDropItem.DropDownItems.Count - 1


                            ObjTabstripDropItem.DropDownItems(IntX1).Visible = True
                            ObjTabstripDropItem.DropDownItems(IntX1).Tag = "VISIBLE"

                            Dim ObjTabstripDropItem1 As ToolStripDropDownItem = ObjTabstripDropItem.DropDownItems(IntX1)
                            For IntX2 As Integer = 0 To ObjTabstripDropItem.DropDownItems.Count - 1
                                ObjTabstripDropItem.DropDownItems(IntX1).Visible = False
                                ObjTabstripDropItem.DropDownItems(IntX1).Tag = "INVISIBLE"
                                MenuVisiblitySettings(ObjTabstripDropItem.DropDownItems(IntX2))
                            Next
                        Next
                    End If

                ElseIf VisiblitySetting1 = "1" Then

                    'If objMenuItem.DropDownItems(IntX).Name = "mnu_Report_Accesssories_Purchase" Then
                    '    MsgBox("B")
                    'End If

                    objMenuItem.DropDownItems(IntX).Visible = True
                    objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"



                Else
                    'MsgBox(objMenuItem.DropDownItems(IntX).Text)
                    objMenuItem.DropDownItems(IntX).Visible = False
                    objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"



                End If



B:


            Next

            Dim ISPREVSEPARATOR As Boolean = False
            Dim LastVisibleLoc As Integer = 0
            Dim LastVisibleSeparatorLoc As Integer = -1

            For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1

                If Trim(objMenuItem.DropDownItems(IntX).Tag) = "VISIBLE" Then

                    If ISPREVSEPARATOR = True And Len(Trim(objMenuItem.DropDownItems(IntX).Text)) <= 1 Then
                        objMenuItem.DropDownItems(IntX).Visible = False
                        objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                    End If

                    If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) <= 1 Then
                        ISPREVSEPARATOR = True
                    Else
                        ISPREVSEPARATOR = False
                    End If

                End If

                If IntX = objMenuItem.DropDownItems.Count - 1 And Len(Trim(objMenuItem.DropDownItems(IntX).Text)) <= 1 Then
                    objMenuItem.DropDownItems(IntX).Visible = False
                    objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                End If

            Next

            For IntX As Integer = objMenuItem.DropDownItems.Count - 1 To 0 Step -1
                If Trim(objMenuItem.DropDownItems(IntX).Tag) = "VISIBLE" Then
                    If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) <= 1 Then
                        objMenuItem.DropDownItems(IntX).Visible = False
                        objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                    End If
                    GoTo A
                End If
            Next




        ElseIf VisiblitySetting = "01" Then

            objMenuItem.Visible = True

            For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1

                objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"
                VisiblitySetting1 = ""

                Try
                    VisiblitySetting1 = MenuVisiblitySetting.Item(objMenuItem.DropDownItems(IntX).Name)
                Catch
                    If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) > 1 Then  'if not separator then visible
                        objMenuItem.DropDownItems(IntX).Visible = False
                        objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                    Else
                        objMenuItem.DropDownItems(IntX).Visible = False
                        objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                    End If
                    'GoTo B
                    VisiblitySetting1 = "0"
                End Try


                If VisiblitySetting1 = "10" Then

                    'If objMenuItem.DropDownItems(IntX).Name = "mnu_Report_Accesssories_Purchase" Then
                    '    MsgBox("B")
                    'End If
                    objMenuItem.DropDownItems(IntX).Visible = True
                    objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"

                    Dim ObjTabstripDropItem As ToolStripDropDownItem = objMenuItem.DropDownItems(IntX)
                    If ObjTabstripDropItem.HasDropDownItems Then
                        For IntX1 As Integer = 0 To ObjTabstripDropItem.DropDownItems.Count - 1
                            ObjTabstripDropItem.DropDownItems(IntX1).Visible = False
                            ObjTabstripDropItem.DropDownItems(IntX1).Tag = "INVISIBLE"
                            MenuVisiblitySettings(ObjTabstripDropItem.DropDownItems(IntX1))
                        Next
                    End If

                ElseIf VisiblitySetting1 = "11" Then


                    objMenuItem.DropDownItems(IntX).Visible = True
                    objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"

                    Dim ObjTabstripDropItem As ToolStripDropDownItem = objMenuItem.DropDownItems(IntX)
                    If ObjTabstripDropItem.HasDropDownItems Then
                        For IntX1 As Integer = 0 To ObjTabstripDropItem.DropDownItems.Count - 1


                            ObjTabstripDropItem.DropDownItems(IntX1).Visible = True
                            ObjTabstripDropItem.DropDownItems(IntX1).Tag = "VISIBLE"

                            Dim ObjTabstripDropItem1 As ToolStripDropDownItem = ObjTabstripDropItem.DropDownItems(IntX1)
                            For IntX2 As Integer = 0 To ObjTabstripDropItem.DropDownItems.Count - 1
                                ObjTabstripDropItem.DropDownItems(IntX1).Visible = False
                                ObjTabstripDropItem.DropDownItems(IntX1).Tag = "INVISIBLE"
                                MenuVisiblitySettings(ObjTabstripDropItem.DropDownItems(IntX2))
                            Next
                        Next
                    End If

                ElseIf VisiblitySetting1 = "1" Then

                    'If objMenuItem.DropDownItems(IntX).Name = "mnu_Report_Accesssories_Purchase" Then
                    '    MsgBox("B")
                    'End If

                    objMenuItem.DropDownItems(IntX).Visible = True
                    objMenuItem.DropDownItems(IntX).Tag = "VISIBLE"



                Else
                    'MsgBox(objMenuItem.DropDownItems(IntX).Text)
                    objMenuItem.DropDownItems(IntX).Visible = False
                    objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"



                End If



c:


            Next

            Dim ISPREVSEPARATOR As Boolean = False
            Dim LastVisibleLoc As Integer = 0
            Dim LastVisibleSeparatorLoc As Integer = -1

            For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1

                If Trim(objMenuItem.DropDownItems(IntX).Tag) = "VISIBLE" Then

                    If ISPREVSEPARATOR = True And Len(Trim(objMenuItem.DropDownItems(IntX).Text)) <= 1 Then
                        objMenuItem.DropDownItems(IntX).Visible = False
                        objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                    End If

                    If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) <= 1 Then
                        ISPREVSEPARATOR = True
                    Else
                        ISPREVSEPARATOR = False
                    End If

                End If

                If IntX = objMenuItem.DropDownItems.Count - 1 And Len(Trim(objMenuItem.DropDownItems(IntX).Text)) <= 1 Then
                    objMenuItem.DropDownItems(IntX).Visible = False
                    objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                End If

            Next

            For IntX As Integer = objMenuItem.DropDownItems.Count - 1 To 0 Step -1
                If Trim(objMenuItem.DropDownItems(IntX).Tag) = "VISIBLE" Then
                    If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) <= 1 Then
                        objMenuItem.DropDownItems(IntX).Visible = False
                        objMenuItem.DropDownItems(IntX).Tag = "INVISIBLE"
                    End If
                    GoTo A
                End If
            Next



        End If

a:

    End Sub

    Private Sub ReNumberMenus(ByVal objMenuItem As ToolStripMenuItem)

        Dim Cnt As Integer = 0

        'If objMenuItem.Name = "ToolStripMenuItem1" Then
        '    MsgBox("A")
        'End If

        For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1

            'If objMenuItem.DropDownItems(IntX).Name = "ToolStripMenuItem1" Then
            '    MsgBox("A")
            'End If

            If objMenuItem.DropDownItems(IntX).Tag = "VISIBLE" Or Len(objMenuItem.DropDownItems(IntX).Tag) = 0 Then

                'If objMenuItem.DropDownItems(IntX).Visible Then

                If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) > 2 Then
                    If Mid(Trim(objMenuItem.DropDownItems(IntX).Text), 2, 1) = "." Then
                        objMenuItem.DropDownItems(IntX).Text = Microsoft.VisualBasic.Right(objMenuItem.DropDownItems(IntX).Text, Len(objMenuItem.DropDownItems(IntX).Text) - 2)
                    End If
                End If

                If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) > 3 Then
                    If Mid(Trim(objMenuItem.DropDownItems(IntX).Text), 3, 1) = "." And Mid(Trim(objMenuItem.DropDownItems(IntX).Text), 2, 1) = " " Then
                        objMenuItem.DropDownItems(IntX).Text = Microsoft.VisualBasic.Right(objMenuItem.DropDownItems(IntX).Text, Len(objMenuItem.DropDownItems(IntX).Text) - 3)
                    End If
                End If

                If Len(Trim(objMenuItem.DropDownItems(IntX).Text)) > 1 Then

                    If Cnt < 26 Then
                        objMenuItem.DropDownItems(IntX).Text = Chr(65 + Cnt) + ". " & objMenuItem.DropDownItems(IntX).Text
                    ElseIf Cnt < 35 Then
                        objMenuItem.DropDownItems(IntX).Text = CStr(Cnt - 25) + ". " & objMenuItem.DropDownItems(IntX).Text
                    End If
                    ReNumberSubMenus(objMenuItem.DropDownItems(IntX))
                    Cnt = Cnt + 1
                End If

            End If
        Next


    End Sub

    Private Sub ReNumberSubMenus(ByVal objDropDownMenuItem As ToolStripDropDownItem)

        Dim Cnt As Integer = 0

        'If objDropDownMenuItem.Name = "ToolStripMenuItem1" Then
        '    MsgBox("A")
        'End If

        For IntX1 As Integer = 0 To objDropDownMenuItem.DropDownItems.Count - 1

            'If objDropDownMenuItem.DropDownItems(IntX1).Name = "ToolStripMenuItem1" Then
            '    MsgBox("A")
            'End If

            If objDropDownMenuItem.DropDownItems(IntX1).Tag <> "INVISIBLE" Or Len(objDropDownMenuItem.DropDownItems(IntX1).Tag) = 0 Then
                'If objDropDownMenuItem.DropDownItems(IntX1).Visible Then

                If Len(Trim(objDropDownMenuItem.DropDownItems(IntX1).Text)) > 2 Then
                    If Mid(Trim(objDropDownMenuItem.DropDownItems(IntX1).Text), 2, 1) = "." Then
                        objDropDownMenuItem.DropDownItems(IntX1).Text = Microsoft.VisualBasic.Right(objDropDownMenuItem.DropDownItems(IntX1).Text, Len(objDropDownMenuItem.DropDownItems(IntX1).Text) - 2)
                    End If
                End If

                If Len(Trim(objDropDownMenuItem.DropDownItems(IntX1).Text)) > 3 Then
                    If Mid(Trim(objDropDownMenuItem.DropDownItems(IntX1).Text), 3, 1) = "." And Mid(Trim(objDropDownMenuItem.DropDownItems(IntX1).Text), 2, 1) = " " Then
                        objDropDownMenuItem.DropDownItems(IntX1).Text = Microsoft.VisualBasic.Right(objDropDownMenuItem.DropDownItems(IntX1).Text, Len(objDropDownMenuItem.DropDownItems(IntX1).Text) - 3)
                    End If
                End If

                If Len(Trim(objDropDownMenuItem.DropDownItems(IntX1).Text)) > 1 Then

                    If Cnt < 26 Then
                        objDropDownMenuItem.DropDownItems(IntX1).Text = Chr(65 + Cnt) + ". " & objDropDownMenuItem.DropDownItems(IntX1).Text
                    ElseIf Cnt < 35 Then
                        objDropDownMenuItem.DropDownItems(IntX1).Text = CStr(Cnt - 25) + ". " & objDropDownMenuItem.DropDownItems(IntX1).Text
                    End If

                    Dim objDropDownItem1 As ToolStripDropDownItem = objDropDownMenuItem.DropDownItems(IntX1)
                    ReNumberSubMenus(objDropDownMenuItem.DropDownItems(IntX1))

                    Cnt = Cnt + 1

                End If

            End If
        Next

    End Sub

    Private Sub MarkInvisible(ByVal objToolStropDropDownItem As ToolStripDropDownItem)

        'If objToolStropDropDownItem.Name = "ToolStripMenuItem1" Then
        '    MsgBox("A")
        'End If

        For IntX As Integer = 0 To objToolStropDropDownItem.DropDownItems.Count - 1

            'If objToolStropDropDownItem.DropDownItems(IntX).Name = "ToolStripMenuItem1" Then
            '    MsgBox("A")
            'End If

            objToolStropDropDownItem.DropDownItems(IntX).Tag = "INVISIBLE"

            If Len(objToolStropDropDownItem.DropDownItems(IntX).Text) > 2 Then
                Dim objToolStropDropDownItem1 As ToolStripDropDownItem = objToolStropDropDownItem.DropDownItems(IntX)

                If objToolStropDropDownItem1.HasDropDownItems Then

                    MarkInvisible(objToolStropDropDownItem1)

                End If
            End If

        Next

    End Sub

    Private Sub MarkVisible(ByVal objToolStropDropDownItem As ToolStripDropDownItem)

        For IntX As Integer = 0 To objToolStropDropDownItem.DropDownItems.Count - 1

            objToolStropDropDownItem.DropDownItems(IntX).Tag = "VISIBLE"

            Dim objToolStropDropDownItem1 As ToolStripDropDownItem = objToolStropDropDownItem.DropDownItems(IntX)

            If objToolStropDropDownItem1.HasDropDownItems Then

                MarkInvisible(objToolStropDropDownItem1)

            End If

        Next

    End Sub

    Private Sub mnu_Accounts_Main_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_Main.Click

    End Sub

    Private Sub mnu_Payroll_EmployeeBonusEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Payroll_EmployeeBonusEntry.Click

        Common_Procedures.CompIdNo = 0
        If Val(Common_Procedures.settings.PAYROLLENTRY_Attendance_In_Hours_Status) = 1 Then
            Dim f As New PayRoll_Employee_Salary_Hours
            f.MdiParent = Me
            f.Show()
            If Common_Procedures.CompIdNo = 0 Then
                f.Close()
                f.Dispose()
            End If

        Else

            'Dim f As New Payroll_Bonus_Entry_Details
            'f.MdiParent = Me
            'f.Show()
            'If Common_Procedures.CompIdNo = 0 Then
            '    f.Close()
            '    f.Dispose()
            'End If

            Dim f As New Payroll_Bonus_Entry_Details

            Match_UserRights_WithForm("mnu_Payroll_EmployeeBonusEntry", "Payroll_Bonus_Entry_Details")

            If Common_Procedures.User.IdNo = 1 Then

                f.previlege = "L"

            Else

                Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_EmployeeBonusEntry")
                If I > -1 Then
                    f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
                End If

            End If

            f.MdiParent = Me
            f.Show()

            If Common_Procedures.CompIdNo = 0 Then
                f.Close()
                f.Dispose()
            End If

        End If

    End Sub

    Private Sub mnu_Reports_Payroll_Salary_Register_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Salary_Register.Click

    End Sub

    Private Sub mnu_Reports_Payroll_Bonus_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Bonus_Register.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Bonus Register Simple"
        Common_Procedures.RptInputDet.ReportHeading = "Bonus Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,PT,CAT"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_Payroll_Bonus_Summary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Bonus_Summary.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Bonus Register Summary"
        Common_Procedures.RptInputDet.ReportHeading = "Bonus Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,PT,CAT"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_Payroll_Addition_Summary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Reports_Payroll_Addition_Summary.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Payroll Addition Deduction Register"
        Common_Procedures.RptInputDet.ReportHeading = "Bonus Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,PT,CAT,EMP"
        f.MdiParent = Me
        f.Show()

    End Sub


    Private Sub mnu_Accounts_VoucherRegisters_Filter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Accounts_VoucherRegisters_Filter.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Voucher Register - General Filter"
        Common_Procedures.RptInputDet.ReportHeading = "Voucher Register - General Filter"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,CRL,DRL"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Master_ComponentCreation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Master_ComponentCreation.Click

        Match_UserRights_WithForm("mnu_Master_ComponentCreation", "Component_Creation")
        Dim f As New Component_Creation
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub mnu_Tools_UpdateApp_Click(sender As Object, e As EventArgs) Handles mnu_Tools_UpdateApp.Click


        UpdatingSW = True

        Dim c As Integer = MsgBox("Are you sure you want to update the software ?", vbYesNo, " Confirm Update ?")

        Try
            If c = vbYes Then

                Dim pth As String = Application.StartupPath & "\connection1.ini"
                Dim fs As FileStream
                Dim w As StreamWriter

                If File.Exists(pth) = False Then
                    fs = New FileStream(pth, FileMode.Create)
                Else
                    fs = New FileStream(pth, FileMode.Open)
                End If
                w = New StreamWriter(fs)

                Dim tmptxt As String

                Do
                    tmptxt = Authenticate.AuthenticationCode(Common_Procedures.ServerName)
                    If Authenticate.RevertAuthenticationCode(tmptxt) = Common_Procedures.ServerName Then
                        w.WriteLine(tmptxt)
                        Exit Do
                    End If
                Loop

                Do
                    tmptxt = Authenticate.AuthenticationCode(Common_Procedures.ServerPassword)
                    If Authenticate.RevertAuthenticationCode(tmptxt) = Common_Procedures.ServerPassword Then
                        w.WriteLine(tmptxt)
                        Exit Do
                    End If
                Loop

                w.WriteLine("")

                Do
                    tmptxt = Authenticate.AuthenticationCode(Common_Procedures.CompanyDetailsDataBaseName)
                    If Authenticate.RevertAuthenticationCode(tmptxt) = Common_Procedures.CompanyDetailsDataBaseName Then
                        w.WriteLine(tmptxt)
                        Exit Do
                    End If
                Loop

                w.WriteLine("")

                Do
                    tmptxt = Authenticate.AuthenticationCode("sa")
                    If Authenticate.RevertAuthenticationCode(tmptxt) = "sa" Then
                        w.WriteLine(tmptxt)
                        Exit Do
                    End If
                Loop

                w.Close()
                fs.Close()
                w.Dispose()
                fs.Dispose()

                DownloadFile(Common_Procedures.AWS_BUCKET_FOR_DOWNLOADER, "", "", "downloader", True)
                Shell(Application.StartupPath & "\downloader\DownloadNApp.exe")

                Me.Close()
                Application.Exit()

            End If

        Catch ex As Exception

            'pb1.Visible = False
            'lblWait.Visible = False
            UpdatingSW = False
            MsgBox(ex.Message & ". Download Fails.")

        End Try

    End Sub

    'Public Function DownloadFile(bucketName As String, folderName As String) As String

    '    If Not Directory.Exists(Application.StartupPath & "\downloader") Then
    '        Directory.CreateDirectory(Application.StartupPath & "\downloader")
    '    End If

    '    Dim target As String = Path.GetTempPath()
    '    Dim returnval As String = ""

    '    folderName = folderName.Replace("\", "/")

    '    Try

    '        Try
    '            If Not AmazonS3Util.DoesS3BucketExist(s3Client, bucketName) Then
    '                returnval = "Bucket does not exist"
    '            Else
    '                Dim request As ListObjectsRequest = New ListObjectsRequest() With {.BucketName = bucketName, .Prefix = folderName & "/"}
    '                Do
    '                    Dim response As ListObjectsResponse = s3Client.ListObjects(request)
    '                    For i As Integer = 1 To response.S3Objects.Count - 1
    '                        Dim entry As S3Object = response.S3Objects(i)
    '                        'MsgBox(entry.GetType.ToString)
    '                        'If Replace(entry.Key, folderName & "/", "") = filename Then
    '                        Dim objRequest As GetObjectRequest = New GetObjectRequest() With {.bucketName = bucketName, .Key = entry.Key}
    '                        Dim objResponse As GetObjectResponse = s3Client.GetObject(objRequest)

    '                        'objResponse.WriteResponseStreamToFile("d:\" & FileName)

    '                        'If Not File.Exists(Application.StartupPath & "\downloader\" & entry.Key.Replace(folderName & "/", "")) And entry.Key.Contains(folderName & "\") Then
    '                        If entry.Key.Contains(folderName & "/") Then
    '                            objResponse.WriteResponseStreamToFile(Application.StartupPath & "\downloader\" & entry.Key.Replace(folderName & "/", ""))
    '                        End If

    '                    Next



    '                    If (response.IsTruncated) Then
    '                        request.Marker = response.NextMarker
    '                    Else
    '                        request = Nothing
    '                    End If

    '                Loop Until IsNothing(request)



    '            End If

    '        Catch ex As AmazonS3Exception

    '            returnval = ex.Message
    '            MsgBox(ex.Message & ". Update Fails")
    '        End Try
    '    Catch ex As Exception

    '        returnval = ex.Message
    '        MsgBox(ex.Message & ". Update Fails")

    '    End Try
    '    Return returnval

    'End Function

    Public Sub DownloadFile(bucketName As String, Optional folderName As String = "", Optional filename As String = "", Optional DestinationFolderName As String = "", Optional IsPublicBucket As Boolean = False)

        'If IsPublicBucket Then
        's3Client = New AmazonS3Client(RegionEndpoint.APSouth1)
        'Else
        s3Client = New AmazonS3Client(Common_Procedures.AWS_ACCESS_KEY, Common_Procedures.AWS_SECRET_KEY, RegionEndpoint.APSouth1)
        'End If


        If Len(Trim(DestinationFolderName)) > 0 Then

            If Not Directory.Exists(Application.StartupPath & "\" & DestinationFolderName) Then
                Directory.CreateDirectory(Application.StartupPath & "\" & DestinationFolderName)
            End If

        End If

        Dim target As String = Path.GetTempPath()
        Dim returnval As String = ""

        folderName = folderName.Replace("\", "/")

        Try

            Try
                If Not AmazonS3Util.DoesS3BucketExist(s3Client, bucketName) Then
                    returnval = "Bucket does not exist"
                Else

                    Dim request As ListObjectsRequest

                    If Len(Trim(folderName)) > 0 Then
                        request = New ListObjectsRequest() With {.BucketName = bucketName, .Prefix = folderName & "/"}
                    Else
                        request = New ListObjectsRequest() With {.BucketName = bucketName, .Prefix = ""}
                    End If

                    Do
                        Dim response As ListObjectsResponse = s3Client.ListObjects(request)
                        For i As Integer = 0 To response.S3Objects.Count - 1
                            Dim entry As S3Object = response.S3Objects(i)
                            'MsgBox(entry.GetType.ToString)
                            'If Replace(entry.Key, folderName & "/", "") = filename Then
                            Dim objRequest As GetObjectRequest = New GetObjectRequest() With {.BucketName = bucketName, .Key = entry.Key}
                            Dim objResponse As GetObjectResponse = s3Client.GetObject(objRequest)

                            If Len(Trim(filename)) > 0 And Len(Trim(folderName)) = 0 Then
                                If UCase(entry.Key).Contains(UCase(filename)) Then
                                    If Len(Trim(DestinationFolderName)) > 0 Then
                                        objResponse.WriteResponseStreamToFile(Application.StartupPath & "\" & DestinationFolderName & "\" & entry.Key.Replace(folderName & "/", ""))
                                    Else
                                        objResponse.WriteResponseStreamToFile(Application.StartupPath & "\" & entry.Key.Replace(folderName & "/", ""))
                                    End If
                                End If


                            ElseIf Len(Trim(filename)) > 0 And Len(Trim(folderName)) > 0 Then
                                'If UCase(entry.Key).Contains(UCase(filename)) Then
                                If Replace(entry.Key, folderName & "/", "") = filename Then
                                    If Len(Trim(DestinationFolderName)) > 0 Then
                                        objResponse.WriteResponseStreamToFile(Application.StartupPath & "\" & DestinationFolderName & "\" & entry.Key.Replace(folderName & "/", ""))
                                    Else
                                        objResponse.WriteResponseStreamToFile(Application.StartupPath & "\" & entry.Key.Replace(folderName & "/", ""))
                                    End If
                                End If
                            End If

                            If Len(Trim(filename)) = 0 And Len(Trim(folderName)) = 0 Then

                                If Len(Trim(DestinationFolderName)) > 0 Then
                                    objResponse.WriteResponseStreamToFile(Application.StartupPath & "\" & DestinationFolderName & "\" & entry.Key.Replace(folderName & "/", ""))
                                Else
                                    objResponse.WriteResponseStreamToFile(Application.StartupPath & "\" & entry.Key.Replace(folderName & "/", ""))
                                End If

                            End If

                        Next

                        If (response.IsTruncated) Then
                            request.Marker = response.NextMarker
                        Else
                            request = Nothing
                        End If

                    Loop Until IsNothing(request)

                End If

            Catch ex As AmazonS3Exception

                returnval = ex.Message
                MsgBox(ex.Message & ". Update Fails")
                s3Client = Nothing

            End Try

        Catch ex As Exception

            returnval = ex.Message
            MsgBox(ex.Message & ". Update Fails")
            s3Client = Nothing

        End Try

        s3Client = Nothing

    End Sub

    Private Sub mnu_Report_Embroidery_Invoices_ByDate_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Invoices_ByDate.Click

        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery invoice register"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Invoice Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EBT"  'EBT
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,EBT"
        'EBT
        '2DT,Z,P,EMB-JOB,EMB-ORD
        f1.MdiParent = Me
        f1.Show()

    End Sub

    Private Sub mnu_Report_Embroidery_Invoices_ByInvNo_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Invoices_ByInvNo.Click

        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery invoice register - 1"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Invoice Register"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EBT"  'EBT
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,EBT"
        f1.MdiParent = Me
        f1.Show()

    End Sub

    Private Sub mnu_Reports_PurchaseDetails_General_ByDate_Click(sender As Object, e As EventArgs) Handles mnu_Reports_PurchaseDetails_General_ByDate.Click

        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Details"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Details"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"

        f.MdiParent = Me
        f.Show()

    End Sub


    Private Sub mnu_Reports_PurchaseDetails_General_ByNo_Click(sender As Object, e As EventArgs) Handles mnu_Reports_PurchaseDetails_General_ByNo.Click

        Dim f As New Report_Details

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Purchase Details - 1"
        Common_Procedures.RptInputDet.ReportHeading = "Purchase Details"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,I"

        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_GSTR1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_GSTR1.Click

        Dim f As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Outward Supply - Registered"
        Common_Procedures.RptInputDet.ReportHeading = "Outward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"

        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_GSTR2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_GSTR2.Click

        Dim f As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Inward Supply - Registered"
        Common_Procedures.RptInputDet.ReportHeading = "Inward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_reports_CreditOrDebit_Note_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_reports_CreditOrDebit_Note.Click

        Dim f As New Report_Details
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Credit or Debit Note Registered"
        Common_Procedures.RptInputDet.ReportHeading = "Credit or Debit Note Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR,CRDR"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_GSTR1_WithPartyName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_GSTR1_WithPartyName.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Outward Supply - Registered with PartyName"
        Common_Procedures.RptInputDet.ReportHeading = "Outward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_GSTR2_WithPartyName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_GSTR2_WithPartyName.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Inward Supply - Registered With PartyName"
        Common_Procedures.RptInputDet.ReportHeading = "Inward Supply - Registered"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_GSTR1_Statewise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_Report_GSTR1_Statewise.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "outward supply - registered statewise"
        Common_Procedures.RptInputDet.ReportHeading = "Outward Supply - Registered statewise"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,ST,UR"
        f.MdiParent = Me
        f.Show()

    End Sub


    Private Sub mnu_report_GSTR1_GstWise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_report_GSTR1_GstWise.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Outward Supply - Registered GSTWise"
        Common_Procedures.RptInputDet.ReportHeading = "Outward Supply - Registered GSTWise"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,UR"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_Main_Click(sender As Object, e As EventArgs) Handles mnu_Reports_Main.Click

    End Sub

    Private Sub mnu_Report_Order_Details_ByDate_Click(sender As Object, e As EventArgs) Handles mnu_Report_Order_Details_ByDate.Click

        Dim f As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Order Details"
        Common_Procedures.RptInputDet.ReportHeading = "Order Details - Ordered by Date"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD"

        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_Order_Details_ByNo_Click(sender As Object, e As EventArgs) Handles mnu_Report_Order_Details_ByNo.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Order Details - a"
        If Common_Procedures.settings.CustomerCode = "5010" Then
            Common_Procedures.RptInputDet.ReportHeading = "Order Details  - Ordered by Sl. No (SPC)"
        ElseIf Common_Procedures.settings.CustomerCode = "5022" Then
            Common_Procedures.RptInputDet.ReportHeading = "Order Details  - Ordered by Sl. No (RVM)"
        ElseIf Common_Procedures.settings.CustomerCode = "5027" Then
            Common_Procedures.RptInputDet.ReportHeading = "Order Details  - Ordered by Sl. No (FWC)"
        End If

        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD"
        'Common_Procedures.RptInputDet.Prev_IdNo_Column = ",,,Ledger_IdNo"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub get_User_Rights()

        Dim cn1 As SqlClient.SqlConnection
        Dim da1 As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        cn1.Open()

        da1 = New SqlClient.SqlDataAdapter("Select* from User_Access_Rights Where User_IdNo = '" & Common_Procedures.User.IdNo & "'", cn1)
        da1.Fill(dt1)

        UserRights.Clear()

        For I As Integer = 0 To dt1.Rows.Count - 1
            If Not IsDBNull(dt1.Rows(I).Item(1)) And Not IsDBNull(dt1.Rows(I).Item(2)) Then
                UserRights.Add(dt1.Rows(I).Item(1), dt1.Rows(I).Item(2))
            End If
        Next

    End Sub

    Private Sub mnu_Report_Embroidery_Quotation_ByDate_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Quotation_ByDate.Click

        Dim f1 As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery quotation register"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Quotation Register - Ordered by Date"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD,CNFS"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_Quotation_ByNo_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Quotation_ByNo.Click

        Dim f1 As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery quotation register - 1"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Quotation Register - Ordered by Serial Number"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD,CNFS"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_Delivery_Register_ByDate_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Delivery_Register_ByDate.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery delivery register"
        Common_Procedures.RptInputDet.ReportHeading = "Delivery Register - Ordered By Date"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,CL,SZ"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_Delivery_Register_By_SlNo_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Delivery_Register_By_SlNo.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery delivery register - 1"
        Common_Procedures.RptInputDet.ReportHeading = "Delivery Register - Ordered by Sl. No."
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,CL,SZ"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_Inward_Register_ByDate_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Inward_Register_ByDate.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery inward register"
        Common_Procedures.RptInputDet.ReportHeading = "Inward Register - Ordered by Date"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,CL,SZ"

        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Report_Embroidery_Inward_Register_By_SlNo_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Inward_Register_By_SlNo.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery inward register - 1"
        Common_Procedures.RptInputDet.ReportHeading = "Inward Register - Ordered by Sl. No."
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,CL,SZ"

        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub OrderedBySPCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Quotation_BySPC.Click

        Dim f1 As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery quotation register - 2"
        If Val(Common_Procedures.settings.CustomerCode) = 5010 Then
            Common_Procedures.RptInputDet.ReportHeading = "Embroidery Quotation Register - Ordered by SPC"
        ElseIf Val(Common_Procedures.settings.CustomerCode) = 5022 Then
            Common_Procedures.RptInputDet.ReportHeading = "Embroidery Quotation Register - Ordered by RVM"
        ElseIf Val(Common_Procedures.settings.CustomerCode) = 5027 Then
            Common_Procedures.RptInputDet.ReportHeading = "Embroidery Quotation Register - Ordered by FWC"
        End If

        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD,CNFS"
        f1.MdiParent = Me
        f1.Show()

    End Sub

    Private Sub mnu_Report_Embroidery_Quotation_To_be_raised_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Quotation_To_be_raised.Click

        Dim f1 As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery pending quotation register"

        If Val(Common_Procedures.settings.CustomerCode) = 5010 Then
            Common_Procedures.RptInputDet.ReportHeading = "LIST OF ORDERS (SPC) WITHOUT QUOTATION"
        ElseIf Val(Common_Procedures.settings.CustomerCode) = 5022 Then
            Common_Procedures.RptInputDet.ReportHeading = "LIST OF ORDERS (RVM) WITHOUT QUOTATION"
        ElseIf Val(Common_Procedures.settings.CustomerCode) = 5027 Then
            Common_Procedures.RptInputDet.ReportHeading = "LIST OF ORDERS (FWC) WITHOUT QUOTATION"
        End If

        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-ORD"
        f1.MdiParent = Me
        f1.Show()

    End Sub

    Private Sub Mnu_Report_Embroidery_Delivery_Pending_Register_Detailed_Click(sender As Object, e As EventArgs) Handles Mnu_Report_Embroidery_Delivery_Pending_Register_Detailed.Click
        Dim f1 As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery delivery pending register detailed"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Delivery Vs Receipt Comparision Detailed (Table)"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,CL,SZ"
        f1.MdiParent = Me
        f1.Show()
    End Sub

    Private Sub mnu_Tools_UpdateInvoiceDCDetails_Click(sender As Object, e As EventArgs) Handles mnu_Tools_UpdateInvoiceDCDetails.Click

        Dim cn1 As SqlClient.SqlConnection


        cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        cn1.Open()

        FieldsCheck.Populate_Invoice_DC_Codes(cn1)

    End Sub

    Private Sub mnu_Entry_ProdnCostEntry_Click(sender As Object, e As EventArgs) Handles mnu_Entry_ProdnCostEntry.Click

        Match_UserRights_WithForm("mnu_Entry_ProdnCostEntry", "Embroidery_Quotation_Entry")

        Dim f As New Embroidery_Production_Cost
        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else
            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Entry_ProdnCostEntry")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If

        f.MdiParent = Me
        f.Show()
    End Sub


    Private Sub mnu_Report_Embroidery_Delivery_Pending_Register_Detailed_WO_OrderNo_Click(sender As Object, e As EventArgs) Handles mnu_Report_Embroidery_Delivery_Pending_Register_Detailed_WO_OrderNo.Click

        Dim f1 As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "embroidery delivery pending register detailed wo orderno"
        Common_Procedures.RptInputDet.ReportHeading = "Embroidery Delivery Vs Receipt Comparision Detailed (Table) Without (Internal) Order No."
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD,CL,SZ"

        f1.MdiParent = Me
        f1.Show()

    End Sub

    Private Sub mnu_Payroll_Employee_PemissionLeaveTime_Click(sender As Object, e As EventArgs) Handles mnu_Payroll_Employee_PemissionLeaveTime.Click

        Dim f As New PayRoll_Employee_PermissionLeaveTime_Entry

        Match_UserRights_WithForm("mnu_Payroll_Employee_PemissionLeaveTime", "PayRoll_Employee_PermissionLeaveTime_Entry")


        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_Employee_PemissionLeaveTime")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Payroll_Employee_Incentive_Click(sender As Object, e As EventArgs) Handles mnu_Payroll_Employee_Incentive.Click

        Dim f As New PayRoll_Employee_Incentive_Entry

        Match_UserRights_WithForm("mnu_Payroll_Employee_Incentive", "PayRoll_Employee_Incentive_Entry")


        If Common_Procedures.User.IdNo = 1 Then

            f.previlege = "L"

        Else

            Dim I As Int16 = Common_Procedures.LocateUserInfo("mnu_Payroll_Employee_Incentive")
            If I > -1 Then
                f.previlege = Common_Procedures.UR1.UserInfo(I, 1)
            End If
        End If


        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_Payroll_Attendance_Register_Order_By_Employee_Click(sender As Object, e As EventArgs) Handles mnu_Reports_Payroll_Attendance_Register_Order_By_Employee.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "payroll attendance register - ordered by employee"
        Common_Procedures.RptInputDet.ReportHeading = "Attendance Register - Ordered by Employee"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Reports_Payroll_Attendance_Register_Order_By_Date_Click(sender As Object, e As EventArgs) Handles mnu_Reports_Payroll_Attendance_Register_Order_By_Date.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "payroll attendance register - ordered by date"
        Common_Procedures.RptInputDet.ReportHeading = "Attendance Register - Ordered by Date"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Payroll_Incentive_Register_OBDate_Click(sender As Object, e As EventArgs) Handles mnu_Payroll_Incentive_Register_OBDate.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "payroll incentive register - ordered by date"
        Common_Procedures.RptInputDet.ReportHeading = "Incentive Register - Ordered by Date"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Payroll_Incentive_Register_OBEmployee_Click(sender As Object, e As EventArgs) Handles mnu_Payroll_Incentive_Register_OBEmployee.Click

        Dim f As New Report_Details_1
        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "payroll incentive register - ordered by employee"
        Common_Procedures.RptInputDet.ReportHeading = "Incentive Register - Ordered by Employee"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,EMP"
        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_Order_Details_ByColour_ByDate_Click(sender As Object, e As EventArgs) Handles mnu_Report_Order_Details_ByColour_ByDate.Click

        Dim f As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Order Details by Colour"
        Common_Procedures.RptInputDet.ReportHeading = "Order Details (With Colour Breakup)- Ordered by Date"
        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD"

        f.MdiParent = Me
        f.Show()

    End Sub

    Private Sub mnu_Report_Order_Details_ByNo_ByDate_Click(sender As Object, e As EventArgs) Handles mnu_Report_Order_Details_ByNo_ByDate.Click

        Dim f As New Report_Details_1

        Common_Procedures.RptInputDet.ReportGroupName = "Register"
        Common_Procedures.RptInputDet.ReportName = "Order Details by Colour - a"
        If Common_Procedures.settings.CustomerCode = "5010" Then
            Common_Procedures.RptInputDet.ReportHeading = "Order Details (With Colour Breakup) - Ordered by Sl. No (SPC)"
        ElseIf Common_Procedures.settings.CustomerCode = "5022" Then
            Common_Procedures.RptInputDet.ReportHeading = "Order Details (With Colour Breakup) - Ordered by Sl. No (RVM)"
        ElseIf Common_Procedures.settings.CustomerCode = "5027" Then
            Common_Procedures.RptInputDet.ReportHeading = "Order Details (With Colour Breakup) - Ordered by Sl. No (FWC)"
        End If

        Common_Procedures.RptInputDet.ReportInputs = "2DT,Z,P,EMB-JOB,EMB-ORD"
        f.MdiParent = Me
        f.Show()

    End Sub

End Class
