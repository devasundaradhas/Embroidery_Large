Public Class User_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private New_Entry As Boolean = False

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Open.Visible = False

        lbl_UserID.Text = ""
        lbl_UserID.ForeColor = Color.Black

        txt_Name.Text = ""
        txt_AcPwd.Text = ""
        txt_UnAcPwd.Text = ""

        dgv_Details.Rows.Clear()
        Add_EntryNames()

    End Sub

    Private Sub Add_EntryNames()


        Dim Sno As Integer = 0
        Dim n As Integer


        With dgv_Details

            .Rows.Clear()


            Dim objToolStripItem As ToolStripItem
            Dim Tmp_Tag As String
            Dim Tmp_Tag1() As String

            ' For Each objToolStripItem In MDIParent1.MenuStrip.Items

            For Each objMenuItem As ToolStripMenuItem In MDIParent1.MenuStrip.Items

                If objMenuItem.Name <> "mnu_Action_Main" And objMenuItem.Name <> "mnu_WindowsMenu_Main" And _
                    objMenuItem.Name <> "mnu_HelpMenu_Main" And objMenuItem.Name <> "mnu_ExitMenu_Main" Then

                    For IntX As Integer = 0 To objMenuItem.DropDownItems.Count - 1

                        If objMenuItem.DropDownItems(IntX).Enabled Then

                            If objMenuItem.DropDownItems(IntX).Name <> "mnu_Company_Exit" And objMenuItem.DropDownItems(IntX).Name <> "mnu_Company_SelectCompany" And objMenuItem.DropDownItems(IntX).Name <> "mnu_Company_ChangePeriod" Then

                                Tmp_Tag = Replace(UCase(objMenuItem.DropDownItems(IntX).Text), "&", "")


                                If Split(Tmp_Tag, ".").GetUpperBound(0) > 0 Then
                                    Tmp_Tag1 = Split(Tmp_Tag, ".")
                                    Tmp_Tag1(0) = ""
                                    Tmp_Tag = Join(Tmp_Tag1, ".")
                                End If

                                If Len(Trim(Tmp_Tag)) > 0 Then
                                    n = .Rows.Add()
                                    .Rows(n).Cells(0).Value = n + 1

                                    If Microsoft.VisualBasic.Left(Trim(Tmp_Tag), 1) = "." Then
                                        .Rows(n).Cells(1).Value = Trim(Microsoft.VisualBasic.Right(Trim(Tmp_Tag), Len(Trim(Tmp_Tag)) - 1))
                                    Else
                                        .Rows(n).Cells(1).Value = Trim(Tmp_Tag)
                                    End If

                                    .Rows(n).Cells(1).Value = Replace(UCase(Trim(objMenuItem.Text)), "&", "") & " >> " & .Rows(n).Cells(1).Value
                                    .Rows(n).Cells(8).Value = objMenuItem.DropDownItems(IntX).Name

                                End If
                            End If

                        End If
                    Next

                End If


            Next

            'Next


        End With

        'With dgv_Details
        '    .Rows.Clear()
        '    Sno = 0

        '    n = .Rows.Add()
        '    .Rows(n).Cells(0).Value = n + 1
        '    .Rows(n).Cells(1).Value = "LEDGER CREATION"
        '    .Rows(n).Cells(8).Value = "MASTER_LEDGER_CREATION"

        '    n = .Rows.Add()
        '    .Rows(n).Cells(0).Value = n + 1
        '    .Rows(n).Cells(1).Value = "AREA CREATION"
        '    .Rows(n).Cells(8).Value = "MASTER_AREA_CREATION"

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1048" Then
        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "SAARA INVOICE ENTRY"
        '        .Rows(n).Cells(8).Value = "SAARA_INVOICE_ENTRY"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "SAARA DELIVERY ENTRY"
        '        .Rows(n).Cells(8).Value = "SAARA_DELIVERY_ENTRY"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "BILL ENTRY"
        '        .Rows(n).Cells(8).Value = "SAARA_BILL_ENTRY"

        '    End If

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "LEDGER OPENING BALANCE"
        '        .Rows(n).Cells(8).Value = "MASTER_LEDGER_OPENING_STOCK"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "KNOTTING ENTRY"
        '        .Rows(n).Cells(8).Value = "ENTRY_KNOTTING_ENTRY"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "KNOTTING INVOICE ENTRY"
        '        .Rows(n).Cells(8).Value = "ENTRY_KNOTTING_INVOICE_ENTRY"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "REPORTS"
        '        .Rows(n).Cells(8).Value = "REPORT_KNOTTING_REPORTS"

        '    Else

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1003" Then
        '            .Rows(n).Cells(1).Value = "COUNT CREATION"
        '        Else
        '            .Rows(n).Cells(1).Value = "ITEM CREATION"
        '        End If
        '        .Rows(n).Cells(8).Value = "MASTER_ITEM_CREATION"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1003" Then
        '            .Rows(n).Cells(1).Value = "ITEM DESCRIPTION CREATION"
        '        Else
        '            .Rows(n).Cells(1).Value = "ITEM GROUP CREATION"
        '        End If

        '        .Rows(n).Cells(8).Value = "MASTER_ITEMGROUP_CREATION"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "UNIT CREATION"
        '        .Rows(n).Cells(8).Value = "MASTER_UNIT_CREATION"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "CATEGORY CREATION"
        '        .Rows(n).Cells(8).Value = "MASTER_CATEGORY_CREATION"

        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1003" Then
        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "VARIETY CREATION"
        '            .Rows(n).Cells(8).Value = "MASTER_VARIETY_CREATION"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "WASTE CREATION"
        '            .Rows(n).Cells(8).Value = "MASTER_WASTE_CREATION"

        '        End If

        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Then

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "SIZE CREATION"
        '            .Rows(n).Cells(8).Value = "MASTER_SIZE_CREATION"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "TRANSPORT CREATION"
        '            .Rows(n).Cells(8).Value = "MASTER_TRANSPORT_CREATION"

        '        End If

        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1008" Then

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "LEDGER OPENING BALANCE"
        '            .Rows(n).Cells(8).Value = "MASTER_LEDGER_OPENING_STOCK"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "ITEM OPENING STOCK"
        '            .Rows(n).Cells(8).Value = "MASTER_ITEM_OPENING_STOCK"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "PURCHASE ENTRY"
        '            .Rows(n).Cells(8).Value = "ENTRY_PURCHASE"

        '        End If


        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Then
        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "INVOICE ENTRY"
        '            .Rows(n).Cells(8).Value = "ENTRY_SALES_ENTRY"

        '        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1091" Then '-----Arul Engineering
        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "SALES QUOTATION ENTRY"
        '            .Rows(n).Cells(8).Value = "ENTRY_SALES_QUOTATION_ENTRY"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "SALES DELIVERY ENTRY"
        '            .Rows(n).Cells(8).Value = "ENTRY_SALES_DELIVERY_ENTRY"
        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "TAX INVOICE ENTRY"
        '            .Rows(n).Cells(8).Value = "ENTRY_TAX_SALES_ENTRY"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "LABOUR INVOICE ENTRY"
        '            .Rows(n).Cells(8).Value = "ENTRY_LABOUR_SALES_ENTRY"


        '        Else
        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "SALES ENTRY"
        '            .Rows(n).Cells(8).Value = "ENTRY_SALES_ENTRY"
        '        End If




        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1003" Then
        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "WASTE SALES ENTRY"
        '            .Rows(n).Cells(8).Value = "ENTRY_WASTESALES"
        '        End If

        '    End If


        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1008" Then

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "VOUCHER ENTRY"
        '        .Rows(n).Cells(8).Value = "ENTRY_VOUCHER"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "ACCOUNTS - LEDGER REPORT"
        '        .Rows(n).Cells(8).Value = "ACCOUNTS_LEDGER_REPORT"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "ACCOUNTS - GROUP LEDGER REPORT"
        '        .Rows(n).Cells(8).Value = "ACCOUNTS_GROUPLEDGER_REPORT"


        '        'n = .Rows.Add()
        '        '.Rows(n).Cells(0).Value = n + 1
        '        '.Rows(n).Cells(1).Value = "ACCOUNTS - DAYBOOK"
        '        '.Rows(n).Cells(8).Value = "ACCOUNTS_DAYBOOK"

        '        'n = .Rows.Add()
        '        '.Rows(n).Cells(0).Value = n + 1
        '        '.Rows(n).Cells(1).Value = "ACCOUNTS - ALL LEDGER"
        '        '.Rows(n).Cells(8).Value = "ACCOUNTS_ALLLEDGER"


        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "ACCOUNTS - TB"
        '        .Rows(n).Cells(8).Value = "ACCOUNTS_TB"

        '        'n = .Rows.Add()
        '        '.Rows(n).Cells(0).Value = n + 1
        '        '.Rows(n).Cells(1).Value = "ACCOUNTS - PROFIT & LOSS"
        '        '.Rows(n).Cells(8).Value = "ACCOUNTS_PROFIT_LOSS"

        '        'n = .Rows.Add()
        '        '.Rows(n).Cells(0).Value = n + 1
        '        '.Rows(n).Cells(1).Value = "ACCOUNTS - BALANCE SHEET"
        '        '.Rows(n).Cells(8).Value = "ACCOUNTS_BALANCESHEET"

        '    End If


        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1016" Then

        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1008" Then
        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "PURCHASE REPORTS"
        '            .Rows(n).Cells(8).Value = "REPORT_PURCHASE_REGISTER"
        '        End If

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1

        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1008" Then
        '            .Rows(n).Cells(1).Value = "INVOICE REPORTS"
        '        Else
        '            .Rows(n).Cells(1).Value = "SALES REPORTS"
        '        End If
        '        .Rows(n).Cells(8).Value = "REPORT_SALES_REGISTER"

        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1008" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1091" Then

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "STOCK REPORTS"
        '            .Rows(n).Cells(8).Value = "REPORT_STOCK_REGISTER"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "MINIMUM STOCK REPORT"
        '            .Rows(n).Cells(8).Value = "REPORT_MINIMUMSTOCK_REGISTER"

        '        End If

        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1095" Then

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "PRINTING INVOICE ENTRY"
        '            .Rows(n).Cells(8).Value = "PRINTING_INVOICE_ENTRY"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "PRINTING ORDER ENTRY"
        '            .Rows(n).Cells(8).Value = "PRINTING_ORDER_ENTRY"

        '            n = .Rows.Add()
        '            .Rows(n).Cells(0).Value = n + 1
        '            .Rows(n).Cells(1).Value = "PRINTING ORDER PROGRAM ENTRY"
        '            .Rows(n).Cells(8).Value = "PRINTING_ORDER_PROGRAM_ENTRY"

        '        End If



        '    End If

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1193" Then ' USR TWO WHEELER STAND 

        '        .Rows.Clear()

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "TOCKEN ENTRY"
        '        .Rows(n).Cells(8).Value = "ENTRY_TOCKEN_ENTRY"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "MONTHLY TOCKEN ENTRY"
        '        .Rows(n).Cells(8).Value = "ENTRY_MONTHLY_TOCKEN_ENTRY"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "REPORT"
        '        .Rows(n).Cells(8).Value = "REPORT"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "MASTER"
        '        .Rows(n).Cells(8).Value = "MASTER"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "VOUCHER"
        '        .Rows(n).Cells(8).Value = "VOUCHER"

        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "ACCOUNTS"
        '        .Rows(n).Cells(8).Value = "ACCOUNTS"
        '        n = .Rows.Add()
        '        .Rows(n).Cells(0).Value = n + 1
        '        .Rows(n).Cells(1).Value = "OPENING"
        '        .Rows(n).Cells(8).Value = "OPENING"


        '    End If


        'End With

    End Sub

    Private Sub move_record(ByVal idno As Integer)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim I As Integer, J As Integer
        Dim All_STS As Boolean, Add_STS As Boolean
        Dim Edit_STS As Boolean, Del_STS As Boolean
        Dim View_STS As Boolean, Ins_STS As Boolean

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from User_Head where User_IdNo = " & Str(Val(idno)), con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_UserID.Text = dt1.Rows(0).Item("User_IdNo").ToString
                txt_Name.Text = dt1.Rows(0).Item("User_Name").ToString
                txt_AcPwd.Text = dt1.Rows(0).Item("Account_Password").ToString
                txt_UnAcPwd.Text = dt1.Rows(0).Item("UnAccount_Password").ToString
                txt_RealName.Text = dt1.Rows(0).Item("User_Real_Name").ToString

                da2 = New SqlClient.SqlDataAdapter("select * from User_Access_Rights where User_IdNo = " & Str(Val(lbl_UserID.Text)) & " Order by User_IdNo, Entry_Code", con)
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                Add_EntryNames()

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        With dgv_Details

                            For J = 0 To .Rows.Count - 1

                                If Trim(UCase(dgv_Details.Rows(J).Cells(8).Value)) = Trim(UCase(dt2.Rows(I).Item("Entry_Code").ToString)) Then

                                    All_STS = False : Add_STS = False
                                    Edit_STS = False : Del_STS = False
                                    View_STS = False : Ins_STS = False

                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~L~") Then All_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~A~") Then Add_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~E~") Then Edit_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~D~") Then Del_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~V~") Then View_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~I~") Then Ins_STS = True

                                    .Rows(J).Cells(2).Value = All_STS
                                    .Rows(J).Cells(3).Value = Add_STS
                                    .Rows(J).Cells(4).Value = Edit_STS
                                    .Rows(J).Cells(5).Value = Del_STS
                                    .Rows(J).Cells(6).Value = View_STS
                                    .Rows(J).Cells(7).Value = Ins_STS

                                    Exit For

                                End If

                            Next

                        End With

                    Next I

                End If

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            cmd.Connection = con

            cmd.CommandText = "Delete from User_Access_Rights Where User_IdNo = " & Str(Val(lbl_UserID.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from User_Head Where User_IdNo = " & Str(Val(lbl_UserID.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 User_IdNo from User_Head WHERE User_IdNo <> 0 Order by User_IdNo"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                move_record(movno)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 User_IdNo from User_Head WHERE User_IdNo <> 0 Order by User_IdNo desc"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                move_record(movno)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer
        Dim OrdByNo As Integer

        Try

            OrdByNo = Val(lbl_UserID.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 User_IdNo from User_Head WHERE User_IdNo <> 0 and user_idno > " & Str(Val(OrdByNo)) & " Order by User_IdNo"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                move_record(movno)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer
        Dim OrdByNo As Integer

        Try

            OrdByNo = Val(lbl_UserID.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 User_IdNo from User_Head where User_IdNo <> 0 and user_idno < " & Str(Val(OrdByNo)) & " Order by User_IdNo desc"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                move_record(movno)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As Integer = 0
        Dim NewNo As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(User_IdNo) from User_Head where User_IdNo <> 0", con)
            da.Fill(dt1)

            NewNo = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NewNo = Val(dt1.Rows(0)(0).ToString)
                End If
            End If

            NewNo = NewNo + 1

            lbl_UserID.Text = NewNo
            lbl_UserID.ForeColor = Color.Red

            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select User_Name from User_Head order by User_Name", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "User_Name"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        pnl_Back.Enabled = False

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_idno As Integer = 0
        Dim Sno As Integer = 0
        Dim db_idno As Integer = 0
        Dim cr_idno As Integer = 0
        Dim VouAmt As Decimal = 0

        Dim UnPwd As String
        Dim Sur As String
        Dim r As String

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Window", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" Then
            If Trim(txt_UnAcPwd.Text) = "" Then
                MessageBox.Show("Invalid UnAccount PassWord", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_UnAcPwd.Enabled And txt_UnAcPwd.Visible Then txt_UnAcPwd.Focus()
            End If
            If Trim(txt_AcPwd.Text) = Trim(txt_UnAcPwd.Text) Then
                MessageBox.Show("Both PassWords Should'nt be Same", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_AcPwd.Enabled Then txt_AcPwd.Focus()
            End If
            UnPwd = Trim(txt_UnAcPwd.Text)

        Else
            UnPwd = ""
            If Trim(txt_UnAcPwd.Text) <> "" Then UnPwd = Trim(txt_UnAcPwd.Text)
            If Trim(UnPwd) = "" And Val(lbl_UserID.Text) = 1 Then UnPwd = "TSUA"

        End If

        Sur = Common_Procedures.Remove_NonCharacters(txt_Name.Text)

        tr = con.BeginTransaction

        Try

            If New_Entry = True Then
                da = New SqlClient.SqlDataAdapter("select max(User_IdNo) from User_Head", con)
                da.SelectCommand.Transaction = tr
                dt4 = New DataTable
                da.Fill(dt4)

                NewNo = 0
                If dt4.Rows.Count > 0 Then
                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        NewNo = Int(Val(dt4.Rows(0)(0).ToString))
                    End If
                End If
                dt4.Clear()

                NewNo = Val(NewNo) + 1

                lbl_UserID.Text = NewNo

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            If New_Entry = True Then
                cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password ,User_Real_Name) Values (" & Str(Val(NewNo)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "', '" & Trim(txt_AcPwd.Text) & "', '" & Trim(UnPwd) & "','" & txt_RealName.Text & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update User_Head set User_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "', Account_Password = '" & Trim(txt_AcPwd.Text) & "', UnAccount_Password = '" & Trim(txt_UnAcPwd.Text) & "',User_Real_Name = '" & txt_RealName.Text & "' Where User_IdNo = " & Str(Val(lbl_UserID.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from User_Access_Rights Where User_IdNo = " & Str(Val(lbl_UserID.Text))
            cmd.ExecuteNonQuery()

            Sno = 0
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" And Trim(dgv_Details.Rows(i).Cells(8).Value) <> "" Then

                    Sno = Sno + 1

                    If Val(lbl_UserID.Text) = 1 Then
                        r = "~L~A~E~D~V~I~"

                    Else
                        r = "~"
                        If dgv_Details.Rows(i).Cells(2).Value = True Then r = r & "L~"
                        If dgv_Details.Rows(i).Cells(3).Value = True Then r = r & "A~"
                        If dgv_Details.Rows(i).Cells(4).Value = True Then r = r & "E~"
                        If dgv_Details.Rows(i).Cells(5).Value = True Then r = r & "D~"
                        If dgv_Details.Rows(i).Cells(6).Value = True Then r = r & "V~"
                        If dgv_Details.Rows(i).Cells(7).Value = True Then r = r & "I~"

                    End If

                    If Trim(r) = "~" Then r = ""

                    Debug.Print(Trim(dgv_Details.Rows(i).Cells(8).Value))

                    cmd.CommandText = "Insert into User_Access_Rights(User_IdNo, Entry_Code, Access_Type) Values (" & Str(Val(lbl_UserID.Text)) & ", '" & Trim(dgv_Details.Rows(i).Cells(8).Value) & "', '" & Trim(r) & "')"
                    cmd.ExecuteNonQuery()

                End If

            Next

            tr.Commit()

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub User_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Common_Procedures.UserRight_Check_1(Me.Name, Common_Procedures.OperationType.Open) = False Then
            MsgBox("This User Is Restircetd From Opening The Form " & Me.Text)
            Me.Close()
        End If

        lbl_UnAcPwd.Visible = False
        txt_UnAcPwd.Visible = False
        If Trim(Common_Procedures.User.Type) = "UNACCOUNT" Then
            lbl_UnAcPwd.Visible = True
            txt_UnAcPwd.Visible = True
        End If

        con.Open()

        dgv_Details.RowTemplate.Height = 27

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 100
        grp_Open.Top = (Me.Height - grp_Open.Height) - 100

        new_record()

    End Sub

    Private Sub User_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub User_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        cmd.CommandText = "select user_idno from user_head where user_name = '" & Trim(cbo_Open.Text) & "'"
        cmd.Connection = con

        movid = 0

        dr = cmd.ExecuteReader()
        If dr.HasRows Then
            If dr.Read Then
                If IsDBNull(dr(0).ToString) = False Then
                    movid = Val((dr(0).ToString))
                End If
            End If
        End If
        dr.Close()
        cmd.Dispose()

        If movid <> 0 Then move_record(movid)

        pnl_Back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Try
            With cbo_Open
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Open

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

                        Call btn_Find_Click(sender, e)

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
                            Condt = " Where User_Name like '" & Trim(FindStr) & "%' or User_Name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select User_Name from User_Head " & Condt & " order by User_Name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "User_Name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        da.Dispose()


        'If Asc(e.KeyChar) = 13 Then
        '    Call btn_Find_Click(sender, e)
        'End If

    End Sub


    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        'If e.KeyCode = Keys.Enter Then
        '    e.SuppressKeyPress = True
        '    e.Handled = True
        '    SendKeys.Send("{Tab}")
        'End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub txt_AcPwd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AcPwd.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_UnAcPwd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_UnAcPwd.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        Dim K As Integer

        If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
            K = Asc(e.KeyChar)
            K = K - 32
            e.KeyChar = Chr(K)
        End If

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Name_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Name.TextChanged

    End Sub

    Private Sub txt_AcPwd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AcPwd.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_AcPwd_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AcPwd.TextChanged

    End Sub

    Private Sub txt_UnAcPwd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_UnAcPwd.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chk_All_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_All.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_All.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(2).Value = STS

            Next

        End With

    End Sub

    Private Sub chk_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Add.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Add.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(3).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Edit.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Edit.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(4).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Delete_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_Delete.CheckedChanged

    End Sub

    Private Sub chk_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Delete.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Delete.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(5).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_View_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_View.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_View.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(6).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Insert.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Insert.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(7).Value = STS

            Next

        End With
    End Sub

    Private Sub pnl_Back_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Back.Paint

    End Sub
End Class
