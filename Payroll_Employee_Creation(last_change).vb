Imports System.IO
Public Class Payroll_Employee_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private EmpJoin_date As DateTime
    Private dgv_ActCtrlName As String = ""

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private prn_DetIndx As Integer
    Private WithEvents dgtxt_SchemesalaryDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ReleaveDetails As New DataGridViewTextBoxEditingControl

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private NoCalc_Status As Boolean

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        'grp_Find.Visible = False
        'grp_Filter.Visible = False

        'Me.Height = 296

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        EmpJoin_date = New DateTime(Val(Microsoft.VisualBasic.Right(Common_Procedures.FnRange, 4)) - 1, 4, 1)

        msk_JoinDate.Text = EmpJoin_date.ToString
        txt_Name.Text = ""
        txt_CardNo.Text = ""
        cbo_Category.Text = ""
        cbo_Company.Text = ""
        cbo_Area.Text = ""

        cbo_ShiftDay.Text = "SHIFT"
        cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, 1)
        cbo_WeekOff.Text = ""
        txt_Designation.Text = ""
        txt_Dispensary.Text = ""
        cbo_Department.Text = ""
        Cbo_BankName.Text = ""
        msk_EsiJoinDate.Text = ""
        msk_EsiLeaveDate.Text = ""
        msk_pfJoinDate.Text = ""
        msk_pfLeaveDate.Text = ""
        txt_WekkOffCredit.Text = "0"
        chk_Esists.Checked = False
        txt_EsiNo.Text = ""
        chk_EsiSalary.Checked = False
        txt_PfNo.Text = ""
        chk_PfSalary.Checked = False
        chk_Pfsts.Checked = False
        txt_opAmt.Text = "0"
        txt_BankAcNo.Text = ""
        txt_Wages.Text = ""
        Cbo_ESIPF_Group.Text = ""
        cbo_MotherTongue.Text = "TAMIL"

        msk_DateOfBirth.Text = ""
        txt_Age.Text = "0"
        cbo_Sex.Text = ""
        txt_Height.Text = "0"
        txt_Weight.Text = "0"
        txt_FatherHusband.Text = ""
        cbo_MarriedStatus.Text = ""
        txt_NoOfChildren.Text = "0"
        txt_BloodGroup.Text = ""
        txt_Qualification.Text = ""
        txt_Community.Text = ""

        txt_Address1.Text = ""
        txt_Address2.Text = ""
        txt_Address3.Text = ""
        txt_Village.Text = ""
        txt_Taulk.Text = ""
        txt_District.Text = ""
        txt_PhoneNo.Text = ""
        txt_MobileNo.Text = ""
        txt_BankCode.Text = ""

        txt_RelationName1.Text = ""
        txt_RelationName2.Text = ""
        txt_RelationName3.Text = ""
        txt_RelationName4.Text = ""
        txt_RelationShip1.Text = ""
        txt_Relationship2.Text = ""
        txt_RelationShip3.Text = ""
        txt_Relationship4.Text = ""

        txt_Document1.Text = ""
        txt_Document2.Text = ""
        txt_Document3.Text = ""
        txt_Document4.Text = ""
        txt_Certificate1.Text = ""
        txt_Certificate2.Text = ""
        txt_Certificate3.Text = ""
        txt_Certificate4.Text = ""

        chk_ReleaveDate.Checked = False
        msk_ReleaveDate.Text = ""
        msk_ReleaveDate.Enabled = False
        txt_Reason.Text = ""
        txt_Reason.Enabled = False

        dgv_SchemeSalarydetails.Rows.Clear()
        dgv_Releavedetails.Rows.Clear()
        'dgv_SchemeSalarydetails.Rows.Add()
        dgv_ActCtrlName = ""

        PictureBox1.BackgroundImage = Nothing
        PictureBox2.BackgroundImage = Nothing
        PictureBox3.BackgroundImage = Nothing
        PictureBox4.BackgroundImage = Nothing
        PictureBox5.BackgroundImage = Nothing
        PictureBox6.BackgroundImage = Nothing
        PictureBox7.BackgroundImage = Nothing
        PictureBox8.BackgroundImage = Nothing
        PictureBox9.BackgroundImage = Nothing

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Black

            End If
        End If

    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        dgv_SchemeSalarydetails.CurrentCell.Selected = False
        dgv_Releavedetails.CurrentCell.Selected = False

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim SNo As Integer
        Dim i As Integer, n As Integer

        If Val(idno) = 0 Then Exit Sub
        NoCalc_Status = True
        clear()

        Try
            da = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Head a where a.Employee_IdNo = " & Str(Val(idno)), con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                lbl_IdNo.Text = dt.Rows(0).Item("Employee_IdNo").ToString
                txt_Name.Text = dt.Rows(0).Item("Employee_MainName").ToString
                txt_CardNo.Text = dt.Rows(0).Item("Card_No").ToString
                cbo_Category.Text = Common_Procedures.Category_IdNoToName(con, Val(dt.Rows(0).Item("Category_IdNo").ToString))
                cbo_Area.Text = Common_Procedures.Area_IdNoToName(con, Val(dt.Rows(0).Item("Area_IdNo").ToString))

                cbo_Company.Text = Common_Procedures.Company_IdNoToShortName(con, Val(dt.Rows(0).Item("Company_IdNo").ToString))
                If IsDBNull(dt.Rows(0).Item("Employee_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(dt.Rows(0).Item("Employee_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                PictureBox1.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If

                msk_JoinDate.Text = dt.Rows(0).Item("Join_Date").ToString
                cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, Val(dt.Rows(0).Item("Salary_Payment_Type_IdNo").ToString))
                cbo_ShiftDay.Text = dt.Rows(0).Item("Shift_Day_Month").ToString
                cbo_WeekOff.Text = dt.Rows(0).Item("Week_Off").ToString
                txt_Designation.Text = dt.Rows(0).Item("Designation").ToString
                cbo_Department.Text = Common_Procedures.Department_IdNoToName(con, Val(dt.Rows(0).Item("Department_IdNo").ToString))
                Cbo_BankName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0).Item("Bank_IdNo").ToString))
                txt_BankCode.Text = dt.Rows(0).Item("Bank_Code").ToString
                txt_Dispensary.Text = dt.Rows(0).Item("Dispensary").ToString
                msk_EsiJoinDate.Text = dt.Rows(0).Item("Esi_Join_Date").ToString
                msk_EsiLeaveDate.Text = dt.Rows(0).Item("Esi_Leave_Date").ToString
                msk_pfJoinDate.Text = dt.Rows(0).Item("Pf_Join_Date").ToString
                msk_pfLeaveDate.Text = dt.Rows(0).Item("Pf_Leave_Date").ToString
                If Val(dt.Rows(0).Item("Esi_Status").ToString) = 1 Then
                    chk_Esists.Checked = True
                End If
                If Val(dt.Rows(0).Item("Esi_Salary").ToString) = 1 Then
                    chk_EsiSalary.Checked = True
                End If

                txt_EsiNo.Text = dt.Rows(0).Item("Esi_No").ToString
                If Val(dt.Rows(0).Item("Pf_Status").ToString) = 1 Then
                    chk_Pfsts.Checked = True
                End If
                If Val(dt.Rows(0).Item("Pf_Salary").ToString) = 1 Then
                    chk_PfSalary.Checked = True
                End If

                Cbo_ESIPF_Group.Text = Common_Procedures.Esi_Pf_Group_IdNoToName(con, Val(dt.Rows(0).Item("ESI_PF_Group_IdNo").ToString))

                txt_PfNo.Text = Val(dt.Rows(0).Item("Pf_No").ToString)
                txt_WekkOffCredit.Text = Val(dt.Rows(0).Item("Wekk_Credit").ToString)
                txt_opAmt.Text = Val(dt.Rows(0).Item("Op_Amount").ToString)
                txt_BankAcNo.Text = Trim(dt.Rows(0).Item("Bank_Ac_No").ToString)

                txt_Wages.Text = Val(dt.Rows(0).Item("Wages_Amount").ToString)

                msk_DateOfBirth.Text = dt.Rows(0).Item("Date_Birth").ToString
                txt_Age.Text = dt.Rows(0).Item("Age").ToString
                cbo_Sex.Text = dt.Rows(0).Item("Sex").ToString
                txt_Height.Text = dt.Rows(0).Item("Height").ToString
                txt_Weight.Text = dt.Rows(0).Item("Weight").ToString
                txt_FatherHusband.Text = dt.Rows(0).Item("Father_Husband").ToString
                cbo_MarriedStatus.Text = dt.Rows(0).Item("Marital_Status").ToString
                txt_NoOfChildren.Text = dt.Rows(0).Item("No_Children").ToString
                txt_BloodGroup.Text = dt.Rows(0).Item("Blood_Group").ToString
                txt_Qualification.Text = dt.Rows(0).Item("Qualification").ToString
                txt_Community.Text = dt.Rows(0).Item("Community").ToString
                cbo_MotherTongue.Text = dt.Rows(0).Item("Mother_Tongue").ToString

                txt_Address1.Text = dt.Rows(0).Item("Address1").ToString
                txt_Address2.Text = dt.Rows(0).Item("Address2").ToString
                txt_Address3.Text = dt.Rows(0).Item("Address3").ToString
                txt_Village.Text = dt.Rows(0).Item("Village").ToString
                txt_Taulk.Text = dt.Rows(0).Item("Taulk").ToString
                txt_District.Text = dt.Rows(0).Item("District").ToString
                txt_PhoneNo.Text = dt.Rows(0).Item("Phone_No").ToString
                txt_MobileNo.Text = dt.Rows(0).Item("Mobile_No").ToString

                txt_RelationName1.Text = dt.Rows(0).Item("Relation_Name1").ToString
                txt_RelationName2.Text = dt.Rows(0).Item("Relation_Name2").ToString
                txt_RelationName3.Text = dt.Rows(0).Item("Relation_Name3").ToString
                txt_RelationName4.Text = dt.Rows(0).Item("Relation_Name4").ToString
                txt_RelationShip1.Text = dt.Rows(0).Item("Relation_Ship1").ToString
                txt_Relationship2.Text = dt.Rows(0).Item("Relation_ship2").ToString
                txt_RelationShip3.Text = dt.Rows(0).Item("Relation_Ship3").ToString
                txt_Relationship4.Text = dt.Rows(0).Item("Relation_Ship4").ToString

                If IsDBNull(dt.Rows(0).Item("RelationName_Image1")) = False Then
                    Dim imageData1 As Byte() = DirectCast(dt.Rows(0).Item("RelationName_Image1"), Byte())
                    If Not imageData1 Is Nothing Then
                        Using ms1 As New MemoryStream(imageData1, 0, imageData1.Length)
                            ms1.Write(imageData1, 0, imageData1.Length)
                            If imageData1.Length > 0 Then

                                PictureBox2.BackgroundImage = Image.FromStream(ms1)

                            End If
                        End Using
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("RelationName_Image2")) = False Then
                    Dim imageData2 As Byte() = DirectCast(dt.Rows(0).Item("RelationName_Image2"), Byte())
                    If Not imageData2 Is Nothing Then
                        Using ms2 As New MemoryStream(imageData2, 0, imageData2.Length)
                            ms2.Write(imageData2, 0, imageData2.Length)
                            If imageData2.Length > 0 Then

                                PictureBox3.BackgroundImage = Image.FromStream(ms2)



                            End If
                        End Using
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("RelationName_Image3")) = False Then
                    Dim imageData3 As Byte() = DirectCast(dt.Rows(0).Item("RelationName_Image3"), Byte())
                    If Not imageData3 Is Nothing Then
                        Using ms3 As New MemoryStream(imageData3, 0, imageData3.Length)
                            ms3.Write(imageData3, 0, imageData3.Length)
                            If imageData3.Length > 0 Then

                                PictureBox4.BackgroundImage = Image.FromStream(ms3)



                            End If
                        End Using
                    End If
                End If



                If IsDBNull(dt.Rows(0).Item("RelationName_Image4")) = False Then
                    Dim imageData4 As Byte() = DirectCast(dt.Rows(0).Item("RelationName_Image4"), Byte())
                    If Not imageData4 Is Nothing Then
                        Using ms4 As New MemoryStream(imageData4, 0, imageData4.Length)
                            ms4.Write(imageData4, 0, imageData4.Length)
                            If imageData4.Length > 0 Then

                                PictureBox5.BackgroundImage = Image.FromStream(ms4)



                            End If
                        End Using
                    End If
                End If



                txt_Document1.Text = dt.Rows(0).Item("Document_Name1").ToString
                txt_Document2.Text = dt.Rows(0).Item("Document_Name2").ToString
                txt_Document3.Text = dt.Rows(0).Item("Document_Name3").ToString
                txt_Document4.Text = dt.Rows(0).Item("Document_Name4").ToString
                txt_Certificate1.Text = dt.Rows(0).Item("Certificate1").ToString
                txt_Certificate2.Text = dt.Rows(0).Item("Certificate2").ToString
                txt_Certificate3.Text = dt.Rows(0).Item("Certificate3").ToString
                txt_Certificate4.Text = dt.Rows(0).Item("Certificate4").ToString

                If IsDBNull(dt.Rows(0).Item("Document_Image1")) = False Then
                    Dim imageData1 As Byte() = DirectCast(dt.Rows(0).Item("Document_Image1"), Byte())
                    If Not imageData1 Is Nothing Then
                        Using ms6 As New MemoryStream(imageData1, 0, imageData1.Length)
                            ms6.Write(imageData1, 0, imageData1.Length)
                            If imageData1.Length > 0 Then

                                PictureBox6.BackgroundImage = Image.FromStream(ms6)



                            End If
                        End Using
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("Document_Image2")) = False Then
                    Dim imageData2 As Byte() = DirectCast(dt.Rows(0).Item("Document_Image2"), Byte())
                    If Not imageData2 Is Nothing Then
                        Using ms7 As New MemoryStream(imageData2, 0, imageData2.Length)
                            ms7.Write(imageData2, 0, imageData2.Length)
                            If imageData2.Length > 0 Then

                                PictureBox7.BackgroundImage = Image.FromStream(ms7)



                            End If
                        End Using
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("Document_Image3")) = False Then
                    Dim imageData3 As Byte() = DirectCast(dt.Rows(0).Item("Document_Image3"), Byte())
                    If Not imageData3 Is Nothing Then
                        Using ms8 As New MemoryStream(imageData3, 0, imageData3.Length)
                            ms8.Write(imageData3, 0, imageData3.Length)
                            If imageData3.Length > 0 Then

                                PictureBox8.BackgroundImage = Image.FromStream(ms8)



                            End If
                        End Using
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("Document_Image4")) = False Then
                    Dim imageData4 As Byte() = DirectCast(dt.Rows(0).Item("Document_Image4"), Byte())
                    If Not imageData4 Is Nothing Then
                        Using ms9 As New MemoryStream(imageData4, 0, imageData4.Length)
                            ms9.Write(imageData4, 0, imageData4.Length)
                            If imageData4.Length > 0 Then

                                PictureBox9.BackgroundImage = Image.FromStream(ms9)



                            End If
                        End Using
                    End If
                End If

                If Val(dt.Rows(0).Item("Date_Status").ToString) = 1 Then
                    chk_ReleaveDate.Checked = True
                End If
                msk_ReleaveDate.Text = dt.Rows(0).Item("Releave_Date").ToString
                txt_Reason.Text = dt.Rows(0).Item("Reason").ToString


                da2 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Salary_Details a where a.Employee_IdNo = " & Str(Val(idno)) & " Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_SchemeSalarydetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_SchemeSalarydetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_SchemeSalarydetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_SchemeSalarydetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("From_Date").ToString
                        dgv_SchemeSalarydetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("To_Date").ToString
                        dgv_SchemeSalarydetails.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("For_Salary").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Esi_Pf").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("O_T").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("D_A").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("H_R_A").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Conveyance_Esi_Pf").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Conveyance_Salary").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Washing").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Entertainment").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Maintenance").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("MessDeduction").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Provision").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Other_Addition1").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(16).Value = Format(Val(dt2.Rows(i).Item("Other_Addition2").ToString), "#########0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(17).Value = Val(dt2.Rows(i).Item("CL").ToString)
                        dgv_SchemeSalarydetails.Rows(n).Cells(18).Value = Val(dt2.Rows(i).Item("SL").ToString)
                        dgv_SchemeSalarydetails.Rows(n).Cells(19).Value = Format(Val(dt2.Rows(i).Item("Week_Off_Allowance").ToString), "#######0.00")
                        dgv_SchemeSalarydetails.Rows(n).Cells(20).Value = Format(Val(dt2.Rows(i).Item("Other_Deduction1").ToString), "#######0.00")

                    Next i
                    For i = 0 To dgv_SchemeSalarydetails.RowCount - 1
                        dgv_SchemeSalarydetails.Rows(i).Cells(0).Value = Val(i) + 1
                    Next
                End If

                dt2.Dispose()
                da2.Dispose()



                da2 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Releave_Details a where a.Employee_IdNo = " & Str(Val(idno)) & " Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Releavedetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Releavedetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_Releavedetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Releavedetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Join_Date").ToString
                        dgv_Releavedetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Releave_Date").ToString
                        dgv_Releavedetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Reason").ToString

                    Next i
                    For i = 0 To dgv_Releavedetails.RowCount - 1
                        dgv_Releavedetails.Rows(i).Cells(0).Value = Val(i) + 1
                    Next
                End If

                dt2.Dispose()
                da2.Dispose()


            Else
                new_record()

            End If

            dgv_ActCtrlName = ""

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt.Dispose()
            da.Dispose()

            dt2.Dispose()
            da2.Dispose()

            Grid_Cell_DeSelect()

            Get_Columns_Head_Name()

            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub Employee_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Category.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CATEGORY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Category.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PaymentType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SALARY PAYMENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_PaymentType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Department.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DEPARTMENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Department.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_ESIPF_Group.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ESI PF GROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            Cbo_ESIPF_Group.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_BankName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BANKNAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            Cbo_BankName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        Common_Procedures.Master_Return.Return_Value = ""
        Common_Procedures.Master_Return.Master_Type = ""

    End Sub

    Private Sub Employee_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        con.Open()

        cbo_ShiftDay.Items.Clear()
        cbo_ShiftDay.Items.Add(" ")
        cbo_ShiftDay.Items.Add("SHIFT")
        cbo_ShiftDay.Items.Add("MONTH")

        cbo_MarriedStatus.Items.Clear()
        cbo_MarriedStatus.Items.Add(" ")
        cbo_MarriedStatus.Items.Add("MARRIED")
        cbo_MarriedStatus.Items.Add("UNMARRIED")

        cbo_Sex.Items.Clear()
        cbo_Sex.Items.Add(" ")
        cbo_Sex.Items.Add("MALE")
        cbo_Sex.Items.Add("FEMALE")

        cbo_WeekOff.Items.Clear()
        cbo_WeekOff.Items.Add(" ")
        cbo_WeekOff.Items.Add("SUNDAY")
        cbo_WeekOff.Items.Add("MONDAY")
        cbo_WeekOff.Items.Add("TUESDAY")
        cbo_WeekOff.Items.Add("WEDNESDAY")
        cbo_WeekOff.Items.Add("THURSDAY")
        cbo_WeekOff.Items.Add("FRIDAY")
        cbo_WeekOff.Items.Add("SATURDAY")


        txt_CardNo.Enabled = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
            txt_CardNo.Enabled = False
        End If

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
            btn_SaveAll.Visible = True
        End If

        grp_Open.Visible = False

        grp_Open.Left = (Me.Width - grp_Open.Width) \ 2
        grp_Open.Top = (Me.Height - grp_Open.Height) \ 2
        grp_Open.BringToFront()


        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CardNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Category.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Company.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Area.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_JoinDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ShiftDay.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeekOff.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Department.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_BankName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Designation.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Dispensary.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Esists.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_EsiSalary.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Pfsts.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_PfSalary.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EsiNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PfNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_EsiJoinDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_EsiLeaveDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_pfJoinDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_pfLeaveDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WekkOffCredit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_opAmt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BankAcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Wages.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_ESIPF_Group.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BankCode.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_DateOfBirth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sex.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Age.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Height.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FatherHusband.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MarriedStatus.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfChildren.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BloodGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Qualification.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Community.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Village.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Taulk.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_District.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PhoneNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MobileNo.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_RelationName1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RelationName2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RelationName3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RelationName4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RelationShip1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Relationship2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RelationShip3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Relationship4.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Document1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Document2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Document3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Document4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Certificate1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Certificate2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Certificate3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Certificate4.GotFocus, AddressOf ControlGotFocus

        AddHandler chk_ReleaveDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_ReleaveDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Reason.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_MotherTongue.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CardNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Category.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Company.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Area.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_JoinDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ShiftDay.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WeekOff.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Designation.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Dispensary.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Department.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_BankName.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Esists.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_EsiSalary.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EsiNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PfNo.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_PfSalary.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Pfsts.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_EsiJoinDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_EsiLeaveDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_pfJoinDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_pfLeaveDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WekkOffCredit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_opAmt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BankAcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Wages.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_ESIPF_Group.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_DateOfBirth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Age.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sex.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Height.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FatherHusband.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MarriedStatus.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfChildren.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BloodGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Qualification.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Community.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Village.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Taulk.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_District.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PhoneNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MobileNo.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_RelationName1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RelationName2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RelationName3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RelationName4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RelationShip1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Relationship2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RelationShip3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Relationship4.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Document1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Document2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Document3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Document4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Certificate1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Certificate2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Certificate3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Certificate4.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_ReleaveDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Reason.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_ReleaveDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BankCode.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_MotherTongue.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CardNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Designation.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Dispensary.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_EsiSalary.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler chk_Esists.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_PfSalary.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler chk_Pfsts.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_EsiJoinDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_EsiLeaveDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EsiNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PfNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_pfJoinDate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler msk_pfLeaveDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WekkOffCredit.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_OpBalance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BankAcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_opAmt.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Community.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Age.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Height.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FatherHusband.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfChildren.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BloodGroup.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Qualification.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Village.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Taulk.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_District.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PhoneNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BankCode.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_RelationName2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RelationName3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RelationName4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RelationShip1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Relationship2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RelationShip3.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Document2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Document3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Document4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Certificate1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Certificate2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Certificate3.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_ReleaveDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CardNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_JoinDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Designation.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Dispensary.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_EsiJoinDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_EsiLeaveDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_EsiSalary.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler chk_Esists.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_PfSalary.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler chk_Pfsts.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EsiNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PfNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_pfJoinDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler msk_pfLeaveDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WekkOffCredit.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_OpBalance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BankAcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_opAmt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Community.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler msk_DateOfBirth.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Age.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Height.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FatherHusband.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NoOfChildren.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BloodGroup.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Qualification.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Village.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Taulk.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_District.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PhoneNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_RelationName1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RelationName2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RelationName3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RelationName4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RelationShip1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Relationship2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RelationShip3.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Document1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Document2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Document3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Document4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Certificate1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Certificate2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Certificate3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BankCode.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_ReleaveDate.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_ESIforSalary.Visible = False
        lbl_PFforSalary.Visible = False
        chk_EsiSalary.Visible = False
        chk_PfSalary.Visible = False
        lbl_ESIforAudit.Text = "ESI"
        lbl_PFforAudit.Text = "PF"
        If Trim(Common_Procedures.User.Type) = "UNACCOUNT" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then
            lbl_ESIforSalary.Visible = True
            lbl_PFforSalary.Visible = True
            chk_EsiSalary.Visible = True
            chk_PfSalary.Visible = True
            lbl_ESIforAudit.Text = "ESI(Audit.)"
            lbl_PFforAudit.Text = "PF(Audit.)"
        End If

        Get_Columns_Head_Name()

        new_record()

        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Private Sub Employee_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
                'ElseIf grp_Filter.Visible Then
                '    btn_CloseFilter_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Employee_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable


        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_creation, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) < 101 Then
            MessageBox.Show("Cannot delete this default Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select count(*) from PayRoll_Employee_Attendance_Details Where Employee_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Employee in Attendance", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If


        da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Employee in Voucher", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        da = New SqlClient.SqlDataAdapter("select count(*) from PayRoll_Salary_Details Where Employee_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Employee in Salary", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        da = New SqlClient.SqlDataAdapter("select count(*) from PayRoll_Employee_Deduction_Head Where Employee_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Employee in Deduction", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        da = New SqlClient.SqlDataAdapter("select count(*) from PayRoll_Employee_Payment_Head Where Employee_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Employee in Payment", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        Try

            cmd.Connection = con

            cmd.CommandText = "delete from PayRoll_Employee_Head where Employee_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from PayRoll_Employee_Salary_Details where Employee_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from PayRoll_Employee_Releave_Details where Employee_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ledger_head where ledger_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        'Dim da As New SqlClient.SqlDataAdapter("select Employee_IdNo, Bag_Type_Name,Weight_Bag from PayRoll_Employee_Head where Employee_IdNo <> 0 order by Employee_IdNo", con)
        'Dim dt As New DataTable

        'da.Fill(dt)

        'With dgv_Filter

        '    .Columns.Clear()
        '    .DataSource = dt

        '    .RowHeadersVisible = False

        '    .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        '    .Columns(0).HeaderText = "IDNO"
        '    .Columns(1).HeaderText = "BAGTYPE NAME"
        '    .Columns(2).HeaderText = "WEIGHT BAG"

        '    .Columns(0).FillWeight = 40
        '    .Columns(1).FillWeight = 160
        '    .Columns(2).FillWeight = 80

        'End With

        'new_record()

        'grp_Filter.Visible = True
        'grp_Filter.Left = grp_Find.Left
        'grp_Filter.Top = grp_Find.Top

        'pnl_Back.Enabled = False

        'If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        'Me.Height = 595 ' 400

        'dt.Dispose()
        'da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(ledger_idno) from ledger_head Where ledger_idno <> 0 and ledger_type = 'EMPLOYEE'", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select max(ledger_idno) from ledger_head Where ledger_idno <> 0 and ledger_type = 'EMPLOYEE'", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select min(ledger_idno) from ledger_head Where ledger_idno <> 0 and ledger_idno > " & Str(Val(lbl_IdNo.Text)) & "  and ledger_type = 'EMPLOYEE'", con)
            da.Fill(dt)


            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(ledger_idno ) from ledger_head where ledger_idno <> 0 and ledger_idno < " & Str((lbl_IdNo.Text)) & " and ledger_type = 'EMPLOYEE'", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        clear()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "")

        If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101
        'cbo_MotherTongue.Text = "TAMIL"

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_type = 'EMPLOYEE') order by Ledger_DisplayName", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String
        Dim area_id As Integer = 0
        Dim WrkTy_id As Integer = 0
        Dim SlryTy_id As Integer = 0
        Dim Dep_id As Integer = 0, Bnk_id As Integer = 0, EsPf_id As Integer = 0
        Dim Com_id As Integer = 0
        Dim SNo As Integer
        Dim vRleSTS As Integer = 0
        Dim vEsiSTS As Integer = 0, vEsiSalry As Integer = 0
        Dim vPfSTS As Integer = 0, vPfSalry As Integer = 0
        Dim LedArName As String = ""
        Dim AcGrp_ID As Integer = 0
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim LedPhNo As String = ""
        Dim esijndt As String = ""
        Dim esilvdt As String = ""
        Dim pfjndt As String = ""
        Dim pflvdt As String = ""
        Dim reldt As String = ""
        Dim DOB As String = ""
        Dim Ldgr_DispName As String = ""

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_creation, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Employee Name ", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub
        End If
        If Trim(cbo_Category.Text) = "" Then
            MessageBox.Show("Invalid Category", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Category.Enabled And cbo_Category.Visible Then cbo_Category.Focus()
            Exit Sub
        End If

        If Trim(msk_JoinDate.Text) = "" Then
            MessageBox.Show("Invalid Join Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_JoinDate.Enabled And msk_JoinDate.Visible Then msk_JoinDate.Focus()
            Exit Sub
        End If
        If Not IsDate(msk_JoinDate.Text) Then
            MessageBox.Show("Invalid Join Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_JoinDate.Enabled And msk_JoinDate.Visible Then msk_JoinDate.Focus()
            Exit Sub
        End If


        If Trim(cbo_PaymentType.Text) = "" Then
            MessageBox.Show("Invalid Payment Type", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PaymentType.Enabled And cbo_PaymentType.Visible Then cbo_PaymentType.Focus()
            Exit Sub
        End If

        If Trim(cbo_ShiftDay.Text) = "" Then
            MessageBox.Show("Invalid Salary Type", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ShiftDay.Enabled And cbo_ShiftDay.Visible Then cbo_ShiftDay.Focus()
            Exit Sub
        End If

        esijndt = ""
        If chk_Esists.Checked = True Or chk_EsiSalary.Checked = True Then
            If Not Trim(msk_EsiJoinDate.Text) = "-  -" Then
                'If Not IsDate(msk_EsiJoinDate.Text) Then
                '    MessageBox.Show("Invalid ESI Join Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    If msk_EsiJoinDate.Enabled And msk_EsiJoinDate.Visible Then msk_EsiJoinDate.Focus()
                '    Exit Sub
                'End If
                esijndt = Trim(msk_EsiJoinDate.Text)
            Else
                'MessageBox.Show("Invalid ESI Join Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'If msk_EsiJoinDate.Enabled And msk_EsiJoinDate.Visible Then msk_EsiJoinDate.Focus()
                'Exit Sub
            End If
        End If


        esilvdt = ""
        If Not Trim(msk_EsiLeaveDate.Text) = "-  -" Then
            If Not IsDate(msk_EsiLeaveDate.Text) Then
                'MessageBox.Show("Invalid ESI Leave Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'If msk_EsiLeaveDate.Enabled And msk_EsiLeaveDate.Visible Then msk_EsiLeaveDate.Focus()
                'Exit Sub
            End If
            esilvdt = Trim(msk_EsiLeaveDate.Text)
        End If


        pfjndt = ""
        If chk_Pfsts.Checked = True Or chk_PfSalary.Checked = True Then

            If Not Trim(msk_pfJoinDate.Text) = "-  -" Then

                'If Not IsDate(msk_pfJoinDate.Text) Then
                '    MessageBox.Show("Invalid PF Join Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '    If msk_pfJoinDate.Enabled And msk_pfJoinDate.Visible Then msk_pfJoinDate.Focus()
                '    Exit Sub
                'End If
                pfjndt = Trim(msk_pfJoinDate.Text)

            Else

                'MessageBox.Show("Invalid  PF Join Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'If msk_pfJoinDate.Enabled And msk_pfJoinDate.Visible Then msk_pfJoinDate.Focus()
                'Exit Sub

            End If

        End If


        pflvdt = ""
        If Not Trim(msk_pfLeaveDate.Text) = "-  -" Then
            'If Not IsDate(msk_pfLeaveDate.Text) Then
            '    MessageBox.Show("Invalid PF leave Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            '    If msk_pfLeaveDate.Enabled And msk_pfLeaveDate.Visible Then msk_pfLeaveDate.Focus()
            '    Exit Sub
            'End If
            pflvdt = Trim(msk_pfLeaveDate.Text)
        End If

        reldt = ""
        If chk_ReleaveDate.Checked = True Then
            If Trim(msk_ReleaveDate.Text) = "-  -" Then
                MessageBox.Show("Invalid Releve Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_ReleaveDate.Enabled And msk_ReleaveDate.Visible Then msk_ReleaveDate.Focus()
                Exit Sub
            End If

            If Not IsDate(msk_ReleaveDate.Text) Then
                MessageBox.Show("Invalid Releve Date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_ReleaveDate.Enabled And msk_ReleaveDate.Visible Then msk_ReleaveDate.Focus()
                Exit Sub
            End If
            reldt = Trim(msk_ReleaveDate.Text)
        End If


        DOB = ""
        If Not Trim(msk_DateOfBirth.Text) = "-  -" Then
            If Not IsDate(msk_DateOfBirth.Text) Then
                'MessageBox.Show("Invalid Date of Birth", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'If msk_DateOfBirth.Enabled And msk_DateOfBirth.Visible Then msk_DateOfBirth.Focus()
                'Exit Sub
            End If
            DOB = Trim(msk_DateOfBirth.Text)
        End If

        AcGrp_ID = 14
        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, AcGrp_ID)

        LedName = Trim(txt_Name.Text)
        If Val(area_id) <> 0 Then
            LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
        End If

        ' Sur = Common_Procedures.Remove_NonCharacters(Trim(LedName))
        area_id = Common_Procedures.Area_NameToIdNo(con, cbo_Area.Text)
        WrkTy_id = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)
        SlryTy_id = Common_Procedures.Salary_PaymentType_NameToIdNo(con, cbo_PaymentType.Text)
        Dep_id = Common_Procedures.Department_NameToIdNo(con, cbo_Department.Text)
        Bnk_id = Common_Procedures.Ledger_NameToIdNo(con, Cbo_BankName.Text)
        Com_id = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)
        EsPf_id = Common_Procedures.Esi_Pf_Group_NameToIdNo(con, Cbo_ESIPF_Group.Text)


        If txt_CardNo.Enabled = False And (New_Entry = True Or SaveAll_STS = True) Then
            TokenNo_Generation()
        End If

        Ldgr_DispName = Trim(txt_Name.Text)
        If Trim(cbo_Area.Text) <> "" Then Ldgr_DispName = Ldgr_DispName & " (" & Trim(cbo_Area.Text) & ") "
        Ldgr_DispName = Ldgr_DispName & " (ID : " & Trim(txt_CardNo.Text) & ")"

        Sur = Common_Procedures.Remove_NonCharacters(Trim(Ldgr_DispName))

        vRleSTS = 0
        If chk_ReleaveDate.Checked = True Then vRleSTS = 1

        vEsiSTS = 0
        If chk_Esists.Checked = True Then vEsiSTS = 1

        vEsiSalry = 0
        If chk_EsiSalary.Checked = True Then vEsiSalry = 1

        vPfSTS = 0
        If chk_Pfsts.Checked = True Then vPfSTS = 1

        vPfSalry = 0
        If chk_PfSalary.Checked = True Then vPfSalry = 1

        With dgv_SchemeSalarydetails
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(13).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Salary From Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Tab_Main.SelectTab(6)
                        .CurrentCell = .Rows(i).Cells(1)
                        .Focus()
                        Exit Sub
                    End If

                    If IsDate(.Rows(i).Cells(1).Value) = False Then
                        MessageBox.Show("Invalid Salary From Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Tab_Main.SelectTab(6)
                        .CurrentCell = .Rows(i).Cells(1)
                        .Focus()
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(2).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(2).Value) = False Then
                            MessageBox.Show("Invalid Salary To Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Tab_Main.SelectTab(6)
                            .CurrentCell = .Rows(i).Cells(2)
                            .Focus()
                            Exit Sub
                        End If
                    End If
                End If

            Next

        End With



        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EmpJoinDate", CDate(msk_JoinDate.Text))
            If Trim(reldt) <> "" Then
                cmd.Parameters.AddWithValue("@EmpReleaveDate", CDate(reldt))
            End If


            Dim ms As New MemoryStream()
            If IsNothing(PictureBox1.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox1.BackgroundImage)
                bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data As Byte() = ms.GetBuffer()
            Dim p As New SqlClient.SqlParameter("@photo", SqlDbType.Image)
            p.Value = data
            cmd.Parameters.Add(p)
            ms.Dispose()

            Dim ms1 As New MemoryStream()
            If IsNothing(PictureBox2.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox2.BackgroundImage)
                bitmp.Save(ms1, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data1 As Byte() = ms1.GetBuffer()
            Dim N As New SqlClient.SqlParameter("@Relation1", SqlDbType.Image)
            N.Value = data1
            cmd.Parameters.Add(N)
            ms1.Dispose()

            Dim ms2 As New MemoryStream()
            If IsNothing(PictureBox3.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox3.BackgroundImage)
                bitmp.Save(ms2, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data2 As Byte() = ms2.GetBuffer()
            Dim N2 As New SqlClient.SqlParameter("@Relation2", SqlDbType.Image)
            N2.Value = data2
            cmd.Parameters.Add(N2)
            ms2.Dispose()

            Dim ms3 As New MemoryStream()
            If IsNothing(PictureBox4.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox4.BackgroundImage)
                bitmp.Save(ms3, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data3 As Byte() = ms3.GetBuffer()
            Dim N3 As New SqlClient.SqlParameter("@Relation3", SqlDbType.Image)
            N3.Value = data3
            cmd.Parameters.Add(N3)
            ms3.Dispose()

            Dim ms4 As New MemoryStream()
            If IsNothing(PictureBox5.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox5.BackgroundImage)
                bitmp.Save(ms4, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data4 As Byte() = ms4.GetBuffer()
            Dim N4 As New SqlClient.SqlParameter("@Relation4", SqlDbType.Image)
            N4.Value = data4
            cmd.Parameters.Add(N4)

            ms4.Dispose()

            Dim ms6 As New MemoryStream()
            If IsNothing(PictureBox6.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox6.BackgroundImage)
                bitmp.Save(ms6, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data6 As Byte() = ms6.GetBuffer()
            Dim N6 As New SqlClient.SqlParameter("@Document1", SqlDbType.Image)
            N6.Value = data6
            cmd.Parameters.Add(N6)
            ms6.Dispose()

            Dim ms7 As New MemoryStream()
            If IsNothing(PictureBox7.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox7.BackgroundImage)
                bitmp.Save(ms7, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data7 As Byte() = ms7.GetBuffer()
            Dim N7 As New SqlClient.SqlParameter("@Document2", SqlDbType.Image)
            N7.Value = data7
            cmd.Parameters.Add(N7)
            ms7.Dispose()

            Dim ms8 As New MemoryStream()
            If IsNothing(PictureBox8.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox8.BackgroundImage)
                bitmp.Save(ms8, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data8 As Byte() = ms8.GetBuffer()
            Dim N8 As New SqlClient.SqlParameter("@Document3", SqlDbType.Image)
            N8.Value = data8
            cmd.Parameters.Add(N8)
            ms8.Dispose()

            Dim ms9 As New MemoryStream()
            If IsNothing(PictureBox9.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(PictureBox9.BackgroundImage)
                bitmp.Save(ms9, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data9 As Byte() = ms9.GetBuffer()
            Dim N9 As New SqlClient.SqlParameter("@Document4", SqlDbType.Image)
            N9.Value = data9
            cmd.Parameters.Add(N9)

            ms9.Dispose()


            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", trans)
                If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into ledger_head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_PhoneNo, Ledger_Type ) Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(LedName) & "', '" & Trim(Sur) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_Name.Text) & "', " & Str(Val(area_id)) & ", " & Str(Val(AcGrp_ID)) & ", '" & Trim(Parnt_CD) & "', 'BALANCE ONLY', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "' , '" & Trim(txt_PhoneNo.Text) & "', 'EMPLOYEE'  )"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into PayRoll_Employee_Head(Employee_IdNo           , Employee_Name                , sur_name           , Employee_MainName           , Card_No                       , Category_IdNo          , Area_IdNo            , Company_IdNo       , Employee_Image    , Join_Date                        , Join_DateTime                                                         , Salary_Payment_Type_IdNo,Shift_Day_Month                 ,Week_Off                        ,designation                         ,Dispensary                         ,Department_IdNo    ,Esi_Status          ,Pf_Status          ,Esi_Salary            ,Pf_Salary            ,Esi_No                     ,Pf_no                     ,Esi_Join_Date            ,Esi_Leave_Date           ,Pf_Join_Date          ,Pf_Leave_Date         ,Wekk_Credit                        ,Op_Amount                  ,Bank_Ac_No                        ,Date_Birth         ,Age                      ,Sex                         ,Height                      ,Weight                      ,Father_Husband                        ,Marital_Status                        ,No_Children                       ,Blood_Group                        ,Qualification                         ,Community                         ,Address1                         ,Address2                         ,Address3                         ,Village                         ,Taulk                         ,District                         ,Phone_No                        ,Mobile_No                        , Relation_Name1                        ,Relation_Name2                         ,Relation_Name3                         , Relation_name4                         ,Relation_Ship1                         , Relation_Ship2                        ,Relation_Ship3                         , Relation_Ship4                        ,RelationName_Image1, RelationName_Image2 ,RelationName_Image3 , RelationName_Image4 , Document_Name1                   ,Document_Name2                      ,Document_Name3                    , Document_name4                      ,Certificate1                          , Certificate2                         ,Certificate3                          , Certificate4                         ,Document_Image1, Document_Image2 ,Document_Image3 , Document_Image4 ,Date_Status           , Releave_Date        , Releave_DateTime                                             , Reason                          , Wages_Amount                    , Bank_IdNo         ,ESI_PF_Group_IdNo     ,              Bank_Code           , Mother_Tongue) " & _
                                                    " values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(Ldgr_DispName) & "', '" & Trim(Sur) & "','" & Trim(txt_Name.Text) & "','" & Trim(txt_CardNo.Text) & "',  " & Val(WrkTy_id) & " ," & Val(area_id) & "  , " & Val(Com_id) & " ,@Photo            , '" & Trim(msk_JoinDate.Text) & "',  " & IIf(IsDate(msk_JoinDate.Text) = True, "@EmpJoinDate", "Null") & ", " & Val(SlryTy_id) & " ,'" & Trim(cbo_ShiftDay.Text) & "','" & Trim(cbo_WeekOff.Text) & "','" & Trim(txt_Designation.Text) & "','" & Trim(txt_Dispensary.Text) & "'," & Val(Dep_id) & "," & Val(vEsiSTS) & "," & Val(vPfSTS) & "," & Val(vEsiSalry) & "," & Val(vPfSalry) & "," & Val(txt_EsiNo.Text) & "," & Val(txt_PfNo.Text) & ", '" & Trim(esijndt) & "' , '" & Trim(esilvdt) & "' ,'" & Trim(pfjndt) & "','" & Trim(pflvdt) & "'," & Val(txt_WekkOffCredit.Text) & "," & Val(txt_opAmt.Text) & ", '" & Trim(txt_BankAcNo.Text) & "','" & Trim(DOB) & "'," & Val(txt_Age.Text) & ",'" & Trim(cbo_Sex.Text) & "'," & Val(txt_Height.Text) & "," & Val(txt_Weight.Text) & ",'" & Trim(txt_FatherHusband.Text) & "','" & Trim(cbo_MarriedStatus.Text) & "'," & Val(txt_NoOfChildren.Text) & ",'" & Trim(txt_BloodGroup.Text) & "','" & Trim(txt_Qualification.Text) & "','" & Trim(txt_Community.Text) & "','" & Trim(txt_Address1.Text) & "','" & Trim(txt_Address2.Text) & "','" & Trim(txt_Address3.Text) & "','" & Trim(txt_Village.Text) & "','" & Trim(txt_Taulk.Text) & "','" & Trim(txt_District.Text) & "','" & Trim(txt_PhoneNo.Text) & "','" & Trim(txt_MobileNo.Text) & "','" & Trim(txt_RelationName1.Text) & "' ,'" & Trim(txt_RelationName2.Text) & "' ,'" & Trim(txt_RelationName3.Text) & "' , '" & Trim(txt_RelationName4.Text) & "' ,'" & Trim(txt_RelationShip1.Text) & "' ,'" & Trim(txt_Relationship2.Text) & "' ,'" & Trim(txt_RelationShip3.Text) & "' ,'" & Trim(txt_Relationship4.Text) & "' , @Relation1        , @Relation2          ,  @Relation3        ,   @Relation4        ,'" & Trim(txt_Document1.Text) & "' ,'" & Trim(txt_Document2.Text) & "' ,'" & Trim(txt_Document3.Text) & "' , '" & Trim(txt_Document4.Text) & "' ,'" & Trim(txt_Certificate1.Text) & "' ,'" & Trim(txt_Certificate2.Text) & "' ,'" & Trim(txt_Certificate3.Text) & "' ,'" & Trim(txt_Certificate4.Text) & "' , @Document1  , @Document2        ,  @Document3    ,   @Document4    , " & Val(vRleSTS) & " ,'" & Trim(reldt) & "',  " & IIf(IsDate(reldt) = True, "@EmpReleaveDate", "Null") & ", '" & Trim(txt_Reason.Text) & "' , " & Str(Val(txt_Wages.Text)) & "," & Val(Bnk_id) & ", " & Val(EsPf_id) & " ,'" & Trim(txt_BankCode.Text) & "' , '" & Trim(cbo_MotherTongue.Text) & "')"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update ledger_head set Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(Sur) & "', Ledger_MainName = '" & Trim(txt_Name.Text) & "', Ledger_AlaisName = '" & Trim(txt_Name.Text) & "', Area_IdNo = " & Str(Val(area_id)) & ", AccountsGroup_IdNo = " & Str(Val(AcGrp_ID)) & ", Parent_Code = '" & Trim(Parnt_CD) & "', Bill_Type = 'BALANCE ONLY' , Ledger_Address1 = '" & Trim(txt_Address1.Text) & "', Ledger_Address2 = '" & Trim(txt_Address2.Text) & "', Ledger_Address3 = '" & Trim(txt_Address3.Text) & "', Ledger_PhoneNo = '" & Trim(txt_PhoneNo.Text) & "' where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update PayRoll_Employee_Head set Employee_Name = '" & Trim(Ldgr_DispName) & "', sur_name = '" & Trim(Sur) & "',Employee_MainName = '" & Trim(txt_Name.Text) & "',Card_No = '" & Trim(txt_CardNo.Text) & "', Area_IdNo = " & Val(area_id) & "  ,Category_IdNo = " & Val(WrkTy_id) & " ,Company_IdNo = " & Val(Com_id) & " , Employee_Image = @Photo, Join_Date = '" & Trim(msk_JoinDate.Text) & "', Join_DateTime = " & IIf(IsDate(msk_JoinDate.Text) = True, "@EmpJoinDate", "Null") & ", Salary_Payment_Type_IdNo = " & Val(SlryTy_id) & " , Shift_Day_Month = '" & Trim(cbo_ShiftDay.Text) & "',Week_Off = '" & Trim(cbo_WeekOff.Text) & "', designation = '" & Trim(txt_Designation.Text) & "', Dispensary = '" & Trim(txt_Dispensary.Text) & "',Department_idno = " & Val(Dep_id) & ",Esi_Status  = " & Val(vEsiSTS) & ",Pf_Status  = " & Val(vPfSTS) & ", Esi_Salary  = " & Val(vEsiSalry) & ",Pf_Salary = " & Val(vPfSalry) & ",Esi_No = " & Val(txt_EsiNo.Text) & ",Pf_no = " & Val(txt_PfNo.Text) & ",Esi_Join_Date = '" & Trim(esijndt) & "' , Esi_Leave_Date = '" & Trim(esilvdt) & "' , Pf_Join_Date = '" & Trim(pfjndt) & "',Pf_Leave_Date = '" & Trim(pflvdt) & "' ,Wekk_Credit = " & Val(txt_WekkOffCredit.Text) & ",Op_Amount = " & Val(txt_opAmt.Text) & ",Bank_Ac_No = '" & Trim(txt_BankAcNo.Text) & "', Date_Birth = '" & Trim(DOB) & "'  , Age = " & Val(txt_Age.Text) & ",sex  = '" & Trim(cbo_Sex.Text) & "',Height= " & Val(txt_Height.Text) & ",Weight = " & Val(txt_Weight.Text) & ",Father_Husband = '" & Trim(txt_FatherHusband.Text) & "' ,Marital_Status = '" & Trim(cbo_MarriedStatus.Text) & "',No_Children = " & Val(txt_NoOfChildren.Text) & ",Blood_Group = '" & Trim(txt_BloodGroup.Text) & "',Qualification = '" & Trim(txt_Qualification.Text) & "',Community = '" & Trim(txt_Community.Text) & "',Address1 = '" & Trim(txt_Address1.Text) & "'  , Address2 = '" & Trim(txt_Address2.Text) & "',Address3 = '" & Trim(txt_Address3.Text) & "',Village= '" & Trim(txt_Village.Text) & "',Taulk = '" & Trim(txt_Taulk.Text) & "',District = '" & Trim(txt_District.Text) & "' ,Phone_No = '" & Trim(txt_PhoneNo.Text) & "',Mobile_No = '" & Trim(txt_MobileNo.Text) & "',Relation_Name1   = '" & Trim(txt_RelationName1.Text) & "',Relation_Name2  = '" & Trim(txt_RelationName2.Text) & "' , Relation_Name3  = '" & Trim(txt_RelationName3.Text) & "' , Relation_name4  = '" & Trim(txt_RelationName4.Text) & "' ,Relation_Ship1   = '" & Trim(txt_RelationShip1.Text) & "' , Relation_Ship2  = '" & Trim(txt_Relationship2.Text) & "' ,Relation_Ship3  = '" & Trim(txt_RelationShip3.Text) & "' , Relation_Ship4  = '" & Trim(txt_Relationship4.Text) & "' ,RelationName_Image1 = @Relation1 , RelationName_Image2 = @Relation2  ,RelationName_Image3 = @Relation3  , RelationName_Image4 = @Relation4 ,Document_Name1   = '" & Trim(txt_Document1.Text) & "',Document_Name2  = '" & Trim(txt_Document2.Text) & "' , Document_Name3  = '" & Trim(txt_Document3.Text) & "' , Document_name4  = '" & Trim(txt_Document4.Text) & "' ,Certificate1   = '" & Trim(txt_Certificate1.Text) & "' , Certificate2  = '" & Trim(txt_Certificate2.Text) & "' ,Certificate3  = '" & Trim(txt_Certificate3.Text) & "' , Certificate4  = '" & Trim(txt_Certificate4.Text) & "' ,Document_Image1 = @Document1 , Document_Image2 = @Document2  ,Document_Image3 = @Document3  , Document_Image4 = @Document4 ,Date_Status = " & Val(vRleSTS) & ", Releave_Date = '" & Trim(reldt) & "', Releave_DateTime =  " & IIf(IsDate(reldt) = True, "@EmpReleaveDate", "Null") & ", Reason = '" & Trim(txt_Reason.Text) & "' , Wages_Amount = " & Str(Val(txt_Wages.Text)) & ", bank_IdNo = " & Val(Bnk_id) & ", ESI_PF_Group_IdNo = " & Val(EsPf_id) & ",Bank_Code='" & Trim(txt_BankCode.Text) & "',Mother_Tongue='" & Trim(cbo_MotherTongue.Text) & "' Where Employee_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            LedArName = Trim(txt_Name.Text)
            If Val(area_id) <> 0 Then
                LedArName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
            End If

            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(AcGrp_ID)) & ", 'EMPLOYEE')"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from PayRoll_Employee_Salary_Details where Employee_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            With dgv_SchemeSalarydetails
                SNo = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        If IsDate(.Rows(i).Cells(1).Value) = True Then

                            If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(13).Value) <> 0 Then

                                SNo = SNo + 1

                                cmd.Parameters.Clear()
                                cmd.Parameters.AddWithValue("@FromDate", CDate(.Rows(i).Cells(1).Value))
                                If Trim(.Rows(i).Cells(2).Value) <> "" Then
                                    If IsDate(.Rows(i).Cells(2).Value) = True Then
                                        cmd.Parameters.AddWithValue("@ToDate", CDate(.Rows(i).Cells(2).Value))
                                    End If
                                End If

                                cmd.CommandText = "Insert into PayRoll_Employee_Salary_Details (             Employee_IdNo      ,            sl_no     ,                    From_Date           , From_DateTime,                    To_date             ,                                                To_DateTime    ,                      For_Salary          ,                      Esi_Pf              ,                      O_T                  ,                      D_A                 ,                      H_R_A               ,              Conveyance_Esi_Pf           ,                      Conveyance_Salary   ,                      Washing              ,                      Entertainment        ,                      Maintenance          ,                      MessDeduction          ,Provision                                   ,Other_Addition1                           , Other_Addition2                           ,                      CL                    ,                      SL                   ,Week_Off_Allowance                         ,Other_Deduction1 ) " & _
                                                        "       Values                 ( " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SNo)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "',   @FromDate  , '" & Trim(.Rows(i).Cells(2).Value) & "', " & IIf(IsDate(.Rows(i).Cells(2).Value) = True, "@ToDate", "Null") & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & "  , " & Str(Val(.Rows(i).Cells(14).Value)) & " ," & Str(Val(.Rows(i).Cells(15).Value)) & ", " & Str(Val(.Rows(i).Cells(16).Value)) & " ," & Str(Val(.Rows(i).Cells(17).Value)) & " ," & Str(Val(.Rows(i).Cells(18).Value)) & " ," & Str(Val(.Rows(i).Cells(19).Value)) & " ," & Str(Val(.Rows(i).Cells(20).Value)) & ") "
                                cmd.ExecuteNonQuery()

                            End If

                        End If

                    End If

                Next

            End With

            cmd.CommandText = "delete from PayRoll_Employee_Releave_Details where Employee_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_Releavedetails
                SNo = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        If IsDate(.Rows(i).Cells(1).Value) = True Then

                            If Trim(.Rows(i).Cells(1).Value) <> "" Then

                                SNo = SNo + 1

                                cmd.Parameters.Clear()
                                cmd.Parameters.AddWithValue("@JoinDate", CDate(.Rows(i).Cells(1).Value))
                                If Trim(.Rows(i).Cells(2).Value) <> "" Then
                                    If IsDate(.Rows(i).Cells(2).Value) = True Then
                                        cmd.Parameters.AddWithValue("@ReleaveDate", CDate(.Rows(i).Cells(2).Value))
                                    End If
                                End If

                                cmd.CommandText = "Insert into PayRoll_Employee_Releave_Details (             Employee_IdNo      ,            sl_no     ,                    Join_Date           , Join_DateTime,                    Releave_date             ,                                                Releave_DateTime            ,                      Reason                  ) " & _
                                                        "       Values                          ( " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(SNo)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "',   @JoinDate  , '" & Trim(.Rows(i).Cells(2).Value) & "', " & IIf(IsDate(.Rows(i).Cells(2).Value) = True, "@ReleaveDate", "Null") & "     ,  '" & Trim(.Rows(i).Cells(3).Value) & "'     ) "
                                cmd.ExecuteNonQuery()

                            End If

                        End If

                    End If

                Next

            End With

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "EMPLOYEE"

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If
            Else
                move_record(lbl_IdNo.Text)
            End If

        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_payRoll_employee_head") > 0 Then
                MessageBox.Show("Duplicate Employee Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    'Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
    '    btn_Open_Click(sender, e)
    'End Sub

    'Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        btn_Open_Click(sender, e)
    '    End If
    'End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_SchemeSalarydetails.Name Or ActiveControl.Name = dgv_Releavedetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            'If ActiveControl.Name = dgv_SchemeSalarydetails.Name Then
            '    dgv1 = dgv_SchemeSalarydetails
            'ElseIf dgv_SchemeSalarydetails.IsCurrentRowDirty = True Then
            '    dgv1 = dgv_SchemeSalarydetails
            'ElseIf ActiveControl.Name = dgv_Releavedetails.Name Then
            '    dgv1 = dgv_Releavedetails
            'ElseIf dgv_Releavedetails.IsCurrentRowDirty = True Then
            '    dgv1 = dgv_Releavedetails
            'ElseIf pnl_Back.Enabled = True Then
            '    dgv1 = dgv_Releavedetails
            'ElseIf pnl_Back.Enabled = True Then
            '    dgv1 = dgv_SchemeSalarydetails
            'End If

            If ActiveControl.Name = dgv_SchemeSalarydetails.Name Then
                dgv1 = dgv_SchemeSalarydetails

            ElseIf dgv_SchemeSalarydetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_SchemeSalarydetails

            ElseIf ActiveControl.Name = dgv_Releavedetails.Name Then
                dgv1 = dgv_Releavedetails

            ElseIf dgv_Releavedetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_Releavedetails


            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_SchemeSalarydetails.Name.ToString)) Then
                dgv1 = dgv_SchemeSalarydetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_Releavedetails.Name.ToString)) Then
                dgv1 = dgv_Releavedetails


            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_SchemeSalarydetails.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    Tab_Main.SelectTab(7)
                                    txt_Document1.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            ElseIf .CurrentCell.ColumnIndex = 3 Then
                                'If Trim(.Rows(.CurrentRow.Index).Cells(3).Value) = "" Then
                                '    Tab_Main.SelectTab(7)
                                '    txt_Document1.Focus()
                                'End If
                                '.CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

                                If Val(.Rows(.CurrentRow.Index).Cells(3).Value) = 0 Then
                                    Tab_Main.SelectTab(7)
                                    txt_Document1.Focus()
                                End If
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)



                                'ElseIf .CurrentCell.ColumnIndex = 5 Then

                                '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(13)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    Tab_Main.SelectTab(5)
                                    txt_RelationName1.Focus()
                                    'dgv_ReleationDetails.CurrentCell = dgv_ReleationDetails.Rows(0).Cells(1)

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                                End If


                            ElseIf .CurrentCell.ColumnIndex = 5 Then

                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)

                            ElseIf .CurrentCell.ColumnIndex = 13 Then

                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_Releavedetails.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    Tab_Main.SelectTab(5)
                                    txt_RelationName1.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    Tab_Main.SelectTab(4)
                                    txt_Address1.Focus()
                                    'dgv_ReleationDetails.CurrentCell = dgv_ReleationDetails.Rows(0).Cells(1)

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                                End If



                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    End If
                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If
    End Function

    Private Sub dgv_SchemesalaryDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SchemeSalarydetails.CellEndEdit
        dgv_SchemesalaryDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_SchemesalaryDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SchemeSalarydetails.CellLeave
        With dgv_SchemeSalarydetails
            If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 10 Or .CurrentCell.ColumnIndex = 11 Or .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_SchemesalaryDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_SchemeSalarydetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_SchemeSalarydetails
                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

            End With
        End If

    End Sub

    Private Sub dgv_SchemeSalaryDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_SchemeSalarydetails.LostFocus
        On Error Resume Next
        ' dgv_SchemeSalarydetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_SchemesalaryDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_SchemeSalarydetails.RowsAdded
        Dim n As Integer

        With dgv_SchemeSalarydetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub



    Private Sub dgv_Releavedetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Releavedetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_SchemeSalarydetails
                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

            End With
        End If

    End Sub

    Private Sub dgv_Releavedetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Releavedetails.LostFocus
        On Error Resume Next
        ' dgv_SchemeSalarydetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Releavedetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Releavedetails.RowsAdded
        Dim n As Integer

        With dgv_Releavedetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Category_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Category.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
        cbo_Category.Tag = cbo_Category.Text
    End Sub

    Private Sub cbo_Category_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Category.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Category, txt_CardNo, cbo_Area, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
    End Sub

    Private Sub cbo_Category_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Category.KeyPress
        Dim Sal_type As String = ""
        Dim cat_IDNo As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Category, cbo_Area, "PayRoll_Category_Head", "Category_Name", "", "(Category_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Category.Tag)) <> Trim(UCase(cbo_Category.Text)) Then
                cat_IDNo = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)
                Sal_type = Common_Procedures.get_FieldValue(con, "PayRoll_Category_Head", "Monthly_Shift", "(Category_IdNo = " & Str(Val(cat_IDNo)) & ")")
                cbo_ShiftDay.Text = Sal_type
                If Trim(UCase(cbo_ShiftDay.Text)) = "MONTH" Then
                    cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, 2)
                Else
                    cbo_PaymentType.Text = Common_Procedures.Salary_PaymentType_IdNoToName(con, 1)
                End If
            End If
        End If

    End Sub

    Private Sub cbo_Category_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Category.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Category_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Category.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub btn_Photo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Photo.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox1.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub btn_AddPhoto1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AddPhoto1.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox2.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub btn_AddPhoto2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AddPhoto2.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox3.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_AddPhoto3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AddPhoto3.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox4.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub btn_AddPhoto4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AddPhoto4.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox5.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub cbo_ShiftDay_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ShiftDay.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ShiftDay, msk_JoinDate, cbo_PaymentType, "", "", "", "")


    End Sub

    Private Sub cbo_ShiftDay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ShiftDay.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ShiftDay, cbo_PaymentType, "", "", "", "")

    End Sub
    Private Sub cbo_Sex_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sex.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sex, txt_Age, txt_Height, "", "", "", "")
    End Sub

    Private Sub cbo_Sex_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sex.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sex, txt_Height, "", "", "", "")
    End Sub

    Private Sub cbo_MarriedStatus_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MarriedStatus.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MarriedStatus, txt_FatherHusband, txt_NoOfChildren, "", "", "", "")
    End Sub

    Private Sub cbo_MarriedStatus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MarriedStatus.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MarriedStatus, txt_NoOfChildren, "", "", "", "")
    End Sub

    Private Sub dgtxt_SchemesalaryDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_SchemesalaryDetails.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub dgtxt_SchemeSalaryDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_SchemesalaryDetails.KeyPress

        If Val(dgv_SchemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 3 Or Val(dgv_SchemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 4 Or Val(dgv_SchemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 5 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        End If

    End Sub

    Private Sub dgtxt_SchemeSalaryDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_SchemesalaryDetails.Enter
        dgv_ActCtrlName = dgv_SchemeSalarydetails.Name
        dgv_SchemeSalarydetails.EditingControl.BackColor = Color.Lime
        dgv_SchemeSalarydetails.EditingControl.ForeColor = Color.Blue
        dgtxt_SchemesalaryDetails.SelectAll()
    End Sub

    Private Sub dgtxt_ReleaveDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_ReleaveDetails.KeyPress

        'If Val(dgv_SchemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 3 Or Val(dgv_SchemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 4 Or Val(dgv_SchemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 5 Then
        '    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'End If


    End Sub

    Private Sub dgtxt_ReleaveDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_ReleaveDetails.Enter
        dgv_ActCtrlName = dgv_Releavedetails.Name
        dgv_Releavedetails.EditingControl.BackColor = Color.Lime
        dgv_Releavedetails.EditingControl.ForeColor = Color.Blue
        dgtxt_ReleaveDetails.SelectAll()
    End Sub
    'Private Sub dgtxt_NonSchemeSalaryDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_NonSchemesalaryDetails.KeyPress

    '    If Val(dgv_NonschemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 3 Or Val(dgv_NonschemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 4 Or Val(dgv_NonschemeSalarydetails.CurrentCell.ColumnIndex.ToString) = 5 Then
    '        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    '    End If


    'End Sub

    'Private Sub dgtxt_NonSchemeSalaryDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_NonSchemesalaryDetails.Enter
    '    dgv_NonschemeSalarydetails.EditingControl.BackColor = Color.Lime
    '    dgv_NonschemeSalarydetails.EditingControl.ForeColor = Color.Blue
    '    dgtxt_NonSchemesalaryDetails.SelectAll()
    'End Sub
    'Private Sub dgv_ContacDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_ContactDetails.EditingControlShowing
    '    dgtxt_ContactDetails = CType(dgv_ContactDetails.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub
    'Private Sub dgv_OfficalDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_OfficalDetails.EditingControlShowing
    '    dgtxt_OfficalDetails = CType(dgv_OfficalDetails.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub

    'Private Sub dgv_PersonalDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PersonalDetails.EditingControlShowing
    '    dgtxt_PersonalDetails = CType(dgv_PersonalDetails.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub
    'Private Sub dgv_ReleaveDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_ReleaveDetails.EditingControlShowing
    '    dgtxt_ReleaveDetails = CType(dgv_ReleaveDetails.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub

    'Private Sub dgv_ReleationDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_ReleationDetails.EditingControlShowing
    '    dgtxt_ReleationDetails = CType(dgv_ReleationDetails.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub
    Private Sub dgv_SchemeSalaryDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_SchemeSalarydetails.EditingControlShowing
        dgtxt_SchemesalaryDetails = CType(dgv_SchemeSalarydetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Releavedetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Releavedetails.EditingControlShowing
        dgtxt_ReleaveDetails = CType(dgv_Releavedetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub txt_Community_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Community.KeyDown
        'If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        'If e.KeyCode = 40 Then
        '    Tab_Main.SelectTab(3)

        'End If
    End Sub


    Private Sub txt_Community_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Community.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    Tab_Main.SelectTab(3)
        'End If
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Height_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Height.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_NoOfChildren_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfChildren.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Age_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Age.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_MobileNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MobileNo.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            Tab_Main.SelectTab(4)

        End If
    End Sub

    Private Sub txt_MobileNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MobileNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Tab_Main.SelectTab(4)
        End If
    End Sub

    Private Sub msk_DateOfBirth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DateOfBirth.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            Tab_Main.SelectTab(1)
            Cbo_ESIPF_Group.Focus()
        End If
    End Sub

    Private Sub msk_ReleaveDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_ReleaveDate.KeyDown
        If e.KeyCode = 38 Then

            Tab_Main.SelectTab(3)
            txt_Address1.Focus()

        End If
        If e.KeyCode = 40 Then
            dgv_Releavedetails.CurrentCell = dgv_Releavedetails.Rows(0).Cells(1)
            dgv_Releavedetails.Focus()

        End If
    End Sub

    Private Sub chk_ReleaveDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_ReleaveDate.CheckedChanged
        If chk_ReleaveDate.Checked = True Then
            msk_ReleaveDate.Enabled = True
            txt_Reason.Enabled = True
        Else
            msk_ReleaveDate.Enabled = False
            txt_Reason.Enabled = False
            msk_ReleaveDate.Text = ""
            txt_Reason.Text = ""
        End If
    End Sub


    Private Sub chk_ReleaveDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_ReleaveDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If chk_ReleaveDate.Checked = True And msk_ReleaveDate.Visible And msk_ReleaveDate.Enabled Then
                msk_ReleaveDate.Focus()
            Else
                Tab_Main.SelectTab(5)
                txt_RelationName1.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Reason_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Reason.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            Tab_Main.SelectTab(5)
            txt_RelationName1.Focus()
        End If
    End Sub

    Private Sub txt_Reason_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Reason.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Tab_Main.SelectTab(5)
            txt_RelationName1.Focus()
        End If
    End Sub

    Private Sub txt_opamt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_opAmt.KeyDown

    End Sub

    Private Sub txt_opamt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_opAmt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub msk_JoinDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_JoinDate.KeyDown
        If e.KeyCode = 40 Then
            cbo_WeekOff.Focus()
        End If
        If e.KeyCode = 38 Then
            cbo_Company.Focus()

        End If
    End Sub

    Private Sub txt_EsiNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EsiNo.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub


    Private Sub txt_PfNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PfNo.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_WekkOffCredit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_WekkOffCredit.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            'Tab_Main.SelectTab(1)
            'msk_DateOfBirth.Focus()
        End If
    End Sub

    Private Sub txt_WekkOffCredit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WekkOffCredit.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            Tab_Main.SelectTab(1)
            msk_DateOfBirth.Focus()
        End If
    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            Call btn_Open_Click(sender, e)
        End If

    End Sub
    Private Sub btn_Open_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Employee_IdNo from PayRoll_Employee_Head where Employee_Name = '" & Trim(cbo_Open.Text) & "'", con)
        da.Fill(dt)

        movid = 0
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movid = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        dt.Dispose()
        da.Dispose()

        If movid <> 0 Then
            move_record(movid)
        Else
            new_record()
        End If

        btn_CloseOpen_Click(sender, e)

    End Sub
    Private Sub btn_CloseOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        ' Me.Height = 269
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub cbo_PaymentType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaymentType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")
    End Sub

    Private Sub cbo_PaymentType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentType, cbo_ShiftDay, cbo_Department, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")


    End Sub

    Private Sub cbo_PaymentType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentType, cbo_Department, "PayRoll_Salary_Payment_Type_Head", "Salary_Payment_Type_Name", "", "(Salary_Payment_Type_IdNo = 0)")

    End Sub

    Private Sub cbo_PaymentType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentType.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New PayRoll_Salary_PaymentType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PaymentType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_WeekOff_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeekOff.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WeekOff, txt_Designation, txt_Dispensary, "", "", "", "")

    End Sub

    Private Sub cbo_Weekoff_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeekOff.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WeekOff, txt_Dispensary, "", "", "", "")

    End Sub

    Private Sub cbo_Area_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Area.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "area_head", "area_name", "", "(area_idno = 0)")
    End Sub

    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Area, cbo_Category, cbo_Company, "area_head", "area_name", "", "(area_idno = 0)")
    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Area.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Area, cbo_Company, "area_head", "area_name", "", "(area_idno = 0)")

    End Sub

    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Area.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Department_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Department.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Department_Head", "Department_Name", "", "(Department_IdNo = 0)")
    End Sub

    Private Sub cbo_Department_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Department.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Department, cbo_PaymentType, txt_Designation, "Department_Head", "Department_Name", "", "(Department_IdNo = 0)")

    End Sub

    Private Sub cbo_Department_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Department.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Department, txt_Designation, "Department_Head", "Department_Name", "", "(Department_IdNo = 0)")

    End Sub


    Private Sub cbo_Department_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Department.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Department_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Department.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Company_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.GotFocus
        Dim CompCondt As String = ""

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "ACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Company_Head", "Company_ShortName", CompCondt, "(Company_IdNo = 0)")

    End Sub

    Private Sub cbo_company1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Company.KeyDown
        Dim CompCondt As String = ""

        vcbo_KeyDwnVal = e.KeyValue

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "ACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Company, cbo_Area, Nothing, "Company_Head", "Company_ShortName", CompCondt, "(Company_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_Company.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            Tab_Main.SelectTab(0)
            msk_JoinDate.Focus()

        End If
    End Sub

    Private Sub cbo_company1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company.KeyPress
        Dim CompCondt As String = ""

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "ACCOUNT" Then
            CompCondt = "(Company_Type <> 'UNACCOUNT')"
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Company, Nothing, "Company_Head", "Company_ShortName", CompCondt, "(Company_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            Tab_Main.SelectTab(0)
            msk_JoinDate.Focus()
        End If
    End Sub

    Private Sub txt_RelationShip4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Relationship4.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            Tab_Main.SelectTab(6)
            dgv_SchemeSalarydetails.Focus()
            dgv_SchemeSalarydetails.CurrentCell = dgv_SchemeSalarydetails.Rows(0).Cells(1)

        End If
    End Sub

    Private Sub txt_RelationShip4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Relationship4.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Tab_Main.SelectTab(6)
            dgv_SchemeSalarydetails.Focus()
            dgv_SchemeSalarydetails.CurrentCell = dgv_SchemeSalarydetails.Rows(0).Cells(1)
        End If
    End Sub



    Private Sub btn_AddPhoto9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddPhoto9.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox9.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_AddPhoto8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddPhoto8.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox8.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_AddPhoto7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddPhoto7.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox7.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btn_AddPhotho6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddPhotho6.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox6.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub txt_Certificate4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Certificate4.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If

        End If
    End Sub

    Private Sub txt_Certificate4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Certificate4.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub txt_Address1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address1.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            Tab_Main.SelectTab(2)
            cbo_MotherTongue.Focus()
        End If
    End Sub

    Private Sub txt_Document1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Document1.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            Tab_Main.SelectTab(6)
            dgv_SchemeSalarydetails.Focus()
            dgv_SchemeSalarydetails.CurrentCell = dgv_SchemeSalarydetails.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub txt_RelationName1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RelationName1.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            Tab_Main.SelectTab(4)
            msk_ReleaveDate.Focus()
        End If
    End Sub


    Private Sub chk_Pfsts_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Pfsts.CheckedChanged
        If chk_Pfsts.Checked = True Then
            msk_pfJoinDate.Enabled = True
            msk_pfLeaveDate.Enabled = True
            txt_PfNo.Enabled = True
        ElseIf chk_PfSalary.Checked = False Then
            msk_pfJoinDate.Enabled = False
            msk_pfLeaveDate.Enabled = False
            txt_PfNo.Enabled = False
            msk_ReleaveDate.Text = ""
            msk_pfLeaveDate.Text = ""
            txt_PfNo.Text = ""
        End If
    End Sub

    Private Sub chk_PfSalary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_PfSalary.CheckedChanged
        If chk_PfSalary.Checked = True Then
            msk_pfJoinDate.Enabled = True
            msk_pfLeaveDate.Enabled = True
            txt_PfNo.Enabled = True
        ElseIf chk_Pfsts.Checked = False Then
            msk_pfJoinDate.Enabled = False
            msk_pfLeaveDate.Enabled = False
            txt_PfNo.Enabled = False
            msk_ReleaveDate.Text = ""
            msk_pfLeaveDate.Text = ""
            txt_PfNo.Text = ""
        End If
    End Sub

    Private Sub chk_Esists_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Esists.CheckedChanged
        If chk_Esists.Checked = True Then
            msk_EsiJoinDate.Enabled = True
            msk_EsiLeaveDate.Enabled = True
            txt_EsiNo.Enabled = True
        ElseIf chk_EsiSalary.Checked = False Then
            msk_EsiJoinDate.Enabled = False
            msk_EsiLeaveDate.Enabled = False
            txt_EsiNo.Enabled = False
            msk_EsiJoinDate.Text = ""
            msk_EsiLeaveDate.Text = ""
            txt_EsiNo.Text = ""
        End If
    End Sub

    Private Sub chk_EsiSalary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_EsiSalary.CheckedChanged
        If chk_EsiSalary.Checked = True Then
            msk_EsiJoinDate.Enabled = True
            msk_EsiLeaveDate.Enabled = True
            txt_EsiNo.Enabled = True
        ElseIf chk_Esists.Checked = False Then
            msk_EsiJoinDate.Enabled = False
            msk_EsiLeaveDate.Enabled = False
            txt_EsiNo.Enabled = False
            msk_EsiJoinDate.Text = ""
            msk_EsiLeaveDate.Text = ""
            txt_EsiNo.Text = ""
        End If
    End Sub

    Private Sub btn_Clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Clear.Click
        PictureBox1.BackgroundImage = Nothing
    End Sub

    Private Sub btn_PhotoClear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PhotoClear1.Click
        PictureBox2.BackgroundImage = Nothing
    End Sub

    Private Sub btn_PhotoClear2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PhotoClear2.Click
        PictureBox3.BackgroundImage = Nothing
    End Sub

    Private Sub btn_PhotoClear3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PhotoClear3.Click
        PictureBox4.BackgroundImage = Nothing
    End Sub

    Private Sub btn_PhotoClear4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PhotoClear4.Click
        PictureBox5.BackgroundImage = Nothing
    End Sub

    Private Sub cbo_Category_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Category.SelectedIndexChanged

    End Sub

    Private Sub txt_Wages_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Wages.KeyDown
        ' If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            Tab_Main.SelectTab(1)
            chk_Esists.Focus()
        End If
    End Sub

    Private Sub txt_Wages_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Wages.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Tab_Main.SelectTab(1)
            chk_Esists.Focus()
        End If
    End Sub

    Private Sub chk_Esists_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_Esists.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            If txt_EsiNo.Visible And txt_EsiNo.Enabled Then
                txt_EsiNo.Focus()
            Else
                chk_Pfsts.Focus()
            End If
        End If
    End Sub

    Private Sub chk_Esists_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Esists.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_EsiNo.Visible And txt_EsiNo.Enabled Then
                txt_EsiNo.Focus()
            Else
                chk_Pfsts.Focus()
            End If
        End If
    End Sub

    Private Sub chk_Pfsts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_Pfsts.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            If txt_PfNo.Visible And txt_PfNo.Enabled Then
                txt_PfNo.Focus()
            Else
                Tab_Main.SelectTab(2)
                msk_DateOfBirth.Focus()
            End If
        End If
    End Sub

    Private Sub chk_Pfsts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Pfsts.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_PfNo.Visible And txt_PfNo.Enabled Then
                txt_PfNo.Focus()
            Else
                Tab_Main.SelectTab(2)
                msk_DateOfBirth.Focus()
            End If
        End If
    End Sub

    Private Sub msk_pfLeaveDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_pfLeaveDate.KeyDown
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            Cbo_ESIPF_Group.Focus()
        End If
        '
        'If e.KeyCode = 40 Then

        '        Tab_Main.SelectTab(2)
        '        msk_DateOfBirth.Focus()

        'End If
    End Sub

    Private Sub msk_pfLeaveDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_pfLeaveDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Cbo_ESIPF_Group.Focus()
            'Tab_Main.SelectTab(2)
            'msk_DateOfBirth.Focus()

        End If
    End Sub

    Private Sub msk_ReleaveDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_ReleaveDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Releavedetails.Focus()
            dgv_Releavedetails.CurrentCell = dgv_Releavedetails.Rows(0).Cells(1)
        End If
    End Sub
    Private Sub Get_Columns_Head_Name()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 * from PayRoll_Settings order by Auto_SlNo", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    dgv_SchemeSalarydetails.Columns(9).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption1").ToString))
                    dgv_SchemeSalarydetails.Columns(10).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption2").ToString))
                    dgv_SchemeSalarydetails.Columns(11).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption3").ToString))
                    dgv_SchemeSalarydetails.Columns(12).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption4").ToString))
                    dgv_SchemeSalarydetails.Columns(14).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption5").ToString))
                    dgv_SchemeSalarydetails.Columns(15).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption6").ToString))
                    dgv_SchemeSalarydetails.Columns(16).HeaderText = UCase(Trim(dt.Rows(0).Item("Add_Caption7").ToString))
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Private Sub cbo_Category_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Category.TextChanged
        Dim CAT_ID As Integer = 0


        Try

            CAT_ID = Val(Common_Procedures.Category_NameToIdNo(con, Trim(cbo_Category.Text)))

            cbo_ShiftDay.Text = Trim(Common_Procedures.get_FieldValue(con, "PayRoll_Category_Head", "Monthly_Shift", "Category_IdNo = " & CAT_ID))

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btn_EnlargePhoto6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnlargePhoto6.Click
        Dim f As New Enlarge_Image(PictureBox6.BackgroundImage)
        f.MdiParent = MDIParent1
        f.Show()
    End Sub

    Private Sub btn_EnlargePhoto7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnlargePhoto7.Click
        Dim f As New Enlarge_Image(PictureBox7.BackgroundImage)
        f.MdiParent = MDIParent1
        f.Show()
    End Sub

    Private Sub btn_EnlargePhoto8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnlargePhoto8.Click
        Dim f As New Enlarge_Image(PictureBox8.BackgroundImage)
        f.MdiParent = MDIParent1
        f.Show()
    End Sub

    Private Sub btn_EnlargePhoto9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnlargePhoto9.Click
        Dim f As New Enlarge_Image(PictureBox9.BackgroundImage)
        f.MdiParent = MDIParent1
        f.Show()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim MAX_PAGE_COUNT As Integer = 3
        Dim PageNumber As Integer


        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Head a where a.Employee_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        PageNumber = 1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub


    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_PurchaseNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select  * from PayRoll_Employee_Head a where a.Employee_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        prn_PageNo = prn_PageNo + 1
        If prn_PageNo = 1 Then
            Printing_Format2(e)
            e.HasMorePages = True
        Else
            Printing_Format4(e)
            e.HasMorePages = False
        End If

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim pfont As Font
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim LedNmAr(10) As String
        Dim CurY As Single
        Dim TMargin As Single
        Dim LMargin As Single
        Dim RMargin As Single
        Dim BMargin As Single
        Dim ps As Printing.PaperSize

        Dim TxtHgt As Single
        Dim PrintWidth As Single
        Dim PageWidth As Single
        Dim PrintHeight As Single
        Dim PageHeight As Single
        Dim NoofItems_PerPage As Integer
        Dim LnAr() As Single
        Dim ClArr() As Single
        Dim PageNumber As Integer = 0
        Dim MAX_PAGE_COUNT As Integer = 3

        ' PageNumber = PageNumber + 1

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pfont = New Font("Calibri", 12, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        TxtHgt = e.Graphics.MeasureString("A", pfont).Height
        TxtHgt = 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 13

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr(0) = 0
        ClArr(1) = 150 : ClArr(2) = 145 : ClArr(3) = 145 : ClArr(4) = 125
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        CurY = TMargin





        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BIO-DATA", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt

        W2 = 300


        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Employee_Name").ToString, LMargin + W2 + 10, CurY, 0, 0, pfont)

        ' e.Graphics.DrawImage(DirectCast(Global.Payroll.My.Resources.Resources.SUBI_LOGO1, Drawing.Image), LMargin + 24, CurY, 90, 120)

        ' e.Graphics.DrawImage(DirectCast(prn_HdDt.Rows(0).Item("Employee_Image"), Drawing.Image), LMargin + 24, CurY, 90, 120)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DATE OF BIRTH", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Birth").ToString, LMargin + W2 + 10, CurY, 0, 0, pfont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "BIRTH PLACE/DISTRICT", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Village").ToString & IIf(prn_HdDt.Rows(0).Item("District").ToString <> "", "/" & prn_HdDt.Rows(0).Item("District").ToString, ""), LMargin + W2 + 10, CurY, 0, 0, pfont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "RELIGION/CASTE", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Community").ToString, LMargin + W2 + 10, CurY, 0, 0, pfont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "MALE/FEMALE", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sex").ToString, LMargin + W2 + 10, CurY, 0, 0, pfont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "MARITAL STATUS", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Marital_Status").ToString, LMargin + W2 + 10, CurY, 0, 0, pfont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "FATHER NAME", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Father_Husband").ToString, LMargin + W2 + 10, CurY, 0, 0, pfont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "FATHER WORK AND ANNUAL INCOME", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Birth").ToString, LMargin + W2 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "MOTHER NAME", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Birth").ToString, LMargin + W2 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "FAMILY DETAILS :", LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + 20, CurY, PageWidth - 20, CurY)
        LnAr(2) = CurY

        ' e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS", LMargin, CurY, 2, ClArr(1), pfont)
        Common_Procedures.Print_To_PrintDocument(e, "BROTHERS", LMargin + ClArr(1) + ClArr(2) - 70, CurY, 2, ClArr(3), pfont)
        Common_Procedures.Print_To_PrintDocument(e, "SISTERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 85, CurY, 2, ClArr(5), pfont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 20, CurY)
        LnAr(3) = CurY

        Common_Procedures.Print_To_PrintDocument(e, "ELDER", LMargin + ClArr(1), CurY, 2, ClArr(2), pfont)
        Common_Procedures.Print_To_PrintDocument(e, "YOUNGER", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pfont)



        Common_Procedures.Print_To_PrintDocument(e, "ELDER", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pfont)
        Common_Procedures.Print_To_PrintDocument(e, "YOUNGER", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pfont)



        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY)
        LnAr(4) = CurY



        ' Common_Procedures.Print_To_PrintDocument(e, "SISTERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pfont)
        'CurY = CurY + TxtHgt + 5
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY)
        'LnAr(3) = CurY


        'Common_Procedures.Print_To_PrintDocument(e, "ELDER", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pfont)
        'Common_Procedures.Print_To_PrintDocument(e, "YOUNGER", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pfont)

        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pfont)
        'Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pfont)
        'Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pfont)
        'Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, 2, ClArr(7), pfont)
        'Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 2, ClArr(8), pfont)

        e.Graphics.DrawLine(Pens.Black, LMargin + 20, CurY, PageWidth - 20, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin, CurY, 2, ClArr(1), pfont)
        CurY = CurY + TxtHgt + 30
        e.Graphics.DrawLine(Pens.Black, LMargin + 20, CurY, PageWidth - 20, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "AGE", LMargin, CurY, 2, ClArr(1), pfont)
        CurY = CurY + TxtHgt + 30
        e.Graphics.DrawLine(Pens.Black, LMargin + 20, CurY, PageWidth - 20, CurY)
        LnAr(7) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "WORK", LMargin, CurY, 2, ClArr(1), pfont)
        CurY = CurY + TxtHgt + 30
        e.Graphics.DrawLine(Pens.Black, LMargin + 20, CurY, PageWidth - 20, CurY)
        LnAr(8) = CurY

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + 20, LnAr(2), LMargin + 20, LnAr(8))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), LnAr(2), LMargin + ClArr(1), LnAr(8))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), LnAr(3), LMargin + ClArr(1) + ClArr(2), LnAr(8))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(2), LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(8))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(8))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 20, LnAr(2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 20, LnAr(8))

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "ADDRESS :", LMargin + W2 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + 10, CurY, PageWidth - 20, CurY)
        LnAr(9) = CurY
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CURRENT ADDRESS", LMargin, CurY, 2, ClArr(1) + 38, pfont)
        Common_Procedures.Print_To_PrintDocument(e, "PERMANENT ADDRESS", LMargin + ClArr(1) + ClArr(2) - 55, CurY, 2, ClArr(3), pfont)
        Common_Procedures.Print_To_PrintDocument(e, "EMERGENCY ADDRESS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 75, CurY, 2, ClArr(5), pfont)



        CurY = CurY + TxtHgt + 25
        e.Graphics.DrawLine(Pens.Black, LMargin + 10, CurY, PageWidth - 20, CurY)
        LnAr(10) = CurY
        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        CurY = CurY + 5
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("address1").ToString, LMargin + 15, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("address2").ToString, LMargin + 15, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("address3").ToString, LMargin + 15, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Village").ToString, LMargin + 15, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Taulk").ToString, LMargin + 15, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("District").ToString, LMargin + 15, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Phone No. : " & prn_HdDt.Rows(0).Item("Phone_No").ToString, LMargin + 15, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Mobile No. : " & prn_HdDt.Rows(0).Item("Mobile_No").ToString, LMargin + 15, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + 10, CurY, PageWidth - 20, CurY)
        LnAr(11) = CurY

        CurY = CurY + TxtHgt + 60
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + 10, LnAr(9), LMargin + 10, LnAr(11))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + 48, LnAr(9), LMargin + ClArr(1) + 48, LnAr(11))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(9), LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(11))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 20, LnAr(9), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 20, LnAr(11))

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, LnAr(12))

        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, LnAr(12))

        'If PageNumber = MAX_PAGE_COUNT Then

        '    e.HasMorePages = False
        '    PageNumber = 1


        'Else
        '    e.HasMorePages = True
        '    PageNumber += 1
        '    Printing_Format4(e)

        'End If



    End Sub


    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim pfont As Font
        Dim p3Font As Font
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim LedNmAr(10) As String
        Dim CurY As Single
        Dim TMargin As Single
        Dim LMargin As Single
        Dim RMargin As Single
        Dim BMargin As Single
        Dim ps As Printing.PaperSize
        Dim TxtHgt As Single
        Dim PrintWidth As Single
        Dim PageWidth As Single
        Dim PrintHeight As Single
        Dim PageHeight As Single
        Dim NoofItems_PerPage As Integer
        Dim LnAr() As Single
        Dim ClArr() As Single

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 65
            .Right = 50
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pfont = New Font("Calibri", 12, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        TxtHgt = e.Graphics.MeasureString("A", pfont).Height
        TxtHgt = 18.75 ' 20  ' e.Graphics.MeasureString("A", pFont).Height

        NoofItems_PerPage = 13

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr(0) = 0
        ClArr(1) = 150 : ClArr(2) = 145 : ClArr(3) = 145 : ClArr(4) = 125
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        CurY = TMargin


        ' p1Font = New Font("Calibri", 12, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "BIO-DATA", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        W2 = 300


        p1Font = New Font("Calibri", 11, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "FRESHER/EXPERIENCED", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e,  prn_HdDt.Rows(0).Item("Mobile_No").ToString, LMargin + 15, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "PREVIOUS SALARY", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "EDUCATION QUALIFICATION", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Qualification").ToString, LMargin + W2 + 15, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "COMPLETED YEAR", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "MOTHER LANGUAGE AND OTHERS", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Qualification").ToString, LMargin + W2 + 15, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "OTHER ACTIVITIES", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "HEIGHT", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Height").ToString, LMargin + W2 + 15, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "ABOUT FOOD", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "ENTERTAINMENT", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "MILL AROUND RELATIONS AND FRIENDS", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)



        'CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "MOTHER NAME", LMargin + W2, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "ANY RELATION WORKED IN HERE? ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "YOUR REFERENCED NAME, ADDRESS", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 70

        Common_Procedures.Print_To_PrintDocument(e, "I AGREE GIVEN ALL INFORMATION ARE CORRECT. ", LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "PLACE :", LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "DATE :", LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt + 70

        Common_Procedures.Print_To_PrintDocument(e, "FATHER / GAURDIAN SIGNATURE", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "SIGNATURE", PageWidth - 110, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt + 40

        p3Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "MILL TRAINING PERIOD", LMargin, CurY - TxtHgt, 2, PrintWidth, p3Font)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "RESPECTED SIR,", LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 20
        Common_Procedures.Print_To_PrintDocument(e, "I WISH TO JOIN WITH YOUR ALLOCATED TRAINING PERIOD                      /       ", LMargin + 10, CurY, 0, 0, p1Font)



        CurY = CurY + TxtHgt + 100
        Common_Procedures.Print_To_PrintDocument(e, "FATHER / GAURDIAN SIGNATURE", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "AGENT", PageWidth - 400, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "TRAINER SIGNATURE", PageWidth - 170, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt + 80
        Common_Procedures.Print_To_PrintDocument(e, "HOSTEL WARDEN", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "MANAGER", PageWidth - 400, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "MANAGING DIRECTOR", PageWidth - 170, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(12) = CurY



        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, LnAr(12))

        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, LnAr(12))


        'e.HasMorePages = False

    End Sub

    Private Sub Cbo_BankName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_BankName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_BankName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_BankName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_BankName, txt_BankAcNo, txt_BankCode, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub Cbo_BankName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_BankName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_BankName, txt_BankCode, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub Cbo_BankName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_BankName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "BANK_CREATION"
            Dim f As New Ledger_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_BankName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub TokenNo_Generation(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vComp_id As Integer = 0
        Dim vDept_id As Integer = 0
        Dim vDept_CD As String = ""
        Dim vCatg_ID As Integer = 0
        Dim vMaxNo As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
            txt_CardNo.Text = lbl_IdNo.Text

            'vComp_id = Common_Procedures.Company_ShortNameToIdNo(con, cbo_Company.Text)
            'vDept_id = Common_Procedures.Department_NameToIdNo(con, cbo_Department.Text)
            'vDept_CD = Common_Procedures.get_FieldValue(con, "Department_Head", "Department_Code", "(Department_IdNo = " & Str(Val(vDept_id)) & ")")

            'Da = New SqlClient.SqlDataAdapter("Select count(*) from PayRoll_Employee_Head where Company_idno = " & Str(Val(vComp_id)) & " and Department_idno = " & Str(Val(vDept_id)) & " and Employee_IdNo < " & Str(Val(lbl_IdNo.Text)), con)
            'If IsNothing(sqltr) = False Then
            '    Da.SelectCommand.Transaction = sqltr
            'End If
            'Da.Fill(Dt)

            'vMaxNo = 0
            'If Dt.Rows.Count > 0 Then
            '    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '        vMaxNo = Val(Dt.Rows(0)(0).ToString)
            '    End If
            'End If

            'Dt.Dispose()
            'Da.Dispose()

            'vMaxNo = vMaxNo + 1

            'txt_CardNo.Text = Trim(Val(vComp_id)) & Trim(Format(Val(vDept_CD), "00")) & Trim(Format(Val(vMaxNo), "000"))

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL

            vCatg_ID = Common_Procedures.Category_NameToIdNo(con, cbo_Category.Text)

            Da = New SqlClient.SqlDataAdapter("Select count(*) from PayRoll_Employee_Head where Employee_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Category_IdNo = " & Str(Val(vCatg_ID)), con)
            If IsNothing(sqltr) = False Then
                Da.SelectCommand.Transaction = sqltr
            End If
            Da.Fill(Dt)

            vMaxNo = 0
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    vMaxNo = Val(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Dispose()
            Da.Dispose()

            vMaxNo = vMaxNo + 1

            txt_CardNo.Text = Trim(Format(Val(vCatg_ID) * 1000)) + Val(vMaxNo)


        End If
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then
            cmd.Connection = con

            cmd.CommandText = "Update PayRoll_Employee_Head set Card_No = Employee_IdNo"
            cmd.ExecuteNonQuery()

        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_IdNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_IdNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            movenext_record()
        End If
    End Sub

    Private Sub Cbo_ESIPF_Group_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ESIPF_Group.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ESI_PF_Head", "ESI_PF_Group_Name", "", "(ESI_PF_Group_IdNo = 0)")
    End Sub

    Private Sub Cbo_ESIPF_Group_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_ESIPF_Group.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_ESIPF_Group, Nothing, Nothing, "ESI_PF_Head", "ESI_PF_Group_Name", "", "(ESI_PF_Group_IdNo = 0)")

        If (e.KeyValue = 38 And Cbo_ESIPF_Group.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            msk_pfLeaveDate.Focus()
        End If

        If e.KeyCode = 40 And Cbo_ESIPF_Group.DroppedDown = False Or (e.Control = True And e.KeyValue = 40) Then
            Tab_Main.SelectTab(2)
            msk_DateOfBirth.Focus()
        End If
    End Sub

    Private Sub Cbo_ESIPF_Group_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_ESIPF_Group.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_ESIPF_Group, Nothing, "ESI_PF_Head", "ESI_PF_Group_Name", "", "(ESI_PF_Group_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            Tab_Main.SelectTab(2)
            msk_DateOfBirth.Focus()
        End If
    End Sub

    Private Sub Cbo_ESIPF_Group_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_ESIPF_Group.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ESI_PF_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_ESIPF_Group.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub dgtxt_SchemesalaryDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_SchemesalaryDetails.TextChanged
        Try
            With dgv_SchemeSalarydetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_SchemesalaryDetails.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_SchemeSalarydetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_SchemeSalarydetails.CellValueChanged
        Try

            If NoCalc_Status = True Then Exit Sub

            With dgv_SchemeSalarydetails

                If .Visible Then

                    If .Rows.Count > 0 Then
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- SOUTHERN COTSPINNERS COIMBATORE LTD (COIMBATORE)  --SPINNING MILL
                            If e.ColumnIndex = 3 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Or e.ColumnIndex = 11 Or e.ColumnIndex = 12 Or e.ColumnIndex = 14 Or e.ColumnIndex = 15 Or e.ColumnIndex = 16 Then
                                .Rows(e.RowIndex).Cells(5).Value = Val(.Rows(e.RowIndex).Cells(3).Value) + Val(.Rows(e.RowIndex).Cells(6).Value) + Val(.Rows(e.RowIndex).Cells(7).Value) + Val(.Rows(e.RowIndex).Cells(8).Value) + Val(.Rows(e.RowIndex).Cells(9).Value) + Val(.Rows(e.RowIndex).Cells(10).Value) + Val(.Rows(e.RowIndex).Cells(11).Value) + Val(.Rows(e.RowIndex).Cells(12).Value) + Val(.Rows(e.RowIndex).Cells(14).Value) + Val(.Rows(e.RowIndex).Cells(15).Value)

                            End If

                        End If


                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cbo_ESIPF_Group_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ESIPF_Group.TextChanged
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ESIPF_ID As Integer = 0

        ESIPF_ID = Val(Common_Procedures.Esi_Pf_Group_NameToIdNo(con, Trim(Cbo_ESIPF_Group.Text)))
        Try
            da = New SqlClient.SqlDataAdapter("select ESI_AUDIT_STATUS , PF_AUDIT_STATUS , ESI_SALARY_STATUS , PF_SALARY_STATUS from ESI_PF_Head Where ESI_PF_Group_IdNo= " & Val(ESIPF_ID), con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0).Item("ESI_AUDIT_STATUS").ToString) = False Then
                    If Val(dt.Rows(0).Item("ESI_AUDIT_STATUS").ToString) <> 0 Then
                        chk_Esists.Checked = True
                    Else
                        chk_Esists.Checked = False
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("PF_AUDIT_STATUS").ToString) = False Then
                    If Val(dt.Rows(0).Item("PF_AUDIT_STATUS").ToString) <> 0 Then
                        chk_Pfsts.Checked = True
                    Else
                        chk_Pfsts.Checked = False
                    End If
                End If


                If IsDBNull(dt.Rows(0).Item("ESI_SALARY_STATUS").ToString) = False Then
                    If Val(dt.Rows(0).Item("ESI_SALARY_STATUS").ToString) <> 0 Then
                        chk_EsiSalary.Checked = True
                    Else
                        chk_EsiSalary.Checked = False
                    End If
                End If

                If IsDBNull(dt.Rows(0).Item("PF_SALARY_STATUS").ToString) = False Then
                    If Val(dt.Rows(0).Item("PF_SALARY_STATUS").ToString) <> 0 Then
                        chk_PfSalary.Checked = True
                    Else
                        chk_PfSalary.Checked = False
                    End If
                End If
            End If



            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_BankCode_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_BankCode.KeyDown
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to Save ?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txt_BankCode_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BankCode.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_MotherTongue_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_MotherTongue.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MotherTongue, txt_Community, Nothing, "", "", "", "")
  
        If e.KeyCode = 40 And cbo_MotherTongue.DroppedDown = False Or (e.Control = True And e.KeyValue = 40) Then
            txt_Address1.Focus()
            Tab_Main.SelectTab(3)
        End If
    End Sub

    Private Sub cbo_MotherTongue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MotherTongue.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MotherTongue, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            txt_Address1.Focus()
            Tab_Main.SelectTab(3)
        End If
    End Sub


End Class
