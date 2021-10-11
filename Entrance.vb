Imports System.IO

Public Class Entrance

    Private Sub Entrance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim cn1 As SqlClient.SqlConnection
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            Check_Software_Registration()
            Change_DateFormat()
            Get_ServerDetails()

            If Trim(Common_Procedures.ServerName) = "" Then
                MessageBox.Show("Invalid Connection File Details", "INVALID SERVER DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()
                Exit Sub
            End If


            Common_Procedures.ConnectionString_Master = ""
            Common_Procedures.ConnectionString_CompanyGroupdetails = ""
            Common_Procedures.Connection_String = ""

            Common_Procedures.ConnectionString_Master = Common_Procedures.Create_Sql_ConnectionString("master")
            Common_Procedures.ConnectionString_CompanyGroupdetails = Common_Procedures.Create_Sql_ConnectionString(Common_Procedures.CompanyDetailsDataBaseName)

            Attach_Existing_Databases()
            Check_Create_CompanyGroupDetails_DB()

            If Trim(UCase(Common_Procedures.ServerDataBaseLocation_InExTernalUSB)) = "USB" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then
                If Common_Procedures.is_Database_File_Exists(Common_Procedures.CompanyDetailsDataBaseName) = False Then
                    MessageBox.Show("Invalid Database File - " & Common_Procedures.CompanyDetailsDataBaseName, "INVALID DB FILE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Application.Exit()
                    Exit Sub
                End If
            End If
         
            cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

            cn1.Open()

            Get_CompanyGroupDetails_SettingsValue(cn1)

            Check_Update_SystemDateTime(cn1)

            get_UserName_Password(cn1, Common_Procedures.settings.SoftWare_UserType)

            cn1.Close()
            cn1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT OPEN...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Entrance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim cn1 As SqlClient.SqlConnection
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim n As Integer = 0
        Dim CompgrpCondt As String = ""
        Dim DBName As String = ""
        Dim ShowSTS As Boolean = False

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        cn1.Open()

        Get_CompanyGroupDetails_SettingsValue(cn1)

        CompgrpCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" Then
            'CompgrpCondt = " Where (CompanyGroup_Type <> 'ACCOUNT')"
        Else
            CompgrpCondt = " Where (CompanyGroup_Type <> 'UNACCOUNT')"
        End If

        da2 = New SqlClient.SqlDataAdapter("select * from CompanyGroup_Head " & CompgrpCondt & " Order by To_Date desc, CompanyGroup_IdNo, From_Date", cn1)
        dt2 = New DataTable
        da2.Fill(dt2)

        dgv_Details.Rows.Clear()

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1
                DBName = Common_Procedures.get_Company_DataBaseName(Trim(Val(dt2.Rows(i).Item("CompanyGroup_IdNo").ToString)))
                '****************

                ShowSTS = False
                If Trim(UCase(Common_Procedures.ServerDataBaseLocation_InExTernalUSB)) = "USB" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then
                    If Common_Procedures.is_Database_File_Exists(DBName) = True Then ShowSTS = True
                Else
                    ShowSTS = True
                End If

                If ShowSTS = True Then

                    n = dgv_Details.Rows.Add()

                    dgv_Details.Rows(n).Cells(0).Value = "  " & dt2.Rows(i).Item("CompanyGroup_Name").ToString
                    dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("CompanyGroup_IdNo").ToString
                    dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Financial_Range").ToString
                End If
            Next i

        End If

        If dgv_Details.Enabled = True And dgv_Details.Visible = True Then
            If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            If dgv_Details.Rows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = dgv_Details.Rows(0)
                selectedRow.Selected = True
            End If
        End If

        dt2.Dispose()
        da2.Dispose()

        cn1.Close()
        cn1.Dispose()

    End Sub

    Private Sub Entrance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING SOFTWARE...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    Me.Close()
                    Application.Exit()
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Create_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Create.Click
        If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
        dgv_Details.Focus()
        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)

        Dim f As New CompanyGroup_Creation
        f.ShowDialog()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click

        Dim cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim IdNo As Integer, Nr As Integer
        'Dim DBPartName As String

        Try

            IdNo = Trim(dgv_Details.CurrentRow.Cells(1).Value)

            Common_Procedures.CompGroupIdNo = 0
            Common_Procedures.CompGroupName = ""
            Common_Procedures.CompGroupFnRange = ""

            Common_Procedures.Connection_String = ""
            Common_Procedures.DataBaseName = ""

            If Val(IdNo) <> 0 Then

                Common_Procedures.CompGroupIdNo = Val(IdNo)
                Common_Procedures.CompGroupName = Trim(dgv_Details.CurrentRow.Cells(0).Value)
                Common_Procedures.CompGroupFnRange = Trim(dgv_Details.CurrentRow.Cells(2).Value)

                Common_Procedures.DataBaseName = Common_Procedures.get_Company_DataBaseName(Trim(Val(IdNo)))


                Common_Procedures.Connection_String = Common_Procedures.Create_Sql_ConnectionString(Common_Procedures.DataBaseName)


                If Trim(UCase(Common_Procedures.ServerDataBaseLocation_InExTernalUSB)) = "USB" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1130" Then
                    If Common_Procedures.is_Database_File_Exists(Common_Procedures.DataBaseName) = False Then
                        MessageBox.Show("Invalid Database File - " & Common_Procedures.DataBaseName, "INVALID COMPANY GROUP SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
                cn1.Open()

                cmd.Connection = cn1

                cmd.CommandText = "Update FinancialRange_Head set Financial_Range = '" & Trim(Common_Procedures.CompGroupFnRange) & "'"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    cmd.CommandText = "Insert into FinancialRange_Head(Financial_Range) values ('" & Trim(Common_Procedures.CompGroupFnRange) & "')"
                    cmd.ExecuteNonQuery()
                End If

                cmd.Dispose()
                cn1.Close()


                MDIParent1.Show()

               

                cn1.Dispose()

                Me.Hide()

            Else
                MessageBox.Show("Select Company Group Name", "INVALID COMPANY GROUP SELECTION....", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN COMPANYGROUP SELECTION....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        If e.KeyCode = 13 Then
            btn_Open_Click(sender, e)
        End If
    End Sub

    Private Sub Check_Software_Registration()

        'Dim FontsPath As String
        'Dim RegFile As String

        'FontsPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts)

        'RegFile = Trim(FontsPath) & "\amutha.ttf"

        'If File.Exists(RegFile) = False Then
        '    MessageBox.Show("Invalid Software Registration", "TSOFT LICENSE....", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Me.Close()
        '    Application.Exit()
        '    End
        'End If

        'OSPath = Path.GetPathRoot(Environment.SystemDirectory)
        'OSPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        'OSPath = Path.GetPathRoot(Environment.CurrentDirectory)
        'MessageBox.Show(OSPath, "WINDOWS PATH...", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Change_DateFormat()

        Dim DD_Format As String
        Try

            DD_Format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern()

            If Trim(DD_Format) <> "dd/MM/yyyy" Then
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Control Panel\International", "sShortDate", "dd/MM/yyyy")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE CHANGING DATE FORMAT IN CONTROL PANEL..", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Get_ServerDetails()

        Dim pth As String, ConStr As String
        Dim a() As String
        Dim fs As FileStream
        Dim r As StreamReader
        Dim w As StreamWriter

        Try

            If InStr(1, Trim(LCase(Application.StartupPath)), "\bin\debug") > 0 Then
                Common_Procedures.AppPath = Replace(Trim(LCase(Application.StartupPath)), "\bin\debug", "")
            Else
                Common_Procedures.AppPath = Application.StartupPath
            End If

            pth = Trim(Common_Procedures.AppPath) & "\connection.ini"

            Common_Procedures.ServerName = ""
            Common_Procedures.ServerPassword = ""
            Common_Procedures.ServerWindowsLogin = ""
            Common_Procedures.ServerDataBaseLocation_InExTernalUSB = ""

            Common_Procedures.ConnectionString_CompanyGroupdetails = ""
            Common_Procedures.ConnectionString_Master = ""
            Common_Procedures.Connection_String = ""
            Common_Procedures.DataBaseName = ""

            If File.Exists(pth) = False Then
                fs = New FileStream(pth, FileMode.Create)
                w = New StreamWriter(fs)
                w.WriteLine(SystemInformation.ComputerName & "\tsoft,tsoftsql")
                w.Close()
                fs.Close()
                w.Dispose()
                fs.Dispose()
            End If

            ConStr = ""
            If File.Exists(pth) = True Then
                fs = New FileStream(pth, FileMode.Open)
                r = New StreamReader(fs)
                ConStr = r.ReadLine
                r.Close()
                fs.Close()
                r.Dispose()
                fs.Dispose()
            End If

            If Trim(ConStr) <> "" Then
                a = Split(ConStr, ",")
                If UBound(a) >= 0 Then Common_Procedures.ServerName = Trim(a(0))
                If UBound(a) >= 1 Then Common_Procedures.ServerPassword = Trim(a(1))
                If UBound(a) >= 2 Then Common_Procedures.ServerWindowsLogin = Trim(a(2))
                If UBound(a) >= 3 Then
                    If Trim(a(3)) <> "" Then
                        If InStr(1, Trim(UCase(a(3))), "TSOFT") > 0 And InStr(1, Trim(UCase(a(3))), "COMPANYGROUP") > 0 Then
                            Common_Procedures.CompanyDetailsDataBaseName = Trim(a(3))
                        End If
                    End If
                End If
                If UBound(a) >= 4 Then Common_Procedures.ServerDataBaseLocation_InExTernalUSB = Trim(a(4))
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT OPEN...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Attach_Existing_Databases()
        '-----
    End Sub

    Private Sub Check_Create_CompanyGroupDetails_DB()
        Dim cn2 As SqlClient.SqlConnection
        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As DataTable
        Dim Nr As Integer

        Try

            cn2 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
            cn2.Open()

            Try


                da2 = New SqlClient.SqlDataAdapter("Select name from sysdatabases where name = '" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "'", cn2)
                dt2 = New DataTable
                da2.Fill(dt2)

                Nr = 0
                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("name").ToString) = False Then
                        Nr = 1
                    End If
                End If

                dt2.Dispose()
                da2.Dispose()

                If Nr = 0 Then
                    Create_CompanyDetails_Database(cn2)
                Else
                    DoesTableExist("AutoBackup_Path_Head")
                End If

                cn2.Close()
                cn2.Dispose()

            Catch ex As Exception
                MessageBox.Show(ex.Message, "ERROR IN CHECKING/CREATING COMPANYGROUP DETAILS DB...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()

            End Try

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID MASTER DATABASE CONNECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()

        End Try

    End Sub

    Private Sub Create_CompanyDetails_Database(ByVal cnmas As SqlClient.SqlConnection)
        Dim Cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        'Dim ConnString As String

        cmd.Connection = cnmas

        cmd.CommandText = "Create Database " & Trim(Common_Procedures.CompanyDetailsDataBaseName)
        cmd.ExecuteNonQuery()


        Cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

        'If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "WIN" Then
        '    ConnString = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";Integrated Security=True"
        'Else
        '    ConnString = "Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False"
        'End If
        'Cn1 = New SqlClient.SqlConnection(ConnString)

        Cn1.Open()

        cmd.Connection = Cn1

        cmd.CommandText = "CREATE TABLE [CompanyGroup_Head] ( [CompanyGroup_IdNo] [smallint] NOT NULL, [CompanyGroup_Name] [varchar](100) NOT NULL, [From_Date] [smalldatetime] NOT NULL, [To_Date] [smalldatetime] NOT NULL, [Financial_Range] [varchar](10) NOT NULL, CONSTRAINT [PK_CompanyGroup_Head] PRIMARY KEY CLUSTERED ( [CompanyGroup_IdNo] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Settings_Head] ( [Auto_SlNo] [int] IDENTITY(1,1) NOT NULL, [Cc_No] [varchar](50) NULL CONSTRAINT [DF__Settings___Cc_No__2FEF161B]  DEFAULT (''), [SoftWare_UserType] [varchar](50) NULL CONSTRAINT [DF_Settings_Head_Software_Type]  DEFAULT (''), [Sdd] [smalldatetime] NULL, CONSTRAINT [PK_Settings_Head] PRIMARY KEY CLUSTERED ( [Auto_SlNo] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [User_Head] ( [User_IdNo] [smallint] NOT NULL, [User_Name] [varchar](50) NOT NULL, [Sur_Name] [varchar](50) NOT NULL, [Account_Password] [varchar](50) NULL CONSTRAINT [DF_User_Head_Account_Password]  DEFAULT (''), [UnAccount_Password] [varchar](50) NULL CONSTRAINT [DF_User_Head_UnAccount_Password]  DEFAULT (''), CONSTRAINT [PK_User_Head] PRIMARY KEY CLUSTERED  ( [User_IdNo] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [User_Access_Rights] ( [User_IdNo] [smallint] NOT NULL, [Entry_Code] [varchar](100) NOT NULL, [Access_Type] [varchar](50) NULL, CONSTRAINT [PK_User_Access_Details] PRIMARY KEY NONCLUSTERED  ( [User_IdNo], [Entry_Code] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.Dispose()

        Call FieldCheck_CompanyGroupDetails_Db(Cn1)

        Call DefaultValues_CompanyGroupDetails_Db(Cn1)

        Cn1.Close()
        Cn1.Dispose()

    End Sub

    Private Sub FieldCheck_CompanyGroupDetails_Db(ByVal cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim Dat As Date
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        On Error Resume Next

        cmd.Connection = cn1

        cmd.Connection = cn1

        cmd.CommandText = "  ALTER TABLE Settings_Head ALTER COLUMN Autobackup_Path_Server varchar(200)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Autobackup_Path_Server varchar(200) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Autobackup_Path_Server = '' Where Autobackup_Path_Server is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Auto_SlNo [int] IDENTITY (1, 1) NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add SoftWare_UserType varchar(50) default 'SINGLE USER'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set SoftWare_UserType = 'SINGLE USER' Where SoftWare_UserType is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Basic_Wages_For_Esi Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Basic_Wages_For_Esi = 0 where Basic_Wages_For_Esi is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Basic_Pay_For_Epf Numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Basic_Pay_For_Epf = 0 where Basic_Pay_For_Epf is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Sdd smalldatetime"
        cmd.ExecuteNonQuery()
        Dat = #1/1/2000#
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@SystemDate", Dat.ToShortDateString)
        cmd.CommandText = "Update Settings_Head set Sdd = @SystemDate Where Sdd is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table Settings_Head add Cc_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Cc_No = '' Where Cc_No is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table CompanyGroup_Head add CompanyGroup_Type varchar(50) default 'ACCOUNT'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update CompanyGroup_Head set CompanyGroup_Type  = 'ACCOUNT' Where CompanyGroup_Type  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [User_Access_Rights](	[User_IdNo] [smallint] NOT NULL, 	[Entry_Code] [varchar](100) NOT NULL, 	[Access_Type] [varchar](50) NULL,  CONSTRAINT [PK_User_Access_Details] PRIMARY KEY NONCLUSTERED  ( 	[User_IdNo] , [Entry_Code] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add SoftWare_UserType varchar(50) default 'SINGLE USER'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set SoftWare_UserType = 'SINGLE USER' Where SoftWare_UserType is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Sdd smalldatetime"
        cmd.ExecuteNonQuery()

        Dat = #1/1/2000#
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@SystemDate", Dat.ToShortDateString)

        cmd.CommandText = "Update Settings_Head set Sdd = @SystemDate Where Sdd is Null"
        cmd.ExecuteNonQuery()

        cmd.Parameters.Clear()

        da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", cn1)
        dt1 = New DataTable
        da1.Fill(dt1)

        If dt1.Rows.Count = 0 Then
            cmd.CommandText = "truncate table Settings_Head"
            cmd.ExecuteNonQuery()

            Dat = #1/1/2000#
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SystemDate", Dat.ToShortDateString)

            cmd.CommandText = "Insert into Settings_Head(SoftWare_UserType, Sdd,Autobackup_Path_Server) values ('SINGLE USER', @SystemDate,'" & Trim(Common_Procedures.AppPath) & "') "
            cmd.ExecuteNonQuery()
        End If

        dt1.Dispose()
        da1.Dispose()

        cmd.Dispose()


    End Sub

    Private Sub DefaultValues_CompanyGroupDetails_Db(ByVal cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim Dat As Date

        cmd.Connection = cn1

        cmd.CommandText = "truncate table Settings_Head"
        cmd.ExecuteNonQuery()

        Dat = #1/1/2000#
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@SystemDate", Dat.ToShortDateString)

        cmd.CommandText = "Insert into Settings_Head(SoftWare_UserType, Sdd) values ('SINGLE USER', @SystemDate) "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from User_Head where user_idno = 0 or user_idno = 1"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password) values (0, '', '', '', '') "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password) values (1, 'Admin', 'Admin', 'TSOFT', 'TS2') "
        cmd.ExecuteNonQuery()

        cmd.Dispose()

    End Sub

    Private Sub Check_Update_SystemDateTime(ByVal Cn2 As SqlClient.SqlConnection)

        Dim cmd As New SqlClient.SqlCommand
        Dim Dset As New DataSet
        Dim DAdapt As New SqlClient.SqlDataAdapter
        Dim dat As Date
        Dim lckdt As Date
        Dim licdt As Date
        Dim Nr As Integer


        Try

            cmd.Connection = Cn2

            dat = #1/1/2000#

            If Val(Common_Procedures.settings.CustomerDBCode) < 1000 Or Val(Common_Procedures.settings.CustomerDBCode) > 9999 Then
                lckdt = #3/3/2019#
                licdt = #3/3/2019#

            Else
                lckdt = #11/11/2099#
                licdt = #12/12/2099#

            End If

            If IsDate(Common_Procedures.settings.Sdd) = True Then
                dat = Common_Procedures.settings.Sdd
            End If

            If Val(Common_Procedures.settings.CustomerDBCode) < 1000 Or Val(Common_Procedures.settings.CustomerDBCode) > 9999 Then
                If DateDiff("d", dat.ToShortDateString, Date.Today.ToShortDateString) < 0 Then
                    MessageBox.Show("Invalid system date - set correct date", "ERROR IN SYSTEM DATE CHECKING", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Me.Close()
                    Application.Exit()
                End If
            End If

            If DateDiff("d", dat.ToShortDateString, Date.Today.ToShortDateString) > 0 Then
                dat = Date.Today.ToShortDateString

                cmd.Connection = Cn2

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@SystemDate", dat.Date)

                Nr = 0
                cmd.CommandText = "Update Settings_Head set Sdd = @SystemDate"
                Nr = cmd.ExecuteNonQuery()

                If Nr = 0 Then
                    cmd.CommandText = "Insert into Settings_Head(Sdd) values (@SystemDate) "
                    cmd.ExecuteNonQuery()
                End If

            End If

            'If DateDiff("d", lckdt.ToShortDateString, Date.Today.ToShortDateString) > 0 Then
            '    MessageBox.Show("Database '" & Common_Procedures.DataBaseName & "' cannot be opened. It has been marked SUSPECT by recovery. See the SQL Server errorlog for more information. (Microsoft SQL Server, Error: 926)", "TSOFT TEXTILE...", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
            '    Me.Close()
            '    Application.Exit()
            'End If

            'If DateDiff("d", licdt.ToShortDateString, Date.Today.ToShortDateString) > 0 Then
            '    MessageBox.Show("SoftWare license expires on " & lckdt.ToShortDateString, "TSOFT TEXTILE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Me.Close()
            '    Application.Exit()
            'End If

            Demo_Data_Checking(Cn2)



        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN DATE CHECKING", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        cmd.Dispose()

    End Sub

    Private Sub Get_CompanyGroupDetails_SettingsValue(ByVal Cn1 As SqlClient.SqlConnection)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            Call FieldCheck_CompanyGroupDetails_Db(Cn1)

            Common_Procedures.settings.CompanyName = ""
            Common_Procedures.settings.CustomerCode = ""
            Common_Procedures.settings.CustomerDBCode = ""
            Common_Procedures.settings.AutoBackUp_Date = #1/1/1900#

            Common_Procedures.settings.SoftWare_UserType = ""
            Common_Procedures.settings.Sdd = #1/1/2000#

            da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", Cn1)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0).Item("SoftWare_UserType").ToString) = False Then
                    Common_Procedures.settings.SoftWare_UserType = dt1.Rows(0).Item("SoftWare_UserType").ToString()
                End If
                If IsDBNull(dt1.Rows(0).Item("Sdd").ToString) = False Then
                    If IsDate(dt1.Rows(0).Item("Sdd").ToString) = True Then
                        Common_Procedures.settings.Sdd = dt1.Rows(0).Item("sdd").ToString()
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("Cc_No").ToString) = False Then
                    Common_Procedures.settings.CustomerDBCode = dt1.Rows(0).Item("Cc_No").ToString()
                    Common_Procedures.settings.CustomerCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.settings.CustomerDBCode), 4)
                End If

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN GETTING SETTINGS VALUES...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub get_UserName_Password(ByVal cN1 As SqlClient.SqlConnection, ByVal SoftWare_UserType As String)

        Common_Procedures.User.IdNo = 0
        Common_Procedures.User.Name = ""
        Common_Procedures.User.Type = ""

        Login.ShowDialog()

        If Val(Common_Procedures.User.IdNo) <> 0 Then
            get_User_AccessRights(cN1)

        Else
            MessageBox.Show("Invalid Login", "LOGIN FAILED...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Application.Exit()

        End If

    End Sub

    Private Sub get_User_AccessRights(ByVal Cn1 As SqlClient.SqlConnection)

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Common_Procedures.UR.Ledger_Creation = ""
        Common_Procedures.UR.Area_Creation = ""
        Common_Procedures.UR.Item_Creation = ""
        Common_Procedures.UR.ItemGroup_Creation = ""
        Common_Procedures.UR.Unit_Creation = ""
        Common_Procedures.UR.Variety_Creation = ""
        Common_Procedures.UR.Waste_Creation = ""
        Common_Procedures.UR.Size_Creation = ""
        Common_Procedures.UR.Transport_Creation = ""
        Common_Procedures.UR.Ledger_OpeningBalance = ""
        Common_Procedures.UR.Opening_Stock = ""
        Common_Procedures.UR.Category_Creation = ""

        Common_Procedures.UR.Purchase_Entry = ""
        Common_Procedures.UR.Sales_Entry = ""
        Common_Procedures.UR.WasteSales_Entry = ""

        Common_Procedures.UR.Knotting_Entry = ""
        Common_Procedures.UR.Knotting_Invoice_Entry = ""

        Common_Procedures.UR.Voucher_Entry = ""

        Common_Procedures.UR.Accounts_Ledger_Report = ""
        Common_Procedures.UR.Accounts_GroupLedger_Report = ""
        Common_Procedures.UR.Accounts_DayBook = ""
        Common_Procedures.UR.Accounts_AllLedger = ""
        Common_Procedures.UR.Accounts_TB = ""
        Common_Procedures.UR.Accounts_Profit_Loss = ""
        Common_Procedures.UR.Accounts_BalanceSheet = ""
        Common_Procedures.UR.Accounts_CustomerBills = ""
        Common_Procedures.UR.Report_Purchase_Register = ""
        Common_Procedures.UR.Report_Sales_Register = ""
        Common_Procedures.UR.Report_Stock_Register = ""
        Common_Procedures.UR.Report_Minimum_Stock_Register = ""
        Common_Procedures.UR.Report_Knotting_Reports = ""

        Common_Procedures.UR.Invoice_Saara_Entry = ""
        Common_Procedures.UR.Bill_Entry_Saara = ""
        Common_Procedures.UR.Delivery_Saara_Entry = ""

        Common_Procedures.UR.Printing_Invoice_Entry = ""
        Common_Procedures.UR.Printing_Order_Entry = ""
        Common_Procedures.UR.Printing_Order_Program_Entry = ""


        If Val(Common_Procedures.User.IdNo) = 1 Then

            Common_Procedures.UR.Ledger_Creation = "~L~"
            Common_Procedures.UR.Area_Creation = "~L~"
            Common_Procedures.UR.Item_Creation = "~L~"
            Common_Procedures.UR.ItemGroup_Creation = "~L~"
            Common_Procedures.UR.Unit_Creation = "~L~"
            Common_Procedures.UR.Variety_Creation = "~L~"
            Common_Procedures.UR.Waste_Creation = "~L~"
            Common_Procedures.UR.Size_Creation = "~L~"
            Common_Procedures.UR.Transport_Creation = "~L~"
            Common_Procedures.UR.Ledger_OpeningBalance = "~L~"
            Common_Procedures.UR.Opening_Stock = "~L~"
            Common_Procedures.UR.Category_Creation = "~L~"

            Common_Procedures.UR.Purchase_Entry = "~L~"
            Common_Procedures.UR.Sales_Entry = "~L~"
            Common_Procedures.UR.WasteSales_Entry = "~L~"

            Common_Procedures.UR.Knotting_Entry = "~L~"
            Common_Procedures.UR.Knotting_Invoice_Entry = "~L~"
            Common_Procedures.UR.Invoice_Saara_Entry = "~L~"
            Common_Procedures.UR.Bill_Entry_Saara = "~L~"
            Common_Procedures.UR.Delivery_Saara_Entry = "~L~"

            Common_Procedures.UR.Printing_Invoice_Entry = "~L~"
            Common_Procedures.UR.Printing_Order_Entry = "~L~"
            Common_Procedures.UR.Printing_Order_Program_Entry = "~L~"


            Common_Procedures.UR.Voucher_Entry = "~L~"

            Common_Procedures.UR.Accounts_Ledger_Report = "~L~"
            Common_Procedures.UR.Accounts_GroupLedger_Report = "~L~"
            Common_Procedures.UR.Accounts_DayBook = "~L~"
            Common_Procedures.UR.Accounts_AllLedger = "~L~"
            Common_Procedures.UR.Accounts_TB = "~L~"
            Common_Procedures.UR.Accounts_Profit_Loss = "~L~"
            Common_Procedures.UR.Accounts_BalanceSheet = "~L~"
            Common_Procedures.UR.Accounts_CustomerBills = "~L~"
            Common_Procedures.UR.Report_Purchase_Register = "~L~"
            Common_Procedures.UR.Report_Sales_Register = "~L~"
            Common_Procedures.UR.Report_Stock_Register = "~L~"
            Common_Procedures.UR.Report_Minimum_Stock_Register = "~L~"
            Common_Procedures.UR.Report_Knotting_Reports = "~L~"


        Else

            Da = New SqlClient.SqlDataAdapter("select * from User_Access_Rights where user_idno = " & Str(Common_Procedures.User.IdNo), Cn1)
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Select Case Trim(UCase(Dt1.Rows(i).Item("Entry_Code").ToString))
                        Case "MASTER_LEDGER_CREATION"
                            Common_Procedures.UR.Ledger_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_AREA_CREATION"
                            Common_Procedures.UR.Area_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_ITEM_CREATION"
                            Common_Procedures.UR.Item_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_ITEMGROUP_CREATION"
                            Common_Procedures.UR.ItemGroup_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_UNIT_CREATION"
                            Common_Procedures.UR.Unit_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_CATEGORY_CREATION"
                            Common_Procedures.UR.Category_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_VARIETY_CREATION"
                            Common_Procedures.UR.Variety_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_WASTE_CREATION"
                            Common_Procedures.UR.Waste_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_SIZE_CREATION"
                            Common_Procedures.UR.Size_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_TRANSPORT_CREATION"
                            Common_Procedures.UR.Transport_Creation = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_LEDGER_OPENING_STOCK"
                            Common_Procedures.UR.Ledger_OpeningBalance = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "MASTER_OPENING_STOCK"
                            Common_Procedures.UR.Opening_Stock = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ENTRY_PURCHASE"
                            Common_Procedures.UR.Purchase_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ENTRY_SALES_ENTRY"
                            Common_Procedures.UR.Sales_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ENTRY_TAX_SALES_ENTRY"
                            Common_Procedures.UR.Tax_Sales_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ENTRY_LABOUR_SALES_ENTRY"
                            Common_Procedures.UR.Labour_Sales_Entry = Dt1.Rows(i).Item("Access_Type").ToString


                        Case "ENTRY_SALES_QUOTATION_ENTRY"
                            Common_Procedures.UR.sales_Quotation_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ENTRY_SALES_DELIVERY_ENTRY"
                            Common_Procedures.UR.Delivery_entry = Dt1.Rows(i).Item("Access_Type").ToString


                        Case "ENTRY_WASTESALES"
                            Common_Procedures.UR.WasteSales_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ENTRY_KNOTTING_ENTRY"
                            Common_Procedures.UR.Knotting_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ENTRY_KNOTTING_INVOICE_ENTRY"
                            Common_Procedures.UR.Knotting_Invoice_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ENTRY_VOUCHER"
                            Common_Procedures.UR.Voucher_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "SAARA_INVOICE_ENTRY"
                            Common_Procedures.UR.Invoice_Saara_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "SAARA_BILL_ENTRY"
                            Common_Procedures.UR.Bill_Entry_Saara = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "SAARA_DELIVERY_ENTRY"
                            Common_Procedures.UR.Delivery_Saara_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ACCOUNTS_LEDGER_REPORT"
                            Common_Procedures.UR.Accounts_Ledger_Report = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "PRINTING_INVOICE_ENTRY"
                            Common_Procedures.UR.Printing_Invoice_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "PRINTING_ORDER_ENTRY"
                            Common_Procedures.UR.Printing_Order_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "PRINTING_ORDER_PROGRAM_ENTRY"
                            Common_Procedures.UR.Printing_Order_Program_Entry = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ACCOUNTS_GROUPLEDGER_REPORT"
                            Common_Procedures.UR.Accounts_GroupLedger_Report = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ACCOUNTS_DAYBOOK"
                            Common_Procedures.UR.Accounts_DayBook = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ACCOUNTS_ALLLEDGER"
                            Common_Procedures.UR.Accounts_AllLedger = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ACCOUNTS_TB"
                            Common_Procedures.UR.Accounts_TB = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ACCOUNTS_PROFIT_LOSS"
                            Common_Procedures.UR.Accounts_Profit_Loss = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ACCOUNTS_BALANCESHEET"
                            Common_Procedures.UR.Accounts_BalanceSheet = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "ACCOUNTS_CUSTOMERBILLS"
                            Common_Procedures.UR.Accounts_CustomerBills = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "REPORT_PURCHASE_REGISTER"
                            Common_Procedures.UR.Report_Purchase_Register = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "REPORT_SALES_REGISTER"
                            Common_Procedures.UR.Report_Sales_Register = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "REPORT_STOCK_REGISTER"
                            Common_Procedures.UR.Report_Stock_Register = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "REPORT_MINIMUMSTOCK_REGISTER"
                            Common_Procedures.UR.Report_Minimum_Stock_Register = Dt1.Rows(i).Item("Access_Type").ToString

                        Case "REPORT_KNOTTING_REPORTS"
                            Common_Procedures.UR.Report_Knotting_Reports = Dt1.Rows(i).Item("Access_Type").ToString

                    End Select

                Next

            End If

        End If

        Dt1.Dispose()
        Da.Dispose()

    End Sub

    Public Sub determine_User_Access_Rights(ByVal Cn1 As SqlClient.SqlConnection)

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        With Common_Procedures.UR1
            If Common_Procedures.User.IdNo = 1 Then
                ReDim .UserInfo(0, 0)
                .UserInfo(0, 0) = "ALL"
                Exit Sub
            Else

                Da = New SqlClient.SqlDataAdapter("select * from User_Access_Rights where user_idno = " & Str(Common_Procedures.User.IdNo), Cn1)
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    ReDim .UserInfo(Dt1.Rows.Count - 1, 2)

                    For i As Integer = 0 To Dt1.Rows.Count - 1

                        If Not IsDBNull(Dt1.Rows(i).Item(1)) And Not IsDBNull(Dt1.Rows(i).Item(2)) Then
                            'MsgBox(Dt1.Rows(i).Item(1))
                            .UserInfo(i, 0) = Dt1.Rows(i).Item(1)
                            .UserInfo(i, 1) = Dt1.Rows(i).Item(2)
                        End If

                    Next

                End If


            End If

        End With

    End Sub



    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'MsgBox("CommonAppDataPath = " & Application.CommonAppDataPath)
        'MsgBox("ExecutablePath = " & Application.ExecutablePath)
        'MsgBox("StartupPath = " & Application.StartupPath)
        'MsgBox("UserAppDataPath = " & Application.UserAppDataPath)
        'MsgBox("LocalUserAppDataPath = " & Application.LocalUserAppDataPath)
    End Sub

    Public Function DoesTableExist(ByVal tblName As String) As Boolean
        Dim Cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim Nr As Integer = 0
        Dim ServNam As String = Common_Procedures.get_Server_SystemName

        On Error Resume Next

        Cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

        Cn1.Open()

        Dim restrictions(3) As String
        restrictions(2) = tblName
        Dim dbTbl As DataTable = Cn1.GetSchema("Tables", restrictions)

        cmd.Connection = Cn1

        If dbTbl.Rows.Count = 0 Then

            DoesTableExist = False


            cmd.CommandText = "CREATE TABLE [AutoBackup_Path_Head](	[Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,	[Computer_Name] [varchar](100) NULL,	[App_Path] [varchar](200) NULL,  CONSTRAINT [PK_AutoBackup_Path_Head] PRIMARY KEY CLUSTERED (  [Auto_SlNo]) ON [PRIMARY]) ON [PRIMARY]"
            Nr = cmd.ExecuteNonQuery()



            cmd.Dispose()

        Else

            DoesTableExist = True

        End If

        If ServNam = Trim(UCase(SystemInformation.ComputerName)) Then
            Nr = 0
            cmd.CommandText = "update Settings_Head set Autobackup_Path_Server = '" & Trim(Common_Procedures.AppPath) & "' where SoftWare_UserType <> ''"
            Nr = cmd.ExecuteNonQuery()

        End If

        dbTbl.Dispose()
        Cn1.Close()
        Cn1.Dispose()
    End Function

    Private Sub Demo_Data_Checking(ByVal Cn1 As SqlClient.SqlConnection)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Nr As Integer = 0
        Dim DataCount As Integer = 0
        Dim LedCount As Integer = 0
        Dim vDBName As String = ""


        If Val(Common_Procedures.settings.CustomerDBCode) > 1000 And Val(Common_Procedures.settings.CustomerDBCode) < 9999 Then
            Exit Sub
        End If

        DataCount = 0
        LedCount = 0

        da1 = New SqlClient.SqlDataAdapter("Select * from CompanyGroup_Head Order by CompanyGroup_IdNo", Cn1)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then

            For I = 0 To dt1.Rows.Count - 1

                vDBName = Common_Procedures.get_Company_DataBaseName(Trim(Val(dt1.Rows(I).Item("CompanyGroup_IdNo").ToString)))

                da1 = New SqlClient.SqlDataAdapter("Select * from master..sysdatabases Where name = '" & Trim(vDBName) & "'", Cn1)
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then

                    da1 = New SqlClient.SqlDataAdapter("Select count(Voucher_Code) from " & vDBName & "..voucher_head", Cn1)
                    dt3 = New DataTable
                    da1.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        If Not IsDBNull(dt3.Rows(0).Item(0)) Then
                            DataCount = DataCount + Val(dt3.Rows(0).Item(0))
                        End If
                    End If
                    dt3.Clear()

                    da1 = New SqlClient.SqlDataAdapter("Select count(Ledger_IdNo) from " & vDBName & "..ledger_head where Ledger_IdNo > 100", Cn1)
                    dt3 = New DataTable
                    da1.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        If Not IsDBNull(dt3.Rows(0).Item(0)) Then
                            LedCount = LedCount + Val(dt3.Rows(0).Item(0))
                        End If
                    End If
                    dt3.Clear()

                End If
                dt2.Clear()

            Next I

        End If

        dt1.Clear()

        If LedCount > 10 Or DataCount > 50 Then
            MessageBox.Show("SoftWare Trial Period Expires", "embroITS", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Me.Close()
            Application.Exit()
        End If

        dt1.Dispose()
        dt2.Dispose()
        dt3.Dispose()
        da1.Dispose()

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
